using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ClassLibrary1
{
    public class Logic //decisions
    {
        public Student[] LoadStudentFromJson()
        {
            if (!File.Exists("data.json"))
            {
                CreateJson();
            }
            Task<Student[]> r = LoadFromJsonAsync("data.json");
            r.Start();
            r.Wait();
            Student[] students = r.Result;
            return students;
        }

        public void CreateJson()
        {
            
            Student[] students = new Student[10];
            for (int i = 0; i < 10; i++)
            {
                Student s = Student.Generate("M");
                students[i] = s;
            }
            string json = JsonConvert.SerializeObject(students);
            WriteToJsonAsync("data.json", json);
        }

        public Student SearchByID(Student[] student, string ID)
        {
            var s = student.FirstOrDefault(st => st.ID.Contains(ID));

            return s;
        }

        public static async Task<Student[]> LoadFromJsonAsync(string JsonFile)
        {
            string JsonString = await DeserializeFileAsync(JsonFile);
            if (JsonString != null)
                return JsonConvert.DeserializeObject<Student[]>(JsonString);
            return null;
        }

        private static async Task<string> DeserializeFileAsync(string fileName)
        {
            try
            {
                StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(localFile);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static async void WriteToJsonAsync(string JsonFile, string json)
        {
            StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(JsonFile);
            await FileIO.WriteTextAsync(localFile, json);
            return;
        }

        public double PercentComplete(IEnumerable<Course> courses)
        {
            var incompleteGrades = new[] { "F", "W", "I" };
            return courses
                .Select(c => incompleteGrades.Contains(c.Grade) ? 0 : 100)
                .DefaultIfEmpty()
                .Average(c => c);
            
        }
    }
}
