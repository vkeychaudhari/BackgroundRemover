from rembg import remove
from rembg.session_factory import new_session
from PIL import Image
import io
import sys

def remove_background(input_path: str, output_path: str):
    model_name = "u2net"  # Using working model
    session = new_session(model_name=model_name)
    
    with open(input_path, "rb") as input_file:
        input_data = input_file.read()

    output_data = remove(
        input_data,
        session=session,
        alpha_matting=True,
        alpha_matting_foreground_threshold=240,
        alpha_matting_background_threshold=10
    )

    image = Image.open(io.BytesIO(output_data)).convert("RGBA")
    image.save(output_path)
    print(f"✅ Background removed! Saved to: {output_path}")

if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: python bgRemover.py <input_path> <output_path>")
        sys.exit(1)

    input_path = sys.argv[1]
    output_path = sys.argv[2]

    try:
        remove_background(input_path, output_path)
    except Exception as e:
        print(f"❌ Error: {e}")
        sys.exit(1)