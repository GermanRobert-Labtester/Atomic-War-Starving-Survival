import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/trade_texts.json"
with open(file_path, "r") as f:
    data = json.load(f)

print(f"Total keys: {len(data.keys())}")
sample_keys = ["trade_jokes", "trade_myths", "trade_ceremonies", "trade_riddles"]
for k in sample_keys:
    if k in data:
        print(f"\n--- {k} ---")
        for item in data[k][:3]:
            print(item)
