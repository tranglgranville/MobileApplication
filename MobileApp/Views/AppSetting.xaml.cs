using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApp.Services;
using MobileApp.ModelClass;
using Xamarin.Essentials;

namespace MobileApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppSetting : ContentPage
    {
        public AppSetting()
        {
            InitializeComponent();
        }

        private void ClearPreferences_OnClicked(object sender, EventArgs e)
        {
            Preferences.Clear();
        }

        async void LoadSampleData_OnClicked(object sender, EventArgs e)
        {
            if (Settings.FirstRun)
            {
                SQLiteDB.LoadSampleData();
                Settings.FirstRun = false;

                await Navigation.PopToRootAsync();
            }
        }

        async void ClearSampleData_OnClicked(object sender, EventArgs e)
        {
            await SQLiteDB.ClearSampleData();
        }        
    }
}