namespace BattleshipCS;

public class Ship
{
    public enum ShotResult
    {
        Miss = 0,
        Hit = 1,
        Sunk = 2,
        AlreadyShot = 3
    }

    public int Size { get; }
    public List<(int, int)> Coordinates { get; }
    public bool[] Hits { get; }
    public bool IsHorizontal { get; }

    public Ship(int size, (int, int) startCoord, bool isHorizontal)
    {
        if (size <= 0)
            throw new ArgumentException("Размер корабля должен быть положительным");

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
    }

    public bool IsSunk()
    {
        return Hits.All(hit => hit);
    }

    public bool TakeHit((int, int) coord)
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
}