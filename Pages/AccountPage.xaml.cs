using Marcu_Nastasiu_Coworking_Mobile.Models;

namespace Marcu_Nastasiu_Coworking_Mobile.Pages
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            EmailLabel.Text = $"Email: {Session.Email}";
            RoleLabel.Text = $"Role: {Session.Role}";
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            Session.Logout();
            App.GoToLogin();
        }
    }
}
