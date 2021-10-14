using System.Collections.Generic;
using BattleshipGame.CustomExceptions;
using BattleshipGame.Enums;
using BattleshipGame.Services;
using Xunit;

namespace BattleshipGame.Tests
{
  public class Game_Test
  {
    private readonly Player _player1;
    private readonly Player _player2;

    private readonly Game _game;
    private readonly Coordinate _a1;
    private readonly Coordinate _b1;
    private readonly Coordinate _c1;
    private readonly Coordinate _d1;
    private readonly Coordinate _e1;
    private readonly List<Ship> _ships;

    public Game_Test()
    {
      var isHorizontal = true;
      _a1 = new Coordinate(BoardRow.A, BoardColumn.One);
      _b1 = new Coordinate(BoardRow.B, BoardColumn.One);
      _c1 = new Coordinate(BoardRow.C, BoardColumn.One);
      _d1 = new Coordinate(BoardRow.D, BoardColumn.One);
      _e1 = new Coordinate(BoardRow.E, BoardColumn.One);

      _ships = new List<Ship>
      {
        new Ship(ShipType.Carrier, _a1, isHorizontal),
        new Ship(ShipType.Battleship, _b1, isHorizontal),
        new Ship(ShipType.Cruiser, _c1, isHorizontal),
        new Ship(ShipType.Submarine, _d1, isHorizontal),
        new Ship(ShipType.Destroyer, _e1, isHorizontal)
      };
      _player1 = new Player("Player1", _ships);
      _player2 = new Player("Player2", _ships);
      _game = new Game(_player1, _player2);
    }

    public static IEnumerable<Player> Players()
    {
      var isHorizontal = true;
      var a1 = new Coordinate(BoardRow.A, BoardColumn.One);
      var b1 = new Coordinate(BoardRow.B, BoardColumn.One);
      var c1 = new Coordinate(BoardRow.C, BoardColumn.One);
      var d1 = new Coordinate(BoardRow.D, BoardColumn.One);
      var e1 = new Coordinate(BoardRow.E, BoardColumn.One);

      var ships = new List<Ship>
      {
        new Ship(ShipType.Carrier, a1, isHorizontal),
        new Ship(ShipType.Battleship, b1, isHorizontal),
        new Ship(ShipType.Cruiser, c1, isHorizontal),
        new Ship(ShipType.Submarine, d1, isHorizontal),
        new Ship(ShipType.Destroyer, e1, isHorizontal)
      };

      yield return new Player("Player1", ships);
      yield return new Player("Player2", ships);
    }

    [Fact]
    public void Game_Should_Have_Two_Players()
    {
      Assert.Equal(_player1, _game.Player1);
      Assert.Equal(_player2, _game.Player2);
    }

    [Fact]
    public void ShouldMarkPlayerMapBoard_WithMissLandmark_WhenPlayer1Misses()
    {
      var missTarget = new Coordinate(BoardRow.J, BoardColumn.Ten);

      _game.Attack(_player2, missTarget);

      var missLandmark = _player1.MapBoard.GetItem(missTarget);
      var missedShip = _player2.BattleBoard.GetItem(missTarget);

      Assert.Equal(Landmark.Miss, missLandmark);
      Assert.Null(missedShip);
    }
    [Fact]
    public void ShouldMarkPlayerMapBoard_WithMissLandmark_WhenPlayer2Misses()
    {
      var missTarget = new Coordinate(BoardRow.J, BoardColumn.Ten);

      _game.Attack(_player1, missTarget);

      var missLandmark = _player2.MapBoard.GetItem(missTarget);
      var missedShip = _player1.BattleBoard.GetItem(missTarget);

      Assert.Equal(Landmark.Miss, missLandmark);
      Assert.Null(missedShip);
    }

    [Fact]
    public void ShouldMarkPlayerMapBoard_WithHitLandmark_WhenPlayer1Hits()
    {
      var hitTarget = _a1;
      _game.Attack(_player2, hitTarget);

      var hitLandmark = _player1.MapBoard.GetItem(hitTarget);

      var hitShip = _player2.BattleBoard.GetItem(hitTarget);
      var previousShipLife = Ship.GetLength(hitShip.ShipType);

      Assert.Equal(Landmark.Hit, hitLandmark);
      Assert.Equal(previousShipLife - 1, hitShip.Life);
    }

