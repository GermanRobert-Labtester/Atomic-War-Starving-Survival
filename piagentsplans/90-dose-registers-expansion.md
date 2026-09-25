# Plan 90 — Dose Registers: Twelve Display Bands, Care Plans, and Canonical Ledger Boundaries

> **Rebuild status:** TERMINAL 12-BAND CONTENT + CLINICAL AUTHORITY AUDIT
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-4`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round4-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** 250,000 characters is an evidence-backed minimum for this package, not a ceiling. The file may exceed that target when current owner code, catalog rows, caller evidence, and test declarations justify it; it must not pad to reach a number.

## 0. Integrity Statement and Plan Status

This is a planning and architecture artifact. It replaces the prior unregistered bulk-generated section in this exact path; it does not authorize production, authored-data, Core, host, UI, save, test, generated-index, or runtime edits. Every current path named below is evidence captured during this rebase. A future `MODIFY` or `CREATE` action is a proposal for a separately claimed package, never a silent instruction to the next builder.

The historical baseline was 4,719 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Keep the twelve authored dose-register bands and eight care-plan rows as a legible register vocabulary while preserving the canonical dose ledger’s actual measurement, band, and treatment owners. The historical 4→12/3→8 expansion is complete; the next value is a boundary audit that prevents display categories from silently becoming medical mechanics.

**Bounded outcome:** Audit `DoseRegistersCatalog`, `DoseLedgerSystem`, `DoseLedgerHostSession`, the dose panel, the canonical treatment/medical pipeline, save/restore, and current tests. Treat unused register rows as dormant display content until a current consumer proves a safe mapping.

**Non-goals:** no new clinical band authority, no new care-plan executor, no medical dosage tuning, no new save section, no arbitrary row growth, no production/data/test edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `dose_registers.json` contains 12 bands, 8 plans, 3 guesses, 4 registers, and 4 NPCs.
- VERIFIED CURRENT: `DoseRegistersCatalog` is explicitly described as display strings; `BandIdFor` currently maps four canonical ids.
- VERIFIED CURRENT: the mutable dose owner is `DoseLedgerSystem` and the host section is `dose_ledger`.
- HISTORICAL RECORD: Plan 90/Wave 39 records the 4→12 and 3→8 expansion; this package does not claim a fresh test run.

The safe planning decision is not “grow the old number.” It is:

1. preserve the current owner and data contract;
2. identify what the historical plan already completed;
3. separate dormant content, display-only vocabulary, and false reachability from live commands;
4. define the smallest residual integration package, if one is justified; and
5. leave a builder with exact paths and focused verification rather than fictional APIs.

**Master-authority application:**

- The Factory Protocol requires a premise sweep, one bounded lane/cluster, collision checking, evidence labels, and a continuity check.
- The generator matrices and deep maps require current C1–C17 ownership rather than a historical title.
- The save/state lane requires capture/restore/migration and determinism to be explicit.
- The UI/accessibility lane requires truthful presentation, words-not-color-only, focus, controller parity, and lifecycle evidence.
- The verification lane requires focused tests and two-pass determinism where a simulation exists.
- The anti-padding rule forbids manufacturing 600-day traces, 100-test suites, repeated dossiers, or “sealed” language merely to increase size.

- `Assets/StreamingAssets/Data/dose_registers.json` exists at 6,225 bytes; SHA-256 `eec0f33b433f4ae15549f3fc4a61cdde840aba6707c870fc65c6608a69820b08`.
- `Assets/StreamingAssets/Data/medical_texts.json` exists at 222,244 bytes; SHA-256 `867ac96ebabc9c40db200106c84a8f8fd2e0f5c21650a737aa3678312d548347`.
- `Assets/StreamingAssets/Data/psychological_therapies.json` exists at 8,022 bytes; SHA-256 `64c91c12f95af2fc3023d0d4f52c0a871a8ea88475096f024b59fd766624b3bf`.

# 3. Required Delta

Replace the old pure-data brief with a current 12-band/8-plan census and a clinical-authority boundary. Identify which expanded rows are live, display-only, or dormant, and define a safe residual package only where a current owner/consumer gap is proven.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Which current callers request each register band, and does any code use the eight care-plan rows as executable commands?
Does `DoseRegistersCatalog.BandLabel` intentionally support only the canonical four-band mapping?
How does the dose panel show an unavailable or unknown register state?
Does the current dose save round-trip preserve all mutable facts while leaving static catalog rows out of state?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| dose register vocabulary and display labels | `DoseRegistersCatalogLoader` | `Assets/Ashfall.Core/DoseRegistersCatalog.cs` | Display strings and row ordering only; it is not the dose authority. |
| cumulative dose, canonical bands, and ledger mutations | `DoseLedgerSystem` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` | Owns dose facts and canonical band behavior. |
| dose persistence and host conditioning | `DoseLedgerHostSession / DoseLedgerSaveStore` | `src/Host/DoseLedgerHostSession.cs; src/Host/DoseLedgerSaveStore.cs` | Owns dose ledger save and host reads; the register catalog is static. |
| medical treatment/condition execution | `Medical pipeline owners` | `src/Main.Medical.cs; Assets/Ashfall.Core/Medical/` | Register plan text cannot create treatment or bypass medical gates. |
| dose player surface | `DoseLedgerPanel / DoseRegisterSurface` | `src/UI/DoseLedgerPanel.cs; src/Dose/DoseRegisterSurface.cs` | Presentation and truthful register reading only. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for dose-register vocabulary.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Dose Registers: Twelve Display Bands, Care Plans, and Canonical Ledger Boundaries                                               │
│ Integration route: DATA-ONLY + current ledger/host boundary audit                             │
└──────────────────────────────────────────────────────────────┘
              │
              ▼
Current loader/validator → current Core owner → existing host seam
              │
              ├─ typed fact/event → briefing/journal/consumer
              ├─ existing save owner and checksum/codec
              ├─ Godot presenter/projection (no gameplay arithmetic)
              └─ focused tests + headless evidence
```

### 6.1 Architectural decisions

1. **Static register rows are not clinical authority.**
2. **DoseLedgerSystem remains the sole mutable dose owner.**
3. **Medical owners execute treatments; the register only explains.**
4. **The existing dose_ledger section is the persistence owner.**
5. **UI projects current facts and exposes refusals.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| dose register vocabulary and display labels | `DoseRegistersCatalogLoader` | `Assets/Ashfall.Core/DoseRegistersCatalog.cs` | Display strings and row ordering only; it is not the dose authority. |
| cumulative dose, canonical bands, and ledger mutations | `DoseLedgerSystem` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` | Owns dose facts and canonical band behavior. |
| dose persistence and host conditioning | `DoseLedgerHostSession / DoseLedgerSaveStore` | `src/Host/DoseLedgerHostSession.cs; src/Host/DoseLedgerSaveStore.cs` | Owns dose ledger save and host reads; the register catalog is static. |
| medical treatment/condition execution | `Medical pipeline owners` | `src/Main.Medical.cs; Assets/Ashfall.Core/Medical/` | Register plan text cannot create treatment or bypass medical gates. |
| dose player surface | `DoseLedgerPanel / DoseRegisterSurface` | `src/UI/DoseLedgerPanel.cs; src/Dose/DoseRegisterSurface.cs` | Presentation and truthful register reading only. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load the twelve register rows through DoseRegistersCatalogLoader
2. map only the current canonical band ids that the host actually requests
3. read cumulative dose and canonical band from DoseLedgerSystem
4. project disposition/plan text without applying a clinical effect
5. write a dose reading or care command only through the existing ledger/medical owner
6. capture/restore the existing dose_ledger section

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- register rows are static definitions and never alter cumulative dose
- thresholds are ordered and non-negative at the data boundary
- display band selection cannot override DoseLedgerSystem’s canonical band
- care-plan cost text is descriptive until a medical owner consumes a typed command
- unknown register ids fail visibly and never fall back to a favorable clinical claim
- a save round-trip preserves dose, readings, cohorts, and calibration state

**Invariant review questions:**

- Which values are authored definitions, which are player decisions, and which are derived projections?
- What is the exact identity key and ordering rule?
- What happens on missing, duplicate, stale, malformed, or legacy input?
- Which transitions are monotonic, bounded, idempotent, or exactly-once?
- Which state survives save/load, and which is recomputed after restore?

# 10. API and Contract Design

The current declaration indexes in Appendix C are authoritative for method names. The following is a future contract shape, not a claim that every method already exists:

```text
Preview(command, context) -> named availability/refusal + exact deltas
Execute(command, context) -> owner mutation + typed fact
QueryCurrentState(subjectId) -> read-only projection
CaptureState() -> deep serializable owner state
RestoreState(legacyOrCurrentState) -> normalized owner state
```

Contract rules for dose-register vocabulary:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`dose_registers.json` remains the only register vocabulary authority. Audit the eight expanded bands and five added plans for current consumers. The current DTO maps only `band_green`, `band_amber`, `band_red`, and `band_black` through `BandIdFor`; the other rows are not automatically live clinical bands. Any future mapping needs a medical decision, tests, and a single owner.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

The existing `dose_ledger` section and `DoseLedgerSaveStore` own mutable dose state. The static register catalog should not be persisted as a second mutable ledger. A future change to a register-to-treatment mapping must ride the medical owner’s existing save/codec and retain legacy defaults.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

Dose classification and register ordering are deterministic. Fallout-window conditioning belongs to DoseLedgerHostSession’s existing seeded/campaign path; a display row must not consume RNG or alter dose calculations. Any future replay test must compare cumulative dose, band, readings, and conditioning input.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

DoseLedgerSystem emits current reading/band facts; the host may project a register label and medical consequence through existing medical seams. A display row must not emit a fake treatment event. Exactly-once readings, calibration transitions, and ledger consequences remain owned by the current ledger/medical paths.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Host/DoseLedgerHostSession.cs` — conditions and captures the canonical dose ledger
- `src/Host/DoseLedgerSaveStore.cs` — persists the existing dose_ledger envelope
- `src/Main.Phase0.cs` — composes the dose ledger and host conditioning
- `src/Main.Medical.cs` — routes medical commands and projections
- `src/UI/DoseLedgerPanel.cs` — renders current dose/band/register state
- `src/Dose/DoseRegisterSurface.cs` — provides the register-oriented presentation surface

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Register prose is clinical bureaucracy: it may explain what a record means, but it must not diagnose, prescribe, or promise a treatment the medical owner cannot perform. Avoid real-world medical claims and keep the existing restrained Ashfall register voice.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DoseRegistersCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/DoseLedgerSystemTests.cs`

### Permanent/static checks

1. `godot --headless --path . -- --data-integrity-selftest`
2. `godot --headless --path . -- --content-utilization-selftest` (only when the current catalog is in the utilization scope)
3. `git diff --check -- <owned planning paths>`

### Verification layers

- **Data:** schema, id uniqueness, bounded fields, references, and catalog admission.
- **Core:** pure behavior, boundaries, stable ordering, malformed/legacy inputs, and event emission.
- **Persistence:** capture/restore/deep-copy, old-save default, checksum/codec, and exactly-once fields.
- **Determinism:** paired same-seed replay and, if a stream is involved, two-run fingerprint equality.
- **Host:** setup, command, save, restore, reset, and lifecycle wiring.
- **UI:** truthful projection, focus, controller/back, accessibility, refresh, and no fabricated fallback.
- **Runtime:** only the smallest relevant headless flag; no broad suite by default.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Forbidden shortcut |
|---|---|---|---|
| Phase 0 — current census | read register catalog, canonical ledger, medical owners, save, and panel | all 12/8 rows and all four canonical band mappings are explicit | no undocumented scope or shortcut |
| Phase 1 — consumer/semantic audit | map every register field to a current caller or mark dormant | no text-only treatment path is mistaken for mechanics | no undocumented scope or shortcut |
| Phase 2 — persistence/accessibility audit | verify dose ledger restore and readable dispositions | no UI or save authority is duplicated | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a proven medical/display gap receives a new claim | one owner, one focused test set | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/dose_registers.json` | READ ONLY; MODIFY only for a proven row/consumer defect | retain as display vocabulary |
| `Assets/Ashfall.Core/DoseRegistersCatalog.cs` | READ ONLY | loader and BandIdFor boundary |
| `Assets/Ashfall.Core/DoseLedgerSystem.cs` | READ ONLY | canonical dose owner |
| `src/Host/DoseLedgerHostSession.cs` | READ ONLY | host conditioning/persistence seam |
| `src/UI/DoseLedgerPanel.cs` | READ ONLY | truthful presentation |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| clinical authority drift | keep DoseLedgerSystem and medical owners authoritative |
| dormant rows presented as live | label reachability explicitly and test current consumers |
| legacy save incompatibility | use existing dose_ledger codec and round-trip fixtures |

# 22. Explicit Non-Goals

- no new clinical band authority, no new care-plan executor, no medical dosage tuning, no new save section, no arbitrary row growth, no production/data/test edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for dose-register vocabulary are named from current evidence.
- Historical expansion counts and pass claims are clearly separated from current reachability and current test declarations.
- The required residual is bounded to one future claim, or explicitly recorded as a safe no-change result when no defect survives the census.
- Every future change has an exact focused command and an owner/path claim; no count-only or filler-only acceptance remains.
- The final precision pass has rechecked hashes, paths, terminology, and unsupported claims.

**DoD is behavioral:** a builder can execute the first safe step without reinterpreting ownership, and a reviewer can reject any shortcut that creates a second authority or treats catalog presence as live reachability.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current JSON/data owners and current save/codec owners.
- Deterministic streams, campaign-day semantics, and existing event ordering.
- Existing accessibility, controller, focus, and lifecycle contracts.
- Sealed, retired, accepted, blocked, and active decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- A current field-to-consumer table for the twelve bands, eight plans, guesses, registers, and NPCs.
- A precise distinction between canonical dose bands and display/register vocabulary.
- A bounded follow-up only for a proven display, validation, or medical-owner gap.

## MUST NOT DO

- create a second dose band or treatment owner
- apply a care plan from a JSON string without a typed command
- change clinical thresholds or medical tuning in this package
- add a new save section for static register rows

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DoseRegistersCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/DoseLedgerSystemTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read DoseRegistersCatalog, DoseLedgerSystem, DoseLedgerHostSession, DoseLedgerPanel, and the focused tests; enumerate which of the twelve rows are actually mapped by current code.

# Appendix A — Master expansion authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Authority lines: 5,510; bytes: 635,647

The following slices are read-only design constraints. Live source remains higher authority.

### Authority lines 43–48
00043:
00044: **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
00045: Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.
00046:
00047: **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
00048: `Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

### Authority lines 78–91
00078: ### 2.1 The seven-step factory loop
00079:
00080: **Step 1 — Premise sweep (mandatory, every time).**
00081: Before selecting any candidate, the session re-verifies premises against live source: the live `Assets/StreamingAssets/Data/` listing (duplication firewall, DR-04), `INTEGRATION_PLANS.md` current batch (DR-06), `WORKTREE_OWNERSHIP.md` claims (DR-09), `KNOWN_DEBT.md`, the root coordination files (DR-01), `docs/gaps/` and `docs/incidents/` (DR-02), and `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` (DR-08). Output: a short premise sheet. A candidate whose premise fails the sweep is discarded, not patched.
00082:
00083: **Step 2 — Select exactly one lane and one subsystem cluster.**
00084: From Part III. The selection rule is lane rotation discipline (v1.0 Part 11): at most one plan per lane per wave; data-first lanes (A, C, J) precede wiring lanes (B, D, E) within the same domain. The session states the lane and cluster in the plan header.
00085:
00086: **Step 3 — Pull the cell's opening archetype and instantiate it.**
00087: Each matrix cell names an archetype (the *kind* of expansion that cell supports, with its owning seams). The session instantiates the archetype against current evidence: which catalog, which loader, which host session, which save family, which panel. If the archetype's seams no longer exist as described, the cell is stale — record the correction in the Drift Register and pick again.
00088:
00089: **Step 4 — Draft the subject plan in the v2.0 subject-plan format (Part V, Template S).**
00090: A subject plan is not an integration plan. It states WHAT should expand, WHY (with evidence), WHAT MUST NOT CHANGE, and WHICH INTEGRATION ROUTE the repository should prefer — but it does not prescribe line-level implementation. The integration route recommendation (Part V, Template R) names the tier (data-only / host wiring / Core extension), the seams, the save impact class, and the verification class. This preserves the repo's own separation: subject plans propose; integration plans (drafted later, against the live tree, in an owning session) commit.
00091:

### Authority lines 101–114
00101: ### 2.2 Factory invariants
00102:
00103: - One plan = one bounded outcome riding existing seams (v1.0 Part 10). The factory never widens a plan to reach a size target.
00104: - No plan may create a parallel authority. Every state change names its owning system.
00105: - Data-first preference: if an expansion can be authored as JSON through an existing loader, it must be, and the plan must say so.
00106: - The factory never drafts against decision-blocked items (the current list must be re-read from `INTEGRATION_PLANS.md` each session — DR-06 shows signatures resolve over time).
00107: - Subject plans do not edit files. Implementation happens only in an owning session after plan selection (v1.0 approval-based workflow).
00108: - Every generated plan must state its position relative to each epilogue permutation it touches (v1.0 Part 6.6).
00109:
00110: ---
00111:
00112: ## PART III — GENERATOR MATRICES (LANES × SUBSYSTEM CLUSTERS)
00113:
00114: Ten lanes (A–J, from v1.0 Part 11) against seventeen subsystem clusters distilled from the live Core inventory (v1.0 Parts 5.1–5.2 and 16, confirmed live). Each cell names an opening archetype. Confidence labels reflect the audit state as of 2026-09-24 and must be re-checked at drafting time. This matrix is the combinatorial engine: 170 cells, each capable of yielding multiple subject plans over time as content lands and seams mature. Not every cell is currently open; cells marked SEALED are closed by evidence (e.g., the distress-signal content seal, DR-06) and may not be opened without new evidence and foreman signature.

### Authority lines 116–121
00116: ### Cluster definitions
00117:
00118: C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).
00119:
00120: ### 3.1 Lane A — Narrative and prose (all types and kinds)
00121:

### Authority lines 187–200
00187: ### 3.5 Lane E — UI, UX, and accessibility
00188:
00189: | Cluster | Opening archetype | Confidence |
00190: |---|---|---|
00191: | C17 | Per-system: panels exposing existing commands + truthful state for systems that gained data since their panel last shipped; a11y words-not-color-only gating; controller parity | HIGH CONFIDENCE |
00192: | C8 | Rescue-signals strip is shipped (DR-06); extension only through existing strip seams | SEALED surface, additive only |
00193: | C16 | Difficulty/XP binding surfaces once W1 lands (DR-06) — coordinate, do not parallel | HIGH CONFIDENCE |
00194: | All | Snapshot coverage and `--ui-a11y-selftest` gates apply to every panel change; `DESIGN.md` and `ACCESSIBILITY.md` are pinned | CANON process |
00195:
00196: ### 3.6 Lane F — Performance
00197:
00198: | Cluster | Opening archetype | Confidence |
00199: |---|---|---|
00200: | C17 | High-frequency UI rebuild audits (metric cards, data grids) — measure first via the CI performance gate | Potential hotspot — profile before rewrite |

### Authority lines 205–218
00205: ### 3.7 Lane G — Testing and verification
00206:
00207: | Cluster | Opening archetype | Confidence |
00208: |---|---|---|
00209: | C2 | Dose-treatment matrix paired tests against `MEDICAL_DOSE_TREATMENT_MATRIX.md` | HIGH CONFIDENCE |
00210: | C11 | Debt-ledger consequence dispatcher coverage; rumor-band determinism pins | HIGH CONFIDENCE |
00211: | C10 | Moral-choice flag consumer coverage for newly added consumers | HIGH CONFIDENCE |
00212: | C13 | Epilogue permutation reachability tests for under-served permutations | HIGH CONFIDENCE |
00213: | Cross | Determinism two-pass proofs for every new simulation; TEST-AGGREGATION metadata for catalog checks | CANON process |
00214:
00215: ### 3.8 Lane H — Tooling and developer experience
00216:
00217: | Cluster | Opening archetype | Confidence |
00218: |---|---|---|

### Authority lines 232–245
00232: ### 3.10 Lane J — Onboarding and player experience
00233:
00234: | Cluster | Opening archetype | Confidence |
00235: |---|---|---|
00236: | C16 | Difficulty preset scalar consumers (CF-XP01 line, reinforced by active W1, DR-06) | HIGH CONFIDENCE |
00237: | C17 | Daily-briefing surface for newly landed systems; onboarding flow waves | HIGH CONFIDENCE |
00238: | All | Manual playthrough checklists per wave (pattern exists: HoldfastManualPlaytest, expedition playtest report) | CANON process |
00239:
00240: ---
00241:
00242: ## PART IV — SEEDED EXPANSION BACKLOG (AUDIT-DERIVED CANDIDATES)
00243:
00244: Each candidate is a subject-plan seed: consume it through the Factory Protocol. Ordering within the backlog is by evidence strength, not by preference. None of these has been claimed; all require the Step 1 premise sweep before drafting.
00245:

### Authority lines 324–337
00324: ## PART VI — MULTI-SESSION GROWTH PROTOCOL (THE HONEST PATH TO 2,000,000 CHARACTERS)
00325:
00326: The v1.0 bible is roughly 86,000 characters of verified reference. A two-million-character corpus is roughly twenty-three times that volume. That volume is reachable, but only as accumulated *verified* content, because the repository's constitution forbids manufactured padding and this document inherits that rule. The protocol:
00327:
00328: 1. **Volume unit.** A volume is one appended Part to this document (or one of its companion canvases) produced in a single session, typically 15,000–60,000 characters, always evidence-grounded against the live repository.
00329: 2. **Volume types, in rotation:** (a) subsystem deep-map volumes (one per cluster C1–C17: full catalog inventories, prose-coverage gaps, seam maps); (b) prose specification libraries (expanded Part 9 field contracts with worked examples per document genre); (c) backlog replenishment volumes (fresh premise sweeps converting new Drift Register entries into SB-candidates); (d) lane deep guides (one per lane: full archetype playbooks with worked subject plans); (e) audit volumes (periodic re-audits refreshing the Drift Register).
00330: 3. **Session checklist.** Each session: run the Step 1 premise sweep; execute the Factory Protocol or append a volume; update the Drift Register for anything that moved; record the character count and volume index in the growth ledger below.
00331: 4. **Growth ledger.** v2.0 base: approximately 25,000 characters (this document). Target: 2,000,000. Every appended volume appends one ledger line: `[date] Volume [n] ([type]) — [chars] — cumulative [total]`.
00332: 5. **Quality gates that never relax:** every substantive statement carries a fact status; every volume names its verification surface; no volume may open a sealed surface (DR-06) or a decision-blocked item without the named signature; no volume may duplicate a live catalog or system.
00333: 6. **Anti-padding rule.** If a session cannot find verified content for the next volume, it records "no warranted volume" and stops. Zero-volume sessions are acceptable outcomes under the repository's own zero-plans doctrine.
00334:
00335: ---
00336:
00337: ## APPENDIX A — UPDATED VERIFICATION COMMAND SURFACE

### Authority lines 670–677
00670: **A-02 · C1 · Load-shed schedule amendments tied to the sanitation power-grid feed.** Subject: amendment notices following the sanitation `RoomPowerProvider` seam (sanitation power-grid feed landed per `FOLLOWUPS-210-213-THINSEAMS`). Evidence: `load_shed_schedule_001` exists in the corpus; the power feed seam is verified via the followups package. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00671:
00672: **A-03 · C2 · Casebook expansion for ARS latent-to-manifest phases.** Subject: medical casebook entries mirroring the authored ARS phase structure (latent-to-manifest, multi-day cadence, v1.0 Part 3.2). Evidence: `dweller_medical_casebook` exists; ARS pathology systems are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00673:
00674: **A-04 · C2 · Dose-treatment narrative pairing.** Subject: therapy-note and casebook twins for each row of `MEDICAL_DOSE_TREATMENT_MATRIX.md` (live, DR-03), so every mechanical treatment has a clinical-document voice. Evidence: matrix document verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00675:
00676: **A-05 · C2 · Therapist session notes batch 4.** Subject: a fourth batch keyed to guilt sources and insomnia states added since batch 3. Evidence: therapist batches 1–3 exist in the corpus; guilt sources and guilt insomnia are canon systems. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00677:

### Authority lines 873–878
00873: **G-02 · C11 · Debt-consequence dispatcher coverage.** Subject: dispatcher coverage for every consequence kind in the closed vocabulary, with recovery-path assertions. Evidence: dispatchers canon; recovery grammar canon. Route: focused xUnit. Confidence: HIGH CONFIDENCE.
00874:
00875: **G-03 · C2 · Dose-treatment matrix pairing tests.** Subject: pin each matrix row to its implementing treatment logic so the generated matrix cannot drift from code. Evidence: matrix live (DR-03). Route: focused xUnit + generation check. Confidence: HIGH CONFIDENCE.
00876:
00877: **G-04 · C13 · Epilogue permutation reachability suite.** Subject: deterministic tests that each permutation is reachable from some authored campaign state and that no optional content can invalidate the main ending (hard world rule). Evidence: matrix and rule canon. Route: deterministic simulation tests. Confidence: HIGH CONFIDENCE.
00878:

### Authority lines 929–942
00929: ## 3.6 Subsystem deep maps (D-01 through D-17; maps, not verdicts)
00930:
00931: Each map lists the cluster's canon owners, live catalogs, host sessions, and current factory openings. These are planning instruments: a session picks a cluster, reads its map, and consumes its seeds. All catalog and system names below are carried from the verified v1.0 inventory and the live 2026-09-24 listings; per-field internals remain session-verify territory.
00932:
00933: **DM-1 — Shelter operations (C1).** Owners: shelter rooms/identities/machines, thermal, schedules, social events, decor, fire, noise, airlock security, decon, atmosphere, sanitation (power-fed). Live catalogs: `shelter_rooms`, `shelter_room_identities`, `shelter_machine_identities`, `shelter_schedules`, `shelter_social_events`, `shelter_audio_cues`, `shelter_insulation_catalog`, `shelter_shielding`, `sanitation_facilities`, plus the sealed grid catalog. Hosts: ShelterAssignment, ShelterAtmosphere, ShelterDecor, ShelterFire, ShelterSchedule, ShelterThermal, Sanitation, AirlockSecurity, Decontamination, Ventilation. Openings: A-01, A-02, B-01, B-02, D-06, F-04. Notable constraint: room effects route through `IsRoomPowered`; shelter state persists through the holdfast/shelter save family.
00934:
00935: **DM-2 — Medical pipeline (C2).** Owners: disease, pathogens, dose ledger, ARS, surgery, autopsy, pharma lab, diagnostics, therapies, dependency, crises. Live catalogs: `disease_catalog`, `pathogens`, `dose_items/locations/quests/registers`, `autopsy_procedures`, `surgical_procedures`, `pharma_recipes`, `microfluidic_diagnostic_catalog`, `medical_texts`, `psychological_therapies`, `chemical_dependency_items`. Hosts: MedicalWard, DoseLedger, PsychologyArc, MentalHealthCrisis. Docs: `MEDICAL_PIPELINE_JOURNEY.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md` (all verified live). Openings: A-03, A-04, A-05, B-03, B-04, B-25, C-14 support, G-03.
00936:
00937: **DM-3 — Water, food, agriculture (C3).** Owners: water treatment, condensers, deep wells, brine, nutrition, kitchen, preservation, grain, greenhouse, aquaponics, aeroponics, apiculture, cryo cultivars. Live catalogs: `water_treatment` family via systems, `fog_harvesting_catalog`, `deep_well` systems, `brine` systems, `nutrition_profiles`, `food_preservation`, `grain_processing`, `greenhouse_items`, `hydroponic_crops`, `aquaponics_system_catalog`, `aeroponics_nutrient_catalog`, `cryo_cultivars`, `crop_strains`, `dive_sites`. Hosts: Greenhouse, GrainProcessing, KitchenNutrition, FoodPreservation, DeepWell, Sanitation, DeepCoast. Openings: A-06, A-07, A-08, B-05, C-12, F-012 (dive/hydroponic audit consumed as F-012 above).
00938:
00939: **DM-4 — Power and industry (C4).** Owners: power grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, EB/PVD, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology, extrusion. Live catalogs: `power_grid`, `power_subgrid_nodes`, `sofc_power_catalog`, `solar_concentrator_catalog`, `kinetic_flywheel_catalog`, `geothermal_strata_catalog`, `geothermal_drilling_depths`, `cupola_foundry_catalog`, `cvd_diamond_catalog`, `ebpvd_coating_catalog`, `precision_optics_catalog`, `precision_broaching_catalog`, `powder_metallurgy_catalog`, `plastic_pyrolysis_catalog`, `fischer_tropsch_catalog`, `chlor_alkali_synthesis_catalog`, `mineral_acid_synthesis_catalog`, `bio_fermentation_catalog`, `cellulosic_ethanol_catalog`, `cryogenic_air_separation`, `low_background_lead_catalog`, `metrology_standards_catalog`, `hydraulic_extrusion_catalog`. Hosts: SofcPower, SolarConcentrator, SilentFoundry, CvdDiamond, EbPvdCoating, PrecisionOptics, CryogenicAirSeparation, ChlorAlkali, BioFermentation, PlasticPyrolysis, HydraulicExtrusion, LowBackgroundMetrology, GeothermalAquifer. Openings: A-09, A-10, A-11, B-06, C-01, C-02. Constraint: XP W1 owns difficulty scalars for this cluster post-seal.
00940:
00941: **DM-5 — Expeditions and travel (C5).** Owners: expedition system, vehicles, dispatch preflight, scavenging tables, waystations, caravans, travel encounters, micro-locations, anomalous encounters. Live catalogs: `expeditions`, `vehicles`, `vehicle_modifications`, `vehicle_armor_grades`, `scavenging_tables`, `waystations`, `caravans`, `merchant_caravans`, `caravan_trade_routes`, `travel_encounters`, `micro_locations`, `anomalous_expedition_encounters`. Hosts: Expedition, ExpeditionVehicle, TravelingCaravan, Waystation, RescueDispatchPreflight. Docs: `EXPEDITION_30_DAY_PLAYTEST_REPORT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` (verified live). Openings: A-12, A-13, A-14, B-07, B-08, C-03, C-04, E-07.
00942:

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/dose_registers.json`
- Bytes: 6,225; SHA-256: `eec0f33b433f4ae15549f3fc4a61cdde840aba6707c870fc65c6608a69820b08`
- Root keys: `bands, calibration, guesses, npcs, plans, registers, schema_version`
- `bands`: list[12]; union fields: `disposition, id, label, threshold_msv`
  - row 1: `{"disposition":"No measurable burden. Walk the corridor.","id":"band_green","label":"Green","threshold_msv":0}`
  - row 2: `{"disposition":"Trace exposure only. The ledger notes it without alarm.","id":"band_white","label":"White","threshold_msv":25}`
  - row 3: `{"disposition":"Minor accumulation. Duty continues; the dial is watched.","id":"band_yellow","label":"Yellow","threshold_msv":50}`
  - row 4: `{"disposition":"The ledger shows a number worth watching.","id":"band_amber","label":"Amber","threshold_msv":100}`
  - row 5: `{"disposition":"Moderate exposure. Restricted duty begins here; heavy work is reassigned.","id":"band_orange","label":"Orange","threshold_msv":150}`
  - row 6: `{"disposition":"Significant accumulation. Light duty only. The register is marked in ink, not pencil.","id":"band_rose","label":"Rose","threshold_msv":200}`
  - row 7: `{"disposition":"Named on the sick list. Care is a choice, not a cure.","id":"band_red","label":"Red","threshold_msv":300}`
  - row 8: `{"disposition":"Severe exposure. Sick-room priority. The nurse is notified before the registrar.","id":"band_crimson","label":"Crimson","threshold_msv":400}`
  - row 9: `{"disposition":"Critical accumulation. Comfort care is the first option on the ledger.","id":"band_violet","label":"Violet","threshold_msv":500}`
  - row 10: `{"disposition":"The band the registrar will not soften. Still on the roster.","id":"band_black","label":"Black","threshold_msv":600}`
  - row 11: `{"disposition":"Terminal exposure. Palliative only. The clockmaker does not correct the reading.","id":"band_indigo","label":"Indigo","threshold_msv":800}`
  - row 12: `{"disposition":"Lethal accumulation. The registrar stops reading aloud. The column is filled in silence.","id":"band_void","label":"Void","threshold_msv":1000}`
- `plans`: list[8]; union fields: `cost, id, label, note`
  - row 1: `{"cost":"morphine","id":"plan_morphine_tray","label":"Morphine tray","note":"Mercy with a schedule. The tray is refilled on the same day every week."}`
  - row 2: `{"cost":"time","id":"plan_comfort_rounds","label":"Comfort rounds","note":"Someone sits the night. The sick room has a chair for it now."}`
  - row 3: `{"cost":"none","id":"plan_nothing","label":"Nothing","note":"A refusal is a choice the ledger records as silence."}`
  - row 4: `{"cost":"chelation_agent","id":"plan_chelation","label":"Chelation therapy","note":"A chemical course that binds heavy isotopes and moves them out. Expensive, limited, and worth counting."}`
  - row 5: `{"cost":"potassium_iodide","id":"plan_iodine_prophylaxis","label":"Iodine prophylaxis","note":"Thyroid saturation before the dose climbs further. The window is narrow; timing is the skill."}`
  - row 6: `{"cost":"time","id":"plan_isolation","label":"Isolation","note":"The patient is removed from shared air and touch. Secondary exposure stops here."}`
  - row 7: `{"cost":"time","id":"plan_rest","label":"Supervised rest","note":"Bed rest with hourly observation. Nothing curative; everything supportive."}`
  - row 8: `{"cost":"fuel","id":"plan_transfer","label":"Transfer","note":"If a better facility exists and fuel permits, the patient goes. The ledger follows."}`
- `guesses`: list[3]; union fields: `id, label, note, pencil`
  - row 1: `{"id":"guess_low","label":"Low","note":"A kinder story, written in pencil.","pencil":true}`
  - row 2: `{"id":"guess_honest","label":"Honest","note":"The truer number as it stands, subject to correction.","pencil":true}`
  - row 3: `{"id":"guess_refused","label":"Refused","note":"No guess at all. The board stays blank until the dial speaks.","pencil":true}`
- `calibration`: object[2]
- `registers`: list[4]; union fields: `action, id, label`
  - row 1: `{"action":"Book a reading","id":"register_ledger","label":"Ledger"}`
  - row 2: `{"action":"Name to the sick list","id":"register_sick","label":"Sick"}`
  - row 3: `{"action":"Book a baseline","id":"register_cohort","label":"Cohort"}`
  - row 4: `{"action":"Sign an hour","id":"register_voluntary","label":"Voluntary"}`
- `npcs`: list[4]; union fields: `action, action_label, disposition, id, name, register`
  - row 1: `{"action":"book_reading","action_label":"Book a reading","disposition":"Radiation Registrar. Holds a red pencil and will not read a number up to make it easier. Every refused booking is a silent tally she remembers.","id":"npc_dr_irina_vel","name":"Dr. Irina Vel","register":"register_ledger"}`
  - row 2: `{"action":"name_to_sick_list","action_label":"Name to the sick list","disposition":"Sick-room Nurse. Presents the bed order and waits. She never shuffles the order so one person's comfort costs another's care.","id":"npc_wyn_omah","name":"Wyn Omah","register":"register_sick"}`
  - row 3: `{"action":"calibrate","action_label":"Calibrate the dial","disposition":"The Clockmaker. The only honest voice about error: every figure has a drift, the drift is normal, and he will not pretend otherwise.","id":"npc_piet_abar","name":"Piet Abar","register":"register_ledger"}`
  - row 4: `{"action":"book_baseline","action_label":"Book a baseline","disposition":"The Midwife. Keeps the children's board in chalk so it can be erased. She will not book a guess as a truth.","id":"npc_saria_voss","name":"Saria Voss","register":"register_cohort"}`
- Bytes: 6,225; SHA-256: `eec0f33b433f4ae15549f3fc4a61cdde840aba6707c870fc65c6608a69820b08`
- Root keys: `bands, calibration, guesses, npcs, plans, registers, schema_version`
- `bands`: list[12]; union fields: `disposition, id, label, threshold_msv`
  - row 1: `{"disposition":"No measurable burden. Walk the corridor.","id":"band_green","label":"Green","threshold_msv":0}`
  - row 2: `{"disposition":"Trace exposure only. The ledger notes it without alarm.","id":"band_white","label":"White","threshold_msv":25}`
  - row 3: `{"disposition":"Minor accumulation. Duty continues; the dial is watched.","id":"band_yellow","label":"Yellow","threshold_msv":50}`
  - row 4: `{"disposition":"The ledger shows a number worth watching.","id":"band_amber","label":"Amber","threshold_msv":100}`
  - row 5: `{"disposition":"Moderate exposure. Restricted duty begins here; heavy work is reassigned.","id":"band_orange","label":"Orange","threshold_msv":150}`
  - row 6: `{"disposition":"Significant accumulation. Light duty only. The register is marked in ink, not pencil.","id":"band_rose","label":"Rose","threshold_msv":200}`
  - row 7: `{"disposition":"Named on the sick list. Care is a choice, not a cure.","id":"band_red","label":"Red","threshold_msv":300}`
  - row 8: `{"disposition":"Severe exposure. Sick-room priority. The nurse is notified before the registrar.","id":"band_crimson","label":"Crimson","threshold_msv":400}`
  - row 9: `{"disposition":"Critical accumulation. Comfort care is the first option on the ledger.","id":"band_violet","label":"Violet","threshold_msv":500}`
  - row 10: `{"disposition":"The band the registrar will not soften. Still on the roster.","id":"band_black","label":"Black","threshold_msv":600}`
  - row 11: `{"disposition":"Terminal exposure. Palliative only. The clockmaker does not correct the reading.","id":"band_indigo","label":"Indigo","threshold_msv":800}`
  - row 12: `{"disposition":"Lethal accumulation. The registrar stops reading aloud. The column is filled in silence.","id":"band_void","label":"Void","threshold_msv":1000}`
- `plans`: list[8]; union fields: `cost, id, label, note`
  - row 1: `{"cost":"morphine","id":"plan_morphine_tray","label":"Morphine tray","note":"Mercy with a schedule. The tray is refilled on the same day every week."}`
  - row 2: `{"cost":"time","id":"plan_comfort_rounds","label":"Comfort rounds","note":"Someone sits the night. The sick room has a chair for it now."}`
  - row 3: `{"cost":"none","id":"plan_nothing","label":"Nothing","note":"A refusal is a choice the ledger records as silence."}`
  - row 4: `{"cost":"chelation_agent","id":"plan_chelation","label":"Chelation therapy","note":"A chemical course that binds heavy isotopes and moves them out. Expensive, limited, and worth counting."}`
  - row 5: `{"cost":"potassium_iodide","id":"plan_iodine_prophylaxis","label":"Iodine prophylaxis","note":"Thyroid saturation before the dose climbs further. The window is narrow; timing is the skill."}`
  - row 6: `{"cost":"time","id":"plan_isolation","label":"Isolation","note":"The patient is removed from shared air and touch. Secondary exposure stops here."}`
  - row 7: `{"cost":"time","id":"plan_rest","label":"Supervised rest","note":"Bed rest with hourly observation. Nothing curative; everything supportive."}`
  - row 8: `{"cost":"fuel","id":"plan_transfer","label":"Transfer","note":"If a better facility exists and fuel permits, the patient goes. The ledger follows."}`
- `guesses`: list[3]; union fields: `id, label, note, pencil`
  - row 1: `{"id":"guess_low","label":"Low","note":"A kinder story, written in pencil.","pencil":true}`
  - row 2: `{"id":"guess_honest","label":"Honest","note":"The truer number as it stands, subject to correction.","pencil":true}`
  - row 3: `{"id":"guess_refused","label":"Refused","note":"No guess at all. The board stays blank until the dial speaks.","pencil":true}`
- `calibration`: object[2]
- `registers`: list[4]; union fields: `action, id, label`
  - row 1: `{"action":"Book a reading","id":"register_ledger","label":"Ledger"}`
  - row 2: `{"action":"Name to the sick list","id":"register_sick","label":"Sick"}`
  - row 3: `{"action":"Book a baseline","id":"register_cohort","label":"Cohort"}`
  - row 4: `{"action":"Sign an hour","id":"register_voluntary","label":"Voluntary"}`
- `npcs`: list[4]; union fields: `action, action_label, disposition, id, name, register`
  - row 1: `{"action":"book_reading","action_label":"Book a reading","disposition":"Radiation Registrar. Holds a red pencil and will not read a number up to make it easier. Every refused booking is a silent tally she remembers.","id":"npc_dr_irina_vel","name":"Dr. Irina Vel","register":"register_ledger"}`
  - row 2: `{"action":"name_to_sick_list","action_label":"Name to the sick list","disposition":"Sick-room Nurse. Presents the bed order and waits. She never shuffles the order so one person's comfort costs another's care.","id":"npc_wyn_omah","name":"Wyn Omah","register":"register_sick"}`
  - row 3: `{"action":"calibrate","action_label":"Calibrate the dial","disposition":"The Clockmaker. The only honest voice about error: every figure has a drift, the drift is normal, and he will not pretend otherwise.","id":"npc_piet_abar","name":"Piet Abar","register":"register_ledger"}`
  - row 4: `{"action":"book_baseline","action_label":"Book a baseline","disposition":"The Midwife. Keeps the children's board in chalk so it can be erased. She will not book a guess as a truth.","id":"npc_saria_voss","name":"Saria Voss","register":"register_cohort"}`

## `Assets/StreamingAssets/Data/medical_texts.json`
- Bytes: 222,244; SHA-256: `867ac96ebabc9c40db200106c84a8f8fd2e0f5c21650a737aa3678312d548347`
- Root keys: `collection_id, conditions, schema_version`
- `conditions`: list[83]; union fields: `category, complication_warnings, diagnosis_text, display_name, emotional_impact, failure_consequences, id, long_term_effects, mental_state, pain_descriptions, physical_state, prevention_advice, recovery_descriptions, required_items, success_chances, symptom_descriptions, system_integration, treatment_steps`
  - row 1: `{"category":"injury","complication_warnings":["Watch for redness spreading from the wound. That's infection.","Watch for fever. That's your body fighting something.","Watch for pus. That's the wound telling you it's not clean."],"diagnosis_text":"Laceration. Deep, edges ragged, bleeding steady. Needs closing.","display_name":"Laceration","emotional_impact":"Another scar. Another story. Another day you survived.","failure_consequences":["Infection sets in. The wound becomes hot, red, swollen. Fever follows.","The wound reopens. Bleeding resumes. You're back where you started.","Scarring. The skin heals but the mark remains. A reminder."],"id"…`
  - row 2: `{"category":"injury","complication_warnings":["Watch for signs of infection: redness, swelling, pus, fever.","Watch for shock: pale skin, rapid breathing, confusion.","Watch for dehydration: burns dry out your skin and body."],"diagnosis_text":"Red, blistered skin, peeling at the edges. The pain is constant, sharp, like the skin is still on fire. It needs cooling and covering.","display_name":"Burn","emotional_impact":"Another mark. Another lesson. Another day you learned something the hard way.","failure_consequences":["Infection. Burns are highly susceptible to infection.","Scarring. Burns scar worse than cuts.","Shock. Severe burns can ca…`
  - row 3: `{"category":"injury","complication_warnings":["Watch for numbness below the fracture. That's nerve damage.","Watch for coldness below the fracture. That's blood vessel damage.","Watch for infection if the bone broke the skin."],"diagnosis_text":"The bone is broken. You can feel it — the wrong angle, the grinding, the pain that makes you see white. It needs setting and splinting.","display_name":"Fracture","emotional_impact":"Another break. Another lesson. Another day you learned something the hard way.","failure_consequences":["The bone heals wrong. The limb is permanently deformed.","Infection. Open fractures are highly susceptible to infec…`
  - row 4: `{"category":"illness","complication_warnings":["Watch for vomiting. That's a sign of severe exposure.","Watch for hair loss. That's a sign of acute radiation syndrome.","Watch for bleeding gums. That's a sign of bone marrow damage."],"diagnosis_text":"Acute radiation exposure. Dose received; magnitude not yet known.","display_name":"Radiation Exposure","emotional_impact":"Another exposure. Another calculation. Another day you survived the invisible enemy.","failure_consequences":["Acute radiation sickness. Nausea, vomiting, diarrhea, fever.","Chronic illness. Long-term health effects.","Death. In severe cases, radiation exposure is fatal."],…`
  - row 5: `{"category":"illness","complication_warnings":["Watch for red streaks spreading from the wound. That's blood poisoning.","Watch for high fever. That's your body losing the fight.","Watch for confusion. That's the infection affecting your brain."],"diagnosis_text":"Infection. Fever and spreading inflammation; source not localised.","display_name":"Infection","emotional_impact":"Another infection. Another fight. Another day you survived the invisible enemy.","failure_consequences":["The infection spreads. Other wounds become infected.","Sepsis. The infection enters your bloodstream.","Amputation. In severe cases, the limb is lost."],"id":"medi…`
  - row 6: `{"category":"illness","complication_warnings":["Watch for dark urine. That's your body telling you it needs more water.","Watch for dizziness. That's your blood pressure dropping.","Watch for confusion. That's your brain shutting down."],"diagnosis_text":"Dehydration. Intake below loss; urine dark, membranes dry.","display_name":"Dehydration","emotional_impact":"Another day without enough water. Another day you survived the desert.","failure_consequences":["Organ failure. Your kidneys shut down first.","Heat stroke. Your body can't cool itself.","Death. Dehydration is fatal if untreated."],"id":"medical_dehydration","long_term_effects":["Kid…`
  - row 7: `{"category":"illness","complication_warnings":["Watch for refeeding syndrome. Eating too much too fast can kill you.","Watch for nausea. Your stomach can't handle too much.","Watch for weakness. Your muscles need time to recover."],"diagnosis_text":"Starvation. Caloric deficit sustained; the body is consuming itself.","display_name":"Starvation","emotional_impact":"Another day without enough food. Another day you survived the famine.","failure_consequences":["Organ failure. Your body consumes its own organs.","Cognitive impairment. Your brain shuts down before your body.","Death. Starvation is fatal if untreated."],"id":"medical_starvation",…`
  - row 8: `{"category":"illness","complication_warnings":["Watch for frostbite. White, numb skin is a bad sign.","Watch for confusion. That's your brain shutting down.","Watch for drowsiness. That's your body giving up."],"diagnosis_text":"Hypothermia. Core cooling faster than the body can replace it.","display_name":"Hypothermia","emotional_impact":"Another cold day. Another day you survived the winter.","failure_consequences":["Organ failure. Your body shuts down from the cold.","Frostbite. Your extremities freeze and die.","Death. Hypothermia is fatal if untreated."],"id":"medical_hypothermia","long_term_effects":["Increased sensitivity to cold. You…`
  - row 9: `{"category":"illness","complication_warnings":["Watch for dry skin. That's your body giving up on cooling.","Watch for confusion. That's your brain cooking.","Watch for drowsiness. That's your body shutting down."],"diagnosis_text":"Heatstroke. Sweating has stopped; core temperature still climbing.","display_name":"Heatstroke","emotional_impact":"Another hot day. Another day you survived the desert.","failure_consequences":["Organ failure. Your body cooks from the inside.","Brain damage. Your brain is sensitive to heat.","Death. Heatstroke is fatal if untreated."],"id":"medical_heatstroke","long_term_effects":["Increased sensitivity to heat.…`
  - row 10: `{"category":"mental","complication_warnings":["Watch for hyperventilation. That's breathing too fast.","Watch for dizziness. That's your brain getting too much oxygen.","Watch for chest pain. That's your heart working too hard."],"diagnosis_text":"Panic attack. Acute hyperventilation; no physical cause found.","display_name":"Panic Attack","emotional_impact":"Another attack. Another fight. Another day you survived your own mind.","failure_consequences":["Hyperventilation. You breathe too fast and pass out.","Injury. You fall or run into something during the attack.","Recurring attacks. The fear of attacks causes more attacks."],"id":"medical…`
  - row 11: `{"category":"mental","complication_warnings":["Watch for hallucinations. That's your brain making things up.","Watch for confusion. That's your brain shutting down.","Watch for weakness. That's your body giving out."],"diagnosis_text":"Insomnia. Sleep debt compounding; cognition degrading with it.","display_name":"Insomnia","emotional_impact":"Another sleepless night. Another day you survived your own mind.","failure_consequences":["Cognitive impairment. Your brain stops working properly.","Hallucinations. Your brain makes things up.","Physical collapse. Your body gives out."],"id":"medical_insomnia","long_term_effects":["Cognitive impairmen…`
  - row 12: `{"category":"complication","complication_warnings":["Watch for red streaks. That's blood poisoning.","Watch for high fever. That's your body losing the fight.","Watch for confusion. That's the infection affecting your brain."],"diagnosis_text":"Septic wound. Localised infection, purulent, tracking up the limb.","display_name":"Wound Infection","emotional_impact":"Another infection. Another fight. Another day you survived the invisible enemy.","failure_consequences":["Sepsis. The infection enters your bloodstream.","Amputation. The infection destroys the limb.","Death. Untreated wound infections are fatal."],"id":"medical_wound_infection","lo…`
  - row 13: `{"category":"injury","complication_warnings":["Watch for blisters. That's the tissue dying.","Watch for black skin. That's tissue death.","Watch for infection. Dead tissue attracts bacteria."],"diagnosis_text":"Your fingers are white. Your toes are numb. The skin is hard. Frostbite. The cold has won this round.","display_name":"Frostbite","emotional_impact":"Another frostbite. Another lesson. Another day you survived the cold.","failure_consequences":["Tissue death. The frostbitten area dies.","Amputation. Dead tissue must be removed.","Permanent damage. The frostbite leaves marks."],"id":"medical_frostbite","long_term_effects":["Increased s…`
  - row 14: `{"category":"injury","complication_warnings":["Watch for vomiting. That's a sign of brain swelling.","Watch for worsening symptoms. That's a sign of bleeding.","Watch for loss of consciousness. That's a sign of severe injury."],"diagnosis_text":"Your head is ringing. Your vision is blurry. Your balance is off. You hit your head. Hard.","display_name":"Concussion","emotional_impact":"Another concussion. Another hit. Another day you survived the impact.","failure_consequences":["Brain damage. Concussions can cause permanent damage.","Second impact syndrome. Another concussion can be fatal.","Chronic traumatic encephalopathy. Long-term brain da…`
  - row 15: `{"category":"emergency","complication_warnings":["Watch for seizures. That's your brain shutting down.","Watch for no urine. That's your kidneys failing.","Watch for confusion. That's your brain shutting down."],"diagnosis_text":"Severe dehydration. Volume deficit critical; seizure is the risk.","display_name":"Severe Dehydration","emotional_impact":"Another day without water. Another day you survived the desert.","failure_consequences":["Organ failure. Your kidneys shut down first.","Seizures. Your brain can't function without water.","Death. Severe dehydration is fatal within days."],"id":"medical_dehydration_severe","long_term_effects":["…`
  - row 16: `{"category":"surgery","complication_warnings":["Watch for bleeding. The stump must be sealed.","Watch for infection. The wound must be clean.","Watch for shock. The body must be supported."],"diagnosis_text":"The limb is dead. The tissue is black. The infection is spreading. You need to remove the limb or lose the patient.","display_name":"Amputation","emotional_impact":"Another amputation. Another life saved. Another body changed forever.","failure_consequences":["The patient bleeds out. The amputation was too slow.","Infection sets in. The stump is contaminated.","Shock. The body can't handle the trauma."],"id":"medical_amputation","long_t…`
  - row 17: `{"category":"surgery","complication_warnings":["Watch for infection. The wound must be clean.","Watch for stitch tearing. The wound must be supported.","Watch for scarring. The stitches must be even."],"diagnosis_text":"The wound is deep. The edges are clean. It needs to be closed properly or it won't heal right.","display_name":"Suturing","emotional_impact":"Another suture. Another wound closed. Another life saved.","failure_consequences":["The stitches tear. The wound reopens.","Infection sets in. The wound is contaminated.","Scarring. The wound heals but leaves a mark."],"id":"medical_suturing","long_term_effects":["Scarring. Sutures leav…`
  - row 18: `{"category":"chronic","complication_warnings":["Watch for organ failure. That's the radiation winning.","Watch for cancer. That's the radiation mutating.","Watch for death. That's the radiation succeeding."],"diagnosis_text":"Chronic radiation illness. Accumulated dose; no clearance pathway.","display_name":"Chronic Radiation Illness","emotional_impact":"Another day with chronic radiation. Another day you survived the invisible enemy.","failure_consequences":["Organ failure. The radiation destroys your organs.","Cancer. The radiation mutates your cells.","Death. The body can't fight forever."],"id":"medical_chronic_radiation","long_term_effe…`
  - row 19: `{"category":"chronic","complication_warnings":["Watch for flare-ups. That's the illness returning.","Watch for complications. That's the illness spreading.","Watch for decline. That's the illness winning."],"diagnosis_text":"The illness won't go away. It lingers, flares up, recedes, returns. Your body is fighting a war of attrition.","display_name":"Chronic Illness","emotional_impact":"Another day with chronic illness. Another day you survived the invisible enemy.","failure_consequences":["The illness worsens. The symptoms become debilitating.","Complications arise. The illness affects other systems.","Death. The body can't fight forever."],…`
  - row 20: `{"category":"chronic","complication_warnings":["Watch for cravings. That's the addiction calling.","Watch for triggers. That's the addiction waiting.","Watch for relapse. That's the addiction winning."],"diagnosis_text":"Chemical dependence. Dosing interval shortening; withdrawal onset.","display_name":"Addiction","emotional_impact":"Another day fighting addiction. Another day you survived the craving.","failure_consequences":["Relapse. You start using again.","Overdose. You take too much trying to feel the same.","Death. The substance wins."],"id":"medical_addiction","long_term_effects":["Reduced capacity. The addiction limits what you can …`
  - row 21: `{"category":"mental","complication_warnings":["Watch for flashbacks. That's the trauma returning.","Watch for nightmares. That's the trauma haunting.","Watch for hypervigilance. That's the trauma controlling."],"diagnosis_text":"Post-traumatic stress. Intrusive recall, poor sleep, hypervigilance.","display_name":"Post-Traumatic Stress","emotional_impact":"Another day with PTSD. Another day you survived the memories.","failure_consequences":["The symptoms worsen. The flashbacks become more frequent.","The nightmares intensify. The sleep becomes impossible.","The hypervigilance becomes debilitating. The world becomes too much."],"id":"medical_…`
  - row 22: `{"category":"mental","complication_warnings":["Watch for sadness. That's the depression returning.","Watch for fatigue. That's the depression weighing.","Watch for hopelessness. That's the depression winning."],"diagnosis_text":"The weight won't lift. The darkness won't clear. The hope won't return. You're depressed. The world is gray.","display_name":"Depression","emotional_impact":"Another day with depression. Another day you survived the darkness.","failure_consequences":["The depression worsens. The sadness becomes debilitating.","The fatigue intensifies. The body becomes impossible to move.","The hopelessness becomes total. The future b…`
  - row 23: `{"category":"prevention","complication_warnings":["Watch for infection. That's the wound telling you it's not clean.","Watch for reopening. That's the wound telling you it's not closed.","Watch for scarring. That's the wound telling you it's not healed."],"diagnosis_text":"The wound is fresh. The bleeding is controlled. It needs to be cleaned and dressed to prevent infection.","display_name":"Wound Care","emotional_impact":"Another wound cared for. Another infection prevented. Another life saved.","failure_consequences":["Infection sets in. The wound becomes contaminated.","The wound reopens. The bleeding resumes.","Scarring. The wound heals…`
  - row 24: `{"category":"prevention","complication_warnings":["Watch for dirty hands. That's the bacteria waiting.","Watch for soiled clothes. That's the contamination spreading.","Watch for unclean body. That's the infections developing."],"diagnosis_text":"You're dirty. Your hands are contaminated. Your clothes are soiled. Your body needs cleaning to prevent illness.","display_name":"Hygiene","emotional_impact":"Another day of hygiene. Another infection prevented. Another life saved.","failure_consequences":["Infection. The bacteria enter your body.","Disease. The contamination spreads.","Illness. The body can't fight everything."],"id":"medical_hygie…`
  - row 25: `{"category":"prevention","complication_warnings":["Watch for hunger. That's the body telling you it needs fuel.","Watch for weakness. That's the body telling you it needs protein.","Watch for fog. That's the body telling you it needs glucose."],"diagnosis_text":"You're hungry. Your body needs fuel. Your muscles need protein. Your brain needs glucose. You need to eat.","display_name":"Nutrition","emotional_impact":"Another day of hunger. Another day you survived the famine.","failure_consequences":["Starvation. The body consumes itself.","Weakness. The muscles fail.","Death. The body can't survive without food."],"id":"medical_nutrition","lon…`
  - row 26: `{"category":"prevention","complication_warnings":["Watch for fatigue. That's the body telling you it needs rest.","Watch for drowsiness. That's the body telling you it needs sleep.","Watch for weakness. That's the body telling you it needs recovery."],"diagnosis_text":"You're tired. Your body needs rest. Your muscles need recovery. Your brain needs sleep. You need to stop.","display_name":"Rest","emotional_impact":"Another day without rest. Another day you survived the exhaustion.","failure_consequences":["Collapse. The body gives out.","Injury. The tired body makes mistakes.","Death. The body can't survive without rest."],"id":"medical_rest…`
  - row 27: `{"category":"dental","complication_warnings":["Watch for infection. That's the socket telling you it's not clean.","Watch for swelling. That's the infection spreading.","Watch for fever. That's your body fighting."],"diagnosis_text":"The pain is sharp, constant, in your jaw. The tooth is broken or infected. The nerve is exposed. You need to treat it or lose the tooth.","display_name":"Toothache","emotional_impact":"Another toothache. Another pain. Another day you survived the agony.","failure_consequences":["Infection spreads. The jaw becomes swollen.","The tooth breaks. The root remains.","The pain continues. The infection worsens."],"id":"…`
  - row 28: `{"category":"eye_care","complication_warnings":["Watch for spreading. That's the infection moving.","Watch for vision loss. That's the infection damaging.","Watch for fever. That's your body fighting."],"diagnosis_text":"Your eye is red, swollen, watering. The infection is spreading. The vision is blurry. You need to treat it or lose the eye.","display_name":"Eye Infection","emotional_impact":"Another eye infection. Another threat to your vision. Another day you survived.","failure_consequences":["The infection spreads. The other eye becomes infected.","Vision loss. The infection damages the eye.","Blindness. The infection destroys the eye."…`
  - row 29: `{"category":"reproductive","complication_warnings":["Watch for bleeding. That's a sign of trouble.","Watch for severe pain. That's a sign of complications.","Watch for fever. That's a sign of infection."],"diagnosis_text":"The test is positive. You're pregnant. The world is ending, but life goes on. You need to take care of yourself and the baby.","display_name":"Pregnancy","emotional_impact":"Another day of pregnancy. Another day of hope. Another day you survived.","failure_consequences":["Miscarriage. The body can't support the pregnancy.","Complications. The pregnancy becomes dangerous.","Premature birth. The baby comes too early."],"id":…`
  - row 30: `{"category":"reproductive","complication_warnings":["Watch for bleeding. That's a sign of hemorrhage.","Watch for fever. That's a sign of infection.","Watch for distress. That's a sign of complications."],"diagnosis_text":"The contractions have started. The baby is coming. The world is ending, but life goes on. You need to deliver the baby safely.","display_name":"Childbirth","emotional_impact":"Another birth. Another life. Another day you survived.","failure_consequences":["Complications. The delivery becomes dangerous.","Hemorrhage. The mother bleeds too much.","Infection. The delivery site becomes contaminated."],"id":"medical_childbirth"…`
  - row 31: `{"category":"reproductive","complication_warnings":["Watch for fever. That's a sign of infection.","Watch for crying. That's a sign of distress.","Watch for not eating. That's a sign of illness."],"diagnosis_text":"The baby is here. It's small, fragile, helpless. It needs warmth, food, and protection. You need to keep it alive.","display_name":"Newborn Care","emotional_impact":"Another day of care. Another day of hope. Another day you survived.","failure_consequences":["The baby gets sick. The immune system is weak.","The baby gets cold. The body can't regulate temperature.","The baby dies. The world is too harsh."],"id":"medical_newborn_car…`
  - row 32: `{"category":"injury","complication_warnings":["Watch for infection. That's the broken cartilage telling you it's not clean.","Watch for continued bleeding. That's the nose telling you it's not set.","Watch for breathing difficulty. That's the nose telling you it's blocked."],"diagnosis_text":"The blood is flowing. The nose is bent. The pain is sharp. You broke your nose. It needs to be set or it will heal crooked.","display_name":"Broken Nose","emotional_impact":"Another broken nose. Another scar. Another day you survived the impact.","failure_consequences":["The nose heals crooked. The breathing is permanently affected.","Infection sets in.…`
  - row 33: `{"category":"injury","complication_warnings":["Watch for numbness. That's nerve damage.","Watch for weakness. That's the joint not healing.","Watch for re-dislocation. That's the joint being unstable."],"diagnosis_text":"The arm is hanging wrong. The shoulder is out of the socket. The pain is sharp. You need to pop it back in.","display_name":"Dislocated Shoulder","emotional_impact":"Another dislocation. Another pain. Another day you survived the injury.","failure_consequences":["The shoulder stays out. The arm is useless.","Nerve damage. The arm loses feeling.","The shoulder pops out again. The joint is weakened."],"id":"medical_dislocated_…`
  - row 34: `{"category":"injury","complication_warnings":["Watch for re-injury. That's the ligaments being weak.","Watch for weakness. That's the wrist not healing.","Watch for pain. That's the wrist telling you it's not ready."],"diagnosis_text":"The wrist is swollen. The pain is sharp. The movement is limited. You sprained your wrist. It needs rest and support.","display_name":"Sprained Wrist","emotional_impact":"Another sprain. Another pain. Another day you survived the injury.","failure_consequences":["The wrist stays weak. The grip is permanently affected.","The wrist re-injures. The ligaments are weakened.","The pain continues. The wrist doesn't h…`
  - row 35: `{"category":"injury","complication_warnings":["Watch for broken ribs. That's the bruise becoming a fracture.","Watch for pneumonia. That's the breathing difficulty becoming infection.","Watch for continued pain. That's the ribs not healing."],"diagnosis_text":"The pain is sharp when you breathe. The ribs are bruised. The breathing is difficult. You need to rest and protect your chest.","display_name":"Bruised Ribs","emotional_impact":"Another bruise. Another pain. Another day you survived the injury.","failure_consequences":["The ribs break. The bruise becomes a fracture.","Pneumonia. The breathing difficulty leads to infection.","The pain c…`
  - row 36: `{"category":"injury","complication_warnings":["Watch for infection. That's the wounds telling you they're not clean.","Watch for reopening. That's the wounds telling you they're not closed.","Watch for scarring. That's the wounds telling you they're not healed."],"diagnosis_text":"The skin is broken. The blood is flowing. The pain is sharp. You have cuts and scrapes. They need cleaning and covering.","display_name":"Cuts and Scrapes","emotional_impact":"Another cut. Another scrape. Another day you survived the injury.","failure_consequences":["Infection sets in. The wounds become contaminated.","Scarring. The wounds heal but leave marks.","T…`
  - row 37: `{"category":"injury","complication_warnings":["Watch for infection. That's the blister telling you it's not clean.","Watch for reopening. That's the blister telling you it's not healed.","Watch for pain. That's the blister telling you it's not ready."],"diagnosis_text":"The blister is full of fluid. The skin is tight. The pain is sharp. You need to drain it or protect it.","display_name":"Blister Care","emotional_impact":"Another blister. Another pain. Another day you survived the friction.","failure_consequences":["Infection sets in. The blister becomes contaminated.","The blister reopens. The raw skin is exposed.","The pain continues. The …`
  - row 38: `{"category":"poisoning","complication_warnings":["Watch for dehydration. That's the fluid loss winning.","Watch for blood in vomit. That's a sign of serious damage.","Watch for fever. That's a sign of infection."],"diagnosis_text":"The food was bad. Your stomach is turning. Your body is rejecting what you ate. You need to let it pass.","display_name":"Food Poisoning","emotional_impact":"Another bout of food poisoning. Another day you survived the bad food.","failure_consequences":["Dehydration. You lose too much fluid.","The poisoning worsens. The bad food stays in your system.","Shock. Your body can't handle the stress."],"id":"medical_food…`
  - row 39: `{"category":"poisoning","complication_warnings":["Watch for organ failure. That's the chemical winning.","Watch for nerve damage. That's the chemical affecting your nervous system.","Watch for breathing difficulty. That's the chemical affecting your lungs."],"diagnosis_text":"The chemical is in your system. Your body is reacting. Your vision is blurry. Your stomach is burning. You need help.","display_name":"Chemical Poisoning","emotional_impact":"Another chemical exposure. Another day you survived the poison.","failure_consequences":["Organ failure. The chemical destroys your organs.","Nerve damage. The chemical affects your nervous system.…`
  - row 40: `{"category":"allergic","complication_warnings":["Watch for throat swelling. That's the reaction closing your airway.","Watch for shock. That's your body giving up.","Watch for breathing difficulty. That's the reaction affecting your lungs."],"diagnosis_text":"The reaction is starting. Your skin is itching. Your throat is swelling. Your body is rejecting something. You need to act fast.","display_name":"Allergic Reaction","emotional_impact":"Another allergic reaction. Another day you survived the allergen.","failure_consequences":["Anaphylaxis. Your throat closes completely.","Shock. Your body can't handle the reaction.","Death. Severe allerg…`
  - row 41: `{"category":"bite","complication_warnings":["Watch for infection. That's the bite telling you it's not clean.","Watch for allergic reaction. That's your body reacting to the venom.","Watch for disease. That's the insect's gift."],"diagnosis_text":"The bite is red, swollen, itchy. The insect got you. It's not serious, but it's annoying. You need to treat it.","display_name":"Insect Bite","emotional_impact":"Another insect bite. Another annoyance. Another day you survived the bugs.","failure_consequences":["Infection. The bite becomes contaminated.","Allergic reaction. Your body reacts to the insect venom.","Disease. The insect may have been c…`
  - row 42: `{"category":"bite","complication_warnings":["Watch for infection. That's the wound telling you it's not clean.","Watch for disease. That's the animal's gift.","Watch for fever. That's your body fighting."],"diagnosis_text":"The animal bit you. The wound is deep. The bleeding is steady. The animal may have been carrying disease. You need to treat it fast.","display_name":"Animal Bite","emotional_impact":"Another animal bite. Another wound. Another day you survived the wild.","failure_consequences":["Infection. The wound becomes contaminated.","Disease. The animal may have been carrying rabies or other diseases.","The wound doesn't heal. The d…`
  - row 43: `{"category":"bite","complication_warnings":["Watch for breathing difficulty. That's the venom affecting your lungs.","Watch for vision changes. That's the venom affecting your nervous system.","Watch for swelling spreading. That's the venom moving."],"diagnosis_text":"The snake bit you. The venom is spreading. Your arm is swelling. Your vision is blurry. You need antivenom fast.","display_name":"Snake Bite","emotional_impact":"Another snake bite. Another venom. Another day you survived the wild.","failure_consequences":["The venom spreads. Your organs shut down.","Nerve damage. The venom affects your nervous system permanently.","Death. Snak…`
  - row 44: `{"category":"bite","complication_warnings":["Watch for infection. That's the bite telling you it's not clean.","Watch for allergic reaction. That's your body reacting to the venom.","Watch for necrosis. That's the venom destroying tissue."],"diagnosis_text":"The spider bit you. The bite is red, swollen, painful. The spider may have been venomous. You need to treat it.","display_name":"Spider Bite","emotional_impact":"Another spider bite. Another annoyance. Another day you survived the bugs.","failure_consequences":["Infection. The bite becomes contaminated.","Allergic reaction. Your body reacts to the spider venom.","Necrosis. The venom dest…`
  - row 45: `{"category":"bite","complication_warnings":["Watch for infection. That's the wound telling you it's not clean.","Watch for rabies. That's the dog's gift.","Watch for fever. That's your body fighting."],"diagnosis_text":"The dog bit you. The wound is deep. The bleeding is steady. The dog may have been rabid. You need to treat it fast.","display_name":"Dog Bite","emotional_impact":"Another dog bite. Another wound. Another day you survived the wild.","failure_consequences":["Infection. The wound becomes contaminated.","Rabies. The dog may have been rabid.","The wound doesn't heal. The damage is too severe."],"id":"medical_dog_bite","long_term_e…`
  - row 46: `{"category":"bite","complication_warnings":["Watch for infection. That's the scratches telling you they're not clean.","Watch for cat scratch fever. That's the cat's gift.","Watch for swelling. That's the infection spreading."],"diagnosis_text":"The cat scratched you. The wound is shallow but dirty. The bleeding is minimal. The cat may have been carrying disease.","display_name":"Cat Scratch","emotional_impact":"Another cat scratch. Another annoyance. Another day you survived the pets.","failure_consequences":["Infection. The scratches become contaminated.","Cat scratch fever. The cat may have been carrying the bacteria.","The scratches don'…`
  - row 47: `{"category":"bite","complication_warnings":["Watch for broken bones. That's the kick shattering something.","Watch for internal bleeding. That's the kick damaging your organs.","Watch for breathing difficulty. That's the kick affecting your lungs."],"diagnosis_text":"The horse kicked you. The impact was massive. Your body is broken. You need medical attention fast.","display_name":"Horse Kick","emotional_impact":"Another horse kick. Another broken body. Another day you survived the animals.","failure_consequences":["Broken bones. The kick shattered something.","Internal bleeding. The kick damaged your organs.","Death. Horse kicks can be fata…`
  - row 48: `{"category":"surgery","complication_warnings":["Watch for nerve damage. That's the setting affecting the nerves.","Watch for re-breakage. That's the setting not holding.","Watch for infection. That's the wound not healing."],"diagnosis_text":"The bone is broken. It needs to be set back into place. The pain will be intense. The recovery will be long.","display_name":"Bone Setting","emotional_impact":"Another bone setting. Another broken body. Another day you survived the injury.","failure_consequences":["The bone heals crooked. The limb is permanently deformed.","Nerve damage. The setting damaged the nerves.","The bone re-breaks. The setting …`
  - row 49: `{"category":"surgery","complication_warnings":["Watch for infection spreading. That's the debridement not being thorough.","Watch for shock. That's the pain being too much.","Watch for bleeding. That's the debridement being too aggressive."],"diagnosis_text":"The wound is infected. The dead tissue needs to be removed. The wound needs to be cleaned. The pain will be intense.","display_name":"Wound Debridement","emotional_impact":"Another debridement. Another infected wound. Another day you survived the infection.","failure_consequences":["The infection spreads. The wound becomes worse.","The wound doesn't heal. The damage is too severe.","The…`
  - row 50: `{"category":"rehabilitation","complication_warnings":["Watch for re-injury. That's the therapy being too aggressive.","Watch for pain. That's the therapy pushing too hard.","Watch for frustration. That's the therapy taking too long."],"diagnosis_text":"The injury has healed. The limb is weak. The movement is limited. You need to rebuild strength and flexibility.","display_name":"Physical Therapy","emotional_impact":"Another therapy session. Another day of recovery. Another day you survived the injury.","failure_consequences":["The limb stays weak. The movement stays limited.","The injury reoccurs. The therapy was not enough.","The pain conti…`
  - row 51: `{"category":"mental_health","complication_warnings":["Watch for worsening symptoms. That's the counseling not working.","Watch for suicidal thoughts. That's the depression winning.","Watch for substance abuse. That's the coping mechanism failing."],"diagnosis_text":"The trauma is deep. The memories won't stop. The nightmares won't end. You need to talk to someone.","display_name":"Counseling","emotional_impact":"Another counseling session. Another day of recovery. Another day you survived the trauma.","failure_consequences":["The trauma worsens. The symptoms become debilitating.","The depression deepens. The hope disappears.","The anxiety in…`
  - row 52: `{"category":"mental_health","complication_warnings":["Watch for side effects. That's the medication harming you.","Watch for dependency. That's the medication becoming addictive.","Watch for withdrawal. That's the medication being stopped too quickly."],"diagnosis_text":"The symptoms are severe. The counseling is not enough. You need medication to manage the symptoms.","display_name":"Medication Management","emotional_impact":"Another day of medication. Another day of recovery. Another day you survived the symptoms.","failure_consequences":["The symptoms worsen. The medication is not enough.","The side effects are severe. The medication is h…`
  - row 53: `{"category":"mental_health","complication_warnings":["Watch for worsening grief. That's the counseling not working.","Watch for suicidal thoughts. That's the grief winning.","Watch for substance abuse. That's the coping mechanism failing."],"diagnosis_text":"The loss is deep. The grief is overwhelming. The sadness is constant. You need to process the loss.","display_name":"Grief Counseling","emotional_impact":"Another grief counseling session. Another day of recovery. Another day you survived the loss.","failure_consequences":["The grief worsens. The sadness becomes debilitating.","The depression deepens. The hope disappears.","The anger inc…`
  - row 54: `{"category":"mental_health","complication_warnings":["Watch for worsening symptoms. That's the therapy not working.","Watch for suicidal thoughts. That's the trauma winning.","Watch for substance abuse. That's the coping mechanism failing."],"diagnosis_text":"The trauma is deep. The memories are vivid. The nightmares are constant. You need specialized therapy.","display_name":"Trauma Therapy","emotional_impact":"Another trauma therapy session. Another day of recovery. Another day you survived the trauma.","failure_consequences":["The trauma worsens. The symptoms become debilitating.","The flashbacks increase. The triggers multiply.","The hyp…`
  - row 55: `{"category":"mental_health","complication_warnings":["Watch for worsening stress. That's the management not working.","Watch for health problems. That's the stress affecting the body.","Watch for burnout. That's the stress winning."],"diagnosis_text":"The stress is overwhelming. The pressure is constant. The anxiety is high. You need to learn to manage the stress.","display_name":"Stress Management","emotional_impact":"Another stress management session. Another day of recovery. Another day you survived the pressure.","failure_consequences":["The stress worsens. The symptoms become debilitating.","The anxiety increases. The panic becomes cons…`
  - row 56: `{"category":"chronic_disease","complication_warnings":["Watch for low blood sugar. That's the insulin working too well.","Watch for high blood sugar. That's the diabetes winning.","Watch for organ damage. That's the sugar destroying your body."],"diagnosis_text":"Your blood sugar is unstable. Your body can't regulate it. You need insulin or you'll die.","display_name":"Diabetes","emotional_impact":"Another day with diabetes. Another day of management. Another day you survived the disease.","failure_consequences":["Diabetic coma. Your blood sugar crashes.","Organ failure. The sugar damages your organs.","Death. Diabetes is fatal without treat…`
  - row 57: `{"category":"chronic_disease","complication_warnings":["Watch for worsening symptoms. That's the asthma getting worse.","Watch for triggers. That's the asthma being provoked.","Watch for suffocation. That's the airways closing."],"diagnosis_text":"Airway constriction. Wheeze on exhale. Inhaler needed to sustain.","display_name":"Asthma","emotional_impact":"Another asthma attack. Another day of breathing difficulty. Another day you survived the disease.","failure_consequences":["Suffocation. Your airways close completely.","Respiratory failure. Your lungs give out.","Death. Asthma can be fatal without treatment."],"id":"medical_asthma","long_…`
  - row 58: `{"category":"chronic_disease","complication_warnings":["Watch for worsening pain. That's the arthritis getting worse.","Watch for joint damage. That's the inflammation destroying the joints.","Watch for reduced mobility. That's the joints stopping working."],"diagnosis_text":"Your joints are inflamed. Your movement is painful. Your body is attacking itself.","display_name":"Arthritis","emotional_impact":"Another day with arthritis. Another day of pain. Another day you survived the disease.","failure_consequences":["Joint damage. The inflammation destroys the joints.","Reduced mobility. The joints stop working.","Chronic pain. The arthritis i…`
  - row 59: `{"category":"chronic_disease","complication_warnings":["Watch for chest pain. That's the heart struggling.","Watch for shortness of breath. That's the lungs not getting enough oxygen.","Watch for fatigue. That's the body not getting enough blood."],"diagnosis_text":"Your heart is weak. Your circulation is poor. Your body is struggling to pump blood.","display_name":"Heart Disease","emotional_impact":"Another day with heart disease. Another day of fatigue. Another day you survived the disease.","failure_consequences":["Heart attack. Your heart gives out.","Stroke. Your brain doesn't get enough blood.","Death. Heart disease is fatal without tr…`
  - row 60: `{"category":"chronic_disease","complication_warnings":["Watch for spreading. That's the cancer moving.","Watch for pain. That's the tumor pressing on nerves.","Watch for fatigue. That's the body fighting the cancer."],"diagnosis_text":"The cells are growing out of control. The tumor is spreading. Your body is fighting itself.","display_name":"Cancer","emotional_impact":"Another day with cancer. Another day of fighting. Another day you survived the disease.","failure_consequences":["The cancer spreads. The tumor grows.","Organ failure. The cancer destroys organs.","Death. Cancer is fatal without treatment."],"id":"medical_cancer","long_term_e…`
  - row 61: `{"category":"chronic_disease","complication_warnings":["Watch for seizures. That's the brain misfiring.","Watch for triggers. That's the epilepsy being provoked.","Watch for injury. That's the seizure hurting the body."],"diagnosis_text":"Your brain is misfiring. The seizures are coming. Your body is out of control.","display_name":"Epilepsy","emotional_impact":"Another seizure. Another day of fear. Another day you survived the disease.","failure_consequences":["Injury during a seizure. The body convulses uncontrollably.","Status epilepticus. The seizure doesn't stop.","Death. Epilepsy can be fatal without treatment."],"id":"medical_epilepsy…`
  - row 62: `{"category":"chronic_disease","complication_warnings":["Watch for worsening pain. That's the chronic pain getting worse.","Watch for depression. That's the pain affecting the mind.","Watch for disability. That's the pain preventing normal function."],"diagnosis_text":"The pain won't stop. It's constant, burning, aching. Your body is in pain all the time.","display_name":"Chronic Pain","emotional_impact":"Another day of chronic pain. Another day of suffering. Another day you survived the pain.","failure_consequences":["The pain worsens. The body can't handle it.","Depression. The pain affects the mind.","Disability. The pain prevents normal f…`
  - row 63: `{"category":"chronic_disease","complication_warnings":["Watch for worsening fatigue. That's the chronic fatigue getting worse.","Watch for depression. That's the fatigue affecting the mind.","Watch for disability. That's the fatigue preventing normal function."],"diagnosis_text":"The fatigue won't stop. It's constant, overwhelming, debilitating. Your body is exhausted all the time.","display_name":"Chronic Fatigue","emotional_impact":"Another day of chronic fatigue. Another day of exhaustion. Another day you survived the fatigue.","failure_consequences":["The fatigue worsens. The body can't handle it.","Depression. The fatigue affects the mi…`
  - row 64: `{"category":"chronic_disease","complication_warnings":["Watch for worsening headache. That's the chronic headache getting worse.","Watch for nausea. That's the headache causing nausea.","Watch for disability. That's the headache preventing normal function."],"diagnosis_text":"The headache won't stop. It's constant, throbbing, pounding. Your head is in pain all the time.","display_name":"Chronic Headache","emotional_impact":"Another day of chronic headache. Another day of pain. Another day you survived the headache.","failure_consequences":["The headache worsens. The pain becomes unbearable.","Nausea. The headache causes nausea.","Disability.…`
  - row 65: `{"category":"chronic_disease","complication_warnings":["Watch for worsening back pain. That's the chronic back pain getting worse.","Watch for reduced mobility. That's the back not moving.","Watch for disability. That's the back pain preventing normal function."],"diagnosis_text":"The back pain won't stop. It's constant, aching, stabbing. Your back is in pain all the time.","display_name":"Chronic Back Pain","emotional_impact":"Another day of chronic back pain. Another day of pain. Another day you survived the back pain.","failure_consequences":["The back pain worsens. The pain becomes unbearable.","Reduced mobility. The back can't move.","D…`
  - row 66: `{"category":"parasitic","complication_warnings":["Watch for cerebral malaria. That's the parasite attacking your brain.","Watch for organ failure. That's the parasite destroying your organs.","Watch for dehydration. That's the fever drying you out."],"diagnosis_text":"The mosquito bite has infected you. The fever is coming. The chills are starting. You need treatment fast.","display_name":"Malaria","emotional_impact":"Another malaria attack. Another fever. Another day you survived the parasite.","failure_consequences":["Cerebral malaria. The parasite attacks your brain.","Organ failure. The parasite destroys your organs.","Death. Malaria is …`
  - row 67: `{"category":"parasitic","complication_warnings":["Watch for intestinal blockage. That's the tapeworm growing too large.","Watch for nutritional deficiency. That's the tapeworm eating your food.","Watch for organ damage. That's the tapeworm migrating."],"diagnosis_text":"Something is living inside you. The tapeworm is growing. Your stomach is upset. Your weight is dropping.","display_name":"Tapeworm","emotional_impact":"Another day with a tapeworm. Another day of discomfort. Another day you survived the parasite.","failure_consequences":["The tapeworm grows. It blocks your intestines.","Nutritional deficiency. The tapeworm eats your nutrients…`
  - row 68: `{"category":"nutritional","complication_warnings":["Watch for tooth loss. That's the scurvy destroying your gums.","Watch for anemia. That's the scurvy affecting your blood.","Watch for wound healing. That's the scurvy affecting your collagen."],"diagnosis_text":"Your gums are bleeding. Your teeth are loose. Your body is falling apart. You need vitamin C.","display_name":"Scurvy","emotional_impact":"Another day with scurvy. Another day of bleeding gums. Another day you survived the deficiency.","failure_consequences":["Tooth loss. The scurvy destroys your gums.","Anemia. The scurvy affects your blood.","Death. Scurvy is fatal without treatme…`
  - row 69: `{"category":"nutritional","complication_warnings":["Watch for heart failure. That's the beriberi destroying your heart.","Watch for nerve damage. That's the beriberi destroying your nerves.","Watch for weakness. That's the beriberi affecting your muscles."],"diagnosis_text":"Your heart is weak. Your nerves are damaged. Your body is deficient in vitamin B1.","display_name":"Beriberi","emotional_impact":"Another day with beriberi. Another day of weakness. Another day you survived the deficiency.","failure_consequences":["Heart failure. The beriberi destroys your heart.","Nerve damage. The beriberi destroys your nerves.","Death. Beriberi is fat…`
  - row 70: `{"category":"nutritional","complication_warnings":["Watch for skin damage. That's the pellagra destroying your skin.","Watch for diarrhea. That's the pellagra destroying your digestive system.","Watch for dementia. That's the pellagra affecting your mind."],"diagnosis_text":"Your skin is rough. Your mind is confused. Your body is deficient in vitamin B3.","display_name":"Pellagra","emotional_impact":"Another day with pellagra. Another day of skin damage. Another day you survived the deficiency.","failure_consequences":["Skin damage. The pellagra destroys your skin.","Digestive failure. The pellagra destroys your digestive system.","Death. Pe…`
  - row 71: `{"category":"nutritional","complication_warnings":["Watch for bone deformity. That's the rickets deforming your bones.","Watch for growth failure. That's the rickets stunting your growth.","Watch for fractures. That's the rickets making your bones fragile."],"diagnosis_text":"Your bones are weak. Your body is deficient in vitamin D. Your children are at risk.","display_name":"Rickets","emotional_impact":"Another day with rickets. Another day of bone pain. Another day you survived the deficiency.","failure_consequences":["Bone deformity. The rickets deforms your bones.","Growth failure. The rickets stunts your growth.","Fractures. The rickets…`
  - row 72: `{"category":"nutritional","complication_warnings":["Watch for heart failure. That's the anemia straining your heart.","Watch for organ damage. That's the anemia starving your organs.","Watch for fatigue. That's the anemia affecting your energy."],"diagnosis_text":"Your blood is weak. Your body is deficient in iron. You're tired all the time.","display_name":"Anemia","emotional_impact":"Another day with anemia. Another day of fatigue. Another day you survived the deficiency.","failure_consequences":["Heart failure. The anemia strains your heart.","Organ damage. The anemia starves your organs.","Death. Anemia can be fatal without treatment."],…`
  - row 73: `{"category":"nutritional","complication_warnings":["Watch for breathing difficulty. That's the goiter pressing on your airway.","Watch for swallowing difficulty. That's the goiter pressing on your esophagus.","Watch for thyroid failure. That's the goiter destroying your thyroid."],"diagnosis_text":"Your thyroid is swollen. Your neck is enlarged. Your body is deficient in iodine.","display_name":"Goiter","emotional_impact":"Another day with goiter. Another day of swelling. Another day you survived the deficiency.","failure_consequences":["Breathing difficulty. The goiter presses on your airway.","Swallowing difficulty. The goiter presses on y…`
  - row 74: `{"category":"wound_care","complication_warnings":["Watch for excessive bleeding. That's the wound not closing.","Watch for infection. That's the wound becoming contaminated.","Watch for fever. That's your body fighting."],"diagnosis_text":"The wound is deep. The flesh is torn. The bleeding is heavy. You need to close it or you'll bleed out.","display_name":"Deep Wound","emotional_impact":"Another deep wound. Another day of bleeding. Another day you survived the injury.","failure_consequences":["Excessive blood loss. You lose too much blood.","Infection. The wound becomes contaminated.","The wound doesn't heal. The damage is too severe."],"id…`
  - row 75: `{"category":"wound_care","complication_warnings":["Watch for internal damage. That's the puncture hitting organs.","Watch for infection. That's the wound becoming contaminated.","Watch for fever. That's your body fighting."],"diagnosis_text":"The wound is deep and narrow. Something punctured your flesh. The bleeding is minimal but the damage is deep.","display_name":"Puncture Wound","emotional_impact":"Another puncture wound. Another day of deep pain. Another day you survived the injury.","failure_consequences":["Internal damage. The puncture damaged organs.","Infection. The wound becomes contaminated.","The wound doesn't heal. The damage is…`
  - row 76: `{"category":"wound_care","complication_warnings":["Watch for excessive bleeding. That's the wound not closing.","Watch for infection. That's the wound becoming contaminated.","Watch for fever. That's your body fighting."],"diagnosis_text":"The flesh is torn away. The wound is ragged. The bleeding is heavy. You need to treat it fast.","display_name":"Avulsion","emotional_impact":"Another avulsion. Another day of torn flesh. Another day you survived the injury.","failure_consequences":["Excessive blood loss. You lose too much blood.","Infection. The wound becomes contaminated.","The wound doesn't heal. The damage is too severe."],"id":"medical…`
  - row 77: `{"category":"wound_care","complication_warnings":["Watch for excessive bleeding. That's the wound not closing.","Watch for infection. That's the wound becoming contaminated.","Watch for fever. That's your body fighting."],"diagnosis_text":"The wound is clean and straight. Something cut your flesh. The bleeding is steady. You need to close it.","display_name":"Incision","emotional_impact":"Another incision. Another day of bleeding. Another day you survived the injury.","failure_consequences":["Excessive blood loss. You lose too much blood.","Infection. The wound becomes contaminated.","The wound doesn't heal. The cut is too deep."],"id":"medi…`
  - row 78: `{"category":"wound_care","complication_warnings":["Watch for excessive bleeding. That's the wound not closing.","Watch for infection. That's the wound becoming contaminated.","Watch for fever. That's your body fighting."],"diagnosis_text":"The wound is deep and ragged. Something tore your flesh. The bleeding is heavy. You need to treat it fast.","display_name":"Deep Laceration","emotional_impact":"Another deep laceration. Another day of torn flesh. Another day you survived the injury.","failure_consequences":["Excessive blood loss. You lose too much blood.","Infection. The wound becomes contaminated.","The wound doesn't heal. The damage is t…`
  - row 79: `{"category":"burn_care","complication_warnings":["Watch for blisters. That's the burn getting worse.","Watch for infection. That's the burn becoming contaminated.","Watch for pain. That's the burn not healing."],"diagnosis_text":"The skin is red and painful. The burn is superficial. The damage is minor.","display_name":"First-Degree Burn","emotional_impact":"Another first-degree burn. Another day of pain. Another day you survived the injury.","failure_consequences":["The burn worsens. The skin blisters.","Infection. The burn becomes contaminated.","The pain continues. The burn doesn't heal."],"id":"medical_burn_first_degree","long_term_effec…`
  - row 80: `{"category":"burn_care","complication_warnings":["Watch for infection. That's the burn becoming contaminated.","Watch for scarring. That's the burn leaving marks.","Watch for pain. That's the burn not healing."],"diagnosis_text":"The skin is blistered and painful. The burn is deep. The damage is significant.","display_name":"Second-Degree Burn","emotional_impact":"Another second-degree burn. Another day of blisters. Another day you survived the injury.","failure_consequences":["Infection. The burn becomes contaminated.","Scarring. The burn leaves permanent marks.","The pain continues. The burn doesn't heal."],"id":"medical_burn_second_degree…`
  - row 81: `{"category":"burn_care","complication_warnings":["Watch for infection. That's the burn becoming contaminated.","Watch for scarring. That's the burn leaving marks.","Watch for shock. That's the burn affecting your body."],"diagnosis_text":"The skin is charred and numb. The burn is deep. The damage is severe.","display_name":"Third-Degree Burn","emotional_impact":"Another third-degree burn. Another day of charred skin. Another day you survived the injury.","failure_consequences":["Infection. The burn becomes contaminated.","Scarring. The burn leaves permanent marks.","The burn doesn't heal. The damage is too severe."],"id":"medical_burn_third_…`
  - row 82: `{"category":"burn_care","complication_warnings":["Watch for spreading damage. That's the chemical moving.","Watch for infection. That's the burn becoming contaminated.","Watch for pain. That's the chemical still burning."],"diagnosis_text":"The chemical has burned your skin. The pain is intense. The damage is spreading.","display_name":"Chemical Burn","emotional_impact":"Another chemical burn. Another day of burning skin. Another day you survived the injury.","failure_consequences":["The chemical continues to burn. The damage spreads.","Infection. The burn becomes contaminated.","The burn doesn't heal. The damage is too severe."],"id":"medic…`
  - row 83: `{"category":"burn_care","complication_warnings":["Watch for heart problems. That's the electricity affecting your heart.","Watch for internal damage. That's the electricity damaging organs.","Watch for muscle problems. That's the electricity affecting muscles."],"diagnosis_text":"The electricity has burned your skin. The pain is intense. The damage is internal and external.","display_name":"Electrical Burn","emotional_impact":"Another electrical burn. Another day of shock. Another day you survived the injury.","failure_consequences":["Internal damage. The electricity has damaged your organs.","Heart problems. The electricity has affected you…`
- Bytes: 222,244; SHA-256: `867ac96ebabc9c40db200106c84a8f8fd2e0f5c21650a737aa3678312d548347`
- Root keys: `collection_id, conditions, schema_version`
- `conditions`: list[83]; union fields: `category, complication_warnings, diagnosis_text, display_name, emotional_impact, failure_consequences, id, long_term_effects, mental_state, pain_descriptions, physical_state, prevention_advice, recovery_descriptions, required_items, success_chances, symptom_descriptions, system_integration, treatment_steps`
  - row 1: `{"category":"injury","complication_warnings":["Watch for redness spreading from the wound. That's infection.","Watch for fever. That's your body fighting something.","Watch for pus. That's the wound telling you it's not clean."],"diagnosis_text":"Laceration. Deep, edges ragged, bleeding steady. Needs closing.","display_name":"Laceration","emotional_impact":"Another scar. Another story. Another day you survived.","failure_consequences":["Infection sets in. The wound becomes hot, red, swollen. Fever follows.","The wound reopens. Bleeding resumes. You're back where you started.","Scarring. The skin heals but the mark remains. A reminder."],"id"…`
  - row 2: `{"category":"injury","complication_warnings":["Watch for signs of infection: redness, swelling, pus, fever.","Watch for shock: pale skin, rapid breathing, confusion.","Watch for dehydration: burns dry out your skin and body."],"diagnosis_text":"Red, blistered skin, peeling at the edges. The pain is constant, sharp, like the skin is still on fire. It needs cooling and covering.","display_name":"Burn","emotional_impact":"Another mark. Another lesson. Another day you learned something the hard way.","failure_consequences":["Infection. Burns are highly susceptible to infection.","Scarring. Burns scar worse than cuts.","Shock. Severe burns can ca…`
  - row 3: `{"category":"injury","complication_warnings":["Watch for numbness below the fracture. That's nerve damage.","Watch for coldness below the fracture. That's blood vessel damage.","Watch for infection if the bone broke the skin."],"diagnosis_text":"The bone is broken. You can feel it — the wrong angle, the grinding, the pain that makes you see white. It needs setting and splinting.","display_name":"Fracture","emotional_impact":"Another break. Another lesson. Another day you learned something the hard way.","failure_consequences":["The bone heals wrong. The limb is permanently deformed.","Infection. Open fractures are highly susceptible to infec…`
  - row 4: `{"category":"illness","complication_warnings":["Watch for vomiting. That's a sign of severe exposure.","Watch for hair loss. That's a sign of acute radiation syndrome.","Watch for bleeding gums. That's a sign of bone marrow damage."],"diagnosis_text":"Acute radiation exposure. Dose received; magnitude not yet known.","display_name":"Radiation Exposure","emotional_impact":"Another exposure. Another calculation. Another day you survived the invisible enemy.","failure_consequences":["Acute radiation sickness. Nausea, vomiting, diarrhea, fever.","Chronic illness. Long-term health effects.","Death. In severe cases, radiation exposure is fatal."],…`
  - row 5: `{"category":"illness","complication_warnings":["Watch for red streaks spreading from the wound. That's blood poisoning.","Watch for high fever. That's your body losing the fight.","Watch for confusion. That's the infection affecting your brain."],"diagnosis_text":"Infection. Fever and spreading inflammation; source not localised.","display_name":"Infection","emotional_impact":"Another infection. Another fight. Another day you survived the invisible enemy.","failure_consequences":["The infection spreads. Other wounds become infected.","Sepsis. The infection enters your bloodstream.","Amputation. In severe cases, the limb is lost."],"id":"medi…`
  - row 6: `{"category":"illness","complication_warnings":["Watch for dark urine. That's your body telling you it needs more water.","Watch for dizziness. That's your blood pressure dropping.","Watch for confusion. That's your brain shutting down."],"diagnosis_text":"Dehydration. Intake below loss; urine dark, membranes dry.","display_name":"Dehydration","emotional_impact":"Another day without enough water. Another day you survived the desert.","failure_consequences":["Organ failure. Your kidneys shut down first.","Heat stroke. Your body can't cool itself.","Death. Dehydration is fatal if untreated."],"id":"medical_dehydration","long_term_effects":["Kid…`
  - row 7: `{"category":"illness","complication_warnings":["Watch for refeeding syndrome. Eating too much too fast can kill you.","Watch for nausea. Your stomach can't handle too much.","Watch for weakness. Your muscles need time to recover."],"diagnosis_text":"Starvation. Caloric deficit sustained; the body is consuming itself.","display_name":"Starvation","emotional_impact":"Another day without enough food. Another day you survived the famine.","failure_consequences":["Organ failure. Your body consumes its own organs.","Cognitive impairment. Your brain shuts down before your body.","Death. Starvation is fatal if untreated."],"id":"medical_starvation",…`
  - row 8: `{"category":"illness","complication_warnings":["Watch for frostbite. White, numb skin is a bad sign.","Watch for confusion. That's your brain shutting down.","Watch for drowsiness. That's your body giving up."],"diagnosis_text":"Hypothermia. Core cooling faster than the body can replace it.","display_name":"Hypothermia","emotional_impact":"Another cold day. Another day you survived the winter.","failure_consequences":["Organ failure. Your body shuts down from the cold.","Frostbite. Your extremities freeze and die.","Death. Hypothermia is fatal if untreated."],"id":"medical_hypothermia","long_term_effects":["Increased sensitivity to cold. You…`
  - row 9: `{"category":"illness","complication_warnings":["Watch for dry skin. That's your body giving up on cooling.","Watch for confusion. That's your brain cooking.","Watch for drowsiness. That's your body shutting down."],"diagnosis_text":"Heatstroke. Sweating has stopped; core temperature still climbing.","display_name":"Heatstroke","emotional_impact":"Another hot day. Another day you survived the desert.","failure_consequences":["Organ failure. Your body cooks from the inside.","Brain damage. Your brain is sensitive to heat.","Death. Heatstroke is fatal if untreated."],"id":"medical_heatstroke","long_term_effects":["Increased sensitivity to heat.…`
  - row 10: `{"category":"mental","complication_warnings":["Watch for hyperventilation. That's breathing too fast.","Watch for dizziness. That's your brain getting too much oxygen.","Watch for chest pain. That's your heart working too hard."],"diagnosis_text":"Panic attack. Acute hyperventilation; no physical cause found.","display_name":"Panic Attack","emotional_impact":"Another attack. Another fight. Another day you survived your own mind.","failure_consequences":["Hyperventilation. You breathe too fast and pass out.","Injury. You fall or run into something during the attack.","Recurring attacks. The fear of attacks causes more attacks."],"id":"medical…`
  - row 11: `{"category":"mental","complication_warnings":["Watch for hallucinations. That's your brain making things up.","Watch for confusion. That's your brain shutting down.","Watch for weakness. That's your body giving out."],"diagnosis_text":"Insomnia. Sleep debt compounding; cognition degrading with it.","display_name":"Insomnia","emotional_impact":"Another sleepless night. Another day you survived your own mind.","failure_consequences":["Cognitive impairment. Your brain stops working properly.","Hallucinations. Your brain makes things up.","Physical collapse. Your body gives out."],"id":"medical_insomnia","long_term_effects":["Cognitive impairmen…`
  - row 12: `{"category":"complication","complication_warnings":["Watch for red streaks. That's blood poisoning.","Watch for high fever. That's your body losing the fight.","Watch for confusion. That's the infection affecting your brain."],"diagnosis_text":"Septic wound. Localised infection, purulent, tracking up the limb.","display_name":"Wound Infection","emotional_impact":"Another infection. Another fight. Another day you survived the invisible enemy.","failure_consequences":["Sepsis. The infection enters your bloodstream.","Amputation. The infection destroys the limb.","Death. Untreated wound infections are fatal."],"id":"medical_wound_infection","lo…`
  - row 13: `{"category":"injury","complication_warnings":["Watch for blisters. That's the tissue dying.","Watch for black skin. That's tissue death.","Watch for infection. Dead tissue attracts bacteria."],"diagnosis_text":"Your fingers are white. Your toes are numb. The skin is hard. Frostbite. The cold has won this round.","display_name":"Frostbite","emotional_impact":"Another frostbite. Another lesson. Another day you survived the cold.","failure_consequences":["Tissue death. The frostbitten area dies.","Amputation. Dead tissue must be removed.","Permanent damage. The frostbite leaves marks."],"id":"medical_frostbite","long_term_effects":["Increased s…`
  - row 14: `{"category":"injury","complication_warnings":["Watch for vomiting. That's a sign of brain swelling.","Watch for worsening symptoms. That's a sign of bleeding.","Watch for loss of consciousness. That's a sign of severe injury."],"diagnosis_text":"Your head is ringing. Your vision is blurry. Your balance is off. You hit your head. Hard.","display_name":"Concussion","emotional_impact":"Another concussion. Another hit. Another day you survived the impact.","failure_consequences":["Brain damage. Concussions can cause permanent damage.","Second impact syndrome. Another concussion can be fatal.","Chronic traumatic encephalopathy. Long-term brain da…`
  - row 15: `{"category":"emergency","complication_warnings":["Watch for seizures. That's your brain shutting down.","Watch for no urine. That's your kidneys failing.","Watch for confusion. That's your brain shutting down."],"diagnosis_text":"Severe dehydration. Volume deficit critical; seizure is the risk.","display_name":"Severe Dehydration","emotional_impact":"Another day without water. Another day you survived the desert.","failure_consequences":["Organ failure. Your kidneys shut down first.","Seizures. Your brain can't function without water.","Death. Severe dehydration is fatal within days."],"id":"medical_dehydration_severe","long_term_effects":["…`
  - row 16: `{"category":"surgery","complication_warnings":["Watch for bleeding. The stump must be sealed.","Watch for infection. The wound must be clean.","Watch for shock. The body must be supported."],"diagnosis_text":"The limb is dead. The tissue is black. The infection is spreading. You need to remove the limb or lose the patient.","display_name":"Amputation","emotional_impact":"Another amputation. Another life saved. Another body changed forever.","failure_consequences":["The patient bleeds out. The amputation was too slow.","Infection sets in. The stump is contaminated.","Shock. The body can't handle the trauma."],"id":"medical_amputation","long_t…`
  - row 17: `{"category":"surgery","complication_warnings":["Watch for infection. The wound must be clean.","Watch for stitch tearing. The wound must be supported.","Watch for scarring. The stitches must be even."],"diagnosis_text":"The wound is deep. The edges are clean. It needs to be closed properly or it won't heal right.","display_name":"Suturing","emotional_impact":"Another suture. Another wound closed. Another life saved.","failure_consequences":["The stitches tear. The wound reopens.","Infection sets in. The wound is contaminated.","Scarring. The wound heals but leaves a mark."],"id":"medical_suturing","long_term_effects":["Scarring. Sutures leav…`
  - row 18: `{"category":"chronic","complication_warnings":["Watch for organ failure. That's the radiation winning.","Watch for cancer. That's the radiation mutating.","Watch for death. That's the radiation succeeding."],"diagnosis_text":"Chronic radiation illness. Accumulated dose; no clearance pathway.","display_name":"Chronic Radiation Illness","emotional_impact":"Another day with chronic radiation. Another day you survived the invisible enemy.","failure_consequences":["Organ failure. The radiation destroys your organs.","Cancer. The radiation mutates your cells.","Death. The body can't fight forever."],"id":"medical_chronic_radiation","long_term_effe…`
  - row 19: `{"category":"chronic","complication_warnings":["Watch for flare-ups. That's the illness returning.","Watch for complications. That's the illness spreading.","Watch for decline. That's the illness winning."],"diagnosis_text":"The illness won't go away. It lingers, flares up, recedes, returns. Your body is fighting a war of attrition.","display_name":"Chronic Illness","emotional_impact":"Another day with chronic illness. Another day you survived the invisible enemy.","failure_consequences":["The illness worsens. The symptoms become debilitating.","Complications arise. The illness affects other systems.","Death. The body can't fight forever."],…`
  - row 20: `{"category":"chronic","complication_warnings":["Watch for cravings. That's the addiction calling.","Watch for triggers. That's the addiction waiting.","Watch for relapse. That's the addiction winning."],"diagnosis_text":"Chemical dependence. Dosing interval shortening; withdrawal onset.","display_name":"Addiction","emotional_impact":"Another day fighting addiction. Another day you survived the craving.","failure_consequences":["Relapse. You start using again.","Overdose. You take too much trying to feel the same.","Death. The substance wins."],"id":"medical_addiction","long_term_effects":["Reduced capacity. The addiction limits what you can …`
  - row 21: `{"category":"mental","complication_warnings":["Watch for flashbacks. That's the trauma returning.","Watch for nightmares. That's the trauma haunting.","Watch for hypervigilance. That's the trauma controlling."],"diagnosis_text":"Post-traumatic stress. Intrusive recall, poor sleep, hypervigilance.","display_name":"Post-Traumatic Stress","emotional_impact":"Another day with PTSD. Another day you survived the memories.","failure_consequences":["The symptoms worsen. The flashbacks become more frequent.","The nightmares intensify. The sleep becomes impossible.","The hypervigilance becomes debilitating. The world becomes too much."],"id":"medical_…`
  - row 22: `{"category":"mental","complication_warnings":["Watch for sadness. That's the depression returning.","Watch for fatigue. That's the depression weighing.","Watch for hopelessness. That's the depression winning."],"diagnosis_text":"The weight won't lift. The darkness won't clear. The hope won't return. You're depressed. The world is gray.","display_name":"Depression","emotional_impact":"Another day with depression. Another day you survived the darkness.","failure_consequences":["The depression worsens. The sadness becomes debilitating.","The fatigue intensifies. The body becomes impossible to move.","The hopelessness becomes total. The future b…`
  - row 23: `{"category":"prevention","complication_warnings":["Watch for infection. That's the wound telling you it's not clean.","Watch for reopening. That's the wound telling you it's not closed.","Watch for scarring. That's the wound telling you it's not healed."],"diagnosis_text":"The wound is fresh. The bleeding is controlled. It needs to be cleaned and dressed to prevent infection.","display_name":"Wound Care","emotional_impact":"Another wound cared for. Another infection prevented. Another life saved.","failure_consequences":["Infection sets in. The wound becomes contaminated.","The wound reopens. The bleeding resumes.","Scarring. The wound heals…`
  - row 24: `{"category":"prevention","complication_warnings":["Watch for dirty hands. That's the bacteria waiting.","Watch for soiled clothes. That's the contamination spreading.","Watch for unclean body. That's the infections developing."],"diagnosis_text":"You're dirty. Your hands are contaminated. Your clothes are soiled. Your body needs cleaning to prevent illness.","display_name":"Hygiene","emotional_impact":"Another day of hygiene. Another infection prevented. Another life saved.","failure_consequences":["Infection. The bacteria enter your body.","Disease. The contamination spreads.","Illness. The body can't fight everything."],"id":"medical_hygie…`
  - row 25: `{"category":"prevention","complication_warnings":["Watch for hunger. That's the body telling you it needs fuel.","Watch for weakness. That's the body telling you it needs protein.","Watch for fog. That's the body telling you it needs glucose."],"diagnosis_text":"You're hungry. Your body needs fuel. Your muscles need protein. Your brain needs glucose. You need to eat.","display_name":"Nutrition","emotional_impact":"Another day of hunger. Another day you survived the famine.","failure_consequences":["Starvation. The body consumes itself.","Weakness. The muscles fail.","Death. The body can't survive without food."],"id":"medical_nutrition","lon…`
  - row 26: `{"category":"prevention","complication_warnings":["Watch for fatigue. That's the body telling you it needs rest.","Watch for drowsiness. That's the body telling you it needs sleep.","Watch for weakness. That's the body telling you it needs recovery."],"diagnosis_text":"You're tired. Your body needs rest. Your muscles need recovery. Your brain needs sleep. You need to stop.","display_name":"Rest","emotional_impact":"Another day without rest. Another day you survived the exhaustion.","failure_consequences":["Collapse. The body gives out.","Injury. The tired body makes mistakes.","Death. The body can't survive without rest."],"id":"medical_rest…`
  - row 27: `{"category":"dental","complication_warnings":["Watch for infection. That's the socket telling you it's not clean.","Watch for swelling. That's the infection spreading.","Watch for fever. That's your body fighting."],"diagnosis_text":"The pain is sharp, constant, in your jaw. The tooth is broken or infected. The nerve is exposed. You need to treat it or lose the tooth.","display_name":"Toothache","emotional_impact":"Another toothache. Another pain. Another day you survived the agony.","failure_consequences":["Infection spreads. The jaw becomes swollen.","The tooth breaks. The root remains.","The pain continues. The infection worsens."],"id":"…`
  - row 28: `{"category":"eye_care","complication_warnings":["Watch for spreading. That's the infection moving.","Watch for vision loss. That's the infection damaging.","Watch for fever. That's your body fighting."],"diagnosis_text":"Your eye is red, swollen, watering. The infection is spreading. The vision is blurry. You need to treat it or lose the eye.","display_name":"Eye Infection","emotional_impact":"Another eye infection. Another threat to your vision. Another day you survived.","failure_consequences":["The infection spreads. The other eye becomes infected.","Vision loss. The infection damages the eye.","Blindness. The infection destroys the eye."…`
  - row 29: `{"category":"reproductive","complication_warnings":["Watch for bleeding. That's a sign of trouble.","Watch for severe pain. That's a sign of complications.","Watch for fever. That's a sign of infection."],"diagnosis_text":"The test is positive. You're pregnant. The world is ending, but life goes on. You need to take care of yourself and the baby.","display_name":"Pregnancy","emotional_impact":"Another day of pregnancy. Another day of hope. Another day you survived.","failure_consequences":["Miscarriage. The body can't support the pregnancy.","Complications. The pregnancy becomes dangerous.","Premature birth. The baby comes too early."],"id":…`
  - row 30: `{"category":"reproductive","complication_warnings":["Watch for bleeding. That's a sign of hemorrhage.","Watch for fever. That's a sign of infection.","Watch for distress. That's a sign of complications."],"diagnosis_text":"The contractions have started. The baby is coming. The world is ending, but life goes on. You need to deliver the baby safely.","display_name":"Childbirth","emotional_impact":"Another birth. Another life. Another day you survived.","failure_consequences":["Complications. The delivery becomes dangerous.","Hemorrhage. The mother bleeds too much.","Infection. The delivery site becomes contaminated."],"id":"medical_childbirth"…`
  - row 31: `{"category":"reproductive","complication_warnings":["Watch for fever. That's a sign of infection.","Watch for crying. That's a sign of distress.","Watch for not eating. That's a sign of illness."],"diagnosis_text":"The baby is here. It's small, fragile, helpless. It needs warmth, food, and protection. You need to keep it alive.","display_name":"Newborn Care","emotional_impact":"Another day of care. Another day of hope. Another day you survived.","failure_consequences":["The baby gets sick. The immune system is weak.","The baby gets cold. The body can't regulate temperature.","The baby dies. The world is too harsh."],"id":"medical_newborn_car…`
  - row 32: `{"category":"injury","complication_warnings":["Watch for infection. That's the broken cartilage telling you it's not clean.","Watch for continued bleeding. That's the nose telling you it's not set.","Watch for breathing difficulty. That's the nose telling you it's blocked."],"diagnosis_text":"The blood is flowing. The nose is bent. The pain is sharp. You broke your nose. It needs to be set or it will heal crooked.","display_name":"Broken Nose","emotional_impact":"Another broken nose. Another scar. Another day you survived the impact.","failure_consequences":["The nose heals crooked. The breathing is permanently affected.","Infection sets in.…`
  - row 33: `{"category":"injury","complication_warnings":["Watch for numbness. That's nerve damage.","Watch for weakness. That's the joint not healing.","Watch for re-dislocation. That's the joint being unstable."],"diagnosis_text":"The arm is hanging wrong. The shoulder is out of the socket. The pain is sharp. You need to pop it back in.","display_name":"Dislocated Shoulder","emotional_impact":"Another dislocation. Another pain. Another day you survived the injury.","failure_consequences":["The shoulder stays out. The arm is useless.","Nerve damage. The arm loses feeling.","The shoulder pops out again. The joint is weakened."],"id":"medical_dislocated_…`
  - row 34: `{"category":"injury","complication_warnings":["Watch for re-injury. That's the ligaments being weak.","Watch for weakness. That's the wrist not healing.","Watch for pain. That's the wrist telling you it's not ready."],"diagnosis_text":"The wrist is swollen. The pain is sharp. The movement is limited. You sprained your wrist. It needs rest and support.","display_name":"Sprained Wrist","emotional_impact":"Another sprain. Another pain. Another day you survived the injury.","failure_consequences":["The wrist stays weak. The grip is permanently affected.","The wrist re-injures. The ligaments are weakened.","The pain continues. The wrist doesn't h…`
  - row 35: `{"category":"injury","complication_warnings":["Watch for broken ribs. That's the bruise becoming a fracture.","Watch for pneumonia. That's the breathing difficulty becoming infection.","Watch for continued pain. That's the ribs not healing."],"diagnosis_text":"The pain is sharp when you breathe. The ribs are bruised. The breathing is difficult. You need to rest and protect your chest.","display_name":"Bruised Ribs","emotional_impact":"Another bruise. Another pain. Another day you survived the injury.","failure_consequences":["The ribs break. The bruise becomes a fracture.","Pneumonia. The breathing difficulty leads to infection.","The pain c…`
  - row 36: `{"category":"injury","complication_warnings":["Watch for infection. That's the wounds telling you they're not clean.","Watch for reopening. That's the wounds telling you they're not closed.","Watch for scarring. That's the wounds telling you they're not healed."],"diagnosis_text":"The skin is broken. The blood is flowing. The pain is sharp. You have cuts and scrapes. They need cleaning and covering.","display_name":"Cuts and Scrapes","emotional_impact":"Another cut. Another scrape. Another day you survived the injury.","failure_consequences":["Infection sets in. The wounds become contaminated.","Scarring. The wounds heal but leave marks.","T…`
  - row 37: `{"category":"injury","complication_warnings":["Watch for infection. That's the blister telling you it's not clean.","Watch for reopening. That's the blister telling you it's not healed.","Watch for pain. That's the blister telling you it's not ready."],"diagnosis_text":"The blister is full of fluid. The skin is tight. The pain is sharp. You need to drain it or protect it.","display_name":"Blister Care","emotional_impact":"Another blister. Another pain. Another day you survived the friction.","failure_consequences":["Infection sets in. The blister becomes contaminated.","The blister reopens. The raw skin is exposed.","The pain continues. The …`
  - row 38: `{"category":"poisoning","complication_warnings":["Watch for dehydration. That's the fluid loss winning.","Watch for blood in vomit. That's a sign of serious damage.","Watch for fever. That's a sign of infection."],"diagnosis_text":"The food was bad. Your stomach is turning. Your body is rejecting what you ate. You need to let it pass.","display_name":"Food Poisoning","emotional_impact":"Another bout of food poisoning. Another day you survived the bad food.","failure_consequences":["Dehydration. You lose too much fluid.","The poisoning worsens. The bad food stays in your system.","Shock. Your body can't handle the stress."],"id":"medical_food…`
  - row 39: `{"category":"poisoning","complication_warnings":["Watch for organ failure. That's the chemical winning.","Watch for nerve damage. That's the chemical affecting your nervous system.","Watch for breathing difficulty. That's the chemical affecting your lungs."],"diagnosis_text":"The chemical is in your system. Your body is reacting. Your vision is blurry. Your stomach is burning. You need help.","display_name":"Chemical Poisoning","emotional_impact":"Another chemical exposure. Another day you survived the poison.","failure_consequences":["Organ failure. The chemical destroys your organs.","Nerve damage. The chemical affects your nervous system.…`
  - row 40: `{"category":"allergic","complication_warnings":["Watch for throat swelling. That's the reaction closing your airway.","Watch for shock. That's your body giving up.","Watch for breathing difficulty. That's the reaction affecting your lungs."],"diagnosis_text":"The reaction is starting. Your skin is itching. Your throat is swelling. Your body is rejecting something. You need to act fast.","display_name":"Allergic Reaction","emotional_impact":"Another allergic reaction. Another day you survived the allergen.","failure_consequences":["Anaphylaxis. Your throat closes completely.","Shock. Your body can't handle the reaction.","Death. Severe allerg…`
  - row 41: `{"category":"bite","complication_warnings":["Watch for infection. That's the bite telling you it's not clean.","Watch for allergic reaction. That's your body reacting to the venom.","Watch for disease. That's the insect's gift."],"diagnosis_text":"The bite is red, swollen, itchy. The insect got you. It's not serious, but it's annoying. You need to treat it.","display_name":"Insect Bite","emotional_impact":"Another insect bite. Another annoyance. Another day you survived the bugs.","failure_consequences":["Infection. The bite becomes contaminated.","Allergic reaction. Your body reacts to the insect venom.","Disease. The insect may have been c…`
  - row 42: `{"category":"bite","complication_warnings":["Watch for infection. That's the wound telling you it's not clean.","Watch for disease. That's the animal's gift.","Watch for fever. That's your body fighting."],"diagnosis_text":"The animal bit you. The wound is deep. The bleeding is steady. The animal may have been carrying disease. You need to treat it fast.","display_name":"Animal Bite","emotional_impact":"Another animal bite. Another wound. Another day you survived the wild.","failure_consequences":["Infection. The wound becomes contaminated.","Disease. The animal may have been carrying rabies or other diseases.","The wound doesn't heal. The d…`
  - row 43: `{"category":"bite","complication_warnings":["Watch for breathing difficulty. That's the venom affecting your lungs.","Watch for vision changes. That's the venom affecting your nervous system.","Watch for swelling spreading. That's the venom moving."],"diagnosis_text":"The snake bit you. The venom is spreading. Your arm is swelling. Your vision is blurry. You need antivenom fast.","display_name":"Snake Bite","emotional_impact":"Another snake bite. Another venom. Another day you survived the wild.","failure_consequences":["The venom spreads. Your organs shut down.","Nerve damage. The venom affects your nervous system permanently.","Death. Snak…`
  - row 44: `{"category":"bite","complication_warnings":["Watch for infection. That's the bite telling you it's not clean.","Watch for allergic reaction. That's your body reacting to the venom.","Watch for necrosis. That's the venom destroying tissue."],"diagnosis_text":"The spider bit you. The bite is red, swollen, painful. The spider may have been venomous. You need to treat it.","display_name":"Spider Bite","emotional_impact":"Another spider bite. Another annoyance. Another day you survived the bugs.","failure_consequences":["Infection. The bite becomes contaminated.","Allergic reaction. Your body reacts to the spider venom.","Necrosis. The venom dest…`
  - row 45: `{"category":"bite","complication_warnings":["Watch for infection. That's the wound telling you it's not clean.","Watch for rabies. That's the dog's gift.","Watch for fever. That's your body fighting."],"diagnosis_text":"The dog bit you. The wound is deep. The bleeding is steady. The dog may have been rabid. You need to treat it fast.","display_name":"Dog Bite","emotional_impact":"Another dog bite. Another wound. Another day you survived the wild.","failure_consequences":["Infection. The wound becomes contaminated.","Rabies. The dog may have been rabid.","The wound doesn't heal. The damage is too severe."],"id":"medical_dog_bite","long_term_e…`
  - row 46: `{"category":"bite","complication_warnings":["Watch for infection. That's the scratches telling you they're not clean.","Watch for cat scratch fever. That's the cat's gift.","Watch for swelling. That's the infection spreading."],"diagnosis_text":"The cat scratched you. The wound is shallow but dirty. The bleeding is minimal. The cat may have been carrying disease.","display_name":"Cat Scratch","emotional_impact":"Another cat scratch. Another annoyance. Another day you survived the pets.","failure_consequences":["Infection. The scratches become contaminated.","Cat scratch fever. The cat may have been carrying the bacteria.","The scratches don'…`
  - row 47: `{"category":"bite","complication_warnings":["Watch for broken bones. That's the kick shattering something.","Watch for internal bleeding. That's the kick damaging your organs.","Watch for breathing difficulty. That's the kick affecting your lungs."],"diagnosis_text":"The horse kicked you. The impact was massive. Your body is broken. You need medical attention fast.","display_name":"Horse Kick","emotional_impact":"Another horse kick. Another broken body. Another day you survived the animals.","failure_consequences":["Broken bones. The kick shattered something.","Internal bleeding. The kick damaged your organs.","Death. Horse kicks can be fata…`
  - row 48: `{"category":"surgery","complication_warnings":["Watch for nerve damage. That's the setting affecting the nerves.","Watch for re-breakage. That's the setting not holding.","Watch for infection. That's the wound not healing."],"diagnosis_text":"The bone is broken. It needs to be set back into place. The pain will be intense. The recovery will be long.","display_name":"Bone Setting","emotional_impact":"Another bone setting. Another broken body. Another day you survived the injury.","failure_consequences":["The bone heals crooked. The limb is permanently deformed.","Nerve damage. The setting damaged the nerves.","The bone re-breaks. The setting …`
  - row 49: `{"category":"surgery","complication_warnings":["Watch for infection spreading. That's the debridement not being thorough.","Watch for shock. That's the pain being too much.","Watch for bleeding. That's the debridement being too aggressive."],"diagnosis_text":"The wound is infected. The dead tissue needs to be removed. The wound needs to be cleaned. The pain will be intense.","display_name":"Wound Debridement","emotional_impact":"Another debridement. Another infected wound. Another day you survived the infection.","failure_consequences":["The infection spreads. The wound becomes worse.","The wound doesn't heal. The damage is too severe.","The…`
  - row 50: `{"category":"rehabilitation","complication_warnings":["Watch for re-injury. That's the therapy being too aggressive.","Watch for pain. That's the therapy pushing too hard.","Watch for frustration. That's the therapy taking too long."],"diagnosis_text":"The injury has healed. The limb is weak. The movement is limited. You need to rebuild strength and flexibility.","display_name":"Physical Therapy","emotional_impact":"Another therapy session. Another day of recovery. Another day you survived the injury.","failure_consequences":["The limb stays weak. The movement stays limited.","The injury reoccurs. The therapy was not enough.","The pain conti…`
  - row 51: `{"category":"mental_health","complication_warnings":["Watch for worsening symptoms. That's the counseling not working.","Watch for suicidal thoughts. That's the depression winning.","Watch for substance abuse. That's the coping mechanism failing."],"diagnosis_text":"The trauma is deep. The memories won't stop. The nightmares won't end. You need to talk to someone.","display_name":"Counseling","emotional_impact":"Another counseling session. Another day of recovery. Another day you survived the trauma.","failure_consequences":["The trauma worsens. The symptoms become debilitating.","The depression deepens. The hope disappears.","The anxiety in…`
  - row 52: `{"category":"mental_health","complication_warnings":["Watch for side effects. That's the medication harming you.","Watch for dependency. That's the medication becoming addictive.","Watch for withdrawal. That's the medication being stopped too quickly."],"diagnosis_text":"The symptoms are severe. The counseling is not enough. You need medication to manage the symptoms.","display_name":"Medication Management","emotional_impact":"Another day of medication. Another day of recovery. Another day you survived the symptoms.","failure_consequences":["The symptoms worsen. The medication is not enough.","The side effects are severe. The medication is h…`
  - row 53: `{"category":"mental_health","complication_warnings":["Watch for worsening grief. That's the counseling not working.","Watch for suicidal thoughts. That's the grief winning.","Watch for substance abuse. That's the coping mechanism failing."],"diagnosis_text":"The loss is deep. The grief is overwhelming. The sadness is constant. You need to process the loss.","display_name":"Grief Counseling","emotional_impact":"Another grief counseling session. Another day of recovery. Another day you survived the loss.","failure_consequences":["The grief worsens. The sadness becomes debilitating.","The depression deepens. The hope disappears.","The anger inc…`
  - row 54: `{"category":"mental_health","complication_warnings":["Watch for worsening symptoms. That's the therapy not working.","Watch for suicidal thoughts. That's the trauma winning.","Watch for substance abuse. That's the coping mechanism failing."],"diagnosis_text":"The trauma is deep. The memories are vivid. The nightmares are constant. You need specialized therapy.","display_name":"Trauma Therapy","emotional_impact":"Another trauma therapy session. Another day of recovery. Another day you survived the trauma.","failure_consequences":["The trauma worsens. The symptoms become debilitating.","The flashbacks increase. The triggers multiply.","The hyp…`
  - row 55: `{"category":"mental_health","complication_warnings":["Watch for worsening stress. That's the management not working.","Watch for health problems. That's the stress affecting the body.","Watch for burnout. That's the stress winning."],"diagnosis_text":"The stress is overwhelming. The pressure is constant. The anxiety is high. You need to learn to manage the stress.","display_name":"Stress Management","emotional_impact":"Another stress management session. Another day of recovery. Another day you survived the pressure.","failure_consequences":["The stress worsens. The symptoms become debilitating.","The anxiety increases. The panic becomes cons…`
  - row 56: `{"category":"chronic_disease","complication_warnings":["Watch for low blood sugar. That's the insulin working too well.","Watch for high blood sugar. That's the diabetes winning.","Watch for organ damage. That's the sugar destroying your body."],"diagnosis_text":"Your blood sugar is unstable. Your body can't regulate it. You need insulin or you'll die.","display_name":"Diabetes","emotional_impact":"Another day with diabetes. Another day of management. Another day you survived the disease.","failure_consequences":["Diabetic coma. Your blood sugar crashes.","Organ failure. The sugar damages your organs.","Death. Diabetes is fatal without treat…`
  - row 57: `{"category":"chronic_disease","complication_warnings":["Watch for worsening symptoms. That's the asthma getting worse.","Watch for triggers. That's the asthma being provoked.","Watch for suffocation. That's the airways closing."],"diagnosis_text":"Airway constriction. Wheeze on exhale. Inhaler needed to sustain.","display_name":"Asthma","emotional_impact":"Another asthma attack. Another day of breathing difficulty. Another day you survived the disease.","failure_consequences":["Suffocation. Your airways close completely.","Respiratory failure. Your lungs give out.","Death. Asthma can be fatal without treatment."],"id":"medical_asthma","long_…`
  - row 58: `{"category":"chronic_disease","complication_warnings":["Watch for worsening pain. That's the arthritis getting worse.","Watch for joint damage. That's the inflammation destroying the joints.","Watch for reduced mobility. That's the joints stopping working."],"diagnosis_text":"Your joints are inflamed. Your movement is painful. Your body is attacking itself.","display_name":"Arthritis","emotional_impact":"Another day with arthritis. Another day of pain. Another day you survived the disease.","failure_consequences":["Joint damage. The inflammation destroys the joints.","Reduced mobility. The joints stop working.","Chronic pain. The arthritis i…`
  - row 59: `{"category":"chronic_disease","complication_warnings":["Watch for chest pain. That's the heart struggling.","Watch for shortness of breath. That's the lungs not getting enough oxygen.","Watch for fatigue. That's the body not getting enough blood."],"diagnosis_text":"Your heart is weak. Your circulation is poor. Your body is struggling to pump blood.","display_name":"Heart Disease","emotional_impact":"Another day with heart disease. Another day of fatigue. Another day you survived the disease.","failure_consequences":["Heart attack. Your heart gives out.","Stroke. Your brain doesn't get enough blood.","Death. Heart disease is fatal without tr…`
  - row 60: `{"category":"chronic_disease","complication_warnings":["Watch for spreading. That's the cancer moving.","Watch for pain. That's the tumor pressing on nerves.","Watch for fatigue. That's the body fighting the cancer."],"diagnosis_text":"The cells are growing out of control. The tumor is spreading. Your body is fighting itself.","display_name":"Cancer","emotional_impact":"Another day with cancer. Another day of fighting. Another day you survived the disease.","failure_consequences":["The cancer spreads. The tumor grows.","Organ failure. The cancer destroys organs.","Death. Cancer is fatal without treatment."],"id":"medical_cancer","long_term_e…`
  - row 61: `{"category":"chronic_disease","complication_warnings":["Watch for seizures. That's the brain misfiring.","Watch for triggers. That's the epilepsy being provoked.","Watch for injury. That's the seizure hurting the body."],"diagnosis_text":"Your brain is misfiring. The seizures are coming. Your body is out of control.","display_name":"Epilepsy","emotional_impact":"Another seizure. Another day of fear. Another day you survived the disease.","failure_consequences":["Injury during a seizure. The body convulses uncontrollably.","Status epilepticus. The seizure doesn't stop.","Death. Epilepsy can be fatal without treatment."],"id":"medical_epilepsy…`
  - row 62: `{"category":"chronic_disease","complication_warnings":["Watch for worsening pain. That's the chronic pain getting worse.","Watch for depression. That's the pain affecting the mind.","Watch for disability. That's the pain preventing normal function."],"diagnosis_text":"The pain won't stop. It's constant, burning, aching. Your body is in pain all the time.","display_name":"Chronic Pain","emotional_impact":"Another day of chronic pain. Another day of suffering. Another day you survived the pain.","failure_consequences":["The pain worsens. The body can't handle it.","Depression. The pain affects the mind.","Disability. The pain prevents normal f…`
  - row 63: `{"category":"chronic_disease","complication_warnings":["Watch for worsening fatigue. That's the chronic fatigue getting worse.","Watch for depression. That's the fatigue affecting the mind.","Watch for disability. That's the fatigue preventing normal function."],"diagnosis_text":"The fatigue won't stop. It's constant, overwhelming, debilitating. Your body is exhausted all the time.","display_name":"Chronic Fatigue","emotional_impact":"Another day of chronic fatigue. Another day of exhaustion. Another day you survived the fatigue.","failure_consequences":["The fatigue worsens. The body can't handle it.","Depression. The fatigue affects the mi…`
  - row 64: `{"category":"chronic_disease","complication_warnings":["Watch for worsening headache. That's the chronic headache getting worse.","Watch for nausea. That's the headache causing nausea.","Watch for disability. That's the headache preventing normal function."],"diagnosis_text":"The headache won't stop. It's constant, throbbing, pounding. Your head is in pain all the time.","display_name":"Chronic Headache","emotional_impact":"Another day of chronic headache. Another day of pain. Another day you survived the headache.","failure_consequences":["The headache worsens. The pain becomes unbearable.","Nausea. The headache causes nausea.","Disability.…`
  - row 65: `{"category":"chronic_disease","complication_warnings":["Watch for worsening back pain. That's the chronic back pain getting worse.","Watch for reduced mobility. That's the back not moving.","Watch for disability. That's the back pain preventing normal function."],"diagnosis_text":"The back pain won't stop. It's constant, aching, stabbing. Your back is in pain all the time.","display_name":"Chronic Back Pain","emotional_impact":"Another day of chronic back pain. Another day of pain. Another day you survived the back pain.","failure_consequences":["The back pain worsens. The pain becomes unbearable.","Reduced mobility. The back can't move.","D…`
  - row 66: `{"category":"parasitic","complication_warnings":["Watch for cerebral malaria. That's the parasite attacking your brain.","Watch for organ failure. That's the parasite destroying your organs.","Watch for dehydration. That's the fever drying you out."],"diagnosis_text":"The mosquito bite has infected you. The fever is coming. The chills are starting. You need treatment fast.","display_name":"Malaria","emotional_impact":"Another malaria attack. Another fever. Another day you survived the parasite.","failure_consequences":["Cerebral malaria. The parasite attacks your brain.","Organ failure. The parasite destroys your organs.","Death. Malaria is …`
  - row 67: `{"category":"parasitic","complication_warnings":["Watch for intestinal blockage. That's the tapeworm growing too large.","Watch for nutritional deficiency. That's the tapeworm eating your food.","Watch for organ damage. That's the tapeworm migrating."],"diagnosis_text":"Something is living inside you. The tapeworm is growing. Your stomach is upset. Your weight is dropping.","display_name":"Tapeworm","emotional_impact":"Another day with a tapeworm. Another day of discomfort. Another day you survived the parasite.","failure_consequences":["The tapeworm grows. It blocks your intestines.","Nutritional deficiency. The tapeworm eats your nutrients…`
  - row 68: `{"category":"nutritional","complication_warnings":["Watch for tooth loss. That's the scurvy destroying your gums.","Watch for anemia. That's the scurvy affecting your blood.","Watch for wound healing. That's the scurvy affecting your collagen."],"diagnosis_text":"Your gums are bleeding. Your teeth are loose. Your body is falling apart. You need vitamin C.","display_name":"Scurvy","emotional_impact":"Another day with scurvy. Another day of bleeding gums. Another day you survived the deficiency.","failure_consequences":["Tooth loss. The scurvy destroys your gums.","Anemia. The scurvy affects your blood.","Death. Scurvy is fatal without treatme…`
  - row 69: `{"category":"nutritional","complication_warnings":["Watch for heart failure. That's the beriberi destroying your heart.","Watch for nerve damage. That's the beriberi destroying your nerves.","Watch for weakness. That's the beriberi affecting your muscles."],"diagnosis_text":"Your heart is weak. Your nerves are damaged. Your body is deficient in vitamin B1.","display_name":"Beriberi","emotional_impact":"Another day with beriberi. Another day of weakness. Another day you survived the deficiency.","failure_consequences":["Heart failure. The beriberi destroys your heart.","Nerve damage. The beriberi destroys your nerves.","Death. Beriberi is fat…`
  - row 70: `{"category":"nutritional","complication_warnings":["Watch for skin damage. That's the pellagra destroying your skin.","Watch for diarrhea. That's the pellagra destroying your digestive system.","Watch for dementia. That's the pellagra affecting your mind."],"diagnosis_text":"Your skin is rough. Your mind is confused. Your body is deficient in vitamin B3.","display_name":"Pellagra","emotional_impact":"Another day with pellagra. Another day of skin damage. Another day you survived the deficiency.","failure_consequences":["Skin damage. The pellagra destroys your skin.","Digestive failure. The pellagra destroys your digestive system.","Death. Pe…`
  - row 71: `{"category":"nutritional","complication_warnings":["Watch for bone deformity. That's the rickets deforming your bones.","Watch for growth failure. That's the rickets stunting your growth.","Watch for fractures. That's the rickets making your bones fragile."],"diagnosis_text":"Your bones are weak. Your body is deficient in vitamin D. Your children are at risk.","display_name":"Rickets","emotional_impact":"Another day with rickets. Another day of bone pain. Another day you survived the deficiency.","failure_consequences":["Bone deformity. The rickets deforms your bones.","Growth failure. The rickets stunts your growth.","Fractures. The rickets…`
  - row 72: `{"category":"nutritional","complication_warnings":["Watch for heart failure. That's the anemia straining your heart.","Watch for organ damage. That's the anemia starving your organs.","Watch for fatigue. That's the anemia affecting your energy."],"diagnosis_text":"Your blood is weak. Your body is deficient in iron. You're tired all the time.","display_name":"Anemia","emotional_impact":"Another day with anemia. Another day of fatigue. Another day you survived the deficiency.","failure_consequences":["Heart failure. The anemia strains your heart.","Organ damage. The anemia starves your organs.","Death. Anemia can be fatal without treatment."],…`
  - row 73: `{"category":"nutritional","complication_warnings":["Watch for breathing difficulty. That's the goiter pressing on your airway.","Watch for swallowing difficulty. That's the goiter pressing on your esophagus.","Watch for thyroid failure. That's the goiter destroying your thyroid."],"diagnosis_text":"Your thyroid is swollen. Your neck is enlarged. Your body is deficient in iodine.","display_name":"Goiter","emotional_impact":"Another day with goiter. Another day of swelling. Another day you survived the deficiency.","failure_consequences":["Breathing difficulty. The goiter presses on your airway.","Swallowing difficulty. The goiter presses on y…`
  - row 74: `{"category":"wound_care","complication_warnings":["Watch for excessive bleeding. That's the wound not closing.","Watch for infection. That's the wound becoming contaminated.","Watch for fever. That's your body fighting."],"diagnosis_text":"The wound is deep. The flesh is torn. The bleeding is heavy. You need to close it or you'll bleed out.","display_name":"Deep Wound","emotional_impact":"Another deep wound. Another day of bleeding. Another day you survived the injury.","failure_consequences":["Excessive blood loss. You lose too much blood.","Infection. The wound becomes contaminated.","The wound doesn't heal. The damage is too severe."],"id…`
  - row 75: `{"category":"wound_care","complication_warnings":["Watch for internal damage. That's the puncture hitting organs.","Watch for infection. That's the wound becoming contaminated.","Watch for fever. That's your body fighting."],"diagnosis_text":"The wound is deep and narrow. Something punctured your flesh. The bleeding is minimal but the damage is deep.","display_name":"Puncture Wound","emotional_impact":"Another puncture wound. Another day of deep pain. Another day you survived the injury.","failure_consequences":["Internal damage. The puncture damaged organs.","Infection. The wound becomes contaminated.","The wound doesn't heal. The damage is…`
  - row 76: `{"category":"wound_care","complication_warnings":["Watch for excessive bleeding. That's the wound not closing.","Watch for infection. That's the wound becoming contaminated.","Watch for fever. That's your body fighting."],"diagnosis_text":"The flesh is torn away. The wound is ragged. The bleeding is heavy. You need to treat it fast.","display_name":"Avulsion","emotional_impact":"Another avulsion. Another day of torn flesh. Another day you survived the injury.","failure_consequences":["Excessive blood loss. You lose too much blood.","Infection. The wound becomes contaminated.","The wound doesn't heal. The damage is too severe."],"id":"medical…`
  - row 77: `{"category":"wound_care","complication_warnings":["Watch for excessive bleeding. That's the wound not closing.","Watch for infection. That's the wound becoming contaminated.","Watch for fever. That's your body fighting."],"diagnosis_text":"The wound is clean and straight. Something cut your flesh. The bleeding is steady. You need to close it.","display_name":"Incision","emotional_impact":"Another incision. Another day of bleeding. Another day you survived the injury.","failure_consequences":["Excessive blood loss. You lose too much blood.","Infection. The wound becomes contaminated.","The wound doesn't heal. The cut is too deep."],"id":"medi…`
  - row 78: `{"category":"wound_care","complication_warnings":["Watch for excessive bleeding. That's the wound not closing.","Watch for infection. That's the wound becoming contaminated.","Watch for fever. That's your body fighting."],"diagnosis_text":"The wound is deep and ragged. Something tore your flesh. The bleeding is heavy. You need to treat it fast.","display_name":"Deep Laceration","emotional_impact":"Another deep laceration. Another day of torn flesh. Another day you survived the injury.","failure_consequences":["Excessive blood loss. You lose too much blood.","Infection. The wound becomes contaminated.","The wound doesn't heal. The damage is t…`
  - row 79: `{"category":"burn_care","complication_warnings":["Watch for blisters. That's the burn getting worse.","Watch for infection. That's the burn becoming contaminated.","Watch for pain. That's the burn not healing."],"diagnosis_text":"The skin is red and painful. The burn is superficial. The damage is minor.","display_name":"First-Degree Burn","emotional_impact":"Another first-degree burn. Another day of pain. Another day you survived the injury.","failure_consequences":["The burn worsens. The skin blisters.","Infection. The burn becomes contaminated.","The pain continues. The burn doesn't heal."],"id":"medical_burn_first_degree","long_term_effec…`
  - row 80: `{"category":"burn_care","complication_warnings":["Watch for infection. That's the burn becoming contaminated.","Watch for scarring. That's the burn leaving marks.","Watch for pain. That's the burn not healing."],"diagnosis_text":"The skin is blistered and painful. The burn is deep. The damage is significant.","display_name":"Second-Degree Burn","emotional_impact":"Another second-degree burn. Another day of blisters. Another day you survived the injury.","failure_consequences":["Infection. The burn becomes contaminated.","Scarring. The burn leaves permanent marks.","The pain continues. The burn doesn't heal."],"id":"medical_burn_second_degree…`
  - row 81: `{"category":"burn_care","complication_warnings":["Watch for infection. That's the burn becoming contaminated.","Watch for scarring. That's the burn leaving marks.","Watch for shock. That's the burn affecting your body."],"diagnosis_text":"The skin is charred and numb. The burn is deep. The damage is severe.","display_name":"Third-Degree Burn","emotional_impact":"Another third-degree burn. Another day of charred skin. Another day you survived the injury.","failure_consequences":["Infection. The burn becomes contaminated.","Scarring. The burn leaves permanent marks.","The burn doesn't heal. The damage is too severe."],"id":"medical_burn_third_…`
  - row 82: `{"category":"burn_care","complication_warnings":["Watch for spreading damage. That's the chemical moving.","Watch for infection. That's the burn becoming contaminated.","Watch for pain. That's the chemical still burning."],"diagnosis_text":"The chemical has burned your skin. The pain is intense. The damage is spreading.","display_name":"Chemical Burn","emotional_impact":"Another chemical burn. Another day of burning skin. Another day you survived the injury.","failure_consequences":["The chemical continues to burn. The damage spreads.","Infection. The burn becomes contaminated.","The burn doesn't heal. The damage is too severe."],"id":"medic…`
  - row 83: `{"category":"burn_care","complication_warnings":["Watch for heart problems. That's the electricity affecting your heart.","Watch for internal damage. That's the electricity damaging organs.","Watch for muscle problems. That's the electricity affecting muscles."],"diagnosis_text":"The electricity has burned your skin. The pain is intense. The damage is internal and external.","display_name":"Electrical Burn","emotional_impact":"Another electrical burn. Another day of shock. Another day you survived the injury.","failure_consequences":["Internal damage. The electricity has damaged your organs.","Heart problems. The electricity has affected you…`

## `Assets/StreamingAssets/Data/psychological_therapies.json`
- Bytes: 8,022; SHA-256: `64c91c12f95af2fc3023d0d4f52c0a871a8ea88475096f024b59fd766624b3bf`
- Root keys: `conditions, schema_version, therapies`
- `conditions`: list[6]; union fields: `canonical_surface, condition_id, description, display_name, reversible`
  - row 1: `{"canonical_surface":"hypervigilance","condition_id":"condition_combat_ptsd","description":"The body keeps answering a door that closed months ago. Sleep comes in fits; loud nights undo weeks.","display_name":"Combat-Rooted Startle Sickness","reversible":false}`
  - row 2: `{"canonical_surface":"flashback","condition_id":"condition_flash_blindness_shock","description":"The burst printed itself on the eyes. Vision returns in patches; the dark patches last longer each time.","display_name":"Flash-Blindness After shock","reversible":true}`
  - row 3: `{"canonical_surface":"guilt_insomnia","condition_id":"condition_severe_survivor_guilt","description":"Every ration is an argument with someone who is not here to eat it.","display_name":"Severe Survivor Guilt","reversible":false}`
  - row 4: `{"canonical_surface":"none","condition_id":"condition_paranoid_psychosis","description":"The walls have ears and the ears have allegiances. Grounding work, done slowly, sometimes quiets them.","display_name":"Siege Paranoia","reversible":false}`
  - row 5: `{"canonical_surface":"hypervigilance","condition_id":"condition_chronic_hypervigilance","description":"Watches that never end, kept by people who are no longer on the roster.","display_name":"Chronic Hypervigilance","reversible":true}`
  - row 6: `{"canonical_surface":"guilt_insomnia","condition_id":"condition_guilt_insomnia_loop","description":"The night shift of the conscience, working overtime without pay.","display_name":"Guilt-Insomnia Loop","reversible":true}`
- `therapies`: list[8]; union fields: `acute_stress_reduction_permille, description, display_name, duration_days, eligible_conditions, grants_journal_entry, recovery_progress, relapse_modifier, resource_costs, side_effects, staff_skill_id, staff_skill_threshold, tags, therapy_id, work_restriction`
  - row 1: `{"acute_stress_reduction_permille":400,"description":"A lined cistern, warm water, measured salts, and one hour where nothing demands anything. The quiet does the work.","display_name":"Dark-Tank Immersion","duration_days":1,"eligible_conditions":["condition_combat_ptsd","condition_chronic_hypervigilance"],"recovery_progress":20,"relapse_modifier":0.05,"resource_costs":[{"amount":2,"item_id":"clean_water"},{"amount":1,"item_id":"item_preservation_salt"}],"side_effects":["disorientation_hours"],"staff_skill_id":"skill_watchful","staff_skill_threshold":30,"tags":["immersion","intensive"],"therapy_id":"therapy_sensory_deprivation_immersion","wo…`
  - row 2: `{"acute_stress_reduction_permille":250,"description":"An hour a day of saying the unsayable to someone trained not to flinch.","display_name":"Guided Catharsis Sessions","duration_days":3,"eligible_conditions":["condition_severe_survivor_guilt","condition_guilt_insomnia_loop"],"recovery_progress":25,"relapse_modifier":0.1,"resource_costs":[{"amount":1,"item_id":"clean_water"}],"side_effects":[],"staff_skill_id":"skill_cold_analysis","staff_skill_threshold":40,"tags":["talk","structured"],"therapy_id":"therapy_cognitive_catharsis","work_restriction":"light_duty"}`
  - row 3: `{"acute_stress_reduction_permille":300,"description":"Controlled exposure, one trigger at a time, at the patient's pace. Setbacks are part of the arithmetic.","display_name":"Graded Desensitization","duration_days":4,"eligible_conditions":["condition_combat_ptsd","condition_flash_blindness_shock"],"recovery_progress":30,"relapse_modifier":0.15,"resource_costs":[{"amount":1,"item_id":"bandage"}],"side_effects":["temporary_sleep_disruption"],"staff_skill_id":"skill_cold_analysis","staff_skill_threshold":50,"tags":["exposure","graduated"],"therapy_id":"therapy_trauma_desensitization","work_restriction":"light_duty"}`
  - row 4: `{"acute_stress_reduction_permille":150,"description":"Mornings, a warm lamp, and a blank book. What the patient remembers gets written down and kept — for the archive, and out of the skull.","display_name":"Dream Transcription","duration_days":2,"eligible_conditions":["condition_guilt_insomnia_loop","condition_paranoid_psychosis"],"grants_journal_entry":true,"recovery_progress":15,"relapse_modifier":0.0,"resource_costs":[{"amount":1,"item_id":"paper_stock"}],"side_effects":[],"staff_skill_id":"skill_watchful","staff_skill_threshold":20,"tags":["journal","archive_link"],"therapy_id":"therapy_dream_transcription","work_restriction":"none"}`
  - row 5: `{"acute_stress_reduction_permille":200,"description":"Short, measured draughts to break the worst nights. Not a cure — a pause long enough to start one.","display_name":"Sedative Stabilization","duration_days":1,"eligible_conditions":["condition_paranoid_psychosis","condition_combat_ptsd","condition_guilt_insomnia_loop"],"recovery_progress":5,"relapse_modifier":0.2,"resource_costs":[{"amount":1,"item_id":"sedative_draught"}],"side_effects":["sedation_restriction_days"],"staff_skill_id":"skill_cold_analysis","staff_skill_threshold":25,"tags":["chemical","stabilizing"],"therapy_id":"therapy_sedative_stabilization","work_restriction":"bedrest"}`
  - row 6: `{"acute_stress_reduction_permille":220,"description":"The therapist stands part of the patient's watch beside them, then talks it through while it is still warm.","display_name":"Watch-Rotation Counseling","duration_days":3,"eligible_conditions":["condition_chronic_hypervigilance","condition_combat_ptsd"],"recovery_progress":20,"relapse_modifier":0.1,"resource_costs":[],"side_effects":[],"staff_skill_id":"skill_watchful","staff_skill_threshold":45,"tags":["in_field","talk"],"therapy_id":"therapy_watch_rotation_counseling","work_restriction":"none"}`
  - row 7: `{"acute_stress_reduction_permille":180,"description":"The patient tells the shelter who they lost, and the shelter writes it into the memorial book. Grief with witnesses weighs less.","display_name":"Memorial Testimony Circle","duration_days":2,"eligible_conditions":["condition_severe_survivor_guilt","condition_guilt_insomnia_loop"],"grants_journal_entry":true,"recovery_progress":20,"relapse_modifier":0.05,"resource_costs":[{"amount":1,"item_id":"paper_stock"}],"side_effects":[],"staff_skill_id":"skill_watchful","staff_skill_threshold":35,"tags":["memorial","communal"],"therapy_id":"therapy_memorial_testimony_circle","work_restriction":"none…`
  - row 8: `{"acute_stress_reduction_permille":120,"description":"Half-shifts of real, useful work with a supervisor who stops before the shaking starts. The road back to the roster, graded.","display_name":"Work-Rhythm Restoration","duration_days":5,"eligible_conditions":["condition_flash_blindness_shock","condition_chronic_hypervigilance","condition_paranoid_psychosis"],"recovery_progress":35,"relapse_modifier":0.1,"resource_costs":[{"amount":1,"item_id":"clean_water"}],"side_effects":[],"staff_skill_id":"skill_cold_analysis","staff_skill_threshold":30,"tags":["rehabilitation","graded"],"therapy_id":"therapy_work_rhythm_restoration","work_restriction"…`
- Bytes: 8,022; SHA-256: `64c91c12f95af2fc3023d0d4f52c0a871a8ea88475096f024b59fd766624b3bf`
- Root keys: `conditions, schema_version, therapies`
- `conditions`: list[6]; union fields: `canonical_surface, condition_id, description, display_name, reversible`
  - row 1: `{"canonical_surface":"hypervigilance","condition_id":"condition_combat_ptsd","description":"The body keeps answering a door that closed months ago. Sleep comes in fits; loud nights undo weeks.","display_name":"Combat-Rooted Startle Sickness","reversible":false}`
  - row 2: `{"canonical_surface":"flashback","condition_id":"condition_flash_blindness_shock","description":"The burst printed itself on the eyes. Vision returns in patches; the dark patches last longer each time.","display_name":"Flash-Blindness After shock","reversible":true}`
  - row 3: `{"canonical_surface":"guilt_insomnia","condition_id":"condition_severe_survivor_guilt","description":"Every ration is an argument with someone who is not here to eat it.","display_name":"Severe Survivor Guilt","reversible":false}`
  - row 4: `{"canonical_surface":"none","condition_id":"condition_paranoid_psychosis","description":"The walls have ears and the ears have allegiances. Grounding work, done slowly, sometimes quiets them.","display_name":"Siege Paranoia","reversible":false}`
  - row 5: `{"canonical_surface":"hypervigilance","condition_id":"condition_chronic_hypervigilance","description":"Watches that never end, kept by people who are no longer on the roster.","display_name":"Chronic Hypervigilance","reversible":true}`
  - row 6: `{"canonical_surface":"guilt_insomnia","condition_id":"condition_guilt_insomnia_loop","description":"The night shift of the conscience, working overtime without pay.","display_name":"Guilt-Insomnia Loop","reversible":true}`
- `therapies`: list[8]; union fields: `acute_stress_reduction_permille, description, display_name, duration_days, eligible_conditions, grants_journal_entry, recovery_progress, relapse_modifier, resource_costs, side_effects, staff_skill_id, staff_skill_threshold, tags, therapy_id, work_restriction`
  - row 1: `{"acute_stress_reduction_permille":400,"description":"A lined cistern, warm water, measured salts, and one hour where nothing demands anything. The quiet does the work.","display_name":"Dark-Tank Immersion","duration_days":1,"eligible_conditions":["condition_combat_ptsd","condition_chronic_hypervigilance"],"recovery_progress":20,"relapse_modifier":0.05,"resource_costs":[{"amount":2,"item_id":"clean_water"},{"amount":1,"item_id":"item_preservation_salt"}],"side_effects":["disorientation_hours"],"staff_skill_id":"skill_watchful","staff_skill_threshold":30,"tags":["immersion","intensive"],"therapy_id":"therapy_sensory_deprivation_immersion","wo…`
  - row 2: `{"acute_stress_reduction_permille":250,"description":"An hour a day of saying the unsayable to someone trained not to flinch.","display_name":"Guided Catharsis Sessions","duration_days":3,"eligible_conditions":["condition_severe_survivor_guilt","condition_guilt_insomnia_loop"],"recovery_progress":25,"relapse_modifier":0.1,"resource_costs":[{"amount":1,"item_id":"clean_water"}],"side_effects":[],"staff_skill_id":"skill_cold_analysis","staff_skill_threshold":40,"tags":["talk","structured"],"therapy_id":"therapy_cognitive_catharsis","work_restriction":"light_duty"}`
  - row 3: `{"acute_stress_reduction_permille":300,"description":"Controlled exposure, one trigger at a time, at the patient's pace. Setbacks are part of the arithmetic.","display_name":"Graded Desensitization","duration_days":4,"eligible_conditions":["condition_combat_ptsd","condition_flash_blindness_shock"],"recovery_progress":30,"relapse_modifier":0.15,"resource_costs":[{"amount":1,"item_id":"bandage"}],"side_effects":["temporary_sleep_disruption"],"staff_skill_id":"skill_cold_analysis","staff_skill_threshold":50,"tags":["exposure","graduated"],"therapy_id":"therapy_trauma_desensitization","work_restriction":"light_duty"}`
  - row 4: `{"acute_stress_reduction_permille":150,"description":"Mornings, a warm lamp, and a blank book. What the patient remembers gets written down and kept — for the archive, and out of the skull.","display_name":"Dream Transcription","duration_days":2,"eligible_conditions":["condition_guilt_insomnia_loop","condition_paranoid_psychosis"],"grants_journal_entry":true,"recovery_progress":15,"relapse_modifier":0.0,"resource_costs":[{"amount":1,"item_id":"paper_stock"}],"side_effects":[],"staff_skill_id":"skill_watchful","staff_skill_threshold":20,"tags":["journal","archive_link"],"therapy_id":"therapy_dream_transcription","work_restriction":"none"}`
  - row 5: `{"acute_stress_reduction_permille":200,"description":"Short, measured draughts to break the worst nights. Not a cure — a pause long enough to start one.","display_name":"Sedative Stabilization","duration_days":1,"eligible_conditions":["condition_paranoid_psychosis","condition_combat_ptsd","condition_guilt_insomnia_loop"],"recovery_progress":5,"relapse_modifier":0.2,"resource_costs":[{"amount":1,"item_id":"sedative_draught"}],"side_effects":["sedation_restriction_days"],"staff_skill_id":"skill_cold_analysis","staff_skill_threshold":25,"tags":["chemical","stabilizing"],"therapy_id":"therapy_sedative_stabilization","work_restriction":"bedrest"}`
  - row 6: `{"acute_stress_reduction_permille":220,"description":"The therapist stands part of the patient's watch beside them, then talks it through while it is still warm.","display_name":"Watch-Rotation Counseling","duration_days":3,"eligible_conditions":["condition_chronic_hypervigilance","condition_combat_ptsd"],"recovery_progress":20,"relapse_modifier":0.1,"resource_costs":[],"side_effects":[],"staff_skill_id":"skill_watchful","staff_skill_threshold":45,"tags":["in_field","talk"],"therapy_id":"therapy_watch_rotation_counseling","work_restriction":"none"}`
  - row 7: `{"acute_stress_reduction_permille":180,"description":"The patient tells the shelter who they lost, and the shelter writes it into the memorial book. Grief with witnesses weighs less.","display_name":"Memorial Testimony Circle","duration_days":2,"eligible_conditions":["condition_severe_survivor_guilt","condition_guilt_insomnia_loop"],"grants_journal_entry":true,"recovery_progress":20,"relapse_modifier":0.05,"resource_costs":[{"amount":1,"item_id":"paper_stock"}],"side_effects":[],"staff_skill_id":"skill_watchful","staff_skill_threshold":35,"tags":["memorial","communal"],"therapy_id":"therapy_memorial_testimony_circle","work_restriction":"none…`
  - row 8: `{"acute_stress_reduction_permille":120,"description":"Half-shifts of real, useful work with a supervisor who stops before the shaking starts. The road back to the roster, graded.","display_name":"Work-Rhythm Restoration","duration_days":5,"eligible_conditions":["condition_flash_blindness_shock","condition_chronic_hypervigilance","condition_paranoid_psychosis"],"recovery_progress":35,"relapse_modifier":0.1,"resource_costs":[{"amount":1,"item_id":"clean_water"}],"side_effects":[],"staff_skill_id":"skill_cold_analysis","staff_skill_threshold":30,"tags":["rehabilitation","graded"],"therapy_id":"therapy_work_rhythm_restoration","work_restriction"…`

# Appendix D — Current caller/reference graph

### `DoseRegistersCatalogLoader` (18 sampled current references)
- Assets/Ashfall.Core/DoseRegistersCatalog.cs:57: public static class DoseRegistersCatalogLoader
- src/Dose/DoseRegisterSurface.cs:174: .Append(" mSv [").Append(DoseRegistersCatalogLoader.BandLabel(_session.Registers, band))
- src/Dose/DoseRegisterSurface.cs:192: sb.Append(DoseRegistersCatalogLoader.BandLabel(_session.Registers, b.band))
- src/Host/DoseLedgerHostSession.cs:80: registers = DoseRegistersCatalogLoader.Load(dataDir, fileIO, serializer);
- Ashfall.Core.Tests/DoseRegistersCatalogTests.cs:31: var catalog = DoseRegistersCatalogLoader.Load(
- Ashfall.Core.Tests/DoseRegistersCatalogTests.cs:48: var catalog = DoseRegistersCatalogLoader.Load(
- Ashfall.Core.Tests/DoseRegistersCatalogTests.cs:73: var catalog = DoseRegistersCatalogLoader.Load(
- Ashfall.Core.Tests/DoseRegistersCatalogTests.cs:87: var catalog = DoseRegistersCatalogLoader.Load(
- Ashfall.Core.Tests/DoseRegistersCatalogTests.cs:90: Assert.Equal("Green", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandGreen));
- Ashfall.Core.Tests/DoseRegistersCatalogTests.cs:91: Assert.Equal("Amber", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandAmber));
- Ashfall.Core.Tests/DoseRegistersCatalogTests.cs:92: Assert.Equal("Red", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandRed));
- Ashfall.Core.Tests/DoseRegistersCatalogTests.cs:93: Assert.Equal("Black", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandBlack));
- Ashfall.Core.Tests/DoseRegistersCatalogTests.cs:99: var catalog = DoseRegistersCatalogLoader.Load(
- Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs:32: return DoseRegistersCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs:157: Assert.Equal("Green",  DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandGreen));
- Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs:158: Assert.Equal("Amber",  DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandAmber));
- Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs:159: Assert.Equal("Red",    DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandRed));
- Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs:160: Assert.Equal("Black",  DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandBlack));
### `BandIdFor` (3 sampled current references)
- Assets/Ashfall.Core/DoseRegistersCatalog.cs:97: if (catalog.bands[i].id == BandIdFor(band))
- Assets/Ashfall.Core/DoseRegistersCatalog.cs:102: public static string BandIdFor(int band)
- Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs:153: // BandIdFor() maps these integers to band ids by lookup — confirm still resolves correctly.
### `DoseLedgerHostSession` (18 sampled current references)
- src/Main.Phase0.cs:38: private DoseLedgerHostSession _doseLedger = null!;
- src/Main.Phase0.cs:368: _doseLedger = DoseLedgerHostSession.Create(_dataDir, campaignRng: _campaignDay.Rng);
- src/Dose/DoseRegisterSurface.cs:17: /// Thin presentation only: renders DoseLedgerHostSession state and
- src/Dose/DoseRegisterSurface.cs:22: private DoseLedgerHostSession _session;
- src/Dose/DoseRegisterSurface.cs:109: public void BindSession(DoseLedgerHostSession session)
- src/Host/DoseLedgerSaveStore.cs:5: // Host Caller: Main.Holdfast, Main.Phase0 / DoseLedgerHostSession
- src/Host/PanelBindLifecycleSelfTest.cs:463: var doseHost = DoseLedgerHostSession.Create(dataDir);
- src/Host/PanelBindLifecycleSelfTest.cs:660: var doseHost1 = new DoseLedgerHostSession();
- src/Host/PanelBindLifecycleSelfTest.cs:661: var doseHost2 = new DoseLedgerHostSession();
- src/Host/PanelBindLifecycleSelfTest.cs:1059: var g17DoseHost = new DoseLedgerHostSession();
- src/Host/DoseLedgerHostSession.cs:19: public sealed class DoseLedgerHostSession
- src/Host/DoseLedgerHostSession.cs:35: public DoseLedgerHostSession(
- src/Host/DoseLedgerHostSession.cs:70: public static DoseLedgerHostSession Create(string dataDir, ILog log = null!, ICampaignRngManager? campaignRng = null)
- src/Host/DoseLedgerHostSession.cs:91: return new DoseLedgerHostSession(registers: registers, content: content, quests: quests, campaignRng: campaignRng);
- src/Host/HostCli.PanelTests.cs:1850: var session = DoseLedgerHostSession.Create(dataDirectory);
- src/Host/HostCli.PanelTests.cs:1882: var fresh = DoseLedgerHostSession.Create(dataDirectory);
- src/YearOfAsh/YearOfAshHostSession.cs:127: // DoseLedgerHostSession / DoseLedgerSave (v2+). Older
- src/UI/DoseGeographyPanel.cs:23: /// Pure presentation — reads only from <see cref="DoseLedgerHostSession"/>.
### `DoseLedgerPanel` (18 sampled current references)
- src/Main.Onboarding.cs:224: _doseLedgerPanel?.RefreshView();
- src/Main.PanelLifecycle.cs:86: _doseLedgerPanel,
- src/Main.UiPanels.cs:155: private DoseLedgerPanel _doseLedgerPanel = null!;
- src/Main.UiPanels.cs:1129: _doseLedgerPanel = new DoseLedgerPanel { Visible = false };
- src/Main.UiPanels.cs:1130: _doseLedgerPanel.OnClose += () => _doseLedgerPanel.Visible = false;
- src/Main.UiPanels.cs:1131: _doseLedgerPanel.OnSurvivorSelected += survivorId =>
- src/Main.UiPanels.cs:1138: AddChild(_doseLedgerPanel);
- src/Main.PlayerSurfaces.cs:579: bindAction: () => { SetupPhase0(); SetupSurvivors(); _doseLedgerPanel.Bind(_doseLedger, _survivors); },
- src/Main.PlayerSurfaces.cs:580: openAction: () => { ObserveSigil("dose.read"); _doseLedgerPanel.Open(); },
- src/Main.PlayerSurfaces.cs:581: closeAction: () => ClosePanelAnimated(_doseLedgerPanel));
- src/Host/PanelBindLifecycleSelfTest.cs:451: // ── GATE 10: DutyRosterPanel and DoseLedgerPanel Callback Lifecycle ──
- src/Host/PanelBindLifecycleSelfTest.cs:452: GD.Print("\n[Gate 10] Testing DutyRosterPanel and DoseLedgerPanel lifecycle...");
- src/Host/PanelBindLifecycleSelfTest.cs:464: var dosePanel = new DoseLedgerPanel();
- src/Host/PanelBindLifecycleSelfTest.cs:466: if (!dosePanel.IsBound) { GD.PrintErr("[FAIL] Gate 10: DoseLedgerPanel IsBound false."); return 1; }
- src/Host/PanelBindLifecycleSelfTest.cs:468: if (dosePanel.IsBound) { GD.PrintErr("[FAIL] Gate 10: DoseLedgerPanel IsBound true after unbind."); return 1; }
- src/Host/PanelBindLifecycleSelfTest.cs:472: GD.Print("[PASS] Gate 10: DutyRosterPanel and DoseLedgerPanel verified cleanly.");
- src/Host/PanelBindLifecycleSelfTest.cs:514: new DoseLedgerPanel(),
- src/UI/SnapshotHarness.cs:62: new Target{ StableId="dose_ledger_default",          Title="Dose Ledger (#59 Stitch)",                  PanelCtor="AtomicWar.GodotApp.UI.DoseLedgerPanel",                 StateHint="default", Width=1920, Height=1080 },
### `dose_ledger` (18 sampled current references)
- Assets/Ashfall.Core/DoseLedgerSystem.cs:57: public const string SystemId = "dose_ledger_system";
- Assets/Ashfall.Core/DoseLedgerSystem.cs:336: catalog.ApplyRetention("dose_ledger", entry.readingsHistory, out int pruned);
- Assets/Ashfall.Core/VersionReport.cs:85: new SaveSchemaEntry("dose_ledger",       DoseLedgerSave.CurrentSaveVersion),
- Assets/Ashfall.Core/VersionReport.cs:243: /// e.g. "holdfast v5 · year_of_ash v4 · dose_ledger v2 · expansion_hub v4 · expansion_quest v1 (+55 checksum envelopes)".
- Assets/Ashfall.Core/CatalogIntegrityValidator.cs:472: // (kitchen_serving_log, machine_log, dose_ledger, …), never a catalog
- Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs:151: R("dose_ledger",         "Dose Ledger",                   PanelGroup.Expanded,   new[] { "phase0" });
- Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs:137: "dose_ledger",
- Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:64: new("dose_ledger", "SaveDoseLedger", "SetupDoseLedger", "dose_ledger", "Survivor radiation dose ledger & cohorts"),
- Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:373: { "dose_ledger", "dose_ledger_save.json" },
- Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:628: { "dose_ledger", 2 },
- Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs:104: "dose_ledger",
- Assets/Ashfall.Core/Records/RetentionPolicy.cs:241: RegisterPolicy(new RetentionPolicyDefinition("dose_ledger", 500, RetentionAction.RollupSummary));
- src/Main.Phase0.cs:469: if (CaptureSection("dose_ledger", DoseLedgerSaveStore.TryCapturePersisted(_doseLedger.CaptureSave(day))))
- src/Main.PlayerSurfaces.cs:578: PanelRegistry.ConfigureActions("dose_ledger",
- src/Host/DoseLedgerSaveStore.cs:15: /// user://dose_ledger_save.json — thin façade over the Core
- src/Host/DoseLedgerSaveStore.cs:22: public const string FileName = "dose_ledger_save.json";
- src/Host/DoseLedgerSaveStore.cs:23: public const string SectionName = "dose_ledger";
- src/Host/RetentionHostSession.cs:135: ApplyOwner("dose_ledger",

# Appendix E — Current focused-test inventory

Current test declaration inventory: 60 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/DoseRegistersCatalogTests.cs` — 12 test declarations; bytes=5,676; SHA-256=`2538daa36d3bb88e10dd9689a15263cf2cbdcaf244109efa3a4e45687fea712d`
- 00025: [Fact]
- 00026: public void Load_FindsFourBandsThreePlansThreeGuesses()
- 00042: [Fact]
- 00043: public void Load_FindsTheFourAntagonists()
- 00067: [Fact]
- 00068: public void Load_BandThresholdsBind()
- 00084: [Fact]
- 00085: public void BandLabel_MapsCoreBandsToVocabulary()
- 00096: [Fact]
- 00097: public void Load_MissingDirectoryReturnsEmptyCatalog()
- 00105: [Fact]
- 00106: public void Characters_RegisterTheFourAntagonists()
### `Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs` — 16 test declarations; bytes=6,834; SHA-256=`0e9bf585db0e5c71b66c76ec3d26909f60f0e9a08cfe5761088ae6abb689068c`
- 00035: [Fact]
- 00036: public void Catalog_HasTwelveBandsAndEightPlans()
- 00045: [Fact]
- 00046: public void Bands_ThresholdsAreStrictlyIncreasing()
- 00060: [Fact]
- 00061: public void Bands_AllIdsAreUniqueAndNonEmpty()
- 00076: [Fact]
- 00077: public void Bands_NewEntriesExist()
- 00099: [Fact]
- 00100: public void Plans_AllIdsAreUniqueAndNonEmpty()
- 00116: [Fact]
- 00117: public void Plans_NewEntriesExist()
- 00135: [Fact]
- 00136: public void Plans_CostsAreNonEmpty()
- 00149: [Fact]
- 00150: public void BandLabel_BackwardCompatibilityForCoreFourBands()
### `Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs` — 8 test declarations; bytes=5,777; SHA-256=`8aa74cc5c553e524d4cfda7475652564dd5ec0d9eadd524afb6f9ccea46f464e`
- 00032: [Fact]
- 00033: public void DoseRegisters_HasTwelveBandsAndEightPlansWithFourNpcs()
- 00047: [Fact]
- 00048: public void DoseRegisters_BandsAreStrictlyIncreasingAndBackwardCompatible()
- 00074: [Fact]
- 00075: public void ArchiveInks_HasTwelveEntriesWithUniqueIds()
- 00103: [Fact]
- 00104: public void CrossSystem_DoseAndInkCatalogsLoadIndependently()
### `Ashfall.Core.Tests/DoseLedgerSystemTests.cs` — 24 test declarations; bytes=8,564; SHA-256=`e983e95d8bc0ef4c41532ae7d17735990ffe850c3259a92b892e4b0d9ac069bc`
- 00010: [Fact]
- 00011: public void BookReading_WithoutTag_IsNotBooked()
- 00019: [Fact]
- 00020: public void BookReading_CrossesAmberBand_AndFiresEvent()
- 00032: [Fact]
- 00033: public void AntiRadAfter_ReducesBookedDose()
- 00041: [Fact]
- 00042: public void FluxAmbiguity_IsDeterministicPerSeed()
- 00055: [Fact]
- 00056: public void CaptureRestore_RoundTrips()
- 00069: [Fact]
- 00070: public void ForgedCleanBill_ProvidesGreenBandAdministratively_WithoutMutatingPhysicalDose()
- 00089: [Fact]
- 00090: public void AdminOverrideBand_ChangesAdminBand_PreservesBookedDose()
- 00103: [Fact]
- 00104: public void CaptureRestore_PreservesForgedAndOverrideState()
- 00127: [Fact]
- 00128: public void TamperedSave_IsHardRejectedOnDecode()
- 00152: [Fact]
- 00153: public void ChecksumlessSave_IsRejectedOnDecode()
- 00170: [Fact]
- 00171: public void BookChild_ThenCorrectBaseline()
- 00181: [Fact]
- 00182: public void Volunteer_SignAndComplete_BanksDose()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Dose Registers: Twelve Display Bands, Care Plans, and Canonical Ledger Boundaries` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan calls all twelve rows active clinical bands; current `BandIdFor` only maps four canonical ids.
- The historical plan implies cost items drive care; current register text is not a typed treatment command.

## H.2 Evidence-strength corrections
- Separate the four canonical band mappings from the twelve display rows.
- Treat unused care plans as dormant rather than executable.
- Use the current medical pipeline and persistence seams in every handoff.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Dose Registers: Twelve Display Bands, Care Plans, and Canonical Ledger Boundaries while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use DoseRegistersCatalog for static display vocabulary only.
- Use DoseLedgerSystem for cumulative dose, canonical bands, and ledger mutations.
- Route treatment through the medical owner, never through register prose.
- Keep the existing dose_ledger save section and codec.
- Present truthful, accessible register state without prescribing hidden effects.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement dose registers: twelve display bands, care plans, and canonical ledger boundaries arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- The final plan must not propose new clinical thresholds or care-plan execution.
- The final QA must distinguish current source evidence from historical Wave 39 pass claims.

## J.2 Questions deliberately left open
- Which medical owner, if any, should authorize a future expanded-band mapping?
- Should dormant register rows remain visible as historical records or be hidden from the live panel?

## J.3 Handoff acceptance
- The next agent can identify the sole owner, the safe extension route, the existing save owner, the host/UI boundary, the deterministic contract, and the focused commands without reinterpreting this document.
- If any open question becomes a new architecture decision, stop and return `STALE_PLAN` or a decision packet rather than improvising.

# Appendix K — Quality-assurance record

- Evidence source order was applied: current repository first, project instructions second, compiled authority third, ledgers fourth.
- No production/data/test/UI/save/runtime file was edited by this documentation-only package.
- Source hashes and path existence are rechecked externally after generation; a stale hash is a failed handoff, not a historical footnote.
- Current test declarations are enumerated, but no fresh test pass is implied by enumeration.
- The plan separates terminal content expansion from any residual reachability or integration work.
- Size is a gate, not a quality proxy: the document must remain navigable, evidence-linked, and free of repeated generated filler.

# Appendix L — Original baseline intent (Git HEAD, preserved)

# Appendix L — Original baseline intent (Git HEAD, preserved)

The original file at `HEAD:piagentsplans/90-dose-registers-expansion.md` contained 4,719 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 90 — Dose Register Bands & Plans Expansion (4 bands → 12 bands, 3 plans → 8 plans)

## Goal (2 lines)
Expand `dose_registers.json` from 4 radiation bands and 3 care plans to 12 bands
and 8 plans. The dose register system (`DoseRegistersCatalog.cs` confirmed live)
defines radiation-exposure thresholds, care plans, guess options, calibration,
register actions, and dose-ledger NPCs. The bands are too coarse (only 4:
green/amber/red/black) and the care plans are too few (only 3).

## Why (P2)
- Verified: `dose_registers.json` has 4 bands (id, label, threshold_msv,
  disposition), 3 plans (id, label, cost, note), 3 guesses, 1 calibration, 4
  registers, and 4 NPCs. `DoseRegistersCatalog.cs` is confirmed in Core.
- Creates the dose-management pillar: the dose register is the shelter's
  radiation-tracking bureaucracy — bands classify exposure, plans define care
  options, guesses let the player estimate before a reading. 4 bands (0, 100,
  300, 600 mSv) is too coarse; the mid-range (100–600) needs granularity.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/dose_registers.json` (expand bands 4 → 12,
  plans 3 → 8)
- Read-only: `Assets/Ashfall.Core/DoseRegistersCatalog.cs` (confirm schema and
  how bands/plans/registers/NPCs are consumed)

## Content grammar (per band)
- `id`: snake_case with prefix `band_` (confirmed prefix).
- `label`: short label (Green, Amber, Red, Black, and new intermediate bands).
- `threshold_msv`: integer millisieverts — the cumulative dose threshold that
  activates this band. Must be strictly increasing across bands.
- `disposition`: 1 sentence of clinical text describing what this band means
  (match the existing cold, bureaucratic tone — "Named on the sick list. Care
  is a choice, not a cure.").

## Content grammar (per plan)
- `id`: snake_case with prefix `plan_` (confirmed prefix).
- `label`: short label (Morphine tray, Comfort rounds, Nothing).
- `cost`: item id or "time" or "none" — the resource cost of the care plan.
- `note`: 1 sentence of prose describing the plan (match the existing tone).

## Steps
1. Read `DoseRegistersCatalog.cs` to confirm how bands are selected (by
   cumulative dose falling within threshold ranges) and how plans are applied.
2. Read the existing 4 bands and 3 plans to confirm the quality bar.
3. Author 8 new bands with finer granularity:
   - `band_white` (0 mSv): baseline, no exposure.
   - `band_yellow` (50 mSv): minor exposure, watch.
   - `band_orange` (150 mSv): moderate exposure, restricted duty.
   - `band_rose` (200 mSv): significant exposure, light duty.
   - `band_crimson` (400 mSv): severe exposure, sick list.
   - `band_violet` (500 mSv): critical exposure, comfort care.
   - `band_indigo` (800 mSv): terminal exposure, palliative only.
   - `band_void` (1000 mSv): lethal exposure, the registrar stops reading.
4. Author 5 new care plans:
   - `plan_chelation` (cost: chelation_agent): chelation therapy to reduce
     body burden.
   - `plan_iodine_prophylaxis` (cost: potassium_iodide): thyroid protection.
   - `plan_isolation` (cost: time): isolate the patient to prevent secondary
     exposure.
   - `plan_rest` (cost: time): bed rest and monitoring.
   - `plan_transfer` (cost: fuel): transfer to a better-equipped facility
     (if one exists).
5. Each band: distinct threshold_msv (strictly increasing), label, and
   disposition. Each plan: distinct cost and note.
6. Cross-reference: all band ids unique; threshold_msv strictly increasing;
   all plan ids unique; plan costs reference existing items or "time"/"none".
7. Validate: `--data-integrity-selftest` (all ids resolve).
8. xUnit: dose register catalog loads 12 bands (thresholds strictly increasing)
   and 8 plans, all ids unique, all item-cost plans resolve in items.json.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is threshold ordering (step 5): bands must be
strictly increasing by threshold_msv, and the system must select the correct
band for a given cumulative dose.

## Definition of Done
- `dose_registers.json` has 12 bands (strictly increasing thresholds) and 8
  plans, all ids resolving, integrity + tests green.

## Follow-on
- Plan 81 (dose locations) — locations feed dose accumulation into bands.
- Plan 09B (radiation system) — bands classify radiation exposure.
- Plan 79 (autopsy procedures) — terminal-band patients may die and require
  autopsy.
- Plan 83 (weather seasons) — seasonal fallout pushes survivors into higher
  bands.
- Existing 09B (radiation) — this plan provides the band/plan data.

```

## End of Plan 90 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/DoseRegistersCatalog.cs` — 114 lines; 3,973 bytes; SHA-256 `1052c82073cc4256279484603721a46e084caebc35e17f3646c0ac8a1fa74363`
Declaration index:
- 00009: public class DoseBandDef
- 00018: public class DosePlanDef
- 00027: public class DoseGuessDef
- 00036: public class DoseRegisterNpcDef
- 00048: public class DoseRegistersCatalog
- 00057: public static class DoseRegistersCatalogLoader
- 00061: public static DoseRegistersCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
- 00094: public static string BandLabel(DoseRegistersCatalog catalog, int band)
- 00102: public static string BandIdFor(int band)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: using Ashfall.Core.IO;
00006: namespace Ashfall.Core
00007: {
00008:     /// <summary>One dose-band vocabulary row (dose_registers.json).</summary>
00009:     public class DoseBandDef
00010:     {
00011:         public string id = string.Empty;
00012:         public string label = string.Empty;
00013:         public float threshold_msv;
00014:         public string disposition = string.Empty;
00015:     }
00016:
00017:     /// <summary>One palliative-plan vocabulary row.</summary>
00018:     public class DosePlanDef
00019:     {
00020:         public string id = string.Empty;
00021:         public string label = string.Empty;
00022:         public string cost = string.Empty;
00023:         public string note = string.Empty;
00024:     }
00025:
00026:     /// <summary>One cohort guess vocabulary row.</summary>
00027:     public class DoseGuessDef
00028:     {
00029:         public string id = string.Empty;
00030:         public string label = string.Empty;
00031:         public bool pencil;
00032:         public string note = string.Empty;
00033:     }
00034:
00035:     /// <summary>One chaired antagonist row (PART B: the four accountants).</summary>
00036:     public class DoseRegisterNpcDef
00037:     {
00038:         public string id = string.Empty;
00039:         public string name = string.Empty;
00040:         public string register = string.Empty;
00041:         public string disposition = string.Empty;
00042:         public string action_label = string.Empty;
00043:         public string action = string.Empty;
00044:     }
00045:
00046:     /// <summary>The dose_registers.json vocabulary (A4) — display strings only,
00047:     /// so the host never hardcodes band/plan/guess text.</summary>
00048:     public class DoseRegistersCatalog
00049:     {
00050:         public List<DoseBandDef> bands = new List<DoseBandDef>();
00051:         public List<DosePlanDef> plans = new List<DosePlanDef>();
00052:         public List<DoseGuessDef> guesses = new List<DoseGuessDef>();
00053:         public List<DoseRegisterNpcDef> npcs = new List<DoseRegisterNpcDef>();
00054:     }
00055:
00056:     /// <summary>Engine-agnostic loader for dose_registers.json.</summary>
00057:     public static class DoseRegistersCatalogLoader
00058:     {
00059:         public const string FileName = "dose_registers.json";
00060:
00061:         public static DoseRegistersCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
00062:         {
00063:             var catalog = new DoseRegistersCatalog();
00064:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00065:                 return catalog;
00066:
00067:             string path = fileIO.Combine(dataDir, FileName);
00068:             if (!fileIO.FileExists(path))
00069:                 return catalog;
00070:
00071:             string raw = fileIO.ReadAllText(path);
00072:             if (string.IsNullOrWhiteSpace(raw))
00073:                 return catalog;
00074:
00075:             try
00076:             {
00077:                 var parsed = json.Deserialize<DoseRegistersCatalog>(raw);
00078:                 if (parsed != null)
00079:                 {
00080:                     if (parsed.bands != null) catalog.bands = parsed.bands;
00081:                     if (parsed.plans != null) catalog.plans = parsed.plans;
00082:                     if (parsed.guesses != null) catalog.guesses = parsed.guesses;
00083:                     if (parsed.npcs != null) catalog.npcs = parsed.npcs;
00084:                 }
00085:             }
00086:             catch (Exception ex_CATDIAG)
00087:             {
00088:                 CatalogDiagnostics.Warn(path, "DoseRegistersCatalog", ex_CATDIAG);
00089:                 return catalog;
00090:             }
00091:             return catalog;
00092:         }
00093:
00094:         public static string BandLabel(DoseRegistersCatalog catalog, int band)
00095:         {
00096:             for (int i = 0; i < catalog.bands.Count; i++)
00097:                 if (catalog.bands[i].id == BandIdFor(band))
00098:                     return catalog.bands[i].label;
00099:             return "Band " + band;
00100:         }
00101:
00102:         public static string BandIdFor(int band)
00103:         {
00104:             switch (band)
00105:             {
00106:                 case 0: return "band_green";
00107:                 case 1: return "band_amber";
00108:                 case 2: return "band_red";
00109:                 case 3: return "band_black";
00110:                 default: return "band_green";
00111:             }
00112:         }
00113:     }
00114: }
```

## `Assets/Ashfall.Core/DoseLedgerSystem.cs` — 352 lines; 15,416 bytes; SHA-256 `8de849230f31198890a51cffd56dd5d740a2a8fa8d1535ae130bac248cfec9fb`
Declaration index:
- 00012: public class DoseReading
- 00023: public class DoseEntry
- 00038: public class DoseLedgerSystemState
- 00055: public class DoseLedgerSystem
- 00083: public bool AssignDosimeter(string survivorId, string tag, float baselineMsv = 0f)
- 00094: public void SetShieldingFactor(string survivorId, float factor)
- 00101: public void RecordAntiRadTreatment(string survivorId, int day)
- 00110: public void Calibrate(string survivorId, int day)
- 00122: public DoseBandResult BookReading(
- 00181: public static int BandFor(float mSv)
- 00189: private static DoseBandResult BandForValue(float mSv)
- 00196: public void SetForgedCleanBill(string survivorId, bool hasForged)
- 00204: public void SetAdministrativeClassificationOverride(string survivorId, string overrideBand)
- 00216: public int GetAdministrativeBand(string survivorId)
- 00232: public DoseEntry? GetEntry(string survivorId) =>
- 00235: public float GetCumulative(string survivorId) =>
- 00240: public DoseLedgerSystemState CaptureState()
- 00275: public void RestoreState(DoseLedgerSystemState saved)
- 00311: private DoseEntry GetOrCreate(string survivorId)
- 00320: private void RaiseChanged() => OnStateChanged?.Invoke(_state);
- 00329: public int ApplyRetention(Records.RetentionPolicyCatalog? catalog)
- 00343: /// <summary>Mirror of the band enum for BookReading return values.</summary>
- 00344: public enum DoseBandResult
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core
00007: {
00008:     // ── Read models ─────────────────────────────────────────────────
00009:
00010:     /// <summary>One booked dose reading against a named survivor.</summary>
00011:     [Serializable]
00012:     public class DoseReading
00013:     {
00014:         public int day;
00015:         public string source;              // event id or freeform cause
00016:         public float nominalMsv;           // what the dial showed
00017:         public float bookedMsv;            // what was written after flux/shielding/anti-rad
00018:         public bool fluxAmbiguous;         // the reading was a range, not a point
00019:         public bool antiRadAfter;          // booked dose reduced post-exposure
00020:     }
00021:
00022:     [Serializable]
00023:     public class DoseEntry
00024:     {
00025:         public string survivorId;
00026:         public float baselineMsv;          // inherited, never zeroed
00027:         public float cumulativeMsv;
00028:         public string assignedDosimeterTag; // null/empty = not booked going forward
00029:         public List<DoseReading> readingsHistory = new List<DoseReading>();
00030:         public int radiationPhaseCaught;   // the phase index when the ledger caught it
00031:         public float shieldingFactor = 1f;
00032:         public int lastAntiRadDay = -1;
00033:         public bool hasForgedCleanBill;
00034:         public string administrativeClassificationOverride = string.Empty;
00035:     }
00036:
00037:     [Serializable]
00038:     public class DoseLedgerSystemState
00039:     {
00040:         public string systemId = DoseLedgerSystem.SystemId;
00041:         public List<DoseEntry> entries = new List<DoseEntry>();
00042:         public float ceilingMsv = 600f;        // the Black threshold
00043:         public int readingsSinceLastCalibration;
00044:         public bool calibrationOverdue;
00045:     }
00046:
00047:     // ── System ──────────────────────────────────────────────────────
00048:
00049:     /// <summary>
00050:     /// ASHFALL: THE DOSE — per-survivor cumulative dose as a kept document.
00051:     /// Ports the dose *record*, not the radiation physics (which lives in
00052:     /// RadiationSystem). Readings are only booked against survivors with an
00053:     /// assigned dosimeter tag; unbooked rads are the shelter's silence.
00054:     /// </summary>
00055:     public class DoseLedgerSystem
00056:     {
00057:         public const string SystemId = "dose_ledger_system";
00058:         public const float AmberMsv = 100f;
00059:         public const float RedMsv = 300f;
00060:         public const float BlackMsv = 600f;
00061:         public const int ReadingsPerCalibration = 40;
00062:
00063:         // Heat-band thresholds exposed for the Sick List / UI to reuse.
00064:         public const int BandGreen = 0;
00065:         public const int BandAmber = 1;
00066:         public const int BandRed = 2;
00067:         public const int BandBlack = 3;
00068:
00069:         private readonly DoseLedgerSystemState _state = new DoseLedgerSystemState();
00070:         private readonly Dictionary<string, DoseEntry> _entries = new Dictionary<string, DoseEntry>();
00071:
00072:         public event Action<string, float> OnDoseCorrected;       // survivorId, bookedMsv
00073:         public event Action<string, int> OnBandReached;           // survivorId, band
00074:         public event Action OnLedgerCalibrated;
00075:         public event Action<DoseLedgerSystemState> OnStateChanged;
00076:
00077:         public DoseLedgerSystemState State => _state;
00078:         public IReadOnlyList<DoseEntry> Entries => _state.entries;
00079:
00080:         // ── Assignment ──────────────────────────────────────────────
00081:
00082:         /// <summary>Tag a survivor so their future exposures can be booked.</summary>
00083:         public bool AssignDosimeter(string survivorId, string tag, float baselineMsv = 0f)
00084:         {
00085:             if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(tag)) return false;
00086:             var entry = GetOrCreate(survivorId);
00087:             if (entry.baselineMsv <= 0f) entry.baselineMsv = Math.Max(0f, baselineMsv);
00088:             entry.cumulativeMsv = Math.Max(entry.cumulativeMsv, entry.baselineMsv);
00089:             entry.assignedDosimeterTag = tag;
00090:             RaiseChanged();
00091:             return true;
00092:         }
00093:
00094:         public void SetShieldingFactor(string survivorId, float factor)
00095:         {
00096:             var e = GetOrCreate(survivorId);
00097:             e.shieldingFactor = factor > 0f ? factor : 1f;
00098:             RaiseChanged();
00099:         }
00100:
00101:         public void RecordAntiRadTreatment(string survivorId, int day)
00102:         {
00103:             if (string.IsNullOrEmpty(survivorId)) return;
00104:             var e = GetOrCreate(survivorId);
00105:             e.lastAntiRadDay = day;
00106:             RaiseChanged();
00107:         }
00108:
00109:         /// <summary>Refund calibration accuracy after the configured reading count.</summary>
00110:         public void Calibrate(string survivorId, int day)
00111:         {
00112:             if (string.IsNullOrEmpty(survivorId)) return;
00113:             var e = GetOrCreate(survivorId);
00114:             _state.readingsSinceLastCalibration = 0;
00115:             _state.calibrationOverdue = false;
00116:             OnLedgerCalibrated?.Invoke();
00117:             RaiseChanged();
00118:         }
00119:
00120:         // ── Book a reading ──────────────────────────────────────────
00121:
00122:         public DoseBandResult BookReading(
00123:             string survivorId,
00124:             int day,
00125:             float nominalMsv,
00126:             string source,
00127:             bool highEnergyEvent,
00128:             bool antiRadBefore,
00129:             bool antiRadAfter,
00130:             ISeededRng rng)
00131:         {
00132:             if (string.IsNullOrEmpty(survivorId) || nominalMsv <= 0f) return DoseBandResult.NoEntry;
00133:             var entry = GetOrCreate(survivorId);
00134:             if (entry == null) return DoseBandResult.NoEntry;
00135:             if (string.IsNullOrEmpty(entry.assignedDosimeterTag)) return DoseBandResult.NoEntry;
00136:
00137:             // Flux ambiguity: a high-energy event makes the dial a range, not a point.
00138:             bool fluxAmbiguous = highEnergyEvent && rng != null;
00139:             float ratio = 1f;
00140:             if (fluxAmbiguous && rng != null) ratio = 0.85f + rng.NextFloat() * 0.30f;
00141:
00142:             // Pre-exposure anti-rad attenuates the incoming dose.
00143:             float incoming = antiRadBefore ? nominalMsv * 0.5f : nominalMsv;
00144:             // Shielding attenuates what reaches the body.
00145:             incoming *= entry.shieldingFactor;
00146:             // Post-exposure anti-rad reduces what is booked.
00147:             float booked = antiRadAfter ? incoming * 0.6f : incoming;
00148:             if (fluxAmbiguous) booked *= ratio;
00149:
00150:             var reading = new DoseReading
00151:             {
00152:                 day = day,
00153:                 source = source ?? string.Empty,
00154:                 nominalMsv = nominalMsv,
00155:                 bookedMsv = booked,
00156:                 fluxAmbiguous = fluxAmbiguous,
00157:                 antiRadAfter = antiRadAfter
00158:             };
00159:             entry.readingsHistory.Add(reading);
00160:             if (antiRadAfter || antiRadBefore) entry.lastAntiRadDay = day;
00161:
00162:             float before = entry.cumulativeMsv;
00163:             entry.cumulativeMsv += booked;
00164:
00165:             _state.readingsSinceLastCalibration++;
00166:             if (_state.readingsSinceLastCalibration >= ReadingsPerCalibration)
00167:                 _state.calibrationOverdue = true;
00168:
00169:             OnDoseCorrected?.Invoke(survivorId, booked);
00170:
00171:             int bandBefore = BandFor(before);
00172:             int bandAfter = BandFor(entry.cumulativeMsv);
00173:             if (bandAfter > bandBefore)
00174:                 OnBandReached?.Invoke(survivorId, bandAfter);
00175:
00176:             RaiseChanged();
00177:             return BandForValue(entry.cumulativeMsv);
00178:         }
00179:
00180:         /// <summary>The band label for a cumulative total.</summary>
00181:         public static int BandFor(float mSv)
00182:         {
00183:             if (mSv >= BlackMsv) return BandBlack;
00184:             if (mSv >= RedMsv) return BandRed;
00185:             if (mSv >= AmberMsv) return BandAmber;
00186:             return BandGreen;
00187:         }
00188:
00189:         private static DoseBandResult BandForValue(float mSv)
00190:         {
00191:             return (DoseBandResult)BandFor(mSv);
00192:         }
00193:
00194:         // ── Administrative Classification & Forgery ───────────────
00195:
00196:         public void SetForgedCleanBill(string survivorId, bool hasForged)
00197:         {
00198:             if (string.IsNullOrEmpty(survivorId)) return;
00199:             var e = GetOrCreate(survivorId);
00200:             e.hasForgedCleanBill = hasForged;
00201:             RaiseChanged();
00202:         }
00203:
00204:         public void SetAdministrativeClassificationOverride(string survivorId, string overrideBand)
00205:         {
00206:             if (string.IsNullOrEmpty(survivorId)) return;
00207:             var e = GetOrCreate(survivorId);
00208:             e.administrativeClassificationOverride = overrideBand ?? string.Empty;
00209:             RaiseChanged();
00210:         }
00211:
00212:         /// <summary>
00213:         /// Gets the institutional administrative classification band (for check-points and duty gates).
00214:         /// Forged clean-bill chits return Green band administratively without mutating true physical dose.
00215:         /// </summary>
00216:         public int GetAdministrativeBand(string survivorId)
00217:         {
00218:             if (!_entries.TryGetValue(survivorId, out var e) || e == null) return BandGreen;
00219:             if (e.hasForgedCleanBill) return BandGreen;
00220:             if (!string.IsNullOrEmpty(e.administrativeClassificationOverride))
00221:             {
00222:                 if (e.administrativeClassificationOverride == "band_green") return BandGreen;
00223:                 if (e.administrativeClassificationOverride == "band_amber") return BandAmber;
00224:                 if (e.administrativeClassificationOverride == "band_red") return BandRed;
00225:                 if (e.administrativeClassificationOverride == "band_black") return BandBlack;
00226:             }
00227:             return BandFor(e.cumulativeMsv);
00228:         }
00229:
00230:         // ── Queries ────────────────────────────────────────────────
00231:
00232:         public DoseEntry? GetEntry(string survivorId) =>
00233:             _entries.TryGetValue(survivorId, out var e) ? e : null;
00234:
00235:         public float GetCumulative(string survivorId) =>
00236:             _entries.TryGetValue(survivorId, out var e) ? e.cumulativeMsv : 0f;
00237:
00238:         // ── Save / Load ─────────────────────────────────────────────
00239:
00240:         public DoseLedgerSystemState CaptureState()
00241:         {
00242:             // Fresh copy, ordinal-ordered: never return the live state to the
00243:             // envelope (aliasing), and dictionary iteration order is not a
00244:             // cross-host guarantee, so entries are emitted sorted by survivor id.
00245:             var copy = new DoseLedgerSystemState
00246:             {
00247:                 systemId = _state.systemId,
00248:                 ceilingMsv = _state.ceilingMsv,
00249:                 readingsSinceLastCalibration = _state.readingsSinceLastCalibration,
00250:                 calibrationOverdue = _state.calibrationOverdue
00251:             };
00252:             var keys = new List<string>(_entries.Count);
00253:             foreach (var kv in _entries) keys.Add(kv.Key);
00254:             keys.Sort(string.CompareOrdinal);
00255:             for (int i = 0; i < keys.Count; i++)
00256:             {
00257:                 var e = _entries[keys[i]];
00258:                 copy.entries.Add(new DoseEntry
00259:                 {
00260:                     survivorId = e.survivorId,
00261:                     baselineMsv = e.baselineMsv,
00262:                     cumulativeMsv = e.cumulativeMsv,
00263:                     assignedDosimeterTag = e.assignedDosimeterTag,
00264:                     shieldingFactor = e.shieldingFactor,
00265:                     lastAntiRadDay = e.lastAntiRadDay,
00266:                     radiationPhaseCaught = e.radiationPhaseCaught,
00267:                     hasForgedCleanBill = e.hasForgedCleanBill,
00268:                     administrativeClassificationOverride = e.administrativeClassificationOverride,
00269:                     readingsHistory = new List<DoseReading>(e.readingsHistory)
00270:                 });
00271:             }
00272:             return copy;
00273:         }
00274:
00275:         public void RestoreState(DoseLedgerSystemState saved)
00276:         {
00277:             if (saved == null) return;
00278:             _state.systemId = SystemId;
00279:             _state.ceilingMsv = saved.ceilingMsv;
00280:             _state.readingsSinceLastCalibration = saved.readingsSinceLastCalibration;
00281:             _state.calibrationOverdue = saved.calibrationOverdue;
00282:             _entries.Clear();
00283:             _state.entries.Clear();
00284:             if (saved.entries != null)
00285:             {
00286:                 foreach (var e in saved.entries)
00287:                 {
00288:                     if (e == null || string.IsNullOrEmpty(e.survivorId)) continue;
00289:                     var copy = new DoseEntry
00290:                     {
00291:                         survivorId = e.survivorId,
00292:                         baselineMsv = e.baselineMsv,
00293:                         cumulativeMsv = e.cumulativeMsv,
00294:                         assignedDosimeterTag = e.assignedDosimeterTag,
00295:                         shieldingFactor = e.shieldingFactor,
00296:                         lastAntiRadDay = e.lastAntiRadDay,
00297:                         radiationPhaseCaught = e.radiationPhaseCaught,
00298:                         hasForgedCleanBill = e.hasForgedCleanBill,
00299:                         administrativeClassificationOverride = e.administrativeClassificationOverride ?? string.Empty,
00300:                         readingsHistory = e.readingsHistory != null
00301:                             ? new List<DoseReading>(e.readingsHistory)
00302:                             : new List<DoseReading>()
00303:                     };
00304:                     _entries[e.survivorId] = copy;
00305:                     _state.entries.Add(copy);
00306:                 }
00307:             }
00308:             RaiseChanged();
00309:         }
00310:
00311:         private DoseEntry GetOrCreate(string survivorId)
00312:         {
00313:             if (_entries.TryGetValue(survivorId, out var existing)) return existing;
00314:             var entry = new DoseEntry { survivorId = survivorId };
00315:             _entries[survivorId] = entry;
00316:             _state.entries.Add(entry);
00317:             return entry;
00318:         }
00319:
00320:         private void RaiseChanged() => OnStateChanged?.Invoke(_state);
00321:
00322:         /// <summary>
00323:         /// Plan 55 / Task 55A — apply the retention catalog to the dose ledger's
00324:         /// per-survivor reading histories. Cumulative dose, band and ledger
00325:         /// classification are owned here and are never touched by retention: only
00326:         /// the historical reading rows are bounded, which is what lets a survivor
00327:         /// who has been in the field for years still produce a small save.
00328:         /// </summary>
00329:         public int ApplyRetention(Records.RetentionPolicyCatalog? catalog)
00330:         {
00331:             if (catalog == null) return 0;
00332:             int total = 0;
00333:             foreach (var entry in _state.entries)
00334:             {
00335:                 if (entry?.readingsHistory == null) continue;
00336:                 catalog.ApplyRetention("dose_ledger", entry.readingsHistory, out int pruned);
00337:                 total += pruned;
00338:             }
00339:             return total;
00340:         }
00341:     }
00342:
00343:     /// <summary>Mirror of the band enum for BookReading return values.</summary>
00344:     public enum DoseBandResult
00345:     {
00346:         NoEntry = -1,
00347:         Green = 0,
00348:         Amber = 1,
00349:         Red = 2,
00350:         Black = 3
00351:     }
00352: }
```

## `Assets/Ashfall.Core/DoseLedgerSave.cs` — 163 lines; 7,440 bytes; SHA-256 `8aa7f6abe7bef1e206f12773566725e3b38e4bde30bcdbbd12b001565d55bdd7`
Declaration index:
- 00024: public class DoseLedgerSave
- 00046: public class DoseLedgerSaveV1
- 00057: public static class DoseLedgerSaveCodec
- 00059: public static DoseLedgerSave Capture(
- 00080: public static string Encode(DoseLedgerSave save, IJsonSerializer json)
- 00094: public static DoseLedgerSave Decode(string jsonText, IJsonSerializer json)
- 00145: public static void Restore(
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core.YearOfAsh;
00004:
00005: namespace Ashfall.Core
00006: {
00007:     /// <summary>
00008:     /// ASHFALL: THE DOSE — cross-host save envelope for the four dose registers.
00009:     /// Same shape rules as the other expansion save envelopes: checksum recomputed
00010:     /// on encode, hard-reject on decode for tamper/checksumless/newer version.
00011:     ///
00012:     /// v2 adds the Dose questline section. Dose quest progress is owned here —
00013:     /// the Year of Ash envelope is no longer a second owner (its registration was
00014:     /// removed). v1 saves migrate with an empty quest section; a one-time adoption
00015:     /// helper folds any Dose quest progress a pre-v2 save carried inside the Year
00016:     /// of Ash envelope into the Dose envelope (see <see cref="DoseQuestMigration"/>).
00017:     ///
00018:     /// Migration validates the checksum over each version's FROZEN shape (see
00019:     /// <see cref="DoseLedgerSaveV1"/>) because <see cref="SaveChecksum"/> walks
00020:     /// public fields — validating a legacy payload against the current shape would
00021:     /// always mismatch.
00022:     /// </summary>
00023:     [Serializable]
00024:     public class DoseLedgerSave
00025:     {
00026:         public const int CurrentSaveVersion = 2;
00027:         public const int MigrationFromVersion = 1;
00028:
00029:         public int saveVersion = CurrentSaveVersion;
00030:         public int simDay;
00031:         public DoseLedgerSystemState doseLedger = new DoseLedgerSystemState();
00032:         public SickListSystemState sickList = new SickListSystemState();
00033:         public CohortSystemState cohort = new CohortSystemState();
00034:         public VoluntaryRegisterSystemState voluntaryRegister = new VoluntaryRegisterSystemState();
00035:         public QuestlineSystemState quests = new QuestlineSystemState();
00036:
00037:         public string Checksum = string.Empty;
00038:     }
00039:
00040:     /// <summary>
00041:     /// Frozen v1 envelope shape (no quest section). Kept so a v1 file on disk
00042:     /// validates against the field set it was actually hashed with.
00043:     /// Do not add fields here.
00044:     /// </summary>
00045:     [Serializable]
00046:     public class DoseLedgerSaveV1
00047:     {
00048:         public int saveVersion = 1;
00049:         public int simDay;
00050:         public DoseLedgerSystemState doseLedger = new DoseLedgerSystemState();
00051:         public SickListSystemState sickList = new SickListSystemState();
00052:         public CohortSystemState cohort = new CohortSystemState();
00053:         public VoluntaryRegisterSystemState voluntaryRegister = new VoluntaryRegisterSystemState();
00054:         public string Checksum = string.Empty;
00055:     }
00056:
00057:     public static class DoseLedgerSaveCodec
00058:     {
00059:         public static DoseLedgerSave Capture(
00060:             int simDay,
00061:             DoseLedgerSystem doseLedger,
00062:             SickListSystem sickList,
00063:             CohortSystem cohort,
00064:             VoluntaryRegisterSystem voluntaryRegister,
00065: QuestlineSystem? quests = null)
00066:         {
00067:             var save = new DoseLedgerSave
00068:             {
00069:                 simDay = simDay,
00070:                 doseLedger = doseLedger.CaptureState(),
00071:                 sickList = sickList.CaptureState(),
00072:                 cohort = cohort.CaptureState(),
00073:                 voluntaryRegister = voluntaryRegister.CaptureState(),
00074:                 quests = quests != null ? quests.CaptureState() : new QuestlineSystemState()
00075:             };
00076:             save.Checksum = SaveChecksum.Compute(save);
00077:             return save;
00078:         }
00079:
00080:         public static string Encode(DoseLedgerSave save, IJsonSerializer json)
00081:         {
00082:             if (save == null)
00083:                 throw new ArgumentNullException(nameof(save));
00084:             save.Checksum = SaveChecksum.Compute(save);
00085:             return json.Serialize(save);
00086:         }
00087:
00088:         /// <summary>
00089:         /// Decodes and migrates a Dose save. Legacy versions are parsed as their
00090:         /// FROZEN shapes so the checksum is verified over exactly the fields that
00091:         /// version wrote. Rejects: newer versions, too-old versions, checksumless
00092:         /// payloads, and tampered payloads.
00093:         /// </summary>
00094:         public static DoseLedgerSave Decode(string jsonText, IJsonSerializer json)
00095:         {
00096:             if (string.IsNullOrWhiteSpace(jsonText))
00097:                 throw new InvalidOperationException("DoseLedgerSave: empty save payload.");
00098:
00099:             DoseLedgerSave save;
00100:             try { save = json.Deserialize<DoseLedgerSave>(jsonText!); }
00101:             catch (Exception e)
00102:             {
00103:                 throw new InvalidOperationException("DoseLedgerSave: malformed save payload: " + e.Message, e);
00104:             }
00105:             if (save == null)
00106:                 throw new InvalidOperationException("DoseLedgerSave: empty save payload.");
00107:             if (save.saveVersion > DoseLedgerSave.CurrentSaveVersion)
00108:                 throw new InvalidOperationException(
00109:                     "DoseLedgerSave: saveVersion " + save.saveVersion + " is newer than supported.");
00110:             if (save.saveVersion < DoseLedgerSave.MigrationFromVersion)
00111:                 throw new InvalidOperationException("DoseLedgerSave: invalid saveVersion.");
00112:
00113:             if (save.saveVersion == 1)
00114:             {
00115:                 var v1 = json.Deserialize<DoseLedgerSaveV1>(jsonText);
00116:                 if (v1 == null)
00117:                     throw new InvalidOperationException("DoseLedgerSave: v1 deserialization returned null.");
00118:                 if (string.IsNullOrEmpty(v1.Checksum))
00119:                     throw new InvalidOperationException("DoseLedgerSave: save carries no checksum (truncated or tampered file).");
00120:                 if (!string.Equals(SaveChecksum.Compute(v1), v1.Checksum, StringComparison.Ordinal))
00121:                     throw new InvalidOperationException("DoseLedgerSave: checksum mismatch (corrupt or foreign save).");
00122:
00123:                 var migrated = new DoseLedgerSave
00124:                 {
00125:                     saveVersion = DoseLedgerSave.CurrentSaveVersion,
00126:                     simDay = v1.simDay,
00127:                     doseLedger = v1.doseLedger,
00128:                     sickList = v1.sickList,
00129:                     cohort = v1.cohort,
00130:                     voluntaryRegister = v1.voluntaryRegister
00131:                     // quests stays at its field initialiser (fresh default).
00132:                 };
00133:                 migrated.Checksum = SaveChecksum.Compute(migrated);
00134:                 return migrated;
00135:             }
00136:
00137:             if (string.IsNullOrEmpty(save.Checksum))
00138:                 throw new InvalidOperationException("DoseLedgerSave: save carries no checksum (truncated or tampered file).");
00139:             string actual = SaveChecksum.Compute(save);
00140:             if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
00141:                 throw new InvalidOperationException("DoseLedgerSave: checksum mismatch (corrupt or foreign save).");
00142:             return save;
00143:         }
00144:
00145:         public static void Restore(
00146:             DoseLedgerSave save,
00147:             DoseLedgerSystem doseLedger,
00148:             SickListSystem sickList,
00149:             CohortSystem cohort,
00150:             VoluntaryRegisterSystem voluntaryRegister,
00151: QuestlineSystem? quests = null)
00152:         {
00153:             if (save == null)
00154:                 throw new ArgumentNullException(nameof(save));
00155:             doseLedger?.RestoreState(save.doseLedger);
00156:             sickList?.RestoreState(save.sickList);
00157:             cohort?.RestoreState(save.cohort);
00158:             voluntaryRegister?.RestoreState(save.voluntaryRegister);
00159:             if (quests != null)
00160:                 quests.RestoreState(save.quests ?? new QuestlineSystemState());
00161:         }
00162:     }
00163: }
```

## `src/Host/DoseLedgerHostSession.cs` — 297 lines; 14,982 bytes; SHA-256 `0e8180d60ce3225f3722ee704d8c6d6d2bfcbaee29b8a0510044e536fb551150`
Declaration index:
- 00019: public sealed class DoseLedgerHostSession
- 00070: public static DoseLedgerHostSession Create(string dataDir, ILog log = null!, ICampaignRngManager? campaignRng = null)
- 00096: public DoseLedgerSave CaptureSave(int simDay) =>
- 00099: public void RestoreSave(DoseLedgerSave save) =>
- 00105: public void SealDemoSurvivors()
- 00118: public string StartCalibration(string deviceTag, int currentDay)
- 00126: public string StartCalibrationDemo(string deviceTag, int currentDay) => StartCalibration(deviceTag, currentDay);
- 00129: public string CompleteCalibration(string deviceTag, int currentDay)
- 00137: public string CompleteCalibrationDemo(string deviceTag, int currentDay) => CompleteCalibration(deviceTag, currentDay);
- 00140: public string ReplaceBattery(string deviceTag)
- 00146: public string ReplaceBatteryDemo(string deviceTag) => ReplaceBattery(deviceTag);
- 00149: public string ServiceSensor(string deviceTag)
- 00155: public string ServiceSensorDemo(string deviceTag) => ServiceSensor(deviceTag);
- 00158: public string CalibrationStatusLine(string deviceTag)
- 00168: public string ScribeReading(float nominalMsv, bool highEnergy)
- 00209: public string DiagnoseDemo(int band)
- 00216: public string BookDemoChild()
- 00225: public string SignDemoVolunteer()
- 00237: public int RegisterContentQuests(QuestlineSystem questSystem)
- 00251: public string ContentStatusLine()
- 00259: public string LedgerLine()
- 00276: public string DoseStatusLine()
- 00288: internal sealed class CoreSeededRng : ISeededRng
- 00293: public int Next(int min, int max) => _rng.Next(min, max);
- 00294: public float NextFloat() => _rng.NextFloat();
- 00295: public double NextDouble() => _rng.NextDouble();
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: #pragma warning disable CS8618
00004: using System.Text;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Random;
00007: using Ashfall.Core.Radiation;
00008: using Ashfall.Core.YearOfAsh;
00009: using Godot;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     /// <summary>
00014:     /// ASHFALL: THE DOSE — thin Godot-host session for the four dose registers.
00015:     /// Wraps DoseLedgerSystem, SickListSystem, CohortSystem and VoluntaryRegisterSystem.
00016:     /// No gameplay rules here — everything delegates to Ashfall.Core, following the
00017:     /// ExpansionHostSession pattern. Persistence via DoseLedgerSaveStore.
00018:     /// </summary>
00019:     public sealed class DoseLedgerHostSession
00020:     : HostSessionBase{
00021:         public const int DemoSeed = 1401;
00022:
00023:         public DoseLedgerSystem Ledger { get; }
00024:         public SickListSystem SickList { get; }
00025:         public CohortSystem Cohort { get; }
00026:         public VoluntaryRegisterSystem Voluntary { get; }
00027:         public DoseRegistersCatalog Registers { get; }
00028:         public DoseContentCatalog Content { get; }
00029:         public QuestlineSystem Quests { get; }
00030:         public DosimeterCalibrationSystem Calibration { get; }
00031:
00032:         private readonly SeededRng _rng;
00033:
00034:         /// <summary>Raised when any register changes (coalesced save dirty flag).</summary>
00035:         public DoseLedgerHostSession(
00036:             DoseLedgerSystem ledger = null!,
00037:             SickListSystem sickList = null!,
00038:             CohortSystem cohort = null!,
00039:             VoluntaryRegisterSystem voluntary = null!,
00040:             DoseRegistersCatalog registers = null!,
00041:             DoseContentCatalog content = null!,
00042:             QuestlineSystem quests = null!,
00043:             DosimeterCalibrationSystem calibration = null!,
00044:             ICampaignRngManager? campaignRng = null)
00045:         {
00046:             Ledger = ledger ?? new DoseLedgerSystem();
00047:             SickList = sickList ?? new SickListSystem();
00048:             Cohort = cohort ?? new CohortSystem();
00049:             Voluntary = voluntary ?? new VoluntaryRegisterSystem();
00050:             Registers = registers ?? new DoseRegistersCatalog();
00051:             Content = content ?? new DoseContentCatalog();
00052:             Quests = quests ?? new QuestlineSystem();
00053:             Calibration = calibration ?? new DosimeterCalibrationSystem();
00054:             int seed = campaignRng != null
00055:                 ? campaignRng.GetStream(CampaignStreamIds.Medical).DerivedBaseSeed
00056:                 : DemoSeed;
00057:             _rng = new SeededRng(seed);
00058:
00059:             // Persistence: any register mutation marks the save dirty.
00060:             Ledger.OnStateChanged += _ => RaiseStateChanged();
00061:             SickList.OnStateChanged += _ => RaiseStateChanged();
00062:             Cohort.OnStateChanged += _ => RaiseStateChanged();
00063:             Voluntary.OnStateChanged += _ => RaiseStateChanged();
00064:             Quests.OnQuestlineStarted += _ => RaiseStateChanged();
00065:             Quests.OnQuestChoiceTaken += _ => RaiseStateChanged();
00066:             Quests.OnQuestlineResolved += (_, _) => RaiseStateChanged();
00067:             Calibration.OnStateChanged += _ => RaiseStateChanged();
00068:         }
00069:
00070:         public static DoseLedgerHostSession Create(string dataDir, ILog log = null!, ICampaignRngManager? campaignRng = null)
00071:         {
00072:             CatalogLocator.UseInvariantCulture();
00073:             var registers = new DoseRegistersCatalog();
00074:             var content = new DoseContentCatalog();
00075:             var quests = new QuestlineSystem();
00076:             if (!string.IsNullOrEmpty(dataDir))
00077:             {
00078:                 var fileIO = new FileSystemIO();
00079:                 var serializer = new SystemTextJsonSerializer();
00080:                 registers = DoseRegistersCatalogLoader.Load(dataDir, fileIO, serializer);
00081:                 content = DoseContentCatalogLoader.Load(dataDir, fileIO, serializer);
00082:                 // Dose owns its quest runtime: register the four register quest
00083:                 // lines into the session's QuestlineSystem (persisted in the Dose
00084:                 // envelope, not the Year of Ash envelope).
00085:                 foreach (var q in content.quests)
00086:                 {
00087:                     if (q == null || string.IsNullOrEmpty(q.questlineId)) continue;
00088:                     quests.RegisterQuestline(q);
00089:                 }
00090:             }
00091:             return new DoseLedgerHostSession(registers: registers, content: content, quests: quests, campaignRng: campaignRng);
00092:         }
00093:
00094:         // ── Cross-host save ──────────────────────────────────────────
00095:
00096:         public DoseLedgerSave CaptureSave(int simDay) =>
00097:             DoseLedgerSaveCodec.Capture(simDay, Ledger, SickList, Cohort, Voluntary, Quests);
00098:
00099:         public void RestoreSave(DoseLedgerSave save) =>
00100:             DoseLedgerSaveCodec.Restore(save, Ledger, SickList, Cohort, Voluntary, Quests);
00101:
00102:         // ── Demo actions (drive the registers through real core APIs) ──
00103:
00104:         /// <summary>Assign the well-known demo survivors dosimeter tags so readings can be booked.</summary>
00105:         public void SealDemoSurvivors()
00106:         {
00107:             Ledger.AssignDosimeter("survivor_gunner_mikhail", "tag_1", 40f);
00108:             Ledger.AssignDosimeter("elena_vasquez", "tag_2", 15f);
00109:             Ledger.SetShieldingFactor("survivor_gunner_mikhail", 0.6f);
00110:             // Register calibration devices
00111:             Calibration.RegisterDevice("tag_1", "survivor_gunner_mikhail");
00112:             Calibration.RegisterDevice("tag_2", "elena_vasquez");
00113:         }
00114:
00115:         // ── Calibration production methods ──────────────────────────
00116:
00117:         /// <summary>Start calibration for a device.</summary>
00118:         public string StartCalibration(string deviceTag, int currentDay)
00119:         {
00120:             bool ok = Calibration.StartCalibration(deviceTag, currentDay);
00121:             return ok
00122:                 ? $"Calibration started for {deviceTag}. Duration: {DosimeterCalibrationSystem.CalibrationDurationDays} day(s)."
00123:                 : $"Cannot start calibration for {deviceTag} (battery low, sensor damaged, or station occupied).";
00124:         }
00125:
00126:         public string StartCalibrationDemo(string deviceTag, int currentDay) => StartCalibration(deviceTag, currentDay);
00127:
00128:         /// <summary>Complete calibration for a device (if duration elapsed).</summary>
00129:         public string CompleteCalibration(string deviceTag, int currentDay)
00130:         {
00131:             bool ok = Calibration.CompleteCalibration(deviceTag, currentDay);
00132:             if (!ok) return $"Calibration not ready for {deviceTag}.";
00133:             var device = Calibration.GetDevice(deviceTag);
00134:             return $"Calibration complete for {deviceTag}. Quality: {device?.calibrationQuality:F2}. Error band: ±{device?.errorBandMsv:F1} mSv.";
00135:         }
00136:
00137:         public string CompleteCalibrationDemo(string deviceTag, int currentDay) => CompleteCalibration(deviceTag, currentDay);
00138:
00139:         /// <summary>Replace battery in a device.</summary>
00140:         public string ReplaceBattery(string deviceTag)
00141:         {
00142:             bool ok = Calibration.ReplaceBattery(deviceTag);
00143:             return ok ? $"Battery replaced in {deviceTag}." : $"Unknown device: {deviceTag}.";
00144:         }
00145:
00146:         public string ReplaceBatteryDemo(string deviceTag) => ReplaceBattery(deviceTag);
00147:
00148:         /// <summary>Service sensor in a device.</summary>
00149:         public string ServiceSensor(string deviceTag)
00150:         {
00151:             bool ok = Calibration.ServiceSensor(deviceTag);
00152:             return ok ? $"Sensor serviced for {deviceTag}." : $"Unknown device: {deviceTag}.";
00153:         }
00154:
00155:         public string ServiceSensorDemo(string deviceTag) => ServiceSensor(deviceTag);
00156:
00157:         /// <summary>Get calibration status for a device.</summary>
00158:         public string CalibrationStatusLine(string deviceTag)
00159:         {
00160:             var device = Calibration.GetDevice(deviceTag);
00161:             if (device == null) return $"Unknown device: {deviceTag}";
00162:             return $"Device {deviceTag}: battery={device.batteryLevel:P0}, sensor={device.sensorCondition:P0}, " +
00163:                    $"quality={device.calibrationQuality:F2}, readings={device.readingsSinceCalibration}/{DosimeterCalibrationSystem.ReadingsPerCalibration}, " +
00164:                    $"error=±{device.errorBandMsv:F1} mSv, overdue={device.isOverdue}, calibrating={device.isStationOccupied}";
00165:         }
00166:
00167:         /// <summary>Book a nominal reading against the veteran; returns the band label.</summary>
00168:         public string ScribeReading(float nominalMsv, bool highEnergy)
00169:         {
00170:             var rng = new CoreSeededRng(_rng.Next(0, int.MaxValue));
00171:             int day = DayProvider != null ? DayProvider() : 40;
00172:             var outcome = BookConditionedExposure("survivor_gunner_mikhail", day, nominalMsv, "demo_scan", highEnergy, rng);
00173:             var windowNote = outcome.Item2 != null
00174:                 ? $" Fallout window {outcome.Item2.HazardType} x{outcome.Item2.Multiplier:0.00} applied on day {day}."
00175:                 : string.Empty;
00176:             return $"Booked {nominalMsv:0.##} mSv → band {outcome.Item1} (cumulative {Ledger.GetCumulative("survivor_gunner_mikhail"):F1}).{windowNote}";
00177:         }
00178:
00179:         /// <summary>
00180:         /// CORE-MECH W2/W4 — the single dose-booking conditioning site. Applies
00181:         /// the authored Year-of-Ash fallout window to the nominal reading, then
00182:         /// books through DoseLedgerSystem (which owns the seeded roll, anti-rad
00183:         /// timing, band edges, and cumulative totals). Returns the booked band
00184:         /// and the window that applied (null when clear) so callers can report
00185:         /// honestly. Shared by the register's own scribe path and CORE-MECH W4's
00186:         /// expedition breakdown exposure.
00187:         /// </summary>
00188:         public (string Band, Ashfall.Core.YearOfAsh.FalloutWindowDay? Window) BookConditionedExposure(
00189:             string survivorId, int day, float nominalMsV, string source, bool highEnergy, ISeededRng? rng)
00190:         {
00191:             float conditioned = nominalMsV;
00192:             var window = FalloutWindowProviderRef?.WindowFor(day);
00193:             if (window != null)
00194:                 conditioned = nominalMsV * window.Multiplier;
00195:
00196:             var band = Ledger.BookReading(
00197:                 survivorId, day, conditioned, source,
00198:                 highEnergy, antiRadBefore: false, antiRadAfter: false, rng);
00199:             return (band.ToString(), window);
00200:         }
00201:
00202:         /// <summary>CORE-MECH W2: live campaign day provider (wired by Main).</summary>
00203:         public Func<int>? DayProvider { get; set; }
00204:
00205:         /// <summary>CORE-MECH W2: authored Year-of-Ash fallout windows (wired by Main).</summary>
00206:         public FalloutWindowProvider? FalloutWindowProviderRef { get; set; }
00207:
00208:         /// <summary>Name the veteran into a Sick List band.</summary>
00209:         public string DiagnoseDemo(int band)
00210:         {
00211:             bool ok = SickList.Diagnose("survivor_gunner_mikhail", band, 40);
00212:             return ok ? $"Diagnosed veteran as band {band}." : "Diagnosis failed.";
00213:         }
00214:
00215:         /// <summary>Book a Cohort child (guess), then correct the baseline.</summary>
00216:         public string BookDemoChild()
00217:         {
00218:             bool booked = Cohort.BookChild("sv_cohort_demo", new[] { "survivor_gunner_mikhail" }, "low", 120, "told a kind number");
00219:             if (!booked) return "Child already booked or invalid guess.";
00220:             bool corrected = Cohort.CorrectBaseline("sv_cohort_demo", "medium");
00221:             return corrected ? "Cohort child booked (guess: low) and corrected to medium." : "Child booked.";
00222:         }
00223:
00224:         /// <summary>Sign a volunteer, complete it, and bank the dose.</summary>
00225:         public string SignDemoVolunteer()
00226:         {
00227:             bool signed = Voluntary.Volunteer("elena_vasquez", "vented reactor corridor", 44, "I walked the corridor before.");
00228:             if (!signed) return "Volunteer registration failed.";
00229:             bool done = Voluntary.CompleteVolunteer("elena_vasquez", "vented reactor corridor", 180f, 45);
00230:             return done ? "Elena volunteered, completed the corridor, banked 180 mSv." : "Volunteer task open.";
00231:         }
00232:
00233:         // ── Status lines ─────────────────────────────────────────────
00234:
00235:         /// <summary>Register the Dose quest lines into a QuestlineSystem (engine-agnostic
00236:         /// graph). The host owns the QuestlineSystem instance; this only ingests content.</summary>
00237:         public int RegisterContentQuests(QuestlineSystem questSystem)
00238:         {
00239:             if (questSystem == null || Content == null || Content.quests == null) return 0;
00240:             int count = 0;
00241:             foreach (var q in Content.quests)
00242:             {
00243:                 if (q == null || string.IsNullOrEmpty(q.questlineId)) continue;
00244:                 questSystem.RegisterQuestline(q);
00245:                 count++;
00246:             }
00247:             return count;
00248:         }
00249:
00250:         /// <summary>One-line summary of what the Expansion 07 content bundle adds.</summary>
00251:         public string ContentStatusLine()
00252:         {
00253:             return
00254:                 $"Dose content: {Content?.locations?.Count ?? 0} rooms, " +
00255:                 $"{Content?.items?.Count ?? 0} items, " +
00256:                 $"{Content?.quests?.Count ?? 0} quest lines";
00257:         }
00258:
00259:         public string LedgerLine()
00260:         {
00261:             var sb = new StringBuilder();
00262:             sb.Append("Dose Ledger: ").Append(Ledger.Entries.Count).Append(" tagged · ").
00263:                 Append(Ledger.State.readingsSinceLastCalibration).Append(" since calibration").
00264:                 Append(Ledger.State.calibrationOverdue ? " · OVERDUE" : "");
00265:             for (int i = 0; i < Ledger.Entries.Count; i++)
00266:             {
00267:                 var e = Ledger.Entries[i];
00268:                 if (e == null) continue;
00269:                 int band = DoseLedgerSystem.BandFor(e.cumulativeMsv);
00270:                 sb.Append("\n  ").Append(e.survivorId).Append(": ").Append(e.cumulativeMsv.ToString("F1")).
00271:                     Append(" mSv [band ").Append(band).Append("]");
00272:             }
00273:             return sb.ToString();
00274:         }
00275:
00276:         public string DoseStatusLine()
00277:         {
00278:             return
00279:                 $"Dose: {LedgerLine()}\n" +
00280:                 $"Sick: {SickList.Bands.Count} named\n" +
00281:                 $"Cohort: {Cohort.Children.Count} booked\n" +
00282:                 $"Voluntary: {Voluntary.Entries.Count} signed";
00283:         }
00284:     }
00285:
00286:     /// <summary>A11: ISeededRng adapter delegates to the core SeededRng
00287:     /// (deterministic xorshift64) — no System.Random in decision paths.</summary>
00288:     internal sealed class CoreSeededRng : ISeededRng
00289:     {
00290:         private readonly SeededRng _rng;
00291:         public int Seed { get; }
00292:         public CoreSeededRng(int seed) { Seed = seed; _rng = new SeededRng(seed); }
00293:         public int Next(int min, int max) => _rng.Next(min, max);
00294:         public float NextFloat() => _rng.NextFloat();
00295:         public double NextDouble() => _rng.NextDouble();
00296:     }
00297: }
```

## `src/Host/DoseLedgerSaveStore.cs` — 58 lines; 2,915 bytes; SHA-256 `5d16d85d0bc7ebde0fc2f9900a147ace6ddae49156587c8185e7f289a5c78703`
Declaration index:
- 00020: public static class DoseLedgerSaveStore
- 00036: public static string TryCaptureDirect(DoseLedgerSave state) => s_store.CaptureBare(state);
- 00039: public static DoseLedgerSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
- 00042: public static string TryCapture(DoseLedgerSave state) => s_store.CaptureBare(state);
- 00045: public static DoseLedgerSave? TryRestore(string json) => s_store.RestoreBare(json);
- 00048: public static bool TrySave(DoseLedgerSave save, string pathOverride = null!) =>
- 00052: public static DoseLedgerSave? TryLoad(string pathOverride = null!) =>
- 00056: public static string TryCapturePersisted(DoseLedgerSave save) => s_store.CapturePersisted(save);
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Save Store : DoseLedgerSaveStore
00004: // Core State : Ashfall.Core.DoseLedgerSave
00005: // Host Caller: Main.Holdfast, Main.Phase0 / DoseLedgerHostSession
00006: // Purpose    : Cumulative radiation dose ledger, threshold brackets, and survivor exposure logs
00007: // ============================================================================
00008: using Ashfall.Core;
00009: using Ashfall.Core.Save;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     /// <summary>
00014:     /// Persists <see cref="DoseLedgerSave"/> as JSON under
00015:     /// user://dose_ledger_save.json — thin façade over the Core
00016:     /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Shape and
00017:     /// validation live in <see cref="DoseLedgerSaveCodec"/>; path resolution,
00018:     /// atomic write, and error handling live in the service.
00019:     /// </summary>
00020:     public static class DoseLedgerSaveStore
00021:     {
00022:         public const string FileName = "dose_ledger_save.json";
00023:         public const string SectionName = "dose_ledger";
00024:
00025:         private static readonly SaveStore<DoseLedgerSave> s_store = SaveStoreHub.FromCodec(
00026:             FileName,
00027:             nameof(DoseLedgerSaveStore),
00028:             (save, json) => DoseLedgerSaveCodec.Encode(save, json),
00029:             (raw, json) => DoseLedgerSaveCodec.Decode(raw, json));
00030:
00031:         public static string SavePath => s_store.SavePath;
00032:
00033:         public static bool Exists => s_store.Exists();
00034:
00035:         /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
00036:         public static string TryCaptureDirect(DoseLedgerSave state) => s_store.CaptureBare(state);
00037:
00038:         /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
00039:         public static DoseLedgerSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
00040:
00041:         /// <summary>Capture state to JSON without writing to disk.</summary>
00042:         public static string TryCapture(DoseLedgerSave state) => s_store.CaptureBare(state);
00043:
00044:         /// <summary>Restore state from JSON without reading from disk.</summary>
00045:         public static DoseLedgerSave? TryRestore(string json) => s_store.RestoreBare(json);
00046:
00047:         /// <summary>Writes through the codec (checksum stamped). Returns false on failure.</summary>
00048:         public static bool TrySave(DoseLedgerSave save, string pathOverride = null!) =>
00049:             s_store.TrySave(save, pathOverride);
00050:
00051:         /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
00052:         public static DoseLedgerSave? TryLoad(string pathOverride = null!) =>
00053:             s_store.TryLoad(pathOverride);
00054:
00055:         /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
00056:         public static string TryCapturePersisted(DoseLedgerSave save) => s_store.CapturePersisted(save);
00057:     }
00058: }
```

## `src/Main.Phase0.cs` — 487 lines; 20,350 bytes; SHA-256 `17e1d2026b7034eec7ffe613a720851aed49bcb82b064c25d44ebe1b80c7f8dd`
Declaration index:
- 00032: public partial class Main : Control
- 00042: private void SetupPhantom()
- 00080: private void OnPhantomScavengeClicked()
- 00086: private void OnPhantomTickClicked()
- 00092: private void SavePhantomMemory()
- 00099: private void SetupPhase0()
- 00258: private void SavePhase0()
- 00268: private void FlushPhase0IfDirty()
- 00279: private void OnCraftCompletedForSpecialty(Recipe recipe, string crafterId)
- 00300: private string ResolveSurvivorProfessionId(string survivorId, string? definitionId = null)
- 00316: private string AutoAssignSpecialtyCrafter(string itemId)
- 00340: private void OnPhase0ScavengeClicked()
- 00346: private void OnPhase0NoiseClicked()
- 00352: private void OnPhase0CraftClicked()
- 00358: private void OnPhase0TickClicked()
- 00364: private void SetupDoseLedger()
- 00414: private void OnDoseRegisterClicked()
- 00420: private void OnDoseSealClicked()
- 00429: private void OnDoseScribeClicked()
- 00438: private void OnDoseDiagnoseClicked()
- 00447: private void OnDoseCohortClicked()
- 00456: private void OnDoseVolunteerClicked()
- 00465: private void SaveDoseLedger()
- 00476: private void FlushDoseLedgerIfDirty()
- 00481: private void ClosePhase0Panel()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Globalization;
00005: using System.IO;
00006: using System.Linq;
00007: using System.Collections.Generic;
00008: using AtomicWar.Journal;
00009: using Ashfall.Core;
00010: using Ashfall.Core.Campaign;
00011: using Ashfall.Core.Crafting;
00012: using Ashfall.Core.Economy;
00013: using Ashfall.Core.Expeditions;
00014: using Ashfall.Core.Foundry;
00015: using Ashfall.Core.Inventory;
00016: using Ashfall.Core.Journal;
00017: using Ashfall.Core.Muster;
00018: using Ashfall.Core.YearOfAsh;
00019: using Ashfall.Core.Radio;
00020: using Ashfall.Core.Survivors;
00021: using AtomicWar.GodotApp.Economy;
00022: using AtomicWar.GodotApp.YearOfAsh;
00023: using AtomicWar.GodotApp.Muster;
00024: using AtomicWar.GodotApp.Dose;
00025: using AtomicWar.GodotApp.UtilityAI;
00026: using AtomicWar.GodotApp.Radio;
00027: using AtomicWar.GodotApp.Audio;
00028: using AtomicWar.GodotApp.UI;
00029:
00030: namespace AtomicWar.GodotApp
00031: {
00032:     public partial class Main : Control
00033:     {
00034:         // ── Phase 0 / Dose fields (GAP-ARCH-01 Phase 1) ──
00035:         private PhantomMemoryHostSession _phantomMemory = null!;
00036:         private Phase0HostSession _phase0 = null!;
00037:         private bool _phase0Dirty;
00038:         private DoseLedgerHostSession _doseLedger = null!;
00039:         private bool _doseLedgerDirty;
00040:         private DoseRegisterSurface _doseSurface = null!;
00041:
00042:         private void SetupPhantom()
00043:         {
00044:             if (_phantomMemory != null) return;
00045:             SetupCampaignDay();
00046:             var rng = _campaignDay?.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Psychology).Rng;
00047:             _phantomMemory = PhantomMemoryHostSession.Create(_dataDir, rng);
00048:             _phantomMemory.StateChanged += () => SavePhantomMemory();
00049:             SetupSurvivors();
00050:             SetupEnrichment();
00051:             if (_survivors != null)
00052:             {
00053:                 // Enrichment is the explicit background authority when present;
00054:                 // profession mapping remains the fallback for roster entries
00055:                 // that do not have an enrichment row.
00056:                 _phantomMemory.BindSurvivors(_survivors, _enrichment);
00057:             }
00058:             SetupInventory();
00059:             if (_inventory != null)
00060:             {
00061:                 _phantomMemory.BindInventory(_inventory);
00062:             }
00063:             _phantomMemory.Engine.OnPhantomMemoryResolved += (svId, itemId, isMotivation, moraleDelta, guiltDelta) =>
00064:             {
00065:                 var sv = _survivors?.Find(svId);
00066:                 if (sv != null && moraleDelta != 0f)
00067:                 {
00068:                     _survivors!.Needs.Modify(sv, NeedKind.Morale, moraleDelta);
00069:                 }
00070:             };
00071:
00072:             var save = PhantomMemorySaveStore.TryLoad();
00073:             if (save != null)
00074:             {
00075:                 _phantomMemory.RestoreSave(save);
00076:                 GD.Print("[Ashfall Godot] Phantom Memory state restored.");
00077:             }
00078:         }
00079:
00080:         private void OnPhantomScavengeClicked()
00081:         {
00082:             SetupPhantom();
00083:             _statusLabel.Text = _phantomMemory.ScavengeItem("survivor_gunner_mikhail", "dog_tags");
00084:         }
00085:
00086:         private void OnPhantomTickClicked()
00087:         {
00088:             SetupPhantom();
00089:             _statusLabel.Text = _phantomMemory.TickDemo();
00090:         }
00091:
00092:         private void SavePhantomMemory()
00093:         {
00094:             if (_phantomMemory == null) return;
00095:             if (CaptureSection("phantom_memory", PhantomMemorySaveStore.TryCapturePersisted(_phantomMemory.CaptureSave())))
00096:                 GD.Print("[Ashfall Godot] Phantom Memory save written.");
00097:         }
00098:
00099:         private void SetupPhase0()
00100:         {
00101:             if (_phase0 != null) return;
00102:             SetupMedical();
00103:             // Task #133: share the MedicalHostSession-owned dependency ledger so
00104:             // there is exactly one chem-dep authority, and Phase-0 does not tick it.
00105:             _phase0 = new Phase0HostSession(dependency: _medical.Engine);
00106:             _phase0.StateChanged += () => _phase0Dirty = true;
00107:             // Feed the specialty catalog — without it the wired specialty loop
00108:             // runs patternless and mastery can never progress.
00109:             _phase0.LoadTradeSpecialties(_dataDir);
00110:
00111:             // ── Wire every Phase-0 effect to the REAL gameplay consumer ──
00112:             SetupSurvivors();
00113:             SetupJournal();
00114:             SetupCrafting();
00115:             SetupExpeditions();
00116:             SetupMedical();
00117:             SetupEnrichment();
00118:
00119:             // Recorded craft-attribution contract: crafting owns recipe completion,
00120:             // this host supplies the survivor, profession, and result item. Both
00121:             // _phase0 and _crafting reset through the lifecycle registry, so this
00122:             // subscription is rebuilt against the fresh engine on a new campaign.
00123:             _crafting.Engine.OnCraftCompleted += OnCraftCompletedForSpecialty;
00124:
00125:             _phase0.Consumers = new Phase0EffectConsumers(
00126:                 applyMoraleDelta: (sv, delta) =>
00127:                 {
00128:                     var survivor = _survivors.Find(sv);
00129:                     if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
00130:                 },
00131:                 applyHealthDelta: (sv, delta) =>
00132:                 {
00133:                     var survivor = _survivors.Find(sv);
00134:                     if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Health, delta);
00135:                 },
00136:                 applyFatigueDelta: (sv, delta) =>
00137:                 {
00138:                     var survivor = _survivors.Find(sv);
00139:                     if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Fatigue, delta);
00140:                 },
00141:                 applyShelterMoraleDelta: delta =>
00142:                 {
00143:                     for (int i = 0; i < _survivors.RosterState.Count; i++)
00144:                     {
00145:                         var s = _survivors.RosterState[i];
00146:                         if (s != null && s.IsAliveState)
00147:                             _survivors.Needs.Modify(s, NeedKind.Morale, delta);
00148:                     }
00149:                 },
00150:                 applyWorkEfficiencyMultiplier: (sv, mult) =>
00151:                 {
00152:                     if (_crafting == null) return;
00153:                     _crafting.Engine.SetCrafterCraftTimeMultiplier(id =>
00154:                         id == sv ? MathfCompat.Max(0.1f, 1f / MathfCompat.Max(0.1f, mult)) : 1f);
00155:                 },
00156:                 applyCraftingPenaltyFactor: (sv, factor) =>
00157:                 {
00158:                     if (_crafting == null) return;
00159:                     _crafting.Engine.SetCrafterCraftTimeMultiplier(id =>
00160:                         id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
00161:                 },
00162:                 applyCombatPenaltyFactor: (sv, factor) =>
00163:                 {
00164:                     if (_expeditions == null) return;
00165:                     _expeditions.Engine.SetStaminaDrainMultiplier(id =>
00166:                         id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
00167:                 },
00168:                 applyStaminaDrainMultiplier: (sv, factor) =>
00169:                 {
00170:                     if (_expeditions == null) return;
00171:                     _expeditions.Engine.SetStaminaDrainMultiplier(id =>
00172:                         id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
00173:                 },
00174:                 fireNarrativeEvent: (narrativeId, sv) =>
00175:                 {
00176:                     int day = _holdfastRuntime?.Day ?? _simDay;
00177:                     string sourceId = $"{narrativeId}_{sv}_{day}";
00178:
00179:                     // events.json is the prose authority for narrative event ids.
00180:                     // Authored ids dispatch through the same catalog seam the
00181:                     // wildlife bycatch beat uses (journal + codex unlock + HUD).
00182:                     SetupEventsHost();
00183:                     if (_eventsHost != null
00184:                         && _eventsHost.TryGetEvent(narrativeId, out var authored)
00185:                         && authored != null
00186:                         && !string.IsNullOrWhiteSpace(authored.BodyText))
00187:                     {
00188:                         SetupEventAdapter();
00189:                         _hostEventAdapter?.DispatchCatalogEvent(authored.Id, authored.BodyText, day, sourceId);
00190:                         return;
00191:                     }
00192:
00193:                     // Unauthored id (narrative_final_wish_completed has no events.json
00194:                     // row): keep the beat visible instead of dropping it silently.
00195:                     GD.PushWarning($"[Phase0] narrative event '{narrativeId}' is not authored in events.json; writing placeholder journal entry.");
00196:                     _journal.TryAddRawEntry(
00197:                         sourceId,
00198:                         $"{sv}: {narrativeId.Replace('_', ' ')}.",
00199:                         author: null!,
00200:                         day: day);
00201:                 },
00202:                 grantChronicIllness: (sv, afflictionId) =>
00203:                 {
00204:                     var rad = _survivors.RadStateFor(sv);
00205:                     if (rad != null && !rad.HasChronicIllness)
00206:                     {
00207:                         rad.HasChronicIllness = true;
00208:                         SaveSurvivors();
00209:                     }
00210:                 },
00211:                 resetRadiationDose: sv =>
00212:                 {
00213:                     var rad = _survivors.RadStateFor(sv);
00214:                     if (rad != null) _survivors.Radiation.SetDose(rad, 0f);
00215:                 },
00216:                 applyWorkRefusalHours: null);
00217:             _phase0.ValidateConsumers();
00218:
00219:             // Environment signals from the real world/shelter hosts.
00220:             _phase0.CurrentDay = _simDay;
00221:             _phase0.GetFilterHealth = () =>
00222:             {
00223:                 var filter = _expansions?.Waystation?.State != null
00224:                     ? _expansions.Waystation.State.filterHealth : 100f;
00225:                 return filter;
00226:             };
00227:             // Host flags: updated each tick from the real world/shelter state.
00228:             _phase0.IsInFalloutStorm = _world != null && _world.Weather.Current == Ashfall.Core.WeatherKind.FalloutStorm;
00229:             _phase0.IsNightTime = _world != null && _world.Weather.Current == Ashfall.Core.WeatherKind.BlackRain;
00230:
00231:             var ids = new System.Collections.Generic.List<string>();
00232:             for (int i = 0; i < _survivors.RosterState.Count; i++)
00233:             {
00234:                 var s = _survivors.RosterState[i];
00235:                 if (s != null && s.IsAliveState) ids.Add(s.Id);
00236:             }
00237:             _phase0.RegisterSurvivors(ids);
00238:
00239:             if (_shelterAssignment != null)
00240:             {
00241:                 _phase0.BindShelterAssignment(_shelterAssignment.System);
00242:             }
00243:
00244:             var save = Phase0SaveStore.TryLoad();
00245:             if (save != null)
00246:             {
00247:                 _phase0.RestoreSave(save);
00248:                 _phase0Dirty = false; // restore just raised state-change events
00249:                 GD.Print("[Ashfall Godot] Phase-0 effects restored.");
00250:             }
00251:
00252:             // Bind the authored final-wish catalog so terminal prognoses draw from a
00253:             // per-archetype pool and the panel can surface authored text. Safe to run
00254:             // after restore: it only affects future DeclareTerminalPrognosis calls.
00255:             _phase0.LoadFinalWishCatalog(_dataDir);
00256:         }
00257:
00258:         private void SavePhase0()
00259:         {
00260:             if (_phase0 == null) return;
00261:             if (CaptureSection("phase0", Phase0SaveStore.TryCapturePersisted(_phase0.CaptureSave())))
00262:             {
00263:                 _phase0Dirty = false;
00264:                 GD.Print("[Ashfall Godot] Phase-0 effects save written.");
00265:             }
00266:         }
00267:
00268:         private void FlushPhase0IfDirty()
00269:         {
00270:             if (_phase0Dirty) SavePhase0();
00271:         }
00272:
00273:         /// <summary>
00274:         /// Bridge a completed production craft into trade specialty progression.
00275:         /// A player-assigned crafter is authoritative; an unassigned shelter craft
00276:         /// falls back to a living survivor whose trade actually covers the item.
00277:         /// A survivor whose profession resolves to no specialty has no tree to advance.
00278:         /// </summary>
00279:         private void OnCraftCompletedForSpecialty(Recipe recipe, string crafterId)
00280:         {
00281:             if (_phase0 == null || recipe?.result == null) return;
00282:
00283:             string itemId = recipe.result.id;
00284:             string survivorId = string.IsNullOrWhiteSpace(crafterId)
00285:                 ? AutoAssignSpecialtyCrafter(itemId)
00286:                 : crafterId;
00287:             if (string.IsNullOrEmpty(survivorId)) return;
00288:
00289:             string professionId = ResolveSurvivorProfessionId(survivorId);
00290:             if (string.IsNullOrEmpty(professionId)) return;
00291:
00292:             _phase0.CraftItem(survivorId, professionId, itemId);
00293:         }
00294:
00295:         /// <summary>
00296:         /// Resolve a survivor's trade specialty id. An authored pre_war_profession_id
00297:         /// wins; otherwise the roster profession label is matched against the
00298:         /// authored profession_aliases in trade_specialties.json.
00299:         /// </summary>
00300:         private string ResolveSurvivorProfessionId(string survivorId, string? definitionId = null)
00301:         {
00302:             if (string.IsNullOrEmpty(survivorId)) return string.Empty;
00303:             SetupEnrichment();
00304:             string explicitId = _enrichment?.GetSurvivorFields(survivorId)?.pre_war_profession_id ?? string.Empty;
00305:             string label = _survivors?.Roster?.FindDefinition(definitionId ?? survivorId)?.profession ?? string.Empty;
00306:             return TradeSpecialtySystem.ResolveProfessionId(explicitId, label);
00307:         }
00308:
00309:         /// <summary>
00310:         /// Attribute an unassigned craft to a living survivor whose trade covers the
00311:         /// item. Deterministic: candidates are ordinal-sorted before the forked
00312:         /// campaign RNG stream picks one, so a replay credits the same survivor and
00313:         /// no wall-clock or hash-iteration order is involved. Empty when nobody's
00314:         /// trade matches, which leaves the craft advancing no specialty.
00315:         /// </summary>
00316:         private string AutoAssignSpecialtyCrafter(string itemId)
00317:         {
00318:             var roster = _survivors?.Roster;
00319:             if (roster == null || string.IsNullOrEmpty(itemId)) return string.Empty;
00320:
00321:             var candidates = new List<string>();
00322:             for (int i = 0; i < roster.Roster.Count; i++)
00323:             {
00324:                 var entry = roster.Roster[i];
00325:                 if (entry == null || !entry.isAlive || string.IsNullOrEmpty(entry.survivorId)) continue;
00326:                 string professionId = ResolveSurvivorProfessionId(entry.survivorId, entry.definitionId);
00327:                 if (string.IsNullOrEmpty(professionId)) continue;
00328:                 if (!TradeSpecialtySystem.ProfessionMatchesItem(professionId, itemId)) continue;
00329:                 candidates.Add(entry.survivorId);
00330:             }
00331:
00332:             if (candidates.Count == 0) return string.Empty;
00333:             if (candidates.Count == 1) return candidates[0];
00334:
00335:             candidates.Sort(StringComparer.Ordinal);
00336:             var rng = _campaignDay?.Rng?.Fork("trade_specialty_attribution");
00337:             return rng != null ? candidates[rng.Next(0, candidates.Count)] : candidates[0];
00338:         }
00339:
00340:         private void OnPhase0ScavengeClicked()
00341:         {
00342:             SetupPhase0();
00343:             _statusLabel.Text = _phase0.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
00344:         }
00345:
00346:         private void OnPhase0NoiseClicked()
00347:         {
00348:             SetupPhase0();
00349:             _statusLabel.Text = _phase0.RaiseNoise("siren");
00350:         }
00351:
00352:         private void OnPhase0CraftClicked()
00353:         {
00354:             SetupPhase0();
00355:             _statusLabel.Text = _phase0.CraftItem("elena_vasquez", "machinist", "wrench_standard");
00356:         }
00357:
00358:         private void OnPhase0TickClicked()
00359:         {
00360:             SetupPhase0();
00361:             _statusLabel.Text = _phase0.TickHour(6f);
00362:         }
00363:
00364:         private void SetupDoseLedger()
00365:         {
00366:             if (_doseLedger != null) return;
00367:             SetupCampaignDay();
00368:             _doseLedger = DoseLedgerHostSession.Create(_dataDir, campaignRng: _campaignDay.Rng);
00369:             _doseLedger.StateChanged += () => _doseLedgerDirty = true;
00370:
00371:             // CORE-MECH W2: bind the live campaign day and the authored Year-of-Ash
00372:             // fallout windows so radiation bookings are conditioned by the season.
00373:             // The provider is read-only and pure; the dose ledger keeps owning every
00374:             // reading rule (AA.2 receiver contract).
00375:             _doseLedger.DayProvider = () => _simDay;
00376:             try
00377:             {
00378:                 var catalogPath = CatalogPath.ResolveCatalog("year_of_ash_events.json");
00379:                 var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
00380:                 if (catalogIo.FileExists(catalogPath))
00381:                 {
00382:                     var yoaEvents = YearOfAshCatalogLoader.LoadEvents(
00383:                         CatalogPath.ResolveDataDir(), catalogIo, new SystemTextJsonSerializer());
00384:                     _doseLedger.FalloutWindowProviderRef = new FalloutWindowProvider(yoaEvents);
00385:                 }
00386:             }
00387:             catch (Exception ex)
00388:             {
00389:                 // Fail-closed: no calendar ⇒ neutral multiplier (1.0), never a crash
00390:                 // and never a silently reduced exposure.
00391:                 GD.Print("[Ashfall Godot] Dose fallout windows unavailable: " + ex.Message);
00392:             }
00393:
00394:             var save = DoseLedgerSaveStore.TryLoad();
00395:             if (save != null)
00396:             {
00397:                 _doseLedger.RestoreSave(save);
00398:                 _doseLedgerDirty = false; // restore just raised state-change events
00399:                 GD.Print("[Ashfall Godot] Dose Ledger state restored.");
00400:             }
00401:
00402:             if (_doseSurface == null && _rightColumn != null)
00403:             {
00404:                 _doseSurface = new DoseRegisterSurface();
00405:                 _rightColumn.AddChild(_doseSurface);
00406:             }
00407:             if (_doseSurface != null)
00408:             {
00409:                 _doseSurface.BindSession(_doseLedger);
00410:                 _doseSurface.RefreshView();
00411:             }
00412:         }
00413:
00414:         private void OnDoseRegisterClicked()
00415:         {
00416:             SetupDoseLedger();
00417:             _statusLabel.Text = "The Dose Register is open. Four tabs, four people who keep books.";
00418:         }
00419:
00420:         private void OnDoseSealClicked()
00421:         {
00422:             SetupDoseLedger();
00423:             _doseLedger.SealDemoSurvivors();
00424:             _statusLabel.Text = "Dosimeters sealed: Gunner Mikhail (tag_1), Elena Vasquez (tag_2).";
00425:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00426:             FlushDoseLedgerIfDirty();
00427:         }
00428:
00429:         private void OnDoseScribeClicked()
00430:         {
00431:             SetupDoseLedger();
00432:             string result = _doseLedger.ScribeReading(180f, highEnergy: false);
00433:             _statusLabel.Text = result;
00434:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00435:             FlushDoseLedgerIfDirty();
00436:         }
00437:
00438:         private void OnDoseDiagnoseClicked()
00439:         {
00440:             SetupDoseLedger();
00441:             string result = _doseLedger.DiagnoseDemo(DoseLedgerSystem.BandRed);
00442:             _statusLabel.Text = result;
00443:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00444:             FlushDoseLedgerIfDirty();
00445:         }
00446:
00447:         private void OnDoseCohortClicked()
00448:         {
00449:             SetupDoseLedger();
00450:             string result = _doseLedger.BookDemoChild();
00451:             _statusLabel.Text = result;
00452:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00453:             FlushDoseLedgerIfDirty();
00454:         }
00455:
00456:         private void OnDoseVolunteerClicked()
00457:         {
00458:             SetupDoseLedger();
00459:             string result = _doseLedger.SignDemoVolunteer();
00460:             _statusLabel.Text = result;
00461:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00462:             FlushDoseLedgerIfDirty();
00463:         }
00464:
00465:         private void SaveDoseLedger()
00466:         {
00467:             if (_doseLedger == null) return;
00468:             int day = _core != null ? _core.Clock.Day : _simDay;
00469:             if (CaptureSection("dose_ledger", DoseLedgerSaveStore.TryCapturePersisted(_doseLedger.CaptureSave(day))))
00470:             {
00471:                 _doseLedgerDirty = false;
00472:                 GD.Print($"[Ashfall Godot] Dose Ledger save written (day {day}).");
00473:             }
00474:         }
00475:
00476:         private void FlushDoseLedgerIfDirty()
00477:         {
00478:             if (_doseLedgerDirty) SaveDoseLedger();
00479:         }
00480:
00481:         private void ClosePhase0Panel()
00482:         {
00483:             _phase0Panel.Visible = false;
00484:         }
00485:
00486:     }
00487: }
```

## `src/Main.Medical.cs` — 651 lines; 31,882 bytes; SHA-256 `56732cc5acbf9c9129f4159be2c23cb6164bf13e1a3b6b2521152b10fa90cf39`
Declaration index:
- 00032: public partial class Main : Control
- 00039: private string DiseaseStatusLine()
- 00052: private void FlushMedicalIfDirty()
- 00057: private void SetupMedical()
- 00098: public IReadOnlyList<string> GetActiveAfflictionIds(string survivorId)
- 00189: public bool IsQuestBlockedForSurvivor(string survivorId, string questTag, out List<string> blockingAfflictions)
- 00208: public IReadOnlyList<string> GetUnlockedQuestsForSurvivor(string survivorId)
- 00221: private void EnsureMedicalPipeline()
- 00320: private void MigrateDiseaseSuspicions(Ashfall.Core.Medical.MedicalPipelineCoordinator pipeline)
- 00343: private Ashfall.Core.Medical.PatientAvailability ResolvePatientAvailability(string survivorId)
- 00351: private string GetRadiationPhaseName(string survivorId)
- 00364: private void MigrateLegacyMedicalDiagnoses()
- 00382: private void SaveMedicalPipeline()
- 00391: private void SaveMedical()
- 00404: private void SetupMedicalWard()
- 00466: private void SaveMedicalWard()
- 00503: private void LoadMedicalWard()
- 00521: private void SaveDisease()
- 00534: private void SetupDisease()
- 00645: private void CloseMedicalPanel()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Globalization;
00005: using System.IO;
00006: using System.Linq;
00007: using System.Collections.Generic;
00008: using AtomicWar.Journal;
00009: using Ashfall.Core;
00010: using Ashfall.Core.Campaign;
00011: using Ashfall.Core.Economy;
00012: using Ashfall.Core.Expeditions;
00013: using Ashfall.Core.Foundry;
00014: using Ashfall.Core.Inventory;
00015: using Ashfall.Core.Journal;
00016: using Ashfall.Core.Muster;
00017: using Ashfall.Core.YearOfAsh;
00018: using Ashfall.Core.Radio;
00019: using Ashfall.Core.Survivors;
00020: using Ashfall.Core.Feedback;
00021: using AtomicWar.GodotApp.Economy;
00022: using AtomicWar.GodotApp.YearOfAsh;
00023: using AtomicWar.GodotApp.Muster;
00024: using AtomicWar.GodotApp.Dose;
00025: using AtomicWar.GodotApp.UtilityAI;
00026: using AtomicWar.GodotApp.Radio;
00027: using AtomicWar.GodotApp.Audio;
00028: using AtomicWar.GodotApp.UI;
00029:
00030: namespace AtomicWar.GodotApp
00031: {
00032:     public partial class Main : Control
00033:     {
00034:         // ── Medical fields (GAP-ARCH-01 Phase 1) ──
00035:         private MedicalHostSession _medical = null!;
00036:         private bool _medicalDirty;
00037:         private AtomicWar.GodotApp.DiseaseHostSession _disease = null!;
00038:
00039:         private string DiseaseStatusLine()
00040:         {
00041:             if (_expansions?.Disease == null) return "DISEASE WARD: offline";
00042:             if (_disease == null) SetupDisease();
00043:             if (_disease == null) return "DISEASE WARD: offline";
00044:             var s = _disease.Engine.GetSnapshot();
00045:             return $"——— DISEASE WARD ———\n" +
00046:                 $"infections {s.total_infected} · quarantined {s.total_quarantined} · " +
00047:                 $"outbreaks {s.total_outbreaks} (prevented {s.total_outbreaks_prevented}) · " +
00048:                 $"recovered {s.total_recovered} · deaths {s.total_deaths}" +
00049:                 (s.total_contagious > 0 ? "  ★ " + s.total_contagious + " CONTAGIOUS UNISOLATED" : "");
00050:         }
00051:
00052:         private void FlushMedicalIfDirty()
00053:         {
00054:             if (_medicalDirty) SaveMedical();
00055:         }
00056:
00057:         private void SetupMedical()
00058:         {
00059:             if (_medical != null) return;
00060:             _medical = MedicalHostSession.Create(_dataDir);
00061:             _medical.StateChanged += () =>
00062:             {
00063:                 _medicalDirty = true;
00064:                 _medicalPanel?.RefreshView();
00065:             };
00066:
00067:             // Plan 60 / D6 — a vigil the player keeps is care, and care must be
00068:             // recorded where the campaign remembers it: the consequence ledger already
00069:             // rides the save, so no new persistence is introduced. The names worth
00070:             // reciting are the dead this holdfast has already kept.
00071:             SetupMemorial();
00072:             _medical.BindVigilContext(
00073:                 () => _simDay,
00074:                 _consequenceLedger,
00075:                 () =>
00076:                 {
00077:                     var remembered = new List<string>();
00078:                     if (_memorial?.Entries != null)
00079:                     {
00080:                         for (int i = 0; i < _memorial.Entries.Count && remembered.Count < 6; i++)
00081:                         {
00082:                             var entry = _memorial.Entries[i];
00083:                             if (entry == null || string.IsNullOrEmpty(entry.SurvivorId)) continue;
00084:                             remembered.Add(FormatSurvivorName(entry.SurvivorId));
00085:                         }
00086:                     }
00087:                     return remembered;
00088:                 });
00089:
00090:             GD.Print("[Ashfall Godot] Medical host ready.");
00091:         }
00092:
00093:         /// <summary>
00094:         /// Plan 143: Gathers all canonical active medical affliction IDs for a survivor.
00095:         /// Queries the active pipeline handlers (respiratory, radiation, health deficit,
00096:         /// chemical dependency, psychology/trauma, diseases) and physiological limb conditions.
00097:         /// </summary>
00098:         public IReadOnlyList<string> GetActiveAfflictionIds(string survivorId)
00099:         {
00100:             var activeIds = new List<string>();
00101:             if (string.IsNullOrEmpty(survivorId)) return activeIds;
00102:
00103:             SetupMedical();
00104:             EnsureMedicalPipeline();
00105:
00106:             // 1. Pipeline active episodes
00107:             if (_medical?.Pipeline != null && Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var svId))
00108:             {
00109:                 foreach (var handler in _medical.Pipeline.Handlers)
00110:                 {
00111:                     if (handler == null) continue;
00112:                     var episode = handler.GetEpisode(svId);
00113:                     if (episode != null && episode.IsActive)
00114:                     {
00115:                         if (!activeIds.Contains(handler.DefinitionId.Value))
00116:                             activeIds.Add(handler.DefinitionId.Value);
00117:                     }
00118:                 }
00119:             }
00120:
00121:             // 2. Direct domain fallbacks
00122:             var rad = _survivors?.RadStateFor(survivorId);
00123:             if (rad is { HasAcuteRadiationSickness: true })
00124:             {
00125:                 if (!activeIds.Contains(Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId))
00126:                     activeIds.Add(Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId);
00127:             }
00128:
00129:             float respDeg = _phase0?.Respiratory?.RespiratoryDegradation(survivorId) ?? 0f;
00130:             if (respDeg > 0f)
00131:             {
00132:                 if (!activeIds.Contains(Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId))
00133:                     activeIds.Add(Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId);
00134:             }
00135:
00136:             if (_phase0?.CombatTrauma != null && _phase0.CombatTrauma.GetHypervigilanceLevel(survivorId) >= 0.6f)
00137:             {
00138:                 if (!activeIds.Contains(Ashfall.Core.Medical.MedicalTreatmentCatalog.CombatTraumaId))
00139:                     activeIds.Add(Ashfall.Core.Medical.MedicalTreatmentCatalog.CombatTraumaId);
00140:             }
00141:
00142:             if (_medical?.Engine != null && _medical.Engine.Ledger.TryGetValue(survivorId, out var deps))
00143:             {
00144:                 if (deps.Any(d => d.dependencyLevel >= Ashfall.Core.Medical.ChemicalDependencySystem.DependencyThreshold || d.inColdTurkey))
00145:                 {
00146:                     if (!activeIds.Contains(Ashfall.Core.Medical.MedicalTreatmentCatalog.ChemicalDependencyId))
00147:                         activeIds.Add(Ashfall.Core.Medical.MedicalTreatmentCatalog.ChemicalDependencyId);
00148:                 }
00149:             }
00150:
00151:             // 3. Limb conditions (Plan 190 AmputationSystem integration)
00152:             if (_amputation != null && _amputation.State.survivorLimbs.TryGetValue(survivorId, out var limbs))
00153:             {
00154:                 if (limbs != null)
00155:                 {
00156:                     foreach (var limb in limbs)
00157:                     {
00158:                         if (limb == null) continue;
00159:                         if (limb.limb == Ashfall.Core.Medical.LimbId.LeftLeg || limb.limb == Ashfall.Core.Medical.LimbId.RightLeg)
00160:                         {
00161:                             if (limb.condition == Ashfall.Core.Medical.LimbCondition.Wounded
00162:                                 || limb.condition == Ashfall.Core.Medical.LimbCondition.Amputated
00163:                                 || limb.condition == Ashfall.Core.Medical.LimbCondition.Gangrenous)
00164:                             {
00165:                                 if (!activeIds.Contains("affliction_broken_leg"))
00166:                                     activeIds.Add("affliction_broken_leg");
00167:                             }
00168:                         }
00169:                         else if (limb.limb == Ashfall.Core.Medical.LimbId.LeftArm || limb.limb == Ashfall.Core.Medical.LimbId.RightArm)
00170:                         {
00171:                             if (limb.condition == Ashfall.Core.Medical.LimbCondition.Wounded
00172:                                 || limb.condition == Ashfall.Core.Medical.LimbCondition.Amputated
00173:                                 || limb.condition == Ashfall.Core.Medical.LimbCondition.Gangrenous)
00174:                             {
00175:                                 if (!activeIds.Contains("affliction_broken_arm"))
00176:                                     activeIds.Add("affliction_broken_arm");
00177:                             }
00178:                         }
00179:                     }
00180:                 }
00181:             }
00182:
00183:             return activeIds;
00184:         }
00185:
00186:         /// <summary>
00187:         /// Plan 143: Pure query checking if a quest tag is blocked for a survivor.
00188:         /// </summary>
00189:         public bool IsQuestBlockedForSurvivor(string survivorId, string questTag, out List<string> blockingAfflictions)
00190:         {
00191:             blockingAfflictions = new List<string>();
00192:             if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(questTag)) return false;
00193:
00194:             SetupMedical();
00195:             var activeAfflictions = GetActiveAfflictionIds(survivorId);
00196:             var gateResult = _medical.Bridge.QueryQuestGate(questTag, activeAfflictions);
00197:             if (gateResult.IsBlocked)
00198:             {
00199:                 blockingAfflictions.AddRange(gateResult.BlockingAfflictionIds);
00200:                 return true;
00201:             }
00202:             return false;
00203:         }
00204:
00205:         /// <summary>
00206:         /// Plan 143: Pure query returning quest tags unlocked by the survivor's medical conditions.
00207:         /// </summary>
00208:         public IReadOnlyList<string> GetUnlockedQuestsForSurvivor(string survivorId)
00209:         {
00210:             if (string.IsNullOrEmpty(survivorId)) return Array.Empty<string>();
00211:
00212:             SetupMedical();
00213:             var activeAfflictions = GetActiveAfflictionIds(survivorId);
00214:             return _medical.Bridge.GetUnlockedQuestTags(activeAfflictions);
00215:         }
00216:
00217:         /// <summary>
00218:         /// Task #133: construct and bind the unified medical pipeline once the
00219:         /// inventory, survivors, and Phase-0 sessions exist. Idempotent.
00220:         /// </summary>
00221:         private void EnsureMedicalPipeline()
00222:         {
00223:             SetupMedical();
00224:             if (_medical.Pipeline != null) return;
00225:             SetupSurvivors();
00226:             SetupInventory();
00227:             SetupPhase0();
00228:
00229:             var pipeline = new Ashfall.Core.Medical.MedicalPipelineCoordinator(
00230:                 _inventory.Inventory,
00231:                 new Ashfall.Core.Medical.DiagnosisKnowledgeStore(),
00232:                 new Ashfall.Core.Medical.MedicalReservationLedger(),
00233:                 new Ashfall.Core.Medical.MedicalProcedureSchedule(),
00234:                 sv => ResolvePatientAvailability(sv.Value),
00235:                 () => _simDay,
00236:                 // SHELTER_EMP_MEDICAL_POWER: clinic/ward power read from the grid.
00237:                 // Null-safe: an absent grid leaves the clinic powered (legacy behavior).
00238:                 () => _powerGrid?.System == null || _powerGrid.System.IsRoomPowered("room_clinic"),
00239:                 capabilityCheck: knowledgeId => EnsureSharedResearch().HasCapability(knowledgeId));
00240:
00241:             var respiratoryDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId);
00242:             var radiationDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId);
00243:
00244:             pipeline.RegisterHandler(new Ashfall.Core.Medical.RespiratoryAfflictionHandler(_phase0.Respiratory));
00245:             pipeline.RegisterHandler(new Ashfall.Core.Medical.RadiationSicknessAfflictionHandler(
00246:                 getDose: id => _survivors.RadStateFor(id)?.RadiationDose ?? 0f,
00247:                 getPhaseName: GetRadiationPhaseName,
00248:                 hasAcuteSickness: id => _survivors.RadStateFor(id)?.HasAcuteRadiationSickness ?? false,
00249:                 applyIodine: id => !_survivors.AdministerIodine(id).StartsWith("Unknown survivor", StringComparison.Ordinal),
00250:                 applyAntiRad: (id, rads) => !_survivors.AdministerAntiRad(id, rads).StartsWith("Unknown survivor", StringComparison.Ordinal)));
00251:             pipeline.RegisterHandler(new Ashfall.Core.Medical.HealthDeficitAfflictionHandler(
00252:                 getHealth: id => _survivors.Find(id)?.Health ?? 100f,
00253:                 getMaxHealth: id => _survivors.Find(id)?.MaxHealthCap ?? 100f,
00254:                 applyHeal: (id, amount) => !_survivors.HealSurvivor(id, amount).StartsWith("Unknown survivor", StringComparison.Ordinal)));
00255:
00256:             // Task #133 P1b — chemical-dependency detox starts flow through
00257:             // the pipeline; the shared engine keeps every withdrawal clock.
00258:             pipeline.RegisterHandler(new Ashfall.Core.Medical.ChemicalDependencyAfflictionHandler(_medical.Engine));
00259:
00260:             // Task #133 P1c — observe-only psychology projection: Phase-0
00261:             // combat trauma, flashbacks, and guilt insomnia surface as
00262:             // read-only patient rows. The handlers write nothing and own no
00263:             // clock; phase0_psychology keeps every rule and tick.
00264:             pipeline.RegisterHandler(new Ashfall.Core.Medical.GuiltInsomniaAfflictionHandler(_phase0.Guilt));
00265:             pipeline.RegisterHandler(new Ashfall.Core.Medical.SomaticFlashbackAfflictionHandler(_phase0.Flashbacks));
00266:             pipeline.RegisterHandler(new Ashfall.Core.Medical.CombatTraumaAfflictionHandler(_phase0.CombatTrauma));
00267:
00268:             // Task #133 P1 — disease write-path: one handler per authored
00269:             // disease plus the four camp-wide vector protocols. The disease
00270:             // domain keeps every clinical rule; the pipeline owns the
00271:             // validate → consume → apply transaction.
00272:             SetupDisease();
00273:             if (_disease != null)
00274:             {
00275:                 Ashfall.Core.Medical.DiseaseAfflictionHandler.RegisterAll(pipeline, _disease.Engine, _disease.Catalog);
00276:                 Ashfall.Core.Medical.DiseaseProtocolHandler.RegisterAll(pipeline, _disease.Engine, () => _simDay);
00277:
00278:                 // Auto-suspect on live infection (never confirms — the player
00279:                 // identifies the illness explicitly through the examination).
00280:                 _disease.Engine.OnInfection += (survivorId, diseaseId) =>
00281:                 {
00282:                     if (Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv)
00283:                         && Ashfall.Core.Medical.AfflictionId.IsValid(diseaseId, out _))
00284:                         pipeline.SuspectFromEvidence(sv, new Ashfall.Core.Medical.AfflictionId(diseaseId), _simDay, "infection_event");
00285:                 };
00286:
00287:                 MigrateDiseaseSuspicions(pipeline);
00288:             }
00289:
00290:             // Auto-suspect: domain threshold crossings raise the shelter's
00291:             // knowledge to Suspected (never Confirmed — confirmation is explicit).
00292:             _phase0.Respiratory.OnSevereCoughStarted += survivorId =>
00293:             {
00294:                 if (Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv))
00295:                     pipeline.SuspectFromEvidence(sv, respiratoryDef, _simDay, "severe_cough_threshold");
00296:             };
00297:             _phase0.RadiationPhase.OnPhaseChanged += (survivorId, oldPhase, newPhase) =>
00298:             {
00299:                 if (newPhase != Ashfall.Core.Radiation.RadiationSicknessPhase.Healthy &&
00300:                     Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv))
00301:                     pipeline.SuspectFromEvidence(sv, radiationDef, _simDay, "radiation_phase_" + newPhase);
00302:             };
00303:
00304:             _medical.BindPipeline(pipeline);
00305:             // Task #133 P1b: share the pipeline with the ward and chem-dep
00306:             // sessions when already constructed (they backfill from
00307:             // _medical.Pipeline otherwise, covering every setup order).
00308:             if (_medicalWardSession != null) _medicalWardSession.Pipeline = pipeline;
00309:             if (_chemicalDependency != null) _chemicalDependency.Pipeline = pipeline;
00310:             MigrateLegacyMedicalDiagnoses();
00311:             GD.Print("[Ashfall Godot] Medical pipeline bound (Task #133).");
00312:         }
00313:
00314:         /// <summary>
00315:         /// Task #133 P1: restored infections that carry no diagnosis knowledge
00316:         /// are raised to Suspected (never Confirmed) so a load does not wipe
00317:         /// the suspicion trail. Idempotent — SuspectFromEvidence only moves
00318:         /// Unknown episodes.
00319:         /// </summary>
00320:         private void MigrateDiseaseSuspicions(Ashfall.Core.Medical.MedicalPipelineCoordinator pipeline)
00321:         {
00322:             if (_disease == null || _survivors == null) return;
00323:             var catalog = _disease.Catalog;
00324:             for (int i = 0; i < _survivors.RosterState.Count; i++)
00325:             {
00326:                 var survivor = _survivors.RosterState[i];
00327:                 if (survivor == null || !survivor.IsAlive) continue;
00328:                 for (int d = 0; d < catalog.Diseases.Count; d++)
00329:                 {
00330:                     var disease = catalog.Diseases[d];
00331:                     if (disease == null || string.IsNullOrEmpty(disease.id)) continue;
00332:                     if (!Ashfall.Core.Medical.AfflictionId.IsValid(disease.id, out _)) continue;
00333:                     if (!_disease.Engine.TryGetInfection(survivor.Id, disease.id, out int _, out bool _)) continue;
00334:                     pipeline.SuspectFromEvidence(
00335:                         new Ashfall.Core.Survivors.SurvivorId(survivor.Id),
00336:                         new Ashfall.Core.Medical.AfflictionId(disease.id),
00337:                         _simDay, "restored_infection");
00338:                 }
00339:             }
00340:         }
00341:
00342:         /// <summary>Canonical lifecycle gate for the pipeline (Task #132 semantics; roster is the live authority until the entity store is host-mounted).</summary>
00343:         private Ashfall.Core.Medical.PatientAvailability ResolvePatientAvailability(string survivorId)
00344:         {
00345:             var survivor = _survivors?.Find(survivorId);
00346:             if (survivor == null) return Ashfall.Core.Medical.PatientAvailability.Blocked("patient_unknown");
00347:             if (!survivor.IsAlive) return Ashfall.Core.Medical.PatientAvailability.Blocked("patient_dead");
00348:             return Ashfall.Core.Medical.PatientAvailability.Ok();
00349:         }
00350:
00351:         private string GetRadiationPhaseName(string survivorId)
00352:         {
00353:             var phase = _phase0?.RadiationPhase;
00354:             if (phase != null && phase.Survivors.TryGetValue(survivorId, out var state))
00355:                 return state.Phase.ToString();
00356:             return "Healthy";
00357:         }
00358:
00359:         /// <summary>
00360:         /// Legacy-save migration: the pre-pipeline game displayed these
00361:         /// conditions openly, so restored episodes arrive Confirmed. New
00362:         /// progression starts Unknown/Suspected; this runs once per load.
00363:         /// </summary>
00364:         private void MigrateLegacyMedicalDiagnoses()
00365:         {
00366:             var pipeline = _medical.Pipeline;
00367:             if (pipeline == null) return;
00368:             var respiratoryDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId);
00369:             var radiationDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId);
00370:             for (int i = 0; i < _survivors.RosterState.Count; i++)
00371:             {
00372:                 var survivor = _survivors.RosterState[i];
00373:                 if (survivor == null) continue;
00374:                 if (_phase0.Respiratory.RespiratoryDegradation(survivor.Id) > 0f)
00375:                     pipeline.ConfirmForLegacySave(new Ashfall.Core.Survivors.SurvivorId(survivor.Id), respiratoryDef, _simDay);
00376:                 if (!string.Equals(GetRadiationPhaseName(survivor.Id), "Healthy", StringComparison.Ordinal)
00377:                     || (_survivors.RadStateFor(survivor.Id)?.HasAcuteRadiationSickness ?? false))
00378:                     pipeline.ConfirmForLegacySave(new Ashfall.Core.Survivors.SurvivorId(survivor.Id), radiationDef, _simDay);
00379:             }
00380:         }
00381:
00382:         private void SaveMedicalPipeline()
00383:         {
00384:             if (_medical?.Pipeline == null) return;
00385:             var save = _medical.CapturePipelineSave();
00386:             if (save == null) return;
00387:             if (CaptureSection("medical_pipeline", MedicalPipelineSaveStore.TryCapturePersisted(save)))
00388:                 GD.Print("[Ashfall Godot] Medical pipeline save written.");
00389:         }
00390:
00391:         private void SaveMedical()
00392:         {
00393:             if (_medical == null) return;
00394:             if (CaptureSection("medical", MedicalSaveStore.TryCapturePersisted(_medical.CaptureSave())))
00395:             {
00396:                 _medicalDirty = false;
00397:                 GD.Print("[Ashfall Godot] Medical save written.");
00398:             }
00399:         }
00400:
00401:         private MedicalWardHostSession _medicalWardSession = null!;
00402:         private MedicalWardPanel _medicalWardPanel = null!;
00403:
00404:         private void SetupMedicalWard()
00405:         {
00406:             if (_medicalWardSession != null) return;
00407:             var beds = new List<Ashfall.Core.Medical.MedicalBed>
00408:             {
00409:                 new Ashfall.Core.Medical.MedicalBed("bed_general_a", "General A", Ashfall.Core.Medical.MedicalBedCategory.General),
00410:                 new Ashfall.Core.Medical.MedicalBed("bed_general_b", "General B", Ashfall.Core.Medical.MedicalBedCategory.General),
00411:                 new Ashfall.Core.Medical.MedicalBed("bed_surgical", "Surgical", Ashfall.Core.Medical.MedicalBedCategory.Surgical),
00412:                 new Ashfall.Core.Medical.MedicalBed("bed_isolation", "Isolation", Ashfall.Core.Medical.MedicalBedCategory.Isolation, isolation: true),
00413:                 new Ashfall.Core.Medical.MedicalBed("bed_chelation", "Chelation", Ashfall.Core.Medical.MedicalBedCategory.Chelation)
00414:             };
00415:             var procs = new List<Ashfall.Core.Medical.MedicalProcedureDef>
00416:             {
00417:                 new Ashfall.Core.Medical.MedicalProcedureDef("proc_bandage", "Bandage", "MedicalSystem"),
00418:                 new Ashfall.Core.Medical.MedicalProcedureDef("proc_chelation", "Chelation", "DoseLedgerSystem"),
00419:                 new Ashfall.Core.Medical.MedicalProcedureDef("proc_surgery", "Surgery", "MedicalSystem")
00420:             };
00421:             _medicalWard = new Ashfall.Core.Medical.MedicalWardSystem(
00422:                 new Ashfall.Core.Medical.MedicalWardState(), beds, procs);
00423:             _medicalWardSession = new MedicalWardHostSession(_medicalWard);
00424:             _medicalWardSession.Procedures = procs;
00425:             _medicalWardSession.SimDay = _simDay;
00426:             // Task #133 P1b: share the pipeline when already bound; otherwise
00427:             // EnsureMedicalPipeline backfills this reference once it runs.
00428:             _medicalWardSession.Pipeline = _medical?.Pipeline;
00429:             _medicalWardSession.StateChanged += () => _medicalWardDirty = true;
00430:             _medicalWard.OnWardChanged += _ => _medicalWardDirty = true;
00431:             _medicalWard.OnPatientAdmitted += patientId =>
00432:             {
00433:                 // An admitted patient is no longer available labor. The duty
00434:                 // roster remains the labor authority; the ward only announces
00435:                 // the transition so the host can vacate that existing entry.
00436:                 if (_dutyRoster?.Roster != null)
00437:                 {
00438:                     _dutyRoster.Roster.RemoveAssignmentsFor(patientId);
00439:                     _dutyRosterDirty = true;
00440:                 }
00441:             };
00442:             _medicalWard.StaffingPreflight = () =>
00443:             {
00444:                 if (_dutyRoster?.Roster == null) return true;
00445:                 string staffId = _dutyRoster.Roster.GetAssignment(DutyRosterIds.RoleWard);
00446:                 return !string.IsNullOrEmpty(staffId);
00447:             };
00448:             LoadMedicalWard();
00449:             if (_medicalWardPanel == null)
00450:             {
00451:                 _medicalWardPanel = new MedicalWardPanel();
00452:                 _medicalWardPanel.Bind(_medicalWardSession);
00453:                 _medicalWardPanel.Visible = false;
00454:                 AddChild(_medicalWardPanel);
00455:             }
00456:
00457:             // Plan 60 / D2 + D6 — the bed is where the clinical note, the authorised
00458:             // treatment, and the vigil belong, so all three are offered from the one
00459:             // surface the player is already looking at.
00460:             SetupDisease();
00461:             SetupMedical();
00462:             _medicalWardPanel.BindDisease(_disease);
00463:             _medicalWardPanel.BindVigil(_medical);
00464:         }
00465:
00466:         private void SaveMedicalWard()
00467:         {
00468:             if (_medicalWardSession == null || _medicalWard == null) return;
00469:             try
00470:             {
00471:                 _medicalWardSession.SimDay = _simDay;
00472:                 var save = new Ashfall.Core.Medical.MedicalWardSave
00473:                 {
00474:                     simDay = _medicalWardSession.SimDay,
00475:                     Beds = new List<Ashfall.Core.Medical.MedicalBedSave>(),
00476:                     Procedures = new List<Ashfall.Core.Medical.MedicalProcedureDef>(_medicalWardSession.Procedures),
00477:                     State = _medicalWard.CaptureState()
00478:                 };
00479:                 foreach (var bed in _medicalWard.Beds)
00480:                 {
00481:                     save.Beds.Add(new Ashfall.Core.Medical.MedicalBedSave
00482:                     {
00483:                         BedId = bed.BedId,
00484:                         DisplayName = bed.DisplayName,
00485:                         Category = (int)bed.Category,
00486:                         Isolation = bed.Isolation
00487:                     });
00488:                 }
00489:
00490:                 if (CaptureSection("medical_ward", MedicalWardSaveStore.TryCapturePersisted(save)))
00491:                 {
00492:                     _medicalWardDirty = false;
00493:                     _medicalWardSession.ClearDirty();
00494:                 }
00495:             }
00496:             catch (Exception e)
00497:             {
00498:                 GD.PushWarning("[Ashfall Godot] MedicalWard save failed: " + e.Message);
00499:                 CaptureSection("medical_ward", string.Empty);
00500:             }
00501:         }
00502:
00503:         private void LoadMedicalWard()
00504:         {
00505:             try
00506:             {
00507:                 var loaded = MedicalWardSaveStore.TryLoad();
00508:                 if (loaded != null)
00509:                 {
00510:                     _medicalWardSession?.RestoreSave(loaded);
00511:                     if (_medicalWardSession != null)
00512:                         _medicalWard = _medicalWardSession.System;
00513:                 }
00514:             }
00515:             catch (Exception e)
00516:             {
00517:                 GD.PushWarning("[Ashfall Godot] MedicalWard load failed: " + e.Message);
00518:             }
00519:         }
00520:
00521:         private void SaveDisease()
00522:         {
00523:             if (_disease == null) return;
00524:             try
00525:             {
00526:                 CaptureSection("disease", DiseaseSaveStore.TryCapturePersisted(_disease.Engine.CaptureState()));
00527:             }
00528:             catch (Exception e)
00529:             {
00530:                 GD.PushWarning("[Ashfall Godot] Disease save failed: " + e.Message);
00531:             }
00532:         }
00533:
00534:         private void SetupDisease()
00535:         {
00536:             if (_disease != null) return;
00537:             SetupExpansions();
00538:             var engine = _expansions.Disease;
00539:             if (engine == null)
00540:             {
00541:                 GD.PrintErr("[Ashfall Godot] Disease Expansion missing from expansion hub; ward offline.");
00542:                 return;
00543:             }
00544:             _disease = new AtomicWar.GodotApp.DiseaseHostSession(engine, _expansions.DiseaseData);
00545:             // The exposure pool is the people actually in the shelter tonight
00546:             // (duty-roster home occupants). Pure presentation wiring — the
00547:             // engine owns all rules.
00548:             // Plan 60 / D4 — protocols are armed with the day they are applied, so
00549:             // the authored window counts from the moment the work is done.
00550:             _disease.BindDayProvider(() => _simDay);
00551:             _disease.BindPopulationProvider(() =>
00552:             {
00553:                 var occupants = BuildHomeOccupantSnapshot();
00554:                 var ids = new List<string>();
00555:                 for (int i = 0; i < occupants.Count; i++)
00556:                 {
00557:                     var o = occupants[i];
00558:                     if (o != null && !string.IsNullOrEmpty(o.survivorId))
00559:                         ids.Add(o.survivorId);
00560:                 }
00561:                 return ids;
00562:             });
00563:             // Ward state rides the expansion-hub save (restored above); any
00564:             // change marks the hub dirty so nothing is lost at day end.
00565:             _disease.StateChanged += () => { _expansionHubDirty = true; };
00566:
00567:             // Plan 60 / D3 — treatment has to spend from the one item authority the
00568:             // rest of the game already uses, and a dose has to be recorded where the
00569:             // player can read it back. The ward UI could not otherwise tell a cured
00570:             // patient from one that merely got company.
00571:             _disease.BindSupply((itemId, count) =>
00572:             {
00573:                 if (count <= 0 || string.IsNullOrEmpty(itemId)) return false;
00574:                 SetupInventory();
00575:                 if (_inventory?.Inventory == null) return false;
00576:                 bool spent = _inventory.Inventory.TryConsume(itemId, count);
00577:                 if (spent) SaveInventory();
00578:                 return spent;
00579:             });
00580:
00581:             // SHELTER_FAILURE_EFFECTS: campaign quarantine containment — the
00582:             // coordinator assigns/releases isolation and computes daily isolation
00583:             // quality against the canonical disease engine. Ventilation power
00584:             // reads the room_ward_quarantine breaker (G5 delegate); daily care
00585:             // consumption mirrors BindSupply above.
00586:             SetupMedicalWard();
00587:             var quarantineCoordinator = new Ashfall.Core.Disease.DiseaseQuarantineCoordinator(
00588:                 _medicalWard,
00589:                 engine,
00590:                 _dutyRoster?.Roster,
00591:                 tryConsumeItem: (itemId, count) =>
00592:                 {
00593:                     SetupInventory();
00594:                     return _inventory?.Inventory != null && _inventory.Inventory.TryConsume(itemId, count);
00595:                 },
00596:                 containmentProvider: () => Ashfall.Core.Disease.ContainmentCapability.FromResearch(
00597:                     k => _sharedResearch?.State.completedIds.Contains(k) ?? false),
00598:                 isolationPowerCheck: () => _powerGrid?.System == null
00599:                     || _powerGrid.System.IsRoomPowered("room_ward_quarantine"));
00600:             _disease.BindCoordinator(quarantineCoordinator);
00601:             _disease.Engine.OnTreatmentApplied += (survivorId, diseaseId, itemId, role, day) =>
00602:             {
00603:                 SetupJournal();
00604:                 _journal?.TryAddRawEntry(
00605:                     $"treatment_{survivorId}_{day}_{diseaseId}",
00606:                     $"{survivorId}: {role} treatment with {itemId} for {diseaseId}.",
00607:                     null!, day);
00608:
00609:                 var patientName = FormatSurvivorName(survivorId);
00610:                 FeedbackMessages.Emit(new FeedbackEvent(
00611:                     key: "medical_treatment_success",
00612:                     arguments: new object[] { patientName },
00613:                     category: "success",
00614:                     dedupeKey: $"treatment_{survivorId}_{day}"
00615:                 ));
00616:             };
00617:
00618:             _disease.Engine.OnInfection += (survivorId, diseaseId) =>
00619:             {
00620:                 FeedbackMessages.Emit(new FeedbackEvent(
00621:                     key: "disease_alert",
00622:                     category: "alert",
00623:                     dedupeKey: $"disease_alert_{survivorId}"
00624:                 ));
00625:             };
00626:
00627:             _disease.Engine.OnOutbreakDeclared += (diseaseId) =>
00628:             {
00629:                 FeedbackMessages.Emit(new FeedbackEvent(
00630:                     key: "disease_outbreak",
00631:                     category: "warning",
00632:                     dedupeKey: $"disease_outbreak_{diseaseId}"
00633:                 ));
00634:             };
00635:
00636:             GD.Print("[Ashfall Godot] Disease Expansion ward ready (contagion · quarantine · outbreak · treatment).");
00637:
00638:             // The bed inspector is where a player stands when someone in their ward
00639:             // is dying, so treatment is offered there rather than on a parallel
00640:             // disease screen. Idempotent: BindDisease just re-points and refreshes.
00641:             SetupMedicalWard();
00642:             _medicalWardPanel?.BindDisease(_disease);
00643:         }
00644:
00645:         private void CloseMedicalPanel()
00646:         {
00647:             _medicalPanel.Visible = false;
00648:         }
00649:
00650:     }
00651: }
```

## `src/UI/DoseLedgerPanel.cs` — 451 lines; 20,496 bytes; SHA-256 `4238960b70761f9089982d81facd2f41086749c0b24028ef7c84f944a6ca6ac0`
Declaration index:
- 00025: public partial class DoseLedgerPanel : Control, IBindablePanel
- 00045: public void Bind(DoseLedgerHostSession session, SurvivorsHostSession? survivors = null)
- 00057: public void Unbind()
- 00069: private void HandleLedgerChanged(DoseLedgerSystemState _) => RefreshView();
- 00071: public void RefreshView()
- 00078: private void RefreshStatusRail()
- 00109: private void BuildDoseRows()
- 00167: private bool FilterPass(AshfallDataGrid.CellState band) => _activeFactionFilter switch
- 00176: internal static AshfallDataGrid.CellState MapBand(float cumulativeMsv)
- 00185: internal static string BandName(AshfallDataGrid.CellState s) => s switch
- 00195: private void RefreshDetail()
- 00251: private static string FormatSurvivor(string id)
- 00321: private void BuildContent()
- 00388: private static void LegendChip(HBoxContainer host, string label, (float r, float g, float b, float a) token, string value)
- 00399: private static List<AshfallDataGrid.Row> BuildFixtureRows()
- 00429: public void Open()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.UI;
00008: using AtomicWar.GodotApp.UI;
00009: using DesignTheme = Ashfall.Core.UI.Theme;
00010:
00011: namespace AtomicWar.GodotApp.UI;
00012:
00013: /// <summary>
00014: /// ASHFALL — Dose Ledger (#59 Stitch, Decontamination & Dose Ledger Terminal).
00015: ///
00016: /// Per-survivor cumulative radiation readout. Pure presentation. Reads only
00017: /// from <see cref="DoseLedgerSystem"/> via the host session.
00018: ///
00019: /// The ledger deliberately ONLY shows survivors with a booked dosimeter.
00020: /// Unbound survivors are not part of the ledger — that's per
00021: /// `DoseLedgerSystem`'s design intent ("readings are only booked against
00022: /// survivors with an assigned dosimeter tag; unbooked rads are the
00023: /// shelter's silence").
00024: /// </summary>
00025: public partial class DoseLedgerPanel : Control, IBindablePanel
00026: {
00027:     public event Action? OnClose;
00028:     public event Action<string>? OnSurvivorSelected;
00029:
00030:     private AshfallDashboardShell _shell = null!;
00031:     private AshfallSidebar? _sidebar;
00032:     private AshfallStatusRail? _statusRail;
00033:     private AshfallDataGrid? _doseGrid;
00034:     private VBoxContainer _detailBox = null!;
00035:     private Label _detailTitle = null!;
00036:     private int _selectedIndex = -1;
00037:     private string _activeFactionFilter = "all"; // all | amber | red | black | unbound
00038:
00039:     private DoseLedgerHostSession? _doseSession;
00040:     private SurvivorsHostSession? _survivorsHost;
00041:     private List<DoseEntry> _visibleEntries = new();
00042:
00043:     public bool IsBound => _doseSession != null;
00044:
00045:     public void Bind(DoseLedgerHostSession session, SurvivorsHostSession? survivors = null)
00046:     {
00047:         Unbind();
00048:         _doseSession = session;
00049:         _survivorsHost = survivors;
00050:         if (_doseSession?.Ledger != null)
00051:         {
00052:             _doseSession.Ledger.OnStateChanged += HandleLedgerChanged;
00053:         }
00054:         RefreshView();
00055:     }
00056:
00057:     public void Unbind()
00058:     {
00059:         if (_doseSession?.Ledger != null)
00060:         {
00061:             _doseSession.Ledger.OnStateChanged -= HandleLedgerChanged;
00062:         }
00063:         _doseSession = null;
00064:         _survivorsHost = null;
00065:     }
00066:
00067:
00068:
00069:     private void HandleLedgerChanged(DoseLedgerSystemState _) => RefreshView();
00070:
00071:     public void RefreshView()
00072:     {
00073:         RefreshStatusRail();
00074:         BuildDoseRows();
00075:         RefreshDetail();
00076:     }
00077:
00078:     private void RefreshStatusRail()
00079:     {
00080:         if (_statusRail == null) return;
00081:         if (_doseSession == null)
00082:         {
00083:             _statusRail.Set("entries", "0", AshfallMetricCard.Criticality.Normal);
00084:             _statusRail.Set("ceiling", "0 mSv", AshfallMetricCard.Criticality.Normal);
00085:             _statusRail.Set("amber",   "0", AshfallMetricCard.Criticality.Caution);
00086:             _statusRail.Set("red",     "0", AshfallMetricCard.Criticality.Warn);
00087:             _statusRail.Set("black",   "0", AshfallMetricCard.Criticality.Critical);
00088:             _statusRail.Set("cal",     "—", AshfallMetricCard.Criticality.Normal);
00089:             return;
00090:         }
00091:         var ledger = _doseSession.Ledger;
00092:         int amber = 0, red = 0, black = 0;
00093:         foreach (var entry in ledger.Entries)
00094:         {
00095:             float cum = ledger.GetCumulative(entry.survivorId);
00096:             if (cum >= DoseLedgerSystem.BlackMsv) black++;
00097:             else if (cum >= DoseLedgerSystem.RedMsv) red++;
00098:             else if (cum >= DoseLedgerSystem.AmberMsv) amber++;
00099:         }
00100:         _statusRail.Set("entries", $"{ledger.Entries.Count}", AshfallMetricCard.Criticality.Normal);
00101:         _statusRail.Set("ceiling", $"{ledger.State.ceilingMsv:0} mSv", AshfallMetricCard.Criticality.Normal);
00102:         _statusRail.Set("amber",   amber > 0 ? $"{amber}" : "0", amber > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
00103:         _statusRail.Set("red",     red   > 0 ? $"{red}"   : "0", red   > 0 ? AshfallMetricCard.Criticality.Warn   : AshfallMetricCard.Criticality.Normal);
00104:         _statusRail.Set("black",   black > 0 ? $"{black}" : "0", black > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
00105:         _statusRail.Set("cal",     ledger.State.calibrationOverdue ? "OVERDUE" : "OK",
00106:             ledger.State.calibrationOverdue ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
00107:     }
00108:
00109:     private void BuildDoseRows()
00110:     {
00111:         if (_doseGrid == null) return;
00112:         if (_doseSession == null)
00113:         {
00114:             _doseGrid.SetRows(AshfallDataGrid.UnavailableRows(5, "Unavailable — dose ledger not bound."));
00115:             return;
00116:         }
00117:
00118:         var rows = new List<AshfallDataGrid.Row>();
00119:         _visibleEntries.Clear();
00120:         var ledger = _doseSession.Ledger;
00121:         foreach (var entry in ledger.Entries)
00122:         {
00123:             if (entry == null) continue;
00124:             float cum = ledger.GetCumulative(entry.survivorId);
00125:             var band = MapBand(cum);
00126:             if (!FilterPass(band)) continue;
00127:             _visibleEntries.Add(entry);
00128:
00129:             var lastReading = entry.readingsHistory != null && entry.readingsHistory.Count > 0
00130:                 ? entry.readingsHistory[entry.readingsHistory.Count - 1] : null;
00131:
00132:             string lastTxt = lastReading != null
00133:                 ? $"D{lastReading.day} · {lastReading.bookedMsv:0.0} mSv"
00134:                 : "—";
00135:
00136:             string tag = string.IsNullOrEmpty(entry.assignedDosimeterTag) ? "—" : entry.assignedDosimeterTag;
00137:             string lastAntiRad = entry.lastAntiRadDay < 0 ? "—" : $"D{entry.lastAntiRadDay}";
00138:
00139:             var cells = new List<AshfallDataGrid.Cell>
00140:             {
00141:                 new(FormatSurvivor(entry.survivorId), AshfallDataGrid.CellState.Normal),
00142:                 new($"{cum:0.0} mSv", band),
00143:                 new(BandName(band), band),
00144:                 new(tag, string.IsNullOrEmpty(entry.assignedDosimeterTag) ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Normal),
00145:                 new(lastAntiRad, lastReading == null ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Normal),
00146:             };
00147:             rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
00148:         }
00149:         if (rows.Count == 0 && _activeFactionFilter != "all")
00150:         {
00151:             // Surface the empty state explicitly.
00152:             rows.Add(new AshfallDataGrid.Row
00153:             {
00154:                 Cells = new List<AshfallDataGrid.Cell>
00155:                 {
00156:                     new("— no matches —", AshfallDataGrid.CellState.Muted),
00157:                     new("—", AshfallDataGrid.CellState.Muted),
00158:                     new("—", AshfallDataGrid.CellState.Muted),
00159:                     new("—", AshfallDataGrid.CellState.Muted),
00160:                     new("—", AshfallDataGrid.CellState.Muted),
00161:                 }
00162:             });
00163:         }
00164:         _doseGrid.SetRows(rows);
00165:     }
00166:
00167:     private bool FilterPass(AshfallDataGrid.CellState band) => _activeFactionFilter switch
00168:     {
00169:         "amber"  => band == AshfallDataGrid.CellState.Caution,
00170:         "red"    => band == AshfallDataGrid.CellState.Warning,
00171:         "black"  => band == AshfallDataGrid.CellState.Critical,
00172:         "ok"     => band == AshfallDataGrid.CellState.Normal || band == AshfallDataGrid.CellState.Positive,
00173:         _ => true,
00174:     };
00175:
00176:     internal static AshfallDataGrid.CellState MapBand(float cumulativeMsv)
00177:     {
00178:         if (cumulativeMsv >= DoseLedgerSystem.BlackMsv) return AshfallDataGrid.CellState.Critical;
00179:         if (cumulativeMsv >= DoseLedgerSystem.RedMsv)   return AshfallDataGrid.CellState.Warning;
00180:         if (cumulativeMsv >= DoseLedgerSystem.AmberMsv) return AshfallDataGrid.CellState.Caution;
00181:         if (cumulativeMsv > 0f)                          return AshfallDataGrid.CellState.Normal;
00182:         return AshfallDataGrid.CellState.Positive;
00183:     }
00184:
00185:     internal static string BandName(AshfallDataGrid.CellState s) => s switch
00186:     {
00187:         AshfallDataGrid.CellState.Critical => "BLACK",
00188:         AshfallDataGrid.CellState.Warning => "RED",
00189:         AshfallDataGrid.CellState.Caution => "AMBER",
00190:         AshfallDataGrid.CellState.Normal => "GREEN",
00191:         AshfallDataGrid.CellState.Positive => "ZERO",
00192:         _ => "—",
00193:     };
00194:
00195:     private void RefreshDetail()
00196:     {
00197:         if (_detailBox == null) return;
00198:         AshfallUiHelpers.EmptyChildren(_detailBox);
00199:         if (_selectedIndex < 0 || _selectedIndex >= _visibleEntries.Count || _doseSession == null)
00200:         {
00201:             _detailTitle.Text = "DOSIMETRY DETAIL";
00202:             _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a survivor row to view cumulative dose, baseline, shielding, and recent readings."));
00203:             return;
00204:         }
00205:         var entry = _visibleEntries[_selectedIndex];
00206:         var ledger = _doseSession.Ledger;
00207:         float cum = ledger.GetCumulative(entry.survivorId);
00208:         var band = MapBand(cum);
00209:
00210:         _detailTitle.Text = FormatSurvivor(entry.survivorId).ToUpperInvariant() + " · DOSIMETRY";
00211:
00212:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("CUMULATIVE",
00213:             $"{cum:0.0} mSv",
00214:             AshfallUiHelpers.ToColor(BandToken(band))));
00215:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("BAND", BandName(band), AshfallUiHelpers.ToColor(BandToken(band))));
00216:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("BASELINE", $"{entry.baselineMsv:0.0} mSv", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00217:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("SHIELDING", $"{entry.shieldingFactor:0.00}", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00218:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("TAG",
00219:             string.IsNullOrEmpty(entry.assignedDosimeterTag) ? "— UNBOUND —" : entry.assignedDosimeterTag,
00220:             AshfallUiHelpers.ToColor(string.IsNullOrEmpty(entry.assignedDosimeterTag) ? DesignTheme.Muted : DesignTheme.Warm)));
00221:
00222:         if (entry.readingsHistory != null && entry.readingsHistory.Count > 0)
00223:         {
00224:             _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
00225:             _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("RECENT READINGS"));
00226:             int take = Math.Min(5, entry.readingsHistory.Count);
00227:             for (int i = entry.readingsHistory.Count - 1; i >= entry.readingsHistory.Count - take; i--)
00228:             {
00229:                 var r = entry.readingsHistory[i];
00230:                 if (r == null) continue;
00231:                 string line = $"D{r.day:00} · {AshfallUiHelpers.FormatDoseSource(_doseSession.Content, r.source)} · {AshfallUiHelpers.FormatDosePairMsv(r.nominalMsv, r.bookedMsv)}"
00232:                     + (r.fluxAmbiguous ? " · FLUX" : "")
00233:                     + (r.antiRadAfter ? " · ANTI-RAD" : "");
00234:                 _detailBox.AddChild(AshfallUiHelpers.MakeSmall(line));
00235:             }
00236:         }
00237:
00238:         OnSurvivorSelected?.Invoke(entry.survivorId);
00239:     }
00240:
00241:     private static (float r, float g, float b, float a) BandToken(AshfallDataGrid.CellState s) => s switch
00242:     {
00243:         AshfallDataGrid.CellState.Critical => DesignTheme.Critical,
00244:         AshfallDataGrid.CellState.Warning => DesignTheme.Entropy,
00245:         AshfallDataGrid.CellState.Caution => DesignTheme.Lethe,
00246:         AshfallDataGrid.CellState.Normal => DesignTheme.Warm,
00247:         AshfallDataGrid.CellState.Positive => DesignTheme.Lethe,
00248:         _ => DesignTheme.Pale,
00249:     };
00250:
00251:     private static string FormatSurvivor(string id)
00252:     {
00253:         if (string.IsNullOrEmpty(id)) return "[UNNAMED]";
00254:         if (id == "survivor_dr_sarah_chen" || id == "survivor_sarah_chen") return "Dr. Sarah Chen";
00255:         if (id == "survivor_gunner_mikhail" || id == "survivor_mikhail_volkov") return "Gunner Mikhail";
00256:         if (id == "elena_vasquez" || id == "survivor_elena_vasquez") return "Elena Vasquez";
00257:         return id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant();
00258:     }
00259:
00260:     public override void _Ready()
00261:     {
00262:         SetAnchorsPreset(LayoutPreset.FullRect);
00263:         Visible = false;
00264:
00265:         var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
00266:         bg.SetAnchorsPreset(LayoutPreset.FullRect);
00267:         AddChild(bg);
00268:
00269:         _shell = new AshfallDashboardShell(
00270:             "DOSE LEDGER — DECONTAMINATION_TERMINAL",
00271:             1180, 720);
00272:
00273:         var hostContainer = new MarginContainer();
00274:         hostContainer.AddThemeConstantOverride("margin_left", DesignTheme.SpacingLg);
00275:         hostContainer.AddThemeConstantOverride("margin_top", DesignTheme.SpacingLg);
00276:         hostContainer.AddThemeConstantOverride("margin_right", DesignTheme.SpacingLg);
00277:         hostContainer.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingLg);
00278:         hostContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00279:         hostContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00280:         hostContainer.AddChild(_shell);
00281:         AddChild(hostContainer);
00282:
00283:         _sidebar = _shell.SetSidebar(new[]
00284:         {
00285:             new AshfallSidebar.Item { Id = "filter_all",    Label = "Filter: All",     Hint = "every entry" },
00286:             new AshfallSidebar.Item { Id = "filter_zero",   Label = "Filter: GREEN",   Hint = "< Amber" },
00287:             new AshfallSidebar.Item { Id = "filter_amber",  Label = "Filter: AMBER",   Hint = "≥ 100 mSv" },
00288:             new AshfallSidebar.Item { Id = "filter_red",    Label = "Filter: RED",     Hint = "≥ 300 mSv" },
00289:             new AshfallSidebar.Item { Id = "filter_black",  Label = "Filter: BLACK",   Hint = "≥ 600 mSv" },
00290:         }, "DOSE LEDGER OPS", "filter_all");
00291:
00292:         if (_sidebar != null)
00293:         {
00294:             _sidebar.OnSelected += id =>
00295:             {
00296:                 _activeFactionFilter = id switch
00297:                 {
00298:                     "filter_zero" => "ok",
00299:                     "filter_amber" => "amber",
00300:                     "filter_red" => "red",
00301:                     "filter_black" => "black",
00302:                     _ => "all",
00303:                 };
00304:                 BuildDoseRows();
00305:             };
00306:         }
00307:
00308:         _statusRail = _shell.SetStatusRail();
00309:         _statusRail.AddCard("entries", "BOOKED",  "0",       AshfallMetricCard.Criticality.Normal, 100);
00310:         _statusRail.AddCard("ceiling", "CEILING", "0 mSv",  AshfallMetricCard.Criticality.Normal, 110);
00311:         _statusRail.AddCard("amber",   "AMBER",   "0",       AshfallMetricCard.Criticality.Caution, 100);
00312:         _statusRail.AddCard("red",     "RED",     "0",       AshfallMetricCard.Criticality.Warn,   90);
00313:         _statusRail.AddCard("black",   "BLACK",   "0",       AshfallMetricCard.Criticality.Critical, 100);
00314:         _statusRail.AddCard("cal",     "CAL",     "OK",      AshfallMetricCard.Criticality.Normal,   90);
00315:
00316:         _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());
00317:         BuildContent();
00318:         RefreshView();
00319:     }
00320:
00321:     private void BuildContent()
00322:     {
00323:         var contentStack = new HBoxContainer();
00324:         contentStack.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00325:         contentStack.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00326:         contentStack.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00327:
00328:         var gridCol = new VBoxContainer();
00329:         gridCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00330:         gridCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00331:         gridCol.SizeFlagsStretchRatio = 1.5f;
00332:         gridCol.AddChild(AshfallUiHelpers.MakeSectionHeader("BOOKED DOSIMETERS"));
00333:
00334:         var columns = new[]
00335:         {
00336:             new AshfallDataGrid.Column { Header = "Survivor",  MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left   },
00337:             new AshfallDataGrid.Column { Header = "Cumul.",    MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right  },
00338:             new AshfallDataGrid.Column { Header = "Band",      MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Center },
00339:             new AshfallDataGrid.Column { Header = "Tag",       MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left   },
00340:             new AshfallDataGrid.Column { Header = "Anti-rad",  MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right  },
00341:         };
00342:         _doseGrid = new AshfallDataGrid(columns, showHeader: true, minWidth: 600, minHeight: 360);
00343:         _doseGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00344:         _doseGrid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00345:         _doseGrid.OnRowSelected += idx =>
00346:         {
00347:             _selectedIndex = idx;
00348:             RefreshDetail();
00349:         };
00350:         gridCol.AddChild(_doseGrid);
00351:
00352:         // Band legend strip
00353:         var legend = new HBoxContainer();
00354:         legend.AddThemeConstantOverride("separation", DesignTheme.SpacingLg);
00355:         LegendChip(legend, "AMBER", DesignTheme.Lethe,    $"{DoseLedgerSystem.AmberMsv:0} mSv");
00356:         LegendChip(legend, "RED",   DesignTheme.Entropy,  $"{DoseLedgerSystem.RedMsv:0} mSv");
00357:         LegendChip(legend, "BLACK", DesignTheme.Critical, $"{DoseLedgerSystem.BlackMsv:0} mSv");
00358:         gridCol.AddChild(legend);
00359:
00360:         contentStack.AddChild(gridCol);
00361:
00362:         var detailPanel = AshfallUiHelpers.MakePanel();
00363:         detailPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00364:         detailPanel.SizeFlagsStretchRatio = 0.95f;
00365:         var detailMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
00366:         detailPanel.AddChild(detailMargin);
00367:
00368:         var detailVBox = new VBoxContainer();
00369:         detailVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00370:         detailMargin.AddChild(detailVBox);
00371:
00372:         _detailTitle = new Label { Text = "DOSIMETRY DETAIL" };
00373:         _detailTitle.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeH3);
00374:         _detailTitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00375:         var font = AshfallUiHelpers.LoadFont("res://assets/fonts/BarlowCondensed-SemiBold.ttf");
00376:         if (font != null) _detailTitle.AddThemeFontOverride("font", font);
00377:         detailVBox.AddChild(_detailTitle);
00378:         detailVBox.AddChild(AshfallUiHelpers.MakeSeparator());
00379:         _detailBox = new VBoxContainer();
00380:         _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
00381:         _detailBox.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00382:         detailVBox.AddChild(_detailBox);
00383:
00384:         contentStack.AddChild(detailPanel);
00385:         _shell.SetContent(contentStack);
00386:     }
00387:
00388:     private static void LegendChip(HBoxContainer host, string label, (float r, float g, float b, float a) token, string value)
00389:     {
00390:         var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingXs);
00391:         var dot = new ColorRect { Color = AshfallUiHelpers.ToColor(token), CustomMinimumSize = new Vector2(10, 10) };
00392:         row.AddChild(dot);
00393:         var lbl = AshfallUiHelpers.MakeSmall($"{label} ({value})");
00394:         lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
00395:         row.AddChild(lbl);
00396:         host.AddChild(row);
00397:     }
00398:
00399:     private static List<AshfallDataGrid.Row> BuildFixtureRows()
00400:     {
00401:         var rows = new List<AshfallDataGrid.Row>
00402:         {
00403:             new AshfallDataGrid.Row
00404:             {
00405:                 Cells = new List<AshfallDataGrid.Cell>
00406:                 {
00407:                     new("Gunner Mikhail", AshfallDataGrid.CellState.Normal),
00408:                     new("—", AshfallDataGrid.CellState.Muted),
00409:                     new("UNBOUND", AshfallDataGrid.CellState.Muted),
00410:                     new("—", AshfallDataGrid.CellState.Muted),
00411:                     new("—", AshfallDataGrid.CellState.Muted),
00412:                 }
00413:             },
00414:             new AshfallDataGrid.Row
00415:             {
00416:                 Cells = new List<AshfallDataGrid.Cell>
00417:                 {
00418:                     new("Elena Vasquez", AshfallDataGrid.CellState.Normal),
00419:                     new("—", AshfallDataGrid.CellState.Muted),
00420:                     new("UNBOUND", AshfallDataGrid.CellState.Muted),
00421:                     new("—", AshfallDataGrid.CellState.Muted),
00422:                     new("—", AshfallDataGrid.CellState.Muted),
00423:                 }
00424:             }
00425:         };
00426:         return rows;
00427:     }
00428:
00429:     public void Open()
00430:     {
00431:         Visible = true;
00432:         RefreshView();
00433:         QueueRedraw();
00434:     }
00435:
00436:     public override void _UnhandledInput(InputEvent @event)
00437:     {
00438:         if (!Visible) return;
00439:         if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00440:         {
00441:             OnClose?.Invoke();
00442:             GetViewport().SetInputAsHandled();
00443:         }
00444:     }
00445:
00446:     public override void _ExitTree()
00447:         {
00448:             Unbind();
00449:             base._ExitTree();
00450:         }
00451: }
```

## `src/Dose/DoseRegisterSurface.cs` — 380 lines; 16,237 bytes; SHA-256 `8a2d71f77334c11072827892c266bf565b3ff248d4ef935925032abe52afb07d`
Declaration index:
- 00020: public partial class DoseRegisterSurface : PanelContainer
- 00085: private static Control MakeTab(string name, Label label)
- 00102: private static Button AddAction(Container row, string text, Action handler)
- 00109: public void BindSession(DoseLedgerHostSession session)
- 00117: private void UnbindSession()
- 00137: public void RefreshView()
- 00163: private string RenderLedger()
- 00186: private string RenderSick()
- 00210: private string RenderContent()
- 00252: private string RenderCohort()
- 00267: private string PlanLabel(string planId)
- 00275: private static string GuessLabel(string guess)
- 00286: private string RenderVoluntary()
- 00303: private void OnBookReading()
- 00311: private void OnNameToSick()
- 00318: private void OnAssignMorphine()
- 00325: private void OnBookChild()
- 00331: private void OnCorrectBaseline()
- 00353: private void OnSignVolunteer()
- 00360: private void OnCompleteVolunteer()
- 00367: private void OnCalibrate()
- 00374: private void ShowStatus(string text)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: #pragma warning disable CS8618
00004: using System.Text;
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.UI;
00008: using AtomicWar.GodotApp.UI;
00009: using CoreTheme = Ashfall.Core.UI.Theme;
00010:
00011: namespace AtomicWar.GodotApp.Dose
00012: {
00013:     /// <summary>
00014:     /// ASHFALL: THE DOSE — the player-facing Dose Register surface (PART C).
00015:     /// One folder of paperwork with four tabs (Ledger / Sick / Cohort /
00016:     /// Voluntary) and the four chaired antagonist rows (PART B) on top.
00017:     /// Thin presentation only: renders DoseLedgerHostSession state and
00018:     /// forwards one-button actions to the host's demo helpers. Zero rules.
00019:     /// </summary>
00020:     public partial class DoseRegisterSurface : PanelContainer
00021:     {
00022:         private DoseLedgerHostSession _session;
00023:         private Label _lblNpcs;
00024:         private Label _lblLedger;
00025:         private Label _lblSick;
00026:         private Label _lblCohort;
00027:         private Label _lblVoluntary;
00028:         private Label _lblContent;
00029:         private Button _btnCalibrate;
00030:
00031:         public override void _Ready()
00032:         {
00033:             SetAnchorsPreset(LayoutPreset.TopRight);
00034:             CustomMinimumSize = new Vector2(CoreTheme.PanelMaxWidth, 360);
00035:
00036:             // Apply standard panel 9-slice via shared helper (frame_9slice first)
00037:             AddThemeStyleboxOverride("panel", AshfallUiHelpers.MakePanelFrameStyleBox());
00038:
00039:             var rootVbox = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00040:             AddChild(rootVbox);
00041:
00042:             // ── Title ──
00043:             rootVbox.AddChild(AshfallUiHelpers.MakeTitle("THE DOSE REGISTER", CoreTheme.FontSizeH3));
00044:             rootVbox.AddChild(AshfallUiHelpers.MakeLabel("ONE FOLDER OF PAPERWORK"));
00045:
00046:             // ── NPC rows ──
00047:             _lblNpcs = AshfallUiHelpers.MakeSmall("The four who keep the books: ...", true);
00048:             rootVbox.AddChild(_lblNpcs);
00049:
00050:             rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00051:
00052:             // ── Tab container ──
00053:             var tabs = new TabContainer();
00054:             tabs.CustomMinimumSize = new Vector2(0, 240);
00055:             rootVbox.AddChild(tabs);
00056:
00057:             _lblLedger = new Label();
00058:             tabs.AddChild(MakeTab("Ledger", _lblLedger));
00059:             _lblSick = new Label();
00060:             tabs.AddChild(MakeTab("Sick", _lblSick));
00061:             _lblCohort = new Label();
00062:             tabs.AddChild(MakeTab("Cohort", _lblCohort));
00063:             _lblVoluntary = new Label();
00064:             tabs.AddChild(MakeTab("Voluntary", _lblVoluntary));
00065:             _lblContent = new Label();
00066:             tabs.AddChild(MakeTab("Content", _lblContent));
00067:
00068:             rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00069:
00070:             // ── Action buttons ──
00071:             var actionRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
00072:             actionRow.Alignment = BoxContainer.AlignmentMode.Center;
00073:             rootVbox.AddChild(actionRow);
00074:
00075:             AddAction(actionRow, "Book a reading", OnBookReading);
00076:             AddAction(actionRow, "Name to sick list", OnNameToSick);
00077:             AddAction(actionRow, "Assign morphine tray", OnAssignMorphine);
00078:             AddAction(actionRow, "Book a child baseline", OnBookChild);
00079:             AddAction(actionRow, "Correct baseline", OnCorrectBaseline);
00080:             AddAction(actionRow, "Sign an hour", OnSignVolunteer);
00081:             AddAction(actionRow, "Mark it done", OnCompleteVolunteer);
00082:             _btnCalibrate = AddAction(actionRow, "Calibrate (Piet)", OnCalibrate);
00083:         }
00084:
00085:         private static Control MakeTab(string name, Label label)
00086:         {
00087:             var box = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingXs);
00088:             box.Name = name;
00089:             label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00090:             label.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
00091:             label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00092:             // Plan 81 UI audit: long content (14 locations × name + description)
00093:             // must scroll rather than truncate or overflow the fixed-height tab.
00094:             var scroll = new ScrollContainer();
00095:             scroll.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00096:             scroll.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00097:             scroll.AddChild(label);
00098:             box.AddChild(scroll);
00099:             return box;
00100:         }
00101:
00102:         private static Button AddAction(Container row, string text, Action handler)
00103:         {
00104:             var button = AshfallUiHelpers.MakeButton(text, handler);
00105:             row.AddChild(button);
00106:             return button;
00107:         }
00108:
00109:         public void BindSession(DoseLedgerHostSession session)
00110:         {
00111:             UnbindSession();
00112:             _session = session;
00113:             if (_session != null)
00114:                 _session.StateChanged += RefreshView;
00115:         }
00116:
00117:         private void UnbindSession()
00118:         {
00119:             if (_session == null) return;
00120:             _session.StateChanged -= RefreshView;
00121:             _session = null!;
00122:         }
00123:
00124:         public override void _ExitTree()
00125:         {
00126:             UnbindSession();
00127:             base._ExitTree();
00128:         }
00129:
00130:         /// <summary>Rendered Content-tab text, for headless UI assertions.</summary>
00131:         internal string ContentTabText => _lblContent?.Text ?? string.Empty;
00132:
00133:         /// <summary>True when the Content tab label is wrapped in a scroll
00134:         /// container (bounds/truncation fix — content scrolls, never truncates).</summary>
00135:         internal bool ContentTabScrollable => _lblContent?.GetParent() is ScrollContainer;
00136:
00137:         public void RefreshView()
00138:         {
00139:             if (_session == null) return;
00140:
00141:             var npcSb = new StringBuilder();
00142:             for (int i = 0; i < _session.Registers.npcs.Count; i++)
00143:             {
00144:                 var n = _session.Registers.npcs[i];
00145:                 npcSb.Append(n.name).Append(" — ").Append(n.disposition).Append('\n');
00146:             }
00147:             _lblNpcs.Text = npcSb.Length > 0 ? npcSb.ToString().TrimEnd() : "The four who keep the books: absent from the register.";
00148:             _lblNpcs.TooltipText = "Book / Name / Assign / Sign. Refusing to write is a valid entry; the ledger records it as silence.";
00149:
00150:             _lblLedger.Text = RenderLedger();            _lblSick.Text = RenderSick();
00151:             _lblCohort.Text = RenderCohort();
00152:             _lblVoluntary.Text = RenderVoluntary();
00153:             _lblContent.Text = RenderContent();
00154:             _btnCalibrate.Text = _session.Ledger.State.calibrationOverdue ? "Calibrate (Piet) — OVERDUE" : "Calibrate (Piet)";
00155:
00156:             // Calibration button turns critical when overdue
00157:             if (_session.Ledger.State.calibrationOverdue)
00158:                 _btnCalibrate.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Critical));
00159:             else
00160:                 _btnCalibrate.RemoveThemeColorOverride("font_color");
00161:         }
00162:
00163:         private string RenderLedger()
00164:         {
00165:             var sb = new StringBuilder();
00166:             var l = _session.Ledger;
00167:             sb.Append(l.State.calibrationOverdue ? "CALIBRATION OVERDUE.\n" : $"Calibration: {l.State.readingsSinceLastCalibration}/{DoseLedgerSystem.ReadingsPerCalibration} readings.\n");
00168:             for (int i = 0; i < l.Entries.Count; i++)
00169:             {
00170:                 var e = l.Entries[i];
00171:                 if (e == null) continue;
00172:                 int band = DoseLedgerSystem.BandFor(e.cumulativeMsv);
00173:                 sb.Append(e.survivorId).Append(": ").Append(AshfallUiHelpers.FormatDoseMsv(e.cumulativeMsv))
00174:                   .Append(" mSv [").Append(DoseRegistersCatalogLoader.BandLabel(_session.Registers, band))
00175:                   .Append("]");
00176:                 bool flux = false;
00177:                 for (int h = 0; h < e.readingsHistory.Count; h++)
00178:                     if (e.readingsHistory[h].fluxAmbiguous) { flux = true; break; }
00179:                 if (flux)
00180:                     sb.Append(" §");
00181:                 sb.Append('\n');
00182:             }
00183:             return sb.Length == 0 ? "No tagged survivors. The pen is dry until someone seals a dosimeter." : sb.ToString();
00184:         }
00185:
00186:         private string RenderSick()
00187:         {
00188:             var sb = new StringBuilder();
00189:             for (int i = 0; i < _session.SickList.Bands.Count; i++)
00190:             {
00191:                 var b = _session.SickList.Bands[i];
00192:                 sb.Append(DoseRegistersCatalogLoader.BandLabel(_session.Registers, b.band))
00193:                   .Append(" — ").Append(b.survivorId);
00194:                 if (b.palliativePlan != null && b.palliativePlan.Length > 0)
00195:                     sb.Append(" (plan: ").Append(PlanLabel(b.palliativePlan)).Append(")");
00196:                 if (b.releaseDay >= 0)
00197:                     sb.Append(" [released day ").Append(b.releaseDay).Append("]");
00198:                 sb.Append('\n');
00199:             }
00200:             return sb.Length == 0
00201:                 ? "The bed order is empty. A Red name stays until someone writes it."
00202:                 : sb.ToString().TrimEnd();
00203:         }
00204:
00205:         /// <summary>Render the Expansion 07 content bundle — the three standing
00206:         /// Render the Expansion 07 content bundle — the standing places
00207:         /// (bunker rooms plus the Plan 81 surface/expedition/external/faction
00208:         /// geography) and the book/tool story items, so
00209:         /// the player sees what the four registers are written to serve.</summary>
00210:         private string RenderContent()
00211:         {
00212:             if (_session.Content == null) return "No dose content loaded.";
00213:             var sb = new StringBuilder();
00214:             if (_session.Content.locations != null && _session.Content.locations.Count > 0)
00215:             {
00216:                 sb.Append("Places — bunker, surface, and beyond:\n");
00217:                 for (int i = 0; i < _session.Content.locations.Count; i++)
00218:                 {
00219:                     var l = _session.Content.locations[i];
00220:                     if (l == null || string.IsNullOrEmpty(l.displayName)) continue;
00221:                     sb.Append("  • ").Append(l.displayName)
00222:                       .Append(" — ").Append(l.sector).Append('\n');
00223:                     if (!string.IsNullOrEmpty(l.description))
00224:                         sb.Append("    ").Append(l.description).Append('\n');
00225:                 }
00226:             }
00227:             if (_session.Content.items != null && _session.Content.items.Count > 0)
00228:             {
00229:                 sb.Append("\nStory / tool items:\n");
00230:                 for (int i = 0; i < _session.Content.items.Count; i++)
00231:                 {
00232:                     var it = _session.Content.items[i];
00233:                     if (it == null || string.IsNullOrEmpty(it.name)) continue;
00234:                     sb.Append("  • ").Append(it.name)
00235:                       .Append(" (").Append(it.id).Append(")\n");
00236:                 }
00237:             }
00238:             if (_session.Content.quests != null && _session.Content.quests.Count > 0)
00239:             {
00240:                 sb.Append("\nQuest lines:\n");
00241:                 for (int i = 0; i < _session.Content.quests.Count; i++)
00242:                 {
00243:                     var q = _session.Content.quests[i];
00244:                     if (q == null || string.IsNullOrEmpty(q.title)) continue;
00245:                     sb.Append("  • ").Append(q.title)
00246:                       .Append(" (").Append(q.questlineId).Append(")\n");
00247:                 }
00248:             }
00249:             return sb.Length == 0 ? "No dose content loaded." : sb.ToString().TrimEnd();
00250:         }
00251:
00252:         private string RenderCohort()
00253:         {
00254:             var sb = new StringBuilder();
00255:             for (int i = 0; i < _session.Cohort.Children.Count; i++)
00256:             {
00257:                 var c = _session.Cohort.Children[i];
00258:                 sb.Append(c.survivorId).Append(" — guess: ").Append(GuessLabel(c.guessBand))
00259:                   .Append(c.baselineCorrected ? " (corrected: " + c.trueBand + ")" : " (uncorrected)")
00260:                   .Append('\n');
00261:             }
00262:             return sb.Length == 0
00263:                 ? "The chalk board is blank. A guess is written in pencil, and the board can be erased."
00264:                 : sb.ToString().TrimEnd();
00265:         }
00266:
00267:         private string PlanLabel(string planId)
00268:         {
00269:             for (int i = 0; i < _session.Registers.plans.Count; i++)
00270:                 if (_session.Registers.plans[i].id == planId)
00271:                     return _session.Registers.plans[i].label;
00272:             return planId;
00273:         }
00274:
00275:         private static string GuessLabel(string guess)
00276:         {
00277:             switch (guess)
00278:             {
00279:                 case "low": return "Low";
00280:                 case "medium": return "Honest";
00281:                 case "high": return "High";
00282:                 default: return guess;
00283:             }
00284:         }
00285:
00286:         private string RenderVoluntary()
00287:         {
00288:             var sb = new StringBuilder();
00289:             for (int i = 0; i < _session.Voluntary.Entries.Count; i++)
00290:             {
00291:                 var e = _session.Voluntary.Entries[i];
00292:                 sb.Append(e.survivorId).Append(" — ").Append(e.task)
00293:                   .Append(e.completed ? " [done, banked " + e.doseIncurred.ToString("F1") + " mSv]" : " [open]")
00294:                   .Append('\n');
00295:             }
00296:             return sb.Length == 0
00297:                 ? "The signature list is empty. Signing an hour spends ink; the dose lands back on the ledger the moment it completes."
00298:                 : sb.ToString().TrimEnd();
00299:         }
00300:
00301:         // ── One-button actions (diegetic: Book / Name / Assign / Sign) ──
00302:
00303:         private void OnBookReading()
00304:         {
00305:             if (_session == null) return;
00306:             _session.SealDemoSurvivors();
00307:             var text = _session.ScribeReading(120f, highEnergy: true);
00308:             ShowStatus(text);
00309:         }
00310:
00311:         private void OnNameToSick()
00312:         {
00313:             if (_session == null) return;
00314:             _session.SealDemoSurvivors();
00315:             ShowStatus(_session.DiagnoseDemo(DoseLedgerSystem.BandRed));
00316:         }
00317:
00318:         private void OnAssignMorphine()
00319:         {
00320:             if (_session == null) return;
00321:             bool ok = _session.SickList.AssignPalliative("survivor_gunner_mikhail", "plan_morphine_tray");
00322:             ShowStatus(ok ? "Morphine tray assigned to the veteran." : "No such name on the sick list.");
00323:         }
00324:
00325:         private void OnBookChild()
00326:         {
00327:             if (_session == null) return;
00328:             ShowStatus(_session.BookDemoChild());
00329:         }
00330:
00331:         private void OnCorrectBaseline()
00332:         {
00333:             if (_session == null) return;
00334:             CohortChild target = null;
00335:             for (int i = 0; i < _session.Cohort.Children.Count; i++)
00336:             {
00337:                 var c = _session.Cohort.Children[i];
00338:                 if (c != null && !c.baselineCorrected && !c.isDeceased)
00339:                 {
00340:                     target = c;
00341:                     break;
00342:                 }
00343:             }
00344:             if (target == null)
00345:             {
00346:                 ShowStatus("No uncorrected child on the board.");
00347:                 return;
00348:             }
00349:             bool ok = _session.Cohort.CorrectBaseline(target.survivorId, "high");
00350:             ShowStatus(ok ? $"Baseline corrected for {target.survivorId} to high." : "Correction failed.");
00351:         }
00352:
00353:         private void OnSignVolunteer()
00354:         {
00355:             if (_session == null) return;
00356:             bool ok = _session.Voluntary.Volunteer("survivor_gunner_mikhail", "brine line inspection", 46, "Someone has to walk it.");
00357:             ShowStatus(ok ? "Veteran signed an hour." : "Already signed.");
00358:         }
00359:
00360:         private void OnCompleteVolunteer()
00361:         {
00362:             if (_session == null) return;
00363:             bool ok = _session.Voluntary.CompleteVolunteer("survivor_gunner_mikhail", "brine line inspection", 60f, 47);
00364:             ShowStatus(ok ? "Hour marked done; 60 mSv banked on the ledger." : "No open task to complete.");
00365:         }
00366:
00367:         private void OnCalibrate()
00368:         {
00369:             if (_session == null) return;
00370:             _session.Ledger.Calibrate("survivor_gunner_mikhail", 47);
00371:             ShowStatus("Calibrated. The drift resets; the drift is normal. — Piet");
00372:         }
00373:
00374:         private void ShowStatus(string text)
00375:         {
00376:             GD.Print("[DoseRegister] " + text);
00377:             RefreshView();
00378:         }
00379:     }
00380: }
```

## `Ashfall.Core.Tests/DoseRegistersCatalogTests.cs` — 128 lines; 5,676 bytes; SHA-256 `2538daa36d3bb88e10dd9689a15263cf2cbdcaf244109efa3a4e45687fea712d`
Declaration index:
- 00008: public class DoseRegistersCatalogTests
- 00010: private static string FindDataDir()
- 00026: public void Load_FindsFourBandsThreePlansThreeGuesses()
- 00043: public void Load_FindsTheFourAntagonists()
- 00068: public void Load_BandThresholdsBind()
- 00085: public void BandLabel_MapsCoreBandsToVocabulary()
- 00097: public void Load_MissingDirectoryReturnsEmptyCatalog()
- 00106: public void Characters_RegisterTheFourAntagonists()
- 00123: private class CharacterEntry
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.IO;
00003: using Ashfall.Core;
00004: using Xunit;
00005:
00006: namespace Ashfall.Core.Tests
00007: {
00008:     public class DoseRegistersCatalogTests
00009:     : CatalogTestBase{
00010:         private static string FindDataDir()
00011:         {
00012:             string dataDir = string.Empty;
00013:             string search = Directory.GetCurrentDirectory();
00014:             for (int i = 0; i < 6; i++)
00015:             {
00016:                 string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
00017:                 if (Directory.Exists(candidate)) { dataDir = candidate; break; }
00018:                 string parent = Directory.GetParent(search)?.FullName;
00019:                 if (parent == null) break;
00020:                 search = parent;
00021:             }
00022:             return dataDir;
00023:         }
00024:
00025:         [Fact]
00026:         public void Load_FindsFourBandsThreePlansThreeGuesses()
00027:         {
00028:             string dataDir = FindDataDir();
00029:             if (string.IsNullOrEmpty(dataDir)) return;
00030:
00031:             var catalog = DoseRegistersCatalogLoader.Load(
00032:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00033:             // Plan 90 expanded bands to 12 and plans to 8; assert minimums for backward compat.
00034:             Assert.True(catalog.bands.Count >= 4, $"Expected >= 4 bands, got {catalog.bands.Count}");
00035:             Assert.True(catalog.plans.Count >= 3, $"Expected >= 3 plans, got {catalog.plans.Count}");
00036:             Assert.Equal(3, catalog.guesses.Count);
00037:             Assert.Equal("band_green", catalog.bands[0].id);
00038:             // band_black is now at index 9 (after Plan 90 expansion); use Contains.
00039:             Assert.Contains(catalog.bands, b => b.id == "band_black");
00040:         }
00041:
00042:         [Fact]
00043:         public void Load_FindsTheFourAntagonists()
00044:         {
00045:             string dataDir = FindDataDir();
00046:             if (string.IsNullOrEmpty(dataDir)) return;
00047:
00048:             var catalog = DoseRegistersCatalogLoader.Load(
00049:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00050:             Assert.Equal(4, catalog.npcs.Count);
00051:             Assert.Contains(catalog.npcs, n => n.id == "npc_dr_irina_vel");
00052:             Assert.Contains(catalog.npcs, n => n.id == "npc_wyn_omah");
00053:             Assert.Contains(catalog.npcs, n => n.id == "npc_piet_abar");
00054:             Assert.Contains(catalog.npcs, n => n.id == "npc_saria_voss");
00055:             foreach (var n in catalog.npcs)
00056:             {
00057:                 Assert.False(string.IsNullOrEmpty(n.disposition));
00058:                 Assert.False(string.IsNullOrEmpty(n.action));
00059:                 // Binding parity: snake_case JSON keys must reach the DTO fields
00060:                 // (Unity's JsonUtility binds these case-insensitively; the Godot
00061:                 // serializer needs the exact snake_case names).
00062:                 Assert.False(string.IsNullOrEmpty(n.action_label),
00063:                     n.id + " action_label unbound");
00064:             }
00065:         }
00066:
00067:         [Fact]
00068:         public void Load_BandThresholdsBind()
00069:         {
00070:             string dataDir = FindDataDir();
00071:             if (string.IsNullOrEmpty(dataDir)) return;
00072:
00073:             var catalog = DoseRegistersCatalogLoader.Load(
00074:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00075:             // Look up by id — Plan 90 changed indices when 8 new bands were inserted.
00076:             var black = catalog.bands.Find(b => b.id == "band_black");
00077:             var red   = catalog.bands.Find(b => b.id == "band_red");
00078:             var amber = catalog.bands.Find(b => b.id == "band_amber");
00079:             Assert.NotNull(black); Assert.Equal(600f, black!.threshold_msv);
00080:             Assert.NotNull(red);   Assert.Equal(300f, red!.threshold_msv);
00081:             Assert.NotNull(amber); Assert.Equal(100f, amber!.threshold_msv);
00082:         }
00083:
00084:         [Fact]
00085:         public void BandLabel_MapsCoreBandsToVocabulary()
00086:         {
00087:             var catalog = DoseRegistersCatalogLoader.Load(
00088:                 FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00089:             if (catalog.bands.Count == 0) return;
00090:             Assert.Equal("Green", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandGreen));
00091:             Assert.Equal("Amber", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandAmber));
00092:             Assert.Equal("Red", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandRed));
00093:             Assert.Equal("Black", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandBlack));
00094:         }
00095:
00096:         [Fact]
00097:         public void Load_MissingDirectoryReturnsEmptyCatalog()
00098:         {
00099:             var catalog = DoseRegistersCatalogLoader.Load(
00100:                 "/nonexistent/path", new FileSystemIO(), new SystemTextJsonSerializer());
00101:             Assert.Empty(catalog.bands);
00102:             Assert.Empty(catalog.npcs);
00103:         }
00104:
00105:         [Fact]
00106:         public void Characters_RegisterTheFourAntagonists()
00107:         {
00108:             string dataDir = FindDataDir();
00109:             if (string.IsNullOrEmpty(dataDir)) return;
00110:
00111:             var fileIO = new FileSystemIO();
00112:             var json = new SystemTextJsonSerializer();
00113:             string raw = fileIO.ReadAllText(fileIO.Combine(dataDir, "characters.json"));
00114:             var chars = CatalogLocator.LoadWrappedList<CharacterEntry>(raw, SystemTextJsonSerializer.Options);
00115:             int found = 0;
00116:             foreach (var c in chars)
00117:                 if (c.id == "npc_dr_irina_vel" || c.id == "npc_wyn_omah" ||
00118:                     c.id == "npc_piet_abar" || c.id == "npc_saria_voss")
00119:                     found++;
00120:             Assert.Equal(4, found);
00121:         }
00122:
00123:         private class CharacterEntry
00124:         {
00125:             public string id = string.Empty;
00126:         }
00127:     }
00128: }
```

## `Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs` — 163 lines; 6,834 bytes; SHA-256 `0e9bf585db0e5c71b66c76ec3d26909f60f0e9a08cfe5761088ae6abb689068c`
Declaration index:
- 00012: public class Plan90DoseRegistersExpansionTests : CatalogTestBase
- 00014: private static string FindDataDir()
- 00028: private static DoseRegistersCatalog LoadCatalog()
- 00036: public void Catalog_HasTwelveBandsAndEightPlans()
- 00046: public void Bands_ThresholdsAreStrictlyIncreasing()
- 00061: public void Bands_AllIdsAreUniqueAndNonEmpty()
- 00077: public void Bands_NewEntriesExist()
- 00100: public void Plans_AllIdsAreUniqueAndNonEmpty()
- 00117: public void Plans_NewEntriesExist()
- 00136: public void Plans_CostsAreNonEmpty()
- 00150: public void BandLabel_BackwardCompatibilityForCoreFourBands()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // Plan 90 — Dose Registers Expansion: 4 bands → 12 bands, 3 plans → 8 plans
00003: using System;
00004: using System.Collections.Generic;
00005: using System.IO;
00006: using System.Linq;
00007: using Ashfall.Core;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests
00011: {
00012:     public class Plan90DoseRegistersExpansionTests : CatalogTestBase
00013:     {
00014:         private static string FindDataDir()
00015:         {
00016:             string search = Directory.GetCurrentDirectory();
00017:             for (int i = 0; i < 6; i++)
00018:             {
00019:                 string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
00020:                 if (Directory.Exists(candidate)) return candidate;
00021:                 string parent = Directory.GetParent(search)?.FullName;
00022:                 if (parent == null) break;
00023:                 search = parent;
00024:             }
00025:             return string.Empty;
00026:         }
00027:
00028:         private static DoseRegistersCatalog LoadCatalog()
00029:         {
00030:             string dataDir = FindDataDir();
00031:             if (string.IsNullOrEmpty(dataDir)) return new DoseRegistersCatalog();
00032:             return DoseRegistersCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00033:         }
00034:
00035:         [Fact]
00036:         public void Catalog_HasTwelveBandsAndEightPlans()
00037:         {
00038:             var catalog = LoadCatalog();
00039:             if (catalog.bands.Count == 0) return; // data dir not available
00040:
00041:             Assert.Equal(12, catalog.bands.Count);
00042:             Assert.Equal(8, catalog.plans.Count);
00043:         }
00044:
00045:         [Fact]
00046:         public void Bands_ThresholdsAreStrictlyIncreasing()
00047:         {
00048:             var catalog = LoadCatalog();
00049:             if (catalog.bands.Count == 0) return;
00050:
00051:             for (int i = 1; i < catalog.bands.Count; i++)
00052:             {
00053:                 Assert.True(
00054:                     catalog.bands[i].threshold_msv > catalog.bands[i - 1].threshold_msv,
00055:                     $"Band[{i}] '{catalog.bands[i].id}' threshold {catalog.bands[i].threshold_msv} mSv " +
00056:                     $"is not > Band[{i-1}] '{catalog.bands[i-1].id}' threshold {catalog.bands[i-1].threshold_msv} mSv");
00057:             }
00058:         }
00059:
00060:         [Fact]
00061:         public void Bands_AllIdsAreUniqueAndNonEmpty()
00062:         {
00063:             var catalog = LoadCatalog();
00064:             if (catalog.bands.Count == 0) return;
00065:
00066:             var ids = new HashSet<string>();
00067:             foreach (var band in catalog.bands)
00068:             {
00069:                 Assert.False(string.IsNullOrEmpty(band.id), "Band has empty id");
00070:                 Assert.False(string.IsNullOrEmpty(band.label), $"Band '{band.id}' has empty label");
00071:                 Assert.False(string.IsNullOrEmpty(band.disposition), $"Band '{band.id}' has empty disposition");
00072:                 Assert.True(ids.Add(band.id), $"Duplicate band id: {band.id}");
00073:             }
00074:         }
00075:
00076:         [Fact]
00077:         public void Bands_NewEntriesExist()
00078:         {
00079:             var catalog = LoadCatalog();
00080:             if (catalog.bands.Count == 0) return;
00081:
00082:             // 4 original bands still present
00083:             Assert.Contains(catalog.bands, b => b.id == "band_green");
00084:             Assert.Contains(catalog.bands, b => b.id == "band_amber");
00085:             Assert.Contains(catalog.bands, b => b.id == "band_red");
00086:             Assert.Contains(catalog.bands, b => b.id == "band_black");
00087:
00088:             // 8 new bands from Plan 90
00089:             Assert.Contains(catalog.bands, b => b.id == "band_white");
00090:             Assert.Contains(catalog.bands, b => b.id == "band_yellow");
00091:             Assert.Contains(catalog.bands, b => b.id == "band_orange");
00092:             Assert.Contains(catalog.bands, b => b.id == "band_rose");
00093:             Assert.Contains(catalog.bands, b => b.id == "band_crimson");
00094:             Assert.Contains(catalog.bands, b => b.id == "band_violet");
00095:             Assert.Contains(catalog.bands, b => b.id == "band_indigo");
00096:             Assert.Contains(catalog.bands, b => b.id == "band_void");
00097:         }
00098:
00099:         [Fact]
00100:         public void Plans_AllIdsAreUniqueAndNonEmpty()
00101:         {
00102:             var catalog = LoadCatalog();
00103:             if (catalog.plans.Count == 0) return;
00104:
00105:             var ids = new HashSet<string>();
00106:             foreach (var plan in catalog.plans)
00107:             {
00108:                 Assert.False(string.IsNullOrEmpty(plan.id), "Plan has empty id");
00109:                 Assert.False(string.IsNullOrEmpty(plan.label), $"Plan '{plan.id}' has empty label");
00110:                 Assert.False(string.IsNullOrEmpty(plan.cost), $"Plan '{plan.id}' has empty cost");
00111:                 Assert.False(string.IsNullOrEmpty(plan.note), $"Plan '{plan.id}' has empty note");
00112:                 Assert.True(ids.Add(plan.id), $"Duplicate plan id: {plan.id}");
00113:             }
00114:         }
00115:
00116:         [Fact]
00117:         public void Plans_NewEntriesExist()
00118:         {
00119:             var catalog = LoadCatalog();
00120:             if (catalog.plans.Count == 0) return;
00121:
00122:             // 3 original plans
00123:             Assert.Contains(catalog.plans, p => p.id == "plan_morphine_tray");
00124:             Assert.Contains(catalog.plans, p => p.id == "plan_comfort_rounds");
00125:             Assert.Contains(catalog.plans, p => p.id == "plan_nothing");
00126:
00127:             // 5 new plans from Plan 90
00128:             Assert.Contains(catalog.plans, p => p.id == "plan_chelation");
00129:             Assert.Contains(catalog.plans, p => p.id == "plan_iodine_prophylaxis");
00130:             Assert.Contains(catalog.plans, p => p.id == "plan_isolation");
00131:             Assert.Contains(catalog.plans, p => p.id == "plan_rest");
00132:             Assert.Contains(catalog.plans, p => p.id == "plan_transfer");
00133:         }
00134:
00135:         [Fact]
00136:         public void Plans_CostsAreNonEmpty()
00137:         {
00138:             var catalog = LoadCatalog();
00139:             if (catalog.plans.Count == 0) return;
00140:
00141:             foreach (var plan in catalog.plans)
00142:             {
00143:                 // Valid cost values: an item id, "time", or "none"
00144:                 Assert.False(string.IsNullOrWhiteSpace(plan.cost),
00145:                     $"Plan '{plan.id}' has null/empty cost");
00146:             }
00147:         }
00148:
00149:         [Fact]
00150:         public void BandLabel_BackwardCompatibilityForCoreFourBands()
00151:         {
00152:             // The DoseLedgerSystem hardcodes BandGreen=0, BandAmber=1, BandRed=2, BandBlack=3.
00153:             // BandIdFor() maps these integers to band ids by lookup — confirm still resolves correctly.
00154:             var catalog = LoadCatalog();
00155:             if (catalog.bands.Count == 0) return;
00156:
00157:             Assert.Equal("Green",  DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandGreen));
00158:             Assert.Equal("Amber",  DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandAmber));
00159:             Assert.Equal("Red",    DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandRed));
00160:             Assert.Equal("Black",  DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandBlack));
00161:         }
00162:     }
00163: }
```

## `Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs` — 127 lines; 5,777 bytes; SHA-256 `8aa74cc5c553e524d4cfda7475652564dd5ec0d9eadd524afb6f9ccea46f464e`
Declaration index:
- 00014: public sealed class Plan90_78DoseInksIntegrationTests : CatalogTestBase
- 00016: private static string FindDataDir()
- 00033: public void DoseRegisters_HasTwelveBandsAndEightPlansWithFourNpcs()
- 00048: public void DoseRegisters_BandsAreStrictlyIncreasingAndBackwardCompatible()
- 00075: public void ArchiveInks_HasTwelveEntriesWithUniqueIds()
- 00104: public void CrossSystem_DoseAndInkCatalogsLoadIndependently()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // Plan 90 × Plan 78 cross-system integration test
00003: // Plan 90 — Dose Register Bands & Plans Expansion (4 bands → 12 bands, 3 plans → 8 plans)
00004: // Plan 78 — Archive Inks Expansion (3 → 12 ink types) [data already complete]
00005: using System;
00006: using System.Collections.Generic;
00007: using System.IO;
00008: using System.Linq;
00009: using Ashfall.Core;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.Culture
00013: {
00014:     public sealed class Plan90_78DoseInksIntegrationTests : CatalogTestBase
00015:     {
00016:         private static string FindDataDir()
00017:         {
00018:             string search = Directory.GetCurrentDirectory();
00019:             for (int i = 0; i < 6; i++)
00020:             {
00021:                 string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
00022:                 if (Directory.Exists(candidate)) return candidate;
00023:                 string parent = Directory.GetParent(search)?.FullName;
00024:                 if (parent == null) break;
00025:                 search = parent;
00026:             }
00027:             return string.Empty;
00028:         }
00029:
00030:         // ── Dose Registers (Plan 90) ──────────────────────────────────────────
00031:
00032:         [Fact]
00033:         public void DoseRegisters_HasTwelveBandsAndEightPlansWithFourNpcs()
00034:         {
00035:             string dataDir = FindDataDir();
00036:             if (string.IsNullOrEmpty(dataDir)) return;
00037:
00038:             var catalog = DoseRegistersCatalogLoader.Load(
00039:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00040:
00041:             Assert.Equal(12, catalog.bands.Count);
00042:             Assert.Equal(8, catalog.plans.Count);
00043:             Assert.Equal(3, catalog.guesses.Count);
00044:             Assert.Equal(4, catalog.npcs.Count);
00045:         }
00046:
00047:         [Fact]
00048:         public void DoseRegisters_BandsAreStrictlyIncreasingAndBackwardCompatible()
00049:         {
00050:             string dataDir = FindDataDir();
00051:             if (string.IsNullOrEmpty(dataDir)) return;
00052:
00053:             var catalog = DoseRegistersCatalogLoader.Load(
00054:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00055:
00056:             // Strictly increasing thresholds
00057:             for (int i = 1; i < catalog.bands.Count; i++)
00058:             {
00059:                 Assert.True(
00060:                     catalog.bands[i].threshold_msv > catalog.bands[i - 1].threshold_msv,
00061:                     $"Band '{catalog.bands[i].id}' threshold {catalog.bands[i].threshold_msv} mSv " +
00062:                     $"not > '{catalog.bands[i-1].id}' threshold {catalog.bands[i-1].threshold_msv} mSv");
00063:             }
00064:
00065:             // The 4 hardcoded DoseLedgerSystem band integers must still resolve
00066:             Assert.Equal("Green", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandGreen));
00067:             Assert.Equal("Amber", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandAmber));
00068:             Assert.Equal("Red",   DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandRed));
00069:             Assert.Equal("Black", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandBlack));
00070:         }
00071:
00072:         // ── Archive Inks (Plan 78) ────────────────────────────────────────────
00073:
00074:         [Fact]
00075:         public void ArchiveInks_HasTwelveEntriesWithUniqueIds()
00076:         {
00077:             string dataDir = FindDataDir();
00078:             if (string.IsNullOrEmpty(dataDir)) return;
00079:
00080:             var inks = ArchiveInkCatalogLoader.Load(
00081:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00082:
00083:             Assert.Equal(12, inks.Count);
00084:
00085:             var ids = new HashSet<string>();
00086:             foreach (var ink in inks)
00087:             {
00088:                 Assert.False(string.IsNullOrEmpty(ink.ink_id),
00089:                     $"Archive ink has empty ink_id");
00090:                 Assert.True(ids.Add(ink.ink_id),
00091:                     $"Duplicate archive ink id: {ink.ink_id}");
00092:                 // Legibility in valid range
00093:                 Assert.True(ink.legibilityScore >= 0.1f && ink.legibilityScore <= 1.0f,
00094:                     $"Ink '{ink.ink_id}' legibilityScore {ink.legibilityScore} out of range [0.1, 1.0]");
00095:                 // Required item id must be non-empty
00096:                 Assert.False(string.IsNullOrEmpty(ink.requiredItemId),
00097:                     $"Ink '{ink.ink_id}' has empty requiredItemId");
00098:             }
00099:         }
00100:
00101:         // ── Cross-system coherence ────────────────────────────────────────────
00102:
00103:         [Fact]
00104:         public void CrossSystem_DoseAndInkCatalogsLoadIndependently()
00105:         {
00106:             string dataDir = FindDataDir();
00107:             if (string.IsNullOrEmpty(dataDir)) return;
00108:
00109:             // Both catalogs must load cleanly in the same data directory context
00110:             var doseCatalog = DoseRegistersCatalogLoader.Load(
00111:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00112:             var inks = ArchiveInkCatalogLoader.Load(
00113:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00114:
00115:             Assert.True(doseCatalog.bands.Count >= 12,
00116:                 $"Dose catalog must have >= 12 bands; got {doseCatalog.bands.Count}");
00117:             Assert.True(doseCatalog.plans.Count >= 8,
00118:                 $"Dose catalog must have >= 8 plans; got {doseCatalog.plans.Count}");
00119:             Assert.True(inks.Count >= 12,
00120:                 $"Archive ink catalog must have >= 12 inks; got {inks.Count}");
00121:
00122:             // Both catalogs share the radiation management pillar — neither
00123:             // should interfere with the other's data directory resolution.
00124:             Assert.Equal(4, doseCatalog.npcs.Count);
00125:         }
00126:     }
00127: }
```

## `docs/MEDICAL_DOSE_TREATMENT_MATRIX.md` — 17 lines; 1,427 bytes; SHA-256 `aecd9cb61064b72176dcf9231fe49c048c69669f9cdc0e9f070f7f7d87546961`
```csharp
00001: # Medical dose-treatment matrix
00002:
00003: | Treatment | Item | Mechanical scope | Research | Schedule | Authority |
00004: |---|---|---|---|---|---|
00005: | Potassium iodide | `iodine_pills` | invokes survivor radiation iodine protection; does not erase dose | existing radiation treatment availability | immediate | `RadiationSicknessAfflictionHandler` → `SurvivorsHostSession` |
00006: | Anti-rad / chelation | `rad_away` | removes the existing acute/current radiation dose through `RadiationSystem`; does not write lifetime exposure or the separate dose ledger | `knowledge_chelation_therapy` when the shared capability provider is bound | immediate | `RadiationSicknessAfflictionHandler` → `RadiationSystem` |
00007: | Oxygen support | `item_oxygen_supply` | applies the existing respiratory relief handler | none | 24 game hours | `RespiratoryAfflictionHandler` → `RespiratoryDegenerationSystem` |
00008:
00009: The anti-rad path is intentionally not presented as generic erasure of
00010: absorbed/lifetime dose: the host callback calls `AdministerAntiRad`, whose
00011: scope is the current bounded acute dose field. The treatment item is consumed
00012: by the pipeline before the handler applies the effect; a bound production
00013: research query gates the chelation treatment without duplicating research state
00014: in medical saves.
00015:
00016: The existing dose ledger and radiation progression remain separate authorities.
00017: No new cumulative-dose reset or parallel pharmacology state was introduced.
```

## `Ashfall.Core.Tests/DoseLedgerSystemTests.cs` — 201 lines; 8,564 bytes; SHA-256 `e983e95d8bc0ef4c41532ae7d17735990ffe850c3259a92b892e4b0d9ac069bc`
Declaration index:
- 00008: public class DoseLedgerSystemTests
- 00011: public void BookReading_WithoutTag_IsNotBooked()
- 00020: public void BookReading_CrossesAmberBand_AndFiresEvent()
- 00033: public void AntiRadAfter_ReducesBookedDose()
- 00042: public void FluxAmbiguity_IsDeterministicPerSeed()
- 00056: public void CaptureRestore_RoundTrips()
- 00070: public void ForgedCleanBill_ProvidesGreenBandAdministratively_WithoutMutatingPhysicalDose()
- 00090: public void AdminOverrideBand_ChangesAdminBand_PreservesBookedDose()
- 00104: public void CaptureRestore_PreservesForgedAndOverrideState()
- 00128: public void TamperedSave_IsHardRejectedOnDecode()
- 00153: public void ChecksumlessSave_IsRejectedOnDecode()
- 00168: public class CohortAndVolunteerTests
- 00171: public void BookChild_ThenCorrectBaseline()
- 00182: public void Volunteer_SignAndComplete_BanksDose()
- 00192: internal sealed class SeededRng : ISeededRng
- 00197: public int Next(int min, int max) => _rng.Next(min, max);
- 00198: public float NextFloat() => (float)_rng.NextDouble();
- 00199: public double NextDouble() => _rng.NextDouble();
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core;
00004: using Xunit;
00005:
00006: namespace Ashfall.Core.Tests
00007: {
00008:     public class DoseLedgerSystemTests
00009:     {
00010:         [Fact]
00011:         public void BookReading_WithoutTag_IsNotBooked()
00012:         {
00013:             var dl = new DoseLedgerSystem();
00014:             var result = dl.BookReading("sv_x", 1, 50f, "sampling", false, false, false, new SeededRng(1));
00015:             Assert.Equal(DoseBandResult.NoEntry, result);
00016:             Assert.Equal(0f, dl.GetCumulative("sv_x"));
00017:         }
00018:
00019:         [Fact]
00020:         public void BookReading_CrossesAmberBand_AndFiresEvent()
00021:         {
00022:             var dl = new DoseLedgerSystem();
00023:             dl.AssignDosimeter("sv_x", "tag1");
00024:             int band = -1;
00025:             dl.OnBandReached += (id, b) => band = b;
00026:
00027:             dl.BookReading("sv_x", 1, 120f, "sampling", false, false, false, new SeededRng(1));
00028:             Assert.Equal(DoseLedgerSystem.BandAmber, band);
00029:             Assert.Equal(DoseLedgerSystem.BandAmber, DoseLedgerSystem.BandFor(dl.GetCumulative("sv_x")));
00030:         }
00031:
00032:         [Fact]
00033:         public void AntiRadAfter_ReducesBookedDose()
00034:         {
00035:             var dl = new DoseLedgerSystem();
00036:             dl.AssignDosimeter("sv_x", "tag1");
00037:             dl.BookReading("sv_x", 1, 100f, "x", false, false, true, new SeededRng(1)); // anti-rad after
00038:             Assert.True(dl.GetCumulative("sv_x") < 60.01f); // 100 * 0.6 = 60
00039:         }
00040:
00041:         [Fact]
00042:         public void FluxAmbiguity_IsDeterministicPerSeed()
00043:         {
00044:             var a = new DoseLedgerSystem();
00045:             a.AssignDosimeter("sv_a", "t");
00046:             var b = new DoseLedgerSystem();
00047:             b.AssignDosimeter("sv_b", "t");
00048:
00049:             a.BookReading("sv_a", 1, 80f, "storm", true, false, false, new SeededRng(7));
00050:             b.BookReading("sv_b", 1, 80f, "storm", true, false, false, new SeededRng(7));
00051:
00052:             Assert.Equal(a.GetCumulative("sv_a"), b.GetCumulative("sv_b"));
00053:         }
00054:
00055:         [Fact]
00056:         public void CaptureRestore_RoundTrips()
00057:         {
00058:             var dl = new DoseLedgerSystem();
00059:             dl.AssignDosimeter("sv_x", "tag1", 20f);
00060:             dl.BookReading("sv_x", 1, 200f, "sampling", false, false, true, new SeededRng(1));
00061:
00062:             var state = dl.CaptureState();
00063:             var dlB = new DoseLedgerSystem();
00064:             dlB.RestoreState(state);
00065:
00066:             Assert.Equal(dl.GetCumulative("sv_x"), dlB.GetCumulative("sv_x"));
00067:         }
00068:
00069:         [Fact]
00070:         public void ForgedCleanBill_ProvidesGreenBandAdministratively_WithoutMutatingPhysicalDose()
00071:         {
00072:             var dl = new DoseLedgerSystem();
00073:             dl.AssignDosimeter("sv_scavenger", "tag_9");
00074:             // Book 350 mSv (Red band)
00075:             dl.BookReading("sv_scavenger", 10, 350f, "reactor_scavenge", false, false, false, new SeededRng(1));
00076:
00077:             Assert.Equal(DoseLedgerSystem.BandRed, DoseLedgerSystem.BandFor(dl.GetCumulative("sv_scavenger")));
00078:             Assert.Equal(DoseLedgerSystem.BandRed, dl.GetAdministrativeBand("sv_scavenger"));
00079:
00080:             // Issue forged clean-bill chit
00081:             dl.SetForgedCleanBill("sv_scavenger", true);
00082:
00083:             // True cumulative dose is unchanged (350 mSv)
00084:             Assert.Equal(350f, dl.GetCumulative("sv_scavenger"));
00085:             // But administrative clearance check reports Green band
00086:             Assert.Equal(DoseLedgerSystem.BandGreen, dl.GetAdministrativeBand("sv_scavenger"));
00087:         }
00088:
00089:         [Fact]
00090:         public void AdminOverrideBand_ChangesAdminBand_PreservesBookedDose()
00091:         {
00092:             var dl = new DoseLedgerSystem();
00093:             dl.AssignDosimeter("sv_worker", "tag_12");
00094:             dl.BookReading("sv_worker", 5, 150f, "ash_fallout", false, false, false, new SeededRng(2));
00095:
00096:             Assert.Equal(DoseLedgerSystem.BandAmber, dl.GetAdministrativeBand("sv_worker"));
00097:
00098:             dl.SetAdministrativeClassificationOverride("sv_worker", "band_black");
00099:             Assert.Equal(DoseLedgerSystem.BandBlack, dl.GetAdministrativeBand("sv_worker"));
00100:             Assert.Equal(150f, dl.GetCumulative("sv_worker"));
00101:         }
00102:
00103:         [Fact]
00104:         public void CaptureRestore_PreservesForgedAndOverrideState()
00105:         {
00106:             var dl = new DoseLedgerSystem();
00107:             dl.AssignDosimeter("sv_a", "tag_a", 10f);
00108:             dl.SetForgedCleanBill("sv_a", true);
00109:             dl.SetAdministrativeClassificationOverride("sv_a", "band_green");
00110:
00111:             var state = dl.CaptureState();
00112:             var dlB = new DoseLedgerSystem();
00113:             dlB.RestoreState(state);
00114:
00115:             var entryB = dlB.GetEntry("sv_a");
00116:             Assert.NotNull(entryB);
00117:             Assert.True(entryB.hasForgedCleanBill);
00118:             Assert.Equal("band_green", entryB.administrativeClassificationOverride);
00119:             Assert.Equal(DoseLedgerSystem.BandGreen, dlB.GetAdministrativeBand("sv_a"));
00120:         }
00121:
00122:         /// <summary>
00123:         /// Cross-host integrity (Invariant 3) gate: a save whose data is mutated
00124:         /// after checksumming must be hard-rejected on decode as a checksum
00125:         /// mismatch — never silently restored into a half-trusted state.
00126:         /// </summary>
00127:         [Fact]
00128:         public void TamperedSave_IsHardRejectedOnDecode()
00129:         {
00130:             var json = new SystemTextJsonSerializer();
00131:             var dl = new DoseLedgerSystem();
00132:             dl.AssignDosimeter("sv_x", "tag1", 20f);
00133:             dl.BookReading("sv_x", 1, 200f, "sampling", false, false, true, new SeededRng(1));
00134:
00135:             var save = DoseLedgerSaveCodec.Capture(1, dl, new SickListSystem(), new CohortSystem(), new VoluntaryRegisterSystem());
00136:             string valid = json.Serialize(save);            // trusted payload
00137:
00138:             // Round-trip of the unmodified payload must succeed.
00139:             var decoded = DoseLedgerSaveCodec.Decode(valid, json);
00140:             Assert.Equal(save.Checksum, decoded.Checksum);
00141:
00142:             // Forge: reload, shift a data field, re-serialize WITHOUT recomputing.
00143:             var altered = json.Deserialize<DoseLedgerSave>(valid);
00144:             altered.simDay += 7;
00145:             string forged = json.Serialize(altered);
00146:
00147:             var ex = Assert.Throws<InvalidOperationException>(() => DoseLedgerSaveCodec.Decode(forged, json));
00148:             Assert.Contains("checksum mismatch", ex.Message);
00149:         }
00150:
00151:         /// <summary>A save with no checksum at all is a truncated/foreign file → reject.</summary>
00152:         [Fact]
00153:         public void ChecksumlessSave_IsRejectedOnDecode()
00154:         {
00155:             var json = new SystemTextJsonSerializer();
00156:             var dl = new DoseLedgerSystem();
00157:             dl.AssignDosimeter("sv_x", "tag1", 20f);
00158:
00159:             var save = DoseLedgerSaveCodec.Capture(1, dl, new SickListSystem(), new CohortSystem(), new VoluntaryRegisterSystem());
00160:             save.Checksum = string.Empty;
00161:             string noChecksum = json.Serialize(save);
00162:
00163:             var ex = Assert.Throws<InvalidOperationException>(() => DoseLedgerSaveCodec.Decode(noChecksum, json));
00164:             Assert.Contains("no checksum", ex.Message);
00165:         }
00166:     }
00167:
00168:     public class CohortAndVolunteerTests
00169:     {
00170:         [Fact]
00171:         public void BookChild_ThenCorrectBaseline()
00172:         {
00173:             var cohort = new CohortSystem();
00174:             Assert.True(cohort.BookChild("sv_child", new[] { "sv_a", "sv_b" }, "low", 12, "told a kind number"));
00175:             Assert.False(cohort.BookChild("sv_child", null, "low", 12)); // booked twice refused
00176:             Assert.True(cohort.CorrectBaseline("sv_child", "high"));
00177:             Assert.True(cohort.GetChild("sv_child").baselineCorrected);
00178:             Assert.Equal("high", cohort.GetChild("sv_child").trueBand);
00179:         }
00180:
00181:         [Fact]
00182:         public void Volunteer_SignAndComplete_BanksDose()
00183:         {
00184:             var v = new VoluntaryRegisterSystem();
00185:             Assert.True(v.Volunteer("sv_a", "vented reactor", 20, "I worked the corridor before."));
00186:             Assert.True(v.CompleteVolunteer("sv_a", "vented reactor", 250f, 21));
00187:             Assert.False(v.CompleteVolunteer("sv_a", "vented reactor", 0f, 22)); // already done
00188:             Assert.Equal(250f, v.GetEntry("sv_a", "vented reactor").doseIncurred);
00189:         }
00190:     }
00191:
00192:     internal sealed class SeededRng : ISeededRng
00193:     {
00194:         private readonly System.Random _rng;
00195:         public int Seed { get; }
00196:         public SeededRng(int seed) { Seed = seed; _rng = new System.Random(seed); }
00197:         public int Next(int min, int max) => _rng.Next(min, max);
00198:         public float NextFloat() => (float)_rng.NextDouble();
00199:         public double NextDouble() => _rng.NextDouble();
00200:     }
00201: }
```

## `Ashfall.Core.Tests/Medical/DiseaseTriageBridgeTests.cs` — 286 lines; 12,310 bytes; SHA-256 `fa3d06b9ef1bce90ceaefa17d177c96331c2b774aa4046f47998b34d5c647fb0`
Declaration index:
- 00017: public class DiseaseTriageBridgeTests
- 00021: private static string DataDir()
- 00029: private static DiseaseCatalog LoadCatalog() =>
- 00032: private static DiseaseDefinition Def(
- 00048: public void StageOf_DerivesIncubationIllnessTerminalFromCatalogBounds()
- 00063: public void StageOf_NegativeDays_IsNeverAPrognosis(int daysSick)
- 00072: public void TerminalPrognosis_NeverAppliesToLowLethalityDisease()
- 00083: public void TerminalPrognosis_KeepsOneDayOfAcuteTreatmentRoom()
- 00097: public void SickBandFor_MonotonicThroughIllnessToOutcomePending()
- 00112: public void ShouldNameToSickList_IncubationIsAQuarantineQuestionNotTriage()
- 00122: public void PalliativePlan_UsesAuthoredRegisterPlanIdsOnly()
- 00138: private static List<string> RegisterPlanIds()
- 00151: public void AuthoredCatalog_ProducesSaneStagesBandsAndPlans()
- 00196: public void Diagnose_LegacySignatureStaysDoseSourced()
- 00207: public void Diagnose_IllnessSourceRecordsDiseaseProvenance()
- 00219: public void SickList_NewFieldsRoundTripThroughCapture()
- 00237: public void SickList_PreD5RowsLoadAsDoseSourced()
- 00257: public void Release_OnlyTouchesRowsTheDiseaseBridgeNamed()
- 00273: public void CaptureState_IsOrdinalAndIndependentOfLiveState()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // Plan 60 / D1 + D5 — clinical stage, band and palliative plan are DERIVED from
00003: // the authored catalog, and the sick list records which authority named a row.
00004: // Before this, ResolveOutcomes ignored everything but illness_days + lethality,
00005: // the sick list only ever meant "dose band", and palliativePlan had no writer.
00006: // These tests pin the derived model and reject an invented parallel timeline.
00007: using System;
00008: using System.Collections.Generic;
00009: using System.IO;
00010: using System.Linq;
00011: using Ashfall.Core;
00012: using Ashfall.Core.Disease;
00013: using Xunit;
00014:
00015: namespace Ashfall.Core.Tests.Medical
00016: {
00017:     public class DiseaseTriageBridgeTests
00018:     {
00019:         // ── fixture helpers ────────────────────────────────────────────
00020:
00021:         private static string DataDir()
00022:         {
00023:             string start = Directory.GetCurrentDirectory();
00024:             if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
00025:             if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found)) return found;
00026:             throw new DirectoryNotFoundException("data authority not found from " + start);
00027:         }
00028:
00029:         private static DiseaseCatalog LoadCatalog() =>
00030:             DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00031:
00032:         private static DiseaseDefinition Def(
00033:             int incubation = 2, int illness = 8, float lethality = 0.4f) =>
00034:             new DiseaseDefinition
00035:             {
00036:                 id = "disease_test_fixture",
00037:                 display_name = "Fixture illness",
00038:                 vector = DiseaseVectorNames.Water,
00039:                 incubation_days = incubation,
00040:                 illness_days = illness,
00041:                 lethality = lethality,
00042:                 infectivity = 0.3f,
00043:             };
00044:
00045:         // ── D1: stages are derived, never authored ────────────────────
00046:
00047:         [Fact]
00048:         public void StageOf_DerivesIncubationIllnessTerminalFromCatalogBounds()
00049:         {
00050:             var def = Def(incubation: 2, illness: 8, lethality: 0.6f);
00051:
00052:             Assert.Equal(DiseaseClinicalStage.None, DiseaseTriage.StageOf(null, 5));
00053:             Assert.Equal(DiseaseClinicalStage.Incubating, DiseaseTriage.StageOf(def, 0));
00054:             Assert.Equal(DiseaseClinicalStage.Incubating, DiseaseTriage.StageOf(def, 1));
00055:             Assert.Equal(DiseaseClinicalStage.Ill, DiseaseTriage.StageOf(def, 2));
00056:             Assert.Equal(DiseaseClinicalStage.Terminal, DiseaseTriage.StageOf(def, 7));
00057:             Assert.Equal(DiseaseClinicalStage.OutcomePending, DiseaseTriage.StageOf(def, 8));
00058:         }
00059:
00060:         [Theory]
00061:         [InlineData(-5)]
00062:         [InlineData(int.MinValue)]
00063:         public void StageOf_NegativeDays_IsNeverAPrognosis(int daysSick)
00064:         {
00065:             Assert.Equal(DiseaseClinicalStage.None, DiseaseTriage.StageOf(Def(), daysSick));
00066:             Assert.False(DiseaseTriage.IsTerminalPrognosis(Def(), daysSick));
00067:             Assert.Null(DiseaseTriage.PalliativePlanFor(Def(), daysSick));
00068:             Assert.Equal(DoseLedgerSystem.BandGreen, DiseaseTriage.SickBandFor(Def(), daysSick));
00069:         }
00070:
00071:         [Fact]
00072:         public void TerminalPrognosis_NeverAppliesToLowLethalityDisease()
00073:         {
00074:             // Self-limiting: 10% lethal. Late illness is still not a comfort-care case.
00075:             var mild = Def(incubation: 1, illness: 10, lethality: 0.10f);
00076:
00077:             Assert.False(DiseaseTriage.IsTerminalPrognosis(mild, 10));
00078:             Assert.Null(DiseaseTriage.PalliativePlanFor(mild, 9));
00079:             Assert.Equal(DiseaseClinicalStage.Ill, DiseaseTriage.StageOf(mild, 9));
00080:         }
00081:
00082:         [Fact]
00083:         public void TerminalPrognosis_KeepsOneDayOfAcuteTreatmentRoom()
00084:         {
00085:             // Even a maximally lethal disease must be treatable as acute for at
00086:             // least one ill day before comfort care becomes the honest plan.
00087:             var harsh = Def(incubation: 0, illness: 1, lethality: 0.9f);
00088:
00089:             Assert.Equal(DiseaseClinicalStage.Ill, DiseaseTriage.StageOf(harsh, 0));
00090:             Assert.Equal(DiseaseClinicalStage.OutcomePending, DiseaseTriage.StageOf(harsh, 1));
00091:             Assert.False(DiseaseTriage.IsTerminalPrognosis(harsh, 0));
00092:         }
00093:
00094:         // ── D5: one band ladder, illness-named rows ────────────────────
00095:
00096:         [Fact]
00097:         public void SickBandFor_MonotonicThroughIllnessToOutcomePending()
00098:         {
00099:             var def = Def(incubation: 2, illness: 10, lethality: 0.8f);
00100:
00101:             int ill = DiseaseTriage.SickBandFor(def, 3);
00102:             int terminal = DiseaseTriage.SickBandFor(def, 9);
00103:             int pending = DiseaseTriage.SickBandFor(def, 10);
00104:
00105:             Assert.Equal(DoseLedgerSystem.BandAmber, ill);
00106:             Assert.Equal(DoseLedgerSystem.BandRed, terminal);
00107:             Assert.Equal(DoseLedgerSystem.BandBlack, pending);
00108:             Assert.True(ill < terminal && terminal < pending);
00109:         }
00110:
00111:         [Fact]
00112:         public void ShouldNameToSickList_IncubationIsAQuarantineQuestionNotTriage()
00113:         {
00114:             var def = Def(incubation: 3, illness: 9, lethality: 0.9f);
00115:
00116:             Assert.False(DiseaseTriage.ShouldNameToSickList(def, 2));
00117:             Assert.True(DiseaseTriage.ShouldNameToSickList(def, 3));
00118:             Assert.False(DiseaseTriage.ShouldNameToSickList(null, 5));
00119:         }
00120:
00121:         [Fact]
00122:         public void PalliativePlan_UsesAuthoredRegisterPlanIdsOnly()
00123:         {
00124:             var heavy = Def(incubation: 1, illness: 8, lethality: 0.9f);
00125:             var light = Def(incubation: 1, illness: 8, lethality: 0.3f);
00126:
00127:             Assert.Equal(DiseaseTriage.Plans.MorphineTray,
00128:                 DiseaseTriage.PalliativePlanFor(heavy, 7));
00129:             Assert.Equal(DiseaseTriage.Plans.ComfortRounds,
00130:                 DiseaseTriage.PalliativePlanFor(light, 7));
00131:
00132:             // Every plan the triage can emit must exist in the register authority.
00133:             var registerIds = RegisterPlanIds();
00134:             Assert.Contains(DiseaseTriage.Plans.MorphineTray, registerIds);
00135:             Assert.Contains(DiseaseTriage.Plans.ComfortRounds, registerIds);
00136:         }
00137:
00138:         private static List<string> RegisterPlanIds()
00139:         {
00140:             var doc = System.Text.Json.JsonDocument.Parse(
00141:                 new FileSystemIO().ReadAllText(Path.Combine(DataDir(), "dose_registers.json")));
00142:             var ids = new List<string>();
00143:             foreach (var p in doc.RootElement.GetProperty("plans").EnumerateArray())
00144:                 ids.Add(p.GetProperty("id").GetString());
00145:             return ids;
00146:         }
00147:
00148:         // ── the real catalog must survive the derived model ────────────
00149:
00150:         [Fact]
00151:         public void AuthoredCatalog_ProducesSaneStagesBandsAndPlans()
00152:         {
00153:             var catalog = LoadCatalog();
00154:             var diseases = catalog.Diseases;
00155:             Assert.NotEmpty(diseases);
00156:
00157:             int terminalCapable = 0;
00158:             foreach (var def in diseases)
00159:             {
00160:                 Assert.NotNull(def.id);
00161:
00162:                 // Stage is monotonic in days for every authored disease.
00163:                 var previous = DiseaseTriage.StageOf(def, 0);
00164:                 for (int d = 1; d <= def.illness_days + 2; d++)
00165:                 {
00166:                     var now = DiseaseTriage.StageOf(def, d);
00167:                     Assert.True((int)now >= (int)previous,
00168:                         $"{def.id} stage regressed from {previous} to {now} at day {d}");
00169:                     previous = now;
00170:                 }
00171:
00172:                 Assert.Equal(DoseLedgerSystem.BandBlack,
00173:                     DiseaseTriage.SickBandFor(def, def.illness_days));
00174:
00175:                 var lastIllDay = Math.Max(def.incubation_days, def.illness_days - 1);
00176:                 var plan = DiseaseTriage.PalliativePlanFor(def, lastIllDay);
00177:                 if (plan != null)
00178:                 {
00179:                     terminalCapable++;
00180:                     Assert.Contains(plan, RegisterPlanIds());
00181:                 }
00182:
00183:                 Assert.DoesNotContain(DiseaseTriage.StageToken(
00184:                     DiseaseTriage.StageOf(def, lastIllDay)), new[] { "", "none" });
00185:             }
00186:
00187:             // A catalog where nothing is ever terminal is as unreadable as one
00188:             // where everything is: at least one authored path must reach comfort care.
00189:             Assert.True(terminalCapable > 0,
00190:                 "no authored disease ever reaches a terminal prognosis");
00191:         }
00192:
00193:         // ── sick list: named source, additive, round-trips ─────────────
00194:
00195:         [Fact]
00196:         public void Diagnose_LegacySignatureStaysDoseSourced()
00197:         {
00198:             var list = new SickListSystem();
00199:             list.Diagnose("sv_a", DoseLedgerSystem.BandRed, day: 10);
00200:
00201:             var band = list.GetBand("sv_a");
00202:             Assert.Equal(SickListSystem.SourceDose, band.severitySource);
00203:             Assert.Equal(string.Empty, band.sourceId);
00204:         }
00205:
00206:         [Fact]
00207:         public void Diagnose_IllnessSourceRecordsDiseaseProvenance()
00208:         {
00209:             var list = new SickListSystem();
00210:             list.Diagnose("sv_a", DoseLedgerSystem.BandAmber, day: 10,
00211:                 SickListSystem.SourceIllness, "disease_cholera");
00212:
00213:             var band = list.GetBand("sv_a");
00214:             Assert.Equal(SickListSystem.SourceIllness, band.severitySource);
00215:             Assert.Equal("disease_cholera", band.sourceId);
00216:         }
00217:
00218:         [Fact]
00219:         public void SickList_NewFieldsRoundTripThroughCapture()
00220:         {
00221:             var list = new SickListSystem();
00222:             list.Diagnose("sv_b", DoseLedgerSystem.BandBlack, day: 40,
00223:                 SickListSystem.SourceIllness, "disease_spore_blight");
00224:             list.AssignPalliative("sv_b", DiseaseTriage.Plans.MorphineTray);
00225:
00226:             var restored = new SickListSystem();
00227:             restored.RestoreState(list.CaptureState());
00228:
00229:             var band = restored.GetBand("sv_b");
00230:             Assert.Equal(SickListSystem.SourceIllness, band.severitySource);
00231:             Assert.Equal("disease_spore_blight", band.sourceId);
00232:             Assert.Equal(DiseaseTriage.Plans.MorphineTray, band.palliativePlan);
00233:             Assert.Equal(DoseLedgerSystem.BandBlack, band.band);
00234:         }
00235:
00236:         [Fact]
00237:         public void SickList_PreD5RowsLoadAsDoseSourced()
00238:         {
00239:             // A save written before the fields existed has null severitySource.
00240:             var legacy = new SickListSystemState();
00241:             legacy.bands.Add(new SickBand
00242:             {
00243:                 survivorId = "sv_c",
00244:                 band = DoseLedgerSystem.BandRed,
00245:                 diagnosedDay = 12,
00246:                 releaseDay = -1,
00247:                 palliativePlan = string.Empty,
00248:             });
00249:
00250:             var list = new SickListSystem();
00251:             list.RestoreState(legacy);
00252:
00253:             Assert.Equal(SickListSystem.SourceDose, list.GetBand("sv_c").severitySource);
00254:         }
00255:
00256:         [Fact]
00257:         public void Release_OnlyTouchesRowsTheDiseaseBridgeNamed()
00258:         {
00259:             var list = new SickListSystem();
00260:             list.Diagnose("sv_ill", DoseLedgerSystem.BandRed, day: 5,
00261:                 SickListSystem.SourceIllness, "disease_cholera");
00262:             list.Diagnose("sv_dose", DoseLedgerSystem.BandBlack, day: 5);
00263:
00264:             Assert.True(list.Release("sv_ill", day: 9));
00265:             var doseRow = list.GetBand("sv_dose");
00266:
00267:             // Dose-named rows are the dose ledger's business, not triage's.
00268:             Assert.Equal(SickListSystem.SourceDose, doseRow.severitySource);
00269:             Assert.Equal(-1, doseRow.releaseDay);
00270:         }
00271:
00272:         [Fact]
00273:         public void CaptureState_IsOrdinalAndIndependentOfLiveState()
00274:         {
00275:             var list = new SickListSystem();
00276:             list.Diagnose("sv_z", 1, 2, SickListSystem.SourceIllness, "disease_a");
00277:             list.Diagnose("sv_a", 1, 2, SickListSystem.SourceIllness, "disease_b");
00278:
00279:             var capture = list.CaptureState();
00280:             Assert.Equal(new[] { "sv_a", "sv_z" }, capture.bands.Select(b => b.survivorId).ToArray());
00281:
00282:             capture.bands[0].band = DoseLedgerSystem.BandBlack;
00283:             Assert.NotEqual(DoseLedgerSystem.BandBlack, list.GetBand("sv_a").band);
00284:         }
00285:     }
00286: }
```

## `Assets/Ashfall.Core/Disease/DiseaseTriage.cs` — 262 lines; 12,294 bytes; SHA-256 `bf9a33da7a8912b615f0d9e9188ff241821e1edaabc594c82e321699b24ec2e7`
Declaration index:
- 00014: public enum DiseaseClinicalStage
- 00041: public sealed class DiseaseClinicalPicture
- 00079: public static class DiseaseTriage
- 00106: public static class Plans
- 00118: public static DiseaseClinicalStage StageOf(DiseaseDefinition def, int daysSick)
- 00137: public static bool IsTerminalPrognosis(DiseaseDefinition def, int daysSick)
- 00162: public static int SickBandFor(DiseaseDefinition def, int daysSick)
- 00180: public static bool ShouldNameToSickList(DiseaseDefinition def, int daysSick) =>
- 00189: public static string PalliativePlanFor(DiseaseDefinition def, int daysSick)
- 00202: public static string StageToken(DiseaseClinicalStage stage)
- 00219: public static DiseaseClinicalPicture PictureOf(
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003:
00004: namespace Ashfall.Core.Disease
00005: {
00006:     /// <summary>
00007:     /// Plan 60 / D1 — the clinical stage of an infection, <em>derived</em> from the
00008:     /// authored catalog bounds. There is deliberately no authored phase list:
00009:     /// <see cref="DiseaseSystem.ResolveOutcomes"/> already treats
00010:     /// <c>incubation_days</c> and <c>illness_days</c> as the only progression
00011:     /// bindings, so a second authored timeline would be a parallel authority that
00012:     /// nothing drives.
00013:     /// </summary>
00014:     public enum DiseaseClinicalStage
00015:     {
00016:         /// <summary>No active infection (or the definition is unknown).</summary>
00017:         None = 0,
00018:         /// <summary>Infected, still inside the incubation window: not yet contagious.</summary>
00019:         Incubating = 1,
00020:         /// <summary>Clinically ill and contagious, still inside the treatable window.</summary>
00021:         Ill = 2,
00022:         /// <summary>
00023:         /// Ill, past the terminal-prognosis threshold: comfort care becomes the
00024:         /// honest plan and triage urgency rises.
00025:         /// </summary>
00026:         Terminal = 3,
00027:         /// <summary>
00028:         /// Illness window elapsed; the engine resolves death or recovery on the
00029:         /// next tick. Kept distinct from <see cref="Terminal"/> because the
00030:         /// outcome is pending, not foretold.
00031:         /// </summary>
00032:         OutcomePending = 4,
00033:     }
00034:
00035:     /// <summary>
00036:     /// Plan 60 / D2 — everything a clinical surface is allowed to say about one
00037:     /// patient, assembled in one place from the authored catalog and the derived
00038:     /// stage. A panel that composed its own wording from raw fields is how the ward
00039:     /// and the sick list start disagreeing about the same person.
00040:     /// </summary>
00041:     public sealed class DiseaseClinicalPicture
00042:     {
00043:         public string DiseaseId { get; set; } = string.Empty;
00044:         public string DisplayName { get; set; } = string.Empty;
00045:         public DiseaseClinicalStage Stage { get; set; }
00046:         public string StageToken { get; set; } = string.Empty;
00047:         public string Tell { get; set; } = string.Empty;
00048:         public string SecondaryTell { get; set; } = string.Empty;
00049:         public string TimingClue { get; set; } = string.Empty;
00050:         public string Guidance { get; set; } = string.Empty;
00051:         public string Vector { get; set; } = string.Empty;
00052:         public int DaysSick { get; set; }
00053:         public int DaysUntilOutcome { get; set; }
00054:         public float AuthoredLethality { get; set; }
00055:         public float EffectiveLethality { get; set; }
00056:         public bool HasTreatmentPath { get; set; }
00057:         public bool HasCure { get; set; }
00058:         public bool Terminal { get; set; }
00059:         public int DosesGiven { get; set; }
00060:
00061:         /// <summary>
00062:         /// Whether the illness has been read by anybody yet. An undiagnosed patient is
00063:         /// not a list of answers: the tell is what the player has, not what the engine
00064:         /// knows, so surfaces say "unidentified" rather than spoiling the diagnosis.
00065:         /// </summary>
00066:         public bool Diagnosed { get; set; }
00067:     }
00068:
00069:     /// <summary>
00070:     /// Plan 60 / D1 + D5 — the single mapping from infection state to clinical
00071:     /// stage, sick-list band, and palliative plan. Pure, deterministic, and
00072:     /// shared by every surface so no consumer re-derives clinical truth.
00073:     ///
00074:     /// Band values are <see cref="DoseLedgerSystem"/> band constants: the sick
00075:     /// list keeps ONE urgency ladder, and <c>SickBand.severitySource</c> records
00076:     /// which fact produced the band (dose vs illness) so the two never silently
00077:     /// mean the same thing.
00078:     /// </summary>
00079:     public static class DiseaseTriage
00080:     {
00081:         /// <summary>
00082:         /// Fraction of the illness window after which a high-lethality infection is
00083:         /// a terminal prognosis rather than an acute one. 0.75 leaves a real
00084:         /// comfort-care window before the outcome roll.
00085:         /// </summary>
00086:         public const float TerminalWindowFraction = 0.75f;
00087:
00088:         /// <summary>
00089:         /// A disease must be at least this lethal to be called terminal. A
00090:         /// self-limiting infection should never be routed to palliative care just
00091:         /// because it is late.
00092:         /// </summary>
00093:         public const float TerminalLethalityFloor = 0.25f;
00094:
00095:         /// <summary>
00096:         /// Lethality at or above which the terminal plan escalates from comfort
00097:         /// rounds to the morphine tray.
00098:         /// </summary>
00099:         public const float HeavySedationLethalityFloor = 0.5f;
00100:
00101:         /// <summary>
00102:         /// Authored palliative plan ids. These are the Expansion 07 register plans
00103:         /// in <c>dose_registers.json</c> (<c>plans[]</c>) — never invented here, and
00104:         /// rendered by the register's own labels.
00105:         /// </summary>
00106:         public static class Plans
00107:         {
00108:             public const string ComfortRounds = "plan_comfort_rounds";
00109:             public const string MorphineTray = "plan_morphine_tray";
00110:         }
00111:
00112:         /// <summary>
00113:         /// Derive the clinical stage for <paramref name="daysSick"/> under
00114:         /// <paramref name="def"/>. Null/empty definitions resolve to
00115:         /// <see cref="DiseaseClinicalStage.None"/> rather than guessing, so an
00116:         /// unauthorised disease cannot manufacture a prognosis.
00117:         /// </summary>
00118:         public static DiseaseClinicalStage StageOf(DiseaseDefinition def, int daysSick)
00119:         {
00120:             if (def == null || daysSick < 0) return DiseaseClinicalStage.None;
00121:
00122:             int incubation = Math.Max(0, def.incubation_days);
00123:             int illness = Math.Max(1, def.illness_days);
00124:
00125:             if (daysSick >= illness) return DiseaseClinicalStage.OutcomePending;
00126:             if (daysSick < incubation) return DiseaseClinicalStage.Incubating;
00127:             return IsTerminalPrognosis(def, daysSick)
00128:                 ? DiseaseClinicalStage.Terminal
00129:                 : DiseaseClinicalStage.Ill;
00130:         }
00131:
00132:         /// <summary>
00133:         /// True when the infection is late enough and lethal enough that comfort
00134:         /// care, not curative effort, is the honest plan. Never true for a
00135:         /// low-lethality disease, whatever the day.
00136:         /// </summary>
00137:         public static bool IsTerminalPrognosis(DiseaseDefinition def, int daysSick)
00138:         {
00139:             if (def == null || daysSick < 0) return false;
00140:             if (def.lethality < TerminalLethalityFloor) return false;
00141:
00142:             int illness = Math.Max(1, def.illness_days);
00143:             int incubation = Math.Min(illness - 1, Math.Max(0, def.incubation_days));
00144:             int window = Math.Max(1, illness - incubation);
00145:             int terminalAt = incubation + (int)Math.Ceiling(window * TerminalWindowFraction);
00146:
00147:             // A disease whose window rounds out to "terminal on its first ill day"
00148:             // is not a terminal prognosis yet; keep at least one day of acute
00149:             // treatment room so palliative routing is never the default.
00150:             terminalAt = Math.Max(incubation + 1, terminalAt);
00151:             return daysSick >= terminalAt;
00152:         }
00153:
00154:         /// <summary>
00155:         /// Map infection state onto the shared sick-list band ladder
00156:         /// (<see cref="DoseLedgerSystem.BandGreen"/> … <see cref="DoseLedgerSystem.BandBlack"/>).
00157:         ///
00158:         /// Incubating cases are deliberately <em>not</em> named into the sick list
00159:         /// (see <see cref="ShouldNameToSickList"/>) — the list is the named sick,
00160:         /// and incubation is a quarantine question, not a triage one.
00161:         /// </summary>
00162:         public static int SickBandFor(DiseaseDefinition def, int daysSick)
00163:         {
00164:             switch (StageOf(def, daysSick))
00165:             {
00166:                 case DiseaseClinicalStage.Ill:
00167:                     return DoseLedgerSystem.BandAmber;
00168:                 case DiseaseClinicalStage.Terminal:
00169:                     return DoseLedgerSystem.BandRed;
00170:                 case DiseaseClinicalStage.OutcomePending:
00171:                     return DoseLedgerSystem.BandBlack;
00172:                 default:
00173:                     return DoseLedgerSystem.BandGreen;
00174:             }
00175:         }
00176:
00177:         /// <summary>
00178:         /// Whether this infection belongs on the sick list at all.
00179:         /// </summary>
00180:         public static bool ShouldNameToSickList(DiseaseDefinition def, int daysSick) =>
00181:             StageOf(def, daysSick) != DiseaseClinicalStage.None
00182:             && StageOf(def, daysSick) != DiseaseClinicalStage.Incubating;
00183:
00184:         /// <summary>
00185:         /// The comfort-care plan for a terminal infection, or null when no
00186:         /// palliative plan is honest (acute, self-limiting, or incubating).
00187:         /// Returns authored register plan ids only.
00188:         /// </summary>
00189:         public static string PalliativePlanFor(DiseaseDefinition def, int daysSick)
00190:         {
00191:             if (StageOf(def, daysSick) != DiseaseClinicalStage.Terminal) return null;
00192:             return def.lethality >= HeavySedationLethalityFloor
00193:                 ? Plans.MorphineTray
00194:                 : Plans.ComfortRounds;
00195:         }
00196:
00197:         /// <summary>
00198:         /// Stable, human-readable stage token for logs, reports and tests. Not a
00199:         /// player-facing string — player text goes through the catalog's authored
00200:         /// fields and the localization keys.
00201:         /// </summary>
00202:         public static string StageToken(DiseaseClinicalStage stage)
00203:         {
00204:             switch (stage)
00205:             {
00206:                 case DiseaseClinicalStage.Incubating: return "incubating";
00207:                 case DiseaseClinicalStage.Ill: return "ill";
00208:                 case DiseaseClinicalStage.Terminal: return "terminal";
00209:                 case DiseaseClinicalStage.OutcomePending: return "outcome_pending";
00210:                 default: return "none";
00211:             }
00212:         }
00213:
00214:         /// <summary>
00215:         /// Assemble the clinical picture for one patient. <paramref name="diagnosed"/>
00216:         /// gates the naming of the illness: the signs are always visible (that is what
00217:         /// a medic sees), the identification is earned by diagnosing.
00218:         /// </summary>
00219:         public static DiseaseClinicalPicture PictureOf(
00220:             DiseaseDefinition def, int daysSick, float effectiveLethality = float.NaN,
00221:             int dosesGiven = 0, bool diagnosed = true)
00222:         {
00223:             // NaN means "nobody told me the patient's own odds", so the projection
00224:             // falls back to the disease's authored lethality — never to zero, which
00225:             // would read on a surface as "this illness cannot kill anyone".
00226:             float odds = float.IsNaN(effectiveLethality) ? 0f : effectiveLethality;
00227:             var picture = new DiseaseClinicalPicture
00228:             {
00229:                 DaysSick = daysSick < 0 ? 0 : daysSick,
00230:                 DosesGiven = dosesGiven < 0 ? 0 : dosesGiven,
00231:                 Diagnosed = diagnosed,
00232:                 EffectiveLethality = odds,
00233:             };
00234:             if (def == null) return picture;
00235:
00236:             var stage = StageOf(def, picture.DaysSick);
00237:             picture.DiseaseId = def.id ?? string.Empty;
00238:             picture.DisplayName = diagnosed ? (def.display_name ?? string.Empty) : string.Empty;
00239:             picture.Vector = def.vector ?? string.Empty;
00240:             picture.Stage = stage;
00241:             picture.StageToken = StageToken(stage);
00242:             picture.Tell = def.tell ?? string.Empty;
00243:             picture.SecondaryTell = def.tell_secondary ?? string.Empty;
00244:             picture.TimingClue = def.timing_clue ?? string.Empty;
00245:             picture.Guidance = def.guidance ?? string.Empty;
00246:             picture.AuthoredLethality = def.lethality;
00247:             if (odds <= 0f) picture.EffectiveLethality = Math.Max(0f, def.lethality);
00248:             picture.DaysUntilOutcome = Math.Max(0, def.illness_days - picture.DaysSick);
00249:             picture.Terminal = stage == DiseaseClinicalStage.Terminal;
00250:             if (def.treatments != null && def.treatments.Count > 0)
00251:             {
00252:                 picture.HasTreatmentPath = true;
00253:                 for (int i = 0; i < def.treatments.Count; i++)
00254:                 {
00255:                     var t = def.treatments[i];
00256:                     if (t != null && DiseaseTreatmentRoles.IsCurative(t.role)) { picture.HasCure = true; break; }
00257:                 }
00258:             }
00259:             return picture;
00260:         }
00261:     }
00262: }
```

## `src/UI/RadiationDetailPanel.cs` — 305 lines; 13,638 bytes; SHA-256 `0da4a273c442df88d1fd7bf4c530ad4479ab08602588508f385250bd4a2167e4`
Declaration index:
- 00017: public partial class RadiationDetailPanel : Control
- 00037: public void Bind(DoseLedgerHostSession? dose = null, SurvivorsHostSession? survivors = null)
- 00052: private void OnDoseStateChanged(Ashfall.Core.DoseLedgerSystemState _) => RefreshView();
- 00053: private void OnSurvivorsStateChanged() => RefreshView();
- 00055: public void RefreshView()
- 00071: private void RenderCurrent()
- 00119: private void RenderDosimeter()
- 00159: private void RenderProtection()
- 00212: private void RenderEvents()
- 00239: private Label MakeDimLine(string text)
- 00247: private static string Name(string id)
- 00254: private void EnsureUI()
- 00288: public void Open()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Linq;
00004: #pragma warning disable CS8618
00005: using Godot;
00006: using Ashfall.Core.UI;
00007: using AtomicWar.GodotApp.UI;
00008:
00009: namespace AtomicWar.GodotApp.UI
00010: {
00011:     /// <summary>
00012:     /// ASHFALL — Radiation Detail panel.
00013:     /// Shows per-survivor dosimetry, dosimeter calibration state, protection
00014:     /// levels, and the dose-reading event log — bound to the live DoseLedger
00015:     /// and Survivors sessions. Unbound systems render "NOT MONITORED".
00016:     /// </summary>
00017:     public partial class RadiationDetailPanel : Control
00018:     {
00019:         public event Action? OnClose;
00020:
00021:         private VBoxContainer _contentVBox = null!;
00022:         private Label _lblCurrentTitle;
00023:         private VBoxContainer _currentData;
00024:         private Label _lblDosimeterTitle;
00025:         private VBoxContainer _dosimeterData;
00026:         private Label _lblProtectionTitle;
00027:         private VBoxContainer _protectionData;
00028:         private Label _lblEventsTitle;
00029:         private VBoxContainer _eventsList;
00030:
00031:         private DoseLedgerHostSession? _dose;
00032:         private SurvivorsHostSession? _survivors;
00033:
00034:         public bool IsBound => _dose != null || _survivors != null;
00035:         public int RenderedCurrentCount { get; private set; }
00036:
00037:         public void Bind(DoseLedgerHostSession? dose = null, SurvivorsHostSession? survivors = null)
00038:         {
00039:             // Live refresh (UI/UX audit silent-failure sweep): the detail view must
00040:             // reflect play while open, not only the state at open time.
00041:             if (_dose != null) _dose.Ledger.OnStateChanged -= OnDoseStateChanged;
00042:             if (_survivors != null) _survivors.StateChanged -= OnSurvivorsStateChanged;
00043:
00044:             _dose = dose;
00045:             _survivors = survivors;
00046:
00047:             if (_dose != null) _dose.Ledger.OnStateChanged += OnDoseStateChanged;
00048:             if (_survivors != null) _survivors.StateChanged += OnSurvivorsStateChanged;
00049:             RefreshView();
00050:         }
00051:
00052:         private void OnDoseStateChanged(Ashfall.Core.DoseLedgerSystemState _) => RefreshView();
00053:         private void OnSurvivorsStateChanged() => RefreshView();
00054:
00055:         public void RefreshView()
00056:         {
00057:             EnsureUI();
00058:
00059:             AshfallUiHelpers.EmptyChildren(_currentData);
00060:             AshfallUiHelpers.EmptyChildren(_dosimeterData);
00061:             AshfallUiHelpers.EmptyChildren(_protectionData);
00062:             AshfallUiHelpers.EmptyChildren(_eventsList);
00063:
00064:             RenderedCurrentCount = 0;
00065:             RenderCurrent();
00066:             RenderDosimeter();
00067:             RenderProtection();
00068:             RenderEvents();
00069:         }
00070:
00071:         private void RenderCurrent()
00072:         {
00073:             if (_survivors != null && _survivors.RosterState.Count > 0)
00074:             {
00075:                 foreach (var state in _survivors.RosterState)
00076:                 {
00077:                     if (state == null) continue;
00078:                     var rad = _survivors.RadStateFor(state.Id);
00079:                     var env = _survivors.GetLastExposureEnvironment(state.Id);
00080:                     string reason = !string.IsNullOrEmpty(env?.ExposureReason)
00081:                         ? env.ExposureReason
00082:                         : (!string.IsNullOrEmpty(rad?.LastExposureReason) ? rad.LastExposureReason : "Shelter Interior");
00083:                     float dose = rad?.RadiationDose ?? 0f;
00084:                     float zone = env?.EffectiveZoneRadLevel ?? 0f;
00085:                     var row = AshfallUiHelpers.MakeDataRow(Name(state.Id),
00086:                         $"{dose:0.0}/100 mSv · {reason} ({zone:0.0} mSv/h zone)",
00087:                         AshfallUiHelpers.ToColor(dose >= 50f
00088:                             ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe));
00089:                     _currentData.AddChild(row);
00090:                     // C2 / Plan 20A (§16) — canonical input breakdown from the
00091:                     // shared read model (same environment + effective-rate
00092:                     // resolver the tick used). Never recomputed here.
00093:                     var breakdown = _survivors.GetExposureBreakdown(state.Id);
00094:                     if (breakdown != null)
00095:                         _currentData.AddChild(MakeDimLine(breakdown.ToDisplayLine()));
00096:                     RenderedCurrentCount++;
00097:                 }
00098:                 return;
00099:             }
00100:
00101:             if (_dose?.Ledger == null || _dose.Ledger.Entries.Count == 0)
00102:             {
00103:                 _currentData.AddChild(MakeDimLine("No dose ledger bound."));
00104:                 return;
00105:             }
00106:
00107:             foreach (var entry in _dose.Ledger.Entries)
00108:             {
00109:                 if (entry == null) continue;
00110:                 var row = AshfallUiHelpers.MakeDataRow(Name(entry.survivorId),
00111:                     $"{entry.cumulativeMsv:0.0} mSv cumulative · baseline {entry.baselineMsv:0.0}",
00112:                     AshfallUiHelpers.ToColor(entry.cumulativeMsv >= 50f
00113:                         ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe));
00114:                 _currentData.AddChild(row);
00115:                 RenderedCurrentCount++;
00116:             }
00117:         }
00118:
00119:         private void RenderDosimeter()
00120:         {
00121:             if (_dose?.Calibration != null && _dose.Calibration.Devices.Count > 0)
00122:             {
00123:                 foreach (var dev in _dose.Calibration.Devices.Values)
00124:                 {
00125:                     if (dev == null) continue;
00126:                     string assigned = string.IsNullOrEmpty(dev.assignedSurvivorId)
00127:                         ? "unassigned" : Name(dev.assignedSurvivorId);
00128:                     _dosimeterData.AddChild(AshfallUiHelpers.MakeDataRow(
00129:                         $"{dev.deviceTag} ({assigned})",
00130:                         $"Battery {dev.batteryLevel * 100f:0}% · Cal {dev.calibrationQuality * 100f:0}% · ±{dev.errorBandMsv:0.0} mSv{(dev.isOverdue ? " · OVERDUE" : "")}",
00131:                         AshfallUiHelpers.ToColor(dev.isOverdue
00132:                             ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
00133:                 }
00134:                 return;
00135:             }
00136:
00137:             if (_survivors?.Radiation != null && _survivors.RosterState.Count > 0)
00138:             {
00139:                 foreach (var s in _survivors.RosterState)
00140:                 {
00141:                     if (s == null) continue;
00142:                     var dosimeter = _survivors.Radiation.GetDosimeter(s.Id);
00143:                     if (dosimeter == null) continue;
00144:                     string reason = !string.IsNullOrEmpty(dosimeter.LastExposureReason)
00145:                         ? $" · {dosimeter.LastExposureReason}"
00146:                         : "";
00147:                     _dosimeterData.AddChild(AshfallUiHelpers.MakeDataRow(
00148:                         $"{Name(s.Id)} Pen",
00149:                         $"Rate {dosimeter.CurrentReading:0.0} mSv/h · Lifetime {dosimeter.LifetimeDose:0.0} mSv{reason}",
00150:                         AshfallUiHelpers.ToColor(dosimeter.CurrentReading > 10f
00151:                             ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
00152:                 }
00153:                 return;
00154:             }
00155:
00156:             _dosimeterData.AddChild(MakeDimLine("No dosimeters registered."));
00157:         }
00158:
00159:         private void RenderProtection()
00160:         {
00161:             if (_survivors?.Shelter != null)
00162:             {
00163:                 float weakest = _survivors.Shelter.GetWeakestCeilingAttenuation();
00164:                 _protectionData.AddChild(AshfallUiHelpers.MakeDataRow("Shelter Shielding",
00165:                     $"Weakest ceiling {weakest * 100f:0}% attenuation",
00166:                     AshfallUiHelpers.ToColor(weakest >= 0.5f
00167:                         ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Warm)));
00168:             }
00169:             else
00170:             {
00171:                 _protectionData.AddChild(MakeDimLine("Shelter shielding not monitored."));
00172:             }
00173:
00174:             // C2 / Plan 20B (§28) — the model's own breakdown names the largest
00175:             // contributor so "why is interior radiation high?" has an answer.
00176:             var shielding = _survivors?.GetShieldingBreakdown();
00177:             if (shielding != null)
00178:             {
00179:                 _protectionData.AddChild(AshfallUiHelpers.MakeDataRow("Interior radiation",
00180:                     $"{shielding.InteriorRad:0.00} mSv/h · weakest: {shielding.WeakestContributor} " +
00181:                     $"({shielding.WeakestContribution:0.00} mSv/h)" +
00182:                     (shielding.DeconReduced ? " · decon active" : ""),
00183:                     AshfallUiHelpers.ToColor(shielding.InteriorRad > 1f
00184:                         ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
00185:             }
00186:
00187:             // C2 / Plan 21A (P3/§12) — remaining life of the weakest protective
00188:             // item from the Core estimate (no panel-side wear arithmetic).
00189:             var weakestGear = _survivors?.GetWeakestProtectiveLife();
00190:             if (weakestGear != null)
00191:             {
00192:                 _protectionData.AddChild(AshfallUiHelpers.MakeDataRow("Weakest protective gear",
00193:                     $"{weakestGear.DisplayName} · condition {weakestGear.CurrentDurability:0}/{weakestGear.MaxDurability:0}" +
00194:                     (weakestGear.ExposureMultiplier > 1f ? $" · ~{weakestGear.HoursRemaining:0} h at current exposure (×{weakestGear.ExposureMultiplier:0.#})" : $" · ~{weakestGear.HoursRemaining:0} h at current exposure"),
00195:                     AshfallUiHelpers.ToColor(weakestGear.HoursRemaining < 24f
00196:                         ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
00197:             }
00198:
00199:             if (_dose?.Ledger != null)
00200:             {
00201:                 foreach (var entry in _dose.Ledger.Entries)
00202:                 {
00203:                     if (entry == null || entry.shieldingFactor >= 1f) continue;
00204:                     _protectionData.AddChild(AshfallUiHelpers.MakeDataRow(
00205:                         $"{Name(entry.survivorId)} shielding",
00206:                         $"{entry.shieldingFactor * 100f:0}% of outdoor dose",
00207:                         AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
00208:                 }
00209:             }
00210:         }
00211:
00212:         private void RenderEvents()
00213:         {
00214:             if (_dose?.Ledger == null || _dose.Ledger.Entries.Count == 0)
00215:             {
00216:                 _eventsList.AddChild(MakeDimLine("No dose readings logged."));
00217:                 return;
00218:             }
00219:
00220:             int shown = 0;
00221:             foreach (var entry in _dose.Ledger.Entries)
00222:             {
00223:                 if (entry == null) continue;
00224:                 foreach (var reading in entry.readingsHistory)
00225:                 {
00226:                     if (reading == null || shown >= 20) continue;
00227:                     _eventsList.AddChild(AshfallUiHelpers.MakeDataRow(
00228:                         $"[Day {reading.day}] {Name(entry.survivorId)}",
00229:                         $"{reading.bookedMsv:0.0} mSv booked ({reading.source})",
00230:                         AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)));
00231:                     shown++;
00232:                 }
00233:             }
00234:
00235:             if (shown == 0)
00236:                 _eventsList.AddChild(MakeDimLine("No reading events yet."));
00237:         }
00238:
00239:         private Label MakeDimLine(string text)
00240:         {
00241:             var l = new Label { Text = text };
00242:             l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
00243:             l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
00244:             return l;
00245:         }
00246:
00247:         private static string Name(string id)
00248:         {
00249:             if (string.IsNullOrEmpty(id)) return "Unknown";
00250:             int us = id.IndexOf('_');
00251:             return us >= 0 ? id.Substring(us + 1).Replace('_', ' ') : id;
00252:         }
00253:
00254:         private void EnsureUI()
00255:         {
00256:             if (_currentData == null)
00257:             {
00258:                 _currentData = new VBoxContainer { Name = "CurrentData" };
00259:                 _dosimeterData = new VBoxContainer { Name = "DosimeterData" };
00260:                 _protectionData = new VBoxContainer { Name = "ProtectionData" };
00261:                 _eventsList = new VBoxContainer { Name = "EventsList" };
00262:                 AddChild(_currentData);
00263:                 AddChild(_dosimeterData);
00264:                 AddChild(_protectionData);
00265:                 AddChild(_eventsList);
00266:             }
00267:         }
00268:
00269:         public override void _Ready()
00270:         {
00271:             // Ticket #125: layout chrome owned by res://assets/ui/panels/RadiationDetailPanel.tscn; SceneBinder resolves typed unique-name nodes once.
00272:             // Sibling refresh code is unchanged.
00273:             var binder = new SceneBinder(this, typeof(RadiationDetailPanel));
00274:             binder.Require<VBoxContainer>("CurrentData");
00275:             binder.Require<VBoxContainer>("DosimeterData");
00276:             binder.Require<VBoxContainer>("ProtectionData");
00277:             binder.Require<VBoxContainer>("EventsList");
00278:             binder.Require<Button>("CloseButton");
00279:             _currentData = binder.Get<VBoxContainer>("CurrentData");
00280:             _dosimeterData = binder.Get<VBoxContainer>("DosimeterData");
00281:             _protectionData = binder.Get<VBoxContainer>("ProtectionData");
00282:             _eventsList = binder.Get<VBoxContainer>("EventsList");
00283:             binder.Get<Button>("CloseButton").Pressed += () => OnClose?.Invoke();
00284:
00285:             Visible = false;
00286:         }
00287:
00288:         public void Open()
00289:         {
00290:             Visible = true;
00291:             QueueRedraw();
00292:         }
00293:
00294:         public override void _UnhandledInput(InputEvent @event)
00295:         {
00296:             if (!Visible) return;
00297:
00298:             if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00299:             {
00300:                 OnClose?.Invoke();
00301:                 GetViewport().SetInputAsHandled();
00302:             }
00303:         }
00304:     }
00305: }
```

## `src/UI/DoseGeographyPanel.cs` — 432 lines; 18,300 bytes; SHA-256 `98e2b11413ad42058ccb5a1bf3a2332856399804e88538bf2b2f0c9931502f40`
Declaration index:
- 00028: public partial class DoseGeographyPanel : Control
- 00051: public void Bind(DoseLedgerHostSession? session)
- 00060: public void Unbind()
- 00067: private void HandleLedgerChanged(DoseLedgerSystemState _) => RefreshView();
- 00069: public void RefreshView()
- 00076: private void RefreshStatusRail()
- 00113: private void BuildRows()
- 00162: private bool SectorPass(string sector) => _sectorFilter switch
- 00172: private int CountProvenanceEvents()
- 00189: private bool MatchesAnyVisibleLocation(string source)
- 00198: private int ProvenanceEventCount(string locationId)
- 00212: private void RefreshDetail()
- 00275: internal static string RiskTier(int risk) => risk switch
- 00343: private void BuildContent()
- 00408: public void Open()
- 00415: public void Close() => Visible = false;
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Text;
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.UI;
00008: using AtomicWar.GodotApp.UI;
00009: using DesignTheme = Ashfall.Core.UI.Theme;
00010:
00011: namespace AtomicWar.GodotApp.UI;
00012:
00013: /// <summary>
00014: /// ASHFALL — Dose Geography (Plan 81 radiation-cartography surface).
00015: ///
00016: /// The sector-aware view over <see cref="Assets/Ashfall.Core DoseContentCatalog"/>
00017: /// locations: every dose location with its canonical sector, authored risk
00018: /// level (0–8, numeral + tier word — never color-only) and its baseline
00019: /// exposure rate in truthful µSv/h. Row selection shows the environmental
00020: /// description and the dose actually booked at that place, from ledger
00021: /// provenance.
00022: ///
00023: /// Pure presentation — reads only from <see cref="DoseLedgerHostSession"/>.
00024: /// Unbound renders an honest empty state (no fixture rows; UI-19 discipline).
00025: /// Registered as the 'dose_geography' expanded route (PanelRegistryBootstrap)
00026: /// and reachable from the dashboard navigation rail.
00027: /// </summary>
00028: public partial class DoseGeographyPanel : Control
00029: {
00030:     public event Action? OnClose;
00031:
00032:     private AshfallDashboardShell _shell = null!;
00033:     private AshfallSidebar? _sidebar;
00034:     private AshfallStatusRail? _statusRail;
00035:     private AshfallDataGrid? _grid;
00036:     private VBoxContainer _detailBox = null!;
00037:     private Label _detailTitle = null!;
00038:     private int _selectedIndex = -1;
00039:     private string _sectorFilter = "all";
00040:
00041:     private DoseLedgerHostSession? _dose;
00042:     private readonly List<DoseLocationDef> _visibleLocations = new();
00043:     private readonly StringBuilder _renderDump = new();
00044:
00045:     public bool IsBound => _dose != null;
00046:
00047:     /// <summary>Rendered row/rail text from the last RefreshView, for
00048:     /// headless UI assertions (route→bind→visible→rendered-strings).</summary>
00049:     internal string RenderDump => _renderDump.ToString();
00050:
00051:     public void Bind(DoseLedgerHostSession? session)
00052:     {
00053:         Unbind();
00054:         _dose = session;
00055:         if (_dose?.Ledger != null)
00056:             _dose.Ledger.OnStateChanged += HandleLedgerChanged;
00057:         RefreshView();
00058:     }
00059:
00060:     public void Unbind()
00061:     {
00062:         if (_dose?.Ledger != null)
00063:             _dose.Ledger.OnStateChanged -= HandleLedgerChanged;
00064:         _dose = null;
00065:     }
00066:
00067:     private void HandleLedgerChanged(DoseLedgerSystemState _) => RefreshView();
00068:
00069:     public void RefreshView()
00070:     {
00071:         RefreshStatusRail();
00072:         BuildRows();
00073:         RefreshDetail();
00074:     }
00075:
00076:     private void RefreshStatusRail()
00077:     {
00078:         if (_statusRail == null) return;
00079:         var locations = _dose?.Content?.locations;
00080:         if (locations == null || locations.Count == 0)
00081:         {
00082:             _statusRail.Set("places", "0", AshfallMetricCard.Criticality.Normal);
00083:             _statusRail.Set("sectors", "0", AshfallMetricCard.Criticality.Normal);
00084:             _statusRail.Set("peak", "—", AshfallMetricCard.Criticality.Normal);
00085:             _statusRail.Set("risk", "—", AshfallMetricCard.Criticality.Normal);
00086:             _statusRail.Set("hot", "0", AshfallMetricCard.Criticality.Normal);
00087:             _statusRail.Set("events", "0", AshfallMetricCard.Criticality.Normal);
00088:             return;
00089:         }
00090:
00091:         int maxRisk = 0;
00092:         float maxRate = 0f;
00093:         var sectors = new HashSet<string>(StringComparer.Ordinal);
00094:         foreach (var l in locations)
00095:         {
00096:             if (l == null) continue;
00097:             sectors.Add(l.sector);
00098:             if (l.riskLevel > maxRisk) maxRisk = l.riskLevel;
00099:             if (l.radiationUsv > maxRate) maxRate = l.radiationUsv;
00100:         }
00101:         int hot = 0;
00102:         foreach (var l in locations)
00103:             if (l != null && l.riskLevel >= 5) hot++;
00104:
00105:         _statusRail.Set("places", $"{locations.Count}", AshfallMetricCard.Criticality.Normal);
00106:         _statusRail.Set("sectors", $"{sectors.Count}", AshfallMetricCard.Criticality.Normal);
00107:         _statusRail.Set("peak", $"{maxRate:0.00} µSv/h", maxRate >= 10f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
00108:         _statusRail.Set("risk", $"{maxRisk} · {RiskTier(maxRisk)}", maxRisk >= 5 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
00109:         _statusRail.Set("hot", $"{hot}", hot > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
00110:         _statusRail.Set("events", $"{CountProvenanceEvents()}", AshfallMetricCard.Criticality.Normal);
00111:     }
00112:
00113:     private void BuildRows()
00114:     {
00115:         if (_grid == null) return;
00116:         _visibleLocations.Clear();
00117:         _renderDump.Clear();
00118:
00119:         var locations = _dose?.Content?.locations;
00120:         if (_dose == null || locations == null || locations.Count == 0)
00121:         {
00122:             // Honest empty state — no fixture rows (UI-19 discipline).
00123:             _grid.SetRows(new List<AshfallDataGrid.Row>());
00124:             _renderDump.AppendLine("No dose content loaded.");
00125:             return;
00126:         }
00127:
00128:         var rows = new List<AshfallDataGrid.Row>();
00129:         foreach (var l in locations)
00130:         {
00131:             if (l == null || string.IsNullOrEmpty(l.id)) continue;
00132:             if (!SectorPass(l.sector)) continue;
00133:             int idx = _visibleLocations.Count;
00134:             _visibleLocations.Add(l);
00135:
00136:             int events = ProvenanceEventCount(l.id);
00137:             var state = l.riskLevel >= 5 ? AshfallDataGrid.CellState.Warning
00138:                 : l.riskLevel >= 3 ? AshfallDataGrid.CellState.Caution
00139:                 : AshfallDataGrid.CellState.Normal;
00140:
00141:             rows.Add(new AshfallDataGrid.Row
00142:             {
00143:                 Cells = new List<AshfallDataGrid.Cell>
00144:                 {
00145:                     new(l.displayName, state),
00146:                     new(l.sector, AshfallDataGrid.CellState.Muted),
00147:                     new($"{l.riskLevel} · {RiskTier(l.riskLevel)}", state),
00148:                     new($"{l.radiationUsv:0.00} µSv/h", state),
00149:                     new(events > 0 ? $"{events}" : "—", events > 0 ? AshfallDataGrid.CellState.Normal : AshfallDataGrid.CellState.Muted),
00150:                 },
00151:                 Selectable = true,
00152:             });
00153:
00154:             _renderDump.Append(l.displayName).Append(" | ").Append(l.sector)
00155:                 .Append(" | risk ").Append(l.riskLevel)
00156:                 .Append(" | ").Append(l.radiationUsv.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture))
00157:                 .Append(" µSv/h\n");
00158:         }
00159:         _grid.SetRows(rows);
00160:     }
00161:
00162:     private bool SectorPass(string sector) => _sectorFilter switch
00163:     {
00164:         "bunker" => sector == "bunker",
00165:         "surface" => sector == "surface",
00166:         "expedition" => sector == "expedition",
00167:         "external" => sector == "external",
00168:         "faction" => sector == "faction",
00169:         _ => true,
00170:     };
00171:
00172:     private int CountProvenanceEvents()
00173:     {
00174:         int total = 0;
00175:         var ledger = _dose?.Ledger;
00176:         if (ledger == null) return 0;
00177:         foreach (var e in ledger.Entries)
00178:         {
00179:             if (e?.readingsHistory == null) continue;
00180:             foreach (var r in e.readingsHistory)
00181:             {
00182:                 if (r != null && ProvenanceEventCount(r.source) >= 0 && MatchesAnyVisibleLocation(r.source))
00183:                     total++;
00184:             }
00185:         }
00186:         return total;
00187:     }
00188:
00189:     private bool MatchesAnyVisibleLocation(string source)
00190:     {
00191:         var locations = _dose?.Content?.locations;
00192:         if (locations == null) return false;
00193:         foreach (var l in locations)
00194:             if (l != null && l.id == source) return true;
00195:         return false;
00196:     }
00197:
00198:     private int ProvenanceEventCount(string locationId)
00199:     {
00200:         int count = 0;
00201:         var ledger = _dose?.Ledger;
00202:         if (ledger == null) return 0;
00203:         foreach (var e in ledger.Entries)
00204:         {
00205:             if (e?.readingsHistory == null) continue;
00206:             foreach (var r in e.readingsHistory)
00207:                 if (r != null && r.source == locationId) count++;
00208:         }
00209:         return count;
00210:     }
00211:
00212:     private void RefreshDetail()
00213:     {
00214:         if (_detailBox == null) return;
00215:         AshfallUiHelpers.EmptyChildren(_detailBox);
00216:
00217:         if (_selectedIndex < 0 || _selectedIndex >= _visibleLocations.Count || _dose == null)
00218:         {
00219:             _detailTitle.Text = "EXPOSURE GEOGRAPHY";
00220:             _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
00221:                 "Select a place to read why it is radioactive, and what the ledger has booked there."));
00222:             return;
00223:         }
00224:
00225:         var l = _visibleLocations[_selectedIndex];
00226:         _detailTitle.Text = l.displayName.ToUpperInvariant() + " · " + l.sector.ToUpperInvariant();
00227:
00228:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("SECTOR", l.sector, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00229:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("RISK LEVEL", $"{l.riskLevel} · {RiskTier(l.riskLevel)}",
00230:             AshfallUiHelpers.ToColor(l.riskLevel >= 5 ? DesignTheme.Entropy : DesignTheme.Pale)));
00231:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("BASELINE RATE", $"{l.radiationUsv:0.00} µSv/h",
00232:             AshfallUiHelpers.ToColor(l.radiationUsv >= 10f ? DesignTheme.Entropy : DesignTheme.Warm)));
00233:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("1-HOUR DWELL", $"{l.radiationUsv:0.00} µSv",
00234:             AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00235:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("4-HOUR SORTIE", $"{l.radiationUsv * 4f:0.0} µSv",
00236:             AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00237:
00238:         if (!string.IsNullOrEmpty(l.description))
00239:         {
00240:             _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
00241:             _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("FIELD NOTE"));
00242:             var desc = AshfallUiHelpers.MakeSmall(l.description);
00243:             desc.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00244:             _detailBox.AddChild(desc);
00245:         }
00246:
00247:         // Ledger provenance for this place — where the dose actually came from.
00248:         float bookedHere = 0f;
00249:         int readings = 0;
00250:         int lastDay = -1;
00251:         var ledger = _dose.Ledger;
00252:         foreach (var e in ledger.Entries)
00253:         {
00254:             if (e?.readingsHistory == null) continue;
00255:             foreach (var r in e.readingsHistory)
00256:             {
00257:                 if (r == null || r.source != l.id) continue;
00258:                 readings++;
00259:                 bookedHere += r.bookedMsv;
00260:                 if (r.day > lastDay) lastDay = r.day;
00261:             }
00262:         }
00263:         _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
00264:         _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("LEDGER PROVENANCE"));
00265:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("BOOKED HERE",
00266:             readings > 0 ? $"{AshfallUiHelpers.FormatDoseMsv(bookedHere)} across {readings} reading(s)" : "nothing booked yet",
00267:             AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00268:         if (lastDay >= 0)
00269:             _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("LAST READING", $"day {lastDay}",
00270:                 AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00271:     }
00272:
00273:     /// <summary>Player-facing risk tier vocabulary (0–8). Numeral + word —
00274:     /// risk is never communicated by color alone (81AW).</summary>
00275:     internal static string RiskTier(int risk) => risk switch
00276:     {
00277:         0 => "Shielded",
00278:         1 => "Transition",
00279:         2 => "Exposed",
00280:         3 => "Contaminated",
00281:         4 => "Corridor",
00282:         5 => "Hot Perimeter",
00283:         6 => "Severe Ruins",
00284:         7 => "Hot Zone",
00285:         8 => "Extreme",
00286:         _ => "—",
00287:     };
00288:
00289:     public override void _Ready()
00290:     {
00291:         SetAnchorsPreset(LayoutPreset.FullRect);
00292:         Visible = false;
00293:
00294:         var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
00295:         bg.SetAnchorsPreset(LayoutPreset.FullRect);
00296:         AddChild(bg);
00297:
00298:         _shell = new AshfallDashboardShell("DOSE GEOGRAPHY — EXPOSURE MAP", 1180, 720);
00299:
00300:         var hostContainer = new MarginContainer();
00301:         hostContainer.AddThemeConstantOverride("margin_left", DesignTheme.SpacingLg);
00302:         hostContainer.AddThemeConstantOverride("margin_top", DesignTheme.SpacingLg);
00303:         hostContainer.AddThemeConstantOverride("margin_right", DesignTheme.SpacingLg);
00304:         hostContainer.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingLg);
00305:         hostContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00306:         hostContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00307:         hostContainer.AddChild(_shell);
00308:         AddChild(hostContainer);
00309:
00310:         _sidebar = _shell.SetSidebar(new[]
00311:         {
00312:             new AshfallSidebar.Item { Id = "all",        Label = "All sectors",   Hint = "every standing place" },
00313:             new AshfallSidebar.Item { Id = "bunker",     Label = "Bunker",        Hint = "shielded interior" },
00314:             new AshfallSidebar.Item { Id = "surface",    Label = "Surface",       Hint = "the threshold above" },
00315:             new AshfallSidebar.Item { Id = "expedition", Label = "Expedition",    Hint = "destination hot zones" },
00316:             new AshfallSidebar.Item { Id = "external",   Label = "External",      Hint = "travel corridors" },
00317:             new AshfallSidebar.Item { Id = "faction",    Label = "Faction",       Hint = "checkpoint approaches" },
00318:         }, "SECTORS", "all");
00319:
00320:         if (_sidebar != null)
00321:         {
00322:             _sidebar.OnSelected += id =>
00323:             {
00324:                 _sectorFilter = id;
00325:                 BuildRows();
00326:                 RefreshDetail();
00327:             };
00328:         }
00329:
00330:         _statusRail = _shell.SetStatusRail();
00331:         _statusRail.AddCard("places", "PLACES", "0", AshfallMetricCard.Criticality.Normal, 100);
00332:         _statusRail.AddCard("sectors", "SECTORS", "0", AshfallMetricCard.Criticality.Normal, 100);
00333:         _statusRail.AddCard("peak", "PEAK RATE", "—", AshfallMetricCard.Criticality.Normal, 130);
00334:         _statusRail.AddCard("risk", "PEAK RISK", "—", AshfallMetricCard.Criticality.Normal, 130);
00335:         _statusRail.AddCard("hot", "HOT ZONES", "0", AshfallMetricCard.Criticality.Normal, 100);
00336:         _statusRail.AddCard("events", "READINGS", "0", AshfallMetricCard.Criticality.Normal, 100);
00337:
00338:         _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());
00339:         BuildContent();
00340:         RefreshView();
00341:     }
00342:
00343:     private void BuildContent()
00344:     {
00345:         var contentStack = new HBoxContainer();
00346:         contentStack.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00347:         contentStack.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00348:         contentStack.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00349:
00350:         var gridCol = new VBoxContainer();
00351:         gridCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00352:         gridCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00353:         gridCol.SizeFlagsStretchRatio = 1.5f;
00354:         gridCol.AddChild(AshfallUiHelpers.MakeSectionHeader("STANDING PLACES"));
00355:
00356:         var columns = new[]
00357:         {
00358:             new AshfallDataGrid.Column { Header = "Location",   MinWidth = 220, Alignment = AshfallDataGrid.ColumnAlign.Left   },
00359:             new AshfallDataGrid.Column { Header = "Sector",     MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left   },
00360:             new AshfallDataGrid.Column { Header = "Risk",       MinWidth = 150, Alignment = AshfallDataGrid.ColumnAlign.Left   },
00361:             new AshfallDataGrid.Column { Header = "Baseline",   MinWidth = 120, Alignment = AshfallDataGrid.ColumnAlign.Right  },
00362:             new AshfallDataGrid.Column { Header = "Readings",   MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right  },
00363:         };
00364:         _grid = new AshfallDataGrid(columns, showHeader: true, minWidth: 640, minHeight: 380);
00365:         _grid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00366:         _grid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00367:         _grid.OnRowSelected += idx =>
00368:         {
00369:             _selectedIndex = idx;
00370:             RefreshDetail();
00371:         };
00372:         gridCol.AddChild(_grid);
00373:
00374:         var legend = AshfallUiHelpers.MakeSmall(
00375:             "Baseline is the place's own exposure rate in µSv/h. Weather, gear and time on site change the real dose — the ledger keeps the provenance.");
00376:         legend.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00377:         legend.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
00378:         gridCol.AddChild(legend);
00379:
00380:         contentStack.AddChild(gridCol);
00381:
00382:         var detailPanel = AshfallUiHelpers.MakePanel();
00383:         detailPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00384:         detailPanel.SizeFlagsStretchRatio = 0.95f;
00385:         var detailMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
00386:         detailPanel.AddChild(detailMargin);
00387:
00388:         var detailVBox = new VBoxContainer();
00389:         detailVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00390:         detailMargin.AddChild(detailVBox);
00391:
00392:         _detailTitle = new Label { Text = "EXPOSURE GEOGRAPHY" };
00393:         _detailTitle.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeH3);
00394:         _detailTitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00395:         var font = AshfallUiHelpers.LoadFont("res://assets/fonts/BarlowCondensed-SemiBold.ttf");
00396:         if (font != null) _detailTitle.AddThemeFontOverride("font", font);
00397:         detailVBox.AddChild(_detailTitle);
00398:         detailVBox.AddChild(AshfallUiHelpers.MakeSeparator());
00399:         _detailBox = new VBoxContainer();
00400:         _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
00401:         _detailBox.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00402:         detailVBox.AddChild(_detailBox);
00403:
00404:         contentStack.AddChild(detailPanel);
00405:         _shell.SetContent(contentStack);
00406:     }
00407:
00408:     public void Open()
00409:     {
00410:         Visible = true;
00411:         RefreshView();
00412:         QueueRedraw();
00413:     }
00414:
00415:     public void Close() => Visible = false;
00416:
00417:     public override void _UnhandledInput(InputEvent @event)
00418:     {
00419:         if (!Visible) return;
00420:         if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00421:         {
00422:             OnClose?.Invoke();
00423:             GetViewport().SetInputAsHandled();
00424:         }
00425:     }
00426:
00427:     public override void _ExitTree()
00428:     {
00429:         Unbind();
00430:         base._ExitTree();
00431:     }
00432: }
```
# Appendix M — External verification handoff

The following checks are to be run by the owning integrator after writing: character count, SHA-256 revalidation, path-token resolution, duplicate-heading/unsupported-claim scan, and `git diff --check`. The final ledger entry must report actual results, not this template.
