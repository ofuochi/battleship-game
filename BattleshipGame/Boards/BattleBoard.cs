using System.Collections.Generic;
using System;
using BattleshipGame.CustomExceptions;
using BattleshipGame.Enums;

namespace BattleshipGame.Boards
{
  public class BattleBoard : BaseBoard<Ship>
  {
    public const int ALLOWED_SHIP_COUNT_ON_BOARD = 5;

    public HashSet<Ship> Ships { get; private set; } = new HashSet<Ship>();

    // Initialize board with random ship positions
    public BattleBoard() => PlaceShipsAtRandomPositions();

    public BattleBoard(List<Ship> ships)
    {
      // Ship types must be unique
      var shipTypes = new HashSet<ShipType>();

      foreach (var ship in ships)
      {
        if (ship == null)
          throw new ArgumentNullException($"Ship cannot be null!");

        shipTypes.Add(ship.ShipType);
      }

      if (ships.Count != ALLOWED_SHIP_COUNT_ON_BOARD || shipTypes.Count != ALLOWED_SHIP_COUNT_ON_BOARD)
        throw new ArgumentException($"There must be {ALLOWED_SHIP_COUNT_ON_BOARD} unique Ships on the Board!");

      foreach (var ship in ships) PlaceShipOnBoard(ship);
    }

    private void PlaceShipOnBoard(Ship ship)
    {
      if (ship.Position.IsHorizontal)
      {
        for (int i = 0; i < ship.Length; i++)
        {
          var coordinate = new Coordinate(ship.Position.StartCoordinate.Row, ship.Position.StartCoordinate.Column + i);
          CheckItemOverlap(coordinate);
          SetBoardItem(coordinate, ship);
        }
      }
      else
      {
        for (int i = 0; i < ship.Length; i++)
        {
          var coordinate = new Coordinate(ship.Position.StartCoordinate.Row + i, ship.Position.StartCoordinate.Column);
          CheckItemOverlap(coordinate);
          SetBoardItem(coordinate, ship);
        }
      }

      Ships.Add(ship);
    }

    private void PlaceShipsAtRandomPositions()
    {
      Random random = new Random();
      var shipTypes = new Stack<ShipType>(Ship.ShipTypes);
      var shipCoordinatesSet = new HashSet<string>();

    MainLoop: while (shipTypes.Count > 0)
      {
        var isHorizontal = random.Next(0, 2) > 0;
        var randomRow = Coordinate.Rows[random.Next(0, Coordinate.Rows.Length)];
        var randomColumn = Coordinate.Columns[random.Next(0, Coordinate.Columns.Length)];
        var randomStartCoordinate = new Coordinate(randomRow, randomColumn);
        var shipType = shipTypes.Pop();

        var canCoordinateFitShip = Ship.ShipPosition.TryGetPositionCoordinates(randomStartCoordinate, shipType, isHorizontal, out var shipCoordinates);

        if (!canCoordinateFitShip)
        {
          shipTypes.Push(shipType);
          continue;
        }

        var coordinateStrList = new List<string>();
        foreach (var shipCoordinate in shipCoordinates)
        {
          var shipCoordinateString = $"{shipCoordinate.Row},{shipCoordinate.Column}";
          if (shipCoordinatesSet.Contains(shipCoordinateString))
          {
            shipTypes.Push(shipType);
            goto MainLoop;
          }
          coordinateStrList.Add(shipCoordinateString);
        }

        shipCoordinatesSet.UnionWith(coordinateStrList);
        PlaceShipOnBoard(new Ship(shipType, randomStartCoordinate, isHorizontal));
      }
    }

    protected override void CheckItemOverlap(Coordinate coordinate)
    {
      var ship = GetItem(coordinate);

      if (ship != null)
        throw new PieceOverlapException($"A \"{ship}\" already exist on coordinate \"{coordinate}\"");
    }
  }

}