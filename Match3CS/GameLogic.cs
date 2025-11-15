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
    }

    /// <summary>
    /// Основная игровая логика "Три в ряд"
    /// Отвечает за проверку совпадений, обработку ходов и управление состоянием игры
    /// </summary>
    public class GameLogic
    {
        private bool initializing;       // Флаг начальной инициализации
        private GameState currentState;  // Текущее состояние игры

        /// <summary>
        /// Конструктор игровой логики
        /// </summary>
        public GameLogic()
        {
            initializing = true;
            currentState = GameState.Initializing;
        }

        /// <summary>
        /// Проверяет и удаляет совпадения (3+ в ряд по горизонтали или вертикали)
        /// </summary>
        /// <param name="grid">Игровая сетка с плитками</param>
        /// <param name="gridSize">Размер сетки</param>
        /// <param name="matchedTiles">Выходной параметр - матрица совпавших плиток</param>
        /// <returns>true если найдены совпадения, иначе false</returns>
        public bool CheckMatches(IEnumerable<IEnumerable<Button>> grid, out bool[,] matchedTiles)
        {
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

        /// <summary>
        /// Проверяет горизонтальные совпадения (3+ плиток одного цвета в ряд)
        /// </summary>
        private void CheckHorizontalMatches(Button[,] grid, int gridSize, bool[,] matched)
        {
            for (int i = 0; i < gridSize; i++)          // Проходим по всем строкам
            {
                for (int j = 0; j < gridSize - 2; j++) // Проходим по столбцам (оставляем место для 3 плиток)
                {
                    // Получаем цвета трех последовательных плиток
                    var color1 = GetButtonColor(grid[i, j]);
                    var color2 = GetButtonColor(grid[i, j + 1]);
                    var color3 = GetButtonColor(grid[i, j + 2]);

                    // Проверяем совпадение трех плиток подряд
                    if (color1.HasValue && color1 == color2 && color1 == color3)
                    {
                        // Отмечаем все три плитки как совпавшие
                        matched[i, j] = true;
                        matched[i, j + 1] = true;
                        matched[i, j + 2] = true;
                    }
                }
            }
        }

        /// <summary>
        /// Проверяет вертикальные совпадения (3+ плиток одного цвета в столбец)
        /// </summary>
        private void CheckVerticalMatches(Button[,] grid, int gridSize, bool[,] matched)
        {
            for (int i = 0; i < gridSize - 2; i++)      // Проходим по строкам (оставляем место для 3 плиток)
            {
                for (int j = 0; j < gridSize; j++)     // Проходим по всем столбцам
                {
                    // Получаем цвета трех последовательных плиток
                    var color1 = GetButtonColor(grid[i, j]);
                    var color2 = GetButtonColor(grid[i + 1, j]);
                    var color3 = GetButtonColor(grid[i + 2, j]);

                    // Проверяем совпадение трех плиток в столбец
                    if (color1.HasValue && color1 == color2 && color1 == color3)
                    {
                        // Отмечаем все три плитки как совпавшие
                        matched[i, j] = true;
                        matched[i + 1, j] = true;
                        matched[i + 2, j] = true;
                    }
                }
            }
        }

        /// <summary>
        /// Проверяет, есть ли в матрице отмеченные совпадения
        /// </summary>
        private bool HasMatches(int gridSize, bool[,] matched)
        {
            // Используем foreach для перебора всех элементов матрицы
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
                        SetButtonColor(grid[i, j], null); // Устанавливаем прозрачный цвет
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
            // Обрабатываем каждый столбец отдельно
            for (int j = 0; j < gridSize; j++)
            {
                int emptyRow = gridSize - 1; // Начинаем с самой нижней строки

                // Проходим столбец снизу вверх
                for (int i = gridSize - 1; i >= 0; i--)
                {
                    // Если плитка не пустая (не прозрачная)
                    if (GetButtonColor(grid[i, j]).HasValue)
                    {
                        // Если плитка не на своем месте - перемещаем ее вниз
                        if (i != emptyRow)
                        {
                            SetButtonColor(grid[emptyRow, j], GetButtonColor(grid[i, j]));
                            SetButtonColor(grid[i, j], null); // Освобождаем старую позицию
                        }
                        emptyRow--; // Поднимаемся к следующей пустой позиции
                    }
                }
            }
        }

        /// <summary>
        /// Обрабатывает цепную реакцию совпадений до их полного отсутствия
        /// </summary>
        /// <returns>Общее количество удаленных плиток за всю цепную реакцию</returns>
        public int ProcessMatches(IEnumerable<IEnumerable<Button>> grid, GameGrid gameGrid)
        {
            currentState = GameState.Processing; // Блокируем ввод во время обработки
            int totalRemoved = 0;
            bool changed;

            // Цикл продолжается пока находятся новые совпадения
            do
            {
                // Заполняем пустые плитки новыми цветами
                gameGrid.ForEachTile(btn => gameGrid.FillEmptyTile(btn));

                // Проверяем совпадения
                changed = CheckMatches(grid, out bool[,] matchedTiles);

                if (changed)
                {
                    // Подсчитываем удаленные на этом шаге плитки
                    int stepRemoved = CountRemovedTiles(matchedTiles);
                    totalRemoved += stepRemoved;
                }
            } while (changed);  // Продолжаем пока есть изменения

            currentState = GameState.Playing; // Разблокируем ввод
            initializing = false;             // Снимаем флаг инициализации

            return totalRemoved;
        }

        /// <summary>
        /// Проверяет, являются ли две плитки соседними (по горизонтали или вертикали)
        /// </summary>
        public bool AreAdjacent(Button a, Button b)
        {
            // Получаем координаты из Tag плиток
            if (a.Tag is not TilePosition pos1 || b.Tag is not TilePosition pos2)
                return false;

            // Вычисляем разницу по координатам
            int dx = Math.Abs(pos1.X - pos2.X);
            int dy = Math.Abs(pos1.Y - pos2.Y);

            // Соседние плитки имеют разницу координат (1,0) или (0,1)
            return (dx + dy) == 1;
        }

        /// <summary>
        /// Обрабатывает попытку обмена двух плиток
        /// </summary>
        /// <returns>Количество удаленных плиток в результате обмена</returns>
        public int HandleTileSwap(Button tile1, Button tile2, 
            IEnumerable<IEnumerable<Button>> grid, GameGrid gameGrid)
        {
            // Проверяем, что плитки соседние
            if (AreAdjacent(tile1, tile2))
            {
                // Пробуем обменять плитки
                gameGrid.SwapTiles(tile1, tile2);

                // Проверяем, привел ли обмен к совпадениям
                if (CheckMatches(grid, out bool[,] matchedTiles))
                {
                    // Если есть совпадения - обрабатываем цепную реакцию
                    int removedCount = CountRemovedTiles(matchedTiles);
                    removedCount += ProcessMatches(grid, gameGrid);
                    return removedCount;
                }
                else
                {
                    // Если нет совпадений - откатываем обмен
                    gameGrid.SwapTiles(tile1, tile2);
                    return 0;
                }
            }
            return 0;
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
            // Преобразуем Background в SolidColorBrush и извлекаем цвет
            if (button.Background is SolidColorBrush brush)
            {
                return brush.Color;
            }
            return null; // Прозрачная плитка
        }

        /// <summary>
        /// Вспомогательный метод для установки цвета плитки
        /// </summary>
        private void SetButtonColor(Button button, Color? color)
        {
            button.Background = color.HasValue
                ? new SolidColorBrush(color.Value)  // Устанавливаем цвет
                : Brushes.Transparent;              // Или делаем прозрачной
        }

        /// <summary>
        /// Текущее состояние игры
        /// </summary>
        public GameState CurrentState
        {
            get => currentState;
            set => currentState = value;
        }

        // Публичные свойства с геттерами и сеттерами
        /// <summary>
        /// Флаг начальной инициализации игры
        /// </summary>
        public bool Initializing
        {
            get => initializing;
            set => initializing = value;
        }
    }

    /// <summary>
    /// Запись для хранения координат плитки в сетке
    /// </summary>
    /// <param name="X">Координата X (строка)</param>
    /// <param name="Y">Координата Y (столбец)</param>
    public record TilePosition(int X, int Y);
}