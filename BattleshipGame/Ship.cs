using System.Collections.Generic;
using System;
using BattleshipGame.CustomExceptions;
using BattleshipGame.Enums;

namespace BattleshipGame
{
  public class Ship
  {
    public struct ShipPosition
    {
      public Coordinate StartCoordinate { get; private set; }
      public Coordinate EndCoordinate { get; private set; }
      public bool IsHorizontal { get; private set; }

      public ShipPosition(Coordinate startCoordinate, bool isHorizontal, ShipType shipType)
      {
        var isValid = TryComputeEndCoordinate(startCoordinate, isHorizontal, shipType, out Coordinate endCoordinate);

        if (!isValid) throw new InvalidCoordinateException("Invalid ship start coordinate");

        StartCoordinate = startCoordinate;
        EndCoordinate = endCoordinate;
        IsHorizontal = isHorizontal;
      }

      private static bool TryComputeEndCoordinate(Coordinate startCoordinate, bool isHorizontal, ShipType shipType, out Coordinate endCoordinate)
      {
        var shipSize = Ship.GetLength(shipType);
        if (isHorizontal)
        {
          var column = startCoordinate.Column + shipSize - 1;
          if (!Coordinate.IsColumnValid(column))
          {
            endCoordinate = new Coordinate(0, 0);
            return false;
          }
          endCoordinate = new Coordinate(startCoordinate.Row, column);
        }
        else
        {
          var row = startCoordinate.Row + shipSize - 1;
          if (!Coordinate.IsRowValid(row))
          {
            endCoordinate = new Coordinate(0, 0);
            return false;
          }
          endCoordinate = new Coordinate(row, startCoordinate.Column);
        }

        return true;
      }
      public static bool CanCoordinateFitShip(Coordinate startCoordinate, bool isHorizontal, ShipType shipType) =>
        TryComputeEndCoordinate(startCoordinate, isHorizontal, shipType, out _);

      public static bool TryGetPositionCoordinates(Coordinate startCoordinate, ShipType shipType, bool isHorizontal, out List<Coordinate> coordinates)
      {
        coordinates = new List<Coordinate>();
        if (!CanCoordinateFitShip(startCoordinate, isHorizontal, shipType)) return false;

        var shipLength = Ship.GetLength(shipType);
        if (isHorizontal)
        {
          for (var i = 0; i < shipLength; i++)
            coordinates.Add(new Coordinate(startCoordinate.Row, startCoordinate.Column + i));
        }
        else
        {
          for (var i = 0; i < shipLength; i++)
            coordinates.Add(new Coordinate(startCoordinate.Row + i, startCoordinate.Column));
        }
        return true;
      }

      override public string ToString() => $"{StartCoordinate} {EndCoordinate} {(IsHorizontal ? "H" : "V")}";
    }

    public int Life { get; private set; }
    private const int SUBMARINE_LENGTH = 3;

    public ShipType ShipType { get; private set; }

    public int Length { get; private set; }
    public bool IsSunk { get; private set; }
    public ShipPosition Position { get; private set; }


    public Ship(ShipType shipType, Coordinate startCoordinate, bool isHorizontal)
    {
      var shipLength = GetLength(shipType);
      ShipType = shipType;
      Life = shipLength;
      Length = shipLength;
      Position = new ShipPosition(startCoordinate, isHorizontal, shipType);
    }

    public void Hit()
    {
      if (IsSunk) throw new InvalidOperationException("Ship is already sunk!");

      Life--;

      if (Life == 0) IsSunk = true;
    }

    public static ShipType[] ShipTypes => (ShipType[])Enum.GetValues(typeof(ShipType));

    /// <summary>This is a helper method to get the length of a ship because a Submarine and a Destroyer are both 3 units long.
    /// This method serves to differentiate between the two ships. More info: https://en.wikipedia.org/wiki/Battleship_(game)
    /// </summary>
    /// <param name="shipType"></param>
    /// <returns>an integer representing the ship's unit length</returns>
    public static int GetLength(ShipType shipType) => shipType == ShipType.Submarine ? SUBMARINE_LENGTH : (int)shipType;

    public override string ToString() => $"{ShipType}";
  }
}