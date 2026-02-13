#include "../include/encryption_helper.h"
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

// Simple XOR Encryption ("Shared Secret")
// Key must match Backend Logic
static const char* XOR_KEY = "BANK_ECO_SECURE_2026";

int Encryption_EncryptPin(const char* rawPin, char* encryptedOutput, int maxLen) {
    if (!rawPin || !encryptedOutput || maxLen < 1) return -1;

    size_t pinLen = strlen(rawPin);
    size_t keyLen = strlen(XOR_KEY);
    
    // Output will be hex string, so we need 2 chars per byte + null terminator
    if ((size_t)maxLen < (pinLen * 2 + 1)) return -1;

    for (size_t i = 0; i < pinLen; ++i) {
        char encryptedByte = rawPin[i] ^ XOR_KEY[i % keyLen];
        sprintf(encryptedOutput + (i * 2), "%02X", (unsigned char)encryptedByte);
    }
    encryptedOutput[pinLen * 2] = '\0';

    return 0;
}
