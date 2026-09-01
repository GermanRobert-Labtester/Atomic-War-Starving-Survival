# Plan 29 — Shelter Room Inventory (Phase 0, §3.2)

> Verified against source on 2026-09-01. **No room name/lore was authored from
> assumptions** — every row cites its runtime or data source.

---

## 1. The four runtime room rosters (fragmented)

### 1.1 StartingLevelSystem — Day-1 Holdfast roster (`StartingLevelSystem.InitializeDefaultHoldfast`)

| Room ID | Display name | Material | Attenuation | Inspected at start |
|---|---|---|---|---|
| `room_bunker_corridor` | Central Access Corridor | Concrete | 0.80 | yes |
| `room_filtration_stack` | Air Filtration & HEPA Bay | Lead | 0.99 | no |
| `room_storage_bay` | Ration & Supply Locker | Concrete | 0.80 | yes |
| `room_bunks_living` | Survivor Bunk Quarters | Wood | 0.10 (→ Lead 0.99 via `FortifyBunksLead` directive) | no |
| `room_radio_tuner` | 142.850 MHz Tuner Station | Concrete | 0.80 | no |

Owner: `StartingLevelSaveState.rooms` (`ShelterRoomState`: roomId, displayName, material,
attenuation, isInspected). Saved in the starting-level section.

### 1.2 ShelterAssignmentHostSession — work/assignment roster (`src/Host/ShelterAssignmentHostSession.cs:28`)

| Room ID | Display name | Capacity | Skill gate |
|---|---|---|---|
| `room_bunker_corridor` | Central Access Corridor | 0 | — |
| `room_bunks` | Bunks | 4 | — |
| `room_kitchen` | Kitchen | 2 | `skill_cooking` |
| `room_clinic` | Clinic | 2 | `skill_medic` |
| `room_workshop` | Workshop | 2 | `skill_crafting` |
| `room_filtration` | Filtration Stack | 1 | `skill_technician` |

### 1.3 HoldfastInteriorView — visual/spatial roster (`src/World/HoldfastInteriorView.cs:92-99`)

The de-facto full shelter map (8 rooms, with hotspot positions):

| Room ID | Display name | Position | Status line |
|---|---|---|---|
| `room_storage_bay` | Storage Bay | (160,290) | "Tool & Supply Depot" |
| `room_workshop` | Workshop | (250,290) | "Fabrication Bench Ready" |
| `room_bunker_corridor` | Central Corridor | (340,290) | "Access Concourse" |
| `room_kitchen` | Galley Kitchen | (420,290) | "Ration Prep Operational" |
| `room_bunks` | Bunk Living | (510,290) | "Living Quarters: Warmth 100%" |
| `room_filtration` | Filtration Stack | (680,290) | "Filtration Stack: Active · Attenuation: 99%" |
| `room_clinic` | Medical Ward | (595,290) | "Triage Bay Ready" |
| `room_airlock` | Airlock Hatch | (860,290) | "Airlock: Sealed · Outer Decon Ready" |

Fallback room: `room_bunks` (`DefaultFallbackRoomId`).

### 1.4 PowerGrid data authority — electrical rooms (`power_grid.json`)

| Room ID | Display name | Draw | Priority | Failure effect |
|---|---|---|---|---|
| `room_air_filtration` | Air Filtration | 180 W | critical | `fx_filtration_off` |
| `room_clinic` | Clinic | 120 W | critical | `fx_clinic_off` |
| `room_water_pump` | Water Pump | 100 W | critical | `fx_water_pressure_drop` |
| `room_greenhouse` | Greenhouse | 160 W | standard | `fx_grow_lights_off` |
| `room_foundry` | Silent Foundry | 220 W | low | `fx_foundry_standstill` |
| `room_lighting_main` | Main Lighting | 80 W | low | (none) |

⚠ **Drift:** `PowerGridHostSession.DefaultGrid()` hard-codes only 4 of these 6
(missing `room_water_pump`, `room_lighting_main`) with a comment claiming it matches
the catalog. Also `ShelterScheduleHostSession` uses a divergent `room_main` "Main Vault".

### 1.5 ID drift summary (must be resolved before Plan 29A)

| Concept | IDs in circulation |
|---|---|
| Filtration space | `room_filtration_stack` (1.1) · `room_filtration` (1.2, 1.3) · `room_air_filtration` (1.4) |
| Bunks | `room_bunks_living` (1.1) · `room_bunks` (1.2, 1.3) |
| Home location | `loc_bunker_holdfast` (StartingLevelSystem const) · `loc_holdfast` (locations.json:992) |
| Corridor | `room_bunker_corridor` (1.1, 1.2, 1.3) — consistent |
| Clinic | `room_clinic` (1.2, 1.3, 1.4) — consistent |

## 2. Non-runtime room vocabularies (narrative)

