using System;

namespace BattleshipGame.CustomExceptions
{
  public class GameOverException : InvalidOperationException
  {
    public GameOverException() : base("Game is over!")
    {
    }

    public GameOverException(string message) : base(message)
    {
    }

    public GameOverException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}