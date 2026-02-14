using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace BankingEcosystem.Atm.UI.ViewModels;

public partial class ErrorViewModel : ObservableObject
{
    [ObservableProperty]
    private string errorMessage = "An unexpected error occurred.";

    [RelayCommand]
    private void Close()
    {
        // Simple close logic for now, or navigation back
        // For a critical error dialog, we might just shutdown or restart the app
        // But for this view model, let's assume it's used in a Window or Overlay
        Application.Current.Shutdown(); 
    }
}
