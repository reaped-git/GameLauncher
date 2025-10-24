#pragma once

namespace GameLauncher {

    using namespace System;
    using namespace System::Windows::Forms;
    using namespace System::Drawing;

    /// <summary>
    /// Класс для управления пользовательским интерфейсом - темы, диалоги, визуальные настройки
    /// </summary>
    public ref class UIManager
    {
    public:
        /// <summary>
        /// Доступные цветовые темы интерфейса
        /// </summary>
        enum class ColorTheme
        {
            eLight = 0, // Светлая тема
            eDark = 1,  // Темная тема
        };

    private:
        ColorTheme currentTheme; // Текущая активная тема
        Form^ mainForm;          // Ссылка на главную форму для применения настроек

        // Цвета для светлой темы
        Color lightBackground;
        Color lightForeground;
        Color lightControl;
        Color lightAccent;

        // Цвета для темной темы
        Color darkBackground;
        Color darkForeground;
        Color darkControl;
        Color darkAccent;

    public:
        /// <summary>
        /// Конструктор менеджера интерфейса
        /// </summary>
        /// <param name="form">Главная форма приложения</param>
        UIManager(Form^ form);

        /// <summary>
        /// Деструктор
        /// </summary>
        ~UIManager();

        /// <summary>
        /// Инициализирует цвета интерфейса согласно выбранной теме
        /// </summary>
        /// <param name="white">Цвет для светлых элементов</param>
        /// <param name="black">Цвет для темных элементов</param>
        /// <param name="gray">Цвет для нейтральных элементов</param>
        Void InitializeColors(Color white, Color black, Color gray);

        /// <summary>
        /// Устанавливает цветовую тему интерфейса
        /// </summary>
        /// <param name="theme">Тема для применения</param>
        Void SetTheme(ColorTheme theme);

        /// <summary>
        /// Показывает диалоговое окно с информацией о игре и правилами
        /// </summary>
        Void ShowInfoDialog();

        /// <summary>
        /// Применяет тему к игровым плиткам (вызывается из Match3 после создания сетки)
        /// </summary>
        /// <param name="tiles">Массив плиток</param>
        /// <param name="gridSize">Размер сетки</param>
        /// <param name="isDarkTheme">true для темной темы</param>
        Void ApplyThemeToTiles(array<System::Windows::Forms::Button^, 2>^ tiles, Int64 gridSize, Boolean isDarkTheme);

        /// <summary>
        /// Возвращает текущую активную тему
        /// </summary>
        property ColorTheme CurrentTheme{
            ColorTheme get() { return currentTheme; }
        }

        ///// <summary>
        ///// Показывает диалоговое окно окончания игры с итоговым счетом
        ///// </summary>
        ///// <param name="score">Финальный счет игрока</param>
        //Void ShowGameOverDialog(Int64 score);
    private:
        /// <summary>
        /// Применяет светлую цветовую тему к интерфейсу
        /// </summary>
        Void ApplyLightTheme();

        /// <summary>
        /// Применяет темную цветовую тему к интерфейсу
        /// </summary>
        Void ApplyDarkTheme();

        ///// <summary>
        ///// Рекурсивно применяет тему ко всем элементам управления формы
        ///// </summary>
        ///// <param name="control">Родительский элемент управления</param>
        ///// <param name="isDarkTheme">true для темной темы, false для светлой</param>
        //Void ApplyThemeToControl(Control^ control, Boolean isDarkTheme);
    };

}