# Faction War Damaged Zone Handoff

> **Integration Anchor:** Damaged Zone Catalogs & Radiation Subsystems
> **Source Catalogs:** `damaged_zone_locations.json`, `deep_lore_locations.json`
> **Target Overrides:** `loc_override_well_contaminated`, `loc_override_field_scorched`, `loc_override_almshouse_post_strike`, `loc_override_cache_looted`

---

## 1. Environmental Degradation Architecture

Damaged zones in ASHFALL model radiation hotspots, physical structural instability, and toxic contamination.
Plan 124 aligns temporal location overrides with environmental degradation so that war damage visibly alters the sensory and atmospheric identity of affected sites.

---

## 2. Integrated Environmental Hazard Profiles

### 2.1 Petrochemical & Toxic Runoff (`location_municipal_water_reservoir`)
- **Override State:** `loc_override_well_contaminated` (Days 240–280)
- **Atmosphere:** Scum, sulfur smell, scorched petroleum.
- **Environmental Context:** War actions upstream discharge chemical waste into the city water supply. The override provides visual and sensory feedback of water danger before the player attempts collection.

### 2.2 Secondary Pyrogenic Ash & Peat Smolder (`location_burned_woodland`)
- **Override State:** `loc_override_field_scorched` (Days 360–400)
- **Atmosphere:** White ash drifts, smoldering peat, total silence.
- **Environmental Context:** Combat flare fallout reignites dry peat beds. The woodland becomes a stark secondary burn zone with zero natural cover.

### 2.3 Subterranean Sump Fluid & Toxic Seepage (`loc_d9_cache_bunker_delta`)
- **Override State:** `loc_override_cache_looted` (Days 300–450)
- **Atmosphere:** Hydraulic fluid, cutting torch slag, stale stagnant sump air.
- **Environmental Context:** Breach of pre-war pressure seals introduces corrosive groundwater and electrical fire residue into previously sterile storage bays.

---

## 3. Core Radiation & Needs Decoupling

The overrides preserve Core Invariant 5 (presentation only):
- Radiation calculations, rad accumulation, and toxic water illness remain managed by `RadiationSystem` and `MedicalSystem`.
- Overrides provide atmospheric cues that ground the player's survival choices in visible environmental feedback.
