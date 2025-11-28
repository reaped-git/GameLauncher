using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Threading.Tasks;

namespace Match3GameCS
{
    public partial class MainWindow : Window
    {
        private GameGrid gameGrid;
        private GameLogic gameLogic;
        private InputHandler inputHandler;
        private ScoreManager scoreManager;
        private bool dragging;
        private Point startPoint;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            InitializeGame();
        }

        /// <summary>
        /// Инициализация игровых компонентов с обработкой исключений
        /// </summary>
        private void InitializeGame()
        {
            try
            {
                // Создаем игровые компоненты
                gameGrid = new GameGrid(GameArea, GameGrid.DEFAULT_GRID_SIZE, GameGrid.DEFAULT_TILE_SIZE);
                scoreManager = new ScoreManager("Player1");
                gameLogic = new GameLogic(30); // 30 ходов
                inputHandler = new InputHandler();

                // Настраиваем обработчики событий для плиток
                SetupTileEventHandlers();

                // Обновляем информацию о играх
                UpdateGamesInfo();
                UpdateMovesDisplay();

                // Запускаем начальную обработку совпадений
                gameLogic.ProcessMatches(gameGrid.Grid, gameGrid);
            }
            catch (GameInitializationException ex)
            {
                ShowErrorDialog("Game Initialization Error", ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Unexpected Error", $"Failed to initialize game: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновляет информацию о созданных играх
        /// </summary>
        private void UpdateGamesInfo()
        {
            try
            {
                GamesInfoTextBlock.Text = $"Games: {GameLogic.GamesCreated} | Grids: {GameGrid.GridsCreated}";
                GlobalScoreLabel.Text = ScoreManager.GetGlobalStats();
            }
            catch (Exception ex)
            {
                // Игнорируем ошибки обновления информации, чтобы не прерывать игру
                System.Diagnostics.Debug.WriteLine($"Error updating game info: {ex.Message}");
            }
        }

        /// <summary>
        /// Настраивает обработчики событий для всех плиток
        /// </summary>
        private void SetupTileEventHandlers()
        {
            try
            {
                gameGrid.SetTileClickHandler(OnTileClicked);
            }
            catch (TileOperationException ex)
            {
                ShowErrorDialog("Tile Handler Error", ex.Message);
            }
        }

        /// <summary>
        /// Обработчик клика по плитке с обработкой исключений
        /// </summary>
        private void OnTileClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = inputHandler.HandleTileClick(sender, gameLogic);

                switch (result)
                {
                    case TileClickResult.FirstSelected:
                        gameGrid.ForEachTile(btn => gameGrid.ResetTileSelection(btn));
                        var firstTile = inputHandler.SelectedTile1;
                        if (firstTile != null)
                        {
                            firstTile.BorderThickness = new Thickness(2);
                            firstTile.BorderBrush = Brushes.Black;
                        }
                        break;

                    case TileClickResult.SecondSelected:
                        var tile1 = inputHandler.SelectedTile1;
                        var tile2 = inputHandler.SelectedTile2;

                        if (tile1 != null && tile2 != null)
                        {
                            int removedTiles = gameLogic.HandleTileSwap(tile1, tile2, gameGrid.Grid, gameGrid);

                            if (removedTiles > 0)
                            {
                                string score = scoreManager.AddScoreForTiles(removedTiles);
                                UpdateScoreDisplay(score);
                                UpdateMovesDisplay();
                            }
                        }

                        inputHandler.ResetSelection();
                        gameGrid.ForEachTile(btn => gameGrid.ResetTileSelection(btn));
                        break;

                    case TileClickResult.Deselected:
                        gameGrid.ForEachTile(btn => gameGrid.ResetTileSelection(btn));
                        break;
                }
            }
            catch (GameInitializationException ex) when (ex.Message.Contains("game over"))
            {
                ShowGameOverDialog();
            }
            catch (TileOperationException ex)
            {
                ShowErrorDialog("Tile Operation Error", ex.Message);
            }
            catch (ScoreOperationException ex)
            {
                ShowErrorDialog("Score Error", ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Game Error", $"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновляет отображение счета на форме
        /// </summary>
        private void UpdateScoreDisplay(string score)
        {
            try
            {
                ScoreLabel.Text = score;
                GlobalScoreLabel.Text = ScoreManager.GetGlobalStats();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating score display: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновляет отображение оставшихся ходов
        /// </summary>
        private void UpdateMovesDisplay()
        {
            try
            {
                MovesLabel.Text = $"Moves: {gameLogic.MovesLeft}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating moves display: {ex.Message}");
            }
        }

        /// <summary>
        /// Показывает диалог окончания игры
        /// </summary>
        private async void ShowGameOverDialog()
        {
            try
            {
                // Сохраняем счет перед завершением
                scoreManager.SaveScoreToFile("scores.txt");

                var dialog = new Window
                {
                    Title = "Game Over",
                    Width = 300,
                    Height = 200,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                var stackPanel = new StackPanel
                {
                    Margin = new Thickness(20),
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                };

                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"Game Over!\nFinal Score: {scoreManager.CurrentScore}",
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = Avalonia.Media.TextAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 20),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                });

                var restartButton = new Button
                {
                    Content = "Restart Game",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    Width = 100
                };

                restartButton.Click += (s, e) =>
                {
                    RestartGame();
                    dialog.Close();
                };

                stackPanel.Children.Add(restartButton);
                dialog.Content = stackPanel;

                await dialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Save Error", $"Failed to save score: {ex.Message}");
            }
        }

        /// <summary>
        /// Перезапускает игру
        /// </summary>
        private void RestartGame()
        {
            try
            {
                // Очищаем игровое поле
                GameArea.Children.Clear();

                // Пересоздаем игровые компоненты
                InitializeGame();

                // Сбрасываем отображение
                UpdateScoreDisplay("Score: 0");
                UpdateMovesDisplay();
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Restart Error", $"Failed to restart game: {ex.Message}");
            }
        }

        /// <summary>
        /// Показывает диалог ошибки
        /// </summary>
        private async void ShowErrorDialog(string title, string message)
        {
            try
            {
                var dialog = new Window
                {
                    Title = title,
                    Width = 400,
                    Height = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                var textBlock = new TextBlock
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(20),
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                };

                dialog.Content = textBlock;
                await dialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                // Если даже диалог ошибки не работает, выводим в консоль
                System.Diagnostics.Debug.WriteLine($"Critical error: {title} - {message}");
                System.Diagnostics.Debug.WriteLine($"Dialog error: {ex.Message}");
            }
        }

        // Обработчики событий UI элементов

        /// <summary>
        /// Обработчик нажатия кнопки закрытия приложения
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Close Error", $"Failed to close application: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки сворачивания окна
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Minimize Error", $"Failed to minimize window: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик наведения курсора на кнопку закрытия
        /// </summary>
        private void CloseButton_PointerEntered(object sender, PointerEventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    button.Background = new SolidColorBrush(Colors.LightCoral);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CloseButton_PointerEntered: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик ухода курсора с кнопки закрытия
        /// </summary>
        private void CloseButton_PointerExited(object sender, PointerEventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    button.Background = new SolidColorBrush(Colors.White);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CloseButton_PointerExited: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик наведения курсора на кнопку сворачивания
        /// </summary>
        private void MinimizeButton_PointerEntered(object sender, PointerEventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    button.Background = new SolidColorBrush(Colors.LightGray);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in MinimizeButton_PointerEntered: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик ухода курсора с кнопки сворачивания
        /// </summary>
        private void MinimizeButton_PointerExited(object sender, PointerEventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    button.Background = new SolidColorBrush(Colors.White);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in MinimizeButton_PointerExited: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик начала перетаскивания окна
        /// </summary>
        private void ToolBar_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            try
            {
                if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                {
                    dragging = true;
                    startPoint = e.GetPosition(this);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ToolBar_PointerPressed: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик перемещения окна при перетаскивании
        /// </summary>
        private void ToolBar_PointerMoved(object sender, PointerEventArgs e)
        {
            try
            {
                if (dragging && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                {
                    var currentPoint = e.GetPosition(null);
                    Position = new PixelPoint(
                        (int)(Position.X + currentPoint.X - startPoint.X),
                        (int)(Position.Y + currentPoint.Y - startPoint.Y)
                    );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ToolBar_PointerMoved: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик окончания перетаскивания окна
        /// </summary>
        private void ToolBar_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            try
            {
                dragging = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ToolBar_PointerReleased: {ex.Message}");
            }
        }
    }
}