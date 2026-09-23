# PLAN-MORTUARY-MEMORIAL-TRUTH-123 — Body Handling, Rites & Memorial Surfaces

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-VERTICAL-CULTURE-04, PLAN-MENTAL-HEALTH-THERAPY-64, PLAN-SAVE-GOVERNANCE-12.
**Non-goals:** no graphic depiction, no new religion system (Plan 36 owns
belief), no duplicate memorial store beside the existing data catalogs.

## 1. Outcome
Memorial content exists — `Data/memorial_rites.json`,
`Data/memorials_expansion_05.json` — and culture/belief plans own festivals and
doctrine (Plans 4, 36). What has no stated contract is the **death pipeline**:
what happens to a body, whether a rite occurs, how a memorial is created and
persisted, and how bereavement feeds the existing mental-health owner (Plan 64).

| Deliverable | Detail |
|---|---|
| Death pipeline | body → morgue/burial/other with a documented owner per step and a sanitary/psychological consequence |
| Rite selection | rites come from the existing beliefs/culture data; a missing rite degrades to a documented default, never a placeholder |
| Memorial creation | a memorial record from a catalog entry: subject, day, cause-of-death class, text key — persisted once, shown in an existing surface |
| Bereavement hand-off | grief effect enters Plan 64's model as a typed input; no separate mood score |
| Body hygiene | unhandled bodies feed the sanitation/disease owners (Plans 47/103) per a documented threshold |

## 2. Evidence
- `Assets/StreamingAssets/Data/memorial_rites.json`, `memorials_expansion_05.json` exist (verified).
- Plan 4 owns culture surfaces; Plan 36 owns belief data; Plan 64 owns psychological state.
- `SaveSectionRegistry`: `survivors` (deaths) and shelter-family sections already record the facts this pipeline consumes.
- Plan 47 owns outbreak response; body hygiene is an input to it, not a new epidemic system.

## 3. Packages
- **MMT-123A** death pipeline doc + owner per step.
- **MMT-123B** rite selection from culture/belief data + default degradation test.
- **MMT-123C** memorial record creation + persistence round-trip.
- **MMT-123D** bereavement hand-off typed input to Plan 64 + test.
- **MMT-123E** body-hygiene threshold into Plan 47/103 owners.

## 4. Acceptance & verification
- Every death completes the pipeline with one terminal handling; no body state is lost on load.
- A missing rite shows the documented default, not an empty string.
- Bereavement appears in Plan 64's model exactly once per death.
- `bash scripts/run_test.sh` on the survivors/culture regions.

## 5. Risks
Tone drift → content stays in the retired-lore voice; no new graphic depiction is added.
Overlap with Plan 64 → this plan supplies one typed input; the model stays there.

---

## 6. Expanded census (53 systems · 4 memorial data files)

This plan is data-driven: the census covers systems that reference memorial data
and the authored data files themselves. (Corrected: a name-token-only scan in
`Culture/` was empty.)

