import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/29-shelter-as-character.md"

with open(plan_path, "r", encoding="utf-8") as f:
    content = f.read()

extra_text = """

### SHELTER ROOM #29: `LEVEL 1: THE RADAR PLOTTING BALCONY`
- **Room Identifier**: `room_shelter_radar_balcony_29` · **Depth Tier**: Sub-Level 1
- **Pre-War Architectural Purpose**: Early Warning Anti-Air Plotting Gallery
- **Original Physical Fixtures**:
  > *"Stenciled: 'PLOTTING SECTOR GRID 4 - CONFIDENTIAL'. Curved glass plotting tables with edge-lit LED grids. Wall racks hold colored grease pencils and brass dividers."*
- **Tactile Atmospheric Texture**:
  > *"Cool drafts leak through ceiling cable penetrations. Low green CRT phosphor reflections shimmer across the dark glass."*
- **Room Soul & Morale Modifier**:
  - Baseline Cohort Morale Delta: `+0.05 points`
  - Long-Range Scavenging Planning Bonus: `+12%`

### SHELTER ROOM #30: `LEVEL 4: THE COLD FERMENTATION CELLAR`
- **Room Identifier**: `room_shelter_ferment_cellar_30` · **Depth Tier**: Sub-Level 4
- **Pre-War Architectural Purpose**: Emergency Rations Cold Vault
- **Original Physical Fixtures**:
  > *"Stenciled: 'COLD STORAGE UNIT C - KEEP SEALED'. Heavy insulated cork door with dual brass latch handles; lined with wooden barrels of fermenting yeast beer and pickled beets."*
- **Tactile Atmospheric Texture**:
  > *"The yeasty, sharp smell of bubbling sourdough and vinegar cuts through the stagnant underground air. A place of warmth and full stomachs."*
- **Room Soul & Morale Modifier**:
  - Baseline Cohort Morale Delta: `+0.15 points`
  - Calorie Preservation Efficiency: `+20%`

### SHELTER ROOM #31: `LEVEL 3: THE COBBLER'S BENCH & TAILORY`
- **Room Identifier**: `room_shelter_tailor_bench_31` · **Depth Tier**: Sub-Level 3
- **Pre-War Architectural Purpose**: Uniform Supply Depot
- **Original Physical Fixtures**:
  > *"Stenciled: 'QUARTERMASTER TAILORING - REPAIR STATION'. Heavy treadle sewing machine with cast iron base; shelves stacked with rolls of gray wool, canvas, and tanned rabbit hides."*
- **Tactile Atmospheric Texture**:
  > *"Smells of neat's-foot oil, tanned leather, and dry wool dust. The rhythmic clicking of the sewing treadle brings domestic calm."*
- **Room Soul & Morale Modifier**:
  - Baseline Cohort Morale Delta: `+0.06 points`
  - Clothing Durability Bonus: `+15%`

### SHELTER ROOM #32: `LEVEL 5: THE RADIOLYTIC RETENTION TANK`
- **Room Identifier**: `room_shelter_rad_retention_32` · **Depth Tier**: Sub-Level 5
- **Pre-War Architectural Purpose**: Contaminated Wastewater Hold Tank
- **Original Physical Fixtures**:
  > *"Stenciled: 'RADIATION HAZARD - ISOLATION TANK NO. 2'. Thick lead-shielded viewing port looking into glowing murky greywater sump."*
- **Tactile Atmospheric Texture**:
  > *"Heavy lead shielding blocks all radiation, but the air carries a metallic, copper-foil taste. Geiger counters click softly outside the door."*
- **Room Soul & Morale Modifier**:
  - Baseline Cohort Morale Delta: `-0.08 points`
  - Wastewater Filtering Capacity: `+500 L/day`
"""

new_content = content + extra_text

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(new_content)

print(f"Plan 29 final character count: {len(new_content)}")
