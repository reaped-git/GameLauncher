namespace BattleshipCS;

public class HumanPlayer : Player
{
    public const int MAX_ATTEMPTS = 100;
    private readonly List<int> shipSizes;

    public HumanPlayer(string name, int boardSize) : base(name, boardSize)
    {
        shipSizes = GameBoard.MakeShipSizes(GameBoard.DEFAULT_SHIP_CONFIG);
    }

    public override void PlaceShips()
    {
        Console.WriteLine($"{Name}, выберите способ расстановки кораблей:");
        Console.WriteLine("1 - Ручная расстановка");
        Console.WriteLine("2 - Автоматическая расстановка");

        int choice;
        while (true)
        {
            Console.Write("Ваш выбор (1 или 2): ");
            if (int.TryParse(Console.ReadLine(), out choice) && (choice == 1 || choice == 2))
            {
                break;
            }
            Console.WriteLine("Неверный выбор. Попробуйте снова.");
        }

        if (choice == 1)
        {
            ManualPlacement();
        }
        else
        {
            AutomaticPlacement();
        }
    }

    private void ManualPlacement()
    {
        Console.WriteLine("\n=== РУЧНАЯ РАССТАНОВКА КОРАБЛЕЙ ===");

        foreach (int size in shipSizes)
        {
            bool placed = false;
            while (!placed)
            {
                DisplayBoardState();
                Console.WriteLine($"Разместите корабль размером {size}");

                int row = GetValidatedInput($"Введите номер ряда (0-{MyBoard.Size - 1}): ", 0, MyBoard.Size - 1);
                int col = GetValidatedInput($"Введите номер столбца (0-{MyBoard.Size - 1}): ", 0, MyBoard.Size - 1);
                int orientation = GetValidatedInput("Ориентация (0 - горизонтально, 1 - вертикально): ", 0, 1);

                placed = TryPlaceShip(size, row, col, orientation == 0);

                if (!placed)
                {
                    Console.WriteLine("Невозможно разместить корабль здесь. Попробуйте снова.");
                }
            }
        }

        Console.WriteLine("Все корабли успешно расставлены!");
        DisplayBoardState();
    }

    private void AutomaticPlacement()
    {
        Console.WriteLine("\n=== АВТОМАТИЧЕСКАЯ РАССТАНОВКА КОРАБЛЕЙ ===");

        var random = new Random();

        foreach (int size in shipSizes)
        {
            bool placed = false;
            int attempts = 0;

            while (!placed && attempts < MAX_ATTEMPTS)
            {
                int row = random.Next(MyBoard.Size);
                int col = random.Next(MyBoard.Size);
                bool horizontal = random.Next(2) == 0;

                placed = TryPlaceShip(size, row, col, horizontal);
                attempts++;
            }

            if (!placed)
            {
                Console.WriteLine($"Не удалось автоматически разместить корабль размером {size}. Попробуйте ручную расстановку.");
                ManualPlacement();
                return;
            }
        }

        Console.WriteLine("Все корабли успешно расставлены автоматически!");
        DisplayBoardState();
    }

    private bool TryPlaceShip(int size, int row, int col, bool horizontal)
    {
        var ship = new Ship(size, (row, col), horizontal);
        return MyBoard.PlaceShip(ship);
    }

    public override (int, int) MakeMove()
    {
        Console.WriteLine($"{Name}, ваш ход:");

        int row = GetValidatedInput($"Введите номер ряда (0-{MyBoard.Size - 1}): ", 0, MyBoard.Size - 1);
        int col = GetValidatedInput($"Введите номер столбца (0-{MyBoard.Size - 1}): ", 0, MyBoard.Size - 1);

        return (row, col);
    }

    private void DisplayBoardState()
    {
        var state = MyBoard.GetVisibleState(true);
        Console.WriteLine("Ваше поле:");

        // Вывод номеров столбцов
        Console.Write("  ");
        for (int j = 0; j < MyBoard.Size; j++)
        {
            Console.Write($"{j} ");
        }
        Console.WriteLine();

        for (int i = 0; i < MyBoard.Size; i++)
        {
            Console.Write($"{i} ");
            for (int j = 0; j < MyBoard.Size; j++)
            {
                Console.Write($"{state[i, j]} ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private int GetValidatedInput(string prompt, int minValue, int maxValue)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out value) && value >= minValue && value <= maxValue)
            {
                break;
            }
            Console.WriteLine($"Ошибка: число должно быть в диапазоне от {minValue} до {maxValue}!");
        }
        return value;
    }
}