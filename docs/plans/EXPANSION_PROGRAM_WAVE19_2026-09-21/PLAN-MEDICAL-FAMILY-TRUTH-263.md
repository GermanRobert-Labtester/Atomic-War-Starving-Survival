# PLAN-MEDICAL-FAMILY-TRUTH-263 — Affliction Contracts & Handler Wiring

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-PANDEMIC-PUBLIC-HEALTH-47, PLAN-ACUTE-TRAUMA-CARE-124, PLAN-QUARANTINE-STRAIN-TRUTH-241.
**Non-goals:** no new conditions; the family is audited for handler coverage and
bridge integrity.

## 1. Outcome
**29 `Medical/` files** are referenced by no plan, including the affliction
architecture: `AfflictionContracts`, `AfflictionId`, `DiseaseAfflictionHandler`,
`ChemicalDependencyAfflictionHandler`, `AfflictionDutyBridge`,
`AfflictionQuestWorkBridge`, `MedicalConditionResolver`. This is a
handler/bridge family — exactly where a broken link is silent.

| Deliverable | Detail |
|---|---|
| Handler map | each affliction id ↔ handler ↔ bridge(s) ↔ consumer; a missing handler is a typed finding |
| Bridge integrity | duty/quest work bridges tested with one fixture per bridge |
| Resolver truth | the condition resolver's inputs and precedence documented and tested |
| Coverage | every affliction id in data resolves to a handler (or an explicit no-op) |
| Test presence | handlers without direct tests get a minimal fixture |

## 2. Evidence
- 29 `Medical/` basenames absent from every plan body (Wave 19 file-level audit).
- Plans 47/124/241 own the conditions and facilities; this plan owns the handler wiring between them.
- Plan 90's schema stage validates affliction data shape.

## 3. Packages
- **MFT-263A** handler/bridge map + missing-handler report.
- **MFT-263B** bridge fixtures (duty, quest-work).
- **MFT-263C** resolver precedence tests.
- **MFT-263D** id→handler coverage test.
- **MFT-263E** minimal fixtures for un-tested handlers.

