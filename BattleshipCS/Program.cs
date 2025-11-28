// Program.cs
namespace BattleshipCS;

class Program
{
    static void Main()
    {
        // Использование using для GameManager (если бы он был IDisposable)
        try
        {
            RunGame();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка: {ex.Message}");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }

    private static void RunGame()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("           МОРСКОЙ БОЙ");
            Console.WriteLine("========================================");
            Console.WriteLine("Правила игры:");
            Console.WriteLine("- Расставьте корабли на поле 10x10");
            Console.WriteLine("- Корабли не могут касаться друг друга");
            Console.WriteLine("- Стандартный набор кораблей:");
            Console.WriteLine("  1 корабль - 4 клетки");
            Console.WriteLine("  2 корабля - 3 клетки");
            Console.WriteLine("  3 корабля - 2 клетки");
            Console.WriteLine("  4 корабля - 1 клетка");
            Console.WriteLine("- Вы можете выбрать ручную или автоматическую расстановку");
            Console.WriteLine("========================================\n");

            try
            {
                const int BOARD_SIZE = 10;

                // Использование статических методов
                if (!GameBoard.IsValidBoardSize(BOARD_SIZE))
                {
                    throw new InvalidOperationException("Недопустимый размер игрового поля");
                }

                // Сброс счетчика кораблей перед новой игрой
                Ship.ResetShipCounter();

                using (var gameManager = new GameManager(BOARD_SIZE))
                {
                    // Настройка игры
                    gameManager.SetupGame();

                    Console.WriteLine("\n========================================");
                    Console.WriteLine("           НАЧАЛО ИГРЫ!");
                    Console.WriteLine("========================================");
                    Console.WriteLine("После каждого хода будут показаны оба поля.");
                    Console.WriteLine("Нажмите Enter для продолжения...");
                    Console.ReadLine();

                    // Запуск игрового цикла
                    gameManager.RunGameLoop();

                    // Вывод статистики по созданным кораблям
                    Console.WriteLine($"\nВсего создано кораблей в игре: {Ship.TotalShipsCreated}");
                }

                Console.WriteLine("\nЕще раз? (y/n)");

                var again = Console.ReadLine();
                if (again?.ToLower() == "y") continue;
                else break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка аргумента: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Ошибка операции: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }
}