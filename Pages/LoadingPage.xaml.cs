using Marcu_Nastasiu_Coworking_Mobile.Models;

namespace Marcu_Nastasiu_Coworking_Mobile.Pages
{
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await App.Database.InitAsync();

            // daca avem deja sesiune -> AppShell
            if (Session.IsLoggedIn)
            {
                App.GoToAppShell();
                return;
            }

            // daca nu exista niciun user in DB -> Register
            int count = await App.Database.UsersCountAsync();
            if (count == 0)
            {
                App.GoToRegister();
            }
            else
            {
                App.GoToLogin();
            }
        }
    }
}
