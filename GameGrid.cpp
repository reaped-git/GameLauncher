#include "GameGrid.hpp"

namespace GameLauncher {

    GameGrid::GameGrid(Panel^ background, Int64 size, Int64 tileSize)
    {
        backgroundPanel = background;
        gridSize = size;
        this->tileSize = tileSize; 
        random = gcnew Random();
        grid = gcnew array<Button^, 2>(gridSize, gridSize);

        InitializeGrid();
    }

    GameGrid::~GameGrid()
    {
    }

    /// Инициализирует игровую сетку
    void GameGrid::InitializeGrid()
    {
        for (Int64 i = 0; i < gridSize; i++)
        {
            for (Int64 j = 0; j < gridSize; j++)
            {
                CreateTile(i, j);
            }
        }

    }

    /// Возвращает случайный цвет из предопределенной палитры цветов игры
    Color GameGrid::GetRandomColor()
    {
        // Палитра из 6 красивых цветов 
        array<Color>^ colors = {
            Color::FromArgb(158, 25, 66),    // Темно-красный
            Color::FromArgb(244, 109, 67),   // Оранжевый
            Color::FromArgb(254, 224, 139),  // Светло-желтый
            Color::FromArgb(147, 219, 135),  // Светло-зеленый
            Color::FromArgb(50, 136, 189),   // Синий
            Color::FromArgb(94, 79, 162)     // Фиолетовый
        };
        return colors[random->Next(colors->Length)];
    }

    /// Меняет местами цвета двух плиток (визуальное представление обмена)
    void GameGrid::SwapTiles(Button^ a, Button^ b)
    {
        Color temp = a->BackColor;
        a->BackColor = b->BackColor;
        b->BackColor = temp;
    }

    /// Заполняет все прозрачные плитки случайными цветами после удаления совпадений
    void GameGrid::FillEmptyTiles()
    {
        for (Int64 i = 0; i < gridSize; i++)
        {
            for (Int64 j = 0; j < gridSize; j++)
            {
                if (grid[i, j]->BackColor == Color::Transparent)
                {
                    grid[i, j]->BackColor = GetRandomColor();
                }
            }
        }
    }

    /// Создает отдельную плитку в указанной позиции сетки
    void GameGrid::CreateTile(Int64 i, Int64 j)
    {
        Button^ btn = gcnew Button();
        btn->Size = Drawing::Size(tileSize, tileSize);
        btn->Location = Drawing::Point(j * tileSize, i * tileSize);
        btn->FlatStyle = FlatStyle::Flat;
        btn->Tag = Point(i, j);
        btn->FlatAppearance->BorderColor = Color::Black;
        btn->FlatAppearance->BorderSize = 1;
        btn->BackColor = GetRandomColor();
        btn->Text = ""; 
        btn->Name = "Tile_" + i + "_" + j; 

        backgroundPanel->Controls->Add(btn);
        grid[i, j] = btn;

    }

}