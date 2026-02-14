using System;
using BankingEcosystem.Atm.AppLayer.Services;

namespace BankingEcosystem.Atm.UI.Services;

public class UiService : IUiService
{
    private bool _isBusy;

    public event Action<bool>? IsBusyChanged;

    public void SetBusy(bool isBusy)
    {
        // Invoke on UI Thread if needed, but bool assignment is atomic enough for simple toggle
        // However, event invocation might need dispatcher if subscribers touch UI
        if (_isBusy != isBusy)
        {
            _isBusy = isBusy;
            IsBusyChanged?.Invoke(_isBusy);
        }
    }
}
