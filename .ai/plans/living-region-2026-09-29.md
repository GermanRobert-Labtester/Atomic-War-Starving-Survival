# Feature / Task Plan: The Living Region — settlements, refugees and prices shift with the war

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_living_region_plan.md`. Family index: `docs/expansions/expansion_world_moves_without_you_index.md`.
> Nothing here is claimed, implemented, or committed. The foreman must add the `INTEGRATION_PLANS.md` entry and `WORKTREE_OWNERSHIP.md` claims per package. Decisions DEC-LR-01…10 are **proposals**, unsigned.

## 1. Goal & Outcome
- **Goal:** Twelve authored settlements gain a small, persisted, war-aware condition (Steady / Strained / Failing / Emptied / Swollen); refugee waves move conserved population between regions and petition the shelter's gate; prices nudge through the market's existing shock seam; the player learns it all through graded news (Heard / Told / Seen).
- **Outcome (observable):** on a fixed seed, a scripted war-tension rise produces a wave, a rung change, a price nudge and a gate petition, in that order, identically on replay and after a save/load round-trip.
- **Non-Goals:** no new economy/price/ownership/population authority; no new save section; no new routed panel; no change to FactionWar outcomes; no restoration of retired code (Rule 10); no Year One pacing change when ship-dark; no Unity.
- **"Done" means:** every acceptance row in §6 passes via `bin/run-scoped-tests`; ship-dark parity test passes; the handoff lists untouched shared paths.

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Evolving world ticks in phase 4 `world_evolution`, incl. wildlife→scarcity and dominance→ownership. | `src/Main.CampaignOwners.cs` `EvolvingWorldDayOwner` (~L2150–2330) | LIVE |
| E2 | Migration engine persists `RegionWeights`, `LastAppliedPhase`, `AppliedTransitionKeys`; consequence engine persists exactly-once keys; both ticked by the campaign. | `Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs`, `MigrationConsequenceEngine.cs`; `src/Main.MigrationConsequence.cs`; sections `human_migration`, `migration_consequence` in `Save/SaveSectionRegistry.cs` | LIVE |
| E3 | Market shock seam is idempotent and already used by migration. | `src/Main.MigrationConsequence.cs` `ApplyMigrationMarketConsequence` | LIVE |
| E4 | 12 settlements static; no mutable per-settlement state. | `settlements.json`; `Assets/Ashfall.Core/World/SettlementCatalog.cs` | GAP |
| E5 | Four unmapped region vocabularies. | `settlements.json`, `seasonal_human_migration.json`, `caravan_trade_routes.json`, `regional_prices.json`, `map_regions.json`, `world_evolution_seeds.json` | GAP |
| E6 | `FactionWarSystemState`: `activeWarTension`, `dominantFactionId`, standings, control %, defense pressures. | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` | LIVE |
| E7 | Dominance→ownership loop re-owns only seeds authored to the new dominant. | `EvolvingWorldDayOwner` | VERIFY |
| E8 | `PatrolTerritoryAuthority` has no `src/` reference. | grep `src/` | GAP / VERIFY |
| E9 | Embargo rules (14) and weather shocks exist and decay. | `trade_embargoes.json`; `TradeEmbargoSystem.cs` | LIVE |
| E10 | Selftest verbs: `--evolving-world-selftest`, `--world-evolution-selftest`, `--human-migration-selftest`, `--migration-consequence-selftest`, `--outpost-settlement-selftest`. | `src/Host/HostCli*.cs` | LIVE (VERIFY current arguments) |
| E11 | Gate/arrival authority for strangers at the shelter. **Found (2026-09-29, later pass):** three existing systems — `AirlockSecuritySystem` (`VisitorArrives`, `ResolveIncident`: Admit/Inspect/Quarantine/TurnAway/Defend; section `airlock_security`), `DoorEncounterSystem` (80 authored knocks), `VisitorIntegrationSystem` (the stay; `SourceVisitorId` handoff). | `Assets/Ashfall.Core/AirlockSecuritySystem.cs`; `YearOfAsh/DoorEncounterSystem.cs`; `Visitors/VisitorIntegrationSystem.cs` | **VERIFY (P0)** — confirm the live call path; agree **one shared gate adapter** with *The Quiet War* (QW-P0) and *The Plague Year* (PY-P3/P4); do not build a second |

