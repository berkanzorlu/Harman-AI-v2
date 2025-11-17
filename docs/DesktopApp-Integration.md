# Desktop App Integration

## Modules
- `VoiceEngine`: wraps Windows Speech SDK for STT/TTS.
- `UIAutomationController`: uses UIAutomationClient to interact with Windows controls.
- `ScreenAnalyzer`: captures screen via DXGI and forwards to Python backend for OCR.
- `PythonBridge`: gRPC client to Python backend.
- `NodeBrowserBridge`: gRPC/HTTP client to Node Playwright controller.
- `MemoryClient`: syncs conversation/action logs with backend.
- `LicenseManager`: validates JWT tokens with portal API.

## Event Loop
1. Microphone event triggers transcription.
2. Command interpreted by backend.
3. Desktop executes automation tasks or prompts user.
4. Results logged locally and pushed to memory service.

## Configuration
`appsettings.json` includes:
```json
{
  "PythonBackendUrl": "https://localhost:8000",
  "BrowserControllerUrl": "http://localhost:4000",
  "PortalApiUrl": "https://portal.local/api",
  "Logging": { "Path": "%APPDATA%/HarmanAI/logs" }
}
```
