using BattleshipGame.Boards;
using BattleshipGame.CustomExceptions;
using BattleshipGame.Enums;
using Xunit;

namespace BattleshipGame.Tests
{
  public class MapBoard_Tests
  {
    private readonly MapBoard _mapBoard;

    public MapBoard_Tests() => _mapBoard = new MapBoard();


    [Fact]
    public void ShouldMarkBoardWithHitLandmark_WhenHitIsCalled()
    {
      var coordinate = new Coordinate(BoardRow.A, BoardColumn.One);
      _mapBoard.MarkHit(coordinate);

      Assert.Equal(Landmark.Hit, _mapBoard.GetItem(coordinate));
    }

    [Fact]
    public void ShouldMarkBoardWithMissLandmark_WhenMissIsCalled()
    {
      var coordinate = new Coordinate(BoardRow.A, BoardColumn.One);
      _mapBoard.MarkMiss(coordinate);

      Assert.Equal(Landmark.Miss, _mapBoard.GetItem(coordinate));
    }

    [Fact]
    public void ShouldThrowPieceOverlapException_WhenAnAlreadyMarkedCoordinateIsMarkedAgain()
    {
      var coordinate = new Coordinate(BoardRow.A, BoardColumn.One);
      _mapBoard.MarkHit(coordinate);

      Assert.Throws<PieceOverlapException>(() => _mapBoard.MarkHit(coordinate));
      Assert.Throws<PieceOverlapException>(() => _mapBoard.MarkMiss(coordinate));
    }

  }
}