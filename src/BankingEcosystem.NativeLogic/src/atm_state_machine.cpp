#include "atm_state_machine.h"

static int g_currentState = ATM_STATE_IDLE;

// Valid state transitions
static bool isValidTransition(int from, int to) {
    switch (from) {
        case ATM_STATE_IDLE:          return to == ATM_STATE_CARD_INSERTED;
        case ATM_STATE_CARD_INSERTED: return to == ATM_STATE_PIN_ENTRY || to == ATM_STATE_IDLE;
        case ATM_STATE_PIN_ENTRY:     return to == ATM_STATE_AUTHENTICATED || to == ATM_STATE_ERROR || to == ATM_STATE_IDLE;
        case ATM_STATE_AUTHENTICATED: return to == ATM_STATE_TRANSACTION || to == ATM_STATE_IDLE;
        case ATM_STATE_TRANSACTION:   return to == ATM_STATE_DISPENSING || to == ATM_STATE_COMPLETED || to == ATM_STATE_ERROR;
        case ATM_STATE_DISPENSING:    return to == ATM_STATE_COMPLETED || to == ATM_STATE_ERROR;
        case ATM_STATE_COMPLETED:     return to == ATM_STATE_IDLE;
        case ATM_STATE_ERROR:         return to == ATM_STATE_IDLE;
        default: return false;
    }
}

int AtmFsm_GetState() {
    return g_currentState;
}

int AtmFsm_TransitionTo(int newState) {
    if (!isValidTransition(g_currentState, newState)) {
        return -1; // Invalid transition
    }
    g_currentState = newState;
    return 0; // Success
}

int AtmFsm_Reset() {
    g_currentState = ATM_STATE_IDLE;
    return 0;
}

const char* AtmFsm_GetStateName(int state) {
    switch (state) {
        case ATM_STATE_IDLE:          return "IDLE";
        case ATM_STATE_CARD_INSERTED: return "CARD_INSERTED";
        case ATM_STATE_PIN_ENTRY:     return "PIN_ENTRY";
        case ATM_STATE_AUTHENTICATED: return "AUTHENTICATED";
        case ATM_STATE_TRANSACTION:   return "TRANSACTION";
        case ATM_STATE_DISPENSING:    return "DISPENSING";
        case ATM_STATE_COMPLETED:     return "COMPLETED";
        case ATM_STATE_ERROR:         return "ERROR";
        default:                      return "UNKNOWN";
    }
}
