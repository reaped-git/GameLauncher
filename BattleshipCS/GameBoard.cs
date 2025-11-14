namespace BattleshipCS;

public class GameBoard
{
    public static readonly int DEFAULT_BOARD_SIZE = 10;
    public static readonly (int, int)[] DEFAULT_SHIP_CONFIG = {
        (4, 1), (3, 2), (2, 3), (1, 4)
    };

    public int Size { get; }
    public List<Ship> Ships { get; }
    public HashSet<(int, int)> Shots { get; }
    public List<(int, int)> Misses { get; }

    public GameBoard(int size)
    {
        Size = size;
        Ships = new List<Ship>();
        Shots = new HashSet<(int, int)>();
        Misses = new List<(int, int)>();
    }

    public bool PlaceShip(Ship ship)
    {
        // Проверка на выход за границы
        foreach (var coord in ship.Coordinates)
        {
            if (coord.Item1 < 0 || coord.Item1 >= Size ||
                coord.Item2 < 0 || coord.Item2 >= Size)
            {
                return false;
            }
        }

        // Проверка на пересечение с другими кораблями
        foreach (var existingShip in Ships)
        {
            foreach (var existingCoord in existingShip.Coordinates)
            {
                foreach (var newCoord in ship.Coordinates)
                {
                    // Проверка самой координаты и соседних клеток
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (existingCoord.Item1 + i == newCoord.Item1 &&
                                existingCoord.Item2 + j == newCoord.Item2)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }

        Ships.Add(ship);
        return true;
    }

    public Ship.ShotResult ReceiveShot((int, int) coord)
    {
        // Проверка на повторный выстрел
        if (Shots.Contains(coord))
        {
            return Ship.ShotResult.AlreadyShot;
        }

        Shots.Add(coord);

        // Проверка попадания
        foreach (var ship in Ships)
        {
            if (ship.TakeHit(coord))
            {
                if (ship.IsSunk())
                {
                    return Ship.ShotResult.Sunk;
                }
                return Ship.ShotResult.Hit;
            }
        }

        Misses.Add(coord);
        return Ship.ShotResult.Miss;
    }

    public bool IsAllShipsSunk()
    {
        return Ships.All(ship => ship.IsSunk());
    }

    public char[,] GetVisibleState(bool forOwner)
    {
        var state = new char[Size, Size];

        // Инициализация поля
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                state[i, j] = '.';

        // Отображение промахов
        foreach (var miss in Misses)
        {
            state[miss.Item1, miss.Item2] = 'O';
        }

        // Отображение попаданий
        foreach (var ship in Ships)
        {
            foreach (var coord in ship.Coordinates)
            {
                if (Shots.Contains(coord))
                {
                    state[coord.Item1, coord.Item2] = 'X';
                }
            }
        }

        // Показываем корабли ТОЛЬКО если это поле владельца
        if (forOwner)
        {
            foreach (var ship in Ships)
            {
                foreach (var coord in ship.Coordinates)
                {
                    // Показываем неподбитые части кораблей
                    if (!Shots.Contains(coord))
                    {
                        state[coord.Item1, coord.Item2] = 'S';
                    }
                }
            }
        }

        return state;
    }

    public static List<int> MakeShipSizes((int, int)[] shipConfig)
    {
        var sizes = new List<int>();
        foreach (var (shipLength, shipCount) in shipConfig)
        {
            for (int i = 0; i < shipCount; i++)
            {
                sizes.Add(shipLength);
            }
        }
        return sizes;
    }
}