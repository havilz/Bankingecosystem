#include "transaction_validator.h"

int Validator_CheckWithdrawalLimit(double amount, double dailyLimit, double alreadyWithdrawn) {
    if (amount <= 0) return -1;                          // Invalid amount
    if ((alreadyWithdrawn + amount) > dailyLimit) return -2; // Exceeds daily limit
    return 0; // OK
}

int Validator_CheckSufficientBalance(double balance, double amount, double minBalance) {
    if (amount <= 0) return -1;                     // Invalid amount
    if ((balance - amount) < minBalance) return -2;  // Would go below minimum
    return 0; // OK
}

int Validator_CheckTransferAmount(double amount) {
    if (amount <= 0) return -1;
    if (amount > 100000000) return -2; // Max 100 juta per transfer
    return 0; // OK
}
