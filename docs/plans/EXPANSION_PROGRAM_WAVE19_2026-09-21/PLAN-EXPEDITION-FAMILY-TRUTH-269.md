# PLAN-EXPEDITION-FAMILY-TRUTH-269 — Encounter, Loot & Aggregate Plumbing

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TRANSPORT-EXPEDITION-30, PLAN-TRAVEL-ENCOUNTER-TRUTH-177, PLAN-REFERENCE-INTEGRITY-34.
**Non-goals:** no expedition redesign; the family is audited as a referential
plumbing layer.

## 1. Outcome
**20 `Expeditions/` files** are referenced by no plan: resolvers
(`EncounterChoiceResolver`, `ExpeditionLootReferenceResolver`), validators
(`ExpeditionLootValidator`), aggregates (`ExpeditionAggregate`), bridges
(`ExpeditionEncounterBridge`), catalogs, and a headless demo. Loot resolution
is where unreachable ids become silent empty hauls — exactly the class Plan 34
hunts at data level; this plan verifies it at code level.

| Deliverable | Detail |
|---|---|
| Resolver census | every resolver ↔ its consumer (Plans 30/177) ↔ failure mode |
| Loot validation | validator rejects unknown ids with a typed failure; a fixture proves it |
| Aggregate integrity | `ExpeditionAggregate` fields trace to owners; no duplicated totals |
| Bridge tests | encounter bridge routes to Plan 177's table with fixtures |
| Demo truth | the headless demo names a verb (Plan 86 pattern) |

## 2. Evidence
- 20 `Expeditions/` basenames absent from every plan body (Wave 19 file-level audit).
- Plan 34's reference families are the id space loot references.
- Plans 30/177 own travel and encounters.

## 3. Packages
- **EXF-269A** resolver census + failure-mode table.
- **EXF-269B** loot validator tests (unknown id → typed failure).
- **EXF-269C** aggregate integrity tests.
- **EXF-269D** bridge fixture.
- **EXF-269E** demo→verb verification.

## 4. Acceptance & verification
- Unknown loot ids fail loudly; aggregates equal their owners' totals.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/`.

## 5. Risks
Silent empty hauls → validator is the guard.
Aggregate drift → integrity tests bind to owners.

---

## 6. Expanded census (41 files in scope · 11,843 lines · 0 still unmentioned)

Scope: files under `Assets/Ashfall.Core/Expeditions/`. The plan's premise was the family **at authoring time**; later
plans have since referenced some files, so the table marks each row
`still unmentioned` or `since-mentioned`. Both are in scope for this plan: the
since-mentioned files are re-verified for owner discipline, and the still-
unmentioned ones are the original audit target. Class distribution:
System 17 · Catalog 9 · Support 8 · Loader 4 · Demo 2 · DTO/Type 1.

