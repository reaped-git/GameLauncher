#pragma once

#include <array>

namespace GameLauncher {

    using namespace System;
    using namespace System::Windows::Forms;
    using namespace System::Collections::Generic;

    /// <summary>
    /// Класс для управления счетом игры
    /// Отвечает за хранение, обновление и отображение счета
    /// </summary>
    class ScoreManager
    {
    private:
        static const Int64 SCORE_PER_TILE = 10;
        std::array<std::pair<Int64, Int64>, 3> bonuses;
        Int64 currentScore;       // Текущее количество очков

    public:
        /// <summary>
        /// Конструктор менеджера счета
        /// </summary>
        /// <param name="scoreLabel">Метка для отображения счета</param>
        ScoreManager();

        /// <summary>
        /// Деструктор
        /// </summary>
        ~ScoreManager() = default;

        /// <summary>
        /// Рассчитывает бонусные очки
        /// </summary>
        /// <param name="tilesRemoved">Количество удаленных плиток</param>
        Int64 CalculateBonus(Int64 tilesRemoved);

        /// <summary>
        /// Добавляет очки на основе количества удаленных плиток
        /// </summary>
        /// <param name="tilesRemoved">Количество удаленных плиток</param>
        String^ AddScoreForTiles(Int64 tilesRemoved);

        /// <summary>
        /// Добавляет очки к текущему счету
        /// </summary>
        /// <param name="points">Количество очков для добавления</param>
        String^ AddScore(Int64 points);

        /// <summary>
        /// Сбрасывает счет до нуля
        /// </summary>
        Void ResetScore();

        /// <summary>
        /// Возвращает текущий счет
        /// </summary>
        Int64 GetCurrentScore();

        /// <summary>
        /// Обновляет отображение счета на форме
        /// </summary>
        String^ FormatScore(Int64 currentScore);
    };

}