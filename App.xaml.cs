using Marcu_Nastasiu_Coworking_Mobile.Data;
using Marcu_Nastasiu_Coworking_Mobile.Pages;
using Marcu_Nastasiu_Coworking_Mobile.Models;

namespace Marcu_Nastasiu_Coworking_Mobile
{
    public partial class App : Application
    {
        public static CoworkingDatabase Database { get; private set; } = default!;

        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "coworking.db3");
            Database = new CoworkingDatabase(dbPath);

            MainPage = new NavigationPage(new LoadingPage());
        }

        public static void GoToAppShell()
        {
            Current!.MainPage = new AppShell();
        }

        public static void GoToLogin()
        {
            Current!.MainPage = new NavigationPage(new LoginPage());
        }

        public static void GoToRegister()
        {
            Current!.MainPage = new NavigationPage(new RegisterPage());
        }
    }
}
