using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3GameCS
{
    /// <summary>
    /// Класс для управления игровой сеткой плиток
    /// Создает, размещает и управляет состоянием плиток на игровом поле
    /// </summary>
    public class GameGrid
    {
        // Константы для настроек по умолчанию
        public const int DEFAULT_GRID_SIZE = 16;
        public const int DEFAULT_TILE_SIZE = 34;

        // Статическое поле - количество созданных сеток
        private static int gridsCreated = 0;

        // Поля класса
        private Button[,] grid;
        private int gridSize;
        private int tileSize;
        private Random random;
        private Canvas backgroundPanel;
        private readonly Color[] colorPalette;

        /// <summary>
        /// Статическое свойство для отслеживания созданных сеток
        /// </summary>
        public static int GridsCreated => gridsCreated;

        /// <summary>
        /// Статический метод для получения информации о сетках
        /// </summary>
        public static string GetGridsInfo()
        {
            return $"Total grids created: {gridsCreated}";
        }

        /// <summary>
        /// Конструктор игровой сетки
        /// </summary>
        public GameGrid(Canvas background, int size, int tileSize)
        {
            try
            {
                if (background == null)
                    throw new ArgumentNullException(nameof(background));
                if (size <= 0)
                    throw new GameInitializationException("Grid size must be positive");
                if (tileSize <= 0)
                    throw new GameInitializationException("Tile size must be positive");

                backgroundPanel = background;
                GridSize = size;
                TileSize = tileSize;
                random = new Random();
                grid = new Button[GridSize, GridSize];

                colorPalette = new Color[]
                {
                    Color.FromArgb(255, 158, 25, 66),
                    Color.FromArgb(255, 244, 109, 67),
                    Color.FromArgb(255, 254, 224, 139),
                    Color.FromArgb(255, 147, 219, 135),
                    Color.FromArgb(255, 50, 136, 189),
                    Color.FromArgb(255, 94, 79, 162)
                };

                gridsCreated++; // Увеличиваем счетчик созданных сеток
                InitializeGrid();
            }
            catch (Exception ex)
            {
                throw new GameInitializationException("Failed to create game grid", ex);
            }
        }

        /// <summary>
        /// Инициализирует сетку - создает и размещает все плитки
        /// </summary>
        public void InitializeGrid()
        {
            try
            {
                for (int i = 0; i < GridSize; i++)
                {
                    for (int j = 0; j < GridSize; j++)
                    {
                        CreateTile(i, j);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Failed to initialize grid", ex);
            }
        }

        /// <summary>
        /// Создает отдельную плитку в указанной позиции сетки
        /// </summary>
        private void CreateTile(int i, int j)
        {
            try
            {
                var btn = new Button
                {
                    Width = TileSize,
                    Height = TileSize,
                    Margin = new Thickness(0),
                    Tag = new TilePosition(i, j),
                    Background = new SolidColorBrush(GetRandomColor()),
                    Content = "",
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1)
                };

                Canvas.SetLeft(btn, j * TileSize);
                Canvas.SetTop(btn, i * TileSize);

                backgroundPanel.Children.Add(btn);
                grid[i, j] = btn;
            }
            catch (Exception ex)
            {
                throw new TileOperationException($"Failed to create tile at position ({i}, {j})", ex);
            }
        }

        /// <summary>
        /// Генерирует случайный цвет для плитки из предопределенной палитры
        /// </summary>
        public Color GetRandomColor()
        {
            try
            {
                return colorPalette[random.Next(colorPalette.Length)];
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Failed to get random color", ex);
            }
        }

        /// <summary>
        /// Меняет цвета двух плиток местами
        /// </summary>
        public void SwapTiles(Button a, Button b)
        {
            try
            {
                if (a == null || b == null)
                    throw new ArgumentNullException(a == null ? nameof(a) : nameof(b));

                var temp = a.Background;
                a.Background = b.Background;
                b.Background = temp;
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Failed to swap tiles", ex);
            }
        }

        /// <summary>
        /// Заполняет пустые (прозрачные) плитки случайными цветами
        /// </summary>
        public void FillEmptyTile(Button btn)
        {
            try
            {
                if (btn == null)
                    throw new ArgumentNullException(nameof(btn));

                if (btn.Background == Brushes.Transparent)
                {
                    btn.Background = new SolidColorBrush(GetRandomColor());
                }
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Failed to fill empty tile", ex);
            }
        }

        /// <summary>
        /// Устанавливает обработчик клика для всех плиток сетки
        /// </summary>
        public void SetTileClickHandler(EventHandler<RoutedEventArgs> handler)
        {
            try
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                foreach (var tile in grid)
                {
                    if (tile != null)
                    {
                        tile.Click += handler;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Failed to set tile click handler", ex);
            }
        }

        /// <summary>
        /// Сбрасывает выделение плитки (восстанавливает стандартную рамку)
        /// </summary>
        public void ResetTileSelection(Button btn)
        {
            try
            {
                if (btn != null)
                {
                    btn.BorderThickness = new Thickness(1);
                    btn.BorderBrush = Brushes.Black;
                }
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Failed to reset tile selection", ex);
            }
        }

        /// <summary>
        /// Выполняет действие для каждой плитки в сетке
        /// </summary>
        public void ForEachTile(Action<Button> action)
        {
            try
            {
                if (action == null)
                    throw new ArgumentNullException(nameof(action));

                foreach (var tile in grid)
                {
                    if (tile != null)
                    {
                        action(tile);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TileOperationException("Failed to execute action for tiles", ex);
            }
        }

        // Свойства с валидацией

        /// <summary>
        /// Игровая сетка как IEnumerable (только для чтения)
        /// </summary>
        public IEnumerable<IEnumerable<Button>> Grid
        {
            get
            {
                try
                {
                    return grid.ToJaggedArray().Select(row => row.AsEnumerable());
                }
                catch (Exception ex)
                {
                    throw new TileOperationException("Failed to convert grid to enumerable", ex);
                }
            }
        }

        /// <summary>
        /// Цветовая палитра игры (только для чтения)
        /// </summary>
        public IEnumerable<Color> ColorPalette
        {
            get => colorPalette;
        }

        /// <summary>
        /// Размер игровой сетки
        /// </summary>
        public int GridSize
        {
            get => gridSize;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Grid size must be positive");
                gridSize = value;
            }
        }

        /// <summary>
        /// Размер плитки
        /// </summary>
        public int TileSize
        {
            get => tileSize;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Tile size must be positive");
                tileSize = value;
            }
        }

        /// <summary>
        /// Общее количество плиток в сетке
        /// </summary>
        public int TotalTiles => GridSize * GridSize;
    }
}