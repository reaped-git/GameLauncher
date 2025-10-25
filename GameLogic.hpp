#pragma once

namespace GameLauncher {

    using namespace System;
    using namespace System::Windows::Forms;
    using namespace System::Drawing;
    using namespace System::Threading;

    // Forward declaration
    ref class GameGrid;
    ref class ScoreManager;

    /// <summary>
    /// Класс игровой логики - отвечает за проверку совпадений, обработку ходов и управление состоянием игры
    /// </summary>
    public ref class GameLogic
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
        GameGrid^ gameGrid;         // Ссылка на игровую сетку
        ScoreManager^ scoreManager; // Ссылка на менеджер счета
        Boolean initializing;       // Флаг инициализации
        GameState currentState;     // Текущее состояние игры

    public:
        /// <summary>
        /// Конструктор игровой логики
        /// </summary>
        /// <param name="grid">Игровая сетка для управления</param>
        GameLogic(GameGrid^ grid, ScoreManager^ scoreMgr);


        /// <summary>
        /// Деструктор
        /// </summary>
        ~GameLogic();

        /// <summary>
        /// Проверяет и удаляет совпадения (3+ в ряд по горизонтали или вертикали)
        /// </summary>
        /// <returns>true если найдены совпадения, иначе false</returns>
        Boolean CheckMatches();                       

        /// <summary>
        /// Обрабатывает цепную реакцию совпадений до их полного отсутствия
        /// </summary>
        Void ProcessMatches();         

        /// <summary>
        /// Проверяет, являются ли две плитки соседними
        /// </summary>
        Boolean AreAdjacent(Button^ a, Button^ b);                     

        /// <summary>
        /// Обрабатывает попытку обмена двух плиток
        /// </summary>
        Void HandleTileSwap(Button^ tile1, Button^ tile2);          

        /// <summary>
        /// Возвращает текущее состояние игры
        /// </summary>
        GameState GetCurrentState() { return currentState; }

        /// <summary>
        /// Устанавливает состояние игры
        /// </summary>
        Void SetCurrentState (GameState state) { currentState = state; }

    private:
        /// <summary>
        /// Проверяет горизонтальные совпадения
        /// </summary>
        Void CheckHorizontalMatches(array<array<Boolean>^>^ matched);  

        /// <summary>
        /// Проверяет вертикальные совпадения
        /// </summary>
        Void CheckVerticalMatches(array<array<Boolean>^>^ matched);    

        /// <summary>
        /// Проверяет, есть ли в матрице отмеченные совпадения
        /// </summary>
        /// <param name="matched">Матрица совпадений</param>
        /// <returns>true если есть хотя бы одно совпадение</returns>
        Boolean HasMatches(array<array<Boolean>^>^ matched);           

        /// <summary>
        /// Удаляет плитки, отмеченные как совпавшие
        /// </summary>
        Void RemoveMatchedTiles(array<array<Boolean>^>^ matched);      

        /// <summary>
        /// Осуществляет падение плиток после удаления совпадений
        /// </summary>
        Void DropTiles();                                            
    };

}