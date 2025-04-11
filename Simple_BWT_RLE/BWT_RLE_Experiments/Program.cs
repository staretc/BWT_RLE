using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWT_RLE_Lib;
using System.Text.Json;

namespace BWT_RLE_Experiments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var encoder = new BWT_RLE_Encoder();
            string path;
            int menuItem = ShowMenu();

            while (menuItem != 4)
            {
                switch (menuItem)
                {
                    case 1:
                        Console.Write("Введите путь к файлу: ");
                        path = Console.ReadLine();
                        if (!IsValidInputPath(path))
                        {
                            Console.WriteLine("Указан неправильный путь к файлу!");
                            break;
                        }
                        var compressedText = encoder.Encode(File.ReadAllText(path));
                        Console.Write("Введите путь к файлу для сохранения: ");
                        path = Console.ReadLine();
                        if (!IsValidInputPath(path))
                        {
                            Console.WriteLine("Указан неправильный путь к файлу!");
                            break;
                        }
                        File.WriteAllText(path, JsonSerializer.Serialize(compressedText));
                        Console.WriteLine("Успешно закодировано!");
                        Console.WriteLine($"Степень сжатия текста: {encoder.CompressionRatio}");
                        break;
                    case 2:
                        Console.Write("Введите путь к файлу: ");
                        path = Console.ReadLine();
                        if (!IsValidInputPath(path))
                        {
                            Console.WriteLine("Указан неправильный путь к файлу!");
                            break;
                        }
                        var compressedTextFromFile = JsonSerializer.Deserialize<Tuple<string, int>>(File.ReadAllText(path));
                        var decompressedText = encoder.Decode(compressedTextFromFile);
                        Console.Write("Введите путь к файлу для сохранения: ");
                        path = Console.ReadLine();
                        if (!IsValidInputPath(path))
                        {
                            Console.WriteLine("Указан неправильный путь к файлу!");
                            break;
                        }
                        File.WriteAllText(path, decompressedText);
                        Console.WriteLine("Успешно декодировано!");
                        break;
                    case 3:
                        if (encoder.CompressionRatio == 0)
                        {
                            Console.WriteLine("Сначала закодируйте строку!");
                            break;
                        }
                        Console.WriteLine($"Степень сжатия: {encoder.CompressionRatio}");
                        break;
                    default:
                        Console.WriteLine("Пожалуйста, выберите корректный пункт меню!");
                        break;
                }
                Console.ReadKey();
                menuItem = ShowMenu();
            }
        }
        static int ShowMenu()
        {
            string[] menu = { 
            "1. Преобразовать строку алгоритмом BWT и сжать алгоритмом RLE",
            "2. Восстановить преобразованную алгоритмом BWT и сжатую алгоритмом RLE строку",
            "3. Показать степень сжатия",
            "4. Выход"
            };
            int currentMenuItem = 0;
            ConsoleKeyInfo cki;
            do
            {
                Console.Clear();
                for (int i = 0; i < menu.Length; i++)
                {
                    if (currentMenuItem == i) Console.ForegroundColor = ConsoleColor.DarkRed;
                    else Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(menu[i]);
                }
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    currentMenuItem--;
                    if (currentMenuItem < 0) currentMenuItem = menu.Length - 1;
                }
                else if (cki.Key == ConsoleKey.DownArrow)
                {
                    currentMenuItem++;
                    if (currentMenuItem > menu.Length - 1) currentMenuItem = 0;
                }
                else if (cki.Key == ConsoleKey.Enter)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    return currentMenuItem + 1;
                }
            }
            while (true);
        }
        /// <summary>
        /// Проверяет валидность пути к входному файлу
        /// </summary>
        /// <param name="path">Путь, который необходимо проверить</param>
        /// <returns>Результат проверки на валидность</returns>
        private static bool IsValidInputPath(string path)
        {
            // Путь не должен содержать недопустимые символы, должен быть абсолютьным и по данному пути должен существовать файл
            return path.IndexOfAny(Path.GetInvalidPathChars()) == -1 &&
                   //Path.IsPathRooted(path) &&
                   File.Exists(path);
        }
    }
}
