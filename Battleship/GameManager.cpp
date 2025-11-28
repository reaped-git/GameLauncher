// GameManager.cpp
#include "GameManager.hpp"
#include "UserInterface.hpp"
#include <iostream>
#include <sstream>

GameManager::GameManager(int boardSize)
    : m_gameOver(false)
    , m_userInterface(std::make_unique<UserInterface>(this))
{
    // Создаем игроков через умные указатели
    m_player1 = std::make_unique<HumanPlayer>("Игрок 1", boardSize);
    m_player2 = std::make_unique<AIPlayer>("Компьютер", boardSize);

    // Текущий игрок - просто указатель (не владеющий)
    m_currentPlayer = m_player1.get();

    // Устанавливаем ссылки на поля противников через shared_ptr
    m_player1->SetEnemyBoard(std::make_shared<GameBoard>(m_player2->GetMyBoard()));
    m_player2->SetEnemyBoard(std::make_shared<GameBoard>(m_player1->GetMyBoard()));
}

void GameManager::SetupGame()
{
    std::cout << "Расстановка кораблей для " << m_player1->GetName() << ":\n";
    m_player1->PlaceShips();

    std::cout << "Расстановка кораблей для " << m_player2->GetName() << ":\n";
    m_player2->PlaceShips();

    std::cout << "Игра начинается!\n";
}

void GameManager::RunGameLoop()
{
    while (!m_gameOver)
    {
        // Используем dynamic_cast с сырыми указателями
        AIPlayer* aiPlayer = dynamic_cast<AIPlayer*>(m_currentPlayer);

        // Показываем состояние после хода
        if (!aiPlayer)
        {
            DisplayGameState();
        }

        // Ход текущего игрока
        Player::MoveType move = m_currentPlayer->MakeMove();

        // Обработка выстрела - получаем shared_ptr и разыменовываем
        std::shared_ptr<GameBoard> enemyBoard = m_currentPlayer->GetEnemyBoard();
        Ship::ShotResult result = enemyBoard->ReceiveShot(move);

        // Обновление состояния ИИ если нужно
        if (aiPlayer)
        {
            aiPlayer->UpdateAIState(result, move);
        }

        // Отображение результата
        std::cout << m_currentPlayer->GetName() << " стреляет в ("
            << move.first << ", " << move.second << ") - ";

        switch (result)
        {
        case Ship::ShotResult::eHit:
            std::cout << "ПОПАДАНИЕ!\n";
            break;
        case Ship::ShotResult::eSunk:
            std::cout << "КОРАБЛЬ ПОТОПЛЕН!\n";
            break;
        case Ship::ShotResult::eMiss:
            std::cout << "ПРОМАХ!\n";
            break;
        case Ship::ShotResult::eAlreadyShot:
            std::cout << "Уже стреляли сюда!\n";
            break;
        }

        // Показываем состояние после хода
        if (!aiPlayer)
        {
            DisplayGameState();
        }

        // Проверка окончания игры
        if (enemyBoard->IsAllShipsSunk())
        {
            m_gameOver = true;
            std::cout << "\n=== ИГРА ОКОНЧЕНА ===\n";
            std::cout << m_currentPlayer->GetName() << " ПОБЕДИЛ!\n";
            m_userInterface->ShowGameOver(m_currentPlayer->GetName());
            break;
        }

        // Смена хода если не попадание
        if (result != Ship::ShotResult::eHit && result != Ship::ShotResult::eSunk)
        {
            SwitchTurn();
        }

        // Пауза для удобства восприятия
        if (!aiPlayer)
        {
            std::cout << "Нажмите Enter для продолжения...";
            std::cin.ignore();
            std::cin.get();
        }
    }
}

void GameManager::SwitchTurn()
{
    if (m_currentPlayer == m_player1.get())
    {
        m_currentPlayer = m_player2.get();
    }
    else
    {
        m_currentPlayer = m_player1.get();
    }
}

void GameManager::DisplayGameState()
{
    std::cout << "\n";
    m_userInterface->DisplayBoards(m_currentPlayer);
}