# PLAN-ELECTRONICS-COMPUTING-65 — Repair, Data Recovery, Pre-War Systems & Networks

**Wave 6 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SCIENCE-EDUCATION-38, PLAN-SIGNALS-REMOTE-SENSING-49,
PLAN-ENERGY-NUCLEAR-48.
**Non-goals:** no real exploit/hacking detail, no network simulation, no
second research authority.

## Outcome
Electronics appears across the tree — `ElectrostaticFiltrationCatalog`,
`CvdDiamond`, radio tuning, `PrewarArchiveDecryptionSystem`, `TechSalvageCatalog`,
`WorkshopReverseEngineeringSystem`, `CommsArraySystem`, `ReconTelemetrySystem`,
control systems for power/fluids/airlocks — but there is no **electronics
discipline**: repair, substitution, data recovery, and bringing dead pre-war
machines back.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Repair | equipment condition + workshop | diagnose, replace components | restored function, quality |
| Components | inventory + salvage | scavenge, substitute | compatibility, reliability |
| Data recovery | `PrewarArchiveDecryptionSystem` | read dead drives/tape | clues, schematics, records |
| Control systems | power/fluid/airlock hosts | restore automation | less manual labour, new failure modes |
| Radio/telecom | comms array + radio owner | build repeaters, wire | coverage, reliability |
| Sensors | GPR/metrology/soundings | install, calibrate | intel feeds (Plan 49) |
| Networks | shelter wiring | connect subsystems | central monitoring, single-point failure |
| Skill | survivor disciplines | study, apprentice | electronics specialty |

## Evidence
- Core: `ElectrostaticFiltrationCatalog`, `CvdDiamond*`, `PrewarArchiveCatalog/DecryptionSystem`, `TechSalvageCatalog`, `WorkshopReverseEngineeringSystem`, `CommsArraySystem`, `GroundPenetratingRadarEngine`, `OrbitalHarrowTelemetrySystem`.
- Data: `electrostatic_filtration_catalog.json`, `cvd_diamond_catalog.json`, `prewar_archives.json`, `workshop_recipes.json`, `comms_targets.json`, `recon_telemetry_probes.json`.
- Sealed prior: electronics/salvage in Plans 87/139/213; workshop reverse engineering tests.
- Contracts: equipment condition owner; no real exploit detail (abstract "machine languages").

## Packages
- **EC-65A** repair bench: diagnosis with uncertainty, component substitution, quality outcome, no instant perfect repair.
- **EC-65B** component economy: salvage yields parts; compatibility matrix authored; scarcity drives substitution.
- **EC-65C** data recovery: drives/tapes with fragment chance; recovered data feeds research/quests/records.
- **EC-65D** control restoration: automate a subsystem; benefits and new failure paths (a bad controller can misbehave).
- **EC-65E** telecom build-out: repeaters, antenna farms, coverage map (feeds Plan 42/49).
- **EC-65F** shelter network: monitoring screen + fault propagation; isolation drills.
- **EC-65G** content volumes: +10 components, +8 devices, +10 data fragments, +6 faults; abstract/fictional.

## Acceptance & verification
- Repairs are skill-dependent and never free; recovered data has consumers; determinism.
- `godot --headless --path . -- --workshop-relic-selftest`; `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/`; research suites.

## Risks
"Magic repair" trivializes scarcity → component consumption and failure chance; no infinite parts.

---

## 6. Expanded census (bespoke: electronics & computing surface)

Corrected scope: the plan's domain is not namespaced under one directory, so
this census finds electronics/computing-bearing files by name and content
across the Core tree.

