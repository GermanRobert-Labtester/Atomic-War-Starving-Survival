# Faction War Deep Lore Locations Handoff

> **Integration Anchor:** Plan 116 (Deep Lore Locations Expansion, 10 → 25 destinations)
> **Source Catalog:** `Assets/StreamingAssets/Data/deep_lore_locations.json`
> **Target Overrides:** Indices 11, 14, 18, 19 in `faction_war_location_overrides.json`

---

## 1. Architectural Handoff Objectives

Plan 116 established deep exploration destinations with distinct travel hazards, radiation profiles, loot tables, and narrative discovery entries.
Plan 124 bridges these exploration nodes with the dynamic Faction War timeline. By applying temporal overrides to 4 key Deep Lore destinations, players revisiting these remote regions discover that the war's violence, pollution, and resource demands reach far beyond the municipal core.

---

## 2. Integrated Deep Lore Locations

### 2.1 `location_municipal_water_reservoir` → `loc_override_well_contaminated`
- **Active Window:** Days 240–280 (41 days)
- **Override Type:** `contaminated`
- **Display Name Override:** `Municipal Water Reservoir (Soot-Choked Basin)`
- **Atmosphere Override:** `The stench of scorched petroleum, sulfur scum, and dead algae.`
- **Narrative & Gameplay Effect:**
  During this window, upstream petrochemical burning washes toxic soot into the reservoir basin. Expeditions encountering the reservoir find that the open water surface is slicked with rainbow film, requiring chemical filtration or boiling before collection.

### 2.2 `location_chemical_plant` → `loc_override_factory_occupied`
- **Active Window:** Days 300–350 (51 days)
- **Override Type:** `occupied`
- **Display Name Override:** `Chemical Synthesis Plant (Militia Extraction Post)`
- **Atmosphere Override:** `Pungent ammonia fumes, crackling coal braziers, and watchful guards on iron catwalks.`
- **Narrative & Gameplay Effect:**
  A local faction militia secures the facility to extract industrial solvent precursors and ammonia. Rather than exploring an empty derelict, expeditions face armed sentries and fortified distillation towers.

### 2.3 `location_metro_station` → `loc_override_station_reclaimed`
- **Active Window:** Days 350–400 (51 days)
- **Override Type:** `reclaimed`
- **Display Name Override:** `Flooded Metro Station (Salvage Platform)`
- **Atmosphere Override:** `Echoing pump clatter, dripping conduit, and the smell of grease and swamp grass.`
- **Narrative & Gameplay Effect:**
  An organized survivor consortium mounts hand-pumps and wooden duckboards over the flooded tracks to reach dry maintenance rooms, shifting the location from a drowning hazard to a cautious trade and repair station.

### 2.4 `location_burned_woodland` → `loc_override_field_scorched`
- **Active Window:** Days 360–400 (41 days)
- **Override Type:** `post_strike`
- **Display Name Override:** `Burned Woodland Basin (Secondary Burn Zone)`
- **Atmosphere Override:** `Smoldering peat smoke, white ash drifts, and complete silence.`
- **Narrative & Gameplay Effect:**
  Following a secondary wildfire triggered by artillery flares, the already damaged woodland is swept by ground fires that consume remaining root scrub and peat, transforming the basin into an ash desert.

---

## 3. Preservation of Deep Lore Systems

These overrides purely modify presentation strings (`displayNameOverride`, `descriptionOverride`, `atmosphereOverride`).
- They do **not** overwrite the underlying loot tables in `deep_lore_locations.json`.
- They do **not** bypass `DeepLoreLocationCatalogLoader`.
- Expeditions continue to roll deterministic loot and radiation encounters via canonical Core systems.
