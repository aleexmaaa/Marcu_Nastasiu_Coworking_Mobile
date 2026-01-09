using SQLite;
using Marcu_Nastasiu_Coworking_Mobile.Models;

namespace Marcu_Nastasiu_Coworking_Mobile.Data
{
    public class CoworkingDatabase
    {
        private readonly SQLiteAsyncConnection _db;
        private bool _initialized;

        public CoworkingDatabase(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitAsync()
        {
            if (_initialized) return;

            await _db.CreateTableAsync<AppUser>();
            await _db.CreateTableAsync<Reservation>();
            await _db.CreateTableAsync<Notification>();

            _initialized = true;
        }

        // USERS
        public Task<int> UsersCountAsync()
            => _db.Table<AppUser>().CountAsync();

        public Task<AppUser?> GetUserByEmailAsync(string email)
            => _db.Table<AppUser>()
                  .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        public Task<int> SaveUserAsync(AppUser user)
            => user.AppUserId == 0 ? _db.InsertAsync(user) : _db.UpdateAsync(user);

        public async Task<(bool ok, string errorMessage, AppUser? user)> RegisterAsync(
            string fullName, string email, string plainPassword)
        {
            await InitAsync();

            fullName = (fullName ?? "").Trim();
            email = (email ?? "").Trim();

            if (string.IsNullOrWhiteSpace(fullName))
                return (false, "Full name is required.", null);

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return (false, "Valid email is required.", null);

            if (string.IsNullOrWhiteSpace(plainPassword) || plainPassword.Length < 6)
                return (false, "Password must be at least 6 characters.", null);

            var existing = await GetUserByEmailAsync(email);
            if (existing != null)
                return (false, "This email is already registered.", null);

            int count = await UsersCountAsync();
            string role = (count == 0) ? "Admin" : "User";

            var user = new AppUser
            {
                FullName = fullName,
                Email = email,
                PasswordHash = AuthHelpers.HashPassword(plainPassword),
                Role = role
            };

            await SaveUserAsync(user);
            return (true, "", user);
        }

        public async Task<(bool ok, string errorMessage, AppUser? user)> LoginAsync(
            string email, string plainPassword)
        {
            await InitAsync();

            email = (email ?? "").Trim();

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return (false, "Valid email is required.", null);

            if (string.IsNullOrWhiteSpace(plainPassword))
                return (false, "Password is required.", null);

            var user = await GetUserByEmailAsync(email);
            if (user == null)
                return (false, "Invalid email or password.", null);

            if (user.PasswordHash != AuthHelpers.HashPassword(plainPassword))
                return (false, "Invalid email or password.", null);

            return (true, "", user);
        }

        // RESERVATIONS
        public Task<List<Reservation>> GetReservationsAsync()
            => _db.Table<Reservation>().OrderByDescending(r => r.StartTime).ToListAsync();

        public Task<Reservation?> GetReservationAsync(int id)
            => _db.Table<Reservation>().FirstOrDefaultAsync(r => r.ReservationId == id);

        public Task<int> SaveReservationAsync(Reservation item)
            => item.ReservationId == 0 ? _db.InsertAsync(item) : _db.UpdateAsync(item);

        public async Task<int> DeleteReservationAsync(Reservation item)
        {
            await _db.Table<Notification>()
                .Where(n => n.ReservationId == item.ReservationId)
                .DeleteAsync();

            return await _db.DeleteAsync(item);
        }

        // NOTIFICATIONS
        public Task<List<Notification>> GetNotificationsAsync()
            => _db.Table<Notification>().OrderByDescending(n => n.NotifyAt).ToListAsync();

        public Task<List<Notification>> GetNotificationsForReservationAsync(int reservationId)
            => _db.Table<Notification>()
                  .Where(n => n.ReservationId == reservationId)
                  .OrderByDescending(n => n.NotifyAt)
                  .ToListAsync();

        public Task<int> SaveNotificationAsync(Notification item)
            => item.NotificationId == 0 ? _db.InsertAsync(item) : _db.UpdateAsync(item);

        public Task<int> DeleteNotificationAsync(Notification item)
            => _db.DeleteAsync(item);
    }
}
