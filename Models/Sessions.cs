namespace Marcu_Nastasiu_Coworking_Mobile.Models
{
    public static class Session
    {
        private const string KeyUserId = "session_user_id";
        private const string KeyEmail = "session_email";
        private const string KeyRole = "session_role";

        public static bool IsLoggedIn => Preferences.Get(KeyUserId, 0) != 0;

        public static int UserId => Preferences.Get(KeyUserId, 0);
        public static string Email => Preferences.Get(KeyEmail, "");
        public static string Role => Preferences.Get(KeyRole, "");

        public static void Login(int userId, string email, string role)
        {
            Preferences.Set(KeyUserId, userId);
            Preferences.Set(KeyEmail, email);
            Preferences.Set(KeyRole, role);
        }

        public static void Logout()
        {
            Preferences.Remove(KeyUserId);
            Preferences.Remove(KeyEmail);
            Preferences.Remove(KeyRole);
        }
    }
}
