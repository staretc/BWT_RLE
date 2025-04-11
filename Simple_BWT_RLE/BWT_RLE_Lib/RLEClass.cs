using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWT_RLE_Lib
{
    /// <summary>
    /// Класс алгоритма RLE
    /// </summary>
    internal static class RunLengthEncoding
    {
        #region Methods

        /// <summary>
        /// Сжатие строки алгоритмом RLE
        /// </summary>
        /// <param name="inputText">Входная строка для сжатия</param>
        /// <returns>Строка вида [lexem,count,] для каждой последовательности подряд идущих символов</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string Encode(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                throw new ArgumentException();
            }

            var stringBuilder = new StringBuilder();
            var count = 1;
            char currentLexem = inputText[0];
            for (int index = 1; index < inputText.Length; index++)
            {
                // если текущий рассматриваемый символ и символ в буфере совпали
                if (currentLexem == inputText[index])
                {
                    // увеличиваем счётчик идущих подряд одинаковых символов
                    count++;
                    continue;
                }
                // если текущий рассматриваемый символ и символ в буфере совпали
                // то заносим символ из буфера с количеством повторений в выходную строку
                // чтобы установить однозначность кодирования, используем символы-разделители
                stringBuilder.Append(currentLexem.ToString() + ',' + count.ToString() + ',');
                currentLexem = inputText[index];
                count = 1;
            }
            // чтобы установить однозначность кодирования, используем символы-разделители
            stringBuilder.Append(currentLexem.ToString() + ',' + count.ToString() + ',');
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Восстановление строки, закодированной алгоритмом RLE
        /// </summary>
        /// <param name="encodedText">Закодированная строка</param>
        /// <returns>Восстановленная исходная строка</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string Decode(string encodedText)
        {
            if (string.IsNullOrEmpty(encodedText))
            {
                throw new ArgumentException();
            }
            var stringBuilder = new StringBuilder();
            char currentLexem = (char)0;
            var count = 0;
            foreach (var lexem in encodedText)
            {
                // если рассматриваем следующий символ
                if (currentLexem == (char)0)
                {
                    currentLexem = lexem;
                    continue;
                }
                // если пришли на символ-разделитель
                if (lexem == ',')
                {
                    // если не рассматривали количество текущего символа
                    if (count == 0)
                    {
                        continue;
                    }
                    // если есть счётчик текущего символа, переносим символ в выходную строку count раз
                    while (count > 0)
                    {
                        stringBuilder.Append(currentLexem);
                        count--;
                    }
                    // стираем из буфера текущий символ
                    currentLexem = (char)0;
                    continue;
                }
                // если при подсчёте счётчика попались не на цифру, бросаем исключение
                if (!char.IsDigit(lexem))
                {
                    throw new ArgumentException();
                }
                // если находимся на подсчёте числа, заносим следующую цифру
                count = count * 10 + (lexem - '0');
            }
            return stringBuilder.ToString();
        }

        #endregion
    }
}
