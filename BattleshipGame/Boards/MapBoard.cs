using BattleshipGame.CustomExceptions;
using BattleshipGame.Enums;

namespace BattleshipGame.Boards
{
  public class MapBoard : BaseBoard<Landmark>
  {
    public void MarkHit(Coordinate coordinate) => SetBoardItem(coordinate, Landmark.Hit);
    public void MarkMiss(Coordinate coordinate) => SetBoardItem(coordinate, Landmark.Miss);

    protected override void CheckItemOverlap(Coordinate coordinate)
    {
      if (GetItem(coordinate) != Landmark.Empty)
        throw new PieceOverlapException();
    }
  }
}