| File | Lines | Class | Banned | Empty catches | Capture/Restore | Status |
|---|---:|---|---:|---:|---:|---|
| `AerialReconWindowEngine.cs` | 200 | System | 0 | 0 | 0 | since-mentioned |
| `AmphibiousDraisineCatalog.cs` | 237 | Catalog | 0 | 0 | 0 | since-mentioned |
| `AmphibiousDraisineEngine.cs` | 516 | System | 0 | 0 | 2 | since-mentioned |
| `ArmoredCrawlerExpeditionSystem.cs` | 402 | System | 0 | 0 | 2 | since-mentioned |
| `ArmoredCrawlerModuleCatalog.cs` | 133 | Catalog | 0 | 0 | 0 | since-mentioned |
| `AviationSystem.cs` | 462 | System | 0 | 0 | 2 | since-mentioned |
| `ChemicalReconEngine.cs` | 593 | System | 0 | 0 | 2 | since-mentioned |
| `ColonySystem.cs` | 517 | System | 0 | 0 | 2 | since-mentioned |
| `DiscoveryConsequenceSystem.cs` | 390 | System | 0 | 1 | 2 | since-mentioned |
| `DiveInstanceRunner.cs` | 117 | Support | 0 | 0 | 0 | since-mentioned |
| `DraisineRerailingSystem.cs` | 255 | System | 0 | 0 | 3 | since-mentioned |
| `EncounterChoiceResolver.cs` | 133 | Support | 0 | 0 | 6 | since-mentioned |
| `ExpeditionAggregate.cs` | 116 | Support | 0 | 0 | 1 | since-mentioned |
| `ExpeditionCatalogLoader.cs` | 130 | Loader | 0 | 0 | 0 | since-mentioned |
| `ExpeditionEncounterBridge.cs` | 355 | Support | 0 | 0 | 0 | since-mentioned |
| `ExpeditionHeadlessDemo.cs` | 120 | Demo | 0 | 0 | 5 | since-mentioned |
| `ExpeditionLootReferenceResolver.cs` | 93 | Support | 0 | 0 | 0 | since-mentioned |
| `ExpeditionLootValidator.cs` | 115 | Support | 0 | 0 | 0 | since-mentioned |
| `ExpeditionNavalSystem.cs` | 280 | System | 0 | 0 | 0 | since-mentioned |
| `ExpeditionSystem.cs` | 1580 | System | 0 | 0 | 5 | since-mentioned |
| `ExpeditionTravelStretch.cs` | 54 | Support | 0 | 0 | 0 | since-mentioned |
| `MineClearingFlailEngine.cs` | 351 | System | 0 | 0 | 2 | since-mentioned |
| `MineFlailCatalogLoader.cs` | 110 | Loader | 0 | 0 | 0 | since-mentioned |
| `RadarEcmCatalog.cs` | 96 | Catalog | 0 | 0 | 0 | since-mentioned |
| `RailGrindingCatalogLoader.cs` | 128 | Loader | 0 | 0 | 0 | since-mentioned |
| `RailGrindingEngine.cs` | 336 | System | 0 | 0 | 2 | since-mentioned |
| `RailLogisticsCatalog.cs` | 59 | Catalog | 0 | 0 | 0 | since-mentioned |
| `RailwayInterlockEngine.cs` | 730 | System | 1 | 0 | 11 | since-mentioned |
| `RailwaySystem.cs` | 831 | System | 0 | 0 | 3 | since-mentioned |
| `ReconTelemetryCatalog.cs` | 83 | Catalog | 0 | 0 | 0 | since-mentioned |
| `ReconTelemetryCatalogLoader.cs` | 34 | Loader | 0 | 0 | 0 | since-mentioned |
| `ReconTelemetryHeadlessDemo.cs` | 94 | Demo | 0 | 0 | 1 | since-mentioned |
| `ReconTelemetryState.cs` | 57 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `ReconTelemetrySystem.cs` | 331 | System | 0 | 0 | 2 | since-mentioned |
| `RunFlatTireEngine.cs` | 348 | System | 0 | 0 | 2 | since-mentioned |
| `ScavengingTableCatalog.cs` | 209 | Catalog | 0 | 0 | 0 | since-mentioned |
| `TravelEncounterCombatBinder.cs` | 57 | Support | 0 | 0 | 0 | since-mentioned |
| `VehicleArmorGradeCatalog.cs` | 155 | Catalog | 0 | 0 | 0 | since-mentioned |
| `VehicleGarageCatalog.cs` | 62 | Catalog | 0 | 0 | 0 | since-mentioned |
| `VehicleGarageSystem.cs` | 865 | System | 0 | 0 | 2 | since-mentioned |
| `VerticalAscentCatalog.cs` | 109 | Catalog | 0 | 0 | 0 | since-mentioned |

**Census totals:** 1 banned nondeterministic references · 1 empty-catch sites · 19 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `expeditions.json` | object[2 keys] |
| `wasteland_expeditions_master.json` | object[3 keys] |

**State surfaces (capture/restore present):**

