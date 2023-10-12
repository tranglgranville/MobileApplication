using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApp.ModelClass;
using MobileApp.Services;
using Plugin.LocalNotifications;


namespace MobileApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermMainPage : ContentPage
    {       
        public TermMainPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MainPageCollectionView.ItemsSource = await SQLiteDB.GetTerms();

            //Notify for assesments
            var assessmentList = await SQLiteDB.GetAssessment();
            var notify = new Random();
            var notifyId = notify.Next(1000);

            foreach (Assessment assessmentRecord in assessmentList)
            {
                if(assessmentRecord.Notification == true)
                {
                    if (assessmentRecord.Start == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{assessmentRecord.Name} begins today!", notifyId);
                    }
                    if (assessmentRecord.End == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{assessmentRecord.Name} ends today!", notifyId);
                    }
                }
            }

            //Notify for course 
            var courseList = await SQLiteDB.GetCourse();
            var notify1 = new Random();
            var notifyId1 = notify1.Next(1000);

            foreach (Course courseRecord in courseList)
            {
                if(courseRecord.Notification == true)
                {
                    if (courseRecord.Start == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{courseRecord.Name} begins today!", notifyId1);
                    }
                    if (courseRecord.End == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{courseRecord.Name} ends today!", notifyId1);
                    }
                }
            }
        }
        private async void MainPageCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.CurrentSelection != null)
            {
                Term term = (Term)e.CurrentSelection.FirstOrDefault();
                
                await Navigation.PushAsync(new EditTerm(term));
            }
        }
        
        #region Buttons        
        
        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTermPage());
        }
        #endregion

        async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppSetting());
        }
    }
}