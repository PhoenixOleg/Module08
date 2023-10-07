using T2 = Task2;
using Lb = Library;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Task2;
using System.Runtime;
using System.Data.Common;

namespace Task3
{
    /// <summary>
    /// Класс, содержащий информацию о размере удаленных файлов и их количестве
    /// </summary>
    public class DirInfo
    {
        private long filessize;
        private long filescount;

        public DirInfo() { }

        public long FilesSize
        {
            get
            {
                return filessize;
            }
            set
            {
                filessize = value;
            }
        }

        public long FilesCount
        {
            get 
            { 
            return filescount;
            }
            set 
            { 
                filescount = value; 
            }
        }

        public DirInfo(DirInfo firstdir, DirInfo seconddir)
        { 
            filessize =firstdir.FilesSize + seconddir.filessize;
            filescount = firstdir.filescount + seconddir.filescount; 
        }
    }


internal class Task3
    {
        static void Main(string[] args)
        {
            const string destFolder = @"C:\SkillFactory\Модуль 08\Task 3";
            long size = 0;
            DirInfo dirInfo = new DirInfo();

            try
            {
                if (Directory.Exists(destFolder))
                {
                    size = new DirectoryInfo(destFolder).GetDirectorySize(true);

                    dirInfo = DelFilesByLastAccessDate(new DirectoryInfo(destFolder));
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
            Console.WriteLine($"Исходный размер папки {destFolder}: {size.ToString("N0")} байт.");
            Console.WriteLine($"Освобождено: {dirInfo.FilesSize.ToString("N0")} байт в {dirInfo.FilesCount} файлах.");
            Console.WriteLine($"Текущий размер папки {destFolder}: {new DirectoryInfo(destFolder).GetDirectorySize(true).ToString("N0")} байт.");

            Console.Write("Нажмите любую клавишу для выхода");
            Console.ReadKey();
        }

        /// <summary>
        /// Рекурсивное удаление файлов по дате последнего доступа
        /// </summary>
        /// <param name="dr">Целевой каталог</param>
        /// <returns>Возвращает класс DirInfo (размер и количество удаленных файлов)</returns>
        private static DirInfo DelFilesByLastAccessDate(DirectoryInfo dr)
        {
            DirInfo dirInfo = new DirInfo();

            foreach (var fl in dr.GetFiles())
            {
                try
                {
                    if (DateTime.Now - fl.LastAccessTime > TimeSpan.FromMinutes(30))
                    {
                        long Length = DelFile(fl);
                        if (Length > -1)
                        {
                            dirInfo.FilesCount++;
                            dirInfo.FilesSize += Length;
                        }
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
                    dirInfo = new DirInfo(dirInfo, DelFilesByLastAccessDate(dir)); //много думал и искал
                    if (DateTime.Now - dir.LastAccessTime > TimeSpan.FromMinutes(30))
                    {
                        DelFolders(dir);
                    }
                }
                catch (Exception ex)
                {
                    Lb.Library.ErrorHandler(ex.Message);
                }
            }
            return dirInfo;
        }

        /// <summary>
        /// Метод получает размер файла, который надо удалить
        /// Если удаление прошло с ошибкой, возвращаемое значение устанавливается в -1, как маркер неудачи
        /// Иначе возвращается размер удаленного файла
        /// </summary>
        /// <param name="fl">Объект типа FileInfo, содержащий файл для удаления</param>      
        private static long DelFile(FileInfo fl)
        {
            long _size;
            try
            {
                _size = fl.Length;
                fl.Delete();
            }
            catch (Exception ex)
            {
                _size = -1;
                Lb.Library.ErrorHandler(ex.Message);
            }
            return _size;
        }

        /// <summary>
        /// Метод удаляет каталог с учетом даты последнего доступа
        /// </summary>
        /// <param name="dr"></param>
        private static void DelFolders(DirectoryInfo dr)
        {
            {
                try
                {
                    Console.WriteLine($"Каталог {dr.FullName}\nДата последнего доступа - {dr.LastAccessTime.ToString()}\n");
                    dr.Delete();
                }
                catch (Exception ex)
                {
                    Lb.Library.ErrorHandler(ex.Message);
                }
            }
        }
    }
}