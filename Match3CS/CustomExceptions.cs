using System;

namespace Match3GameCS
{
    /// <summary>
    /// Пользовательское исключение для ошибок инициализации игры
    /// </summary>
    public class GameInitializationException : Exception
    {
        public GameInitializationException() { }
        public GameInitializationException(string message) : base(message) { }
        public GameInitializationException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Пользовательское исключение для ошибок операций с плитками
    /// </summary>
    public class TileOperationException : Exception
    {
        public TileOperationException() { }
        public TileOperationException(string message) : base(message) { }
        public TileOperationException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Пользовательское исключение для ошибок операций со счетом
    /// </summary>
    public class ScoreOperationException : Exception
    {
        public ScoreOperationException() { }
        public ScoreOperationException(string message) : base(message) { }
        public ScoreOperationException(string message, Exception inner) : base(message, inner) { }
    }
}