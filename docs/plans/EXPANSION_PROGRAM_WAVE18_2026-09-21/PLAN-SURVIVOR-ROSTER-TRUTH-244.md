# PLAN-SURVIVOR-ROSTER-TRUTH-244 — The Person Registry: Identity, Status & Transfer

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DUTY-ROSTER-TRUTH-101, PLAN-INSTITUTIONS-TRUTH-141, PLAN-CAREGIVING-TRUTH-203.
**Non-goals:** no shift model (Plan 101), no offices (Plan 141), no care load
(Plan 203).

## 1. Outcome
`Survivors/SurvivorCatalog.cs` defines `SurvivorRosterSystem` (**297 lines**),
reachable and unaddressed: the registry of who exists, their status (present,
absent, hospital, held, dead), and where they are assigned. Every system reads
"who is available"; the **registry itself** has no stated contract, so status
can disagree between systems.

| Deliverable | Detail |
|---|---|
| Status model | one status per survivor with the event that sets it; systems read, never set independently |
| Availability derivation | roster/institution/care availability derived from status, not stored twice |
| Transfers | moving between assignments/sites updates status through one hook and records the move |
| Reconciliation | a disagreement between a system's copy and the registry is a defect with a named owner (audit test) |
| Save truth | registry restores with the survivors section; no duplicate person records on load |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` (contains `SurvivorRosterSystem`; 297 lines; unaddressed — Wave 18 audit).
- Plan 101/141/203 derive availability; the registry is their shared source.
- The `survivors` save section is the persistence seam.
- Plan 82/85 arrivals add records — boundary noted.

## 3. Packages
- **SRT-244A** status model + setter table.
- **SRT-244B** availability derivation tests (no double storage).
- **SRT-244C** transfer hook + move records.
- **SRT-244D** reconciliation audit test (systems vs registry).
- **SRT-244E** save round-trip; no duplicate records.

## 4. Acceptance & verification
- Status changes only via the hook; availability matches status in every consumer fixture.
- Reconciliation finds zero disagreements after a scripted week.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Double bookkeeping → derivation tests and the reconciliation audit.
Duplicate records → save round-trip asserts identity uniqueness.

---

## 6. Expanded census (1 files · 297 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SurvivorCatalog.cs` | 297 | Catalog | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `duty_roster_locations.json` | object[2 keys] |
| `duty_roster_marks.json` | array[43] |
| `duty_roster_quests.json` | object[2 keys] |
| `duty_roster_seasons.json` | array[8] |

**State surfaces:** `SurvivorCatalog.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 4 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 8. Other plans referencing their names: **9**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 4 |
| `PLAN-FINAL-WISH-TRUTH-200` | 2 |
| `EVIDENCE` | 1 |
| `PLAN-RECREATION-MORALE-50` | 1 |
| `PLAN-SKILL-PROGRESSION-TRUTH-113` | 1 |
| `PLAN-MORALE-CONTAGION-TRUTH-162` | 1 |
| `PLAN-LATENT-EXPERT-TRUTH-239` | 1 |
| `PLAN-CONTRACTOR-ROSTER-TRUTH-245` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SRT-244A` | no name match — resolve at claim time |
| `SRT-244B` | no name match — resolve at claim time |
| `SRT-244C` | no name match — resolve at claim time |
| `SRT-244D` | no name match — resolve at claim time |
| `SRT-244E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **3** · Test files: **4** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/AssetCoverageScanner.cs`, `src/Host/AssetRegistry.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`, `Ashfall.Core.Tests/FactionWarLocationOverridesExpansionTests.cs`, `Ashfall.Core.Tests/Plan10RemediationTests.cs`, `Ashfall.Core.Tests/Tooling/JsonNamingMixPinTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **12** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `contractor_roster` |
| `deep_well` |
| `duty_roster` |
| `dynamic_quests` |
| `faction_espionage` |
| `personal_quests` |
| `quests` |
| `shelter_atmosphere` |
| `survivor_fate` |
| `survivor_mental_health` |
| `survivor_relations` |
| `survivor_social` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **16** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--asset-coverage-report` |
| `--asset-registry-selftest` |
| `--atmosphere-selftest` |
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--faction-communique-board-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **17**.

| Event | First declaration |
|---|---|
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnLocationMutated` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationOwnerChanged` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationRecast` | `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs` |
| `OnLocationRevealed` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnRosterBurned` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnRosterChanged` | `Assets/Ashfall.Core/ContractorRosterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json` |
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/asset_registry.json` |
| `Assets/StreamingAssets/Data/atmosphere_profiles.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/dose_locations.json` |
| `Assets/StreamingAssets/Data/dose_quests.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **7** (69 files, 493 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Balance` | 1 | 5 |
| `DutyRoster` | 5 | 49 |
| `Economy` | 41 | 329 |
| `Integration` | 16 | 74 |
| `PlayerCommand` | 1 | 1 |
| `Quests` | 4 | 25 |
| `Remediation` | 1 | 10 |

**Verdict:** 493 cases sit under matching regions — run those first (`Balance`, `DutyRoster`, `Economy`, `Integration`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **74**
(21 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Host/AssetCoverageReport.cs` |
| `src/Host/AssetCoverageScanner.cs` |
| `src/Host/AssetRegistry.cs` |
| `src/Host/ContentUtilizationRuntimeCollector.cs` |
| `src/Host/ContentUtilizationSelfTest.cs` |
| `src/Host/ContractorRosterHostSession.cs` |
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |
| `src/Host/DutyRosterHostSession.cs` |
| `src/Host/DutyRosterSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **19**, of which versioned-ladder sections:
**2**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `contractor_roster` | no |
| `crossing` | no |
| `deep_well` | no |
| `dose_ledger` | yes |
| `duty_roster` | no |
| `dynamic_quests` | no |
| `economy` | no |
| `faction_espionage` | no |
| `oral_lore` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `acoustic_detection` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `deep_coast` |
| `duty_roster` |
| `economy` |
| `world_evolution` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **538**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 162, OPTIONAL 24, UNRESOLVED 73).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `aircraft_parts.json` | GAMEPLAY_CONSUMED |
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `armored_crawler_modules.json` | UNRESOLVED |
| `atmospheric_sounding_catalog.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |

**Verdict:** 73 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 19 (laddered 2) · RNG streams 8 · host files 23 · catalogs 22 · test regions 7 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SURVIVOR-ROSTER-TRUTH-244
wave: 18
status: PROPOSED — foreman claim required
packages: SRT-244A, SRT-244B, SRT-244C, SRT-244D, SRT-244E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Host/AssetCoverageReport.cs  # §19 candidate host surface
  - src/Host/AssetCoverageScanner.cs  # §19 candidate host surface
  - src/Host/AssetRegistry.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Balance/
  - godot --headless --path . -- --asset-coverage-report
dependencies:
  - coordinate: 9 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 2 versioned save ladder(s) — extend, never fork
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
