using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApp.Services;
using MobileApp.ModelClass;

namespace MobileApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAssessment : ContentPage
    {
        private readonly int _selectedCourseId;
        public AddAssessment()
        {
            InitializeComponent();
        }
        public AddAssessment(int courseId)
        {
            InitializeComponent();
            _selectedCourseId = courseId;
        }
        #region Buttons
        async void SaveAssessment_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AssessmentName.Text))
            {
                await DisplayAlert("Missing Assessment Name", "Please Enter Assessment Name", "OK");
            }
            await SQLiteDB.AssessmentAdd(_selectedCourseId, AssessmentName.Text, StartDate.Date, EndDate.Date, Noti.IsToggled);
            await Navigation.PopAsync();
        }

        async void Cancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }        
        #endregion
    }
}