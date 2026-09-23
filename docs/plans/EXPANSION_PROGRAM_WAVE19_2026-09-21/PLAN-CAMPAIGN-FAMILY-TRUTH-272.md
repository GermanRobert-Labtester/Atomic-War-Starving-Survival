# PLAN-CAMPAIGN-FAMILY-TRUTH-272 — Calendar, Briefing & Provenance Data

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TEMPORAL-AUTHORITY-33, PLAN-CAMPAIGN-PORTABILITY-104, PLAN-CAMPAIGN-EPILOGUE-TRUTH-259.
**Non-goals:** no clock redesign; the family is audited for calendar/briefing
correctness.

## 1. Outcome
**14 `Campaign/` files** are referenced by no plan: `CampaignCalendar`,
`CampaignCalendarReadModel`, `CampaignDaySave`, `DailyBriefingSave`,
`DailyBriefingCadenceFilter`, `CrisisPredictionModel`, `CampaignProvenance`,
`CampaignInitializationMode`, `BriefingRouteMap`, `CampaignEpilogueCatalog`.
Calendar/save pairs must agree with the canonical clock (Plan 33).

| Deliverable | Detail |
|---|---|
| Calendar truth | calendar derivations read the canonical clock; a fixture compares both |
| Save agreement | day/briefing saves round-trip and match the clock after load |
| Cadence filter | briefing cadence is day-based; OS clock change is a no-op (fixture) |
| Prediction model | crisis prediction inputs documented; outputs advisory only (no hidden decisions) |
| Provenance | campaign provenance fields are recorded and readable |

## 2. Evidence
- 14 `Campaign/` basenames absent from every plan body (Wave 19 file-level audit).
- Plan 33 owns the clock; Plan 104 exports campaigns; Plan 259 assembles epilogues.
- Plan 87's ladder applies if briefing/day save shapes change.

## 3. Packages
- **CMF-272A** calendar↔clock tests.
- **CMF-272B** day/briefing save round-trips.
- **CMF-272C** cadence OS-clock no-op fixture.
- **CMF-272D** prediction input/authority audit.
- **CMF-272E** provenance record test.