| File | Lines | Class | Banned | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `CrisisPredictionModel.cs` | 374 | Support | 0 | 0 | 0 |
| `DailyBriefingReportBuilder.cs` | 743 | Support | 0 | 0 | 0 |
| `CombatHeadlessDemo.cs` | 289 | Support | 0 | 0 | 4 |
| `CombatTypes.cs` | 401 | DTO/Type | 0 | 0 | 0 |
| `CommitmentSystem.cs` | 314 | System | 0 | 0 | 4 |
| `DiseaseCatalog.cs` | 632 | Catalog | 0 | 0 | 0 |
| `DiseaseSystem.cs` | 1707 | System | 1 | 0 | 3 |
| `DiseaseTriage.cs` | 263 | Support | 0 | 0 | 0 |
| `EcologicalInfestationDefs.cs` | 90 | Support | 0 | 0 | 0 |
| `EcologicalInfestationSystem.cs` | 381 | System | 0 | 0 | 2 |
| `CaravanCatalogLoader.cs` | 143 | Loader | 0 | 0 | 0 |
| `EquipmentConditionSystem.cs` | 555 | System | 0 | 0 | 2 |
| `ExpeditionLootReferenceResolver.cs` | 93 | Support | 0 | 0 | 0 |
| `RailwaySystem.cs` | 831 | System | 0 | 0 | 3 |
| `HoldfastFactionsCatalog.cs` | 113 | Catalog | 0 | 0 | 0 |
| `HoldfastItemsCatalog.cs` | 71 | Catalog | 0 | 0 | 0 |
| `HoldfastTradeSession.cs` | 1131 | Support | 0 | 0 | 2 |
| `HostCliRegistry.cs` | 1414 | Support | 0 | 0 | 0 |
| `ItemAliases.cs` | 83 | Support | 0 | 0 | 0 |
| `MaritimeDiveSystem.cs` | 765 | System | 0 | 0 | 3 |
| `ClinicalWardTriageEngine.cs` | 307 | System | 0 | 0 | 0 |
| `DiagnosisKnowledgeStore.cs` | 190 | Support | 0 | 0 | 2 |
| `PalliativeCareDignityEngine.cs` | 254 | System | 0 | 0 | 0 |
| `PatientRecord.cs` | 233 | Support | 0 | 0 | 0 |
| `RespiratoryAfflictionHandler.cs` | 137 | Support | 0 | 0 | 0 |
| `CampSceneCatalog.cs` | 204 | Catalog | 0 | 0 | 0 |
| `EpilogueMatrix.cs` | 219 | Support | 0 | 0 | 0 |
| `LongWalkSystem.cs` | 153 | System | 0 | 0 | 2 |
| `MusterSystem.cs` | 527 | System | 0 | 0 | 3 |
| `WitnessSelector.cs` | 173 | Support | 0 | 0 | 0 |

… and 46 more.

**Totals:** 29,587 lines · 1 banned refs · 0 empty catches · 31 files with capture/restore.

## 7. Expanded surface: device & computation contract

| Rule | Detail |
|---|---|
| Device state | electronics have condition (Plan 119) and power (Plan 48); no unpowered function |
| Computation | any computing device consumes power/time and produces a documented result |
| Components | parts are inventory items; assembly consumes through Plan 93 |
| Data | schematics/references resolve through catalogs, not hardcoded knowledge |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Power coupling | a device without power performs no function (fixture) |
| Conservation | assembly consumes parts; disassembly returns them |
| Determinism | computation results seeded where variance exists |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section) — classify each file.
2. Device/power coupling audit.
3. Assembly conservation tests.
4. Catalog resolution for schematics.
5. Regression: focused shelter/crafting region.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Device | condition + power required; no unpowered function |
| Assembly | parts balance; no duplication |
| Computation | documented inputs/outputs; seeded variance only |
| Data | schematics resolve; no hardcoded knowledge |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not add a new electronics system.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 30. Other plans referencing them: **33**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-PANDEMIC-PUBLIC-HEALTH-47` | 5 |
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 5 |
| `PLAN-MUSTER-FAMILY-TRUTH-275` | 5 |
| `PLAN-ORPHAN-SEAL-01` | 4 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 4 |
| `EVIDENCE` | 4 |
| `PLAN-MUSTER-COALITION-TRUTH-130` | 4 |
| `PLAN-QUARANTINE-STRAIN-TRUTH-241` | 3 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `EC-65A` | `DiagnosisKnowledgeStore.cs` |
| `EC-65B` | `EpilogueMatrix.cs` |
| `EC-65C` | no name match — resolve at claim time |
| `EC-65D` | no name match — resolve at claim time |
| `EC-65E` | `DailyBriefingReportBuilder.cs` |
| `EC-65F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 30; intra-domain edges: **10**; isolated files:
**17**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `CampSceneCatalog` | `MusterSystem` |
| `DiseaseCatalog` | `DiseaseSystem` |
| `DiseaseSystem` | `DiseaseCatalog` |
| `DiseaseSystem` | `DiseaseTriage` |
| `DiseaseTriage` | `DiseaseSystem` |
| `EcologicalInfestationDefs` | `EcologicalInfestationSystem` |
| `EpilogueMatrix` | `MusterSystem` |
| `HoldfastTradeSession` | `ItemAliases` |
| `HostCliRegistry` | `EpilogueMatrix` |
| `PatientRecord` | `DiagnosisKnowledgeStore` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `DiseaseSystem` | 2 |
| `MusterSystem` | 2 |
| `DiagnosisKnowledgeStore` | 1 |
| `DiseaseCatalog` | 1 |
| `DiseaseTriage` | 1 |
| `EcologicalInfestationSystem` | 1 |
| `EpilogueMatrix` | 1 |
| `ItemAliases` | 1 |
| `CampSceneCatalog` | 0 |
| `CaravanCatalogLoader` | 0 |

