# Harman AI System Architecture

## Overview
Harman AI is a modular desktop automation platform composed of a Windows desktop client, a Python AI backend, a Node.js-based browser controller, and a Next.js developer portal backed by PostgreSQL. All modules communicate through secure HTTPS/gRPC channels and share authentication via RS256-signed JWT licenses.

## Components
- **Desktop Core (C# .NET 8):** Handles UI automation, speech, screen capture, and orchestrates remote services.
- **Python AI Backend:** Provides Gemini-powered natural language understanding, OCR, memory management, and self-repair suggestions.
- **Node Browser Controller:** Uses Playwright to automate Chromium-based browsers following JSON commands.
- **Developer Portal:** Next.js app for admins to manage licenses, memories, and audit logs.
- **PostgreSQL Database:** Central data store accessed via Prisma (portal) and SQLAlchemy (Python backend).

## Data Flow
1. User speaks to Harman AI. The desktop VoiceEngine transcribes the command and sends it to the Python backend via the PythonBridge.
2. The backend interprets intent, queries memory, and may request desktop automation or browser control.
3. Browser tasks are forwarded to the Node controller via NodeBrowserBridge, returning structured JSON results.
4. MemoryManager stores conversation logs, action history, and retrieved screen context in PostgreSQL.
5. Licenses are validated through RS256 JWT tokens stored securely with DPAPI on the desktop.

## Security
- Mutual TLS for service-to-service calls.
- DPAPI encryption for local license cache.
- Anti-tamper hashing for binaries.
- Admin portal protected with 2FA and RBAC.
- Audit logs persisted for every privileged action.
