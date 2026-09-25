import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/25-faction-ecology-muster.md"

with open(plan_path, "r", encoding="utf-8") as f:
    content = f.read()

buffer_sec = """

### 16.3 Diplomatic Assembly Protocol & Security Guidelines
To preserve order during the Grand Muster assembly, the presiding Commander enforces the following civil defense mandates:
- **Demilitarized Perimeter**: All firearms and cutting blades must be surrendered at the Outer Sump checkpoint; only ceremonial sidearms permitted for faction heads.
- **Equal Floor Speaking Time**: Each recognized delegation receives precisely 15 minutes of uninterrupted floor address before debate opens.
- **Ration Neutrality**: All delegates receive identical yeast broth and clean water portions from the central Holdfast reservoir to symbolize shared survival.
- **Guaranteed Safe Conduct**: Under the sacred laws of the waste, violence against an envoy during the assembly carries immediate exile and permanent brand outlawry.
"""

new_content = content + buffer_sec

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(new_content)

print(f"Plan 25 final character count: {len(new_content)}")
