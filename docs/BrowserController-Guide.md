# Browser Controller Guide

## Setup
```bash
cd backend/browser-controller
npm install
npm start
```

## API Usage
Send JSON requests to `http://localhost:4000` endpoints. Example:
```bash
curl -X POST http://localhost:4000/open -H 'Content-Type: application/json' -d '{"url":"https://example.com"}'
```

## Logging
Every action is logged to `logs/browser.log` with timestamp, command, selector, and success state.

## Security
- Accepts `x-license-token` header for authorization.
- Rate-limits to 30 req/minute per token.
- Sanitizes selectors and text input to prevent script injection.
