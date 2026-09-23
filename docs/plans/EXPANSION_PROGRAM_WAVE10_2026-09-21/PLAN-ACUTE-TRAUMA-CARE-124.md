# PLAN-ACUTE-TRAUMA-CARE-124 — Wounds, Triage, Surgery & Rehabilitative Follow-Up

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-PANDEMIC-PUBLIC-HEALTH-47, PLAN-BIONICS-ENHANCEMENT-78, PLAN-MUTATION-HEREDITY-81, PLAN-THERMAL-EXPOSURE-TRUTH-117.
**Non-goals:** no outbreak modeling (Plan 47), no prosthetics/bionics (Plan 78),
no chronic dose conditions (Plan 81), no new body-integrity schema (blocked
F14/XP-06 decision — do not touch it).

## 1. Outcome
`Medical/` holds six host-unreachable engines: `ClinicalWardTriageEngine`,
`SurgicalGraftRejectionEngine`, `RehabilitationProgressionEngine`,
`PalliativeCareDignityEngine`, `DependencyTaperWithdrawalEngine`,
`ProstheticConditionWearEngine` (Plan 1 Appendices A/H/K). Plans 47, 78, and 81
own outbreaks, enhancement, and hereditary/chronic effects. What remains
unowned is the **acute path**: wound severity, triage ordering, surgery
outcomes, and the rehabilitative follow-up that returns a survivor to duty.

| Deliverable | Detail |
|---|---|
| Wound model | wound classes with severity bands, bleeding/infection pressure, and the existing health owner as the only writer |
| Triage rules | `ClinicalWardTriageEngine` orders casualties by a documented score; ties broken deterministically |
| Surgery outcome | success/complication derived from supplies, skill, ward condition; graft rejection uses its own engine with a seeded roll |
| Recovery & rehab | `RehabilitationProgressionEngine` returns capability over documented days; a survivor is not productive until cleared |
| Palliative/taper | `PalliativeCareDignityEngine` and `DependencyTaperWithdrawalEngine` handle terminal and withdrawal cases with visible, non-silent outcomes |

## 2. Evidence
- Plan 1 Appendix A: the six engines are host-unreachable; Appendix K lists their public members.
- Plan 1 Appendix L: none of the six carries a banned deterministic source — wiring is safe under the seeded contract.
- Plan 47 owns epidemic response; Plan 78 owns replacement parts; Plan 81 owns chronic/hereditary effects.
- Existing health/needs owners persist survivor state; the acute path writes through them.

## 3. Packages
- **ATC-124A** wound model + severity table (writer = existing health owner).
- **ATC-124B** triage ordering + deterministic tie-break test.
- **ATC-124C** surgery outcome + complication/rejection seeded tests.
- **ATC-124D** rehab progression + duty-clearance gate test (ties to Plan 101/107).
- **ATC-124E** palliative/taper paths with visible outcome + no silent expiry.

## 4. Acceptance & verification
- Same wound + same supplies/skill/seed → same outcome.
- Triage ordering stable under equal scores (deterministic tie-break).
- Rehab blocks duty assignment until clearance (observable in roster coverage).
- `bash scripts/run_test.sh` on the medical region.

## 5. Risks
Body-schema overlap → the blocked F14/XP-06 decision is explicitly out of scope; this plan uses existing state only.
Triage as authority → it orders treatment; the health owner writes outcomes.

---

## 6. Expanded census (6 files · 1,275 lines)

Scope: `Assets/Ashfall.Core/Medical/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Support 1 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AdvancedSurgicalWardSystem.cs` | 381 | System | — | 0 | 0 | 2 |
| `ClinicalWardTriageEngine.cs` | 307 | System | **yes** | 0 | 0 | 0 |
| `RehabilitationProgressionEngine.cs` | 112 | System | **yes** | 0 | 0 | 0 |
| `RehabilitationSlateProjection.cs` | 126 | Support | — | 0 | 0 | 0 |
| `SurgicalGraftRejectionEngine.cs` | 284 | System | **yes** | 0 | 0 | 4 |
| `SurgicalProcedureCatalog.cs` | 65 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `surgical_procedures.json` | object[2 keys] |
| `psychological_trauma.json` | object[4 keys] |
| `vel_triage_log_names.json` | object[6 keys] |
| `operating_theater_surgical_logs.json` | array[7] |

