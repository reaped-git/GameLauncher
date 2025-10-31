#include "Player.hpp"

Player::Player(std::string name, int boardSize)
	: m_name(name)
	, m_myBoard(boardSize)
	, m_enemyBoard(nullptr)
{
}