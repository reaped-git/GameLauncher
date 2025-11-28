#pragma once

#include <array>
#include <memory>
#include <string>
#include <vector>

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
        std::string playerName;

    public:
        /// <summary>
        /// Конструктор менеджера счета
        /// </summary>
        /// <param name="scoreLabel">Метка для отображения счета</param>
        ScoreManager();
        ScoreManager(const std::string& name);
        ScoreManager(const ScoreManager& other);

        // Оператор присваивания
        ScoreManager& operator=(const ScoreManager& other);

        // Перегрузка операторов
        ScoreManager& operator+=(Int64 points);
        ScoreManager operator+(Int64 points) const;
        bool operator>(const ScoreManager& other) const;
        bool operator<(const ScoreManager& other) const;

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

        // Работа с std::string
        void SetPlayerName(const std::string& name);
        std::string GetPlayerName() const;
        std::string GetFullScoreInfo() const;

        // Оператор вывода в строку
        operator std::string() const;
    };

}