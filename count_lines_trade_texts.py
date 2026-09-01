import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/trade_texts.json"

with open(file_path, "r") as f:
    data = json.load(f)

print(json.dumps(data, indent=2)[:1000])