| File | Lines | Class | Banned | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `AchievementSystem.cs` | 404 | System | 1 | 0 | 2 |
| `CampaignCalendar.cs` | 400 | Support | 0 | 0 | 0 |
| `CampaignDayCoordinator.cs` | 513 | System | 0 | 0 | 8 |
| `DayEventVocabulary.cs` | 279 | Support | 0 | 0 | 0 |
| `CatalogIntegrityRules.cs` | 390 | Support | 0 | 0 | 0 |
| `CatalogIntegrityValidator.cs` | 3141 | Support | 0 | 0 | 0 |
| `InternalCommunicationSystem.cs` | 509 | System | 0 | 0 | 2 |
| `ContentUtilizationScanner.cs` | 2073 | Support | 0 | 0 | 0 |
| `ArchiveChronicleMilestones.cs` | 24 | Support | 0 | 0 | 0 |
| `CultureCreationSystem.cs` | 287 | System | 0 | 1 | 2 |
| `ShelterFestivalEngine.cs` | 323 | System | 0 | 0 | 4 |
| `ShelterMuseumSystem.cs` | 539 | System | 0 | 0 | 2 |
| `EndgameSystem.cs` | 323 | System | 0 | 0 | 2 |
| `UnifiedEndingResolver.cs` | 596 | Support | 1 | 0 | 2 |
| `HostCliRegistry.cs` | 1414 | Support | 0 | 0 | 0 |
| `ProceduralEulogyEngine.cs` | 105 | System | 0 | 0 | 2 |
| `LocalizationService.cs` | 595 | Support | 0 | 0 | 0 |
| `PalliativeCareDignityEngine.cs` | 254 | System | 0 | 0 | 0 |
| `VigilCare.cs` | 63 | Support | 0 | 0 | 0 |
| `GraveEpitaphCatalog.cs` | 134 | Catalog | 0 | 0 | 0 |
| `MemorialSave.cs` | 20 | Save | 0 | 0 | 0 |
| `MemorialSystem.cs` | 399 | System | 0 | 0 | 6 |
| `RelationsGriefSink.cs` | 148 | Support | 0 | 0 | 0 |
| `NarrativeDiscoveryCatalog.cs` | 1552 | Catalog | 0 | 0 | 0 |
| `OralLorePerformanceSystem.cs` | 241 | System | 0 | 0 | 2 |
| `SubsystemManifest.cs` | 314 | Support | 0 | 0 | 0 |
| `HeirloomCatalog.cs` | 108 | Catalog | 0 | 0 | 0 |
| `HeirloomSystem.cs` | 486 | System | 0 | 0 | 2 |
| `RetentionPolicy.cs` | 227 | Support | 0 | 0 | 0 |
| `SaveSectionRegistry.cs` | 600 | Support | 0 | 0 | 0 |
| `SaveSlotService.cs` | 1294 | Support | 0 | 0 | 0 |
| `ShelterMachineTellCatalog.cs` | 654 | Catalog | 0 | 0 | 0 |
| `ShelterArchiveSystem.cs` | 434 | System | 0 | 0 | 2 |
| `ShelterDecorSystem.cs` | 340 | System | 0 | 0 | 6 |
| `ShelterSocialDynamicsSystem.cs` | 374 | System | 0 | 0 | 2 |
| `SpiritualCatalogLoader.cs` | 116 | Loader | 0 | 0 | 0 |
| `SpiritualMeaningCoordinator.cs` | 207 | System | 0 | 0 | 2 |
| `SpiritualModels.cs` | 121 | Support | 0 | 0 | 0 |
| `SpiritualRitualCalendarEngine.cs` | 182 | System | 0 | 0 | 0 |
| `SurvivorRelationsSystem.cs` | 497 | System | 1 | 0 | 2 |
| `ISurvivorComponentStore.cs` | 93 | Support | 0 | 0 | 0 |
| `MemorialComponentAdapter.cs` | 139 | Support | 0 | 0 | 1 |
| `MemorialComponentParity.cs` | 388 | Support | 0 | 0 | 0 |
| `MemorialComponentStore.cs` | 301 | Support | 0 | 0 | 5 |
| `SurvivorAggregate.cs` | 194 | Support | 0 | 0 | 1 |
| `SurvivorDeathLegacySystem.cs` | 605 | System | 0 | 0 | 2 |
| `SurvivorEntityStore.cs` | 632 | Support | 2 | 0 | 4 |
| `SurvivorFateSystem.cs` | 476 | System | 0 | 0 | 2 |
| `SurvivorId.cs` | 238 | Support | 0 | 0 | 0 |
| `SurvivorLifecycle.cs` | 392 | Support | 0 | 0 | 0 |
| `SurvivorSocialCoordinator.cs` | 534 | System | 0 | 0 | 16 |
| `CrisisPresentationCoordinator.cs` | 461 | System | 0 | 0 | 0 |
| `PanelRegistryBootstrap.cs` | 274 | Support | 0 | 0 | 0 |

**Totals:** 5 banned refs · 1 empty catches.

