using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{
    [Serializable]
    public class Student
    {
        //public string name;
        //public string group;
        //public DateTime dateOfBirth;

        //public Student(string name, string group, DateTime dateOfBirth)
        //{
        //    this.name = name;
        //    this.group = group;
        //    this.dateOfBirth = dateOfBirth;
        //}

        //public string Name
        //{
        //    get
        //    {
        //        return name;
        //    }

        //    set
        //    {
        //        name = value;
        //    }
        //}

        public string Name { get; set; }
        public string Group {  get; set; }
        public DateTime DateOfBirth {  get; set; }  
    }

    internal class Task4
    {

        static void Main(string[] args)
        {
            const string FileWithStudents = @"C:\SkillFactory\Модуль 08\Task 4\Students.dat";
            ReadFile(FileWithStudents);

            Console.ReadKey();
        }



        static void ReadFile(string file)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Student[] students;

            

            if (File.Exists(file))
            {              
                using (FileStream fs = new FileStream(file , FileMode.Open))
                { 
                    students = (Student[])formatter.Deserialize(fs);
                }
            }
            else
            {
                Console.WriteLine();
                students = null;// new Student[0];
            }
            Console.WriteLine(students[1].Name);
        }
    }
}