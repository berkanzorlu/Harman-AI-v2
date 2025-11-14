# API Specifications

## Python Backend (FastAPI)
- `POST /v1/interpret`: Accepts `{ text, context, licenseToken }` and returns parsed intent plus required actions.
- `POST /v1/memory/write`: Persists memory chunk; body matches schema in Memory-Model.
- `GET /v1/memory/{userId}`: Lists recent chunks.
- `POST /v1/memory/search`: Full-text/vector search.
- `POST /v1/vision/analyze`: Uploads base64 screenshot for OCR/vision summary.
- `POST /v1/self-repair`: Uploads failure log and receives code patch proposal.
- `POST /v1/license/heartbeat`: Validates license token and hardware hash.

## License Server Endpoints
Implemented inside Next.js API routes:
- `POST /api/license/activate`
- `POST /api/license/verify`
- `POST /api/license/revoke`
- `POST /api/license/generate`
- `GET /api/license/{id}`
- `GET /api/admin/licenses`
- `GET /api/admin/users`
- `GET /api/admin/memory/{userId}`
All requests use bearer tokens signed with RS256 private key.

## Node Browser Controller
- `POST /open` `{ url }`
- `POST /click` `{ selector }`
- `POST /fill` `{ selector, text }`
- `POST /getText` `{ selector }`
- `GET /getLinks`
- `POST /scroll` `{ x, y }`
- `GET /screenshot`
Responses follow `{ success: bool, data, error }`.

## Desktop IPC
Desktop communicates via gRPC:
- `PythonBridge.ExecuteCommand(CommandRequest)`
- `NodeBrowserBridge.SendAction(BrowserAction)`
