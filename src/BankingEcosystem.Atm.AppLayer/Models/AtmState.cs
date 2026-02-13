namespace BankingEcosystem.Atm.AppLayer.Models;

public enum AtmState
{
    Idle = 0,
    CardInserted = 1,
    PinEntry = 2,
    Authenticated = 3,
    Transaction = 4,
    Dispensing = 5,
    Completed = 6,
    Error = 7
}
