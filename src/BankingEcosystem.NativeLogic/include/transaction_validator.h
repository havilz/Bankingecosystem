#pragma once

#ifdef NATIVELOGIC_EXPORTS
    #define NATIVELOGIC_API __declspec(dllexport)
#else
    #define NATIVELOGIC_API __declspec(dllimport)
#endif

extern "C" {
    NATIVELOGIC_API int Validator_CheckWithdrawalLimit(double amount, double dailyLimit, double alreadyWithdrawn);
    NATIVELOGIC_API int Validator_CheckSufficientBalance(double balance, double amount, double minBalance);
    NATIVELOGIC_API int Validator_CheckTransferAmount(double amount);
}
