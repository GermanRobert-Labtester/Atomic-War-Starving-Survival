import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json"

with open(file_path, "r") as f:
    data = json.load(f)

# Reauthor to ASHFALL tone (cold, material, bureaucratic)
new_epitaphs = {
    "radiation": "Lethal cellular degradation. Biological remains require deep burial.",
    "combat": "Terminated by hostiles. Equipment recovered and sanitized.",
    "starvation": "Caloric deficit reached terminal state.",
    "exhaustion": "Cardiovascular collapse due to sustained labor output.",
    "disease": "Pathological contamination event. Sector quarantined.",
    "expedition": "Asset failed to return from surface operations. Logged as loss.",
    "trauma": "Severe structural damage to biological unit.",
    "unspecified": "Termination logged. Rations redistributed."
}

for item in data.get("epitaphs", []):
    c = item.get("cause")
    if c in new_epitaphs:
        item["epitaph"] = new_epitaphs[c]

with open(file_path, "w") as f:
    json.dump(data, f, indent=4)