## 4. Acceptance & verification
- Every data affliction id resolves; bridges pass their fixtures.
- Resolver precedence is documented and tested.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`.

## 5. Risks
Silent no-op handlers → coverage test reports them.
Bridge duplication → bridges are the only cross-system path; the map asserts it.

---

## 6. Expanded census (51 files in scope · 13,236 lines · 0 still unmentioned)

Scope: files under `Assets/Ashfall.Core/Medical/`. The plan's premise was the family **at authoring time**; later
plans have since referenced some files, so the table marks each row
`still unmentioned` or `since-mentioned`. Both are in scope for this plan: the
since-mentioned files are re-verified for owner discipline, and the still-
unmentioned ones are the original audit target. Class distribution:
Support 23 · System 20 · Catalog 3 · Save 2 · Demo 1 · Loader 1 · DTO/Type 1.

| File | Lines | Class | Banned | Empty catches | Capture/Restore | Status |
|---|---:|---|---:|---:|---:|---|
| `AdvancedSurgicalWardSystem.cs` | 381 | System | 0 | 0 | 2 | since-mentioned |
| `AfflictionContracts.cs` | 128 | Support | 0 | 0 | 0 | since-mentioned |
| `AfflictionDutyBridge.cs` | 91 | Support | 0 | 0 | 0 | since-mentioned |
| `AfflictionId.cs` | 275 | Support | 0 | 0 | 0 | since-mentioned |
| `AfflictionQuestWorkBridge.cs` | 229 | Support | 0 | 1 | 0 | since-mentioned |
| `AmputationSystem.cs` | 528 | System | 0 | 0 | 2 | since-mentioned |
| `BionicsSystem.cs` | 805 | System | 0 | 0 | 2 | since-mentioned |
| `ChemicalDependencyAfflictionHandler.cs` | 229 | Support | 0 | 0 | 0 | since-mentioned |
| `ChemicalDependencySystem.cs` | 536 | System | 0 | 0 | 2 | since-mentioned |
| `ChronicConditionSystem.cs` | 346 | System | 0 | 1 | 2 | since-mentioned |
| `ClinicalWardTriageEngine.cs` | 307 | System | 0 | 0 | 0 | since-mentioned |
| `DependencyTaperWithdrawalEngine.cs` | 268 | System | 0 | 0 | 0 | since-mentioned |
| `DiagnosisKnowledgeStore.cs` | 190 | Support | 0 | 0 | 2 | since-mentioned |
| `DiseaseAfflictionHandler.cs` | 177 | Support | 0 | 0 | 0 | since-mentioned |
| `DiseaseProtocolHandler.cs` | 141 | Support | 0 | 0 | 0 | since-mentioned |
| `HealthHistorySystem.cs` | 411 | System | 0 | 0 | 2 | since-mentioned |
| `LyophilizationSystem.cs` | 332 | System | 0 | 0 | 2 | since-mentioned |
| `MedicalConditionResolver.cs` | 118 | Support | 1 | 0 | 0 | since-mentioned |
| `MedicalHeadlessDemo.cs` | 108 | Demo | 0 | 0 | 5 | since-mentioned |
| `MedicalPipelineCoordinator.cs` | 747 | System | 0 | 0 | 10 | since-mentioned |
| `MedicalPipelineSave.cs` | 48 | Save | 0 | 0 | 0 | since-mentioned |
| `MedicalProcedureSchedule.cs` | 267 | Support | 0 | 0 | 2 | since-mentioned |
| `MedicalRecordLog.cs` | 143 | Support | 0 | 0 | 2 | since-mentioned |
| `MedicalReservationLedger.cs` | 185 | Support | 0 | 0 | 2 | since-mentioned |
| `MedicalTextCatalog.cs` | 177 | Catalog | 0 | 0 | 0 | since-mentioned |
| `MedicalTreatmentCatalog.cs` | 221 | Catalog | 0 | 0 | 0 | since-mentioned |
| `MedicalWardPipelineBridge.cs` | 88 | Support | 0 | 0 | 0 | since-mentioned |
| `MedicalWardSave.cs` | 80 | Save | 0 | 0 | 0 | since-mentioned |
| `MedicalWardSystem.cs` | 363 | System | 0 | 0 | 6 | since-mentioned |
| `MicrofluidicDiagnosticCatalogLoader.cs` | 107 | Loader | 0 | 0 | 0 | since-mentioned |
| `MicrofluidicDiagnosticEngine.cs` | 609 | System | 0 | 0 | 2 | since-mentioned |
| `MutationSystem.cs` | 352 | System | 0 | 0 | 2 | since-mentioned |
| `NarcoticsSystem.cs` | 368 | System | 0 | 0 | 2 | since-mentioned |
| `PalliativeCareDignityEngine.cs` | 254 | System | 0 | 0 | 0 | since-mentioned |
| `PatientRecord.cs` | 233 | Support | 0 | 0 | 0 | since-mentioned |
| `PatientRecordIntegrityValidator.cs` | 195 | Support | 0 | 0 | 0 | since-mentioned |
| `PharmaceuticalTabletEngine.cs` | 721 | System | 1 | 0 | 4 | since-mentioned |
| `ProstheticConditionWearEngine.cs` | 157 | System | 0 | 0 | 0 | since-mentioned |
| `PsychologyAfflictionHandlers.cs` | 262 | Support | 0 | 0 | 0 | since-mentioned |
| `RadiationAfflictionHandlers.cs` | 239 | Support | 0 | 0 | 0 | since-mentioned |
| `RehabilitationProgressionEngine.cs` | 112 | System | 0 | 0 | 0 | since-mentioned |
| `RehabilitationSlateProjection.cs` | 126 | Support | 0 | 0 | 0 | since-mentioned |
| `RespiratoryAfflictionHandler.cs` | 137 | Support | 0 | 0 | 0 | since-mentioned |
| `RespiratoryDegenerationSystem.cs` | 287 | System | 0 | 0 | 2 | since-mentioned |
| `StressRelapseRules.cs` | 83 | Support | 0 | 0 | 0 | since-mentioned |

… and 6 more files in scope.

**Census totals:** 2 banned nondeterministic references · 2 empty-catch sites · 21 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `medical_texts.json` | object[3 keys] |
| `medical_record_templates.json` | object[2 keys] |
| `dweller_medical_casebook.json` | object[3 keys] |
| `medical_documents_expansion.json` | object[4 keys] |

**State surfaces (capture/restore present):**

- `AdvancedSurgicalWardSystem.cs`
- `AmputationSystem.cs`
- `BionicsSystem.cs`
- `ChemicalDependencySystem.cs`
- `ChronicConditionSystem.cs`
- `DiagnosisKnowledgeStore.cs`
- `HealthHistorySystem.cs`
- `LyophilizationSystem.cs`
- `MedicalHeadlessDemo.cs`
- `MedicalPipelineCoordinator.cs`
- `MedicalProcedureSchedule.cs`
- `MedicalRecordLog.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Medical/` |
| Files referenced by tests | 168 name references across the test tree |
| Determinism scan | 2 banned references to fix or justify |
| Failure scan | 2 empty-catch sites to route through Plan 35's rules |
| Drift | 51 of 51 files became plan-referenced since authoring — re-verify their owners |
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
(45 files). Other plans referencing those names: **23**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 12 |
| `PLAN-PANDEMIC-PUBLIC-HEALTH-47` | 11 |
| `PLAN-ORPHAN-SEAL-01` | 8 |
| `EVIDENCE` | 7 |
| `PLAN-ACUTE-TRAUMA-CARE-124` | 7 |
| `PLAN-THREADING-ASYNCHRONY-72` | 6 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 5 |
| `PLAN-BIONICS-ENHANCEMENT-78` | 5 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MFT-263A` | `ChemicalDependencyAfflictionHandler.cs`, `DiseaseAfflictionHandler.cs`, `DiseaseProtocolHandler.cs` |
| `MFT-263B` | `AfflictionQuestWorkBridge.cs`, `AfflictionDutyBridge.cs`, `MedicalWardPipelineBridge.cs` |
| `MFT-263C` | `MedicalConditionResolver.cs` |
| `MFT-263D` | `ChemicalDependencyAfflictionHandler.cs`, `DiseaseAfflictionHandler.cs`, `DiseaseProtocolHandler.cs` |
| `MFT-263E` | `PsychologyAfflictionHandlers.cs`, `RadiationAfflictionHandlers.cs` |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 45; intra-domain edges: **50**; isolated files:
**10**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `AfflictionContracts` | `AfflictionId` |
| `BionicsSystem` | `AmputationSystem` |
| `BionicsSystem` | `MedicalPipelineCoordinator` |
| `ChemicalDependencyAfflictionHandler` | `AfflictionId` |
| `ChemicalDependencyAfflictionHandler` | `ChemicalDependencySystem` |
| `ChemicalDependencyAfflictionHandler` | `MedicalProcedureSchedule` |
| `ChemicalDependencyAfflictionHandler` | `MedicalTreatmentCatalog` |
| `ChemicalDependencySystem` | `StressRelapseRules` |
| `ClinicalWardTriageEngine` | `AdvancedSurgicalWardSystem` |
| `ClinicalWardTriageEngine` | `MedicalWardSystem` |
| `DependencyTaperWithdrawalEngine` | `ChemicalDependencySystem` |
| `DependencyTaperWithdrawalEngine` | `PharmaceuticalTabletEngine` |
| `DiseaseAfflictionHandler` | `AfflictionId` |
| `DiseaseAfflictionHandler` | `MedicalPipelineCoordinator` |
| `DiseaseAfflictionHandler` | `MedicalTreatmentCatalog` |
| `DiseaseProtocolHandler` | `MedicalPipelineCoordinator` |
| `DiseaseProtocolHandler` | `MedicalTreatmentCatalog` |
| `LyophilizationSystem` | `MedicalPipelineCoordinator` |
| `MedicalConditionResolver` | `MedicalTextCatalog` |
| `MedicalConditionResolver` | `MedicalTreatmentCatalog` |
| `MedicalHeadlessDemo` | `ChemicalDependencySystem` |
| `MedicalPipelineCoordinator` | `AfflictionId` |
| `MedicalPipelineCoordinator` | `DiagnosisKnowledgeStore` |
| `MedicalPipelineCoordinator` | `MedicalProcedureSchedule` |
| `MedicalPipelineCoordinator` | `MedicalRecordLog` |
| `MedicalPipelineCoordinator` | `MedicalReservationLedger` |
| `MedicalPipelineCoordinator` | `MedicalTreatmentCatalog` |
| `MedicalRecordLog` | `ChronicConditionSystem` |
| `MedicalRecordLog` | `HealthHistorySystem` |
| `MedicalTreatmentCatalog` | `AfflictionId` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `MedicalTreatmentCatalog` | 12 |
| `AfflictionId` | 9 |
| `MedicalPipelineCoordinator` | 6 |
| `ChemicalDependencySystem` | 5 |
| `MedicalProcedureSchedule` | 3 |
| `DiagnosisKnowledgeStore` | 2 |
| `MedicalWardSystem` | 2 |
| `AdvancedSurgicalWardSystem` | 1 |
| `AmputationSystem` | 1 |
| `ChronicConditionSystem` | 1 |

