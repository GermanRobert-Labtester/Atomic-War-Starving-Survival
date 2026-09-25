import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/29-shelter-as-character.md"

with open(plan_path, "r", encoding="utf-8") as f:
    content = f.read()

extra_text = """

### SHELTER ROOM #33: `LEVEL 2: THE APPRENTICE CLASSROOM & SCHOOL`
- **Room Identifier**: `room_shelter_classroom_33` · **Depth Tier**: Sub-Level 2
- **Pre-War Architectural Purpose**: Civil Defense Briefing Auditorium
- **Original Physical Fixtures**:
  > *"Stenciled: 'LECTURE THEATRE B - CAPACITY 30'. Tiered wooden folding desks facing a large slate chalkboard. Slate chalk trays hold white chalk stubs."*
- **Tactile Atmospheric Texture**:
  > *"The dry scent of chalkboard slate and damp paper notebooks. Children's cursive handwriting covers the board: 'Water boils at 100°C; lead melts at 327°C.' "*
- **Room Soul & Morale Modifier**:
  - Baseline Cohort Morale Delta: `+0.14 points`
  - Apprentice Skill Growth Velocity: `+25%`

### SHELTER ROOM #34: `LEVEL 3: THE FOUNDRY SLAG LEACHING TRENCH`
- **Room Identifier**: `room_shelter_slag_trench_34` · **Depth Tier**: Sub-Level 3
- **Pre-War Architectural Purpose**: Metal Heat-Treating Quench Pit
- **Original Physical Fixtures**:
  > *"Stenciled: 'OIL QUENCH PIT - CAUTION FLAMMABLE VAPORS'. Cast-iron drainage trench where hot iron slag is dumped into alkaline slurry to leach out trace minerals."*
- **Tactile Atmospheric Texture**:
  > *"Acrid mineral vapors and sizzling steam plumes whenever hot scrap is dropped into the water. Workers wear leather hoods."*
- **Room Soul & Morale Modifier**:
  - Baseline Cohort Morale Delta: `-0.03 points`
  - Trace Mineral Recovery: `+15%`

### SHELTER ROOM #35: `LEVEL 1: THE MAIN BLAST AIRLOCK VESTIBULE`
- **Room Identifier**: `room_shelter_airlock_vestibule_35` · **Depth Tier**: Sub-Level 1
- **Pre-War Architectural Purpose**: Primary Personnel Chemical Decontamination Airlock
- **Original Physical Fixtures**:
  > *"Stenciled: 'DECONTAMINATION PROTOCOL TIER 4 - SEAL BOTH DOORS BEFORE CYCLE'. Massive 10-ton cast-steel blast door with dual pneumatic dogging pins and copper weather seals."*
- **Tactile Atmospheric Texture**:
  > *"Cold, rushing drafts whistling through the outer door gaskets. The floor is perforated steel grating over a lead effluent trough."*
- **Room Soul & Morale Modifier**:
  - Baseline Cohort Morale Delta: `+0.05 points`
  - Perimeter Defense Rating: `+30%`
"""

new_content = content + extra_text

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(new_content)

print(f"Plan 29 final character count: {len(new_content)}")
