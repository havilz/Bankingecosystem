using System;

namespace BankingEcosystem.Atm.AppLayer.Services;

public interface IUiService
{
    void SetBusy(bool isBusy);
    event Action<bool>? IsBusyChanged;
}
