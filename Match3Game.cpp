#include "Match3Game.hpp"
#include "GameGrid.hpp"
#include "GameLogic.hpp"
#include "InputHandler.hpp"
#include "ScoreManager.hpp"
#include "UIManager.hpp"

namespace GameLauncher {

    /// <summary>
    /// Конструктор главной формы игры
    /// </summary>
    Match3::Match3()
        : dragging(false)
        , start_point(Point(0, 0))
    {
        InitializeComponent();

        // Инициализация игровых компонентов
        this->gameGrid = gcnew GameGrid(background, GameGrid::DEFAULT_GRID_SIZE, GameGrid::DEFAULT_TILE_SIZE);
        this->scoreManager = gcnew ScoreManager(scoreLabel); // Создаем первым
        this->gameLogic = gcnew GameLogic(gameGrid, scoreManager); // Передаем scoreManager
        this->inputHandler = gcnew InputHandler(gameLogic);
        this->uiManager = gcnew UIManager(this);

        // Настройка обработчиков событий для плиток
        SetupTileEventHandlers();

        // Подписка на событие выбора плитки
        inputHandler->OnTileSelected += gcnew TileSelectedHandler(this, &Match3::OnTileSelected);

        // Инициализация UI
        uiManager->InitializeColors(Color::White, Color::Black, Color::Gray);

        // Начало игры
        gameLogic->ProcessMatches();      
    }
    
    /// Деструктор формы
    Match3::~Match3()
    {
        if (components)
        {
            delete components;
        }
    }

    /// <summary>
    /// Настраивает обработчики событий для всех плиток игрового поля
    /// </summary>
    System::Void Match3::SetupTileEventHandlers()
    {
        auto grid = gameGrid->GetGrid();
        Int64 gridSize = gameGrid->GetSize();

        for (Int64 i = 0; i < gridSize; i++)
        {
            for (Int64 j = 0; j < gridSize; j++)
            {
                Button^ tile = grid[i, j];
                if (tile != nullptr)
                {
                    tile->Click += gcnew EventHandler(inputHandler, &InputHandler::HandleTileClick);
                }
            }
        }

        // Применяем тему к плиткам после их создания
        uiManager->ApplyThemeToTiles(grid, gridSize, false); // false = светлая тема
    }
    /// <summary>
    /// Обработчик события выбора плитки (подсветка выбранной плитки)
    /// </summary>
    /// <param name="tile">Выбранная плитка</param>
    System::Void Match3::OnTileSelected(Button^ tile)
    {
        // Подсветка выбранной плитки
        if (tile != nullptr)
        {
            tile->FlatAppearance->BorderSize = 2;
            tile->FlatAppearance->BorderColor = Color::Black;
        }
    }

    /// <summary>
    /// Переключает между светлой и темной темами
    /// </summary>
    System::Void Match3::ToggleTheme()
    {
        if (uiManager->CurrentTheme == UIManager::ColorTheme::eLight)
        {
            uiManager->SetTheme(UIManager::ColorTheme::eDark);
            // Обновляем плитки для темной темы
            uiManager->ApplyThemeToTiles(gameGrid->GetGrid(), gameGrid->GetSize(), true);
        }
        else
        {
            uiManager->SetTheme(UIManager::ColorTheme::eLight);
            // Обновляем плитки для светлой темы
            uiManager->ApplyThemeToTiles(gameGrid->GetGrid(), gameGrid->GetSize(), false);
        }
    }