**Class split:** hub 4 · sink 4 · source 5 · isolated 17.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 30. Host files: **51** · Test files: **112** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 51 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/ExpansionAudioBridge.cs`, `src/Disease/DiseaseHostSession.cs`, `src/Host/BlackMarketHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 112 | `Ashfall.Core.Tests/AutopsyBridgeTests.cs`, `Ashfall.Core.Tests/Campaign/CommitmentSystemTests.cs`, `Ashfall.Core.Tests/Campaign/DailyBriefingCadenceTests.cs`, `Ashfall.Core.Tests/Campaign/DailyBriefingCrisisTests.cs`, `Ashfall.Core.Tests/Campaign/DailyBriefingEventDerivedTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/ecological_infestations.json`, `Assets/StreamingAssets/Data/slice_seven_days.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **20** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan` |
| `caravan_trade_network` |
| `combat` |
| `daily_briefing` |
| `disease` |
| `ecological_infestation` |
| `equipment` |
| `equipment_condition` |
| `expedition` |
| `expedition_stealth` |
| `factions` |
| `holdfast` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **25** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--caravan-selftest` |
| `--combat-breaching-selftest` |
| `--combat-selftest` |
| `--disease-expansion-selftest` |
| `--disease-selftest` |
| `--expedition-encounter-bridge-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--expedition-playtest-selftest` |
| `--expedition-selftest` |
| `--holdfast-briefing` |
| `--holdfast-runtime-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **30**.

| Event | First declaration |
|---|---|
| `OnCampDawnResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterSurfaced` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEntered` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampFormed` | `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs` |
| `OnCampNightSegmentResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampSuppliesReserved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/disease_catalog.json` |
| `Assets/StreamingAssets/Data/dive_sites.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **5** (23 files, 179 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Combat` | 10 | 84 |
| `Equipment` | 1 | 4 |
| `Factions` | 10 | 72 |
| `Holdfast` | 1 | 13 |
| `Maritime` | 1 | 6 |

**Verdict:** 179 cases sit under matching regions — run those first (`Combat`, `Equipment`, `Factions`, `Holdfast`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **371**
(29 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **20**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `caravan` | no |
| `caravan_trade_network` | no |
| `combat` | no |
| `daily_briefing` | no |
| `disease` | no |
| `ecological_infestation` | no |
| `equipment` | no |
| `equipment_condition` | no |
| `expedition` | no |
| `expedition_stealth` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **6**.

| Stream |
|---|
| `aquaponics_disease` |
| `combat` |
| `disease` |
| `expedition` |
| `maritime` |
| `muster` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **53**
(CODEX_ONLY 14, GAMEPLAY_CONSUMED 27, OPTIONAL 5, UNRESOLVED 7).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `dive_sites.json` | GAMEPLAY_CONSUMED |
| `documents/vel_triage_log_names.json` | OPTIONAL |

**Verdict:** 7 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 33
**Surface:** save sections 20 (laddered 1) · RNG streams 6 · host files 18 · catalogs 22 · test regions 5 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ELECTRONICS-COMPUTING-65
wave: 6
status: PROPOSED — foreman claim required
packages: EC-65A, EC-65B, EC-65C, EC-65D, EC-65E, EC-65F, EC-65G
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Combat/
  - godot --headless --path . -- --caravan-selftest
dependencies:
  - coordinate: 33 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
