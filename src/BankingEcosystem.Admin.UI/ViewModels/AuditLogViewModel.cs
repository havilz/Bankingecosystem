using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BankingEcosystem.Admin.UI.Services;
using BankingEcosystem.Shared.DTOs;

namespace BankingEcosystem.Admin.UI.ViewModels;

public partial class AuditLogViewModel : ObservableObject
{
    private readonly AdminApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<AuditLogDto> _logs = [];

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private int _page = 1;

    [ObservableProperty]
    private int _pageSize = 50;

    public bool CanGoPrevious => Page > 1;
    [ObservableProperty]
    private bool _canGoNext = true; // Assume true initially

    public AuditLogViewModel(AdminApiService apiService)
    {
        _apiService = apiService;
        LoadLogsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadLogsAsync()
    {
        if (IsLoading) return;
        IsLoading = true;
        Logs.Clear();

        try
        {
            var result = await _apiService.GetAuditLogsAsync(Page, PageSize);
            foreach (var item in result) Logs.Add(item);

            CanGoNext = result.Count == PageSize;
            OnPropertyChanged(nameof(CanGoPrevious));
        }
        catch
        {
            // Handle error
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (CanGoNext)
        {
            Page++;
            await LoadLogsAsync();
        }
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (CanGoPrevious)
        {
            Page--;
            await LoadLogsAsync();
        }
    }
}
