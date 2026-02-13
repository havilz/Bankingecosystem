@echo off
echo Setting up VS environment...
call "C:\Program Files\Microsoft Visual Studio\18\Community\VC\Auxiliary\Build\vcvars64.bat"
if %errorlevel% neq 0 (
    echo Failed to setup VS environment
    exit /b %errorlevel%
)
echo Compiling NativeLogic...
cl /LD /Fe:BankingEcosystem.NativeLogic.dll src\atm_state_machine.cpp src\transaction_validator.cpp src\encryption_helper.cpp
copy /Y BankingEcosystem.NativeLogic.dll ..\..\bin\Debug\net10.0\
copy /Y BankingEcosystem.NativeLogic.dll ..\BankingEcosystem.Atm.Client\bin\Debug\net10.0-windows\
exit /b %errorlevel%
