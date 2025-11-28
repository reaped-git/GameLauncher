namespace BattleshipCS;

public class Ship
{
    // Статическое поле - счетчик созданных кораблей
    public static int TotalShipsCreated { get; private set; } = 0;

    // Статический метод для сброса счетчика
    public static void ResetShipCounter()
    {
        TotalShipsCreated = 0;
    }

    public enum ShotResult
    {
        Miss = 0,
        Hit = 1,
        Sunk = 2,
        AlreadyShot = 3
    }

    // Свойства вместо публичных полей
    public int Size { get; }
    public List<(int, int)> Coordinates { get; }
    public bool[] Hits { get; }
    public bool IsHorizontal { get; }

    // Новое вычисляемое свойство
    public bool IsOperational => !IsSunk();

    public Ship(int size, (int, int) startCoord, bool isHorizontal)
    {
        if (size <= 0)
            throw new ArgumentException("Размер корабля должен быть положительным");

        // Использование try-catch блока
        try
        {
            Size = size;
            IsHorizontal = isHorizontal;
            Hits = new bool[size];
            Coordinates = new List<(int, int)>();

            // Генерация всех координат корабля
            for (int i = 0; i < size; i++)
            {
                if (isHorizontal)
                {
                    Coordinates.Add((startCoord.Item1, startCoord.Item2 + i));
                }
                else
                {
                    Coordinates.Add((startCoord.Item1 + i, startCoord.Item2));
                }
            }

            // Увеличиваем счетчик созданных кораблей
            TotalShipsCreated++;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при создании корабля: {ex.Message}", ex);
        }
    }

    public bool IsSunk()
    {
        return Hits.All(hit => hit);
    }

    public bool TakeHit((int, int) coord)
    {
        // Защитный блок с обработкой исключений
        try
        {
            for (int i = 0; i < Size; i++)
            {
                if (Coordinates[i].Equals(coord))
                {
                    Hits[i] = true;
                    return true;
                }
            }
            return false;
        }
        catch (IndexOutOfRangeException ex)
        {
            throw new InvalidOperationException("Ошибка доступа к координатам корабля", ex);
        }
    }
}