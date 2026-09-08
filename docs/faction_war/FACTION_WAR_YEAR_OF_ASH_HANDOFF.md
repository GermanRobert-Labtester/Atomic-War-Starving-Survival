# Faction War Year of Ash Crisis Outcomes Handoff

> **Integration Anchor:** Plan 114 (Year of Ash Crisis Outcomes)
> **Source Catalog:** `Assets/StreamingAssets/Data/year_of_ash_locations.json` / `year_of_ash_crises.json`
> **Target Overrides:** Indices 9, 12, 16 in `faction_war_location_overrides.json`

---

## 1. Architectural Alignment

The Year of Ash represents the systemic crisis of prolonged survival in Year 2 (Days 365+), with early build-up occurring in Days 200–365. Plan 114 defines regional crises, military mobilizations, and logistics breakdowns.
Plan 124 bridges these crisis arcs by modifying the visible operational state of 3 critical military and logistics checkpoints during the escalation phase.

---

## 2. Integrated Locations & Temporal Overrides

### 2.1 `loc_garrison_checkpoint_gamma` → `loc_override_checkpoint_occupied`
- **Active Window:** Days 200–250 (51 days)
- **Override Type:** `occupied`
- **Display Name Override:** `Garrison Checkpoint Gamma (Reinforced Redoubt)`
- **Atmosphere Override:** `A smell of burning green wood, harsh coal soot, and oiled rifle mechanisms.`
- **Narrative Continuity:**
  Reflects the Garrison's aggressive mobilization in response to frontier rebel raids. The open toll-bar is replaced with a double log palisade and sandbag redoubts; sentries inspect all passing travelers for contraband, forged papers, and concealed weapons.

### 2.2 `loc_sector_4_rail_switchyard` → `loc_override_rail_yard_fortified`
- **Active Window:** Days 260–320 (61 days)
- **Override Type:** `fortified`
- **Display Name Override:** `Sector 4 Rail Switchyard (Armored Siding)`
- **Atmosphere Override:** `Heavy grease, cold iron rail ties, and the distant rhythm of metal fabrication.`
- **Narrative Continuity:**
  The switchyard is militarized as a fortified marshalling terminal. Flatcars are retrofitted with steel plates and timber bastions, turning the siding into an impregnable logistics defensive pivot point.

### 2.3 `loc_ash_militia_deadfall_barrier` → `loc_override_roadblock_liberated`
- **Active Window:** Days 330–370 (41 days)
- **Override Type:** `liberated`
- **Display Name Override:** `Ash Militia Deadfall Barrier (Opened Passage)`
- **Atmosphere Override:** `Freshly chopped pine boughs, muddy boot ruts, and cold wind through cleared tree trunks.`
- **Narrative Continuity:**
  Following the withdrawal or defeat of the Ash Militia irregulars, local travelers and trade caravans fell the spiked deadfall logs and roll timber into ditches, opening the highway to cautious passage.

---

## 3. Preservation of Year of Ash Engine Invariants

- State changes are strictly evaluated through `FactionWarContentCatalog.GetActiveLocationOverride`.
- No direct mutation of Year of Ash crisis flag states occurs in these overrides; instead, they reflect the visible physical reality of the crisis as days advance.
