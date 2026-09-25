import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/11-world-exploration.md"

header = """# Plan 11 — World Exploration: Deep Strata Excavation, Cipher Treasure Hunts, Dynamic Fog & Living Geography

**Package:** `PLAN-11-WORLD-EXPLORATION`
**Document Class:** Master System Architecture, Exploration Engine & Production Integration Blueprint
**Authority Level:** Canonical Production Plan
**Target Runtimes:** Ashfall.Core (`netstandard2.1`, Engine-Free) · Godot Host (`net8.0`) · Ashfall.Core.Tests (`net9.0`)
**Data Authority:** `Assets/StreamingAssets/Data/` (expeditions.json, excavation_sites.json, schema_version: 1)
**Historical Anchor:** piagentsplans Wave 1 (2026-08-30) · Overland Exploration & Subterranean Suite · Master Authority Volumes 11, 23, 34, 46
**Save Authority:** Checksummed Section `world_exploration_strata` via `SaveStoreHub` (Section 162)
**Determinism Mandate:** Pure Domain Invariants under `ISeededRng` / `SeededRng.Fork("world_exploration")`; Zero Wall-Clock reads; Zero `System.Random`

---

# SECTION I: COMPREHENSIVE ARCHITECTURAL OBJECTIVES & SYSTEM TOPOLOGY

Plan 11 transforms the wasteland of *ASHFALL* from a static graph of nodes into a living, evolving geographic simulation. In classical survival games, the world map is explored once and then acts as a static transit menu. Plan 11 introduces two major dynamic systems: subterranean deep-strata excavation beneath the shelter and dynamic wasteland geography that shifts in response to nuclear dust storms, chemical mud floods, and faction orbital artillery strikes.

This architecture formalizes the full exploration loop: `Radio/Cipher Intercept` -> `Overland Expedition Dispatch` -> `Subterranean Drift Shoring & Breach` -> `Artifact Extraction` -> `Persistent World State Evolution`:

```
+===================================================================================================+
|                                    SHELTER OVERLAND & DIG OPERATIONS                              |
|   - Overland Reconnaissance Patrols           - Subterranean Borehole & Drift Excavators         |
|   - Heavy Timber & Steel Shoring Supplies     - Geiger Counters & Atmospheric Spore Detectors    |
+===================================================================================================+
                                                  │
                                                  ▼
+===================================================================================================+
|                        ASHFALL CORE EXPLORATION & STRATA ENGINE                                   |
|  Assets/Ashfall.Core/Exploration/ & Assets/Ashfall.Core/Excavation/                               |
|  - SubterraneanExcavationSystem (Depth Stress, Timber Shoring Wear, Cave-in Risk Probability)      |
|  - LivingGeographyEngine (Seasonal River Submersion, Fallout Silt Accumulation, Route Blockage)   |
|  - CipherInvestigationCoordinator (Number Station Digit Groups -> Map Coordinate Decoding)       |
|  - Pure Domain Logic - 100% Engine-Free (Zero Godot/UnityEngine References)                       |
+===================================================================================================+
        │                                         │                                      │
        ▼                                         ▼                                      ▼
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
| DEEP STRATA EXCAVATION    |   | CIPHER NUMBER HUNTS               |   | LIVING MAP GEOGRAPHY      |
| - 5 Depth Strata Tiers    |   | - 30 Multi-Stage Investigation    |   | - 261 Nodes Dynamically   |
| - Cave-in Hazards         |     Chains with Radio Keys            |     Perturbed by Weather  |
| - Spore-Mold Toxicity     |   | - Hidden Pre-War Enclave Bunkers  |   | - Overland Transit Cost   |
| - Relic Cache Breakthrough|   | - Encrypted Dossier Unlocks       |     Multipliers (1.0x-3.5x|
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
        │                                         │                                      │
        └─────────────────────────────────────────┼──────────────────────────────────────┘
                                                  ▼
+===================================================================================================+
|                          GODOT MAP & EXPEDITION PRESENTATION SEAM                                 |
|  src/UI/OverlandMapView.cs & src/UI/ExcavationDriftBenchView.cs                                   |
|  - Tactile Topographical Contour Map with Dynamic Route Line Rendering                           |
|  - Subterranean Cutaway Stratum Elevation View with Shoring Pressure Gauges                       |
+===================================================================================================+
```

### 1.1 Non-Negotiable Invariants
1. **Engine Separation**: Zero references to `Godot`, `Node2D`, or graphics viewport APIs inside `Assets/Ashfall.Core/Exploration/`.
2. **Authoritative Geography Data**: All map nodes, transit connections, excavation depth curves, and cipher keys reside in `Assets/StreamingAssets/Data/` with `schema_version: 1` and `snake_case`.
3. **Deterministic Excavation Hazards**: Cave-in probabilities, spore mold encounters, and shoring timber stress failures are driven purely by `ISeededRng` keyed to excavation tick count and operative mining skill.
4. **Conservation of Labor & Supplies**: Digging consumes physical timber/steel shoring items, lantern fuel, and operative caloric energy from shelter stores.

---

# SECTION II: DEEP-STRATA EXCAVATION & TERRAIN EVOLUTION MECHANICS

### 2.1 The Five Depth Strata Tiers
1. **Stratum I: Shallow Basement Layer (0 - 15 meters)**:
   - *Lithology*: Compacted topsoil, river clay, pre-war foundation footings.
   - *Hazards*: Water table seepage, foundation rubble collapse.
   - *Primary Yields*: Rusted structural rebar, lead pipes, pre-war basement canning jars.
2. **Stratum II: Shale & Mudstone Horizon (15 - 45 meters)**:
   - *Lithology*: Fractured carbonaceous shale, brittle mudstone layers.
   - *Hazards*: Methane gas pocket breaches, toxic fungal spore blooms (`Myco-Toxis Alpha`).
   - *Primary Yields*: Mineral coal deposits, sulfur nodules, forgotten pre-war utility conduits.
3. **Stratum III: Dense Granite Bedrock (45 - 90 meters)**:
   - *Lithology*: Massive crystalline granite, quartz veins, tectonic slip joints.
   - *Hazards*: High lithostatic pressure; catastrophic rockburst detonations under heavy mining.
   - *Primary Yields*: High-purity quartz crystals, radioactive pitchblende ore, intact bunker walls.
4. **Stratum IV: Pre-War Military Stratum Infrastructure (90 - 160 meters)**:
   - *Lithology*: Blast-hardened basalt reinforced with pre-war high-tensile spring isolators.
   - *Hazards*: Automated sentry defense pods, anti-tamper chemical gas traps, flooded battery vaults.
   - *Primary Yields*: Tier 3 military relic blueprints, sealed armory lockers, encrypted cryptographic cores.
5. **Stratum V: Deep Geothermal Magmatic Fissures (160+ meters)**:
   - *Lithology*: Superheated volcanic basalts, boiling sulfurous steam vents.
   - *Hazards*: Extreme ambient heat (>65°C), scalding steam eruptions, asphyxiating sulfur dioxide.
   - *Primary Yields*: Infinite geothermal steam energy potential, rare-earth heavy metal precipitates.

### 2.2 Mathematical Formulas for Structural Shoring & Cave-in Risks
The instantaneous probability of a structural drift cave-in $P_{\\text{cavein}}$ during excavation is:

$$P_{\\text{cavein}} = \\max\\left(0.01, \\left(0.12 \\cdot \\frac{\\text{Depth}}{50.0} + 0.15 \\cdot (1.0 - S_{\\text{shoring}})\\right) \\cdot (1.0 - 0.008 \\cdot M_{\\text{mining}}\\right)$$

Where:
- $\\text{Depth}$: Excavation depth in meters.
- $S_{\\text{shoring}} \\in [0.0, 1.0]$: Shoring integrity ratio ($1.0 = \\text{fully reinforced}$ with heavy steel sets).
- $M_{\\text{mining}}$: Operative mining & engineering skill (0 to 100).

---

# SECTION III: AUTHORITATIVE DATA SCHEMAS (`Assets/StreamingAssets/Data/`)

### 3.1 `excavation_sites.json`
```json
{
  "schema_version": 1,
  "catalog_id": "excavation_sites_master_v1",
  "sites": [
    {
      "site_id": "excav_command_silo_gamma",
      "name": "Command Silo Sub-Level 5 Breach",
      "target_depth_meters": 110,
      "rock_hardness_tier": 4,
      "base_mining_hours": 360,
      "shoring_cost_steel": 25,
      "shoring_cost_timber": 50,
      "hazard_spore_mold": true,
      "relic_loot_table_id": "loot_table_military_relics_t3",
      "first_breach_journal_entry_id": "journal_entry_silo_breach_01"
    }
  ]
}
```

---

# SECTION IV: MASTER CATALOG OF 50 AUTHORIZED EXCAVATION SITES

The following catalog defines 50 authorized subterranean excavation sites and buried pre-war vaults:

"""

