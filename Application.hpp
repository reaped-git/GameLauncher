#pragma once

#include "Match3Game.hpp"

namespace GameLauncher {

    using namespace System;
    using namespace System::Windows::Forms;

    /// <summary>
    /// Главный класс приложения
    /// </summary>
    class Application
    {
    public:
        /// <summary>
        /// Конструктор приложения
        /// </summary>
        Application();

        /// <summary>
        /// Запуск основного цикла приложения
        /// </summary>
        Void Run();

    private:
        /// <summary>
        /// Инициализация приложения (настройка визуальных стилей)
        /// </summary>
        Void Initialize();
    };

}