#pragma once

namespace GameLauncher {

    using namespace System;
    using namespace System::Windows::Forms;

    ref class GameLogic; // Forward declaration

    // Делегейт для события выбора плитки
    /// <summary>
    /// Делегейт для обработки события выбора плитки
    /// </summary>
    /// <param name="tile">Выбранная плитка</param>
    public delegate Void TileSelectedHandler(Button^ tile);

    /// <summary>
    /// Класс обработки ввода пользователя - управляет выбором и обменом плиток
    /// </summary>
    public ref class InputHandler
    {
    public:
        /// <summary>
        /// Событие выбора плитки (для визуальной обратной связи)
        /// </summary>
        event TileSelectedHandler^ OnTileSelected;

    private:
        Button^ selectedTile1;    // Первая выбранная плитка
        Button^ selectedTile2;    // Вторая выбранная плитка
        GameLogic^ gameLogic;     // Ссылка на игровую логику для проверки допустимости ходов

    public:
        /// <summary>
        /// Конструктор обработчика ввода
        /// </summary>
        /// <param name="logic">Игровая логика для проверки ходов</param>
        InputHandler(GameLogic^ logic);

        /// <summary>
        /// Деструктор
        /// </summary>
        ~InputHandler();

        /// <summary>
        /// Основной обработчик кликов по плиткам
        /// </summary>
        Void HandleTileClick(Object^ sender, EventArgs^ e);

        /// <summary>
        /// Сбрасывает текущий выбор плиток
        /// </summary>
        Void ResetSelection();

    private:
        /// <summary>
        /// Обрабатывает логику выбора плитки (первая, вторая, отмена)
        /// </summary>
        Void ProcessTileSelection(Button^ clickedTile);

        /// <summary>
        /// Пытается обменять выбранные плитки
        /// </summary>
        Void TrySwapTiles();
    };

}