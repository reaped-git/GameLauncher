#pragma once

#include <vector>
#include <utility>
#include <stdexcept>
#include <string>
#include <sstream>

class Ship
{
public:
    using CoordinatesType = std::vector<std::pair<int, int>>;
    using HitsType = std::vector<bool>;

    enum class ShotResult
    {
        eMiss = 0,
        eHit = 1,
        eSunk = 2,
        eAlreadyShot = 3
    };

public:
    Ship(int size, std::pair<int, int> startCoord, bool isHorizontal);
    ~Ship() = default;

    // Конструктор копирования
    Ship(const Ship& other);

    // Перегрузка операторов
    Ship& operator=(const Ship& other);
    bool operator==(const Ship& other) const;
    bool operator!=(const Ship& other) const;

    bool IsSunk() const;
    bool TakeHit(std::pair<int, int> coord);

    // Перегрузка оператора преобразования в строку
    explicit operator std::string() const;

    const CoordinatesType& GetCoordinates() const { return m_coordinates; }
    int GetSize() const { return m_size; }
    bool GetIsHorizontal() const { return m_isHorizontal; }

private:
    int m_size;
    CoordinatesType m_coordinates;
    HitsType m_hits;
    bool m_isHorizontal;
};