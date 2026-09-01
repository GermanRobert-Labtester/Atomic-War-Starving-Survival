import json
import re

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/survivors.json"
with open(file_path, "r") as f:
    data = json.load(f)

for i in range(10):
    bio = data["survivors"][i].get("bio", "")
    sentences = re.split(r'(?<=[.!?]) +', bio)
    new_bio = sentences[0]
    if len(new_bio) < 40 and len(sentences) > 1:
        new_bio += " " + sentences[1]
    print(f"[{data['survivors'][i].get('profession')}] -> {new_bio}")
