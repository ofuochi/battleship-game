using System;

namespace BattleshipGame.CustomExceptions
{
  public class PieceOverlapException : InvalidOperationException
  {
    public PieceOverlapException(string message) : base(message)
    {

    }

    public PieceOverlapException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public PieceOverlapException() : base("Piece overlaps")
    {
    }
  }

}