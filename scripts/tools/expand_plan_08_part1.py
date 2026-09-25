import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/08-visual-art-completion.md"

header = """# Plan 08 — Visual Art Completion: Locations, Character Portraits, Diegetic Iconography & Aesthetic Constitution

**Package:** `PLAN-08-VISUAL-ART-COMPLETION`
**Document Class:** Master Visual Art Architecture, Production Catalog & Integration Blueprint
**Authority Level:** Canonical Production Plan
**Target Runtimes:** Ashfall.Core (`netstandard2.1`, Engine-Free) · Godot Host (`net8.0`) · Ashfall.Core.Tests (`net9.0`)
**Data Authority:** `Assets/StreamingAssets/Data/assets.json` (snake_case JSON, schema-validated)
**Historical Anchor:** piagentsplans Wave 1 (2026-08-30) · Visual Art & Atmosphere Suite · Master Authority Volumes 8, 19, 31, 45
**Save Authority:** Presentation State Layer; Pure Domain Asset ID Resolvers; Zero Visual State in Saves
**Determinism Mandate:** Pure Domain Asset Mappings; Deterministic Fallback Chains; Strict Texture Bounds

---

# SECTION I: COMPREHENSIVE ARCHITECTURAL OBJECTIVES & VISUAL CONSTITUTION

Plan 08 closes the visual asset backlog of *ASHFALL*, elevating the game from placeholder boxes and colored node markers into a visually unified, atmospheric post-nuclear wasteland. In survival management, visual art is not mere decoration; it establishes the visceral psychological weight of human survival in a scorched world: the peeling lead paint of underground bunks, the bleak expanse of irradiated salt flats, the sunken hollows of irradiated survivor cheeks, and the precision-machined lines of scavenged pre-war relics.

This architecture formalizes the full visual production pipeline: `Domain Entity ID` -> `Visual Asset Resolver` -> `Fallback Archetype Resolution` -> `Godot Texture Streaming` -> `Fixed 1080p Viewport Presentation`:

```
+===================================================================================================+
|                                    ASHFALL DOMAIN CORE ENTITIES                                   |
|   Locations (261 Nodes) · Survivors (Cohort & Named NPCs) · Items (315 Entities) · Factions (43)  |
+===================================================================================================+
                                                  │
                                                  ▼ (Domain String Identifiers: loc_*, surv_*, item_*)
+===================================================================================================+
|                        ASHFALL CORE VISUAL RESOLUTION & FALLBACK ENGINE                           |
|  Assets/Ashfall.Core/Visual/                                                                      |
|  - LocationArtRegistry (Bi-Directional Location-to-Texture Mapping, Ambient Tint Tags)            |
|  - PortraitAssetResolver (Named Survivor Match -> Trade Archetype -> Affliction Overlay Fallback) |
|  - AssetCoverageValidator (Automated CI Coverage Auditing, Orphan Asset Detection)                |
|  - Pure Domain Logic - 100% Engine-Free (Zero Godot/UnityEngine Texture Sinks)                    |
+===================================================================================================+
        │                                         │                                      │
        ▼                                         ▼                                      ▼
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
| WASTELAND LOCATION ART    |   | CHARACTER PORTRAITS               |   | DIEGETIC ICONOGRAPHY      |
| - 60 Authored Landscapes  |   | - 60 Named & Archetype Portraits  |   | - 60 Technical Item Icons |
| - 4 Strategic Batches:    |   | - 6 Core Trade Archetypes:        |   | - 128x128 Pixel Standards |
|   Seats, Routes, Strikes, |     Medic, Mechanic, Soldier,         |   | - High-Contrast Alpha     |
|   Maritime Flotilla Nodes |     Farmer, Scavenger, Warden         |   |   Masks for CRT Palettes  |
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
        │                                         │                                      │
        └─────────────────────────────────────────┼──────────────────────────────────────┘
                                                  ▼
+===================================================================================================+
|                           GODOT HOST RENDERING & TEXTURE STREAMING                                |
|  src/UI/Components/ & assets/art/ (Git LFS Tracked, VRAM Compressed .import Presets)              |
|  - Fixed 1920x1080 Viewport with Pixel-Accurate Scaling                                           |
|  - Low VRAM Footprint: Dedicated Texture Atlasing & BC7/Lossless WebP Compression                 |
|  - WCAG AA High-Contrast Accessibility Overlays & Colorblind-Safe Danger Badges                   |
+===================================================================================================+
```

### 1.1 Non-Negotiable Visual Invariants
1. **Engine Separation**: Zero references to `Godot`, `Texture2D`, `Image`, or engine graphics APIs inside `Assets/Ashfall.Core/Visual/`. All asset mappings operate on pure string keys and enum flags.
2. **Authoritative Asset Registry**: Master texture paths, dimensions, compression formats, and fallback bindings reside exclusively in `Assets/StreamingAssets/Data/assets.json` with `schema_version: 1`.
3. **No Unmanaged Art Assets**: All production illustrations and sprites reside in `assets/art/` or `assets/sprites/`, tracked via Git LFS (`.gitattributes`) with explicit `.import` preset files.
4. **Strict Color Palette & Aesthetic Tone**: Visual art must strictly adhere to the Ashfall Aesthetic Standard: desaturated earthen tones, cadmium yellows, oxidized iron reds, and phosphor terminal greens. Zero high-saturation fantasy or sci-fi neon aesthetics.

---

# SECTION II: THE ASHFALL AESTHETIC STANDARD & COLOR PALETTES

### 2.1 The Four Regional Palette Grading Bands
1. **The Ash Flats (Surface Wasteland)**:
   - Dominant Hues: Bone white, ash grey (`#4A4A48`), scorched charcoal (`#1F1F1E`), pale cadmium yellow (`#D4AF37`).
   - Lighting: Harsh overexposed overcast sky with high atmospheric haze.
2. **Subterranean Bunker Vaults (Interior)**:
   - Dominant Hues: Oxidized steel green (`#2D3830`), emergency amber (`#CC7A00`), raw concrete grey (`#5C5F60`).
   - Lighting: High-contrast directional tungsten filament and fluorescent green glow; deep heavy shadows.
3. **The Irradiated Crater Zones (Ground Zero)**:
   - Dominant Hues: Cherenkov blue-violet (`#3B447A`), toxic radioactive yellow-green (`#879942`), charred obsidian (`#0F1117`).
   - Lighting: Ionized atmospheric glow, dust scintillation sparkle.
4. **The Maritime Black Flotilla (Coastal Ruins)**:
   - Dominant Hues: Cold salt-spray slate (`#33424D`), sea-foam grey (`#6B7A82`), rusted iron flake (`#8B3A2B`).
   - Lighting: Heavy cold sea fog, silhouette lighting against grey waters.

---

# SECTION III: AUTHORITATIVE DATA SCHEMAS (`Assets/StreamingAssets/Data/`)

### 3.1 `assets.json`
```json
{
  "schema_version": 1,
  "catalog_id": "assets_master_v1",
  "comment": "Authoritative Master Registry of Location Art, Portraits and Icons",
  "locations": [
    {
      "location_id": "loc_iron_creek_crossing",
      "texture_path": "assets/art/locations/loc_iron_creek_crossing.webp",
      "palette_band": "ash_flats",
      "resolution_width": 960,
      "resolution_height": 540,
      "fallback_archetype": "ash_flats_generic"
    }
  ],
  "portraits": [
    {
      "character_id": "char_commander_karkoff",
      "texture_path": "assets/sprites/Characters/portraits/port_named_karkoff.webp",
      "trade_archetype": "soldier",
      "resolution_width": 256,
      "resolution_height": 256,
      "fallback_portrait": "port_soldier_veteran_male"
    }
  ]
}
```

---

# SECTION IV: MASTER CATALOG OF 60 WASTELAND LOCATION ILLUSTRATIONS

The following catalog defines 60 fully authored location illustrations across the 4 strategic production batches:

"""

