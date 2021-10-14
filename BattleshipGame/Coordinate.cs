using System;
using BattleshipGame.CustomExceptions;
using BattleshipGame.Enums;

namespace BattleshipGame
{
  public struct Coordinate
  {
    public BoardRow Row { get; private set; }
    public BoardColumn Column { get; private set; }

    public Coordinate(BoardRow r, BoardColumn c)
    {
      if (!IsRowValid(r) || !IsColumnValid(c))
        throw new InvalidCoordinateException();

      (Row, Column) = (r, c);
    }

    public static bool IsRowValid(BoardRow r) => r >= BoardRow.A && r <= BoardRow.J;
    public static bool IsColumnValid(BoardColumn c) => c >= BoardColumn.One && c <= BoardColumn.Ten;

    public static BoardRow[] Rows => (BoardRow[])Enum.GetValues(typeof(BoardRow));

    public static BoardColumn[] Columns => (BoardColumn[])Enum.GetValues(typeof(BoardColumn));

    public override string ToString() => $"{Row},{Column}".ToUpper();
  }
}