    /// <summary>
    /// Инициализирует компоненты формы (автогенерируемый код WinForms)
    /// </summary>
    System::Void Match3::InitializeComponent()
    {
        this->toolBar = (gcnew System::Windows::Forms::Panel());
        this->buttonClose = (gcnew System::Windows::Forms::Button());
        this->buttonMinimize = (gcnew System::Windows::Forms::Button());
        this->buttonInfo = (gcnew System::Windows::Forms::Button());
        this->scoreLabel = (gcnew System::Windows::Forms::Label());
        this->background = (gcnew System::Windows::Forms::Panel());
        this->toolBar->SuspendLayout();
        this->SuspendLayout();

        // toolBar
        this->toolBar->BackColor = System::Drawing::Color::White;
        this->toolBar->Controls->Add(this->buttonClose);
        this->toolBar->Controls->Add(this->buttonMinimize);
        this->toolBar->Controls->Add(this->buttonInfo);
        this->toolBar->Controls->Add(this->scoreLabel);
        this->toolBar->Location = System::Drawing::Point(1, 1);
        this->toolBar->Name = L"toolBar";
        this->toolBar->Size = System::Drawing::Size(543, 32);
        this->toolBar->TabIndex = 8;
        this->toolBar->MouseDown += gcnew System::Windows::Forms::MouseEventHandler(this, &Match3::toolBar_MouseDown);
        this->toolBar->MouseMove += gcnew System::Windows::Forms::MouseEventHandler(this, &Match3::toolBar_MouseMove);
        this->toolBar->MouseUp += gcnew System::Windows::Forms::MouseEventHandler(this, &Match3::toolBar_MouseUp);

        // buttonClose
        this->buttonClose->BackColor = System::Drawing::Color::White;
        this->buttonClose->FlatAppearance->BorderColor = System::Drawing::Color::Black;
        this->buttonClose->FlatAppearance->BorderSize = 0;
        this->buttonClose->FlatStyle = System::Windows::Forms::FlatStyle::Flat;
        this->buttonClose->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 16, System::Drawing::FontStyle::Bold));
        this->buttonClose->ForeColor = System::Drawing::Color::Black;
        this->buttonClose->Location = System::Drawing::Point(507, 0);
        this->buttonClose->Margin = System::Windows::Forms::Padding(0);
        this->buttonClose->Name = L"buttonClose";
        this->buttonClose->Size = System::Drawing::Size(36, 32);
        this->buttonClose->TabIndex = 4;
        this->buttonClose->Text = L"x";
        this->buttonClose->UseVisualStyleBackColor = false;
        this->buttonClose->Click += gcnew System::EventHandler(this, &Match3::buttonClose_Click);
        this->buttonClose->MouseEnter += gcnew System::EventHandler(this, &Match3::buttonClose_MouseEnter);
        this->buttonClose->MouseLeave += gcnew System::EventHandler(this, &Match3::buttonClose_MouseLeave);

        // buttonMinimize
        this->buttonMinimize->BackColor = System::Drawing::Color::White;
        this->buttonMinimize->FlatAppearance->BorderColor = System::Drawing::Color::Black;
        this->buttonMinimize->FlatAppearance->BorderSize = 0;
        this->buttonMinimize->FlatStyle = System::Windows::Forms::FlatStyle::Flat;
        this->buttonMinimize->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 15.75F, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point,
            static_cast<System::Byte>(0)));
        this->buttonMinimize->ForeColor = System::Drawing::Color::Black;
        this->buttonMinimize->Location = System::Drawing::Point(470, 0);
        this->buttonMinimize->Margin = System::Windows::Forms::Padding(0);
        this->buttonMinimize->Name = L"buttonMinimize";
        this->buttonMinimize->Size = System::Drawing::Size(36, 32);
        this->buttonMinimize->TabIndex = 5;
        this->buttonMinimize->Text = L"_";
        this->buttonMinimize->UseVisualStyleBackColor = false;
        this->buttonMinimize->Click += gcnew System::EventHandler(this, &Match3::buttonMinimize_Click);

        // buttonInfo
        this->buttonInfo->BackColor = System::Drawing::Color::White;
        this->buttonInfo->FlatAppearance->BorderColor = System::Drawing::Color::Black;
        this->buttonInfo->FlatAppearance->BorderSize = 0;
        this->buttonInfo->FlatStyle = System::Windows::Forms::FlatStyle::Flat;
        this->buttonInfo->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point,
            static_cast<System::Byte>(0)));
        this->buttonInfo->ForeColor = System::Drawing::Color::Black;
        this->buttonInfo->Location = System::Drawing::Point(433, 0);
        this->buttonInfo->Margin = System::Windows::Forms::Padding(0);
        this->buttonInfo->Name = L"buttonInfo";
        this->buttonInfo->Size = System::Drawing::Size(36, 32);
        this->buttonInfo->TabIndex = 8;
        this->buttonInfo->Text = L"?";
        this->buttonInfo->UseVisualStyleBackColor = false;
        this->buttonInfo->Click += gcnew System::EventHandler(this, &Match3::buttonInfo_Click);

        // scoreLabel
        this->scoreLabel->Font = (gcnew System::Drawing::Font(L"Impact", 15.75F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point,
            static_cast<System::Byte>(0)));
        this->scoreLabel->ForeColor = System::Drawing::Color::Black;
        this->scoreLabel->Location = System::Drawing::Point(2, 2);
        this->scoreLabel->Margin = System::Windows::Forms::Padding(0);
        this->scoreLabel->Name = L"scoreLabel";
        this->scoreLabel->Size = System::Drawing::Size(200, 28);
        this->scoreLabel->TabIndex = 0;
        this->scoreLabel->Text = L"Score: 0";
        this->scoreLabel->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
        this->scoreLabel->MouseDown += gcnew System::Windows::Forms::MouseEventHandler(this, &Match3::toolBar_MouseDown);
        this->scoreLabel->MouseMove += gcnew System::Windows::Forms::MouseEventHandler(this, &Match3::toolBar_MouseMove);
        this->scoreLabel->MouseUp += gcnew System::Windows::Forms::MouseEventHandler(this, &Match3::toolBar_MouseUp);

        // background
        this->background->BackColor = System::Drawing::Color::Black;
        this->background->ForeColor = System::Drawing::Color::Black;
        this->background->Location = System::Drawing::Point(1, 35);
        this->background->Name = L"background";
        this->background->Size = System::Drawing::Size(543, 543);
        this->background->TabIndex = 10;

        // Match3
        this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
        this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
        this->BackColor = System::Drawing::Color::Black;
        this->ClientSize = System::Drawing::Size(545, 579);
        this->Controls->Add(this->background);
        this->Controls->Add(this->toolBar);
        this->FormBorderStyle = System::Windows::Forms::FormBorderStyle::None;
        this->Name = L"Match3";
        this->Text = L"GameForm";
        this->toolBar->ResumeLayout(false);
        this->ResumeLayout(false);
    }

    System::Void Match3::buttonClose_Click(System::Object^ sender, System::EventArgs^ e)
    { 
        this->Close(); 
    }
    System::Void Match3::buttonClose_MouseLeave(System::Object^ sender, System::EventArgs^ e)
    { 
        this->buttonClose->BackColor = Color::White; 
    }
    System::Void Match3::buttonClose_MouseEnter(System::Object^ sender, System::EventArgs^ e) 
    { 
        this->buttonClose->BackColor = Color::LightCoral; 
    }
    System::Void Match3::buttonMinimize_Click(System::Object^ sender, System::EventArgs^ e) 
    {
        this->WindowState = System::Windows::Forms::FormWindowState::Minimized; 
    }
    System::Void Match3::toolBar_MouseDown(System::Object^ sender, System::Windows::Forms::MouseEventArgs^ e)
    {
        dragging = true; start_point = Point(e->X, e->Y);
    }
    System::Void Match3::toolBar_MouseMove(System::Object^ sender, System::Windows::Forms::MouseEventArgs^ e)
    {
        if (dragging) 
        { 
            Point p = PointToScreen(e->Location);
            Location = Point(p.X - start_point.X, p.Y - start_point.Y); 
        }
    }
    System::Void Match3::toolBar_MouseUp(System::Object^ sender, System::Windows::Forms::MouseEventArgs^ e)
    {
        dragging = false;
    }
    System::Void Match3::buttonInfo_Click(System::Object^ sender, System::EventArgs^ e) 
    {
        uiManager->ShowInfoDialog();
    }
}