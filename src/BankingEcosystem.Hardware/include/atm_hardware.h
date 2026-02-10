#pragma once

#ifdef HARDWARE_EXPORTS
    #define HARDWARE_API __declspec(dllexport)
#else
    #define HARDWARE_API __declspec(dllimport)
#endif

extern "C" {
    // Card Reader
    HARDWARE_API int CardReader_Insert(const char* cardNumber);
    HARDWARE_API int CardReader_Eject();
    HARDWARE_API int CardReader_GetCardNumber(char* buffer, int maxLen);
    HARDWARE_API int CardReader_IsCardInserted();

    // Cash Dispenser
    HARDWARE_API int CashDispenser_Dispense(int amount);
    HARDWARE_API int CashDispenser_GetRemainingCash();
    HARDWARE_API int CashDispenser_Refill(int amount);

    // Receipt Printer
    HARDWARE_API int ReceiptPrinter_Print(const char* receiptData);
    HARDWARE_API int ReceiptPrinter_IsReady();

    // Keypad
    HARDWARE_API int Keypad_ReadPin(char* buffer, int maxLen);
    HARDWARE_API int Keypad_IsActive();
}