## 7. Expanded data & state surface

| Data file | Shape |
|---|---|
| `memorial_rites.json` | object[2 keys] |
| `memorials_expansion_05.json` | array[27] |
| `calcium_hypochlorite_titration_reports.json` | array[7] |
| `memorials_expansion.json` | array[40] |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Culture/` (create if absent) |
| Memorial references | 400 test name references across the tree |
| Data resolution | every rite/memorial record resolves through a loader; unknown ids fail typed |
| Determinism | 5 banned refs to classify |

## 9. Rollout sequence

1. Premise re-check: this plan is data-driven; confirm both data files unchanged.
2. Rite resolution: catalog → loader → selection rule; missing rite degrades to a documented default.
3. Memorial record: create-once semantics; persistence through the owners' sections.
4. Body hygiene hand-off: threshold to Plans 47/103 owners.
5. Bereavement hand-off: typed input to Plan 64.
6. Regression: focused region plus this census regenerated.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Data file | resolves through a loader; records reference real ids |
| Rite selection | documented default on missing entry; no placeholder text |
| Memorial record | created once, persisted, readable |
| Hand-offs | hygiene and bereavement appear only in their owners |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 53. Other plans referencing them: **75**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 11 |
| `PLAN-VERTICAL-CULTURE-04` | 10 |
| `EVIDENCE` | 8 |
| `PLAN-BELIEF-IDEOLOGY-36` | 4 |
| `PLAN-CREATIVE-WORKS-66` | 4 |
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 4 |
| `PLAN-SHELTER-DECOR-TRUTH-225` | 3 |
| `PLAN-CAMPAIGN-FAMILY-TRUTH-272` | 3 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `MMT-123A` | `SurvivorDeathLegacySystem.cs` |
| `MMT-123B` | `CultureCreationSystem.cs` |
| `MMT-123C` | `CultureCreationSystem.cs`, `MemorialComponentAdapter.cs`, `MemorialComponentParity.cs` |
| `MMT-123D` | no name match — resolve at claim time |
| `MMT-123E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 53; intra-domain edges: **57**; isolated files:
**21**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `CampaignCalendar` | `CampaignDayCoordinator` |
| `CampaignDayCoordinator` | `CampaignCalendar` |
| `ContentUtilizationScanner` | `HeirloomCatalog` |
| `ContentUtilizationScanner` | `HeirloomSystem` |
| `ContentUtilizationScanner` | `MemorialSystem` |
| `ContentUtilizationScanner` | `NarrativeDiscoveryCatalog` |
| `ContentUtilizationScanner` | `ShelterSocialDynamicsSystem` |
| `ContentUtilizationScanner` | `SpiritualCatalogLoader` |
| `ContentUtilizationScanner` | `SpiritualMeaningCoordinator` |
| `ContentUtilizationScanner` | `SurvivorRelationsSystem` |
| `CrisisPresentationCoordinator` | `SurvivorFateSystem` |
| `HeirloomSystem` | `HeirloomCatalog` |
| `HeirloomSystem` | `SurvivorRelationsSystem` |
| `ISurvivorComponentStore` | `SurvivorEntityStore` |
| `ISurvivorComponentStore` | `SurvivorId` |
| `MemorialComponentAdapter` | `MemorialComponentStore` |
| `MemorialComponentAdapter` | `SurvivorEntityStore` |
| `MemorialComponentAdapter` | `SurvivorId` |
| `MemorialComponentParity` | `MemorialComponentStore` |
| `MemorialComponentParity` | `SurvivorId` |
| `MemorialComponentStore` | `ISurvivorComponentStore` |
| `MemorialComponentStore` | `MemorialSystem` |
| `MemorialComponentStore` | `SurvivorEntityStore` |
| `MemorialComponentStore` | `SurvivorId` |
| `MemorialSystem` | `GraveEpitaphCatalog` |
| `MemorialSystem` | `ProceduralEulogyEngine` |
| `MemorialSystem` | `SurvivorId` |
| `MemorialSystem` | `SurvivorRelationsSystem` |
| `PalliativeCareDignityEngine` | `SurvivorId` |
| `RelationsGriefSink` | `MemorialSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `SurvivorId` | 14 |
| `MemorialSystem` | 9 |
| `SurvivorRelationsSystem` | 6 |
| `SurvivorEntityStore` | 4 |
| `SurvivorFateSystem` | 3 |
| `HeirloomCatalog` | 2 |
| `ISurvivorComponentStore` | 2 |
| `MemorialComponentStore` | 2 |
| `SurvivorLifecycle` | 2 |
| `CampaignCalendar` | 1 |

**Class split:** hub 13 · sink 9 · source 10 · isolated 21.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 53. Host files: **89** · Test files: **245** · Data files: **3**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 89 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/ShelterOperationsAudioBridge.cs`, `src/Disease/DiseaseHostSession.cs`, `src/Host/CampaignDayPersistenceAdapter.cs` |
| Tests (`Ashfall.Core.Tests/`) | 245 | `Ashfall.Core.Tests/AbyssalAnomaliesCatalogTests.cs`, `Ashfall.Core.Tests/AbyssalAnomaliesRuntimeActivationTests.cs`, `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs`, `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs`, `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 3 | `Assets/StreamingAssets/Data/slice_seven_days.json`, `Assets/StreamingAssets/Data/store_capability_claims.json`, `Assets/StreamingAssets/Data/whitelists/companion_trust_flags.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **43** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `archive_desk` |
| `black_projects_archive` |
| `campaign` |
| `campaign_day` |
| `collectible_discovery` |
| `communication` |
| `death_legacy` |
| `endgame` |
| `expanded_shelter` |
| `grain_milling_archive` |
| `host_event` |
| `hydrogeology_archive` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **32** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--data-integrity-selftest` |
| `--death-legacy-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--memorial-wall-selftest` |
| `--narrative-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--performance-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **24**.

