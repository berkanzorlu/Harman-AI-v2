import asyncio
from command_interpreter import CommandInterpreter
from gemini_client import GeminiClient


class DummyGemini(GeminiClient):
    def __init__(self):
        pass

    async def complete(self, prompt: str, images=None):  # type: ignore
        return "ok"


def test_interpret_event_loop():
    interpreter = CommandInterpreter(DummyGemini())
    result = asyncio.get_event_loop().run_until_complete(interpreter.interpret("open docs", {}))
    assert result["commands"][0]["type"] == "browser.open"
