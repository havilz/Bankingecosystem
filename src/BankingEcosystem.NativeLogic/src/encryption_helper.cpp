#include "encryption_helper.h"
#include <cstring>
#include <cstdio>

// Simple hash for simulation (NOT production-grade - use BCrypt in backend)
static unsigned long simpleHash(const char* str) {
    unsigned long hash = 5381;
    int c;
    while ((c = *str++))
        hash = ((hash << 5) + hash) + c;
    return hash;
}

int Encryption_HashPin(const char* pin, char* hashOutput, int maxLen) {
    if (!pin || !hashOutput || maxLen < 20) return -1;

    unsigned long hash = simpleHash(pin);
    snprintf(hashOutput, maxLen, "%lu", hash);
    return 0;
}

int Encryption_VerifyPin(const char* pin, const char* expectedHash) {
    if (!pin || !expectedHash) return -1;

    char computed[64] = { 0 };
    Encryption_HashPin(pin, computed, sizeof(computed));

    return (strcmp(computed, expectedHash) == 0) ? 1 : 0;
}
