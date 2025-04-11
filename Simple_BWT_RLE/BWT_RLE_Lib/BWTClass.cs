using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWT_RLE_Lib
{
    /// <summary>
    /// Класс алгоритма BWT
    /// </summary>
    internal static class BurrowsWhellerTransform
    {
        #region Methods

        /// <summary>
        /// Трансформация строки алгоритмом BWT
        /// </summary>
        /// <param name="inputText">Входная строка для трансформации</param>
        /// <returns>Пара [Трансформированная строка, Индекс на исходную строку в массиве сдвигов]</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Tuple<string, int> CreateTransform(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                throw new ArgumentException();
            }
            // получаем все сдвиго строки
            var shifts = GetAllShifts(inputText);
            // сортируем в лексикографическом порядке
            shifts.Sort();

            var stringBuilder = new StringBuilder();
            foreach (var shift in shifts)
            {
                // добавляем в выходную строку последние символы каждого сдвига
                stringBuilder.Append(shift[shift.Length - 1]);
            }

            var index = shifts.IndexOf(inputText);
            // возвращаем преобразованную стрроку и индекс на исходную строку в массиве сдвигов
            return new Tuple<string, int>(stringBuilder.ToString(), index);
        }
        /// <summary>
        /// Восстановление строки, трансформированной алгоритмом BWT
        /// </summary>
        /// <param name="encodedText">Трансофрмированная строка</param>
        /// <param name="originalStringIndex">Индекс исходной строки в массиве сдвигов</param>
        /// <returns>Восстановленная исходная строка</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string RestoreString(string encodedText, int originalStringIndex)
        {
            if (string.IsNullOrEmpty(encodedText))
            {
                throw new ArgumentException();
            }
            var shifts = new List<string>(encodedText.ToCharArray().Select(chr => chr.ToString()));
            for (int index = 0; index < shifts.Count - 1; index++)
            {
                shifts = RestoreShifts(shifts, index);
            }
            return shifts[originalStringIndex];
        }
        /// <summary>
        /// Получение всех сдвигов строки
        /// </summary>
        /// <param name="text">Строка, для которой получаем сдвиги</param>
        /// <returns>Список всех сдвигов строки</returns>
        private static List<string> GetAllShifts(string text)
        {
            var shifts = new List<string>();
            shifts.Add(text);
            for (int i = 1; i < text.Length; i++)
            {
                // Сдвиг на i позиций влево: первые i символов переносятся в конец
                string shifted = text.Substring(i) + text.Substring(0, i);
                shifts.Add(shifted);
            }
            return shifts;
        }
        /// <summary>
        /// Восстановление матрицы сдвигов в алгоритме BWT
        /// </summary>
        /// <param name="shifts">Массив сдвигов</param>
        /// <param name="index">Текущее количество восстановленных символов начала слова</param>
        /// <returns>[index+1] восстановленных символов начала слова</returns>
        private static List<string> RestoreShifts(List<string> shifts, int index)
        {
            // получаем новый массив сдвигов путём объединения в пары [Последний символ слова, Восстановленное начало слова]
            var newShifts = shifts.Select(str => str.Last() + str.Substring(0, index)).ToList();
            // сортируем полученный массив сдвигов
            newShifts.Sort();
            // возвращаем полученный массив, обратно приписывая к нему последние символы слова
            return newShifts.Select((str, i) => str + shifts[i].Last()).ToList();
        }

        #endregion
    }
}
