// UserInterface.hpp
#pragma once

#include "Player.hpp"
#include <string>
#include <memory>
#include <iostream>
#include <sstream>

class GameManager;

class UserInterface
{
public:
    UserInterface(GameManager* manager);
    ~UserInterface() = default;

    // Конструктор копирования
    UserInterface(const UserInterface& other);

    // Перегрузка оператора присваивания
    UserInterface& operator=(const UserInterface& other);

    void DisplayBoards(Player* player);
    void DisplayMessage(const std::string& message);
    void ShowGameOver(const std::string& winnerName);
    void DisplayLegend();

private:
    GameManager* m_gameManager;

    // метод для работы со строками
    std::string FormatBoardTitle(const std::string& title) const;
};