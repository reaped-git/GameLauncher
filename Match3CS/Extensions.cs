using System.Collections.Generic;
using System.Linq;

namespace Match3GameCS
{
    /// <summary>
    /// Методы расширения для работы с коллекциями
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Преобразует IEnumerable<IEnumerable<T>> в двумерный массив
        /// </summary>
        public static T[,] To2DArray<T>(this IEnumerable<IEnumerable<T>> source)
        {
            var array = source.ToArray();
            int rows = array.Length;
            int cols = array[0].Count();

            var result = new T[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                var row = array[i].ToArray();
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = row[j];
                }
            }

            return result;
        }

        /// <summary>
        /// Преобразует двумерный массив в зубчатый массив (jagged array)
        /// </summary>
        public static T[][] ToJaggedArray<T>(this T[,] source)
        {
            int rows = source.GetLength(0);
            int cols = source.GetLength(1);

            var result = new T[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new T[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = source[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Преобразует двумерный массив в IEnumerable<IEnumerable<T>>
        /// </summary>
        public static IEnumerable<IEnumerable<T>> ToEnumerable<T>(this T[,] source)
        {
            return source.ToJaggedArray().Select(row => row.AsEnumerable());
        }

    }
}