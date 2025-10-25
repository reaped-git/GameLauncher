#ifndef AIPLAYER_HPP
#define AIPLAYER_HPP

#include "Player.hpp"
#include <vector>
#include <algorithm>
#include <random>

class AIPlayer : public Player
{
public:
	// публичные: переопределение типом
	using TargetsType = std::vector<MoveType>;
	using MovesType = std::vector<MoveType>;

public:
	// конструкторы и деконструктор
	AIPlayer(std::string name, int boardSize);
	~AIPlayer() override = default;

	// публичные методы
	void PlaceShips() override;
	MoveType MakeMove() override;
	void UpdateAIState(const std::string& result, MoveType coord);
	bool PlaceShipAlternative(int size, std::mt19937& gen);

private:
	// приватные переменные
	MoveType m_lastHit;
	TargetsType m_potentialTargets;
	MovesType m_allPossibleMoves;
};

#endif // AIPLAYER_HPP