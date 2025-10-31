#include "GameLogic.hpp"

namespace GameLauncher {

    /// <summary>
    /// Конструктор игровой логики
    /// </summary>
    GameLogic::GameLogic()
    {
        initializing = true;
        currentState = GameState::eInitializing;
    }

    /// <summary>
    /// Деструктор
    /// </summary>
    GameLogic::~GameLogic()
    {
    }

    /// <summary>
    /// Проверяет и удаляет совпадения (3+ в ряд по горизонтали или вертикали)
    /// </summary>
    /// <param name="matchedTiles">Возвращает информацию о совпавших плитках</param>
    /// <returns>true если найдены совпадения, иначе false</returns>
    Boolean GameLogic::CheckMatches(array<Button^, 2>^ grid, Int64 gridSize, array<array<Boolean>^>^% matchedTiles)
    {
        Boolean found = false; // Флаг обнаружения совпадений

        // Создаем матрицу для отметки совпавших плиток
        array<array<Boolean>^>^ matched = gcnew array<array<Boolean>^>(gridSize);
        for (Int64 i = 0; i < gridSize; i++)
        {
            matched[i] = gcnew array<Boolean>(gridSize);
            for (Int64 j = 0; j < gridSize; j++)
                matched[i][j] = false; // Инициализируем все значения как false
        }

        CheckHorizontalMatches(grid, gridSize, matched);
        CheckVerticalMatches(grid, gridSize, matched);
        found = HasMatches(gridSize, matched);

        if (found)
        {
            matchedTiles = matched; // Возвращаем информацию о совпавших плитках
            RemoveMatchedTiles(grid, gridSize, matched);
            DropTiles(grid, gridSize);
        }
        else
        {
            matchedTiles = nullptr;
        }

        return found;
    }

    /// <summary>
    /// Проверяет горизонтальные совпадения
    /// </summary>
    Void GameLogic::CheckHorizontalMatches(array<Button^, 2>^ grid, Int64 gridSize, array<array<Boolean>^>^ matched)
    {
        for (Int64 i = 0; i < gridSize; i++)
        {
            for (Int64 j = 0; j < gridSize - 2; j++)
            {
                Color c = grid[i, j]->BackColor;
                if (c != Color::Transparent &&
                    c == grid[i, j + 1]->BackColor &&
                    c == grid[i, j + 2]->BackColor)
                {
                    // Помечаем все три совпавшие плитки
                    matched[i][j] = true;
                    matched[i][j + 1] = true;
                    matched[i][j + 2] = true;
                }
            }
        }
    }

    /// <summary>
    /// Проверяет вертикальные совпадения
    /// </summary>
    Void GameLogic::CheckVerticalMatches(array<Button^, 2>^ grid, Int64 gridSize, array<array<Boolean>^>^ matched)
    {
        for (Int64 j = 0; j < gridSize; j++)
        {
            for (Int64 i = 0; i < gridSize - 2; i++)
            {
                Color c = grid[i, j]->BackColor;
                if (c != Color::Transparent &&
                    c == grid[i + 1, j]->BackColor &&
                    c == grid[i + 2, j]->BackColor)
                {
                    // Помечаем все три совпавшие плитки
                    matched[i][j] = true;
                    matched[i + 1][j] = true;
                    matched[i + 2][j] = true;
                }
            }
        }
    }

