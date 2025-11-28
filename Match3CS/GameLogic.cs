using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3GameCS
{
    /// <summary>
    /// Состояния игрового процесса
    /// </summary>
    public enum GameState
    {
        Initializing = 0,    // Инициализация игрового поля
        Playing = 1,         // Игрок может делать ходы
        Processing = 2,      // Обработка совпадений (блокировка ввода)
        GameOver = 3         // Игра завершена
    }

    /// <summary>
    /// Основная игровая логика "Три в ряд"
    /// Отвечает за проверку совпадений, обработку ходов и управление состоянием игры
    /// </summary>
    public class GameLogic
    {
        private bool initializing;
        private GameState currentState;
        private int maxMoves;
        private int movesLeft;

        // Статическое поле - количество созданных экземпляров игры
        private static int gamesCreated = 0;

        /// <summary>
        /// Статическое свойство для отслеживания созданных игр
        /// </summary>
        public static int GamesCreated => gamesCreated;

        /// <summary>
        /// Статический метод для получения информации о играх
        /// </summary>
        public static string GetGamesInfo()
        {
            return $"Total games created: {gamesCreated}";
        }

        /// <summary>
        /// Конструктор игровой логики
        /// </summary>
        public GameLogic()
        {
            initializing = true;
            currentState = GameState.Initializing;
            maxMoves = 50;
            movesLeft = maxMoves;

            gamesCreated++; // Увеличиваем счетчик созданных игр
        }

        /// <summary>
        /// Конструктор с настройкой максимального количества ходов
        /// </summary>
        public GameLogic(int maxMoves) : this()
        {
            if (maxMoves <= 0)
                throw new GameInitializationException("Max moves must be positive");

            this.maxMoves = maxMoves;
            this.movesLeft = maxMoves;
        }

        /// <summary>
        /// Проверяет и удаляет совпадения (3+ в ряд по горизонтали или вертикали)
        /// </summary>
        /// <param name="grid">Игровая сетка с плитками</param>
        /// <param name="matchedTiles">Выходной параметр - матрица совпавших плиток</param>
        /// <returns>true если найдены совпадения, иначе false</returns>
        public bool CheckMatches(IEnumerable<IEnumerable<Button>> grid, out bool[,] matchedTiles)
        {
            try
            {
                if (grid == null)
                    throw new ArgumentNullException(nameof(grid));

                var gridArray = grid.To2DArray();
                int gridSize = gridArray.GetLength(0);

                // Инициализируем матрицу для отметки совпавших плиток
                matchedTiles = new bool[gridSize, gridSize];
                bool found = false;

                // Проверяем совпадения в обоих направлениях
                CheckHorizontalMatches(gridArray, gridSize, matchedTiles);
                CheckVerticalMatches(gridArray, gridSize, matchedTiles);
                found = HasMatches(gridSize, matchedTiles);

                // Если найдены совпадения - обрабатываем их
                if (found)
                {
                    RemoveMatchedTiles(gridArray, gridSize, matchedTiles);
                    DropTiles(gridArray, gridSize);
                }

                return found;
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Error checking matches", ex);
            }
        }

        /// <summary>
        /// Проверяет горизонтальные совпадения (3+ плиток одного цвета в ряд)
        /// </summary>
        private void CheckHorizontalMatches(Button[,] grid, int gridSize, bool[,] matched)
        {
            try
            {
                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize - 2; j++)
                    {
                        var color1 = GetButtonColor(grid[i, j]);
                        var color2 = GetButtonColor(grid[i, j + 1]);
                        var color3 = GetButtonColor(grid[i, j + 2]);

                        if (color1.HasValue && color1 == color2 && color1 == color3)
                        {
                            matched[i, j] = true;
                            matched[i, j + 1] = true;
                            matched[i, j + 2] = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Error in horizontal match checking", ex);
            }
        }

        /// <summary>
        /// Проверяет вертикальные совпадения (3+ плиток одного цвета в столбец)
        /// </summary>
        private void CheckVerticalMatches(Button[,] grid, int gridSize, bool[,] matched)
        {
            try
            {
                for (int i = 0; i < gridSize - 2; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        var color1 = GetButtonColor(grid[i, j]);
                        var color2 = GetButtonColor(grid[i + 1, j]);
                        var color3 = GetButtonColor(grid[i + 2, j]);

                        if (color1.HasValue && color1 == color2 && color1 == color3)
                        {
                            matched[i, j] = true;
                            matched[i + 1, j] = true;
                            matched[i + 2, j] = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Error in vertical match checking", ex);
            }
        }

        /// <summary>
        /// Проверяет, есть ли в матрице отмеченные совпадения
        /// </summary>
        private bool HasMatches(int gridSize, bool[,] matched)
        {
            foreach (bool isMatched in matched)
            {
                if (isMatched)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Удаляет плитки, отмеченные как совпавшие (делает их прозрачными)
        /// </summary>
        private void RemoveMatchedTiles(Button[,] grid, int gridSize, bool[,] matched)
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (matched[i, j])
                    {
                        SetButtonColor(grid[i, j], null);
                    }
                }
            }
        }

        /// <summary>
        /// Осуществляет падение плиток после удаления совпадений
        /// Плитки "падают" вниз, заполняя образовавшиеся пустоты
        /// </summary>
        private void DropTiles(Button[,] grid, int gridSize)
        {
            try
            {
                for (int j = 0; j < gridSize; j++)
                {
                    int emptyRow = gridSize - 1;

                    for (int i = gridSize - 1; i >= 0; i--)
                    {
                        if (GetButtonColor(grid[i, j]).HasValue)
                        {
                            if (i != emptyRow)
                            {
                                SetButtonColor(grid[emptyRow, j], GetButtonColor(grid[i, j]));
                                SetButtonColor(grid[i, j], null);
                            }
                            emptyRow--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Error in tile dropping", ex);
            }
        }

        /// <summary>
        /// Обрабатывает цепную реакцию совпадений до их полного отсутствия
        /// </summary>
        /// <returns>Общее количество удаленных плиток за всю цепную реакцию</returns>
        public int ProcessMatches(IEnumerable<IEnumerable<Button>> grid, GameGrid gameGrid)
        {
            try
            {
                if (grid == null || gameGrid == null)
                    throw new ArgumentNullException(grid == null ? nameof(grid) : nameof(gameGrid));

                CurrentState = GameState.Processing;
                int totalRemoved = 0;
                bool changed;

                do
                {
                    gameGrid.ForEachTile(btn => gameGrid.FillEmptyTile(btn));
                    changed = CheckMatches(grid, out bool[,] matchedTiles);

                    if (changed)
                    {
                        int stepRemoved = CountRemovedTiles(matchedTiles);
                        totalRemoved += stepRemoved;
                    }
                } while (changed);

                CurrentState = GameState.Playing;
                Initializing = false;

                return totalRemoved;
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Error processing matches", ex);
            }
        }

        /// <summary>
        /// Проверяет, являются ли две плитки соседними (по горизонтали или вертикали)
        /// </summary>
        public bool AreAdjacent(Button a, Button b)
        {
            try
            {
                if (a?.Tag is not TilePosition pos1 || b?.Tag is not TilePosition pos2)
                    return false;

                int dx = Math.Abs(pos1.X - pos2.X);
                int dy = Math.Abs(pos1.Y - pos2.Y);

                return (dx + dy) == 1;
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Error checking tile adjacency", ex);
            }
        }

        /// <summary>
        /// Обрабатывает попытку обмена двух плиток
        /// </summary>
        /// <returns>Количество удаленных плиток в результате обмена</returns>
        public int HandleTileSwap(Button tile1, Button tile2,
            IEnumerable<IEnumerable<Button>> grid, GameGrid gameGrid)
        {
            try
            {
                if (tile1 == null || tile2 == null)
                    throw new ArgumentNullException(tile1 == null ? nameof(tile1) : nameof(tile2));

                if (AreAdjacent(tile1, tile2))
                {
                    // Используем ход
                    UseMove();

                    gameGrid.SwapTiles(tile1, tile2);

                    if (CheckMatches(grid, out bool[,] matchedTiles))
                    {
                        int removedCount = CountRemovedTiles(matchedTiles);
                        removedCount += ProcessMatches(grid, gameGrid);
                        return removedCount;
                    }
                    else
                    {
                        gameGrid.SwapTiles(tile1, tile2);
                        return 0;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Error handling tile swap", ex);
            }
        }

        /// <summary>
        /// Использует один ход
        /// </summary>
        private void UseMove()
        {
            MovesLeft--;

            if (MovesLeft <= 0)
            {
                CurrentState = GameState.GameOver;
                throw new GameInitializationException("No moves left - game over!");
            }
        }

        /// <summary>
        /// Подсчитывает количество удаленных плиток в матрице совпадений
        /// </summary>
        private int CountRemovedTiles(bool[,] matched)
        {
            return matched.Cast<bool>().Count(isMatched => isMatched);
        }

        /// <summary>
        /// Вспомогательный метод для получения цвета плитки
        /// </summary>
        private Color? GetButtonColor(Button button)
        {
            if (button?.Background is SolidColorBrush brush)
            {
                return brush.Color;
            }
            return null;
        }

        /// <summary>
        /// Вспомогательный метод для установки цвета плитки
        /// </summary>
        private void SetButtonColor(Button button, Color? color)
        {
            button.Background = color.HasValue
                ? new SolidColorBrush(color.Value)
                : Brushes.Transparent;
        }

        // Свойства с валидацией

        /// <summary>
        /// Текущее состояние игры
        /// </summary>
        public GameState CurrentState
        {
            get => currentState;
            set
            {
                if (!Enum.IsDefined(typeof(GameState), value))
                    throw new ArgumentException("Invalid game state");
                currentState = value;
            }
        }

        /// <summary>
        /// Флаг начальной инициализации игры
        /// </summary>
        public bool Initializing
        {
            get => initializing;
            set => initializing = value;
        }

        /// <summary>
        /// Оставшееся количество ходов
        /// </summary>
        public int MovesLeft
        {
            get => movesLeft;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Moves cannot be negative");
                movesLeft = value;
            }
        }

        /// <summary>
        /// Максимальное количество ходов
        /// </summary>
        public int MaxMoves
        {
            get => maxMoves;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Max moves must be positive");
                maxMoves = value;
            }
        }

        /// <summary>
        /// Прогресс игры в процентах
        /// </summary>
        public double GameProgress
        {
            get => (double)(maxMoves - movesLeft) / maxMoves * 100.0;
        }
    }

    /// <summary>
    /// Запись для хранения координат плитки в сетке
    /// </summary>
    /// <param name="X">Координата X (строка)</param>
    /// <param name="Y">Координата Y (столбец)</param>
    public record TilePosition(int X, int Y);
}