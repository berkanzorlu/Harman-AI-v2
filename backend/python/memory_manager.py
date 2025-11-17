from dataclasses import dataclass, asdict
from typing import List, Any
import asyncio


@dataclass
class MemoryChunk:
    userId: str
    role: str
    actionType: str
    content: str
    metadata: dict[str, Any] | None = None
    timestamp: str | None = None


class MemoryManager:
    def __init__(self, db_url: str | None):
        self.db_url = db_url
        self._cache: list[dict] = []
        self._lock = asyncio.Lock()

    async def write_chunk(self, chunk: MemoryChunk) -> None:
        async with self._lock:
            self._cache.append(asdict(chunk))

    async def list_chunks(self, user_id: str) -> List[dict]:
        async with self._lock:
            return [chunk for chunk in self._cache if chunk["userId"] == user_id]

    async def search_chunks(self, user_id: str, query: str) -> List[dict]:
        async with self._lock:
            return [chunk for chunk in self._cache if chunk["userId"] == user_id and query.lower() in chunk["content"].lower()]
