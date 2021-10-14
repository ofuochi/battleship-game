using System.Collections.Generic;
using BattleshipGame.Enums;
using Xunit;

namespace BattleshipGame.Tests
{
  public class Player_Tests
  {
    [Fact]
    public void ShouldRandomlyPlacesShipsOnBoard_WhenNewPlayerIsCreated()
    {
      var player = new Player("Player");

      Assert.Equal("Player", player.Name);
      Assert.Equal("Player", player.ToString());
      Assert.Equal(5, player.BattleBoard.Ships.Count);

    }

    [Fact]
    public void ShouldPlaceShipInCorrectPosition_WhenPlayerIsCreatedWithListOfShips()
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
        new Ship(ShipType.Destroyer, coordinateE5, isHorizontal)
      };
      var player = new Player("PlayerName", ships);

      Assert.Equal(5, player.BattleBoard.Ships.Count);

      Assert.Equal(ShipType.Carrier, player.BattleBoard.GetItem(coordinateA1).ShipType);
      Assert.Equal(ShipType.Battleship, player.BattleBoard.GetItem(coordinateB2).ShipType);
      Assert.Equal(ShipType.Cruiser, player.BattleBoard.GetItem(coordinateC3).ShipType);
      Assert.Equal(ShipType.Submarine, player.BattleBoard.GetItem(coordinateD4).ShipType);
      Assert.Equal(ShipType.Destroyer, player.BattleBoard.GetItem(coordinateE5).ShipType);

    }

    [Fact]
    public void PlayerShouldHaveBattleBoardAndMapBoard_WhenPlayerIsCreated()
    {
      var player = new Player("Player");

      Assert.NotNull(player.BattleBoard);
      Assert.NotNull(player.MapBoard);
    }

  }
}
