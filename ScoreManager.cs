using System.Collections.Generic;

namespace Match3GameCS
{
    /// <summary>
    /// Класс для управления счетом игры
    /// Отвечает за хранение, обновление и форматирование счета
    /// Реализует систему бонусов за комбо
    /// </summary>
    public class ScoreManager
    {
        // Константы для расчета очков
        private const int SCORE_PER_TILE = 10;  // Базовые очки за одну плитку

        // Словарь бонусных множителей в зависимости от количества удаленных плиток
        private readonly Dictionary<int, int> bonuses;

        // Текущее количество очков
        private int currentScore;

        /// <summary>
        /// Конструктор менеджера счета
        /// </summary>
        public ScoreManager()
        {
            currentScore = 0;

            // Инициализация системы бонусов:
            // Ключ - минимальное количество плиток, значение - множитель
            bonuses = new Dictionary<int, int>
            {
                { 15, 5 },  // 15+ плиток: множитель 5
                { 10, 3 },  // 10+ плиток: множитель 3  
                { 5, 2 }    // 5+ плиток: множитель 2
            };
        }

        /// <summary>
        /// Добавляет очки к текущему счету
        /// </summary>
        /// <param name="points">Количество очков для добавления</param>
        /// <returns>Отформатированная строка с текущим счетом</returns>
        public string AddScore(int points)
        {
            currentScore += points;
            return FormatScore(currentScore);
        }

        /// <summary>
        /// Сбрасывает счет до нуля
        /// </summary>
        public void ResetScore()
        {
            currentScore = 0;
        }

        /// <summary>
        /// Форматирует счет для отображения
        /// </summary>
        /// <param name="currentScore">Текущее количество очков</param>
        /// <returns>Отформатированная строка "Score: {число}"</returns>
        public string FormatScore(int currentScore)
        {
            return $"Score: {currentScore}";
        }

        /// <summary>
        /// Возвращает текущий счет
        /// </summary>
        public int GetCurrentScore() => currentScore;

        /// <summary>
        /// Рассчитывает бонусные очки на основе количества удаленных плиток
        /// </summary>
        /// <param name="tilesRemoved">Количество удаленных плиток за ход</param>
        /// <returns>Количество бонусных очков</returns>
        private int CalculateBonus(int tilesRemoved)
        {
            // Ищем подходящий бонус (проверяем от большего порога к меньшему)
            foreach (var pair in bonuses)
            {
                if (tilesRemoved >= pair.Key)
                {
                    // Бонус = количество плиток × множитель
                    return tilesRemoved * pair.Value;
                }
            }
            return 0; // Бонус не применяется
        }

        /// <summary>
        /// Добавляет очки на основе количества удаленных плиток
        /// Включает базовые очки и бонусы за комбо
        /// </summary>
        /// <param name="tilesRemoved">Количество удаленных плиток</param>
        /// <returns>Отформатированная строка с обновленным счетом</returns>
        public string AddScoreForTiles(int tilesRemoved)
        {
            if (tilesRemoved > 0)
            {
                // Базовые очки за каждую удаленную плитку
                int baseScore = tilesRemoved * SCORE_PER_TILE;

                // Бонус за комбо (чем больше плиток удалено за один ход - тем больше множитель)
                int comboBonus = CalculateBonus(tilesRemoved);

                // Общее количество очков за ход
                int totalScore = baseScore + comboBonus;

                // Добавляем очки и возвращаем отформатированную строку
                string score = AddScore(totalScore);
                return score;
            }

            // Если плиток не удалено, возвращаем текущий счет без изменений
            return FormatScore(currentScore);
        }
    }
}