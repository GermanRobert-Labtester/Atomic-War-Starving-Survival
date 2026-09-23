# PLAN-CORE-ROOT-FAMILY-TRUTH-262 — The 59 Unreferenced Root Files

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-CORE-ONLY-REGISTRY-11, PLAN-INTEGRATION-KIT-02, PLAN-DEV-TOOLING-TRUTH-75.
**Non-goals:** no new systems; the root family is audited for ownership,
compile role, and test presence only.

## 1. Outcome
**59 files under `Assets/Ashfall.Core/` itself** are referenced by no plan —
loaders (`ArchiveInkCatalogLoader`, `AtmosphereCatalogLoader`), demos
(`BrineWaterHeadlessDemo`, `CensusHeadlessDemo`, `Cluster12CHeadlessDemo`),
tooling (`CatalogIntegrityCheckers`), and tuning (`CohortTuning`). Root files
are easy to leave ownerless; this plan assigns each one.

| Deliverable | Detail |
|---|---|
| Root census | each file classified: loader / demo / tooling / tuning / type |
| Owner map | each file points to the system it supports (or is flagged ownerless) |
| Demo truth | each headless demo names its verb/entry and whether the verb exists (Plan 86 pattern) |
| Tooling truth | `CatalogIntegrityCheckers` etc. say where they are invoked; an uninvoked checker is a finding |
| Compile/test record | test presence per file; missing fixtures listed |

## 2. Evidence
- 59 root basenames absent from every plan body (Wave 19 file-level audit).
- Plan 11's registry rules: a Core type with no live consumer is not reachable.
- Plan 75 owns dev tooling surfaces the demos belong to.

## 3. Packages
- **CRF-262A** root census + owner map.
- **CRF-262B** demo→verb verification.
- **CRF-262C** tooling invocation audit.
- **CRF-262D** fixture pass for un-tested files.
- **CRF-262E** ownerless report.

## 4. Acceptance & verification
- Every root file has an owner or an ownerless finding; demos resolve to real verbs.
- Inspected checkers run or are reported; fixtures pass.
- `bash scripts/run_test.sh` on the core-root region (create if absent).

## 5. Risks
Demo sprawl → verbs verified; dead demos reported.
Tooling rot → invocation audit is the guard.

---

## 6. Expanded census (135 files in scope · 38,938 lines · 5 still unmentioned)

Scope: files directly under `Assets/Ashfall.Core/`. The plan's premise was the family **at authoring time**; later
plans have since referenced some files, so the table marks each row
`still unmentioned` or `since-mentioned`. Both are in scope for this plan: the
since-mentioned files are re-verified for owner discipline, and the still-
unmentioned ones are the original audit target. Class distribution:
System 53 · Support 43 · Catalog 14 · Demo 12 · Loader 8 · Save 4 · DTO/Type 1.

