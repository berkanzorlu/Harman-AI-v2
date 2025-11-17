# Memory Model

## Tables
- `memory_chunks(id, user_id, embedding, content, metadata_json, created_at)`
- `conversation_logs(id, user_id, role, text, context, created_at)`
- `actions_history(id, user_id, action_type, payload, result, created_at)`

## Chunk Format
```json
{
  "userId": "uuid",
  "role": "user|assistant|system",
  "actionType": "speech|automation|browser|system",
  "content": "Full text chunk",
  "metadata": {
    "source": "voice",
    "confidence": 0.94,
    "screenSummary": "..."
  },
  "timestamp": "ISO-8601"
}
```

## APIs
- `/memory/write`: insert chunk and optional embeddings.
- `/memory/read`: fetch chronological chunks with pagination.
- `/memory/search`: hybrid similarity search combining trigram text search and cosine distance.
- `/memory/delete`: soft delete flag stored in metadata.
- `/memory/list`: admin endpoint for listing per-user stats.

## Developer Portal Views
- Memory timeline (grouped by day)
- Conversation viewer
- JSON explorer with syntax highlight
- Export to JSON/CSV
