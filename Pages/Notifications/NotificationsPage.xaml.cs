using Marcu_Nastasiu_Coworking_Mobile.Models;

namespace Marcu_Nastasiu_Coworking_Mobile.Pages;

public partial class NotificationsPage : ContentPage
{
    public NotificationsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await App.Database.InitAsync();
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        var list = await App.Database.GetNotificationsAsync();
        NotificationsView.ItemsSource = list;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NotificationEditPage(null));
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Notification item)
            await Navigation.PushAsync(new NotificationEditPage(item));
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is not Button btn || btn.BindingContext is not Notification item)
            return;

        bool ok = await DisplayAlert("Delete", "Delete notification?", "Yes", "No");
        if (!ok) return;

        await App.Database.DeleteNotificationAsync(item);
        await LoadAsync();
    }
}
