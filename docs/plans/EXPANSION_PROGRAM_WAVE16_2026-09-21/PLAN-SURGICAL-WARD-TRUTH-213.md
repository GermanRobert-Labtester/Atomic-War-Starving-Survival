# PLAN-SURGICAL-WARD-TRUTH-213 — Ward Capacity, Sterility & Post-Operative Care

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ACUTE-TRAUMA-CARE-124, PLAN-PANDEMIC-PUBLIC-HEALTH-47, PLAN-SANATORIUM-TRUTH-144, PLAN-SHELTER-CAPACITY-AUTHORITY-103.
**Non-goals:** no wound/triage model (Plan 124), no outbreak model (Plan 47), no
psychiatric facility (Plan 144).

## 1. Outcome
`Medical/AdvancedSurgicalWardSystem.cs` (**381 lines**) is reachable and
unaddressed: the **facility** where Plan 124's clinical engines operate.
Capacity, sterility, and post-operative care are environmental inputs to
surgery outcomes — none are stated, so ward quality is invisible and outcomes
depend only on dice.

| Deliverable | Detail |
|---|---|
| Ward model | bays with capacity (Plan 103's occupancy patterns), sterility level, and equipment from built state |
| Sterility rule | sterility decays with use and events (Plan 47's infection pressure reads it); resterilization consumes documented supplies |
| Outcome feeding | ward state feeds Plan 124's surgery/complication probabilities as a typed input |
| Post-op care | recovery occupancy blocks a bay; a patient is not discharged until the documented condition |
| Save truth | ward state and patient placement restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Medical/AdvancedSurgicalWardSystem.cs` (381 lines; unaddressed — Wave 16 audit).
- Plan 124's engines consume ward quality; Plan 47's infection model receives sterility.
- Plan 103's occupancy semantics apply to bays.
- Plan 93 verifies supply consumption.

## 3. Packages
- **SWT-213A** ward model + sterility table.
- **SWT-213B** sterility decay/reset path (supplies consumed).
- **SWT-213C** outcome-feed contract with Plan 124 + fixture per sterility band.
- **SWT-213D** post-op occupancy/discharge tests.
- **SWT-213E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Sterility changes only through documented use/reset; outcome probabilities shift per band.
- Bays are occupied exactly as modeled; discharges require their condition.
- Save/load preserves ward and patient state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`.

## 5. Risks
Invisible quality → sterility is surfaced in the ward view and feeds outcomes visibly.
Overlap with 124 → the facility supplies inputs; the engines stay there.

---

## 6. Expanded census (7 files · 1,568 lines)

Scope: `Assets/Ashfall.Core/Medical/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Save 1 · Support 1 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AdvancedSurgicalWardSystem.cs` | 381 | System | **yes** | 0 | 0 | 2 |
| `ClinicalWardTriageEngine.cs` | 307 | System | — | 0 | 0 | 0 |
| `MedicalWardPipelineBridge.cs` | 88 | Support | — | 0 | 0 | 0 |
| `MedicalWardSave.cs` | 80 | Save | — | 0 | 0 | 0 |
| `MedicalWardSystem.cs` | 363 | System | — | 0 | 0 | 6 |
| `SurgicalGraftRejectionEngine.cs` | 284 | System | — | 0 | 0 | 4 |
| `SurgicalProcedureCatalog.cs` | 65 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `surgical_procedures.json` | object[2 keys] |
| `operating_theater_surgical_logs.json` | array[7] |

**State surfaces:** `AdvancedSurgicalWardSystem.cs`, `MedicalWardSystem.cs`, `SurgicalGraftRejectionEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Medical/` |
| Test references | 27 name references across the test tree |
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

Domain files: 7. Other plans referencing their names: **11**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 5 |
| `PLAN-ACUTE-TRAUMA-CARE-124` | 4 |
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 3 |
| `EVIDENCE` | 2 |
| `PLAN-PANDEMIC-PUBLIC-HEALTH-47` | 2 |
| `PLAN-MARITIME-DEEPWATER-27` | 1 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SWT-213A` | no name match — resolve at claim time |
| `SWT-213B` | no name match — resolve at claim time |
| `SWT-213C` | no name match — resolve at claim time |
| `SWT-213D` | no name match — resolve at claim time |
| `SWT-213E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 7; intra-domain edges: **3**; isolated files:
**3**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `ClinicalWardTriageEngine` | `AdvancedSurgicalWardSystem` |
| `ClinicalWardTriageEngine` | `MedicalWardSystem` |
| `MedicalWardPipelineBridge` | `MedicalWardSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `MedicalWardSystem` | 2 |
| `AdvancedSurgicalWardSystem` | 1 |
| `ClinicalWardTriageEngine` | 0 |
| `MedicalWardPipelineBridge` | 0 |
| `MedicalWardSave` | 0 |
| `SurgicalGraftRejectionEngine` | 0 |
| `SurgicalProcedureCatalog` | 0 |

**Class split:** hub 0 · sink 2 · source 2 · isolated 3.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 7. Host files: **8** · Test files: **24** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/Host/AutopsyHostSession.cs`, `src/Host/MedicalWardHostSession.cs`, `src/Host/MedicalWardSaveStore.cs`, `src/Host/MentalHealthCrisisHostSession.cs`, `src/Main.AdvancedShelterSystems.cs` |
| Tests (`Ashfall.Core.Tests/`) | 24 | `Ashfall.Core.Tests/AutopsyBridgeTests.cs`, `Ashfall.Core.Tests/AutopsyIntegrationTests.cs`, `Ashfall.Core.Tests/AutopsySystemTests.cs`, `Ashfall.Core.Tests/Content/Plan49ContentAtmosphereIntegrationTests.cs`, `Ashfall.Core.Tests/Integration/Plans60To63ThirtyDayIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `medical` |
| `medical_pipeline` |
| `medical_ward` |
| `surgical_ward` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--medical-selftest` |
| `--medical-ward-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnChapterAdvanced` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` |
| `OnDayAdvanced` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnMedicalProcessingCompleted` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnStageAdvanced` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **7**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/documents/vel_triage_log_names.json` |
| `Assets/StreamingAssets/Data/medical_record_templates.json` |
| `Assets/StreamingAssets/Data/medical_texts.json` |
| `Assets/StreamingAssets/Data/narrative/dweller_medical_casebook.json` |
| `Assets/StreamingAssets/Data/narrative/medical_documents_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/operating_theater_surgical_logs.json` |
| `Assets/StreamingAssets/Data/surgical_procedures.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (49 files, 435 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Medical` | 49 | 435 |

**Verdict:** 435 cases sit under matching regions — run those first (`Medical`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **14**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/HostCli.AdvancedIndustrialRecon.cs` |
| `src/Host/MedicalHostSession.cs` |
| `src/Host/MedicalPipelineSaveStore.cs` |
| `src/Host/MedicalSaveStore.cs` |
| `src/Host/MedicalWardHostSession.cs` |
| `src/Host/MedicalWardSaveSelfTest.cs` |
| `src/Host/MedicalWardSaveStore.cs` |
| `src/Host/SurgicalWardSaveStore.cs` |
| `src/Main.AdvancedShelterSystems.cs` |
| `src/Main.Medical.cs` |
| `src/Main.MedicalTriage.cs` |
| `src/UI/AmputationTriagePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **4**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `medical` | no |
| `medical_pipeline` | no |
| `medical_ward` | no |
| `surgical_ward` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `medical` |
| `medical_microfluidic_diagnostics` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **6**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 1, OPTIONAL 2).

| Catalog | Classification |
|---|---|
| `documents/vel_triage_log_names.json` | OPTIONAL |
| `medical_texts.json` | OPTIONAL |
| `narrative/dweller_medical_casebook.json` | CODEX_ONLY |
| `narrative/medical_documents_expansion.json` | CODEX_ONLY |
| `narrative/operating_theater_surgical_logs.json` | CODEX_ONLY |
| `surgical_procedures.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 11
**Surface:** save sections 4 (laddered 0) · RNG streams 3 · host files 15 · catalogs 13 · test regions 1 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SURGICAL-WARD-TRUTH-213
wave: 16
status: PROPOSED — foreman claim required
packages: SWT-213A, SWT-213B, SWT-213C, SWT-213D, SWT-213E
claim paths:
  - src/Host/HostCli.AdvancedIndustrialRecon.cs  # §19 candidate host surface
  - src/Host/MedicalHostSession.cs  # §19 candidate host surface
  - src/Host/MedicalPipelineSaveStore.cs  # §19 candidate host surface
  - src/Host/MedicalSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/documents/vel_triage_log_names.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/medical_record_templates.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Medical/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 11 other plan(s) name these artifacts (§12)
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
