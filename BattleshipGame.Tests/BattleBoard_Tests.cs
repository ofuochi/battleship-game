using System;
using System.Collections.Generic;
using BattleshipGame.Boards;
using BattleshipGame.CustomExceptions;
using BattleshipGame.Enums;
using Xunit;

namespace BattleshipGame.Tests
{
  public class BattleBoard_Tests
  {
    [Fact]
    public void ShouldThrowPieceOverlapException_WhenShipOverlapsInHorizontalPosition()
    {
      var isHorizontal = true;
      var coordinate = new Coordinate(BoardRow.A, BoardColumn.One);

      var ships = new List<Ship>
      {
        new Ship(ShipType.Carrier, coordinate, isHorizontal),
        new Ship(ShipType.Battleship, coordinate, isHorizontal),
        new Ship(ShipType.Cruiser, coordinate, isHorizontal),
        new Ship(ShipType.Submarine, coordinate, isHorizontal),
        new Ship(ShipType.Destroyer, coordinate, isHorizontal)
      };

      var ex = Assert.Throws<PieceOverlapException>(() => new BattleBoard(ships));
      Assert.Contains("already exist", ex.Message.ToLower());
    }

    [Fact]
    public void ShouldThrowPieceOverlapException_WhenShipOverlapsInVerticalPosition()
    {
      var isHorizontal = true;
      var coordinate = new Coordinate(BoardRow.A, BoardColumn.One);

      var ships = new List<Ship>
      {
        new Ship(ShipType.Carrier, coordinate, !isHorizontal),
        new Ship(ShipType.Battleship, coordinate, !isHorizontal),
        new Ship(ShipType.Cruiser, coordinate, !isHorizontal),
        new Ship(ShipType.Submarine, coordinate, !isHorizontal),
        new Ship(ShipType.Destroyer, coordinate, !isHorizontal)
      };

      var ex = Assert.Throws<PieceOverlapException>(() => new BattleBoard(ships));
      Assert.Contains("already exist", ex.Message.ToLower());
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenShipsToBePlaceAreNotUnique()
    {
      var isHorizontal = true;
      var coordinateA1 = new Coordinate(BoardRow.A, BoardColumn.One);
      var coordinateB2 = new Coordinate(BoardRow.B, BoardColumn.Two);
      var coordinateC3 = new Coordinate(BoardRow.C, BoardColumn.Three);
      var coordinateD4 = new Coordinate(BoardRow.D, BoardColumn.Four);
      var coordinateE5 = new Coordinate(BoardRow.E, BoardColumn.Five);

      var ships = new List<Ship>
      {
        new Ship(ShipType.Carrier, coordinateA1, isHorizontal),
        new Ship(ShipType.Carrier, coordinateB2, !isHorizontal),
        new Ship(ShipType.Cruiser, coordinateC3, isHorizontal),
        new Ship(ShipType.Submarine, coordinateD4, !isHorizontal),
        new Ship(ShipType.Destroyer, coordinateE5, isHorizontal)
      };

      var ex = Assert.Throws<ArgumentException>(() => new BattleBoard(ships));
      Assert.Contains("5 unique", ex.Message);
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenCreatingBoard_WithLessThanFiveShips()
    {
      var isHorizontal = true;
      var coordinate = new Coordinate(BoardRow.A, BoardColumn.One);

      var ships = new List<Ship> { new Ship(ShipType.Carrier, coordinate, isHorizontal) };
      var ex = Assert.Throws<ArgumentException>(() => new BattleBoard(ships));
      Assert.Contains("5 unique", ex.Message);
    }

    [Fact]
    public void ShouldArgumentNullException_WhenCreatingBattleBoard_WithAnyShipIsNull()
    {
      var isHorizontal = true;
      var coordinateA1 = new Coordinate(BoardRow.A, BoardColumn.One);
      var coordinateB2 = new Coordinate(BoardRow.B, BoardColumn.Two);
      var coordinateC3 = new Coordinate(BoardRow.C, BoardColumn.Three);
      var coordinateD4 = new Coordinate(BoardRow.D, BoardColumn.Four);
      var coordinateE5 = new Coordinate(BoardRow.E, BoardColumn.Five);

      var ships = new List<Ship>
      {
        new Ship(ShipType.Carrier, coordinateA1, isHorizontal),
        new Ship(ShipType.Battleship, coordinateB2, !isHorizontal),
        new Ship(ShipType.Cruiser, coordinateC3, isHorizontal),
        new Ship(ShipType.Submarine, coordinateD4, !isHorizontal),
        null
      };

      var ex = Assert.Throws<ArgumentNullException>(() => new BattleBoard(ships));
      Assert.Contains("cannot be null", ex.Message);
    }
  }
}