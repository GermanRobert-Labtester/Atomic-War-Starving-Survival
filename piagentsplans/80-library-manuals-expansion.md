# Plan 80 — Library Manuals: Twenty-Four-Manual Knowledge Progression, Skill XP, and Existing Save/Host Seams

> **Rebuild status:** TERMINAL 24-MANUAL CONTENT + CONSUMER/REFERENCE AUDIT
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

The historical baseline was 4,960 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Keep the current twenty-four authored library manuals as the knowledge-progression authority while preserving `LibraryStudySystem` for study jobs, completion, fatigue, comprehension, skill XP, research/knowledge unlocks, and the existing `library_study` save section. The historical 3→15/24 content expansion is complete; the residual is a reference, consumer, and balance audit—not more manuals.

**Bounded outcome:** Audit `library_manuals.json`, `LibraryManualCatalogLoader`, `LibraryStudySystem`, `LibraryStudyHostSession`, `LibraryStudyPanel`, current shelter host composition, skill/research/knowledge catalogs, and focused tests. Classify manual fields as live, display-only, or unresolved and preserve current progression semantics.

**Non-goals:** no second learning/skill owner, no new save section, no automatic research completion, no arbitrary manual growth, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `library_manuals.json` contains 24 manuals.
- VERIFIED CURRENT: `LibraryStudySystem` owns active jobs, completed ids, fatigue, comprehension, and progression results.
- VERIFIED CURRENT: `library_study` is the existing save section/host façade.
- The current execution of every skill/research/knowledge reference is an explicit audit question.
- HISTORICAL RECORD: Plan 80/Wave 39 records the manual expansion; this package does not claim a fresh test run.

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

- `Assets/StreamingAssets/Data/library_manuals.json` exists at 21,889 bytes; SHA-256 `0b8590aac60a274c7e0fb8272ee6563a97bd95aff20f8be6534a8ed73633a6f2`.
- `Assets/StreamingAssets/Data/skills.json` exists at 55,682 bytes; SHA-256 `2bd8a261fc1ce61b1134cce9396810b21131330e85b3acca71dd1dfc0b2848d0`.
- `Assets/StreamingAssets/Data/research_knowledge.json` exists at 21,516 bytes; SHA-256 `6eef977ff53a20c22577be27cddf6564426d7d484cece6b17634c136f29d31c7`.
- `Assets/StreamingAssets/Data/items.json` exists at 390,056 bytes; SHA-256 `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`.

# 3. Required Delta

Replace the old 3→15 brief with a current 24-row catalog/reference/owner audit. Preserve LibraryStudySystem, canonical progression owners, and the existing library_study section.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Which current skill, research, knowledge, facility, and expedition references are live consumers?
Are all prerequisite graphs acyclic and all referenced ids canonical?
Does the current host use the same catalog on fresh boot and restore?
How are power, fatigue, morale, and completion side effects bounded and presented?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| manual definitions and reference validation | `LibraryManualCatalogLoader` | `Assets/Ashfall.Core/LibraryManualCatalogLoader.cs` | Static manual rows and prerequisite/reference checks. |
| study jobs, completion, fatigue, comprehension, and XP | `LibraryStudySystem` | `Assets/Ashfall.Core/LibraryStudySystem.cs` | Sole mutable study owner. |
| host persistence and command façade | `LibraryStudyHostSession / LibraryStudySaveStore` | `src/Host/LibraryStudyHostSession.cs` | Binds catalog, starts/cancels study, ticks, saves. |
| skills/research/knowledge unlocks | `canonical skill/research/knowledge owners` | `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs; Assets/Ashfall.Core/Research` | Manual completion proposes/routs through existing owners. |
| library presentation | `LibraryStudyPanel` | `src/UI/LibraryStudyPanel.cs` | Current catalog/state projection only. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for library-study manual catalog.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Library Manuals: Twenty-Four-Manual Knowledge Progression, Skill XP, and Existing Save/Host Seams                                               │
│ Integration route: DATA-ONLY + current study/reference/balance audit                             │
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

1. **Catalog defines manuals.**
2. **Study system owns jobs/completion.**
3. **Canonical owners own unlocks.**
4. **Host/save façade persists study.**
5. **Panel projects current state.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| manual definitions and reference validation | `LibraryManualCatalogLoader` | `Assets/Ashfall.Core/LibraryManualCatalogLoader.cs` | Static manual rows and prerequisite/reference checks. |
| study jobs, completion, fatigue, comprehension, and XP | `LibraryStudySystem` | `Assets/Ashfall.Core/LibraryStudySystem.cs` | Sole mutable study owner. |
| host persistence and command façade | `LibraryStudyHostSession / LibraryStudySaveStore` | `src/Host/LibraryStudyHostSession.cs` | Binds catalog, starts/cancels study, ticks, saves. |
| skills/research/knowledge unlocks | `canonical skill/research/knowledge owners` | `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs; Assets/Ashfall.Core/Research` | Manual completion proposes/routs through existing owners. |
| library presentation | `LibraryStudyPanel` | `src/UI/LibraryStudyPanel.cs` | Current catalog/state projection only. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load 24 manual definitions through LibraryManualCatalogLoader
2. validate prerequisites, skills, research, knowledge, and related tables
3. bind the catalog to LibraryStudySystem
4. start/cancel a study job through the host session
5. tick comprehension/fatigue/completion in the current day path
6. apply skill/research/knowledge effects only through their canonical owners and persist library_study

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- manual ids are unique and prerequisites resolve
- prerequisite graph is acyclic
- skill XP grants are typed and bounded
- research/knowledge unlocks resolve to current catalogs
- power/facility gates are truthful and do not mutate the manual owner
- study jobs are exclusive/consistent under current rules
- completion is persisted and cannot be rerolled
- fatigue/morale effects are bounded and use current needs owners

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

Contract rules for library-study manual catalog:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`library_manuals.json` remains the manual-definition authority with 24 rows. Current fields include study hours, fatigue, morale, skill XP grants, research/knowledge unlocks, prerequisites, power requirement, loot/expedition/trader references, origin, and complexity. Audit each reference against current catalogs and distinguish a reference from an executable effect.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

Use the existing `library_study` section and `LibraryStudySaveStore`. Persist active jobs and completed manual ids; do not persist authored manual definitions. Any future new reward owner must use its own canonical save/codec and preserve current library state.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

LibraryStudySystem is deterministic for fixed reader/manual/catalog state; day ticks and completion ordering are ordinal-stable. Any future random study variation must use the current seeded stream, not wall-clock/hash order. Paired replay compares job progress, fatigue, completion ids, and unlock events.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

LibraryStudySystem emits current study/completion facts. The host/panel projects them; skill/research/knowledge owners consume completion through existing APIs. A manual row does not itself unlock a skill or research node.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Host/LibraryStudyHostSession.cs` — loads catalog, starts/cancels study, ticks, saves
- `src/UI/LibraryStudyPanel.cs` — renders current manuals/jobs/prerequisites
- `src/Main.ShelterBatch3.cs` — current shelter composition/persistence handoff
- `src/Main.ExpandedShelterSystems.cs` — route/lifecycle integration
- `src/Main.PlayerSurfaces.cs` — player route and open action

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Manual descriptions and schematic summaries should make knowledge feel earned and situated, but they must not promise an unlock or skill level that current owner state cannot grant. Preserve the field-manual tone and avoid copied technical text.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/LibraryStudySystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Library/LibraryStudyContractTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/LibraryStudyCatalogExpansionTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs`

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
| Phase 0 — catalog/reference census | read 24 rows, loader, system, host, save, and tests | all fields/references and current counts are explicit | no undocumented scope or shortcut |
| Phase 1 — owner/consumer trace | map skill/research/knowledge/facility consumers | live versus dormant references are classified | no undocumented scope or shortcut |
| Phase 2 — study/determinism/save audit | check prerequisites, power, fatigue, completion, and replay | no duplicate progression state | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a proven reference/consumer/balance gap is promoted | one owner and focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/library_manuals.json` | READ ONLY; MODIFY only for a proven reference/consumer defect | retain as manual authority |
| `Assets/Ashfall.Core/LibraryManualCatalogLoader.cs` | READ ONLY | loader/reference contract |
| `Assets/Ashfall.Core/LibraryStudySystem.cs` | READ ONLY | study owner |
| `src/Host/LibraryStudyHostSession.cs` | READ ONLY | command/save façade |
| `src/UI/LibraryStudyPanel.cs` | READ ONLY | truthful presentation |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| parallel skill/research state | use canonical owners |
| reference drift | validate current catalogs |
| study/save duplication | use library_study |
| unbounded manual growth | measure content and balance coverage |

# 22. Explicit Non-Goals

- no second learning/skill owner, no new save section, no automatic research completion, no arbitrary manual growth, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for library-study manual catalog are named from current evidence.
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

- A current 24-row reference/consumer/prerequisite matrix.
- A clear distinction between authored unlock references and executed effects.
- A bounded residual only for a proven study/reference/balance gap.

## MUST NOT DO

- create a second learning/skill owner
- persist static manuals as mutable state
- start study without current power/facility gates
- add manuals to reach a count

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/LibraryStudySystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Library/LibraryStudyContractTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/LibraryStudyCatalogExpansionTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read library_manuals.json, LibraryManualCatalogLoader, LibraryStudySystem, LibraryStudyHostSession, LibraryStudyPanel, and focused tests; validate every reference and prerequisite.

# Appendix A — Master expansion authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Authority lines: 5,510; bytes: 635,647

The following slices are read-only design constraints. Live source remains higher authority.

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

### Authority lines 772–777
00772: **B-22 · C15 · Defense-grid ↔ siege math.** Subject: perimeter defense values entering warlord siege/raid resolution; sky-armor values entering orbital-harrow telemetry thresholds. Evidence: warlord siege math and orbital harrow telemetry are canon systems; catalogs live. Route: CORE-EXTENSION. Confidence: PROPOSAL.
00773:
00774: **B-23 · C16 · Difficulty binding consumers (CF-XP01).** Subject: complete difficulty preset scalar consumer binding across systems — the ledger records this line as available and W1-reinforced. Evidence: DR-06. Route: CORE-EXTENSION through the difficulty authority only. Confidence: HIGH CONFIDENCE.
00775:
00776: **B-24 · C17 · Stale-panel refresh sweep.** Subject: sweep panels whose data source gained fields since the panel shipped (verify via `--ui-layout-selftest` and snapshot coverage, then expose truthful current state). Evidence: UI selftest surface is canon. Route: HOST-WIRING only, zero gameplay authority. Confidence: HIGH CONFIDENCE that the class exists; per-panel verification required.
00777:

### Authority lines 832–837
00832: **E-02 · C17 · Snapshot coverage for newest panels.** Subject: extend snapshot coverage to panels shipped since the last snapshot wave. Evidence: snapshot coverage doc is generated and gate-enforced. Route: snapshots + gates. Confidence: HIGH CONFIDENCE.
00833:
00834: **E-03 · C16 · Difficulty preset selection surface.** Subject: preset selection UI bound to the W1 difficulty authority (post-seal, coordinated). Evidence: W1 ACTIVE. Route: HOST-WIRING. Confidence: PROPOSAL, sequence-gated.
00835:
00836: **E-04 · C8 · Radio strip extension points.** Subject: any new radio-conditioned content surfaces through the existing RESCUE SIGNALS strip seams — additive only, sealed surface respected. Evidence: strip shipped and sealed (DR-06). Route: HOST-WIRING. Status: SEALED-adjacent. Confidence: HIGH CONFIDENCE constraint.
00837:

### Authority lines 867–872
00867: **F-06 · Cross · Save-flush cost at day tick.** Subject: measure daily save-flush duration against tick budget for large late-game states (many survivors, full dose ledger, long journals). Evidence: daily save flush is a canon tick step. Route: measurement with a synthetic late-game fixture. Confidence: potential hotspot — requires profiling.
00868:
00869: ## 3.2 Lane G — Testing seeds (G-01 … G-08)
00870:
00871: **G-01 · C10 · Moral-choice flag consumer coverage.** Subject: tests proving every authored flag id has at least one consumer path and every consumer reads a persisted flag (supports F-001 and D-07). Evidence: flags catalog live. Route: focused xUnit, aggregate with per-row failures. Confidence: HIGH CONFIDENCE.
00872:

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

### Authority lines 961–966
00961: **DM-15 — Defense and security (C15).** Owners: perimeter defenses, defense grid, sky defense ordnance and armor, chemical defense, orbital harrow telemetry, interlocks, EMP effects. Live catalogs: `perimeter_defenses`, `defenses`, `sky_defense_ordnance`, `sky_layer_armor_catalog`, `chemical_weapons`, `orbital_harrow_events`, `railway_interlock_catalog`. Hosts: DefenseGrid, SkyDefense, ChemWarfareDefense, OrbitalHarrowTelemetrySystem. Openings: A-30, B-22. Constraint: sky-armor-to-weather bridge already partially built; verify before extending.
00962:
00963: **DM-16 — Progression and meta (C16).** Owners: skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, L10N, mods, settings, input, cohort tuning, apprenticeship, library study. Live catalogs: `skills`, `research_knowledge`, `collectibles`, `trophies`, `difficulty_presets`, `cohort_tuning`, `apprenticeship_catalog`, `library_manuals`, `cultural_archive_tomes`, `codex_entries`, `field_guide`. Hosts: Codex, Research, Collectibles, Difficulty, Apprenticeship, LibraryStudy, Mods, Onboarding, StartingLevel. Openings: B-06, B-23, C-11, D-08, E-03, G-08, J-02. Constraint: XP W1 owns difficulty authority while ACTIVE.
00964:
00965: **DM-17 — Host surface and UI (C17).** Owners: the panel families (v1.0 Part 5.7), shell components, focus navigator, snapshots, a11y, briefings. Design pinned by `DESIGN.md`; a11y by `ACCESSIBILITY.md`; input by the 22-action map. Openings: E-01 through E-10, B-24, F-01. Constraint: zero gameplay authority in panels; every panel exposes existing commands and truthful state.
00966:

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/library_manuals.json`
- Bytes: 21,889; SHA-256: `0b8590aac60a274c7e0fb8272ee6563a97bd95aff20f8be6534a8ed73633a6f2`
- Root keys: `collection_id, manuals, schema_version`
- `manuals`: list[24]; union fields: `category, display_name, expedition_reward_ids, fatigue_per_hour, knowledge_unlocks, loot_table_ids, manual_id, morale_effect, origin_facility, prerequisites, requires_power, research_unlocks, schematic_summary, skill_xp_grants, study_hours_required, technical_complexity_tier, trader_pool_ids`
  - row 1: `{"category":"survival","display_name":"Field Water Filtration","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_water_basics"],"loot_table_ids":["table_loot_farm","table_loot_apartment_block"],"manual_id":"manual_water_filtration","morale_effect":-0.5,"origin_facility":"Municipal Civil Defense Water Board","prerequisites":[],"requires_power":true,"research_unlocks":["knowledge_water_basics"],"schematic_summary":"Multi-stage sand, gravel, and activated charcoal canister diagrams with gravity flow head.","skill_xp_grants":["survival","25"],"study_hours_required":10,"technical_complexity_tier…`
  - row 2: `{"category":"survival","display_name":"Subterranean Hydroponics & Soil Nutrients","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.25,"knowledge_unlocks":["knowledge_hydroponics"],"loot_table_ids":["table_loot_greenhouse","table_loot_farm"],"manual_id":"manual_bunker_hydroponics","morale_effect":0.2,"origin_facility":"State Agricultural Research Station #9","prerequisites":["manual_water_filtration"],"requires_power":true,"research_unlocks":["knowledge_hydroponics"],"schematic_summary":"Nutrient-film technique trough sizing and pH buffer titration tables.","skill_xp_grants":["survival","30"],"study_hours_required":12,"tech…`
  - row 3: `{"category":"survival","display_name":"Pressure Canning & Food Preservation","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.25,"knowledge_unlocks":["knowledge_food_preservation"],"loot_table_ids":["table_loot_apartment_block","table_loot_shopping_center"],"manual_id":"manual_vacuum_preservation","morale_effect":0.1,"origin_facility":"Rural Cooperative Extension Office","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_food_preservation"],"schematic_summary":"High-pressure canning jar gaskets and botulinum heat-inactivation timelines.","skill_xp_grants":["survival","25"],"study_hours_required":10,"t…`
  - row 4: `{"category":"survival","display_name":"Enclosed Apiculture & Colony Thermoregulation","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.25,"knowledge_unlocks":["knowledge_apiculture_ecology"],"loot_table_ids":["table_loot_greenhouse"],"manual_id":"manual_apiculture_and_pollination","morale_effect":0.2,"origin_facility":"Institute of Applied Entomology","prerequisites":["manual_bunker_hydroponics"],"requires_power":true,"research_unlocks":["knowledge_apiculture_ecology"],"schematic_summary":"Insulated observation hive frames with infrared heater tape and sugar syrup dosers.","skill_xp_grants":["survival","35"],"study_hours_r…`
  - row 5: `{"category":"engineering","display_name":"Photovoltaic Maintenance & Inverter Rewiring","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_solar_basics"],"loot_table_ids":["table_loot_power_substation","table_loot_industrial_district"],"manual_id":"manual_solar_maintenance","morale_effect":-0.3,"origin_facility":"Regional Power Authority Substation #4","prerequisites":[],"requires_power":true,"research_unlocks":["knowledge_solar_basics"],"schematic_summary":"Solid-state inverter bridge topologies and bypass diode thermal dissipation.","skill_xp_grants":["crafting","30"],"study_hours_r…`
  - row 6: `{"category":"engineering","display_name":"Pre-War Solid-State Electronics Repair","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.4,"knowledge_unlocks":["knowledge_signal_amplifier_blueprint"],"loot_table_ids":["table_loot_relay_mast","table_loot_military_depot"],"manual_id":"manual_relic_reverse_engineering","morale_effect":-0.4,"origin_facility":"Ministry of Electronics Factory 41","prerequisites":["manual_solar_maintenance"],"requires_power":true,"research_unlocks":["knowledge_signal_amplifier_blueprint"],"schematic_summary":"Surface-mount transistor de-soldering heat profiles and silicon junction tests.","skill…`
  - row 7: `{"category":"engineering","display_name":"Radiation Shielding & Attenuation Matrix","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_radiation_shielding"],"loot_table_ids":["table_loot_industrial_district","table_loot_power_substation"],"manual_id":"manual_radiation_shielding_fabrication","morale_effect":-0.4,"origin_facility":"Silo Civil Engineering Directorate","prerequisites":["manual_solar_maintenance"],"requires_power":true,"research_unlocks":["knowledge_radiation_shielding"],"schematic_summary":"Lead-sheet rolling, borated paraffin sandwich panels, and labyrinth door seams.",…`
  - row 8: `{"category":"engineering","display_name":"Gas Mask Filter Repacking & Seal Testing","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_gas_mask_improved"],"loot_table_ids":["table_loot_military_depot","table_loot_industrial_district"],"manual_id":"manual_gas_mask_canister_rebuild","morale_effect":-0.2,"origin_facility":"Civil Defense Chemical Depot #12","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_gas_mask_improved"],"schematic_summary":"Canister crimp removal, activated charcoal sieve grading, and particulate aerosol smoke tests.","skill_xp_grants":["craft…`
  - row 9: `{"category":"medical","display_name":"Radiation First Aid & Dose Mitigation","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_radiation_basics"],"loot_table_ids":["table_loot_clinic","table_loot_hospital"],"manual_id":"manual_rad_first_aid","morale_effect":-0.4,"origin_facility":"Red Cross Radiation Casualty Protocol Division","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_radiation_basics"],"schematic_summary":"Potassium iodide administration timelines, prodromal symptom triage, and de-clothing washdown.","skill_xp_grants":["medical","30"],"study_hours_required"…`
  - row 10: `{"category":"medical","display_name":"Emergency Trauma & Field Surgery Protocols","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.45,"knowledge_unlocks":["knowledge_field_trauma_surgery"],"loot_table_ids":["table_loot_hospital"],"manual_id":"manual_field_trauma_surgery","morale_effect":-0.7,"origin_facility":"Military Medical Academy Field Hospital Guide","prerequisites":["manual_rad_first_aid"],"requires_power":true,"research_unlocks":["knowledge_field_trauma_surgery"],"schematic_summary":"Arterial tourniquet pressure points, shrapnel debridement, and local anesthesia nerve blocks.","skill_xp_grants":["medical","40"],"st…`
  - row 11: `{"category":"medical","display_name":"Pathogen Containment & Quarantine Protocols","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.4,"knowledge_unlocks":["knowledge_pathogen_containment"],"loot_table_ids":["table_loot_clinic","table_loot_hospital"],"manual_id":"manual_quarantine_epidemiology","morale_effect":-0.6,"origin_facility":"Epidemiological Isolation Bureau","prerequisites":["manual_rad_first_aid"],"requires_power":true,"research_unlocks":["knowledge_pathogen_containment"],"schematic_summary":"Negative pressure room airflow staging, bleach disinfection ratios, and fever ward barrier charts.","skill_xp_grants":["med…`
  - row 12: `{"category":"medical","display_name":"Post-Collapse Pharmacology & Chemical Syntheses","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.45,"knowledge_unlocks":["knowledge_pharmacology_synthesis"],"loot_table_ids":["table_loot_clinic","table_loot_hospital"],"manual_id":"manual_pharmacology_synthesis","morale_effect":-0.5,"origin_facility":"Pharmaceutical Chemical Institute","prerequisites":["manual_quarantine_epidemiology"],"requires_power":true,"research_unlocks":["knowledge_pharmacology_synthesis"],"schematic_summary":"Sulfonamide antibiotic crystallization and botanical tincture distillation curves.","skill_xp_grants":["…`
  - row 13: `{"category":"science","display_name":"Radio Direction Finding & Morse Signal Analysis","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_radio_basics"],"loot_table_ids":["table_loot_relay_mast","table_loot_apartment_block"],"manual_id":"manual_radio_signal_direction","morale_effect":-0.2,"origin_facility":"State Telecommunications Directorate","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_radio_basics"],"schematic_summary":"Loop antenna null-bearing triangulation and Morse signal extraction below noise floor.","skill_xp_grants":["science","30"],"study_hours…`
  - row 14: `{"category":"science","display_name":"Tropospheric Condensation & Silver Iodide Seeding","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_atmospheric_cloud_seeding"],"loot_table_ids":["table_loot_observatory","table_loot_relay_mast"],"manual_id":"manual_cloud_seeding_meteorology","morale_effect":0.0,"origin_facility":"Hydrometeorological Center Station #3","prerequisites":["manual_radio_signal_direction"],"requires_power":true,"research_unlocks":["knowledge_atmospheric_cloud_seeding"],"schematic_summary":"Pyrotechnic flare burner mountings for rocket payloads and thermal updraft tr…`
  - row 15: `{"category":"science","display_name":"High-Frequency Ionospheric Skip & Grayline Propagation","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_ionospheric_propagation"],"loot_table_ids":["table_loot_relay_mast","table_loot_military_depot"],"manual_id":"manual_ionospheric_propagation","morale_effect":-0.1,"origin_facility":"Military Radio Research Laboratory","prerequisites":["manual_radio_signal_direction"],"requires_power":true,"research_unlocks":["knowledge_ionospheric_propagation"],"schematic_summary":"Critical frequency foF2 prediction nomograms and solar storm absorption recove…`
  - row 16: `{"category":"science","display_name":"Seismic Geophone Arrays & Fault Drift Detection","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_seismic_geophone_blueprint"],"loot_table_ids":["table_loot_industrial_district","table_loot_relay_mast"],"manual_id":"manual_subterranean_geophone","morale_effect":-0.3,"origin_facility":"Seismic Survey Institute","prerequisites":["manual_radio_signal_direction"],"requires_power":true,"research_unlocks":["knowledge_seismic_geophone_blueprint"],"schematic_summary":"Moving-coil geophone damping resistors and differential amplifier Wheatstone circuits…`
  - row 17: `{"category":"scavenging","display_name":"Subterranean Fault & Vault Cartography","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_ruin_structural_survey"],"loot_table_ids":["table_loot_industrial_district","table_loot_apartment_block"],"manual_id":"manual_subterranean_cartography","morale_effect":-0.3,"origin_facility":"Metropolitan Civil Tunnel Authority","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_ruin_structural_survey"],"schematic_summary":"Reinforced concrete load-bearing wall identification and collapsed ceiling shoring.","skill_xp_grants":["scave…`
  - row 18: `{"category":"scavenging","display_name":"High-Yield Scrap Extraction & Rigging","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_scavenge_efficiency"],"loot_table_ids":["table_loot_industrial_district","table_loot_military_depot"],"manual_id":"manual_salvage_mechanics","morale_effect":-0.2,"origin_facility":"Heavy Scrap Recovery Depot","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_scavenge_efficiency"],"schematic_summary":"Cold-chisel shearing techniques and block-and-tackle mechanical advantage layouts.","skill_xp_grants":["scavenging","30"],"study_hours_require…`
  - row 19: `{"category":"scavenging","display_name":"Hazmat Vault Breaching & Hot-Zone Infiltration","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.45,"knowledge_unlocks":["knowledge_hazmat_breaching_technique"],"loot_table_ids":["table_loot_military_depot","table_loot_industrial_district"],"manual_id":"manual_hazmat_breaching_drills","morale_effect":-0.6,"origin_facility":"Tactical Civil Defense Rescue Unit #7","prerequisites":["manual_subterranean_cartography"],"requires_power":true,"research_unlocks":["knowledge_hazmat_breaching_technique"],"schematic_summary":"Thermite lance burn rates against 100mm ballistic steel doors …`
  - row 20: `{"category":"scavenging","display_name":"Wasteland Botanical & Mineral Field Guide","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_field_guide_taxonomy"],"loot_table_ids":["table_loot_apartment_block","table_loot_shopping_center"],"manual_id":"manual_wasteland_taxonomy","morale_effect":-0.1,"origin_facility":"State Botanical Herbarium & Mineral Survey","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_field_guide_taxonomy"],"schematic_summary":"Rad-accumulator lichen visual markers and safe saline crust harvest diagnostics.","skill_xp_grants":["scavenging","35"],"s…`
  - row 21: `{"category":"combat","display_name":"Improvised Weapons Fabrication","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.4,"knowledge_unlocks":["knowledge_combat_training"],"loot_table_ids":["table_loot_military_depot","table_loot_industrial_district"],"manual_id":"manual_improvised_weapons","morale_effect":-0.6,"origin_facility":"Partisan Resistance Armorer Notes","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_combat_training"],"schematic_summary":"Smoothbore pipe gun tolerances, spring tempering, and shotgun shell primer reload.","skill_xp_grants":["combat","35"],"study_hours_required":14,"technica…`
  - row 22: `{"category":"combat","display_name":"Precision Match Handloaded Ammunition","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_precision_ballistics"],"loot_table_ids":["table_loot_military_depot","table_loot_police_station"],"manual_id":"manual_ballistic_handloading","morale_effect":-0.4,"origin_facility":"State Marksmanship Training Armory","prerequisites":["manual_improvised_weapons"],"requires_power":true,"research_unlocks":["knowledge_precision_ballistics"],"schematic_summary":"Powder grain weight measurement, lead bullet swaging, and crimp concentricity checks.","skill_xp_grants…`
  - row 23: `{"category":"combat","display_name":"Corridor Defense & Fortified Chokepoints","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.4,"knowledge_unlocks":["knowledge_fortified_chokepoints"],"loot_table_ids":["table_loot_military_depot","table_loot_police_station"],"manual_id":"manual_fortified_chokepoints","morale_effect":-0.5,"origin_facility":"Garrison Fortification Engineering Corps","prerequisites":["manual_improvised_weapons"],"requires_power":false,"research_unlocks":["knowledge_fortified_chokepoints"],"schematic_summary":"Crossfire angles, interlocking firing ports, and sandbag revetment stability formulas.","skill_xp_g…`
  - row 24: `{"category":"combat","display_name":"Perimeter Tripwire & Area Denial Arrays","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_defensive_tripwire_arrays"],"loot_table_ids":["table_loot_military_depot"],"manual_id":"manual_defensive_tripwire_doctrine","morale_effect":-0.4,"origin_facility":"Combat Pioneer Manual #88","prerequisites":["manual_improvised_weapons"],"requires_power":false,"research_unlocks":["knowledge_defensive_tripwire_arrays"],"schematic_summary":"Spring-loaded tension release triggers, trip line concealment, and acoustic bell rattles.","skill_xp_grants":["combat","3…`
- Bytes: 21,889; SHA-256: `0b8590aac60a274c7e0fb8272ee6563a97bd95aff20f8be6534a8ed73633a6f2`
- Root keys: `collection_id, manuals, schema_version`
- `manuals`: list[24]; union fields: `category, display_name, expedition_reward_ids, fatigue_per_hour, knowledge_unlocks, loot_table_ids, manual_id, morale_effect, origin_facility, prerequisites, requires_power, research_unlocks, schematic_summary, skill_xp_grants, study_hours_required, technical_complexity_tier, trader_pool_ids`
  - row 1: `{"category":"survival","display_name":"Field Water Filtration","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_water_basics"],"loot_table_ids":["table_loot_farm","table_loot_apartment_block"],"manual_id":"manual_water_filtration","morale_effect":-0.5,"origin_facility":"Municipal Civil Defense Water Board","prerequisites":[],"requires_power":true,"research_unlocks":["knowledge_water_basics"],"schematic_summary":"Multi-stage sand, gravel, and activated charcoal canister diagrams with gravity flow head.","skill_xp_grants":["survival","25"],"study_hours_required":10,"technical_complexity_tier…`
  - row 2: `{"category":"survival","display_name":"Subterranean Hydroponics & Soil Nutrients","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.25,"knowledge_unlocks":["knowledge_hydroponics"],"loot_table_ids":["table_loot_greenhouse","table_loot_farm"],"manual_id":"manual_bunker_hydroponics","morale_effect":0.2,"origin_facility":"State Agricultural Research Station #9","prerequisites":["manual_water_filtration"],"requires_power":true,"research_unlocks":["knowledge_hydroponics"],"schematic_summary":"Nutrient-film technique trough sizing and pH buffer titration tables.","skill_xp_grants":["survival","30"],"study_hours_required":12,"tech…`
  - row 3: `{"category":"survival","display_name":"Pressure Canning & Food Preservation","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.25,"knowledge_unlocks":["knowledge_food_preservation"],"loot_table_ids":["table_loot_apartment_block","table_loot_shopping_center"],"manual_id":"manual_vacuum_preservation","morale_effect":0.1,"origin_facility":"Rural Cooperative Extension Office","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_food_preservation"],"schematic_summary":"High-pressure canning jar gaskets and botulinum heat-inactivation timelines.","skill_xp_grants":["survival","25"],"study_hours_required":10,"t…`
  - row 4: `{"category":"survival","display_name":"Enclosed Apiculture & Colony Thermoregulation","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.25,"knowledge_unlocks":["knowledge_apiculture_ecology"],"loot_table_ids":["table_loot_greenhouse"],"manual_id":"manual_apiculture_and_pollination","morale_effect":0.2,"origin_facility":"Institute of Applied Entomology","prerequisites":["manual_bunker_hydroponics"],"requires_power":true,"research_unlocks":["knowledge_apiculture_ecology"],"schematic_summary":"Insulated observation hive frames with infrared heater tape and sugar syrup dosers.","skill_xp_grants":["survival","35"],"study_hours_r…`
  - row 5: `{"category":"engineering","display_name":"Photovoltaic Maintenance & Inverter Rewiring","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_solar_basics"],"loot_table_ids":["table_loot_power_substation","table_loot_industrial_district"],"manual_id":"manual_solar_maintenance","morale_effect":-0.3,"origin_facility":"Regional Power Authority Substation #4","prerequisites":[],"requires_power":true,"research_unlocks":["knowledge_solar_basics"],"schematic_summary":"Solid-state inverter bridge topologies and bypass diode thermal dissipation.","skill_xp_grants":["crafting","30"],"study_hours_r…`
  - row 6: `{"category":"engineering","display_name":"Pre-War Solid-State Electronics Repair","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.4,"knowledge_unlocks":["knowledge_signal_amplifier_blueprint"],"loot_table_ids":["table_loot_relay_mast","table_loot_military_depot"],"manual_id":"manual_relic_reverse_engineering","morale_effect":-0.4,"origin_facility":"Ministry of Electronics Factory 41","prerequisites":["manual_solar_maintenance"],"requires_power":true,"research_unlocks":["knowledge_signal_amplifier_blueprint"],"schematic_summary":"Surface-mount transistor de-soldering heat profiles and silicon junction tests.","skill…`
  - row 7: `{"category":"engineering","display_name":"Radiation Shielding & Attenuation Matrix","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_radiation_shielding"],"loot_table_ids":["table_loot_industrial_district","table_loot_power_substation"],"manual_id":"manual_radiation_shielding_fabrication","morale_effect":-0.4,"origin_facility":"Silo Civil Engineering Directorate","prerequisites":["manual_solar_maintenance"],"requires_power":true,"research_unlocks":["knowledge_radiation_shielding"],"schematic_summary":"Lead-sheet rolling, borated paraffin sandwich panels, and labyrinth door seams.",…`
  - row 8: `{"category":"engineering","display_name":"Gas Mask Filter Repacking & Seal Testing","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_gas_mask_improved"],"loot_table_ids":["table_loot_military_depot","table_loot_industrial_district"],"manual_id":"manual_gas_mask_canister_rebuild","morale_effect":-0.2,"origin_facility":"Civil Defense Chemical Depot #12","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_gas_mask_improved"],"schematic_summary":"Canister crimp removal, activated charcoal sieve grading, and particulate aerosol smoke tests.","skill_xp_grants":["craft…`
  - row 9: `{"category":"medical","display_name":"Radiation First Aid & Dose Mitigation","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_radiation_basics"],"loot_table_ids":["table_loot_clinic","table_loot_hospital"],"manual_id":"manual_rad_first_aid","morale_effect":-0.4,"origin_facility":"Red Cross Radiation Casualty Protocol Division","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_radiation_basics"],"schematic_summary":"Potassium iodide administration timelines, prodromal symptom triage, and de-clothing washdown.","skill_xp_grants":["medical","30"],"study_hours_required"…`
  - row 10: `{"category":"medical","display_name":"Emergency Trauma & Field Surgery Protocols","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.45,"knowledge_unlocks":["knowledge_field_trauma_surgery"],"loot_table_ids":["table_loot_hospital"],"manual_id":"manual_field_trauma_surgery","morale_effect":-0.7,"origin_facility":"Military Medical Academy Field Hospital Guide","prerequisites":["manual_rad_first_aid"],"requires_power":true,"research_unlocks":["knowledge_field_trauma_surgery"],"schematic_summary":"Arterial tourniquet pressure points, shrapnel debridement, and local anesthesia nerve blocks.","skill_xp_grants":["medical","40"],"st…`
  - row 11: `{"category":"medical","display_name":"Pathogen Containment & Quarantine Protocols","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.4,"knowledge_unlocks":["knowledge_pathogen_containment"],"loot_table_ids":["table_loot_clinic","table_loot_hospital"],"manual_id":"manual_quarantine_epidemiology","morale_effect":-0.6,"origin_facility":"Epidemiological Isolation Bureau","prerequisites":["manual_rad_first_aid"],"requires_power":true,"research_unlocks":["knowledge_pathogen_containment"],"schematic_summary":"Negative pressure room airflow staging, bleach disinfection ratios, and fever ward barrier charts.","skill_xp_grants":["med…`
  - row 12: `{"category":"medical","display_name":"Post-Collapse Pharmacology & Chemical Syntheses","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.45,"knowledge_unlocks":["knowledge_pharmacology_synthesis"],"loot_table_ids":["table_loot_clinic","table_loot_hospital"],"manual_id":"manual_pharmacology_synthesis","morale_effect":-0.5,"origin_facility":"Pharmaceutical Chemical Institute","prerequisites":["manual_quarantine_epidemiology"],"requires_power":true,"research_unlocks":["knowledge_pharmacology_synthesis"],"schematic_summary":"Sulfonamide antibiotic crystallization and botanical tincture distillation curves.","skill_xp_grants":["…`
  - row 13: `{"category":"science","display_name":"Radio Direction Finding & Morse Signal Analysis","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_radio_basics"],"loot_table_ids":["table_loot_relay_mast","table_loot_apartment_block"],"manual_id":"manual_radio_signal_direction","morale_effect":-0.2,"origin_facility":"State Telecommunications Directorate","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_radio_basics"],"schematic_summary":"Loop antenna null-bearing triangulation and Morse signal extraction below noise floor.","skill_xp_grants":["science","30"],"study_hours…`
  - row 14: `{"category":"science","display_name":"Tropospheric Condensation & Silver Iodide Seeding","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_atmospheric_cloud_seeding"],"loot_table_ids":["table_loot_observatory","table_loot_relay_mast"],"manual_id":"manual_cloud_seeding_meteorology","morale_effect":0.0,"origin_facility":"Hydrometeorological Center Station #3","prerequisites":["manual_radio_signal_direction"],"requires_power":true,"research_unlocks":["knowledge_atmospheric_cloud_seeding"],"schematic_summary":"Pyrotechnic flare burner mountings for rocket payloads and thermal updraft tr…`
  - row 15: `{"category":"science","display_name":"High-Frequency Ionospheric Skip & Grayline Propagation","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_ionospheric_propagation"],"loot_table_ids":["table_loot_relay_mast","table_loot_military_depot"],"manual_id":"manual_ionospheric_propagation","morale_effect":-0.1,"origin_facility":"Military Radio Research Laboratory","prerequisites":["manual_radio_signal_direction"],"requires_power":true,"research_unlocks":["knowledge_ionospheric_propagation"],"schematic_summary":"Critical frequency foF2 prediction nomograms and solar storm absorption recove…`
  - row 16: `{"category":"science","display_name":"Seismic Geophone Arrays & Fault Drift Detection","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_seismic_geophone_blueprint"],"loot_table_ids":["table_loot_industrial_district","table_loot_relay_mast"],"manual_id":"manual_subterranean_geophone","morale_effect":-0.3,"origin_facility":"Seismic Survey Institute","prerequisites":["manual_radio_signal_direction"],"requires_power":true,"research_unlocks":["knowledge_seismic_geophone_blueprint"],"schematic_summary":"Moving-coil geophone damping resistors and differential amplifier Wheatstone circuits…`
  - row 17: `{"category":"scavenging","display_name":"Subterranean Fault & Vault Cartography","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_ruin_structural_survey"],"loot_table_ids":["table_loot_industrial_district","table_loot_apartment_block"],"manual_id":"manual_subterranean_cartography","morale_effect":-0.3,"origin_facility":"Metropolitan Civil Tunnel Authority","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_ruin_structural_survey"],"schematic_summary":"Reinforced concrete load-bearing wall identification and collapsed ceiling shoring.","skill_xp_grants":["scave…`
  - row 18: `{"category":"scavenging","display_name":"High-Yield Scrap Extraction & Rigging","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_scavenge_efficiency"],"loot_table_ids":["table_loot_industrial_district","table_loot_military_depot"],"manual_id":"manual_salvage_mechanics","morale_effect":-0.2,"origin_facility":"Heavy Scrap Recovery Depot","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_scavenge_efficiency"],"schematic_summary":"Cold-chisel shearing techniques and block-and-tackle mechanical advantage layouts.","skill_xp_grants":["scavenging","30"],"study_hours_require…`
  - row 19: `{"category":"scavenging","display_name":"Hazmat Vault Breaching & Hot-Zone Infiltration","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.45,"knowledge_unlocks":["knowledge_hazmat_breaching_technique"],"loot_table_ids":["table_loot_military_depot","table_loot_industrial_district"],"manual_id":"manual_hazmat_breaching_drills","morale_effect":-0.6,"origin_facility":"Tactical Civil Defense Rescue Unit #7","prerequisites":["manual_subterranean_cartography"],"requires_power":true,"research_unlocks":["knowledge_hazmat_breaching_technique"],"schematic_summary":"Thermite lance burn rates against 100mm ballistic steel doors …`
  - row 20: `{"category":"scavenging","display_name":"Wasteland Botanical & Mineral Field Guide","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.3,"knowledge_unlocks":["knowledge_field_guide_taxonomy"],"loot_table_ids":["table_loot_apartment_block","table_loot_shopping_center"],"manual_id":"manual_wasteland_taxonomy","morale_effect":-0.1,"origin_facility":"State Botanical Herbarium & Mineral Survey","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_field_guide_taxonomy"],"schematic_summary":"Rad-accumulator lichen visual markers and safe saline crust harvest diagnostics.","skill_xp_grants":["scavenging","35"],"s…`
  - row 21: `{"category":"combat","display_name":"Improvised Weapons Fabrication","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.4,"knowledge_unlocks":["knowledge_combat_training"],"loot_table_ids":["table_loot_military_depot","table_loot_industrial_district"],"manual_id":"manual_improvised_weapons","morale_effect":-0.6,"origin_facility":"Partisan Resistance Armorer Notes","prerequisites":[],"requires_power":false,"research_unlocks":["knowledge_combat_training"],"schematic_summary":"Smoothbore pipe gun tolerances, spring tempering, and shotgun shell primer reload.","skill_xp_grants":["combat","35"],"study_hours_required":14,"technica…`
  - row 22: `{"category":"combat","display_name":"Precision Match Handloaded Ammunition","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_precision_ballistics"],"loot_table_ids":["table_loot_military_depot","table_loot_police_station"],"manual_id":"manual_ballistic_handloading","morale_effect":-0.4,"origin_facility":"State Marksmanship Training Armory","prerequisites":["manual_improvised_weapons"],"requires_power":true,"research_unlocks":["knowledge_precision_ballistics"],"schematic_summary":"Powder grain weight measurement, lead bullet swaging, and crimp concentricity checks.","skill_xp_grants…`
  - row 23: `{"category":"combat","display_name":"Corridor Defense & Fortified Chokepoints","expedition_reward_ids":["loc_the_allotments"],"fatigue_per_hour":0.4,"knowledge_unlocks":["knowledge_fortified_chokepoints"],"loot_table_ids":["table_loot_military_depot","table_loot_police_station"],"manual_id":"manual_fortified_chokepoints","morale_effect":-0.5,"origin_facility":"Garrison Fortification Engineering Corps","prerequisites":["manual_improvised_weapons"],"requires_power":false,"research_unlocks":["knowledge_fortified_chokepoints"],"schematic_summary":"Crossfire angles, interlocking firing ports, and sandbag revetment stability formulas.","skill_xp_g…`
  - row 24: `{"category":"combat","display_name":"Perimeter Tripwire & Area Denial Arrays","expedition_reward_ids":["loc_denial_cut_substation"],"fatigue_per_hour":0.35,"knowledge_unlocks":["knowledge_defensive_tripwire_arrays"],"loot_table_ids":["table_loot_military_depot"],"manual_id":"manual_defensive_tripwire_doctrine","morale_effect":-0.4,"origin_facility":"Combat Pioneer Manual #88","prerequisites":["manual_improvised_weapons"],"requires_power":false,"research_unlocks":["knowledge_defensive_tripwire_arrays"],"schematic_summary":"Spring-loaded tension release triggers, trip line concealment, and acoustic bell rattles.","skill_xp_grants":["combat","3…`

## `Assets/StreamingAssets/Data/skills.json`
- Bytes: 55,682; SHA-256: `2bd8a261fc1ce61b1134cce9396810b21131330e85b3acca71dd1dfc0b2848d0`
- Root keys: `collection_id, schema_version, skills`
- `skills`: list[161]; union fields: `description, discipline_id, display_name, id, is_expert_skill, skill_bonus, xp_threshold`
  - row 1: `{"description":"Triage and sterile bandaging under rough conditions. Hands that know where the bleeding is before the light finds it.","discipline_id":"medical","display_name":"Field Dressing","id":"skill_field_dressing","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 2: `{"description":"Surgical precision under bombardment or exhaustion. The tremor stops when the work starts, and returns afterward.","discipline_id":"medical","display_name":"Steady Hands","id":"skill_steady_hands","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":120.0}`
  - row 3: `{"description":"Fast patching and duct-tape maintenance on shelter structures. Nothing pretty; everything holding.","discipline_id":"crafting","display_name":"Rough Repairs","id":"skill_rough_repairs","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 4: `{"description":"Builds and repairs with the tools and materials the shelter has left.","discipline_id":"crafting","display_name":"General Crafting","id":"skill_crafting","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 5: `{"description":"Knows how much wear a tool can take and how much abuse a scrap part will accept before it quits.","discipline_id":"crafting","display_name":"Workshop Sense","id":"skill_workshop_sense","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":120.0}`
  - row 6: `{"description":"Pulls intelligible words out of ionospheric static and Morse loops. Other people hear weather; this one hears senders.","discipline_id":"science","display_name":"Signal Ear","id":"skill_signal_ear","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 7: `{"description":"Rigorous scientific deduction untainted by panic or bias. The room calms down when this one starts counting instead of guessing.","discipline_id":"science","display_name":"Cold Analysis","id":"skill_cold_analysis","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":120.0}`
  - row 8: `{"description":"Perimeter awareness that spots incoming hostiles before the ambush springs. Sleeps with one ear on the corridor.","discipline_id":"combat","display_name":"Watchful","id":"skill_watchful","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 9: `{"description":"Navigates shattered streets and dead landmarks by remembering what the rubble used to be.","discipline_id":"scavenging","display_name":"Trail Memory","id":"skill_trail_memory","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 10: `{"description":"A body that has made its peace with calorie deficits and bad bunker air. Runs on less, longer, and complains last.","discipline_id":"survival","display_name":"Hard Living","id":"skill_hard_living","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 11: `{"description":"Immediate jam-clearing drill under fire. Tap, rack, bang — the reflex is faster than the flinch.","discipline_id":"combat","display_name":"Tap-Rack-Bang","id":"skill_tap_rack_bang","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 12: `{"description":"First-shot accuracy from concealed cover. The first one is the only one that matters, and the only one they practice.","discipline_id":"combat","display_name":"Cold Bore","id":"skill_cold_bore","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 13: `{"description":"Pins hostiles behind cover while the squad moves. Not trying to hit; trying to make them stay down.","discipline_id":"combat","display_name":"Suppressing Fire","id":"skill_suppressing_fire","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 14: `{"description":"Lethal hand-to-hand and point-blank work in corridors, where the walls are part of the fight.","discipline_id":"combat","display_name":"Close Quarters","id":"skill_close_quarters","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 15: `{"description":"Rigs deadfalls and tripwires along shelter approaches. Knows exactly where a stranger's boot will land.","discipline_id":"combat","display_name":"Trap Setter","id":"skill_trap_setter","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 16: `{"description":"Spots the high-value cache points in collapsing structures — the places people hide things and never live to collect.","discipline_id":"scavenging","display_name":"Looter's Reflex","id":"skill_looters_reflex","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 17: `{"description":"Morale holds through violent death and carnage. Not cruelty; the shock burned off somewhere else, a long time ago.","discipline_id":"combat","display_name":"Desensitized","id":"skill_desensitized","is_expert_skill":false,"skill_bonus":0.0,"xp_threshold":999999.0}`
  - row 18: `{"description":"Extracts maximum nutrition from spoiled tins and root starch. Nothing gets thrown out until this one has argued with it.","discipline_id":"survival","display_name":"Ration Stretcher","id":"skill_ration_stretcher","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 19: `{"description":"Resists foodborne illness and irradiated-water nausea. Eats first, sets the example, usually survives.","discipline_id":"survival","display_name":"Iron Stomach","id":"skill_iron_stomach","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 20: `{"description":"Ferments mash and distills drinkable alcohol from sugar beet scrap. Fuel, medicine, and courage, in that order.","discipline_id":"survival","display_name":"Wasteland Brewer","id":"skill_wasteland_brewer","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 21: `{"description":"Clean field dressing of irradiated fauna; the bonus yield is knowing which parts are safe to keep.","discipline_id":"survival","display_name":"The Butcher","id":"skill_butcher","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 22: `{"description":"Compounds clean medicinal compounds from chemical precursors. Measures twice because the second measure might not exist.","discipline_id":"medical","display_name":"Pharmacologist","id":"skill_pharmacologist","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 23: `{"description":"Identifies edible, medicinal, and bioluminescent bunker fungi. Knows which ones glow, which ones kill, and which ones do both.","discipline_id":"science","display_name":"Mycology","id":"skill_mycology","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 24: `{"description":"Improvises replacement gears from copper pipe and solder. The machine never asks where the part came from.","discipline_id":"crafting","display_name":"Jury-Rigger","id":"skill_jury_rigger","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 25: `{"description":"Reinforces tunnel ceilings and vault bulkheads against seismic load. Reads cracks the way other people read sentences.","discipline_id":"crafting","display_name":"Structural Engineer","id":"skill_structural_engineer","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 26: `{"description":"Balances duct pressure and cleans blower impellers. The shelter breathes evenly and nobody knows whose doing it is.","discipline_id":"crafting","display_name":"HVAC Technician","id":"skill_hvac_tech","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 27: `{"description":"Harvests pristine copper wiring and steel rebar from ruins. Knows which walls are load-bearing and which are just tired.","discipline_id":"crafting","display_name":"Scrapper","id":"skill_scrapper","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 28: `{"description":"Excavates bunker expansion tunnels safely. Shores as they go; the dark is a coworker, not an enemy.","discipline_id":"crafting","display_name":"The Sandhog","id":"skill_sandhog","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 29: `{"description":"Optimizes heat exchangers and steam recycling efficiency. Heat is a currency and this one runs the mint.","discipline_id":"science","display_name":"Thermodynamics","id":"skill_thermodynamics","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 30: `{"description":"Treats severe puncture wounds in dark, shaking shelters. Steady because the patient can feel the difference.","discipline_id":"medical","display_name":"Steady Hands (Field)","id":"skill_steady_hands_field","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 31: `{"description":"Decisive emergency prioritization during hostile incursions. Chooses fast, chooses right, and carries the choosing afterward.","discipline_id":"medical","display_name":"Triage Under Fire","id":"skill_triage_under_fire","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 32: `{"description":"Precision tracking of tissue rem accumulation and chelation timing. Reads a dose ledger like a biography.","discipline_id":"medical","display_name":"Radiologist","id":"skill_radiologist","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 33: `{"description":"Pathological knowledge that reveals organ vulnerabilities and autopsy findings. The body keeps no secrets from this one, only paperwork.","discipline_id":"medical","display_name":"Anatomist","id":"skill_anatomist","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 34: `{"description":"Rapid trauma stabilization that beats exsanguination by minutes. The minutes are the whole discipline.","discipline_id":"medical","display_name":"Paramedic","id":"skill_paramedic","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 35: `{"description":"Optimal rucksack strapping for expedition carry capacity. The load never shifts, so the legs never fail.","discipline_id":"scavenging","display_name":"Pack Mule","id":"skill_pack_mule","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 36: `{"description":"Silent movement over glass, rubble, and creaking floorboards. The floorboards give no testimony.","discipline_id":"scavenging","display_name":"Light Step","id":"skill_light_step","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 37: `{"description":"Recognizes structural collapse patterns and shortcut alleys. The city is a map, and this one reads the tears in it.","discipline_id":"scavenging","display_name":"Urban Pathfinder","id":"skill_urban_pathfinder","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 38: `{"description":"Combat superiority in absolute darkness and ash blizzards. Others fear the dark; the dark knows this one by name.","discipline_id":"combat","display_name":"Night Terror","id":"skill_night_terror","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 39: `{"description":"Finds edible lichen and unpoisoned roots in dead scrubland. Green enough to eat is a color most people miss.","discipline_id":"survival","display_name":"Forager","id":"skill_forager","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 40: `{"description":"Diffuses shelter panic and resolves violent disputes with words. Lowers the volume of a room before lowering the stakes.","discipline_id":"survival","display_name":"De-Escalator","id":"skill_de_escalator","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 41: `{"description":"Impeccable inventory audits that prevent waste and theft. The ledger is a weapon, and this one keeps it sharp.","discipline_id":"scavenging","display_name":"Quartermaster","id":"skill_quartermaster","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 42: `{"description":"Drives work shifts with rigorous efficiency and stamina. Hated at six, thanked at midnight, right both times.","discipline_id":"survival","display_name":"Taskmaster","id":"skill_taskmaster","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 43: `{"description":"Life-saving surgical intervention against impossible odds. The odds are stated honestly, then ignored.","discipline_id":"medical","display_name":"Miracle Worker","id":"skill_miracle_worker","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 44: `{"description":"Refines chemical reagents and anti-rad chelation cocktails from raw minerals. Turns rocks into second chances.","discipline_id":"science","display_name":"Alchemist","id":"skill_alchemist","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 45: `{"description":"Isolates animal-borne pathogens before cross-species outbreak. Watches the animals so the people don't have to learn from them.","discipline_id":"medical","display_name":"Zoonotic Expert","id":"skill_zoonotic_expert","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 46: `{"description":"Unwavering emotional stability that sets the shelter's morale baseline. The calm is not the absence of fear; it is furniture.","discipline_id":"survival","display_name":"Anchor","id":"skill_anchor","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 47: `{"description":"Complete psychological numbing to catastrophic casualty events. Still does the paperwork for every one. Especially every one.","discipline_id":"survival","display_name":"Death Blind","id":"skill_death_blind","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 48: `{"description":"Tactical coordination that boosts squad firepower. One voice turns five guns into a single decision.","discipline_id":"combat","display_name":"Warlord","id":"skill_warlord","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 49: `{"description":"Commands the respect that prevents mutiny and faction infighting. Walks into rooms that were about to become corridors.","discipline_id":"survival","display_name":"Peacekeeper","id":"skill_peacekeeper","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 50: `{"description":"Exceptional physical constitution; blunt trauma becomes an inconvenience. The doorframe disagrees with this one, and the doorframe loses.","discipline_id":"combat","display_name":"Juggernaut","id":"skill_juggernaut","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 51: `{"description":"Mastery of wilderness ambush and silent stalker tactics. The wasteland's food chain has an edit in it.","discipline_id":"combat","display_name":"Apex Predator","id":"skill_apex_predator","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 52: `{"description":"Deep self-reliance that lowers daily food and water consumption. Needs less, and needs it later.","discipline_id":"survival","display_name":"Survivalist","id":"skill_survivalist","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 53: `{"description":"Engineers high-pressure plumbing, valves, and desalination rigs. Water obeys, grudgingly, and on schedule.","discipline_id":"crafting","display_name":"Hydraulic Master","id":"skill_hydraulic_master","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 54: `{"description":"Instinctive understanding of high-voltage conduits and substations. Knows which wires still remember being alive.","discipline_id":"crafting","display_name":"Grid Walker","id":"skill_grid_walker","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 55: `{"description":"Reinforced blast construction and seismic shock isolation. Builds doors meant to outlive the argument about opening them.","discipline_id":"crafting","display_name":"Vault Builder","id":"skill_vault_builder","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 56: `{"description":"Maintains expedition engines, chassis, and generators. Talks to them, and — this is the unsettling part — they answer.","discipline_id":"crafting","display_name":"Grease Monkey","id":"skill_grease_monkey","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 57: `{"description":"Compounds polymer plastics and synthetic insulation. The new world's materials, built from the old world's mistakes.","discipline_id":"science","display_name":"Synthesizer","id":"skill_synthesizer","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 58: `{"description":"Botanical intuition that maximizes greenhouse yield. The plants lean toward this one, and nobody can explain it.","discipline_id":"survival","display_name":"Gaia","id":"skill_gaia","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 59: `{"description":"Sprint stamina that cuts travel time across open ash plains. Distance is a suggestion; the horizon is a habit.","discipline_id":"scavenging","display_name":"Wasteland Runner","id":"skill_wasteland_runner","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 60: `{"description":"Complete concealment from optics, infrared, and auditory patrols. The guard report describes a draft.","discipline_id":"scavenging","display_name":"Ghost","id":"skill_ghost","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 61: `{"description":"Predicts ash blizzards and harrow fronts hours before they arrive. Watches the sky the way other people watch doors.","discipline_id":"science","display_name":"Stormcaller","id":"skill_stormcaller","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 62: `{"description":"Remarkable biological resistance to chronic background gamma. The dosimeter keeps a record this one argues with.","discipline_id":"survival","display_name":"Rad Walker","id":"skill_rad_walker","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 63: `{"description":"Accelerated multi-disciplinary learning across technical fields. One question away from someone else's specialty.","discipline_id":"science","display_name":"Polymath","id":"skill_polymath","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 64: `{"description":"Charismatic rhetoric capable of swaying hostile crowds. Believes the speech, which is why the speech works.","discipline_id":"survival","display_name":"Demagogue","id":"skill_demagogue","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 65: `{"description":"Mentors novices and accelerates apprentice progression. Measures success in other people's competence.","discipline_id":"survival","display_name":"Shepherd","id":"skill_shepherd","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 66: `{"description":"Investigates conspiracy records and uncovers buried bunker archives. Asks the second question, which is where the truth lives.","discipline_id":"science","display_name":"Muckraker","id":"skill_muckraker","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 67: `{"description":"Radio resonance that calms listeners across the dial. People who have never met this one sleep better anyway.","discipline_id":"science","display_name":"Voice of the Wastes","id":"skill_voice_of_the_wastes","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 68: `{"description":"Culinary mastery that turns wasteland rations into morale feasts. The same tin twice: once as food, once as occasion.","discipline_id":"survival","display_name":"Iron Chef","id":"skill_iron_chef","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 69: `{"description":"Endures prolonged double shifts without fatigue collapse. The bunker's clocks are wrong about this one, consistently.","discipline_id":"survival","display_name":"Tireless","id":"skill_tireless","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 70: `{"description":"Immunity to toxic particulate and chemical fume inhalation. Lungs that made terms with the smoke.","discipline_id":"survival","display_name":"Asbestos","id":"skill_asbestos","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 71: `{"description":"Forges reinforced ballistic plates and services military weaponry. The armor is the argument, and the argument is steel.","discipline_id":"crafting","display_name":"Armorer","id":"skill_armorer","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 72: `{"description":"Modifies electronics and optical sensors for maximum output. Everything leaves better than it arrived, and slightly dangerous.","discipline_id":"crafting","display_name":"Tinkerer","id":"skill_tinkerer","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 73: `{"description":"Preserves pre-war literature, logs, and cultural records. Keeps the before alive because the after will ask.","discipline_id":"science","display_name":"Lorekeeper","id":"skill_lorekeeper","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 74: `{"description":"Combat bonus against fanatical cultists and violent raiders. Faith is fine; this one has studied where it stands.","discipline_id":"combat","display_name":"Zealot's Bane","id":"skill_zealots_bane","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 75: `{"description":"Metabolic tolerance that prevents dependency on heavy combat stimulants. The body refuses to borrow strength it cannot repay.","discipline_id":"medical","display_name":"Chem Resistant","id":"skill_chem_resistant","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 76: `{"description":"Shields companions from direct attack during tactical withdrawals. Steps between — a trained motion and a chosen one.","discipline_id":"combat","display_name":"Protector","id":"skill_protector","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 77: `{"description":"Guides shelter decisions with matriarchal wisdom and authority. The room settles when she sits down, before she speaks.","discipline_id":"survival","display_name":"Matriarch","id":"skill_matriarch","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 78: `{"description":"Absolute resolve that upholds bunker cohesion through crisis. Carries the weight; does not discuss the weight.","discipline_id":"survival","display_name":"Pillar of Atlas","id":"skill_pillar_of_atlas","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 79: `{"description":"Advanced cartography and threat detection across uncharted sectors. Maps the places, and the reasons to avoid them.","discipline_id":"scavenging","display_name":"Wasteland Scout","id":"skill_wasteland_scout","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 80: `{"description":"Born into the fallout; instinctive adaptation to surface hazards. Never saw the sky before, and reads it perfectly.","discipline_id":"survival","display_name":"Child of the Ash","id":"skill_child_of_the_ash","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 81: `{"description":"Optimizes strategic resource allocation with emotionless efficiency. The arithmetic is right, and the arithmetic is watched.","discipline_id":"science","display_name":"Cold Calculus","id":"skill_cold_calculus","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 82: `{"description":"Grim surgical efficiency honed during the worst starvation crisis. Learned what a body is for when food stopped being one.","discipline_id":"survival","display_name":"Butcher of Day 30","id":"skill_butcher_of_day_30","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 83: `{"description":"Leverages trade and diplomatic secrets to extract favorable terms. Collects truths the way others collect tins.","discipline_id":"survival","display_name":"Master Manipulator","id":"skill_master_manipulator","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 84: `{"description":"Safekeeps rare materials and prevents resource decay. Nothing leaves the hoard without a reason, and reasons are rationed.","discipline_id":"scavenging","display_name":"Dragon's Hoard","id":"skill_dragons_hoard","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 85: `{"description":"Deep doctrine mastery that reduces friendly combat losses. Wins the fights that do not need fighting, first.","discipline_id":"combat","display_name":"Art of War","id":"skill_art_of_war","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 86: `{"description":"Precision breaching and explosive ordnance disposal. Argues with bombs politely, in their own language.","discipline_id":"combat","display_name":"Demolitions Expert","id":"skill_demolitions_expert","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 87: `{"description":"Concealed long-range sniping without revealing position. The shot arrives before the direction does.","discipline_id":"combat","display_name":"Ghost Shooter","id":"skill_ghost_shooter","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 88: `{"description":"Optimizes trade convoy routes and warehouse turnover. Sees the whole road at once: every axle, every bribe, every delay.","discipline_id":"scavenging","display_name":"Supply Chain Master","id":"skill_supply_chain_master","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 89: `{"description":"Unnatural vitality and swift injury recovery. The body decided the wasteland was not finished with its argument.","discipline_id":"survival","display_name":"Reclaimed Youth","id":"skill_reclaimed_youth","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 90: `{"description":"Psychological counseling that restores fractured minds. Listens the way a surgeon operates: patiently, and with clean hands.","discipline_id":"medical","display_name":"Soul Weaver","id":"skill_soul_weaver","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 91: `{"description":"Enhanced performance when operating solo on expeditions. Company is a luxury; solitude is a discipline.","discipline_id":"survival","display_name":"Lone Wolf","id":"skill_lone_wolf","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 92: `{"description":"Spreads pragmatic hope that shields allies from despair. Not sunshine — a handhold. Points at it without promising.","discipline_id":"survival","display_name":"Grounded Optimist","id":"skill_grounded_optimist","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 93: `{"description":"Inspires the devotion that unites disparate survivor factions. People behave better in rooms this one walks into, and afterward.","discipline_id":"survival","display_name":"Living Saint","id":"skill_living_saint","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 94: `{"description":"Dedication to patient care with zero ego or reluctance. Once kept score of a wing with their name on it; now keeps charts.","discipline_id":"medical","display_name":"Humbled Healer","id":"skill_humbled_healer","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 95: `{"description":"Total immunity to chemical cravings and withdrawal penalties. Counts the days in both directions and starts from the same one, now.","discipline_id":"survival","display_name":"Clean and Sober","id":"skill_clean_and_sober","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 96: `{"description":"Unblinking vigilance that prevents nocturnal breaches. Knows every hour of every night personally.","discipline_id":"combat","display_name":"The Watcher","id":"skill_the_watcher","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 97: `{"description":"Sensory acuity that detects hidden traps and micro-contaminants. Scared of exactly the right things, on schedule.","discipline_id":"scavenging","display_name":"Hyper Aware","id":"skill_hyper_aware","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 98: `{"description":"Mastery of incendiary weapons and flame traps. Watches fire the way others watch water — for what it does next.","discipline_id":"combat","display_name":"Fire Breather","id":"skill_fire_breather","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 99: `{"description":"Acoustic echolocation in dark tunnels and flooded shafts. Sees with the ears; the dark is just a medium.","discipline_id":"science","display_name":"Sonar","id":"skill_sonar","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 100: `{"description":"Assembles complex machines from mismatched salvage. The machine does not know its parts have never met.","discipline_id":"crafting","display_name":"Improvised Engineering","id":"skill_improvised_engineering","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 101: `{"description":"Bizarre biological symbiosis with low-level background radiation. Where others get sick, this one gets — better. Nobody says it out loud.","discipline_id":"survival","display_name":"Radiotrophic","id":"skill_radiotrophic","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 102: `{"description":"Locates intact pre-war relics in extreme hotspots. Goes where the counters scream and comes back with quiet things.","discipline_id":"scavenging","display_name":"Apex Scavenger","id":"skill_apex_scavenger","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 103: `{"description":"Absolute calm during traumatic surgery and high-stakes crafting. The pulse does not rise; the work does not shake.","discipline_id":"survival","display_name":"Zen State","id":"skill_zen_state","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 104: `{"description":"Selective breeding of radiation-hardened crop strains. Plays the long game in seasons and wins it in grams.","discipline_id":"science","display_name":"Master Geneticist","id":"skill_master_geneticist","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 105: `{"description":"Iron-fisted guard duty that prevents internal unrest. Stands where nobody wants to be tested, and nobody tests.","discipline_id":"combat","display_name":"The Enforcer","id":"skill_the_enforcer","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 106: `{"description":"Renown that unlocks instant respect and trade concessions. The name arrives at settlements before the person does.","discipline_id":"survival","display_name":"Legend of the Wastes","id":"skill_legend_of_the_wastes","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 107: `{"description":"Drafts faction treaties and neutral accords. Writes in the grammar of shared interest, the only grammar everyone reads.","discipline_id":"survival","display_name":"The Statesman","id":"skill_the_statesman","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 108: `{"description":"Interfaces neural cyber-implants and prosthetic limbs. Speaks fluent machine to the parts that used to be a person.","discipline_id":"science","display_name":"Cybernetics","id":"skill_cybernetics","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 109: `{"description":"Uncovers propaganda and restores verified historical truth. Checks the story against the ledger; the ledger always wins.","discipline_id":"science","display_name":"Beacon of Truth","id":"skill_beacon_of_truth","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 110: `{"description":"Forensic autopsy insight identifying microscopic viral vectors. The dead testify; this one takes the statement correctly.","discipline_id":"medical","display_name":"Master Pathologist","id":"skill_master_pathologist","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 111: `{"description":"Controls critical commodity markets for maximum economic leverage. Does not corner the market — corners the thing the market needs.","discipline_id":"survival","display_name":"Monopolist","id":"skill_monopolist","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 112: `{"description":"Navigates subterranean ruins and flooded mine complexes. Down is a direction with its own weather, and this one dresses for it.","discipline_id":"scavenging","display_name":"Deep Delver","id":"skill_deep_delver","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 113: `{"description":"Eliminates freight waste and streamlines supply chains. Every crate has a story; this one edits out the boring parts.","discipline_id":"scavenging","display_name":"Logistics Master","id":"skill_logistics_master","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 114: `{"description":"Smelts Damascus alloys and precision-casts machine parts. Folds steel like a letter nobody else can open.","discipline_id":"crafting","display_name":"Forge Master","id":"skill_forge_master","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 115: `{"description":"Decontaminates biological and chemical hotspots completely. Cleans the room, the air, and the record. In that order.","discipline_id":"medical","display_name":"Sanitization Expert","id":"skill_sanitization_expert","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 116: `{"description":"Clears radioactive timber and harvests dry lumber. Knows which trees are fuel, which are shelter, and which are warnings.","discipline_id":"survival","display_name":"Deforester","id":"skill_deforester","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 117: `{"description":"Maps epidemic transmission vectors across wasteland trade nodes. Draws the sickness's route before the sickness does.","discipline_id":"medical","display_name":"Epidemiologist","id":"skill_epidemiologist","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 118: `{"description":"Navigates by starlight and sextant when GPS and compasses fail. The old sky still works; it never signed anything.","discipline_id":"scavenging","display_name":"Celestial Navigator","id":"skill_celestial_navigator","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 119: `{"description":"Catalogs and preserves fragile parchment and magnetic tapes. Cotton gloves in a world that has forgotten what soft means.","discipline_id":"science","display_name":"Archivist","id":"skill_archivist","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 120: `{"description":"Detects counterfeit currency and fraudulent barter ledgers. Reads a forgery the way a medic reads a cough.","discipline_id":"survival","display_name":"Auditor","id":"skill_auditor","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 121: `{"description":"Plays acoustic instruments that lift bunker spirits during blackouts. The blackout is the audience; the audience is everybody.","discipline_id":"survival","display_name":"Maestro","id":"skill_maestro","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 122: `{"description":"Smuggles critical medicine through enemy siege cordons. The cargo is light and the reason is heavy.","discipline_id":"scavenging","display_name":"Blockade Runner","id":"skill_blockade_runner","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 123: `{"description":"Ruthless close-range finishing strikes against armored foes. The armor has a gap; the patience finds it.","discipline_id":"combat","display_name":"Executioner","id":"skill_executioner","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 124: `{"description":"Blends into darkness to evade searchlights and thermal sensors. The light bends its reports around this one.","discipline_id":"scavenging","display_name":"Shadow","id":"skill_shadow","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 125: `{"description":"Infiltrates rival settlements using forged insignia and uniforms. Wears the enemy's paperwork better than the enemy does.","discipline_id":"survival","display_name":"Master of Disguise","id":"skill_master_of_disguise","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 126: `{"description":"Restores ruined engine blocks in half the normal time. Listens once; the engine tells everything.","discipline_id":"crafting","display_name":"Mechanic Prodigy","id":"skill_mechanic_prodigy","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 127: `{"description":"Secures non-aggression pacts with suspicious wasteland tribes. Sits down first, unarmed, and it keeps working.","discipline_id":"survival","display_name":"Diplomat","id":"skill_diplomat","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 128: `{"description":"Mastery of improvised melee weapons in brutal pit fights. Everything is a weapon; the trick is knowing which end.","discipline_id":"combat","display_name":"Wasteland Gladiator","id":"skill_wasteland_gladiator","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 129: `{"description":"Organizes entire hospital wards and triage protocols. Runs the clinic like a watch, because it is one.","discipline_id":"medical","display_name":"Chief of Medicine","id":"skill_chief_of_medicine","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 130: `{"description":"Pilots aerial micro-drones for over-the-horizon reconnaissance. Sees the wasteland from the only angle it cannot hide from.","discipline_id":"science","display_name":"Drone Operator","id":"skill_drone_operator","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 131: `{"description":"Radio singing that calms bunker distress panics. One voice, one frequency, and the panic sits down to listen.","discipline_id":"science","display_name":"Choir of One","id":"skill_choir_of_one","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 132: `{"description":"Interlocking defensive maneuvers among tight-knit squads. Five people moving like one animal with five opinions.","discipline_id":"combat","display_name":"Hive Tactics","id":"skill_hive_tactics","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 133: `{"description":"Communal first-aid and wound dressing across shared dormitories. Everyone's hands trained; everyone's hands busy.","discipline_id":"medical","display_name":"Hive Healing","id":"skill_hive_healing","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 134: `{"description":"Decodes classified wartime telegrams and bunker transcripts. The war kept a diary; this one reads it aloud.","discipline_id":"science","display_name":"Truth Seeker","id":"skill_truth_seeker","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 135: `{"description":"Thrives in untamed irradiated wilderness without manufactured tools. The wasteland stopped trying to kill this one out of respect.","discipline_id":"survival","display_name":"Wildman","id":"skill_wildman","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 136: `{"description":"Survives catastrophic lethal injury once per campaign. Died once, on schedule; the paperwork is still pending.","discipline_id":"survival","display_name":"Second Life","id":"skill_second_life","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 137: `{"description":"Immunity to psychic shock, trauma panic, and despair flags. The mind is a door that locks from the inside and stays locked.","discipline_id":"survival","display_name":"Iron Will","id":"skill_iron_will","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 138: `{"description":"Eavesdrops on raider radio comms and unencrypted walkie frequencies. The raiders never encrypted; they still do not know.","discipline_id":"science","display_name":"Unseen Listener","id":"skill_unseen_listener","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 139: `{"description":"Drives brutal barter bargains that maximize shelter profit. Fair is a direction, not a destination.","discipline_id":"survival","display_name":"Ruthless Capitalist","id":"skill_ruthless_capitalist","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 140: `{"description":"Unmatched raw intellect mastering new disciplines at triple speed. Boredom is the only hazard this one reports.","discipline_id":"science","display_name":"Prodigy","id":"skill_prodigy","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 141: `{"description":"Inspiring battle cries that boost squad accuracy and courage. The voice arrives before the order and does half its work.","discipline_id":"combat","display_name":"Commander","id":"skill_commander","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 142: `{"description":"Hydraulic bionic limb with crushing grip and recoil compensation. The arm keeps the receipts; the shoulder keeps the score.","discipline_id":"combat","display_name":"Cyber Arm","id":"skill_cyber_arm","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 143: `{"description":"Atones for past sins through self-sacrifice and labor. The debt is internal, and the payments are visible.","discipline_id":"survival","display_name":"Redemption","id":"skill_redemption","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 144: `{"description":"Pushes machinery beyond safety margins for emergency output bursts. The machine survives; the machine is also offended.","discipline_id":"crafting","display_name":"Overclocked","id":"skill_overclocked","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 145: `{"description":"Defends defenseless refugees with relentless protective fury. Gentle at the table; weather in the doorway.","discipline_id":"combat","display_name":"Wasteland Guardian","id":"skill_wasteland_guardian","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 146: `{"description":"Total mastery of wasteland knowledge, maps, and survivor behavior. Knows who owes what, who lies, and where the water hides.","discipline_id":"science","display_name":"Omniscience","id":"skill_omniscience","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 147: `{"description":"Advanced invasive trauma procedures and sterile resection under austerity. Sterility is a discipline, not a supply.","discipline_id":"medical","display_name":"Field Surgery","id":"skill_field_surgery","is_expert_skill":false,"skill_bonus":0.15,"xp_threshold":999999.0}`
  - row 148: `{"description":"Operates ceramic, sand, and charcoal filter stages for maximum potable yield. Stands between the water and the mouth.","discipline_id":"survival","display_name":"Water Filtration","id":"skill_water_filtration","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 149: `{"description":"Vacuum-tube alignment, antenna tuning, and transmitter circuit patching. The voice in the dark depends on this solder.","discipline_id":"science","display_name":"Radio Repair","id":"skill_radio_repair","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 150: `{"description":"Foundational reading and written communication enabling library study and technical transcription.","discipline_id":"science","display_name":"Basic Literacy & Writing","id":"skill_reading_comprehension","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 151: `{"description":"Arithmetic, measurement calculations, and basic problem solving.","discipline_id":"science","display_name":"Basic Numeracy & Logic","id":"skill_mathematical_logic","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 152: `{"description":"Conflict resolution, empathy, and collective shelter duty adherence.","discipline_id":"survival","display_name":"Social Cooperation & Ethics","id":"skill_communal_diplomacy","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 153: `{"description":"Hazard recognition, airlock decontamination drills, and water rationing habits.","discipline_id":"survival","display_name":"Bunker & Wasteland Survival","id":"skill_radiation_awareness","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 154: `{"description":"Workshop tool handling, weld repair, and scrap reclamation.","discipline_id":"crafting","display_name":"Mechanical Crafting & Repair","id":"skill_machining_basics","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 155: `{"description":"Bandaging, antiseptics, trauma stabilization, and symptom identification.","discipline_id":"medical","display_name":"First Aid & Human Physiology","id":"skill_field_triage","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 156: `{"description":"Map reading, route plotting through ruins, and terrain hazard evasion.","discipline_id":"scavenging","display_name":"Scouting & Surface Navigation","id":"skill_cartography","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 157: `{"description":"Firearm discipline, defensive positioning, and emergency lockdown breach containment.","discipline_id":"combat","display_name":"Shelter Defense & Weapon Safety","id":"skill_firearm_handling","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 158: `{"description":"Generator maintenance, air filtration overhaul, and electrical bus troubleshooting.","discipline_id":"crafting","display_name":"Power & Life Support Engineering","id":"skill_reactor_maintenance","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 159: `{"description":"Medication synthesis, sterile surgical procedures, and radiation sickness counter-therapy.","discipline_id":"medical","display_name":"Pharmacology & Advanced Care","id":"skill_surgery_assistance","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 160: `{"description":"Squad maneuvering, risk calculation under fire, and retreat protocols.","discipline_id":"combat","display_name":"Expedition Leadership & Tactics","id":"skill_patrol_command","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - ... 1 additional rows omitted from the compact audit; the complete current file is identified above ...
- Bytes: 55,682; SHA-256: `2bd8a261fc1ce61b1134cce9396810b21131330e85b3acca71dd1dfc0b2848d0`
- Root keys: `collection_id, schema_version, skills`
- `skills`: list[161]; union fields: `description, discipline_id, display_name, id, is_expert_skill, skill_bonus, xp_threshold`
  - row 1: `{"description":"Triage and sterile bandaging under rough conditions. Hands that know where the bleeding is before the light finds it.","discipline_id":"medical","display_name":"Field Dressing","id":"skill_field_dressing","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 2: `{"description":"Surgical precision under bombardment or exhaustion. The tremor stops when the work starts, and returns afterward.","discipline_id":"medical","display_name":"Steady Hands","id":"skill_steady_hands","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":120.0}`
  - row 3: `{"description":"Fast patching and duct-tape maintenance on shelter structures. Nothing pretty; everything holding.","discipline_id":"crafting","display_name":"Rough Repairs","id":"skill_rough_repairs","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 4: `{"description":"Builds and repairs with the tools and materials the shelter has left.","discipline_id":"crafting","display_name":"General Crafting","id":"skill_crafting","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 5: `{"description":"Knows how much wear a tool can take and how much abuse a scrap part will accept before it quits.","discipline_id":"crafting","display_name":"Workshop Sense","id":"skill_workshop_sense","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":120.0}`
  - row 6: `{"description":"Pulls intelligible words out of ionospheric static and Morse loops. Other people hear weather; this one hears senders.","discipline_id":"science","display_name":"Signal Ear","id":"skill_signal_ear","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 7: `{"description":"Rigorous scientific deduction untainted by panic or bias. The room calms down when this one starts counting instead of guessing.","discipline_id":"science","display_name":"Cold Analysis","id":"skill_cold_analysis","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":120.0}`
  - row 8: `{"description":"Perimeter awareness that spots incoming hostiles before the ambush springs. Sleeps with one ear on the corridor.","discipline_id":"combat","display_name":"Watchful","id":"skill_watchful","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 9: `{"description":"Navigates shattered streets and dead landmarks by remembering what the rubble used to be.","discipline_id":"scavenging","display_name":"Trail Memory","id":"skill_trail_memory","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 10: `{"description":"A body that has made its peace with calorie deficits and bad bunker air. Runs on less, longer, and complains last.","discipline_id":"survival","display_name":"Hard Living","id":"skill_hard_living","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":50.0}`
  - row 11: `{"description":"Immediate jam-clearing drill under fire. Tap, rack, bang — the reflex is faster than the flinch.","discipline_id":"combat","display_name":"Tap-Rack-Bang","id":"skill_tap_rack_bang","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 12: `{"description":"First-shot accuracy from concealed cover. The first one is the only one that matters, and the only one they practice.","discipline_id":"combat","display_name":"Cold Bore","id":"skill_cold_bore","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 13: `{"description":"Pins hostiles behind cover while the squad moves. Not trying to hit; trying to make them stay down.","discipline_id":"combat","display_name":"Suppressing Fire","id":"skill_suppressing_fire","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 14: `{"description":"Lethal hand-to-hand and point-blank work in corridors, where the walls are part of the fight.","discipline_id":"combat","display_name":"Close Quarters","id":"skill_close_quarters","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 15: `{"description":"Rigs deadfalls and tripwires along shelter approaches. Knows exactly where a stranger's boot will land.","discipline_id":"combat","display_name":"Trap Setter","id":"skill_trap_setter","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 16: `{"description":"Spots the high-value cache points in collapsing structures — the places people hide things and never live to collect.","discipline_id":"scavenging","display_name":"Looter's Reflex","id":"skill_looters_reflex","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 17: `{"description":"Morale holds through violent death and carnage. Not cruelty; the shock burned off somewhere else, a long time ago.","discipline_id":"combat","display_name":"Desensitized","id":"skill_desensitized","is_expert_skill":false,"skill_bonus":0.0,"xp_threshold":999999.0}`
  - row 18: `{"description":"Extracts maximum nutrition from spoiled tins and root starch. Nothing gets thrown out until this one has argued with it.","discipline_id":"survival","display_name":"Ration Stretcher","id":"skill_ration_stretcher","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 19: `{"description":"Resists foodborne illness and irradiated-water nausea. Eats first, sets the example, usually survives.","discipline_id":"survival","display_name":"Iron Stomach","id":"skill_iron_stomach","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 20: `{"description":"Ferments mash and distills drinkable alcohol from sugar beet scrap. Fuel, medicine, and courage, in that order.","discipline_id":"survival","display_name":"Wasteland Brewer","id":"skill_wasteland_brewer","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 21: `{"description":"Clean field dressing of irradiated fauna; the bonus yield is knowing which parts are safe to keep.","discipline_id":"survival","display_name":"The Butcher","id":"skill_butcher","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 22: `{"description":"Compounds clean medicinal compounds from chemical precursors. Measures twice because the second measure might not exist.","discipline_id":"medical","display_name":"Pharmacologist","id":"skill_pharmacologist","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 23: `{"description":"Identifies edible, medicinal, and bioluminescent bunker fungi. Knows which ones glow, which ones kill, and which ones do both.","discipline_id":"science","display_name":"Mycology","id":"skill_mycology","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 24: `{"description":"Improvises replacement gears from copper pipe and solder. The machine never asks where the part came from.","discipline_id":"crafting","display_name":"Jury-Rigger","id":"skill_jury_rigger","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 25: `{"description":"Reinforces tunnel ceilings and vault bulkheads against seismic load. Reads cracks the way other people read sentences.","discipline_id":"crafting","display_name":"Structural Engineer","id":"skill_structural_engineer","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 26: `{"description":"Balances duct pressure and cleans blower impellers. The shelter breathes evenly and nobody knows whose doing it is.","discipline_id":"crafting","display_name":"HVAC Technician","id":"skill_hvac_tech","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 27: `{"description":"Harvests pristine copper wiring and steel rebar from ruins. Knows which walls are load-bearing and which are just tired.","discipline_id":"crafting","display_name":"Scrapper","id":"skill_scrapper","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 28: `{"description":"Excavates bunker expansion tunnels safely. Shores as they go; the dark is a coworker, not an enemy.","discipline_id":"crafting","display_name":"The Sandhog","id":"skill_sandhog","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 29: `{"description":"Optimizes heat exchangers and steam recycling efficiency. Heat is a currency and this one runs the mint.","discipline_id":"science","display_name":"Thermodynamics","id":"skill_thermodynamics","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 30: `{"description":"Treats severe puncture wounds in dark, shaking shelters. Steady because the patient can feel the difference.","discipline_id":"medical","display_name":"Steady Hands (Field)","id":"skill_steady_hands_field","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 31: `{"description":"Decisive emergency prioritization during hostile incursions. Chooses fast, chooses right, and carries the choosing afterward.","discipline_id":"medical","display_name":"Triage Under Fire","id":"skill_triage_under_fire","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 32: `{"description":"Precision tracking of tissue rem accumulation and chelation timing. Reads a dose ledger like a biography.","discipline_id":"medical","display_name":"Radiologist","id":"skill_radiologist","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 33: `{"description":"Pathological knowledge that reveals organ vulnerabilities and autopsy findings. The body keeps no secrets from this one, only paperwork.","discipline_id":"medical","display_name":"Anatomist","id":"skill_anatomist","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 34: `{"description":"Rapid trauma stabilization that beats exsanguination by minutes. The minutes are the whole discipline.","discipline_id":"medical","display_name":"Paramedic","id":"skill_paramedic","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 35: `{"description":"Optimal rucksack strapping for expedition carry capacity. The load never shifts, so the legs never fail.","discipline_id":"scavenging","display_name":"Pack Mule","id":"skill_pack_mule","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 36: `{"description":"Silent movement over glass, rubble, and creaking floorboards. The floorboards give no testimony.","discipline_id":"scavenging","display_name":"Light Step","id":"skill_light_step","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 37: `{"description":"Recognizes structural collapse patterns and shortcut alleys. The city is a map, and this one reads the tears in it.","discipline_id":"scavenging","display_name":"Urban Pathfinder","id":"skill_urban_pathfinder","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 38: `{"description":"Combat superiority in absolute darkness and ash blizzards. Others fear the dark; the dark knows this one by name.","discipline_id":"combat","display_name":"Night Terror","id":"skill_night_terror","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 39: `{"description":"Finds edible lichen and unpoisoned roots in dead scrubland. Green enough to eat is a color most people miss.","discipline_id":"survival","display_name":"Forager","id":"skill_forager","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 40: `{"description":"Diffuses shelter panic and resolves violent disputes with words. Lowers the volume of a room before lowering the stakes.","discipline_id":"survival","display_name":"De-Escalator","id":"skill_de_escalator","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 41: `{"description":"Impeccable inventory audits that prevent waste and theft. The ledger is a weapon, and this one keeps it sharp.","discipline_id":"scavenging","display_name":"Quartermaster","id":"skill_quartermaster","is_expert_skill":false,"skill_bonus":0.05,"xp_threshold":999999.0}`
  - row 42: `{"description":"Drives work shifts with rigorous efficiency and stamina. Hated at six, thanked at midnight, right both times.","discipline_id":"survival","display_name":"Taskmaster","id":"skill_taskmaster","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 43: `{"description":"Life-saving surgical intervention against impossible odds. The odds are stated honestly, then ignored.","discipline_id":"medical","display_name":"Miracle Worker","id":"skill_miracle_worker","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 44: `{"description":"Refines chemical reagents and anti-rad chelation cocktails from raw minerals. Turns rocks into second chances.","discipline_id":"science","display_name":"Alchemist","id":"skill_alchemist","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 45: `{"description":"Isolates animal-borne pathogens before cross-species outbreak. Watches the animals so the people don't have to learn from them.","discipline_id":"medical","display_name":"Zoonotic Expert","id":"skill_zoonotic_expert","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 46: `{"description":"Unwavering emotional stability that sets the shelter's morale baseline. The calm is not the absence of fear; it is furniture.","discipline_id":"survival","display_name":"Anchor","id":"skill_anchor","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 47: `{"description":"Complete psychological numbing to catastrophic casualty events. Still does the paperwork for every one. Especially every one.","discipline_id":"survival","display_name":"Death Blind","id":"skill_death_blind","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 48: `{"description":"Tactical coordination that boosts squad firepower. One voice turns five guns into a single decision.","discipline_id":"combat","display_name":"Warlord","id":"skill_warlord","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 49: `{"description":"Commands the respect that prevents mutiny and faction infighting. Walks into rooms that were about to become corridors.","discipline_id":"survival","display_name":"Peacekeeper","id":"skill_peacekeeper","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 50: `{"description":"Exceptional physical constitution; blunt trauma becomes an inconvenience. The doorframe disagrees with this one, and the doorframe loses.","discipline_id":"combat","display_name":"Juggernaut","id":"skill_juggernaut","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 51: `{"description":"Mastery of wilderness ambush and silent stalker tactics. The wasteland's food chain has an edit in it.","discipline_id":"combat","display_name":"Apex Predator","id":"skill_apex_predator","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 52: `{"description":"Deep self-reliance that lowers daily food and water consumption. Needs less, and needs it later.","discipline_id":"survival","display_name":"Survivalist","id":"skill_survivalist","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 53: `{"description":"Engineers high-pressure plumbing, valves, and desalination rigs. Water obeys, grudgingly, and on schedule.","discipline_id":"crafting","display_name":"Hydraulic Master","id":"skill_hydraulic_master","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 54: `{"description":"Instinctive understanding of high-voltage conduits and substations. Knows which wires still remember being alive.","discipline_id":"crafting","display_name":"Grid Walker","id":"skill_grid_walker","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 55: `{"description":"Reinforced blast construction and seismic shock isolation. Builds doors meant to outlive the argument about opening them.","discipline_id":"crafting","display_name":"Vault Builder","id":"skill_vault_builder","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 56: `{"description":"Maintains expedition engines, chassis, and generators. Talks to them, and — this is the unsettling part — they answer.","discipline_id":"crafting","display_name":"Grease Monkey","id":"skill_grease_monkey","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 57: `{"description":"Compounds polymer plastics and synthetic insulation. The new world's materials, built from the old world's mistakes.","discipline_id":"science","display_name":"Synthesizer","id":"skill_synthesizer","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 58: `{"description":"Botanical intuition that maximizes greenhouse yield. The plants lean toward this one, and nobody can explain it.","discipline_id":"survival","display_name":"Gaia","id":"skill_gaia","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 59: `{"description":"Sprint stamina that cuts travel time across open ash plains. Distance is a suggestion; the horizon is a habit.","discipline_id":"scavenging","display_name":"Wasteland Runner","id":"skill_wasteland_runner","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 60: `{"description":"Complete concealment from optics, infrared, and auditory patrols. The guard report describes a draft.","discipline_id":"scavenging","display_name":"Ghost","id":"skill_ghost","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 61: `{"description":"Predicts ash blizzards and harrow fronts hours before they arrive. Watches the sky the way other people watch doors.","discipline_id":"science","display_name":"Stormcaller","id":"skill_stormcaller","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 62: `{"description":"Remarkable biological resistance to chronic background gamma. The dosimeter keeps a record this one argues with.","discipline_id":"survival","display_name":"Rad Walker","id":"skill_rad_walker","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 63: `{"description":"Accelerated multi-disciplinary learning across technical fields. One question away from someone else's specialty.","discipline_id":"science","display_name":"Polymath","id":"skill_polymath","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 64: `{"description":"Charismatic rhetoric capable of swaying hostile crowds. Believes the speech, which is why the speech works.","discipline_id":"survival","display_name":"Demagogue","id":"skill_demagogue","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 65: `{"description":"Mentors novices and accelerates apprentice progression. Measures success in other people's competence.","discipline_id":"survival","display_name":"Shepherd","id":"skill_shepherd","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 66: `{"description":"Investigates conspiracy records and uncovers buried bunker archives. Asks the second question, which is where the truth lives.","discipline_id":"science","display_name":"Muckraker","id":"skill_muckraker","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 67: `{"description":"Radio resonance that calms listeners across the dial. People who have never met this one sleep better anyway.","discipline_id":"science","display_name":"Voice of the Wastes","id":"skill_voice_of_the_wastes","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 68: `{"description":"Culinary mastery that turns wasteland rations into morale feasts. The same tin twice: once as food, once as occasion.","discipline_id":"survival","display_name":"Iron Chef","id":"skill_iron_chef","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 69: `{"description":"Endures prolonged double shifts without fatigue collapse. The bunker's clocks are wrong about this one, consistently.","discipline_id":"survival","display_name":"Tireless","id":"skill_tireless","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 70: `{"description":"Immunity to toxic particulate and chemical fume inhalation. Lungs that made terms with the smoke.","discipline_id":"survival","display_name":"Asbestos","id":"skill_asbestos","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 71: `{"description":"Forges reinforced ballistic plates and services military weaponry. The armor is the argument, and the argument is steel.","discipline_id":"crafting","display_name":"Armorer","id":"skill_armorer","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 72: `{"description":"Modifies electronics and optical sensors for maximum output. Everything leaves better than it arrived, and slightly dangerous.","discipline_id":"crafting","display_name":"Tinkerer","id":"skill_tinkerer","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 73: `{"description":"Preserves pre-war literature, logs, and cultural records. Keeps the before alive because the after will ask.","discipline_id":"science","display_name":"Lorekeeper","id":"skill_lorekeeper","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 74: `{"description":"Combat bonus against fanatical cultists and violent raiders. Faith is fine; this one has studied where it stands.","discipline_id":"combat","display_name":"Zealot's Bane","id":"skill_zealots_bane","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 75: `{"description":"Metabolic tolerance that prevents dependency on heavy combat stimulants. The body refuses to borrow strength it cannot repay.","discipline_id":"medical","display_name":"Chem Resistant","id":"skill_chem_resistant","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 76: `{"description":"Shields companions from direct attack during tactical withdrawals. Steps between — a trained motion and a chosen one.","discipline_id":"combat","display_name":"Protector","id":"skill_protector","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 77: `{"description":"Guides shelter decisions with matriarchal wisdom and authority. The room settles when she sits down, before she speaks.","discipline_id":"survival","display_name":"Matriarch","id":"skill_matriarch","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 78: `{"description":"Absolute resolve that upholds bunker cohesion through crisis. Carries the weight; does not discuss the weight.","discipline_id":"survival","display_name":"Pillar of Atlas","id":"skill_pillar_of_atlas","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 79: `{"description":"Advanced cartography and threat detection across uncharted sectors. Maps the places, and the reasons to avoid them.","discipline_id":"scavenging","display_name":"Wasteland Scout","id":"skill_wasteland_scout","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 80: `{"description":"Born into the fallout; instinctive adaptation to surface hazards. Never saw the sky before, and reads it perfectly.","discipline_id":"survival","display_name":"Child of the Ash","id":"skill_child_of_the_ash","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 81: `{"description":"Optimizes strategic resource allocation with emotionless efficiency. The arithmetic is right, and the arithmetic is watched.","discipline_id":"science","display_name":"Cold Calculus","id":"skill_cold_calculus","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 82: `{"description":"Grim surgical efficiency honed during the worst starvation crisis. Learned what a body is for when food stopped being one.","discipline_id":"survival","display_name":"Butcher of Day 30","id":"skill_butcher_of_day_30","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 83: `{"description":"Leverages trade and diplomatic secrets to extract favorable terms. Collects truths the way others collect tins.","discipline_id":"survival","display_name":"Master Manipulator","id":"skill_master_manipulator","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 84: `{"description":"Safekeeps rare materials and prevents resource decay. Nothing leaves the hoard without a reason, and reasons are rationed.","discipline_id":"scavenging","display_name":"Dragon's Hoard","id":"skill_dragons_hoard","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 85: `{"description":"Deep doctrine mastery that reduces friendly combat losses. Wins the fights that do not need fighting, first.","discipline_id":"combat","display_name":"Art of War","id":"skill_art_of_war","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 86: `{"description":"Precision breaching and explosive ordnance disposal. Argues with bombs politely, in their own language.","discipline_id":"combat","display_name":"Demolitions Expert","id":"skill_demolitions_expert","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 87: `{"description":"Concealed long-range sniping without revealing position. The shot arrives before the direction does.","discipline_id":"combat","display_name":"Ghost Shooter","id":"skill_ghost_shooter","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 88: `{"description":"Optimizes trade convoy routes and warehouse turnover. Sees the whole road at once: every axle, every bribe, every delay.","discipline_id":"scavenging","display_name":"Supply Chain Master","id":"skill_supply_chain_master","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 89: `{"description":"Unnatural vitality and swift injury recovery. The body decided the wasteland was not finished with its argument.","discipline_id":"survival","display_name":"Reclaimed Youth","id":"skill_reclaimed_youth","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 90: `{"description":"Psychological counseling that restores fractured minds. Listens the way a surgeon operates: patiently, and with clean hands.","discipline_id":"medical","display_name":"Soul Weaver","id":"skill_soul_weaver","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 91: `{"description":"Enhanced performance when operating solo on expeditions. Company is a luxury; solitude is a discipline.","discipline_id":"survival","display_name":"Lone Wolf","id":"skill_lone_wolf","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 92: `{"description":"Spreads pragmatic hope that shields allies from despair. Not sunshine — a handhold. Points at it without promising.","discipline_id":"survival","display_name":"Grounded Optimist","id":"skill_grounded_optimist","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 93: `{"description":"Inspires the devotion that unites disparate survivor factions. People behave better in rooms this one walks into, and afterward.","discipline_id":"survival","display_name":"Living Saint","id":"skill_living_saint","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 94: `{"description":"Dedication to patient care with zero ego or reluctance. Once kept score of a wing with their name on it; now keeps charts.","discipline_id":"medical","display_name":"Humbled Healer","id":"skill_humbled_healer","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 95: `{"description":"Total immunity to chemical cravings and withdrawal penalties. Counts the days in both directions and starts from the same one, now.","discipline_id":"survival","display_name":"Clean and Sober","id":"skill_clean_and_sober","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 96: `{"description":"Unblinking vigilance that prevents nocturnal breaches. Knows every hour of every night personally.","discipline_id":"combat","display_name":"The Watcher","id":"skill_the_watcher","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 97: `{"description":"Sensory acuity that detects hidden traps and micro-contaminants. Scared of exactly the right things, on schedule.","discipline_id":"scavenging","display_name":"Hyper Aware","id":"skill_hyper_aware","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 98: `{"description":"Mastery of incendiary weapons and flame traps. Watches fire the way others watch water — for what it does next.","discipline_id":"combat","display_name":"Fire Breather","id":"skill_fire_breather","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 99: `{"description":"Acoustic echolocation in dark tunnels and flooded shafts. Sees with the ears; the dark is just a medium.","discipline_id":"science","display_name":"Sonar","id":"skill_sonar","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 100: `{"description":"Assembles complex machines from mismatched salvage. The machine does not know its parts have never met.","discipline_id":"crafting","display_name":"Improvised Engineering","id":"skill_improvised_engineering","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 101: `{"description":"Bizarre biological symbiosis with low-level background radiation. Where others get sick, this one gets — better. Nobody says it out loud.","discipline_id":"survival","display_name":"Radiotrophic","id":"skill_radiotrophic","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 102: `{"description":"Locates intact pre-war relics in extreme hotspots. Goes where the counters scream and comes back with quiet things.","discipline_id":"scavenging","display_name":"Apex Scavenger","id":"skill_apex_scavenger","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 103: `{"description":"Absolute calm during traumatic surgery and high-stakes crafting. The pulse does not rise; the work does not shake.","discipline_id":"survival","display_name":"Zen State","id":"skill_zen_state","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 104: `{"description":"Selective breeding of radiation-hardened crop strains. Plays the long game in seasons and wins it in grams.","discipline_id":"science","display_name":"Master Geneticist","id":"skill_master_geneticist","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 105: `{"description":"Iron-fisted guard duty that prevents internal unrest. Stands where nobody wants to be tested, and nobody tests.","discipline_id":"combat","display_name":"The Enforcer","id":"skill_the_enforcer","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 106: `{"description":"Renown that unlocks instant respect and trade concessions. The name arrives at settlements before the person does.","discipline_id":"survival","display_name":"Legend of the Wastes","id":"skill_legend_of_the_wastes","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 107: `{"description":"Drafts faction treaties and neutral accords. Writes in the grammar of shared interest, the only grammar everyone reads.","discipline_id":"survival","display_name":"The Statesman","id":"skill_the_statesman","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 108: `{"description":"Interfaces neural cyber-implants and prosthetic limbs. Speaks fluent machine to the parts that used to be a person.","discipline_id":"science","display_name":"Cybernetics","id":"skill_cybernetics","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 109: `{"description":"Uncovers propaganda and restores verified historical truth. Checks the story against the ledger; the ledger always wins.","discipline_id":"science","display_name":"Beacon of Truth","id":"skill_beacon_of_truth","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 110: `{"description":"Forensic autopsy insight identifying microscopic viral vectors. The dead testify; this one takes the statement correctly.","discipline_id":"medical","display_name":"Master Pathologist","id":"skill_master_pathologist","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 111: `{"description":"Controls critical commodity markets for maximum economic leverage. Does not corner the market — corners the thing the market needs.","discipline_id":"survival","display_name":"Monopolist","id":"skill_monopolist","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 112: `{"description":"Navigates subterranean ruins and flooded mine complexes. Down is a direction with its own weather, and this one dresses for it.","discipline_id":"scavenging","display_name":"Deep Delver","id":"skill_deep_delver","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 113: `{"description":"Eliminates freight waste and streamlines supply chains. Every crate has a story; this one edits out the boring parts.","discipline_id":"scavenging","display_name":"Logistics Master","id":"skill_logistics_master","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 114: `{"description":"Smelts Damascus alloys and precision-casts machine parts. Folds steel like a letter nobody else can open.","discipline_id":"crafting","display_name":"Forge Master","id":"skill_forge_master","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 115: `{"description":"Decontaminates biological and chemical hotspots completely. Cleans the room, the air, and the record. In that order.","discipline_id":"medical","display_name":"Sanitization Expert","id":"skill_sanitization_expert","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 116: `{"description":"Clears radioactive timber and harvests dry lumber. Knows which trees are fuel, which are shelter, and which are warnings.","discipline_id":"survival","display_name":"Deforester","id":"skill_deforester","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 117: `{"description":"Maps epidemic transmission vectors across wasteland trade nodes. Draws the sickness's route before the sickness does.","discipline_id":"medical","display_name":"Epidemiologist","id":"skill_epidemiologist","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 118: `{"description":"Navigates by starlight and sextant when GPS and compasses fail. The old sky still works; it never signed anything.","discipline_id":"scavenging","display_name":"Celestial Navigator","id":"skill_celestial_navigator","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 119: `{"description":"Catalogs and preserves fragile parchment and magnetic tapes. Cotton gloves in a world that has forgotten what soft means.","discipline_id":"science","display_name":"Archivist","id":"skill_archivist","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 120: `{"description":"Detects counterfeit currency and fraudulent barter ledgers. Reads a forgery the way a medic reads a cough.","discipline_id":"survival","display_name":"Auditor","id":"skill_auditor","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 121: `{"description":"Plays acoustic instruments that lift bunker spirits during blackouts. The blackout is the audience; the audience is everybody.","discipline_id":"survival","display_name":"Maestro","id":"skill_maestro","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 122: `{"description":"Smuggles critical medicine through enemy siege cordons. The cargo is light and the reason is heavy.","discipline_id":"scavenging","display_name":"Blockade Runner","id":"skill_blockade_runner","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 123: `{"description":"Ruthless close-range finishing strikes against armored foes. The armor has a gap; the patience finds it.","discipline_id":"combat","display_name":"Executioner","id":"skill_executioner","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 124: `{"description":"Blends into darkness to evade searchlights and thermal sensors. The light bends its reports around this one.","discipline_id":"scavenging","display_name":"Shadow","id":"skill_shadow","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 125: `{"description":"Infiltrates rival settlements using forged insignia and uniforms. Wears the enemy's paperwork better than the enemy does.","discipline_id":"survival","display_name":"Master of Disguise","id":"skill_master_of_disguise","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 126: `{"description":"Restores ruined engine blocks in half the normal time. Listens once; the engine tells everything.","discipline_id":"crafting","display_name":"Mechanic Prodigy","id":"skill_mechanic_prodigy","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 127: `{"description":"Secures non-aggression pacts with suspicious wasteland tribes. Sits down first, unarmed, and it keeps working.","discipline_id":"survival","display_name":"Diplomat","id":"skill_diplomat","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 128: `{"description":"Mastery of improvised melee weapons in brutal pit fights. Everything is a weapon; the trick is knowing which end.","discipline_id":"combat","display_name":"Wasteland Gladiator","id":"skill_wasteland_gladiator","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 129: `{"description":"Organizes entire hospital wards and triage protocols. Runs the clinic like a watch, because it is one.","discipline_id":"medical","display_name":"Chief of Medicine","id":"skill_chief_of_medicine","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 130: `{"description":"Pilots aerial micro-drones for over-the-horizon reconnaissance. Sees the wasteland from the only angle it cannot hide from.","discipline_id":"science","display_name":"Drone Operator","id":"skill_drone_operator","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 131: `{"description":"Radio singing that calms bunker distress panics. One voice, one frequency, and the panic sits down to listen.","discipline_id":"science","display_name":"Choir of One","id":"skill_choir_of_one","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 132: `{"description":"Interlocking defensive maneuvers among tight-knit squads. Five people moving like one animal with five opinions.","discipline_id":"combat","display_name":"Hive Tactics","id":"skill_hive_tactics","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 133: `{"description":"Communal first-aid and wound dressing across shared dormitories. Everyone's hands trained; everyone's hands busy.","discipline_id":"medical","display_name":"Hive Healing","id":"skill_hive_healing","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 134: `{"description":"Decodes classified wartime telegrams and bunker transcripts. The war kept a diary; this one reads it aloud.","discipline_id":"science","display_name":"Truth Seeker","id":"skill_truth_seeker","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 135: `{"description":"Thrives in untamed irradiated wilderness without manufactured tools. The wasteland stopped trying to kill this one out of respect.","discipline_id":"survival","display_name":"Wildman","id":"skill_wildman","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 136: `{"description":"Survives catastrophic lethal injury once per campaign. Died once, on schedule; the paperwork is still pending.","discipline_id":"survival","display_name":"Second Life","id":"skill_second_life","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 137: `{"description":"Immunity to psychic shock, trauma panic, and despair flags. The mind is a door that locks from the inside and stays locked.","discipline_id":"survival","display_name":"Iron Will","id":"skill_iron_will","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 138: `{"description":"Eavesdrops on raider radio comms and unencrypted walkie frequencies. The raiders never encrypted; they still do not know.","discipline_id":"science","display_name":"Unseen Listener","id":"skill_unseen_listener","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 139: `{"description":"Drives brutal barter bargains that maximize shelter profit. Fair is a direction, not a destination.","discipline_id":"survival","display_name":"Ruthless Capitalist","id":"skill_ruthless_capitalist","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 140: `{"description":"Unmatched raw intellect mastering new disciplines at triple speed. Boredom is the only hazard this one reports.","discipline_id":"science","display_name":"Prodigy","id":"skill_prodigy","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 141: `{"description":"Inspiring battle cries that boost squad accuracy and courage. The voice arrives before the order and does half its work.","discipline_id":"combat","display_name":"Commander","id":"skill_commander","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 142: `{"description":"Hydraulic bionic limb with crushing grip and recoil compensation. The arm keeps the receipts; the shoulder keeps the score.","discipline_id":"combat","display_name":"Cyber Arm","id":"skill_cyber_arm","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 143: `{"description":"Atones for past sins through self-sacrifice and labor. The debt is internal, and the payments are visible.","discipline_id":"survival","display_name":"Redemption","id":"skill_redemption","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 144: `{"description":"Pushes machinery beyond safety margins for emergency output bursts. The machine survives; the machine is also offended.","discipline_id":"crafting","display_name":"Overclocked","id":"skill_overclocked","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 145: `{"description":"Defends defenseless refugees with relentless protective fury. Gentle at the table; weather in the doorway.","discipline_id":"combat","display_name":"Wasteland Guardian","id":"skill_wasteland_guardian","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 146: `{"description":"Total mastery of wasteland knowledge, maps, and survivor behavior. Knows who owes what, who lies, and where the water hides.","discipline_id":"science","display_name":"Omniscience","id":"skill_omniscience","is_expert_skill":true,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 147: `{"description":"Advanced invasive trauma procedures and sterile resection under austerity. Sterility is a discipline, not a supply.","discipline_id":"medical","display_name":"Field Surgery","id":"skill_field_surgery","is_expert_skill":false,"skill_bonus":0.15,"xp_threshold":999999.0}`
  - row 148: `{"description":"Operates ceramic, sand, and charcoal filter stages for maximum potable yield. Stands between the water and the mouth.","discipline_id":"survival","display_name":"Water Filtration","id":"skill_water_filtration","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 149: `{"description":"Vacuum-tube alignment, antenna tuning, and transmitter circuit patching. The voice in the dark depends on this solder.","discipline_id":"science","display_name":"Radio Repair","id":"skill_radio_repair","is_expert_skill":false,"skill_bonus":0.1,"xp_threshold":999999.0}`
  - row 150: `{"description":"Foundational reading and written communication enabling library study and technical transcription.","discipline_id":"science","display_name":"Basic Literacy & Writing","id":"skill_reading_comprehension","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 151: `{"description":"Arithmetic, measurement calculations, and basic problem solving.","discipline_id":"science","display_name":"Basic Numeracy & Logic","id":"skill_mathematical_logic","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 152: `{"description":"Conflict resolution, empathy, and collective shelter duty adherence.","discipline_id":"survival","display_name":"Social Cooperation & Ethics","id":"skill_communal_diplomacy","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 153: `{"description":"Hazard recognition, airlock decontamination drills, and water rationing habits.","discipline_id":"survival","display_name":"Bunker & Wasteland Survival","id":"skill_radiation_awareness","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 154: `{"description":"Workshop tool handling, weld repair, and scrap reclamation.","discipline_id":"crafting","display_name":"Mechanical Crafting & Repair","id":"skill_machining_basics","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 155: `{"description":"Bandaging, antiseptics, trauma stabilization, and symptom identification.","discipline_id":"medical","display_name":"First Aid & Human Physiology","id":"skill_field_triage","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 156: `{"description":"Map reading, route plotting through ruins, and terrain hazard evasion.","discipline_id":"scavenging","display_name":"Scouting & Surface Navigation","id":"skill_cartography","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 157: `{"description":"Firearm discipline, defensive positioning, and emergency lockdown breach containment.","discipline_id":"combat","display_name":"Shelter Defense & Weapon Safety","id":"skill_firearm_handling","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 158: `{"description":"Generator maintenance, air filtration overhaul, and electrical bus troubleshooting.","discipline_id":"crafting","display_name":"Power & Life Support Engineering","id":"skill_reactor_maintenance","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 159: `{"description":"Medication synthesis, sterile surgical procedures, and radiation sickness counter-therapy.","discipline_id":"medical","display_name":"Pharmacology & Advanced Care","id":"skill_surgery_assistance","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - row 160: `{"description":"Squad maneuvering, risk calculation under fire, and retreat protocols.","discipline_id":"combat","display_name":"Expedition Leadership & Tactics","id":"skill_patrol_command","is_expert_skill":false,"skill_bonus":0.2,"xp_threshold":999999.0}`
  - ... 1 additional rows omitted from the compact audit; the complete current file is identified above ...

## `Assets/StreamingAssets/Data/research_knowledge.json`
- Bytes: 21,516; SHA-256: `6eef977ff53a20c22577be27cddf6564426d7d484cece6b17634c136f29d31c7`
- Root keys: `collection_id, knowledge_nodes, schema_version`
- `knowledge_nodes`: list[62]; union fields: `breakthrough_item, category, days_to_complete, description, display_name, id, prerequisites`
  - row 1: `{"category":"survival","days_to_complete":5,"description":"Boiling, charcoal filtration, and still-building from salvage.","display_name":"Water Purification Basics","id":"knowledge_water_basics","prerequisites":[]}`
  - row 2: `{"breakthrough_item":"item_water_filter_advanced","category":"survival","days_to_complete":12,"description":"Multi-stage ceramic filters reduce fallout particulate by 90%.","display_name":"Advanced Water Filtration","id":"knowledge_water_advanced","prerequisites":["knowledge_water_basics"]}`
  - row 3: `{"category":"medical","days_to_complete":5,"description":"Iodine prophylaxis, chelation agents, and dose-ledger tracking.","display_name":"Radiation Medicine Basics","id":"knowledge_radiation_basics","prerequisites":[]}`
  - row 4: `{"breakthrough_item":"item_radiation_shielding_panel","category":"engineering","days_to_complete":15,"description":"Layered lead-cloth, borated polyethylene, and sky-layer armour panels.","display_name":"Radiation Shielding Materials","id":"knowledge_radiation_shielding","prerequisites":["knowledge_radiation_basics"]}`
  - row 5: `{"breakthrough_item":"item_gas_mask_improved","category":"engineering","days_to_complete":10,"description":"Charcoal-canister rebuild doubles filter lifespan under heavy fallout.","display_name":"Improved Gas Masks","id":"knowledge_gas_mask_improved","prerequisites":["knowledge_radiation_basics"]}`
  - row 6: `{"category":"survival","days_to_complete":8,"description":"Nutrient-film technique in recycled bunker trays yields greens in 14 days.","display_name":"Hydroponic Cultivation","id":"knowledge_hydroponics","prerequisites":[]}`
  - row 7: `{"category":"engineering","days_to_complete":7,"description":"Junction-box rebuild and panel-angle tracking from scrap photovoltaic cells.","display_name":"Solar Power Basics","id":"knowledge_solar_basics","prerequisites":[]}`
  - row 8: `{"breakthrough_item":"item_solar_inverter","category":"engineering","days_to_complete":14,"description":"Battery-bank topology and inverter rebuild for overnight draw.","display_name":"Solar Power Systems","id":"knowledge_solar_advanced","prerequisites":["knowledge_solar_basics"]}`
  - row 9: `{"category":"survival","days_to_complete":10,"description":"Salt-curing, cold-smoking, and vacuum-seal scavenge from ruined canneries.","display_name":"Food Preservation","id":"knowledge_food_preservation","prerequisites":[]}`
  - row 10: `{"category":"science","days_to_complete":6,"description":"Direction-finding, squelch calibration, and Morse decoding from static.","display_name":"Radio Signal Processing","id":"knowledge_radio_basics","prerequisites":[]}`
  - row 11: `{"breakthrough_item":"item_radio_cipher_rotor","category":"science","days_to_complete":12,"description":"One-time pad key exchange and frequency-hopping from salvaged cipher rotors.","display_name":"Encrypted Radio Communication","id":"knowledge_radio_advanced","prerequisites":["knowledge_radio_basics"]}`
  - row 12: `{"category":"engineering","days_to_complete":8,"description":"Spray-foam salvage and thermal-barrier panels cut bunker heat loss by 40%.","display_name":"Shelter Insulation","id":"knowledge_shelter_insulation","prerequisites":[]}`
  - row 13: `{"breakthrough_item":"item_air_filter_hepa","category":"engineering","days_to_complete":10,"description":"HEPA-grade filter rebuild extends bunker air-filtration lifespan by 50%.","display_name":"Air Filtration Systems","id":"knowledge_air_filtration","prerequisites":["knowledge_shelter_insulation"]}`
  - row 14: `{"category":"scavenging","days_to_complete":7,"description":"Route-mapping and weight-distribution analysis cuts expedition fatigue by 15%.","display_name":"Scavenge Efficiency","id":"knowledge_scavenge_efficiency","prerequisites":[]}`
  - row 15: `{"category":"combat","days_to_complete":8,"description":"Close-quarters drills and cover-fire protocols improve survivor combat readiness.","display_name":"Combat Training Doctrine","id":"knowledge_combat_training","prerequisites":[]}`
  - row 16: `{"breakthrough_item":"item_hydraulic_actuator","category":"survival","days_to_complete":14,"description":"High-pressure submersible pump rebuilding to tap sub-bedrock aquifers.","display_name":"Deep-Well Hydraulics","id":"knowledge_deep_well_hydraulics","prerequisites":["knowledge_water_advanced"]}`
  - row 17: `{"category":"survival","days_to_complete":12,"description":"Thermal-siphon soil heating and spectral LED augmentation for winter crops.","display_name":"Greenhouse Microclimate Engineering","id":"knowledge_greenhouse_microclimate","prerequisites":["knowledge_hydroponics"]}`
  - row 18: `{"breakthrough_item":"item_vacuum_seal_canner","category":"survival","days_to_complete":11,"description":"Pressure-canning techniques for long-term storage without grid power.","display_name":"Vacuum-Seal Cold Canning","id":"knowledge_cold_canning_preservation","prerequisites":["knowledge_food_preservation"]}`
  - row 19: `{"category":"survival","days_to_complete":9,"description":"Enclosed beehive management and indoor pollination for seed crops.","display_name":"Bunker Apiculture & Pollination","id":"knowledge_apiculture_ecology","prerequisites":["knowledge_hydroponics"]}`
  - row 20: `{"breakthrough_item":"item_surgical_kit","category":"medical","days_to_complete":16,"description":"Emergency thoracotomy and vascular shunts under blackout conditions.","display_name":"Field Trauma Surgery","id":"knowledge_field_trauma_surgery","prerequisites":["knowledge_radiation_basics"]}`
  - row 21: `{"category":"medical","days_to_complete":12,"description":"Negative-pressure anterooms and chlorine mist protocols against epidemics.","display_name":"Pathogen Isolation & Quarantine","id":"knowledge_pathogen_containment","prerequisites":["knowledge_radiation_basics"]}`
  - row 22: `{"breakthrough_item":"item_reagent_clean","category":"medical","days_to_complete":14,"description":"Organic solvent extraction of antibiotics and analgesics from mould and bark.","display_name":"Reagent Pharmacology Synthesis","id":"knowledge_pharmacology_synthesis","prerequisites":["knowledge_pathogen_containment"]}`
  - row 23: `{"category":"engineering","days_to_complete":15,"description":"Refractory clay lining and flux chemistry for scrap tool-steel casting.","display_name":"Crucible Metallurgy Practice","id":"knowledge_high_temp_metallurgy","prerequisites":["knowledge_shelter_insulation"]}`
  - row 24: `{"category":"engineering","days_to_complete":18,"description":"Deep-loop glycol heat pumps extracting subterranean fissure heat.","display_name":"Geothermal Heat Exchanger","id":"knowledge_geothermal_tap","prerequisites":["knowledge_shelter_insulation","knowledge_solar_advanced"]}`
  - row 25: `{"breakthrough_item":"item_diving_suit_vulcanized","category":"engineering","days_to_complete":16,"description":"Vulcanized rubber diving suits and weighted descent harnesses for submerged hulls.","display_name":"Submersible Salvage Rig","id":"knowledge_submersible_salvage_rig","prerequisites":["knowledge_gas_mask_improved"]}`
  - row 26: `{"breakthrough_item":"item_cloud_seeding_canister","category":"science","days_to_complete":16,"description":"Pyrotechnic flare dispersion of condensing nuclei to trigger cleansing rainfall.","display_name":"Silver-Iodide Cloud Seeding","id":"knowledge_atmospheric_cloud_seeding","prerequisites":["knowledge_radio_basics","knowledge_radiation_shielding"]}`
  - row 27: `{"category":"science","days_to_complete":13,"description":"Over-the-horizon radio skip forecasting using geomagnetic sensor loops.","display_name":"Ionospheric Waveguide Analysis","id":"knowledge_ionospheric_propagation","prerequisites":["knowledge_radio_advanced"]}`
  - row 28: `{"breakthrough_item":"item_seismic_detector","category":"science","days_to_complete":11,"description":"Triangulated geophone networks predicting aftershocks and subterranean collapses.","display_name":"Seismic Fault Array Mapping","id":"knowledge_seismic_fault_mapping","prerequisites":["knowledge_radio_basics"]}`
  - row 29: `{"category":"scavenging","days_to_complete":10,"description":"Pre-stressed concrete stress analysis to safely breach collapsed skyscraper vaults.","display_name":"Urban Structural Cartography","id":"knowledge_ruin_structural_survey","prerequisites":["knowledge_scavenge_efficiency"]}`
  - row 30: `{"breakthrough_item":"item_field_guide_annotated","category":"scavenging","days_to_complete":9,"description":"Systematic tracking of irradiated flora, fungal mutations, and migration corridors.","display_name":"Wasteland Biological Taxonomy","id":"knowledge_field_guide_taxonomy","prerequisites":["knowledge_scavenge_efficiency"]}`
  - row 31: `{"breakthrough_item":"item_thermal_lance","category":"scavenging","days_to_complete":14,"description":"Magnesium-core cutting lance deployment to melt bank vault and bunker doors.","display_name":"Thermal Lance Breaching Rig","id":"knowledge_hazmat_breaching_technique","prerequisites":["knowledge_scavenge_efficiency","knowledge_high_temp_metallurgy"]}`
  - row 32: `{"category":"combat","days_to_complete":11,"description":"Interlocking field-of-fire barricades and blast baffle construction.","display_name":"Subterranean Chokepoint Fortification","id":"knowledge_fortified_chokepoints","prerequisites":["knowledge_combat_training"]}`
  - row 33: `{"category":"combat","days_to_complete":9,"description":"Concealed perimeter perimeter defense rigs providing early warning against raiders.","display_name":"Acoustic & Frag Tripwire Arrays","id":"knowledge_defensive_tripwire_arrays","prerequisites":["knowledge_combat_training"]}`
  - row 34: `{"breakthrough_item":"item_sentry_targeting_chip","category":"combat","days_to_complete":15,"description":"Logic chip integration with pneumatic pan-tilt servo mounts for autonomous sentry guns.","display_name":"Automated Sentry Mount Targeting","id":"knowledge_automated_sentry_doctrine","prerequisites":["knowledge_combat_training","knowledge_solar_advanced"]}`
  - row 35: `{"category":"combat","days_to_complete":12,"description":"Consistent powder titration and concentric brass reaming for long-range defense.","display_name":"Handloaded Match Ballistics","id":"knowledge_precision_ballistics","prerequisites":["knowledge_combat_training"]}`
  - row 36: `{"category":"survival","days_to_complete":10,"description":"Cultivating edible oyster and button mushroom mycelium on sterilized cardboard substrate.","display_name":"Subterranean Fungiculture","id":"knowledge_subterranean_fungiculture","prerequisites":["knowledge_hydroponics"]}`
  - row 37: `{"category":"medical","days_to_complete":12,"description":"Synthesizing EDTA and Prussian Blue regimens to bind and purge ingested heavy radionuclides.","display_name":"Advanced Chelation Protocols","id":"knowledge_chelation_therapy","prerequisites":["knowledge_radiation_basics"]}`
  - row 38: `{"category":"engineering","days_to_complete":14,"description":"Refractory sand molding to cast replacement engine blocks, pump housings, and armor plates.","display_name":"Heavy Sand Foundry Casting","id":"knowledge_heavy_foundry_casting","prerequisites":["knowledge_high_temp_metallurgy"]}`
  - row 39: `{"category":"science","days_to_complete":10,"description":"Multi-station phase-locked receiver arrays to pinpoint surface distress beacons and radar sites.","display_name":"Automated Radio Triangulation","id":"knowledge_signal_triangulation","prerequisites":["knowledge_radio_basics"]}`
  - row 40: `{"category":"combat","days_to_complete":10,"description":"Tunnel choke traps, false retreats, and acoustic blind-spot exploitation.","display_name":"Guerrilla Tunnel Defense Doctrine","id":"knowledge_guerrilla_ambush_tactics","prerequisites":["knowledge_combat_training"]}`
  - row 41: `{"breakthrough_item":"item_dosimeter_calibrated","category":"medical","days_to_complete":6,"description":"Circuit schematic for miniaturized solid-state gamma detectors.","display_name":"Micro-Dosimeter Blueprint","id":"knowledge_micro_dosimeter_blueprint","prerequisites":[]}`
  - row 42: `{"breakthrough_item":"item_desal_membrane","category":"engineering","days_to_complete":8,"description":"Peltier condensation array blueprint for extracting humidity from shelter exhaust.","display_name":"Atmospheric Water Condenser Blueprint","id":"knowledge_water_condenser_blueprint","prerequisites":[]}`
  - row 43: `{"breakthrough_item":"item_radio_vacuum_tube","category":"science","days_to_complete":6,"description":"Low-noise FET pre-amplifier design for distant radio signal capture.","display_name":"Signal Amplifier Blueprint","id":"knowledge_signal_amplifier_blueprint","prerequisites":[]}`
  - row 44: `{"breakthrough_item":"item_battery_reconditioned","category":"engineering","days_to_complete":8,"description":"Pulse-desulfation charger topology to restore dead lead-acid and lithium cells.","display_name":"Battery Reconditioner Blueprint","id":"knowledge_battery_reconditioner_blueprint","prerequisites":[]}`
  - row 45: `{"breakthrough_item":"item_hydroponic_nutrients","category":"survival","days_to_complete":7,"description":"Automated peristaltic dosing schematic for precise nutrient and pH delivery.","display_name":"Hydroponic Nutrient Doser Blueprint","id":"knowledge_hydroponic_doser_blueprint","prerequisites":[]}`
  - row 46: `{"breakthrough_item":"item_surgical_kit","category":"medical","days_to_complete":7,"description":"Shortwave germicidal UV-C reactor for medical instrument and water sterilization.","display_name":"UV Sterilizer Chamber Blueprint","id":"knowledge_uv_sterilizer_blueprint","prerequisites":[]}`
  - row 47: `{"breakthrough_item":"item_reagent_clean","category":"medical","days_to_complete":5,"description":"High-RPM mechanical separation schematic for blood fractioning and pathogen isolation.","display_name":"Hand-Cranked Centrifuge Blueprint","id":"knowledge_hand_centrifuge_blueprint","prerequisites":[]}`
  - row 48: `{"breakthrough_item":"item_seismic_detector","category":"scavenging","days_to_complete":6,"description":"Piezoelectric ground-vibration sensor array to detect approaching burrowers and cave-ins.","display_name":"Seismic Geophone Sensor Blueprint","id":"knowledge_seismic_geophone_blueprint","prerequisites":[]}`
  - row 49: `{"breakthrough_item":"item_sentry_targeting_chip","category":"combat","days_to_complete":10,"description":"Optical target-tracking logic board for perimeter defense sentry mounts.","display_name":"Automated Turret Controller Blueprint","id":"knowledge_turret_controller_blueprint","prerequisites":[]}`
  - row 50: `{"breakthrough_item":"item_military_radio_module","category":"science","days_to_complete":10,"description":"Frequency-hopping spread spectrum transceiver blueprint for secure long-range communications.","display_name":"Encrypted Military Transceiver Blueprint","id":"knowledge_encrypted_radio_blueprint","prerequisites":[]}`
  - row 51: `{"breakthrough_item":"item_radar_display_tube","category":"scavenging","days_to_complete":9,"description":"X-band weather and threat radar schematic for long-distance expedition reconnaissance.","display_name":"Doppler Radar Scope Blueprint","id":"knowledge_radar_scope_blueprint","prerequisites":[]}`
  - row 52: `{"breakthrough_item":"item_hydraulic_actuator","category":"engineering","days_to_complete":12,"description":"High-torque pneumatic servo actuator blueprint for load-bearing exo-frames.","display_name":"Exoskeleton Actuator Blueprint","id":"knowledge_power_armor_servo_blueprint","prerequisites":[]}`
  - row 53: `{"breakthrough_item":"item_thermal_lance","category":"scavenging","days_to_complete":8,"description":"Magnesium-core thermal cutting torch design to penetrate reinforced blast doors.","display_name":"Thermal Lance Breaching Rig Blueprint","id":"knowledge_vault_breach_blueprint","prerequisites":[]}`
  - row 54: `{"breakthrough_item":"item_iff_beacon","category":"combat","days_to_complete":8,"description":"Pre-war military friend-or-foe beacon encoder to bypass automated defense grids.","display_name":"IFF Transponder Beacon Blueprint","id":"knowledge_iff_transponder_blueprint","prerequisites":[]}`
  - row 55: `{"breakthrough_item":"item_cbrn_cartridge","category":"survival","days_to_complete":9,"description":"Electrostatic carbon-nanotube particulate filter for full-spectrum nuclear and chemical defense.","display_name":"Advanced CBRN Filter Cartridge Blueprint","id":"knowledge_cbrn_filter_blueprint","prerequisites":[]}`
  - row 56: `{"breakthrough_item":"item_surgical_arm_servo","category":"medical","days_to_complete":12,"description":"Micron-precision servo manipulator blueprint for autonomous emergency trauma surgery.","display_name":"Surgical Manipulator Assembly Blueprint","id":"knowledge_surgical_robot_blueprint","prerequisites":[]}`
  - row 57: `{"category":"medical","days_to_complete":6,"description":"Triage order, splinting, and wound closure with what the clinic still had when the ambulances stopped.","display_name":"Field Medicine","id":"knowledge_field_medicine","prerequisites":[]}`
  - row 58: `{"category":"engineering","days_to_complete":5,"description":"Load, leverage, and safe repair practice — knowing which wall holds and which bolt is worth turning.","display_name":"Basic Engineering","id":"knowledge_basic_engineering","prerequisites":[]}`
  - row 59: `{"category":"engineering","days_to_complete":12,"description":"Diagnosis and overhaul of diesel gensets and pumps: fuel paths, compression, and the patience of a cold start.","display_name":"Diesel Mechanics","id":"knowledge_diesel_mechanics","prerequisites":[]}`
  - row 60: `{"category":"engineering","days_to_complete":10,"description":"Handset and loudspeaker repair, antenna matching, and solder salvaged from dead boards.","display_name":"Radio Repair","id":"knowledge_radio_repair","prerequisites":[]}`
  - row 61: `{"category":"survival","days_to_complete":9,"description":"Settling beds, multi-stage sand and charcoal columns, and measured chlorination for cistern-scale water.","display_name":"Water Treatment","id":"knowledge_water_treatment","prerequisites":[]}`
  - row 62: `{"category":"science","days_to_complete":8,"description":"Dosimeter calibration, survey meter maintenance, and dose mapping that turns a hot street into numbers.","display_name":"Radiation Measurement","id":"knowledge_radiation_measurement","prerequisites":[]}`
- Bytes: 21,516; SHA-256: `6eef977ff53a20c22577be27cddf6564426d7d484cece6b17634c136f29d31c7`
- Root keys: `collection_id, knowledge_nodes, schema_version`
- `knowledge_nodes`: list[62]; union fields: `breakthrough_item, category, days_to_complete, description, display_name, id, prerequisites`
  - row 1: `{"category":"survival","days_to_complete":5,"description":"Boiling, charcoal filtration, and still-building from salvage.","display_name":"Water Purification Basics","id":"knowledge_water_basics","prerequisites":[]}`
  - row 2: `{"breakthrough_item":"item_water_filter_advanced","category":"survival","days_to_complete":12,"description":"Multi-stage ceramic filters reduce fallout particulate by 90%.","display_name":"Advanced Water Filtration","id":"knowledge_water_advanced","prerequisites":["knowledge_water_basics"]}`
  - row 3: `{"category":"medical","days_to_complete":5,"description":"Iodine prophylaxis, chelation agents, and dose-ledger tracking.","display_name":"Radiation Medicine Basics","id":"knowledge_radiation_basics","prerequisites":[]}`
  - row 4: `{"breakthrough_item":"item_radiation_shielding_panel","category":"engineering","days_to_complete":15,"description":"Layered lead-cloth, borated polyethylene, and sky-layer armour panels.","display_name":"Radiation Shielding Materials","id":"knowledge_radiation_shielding","prerequisites":["knowledge_radiation_basics"]}`
  - row 5: `{"breakthrough_item":"item_gas_mask_improved","category":"engineering","days_to_complete":10,"description":"Charcoal-canister rebuild doubles filter lifespan under heavy fallout.","display_name":"Improved Gas Masks","id":"knowledge_gas_mask_improved","prerequisites":["knowledge_radiation_basics"]}`
  - row 6: `{"category":"survival","days_to_complete":8,"description":"Nutrient-film technique in recycled bunker trays yields greens in 14 days.","display_name":"Hydroponic Cultivation","id":"knowledge_hydroponics","prerequisites":[]}`
  - row 7: `{"category":"engineering","days_to_complete":7,"description":"Junction-box rebuild and panel-angle tracking from scrap photovoltaic cells.","display_name":"Solar Power Basics","id":"knowledge_solar_basics","prerequisites":[]}`
  - row 8: `{"breakthrough_item":"item_solar_inverter","category":"engineering","days_to_complete":14,"description":"Battery-bank topology and inverter rebuild for overnight draw.","display_name":"Solar Power Systems","id":"knowledge_solar_advanced","prerequisites":["knowledge_solar_basics"]}`
  - row 9: `{"category":"survival","days_to_complete":10,"description":"Salt-curing, cold-smoking, and vacuum-seal scavenge from ruined canneries.","display_name":"Food Preservation","id":"knowledge_food_preservation","prerequisites":[]}`
  - row 10: `{"category":"science","days_to_complete":6,"description":"Direction-finding, squelch calibration, and Morse decoding from static.","display_name":"Radio Signal Processing","id":"knowledge_radio_basics","prerequisites":[]}`
  - row 11: `{"breakthrough_item":"item_radio_cipher_rotor","category":"science","days_to_complete":12,"description":"One-time pad key exchange and frequency-hopping from salvaged cipher rotors.","display_name":"Encrypted Radio Communication","id":"knowledge_radio_advanced","prerequisites":["knowledge_radio_basics"]}`
  - row 12: `{"category":"engineering","days_to_complete":8,"description":"Spray-foam salvage and thermal-barrier panels cut bunker heat loss by 40%.","display_name":"Shelter Insulation","id":"knowledge_shelter_insulation","prerequisites":[]}`
  - row 13: `{"breakthrough_item":"item_air_filter_hepa","category":"engineering","days_to_complete":10,"description":"HEPA-grade filter rebuild extends bunker air-filtration lifespan by 50%.","display_name":"Air Filtration Systems","id":"knowledge_air_filtration","prerequisites":["knowledge_shelter_insulation"]}`
  - row 14: `{"category":"scavenging","days_to_complete":7,"description":"Route-mapping and weight-distribution analysis cuts expedition fatigue by 15%.","display_name":"Scavenge Efficiency","id":"knowledge_scavenge_efficiency","prerequisites":[]}`
  - row 15: `{"category":"combat","days_to_complete":8,"description":"Close-quarters drills and cover-fire protocols improve survivor combat readiness.","display_name":"Combat Training Doctrine","id":"knowledge_combat_training","prerequisites":[]}`
  - row 16: `{"breakthrough_item":"item_hydraulic_actuator","category":"survival","days_to_complete":14,"description":"High-pressure submersible pump rebuilding to tap sub-bedrock aquifers.","display_name":"Deep-Well Hydraulics","id":"knowledge_deep_well_hydraulics","prerequisites":["knowledge_water_advanced"]}`
  - row 17: `{"category":"survival","days_to_complete":12,"description":"Thermal-siphon soil heating and spectral LED augmentation for winter crops.","display_name":"Greenhouse Microclimate Engineering","id":"knowledge_greenhouse_microclimate","prerequisites":["knowledge_hydroponics"]}`
  - row 18: `{"breakthrough_item":"item_vacuum_seal_canner","category":"survival","days_to_complete":11,"description":"Pressure-canning techniques for long-term storage without grid power.","display_name":"Vacuum-Seal Cold Canning","id":"knowledge_cold_canning_preservation","prerequisites":["knowledge_food_preservation"]}`
  - row 19: `{"category":"survival","days_to_complete":9,"description":"Enclosed beehive management and indoor pollination for seed crops.","display_name":"Bunker Apiculture & Pollination","id":"knowledge_apiculture_ecology","prerequisites":["knowledge_hydroponics"]}`
  - row 20: `{"breakthrough_item":"item_surgical_kit","category":"medical","days_to_complete":16,"description":"Emergency thoracotomy and vascular shunts under blackout conditions.","display_name":"Field Trauma Surgery","id":"knowledge_field_trauma_surgery","prerequisites":["knowledge_radiation_basics"]}`
  - row 21: `{"category":"medical","days_to_complete":12,"description":"Negative-pressure anterooms and chlorine mist protocols against epidemics.","display_name":"Pathogen Isolation & Quarantine","id":"knowledge_pathogen_containment","prerequisites":["knowledge_radiation_basics"]}`
  - row 22: `{"breakthrough_item":"item_reagent_clean","category":"medical","days_to_complete":14,"description":"Organic solvent extraction of antibiotics and analgesics from mould and bark.","display_name":"Reagent Pharmacology Synthesis","id":"knowledge_pharmacology_synthesis","prerequisites":["knowledge_pathogen_containment"]}`
  - row 23: `{"category":"engineering","days_to_complete":15,"description":"Refractory clay lining and flux chemistry for scrap tool-steel casting.","display_name":"Crucible Metallurgy Practice","id":"knowledge_high_temp_metallurgy","prerequisites":["knowledge_shelter_insulation"]}`
  - row 24: `{"category":"engineering","days_to_complete":18,"description":"Deep-loop glycol heat pumps extracting subterranean fissure heat.","display_name":"Geothermal Heat Exchanger","id":"knowledge_geothermal_tap","prerequisites":["knowledge_shelter_insulation","knowledge_solar_advanced"]}`
  - row 25: `{"breakthrough_item":"item_diving_suit_vulcanized","category":"engineering","days_to_complete":16,"description":"Vulcanized rubber diving suits and weighted descent harnesses for submerged hulls.","display_name":"Submersible Salvage Rig","id":"knowledge_submersible_salvage_rig","prerequisites":["knowledge_gas_mask_improved"]}`
  - row 26: `{"breakthrough_item":"item_cloud_seeding_canister","category":"science","days_to_complete":16,"description":"Pyrotechnic flare dispersion of condensing nuclei to trigger cleansing rainfall.","display_name":"Silver-Iodide Cloud Seeding","id":"knowledge_atmospheric_cloud_seeding","prerequisites":["knowledge_radio_basics","knowledge_radiation_shielding"]}`
  - row 27: `{"category":"science","days_to_complete":13,"description":"Over-the-horizon radio skip forecasting using geomagnetic sensor loops.","display_name":"Ionospheric Waveguide Analysis","id":"knowledge_ionospheric_propagation","prerequisites":["knowledge_radio_advanced"]}`
  - row 28: `{"breakthrough_item":"item_seismic_detector","category":"science","days_to_complete":11,"description":"Triangulated geophone networks predicting aftershocks and subterranean collapses.","display_name":"Seismic Fault Array Mapping","id":"knowledge_seismic_fault_mapping","prerequisites":["knowledge_radio_basics"]}`
  - row 29: `{"category":"scavenging","days_to_complete":10,"description":"Pre-stressed concrete stress analysis to safely breach collapsed skyscraper vaults.","display_name":"Urban Structural Cartography","id":"knowledge_ruin_structural_survey","prerequisites":["knowledge_scavenge_efficiency"]}`
  - row 30: `{"breakthrough_item":"item_field_guide_annotated","category":"scavenging","days_to_complete":9,"description":"Systematic tracking of irradiated flora, fungal mutations, and migration corridors.","display_name":"Wasteland Biological Taxonomy","id":"knowledge_field_guide_taxonomy","prerequisites":["knowledge_scavenge_efficiency"]}`
  - row 31: `{"breakthrough_item":"item_thermal_lance","category":"scavenging","days_to_complete":14,"description":"Magnesium-core cutting lance deployment to melt bank vault and bunker doors.","display_name":"Thermal Lance Breaching Rig","id":"knowledge_hazmat_breaching_technique","prerequisites":["knowledge_scavenge_efficiency","knowledge_high_temp_metallurgy"]}`
  - row 32: `{"category":"combat","days_to_complete":11,"description":"Interlocking field-of-fire barricades and blast baffle construction.","display_name":"Subterranean Chokepoint Fortification","id":"knowledge_fortified_chokepoints","prerequisites":["knowledge_combat_training"]}`
  - row 33: `{"category":"combat","days_to_complete":9,"description":"Concealed perimeter perimeter defense rigs providing early warning against raiders.","display_name":"Acoustic & Frag Tripwire Arrays","id":"knowledge_defensive_tripwire_arrays","prerequisites":["knowledge_combat_training"]}`
  - row 34: `{"breakthrough_item":"item_sentry_targeting_chip","category":"combat","days_to_complete":15,"description":"Logic chip integration with pneumatic pan-tilt servo mounts for autonomous sentry guns.","display_name":"Automated Sentry Mount Targeting","id":"knowledge_automated_sentry_doctrine","prerequisites":["knowledge_combat_training","knowledge_solar_advanced"]}`
  - row 35: `{"category":"combat","days_to_complete":12,"description":"Consistent powder titration and concentric brass reaming for long-range defense.","display_name":"Handloaded Match Ballistics","id":"knowledge_precision_ballistics","prerequisites":["knowledge_combat_training"]}`
  - row 36: `{"category":"survival","days_to_complete":10,"description":"Cultivating edible oyster and button mushroom mycelium on sterilized cardboard substrate.","display_name":"Subterranean Fungiculture","id":"knowledge_subterranean_fungiculture","prerequisites":["knowledge_hydroponics"]}`
  - row 37: `{"category":"medical","days_to_complete":12,"description":"Synthesizing EDTA and Prussian Blue regimens to bind and purge ingested heavy radionuclides.","display_name":"Advanced Chelation Protocols","id":"knowledge_chelation_therapy","prerequisites":["knowledge_radiation_basics"]}`
  - row 38: `{"category":"engineering","days_to_complete":14,"description":"Refractory sand molding to cast replacement engine blocks, pump housings, and armor plates.","display_name":"Heavy Sand Foundry Casting","id":"knowledge_heavy_foundry_casting","prerequisites":["knowledge_high_temp_metallurgy"]}`
  - row 39: `{"category":"science","days_to_complete":10,"description":"Multi-station phase-locked receiver arrays to pinpoint surface distress beacons and radar sites.","display_name":"Automated Radio Triangulation","id":"knowledge_signal_triangulation","prerequisites":["knowledge_radio_basics"]}`
  - row 40: `{"category":"combat","days_to_complete":10,"description":"Tunnel choke traps, false retreats, and acoustic blind-spot exploitation.","display_name":"Guerrilla Tunnel Defense Doctrine","id":"knowledge_guerrilla_ambush_tactics","prerequisites":["knowledge_combat_training"]}`
  - row 41: `{"breakthrough_item":"item_dosimeter_calibrated","category":"medical","days_to_complete":6,"description":"Circuit schematic for miniaturized solid-state gamma detectors.","display_name":"Micro-Dosimeter Blueprint","id":"knowledge_micro_dosimeter_blueprint","prerequisites":[]}`
  - row 42: `{"breakthrough_item":"item_desal_membrane","category":"engineering","days_to_complete":8,"description":"Peltier condensation array blueprint for extracting humidity from shelter exhaust.","display_name":"Atmospheric Water Condenser Blueprint","id":"knowledge_water_condenser_blueprint","prerequisites":[]}`
  - row 43: `{"breakthrough_item":"item_radio_vacuum_tube","category":"science","days_to_complete":6,"description":"Low-noise FET pre-amplifier design for distant radio signal capture.","display_name":"Signal Amplifier Blueprint","id":"knowledge_signal_amplifier_blueprint","prerequisites":[]}`
  - row 44: `{"breakthrough_item":"item_battery_reconditioned","category":"engineering","days_to_complete":8,"description":"Pulse-desulfation charger topology to restore dead lead-acid and lithium cells.","display_name":"Battery Reconditioner Blueprint","id":"knowledge_battery_reconditioner_blueprint","prerequisites":[]}`
  - row 45: `{"breakthrough_item":"item_hydroponic_nutrients","category":"survival","days_to_complete":7,"description":"Automated peristaltic dosing schematic for precise nutrient and pH delivery.","display_name":"Hydroponic Nutrient Doser Blueprint","id":"knowledge_hydroponic_doser_blueprint","prerequisites":[]}`
  - row 46: `{"breakthrough_item":"item_surgical_kit","category":"medical","days_to_complete":7,"description":"Shortwave germicidal UV-C reactor for medical instrument and water sterilization.","display_name":"UV Sterilizer Chamber Blueprint","id":"knowledge_uv_sterilizer_blueprint","prerequisites":[]}`
  - row 47: `{"breakthrough_item":"item_reagent_clean","category":"medical","days_to_complete":5,"description":"High-RPM mechanical separation schematic for blood fractioning and pathogen isolation.","display_name":"Hand-Cranked Centrifuge Blueprint","id":"knowledge_hand_centrifuge_blueprint","prerequisites":[]}`
  - row 48: `{"breakthrough_item":"item_seismic_detector","category":"scavenging","days_to_complete":6,"description":"Piezoelectric ground-vibration sensor array to detect approaching burrowers and cave-ins.","display_name":"Seismic Geophone Sensor Blueprint","id":"knowledge_seismic_geophone_blueprint","prerequisites":[]}`
  - row 49: `{"breakthrough_item":"item_sentry_targeting_chip","category":"combat","days_to_complete":10,"description":"Optical target-tracking logic board for perimeter defense sentry mounts.","display_name":"Automated Turret Controller Blueprint","id":"knowledge_turret_controller_blueprint","prerequisites":[]}`
  - row 50: `{"breakthrough_item":"item_military_radio_module","category":"science","days_to_complete":10,"description":"Frequency-hopping spread spectrum transceiver blueprint for secure long-range communications.","display_name":"Encrypted Military Transceiver Blueprint","id":"knowledge_encrypted_radio_blueprint","prerequisites":[]}`
  - row 51: `{"breakthrough_item":"item_radar_display_tube","category":"scavenging","days_to_complete":9,"description":"X-band weather and threat radar schematic for long-distance expedition reconnaissance.","display_name":"Doppler Radar Scope Blueprint","id":"knowledge_radar_scope_blueprint","prerequisites":[]}`
  - row 52: `{"breakthrough_item":"item_hydraulic_actuator","category":"engineering","days_to_complete":12,"description":"High-torque pneumatic servo actuator blueprint for load-bearing exo-frames.","display_name":"Exoskeleton Actuator Blueprint","id":"knowledge_power_armor_servo_blueprint","prerequisites":[]}`
  - row 53: `{"breakthrough_item":"item_thermal_lance","category":"scavenging","days_to_complete":8,"description":"Magnesium-core thermal cutting torch design to penetrate reinforced blast doors.","display_name":"Thermal Lance Breaching Rig Blueprint","id":"knowledge_vault_breach_blueprint","prerequisites":[]}`
  - row 54: `{"breakthrough_item":"item_iff_beacon","category":"combat","days_to_complete":8,"description":"Pre-war military friend-or-foe beacon encoder to bypass automated defense grids.","display_name":"IFF Transponder Beacon Blueprint","id":"knowledge_iff_transponder_blueprint","prerequisites":[]}`
  - row 55: `{"breakthrough_item":"item_cbrn_cartridge","category":"survival","days_to_complete":9,"description":"Electrostatic carbon-nanotube particulate filter for full-spectrum nuclear and chemical defense.","display_name":"Advanced CBRN Filter Cartridge Blueprint","id":"knowledge_cbrn_filter_blueprint","prerequisites":[]}`
  - row 56: `{"breakthrough_item":"item_surgical_arm_servo","category":"medical","days_to_complete":12,"description":"Micron-precision servo manipulator blueprint for autonomous emergency trauma surgery.","display_name":"Surgical Manipulator Assembly Blueprint","id":"knowledge_surgical_robot_blueprint","prerequisites":[]}`
  - row 57: `{"category":"medical","days_to_complete":6,"description":"Triage order, splinting, and wound closure with what the clinic still had when the ambulances stopped.","display_name":"Field Medicine","id":"knowledge_field_medicine","prerequisites":[]}`
  - row 58: `{"category":"engineering","days_to_complete":5,"description":"Load, leverage, and safe repair practice — knowing which wall holds and which bolt is worth turning.","display_name":"Basic Engineering","id":"knowledge_basic_engineering","prerequisites":[]}`
  - row 59: `{"category":"engineering","days_to_complete":12,"description":"Diagnosis and overhaul of diesel gensets and pumps: fuel paths, compression, and the patience of a cold start.","display_name":"Diesel Mechanics","id":"knowledge_diesel_mechanics","prerequisites":[]}`
  - row 60: `{"category":"engineering","days_to_complete":10,"description":"Handset and loudspeaker repair, antenna matching, and solder salvaged from dead boards.","display_name":"Radio Repair","id":"knowledge_radio_repair","prerequisites":[]}`
  - row 61: `{"category":"survival","days_to_complete":9,"description":"Settling beds, multi-stage sand and charcoal columns, and measured chlorination for cistern-scale water.","display_name":"Water Treatment","id":"knowledge_water_treatment","prerequisites":[]}`
  - row 62: `{"category":"science","days_to_complete":8,"description":"Dosimeter calibration, survey meter maintenance, and dose mapping that turns a hot street into numbers.","display_name":"Radiation Measurement","id":"knowledge_radiation_measurement","prerequisites":[]}`

## `Assets/StreamingAssets/Data/items.json`
- Bytes: 390,056; SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
- Root keys: `items, schema_version`
- `items`: list[724]; union fields: `category, contamination, decorLocalizedMoraleDelta, degradeRate, description, disassembleYieldFraction, displayName, display_name, durability, empShielded, equipSlot, healthEffect, hungerRestore, id, isEquipable, moraleEffect, radCleanse, radProtection, repairCosts, repairRecipe, scrapValue, stackMax, tags, thirstRestore, tradeValue, type, value, weight, weight_kg`
  - row 1: `{"description":"A sealed glass ampoule of chelating agent concentrate. The label is half dissolved but the formula is standard: binds heavy radionuclides into a water-soluble complex for rinse removal. One ampoule per decon cycle. The pre-war stock won't last forever, and the synthesis requires a working pharma bench.","displayName":"Chelator Concentrate","id":"item_decon_chelator_concentrate","stackMax":20,"tradeValue":8,"type":"Consumable","weight":0.2}`
  - row 2: `{"description":"A cylindrical filtration cartridge with a lead-foil inner liner and activated charcoal matrix. Installed in the decon airlock effluent tank to capture radionuclide-laden particulates before they can be sluiced into the general water system. Good for approximately five hundred liters of contaminated wash water before replacement is required.","displayName":"Lead-Lined Effluent Filter","id":"item_lead_lined_effluent_filter","stackMax":5,"tradeValue":15,"type":"Equipment","weight":3.5}`
  - row 3: `{"description":"A stiff-bristled scrub brush with a neoprene grip and an integrated scraper edge. Designed for the coarse physical removal of radioactive particulates from canvas, leather, and skin before the chemical wash stage. The bristles are nylon — the pre-war kind that does not melt in mild solvent.","displayName":"Heavy Neoprene Scrub Brush","id":"item_heavy_neoprene_scrub_brush","stackMax":3,"tradeValue":4,"type":"Tool","weight":0.5}`
  - row 4: `{"description":"A galvanized steel bin with a rubber gasket seal and a clamp-down lid. Once sealed, the contents are considered permanently isolated: the bin is stacked in the waste gallery and added to the long-term burial manifest. No one opens these again without a very good reason.","displayName":"Sealed Hazardous Waste Bin","id":"item_sealed_waste_bin","stackMax":3,"tradeValue":5,"type":"Container","weight":8.0}`
  - row 5: `{"description":"A pre-war surveying theodolite with a brass body, etched vernier scales, and a spirit level that still holds true. The optics are fogged at the edges but the crosshairs are sharp. Measures horizontal and vertical angles to within five hundredths of a degree. Requires a stable tripod mount and a steady hand.","displayName":"Brass Precision Theodolite","id":"item_theodolite_brass_precision","stackMax":1,"tradeValue":40,"type":"Tool","weight":4.5}`
  - row 6: `{"description":"A collapsible aluminum stadia rod with metric graduations and a reflective target panel. Extends to four meters and collapses to a meter and a half. The red-and-white markings are faded but still legible. Used in conjunction with the theodolite to determine distance and elevation difference.","displayName":"Surveyor Stadia Rod","id":"item_surveyor_stadia_rod","stackMax":2,"tradeValue":12,"type":"Tool","weight":2.0}`
  - row 7: `{"description":"A small bronze plaque stamped with the survey datum designation and a crosshair center mark. Installed at established benchmarks to serve as a permanent reference point. Bronze because it does not rust, does not spark, and survives the freeze-thaw cycle better than iron. The stamp set is in the survey kit.","displayName":"Bronze Datum Plate","id":"item_datum_plate_bronze","stackMax":10,"tradeValue":6,"type":"Material","weight":0.3}`
  - row 8: `{"description":"A twenty-kilogram sack of pre-mixed concrete with a cold-weather additive. Sets in four hours even below freezing. Used for monument foundations, vault reinforcement, and emergency structural repairs. The aggregate is crushed rubble from the upper levels.","displayName":"Quick-Set Concrete Mix","id":"item_concrete_mix","stackMax":5,"tradeValue":3,"type":"Material","weight":20.0}`
  - row 9: `{"description":"A precision-forged steel shaft machined to sub-millimeter tolerance. The bearing journals are polished to a mirror finish and the keyway is broached for a shear pin. This is the heart of the flywheel assembly: everything else spins around it, and if it fails, everything else stops.","displayName":"Forged Rotor Shaft","id":"item_forged_rotor_shaft","stackMax":1,"tradeValue":60,"type":"Material","weight":45.0}`
  - row 10: `{"description":"A set of electromagnetic bearing coils wound with lacquered copper wire and potted in epoxy. When energized, they levitate the rotor shaft on a magnetic field, eliminating mechanical contact at operating speed. The control circuitry is delicate and the coils must be kept absolutely dry.","displayName":"Magnetic Bearing Coil Assembly","id":"item_magnetic_bearing_coil","stackMax":2,"tradeValue":35,"type":"Equipment","weight":3.0}`
  - row 11: `{"description":"A compact turbomolecular pump capable of pulling a vacuum of one ten-thousandth of a torr. The rotor spins at forty thousand RPM on its own magnetic bearings. Without this, the flywheel's aerodynamic drag would turn stored energy into waste heat within hours.","displayName":"High-Vacuum Turbomolecular Pump","id":"item_high_vacuum_pump","stackMax":1,"tradeValue":80,"type":"Equipment","weight":12.0}`
  - row 12: `{"description":"A forged steel ring, two centimeters thick and one meter in diameter, designed to encircle the flywheel rotor. In the event of a catastrophic rotor failure, the ring absorbs the initial fragment impact and redirects the energy into the vault floor. It is not a guarantee — but it is the difference between a contained failure and a breached room.","displayName":"Steel Containment Ring","id":"item_containment_ring_steel","stackMax":1,"tradeValue":45,"type":"Material","weight":80.0}`
  - row 13: `{"description":"A pre-cast reinforced concrete vault section with embedded steel rebar and anchor bolt channels. Weighs over a ton. Designed to be lowered into the flywheel pit and bolted together to form a containment vault that can redirect a rotor failure upward into the ceiling rather than sideways into occupied rooms.","displayName":"Reinforced Concrete Vault Section","id":"item_reinforced_concrete_vault","stackMax":1,"tradeValue":30,"type":"Material","weight":1200.0}`
  - row 14: `{"description":"A layered elastomer-and-steel isolation pad that sits between the flywheel foundation and the bedrock. Absorbs micro-tremors, rotor imbalance vibrations, and the occasional seismic event. Without it, a four-ton rotor at six thousand RPM can transmit enough vibration to crack concrete three rooms away.","displayName":"Seismic Damper Pad","id":"item_seismic_damper_pad","stackMax":2,"tradeValue":25,"type":"Equipment","weight":25.0}`
  - row 15: `{"description":"A liter of synthetic vacuum pump oil with low vapor pressure. Used to lubricate the roughing pump and maintain the turbomolecular pump's backing vacuum. The oil darkens with use as it absorbs water vapor and trace contaminants. Change it on schedule or the vacuum degrades.","displayName":"Vacuum Pump Oil","id":"item_vacuum_pump_oil","stackMax":10,"tradeValue":5,"type":"Consumable","weight":1.0}`
  - row 16: `{"description":"A tube of synthetic grease rated for continuous operation at one hundred twenty degrees Celsius. Used on the flywheel's touchdown bearings — the mechanical backup that engages when the magnetic suspension loses power. Without it, a bearing touchdown at speed becomes a friction weld.","displayName":"High-Temperature Bearing Grease","id":"item_bearing_grease","stackMax":15,"tradeValue":3,"type":"Consumable","weight":0.3}`
  - row 17: `{"description":"A kit containing precision weights, a stroboscopic balancer, and a set of balance-adjustment shims. Used to correct rotor imbalance that develops over time from thermal cycling and material fatigue. An unbalanced rotor at speed is a slow-motion demolition project.","displayName":"Rotor Balancing Kit","id":"item_rotor_balancing_kit","stackMax":2,"tradeValue":20,"type":"Tool","weight":2.5}`
  - row 18: `{"description":"A hand-held photoionization detector with a UV lamp module and interchangeable sensor heads. Responds to a broad range of volatile organic compounds and many inorganic gases. The display shows a normalized concentration bar and hazard-class identification. The battery pack is rechargeable and good for about one hundred twenty scans.","displayName":"Portable PID Detector","id":"item_portable_pid_detector","stackMax":1,"tradeValue":55,"type":"Tool","weight":1.8}`
  - row 19: `{"description":"An interchangeable sensor head for the portable PID detector. Different modules are optimized for different bands: low-band for nerve agents and blister agents, medium-band for corrosive vapors, wide-band for broad-spectrum screening. The module contains the UV lamp, the ionization chamber, and the response-curve calibration data.","displayName":"Detector Sensor Module","id":"item_detector_sensor_module","stackMax":3,"tradeValue":25,"type":"Equipment","weight":0.4}`
  - row 20: `{"description":"A borosilicate glass ampoule with a PTFE-lined screw cap and a vacuum-seal indicator dot. Designed to hold atmospheric or soil samples for transport back to the shelter laboratory. The interior is purged and sterile. Once the cap is sealed, the sample is isolated from the outside environment.","displayName":"Hermetic Sample Ampoule","id":"item_hermetic_sample_ampoule","stackMax":12,"tradeValue":3,"type":"Consumable","weight":0.1}`
  - row 21: `{"description":"A drum of concentrate from the electrostatic air scrubber: the fallout the plates caught so the rest of us would not. Lid welded, sides taped, paint marked with the trefoil. It does not decay on any schedule that matters to us. Bury it deep and mark the map.","displayName":"Sealed Hot-Dust Drum","id":"item_hot_dust_drum","stackMax":4,"tradeValue":0,"type":"Material","weight":12.0}`
  - row 22: `{"description":"A grey-green pressed block of silt and settled muck from the deep sump centrifuge. Heavy, reeking, and faintly warm to the palm. The assay says there is metal in it - a little. The foundry pays in scrap what the ground gives back slowly.","displayName":"Dewatered Sludge Cake","id":"item_sludge_cake","stackMax":10,"tradeValue":2,"type":"Material","weight":5.0}`
  - row 23: `{"description":"A rust-painted drum of centrifuge tailings, lid clamped and the seam sealed with tar. The concentrate even the sump would not keep. Handle with gloves, bury it deep, and do not camp downstream of the hole.","displayName":"Sealed Tailings Drum","id":"item_tailings_drum","stackMax":4,"tradeValue":0,"type":"Material","weight":10.0}`
  - row 24: `{"contamination":0,"description":"A pen-sized instrument on a worn lanyard, its window scratched but the needle still free. It measures accumulated dose in rads, no batteries needed, just a charge you cannot renew. One per person is the rule in every bunker that still keeps rules. Worth thirty at any settlement counter, because the ones that survived are the ones that were already carried. It tells you how long you can stay outside, which is the only question that matters.","displayName":"Dosimeter","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"dosimeter","isEquipable":false,"moraleEffect":0,"radC…`
  - row 25: `{"contamination":0,"description":"A boxy field unit with a speaker grille and a dial marked in rads per hour. It reads the fallout around you, where the dosimeter only counts what you already absorbed. The click rate climbs with the contamination, so you hear the danger before you see it. One kilo, worth forty-two, and it trades fast. The needle still moves on every ash drift. Some scavengers say the clicks are the only honest sound left.","disassembleYieldFraction":0.5,"displayName":"Geiger Counter","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"geiger_counter","isEquipable":false,"moraleEffect":0…`
  - row 26: `{"contamination":0,"description":"A small blister strip of potassium iodide tablets in foil that still crinkles. Taken before or just after exposure, they saturate the thyroid so it passes on the radioactive kind. Five pills to a pack, light enough to forget in a pocket, worth six. The taste is bitter and chalky. People hoard them for the children first, and say nothing about that at the trade table.","displayName":"Iodine Pills","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"iodine_pills","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"tags":["medical"],"thirstR…`
  - row 27: `{"contamination":0,"description":"Decorporation medication in a foil pack of capsules. Taken after exposure, it pulls accumulated dose out of the body, clearing fifty rads per dose. Not a cure, just a deduction, and the body pays for it later. Five packs to a stack, nearly weightless, worth eight. Pharmacies that still stand trade it only for things they cannot scavenge. People argue over the last pack like it is a promise.","displayName":"Anti-Rad","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"anti_rad","isEquipable":false,"moraleEffect":0,"radCleanse":50,"radProtection":0,"stackMax":5,"tags":["m…`
  - row 28: `{"contamination":0,"degradeRate":1.0,"description":"A full-face respirator with twin filter canisters and a rubber seal that still holds. It cuts airborne contamination by thirty percent, enough for minutes in heavy ash and hours in light drift. Full durability when found, and the seal is what you check first, every time. A kilo and a half, worth forty. Most masks still in circulation came off mannequins in closed shops, and the filters are older than the people wearing them.","displayName":"Gas Mask","durability":100,"empShielded":false,"equipSlot":"Face","healthEffect":0,"hungerRestore":0,"id":"gas_mask","isEquipable":true,"moraleEffect":0…`
  - row 29: `{"contamination":0,"degradeRate":0.5,"description":"A one-piece sealed suit of layered PVC with boot covers and a hood. It stops eighty percent of radiation on the body, the best protection you can wear into a hot zone, and every percent shows in the weight: five kilos of plastic and seams. Full durability when it is whole; a tear is a hole you cannot properly patch. Worth forty. The first ones came from hospital stockrooms, and the last wearers died anyway, but slowly, in clean rooms.","displayName":"Hazmat Suit","durability":100,"empShielded":false,"equipSlot":"Body","healthEffect":0,"hungerRestore":0,"id":"hazmat_suit","isEquipable":true,…`
  - row 30: `{"contamination":0,"description":"A hand pump filter the size of a thermos, a ceramic element inside and a rubber bulb outside. It turns standing water into something you can drink without trading rads for thirst, and no power is needed. One filter outfits a bunker for a season if it is kept clean. Half a kilo, worth twenty. Scavengers carry them on long runs and argue over whose turn it is to prime the bulb.","displayName":"Water Filter","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"water_filter","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,…`
  - row 31: `{"contamination":0,"description":"A rectangular panel filter for shelter air systems, sealed in heavy paper. It pulls fallout dust out of the air drawn into a bunker, and the sealing edge has to sit perfectly or it is just a cardboard rectangle. One kilo, worth twenty. Shelters that ran out of these last winter switched to tarps and blankets, and the cough started in December. Check the date stamp. Most of them are three years old now.","displayName":"Air Filter","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"air_filter","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMa…`
  - row 32: `{"contamination":0,"description":"A sealed bottle of clean water, clear to the bottom. It restores forty points of thirst without adding anything to your dose, and even that small certainty, drinking without checking the color first, lifts morale a little. Half a kilo carried, worth fifteen at any settlement. Water this clean is the measure of the exchange: people barter it the way the old world bartered gold.","displayName":"Clean Water","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"clean_water","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":40…`
  - row 33: `{"contamination":0.5,"description":"Murky water in a plastic bottle, silt settled in the bottom. It restores twenty-five points of thirst, but drinking it adds half a point of contamination to your dose. Traded at two, because someone always drinks it. People ration it for the last stretch of a long walk, when thirst outweighs the math. The taste is flat and metallic, and no one describes it out loud.","displayName":"Irradiated Water","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"irradiated_water","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":2…`
  - row 34: `{"contamination":0,"description":"A tin can with a paper label gone grey at the edges. It restores forty points of hunger, and the slow ceremony of heating it, or eating it cold from the tin, is worth two points of morale on its own. Half a kilo, worth twelve. The best ones are three years past their printed date, and the worst are older than that. The dented ones are cheaper, and every so often one has gone bad, which everyone pretends is a rumor.","displayName":"Canned Food","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":40,"id":"canned_food","isEquipable":false,"moraleEffect":2,"radCleanse":0,"radProtec…`
  - row 35: `{"contamination":0,"description":"A plastic jerrycan of fuel, sloshing heavy. Two kilos of diesel or kerosene, worth fourteen, and every liter has a story about who siphoned it and from what. Twenty cans stack in a corner of the bunker, and the corner becomes the warmest place in winter. Fuel is the only thing that turns cold into heat and rust into movement. People measure the winter in cans, not days.","displayName":"Fuel","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"fuel","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":14,"type…`
  - row 36: `{"contamination":0,"description":"Folded cloth, cotton or wool or whatever was left in a factory flat. A fifth of a kilo a bolt, worth a little over one, and it stacks twenty deep. It mends clothes, lines boots, wraps wounds, muffles sound and covers windows. In the old world it was called fabric. Now it is called cloth, and you do not throw any of it away.","displayName":"Cloth","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"cloth","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":1.2,"type":"Material","weight":0.2}`
  - row 37: `{"contamination":0,"description":"Pieces of metal: brackets, panels, a car door someone already stripped. Half a kilo each, worth a little over one, and twenty stack to a load a person can carry. It becomes braces, patch plates, traps and stove pipe. Nothing is thrown away if it is still metal. The ground is full of it now, which is the only part of the world that got richer.","displayName":"Scrap Metal","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"scrap_metal","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":1.2,"type":"Material",…`
  - row 38: `{"contamination":0,"description":"A rolled bandage with a strip of tape and a sealed gauze pad. It restores thirty points of health, stopping the bleed and covering a wound until it can heal on its own. Not a cure, a delay that gives the body time. Worth ten, which is what a hand is worth when you cut it open opening a can. Every bunker has one taped to the inside of a door, waiting.","displayName":"Bandage","durability":0,"empShielded":false,"equipSlot":"","healthEffect":30,"hungerRestore":0,"id":"bandage","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":10,"type":"Medical","…`
  - row 39: `{"contamination":0.3,"description":"A slab of raw meat wrapped in paper, still cold at the edges. Eaten uncooked it adds three tenths of a point of contamination to your dose, so it is always cooked first, over a fire or a stove. Half a kilo, worth twelve, and the animal it came from is never discussed. The meat trades out of sight, because some settlements do not ask where it came from, and others ask too much.","displayName":"Raw Meat","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"raw_meat","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"trad…`
  - row 40: `{"contamination":0,"description":"Cooked meat, dark on the outside, still warm in the middle of the cut. It restores fifty points of hunger and three points of morale, because meat is the difference between surviving and having had dinner. Half a kilo, worth twelve, and it trades faster than anything else in the markets. People who have eaten nothing but roots all month smell it from two stalls away and stop walking.","displayName":"Cooked Meat","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":50,"id":"cooked_meat","isEquipable":false,"moraleEffect":3,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRest…`
  - row 41: `{"contamination":0.6,"description":"Water scooped from a puddle or a barrel, brown at the bottom. Drinking it adds six tenths of a point of contamination to your dose, and it offers nothing back in return. Worth two, and that price is only for the desperate. People boil it anyway, because the fire is cheaper than the dose. The taste of boiled mud stays with you longer than the water does.","displayName":"Dirty Water","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"dirty_water","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":2,"type":…`
  - row 42: `{"contamination":0,"description":"An amber glass ampoule of pharmaceutical morphine, batch seal intact. It restores thirty-five points of health where stronger medicine is wasted on a body that only needs the pain to stop. Two hundred grams to the padded case, worth sixty, four to a stack. Medics ration it in seconds and hours, not milliliters, because the dose that kills the pain is patient, and it keeps count.","displayName":"Morphine Ampoule","durability":0,"empShielded":false,"equipSlot":"","healthEffect":35,"hungerRestore":0,"id":"morphine","isEquipable":false,"moraleEffect":4,"radCleanse":0,"radProtection":0,"stackMax":4,"thirstRestore…`
  - row 43: `{"contamination":0,"description":"A sealed pharmaceutical vial of DMSA compound, pre-war stock. It binds heavy radioisotopes in the bloodstream and escorts them out through the kidneys. One course costs a week and the patient stays close to the basin. Worth ninety on any table that knows what it is; most tables do not.","displayName":"Chelation Agent","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"chelation_agent","isEquipable":false,"moraleEffect":0,"radCleanse":40,"radProtection":0,"stackMax":3,"thirstRestore":0,"tradeValue":90,"type":"Medical","weight":0.15}`
  - row 44: `{"contamination":0,"description":"A small white tablet in a foil blister, stamped with a half-life and a dosage. It saturates the thyroid with stable iodine so the radioactive kind finds no purchase. Timing matters more than amount; taken too late the window is gone. Worth thirty to someone who knows the exposure curve; worth nothing once the window closes.","displayName":"Potassium Iodide Tablet","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"potassium_iodide","isEquipable":false,"moraleEffect":0,"radCleanse":15,"radProtection":0,"stackMax":8,"thirstRestore":0,"tradeValue":30,"type":"Medical","wei…`
  - row 45: `{"contamination":0,"description":"A canvas kit with rolled bandages, tape, scissors and a small bottle of antiseptic. It restores sixty points of health, a proper field kit that can close a cut, pack a wound and stabilize someone for the walk back. Half a kilo, worth ten, and the five kits in a stack are what a bunker calls a clinic. People who carry one tend to walk a little straighter, and everyone notices.","displayName":"Medical Kit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":60,"hungerRestore":0,"id":"medical_kit","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"…`
  - row 46: `{"contamination":0,"description":"A sealed battery, the kind that powered car doors and sirens in another year. Two tenths of a kilo, worth five, and ten stack in a crate that keeps radios, clocks and meters alive a little longer. Every battery is a countdown that started the day it left the factory. People sell the half-dead ones to the people who cannot afford the half-alive ones.","displayName":"Battery","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"battery","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":5,"type":"Material","we…`
  - row 47: `{"contamination":0,"description":"A small case of weights, shims and reference cards for zeroing instruments. It keeps dosimeters and geiger counters honest, because a meter that lies gets people killed by the numbers. Four tenths of a kilo, worth eighteen, and five kits stack in a sack. The people who know how to use it are older than the instruments they service. Nobody regrets paying for an honest reading.","displayName":"Calibration Kit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"calibration_kit","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstResto…`
  - row 48: `{"contamination":0,"description":"Stainless tweezers with a fine point, kept in a leather sleeve. Worth eighteen, which sounds like a lot for a pair of tweezers, until you need a shard of glass out of a hand or a splinter out of a boot sole. A tenth of a kilo, five to a stack. They are one of those things you only understand the value of after you have watched someone work on a wound with a knife because the tweezers were gone.","displayName":"Tweezers","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"tweezers","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirst…`
  - row 49: `{"contamination":0,"description":"Two flat boards with padding and a roll of webbing, sized for an arm or a leg. It holds a broken bone straight so the break can set, worth nine, four tenths of a kilo. The webbing gets reused until it frays, and the boards get carved down until they are kindling. A broken leg without a splint is a long walk on the road; with one, it is three months of favors and boredom.","displayName":"Splint","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"splint","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":9.0,…`
  - row 50: `{"contamination":0,"description":"A blister pack of antibiotics, sealed and dry. Ten packs stack to a weight you can forget, each pack worth ten, which makes it the best value per gram in the wasteland. They treat the infections that turn a small wound into a fever, and the fever into a funeral. Expiry dates were printed on the foil; nobody looks at them anymore. People trade these last, and only for what they cannot steal.","displayName":"Antibiotics","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"antibiotics","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thi…`
  - row 51: `{"contamination":0,"description":"A small piece of jewelry: a ring, a chain, a pin that catches the light. It restores two points of morale when worn, because the person who wears it is not completely reduced yet. Nearly weightless, fifty trade units, and twenty stack in a cloth bag. Wedding rings are the most common, then watches, then everything else. Nobody asks what it cost the previous owner, because the answer is always the same.","displayName":"Jewelry","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"jewelry","isEquipable":false,"moraleEffect":2,"radCleanse":0,"radProtection":0,"stackMax":20,…`
  - row 52: `{"contamination":0,"description":"A loose cut stone kept in a dented steel specimen box. It has no practical use at the Holdfast, but the Cold Ledger still recognizes its old scarcity.","displayName":"Cut Diamond","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"diamond","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":1,"thirstRestore":0,"tradeValue":150,"type":"Trade","weight":0.01}`
  - row 53: `{"contamination":0,"description":"Paper currency from before, bundled with a band that still says a bank name nobody visits. Worth twenty at trade tables, which is what a collector pays and what a fire starter would not. A hundred bills stack to nothing. The old faces on the notes are all gone from the world, and the ink is the only part of them that remains. People use it for the exchange, and never for what it once meant.","displayName":"Paper Currency","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"currency","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":100,"th…`
  - row 54: `{"contamination":0,"description":"Gears, shafts, bearings and fasteners in a greasy bag, the innards of machines that no longer run whole. Three trade units for a fifteenth of a kilo, fifty to a stack. They rebuild pumps, generators, latches and anything else with moving parts. The wasteland runs on salvage, and this is what salvage looks like before it becomes a tool. A machine is just parts that have not been taken apart yet.","disassembleYieldFraction":0,"displayName":"Mechanical Parts","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"mechanical_parts","isEquipable":false,"moraleEffect":0,"radClea…`
  - row 55: `{"contamination":0,"description":"Circuit boards, wiring and chips pulled from dead electronics, the EMP and the years having done the killing. A tenth of a kilo, six trade units, fifty to a stack. Some of it is worth nothing, and some of it is a radio waiting for a soldering iron and an afternoon. People sort it by hand, on a table, in the evenings. The gold pins still shine, and the rest is dust.","disassembleYieldFraction":0,"displayName":"Electronic Scrap","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"electronic_scrap","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"scra…`
  - row 56: `{"category":"equipment","description":"A salvaged radiosonde payload, recovered after atmospheric flight. Contains intact sensors and telemetry logs.","display_name":"Recovered Radiosonde Package","durability":0.85,"id":"item_radiosonde","tags":["electronics","scientific","recovered"],"value":14,"weight_kg":1.2}`
  - row 57: `{"contamination":0,"description":"A single solar cell, glass intact or cracked, frame bent but the wafer still blue. One point two kilos, twenty-two trade units, ten to a stack. Charged, it feeds a battery; broken, it is still the best glass around. The sun still does its part, every day, on schedule. It is the only utility left in the world that has not failed, and people treat it accordingly: first pick, first price, no haggling.","disassembleYieldFraction":0,"displayName":"Solar Cell","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"solar_cell","isEquipable":false,"moraleEffect":0,"radCleanse":0,"…`
  - row 58: `{"contamination":0.05,"description":"Containers of industrial chemicals: acids, solvents, powders in unlabeled jars. Two hundred fifty grams, five trade units, thirty to a stack, and handling them carries a contamination risk of one twentieth of a point. They clean metal, strip paint, set dyes and burn. The jars have no labels, because the labels washed off or were removed. You smell them before you read them.","disassembleYieldFraction":0,"displayName":"Chemicals","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"chemicals","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"scrapV…`
  - row 59: `{"contamination":0,"description":"A handheld radio with a rubber antenna and a cracked dial face. It receives the bands that still carry voices, static, and the occasional transmission from a settlement you cannot reach. Eight tenths of a kilo, worth twenty-two, and it only works if the batteries hold. People listen to it at night, in the dark, alone. The silence between broadcasts is the loudest thing in the bunker.","disassembleYieldFraction":0.5,"displayName":"Handheld Radio","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"handheld_radio","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radPr…`
  - row 60: `{"contamination":0,"description":"An engine block, complete enough to turn: pistons, head, and the wiring that used to be the harness. Twenty-five kilos, worth eighty, full durability when it is whole, and one is all a person carries. It powers a pump, a generator, a workshop belt, or a wagon already half-built in someone's yard. Engines are the heartbeat of the rebuild. People trade whole summers of salvage for one that turns over.","disassembleYieldFraction":0.5,"displayName":"Engine","durability":100,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"engine","isEquipable":false,"moraleEffect":0,"radCleanse":0,"rad…`
  - row 61: `{"contamination":0,"description":"Washed roots, pale and knobby, tied in a bundle. Eight points of hunger per serving, one trade unit, two tenths of a kilo, twenty to a stack. They boil soft in an hour and taste like nothing, which is fine, because nothing is what people can afford. Children are told the thin white ones are the sweet ones. No one argues with them.","displayName":"Roots","healthEffect":0,"hungerRestore":8.0,"id":"roots","isEquipable":false,"moraleEffect":0,"radCleanse":0,"stackMax":20,"thirstRestore":0,"tradeValue":1,"type":"Food","weight":0.2}`
  - row 62: `{"contamination":0,"description":"A handful of dark berries in a folded leaf, soft at the press of a thumb. Six points of hunger, one trade unit, twenty bundles to a stack. Foragers argue about which bushes are safe, and the argument has no referee. People pick in the middle of the day when the light is good, and they bring them home whole. Berries are a small meal and a large question.","displayName":"Berries","healthEffect":0,"hungerRestore":6.0,"id":"berries","isEquipable":false,"moraleEffect":0,"radCleanse":0,"stackMax":20,"thirstRestore":0,"tradeValue":1,"type":"Food","weight":0.15}`
  - row 63: `{"contamination":0,"description":"A glass vacuum tube with filigreed pins, still intact after decades on a shelf. It carries signals the way the old world carried conversations: through heated wire and careful vacuum. Worth eight, a tenth of a kilo, and five to a stack. Radios and gramophones both beg for them. The glass is fragile, and so is the signal.","displayName":"Vacuum Tube","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"vacuum_tube","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":8,"type":"Component","weight":0.1}`
  - row 64: `{"contamination":0,"description":"A coiled spring mechanism, still tensioned inside its housing. It stores energy the way a lung stores breath, and releases it the way a memory releases itself: all at once. Worth six, a fifth of a kilo, five to a stack. Gramophones, clocks, and old traps all rely on the same principle: steel that remembers how to push back.","displayName":"Spring Mechanism","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"spring_mechanism","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":6,"type":"Component","weight":0.…`
  - row 65: `{"contamination":0,"description":"A tiny sapphire needle, still mounted in its cartridge. It reads the grooves of a record the way a finger reads braille: by feeling the shape of something that was made to be heard. Worth four, nearly weightless, ten to a stack. The last ones came from shops that sold music by the disc. Now they are scavenged from the same discs they used to play.","displayName":"Phonograph Needle","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"phonograph_needle","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":4,"ty…`
  - row 66: `{"contamination":0,"description":"A high-wattage projector bulb, filament intact or merely resting. It throws light through a lens the way memory throws light through time: bright enough to see, not bright enough to stay. Worth twelve, a quarter kilo, three to a stack. Film projectors need one, and so do the people who still believe in screenings.","displayName":"Projector Bulb","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"projector_bulb","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":3,"thirstRestore":0,"tradeValue":12,"type":"Component","weight":0.25}`
  - row 67: `{"contamination":0,"description":"A small can of precision lubricant oil, the kind used in clockwork and camera shutters. It reduces friction the way patience reduces panic: slowly, and only when applied correctly. Worth three, a tenth of a kilo, twenty to a stack. Mechanics hoard it. Filmmakers beg for it. The label is gone, but the viscosity is still right.","displayName":"Lubricant Oil","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"lubricant_oil","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":3,"type":"Material","weight":0.1}`
  - row 68: `{"contamination":0,"description":"A metal film reel with a few meters of 8mm celluloid still wound tight. The images on it are someone's birthday, someone's parade, someone's last clear day. Worth fifteen, three tenths of a kilo, five to a stack. The projector needs it, and so does the memory. No one projects these for strangers. Some things are kept private even at the end of the world.","displayName":"Film Reel","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"film_reel","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":15,"type":"Comp…`
  - row 69: `{"contamination":0,"description":"A wound copper antenna coil, tinned and still conductive after years in a damp bunker. It catches signals the way a shoreline catches driftwood: whatever comes close enough to touch. Worth ten, two tenths of a kilo, ten to a stack. Radios need it, and so do the people who still listen for voices in the static.","displayName":"Antenna Coil","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"antenna_coil","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":10,"type":"Component","weight":0.2}`
  - row 70: `{"contamination":0,"description":"A small soldering kit with a coil of rosin-core solder, a tip cleaner, and a pencil iron that still heats when given a battery. It joins wire to wire and trace to trace, which is how the old world fixed anything that broke. Worth fourteen, four tenths of a kilo, five to a stack. Electronics do not stay repaired without it. Neither do radios, and neither do hope.","displayName":"Soldering Kit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"soldering_kit","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue"…`
  - row 71: `{"contamination":0,"description":"A brass music box comb with teeth still filed to pitch. It plucks the cylinder the way a fingernail plucks a thread: each tooth a note, each note a ghost. Worth nine, three tenths of a kilo, five to a stack. The mechanism is useless without it, and the melody is useless without the mechanism. Both are useless without someone to wind the key.","displayName":"Music Box Comb","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"music_box_comb","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":9,"type":"Componen…`
  - row 72: `{"contamination":0,"description":"A small winding key for a music box or clock mechanism, still fitted to its shaft. It stores torque the way a promise stores obligation: tight, and released all at once. Worth four, a tenth of a kilo, ten to a stack. Without it, the comb stays silent and the cylinder stays still. With it, even the oldest mechanism remembers its tune.","displayName":"Spring Key","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"spring_key","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":4,"type":"Component","weight":0.1}`
  - row 73: `{"contamination":0,"description":"A dried typewriter ribbon, ink still dark in the fabric but the strike surface gone to dust. It leaves no mark, which is the tragedy of all good tools worn past their last honest use. Worth three, nearly weightless, ten to a stack. A new ribbon changes everything. This one is a souvenir from the last person who had something to say.","displayName":"Typewriter Ribbon","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"typewriter_ribbon","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":3,"type":"Component"…`
  - row 74: `{"contamination":0,"description":"A small can of machine oil, the thin kind that runs into gears and bearings and makes them forget they ever seized. It stops rust the way a good day stops despair: temporarily, and only where it reaches. Worth two, a tenth of a kilo, twenty to a stack. Typewriters, lathes, and generators all ask for it. The can says industrial. The label is a lie. Everything here is industrial now.","displayName":"Machine Oil","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"machine_oil","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestor…`
  - row 75: `{"contamination":0,"description":"A small lens cleaning kit with a blower brush and a strip of microfiber cloth. It clears fog and dust from glass the way a clear thought clears confusion: slowly, and only when you are patient enough to use it. Worth five, a tenth of a kilo, ten to a stack. Cameras need it. So do the people who still believe there is something worth recording.","displayName":"Lens Cleaning Kit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"camera_lens_cleaner","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":5,"type…`
  - row 76: `{"contamination":0,"description":"A sealed canister of undeveloped 120 film, expiration date long past but the emulsion still potentially viable. It captures light the way a promise captures trust: briefly, and only if you act before it fades. Worth seven, a tenth of a kilo, ten to a stack. A camera without film is a box of good intentions. A film without a camera is a story no one will read.","displayName":"Photographic Film","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"photographic_film","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"trade…`
  - row 77: `{"contamination":0,"description":"A salvaged acoustic decoy module, still responsive to sound triggers. It emits a localized auditory signature that draws hostile attention away from the source. Fragile, improvised, and worth more in the right hands than the wrong. Ten trade units, nearly weightless, three to a stack.","displayName":"Acoustic Decoy","durability":30,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_acoustic_decoy","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":3,"thirstRestore":0,"tradeValue":10,"type":"Device","weight":0.2}`
  - row 78: `{"contamination":0,"description":"A fifty-kilo sack of fertilizer-grade ammonium nitrate, the kind that feeds fields and, under the wrong conditions, changes them. Sealed in a worn canvas sack with a printed lot number that predates the exchange. Worth eighteen, two kilos, five to a stack. Handling it requires the kind of respect that most people have forgotten.","displayName":"Ammonium Nitrate Sack","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_ammonium_nitrate_sack","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":18,"type":"M…`
  - row 79: `{"contamination":0,"description":"A chilled bottle of amnestic syrup, labeled in faded pharmacy script. It induces temporary memory suppression — a mercy in some cases, a liability in others. Worth twenty-two, three tenths of a kilo, five to a stack. The side effects are listed on a label that has mostly peeled away. Doctors used to warn against it. Now they measure doses by eye.","displayName":"Amnestic Syrup","durability":0,"empShielded":false,"equipSlot":"","healthEffect":-10,"hungerRestore":0,"id":"item_amnestic_syrup","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":22,"ty…`
  - row 80: `{"contamination":0,"description":"A sheaf of handwritten notes tied with twine, detailing fixed coordinates, shelter layouts, and cached supply points. The handwriting is steady, the ink faded, the information older than the writer. Worth fourteen, nearly weightless, ten to a stack. They are the difference between walking in circles and walking with purpose.","displayName":"Anchor Notes","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_anchor_notes","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":14,"type":"Document","weight":0.0…`
  - row 81: `{"contamination":0,"degradeRate":0.5,"description":"A salvaged ghillie wrap woven from ash-colored fabric strips and frayed cord. It breaks up a silhouette the way a lie breaks up a confrontation: only if you are patient enough to apply it right. Worth eleven, a kilo and a half, five to a stack. Wasteland scouts and the cautious both treat it as essential. The ash stays in the weave long after you take it off.","displayName":"Ash Ghillie Wrap","durability":40,"empShielded":false,"equipSlot":"Body","healthEffect":0,"hungerRestore":0,"id":"item_ash_ghillie","isEquipable":true,"moraleEffect":0,"radCleanse":0,"radProtection":5,"stackMax":5,"thir…`
  - row 82: `{"contamination":0,"description":"A flexible sheet of mycelium-based bioplastic, grown in a darkroom and cured under pressure. It seals tanks, patches suits, and lines containers the way patience seals wounds: imperfectly, but well enough to hold. Worth sixteen, four tenths of a kilo, ten to a stack. The old world called it experimental. The new world calls it useful.","displayName":"Bio-Plastic Sheet","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_bio_plastic","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":16,"type":"Material…`
  - row 83: `{"contamination":0,"description":"A sealed glass vial of ultra-filtered black water, drawn from a deep aquifer and run through three stages of charcoal and pressure. It restores thirst without the usual contamination tax, which makes it worth trading for. Worth nine, a tenth of a kilo, ten to a stack. The water is so clear it looks like nothing. That is the point.","displayName":"Black Water Vial","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_black_water_vial","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":35,"tradeValue":9,"type":"Water","…`
  - row 84: `{"contamination":0,"description":"A cylindrical CO2 scrubber cartridge filled with activated charcoal and soda lime. It strips carbon dioxide from recirculated air the way a deadline strips hesitation: efficiently, and with an expiration date nobody reads. Worth thirteen, a quarter kilo, five to a stack. Rebreathers and sealed shelters both depend on it. Breathing does not stop when the world does.","displayName":"CO2 Scrubber Cartridge","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_co2_scrubber_cartridge","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thi…`
  - row 85: `{"contamination":0,"description":"A dual-cartridge epoxy injector with a static mixer tip. It bonds metal to metal, ceramic to ceramic, and hope to desperation in under five minutes. Worth eleven, three tenths of a kilo, eight to a stack. The resin cures fast and holds longer than the people who mixed it. Surgeons and mechanics both keep one in their kit.","displayName":"Epoxy Injector","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_epoxy_injector","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":8,"thirstRestore":0,"tradeValue":11,"type":"Tool","weight":0.3}`
  - row 86: `{"contamination":0,"description":"A roll of woven copper Faraday mesh, fine enough to wrap a circuit and thick enough to stop a pulse. It shields electronics the way a locked door shields a room: only if the seal is complete. Worth sixteen, a third of a kilo, five to a stack. EMP survival is not about hardening every device. It is about having one clean room left.","displayName":"Faraday Mesh Roll","durability":0,"empShielded":true,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_faraday_mesh","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":16,"type":"Material","we…`
  - row 87: `{"contamination":0,"description":"A tin of medicated frostbite salve with a sharp camphor smell. It restores circulation and reduces tissue damage when applied early, which is the only time it works. Worth seven, a tenth of a kilo, ten to a stack. People who have lost fingers to the cold keep one in their pocket and check it every morning. Prevention is not a guarantee. It is just a better chance.","displayName":"Frostbite Salve","durability":0,"empShielded":false,"equipSlot":"","healthEffect":20,"hungerRestore":0,"id":"item_frostbite_salve","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0…`
  - row 88: `{"contamination":0,"description":"A pressurized fungicide fogger with a replaceable cartridge. It clears mold from sealed rooms and fungal growth from ventilation shafts the way a whistle clears a room: loudly, and with mixed results. Worth fifteen, four tenths of a kilo, five to a stack. Bunkers that run out of these run out of breathable air faster. Fungus does not negotiate.","displayName":"Fungicide Fogger","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_fungicide_fogger","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":15,"ty…`
  - row 89: `{"contamination":0,"description":"A length of hot-dip galvanized rebar, still coated and still straight. It reinforces concrete the way principles reinforce decisions: visibly, and only if you pour before it sets. Worth eight, three kilos, ten to a stack. Construction crews, bunker crews, and the stubborn all ask for the same thing: something that does not bend.","displayName":"Galvanized Rebar","durability":100,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_galvanized_rebar","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"type":"Material"…`
  - row 90: `{"contamination":0,"description":"A sealed one-litre canister of ethylene glycol antifreeze. It prevents freezing in engines and heat exchangers the way morale prevents collapse in a long winter: chemically, and not for everyone. Worth five, a kilo, five to a stack. Engines, shelters, and the desperate all need it. The label says automotive. The use is broader now.","displayName":"Glycol Antifreeze Canister","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_glycol_antifreeze_canister","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue"…`
  - row 91: `{"contamination":0,"description":"A custom-cut silicone gasket for a bunker hermetic hatch. It seals against pressure, fallout dust, and the slow creep of air that should not be moving. Worth twenty-one, a quarter kilo, five to a stack. Bunkers that skip this do not stay sealed. The difference between a shelter and a sealed room is a strip of rubber someone measured twice.","displayName":"Hermetic Hatch Gasket","durability":60,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_hermetic_hatch_silicone_gasket","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tra…`
  - row 92: `{"contamination":0,"description":"A curved high-tensile steel brace salvaged from a collapsed culvert section. It spans gaps the way a decision spans consequences: with structural integrity, and only if the load is calculated. Worth nineteen, five kilos, five to a stack. Engineers, barricaders, and the hopeful all recognize the same shape: something that was built to hold back the world.","displayName":"Steel Culvert Brace","durability":100,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_high_tensile_steel_culvert_brace","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirs…`
  - row 93: `{"contamination":0,"description":"A heavy insulated battery from a snowmobile engine block, still holding a charge through the cold that killed the machine it came from. It powers heaters, radios, and the small comforts people refuse to give up. Worth sixteen, three kilos, five to a stack. Cold is the oldest enemy. Batteries are the oldest ally.","displayName":"Insulated Snowmobile Battery","durability":60,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_insulated_snowmobile_battery","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":16,"type":"Dev…`
  - row 94: `{"contamination":0,"degradeRate":0.5,"description":"A small lead-shielded cask for transporting radioactive samples. It protects the handler the way a secret protects the guilty: completely, and with moral weight nobody discusses. Worth seventeen, two kilos, three to a stack. Scientists, scavengers, and the foolish all open them eventually. The lead is not there to keep the outside out. It is there to keep the inside in.","displayName":"Lead-Shielded Sample Cask","durability":80,"empShielded":true,"equipSlot":"Body","healthEffect":0,"hungerRestore":0,"id":"item_lead_shielded_sample_cask","isEquipable":true,"moraleEffect":0,"radCleanse":0,"ra…`
  - row 95: `{"contamination":0,"degradeRate":1.0,"description":"A heavy lead-glass visor mounted in a leather head harness. It protects the eyes and face from radiant heat and flash, the way sunglasses protect the eyes from ordinary light: only this kind can blind you permanently. Worth nineteen, eight tenths of a kilo, five to a stack. Welders, radiomen, and the curious all wear them. The glass is clouded. The protection is not.","displayName":"Lead Visor","durability":70,"empShielded":false,"equipSlot":"Face","healthEffect":0,"hungerRestore":0,"id":"item_lead_visor","isEquipable":true,"moraleEffect":0,"radCleanse":0,"radProtection":40,"stackMax":5,"th…`
  - row 96: `{"contamination":0,"description":"A sealed pouch of lithium carbonate salts, the psychiatric staple that became a wasteland trade good. It stabilizes mood the way a fixed schedule stabilizes a day: imperfectly, but enough to function. Worth twenty-four, a tenth of a kilo, ten to a stack. Demand is constant. Supply is not. The people who need it most are the people who can least afford to run out.","displayName":"Lithium Salts","durability":0,"empShielded":false,"equipSlot":"","healthEffect":10,"hungerRestore":0,"id":"item_lithium_salts","isEquipable":false,"moraleEffect":5,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tra…`
  - row 97: `{"contamination":0,"description":"A bundle of insulated copper mine prods, the kind used to test electrical continuity in dangerous circuits. They save lives the way a second opinion saves a diagnosis: by confirming what should not be assumed. Worth seven, a fifth of a kilo, ten to a stack. Electricians, deminers, and the cautious test everything. The prod is the extension of that instinct.","displayName":"Mine Prods","durability":50,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_mine_prod","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":7,"t…`
  - row 98: `{"contamination":0,"description":"Compressed bricks of cultivated mycelium bound with agricultural waste. They insulate, they dampen sound, and they grow if you leave them in the dark too long. Worth thirteen, three kilos, ten to a stack. The old world called it sustainable. The new world calls it available. Either way, they build walls that breathe.","displayName":"Mycelium Bricks","durability":40,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_mycelium_bricks","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":5,"stackMax":10,"thirstRestore":0,"tradeValue":13,"type":"Material","weight":3.0}`
  - row 99: `{"contamination":0,"description":"A bottle of Prussian blue chelating pellets, the cesium and thallium binder that turns internal contamination into something the body can pass. Worth twenty-six, a tenth of a kilo, five to a stack. It does not fix everything. It fixes the specific poisons that certain fallout isotopes leave behind, which is enough to make it worth its weight in clean water.","displayName":"Prussian Blue Chelating Pellets","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_prussian_blue_chelating_pellets","isEquipable":false,"moraleEffect":0,"radCleanse":40,"radProtection":0,"stack…`
  - row 100: `{"contamination":0,"description":"A passive radon detector electret chamber, small enough to carry and slow enough to trust. It accumulates charge the way a bunker accumulates secrets: over time, and only if left undisturbed. Worth fifteen, three tenths of a kilo, five to a stack. Geiger counters catch gamma. This catches the gas that seeps through concrete and accumulates in the dark.","displayName":"Radon Detector Electret","durability":80,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_radon_detector_electret","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore…`
  - row 101: `{"contamination":0,"description":"A compact rebreather scrubber pack with replaceable CO2 and moisture cartridges. It recycles exhaled air the way a library recycles stories: by filtering out the parts that are dangerous to repeat. Worth eighteen, four tenths of a kilo, five to a stack. Extended operations depend on it. So does the discipline to check the gauge before leaving the airlock.","displayName":"Rebreather Scrubber Pack","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_rebreather_scrubber","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore"…`
  - row 102: `{"contamination":0,"description":"A thin-film reverse osmosis membrane sheet, rated for brackish and lightly contaminated water. It turns undrinkable water into drinkable water the way discipline turns chaos into routine: slowly, with waste, and only if the pressure holds. Worth twelve, a tenth of a kilo, ten to a stack. Water filters depend on it. So do the people who refuse to drink from the puddle.","displayName":"Reverse Osmosis Membrane","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_ro_membrane","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRe…`
  - row 103: `{"contamination":0,"description":"A dried bundle of scopolamine-bearing root, harvested from a plant that thrives in disturbed soil. It suppresses memory and nausea, which makes it useful for trauma and for travel. Worth twenty, nearly weightless, ten to a stack. The dose is critical. Too little does nothing. Too much erases the wrong things. Healers used to call it the truth serum. Survivors call it the forgetting herb.","displayName":"Scopolamine Root","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_scopolamine_root","isEquipable":false,"moraleEffect":-2,"radCleanse":0,"radProtection":0,"stac…`
  - row 104: `{"contamination":0,"degradeRate":0.5,"description":"A sealed lead container for transporting radioactive sources. It is heavy, warm to the touch, and marked with a trefoil that nobody alive today remembers being taught to fear. Worth fifteen, three kilos, three to a stack. The radiation inside is measured in sieverts. The respect it demands is measured in distance and time.","displayName":"Sealed Lead Pig","durability":100,"empShielded":true,"equipSlot":"Body","healthEffect":0,"hungerRestore":0,"id":"item_sealed_lead_pig","isEquipable":true,"moraleEffect":0,"radCleanse":0,"radProtection":100,"stackMax":3,"thirstRestore":0,"tradeValue":15,"ty…`
  - row 105: `{"contamination":0,"description":"Goggles carved from scrap leather and fitted with slotted wood or bone. They prevent snow blindness the way a shelter prevents hypothermia: imperfectly, but with enough discipline to make the difference. Worth three, two tenths of a kilo, ten to a stack. The old world called them Inuit goggles. The new world calls them scavenged.","displayName":"Improvised Snow Goggles","durability":20,"empShielded":false,"equipSlot":"Face","healthEffect":0,"hungerRestore":0,"id":"item_snow_goggles_improvised","isEquipable":true,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":3,…`
  - row 106: `{"contamination":0,"description":"Folded panels of acoustic foam and compressed fibreglass, salvaged from recording studios and server rooms. They deaden sound the way a closed mouth deadens conflict: partially, and only if the seal is honest. Worth nine, a kilo, five to a stack. In a bunker, noise carries fear. Silence carries control.","displayName":"Sound Baffling Panels","durability":30,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_sound_baffling","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":9,"type":"Material","weight":1.0}`
  - row 107: `{"contamination":0,"description":"A hard-shell suitcase with a combination dial still set to factory default. It rattles when shaken, which means something solid is inside, and it smells faintly of old tobacco and camphor. Worth seventeen, two kilos, five to a stack. People leave them in bunker corners for years, then open them one morning and find a life packed by someone who never came home.","displayName":"Locked Suitcase","durability":50,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_suitcase_locked","isEquipable":false,"moraleEffect":2,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tra…`
  - row 108: `{"contamination":0,"description":"A stainless steel bone chisel with a sterilized handle and a blade that still holds an edge. It removes bone the way a decision removes doubt: precisely, and with finality. Worth twenty-four, two tenths of a kilo, five to a stack. Surgeons in field conditions use it. So do the desperate, when the alternative is a slow death.","displayName":"Surgical Bone Chisel","durability":80,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_surgical_bone_chisel","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":24,"type":"Medica…`
  - row 109: `{"contamination":0,"description":"A worn teddy bear with one button eye and a fur matted by ash and time. It restores three points of morale simply by being present, which is more than most things in the bunker manage. Worth eight, two tenths of a kilo, ten to a stack. Children claim them. Adults keep them in pockets and say nothing. The bear does not judge the hand that holds it.","displayName":"Teddy Bear","durability":30,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_teddy_bear","isEquipable":false,"moraleEffect":3,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"type":"Tra…`
  - row 110: `{"contamination":0,"description":"A syringe of ceramic thermal paste, the kind used between heat spreaders and processors. It bridges microscopic gaps the way diplomacy bridges ideological ones: thinly, evenly, and with the understanding that both sides are generating heat. Worth five, a tenth of a kilo, ten to a stack. Electronics overheat without it. So do arguments.","displayName":"Thermal Paste","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_thermal_paste","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":5,"type":"Material",…`
  - row 111: `{"contamination":0,"description":"A set of darkened welding glass plates in a steel frame. They filter the arc the way a bunker filters fallout: by blocking the part that does permanent damage. Worth thirteen, a quarter kilo, five to a stack. Welders, mechanics, and the cautious all respect the same brightness threshold. Looking directly at the work is not a test of courage. It is a test of foolishness.","displayName":"Welder's Glass","durability":70,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_welders_glass","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore"…`
  - row 112: `{"contamination":0,"description":"A pair of alkaline AA batteries, still holding a faint charge. They power flashlights, radios, and the small devices people refuse to let go of. Worth three, a tenth of a kilo, twenty to a stack. Every battery in the bunker has a job. These are the ones that run the flashlight nobody turns off.","displayName":"AA Batteries","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"aa_batteries","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":3,"type":"Material","weight":0.1}`
  - row 113: `{"contamination":0,"description":"A sealed box of ten isopropyl alcohol wipes. They sterilize surfaces and skin the way silence sterilizes a room: quickly, and only where applied. Worth four, two tenths of a kilo, ten to a stack. Medics, mechanics, and the cautious keep them close. The seal is intact. That is the first thing to check.","displayName":"Alcohol Wipes (Box of 10)","durability":0,"empShielded":false,"equipSlot":"","healthEffect":5,"hungerRestore":0,"id":"alcohol_wipes_box_10_of_10","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":4,"type":"Medical","weight":0.2}`
  - row 114: `{"contamination":0,"description":"A handful of 7.62x54R jacketed hollow-point armour-piercing rounds. They punch through cover and expand in tissue the way a bad decision punches through a truce: with consequences nobody wanted. Worth eleven, two tenths of a kilo, twenty to a stack. Hunters, defenders, and the desperate treat them as currency. The brass is polished. The lethality is not.","displayName":"7.62x54R JHP-AP Rounds","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_762x54r_jhp_ap","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tra…`
  - row 115: `{"contamination":0,"description":"A box of .357 revolver rounds, brass casings, lead bullets. Feeds jury-rigged pipe rifles and revolvers. The box is dented. The rounds are clean. The ammunition works.","displayName":".357 Rounds","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_357","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":6,"type":"Ammo","weight":0.2}`
  - row 116: `{"contamination":0,"description":"A box of 12-gauge shells, plastic hulls, lead shot. Feeds scrap shotguns. The box is dented. The shells are clean. The ammunition works.","displayName":"12-Gauge Shells","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_12g","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":7,"type":"Ammo","weight":0.25}`
  - row 117: `{"contamination":0,"description":"A box of .308 Winchester ammunition, brass casings, copper bullets. Feeds held-bolt rifles. The box is dented. The rounds are clean. The ammunition works.","displayName":".308 Rounds","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_308","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":9,"type":"Ammo","weight":0.2}`
  - row 118: `{"contamination":0,"description":"A box of 5.56mm ammunition, brass casings, copper bullets. Feeds assault rifles. The box is dented. The rounds are clean. The ammunition works.","displayName":"5.56mm Rounds","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_556","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":11,"type":"Ammo","weight":0.2}`
  - row 119: `{"contamination":0,"description":"A box of 7.62mm ammunition, brass casings, copper bullets. Feeds light machine guns. The box is dented. The rounds are clean. The ammunition works.","displayName":"7.62mm Rounds","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_762","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":12,"type":"Ammo","weight":0.2}`
  - row 120: `{"contamination":0,"description":"A one-litre bottle of surgical-grade antiseptic solution. It cleans wounds and surfaces the way a verdict cleans a court: decisively, and not always gently. Worth seven, a kilo, five to a stack. The label says hospital use. The use is broader now. Every surface in a bunker is a wound waiting to happen.","displayName":"Antiseptic (1L)","durability":0,"empShielded":false,"equipSlot":"","healthEffect":10,"hungerRestore":0,"id":"antiseptic_1l_of_1l","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":7,"type":"Medical","weight":1.0}`
  - row 121: `{"contamination":0,"description":"A sealed battery pack, the kind that powered tools and emergency lighting before the exchange. It stores energy the way a promise stores obligation: visibly, and with an expiration date nobody reads. Worth eight, a kilo, ten to a stack. Radios, heaters, and the long nights all depend on it. People hoard them the way they hoard daylight.","displayName":"Battery Pack","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"battery_pack","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"type":"Device","weight"…`
  - row 122: `{"contamination":0,"description":"A small carton of ten steel nails, galvanized and straight. They hold wood the way a contract holds people: only if both sides are honest and the surface is prepared. Worth two, a tenth of a kilo, twenty to a stack. Carpenters, barricaders, and the desperate all reach for the same thing when the structure fails.","displayName":"Box of Nails (10)","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"box_of_nails_10","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":2,"type":"Material","weight":0.1}`
  - row 123: `{"contamination":0,"description":"A can of condensed soup, label faded but seal intact. It restores thirty points of hunger and one point of morale, because warmth is not just temperature. Worth eight, three tenths of a kilo, ten to a stack. The best ones taste like childhood. The worst ones taste like metal. Nobody asks which is which.","displayName":"Canned Soup","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":30,"id":"canned_soup","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"type":"Food","weight":0.3}`
  - row 124: `{"contamination":0,"description":"A bundle of children's picture books, pages intact but covers softened by damp. They teach reading the way a bunker teaches patience: one letter, one day, one survival at a time. Worth five, three tenths of a kilo, five to a stack. Teachers, parents, and the hopeful all keep a few. The words are simple. The context is not.","displayName":"Children's Books","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"childrens_books","isEquipable":false,"moraleEffect":2,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":5,"type":"Document","weight":0.3}`
  - row 125: `{"contamination":0,"description":"A brass pocket lighter, still filled and still sparking. It creates fire the way a speech creates momentum: out of nothing, and only if the conditions are right. Worth six, two tenths of a kilo, ten to a stack. Smokers, mechanics, and the cold all demand the same thing: a controlled spark in an uncontrolled world.","displayName":"Cigarette Lighter","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"cigarette_lighter","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":6,"type":"Tool","weight":0.2}`
  - row 126: `{"contamination":0,"description":"A one-litre jug of filtered clean water, sealed with a screw cap. It restores thirst without the contamination tax, which is the only tax people refuse to pay voluntarily. Worth twelve, a kilo, five to a stack. Water this clean is the measure of a settlement. The jug says more about the place that filled it than the place that sold it.","displayName":"Clean Water Jug","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"clean_water_jug","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":40,"tradeValue":12,"type":"Water","we…`
  - row 127: `{"contamination":0,"description":"A bottle of vegetable cooking oil, yellow and clear. It calms hunger the way diplomacy calms borders: by making everything more slippery and less direct. Worth four, three tenths of a kilo, ten to a stack. Cooks hoard it. Survivors trade for it. The label says food grade. The use is survival grade.","displayName":"Cooking Oil","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":10,"id":"cooking_oil","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":4,"type":"Food","weight":0.3}`
  - row 128: `{"contamination":0,"description":"A coil of ten metres of solid-core copper wire, insulated and still bright. It carries current the way a road carries traffic: only if the path is clear and the connection is honest. Worth eight, three tenths of a kilo, ten to a stack. Electricians, tinkerers, and the hopeful all measure twice before cutting. Copper does not forgive mistakes.","displayName":"Copper Wire (10m)","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"copper_wire_10m_of_10m","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"ty…`
  - row 129: `{"contamination":0,"description":"A can of diesel fuel, the smell unchanged since the last time a truck engine turned over. It powers generators, heaters, and the slow hope that something might still move. Worth ten, two kilos, five to a stack. Fuel is the measure of winter. The can says litres. The use is survival.","displayName":"Diesel Fuel","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"diesel_fuel","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":10,"type":"Fuel","weight":2.0}`
  - row 130: `{"contamination":0,"description":"A pack of dried meat and grain biscuits, vacuum-sealed and still edible. It restores twenty points of hunger and nothing else, which is exactly what a ration is supposed to do. Worth five, two tenths of a kilo, twenty to a stack. Soldiers, scavengers, and the disciplined eat these first and complain later. Flavour is a luxury. Calories are a promise.","displayName":"Dried Rations","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":20,"id":"dried_rations","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":5,"type":…`
  - row 131: `{"contamination":0,"description":"A roll-up Faraday pack with conductive mesh lining and a magnetic seal. It shields electronics from EMP the way a basement shields people from blast: imperfectly, but better than nothing. Worth fourteen, four tenths of a kilo, five to a stack. The EMP did not end electronics. The lack of shielding did. This is the correction.","displayName":"Faraday Pack","durability":40,"empShielded":true,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"faraday_pack","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":14,"type":"Container","weight":0.4}`
  - row 132: `{"contamination":0,"description":"A compact field surgical kit with scalpels, sutures, and a tourniquet. It closes wounds the way a treaty closes conflict: under pressure, with limited resources, and with the understanding that scarring is inevitable. Worth nineteen, a kilo, five to a stack. Surgeons in the field work with what they carry. This is what they carry.","displayName":"Field Surgical Kit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":60,"hungerRestore":0,"id":"field_surgical_kit","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":19,"type":"Medical",…`
  - row 133: `{"contamination":0,"description":"A sealed one-litre can of fuel, diesel or kerosene, the kind that runs engines and stoves and keeps the dark at bay. Worth six, a kilo, five to a stack. Fuel is measured in litres but traded in survival. A litre is enough to heat a room for an evening or move a vehicle a short distance. Both are victories.","displayName":"Fuel (1L)","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"fuel_1l","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":6,"type":"Fuel","weight":1.0}`
  - row 134: `{"contamination":0,"description":"A compact hydrogen fuel cell, still pressurised and still delivering current. It powers sensors, radios, and life support the way a savings account powers a retirement: slowly, and only if you did not touch it. Worth thirteen, three tenths of a kilo, five to a stack. The technology outlasted the supply chain. That is why people carry them.","displayName":"Fuel Cell","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"fuel_cell","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":13,"type":"Device","weight":0.…`
  - row 135: `{"contamination":0,"description":"A water-damaged growing manual with soil charts and planting calendars. It turns dirt into food the way a teacher turns ignorance into skill: with patience, repetition, and the willingness to fail publicly. Worth eight, three tenths of a kilo, five to a stack. Farmers, gardeners, and the hungry all recognise the same thing: information that survives is information worth trading for.","displayName":"Growing Manual","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"growing_manual","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":5,"thirst…`
  - row 136: `{"contamination":0,"description":"A small bottle of potassium iodide tablets, the thyroid-blocking staple of fallout preparedness. They are bitter, cheap, and worth more than gold when the siren sounds. Worth five, two tenths of a kilo, ten to a stack. Pharmacies used to hand them out for free. Now they are traded like ammo. The taste never got better.","displayName":"Iodine Tablets","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"iodine_tablets","isEquipable":false,"moraleEffect":0,"radCleanse":20,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":5,"type":"Medical","weight":0.2}`
  - row 137: `{"contamination":0,"description":"A cassette tape with a handwritten label. The recording on it is someone's voice, telling a story that may or may not be true. Worth nine, a tenth of a kilo, ten to a stack. Recordings outlive the people who made them. That is both the comfort and the curse of magnetic tape.","displayName":"Cassette Tape","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_cassette_tape","isEquipable":false,"moraleEffect":3,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":9,"type":"Media","weight":0.1}`
  - row 138: `{"contamination":0,"description":"A leather-bound photo album filled with pre-war family photographs. The faces are strangers, the places are gone, and the captions are in handwriting nobody reads anymore. Worth thirteen, three tenths of a kilo, five to a stack. Looking through it is an act of time travel. Closing it is an act of survival.","displayName":"Pre-War Photo Album","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_pre_war_photo_album","isEquipable":false,"moraleEffect":4,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":13,"type":"Document","weight":0.3}`
  - row 139: `{"contamination":0,"description":"A crate of vinyl records, sleeves worn but discs intact. They spin at 33 rpm and carry music that predates the exchange by decades. Worth sixteen, two kilos, five to a stack. Gramophones are rare. Records are not. The mismatch is the tragedy and the trade.","displayName":"Vinyl Record Collection","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_vinyl_collection","isEquipable":false,"moraleEffect":5,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":16,"type":"Media","weight":2.0}`
  - row 140: `{"contamination":0,"description":"An assortment of gears, cams, and bearings scavenged from dead machinery. They are the vocabulary of repair, and without them nothing mechanical says anything intelligible. Worth seven, a quarter kilo, twenty to a stack. Mechanics sort them by shape and sound. The rest of us sort them by hope.","displayName":"Mechanical Components","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"mechanical_components","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":7,"type":"Material","weight":0.25}`
  - row 141: `{"contamination":0,"description":"A canvas medical kit with bandages, antiseptic, and basic surgical tools. It stabilises the injured the way a truce stabilises a war: temporarily, and only if both sides respect the terms. Worth eleven, four tenths of a kilo, five to a stack. Bunkers that run out of these start measuring losses differently.","displayName":"Medkit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":40,"hungerRestore":0,"id":"medkit","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":11,"type":"Medical","weight":0.4}`
  - row 142: `{"contamination":0,"description":"A length of steel pipe, threaded at one end and rusted at the other. It moves water, gas, and ideas through confined spaces the way a messenger moves through hostile territory: quickly, and with risk. Worth three, a kilo, twenty to a stack. Plumbers, welders, and the desperate all recognise the same shape: something hollow that can carry pressure.","displayName":"Metal Pipe","durability":60,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"metal_pipe","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":3,"type":"Material…`
  - row 143: `{"contamination":0,"description":"A steel-frame hatchet with a polymer handle and a blade balanced for throwing or chopping. It splits wood the way a verdict splits a room: with finality, and with attention to who is holding it. Worth twelve, a kilo, five to a stack. Soldiers, woodsmen, and the desperate all sharpen it the same way: with respect.","displayName":"Military-Grade Hatchet","durability":80,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"military_grade_hatchet","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":12,"type":"Tool","weight":1.0}`
  - row 144: `{"contamination":0,"description":"A pre-war military Meal, Ready-to-Eat. The pouch is swollen at one corner, which means the contents are still safe, and the heater works if you have a match. Worth seven, five tenths of a kilo, ten to a stack. Soldiers ate these. Survivors trade for them. The flavour is not the point. The calories are.","displayName":"Military MRE","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":45,"id":"military_mre","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":7,"type":"Food","weight":0.5}`
  - row 145: `{"contamination":0,"description":"A military-specification radio set with encryption modules and a frequency range that still includes the bands that matter. It receives orders, weather, and the occasional voice that sounds like authority. Worth twenty-two, three kilos, five to a stack. The encryption is useless without a key. The listening is not.","displayName":"Military Radio","durability":0,"empShielded":true,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"military_radio","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":22,"type":"Device","weight":3.0}`
  - row 146: `{"contamination":0,"description":"A pack of military-issue ration bars, dense and tasteless and reliable. They restore forty points of hunger and zero points of joy, which is exactly what a survival ration is designed to do. Worth six, three tenths of a kilo, twenty to a stack. Soldiers, scouts, and the practical eat these first and argue about flavour later.","displayName":"Military Rations","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":40,"id":"military_rations","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":6,"type":"Food","weight":0.3}`
  - row 147: `{"contamination":0,"description":"A wooden military supply crate with stencilled markings and a lid that still seals. Inside is the kind of inventory that makes a bunker feel like a fortress: ammo, rations, medical supplies, and the quiet confidence of logistics. Worth twenty-five, five kilos, three to a stack. Crates like this are the reason people dig bunkers in the first place.","displayName":"Military Supply Crate","durability":80,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"military_supply_crate","isEquipable":false,"moraleEffect":2,"radCleanse":0,"radProtection":0,"stackMax":3,"thirstRestore":0,"tradeValu…`
  - row 148: `{"contamination":0,"description":"A replacement cylinder for a music box, programmed with the opening bars of Fur Elise. It plays the same melody every time, which is either comfort or curse depending on the day. Worth seven, a tenth of a kilo, ten to a stack. Music boxes are one of the few machines that do exactly what they were built to do. That is why people keep winding them.","displayName":"Fur Elise Music Box Cylinder","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"music_box_fur_elise","isEquipable":false,"moraleEffect":3,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"trade…`
  - row 149: `{"contamination":0,"description":"A generation-one night vision scope with a damaged IR illuminator and a lens that still gains in the dark. It turns night into grey the way optimism turns despair into strategy: imperfectly, but enough to act. Worth eighteen, a quarter kilo, five to a stack. Scouts, sentries, and the nocturnal all recognise the same advantage: seeing before being seen.","displayName":"Night Vision Scope","durability":0,"empShielded":true,"equipSlot":"Face","healthEffect":0,"hungerRestore":0,"id":"night_vision_scope","isEquipable":true,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValu…`
  - row 150: `{"contamination":0,"description":"A sheet of industrial-grade plastic sheeting, the kind used for vapour barriers and temporary shelters. It keeps moisture out the way a lie keeps the truth out: completely, and only if the edges are sealed. Worth four, three tenths of a kilo, twenty to a stack. Builders, farmers, and the damp all know the same rule: plastic is the difference between a shelter and a cave.","displayName":"Plastic Material Sheet","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"plastic_material","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstR…`
  - row 151: `{"contamination":0,"description":"Torn sheeting, cracked containers, bottle shards — the shelter sheds plastic the way it sheds heat. Sorted and baled, it is feedstock for the retort. Worth one, a fifth of a kilo, fifty to a stack.","displayName":"Waste Plastic Scrap","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"scrap_plastic","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":50,"thirstRestore":0,"tradeValue":1,"type":"Material","weight":0.2}`
  - row 152: `{"contamination":0,"description":"A dented can of reclamation fuel rendered from waste plastic in the back-draft retort. It burns dirty and runs engines rough — expect more wear, fewer kilometres to the can. Worth eight, four kilos, ten to a stack.","displayName":"Retort Synthetic Fuel","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"synthetic_fuel_canister","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"type":"Fuel","weight":4.0}`
  - row 153: `{"contamination":0,"description":"Fine soot pressed from the retort's draft chamber. Seals gaskets, cuts rubber compound, recharge respirator inserts. Breathing it is its own small emergency. Worth three, half a kilo, twenty to a stack.","displayName":"Carbon Black Powder","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"carbon_black_powder","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":3,"type":"Material","weight":0.5}`
  - row 154: `{"contamination":0,"degradeRate":0.5,"description":"A small padded coat with a detachable hood, sized for a child. It provides insulation and a modicum of rad protection the way a promise provides safety: only if the adult keeps it. Worth nine, four tenths of a kilo, five to a stack. Parents trade for it. Survivors keep it. The size never changes. The need does.","displayName":"Child's Protective Coat","durability":50,"empShielded":false,"equipSlot":"Body","healthEffect":0,"hungerRestore":0,"id":"protective_childs_coat","isEquipable":true,"moraleEffect":2,"radCleanse":0,"radProtection":15,"stackMax":5,"thirstRestore":0,"tradeValue":9,"type":…`
  - row 155: `{"contamination":0,"description":"A length of reinforced rubber hose, still flexible and still capable of carrying water or air. It connects systems the way a mediator connects people: by finding a path through resistance. Worth three, two tenths of a kilo, twenty to a stack. Plumbers, mechanics, and the inventive all carry a length. A hose is never the hero. It is the thing that makes the hero possible.","displayName":"Rubber Hose","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"rubber_hose","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"trade…`
  - row 156: `{"contamination":0,"description":"A bundle of salvaged wood planks and boards, warped but still structural. It builds shelves, beds, and the small walls that make a bunker feel like a home. Worth two, a kilo, twenty to a stack. Carpenters call it lumber. Survivors call it possibility. The difference is only in the cutting.","displayName":"Scrap Wood","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"scrap_wood","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":2,"type":"Material","weight":1.0}`
  - row 157: `{"contamination":0,"description":"A manila envelope marked RESTRICTED in faded ink, sealed with wax that cracked long ago. The contents are bureaucratic and obsolete, but bureaucracy once ran the world, and its remnants still carry weight. Worth eleven, two tenths of a kilo, five to a stack. Lawyers, clerks, and the nostalgic all recognise the same thing: authority that outlives its author.","displayName":"Sealed Government Document","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"sealed_government_document","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRe…`
  - row 158: `{"contamination":0,"description":"A envelope of assorted vegetable seeds, some dated, some anonymous. They grow food the way a decision grows consequences: slowly, and only if the soil is honest. Worth six, a tenth of a kilo, twenty to a stack. Farmers, gardeners, and the hopeful all treat seeds as futures. Some of them are. Most of them are not.","displayName":"Seed Packets","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"seed_packets","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":6,"type":"Material","weight":0.1}`
  - row 159: `{"contamination":0,"description":"A bottle of industrial-grade spirits, the kind used for cleaning, sterilising, and forgetting. It burns the throat and clears the mind the way a confrontation clears the air: painfully, and only for a moment. Worth nine, a kilo, five to a stack. Drinkers, medics, and the mournful all recognise the same bottle. The label says medical. The use is broader.","displayName":"Spirits","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"spirits","isEquipable":false,"moraleEffect":-1,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":9,"type":"Material"…`
  - row 160: `{"contamination":0,"description":"A length of steel reinforcing bar, rusted at the cut end and still straight. It reinforces concrete the way a conviction reinforces a person: visibly, and only if poured while the moment is still hot. Worth three, three kilos, ten to a stack. Builders, fortifiers, and the stubborn all recognise the same shape: something that does not bend because it was not asked to.","displayName":"Steel Rebar","durability":100,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"steel_rebar","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeVa…`
  - ... 564 additional rows omitted from the compact audit; the complete current file is identified above ...
- Bytes: 390,056; SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
- Root keys: `items, schema_version`
- `items`: list[724]; union fields: `category, contamination, decorLocalizedMoraleDelta, degradeRate, description, disassembleYieldFraction, displayName, display_name, durability, empShielded, equipSlot, healthEffect, hungerRestore, id, isEquipable, moraleEffect, radCleanse, radProtection, repairCosts, repairRecipe, scrapValue, stackMax, tags, thirstRestore, tradeValue, type, value, weight, weight_kg`
  - row 1: `{"description":"A sealed glass ampoule of chelating agent concentrate. The label is half dissolved but the formula is standard: binds heavy radionuclides into a water-soluble complex for rinse removal. One ampoule per decon cycle. The pre-war stock won't last forever, and the synthesis requires a working pharma bench.","displayName":"Chelator Concentrate","id":"item_decon_chelator_concentrate","stackMax":20,"tradeValue":8,"type":"Consumable","weight":0.2}`
  - row 2: `{"description":"A cylindrical filtration cartridge with a lead-foil inner liner and activated charcoal matrix. Installed in the decon airlock effluent tank to capture radionuclide-laden particulates before they can be sluiced into the general water system. Good for approximately five hundred liters of contaminated wash water before replacement is required.","displayName":"Lead-Lined Effluent Filter","id":"item_lead_lined_effluent_filter","stackMax":5,"tradeValue":15,"type":"Equipment","weight":3.5}`
  - row 3: `{"description":"A stiff-bristled scrub brush with a neoprene grip and an integrated scraper edge. Designed for the coarse physical removal of radioactive particulates from canvas, leather, and skin before the chemical wash stage. The bristles are nylon — the pre-war kind that does not melt in mild solvent.","displayName":"Heavy Neoprene Scrub Brush","id":"item_heavy_neoprene_scrub_brush","stackMax":3,"tradeValue":4,"type":"Tool","weight":0.5}`
  - row 4: `{"description":"A galvanized steel bin with a rubber gasket seal and a clamp-down lid. Once sealed, the contents are considered permanently isolated: the bin is stacked in the waste gallery and added to the long-term burial manifest. No one opens these again without a very good reason.","displayName":"Sealed Hazardous Waste Bin","id":"item_sealed_waste_bin","stackMax":3,"tradeValue":5,"type":"Container","weight":8.0}`
  - row 5: `{"description":"A pre-war surveying theodolite with a brass body, etched vernier scales, and a spirit level that still holds true. The optics are fogged at the edges but the crosshairs are sharp. Measures horizontal and vertical angles to within five hundredths of a degree. Requires a stable tripod mount and a steady hand.","displayName":"Brass Precision Theodolite","id":"item_theodolite_brass_precision","stackMax":1,"tradeValue":40,"type":"Tool","weight":4.5}`
  - row 6: `{"description":"A collapsible aluminum stadia rod with metric graduations and a reflective target panel. Extends to four meters and collapses to a meter and a half. The red-and-white markings are faded but still legible. Used in conjunction with the theodolite to determine distance and elevation difference.","displayName":"Surveyor Stadia Rod","id":"item_surveyor_stadia_rod","stackMax":2,"tradeValue":12,"type":"Tool","weight":2.0}`
  - row 7: `{"description":"A small bronze plaque stamped with the survey datum designation and a crosshair center mark. Installed at established benchmarks to serve as a permanent reference point. Bronze because it does not rust, does not spark, and survives the freeze-thaw cycle better than iron. The stamp set is in the survey kit.","displayName":"Bronze Datum Plate","id":"item_datum_plate_bronze","stackMax":10,"tradeValue":6,"type":"Material","weight":0.3}`
  - row 8: `{"description":"A twenty-kilogram sack of pre-mixed concrete with a cold-weather additive. Sets in four hours even below freezing. Used for monument foundations, vault reinforcement, and emergency structural repairs. The aggregate is crushed rubble from the upper levels.","displayName":"Quick-Set Concrete Mix","id":"item_concrete_mix","stackMax":5,"tradeValue":3,"type":"Material","weight":20.0}`
  - row 9: `{"description":"A precision-forged steel shaft machined to sub-millimeter tolerance. The bearing journals are polished to a mirror finish and the keyway is broached for a shear pin. This is the heart of the flywheel assembly: everything else spins around it, and if it fails, everything else stops.","displayName":"Forged Rotor Shaft","id":"item_forged_rotor_shaft","stackMax":1,"tradeValue":60,"type":"Material","weight":45.0}`
  - row 10: `{"description":"A set of electromagnetic bearing coils wound with lacquered copper wire and potted in epoxy. When energized, they levitate the rotor shaft on a magnetic field, eliminating mechanical contact at operating speed. The control circuitry is delicate and the coils must be kept absolutely dry.","displayName":"Magnetic Bearing Coil Assembly","id":"item_magnetic_bearing_coil","stackMax":2,"tradeValue":35,"type":"Equipment","weight":3.0}`
  - row 11: `{"description":"A compact turbomolecular pump capable of pulling a vacuum of one ten-thousandth of a torr. The rotor spins at forty thousand RPM on its own magnetic bearings. Without this, the flywheel's aerodynamic drag would turn stored energy into waste heat within hours.","displayName":"High-Vacuum Turbomolecular Pump","id":"item_high_vacuum_pump","stackMax":1,"tradeValue":80,"type":"Equipment","weight":12.0}`
  - row 12: `{"description":"A forged steel ring, two centimeters thick and one meter in diameter, designed to encircle the flywheel rotor. In the event of a catastrophic rotor failure, the ring absorbs the initial fragment impact and redirects the energy into the vault floor. It is not a guarantee — but it is the difference between a contained failure and a breached room.","displayName":"Steel Containment Ring","id":"item_containment_ring_steel","stackMax":1,"tradeValue":45,"type":"Material","weight":80.0}`
  - row 13: `{"description":"A pre-cast reinforced concrete vault section with embedded steel rebar and anchor bolt channels. Weighs over a ton. Designed to be lowered into the flywheel pit and bolted together to form a containment vault that can redirect a rotor failure upward into the ceiling rather than sideways into occupied rooms.","displayName":"Reinforced Concrete Vault Section","id":"item_reinforced_concrete_vault","stackMax":1,"tradeValue":30,"type":"Material","weight":1200.0}`
  - row 14: `{"description":"A layered elastomer-and-steel isolation pad that sits between the flywheel foundation and the bedrock. Absorbs micro-tremors, rotor imbalance vibrations, and the occasional seismic event. Without it, a four-ton rotor at six thousand RPM can transmit enough vibration to crack concrete three rooms away.","displayName":"Seismic Damper Pad","id":"item_seismic_damper_pad","stackMax":2,"tradeValue":25,"type":"Equipment","weight":25.0}`
  - row 15: `{"description":"A liter of synthetic vacuum pump oil with low vapor pressure. Used to lubricate the roughing pump and maintain the turbomolecular pump's backing vacuum. The oil darkens with use as it absorbs water vapor and trace contaminants. Change it on schedule or the vacuum degrades.","displayName":"Vacuum Pump Oil","id":"item_vacuum_pump_oil","stackMax":10,"tradeValue":5,"type":"Consumable","weight":1.0}`
  - row 16: `{"description":"A tube of synthetic grease rated for continuous operation at one hundred twenty degrees Celsius. Used on the flywheel's touchdown bearings — the mechanical backup that engages when the magnetic suspension loses power. Without it, a bearing touchdown at speed becomes a friction weld.","displayName":"High-Temperature Bearing Grease","id":"item_bearing_grease","stackMax":15,"tradeValue":3,"type":"Consumable","weight":0.3}`
  - row 17: `{"description":"A kit containing precision weights, a stroboscopic balancer, and a set of balance-adjustment shims. Used to correct rotor imbalance that develops over time from thermal cycling and material fatigue. An unbalanced rotor at speed is a slow-motion demolition project.","displayName":"Rotor Balancing Kit","id":"item_rotor_balancing_kit","stackMax":2,"tradeValue":20,"type":"Tool","weight":2.5}`
  - row 18: `{"description":"A hand-held photoionization detector with a UV lamp module and interchangeable sensor heads. Responds to a broad range of volatile organic compounds and many inorganic gases. The display shows a normalized concentration bar and hazard-class identification. The battery pack is rechargeable and good for about one hundred twenty scans.","displayName":"Portable PID Detector","id":"item_portable_pid_detector","stackMax":1,"tradeValue":55,"type":"Tool","weight":1.8}`
  - row 19: `{"description":"An interchangeable sensor head for the portable PID detector. Different modules are optimized for different bands: low-band for nerve agents and blister agents, medium-band for corrosive vapors, wide-band for broad-spectrum screening. The module contains the UV lamp, the ionization chamber, and the response-curve calibration data.","displayName":"Detector Sensor Module","id":"item_detector_sensor_module","stackMax":3,"tradeValue":25,"type":"Equipment","weight":0.4}`
  - row 20: `{"description":"A borosilicate glass ampoule with a PTFE-lined screw cap and a vacuum-seal indicator dot. Designed to hold atmospheric or soil samples for transport back to the shelter laboratory. The interior is purged and sterile. Once the cap is sealed, the sample is isolated from the outside environment.","displayName":"Hermetic Sample Ampoule","id":"item_hermetic_sample_ampoule","stackMax":12,"tradeValue":3,"type":"Consumable","weight":0.1}`
  - row 21: `{"description":"A drum of concentrate from the electrostatic air scrubber: the fallout the plates caught so the rest of us would not. Lid welded, sides taped, paint marked with the trefoil. It does not decay on any schedule that matters to us. Bury it deep and mark the map.","displayName":"Sealed Hot-Dust Drum","id":"item_hot_dust_drum","stackMax":4,"tradeValue":0,"type":"Material","weight":12.0}`
  - row 22: `{"description":"A grey-green pressed block of silt and settled muck from the deep sump centrifuge. Heavy, reeking, and faintly warm to the palm. The assay says there is metal in it - a little. The foundry pays in scrap what the ground gives back slowly.","displayName":"Dewatered Sludge Cake","id":"item_sludge_cake","stackMax":10,"tradeValue":2,"type":"Material","weight":5.0}`
  - row 23: `{"description":"A rust-painted drum of centrifuge tailings, lid clamped and the seam sealed with tar. The concentrate even the sump would not keep. Handle with gloves, bury it deep, and do not camp downstream of the hole.","displayName":"Sealed Tailings Drum","id":"item_tailings_drum","stackMax":4,"tradeValue":0,"type":"Material","weight":10.0}`
  - row 24: `{"contamination":0,"description":"A pen-sized instrument on a worn lanyard, its window scratched but the needle still free. It measures accumulated dose in rads, no batteries needed, just a charge you cannot renew. One per person is the rule in every bunker that still keeps rules. Worth thirty at any settlement counter, because the ones that survived are the ones that were already carried. It tells you how long you can stay outside, which is the only question that matters.","displayName":"Dosimeter","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"dosimeter","isEquipable":false,"moraleEffect":0,"radC…`
  - row 25: `{"contamination":0,"description":"A boxy field unit with a speaker grille and a dial marked in rads per hour. It reads the fallout around you, where the dosimeter only counts what you already absorbed. The click rate climbs with the contamination, so you hear the danger before you see it. One kilo, worth forty-two, and it trades fast. The needle still moves on every ash drift. Some scavengers say the clicks are the only honest sound left.","disassembleYieldFraction":0.5,"displayName":"Geiger Counter","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"geiger_counter","isEquipable":false,"moraleEffect":0…`
  - row 26: `{"contamination":0,"description":"A small blister strip of potassium iodide tablets in foil that still crinkles. Taken before or just after exposure, they saturate the thyroid so it passes on the radioactive kind. Five pills to a pack, light enough to forget in a pocket, worth six. The taste is bitter and chalky. People hoard them for the children first, and say nothing about that at the trade table.","displayName":"Iodine Pills","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"iodine_pills","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"tags":["medical"],"thirstR…`
  - row 27: `{"contamination":0,"description":"Decorporation medication in a foil pack of capsules. Taken after exposure, it pulls accumulated dose out of the body, clearing fifty rads per dose. Not a cure, just a deduction, and the body pays for it later. Five packs to a stack, nearly weightless, worth eight. Pharmacies that still stand trade it only for things they cannot scavenge. People argue over the last pack like it is a promise.","displayName":"Anti-Rad","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"anti_rad","isEquipable":false,"moraleEffect":0,"radCleanse":50,"radProtection":0,"stackMax":5,"tags":["m…`
  - row 28: `{"contamination":0,"degradeRate":1.0,"description":"A full-face respirator with twin filter canisters and a rubber seal that still holds. It cuts airborne contamination by thirty percent, enough for minutes in heavy ash and hours in light drift. Full durability when found, and the seal is what you check first, every time. A kilo and a half, worth forty. Most masks still in circulation came off mannequins in closed shops, and the filters are older than the people wearing them.","displayName":"Gas Mask","durability":100,"empShielded":false,"equipSlot":"Face","healthEffect":0,"hungerRestore":0,"id":"gas_mask","isEquipable":true,"moraleEffect":0…`
  - row 29: `{"contamination":0,"degradeRate":0.5,"description":"A one-piece sealed suit of layered PVC with boot covers and a hood. It stops eighty percent of radiation on the body, the best protection you can wear into a hot zone, and every percent shows in the weight: five kilos of plastic and seams. Full durability when it is whole; a tear is a hole you cannot properly patch. Worth forty. The first ones came from hospital stockrooms, and the last wearers died anyway, but slowly, in clean rooms.","displayName":"Hazmat Suit","durability":100,"empShielded":false,"equipSlot":"Body","healthEffect":0,"hungerRestore":0,"id":"hazmat_suit","isEquipable":true,…`
  - row 30: `{"contamination":0,"description":"A hand pump filter the size of a thermos, a ceramic element inside and a rubber bulb outside. It turns standing water into something you can drink without trading rads for thirst, and no power is needed. One filter outfits a bunker for a season if it is kept clean. Half a kilo, worth twenty. Scavengers carry them on long runs and argue over whose turn it is to prime the bulb.","displayName":"Water Filter","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"water_filter","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,…`
  - row 31: `{"contamination":0,"description":"A rectangular panel filter for shelter air systems, sealed in heavy paper. It pulls fallout dust out of the air drawn into a bunker, and the sealing edge has to sit perfectly or it is just a cardboard rectangle. One kilo, worth twenty. Shelters that ran out of these last winter switched to tarps and blankets, and the cough started in December. Check the date stamp. Most of them are three years old now.","displayName":"Air Filter","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"air_filter","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMa…`
  - row 32: `{"contamination":0,"description":"A sealed bottle of clean water, clear to the bottom. It restores forty points of thirst without adding anything to your dose, and even that small certainty, drinking without checking the color first, lifts morale a little. Half a kilo carried, worth fifteen at any settlement. Water this clean is the measure of the exchange: people barter it the way the old world bartered gold.","displayName":"Clean Water","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"clean_water","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":40…`
  - row 33: `{"contamination":0.5,"description":"Murky water in a plastic bottle, silt settled in the bottom. It restores twenty-five points of thirst, but drinking it adds half a point of contamination to your dose. Traded at two, because someone always drinks it. People ration it for the last stretch of a long walk, when thirst outweighs the math. The taste is flat and metallic, and no one describes it out loud.","displayName":"Irradiated Water","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"irradiated_water","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":2…`
  - row 34: `{"contamination":0,"description":"A tin can with a paper label gone grey at the edges. It restores forty points of hunger, and the slow ceremony of heating it, or eating it cold from the tin, is worth two points of morale on its own. Half a kilo, worth twelve. The best ones are three years past their printed date, and the worst are older than that. The dented ones are cheaper, and every so often one has gone bad, which everyone pretends is a rumor.","displayName":"Canned Food","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":40,"id":"canned_food","isEquipable":false,"moraleEffect":2,"radCleanse":0,"radProtec…`
  - row 35: `{"contamination":0,"description":"A plastic jerrycan of fuel, sloshing heavy. Two kilos of diesel or kerosene, worth fourteen, and every liter has a story about who siphoned it and from what. Twenty cans stack in a corner of the bunker, and the corner becomes the warmest place in winter. Fuel is the only thing that turns cold into heat and rust into movement. People measure the winter in cans, not days.","displayName":"Fuel","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"fuel","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":14,"type…`
  - row 36: `{"contamination":0,"description":"Folded cloth, cotton or wool or whatever was left in a factory flat. A fifth of a kilo a bolt, worth a little over one, and it stacks twenty deep. It mends clothes, lines boots, wraps wounds, muffles sound and covers windows. In the old world it was called fabric. Now it is called cloth, and you do not throw any of it away.","displayName":"Cloth","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"cloth","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":1.2,"type":"Material","weight":0.2}`
  - row 37: `{"contamination":0,"description":"Pieces of metal: brackets, panels, a car door someone already stripped. Half a kilo each, worth a little over one, and twenty stack to a load a person can carry. It becomes braces, patch plates, traps and stove pipe. Nothing is thrown away if it is still metal. The ground is full of it now, which is the only part of the world that got richer.","displayName":"Scrap Metal","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"scrap_metal","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":1.2,"type":"Material",…`
  - row 38: `{"contamination":0,"description":"A rolled bandage with a strip of tape and a sealed gauze pad. It restores thirty points of health, stopping the bleed and covering a wound until it can heal on its own. Not a cure, a delay that gives the body time. Worth ten, which is what a hand is worth when you cut it open opening a can. Every bunker has one taped to the inside of a door, waiting.","displayName":"Bandage","durability":0,"empShielded":false,"equipSlot":"","healthEffect":30,"hungerRestore":0,"id":"bandage","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":10,"type":"Medical","…`
  - row 39: `{"contamination":0.3,"description":"A slab of raw meat wrapped in paper, still cold at the edges. Eaten uncooked it adds three tenths of a point of contamination to your dose, so it is always cooked first, over a fire or a stove. Half a kilo, worth twelve, and the animal it came from is never discussed. The meat trades out of sight, because some settlements do not ask where it came from, and others ask too much.","displayName":"Raw Meat","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"raw_meat","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"trad…`
  - row 40: `{"contamination":0,"description":"Cooked meat, dark on the outside, still warm in the middle of the cut. It restores fifty points of hunger and three points of morale, because meat is the difference between surviving and having had dinner. Half a kilo, worth twelve, and it trades faster than anything else in the markets. People who have eaten nothing but roots all month smell it from two stalls away and stop walking.","displayName":"Cooked Meat","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":50,"id":"cooked_meat","isEquipable":false,"moraleEffect":3,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRest…`
  - row 41: `{"contamination":0.6,"description":"Water scooped from a puddle or a barrel, brown at the bottom. Drinking it adds six tenths of a point of contamination to your dose, and it offers nothing back in return. Worth two, and that price is only for the desperate. People boil it anyway, because the fire is cheaper than the dose. The taste of boiled mud stays with you longer than the water does.","displayName":"Dirty Water","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"dirty_water","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":2,"type":…`
  - row 42: `{"contamination":0,"description":"An amber glass ampoule of pharmaceutical morphine, batch seal intact. It restores thirty-five points of health where stronger medicine is wasted on a body that only needs the pain to stop. Two hundred grams to the padded case, worth sixty, four to a stack. Medics ration it in seconds and hours, not milliliters, because the dose that kills the pain is patient, and it keeps count.","displayName":"Morphine Ampoule","durability":0,"empShielded":false,"equipSlot":"","healthEffect":35,"hungerRestore":0,"id":"morphine","isEquipable":false,"moraleEffect":4,"radCleanse":0,"radProtection":0,"stackMax":4,"thirstRestore…`
  - row 43: `{"contamination":0,"description":"A sealed pharmaceutical vial of DMSA compound, pre-war stock. It binds heavy radioisotopes in the bloodstream and escorts them out through the kidneys. One course costs a week and the patient stays close to the basin. Worth ninety on any table that knows what it is; most tables do not.","displayName":"Chelation Agent","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"chelation_agent","isEquipable":false,"moraleEffect":0,"radCleanse":40,"radProtection":0,"stackMax":3,"thirstRestore":0,"tradeValue":90,"type":"Medical","weight":0.15}`
  - row 44: `{"contamination":0,"description":"A small white tablet in a foil blister, stamped with a half-life and a dosage. It saturates the thyroid with stable iodine so the radioactive kind finds no purchase. Timing matters more than amount; taken too late the window is gone. Worth thirty to someone who knows the exposure curve; worth nothing once the window closes.","displayName":"Potassium Iodide Tablet","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"potassium_iodide","isEquipable":false,"moraleEffect":0,"radCleanse":15,"radProtection":0,"stackMax":8,"thirstRestore":0,"tradeValue":30,"type":"Medical","wei…`
  - row 45: `{"contamination":0,"description":"A canvas kit with rolled bandages, tape, scissors and a small bottle of antiseptic. It restores sixty points of health, a proper field kit that can close a cut, pack a wound and stabilize someone for the walk back. Half a kilo, worth ten, and the five kits in a stack are what a bunker calls a clinic. People who carry one tend to walk a little straighter, and everyone notices.","displayName":"Medical Kit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":60,"hungerRestore":0,"id":"medical_kit","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"…`
  - row 46: `{"contamination":0,"description":"A sealed battery, the kind that powered car doors and sirens in another year. Two tenths of a kilo, worth five, and ten stack in a crate that keeps radios, clocks and meters alive a little longer. Every battery is a countdown that started the day it left the factory. People sell the half-dead ones to the people who cannot afford the half-alive ones.","displayName":"Battery","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"battery","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":5,"type":"Material","we…`
  - row 47: `{"contamination":0,"description":"A small case of weights, shims and reference cards for zeroing instruments. It keeps dosimeters and geiger counters honest, because a meter that lies gets people killed by the numbers. Four tenths of a kilo, worth eighteen, and five kits stack in a sack. The people who know how to use it are older than the instruments they service. Nobody regrets paying for an honest reading.","displayName":"Calibration Kit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"calibration_kit","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstResto…`
  - row 48: `{"contamination":0,"description":"Stainless tweezers with a fine point, kept in a leather sleeve. Worth eighteen, which sounds like a lot for a pair of tweezers, until you need a shard of glass out of a hand or a splinter out of a boot sole. A tenth of a kilo, five to a stack. They are one of those things you only understand the value of after you have watched someone work on a wound with a knife because the tweezers were gone.","displayName":"Tweezers","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"tweezers","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirst…`
  - row 49: `{"contamination":0,"description":"Two flat boards with padding and a roll of webbing, sized for an arm or a leg. It holds a broken bone straight so the break can set, worth nine, four tenths of a kilo. The webbing gets reused until it frays, and the boards get carved down until they are kindling. A broken leg without a splint is a long walk on the road; with one, it is three months of favors and boredom.","displayName":"Splint","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"splint","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":9.0,…`
  - row 50: `{"contamination":0,"description":"A blister pack of antibiotics, sealed and dry. Ten packs stack to a weight you can forget, each pack worth ten, which makes it the best value per gram in the wasteland. They treat the infections that turn a small wound into a fever, and the fever into a funeral. Expiry dates were printed on the foil; nobody looks at them anymore. People trade these last, and only for what they cannot steal.","displayName":"Antibiotics","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"antibiotics","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thi…`
  - row 51: `{"contamination":0,"description":"A small piece of jewelry: a ring, a chain, a pin that catches the light. It restores two points of morale when worn, because the person who wears it is not completely reduced yet. Nearly weightless, fifty trade units, and twenty stack in a cloth bag. Wedding rings are the most common, then watches, then everything else. Nobody asks what it cost the previous owner, because the answer is always the same.","displayName":"Jewelry","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"jewelry","isEquipable":false,"moraleEffect":2,"radCleanse":0,"radProtection":0,"stackMax":20,…`
  - row 52: `{"contamination":0,"description":"A loose cut stone kept in a dented steel specimen box. It has no practical use at the Holdfast, but the Cold Ledger still recognizes its old scarcity.","displayName":"Cut Diamond","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"diamond","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":1,"thirstRestore":0,"tradeValue":150,"type":"Trade","weight":0.01}`
  - row 53: `{"contamination":0,"description":"Paper currency from before, bundled with a band that still says a bank name nobody visits. Worth twenty at trade tables, which is what a collector pays and what a fire starter would not. A hundred bills stack to nothing. The old faces on the notes are all gone from the world, and the ink is the only part of them that remains. People use it for the exchange, and never for what it once meant.","displayName":"Paper Currency","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"currency","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":100,"th…`
  - row 54: `{"contamination":0,"description":"Gears, shafts, bearings and fasteners in a greasy bag, the innards of machines that no longer run whole. Three trade units for a fifteenth of a kilo, fifty to a stack. They rebuild pumps, generators, latches and anything else with moving parts. The wasteland runs on salvage, and this is what salvage looks like before it becomes a tool. A machine is just parts that have not been taken apart yet.","disassembleYieldFraction":0,"displayName":"Mechanical Parts","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"mechanical_parts","isEquipable":false,"moraleEffect":0,"radClea…`
  - row 55: `{"contamination":0,"description":"Circuit boards, wiring and chips pulled from dead electronics, the EMP and the years having done the killing. A tenth of a kilo, six trade units, fifty to a stack. Some of it is worth nothing, and some of it is a radio waiting for a soldering iron and an afternoon. People sort it by hand, on a table, in the evenings. The gold pins still shine, and the rest is dust.","disassembleYieldFraction":0,"displayName":"Electronic Scrap","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"electronic_scrap","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"scra…`
  - row 56: `{"category":"equipment","description":"A salvaged radiosonde payload, recovered after atmospheric flight. Contains intact sensors and telemetry logs.","display_name":"Recovered Radiosonde Package","durability":0.85,"id":"item_radiosonde","tags":["electronics","scientific","recovered"],"value":14,"weight_kg":1.2}`
  - row 57: `{"contamination":0,"description":"A single solar cell, glass intact or cracked, frame bent but the wafer still blue. One point two kilos, twenty-two trade units, ten to a stack. Charged, it feeds a battery; broken, it is still the best glass around. The sun still does its part, every day, on schedule. It is the only utility left in the world that has not failed, and people treat it accordingly: first pick, first price, no haggling.","disassembleYieldFraction":0,"displayName":"Solar Cell","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"solar_cell","isEquipable":false,"moraleEffect":0,"radCleanse":0,"…`
  - row 58: `{"contamination":0.05,"description":"Containers of industrial chemicals: acids, solvents, powders in unlabeled jars. Two hundred fifty grams, five trade units, thirty to a stack, and handling them carries a contamination risk of one twentieth of a point. They clean metal, strip paint, set dyes and burn. The jars have no labels, because the labels washed off or were removed. You smell them before you read them.","disassembleYieldFraction":0,"displayName":"Chemicals","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"chemicals","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"scrapV…`
  - row 59: `{"contamination":0,"description":"A handheld radio with a rubber antenna and a cracked dial face. It receives the bands that still carry voices, static, and the occasional transmission from a settlement you cannot reach. Eight tenths of a kilo, worth twenty-two, and it only works if the batteries hold. People listen to it at night, in the dark, alone. The silence between broadcasts is the loudest thing in the bunker.","disassembleYieldFraction":0.5,"displayName":"Handheld Radio","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"handheld_radio","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radPr…`
  - row 60: `{"contamination":0,"description":"An engine block, complete enough to turn: pistons, head, and the wiring that used to be the harness. Twenty-five kilos, worth eighty, full durability when it is whole, and one is all a person carries. It powers a pump, a generator, a workshop belt, or a wagon already half-built in someone's yard. Engines are the heartbeat of the rebuild. People trade whole summers of salvage for one that turns over.","disassembleYieldFraction":0.5,"displayName":"Engine","durability":100,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"engine","isEquipable":false,"moraleEffect":0,"radCleanse":0,"rad…`
  - row 61: `{"contamination":0,"description":"Washed roots, pale and knobby, tied in a bundle. Eight points of hunger per serving, one trade unit, two tenths of a kilo, twenty to a stack. They boil soft in an hour and taste like nothing, which is fine, because nothing is what people can afford. Children are told the thin white ones are the sweet ones. No one argues with them.","displayName":"Roots","healthEffect":0,"hungerRestore":8.0,"id":"roots","isEquipable":false,"moraleEffect":0,"radCleanse":0,"stackMax":20,"thirstRestore":0,"tradeValue":1,"type":"Food","weight":0.2}`
  - row 62: `{"contamination":0,"description":"A handful of dark berries in a folded leaf, soft at the press of a thumb. Six points of hunger, one trade unit, twenty bundles to a stack. Foragers argue about which bushes are safe, and the argument has no referee. People pick in the middle of the day when the light is good, and they bring them home whole. Berries are a small meal and a large question.","displayName":"Berries","healthEffect":0,"hungerRestore":6.0,"id":"berries","isEquipable":false,"moraleEffect":0,"radCleanse":0,"stackMax":20,"thirstRestore":0,"tradeValue":1,"type":"Food","weight":0.15}`
  - row 63: `{"contamination":0,"description":"A glass vacuum tube with filigreed pins, still intact after decades on a shelf. It carries signals the way the old world carried conversations: through heated wire and careful vacuum. Worth eight, a tenth of a kilo, and five to a stack. Radios and gramophones both beg for them. The glass is fragile, and so is the signal.","displayName":"Vacuum Tube","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"vacuum_tube","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":8,"type":"Component","weight":0.1}`
  - row 64: `{"contamination":0,"description":"A coiled spring mechanism, still tensioned inside its housing. It stores energy the way a lung stores breath, and releases it the way a memory releases itself: all at once. Worth six, a fifth of a kilo, five to a stack. Gramophones, clocks, and old traps all rely on the same principle: steel that remembers how to push back.","displayName":"Spring Mechanism","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"spring_mechanism","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":6,"type":"Component","weight":0.…`
  - row 65: `{"contamination":0,"description":"A tiny sapphire needle, still mounted in its cartridge. It reads the grooves of a record the way a finger reads braille: by feeling the shape of something that was made to be heard. Worth four, nearly weightless, ten to a stack. The last ones came from shops that sold music by the disc. Now they are scavenged from the same discs they used to play.","displayName":"Phonograph Needle","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"phonograph_needle","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":4,"ty…`
  - row 66: `{"contamination":0,"description":"A high-wattage projector bulb, filament intact or merely resting. It throws light through a lens the way memory throws light through time: bright enough to see, not bright enough to stay. Worth twelve, a quarter kilo, three to a stack. Film projectors need one, and so do the people who still believe in screenings.","displayName":"Projector Bulb","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"projector_bulb","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":3,"thirstRestore":0,"tradeValue":12,"type":"Component","weight":0.25}`
  - row 67: `{"contamination":0,"description":"A small can of precision lubricant oil, the kind used in clockwork and camera shutters. It reduces friction the way patience reduces panic: slowly, and only when applied correctly. Worth three, a tenth of a kilo, twenty to a stack. Mechanics hoard it. Filmmakers beg for it. The label is gone, but the viscosity is still right.","displayName":"Lubricant Oil","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"lubricant_oil","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":3,"type":"Material","weight":0.1}`
  - row 68: `{"contamination":0,"description":"A metal film reel with a few meters of 8mm celluloid still wound tight. The images on it are someone's birthday, someone's parade, someone's last clear day. Worth fifteen, three tenths of a kilo, five to a stack. The projector needs it, and so does the memory. No one projects these for strangers. Some things are kept private even at the end of the world.","displayName":"Film Reel","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"film_reel","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":15,"type":"Comp…`
  - row 69: `{"contamination":0,"description":"A wound copper antenna coil, tinned and still conductive after years in a damp bunker. It catches signals the way a shoreline catches driftwood: whatever comes close enough to touch. Worth ten, two tenths of a kilo, ten to a stack. Radios need it, and so do the people who still listen for voices in the static.","displayName":"Antenna Coil","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"antenna_coil","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":10,"type":"Component","weight":0.2}`
  - row 70: `{"contamination":0,"description":"A small soldering kit with a coil of rosin-core solder, a tip cleaner, and a pencil iron that still heats when given a battery. It joins wire to wire and trace to trace, which is how the old world fixed anything that broke. Worth fourteen, four tenths of a kilo, five to a stack. Electronics do not stay repaired without it. Neither do radios, and neither do hope.","displayName":"Soldering Kit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"soldering_kit","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue"…`
  - row 71: `{"contamination":0,"description":"A brass music box comb with teeth still filed to pitch. It plucks the cylinder the way a fingernail plucks a thread: each tooth a note, each note a ghost. Worth nine, three tenths of a kilo, five to a stack. The mechanism is useless without it, and the melody is useless without the mechanism. Both are useless without someone to wind the key.","displayName":"Music Box Comb","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"music_box_comb","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":9,"type":"Componen…`
  - row 72: `{"contamination":0,"description":"A small winding key for a music box or clock mechanism, still fitted to its shaft. It stores torque the way a promise stores obligation: tight, and released all at once. Worth four, a tenth of a kilo, ten to a stack. Without it, the comb stays silent and the cylinder stays still. With it, even the oldest mechanism remembers its tune.","displayName":"Spring Key","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"spring_key","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":4,"type":"Component","weight":0.1}`
  - row 73: `{"contamination":0,"description":"A dried typewriter ribbon, ink still dark in the fabric but the strike surface gone to dust. It leaves no mark, which is the tragedy of all good tools worn past their last honest use. Worth three, nearly weightless, ten to a stack. A new ribbon changes everything. This one is a souvenir from the last person who had something to say.","displayName":"Typewriter Ribbon","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"typewriter_ribbon","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":3,"type":"Component"…`
  - row 74: `{"contamination":0,"description":"A small can of machine oil, the thin kind that runs into gears and bearings and makes them forget they ever seized. It stops rust the way a good day stops despair: temporarily, and only where it reaches. Worth two, a tenth of a kilo, twenty to a stack. Typewriters, lathes, and generators all ask for it. The can says industrial. The label is a lie. Everything here is industrial now.","displayName":"Machine Oil","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"machine_oil","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestor…`
  - row 75: `{"contamination":0,"description":"A small lens cleaning kit with a blower brush and a strip of microfiber cloth. It clears fog and dust from glass the way a clear thought clears confusion: slowly, and only when you are patient enough to use it. Worth five, a tenth of a kilo, ten to a stack. Cameras need it. So do the people who still believe there is something worth recording.","displayName":"Lens Cleaning Kit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"camera_lens_cleaner","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":5,"type…`
  - row 76: `{"contamination":0,"description":"A sealed canister of undeveloped 120 film, expiration date long past but the emulsion still potentially viable. It captures light the way a promise captures trust: briefly, and only if you act before it fades. Worth seven, a tenth of a kilo, ten to a stack. A camera without film is a box of good intentions. A film without a camera is a story no one will read.","displayName":"Photographic Film","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"photographic_film","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"trade…`
  - row 77: `{"contamination":0,"description":"A salvaged acoustic decoy module, still responsive to sound triggers. It emits a localized auditory signature that draws hostile attention away from the source. Fragile, improvised, and worth more in the right hands than the wrong. Ten trade units, nearly weightless, three to a stack.","displayName":"Acoustic Decoy","durability":30,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_acoustic_decoy","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":3,"thirstRestore":0,"tradeValue":10,"type":"Device","weight":0.2}`
  - row 78: `{"contamination":0,"description":"A fifty-kilo sack of fertilizer-grade ammonium nitrate, the kind that feeds fields and, under the wrong conditions, changes them. Sealed in a worn canvas sack with a printed lot number that predates the exchange. Worth eighteen, two kilos, five to a stack. Handling it requires the kind of respect that most people have forgotten.","displayName":"Ammonium Nitrate Sack","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_ammonium_nitrate_sack","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":18,"type":"M…`
  - row 79: `{"contamination":0,"description":"A chilled bottle of amnestic syrup, labeled in faded pharmacy script. It induces temporary memory suppression — a mercy in some cases, a liability in others. Worth twenty-two, three tenths of a kilo, five to a stack. The side effects are listed on a label that has mostly peeled away. Doctors used to warn against it. Now they measure doses by eye.","displayName":"Amnestic Syrup","durability":0,"empShielded":false,"equipSlot":"","healthEffect":-10,"hungerRestore":0,"id":"item_amnestic_syrup","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":22,"ty…`
  - row 80: `{"contamination":0,"description":"A sheaf of handwritten notes tied with twine, detailing fixed coordinates, shelter layouts, and cached supply points. The handwriting is steady, the ink faded, the information older than the writer. Worth fourteen, nearly weightless, ten to a stack. They are the difference between walking in circles and walking with purpose.","displayName":"Anchor Notes","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_anchor_notes","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":14,"type":"Document","weight":0.0…`
  - row 81: `{"contamination":0,"degradeRate":0.5,"description":"A salvaged ghillie wrap woven from ash-colored fabric strips and frayed cord. It breaks up a silhouette the way a lie breaks up a confrontation: only if you are patient enough to apply it right. Worth eleven, a kilo and a half, five to a stack. Wasteland scouts and the cautious both treat it as essential. The ash stays in the weave long after you take it off.","displayName":"Ash Ghillie Wrap","durability":40,"empShielded":false,"equipSlot":"Body","healthEffect":0,"hungerRestore":0,"id":"item_ash_ghillie","isEquipable":true,"moraleEffect":0,"radCleanse":0,"radProtection":5,"stackMax":5,"thir…`
  - row 82: `{"contamination":0,"description":"A flexible sheet of mycelium-based bioplastic, grown in a darkroom and cured under pressure. It seals tanks, patches suits, and lines containers the way patience seals wounds: imperfectly, but well enough to hold. Worth sixteen, four tenths of a kilo, ten to a stack. The old world called it experimental. The new world calls it useful.","displayName":"Bio-Plastic Sheet","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_bio_plastic","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":16,"type":"Material…`
  - row 83: `{"contamination":0,"description":"A sealed glass vial of ultra-filtered black water, drawn from a deep aquifer and run through three stages of charcoal and pressure. It restores thirst without the usual contamination tax, which makes it worth trading for. Worth nine, a tenth of a kilo, ten to a stack. The water is so clear it looks like nothing. That is the point.","displayName":"Black Water Vial","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_black_water_vial","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":35,"tradeValue":9,"type":"Water","…`
  - row 84: `{"contamination":0,"description":"A cylindrical CO2 scrubber cartridge filled with activated charcoal and soda lime. It strips carbon dioxide from recirculated air the way a deadline strips hesitation: efficiently, and with an expiration date nobody reads. Worth thirteen, a quarter kilo, five to a stack. Rebreathers and sealed shelters both depend on it. Breathing does not stop when the world does.","displayName":"CO2 Scrubber Cartridge","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_co2_scrubber_cartridge","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thi…`
  - row 85: `{"contamination":0,"description":"A dual-cartridge epoxy injector with a static mixer tip. It bonds metal to metal, ceramic to ceramic, and hope to desperation in under five minutes. Worth eleven, three tenths of a kilo, eight to a stack. The resin cures fast and holds longer than the people who mixed it. Surgeons and mechanics both keep one in their kit.","displayName":"Epoxy Injector","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_epoxy_injector","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":8,"thirstRestore":0,"tradeValue":11,"type":"Tool","weight":0.3}`
  - row 86: `{"contamination":0,"description":"A roll of woven copper Faraday mesh, fine enough to wrap a circuit and thick enough to stop a pulse. It shields electronics the way a locked door shields a room: only if the seal is complete. Worth sixteen, a third of a kilo, five to a stack. EMP survival is not about hardening every device. It is about having one clean room left.","displayName":"Faraday Mesh Roll","durability":0,"empShielded":true,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_faraday_mesh","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":16,"type":"Material","we…`
  - row 87: `{"contamination":0,"description":"A tin of medicated frostbite salve with a sharp camphor smell. It restores circulation and reduces tissue damage when applied early, which is the only time it works. Worth seven, a tenth of a kilo, ten to a stack. People who have lost fingers to the cold keep one in their pocket and check it every morning. Prevention is not a guarantee. It is just a better chance.","displayName":"Frostbite Salve","durability":0,"empShielded":false,"equipSlot":"","healthEffect":20,"hungerRestore":0,"id":"item_frostbite_salve","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0…`
  - row 88: `{"contamination":0,"description":"A pressurized fungicide fogger with a replaceable cartridge. It clears mold from sealed rooms and fungal growth from ventilation shafts the way a whistle clears a room: loudly, and with mixed results. Worth fifteen, four tenths of a kilo, five to a stack. Bunkers that run out of these run out of breathable air faster. Fungus does not negotiate.","displayName":"Fungicide Fogger","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_fungicide_fogger","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":15,"ty…`
  - row 89: `{"contamination":0,"description":"A length of hot-dip galvanized rebar, still coated and still straight. It reinforces concrete the way principles reinforce decisions: visibly, and only if you pour before it sets. Worth eight, three kilos, ten to a stack. Construction crews, bunker crews, and the stubborn all ask for the same thing: something that does not bend.","displayName":"Galvanized Rebar","durability":100,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_galvanized_rebar","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"type":"Material"…`
  - row 90: `{"contamination":0,"description":"A sealed one-litre canister of ethylene glycol antifreeze. It prevents freezing in engines and heat exchangers the way morale prevents collapse in a long winter: chemically, and not for everyone. Worth five, a kilo, five to a stack. Engines, shelters, and the desperate all need it. The label says automotive. The use is broader now.","displayName":"Glycol Antifreeze Canister","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_glycol_antifreeze_canister","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue"…`
  - row 91: `{"contamination":0,"description":"A custom-cut silicone gasket for a bunker hermetic hatch. It seals against pressure, fallout dust, and the slow creep of air that should not be moving. Worth twenty-one, a quarter kilo, five to a stack. Bunkers that skip this do not stay sealed. The difference between a shelter and a sealed room is a strip of rubber someone measured twice.","displayName":"Hermetic Hatch Gasket","durability":60,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_hermetic_hatch_silicone_gasket","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tra…`
  - row 92: `{"contamination":0,"description":"A curved high-tensile steel brace salvaged from a collapsed culvert section. It spans gaps the way a decision spans consequences: with structural integrity, and only if the load is calculated. Worth nineteen, five kilos, five to a stack. Engineers, barricaders, and the hopeful all recognize the same shape: something that was built to hold back the world.","displayName":"Steel Culvert Brace","durability":100,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_high_tensile_steel_culvert_brace","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirs…`
  - row 93: `{"contamination":0,"description":"A heavy insulated battery from a snowmobile engine block, still holding a charge through the cold that killed the machine it came from. It powers heaters, radios, and the small comforts people refuse to give up. Worth sixteen, three kilos, five to a stack. Cold is the oldest enemy. Batteries are the oldest ally.","displayName":"Insulated Snowmobile Battery","durability":60,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_insulated_snowmobile_battery","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":16,"type":"Dev…`
  - row 94: `{"contamination":0,"degradeRate":0.5,"description":"A small lead-shielded cask for transporting radioactive samples. It protects the handler the way a secret protects the guilty: completely, and with moral weight nobody discusses. Worth seventeen, two kilos, three to a stack. Scientists, scavengers, and the foolish all open them eventually. The lead is not there to keep the outside out. It is there to keep the inside in.","displayName":"Lead-Shielded Sample Cask","durability":80,"empShielded":true,"equipSlot":"Body","healthEffect":0,"hungerRestore":0,"id":"item_lead_shielded_sample_cask","isEquipable":true,"moraleEffect":0,"radCleanse":0,"ra…`
  - row 95: `{"contamination":0,"degradeRate":1.0,"description":"A heavy lead-glass visor mounted in a leather head harness. It protects the eyes and face from radiant heat and flash, the way sunglasses protect the eyes from ordinary light: only this kind can blind you permanently. Worth nineteen, eight tenths of a kilo, five to a stack. Welders, radiomen, and the curious all wear them. The glass is clouded. The protection is not.","displayName":"Lead Visor","durability":70,"empShielded":false,"equipSlot":"Face","healthEffect":0,"hungerRestore":0,"id":"item_lead_visor","isEquipable":true,"moraleEffect":0,"radCleanse":0,"radProtection":40,"stackMax":5,"th…`
  - row 96: `{"contamination":0,"description":"A sealed pouch of lithium carbonate salts, the psychiatric staple that became a wasteland trade good. It stabilizes mood the way a fixed schedule stabilizes a day: imperfectly, but enough to function. Worth twenty-four, a tenth of a kilo, ten to a stack. Demand is constant. Supply is not. The people who need it most are the people who can least afford to run out.","displayName":"Lithium Salts","durability":0,"empShielded":false,"equipSlot":"","healthEffect":10,"hungerRestore":0,"id":"item_lithium_salts","isEquipable":false,"moraleEffect":5,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tra…`
  - row 97: `{"contamination":0,"description":"A bundle of insulated copper mine prods, the kind used to test electrical continuity in dangerous circuits. They save lives the way a second opinion saves a diagnosis: by confirming what should not be assumed. Worth seven, a fifth of a kilo, ten to a stack. Electricians, deminers, and the cautious test everything. The prod is the extension of that instinct.","displayName":"Mine Prods","durability":50,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_mine_prod","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":7,"t…`
  - row 98: `{"contamination":0,"description":"Compressed bricks of cultivated mycelium bound with agricultural waste. They insulate, they dampen sound, and they grow if you leave them in the dark too long. Worth thirteen, three kilos, ten to a stack. The old world called it sustainable. The new world calls it available. Either way, they build walls that breathe.","displayName":"Mycelium Bricks","durability":40,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_mycelium_bricks","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":5,"stackMax":10,"thirstRestore":0,"tradeValue":13,"type":"Material","weight":3.0}`
  - row 99: `{"contamination":0,"description":"A bottle of Prussian blue chelating pellets, the cesium and thallium binder that turns internal contamination into something the body can pass. Worth twenty-six, a tenth of a kilo, five to a stack. It does not fix everything. It fixes the specific poisons that certain fallout isotopes leave behind, which is enough to make it worth its weight in clean water.","displayName":"Prussian Blue Chelating Pellets","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_prussian_blue_chelating_pellets","isEquipable":false,"moraleEffect":0,"radCleanse":40,"radProtection":0,"stack…`
  - row 100: `{"contamination":0,"description":"A passive radon detector electret chamber, small enough to carry and slow enough to trust. It accumulates charge the way a bunker accumulates secrets: over time, and only if left undisturbed. Worth fifteen, three tenths of a kilo, five to a stack. Geiger counters catch gamma. This catches the gas that seeps through concrete and accumulates in the dark.","displayName":"Radon Detector Electret","durability":80,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_radon_detector_electret","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore…`
  - row 101: `{"contamination":0,"description":"A compact rebreather scrubber pack with replaceable CO2 and moisture cartridges. It recycles exhaled air the way a library recycles stories: by filtering out the parts that are dangerous to repeat. Worth eighteen, four tenths of a kilo, five to a stack. Extended operations depend on it. So does the discipline to check the gauge before leaving the airlock.","displayName":"Rebreather Scrubber Pack","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_rebreather_scrubber","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore"…`
  - row 102: `{"contamination":0,"description":"A thin-film reverse osmosis membrane sheet, rated for brackish and lightly contaminated water. It turns undrinkable water into drinkable water the way discipline turns chaos into routine: slowly, with waste, and only if the pressure holds. Worth twelve, a tenth of a kilo, ten to a stack. Water filters depend on it. So do the people who refuse to drink from the puddle.","displayName":"Reverse Osmosis Membrane","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_ro_membrane","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRe…`
  - row 103: `{"contamination":0,"description":"A dried bundle of scopolamine-bearing root, harvested from a plant that thrives in disturbed soil. It suppresses memory and nausea, which makes it useful for trauma and for travel. Worth twenty, nearly weightless, ten to a stack. The dose is critical. Too little does nothing. Too much erases the wrong things. Healers used to call it the truth serum. Survivors call it the forgetting herb.","displayName":"Scopolamine Root","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_scopolamine_root","isEquipable":false,"moraleEffect":-2,"radCleanse":0,"radProtection":0,"stac…`
  - row 104: `{"contamination":0,"degradeRate":0.5,"description":"A sealed lead container for transporting radioactive sources. It is heavy, warm to the touch, and marked with a trefoil that nobody alive today remembers being taught to fear. Worth fifteen, three kilos, three to a stack. The radiation inside is measured in sieverts. The respect it demands is measured in distance and time.","displayName":"Sealed Lead Pig","durability":100,"empShielded":true,"equipSlot":"Body","healthEffect":0,"hungerRestore":0,"id":"item_sealed_lead_pig","isEquipable":true,"moraleEffect":0,"radCleanse":0,"radProtection":100,"stackMax":3,"thirstRestore":0,"tradeValue":15,"ty…`
  - row 105: `{"contamination":0,"description":"Goggles carved from scrap leather and fitted with slotted wood or bone. They prevent snow blindness the way a shelter prevents hypothermia: imperfectly, but with enough discipline to make the difference. Worth three, two tenths of a kilo, ten to a stack. The old world called them Inuit goggles. The new world calls them scavenged.","displayName":"Improvised Snow Goggles","durability":20,"empShielded":false,"equipSlot":"Face","healthEffect":0,"hungerRestore":0,"id":"item_snow_goggles_improvised","isEquipable":true,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":3,…`
  - row 106: `{"contamination":0,"description":"Folded panels of acoustic foam and compressed fibreglass, salvaged from recording studios and server rooms. They deaden sound the way a closed mouth deadens conflict: partially, and only if the seal is honest. Worth nine, a kilo, five to a stack. In a bunker, noise carries fear. Silence carries control.","displayName":"Sound Baffling Panels","durability":30,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_sound_baffling","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":9,"type":"Material","weight":1.0}`
  - row 107: `{"contamination":0,"description":"A hard-shell suitcase with a combination dial still set to factory default. It rattles when shaken, which means something solid is inside, and it smells faintly of old tobacco and camphor. Worth seventeen, two kilos, five to a stack. People leave them in bunker corners for years, then open them one morning and find a life packed by someone who never came home.","displayName":"Locked Suitcase","durability":50,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_suitcase_locked","isEquipable":false,"moraleEffect":2,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tra…`
  - row 108: `{"contamination":0,"description":"A stainless steel bone chisel with a sterilized handle and a blade that still holds an edge. It removes bone the way a decision removes doubt: precisely, and with finality. Worth twenty-four, two tenths of a kilo, five to a stack. Surgeons in field conditions use it. So do the desperate, when the alternative is a slow death.","displayName":"Surgical Bone Chisel","durability":80,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_surgical_bone_chisel","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":24,"type":"Medica…`
  - row 109: `{"contamination":0,"description":"A worn teddy bear with one button eye and a fur matted by ash and time. It restores three points of morale simply by being present, which is more than most things in the bunker manage. Worth eight, two tenths of a kilo, ten to a stack. Children claim them. Adults keep them in pockets and say nothing. The bear does not judge the hand that holds it.","displayName":"Teddy Bear","durability":30,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_teddy_bear","isEquipable":false,"moraleEffect":3,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"type":"Tra…`
  - row 110: `{"contamination":0,"description":"A syringe of ceramic thermal paste, the kind used between heat spreaders and processors. It bridges microscopic gaps the way diplomacy bridges ideological ones: thinly, evenly, and with the understanding that both sides are generating heat. Worth five, a tenth of a kilo, ten to a stack. Electronics overheat without it. So do arguments.","displayName":"Thermal Paste","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_thermal_paste","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":5,"type":"Material",…`
  - row 111: `{"contamination":0,"description":"A set of darkened welding glass plates in a steel frame. They filter the arc the way a bunker filters fallout: by blocking the part that does permanent damage. Worth thirteen, a quarter kilo, five to a stack. Welders, mechanics, and the cautious all respect the same brightness threshold. Looking directly at the work is not a test of courage. It is a test of foolishness.","displayName":"Welder's Glass","durability":70,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_welders_glass","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore"…`
  - row 112: `{"contamination":0,"description":"A pair of alkaline AA batteries, still holding a faint charge. They power flashlights, radios, and the small devices people refuse to let go of. Worth three, a tenth of a kilo, twenty to a stack. Every battery in the bunker has a job. These are the ones that run the flashlight nobody turns off.","displayName":"AA Batteries","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"aa_batteries","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":3,"type":"Material","weight":0.1}`
  - row 113: `{"contamination":0,"description":"A sealed box of ten isopropyl alcohol wipes. They sterilize surfaces and skin the way silence sterilizes a room: quickly, and only where applied. Worth four, two tenths of a kilo, ten to a stack. Medics, mechanics, and the cautious keep them close. The seal is intact. That is the first thing to check.","displayName":"Alcohol Wipes (Box of 10)","durability":0,"empShielded":false,"equipSlot":"","healthEffect":5,"hungerRestore":0,"id":"alcohol_wipes_box_10_of_10","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":4,"type":"Medical","weight":0.2}`
  - row 114: `{"contamination":0,"description":"A handful of 7.62x54R jacketed hollow-point armour-piercing rounds. They punch through cover and expand in tissue the way a bad decision punches through a truce: with consequences nobody wanted. Worth eleven, two tenths of a kilo, twenty to a stack. Hunters, defenders, and the desperate treat them as currency. The brass is polished. The lethality is not.","displayName":"7.62x54R JHP-AP Rounds","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_762x54r_jhp_ap","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tra…`
  - row 115: `{"contamination":0,"description":"A box of .357 revolver rounds, brass casings, lead bullets. Feeds jury-rigged pipe rifles and revolvers. The box is dented. The rounds are clean. The ammunition works.","displayName":".357 Rounds","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_357","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":6,"type":"Ammo","weight":0.2}`
  - row 116: `{"contamination":0,"description":"A box of 12-gauge shells, plastic hulls, lead shot. Feeds scrap shotguns. The box is dented. The shells are clean. The ammunition works.","displayName":"12-Gauge Shells","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_12g","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":7,"type":"Ammo","weight":0.25}`
  - row 117: `{"contamination":0,"description":"A box of .308 Winchester ammunition, brass casings, copper bullets. Feeds held-bolt rifles. The box is dented. The rounds are clean. The ammunition works.","displayName":".308 Rounds","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_308","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":9,"type":"Ammo","weight":0.2}`
  - row 118: `{"contamination":0,"description":"A box of 5.56mm ammunition, brass casings, copper bullets. Feeds assault rifles. The box is dented. The rounds are clean. The ammunition works.","displayName":"5.56mm Rounds","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_556","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":11,"type":"Ammo","weight":0.2}`
  - row 119: `{"contamination":0,"description":"A box of 7.62mm ammunition, brass casings, copper bullets. Feeds light machine guns. The box is dented. The rounds are clean. The ammunition works.","displayName":"7.62mm Rounds","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"ammo_762","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":12,"type":"Ammo","weight":0.2}`
  - row 120: `{"contamination":0,"description":"A one-litre bottle of surgical-grade antiseptic solution. It cleans wounds and surfaces the way a verdict cleans a court: decisively, and not always gently. Worth seven, a kilo, five to a stack. The label says hospital use. The use is broader now. Every surface in a bunker is a wound waiting to happen.","displayName":"Antiseptic (1L)","durability":0,"empShielded":false,"equipSlot":"","healthEffect":10,"hungerRestore":0,"id":"antiseptic_1l_of_1l","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":7,"type":"Medical","weight":1.0}`
  - row 121: `{"contamination":0,"description":"A sealed battery pack, the kind that powered tools and emergency lighting before the exchange. It stores energy the way a promise stores obligation: visibly, and with an expiration date nobody reads. Worth eight, a kilo, ten to a stack. Radios, heaters, and the long nights all depend on it. People hoard them the way they hoard daylight.","displayName":"Battery Pack","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"battery_pack","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"type":"Device","weight"…`
  - row 122: `{"contamination":0,"description":"A small carton of ten steel nails, galvanized and straight. They hold wood the way a contract holds people: only if both sides are honest and the surface is prepared. Worth two, a tenth of a kilo, twenty to a stack. Carpenters, barricaders, and the desperate all reach for the same thing when the structure fails.","displayName":"Box of Nails (10)","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"box_of_nails_10","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":2,"type":"Material","weight":0.1}`
  - row 123: `{"contamination":0,"description":"A can of condensed soup, label faded but seal intact. It restores thirty points of hunger and one point of morale, because warmth is not just temperature. Worth eight, three tenths of a kilo, ten to a stack. The best ones taste like childhood. The worst ones taste like metal. Nobody asks which is which.","displayName":"Canned Soup","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":30,"id":"canned_soup","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"type":"Food","weight":0.3}`
  - row 124: `{"contamination":0,"description":"A bundle of children's picture books, pages intact but covers softened by damp. They teach reading the way a bunker teaches patience: one letter, one day, one survival at a time. Worth five, three tenths of a kilo, five to a stack. Teachers, parents, and the hopeful all keep a few. The words are simple. The context is not.","displayName":"Children's Books","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"childrens_books","isEquipable":false,"moraleEffect":2,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":5,"type":"Document","weight":0.3}`
  - row 125: `{"contamination":0,"description":"A brass pocket lighter, still filled and still sparking. It creates fire the way a speech creates momentum: out of nothing, and only if the conditions are right. Worth six, two tenths of a kilo, ten to a stack. Smokers, mechanics, and the cold all demand the same thing: a controlled spark in an uncontrolled world.","displayName":"Cigarette Lighter","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"cigarette_lighter","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":6,"type":"Tool","weight":0.2}`
  - row 126: `{"contamination":0,"description":"A one-litre jug of filtered clean water, sealed with a screw cap. It restores thirst without the contamination tax, which is the only tax people refuse to pay voluntarily. Worth twelve, a kilo, five to a stack. Water this clean is the measure of a settlement. The jug says more about the place that filled it than the place that sold it.","displayName":"Clean Water Jug","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"clean_water_jug","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":40,"tradeValue":12,"type":"Water","we…`
  - row 127: `{"contamination":0,"description":"A bottle of vegetable cooking oil, yellow and clear. It calms hunger the way diplomacy calms borders: by making everything more slippery and less direct. Worth four, three tenths of a kilo, ten to a stack. Cooks hoard it. Survivors trade for it. The label says food grade. The use is survival grade.","displayName":"Cooking Oil","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":10,"id":"cooking_oil","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":4,"type":"Food","weight":0.3}`
  - row 128: `{"contamination":0,"description":"A coil of ten metres of solid-core copper wire, insulated and still bright. It carries current the way a road carries traffic: only if the path is clear and the connection is honest. Worth eight, three tenths of a kilo, ten to a stack. Electricians, tinkerers, and the hopeful all measure twice before cutting. Copper does not forgive mistakes.","displayName":"Copper Wire (10m)","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"copper_wire_10m_of_10m","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"ty…`
  - row 129: `{"contamination":0,"description":"A can of diesel fuel, the smell unchanged since the last time a truck engine turned over. It powers generators, heaters, and the slow hope that something might still move. Worth ten, two kilos, five to a stack. Fuel is the measure of winter. The can says litres. The use is survival.","displayName":"Diesel Fuel","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"diesel_fuel","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":10,"type":"Fuel","weight":2.0}`
  - row 130: `{"contamination":0,"description":"A pack of dried meat and grain biscuits, vacuum-sealed and still edible. It restores twenty points of hunger and nothing else, which is exactly what a ration is supposed to do. Worth five, two tenths of a kilo, twenty to a stack. Soldiers, scavengers, and the disciplined eat these first and complain later. Flavour is a luxury. Calories are a promise.","displayName":"Dried Rations","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":20,"id":"dried_rations","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":5,"type":…`
  - row 131: `{"contamination":0,"description":"A roll-up Faraday pack with conductive mesh lining and a magnetic seal. It shields electronics from EMP the way a basement shields people from blast: imperfectly, but better than nothing. Worth fourteen, four tenths of a kilo, five to a stack. The EMP did not end electronics. The lack of shielding did. This is the correction.","displayName":"Faraday Pack","durability":40,"empShielded":true,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"faraday_pack","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":14,"type":"Container","weight":0.4}`
  - row 132: `{"contamination":0,"description":"A compact field surgical kit with scalpels, sutures, and a tourniquet. It closes wounds the way a treaty closes conflict: under pressure, with limited resources, and with the understanding that scarring is inevitable. Worth nineteen, a kilo, five to a stack. Surgeons in the field work with what they carry. This is what they carry.","displayName":"Field Surgical Kit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":60,"hungerRestore":0,"id":"field_surgical_kit","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":19,"type":"Medical",…`
  - row 133: `{"contamination":0,"description":"A sealed one-litre can of fuel, diesel or kerosene, the kind that runs engines and stoves and keeps the dark at bay. Worth six, a kilo, five to a stack. Fuel is measured in litres but traded in survival. A litre is enough to heat a room for an evening or move a vehicle a short distance. Both are victories.","displayName":"Fuel (1L)","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"fuel_1l","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":6,"type":"Fuel","weight":1.0}`
  - row 134: `{"contamination":0,"description":"A compact hydrogen fuel cell, still pressurised and still delivering current. It powers sensors, radios, and life support the way a savings account powers a retirement: slowly, and only if you did not touch it. Worth thirteen, three tenths of a kilo, five to a stack. The technology outlasted the supply chain. That is why people carry them.","displayName":"Fuel Cell","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"fuel_cell","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":13,"type":"Device","weight":0.…`
  - row 135: `{"contamination":0,"description":"A water-damaged growing manual with soil charts and planting calendars. It turns dirt into food the way a teacher turns ignorance into skill: with patience, repetition, and the willingness to fail publicly. Worth eight, three tenths of a kilo, five to a stack. Farmers, gardeners, and the hungry all recognise the same thing: information that survives is information worth trading for.","displayName":"Growing Manual","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"growing_manual","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":5,"thirst…`
  - row 136: `{"contamination":0,"description":"A small bottle of potassium iodide tablets, the thyroid-blocking staple of fallout preparedness. They are bitter, cheap, and worth more than gold when the siren sounds. Worth five, two tenths of a kilo, ten to a stack. Pharmacies used to hand them out for free. Now they are traded like ammo. The taste never got better.","displayName":"Iodine Tablets","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"iodine_tablets","isEquipable":false,"moraleEffect":0,"radCleanse":20,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":5,"type":"Medical","weight":0.2}`
  - row 137: `{"contamination":0,"description":"A cassette tape with a handwritten label. The recording on it is someone's voice, telling a story that may or may not be true. Worth nine, a tenth of a kilo, ten to a stack. Recordings outlive the people who made them. That is both the comfort and the curse of magnetic tape.","displayName":"Cassette Tape","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_cassette_tape","isEquipable":false,"moraleEffect":3,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":9,"type":"Media","weight":0.1}`
  - row 138: `{"contamination":0,"description":"A leather-bound photo album filled with pre-war family photographs. The faces are strangers, the places are gone, and the captions are in handwriting nobody reads anymore. Worth thirteen, three tenths of a kilo, five to a stack. Looking through it is an act of time travel. Closing it is an act of survival.","displayName":"Pre-War Photo Album","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_pre_war_photo_album","isEquipable":false,"moraleEffect":4,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":13,"type":"Document","weight":0.3}`
  - row 139: `{"contamination":0,"description":"A crate of vinyl records, sleeves worn but discs intact. They spin at 33 rpm and carry music that predates the exchange by decades. Worth sixteen, two kilos, five to a stack. Gramophones are rare. Records are not. The mismatch is the tragedy and the trade.","displayName":"Vinyl Record Collection","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"item_vinyl_collection","isEquipable":false,"moraleEffect":5,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":16,"type":"Media","weight":2.0}`
  - row 140: `{"contamination":0,"description":"An assortment of gears, cams, and bearings scavenged from dead machinery. They are the vocabulary of repair, and without them nothing mechanical says anything intelligible. Worth seven, a quarter kilo, twenty to a stack. Mechanics sort them by shape and sound. The rest of us sort them by hope.","displayName":"Mechanical Components","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"mechanical_components","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":7,"type":"Material","weight":0.25}`
  - row 141: `{"contamination":0,"description":"A canvas medical kit with bandages, antiseptic, and basic surgical tools. It stabilises the injured the way a truce stabilises a war: temporarily, and only if both sides respect the terms. Worth eleven, four tenths of a kilo, five to a stack. Bunkers that run out of these start measuring losses differently.","displayName":"Medkit","durability":0,"empShielded":false,"equipSlot":"","healthEffect":40,"hungerRestore":0,"id":"medkit","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":11,"type":"Medical","weight":0.4}`
  - row 142: `{"contamination":0,"description":"A length of steel pipe, threaded at one end and rusted at the other. It moves water, gas, and ideas through confined spaces the way a messenger moves through hostile territory: quickly, and with risk. Worth three, a kilo, twenty to a stack. Plumbers, welders, and the desperate all recognise the same shape: something hollow that can carry pressure.","displayName":"Metal Pipe","durability":60,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"metal_pipe","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":3,"type":"Material…`
  - row 143: `{"contamination":0,"description":"A steel-frame hatchet with a polymer handle and a blade balanced for throwing or chopping. It splits wood the way a verdict splits a room: with finality, and with attention to who is holding it. Worth twelve, a kilo, five to a stack. Soldiers, woodsmen, and the desperate all sharpen it the same way: with respect.","displayName":"Military-Grade Hatchet","durability":80,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"military_grade_hatchet","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":12,"type":"Tool","weight":1.0}`
  - row 144: `{"contamination":0,"description":"A pre-war military Meal, Ready-to-Eat. The pouch is swollen at one corner, which means the contents are still safe, and the heater works if you have a match. Worth seven, five tenths of a kilo, ten to a stack. Soldiers ate these. Survivors trade for them. The flavour is not the point. The calories are.","displayName":"Military MRE","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":45,"id":"military_mre","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":7,"type":"Food","weight":0.5}`
  - row 145: `{"contamination":0,"description":"A military-specification radio set with encryption modules and a frequency range that still includes the bands that matter. It receives orders, weather, and the occasional voice that sounds like authority. Worth twenty-two, three kilos, five to a stack. The encryption is useless without a key. The listening is not.","displayName":"Military Radio","durability":0,"empShielded":true,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"military_radio","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":22,"type":"Device","weight":3.0}`
  - row 146: `{"contamination":0,"description":"A pack of military-issue ration bars, dense and tasteless and reliable. They restore forty points of hunger and zero points of joy, which is exactly what a survival ration is designed to do. Worth six, three tenths of a kilo, twenty to a stack. Soldiers, scouts, and the practical eat these first and argue about flavour later.","displayName":"Military Rations","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":40,"id":"military_rations","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":6,"type":"Food","weight":0.3}`
  - row 147: `{"contamination":0,"description":"A wooden military supply crate with stencilled markings and a lid that still seals. Inside is the kind of inventory that makes a bunker feel like a fortress: ammo, rations, medical supplies, and the quiet confidence of logistics. Worth twenty-five, five kilos, three to a stack. Crates like this are the reason people dig bunkers in the first place.","displayName":"Military Supply Crate","durability":80,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"military_supply_crate","isEquipable":false,"moraleEffect":2,"radCleanse":0,"radProtection":0,"stackMax":3,"thirstRestore":0,"tradeValu…`
  - row 148: `{"contamination":0,"description":"A replacement cylinder for a music box, programmed with the opening bars of Fur Elise. It plays the same melody every time, which is either comfort or curse depending on the day. Worth seven, a tenth of a kilo, ten to a stack. Music boxes are one of the few machines that do exactly what they were built to do. That is why people keep winding them.","displayName":"Fur Elise Music Box Cylinder","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"music_box_fur_elise","isEquipable":false,"moraleEffect":3,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"trade…`
  - row 149: `{"contamination":0,"description":"A generation-one night vision scope with a damaged IR illuminator and a lens that still gains in the dark. It turns night into grey the way optimism turns despair into strategy: imperfectly, but enough to act. Worth eighteen, a quarter kilo, five to a stack. Scouts, sentries, and the nocturnal all recognise the same advantage: seeing before being seen.","displayName":"Night Vision Scope","durability":0,"empShielded":true,"equipSlot":"Face","healthEffect":0,"hungerRestore":0,"id":"night_vision_scope","isEquipable":true,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValu…`
  - row 150: `{"contamination":0,"description":"A sheet of industrial-grade plastic sheeting, the kind used for vapour barriers and temporary shelters. It keeps moisture out the way a lie keeps the truth out: completely, and only if the edges are sealed. Worth four, three tenths of a kilo, twenty to a stack. Builders, farmers, and the damp all know the same rule: plastic is the difference between a shelter and a cave.","displayName":"Plastic Material Sheet","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"plastic_material","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstR…`
  - row 151: `{"contamination":0,"description":"Torn sheeting, cracked containers, bottle shards — the shelter sheds plastic the way it sheds heat. Sorted and baled, it is feedstock for the retort. Worth one, a fifth of a kilo, fifty to a stack.","displayName":"Waste Plastic Scrap","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"scrap_plastic","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":50,"thirstRestore":0,"tradeValue":1,"type":"Material","weight":0.2}`
  - row 152: `{"contamination":0,"description":"A dented can of reclamation fuel rendered from waste plastic in the back-draft retort. It burns dirty and runs engines rough — expect more wear, fewer kilometres to the can. Worth eight, four kilos, ten to a stack.","displayName":"Retort Synthetic Fuel","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"synthetic_fuel_canister","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeValue":8,"type":"Fuel","weight":4.0}`
  - row 153: `{"contamination":0,"description":"Fine soot pressed from the retort's draft chamber. Seals gaskets, cuts rubber compound, recharge respirator inserts. Breathing it is its own small emergency. Worth three, half a kilo, twenty to a stack.","displayName":"Carbon Black Powder","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"carbon_black_powder","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":3,"type":"Material","weight":0.5}`
  - row 154: `{"contamination":0,"degradeRate":0.5,"description":"A small padded coat with a detachable hood, sized for a child. It provides insulation and a modicum of rad protection the way a promise provides safety: only if the adult keeps it. Worth nine, four tenths of a kilo, five to a stack. Parents trade for it. Survivors keep it. The size never changes. The need does.","displayName":"Child's Protective Coat","durability":50,"empShielded":false,"equipSlot":"Body","healthEffect":0,"hungerRestore":0,"id":"protective_childs_coat","isEquipable":true,"moraleEffect":2,"radCleanse":0,"radProtection":15,"stackMax":5,"thirstRestore":0,"tradeValue":9,"type":…`
  - row 155: `{"contamination":0,"description":"A length of reinforced rubber hose, still flexible and still capable of carrying water or air. It connects systems the way a mediator connects people: by finding a path through resistance. Worth three, two tenths of a kilo, twenty to a stack. Plumbers, mechanics, and the inventive all carry a length. A hose is never the hero. It is the thing that makes the hero possible.","displayName":"Rubber Hose","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"rubber_hose","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"trade…`
  - row 156: `{"contamination":0,"description":"A bundle of salvaged wood planks and boards, warped but still structural. It builds shelves, beds, and the small walls that make a bunker feel like a home. Worth two, a kilo, twenty to a stack. Carpenters call it lumber. Survivors call it possibility. The difference is only in the cutting.","displayName":"Scrap Wood","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"scrap_wood","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":2,"type":"Material","weight":1.0}`
  - row 157: `{"contamination":0,"description":"A manila envelope marked RESTRICTED in faded ink, sealed with wax that cracked long ago. The contents are bureaucratic and obsolete, but bureaucracy once ran the world, and its remnants still carry weight. Worth eleven, two tenths of a kilo, five to a stack. Lawyers, clerks, and the nostalgic all recognise the same thing: authority that outlives its author.","displayName":"Sealed Government Document","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"sealed_government_document","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRe…`
  - row 158: `{"contamination":0,"description":"A envelope of assorted vegetable seeds, some dated, some anonymous. They grow food the way a decision grows consequences: slowly, and only if the soil is honest. Worth six, a tenth of a kilo, twenty to a stack. Farmers, gardeners, and the hopeful all treat seeds as futures. Some of them are. Most of them are not.","displayName":"Seed Packets","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"seed_packets","isEquipable":false,"moraleEffect":1,"radCleanse":0,"radProtection":0,"stackMax":20,"thirstRestore":0,"tradeValue":6,"type":"Material","weight":0.1}`
  - row 159: `{"contamination":0,"description":"A bottle of industrial-grade spirits, the kind used for cleaning, sterilising, and forgetting. It burns the throat and clears the mind the way a confrontation clears the air: painfully, and only for a moment. Worth nine, a kilo, five to a stack. Drinkers, medics, and the mournful all recognise the same bottle. The label says medical. The use is broader.","displayName":"Spirits","durability":0,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"spirits","isEquipable":false,"moraleEffect":-1,"radCleanse":0,"radProtection":0,"stackMax":5,"thirstRestore":0,"tradeValue":9,"type":"Material"…`
  - row 160: `{"contamination":0,"description":"A length of steel reinforcing bar, rusted at the cut end and still straight. It reinforces concrete the way a conviction reinforces a person: visibly, and only if poured while the moment is still hot. Worth three, three kilos, ten to a stack. Builders, fortifiers, and the stubborn all recognise the same shape: something that does not bend because it was not asked to.","displayName":"Steel Rebar","durability":100,"empShielded":false,"equipSlot":"","healthEffect":0,"hungerRestore":0,"id":"steel_rebar","isEquipable":false,"moraleEffect":0,"radCleanse":0,"radProtection":0,"stackMax":10,"thirstRestore":0,"tradeVa…`
  - ... 564 additional rows omitted from the compact audit; the complete current file is identified above ...

# Appendix D — Current caller/reference graph

### `LibraryManualCatalogLoader` (18 sampled current references)
- Assets/Ashfall.Core/LibraryManualCatalogLoader.cs:18: public static class LibraryManualCatalogLoader
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:382: ["library_manuals.json"] = new[] { "LibraryManualCatalogLoader" },
- src/Host/LibraryStudyHostSession.cs:53: int count = LibraryManualCatalogLoader.LoadAndRegister(System, dataDir, fileIO, serializer);
- Ashfall.Core.Tests/NewCatalogLoaderTests.cs:133: public class LibraryManualCatalogLoaderTests
- Ashfall.Core.Tests/NewCatalogLoaderTests.cs:151: var defs = LibraryManualCatalogLoader.Load(dataDir, io, json);
- Ashfall.Core.Tests/NewCatalogLoaderTests.cs:170: int count = LibraryManualCatalogLoader.LoadAndRegister(system, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/NewCatalogLoaderTests.cs:182: var result = LibraryManualCatalogLoader.Load("/nonexistent", io, json);
- Ashfall.Core.Tests/Progression/LibraryStudyCatalogExpansionTests.cs:37: var manuals = LibraryManualCatalogLoader.Load(dataDir, fileIO, json);
- Ashfall.Core.Tests/Progression/LibraryStudyCatalogExpansionTests.cs:48: var manuals = LibraryManualCatalogLoader.Load(dataDir, fileIO, json);
- Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs:52: var defs = LibraryManualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs:370: LibraryManualCatalogLoader.LoadAndRegister(
- Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs:393: LibraryManualCatalogLoader.LoadAndRegister(
- Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs:415: LibraryManualCatalogLoader.LoadAndRegister(
- Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs:439: LibraryManualCatalogLoader.LoadAndRegister(
- Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs:453: LibraryManualCatalogLoader.LoadAndRegister(
- Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs:470: LibraryManualCatalogLoader.LoadAndRegister(
- Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs:40: var defs = LibraryManualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/Integration/Plans60To63ThirtyDayIntegrationTests.cs:61: var manuals = LibraryManualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
### `LibraryStudySystem` (18 sampled current references)
- Assets/Ashfall.Core/LibraryManualCatalogLoader.cs:40: LibraryStudySystem system,
- Assets/Ashfall.Core/LibraryStudySystem.cs:14: public string systemId = LibraryStudySystem.SystemId;
- Assets/Ashfall.Core/LibraryStudySystem.cs:93: public sealed class LibraryStudySystem
- Assets/Ashfall.Core/LibraryStudySystem.cs:126: public LibraryStudySystem(
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:746: ["library_manuals.json"] = "LibraryStudySystem",
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:992: ["library_manuals.json"] = new[] { "LibraryStudySystem" },
- src/Main.ShelterBatch3.cs:208: var lsSys = new LibraryStudySystem(lsSkills, lsResearch, lsJournal, _expandedShelterRoster, new GodotLog());
- src/Host/LibraryStudyHostSession.cs:14: /// Host session for LibraryStudySystem.
- src/Host/LibraryStudyHostSession.cs:20: public LibraryStudySystem System { get; }
- src/Host/LibraryStudyHostSession.cs:23: LibraryStudySystem system,
- src/Host/LibraryStudyHostSession.cs:30: ?? new LibraryStudySystem(skills, research, journal, roster, new GodotLog());
- Ashfall.Core.Tests/JournalProducerIntegrationTests.cs:99: var library = new LibraryStudySystem(skills, research, journal, roster);
- Ashfall.Core.Tests/LibraryStudySystemTests.cs:9: public class LibraryStudySystemTests
- Ashfall.Core.Tests/LibraryStudySystemTests.cs:132: private static LibraryStudySystem Create(out SkillProgressionSystem skills, out ResearchSystem research, out JournalSystem journal, out DutyRosterSystem roster)
- Ashfall.Core.Tests/LibraryStudySystemTests.cs:138: return new LibraryStudySystem(skills, research, journal, roster);
- Ashfall.Core.Tests/NewCatalogLoaderTests.cs:168: var system = new LibraryStudySystem(skills, research, journal, roster);
- Ashfall.Core.Tests/Shelter/Plan23APowerContinuityTests.cs:17: /// the authored <c>requires_power</c> gate on <see cref="LibraryStudySystem"/>.
- Ashfall.Core.Tests/Shelter/Plan23APowerContinuityTests.cs:128: private static LibraryStudySystem MakeLibrary()
### `LibraryStudyHostSession` (6 sampled current references)
- src/Main.ShelterBatch3.cs:29: private LibraryStudyHostSession _libraryStudy = null!;
- src/Main.ShelterBatch3.cs:215: _libraryStudy = new LibraryStudyHostSession(lsSys, lsSkills, lsResearch, lsJournal, _expandedShelterRoster);
- src/Host/LibraryStudyHostSession.cs:18: public sealed class LibraryStudyHostSession
- src/Host/LibraryStudyHostSession.cs:22: public LibraryStudyHostSession(
- src/UI/LibraryStudyPanel.cs:28: private LibraryStudyHostSession? _host;
- src/UI/LibraryStudyPanel.cs:33: public void Bind(LibraryStudyHostSession session)
### `LibraryStudyPanel` (10 sampled current references)
- src/Main.ShelterBatch3.cs:47: private LibraryStudyPanel _libraryStudyPanel = null!;
- src/Main.ShelterBatch3.cs:217: if (_libraryStudyPanel != null && _libraryStudyPanel.IsInsideTree())
- src/Main.ShelterBatch3.cs:218: RemoveChild(_libraryStudyPanel);
- src/Main.ShelterBatch3.cs:219: _libraryStudyPanel = new LibraryStudyPanel();
- src/Main.ShelterBatch3.cs:220: _libraryStudyPanel.Bind(_libraryStudy);
- src/Main.ShelterBatch3.cs:221: _libraryStudyPanel.Visible = false;
- src/Main.ShelterBatch3.cs:222: AddChild(_libraryStudyPanel);
- src/Main.ExpandedShelterSystems.cs:628: if (_libraryStudyPanel != null) { _libraryStudyPanel.Visible = true; _libraryStudyPanel.RefreshView(); }
- src/Main.ExpandedShelterSystems.cs:752: RemovePanel(_libraryStudyPanel); _libraryStudyPanel = null!;
- src/UI/LibraryStudyPanel.cs:17: public partial class LibraryStudyPanel : Control, IBindablePanel
### `SaveLibraryStudy` (3 sampled current references)
- Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:125: new("library_study", "SaveLibraryStudy", "SetupLibraryStudy", "knowledge", "Research library books and blueprints", LifecycleGroup: ExpandedShelterLifecycleGroup),
- src/Main.ShelterBatch3.cs:409: private void SaveLibraryStudy()
- src/Main.ExpandedShelterSystems.cs:346: SaveLibraryStudy();

# Appendix E — Current focused-test inventory

Current test declaration inventory: 36 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/LibraryStudySystemTests.cs` — 8 test declarations; bytes=6,497; SHA-256=`ebebdeefe40e1e8593ae004cb917fb0833d54b898a34bb570338eba74651167f`
- 00011: [Fact] public void StartStudy_WithoutPrereq_Blocks()
- 00022: [Fact] public void StartStudy_WithPrereq_StartsJob()
- 00033: [Fact] public void TickDay_CompletesStudy()
- 00046: [Fact] public void CompleteStudy_UnlocksResearch()
- 00058: [Fact] public void StartStudy_AlreadyCompleted_Blocks()
- 00071: [Fact] public void LoadCatalog_OddLengthSkillGrantList_Throws()
- 00089: [Fact] public void StartStudy_ZeroStudyHours_Blocks()
- 00112: [Fact] public void CaptureRestoreState_PreservesJobs()
### `Ashfall.Core.Tests/Library/LibraryStudyContractTests.cs` — 16 test declarations; bytes=16,271; SHA-256=`2ef4e6cbd958e0abbb3321372e52bba0605d746219680042aac9c4981f514279`
- 00036: [Fact]
- 00037: public void B2_001_ManualStudy_CallsUnlockManual_NeverCompleteResearch()
- 00078: [Fact]
- 00079: public void B2_002_And_B2_004_JournalEvidenceAddedAndDedupedWithStableProvenance()
- 00106: [Fact]
- 00107: public void B2_003_DuplicateResearchUnlock_IsIdempotent()
- 00152: [Fact]
- 00153: public void B2_005_And_B2_006_SkillRaisesStudyRateMonotonically_WithinStrictBounds()
- 00202: [Fact]
- 00203: public void B2_007_InvalidZeroOrNegativeHours_Rejected()
- 00231: [Fact]
- 00232: public void B2_008_And_B2_009_BidirectionalAvailabilityReservation_DutyRoster()
- 00279: [Fact]
- 00280: public void B2_010_To_B2_014_AuthoritativeCatalogIntegrity_24Manuals_6Disciplines()
- 00349: [Fact]
- 00350: public void B2_016_And_B2_017_SaveRestore_PreservesJobsAndUnknownCompletedIds()
### `Ashfall.Core.Tests/Progression/LibraryStudyCatalogExpansionTests.cs` — 4 test declarations; bytes=2,194; SHA-256=`8a5e4169976555fd0f8ea4c33fe6892c81889c9ced696fc1d165b8e5107bd508`
- 00030: [Fact]
- 00031: public void Load_Loads12ManualsFromCatalog()
- 00041: [Fact]
- 00042: public void Load_AllManualsHaveValidFieldsAndRequirements()
### `Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs` — 8 test declarations; bytes=9,904; SHA-256=`f67e3c01031b535672055663c72a3eb2d411dd56038c31c16dc7a5598b09cc8a`
- 00066: [Fact]
- 00067: public void Plan80_61_TradeManual_AcquisitionAndShelterStudy_RoundTrip()
- 00125: [Fact]
- 00126: public void Plan80_61_Prerequisites_And_PowerGating_Enforced()
- 00161: [Fact]
- 00162: public void Plan80_61_TradeFairness_And_KnowledgeValuation()
- 00192: [Fact]
- 00193: public void Plan80_61_LibraryStudy_SaveRestore_Parity()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Library Manuals: Twenty-Four-Manual Knowledge Progression, Skill XP, and Existing Save/Host Seams` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan’s 15-manual target is stale; current data has 24.
- A research/skill id in JSON is not proof of a current consumer.

## H.2 Evidence-strength corrections
- Audit all references rather than trusting 24 rows.
- Separate static reward metadata from executed effects.
- Preserve prerequisite and replay contracts.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Library Manuals: Twenty-Four-Manual Knowledge Progression, Skill XP, and Existing Save/Host Seams while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use LibraryManualCatalogLoader for definitions and references.
- Use LibraryStudySystem for mutable study state and progression timing.
- Use canonical skill/research/knowledge owners for unlocks.
- Use LibraryStudyHostSession/library_study for host persistence.
- Use LibraryStudyPanel as a truthful projection.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement library manuals: twenty-four-manual knowledge progression, skill xp, and existing save/host seams arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- No arbitrary manual growth or new progression owner is proposed.
- Every future completion path must use the existing library_study and canonical unlock owners.

## J.2 Questions deliberately left open
- Should manual rewards be applied at completion or through existing research/skill events?
- Which current room/facility is the canonical powered-study gate?

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

The original file at `HEAD:piagentsplans/80-library-manuals-expansion.md` contained 4,960 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 80 — Library Manuals Expansion (3 → 15 study manuals)

## Goal (2 lines)
Expand `library_manuals.json` from 3 verified manuals to 15. The library study
system (`LibraryStudyHostSession.cs` confirmed live) lets survivors study manuals
to gain skill XP, unlock research and knowledge, at the cost of fatigue and
morale. 3 manuals is too few for a knowledge-progression system that should
cover survival, medical, combat, engineering, and science.

## Why (P2)
- Verified: `library_manuals.json` has 3 entries (manual_id, display_name,
  category, study_hours_required, fatigue_per_hour, morale_effect,
  skill_xp_grants, research_unlocks, knowledge_unlocks, prerequisites,
  requires_power). `LibraryManualCatalogLoader.cs` and
  `LibraryStudyHostSession.cs` are confirmed live.
- Creates the knowledge-progression pillar: manuals are how survivors learn
  new skills without expeditions. Studying costs fatigue and morale (sitting
  still in a bunker reading is not free), and the prerequisite chain creates a
  learning tree. 3 manuals cover water filtration, radiation first aid, and
  improvised weapons — the other skill domains are invisible.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/library_manuals.json` (expand 3 → 15 manuals)
- Read-only: `Assets/Ashfall.Core/LibraryManualCatalogLoader.cs` (confirm schema
  and how skill_xp_grants / research_unlocks / knowledge_unlocks resolve)
- `Assets/StreamingAssets/Data/items.json` (if manuals are items, confirm)

## Content grammar (per manual)
- snake_case `id` with prefix `manual_` (confirmed prefix).
- Category: technical / medical / military / scientific / survival / social.
- study_hours_required: 6–20 (time cost to complete the manual).
- fatigue_per_hour: 0.2–0.5 (fatigue accumulated per study hour).
- morale_effect: -0.8 to -0.2 (studying is demoralizing — sitting in a bunker
  reading while the world dies outside).
- skill_xp_grants: 1–2 skill ids (skill_* prefix) — XP granted on completion.
- research_unlocks: 1–2 research ids (research_* prefix).
- knowledge_unlocks: 1–2 knowledge ids (knowledge_* prefix).
- prerequisites: 0–2 manual_ids that must be studied first (creates a learning
  tree — advanced manuals require foundational ones).
- requires_power: true/false (some manuals need powered library/study room).
- Difficulty curve: foundational manuals (no prereqs, low hours) → intermediate
  (1 prereq, moderate hours) → advanced (2 prereqs, high hours, high reward).

## Steps
1. Read `LibraryManualCatalogLoader.cs` to confirm the schema and how
   skill_xp_grants, research_unlocks, and knowledge_unlocks resolve.
2. Read the existing 3 manuals to confirm the prerequisite chain pattern
   (manual_improvised_weapons requires manual_water_filtration).
3. Confirm which skill_*, research_*, and knowledge_* ids exist by grepping the
   relevant catalogs (Plan 33 skills, Plan 34 research).
4. Author 12 new manuals across 6 categories:
   - Technical (2): shelter repair manual, electrical systems manual.
   - Medical (2): field surgery manual, epidemic response manual.
   - Military (2): squad tactics manual, fortification manual.
   - Scientific (2): radiation monitoring manual, soil analysis manual.
   - Survival (3): advanced foraging manual, water purification manual,
     cold-weather survival manual.
   - Social (1): conflict mediation manual (morale management).
5. Each manual: distinct category, study hours, fatigue, morale cost, skill XP,
   research/knowledge unlocks, and prerequisite chain. Build a 3-level learning
   tree: foundational → intermediate → advanced.
6. Cross-reference: every manual_id unique; every skill_xp_grants resolves in
   the skill catalog; every research_unlocks resolves in the research catalog;
   every knowledge_unlocks resolves in the knowledge catalog; every
   prerequisite manual_id exists in this file.
7. Validate: `--data-integrity-selftest` (all ids resolve).
8. xUnit: library manual catalog loads 15 manuals, all ids unique, all skill/
   research/knowledge/prerequisite ids resolve, no circular prerequisites,
   study_hours and fatigue within valid ranges.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is prerequisite cycles (step 6): confirm no
manual's prerequisite chain forms a cycle (A requires B requires A).

## Definition of Done
- `library_manuals.json` has 15 manuals, all ids resolving, no circular
  prerequisites, integrity + tests green.

## Follow-on
- Plan 33 (skill catalog) — manuals grant skill XP.
- Plan 34 (research tree) — manuals unlock research nodes.
- Plan 71 (power grid rooms) — some manuals require powered study rooms.
- Plan 72 (utility AI actions) — study actions reference manuals.
- Existing 26 (knowledge/research/skills) — this plan provides the manual data.

```

## End of Plan 80 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/LibraryStudySystem.cs` — 353 lines; 15,082 bytes; SHA-256 `d8ed15df8fd325172f8ad2b9e2ae8629172d95cbf94708b833150fda32039f5f`
Declaration index:
- 00012: public sealed class LibraryStudyState
- 00021: public sealed class ManualDefinition
- 00082: public sealed class StudyJob
- 00093: public sealed class LibraryStudySystem
- 00114: public bool IsManualPowered(string manualId)
- 00145: public bool IsReaderStudying(string survivorId)
- 00151: public static string NormalizeDiscipline(string category)
- 00176: public float GetComprehensionRate(string readerId, string manualId)
- 00188: public float GetEffectiveStudyHours(string readerId, string manualId)
- 00195: public float GetEstimatedDays(string readerId, string manualId)
- 00202: public void LoadCatalog(List<ManualDefinition> manuals)
- 00220: public ActionResult StartStudy(string manualId, string readerId)
- 00267: public ActionResult CancelStudy(string jobId)
- 00278: public void TickDay(int day)
- 00333: public List<StudyJob> GetActiveJobs() => _state.activeJobs.FindAll(j => !j.isComplete && !j.isCancelled);
- 00335: public bool IsManualCompleted(string manualId) => _state.completedManualIds.Contains(manualId);
- 00337: public LibraryStudyState CaptureState() => CloneState(_state);
- 00339: public void RestoreState(LibraryStudyState saved)
- 00345: private static LibraryStudyState CloneState(LibraryStudyState src)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Text.Json.Serialization;
00005: #pragma warning disable CS8618
00006: using Ashfall.Core.Journal;
00007: using Ashfall.Core.Survivors;
00008:
00009: namespace Ashfall.Core
00010: {
00011:     [Serializable]
00012:     public sealed class LibraryStudyState
00013:     {
00014:         public string systemId = LibraryStudySystem.SystemId;
00015:         public List<StudyJob> activeJobs = new List<StudyJob>();
00016:         public List<string> completedManualIds = new List<string>();
00017:         public int totalStudyHours;
00018:     }
00019:
00020:     [Serializable]
00021:     public sealed class ManualDefinition
00022:     {
00023:         [JsonPropertyName("manual_id")]
00024:         public string manual_id { get; set; } = string.Empty;
00025:
00026:         [JsonPropertyName("display_name")]
00027:         public string display_name { get; set; } = string.Empty;
00028:
00029:         [JsonPropertyName("category")]
00030:         public string category { get; set; } = string.Empty;       // "technical", "medical", "military", etc.
00031:
00032:         [JsonPropertyName("study_hours_required")]
00033:         public int studyHoursRequired { get; set; } = 10;
00034:
00035:         [JsonPropertyName("fatigue_per_hour")]
00036:         public float fatiguePerHour { get; set; } = 0.3f;
00037:
00038:         [JsonPropertyName("morale_effect")]
00039:         public float moraleEffect { get; set; } = -0.5f;           // studying is draining
00040:
00041:         [JsonPropertyName("skill_xp_grants")]
00042:         public List<string> skillXpGrants { get; set; } = new List<string>(); // skill_id, xp_amount pairs
00043:
00044:         [JsonPropertyName("research_unlocks")]
00045:         public List<string> researchUnlocks { get; set; } = new List<string>();
00046:
00047:         [JsonPropertyName("knowledge_unlocks")]
00048:         public List<string> knowledgeUnlocks { get; set; } = new List<string>();
00049:
00050:         [JsonPropertyName("prerequisites")]
00051:         public List<string> prerequisites { get; set; } = new List<string>();
00052:
00053:         [JsonPropertyName("requires_power")]
00054:         public bool requiresPower { get; set; } = true;
00055:
00056:         [JsonPropertyName("loot_table_ids")]
00057:         public List<string> lootTableIds { get; set; } = new List<string>();
00058:
00059:         [JsonPropertyName("expedition_reward_ids")]
00060:         public List<string> expeditionRewardIds { get; set; } = new List<string>();
00061:
00062:         [JsonPropertyName("trader_pool_ids")]
00063:         public List<string> traderPoolIds { get; set; } = new List<string>();
00064:
00065:         [JsonPropertyName("archive_scribing_recipe_id")]
00066:         public string archiveScribingRecipeId { get; set; } = string.Empty;
00067:
00068:         [JsonPropertyName("starting_origin_ids")]
00069:         public List<string> startingOriginIds { get; set; } = new List<string>();
00070:
00071:         [JsonPropertyName("origin_facility")]
00072:         public string originFacility { get; set; } = string.Empty;
00073:
00074:         [JsonPropertyName("technical_complexity_tier")]
00075:         public int technicalComplexityTier { get; set; } = 1;
00076:
00077:         [JsonPropertyName("schematic_summary")]
00078:         public string schematicSummary { get; set; } = string.Empty;
00079:     }
00080:
00081:     [Serializable]
00082:     public sealed class StudyJob
00083:     {
00084:         public string jobId = string.Empty;
00085:         public string manualId = string.Empty;
00086:         public string readerId = string.Empty;
00087:         public int dayStarted = -1;
00088:         public float progressHours;
00089:         public bool isComplete;
00090:         public bool isCancelled;
00091:     }
00092:
00093:     public sealed class LibraryStudySystem
00094:     {
00095:         public const string SystemId = "library_study";
00096:         private LibraryStudyState _state = new LibraryStudyState();
00097:         private readonly Dictionary<string, ManualDefinition> _catalog = new Dictionary<string, ManualDefinition>(StringComparer.Ordinal);
00098:         private readonly ILog _log;
00099:         private readonly SkillProgressionSystem _skills;
00100:         private readonly ResearchSystem _research;
00101:         private readonly JournalSystem _journal;
00102:         private readonly DutyRosterSystem _roster;
00103:         private int _currentDay;
00104:
00105:         /// <summary>
00106:         /// Optional grid-derived power gate for manuals authored with
00107:         /// <c>requires_power</c>. Null (the default) keeps legacy behaviour — study is
00108:         /// always possible. The host binds it to the canonical power allocation so a
00109:         /// blackout does not silently grant a powered manual's full progress.
00110:         /// </summary>
00111:         public Func<bool>? PowerAvailable { get; set; }
00112:
00113:         /// <summary>True when the manual currently has the power it declares it needs.</summary>
00114:         public bool IsManualPowered(string manualId)
00115:         {
00116:             if (PowerAvailable == null) return true;
00117:             if (!_catalog.TryGetValue(manualId, out var manual)) return true;
00118:             return !manual.requiresPower || PowerAvailable();
00119:         }
00120:
00121:         public LibraryStudyState State => _state;
00122:         public IReadOnlyDictionary<string, ManualDefinition> Catalog => _catalog;
00123:         public event Action<StudyJob> OnJobCompleted;
00124:         public event Action OnLibraryChanged;
00125:
00126:         public LibraryStudySystem(
00127:             SkillProgressionSystem skills,
00128:             ResearchSystem research,
00129:             JournalSystem journal,
00130:             DutyRosterSystem roster,
00131:             ILog? log = null)
00132:         {
00133:             _skills = skills ?? throw new ArgumentNullException(nameof(skills));
00134:             _research = research ?? throw new ArgumentNullException(nameof(research));
00135:             _journal = journal ?? throw new ArgumentNullException(nameof(journal));
00136:             _roster = roster ?? throw new ArgumentNullException(nameof(roster));
00137:             _log = log ?? NullLog.Instance;
00138:
00139:             // Bidirectional availability reservation with DutyRoster (B2-009)
00140:             var prevReservation = _roster.IsSurvivorReservedExternally;
00141:             _roster.IsSurvivorReservedExternally = id =>
00142:                 (prevReservation != null && prevReservation(id)) || IsReaderStudying(id);
00143:         }
00144:
00145:         public bool IsReaderStudying(string survivorId)
00146:         {
00147:             if (string.IsNullOrEmpty(survivorId)) return false;
00148:             return _state.activeJobs.Exists(j => !j.isComplete && !j.isCancelled && j.readerId == survivorId);
00149:         }
00150:
00151:         public static string NormalizeDiscipline(string category)
00152:         {
00153:             if (string.IsNullOrEmpty(category)) return "survival";
00154:             string lower = category.Trim().ToLowerInvariant();
00155:             switch (lower)
00156:             {
00157:                 case "technical":
00158:                 case "engineering":
00159:                 case "crafting":
00160:                     return "crafting";
00161:                 case "military":
00162:                 case "combat":
00163:                     return "combat";
00164:                 case "medical":
00165:                     return "medical";
00166:                 case "science":
00167:                     return "science";
00168:                 case "scavenging":
00169:                     return "scavenging";
00170:                 case "survival":
00171:                 default:
00172:                     return "survival";
00173:             }
00174:         }
00175:
00176:         public float GetComprehensionRate(string readerId, string manualId)
00177:         {
00178:             if (!_catalog.TryGetValue(manualId, out var manual)) return 1.0f;
00179:             string disc = NormalizeDiscipline(manual.category);
00180:             float progress01 = _skills.GetDisciplineProgress01(readerId, disc);
00181:             float bonus = _skills.GetCachedBonus(readerId, disc);
00182:             // Monotonic: rate = 1.0 + 0.6 * progress01 + 0.4 * bonus
00183:             float rate = 1.0f + 0.6f * progress01 + 0.4f * bonus;
00184:             // Strict bounds: [0.75f, 2.0f] (B2-006)
00185:             return Math.Clamp(rate, 0.75f, 2.0f);
00186:         }
00187:
00188:         public float GetEffectiveStudyHours(string readerId, string manualId)
00189:         {
00190:             if (!_catalog.TryGetValue(manualId, out var manual)) return 0f;
00191:             float rate = GetComprehensionRate(readerId, manualId);
00192:             return (float)Math.Round(manual.studyHoursRequired / rate, 1);
00193:         }
00194:
00195:         public float GetEstimatedDays(string readerId, string manualId)
00196:         {
00197:             float effHours = GetEffectiveStudyHours(readerId, manualId);
00198:             if (effHours <= 0f) return 0f;
00199:             return (float)Math.Ceiling(effHours / 8.0f);
00200:         }
00201:
00202:         public void LoadCatalog(List<ManualDefinition> manuals)
00203:         {
00204:             if (manuals == null) return;
00205:             _catalog.Clear();
00206:             foreach (var m in manuals)
00207:             {
00208:                 if (string.IsNullOrEmpty(m.manual_id)) continue;
00209:                 // Bug-10: skillXpGrants is documented as (skillId, xpAmount) pairs;
00210:                 // an odd-length list would IndexOutOfRange on TickDay when the loop
00211:                 // reads grants[i+1]. Reject malformed manuals at load time so the
00212:                 // bad data never reaches the tick path.
00213:                 if (m.skillXpGrants != null && m.skillXpGrants.Count % 2 != 0)
00214:                     throw new System.IO.InvalidDataException(
00215:                         $"manual '{m.manual_id}' has {m.skillXpGrants.Count} skillXpGrants entries — expected pairs (skillId, xpAmount)");
00216:                 _catalog[m.manual_id] = m;
00217:             }
00218:         }
00219:
00220:         public ActionResult StartStudy(string manualId, string readerId)
00221:         {
00222:             if (!_catalog.TryGetValue(manualId, out var manual))
00223:                 return ActionResult.Failed("unknown_manual", "library.unknown_manual");
00224:
00225:             // Bug-15b: a manual with studyHoursRequired <= 0 would complete
00226:             // instantly on TickDay (8h >= 0 is trivially satisfied), granting
00227:             // all XP / research / knowledge unlocks in zero time. Reject such
00228:             // manuals at the start path so they never reach the tick loop.
00229:             // Validated at StartStudy, not LoadCatalog, so existing catalogs
00230:             // (with manually-curated 0-hour entries) still load.
00231:             if (manual.studyHoursRequired <= 0)
00232:                 return ActionResult.Blocked("invalid_hours", "library.invalid_hours");
00233:
00234:             if (_state.completedManualIds.Contains(manualId))
00235:                 return ActionResult.Blocked("already_completed", "library.already_completed");
00236:
00237:             // Check prerequisites
00238:             foreach (var prereq in manual.prerequisites)
00239:             {
00240:                 if (!_state.completedManualIds.Contains(prereq))
00241:                     return ActionResult.Blocked("missing_prerequisite", "library.missing_prerequisite");
00242:             }
00243:
00244:             // Authored power requirement: a powered-only manual cannot start during a
00245:             // blackout. Null provider = legacy always-powered behaviour.
00246:             if (manual.requiresPower && PowerAvailable != null && !PowerAvailable())
00247:                 return ActionResult.Blocked("power_unavailable", "library.no_power");
00248:
00249:             // Check duty roster availability (B2-008: GetRoleOf checks if reader is on duty)
00250:             if (!string.IsNullOrEmpty(_roster.GetRoleOf(readerId)))
00251:                 return ActionResult.Blocked("busy", "library.busy");
00252:
00253:             // Reader cannot study two manuals simultaneously
00254:             if (IsReaderStudying(readerId))
00255:                 return ActionResult.Blocked("busy", "library.busy");
00256:
00257:             var job = new StudyJob
00258:             {
00259:                 jobId = $"study_{_currentDay}_{manualId}_{readerId}",
00260:                 manualId = manualId, readerId = readerId, dayStarted = _currentDay
00261:             };
00262:             _state.activeJobs.Add(job);
00263:             OnLibraryChanged?.Invoke();
00264:             return ActionResult.Success("library.study_started");
00265:         }
00266:
00267:         public ActionResult CancelStudy(string jobId)
00268:         {
00269:             var job = _state.activeJobs.Find(j => j.jobId == jobId);
00270:             if (job == null || job.isComplete || job.isCancelled)
00271:                 return ActionResult.Blocked("no_job", "library.no_job");
00272:
00273:             job.isCancelled = true;
00274:             OnLibraryChanged?.Invoke();
00275:             return ActionResult.Success("library.study_cancelled");
00276:         }
00277:
00278:         public void TickDay(int day)
00279:         {
00280:             _currentDay = day;
00281:
00282:             foreach (var job in _state.activeJobs)
00283:             {
00284:                 if (job.isComplete || job.isCancelled) continue;
00285:                 if (!_catalog.TryGetValue(job.manualId, out var manual))
00286:                 {
00287:                     _log.Warn($"[Library] active job '{job.jobId}' references unknown manual '{job.manualId}'");
00288:                     continue;
00289:                 }
00290:
00291:                 // Unpowered powered-only manual: the job is retained but makes no
00292:                 // progress until power returns (never silently completed).
00293:                 if (manual.requiresPower && PowerAvailable != null && !PowerAvailable())
00294:                     continue;
00295:
00296:                 float rate = GetComprehensionRate(job.readerId, job.manualId);
00297:                 job.progressHours += 8f * rate;
00298:
00299:                 if (job.progressHours >= manual.studyHoursRequired)
00300:                 {
00301:                     job.isComplete = true;
00302:                     if (!_state.completedManualIds.Contains(job.manualId))
00303:                         _state.completedManualIds.Add(job.manualId);
00304:                     _state.totalStudyHours += (int)job.progressHours;
00305:
00306:                     // Grant skill XP
00307:                     for (int i = 0; i < manual.skillXpGrants.Count; i += 2)
00308:                     {
00309:                         string skillId = manual.skillXpGrants[i];
00310:                         if (float.TryParse(manual.skillXpGrants[i + 1], out float xp))
00311:                             _skills.RecordAction(new SimpleSkillActor(job.readerId), skillId, xp, _currentDay);
00312:                     }
00313:
00314:                     // Unlock research (reveal / discover only — NEVER CompleteResearch!)
00315:                     foreach (var unlock in manual.researchUnlocks)
00316:                         _research.UnlockManual(unlock);
00317:
00318:                     // Add knowledge evidence (idempotent and deduped in JournalSystem)
00319:                     foreach (var knowledge in manual.knowledgeUnlocks)
00320:                     {
00321:                         _journal.AddKnowledgeEvidence(job.readerId, knowledge);
00322:                     }
00323:
00324:                     _log.Info($"[Library] {job.readerId} completed {manual.display_name}");
00325:                     OnJobCompleted?.Invoke(job);
00326:                 }
00327:             }
00328:
00329:             _state.activeJobs.RemoveAll(j => j.isCancelled);
00330:             OnLibraryChanged?.Invoke();
00331:         }
00332:
00333:         public List<StudyJob> GetActiveJobs() => _state.activeJobs.FindAll(j => !j.isComplete && !j.isCancelled);
00334:
00335:         public bool IsManualCompleted(string manualId) => _state.completedManualIds.Contains(manualId);
00336:
00337:         public LibraryStudyState CaptureState() => CloneState(_state);
00338:
00339:         public void RestoreState(LibraryStudyState saved)
00340:         {
00341:             if (saved == null) return;
00342:             _state = CloneState(saved);
00343:         }
00344:
00345:         private static LibraryStudyState CloneState(LibraryStudyState src)
00346:         {
00347:             if (src == null) return new LibraryStudyState();
00348:             var s = new SystemTextJsonSerializer();
00349:             var json = s.Serialize(src);
00350:             return s.Deserialize<LibraryStudyState>(json) ?? new LibraryStudyState();
00351:         }
00352:     }
00353: }
```

## `Assets/Ashfall.Core/LibraryManualCatalogLoader.cs` — 51 lines; 1,742 bytes; SHA-256 `9edd917d8b80e39a022884e756123404c5850eeea07313f915d302af335eb310`
Declaration index:
- 00009: public sealed class LibraryManualCatalogContainer
- 00018: public static class LibraryManualCatalogLoader
- 00022: public static List<ManualDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
- 00039: public static int LoadAndRegister(
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core
00006: {
00007:     /// <summary>Container shape for library_manuals.json (the authority).</summary>
00008:     [Serializable]
00009:     public sealed class LibraryManualCatalogContainer
00010:     {
00011:         public List<ManualDefinition> manuals = new List<ManualDefinition>();
00012:     }
00013:
00014:     /// <summary>
00015:     /// Loads library manual definitions from JSON.
00016:     /// Engine-agnostic: uses IFileIO and IJsonSerializer ports.
00017:     /// </summary>
00018:     public static class LibraryManualCatalogLoader
00019:     {
00020:         public const string DefaultFileName = "library_manuals.json";
00021:
00022:         public static List<ManualDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
00023:         {
00024:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00025:                 return new List<ManualDefinition>();
00026:
00027:             string path = fileIO.Combine(dataDir, DefaultFileName);
00028:             if (!fileIO.FileExists(path))
00029:                 return new List<ManualDefinition>();
00030:
00031:             string rawText = fileIO.ReadAllText(path);
00032:             if (string.IsNullOrWhiteSpace(rawText))
00033:                 return new List<ManualDefinition>();
00034:
00035:             var container = json.Deserialize<LibraryManualCatalogContainer>(rawText);
00036:             return container?.manuals ?? new List<ManualDefinition>();
00037:         }
00038:
00039:         public static int LoadAndRegister(
00040:             LibraryStudySystem system,
00041:             string dataDir,
00042:             IFileIO fileIO,
00043:             IJsonSerializer json)
00044:         {
00045:             if (system == null) return 0;
00046:             var defs = Load(dataDir, fileIO, json);
00047:             system.LoadCatalog(defs);
00048:             return defs.Count;
00049:         }
00050:     }
00051: }
```

## `src/Host/LibraryStudyHostSession.cs` — 129 lines; 5,063 bytes; SHA-256 `aa2494ed7d7e7148b927868c88f9f2e15b32397ad2762bbbe329346f64d5249a`
Declaration index:
- 00018: public sealed class LibraryStudyHostSession
- 00040: public void LoadCatalog(List<ManualDefinition> manuals)
- 00048: public void LoadCatalog(string dataDir)
- 00061: public ActionResult StartStudy(string manualId, string readerId)
- 00072: public void TickDay(int day)
- 00095: public static class LibraryStudySaveStore
- 00110: public static bool TrySave(LibraryStudyState state) => s_store.TrySave(state);
- 00112: public static LibraryStudyState? TryLoad() => s_store.TryLoad();
- 00115: public static string TryCapturePersisted(LibraryStudyState state) => s_store.CapturePersisted(state);
- 00118: public static string TryCaptureDirect(LibraryStudyState state) => s_store.CaptureBare(state);
- 00121: public static LibraryStudyState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
- 00124: public static string TryCapture(LibraryStudyState state) => s_store.CaptureBare(state);
- 00127: public static LibraryStudyState? TryRestore(string json) => s_store.RestoreBare(json);
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Journal;
00008: using Ashfall.Core.Survivors;
00009: using Ashfall.Core.Save;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     /// <summary>
00014:     /// Host session for LibraryStudySystem.
00015:     /// Wraps the Core library pipeline (LoadCatalog → StartStudy → TickDay)
00016:     /// and forwards StateChanged for host wiring. Engine-agnostic Core authority.
00017:     /// </summary>
00018:     public sealed class LibraryStudyHostSession
00019:     : HostSessionBase{
00020:         public LibraryStudySystem System { get; }
00021:         public string LastEvent { get; private set; } = string.Empty;
00022:         public LibraryStudyHostSession(
00023:             LibraryStudySystem system,
00024:             SkillProgressionSystem skills,
00025:             ResearchSystem research,
00026:             JournalSystem journal,
00027:             DutyRosterSystem roster)
00028:         {
00029:             System = system
00030:                 ?? new LibraryStudySystem(skills, research, journal, roster, new GodotLog());
00031:
00032:             System.OnJobCompleted += _ =>
00033:             {
00034:                 LastEvent = "Study completed";
00035:                 RaiseStateChanged();
00036:             };
00037:             System.OnLibraryChanged += () => RaiseStateChanged();
00038:         }
00039:
00040:         public void LoadCatalog(List<ManualDefinition> manuals)
00041:         {
00042:             System.LoadCatalog(manuals);
00043:             LastEvent = $"Library catalog loaded: {manuals.Count} manuals";
00044:             RaiseStateChanged();
00045:         }
00046:
00047:         /// <summary>Load the library_manuals.json catalog into the Core system (the authority).</summary>
00048:         public void LoadCatalog(string dataDir)
00049:         {
00050:             if (string.IsNullOrEmpty(dataDir)) return;
00051:             var fileIO = new FileSystemIO();
00052:             var serializer = new SystemTextJsonSerializer();
00053:             int count = LibraryManualCatalogLoader.LoadAndRegister(System, dataDir, fileIO, serializer);
00054:             if (count > 0)
00055:             {
00056:                 LastEvent = $"Library manual catalog loaded: {count} manuals";
00057:                 RaiseStateChanged();
00058:             }
00059:         }
00060:
00061:         public ActionResult StartStudy(string manualId, string readerId)
00062:         {
00063:             var res = System.StartStudy(manualId, readerId);
00064:             if (res.IsSuccess)
00065:             {
00066:                 LastEvent = $"Study started: {manualId} by {readerId}";
00067:                 RaiseStateChanged();
00068:             }
00069:             return res;
00070:         }
00071:
00072:         public void TickDay(int day)
00073:         {
00074:             System.TickDay(day);
00075:             RaiseStateChanged();
00076:         }
00077:
00078:         public override void Save()
00079:         {
00080:             if (!IsDirty) return;
00081:             LibraryStudySaveStore.TrySave(System.CaptureState());
00082:             base.Save();
00083:         }
00084:     }
00085:
00086:     /// <summary>
00087:     /// LibraryStudySaveStore save persistence — thin façade over the Core
00088:     /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
00089:     /// shelter-batch section ships the legacy
00090:     /// &lt;c&gt;{ SchemaVersion, State, Checksum }&lt;/c&gt; envelope, preserved
00091:     /// byte-for-byte by the Core &lt;see cref="SchemaVersionedEnvelope{T}"/&gt;
00092:     /// adapter (presence-only checksum, legacy bare-state fallback); path
00093:     /// resolution, atomic write, and error handling live in the service.
00094:     /// </summary>
00095:     public static class LibraryStudySaveStore
00096:     {
00097:         public const string FileName = "library_study_save.json";
00098:         public const string SectionName = "library_study";
00099:
00100:         private static readonly SaveStore<LibraryStudyState> s_store = SaveStoreHub.FromCodec(
00101:             FileName,
00102:             nameof(LibraryStudySaveStore),
00103:             SchemaVersionedEnvelope<LibraryStudyState>.Encode,
00104:             SchemaVersionedEnvelope<LibraryStudyState>.Decode);
00105:
00106:         public static string SavePath => s_store.SavePath;
00107:
00108:         public static bool Exists => s_store.Exists();
00109:
00110:         public static bool TrySave(LibraryStudyState state) => s_store.TrySave(state);
00111:
00112:         public static LibraryStudyState? TryLoad() => s_store.TryLoad();
00113:
00114:         /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
00115:         public static string TryCapturePersisted(LibraryStudyState state) => s_store.CapturePersisted(state);
00116:
00117:         /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
00118:         public static string TryCaptureDirect(LibraryStudyState state) => s_store.CaptureBare(state);
00119:
00120:         /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
00121:         public static LibraryStudyState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
00122:
00123:         /// <summary>Capture state to JSON without writing to disk.</summary>
00124:         public static string TryCapture(LibraryStudyState state) => s_store.CaptureBare(state);
00125:
00126:         /// <summary>Restore state from JSON without reading from disk.</summary>
00127:         public static LibraryStudyState? TryRestore(string json) => s_store.RestoreBare(json);
00128:     }
00129: }
```

## `src/UI/LibraryStudyPanel.cs` — 347 lines; 17,160 bytes; SHA-256 `9437ba6059e60ba946b3b4a08e389683c13f3373f37659b09f831ef4bce580de`
Declaration index:
- 00017: public partial class LibraryStudyPanel : Control, IBindablePanel
- 00033: public void Bind(LibraryStudyHostSession session)
- 00047: public void Unbind()
- 00157: public void Open()
- 00164: public void RefreshView()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.UI;
00008: using AtomicWar.GodotApp;
00009: using DesignTheme = Ashfall.Core.UI.Theme;
00010:
00011: namespace AtomicWar.GodotApp.UI
00012: {
00013:     /// <summary>
00014:     /// ASHFALL — Cohort Library & Tech Study Management Interface.
00015:     /// Manages study manuals, research progression, skill gains, and reader assignments.
00016:     /// </summary>
00017:     public partial class LibraryStudyPanel : Control, IBindablePanel
00018:     {
00019:         public event Action? OnClose;
00020:
00021:         private AshfallDashboardShell _shell = null!;
00022:         private AshfallStatusRail? _statusRail;
00023:         private VBoxContainer _manualList = null!;
00024:         private VBoxContainer _studyDesk = null!;
00025:         private VBoxContainer _studyLogContainer = null!;
00026:         private Label _eventLogLabel = null!;
00027:
00028:         private LibraryStudyHostSession? _host;
00029:         private string? _selectedManualId;
00030:
00031:         public bool IsBound => _host != null;
00032:
00033:         public void Bind(LibraryStudyHostSession session)
00034:         {
00035:             if (_host != null)
00036:             {
00037:                 _host.StateChanged -= RefreshView;
00038:             }
00039:             _host = session;
00040:             if (_host != null)
00041:             {
00042:                 _host.StateChanged += RefreshView;
00043:             }
00044:             RefreshView();
00045:         }
00046:
00047:         public void Unbind()
00048:         {
00049:             if (_host != null)
00050:             {
00051:                 _host.StateChanged -= RefreshView;
00052:                 _host = null;
00053:             }
00054:         }
00055:
00056:
00057:
00058:         public override void _Ready()
00059:         {
00060:             SetAnchorsPreset(LayoutPreset.FullRect);
00061:             Visible = false;
00062:
00063:             var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
00064:             bg.SetAnchorsPreset(LayoutPreset.FullRect);
00065:             AddChild(bg);
00066:
00067:             var center = new CenterContainer();
00068:             center.SetAnchorsPreset(LayoutPreset.FullRect);
00069:             AddChild(center);
00070:
00071:             _shell = new AshfallDashboardShell("SYS: COHORT LIBRARY & TECH STUDY // STUDY MATRIX", minWidth: 1040, minHeight: 680);
00072:             center.AddChild(_shell);
00073:
00074:             _statusRail = _shell.SetStatusRail();
00075:             _statusRail.AddCard("manuals", "ARCHIVED MANUALS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
00076:             _statusRail.AddCard("active_study", "ACTIVE READERS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
00077:             _statusRail.AddCard("completed", "COMPLETED", "0", AshfallMetricCard.Criticality.Normal, minWidth: 110);
00078:             _statusRail.AddCard("study_speed", "LITERACY BUFF", "1.0x", AshfallMetricCard.Criticality.Normal, minWidth: 120);
00079:             _statusRail.AddCard("status", "LIBRARY DESK", "ONLINE", AshfallMetricCard.Criticality.Normal, minWidth: 120);
00080:
00081:             _shell.AttachHeaderCloseButton("CLOSE [Esc]", () =>
00082:             {
00083:                 Visible = false;
00084:                 OnClose?.Invoke();
00085:             });
00086:
00087:             // 3-Column Layout
00088:             var gridRow = new HBoxContainer();
00089:             gridRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00090:             gridRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00091:             gridRow.SizeFlagsVertical = SizeFlags.ExpandFill;
00092:
00093:             // Column 1: Manuals List
00094:             var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
00095:             leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00096:             leftPanel.SizeFlagsStretchRatio = 0.95f;
00097:             var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
00098:             leftPanel.AddChild(leftMargin);
00099:             var leftVbox = new VBoxContainer();
00100:             leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00101:             leftMargin.AddChild(leftVbox);
00102:             leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("ARCHIVED FIELD MANUALS"));
00103:             var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
00104:             _manualList = new VBoxContainer();
00105:             _manualList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
00106:             _manualList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00107:             leftScroll.AddChild(_manualList);
00108:             leftVbox.AddChild(leftScroll);
00109:             gridRow.AddChild(leftPanel);
00110:
00111:             // Column 2: Study Desk & Reader Assignment
00112:             var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
00113:             centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00114:             centerPanel.SizeFlagsStretchRatio = 1.2f;
00115:             var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
00116:             centerPanel.AddChild(centerMargin);
00117:             var centerVbox = new VBoxContainer();
00118:             centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00119:             centerMargin.AddChild(centerVbox);
00120:             centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("STUDY DESK & READER SELECTION"));
00121:             var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
00122:             _studyDesk = new VBoxContainer();
00123:             _studyDesk.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00124:             _studyDesk.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00125:             centerScroll.AddChild(_studyDesk);
00126:             centerVbox.AddChild(centerScroll);
00127:             gridRow.AddChild(centerPanel);
00128:
00129:             // Column 3: Reading Logs & Progression
00130:             var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
00131:             rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00132:             rightPanel.SizeFlagsStretchRatio = 0.95f;
00133:             var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
00134:             rightPanel.AddChild(rightMargin);
00135:             var rightVbox = new VBoxContainer();
00136:             rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00137:             rightMargin.AddChild(rightVbox);
00138:             rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("STUDY SESSIONS LOG"));
00139:             var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
00140:             _studyLogContainer = new VBoxContainer();
00141:             _studyLogContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00142:             _studyLogContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00143:             rightScroll.AddChild(_studyLogContainer);
00144:             rightVbox.AddChild(rightScroll);
00145:
00146:             rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00147:             _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent study events.");
00148:             _eventLogLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00149:             rightVbox.AddChild(_eventLogLabel);
00150:
00151:             gridRow.AddChild(rightPanel);
00152:
00153:             _shell.SetContent(gridRow);
00154:             RefreshView();
00155:         }
00156:
00157:         public void Open()
00158:         {
00159:             Visible = true;
00160:             RefreshView();
00161:             QueueRedraw();
00162:         }
00163:
00164:         public void RefreshView()
00165:         {
00166:             if (_manualList == null || _studyDesk == null || _studyLogContainer == null) return;
00167:
00168:             AshfallUiHelpers.EmptyChildren(_manualList);
00169:             AshfallUiHelpers.EmptyChildren(_studyDesk);
00170:             AshfallUiHelpers.EmptyChildren(_studyLogContainer);
00171:
00172:             if (_host == null || _statusRail == null)
00173:             {
00174:                 _manualList.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("No library study session bound", "offline"));
00175:                 _studyDesk.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Study desk offline", "offline"));
00176:                 _studyLogContainer.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Study log unavailable", "offline"));
00177:                 return;
00178:             }
00179:
00180:             var s = _host.System.State;
00181:             var catalog = _host.System.Catalog.Values.ToList();
00182:             int totalManuals = catalog.Count;
00183:             int activeJobs = s.activeJobs.Count;
00184:             int completedJobs = s.completedManualIds.Count;
00185:
00186:             _statusRail.Set("manuals", totalManuals.ToString(), AshfallMetricCard.Criticality.Normal);
00187:             _statusRail.Set("active_study", activeJobs.ToString(), activeJobs > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
00188:             _statusRail.Set("completed", completedJobs.ToString(), AshfallMetricCard.Criticality.Normal);
00189:             _statusRail.Set("study_speed", "1.25x", AshfallMetricCard.Criticality.Normal);
00190:             _statusRail.Set("status", activeJobs > 0 ? "STUDYING" : "IDLE", AshfallMetricCard.Criticality.Normal);
00191:
00192:             if (!string.IsNullOrEmpty(_host.LastEvent))
00193:             {
00194:                 _eventLogLabel.Text = _host.LastEvent;
00195:             }
00196:
00197:             if (catalog.Count == 0)
00198:             {
00199:                 _manualList.AddChild(AshfallUiHelpers.MakeMetadata("No manuals cataloged in library archive."));
00200:             }
00201:             else
00202:             {
00203:                 if (_selectedManualId == null || !catalog.Exists(m => m.manual_id == _selectedManualId))
00204:                 {
00205:                     _selectedManualId = catalog[0].manual_id;
00206:                 }
00207:
00208:                 foreach (var manual in catalog)
00209:                 {
00210:                     bool isCompleted = s.completedManualIds.Contains(manual.manual_id);
00211:                     bool isInProgress = s.activeJobs.Exists(j => j.manualId == manual.manual_id);
00212:
00213:                     var card = AshfallUiHelpers.MakePanel();
00214:                     var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
00215:                     card.AddChild(cardMargin);
00216:                     var cardVbox = new VBoxContainer();
00217:                     cardVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
00218:                     cardMargin.AddChild(cardVbox);
00219:
00220:                     var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
00221:                     headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon(isCompleted ? "badge_crossing_terms" : "badge_scurvy", 18));
00222:                     var nameLbl = AshfallUiHelpers.MakeBody(manual.display_name);
00223:                     nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00224:                     headerRow.AddChild(nameLbl);
00225:                     cardVbox.AddChild(headerRow);
00226:
00227:                     var skillLbl = AshfallUiHelpers.MakeMono($"DISCIPLINE: {manual.category} ({manual.studyHoursRequired}h Study)");
00228:                     skillLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00229:                     cardVbox.AddChild(skillLbl);
00230:
00231:                     var statusLbl = AshfallUiHelpers.MakeSmall(isCompleted ? "STATUS: [COMPLETED]" : isInProgress ? "STATUS: [IN PROGRESS]" : "STATUS: [AVAILABLE]");
00232:                     statusLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(isCompleted ? DesignTheme.Lethe : isInProgress ? DesignTheme.Hot : DesignTheme.Pale));
00233:                     cardVbox.AddChild(statusLbl);
00234:
00235:                     var selectBtn = AshfallUiHelpers.MakeButton($"SELECT // {manual.manual_id}", () =>
00236:                     {
00237:                         _selectedManualId = manual.manual_id;
00238:                         RefreshView();
00239:                     });
00240:                     selectBtn.CustomMinimumSize = new Vector2(0, 24);
00241:                     cardVbox.AddChild(selectBtn);
00242:
00243:                     _manualList.AddChild(card);
00244:                 }
00245:             }
00246:
00247:             // Study Desk Inspector
00248:             var curManual = catalog.FirstOrDefault(m => m.manual_id == _selectedManualId);
00249:             if (curManual != null)
00250:             {
00251:                 bool isCompleted = s.completedManualIds.Contains(curManual.manual_id);
00252:                 var activeJob = s.activeJobs.FirstOrDefault(j => j.manualId == curManual.manual_id);
00253:
00254:                 _studyDesk.AddChild(AshfallUiHelpers.MakeSectionHeader($"MANUAL: {curManual.display_name.ToUpperInvariant()}"));
00255:                 _studyDesk.AddChild(AshfallUiHelpers.MakeDataRow("Manual ID", curManual.manual_id, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00256:                 _studyDesk.AddChild(AshfallUiHelpers.MakeDataRow("Target Discipline", curManual.category, AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
00257:                 _studyDesk.AddChild(AshfallUiHelpers.MakeDataRow("Study Duration", $"{curManual.studyHoursRequired}h (Est. {Math.Ceiling(curManual.studyHoursRequired / 8.0f)} Standard Days)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00258:
00259:                 string source = !string.IsNullOrEmpty(curManual.originFacility)
00260:                     ? curManual.originFacility
00261:                     : curManual.expeditionRewardIds.Count > 0
00262:                         ? string.Join(", ", curManual.expeditionRewardIds)
00263:                         : curManual.traderPoolIds.Count > 0
00264:                             ? string.Join(", ", curManual.traderPoolIds)
00265:                             : "Shelter Archives";
00266:                 _studyDesk.AddChild(AshfallUiHelpers.MakeDataRow("Acquisition Source", source, AshfallUiHelpers.ToColor(DesignTheme.Muted)));
00267:
00268:                 string prereqs = curManual.prerequisites.Count > 0
00269:                     ? string.Join(", ", curManual.prerequisites)
00270:                     : "None (Entry-Level)";
00271:                 _studyDesk.AddChild(AshfallUiHelpers.MakeDataRow("Prerequisites", prereqs, AshfallUiHelpers.ToColor(curManual.prerequisites.All(p => s.completedManualIds.Contains(p)) ? DesignTheme.Pale : DesignTheme.Warning)));
00272:
00273:                 string unlocks = curManual.knowledgeUnlocks.Count > 0
00274:                     ? string.Join(", ", curManual.knowledgeUnlocks)
00275:                     : "None";
00276:                 _studyDesk.AddChild(AshfallUiHelpers.MakeDataRow("Knowledge Reveals", unlocks, AshfallUiHelpers.ToColor(DesignTheme.Hot)));
00277:
00278:                 _studyDesk.AddChild(AshfallUiHelpers.MakeDataRow("Archival Status", isCompleted ? "Fully Mastered & Transcribed" : activeJob != null ? $"Under Study ({activeJob.progressHours:F0}/{curManual.studyHoursRequired}h)" : "On Shelf", AshfallUiHelpers.ToColor(isCompleted ? DesignTheme.Lethe : activeJob != null ? DesignTheme.Hot : DesignTheme.Dim)));
00279:
00280:                 _studyDesk.AddChild(AshfallUiHelpers.MakeSeparator());
00281:                 _studyDesk.AddChild(AshfallUiHelpers.MakeSubsectionHeader("READER STATUS"));
00282:
00283:                 if (isCompleted)
00284:                 {
00285:                     _studyDesk.AddChild(AshfallUiHelpers.MakeBody("Manual has been fully mastered and transcribed across the shelter."));
00286:                 }
00287:                 else if (activeJob != null)
00288:                 {
00289:                     float compRate = _host.System.GetComprehensionRate(activeJob.readerId, curManual.manual_id);
00290:                     float estDays = _host.System.GetEstimatedDays(activeJob.readerId, curManual.manual_id);
00291:                     _studyDesk.AddChild(AshfallUiHelpers.MakeBody($"Reader {activeJob.readerId.ToUpperInvariant()} assigned ({activeJob.progressHours:F0}/{curManual.studyHoursRequired}h)."));
00292:                     _studyDesk.AddChild(AshfallUiHelpers.MakeSmall($"Comprehension Rate: {compRate:F2}x | Est. Remaining: {estDays:F0} days."));
00293:                 }
00294:                 else
00295:                 {
00296:                     bool prereqsMet = curManual.prerequisites.All(p => s.completedManualIds.Contains(p));
00297:                     if (!prereqsMet)
00298:                     {
00299:                         _studyDesk.AddChild(AshfallUiHelpers.MakeBody("LOCKED: Missing prerequisite manuals in archival collection."));
00300:                     }
00301:                     else
00302:                     {
00303:                         _studyDesk.AddChild(AshfallUiHelpers.MakeBody("Manual is available on archive shelf for reader study assignment."));
00304:                     }
00305:                 }
00306:             }
00307:             else
00308:             {
00309:                 _studyDesk.AddChild(AshfallUiHelpers.MakeMetadata("Select a manual from the library archive to assign reader."));
00310:             }
00311:
00312:             // Study Sessions Log
00313:             if (s.activeJobs.Count == 0 && s.completedManualIds.Count == 0)
00314:             {
00315:                 _studyLogContainer.AddChild(AshfallUiHelpers.MakeMetadata("No active study sessions."));
00316:             }
00317:             else
00318:             {
00319:                 foreach (var job in s.activeJobs)
00320:                 {
00321:                     _studyLogContainer.AddChild(AshfallUiHelpers.MakeMono($"[STUDYING] {job.manualId} by {job.readerId} ({job.progressHours:F0}h logged)"));
00322:                 }
00323:                 foreach (var doneId in s.completedManualIds)
00324:                 {
00325:                     _studyLogContainer.AddChild(AshfallUiHelpers.MakeMono($"[MASTERED] {doneId}"));
00326:                 }
00327:             }
00328:         }
00329:
00330:         public override void _UnhandledInput(InputEvent @event)
00331:         {
00332:             if (!Visible) return;
00333:             if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00334:             {
00335:                 OnClose?.Invoke();
00336:                 Visible = false;
00337:                 GetViewport().SetInputAsHandled();
00338:             }
00339:         }
00340:
00341:         public override void _ExitTree()
00342:         {
00343:             Unbind();
00344:             base._ExitTree();
00345:         }
00346:     }
00347: }
```

## `src/Main.ShelterBatch3.cs` — 467 lines; 24,198 bytes; SHA-256 `641492ff2b80e3c6ae9004000c647a0d8401a3a46b16ea5f9300fab2c3d91338`
Declaration index:
- 00022: public partial class Main : Control
- 00062: private void SetupSumpFlooding()
- 00097: private void SetupDecontamination()
- 00121: private void SetupKitchenNutrition()
- 00176: private void SetupEquipmentCondition()
- 00199: private void SetupLibraryStudy(ResearchSystem sharedResearch)
- 00225: private void SetupArchiveDesk()
- 00246: private void SetupContractorRoster()
- 00266: private void SetupMentalHealthCrisis()
- 00322: private void SetupShelterAssignment()
- 00350: private void SetupShelterDecor()
- 00389: private void SaveSumpFlooding()
- 00394: private void SaveDecontamination()
- 00399: private void SaveKitchenNutrition()
- 00404: private void SaveEquipmentCondition()
- 00409: private void SaveLibraryStudy()
- 00414: private void SaveArchiveDesk()
- 00419: private void SaveContractorRoster()
- 00424: private void SaveMentalHealthCrisis()
- 00429: private void SaveChemicalDependency()
- 00434: private void SaveShelterAssignment()
- 00460: private void SaveShelterDecor()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Inventory;
00008: using Ashfall.Core.Medical;
00009: using Ashfall.Core.Radiation;
00010: using Ashfall.Core.Shelter;
00011: using Ashfall.Core.StartingLevel;
00012: using Ashfall.Core.Survivors;
00013: using Ashfall.Core.YearOfAsh;
00014: using Ashfall.Core.World;
00015: using Ashfall.Core.Crafting;
00016: using Ashfall.Core.Journal;
00017: using Ashfall.Core.Expeditions;
00018: using AtomicWar.GodotApp.UI;
00019:
00020: namespace AtomicWar.GodotApp
00021: {
00022:     public partial class Main : Control
00023:     {
00024:         // ── 8 Batch-3 Host Sessions (Phase 13) ──
00025:         private SumpFloodingHostSession _sumpFlooding = null!;
00026:         private DecontaminationHostSession _decontamination = null!;
00027:         private KitchenNutritionHostSession _kitchenNutrition = null!;
00028:         private EquipmentConditionHostSession _equipmentCondition = null!;
00029:         private LibraryStudyHostSession _libraryStudy = null!;
00030:         private ArchiveDeskHostSession _archiveDesk = null!;
00031:         private ContractorRosterHostSession _contractorRoster = null!;
00032:         private MentalHealthCrisisHostSession _mentalHealthCrisis = null!;
00033:         private ChemicalDependencyHostSession _chemicalDependency = null!;
00034:
00035:         // Phantom / Traveling / ShelterAssignment (created inside MentalHealth/Assignment)
00036:         private PhantomMemoryPanel _phantomMemoryPanel = null!;
00037:         private TravelingCaravanPanel _travelingCaravanPanel = null!;
00038:         private TravelingCaravanHostSession _travelingCaravan = null!;
00039:         private ShelterAssignmentHostSession _shelterAssignment = null!;
00040:         private ShelterDecorHostSession _shelterDecor = null!;
00041:         private ShelterDecorPanel _shelterDecorPanel = null!;
00042:
00043:         private SumpFloodingPanel _sumpFloodingPanel = null!;
00044:         private DecontaminationPanel _decontaminationPanel = null!;
00045:         private KitchenNutritionPanel _kitchenNutritionPanel = null!;
00046:         private EquipmentConditionPanel _equipmentConditionPanel = null!;
00047:         private LibraryStudyPanel _libraryStudyPanel = null!;
00048:         private ArchiveDeskPanel _archiveDeskPanel = null!;
00049:         private ContractorRosterPanel _contractorRosterPanel = null!;
00050:         private MentalHealthCrisisPanel _mentalHealthCrisisPanel = null!;
00051:         private ChemicalDependencyPanel _chemicalDependencyPanel = null!;
00052:
00053:         private bool _sumpFloodingDirty;
00054:         private bool _decontaminationDirty;
00055:         private bool _kitchenNutritionDirty;
00056:         private bool _equipmentConditionDirty;
00057:         private bool _libraryStudyDirty;
00058:         private bool _archiveDeskDirty;
00059:         private bool _contractorRosterDirty;
00060:         private bool _mentalHealthCrisisDirty;
00061:
00062:         private void SetupSumpFlooding()
00063:         {
00064:             if (_sumpFlooding != null) return;
00065:             SetupCampaignDay();
00066:             var sfState = SumpFloodingSaveStore.TryLoad() ?? new SumpFloodingState();
00067:             var sfWeather = _world.Weather;
00068:             var sfPower = _powerGrid.System;
00069:             var sfDeepFreeze = new YearOfAshDeepFreezeSystem();
00070:             var sfSys = new SumpFloodingSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 10), sfWeather, sfPower, sfDeepFreeze, new GodotLog());
00071:             // Plan 70: parameterize drainage ingress/silt/pump-load from the
00072:             // sump_drainage_catalog.json authority (strata stay optional —
00073:             // nodes without a bound stratum keep the legacy inflow model).
00074:             var sumpStrata = Ashfall.Core.SumpDrainageCatalogLoader.Load(
00075:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00076:             sfSys.ApplyStratumCatalog(sumpStrata);
00077:             sfSys.RestoreState(sfState);
00078:             _sumpFlooding = new SumpFloodingHostSession(sfSys, sfWeather, sfPower, sfDeepFreeze);
00079:             if (_sumpFloodingPanel != null && _sumpFloodingPanel.IsInsideTree())
00080:                 RemoveChild(_sumpFloodingPanel);
00081:             _sumpFloodingPanel = new SumpFloodingPanel();
00082:             _sumpFloodingPanel.Bind(_sumpFlooding);
00083:             _sumpFloodingPanel.Visible = false;
00084:             AddChild(_sumpFloodingPanel);
00085:             // Plan 70: bind the sludge-plant console to the same session
00086:             // while retaining the bulk panel pass's ownership and close wiring.
00087:             if (_slurryDewateringSumpPanel == null || !GodotObject.IsInstanceValid(_slurryDewateringSumpPanel))
00088:             {
00089:                 _slurryDewateringSumpPanel = new SlurryDewateringSumpPanel();
00090:                 _slurryDewateringSumpPanel.OnClose += () => _slurryDewateringSumpPanel.Visible = false;
00091:                 AddChild(_slurryDewateringSumpPanel);
00092:             }
00093:             _slurryDewateringSumpPanel.Bind(_sumpFlooding);
00094:             _slurryDewateringSumpPanel.Visible = false;
00095:         }
00096:
00097:         private void SetupDecontamination()
00098:         {
00099:             if (_decontamination != null) return;
00100:             SetupCampaignDay();
00101:             var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
00102:             var dcJson = new SystemTextJsonSerializer();
00103:             // Plan 78: data-driven decon protocols (wash stages, effluent rules).
00104:             var dcCatalog = DeconProtocolCatalogLoader.Load(_dataDir, fileIO, dcJson);
00105:             var dcState = DecontaminationSaveStore.TryLoad() ?? new DecontaminationState();
00106:             var dcInv = _inventory.Inventory;
00107:             var dcRad = _survivors.Radiation;
00108:             var dcAirlock = _airlockSecurity.System;
00109:             var dcStarting = _startingLevel.System;
00110:             var dcSys = new DecontaminationSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 11), dcRad, dcInv, dcAirlock, dcStarting, dcCatalog, new GodotLog());
00111:             dcSys.RestoreState(dcState);
00112:             _decontamination = new DecontaminationHostSession(dcSys, dcRad, dcInv, dcAirlock, dcStarting);
00113:             if (_decontaminationPanel != null && _decontaminationPanel.IsInsideTree())
00114:                 RemoveChild(_decontaminationPanel);
00115:             _decontaminationPanel = new DecontaminationPanel();
00116:             _decontaminationPanel.Bind(_decontamination);
00117:             _decontaminationPanel.Visible = false;
00118:             AddChild(_decontaminationPanel);
00119:         }
00120:
00121:         private void SetupKitchenNutrition()
00122:         {
00123:             if (_kitchenNutrition != null) return;
00124:             SetupCampaignDay();
00125:             var knState = KitchenNutritionSaveStore.TryLoad() ?? new KitchenNutritionState();
00126:             var knInv = _inventory.Inventory;
00127:             var knNeeds = _survivors.Needs;
00128:             var knSys = new KitchenNutritionSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 12), knInv, knNeeds, new GodotLog());
00129:             knSys.RestoreState(knState);
00130:             // Plan 24A: cooking is labor and must consume the same survivor
00131:             // fitness projection as duty and expedition dispatch. The Core
00132:             // kitchen remains the mutation authority; this is only its host
00133:             // binding to the existing survivor state.
00134:             knSys.SurvivorFitnessProvider = EvaluateSurvivorFitness;
00135:             knSys.CookFitnessProvider = survivorId =>
00136:                 EvaluateDutyRoleFitness(survivorId, DutyRosterIds.RoleMess);
00137:             // Plan 24B A2 — the shared worker-productivity seam: the cook's
00138:             // skill/fitness/overwork verdict resolves through the ONE campaign
00139:             // contract with the mess role's data-authored skill id. Unbound
00140:             // catalog → null resolver → exact legacy kitchen behavior.
00141:             SetupFitnessForDuty();
00142:             string messSkillId = string.Empty;
00143:             if (_fitnessRoleCatalog != null
00144:                 && _fitnessRoleCatalog.TryGetRole(DutyRosterIds.RoleMess, out var messRole))
00145:                 messSkillId = messRole.SkillId;
00146:             if (!string.IsNullOrEmpty(messSkillId))
00147:             {
00148:                 string skillId = messSkillId;
00149:                 knSys.CookProductivityResolver = cookId =>
00150:                     EnsureWorkerProductivityContract().Resolve(cookId, skillId);
00151:             }
00152:             _kitchenNutrition = new KitchenNutritionHostSession(knSys, knInv, knNeeds);
00153:             if (_kitchenNutritionPanel != null && _kitchenNutritionPanel.IsInsideTree())
00154:                 RemoveChild(_kitchenNutritionPanel);
00155:             _kitchenNutritionPanel = new KitchenNutritionPanel();
00156:             _kitchenNutritionPanel.DefaultSurvivorResolver = () =>
00157:             {
00158:                 SetupSurvivors();
00159:                 SetupDutyRoster();
00160:                 string assignedCook = _dutyRoster?.Roster.GetAssignment(DutyRosterIds.RoleMess);
00161:                 if (!string.IsNullOrEmpty(assignedCook)
00162:                     && _survivors.Needs.Get(assignedCook)?.IsAliveState == true)
00163:                     return assignedCook;
00164:                 return _holdfastRuntime?.PlayerSurvivorId ?? _survivors.RosterState.FirstOrDefault(s => s != null && s.IsAliveState)?.Id;
00165:             };
00166:             _kitchenNutritionPanel.LivingSurvivorsResolver = () =>
00167:             {
00168:                 SetupSurvivors();
00169:                 return _survivors.RosterState.Where(s => s != null && s.IsAliveState).Select(s => s.Id).ToList();
00170:             };
00171:             _kitchenNutritionPanel.Bind(_kitchenNutrition);
00172:             _kitchenNutritionPanel.Visible = false;
00173:             AddChild(_kitchenNutritionPanel);
00174:         }
00175:
00176:         private void SetupEquipmentCondition()
00177:         {
00178:             if (_equipmentCondition != null) return;
00179:             SetupCampaignDay();
00180:             var ecState = EquipmentConditionSaveStore.TryLoad() ?? new EquipmentConditionState();
00181:             var ecInv = _inventory.Inventory;
00182:             var ecCrafting = _crafting.Engine;
00183:             var ecSys = new EquipmentConditionSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 13), ecInv, ecCrafting, new GodotLog());
00184:             ecSys.RestoreState(ecState);
00185:             _equipmentCondition = new EquipmentConditionHostSession(ecSys, ecInv, ecCrafting);
00186:             _equipmentCondition.LoadCatalog(_dataDir);
00187:             // Combat projects its default weapon loadout from this authority
00188:             // and writes engagement wear back here (WeaponEquipmentBridge).
00189:             if (_combat != null)
00190:                 _combat.Equipment = ecSys;
00191:             if (_equipmentConditionPanel != null && _equipmentConditionPanel.IsInsideTree())
00192:                 RemoveChild(_equipmentConditionPanel);
00193:             _equipmentConditionPanel = new EquipmentConditionPanel();
00194:             _equipmentConditionPanel.Bind(_equipmentCondition);
00195:             _equipmentConditionPanel.Visible = false;
00196:             AddChild(_equipmentConditionPanel);
00197:         }
00198:
00199:         private void SetupLibraryStudy(ResearchSystem sharedResearch)
00200:         {
00201:             if (_libraryStudy != null) return;
00202:             SetupDutyRoster();
00203:             _expandedShelterRoster = _dutyRoster.Roster;
00204:             var lsState = LibraryStudySaveStore.TryLoad() ?? new LibraryStudyState();
00205:             var lsSkills = EnsureSharedSkillProgression();
00206:             var lsResearch = sharedResearch;
00207:             var lsJournal = _journal;
00208:             var lsSys = new LibraryStudySystem(lsSkills, lsResearch, lsJournal, _expandedShelterRoster, new GodotLog());
00209:             lsSys.RestoreState(lsState);
00210:             // C2[6] 23A: authored `requires_power` manuals now actually gate on the
00211:             // real grid (previously dead data). The library/study desks share the
00212:             // laboratory-research electrical bus.
00213:             lsSys.PowerAvailable = () =>
00214:                 _powerGrid?.System == null || _powerGrid.System.IsRoomServed("room_laboratory_research");
00215:             _libraryStudy = new LibraryStudyHostSession(lsSys, lsSkills, lsResearch, lsJournal, _expandedShelterRoster);
00216:             _libraryStudy.LoadCatalog(_dataDir);
00217:             if (_libraryStudyPanel != null && _libraryStudyPanel.IsInsideTree())
00218:                 RemoveChild(_libraryStudyPanel);
00219:             _libraryStudyPanel = new LibraryStudyPanel();
00220:             _libraryStudyPanel.Bind(_libraryStudy);
00221:             _libraryStudyPanel.Visible = false;
00222:             AddChild(_libraryStudyPanel);
00223:         }
00224:
00225:         private void SetupArchiveDesk()
00226:         {
00227:             if (_archiveDesk != null) return;
00228:             SetupDutyRoster();
00229:             _expandedShelterRoster = _dutyRoster.Roster;
00230:             var adState = ArchiveDeskSaveStore.TryLoad() ?? new ArchiveDeskState();
00231:             var adJournal = _journal;
00232:             var adKnowledge = new KnowledgeBase();
00233:             var adInv = _inventory.Inventory;
00234:             var adSys = new ArchiveDeskSystem(adJournal, adKnowledge, adInv, _expandedShelterRoster, new GodotLog());
00235:             adSys.RestoreState(adState);
00236:             _archiveDesk = new ArchiveDeskHostSession(adSys, adJournal, adKnowledge, adInv, _expandedShelterRoster);
00237:             _archiveDesk.LoadInkCatalog(_dataDir);
00238:             if (_archiveDeskPanel != null && _archiveDeskPanel.IsInsideTree())
00239:                 RemoveChild(_archiveDeskPanel);
00240:             _archiveDeskPanel = new ArchiveDeskPanel();
00241:             _archiveDeskPanel.Bind(_archiveDesk);
00242:             _archiveDeskPanel.Visible = false;
00243:             AddChild(_archiveDeskPanel);
00244:         }
00245:
00246:         private void SetupContractorRoster()
00247:         {
00248:             if (_contractorRoster != null) return;
00249:             SetupDutyRoster();
00250:             _expandedShelterRoster = _dutyRoster.Roster;
00251:             SetupCampaignDay();
00252:             var crState = ContractorRosterSaveStore.TryLoad() ?? new ContractorRosterState();
00253:             var crInv = _inventory.Inventory;
00254:             var crExpedition = _expeditions.Engine;
00255:             var crSys = new ContractorRosterSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 14), crInv, _expandedShelterRoster, crExpedition, new GodotLog());
00256:             crSys.RestoreState(crState);
00257:             _contractorRoster = new ContractorRosterHostSession(crSys, crInv, _expandedShelterRoster, crExpedition);
00258:             if (_contractorRosterPanel != null && _contractorRosterPanel.IsInsideTree())
00259:                 RemoveChild(_contractorRosterPanel);
00260:             _contractorRosterPanel = new ContractorRosterPanel();
00261:             _contractorRosterPanel.Bind(_contractorRoster);
00262:             _contractorRosterPanel.Visible = false;
00263:             AddChild(_contractorRosterPanel);
00264:         }
00265:
00266:         private void SetupMentalHealthCrisis()
00267:         {
00268:             if (_mentalHealthCrisis != null) return;
00269:             SetupDutyRoster();
00270:             _expandedShelterRoster = _dutyRoster.Roster;
00271:             SetupCampaignDay();
00272:             var mhState = MentalHealthCrisisSaveStore.TryLoad() ?? new MentalHealthState();
00273:             var mhNeeds = _survivors.Needs;
00274:             var mhMedical = _medicalWard;
00275:             // Task #133: one shared chem-dep authority — the MedicalHostSession
00276:             // engine (already loaded + merged from both save sections). The
00277:             // chemical_dependency save section is written from this same shared
00278:             // engine by SaveChemicalDependency, keeping it in sync.
00279:             SetupMedical();
00280:             _chemicalDependency = new ChemicalDependencyHostSession(_medical.Engine);
00281:             // Task #133 P1b: share the pipeline when already bound; otherwise
00282:             // EnsureMedicalPipeline backfills this reference once it runs.
00283:             _chemicalDependency.Pipeline = _medical.Pipeline;
00284:             var mhSys = new MentalHealthCrisisSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Psychology, 0, 15), mhNeeds, mhMedical, _chemicalDependency.System, _expandedShelterRoster, new GodotLog());
00285:             mhSys.RestoreState(mhState);
00286:             _mentalHealthCrisis = new MentalHealthCrisisHostSession(mhSys, mhNeeds, mhMedical, _chemicalDependency.System, _expandedShelterRoster);
00287:             if (_mentalHealthCrisisPanel != null && _mentalHealthCrisisPanel.IsInsideTree())
00288:                 RemoveChild(_mentalHealthCrisisPanel);
00289:             _mentalHealthCrisisPanel = new MentalHealthCrisisPanel();
00290:             _mentalHealthCrisisPanel.Bind(_mentalHealthCrisis);
00291:             _mentalHealthCrisisPanel.Visible = false;
00292:             AddChild(_mentalHealthCrisisPanel);
00293:
00294:             // Task #133: no separate restore here — the shared engine was already
00295:             // loaded and merged by MedicalHostSession.Create. Re-restoring the
00296:             // legacy section would wipe canonical medical-ledger rows.
00297:             if (_chemicalDependencyPanel != null && _chemicalDependencyPanel.IsInsideTree())
00298:                 RemoveChild(_chemicalDependencyPanel);
00299:             _chemicalDependencyPanel = new ChemicalDependencyPanel();
00300:             _chemicalDependencyPanel.Bind(_chemicalDependency);
00301:             _chemicalDependencyPanel.Visible = false;
00302:             AddChild(_chemicalDependencyPanel);
00303:
00304:             if (_phantomMemory == null) SetupPhantom();
00305:             if (_phantomMemoryPanel != null && _phantomMemoryPanel.IsInsideTree())
00306:                 RemoveChild(_phantomMemoryPanel);
00307:             _phantomMemoryPanel = new PhantomMemoryPanel();
00308:             if (_phantomMemory != null) _phantomMemoryPanel.Bind(_phantomMemory, _inventory);
00309:             _phantomMemoryPanel.Visible = false;
00310:             AddChild(_phantomMemoryPanel);
00311:
00312:             _travelingCaravan = TravelingCaravanHostSession.Create(_dataDir);
00313:             if (_travelingCaravanPanel != null && _travelingCaravanPanel.IsInsideTree())
00314:                 RemoveChild(_travelingCaravanPanel);
00315:             _travelingCaravanPanel = new TravelingCaravanPanel();
00316:             _travelingCaravanPanel.Bind(_travelingCaravan, GetTradeVoiceResolver(),
00317:                 () => _world?.Weather?.Current ?? Ashfall.Core.WeatherKind.Clear);
00318:             _travelingCaravanPanel.Visible = false;
00319:             AddChild(_travelingCaravanPanel);
00320:         }
00321:
00322:         private void SetupShelterAssignment()
00323:         {
00324:             if (_shelterAssignment != null) return;
00325:             SetupCampaignDay();
00326:             // Restore paths may reach shelter assignment before the expanded
00327:             // shelter bundle has rebuilt its thermal authority. Keep this
00328:             // dependency explicit so a restore cannot dereference stale null
00329:             // state after the in-memory lifecycle reset.
00330:             if (_shelterThermal == null)
00331:                 SetupShelterThermal();
00332:             if (_campaignDay == null || _shelterThermal == null)
00333:                 return;
00334:             _shelterAssignment = ShelterAssignmentHostSession.CreateDefault(_campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Shelter).Rng);
00335:             if (!_shelterAssignment.TryLoad())
00336:             {
00337:             }
00338:             _shelterThermal.SetAssignments(_shelterAssignment.System);
00339:             SetupPhase0();
00340:             _phase0.BindShelterAssignment(_shelterAssignment.System);
00341:         }
00342:
00343:         /// <summary>
00344:         /// Plan 12C final host triad. Placements restore from the campaign
00345:         /// envelope; item modifiers are rebuilt from the authoritative live
00346:         /// item catalog, never copied into a save. Existing memorial entries
00347:         /// are reconciled once so saves authored before this panel gain their
00348:         /// wall records deterministically.
00349:         /// </summary>
00350:         private void SetupShelterDecor()
00351:         {
00352:             if (_shelterDecor != null) return;
00353:
00354:             SetupSurvivors();
00355:             SetupInventory();
00356:             SetupShelterAssignment();
00357:             SetupMemorial();
00358:
00359:             var system = new ShelterDecorSystem();
00360:             var saved = ShelterDecorSaveStore.TryLoad();
00361:             if (saved != null)
00362:                 system.RestoreState(saved.Capture());
00363:
00364:             _shelterDecor = new ShelterDecorHostSession(
00365:                 system,
00366:                 _shelterAssignment.System,
00367:                 _survivors.Needs,
00368:                 _inventory);
00369:             _shelterDecor.SetCurrentDay(_simDay);
00370:             _shelterDecor.LoadCatalogModifiers();
00371:
00372:             // This is intentionally idempotent: only legacy memorial entries
00373:             // without a plaque become new placements, and a duplicate survivor
00374:             // key is never mounted twice.
00375:             foreach (var entry in _memorial.Entries)
00376:             {
00377:                 if (!_shelterDecor.TryMountMemorialPlaque(entry, out var reason))
00378:                     GD.PushWarning("[Ashfall Godot] Memorial plaque reconcile skipped: " + reason);
00379:             }
00380:
00381:             if (_shelterDecorPanel != null && _shelterDecorPanel.IsInsideTree())
00382:                 RemoveChild(_shelterDecorPanel);
00383:             _shelterDecorPanel = new ShelterDecorPanel();
00384:             _shelterDecorPanel.Bind(_shelterDecor);
00385:             _shelterDecorPanel.Visible = false;
00386:             AddChild(_shelterDecorPanel);
00387:         }
00388:
00389:         private void SaveSumpFlooding()
00390:         {
00391:             if (_sumpFlooding != null)
00392:                 CaptureSection("sump_flooding", SumpFloodingSaveStore.TryCapturePersisted(_sumpFlooding.System.CaptureState()));
00393:         }
00394:         private void SaveDecontamination()
00395:         {
00396:             if (_decontamination != null)
00397:                 CaptureSection("decontamination", DecontaminationSaveStore.TryCapturePersisted(_decontamination.System.CaptureState()));
00398:         }
00399:         private void SaveKitchenNutrition()
00400:         {
00401:             if (_kitchenNutrition != null)
00402:                 CaptureSection("kitchen_nutrition", KitchenNutritionSaveStore.TryCapturePersisted(_kitchenNutrition.System.CaptureState()));
00403:         }
00404:         private void SaveEquipmentCondition()
00405:         {
00406:             if (_equipmentCondition != null)
00407:                 CaptureSection("equipment_condition", EquipmentConditionSaveStore.TryCapturePersisted(_equipmentCondition.System.CaptureState()));
00408:         }
00409:         private void SaveLibraryStudy()
00410:         {
00411:             if (_libraryStudy != null)
00412:                 CaptureSection("library_study", LibraryStudySaveStore.TryCapturePersisted(_libraryStudy.System.CaptureState()));
00413:         }
00414:         private void SaveArchiveDesk()
00415:         {
00416:             if (_archiveDesk != null)
00417:                 CaptureSection("archive_desk", ArchiveDeskSaveStore.TryCapturePersisted(_archiveDesk.System.CaptureState()));
00418:         }
00419:         private void SaveContractorRoster()
00420:         {
00421:             if (_contractorRoster != null)
00422:                 CaptureSection("contractor_roster", ContractorRosterSaveStore.TryCapturePersisted(_contractorRoster.System.CaptureState()));
00423:         }
00424:         private void SaveMentalHealthCrisis()
00425:         {
00426:             if (_mentalHealthCrisis != null)
00427:                 CaptureSection("mental_health_crisis", MentalHealthCrisisSaveStore.TryCapturePersisted(_mentalHealthCrisis.System.CaptureState()));
00428:         }
00429:         private void SaveChemicalDependency()
00430:         {
00431:             if (_chemicalDependency != null)
00432:                 CaptureSection("chemical_dependency", ChemicalDependencySaveStore.TryCapturePersisted(_chemicalDependency.System.CaptureState()));
00433:         }
00434:         private void SaveShelterAssignment()
00435:         {
00436:             if (_shelterAssignment == null) return;
00437:
00438:             var save = new ShelterAssignmentSave
00439:             {
00440:                 simDay = 0,
00441:                 Rooms = new List<ShelterRoomSave>(),
00442:                 State = _shelterAssignment.System.CaptureState()
00443:             };
00444:             foreach (var room in _shelterAssignment.System.Rooms)
00445:             {
00446:                 save.Rooms.Add(new ShelterRoomSave
00447:                 {
00448:                     RoomId = room.RoomId,
00449:                     DisplayName = room.DisplayName,
00450:                     Capacity = room.Capacity,
00451:                     RequiredSkillId = room.RequiredSkillId,
00452:                     WorkstationId = room.WorkstationId
00453:                 });
00454:             }
00455:
00456:             if (CaptureSection("shelter_assignment", ShelterAssignmentSaveStore.TryCapturePersisted(save)))
00457:                 _shelterAssignment.ClearDirty();
00458:         }
00459:
00460:         private void SaveShelterDecor()
00461:         {
00462:             if (_shelterDecor == null) return;
00463:             if (CaptureSection("shelter_decor", ShelterDecorSaveStore.TryCapturePersisted(_shelterDecor.System.CaptureState())))
00464:                 _shelterDecor.ClearDirty();
00465:         }
00466:     }
00467: }
```

## `src/Main.ExpandedShelterSystems.cs` — 898 lines; 42,067 bytes; SHA-256 `a6f038a1dc347c2ab4b861767374e75aa2a79d980a2e6a7b71396061978c3da2`
Declaration index:
- 00022: public partial class Main : Control
- 00040: private ResearchSystem EnsureSharedResearch()
- 00058: private void OnSharedResearchCompleted(ResearchKnowledgeDef def)
- 00089: private void SetupExpandedShelterSystems()
- 00175: private void BindElectrostaticScrubberPanel()
- 00188: private void WireWaterTreatmentSumpBridge()
- 00211: private void WireWildlifeDiseaseBridge()
- 00234: private void WireVinylRadioBridge()
- 00253: private void WireAutopsyBridge()
- 00311: private void SaveAllExpandedShelterSystems()
- 00371: private void SaveResearch()
- 00393: private static string LightingPhaseForHour(int hour)
- 00402: private void TickAllExpandedShelterSystems(int day)
- 00427: // on the campaign day owner; practice systems record against the
- 00510: public void OpenExpandedPanel(string panelKey)
- 00725: private void ResetExpandedShelterSessions()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Disease;
00007: using Ashfall.Core.Inventory;
00008: using Ashfall.Core.Medical;
00009: using Ashfall.Core.Radiation;
00010: using Ashfall.Core.Shelter;
00011: using Ashfall.Core.StartingLevel;
00012: using Ashfall.Core.Survivors;
00013: using Ashfall.Core.YearOfAsh;
00014: using Ashfall.Core.World;
00015: using Ashfall.Core.Crafting;
00016: using Ashfall.Core.Journal;
00017: using Ashfall.Core.Expeditions;
00018: using AtomicWar.GodotApp.UI;
00019:
00020: namespace AtomicWar.GodotApp
00021: {
00022:     public partial class Main : Control
00023:     {
00024:         // ── 12 Expanded Shelter Host Sessions ──
00025:
00026:         // Single shared ResearchSystem consulted by autopsy + library study
00027:         // (previously a local var in SetupExpandedShelterSystems, which made it
00028:         // unreachable for the Research panel bind). Created fresh by
00029:         // SetupExpandedShelterSystems; lazily by the research panel route.
00030:         private ResearchSystem _sharedResearch = null!;
00031:
00032:         /// <summary>
00033:         /// Lazily create the shared research engine, restore persisted research
00034:         /// progress, and load the authoritative research_knowledge.json catalog
00035:         /// (Plan 34: JSON is the sole authored research authority). All research
00036:         /// consumers — crafting/workshop, autopsy, library study, research and
00037:         /// atlas panels — must obtain the engine through this method so the
00038:         /// campaign has exactly one research instance.
00039:         /// </summary>
00040:         private ResearchSystem EnsureSharedResearch()
00041:         {
00042:             if (_sharedResearch != null) return _sharedResearch;
00043:             var saved = ResearchSaveStore.TryLoad();
00044:             var engine = new ResearchSystem(log: new GodotLog(), state: saved);
00045:             if (!string.IsNullOrEmpty(_dataDir))
00046:             {
00047:                 var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
00048:                 int count = ResearchKnowledgeCatalogLoader.LoadAndRegister(engine, _dataDir, fileIO, new SystemTextJsonSerializer());
00049:                 if (count == 0)
00050:                     GD.PushWarning($"[Ashfall Godot] research_knowledge.json loaded 0 nodes from {_dataDir} — research content unavailable");
00051:             }
00052:             engine.OnResearchCompleted += OnSharedResearchCompleted;
00053:             _sharedResearch = engine;
00054:             SetupResearchUnlockBridge();
00055:             return engine;
00056:         }
00057:
00058:         private void OnSharedResearchCompleted(ResearchKnowledgeDef def)
00059:         {
00060:             if (def == null) return;
00061:             SetupJournal();
00062:             string bt = !string.IsNullOrEmpty(def.breakthroughItem) ? $" Breakthrough: {def.breakthroughItem}." : string.Empty;
00063:             _journal?.TryAddRawEntry($"research_{def.id}", $"Research completed: {def.displayName}.{bt}", null!, _simDay);
00064:             _journalDirty = true;
00065:             _researchUnlock?.ProcessResearchCompletion(def.id);
00066:         }
00067:
00068:
00069:
00070:         // Batch 4 BUG-14 follow-up: a SINGLE shared duty roster passed to
00071:         // the eight systems above that consult the roster (apprenticeship,
00072:         // library study, archive desk, contractor roster, mental health).
00073:         // Previously each system held a fresh `new DutyRosterSystem()`, so
00074:         // cross-system busy checks (mentor_busy / caregiver_busy) observed
00075:         // an empty per-instance roster and never blocked.
00076:         private DutyRosterSystem _expandedShelterRoster = new DutyRosterSystem();
00077:
00078:         // Promoted from SetupExpandedShelterSystems local — Apprenticeship needs
00079:         // the core SurvivorRelationsSystem instance for its constructor.
00080:         private SurvivorRelationsSystem _survivorRelationsCore = null!;
00081:
00082:         // ── 22 UI Panels ──
00083:
00084:
00085:         // ── Dirty Flags ──
00086:
00087:
00088:
00089:         private void SetupExpandedShelterSystems()
00090:         {
00091:             SetupSurvivors();
00092:             SetupDutyRoster();
00093:             // All shelter work consumers share the persisted campaign duty
00094:             // authority; the former empty helper roster could disagree with
00095:             // quarantine, care, fitness and the player-visible shift chart.
00096:             _expandedShelterRoster = _dutyRoster.Roster;
00097:             SetupInventory();
00098:             SetupPowerGrid();
00099:             SetupJournal();
00100:             SetupCrafting();
00101:             SetupExpeditions();
00102:             SetupMedical();
00103:             SetupMedicalWard();
00104:             SetupStartingLevel();
00105:             SetupWorld();
00106:
00107:             _sharedResearch = EnsureSharedResearch();
00108:
00109:             SetupWaterTreatment();
00110:             SetupAirlockSecurity();
00111:             SetupSurvivorRelations();
00112:             SetupRegionalTreaty();
00113:             SetupVinylMorale();
00114:             SetupWildlifeTrapping();
00115:             SetupExcavation();
00116:             SetupApprenticeship();
00117:             SetupCaregiving();
00118:             SetupShelterThermal();
00119:             SetupWeatherHardening();
00120:             SetupGeothermalAquifer();
00121:             SetupShelterSchedule();
00122:             SetupAutopsy(_sharedResearch);
00123:             SetupWaystation();
00124:             SetupSumpFlooding();
00125:             WireWaterTreatmentSumpBridge();
00126:             WireWildlifeDiseaseBridge();
00127:             WireVinylRadioBridge();
00128:             WireAutopsyBridge();
00129:             SetupDecontamination();
00130:             SetupPlans78To81();
00131:             SetupPlans110To113();
00132:             SetupPlans130To133();
00133:             SetupPlans146To149();
00134:             SetupKitchenNutrition();
00135:             SetupGrainProcessing();
00136:             SetupCryogenicAirSeparation();
00137:             SetupHeliograph();
00138:             SetupEquipmentCondition();
00139:             SetupLibraryStudy(_sharedResearch);
00140:             SetupArchiveDesk();
00141:             SetupContractorRoster();
00142:             SetupMentalHealthCrisis();
00143:             SetupShelterAssignment();   // last — post-wiring to Thermal + Phase0
00144:             SetupShelterDecor();        // uses the final assignment map + inventory catalog
00145:             SetupShelterAtmosphere();
00146:             SetupHiddenAgenda();
00147:             SetupShelterReputation();
00148:             SetupPropaganda();
00149:             SetupRumorNetwork();
00150:             SetupShelterSecurity();
00151:             SetupVisitorIntegration();
00152:             SetupPersonalQuests();
00153:             SetupTimeCapsules();
00154:             SetupDeathLegacy();
00155:             SetupRelationshipDecay();
00156:             SetupCommitments();
00157:             SetupSessionDurability();
00158:             SetupPlayMetrics();
00159:             SetupSurvivorVoice();
00160:             SetupContentCertification();
00161:             SetupSevenDaySlice();
00162:             SetupRetention();
00163:             SetupOutpostSettlement();
00164:             SetupWeatherCascade();
00165:             SetupTerritoryControl();
00166:             SetupNightWatch();
00167:             SetupCooking();
00168:             SetupPresentation();
00169:             SetupOrphanSealWave1();
00170:             SetupNeedsPerformance();
00171:             SetupCampaignLegacy();
00172:         }
00173:
00174:         /// <summary>Binds the Plan 72 electrostatic scrubber console to the ventilation session.</summary>
00175:         private void BindElectrostaticScrubberPanel()
00176:         {
00177:             if (_ventilationHost == null) return;
00178:             if (_electrostaticScrubberPanel == null)
00179:             {
00180:                 _electrostaticScrubberPanel = new ElectrostaticScrubberPanel { Visible = false };
00181:                 _electrostaticScrubberPanel.OnClose += () => _electrostaticScrubberPanel.Visible = false;
00182:                 AddChild(_electrostaticScrubberPanel);
00183:             }
00184:             _electrostaticScrubberPanel.Bind(_ventilationHost);
00185:             _electrostaticScrubberPanel.Visible = false;
00186:         }
00187:
00188:         private void WireWaterTreatmentSumpBridge()
00189:         {
00190:             if (_sumpFlooding == null || _waterTreatment == null) return;
00191:             // Plan 70: bind the sludge-plant consumable inventory (flocculant /
00192:             // filter cloth) and the canonical greywater routing target.
00193:             _sumpFlooding.System.BindServices(_inventory?.Inventory, _waterTreatment.System, _ventilation);
00194:             // Plan 72: bind stage services (seeded arc RNG, real power draw,
00195:             // installation components, radioactive-drum inventory, arc-fire handoff).
00196:             _ventilation?.BindStageServices(
00197:                 _campaignDay?.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 14),
00198:                 _powerGrid?.System,
00199:                 _inventory?.Inventory,
00200:                 _stageFireHazard);
00201:             BindElectrostaticScrubberPanel();
00202:             _sumpFlooding.System.OnIncident += incident =>
00203:             {
00204:                 if (incident.kind == FloodIncidentKind.FloodStart || incident.kind == FloodIncidentKind.Contamination)
00205:                 {
00206:                     _waterTreatment.SetIncomingContamination(0.8f);
00207:                 }
00208:             };
00209:         }
00210:
00211:         private void WireWildlifeDiseaseBridge()
00212:         {
00213:             if (_wildlifeTrapping == null || _disease == null) return;
00214:             // Guard: WildlifeTrappingHostSession.ApplyDisease handles authoritative disease routing.
00215:             // Only wire legacy fallback bridge if ApplyDisease delegate is not configured.
00216:             if (_wildlifeTrapping.ApplyDisease != null) return;
00217:             _wildlifeTrapping.System.OnButcheryCompleted += (siteId, butcherId, species, isToxic) =>
00218:             {
00219:                 if (string.IsNullOrEmpty(butcherId)) return;
00220:                 var def = _survivors?.Roster?.FindDefinition(butcherId);
00221:                 if (def != null && def.traitIds != null && def.traitIds.Contains("skill_sanitization_expert"))
00222:                     return;
00223:                 _disease.Engine.TryExpose(new DiseaseExposureContext
00224:                 {
00225:                     SurvivorId = butcherId,
00226:                     DiseaseId = DiseaseIds.ZoonoticFlu,
00227:                     SourceId = "wildlife_butchery",
00228:                     Day = _simDay,
00229:                     ProbabilityModifier = 1.0f
00230:                 });
00231:             };
00232:         }
00233:
00234:         private void WireVinylRadioBridge()
00235:         {
00236:             if (_vinylMorale == null || _radio == null || _powerGrid == null) return;
00237:             _vinylMorale.System.OnCulturalBroadcast += (record, day) =>
00238:             {
00239:                 // 150W transmitter load — if brownout, cancel broadcast and cut signal
00240:                 if (_powerGrid.System.IsBrownout)
00241:                 {
00242:                     _vinylMorale.System.CancelBroadcastBrownout();
00243:                     return;
00244:                 }
00245:                 // The vinyl relay is presentation-only: the Core event has
00246:                 // already resolved the morale/radio consequence. This cue
00247:                 // must never feed back into simulation state.
00248:                 _audio?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.RadioVinylBroadcast);
00249:                 _radio.RecordCulturalBroadcast(record.record_id, record.genre, record.display_name, day, _vinylMorale.System.State.lastBroadcastSignalStrength);
00250:             };
00251:         }
00252:
00253:         private void WireAutopsyBridge()
00254:         {
00255:             if (_autopsy == null) return;
00256:             _autopsy.System.OnCaseCompleted += c =>
00257:             {
00258:                 string finding = c.finding ?? string.Empty;
00259:                 // Future-proof: keyword-based forensic routing — add new findings without changing host wiring structure
00260:                 if (finding.IndexOf("zoonotic", StringComparison.OrdinalIgnoreCase) >= 0 || finding.IndexOf("influenza", StringComparison.OrdinalIgnoreCase) >= 0 || finding.IndexOf("spore", StringComparison.OrdinalIgnoreCase) >= 0)
00261:                 {
00262:                     if (_disease != null && !string.IsNullOrEmpty(c.assignedMedicId))
00263:                     {
00264:                         _disease.Engine.TryExpose(new DiseaseExposureContext
00265:                         {
00266:                             SurvivorId = c.assignedMedicId,
00267:                             DiseaseId = DiseaseIds.ZoonoticFlu,
00268:                             SourceId = "autopsy_pathogen",
00269:                             Day = _simDay,
00270:                             ProbabilityModifier = 1.0f
00271:                         });
00272:                     }
00273:                 }
00274:                 // Always journal the forensic result for memorial/continuity
00275:                 _journal?.TryAddRawEntry("autopsy_completed", $"Autopsy {c.caseId} ({c.specimenId}): {finding}", null!, _simDay);
00276:                 // Memorialize if system available — use Memorialize with minimal input
00277:                 if (_memorial != null)
00278:                 {
00279:                     try
00280:                     {
00281:                         _memorial.Memorialize(new Ashfall.Core.Memorial.MemorialInput
00282:                         {
00283:                             SurvivorId = c.specimenId,
00284:                             Cause = finding,
00285:                             Day = _simDay,
00286:                             BirthDay = 0,
00287:                             Epitaph = $"Forensic finding: {finding}"
00288:                         });
00289:                     }
00290:                     catch (Exception ex)
00291:                     {
00292:                         // Memorial integration is optional; log warning without blocking autopsy flow.
00293:                         GD.PushWarning($"[Ashfall Godot] Autopsy memorialization failed for {c.specimenId}: {ex.Message}");
00294:                     }
00295:                 }
00296:             };
00297:         }
00298:
00299:
00300:
00301:
00302:
00303:
00304:
00305:
00306:
00307:
00308:
00309:
00310:
00311:         private void SaveAllExpandedShelterSystems()
00312:         {
00313:             SaveWaterTreatment();
00314:             SaveAirlockSecurity();
00315:             SaveSurvivorRelations();
00316:             SaveRegionalTreaty();
00317:             SaveVinylMorale();
00318:             SaveWildlifeTrapping();
00319:             SaveExcavation();
00320:             SaveApprenticeship();
00321:             SaveCaregiving();
00322:             SaveShelterThermal();
00323:             SaveShelterAtmosphere();
00324:             SaveHiddenAgenda();
00325:             SaveShelterReputation();
00326:             SavePropaganda();
00327:             SaveRumorNetwork();
00328:             SaveShelterSecurity();
00329:             SaveVisitorIntegration();
00330:             SaveWeatherHardening();
00331:             SaveGeothermalAquifer();
00332:             SaveShelterSchedule();
00333:             SaveAutopsy();
00334:             SaveWaystation();
00335:             SaveSumpFlooding();
00336:             SaveDecontamination();
00337:             PersistPlans78To81();
00338:             PersistPlans110To113();
00339:             PersistPlans130To133();
00340:             PersistPlans146To149();
00341:             SaveKitchenNutrition();
00342:             SaveGrainProcessing();
00343:             SaveCryogenicAirSeparation();
00344:             SaveHeliograph();
00345:             SaveEquipmentCondition();
00346:             SaveLibraryStudy();
00347:             SaveResearch();
00348:             SaveArchiveDesk();
00349:             SaveContractorRoster();
00350:             SaveMentalHealthCrisis();
00351:             SaveChemicalDependency();
00352:             SaveShelterAssignment();
00353:             SaveShelterDecor();
00354:             SaveFactionBranch();
00355:             SaveCommitments();
00356:             SaveSessionDurability();
00357:             SavePlayMetrics();
00358:             SaveSurvivorVoice();
00359:             SaveSevenDaySlice();
00360:             SaveRetention();
00361:             SaveOutpostSettlement();
00362:             SavePresentation();
00363:             PersistOrphanSealWave1();
00364:             SaveCampaignLegacy();
00365:             SaveResearchUnlock();
00366:             SaveUnifiedEnding();
00367:             SaveNightWatch();
00368:         }
00369:
00370:         /// <summary>Capture research progress into the campaign envelope (Plan 34: research state must round-trip).</summary>
00371:         private void SaveResearch()
00372:         {
00373:             if (_sharedResearch != null)
00374:                 CaptureSection("research", ResearchSaveStore.TryCapturePersisted(_sharedResearch.CaptureState()));
00375:         }
00376:
00377:
00378:
00379:
00380:
00381:
00382:
00383:
00384:
00385:
00386:
00387:
00388:
00389:         /// <summary>
00390:         /// Presentation lighting phase from the campaign hour. Drives the placeholder
00391:         /// dawn/day/dusk/night backdrop variants on shelter, map, and expedition views.
00392:         /// </summary>
00393:         private static string LightingPhaseForHour(int hour)
00394:         {
00395:             int h = ((hour % 24) + 24) % 24;
00396:             if (h >= 5 && h < 9) return "dawn";
00397:             if (h >= 9 && h < 17) return "day";
00398:             if (h >= 17 && h < 21) return "dusk";
00399:             return "night";
00400:         }
00401:
00402:         private void TickAllExpandedShelterSystems(int day)
00403:         {
00404:             // Plan 189 intake bridge: piezometer advisory must land before the
00405:             // water plant ticks so a blocked source refuses intake same-day.
00406:             TickPiezometerAdvisoryBridge(day);
00407:
00408:             // C2[6] 23A: water treatment spans two real loads. Extraction follows
00409:             // room_water_pump; processing follows room_water_filtration (previously an
00410:             // unread critical catalog row). Allocation-aware: a brownout that still
00411:             // serves both buses keeps the plant at full throughput; one bus served is
00412:             // partial; neither pauses treatment (the passive path remains).
00413:             bool pumpServed = _powerGrid?.System == null
00414:                 || _powerGrid.System.IsRoomServed("room_water_pump");
00415:             bool filtrationServed = _powerGrid?.System == null
00416:                 || _powerGrid.System.IsRoomServed("room_water_filtration");
00417:             float waterPower = !pumpServed ? 0f : (filtrationServed ? 1f : 0.5f);
00418:             _waterTreatment?.TickDay(day, waterPower);
00419:             _airlockSecurity?.TickDay(day);
00420:             _survivorRelations?.TickDay(day);
00421:             _regionalTreaty?.TickDay(day);
00422:             _vinylMorale?.TickDay(day);
00423:             RefreshTrappingDensity();
00424:             _wildlifeTrapping?.TickDay(day);
00425:             _excavation?.TickDay();
00426:             // DEBT-185: dormancy decay for the shared skill progression must run
00427:             // on the campaign day owner; practice systems record against the
00428:             // same instance and reactivate dormant skills.
00429:             TickSharedSkillProgression(day);
00430:             _apprenticeship?.TickDay(day);
00431:             _caregiving?.TickDay(day);
00432:             // C2[6] 23A: heating circulation is an electrical load. The generator's
00433:             // waste heat only reaches the radiators while the circulation pump has
00434:             // served room_heating power; no pump power = heat generated but not
00435:             // delivered. Derived from the canonical base generator (external fuel-free
00436:             // sources produce no combustion waste heat).
00437:             if (_shelterThermal != null && _powerGrid?.System != null)
00438:             {
00439:                 bool heatingPowered = _powerGrid.System.IsRoomServed("room_heating");
00440:                 float baseGeneratorKw = _powerGrid.System.BaseGenerationWatts
00441:                     * _powerGrid.System.GeneratorOutputFactor / 1000f;
00442:                 _shelterThermal.System.SetGeneratorWasteHeat(baseGeneratorKw, heatingPowered);
00443:             }
00444:             _shelterThermal?.TickDay(day);
00445:             _weatherHardening?.TickDay(day);
00446:             _geothermalAquifer?.TickDay(day);
00447:             // Plan 188: feed the campaign hour so the schedule derives Night from
00448:             // its authored windows (the schedule remains the only phase owner).
00449:             // The same hour drives the placeholder lighting backdrop variants.
00450:             if (_campaignDay?.Calendar != null)
00451:             {
00452:                 int hour = _campaignDay.Calendar.AsSimClock().HourOfDay;
00453:                 _shelterSchedule?.TickHour(hour);
00454:                 string lightingPhase = LightingPhaseForHour(hour);
00455:                 _shelterPanel?.SetLightingPhase(lightingPhase);
00456:                 _mapPanel?.SetLightingPhase(lightingPhase);
00457:                 _expeditionPanel?.SetLightingPhase(lightingPhase);
00458:             }
00459:             _shelterSchedule?.TickDay(day);
00460:             TickShelterAtmosphere(day);
00461:             _autopsy?.TickDay(day);
00462:             // Plan 72 §3 ordering: advance ventilation/air filtration — hosts
00463:             // pass weather truth; Core owns the intake conversion and stage math.
00464:             if (_ventilation != null)
00465:             {
00466:                 var ventWeather = _world.Weather.Current;
00467:                 // C2[6] 23A: mechanical air handling is a real load. When
00468:                 // room_air_filtration is shed the fans stop and only residual
00469:                 // passive draft removes air (see VentilationSystem.TickDay).
00470:                 bool mechanicalPower = _powerGrid?.System == null
00471:                     || _powerGrid.System.IsRoomServed("room_air_filtration");
00472:                 _ventilation.TickDay(
00473:                     day,
00474:                     Ashfall.Core.ElectrostaticFiltrationCatalogLoader.WeatherIntakeParticulateKg(
00475:                         ventWeather, _ventilation.State.mainDuctOpen),
00476:                     Ashfall.Core.ElectrostaticFiltrationCatalogLoader.IsHotAshLoad(ventWeather),
00477:                     mechanicalPower);
00478:             }
00479:             _waystation?.TickDaily(iceRoadOpen: true);
00480:             _sumpFlooding?.TickDay(day);
00481:             TickDeepWell(day); // B5–B8 Phase 6: deep-well raw-water intake (before consumers read the pools)
00482:             TickWaterCondenser(day); // B5–B8 expansion: weather-indexed condensate intake
00483:             _decontamination?.TickDay(day);
00484:             TickPlans78To81(day);
00485:             TickPlans110To113(day);
00486:             TickPlans130To133(day);
00487:             TickPlans146To149(day);
00488:             _kitchenNutrition?.TickDay(day);
00489:             TickPlans94To97(day);
00490:             _equipmentCondition?.TickDay(day);
00491:             _libraryStudy?.TickDay(day);
00492:             _archiveDesk?.TickDay(day);
00493:             _contractorRoster?.TickDay(day);
00494:             _mentalHealthCrisis?.TickDay(day);
00495:             TickSleepNarrative(day);
00496:             _crafting?.TickDay(day);
00497:             TickHiddenAgenda(day);
00498:             TickShelterReputation(day);
00499:             TickPropaganda(day);
00500:             TickRumorNetwork(day);
00501:             TickShelterSecurity(day);
00502:             TickVisitorIntegration(day);
00503:             TickPersonalQuests(day);
00504:             TickTimeCapsule(day);
00505:             TickSurvivorDeathLegacy(day);
00506:             TickRelationshipDecay(day);
00507:             TickOrphanSealWave1(day);
00508:         }
00509:
00510:         public void OpenExpandedPanel(string panelKey)
00511:         {
00512:             switch (panelKey)
00513:             {
00514:                 case "water_treatment":
00515:                     SetupWaterTreatment();
00516:                     BindWaterSourcesPanel();
00517:                     if (_waterTreatmentPanel != null) { _waterTreatmentPanel.Visible = true; _waterTreatmentPanel.RefreshView(); }
00518:                     break;
00519:                 case "airlock_security":
00520:                     if (_airlockSecurityPanel != null) { _airlockSecurityPanel.Visible = true; _airlockSecurityPanel.RefreshView(); }
00521:                     break;
00522:                 case "survivor_relations":
00523:                     if (_survivorRelationsPanel != null) { _survivorRelationsPanel.Visible = true; _survivorRelationsPanel.RefreshView(); }
00524:                     break;
00525:                 case "regional_treaty":
00526:                     if (_regionalTreatyPanel != null) { _regionalTreatyPanel.Visible = true; _regionalTreatyPanel.RefreshView(); }
00527:                     break;
00528:                 case "vinyl_morale":
00529:                     if (_vinylMoralePanel != null) { _vinylMoralePanel.Visible = true; _vinylMoralePanel.RefreshView(); }
00530:                     break;
00531:                 case "low_background_metrology":
00532:                     if (_lowBackgroundPanel != null) { _lowBackgroundPanel.Visible = true; _lowBackgroundPanel.RefreshView(); }
00533:                     break;
00534:                 case "insar_mapping":
00535:                     if (_inSarPanel != null) { _inSarPanel.Visible = true; _inSarPanel.RefreshView(); }
00536:                     break;
00537:                 case "hydraulic_extrusion":
00538:                     if (_hydraulicExtrusionPanel != null) { _hydraulicExtrusionPanel.Visible = true; _hydraulicExtrusionPanel.RefreshView(); }
00539:                     break;
00540:                 case "runflat_tire":
00541:                     if (_runFlatTirePanel != null) { _runFlatTirePanel.Visible = true; _runFlatTirePanel.RefreshView(); }
00542:                     break;
00543:                 case "sofc_power":
00544:                     OpenSofcPowerPanel();
00545:                     break;
00546:                 case "sound_ranging":
00547:                     OpenSoundRangingPanel();
00548:                     break;
00549:                 case "night_watch":
00550:                     ShowNightWatchPanel();
00551:                     break;
00552:                 case "shelter_operations":
00553:                     ShowShelterOperationsPanel();
00554:                     break;
00555:                 case "cvd_diamond":
00556:                     OpenCvdDiamondPanel();
00557:                     break;
00558:                 case "amphibious_draisine":
00559:                     OpenAmphibiousDraisinePanel();
00560:                     break;
00561:                 case "sanitation":
00562:                     OpenSanitationPanel();
00563:                     break;
00564:                 case "black_market":
00565:                     OpenBlackMarketPanel();
00566:                     break;
00567:                 case "sky_defense_battery":
00568:                     OpenSkyDefenseBatteryPanel();
00569:                     break;
00570:                 case "dynamic_quests":
00571:                     OpenDynamicQuestlinePanel();
00572:                     break;
00573:                 case "vehicle_garage":
00574:                     OpenVehicleGaragePanel();
00575:                     break;
00576:                 case "companion_kennel":
00577:                     OpenKennelPanel();
00578:                     break;
00579:                 case "beliefs_panel":
00580:                     OpenBeliefsPanel();
00581:                     break;
00582:                 case "anomaly_watch":
00583:                     OpenAnomalyWatchPanel();
00584:                     break;
00585:                 case "cybernetics":
00586:                     OpenCyberneticsPanel();
00587:                     break;
00588:                 case "wildlife_trapping":
00589:                     if (_wildlifeTrappingPanel != null) { _wildlifeTrappingPanel.Visible = true; _wildlifeTrappingPanel.RefreshView(); }
00590:                     break;
00591:                 case "excavation":
00592:                     if (_excavationPanel != null) { _excavationPanel.Visible = true; _excavationPanel.RefreshView(); }
00593:                     break;
00594:                 case "apprenticeship":
00595:                     if (_apprenticeshipPanel != null) { _apprenticeshipPanel.Visible = true; _apprenticeshipPanel.RefreshView(); }
00596:                     break;
00597:                 case "caregiving":
00598:                     if (_caregivingPanel != null) { _caregivingPanel.Visible = true; _caregivingPanel.RefreshView(); }
00599:                     break;
00600:                 case "shelter_thermal":
00601:                     if (_shelterThermalPanel != null) { _shelterThermalPanel.Visible = true; _shelterThermalPanel.RefreshView(); }
00602:                     break;
00603:                 case "shelter_schedule":
00604:                     if (_shelterSchedulePanel != null) { _shelterSchedulePanel.Visible = true; _shelterSchedulePanel.RefreshView(); }
00605:                     break;
00606:                 case "autopsy_report":
00607:                     if (_autopsyReportPanel != null) { _autopsyReportPanel.Visible = true; _autopsyReportPanel.RefreshView(); }
00608:                     break;
00609:                 case "waystation_network":
00610:                     if (_waystationPanel != null) { _waystationPanel.Visible = true; _waystationPanel.RefreshView(); }
00611:                     break;
00612:                 case "chemical_dependency":
00613:                     if (_chemicalDependencyPanel != null) { _chemicalDependencyPanel.Visible = true; _chemicalDependencyPanel.RefreshView(); }
00614:                     break;
00615:                 case "sump_flooding":
00616:                     if (_sumpFloodingPanel != null) { _sumpFloodingPanel.Visible = true; _sumpFloodingPanel.RefreshView(); }
00617:                     break;
00618:                 case "decontamination":
00619:                     if (_decontaminationPanel != null) { _decontaminationPanel.Visible = true; _decontaminationPanel.RefreshView(); }
00620:                     break;
00621:                 case "kitchen_nutrition":
00622:                     if (_kitchenNutritionPanel != null) { _kitchenNutritionPanel.Visible = true; _kitchenNutritionPanel.RefreshView(); }
00623:                     break;
00624:                 case "equipment_condition":
00625:                     if (_equipmentConditionPanel != null) { _equipmentConditionPanel.Visible = true; _equipmentConditionPanel.RefreshView(); }
00626:                     break;
00627:                 case "library_study":
00628:                     if (_libraryStudyPanel != null) { _libraryStudyPanel.Visible = true; _libraryStudyPanel.RefreshView(); }
00629:                     break;
00630:                 case "archive_desk":
00631:                     SetupJournal();
00632:                     DiscoverBureaucraticDocuments("archive_desk");
00633:                     // Real archive-desk inspection of the bunker records drawer.
00634:                     DiscoverFringeCultRecords("government_bunker");
00635:                     DiscoverPaperPrintingRecords("government_bunker");
00636:                     DiscoverBoneHornRecords("government_bunker");
00637:                     DiscoverAbyssalAnomalyRecords("government_bunker");
00638:                     if (_archiveDeskPanel != null) { _archiveDeskPanel.Visible = true; _archiveDeskPanel.RefreshView(); }
00639:                     break;
00640:                 case "contractor_roster":
00641:                     if (_contractorRosterPanel != null) { _contractorRosterPanel.Visible = true; _contractorRosterPanel.RefreshView(); }
00642:                     break;
00643:                 case "mental_health_crisis":
00644:                     if (_mentalHealthCrisisPanel != null) { _mentalHealthCrisisPanel.Visible = true; _mentalHealthCrisisPanel.RefreshView(); }
00645:                     break;
00646:                 case "phantom_memory":
00647:                     if (_phantomMemoryPanel != null) { _phantomMemoryPanel.Visible = true; _phantomMemoryPanel.RefreshView(); }
00648:                     break;
00649:                 case "traveling_caravan":
00650:                     if (_travelingCaravanPanel != null) { _travelingCaravanPanel.Visible = true; _travelingCaravanPanel.RefreshView(); }
00651:                     break;
00652:                 case "shelter_barter":
00653:                     OpenShelterBarterPanel();
00654:                     break;
00655:                 case "black_projects_archive":
00656:                     OpenBlackProjectsArchivePanel();
00657:                     break;
00658:                 case "shelter_decor":
00659:                     SetupShelterDecor();
00660:                     if (_shelterDecorPanel != null) { _shelterDecorPanel.Visible = true; _shelterDecorPanel.RefreshView(); }
00661:                     break;
00662:                 case "shelter_atmosphere":
00663:                     ShowShelterAtmospherePanel();
00664:                     break;
00665:                 case "hidden_agenda":
00666:                     ShowHiddenAgendaPanel();
00667:                     break;
00668:                 case "shelter_reputation":
00669:                     ShowShelterReputationPanel();
00670:                     break;
00671:                 case "propaganda":
00672:                     ShowPropagandaPanel();
00673:                     break;
00674:                 case "rumors":
00675:                     ShowRumorNetworkPanel();
00676:                     break;
00677:                 case "shelter_security":
00678:                     ShowShelterSecurityPanel();
00679:                     break;
00680:                 case "visitor_integration":
00681:                     ShowVisitorIntegrationPanel();
00682:                     break;
00683:                 case "personal_belongings":
00684:                     ShowPersonalBelongingsPanel();
00685:                     break;
00686:                 case "personal_quests":
00687:                     ShowPersonalQuestPanel();
00688:                     break;
00689:                 case "time_capsule":
00690:                     ShowTimeCapsulePanel();
00691:                     break;
00692:                 case "death_legacy":
00693:                     ShowSurvivorDeathLegacyPanel();
00694:                     break;
00695:                 case "relationship_decay":
00696:                     ShowRelationshipDecayPanel();
00697:                     break;
00698:                 case "medical_ward":
00699:                     SetupJournal();
00700:                     DiscoverBureaucraticDocuments("medical_office");
00701:                     SetupMedicalWard();
00702:                     if (_medicalWardPanel != null) { _medicalWardPanel.Visible = true; _medicalWardPanel.RefreshView(); }
00703:                     break;
00704:                 case "plans_94_97":
00705:                     SetupGrainProcessing();
00706:                     SetupCryogenicAirSeparation();
00707:                     SetupHeliograph();
00708:                     SetupPlans94To97Panel();
00709:                     if (_plans94To97Panel != null) { _plans94To97Panel.Visible = true; _plans94To97Panel.RefreshView(); }
00710:                     break;
00711:                 case "plans_130_133":
00712:                     OpenPlans130To133Panel();
00713:                     break;
00714:                 case "journal":
00715:                     SetupJournal();
00716:                     if (_journalPanel != null) { _journalPanel.Bind(_journal); _journalPanel.Visible = true; _journalPanel.RefreshView(); }
00717:                     break;
00718:                 case "weather":
00719:                     SetupWorld();
00720:                     if (_weatherPanel != null) { _weatherPanel.Bind(_world); _weatherPanel.Visible = true; _weatherPanel.RefreshView(); }
00721:                     break;
00722:             }
00723:         }
00724:
00725:         private void ResetExpandedShelterSessions()
00726:         {
00727:             // Remove instantiated panels from scene tree
00728:             void RemovePanel(Control? panel)
00729:             {
00730:                 if (panel != null && panel.IsInsideTree())
00731:                     RemoveChild(panel);
00732:             }
00733:
00734:             _waterTreatmentPanel?.UnbindWaterSources();
00735:             RemovePanel(_waterTreatmentPanel); _waterTreatmentPanel = null!;
00736:             RemovePanel(_airlockSecurityPanel); _airlockSecurityPanel = null!;
00737:             RemovePanel(_shelterThermalPanel); _shelterThermalPanel = null!;
00738:             RemovePanel(_shelterSchedulePanel); _shelterSchedulePanel = null!;
00739:             RemovePanel(_autopsyReportPanel); _autopsyReportPanel = null!;
00740:             RemovePanel(_waystationPanel); _waystationPanel = null!;
00741:             RemovePanel(_survivorRelationsPanel); _survivorRelationsPanel = null!;
00742:             RemovePanel(_regionalTreatyPanel); _regionalTreatyPanel = null!;
00743:             RemovePanel(_vinylMoralePanel); _vinylMoralePanel = null!;
00744:             RemovePanel(_wildlifeTrappingPanel); _wildlifeTrappingPanel = null!;
00745:             RemovePanel(_excavationPanel); _excavationPanel = null!;
00746:             RemovePanel(_apprenticeshipPanel); _apprenticeshipPanel = null!;
00747:             RemovePanel(_caregivingPanel); _caregivingPanel = null!;
00748:             RemovePanel(_sumpFloodingPanel); _sumpFloodingPanel = null!;
00749:             RemovePanel(_decontaminationPanel); _decontaminationPanel = null!;
00750:             RemovePanel(_kitchenNutritionPanel); _kitchenNutritionPanel = null!;
00751:             RemovePanel(_equipmentConditionPanel); _equipmentConditionPanel = null!;
00752:             RemovePanel(_libraryStudyPanel); _libraryStudyPanel = null!;
00753:             RemovePanel(_archiveDeskPanel); _archiveDeskPanel = null!;
00754:             RemovePanel(_contractorRosterPanel); _contractorRosterPanel = null!;
00755:             RemovePanel(_mentalHealthCrisisPanel); _mentalHealthCrisisPanel = null!;
00756:             RemovePanel(_chemicalDependencyPanel); _chemicalDependencyPanel = null!;
00757:             RemovePanel(_phantomMemoryPanel); _phantomMemoryPanel = null!;
00758:             RemovePanel(_travelingCaravanPanel); _travelingCaravanPanel = null!;
00759:             RemovePanel(_powerGridPanel); _powerGridPanel = null!;
00760:             RemovePanel(_medicalWardPanel); _medicalWardPanel = null!;
00761:             RemovePanel(_shelterDecorPanel); _shelterDecorPanel = null!;
00762:             RemovePanel(_shelterReputationPanel); _shelterReputationPanel = null!;
00763:             _visitorIntegrationPanel?.Unbind();
00764:             RemovePanel(_visitorIntegrationPanel); _visitorIntegrationPanel = null!;
00765:             RemovePanel(_personalQuestPanel); _personalQuestPanel = null!;
00766:             RemovePanel(_timeCapsulePanel); _timeCapsulePanel = null!;
00767:             RemovePanel(_deathLegacyPanel); _deathLegacyPanel = null!;
00768:             RemovePanel(_relationshipDecayPanel); _relationshipDecayPanel = null!;
00769:             _plans94To97Panel?.Unbind();
00770:             RemovePanel(_plans94To97Panel); _plans94To97Panel = null;
00771:             _shelterBarterPanel?.Unbind();
00772:             RemovePanel(_shelterBarterPanel); _shelterBarterPanel = null;
00773:             ResetPlans130To133Panel();
00774:
00775:             // Dispose / null host sessions
00776:             _waterSources?.Dispose(); _waterSources = null;
00777:             _deepWell = null;
00778:             _waterCondenser = null;
00779:             _piezometer = null;
00780:             _waterTreatment?.Dispose(); _waterTreatment = null!;
00781:             _airlockSecurity?.Dispose(); _airlockSecurity = null!;
00782:             _shelterThermal?.Dispose(); _shelterThermal = null!;
00783:             _weatherHardening?.Dispose(); _weatherHardening = null!;
00784:             _geothermalAquifer?.Dispose(); _geothermalAquifer = null!;
00785:             _geothermalAquiferDirty = false;
00786:             _shelterSchedule?.Dispose(); _shelterSchedule = null!;
00787:             _autopsy?.Dispose(); _autopsy = null!;
00788:             _waystation?.Dispose(); _waystation = null!;
00789:             _survivorRelations?.Dispose(); _survivorRelations = null!;
00790:             _survivorRelationsCore = null!;
00791:             _regionalTreaty?.Dispose(); _regionalTreaty = null!;
00792:             _vinylMorale?.Dispose(); _vinylMorale = null!;
00793:             _wildlifeTrapping?.Dispose(); _wildlifeTrapping = null!;
00794:             _excavation?.Dispose(); _excavation = null!;
00795:             _apprenticeship?.Dispose(); _apprenticeship = null!;
00796:             _caregiving?.Dispose(); _caregiving = null!;
00797:             _sumpFlooding?.Dispose(); _sumpFlooding = null!;
00798:             _decontamination?.Dispose(); _decontamination = null!;
00799:             _kitchenNutrition?.Dispose(); _kitchenNutrition = null!;
00800:             _equipmentCondition?.Dispose(); _equipmentCondition = null!;
00801:             _libraryStudy?.Dispose(); _libraryStudy = null!;
00802:             _archiveDesk?.Dispose(); _archiveDesk = null!;
00803:             _contractorRoster?.Dispose(); _contractorRoster = null!;
00804:             _mentalHealthCrisis?.Dispose(); _mentalHealthCrisis = null!;
00805:             _powderMetallurgy?.Dispose(); _powderMetallurgy = null;
00806:             _nvisCommunications?.Dispose(); _nvisCommunications = null;
00807:             _lyophilization?.Dispose(); _lyophilization = null;
00808:             _draisineRerailing?.Dispose(); _draisineRerailing = null;
00809:             _grainProcessing?.Dispose(); _grainProcessing = null;
00810:             _cryogenicAirSeparation?.Dispose(); _cryogenicAirSeparation = null;
00811:             _heliograph?.Dispose(); _heliograph = null;
00812:             _chemicalDependency?.Dispose(); _chemicalDependency = null!;
00813:             _shelterAssignment?.Dispose(); _shelterAssignment = null!;
00814:             _shelterDecor?.Dispose(); _shelterDecor = null!;
00815:             _travelingCaravan?.Dispose(); _travelingCaravan = null!;
00816:             _powerGrid?.Dispose(); _powerGrid = null!;
00817:             _geothermalOrc?.Dispose(); _geothermalOrc = null!;
00818:             _ballisticsWorkbench?.Dispose(); _ballisticsWorkbench = null!;
00819:             _aeroponics?.Dispose(); _aeroponics = null!;
00820:             _pneumaticDispatch?.Dispose(); _pneumaticDispatch = null!;
00821:             _medicalWardSession?.Dispose(); _medicalWardSession = null!;
00822:             _medicalWard = null!;
00823:             _factionBranch?.Dispose(); _factionBranch = null!;
00824:             _counterIntelligence?.Dispose(); _counterIntelligence = null!;
00825:             _factionBranchDirty = false;
00826:             _counterIntelligenceDirty = false;
00827:             ResetShelterOperations();
00828:             ResetOrphanSealWave1();
00829:             ResetCommitments();
00830:             ResetSessionDurability();
00831:             ResetPlayMetrics();
00832:             ResetSurvivorVoice();
00833:             ResetRetention();
00834:             ResetOutpostSettlement();
00835:             ResetPresentation();
00836:             ResetNeedsPerformance();
00837:             ResetCampaignLegacy();
00838:             ResetResearchUnlock();
00839:             ResetUnifiedEnding();
00840:             ResetNightWatch();
00841:             _expandedShelterRoster = new DutyRosterSystem();
00842:
00843:             _airlockSecurityDirty = false;
00844:             _shelterThermalDirty = false;
00845:             _weatherHardeningDirty = false;
00846:             _geothermalAquiferDirty = false;
00847:             _shelterScheduleDirty = false;
00848:             _autopsyDirty = false;
00849:             _waystationDirty = false;
00850:             _survivorRelationsDirty = false;
00851:             _regionalTreatyDirty = false;
00852:             _vinylMoraleDirty = false;
00853:             _wildlifeTrappingDirty = false;
00854:             _excavationDirty = false;
00855:             _apprenticeshipDirty = false;
00856:             _caregivingDirty = false;
00857:             _sumpFloodingDirty = false;
00858:             _decontaminationDirty = false;
00859:             _kitchenNutritionDirty = false;
00860:             _equipmentConditionDirty = false;
00861:             _libraryStudyDirty = false;
00862:             _archiveDeskDirty = false;
00863:             _contractorRosterDirty = false;
00864:             _mentalHealthCrisisDirty = false;
00865:             _powerGridDirty = false;
00866:             _geothermalOrcDirty = false;
00867:             _ballisticsWorkbenchDirty = false;
00868:             _aeroponicsDirty = false;
00869:             _pneumaticDispatchDirty = false;
00870:             _medicalWardDirty = false;
00871:             _shelterReputation = null!;
00872:             _shelterReputationDirty = false;
00873:             _propaganda = null!;
00874:             _propagandaDirty = false;
00875:             _rumorNetwork = null!;
00876:             _rumorNetworkDirty = false;
00877:             _shelterSecurity = null!;
00878:             _shelterSecurityDirty = false;
00879:             _visitorIntegration = null!;
00880:             _visitorIntegrationDirty = false;
00881:             ResetPersonalBelongings();
00882:             _personalQuests = null!;
00883:             _personalQuestsDirty = false;
00884:             _timeCapsule = null!;
00885:             _timeCapsuleDirty = false;
00886:             _deathLegacy = null!;
00887:             _deathLegacyDirty = false;
00888:             _relationshipDecay = null!;
00889:             _relationshipDecayDirty = false;
00890:
00891:             // Lifecycle reset is intentionally persistence-free. The expanded
00892:             // shelter group owns the existing section captures, but it does
00893:             // not delete slot projections, campaign envelopes, backups, or
00894:             // global user:// saves. Destructive new-game cleanup remains in
00895:             // the separately scoped DeleteGlobalSavesOnDisk path.
00896:         }
00897:     }
00898: }
```

## `Ashfall.Core.Tests/LibraryStudySystemTests.cs` — 141 lines; 6,497 bytes; SHA-256 `ebebdeefe40e1e8593ae004cb917fb0833d54b898a34bb570338eba74651167f`
Declaration index:
- 00009: public class LibraryStudySystemTests
- 00011: [Fact] public void StartStudy_WithoutPrereq_Blocks()
- 00022: [Fact] public void StartStudy_WithPrereq_StartsJob()
- 00033: [Fact] public void TickDay_CompletesStudy()
- 00046: [Fact] public void CompleteStudy_UnlocksResearch()
- 00058: [Fact] public void StartStudy_AlreadyCompleted_Blocks()
- 00071: [Fact] public void LoadCatalog_OddLengthSkillGrantList_Throws()
- 00089: [Fact] public void StartStudy_ZeroStudyHours_Blocks()
- 00112: [Fact] public void CaptureRestoreState_PreservesJobs()
- 00132: private static LibraryStudySystem Create(out SkillProgressionSystem skills, out ResearchSystem research, out JournalSystem journal, out DutyRosterSystem roster)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using Ashfall.Core;
00003: using Ashfall.Core.Journal;
00004: using Ashfall.Core.Survivors;
00005: using Xunit;
00006:
00007: namespace Ashfall.Core.Tests
00008: {
00009:     public class LibraryStudySystemTests
00010:     {
00011:         [Fact] public void StartStudy_WithoutPrereq_Blocks()
00012:         {
00013:             var lib = Create(out _, out _, out _, out _);
00014:             lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
00015:             {
00016:                 new ManualDefinition { manual_id = "man_advanced", display_name = "Advanced Tech", prerequisites = new System.Collections.Generic.List<string> { "man_basic" } }
00017:             });
00018:             var r = lib.StartStudy("man_advanced", "survivor_1");
00019:             Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
00020:         }
00021:
00022:         [Fact] public void StartStudy_WithPrereq_StartsJob()
00023:         {
00024:             var lib = Create(out _, out _, out _, out _);
00025:             lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
00026:             {
00027:                 new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5 }
00028:             });
00029:             lib.StartStudy("man_basic", "survivor_1");
00030:             Assert.Single(lib.State.activeJobs);
00031:         }
00032:
00033:         [Fact] public void TickDay_CompletesStudy()
00034:         {
00035:             var lib = Create(out _, out _, out _, out _);
00036:             lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
00037:             {
00038:                 new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5, skillXpGrants = new System.Collections.Generic.List<string> { "skill_engineering", "10" } }
00039:             });
00040:             lib.StartStudy("man_basic", "survivor_1");
00041:             lib.TickDay(1);
00042:             Assert.True(lib.State.activeJobs[0].isComplete);
00043:             Assert.Contains("man_basic", lib.State.completedManualIds);
00044:         }
00045:
00046:         [Fact] public void CompleteStudy_UnlocksResearch()
00047:         {
00048:             var lib = Create(out _, out var research, out _, out _);
00049:             lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
00050:             {
00051:                 new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5, researchUnlocks = new System.Collections.Generic.List<string> { "tech_water_purifier" } }
00052:             });
00053:             lib.StartStudy("man_basic", "survivor_1");
00054:             lib.TickDay(1);
00055:             Assert.True(research.IsManualUnlocked("tech_water_purifier"));
00056:         }
00057:
00058:         [Fact] public void StartStudy_AlreadyCompleted_Blocks()
00059:         {
00060:             var lib = Create(out _, out _, out _, out _);
00061:             lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
00062:             {
00063:                 new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5 }
00064:             });
00065:             lib.StartStudy("man_basic", "survivor_1");
00066:             lib.TickDay(1);
00067:             var r = lib.StartStudy("man_basic", "survivor_1");
00068:             Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
00069:         }
00070:
00071:         [Fact] public void LoadCatalog_OddLengthSkillGrantList_Throws()
00072:         {
00073:             // Bug-10 regression: a manual with an odd number of skillXpGrants
00074:             // entries would crash the tick loop with IndexOutOfRange when the
00075:             // reader advances 'i' by 2 and reads '[i+1]'. The catalog loader
00076:             // must surface this as invalid before a TickDay ever sees it.
00077:             var lib = Create(out _, out _, out _, out _);
00078:             var bad = new ManualDefinition
00079:             {
00080:                 manual_id = "man_bad",
00081:                 display_name = "Bad Manual",
00082:                 studyHoursRequired = 1,
00083:                 skillXpGrants = new System.Collections.Generic.List<string> { "skill_engineering", "10", "orphan" }
00084:             };
00085:             Assert.Throws<System.IO.InvalidDataException>(() =>
00086:                 lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition> { bad }));
00087:         }
00088:
00089:         [Fact] public void StartStudy_ZeroStudyHours_Blocks()
00090:         {
00091:             // Bug-15b regression: a manual with studyHoursRequired == 0 (or
00092:             // negative) would complete instantly on TickDay, granting all XP,
00093:             // research unlocks, and knowledge evidence in zero days. The start
00094:             // path must reject such manuals as malformed before they reach the
00095:             // tick loop. The constructor default is 10; an author who overrides
00096:             // it to 0 is setting a trap.
00097:             var lib = Create(out _, out _, out _, out _);
00098:             lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
00099:             {
00100:                 new ManualDefinition
00101:                 {
00102:                     manual_id = "man_freebie",
00103:                     display_name = "Free Magic",
00104:                     studyHoursRequired = 0
00105:                 }
00106:             });
00107:             var r = lib.StartStudy("man_freebie", "survivor_1");
00108:             Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
00109:             Assert.Empty(lib.State.activeJobs);
00110:         }
00111:
00112:         [Fact] public void CaptureRestoreState_PreservesJobs()
00113:         {
00114:             var lib = Create(out _, out _, out _, out _);
00115:             lib.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
00116:             {
00117:                 new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5 }
00118:             });
00119:             lib.StartStudy("man_basic", "survivor_1");
00120:             var state = lib.CaptureState();
00121:             Assert.Single(state.activeJobs);
00122:
00123:             var lib2 = Create(out _, out _, out _, out _);
00124:             lib2.LoadCatalog(new System.Collections.Generic.List<ManualDefinition>
00125:             {
00126:                 new ManualDefinition { manual_id = "man_basic", display_name = "Basic Tech", studyHoursRequired = 5 }
00127:             });
00128:             lib2.RestoreState(state);
00129:             Assert.Single(lib2.State.activeJobs);
00130:         }
00131:
00132:         private static LibraryStudySystem Create(out SkillProgressionSystem skills, out ResearchSystem research, out JournalSystem journal, out DutyRosterSystem roster)
00133:         {
00134:             skills = new SkillProgressionSystem();
00135:             research = new ResearchSystem();
00136:             journal = new JournalSystem();
00137:             roster = new DutyRosterSystem();
00138:             return new LibraryStudySystem(skills, research, journal, roster);
00139:         }
00140:     }
00141: }
```

## `Ashfall.Core.Tests/Library/LibraryStudyContractTests.cs` — 402 lines; 16,271 bytes; SHA-256 `2ef4e6cbd958e0abbb3321372e52bba0605d746219680042aac9c4981f514279`
Declaration index:
- 00013: public sealed class LibraryStudyContractTests
- 00015: private static string FindDataDir()
- 00023: private static LibraryStudySystem CreateSystem(
- 00037: public void B2_001_ManualStudy_CallsUnlockManual_NeverCompleteResearch()
- 00079: public void B2_002_And_B2_004_JournalEvidenceAddedAndDedupedWithStableProvenance()
- 00107: public void B2_003_DuplicateResearchUnlock_IsIdempotent()
- 00153: public void B2_005_And_B2_006_SkillRaisesStudyRateMonotonically_WithinStrictBounds()
- 00203: public void B2_007_InvalidZeroOrNegativeHours_Rejected()
- 00232: public void B2_008_And_B2_009_BidirectionalAvailabilityReservation_DutyRoster()
- 00280: public void B2_010_To_B2_014_AuthoritativeCatalogIntegrity_24Manuals_6Disciplines()
- 00350: public void B2_016_And_B2_017_SaveRestore_PreservesJobsAndUnknownCompletedIds()
- 00397: private sealed class ResearchCatalogContainer
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Journal;
00008: using Ashfall.Core.Survivors;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.Library
00012: {
00013:     public sealed class LibraryStudyContractTests
00014:     {
00015:         private static string FindDataDir()
00016:         {
00017:             string start = Directory.GetCurrentDirectory();
00018:             if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
00019:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
00020:             throw new DirectoryNotFoundException("Data directory not found from " + start);
00021:         }
00022:
00023:         private static LibraryStudySystem CreateSystem(
00024:             out SkillProgressionSystem skills,
00025:             out ResearchSystem research,
00026:             out JournalSystem journal,
00027:             out DutyRosterSystem roster)
00028:         {
00029:             skills = new SkillProgressionSystem();
00030:             research = new ResearchSystem();
00031:             journal = new JournalSystem();
00032:             roster = new DutyRosterSystem();
00033:             return new LibraryStudySystem(skills, research, journal, roster);
00034:         }
00035:
00036:         [Fact]
00037:         public void B2_001_ManualStudy_CallsUnlockManual_NeverCompleteResearch()
00038:         {
00039:             var sys = CreateSystem(out _, out var research, out _, out _);
00040:             var kdef = new ResearchKnowledgeDef
00041:             {
00042:                 id = "knowledge_water_basics",
00043:                 displayName = "Water Purification",
00044:                 daysToComplete = 5,
00045:                 isCompleted = false,
00046:                 isUnlocked = false
00047:             };
00048:             research.Register(kdef);
00049:
00050:             sys.LoadCatalog(new List<ManualDefinition>
00051:             {
00052:                 new ManualDefinition
00053:                 {
00054:                     manual_id = "manual_water_filtration",
00055:                     display_name = "Field Water Filtration",
00056:                     category = "survival",
00057:                     studyHoursRequired = 8,
00058:                     researchUnlocks = new List<string> { "knowledge_water_basics" },
00059:                     knowledgeUnlocks = new List<string> { "knowledge_water_basics" }
00060:                 }
00061:             });
00062:
00063:             var startRes = sys.StartStudy("manual_water_filtration", "survivor_1");
00064:             Assert.True(startRes.IsSuccess);
00065:
00066:             // Tick day to complete manual study
00067:             sys.TickDay(1);
00068:
00069:             Assert.True(sys.IsManualCompleted("manual_water_filtration"));
00070:             // Architectural invariant: Node is unlocked/revealed, NEVER completed
00071:             Assert.True(research.IsManualUnlocked("knowledge_water_basics"));
00072:             var node = research.GetKnowledge("knowledge_water_basics");
00073:             Assert.NotNull(node);
00074:             Assert.True(node.isUnlocked);
00075:             Assert.False(node.isCompleted); // Must NOT be completed!
00076:         }
00077:
00078:         [Fact]
00079:         public void B2_002_And_B2_004_JournalEvidenceAddedAndDedupedWithStableProvenance()
00080:         {
00081:             var sys = CreateSystem(out _, out _, out var journal, out _);
00082:             sys.LoadCatalog(new List<ManualDefinition>
00083:             {
00084:                 new ManualDefinition
00085:                 {
00086:                     manual_id = "manual_test_evidence",
00087:                     display_name = "Evidence Manual",
00088:                     category = "survival",
00089:                     studyHoursRequired = 8,
00090:                     knowledgeUnlocks = new List<string> { "knowledge_water_basics" }
00091:                 }
00092:             });
00093:
00094:             sys.StartStudy("manual_test_evidence", "survivor_1");
00095:             sys.TickDay(1);
00096:
00097:             // Knowledge key registered in journal knowledge base
00098:             Assert.True(journal.Knowledge.Has("knowledge_water_basics"));
00099:
00100:             // Adding same evidence again is idempotent
00101:             int countBefore = journal.Knowledge.Count;
00102:             journal.AddKnowledgeEvidence("survivor_1", "knowledge_water_basics");
00103:             Assert.Equal(countBefore, journal.Knowledge.Count);
00104:         }
00105:
00106:         [Fact]
00107:         public void B2_003_DuplicateResearchUnlock_IsIdempotent()
00108:         {
00109:             var sys = CreateSystem(out _, out var research, out _, out _);
00110:             var kdef = new ResearchKnowledgeDef
00111:             {
00112:                 id = "knowledge_radio_basics",
00113:                 displayName = "Radio Basics",
00114:                 daysToComplete = 5
00115:             };
00116:             research.Register(kdef);
00117:
00118:             sys.LoadCatalog(new List<ManualDefinition>
00119:             {
00120:                 new ManualDefinition
00121:                 {
00122:                     manual_id = "manual_radio_1",
00123:                     display_name = "Radio Primer",
00124:                     category = "science",
00125:                     studyHoursRequired = 8,
00126:                     researchUnlocks = new List<string> { "knowledge_radio_basics" }
00127:                 },
00128:                 new ManualDefinition
00129:                 {
00130:                     manual_id = "manual_radio_2",
00131:                     display_name = "Radio Handbook",
00132:                     category = "science",
00133:                     studyHoursRequired = 8,
00134:                     researchUnlocks = new List<string> { "knowledge_radio_basics" }
00135:                 }
00136:             });
00137:
00138:             sys.StartStudy("manual_radio_1", "survivor_1");
00139:             sys.TickDay(1);
00140:
00141:             int unlockCount = research.State.unlockedIds.Count;
00142:             Assert.Contains("knowledge_radio_basics", research.State.unlockedIds);
00143:
00144:             // Complete second manual revealing same research
00145:             sys.StartStudy("manual_radio_2", "survivor_1");
00146:             sys.TickDay(2);
00147:
00148:             // Count of unlocked IDs should not duplicate
00149:             Assert.Equal(unlockCount, research.State.unlockedIds.Count);
00150:         }
00151:
00152:         [Fact]
00153:         public void B2_005_And_B2_006_SkillRaisesStudyRateMonotonically_WithinStrictBounds()
00154:         {
00155:             var sys = CreateSystem(out var skills, out _, out _, out _);
00156:             sys.LoadCatalog(new List<ManualDefinition>
00157:             {
00158:                 new ManualDefinition
00159:                 {
00160:                     manual_id = "manual_med_test",
00161:                     display_name = "Medical Manual",
00162:                     category = "medical",
00163:                     studyHoursRequired = 20
00164:                 }
00165:             });
00166:
00167:             // Register medical skill
00168:             skills.RegisterSkill(new SkillDef
00169:             {
00170:                 id = "skill_field_dressing",
00171:                 disciplineId = "medical",
00172:                 xpThreshold = 50f,
00173:                 skillBonus = 0.20f
00174:             });
00175:
00176:             string noviceId = "novice_reader";
00177:             string skilledId = "skilled_reader";
00178:             string masterId = "master_reader";
00179:
00180:             var actorNovice = new SimpleSkillActor(noviceId);
00181:             var actorSkilled = new SimpleSkillActor(skilledId);
00182:             var actorMaster = new SimpleSkillActor(masterId);
00183:
00184:             skills.RecordAction(actorNovice, "medical", 0f, 1);
00185:             skills.RecordAction(actorSkilled, "medical", 50f, 1);
00186:             skills.RecordAction(actorMaster, "medical", 500f, 1);
00187:
00188:             float rateNovice = sys.GetComprehensionRate(noviceId, "manual_med_test");
00189:             float rateSkilled = sys.GetComprehensionRate(skilledId, "manual_med_test");
00190:             float rateMaster = sys.GetComprehensionRate(masterId, "manual_med_test");
00191:
00192:             // Monotonic: Novice <= Skilled <= Master
00193:             Assert.True(rateNovice <= rateSkilled);
00194:             Assert.True(rateSkilled <= rateMaster);
00195:
00196:             // Bounds enforced: min 0.75, max 2.0
00197:             Assert.InRange(rateNovice, 0.75f, 2.0f);
00198:             Assert.InRange(rateSkilled, 0.75f, 2.0f);
00199:             Assert.InRange(rateMaster, 0.75f, 2.0f);
00200:         }
00201:
00202:         [Fact]
00203:         public void B2_007_InvalidZeroOrNegativeHours_Rejected()
00204:         {
00205:             var sys = CreateSystem(out _, out _, out _, out _);
00206:             sys.LoadCatalog(new List<ManualDefinition>
00207:             {
00208:                 new ManualDefinition
00209:                 {
00210:                     manual_id = "manual_zero_hours",
00211:                     display_name = "Broken Manual",
00212:                     studyHoursRequired = 0
00213:                 },
00214:                 new ManualDefinition
00215:                 {
00216:                     manual_id = "manual_neg_hours",
00217:                     display_name = "Broken Negative Manual",
00218:                     studyHoursRequired = -5
00219:                 }
00220:             });
00221:
00222:             var res1 = sys.StartStudy("manual_zero_hours", "survivor_1");
00223:             Assert.Equal(ActionResult.StatusKind.Blocked, res1.Status);
00224:             Assert.Equal("invalid_hours", res1.FailureCode);
00225:
00226:             var res2 = sys.StartStudy("manual_neg_hours", "survivor_1");
00227:             Assert.Equal(ActionResult.StatusKind.Blocked, res2.Status);
00228:             Assert.Equal("invalid_hours", res2.FailureCode);
00229:         }
00230:
00231:         [Fact]
00232:         public void B2_008_And_B2_009_BidirectionalAvailabilityReservation_DutyRoster()
00233:         {
00234:             var sys = CreateSystem(out _, out _, out _, out var roster);
00235:             roster.Unlock(1);
00236:             roster.WriteName("survivor_busy", "Busy Survivor", "Worker", DutyRosterIds.ScriptPencil, 1, true);
00237:             roster.WriteName("survivor_free", "Free Survivor", "Idle", DutyRosterIds.ScriptPencil, 1, true);
00238:
00239:             // Assign survivor_busy to duty roster role
00240:             roster.Assign(DutyRosterIds.RoleHatchOpener, "survivor_busy");
00241:             Assert.Equal(DutyRosterIds.RoleHatchOpener, roster.GetRoleOf("survivor_busy"));
00242:
00243:             sys.LoadCatalog(new List<ManualDefinition>
00244:             {
00245:                 new ManualDefinition
00246:                 {
00247:                     manual_id = "manual_guard",
00248:                     display_name = "Guard Manual",
00249:                     category = "combat",
00250:                     studyHoursRequired = 10
00251:                 }
00252:             });
00253:
00254:             // B2-008: Reader already on duty roster is blocked from starting study
00255:             var studyRes = sys.StartStudy("manual_guard", "survivor_busy");
00256:             Assert.Equal(ActionResult.StatusKind.Blocked, studyRes.Status);
00257:             Assert.Equal("busy", studyRes.FailureCode);
00258:
00259:             // Start study with free survivor
00260:             var freeStudyRes = sys.StartStudy("manual_guard", "survivor_free");
00261:             Assert.True(freeStudyRes.IsSuccess);
00262:             Assert.True(sys.IsReaderStudying("survivor_free"));
00263:
00264:             // B2-009: Active reader cannot be assigned to duty roster while studying
00265:             var assignRes = roster.AssignWithResult(DutyRosterIds.RoleNightWatch, "survivor_free");
00266:             Assert.Equal(ActionResult.StatusKind.Blocked, assignRes.Status);
00267:             Assert.Equal("busy", assignRes.FailureCode);
00268:
00269:             // Cancel study -> reservation released
00270:             var job = sys.GetActiveJobs().First(j => j.readerId == "survivor_free");
00271:             sys.CancelStudy(job.jobId);
00272:             Assert.False(sys.IsReaderStudying("survivor_free"));
00273:
00274:             // Now duty roster assignment succeeds
00275:             var reassignRes = roster.AssignWithResult(DutyRosterIds.RoleNightWatch, "survivor_free");
00276:             Assert.True(reassignRes.IsSuccess);
00277:         }
00278:
00279:         [Fact]
00280:         public void B2_010_To_B2_014_AuthoritativeCatalogIntegrity_24Manuals_6Disciplines()
00281:         {
00282:             string dataDir = FindDataDir();
00283:             var fileIO = new FileSystemIO();
00284:             var json = new SystemTextJsonSerializer();
00285:
00286:             var manuals = LibraryManualCatalogLoader.Load(dataDir, fileIO, json);
00287:             Assert.NotNull(manuals);
00288:
00289:             // B2-010: >= 24 manuals target
00290:             Assert.True(manuals.Count >= 24, $"Expected >= 24 manuals, got {manuals.Count}");
00291:
00292:             // B2-011: Six disciplines represented
00293:             var disciplines = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00294:             foreach (var m in manuals)
00295:             {
00296:                 disciplines.Add(LibraryStudySystem.NormalizeDiscipline(m.category));
00297:             }
00298:
00299:             string[] requiredDisciplines = { "survival", "crafting", "medical", "science", "scavenging", "combat" };
00300:             foreach (var req in requiredDisciplines)
00301:             {
00302:                 Assert.Contains(req, disciplines);
00303:             }
00304:
00305:             // Load research knowledge to verify references
00306:             string kPath = Path.Combine(dataDir, "research_knowledge.json");
00307:             Assert.True(File.Exists(kPath));
00308:             var kData = json.Deserialize<ResearchCatalogContainer>(File.ReadAllText(kPath));
00309:             var knownKnowledgeIds = new HashSet<string>(kData.knowledge_nodes.Select(k => k.id), StringComparer.Ordinal);
00310:
00311:             // Verify each manual
00312:             var manualIds = new HashSet<string>(StringComparer.Ordinal);
00313:             foreach (var m in manuals)
00314:             {
00315:                 Assert.False(string.IsNullOrEmpty(m.manual_id));
00316:                 Assert.True(manualIds.Add(m.manual_id), $"Duplicate manual ID: {m.manual_id}");
00317:                 Assert.True(m.studyHoursRequired > 0, $"Manual {m.manual_id} has invalid study hours: {m.studyHoursRequired}");
00318:
00319:                 // B2-012: all research/knowledge refs resolve
00320:                 foreach (var r in m.researchUnlocks)
00321:                 {
00322:                     Assert.Contains(r, knownKnowledgeIds);
00323:                 }
00324:                 foreach (var k in m.knowledgeUnlocks)
00325:                 {
00326:                     Assert.Contains(k, knownKnowledgeIds);
00327:                 }
00328:
00329:                 // B2-013 & B2-014: every manual has at least one acquisition path
00330:                 bool hasAcquisition = m.lootTableIds.Count > 0 ||
00331:                                       m.expeditionRewardIds.Count > 0 ||
00332:                                       m.traderPoolIds.Count > 0 ||
00333:                                       !string.IsNullOrEmpty(m.archiveScribingRecipeId) ||
00334:                                       m.startingOriginIds.Count > 0 ||
00335:                                       !string.IsNullOrEmpty(m.originFacility);
00336:                 Assert.True(hasAcquisition, $"Manual {m.manual_id} lacks structured acquisition metadata");
00337:             }
00338:
00339:             // Verify prerequisites resolve
00340:             foreach (var m in manuals)
00341:             {
00342:                 foreach (var p in m.prerequisites)
00343:                 {
00344:                     Assert.Contains(p, manualIds);
00345:                 }
00346:             }
00347:         }
00348:
00349:         [Fact]
00350:         public void B2_016_And_B2_017_SaveRestore_PreservesJobsAndUnknownCompletedIds()
00351:         {
00352:             var sys1 = CreateSystem(out _, out _, out _, out _);
00353:             sys1.LoadCatalog(new List<ManualDefinition>
00354:             {
00355:                 new ManualDefinition
00356:                 {
00357:                     manual_id = "manual_known",
00358:                     display_name = "Known Manual",
00359:                     category = "survival",
00360:                     studyHoursRequired = 16
00361:                 }
00362:             });
00363:
00364:             sys1.StartStudy("manual_known", "survivor_1");
00365:             sys1.TickDay(1);
00366:
00367:             // Inject historical/unknown manual ID into state
00368:             sys1.State.completedManualIds.Add("manual_historical_unknown_v1");
00369:
00370:             var saved = sys1.CaptureState();
00371:
00372:             // Create new system and restore
00373:             var sys2 = CreateSystem(out _, out _, out _, out _);
00374:             sys2.LoadCatalog(new List<ManualDefinition>
00375:             {
00376:                 new ManualDefinition
00377:                 {
00378:                     manual_id = "manual_known",
00379:                     display_name = "Known Manual",
00380:                     category = "survival",
00381:                     studyHoursRequired = 16
00382:                 }
00383:             });
00384:
00385:             sys2.RestoreState(saved);
00386:
00387:             // B2-016: Preserves active jobs
00388:             var active = sys2.GetActiveJobs();
00389:             Assert.Single(active);
00390:             Assert.Equal("manual_known", active[0].manualId);
00391:             Assert.Equal(8f, active[0].progressHours, 1);
00392:
00393:             // B2-017: Preserves unknown completed manual ID
00394:             Assert.True(sys2.IsManualCompleted("manual_historical_unknown_v1"));
00395:         }
00396:
00397:         private sealed class ResearchCatalogContainer
00398:         {
00399:             public List<ResearchKnowledgeDef> knowledge_nodes { get; set; } = new List<ResearchKnowledgeDef>();
00400:         }
00401:     }
00402: }
```

## `Ashfall.Core.Tests/Progression/LibraryStudyCatalogExpansionTests.cs` — 60 lines; 2,194 bytes; SHA-256 `8a5e4169976555fd0f8ea4c33fe6892c81889c9ced696fc1d165b8e5107bd508`
Declaration index:
- 00010: public sealed class LibraryStudyCatalogExpansionTests
- 00012: private static string ResolveDataDir()
- 00031: public void Load_Loads12ManualsFromCatalog()
- 00042: public void Load_AllManualsHaveValidFieldsAndRequirements()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using Ashfall.Core;
00005: using Ashfall.Core.IO;
00006: using Xunit;
00007:
00008: namespace Ashfall.Core.Tests.Progression
00009: {
00010:     public sealed class LibraryStudyCatalogExpansionTests
00011:     {
00012:         private static string ResolveDataDir()
00013:         {
00014:             string baseDir = AppContext.BaseDirectory;
00015:             string probe = Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
00016:             if (Directory.Exists(probe)) return probe;
00017:
00018:             string dir = baseDir;
00019:             for (int i = 0; i < 6; i++)
00020:             {
00021:                 probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
00022:                 if (Directory.Exists(probe)) return probe;
00023:                 var parent = Directory.GetParent(dir);
00024:                 if (parent == null) break;
00025:                 dir = parent.FullName;
00026:             }
00027:             return probe;
00028:         }
00029:
00030:         [Fact]
00031:         public void Load_Loads12ManualsFromCatalog()
00032:         {
00033:             string dataDir = ResolveDataDir();
00034:             var fileIO = new FileSystemIO();
00035:             var json = new SystemTextJsonSerializer();
00036:
00037:             var manuals = LibraryManualCatalogLoader.Load(dataDir, fileIO, json);
00038:             Assert.True(manuals.Count >= 24, $"Expected >= 24 manuals, got {manuals.Count}");
00039:         }
00040:
00041:         [Fact]
00042:         public void Load_AllManualsHaveValidFieldsAndRequirements()
00043:         {
00044:             string dataDir = ResolveDataDir();
00045:             var fileIO = new FileSystemIO();
00046:             var json = new SystemTextJsonSerializer();
00047:
00048:             var manuals = LibraryManualCatalogLoader.Load(dataDir, fileIO, json);
00049:             foreach (var m in manuals)
00050:             {
00051:                 Assert.False(string.IsNullOrWhiteSpace(m.manual_id));
00052:                 Assert.StartsWith("manual_", m.manual_id);
00053:                 Assert.False(string.IsNullOrWhiteSpace(m.display_name));
00054:                 Assert.False(string.IsNullOrWhiteSpace(m.category));
00055:                 Assert.True(m.studyHoursRequired > 0, $"{m.manual_id} studyHoursRequired should be > 0");
00056:                 Assert.True(m.fatiguePerHour > 0f, $"{m.manual_id} fatiguePerHour should be > 0");
00057:             }
00058:         }
00059:     }
00060: }
```

## `Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs` — 231 lines; 9,904 bytes; SHA-256 `f67e3c01031b535672055663c72a3eb2d411dd56038c31c16dc7a5598b09cc8a`
Declaration index:
- 00026: public sealed class Plan80_61LibraryTradeIntegrationTests
- 00028: private static string FindDataDir()
- 00036: private static List<ManualDefinition> LoadManuals()
- 00045: private static IReadOnlyList<TradeScreenScenario> LoadScenarios()
- 00053: private static LibraryStudySystem CreateLibrarySystem(
- 00067: public void Plan80_61_TradeManual_AcquisitionAndShelterStudy_RoundTrip()
- 00126: public void Plan80_61_Prerequisites_And_PowerGating_Enforced()
- 00162: public void Plan80_61_TradeFairness_And_KnowledgeValuation()
- 00193: public void Plan80_61_LibraryStudy_SaveRestore_Parity()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using Ashfall.Core;
00008: using Ashfall.Core.Economy;
00009: using Ashfall.Core.Journal;
00010: using Ashfall.Core.Survivors;
00011: using Xunit;
00012:
00013: namespace Ashfall.Core.Tests.Progression
00014: {
00015:     /// <summary>
00016:     /// Wave 39 Batch 1 Integration Suite:
00017:     /// - Plan 80 (DEC-239): Library Manuals & Knowledge Progression
00018:     /// - Plan 61 (DEC-240): Trade Screen Scenarios Expansion
00019:     ///
00020:     /// Validates cross-system integration between wasteland barter negotiation
00021:     /// tables and shelter intellectual development: purchasing technical manuals
00022:     /// from merchants, registering them in the shelter library, enforcing power &
00023:     /// prerequisite gating, progressing survivor study hours, granting skill XP,
00024:     /// and verifying save/restore state roundtrip parity.
00025:     /// </summary>
00026:     public sealed class Plan80_61LibraryTradeIntegrationTests
00027:     {
00028:         private static string FindDataDir()
00029:         {
00030:             string dataDir;
00031:             if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
00032:                 CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
00033:             return dataDir ?? string.Empty;
00034:         }
00035:
00036:         private static List<ManualDefinition> LoadManuals()
00037:         {
00038:             string dataDir = FindDataDir();
00039:             Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");
00040:             var defs = LibraryManualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00041:             Assert.NotNull(defs);
00042:             return defs;
00043:         }
00044:
00045:         private static IReadOnlyList<TradeScreenScenario> LoadScenarios()
00046:         {
00047:             string dataDir = FindDataDir();
00048:             string path = Path.Combine(dataDir, "trade_screen_scenarios.json");
00049:             Assert.True(File.Exists(path), "trade_screen_scenarios.json must exist");
00050:             return TradeScreenScenarioLoader.LoadFromJson(File.ReadAllText(path));
00051:         }
00052:
00053:         private static LibraryStudySystem CreateLibrarySystem(
00054:             out SkillProgressionSystem skills,
00055:             out ResearchSystem research,
00056:             out JournalSystem journal,
00057:             out DutyRosterSystem roster)
00058:         {
00059:             skills = new SkillProgressionSystem();
00060:             research = new ResearchSystem();
00061:             journal = new JournalSystem();
00062:             roster = new DutyRosterSystem();
00063:             return new LibraryStudySystem(skills, research, journal, roster);
00064:         }
00065:
00066:         [Fact]
00067:         public void Plan80_61_TradeManual_AcquisitionAndShelterStudy_RoundTrip()
00068:         {
00069:             var manuals = LoadManuals();
00070:             var scenarios = LoadScenarios();
00071:
00072:             Assert.NotEmpty(manuals);
00073:             Assert.NotEmpty(scenarios);
00074:
00075:             // 1. Verify manuals declare valid categories and metadata
00076:             var waterManual = manuals.Find(m => m.manual_id == "manual_water_filtration");
00077:             Assert.NotNull(waterManual);
00078:             Assert.Equal("survival", waterManual.category);
00079:             Assert.True(waterManual.studyHoursRequired > 0);
00080:
00081:             // 2. Locate a valid trade scenario (e.g. fair_deal)
00082:             var scenario = scenarios[0];
00083:             Assert.NotNull(scenario);
00084:             Assert.False(string.IsNullOrEmpty(scenario.FactionId));
00085:
00086:             // 3. Initialize Shelter Library
00087:             var library = CreateLibrarySystem(out var skills, out var research, out var journal, out var roster);
00088:             library.LoadCatalog(manuals);
00089:
00090:             const string readerId = "dweller_scholar_01";
00091:             Assert.False(library.IsReaderStudying(readerId));
00092:
00093:             // 4. Enroll in study
00094:             var startResult = library.StartStudy(waterManual.manual_id, readerId);
00095:             Assert.Equal(ActionResult.StatusKind.Success, startResult.Status);
00096:             Assert.True(library.IsReaderStudying(readerId));
00097:             Assert.Single(library.State.activeJobs);
00098:
00099:             // 5. Comprehension rate and hours calculation
00100:             float compRate = library.GetComprehensionRate(readerId, waterManual.manual_id);
00101:             Assert.InRange(compRate, 0.75f, 2.0f);
00102:             float effHours = library.GetEffectiveStudyHours(readerId, waterManual.manual_id);
00103:             Assert.True(effHours > 0f);
00104:
00105:             // 6. Complete study via daily tick
00106:             bool eventFired = false;
00107:             library.OnJobCompleted += job =>
00108:             {
00109:                 if (job.manualId == waterManual.manual_id)
00110:                     eventFired = true;
00111:             };
00112:
00113:             // Advance required days
00114:             int daysNeeded = (int)Math.Ceiling(waterManual.studyHoursRequired / 8.0f) + 1;
00115:             for (int d = 0; d < daysNeeded; d++)
00116:             {
00117:                 library.TickDay(d + 1);
00118:             }
00119:
00120:             Assert.True(eventFired, "OnJobCompleted should have fired upon completion");
00121:             Assert.Contains(waterManual.manual_id, library.State.completedManualIds);
00122:             Assert.False(library.IsReaderStudying(readerId));
00123:         }
00124:
00125:         [Fact]
00126:         public void Plan80_61_Prerequisites_And_PowerGating_Enforced()
00127:         {
00128:             var manuals = LoadManuals();
00129:             var library = CreateLibrarySystem(out var skills, out var research, out var journal, out var roster);
00130:             library.LoadCatalog(manuals);
00131:
00132:             // Find manual with prerequisites
00133:             var advancedManual = manuals.Find(m => m.prerequisites != null && m.prerequisites.Count > 0);
00134:             if (advancedManual != null)
00135:             {
00136:                 const string reader = "dweller_apprentice_02";
00137:                 var blockedResult = library.StartStudy(advancedManual.manual_id, reader);
00138:                 Assert.Equal(ActionResult.StatusKind.Blocked, blockedResult.Status);
00139:                 Assert.Equal("missing_prerequisite", blockedResult.FailureCode);
00140:             }
00141:
00142:             // Power gating check
00143:             var poweredManual = manuals.Find(m => m.requiresPower);
00144:             if (poweredManual != null)
00145:             {
00146:                 const string reader = "dweller_electrician_03";
00147:                 // Power cut
00148:                 library.PowerAvailable = () => false;
00149:                 Assert.False(library.IsManualPowered(poweredManual.manual_id));
00150:
00151:                 var powerBlockedResult = library.StartStudy(poweredManual.manual_id, reader);
00152:                 Assert.Equal(ActionResult.StatusKind.Blocked, powerBlockedResult.Status);
00153:                 Assert.Equal("power_unavailable", powerBlockedResult.FailureCode);
00154:
00155:                 // Power restored
00156:                 library.PowerAvailable = () => true;
00157:                 Assert.True(library.IsManualPowered(poweredManual.manual_id));
00158:             }
00159:         }
00160:
00161:         [Fact]
00162:         public void Plan80_61_TradeFairness_And_KnowledgeValuation()
00163:         {
00164:             var scenarios = LoadScenarios();
00165:             Assert.NotEmpty(scenarios);
00166:
00167:             // Test scenario with empty table vs populated table
00168:             var emptyScenario = scenarios.FirstOrDefault(s => s.ExpectedFairness == TradeFairness.EmptyTable);
00169:             if (emptyScenario != null)
00170:             {
00171:                 Assert.Equal(TradeFairness.EmptyTable, emptyScenario.ExpectedFairness);
00172:                 Assert.Equal("EMPTY TABLE", TradeFairnessLabels.For(emptyScenario.ExpectedFairness));
00173:             }
00174:
00175:             var fairScenario = scenarios.FirstOrDefault(s => s.ExpectedFairness == TradeFairness.Fair);
00176:             if (fairScenario != null)
00177:             {
00178:                 Assert.Equal(TradeFairness.Fair, fairScenario.ExpectedFairness);
00179:                 Assert.Equal("DEAL IS FAIR", TradeFairnessLabels.For(fairScenario.ExpectedFairness));
00180:             }
00181:
00182:             // Verify intent sink interaction
00183:             var sink = new MockTradeIntentSink { ConfirmResult = true };
00184:             Assert.True(sink.TryConfirmTrade());
00185:             Assert.Equal(1, sink.ConfirmCalls);
00186:
00187:             sink.Close(traded: true);
00188:             Assert.Equal(1, sink.CloseCalls);
00189:             Assert.True(sink.LastCloseWasTraded);
00190:         }
00191:
00192:         [Fact]
00193:         public void Plan80_61_LibraryStudy_SaveRestore_Parity()
00194:         {
00195:             var manuals = LoadManuals();
00196:             var library1 = CreateLibrarySystem(out var skills1, out var research1, out var journal1, out var roster1);
00197:             library1.LoadCatalog(manuals);
00198:
00199:             var manual = manuals[0];
00200:             const string reader = "dweller_archivist_04";
00201:             library1.StartStudy(manual.manual_id, reader);
00202:
00203:             // Partial progress
00204:             library1.TickDay(1);
00205:
00206:             // Capture state
00207:             var state = library1.CaptureState();
00208:             var serializer = new SystemTextJsonSerializer();
00209:             string json = serializer.Serialize(state);
00210:
00211:             // Restore into fresh system
00212:             var library2 = CreateLibrarySystem(out var skills2, out var research2, out var journal2, out var roster2);
00213:             library2.LoadCatalog(manuals);
00214:             var restoredState = serializer.Deserialize<LibraryStudyState>(json);
00215:             Assert.NotNull(restoredState);
00216:
00217:             library2.RestoreState(restoredState);
00218:
00219:             Assert.Equal(state.totalStudyHours, library2.State.totalStudyHours);
00220:             Assert.Equal(state.completedManualIds.Count, library2.State.completedManualIds.Count);
00221:             Assert.Equal(state.activeJobs.Count, library2.State.activeJobs.Count);
00222:
00223:             if (state.activeJobs.Count > 0)
00224:             {
00225:                 Assert.Equal(state.activeJobs[0].manualId, library2.State.activeJobs[0].manualId);
00226:                 Assert.Equal(state.activeJobs[0].readerId, library2.State.activeJobs[0].readerId);
00227:                 Assert.Equal(state.activeJobs[0].progressHours, library2.State.activeJobs[0].progressHours);
00228:             }
00229:         }
00230:     }
00231: }
```

## `Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs` — 503 lines; 22,746 bytes; SHA-256 `34fd40ed5afa2b721b793af51bff093ddd6eccadcb1407ec78396de550206ba5`
Declaration index:
- 00033: public sealed class Plan80LibraryManualsExpansionTests
- 00040: private static string FindDataDir()
- 00048: private static List<ManualDefinition> LoadManuals()
- 00057: private static HashSet<string> LoadKnowledgeIds()
- 00077: private static LibraryStudySystem CreateSystem(out SkillProgressionSystem skills, out ResearchSystem research, out JournalSystem journal)
- 00098: public void Catalog_LoadsAtLeast24Manuals()
- 00105: public void Catalog_AnchorManualsPreserved()
- 00139: public void Catalog_AllIdsUniqueAndCanonicalPrefix()
- 00152: public void Catalog_AllDisplayNamesNonEmptyAndUnique()
- 00164: public void Catalog_CategoriesAreCanonicalAndCoverAllSixDomains()
- 00177: public void Catalog_NumericBoundsValid()
- 00193: public void Catalog_AllSkillXpGrantsAreValidDisciplineXpPairs()
- 00212: public void Catalog_AllResearchAndKnowledgeUnlocksResolve()
- 00228: public void Catalog_NoDuplicateReferencesWithinOneManual()
- 00241: private static void AssertNoDuplicates(string manualId, string field, List<string> values)
- 00253: public void Graph_PrerequisiteReferencesResolve()
- 00263: public void Graph_IsAcyclic()
- 00293: public void Graph_AllManualsReachableFromFoundations()
- 00326: public void Graph_HasIntermediateAndAdvancedDepth()
- 00367: public void Runtime_PrerequisiteEnforcement_WithRealChain()
- 00390: public void Runtime_CompletionGrantsSkillXpResearchAndKnowledge()
- 00412: public void Runtime_RewardsApplyExactlyOnce_RepeatStudyBlocked()
- 00436: public void Runtime_PartialStudyProgressRoundTripsThroughSave()
- 00467: public void Runtime_ThreeTierBranchCompletesDeterministically()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Text.Json;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Journal;
00008: using Ashfall.Core.Survivors;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.Progression
00012: {
00013:     /// <summary>
00014:     /// Plan 80 — library manual catalog expansion quality gates.
00015:     ///
00016:     /// Repository-truth note (2026-09-06 recon): the library_manuals.json catalog
00017:     /// was expanded by concurrent streams well past the original 3-manual
00018:     /// baseline and now holds 24 manuals across all six canonical categories
00019:     /// (survival / engineering / medical / science / scavenging / combat —
00020:     /// four each). These tests pin that live catalog per the Plan 80 test
00021:     /// catalogue: count, anchor-manual parity, reference resolution, DAG
00022:     /// validity, reachability, tier depth, runtime prerequisite enforcement,
00023:     /// reward atomicity, repeat-study blocking, and save round-trips.
00024:     ///
00025:     /// Category note: the plan draft expected "technical"/"scientific"/"social"
00026:     /// category strings. The canonical categories are the six accepted by
00027:     /// LibraryStudySystem.NormalizeDiscipline and used by every authored
00028:     /// manual; "social" has no discipline mapping and no authored manual.
00029:     /// The plan draft's claim that manual_improvised_weapons requires
00030:     /// manual_water_filtration is stale — current data has it as a foundation
00031:     /// with no prerequisites, and that is what is pinned here.
00032:     /// </summary>
00033:     public sealed class Plan80LibraryManualsExpansionTests
00034:     {
00035:         private static readonly HashSet<string> CanonicalCategories = new(StringComparer.Ordinal)
00036:         {
00037:             "survival", "engineering", "medical", "science", "scavenging", "combat"
00038:         };
00039:
00040:         private static string FindDataDir()
00041:         {
00042:             string dataDir;
00043:             if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
00044:                 CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
00045:             return dataDir ?? string.Empty;
00046:         }
00047:
00048:         private static List<ManualDefinition> LoadManuals()
00049:         {
00050:             string dataDir = FindDataDir();
00051:             Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");
00052:             var defs = LibraryManualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00053:             Assert.NotNull(defs);
00054:             return defs;
00055:         }
00056:
00057:         private static HashSet<string> LoadKnowledgeIds()
00058:         {
00059:             string dataDir = FindDataDir();
00060:             string raw = new FileSystemIO().ReadAllText(Path.Combine(dataDir, "research_knowledge.json"));
00061:             using var doc = JsonDocument.Parse(raw);
00062:             var set = new HashSet<string>(StringComparer.Ordinal);
00063:             if (doc.RootElement.TryGetProperty("knowledge_nodes", out var arr) && arr.ValueKind == JsonValueKind.Array)
00064:             {
00065:                 foreach (var it in arr.EnumerateArray())
00066:                 {
00067:                     if (it.TryGetProperty("id", out var idProp))
00068:                     {
00069:                         string id = idProp.GetString() ?? "";
00070:                         if (!string.IsNullOrEmpty(id)) set.Add(id);
00071:                     }
00072:                 }
00073:             }
00074:             return set;
00075:         }
00076:
00077:         private static LibraryStudySystem CreateSystem(out SkillProgressionSystem skills, out ResearchSystem research, out JournalSystem journal)
00078:         {
00079:             skills = new SkillProgressionSystem();
00080:             research = new ResearchSystem();
00081:             journal = new JournalSystem();
00082:             var roster = new DutyRosterSystem();
00083:             return new LibraryStudySystem(skills, research, journal, roster);
00084:         }
00085:
00086:         private static Dictionary<string, ManualDefinition> IndexById(List<ManualDefinition> manuals)
00087:         {
00088:             var map = new Dictionary<string, ManualDefinition>(StringComparer.Ordinal);
00089:             foreach (var m in manuals) map[m.manual_id] = m;
00090:             return map;
00091:         }
00092:
00093:         // ---------------------------------------------------------------
00094:         // Catalog shape
00095:         // ---------------------------------------------------------------
00096:
00097:         [Fact]
00098:         public void Catalog_LoadsAtLeast24Manuals()
00099:         {
00100:             var manuals = LoadManuals();
00101:             Assert.True(manuals.Count >= 24, $"Expected >= 24 manuals, found {manuals.Count}");
00102:         }
00103:
00104:         [Fact]
00105:         public void Catalog_AnchorManualsPreserved()
00106:         {
00107:             var map = IndexById(LoadManuals());
00108:
00109:             // Original baseline anchors keep their live identity (IDs, hours,
00110:             // prerequisites, power flags). Values match library_manuals.json.
00111:             Assert.True(map.TryGetValue("manual_water_filtration", out var water));
00112:             Assert.Equal("Field Water Filtration", water.display_name);
00113:             Assert.Equal("survival", water.category);
00114:             Assert.Equal(10, water.studyHoursRequired);
00115:             Assert.Equal(0.3f, water.fatiguePerHour);
00116:             Assert.Equal(-0.5f, water.moraleEffect);
00117:             Assert.Empty(water.prerequisites);
00118:             Assert.True(water.requiresPower);
00119:             Assert.Contains("knowledge_water_basics", water.researchUnlocks);
00120:             Assert.Contains("knowledge_water_basics", water.knowledgeUnlocks);
00121:
00122:             Assert.True(map.TryGetValue("manual_rad_first_aid", out var rad));
00123:             Assert.Equal("Radiation First Aid & Dose Mitigation", rad.display_name);
00124:             Assert.Equal("medical", rad.category);
00125:             Assert.Equal(12, rad.studyHoursRequired);
00126:             Assert.Empty(rad.prerequisites);
00127:             Assert.False(rad.requiresPower);
00128:
00129:             Assert.True(map.TryGetValue("manual_improvised_weapons", out var weapons));
00130:             Assert.Equal("Improvised Weapons Fabrication", weapons.display_name);
00131:             Assert.Equal("combat", weapons.category);
00132:             Assert.Equal(14, weapons.studyHoursRequired);
00133:             // Stale plan draft claimed a dependency on manual_water_filtration;
00134:             // the authored catalog intentionally keeps this a foundation manual.
00135:             Assert.Empty(weapons.prerequisites);
00136:         }
00137:
00138:         [Fact]
00139:         public void Catalog_AllIdsUniqueAndCanonicalPrefix()
00140:         {
00141:             var manuals = LoadManuals();
00142:             var seen = new HashSet<string>(StringComparer.Ordinal);
00143:             foreach (var m in manuals)
00144:             {
00145:                 Assert.False(string.IsNullOrWhiteSpace(m.manual_id));
00146:                 Assert.StartsWith("manual_", m.manual_id);
00147:                 Assert.True(seen.Add(m.manual_id), $"Duplicate manual ID: {m.manual_id}");
00148:             }
00149:         }
00150:
00151:         [Fact]
00152:         public void Catalog_AllDisplayNamesNonEmptyAndUnique()
00153:         {
00154:             var manuals = LoadManuals();
00155:             var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00156:             foreach (var m in manuals)
00157:             {
00158:                 Assert.False(string.IsNullOrWhiteSpace(m.display_name));
00159:                 Assert.True(seen.Add(m.display_name), $"Duplicate display name: {m.display_name}");
00160:             }
00161:         }
00162:
00163:         [Fact]
00164:         public void Catalog_CategoriesAreCanonicalAndCoverAllSixDomains()
00165:         {
00166:             var manuals = LoadManuals();
00167:             var categories = new HashSet<string>(StringComparer.Ordinal);
00168:             foreach (var m in manuals)
00169:             {
00170:                 Assert.Contains(m.category, CanonicalCategories);
00171:                 categories.Add(m.category);
00172:             }
00173:             Assert.Equal(CanonicalCategories.Count, categories.Count);
00174:         }
00175:
00176:         [Fact]
00177:         public void Catalog_NumericBoundsValid()
00178:         {
00179:             foreach (var m in LoadManuals())
00180:             {
00181:                 Assert.InRange(m.studyHoursRequired, 5, 25);
00182:                 Assert.InRange(m.fatiguePerHour, 0.10f, 0.60f);
00183:                 Assert.InRange(m.moraleEffect, -1.0f, 1.0f);
00184:                 Assert.InRange(m.technicalComplexityTier, 1, 4);
00185:             }
00186:         }
00187:
00188:         // ---------------------------------------------------------------
00189:         // Reference resolution
00190:         // ---------------------------------------------------------------
00191:
00192:         [Fact]
00193:         public void Catalog_AllSkillXpGrantsAreValidDisciplineXpPairs()
00194:         {
00195:             var validDisciplines = new HashSet<string>(SkillProgressionSystem.Disciplines, StringComparer.Ordinal);
00196:             foreach (var m in LoadManuals())
00197:             {
00198:                 Assert.NotNull(m.skillXpGrants);
00199:                 Assert.True(m.skillXpGrants.Count % 2 == 0,
00200:                     $"Manual {m.manual_id} skillXpGrants has odd length {m.skillXpGrants.Count}");
00201:                 for (int i = 0; i < m.skillXpGrants.Count; i += 2)
00202:                 {
00203:                     Assert.True(validDisciplines.Contains(m.skillXpGrants[i]),
00204:                         $"Manual {m.manual_id} references invalid discipline '{m.skillXpGrants[i]}'");
00205:                     Assert.True(float.TryParse(m.skillXpGrants[i + 1], out float xp) && xp > 0f,
00206:                         $"Manual {m.manual_id} has invalid XP amount '{m.skillXpGrants[i + 1]}'");
00207:                 }
00208:             }
00209:         }
00210:
00211:         [Fact]
00212:         public void Catalog_AllResearchAndKnowledgeUnlocksResolve()
00213:         {
00214:             var validKnowledge = LoadKnowledgeIds();
00215:             Assert.NotEmpty(validKnowledge);
00216:             foreach (var m in LoadManuals())
00217:             {
00218:                 foreach (var r in m.researchUnlocks)
00219:                     Assert.True(validKnowledge.Contains(r),
00220:                         $"Manual {m.manual_id} research unlock '{r}' not found in research_knowledge.json");
00221:                 foreach (var k in m.knowledgeUnlocks)
00222:                     Assert.True(validKnowledge.Contains(k),
00223:                         $"Manual {m.manual_id} knowledge unlock '{k}' not found in research_knowledge.json");
00224:             }
00225:         }
00226:
00227:         [Fact]
00228:         public void Catalog_NoDuplicateReferencesWithinOneManual()
00229:         {
00230:             foreach (var m in LoadManuals())
00231:             {
00232:                 AssertNoDuplicates(m.manual_id, "prerequisites", m.prerequisites);
00233:                 AssertNoDuplicates(m.manual_id, "research_unlocks", m.researchUnlocks);
00234:                 AssertNoDuplicates(m.manual_id, "knowledge_unlocks", m.knowledgeUnlocks);
00235:                 AssertNoDuplicates(m.manual_id, "loot_table_ids", m.lootTableIds);
00236:                 AssertNoDuplicates(m.manual_id, "expedition_reward_ids", m.expeditionRewardIds);
00237:                 AssertNoDuplicates(m.manual_id, "trader_pool_ids", m.traderPoolIds);
00238:             }
00239:         }
00240:
00241:         private static void AssertNoDuplicates(string manualId, string field, List<string> values)
00242:         {
00243:             var seen = new HashSet<string>(StringComparer.Ordinal);
00244:             foreach (var v in values)
00245:                 Assert.True(seen.Add(v), $"Manual {manualId} has duplicate '{v}' in {field}");
00246:         }
00247:
00248:         // ---------------------------------------------------------------
00249:         // Prerequisite graph (DAG / reachability / tiers)
00250:         // ---------------------------------------------------------------
00251:
00252:         [Fact]
00253:         public void Graph_PrerequisiteReferencesResolve()
00254:         {
00255:             var map = IndexById(LoadManuals());
00256:             foreach (var m in map.Values)
00257:                 foreach (var p in m.prerequisites)
00258:                     Assert.True(map.ContainsKey(p),
00259:                         $"Missing prerequisite '{p}' for manual '{m.manual_id}'");
00260:         }
00261:
00262:         [Fact]
00263:         public void Graph_IsAcyclic()
00264:         {
00265:             var map = IndexById(LoadManuals());
00266:             var state = new Dictionary<string, int>(StringComparer.Ordinal); // 0=unvisited 1=visiting 2=done
00267:             foreach (var id in map.Keys) state[id] = 0;
00268:
00269:             void Dfs(string current, List<string> path)
00270:             {
00271:                 state[current] = 1;
00272:                 path.Add(current);
00273:                 foreach (var prereq in map[current].prerequisites)
00274:                 {
00275:                     if (state[prereq] == 1)
00276:                     {
00277:                         path.Add(prereq);
00278:                         Assert.Fail($"Cycle in manual prerequisites: {string.Join(" -> ", path)}");
00279:                     }
00280:                     if (state[prereq] == 0)
00281:                         Dfs(prereq, path);
00282:                 }
00283:                 path.RemoveAt(path.Count - 1);
00284:                 state[current] = 2;
00285:             }
00286:
00287:             foreach (var id in map.Keys)
00288:                 if (state[id] == 0)
00289:                     Dfs(id, new List<string>());
00290:         }
00291:
00292:         [Fact]
00293:         public void Graph_AllManualsReachableFromFoundations()
00294:         {
00295:             var manuals = LoadManuals();
00296:             var map = IndexById(manuals);
00297:             var foundations = new HashSet<string>(StringComparer.Ordinal);
00298:             foreach (var m in manuals)
00299:                 if (m.prerequisites.Count == 0)
00300:                     foundations.Add(m.manual_id);
00301:
00302:             Assert.True(foundations.Count >= 4, $"Expected >= 4 foundation manuals, found {foundations.Count}");
00303:
00304:             var completed = new HashSet<string>(foundations, StringComparer.Ordinal);
00305:             bool progress = true;
00306:             while (progress)
00307:             {
00308:                 progress = false;
00309:                 foreach (var m in manuals)
00310:                 {
00311:                     if (completed.Contains(m.manual_id)) continue;
00312:                     bool canComplete = true;
00313:                     foreach (var p in m.prerequisites)
00314:                         if (!completed.Contains(p)) { canComplete = false; break; }
00315:                     if (canComplete)
00316:                     {
00317:                         completed.Add(m.manual_id);
00318:                         progress = true;
00319:                     }
00320:                 }
00321:             }
00322:             Assert.Equal(map.Count, completed.Count);
00323:         }
00324:
00325:         [Fact]
00326:         public void Graph_HasIntermediateAndAdvancedDepth()
00327:         {
00328:             // Longest-prerequisite-chain depth: foundations = 0. The authored
00329:             // graph carries meaningful depth beyond a single flat tier —
00330:             // e.g. manual_water_filtration -> manual_bunker_hydroponics ->
00331:             // manual_apiculture_and_pollination (depth 2), and
00332:             // manual_rad_first_aid -> manual_quarantine_epidemiology ->
00333:             // manual_pharmacology_synthesis (depth 2).
00334:             var map = IndexById(LoadManuals());
00335:             var memo = new Dictionary<string, int>(StringComparer.Ordinal);
00336:
00337:             int Depth(string id)
00338:             {
00339:                 if (memo.TryGetValue(id, out var d)) return d;
00340:                 memo[id] = 0; // guard (graph is acyclic; safe default)
00341:                 var prereqs = map[id].prerequisites;
00342:                 int result = prereqs.Count == 0 ? 0 : 1 + MaxDepth(prereqs, Depth);
00343:                 memo[id] = result;
00344:                 return result;
00345:             }
00346:
00347:             static int MaxDepth(List<string> ids, Func<string, int> f)
00348:             {
00349:                 int max = 0;
00350:                 foreach (var id in ids) max = Math.Max(max, f(id));
00351:                 return max;
00352:             }
00353:
00354:             int maxDepth = 0;
00355:             foreach (var id in map.Keys) maxDepth = Math.Max(maxDepth, Depth(id));
00356:
00357:             Assert.True(maxDepth >= 2, $"Expected prerequisite depth >= 2, found {maxDepth}");
00358:             Assert.True(Depth("manual_apiculture_and_pollination") == 2, "Apiculture should be a depth-2 advanced manual");
00359:             Assert.True(Depth("manual_pharmacology_synthesis") == 2, "Pharmacology should be a depth-2 advanced manual");
00360:         }
00361:
00362:         // ---------------------------------------------------------------
00363:         // Runtime study behavior
00364:         // ---------------------------------------------------------------
00365:
00366:         [Fact]
00367:         public void Runtime_PrerequisiteEnforcement_WithRealChain()
00368:         {
00369:             var sys = CreateSystem(out _, out _, out _);
00370:             LibraryManualCatalogLoader.LoadAndRegister(
00371:                 sys, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00372:
00373:             // manual_bunker_hydroponics requires manual_water_filtration.
00374:             var blocked = sys.StartStudy("manual_bunker_hydroponics", "survivor_bob");
00375:             Assert.False(blocked.IsSuccess);
00376:             Assert.Equal("missing_prerequisite", blocked.FailureCode);
00377:
00378:             // Complete the foundation (10h at 8h/day -> completes day 2).
00379:             Assert.True(sys.StartStudy("manual_water_filtration", "survivor_bob").IsSuccess);
00380:             sys.TickDay(1);
00381:             Assert.False(sys.IsManualCompleted("manual_water_filtration"));
00382:             sys.TickDay(2);
00383:             Assert.True(sys.IsManualCompleted("manual_water_filtration"));
00384:
00385:             // The intermediate manual is now eligible.
00386:             Assert.True(sys.StartStudy("manual_bunker_hydroponics", "survivor_bob").IsSuccess);
00387:         }
00388:
00389:         [Fact]
00390:         public void Runtime_CompletionGrantsSkillXpResearchAndKnowledge()
00391:         {
00392:             var sys = CreateSystem(out var skills, out var research, out var journal);
00393:             LibraryManualCatalogLoader.LoadAndRegister(
00394:                 sys, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00395:
00396:             int codexBefore = journal.CodexUnlockCount;
00397:             Assert.True(sys.StartStudy("manual_water_filtration", "dweller_alec").IsSuccess);
00398:             sys.TickDay(1);
00399:             Assert.False(sys.IsManualCompleted("manual_water_filtration"));
00400:             sys.TickDay(2);
00401:             Assert.True(sys.IsManualCompleted("manual_water_filtration"));
00402:
00403:             // Skill XP granted to the reader in the manual's discipline (survival, 25).
00404:             Assert.True(skills.GetXp("dweller_alec", "survival") >= 25f);
00405:             // Research unlock applied.
00406:             Assert.True(research.IsManualUnlocked("knowledge_water_basics"));
00407:             // Knowledge evidence recorded via the codex ledger.
00408:             Assert.True(journal.CodexUnlockCount > codexBefore);
00409:         }
00410:
00411:         [Fact]
00412:         public void Runtime_RewardsApplyExactlyOnce_RepeatStudyBlocked()
00413:         {
00414:             var sys = CreateSystem(out var skills, out var research, out _);
00415:             LibraryManualCatalogLoader.LoadAndRegister(
00416:                 sys, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00417:
00418:             Assert.True(sys.StartStudy("manual_water_filtration", "dweller_alec").IsSuccess);
00419:             sys.TickDay(1);
00420:             sys.TickDay(2);
00421:             Assert.True(sys.IsManualCompleted("manual_water_filtration"));
00422:             float xpAfterCompletion = skills.GetXp("dweller_alec", "survival");
00423:
00424:             // Extra ticks must not re-grant anything.
00425:             sys.TickDay(3);
00426:             Assert.Equal(xpAfterCompletion, skills.GetXp("dweller_alec", "survival"));
00427:             Assert.True(research.IsManualUnlocked("knowledge_water_basics"));
00428:
00429:             // Restudying a completed manual is blocked (no infinite XP farm).
00430:             var again = sys.StartStudy("manual_water_filtration", "dweller_alec");
00431:             Assert.Equal(ActionResult.StatusKind.Blocked, again.Status);
00432:             Assert.Equal("already_completed", again.FailureCode);
00433:         }
00434:
00435:         [Fact]
00436:         public void Runtime_PartialStudyProgressRoundTripsThroughSave()
00437:         {
00438:             var sys1 = CreateSystem(out _, out _, out _);
00439:             LibraryManualCatalogLoader.LoadAndRegister(
00440:                 sys1, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00441:
00442:             Assert.True(sys1.StartStudy("manual_water_filtration", "reader_alice").IsSuccess);
00443:             sys1.TickDay(1); // 8h of 10h
00444:
00445:             var state = sys1.CaptureState();
00446:             Assert.Single(state.activeJobs);
00447:             Assert.Equal("manual_water_filtration", state.activeJobs[0].manualId);
00448:             Assert.Equal(8f, state.activeJobs[0].progressHours);
00449:
00450:             var sys2 = CreateSystem(out _, out _, out _);
00451:             // Production loads the catalog at startup before applying a save,
00452:             // so the restored system must have the catalog registered too.
00453:             LibraryManualCatalogLoader.LoadAndRegister(
00454:                 sys2, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00455:             sys2.RestoreState(state);
00456:             Assert.Single(sys2.State.activeJobs);
00457:             Assert.Equal("manual_water_filtration", sys2.State.activeJobs[0].manualId);
00458:             Assert.Equal(8f, sys2.State.activeJobs[0].progressHours);
00459:             Assert.False(sys2.IsManualCompleted("manual_water_filtration"));
00460:
00461:             // Resumed study completes normally on the next tick.
00462:             sys2.TickDay(2);
00463:             Assert.True(sys2.IsManualCompleted("manual_water_filtration"));
00464:         }
00465:
00466:         [Fact]
00467:         public void Runtime_ThreeTierBranchCompletesDeterministically()
00468:         {
00469:             var sys = CreateSystem(out var skills, out var research, out _);
00470:             LibraryManualCatalogLoader.LoadAndRegister(
00471:                 sys, FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00472:
00473:             string[] branch = { "manual_water_filtration", "manual_bunker_hydroponics", "manual_apiculture_and_pollination" };
00474:             var captured = new List<LibraryStudyState>();
00475:
00476:             // Prerequisite gating is campaign-wide: before any study happens,
00477:             // tier-2/3 manuals must be blocked for any reader.
00478:             foreach (var manualId in new[] { branch[1], branch[2] })
00479:             {
00480:                 var probe = sys.StartStudy(manualId, "probe_reader");
00481:                 Assert.Equal(ActionResult.StatusKind.Blocked, probe.Status);
00482:                 Assert.Equal("missing_prerequisite", probe.FailureCode);
00483:             }
00484:
00485:             for (int i = 0; i < branch.Length; i++)
00486:             {
00487:                 Assert.True(sys.StartStudy(branch[i], "dweller_alec").IsSuccess);
00488:                 // Snapshot save-state between tiers.
00489:                 captured.Add(sys.CaptureState());
00490:
00491:                 // Advance until completion (max 3 days; none of these exceed 24h at rate 1.0).
00492:                 for (int day = 1; day <= 3 && !sys.IsManualCompleted(branch[i]); day++)
00493:                     sys.TickDay(i * 10 + day);
00494:                 Assert.True(sys.IsManualCompleted(branch[i]), $"{branch[i]} should complete within 3 study days");
00495:             }
00496:
00497:             Assert.True(research.IsManualUnlocked("knowledge_water_basics"));
00498:             Assert.True(research.IsManualUnlocked("knowledge_hydroponics"));
00499:             Assert.True(research.IsManualUnlocked("knowledge_apiculture_ecology"));
00500:             Assert.True(skills.GetXp("dweller_alec", "survival") >= 25f + 30f + 35f);
00501:         }
00502:     }
00503: }
```

## `Ashfall.Core.Tests/NewCatalogLoaderTests.cs` — 237 lines; 9,938 bytes; SHA-256 `ef7e90aaddfc4ee52ebfeada46088561127baedd869f1b1c304ace9ceb43185e`
Declaration index:
- 00021: public class ShelterScheduleCatalogLoaderTests
- 00023: private static string FindDataDir()
- 00032: public void LoadsThreeSchedules_FromRealJson()
- 00047: public void LoadAndRegister_PopulatesCoreCatalog()
- 00066: public void ReturnsEmpty_WhenMissing()
- 00075: public class AutopsyProcedureCatalogLoaderTests
- 00077: private static string FindDataDir()
- 00086: public void LoadsThreeProcedures_FromRealJson()
- 00101: public void LoadAndRegister_PopulatesCoreCatalog()
- 00124: public void ReturnsEmpty_WhenMissing()
- 00133: public class LibraryManualCatalogLoaderTests
- 00135: private static string FindDataDir()
- 00144: public void LoadsThreeManuals_FromRealJson()
- 00159: public void LoadAndRegister_PopulatesCoreCatalog()
- 00178: public void ReturnsEmpty_WhenMissing()
- 00187: public class ArchiveInkCatalogLoaderTests
- 00189: private static string FindDataDir()
- 00198: public void LoadsThreeInks_FromRealJson()
- 00213: public void LoadAndRegister_PopulatesCoreCatalog()
- 00229: public void ReturnsEmpty_WhenMissing()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // BUG-02 regression tests: the 4 newly-authored catalog loaders
00003: // (ShelterSchedule, Autopsy, LibraryStudy, ArchiveDesk) must load real
00004: // JSON from StreamingAssets/Data and register into their Core systems.
00005: // Pattern follows BlackFlotillaTests loader tests: locate data dir via
00006: // CatalogLocator, use FileSystemIO + SystemTextJsonSerializer ports.
00007: #nullable disable
00008:
00009: using System;
00010: using Xunit;
00011: using Ashfall.Core;
00012: using Ashfall.Core.Shelter;
00013: using Ashfall.Core.Radiation;
00014: using Ashfall.Core.StartingLevel;
00015: using Ashfall.Core.Medical;
00016: using Ashfall.Core.Survivors;
00017: using Ashfall.Core.Journal;
00018:
00019: namespace Ashfall.Core.Tests
00020: {
00021:     public class ShelterScheduleCatalogLoaderTests
00022:     {
00023:         private static string FindDataDir()
00024:         {
00025:             string dataDir;
00026:             if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
00027:                 CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
00028:             return dataDir ?? string.Empty;
00029:         }
00030:
00031:         [Fact]
00032:         public void LoadsThreeSchedules_FromRealJson()
00033:         {
00034:             string dataDir = FindDataDir();
00035:             if (string.IsNullOrEmpty(dataDir)) return;
00036:
00037:             var io = new FileSystemIO();
00038:             var json = new SystemTextJsonSerializer();
00039:             var defs = ShelterScheduleCatalogLoader.Load(dataDir, io, json);
00040:             Assert.True(defs.Count >= 3, $"expected >= 3 schedules, got {defs.Count}");
00041:             Assert.Contains(defs, d => d.schedule_id == "schedule_standard");
00042:             Assert.Contains(defs, d => d.schedule_id == "schedule_night_shift");
00043:             Assert.Contains(defs, d => d.schedule_id == "schedule_curfew_locked");
00044:         }
00045:
00046:         [Fact]
00047:         public void LoadAndRegister_PopulatesCoreCatalog()
00048:         {
00049:             string dataDir = FindDataDir();
00050:             if (string.IsNullOrEmpty(dataDir)) return;
00051:
00052:             var state = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
00053:             var rooms = new System.Collections.Generic.List<PowerGridRoom> { new PowerGridRoom("room_main", "Main Vault", 100f) };
00054:             var grid = new PowerGridSystem(state, rooms, new SeededRng(42));
00055:             var system = new ShelterScheduleSystem(grid);
00056:
00057:             int count = ShelterScheduleCatalogLoader.LoadAndRegister(system, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00058:             Assert.True(count >= 3, $"expected >= 3 loaded, got {count}");
00059:
00060:             // The default baked-in schedule is always present; the catalog adds more.
00061:             var setRes = system.SetSchedule("schedule_standard");
00062:             Assert.True(setRes.IsSuccess, "SetSchedule should succeed after catalog load");
00063:         }
00064:
00065:         [Fact]
00066:         public void ReturnsEmpty_WhenMissing()
00067:         {
00068:             var io = new FileSystemIO();
00069:             var json = new SystemTextJsonSerializer();
00070:             var result = ShelterScheduleCatalogLoader.Load("/nonexistent", io, json);
00071:             Assert.Empty(result);
00072:         }
00073:     }
00074:
00075:     public class AutopsyProcedureCatalogLoaderTests
00076:     {
00077:         private static string FindDataDir()
00078:         {
00079:             string dataDir;
00080:             if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
00081:                 CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
00082:             return dataDir ?? string.Empty;
00083:         }
00084:
00085:         [Fact]
00086:         public void LoadsThreeProcedures_FromRealJson()
00087:         {
00088:             string dataDir = FindDataDir();
00089:             if (string.IsNullOrEmpty(dataDir)) return;
00090:
00091:             var io = new FileSystemIO();
00092:             var json = new SystemTextJsonSerializer();
00093:             var defs = AutopsyProcedureCatalogLoader.Load(dataDir, io, json);
00094:             Assert.True(defs.Count >= 3, $"expected >= 3 procedures, got {defs.Count}");
00095:             Assert.Contains(defs, d => d.procedure_id == "procedure_rad_pathology");
00096:             Assert.Contains(defs, d => d.procedure_id == "procedure_toxicology");
00097:             Assert.Contains(defs, d => d.procedure_id == "procedure_containment_autopsy");
00098:         }
00099:
00100:         [Fact]
00101:         public void LoadAndRegister_PopulatesCoreCatalog()
00102:         {
00103:             string dataDir = FindDataDir();
00104:             if (string.IsNullOrEmpty(dataDir)) return;
00105:
00106:             var inv = new Ashfall.Core.Inventory.Inventory();
00107:             var rad = new RadiationSystem(seed: 42);
00108:             var starting = new StartingLevelSystem();
00109:             var vent = new VentilationSystem(starting);
00110:             var res = new ResearchSystem();
00111:             var wardState = new MedicalWardState();
00112:             var bed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.General);
00113:             var proc = new MedicalProcedureDef("proc_1", "Procedure 1", "MedicalSystem");
00114:             var medical = new MedicalWardSystem(wardState, new[] { bed }, new[] { proc });
00115:             var system = new AutopsySystem(new SeededRng(42), inv, rad, vent, res, medical);
00116:
00117:
00118:
00119:             int count = AutopsyProcedureCatalogLoader.LoadAndRegister(system, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00120:             Assert.True(count >= 3, $"expected >= 3 loaded, got {count}");
00121:         }
00122:
00123:         [Fact]
00124:         public void ReturnsEmpty_WhenMissing()
00125:         {
00126:             var io = new FileSystemIO();
00127:             var json = new SystemTextJsonSerializer();
00128:             var result = AutopsyProcedureCatalogLoader.Load("/nonexistent", io, json);
00129:             Assert.Empty(result);
00130:         }
00131:     }
00132:
00133:     public class LibraryManualCatalogLoaderTests
00134:     {
00135:         private static string FindDataDir()
00136:         {
00137:             string dataDir;
00138:             if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
00139:                 CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
00140:             return dataDir ?? string.Empty;
00141:         }
00142:
00143:         [Fact]
00144:         public void LoadsThreeManuals_FromRealJson()
00145:         {
00146:             string dataDir = FindDataDir();
00147:             if (string.IsNullOrEmpty(dataDir)) return;
00148:
00149:             var io = new FileSystemIO();
00150:             var json = new SystemTextJsonSerializer();
00151:             var defs = LibraryManualCatalogLoader.Load(dataDir, io, json);
00152:             Assert.True(defs.Count >= 3, $"expected >= 3 manuals, got {defs.Count}");
00153:             Assert.Contains(defs, d => d.manual_id == "manual_water_filtration");
00154:             Assert.Contains(defs, d => d.manual_id == "manual_rad_first_aid");
00155:             Assert.Contains(defs, d => d.manual_id == "manual_improvised_weapons");
00156:         }
00157:
00158:         [Fact]
00159:         public void LoadAndRegister_PopulatesCoreCatalog()
00160:         {
00161:             string dataDir = FindDataDir();
00162:             if (string.IsNullOrEmpty(dataDir)) return;
00163:
00164:             var skills = new SkillProgressionSystem();
00165:             var research = new ResearchSystem();
00166:             var journal = new JournalSystem();
00167:             var roster = new DutyRosterSystem();
00168:             var system = new LibraryStudySystem(skills, research, journal, roster);
00169:
00170:             int count = LibraryManualCatalogLoader.LoadAndRegister(system, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00171:             Assert.True(count >= 3, $"expected >= 3 loaded, got {count}");
00172:
00173:             var startRes = system.StartStudy("manual_water_filtration", "reader_1");
00174:             Assert.True(startRes.IsSuccess, "StartStudy should succeed after catalog load");
00175:         }
00176:
00177:         [Fact]
00178:         public void ReturnsEmpty_WhenMissing()
00179:         {
00180:             var io = new FileSystemIO();
00181:             var json = new SystemTextJsonSerializer();
00182:             var result = LibraryManualCatalogLoader.Load("/nonexistent", io, json);
00183:             Assert.Empty(result);
00184:         }
00185:     }
00186:
00187:     public class ArchiveInkCatalogLoaderTests
00188:     {
00189:         private static string FindDataDir()
00190:         {
00191:             string dataDir;
00192:             if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
00193:                 CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
00194:             return dataDir ?? string.Empty;
00195:         }
00196:
00197:         [Fact]
00198:         public void LoadsThreeInks_FromRealJson()
00199:         {
00200:             string dataDir = FindDataDir();
00201:             if (string.IsNullOrEmpty(dataDir)) return;
00202:
00203:             var io = new FileSystemIO();
00204:             var json = new SystemTextJsonSerializer();
00205:             var defs = ArchiveInkCatalogLoader.Load(dataDir, io, json);
00206:             Assert.True(defs.Count >= 3, $"expected >= 3 inks, got {defs.Count}");
00207:             Assert.Contains(defs, d => d.ink_id == "ink_iron_gall");
00208:             Assert.Contains(defs, d => d.ink_id == "ink_soot_lamp");
00209:             Assert.Contains(defs, d => d.ink_id == "ink_plant_dye");
00210:         }
00211:
00212:         [Fact]
00213:         public void LoadAndRegister_PopulatesCoreCatalog()
00214:         {
00215:             string dataDir = FindDataDir();
00216:             if (string.IsNullOrEmpty(dataDir)) return;
00217:
00218:             var journal = new JournalSystem();
00219:             var knowledge = new KnowledgeBase();
00220:             var inv = new Ashfall.Core.Inventory.Inventory();
00221:             var roster = new DutyRosterSystem();
00222:             var system = new ArchiveDeskSystem(journal, knowledge, inv, roster);
00223:
00224:             int count = ArchiveInkCatalogLoader.LoadAndRegister(system, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00225:             Assert.True(count >= 3, $"expected >= 3 loaded, got {count}");
00226:         }
00227:
00228:         [Fact]
00229:         public void ReturnsEmpty_WhenMissing()
00230:         {
00231:             var io = new FileSystemIO();
00232:             var json = new SystemTextJsonSerializer();
00233:             var result = ArchiveInkCatalogLoader.Load("/nonexistent", io, json);
00234:             Assert.Empty(result);
00235:         }
00236:     }
00237: }
```

## `Ashfall.Core.Tests/Integration/Plans60To63ThirtyDayIntegrationTests.cs` — 298 lines; 14,839 bytes; SHA-256 `f439d2c1551bf2b1c75d124770f7297fb0cd5411710b5e453c8435a01d20034e`
Declaration index:
- 00023: public class Plans60To63ThirtyDayIntegrationTests
- 00025: private static string LocateDataDir()
- 00036: public void FullCampaign_30Day_Plans60To63_IntegratedPipeline()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // Plans 60–63 (Batch B1–B4) 30-Day Cross-System Campaign Integration Tests.
00003: // Verifies all four flagship pillars working in unison across a simulated 30-day campaign:
00004: // - B1: Radio Station JSON-driven authority, 24h schedules, and signal profiles.
00005: // - B2: Library Study manual expansion, reader availability reservation, and research discovery.
00006: // - B3: Tactical Combat encounter loop, mid-encounter save replay determinism, and exactly-once aftermath.
00007: // - B4: Disease Quarantine Policy, 8-stage clinical progression, medical ward isolation, and care consumption.
00008: using System;
00009: using System.Collections.Generic;
00010: using System.IO;
00011: using Ashfall.Core;
00012: using Ashfall.Core.Combat;
00013: using Ashfall.Core.Disease;
00014: using Ashfall.Core.Inventory;
00015: using Ashfall.Core.Journal;
00016: using Ashfall.Core.Medical;
00017: using Ashfall.Core.Radio;
00018: using Ashfall.Core.Survivors;
00019: using Xunit;
00020:
00021: namespace Ashfall.Core.Tests.Integration
00022: {
00023:     public class Plans60To63ThirtyDayIntegrationTests
00024:     {
00025:         private static string LocateDataDir()
00026:         {
00027:             string start = Directory.GetCurrentDirectory();
00028:             if (CatalogLocator.TryFindDataDirectory(start, out string found))
00029:                 return found;
00030:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
00031:                 return found;
00032:             throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data from test run");
00033:         }
00034:
00035:         [Fact]
00036:         public void FullCampaign_30Day_Plans60To63_IntegratedPipeline()
00037:         {
00038:             string dataDir = LocateDataDir();
00039:             const int masterSeed = 6063;
00040:             var masterRng = new SeededRng(masterSeed);
00041:
00042:             // =================================================================
00043:             // 1. PILLAR B1 — RADIO STATION AUTHORITY (Plan 60)
00044:             // =================================================================
00045:             var radioCatalog = new RadioStationCatalog();
00046:             int radioLoaded = RadioStationCatalogLoader.LoadAndRegister(radioCatalog, dataDir);
00047:             Assert.True(radioLoaded >= 6, $"Expected at least 6 radio stations loaded, got {radioLoaded}");
00048:             var cdStation = radioCatalog.GetStation("station_civil_defense");
00049:             Assert.NotNull(cdStation);
00050:
00051:             // =================================================================
00052:             // 2. PILLAR B2 — LIBRARY MANUAL STUDY & RESEARCH (Plan 61)
00053:             // =================================================================
00054:             var skills = new SkillProgressionSystem();
00055:             var research = new ResearchSystem();
00056:             var journal = new JournalSystem();
00057:             var dutyRoster = new DutyRosterSystem(101);
00058:             dutyRoster.Unlock(1);
00059:
00060:             var library = new LibraryStudySystem(skills, research, journal, dutyRoster);
00061:             var manuals = LibraryManualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00062:             library.LoadCatalog(manuals);
00063:             Assert.True(manuals.Count >= 24, $"Expected >= 24 library manuals, got {manuals.Count}");
00064:
00065:             // Register researcher survivor
00066:             const string researcherId = "surv_marie";
00067:             dutyRoster.WriteName(researcherId, "Marie Curie", "Archivist", DutyRosterSystem.ScriptPencil, 1, true);
00068:
00069:             // Start studying water filtration manual
00070:             string manualId = "manual_water_filtration";
00071:             var startStudy = library.StartStudy(manualId, researcherId);
00072:             Assert.True(startStudy.IsSuccess, $"Failed to start study: {startStudy.FailureCode}");
00073:
00074:             // Verify reader availability is reserved in DutyRoster
00075:             Assert.True(dutyRoster.IsSurvivorReservedExternally?.Invoke(researcherId) == true,
00076:                 "Active library study must reserve survivor in DutyRoster");
00077:
00078:             // =================================================================
00079:             // 3. PILLAR B3 — TACTICAL COMBAT SETUP (Plan 62)
00080:             // =================================================================
00081:             CombatCatalog.SeedDefaults();
00082:             var combatSystem = new TacticalCombatSystem();
00083:
00084:             // =================================================================
00085:             // 4. PILLAR B4 — MEDICAL WARD & DISEASE QUARANTINE (Plan 63)
00086:             // =================================================================
00087:             var beds = new List<MedicalBed>
00088:             {
00089:                 new MedicalBed("bed_iso_1", "Isolation Chamber 1", MedicalBedCategory.Isolation, isolation: true),
00090:                 new MedicalBed("bed_iso_2", "Isolation Chamber 2", MedicalBedCategory.Isolation, isolation: true),
00091:                 new MedicalBed("bed_gen_1", "General Bed 1", MedicalBedCategory.General, isolation: false),
00092:             };
00093:             var procedures = new List<MedicalProcedureDef>();
00094:             var medicalWard = new MedicalWardSystem(new MedicalWardState(), beds, procedures);
00095:
00096:             var diseaseCatalog = DiseaseCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00097:             var diseaseSystem = new DiseaseSystem(new DiseaseSystemState(), new SeededRng(masterSeed));
00098:             diseaseSystem.BindCatalog(diseaseCatalog);
00099:
00100:             var inventory = new Dictionary<string, int>(StringComparer.Ordinal)
00101:             {
00102:                 { "clean_water", 200 },
00103:                 { "canned_food", 200 },
00104:                 { "medical_kit", 50 },
00105:                 { "antibiotics", 50 },
00106:                 { "ammo_556", 200 }
00107:             };
00108:
00109:             Func<string, int, bool> tryConsume = (item, qty) =>
00110:             {
00111:                 if (inventory.TryGetValue(item, out int count) && count >= qty)
00112:                 {
00113:                     inventory[item] = count - qty;
00114:                     return true;
00115:                 }
00116:                 return false;
00117:             };
00118:
00119:             diseaseSystem.TryConsumeItem = tryConsume;
00120:
00121:             var quarantineCoord = new DiseaseQuarantineCoordinator(
00122:                 medicalWard,
00123:                 diseaseSystem,
00124:                 dutyRoster,
00125:                 tryConsume,
00126:                 () => ContainmentCapability.FromResearch(k => research.IsManualUnlocked(k)));
00127:
00128:             // Register patrol survivor
00129:             const string soldierId = "surv_kane";
00130:             dutyRoster.WriteName(soldierId, "Kane Vance", "Scout", DutyRosterSystem.ScriptPencil, 1, true);
00131:             dutyRoster.Assign(DutyRosterIds.RoleNightWatch, soldierId);
00132:
00133:             // =================================================================
00134:             // 5. 30-DAY CAMPAIGN EXECUTION
00135:             // =================================================================
00136:             var allSurvivors = new List<string> { researcherId, soldierId, "surv_alec", "surv_mira" };
00137:             for (int i = 2; i < allSurvivors.Count; i++)
00138:             {
00139:                 dutyRoster.WriteName(allSurvivors[i], $"Survivor {i}", "Worker", DutyRosterSystem.ScriptPencil, 1, true);
00140:             }
00141:
00142:             int combatEncounterDay = 15;
00143:             int pathogenExposureDay = 10;
00144:             bool studyCompleted = false;
00145:             bool combatResolved = false;
00146:             bool patientCured = false;
00147:
00148:             for (int day = 1; day <= 30; day++)
00149:             {
00150:                 // A. Radio schedule check
00151:                 for (int h = 0; h < 24; h += 6)
00152:                 {
00153:                     var slot = cdStation.GetCurrentSlot(day, h);
00154:                     Assert.NotNull(slot);
00155:                 }
00156:
00157:                 // B. Library Study progression
00158:                 if (!studyCompleted)
00159:                 {
00160:                     library.TickDay(day);
00161:                     if (library.State.completedManualIds.Contains(manualId))
00162:                     {
00163:                         studyCompleted = true;
00164:                         // Verified that manual unlocked research node
00165:                         var manualDef = manuals.Find(m => m.manual_id == manualId);
00166:                         Assert.NotNull(manualDef);
00167:                         foreach (var node in manualDef!.researchUnlocks)
00168:                         {
00169:                             Assert.True(research.IsManualUnlocked(node), $"Node {node} should be unlocked by manual");
00170:                             Assert.False(research.State.completedIds.Contains(node), "Manual should discover/reveal node, NEVER CompleteResearch()");
00171:                         }
00172:                     }
00173:                 }
00174:
00175:                 // C. Tactical Combat encounter on Day 15
00176:                 if (day == combatEncounterDay && !combatResolved)
00177:                 {
00178:                     var combatRoster = new List<CombatantState>
00179:                     {
00180:                         new CombatantState
00181:                         {
00182:                             Id = "actor_soldier",
00183:                             Name = "Kane",
00184:                             SurvivorId = soldierId,
00185:                             WeaponInstanceId = "inst_rifle_1",
00186:                             IsPlayer = true,
00187:                             Health = 100,
00188:                             MaxHealth = 100,
00189:                             ArmorRating = 0.4f
00190:                         }
00191:                     };
00192:                     var combatWeapons = new List<WeaponInstanceState>
00193:                     {
00194:                         new WeaponInstanceState
00195:                         {
00196:                             InstanceId = "inst_rifle_1",
00197:                             WeaponId = "weapon_assault_rifle",
00198:                             OwnerSurvivorId = soldierId,
00199:                             OwnerCombatantId = "actor_soldier",
00200:                             ConditionPct = 1.0f,
00201:                             AmmoId = "ammo_556",
00202:                             AmmoRemaining = 30
00203:                         }
00204:                     };
00205:
00206:                     combatSystem.BeginEncounter("enc_day15", "exp_1", "loc_perimeter", "Perimeter Fence", day, masterSeed, combatRoster, combatWeapons, enemyCount: 1, enemyHealth: 25);
00207:
00208:                     // Mid-encounter save & reload test
00209:                     var savedCombatState = combatSystem.CaptureState();
00210:                     var restoredCombat = new TacticalCombatSystem();
00211:                     restoredCombat.RestoreState(savedCombatState);
00212:
00213:                     // Resolve both to verify identical outcome
00214:                     var simRng1 = new SeededRng(masterSeed);
00215:                     var simRng2 = new SeededRng(masterSeed);
00216:                     var events1 = combatSystem.ResolveToEnd(simRng1, 60);
00217:                     var events2 = restoredCombat.ResolveToEnd(simRng2, 60);
00218:
00219:                     Assert.Equal(events1.Count, events2.Count);
00220:
00221:                     var aftermath = combatSystem.State.Aftermath;
00222:                     Assert.NotNull(aftermath);
00223:                     Assert.Equal("enc_day15", aftermath!.EncounterId);
00224:
00225:                     combatResolved = true;
00226:                 }
00227:
00228:                 // D. Disease exposure on Day 10
00229:                 if (day == pathogenExposureDay)
00230:                 {
00231:                     var expRes = diseaseSystem.TryInfect("surv_alec", DiseaseIds.Cholera, day, "foul_water_draw");
00232:                     Assert.True(expRes.Infected || expRes.Reason == "roll_passed");
00233:                     // Ensure infected for test flow
00234:                     if (!diseaseSystem.IsInfected("surv_alec", DiseaseIds.Cholera))
00235:                         diseaseSystem.Infect("surv_alec", DiseaseIds.Cholera, day);
00236:
00237:                     // Isolate in medical ward
00238:                     var assignPreview = quarantineCoord.PreviewAssignIsolation("surv_alec");
00239:                     Assert.True(assignPreview.CanExecute);
00240:                     var assignRes = quarantineCoord.ExecuteAssignIsolation("surv_alec", day);
00241:                     Assert.True(assignRes.Success);
00242:                     Assert.True(quarantineCoord.IsIsolated("surv_alec"));
00243:                 }
00244:
00245:                 // E. Disease simulation tick & care
00246:                 quarantineCoord.TickDaily(day);
00247:                 diseaseSystem.TickDaily(day, allSurvivors);
00248:
00249:                 // Treat on Day 11 (within antibiotics max_days: 2 window)
00250:                 if (day == 11 && diseaseSystem.IsInfected("surv_alec", DiseaseIds.Cholera) && !patientCured)
00251:                 {
00252:                     var treatRes = diseaseSystem.TryTreat("surv_alec", DiseaseIds.Cholera, "antibiotics", day);
00253:                     Assert.True(treatRes.Accepted, $"Treatment refused: {treatRes.Reason}");
00254:                     Assert.True(treatRes.Cured);
00255:                     patientCured = true;
00256:
00257:                     // Release from isolation on Day 12
00258:                     var relRes = quarantineCoord.ExecuteReleaseIsolation("surv_alec", day + 1);
00259:                     Assert.True(relRes.Success);
00260:                     Assert.False(quarantineCoord.IsIsolated("surv_alec"));
00261:
00262:                     // Verify acquired temporary immunity
00263:                     Assert.True(diseaseSystem.HasImmunity("surv_alec", DiseaseIds.Cholera, day + 5));
00264:                 }
00265:
00266:                 // F. Mid-campaign full save / reload on Day 20
00267:                 if (day == 20)
00268:                 {
00269:                     var savedDiseaseState = diseaseSystem.CaptureState();
00270:                     var savedWardState = medicalWard.CaptureState();
00271:                     var savedRosterState = dutyRoster.CaptureState();
00272:
00273:                     // Restore into fresh instances
00274:                     var newWard = new MedicalWardSystem(new MedicalWardState(), beds, procedures);
00275:                     newWard.RestoreState(savedWardState);
00276:
00277:                     var newDisease = new DiseaseSystem();
00278:                     newDisease.BindCatalog(diseaseCatalog);
00279:                     newDisease.TryConsumeItem = tryConsume;
00280:                     newDisease.RestoreState(savedDiseaseState);
00281:
00282:                     // Verify state continuity
00283:                     Assert.True(newDisease.HasImmunity("surv_alec", DiseaseIds.Cholera, 22));
00284:                     Assert.Equal(diseaseSystem.TotalInfectionsHistory, newDisease.TotalInfectionsHistory);
00285:                 }
00286:             }
00287:
00288:             // =================================================================
00289:             // 6. CAMPAIGN CONCLUSION AUDIT
00290:             // =================================================================
00291:             Assert.True(studyCompleted, "Library manual study must complete during 30-day campaign");
00292:             Assert.True(combatResolved, "Tactical combat encounter must resolve during 30-day campaign");
00293:             Assert.True(patientCured, "Quarantined patient must be treated and cured during 30-day campaign");
00294:             Assert.True(inventory["clean_water"] < 200, "Clean water must be consumed by quarantine care");
00295:             Assert.True(inventory["antibiotics"] < 50, "Antibiotics must be consumed by curative treatment");
00296:         }
00297:     }
00298: }
```

## `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` — 2,073 lines; 130,366 bytes; SHA-256 `7588dbb7ed053936964371ce06c49160f772cb9fffd2e7d519884ab430f044c4`
Declaration index:
- 00020: public sealed class ContentUtilizationScanner
- 00154: public static bool IsNarrativeSubdirectoryFile(string relativePath)
- 00159: private static bool IsPlan142JournalFile(string relativePath)
- 00169: private static bool IsPlan145GraffitiFile(string relativePath)
- 00176: private static bool IsPlan146CourtFile(string relativePath)
- 00182: private static bool IsPlan148MaintenanceFile(string relativePath)
- 00188: private static bool IsPlan149BureaucraticFile(string relativePath)
- 00195: private static bool IsPlan153FringeCultsFile(string relativePath)
- 00204: private static bool IsPlan156PaperPrintingFile(string relativePath)
- 00209: private static bool IsPlan160BoneHornFile(string relativePath)
- 00214: private static bool IsPlan150LetterFile(string relativePath)
- 00221: private static bool IsPlan151AbyssalFile(string relativePath)
- 00230: public static bool IsAuthoritativeCatalog(string fileName)
- 00246: public ContentUtilizationGraph Scan()
- 00269: private void EnsureNode(string id, ContentNodeKind kind, string label)
- 00279: private void AddEdge(string from, string to, ContentEdgeKind kind, EvidenceTier evidence, string context = "")
- 00288: private void InventoryContentFiles()
- 00328: private void InventoryLoaders()
- 00667: private void InventoryRegistries()
- 00943: private void InventoryRuntimeSystems()
- 01357: private void InventoryQueries()
- 01403: private void InventoryUiSurfaces()
- 01662: private void InventoryCodexSurfaces()
- 01707: private void InventoryTests()
- 01777: private void CountDefinitions()
- 01833: private void BuildRelationships()
- 01863: private string DetermineFamily(string path)
- 01911: private void ClassifyContent()
- 01972: private void VerifyConsumersInSource()
- 02035: private void DetectDisconnects()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL Core: Content Utilization Scanner
00003: //
00004: // Phase 1–3 implementation: static inventory of all content files,
00005: // loaders, registries, queries, systems, and their relationships.
00006:
00007: using System;
00008: using System.Collections.Generic;
00009: using System.IO;
00010: using System.Linq;
00011: using System.Text.RegularExpressions;
00012: using Ashfall.Core.Narrative;
00013:
00014: namespace Ashfall.Core.Content
00015: {
00016:     /// <summary>
00017:     /// Static scanner that builds a ContentUtilizationGraph from repository
00018:     /// source analysis. Phase 1–3: discovery, graph schema, static inventory.
00019:     /// </summary>
00020:     public sealed class ContentUtilizationScanner
00021:     {
00022:         private readonly string _repoRoot;
00023:         private readonly string _dataDir;
00024:         private readonly string _coreDir;
00025:         private readonly string _srcDir;
00026:         private readonly ILog? _log;
00027:
00028:         private readonly ContentUtilizationGraph _graph = new ContentUtilizationGraph();
00029:         private readonly Dictionary<string, ContentNode> _nodesById = new Dictionary<string, ContentNode>(StringComparer.Ordinal);
00030:         private readonly HashSet<string> _availableFiles = new HashSet<string>(StringComparer.Ordinal);
00031:
00032:         // Known JSON authoritative catalogs (not narrative flavor text)
00033:         private static readonly HashSet<string> AuthoritativeCatalogs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
00034:         {
00035:             "items.json", "recipes.json", "locations.json", "survivors.json",
00036:             "faction_lore.json", "economy_goods.json", "events.json",
00037:             "weather_seasons.json", "radio.json", "narrative_encounters.json",
00038:             "questline_master.json", "world_history.json", "wasteland_map_v1.json",
00039:             "dive_sites.json", "foundry_accords.json", "foundry_production.json",
00040:             "foundry_treaty_consequences.json", "warlord_doctrines.json",
00041:             "combat_catalog.json", "verdict_data.json", "verdict_items.json",
00042:             "verdict_locations.json", "verdict_radio.json",
00043:             "black_flotilla_items.json", "deep_lore_locations.json",
00044:             "dose_items.json", "dose_locations.json", "dose_quests.json",
00045:             "dose_registers.json", "holdfast_factions.json", "holdfast_flavor.json",
00046:             "holdfast_items.json", "holdfast_locations.json", "holdfast_quests.json",
00047:             "crossing_encounters.json", "crossing_factions.json",
00048:             "crossing_items.json", "crossing_locations.json", "crossing_quests.json",
00049:             "expeditions.json", "disease_catalog.json", "vehicles.json",
00050:             "starting_supplies.json", "starting_survivors.json",
00051:             "greenhouse_items.json", "library_manuals.json",
00052:             "crop_strains.json", "nutrition_profiles.json", "agriculture_items.json", "defenses.json", "mental_arcs.json",
00053:             "research_knowledge.json", "tech_salvage.json", "espionage_missions.json", "fluid_infrastructure.json", "quest_templates.json", "skills.json",
00054:             "spiritual_rituals.json", "memorial_rites.json", "belief_movements.json",
00055:             "prewar_archives.json", "captive_interrogations.json", "food_preservation.json", "campaign_epilogues.json",
00056:             "standing_record_factions.json", "standing_record_layouts.json",
00057:             "standing_record_memory.json", "standing_record_quests.json",
00058:             "year_of_ash_events.json", "year_of_ash_items.json",
00059:             "year_of_ash_locations.json", "year_of_ash_questlines.json",
00060:             "year_of_ash_quests.json", "year_of_ash_radio.json",
00061:             "year_of_ash_survivors.json", "trade_screen_scenarios.json",
00062:             "trade_specialties.json", "trade_tell_lines.json", "trade_texts.json",
00063:             "feedback_messages.json", "final_wishes.json", "guilt_sources.json",
00064:             "incidents.json", "moral_choice_chains.json", "moral_choice_flags.json",
00065:             "moral_choice_quests.json", "moral_choice_quests_branching.json",
00066:             "moral_choice_quests_expansion.json",
00067:             "moral_choice_faction_reactions.json", "moral_choice_gossip.json",
00068:             "hardcore_economy_tuning.json", "relic_recipes.json",
00069:             "world_evolution_seeds.json", "shelter_schedules.json",
00070:             "utility_actions.json", "confession_secrets.json",
00071:             "dynamic_questlines.json", "door_encounters.json",
00072:             "faction_war_communiques.json", "faction_war_dialogue.json",
00073:             "faction_war_events.json", "faction_war_journal.json",
00074:             "faction_war_location_overrides.json", "faction_war_radio.json",
00075:             "independent_faction_branch.json", "military_faction_branch.json",
00076:             "rebel_faction_branch.json", "faction_radio_corpus.json",
00077:             "radio_distress_signals.json", "radio_distress_signals_expansion.json",
00078:             "phantom_triggers.json", "phantom_heirlooms.json", "archive_inks.json",
00079:             "autopsy_procedures.json", "chemical_dependency_items.json",
00080:             "antigravity_survivor_fields.json", "expansion_survivor_fields.json",
00081:             "deep_lore_survivor_fields.json", "expansion_item_tags.json",
00082:             "audio_logs_expansion_05.json", "environmental_texts_expansion_05.json",
00083:             "journal_entries_expansion_05.json", "memorials_expansion_05.json",
00084:             "quests_expansion_05.json", "quests_expansion_06.json",
00085:             "narrative_arc_events.json", "narrative_encounters_expansion.json",
00086:             "narrative_progression.json", "narrative_questlines.json",
00087:             "pharma_recipes.json", "power_grid.json",
00088:             "characters.json", "item_description_texts.json",
00089:             "journal_voice_prose.json", "medical_texts.json",
00090:             "muster_epilogues.json", "muster_witnesses.json",
00091:             "wall_carving_templates.json", "wasteland_grave_epitaphs.json",
00092:             "damaged_map_zones.json", "currents.json",
00093:             "cassette_sets.json", "epilogue_chronicle.json",
00094:             "duty_roster_locations.json", "duty_roster_marks.json",
00095:             "duty_roster_quests.json", "duty_roster_seasons.json",
00096:             "locations_expansion3.json", "environmental_atmosphere_expansion.json",
00097:             "thirdonary_quests.json", "foundry_faction.json",
00098:             "foundry_items.json", "verdict_npcs.json", "verdict_questlines.json",
00099:             "workshop_recipes.json", "radio_intercepts.json",
00100:             "shelter_social_events.json", "excavation_hazard_mitigation.json",
00101:             "chemical_weapons.json", "comms_targets.json",
00102:             "ceremonies.json", "robotics.json",
00103:             "collectibles.json",
00104:             "item_degradation.json", "thermal_gear.json",
00105:             "naval_vessels.json", "recreation.json",
00106:             "fallout_patterns.json", "desperation_events.json",
00107:             "bounty_board.json", "lore_archives.json",
00108:             "surgical_procedures.json", "rail_network.json",
00109:             "underground_flora.json", "wasteland_laws.json",
00110:             "bio_fermentation_catalog.json",
00111:             "development_traits.json", "interrogation_tactics.json",
00112:             "mutations.json", "camouflage_gear.json",
00113:             "aircraft_parts.json", "labor_camps.json",
00114:             "narcotics.json", "political_policies.json",
00115:             // Flagship institutions (Tasks 5-8)
00116:             "cultural_archive_tomes.json", "diplomatic_treaties.json",
00117:             "sky_defense_ordnance.json", "psychological_therapies.json",
00118:             "sump_drainage_catalog.json", "electrostatic_filtration_catalog.json",
00119:             "atmospheric_sounding_catalog.json",
00120:             "caravan_trade_routes.json", "power_subgrid_nodes.json", "perimeter_defenses.json",
00121:             "ebpvd_coating_catalog.json", "mine_flail_catalog.json", "microfluidic_diagnostic_catalog.json", "rail_grinding_catalog.json",
00122:             // Flagship XI (Plans 154-157) + Plan 173 program templates
00123:             "contagion_events.json", "pathogens.json",
00124:             "subterranean_zones.json", "propaganda_campaigns.json",
00125:             "radio_programs.json",
00126:             // Plans 50-53
00127:             "vehicle_modifications.json", "vehicle_armor_grades.json", "faction_intelligence.json",
00128:             "psychological_trauma.json", "shelter_audio_cues.json",
00129:             // Plans 54-57 (Flagship: Trade, Apprenticeship, Seismic, Thermal)
00130:             "shelter_insulation_catalog.json", "seismic_fault_catalog.json",
00131:             "merchant_caravans.json", "apprenticeship_catalog.json",
00132:             // Plan 73 (Flagship: Rail Logistics)
00133:             "rail_logistics_catalog.json",
00134:             // Plans 74-77 (Flagship: geothermal, ballistics, aeroponics, pneumatic dispatch)
00135:             "geothermal_strata_catalog.json", "ballistics_workbench_catalog.json",
00136:             "aeroponics_nutrient_catalog.json", "pneumatic_network_catalog.json",
00137:             // Plan B100 — scientific glassworks projected into Silent Foundry.
00138:             "glassworks_recipes.json",
00139:             // Plans B86–B89 (Flagship: breaching, aquaponics, HF/DF, metrology)
00140:             "breaching_equipment_catalog.json",
00141:             "metrology_standards_catalog.json",
00142:             "direction_finding_catalog.json",
00143:             "aquaponics_system_catalog.json",
00144:             // Plans 198–201: late-game strategic catalogs.
00145:             "chemical_weapons.json", "comms_targets.json",
00146:             "ceremonies.json", "robotics.json",
00147:             "wildlife_trapping_catalog.json",
00148:             // Plans 118-121 — advanced industrial reconnaissance tranche.
00149:             "fischer_tropsch_catalog.json", "uv_corona_detector_catalog.json",
00150:             "carbon_composite_catalog.json", "gpr_exploration_catalog.json",
00151:         };
00152:
00153:         // Narrative JSON files in the narrative/ subdirectory — these are codex/lore, not gameplay catalogs
00154:         public static bool IsNarrativeSubdirectoryFile(string relativePath)
00155:         {
00156:             return relativePath.Replace('\\', '/').StartsWith("narrative/", StringComparison.OrdinalIgnoreCase);
00157:         }
00158:
00159:         private static bool IsPlan142JournalFile(string relativePath)
00160:         {
00161:             string normalized = relativePath.Replace('\\', '/');
00162:             return normalized.Equals("narrative/journals_expansion.json", StringComparison.OrdinalIgnoreCase)
00163:                 || normalized.Equals("narrative/journal_entries_batch_1.json", StringComparison.OrdinalIgnoreCase)
00164:                 || normalized.Equals("narrative/journal_entries_batch_2.json", StringComparison.OrdinalIgnoreCase)
00165:                 || normalized.Equals("narrative/journal_entries_batch_3.json", StringComparison.OrdinalIgnoreCase)
00166:                 || normalized.Equals("journal_entries_expansion_05.json", StringComparison.OrdinalIgnoreCase);
00167:         }
00168:
00169:         private static bool IsPlan145GraffitiFile(string relativePath)
00170:         {
00171:             string normalized = relativePath.Replace('\\', '/');
00172:             return normalized.Equals("narrative/bunker_graffiti_postings.json", StringComparison.OrdinalIgnoreCase)
00173:                 || normalized.Equals("narrative/graffiti_expansion.json", StringComparison.OrdinalIgnoreCase);
00174:         }
00175:
00176:         private static bool IsPlan146CourtFile(string relativePath)
00177:         {
00178:             string normalized = relativePath.Replace('\\', '/');
00179:             return normalized.Equals("narrative/bunker_court_verdicts_codex.json", StringComparison.OrdinalIgnoreCase);
00180:         }
00181:
00182:         private static bool IsPlan148MaintenanceFile(string relativePath)
00183:         {
00184:             string normalized = relativePath.Replace('\\', '/');
00185:             return normalized.Equals("narrative/bunker_maintenance_glitches.json", StringComparison.OrdinalIgnoreCase);
00186:         }
00187:
00188:         private static bool IsPlan149BureaucraticFile(string relativePath)
00189:         {
00190:             string normalized = relativePath.Replace('\\', '/');
00191:             return normalized.Equals("narrative/bureaucratic_documents_expansion.json", StringComparison.OrdinalIgnoreCase)
00192:                 || normalized.Equals("narrative/bureaucratic_document_runtime_map.json", StringComparison.OrdinalIgnoreCase);
00193:         }
00194:
00195:         private static bool IsPlan153FringeCultsFile(string relativePath)
00196:         {
00197:             string normalized = relativePath.Replace('\\', '/');
00198:             return normalized.Equals("narrative/cobalt_liturgies.json", StringComparison.OrdinalIgnoreCase)
00199:                 || normalized.Equals("narrative/iron_synod_canons.json", StringComparison.OrdinalIgnoreCase)
00200:                 || normalized.Equals("narrative/geophone_hymnals.json", StringComparison.OrdinalIgnoreCase)
00201:                 || normalized.Equals("narrative/wasteland_grave_epitaphs.json", StringComparison.OrdinalIgnoreCase);
00202:         }
00203:
00204:         private static bool IsPlan156PaperPrintingFile(string relativePath)
00205:         {
00206:             return PaperPrintRuntimeContract.IsSourceCatalog(relativePath);
00207:         }
00208:
00209:         private static bool IsPlan160BoneHornFile(string relativePath)
00210:         {
00211:             return BoneHornRuntimeContract.IsSourceCatalog(relativePath);
00212:         }
00213:
00214:         private static bool IsPlan150LetterFile(string relativePath)
00215:         {
00216:             string normalized = relativePath.Replace('\\', '/');
00217:             return normalized.Equals("narrative/letters_expansion.json", StringComparison.OrdinalIgnoreCase)
00218:                 || normalized.Equals("narrative/unsent_letters_batch_2.json", StringComparison.OrdinalIgnoreCase);
00219:         }
00220:
00221:         private static bool IsPlan151AbyssalFile(string relativePath)
00222:         {
00223:             string normalized = relativePath.Replace('\\', '/');
00229:
00230:         public static bool IsAuthoritativeCatalog(string fileName)
00231:         {
00232:             return AuthoritativeCatalogs.Contains(fileName);
00234:
00235:         public ContentUtilizationScanner(string repoRoot, string dataDir, string coreDir, string srcDir, ILog? log = null)
00236:         {
00237:             _repoRoot = repoRoot;
00245:
00246:         public ContentUtilizationGraph Scan()
00247:         {
00248:             _graph.ContentRoots.Add(_dataDir);
00268:
00269:         private void EnsureNode(string id, ContentNodeKind kind, string label)
00270:         {
00271:             if (!_nodesById.ContainsKey(id))
00278:
00279:         private void AddEdge(string from, string to, ContentEdgeKind kind, EvidenceTier evidence, string context = "")
00280:         {
00281:             EnsureNode(from, ContentNodeKind.ContentFile, from);
00287:
00288:         private void InventoryContentFiles()
00289:         {
00290:             if (!Directory.Exists(_dataDir))
00327:
00328:         private void InventoryLoaders()
00329:         {
00330:             if (!Directory.Exists(_coreDir))
00666:
00667:         private void InventoryRegistries()
00668:         {
00669:             // Map catalogs to their registries
00942:
00943:         private void InventoryRuntimeSystems()
00944:         {
00945:             // Map catalogs to known runtime consumer systems
01356:
01357:         private void InventoryQueries()
01358:         {
01359:             // Mark catalogs that have runtime query APIs
01402:
01403:         private void InventoryUiSurfaces()
01404:         {
01405:             var uiConsumers = new Dictionary<string, string[]>
01661:
01662:         private void InventoryCodexSurfaces()
01663:         {
01664:             // Codex/Journal surfaces that consume content
01706:
01707:         private void InventoryTests()
01708:         {
01709:             var testCoveredCatalogs = new HashSet<string>
01774:
01775:         private const int MaxSampleIdsPerCatalog = 200;
01776:
01777:         private void CountDefinitions()
01778:         {
01779:             if (!Directory.Exists(_dataDir)) return;
01832:
01833:         private void BuildRelationships()
01834:         {
01835:             // Build family summaries
01862:
01863:         private string DetermineFamily(string path)
01864:         {
01865:             if (path.StartsWith("narrative/", StringComparison.OrdinalIgnoreCase)) return "Narrative";
01910:
01911:         private void ClassifyContent()
01912:         {
01913:             foreach (var cat in _graph.Catalogs)
01954:                     cat.Classification = ContentClassification.ORPHANED;
01955:                     cat.Findings.Add("No known loader for this authoritative catalog");
01956:                 }
01957:                 // Has loader, has consumers → check if it's actually utilized
01958:                 else if (cat.ConsumerSystems.Count > 0)
01959:                 {
01960:                     cat.Classification = ContentClassification.GAMEPLAY_CONSUMED;
01961:                 }
01962:             }
01963:         }
01964:
01965:         // ── Verify Consumers In Source ─────────────────────────────
01966:
01967:         /// <summary>
01968:         /// Cross-references consumer claims against actual source code.
01969:         /// Downgrades catalogs whose consumer claims are only from the
01970:         /// scanner itself (name inference) with no source evidence.
01971:         /// </summary>
01972:         private void VerifyConsumersInSource()
01973:         {
01974:             if (!Directory.Exists(_coreDir)) return;
01975:
01976:             // Build a cache of all source text for fast lookup
01977:             string allSourceText = string.Empty;
01978:             try
01979:             {
01980:                 var coreFiles = Directory.GetFiles(_coreDir, "*.cs", SearchOption.AllDirectories);
01981:                 var srcDir = Path.Combine(_repoRoot, "src");
01982:                 var srcFiles = Directory.Exists(srcDir)
01983:                     ? Directory.GetFiles(srcDir, "*.cs", SearchOption.AllDirectories)
01984:                     : Array.Empty<string>();
01985:
01986:                 var sb = new System.Text.StringBuilder();
01987:                 foreach (var f in coreFiles)
01988:                 {
01989:                     if (f.Contains("ContentUtilization")) continue; // Skip scanner itself
01990:                     try { sb.AppendLine(File.ReadAllText(f)); } catch { /* cleanup: skip unreadable core files */ }
01991:                 }
01992:                 foreach (var f in srcFiles)
01993:                 {
01994:                     if (f.Contains("ContentUtilization")) continue; // Skip scanner itself
01995:                     try { sb.AppendLine(File.ReadAllText(f)); } catch { /* cleanup: skip unreadable src files */ }
01996:                 }
01997:                 allSourceText = sb.ToString();
01998:             }
01999:             catch (Exception ex)
02000:             {
02001:                 _log?.Warn($"[VerifyConsumers] Failed to read source: {ex.Message}");
02002:                 return;
02003:             }
02004:
02005:             foreach (var cat in _graph.Catalogs)
02006:             {
02007:                 // Skip already non-gameplay catalogs
02008:                 if (cat.Classification != ContentClassification.GAMEPLAY_CONSUMED)
02009:                     continue;
02010:
02011:                 string fileName = Path.GetFileName(cat.Path);
02012:
02013:                 // Check if the JSON filename appears in actual source code
02014:                 bool foundInSource = allSourceText.Contains(fileName);
02015:
02016:                 // Also check the base name (without .json) for loader references
02017:                 string baseName = Path.GetFileNameWithoutExtension(cat.Path);
02018:                 bool baseNameInSource = allSourceText.Contains(baseName);
02019:
02020:                 if (!foundInSource && !baseNameInSource)
02021:                 {
02022:                     // This catalog has zero source code evidence. Keep that
02023:                     // state honest and actionable instead of converting it
02024:                     // into a generic exemption that the gate cannot retire.
02025:                     cat.Classification = ContentClassification.ORPHANED;
02026:                     cat.ConsumerSystems.Clear();
02027:                     cat.Findings.Add("VERIFIED: No source code references found. Consumer claims were scanner name-inference only.");
02028:                     cat.ExemptionId = string.Empty;
02029:                 }
02030:             }
02031:         }
02032:
02033:         // ── Detect Disconnects ──────────────────────────────────────
02034:
02035:         private void DetectDisconnects()
02036:         {
02037:             foreach (var cat in _graph.Catalogs)
02038:             {
02039:                 // Skip properly classified catalogs
02040:                 if (cat.Classification == ContentClassification.GAMEPLAY_CONSUMED
02041:                     || cat.Classification == ContentClassification.CODEX_ONLY
02042:                     || cat.Classification == ContentClassification.OPTIONAL
02043:                     || cat.Classification == ContentClassification.TEST_ONLY)
02044:                     continue;
02045:
02046:                 // Catalog with loader but no consumer
02047:                 if (!string.IsNullOrEmpty(cat.Loader) && cat.ConsumerSystems.Count == 0)
02048:                 {
02049:                     _graph.Disconnects.Add(new DisconnectFinding
02050:                     {
02051:                         Catalog = cat.Path,
02052:                         Category = "REGISTERED_NOT_QUERIED",
02053:                         LastStage = cat.MaxStage,
02054:                         MissingLink = "No production runtime consumer found",
02055:                         Details = $"Loader: {cat.Loader}"
02056:                     });
02057:                 }
02058:                 // Catalog with no loader
02059:                 else if (string.IsNullOrEmpty(cat.Loader))
02060:                 {
02061:                     _graph.Disconnects.Add(new DisconnectFinding
02062:                     {
02063:                         Catalog = cat.Path,
02064:                         Category = "NO_LOADER",
02065:                         LastStage = UtilizationStage.DISCOVERED,
02066:                         MissingLink = "No loader registered",
02067:                         Details = "File exists but has no known loader"
02068:                     });
02069:                 }
02070:             }
02071:         }
02072:     }
02073: }
```
# Appendix M — External verification handoff

The following checks are to be run by the owning integrator after writing: character count, SHA-256 revalidation, path-token resolution, duplicate-heading/unsupported-claim scan, and `git diff --check`. The final ledger entry must report actual results, not this template.
