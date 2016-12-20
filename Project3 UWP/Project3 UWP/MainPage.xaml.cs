using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClassLibrary1;
using Windows.UI.Popups;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Project3_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Logic logic = new Logic();
        private Student[] students;

        private double numCore = 0.0;
        private double numGen = 0.0;
        private double numElective = 0.0;


        public MainPage()
        {
            
            this.InitializeComponent();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Ignore
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var id = txtBoxInput.Text;
            var student = logic.SearchByID(students, id);
            if (student != null)
            {
                ClearInfo();
                DisplayStudent(student);

                List<Course> courses = student.Courses;
                foreach (var course in courses)
                {
                    // I want the list box to display
                    // listBoxCourses.Items.Add(course.Name);
                    // while still holding the course information.
                    listBoxCourses.Items.Add(course);
                }
            }
            else
            {
                NullStudent();
            }
        
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            students = logic.LoadStudentFromJson();
            txtBlockName.Text = "";
            txtBlockID.Text = "";
        }

        private void DisplayStudent(Student student)
        {
            numGen = 0;
            numCore = 0;
            numElective = 0;

            foreach (Course course in student.Courses)
            {
                switch (course.CourseType)
                {
                    case "Core":
                        if (course.Grade != "F" && course.Grade != "W" && course.Grade != "I")
                        {
                            numCore++;
                        }
                        break;
                    case "General Education":
                        if (course.Grade != "F" && course.Grade != "W" && course.Grade != "I")
                        {
                            numGen++;
                        }
                        break;
                    case "Elective":
                        if (course.Grade != "F" && course.Grade != "W" && course.Grade != "I")
                        {
                            numElective++;
                        }
                        break;
                }

            }

            txtBlockName.Text = String.Format("{0} {1}", student.FirstName, student.LastName);
            txtBlockID.Text = student.ID;
            txtBlockCore.Text = (Math.Round((numCore / 26), 2) * 100).ToString();
            txtBlockElec.Text = (Math.Round((numElective / 8), 2) * 100).ToString();
            txtBlockGen.Text = (Math.Round((numGen / 8), 2) * 100).ToString();
            txtBlockOverall.Text = (Math.Round(((numCore + numGen + numElective) / 42), 2) * 100).ToString();





        }

        private async void NullStudent()
        {
            MessageDialog msgbox = new MessageDialog("No Student Record Found, Please Input Valid Student ID");
            await msgbox.ShowAsync();
        }

        private void listBoxCourse_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            Course course = (Course)listBoxCourses.SelectedItem;

            textBlockCourseName.Text = course.Name;
            textBlockCourseNumber.Text = course.Number;
            textBlockCourseType.Text = course.CourseType;
            textBlockCourseSemester.Text = course.Semster;
            textBlockCourseYear.Text = course.Year;
            textBlockCourseID.Text = course.ID.ToString();
            textBlockCourseGrade.Text = course.Grade;
            textBlockCourseCredits.Text = course.Credit;

        }

        private void ClearInfo()
        {
            txtBoxInput.Text = "";
            listBoxCourses.Items.Clear();

            textBlockCourseName.Text = "";
            textBlockCourseNumber.Text = "";
            textBlockCourseType.Text = "";
            textBlockCourseSemester.Text = "";
            textBlockCourseYear.Text = "";
            textBlockCourseID.Text = "";
            textBlockCourseGrade.Text = "";
            textBlockCourseCredits.Text = "";

        }
    }
}