    [Fact]
    public void ShouldMarkPlayerMapBoard_WithHitLandmark_WhenPlayer2Hits()
    {
      var hitTarget = _a1;
      _game.Attack(_player1, hitTarget);

      var hitLandmark = _player2.MapBoard.GetItem(hitTarget);

      var hitShip = _player1.BattleBoard.GetItem(hitTarget);
      var previousShipLife = Ship.GetLength(hitShip.ShipType);

      Assert.Equal(Landmark.Hit, hitLandmark);
      Assert.Equal(previousShipLife - 1, hitShip.Life);
    }


    [Fact]
    public void ShouldThrowPieceOverlapException_WhenPlayer1AttackSameCoordinate()
    {
      var hitTarget = _a1;
      _game.Attack(_player1, hitTarget);

      Assert.Throws<PieceOverlapException>(() => _game.Attack(_player1, hitTarget));
    }

    [Fact]
    public void ShouldThrowPieceOverlapException_WhenPlayer2AttackSameCoordinate()
    {
      var hitTarget = _a1;
      _game.Attack(_player2, hitTarget);

      Assert.Throws<PieceOverlapException>(() => _game.Attack(_player2, hitTarget));
    }


    [Fact]
    public void ShouldRemoveShipFromPlayer2Ship_WhenShipIsHitUntilItSinks()
    {
      var hitTarget = new Coordinate(BoardRow.E, BoardColumn.Two);
      _game.Attack(_player2, _e1);

      var hitShip = _player2.BattleBoard.GetItem(hitTarget);

      Assert.Equal(ShipType.Destroyer, hitShip.ShipType);

      Assert.Equal(1, hitShip.Life);
      Assert.False(hitShip.IsSunk);
      Assert.Contains(hitShip, _player2.BattleBoard.Ships);

      _game.Attack(_player2, hitTarget);

      Assert.Equal(0, hitShip.Life);
      Assert.True(hitShip.IsSunk);
      Assert.DoesNotContain(hitShip, _player2.BattleBoard.Ships);

      Assert.Null(_game.GetWinner());
    }

    [Fact]
    public void ShouldRemoveShipFromPlayer1Ship_WhenShipIsHitUntilItSinks()
    {
      var hitTarget = new Coordinate(BoardRow.E, BoardColumn.Two);
      _game.Attack(_player1, _e1);

      var hitShip = _player1.BattleBoard.GetItem(hitTarget);

      Assert.Equal(ShipType.Destroyer, hitShip.ShipType);

      Assert.Equal(1, hitShip.Life);
      Assert.False(hitShip.IsSunk);
      Assert.Contains(hitShip, _player1.BattleBoard.Ships);

      _game.Attack(_player1, hitTarget);

      Assert.Equal(0, hitShip.Life);
      Assert.True(hitShip.IsSunk);
      Assert.DoesNotContain(hitShip, _player1.BattleBoard.Ships);

      Assert.Null(_game.GetWinner());
    }


    [Fact]
    public void ShouldReturnPlayer1AsWinner_WhenPlayer1SinksAllShips()
    {
      Assert.False(_game.IsGameOver());

      for (int i = 0; i < _ships.Count; i++)
      {
        var shipLength = Ship.GetLength(_ships[i].ShipType);

        for (int j = 0; j < shipLength; j++)
          _game.Attack(_player2, new Coordinate((BoardRow)i, (BoardColumn)j));

      }

      Assert.Equal(_player1, _game.GetWinner());
      Assert.Throws<GameOverException>(() => _game.Attack(_player2, _a1));

      Assert.True(_game.IsGameOver());
    }

    [Fact]
    public void ShouldReturnPlayer2AsWinner_WhenPlayer2SinksAllShips()
    {
      Assert.False(_game.IsGameOver());

      for (int i = 0; i < _ships.Count; i++)
      {
        var shipLength = Ship.GetLength(_ships[i].ShipType);

        for (int j = 0; j < shipLength; j++)
          _game.Attack(_player1, new Coordinate((BoardRow)i, (BoardColumn)j));

      }

      Assert.Equal(_player2, _game.GetWinner());
      Assert.Throws<GameOverException>(() => _game.Attack(_player1, _a1));

      Assert.True(_game.IsGameOver());
    }
  }
}