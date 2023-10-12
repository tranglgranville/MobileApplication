using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApp.ModelClass;
using MobileApp.Services;

namespace MobileApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTermPage : ContentPage
    {
        public AddTermPage()
        {
            InitializeComponent();
        }        
        async void SaveButton_OnClicked(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(TermName.Text))
            {
                await DisplayAlert("Missing Term Tittle", "Please Enter a Term Tittle", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(TermStart.ToString()) || string.IsNullOrWhiteSpace(TermEnd.ToString()))
            {
                await DisplayAlert("Missing Term Date", "Please Select a Date", "OK");
            }
            
            await SQLiteDB.TermAdd(TermName.Text, TermStart.Date, TermEnd.Date);
            await Navigation.PopAsync();
        }

        async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}