## 4. Acceptance & verification
- Calendar equals clock derivations; saves round-trip; prediction never mutates authority.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/`.

## 5. Risks
Second clock → the comparison fixture is the guard.
Prediction as authority → advisory-only audit.

---

## 6. Expanded census (18 files in scope · 3,823 lines · 0 still unmentioned)

Scope: files under `Assets/Ashfall.Core/Campaign/`. The plan's premise was the family **at authoring time**; later
plans have since referenced some files, so the table marks each row
`still unmentioned` or `since-mentioned`. Both are in scope for this plan: the
since-mentioned files are re-verified for owner discipline, and the still-
unmentioned ones are the original audit target. Class distribution:
Support 12 · System 2 · Save 2 · Catalog 1 · DTO/Type 1.

| File | Lines | Class | Banned | Empty catches | Capture/Restore | Status |
|---|---:|---|---:|---:|---:|---|
| `BriefingRouteMap.cs` | 130 | Support | 0 | 0 | 0 | since-mentioned |
| `CampaignCalendar.cs` | 400 | Support | 0 | 0 | 0 | since-mentioned |
| `CampaignCalendarReadModel.cs` | 63 | Support | 0 | 0 | 0 | since-mentioned |
| `CampaignDayCoordinator.cs` | 513 | System | 0 | 0 | 8 | since-mentioned |
| `CampaignDaySave.cs` | 126 | Save | 0 | 0 | 0 | since-mentioned |
| `CampaignEpilogueCatalog.cs` | 74 | Catalog | 0 | 0 | 0 | since-mentioned |
| `CampaignEpilogueEngine.cs` | 221 | System | 0 | 0 | 0 | since-mentioned |
| `CampaignInitializationMode.cs` | 14 | Support | 0 | 0 | 0 | since-mentioned |
| `CampaignProvenance.cs` | 170 | Support | 0 | 0 | 0 | since-mentioned |
| `CrisisPredictionModel.cs` | 374 | Support | 0 | 0 | 0 | since-mentioned |
| `DailyBriefingCadenceFilter.cs` | 86 | Support | 0 | 0 | 0 | since-mentioned |
| `DailyBriefingReportBuilder.cs` | 743 | Support | 0 | 0 | 0 | since-mentioned |
| `DailyBriefingSave.cs` | 155 | Save | 0 | 0 | 2 | since-mentioned |
| `DailyBriefingTypes.cs` | 99 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `DayEventVocabulary.cs` | 279 | Support | 0 | 0 | 0 | since-mentioned |
| `DayRecord.cs` | 104 | Support | 0 | 0 | 0 | since-mentioned |
| `SemanticKind.cs` | 45 | Support | 0 | 0 | 0 | since-mentioned |
| `SliceScenario.cs` | 227 | Support | 0 | 0 | 0 | since-mentioned |

**Census totals:** 0 banned nondeterministic references · 0 empty-catch sites · 2 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `campaign_epilogues.json` | object[2 keys] |
| `propaganda_campaigns.json` | object[3 keys] |

**State surfaces (capture/restore present):**

- `CampaignDayCoordinator.cs`
- `DailyBriefingSave.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Campaign/` |
| Files referenced by tests | 55 name references across the test tree |
| Determinism scan | 0 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Drift | 18 of 18 files became plan-referenced since authoring — re-verify their owners |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every file in scope; no edits.
2. Still-unmentioned files: the original audit target — consumer or ownerless verdict.
3. Since-mentioned files: confirm the new plan's claim actually owns them; no double ownership.
4. Catalogs and loaders: justify or report inert.
5. Systems and saves: one owner per state; keys per Plan 1 Appendix Q.
6. Regression: focused region plus this census regenerated.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; malformed fixture fails typed with the field named |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore round-trip; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing verb or is retired |
| Support | consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it
does not widen the plan's scope or create new authorities.

---

## 12. Cross-plan coupling

This is a family-survey plan; the domain set is the plan's own `.cs` enumeration
(18 files). Other plans referencing those names: **7**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-TEMPORAL-AUTHORITY-33` | 3 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 3 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 2 |
| `PLAN-CAMPAIGN-EPILOGUE-TRUTH-259` | 2 |
| `PLAN-UNBLOCK-03` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-SCENARIO-AUTHORING-102` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CMF-272A` | `CampaignCalendar.cs`, `CampaignCalendarReadModel.cs` |
| `CMF-272B` | `BriefingRouteMap.cs`, `DailyBriefingCadenceFilter.cs`, `DailyBriefingReportBuilder.cs` |
| `CMF-272C` | `DailyBriefingCadenceFilter.cs` |
| `CMF-272D` | `CrisisPredictionModel.cs` |
| `CMF-272E` | `CampaignProvenance.cs`, `DayRecord.cs` |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 18; intra-domain edges: **13**; isolated files:
**7**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `BriefingRouteMap` | `DayEventVocabulary` |
| `BriefingRouteMap` | `SemanticKind` |
| `CampaignCalendar` | `CampaignCalendarReadModel` |
| `CampaignCalendar` | `CampaignDayCoordinator` |
| `CampaignDayCoordinator` | `CampaignCalendar` |
| `CampaignDayCoordinator` | `CampaignDaySave` |
| `CampaignDaySave` | `CampaignDayCoordinator` |
| `CampaignEpilogueEngine` | `CampaignEpilogueCatalog` |
| `DailyBriefingReportBuilder` | `BriefingRouteMap` |
| `DailyBriefingReportBuilder` | `DailyBriefingCadenceFilter` |
| `DailyBriefingReportBuilder` | `DayEventVocabulary` |
| `DayEventVocabulary` | `DailyBriefingReportBuilder` |
| `DayEventVocabulary` | `SemanticKind` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `CampaignDayCoordinator` | 2 |
| `DayEventVocabulary` | 2 |
| `SemanticKind` | 2 |
| `BriefingRouteMap` | 1 |
| `CampaignCalendar` | 1 |
| `CampaignCalendarReadModel` | 1 |
| `CampaignDaySave` | 1 |
| `CampaignEpilogueCatalog` | 1 |
| `DailyBriefingCadenceFilter` | 1 |
| `DailyBriefingReportBuilder` | 1 |

