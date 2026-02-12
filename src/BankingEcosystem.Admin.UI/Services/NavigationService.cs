using CommunityToolkit.Mvvm.ComponentModel;

namespace BankingEcosystem.Admin.UI.Services;

/// <summary>
/// Simple navigation service that exposes a CurrentView for ContentControl binding.
/// </summary>
public partial class NavigationService : ObservableObject
{
    [ObservableProperty]
    private ObservableObject? _currentView;
}
