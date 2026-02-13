using System;
using BankingEcosystem.Interop;
using BankingEcosystem.Atm.AppLayer.Models;

namespace BankingEcosystem.Atm.AppLayer.Services;

public interface IAtmStateService
{
    AtmState CurrentState { get; }
    void TransitionTo(AtmState newState);
    void Reset();
}

public class AtmStateService : IAtmStateService
{
    public AtmState CurrentState 
    {
        get
        {
            try
            {
                return (AtmState)NativeLogicInterop.AtmFsm_GetState();
            }
            catch (DllNotFoundException)
            {
                // Fallback for design-time or if DLL is missing
                return _fallbackState;
            }
        }
    }

    private AtmState _fallbackState = AtmState.Idle;

    public void TransitionTo(AtmState newState)
    {
        try
        {
            int result = NativeLogicInterop.AtmFsm_TransitionTo((int)newState);
            if (result != 0)
            {
                throw new InvalidOperationException($"Invalid state transition: {CurrentState} -> {newState}");
            }
        }
        catch (DllNotFoundException)
        {
            // Simulation fallback
            _fallbackState = newState;
        }
    }

    public void Reset()
    {
        try
        {
            NativeLogicInterop.AtmFsm_Reset();
        }
        catch (DllNotFoundException)
        {
            _fallbackState = AtmState.Idle;
        }
    }
}
