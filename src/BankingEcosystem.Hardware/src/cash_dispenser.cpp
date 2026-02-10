#include "atm_hardware.h"
#include <cstdio>

static int g_remainingCash = 50000000; // 50 juta Rupiah default

int CashDispenser_Dispense(int amount) {
    if (amount <= 0) return -1;            // Invalid amount
    if (amount > g_remainingCash) return -2; // Insufficient cash in ATM

    // Validate denomination (must be multiple of 50000)
    if (amount % 50000 != 0) return -3;

    g_remainingCash -= amount;
    return 0; // Success
}

int CashDispenser_GetRemainingCash() {
    return g_remainingCash;
}

int CashDispenser_Refill(int amount) {
    if (amount <= 0) return -1;

    g_remainingCash += amount;
    return 0; // Success
}
