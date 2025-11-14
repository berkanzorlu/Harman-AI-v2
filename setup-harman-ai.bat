@echo off
setlocal enabledelayedexpansion
set SCRIPT_DIR=%~dp0
pushd "%SCRIPT_DIR%"

call :banner
call :require dotnet
call :require python
call :require npm
call :require powershell

call :ensureEnv "backend\python\.env" "backend\python\.env.example"
call :ensureEnv "developer-portal\.env" "developer-portal\.env.example"

call :setupPython
call :setupBrowserController
call :setupDeveloperPortal
call :buildDesktop
call :activateLicense

echo.
echo Harman AI setup finished successfully.
popd
pause
exit /b 0

:banner
echo ==================================================
echo   Harman AI - One Click Setup and Activation

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
    copy %~2 %~1 >nul
    echo Created %~1. Please review and update sensitive values after setup.
)
goto :eof

:setupPython
echo.
echo --- Configuring Python backend ---
set PY_DIR=%SCRIPT_DIR%backend\python
set PY_VENV=%PY_DIR%\.venv
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
pushd backend\browser-controller
npm install || goto :fail
popd
echo Browser controller ready.
goto :eof

:setupDeveloperPortal
echo.
echo --- Installing developer portal and provisioning database ---
pushd developer-portal
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
dotnet restore desktop\HarmanAI.Desktop\HarmanAI.Desktop.csproj || goto :fail
if not exist publish mkdir publish
set PUBLISH_DIR=%SCRIPT_DIR%publish\desktop
if not exist "%PUBLISH_DIR%" mkdir "%PUBLISH_DIR%"
dotnet publish desktop\HarmanAI.Desktop\HarmanAI.Desktop.csproj -c Release -o "%PUBLISH_DIR%" || goto :fail
echo Desktop binaries published to %PUBLISH_DIR%
goto :eof

:activateLicense
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

:fail
echo.
echo Setup aborted due to an earlier error (code %errorlevel%).
popd
pause
exit /b 1
