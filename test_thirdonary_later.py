import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/thirdonary_quests.json"
with open(file_path, "r") as f:
    data = json.load(f)

for q in data.get("quests", [])[65:70]:
    print(f"ID: {q.get('id')}")
    print(f"DISCOVERY: {q.get('discovery')}")
    for i, c in enumerate(q.get("choices", [])):
        print(f"  OUTCOME {i}: {c.get('outcome_text')}")
    print("-" * 40)
