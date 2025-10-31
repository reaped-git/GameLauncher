﻿#include "ScoreManager.hpp"

namespace GameLauncher {

    /// <summary>
    /// Конструктор менеджера счета
    /// </summary>
    /// <param name="scoreLabel">Метка для отображения счета</param>
    ScoreManager::ScoreManager()
    {
        currentScore = 0;
        FormatScore(currentScore);
        // Инициализация словаря бонусов
        bonuses = { {
             { 15, 5 },  // 15+ плиток: множитель 5
             { 10, 3 },  // 10+ плиток: множитель 3  
             { 5,  2 }   // 5+ плиток:  множитель 2
         } };
    }

    ScoreManager::~ScoreManager()
    {
    }

    /// <summary>
    /// Добавляет очки к текущему счету
    /// </summary>
    /// <param name="points">Количество очков для добавления</param>
    String^ ScoreManager::AddScore(Int64 points)
    {
        currentScore += points;
        String^ score = FormatScore(currentScore);
        return score;
    }

    /// <summary>
    /// Сбрасывает счет до нуля
    /// </summary>
    Void ScoreManager::ResetScore()
    {
        currentScore = 0;
        FormatScore(currentScore);
    }

    /// <summary>
    /// Обновляет отображение счета на форме
    /// </summary>
    String^ ScoreManager::FormatScore(Int64 currentScore)
    {
        return "Score: " + currentScore.ToString();
    }

    /// <summary>
    /// Возвращает текущий счет
    /// </summary>
    Int64 ScoreManager::GetCurrentScore()
    {
        return currentScore;
    }

    /// <summary>
    /// Рассчитывает бонусные очки
    /// </summary>
    /// <param name="tilesRemoved">Количество удаленных плиток</param>
    Int64 ScoreManager::CalculateBonus(Int64 tilesRemoved)
    {
        // Ищем подходящий бонус (от большего порога к меньшему)
        for each (auto pair in bonuses)
        {
            if (tilesRemoved >= pair.first)
            {
                return tilesRemoved * pair.second;
            }
        }
        return 0;
    }

    /// <summary>
    /// Добавляет очки на основе количества удаленных плиток
    /// </summary>
    /// <param name="tilesRemoved">Количество удаленных плиток</param>
    String^ ScoreManager::AddScoreForTiles(Int64 tilesRemoved)
    {
        if (tilesRemoved > 0)
        {
            // Базовые очки за каждую удаленную плитку
            Int64 baseScore = tilesRemoved * SCORE_PER_TILE;

            // Бонус за комбо (чем больше плиток удалено за один ход - тем больше множитель)
            Int64 comboBonus = CalculateBonus(tilesRemoved);
                      
            Int64 totalScore = baseScore + comboBonus;
            String^ score = AddScore(totalScore);
            return score;
        }
    }
}
