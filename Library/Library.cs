using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Library
    {
        /// <summary>
        /// Метод вывода сообщения об ошибке красным цветом
        /// </summary>
        /// <param name="msg">Сообщение об ошибке</param>
        public static void ErrorHandler(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Отработка произошедшей ошибки с выходом из программы
        /// </summary>
        public static void ErrorReaction() 
        {
            Console.WriteLine("\nПроизошла аварийная ситуация. Программа завершает работу");
            System.Environment.Exit(-1);
            Console.Write("Нажмите любую кнопку для выхода...");
            Console.ReadKey();
        }
    }
}
