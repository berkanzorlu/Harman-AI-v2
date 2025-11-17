# Python AI Backend Guide

## Installation
```bash
cd backend/python
python -m venv .venv
source .venv/bin/activate
pip install -r requirements.txt
uvicorn app:app --reload
```

## Modules
- `gemini_client.py`: wraps Google Gemini API for text+vision prompts.
- `ocr_engine.py`: uses Tesseract or Google Vision for OCR on screenshots.
- `vision_detector.py`: scene understanding and object detection.
- `command_interpreter.py`: intent parsing + command classification.
- `memory_manager.py`: interfaces with PostgreSQL for chunk storage/search.
- `self_repair_engine.py`: consumes stack traces and proposes patches.
- `app.py`: FastAPI server exposing HTTP endpoints.

## Environment
Create `.env` with `DATABASE_URL`, `GEMINI_API_KEY`, `PORTAL_PUBLIC_KEY`, `NODE_CONTROLLER_URL`.
