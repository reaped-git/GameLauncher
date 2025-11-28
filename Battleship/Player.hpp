#pragma once

#include <string>
#include <utility>
#include <memory>
#include <sstream>
#include "GameBoard.hpp"

class Player
{
public:
    using MoveType = std::pair<int, int>;

public:
    Player(std::string name, int boardSize);
    virtual ~Player() = default;

    // Конструктор копирования
    Player(const Player& other);

    // Перегрузка оператора присваивания
    Player& operator=(const Player& other);

    // Виртуальный метод для клонирования (добавлен для исправления ошибки)
    virtual Player* Clone() const = 0;

    virtual void PlaceShips() = 0;
    virtual MoveType MakeMove() = 0;

    void SetEnemyBoard(std::shared_ptr<GameBoard> board) { m_enemyBoard = board; }
    GameBoard& GetMyBoard() { return m_myBoard; }
    std::shared_ptr<GameBoard> GetEnemyBoard() { return m_enemyBoard; }
    std::string GetName() const { return m_name; }

    // Перегрузка оператора вывода
    friend std::ostream& operator<<(std::ostream& os, const Player& player);

protected:
    std::string m_name;
    GameBoard m_myBoard;
    std::shared_ptr<GameBoard> m_enemyBoard;
};