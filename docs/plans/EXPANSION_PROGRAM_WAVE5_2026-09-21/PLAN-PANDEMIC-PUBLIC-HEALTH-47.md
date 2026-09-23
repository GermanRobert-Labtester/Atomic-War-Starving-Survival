# PLAN-PANDEMIC-PUBLIC-HEALTH-47 — Surveillance, Quarantine, Surge & Countermeasures

**Wave:** 5 (2026-09-21) · **Kind:** MAJOR EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-VERTICAL-BODY-INDUSTRY-05 (clinic continuum),
PLAN-WATER-AGRICULTURE-46, PLAN-SHELTER-ARCHITECTURE-40 (air/ventilation).
**Expanded appendix:** [`PLAN-PANDEMIC-PUBLIC-HEALTH-47_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-PANDEMIC-PUBLIC-HEALTH-47_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's medical/needs/water systems,
each mapped to its parent-plan mechanic row.
**Non-goals:** no real pathogen data, no real vaccines or dosages, no second
disease authority (`DiseaseSystem` remains canonical), no pandemic of a real
named disease.

---

## 1. Outcome

The disease layer is deep (20 conditions across water/air/blood/spore vectors,
diagnostic tells, clinical stages, vector protocols, quarantine tracking,
lethality modulators) but it is mostly **reactive single-case medicine**. This
plan adds **public health**: a shelter can detect an outbreak early, trace it,
intervene, surge, and recover.

Player loop: **watch the sick list → investigate → isolate → treat → protect
the water/air/food → surge the ward → learn for next time**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Surveillance | `SickListSystem`, `DiagnosisKnowledgeStore` | triage, test, record | case counts, stages, uncertainty |
| Contact tracing | relations + duty + rooms | trace, quarantine contacts | exposure map, false alarms |
| Measures | `DiseaseSystem` vectors, sanitation/water/vent | isolate, PPE, disinfect, filter | transmission reduction |
| Surge | `MedicalWardSystem`, triage engines | expand beds, recall staff | capacity, mortality risk |
| Countermeasures | treatments from Plan 60 contract | treat, support, palliate | recovery curves, side effects |
| Vector control | sanitation, water treatment, ventilation | break a vector | source reduction |
| Memory | journal/records | review the outbreak | preparedness, doctrine |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core | `Disease/` (`DiseaseSystem`, `DiseaseCatalog`), `SickListSystem`, `MedicalWardSystem`, `MedicalPipelineCoordinator`, `ClinicalWardTriageEngine`, `DecontaminationSystem`, `AirlockSecuritySystem`, `VentilationSystem`, `SanitationSystem`, `WaterTreatmentSystem` |
| Data | `disease_catalog.json` (20 conditions, schema v3), `autopsy_procedures.json`, `sanitation_facilities.json`, `water_sources.json` |
| Sealed prior | Plan 60 medicine-legible (65/65), Plan 112 disease expansion, quarantine tracking, `foul_water_draw` exposure path, Plan 198 medical record log |
| Contracts | vectors water/air/blood/spore; derived stages; `severitySource` distinguishes dose vs illness; grief dispersion by death quality |
| Selftests | `--disease-selftest`, `--disease-expansion-selftest` |

---

## 3. Packages

### PH-47A — Surveillance and case detection
- Sick-list triage with explicit uncertainty: symptoms → suspected → confirmed;
  diagnosis knowledge improves with study/autopsy; records (Plan 198 bounded
  log) feed counts.
- **Acceptance:** no instant diagnosis; uncertainty visible; counts derive from
  real cases; no duplicate disease store.
- **Verify:** `--disease-selftest` + focused.

### PH-47B — Transmission and contact tracing
- Per-vector transmission uses authored rates; rooms/duties/relations give a
  contact graph for tracing; quarantine of contacts is a policy with morale and
  labour cost.
- **Acceptance:** outbreak growth explainable; tracing reduces spread
  measurably; false positives create drama, not spam.
- **Verify:** disease + relations suites.

### PH-47C — Public health measures
- Isolation rooms, PPE, hand hygiene, water treatment, ventilation, airlocks,
  food safety: each measure has a cost and a measurable vector-specific
  reduction; compliance is a policy, not a button.
- **Acceptance:** each measure's effect proven in a seeded outbreak; stacking
  has diminishing returns; no free prevention.
- **Verify:** sanitation/water/vent focused suites.

### PH-47D — Outbreak surge
- Ward capacity, bed allocation, staff recall, field-hospital expansion,
  triage protocol (sickest-first vs save-most): capacity and mortality model
  from the existing ward/triage owners.
- **Acceptance:** triage choices have explainable outcomes; capacity is a
  visible plan; no hidden death rolls.
- **Verify:** medical ward suites.

### PH-47E — Countermeasures
- Treatment, supportive care, palliative dignity (Plan 60 contract) and
  abstract prophylaxis production (PLAN-SCIENCE-EDUCATION-38 pharma): efficacy
  bands, supply limits, resistance/side-effect tradeoffs.
- **Acceptance:** no real drug names/recipes; supplies consume inventory;
  treatment is never a guaranteed cure.
- **Verify:** medical + pharma focused suites.

### PH-47F — Outbreak memory
- Aftermath: shelter doctrine, survivor trauma/fear, preparedness modifiers,
  and a record of the dead via memorial/archive; immunity where modeled stays
  bounded and fictional.
- **Acceptance:** no permanent immunity god-mode; memorial/archive canonical;
  doctrine read-only.

### PH-47G — Content volumes
- +6 conditions, +10 symptoms/tells, +8 treatment rows, +6 vector protocols,
  +6 preparedness doctrines; fictional/abstract; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Disease spiral kills the run | early warning, mitigation stack, difficulty presets, recovery |
| Real-world disease resemblance | abstract names/descriptions; review |
| Surveillance becomes busywork | daily summary; attention only on thresholds |
| Duplicate medical authority | one `DiseaseSystem`; everything else is a consumer |

## 5. Verification

```bash
godot --headless --path . -- --disease-selftest
godot --headless --path . -- --disease-expansion-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Medical/
bash scripts/run_test.sh Ashfall.Core.Tests/DiseaseSystemTests.cs
```

---

## 6. Expanded census (11 files · 4,411 lines)

Scope: `Assets/Ashfall.Core/Disease/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 2 · Demo 1 · Save 1 · Support 4 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DiseaseCatalog.cs` | 632 | Catalog | — | 0 | 0 | 0 |
| `DiseaseHeadlessDemo.cs` | 490 | Demo | — | 0 | 0 | 11 |
| `DiseaseQuarantineCoordinator.cs` | 376 | System | **yes** | 0 | 0 | 0 |
| `DiseaseSystem.cs` | 1707 | System | — | 1 | 0 | 3 |
| `DiseaseTriage.cs` | 263 | Support | — | 0 | 0 | 0 |
| `IDiseaseOutbreakSource.cs` | 80 | Support | — | 0 | 0 | 0 |
| `PathogenStrainCatalog.cs` | 71 | Catalog | — | 0 | 0 | 0 |
| `PathogenStrainSave.cs` | 113 | Save | — | 0 | 0 | 0 |
| `PathogenStrainSystem.cs` | 361 | System | **yes** | 0 | 0 | 2 |
| `DiseaseAfflictionHandler.cs` | 177 | Support | — | 0 | 0 | 0 |
| `DiseaseProtocolHandler.cs` | 141 | Support | — | 0 | 0 | 0 |

**Totals:** 1 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `pathogens.json` | object[3 keys] |
| `disease_catalog.json` | object[5 keys] |

**State surfaces:** `DiseaseHeadlessDemo.cs`, `DiseaseSystem.cs`, `PathogenStrainSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Disease/` (create if absent) |
| Test references | 74 name references across the test tree |
| Determinism | 1 banned refs to fix or justify |
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

## 11. Tier-2: intra-domain reference graph

Computed across 19 domain files: **66 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `DiseaseSystem.cs` | 1707 | 11 | 11 |
| `DiseaseCatalog.cs` | 632 | 19 | 1 |
| `DiseaseHeadlessDemo.cs` | 490 | 0 | 8 |
| `DiseaseQuarantineCoordinator.cs` | 376 | 0 | 1 |
| `PathogenStrainSystem.cs` | 361 | 2 | 4 |
| `AfflictionId.cs` | 275 | 12 | 0 |
| `DiseaseTriage.cs` | 263 | 2 | 3 |
| `PsychologyAfflictionHandlers.cs` | 262 | 0 | 5 |
| `RadiationAfflictionHandlers.cs` | 239 | 0 | 5 |
| `AfflictionQuestWorkBridge.cs` | 229 | 0 | 0 |

**Highest-coupling files (in×2 + out):**

- `DiseaseCatalog.cs` — in 19, out 1
- `AfflictionContracts.cs` — in 16, out 2
- `DiseaseSystem.cs` — in 11, out 11
- `AfflictionId.cs` — in 12, out 0
- `DiseaseAfflictionHandler.cs` — in 0, out 10
- `DiseaseHeadlessDemo.cs` — in 0, out 8
- `PathogenStrainSystem.cs` — in 2, out 4
- `DiseaseTriage.cs` — in 2, out 3

**Ordering implication:** high in-degree files are depended upon — verify or seal
them first. High out-degree files are consumers whose claims should land after
their dependencies; a file with both is the domain's hub and needs its own
bounded package.

---

## 12. Cross-plan coupling

Domain files: 19. Other plans referencing their names: **11**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-QUARANTINE-STRAIN-TRUTH-241` | 11 |
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 10 |
| `PLAN-TRIO-FAMILY-TRUTH-280` | 9 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 4 |
| `PLAN-THREADING-ASYNCHRONY-72` | 2 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `PLAN-ECOLOGY-WILDLIFE-26` | 1 |
| `PLAN-SILENT-FAILURE-35` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PH-47A` | no name match — resolve at claim time |
| `PH-47B` | no name match — resolve at claim time |
| `PH-47C` | no name match — resolve at claim time |
| `PH-47D` | `IDiseaseOutbreakSource.cs` |
| `PH-47E` | no name match — resolve at claim time |
| `PH-47F` | `IDiseaseOutbreakSource.cs` |
| `PH-47G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 16. Host files: **21** · Test files: **39** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 21 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Disease/DiseaseHostSession.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/DiseaseOutbreakHostAdapter.cs` |
| Tests (`Ashfall.Core.Tests/`) | 39 | `Ashfall.Core.Tests/AutopsyBridgeTests.cs`, `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs`, `Ashfall.Core.Tests/CrisisPresentationCoordinatorTests.cs`, `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`, `Ashfall.Core.Tests/DiseaseCatalogExpansionTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `disease` |
| `expansion_quest` |
| `mental_health_crisis` |
| `pathogen_strains` |
| `psychology` |
| `radiation` |
| `survivor_mental_health` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--disease-expansion-selftest` |
| `--disease-selftest` |
| `--ice-road-tick-demo` |
| `--personal-quest-selftest` |
| `--psychology-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **17**.

| Event | First declaration |
|---|---|
| `OnBlightOutbreak` | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` |
| `OnHealthDeltaRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnMentalHealthChanged` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnOutbreakContained` | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` |
| `OnOutbreakDeclared` | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` |
| `OnPathogenExposure` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` |
| `OnQuarantineEnded` | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` |
| `OnQuarantineStarted` | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnQuestCompleted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestFailed` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **11**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/decontamination_protocol_catalog.json` |
| `Assets/StreamingAssets/Data/disease_catalog.json` |
| `Assets/StreamingAssets/Data/documents/vel_triage_log_names.json` |
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/narrative/quest_narrative_documents.json` |
| `Assets/StreamingAssets/Data/narrative/radiation_survey_readings_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/surface_radiation_topo_sheets.json` |
| `Assets/StreamingAssets/Data/psychology_profiles.json` |
| `Assets/StreamingAssets/Data/quest_templates.json` |
| `Assets/StreamingAssets/Data/radiation_economy_social.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (10 files, 78 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Radiation` | 10 | 78 |

**Verdict:** 78 cases sit under matching regions — run those first (`Radiation`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **32**
(8 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/DiseaseOutbreakHostAdapter.cs` |
| `src/Host/DiseaseSaveStore.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/HostSessionContracts.cs` |
| `src/Host/MentalHealthCrisisHostSession.cs` |
| `src/Host/PathogenStrainSaveStore.cs` |
| `src/Host/PersonalQuestHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **7**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `disease` | no |
| `expansion_quest` | no |
| `mental_health_crisis` | no |
| `pathogen_strains` | no |
| `psychology` | no |
| `radiation` | no |
| `survivor_mental_health` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **6**.

| Stream |
|---|
| `aquaponics_disease` |
| `disease` |
| `psychology` |
| `psychology_arc_behavior` |
| `psychology_arc_trigger` |
| `psychology_recovery` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **7**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 2, OPTIONAL 2).

| Catalog | Classification |
|---|---|
| `decontamination_protocol_catalog.json` | GAMEPLAY_CONSUMED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `documents/vel_triage_log_names.json` | OPTIONAL |
| `moral_choice_quest_stubs.json` | OPTIONAL |
| `narrative/quest_narrative_documents.json` | CODEX_ONLY |
| `narrative/radiation_survey_readings_batch_2.json` | CODEX_ONLY |
| `narrative/surface_radiation_topo_sheets.json` | CODEX_ONLY |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 11
**Surface:** save sections 7 (laddered 0) · RNG streams 6 · host files 18 · catalogs 18 · test regions 1 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PANDEMIC-PUBLIC-HEALTH-47
wave: —
status: PROPOSED — foreman claim required
packages: PH-47A, PH-47B, PH-47C, PH-47D, PH-47E, PH-47F, PH-47G
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/DiseaseOutbreakHostAdapter.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/decontamination_protocol_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Radiation/
  - godot --headless --path . -- --disease-expansion-selftest
dependencies:
  - coordinate: 11 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
