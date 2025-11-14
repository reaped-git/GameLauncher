namespace BattleshipCS;

class Program
{
    static void Main()
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
                var gameManager = new GameManager(BOARD_SIZE);

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

                Console.WriteLine("\nЕще раз? (y/n)");

                var again = Console.ReadLine();
                if (again?.ToLower() == "y") continue;
                else break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return;
            }
        }
    }
}