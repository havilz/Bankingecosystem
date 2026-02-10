#pragma once

#ifdef NATIVELOGIC_EXPORTS
    #define NATIVELOGIC_API __declspec(dllexport)
#else
    #define NATIVELOGIC_API __declspec(dllimport)
#endif

// ATM States
enum AtmState {
    ATM_STATE_IDLE = 0,
    ATM_STATE_CARD_INSERTED = 1,
    ATM_STATE_PIN_ENTRY = 2,
    ATM_STATE_AUTHENTICATED = 3,
    ATM_STATE_TRANSACTION = 4,
    ATM_STATE_DISPENSING = 5,
    ATM_STATE_COMPLETED = 6,
    ATM_STATE_ERROR = 7
};

extern "C" {
    NATIVELOGIC_API int  AtmFsm_GetState();
    NATIVELOGIC_API int  AtmFsm_TransitionTo(int newState);
    NATIVELOGIC_API int  AtmFsm_Reset();
    NATIVELOGIC_API const char* AtmFsm_GetStateName(int state);
}
