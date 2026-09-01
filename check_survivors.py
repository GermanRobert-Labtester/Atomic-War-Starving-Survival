import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/survivors.json"
with open(file_path, "r") as f:
    data = json.load(f)

for i in range(5):
    bio = data["survivors"][i].get("bio", "")
    print(bio)
    print("-" * 20)
