// AIPlayer.hpp
#pragma once

#include "Player.hpp"
#include "GameBoard.hpp"
#include <vector>
#include <algorithm>
#include <random>
#include <memory>
#include <sstream>

class AIPlayer : public Player
{
public:
    static const int MAX_ATEMPTS = 100;
    using TargetsType = std::vector<MoveType>;
    using MovesType = std::vector<MoveType>;

public:
    AIPlayer(std::string name, int boardSize);
    ~AIPlayer() override = default;

    // Конструктор копирования с вызовом базового класса
    AIPlayer(const AIPlayer& other);

    // Перегрузка оператора присваивания
    AIPlayer& operator=(const AIPlayer& other);

    // Виртуальный метод для клонирования (теперь переопределяет базовый)
    AIPlayer* Clone() const override;

    void PlaceShips() override;
    MoveType MakeMove() override;
    void UpdateAIState(Ship::ShotResult result, MoveType coord);
    bool PlaceShipAlternative(int size, std::mt19937& gen);

private:
    MoveType m_lastHit;
    TargetsType m_potentialTargets;
    MovesType m_allPossibleMoves;
    GameBoard::ShipSizesType shipSizes;
};