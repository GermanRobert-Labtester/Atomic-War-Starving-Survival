# PLAN 44 — FACTION TERRITORY MAP CLOSEOUT

---

## 1. Executive Summary

**Plan 44 — Faction Territory Map** establishes the authoritative territorial geography of ASHFALL. It externalizes the territorial footprints, physical anchor locations, and contested flashpoints across all 19 canonical wasteland factions without introducing uncommitted future node IDs or faking runtime mechanics.

- **Completion Mode:** `COMPLETE — data authority & core loader validated`
- **Catalog File:** `Assets/StreamingAssets/Data/faction_territory.json`
- **Engine Representation:** `Ashfall.Core.World.FactionTerritoryCatalog`
- **Prefix Namespaces Registered:** `territory_` in `CatalogIntegrityValidator.cs` and `CatalogIntegrityRules.cs`.

---

## 2. Key Accomplishments

1. **19 Faction Territory Definitions:**
   - Every active faction in `currents.json` and `holdfast_factions.json` has a dedicated territory definition in `faction_territory.json`.
   - Classification honors the lore: 11 Territorial, 5 Nomadic, 2 Ideological, 1 Mixed.
   - Scale accurately calibrated: 4 Major, 5 Medium, 8 Minor, 2 None.

2. **Strict Node & Location Referential Integrity:**
   - All `controlled_nodes` references map exclusively to committed nodes in `wasteland_map_v1.json`.
   - All `control_points` references resolve to validated physical locations in `locations.json` (including Plan 43 settlement anchors).
   - Zero uncommitted Plan 16A future IDs were introduced into production JSON.

3. **5 Contested Flashpoint Zones:**
   - `zone_contested_water_rights` (Estuary Water Manifold — Hydro-Barons vs Undertow vs Cutters)
   - `zone_contested_cut_salvage` (The Cut Salvage Corridor — Scavenger Guild vs Iron Raiders vs Deserters)
   - `zone_contested_merchant_crossroads` (Caravanserai Barter — The Office vs The Tally vs Scavenger Guild)
   - `zone_contested_scarp_pass` (High Scarp Geothermal Pass — Long Walk vs Cold Count vs Quiet House)
   - `zone_contested_coastal_bluff` (Coastal Shelf Saline Demarcation — Black Flotilla vs The Fleet vs Cutters)

4. **Engine-Agnostic Core Catalog & Tests:**
   - Authored `FactionTerritoryCatalog` in `Assets/Ashfall.Core/World/`.
   - Authored 7 thorough unit tests in `Ashfall.Core.Tests/World/FactionTerritoryCatalogTests.cs` verifying deserialization, lookup indexes, range constraints, and referential validity.

---

## 3. Verification Matrix

| Gate | Target | Result |
|---|---|---|
| `dotnet test Ashfall.Core.Tests` | 0 failures | **PASS (5,973 tests passing)** |
| `godot --headless --path . -- --data-integrity-selftest` | 0 errors / 0 warnings | **PASS (0 findings across 172 catalogs)** |
| `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS | **PASS (0 orphaned catalogs)** |
| `godot --headless --path . -- --scene-binding-selftest` | 22/22 passed | **PASS** |
| `python3 scripts/ci/scene-lint.py` | 0 errors | **PASS** |

---

## 4. Status Declaration

```text
Territory catalog authored and validated.
Runtime gameplay consumption (tax levying, travel hazard modification, dynamic decay)
is cleanly decoupled and ready for Plan 45 (Patrols) and Plan 16B (Caravans).
```
