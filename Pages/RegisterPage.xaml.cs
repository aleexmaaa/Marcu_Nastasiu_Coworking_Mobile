using Marcu_Nastasiu_Coworking_Mobile.Models;

namespace Marcu_Nastasiu_Coworking_Mobile.Pages
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
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

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            ClearError();

            string fullName = FullNameEntry.Text?.Trim() ?? "";
            string email = EmailEntry.Text?.Trim() ?? "";
            string pass = PasswordEntry.Text ?? "";
            string confirm = ConfirmEntry.Text ?? "";

            if (pass != confirm)
            {
                ShowError("Passwords do not match.");
                return;
            }

            var result = await App.Database.RegisterAsync(fullName, email, pass);
            if (!result.ok || result.user == null)
            {
                ShowError(result.errorMessage);
                return;
            }

            // login session
            Session.Login(result.user.AppUserId, result.user.Email, result.user.Role);

            await DisplayAlert("Success", $"Account created. Role: {result.user.Role}", "OK");
            App.GoToAppShell();
        }

        private async void OnGoToLoginClicked(object sender, EventArgs e)
        {
            int count = await App.Database.UsersCountAsync();
            if (count == 0)
            {
                await DisplayAlert("Info", "No accounts exist yet. Please register the first account.", "OK");
                return;
            }

            App.GoToLogin();
        }
    }
}
