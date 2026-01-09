using Marcu_Nastasiu_Coworking_Mobile.Models;

namespace Marcu_Nastasiu_Coworking_Mobile.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void ShowError(string msg)
        {
            ErrorLabel.Text = msg;
            ErrorLabel.IsVisible = true;
        }

        private void ClearError()
        {
            ErrorLabel.IsVisible = false;
            ErrorLabel.Text = "";
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            ClearError();

            string email = EmailEntry.Text?.Trim() ?? "";
            string pass = PasswordEntry.Text ?? "";

            var result = await App.Database.LoginAsync(email, pass);
            if (!result.ok || result.user == null)
            {
                ShowError(result.errorMessage);
                return;
            }

            Session.Login(result.user.AppUserId, result.user.Email, result.user.Role);
            App.GoToAppShell();
        }

        private void OnGoToRegisterClicked(object sender, EventArgs e)
        {
            App.GoToRegister();
        }
    }
}