**Class split:** hub 6 · sink 12 · source 17 · isolated 10.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 45. Host files: **35** · Test files: **66** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 35 | `src/Audio/AudioSelfTest.cs`, `src/Audio/ExpansionAudioBridge.cs`, `src/Host/AutopsyHostSession.cs`, `src/Host/ChemicalDependencyHostSession.cs`, `src/Host/HostCli.PanelTests.cs` |
| Tests (`Ashfall.Core.Tests/`) | 66 | `Ashfall.Core.Tests/AutopsyBridgeTests.cs`, `Ashfall.Core.Tests/AutopsyIntegrationTests.cs`, `Ashfall.Core.Tests/AutopsySystemTests.cs`, `Ashfall.Core.Tests/ChemicalDependencyCommandTests.cs`, `Ashfall.Core.Tests/ChemicalDependencySystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/narrative/dweller_dependency_backstories.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **26** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `amputation` |
| `bionics` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `disease` |
| `dose_ledger` |
| `duty_roster` |
| `equipment_condition` |
| `expansion_quest` |
| `knowledge` |
| `lyophilization` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **20** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--chemical-dependency-save-selftest` |
| `--data-integrity-selftest` |
| `--disease-expansion-selftest` |
| `--disease-selftest` |
| `--dose-ledger-selftest` |
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--ice-road-tick-demo` |
| `--ledger-debt-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **34**.

