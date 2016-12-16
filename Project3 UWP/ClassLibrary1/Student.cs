using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Student
    {
        private static uint currentID;
        private static bool firstGenerate = false;

        public string FirstName { get { return _firstName; } set { _firstName = value; } }
        private string _firstName;

        public string LastName { get { return _lastName; } set { _lastName = value; } }
        private string _lastName;

        public string ID { get { return _id; } set { _id = value; } }
        private string _id;

        public List<Course> Courses { get { return _courses; } set { _courses = value; } }
        List<Course> _courses;
        public static List<string> maleN = new List<string>();
        public static List<string> femaleN = new List<string>();
        public static List<string> lastN = new List<string>();
        public static Random rnd = new Random();

        public Student(string first, string last, uint id, List<Course> courses)
        {
            _firstName = first;
            _lastName = last;
            _id = id.ToString("0-00-000");
            _courses = courses;
        }

        public Student(string first, string last, string id, List<Course> courses)
        {
            _firstName = first;
            _lastName = last;
            _id = id;
            _courses = courses;
        }

        public Student() { }

        public static void AddNames()
        {
            maleN.Add("Adam");
            maleN.Add("Austin");
            maleN.Add("Mike");
            maleN.Add("Harambe");

            femaleN.Add("Emily");
            femaleN.Add("Sabrina");
            femaleN.Add("Kelly");
            femaleN.Add("Maddie");

            lastN.Add("Clark");
            lastN.Add("Sousa");
            lastN.Add("Butts");
            lastN.Add("Snowball");
        }

        public static Student Generate(string gender)
        {
            if (firstGenerate == false)
            {
                AddNames();
                firstGenerate = true;
            }
            List<Course> c = new List<Course>();
            Course.ManageData();
            for (int i = 0; i < 4; i++)
            {
                c.Add(Course.Generate());
            }

            return new Student(GetName("M"), lastN[rnd.Next(0,4)] , currentID++, c);
        }


        public static string GetName(string gender)
        {
            switch (gender)
            {
                case "M":
                    return maleN[rnd.Next(0, 4)];
                case "F":
                    return femaleN[rnd.Next(0, 4)];
                default:
                    throw new ArgumentException("Argument is not a gender!", "gender");
            }
        }
    }
}


