import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/25-faction-ecology-muster.md"

with open(plan_path, "r", encoding="utf-8") as f:
    content = f.read()

extra_witnesses = """

### MUSTER WITNESS #29: `FARMER GRETEL`
- **Witness Identifier**: `wit_muster_029` · **Survivor Role**: Hydroponic Agronomist
- **Gating Campaign Flag**: `flag_quarantined_ergot_blight`
- **Branch A (Merciful / Honorable Record)**:
  > *"When the black mold struck the grain beds, he did not destroy our seed stock; he helped us build isolation glass boxes. We have bread today because he was patient."*
- **Branch B (Ruthless / Pragmatic Record)**:
  > *"He burned three months of potato crops with a flamethrower because of a few gray leaves. We ate boiled leather boots for nine weeks."*

### MUSTER WITNESS #30: `DROVER BORIS`
- **Witness Identifier**: `wit_muster_030` · **Survivor Role**: Caravan Master
- **Gating Campaign Flag**: `flag_escorted_salt_convoy`
- **Branch A (Merciful / Honorable Record)**:
  > *"He sent armed scouts to guide our pack brahms through the sniper passes. Not a single sack of salt was lost."*
- **Branch B (Ruthless / Pragmatic Record)**:
  > *"He taxed our caravan fifty percent of our cargo just to walk past his bunker exhaust louvers. Robbery under color of authority."*
"""

new_content = content + extra_witnesses

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(new_content)

print(f"Plan 25 final character count: {len(new_content)}")
