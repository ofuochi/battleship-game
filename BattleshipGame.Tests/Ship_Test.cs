using System;
using BattleshipGame.CustomExceptions;
using BattleshipGame.Enums;
using Xunit;

namespace BattleshipGame.Tests
{
  public class Ship_Test
  {
    [Fact]
    public void ShouldThrowInvalidCoordinateException_WhenStartCoordinate_WillMakeShipGoOutsideBoard()
    {
      var ex = Assert.Throws<InvalidCoordinateException>(() => new Ship(ShipType.Carrier, new Coordinate(BoardRow.J, BoardColumn.Ten), true));
      Assert.Contains("start coordinate", ex.Message);
    }

    [Fact]
    public void ShouldCreateAShip_WhenValidStartCoordinatesArePassed()
    {
      var ship = new Ship(ShipType.Carrier, new Coordinate(BoardRow.J, BoardColumn.One), true);
      Assert.NotNull(ship);
      Assert.Equal("J,ONE J,FIVE H", ship.Position.ToString());
    }

    [Fact]
    public void ShouldThrowInvalidOperationException_WhenAnAlreadySunkShipIsBeingHit()
    {
      var ship = new Ship(ShipType.Destroyer, new Coordinate(), true);

      for (int i = 0; i < ship.Length; i++) ship.Hit();

      var ex = Assert.Throws<InvalidOperationException>(() => ship.Hit());
      Assert.Contains("already sunk", ex.Message);
    }
  }
}