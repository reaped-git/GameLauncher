using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Match3GameCS
{
    /// <summary>
    /// Главный класс приложения - управляет жизненным циклом
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Инициализация приложения - загрузка XAML разметки
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// Вызывается после инициализации фреймворка
        /// Создает и отображает главное окно приложения
        /// </summary>
        public override void OnFrameworkInitializationCompleted()
        {
            // Для десктопного приложения создаем главное окно
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}