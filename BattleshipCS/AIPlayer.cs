namespace BattleshipCS;

public class AIPlayer : Player
{
    public const int MAX_ATTEMPTS = 100;

    private (int, int) lastHit = (-1, -1);
    private readonly List<(int, int)> potentialTargets = new();
    private readonly List<(int, int)> allPossibleMoves = new();
    private readonly List<int> shipSizes;

    public AIPlayer(string name, int boardSize) : base(name, boardSize)
    {
        // Генерируем все возможные ходы
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                allPossibleMoves.Add((i, j));
            }
        }

        // Перемешиваем ходы
        var random = new Random();
        allPossibleMoves = allPossibleMoves.OrderBy(x => random.Next()).ToList();

        shipSizes = GameBoard.MakeShipSizes(GameBoard.DEFAULT_SHIP_CONFIG);
    }

    public override void PlaceShips()
    {
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

                var ship = new Ship(size, (row, col), horizontal);
                placed = MyBoard.PlaceShip(ship);
                attempts++;
            }

            if (!placed)
            {
                placed = PlaceShipAlternative(size, random);
            }
        }
    }

    private bool PlaceShipAlternative(int size, Random random)
    {
        for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
        {
            foreach (bool horizontal in new[] { true, false })
            {
                for (int row = 0; row < MyBoard.Size; row++)
                {
                    for (int col = 0; col < MyBoard.Size; col++)
                    {
                        var ship = new Ship(size, (row, col), horizontal);
                        if (MyBoard.PlaceShip(ship))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public override (int, int) MakeMove()
    {
        // Если есть потенциальные цели, стреляем в них
        if (potentialTargets.Count > 0)
        {
            var target = potentialTargets[^1];
            potentialTargets.RemoveAt(potentialTargets.Count - 1);
            return target;
        }

        // Иначе стреляем в случайную клетку из доступных
        if (allPossibleMoves.Count > 0)
        {
            var move = allPossibleMoves[^1];
            allPossibleMoves.RemoveAt(allPossibleMoves.Count - 1);
            return move;
        }

        // Запасной вариант
        return (0, 0);
    }

    public void UpdateAIState(Ship.ShotResult result, (int, int) coord)
    {
        if (result == Ship.ShotResult.Hit)
        {
            lastHit = coord;

            // Добавляем соседние клетки как потенциальные цели
            (int, int)[] directions = { (0, 1), (1, 0), (0, -1), (-1, 0) };
            foreach (var dir in directions)
            {
                int newRow = coord.Item1 + dir.Item1;
                int newCol = coord.Item2 + dir.Item2;

                if (newRow >= 0 && newRow < MyBoard.Size &&
                    newCol >= 0 && newCol < MyBoard.Size)
                {
                    var newTarget = (newRow, newCol);

                    // Проверяем, что этот ход еще возможен
                    if (allPossibleMoves.Contains(newTarget))
                    {
                        potentialTargets.Add(newTarget);
                        allPossibleMoves.Remove(newTarget);
                    }
                }
            }
        }
        else if (result == Ship.ShotResult.Sunk)
        {
            // Очищаем потенциальные цели при потоплении корабля
            potentialTargets.Clear();
            lastHit = (-1, -1);
        }
    }
}