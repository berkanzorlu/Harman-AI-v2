from typing import Dict, Any

from gemini_client import GeminiClient


class CommandInterpreter:
    def __init__(self, gemini: GeminiClient):
        self.gemini = gemini

    async def interpret(self, text: str, context: Dict[str, Any]) -> Dict[str, Any]:
        prompt = f"You are Harman AI desktop assistant. Convert the instruction '{text}' into JSON with intent, response, and commands list."
        raw = await self.gemini.complete(prompt)
        # fall back simple parser
        commands = []
        if "open" in text.lower():
            commands.append({"type": "browser.open", "parameters": {"url": context.get("url", "https://www.bing.com")}})
        return {
            "intent": "general",
            "response": raw or "Command processed",
            "commands": commands,
        }
