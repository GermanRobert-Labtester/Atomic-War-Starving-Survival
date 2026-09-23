# Batch 7 — Unblock Oldest Partial Plans: Plans 59 + 134

**Package:** `UNBLOCK-OLDEST-BATCH7-PLANS`
**Claim:** `claim-unblock-oldest-batch7-plans-2026-09-23`
**Date:** 2026-09-23
**Status:** COMPLETE (see ledger entry in `INTEGRATION_PLANS.md`)
**Predecessor:** `UNBLOCK-OLDEST-BATCH6-PLANS` (Plans 135 + 59), which completed Plan 135 and established governance host integration for Plan 59.

---

## 1. Scope and Selection

The user requested integration of the next batch of partial plans:
1. **Plan 59:** `Retrospective: Standing Gates / Rules` (confirmed Batch 6 complete: `StandingGatesHostSession.cs`, `HostCli.StandingGates.cs`, `--standing-gates-selftest`, `Plan59StandingGateHostIntegrationTests.cs` 21/21 PASS).
2. **Plan 134:** `Dynamic Faction Territory & Supply Lines` (host refs: 0 -> full host integration).

---

## 2. Premise Audit — Current Evidence

| Claim in the plan | Current evidence | Verdict |
|---|---|---|
| `faction_territory.json` | Exists in `Assets/StreamingAssets/Data/faction_territory.json`, 19 authored territories with nodes, control points, base control strength, and contested factions. | Valid authored data authority |
| `supply_lines.json` | Exists in `Assets/StreamingAssets/Data/supply_lines.json`, 5 authored supply corridors linking origins to destinations. | Valid authored data authority |
| `TerritoryControlSystem.cs` | Exists in `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`. Pure domain logic for node fortification, garrison management, deterministic contest resolution, supply line raids and restorations, and daily deliveries. | Core authority live |
| `LocationTerritorySaveState`, `SupplyLineSaveState`, `TerritoryControlSaveState` | Added to `TerritoryControlSystem.cs` with `schema_version = 1`, deterministic sorted capture, and safe handling of phantom entries. | Sealed in Core |
| Host Session & Save Store | `TerritoryControlHostSession.cs` and `TerritoryControlSaveStore` created in `src/Host/`. Section `territory_control`, filename `territory_control_save.json`. | Integrated |
| Host CLI Probe | `--territory-control-selftest` / `--territory-selftest` in `HostCliRegistry.cs`, implemented in `HostCli.TerritoryControl.cs`. | Integrated |
| Campaign Day Lifecycle | `TerritoryControlDayOwner` registered in phase 5 in `Main.CampaignOwners.cs`, wired to `Main.ExpandedShelterSystems.cs` and `Main.SaveOrchestrator.cs`. | Integrated |

---

## 3. Architecture & Boundary Rules

1. **One Authority per Concern (Rule 5):** `TerritoryControlSystem` is the single authority for dynamic location control, fortification levels, garrison strength, and supply line statuses. Neither Faction War nor Faction Ecology maintains shadow copies of territory ownership.
2. **Core Stays Engine-Free (Rule 2):** `TerritoryControlSystem.cs` has zero references to Godot, Unity, or engine APIs.
3. **Deterministic Persistence (Rule 4):** Save state carries `schema_version = 1`. Capture orders locations and supply lines ordinally by identifier. Day advance draws deterministically from `_campaignDay.Rng`.
4. **SaveSectionRegistry Pin:** Registry pin incremented from 223 to 224, verified by `ComprehensiveSaveStoreCorruptionAndMigrationTests`.
5. **Event Vocabulary:** Internal heartbeat event `territory_control_ticked` added to `DayEventVocabulary` as `SemanticKind.Heartbeat` and documented in `EVENT_SEMANTIC_PARITY_MATRIX.md`.

---

## 4. Verification Evidence

- `Plan134TerritoryControlHostIntegrationTests.cs`: 6/6 PASS
- `Plan134TerritoryControlIntegrationTests.cs`: 5/5 PASS
- `Plan59StandingGateHostIntegrationTests.cs`: 21/21 PASS
- `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`: 1346/1346 PASS
- `generate-architecture-map.py`: 224 subsystems mapped with 100% mechanical evidence.
