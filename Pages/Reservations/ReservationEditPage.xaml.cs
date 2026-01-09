using Marcu_Nastasiu_Coworking_Mobile.Models;

namespace Marcu_Nastasiu_Coworking_Mobile.Pages;

public partial class ReservationEditPage : ContentPage
{
    private Reservation _item;

    public ReservationEditPage(Reservation? edit)
    {
        InitializeComponent();

        _item = edit ?? new Reservation();

        if (edit != null)
        {
            TitleEntry.Text = _item.Title;
            StartDate.Date = _item.StartTime.Date;
            StartTime.Time = _item.StartTime.TimeOfDay;
            EndDate.Date = _item.EndTime.Date;
            EndTime.Time = _item.EndTime.TimeOfDay;
        }
        else
        {
            var now = DateTime.Now;
            StartDate.Date = now.Date;
            EndDate.Date = now.Date;
            StartTime.Time = new TimeSpan(now.Hour, 0, 0);
            EndTime.Time = new TimeSpan(Math.Min(now.Hour + 1, 23), 0, 0);
        }
    }

    private void ShowError(string msg)
    {
        ErrorLabel.Text = msg;
        ErrorLabel.IsVisible = true;
    }

    private void ClearError() => ErrorLabel.IsVisible = false;

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        ClearError();

        var title = (TitleEntry.Text ?? "").Trim();
        if (title.Length < 2) { ShowError("Title is required (min 2 chars)."); return; }

        var start = StartDate.Date + StartTime.Time;
        var end = EndDate.Date + EndTime.Time;
        if (end <= start) { ShowError("End must be after Start."); return; }

        _item.Title = title;
        _item.StartTime = start;
        _item.EndTime = end;

        await App.Database.SaveReservationAsync(_item);
        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
