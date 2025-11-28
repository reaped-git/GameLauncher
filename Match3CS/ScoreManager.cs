using System;
using System.Collections.Generic;
using System.IO;

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

        // Статическое поле - общий счет всех игроков (для статистики)
        private static int totalGlobalScore = 0;

        // Словарь бонусных множителей в зависимости от количества удаленных плиток
        private readonly Dictionary<int, int> bonuses;

        // Текущее количество очков
        private int currentScore;
        private string playerName;

        /// <summary>
        /// Статическое свойство для доступа к глобальному счету
        /// </summary>
        public static int TotalGlobalScore
        {
            get => totalGlobalScore;
            private set => totalGlobalScore = value;
        }

        /// <summary>
        /// Конструктор менеджера счета
        /// </summary>
        public ScoreManager()
        {
            currentScore = 0;
            playerName = "Player";

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
        /// Конструктор с именем игрока
        /// </summary>
        public ScoreManager(string playerName) : this()
        {
            this.playerName = playerName ?? throw new ArgumentNullException(nameof(playerName));
        }

        /// <summary>
        /// Статический метод для сброса глобальной статистики
        /// </summary>
        public static void ResetGlobalStats()
        {
            totalGlobalScore = 0;
        }

        /// <summary>
        /// Статический метод для получения статистики в виде строки
        /// </summary>
        public static string GetGlobalStats()
        {
            return $"Total Global Score: {totalGlobalScore}";
        }

        /// <summary>
        /// Добавляет очки к текущему счету
        /// </summary>
        /// <param name="points">Количество очков для добавления</param>
        /// <returns>Отформатированная строка с текущим счетом</returns>
        public string AddScore(int points)
        {
            if (points < 0)
                throw new ScoreOperationException("Cannot add negative points");

            currentScore += points;
            totalGlobalScore += points; // Обновляем глобальную статистику

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
        /// Сохраняет счет игрока в файл
        /// </summary>
        public void SaveScoreToFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty", nameof(filePath));

            try
            {
                // Используем using для автоматического закрытия файла
                using (var writer = new StreamWriter(filePath, append: true))
                {
                    writer.WriteLine($"{DateTime.Now}: {playerName} - {currentScore} points");
                }
            }
            catch (IOException ex)
            {
                throw new ScoreOperationException("Failed to save score to file", ex);
            }
        }

        /// <summary>
        /// Загружает историю счетов из файла
        /// </summary>
        public static string LoadScoreHistory(string filePath)
        {
            if (!File.Exists(filePath))
                return "No score history found";

            try
            {
                // Используем using для автоматического закрытия файла
                using (var reader = new StreamReader(filePath))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (IOException ex)
            {
                throw new ScoreOperationException("Failed to load score history", ex);
            }
        }

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
            if (tilesRemoved < 0)
                throw new ScoreOperationException("Cannot remove negative number of tiles");

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

        // Свойства с разумным использованием get/set

        /// <summary>
        /// Текущее количество очков (только для чтения)
        /// </summary>
        public int CurrentScore
        {
            get => currentScore;
            private set
            {
                if (value < 0)
                    throw new ScoreOperationException("Score cannot be negative");
                currentScore = value;
            }
        }

        /// <summary>
        /// Имя игрока
        /// </summary>
        public string PlayerName
        {
            get => playerName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Player name cannot be empty");
                playerName = value;
            }
        }

        /// <summary>
        /// Информация о счете игрока
        /// </summary>
        public string PlayerScoreInfo
        {
            get => $"{playerName}: {currentScore} points";
        }
    }
}