- `AmphibiousDraisineEngine.cs`
- `ArmoredCrawlerExpeditionSystem.cs`
- `AviationSystem.cs`
- `ChemicalReconEngine.cs`
- `ColonySystem.cs`
- `DiscoveryConsequenceSystem.cs`
- `DraisineRerailingSystem.cs`
- `EncounterChoiceResolver.cs`
- `ExpeditionAggregate.cs`
- `ExpeditionHeadlessDemo.cs`
- `ExpeditionSystem.cs`
- `MineClearingFlailEngine.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Expeditions/` |
| Files referenced by tests | 139 name references across the test tree |
| Determinism scan | 1 banned references to fix or justify |
| Failure scan | 1 empty-catch sites to route through Plan 35's rules |
| Drift | 41 of 41 files became plan-referenced since authoring — re-verify their owners |
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
(41 files). Other plans referencing those names: **14**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-TRANSPORT-EXPEDITION-30` | 32 |
| `PLAN-EXPEDITION-VEHICLE-TRUTH-219` | 13 |
| `PLAN-AUTONOMOUS-MACHINES-79` | 9 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 5 |
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `EVIDENCE` | 3 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 3 |
| `PLAN-MARITIME-DEEPWATER-27` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `EXF-269A` | `EncounterChoiceResolver.cs`, `ExpeditionLootReferenceResolver.cs`, `ScavengingTableCatalog.cs` |
| `EXF-269B` | `ExpeditionLootValidator.cs` |
| `EXF-269C` | `ExpeditionAggregate.cs` |
| `EXF-269D` | `ExpeditionEncounterBridge.cs` |
| `EXF-269E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 41; intra-domain edges: **21**; isolated files:
**16**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `AmphibiousDraisineEngine` | `AmphibiousDraisineCatalog` |
| `ArmoredCrawlerExpeditionSystem` | `ArmoredCrawlerModuleCatalog` |
| `DiscoveryConsequenceSystem` | `ExpeditionSystem` |
| `DraisineRerailingSystem` | `RailwaySystem` |
| `EncounterChoiceResolver` | `ExpeditionEncounterBridge` |
| `ExpeditionEncounterBridge` | `ExpeditionSystem` |
| `ExpeditionHeadlessDemo` | `ExpeditionSystem` |
| `ExpeditionLootValidator` | `ExpeditionCatalogLoader` |
| `ExpeditionLootValidator` | `ExpeditionLootReferenceResolver` |
| `ExpeditionSystem` | `ScavengingTableCatalog` |
| `RailLogisticsCatalog` | `RailwaySystem` |
| `RailwayInterlockEngine` | `RailwaySystem` |
| `RailwaySystem` | `VehicleGarageSystem` |
| `ReconTelemetryCatalogLoader` | `ReconTelemetryCatalog` |
| `ReconTelemetryHeadlessDemo` | `ReconTelemetryCatalogLoader` |
| `ReconTelemetryHeadlessDemo` | `ReconTelemetrySystem` |
| `ReconTelemetryState` | `ReconTelemetrySystem` |
| `ReconTelemetrySystem` | `ReconTelemetryCatalog` |
| `ReconTelemetrySystem` | `ReconTelemetryState` |
| `VehicleGarageSystem` | `VehicleArmorGradeCatalog` |
| `VehicleGarageSystem` | `VehicleGarageCatalog` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `ExpeditionSystem` | 3 |
| `RailwaySystem` | 3 |
| `ReconTelemetryCatalog` | 2 |
| `ReconTelemetrySystem` | 2 |
| `AmphibiousDraisineCatalog` | 1 |
| `ArmoredCrawlerModuleCatalog` | 1 |
| `ExpeditionCatalogLoader` | 1 |
| `ExpeditionEncounterBridge` | 1 |
| `ExpeditionLootReferenceResolver` | 1 |
| `ReconTelemetryCatalogLoader` | 1 |

