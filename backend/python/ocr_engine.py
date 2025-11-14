import base64


class OcrEngine:
    def extract_text(self, image_base64: str) -> str:
        # Placeholder for actual OCR call
        size = len(base64.b64decode(image_base64))
        return f"OCR text length {size}"
