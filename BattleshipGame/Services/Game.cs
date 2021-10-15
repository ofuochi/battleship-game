using System;
using BattleshipGame.CustomExceptions;
using BattleshipGame.Enums;

namespace BattleshipGame.Services
{
  public class Game
  {
    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }

    public Game(Player player1, Player player2) => (Player1, Player2) = (player1, player2);

    public void AttackPlayer1(Coordinate coordinate) => Attack(Player2, Player1, coordinate);

    public void AttackPlayer2(Coordinate coordinate) => Attack(Player1, Player2, coordinate);

    private void Attack(Player player, Player enemy, Coordinate coordinate)
    {
      var enemyShip = enemy.BattleBoard.GetItem(coordinate);

      if (IsGameOver()) throw new GameOverException();

      if (enemyShip == null)
      {
        player.MapBoard.MarkMiss(coordinate);
        Console.WriteLine();
        Console.WriteLine($"You missed!");
        return;
      }
      else
      {
        if (IsPieceOverlap(player, coordinate))
          throw new PieceOverlapException();

        enemyShip.Hit();
        player.MapBoard.MarkHit(coordinate);
        Console.WriteLine();
        Console.WriteLine($"You hit enemy's {enemyShip}!");
      }

      if (enemyShip.IsSunk)
      {
        enemy.BattleBoard.Ships.Remove(enemyShip);
        Console.WriteLine();
        Console.WriteLine($"You sank an enemy's {enemyShip}!");
      }
    }

    private static bool IsPieceOverlap(Player player, Coordinate coordinate) => player.MapBoard.GetItem(coordinate) != Landmark.Empty;

    public Player GetWinner()
    {
      if (Player1.BattleBoard.Ships.Count == 0 && Player2.BattleBoard.Ships.Count > 0)
        return Player2;

      if (Player2.BattleBoard.Ships.Count == 0 && Player1.BattleBoard.Ships.Count > 0)
        return Player1;

      return null;
    }

    public bool IsGameOver() => Player1.BattleBoard.Ships.Count == 0 || Player2.BattleBoard.Ships.Count == 0;
  }
}