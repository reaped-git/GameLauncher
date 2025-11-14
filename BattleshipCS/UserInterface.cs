namespace BattleshipCS;
public class UserInterface
{
    private readonly GameManager gameManager;

    public UserInterface(GameManager manager)
    {
        gameManager = manager;
    }

    public void DisplayBoards(Player player)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("         ТЕКУЩЕЕ СОСТОЯНИЕ");
        Console.WriteLine("========================================");
        Console.WriteLine($"Игрок: {player.Name}\n");

        // Поле игрока
        Console.WriteLine("=== ВАШЕ ПОЛЕ ===");
        var myState = player.MyBoard.GetVisibleState(true);

        // Вывод номеров столбцов
        Console.Write("  ");
        for (int j = 0; j < player.MyBoard.Size; j++)
        {
            Console.Write($"{j} ");
        }
        Console.WriteLine();

        for (int i = 0; i < player.MyBoard.Size; i++)
        {
            Console.Write($"{i} ");
            for (int j = 0; j < player.MyBoard.Size; j++)
            {
                Console.Write($"{myState[i, j]} ");
            }
            Console.WriteLine();
        }

        // Поле противника
        Console.WriteLine("\n=== ПОЛЕ ПРОТИВНИКА ===");
        var enemyState = player.EnemyBoard!.GetVisibleState(false);

        // Вывод номеров столбцов
        Console.Write("  ");
        for (int j = 0; j < player.EnemyBoard.Size; j++)
        {
            Console.Write($"{j} ");
        }
        Console.WriteLine();

        for (int i = 0; i < player.EnemyBoard.Size; i++)
        {
            Console.Write($"{i} ");
            for (int j = 0; j < player.EnemyBoard.Size; j++)
            {
                Console.Write($"{enemyState[i, j]} ");
            }
            Console.WriteLine();
        }

        // Легенда
        DisplayLegend();
        Console.WriteLine("========================================");
    }

    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void ShowGameOver(string winnerName)
    {
        Console.WriteLine("\n========================================");
        Console.WriteLine("           ИГРА ОКОНЧЕНА!");
        Console.WriteLine("========================================");
        Console.WriteLine($"*** {winnerName} ПОБЕДИЛ В ИГРЕ! ***");

        // Показываем все корабли противника в конце игры
        var currentPlayer = gameManager.CurrentPlayer;
        if (currentPlayer != null)
        {
            Console.WriteLine("\n=== РАСКРЫТОЕ ПОЛЕ ПРОТИВНИКА ===");

            // Используем forOwner = true чтобы показать все корабли противника
            var revealedState = currentPlayer.EnemyBoard!.GetVisibleState(true);

            // Вывод номеров столбцов
            Console.Write("  ");
            for (int j = 0; j < currentPlayer.EnemyBoard.Size; j++)
            {
                Console.Write($"{j} ");
            }
            Console.WriteLine();

            for (int i = 0; i < currentPlayer.EnemyBoard.Size; i++)
            {
                Console.Write($"{i} ");
                for (int j = 0; j < currentPlayer.EnemyBoard.Size; j++)
                {
                    Console.Write($"{revealedState[i, j]} ");
                }
                Console.WriteLine();
            }
        }
        Console.WriteLine("========================================");
    }

    private void DisplayLegend()
    {
        Console.WriteLine("\n--- ЛЕГЕНДА ---");
        Console.WriteLine("S - ваш корабль");
        Console.WriteLine("X - попадание");
        Console.WriteLine("O - промах");
        Console.WriteLine(". - неизвестная клетка");
        Console.WriteLine("----------------");
    }
}