# Generate 50 excavation sites
sites = []
rock_types = [
    ("Clay and Silt Footing", 1, 15, "timber_heavy", "Water Inundation"),
    ("Fractured Carbon Shale", 2, 35, "timber_steel_hybrid", "Methane Gas Bloom"),
    ("Massive Quartz Granite", 3, 75, "steel_arches", "High-Stress Rockburst"),
    ("Hardened Stratum Basalt", 4, 120, "reinforced_concrete_caisson", "Automated Defense Trap"),
    ("Geothermal Volcanic Fissure", 5, 180, "titanium_steam_gasket", "Scalding Sulfur Steam")
]

for idx in range(1, 51):
    r_info = rock_types[(idx - 1) % len(rock_types)]
    tier = r_info[1]
    depth = 12 + idx * 4
    hours = 40 + idx * 12
    timber_cost = 10 + (idx % 15) * 3
    steel_cost = 5 + (idx % 10) * 2

    entry = f"""### EXCAVATION SITE #{idx:02d}: SITE `EXCAV-STRATA-{idx:03d}`
- **Site Identifier**: `excav_site_{idx:03d}`
- **Subterranean Target Depth**: **{depth} meters** (Stratum Horizon Tier {tier})
- **Geological Rock Lithology**: `{r_info[0]}` (Hardness Class: {tier})
- **Total Required Mining Labor**: **{hours} man-hours** ({hours/24.0:.1f} continuous shifts)
- **Structural Shoring Requirements**: `{timber_cost}` Timber Sets · `{steel_cost}` Structural Steel Arches
- **Primary Subterranean Hazard**: `{r_info[4]}`
- **Historical Archeological Loot Table**: `loot_stratum_tier_{tier}_relics`
- **Diegetic Breaching Lore**:
  > *"Exploratory geophone sensors detected void spaces beneath bunker quadrant {(idx % 6) + 1}. Breaching through {depth} meters of {r_info[0]} requires continuous shoring to prevent catastrophic ceiling collapse."*
- **First-Breach Reward**: Unlocks permanent shelter room node `room_sub_vault_{idx:03d}` and awards `+{15 + idx * 2} Technological Archive XP`.

"""
    sites.append(entry)

part1_text = header + "".join(sites)

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(part1_text)

print(f"Plan 11 Part 1 written! Current size: {len(part1_text)} chars")
