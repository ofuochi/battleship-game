using System;
using System.Collections.Generic;
using BattleshipGame.Enums;
using BattleshipGame.Services;

namespace BattleshipGame
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.Clear();

      Console.WriteLine($"What is Player1's name? (Press enter to choose default name)");
      var playerName = Console.ReadLine();
      playerName = playerName.Length > 0 ? playerName : "Player1";
      var player1 = SetupPlayer(playerName.Trim().ToUpper());

      Console.WriteLine();
      Console.WriteLine($"What is Player2's name? (Press enter to choose default name)");
      playerName = Console.ReadLine();
      playerName = playerName.Length > 0 ? playerName : "Player2";
      var player2 = SetupPlayer(playerName.Trim().ToUpper());

      Console.WriteLine();
      // Initialize the game
      StartGame(player1, player2);
    }

    private static Player SetupPlayer(string playerName)
    {
      playerName = playerName.ToUpper();
      Console.WriteLine("Do you want your ships to be randomly placed? y/n (Press enter, \"y\" or \"yes\" to affirm. Press anyother key otherwise...");
      var answer = Console.ReadLine().Trim().ToLower();
      var isRandomSetup = answer.Length == 0 || answer == "y" || answer == "yes";
      return isRandomSetup ? new Player(playerName) : new Player(playerName, SetupShips(new List<Ship>()));
    }

    private static void StartGame(Player player1, Player player2)
    {
      var game = new Game(player1, player2);
      var isPlayer1Turn = true;

      while (true)
      {
        Console.WriteLine();

        BoardRow row; BoardColumn column;
        Coordinate coordinate;
        string[] rowColumn;
        bool isValid;

        // Player 1's turn
        if (isPlayer1Turn)
        {
          Console.WriteLine($"{player1}, attack!");
          Console.WriteLine("Enter attack coordinate (e.g A,1 or a,1)");
          rowColumn = Console.ReadLine().Split(',');
          ValidateRowInput(rowColumn, out row, out isValid);
          if (!isValid)
          {
            Console.WriteLine();
            Console.WriteLine("Invalid row coordinate");
            isPlayer1Turn = true;
            continue;
          }

          ValidateColumnInput(rowColumn, out column, out isValid);
          if (!isValid)
          {
            Console.WriteLine();
            Console.WriteLine("Invalid column coordinate");
            isPlayer1Turn = true;
            continue;
          }

          coordinate = new Coordinate(row, column);
          if (IsCoordinateAlreadyAttacked(player1, coordinate))
          {
            Console.WriteLine();
            Console.WriteLine($"You already attacked {coordinate}");
            isPlayer1Turn = true;
            continue;
          }

          game.AttackPlayer2(coordinate);

          // Check if Player 1 won
          if (Object.ReferenceEquals(game.GetWinner(), player1))
          {
            Console.WriteLine();
            Console.WriteLine($"{player1} WINS!");
            break;
          }
          isPlayer1Turn = false;
        }

        Console.WriteLine();

        // Player 2's turn
        Console.WriteLine($"{player2}, attack!");
        Console.WriteLine("Enter attack coordinate (e.g A,1 or a,1)");
        rowColumn = Console.ReadLine().Split(',');

        ValidateRowInput(rowColumn, out row, out isValid);
        if (!isValid)
        {
          Console.WriteLine();
          Console.WriteLine("Invalid row coordinate");
          isPlayer1Turn = false;
          continue;
        }

        ValidateColumnInput(rowColumn, out column, out isValid);
        if (!isValid)
        {
          Console.WriteLine();
          Console.WriteLine("Invalid column coordinate");
          isPlayer1Turn = false;
          continue;
        }

        coordinate = new Coordinate(row, column);
        if (IsCoordinateAlreadyAttacked(player2, coordinate))
        {
          Console.WriteLine();
          Console.WriteLine($"You already attacked {coordinate}");
          isPlayer1Turn = false;
          continue;
        }

        game.AttackPlayer1(coordinate);

        // Check if Player 2 won
        if (Object.ReferenceEquals(game.GetWinner(), player2))
        {
          Console.WriteLine();
          Console.WriteLine($"{player2} WINS!");
          break;
        }

        isPlayer1Turn = true;
        Console.WriteLine();
      }
    }

    private static void ValidateRowInput(string[] rowColumn, out BoardRow row, out bool isValid)
    {
      isValid = Enum.TryParse(rowColumn[0].ToUpper(), out row);
      isValid &= Coordinate.IsRowValid(row);
    }

    private static void ValidateColumnInput(string[] rowColumn, out BoardColumn column, out bool isValid)
    {
      if (rowColumn[1].Length == 0)
      {
        isValid = false;
        column = BoardColumn.One;
        return;
      }
      if (!int.TryParse(rowColumn[1], out int columnNumber))
      {
        isValid = false;
        column = BoardColumn.One;
        return;
      }
      column = (BoardColumn)(columnNumber - 1);
      isValid = Coordinate.IsColumnValid(column);
    }

    private static bool IsCoordinateAlreadyAttacked(Player player1, Coordinate coordinate) =>
     player1.MapBoard.GetItem(coordinate) != Landmark.Empty;

    private static List<Ship> SetupShips(List<Ship> ships)
    {
      Console.WriteLine();
      var shipCoordinatesSet = new HashSet<string>();
      var shipTypes = new Stack<ShipType>(Ship.ShipTypes);
    MainLoop: while (shipTypes.Count > 0)
      {
        Console.WriteLine();
        Console.WriteLine("Inputted values are case insensitive.");
        var shipType = shipTypes.Pop();
        try
        {
          Console.WriteLine($"Enter START COORDINATE for your {shipType}.\n" +
          "E.G A,5,H - (Row A, Col 5, Horizontal)  or C,4,V - (Row C, Col 5, Vertical)).\n" +
          "If an orientation is not entered, it defaults to \"Horizontal\"");

          var positions = Console.ReadLine().ToLower().Split(',');
          if (positions.Length < 2)
          {
            Console.WriteLine();
            Console.WriteLine("Invalid input! You must at least enter a coordinate, e.g A,5 or A,5,H or A,5,V");
            shipTypes.Push(shipType);
            continue;
          }
          ValidateRowInput(positions, out BoardRow row, out bool isValid);

          if (!isValid)
          {
            Console.WriteLine();
            Console.WriteLine("Invalid row value!");
            shipTypes.Push(shipType);
            continue;
          }

          ValidateColumnInput(positions, out BoardColumn column, out isValid);
          if (!isValid)
          {
            Console.WriteLine();
            Console.WriteLine("Invalid column value!");
            shipTypes.Push(shipType);
            continue;
          }

          var isHorizontal = positions.Length > 2 ? positions[2] == "h" : true;
          var coordinate = new Coordinate(row, column);
          var canCoordinateFitShip = Ship.ShipPosition.TryGetPositionCoordinates(coordinate, shipType, isHorizontal, out var shipCoordinates);

          if (!canCoordinateFitShip)
          {
            Console.WriteLine();
            Console.WriteLine($"You cannot fit a {(isHorizontal ? "horizontal" : "vertical")} {shipType} in that position. Try another position!");
            shipTypes.Push(shipType);
            continue;
          }

          var coordinateStrList = new List<string>();
          foreach (var shipCoordinate in shipCoordinates)
          {
            var shipCoordinateString = $"{shipCoordinate.Row},{shipCoordinate.Column}";
            if (shipCoordinatesSet.Contains(shipCoordinateString))
            {
              Console.WriteLine();
              Console.WriteLine($"There will be a collision on position {shipCoordinateString}. Try another position!");
              shipTypes.Push(shipType);
              goto MainLoop;
            }
            coordinateStrList.Add(shipCoordinateString);
          }
          shipCoordinatesSet.UnionWith(coordinateStrList);


          ships.Add(new Ship(shipType, coordinate, isHorizontal));

          Console.WriteLine();
        }
        catch (Exception e)
        {
          throw new Exception($"Unable to start game!", e);
        }
      }

      return ships;
    }
  }

}
