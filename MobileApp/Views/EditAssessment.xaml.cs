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
    public partial class EditAssessment : ContentPage
    {
        private readonly int _selectedAssessmentId;
        public EditAssessment(Assessment assess)
        {
            InitializeComponent();

            _selectedAssessmentId = assess.Id;

            AssessmentId.Text = assess.Id.ToString();
            AssessmentName.Text = assess.Name;
            StartAssessDate.Date = assess.Start;
            EndAssessDate.Date = assess.End;
            Notify.IsToggled = assess.Notification;
        }

        async void SaveAssessment_OnClicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(AssessmentName.Text))
            {
                await DisplayAlert("Missing assessment name", "Please enter assessment name", "OK");
                return;
            }
            await SQLiteDB.AssessmentUpdate(Int32.Parse(AssessmentId.Text), AssessmentName.Text, StartAssessDate.Date,
                EndAssessDate.Date, Notify.IsToggled);
            await Navigation.PopAsync();
        }

        async void Cancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void Delete_OnClicked(object sender, EventArgs e)
        {
            var command = await DisplayAlert("Delete Assesment?", "Delete Assessment?", "Yes", "No");
            if (command == true)
            {
                var id = int.Parse(AssessmentId.Text);
                await SQLiteDB.AssessmentRemove(id);
                await DisplayAlert("Assesssment Deleted", "Assessment Deleted", "OK");
            }
            else
            {
                await DisplayAlert("Delete Canceled", "Nothing Deleted", "OK");
            }
            await Navigation.PopAsync();
        }
        
    }
}