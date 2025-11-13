#include "Application.hpp"

namespace GameLauncher {

    /// <summary>
    /// Конструктор приложения - выполняет начальную инициализацию
    /// </summary>
    Application::Application()
    {
        Initialize();
    }

    /// <summary>
    /// Запускает главную форму игры и основной цикл сообщений
    /// </summary>
    Void Application::Run()
    {
        // Создаем и запускаем главную форму игры
        Match3^ gameForm = gcnew Match3();
        System::Windows::Forms::Application::Run(gameForm);
    }

    /// <summary>
    /// Инициализирует настройки приложения (визуальные стили)
    /// </summary>
    Void Application::Initialize()
    {
        // Включение визуальных стилей для приложения
        System::Windows::Forms::Application::EnableVisualStyles();
        System::Windows::Forms::Application::SetCompatibleTextRenderingDefault(false);
    }

}