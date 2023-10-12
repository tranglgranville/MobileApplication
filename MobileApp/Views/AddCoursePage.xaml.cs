using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MobileApp.ModelClass;
using MobileApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCoursePage : ContentPage
    {
        private readonly int _selectedTermId;
        private bool verify = false;
        public AddCoursePage()
        {
            InitializeComponent();
        }
        public AddCoursePage(int termId)
        {
            InitializeComponent();
            _selectedTermId = termId;
        }
        #region Buttons Save/Cancel    
        async void SaveCourse_OnClicked(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(CourseName.Text) || string.IsNullOrWhiteSpace(InstructorName.Text) || string.IsNullOrWhiteSpace(Email.Text) || string.IsNullOrWhiteSpace(Phone.Text))
            {
                await DisplayAlert("Missing Information", "Please Fill in the Information", "OK");
                return;
            }
            if (PickedStatus.SelectedIndex == -1)
            {
                await DisplayAlert("Missing Status", "Please Select Course Status", "OK");
                return;
            }
            if (VerifyEmail() == false)
            {
                await DisplayAlert("Email is not valid", "Please enter an email", "OK");
                return;
            }
            await SQLiteDB.CourseAdd(_selectedTermId, CourseName.Text, StartDateCourse.Date, EndDateCourse.Date,
                PickedStatus.SelectedItem.ToString(), InstructorName.Text, Email.Text, Phone.Text, Noti.IsToggled, Note.Text);

            await Navigation.PopAsync();

        }       

        async void Cancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        #endregion
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
    }
}