**Class split:** hub 7 · sink 8 · source 10 · isolated 16.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 41. Host files: **59** · Test files: **86** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 59 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/ExpansionAudioBridge.cs`, `src/Host/AmphibiousDraisineHostSession.cs`, `src/Host/ChemicalReconHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 86 | `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagshipB70_B73Tests.cs`, `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`, `Ashfall.Core.Tests/ContractorRosterSystemTests.cs`, `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/breaching_equipment_catalog.json`, `Assets/StreamingAssets/Data/slice_seven_days.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **21** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `amphibious_draisine` |
| `armored_crawlers` |
| `aviation` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `collectible_discovery` |
| `combat` |
| `draisine_recovery` |
| `encounter_choice` |
| `expedition` |
| `expedition_stealth` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **20** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--amphibious-draisine-selftest` |
| `--chemical-dependency-save-selftest` |
| `--combat-breaching-selftest` |
| `--combat-selftest` |
| `--expedition-encounter-bridge-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--expedition-playtest-selftest` |
| `--expedition-selftest` |
| `--ice-road-tick-demo` |
| `--mine-flail-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **35**.

| Event | First declaration |
|---|---|
| `MoralChoiceSystem` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` |
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterSurfaced` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnColonyDied` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnColonyStressed` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnColonySwarming` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnEncounterArrived` | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/amphibious_draisine_catalog.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/armored_crawler_modules.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/colony_blueprints.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/discovery_consequences.json` |
| `Assets/StreamingAssets/Data/dive_sites.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/mine_flail_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **5** (18 files, 153 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Combat` | 10 | 84 |
| `MoralChoice` | 4 | 33 |
| `NarrativeConsequence` | 1 | 20 |
| `Rail` | 1 | 5 |
| `Telemetry` | 2 | 11 |

**Verdict:** 153 cases sit under matching regions — run those first (`Combat`, `MoralChoice`, `NarrativeConsequence`, `Rail`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **70**
(20 of them panels/HUD).

| Host file |
|---|
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/ArmoredCrawlerSaveStore.cs` |
| `src/Host/AviationSaveStore.cs` |
| `src/Host/ChemicalDependencyHostSession.cs` |
| `src/Host/ChemicalDependencySaveSelfTest.cs` |
| `src/Host/ChemicalDependencySaveStore.cs` |
| `src/Host/ChemicalReconHostSession.cs` |
| `src/Host/ChemicalReconSaveStore.cs` |
| `src/Host/ChemicalSynthesisHostSession.cs` |
| `src/Host/ChemicalSynthesisSaveStore.cs` |
| `src/Host/CollectibleDiscoverySaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **21**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `amphibious_draisine` | no |
| `armored_crawlers` | no |
| `aviation` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `collectible_discovery` | no |
| `combat` | no |
| `draisine_recovery` | no |
| `encounter_choice` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **9**.

| Stream |
|---|
| `amphibious_draisine` |
| `combat` |
| `expedition` |
| `mineral_chemical` |
| `moral_choice` |
| `route_engineering_mine_flail` |
| `route_engineering_rail_grinding` |
| `runflat_tire` |
| `vertical_ascent` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **36**
(CODEX_ONLY 12, GAMEPLAY_CONSUMED 14, OPTIONAL 1, UNRESOLVED 9).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `armored_crawler_modules.json` | UNRESOLVED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `dive_sites.json` | GAMEPLAY_CONSUMED |
| `moral_choice_chains.json` | GAMEPLAY_CONSUMED |
| `moral_choice_faction_reactions.json` | GAMEPLAY_CONSUMED |
| `moral_choice_flags.json` | GAMEPLAY_CONSUMED |

**Verdict:** 9 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 14
**Surface:** save sections 21 (laddered 0) · RNG streams 9 · host files 21 · catalogs 22 · test regions 5 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-EXPEDITION-FAMILY-TRUTH-269
wave: 19
status: PROPOSED — foreman claim required
packages: EXF-269A, EXF-269B, EXF-269C, EXF-269D, EXF-269E
claim paths:
  - src/Host/AmphibiousDraisineHostSession.cs  # §19 candidate host surface
  - src/Host/AmphibiousDraisineSaveStore.cs  # §19 candidate host surface
  - src/Host/ArmoredCrawlerSaveStore.cs  # §19 candidate host surface
  - src/Host/AviationSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/amphibious_draisine_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalous_expedition_encounters.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Combat/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 14 other plan(s) name these artifacts (§12)
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
