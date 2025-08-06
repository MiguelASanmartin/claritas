@echo off
REM TaskManagementSystem/setup-windows.bat

echo Setting up Task Management System for Windows...
echo Using LocalDB for development...

echo Creating database...
cd TMS.Infrastructure
dotnet ef database update --startup-project ../TMS.Web.API

if %errorlevel% equ 0 (
    echo Setup complete!
    echo.
    echo Next steps:
    echo    1. Run: dotnet run --project TMS.Web.API
    echo    2. Open: https://localhost:7000/swagger
) else (
    echo Database setup failed. Check LocalDB installation.
    pause
)