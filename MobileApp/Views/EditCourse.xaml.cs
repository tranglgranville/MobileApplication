using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApp.Services;
using MobileApp.ModelClass;
using Xamarin.Essentials;

namespace MobileApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCourse : ContentPage
    {
        private readonly int _selectedCourseId;
        private bool verify = false;
        protected override async void OnAppearing()
        {
            int countAssessments = await SQLiteDB.GetAssessmentCountAsync(_selectedCourseId);

            CountAssess.Text = countAssessments.ToString();

            AssessmentCollectionView.ItemsSource = await SQLiteDB.GetAssessment(_selectedCourseId);
        }
        public EditCourse(Course course)
        {
            InitializeComponent();

            _selectedCourseId = course.Id;

            CourseId.Text = course.Id.ToString();
            CourseName.Text = course.Name;
            StartDateCourse.Date = course.Start;
            EndDateCourse.Date = course.End;
            CourseStatus.SelectedItem = course.Status;
            InstructorName.Text = course.InstructorName;
            Email.Text = course.InstructorEmail;
            Phone.Text = course.InstructorPhone;
            CourseNote.Text = course.Note;
            Noti.IsToggled = course.Notification;
        }
        #region Buttons Save/Cancel/Delete
        async void SaveCourse_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CourseName.Text) || string.IsNullOrWhiteSpace(InstructorName.Text) || string.IsNullOrWhiteSpace(Phone.Text) || string.IsNullOrWhiteSpace(Email.Text))
            {
                await DisplayAlert("Missing Information", "Please Fill in the Information", "OK");
                return;
            }
            if (CourseStatus.SelectedIndex == -1)
            {
                await DisplayAlert("Missing course status", "Please select course status", "OK");
                return;
            }                
            if (VerifyEmail() == false)
            {
                await DisplayAlert("Email is not valid", "Please enter an email", "OK");
                return;
            }
            await SQLiteDB.CourseUpdate(Int32.Parse(CourseId.Text), CourseName.Text, StartDateCourse.Date, EndDateCourse.Date,
                CourseStatus.SelectedItem.ToString(), InstructorName.Text, Email.Text, Phone.Text, Noti.IsToggled, CourseNote.Text);
            await Navigation.PopAsync();
        }
        private bool VerifyEmail()
        {
            var email = Email.Text;
            var emailpattern = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";

            if (Regex.IsMatch(email, emailpattern))
            {
                verify = true;
            }
            else
            {
                verify = false;
            }
            return verify;
        }
        async void Cancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void Delete_OnClicked(object sender, EventArgs e)
        {
            var command = await DisplayAlert("Delete this Course?", "Delete this Course?", "Yes", "No");
            if (command == true)
            {
                var id = int.Parse(CourseId.Text);
                await SQLiteDB.CourseRemove(id);
                await DisplayAlert("Course Deleted", "Course Deleted", "OK");
            }
            else
            {
                await DisplayAlert("Delete Canceled", "Nothing Deleted", "OK");
            }
            await Navigation.PopAsync();
        }
        #endregion
        async void AssessmentCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var assessment = (Assessment)e.CurrentSelection.FirstOrDefault();
            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new EditAssessment(assessment));
            }
        }

        async void AddAssessment_Clicked(object sender, EventArgs e)
        {
            var courseId = Int32.Parse(CourseId.Text);
            
            if (Int32.Parse(CountAssess.Text) <= 1)
            {
                await Navigation.PushAsync(new AddAssessment(courseId));
            }
            else
            {
                await DisplayAlert("No More than 2 Assessments are in this Course", "Cancel Adding Assessment", "OK");
            }
        }              

        async void ShareButton_Clicked(object sender, EventArgs e)
        {
            var text = CourseNote.Text;
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Share Note"
            });
        }

        
    }
}