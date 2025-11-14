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
