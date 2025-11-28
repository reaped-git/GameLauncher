#pragma once

#include "Player.hpp"
#include "HumanPlayer.hpp"
#include "AIPlayer.hpp"
#include <memory>

class UserInterface;

class GameManager
{
public:
    GameManager(int boardSize);
    ~GameManager() = default; // Деструктор по умолчанию, т.к. unique_ptr сам удалит объекты

    // Удаляем конструктор копирования и оператор присваивания, 
    // т.к. unique_ptr нельзя копировать
    GameManager(const GameManager& other) = delete;
    GameManager& operator=(const GameManager& other) = delete;

    void SetupGame();
    void RunGameLoop();
    void SwitchTurn();
    void DisplayGameState();

    // Возвращаем сырые указатели для совместимости с существующим кодом
    Player* GetCurrentPlayer() const { return m_currentPlayer; }
    Player* GetPlayer1() const { return m_player1.get(); }
    Player* GetPlayer2() const { return m_player2.get(); }

private:
    std::unique_ptr<Player> m_player1;
    std::unique_ptr<Player> m_player2;
    Player* m_currentPlayer; // Сырой указатель для текущего игрока (не владеющий)
    bool m_gameOver;
    std::unique_ptr<UserInterface> m_userInterface;
};