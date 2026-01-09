using SQLite;

namespace Marcu_Nastasiu_Coworking_Mobile.Models;

public class Reservation
{
    [PrimaryKey, AutoIncrement]
    public int ReservationId { get; set; }

    [NotNull]
    public string Title { get; set; } = string.Empty;

    [NotNull]
    public DateTime StartTime { get; set; }

    [NotNull]
    public DateTime EndTime { get; set; }

    public string Notes { get; set; } = string.Empty;
}
