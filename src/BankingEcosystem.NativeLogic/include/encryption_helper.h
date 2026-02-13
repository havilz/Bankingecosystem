#pragma once

#ifdef NATIVELOGIC_EXPORTS
    #define NATIVELOGIC_API __declspec(dllexport)
#else
    #define NATIVELOGIC_API __declspec(dllimport)
#endif

extern "C" {
    NATIVELOGIC_API int Encryption_HashPin(const char* pin, char* hashOutput, int maxLen);
    NATIVELOGIC_API int Encryption_VerifyPin(const char* pin, const char* hash);
    NATIVELOGIC_API int Encryption_EncryptPin(const char* rawPin, char* encryptedOutput, int maxLen);
}
