#include "atm_hardware.h"
#include <cstdio>
#include <cstring>

static bool g_printerReady = true;

int ReceiptPrinter_Print(const char* receiptData) {
    if (!g_printerReady) return -1;   // Printer not ready
    if (!receiptData) return -2;       // No data

    // Simulate printing by logging to stdout
    printf("[RECEIPT PRINTER] Printing receipt...\n");
    printf("================================\n");
    printf("%s\n", receiptData);
    printf("================================\n");

    return 0; // Success
}

int ReceiptPrinter_IsReady() {
    return g_printerReady ? 1 : 0;
}
