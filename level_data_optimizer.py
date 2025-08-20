import os
import json

# Set the folder containing your JSON files
folder_path = "C:/Eray/Repos/remote-level-download-sim/Assets/Resources/"

print(f"starting checking files in {folder_path}")

difficulty_map = {
    "easy": 0,
    "medium": 1,
    "hard": 2
}

def board_to_hex(board):
    hex_rows = []
    for row in board:
        # Convert "o"/"x" to bits: "x"=1, "o"=0
        bits = "".join(["1" if cell == "x" else "0" for cell in row])
        # Convert binary string to hexadecimal
        hex_str = f"{int(bits, 2):02X}"
        hex_rows.append(hex_str)
        
    return "".join(hex_rows)

# Loop through all files in the folder
for filename in os.listdir(folder_path):
    file_path = os.path.join(folder_path, filename)
    try:
        with open(file_path, "r") as f:
            data = json.load(f)
            
        converted = {
            "difficulty": difficulty_map.get(data["difficulty"], 0),
            "board": board_to_hex(data["board"])
        }
        
        with open(file_path, "w") as f:
            json.dump(converted, f, indent=2)
        
    except Exception as e:
        print(f"{filename}: Failed to read/parse JSON ({e})")