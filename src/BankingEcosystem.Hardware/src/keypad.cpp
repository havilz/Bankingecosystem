#include "atm_hardware.h"
#include <cstdio>
#include <cstring>

static bool g_keypadActive = true;

int Keypad_ReadPin(char* buffer, int maxLen) {
    if (!g_keypadActive) return -1;
    if (!buffer || maxLen < 7) return -2; // PIN needs at least 6 chars + null

    // In simulation mode, this just validates the buffer is ready
    // The actual PIN will be sent from the C# UI layer
    return 0; // Ready to receive
}

int Keypad_IsActive() {
    return g_keypadActive ? 1 : 0;
}
