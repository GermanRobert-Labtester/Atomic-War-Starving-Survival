# ASHFALL — ART ASSET INTEGRATION MASTER PLAN

> **Source**: `ASHFALL_PROMPT_CATALOG_EXPANSION.md` (44KB, ~500 AI art prompts)
> **Scope**: Generate & integrate ~330 art assets to bring visual coverage to 100%
> **Current state**: ~300 existing assets from prior libraries | **Missing**: ~330 assets

---

## I. ASSET INVENTORY — WHAT EXISTS VS WHAT'S MISSING

| Category | Existing | Missing | Total | Coverage |
|----------|----------|---------|-------|----------|
| Items (icons) | 249 of 419 | **170** | 419 | 59% → target 100% |
| Locations (establishing) | 5 of 47 | **42** | 47 | 11% → target 100% |
| Survivors (portraits) | 12 generic | **96 named** | 108 | 11% → target 100% |
| Factions (lineups) | 0 | **5** | 5 | 0% → target 100% |
| Weather (effects) | 6 | **15** | 21 | 29% → target 100% |
| **TOTAL** | ~272 | **~328** | ~600 | ~45% → target 100% |

---

## II. GENERATION PIPELINE — TWO DISTINCT STYLES

The existing production pipeline uses **two different visual languages** that must be maintained:

### Style A: ITEMS (Flux 2 Pro via Adobe Firefly)
```
Post-apocalyptic survival game inventory icon, isolated object centered on
pure flat black (#000000) background, dramatic directional rim lighting from
top-left, volumetric dust particles, desaturated color palette with selective
orange-amber highlights, worn and weathered textures, micro-scratches and
grime detail, photorealistic material rendering, cinematic product shot,
no text, no labels, no shadows outside object, no background elements
```
- **Model**: Flux 2 Pro via Firefly
- **Aspect**: 1:1 default (4:3 for long objects, 3:4 for body gear)
- **Format**: PNG, 1024×1024 (or 1024×768 / 768×1024)
- **Negative**: `cartoon, anime, flat icon, logo, watermark, signature, text, label, gradient background, colorful background, bright colors, clean new condition, fantasy, sci-fi laser, alien, UI chrome`

### Style B: LOCATIONS, SURVIVORS, FACTIONS, WEATHER (Hand-Painted Illustration)
```
Original 2D hand-painted survival-management game art, grounded grim realism,
charcoal pencil underdrawing and dry gouache texture, cold restrained palette
of charcoal, concrete grey, faded blue-grey, rust brown, dirty bone, and rare
muted amber practical light; radiation is a subtle cyan-green contamination
cue only. Nuclear-winter ash, condensation, repair marks, functional
materials. No text, logos, flags, brands, readable labels, fantasy, gore, or
weapon glamour.
```
- **Model**: Flux 2 Pro via Firefly (with hand-painted style guidance)
- **Aspect**: 16:9 for locations/weather, 3:4 for portraits
- **Format**: PNG, 1920×1080 (locations) / 768×1024 (portraits)
- **Negative**: `text, letters, numbers, watermark, logo, flag, brand, neon cyberpunk, glossy sci-fi, cartoon, anime, photorealism, oversaturated colors, gore, distorted anatomy, duplicated objects`

---

## III. IMPLEMENTATION PHASES

### Phase A: Items Batch 1 — Ammo & Weapons (66 prompts, Day 1-2)
**70 assets**: deprecated ammo (19), military ammo boxes (16), pistols (8), SMGs (11), shotguns (4), rifles (8), PDWs (5), snipers (10), grenade (1), containers (6)

**Pipeline**:
1. Batch-submit all 70 prompts to Firefly with consistent seed ranges
2. Review each for style consistency against existing 249 items
3. Reject and re-generate any that break the flat-black-background rule
4. Accept → crop to exact sprite dimensions
5. Import into Unity as `Sprite (2D and UI)`
6. Assign to `ItemDefinition. icon` field

