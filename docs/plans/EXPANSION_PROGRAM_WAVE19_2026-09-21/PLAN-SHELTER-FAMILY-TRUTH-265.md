# PLAN-SHELTER-FAMILY-TRUTH-265 — Process Catalogs & Cascade Rules

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-CASCADE-COORDINATOR-TRUTH-249, PLAN-DATA-SCHEMA-COVERAGE-90.
**Non-goals:** no process redesign; the family is audited for catalog→engine
wiring.

## 1. Outcome
**26 `Shelter/` files** are referenced by no plan: process catalogs
(`CarbonCompositeCatalog`, `CellulosicBiofuelCatalog`, `CupolaFoundryCatalog`,
`CvdDiamondCatalog`, `FischerTropschCatalog`), cascade rules
(`CascadeRuleCatalog`), and specialty data (`CaptiveInterrogationCatalog`).
Each catalog should feed exactly one engine; orphan catalogs mean unimplemented
or silently defaulted processes.

| Deliverable | Detail |
|---|---|
| Catalog→engine map | each catalog ↔ its engine (Plans 45/178/187/191/199/202/226/240/260) |
| Orphan catalog report | catalogs with no engine consumer |
| Cascade rule validation | rules parse and reference real event types (Plan 249) |
| Specialty data review | interrogation catalog routed to Plan 197/243 or flagged |
| Test presence | catalog round-trip fixtures |

## 2. Evidence
- 26 `Shelter/` basenames absent from every plan body (Wave 19 file-level audit).
- Plans 45/249 own the engines and coordinator; this plan is the catalog wiring.
- Plan 90's schema stage applies to any catalog gaining a schema.

## 3. Packages
- **SHF-265A** catalog→engine map + orphan report.
- **SHF-265B** cascade rule parse/validation test.
- **SHF-265C** specialty catalog routing decision.
- **SHF-265D** round-trip fixtures.

## 4. Acceptance & verification
- Every catalog names its engine or is reported orphaned; cascade rules reference real types.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Silent defaults → orphan report makes them visible.
Rule drift → parse test binds rules to Plan 249's event set.

---

## 6. Expanded census (21 family files · 3,574 lines)

Scope: files under `Assets/Ashfall.Core/Shelter/` whose basename is referenced
by no plan body (the Wave 19 family definition). Class distribution: Catalog 11 · Support 6 · Loader 2 · DTO/Type 1 · Save 1.

| File | Lines | Class | Banned refs | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `EbPvdCoatingCatalogLoader.cs` | 176 | Loader | 0 | 0 | 0 |
| `FluidDeliveryApplicator.cs` | 137 | Support | 0 | 0 | 0 |
| `FluidWaterTreatmentBridge.cs` | 46 | Support | 0 | 0 | 0 |
| `FogHarvestingCatalog.cs` | 84 | Catalog | 0 | 0 | 0 |
| `FoodPreservationCatalog.cs` | 140 | Catalog | 0 | 0 | 0 |
| `GeothermalAquiferState.cs` | 27 | DTO/Type | 0 | 0 | 0 |
| `GeothermalCatalog.cs` | 58 | Catalog | 0 | 0 | 0 |
| `GeothermalCatalogLoader.cs` | 34 | Loader | 0 | 0 | 0 |
| `HydroponicCropCatalog.cs` | 149 | Catalog | 0 | 0 | 0 |
| `MachineTellAudioSync.cs` | 137 | Support | 0 | 0 | 0 |
| `ShelterMachineTellCatalog.cs` | 654 | Catalog | 0 | 0 | 0 |
| `OrbitalHarrowCatalog.cs` | 70 | Catalog | 0 | 0 | 0 |
| `PrecisionBroachingCatalog.cs` | 81 | Catalog | 0 | 0 | 0 |
| `ShelterRoomIdentityCatalog.cs` | 460 | Catalog | 0 | 0 | 0 |
| `SanitationConsequenceRules.cs` | 51 | Support | 0 | 0 | 0 |
| `SanitationFacilityCatalog.cs` | 288 | Catalog | 0 | 0 | 0 |
| `SeismicDynamicsSystem.Monitoring.cs` | 267 | Support | 0 | 0 | 0 |
| `ShelterAssignmentSave.cs` | 80 | Save | 0 | 0 | 0 |
| `ShelterShieldingModel.cs` | 305 | Support | 0 | 0 | 0 |
| `SkyLayerArmorCatalog.cs` | 222 | Catalog | 0 | 0 | 0 |
| `WallCarvingCatalog.cs` | 108 | Catalog | 0 | 0 | 0 |

**Census totals:** 0 banned nondeterministic references · 0 empty-catch sites · 0 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `shelter_machine_identities.json` | object[5 keys] |
| `shelter_room_identities.json` | object[5 keys] |
| `shelter_social_events.json` | object[2 keys] |
| `shelter_audio_cues.json` | object[3 keys] |
| `shelter_insulation_catalog.json` | object[2 keys] |
| `shelter_rooms.json` | object[4 keys] |

**State surfaces (capture/restore present):**

