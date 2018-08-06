using System;
using System.Security.Cryptography;

namespace ZennoFramework.Utilities
{
    /// <summary>
    /// Представляет интервал значений.
    /// </summary>
    public class Range
    {
        private static readonly Random _random = new Random((int) DateTime.Now.Ticks & 0x0000FFFF);

        /// <summary>
        /// Начальное значение интервала.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Конечное значение интервала.
        /// </summary>
        public int To { get; set; }

        /// <summary>
        /// Возвращает случайное целое число из интервала.
        /// </summary>
        /// <returns></returns>
        public int Next()
        {
            Normalize();
            return _random.Next(From, To);
        }

        /// <summary>
        /// Возвращает случайное целое число в указанном диапазоне.
        /// </summary>
        /// <param name="from">Левая граница диапазона.</param>
        /// <param name="to">Правая граница диапазона.</param>
        /// <returns></returns>
        public static int Random(int from, int to)
        {
            return new Range {From = from, To = to}.Next();
        }

        private void Normalize()
        {
            if (From <= To)
                return;

            From += To;
            To = From - To;
            From -= To;
        }
    }
}