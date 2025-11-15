@echo off
setlocal enabledelayedexpansion
set EXIT_CODE=0

set SCRIPT_DIR=%~dp0
if "%SCRIPT_DIR:~-1%"=="\" set SCRIPT_DIR=%SCRIPT_DIR:~0,-1%
pushd "%SCRIPT_DIR%" || goto :fail

goto :main

:main
call :banner
call :require dotnet
call :require python
call :require npm
call :require powershell

call :ensureEnv "%SCRIPT_DIR%\backend\python\.env" "%SCRIPT_DIR%\backend\python\.env.example"
call :ensureEnv "%SCRIPT_DIR%\developer-portal\.env" "%SCRIPT_DIR%\developer-portal\.env.example"

call :setupPython "%SCRIPT_DIR%\backend\python"
call :setupBrowserController "%SCRIPT_DIR%\backend\browser-controller"
call :setupDeveloperPortal "%SCRIPT_DIR%\developer-portal"
call :buildDesktop "%SCRIPT_DIR%\desktop\HarmanAI.Desktop\HarmanAI.Desktop.csproj" "%SCRIPT_DIR%\publish\desktop"
call :licenseActivation

echo.
echo Harman AI setup finished successfully.
set EXIT_CODE=0
goto :cleanup

:fail
echo.
echo Setup aborted due to an earlier error (code %errorlevel%).
if not "%errorlevel%"=="" set EXIT_CODE=%errorlevel%
goto :cleanup

:cleanup
popd
pause
exit /b %EXIT_CODE%

:banner
echo ==================================================
echo   Harman AI - One Click Setup and Activation
echo.
echo   Root Directory: %SCRIPT_DIR%
echo ==================================================
echo.
goto :eof

:require
where %1 >nul 2>&1
if errorlevel 1 (
    echo Missing required command: %1
    echo Please install it and re-run this script.
    goto :fail
) else (
    echo Verified dependency: %1
)
goto :eof

:ensureEnv
if exist %~1 (
    echo Found %~1
) else (
    echo Creating %~1 from template.
    if not exist %~2 (
        echo Template %~2 missing. Cannot continue.
        goto :fail
    )
    copy %~2 %~1 >nul || goto :fail
    echo Created %~1. Please review and update sensitive values after setup.
)
goto :eof

:setupPython
echo.
echo --- Configuring Python backend ---
set PY_DIR=%~1
set PY_VENV=%PY_DIR%\.venv
if not exist "%PY_DIR%" (
    echo Python backend directory not found: %PY_DIR%
    goto :fail
)
if not exist "%PY_VENV%" (
    echo Creating Python virtual environment...
    python -m venv "%PY_VENV%" || goto :fail
) else (
    echo Using existing Python virtual environment.
)
call "%PY_VENV%\Scripts\activate.bat" || goto :fail
python -m pip install --upgrade pip || goto :fail
pip install -r "%PY_DIR%\requirements.txt" || goto :fail
call "%PY_VENV%\Scripts\deactivate.bat" || goto :fail
echo Python backend ready.
goto :eof

:setupBrowserController
echo.
echo --- Installing browser controller dependencies ---
set BROWSER_DIR=%~1
if not exist "%BROWSER_DIR%" (
    echo Browser controller directory not found: %BROWSER_DIR%
    goto :fail
)
pushd "%BROWSER_DIR%"
npm install || goto :fail
popd
echo Browser controller ready.
goto :eof

:setupDeveloperPortal
echo.
echo --- Installing developer portal and provisioning database ---
set PORTAL_DIR=%~1
if not exist "%PORTAL_DIR%" (
    echo Developer portal directory not found: %PORTAL_DIR%
    goto :fail
)
pushd "%PORTAL_DIR%"
npm install || goto :fail
npx prisma generate || goto :fail
npx prisma migrate deploy || goto :fail
npm run build || goto :fail
popd
echo Developer portal ready.
goto :eof

:buildDesktop
echo.
echo --- Building Harman AI desktop host ---
set DESKTOP_PROJECT=%~1
set PUBLISH_DIR=%~2
if not exist "%DESKTOP_PROJECT%" (
    echo Desktop project file not found: %DESKTOP_PROJECT%
    goto :fail
)
dotnet restore "%DESKTOP_PROJECT%" || goto :fail
if not exist "%PUBLISH_DIR%" mkdir "%PUBLISH_DIR%"
dotnet publish "%DESKTOP_PROJECT%" -c Release -o "%PUBLISH_DIR%" || goto :fail
echo Desktop binaries published to %PUBLISH_DIR%
goto :eof

:licenseActivation
echo.
echo --- License activation ---
set DEFAULT_ACTIVATE_URL=http://localhost:3000/api/license/activate
set /p LICENSE_SERVER_URL=Enter license activation endpoint [%DEFAULT_ACTIVATE_URL%]:
if "%LICENSE_SERVER_URL%"=="" set LICENSE_SERVER_URL=%DEFAULT_ACTIVATE_URL%
set /p LICENSE_KEY=Enter license key (leave blank to skip activation):
if "%LICENSE_KEY%"=="" (
    echo Skipping license activation per request.
    goto :eof
)
for /f "usebackq tokens=*" %%I in (`powershell -NoProfile -Command "(Get-CimInstance -Class Win32_ComputerSystemProduct).UUID"`) do (
    if not defined HARDWARE_ID set HARDWARE_ID=%%I
)
if not defined HARDWARE_ID set HARDWARE_ID=%COMPUTERNAME%
echo Using hardware id: %HARDWARE_ID%
powershell -NoProfile -Command "$body = @{licenseKey='%LICENSE_KEY%'; hardwareId='%HARDWARE_ID%'; machineName='$env:COMPUTERNAME'} | ConvertTo-Json; Invoke-RestMethod -Method Post -Uri '%LICENSE_SERVER_URL%' -Body $body -ContentType 'application/json'" || (
    echo License activation failed. Please check your inputs or run manually later.
    goto :eof
)
echo License activated successfully.
goto :eof
