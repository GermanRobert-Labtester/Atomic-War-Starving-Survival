import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/trade_texts.json"

with open(file_path, "r") as f:
    data = json.load(f)

print(f"Total traders: {len(data.get('traders', []))}")
for t in data.get('traders', [])[:3]:
    print(f"ID: {t.get('id')} - {t.get('display_name')}")
    print(f"Profile: {t.get('profile')}")
