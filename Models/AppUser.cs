using SQLite;

namespace Marcu_Nastasiu_Coworking_Mobile.Models
{
    public class AppUser
    {
        [PrimaryKey, AutoIncrement]
        public int AppUserId { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Indexed(Name = "IX_AppUser_Email", Unique = true)]
        [MaxLength(120)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(128)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Role { get; set; } = "User"; // first user becomes Admin
    }
}
