import json
import sys

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/quests_expansion_05.json"

try:
    with open(file_path, "r") as f:
        data = json.load(f)
except Exception as e:
    print("Error parsing JSON:", e)
    sys.exit(1)

for q in data.get("quests", []):
    print(f"ID: {q.get('id')}")
    print(f"TITLE: {q.get('title')}")
    print(f"DESC: {q.get('description')}")
    print(f"SYNOP: {q.get('synopsis')}")
    for i, c in enumerate(q.get("choices", [])):
        print(f"  CHOICE {i}: {c.get('text')}")
        print(f"  CONSEQ {i}: {c.get('consequences')}")
    print("-" * 40)
