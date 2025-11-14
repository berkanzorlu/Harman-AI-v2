# Developer Portal Manual

## Features
- Admin login with password + TOTP 2FA.
- License CRUD with activation counters and heartbeat timestamps.
- User memory explorer with search box and JSON viewer.
- Actions history timeline and audit log viewer.

## Getting Started
1. Copy `.env.example` to `.env` and set `DATABASE_URL`, `JWT_PRIVATE_KEY`, `JWT_PUBLIC_KEY`, `ADMIN_EMAIL`.
2. Run `npm install` then `npm run dev` for development or `npm run build && npm start` for production.
3. Run `npx prisma migrate dev` to provision PostgreSQL tables.

## Usage
- **Licenses:** create new license tokens, configure max activations, set expiration.
- **Users:** view hardware activations and revoke devices.
- **Memory:** search by keyword, filter by role/action, export results.
- **Audit:** inspect admin actions, login attempts, and license changes.
