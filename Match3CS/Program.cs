using Avalonia;
using System;

namespace Match3GameCS
{
    /// <summary>
    /// Главный класс приложения - точка входа
    /// Инициализирует Avalonia UI и запускает главное окно
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        /// <summary>
        /// Настройка и конфигурация Avalonia приложения
        /// </summary>
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()    // Автоматическое определение платформы
                .LogToTrace();          // Логирование для отладки
    }
}