## 3. Authority table (one authority per concern)

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Population weights per region | `SeasonalHumanMigrationEngine` (`human_migration`) | nested `settlementLive[]` + `waves[]` (additive) — **DEC-LR-02** |
| Market price nudges | market owner's shock seam (E3) | calls only |
| Location ownership | `LocationEvolutionSystem` via `EvolvingWorldDayOwner` | truthful conquest **only if DEC-LR-05 approves** |
| War state | `FactionWarSystem` | read-only |
| Route/embargo state | `TradeEmbargoSystem`, `CaravanTradeNetworkSystem` | read-only |
| Health input | `DiseaseSystem` (Plague Year) | read-only, neutral if absent |
| Strangers at the door | E11 owner | one adapter |
| Region vocabulary | new data `region_vocabulary.json` | one mapping file; nothing deleted |

## 4. Claimed Paths & Affected Files (proposed; foreman records claims. `INT` = integrator-owned shared seam)

**Core (engine-free, netstandard2.1):**
- `Assets/Ashfall.Core/World/RegionVocabularyMap.cs` (new) + loader
- `Assets/Ashfall.Core/World/RegionalPulseProjector.cs` (new, pure)
- `Assets/Ashfall.Core/World/RefugeeWaveLedger.cs` (new, pure, deterministic)
- `Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs` (additive nested state; public mutation API for conserved transfer)
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` (`INT`)
- `Assets/Ashfall.Core/Random/` stream ids (`INT`)
**Data:** `Assets/StreamingAssets/Data/region_vocabulary.json`, `regional_pulse.json` (bands, thresholds, cause tags, wave kinds), `regional_pulse_lines.json` (prose)
**Host:** `src/Main.CampaignOwners.cs` (`INT`, one new day-owner registration in phase 4 after `world_evolution`), `src/Main.MigrationConsequence.cs` (`INT`, hook), one gate adapter (file from P0)
**Presentation:** existing atlas/trade/radio/journal surfaces only (**DEC-LR-06**)
**Tests:** `Ashfall.Core.Tests/World/RegionalPulseTests.cs`, `RefugeeWaveLedgerTests.cs`, `RegionVocabularyMapTests.cs`, `Ashfall.Core.Tests/Save/LivingRegionSaveTests.cs`

## 5. Packages

### LR-P0 — Premise audit (Auditor; read-only)
- Re-verify E1–E11 with `path:line`; close E7/E8/E11; census every consumer of `Population`, `threat_level`, `attitude` in `SettlementCatalog`; write `docs/plans/LIVING_REGION_PREMISE_AUDIT_<date>.md`; foreman signs DEC-LR-01…10.
- **Accept:** each VERIFY row is resolved or converted into a named blocker.

### LR-P1 — Region Vocabulary Map (Core + data)
- Canonical vocabulary = supply-tag set (`settlement, iron_basin, industrial_belt, ash_flats, deep_coast`); map settlements, price regions, sectors, `reg_*` onto it.
- **Accept:** every settlement, price-atlas region, and evolving-world sector resolves to exactly one canonical region; integrity validator fails on an unmapped id; absence of the file changes nothing.

### LR-P2 — Regional Pulse projector (Core, pure)
- Inputs (immutable snapshot): scarcity deltas, embargo/route state, war tension + dominance, settlement authored threat, health flag, migration weight vs baseline.
- Output per settlement: pillar states, rung, **cause tag**. Hysteresis: 5 consecutive days.
- **Accept:** table-driven tests; same inputs → same output; no RNG; no I/O; no Godot; cause tag always set on a rung change.

### LR-P3 — Persisted live state + save (Core)
- Nested `settlementLive[]` (`settlementId, populationDelta, rung, rungSinceDay, causeTag, refugeesHosted`) and `waves[]` in the migration save state; schema bump with default-empty read.
- **Accept:** round-trip identical; old save loads; no new section in `SaveSectionRegistry`; ship-dark parity (no catalog → byte-identical saves to today).

### LR-P4 — Refugee wave ledger (Core)
- Wave kinds authored in `regional_pulse.json`; exactly-once keys; population **conserved** through the engine's own mutation API.
- Deterministic randomness only via a `CampaignStreamIds` fork (new id is `INT`); no `System.Random`.
- **Accept:** property test — total population weight is invariant across any wave sequence; replay with the same seed gives the same waves.

### LR-P5 — Day-owner + market seam (Host, `INT`)
- Register one owner after `world_evolution` in phase 4; on rung change or wave landing call the market shock seam with bounded (proposed 800–1400 permille), decaying, exactly-once nudges.
- **Accept:** nudge decays to exactly neutral; repeated day tick is idempotent; prices stay in [500, 2000].

### LR-P6 — Gate petitions (Host adapter)
- Waves become ≤N petitions/day through the E11 owner: open / ration / refuse / redirect. Stores decide feasibility.
- **Accept:** refusing changes standing/memory only through existing seams; no petition when the shelter is closed; save/load mid-wave preserves remaining petitions.

### LR-P7 — Graded news (Presentation)
- Heard/Told/Seen views over the same state; wrong-by-one Heard lines with later correcting Told line.
- **Accept:** presenter tests prove Heard never exposes exact rung; Seen equals state; keyboard/controller focus unaffected on any touched panel.

### LR-P8 — War settlement profile hook (optional; depends on Year Two P1B)
- Read `war_settlement` (armistice / partition / siege / ashes) from the active Chapter Profile; legacy = siege at current values.
- **Accept:** legacy save behaviour unchanged; missing profile = legacy.

### LR-P9 — Content waves W1–W4 and governance close
- Prose files per the companion §9; validator + voice lock; plan header `FULLY INTEGRATED` ×3 and archival only when the integrator accepts.

## 6. Acceptance criteria (integration proof, not compile-green)
1. Core authority, host owner, event path, persistence and an observable outcome agree for each package (CLAUDE.md "integrated" definition).
2. Deterministic replay: two runs, same seed, identical Pulse/wave/price sequence.
3. Save round-trip mid-wave.
4. Ship-dark parity: with none of the new data present, `--evolving-world-selftest`, `--human-migration-selftest`, `--migration-consequence-selftest` outputs are unchanged.
5. Conservation property (LR-P4).
6. Every rung change has a cause tag and a Board line.

## 7. Cross-plan boundaries
- **Year Two (P6 The Road):** hostile pressure on outposts stays with `OutpostPressureModel`; the Pulse may *supply* a regional-tension read-only input. The Pulse never attacks an outpost.
- **The Long Line: Freight:** routes' blocked/slow state is an input; the company never writes Pulse state.
- **The Plague Year:** outbreak declared → Health pillar fails; Plague Year owns quarantine politics.
- **The Drowned Coast:** flooded-out waves and coastal `deep_coast` harbours are consumers.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-LR-01 | Canonical region vocabulary = supply-tag set. | architecture | Yes |
| DEC-LR-02 | Live state nests in `human_migration` (not a new section, not `migration_consequence`). | architecture | Yes; decide in P0 |
| DEC-LR-03 | Rung hysteresis 5 days; five rungs, no "Thriving". | design | Yes |
| DEC-LR-04 | News grades are presentation-only. | design | Yes |
| DEC-LR-05 | Conquest is truthful (owner flips when a war outcome earns it) vs. leave the E7 loop as is. | design | Decide after E7 |
| DEC-LR-06 | No new routed panel; extend existing surfaces. | UI | Yes |
| DEC-LR-07 | Price nudge bounds 800–1400 permille. | tuning | Yes |
| DEC-LR-08 | Waves may only be met on the road via existing encounter authorities. | scope | Yes |
| DEC-LR-09 | Health pillar soft-reads Plague Year; neutral if absent. | compatibility | Yes |
| DEC-LR-10 | Chapter Profile `war_settlement` is optional (LR-P8). | compatibility | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (re-search source and data for `Refugee`, `Pulse`, `SettlementLive`)
- [ ] Premise re-verified (Rule 7); `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md` re-read; no overlapping live claim
- [ ] Signed decisions for the package in hand (§8)

## 10. Verification
- [ ] `bin/run-scoped-tests` on the new test files above plus existing migration/consequence/evolving-world tests (exact list from P0's changed-file selector)
- [ ] `--evolving-world-selftest`, `--human-migration-selftest`, `--migration-consequence-selftest` (VERIFY arguments)
- [ ] Scoped tests only (<30 s each); max 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: E11 has no existing owner (a second gate system would be needed); population conservation cannot be expressed through the engine's API without a second store; the vocabulary map needs a new region concept; any path overlaps a live claim.
