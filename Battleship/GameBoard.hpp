#pragma once

#include <vector>
#include <set>
#include <utility>
#include <string>
#include <array>
#include <memory>
#include <algorithm>
#include <sstream>
#include "Ship.hpp"

class GameBoard
{
public:
    static const int DEFAULT_BOARD_SIZE = 10;
    static constexpr std::array<std::pair<int, int>, 4> DEFAULT_SHIP_CONFIG = {
        {{ 4, 1 }, { 3, 2 }, { 2, 3 }, { 1, 4 }}
    };

    using ShipsType = std::vector<Ship>;
    using ShotsType = std::set<std::pair<int, int>>;
    using MissesType = std::vector<std::pair<int, int>>;
    using BoardStateType = std::vector<std::vector<char>>;
    using ShipSizesType = std::vector<int>;

public:
    GameBoard(int size);
    ~GameBoard() = default;

    // Конструктор копирования
    GameBoard(const GameBoard& other);

    // Перегрузка операторов
    GameBoard& operator=(const GameBoard& other);
    bool operator==(const GameBoard& other) const;

    // Дружественная функция для конкатенации информации о досках
    friend std::string CombineBoardInfo(const GameBoard& board1, const GameBoard& board2);

    bool PlaceShip(const Ship& ship);
    Ship::ShotResult ReceiveShot(std::pair<int, int> coord);
    bool IsAllShipsSunk() const;
    BoardStateType GetVisibleState(bool forOwner) const;
    static ShipSizesType MakeShipSizes(std::array < std::pair<int, int>, 4> shipConfig);

    int GetSize() const { return m_size; }
    const ShipsType& GetShips() const { return m_ships; }

private:
    int m_size;
    ShipsType m_ships;
    ShotsType m_shots;
    MissesType m_misses;
};