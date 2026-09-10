# Plan 145 — Bunker Graffiti & Wall-Text Runtime Activation and Location Projection — Completion Report

**Date:** 2026-09-09
**Status:** COMPLETE & FULLY VERIFIED
**Authoritative Engine:** Godot 4.7+ (.NET / C#)
**Core Authority:** `Assets/Ashfall.Core/` (zero engine references)

---

## 1. Executive Summary

Plan 145 activated ASHFALL's authored graffiti and wall-text corpus as a real, location-aware ambient storytelling layer using `BunkerGraffitiCatalog`, without confusing graffiti with player memorial carvings (`MemorialSystem`), moral-choice gossip (`MoralChoiceGossipRuntime`), location sensory descriptions (`LocationMemorySystem`), or mutable world save state.

All 76 authored postings (36 canonical base from `narrative/bunker_graffiti_postings.json` and 40 expansion from `narrative/graffiti_expansion.json`) are loaded, validated, time-gated via Model A (`recorded_day <= currentDay`), mapped deterministically to canonical rooms and wasteland locations via `BunkerGraffitiProjection`, and surfaced in production UI across:
1. **Holdfast Interior View** (`HoldfastInteriorView.cs`): Ambient wall-text snippet displayed in room hover/status summaries.
2. **Shelter Panel** (`ShelterPanel.cs`): Dedicated `"WALL MARKINGS // {ROOM}"` section in the Shelter Overview, reactive to room hotspot clicks.
3. **Map Detail Panel** (`MapDetailPanel.cs`): Dedicated `"FIELD MARKINGS & ENVIRONMENTAL TEXT"` card in sector intelligence for explored wasteland sectors.

All 5 project verification gates run clean (10,229 xUnit tests pass, 582 catalogs scanned with 0 orphans and CI gate PASS, 0 data integrity findings across 299 catalogs, 25/25 scene bindings pass, 30/30 scenes lint clean).

---

## 2. Corpus Inventory & Reconciliation

### 2.1 Sources
- **Canonical Base:** `Assets/StreamingAssets/Data/narrative/bunker_graffiti_postings.json` — 36 postings (`graf_bunker_001` to `graf_bunker_036`), recorded days 3 to 3650.
- **Expansion Corpus:** `Assets/StreamingAssets/Data/narrative/graffiti_expansion.json` — 40 postings (`graf_exp_001` to `graf_exp_040`), recorded days 1 to 100.
- **Root Friction File (Isolated):** `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` (10 items, schema `id`, `title`, `text`, `triggerWorldFlag`). Kept completely separate; owned by `Plan12BFrictionTests`.

### 2.2 Deduplication & Integrity
- **Total Unique Postings:** 76.
- **ID Collisions:** 0 across all sources.
- **Case Collisions:** 0 across all sources.
- **Duplicate Content:** 0 across all sources.
- **Author Signatures:** Plain descriptive strings (e.g. `the teacher`, `Boris the Baker`, `Sonya, Council President`). Completely decoupled from live survivor entities.
- **Morale Effect:** Descriptive authoring tone only. Confirmed 0 mutations to survivor morale, flags, or game state upon viewing.

---

## 3. Core Implementation

### 3.1 `BunkerGraffitiProjection.cs` (`Assets/Ashfall.Core/Narrative/BunkerGraffitiProjection.cs`)
- Deterministic 1:1 mapping dictionary from all 76 authored `location` strings to canonical target IDs:
  - Shelter rooms: `room_filtration`, `room_kitchen`, `room_clinic`, `room_workshop`, `room_bunks`, `room_storage_bay`, `room_radio_tuner`, `room_foundry`, `room_greenhouse`, `room_main`, `room_airlock`, `room_water_pump`.
  - Wasteland sectors: `suburban_house`, `loc_collapsed_overpass`, `location_submerged_data_center`, `collapsed_supermarket`, `loc_freight_terminal`, `loc_relay_tower`, etc.
- Robust keyword-based fallback heuristics for room and location matching when location strings vary.

### 3.2 `BunkerGraffitiCatalog.cs` (`Assets/Ashfall.Core/Narrative/BunkerGraffitiCatalog.cs`)
- **Idempotency:** Load operations check `_byId.ContainsKey(entry.posting_id)`. Loading multiple times or calling `LoadFromDirectory` does not duplicate postings.
- **Validation:** Filters out null/empty IDs, whitespace-only content, and negative recorded days.
- **LoadFromDirectory:** Loads both `narrative/bunker_graffiti_postings.json` and `narrative/graffiti_expansion.json`.
- **Query APIs:**
  - `GetById(string postingId)`
  - `GetUnlockedByDay(int currentDay)`
  - `GetByCategory(string category, int currentDay)`
  - `GetPostingsForTarget(string targetId, int currentDay)`
  - `GetPostingsForRoom(string roomId, int currentDay)`
  - `GetPostingsForLocation(string locationId, int currentDay)`
- **Sorting:** Deterministic ordering by `(recorded_day, posting_id)`.

### 3.3 `ContentUtilizationScanner.cs` (`Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`)
- Added `IsPlan145GraffitiFile` identifying `narrative/bunker_graffiti_postings.json` and `narrative/graffiti_expansion.json`.
- Mapped loader to `BunkerGraffitiCatalog`.
- Mapped registry to `BunkerGraffitiCatalog`.
- Mapped runtime consumers to `ShelterPanel`, `HoldfastInteriorView`, and `MapDetailPanel`.
- Promoted both catalogs from `CODEX_ONLY` to `GAMEPLAY_CONSUMED`.

---

## 4. Host & UI Surface Integration

### 4.1 `HoldfastInteriorView.cs` (`src/World/HoldfastInteriorView.cs`)
- Added `SetGraffitiCatalog(BunkerGraffitiCatalog? catalog, int currentDay = int.MaxValue)`.
- Updated `AppendRoomIdentity(string status, string roomId)` to append a non-intrusive ambient snippet:
  `Wall Text: "{snippet}"` for unlocked postings in that room.

### 4.2 `ShelterPanel.cs` (`src/UI/ShelterPanel.cs`)
- Added `SetGraffitiCatalog(BunkerGraffitiCatalog? catalog, int currentDay = int.MaxValue)` and updated `Bind(...)` with optional parameters.
- Connected `_interiorView.RoomSelected` to update `_selectedRoomId` and refresh the view.
- In `RefreshView()`, added `"WALL MARKINGS // {ROOM}"` section in the Shelter Overview showing up to 2 unlocked postings with medium tags, content, and author signatures.

### 4.3 `MapDetailPanel.cs` (`src/UI/MapDetailPanel.cs`)
- Added optional `BunkerGraffitiCatalog? graffitiCatalog` and `int currentDay` to `Bind(...)`.
- Renders a `"FIELD MARKINGS & ENVIRONMENTAL TEXT"` card with medium tags, content quotes, author signatures, and recorded day metadata.

### 4.4 Host Wiring (`src/Main.*.cs`)
- `Main.ShelterInfrastructure.cs`: Added `GetBunkerGraffitiCatalog()` lazy loader.
- `Main.UiHandlers.cs`: `OpenMapDetailPanel` passes `GetBunkerGraffitiCatalog()` and current day.
- `Main.GameFlow.cs`: Shelter panel open action passes `GetBunkerGraffitiCatalog()` and current day.
- `Main.PlayerSurfaces.cs`: Shelter panel `bindAction` passes `GetBunkerGraffitiCatalog()` and current day.

---

## 5. Verification Matrix Evidence

All five verification commands required by project rules (plus UI tests) were executed and exited 0:

| Verification Target | Command | Result | Details |
|---|---|---|---|
| **Core Unit Tests** | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS (Exit 0)** | 10,229 passed, 0 failed, 0 skipped (Duration: 42s). Includes 10/10 `BunkerGraffitiCatalogTests`. |
| **Godot Host Build** | `dotnet build Ashfall.csproj` | **PASS (Exit 0)** | 0 warnings, 0 errors. Assembly built cleanly. |
| **Content Utilization Gate** | `godot --headless --path . -- --content-utilization-selftest` | **PASS (Exit 0)** | 582 catalogs scanned, 221 gameplay-consumed, 0 orphaned. CI Content Utilization Gate: PASS. |
| **Data Integrity Gate** | `godot --headless --path . -- --data-integrity-selftest` | **PASS (Exit 0)** | 0 errors, 0 warnings across 299 catalogs. |
| **Scene Binding Gate** | `godot --headless --path . -- --scene-binding-selftest` | **PASS (Exit 0)** | 25 passed, 0 failed (of 25). |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | **PASS (Exit 0)** | 30 production scenes checked; 0 errors, 0 warnings. |
| **Player Panels UI Test** | `godot --headless --path . -- --player-panels-uitest` | **PASS (Exit 0)** | 16/16 lifecycle gates passed cleanly including Shelter panel. |

---

## 6. Architecture & System Invariants Adherence

1. **Zero Engine Coupling in Core:** `Assets/Ashfall.Core/Narrative/BunkerGraffitiCatalog.cs` and `BunkerGraffitiProjection.cs` contain zero references to `Godot`, `UnityEngine`, or `JsonUtility`.
2. **Authority Separation:** Complete isolation preserved across:
   - Graffiti / Wall-Text: authored, immutable ambient lore via `BunkerGraffitiCatalog`.
   - Memorial Carvings: player-placed memorials via `MemorialSystem` and `ShelterDecorPanel`.
   - Moral Gossip: reactive emergent chatter via `MoralChoiceGossipRuntime`.
   - Location Memory: tactical observations via `LocationMemorySystem`.
3. **State Neutrality:** Zero save state created or modified. No mutations to survivor health, morale, hunger, or world flags upon viewing graffiti.
4. **Determinism:** Deterministic sorting by `(recorded_day, posting_id)` and pure projection lookups guarantee identical ordering across runs.