### Phase B: Items Batch 2 — Devices, Medical, Tools, Materials (65 prompts, Day 3-4)
**65 assets**: anti-rad, prewar letter, moss, radio, UV ballast, geothermal valve, RO membrane, acoustic decoy, logic board, CO2 scrubber, rebreather, water tablets, antiseptic, alcohol wipes, epi pen, decon soap, frostbite salve, scopolamine root, lithium salts, amnestic syrup, snow goggles, lead visor, ash ghillie, black ice, cobalt salt, black water, submerged server, master override, hard drive platter, photo album, vinyl collection, scope, sewing kit, hand crank sled, geiger tether, pneumatic jack, fungicide fogger, mine prod, headphones, epoxy injector, tether harness, nitroglycerin, salvaged tech, rope, copper wire, oat flour, contamination bags, cryo coolant, thermal paste, shoring timber, mycelium bricks, faraday mesh, sound baffling, tungsten core, pneumatic hose, galv rebar, welders glass, mirror shard, bio plastic, rubber gasket, concrete patch, insulation tape, engine block, bearing set, copper tubing

### Phase C: Items Batch 3 — Remaining (39 prompts, Day 5)
Containers with fill variants, specialty items from expansions.

### Phase D: Locations (42 prompts, Day 6-9)
**42 establishing shots, 16:9, hand-painted style.**

**Priority ordering** (most visually impactful first):
1. `location_silent_observatory` — mountaintop dome, frost-rimed telescope, dark sky
2. `location_the_sump_cathedral` — underground cistern lit by blue-green moss
3. `location_deep_core_borehole` — immense vertical shaft into darkness (endgame)
4. `location_the_dead_hand_core` — humming server core in red emergency light (endgame)
5. `location_the_memory_vault` — vast server-farm under dim standby lights (endgame)
6. `location_submerged_data_center` — flooded server racks, faint underwater glow
7. `location_magnetic_anomaly_crater` — debris hovering off ground, spinning compasses
8. `location_acoustic_testing_facility` — anechoic chamber, lone footprints in dust
9. `location_ash_dune_cemetery` — human shapes half-buried in grey ash drifts
10. `location_crashed_icebreaker_convoy` — derailed armored train, glowing reactor breach
11-42: Remaining locations in order from catalog

**Each location**: Generate → review composition → approve → import as 1920×1080 PNG → assign to LocationDefinitionSO

### Phase E: Survivor Portraits (96 prompts, Day 10-16)
**96 character portraits, 3:4, hand-painted style, chest-up three-quarter view.**

**Priority tiers**:
- **Tier 1 (Day 10-11)**: Starting survivors + named expansion survivors
  - `elena_vasquez` (see pilot_batch.md), `marcus_olejnik`, `suki_tanaka`
  - `the_surgeon`, `the_soldier`, `the_veteran`, `the_cop`
  - `aris_thorne`, `maya_lin`, `victor_vance`, `elena_rostov`
- **Tier 2 (Day 12-13)**: Core faction-tied survivors
  - `the_general`, `the_deserter`, `the_saboteur`, `the_defector`
  - `the_sheriff`, `the_politician`, `the_reporter`
- **Tier 3 (Day 14-16)**: Remaining 82 survivors

**Each portrait**: Generate → verify consistency with existing 12 generic portraits → crop to 768×1024 → import → assign to Survivor entry

### Phase F: Faction Lineups + Weather (20 prompts, Day 17-18)
**5 faction lineups** (3-4 figures each, 16:9):
1. Central Garrison Remnants — disciplined, faded olive, scratched insignia
2. Upland Provincial Militia — agrarian, layered hunting gear, farm tools
3. Cultists of the Glow — robed, rust-red symbols, radiation trefoils as icons
4. Scavenger Warlords — mismatched salvaged armor, improvised blades
5. Safe Haven Communities — civilian, no weapons, communal details

**15 weather effects** (16:9 or transparent overlay):
AcidSnow, BioFog, BlackSnow, BloodRain, EMPStorm, GlassStorm, RadHail, AlgaeBloom, AshLightning, ParticulateFog, ThermalInversion, IceStorm, Silence, FalseSpring, SilentSpring

