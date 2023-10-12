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
    public partial class EditTerm : ContentPage
    {
        private readonly int _selectedTermId;
        protected override async void OnAppearing()
        {
            int countCourses = await SQLiteDB.GetCourseCountAsync(_selectedTermId);

            CountCourse.Text = countCourses.ToString();

            CourseCollectionView.ItemsSource = await SQLiteDB.GetCourse(_selectedTermId);
            
        }
        public EditTerm(Term term)
        {
            InitializeComponent();

            _selectedTermId = term.Id;

            TermId.Text = term.Id.ToString();
            TermName.Text = term.Title;
            TermStart.Date = term.Start;
            TermEnd.Date = term.End;
        }
        async void CourseCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var course = (Course)e.CurrentSelection.FirstOrDefault();
            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new EditCourse(course));
            }
        }
        #region Buttons
        async void SaveTerm_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TermName.Text))
            {
                await DisplayAlert("Missing Term Name", "Please Enter Term Name", "OK");
            }
            await SQLiteDB.TermUpdate(Int32.Parse(TermId.Text),TermName.Text,TermStart.Date,TermEnd.Date);

            await Navigation.PopAsync();
        }

        async void Cancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void DeleteTerm_OnClicked(object sender, EventArgs e)
        {
            var command = await DisplayAlert("Delete Term and Related Courses/ Assessments?", "Delete this Term", "Yes", "No");
            if(command == true)
            {
                var id = int.Parse(TermId.Text);
                await SQLiteDB.TermRemove(id);
                await DisplayAlert("Term Deleted", "Term Deleted", "OK");
            }
            else
            {
                await DisplayAlert("Canceling Delete", "Nothing Deleted", "OK");
            }
            await Navigation.PopAsync();
        }

        async void AddCourse_Clicked(object sender, EventArgs e)
        {
            var termId = Int32.Parse(TermId.Text);
            if (Int32.Parse(CountCourse.Text) <= 5)
            {
                await Navigation.PushAsync(new AddCoursePage(termId));
            }
            else
            {
                await DisplayAlert("No More Than 6 Course are in this Term", "Cancel Adding Course", "OK");
            }            
        }
        #endregion
        
    }
}