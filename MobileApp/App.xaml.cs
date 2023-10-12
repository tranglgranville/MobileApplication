using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApp.View;
using MobileApp.Services;

namespace MobileApp
{
    public partial class App : Application
    {
        
        public App()
        {
            InitializeComponent();

            if (Settings.FirstRun)
            {
                SQLiteDB.LoadSampleData();
                //SQLiteDB.LoadSyncSampleData();
                Settings.FirstRun = false;
            }
            var termMainPage = new TermMainPage();
            var navPage = new NavigationPage(termMainPage);
            MainPage = navPage;
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
