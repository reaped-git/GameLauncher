using Avalonia.Controls;

namespace Match3GameCS
{
    /// <summary>
    /// Результат клика по плитке
    /// </summary>
    public enum TileClickResult
    {
        None = 0,           // Ничего не произошло
        FirstSelected = 1,  // Выбрана первая плитка
        SecondSelected = 2, // Выбрана вторая плитка (готов к обмену)
        Deselected = 3      // Снято выделение
    }

    /// <summary>
    /// Класс обработки ввода пользователя - управляет выбором и обменом плиток
    /// Обрабатывает логику выбора двух плиток для обмена
    /// </summary>
    public class InputHandler
    {
        // Поля для хранения выбранных плиток
        private Button selectedTile1;    // Первая выбранная плитка
        private Button selectedTile2;    // Вторая выбранная плитка

        /// <summary>
        /// Конструктор обработчика ввода
        /// </summary>
        public InputHandler()
        {
            // Изначально ни одна плитка не выбрана
            selectedTile1 = null;
            selectedTile2 = null;
        }

        /// <summary>
        /// Основной обработчик кликов по плиткам
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие (плитка)</param>
        /// <param name="gameLogic">Игровая логика для проверки состояния</param>
        /// <returns>Результат обработки клика</returns>
        public TileClickResult HandleTileClick(object sender, GameLogic gameLogic)
        {
            var clicked = (Button)sender;

            // Проверяем, можно ли сейчас делать ходы (только в состоянии Playing)
            if (gameLogic.CurrentState != GameState.Playing)
                return TileClickResult.None;

            // Обрабатываем выбор плитки
            return ProcessTileSelection(clicked);
        }

        /// <summary>
        /// Обрабатывает логику выбора плитки (первая, вторая, отмена)
        /// </summary>
        /// <param name="clickedTile">Плитка, по которой кликнули</param>
        /// <returns>Результат операции выбора</returns>
        private TileClickResult ProcessTileSelection(Button clickedTile)
        {
            // Сценарий 1: Первая плитка еще не выбрана
            if (selectedTile1 == null)
            {
                selectedTile1 = clickedTile;
                return TileClickResult.FirstSelected;
            }

            // Сценарий 2: Клик на уже выбранную плитку - отмена выбора
            else if (selectedTile1 == clickedTile)
            {
                ResetSelection();
                return TileClickResult.Deselected;
            }

            // Сценарий 3: Выбрана вторая плитка - готовы к обмену
            else
            {
                selectedTile2 = clickedTile;
                return TileClickResult.SecondSelected;
            }
        }

        /// <summary>
        /// Сбрасывает текущий выбор плиток
        /// </summary>
        public void ResetSelection()
        {
            selectedTile1 = null;
            selectedTile2 = null;
        }

        // Публичные свойства
        /// <summary>
        /// Первая выбранная плитка
        /// </summary>
        public Button SelectedTile1
        {
            get => selectedTile1;
            private set => selectedTile1 = value;
        }

        /// <summary>
        /// Вторая выбранная плитка
        /// </summary>
        public Button SelectedTile2
        {
            get => selectedTile2;
            private set => selectedTile2 = value;
        }
    }
}