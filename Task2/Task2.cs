using System.IO;
using Lb = Library;
namespace Task2
{
    public class Task2
    {
        static void Main(string[] args)
        {
            
            const string destFolder = @"c:\";
            long size = 0;
            try
            {
                if (Directory.Exists(destFolder))
                {
                    size = (new DirectoryInfo(destFolder).GetDirectorySize(true));
                }
                else
                {
                    Console.WriteLine($"Директория {destFolder} не существует");
                }
            }
            catch (Exception ex)
            {
                Lb.Library.ErrorHandler(ex.Message);
            }
            Console.WriteLine($"Размер каталога {destFolder} равен {size.ToString("N0")} байт.");
            Console.Write("Нажмите любую клавишу для выхода");
            Console.ReadKey();
        }
    }

    public static class DirectoryInfoExtension
    {
        public static long GetDirectorySize(this DirectoryInfo dr, bool Recurcive)
        {
            long size = 0;
            foreach (var fl in dr.GetFiles())
            {
                try
                {
                    size += fl.Length;

                }
                catch (Exception ex)
                {
                    Lb.Library.ErrorHandler(ex.Message);
                }
            }

            if (Recurcive)
            {
                foreach (var dir in dr.GetDirectories())
                {
                    try
                    {
                        size += GetDirectorySize(dir, Recurcive);
                    }
                    catch (Exception ex)
                    {
                        Lb.Library.ErrorHandler(ex.Message);
                    }
                }
            }
            return size;
        }
    }
}