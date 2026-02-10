#include "atm_hardware.h"
#include <cstring>
#include <cstdio>

static char g_currentCard[20] = { 0 };
static bool g_cardInserted = false;

int CardReader_Insert(const char* cardNumber) {
    if (g_cardInserted) return -1; // Card already inserted
    if (!cardNumber || strlen(cardNumber) == 0) return -2; // Invalid card

    strncpy_s(g_currentCard, sizeof(g_currentCard), cardNumber, _TRUNCATE);
    g_cardInserted = true;
    return 0; // Success
}

int CardReader_Eject() {
    if (!g_cardInserted) return -1; // No card to eject

    memset(g_currentCard, 0, sizeof(g_currentCard));
    g_cardInserted = false;
    return 0; // Success
}

int CardReader_GetCardNumber(char* buffer, int maxLen) {
    if (!g_cardInserted) return -1;
    if (!buffer || maxLen <= 0) return -2;

    strncpy_s(buffer, maxLen, g_currentCard, _TRUNCATE);
    return 0;
}

int CardReader_IsCardInserted() {
    return g_cardInserted ? 1 : 0;
}
