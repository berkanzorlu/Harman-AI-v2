import httpx
from typing import Any


class GeminiClient:
    def __init__(self, api_key: str):
        self.api_key = api_key
        self.base_url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent"

    async def complete(self, prompt: str, images: list[str] | None = None) -> str:
        payload: dict[str, Any] = {
            "contents": [{"parts": [{"text": prompt}]}]
        }
        async with httpx.AsyncClient(timeout=30.0) as client:
            resp = await client.post(
                self.base_url,
                params={"key": self.api_key},
                json=payload,
            )
            resp.raise_for_status()
            data = resp.json()
            return data.get("candidates", [{}])[0].get("content", {}).get("parts", [{}])[0].get("text", "")
