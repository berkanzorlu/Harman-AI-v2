from gemini_client import GeminiClient


class SelfRepairEngine:
    def __init__(self, gemini: GeminiClient):
        self.gemini = gemini

    async def generate_patch(self, module: str, stacktrace: str) -> str:
        prompt = f"Module {module} failed with stacktrace:\n{stacktrace}\nProvide a minimal patch in unified diff format."
        return await self.gemini.complete(prompt)