| File | Lines | Class | Banned | Empty catches | Capture/Restore | Status |
|---|---:|---|---:|---:|---:|---|
| `ActionResult.cs` | 202 | Support | 1 | 0 | 0 | since-mentioned |
| `AirlockSecuritySystem.cs` | 229 | System | 0 | 0 | 2 | since-mentioned |
| `ApprenticeshipSystem.cs` | 496 | System | 0 | 0 | 2 | since-mentioned |
| `ArchiveDeskSystem.cs` | 205 | System | 0 | 0 | 2 | since-mentioned |
| `ArchiveInkCatalogLoader.cs` | 52 | Loader | 0 | 0 | 0 | since-mentioned |
| `AtmosphereCatalogLoader.cs` | 105 | Loader | 0 | 0 | 0 | since-mentioned |
| `AtmosphereTextSystem.cs` | 266 | System | 0 | 0 | 0 | since-mentioned |
| `AtmosphericCondenserSystem.cs` | 265 | System | 0 | 0 | 2 | since-mentioned |
| `AudioConditionSystem.cs` | 128 | System | 0 | 0 | 2 | since-mentioned |
| `AutopsyProcedureCatalogLoader.cs` | 52 | Loader | 0 | 0 | 0 | since-mentioned |
| `AutopsySystem.cs` | 246 | System | 0 | 0 | 2 | since-mentioned |
| `BrineWaterHeadlessDemo.cs` | 131 | Demo | 0 | 0 | 3 | since-mentioned |
| `BrineWaterSystem.cs` | 231 | System | 0 | 0 | 2 | since-mentioned |
| `CatalogFileSystem.cs` | 47 | System | 0 | 0 | 0 | since-mentioned |
| `CatalogIntegrityCheckers.cs` | 215 | Support | 0 | 0 | 0 | since-mentioned |
| `CatalogIntegrityRules.cs` | 390 | Support | 0 | 0 | 0 | since-mentioned |
| `CatalogIntegrityValidator.cs` | 3141 | Support | 0 | 0 | 0 | since-mentioned |
| `CensusClaimSystem.cs` | 357 | System | 0 | 0 | 2 | since-mentioned |
| `CensusHeadlessDemo.cs` | 128 | Demo | 0 | 0 | 3 | since-mentioned |
| `Cluster12CHeadlessDemo.cs` | 110 | Demo | 0 | 0 | 2 | since-mentioned |
| `CohortSystem.cs` | 291 | System | 0 | 0 | 2 | since-mentioned |
| `CohortTuning.cs` | 38 | Support | 0 | 0 | 0 | since-mentioned |
| `CollectibleCatalog.cs` | 97 | Catalog | 0 | 0 | 0 | since-mentioned |
| `CollectibleDiscoveryState.cs` | 302 | DTO/Type | 0 | 0 | 2 | since-mentioned |
| `ContractorRosterSystem.cs` | 236 | System | 0 | 0 | 2 | since-mentioned |
| `CrossingArbitrationHeadlessDemo.cs` | 166 | Demo | 0 | 0 | 3 | since-mentioned |
| `CrossingArbitrationSystem.cs` | 490 | System | 0 | 0 | 2 | since-mentioned |
| `CrossingCatalog.cs` | 430 | Catalog | 0 | 0 | 0 | since-mentioned |
| `CrossingHeadlessDemo.cs` | 113 | Demo | 0 | 0 | 2 | since-mentioned |
| `CrossingSession.cs` | 48 | Support | 0 | 0 | 0 | since-mentioned |
| `CryogenicAirSeparationSystem.cs` | 275 | System | 0 | 0 | 2 | since-mentioned |
| `DebtBountyRecord.cs` | 78 | Support | 0 | 0 | 0 | since-mentioned |
| `DebtConsequenceDispatcher.cs` | 317 | Support | 0 | 0 | 2 | since-mentioned |
| `DebtConsequenceHostBridge.cs` | 328 | Support | 0 | 0 | 2 | since-mentioned |
| `DebtTemplateCatalog.cs` | 191 | Catalog | 0 | 0 | 0 | since-mentioned |
| `DeconProtocolCatalogLoader.cs` | 130 | Loader | 0 | 0 | 0 | since-mentioned |
| `DecontaminationSystem.cs` | 643 | System | 0 | 0 | 2 | since-mentioned |
| `DeepCoastHeadlessDemo.cs` | 380 | Demo | 0 | 0 | 7 | since-mentioned |
| `DeepWellSystem.cs` | 247 | System | 0 | 0 | 2 | since-mentioned |
| `District8DeepCoastSystem.cs` | 795 | System | 0 | 0 | 3 | since-mentioned |
| `DoseContentCatalog.cs` | 224 | Catalog | 0 | 0 | 0 | since-mentioned |
| `DoseLedgerSave.cs` | 164 | Save | 0 | 0 | 12 | since-mentioned |
| `DoseLedgerSystem.cs` | 333 | System | 0 | 0 | 2 | since-mentioned |
| `DoseQuestMigration.cs` | 162 | Support | 0 | 0 | 0 | since-mentioned |
| `DoseRegistersCatalog.cs` | 115 | Catalog | 0 | 0 | 0 | since-mentioned |

