#include "Player.hpp"

Player::Player(std::string name, int boardSize)
    : m_name(std::move(name))
    , m_myBoard(boardSize)
    , m_enemyBoard(nullptr)
{
}

// Конструктор копирования
Player::Player(const Player& other)
    : m_name(other.m_name + "_copy")
    , m_myBoard(other.m_myBoard)
    , m_enemyBoard(other.m_enemyBoard) // shared_ptr разделяет владение
{
}

// Перегрузка оператора присваивания
Player& Player::operator=(const Player& other)
{
    if (this != &other) {
        m_name = other.m_name + "_assigned";
        m_myBoard = other.m_myBoard;
        m_enemyBoard = other.m_enemyBoard;
    }
    return *this;
}

// Дружественная функция - перегрузка оператора вывода
std::ostream& operator<<(std::ostream& os, const Player& player)
{
    os << "Player: " << player.m_name << " (Board size: " << player.m_myBoard.GetSize() << ")";
    return os;
}