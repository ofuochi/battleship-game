using System.Collections.Generic;
using BattleshipGame.Boards;

namespace BattleshipGame
{
  public class Player
  {
    public BattleBoard BattleBoard { get; private set; }
    public MapBoard MapBoard { get; private set; }
    public string Name { get; private set; }

    public Player(string name, List<Ship> ships) =>
      (Name, BattleBoard, MapBoard) = (name, new BattleBoard(ships), new MapBoard());

    public Player(string name) =>
      (Name, BattleBoard, MapBoard) = (name, new BattleBoard(), new MapBoard());

    override public string ToString() => $"{Name}";
  }
}