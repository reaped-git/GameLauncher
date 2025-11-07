using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Match3GameCS
{
    /// <summary>
    /// Главное окно игры "Три в ряд"
    /// Содержит игровое поле, управляет взаимодействием всех компонентов игры
    /// Реализует кастомный UI без стандартной рамки окна
    /// </summary>
    public partial class MainWindow : Window
    {
        // Игровые компоненты
        private GameGrid gameGrid;           // Управление игровой сеткой
        private GameLogic gameLogic;         // Логика игры
        private InputHandler inputHandler;   // Обработка ввода
        private ScoreManager scoreManager;   // Управление очками

        // Переменные для перетаскивания окна
        private bool dragging;               // Флаг перетаскивания окна
        private Point startPoint;            // Начальная точка перетаскивания

        /// <summary>
        /// Конструктор главного окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();  // Включаем инструменты разработчика в режиме отладки
#endif
            InitializeGame();
        }

        /// <summary>
        /// Инициализация игровых компонентов
        /// </summary>
        private void InitializeGame()
        {
            // Создаем игровые компоненты
            gameGrid = new GameGrid(GameArea, GameGrid.DEFAULT_GRID_SIZE, GameGrid.DEFAULT_TILE_SIZE);
            scoreManager = new ScoreManager();
            gameLogic = new GameLogic();
            inputHandler = new InputHandler();

            // Настраиваем обработчики событий для плиток
            SetupTileEventHandlers();

            // Запускаем начальную обработку совпадений (убираем начальные комбо)
            gameLogic.ProcessMatches(gameGrid.GetGrid(), gameGrid.GetSize(), gameGrid);
        }

        /// <summary>
        /// Настраивает обработчики событий для всех плиток
        /// </summary>
        private void SetupTileEventHandlers()
        {
            gameGrid.SetTileClickHandler(OnTileClicked);
        }

        /// <summary>
        /// Обработчик клика по плитке
        /// </summary>
        private void OnTileClicked(object sender, RoutedEventArgs e)
        {
            // Обрабатываем клик и получаем результат
            var result = inputHandler.HandleTileClick(sender, gameLogic);

            // Обрабатываем различные сценарии в зависимости от результата
            switch (result)
            {
                case TileClickResult.FirstSelected:
                    // Выделяем первую плитку
                    gameGrid.ForEachTile(btn => gameGrid.ResetTileSelection(btn));
                    var firstTile = inputHandler.GetFirstSelectedTile();
                    if (firstTile != null)
                    {
                        firstTile.BorderThickness = new Thickness(2);    // Утолщенная рамка
                        firstTile.BorderBrush = Brushes.Black;          // Черный цвет рамки
                    }
                    break;

                case TileClickResult.SecondSelected:
                    // Пытаемся обменять две выбранные плитки
                    var tile1 = inputHandler.GetFirstSelectedTile();
                    var tile2 = inputHandler.GetSecondSelectedTile();

                    if (tile1 != null && tile2 != null)
                    {
                        // Обрабатываем обмен плиток
                        int removedTiles = gameLogic.HandleTileSwap(
                            tile1, tile2, gameGrid.GetGrid(), gameGrid.GetSize(), gameGrid);

                        // Если обмен успешен (удалены плитки) - начисляем очки
                        if (removedTiles > 0)
                        {
                            string score = scoreManager.AddScoreForTiles(removedTiles);
                            UpdateScoreDisplay(score);
                        }
                    }

                    // Сбрасываем выбор после обработки обмена
                    inputHandler.ResetSelection();
                    gameGrid.ForEachTile(btn => gameGrid.ResetTileSelection(btn));
                    break;

                case TileClickResult.Deselected:
                    // Снимаем выделение со всех плиток
                    gameGrid.ForEachTile(btn => gameGrid.ResetTileSelection(btn));
                    break;
            }
        }

        /// <summary>
        /// Обновляет отображение счета на форме
        /// </summary>
        /// <param name="score">Строка счета для отображения</param>
        private void UpdateScoreDisplay(string score)
        {
            ScoreLabel.Text = score;
        }

        // Обработчики событий UI элементов

        /// <summary>
        /// Обработчик нажатия кнопки закрытия приложения
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Закрываем приложение
        }

        /// <summary>
        /// Обработчик нажатия кнопки сворачивания окна
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized; // Сворачиваем окно
        }

        /// <summary>
        /// Обработчик наведения курсора на кнопку закрытия
        /// </summary>
        private void CloseButton_PointerEntered(object sender, PointerEventArgs e)
        {
            if (sender is Button button)
            {
                button.Background = new SolidColorBrush(Colors.LightCoral); // Подсвечиваем красным
            }
        }

        /// <summary>
        /// Обработчик ухода курсора с кнопки закрытия
        /// </summary>
        private void CloseButton_PointerExited(object sender, PointerEventArgs e)
        {
            if (sender is Button button)
            {
                button.Background = new SolidColorBrush(Colors.White); // Возвращаем белый цвет
            }
        }

        /// <summary>
        /// Обработчик наведения курсора на кнопку сворачивания
        /// </summary>
        private void MinimizeButton_PointerEntered(object sender, PointerEventArgs e)
        {
            if (sender is Button button)
            {
                button.Background = new SolidColorBrush(Colors.LightGray); // Подсвечиваем серым
            }
        }

        /// <summary>
        /// Обработчик ухода курсора с кнопки сворачивания
        /// </summary>
        private void MinimizeButton_PointerExited(object sender, PointerEventArgs e)
        {
            if (sender is Button button)
            {
                button.Background = new SolidColorBrush(Colors.White); // Возвращаем белый цвет
            }
        }

        /// <summary>
        /// Обработчик начала перетаскивания окна
        /// </summary>
        private void ToolBar_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                dragging = true;                        // Включаем режим перетаскивания
                startPoint = e.GetPosition(this);       // Запоминаем начальную позицию
            }
        }

        /// <summary>
        /// Обработчик перемещения окна при перетаскивании
        /// </summary>
        private void ToolBar_PointerMoved(object sender, PointerEventArgs e)
        {
            if (dragging && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                var currentPoint = e.GetPosition(null);
                // Вычисляем новую позицию окна
                Position = new PixelPoint(
                    (int)(Position.X + currentPoint.X - startPoint.X),
                    (int)(Position.Y + currentPoint.Y - startPoint.Y)
                );
            }
        }

        /// <summary>
        /// Обработчик окончания перетаскивания окна
        /// </summary>
        private void ToolBar_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            dragging = false; // Выключаем режим перетаскивания
        }
    }
}