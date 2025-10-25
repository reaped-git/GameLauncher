#pragma once

namespace GameLauncher {

    using namespace System;
    using namespace System::Windows::Forms;

    /// <summary>
    /// Класс для управления счетом игрока - отслеживание очков и обновление интерфейса
    /// </summary>
    public ref class ScoreManager
    {
    public:
        /// <summary>
        /// Количество очков за каждую удаленную плитку
        /// </summary>
        static const Int64 SCORE_PER_TILE = 1;

    private:
        Int64 score;        // Текущее количество очков
        Label^ scoreLabel;  // Ссылка на элемент интерфейса для отображения счета

    public:
        /// <summary>
        /// Конструктор менеджера счета
        /// </summary>
        /// <param name="label">Метка для отображения счета</param>
        ScoreManager(Label^ label);

        /// <summary>
        /// Деструктор
        /// </summary>
        ~ScoreManager();

        /// <summary>
        /// Добавляет очки к текущему счету
        /// </summary>
        /// <param name="points">Количество очков для добавления</param>
        Void AddScore(Int64 points);

        /// <summary>
        /// Сбрасывает счет к нулю
        /// </summary>
        Void ResetScore();

        /// <summary>
        /// Свойство для получения текущего счета (только чтение)
        /// </summary>
        property Int64 Score{ Int64 get() { return score; } }
            
        /// <summary>
        /// Обновляет отображение счета в интерфейсе
        /// </summary>
        Void UpdateDisplay();

    private:
        /// <summary>
        /// Форматирует счет для отображения в интерфейсе
        /// </summary>
        /// <returns>Отформатированная строка счета</returns>
        String^ FormatScore();
    };

}