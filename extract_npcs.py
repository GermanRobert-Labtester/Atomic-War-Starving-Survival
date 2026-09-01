import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/wasteland_settlement_npcs.json"
with open(file_path, "r") as f:
    data = json.load(f)

for npc in data.get("npcs", []):
    print(f"ID: {npc['id']}")
    print(f"CONTRADICTION: {npc.get('contradiction')}")
    print(f"PERSONAL THREAD: {npc.get('personal_thread')}")
    print("-" * 20)
