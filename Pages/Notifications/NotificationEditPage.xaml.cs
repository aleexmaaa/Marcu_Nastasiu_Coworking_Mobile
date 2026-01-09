using Marcu_Nastasiu_Coworking_Mobile.Models;

namespace Marcu_Nastasiu_Coworking_Mobile.Pages;

public partial class NotificationEditPage : ContentPage
{
    private Notification _item;
    private List<Reservation> _reservations = new();

    public NotificationEditPage(Notification? edit)
    {
        InitializeComponent();
        _item = edit ?? new Notification();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await App.Database.InitAsync();
        _reservations = await App.Database.GetReservationsAsync();

        ReservationPicker.ItemsSource = _reservations;

        // default time
        var now = DateTime.Now;
        NotifyDate.Date = now.Date;
        NotifyTime.Time = new TimeSpan(now.Hour, now.Minute, 0);

        if (_item.NotificationId != 0)
        {
            MessageEntry.Text = _item.Message;

            var notify = _item.NotifyAt;
            NotifyDate.Date = notify.Date;
            NotifyTime.Time = notify.TimeOfDay;

            // select reservation
            var selected = _reservations.FirstOrDefault(r => r.ReservationId == _item.ReservationId);
            ReservationPicker.SelectedItem = selected;
        }
        else
        {
            // select first reservation if exists
            if (_reservations.Count > 0)
                ReservationPicker.SelectedIndex = 0;
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

        if (ReservationPicker.SelectedItem is not Reservation r)
        {
            ShowError("Select a reservation.");
            return;
        }

        var message = (MessageEntry.Text ?? "").Trim();
        if (message.Length < 2)
        {
            ShowError("Message is required (min 2 chars).");
            return;
        }

        var notifyAt = NotifyDate.Date + NotifyTime.Time;

        _item.ReservationId = r.ReservationId;
        _item.Message = message;
        _item.NotifyAt = notifyAt;

        await App.Database.SaveNotificationAsync(_item);
        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
