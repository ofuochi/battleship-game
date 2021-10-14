using System;
using BattleshipGame.CustomExceptions;
using BattleshipGame.Enums;
using Xunit;

namespace BattleshipGame.Tests
{
  public class Coordinate_Test
  {
    [Fact]
    public void Coordinate_Test_Constructor_Should_Create_Coordinate_With_Correct_Values()
    {
      // Arrange
      BoardRow row = BoardRow.A;
      BoardColumn col = BoardColumn.One;

      // Act
      Coordinate coordinate = new Coordinate(row, col);

      // Assert
      Assert.Equal(col, coordinate.Column);
      Assert.Equal(row, coordinate.Row);
    }

    [Fact]
    public void Coordinate_Test_Constructor_Should_Throw_Exception_When_Row_Is_Invalid() =>
        Assert.Throws<InvalidCoordinateException>(() => new Coordinate((BoardRow)11, BoardColumn.One));


    [Fact]
    public void Coordinate_Test_Constructor_Should_Throw_Exception_When_Col_Is_Invalid() =>
        Assert.Throws<InvalidCoordinateException>(() => new Coordinate(BoardRow.A, (BoardColumn)11));

  }
}