---

## IV. UNITY INTEGRATION WORKFLOW

### For Items (170 assets)
```bash
# 1. Generate PNGs to Assets/_Game/Sprites/Items/
# 2. For each PNG, Unity auto-imports as Texture
# 3. Select all → Inspector:
#    - Texture Type: Sprite (2D and UI)
#    - Sprite Mode: Single
#    - Pixels Per Unit: 100
#    - Filter Mode: Bilinear
#    - Compression: High Quality
# 4. Assign sprite to ItemDefinition.icon via editor script
```

### For Locations (42 assets)
```bash
# 1. Generate PNGs to Assets/_Game/Sprites/Locations/
# 2. Select all → Inspector:
#    - Texture Type: Sprite (2D and UI)
#    - Pixels Per Unit: 100
#    - Max Size: 2048
# 3. Assign to LocationDefinitionSO via editor script
```

### For Survivors (96 assets)
```bash
# 1. Generate PNGs to Assets/_Game/Sprites/Portraits/
# 2. Select all → Inspector:
#    - Texture Type: Sprite (2D and UI)
#    - Pixels Per Unit: 100
#    - Max Size: 1024
# 3. Assign to SurvivorArchetypeSO / Survivor entry
```

### For Factions (5 assets)
```bash
# 1. Generate PNGs to Assets/_Game/Sprites/Factions/
# 2. Assign to FactionSO via editor
```

### For Weather (15 assets)
```bash
# 1. Generate PNGs to Assets/_Game/Sprites/Weather/
# 2. Either full-screen backgrounds or transparent overlays
# 3. Assign to Weather definition in WeatherSystem
```

---

## V. QUALITY CONTROL CHECKPOINTS

### Per-Batch Review Criteria
- [ ] Consistent flat black background on ALL items (no exceptions)
- [ ] Color palette matches existing assets (desaturated + amber highlights)
- [ ] No text, labels, brands, or readable marks
- [ ] Worn/weathered textures present (no "new" objects)
- [ ] Lighting direction consistent (top-left rim light)
- [ ] Resolution matches spec (1024×1024 for items, 1920×1080 for locations)
- [ ] No negative-prompt violations (no cartoon, anime, neon, etc.)

### Cross-Batch Consistency
- [ ] All 170 items look like they belong in the same game
- [ ] All 42 locations share the hand-painted charcoal/gouache texture
- [ ] All 96 portraits have consistent lighting and framing
- [ ] No style drift between early and late batches

---

## VI. ESTIMATED EFFORT

| Phase | Assets | Est. Time | Notes |
|-------|--------|-----------|-------|
| A: Items Batch 1 | 70 | 2 days | Ammo + weapons, highest volume |
| B: Items Batch 2 | 65 | 2 days | Devices, medical, tools |
| C: Items Batch 3 | 39 | 1 day | Containers, specialty |
| D: Locations | 42 | 4 days | Complex prompts, 16:9 compositions |
| E: Survivors | 96 | 7 days | Largest batch, requires consistency |
| F: Factions + Weather | 20 | 2 days | Complex compositions |
| Unity Import | 332 | 2 days | Sprite settings, SO assignments |
| QA + Rework | ~30 (10%) | 2 days | Rejects and re-generations |
| **TOTAL** | **~332** | **~22 days** | Full-time AI artist + Unity integrator |

---

## VII. FILE STRUCTURE

```
Assets/_Game/Sprites/
├── Items/           # 419 item icons (170 new)
│   ├── Ammo/
│   ├── Weapons/
│   ├── Medical/
│   ├── Devices/
│   ├── Tools/
│   ├── Materials/
│   └── Containers/
├── Locations/       # 47 establishing shots (42 new)
├── Portraits/       # 108 survivor portraits (96 new)
├── Factions/        # 5 faction lineups
├── Weather/         # 21 weather effects (15 new)
└── UI/              # UI-specific sprites (see Cursor plan)
```
