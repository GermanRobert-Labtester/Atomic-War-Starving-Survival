# Plan 20 — Wasteland Inhabitants: Implementation Summary

> **Status:** ✅ Complete
> **Completed:** 2026-09-01

## Scope

Plan 20 adds the wasteland inhabitants layer: a discoverable field guide, a network of 6 named settlements with 18 NPCs and 6 repeatable quests, and 28 route-aware travel encounters (24 standalone + 4 multi-stage chains of 12 stages).

## Data Files

| File | Contents |
|------|----------|
| `Assets/StreamingAssets/Data/field_guide.json` | 32 field guide entries (20 fauna, 12 flora/fungus), schema_version 1 |
| `Assets/StreamingAssets/Data/settlements.json` | 6 settlement definitions with route_node, trade themes, schema_version 1 |
| `Assets/StreamingAssets/Data/wasteland_settlement_npcs.json` | 18 named NPCs with 3-tier standing-reactive greetings |
| `Assets/StreamingAssets/Data/characters.json` | 18 NPCs appended (54 total characters) |
| `Assets/StreamingAssets/Data/repeatable_quests.json` | 6 repeatable side-work quests with cooldowns and standing rewards |
| `Assets/StreamingAssets/Data/travel_encounters.json` | 24 standalone encounters + 4 chains (12 stages), schema_version 1 |
| `Assets/StreamingAssets/Data/locations.json` | 6 settlement locations + location_quarry_overlook appended |

## Core Systems

| File | Purpose |
|------|---------|
| `Assets/Ashfall.Core/World/FieldGuideCatalog.cs` | Entry DTOs, unlock tracking, category/tag search, state capture/restore |
| `Assets/Ashfall.Core/World/SettlementCatalog.cs` | Settlement/NPC DTOs, standing-reactive greetings, quest cooldown/completion, state |
| `Assets/Ashfall.Core/Narrative/TravelEncounterCatalog.cs` | Encounter/choice DTOs, catalog loader |
| `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs` | Stance weighting, region/season/chain filters, deterministic selection, chain progression, state |

## Host CLI

- `--wasteland-inhabitants-selftest` / `--plan20-selftest` / `--inhabitants-selftest`
- Runner: `src/Host/HostCli.WastelandInhabitants.cs`
- Dispatch: `src/Main.Application.cs`
- Documented in: `src/Host/HostCli.cs` → `PrintHelp()`

## Tests

- `Ashfall.Core.Tests/World/Plan20WastelandInhabitantsTests.cs` — 8 test methods
- All passing (5443/5443 non-performance tests)

## Catalog Integrity Fixes (in this PR)

- Added `field_fauna_`, `field_flora_`, `field_guide_`, `char_`, `creature_` to IdPrefixes (both Validator + Rules)
- Added `choice_id`, `chain_id` to DefinitionKeys
- Added `settlement_wall`, `settlement_center` to KnownRuntimeIds (environmental text pseudo-locations)
- Added `location_quarry_overlook` to `locations.json`
- Added `loc_grain_exchange`, `loc_automated_abattoir`, `loc_flooded_subway_depot`, `loc_scavenger_camp`, `loc_iron_garrison` to `locations.json` (pre-existing codex unlock_refs)
- Fixed `prereq_chain_stage` in pilgrim chain stages 2+3 (should equal their own `chain_stage`, not previous stage)

## Documentation

- `docs/world/field-guide-overview.md`
- `docs/world/settlements-overview.md`
- `docs/world/travel-encounters-overview.md`
- `docs/world/plan20-implementation-summary.md` (this file)
