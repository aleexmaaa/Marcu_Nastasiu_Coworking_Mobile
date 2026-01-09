using SQLite;
using System;

namespace Marcu_Nastasiu_Coworking_Mobile.Models
{
    public class Notification
    {
        [PrimaryKey, AutoIncrement]
        public int NotificationId { get; set; }

        [Indexed]
        public int ReservationId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime NotifyAt { get; set; } = DateTime.Now;
        public bool IsSent { get; set; }
    }
}