… and 90 more files in scope.

**Census totals:** 8 banned nondeterministic references · 0 empty-catch sites · 71 files with capture/restore methods.

## 7. Expanded data & state surface

No catalog in this family's name space; the family is code/support, so no data binding is implied.

**State surfaces (capture/restore present):**

- `AirlockSecuritySystem.cs`
- `ApprenticeshipSystem.cs`
- `ArchiveDeskSystem.cs`
- `AtmosphericCondenserSystem.cs`
- `AudioConditionSystem.cs`
- `AutopsySystem.cs`
- `BrineWaterHeadlessDemo.cs`
- `BrineWaterSystem.cs`
- `CensusClaimSystem.cs`
- `CensusHeadlessDemo.cs`
- `Cluster12CHeadlessDemo.cs`
- `CohortSystem.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/CoreRoot/` (create if absent) |
| Files referenced by tests | 945 name references across the test tree |
| Determinism scan | 8 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Drift | 130 of 135 files became plan-referenced since authoring — re-verify their owners |
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
(45 files). Other plans referencing those names: **32**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-DEV-TOOLING-TRUTH-75` | 6 |
| `PLAN-DATA-AUTHORITY-14` | 3 |
| `PLAN-EVENT-WIRING-21` | 3 |
| `PLAN-REFERENCE-INTEGRITY-34` | 3 |
| `PLAN-WATER-AGRICULTURE-46` | 3 |
| `PLAN-MUTATION-HEREDITY-81` | 3 |
| `PLAN-MARITIME-DEEPWATER-27` | 2 |
| `PLAN-CRIME-SYNDICATES-44` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CRF-262A` | `CensusClaimSystem.cs`, `CensusHeadlessDemo.cs` |
| `CRF-262B` | no name match — resolve at claim time |
| `CRF-262C` | no name match — resolve at claim time |
| `CRF-262D` | `CatalogFileSystem.cs` |
| `CRF-262E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 45; intra-domain edges: **32**; isolated files:
**8**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `AirlockSecuritySystem` | `ActionResult` |
| `ApprenticeshipSystem` | `ActionResult` |
| `ArchiveDeskSystem` | `ActionResult` |
| `ArchiveInkCatalogLoader` | `ArchiveDeskSystem` |
| `AtmosphereCatalogLoader` | `AtmosphereTextSystem` |
| `AtmosphericCondenserSystem` | `ActionResult` |
| `AudioConditionSystem` | `ActionResult` |
| `AutopsyProcedureCatalogLoader` | `AutopsySystem` |
| `AutopsySystem` | `ActionResult` |
| `BrineWaterHeadlessDemo` | `BrineWaterSystem` |
| `CatalogIntegrityCheckers` | `CatalogIntegrityRules` |
| `CatalogIntegrityValidator` | `CatalogFileSystem` |
| `CensusHeadlessDemo` | `CensusClaimSystem` |
| `CohortSystem` | `CohortTuning` |
| `CohortSystem` | `DoseLedgerSave` |
| `ContractorRosterSystem` | `ActionResult` |
| `CrossingArbitrationHeadlessDemo` | `CrossingArbitrationSystem` |
| `CrossingHeadlessDemo` | `CrossingSession` |
| `CrossingSession` | `CrossingCatalog` |
| `DebtConsequenceDispatcher` | `DebtTemplateCatalog` |
| `DebtConsequenceHostBridge` | `DebtConsequenceDispatcher` |
| `DecontaminationSystem` | `ActionResult` |
| `DecontaminationSystem` | `AirlockSecuritySystem` |
| `DeepCoastHeadlessDemo` | `BrineWaterSystem` |
| `DeepCoastHeadlessDemo` | `CensusClaimSystem` |
| `DeepCoastHeadlessDemo` | `District8DeepCoastSystem` |
| `DeepWellSystem` | `ActionResult` |
| `District8DeepCoastSystem` | `ActionResult` |
| `DoseLedgerSave` | `CohortSystem` |
| `DoseLedgerSave` | `DoseLedgerSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `ActionResult` | 10 |
| `BrineWaterSystem` | 2 |
| `CensusClaimSystem` | 2 |
| `DoseLedgerSave` | 2 |
| `AirlockSecuritySystem` | 1 |
| `ArchiveDeskSystem` | 1 |
| `AtmosphereTextSystem` | 1 |
| `AutopsySystem` | 1 |
| `CatalogFileSystem` | 1 |
| `CatalogIntegrityRules` | 1 |

