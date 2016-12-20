using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace ClassLibrary1
{
    public class Logic //decisions
    {
        public Student[] LoadStudentFromJson()
        {
            Task<bool> res = isFilePresent("data.json");
            if (!AsyncHelper.RunSync<bool>(() => isFilePresent("data.json")))
            {
                CreateJson();
            }
            Task<Student[]> r = LoadFromJsonAsync("data.json");
            Student[] students = r.Result;
            return students;
        }

        public async Task<bool> isFilePresent(string fileName)
        {
            var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
            return item != null;
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
            StorageFile localFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(JsonFile);
            await FileIO.WriteTextAsync(localFile, json);
            return;
        }

        internal static class AsyncHelper
        {
            private static readonly TaskFactory _myTaskFactory = new
              TaskFactory(CancellationToken.None,
                          TaskCreationOptions.None,
                          TaskContinuationOptions.None,
                          TaskScheduler.Default);

            public static TResult RunSync<TResult>(Func<Task<TResult>> func)
            {
                return AsyncHelper._myTaskFactory
                  .StartNew<Task<TResult>>(func)
                  .Unwrap<TResult>()
                  .GetAwaiter()
                  .GetResult();
            }

            public static void RunSync(Func<Task> func)
            {
                AsyncHelper._myTaskFactory
                  .StartNew<Task>(func)
                  .Unwrap()
                  .GetAwaiter()
                  .GetResult();
            }
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
