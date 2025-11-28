// HumanPlayer.hpp
#pragma once

#include "Player.hpp"
#include "GameBoard.hpp"
#include <iostream>
#include <memory>
#include <sstream>

class HumanPlayer : public Player
{
public:
    HumanPlayer(std::string name, int boardSize);
    ~HumanPlayer() override = default;

    // Конструктор копирования с вызовом базового класса
    HumanPlayer(const HumanPlayer& other);

    // Перегрузка оператора присваивания
    HumanPlayer& operator=(const HumanPlayer& other);

    // Виртуальный метод для клонирования (теперь переопределяет базовый)
    HumanPlayer* Clone() const override;

    void PlaceShips() override;
    MoveType MakeMove() override;

    static const int MAX_ATTEMPTS = 100;
    GameBoard::ShipSizesType shipSizes;

private:
    void DisplayBoardState();
    void ManualPlacement();
    void AutomaticPlacement();
    bool TryPlaceShip(int size, int row, int col, bool horizontal);
    int GetValidatedInput(const std::string& prompt, int minValue, int maxValue);

    // Новый метод для работы со строками
    std::string GeneratePlacementMessage(int shipSize) const;
};