| Event | First declaration |
|---|---|
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCrisisResolved` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnEulogySpoken` | `Assets/Ashfall.Core/Journal/ProceduralEulogyEngine.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnPalliativeAssigned` | `Assets/Ashfall.Core/SickListSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/barter_rules.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/cascade_rules.json` |
| `Assets/StreamingAssets/Data/communication_templates.json` |
| `Assets/StreamingAssets/Data/cultural_archive_tomes.json` |
| `Assets/StreamingAssets/Data/death_legacy_templates.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **14** (192 files, 1574 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |
| `Communication` | 3 | 17 |
| `Culture` | 7 | 40 |
| `Endgame` | 11 | 101 |
| `Legacy` | 1 | 5 |
| `Lifecycle` | 1 | 5 |
| `Localization` | 4 | 21 |
| `Memorial` | 6 | 67 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 1574 cases sit under matching regions — run those first (`Campaign`, `Communication`, `Culture`, `Endgame`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **487**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **43**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `archive_desk` | no |
| `black_projects_archive` | no |
| `campaign` | no |
| `campaign_day` | no |
| `collectible_discovery` | no |
| `communication` | no |
| `death_legacy` | no |
| `endgame` | no |
| `expanded_shelter` | no |
| `grain_milling_archive` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `black_market_debt_event` |
| `narrative` |
| `shelter` |
| `social` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **304**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 13, OPTIONAL 4, UNRESOLVED 8).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `epilogue_chronicle.json` | GAMEPLAY_CONSUMED |
| `expansion_survivor_fields.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `lore_archives.json` | GAMEPLAY_CONSUMED |
| `memorial_rites.json` | UNRESOLVED |

**Verdict:** 8 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_expelled_survivor` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 75
**Surface:** save sections 43 (laddered 0) · RNG streams 4 · host files 18 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MORTUARY-MEMORIAL-TRUTH-123
wave: 10
status: PROPOSED — foreman claim required
packages: MMT-123A, MMT-123B, MMT-123C, MMT-123D, MMT-123E
claim paths:
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 75 other plan(s) name these artifacts (§12)
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
