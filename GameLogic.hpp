#pragma once

#include "GameGrid.hpp"
#include "ScoreManager.hpp"

namespace GameLauncher {

    using namespace System;
    using namespace System::Windows::Forms;
    using namespace System::Drawing;
    using namespace System::Threading;
    using namespace System::ComponentModel;
    using namespace System::Collections;
    using namespace System::Data;
    using namespace System::Drawing;

    /// <summary>
    /// Класс игровой логики - отвечает за проверку совпадений, обработку ходов и управление состоянием игры
    /// </summary>
    class GameLogic
    {
    public:
        /// <summary>
        /// Состояния игры
        /// </summary>
        enum class GameState
        {
            eInitializing = 0,      // Игра инициализируется
            ePlaying = 1,           // Игрок может делать ходы
            eProcessing = 2,        // Игра обрабатывает совпадения (блокирует ввод)
        };

    private:
        Boolean initializing;       // Флаг инициализации
        GameState currentState;     // Текущее состояние игры

    public:
        /// <summary>
        /// Конструктор игровой логики
        /// </summary>
        GameLogic();

        /// <summary>
        /// Деструктор
        /// </summary>
        ~GameLogic() = default;

        /// <summary>
        /// Проверяет и удаляет совпадения (3+ в ряд по горизонтали или вертикали)
        /// </summary>
        /// <param name="matchedTiles">Возвращает информацию о совпавших плитках</param>
        /// <returns>true если найдены совпадения, иначе false</returns>
        Boolean CheckMatches(array<Button^, 2>^ grid, Int64 gridSize, array<array<Boolean>^>^% matchedTiles);

        /// <summary>
        /// Обрабатывает цепную реакцию совпадений до их полного отсутствия
        /// </summary>
        Int64 ProcessMatches(array<Button^, 2>^ grid, Int64 gridSize, GameGrid^ gameGrid);

        /// <summary>
        /// Проверяет, являются ли две плитки соседними
        /// </summary>
        Boolean AreAdjacent(Button^ a, Button^ b);

        /// <summary>
        /// Обрабатывает попытку обмена двух плиток
        /// </summary>
        Int64 HandleTileSwap(Button^ tile1, Button^ tile2, array<Button^, 2>^ grid, Int64 gridSize, GameGrid^ gameGrid);

        /// <summary>
        /// Возвращает текущее состояние игры
        /// </summary>
        GameState GetCurrentState() { return currentState; }

        /// <summary>
        /// Устанавливает состояние игры
        /// </summary>
        Void SetCurrentState(GameState state) { currentState = state; }

    private:
        /// <summary>
        /// Проверяет горизонтальные совпадения
        /// </summary>
        Void CheckHorizontalMatches(array<Button^, 2>^ grid, Int64 gridSize, array<array<Boolean>^>^ matched);

        /// <summary>
        /// Проверяет вертикальные совпадения
        /// </summary>
        Void CheckVerticalMatches(array<Button^, 2>^ grid, Int64 gridSize, array<array<Boolean>^>^ matched);

        /// <summary>
        /// Проверяет, есть ли в матрице отмеченные совпадения
        /// </summary>
        /// <param name="matched">Матрица совпадений</param>
        /// <returns>true если есть хотя бы одно совпадение</returns>
        Boolean HasMatches(Int64 gridSize, array<array<Boolean>^>^ matched);

        /// <summary>
        /// Удаляет плитки, отмеченные как совпавшие
        /// </summary>
        Void RemoveMatchedTiles(array<Button^, 2>^ grid, Int64 gridSize, array<array<Boolean>^>^ matched);

        /// <summary>
        /// Осуществляет падение плиток после удаления совпадений
        /// </summary>
        Void DropTiles(array<Button^, 2>^ grid, Int64 gridSize);

        /// <summary>
        /// Подсчитывает количество удаленных плиток
        /// </summary>
        Int64 CountRemovedTiles(array<array<Boolean>^>^ matched, Int64 gridSize);
    };
}
