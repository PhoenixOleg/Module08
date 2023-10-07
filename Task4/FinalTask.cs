using System.Collections;
using System.ComponentModel.Design;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using static System.Formats.Asn1.AsnWriter;
using LB = Library;
namespace FinalTask
{
    /// <summary>
    /// Сериализуемый класс информации о студентах
    /// </summary>
    [Serializable]
    internal class Student
    {
        public string Name { get; set; }
        public string Group {  get; set; }
        public DateTime DateOfBirth {  get; set; }  
    }

    internal class Task4
    {
        private static void Main(string[] args)
        {
            const string fileWithStudents = @"C:\SkillFactory\Модуль 08\Task 4\Students.dat";
            string workingPath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\" + "Students";

            if (!CreateDirectory(workingPath))
            {
                LB.Library.ErrorReaction();
            }

            if (!ReadFile(fileWithStudents, out Student[] students))
            {
                LB.Library.ErrorReaction();
            }

            Console.WriteLine("Исходные данные:");
            DisplayContent(students);

            ArrayList Groups = GetGroupsList(students);
      
            Console.WriteLine("\nСписок групп:");
            DisplayGroups (Groups);

            if (WriteStudentsByGroups(students, Groups, workingPath))
            {
                Console.WriteLine("\nДанные записаны в файлы успешно!");
            }
            else
            {
                LB.Library.ErrorReaction();
            }

            Console.Write("Нажмите любую кнопку для выхода...");
            Console.ReadKey();
        }

        /// <summary>
        /// Метод создание каталога
        /// </summary>
        /// <param name="dir">Путь к каталогу для создания</param>
        /// <returns>Результат создания true/false</returns>
        private static bool CreateDirectory(string dir)
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    Console.WriteLine($"Каталог {dir} уже существует");
                    return true;
                }
                else
                {
                    Directory.CreateDirectory(dir);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LB.Library.ErrorHandler(ex.Message);
                return false;
            }
}

        /// <summary>
        /// Чтение из файла с десериализацией в класс
        /// </summary>
        /// <param name="file">Файл для чтения</param>
        /// <param name="students">Массив объектов</param>
        /// <returns>Результат работы true/false</returns>
        private static bool ReadFile(string file, out Student[] students)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                if (File.Exists(file))
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                        students = (Student[])formatter.Deserialize(fs);
                    }
                }
                else
                {
                    students = null;
                    LB.Library.ErrorHandler($"Файл {file} не существует");
                    return false;
                }
            }
            catch (Exception ex) 
            {
                students = null;
                LB.Library.ErrorHandler(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Отображение прочитанного содержимого файла
        /// </summary>
        /// <param name="students">Массив объектов</param>
        private static void DisplayContent(Student[] students)
        {
            foreach (Student student in students)
            {
                Console.WriteLine($"{student.Name} {student.Group} {student.DateOfBirth:dd.MM.yyyy}");
            }
        }

        /// <summary>
        /// Получение списка групп из массива классов
        /// </summary>
        /// <param name="students">Массив классов (объектов)</param>
        /// <returns>Список групп</returns>
        private static ArrayList GetGroupsList(Student[] students)
        {
            ArrayList groups = new ArrayList();
            foreach (Student student in students)
            {
                if (groups.IndexOf(student.Group) == -1)
                {
                    groups.Add(student.Group);
                }
            }
            groups.Sort();
            return groups;
        }

        /// <summary>
        /// Метод отбражения списка групп студентов
        /// </summary>
        /// <param name="groups">Список групп</param>
        private static void DisplayGroups(ArrayList groups)
        {
            foreach (string group in groups)
            { 
                Console.WriteLine(group); 
            }
        }

        /// <summary>
        /// Запись информации о студентах в файлы в зависимости от принадлежности к группе
        /// </summary>
        /// <param name="students">Массив классов с информациней о студентах</param>
        /// <param name="Groups">Список групп</param>
        /// <param name="path">Целевой путь для создания/перезаписи файлов</param>
        /// <returns>Результат работы true/false</returns>
        private static bool WriteStudentsByGroups(Student[] students, ArrayList Groups, string path)
        {
            try
            {
                foreach (string _group in Groups)
                {
                    //Немного Linq. Решил, что это наиболее рациональная реализация с т.з. дисковых операций, скорости выполнения и удобства чтения кода (количества строк и т.п.)
                    //https://learn.microsoft.com/ru-ru/dotnet/csharp/linq/query-a-collection-of-objects

                    var selector =
                    from student in students
                    where student.Group == _group
                    select new
                    {
                        Name = student.Name
                        ,
                        DateOfBirth = student.DateOfBirth
                    };

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(path);
                    stringBuilder.Append(@"\");
                    stringBuilder.Append("Group - ");
                    stringBuilder.Append(_group);
                    stringBuilder.Append(".txt");

                    using StreamWriter sw = File.CreateText(stringBuilder.ToString());
                    {
                        foreach (var item in selector)
                        {
                            sw.WriteLine($"{item.Name}, {item.DateOfBirth}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LB.Library.ErrorHandler(ex.Message);
                return false;
            }
            return true;
        }
    }
}