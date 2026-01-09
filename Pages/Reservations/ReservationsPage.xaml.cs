using Marcu_Nastasiu_Coworking_Mobile.Models;

namespace Marcu_Nastasiu_Coworking_Mobile.Pages;

public partial class ReservationsPage : ContentPage
{
    public ReservationsPage()
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
        var list = await App.Database.GetReservationsAsync();
        ReservationsView.ItemsSource = list;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ReservationEditPage(null));
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Reservation item)
            await Navigation.PushAsync(new ReservationEditPage(item));
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is not Button btn || btn.BindingContext is not Reservation item)
            return;

        bool ok = await DisplayAlert("Delete", $"Delete '{item.Title}'?", "Yes", "No");
        if (!ok) return;

        await App.Database.DeleteReservationAsync(item);
        await LoadAsync();
    }
}
