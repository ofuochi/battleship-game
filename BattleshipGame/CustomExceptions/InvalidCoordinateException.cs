using System;
namespace BattleshipGame.CustomExceptions
{
  public class InvalidCoordinateException : ArgumentException
  {
    public InvalidCoordinateException() : base("Invalid coordinates")
    {
    }


    public InvalidCoordinateException(string message) : base(message)
    {
    }

    public InvalidCoordinateException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }

}