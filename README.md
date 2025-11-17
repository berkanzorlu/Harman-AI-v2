# Harman AI v2

A multi-module automation assistant consisting of:
- Windows desktop client (C# .NET 8)
- Python AI backend with Gemini + memory
- Node.js Playwright browser controller
- Next.js developer portal with PostgreSQL + Prisma

## Structure
- `desktop/` – .NET desktop client
- `backend/python` – FastAPI backend
- `backend/browser-controller` – Playwright automation service
- `developer-portal` – Next.js portal
- `docs/` – architecture, API, and integration guides
- `prisma/` – database schema shared with portal

## Setup
See the documentation files under `docs/` for module-specific instructions.

### One-click Windows setup
Run `setup-harman-ai.bat` from the project root to install every dependency, build each module, and trigger license activation in a single pass.

The script performs the following actions:

1. Verifies that `dotnet`, `python`, `npm`, and `powershell` are available on `PATH`.
2. Ensures `.env` files exist for the Python backend and developer portal (copies from the provided examples if they are missing).
3. Creates/updates the Python virtual environment and installs the backend requirements.
4. Installs Node.js dependencies for the Playwright browser controller.
5. Installs Node.js dependencies for the developer portal, runs Prisma generate/migrate, and builds the Next.js bundle.
6. Restores and publishes the Windows desktop host to `publish/desktop`.
7. Prompts for the license activation endpoint and license key, then sends the activation request (hardware id is collected automatically via PowerShell).

> **Important:** Update the values in `backend/python/.env` and `developer-portal/.env` with your actual database connection strings, API keys, and RSA key pair before running the script so the Prisma migrations and FastAPI service can authenticate correctly.

After the script completes you can start the services from their respective folders (`backend/python`, `backend/browser-controller`, `developer-portal`, and `publish/desktop`).

### Launch everything with one command

Run `run-harman-ai.bat` from the repository root to open all Harman AI components in dedicated terminal windows in the correct order:

1. Python AI backend (virtual environment is activated automatically and `uvicorn` is started).
2. Playwright browser controller (`npm run start`).
3. Next.js developer portal (`npm run start`, which serves the previously built app).
4. The published Windows desktop assistant executable from `publish/desktop`.

The launcher performs the same dependency checks as the installer and validates that the build outputs exist before spawning each process. If any prerequisite is missing the script aborts with a clear error so you can re-run `setup-harman-ai.bat` or install the required tool.