**Class split:** hub 6 · sink 4 · source 1 · isolated 7.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 18. Host files: **21** · Test files: **30** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 21 | `src/Host/CampaignDayPersistenceAdapter.cs`, `src/Host/CampaignDaySaveStore.cs`, `src/Host/DailyBriefingSaveStore.cs`, `src/Host/HostCli.WorldPlaytest.cs`, `src/Main.BriefingCrisis.cs` |
| Tests (`Ashfall.Core.Tests/`) | 30 | `Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs`, `Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs`, `Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorSourceGateTests.cs`, `Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs`, `Ashfall.Core.Tests/Campaign/CampaignDayDifficultyMigrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **6** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `daily_briefing` |
| `host_event` |
| `mental_health_crisis` |
| `route_infrastructure` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--deep-coast-route-selftest` |
| `--holdfast-briefing` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnBarterOnlyModeChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCrisisResolved` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnEntryRead` | `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` |
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnProvenanceComplete` | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **10**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/epilogue_chronicle.json` |
| `Assets/StreamingAssets/Data/epilogue_personalization.json` |
| `Assets/StreamingAssets/Data/food_types.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_route_waypoint_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/intake_filter_clogging_logs.json` |
| `Assets/StreamingAssets/Data/narrative/relic_provenance_dossiers.json` |
| `Assets/StreamingAssets/Data/narrative/scavenger_expedition_route_notes.json` |
| `Assets/StreamingAssets/Data/slice_seven_days.json` |
| `Assets/StreamingAssets/Data/weather_route_gates.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (32 files, 187 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |

**Verdict:** 187 cases sit under matching regions — run those first (`Campaign`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **23**
(4 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/DailyBriefingSaveStore.cs` |
| `src/Host/HoldfastBriefingView.cs` |
| `src/Host/HostEventAdapter.cs` |
| `src/Host/HostEventSaveStore.cs` |
| `src/Host/MentalHealthCrisisHostSession.cs` |
| `src/Host/RouteInfrastructureSaveStore.cs` |
| `src/Main.BriefingCrisis.cs` |
| `src/Main.Campaign.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **6**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `campaign` | no |
| `campaign_day` | no |
| `daily_briefing` | no |
| `host_event` | no |
| `mental_health_crisis` | no |
| `route_infrastructure` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `black_market_debt_event` |
| `route_engineering_mine_flail` |
| `route_engineering_rail_grinding` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **6**
(CODEX_ONLY 4, GAMEPLAY_CONSUMED 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `epilogue_chronicle.json` | GAMEPLAY_CONSUMED |
| `narrative/expedition_route_waypoint_notes_batch_2.json` | CODEX_ONLY |
| `narrative/intake_filter_clogging_logs.json` | CODEX_ONLY |
| `narrative/relic_provenance_dossiers.json` | CODEX_ONLY |
| `narrative/scavenger_expedition_route_notes.json` | CODEX_ONLY |
| `weather_route_gates.json` | UNRESOLVED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 7
**Surface:** save sections 6 (laddered 0) · RNG streams 3 · host files 15 · catalogs 16 · test regions 1 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CAMPAIGN-FAMILY-TRUTH-272
wave: 19
status: PROPOSED — foreman claim required
packages: CMF-272A, CMF-272B, CMF-272C, CMF-272D, CMF-272E
claim paths:
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - src/Host/CampaignDaySaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/epilogue_chronicle.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 7 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
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
