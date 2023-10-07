using System.IO;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Lb = Library;

namespace Task1
{
    internal class Task1
    {
        static void Main(string[] args)
        {
            const string destFolder = @"C:\SkillFactory\Модуль 08\Task 1\TestFolder";

            try
            {
                if (Directory.Exists(destFolder))
                {
                    DelFilesByLastAccessDate(new DirectoryInfo(destFolder));
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
            Console.Write("Нажмите любую клавишу для выхода");
            Console.ReadKey();
        }

        private static void DelFilesByLastAccessDate(DirectoryInfo dr)
        {
            foreach (var fl in dr.GetFiles())
            {
                try
                {
                    if (DateTime.Now - fl.LastAccessTime > TimeSpan.FromMinutes(30))
                    {
                        DelFSObj<FileInfo>(fl);
                    }
                }
                catch (Exception ex)
                {
                    Lb.Library.ErrorHandler(ex.Message);
                }
            }

            foreach (var dir in dr.GetDirectories())
            {
                try
                {
                    DelFilesByLastAccessDate(dir);
                    if (DateTime.Now - dir.LastAccessTime > TimeSpan.FromMinutes(30))
                    {
                        DelFSObj<DirectoryInfo>(dir);
                    }
                }
                catch (Exception ex)
                {
                    Lb.Library.ErrorHandler(ex.Message);
                }
            }
        }

        private static void DelFSObj<T>(T Obj) where T : FileSystemInfo
        {
            Console.WriteLine(string.Concat(Obj.GetType() == typeof(FileInfo) ? "Файл " : "Каталог ", $"{Obj.FullName}\nДата последнего доступа - {Obj.LastAccessTime.ToString()}\n"));
            Obj.Delete();
        }
    }
}