import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/quests_expansion_05.json"

with open(file_path, "r") as f:
    data = json.load(f)

done = ["quest_survivor_disappearance", "quest_bunker_power_crisis", "quest_survivor_mental_health",
"quest_exp09_sunken_submarine", "quest_neighboring_settlement_aid", "quest_black_flotilla_trade",
"quest_bunker_upgrade", "quest_radiation_study", "quest_survivor_rescue", "quest_raider_peace_offer",
"quest_expedition_equipment", "quest_medical_experiment", "quest_scout_training",
"quest_food_storage_theft", "quest_medical_triage"]

for q in data.get("quests", []):
    qid = q.get("id")
    if qid not in done:
        print(f"ID: {qid}")
        print(f"TITLE: {q.get('title')}")
        print(f"DESC: {q.get('description')}")
        print(f"SYNOP: {q.get('synopsis')}")
        for i, c in enumerate(q.get("choices", [])):
            print(f"  CHOICE {i}: {c.get('text')}")
            print(f"  CONSEQ {i}: {c.get('consequences')}")
        print("-" * 40)
