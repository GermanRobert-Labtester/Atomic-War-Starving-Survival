# FOUR-TRACK ORPHAN BATCH 4 — Ice Road / Subsidence / Trade-Route Risk / Informant Network

# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED** (2026-09-26)
> **STATUS: APPROVED BY USER**
> **Authorized by:** user directive ("Please find 4 plans to fully integrate,
> don't leave as partials, don't commit and don't overly test!") — 2026-09-26.
> **Claim:** `claim-four-track-batch4-2026-09-26` (logged in `.ai/state.md`;
> `WORKTREE_OWNERSHIP.md` is foreman-only and was not written).

## Bounded outcome

Convert four committed, zero-`src/`-reference Core orphans into fully wired
host features following the established batch pattern (session / save custody
or derived read-model / day owner / probe / focused test). One authority per
concern; no parallel mutable state; no engine leakage into Core.

| Track | Plan | Core authority (orphan) | Composition | Persistence | Probe |
|---|---|---|---|---|---|
| 1 | Plan 146 residual (Year of Ash) | `YearOfAshIceRoadSystem` + `YearOfAshStormCatalog` | composed into `YearOfAshHostSession` | rides the existing `year_of_ash` envelope | `--yoa-ice-road-selftest` |
| 2 | ORPHAN-SEAL A.56 (Wave 6 world & risk) | `SubterraneanSubsidenceEngine` | decay routed through `SubterraneanSystem` (the one underground owner) | rides the existing `subterranean` envelope (no new section) | `--subsidence-selftest` |
| 3 | ORPHAN-SEAL A.04 (Wave 3 economy) | `TradeRouteRiskBindingEngine` | derived read-model in `TradeRouteHostSession` | derived, no save section | `--trade-route-risk-selftest` |
| 4 | ORPHAN-SEAL A.83 (Wave 7 espionage) | `InformantNetworkTradecraftEngine` | new Core owner `InformantNetworkSystem` + `InformantNetworkHostSession` | new `informant_network` section | `--informant-network-selftest` |

## Premise audit (Rule 7 — verified in current source)

- All four had **0 references in `src/`** at claim time (mechanical sweep).
- **Ice road:** `YearOfAshSave` already carries an `iceRoad` DTO field and the
  codec accepts an optional `YearOfAshIceRoadSystem`, but
  `YearOfAshHostSession` never constructs/ticks/captures it — dead plumbing.
  Authored storm data: `year_of_ash_storm_windows.json` (14 windows; blocking
  types `thaw_flood`/`thermal_inversion`; open threshold ≤ −20 °C).
- **Subsidence:** pure deterministic permille engine (strata resilience, shoring
  mitigation, daily integrity decay, induced seismic events), with existing
  tests. `SubterraneanSystem`'s 156.5 cave-in roll stays the event authority;
  the engine supplies the missing daily integrity-decay path — no second
  underground-risk authority. Authored catalog: `subterranean_zones.json`.
- **Trade-route risk:** the wired `TradeRouteHostSession` holds the canonical
  contracts/routes; the engine is the named read-only projection authority.
  No new routes, no second route store, no mutation of the contract ledger.
- **Informant network:** the static engine had no stateful owner; the
  shelter-side `CounterIntelligenceSystem` (vetting/detention/defectors) does
  not own offensive field tradecraft, so a new Core ledger owner is a distinct
  authority, not a parallel one.

## Premise rejections (evidence, not vibes)

- `EmergencyMusterReadinessEngine`, `CommonTableRationingEngine`,
  `SoilReclamationProfileEngine`, `OilseedPressingEngine`,
  `ChemicalPlumeDispersionEngine`, `VerdictAccusationSystem`,
  `KnockWhitelist` family, `LoanSharkEnforcerEngine` — **claimed mid-task** by
  concurrent lanes (their untracked sessions/probes appeared in `src/Host/`).
- `GarmentLayeringThermalEngine` — duplicate thermal authority over the
  integrated Plan 142 `ClothingWarmthSystem` (+ duplicate WornGear type).
- `PowerLoadSheddingEngine` — `PowerGridHostSession` already executes
  brownout/load-shed with priorities; a second shedding evaluator would split
  the concern.
- `MaritimeExplorationSystem` — Rule 10 blocker: Plan 207 closeout requires a
  signed one-owner map for maritime save custody.
- `SurvivorLetterDeliverySystem` — parallel to the integrated
  `LetterDeliverySystem`/`letter_delivery` section (batch 1).
- Already wired by prior batches/sessions: `PowderMetallurgySystem`,
  `MechanicalPowerDrivelineEngine`, `KilnFiringEngine`,
  `ChemicalReagentSynthesisEngine`, `ShelterIdentitySystem`,
  `TerritoryControlSystem`, `LyophilizationEngine`, branch trio,
  `RationConflict`/`TraumaBond`/`CloudSeeding` (coordinator-composed),
  `ShelterPrisonerSystem` (DEC-205), `NvisC4ISystem` (false orphan),
  `MusterWarfareEngine`/`CombatBreachingEngine` (composed).

## Integration contract

1. **Core stays authority.** Host sessions apply presentation and persistence;
   no gameplay decisions in panels or probe code.
2. **Save:** Track 1 rides the existing `year_of_ash` envelope; Track 2 rides
   the existing `subterranean` envelope; Track 3 is a derived read-model (no
   save section); Track 4 adds one new section `informant_network`
   (registry row + filename + save-orchestrator call + lifecycle participant +
   day tick).
3. **Determinism:** ice road opens on authored thresholds only; subsidence
   decay is integer permille; trade risk is a pure function of committed route
   state; informant rolls hash `(worldSeed, informantId, day)` via StableHash.
   No `System.Random`.
4. **Surfaces:** ice road row on the Year-of-Ash codex readout; subsidence
   survey rows on `SubterraneanOperationsPanel`; trade risk via CLI probe
   (dossier recipe); informant rows on `FactionsPanel`. No new panels.
5. **Probes (`--*-selftest`):** Core enum row + src enum row + parse + dispatch
   + `HostCliRegistry` descriptor + help line each; ≥10 checks asserting
   authored values.
6. **Tests:** one focused file per track (Core contract through current public
   APIs; no speculative tests). Note: namespace `Ashfall.Core.Tests.Plan146`
   (NOT `Ashfall.Core.Tests.YearOfAsh` — that would shadow Core types).

## Naming-collision note

A concurrent lane authored a same-named plan
(`four-track-expansion-batch4-2026-09-26.md`, Common Table / Muster / Soil /
Action Log). Their archive now lives at
`INTEGRATED_FOUR_TRACK_EXPANSION_BATCH4_COMMON_TABLE_MUSTER_SOIL_ACTIONLOG_2026-09-26.md`;
this document is the surviving archive of the other four tracks.

## Executed result (2026-09-26, final)

| Track | Outcome |
|---|---|
| 1 Ice road | FULLY INTEGRATED — `YearOfAshHostSession` composes `YearOfAshIceRoadSystem` + authored storm catalog; TickDay + CaptureSave/RestoreSave wired (envelope `iceRoad` field now actually written); codex readout row; probe `--yoa-ice-road-selftest` 12/12; tests 9/9 |
| 2 Subsidence | FULLY INTEGRATED — `SubterraneanSystem` (one underground owner) routes daily integrity decay through the A.56 engine; truthful per-node survey rows on `SubterraneanOperationsPanel`; probe `--subsidence-selftest` 10/10; tests 6/6 |
| 3 Trade-route risk | FULLY INTEGRATED — `TradeRouteHostSession.EvaluateContractRisk` derived read-model over committed contracts (authored route crosswalk); probe `--trade-route-risk-selftest` 10/10 (no save section — derived per dossier recipe) |
| 4 Informant network | FULLY INTEGRATED — new Core owner `InformantNetworkSystem` over the static engine; `InformantNetworkHostSession` + `informant_network` section (registered, filename, save-orchestrator call, lifecycle reset, day tick); FactionsPanel INFORMANT NETWORK rows; probe `--informant-network-selftest` 10/10; tests 7/7 |

Evidence: full-tree build 0 errors; save registry gate 5/5; comprehensive
save/migration gate 1814/1814 (pin bumped 297→302 for informant_network +
concurrent lanes' rows); `--data-integrity-selftest` 427/427;
`--player-panels-uitest` PASS; `--ui-layout-selftest` PASS. Focused tests only
— no full suite, no commit.

Track 4 substitution note: `OilseedPressingEngine` was claimed by a concurrent
lane mid-task (`OilseedPressingHostSession.cs` appeared); replaced with
unclaimed `InformantNetworkTradecraftEngine` (A.83) under the same directive.
