using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;

namespace Match3GameCS
{
    /// <summary>
    /// Класс для управления игровой сеткой плиток
    /// Создает, размещает и управляет состоянием плиток на игровом поле
    /// </summary>
    public class GameGrid
    {
        // Константы для настроек по умолчанию
        public const int DEFAULT_GRID_SIZE = 16;  // Размер сетки 16x16
        public const int DEFAULT_TILE_SIZE = 34;  // Размер плитки 34x34 пикселей

        // Поля класса
        private Button[,] grid;           // Двумерный массив кнопок-плиток
        private int gridSize;             // Текущий размер сетки
        private int tileSize;             // Размер одной плитки в пикселях
        private Random random;            // Генератор случайных чисел для цветов
        private Canvas backgroundPanel;   // Родительский Canvas для размещения плиток
        private readonly Color[] colorPalette; // Цветовая палитра игры

        /// <summary>
        /// Конструктор игровой сетки
        /// </summary>
        /// <param name="background">Canvas для размещения плиток</param>
        /// <param name="size">Размер сетки (size x size)</param>
        /// <param name="tileSize">Размер одной плитки в пикселях</param>
        public GameGrid(Canvas background, int size, int tileSize)
        {
            backgroundPanel = background;
            gridSize = size;
            this.tileSize = tileSize;
            random = new Random();
            grid = new Button[gridSize, gridSize];

            // Инициализация цветовой палитры (те же цвета, что в оригинальной игре)
            colorPalette = new Color[]
            {
                Color.FromArgb(255, 158, 25, 66),    // Темно-красный
                Color.FromArgb(255, 244, 109, 67),   // Оранжевый
                Color.FromArgb(255, 254, 224, 139),  // Светло-желтый
                Color.FromArgb(255, 147, 219, 135),  // Светло-зеленый
                Color.FromArgb(255, 50, 136, 189),   // Синий
                Color.FromArgb(255, 94, 79, 162)     // Фиолетовый
            };

            InitializeGrid();
        }

        /// <summary>
        /// Инициализирует сетку - создает и размещает все плитки
        /// </summary>
        public void InitializeGrid()
        {
            // Двойной цикл для создания сетки плиток
            for (int i = 0; i < gridSize; i++)      // Строки
            {
                for (int j = 0; j < gridSize; j++) // Столбцы
                {
                    CreateTile(i, j);
                }
            }
        }

        /// <summary>
        /// Создает отдельную плитку в указанной позиции сетки
        /// </summary>
        /// <param name="i">Строка в сетке (0-based)</param>
        /// <param name="j">Столбец в сетке (0-based)</param>
        private void CreateTile(int i, int j)
        {
            // Создаем новую кнопку-плитку
            var btn = new Button
            {
                Width = tileSize,
                Height = tileSize,
                Margin = new Thickness(0),           // Без внешних отступов
                Tag = new TilePosition(i, j),        // Сохраняем координаты
                Background = new SolidColorBrush(GetRandomColor()), // Случайный цвет
                Content = "",                        // Без текста
                BorderBrush = Brushes.Black,         // Черная рамка
                BorderThickness = new Thickness(1)   // Толщина рамки
            };

            // Позиционируем плитку на Canvas
            Canvas.SetLeft(btn, j * tileSize);  // X координата = столбец * размер плитки
            Canvas.SetTop(btn, i * tileSize);   // Y координата = строка * размер плитки

            // Добавляем плитку на родительский Canvas
            backgroundPanel.Children.Add(btn);

            // Сохраняем ссылку в массиве
            grid[i, j] = btn;
        }

        /// <summary>
        /// Генерирует случайный цвет для плитки из предопределенной палитры
        /// </summary>
        /// <returns>Случайный цвет из палитры</returns>
        public Color GetRandomColor()
        {
            return colorPalette[random.Next(colorPalette.Length)];
        }

        /// <summary>
        /// Меняет цвета двух плиток местами
        /// </summary>
        /// <param name="a">Первая плитка</param>
        /// <param name="b">Вторая плитка</param>
        public void SwapTiles(Button a, Button b)
        {
            // Классический обмен значений через временную переменную
            var temp = a.Background;
            a.Background = b.Background;
            b.Background = temp;
        }

        /// <summary>
        /// Заполняет пустые (прозрачные) плитки случайными цветами
        /// Используется после удаления совпадений для заполнения пустот
        /// </summary>
        /// <param name="btn">Плитка для проверки и заполнения</param>
        public void FillEmptyTile(Button btn)
        {
            // Проверяем, является ли плитка пустой (прозрачной)
            if (btn.Background == Brushes.Transparent)
            {
                btn.Background = new SolidColorBrush(GetRandomColor());
            }
        }

        /// <summary>
        /// Устанавливает обработчик клика для всех плиток сетки
        /// </summary>
        /// <param name="handler">Обработчик события клика</param>
        public void SetTileClickHandler(EventHandler<RoutedEventArgs> handler)
        {
            // Проходим по всем плиткам в сетке
            foreach (var tile in grid)
            {
                if (tile != null)
                {
                    tile.Click += handler; // Подписываемся на событие клика
                }
            }
        }

        /// <summary>
        /// Сбрасывает выделение плитки (восстанавливает стандартную рамку)
        /// </summary>
        /// <param name="btn">Плитка для сброса выделения</param>
        public void ResetTileSelection(Button btn)
        {
            if (btn != null)
            {
                btn.BorderThickness = new Thickness(1);  // Стандартная толщина рамки
                btn.BorderBrush = Brushes.Black;         // Черный цвет рамки
            }
        }

        /// <summary>
        /// Выполняет действие для каждой плитки в сетке
        /// </summary>
        /// <param name="action">Действие для выполнения над каждой плиткой</param>
        public void ForEachTile(Action<Button> action)
        {
            foreach (var tile in grid)
            {
                if (tile != null)
                {
                    action(tile); // Выполняем переданное действие
                }
            }
        }

        // Свойства только для чтения

        /// <summary>
        /// Возвращает двумерный массив плиток
        /// </summary>
        public Button[,] GetGrid() => grid;

        /// <summary>
        /// Возвращает размер сетки
        /// </summary>
        public int GetSize() => gridSize;
    }
}