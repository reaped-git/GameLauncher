#include "GameLogic.hpp"
#include "GameGrid.hpp"
#include "ScoreManager.hpp"

namespace GameLauncher {

    /// Конструктор игровой логики
    GameLogic::GameLogic(GameGrid^ grid, ScoreManager^ scoreMgr)
    {
        gameGrid = grid;
        scoreManager = scoreMgr; // Сохраняем ссылку на менеджер счета
        initializing = true;
        currentState = GameState::eInitializing;
    }

    GameLogic::~GameLogic()
    {
    }

    Boolean GameLogic::CheckMatches()
    {
        auto grid = gameGrid->GetGrid();
        Int64 gridSize = gameGrid->GetSize();
        Boolean found = false; // Флаг обнаружения совпадений

        // Создаем матрицу для отметки совпавших плиток
        array<array<Boolean>^>^ matched = gcnew array<array<Boolean>^>(gridSize);
        for (Int64 i = 0; i < gridSize; i++)
        {
            matched[i] = gcnew array<Boolean>(gridSize);
            for (Int64 j = 0; j < gridSize; j++)
                matched[i][j] = false; // Инициализируем все значения как false
        }

        CheckHorizontalMatches(matched);
        CheckVerticalMatches(matched);
        found = HasMatches(matched);

        if (found)
        {
            RemoveMatchedTiles(matched);
            DropTiles();
        }

        return found;
    }

    /// Проверяет горизонтальные совпадения 
    Void GameLogic::CheckHorizontalMatches(array<array<Boolean>^>^ matched)
    {
        auto grid = gameGrid->GetGrid();
        Int64 gridSize = gameGrid->GetSize();

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

    /// Проверяет вертикальные совпадения
    Void GameLogic::CheckVerticalMatches(array<array<Boolean>^>^ matched)
    {
        auto grid = gameGrid->GetGrid();
        Int64 gridSize = gameGrid->GetSize();

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

    Boolean GameLogic::HasMatches(array<array<Boolean>^>^ matched)
    {
        Int64 gridSize = gameGrid->GetSize();

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

    /// Удаляет плитки, отмеченные в матрице совпадений
    Void GameLogic::RemoveMatchedTiles(array<array<Boolean>^>^ matched)
    {
        auto grid = gameGrid->GetGrid();
        Int64 gridSize = gameGrid->GetSize();

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

        if (!initializing && tilesRemoved > 0 && scoreManager != nullptr)
        {
            // Бонус за количество удаленных плиток за один ход
            Int64 baseScore = tilesRemoved * ScoreManager::SCORE_PER_TILE;

            // Бонус за комбо (чем больше плиток удалено за один ход - тем больше множитель)
            Int64 comboBonus = 0;

            // Большой бонус за 15+ плиток
            if (tilesRemoved >= 15)
            {
                comboBonus = tilesRemoved; 
            }
            // Бонус за 10-14 плитки
            else if (tilesRemoved >= 10)
            {
                comboBonus = tilesRemoved / 2;
            }
            // Бонус за 5-9 плитки
            else if (tilesRemoved >= 5)
            {
                comboBonus = tilesRemoved / 4;
            }

            Int64 totalScore = baseScore + comboBonus;
            scoreManager->AddScore(totalScore);

        }
    }

    /// Осуществляет падение плиток после удаления совпадений
    Void GameLogic::DropTiles()
    {
        auto grid = gameGrid->GetGrid();
        Int64 gridSize = gameGrid->GetSize();

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

    /// Обрабатывает цепную реакцию совпадений до полного их отсутствия
    Void GameLogic::ProcessMatches()
    {
        currentState = GameState::eProcessing; // Блокирует ввод во время обработки

        Boolean changed;
        do
        {
            changed = CheckMatches();   // Проверяет и удаляет совпадения
            gameGrid->FillEmptyTiles(); // Заполняет пустоты новыми плитками

            // Добавляет задержку для визуального эффекта
            if (!initializing)
            {
                Application::DoEvents();
                Thread::Sleep(200);
            }
        } while (changed); // Продолжает пока есть изменения

        currentState = GameState::ePlaying;
        initializing = false;           // Снимает флаг инициализации
    }

    /// Проверяет, являются ли две плитки соседними (для допустимости обмена)
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

    /// Обрабатывает попытку обмена двух плиток
    Void GameLogic::HandleTileSwap(Button^ tile1, Button^ tile2)
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

            // Проверяет, создал ли обмен совпадения
            if (!CheckMatches())
            {
                // Если нет совпадений - откатывает обмен
                gameGrid->SwapTiles(tile1, tile2);
            }
            else
            {
                // Если есть совпадения - обрабатывает цепную реакци
                ProcessMatches();
            }
        }
    }

}