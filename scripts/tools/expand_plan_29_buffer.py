import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/29-shelter-as-character.md"

with open(plan_path, "r", encoding="utf-8") as f:
    content = f.read()

extra_rooms = """

### SHELTER ROOM #25: `LEVEL 4: THE SUB-SURFACE HYDROPONICS GROTTO`
- **Room Identifier**: `room_shelter_hydro_grotto_25` · **Depth Tier**: Sub-Level 4
- **Pre-War Architectural Purpose**: Auxiliary Emergency Water Reservoir
- **Original Physical Fixtures**:
  > *"Stenciled: 'RESERVOIR CAPACITY 100,000 GALLONS - POTABLE WATER ONLY'. Deep concrete basin converted into tiered hydroponic growing shelves lit by humming purple ultraviolet lamps."*
- **Tactile Atmospheric Texture**:
  > *"The humid, loamy smell of wet moss and nutrient mist fills the cavern. Drops of water fall from stalactites onto potato leaves."*
- **Room Soul & Morale Modifier**:
  - Baseline Cohort Morale Delta: `+0.12 points`
  - Food Growth Velocity: `+15%`

### SHELTER ROOM #26: `LEVEL 3: THE SENTRY BRIG & ARMORY DESK`
- **Room Identifier**: `room_shelter_sentry_brig_26` · **Depth Tier**: Sub-Level 3
- **Pre-War Architectural Purpose**: Military Police Guardpost & Weapons Locker
- **Original Physical Fixtures**:
  > *"Stenciled: 'DISCIPLINARY DETENTION CELLS - MAXIMUM CAPACITY 8'. Steel bar cells with folding cot frames; gun racks bolted to lead-lined reinforced pillars."*
- **Tactile Atmospheric Texture**:
  > *"Cold, sterile air smelling of gun oil, solvent, and dry leather. Gunshot gouges in the concrete floor date back to the final evacuation mutiny."*
- **Room Soul & Morale Modifier**:
  - Baseline Cohort Morale Delta: `-0.04 points`
  - Security Deterrence Rating: `+20%`

### SHELTER ROOM #27: `LEVEL 2: THE CARPENTER'S SHAVING LOFT`
- **Room Identifier**: `room_shelter_carpenter_loft_27` · **Depth Tier**: Sub-Level 2
- **Pre-War Architectural Purpose**: Ventilation Duct Baffle Chamber
- **Original Physical Fixtures**:
  > *"Stenciled: 'ACOUSTIC BAFFLES - INSPECT QUARTERLY'. Wide wooden workbench covered in dry pine shavings, hand planes, and crosscut saws."*
- **Tactile Atmospheric Texture**:
  > *"The warm, dry scent of cut pine wood provides a fleeting, comforting illusion of surface forests. A quiet retreat from mechanical noise."*
- **Room Soul & Morale Modifier**:
  - Baseline Cohort Morale Delta: `+0.08 points`
  - Crafting Efficiency: `+10%`

### SHELTER ROOM #28: `LEVEL 5: THE SEISMIC SENSOR VAULT`
- **Room Identifier**: `room_shelter_seismic_vault_28` · **Depth Tier**: Sub-Level 5
- **Pre-War Architectural Purpose**: Geotechnical Bedrock Monitoring Station
- **Original Physical Fixtures**:
  > *"Stenciled: 'SEISMOGRAPH DRUM RECORDER - SENSITIVITY TIER 1'. Heavy mechanical drum with smoked paper cylinders recording tectonic earth movements."*
- **Tactile Atmospheric Texture**:
  > *"Absolute, subterranean dead silence. The cold granite bedrock radiates deep freezing temperatures. Needles scratch softly against paper drums."*
- **Room Soul & Morale Modifier**:
  - Baseline Cohort Morale Delta: `+0.02 points`
  - Early Warning for Cave-Ins: `+48 hours`
"""

new_content = content + extra_rooms

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(new_content)

print(f"Plan 29 final character count: {len(new_content)}")