locations_catalog = []
loc_types = [
    # Batch 1: Faction Seats & Major Hubs (1 - 15)
    ("Iron Mountain Foundry Citadel", "loc_citadel_foundry", "subterranean_bunker", "Massive concrete blast doors flanked by molten slag runoff trenches and heavy exhaust chimneys."),
    ("New Dawn Directorate Stratum-1", "loc_stratum_command", "subterranean_bunker", "Pristine pre-war administrative silo with glowing phosphor terminal arrays and armed sentry embrasures."),
    ("Oasis Agricultural Hydro-Domes", "loc_oasis_greenhouses", "ash_flats", "Shattered acrylic geodesic domes housing mutated lush green vegetation and misting cooling towers."),
    ("The Sinking Cathedral of St. Barbara", "loc_sunken_cathedral", "ash_flats", "Partially submerged gothic bell tower rising above a sulfurous mud lake, connected by wooden catwalks."),
    ("Rustland Free Trader Crossroads", "loc_trader_junction", "ash_flats", "Sprawling bazaar constructed from stacked shipping containers and rusted freight train boxcars."),
    ("Cinder Cult Scorched Altar", "loc_cinder_altar", "irradiated_crater", "Jagged monument of twisted structural steel beams surrounded by perpetual crude oil flare pits."),
    ("Black Flotilla Sovereign Hulk", "loc_flotilla_flagship", "maritime_coastal", "Grounded pre-war container supertanker converted into a fortified floating city with crane batteries."),
    ("Borehole Station Omega", "loc_borehole_deep", "subterranean_bunker", "Miles-deep subterranean geothermal elevator head flanked by roaring high-pressure steam turbines."),
    ("Redoubt Seven Emergency Clinic", "loc_redoubt_clinic", "subterranean_bunker", "Triage ward filled with canvas cots, hanging intravenous bottles, and lead-shielded examination stalls."),
    ("The Salt Flats Observation Mast", "loc_salt_flats_mast", "ash_flats", "Lattice steel transmission tower rising out of blinding white crystallized salt desert."),
    ("Cobalt Silo Nine Ruin", "loc_cobalt_silo_ruin", "irradiated_crater", "Shattered missile silo lid collapsed into radioactive rubble with visible blue Cherenkov glow."),
    ("Waystation Delta Railhead", "loc_waystation_rail", "ash_flats", "Abandoned diesel locomotive depot with rusted turntables and collapsed corrugated roof panels."),
    ("Whispering Canyon Relay Tower", "loc_whisper_canyon", "ash_flats", "Narrow red sandstone gorge spanned by a pre-war suspension bridge and high-gain antenna arrays."),
    ("The Drowned Drydock Sub-Base", "loc_drydock_sub_base", "maritime_coastal", "Cavernous subterranean submarine pen flooded with brackish water, housing a rusted diesel sub hull."),
    ("Perimeter Gate Bunker Alpha", "loc_perimeter_gate_alpha", "subterranean_bunker", "Reinforced concrete pillbox with armored vision slits commanding a wide razor-wire killing field."),

    # Batch 2: Main Route Expedition Nodes (16 - 30)
    ("Highway Overpass Dead-End", "loc_highway_overpass", "ash_flats", "Collapsed interstate viaduct with hanging rebar ribbons and abandoned civilian automobile wrecks."),
    ("Irrigated Rad-Mushroom Culvert", "loc_rad_culvert", "subterranean_bunker", "Underground stormwater drainage vault overgrown with bioluminescent pale fungi colonies."),
    ("The Scrapped Armor Graveyard", "loc_armor_graveyard", "ash_flats", "Rows of burned-out main battle tanks and armored personnel carriers half-buried in drifting silt."),
    ("Desalination Pump Outpost", "loc_desal_outpost", "maritime_coastal", "Weather-beaten concrete pump house with exposed titanium intake conduits leading into the grey sea."),
    ("High-Voltage Pylon Substation", "loc_pylon_substation", "ash_flats", "Tangled web of fallen ceramic insulators and high-tension cables humming faintly in wind."),
    ("The Buried Supermarket Vault", "loc_buried_supermarket", "subterranean_bunker", "Underground retail warehouse with collapsed shelving and scattered rusted tin cans."),
    ("Quarry Stone Crusher Platform", "loc_stone_crusher", "ash_flats", "Massive industrial jaw crusher mechanism perched over a deep stepped granite quarry pit."),
    ("Bleached Timber Logging Camp", "loc_timber_camp", "ash_flats", "Skeletal remains of dead pine forest with petrified grey trunks and decaying diesel skidders."),
    ("The Contaminated Artesian Well", "loc_contam_well", "ash_flats", "Hand-cranked iron pump standing in a pool of iridescent, heavy-metal tainted runoff water."),
    ("Military Checkpoint Mile-4", "loc_checkpoint_mile4", "ash_flats", "Sandbag barricades and rusted razor wire concertina coils beside a burned-out armored gatehouse."),
    ("The Cold Radio Relay Shack", "loc_relay_shack", "ash_flats", "Small cinderblock shelter with guyed antenna wires whistling in freezing mountain pass winds."),
    ("Underground Water Cistern 3", "loc_cistern_three", "subterranean_bunker", "Echoing vaulted reservoir with stone pillars supporting subterranean ceiling arches."),
    ("The Scorched Orchard Farmstead", "loc_scorched_orchard", "ash_flats", "Rows of dead black fruit trees surrounding a collapsed stone farmhouse cellar."),
    ("Tailings Pond Slag Heap", "loc_slag_heap", "irradiated_crater", "Mountain of acidic mine tailings glowing faintly amber in the toxic dusk haze."),
    ("The Freight Tunnel Collapse", "loc_freight_collapse", "subterranean_bunker", "Single-track rail tunnel blocked by massive boulders and crushed iron freight flatcars."),

    # Batch 3: Faction Strike & Aftermath Sites (31 - 45)
    ("Ground Zero Impact Crater", "loc_ground_zero_crater", "irradiated_crater", "Glassified silica crater basin with fused green tektite sands and intense ionizing radiation."),
    ("The Bombing Run Trenches", "loc_bomb_trenches", "ash_flats", "Zig-zagging infantry trench network excavated into hardpan clay, littered with spent shell casings."),
    ("Shattered Hydro-Electric Dam", "loc_shattered_dam", "maritime_coastal", "Colossal fractured concrete arch dam with torrent of muddy water pouring through breach."),
    ("The Ambushed Armored Train", "loc_ambushed_train", "ash_flats", "Derailed armored locomotive lying on its flank beside scorched tank-car explosion craters."),
    ("Burned Outposts of the Wardens", "loc_warden_outpost_ruin", "subterranean_bunker", "Fire-blackened bunker embrasures and collapsed sandbag bastions."),
    ("The Chlorine Gas Sinkhole", "loc_chlorine_sinkhole", "irradiated_crater", "Low depression blanketed by dense yellow-green toxic vapor clouds obscuring the ground."),
    ("Fallen Reconnaissance Drone Crash", "loc_drone_crash", "ash_flats", "Composite delta-wing surveillance aircraft impaled into a dry clay embankment."),
    ("The Ruined Field Hospital", "loc_field_hospital_ruin", "ash_flats", "Torn canvas triage tents, rusted hospital gurneys, and scattered medical crates."),
    ("Scavenger Barricade Breach", "loc_barricade_breach", "subterranean_bunker", "Demolished scrap-metal wall with signs of heavy explosive breaching charges."),
    ("The Mortar Pit Encampment", "loc_mortar_pits", "ash_flats", "Fortified earthen redoubts containing rusted pre-war 120mm mortar tubes and empty ammo crates."),
    ("Scattered Convoy Waypoint", "loc_convoy_scatter", "ash_flats", "Five six-wheel transport trucks abandoned in defensive circle with shot-out tires."),
    ("The Burned Hydroponic Silo", "loc_burned_silo", "subterranean_bunker", "Charred hydroponic PVC rack skeletons and melted drip irrigation tubing."),
    ("The Sentry Pillbox Detonation", "loc_pillbox_detonation", "subterranean_bunker", "Concrete fortification split cleanly in half by subterranean mining satchel charge."),
    ("Raider Scaffold Gallows", "loc_raider_gallows", "ash_flats", "Towering timber scaffold decorated with warning glyphs and hanging sheet metal noisemakers."),
    ("The Shattered Water Tower", "loc_shattered_water_tower", "ash_flats", "Four-legged steel water tower collapsed into twisted iron wreckage."),

    # Batch 4: Maritime, Coastal & Deep Strata Nodes (46 - 60)
    ("The Fog-Bound Barrier Reef", "loc_fog_barrier_reef", "maritime_coastal", "Jagged black volcanic rocks exposed at low tide, shrouded in dense salt fog."),
    ("Submerged Coastal Highway", "loc_submerged_highway", "maritime_coastal", "Asphalt roadway disappearing beneath grey ocean swells, lined by submerged lampposts."),
    ("Wreck of the Ore Freighter", "loc_freighter_wreck", "maritime_coastal", "Massive rusted steel bow protruding at 45 degrees from the coastal surf."),
    ("The Kelp Forest Dive Trench", "loc_kelp_dive_trench", "maritime_coastal", "Deep submarine trench filled with towering mutated kelp ribbons waving in dark currents."),
    ("The Tidal Turbine Battery", "loc_tidal_turbines", "maritime_coastal", "Concrete caissons in shallow surf housing submerged hydro-turbines clogged with barnacles."),
    ("Sub-Basement Boiler Catacomb", "loc_boiler_catacomb", "subterranean_bunker", "Echoing vaulted brick chamber filled with looming steam boilers and labyrinthine steam pipes."),
    ("The Flooded Ventilation Shaft", "loc_flooded_shaft", "subterranean_bunker", "Vertical six-foot steel air shaft with stagnant oily water lapping three feet below grating."),
    ("The Pre-War Cold Storage Vault", "loc_cold_storage_vault", "subterranean_bunker", "Frost-covered vault lined with empty stainless steel racks and humming ammonia compressors."),
    ("Geophone Seismic Monitoring Well", "loc_geophone_well", "subterranean_bunker", "Borehole observation room with pen chart recorders tracing tremors on paper rolls."),
    ("The Collapsed Escape Tunnel", "loc_escape_tunnel", "subterranean_bunker", "Narrow shored timber drift tunnel ending in massive granite ceiling block fall."),
    ("The Coastal Beacon Lighthouse", "loc_beacon_lighthouse", "maritime_coastal", "Cast-iron lighthouse tower with shattered lantern room overlooking boiling rocky surf."),
    ("The Sunken Navy Frigate", "loc_sunken_frigate", "maritime_coastal", "Guided missile frigate resting on coastal reef shelf, deck guns encrusted with mussels."),
    ("The Deep Borehole Sump", "loc_borehole_sump", "subterranean_bunker", "Dark industrial cistern receiving subterranean seepage water pumped from deep shaft."),
    ("The Smuggler's Sea Cave", "loc_smuggler_cave", "maritime_coastal", "Natural sea cavern accessible only at low tide, containing hidden fuel drums and skiffs."),
    ("The Vault Archive Vault Door", "loc_archive_vault_door", "subterranean_bunker", "Massive 20-ton round bank vault door sealed shut with six heavy locking lugs.")
]

for idx, loc in enumerate(loc_types, start=1):
    title, loc_id, palette, desc = loc
    entry = f"""### LOCATION ARTWORK #{idx:02d}: `{title.upper()}`
- **Master Location Entity**: `{loc_id}`
- **File System Asset**: `assets/art/locations/{loc_id}.webp`
- **Palette Grading Band**: `{palette}` (Native Resolution: 960x540, Lossless WebP)
- **Art Direction & Composition**:
  > *"{desc} Composition emphasizes claustrophobic horizon, harsh lighting contrast, and worn, oxidized surface materials."*
- **Godot Import Configuration**:
  - Format: VRAM Compressed (BC7 Desktop / ETC2 Mobile fallback).
  - Filter: Linear Mipmap; Slices: 1; Compress Mode: High Quality.
  - Git LFS Status: Verified tracked (`assets/art/locations/*.webp`).
- **Referential Seam**: Bound to `locations.json` entry `{loc_id}` and mapped in `LocationArtRegistry.cs`.

"""
    locations_catalog.append(entry)

part1_text = header + "".join(locations_catalog)

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(part1_text)

print(f"Plan 08 Part 1 written! Current size: {len(part1_text)} chars")
