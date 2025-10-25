#include "UIManager.hpp"

namespace GameLauncher {

    /// <summary>
    /// Конструктор менеджера интерфейса - инициализирует цветовые схемы
    /// </summary>
    UIManager::UIManager(Form^ form)
    {
        mainForm = form;
        currentTheme = ColorTheme::eLight;

        // Инициализация цветов для светлой темы
        lightBackground = Color::White;
        lightForeground = Color::Black;
        lightControl = Color::WhiteSmoke;
        lightAccent = Color::FromArgb(0, 120, 215); // Синий акцент

        // Инициализация цветов для темной темы
        darkBackground = Color::FromArgb(32, 32, 32);   // Темно-серый фон
        darkForeground = Color::WhiteSmoke;             // Светлый текст
        darkControl = Color::FromArgb(64, 64, 64);      // Средне-серый для контролов
        darkAccent = Color::FromArgb(0, 122, 204);      // Синий акцент для темной темы
    }

    /// <summary>
    /// Деструктор
    /// </summary>
    UIManager::~UIManager()
    {
    }

    /// <summary>
    /// Инициализирует цвета интерфейса на основе переданных параметров
    /// </summary>
    Void UIManager::InitializeColors(Color white, Color black, Color gray)
    {
        // Используем переданные цвета или значения по умолчанию
        lightBackground = white;
        lightForeground = black;
        lightControl = gray;

        SetTheme(currentTheme); // Устанавливаем тему по умолчанию
    }

    /// <summary>
    /// Устанавливает указанную цветовую тему и применяет ее к интерфейсу
    /// </summary>
    Void UIManager::SetTheme(ColorTheme theme)
    {
        currentTheme = theme;

        // Применяем соответствующую тему
        if (theme == ColorTheme::eLight)
        {
            ApplyLightTheme();
        }
        else
        {
            ApplyDarkTheme();
        }
    }

    /// <summary>
    /// Применяет тему к игровым плиткам
    /// </summary>
    Void UIManager::ApplyThemeToTiles(array<System::Windows::Forms::Button^, 2>^ tiles, Int64 gridSize, Boolean isDarkTheme)
    {
        if (tiles == nullptr) return;

        try
        {
            for (Int64 i = 0; i < gridSize; i++)
            {
                for (Int64 j = 0; j < gridSize; j++)
                {
                    if (tiles[i, j] != nullptr)
                    {
                        if (isDarkTheme)
                        {
                            // Темная тема - серые рамки
                            tiles[i, j]->FlatAppearance->BorderColor = darkForeground;
                        }
                        else
                        {
                            // Светлая тема - черные рамки
                            tiles[i, j]->FlatAppearance->BorderColor = lightForeground;
                        }
                    }
                }
            }
        }
        catch (Exception^ ex)
        {
            System::Diagnostics::Debug::WriteLine("Error applying theme to tiles: " + ex->Message);
        }
    }

    /// <summary>
    /// Применяет светлую цветовую тему ко всей форме и ее элементам
    /// </summary>
    Void UIManager::ApplyLightTheme()
    {
        if (mainForm == nullptr) return;

        try
        {
            // Устанавливаем цвета для главной формы
            mainForm->BackColor = darkBackground;
            mainForm->ForeColor = lightForeground;

            // Применяем тему ко всем элементам управления формы
            // ApplyThemeToControl(mainForm, false);

            // Специфичные настройки для светлой темы
            // Используем reflection или известные имена контролов для безопасного доступа

            // Ищем контролы по имени
            Control^ toolBar = mainForm->Controls["toolBar"];
            Control^ background = mainForm->Controls["background"];
            Control^ buttonClose = mainForm->Controls["buttonClose"];
            Control^ buttonMinimize = mainForm->Controls["buttonMinimize"];
            Control^ buttonInfo = mainForm->Controls["buttonInfo"];
            Control^ scoreLabel = mainForm->Controls["scoreLabel"];

            // Настройка панели инструментов
            if (toolBar != nullptr)
            {
                toolBar->BackColor = lightBackground;
                toolBar->ForeColor = lightForeground;
            }

            // Настройка кнопок на панели инструментов
            if (buttonClose != nullptr)
            {
                buttonClose->BackColor = lightBackground;
                buttonClose->ForeColor = lightForeground;
                safe_cast<Button^>(buttonClose)->FlatAppearance->BorderColor = lightForeground;
            }

            if (buttonMinimize != nullptr)
            {
                buttonMinimize->BackColor = lightBackground;
                buttonMinimize->ForeColor = lightForeground;
                safe_cast<Button^>(buttonMinimize)->FlatAppearance->BorderColor = lightForeground;
            }

            if (buttonInfo != nullptr)
            {
                buttonInfo->BackColor = lightBackground;
                buttonInfo->ForeColor = lightForeground;
                safe_cast<Button^>(buttonInfo)->FlatAppearance->BorderColor = lightForeground;
            }

            // Настройка метки счета
            if (scoreLabel != nullptr)
            {
                scoreLabel->BackColor = lightBackground;
                scoreLabel->ForeColor = lightForeground;
            }

            // Настройка игрового поля
            if (background != nullptr)
            {
                background->BackColor = Color::Black; // Игровое поле всегда черное
            }

            // Принудительное обновление формы
            mainForm->Refresh();
        }
        catch (Exception^ ex)
        {
            System::Diagnostics::Debug::WriteLine("Error applying light theme: " + ex->Message);
        }
    }

