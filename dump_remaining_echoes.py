import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/echoes.json"

with open(file_path, "r") as f:
    data = json.load(f)

for e in data.get("echoes", [])[16:]:
    print(f"ID: {e.get('id')}")
    print(f"TITLE: {e.get('title')}")
    print(f"BODY: {e.get('bodyText')}")
    for i, c in enumerate(e.get("choices", [])):
        print(f"  CHOICE {i}: {c.get('text')}")
    print("-" * 40)