| Event | First declaration |
|---|---|
| `OnChapterAdvanced` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` |
| `OnChronicFibrosisMarked` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnChronicIllnessRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnDayAdvanced` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnDependencyFormed` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnDependencyReFormedByStress` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnDependencyRisk` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnDeviceConditionChanged` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/barter_rules.json` |
| `Assets/StreamingAssets/Data/bionics.json` |
| `Assets/StreamingAssets/Data/cascade_rules.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/chronic_conditions.json` |
| `Assets/StreamingAssets/Data/decontamination_protocol_catalog.json` |
| `Assets/StreamingAssets/Data/disease_catalog.json` |
| `Assets/StreamingAssets/Data/documents/vel_triage_log_names.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **4** (75 files, 645 cases).

| Region | Files | Cases |
|---|---:|---:|
| `DutyRoster` | 5 | 49 |
| `Medical` | 49 | 435 |
| `Progression` | 11 | 83 |
| `Radiation` | 10 | 78 |

**Verdict:** 645 cases sit under matching regions — run those first (`DutyRoster`, `Medical`, `Progression`, `Radiation`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **255**
(27 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipSaveStore.cs` |
| `src/Host/AquaponicsSaveStore.cs` |
| `src/Host/ArchaeologySaveStore.cs` |
| `src/Host/ArmoredCrawlerSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **26**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `amputation` | no |
| `bionics` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `disease` | no |
| `dose_ledger` | yes |
| `duty_roster` | no |
| `equipment_condition` | no |
| `expansion_quest` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **13**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `agriculture_mutation` |
| `aquaponics_disease` |
| `bionics` |
| `disease` |
| `duty_roster` |
| `medical` |
| `medical_microfluidic_diagnostics` |
| `mineral_chemical` |
| `psychology` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **34**
(CODEX_ONLY 14, GAMEPLAY_CONSUMED 14, OPTIONAL 3, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `decontamination_protocol_catalog.json` | GAMEPLAY_CONSUMED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `documents/vel_triage_log_names.json` | OPTIONAL |
| `duty_roster_locations.json` | GAMEPLAY_CONSUMED |
| `duty_roster_marks.json` | GAMEPLAY_CONSUMED |
| `duty_roster_quests.json` | GAMEPLAY_CONSUMED |
| `duty_roster_seasons.json` | GAMEPLAY_CONSUMED |

**Verdict:** 3 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 23
**Surface:** save sections 26 (laddered 1) · RNG streams 13 · host files 22 · catalogs 22 · test regions 4 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MEDICAL-FAMILY-TRUTH-263
wave: 19
status: PROPOSED — foreman claim required
packages: MFT-263A, MFT-263B, MFT-263C, MFT-263D, MFT-263E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/barter_rules.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 23 other plan(s) name these artifacts (§12)
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
