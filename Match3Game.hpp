#pragma once

namespace GameLauncher {

    using namespace System;
    using namespace System::ComponentModel;
    using namespace System::Collections;
    using namespace System::Windows::Forms;
    using namespace System::Data;
    using namespace System::Drawing;

    // Forward declarations
    ref class GameGrid;
    ref class GameLogic;
    ref class InputHandler;
    ref class ScoreManager;
    ref class UIManager;

    /// <summary>
    /// Главная форма игры "Три в ряд" (Match-3)
    /// Наследуется от System::Windows::Forms::Form для создания оконного приложения
    /// </summary>
    public ref class Match3 : public System::Windows::Forms::Form
    {
    public:
        /// <summary>
        /// Конструктор формы - инициализирует все компоненты игры
        /// </summary>
        Match3();
        ~Match3();

    private:
        // Элементы пользовательского интерфейса
        System::Windows::Forms::Panel^ toolBar;           // Панель заголовка для перетаскивания окна
        System::Windows::Forms::Button^ buttonClose;      // Кнопка закрытия приложения
        System::Windows::Forms::Button^ buttonMinimize;   // Кнопка сворачивания окна
        System::Windows::Forms::Button^ buttonInfo;       // Кнопка информации о игре
        System::Windows::Forms::Label^ scoreLabel;        // Метка для отображения счета
        System::Windows::Forms::Panel^ background;        // Панель игрового поля
        System::ComponentModel::Container^ components;    // Контейнер для компонентов формы

        // Игровые компоненты - основные модули игры
        GameGrid^ gameGrid;        // Управление игровой сеткой и плитками
        GameLogic^ gameLogic;      // Логика проверки совпадений и обработки ходов
        UIManager^ uiManager;      // Управление интерфейсом и темами
        InputHandler^ inputHandler;// Обработка ввода пользователя
        ScoreManager^ scoreManager;// Управление счетом игрока

        // Переменные управления окном
        Boolean dragging;             // Флаг перетаскивания окна
        Point start_point;         // Начальная точка перетаскивания

    private:
        // Объявление методов

        /// <summary>
        /// Инициализирует компоненты формы (автогенерируемый код WinForms)
        /// </summary>
        System::Void InitializeComponent();


        /// <summary>
        /// Настраивает обработчики событий для всех плиток игрового поля
        /// </summary>
        System::Void SetupTileEventHandlers();


        /// <summary>
        /// Обработчик события выбора плитки (подсветка выбранной плитки)
        /// </summary>
        /// <param name="tile">Выбранная плитка</param>
        System::Void OnTileSelected(Button^ tile);

        /// <summary>
        /// Обработчик переключения темы (можно вызвать из меню или кнопки)
        /// </summary>
        System::Void ToggleTheme();

        // Обработчики событий
        System::Void buttonClose_Click(System::Object^ sender, System::EventArgs^ e);
        System::Void buttonClose_MouseLeave(System::Object^ sender, System::EventArgs^ e);
        System::Void buttonClose_MouseEnter(System::Object^ sender, System::EventArgs^ e);
        System::Void buttonMinimize_Click(System::Object^ sender, System::EventArgs^ e);
        System::Void toolBar_MouseDown(System::Object^ sender, System::Windows::Forms::MouseEventArgs^ e);
        System::Void toolBar_MouseMove(System::Object^ sender, System::Windows::Forms::MouseEventArgs^ e);
        System::Void toolBar_MouseUp(System::Object^ sender, System::Windows::Forms::MouseEventArgs^ e);
        System::Void buttonInfo_Click(System::Object^ sender, System::EventArgs^ e);
    };

}