None — no save work is implied by this family.

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Family files referenced by tests | 47 name references across the test tree |
| Determinism scan | 0 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every family file; no edits in this step.
2. Catalogs and loaders: prove a consumer or report the file as inert.
3. DTO/Type files: round-trip or consume-only proof; unknown values fail typed.
4. System files: confirm the single owner per state; remove duplicated stores.
5. Demo/tooling files: resolve to a real CLI verb or retire (Plan 86 pattern).
6. Regression: focused region plus this family census regenerated.

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
(20 files). Other plans referencing those names: **16**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-DEEP-STRATA-83` | 3 |
| `PLAN-GEOTHERMAL-PLANT-TRUTH-191` | 3 |
| `PLAN-GEOTHERMAL-AQUIFER-TRUTH-260` | 3 |
| `PLAN-FLUID-LOGISTICS-TRUTH-179` | 2 |
| `PLAN-INDUSTRY-AUTOMATION-45` | 1 |
| `PLAN-ENERGY-NUCLEAR-48` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |
| `PLAN-PRESERVATION-TRUTH-118` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SHF-265A` | `EbPvdCoatingCatalogLoader.cs`, `FogHarvestingCatalog.cs`, `FoodPreservationCatalog.cs` |
| `SHF-265B` | no name match — resolve at claim time |
| `SHF-265C` | `EbPvdCoatingCatalogLoader.cs`, `FogHarvestingCatalog.cs`, `FoodPreservationCatalog.cs` |
| `SHF-265D` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 20; intra-domain edges: **2**; isolated files:
**16**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `FluidDeliveryApplicator` | `FluidWaterTreatmentBridge` |
| `MachineTellAudioSync` | `ShelterMachineTellCatalog` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `FluidWaterTreatmentBridge` | 1 |
| `ShelterMachineTellCatalog` | 1 |
| `EbPvdCoatingCatalogLoader` | 0 |
| `FluidDeliveryApplicator` | 0 |
| `FogHarvestingCatalog` | 0 |
| `FoodPreservationCatalog` | 0 |
| `GeothermalAquiferState` | 0 |
| `GeothermalCatalog` | 0 |
| `GeothermalCatalogLoader` | 0 |
| `HydroponicCropCatalog` | 0 |

**Class split:** hub 0 · sink 2 · source 2 · isolated 16.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 20. Host files: **15** · Test files: **24** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 15 | `src/Audio/AudioSelfTest.cs`, `src/Host/FluidLogisticsHostSession.cs`, `src/Host/GeothermalAquiferHostSession.cs`, `src/Host/GeothermalAquiferSaveStore.cs`, `src/Host/ShelterAssignmentHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 24 | `Ashfall.Core.Tests/Campaign/Plans62_65_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/FlagshipEconomyScenarioTests.cs`, `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs`, `Ashfall.Core.Tests/NewSaveStoreChecksumSweepTests.cs`, `Ashfall.Core.Tests/Plan166_169ReloadReplayTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **26** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `ebpvd_coating` |
| `expanded_shelter` |
| `fluid_logistics` |
| `food_preservation` |
| `geothermal_aquifer` |
| `geothermal_orc` |
| `hydroponic_biomes` |
| `precision_metrology` |
| `precision_optics` |
| `sanitation` |
| `shelter` |
| `shelter_assignment` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **18** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--audio-selftest` |
| `--audio-test` |
| `--ebpvd-coating-selftest` |
| `--ebpvd-coating-uitest` |
| `--memorial-wall-selftest` |
| `--precision-metrology-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--shelter-interior-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **16**.

| Event | First declaration |
|---|---|
| `OnAssignmentChanged` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnCropFailed` | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` |
| `OnCropHarvested` | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` |
| `OnCropMatured` | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` |
| `OnCropPlanted` | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` |
| `OnRoomEntered` | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` |
| `OnRoomUnlocked` | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/barter_rules.json` |
| `Assets/StreamingAssets/Data/cascade_rules.json` |
| `Assets/StreamingAssets/Data/crop_strains.json` |
| `Assets/StreamingAssets/Data/ebpvd_coating_catalog.json` |
| `Assets/StreamingAssets/Data/fluid_infrastructure.json` |
| `Assets/StreamingAssets/Data/fog_harvesting_catalog.json` |
| `Assets/StreamingAssets/Data/food_preservation.json` |
| `Assets/StreamingAssets/Data/food_types.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **4** (98 files, 839 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `NarrativeConsequence` | 1 | 20 |
| `Shelter` | 87 | 754 |
| `Water` | 5 | 37 |

**Verdict:** 839 cases sit under matching regions — run those first (`Audio`, `NarrativeConsequence`, `Shelter`, `Water`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **89**
(18 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/EbPvdCoatingHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **26**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `ebpvd_coating` | no |
| `expanded_shelter` | no |
| `fluid_logistics` | no |
| `food_preservation` | no |
| `geothermal_aquifer` | no |
| `geothermal_orc` | no |
| `hydroponic_biomes` | no |
| `precision_metrology` | no |
| `precision_optics` | no |
| `sanitation` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **29**
(CODEX_ONLY 15, GAMEPLAY_CONSUMED 4, OPTIONAL 2, UNRESOLVED 8).

| Catalog | Classification |
|---|---|
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `geothermal_drilling_depths.json` | UNRESOLVED |
| `hydroponic_crops.json` | GAMEPLAY_CONSUMED |
| `narrative/crop_experiment_logs.json` | CODEX_ONLY |
| `narrative/crop_genome_degradation_reports.json` | CODEX_ONLY |
| `narrative/geothermal_borehole_logs.json` | CODEX_ONLY |
| `narrative/geothermal_steam_vent_diagnostics.json` | CODEX_ONLY |
| `narrative/geothermal_steam_well_logs.json` | CODEX_ONLY |
| `narrative/lead_wall_degradation_logs.json` | CODEX_ONLY |

**Verdict:** 8 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 16
**Surface:** save sections 26 (laddered 0) · RNG streams 2 · host files 14 · catalogs 22 · test regions 4 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SHELTER-FAMILY-TRUTH-265
wave: 19
status: PROPOSED — foreman claim required
packages: SHF-265A, SHF-265B, SHF-265C, SHF-265D
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_accessibility_cues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --audio-selftest
dependencies:
  - coordinate: 16 other plan(s) name these artifacts (§12)
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
