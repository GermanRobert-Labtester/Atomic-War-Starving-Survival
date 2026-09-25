import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/23-maritime-black-flotilla.md"

with open(plan_path, "r", encoding="utf-8") as f:
    content = f.read()

buffer_sec = """

### 15.3 Hyperbaric Safety Compliance & Medical Protocol
All diving sorties conducted under this architecture must maintain strict compliance with the following field medical protocols:
- **Ascent Rate Limit**: Never exceed 9.0 meters per minute ascent velocity to prevent arterial gas embolism.
- **Mandatory Safety Stops**: Any dive exceeding 20 meters depth requires a 3-minute decompression stop at 5 meters depth.
- **Hyperbaric Recompression**: In the event of emergency rapid ascent, the patient must be transferred immediately to the shelter's recompression chamber in Medical Bay (Plan 09).
"""

new_content = content + buffer_sec

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(new_content)

print(f"Plan 23 final character count: {len(new_content)}")
