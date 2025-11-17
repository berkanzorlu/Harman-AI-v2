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
call :require npm
call :require powershell
call :require python

set PYTHON_DIR=%SCRIPT_DIR%\backend\python
set BROWSER_DIR=%SCRIPT_DIR%\backend\browser-controller
set PORTAL_DIR=%SCRIPT_DIR%\developer-portal
set DESKTOP_EXE=%SCRIPT_DIR%\publish\desktop\HarmanAI.Desktop.exe

call :verifyPath "%PYTHON_DIR%\app.py" "Python backend"
call :verifyPath "%PYTHON_DIR%\.venv\Scripts\activate.bat" "Python virtual environment (run setup first)"
call :verifyPath "%BROWSER_DIR%\package.json" "Browser controller"
call :verifyPath "%PORTAL_DIR%\package.json" "Developer portal"
call :verifyPath "%DESKTOP_EXE%" "Published desktop executable"

call :launchPython "%PYTHON_DIR%"
call :launchBrowser "%BROWSER_DIR%"
call :launchPortal "%PORTAL_DIR%"
call :launchDesktop "%DESKTOP_EXE%"

echo.
echo All Harman AI services have been started in their own windows.
set EXIT_CODE=0
goto :cleanup

:fail
echo.
echo Failed to start Harman AI services (code %errorlevel%).
if not "%errorlevel%"=="" set EXIT_CODE=%errorlevel%
goto :cleanup

:cleanup
popd
exit /b %EXIT_CODE%

:banner
echo ==================================================
echo   Harman AI - Multi-service Launcher
echo.
echo   Root Directory: %SCRIPT_DIR%
echo ==================================================
echo.
goto :eof

:require
where %1 >nul 2>&1
if errorlevel 1 (
    echo Missing required command: %1
    goto :fail
) else (
    echo Verified dependency: %1
)
goto :eof

:verifyPath
if exist %~1 (
    echo OK: %~2 located at %~1
) else (
    echo Missing %~2: %~1
    goto :fail
)
goto :eof

:launchPython
echo.
echo Starting Python AI backend...
set LAUNCH_DIR=%~1
start "Harman AI Backend" cmd /k "cd /d ""%LAUNCH_DIR%"" ^&^& call .venv\Scripts\activate.bat ^&^& uvicorn app:app --host 0.0.0.0 --port 8000"
if errorlevel 1 goto :fail
echo Python backend window launched.
goto :eof

:launchBrowser
echo.
echo Starting Playwright browser controller...
set LAUNCH_DIR=%~1
start "Harman AI Browser" cmd /k "cd /d ""%LAUNCH_DIR%"" ^&^& npm run start"
if errorlevel 1 goto :fail
echo Browser controller window launched.
goto :eof

:launchPortal
echo.
echo Starting developer portal (Next.js)...
set LAUNCH_DIR=%~1
start "Harman AI Portal" cmd /k "cd /d ""%LAUNCH_DIR%"" ^&^& npm run start"
if errorlevel 1 goto :fail
echo Developer portal window launched.
goto :eof

:launchDesktop
echo.
echo Starting Harman AI desktop assistant...
set DESKTOP_EXE=%~1
start "Harman AI Desktop" ""%DESKTOP_EXE%""
if errorlevel 1 goto :fail
echo Desktop assistant launched.
goto :eof
