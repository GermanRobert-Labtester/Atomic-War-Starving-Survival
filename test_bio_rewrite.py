import json
import re

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/survivors.json"
with open(file_path, "r") as f:
    data = json.load(f)

for s in data["survivors"][:10]:
    original_bio = s.get("bio", "")
    sentences = re.split(r'(?<=[.!?]) +', original_bio)
    first_sentence = sentences[0] if sentences else ""

    profession = s.get("profession", "Laborer")
    traits = ", ".join(s.get("traitIds", []))

    # Strip any emotional adjectives from first sentence (rudimentary)
    new_bio = first_sentence

    print(f"ORIGINAL: {original_bio}")
    print(f"REWRITE:  {new_bio} Classified as {profession.lower()}. Assigned to general pool.")
    print("-" * 50)
