// Player.cs
namespace BattleshipCS;

public abstract class Player
{
    // Свойства вместо публичных полей
    public string Name { get; }
    public GameBoard MyBoard { get; }
    public GameBoard? EnemyBoard { get; protected set; }

    // Вычисляемое свойство
    public bool HasLost => MyBoard.IsAllShipsSunk();

    protected Player(string name, int boardSize)
    {
        // Валидация входных данных
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя игрока не может быть пустым");

        try
        {
            Name = name.Trim();
            MyBoard = new GameBoard(boardSize);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при создании игрока {name}", ex);
        }
    }

    public void SetEnemyBoard(GameBoard board)
    {
        EnemyBoard = board ?? throw new ArgumentNullException(nameof(board));
    }

    public abstract void PlaceShips();
    public abstract (int, int) MakeMove();
}