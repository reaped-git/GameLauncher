namespace BattleshipCS;

public abstract class Player
{
    public string Name { get; }
    public GameBoard MyBoard { get; }
    public GameBoard? EnemyBoard { get; protected set; }

    protected Player(string name, int boardSize)
    {
        Name = name;
        MyBoard = new GameBoard(boardSize);
    }

    public void SetEnemyBoard(GameBoard board)
    {
        EnemyBoard = board;
    }

    public abstract void PlaceShips();
    public abstract (int, int) MakeMove();
}