**Class split:** hub 9 · sink 11 · source 17 · isolated 8.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 45. Host files: **105** · Test files: **234** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 105 | `src/Audio/AudioConditionHostBridge.cs`, `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioManager.cs`, `src/Audio/AudioSelfTest.cs`, `src/Dose/DoseRegisterSurface.cs` |
| Tests (`Ashfall.Core.Tests/`) | 234 | `Ashfall.Core.Tests/ActionResultTests.cs`, `Ashfall.Core.Tests/AirlockSecurityCommandTests.cs`, `Ashfall.Core.Tests/AirlockSecurityIntegrationTests.cs`, `Ashfall.Core.Tests/AirlockSecuritySystemTests.cs`, `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/shelter_machine_identities.json`, `Assets/StreamingAssets/Data/whitelists/companion_trust_flags.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **23** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `airlock_security` |
| `apprenticeship` |
| `archive_desk` |
| `autopsy` |
| `black_projects_archive` |
| `collectible_discovery` |
| `contractor_roster` |
| `crossing` |
| `cryogenic_air_separation` |
| `decontamination` |
| `deep_well` |
| `dose_ledger` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **25** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--arbitration-selftest` |
| `--atmosphere-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--brine-selftest` |
| `--census-selftest` |
| `--cluster-selftest` |
| `--crossing-selftest` |
| `--data-integrity-selftest` |
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **34**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnAutopsyChanged` | `Assets/Ashfall.Core/AutopsySystem.cs` |
| `OnBountyRequested` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnBountyRequestedDetailed` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnCensusUpdated` | `Assets/Ashfall.Core/CensusClaimSystem.cs` |
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/apprenticeship_catalog.json` |
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/atmosphere_profiles.json` |
| `Assets/StreamingAssets/Data/atmospheric_sounding_catalog.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/autopsy_procedures.json` |
| `Assets/StreamingAssets/Data/barter_rules.json` |
| `Assets/StreamingAssets/Data/bounty_board.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **4** (16 files, 134 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `DutyRoster` | 5 | 49 |
| `NarrativeConsequence` | 1 | 20 |
| `Water` | 5 | 37 |

**Verdict:** 134 cases sit under matching regions — run those first (`Audio`, `DutyRoster`, `NarrativeConsequence`, `Water`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **222**
(35 of them panels/HUD).

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
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Dose/DoseRegisterSurface.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **23**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `airlock_security` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `autopsy` | no |
| `black_projects_archive` | no |
| `collectible_discovery` | no |
| `contractor_roster` | no |
| `crossing` | no |
| `cryogenic_air_separation` | no |
| `decontamination` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **5**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `deep_coast` |
| `duty_roster` |
| `wildlife_migration` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **49**
(CODEX_ONLY 21, GAMEPLAY_CONSUMED 21, OPTIONAL 3, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `atmospheric_sounding_catalog.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `bounty_board.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_honored_debt` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 32
**Surface:** save sections 23 (laddered 1) · RNG streams 5 · host files 19 · catalogs 22 · test regions 4 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CORE-ROOT-FAMILY-TRUTH-262
wave: 19
status: PROPOSED — foreman claim required
packages: CRF-262A, CRF-262B, CRF-262C, CRF-262D, CRF-262E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/apprenticeship_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --arbitration-selftest
dependencies:
  - coordinate: 32 other plan(s) name these artifacts (§12)
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
