namespace BattleshipGame.Boards
{
  public abstract class BaseBoard<T>
  {
    private readonly T[,] _board;

    public static int Height => 10;

    public static int Width = 10;

    public BaseBoard() => _board = new T[Width, Height];

    public virtual T GetItem(Coordinate coordinate) => _board[(int)coordinate.Row, (int)coordinate.Column];

    protected abstract void CheckItemOverlap(Coordinate coordinate);
    protected virtual void SetBoardItem(Coordinate coordinate, T item)
    {
      CheckItemOverlap(coordinate);
      _board[(int)coordinate.Row, (int)coordinate.Column] = item;
    }
  }
}