    /// <summary>
    /// Проверяет, есть ли в матрице отмеченные совпадения
    /// </summary>
    /// <param name="matched">Матрица совпадений</param>
    /// <returns>true если есть хотя бы одно совпадение</returns>
    Boolean GameLogic::HasMatches(Int64 gridSize, array<array<Boolean>^>^ matched)
    {
        for (Int64 i = 0; i < gridSize; i++)
        {
            for (Int64 j = 0; j < gridSize; j++)
            {
                if (matched[i][j])
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Удаляет плитки, отмеченные как совпавшие
    /// </summary>
    Void GameLogic::RemoveMatchedTiles(array<Button^, 2>^ grid, Int64 gridSize, array<array<Boolean>^>^ matched)
    {
        Int64 tilesRemoved = 0;

        for (Int64 i = 0; i < gridSize; i++)
        {
            for (Int64 j = 0; j < gridSize; j++)
            {
                if (matched[i][j])
                {
                    grid[i, j]->BackColor = Color::Transparent;
                    tilesRemoved++;
                }
            }
        }
    }

    /// <summary>
    /// Осуществляет падение плиток после удаления совпадений
    /// </summary>
    Void GameLogic::DropTiles(array<Button^, 2>^ grid, Int64 gridSize)
    {
        for (Int64 j = 0; j < gridSize; j++)
        {
            Int64 emptyRow = gridSize - 1;
            for (Int64 i = gridSize - 1; i >= 0; i--)
            {
                if (grid[i, j]->BackColor != Color::Transparent)
                {
                    if (i != emptyRow)
                    {
                        grid[emptyRow, j]->BackColor = grid[i, j]->BackColor;
                        grid[i, j]->BackColor = Color::Transparent;
                    }
                    emptyRow--;
                }
            }
        }

        // Добавляет задержку для визуального эффекта
        if (!initializing)
        {
            Application::DoEvents();
            Thread::Sleep(200);
        }
    }

    /// <summary>
    /// Обрабатывает цепную реакцию совпадений до их полного отсутствия
    /// </summary>
    Int64 GameLogic::ProcessMatches(array<Button^, 2>^ grid, Int64 gridSize, GameGrid^ gameGrid)
    {
        currentState = GameState::eProcessing; // Блокирует ввод во время обработки

        Int64 totalRemoved = 0;
        Boolean changed;
        do
        {

            array<array<Boolean>^>^ matchedTiles;
            changed = CheckMatches(grid, gridSize, matchedTiles);   // Проверяет и удаляет совпадения
            gameGrid->FillEmptyTiles(); // Заполняет пустоты новыми плитками

            if (changed)
            {
                // Считаем плитки, удаленные на этом шаге цепной реакции
                Int64 stepRemoved = CountRemovedTiles(matchedTiles, gridSize);
                totalRemoved += stepRemoved;

                // Добавляет задержку для визуального эффекта
                if (!initializing)
                {
                    Application::DoEvents();
                    Thread::Sleep(200);
                }
            }

        } while (changed); // Продолжает пока есть изменения

        currentState = GameState::ePlaying;
        initializing = false;           // Снимает флаг инициализации

        return totalRemoved;
    }

    /// <summary>
    /// Проверяет, являются ли две плитки соседними
    /// </summary>
    Boolean GameLogic::AreAdjacent(Button^ a, Button^ b)
    {
        // Получает координаты из Tag плиток
        Point p1 = safe_cast<Point>(a->Tag);
        Point p2 = safe_cast<Point>(b->Tag);

        // Вычисляет разницу по координатам
        Int64 dx = Math::Abs(p1.X - p2.X);
        Int64 dy = Math::Abs(p1.Y - p2.Y);

        return (dx + dy) == 1;
    }

    /// <summary>
    /// Обрабатывает попытку обмена двух плиток
    /// </summary>
    Int64 GameLogic::HandleTileSwap(Button^ tile1, Button^ tile2, array<Button^, 2>^ grid, Int64 gridSize, GameGrid^ gameGrid)
    {
        if (AreAdjacent(tile1, tile2))
        {
            // Пробует обменять плитки
            gameGrid->SwapTiles(tile1, tile2);

            // Добавляет задержку для визуального эффекта
            if (!initializing)
            {
                Application::DoEvents();
                Thread::Sleep(200);
            }

            // Проверяем совпадения после обмена
            array<array<Boolean>^>^ matchedTiles;
            if (CheckMatches(grid, gridSize, matchedTiles))
            {
                // Если есть совпадения - обрабатываем цепную реакцию и возвращаем общее количество удаленных плиток
                Int64 removedCount = CountRemovedTiles(matchedTiles, gridSize);

                // Обрабатываем цепную реакцию и суммируем все удаленные плитки
                removedCount += ProcessMatches(grid, gridSize, gameGrid);

                return removedCount;
            }
            else
            {
                // Если нет совпадений - откатываем обмен
                gameGrid->SwapTiles(tile1, tile2);
                return 0;
            }
        }
        return 0;
    }

    /// <summary>
    /// Подсчитывает количество удаленных плиток
    /// </summary>
    Int64 GameLogic::CountRemovedTiles(array<array<Boolean>^>^ matched, Int64 gridSize)
    {
        Int64 count = 0;
        for (Int64 i = 0; i < gridSize; i++)
        {
            for (Int64 j = 0; j < gridSize; j++)
            {
                if (matched[i][j])
                {
                    count++;
                }
            }
        }
        return count;
    }
}
