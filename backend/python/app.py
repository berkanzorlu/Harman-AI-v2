from fastapi import FastAPI, HTTPException, Depends
from pydantic import BaseModel
from dotenv import load_dotenv
import os
from typing import List

from gemini_client import GeminiClient
from command_interpreter import CommandInterpreter
from memory_manager import MemoryManager, MemoryChunk
from self_repair_engine import SelfRepairEngine
from ocr_engine import OcrEngine
from vision_detector import VisionDetector

load_dotenv()

app = FastAPI(title="Harman AI Backend")

gemini = GeminiClient(os.getenv("GEMINI_API_KEY", "demo"))
interpreter = CommandInterpreter(gemini)
memory = MemoryManager(os.getenv("DATABASE_URL"))
self_repair = SelfRepairEngine(gemini)
ocr = OcrEngine()
vision = VisionDetector()


class InterpretRequest(BaseModel):
    text: str
    context: dict | None = None


class InterpretResponse(BaseModel):
    intent: str
    response: str
    commands: List[dict]


@app.post("/v1/interpret", response_model=InterpretResponse)
async def interpret(req: InterpretRequest):
    result = await interpreter.interpret(req.text, req.context or {})
    return InterpretResponse(**result)


class MemoryWriteRequest(BaseModel):
    chunk: MemoryChunk


@app.post("/v1/memory/write")
async def memory_write(req: MemoryWriteRequest):
    await memory.write_chunk(req.chunk)
    return {"status": "ok"}


@app.get("/v1/memory/{user_id}")
async def memory_list(user_id: str):
    return await memory.list_chunks(user_id)


class MemorySearchRequest(BaseModel):
    query: str
    user_id: str


@app.post("/v1/memory/search")
async def memory_search(req: MemorySearchRequest):
    return await memory.search_chunks(req.user_id, req.query)


class VisionRequest(BaseModel):
    image_base64: str


@app.post("/v1/vision/analyze")
async def vision_analyze(req: VisionRequest):
    text = ocr.extract_text(req.image_base64)
    summary = vision.describe(req.image_base64)
    return {"text": text, "summary": summary}


class SelfRepairRequest(BaseModel):
    stacktrace: str
    module: str


@app.post("/v1/self-repair")
async def self_repair_route(req: SelfRepairRequest):
    patch = await self_repair.generate_patch(req.module, req.stacktrace)
    return {"patch": patch}
