using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWT_RLE_Lib
{
    /// <summary>
    /// Класс совместного применения алгоритмов BWT и RLE
    /// </summary>
    public class BWT_RLE_Encoder
    {
        #region Properties

        /// <summary>
        /// Коэффициент сжатия
        /// </summary>
        public float CompressionRatio { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Сжатие строки алгоритмами BWT и RLE
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns>Сжатая строка</returns>
        public Tuple<string, int> Encode(string inputText)
        {
            var tupleBWT = BurrowsWhellerTransform.CreateTransform(inputText);
            var tupleRLE = new Tuple<string, int>(RunLengthEncoding.Encode(tupleBWT.Item1), tupleBWT.Item2);
            GetCompressionRatio(inputText, tupleRLE);

            return tupleRLE;
        }
        /// <summary>
        /// Восстановление сжатой алгоритмами BWT и RLE строки
        /// </summary>
        /// <param name="compressedText">Сжатая строка</param>
        /// <returns>Восстановленная исходная строка</returns>
        public string Decode(Tuple<string, int> compressedText)
        {
            var decompressedText = RunLengthEncoding.Decode(compressedText.Item1);
            var restoredText = BurrowsWhellerTransform.RestoreString(decompressedText, compressedText.Item2);
            return restoredText;
        }
        /// <summary>
        /// Получение коэффициента сжатия текста
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="tuple"></param>
        private void GetCompressionRatio(string inputText, Tuple<string, int> tuple)
        {
            var sumInput = inputText.Length * 16; // размер входного текста в битах
            var sumCompressed = 0; // размер сжатого текста в битах
            var maxCountBits = 0; // максимальное количество бит на число подряд идущих символов в сжатом тексте
            var currentLexem = (char)0; // буфер для символа
            var count = 0; // счётчик подряд идущих символов
            var lexemsCount = 0; // счётчик различных последовательностей подряд идущих символов

            foreach (var lexem in tuple.Item1)
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
                    // если есть счётчик текущего символа, добавляем в сумму вес в битах символа + количество повторений
                    var bits = 1;
                    while (count / 2 >= 1)
                    {
                        bits++;
                        count /= 2;
                    }
                    maxCountBits = Math.Max(maxCountBits, bits);
                    lexemsCount++;
                    // стираем из буфера текущий символ
                    currentLexem = (char)0;
                    count = 0;
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

            sumCompressed = lexemsCount * (16 + maxCountBits);
            CompressionRatio = (float)sumInput / sumCompressed;
        }

        #endregion
    }
}
