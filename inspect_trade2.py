import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/trade_texts.json"
with open(file_path, "r") as f:
    data = json.load(f)

for k in ["trade_jokes", "trade_myths", "trade_ceremonies", "trade_riddles"]:
    if k in data:
        val = data[k]
        print(f"\n--- {k} (Type: {type(val).__name__}) ---")
        if isinstance(val, dict):
            for k2, v2 in list(val.items())[:3]:
                print(f"{k2}: {v2}")
        elif isinstance(val, list):
            for i in val[:3]:
                print(i)
        else:
            print(val)