- **`bunker_blueprints_codex.json`** — 24 `room_bp_*` ids describing a large pre-war
  facility: Sub-Levels 1–4, Residential Blocks A/B, hospital bus, named staff (Chief
  Engineer Dmitri, Valery, Fyodor the Stoker, Dr. Vel, Master Oleg, Sonya). This is the
  richest origin substrate but its room set **does not match** the runtime ~8-room
  shelter. Continuity decision required in Task 29A (`BUNKER_ORIGIN_CONTINUITY.md`):
  recommended reading — the blueprint codex is the Holdfast's *original design
  documentation* for a facility of which the current playable shelter is the partly
  collapsed/habitable core. That reconciles both without rewriting either.
- **`duty_roster_locations.json`** — "the_stack" region (The Chart, Inner Airlock…):
  a separate narrative location space used by DutyRoster flavor, not runtime rooms.
- **`excavation_sites.json`** — external world sites (Collapsed Command Vault, Utility
  Tunnel Network…). **Excavation is not shelter room construction**; the
  "constructed/expanded room" class has no runtime producer today.
- **Graffiti/glitch free-text locations** ("Boiler Room Corridor (-10m)",
  "Sub-Level 2 Heating Manifold") — era/mood texture, not bound to room ids.

## 3. Plan 29A.1 classification of the canonical shelter

Based on §1.3 (the only complete spatial map) reconciled with §1.1/1.2/1.4:

| Room | Class (29A.1) | Systems consuming it | Condition owner |
|---|---|---|---|
| `room_bunker_corridor` | Starting major (transit spine) | Assignment (cap 0), Interior, StartingLevel | none (corridor surfaces unmodeled) |
| `room_storage_bay` | Starting major | StartingLevel (inspect), Interior | none (fixtures unmodeled) |
| `room_bunks` / `room_bunks_living` | Starting major | Assignment (cap 4), Interior, Thermal (warmth→Needs), Decor (Plan 12C slots) | Thermal room node (temp/frozen); shielding via StartingLevel/material |
| `room_kitchen` | Starting major | Assignment (`skill_cooking`), Interior, Ventilation valve `kitchen`, KitchenNutritionSystem | Ventilation (smoke/CO); KitchenNutrition |
| `room_clinic` | Starting major | Assignment (`skill_medic`), Interior, PowerGrid (critical), MedicalWard, Ventilation valve `medical` | PowerGrid (breaker); medical systems |
| `room_workshop` | Starting major | Assignment (`skill_crafting`), Interior, Crafting | EquipmentConditionSystem (gear) |
| `room_filtration` / `room_filtration_stack` | Starting major (technical) | Assignment (`skill_technician`), Interior, StartingLevel (HEPA), PowerGrid `room_air_filtration`, Ventilation | StartingLevel (airFilterHealth), Ventilation (saturation, duct) |
| `room_airlock` | Starting major (perimeter) | Interior, AirlockSecuritySystem, DecontaminationSystem | AirlockSecurity (door state/incidents) |
| `room_radio_tuner` | Starting support | StartingLevel (inspect) | none (radio systems elsewhere) |
| `room_foundry` | Constructed/expanded (only via narrative; no runtime construction producer) | PowerGrid (220 W), SilentFoundrySystem, Ventilation valve `foundry`, Thermal aux heat | SilentFoundry (5 facility components) |
| `room_greenhouse` | Constructed/expanded (same caveat) | PowerGrid (160 W), GreenhouseSystem | GreenhouseSystem (no condition field) |
| `room_water_pump` | Non-player-facing technical (electrical only) | PowerGrid (100 W critical), WaterTreatment (by effect) | WaterTreatment (filters); pump itself unmodeled |
| `room_lighting_main` | Non-player-facing technical | PowerGrid (80 W) | PowerGrid |

## 4. Fixture/lore surfaces that already exist

- `StartingLevelSystem.InspectRoom` — first-inspection discovery pattern (per-room
  `isInspected` flag, directive log) — reusable 29A.9 unlock path.
- `HoldfastFlavorCatalog` (`holdfast_flavor.json`) — item marginalia + faction voice
  overlay; the natural precedent for a room-identity overlay (data-driven, keyed by
  canonical ids, neutral fallback).
- `ShelterDecorSystem` slots (`roomId`, `slotId` → `item_decor_*`) — Plan 12C fixture
  anchors already room-bound.

## 5. Roster decision required before Phase 1 (gate)

Freeze the canonical Plan 29 room roster on the **HoldfastInteriorView 8-room set**
(`room_storage_bay`, `room_workshop`, `room_bunker_corridor`, `room_kitchen`,
`room_bunks`, `room_clinic`, `room_filtration`, `room_airlock`) plus
`room_radio_tuner` (StartingLevel) and the two service spaces (`room_foundry`,
`room_greenhouse` — player-facing when active). Introduce an **alias map**
(`room_filtration_stack`→`room_filtration`, `room_bunks_living`→`room_bunks`,
`room_air_filtration`→`room_filtration`) so identity records bind one canonical id while
legacy save states keep loading. Room identity records must NOT rename runtime ids —
they add identity data beside existing rosters (Plan 29 §1.3, §5.1).
