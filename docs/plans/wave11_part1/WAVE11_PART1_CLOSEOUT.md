# WAVE 11 PART 1 CLOSEOUT

## Wave 11 Part 1 — Execution Summary

**Period:** 2026-09-18
**Claim:** `claim-wave11-part1-execution-2026-09-17` (see `WORKTREE_OWNERSHIP.md`)

---

## Task Terminal States

| Task | Census Row | Plan | Terminal State | Tests |
|---|---|---|---|---|
| A1 | C1[11] | Plan 38 — The Year Turns | **SEALED** | 19/19 |
| A2 | C1[12] | Plan 41 — Memory That Acts | **SEALED** | 33/33 |
| A3 | C1[13] | Plan 43 — Governing Together | **SEALED** | 48/48 |
| A4 | C1[14] | Plan 45 — The Content Acceptance Pipeline | **SEALED** | 5/5 |
| A5 | C1[15] | Plan 47 — The Mod & Content-Pack Contract | **SEALED** | 44/44 (38 new) |
| B1 | C2[10] | Plan 30 — Autonomous Outside World | **PARTIALLY-SEALED** (DECISION-BLOCKED) | WastelandMap 68/68 baseline |
| B2 | C2[11] | Plan 32 — One Place Authority, Graph-Native Travel | **PARTIALLY-SEALED** (consolidation promoted) | WastelandMap 68/68 |

---

## Test Evidence

| Filter | Result |
|---|---|
| `~Mods` (all mod tests: Plan47 + existing JsonModLayering) | **44/44 PASS** |
| `~WastelandMap` | **68/68 PASS** |
| `~Calendar\|~Commitment\|~Policy\|~Memorial\|~ContentAcceptance` | All green (confirmed per per-task logs) |

---

## A5 — Plan 47 Implementation (New Code)

| File | Type | Summary |
|---|---|---|
| `Assets/Ashfall.Core/Mods/ModCompatibilityEvaluator.cs` | **New** | Pure Core game-version + mod-contract range evaluator |
| `Assets/Ashfall.Core/Mods/JsonModLayering.cs` | **Extended** | `ModRejectionCode` enum; typed `ModDiagnostic.Code`; manifest fields `game_range`, `mod_contract_range`, `pack_type`, `dependencies`; topological dep sort; acceptance pipeline seam |
| `Ashfall.Core.Tests/Mods/Plan47ModContractTests.cs` | **New** | 38 tests: compatibility governance, dependency resolution, typed rejection, content-pack acceptance, modded replay |
| `docs/mods/MOD_CONTRACT.md` | **Updated** | Full contract: schema table, field aliases, version range syntax, stability classes, typed rejection table, acceptance pipeline section |

---

## B1 — Plan 30 Findings (No New Code)

- **STALE-PREMISE correction:** Prior `SEALED-ELSEWHERE` census label cited wrong Plan 30 document (spiritual/faith axis, not autonomous-world axis)
- **Current-source baseline:** `SimulateDailyFriction` + `FactionWarChainRunner.TickDay` already wired in `YearOfAshHostSession.TickDay`
- **Remaining gap:** Five of six projection events have zero `src/` subscribers; `OnFactionStandingChanged` refreshes the faction-war map. Runtime-clock design decision required; 30B broad consequence reach and 30C caravan/wildlife autonomy remain unimplemented
- **Decision block:** Foreman must decide runtime clock (180–360 cap vs. uncapped simDay) before 30B work begins
- **Promoted:** Next bounded package should wire at minimum one consequence reach path (radio/market) as proof-of-concept after runtime clock decision

## B2 — Plan 32 Findings (No New Code, Data Gap Found)

- **Satisfied at HEAD:** `loc_*` node identity unified; all 68 routes have `distanceKm`; save persistence (68/68 map tests green)
- **Promoted consolidation proposals:**
  - **32A data hygiene:** 10 orphan map nodes need `locations.json` stubs
  - **32B graph-native travel:** Expedition/caravan system wiring to `WastelandMapSystem` pathfinder (full-scope migration; requires foreman decision on blast radius)
  - **32C geographic knowledge:** Discovery gating for expedition dispatch

---

## KNOWN_DEBT Additions

Per audit, the following items should be recorded in `KNOWN_DEBT.md` by the foreman/integrator:

