// GameBoard.cs
namespace BattleshipCS;

public class GameBoard
{
    // Статические поля и методы
    public static readonly int DEFAULT_BOARD_SIZE = 10;
    public static readonly (int, int)[] DEFAULT_SHIP_CONFIG = {
        (4, 1), (3, 2), (2, 3), (1, 4)
    };

    // Статический метод для валидации размера доски
    public static bool IsValidBoardSize(int size)
    {
        return size >= 5 && size <= 15;
    }

    // Свойства с приватными сеттерами
    public int Size { get; }
    public List<Ship> Ships { get; private set; }
    public HashSet<(int, int)> Shots { get; private set; }
    public List<(int, int)> Misses { get; private set; }

    // Вычисляемое свойство
    public int RemainingShips => Ships.Count(ship => !ship.IsSunk());

    public GameBoard(int size)
    {
        // Валидация с выбрасыванием исключения
        if (!IsValidBoardSize(size))
            throw new ArgumentException($"Недопустимый размер доски: {size}. Допустимый диапазон: 5-15");

        try
        {
            Size = size;
            Ships = new List<Ship>();
            Shots = new HashSet<(int, int)>();
            Misses = new List<(int, int)>();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Ошибка инициализации игровой доски", ex);
        }
    }

    public bool PlaceShip(Ship ship)
    {
        // Защитный блок
        try
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
        catch (Exception ex)
        {
            throw new InvalidOperationException("Ошибка при размещении корабля", ex);
        }
    }

    public Ship.ShotResult ReceiveShot((int, int) coord)
    {
        // Защитный блок с обработкой исключений
        try
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
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при обработке выстрела в ({coord.Item1}, {coord.Item2})", ex);
        }
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
        // Защитный блок
        try
        {
            var sizes = new List<int>();
            foreach (var (shipLength, shipCount) in shipConfig)
            {
                if (shipLength <= 0 || shipCount < 0)
                    throw new ArgumentException("Недопустимая конфигурация кораблей");

                for (int i = 0; i < shipCount; i++)
                {
                    sizes.Add(shipLength);
                }
            }
            return sizes;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Ошибка при создании списка размеров кораблей", ex);
        }
    }
}