**State surfaces:** `AdvancedSurgicalWardSystem.cs`, `SurgicalGraftRejectionEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Medical/` |
| Test references | 6 name references across the test tree |
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
| `EVIDENCE` | 5 |
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 5 |
| `PLAN-ORPHAN-SEAL-01` | 4 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 4 |
| `PLAN-SURGICAL-WARD-TRUTH-213` | 4 |
| `PLAN-BIONICS-ENHANCEMENT-78` | 3 |
| `PLAN-UNBLOCK-03` | 2 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `ATC-124A` | no name match — resolve at claim time |
| `ATC-124B` | `ClinicalWardTriageEngine.cs` |
| `ATC-124C` | `SurgicalGraftRejectionEngine.cs` |
| `ATC-124D` | `RehabilitationProgressionEngine.cs`, `RehabilitationSlateProjection.cs` |
| `ATC-124E` | `PalliativeCareDignityEngine.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 7; intra-domain edges: **2**; isolated files:
**3**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `ClinicalWardTriageEngine` | `AdvancedSurgicalWardSystem` |
| `RehabilitationSlateProjection` | `RehabilitationProgressionEngine` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `AdvancedSurgicalWardSystem` | 1 |
| `RehabilitationProgressionEngine` | 1 |
| `ClinicalWardTriageEngine` | 0 |
| `PalliativeCareDignityEngine` | 0 |
| `RehabilitationSlateProjection` | 0 |
| `SurgicalGraftRejectionEngine` | 0 |
| `SurgicalProcedureCatalog` | 0 |

**Class split:** hub 0 · sink 2 · source 2 · isolated 3.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 7. Host files: **2** · Test files: **7** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Main.AdvancedShelterSystems.cs`, `src/Main.Plans190_193.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/Medical/ClinicalWardTriageEngineTests.cs`, `Ashfall.Core.Tests/Medical/PalliativeCareDignityEngineTests.cs`, `Ashfall.Core.Tests/Medical/Plan16_19TriageEpilogueIntegrationTests.cs`, `Ashfall.Core.Tests/Medical/RehabilitationProgressionEngineTests.cs`, `Ashfall.Core.Tests/Medical/RehabilitationSlateProjectionTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `medical_ward` |
| `surgical_ward` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--medical-ward-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnChapterAdvanced` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` |
| `OnDayAdvanced` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnPalliativeAssigned` | `Assets/Ashfall.Core/SickListSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnStageAdvanced` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnTraumaBondDecayed` | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` |
| `OnTraumaBondFormed` | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **5**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/documents/vel_triage_log_names.json` |
| `Assets/StreamingAssets/Data/narrative/operating_theater_surgical_logs.json` |
| `Assets/StreamingAssets/Data/narrative_progression.json` |
| `Assets/StreamingAssets/Data/psychological_trauma.json` |
| `Assets/StreamingAssets/Data/surgical_procedures.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (11 files, 83 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Progression` | 11 | 83 |

**Verdict:** 83 cases sit under matching regions — run those first (`Progression`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **10**
(3 of them panels/HUD).

| Host file |
|---|
| `src/Host/HostCli.AdvancedIndustrialRecon.cs` |
| `src/Host/MedicalWardHostSession.cs` |
| `src/Host/MedicalWardSaveSelfTest.cs` |
| `src/Host/MedicalWardSaveStore.cs` |
| `src/Host/SurgicalWardSaveStore.cs` |
| `src/Main.AdvancedShelterSystems.cs` |
| `src/Main.MedicalTriage.cs` |
| `src/UI/AmputationTriagePanel.cs` |
| `src/UI/MedicalWardPanel.cs` |
| `src/UI/TraumaBondingCohortPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `medical_ward` | no |
| `surgical_ward` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **4**
(CODEX_ONLY 1, GAMEPLAY_CONSUMED 2, OPTIONAL 1).

| Catalog | Classification |
|---|---|
| `documents/vel_triage_log_names.json` | OPTIONAL |
| `narrative/operating_theater_surgical_logs.json` | CODEX_ONLY |
| `narrative_progression.json` | GAMEPLAY_CONSUMED |
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
**Surface:** save sections 2 (laddered 0) · RNG streams 1 · host files 11 · catalogs 9 · test regions 1 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ACUTE-TRAUMA-CARE-124
wave: 10
status: PROPOSED — foreman claim required
packages: ATC-124A, ATC-124B, ATC-124C, ATC-124D, ATC-124E
claim paths:
  - src/Host/HostCli.AdvancedIndustrialRecon.cs  # §19 candidate host surface
  - src/Host/MedicalWardHostSession.cs  # §19 candidate host surface
  - src/Host/MedicalWardSaveSelfTest.cs  # §19 candidate host surface
  - src/Host/MedicalWardSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/documents/vel_triage_log_names.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/operating_theater_surgical_logs.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Progression/
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