1. **C2[10] runtime clock decision** — War chains fire in 180–360 window only; extended-play campaigns cannot surface war narrative without a clock choice. DECISION-BLOCKED.
2. **C2[10] consequence reach (30B)** — Five faction-war projection events have zero subscribers; standing changes already refresh the faction-war map, but clashes/decrees/chain stages lack the required broader consequence reach. ARCHITECTURE-GAP.
3. **C2[11] 10 orphan map nodes** — `loc_electrical_maintenance_exchange`, `loc_municipal_seed_vault`, `loc_quarantine_barn`, `loc_underground_fuel_depot`, `loc_broadcast_bunker_echo`, `loc_blacksite_armory_7`, `loc_materials_research_sublevel`, `loc_forestry_emergency_store`, `loc_evidence_sub_basement`, `loc_sealed_triage_annex` appear in `wasteland_map_v1.json` but not in `locations.json`. DATA-GAP.
4. **C2[11] graph-native travel (32B)** — Expedition/caravan never query `WastelandMapSystem`. Travel cost/distance/risk is not graph-derived. ARCHITECTURE-GAP; requires foreman decision.

---

## Files Modified (Full Wave 11 Part 1)

| File | Task | Action |
|---|---|---|
| `Assets/Ashfall.Core/Campaign/CampaignCalendarReadModel.cs` | A1 | New |
| `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` | A1 | Extended |
| `Assets/Ashfall.Core/Commitment/CommitmentSystem.cs` | A1 | New |
| `Assets/StreamingAssets/Data/commitments.json` | A1 | New |
| `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` | A2 | Extended |
| `Assets/Ashfall.Core/Survivors/CohortSystem.cs` | A2 | Extended |
| `Assets/Ashfall.Core/World/LocationMemorySystem.cs` | A2 | Extended |
| `Assets/Ashfall.Core/Policy/PolicySystem.cs` | A3 | New |
| `Assets/Ashfall.Core/Policy/CrewConsentVerdict.cs` | A3 | New |
| `Assets/StreamingAssets/Data/policies.json` | A3 | New |
| `Assets/Ashfall.Core/Content/ContentAcceptancePipeline.cs` | A4 | New |
| `scripts/ci/content-acceptance-gate.sh` | A4 | New |
| `docs/systems/CONTENT_ACCEPTANCE_PIPELINE.md` | A4 | New |
| `docs/ci/CI_GATE_MANIFEST.json` | A4 | Extended |
| `Assets/Ashfall.Core/Mods/ModCompatibilityEvaluator.cs` | A5 | New |
| `Assets/Ashfall.Core/Mods/JsonModLayering.cs` | A5 | Extended |
| `Ashfall.Core.Tests/Mods/Plan47ModContractTests.cs` | A5 | New |
| `docs/mods/MOD_CONTRACT.md` | A5 | Updated |
| `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` | A1-B2 | Updated (C1[11]-C1[15], C2[10]-C2[11]) |
| `docs/plans/wave11_part1/A1_PLAN38_IMPLEMENTATION_LOG.md` | A1 | New |
| `docs/plans/wave11_part1/A2_PLAN41_IMPLEMENTATION_LOG.md` | A2 | New |
| `docs/plans/wave11_part1/A3_PLAN43_IMPLEMENTATION_LOG.md` | A3 | New |
| `docs/plans/wave11_part1/A4_PLAN45_IMPLEMENTATION_LOG.md` | A4 | New |
| `docs/plans/wave11_part1/A5_PLAN47_IMPLEMENTATION_LOG.md` | A5 | New |
| `docs/plans/wave11_part1/B1_PLAN30_IMPLEMENTATION_LOG.md` | B1 | New |
| `docs/plans/wave11_part1/B2_PLAN32_IMPLEMENTATION_LOG.md` | B2 | New |
| `docs/data/CATALOG_REGISTRY.md` | A4/A5 | Re-generated |

---

## Open Decision Blocks (Require Foreman Signature)

1. **C2[10] runtime clock:** Should war chains fire within the 180–360 campaign window or uncapped past day 360? This determines which war-narrative content is reachable. Current state: frozen at `Timeline.EndDay=360`.
2. **C2[11] 32B scope:** What is the bounded 32B migration plan for graph-native expedition travel? Blast radius includes: ExpeditionSystem, AviationSystem, NavalSystem, CaravanSystem, and all route/save paths.

---

## Claim Close

The Wave 11 Part 1 worktree claim (`claim-wave11-part1-execution-2026-09-17`) may be closed after:
1. Foreman review of this closeout
2. KNOWN_DEBT entries added by integrator for C2[10] and C2[11] remainder
3. Plan file moved to `Seal-steps/Completed/` (optional; foreman discretion)
