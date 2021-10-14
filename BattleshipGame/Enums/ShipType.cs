namespace BattleshipGame.Enums
{
  public enum ShipType
  {
    // Submarines are in reality supposed to be 3 units long, but 
    // 1 is used here took keep these enum values unique.
    Submarine = 1,
    Destroyer = 2,
    Cruiser = 3,
    Battleship = 4,
    Carrier = 5
  }
}