    /// <summary>
    /// Применяет темную цветовую тему ко всей форме и ее элементам
    /// </summary>
    Void UIManager::ApplyDarkTheme()
    {
        if (mainForm == nullptr) return;

        try
        {
            // Устанавливаем цвета для главной формы
            mainForm->BackColor = lightBackground;
            mainForm->ForeColor = darkForeground;

            // Применяем тему ко всем элементам управления формы
            // ApplyThemeToControl(mainForm, true);

            // Ищем контролы по имени
            Control^ toolBar = mainForm->Controls["toolBar"];
            Control^ background = mainForm->Controls["background"];
            Control^ buttonClose = mainForm->Controls["buttonClose"];
            Control^ buttonMinimize = mainForm->Controls["buttonMinimize"];
            Control^ buttonInfo = mainForm->Controls["buttonInfo"];
            Control^ scoreLabel = mainForm->Controls["scoreLabel"];

            // Настройка панели инструментов
            if (toolBar != nullptr)
            {
                toolBar->BackColor = darkControl;
                toolBar->ForeColor = darkForeground;
            }

            // Настройка кнопок на панели инструментов
            if (buttonClose != nullptr)
            {
                buttonClose->BackColor = darkControl;
                buttonClose->ForeColor = darkForeground;
                safe_cast<Button^>(buttonClose)->FlatAppearance->BorderColor = darkForeground;
            }

            if (buttonMinimize != nullptr)
            {
                buttonMinimize->BackColor = darkControl;
                buttonMinimize->ForeColor = darkForeground;
                safe_cast<Button^>(buttonMinimize)->FlatAppearance->BorderColor = darkForeground;
            }

            if (buttonInfo != nullptr)
            {
                buttonInfo->BackColor = darkControl;
                buttonInfo->ForeColor = darkForeground;
                safe_cast<Button^>(buttonInfo)->FlatAppearance->BorderColor = darkForeground;
            }

            // Настройка метки счета
            if (scoreLabel != nullptr)
            {
                scoreLabel->BackColor = darkControl;
                scoreLabel->ForeColor = darkForeground;
            }

            // Настройка игрового поля
            if (background != nullptr)
            {
                background->BackColor = Color::FromArgb(16, 16, 16); // Очень темный серый
            }

            // Принудительное обновление формы
            mainForm->Refresh();
        }
        catch (Exception^ ex)
        {
            System::Diagnostics::Debug::WriteLine("Error applying dark theme: " + ex->Message);
        }
    }

    /// <summary>
    /// Рекурсивно применяет тему ко всем элементам управления формы
    /// </summary>
    //Void UIManager::ApplyThemeToControl(Control^ control, Boolean isDarkTheme)
    //{
    //    if (control == nullptr) return;
    //
    //    try
    //    {
    //        // Пропускаем специальные элементы, которые обрабатываются отдельно
    //        String^ controlName = control->Name;
    //        if (controlName == "toolBar" || controlName == "background" ||
    //            controlName == "buttonClose" || controlName == "buttonMinimize" ||
    //            controlName == "buttonInfo" || controlName == "scoreLabel")
    //        {
    //            return;
    //        }
    //
    //        // Применяем цвета в зависимости от темы
    //        if (isDarkTheme)
    //        {
    //            // Темная тема
    //            if (dynamic_cast<Panel^>(control) != nullptr)
    //            {
    //                control->BackColor = darkControl;
    //                control->ForeColor = darkForeground;
    //            }
    //            else if (dynamic_cast<Button^>(control) != nullptr)
    //            {
    //                Button^ button = safe_cast<Button^>(control);
    //                button->BackColor = darkControl;
    //                button->ForeColor = darkForeground;
    //                button->FlatAppearance->BorderColor = darkForeground;
    //            }
    //            else if (dynamic_cast<Label^>(control) != nullptr)
    //            {
    //                control->BackColor = darkBackground;
    //                control->ForeColor = darkForeground;
    //            }
    //        }
    //        else
    //        {
    //            // Светлая тема
    //            if (dynamic_cast<Panel^>(control) != nullptr)
    //            {
    //                control->BackColor = lightControl;
    //                control->ForeColor = lightForeground;
    //            }
    //            else if (dynamic_cast<Button^>(control) != nullptr)
    //            {
    //                Button^ button = safe_cast<Button^>(control);
    //                button->BackColor = lightControl;
    //                button->ForeColor = lightForeground;
    //                button->FlatAppearance->BorderColor = lightForeground;
    //            }
    //            else if (dynamic_cast<Label^>(control) != nullptr)
    //            {
    //                control->BackColor = lightBackground;
    //                control->ForeColor = lightForeground;
    //            }
    //        }
    //
    //        // Рекурсивно обрабатываем дочерние элементы
    //        for each(Control ^ childControl in control->Controls)
    //        {
    //            ApplyThemeToControl(childControl, isDarkTheme);
    //        }
    //    }
    //    catch (Exception^ ex)
    //    {
    //        System::Diagnostics::Debug::WriteLine("Error applying theme to control " + control->Name + ": " + ex->Message);
    //    }
    //}

    /// <summary>
    /// Показывает модальное диалоговое окно с информацией о игре
    /// </summary>
    Void UIManager::ShowInfoDialog()
    {
        String^ msg = "Game \"Three in a row\"\n\n"
            "How to Play:\n"
            "• Click adjacent tiles to swap them\n"
            "• Match 3 or more same-colored tiles in a row\n"
            "• Tiles can be matched horizontally or vertically\n"
            "• Chains and combos give more points!\n\n"
            "Features:\n"
            "• Switch between Light and Dark themes\n"
            "• Endless gameplay - play as long as you want!\n\n"
            "Tip: Plan your moves to create chain reactions!";

        MessageBox::Show(msg, "Game Information - Three in a Row",
            MessageBoxButtons::OK, MessageBoxIcon::Information);
    }

    ///// <summary>
    ///// Показывает диалоговое окно окончания игры с итоговым счетом игрока
    ///// </summary>
    //Void UIManager::ShowGameOverDialog(Int64 score)
    //{
    //}

}