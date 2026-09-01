import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/thirdonary_quests.json"
with open(file_path, "r") as f:
    data = json.load(f)

for q in data.get("quests", []):
    disc = q.get('discovery', '')
    if 'tear' in disc.lower() or 'cry' in disc.lower() or 'sad' in disc.lower() or 'hope' in disc.lower() or 'love' in disc.lower():
        print(f"ID: {q.get('id')}")
        print(f"DISCOVERY: {disc}")
