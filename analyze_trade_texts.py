import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/trade_texts.json"

with open(file_path, "r") as f:
    data = json.load(f)

for k, v in data.items():
    print(f"Key {k} has type {type(v)} and size roughly {len(str(v))}")
