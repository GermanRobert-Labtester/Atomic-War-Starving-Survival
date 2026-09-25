# Plan 72 — Utility AI Actions: Twenty Definitions, Deterministic Scoring, and Authority-Safe Execution Boundary

> **Rebuild status:** TERMINAL 20-ACTION CONTENT + EXECUTION-SEMANTICS AUDIT
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

The historical baseline was 4,880 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Keep the twenty authored utility actions and deterministic scorer while preventing the catalog from being mistaken for a full autonomous-survivor runtime. The current Core class is a stateless selector; current host code demonstrates/evaluates it but does not execute medical, food, repair, research, or duty commands. The residual is an explicit action-contract audit, not arbitrary action growth.

**Bounded outcome:** Audit `UtilityAction`, `UtilityActionScorer`, `UtilityAiSystem`, the 20-row catalog, `UtilityAiHostSession`, panel/UI, and current survivor/duty/medical consumers. Define a future command adapter only if a canonical owner and typed action intent can be identified; otherwise preserve the selector as a projection/evaluation surface.

**Non-goals:** no second action executor, no direct inventory/medical/duty mutation from JSON, no arbitrary action count, no new save section, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `utility_actions.json` contains 20 actions.
- VERIFIED CURRENT: `UtilityAiSystem` is stateless selection with caller-supplied seeded noise and ordinal candidate order.
- VERIFIED CURRENT: `UtilityActionScorer` applies trait vetoes, listless penalty, curves, and skill/fatigue gates.
- The current host exposes evaluation/panel surfaces; the plan does not assume autonomous execution.
- HISTORICAL RECORD: Plan 72/Wave 39 records the 6→20 expansion; this package does not claim a fresh test run.

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

- `Assets/StreamingAssets/Data/utility_actions.json` exists at 10,835 bytes; SHA-256 `295d743640deb19251c3ff51710c49b02449fab2b1ecfe36518c219d47b2270e`.
- `Assets/StreamingAssets/Data/skills.json` exists at 55,682 bytes; SHA-256 `2bd8a261fc1ce61b1134cce9396810b21131330e85b3acca71dd1dfc0b2848d0`.

# 3. Required Delta

Replace the old “survivors autonomously choose and execute” brief with a current 20-row selector audit. Distinguish deterministic evaluation/debug presentation from future typed execution through canonical owners.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

What exact context fields can current production supply for each survivor?
Does any current production caller execute an action, or only evaluate/display it?
Which canonical owner could accept each action intent without a new queue?
How are equal scores, seeded noise, trait vetoes, and panel refresh pinned?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| action DTOs, curves, gates, and tags | `UtilityAction / UtilityActionCatalogLoader` | `Assets/Ashfall.Core/UtilityAI/UtilityAction.cs` | Static action definitions and bounded response data. |
| deterministic scoring and selection | `UtilityActionScorer / UtilityAiSystem` | `Assets/Ashfall.Core/UtilityAI/UtilityActionScorer.cs; Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` | Stateless selection only; no gameplay side effects. |
| host evaluation/panel | `UtilityAiHostSession / UtilityAiPanel` | `src/Host/UtilityAiHostSession.cs; src/UtilityAI/UtilityAiPanel.cs` | Debug/evaluation presentation; no command execution. |
| survivor state and needs | `NeedsSystem / survivor roster owners` | `Assets/Ashfall.Core/Survivors/; src/Main.Survivors.cs` | Context values must be read from current survivor owner. |
| medical/food/research/duty mutations | `their canonical owners` | `src/Main.Medical.cs; Core cooking/research/duty paths` | A future action adapter must call these owners, never mutate them. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for utility-AI action catalog.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Utility AI Actions: Twenty Definitions, Deterministic Scoring, and Authority-Safe Execution Boundary                                               │
│ Integration route: DATA-ONLY + explicit action-intent boundary audit                             │
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

1. **Action rows are definitions.**
2. **Scorer/selector are deterministic and stateless.**
3. **Context comes from current owners.**
4. **Execution belongs to target owners.**
5. **No selector save section.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| action DTOs, curves, gates, and tags | `UtilityAction / UtilityActionCatalogLoader` | `Assets/Ashfall.Core/UtilityAI/UtilityAction.cs` | Static action definitions and bounded response data. |
| deterministic scoring and selection | `UtilityActionScorer / UtilityAiSystem` | `Assets/Ashfall.Core/UtilityAI/UtilityActionScorer.cs; Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` | Stateless selection only; no gameplay side effects. |
| host evaluation/panel | `UtilityAiHostSession / UtilityAiPanel` | `src/Host/UtilityAiHostSession.cs; src/UtilityAI/UtilityAiPanel.cs` | Debug/evaluation presentation; no command execution. |
| survivor state and needs | `NeedsSystem / survivor roster owners` | `Assets/Ashfall.Core/Survivors/; src/Main.Survivors.cs` | Context values must be read from current survivor owner. |
| medical/food/research/duty mutations | `their canonical owners` | `src/Main.Medical.cs; Core cooking/research/duty paths` | A future action adapter must call these owners, never mutate them. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load 20 action definitions through UtilityActionCatalogLoader
2. construct an AIActionContext from current survivor facts
3. score each candidate in catalog order through UtilityActionScorer
4. use UtilityAiSystem.SelectAction with the caller-supplied seeded stream
5. present the selected action as a proposal/debug fact
6. execute only through a separately claimed typed command adapter and canonical owner

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- positive scores alone compete; a zero/vetoed action never wins
- trait vetoes are explicit and bounded
- listless penalty is applied once and remains bounded
- fatigue gate and skill bonus are finite
- selection is ordinal-stable for equal scores
- the selector consumes no inventory, medical, duty, or research state
- an action proposal is not an executed command

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

Contract rules for utility-AI action catalog:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`utility_actions.json` remains the twenty-row definition authority. Current fields include base score/priority, weight, override flag, tags, response curve, fatigue gate, and skill bonus. The current DTO does not encode hunger, injury, room, recipe, research, or duty prerequisites; those claims require a typed context/command contract before authoring rows.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

UtilityAiSystem is explicitly stateless and the current host has no utility-AI save section. Do not persist a selector cache. A future executable action must persist only the canonical job/assignment/medical/inventory state owned by that command’s target system.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

Selection order is caller candidate order, and deterministic noise uses the supplied `ISeededRng`. Same context, candidate order, action catalog, and seed must produce the same action. A future campaign adapter must use an existing campaign stream or a documented fork, never wall-clock or `System.Random`.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

`OnActionSelected` is a selection fact, not a command completion event. A future adapter emits a typed intent and waits for the target owner’s success/refusal fact. No action row can emit medical, food, research, or inventory effects directly.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Host/UtilityAiHostSession.cs` — loads actions and evaluates the current demo context
- `src/UtilityAI/UtilityAiPanel.cs` — renders scores/selection for diagnostics
- `src/Main.Survivors.cs` — current survivor setup and panel binding
- `src/Main.UiTests.UtilityAi.cs` — headless UI/evaluation smoke surface
- `src/Main.DutyRoster.cs` — current duty owner; future action adapter must use it

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Action descriptions are behavioral labels, not promises that a survivor will autonomously perform a medical procedure or repair. Keep the catalog’s restrained companion-bias language and do not imply autonomy where the current system only ranks candidates.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/UtilityAiTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/UtilityAiProbeTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs`

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
| Phase 0 — catalog/Core census | read 20 rows, DTO, scorer, selector, host, and tests | current scoring semantics are explicit | no undocumented scope or shortcut |
| Phase 1 — consumer/executor audit | trace panel/demo/survivor/duty/medical callers | selector and executor roles are separated | no undocumented scope or shortcut |
| Phase 2 — determinism/UI audit | check tie order, seeded noise, vetoes, and panel truth | no selection refresh side effect | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a typed command adapter or coverage gap is proposed | one target owner and focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/utility_actions.json` | READ ONLY; MODIFY only for a proven DTO/consumer gap | retain as action authority |
| `Assets/Ashfall.Core/UtilityAI/UtilityAction.cs` | READ ONLY | action DTO/context |
| `Assets/Ashfall.Core/UtilityAI/UtilityActionScorer.cs` | READ ONLY | scoring/veto contract |
| `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` | READ ONLY | stateless selector |
| `src/Host/UtilityAiHostSession.cs` | READ ONLY | evaluation host |
| `src/UtilityAI/UtilityAiPanel.cs` | READ ONLY | diagnostic presentation |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| parallel autonomy owner | keep canonical duty/needs/medical/inventory owners |
| fake action execution | typed intent and target-owner command required |
| selection drift | pin order/seed tests |
| UI-only feature presented as gameplay | label evaluator/debug status |

# 22. Explicit Non-Goals

- no second action executor, no direct inventory/medical/duty mutation from JSON, no arbitrary action count, no new save section, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for utility-AI action catalog are named from current evidence.
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

- A current 20-action DTO/consumer/selection matrix.
- An explicit selector-versus-executor boundary.
- A bounded typed command adapter plan only after owner evidence.

## MUST NOT DO

- create a second action queue or save state
- mutate canonical owners from UtilityAiSystem
- call action JSON a command contract
- add actions for unsupported needs/recipes/research surfaces

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/UtilityAiTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/UtilityAiProbeTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read UtilityAction, UtilityActionScorer, UtilityAiSystem, UtilityAiHostSession, the 20-row catalog, and focused tests; record the actual context inputs and side effects.

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

### Authority lines 250–255
00250: **SB-03 — Newest industrial catalogs: corpus twins + consumption wiring (Lanes A and B/C4).** Evidence: DR-04 (`hydraulic_extraction_catalog`, `metrology_standards_catalog` live, absent from v1.0 inventory). Subject: (a) assay-log narrative twins per the Part 16.4 pattern; (b) bind catalogs into consumption/production ledgers via power-grid/foundry seams if not yet consumed — check `UNCLAIMED_CORPUS_CENSUS.md` first (DR-08). Confidence: HIGH CONFIDENCE that content exists; UNVERIFIED whether systems consume them.
00251:
00252: **SB-04 — Muster domain deep expansion (Lanes A and B/C7).** Evidence: DR-04 — five muster catalogs live (`muster_camp_scenes`, `muster_epilogues`, `muster_faction_actions`, `muster_faction_culture`, `muster_witnesses`). Subject: muster-camp encounter depth, witness-driven epilogue evidence, culture-conditioned actions. Integration route: data-first through muster loaders; epilogue touch must be declared. Confidence: HIGH CONFIDENCE.
00253:
00254: **SB-05 — Epilogue permutation coverage campaign (Lanes A and C/C13).** Evidence: 19A/19B/19C closed (DR-06); matrix is 32 permutations. Subject: audit which permutations are under-served in chronicle prose and evidence enrollment; author chronicle depth for the weakest permutations. Integration route: data-first into epilogue chronicle catalogs; Reckoning enrollment through endgame owners. Confidence: HIGH CONFIDENCE.
00255:

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

### Authority lines 710–715
00710: **A-22 · C9 · Final-wishes document corpus.** Subject: unsent-letter and testament prose for `final_wishes.json` entries lacking document twins. Evidence: catalog verified live; `unsent_letters_batch_2` demonstrates the genre. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00711:
00712: **A-23 · C9 · Intake interview continuation.** Subject: new-arrival intake interviews conditioned on the arrival channels that exist (rescue, crossing, holdfast). Evidence: `new_arrival_intake_interviews` exists; arrival channels are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00713:
00714: **A-24 · C10 · Bureaucratic-morality quest prose completion.** Subject: prose-field completion across `quests_bureaucratic_morality.json` records with skeleton `quest_hook`/outcome texts. Evidence: catalog verified live; Part 9 contracts define the fields. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00715:

### Authority lines 754–761
00754: **B-13 · C8 · Intercept-driven journal depth.** Subject: faction intercepts producing journal records conditioned on signal authenticity patterns already sealed. Evidence: intercept catalog verified live; sealed authenticity evaluator is the seam. Route: HOST-WIRING. Confidence: PROPOSAL.
00755:
00756: **B-14 · C9 · Belief-movement ↔ faction-stance bridge.** Subject: belief movement membership shifting faction standing through `FactionStanceEngine`. Evidence: `belief_movements.json` verified live; stance engine is the sole standing authority. Route: CORE-EXTENSION. Confidence: PROPOSAL.
00757:
00758: **B-15 · C9 · Memorial-rite epilogue evidence enrollment.** Subject: performed rites enrolling as Reckoning evidence (rites exist; evidence vocabulary must be checked for a rite class before authoring). Evidence: `memorial_rites.json`, `spiritual_rituals.json` verified live. Route: CORE-EXTENSION through endgame owners. Confidence: PROPOSAL — evidence vocabulary check first.
00759:
00760: **B-16 · C10 · Quest reopening after new discoveries.** Subject: failed/abandoned quests reopening when discovery conditions later satisfy (the failure-recovery grammar of v1.0 Part 6.7). Evidence: abandoned-quest reopen is canon grammar; implementation state unverified. Route: CORE-EXTENSION through quest owners. Confidence: PROPOSAL.
00761:

### Authority lines 812–817
00812: **D-01 · Cross · Mid-event round-trip sweep.** Subject: extend exactly-once round-trip guarantees (rescue-runtime pattern: persisted first-result, exactly-once guards, duplicate arrival guards) to other exactly-once effect classes (one-time caches, bounty claims, unique-item claims). Evidence: pattern sealed in the rescue runtime; `UniqueItemClaimRegistry` exists. Route: CODEC-BUMP-AND-MIGRATE where state must persist; test matrix per v1.0 Part 12.3. Confidence: PROPOSAL per class.
00813:
00814: **D-02 · C9 · Lineage horizon coverage.** Subject: verify 3-year simulation coverage extends to the Day-3650 generational horizon for lineage facts; extend simulation fixtures if not. Evidence: 19B records a 3-year deterministic simulation (DR-06); canon horizon is 3650 days. Route: test-fixture extension. Confidence: PROPOSAL.
00815:
00816: **D-03 · C13 · Epilogue evidence persistence window.** Subject: confirm Day-360+ enrolled evidence survives into the Day-3650 window evaluation; add fixture tests for each evidence class. Evidence: epilogue matrix canon. Route: tests + possible migration. Confidence: PROPOSAL.
00817:

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

### Authority lines 947–952
00947: **DM-8 — Radio and information (C8).** Owners: radio system, stations, programs, intercepts, distress signals (sealed runtime), rumors, sound ranging, direction finding, NVIS, heliograph. Live catalogs: `radio`, `radio_stations`, `radio_programs`, `radio_intercepts`, `radio_distress_signals` (+ expansion), `comms_targets`, `sound_ranging_catalog`, `direction_finding_catalog`, `nvis_communications_catalog`, `heliograph`. Hosts: Radio, RadioProgramProduction, SoundRanging, Heliograph. Sealed: distress content (`CF-P1-DISTRESS-CONTENT-SEAL`); availability consumer retired. Openings: A-19, A-20, B-12, B-13, B-25 (coordinated), E-04, F-05, G-06. Constraint: genuine-never-hostile invariant; no new signal scenarios without signature.
00948:
00949: **DM-9 — Survivors and interiority (C9).** Owners: needs, health, skills, traits, mental arcs, trauma, therapies, guilt, crises, morale contagion, relations, caregiving, dependency, companion animals, beliefs, spiritual rituals, memorial rites, final wishes, belongings, memory decay, phantom memory, lineage, cohorts, apprenticeships. Live catalogs: `survivors`, `skills`, `development_traits`, `mental_arcs`, `psychological_trauma`, `psychological_therapies`, `guilt_sources`, `confession_secrets`, `belief_movements`, `spiritual_rituals`, `memorial_rites`, `final_wishes`, `companion_animals`, `phantom_heirlooms`, `phantom_triggers`, `starting_survivors`, `starting_survivor_cohorts`, `expansion_survivor_fields`. Hosts: Survivors, SurvivorRelations, PsychologyArc, MentalHealthCrisis, Caregiving, Spiritual, PhantomMemory. Openings: A-21, A-22, A-23, B-04, B-14, B-15, D-02. Known caution: `ClaimPersonalBelonging` no-caller finding (unverified at runtime — re-verify before extending).
00950:
00951: **DM-10 — Quests and moral choice (C10).** Owners: questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip/quests (five split catalogs live), branching faction quests, bureaucratic morality, massive expansion corpus, repeatable quests, templates, domain questlines (dose, year-of-ash, holdfast, crossing, thirdonary, verdict, expansion). Live catalogs: `questline_master`, `dynamic_questlines`, `personal_quests`, `npc_arcs`, `quests_npc_arcs`, `moral_choice_chains/flags/gossip/quests/quests_branching/quests_distress/quests_expansion`, `quests_faction_branching`, `quests_bureaucratic_morality`, `quests_massive_expansion_200`, `quests_moral_branching_expansion`, `repeatable_quests`, `quest_templates`. Hosts: NarrativeQuestline, PersonalQuest, MoralChoice, DynamicQuestline, ExpansionQuest, NpcArc. Openings: A-24, A-25, B-16, B-17, D-07, G-01, plus the F-001 flagship.
00952:

### Authority lines 5414–5419
05414: First — the weight seam is named and it is not the catalog's `weight` field alone: `encounterWeightMultiplier` is a persisted, per-save multiplier that scales encounter frequency. Any Lane C balance work touching incident frequency must go through this multiplier, not through re-authoring catalog weights; the A-42 tranche's frozen-`weight` clause (Volume 45.5) is thereby doubly justified — the tuning authority lives in the consumer's state, and the catalog's authored weights are the baseline it scales.
05415:
05416: Second — `seedSalt` defaults to `SeedOffset`, and the source's own comment names the convention: "Utility AI salt. Spec: _worldSeed + 1208." This is the second repository-verified instance of the deterministic-salt convention (the factory's harness specifications already assume per-system seed salts); the encounter system's salt is 1208 off the world seed. Any deterministic incident harness must use the same salt derivation or its draws will not match the game's.
05417:
05418: ## 54.3 The encounter kind inventory and the named-visitor surface
05419:

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/utility_actions.json`
- Bytes: 10,835; SHA-256: `295d743640deb19251c3ff51710c49b02449fab2b1ecfe36518c219d47b2270e`
- Root keys: `actions, schema_version`
- `actions`: list[20]; union fields: `basePriority, baseScore, curvePoints, description, displayName, fatigueGate, id, isOverrideAction, skillBonusFactor, tags, weight`
  - row 1: `{"basePriority":0.1,"baseScore":0.4,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Verifies cargo mass against depot calibration. Osran Kell companion bias.","displayName":"Weigh Goods","fatigueGate":85.0,"id":"action_weigh_goods","isOverrideAction":false,"skillBonusFactor":0.25,"tags":["loud_labor"],"weight":1.0}`
  - row 2: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Studies a contract line by line before signing. The Tally's companion bias.","displayName":"Read Contract","fatigueGate":90.0,"id":"action_read_contract","isOverrideAction":false,"skillBonusFactor":0.2,"tags":[],"weight":1.0}`
  - row 3: `{"basePriority":0.1,"baseScore":0.45,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Walks the hall collecting signatures for the petition. Amnesty-campaign companion bias.","displayName":"Canvas Support","fatigueGate":80.0,"id":"action_canvas_support","isOverrideAction":false,"skillBonusFactor":0.15,"tags":["menial_labor"],"weight":1.0}`
  - row 4: `{"basePriority":0.1,"baseScore":0.3,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Vouches for a stranger at the gate. Standing-record companion bias.","displayName":"Run Vouch","fatigueGate":88.0,"id":"action_run_vouch","isOverrideAction":false,"skillBonusFactor":0.1,"tags":[],"weight":1.0}`
  - row 5: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Counts and verifies shelter stockpiles against the ledger. Inventory-accuracy companion bias.","displayName":"Audit Inventory","fatigueGate":80,"id":"action_audit_inventory","isOverrideAction":false,"skillBonusFactor":0.0,"tags":["quiet_labor"],"weight":1.0}`
  - row 6: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Drafts and files a shelter operations report for the standing record.","displayName":"File Report","fatigueGate":80,"id":"action_file_report","isOverrideAction":false,"skillBonusFactor":0.0,"tags":["quiet_labor"],"weight":1.0}`
  - row 7: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Inspects and repairs degraded shelter equipment. Workshop-companion bias.","displayName":"Repair Equipment","fatigueGate":80,"id":"action_repair_equipment","isOverrideAction":false,"skillBonusFactor":0.25,"tags":["loud_labor"],"weight":1.0}`
  - row 8: `{"basePriority":0.1,"baseScore":0.25,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Walks the shelter checking seals, filters, and structural integrity. Preventive-maintenance companion bias.","displayName":"Inspect Housing","fatigueGate":85,"id":"action_inspect_housing","isOverrideAction":false,"skillBonusFactor":0.1,"tags":["quiet_labor"],"weight":1.0}`
  - row 9: `{"basePriority":0.1,"baseScore":0.55,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Attends to an injured survivor using available medical supplies. Medical-triage companion bias.","displayName":"Treat Wounded","fatigueGate":90,"id":"action_treat_wounded","isOverrideAction":false,"skillBonusFactor":0.3,"tags":["medical_triage"],"weight":1.0}`
  - row 10: `{"basePriority":0.1,"baseScore":0.45,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"A sick or injured survivor seeks medical attention from available care.","displayName":"Seek Treatment","fatigueGate":95,"id":"action_seek_treatment","isOverrideAction":false,"skillBonusFactor":0.0,"tags":["medical"],"weight":1.0}`
  - row 11: `{"basePriority":0.1,"baseScore":0.4,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Prepares a meal from available ingredients in the shelter kitchen.","displayName":"Cook Food","fatigueGate":80,"id":"action_cook_food","isOverrideAction":false,"skillBonusFactor":0.2,"tags":["quiet_labor"],"weight":1.0}`
  - row 12: `{"basePriority":0.1,"baseScore":0.3,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Preserves perishable food to extend shelf life. Cold-storage and drying work.","displayName":"Preserve Food","fatigueGate":80,"id":"action_preserve_food","isOverrideAction":false,"skillBonusFactor":0.15,"tags":["menial_labor"],"weight":1.0}`
  - row 13: `{"basePriority":0.1,"baseScore":0.45,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Processes unsafe water through filtration and treatment equipment.","displayName":"Purify Water","fatigueGate":80,"id":"action_purify_water","isOverrideAction":false,"skillBonusFactor":0.15,"tags":["loud_labor"],"weight":1.0}`
  - row 14: `{"basePriority":0.1,"baseScore":0.2,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Spends time with another survivor — talking, sharing a meal, or sitting together.","displayName":"Socialize","fatigueGate":85,"id":"action_socialize","isOverrideAction":false,"skillBonusFactor":0.0,"tags":[],"weight":1.0}`
  - row 15: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Attempts to mediate an active interpersonal friction between survivors.","displayName":"Resolve Conflict","fatigueGate":85,"id":"action_resolve_conflict","isOverrideAction":false,"skillBonusFactor":0.1,"tags":["order"],"weight":1.0}`
  - row 16: `{"basePriority":0.1,"baseScore":0.15,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Practices a trainable skill during idle time. Slow, steady improvement.","displayName":"Train Skill","fatigueGate":70,"id":"action_train_skill","isOverrideAction":false,"skillBonusFactor":0.2,"tags":["quiet_labor"],"weight":1.0}`
  - row 17: `{"basePriority":0.1,"baseScore":0.15,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"A qualified survivor teaches another survivor a skill they have mastered.","displayName":"Teach Skill","fatigueGate":70,"id":"action_teach_skill","isOverrideAction":false,"skillBonusFactor":0.25,"tags":["quiet_labor"],"weight":1.0}`
  - row 18: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Takes a watch post to monitor for threats and maintain shelter security.","displayName":"Stand Watch","fatigueGate":85,"id":"action_stand_watch","isOverrideAction":false,"skillBonusFactor":0.15,"tags":["weapon"],"weight":1.0}`
  - row 19: `{"basePriority":0.1,"baseScore":0.2,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Advances an active research project in the shelter laboratory.","displayName":"Conduct Research","fatigueGate":75,"id":"action_conduct_research","isOverrideAction":false,"skillBonusFactor":0.3,"tags":["quiet_labor"],"weight":1.0}`
  - row 20: `{"basePriority":0.1,"baseScore":0.5,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Takes time to recover from fatigue. Essential for long-term survivor health.","displayName":"Rest","fatigueGate":0,"id":"action_rest","isOverrideAction":false,"skillBonusFactor":0.0,"tags":[],"weight":1.0}`
- Bytes: 10,835; SHA-256: `295d743640deb19251c3ff51710c49b02449fab2b1ecfe36518c219d47b2270e`
- Root keys: `actions, schema_version`
- `actions`: list[20]; union fields: `basePriority, baseScore, curvePoints, description, displayName, fatigueGate, id, isOverrideAction, skillBonusFactor, tags, weight`
  - row 1: `{"basePriority":0.1,"baseScore":0.4,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Verifies cargo mass against depot calibration. Osran Kell companion bias.","displayName":"Weigh Goods","fatigueGate":85.0,"id":"action_weigh_goods","isOverrideAction":false,"skillBonusFactor":0.25,"tags":["loud_labor"],"weight":1.0}`
  - row 2: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Studies a contract line by line before signing. The Tally's companion bias.","displayName":"Read Contract","fatigueGate":90.0,"id":"action_read_contract","isOverrideAction":false,"skillBonusFactor":0.2,"tags":[],"weight":1.0}`
  - row 3: `{"basePriority":0.1,"baseScore":0.45,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Walks the hall collecting signatures for the petition. Amnesty-campaign companion bias.","displayName":"Canvas Support","fatigueGate":80.0,"id":"action_canvas_support","isOverrideAction":false,"skillBonusFactor":0.15,"tags":["menial_labor"],"weight":1.0}`
  - row 4: `{"basePriority":0.1,"baseScore":0.3,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Vouches for a stranger at the gate. Standing-record companion bias.","displayName":"Run Vouch","fatigueGate":88.0,"id":"action_run_vouch","isOverrideAction":false,"skillBonusFactor":0.1,"tags":[],"weight":1.0}`
  - row 5: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Counts and verifies shelter stockpiles against the ledger. Inventory-accuracy companion bias.","displayName":"Audit Inventory","fatigueGate":80,"id":"action_audit_inventory","isOverrideAction":false,"skillBonusFactor":0.0,"tags":["quiet_labor"],"weight":1.0}`
  - row 6: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Drafts and files a shelter operations report for the standing record.","displayName":"File Report","fatigueGate":80,"id":"action_file_report","isOverrideAction":false,"skillBonusFactor":0.0,"tags":["quiet_labor"],"weight":1.0}`
  - row 7: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Inspects and repairs degraded shelter equipment. Workshop-companion bias.","displayName":"Repair Equipment","fatigueGate":80,"id":"action_repair_equipment","isOverrideAction":false,"skillBonusFactor":0.25,"tags":["loud_labor"],"weight":1.0}`
  - row 8: `{"basePriority":0.1,"baseScore":0.25,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Walks the shelter checking seals, filters, and structural integrity. Preventive-maintenance companion bias.","displayName":"Inspect Housing","fatigueGate":85,"id":"action_inspect_housing","isOverrideAction":false,"skillBonusFactor":0.1,"tags":["quiet_labor"],"weight":1.0}`
  - row 9: `{"basePriority":0.1,"baseScore":0.55,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Attends to an injured survivor using available medical supplies. Medical-triage companion bias.","displayName":"Treat Wounded","fatigueGate":90,"id":"action_treat_wounded","isOverrideAction":false,"skillBonusFactor":0.3,"tags":["medical_triage"],"weight":1.0}`
  - row 10: `{"basePriority":0.1,"baseScore":0.45,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"A sick or injured survivor seeks medical attention from available care.","displayName":"Seek Treatment","fatigueGate":95,"id":"action_seek_treatment","isOverrideAction":false,"skillBonusFactor":0.0,"tags":["medical"],"weight":1.0}`
  - row 11: `{"basePriority":0.1,"baseScore":0.4,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Prepares a meal from available ingredients in the shelter kitchen.","displayName":"Cook Food","fatigueGate":80,"id":"action_cook_food","isOverrideAction":false,"skillBonusFactor":0.2,"tags":["quiet_labor"],"weight":1.0}`
  - row 12: `{"basePriority":0.1,"baseScore":0.3,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Preserves perishable food to extend shelf life. Cold-storage and drying work.","displayName":"Preserve Food","fatigueGate":80,"id":"action_preserve_food","isOverrideAction":false,"skillBonusFactor":0.15,"tags":["menial_labor"],"weight":1.0}`
  - row 13: `{"basePriority":0.1,"baseScore":0.45,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Processes unsafe water through filtration and treatment equipment.","displayName":"Purify Water","fatigueGate":80,"id":"action_purify_water","isOverrideAction":false,"skillBonusFactor":0.15,"tags":["loud_labor"],"weight":1.0}`
  - row 14: `{"basePriority":0.1,"baseScore":0.2,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Spends time with another survivor — talking, sharing a meal, or sitting together.","displayName":"Socialize","fatigueGate":85,"id":"action_socialize","isOverrideAction":false,"skillBonusFactor":0.0,"tags":[],"weight":1.0}`
  - row 15: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Attempts to mediate an active interpersonal friction between survivors.","displayName":"Resolve Conflict","fatigueGate":85,"id":"action_resolve_conflict","isOverrideAction":false,"skillBonusFactor":0.1,"tags":["order"],"weight":1.0}`
  - row 16: `{"basePriority":0.1,"baseScore":0.15,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Practices a trainable skill during idle time. Slow, steady improvement.","displayName":"Train Skill","fatigueGate":70,"id":"action_train_skill","isOverrideAction":false,"skillBonusFactor":0.2,"tags":["quiet_labor"],"weight":1.0}`
  - row 17: `{"basePriority":0.1,"baseScore":0.15,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"A qualified survivor teaches another survivor a skill they have mastered.","displayName":"Teach Skill","fatigueGate":70,"id":"action_teach_skill","isOverrideAction":false,"skillBonusFactor":0.25,"tags":["quiet_labor"],"weight":1.0}`
  - row 18: `{"basePriority":0.1,"baseScore":0.35,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Takes a watch post to monitor for threats and maintain shelter security.","displayName":"Stand Watch","fatigueGate":85,"id":"action_stand_watch","isOverrideAction":false,"skillBonusFactor":0.15,"tags":["weapon"],"weight":1.0}`
  - row 19: `{"basePriority":0.1,"baseScore":0.2,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Advances an active research project in the shelter laboratory.","displayName":"Conduct Research","fatigueGate":75,"id":"action_conduct_research","isOverrideAction":false,"skillBonusFactor":0.3,"tags":["quiet_labor"],"weight":1.0}`
  - row 20: `{"basePriority":0.1,"baseScore":0.5,"curvePoints":[{"x":0.0,"y":0.0},{"x":1.0,"y":1.0}],"description":"Takes time to recover from fatigue. Essential for long-term survivor health.","displayName":"Rest","fatigueGate":0,"id":"action_rest","isOverrideAction":false,"skillBonusFactor":0.0,"tags":[],"weight":1.0}`

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

# Appendix D — Current caller/reference graph

### `UtilityActionCatalogLoader` (9 sampled current references)
- Assets/Ashfall.Core/UtilityAI/UtilityAiHeadlessDemo.cs:37: var defs = UtilityActionCatalogLoader.Load(
- Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs:84: public static class UtilityActionCatalogLoader
- src/Main.UiTests.UtilityAi.cs:45: var loadedActions = Ashfall.Core.UtilityAI.UtilityActionCatalogLoader.Load(_dataDir, fileIO, serializer);
- src/Host/UtilityAiHostSession.cs:38: session.Actions.AddRange(UtilityActionCatalogLoader.Load(dataDir, fileIO, serializer));
- Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs:32: var actions = UtilityActionCatalogLoader.Load(
- Ashfall.Core.Tests/UtilityAiProbeTests.cs:232: var defs = UtilityActionCatalogLoader.Load(
- Ashfall.Core.Tests/UtilityAiTests.cs:263: var defs = UtilityActionCatalogLoader.Load(
- Ashfall.Core.Tests/UtilityAiTests.cs:289: var defs = UtilityActionCatalogLoader.Load(
- Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs:41: var actions = UtilityActionCatalogLoader.Load(
### `UtilityAiSystem` (18 sampled current references)
- Assets/Ashfall.Core/UtilityAI/UtilityAiHeadlessDemo.cs:41: var sys = new UtilityAiSystem();
- Assets/Ashfall.Core/UtilityAI/UtilityAiHeadlessDemo.cs:74: string pickA = new UtilityAiSystem().SelectAction(ctx, defs, new SeededRng(5))!.id;
- Assets/Ashfall.Core/UtilityAI/UtilityAiHeadlessDemo.cs:75: string pickB = new UtilityAiSystem().SelectAction(ctx, defs, new SeededRng(5))!.id;
- Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs:17: public class UtilityAiSystem
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:412: ["utility_actions.json"] = new[] { "UtilityAiSystem" },
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:721: ["utility_actions.json"] = "UtilityAiSystem",
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1018: ["utility_actions.json"] = new[] { "UtilityAiSystem" },
- src/Host/UtilityAiHostSession.cs:17: public UtilityAiSystem Engine { get; }
- src/Host/UtilityAiHostSession.cs:21: public UtilityAiHostSession(UtilityAiSystem engine = null!)
- src/Host/UtilityAiHostSession.cs:23: Engine = engine ?? new UtilityAiSystem();
- src/UtilityAI/UtilityAiPanel.cs:91: var scored = new UtilityAiSystem().ScoreAll(ctx, _session.Actions, scorer);
- Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs:308: var sys = new UtilityAiSystem();
- Ashfall.Core.Tests/UtilityAiProbeTests.cs:52: var sys = new UtilityAiSystem();
- Ashfall.Core.Tests/UtilityAiProbeTests.cs:120: var sys = new UtilityAiSystem();
- Ashfall.Core.Tests/UtilityAiProbeTests.cs:134: var sys = new UtilityAiSystem();
- Ashfall.Core.Tests/UtilityAiProbeTests.cs:141: var sys = new UtilityAiSystem();
- Ashfall.Core.Tests/UtilityAiProbeTests.cs:181: var sys = new UtilityAiSystem();
- Ashfall.Core.Tests/UtilityAiProbeTests.cs:217: var sys = new UtilityAiSystem();
### `UtilityActionScorer` (18 sampled current references)
- Assets/Ashfall.Core/UtilityAI/UtilityActionScorer.cs:13: public class UtilityActionScorer
- Assets/Ashfall.Core/UtilityAI/UtilityAiHeadlessDemo.cs:42: var scorer = new UtilityActionScorer();
- Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs:27: UtilityActionScorer? scorer = null)
- Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs:30: scorer = scorer ?? new UtilityActionScorer();
- Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs:68: UtilityActionScorer? scorer = null)
- Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs:72: scorer = scorer ?? new UtilityActionScorer();
- src/UtilityAI/UtilityAiPanel.cs:90: var scorer = new UtilityActionScorer();
- Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs:171: var scorer = new UtilityActionScorer();
- Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs:189: var scorer = new UtilityActionScorer();
- Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs:206: var scorer = new UtilityActionScorer();
- Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs:220: var scorer = new UtilityActionScorer();
- Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs:234: var scorer = new UtilityActionScorer();
- Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs:248: var scorer = new UtilityActionScorer();
- Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs:265: var scorer = new UtilityActionScorer();
- Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs:284: var scorer = new UtilityActionScorer();
- Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs:295: var scorer = new UtilityActionScorer();
- Ashfall.Core.Tests/UtilityAiProbeTests.cs:87: var scorer = new UtilityActionScorer();
- Ashfall.Core.Tests/UtilityAiProbeTests.cs:110: var scorer = new UtilityActionScorer();
### `UtilityAiHostSession` (8 sampled current references)
- src/Main.Survivors.cs:39: private UtilityAiHostSession _utilityAi = null!;
- src/Main.Survivors.cs:243: _utilityAi = UtilityAiHostSession.Create(_dataDir);
- src/Host/UtilityAiHostSession.cs:15: public sealed class UtilityAiHostSession
- src/Host/UtilityAiHostSession.cs:21: public UtilityAiHostSession(UtilityAiSystem engine = null!)
- src/Host/UtilityAiHostSession.cs:31: public static UtilityAiHostSession Create(string dataDir)
- src/Host/UtilityAiHostSession.cs:33: var session = new UtilityAiHostSession();
- src/UtilityAI/UtilityAiPanel.cs:17: private UtilityAiHostSession _session;
- src/UtilityAI/UtilityAiPanel.cs:55: public void BindSession(UtilityAiHostSession session)
### `UtilityAiPanel` (14 sampled current references)
- src/Main.Survivors.cs:245: if (_utilityAiPanel == null && _rightColumn != null)
- src/Main.Survivors.cs:247: _utilityAiPanel = new UtilityAiPanel();
- src/Main.Survivors.cs:248: _rightColumn.AddChild(_utilityAiPanel);
- src/Main.Survivors.cs:250: if (_utilityAiPanel != null)
- src/Main.Survivors.cs:252: _utilityAiPanel.BindSession(_utilityAi);
- src/Main.Survivors.cs:253: _utilityAiPanel.RefreshView();
- src/Main.Survivors.cs:261: _utilityAiPanel.RefreshView();
- src/Main.UiTests.UtilityAi.cs:42: bool panel = _utilityAiPanel != null;
- src/Main.UiTests.UtilityAi.cs:56: int before = _utilityAiPanel!.GetChild(0).GetChildCount();
- src/Main.UiTests.UtilityAi.cs:57: _utilityAiPanel.RefreshView();
- src/Main.UiTests.UtilityAi.cs:58: _utilityAiPanel.RefreshView();
- src/Main.UiTests.UtilityAi.cs:59: int after = _utilityAiPanel.GetChild(0).GetChildCount();
- src/Main.UiPanels.cs:84: private UtilityAiPanel _utilityAiPanel = null!;
- src/UtilityAI/UtilityAiPanel.cs:15: public partial class UtilityAiPanel : PanelContainer

# Appendix E — Current focused-test inventory

Current test declaration inventory: 100 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs` — 28 test declarations; bytes=12,478; SHA-256=`122419b360689a310870f95d007f64cc08ab786ce323d41a25f599fa1a67a919`
- 00055: [Fact]
- 00056: public void Catalog_LoadsExact20ActionsWithValidSchema()
- 00075: [Fact]
- 00076: public void Catalog_PreservesOriginal6ActionsByteAndFieldParity()
- 00124: [Fact]
- 00125: public void Catalog_All14NewActionsPresentAndCategorized()
- 00152: [Fact]
- 00153: public void Catalog_ContainsZeroDuplicateIds()
- 00164: [Fact]
- 00165: public void Scorer_CowardRefusesLoudLaborActions()
- 00182: [Fact]
- 00183: public void Scorer_GodComplexRefusesMenialLaborActions()
- 00200: [Fact]
- 00201: public void Scorer_PacifistRefusesSecurityWatchWithWeaponTag()
- 00214: [Fact]
- 00215: public void Scorer_HitmanRefusesMedicalTriageActions()
- 00228: [Fact]
- 00229: public void Scorer_GermaphobeGatedOnHazmatForMedicalTriage()
- 00242: [Fact]
- 00243: public void Scorer_ExConRefusesOrderActions()
- 00256: [Fact]
- 00257: public void Scorer_FatigueGatingDisablesWorkWhileAllowingRest()
- 00278: [Fact]
- 00279: public void Scorer_CraftingSkillScalesRepairEquipment()
- 00291: [Fact]
- 00292: public void Scorer_DeadSurvivorScoresZeroAcrossAll20Actions()
- 00304: [Fact]
- 00305: public void Selection_PicksDeterministicallyWithSameSeed()
### `Ashfall.Core.Tests/UtilityAiTests.cs` — 38 test declarations; bytes=12,568; SHA-256=`4a38836ab68615450df3a4f0381ffc0130f89b2b3a64ab468eab9e1880d47176`
- 00044: [Fact]
- 00045: public void Scorer_DeadSurvivorScoresZero()
- 00051: [Fact]
- 00052: public void Scorer_FatigueGateZeroesRaw()
- 00061: [Fact]
- 00062: public void Scorer_SkillBonusApplies()
- 00074: [Fact]
- 00075: public void Scorer_CurveTransformsRaw()
- 00089: [Fact]
- 00090: public void Scorer_ListlessPenaltyFloorsAtZero()
- 00099: [Fact]
- 00100: public void Scorer_OverrideActionsPassThroughUnclamped()
- 00110: [Fact]
- 00111: public void Scorer_VetoMatrix_CowardRefusesLoudLabor()
- 00119: [Fact]
- 00120: public void Scorer_VetoMatrix_PacifistRefusesWeapons_BlindRefusesGuns()
- 00139: [Fact]
- 00140: public void Selection_PicksHighestScoringCandidate()
- 00149: [Fact]
- 00150: public void Selection_OverrideWinsOverAnyNormalAction()
- 00160: [Fact]
- 00161: public void Selection_EmptyOrNullCandidatesReturnsNull()
- 00169: [Fact]
- 00170: public void Selection_AllVetoedReturnsNull()
- 00178: [Fact]
- 00179: public void Selection_WithoutNoise_TiesFirstWins()
- 00192: [Fact]
- 00193: public void Selection_WithNoise_TiePickDeterministicPerSeed()
- 00210: [Fact]
- 00211: public void Selection_DeterministicSameSeedSamePick()
- 00223: [Fact]
- 00224: public void Selection_FiresEventOnPick()
- 00233: [Fact]
- 00234: public void Scorer_NullActionOrContextScoresZero()
- 00257: [Fact]
- 00258: public void Catalog_LoadsExpandedCatalogWithOriginalActionsPreserved()
- 00283: [Fact]
- 00284: public void Catalog_WeighGoodsUnityParity()
### `Ashfall.Core.Tests/UtilityAiProbeTests.cs` — 26 test declarations; bytes=10,751; SHA-256=`340321ce0938ee88cdeb1267ce948bd7734478dd207876db24524455916367f5`
- 00049: [Fact]
- 00050: public void Probe_SeedFuzz_100SeedsMixedContexts_NoExceptionsDeterministic()
- 00084: [Fact]
- 00085: public void Probe_CurveBounds_IdentitySinglePointNonMonotonic()
- 00107: [Fact]
- 00108: public void Probe_FatigueGateBoundary_AtGateAllowedAboveVetoed()
- 00117: [Fact]
- 00118: public void Probe_OverrideDominatesUnderNoise()
- 00131: [Fact]
- 00132: public void Probe_EmptyCatalog_SelectsNull()
- 00138: [Fact]
- 00139: public void Probe_AllZeroScoreActions_SelectsNull()
- 00149: [Fact]
- 00150: public void Probe_VetoMatrix_EveryTraitTagPair()
- 00178: [Fact]
- 00179: public void Probe_ScoreAll_OrdinalStableNoSideEffects()
- 00194: [Fact]
- 00195: public void Probe_UnsortedCurvePoints_DoNotMisEvaluate()
- 00211: [Fact]
- 00212: public void Probe_NoiseOnClampedScore_NeverNaN_DeterministicPerSeed()
- 00229: [Fact]
- 00230: public void Probe_MissingCatalogFile_ReturnsEmpty()
- 00237: [Fact]
- 00238: public void Probe_ScoreAll_NullScorerDefaults()
- 00246: [Fact]
- 00247: public void Probe_SelectionDoesNotMutateContextOrDefs()
### `Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs` — 8 test declarations; bytes=11,291; SHA-256=`160c23615b4d218512ce37de4e490899850aa421670f323d77316220b65973ba`
- 00047: [Fact]
- 00048: public void Catalogs_LoadCleanly_WithValidSchemaAndReferentialIntegrity()
- 00081: [Fact]
- 00082: public void ConfessionResolution_Forgiveness_RestoresSocialBonds_AndEnablesHighProductivityActions()
- 00142: [Fact]
- 00143: public void ConfessionResolution_ExposureOrGuilt_IncreasesFatigue_GatingLaborAndBiasingRest()
- 00203: [Fact]
- 00204: public void ConfessionAndUtilityAi_DeterministicReplayAndSaveRestoreRoundTrip()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Utility AI Actions: Twenty Definitions, Deterministic Scoring, and Authority-Safe Execution Boundary` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan describes need/state curves and command execution not present in current DTO/host.
- The current catalog descriptions are not proof of room/recipe/research wiring.

## H.2 Evidence-strength corrections
- Remove unsupported autonomy claims.
- Document actual context inputs and side effects.
- Keep action growth evidence-gated.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Utility AI Actions: Twenty Definitions, Deterministic Scoring, and Authority-Safe Execution Boundary while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use UtilityAction/CatalogLoader for static definitions.
- Use UtilityActionScorer and UtilityAiSystem for deterministic selection.
- Read survivor context from current owners.
- Use target-owner commands for any future execution.
- Do not add utility-AI save state for a stateless selector.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement utility ai actions: twenty definitions, deterministic scoring, and authority-safe execution boundary arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- No action can be called autonomous without a typed executor and current owner proof.
- The final plan must state that the selector is stateless.

## J.2 Questions deliberately left open
- Which first action is safe to expose as a typed intent?
- Should the panel be diagnostic-only or a proposal queue?
- What is the canonical survivor context read model?

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

The original file at `HEAD:piagentsplans/72-utility-ai-actions-expansion.md` contained 4,880 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 72 — Utility AI Actions Expansion (6 → 20 actions)

## Goal (2 lines)
Expand `utility_actions.json` from 6 verified entries to 20. The utility AI system
governs survivor autonomous behavior — each action has a priority, weight, tags, and
curve points that determine when a survivor chooses it. 6 actions is too few for
survivors to feel like they make their own decisions.

## Why (P2)
- Verified: `utility_actions.json` has 6 entries (id, displayName, description,
  basePriority, weight, isOverrideAction, tags, curvePoints). The utility AI system
  is in `src/UtilityAI/` (Godot host) and `Assets/Ashfall.Core/UtilityAI/` (Core).
- Creates the survivor-autonomy pillar: survivors should do things on their own —
  repair equipment, cook food, treat wounds, clean, socialize, train — not just wait
  for the player's orders. More actions make the shelter feel inhabited.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/utility_actions.json` (expand 6 → 20 actions)
- Read-only: `Assets/Ashfall.Core/UtilityAI/` (confirm action schema: id, displayName,
  description, basePriority, weight, isOverrideAction, tags, curvePoints; confirm how
  curve points drive priority based on need/state)

## Content grammar (per action)
- snake_case `id` with prefix `action_` (confirmed prefix from existing 6).
- basePriority: 0.0–1.0 — base likelihood of the action being chosen.
- weight: multiplier on the final priority score.
- isOverrideAction: true if this action overrides all others (e.g. fleeing from danger).
- tags: behavioral categories (loud_labor, quiet_labor, medical, social, maintenance,
  training, rest, hygiene, food, water, security, research, crafting).
- curvePoints: how priority changes based on a need/state (hunger → eat; fatigue → rest;
  injury → treat; low morale → socialize; broken equipment → repair).

## Steps
1. Read the utility AI system to confirm the action schema and how curve points drive
   priority.
2. Read the 6 existing actions to understand the structure and avoid duplication.
3. Author 14 new actions across 8 categories:
   - Maintenance (2): repair_equipment (fixes degraded items), clean_shelter (morale +
     hygiene bonus).
   - Medical (2): treat_wounded (applies first aid to injured survivors), self_medicate
     (takes medicine when sick — feeds Plan 112 disease content and Plan 09A response).
   - Food (2): cook_food (uses Plan 55 recipes), preserve_food (extends food shelf life).
   - Water (1): purify_water (uses Plan 55 water recipes).
   - Social (2): socialize (morale bonus for both survivors), resolve_conflict (reduces
     friction between two survivors — feeds existing 12B).
   - Training (2): train_skill (practices a skill — feeds Plan 33), teach_skill (teaches
     a skill to another survivor — feeds Plan 65 final wishes).
   - Security (1): stand_watch (perimeter guard — feeds Plan 57 security incidents).
   - Research (1): conduct_research (advances a research node — feeds Plan 34).
   - Rest (1): rest (reduces fatigue — the most basic survival action).
4. Give each action: basePriority, weight, tags, curve points, description.
5. Cross-reference: every action that uses a recipe references Plan 55 recipe ids; every
   skill action references Plan 33 skill ids; every research action references Plan 34
   knowledge ids.
6. Wire 5 actions to Plan 41 shelter rooms (cook_food requires kitchen; repair_equipment
   requires workshop; conduct_research requires laboratory; stand_watch requires
   armory/surveillance; purify_water requires water treatment room).
7. Wire 3 actions to Plan 33 skills (train_skill and teach_skill reference skill ids).
8. Validate: `--data-integrity-selftest`; confirm survivors autonomously choose actions
   based on curve points in a headless boot.
9. xUnit: action catalog loads, all references resolve, curve points drive priority,
   override actions fire on emergency, save round-trip preserves action state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data.

## Definition of Done
- `utility_actions.json` has 20 actions (6 existing + 14 new), all references resolving,
  5 wired to shelter rooms, 3 wired to skills, curve points drive priority, override
  actions fire on emergency, save round-trip green, integrity + tests green.

## Follow-on
- Plan 33 (skills) — train_skill and teach_skill reference the skill catalog.
- Plan 34 (research) — conduct_research advances research nodes.
- Plan 41 (shelter rooms) — actions require specific rooms.
- Plan 55 (recipes) — cook_food and purify_water use recipes.
- Existing 12B (duty roster) — utility AI actions complement duty-roster assignments.
- Plan 65 (final wishes) — teach_skill feeds the teach_lesson wish type.

```

## End of Plan 72 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/UtilityAI/UtilityAction.cs` — 160 lines; 6,124 bytes; SHA-256 `271e6e84f4d8d64505d7fb53a13ca62bfa6107fefc46478f1d0714a6fa0aba2c`
Declaration index:
- 00010: public class CurvePoint
- 00020: public class ResponseCurve
- 00045: public float Evaluate(float x)
- 00072: public class UtilityActionDef
- 00088: public float EvaluateRaw(AIActionContext context)
- 00099: public bool HasTag(string tag)
- 00125: public class AIActionContext
- 00135: public bool HasTrait(string trait) => Traits.Contains(trait);
- 00139: public static class UtilityTags
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.UtilityAI
00007: {
00008:     /// <summary>One point on a data-driven response curve (replaces AnimationCurve).</summary>
00009:     [Serializable]
00010:     public class CurvePoint
00011:     {
00012:         public float x = 0f;
00013:         public float y = 0f;
00014:     }
00015:
00016:     /// <summary>
00017:     /// Piecewise-linear response curve with input clamping (Unity AnimationCurve
00018:     /// parity: Evaluate clamps x to the first/last key; empty curve = identity).
00019:     /// </summary>
00020:     public class ResponseCurve
00021:     {
00022:         private readonly CurvePoint[]? _points;
00023:
00024:         public ResponseCurve(CurvePoint[] points)
00025:         {
00026:             if (points == null || points.Length == 0)
00027:             {
00028:                 _points = points;
00029:                 return;
00030:             }
00031:             // Normalize: interpolation assumes ascending x. A malformed catalog
00032:             // must not silently mis-evaluate (debug-loop defect); sort a copy.
00033:             _points = new CurvePoint[points.Length];
00034:             for (int i = 0; i < points.Length; i++)
00035:                 _points[i] = new CurvePoint { x = points[i].x, y = points[i].y };
00036:             System.Array.Sort(_points, (a, b) => a.x.CompareTo(b.x));
00037:         }
00038:
00039:         public static readonly ResponseCurve Identity = new ResponseCurve(new[]
00040:         {
00041:             new CurvePoint { x = 0f, y = 0f },
00042:             new CurvePoint { x = 1f, y = 1f }
00043:         });
00044:
00045:         public float Evaluate(float x)
00046:         {
00047:             if (_points == null || _points.Length == 0) return x; // identity passthrough (audit A4)
00048:             if (_points.Length == 1) return _points[0].y;
00049:             if (x <= _points[0].x) return _points[0].y;
00050:             if (x >= _points[_points.Length - 1].x) return _points[_points.Length - 1].y;
00051:             for (int i = 1; i < _points.Length; i++)
00052:             {
00053:                 if (x <= _points[i].x)
00054:                 {
00055:                     var a = _points[i - 1];
00056:                     var b = _points[i];
00057:                     float span = b.x - a.x;
00058:                     if (span <= 1e-6f) return b.y;
00059:                     float t = (x - a.x) / span;
00060:                     return a.y + (b.y - a.y) * t;
00061:                 }
00062:             }
00063:             return _points[_points.Length - 1].y;
00064:         }
00065:     }
00066:
00067:     /// <summary>
00068:     /// Data-driven Utility AI action definition (the JSON is the authority).
00069:     /// Mirrors the Unity SurvivorAction fields the crossing companions use.
00070:     /// </summary>
00071:     [Serializable]
00072:     public class UtilityActionDef
00073:     {
00074:         public string id = string.Empty;
00075:         public string displayName = string.Empty;
00076:         public string description = string.Empty;
00077:         public float basePriority = 0.1f;
00078:         public float weight = 1.0f;
00079:         public bool isOverrideAction = false;
00080:         public string[] tags = Array.Empty<string>();
00081:         public CurvePoint[] curvePoints = null!; // null/empty = identity
00082:         public float baseScore = 0f;            // EvaluateRaw baseline
00083:         public float fatigueGate = 0f;          // 0 = off; raw = 0 when fatigue exceeds
00084:         public float skillBonusFactor = 0f;     // + skill * factor (clamped)
00085:
00086:         private ResponseCurve _curve;
00087:
00088:         public float EvaluateRaw(AIActionContext context)
00089:         {
00090:             if (context == null || !context.IsAlive) return 0f;
00091:             if (fatigueGate > 0f && context.Fatigue > fatigueGate) return 0f;
00092:
00093:             float score = baseScore;
00094:             if (skillBonusFactor > 0f && context.CraftingSkill > 0f)
00095:                 score += context.CraftingSkill * skillBonusFactor;
00096:             return Math.Max(0f, Math.Min(1f, score));
00097:         }
00098:
00099:         public bool HasTag(string tag)
00100:         {
00101:             if (string.IsNullOrEmpty(tag) || tags == null) return false;
00102:             for (int i = 0; i < tags.Length; i++)
00103:                 if (tags[i] == tag) return true;
00104:             return false;
00105:         }
00106:
00107:         public ResponseCurve Curve
00108:         {
00109:             get
00110:             {
00111:                 if (_curve == null)
00112:                     _curve = curvePoints != null && curvePoints.Length > 0
00113:                         ? new ResponseCurve(curvePoints)
00114:                         : ResponseCurve.Identity;
00115:                 return _curve;
00116:             }
00117:         }
00118:     }
00119:
00120:     /// <summary>
00121:     /// Per-call decision context (survivor-agnostic). Needs/traits/flags are
00122:     /// plain values the host fills from its own survivor model (audit: the
00123:     /// Unity AIContext carried the whole Survivor object).
00124:     /// </summary>
00125:     public class AIActionContext
00126:     {
00127:         public string SurvivorId = string.Empty;
00128:         public bool IsAlive = true;
00129:         public float Fatigue = 0f;         // 0..100
00130:         public float CraftingSkill = 0f;   // 0..1
00131:         public bool IsListless = false;
00132:         public bool HasHazmat = false;
00133:         public HashSet<string> Traits = new HashSet<string>(StringComparer.Ordinal);
00134:
00135:         public bool HasTrait(string trait) => Traits.Contains(trait);
00136:     }
00137:
00138:     /// <summary>Known trait gates and action tags for the veto matrix (audit: Unity quest vetoes).</summary>
00139:     public static class UtilityTags
00140:     {
00141:         public const string TraitCoward = "coward";
00142:         public const string TraitGodComplex = "god_complex";
00143:         public const string TraitPacifist = "pacifist";
00144:         public const string TraitBlind = "blind";
00145:         public const string TraitExCon = "ex_con";
00146:         public const string TraitHitman = "hitman";
00147:         public const string TraitGermaphobe = "germaphobe";
00148:         public const string TraitPolitician = "politician";
00149:
00150:         public const string TagLoudLabor = "loud_labor";
00151:         public const string TagMenialLabor = "menial_labor";
00152:         public const string TagDirtyLabor = "dirty_labor";
00153:         public const string TagWeapon = "weapon";
00154:         public const string TagGun = "gun";
00155:         public const string TagOrder = "order";
00156:         public const string TagMedicalTriage = "medical_triage";
00157:         public const string TagFarming = "farming";
00158:         public const string TagMedical = "medical";
00159:     }
00160: }
```

## `Assets/Ashfall.Core/UtilityAI/UtilityActionScorer.cs` — 92 lines; 3,877 bytes; SHA-256 `daf6082aaa1d56d2c1df5b0f13c63e9055aa304ebb83a038356237a8426f6395`
Declaration index:
- 00013: public class UtilityActionScorer
- 00017: public float Score(UtilityActionDef action, AIActionContext context)
- 00043: public static bool IsForbiddenByTraits(UtilityActionDef action, AIActionContext context)
- 00078: public static float ApplyTraitBiases(float score, UtilityActionDef action, AIActionContext context)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core.UtilityAI
00006: {
00007:     /// <summary>
00008:     /// Engine-agnostic port of the Unity ActionScorer pipeline (audit A3-A7
00009:     /// preserved): raw -> curve -> (curved + basePriority) x weight -> trait
00010:     /// vetoes -> listless penalty -> override passthrough -> clamp01.
00011:     /// Vetoes are data-driven (trait x action-tag matrix).
00012:     /// </summary>
00013:     public class UtilityActionScorer
00014:     {
00015:         public const float ListlessScorePenalty = 0.08f;
00016:
00017:         public float Score(UtilityActionDef action, AIActionContext context)
00018:         {
00019:             if (action == null || context == null) return 0f;
00020:             if (IsForbiddenByTraits(action, context)) return 0f;
00021:
00022:             float rawScore = action.EvaluateRaw(context);
00023:             if (rawScore <= 0f) return 0f;
00024:
00025:             float curvedScore = action.Curve.Evaluate(rawScore);
00026:
00027:             float score = ApplyTraitBiases(
00028:                 (curvedScore + action.basePriority) * action.weight, action, context);
00029:
00030:             if (context.IsListless)
00031:                 score -= ListlessScorePenalty;
00032:
00033:             if (action.isOverrideAction)
00034:                 return Math.Max(0f, score);
00035:
00036:             return Math.Max(0f, Math.Min(1f, score));
00037:         }
00038:
00039:         /// <summary>
00040:         /// Hard vetoes (audit A7: null quests = no vetoes): the trait-tag matrix
00041:         /// from the Unity quest gates, expressed as data.
00042:         /// </summary>
00043:         public static bool IsForbiddenByTraits(UtilityActionDef action, AIActionContext context)
00044:         {
00045:             if (action == null || context == null) return false;
00046:
00047:             // Coward refuses loud labor.
00048:             if (context.HasTrait(UtilityTags.TraitCoward) && action.HasTag(UtilityTags.TagLoudLabor))
00049:                 return true;
00050:             // God Complex refuses menial labor.
00051:             if (context.HasTrait(UtilityTags.TraitGodComplex) && action.HasTag(UtilityTags.TagMenialLabor))
00052:                 return true;
00053:             // Pacifist cannot equip weapons / combat.
00054:             if (context.HasTrait(UtilityTags.TraitPacifist) && action.HasTag(UtilityTags.TagWeapon))
00055:                 return true;
00056:             // Blind cannot fire guns.
00057:             if (context.HasTrait(UtilityTags.TraitBlind) && action.HasTag(UtilityTags.TagGun))
00058:                 return true;
00059:             // Ex-Con refuses orders from authority (order-tagged actions).
00060:             if (context.HasTrait(UtilityTags.TraitExCon) && action.HasTag(UtilityTags.TagOrder))
00061:                 return true;
00062:             // Hitman refuses medical triage and farming.
00063:             if (context.HasTrait(UtilityTags.TraitHitman) &&
00064:                 (action.HasTag(UtilityTags.TagMedicalTriage) || action.HasTag(UtilityTags.TagFarming)))
00065:                 return true;
00066:             // Germaphobe: no bunker triage without hazmat.
00067:             if (context.HasTrait(UtilityTags.TraitGermaphobe) &&
00068:                 action.HasTag(UtilityTags.TagMedicalTriage) && !context.HasHazmat)
00069:                 return true;
00070:             return false;
00071:         }
00072:
00073:         /// <summary>
00074:         /// Soft biases (reweight, never veto).
00075:         /// Re-weights action priority based on trait-tag synergy/aversion without vetoing.
00076:         /// Politician scores dirty labor lower (0.6x) so they prefer delegating.
00077:         /// </summary>
00078:         public static float ApplyTraitBiases(float score, UtilityActionDef action, AIActionContext context)
00079:         {
00080:             if (action == null || context == null || score <= 0f) return score;
00081:
00082:             float multiplier = 1.0f;
00083:             if (context.HasTrait(UtilityTags.TraitPolitician) && action.HasTag(UtilityTags.TagDirtyLabor))
00084:             {
00085:                 multiplier *= 0.6f;
00086:             }
00087:
00088:             multiplier = Math.Clamp(multiplier, 0.1f, 2.0f);
00089:             return score * multiplier;
00090:         }
00091:     }
00092: }
```

## `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` — 123 lines; 4,839 bytes; SHA-256 `1f387b63e536c330308e2f88447b2f1b1f4ebb89cd3324e4e95888a6b670932b`
Declaration index:
- 00017: public class UtilityAiSystem
- 00023: public UtilityActionDef? SelectAction(
- 00084: public static class UtilityActionCatalogLoader
- 00088: public static List<UtilityActionDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: using Ashfall.Core.IO;
00007: namespace Ashfall.Core.UtilityAI
00008: {
00009:     /// <summary>
00010:     /// Engine-agnostic Utility AI selection core (audit A1/A2 fixed in the
00011:     /// port): picks the highest-scoring candidate with deterministic noise
00012:     /// from the caller-supplied ISeededRng (the Unity original used a hidden
00013:     /// System.Random). Ties are first-wins over the caller's candidate order —
00014:     /// the candidate list order IS the deterministic contract. Stateless:
00015:     /// no save state exists (audit A6); contexts are per-call host data.
00016:     /// </summary>
00017:     public class UtilityAiSystem
00018:     {
00019:         public const double NoiseScale = 0.0001d; // Unity parity: score noise amplitude
00020:
00021:         public event Action<string, string, float> OnActionSelected; // survivorId, actionId, score
00022:
00023:         public UtilityActionDef? SelectAction(
00024:             AIActionContext context,
00025:             IReadOnlyList<UtilityActionDef> candidates,
00026:             ISeededRng rng,
00027: UtilityActionScorer? scorer = null)
00028:         {
00029:             if (context == null || candidates == null || candidates.Count == 0) return null;
00030:             scorer = scorer ?? new UtilityActionScorer();
00031:
00032:             UtilityActionDef? best = null;
00033:             float bestScore = -1f;
00034:
00035:             for (int i = 0; i < candidates.Count; i++)
00036:             {
00037:                 var candidate = candidates[i];
00038:                 if (candidate == null) continue;
00039:
00040:                 float score = scorer.Score(candidate, context);
00041:
00042:                 // Deterministic noise: seeded per call, same scale as Unity.
00043:                 if (score > 0f && rng != null)
00044:                     score += (float)(rng.NextDouble() * NoiseScale);
00045:
00046:                 // Only positive scores compete: a hard-vetoed (0) action must
00047:                 // never win just because bestScore started at -1 (latent Unity
00048:                 // defect fixed in the port — audit A9).
00049:                 if (score > 0f && score > bestScore)
00050:                 {
00051:                     bestScore = score;
00052:                     best = candidate;
00053:                 }
00054:             }
00055:
00056:             if (best != null && !string.IsNullOrEmpty(context.SurvivorId))
00057:                 OnActionSelected?.Invoke(context.SurvivorId, best.id, Math.Max(0f, bestScore));
00058:
00059:             return best;
00060:         }
00061:
00062:         /// <summary>
00063:         /// Score all candidates (for UI/debugging), ordinal-stable.
00064:         /// </summary>
00065:         public List<KeyValuePair<UtilityActionDef, float>> ScoreAll(
00066:             AIActionContext context,
00067:             IReadOnlyList<UtilityActionDef> candidates,
00068: UtilityActionScorer? scorer = null)
00069:         {
00070:             var result = new List<KeyValuePair<UtilityActionDef, float>>();
00071:             if (context == null || candidates == null) return result;
00072:             scorer = scorer ?? new UtilityActionScorer();
00073:             for (int i = 0; i < candidates.Count; i++)
00074:             {
00075:                 var c = candidates[i];
00076:                 if (c == null) continue;
00077:                 result.Add(new KeyValuePair<UtilityActionDef, float>(c, scorer.Score(c, context)));
00078:             }
00079:             return result;
00080:         }
00081:     }
00082:
00083:     /// <summary>Engine-agnostic loader for utility_actions.json (data-driven action definitions).</summary>
00084:     public static class UtilityActionCatalogLoader
00085:     {
00086:         public const string FileName = "utility_actions.json";
00087:
00088:         public static List<UtilityActionDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
00089:         {
00090:             var result = new List<UtilityActionDef>();
00091:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00092:                 return result;
00093:
00094:             string path = fileIO.Combine(dataDir, FileName);
00095:             if (!fileIO.FileExists(path))
00096:                 return result;
00097:
00098:             string raw = fileIO.ReadAllText(path);
00099:             if (string.IsNullOrWhiteSpace(raw))
00100:                 return result;
00101:
00102:             try
00103:             {
00104:                 var parsed = CatalogLocator.LoadWrappedList<UtilityActionDef>(raw, SystemTextJsonSerializer.Options).ToArray();
00105:                 if (parsed == null) return result;
00106:                 for (int i = 0; i < parsed.Length; i++)
00107:                 {
00108:                     var def = parsed[i];
00109:                     if (def == null || string.IsNullOrEmpty(def.id)) continue;
00110:                     if (def.tags == null) def.tags = Array.Empty<string>();
00111:                     if (def.displayName == null) def.displayName = def.id;
00112:                     result.Add(def);
00113:                 }
00114:             }
00115:             catch (Exception ex_CATDIAG)
00116:             {
00117:                 CatalogDiagnostics.Warn(path, "UtilityActionDef list", ex_CATDIAG);
00118:                 return result;
00119:             }
00120:             return result;
00121:         }
00122:     }
00123: }
```

## `Assets/Ashfall.Core/UtilityAI/UtilityAiHeadlessDemo.cs` — 95 lines; 4,152 bytes; SHA-256 `7fc8e7b4b89c1f38422d7e043adacc6ac5673fabe891948fe09d9179743a0212`
Declaration index:
- 00012: public static class UtilityAiHeadlessDemo
- 00014: public static HeadlessReport Run(string dataDirectory, ILog? log = null)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003:
00004: namespace Ashfall.Core.UtilityAI
00005: {
00006:     /// <summary>
00007:     /// Headless verification of the Utility AI core: catalog-driven actions,
00008:     /// scorer pipeline, deterministic selection with seeded noise, veto
00009:     /// matrix, override dominance. Invoked by `dotnet test` and by Godot
00010:     /// `-- --utility-ai-selftest`.
00011:     /// </summary>
00012:     public static class UtilityAiHeadlessDemo
00013:     {
00014:         public static HeadlessReport Run(string dataDirectory, ILog? log = null)
00015:         {
00016:             CatalogLocator.UseInvariantCulture();
00017:             log = log ?? NullLog.Instance;
00018:             var report = new HeadlessReport();
00019:
00020:             void Check(bool condition, string name)
00021:             {
00022:                 report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
00023:                 if (condition)
00024:                 {
00025:                     report.PassedCount++;
00026:                     log.Info("[PASS] " + name);
00027:                 }
00028:                 else
00029:                 {
00030:                     report.FailedCount++;
00031:                     log.Error("[FAIL] " + name);
00032:                 }
00033:             }
00034:
00035:             log.Info("[UtilityAiHeadlessDemo] begin");
00036:
00037:             var defs = UtilityActionCatalogLoader.Load(
00038:                 dataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
00039:             Check(defs.Count >= 6, $"catalog loads >= 6 utility actions ({defs.Count})");
00040:
00041:             var sys = new UtilityAiSystem();
00042:             var scorer = new UtilityActionScorer();
00043:
00044:             var ctx = new AIActionContext
00045:             {
00046:                 SurvivorId = "sv_demo",
00047:                 IsAlive = true,
00048:                 Fatigue = 30f,
00049:                 CraftingSkill = 0.7f
00050:             };
00051:
00052:             var picked = sys.SelectAction(ctx, defs, new SeededRng(99), scorer);
00053:             Check(picked != null, "selection returns an action");
00054:             // treat_wounded 0.55+0.7*0.3=0.76+0.1=0.86 is highest at low fatigue with skill 0.7
00055:             Check(picked != null && picked.id == "action_treat_wounded",
00056:                 "treat wounded (0.86) wins at low fatigue with skill 0.7");
00057:
00058:             // Fatigue 87: weigh (gate 85), canvas (gate 80), repair/cook/purify (gate 80)
00059:             // are gated; treat_wounded (gate 90) still active and wins.
00060:             ctx.Fatigue = 87f;
00061:             var gated = sys.SelectAction(ctx, defs, new SeededRng(99), scorer);
00062:             Check(gated != null && gated.id == "action_treat_wounded",
00063:                 "fatigue gates veto low-gate actions; treat wounded (gate 90) wins at fatigue 87");
00064:
00065:             // Veto matrix: coward refuses loud labor (weigh-goods is tagged loud_labor).
00066:             ctx.Fatigue = 30f;
00067:             ctx.Traits.Add(UtilityTags.TraitCoward);
00068:             var vetoed = sys.SelectAction(ctx, defs, new SeededRng(99), scorer);
00069:             Check(vetoed != null && vetoed.id != "action_weigh_goods",
00070:                 "coward vetoes loud labor (weigh goods)");
00071:
00072:             // Determinism: same seed, same pick, across fresh instances.
00073:             ctx.Traits.Clear();
00074:             string pickA = new UtilityAiSystem().SelectAction(ctx, defs, new SeededRng(5))!.id;
00075:             string pickB = new UtilityAiSystem().SelectAction(ctx, defs, new SeededRng(5))!.id;
00076:             Check(pickA == pickB, "same seed, same pick (determinism)");
00077:
00078:             // All-vetoed returns null (audit A9 regression).
00079:             var strictCtx = new AIActionContext { SurvivorId = "sv_vetoed", Traits = { UtilityTags.TraitCoward } };
00080:             var loudOnly = new List<UtilityActionDef>
00081:             {
00082:                 defs.Find(d => d.id == "action_weigh_goods")!
00083:             };
00084:             Check(sys.SelectAction(strictCtx, loudOnly, new SeededRng(1)) == null,
00085:                 "all-vetoed selection returns null (A9)");
00086:
00087:             report.Passed = report.FailedCount == 0;
00088:             report.Summary =
00089:                 $"[UtilityAiHeadlessDemo] {(report.Passed ? "PASS" : "FAIL")} " +
00090:                 $"{report.PassedCount}/{report.PassedCount + report.FailedCount}";
00091:             log.Info(report.Summary);
00092:             return report;
00093:         }
00094:     }
00095: }
```

## `src/Host/UtilityAiHostSession.cs` — 73 lines; 2,887 bytes; SHA-256 `bf6bb5357079b757e58281c5a99b5cfc13fe823e8b303ee00d446bd0fb61c7e6`
Declaration index:
- 00015: public sealed class UtilityAiHostSession
- 00031: public static UtilityAiHostSession Create(string dataDir)
- 00045: public string EvaluateDemo(string survivorId, float fatigue, float skill,
- 00063: public string StatusLine()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Ashfall.Core;
00006: using Ashfall.Core.UtilityAI;
00007:
00008: namespace AtomicWar.GodotApp
00009: {
00010:     /// <summary>
00011:     /// Thin Godot-host session for the Utility AI core: loads the action
00012:     /// catalog, evaluates + selects actions for a demo survivor, and drives
00013:     /// the crossing companion actions. No rules here — hosts only wire.
00014:     /// </summary>
00015:     public sealed class UtilityAiHostSession
00016:     : HostSessionBase{
00017:         public UtilityAiSystem Engine { get; }
00018:         public List<UtilityActionDef> Actions { get; } = new List<UtilityActionDef>();
00019:
00020:         public string LastEvent { get; private set; } = string.Empty;
00021:         public UtilityAiHostSession(UtilityAiSystem engine = null!)
00022:         {
00023:             Engine = engine ?? new UtilityAiSystem();
00024:             Engine.OnActionSelected += (sv, actionId, score) =>
00025:             {
00026:                 LastEvent = $"{sv} selects {actionId} (score {score:0.000})";
00027:                 RaiseStateChanged();
00028:             };
00029:         }
00030:
00031:         public static UtilityAiHostSession Create(string dataDir)
00032:         {
00033:             var session = new UtilityAiHostSession();
00034:             if (!string.IsNullOrEmpty(dataDir))
00035:             {
00036:                 var fileIO = new FileSystemIO();
00037:                 var serializer = new SystemTextJsonSerializer();
00038:                 session.Actions.AddRange(UtilityActionCatalogLoader.Load(dataDir, fileIO, serializer));
00039:             }
00040:             return session;
00041:         }
00042:
00043:         // ── Demo actions ─────────────────────────────────────────────
00044:
00045:         public string EvaluateDemo(string survivorId, float fatigue, float skill,
00046:             params string[] traits)
00047:         {
00048:             var ctx = new AIActionContext
00049:             {
00050:                 SurvivorId = survivorId,
00051:                 IsAlive = true,
00052:                 Fatigue = fatigue,
00053:                 CraftingSkill = skill
00054:             };
00055:             foreach (var t in traits) ctx.Traits.Add(t);
00056:
00057:             var picked = Engine.SelectAction(ctx, Actions, new SeededRng(2026));
00058:             return picked != null
00059:                 ? $"{survivorId} selects {picked.displayName} ({picked.id})."
00060:                 : $"{survivorId} selects nothing (all actions gated or vetoed).";
00061:         }
00062:
00063:         public string StatusLine()
00064:         {
00065:             var sb = new System.Text.StringBuilder();
00066:             sb.Append($"Utility AI: {Actions.Count} actions in catalog.\n");
00067:             for (int i = 0; i < Actions.Count; i++)
00068:                 sb.Append($"  {Actions[i].id} — base {Actions[i].baseScore:0.00}, " +
00069:                           $"gate {Actions[i].fatigueGate:0}, tags [{string.Join(",", Actions[i].tags)}]\n");
00070:             return sb.ToString().TrimEnd();
00071:         }
00072:     }
00073: }
```

## `src/UtilityAI/UtilityAiPanel.cs` — 104 lines; 3,524 bytes; SHA-256 `463b8589233cc36d6bd12ad34d1af7f3f253416c858389b33043f5a9cf47f323`
Declaration index:
- 00015: public partial class UtilityAiPanel : PanelContainer
- 00055: public void BindSession(UtilityAiHostSession session)
- 00062: public void UnbindSession()
- 00075: public void RefreshView()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: #pragma warning disable CS8618
00004: using Godot;
00005: using AtomicWar.GodotApp.UI;
00006: using Ashfall.Core.UI;
00007: using Ashfall.Core.UtilityAI;
00008:
00009: namespace AtomicWar.GodotApp.UtilityAI
00010: {
00011:     /// <summary>
00012:     /// Thin Godot panel: renders the action catalog and the demo survivor's
00013:     /// current selection with scores. Presentation only; zero rules.
00014:     /// </summary>
00015:     public partial class UtilityAiPanel : PanelContainer
00016:     {
00017:         private UtilityAiHostSession _session;
00018:         private VBoxContainer _actionList;
00019:         private Label _lblSelection;
00020:
00021:         public override void _Ready()
00022:         {
00023:             SetAnchorsPreset(LayoutPreset.TopRight);
00024:             CustomMinimumSize = new Vector2(400, 260);
00025:
00026:             // Apply standard panel 9-slice via shared helper (frame_9slice first)
00027:             AddThemeStyleboxOverride("panel", AtomicWar.GodotApp.UI.AshfallUiHelpers.MakePanelFrameStyleBox());
00028:
00029:             var rootVbox = new VBoxContainer();
00030:             rootVbox.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
00031:             AddChild(rootVbox);
00032:
00033:             var title = new Label
00034:             {
00035:                 Text = "UTILITY AI — COMPANION DECISIONS",
00036:                 HorizontalAlignment = HorizontalAlignment.Center
00037:             };
00038:             title.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
00039:             rootVbox.AddChild(title);
00040:
00041:             _lblSelection = new Label { Text = "No selection yet." };
00042:             rootVbox.AddChild(_lblSelection);
00043:
00044:             var scroll = new ScrollContainer
00045:             {
00046:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
00047:                 CustomMinimumSize = new Vector2(0, 180)
00048:             };
00049:             rootVbox.AddChild(scroll);
00050:
00051:             _actionList = new VBoxContainer();
00052:             scroll.AddChild(_actionList);
00053:         }
00054:
00055:         public void BindSession(UtilityAiHostSession session)
00056:         {
00057:             _session = session;
00058:             if (_session != null)
00059:                 _session.StateChanged += RefreshView;
00060:         }
00061:
00062:         public void UnbindSession()
00063:         {
00064:             if (_session == null) return;
00065:             _session.StateChanged -= RefreshView;
00066:             _session = null!;
00067:         }
00068:
00069:         public override void _ExitTree()
00070:         {
00071:             UnbindSession();
00072:             base._ExitTree();
00073:         }
00074:
00075:         public void RefreshView()
00076:         {
00077:             if (_session == null) return;
00078:
00079:             AshfallUiHelpers.EmptyChildren(_actionList);_lblSelection.Text = string.IsNullOrEmpty(_session.LastEvent)
00080:                 ? "No selection yet."
00081:                 : _session.LastEvent;
00082:
00083:             var ctx = new AIActionContext
00084:             {
00085:                 SurvivorId = "demo",
00086:                 IsAlive = true,
00087:                 Fatigue = 30f,
00088:                 CraftingSkill = 0.5f
00089:             };
00090:             var scorer = new UtilityActionScorer();
00091:             var scored = new UtilityAiSystem().ScoreAll(ctx, _session.Actions, scorer);
00092:             for (int i = 0; i < scored.Count; i++)
00093:             {
00094:                 var row = new Label
00095:                 {
00096:                     Text = $"{scored[i].Key.displayName} — {scored[i].Value:0.00} " +
00097:                            (scored[i].Key.HasTag(UtilityTags.TagLoudLabor) ? " [loud]" : "")
00098:                 };
00099:                 row.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
00100:                 _actionList.AddChild(row);
00101:             }
00102:         }
00103:     }
00104: }
```

## `src/Main.Survivors.cs` — 320 lines; 13,614 bytes; SHA-256 `d108f0909d7a2740f0cdd71e2acc8a5b9fce87beb7f7438253e8b5330972df4f`
Declaration index:
- 00034: public partial class Main : Control
- 00045: private static string FormatSurvivorName(string id)
- 00051: private void SetupSurvivors()
- 00178: private static float AirlockSealFactor(AirlockDoorState? doorState)
- 00192: private float MaxSumpContamination()
- 00205: private StartingCohortCatalog EnsureStartingCohortCatalog()
- 00229: private StartingCohortProfile ResolveStartingCohort(string profileId)
- 00240: private void SetupUtilityAi()
- 00257: private void OnUtilityAiEvaluateClicked()
- 00264: private void OnSurvivorsOpenClicked()
- 00271: private void OnSurvivorsTickClicked()
- 00281: private void OnSurvivorsExposeClicked(string id, float rads)
- 00288: private void OnSurvivorsIodineClicked(string id)
- 00295: private void OnSurvivorsAntiRadClicked(string id, float rads)
- 00302: private void SaveSurvivors()
- 00309: private void CloseSurvivorsOverlay()
- 00314: private void CloseSurvivorDetailPanel()
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
00019: using Ashfall.Core.IO;
00020: using Ashfall.Core.Radiation;
00021: using Ashfall.Core.Shelter;
00022: using Ashfall.Core.Survivors;
00023: using AtomicWar.GodotApp.Economy;
00024: using AtomicWar.GodotApp.YearOfAsh;
00025: using AtomicWar.GodotApp.Muster;
00026: using AtomicWar.GodotApp.Dose;
00027: using AtomicWar.GodotApp.UtilityAI;
00028: using AtomicWar.GodotApp.Radio;
00029: using AtomicWar.GodotApp.Audio;
00030: using AtomicWar.GodotApp.UI;
00031:
00032: namespace AtomicWar.GodotApp
00033: {
00034:     public partial class Main : Control
00035:     {
00036:         // ── Survivor / UtilityAI fields (GAP-ARCH-01 Phase 1) ──
00037:         private SurvivorsHostSession _survivors = null!;
00038:         public SurvivorsHostSession? Survivors => _survivors;
00039:         private UtilityAiHostSession _utilityAi = null!;
00040:         private StartingCohortCatalog _startingCohortCatalog = null!;
00041:         private string _startingCohortProfileId = StartingCohortCatalog.StandardProfileId;
00042:         private bool _survivorInitializationApplied;
00043:         private bool _isRestoringSurvivorState;
00044:
00045:         private static string FormatSurvivorName(string id)
00046:         {
00047:             if (string.IsNullOrEmpty(id)) return "Unknown";
00048:             return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(id.Replace('_', ' '));
00049:         }
00050:
00051:         private void SetupSurvivors()
00052:         {
00053:             if (_survivors == null)
00054:             {
00055:                 _survivors = new SurvivorsHostSession();
00056:                 _survivors.LoadCatalog(_dataDir);
00057:                 SetupEnrichment();
00058:
00059:                 // Wire environmental exposure from location catalogs, weather, and active expeditions
00060:                 var locRads = ExposureEnvironmentResolver.LoadLocationRadRates(
00061:                     _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00062:                 _survivors.ExposureResolver.LocationRadRateProvider = locId =>
00063:                     locRads.TryGetValue(locId, out float r) ? r : ExposureEnvironmentResolver.DefaultWastelandOutdoorRadRate;
00064:                 _survivors.ExposureResolver.WeatherRadModifierProvider = () => _world?.Weather?.OutdoorRadModifier ?? 0f;
00065:                 // C2 / Plan 21A (P7) — weather-scaled protective wear: the
00066:                 // canonical WeatherSystem melt multiplier (black rain ×5).
00067:                 _survivors.BindHazmatWearMultiplier(() => _world?.Weather?.HazmatDegradeMultiplier ?? 1f);
00068:                 // Fallout plume contamination overlays expedition/outdoor exposure when clouds overlap a location.
00069:                 _survivors.ExposureResolver.FalloutContaminationProvider = locId =>
00070:                 {
00071:                     if (_fallout == null || string.IsNullOrEmpty(locId)) return 0f;
00072:                     return _fallout.GetLocationContamination(locId);
00073:                 };
00074:                 // Plan 176 — anomaly/storm-front radiation overlays expedition exposure
00075:                 // the same way (typed handoff; RadiationSystem still owns the dose).
00076:                 _survivors.ExposureResolver.AnomalyRadRateProvider = locId =>
00077:                 {
00078:                     if (_anomalyHazard == null || string.IsNullOrEmpty(locId)) return 0f;
00079:                     return GetAnomalyLocationRate(locId);
00080:                 };
00081:                 // C2 / Plan 20B — one shelter shielding/interior-radiation model.
00082:                 // Every provider reads a canonical owner lazily so setup order
00083:                 // cannot drop a seam; unbound systems degrade to nominal values
00084:                 // (healthy filter/duct/airlock, no internal sources) which keeps
00085:                 // the legacy interior math byte-identical.
00086:                 var shieldCatalog = ShelterShieldingCatalog.LoadFromDirectory(_dataDir, new FileSystemIO());
00087:                 if (shieldCatalog.IsValid)
00088:                 {
00089:                     _survivors.ExposureResolver.ShelterInteriorBaseRadRate =
00090:                         shieldCatalog.interior_baseline_rad_rate;
00091:                     var shielding = new ShelterShieldingModel(shieldCatalog)
00092:                     {
00093:                         StructuralAttenuationProvider = () => _survivors.Shelter.GetWeakestCeilingAttenuation(),
00094:                         FilterHealthPercentProvider = () => _startingLevel?.System.State.airFilterHealthPercent ?? 100f,
00095:                         VentilationDuctIntegrityProvider = () => _ventilation?.State.ductIntegrity ?? 100f,
00096:                         VentilationFilterSaturationProvider = () => _ventilation?.State.exhaustFilterSaturation ?? 0f,
00097:                         VentilationRecirculationProvider = () => _ventilation?.State.emergencyRecirculationMode ?? false,
00098:                         AirlockSealProvider = () => AirlockSealFactor(_airlockSecurity?.System.State.doorState),
00099:                         AirlockIncidentProvider = () => _airlockSecurity?.System.HasPendingIncident ?? false,
00100:                         WeatherRadModifierProvider = () => _world?.Weather?.OutdoorRadModifier ?? 0f,
00101:                         IndoorRadonProvider = () => _startingLevel?.System.State.radonLevelBqm3 ?? 0f,
00102:                         FloodingContaminationProvider = () => MaxSumpContamination(),
00103:                         ShelterContaminationProvider = () => _decontamination?.System.State.shelterContaminationLevel ?? 0f,
00104:                         DeconActiveProvider = () => _decontamination?.System.HasActiveCase ?? false
00105:                     };
00106:                     _survivors.BindShelterShieldingModel(shielding);
00107:                 }
00108:                 _survivors.ExposureResolver.SurvivorLocationQuery = id =>
00109:                 {
00110:                     if (_expeditions?.Engine != null &&
00111:                         _expeditions.Engine.Active.TryGetValue(id, out var exp))
00112:                     {
00113:                         return (SurvivorExposureLocation.Expedition, exp.locationId);
00114:                     }
00115:                     return (SurvivorExposureLocation.ShelterInterior, "");
00116:                 };
00117:
00118:                 _survivors.StateChanged += () =>
00119:                 {
00120:                     SaveSurvivors();
00121:                     _survivorsOverlay?.RefreshView();
00122:                     _medicalPanel?.RefreshView();
00123:                     _shelterPanel?.RefreshView();
00124:                     if (_state == GameState.Playing && !_isRestoringSurvivorState)
00125:                         UpdateHud();
00126:                 };
00127:
00128:                 _survivors.OnSurvivorExposed += (survivorId, delta) =>
00129:                 {
00130:                     ApplyRadiationExposure(survivorId, delta, _simDay);
00131:                 };
00132:             }
00133:
00134:             if (_inventory != null)
00135:             {
00136:                 _survivors.Inventory = _inventory;
00137:                 _inventory.Survivors = _survivors;
00138:             }
00139:             if (_holdfastRuntime != null)
00140:             {
00141:                 _holdfastRuntime.Survivors = _survivors;
00142:             }
00143:
00144:             if (_campaignInitializationMode == CampaignInitializationMode.FreshInitialize &&
00145:                 _survivors.RosterState.Count == 0)
00146:             {
00147:                 var cohort = ResolveStartingCohort(_startingCohortProfileId);
00148:                 _survivors.LoadStartingCohort(cohort);
00149:                 _survivorInitializationApplied = true;
00150:             }
00151:             else if (_campaignInitializationMode == CampaignInitializationMode.Restore &&
00152:                      !_survivorInitializationApplied)
00153:             {
00154:                 var save = SurvivorsSaveStore.TryLoad();
00155:                 if (save != null)
00156:                 {
00157:                     _isRestoringSurvivorState = true;
00158:                     try
00159:                     {
00160:                         _survivors.RestoreSave(save);
00161:                     }
00162:                     finally
00163:                     {
00164:                         _isRestoringSurvivorState = false;
00165:                     }
00166:                     GD.Print(
00167:                         $"[Ashfall Godot] Survivors restore applied: slices={save.survivors?.Count ?? 0} " +
00168:                         $"roster={save.roster?.entries?.Count ?? 0} live={_survivors.RosterState.Count}.");
00169:                 }
00170:                 // A missing save is also a completed restore decision. Never
00171:                 // fall through to a fresh cohort on a later SetupSurvivors.
00172:                 _survivorInitializationApplied = true;
00173:             }
00174:         }
00175:
00176:         /// <summary>C2 / Plan 20B — canonical airlock seal factor from the
00177:         /// authored door state (Secure 1 … Breached 0); unbound = secure.</summary>
00178:         private static float AirlockSealFactor(AirlockDoorState? doorState)
00179:         {
00180:             return doorState switch
00181:             {
00182:                 AirlockDoorState.Secure => 1f,
00183:                 AirlockDoorState.Cycling => 0.5f,
00184:                 AirlockDoorState.Open => 0f,
00185:                 AirlockDoorState.Breached => 0f,
00186:                 _ => 1f
00187:             };
00188:         }
00189:
00190:         /// <summary>C2 / Plan 20B — worst-room sump contamination (0..1) from the
00191:         /// canonical flooding authority; unbound/empty = dry.</summary>
00192:         private float MaxSumpContamination()
00193:         {
00194:             var nodes = _sumpFlooding?.System.State.nodes;
00195:             if (nodes == null) return 0f;
00196:             float max = 0f;
00197:             for (int i = 0; i < nodes.Count; i++)
00198:             {
00199:                 if (nodes[i] == null) continue;
00200:                 if (nodes[i].contaminationLevel > max) max = nodes[i].contaminationLevel;
00201:             }
00202:             return max;
00203:         }
00204:
00205:         private StartingCohortCatalog EnsureStartingCohortCatalog()
00206:         {
00207:             if (_startingCohortCatalog != null) return _startingCohortCatalog;
00208:
00209:             var fileIO = new FileSystemIO();
00210:             var serializer = new SystemTextJsonSerializer();
00211:             var canonical = SurvivorCatalogLoader.Load(_dataDir, fileIO, serializer);
00212:             var loaded = StartingCohortCatalogLoader.LoadDetailed(
00213:                 _dataDir,
00214:                 fileIO,
00215:                 serializer,
00216:                 canonical);
00217:
00218:             if (loaded.Errors.Count > 0)
00219:             {
00220:                 throw new InvalidOperationException(
00221:                     "Starting cohort catalog is invalid: " +
00222:                     string.Join("; ", loaded.Errors));
00223:             }
00224:
00225:             _startingCohortCatalog = loaded.Catalog;
00226:             return _startingCohortCatalog;
00227:         }
00228:
00229:         private StartingCohortProfile ResolveStartingCohort(string profileId)
00230:         {
00231:             var catalog = EnsureStartingCohortCatalog();
00232:             if (catalog.TryGet(profileId, out var profile))
00233:                 return profile;
00234:
00235:             GD.PushWarning(
00236:                 $"[Ashfall Godot] Unknown starting cohort '{profileId}', using Standard Holdfast.");
00237:             return catalog.DefaultProfile;
00238:         }
00239:
00240:         private void SetupUtilityAi()
00241:         {
00242:             if (_utilityAi != null) return;
00243:             _utilityAi = UtilityAiHostSession.Create(_dataDir);
00244:
00245:             if (_utilityAiPanel == null && _rightColumn != null)
00246:             {
00247:                 _utilityAiPanel = new UtilityAiPanel();
00248:                 _rightColumn.AddChild(_utilityAiPanel);
00249:             }
00250:             if (_utilityAiPanel != null)
00251:             {
00252:                 _utilityAiPanel.BindSession(_utilityAi);
00253:                 _utilityAiPanel.RefreshView();
00254:             }
00255:         }
00256:
00257:         private void OnUtilityAiEvaluateClicked()
00258:         {
00259:             SetupUtilityAi();
00260:             _statusLabel.Text = _utilityAi.EvaluateDemo("survivor_gunner_mikhail", 30f, 0.7f);
00261:             _utilityAiPanel.RefreshView();
00262:         }
00263:
00264:         private void OnSurvivorsOpenClicked()
00265:         {
00266:             SetupSurvivors();
00267:             _statusLabel.Text = "Survivors panel open.";
00268:             _codexViewer.Text = _survivors.StatusLine();
00269:         }
00270:
00271:         private void OnSurvivorsTickClicked()
00272:         {
00273:             SetupSurvivors();
00274:             _survivors.TickHour(6f);
00275:             SetupPhase0();
00276:             _phase0.TickHour(6f);
00277:             _statusLabel.Text = _survivors.LastEvent + "\n" + _phase0.LastEvent;
00278:             _codexViewer.Text = _survivors.StatusLine();
00279:         }
00280:
00281:         private void OnSurvivorsExposeClicked(string id, float rads)
00282:         {
00283:             SetupSurvivors();
00284:             _statusLabel.Text = _survivors.ExposeToZone(id, rads);
00285:             _codexViewer.Text = _survivors.StatusLine();
00286:         }
00287:
00288:         private void OnSurvivorsIodineClicked(string id)
00289:         {
00290:             SetupSurvivors();
00291:             _statusLabel.Text = _survivors.AdministerIodine(id);
00292:             _codexViewer.Text = _survivors.StatusLine();
00293:         }
00294:
00295:         private void OnSurvivorsAntiRadClicked(string id, float rads)
00296:         {
00297:             SetupSurvivors();
00298:             _statusLabel.Text = _survivors.AdministerAntiRad(id, rads);
00299:             _codexViewer.Text = _survivors.StatusLine();
00300:         }
00301:
00302:         private void SaveSurvivors()
00303:         {
00304:             if (_survivors == null) return;
00305:             if (CaptureSection("survivors", SurvivorsSaveStore.TryCapturePersisted(_survivors.CaptureSave())))
00306:                 GD.Print("[Ashfall Godot] Survivors save written.");
00307:         }
00308:
00309:         private void CloseSurvivorsOverlay()
00310:         {
00311:             _survivorsOverlay.Visible = false;
00312:         }
00313:
00314:         private void CloseSurvivorDetailPanel()
00315:         {
00316:             _survivorDetailPanel.Visible = false;
00317:         }
00318:
00319:     }
00320: }
```

## `src/Main.UiTests.UtilityAi.cs` — 72 lines; 2,639 bytes; SHA-256 `a0ca94d885825eedcf48e5d8746d8799d4e1a84c6689bef30def5cc1dbe8b3a4`
Declaration index:
- 00031: public partial class Main : Control
- 00037: private void RunUtilityAiUiTestAndQuit()
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
00020: using AtomicWar.GodotApp.Economy;
00021: using AtomicWar.GodotApp.YearOfAsh;
00022: using AtomicWar.GodotApp.Muster;
00023: using AtomicWar.GodotApp.Dose;
00024: using AtomicWar.GodotApp.UtilityAI;
00025: using AtomicWar.GodotApp.Radio;
00026: using AtomicWar.GodotApp.Audio;
00027: using AtomicWar.GodotApp.UI;
00028:
00029: namespace AtomicWar.GodotApp
00030: {
00031:     public partial class Main : Control
00032:     {
00033:         /// <summary>
00034:         /// Headless smoke: utility AI panel builds, scores render, refresh +
00035:         /// rebind are leak-free, evaluation selects an action.
00036:         /// </summary>
00037:         private void RunUtilityAiUiTestAndQuit()
00038:         {
00039:             BuildUserInterface();
00040:             SetupUtilityAi();
00041:
00042:             bool panel = _utilityAiPanel != null;
00043:             var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
00044:             var serializer = new SystemTextJsonSerializer();
00045:             var loadedActions = Ashfall.Core.UtilityAI.UtilityActionCatalogLoader.Load(_dataDir, fileIO, serializer);
00046:             var expectedIds = new System.Collections.Generic.HashSet<string>();
00047:             foreach (var act in loadedActions)
00048:             {
00049:                 if (!string.IsNullOrEmpty(act.id))
00050:                     expectedIds.Add(act.id);
00051:             }
00052:             bool catalog = _utilityAi.Actions.Count == expectedIds.Count &&
00053:                            _utilityAi.Actions.Count > 0 &&
00054:                            _utilityAi.Actions.TrueForAll(a => expectedIds.Contains(a.id));
00055:
00056:             int before = _utilityAiPanel!.GetChild(0).GetChildCount();
00057:             _utilityAiPanel.RefreshView();
00058:             _utilityAiPanel.RefreshView();
00059:             int after = _utilityAiPanel.GetChild(0).GetChildCount();
00060:             bool noLeak = before == after;
00061:
00062:             string result = _utilityAi.EvaluateDemo("sv_demo", 30f, 0.7f);
00063:             bool selected = result.Contains("selects");
00064:
00065:             bool pass = panel && catalog && noLeak && selected;
00066:             GD.Print($"[UtilityAiUiTest] panel={panel} catalog={catalog} noLeak={noLeak} selected={selected}");
00067:             HostCli.EmitSummary("utility_ai_uitest", pass, pass ? 0 : 1);
00068:             QuitUiTestAfterFrame(pass ? 0 : 1);
00069:         }
00070:
00071:     }
00072: }
```

## `Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs` — 322 lines; 12,478 bytes; SHA-256 `122419b360689a310870f95d007f64cc08ab786ce323d41a25f599fa1a67a919`
Declaration index:
- 00012: public class UtilityAiExpandedCatalogTests
- 00014: private static string FindDataDir()
- 00028: private static List<UtilityActionDef> LoadCatalog()
- 00038: private static AIActionContext Ctx(
- 00056: public void Catalog_LoadsExact20ActionsWithValidSchema()
- 00076: public void Catalog_PreservesOriginal6ActionsByteAndFieldParity()
- 00125: public void Catalog_All14NewActionsPresentAndCategorized()
- 00153: public void Catalog_ContainsZeroDuplicateIds()
- 00165: public void Scorer_CowardRefusesLoudLaborActions()
- 00183: public void Scorer_GodComplexRefusesMenialLaborActions()
- 00201: public void Scorer_PacifistRefusesSecurityWatchWithWeaponTag()
- 00215: public void Scorer_HitmanRefusesMedicalTriageActions()
- 00229: public void Scorer_GermaphobeGatedOnHazmatForMedicalTriage()
- 00243: public void Scorer_ExConRefusesOrderActions()
- 00257: public void Scorer_FatigueGatingDisablesWorkWhileAllowingRest()
- 00279: public void Scorer_CraftingSkillScalesRepairEquipment()
- 00292: public void Scorer_DeadSurvivorScoresZeroAcrossAll20Actions()
- 00305: public void Selection_PicksDeterministicallyWithSameSeed()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.UtilityAI;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests
00011: {
00012:     public class UtilityAiExpandedCatalogTests
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
00028:         private static List<UtilityActionDef> LoadCatalog()
00029:         {
00030:             string dataDir = FindDataDir();
00031:             Assert.False(string.IsNullOrEmpty(dataDir), "Could not find StreamingAssets/Data directory");
00032:             var actions = UtilityActionCatalogLoader.Load(
00033:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00034:             Assert.NotNull(actions);
00035:             return actions;
00036:         }
00037:
00038:         private static AIActionContext Ctx(
00039:             bool alive = true, float fatigue = 0f, float skill = 0f,
00040:             bool listless = false, bool hazmat = false, params string[] traits)
00041:         {
00042:             var ctx = new AIActionContext
00043:             {
00044:                 SurvivorId = "sv_test",
00045:                 IsAlive = alive,
00046:                 Fatigue = fatigue,
00047:                 CraftingSkill = skill,
00048:                 IsListless = listless,
00049:                 HasHazmat = hazmat
00050:             };
00051:             foreach (var t in traits) ctx.Traits.Add(t);
00052:             return ctx;
00053:         }
00054:
00055:         [Fact]
00056:         public void Catalog_LoadsExact20ActionsWithValidSchema()
00057:         {
00058:             var actions = LoadCatalog();
00059:             Assert.Equal(20, actions.Count);
00060:
00061:             foreach (var a in actions)
00062:             {
00063:                 Assert.False(string.IsNullOrWhiteSpace(a.id));
00064:                 Assert.StartsWith("action_", a.id);
00065:                 Assert.False(string.IsNullOrWhiteSpace(a.displayName));
00066:                 Assert.True(a.baseScore > 0f);
00067:                 Assert.True(a.weight > 0f);
00068:                 Assert.True(a.basePriority >= 0f);
00069:                 Assert.NotNull(a.tags);
00070:                 Assert.NotNull(a.curvePoints);
00071:                 Assert.True(a.curvePoints.Length >= 2);
00072:             }
00073:         }
00074:
00075:         [Fact]
00076:         public void Catalog_PreservesOriginal6ActionsByteAndFieldParity()
00077:         {
00078:             var actions = LoadCatalog();
00079:             Assert.True(actions.Count >= 6);
00080:
00081:             var a0 = actions[0];
00082:             Assert.Equal("action_weigh_goods", a0.id);
00083:             Assert.Equal("Weigh Goods", a0.displayName);
00084:             Assert.Equal(0.40f, a0.baseScore);
00085:             Assert.Equal(85.0f, a0.fatigueGate);
00086:             Assert.Equal(0.25f, a0.skillBonusFactor);
00087:             Assert.Contains(UtilityTags.TagLoudLabor, a0.tags);
00088:
00089:             var a1 = actions[1];
00090:             Assert.Equal("action_read_contract", a1.id);
00091:             Assert.Equal("Read Contract", a1.displayName);
00092:             Assert.Equal(0.35f, a1.baseScore);
00093:             Assert.Equal(90.0f, a1.fatigueGate);
00094:             Assert.Equal(0.20f, a1.skillBonusFactor);
00095:
00096:             var a2 = actions[2];
00097:             Assert.Equal("action_canvas_support", a2.id);
00098:             Assert.Equal("Canvas Support", a2.displayName);
00099:             Assert.Equal(0.45f, a2.baseScore);
00100:             Assert.Equal(80.0f, a2.fatigueGate);
00101:             Assert.Equal(0.15f, a2.skillBonusFactor);
00102:             Assert.Contains(UtilityTags.TagMenialLabor, a2.tags);
00103:
00104:             var a3 = actions[3];
00105:             Assert.Equal("action_run_vouch", a3.id);
00106:             Assert.Equal("Run Vouch", a3.displayName);
00107:             Assert.Equal(0.30f, a3.baseScore);
00108:             Assert.Equal(88.0f, a3.fatigueGate);
00109:             Assert.Equal(0.10f, a3.skillBonusFactor);
00110:
00111:             var a4 = actions[4];
00112:             Assert.Equal("action_audit_inventory", a4.id);
00113:             Assert.Equal("Audit Inventory", a4.displayName);
00114:             Assert.Equal(0.35f, a4.baseScore);
00115:             Assert.Equal(80.0f, a4.fatigueGate);
00116:
00117:             var a5 = actions[5];
00118:             Assert.Equal("action_file_report", a5.id);
00119:             Assert.Equal("File Report", a5.displayName);
00120:             Assert.Equal(0.35f, a5.baseScore);
00121:             Assert.Equal(80.0f, a5.fatigueGate);
00122:         }
00123:
00124:         [Fact]
00125:         public void Catalog_All14NewActionsPresentAndCategorized()
00126:         {
00127:             var actions = LoadCatalog();
00128:             var expectedNewIds = new[]
00129:             {
00130:                 "action_repair_equipment", // Maintenance 1
00131:                 "action_inspect_housing",  // Maintenance 2
00132:                 "action_treat_wounded",    // Medical 1
00133:                 "action_seek_treatment",   // Medical 2
00134:                 "action_cook_food",        // Food 1
00135:                 "action_preserve_food",    // Food 2
00136:                 "action_purify_water",     // Water 1
00137:                 "action_socialize",        // Social 1
00138:                 "action_resolve_conflict", // Social 2
00139:                 "action_train_skill",      // Training 1
00140:                 "action_teach_skill",      // Training 2
00141:                 "action_stand_watch",      // Security 1
00142:                 "action_conduct_research", // Research 1
00143:                 "action_rest"              // Rest 1
00144:             };
00145:
00146:             foreach (var id in expectedNewIds)
00147:             {
00148:                 Assert.Contains(actions, a => a.id == id);
00149:             }
00150:         }
00151:
00152:         [Fact]
00153:         public void Catalog_ContainsZeroDuplicateIds()
00154:         {
00155:             var actions = LoadCatalog();
00156:             var seen = new HashSet<string>(StringComparer.Ordinal);
00157:             foreach (var a in actions)
00158:             {
00159:                 Assert.True(seen.Add(a.id), $"Duplicate action ID found: {a.id}");
00160:             }
00161:             Assert.Equal(20, seen.Count);
00162:         }
00163:
00164:         [Fact]
00165:         public void Scorer_CowardRefusesLoudLaborActions()
00166:         {
00167:             var actions = LoadCatalog();
00168:             var repair = actions.Find(a => a.id == "action_repair_equipment")!;
00169:             var purify = actions.Find(a => a.id == "action_purify_water")!;
00170:
00171:             var scorer = new UtilityActionScorer();
00172:             var normalCtx = Ctx(fatigue: 20f, skill: 0.5f);
00173:             var cowardCtx = Ctx(fatigue: 20f, skill: 0.5f, traits: new[] { UtilityTags.TraitCoward });
00174:
00175:             Assert.True(scorer.Score(repair, normalCtx) > 0f);
00176:             Assert.Equal(0f, scorer.Score(repair, cowardCtx));
00177:
00178:             Assert.True(scorer.Score(purify, normalCtx) > 0f);
00179:             Assert.Equal(0f, scorer.Score(purify, cowardCtx));
00180:         }
00181:
00182:         [Fact]
00183:         public void Scorer_GodComplexRefusesMenialLaborActions()
00184:         {
00185:             var actions = LoadCatalog();
00186:             var preserve = actions.Find(a => a.id == "action_preserve_food")!;
00187:             var canvas = actions.Find(a => a.id == "action_canvas_support")!;
00188:
00189:             var scorer = new UtilityActionScorer();
00190:             var normalCtx = Ctx(fatigue: 20f);
00191:             var godComplexCtx = Ctx(fatigue: 20f, traits: new[] { UtilityTags.TraitGodComplex });
00192:
00193:             Assert.True(scorer.Score(preserve, normalCtx) > 0f);
00194:             Assert.Equal(0f, scorer.Score(preserve, godComplexCtx));
00195:
00196:             Assert.True(scorer.Score(canvas, normalCtx) > 0f);
00197:             Assert.Equal(0f, scorer.Score(canvas, godComplexCtx));
00198:         }
00199:
00200:         [Fact]
00201:         public void Scorer_PacifistRefusesSecurityWatchWithWeaponTag()
00202:         {
00203:             var actions = LoadCatalog();
00204:             var watch = actions.Find(a => a.id == "action_stand_watch")!;
00205:
00206:             var scorer = new UtilityActionScorer();
00207:             var normalCtx = Ctx(fatigue: 20f);
00208:             var pacifistCtx = Ctx(fatigue: 20f, traits: new[] { UtilityTags.TraitPacifist });
00209:
00210:             Assert.True(scorer.Score(watch, normalCtx) > 0f);
00211:             Assert.Equal(0f, scorer.Score(watch, pacifistCtx));
00212:         }
00213:
00214:         [Fact]
00215:         public void Scorer_HitmanRefusesMedicalTriageActions()
00216:         {
00217:             var actions = LoadCatalog();
00218:             var treat = actions.Find(a => a.id == "action_treat_wounded")!;
00219:
00220:             var scorer = new UtilityActionScorer();
00221:             var normalCtx = Ctx(fatigue: 20f);
00222:             var hitmanCtx = Ctx(fatigue: 20f, traits: new[] { UtilityTags.TraitHitman });
00223:
00224:             Assert.True(scorer.Score(treat, normalCtx) > 0f);
00225:             Assert.Equal(0f, scorer.Score(treat, hitmanCtx));
00226:         }
00227:
00228:         [Fact]
00229:         public void Scorer_GermaphobeGatedOnHazmatForMedicalTriage()
00230:         {
00231:             var actions = LoadCatalog();
00232:             var treat = actions.Find(a => a.id == "action_treat_wounded")!;
00233:
00234:             var scorer = new UtilityActionScorer();
00235:             var germaphobeNoHazmat = Ctx(fatigue: 20f, hazmat: false, traits: new[] { UtilityTags.TraitGermaphobe });
00236:             var germaphobeWithHazmat = Ctx(fatigue: 20f, hazmat: true, traits: new[] { UtilityTags.TraitGermaphobe });
00237:
00238:             Assert.Equal(0f, scorer.Score(treat, germaphobeNoHazmat));
00239:             Assert.True(scorer.Score(treat, germaphobeWithHazmat) > 0f);
00240:         }
00241:
00242:         [Fact]
00243:         public void Scorer_ExConRefusesOrderActions()
00244:         {
00245:             var actions = LoadCatalog();
00246:             var conflict = actions.Find(a => a.id == "action_resolve_conflict")!;
00247:
00248:             var scorer = new UtilityActionScorer();
00249:             var normalCtx = Ctx(fatigue: 20f);
00250:             var exConCtx = Ctx(fatigue: 20f, traits: new[] { UtilityTags.TraitExCon });
00251:
00252:             Assert.True(scorer.Score(conflict, normalCtx) > 0f);
00253:             Assert.Equal(0f, scorer.Score(conflict, exConCtx));
00254:         }
00255:
00256:         [Fact]
00257:         public void Scorer_FatigueGatingDisablesWorkWhileAllowingRest()
00258:         {
00259:             var actions = LoadCatalog();
00260:             var repair = actions.Find(a => a.id == "action_repair_equipment")!;
00261:             var cook = actions.Find(a => a.id == "action_cook_food")!;
00262:             var research = actions.Find(a => a.id == "action_conduct_research")!;
00263:             var rest = actions.Find(a => a.id == "action_rest")!;
00264:
00265:             var scorer = new UtilityActionScorer();
00266:             // High fatigue (92)
00267:             var exhaustedCtx = Ctx(fatigue: 92f, skill: 0.8f);
00268:
00269:             Assert.Equal(0f, scorer.Score(repair, exhaustedCtx));
00270:             Assert.Equal(0f, scorer.Score(cook, exhaustedCtx));
00271:             Assert.Equal(0f, scorer.Score(research, exhaustedCtx));
00272:
00273:             // Rest has fatigueGate == 0 and scores positively under high fatigue
00274:             float restScore = scorer.Score(rest, exhaustedCtx);
00275:             Assert.True(restScore > 0f, "Rest action must remain valid when exhausted");
00276:         }
00277:
00278:         [Fact]
00279:         public void Scorer_CraftingSkillScalesRepairEquipment()
00280:         {
00281:             var actions = LoadCatalog();
00282:             var repair = actions.Find(a => a.id == "action_repair_equipment")!;
00283:
00284:             var scorer = new UtilityActionScorer();
00285:             float skilled = scorer.Score(repair, Ctx(fatigue: 20f, skill: 1.0f));
00286:             float unskilled = scorer.Score(repair, Ctx(fatigue: 20f, skill: 0.0f));
00287:
00288:             Assert.True(skilled > unskilled, "Skilled mechanic must score repair higher than unskilled survivor");
00289:         }
00290:
00291:         [Fact]
00292:         public void Scorer_DeadSurvivorScoresZeroAcrossAll20Actions()
00293:         {
00294:             var actions = LoadCatalog();
00295:             var scorer = new UtilityActionScorer();
00296:             var deadCtx = Ctx(alive: false, fatigue: 0f, skill: 1f);
00297:
00298:             foreach (var a in actions)
00299:             {
00300:                 Assert.Equal(0f, scorer.Score(a, deadCtx));
00301:             }
00302:         }
00303:
00304:         [Fact]
00305:         public void Selection_PicksDeterministicallyWithSameSeed()
00306:         {
00307:             var actions = LoadCatalog();
00308:             var sys = new UtilityAiSystem();
00309:             var ctx = Ctx(fatigue: 40f, skill: 0.6f);
00310:
00311:             for (int seed = 1; seed <= 20; seed++)
00312:             {
00313:                 var pick1 = sys.SelectAction(ctx, actions, new SeededRng(seed * 77));
00314:                 var pick2 = sys.SelectAction(ctx, actions, new SeededRng(seed * 77));
00315:
00316:                 Assert.NotNull(pick1);
00317:                 Assert.NotNull(pick2);
00318:                 Assert.Equal(pick1.id, pick2.id);
00319:             }
00320:         }
00321:     }
00322: }
```

## `Ashfall.Core.Tests/UtilityAiTests.cs` — 303 lines; 12,568 bytes; SHA-256 `4a38836ab68615450df3a4f0381ffc0130f89b2b3a64ab468eab9e1880d47176`
Declaration index:
- 00009: public class UtilityAiTests
- 00011: private static UtilityActionDef Action(
- 00027: private static AIActionContext Ctx(
- 00045: public void Scorer_DeadSurvivorScoresZero()
- 00052: public void Scorer_FatigueGateZeroesRaw()
- 00062: public void Scorer_SkillBonusApplies()
- 00075: public void Scorer_CurveTransformsRaw()
- 00090: public void Scorer_ListlessPenaltyFloorsAtZero()
- 00100: public void Scorer_OverrideActionsPassThroughUnclamped()
- 00111: public void Scorer_VetoMatrix_CowardRefusesLoudLabor()
- 00120: public void Scorer_VetoMatrix_PacifistRefusesWeapons_BlindRefusesGuns()
- 00140: public void Selection_PicksHighestScoringCandidate()
- 00150: public void Selection_OverrideWinsOverAnyNormalAction()
- 00161: public void Selection_EmptyOrNullCandidatesReturnsNull()
- 00170: public void Selection_AllVetoedReturnsNull()
- 00179: public void Selection_WithoutNoise_TiesFirstWins()
- 00193: public void Selection_WithNoise_TiePickDeterministicPerSeed()
- 00211: public void Selection_DeterministicSameSeedSamePick()
- 00224: public void Selection_FiresEventOnPick()
- 00234: public void Scorer_NullActionOrContextScoresZero()
- 00243: private static string FindDataDir()
- 00258: public void Catalog_LoadsExpandedCatalogWithOriginalActionsPreserved()
- 00284: public void Catalog_WeighGoodsUnityParity()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: using Ashfall.Core;
00004: using Ashfall.Core.UtilityAI;
00005: using Xunit;
00006:
00007: namespace Ashfall.Core.Tests
00008: {
00009:     public class UtilityAiTests
00010:     {
00011:         private static UtilityActionDef Action(
00012:             string id, float baseScore = 0.5f, float priority = 0.1f,
00013:             float weight = 1f, bool overrideAction = false, params string[] tags)
00014:         {
00015:             return new UtilityActionDef
00016:             {
00017:                 id = id,
00018:                 displayName = id,
00019:                 baseScore = baseScore,
00020:                 basePriority = priority,
00021:                 weight = weight,
00022:                 isOverrideAction = overrideAction,
00023:                 tags = tags
00024:             };
00025:         }
00026:
00027:         private static AIActionContext Ctx(
00028:             bool alive = true, float fatigue = 0f, float skill = 0f,
00029:             bool listless = false, bool hazmat = false, params string[] traits)
00030:         {
00031:             var ctx = new AIActionContext
00032:             {
00033:                 SurvivorId = "sv_1",
00034:                 IsAlive = alive,
00035:                 Fatigue = fatigue,
00036:                 CraftingSkill = skill,
00037:                 IsListless = listless,
00038:                 HasHazmat = hazmat
00039:             };
00040:             foreach (var t in traits) ctx.Traits.Add(t);
00041:             return ctx;
00042:         }
00043:
00044:         [Fact]
00045:         public void Scorer_DeadSurvivorScoresZero()
00046:         {
00047:             var scorer = new UtilityActionScorer();
00048:             Assert.Equal(0f, scorer.Score(Action("a"), Ctx(alive: false)));
00049:         }
00050:
00051:         [Fact]
00052:         public void Scorer_FatigueGateZeroesRaw()
00053:         {
00054:             var scorer = new UtilityActionScorer();
00055:             var action = Action("a", baseScore: 0.4f);
00056:             action.fatigueGate = 85f;
00057:             Assert.Equal(0f, scorer.Score(action, Ctx(fatigue: 90f)));
00058:             Assert.True(scorer.Score(action, Ctx(fatigue: 50f)) > 0f);
00059:         }
00060:
00061:         [Fact]
00062:         public void Scorer_SkillBonusApplies()
00063:         {
00064:             var scorer = new UtilityActionScorer();
00065:             var action = Action("a", baseScore: 0.4f);
00066:             action.skillBonusFactor = 0.25f;
00067:             float skilled = scorer.Score(action, Ctx(skill: 0.8f));
00068:             float unskilled = scorer.Score(action, Ctx(skill: 0f));
00069:             Assert.True(skilled > unskilled);
00070:             // raw 0.4 + 0.2 = 0.6, clamped 0..1.
00071:             Assert.True(skilled <= 1f && skilled > 0.5f);
00072:         }
00073:
00074:         [Fact]
00075:         public void Scorer_CurveTransformsRaw()
00076:         {
00077:             var scorer = new UtilityActionScorer();
00078:             var action = Action("a", baseScore: 0.5f);
00079:             action.curvePoints = new[]
00080:             {
00081:                 new CurvePoint { x = 0f, y = 0f },
00082:                 new CurvePoint { x = 0.5f, y = 0.1f },
00083:                 new CurvePoint { x = 1f, y = 1f }
00084:             };
00085:             // raw 0.5 -> curve 0.1; (0.1 + 0.1) * 1 = 0.2.
00086:             Assert.Equal(0.2f, scorer.Score(action, Ctx()), 3);
00087:         }
00088:
00089:         [Fact]
00090:         public void Scorer_ListlessPenaltyFloorsAtZero()
00091:         {
00092:             var scorer = new UtilityActionScorer();
00093:             var tiny = Action("a", baseScore: 0.01f); // raw 0.01 -> ~0.11 -> -0.08 -> 0.03
00094:             Assert.True(scorer.Score(tiny, Ctx(listless: true)) >= 0f);
00095:             var zero = Action("b", baseScore: 0f);
00096:             Assert.Equal(0f, scorer.Score(zero, Ctx(listless: true)));
00097:         }
00098:
00099:         [Fact]
00100:         public void Scorer_OverrideActionsPassThroughUnclamped()
00101:         {
00102:             var scorer = new UtilityActionScorer();
00103:             var big = Action("override", baseScore: 0.9f, weight: 2f, overrideAction: true);
00104:             // (0.9 + 0.1) * 2 = 2.0, unclamped.
00105:             Assert.Equal(2f, scorer.Score(big, Ctx()), 3);
00106:             var normal = Action("normal", baseScore: 0.9f, weight: 2f);
00107:             Assert.Equal(1f, scorer.Score(normal, Ctx())); // clamped
00108:         }
00109:
00110:         [Fact]
00111:         public void Scorer_VetoMatrix_CowardRefusesLoudLabor()
00112:         {
00113:             var scorer = new UtilityActionScorer();
00114:             var loud = Action("weigh", baseScore: 0.4f, tags: UtilityTags.TagLoudLabor);
00115:             Assert.Equal(0f, scorer.Score(loud, Ctx(traits: UtilityTags.TraitCoward)));
00116:             Assert.True(scorer.Score(loud, Ctx()) > 0f);
00117:         }
00118:
00119:         [Fact]
00120:         public void Scorer_VetoMatrix_PacifistRefusesWeapons_BlindRefusesGuns()
00121:         {
00122:             var scorer = new UtilityActionScorer();
00123:             Assert.Equal(0f, scorer.Score(Action("w", tags: UtilityTags.TagWeapon),
00124:                 Ctx(traits: UtilityTags.TraitPacifist)));
00125:             Assert.Equal(0f, scorer.Score(Action("g", tags: UtilityTags.TagGun),
00126:                 Ctx(traits: UtilityTags.TraitBlind)));
00127:             Assert.Equal(0f, scorer.Score(Action("o", tags: UtilityTags.TagOrder),
00128:                 Ctx(traits: UtilityTags.TraitExCon)));
00129:             Assert.Equal(0f, scorer.Score(Action("m", tags: UtilityTags.TagMedicalTriage),
00130:                 Ctx(traits: UtilityTags.TraitHitman)));
00131:             Assert.Equal(0f, scorer.Score(Action("f", tags: UtilityTags.TagFarming),
00132:                 Ctx(traits: UtilityTags.TraitHitman)));
00133:             Assert.Equal(0f, scorer.Score(Action("m", tags: UtilityTags.TagMedicalTriage),
00134:                 Ctx(traits: UtilityTags.TraitGermaphobe)));
00135:             Assert.True(scorer.Score(Action("m", tags: UtilityTags.TagMedicalTriage),
00136:                 Ctx(traits: UtilityTags.TraitGermaphobe, hazmat: true)) > 0f);
00137:         }
00138:
00139:         [Fact]
00140:         public void Selection_PicksHighestScoringCandidate()
00141:         {
00142:             var sys = new UtilityAiSystem();
00143:             var low = Action("low", baseScore: 0.2f);
00144:             var high = Action("high", baseScore: 0.8f);
00145:             var picked = sys.SelectAction(Ctx(), new List<UtilityActionDef> { low, high }, new SeededRng(1));
00146:             Assert.Equal("high", picked.id);
00147:         }
00148:
00149:         [Fact]
00150:         public void Selection_OverrideWinsOverAnyNormalAction()
00151:         {
00152:             var sys = new UtilityAiSystem();
00153:             var normal = Action("normal", baseScore: 1f);
00154:             var overrideAction = Action("override", baseScore: 0.3f, weight: 5f, overrideAction: true);
00155:             var picked = sys.SelectAction(Ctx(),
00156:                 new List<UtilityActionDef> { normal, overrideAction }, new SeededRng(1));
00157:             Assert.Equal("override", picked.id);
00158:         }
00159:
00160:         [Fact]
00161:         public void Selection_EmptyOrNullCandidatesReturnsNull()
00162:         {
00163:             var sys = new UtilityAiSystem();
00164:             Assert.Null(sys.SelectAction(Ctx(), new List<UtilityActionDef>(), new SeededRng(1)));
00165:             Assert.Null(sys.SelectAction(null, new List<UtilityActionDef> { Action("a") }, new SeededRng(1)));
00166:             Assert.Null(sys.SelectAction(Ctx(), null, new SeededRng(1)));
00167:         }
00168:
00169:         [Fact]
00170:         public void Selection_AllVetoedReturnsNull()
00171:         {
00172:             var sys = new UtilityAiSystem();
00173:             var loud = Action("weigh", baseScore: 0.4f, tags: UtilityTags.TagLoudLabor);
00174:             var ctx = Ctx(traits: UtilityTags.TraitCoward);
00175:             Assert.Null(sys.SelectAction(ctx, new List<UtilityActionDef> { loud }, new SeededRng(1)));
00176:         }
00177:
00178:         [Fact]
00179:         public void Selection_WithoutNoise_TiesFirstWins()
00180:         {
00181:             // No rng = no noise: strict first-wins on ties.
00182:             var sys = new UtilityAiSystem();
00183:             var a = Action("a", baseScore: 0.5f);
00184:             var b = Action("b", baseScore: 0.5f);
00185:             for (int seed = 0; seed < 20; seed++)
00186:             {
00187:                 var picked = sys.SelectAction(Ctx(), new List<UtilityActionDef> { a, b }, null);
00188:                 Assert.Equal("a", picked.id);
00189:             }
00190:         }
00191:
00192:         [Fact]
00193:         public void Selection_WithNoise_TiePickDeterministicPerSeed()
00194:         {
00195:             // Seeded noise may flip a tie, but the SAME seed must pick the
00196:             // SAME candidate every time (cross-process determinism).
00197:             var sys = new UtilityAiSystem();
00198:             var a = Action("a", baseScore: 0.5f);
00199:             var b = Action("b", baseScore: 0.5f);
00200:             var candidates = new List<UtilityActionDef> { a, b };
00201:             for (int seed = 0; seed < 10; seed++)
00202:             {
00203:                 string p1 = sys.SelectAction(Ctx(), candidates, new SeededRng(seed)).id;
00204:                 string p2 = sys.SelectAction(Ctx(), candidates, new SeededRng(seed)).id;
00205:                 Assert.Equal(p1, p2);
00206:                 Assert.Contains(p1, new[] { "a", "b" });
00207:             }
00208:         }
00209:
00210:         [Fact]
00211:         public void Selection_DeterministicSameSeedSamePick()
00212:         {
00213:             var sys = new UtilityAiSystem();
00214:             var candidates = new List<UtilityActionDef>
00215:             {
00216:                 Action("x", baseScore: 0.3f), Action("y", baseScore: 0.45f), Action("z", baseScore: 0.6f)
00217:             };
00218:             string pickA = sys.SelectAction(Ctx(), candidates, new SeededRng(42)).id;
00219:             string pickB = sys.SelectAction(Ctx(), candidates, new SeededRng(42)).id;
00220:             Assert.Equal(pickA, pickB);
00221:         }
00222:
00223:         [Fact]
00224:         public void Selection_FiresEventOnPick()
00225:         {
00226:             var sys = new UtilityAiSystem();
00227:             string pickedId = null;
00228:             sys.OnActionSelected += (sv, id, score) => pickedId = id;
00229:             sys.SelectAction(Ctx(), new List<UtilityActionDef> { Action("a", baseScore: 0.5f) }, new SeededRng(1));
00230:             Assert.Equal("a", pickedId);
00231:         }
00232:
00233:         [Fact]
00234:         public void Scorer_NullActionOrContextScoresZero()
00235:         {
00236:             var scorer = new UtilityActionScorer();
00237:             Assert.Equal(0f, scorer.Score(null, Ctx()));
00238:             Assert.Equal(0f, scorer.Score(Action("a"), null));
00239:         }
00240:
00241:         // ── Data catalog ───────────────────────────────────────────────
00242:
00243:         private static string FindDataDir()
00244:         {
00245:             string search = System.IO.Directory.GetCurrentDirectory();
00246:             for (int i = 0; i < 6; i++)
00247:             {
00248:                 string candidate = System.IO.Path.Combine(search, "Assets", "StreamingAssets", "Data");
00249:                 if (System.IO.Directory.Exists(candidate)) return candidate;
00250:                 string parent = System.IO.Directory.GetParent(search)?.FullName;
00251:                 if (parent == null) break;
00252:                 search = parent;
00253:             }
00254:             return string.Empty;
00255:         }
00256:
00257:         [Fact]
00258:         public void Catalog_LoadsExpandedCatalogWithOriginalActionsPreserved()
00259:         {
00260:             string dataDir = FindDataDir();
00261:             if (string.IsNullOrEmpty(dataDir)) return;
00262:
00263:             var defs = UtilityActionCatalogLoader.Load(
00264:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00265:             Assert.Equal(20, defs.Count);
00266:             // Original 6 actions preserved
00267:             Assert.Contains(defs, d => d.id == "action_weigh_goods");
00268:             Assert.Contains(defs, d => d.id == "action_read_contract");
00269:             Assert.Contains(defs, d => d.id == "action_canvas_support");
00270:             Assert.Contains(defs, d => d.id == "action_run_vouch");
00271:             Assert.Contains(defs, d => d.id == "action_audit_inventory");
00272:             Assert.Contains(defs, d => d.id == "action_file_report");
00273:             foreach (var d in defs)
00274:             {
00275:                 Assert.False(string.IsNullOrEmpty(d.displayName));
00276:                 Assert.True(d.baseScore > 0f);
00277:                 Assert.True(d.weight > 0f);
00278:                 Assert.NotNull(d.tags);
00279:                 Assert.NotNull(d.curvePoints);
00280:             }
00281:         }
00282:
00283:         [Fact]
00284:         public void Catalog_WeighGoodsUnityParity()
00285:         {
00286:             string dataDir = FindDataDir();
00287:             if (string.IsNullOrEmpty(dataDir)) return;
00288:
00289:             var defs = UtilityActionCatalogLoader.Load(
00290:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00291:             var weigh = defs.Find(d => d.id == "action_weigh_goods");
00292:             Assert.NotNull(weigh);
00293:             // Unity parity: base 0.40, + skill * 0.25, fatigue gate 85.
00294:             Assert.Equal(0.40f, weigh.baseScore);
00295:             Assert.Equal(0.25f, weigh.skillBonusFactor);
00296:             Assert.Equal(85f, weigh.fatigueGate);
00297:             var scorer = new UtilityActionScorer();
00298:             Assert.Equal(0f, scorer.Score(weigh, Ctx(fatigue: 90f)));
00299:             Assert.True(scorer.Score(weigh, Ctx(skill: 0.8f)) >
00300:                         scorer.Score(weigh, Ctx(skill: 0f)));
00301:         }
00302:     }
00303: }
```

## `Ashfall.Core.Tests/UtilityAiProbeTests.cs` — 258 lines; 10,751 bytes; SHA-256 `340321ce0938ee88cdeb1267ce948bd7734478dd207876db24524455916367f5`
Declaration index:
- 00014: public class UtilityAiProbeTests
- 00016: private static UtilityActionDef Action(
- 00032: private static AIActionContext Ctx(
- 00050: public void Probe_SeedFuzz_100SeedsMixedContexts_NoExceptionsDeterministic()
- 00085: public void Probe_CurveBounds_IdentitySinglePointNonMonotonic()
- 00108: public void Probe_FatigueGateBoundary_AtGateAllowedAboveVetoed()
- 00118: public void Probe_OverrideDominatesUnderNoise()
- 00132: public void Probe_EmptyCatalog_SelectsNull()
- 00139: public void Probe_AllZeroScoreActions_SelectsNull()
- 00150: public void Probe_VetoMatrix_EveryTraitTagPair()
- 00179: public void Probe_ScoreAll_OrdinalStableNoSideEffects()
- 00195: public void Probe_UnsortedCurvePoints_DoNotMisEvaluate()
- 00212: public void Probe_NoiseOnClampedScore_NeverNaN_DeterministicPerSeed()
- 00230: public void Probe_MissingCatalogFile_ReturnsEmpty()
- 00238: public void Probe_ScoreAll_NullScorerDefaults()
- 00247: public void Probe_SelectionDoesNotMutateContextOrDefs()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core;
00005: using Ashfall.Core.UtilityAI;
00006: using Xunit;
00007:
00008: namespace Ashfall.Core.Tests
00009: {
00010:     /// <summary>
00011:     /// Adversarial probes for the Utility AI port (Phase F loop). Every probe
00012:     /// is a permanent regression test.
00013:     /// </summary>
00014:     public class UtilityAiProbeTests
00015:     {
00016:         private static UtilityActionDef Action(
00017:             string id, float baseScore = 0.5f, float priority = 0.1f,
00018:             float weight = 1f, bool overrideAction = false, params string[] tags)
00019:         {
00020:             return new UtilityActionDef
00021:             {
00022:                 id = id,
00023:                 displayName = id,
00024:                 baseScore = baseScore,
00025:                 basePriority = priority,
00026:                 weight = weight,
00027:                 isOverrideAction = overrideAction,
00028:                 tags = tags
00029:             };
00030:         }
00031:
00032:         private static AIActionContext Ctx(
00033:             float fatigue = 0f, float skill = 0f, bool listless = false,
00034:             bool hazmat = false, params string[] traits)
00035:         {
00036:             var ctx = new AIActionContext
00037:             {
00038:                 SurvivorId = "sv_p",
00039:                 IsAlive = true,
00040:                 Fatigue = fatigue,
00041:                 CraftingSkill = skill,
00042:                 IsListless = listless,
00043:                 HasHazmat = hazmat
00044:             };
00045:             foreach (var t in traits) ctx.Traits.Add(t);
00046:             return ctx;
00047:         }
00048:
00049:         [Fact]
00050:         public void Probe_SeedFuzz_100SeedsMixedContexts_NoExceptionsDeterministic()
00051:         {
00052:             var sys = new UtilityAiSystem();
00053:             var candidates = new List<UtilityActionDef>
00054:             {
00055:                 Action("a", baseScore: 0.3f, tags: UtilityTags.TagLoudLabor),
00056:                 Action("b", baseScore: 0.5f, tags: UtilityTags.TagGun),
00057:                 Action("c", baseScore: 0.7f, tags: UtilityTags.TagMedicalTriage),
00058:                 Action("d", baseScore: 0.2f, weight: 3f, overrideAction: true),
00059:                 Action("e", baseScore: 0.4f)
00060:             };
00061:             var traitsPool = new[]
00062:             {
00063:                 UtilityTags.TraitCoward, UtilityTags.TraitGodComplex, UtilityTags.TraitPacifist,
00064:                 UtilityTags.TraitBlind, UtilityTags.TraitExCon, UtilityTags.TraitHitman,
00065:                 UtilityTags.TraitGermaphobe
00066:             };
00067:             for (int seed = 0; seed < 100; seed++)
00068:             {
00069:                 var ctx = Ctx(
00070:                     fatigue: seed % 101f,
00071:                     skill: (seed % 11) / 10f,
00072:                     listless: seed % 3 == 0,
00073:                     hazmat: seed % 2 == 0,
00074:                     traits: new[] { traitsPool[seed % traitsPool.Length] });
00075:                 var picked = sys.SelectAction(ctx, candidates, new SeededRng(seed));
00076:                 if (picked != null)
00077:                     Assert.True(picked.baseScore > 0f || picked.isOverrideAction);
00078:                 // Determinism: same seed, same context, same pick.
00079:                 var again = sys.SelectAction(ctx, candidates, new SeededRng(seed));
00080:                 Assert.Equal(picked, again);
00081:             }
00082:         }
00083:
00084:         [Fact]
00085:         public void Probe_CurveBounds_IdentitySinglePointNonMonotonic()
00086:         {
00087:             var scorer = new UtilityActionScorer();
00088:             var identity = Action("i", baseScore: 0.5f);
00089:             // Empty curve -> identity passthrough: raw 0.5 -> (0.5+0.1)=0.6.
00090:             Assert.Equal(0.6f, scorer.Score(identity, Ctx()), 3);
00091:
00092:             var single = Action("s", baseScore: 0.5f);
00093:             single.curvePoints = new[] { new CurvePoint { x = 0.5f, y = 0.9f } };
00094:             // Single point returns its y as the CURVED value; +0.1 priority -> 1.0 clamped.
00095:             Assert.Equal(1f, scorer.Score(single, Ctx()), 3);
00096:
00097:             var clamped = Action("c", baseScore: 0.5f);
00098:             clamped.curvePoints = new[]
00099:             {
00100:                 new CurvePoint { x = 0.2f, y = 0f },
00101:                 new CurvePoint { x = 0.4f, y = 1f }
00102:             };
00103:             // raw 0.5 >= last x (0.4) -> last y = 1.0; (1+0.1)=1.1 clamped 1.
00104:             Assert.Equal(1f, scorer.Score(clamped, Ctx()), 3);
00105:         }
00106:
00107:         [Fact]
00108:         public void Probe_FatigueGateBoundary_AtGateAllowedAboveVetoed()
00109:         {
00110:             var scorer = new UtilityActionScorer();
00111:             var a = Action("a", baseScore: 0.4f);
00112:             a.fatigueGate = 85f;
00113:             Assert.True(scorer.Score(a, Ctx(fatigue: 85f)) > 0f);   // exactly at gate
00114:             Assert.Equal(0f, scorer.Score(a, Ctx(fatigue: 85.01f))); // above gate
00115:         }
00116:
00117:         [Fact]
00118:         public void Probe_OverrideDominatesUnderNoise()
00119:         {
00120:             var sys = new UtilityAiSystem();
00121:             var normal = Action("normal", baseScore: 0.9f, weight: 1f);      // 1.0 clamped
00122:             var ovr = Action("override", baseScore: 0.3f, weight: 4f, overrideAction: true); // 1.3 unclamped
00123:             for (int seed = 0; seed < 50; seed++)
00124:             {
00125:                 var picked = sys.SelectAction(Ctx(),
00126:                     new List<UtilityActionDef> { normal, ovr }, new SeededRng(seed));
00127:                 Assert.Equal("override", picked.id);
00128:             }
00129:         }
00130:
00131:         [Fact]
00132:         public void Probe_EmptyCatalog_SelectsNull()
00133:         {
00134:             var sys = new UtilityAiSystem();
00135:             Assert.Null(sys.SelectAction(Ctx(), new List<UtilityActionDef>(), new SeededRng(1)));
00136:         }
00137:
00138:         [Fact]
00139:         public void Probe_AllZeroScoreActions_SelectsNull()
00140:         {
00141:             var sys = new UtilityAiSystem();
00142:             var dead = Action("dead", baseScore: 0f, priority: 0f);
00143:             var gated = Action("gated", baseScore: 0.4f, priority: 0f);
00144:             gated.fatigueGate = 50f;
00145:             var ctx = Ctx(fatigue: 90f);
00146:             Assert.Null(sys.SelectAction(ctx, new List<UtilityActionDef> { dead, gated }, new SeededRng(1)));
00147:         }
00148:
00149:         [Fact]
00150:         public void Probe_VetoMatrix_EveryTraitTagPair()
00151:         {
00152:             var scorer = new UtilityActionScorer();
00153:             string[][] pairs =
00154:             {
00155:                 new[] { UtilityTags.TraitCoward, UtilityTags.TagLoudLabor },
00156:                 new[] { UtilityTags.TraitGodComplex, UtilityTags.TagMenialLabor },
00157:                 new[] { UtilityTags.TraitPacifist, UtilityTags.TagWeapon },
00158:                 new[] { UtilityTags.TraitBlind, UtilityTags.TagGun },
00159:                 new[] { UtilityTags.TraitExCon, UtilityTags.TagOrder },
00160:                 new[] { UtilityTags.TraitHitman, UtilityTags.TagMedicalTriage },
00161:                 new[] { UtilityTags.TraitHitman, UtilityTags.TagFarming },
00162:                 new[] { UtilityTags.TraitGermaphobe, UtilityTags.TagMedicalTriage }
00163:             };
00164:             foreach (var pair in pairs)
00165:             {
00166:                 float score = scorer.Score(Action("x", baseScore: 0.5f, tags: pair[1]),
00167:                     Ctx(traits: pair[0]));
00168:                 Assert.True(score == 0f, $"{pair[0]} x {pair[1]} must veto, got {score}");
00169:             }
00170:             // Germaphobe with hazmat is NOT vetoed.
00171:             Assert.True(scorer.Score(Action("x", baseScore: 0.5f, tags: UtilityTags.TagMedicalTriage),
00172:                 Ctx(hazmat: true, traits: UtilityTags.TraitGermaphobe)) > 0f);
00173:             // Non-matching trait-tag pairs pass.
00174:             Assert.True(scorer.Score(Action("x", baseScore: 0.5f, tags: UtilityTags.TagLoudLabor),
00175:                 Ctx(traits: UtilityTags.TraitPacifist)) > 0f);
00176:         }
00177:
00178:         [Fact]
00179:         public void Probe_ScoreAll_OrdinalStableNoSideEffects()
00180:         {
00181:             var sys = new UtilityAiSystem();
00182:             var scorer = new UtilityActionScorer();
00183:             var candidates = new List<UtilityActionDef>
00184:             {
00185:                 Action("b", baseScore: 0.6f), Action("a", baseScore: 0.4f), Action("c", baseScore: 0.5f)
00186:             };
00187:             var scored = sys.ScoreAll(Ctx(), candidates, scorer);
00188:             Assert.Equal(3, scored.Count);
00189:             Assert.Equal("b", scored[0].Key.id); // caller order preserved
00190:             // ScoreAll must not mutate the action defs.
00191:             Assert.Equal(0.6f, candidates[0].baseScore);
00192:         }
00193:
00194:         [Fact]
00195:         public void Probe_UnsortedCurvePoints_DoNotMisEvaluate()
00196:         {
00197:             var scorer = new UtilityActionScorer();
00198:             var unsorted = Action("u", baseScore: 0.5f);
00199:             unsorted.curvePoints = new[]
00200:             {
00201:                 new CurvePoint { x = 1f, y = 1f },
00202:                 new CurvePoint { x = 0f, y = 0f },
00203:                 new CurvePoint { x = 0.5f, y = 0.5f }
00204:             };
00205:             // raw 0.5 must evaluate as 0.5 regardless of declaration order.
00206:             float score = scorer.Score(unsorted, Ctx());
00207:             Assert.True(Math.Abs(score - 0.6f) < 1e-3f,
00208:                 $"unsorted curve must evaluate identically to sorted, got {score}");
00209:         }
00210:
00211:         [Fact]
00212:         public void Probe_NoiseOnClampedScore_NeverNaN_DeterministicPerSeed()
00213:         {
00214:             // Unity parity: noise is added AFTER clamping, so a clamped 1.0
00215:             // normal action can report 1.0 + noise via the event. Never NaN,
00216:             // never negative, deterministic per seed.
00217:             var sys = new UtilityAiSystem();
00218:             var top = Action("top", baseScore: 0.9f); // (0.9+0.1) = 1.0 clamped
00219:             float lastScore = -1f;
00220:             sys.OnActionSelected += (sv, id, score) => lastScore = score;
00221:             for (int seed = 0; seed < 20; seed++)
00222:             {
00223:                 sys.SelectAction(Ctx(), new List<UtilityActionDef> { top }, new SeededRng(seed));
00224:                 Assert.False(float.IsNaN(lastScore));
00225:                 Assert.True(lastScore >= 1f);
00226:             }
00227:         }
00228:
00229:         [Fact]
00230:         public void Probe_MissingCatalogFile_ReturnsEmpty()
00231:         {
00232:             var defs = UtilityActionCatalogLoader.Load(
00233:                 "/nonexistent", new FileSystemIO(), new SystemTextJsonSerializer());
00234:             Assert.Empty(defs);
00235:         }
00236:
00237:         [Fact]
00238:         public void Probe_ScoreAll_NullScorerDefaults()
00239:         {
00240:             var sys = new UtilityAiSystem();
00241:             var scored = sys.ScoreAll(Ctx(), new List<UtilityActionDef> { Action("a", baseScore: 0.5f) });
00242:             Assert.Single(scored);
00243:             Assert.True(scored[0].Value > 0f);
00244:         }
00245:
00246:         [Fact]
00247:         public void Probe_SelectionDoesNotMutateContextOrDefs()
00248:         {
00249:             var sys = new UtilityAiSystem();
00250:             var def = Action("a", baseScore: 0.5f);
00251:             var ctx = Ctx(skill: 0.4f);
00252:             sys.SelectAction(ctx, new List<UtilityActionDef> { def }, new SeededRng(1));
00253:             Assert.Equal(0.5f, def.baseScore);
00254:             Assert.Equal(0.4f, ctx.CraftingSkill);
00255:             Assert.Empty(ctx.Traits);
00256:         }
00257:     }
00258: }
```

## `Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs` — 256 lines; 11,291 bytes; SHA-256 `160c23615b4d218512ce37de4e490899850aa421670f323d77316220b65973ba`
Declaration index:
- 00013: public class Plan88_72ConfessionUtilityAiIntegrationTests
- 00030: private ConfessionSecretCatalog CreateLoadedConfessionCatalog()
- 00039: private List<UtilityActionDef> LoadUtilityActionCatalog()
- 00048: public void Catalogs_LoadCleanly_WithValidSchemaAndReferentialIntegrity()
- 00082: public void ConfessionResolution_Forgiveness_RestoresSocialBonds_AndEnablesHighProductivityActions()
- 00143: public void ConfessionResolution_ExposureOrGuilt_IncreasesFatigue_GatingLaborAndBiasingRest()
- 00204: public void ConfessionAndUtilityAi_DeterministicReplayAndSaveRestoreRoundTrip()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Phantoms;
00007: using Ashfall.Core.Survivors;
00008: using Ashfall.Core.UtilityAI;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.Survivors
00012: {
00013:     public class Plan88_72ConfessionUtilityAiIntegrationTests
00014:     {
00015:         private readonly string _dataDir;
00016:         private readonly IJsonSerializer _serializer = new SystemTextJsonSerializer();
00017:
00018:         public Plan88_72ConfessionUtilityAiIntegrationTests()
00019:         {
00020:             if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir))
00021:             {
00022:                 _dataDir = dir;
00023:             }
00024:             else
00025:             {
00026:                 _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
00027:             }
00028:         }
00029:
00030:         private ConfessionSecretCatalog CreateLoadedConfessionCatalog()
00031:         {
00032:             var catalog = new ConfessionSecretCatalog();
00033:             string filePath = Path.Combine(_dataDir, "confession_secrets.json");
00034:             Assert.True(File.Exists(filePath), $"confession_secrets.json must exist at {filePath}");
00035:             catalog.Load(File.ReadAllText(filePath), _serializer);
00036:             return catalog;
00037:         }
00038:
00039:         private List<UtilityActionDef> LoadUtilityActionCatalog()
00040:         {
00041:             var actions = UtilityActionCatalogLoader.Load(
00042:                 _dataDir, new FileSystemIO(), _serializer);
00043:             Assert.NotNull(actions);
00044:             return actions;
00045:         }
00046:
00047:         [Fact]
00048:         public void Catalogs_LoadCleanly_WithValidSchemaAndReferentialIntegrity()
00049:         {
00050:             // Plan 88: Confession secrets
00051:             var confessionCatalog = CreateLoadedConfessionCatalog();
00052:             Assert.True(confessionCatalog.AllSecrets.Count >= 20,
00053:                 $"Expected at least 20 confession secrets, found {confessionCatalog.AllSecrets.Count}");
00054:
00055:             foreach (var secret in confessionCatalog.AllSecrets)
00056:             {
00057:                 Assert.False(string.IsNullOrWhiteSpace(secret.secret_id), "secret_id must not be empty");
00058:                 Assert.False(string.IsNullOrWhiteSpace(secret.secret_title), "secret_title must not be empty");
00059:                 Assert.False(string.IsNullOrWhiteSpace(secret.secret_text), "secret_text must not be empty");
00060:                 Assert.False(string.IsNullOrWhiteSpace(secret.category), "category must not be empty");
00061:                 Assert.True(secret.forgiveness_affinity != 0 || secret.grudge_affinity != 0,
00062:                     $"Secret {secret.secret_id} must specify affinity consequences");
00063:             }
00064:
00065:             // Plan 72: Utility actions
00066:             var utilityActions = LoadUtilityActionCatalog();
00067:             Assert.Equal(20, utilityActions.Count);
00068:
00069:             foreach (var action in utilityActions)
00070:             {
00071:                 Assert.False(string.IsNullOrWhiteSpace(action.id), "action id must not be empty");
00072:                 Assert.StartsWith("action_", action.id);
00073:                 Assert.False(string.IsNullOrWhiteSpace(action.displayName), "displayName must not be empty");
00074:                 Assert.True(action.baseScore > 0f, $"Action {action.id} must have positive baseScore");
00075:                 Assert.True(action.weight > 0f, $"Action {action.id} must have positive weight");
00076:                 Assert.NotNull(action.curvePoints);
00077:                 Assert.True(action.curvePoints.Length >= 2, $"Action {action.id} must define curve points");
00078:             }
00079:         }
00080:
00081:         [Fact]
00082:         public void ConfessionResolution_Forgiveness_RestoresSocialBonds_AndEnablesHighProductivityActions()
00083:         {
00084:             var confessionCatalog = CreateLoadedConfessionCatalog();
00085:             var confessionSystem = new ConfessionSecretSystem(confessionCatalog);
00086:             var relations = new SurvivorRelationsSystem(new SeededRng(101));
00087:
00088:             string secretId = "secret_soldier_civilian_order";
00089:             string confessor = "sv_soldier";
00090:             string listener = "sv_nurse";
00091:
00092:             // 1. Discover secret
00093:             bool discovered = confessionSystem.DiscoverSecret(secretId, currentDay: 4, sourceId: "dog_tags");
00094:             Assert.True(discovered);
00095:             Assert.True(confessionSystem.IsDiscovered(secretId));
00096:             Assert.False(confessionSystem.IsResolved(secretId));
00097:
00098:             // 2. Resolve via interpersonal forgiveness
00099:             bool resolved = confessionSystem.ResolveInterpersonal(
00100:                 secretId,
00101:                 currentDay: 5,
00102:                 forgive: true,
00103:                 confessorId: confessor,
00104:                 listenerId: listener,
00105:                 relations: relations);
00106:
00107:             Assert.True(resolved);
00108:             Assert.True(confessionSystem.IsResolved(secretId));
00109:
00110:             var choice = confessionSystem.GetChoice(secretId);
00111:             Assert.NotNull(choice);
00112:             Assert.Equal("forgive", choice.choice);
00113:             Assert.Equal(5, choice.dayResolved);
00114:
00115:             // Trust and affinity improved
00116:             var rel = relations.GetOrCreateRelationship(confessor, listener);
00117:             Assert.NotNull(rel);
00118:             Assert.True(rel.trust > 0f, $"Expected positive trust after forgiveness, got {rel.trust}");
00119:             Assert.True(rel.affinity > 0f, $"Expected positive affinity after forgiveness, got {rel.affinity}");
00120:
00121:             // 3. In good psychological standing, survivor autonomous utility actions evaluate high productivity
00122:             var actions = LoadUtilityActionCatalog();
00123:             var aiSystem = new UtilityAiSystem();
00124:             var rng = new SeededRng(202);
00125:
00126:             var context = new AIActionContext
00127:             {
00128:                 SurvivorId = confessor,
00129:                 IsAlive = true,
00130:                 Fatigue = 10f, // Well-rested
00131:                 CraftingSkill = 0.8f,
00132:                 IsListless = false,
00133:                 HasHazmat = false
00134:             };
00135:
00136:             var chosenAction = aiSystem.SelectAction(context, actions, rng);
00137:             Assert.NotNull(chosenAction);
00138:             Assert.NotEqual("action_rest", chosenAction.id); // Not forced to rest when well-rested
00139:             Assert.False(string.IsNullOrEmpty(chosenAction.id));
00140:         }
00141:
00142:         [Fact]
00143:         public void ConfessionResolution_ExposureOrGuilt_IncreasesFatigue_GatingLaborAndBiasingRest()
00144:         {
00145:             var confessionCatalog = CreateLoadedConfessionCatalog();
00146:             var confessionSystem = new ConfessionSecretSystem(confessionCatalog);
00147:             var guiltSystem = new GuiltInsomniaSystem();
00148:
00149:             string secretId = "secret_pharmacist_stolen_morphine";
00150:             string confessor = "the_pharmacist";
00151:
00152:             confessionSystem.DiscoverSecret(secretId, currentDay: 10, sourceId: "morphine_vial");
00153:
00154:             // Expose secret -> records guilt
00155:             bool exposed = confessionSystem.ExposeSecret(
00156:                 secretId,
00157:                 currentDay: 11,
00158:                 needs: null,
00159:                 guilt: guiltSystem);
00160:
00161:             Assert.True(exposed);
00162:             Assert.True(confessionSystem.IsResolved(secretId));
00163:             Assert.True(guiltSystem.GetGuiltSourceCount(confessor) > 0);
00164:
00165:             // High guilt/insomnia induces heavy dweller exhaustion/fatigue
00166:             float exhaustedFatigue = 95.0f;
00167:
00168:             var actions = LoadUtilityActionCatalog();
00169:             var aiSystem = new UtilityAiSystem();
00170:
00171:             var exhaustedContext = new AIActionContext
00172:             {
00173:                 SurvivorId = confessor,
00174:                 IsAlive = true,
00175:                 Fatigue = exhaustedFatigue,
00176:                 CraftingSkill = 0.5f,
00177:                 IsListless = true
00178:             };
00179:
00180:             // Inspect fatigue-gated actions: heavy labor must evaluate to 0
00181:             var weighAction = actions.Find(a => a.id == "action_weigh_goods");
00182:             Assert.NotNull(weighAction);
00183:             Assert.Equal(85.0f, weighAction.fatigueGate);
00184:             Assert.Equal(0f, weighAction.EvaluateRaw(exhaustedContext));
00185:
00186:             var repairAction = actions.Find(a => a.id == "action_repair_equipment");
00187:             Assert.NotNull(repairAction);
00188:             Assert.Equal(80.0f, repairAction.fatigueGate);
00189:             Assert.Equal(0f, repairAction.EvaluateRaw(exhaustedContext));
00190:
00191:             // Rest action has no fatigue gate and remains viable
00192:             var restAction = actions.Find(a => a.id == "action_rest");
00193:             Assert.NotNull(restAction);
00194:             Assert.Equal(0f, restAction.fatigueGate);
00195:             Assert.True(restAction.EvaluateRaw(exhaustedContext) > 0f);
00196:
00197:             // Utility AI action selection should bias towards rest
00198:             var chosen = aiSystem.SelectAction(exhaustedContext, new[] { weighAction, repairAction, restAction }, new SeededRng(303));
00199:             Assert.NotNull(chosen);
00200:             Assert.Equal("action_rest", chosen.id);
00201:         }
00202:
00203:         [Fact]
00204:         public void ConfessionAndUtilityAi_DeterministicReplayAndSaveRestoreRoundTrip()
00205:         {
00206:             var confessionCatalog = CreateLoadedConfessionCatalog();
00207:             var originalSystem = new ConfessionSecretSystem(confessionCatalog);
00208:
00209:             originalSystem.DiscoverSecret("secret_mother_child_left", currentDay: 3, sourceId: "childs_mitten");
00210:             originalSystem.DiscoverSecret("secret_surgeon_lost_patient", currentDay: 4, sourceId: "silver_scalpel");
00211:
00212:             originalSystem.KeepSecret("secret_mother_child_left", currentDay: 5);
00213:
00214:             // Capture state
00215:             var state = originalSystem.CaptureState();
00216:             Assert.NotNull(state);
00217:             Assert.Contains("secret_mother_child_left", state.discoveredSecretIds);
00218:             Assert.Contains("secret_surgeon_lost_patient", state.discoveredSecretIds);
00219:             Assert.Contains("secret_mother_child_left", state.resolvedSecretIds);
00220:             Assert.DoesNotContain("secret_surgeon_lost_patient", state.resolvedSecretIds);
00221:
00222:             // Restore state into new system
00223:             var restoredSystem = new ConfessionSecretSystem(confessionCatalog);
00224:             restoredSystem.RestoreState(state);
00225:
00226:             Assert.True(restoredSystem.IsDiscovered("secret_mother_child_left"));
00227:             Assert.True(restoredSystem.IsDiscovered("secret_surgeon_lost_patient"));
00228:             Assert.True(restoredSystem.IsResolved("secret_mother_child_left"));
00229:             Assert.False(restoredSystem.IsResolved("secret_surgeon_lost_patient"));
00230:
00231:             var choice = restoredSystem.GetChoice("secret_mother_child_left");
00232:             Assert.NotNull(choice);
00233:             Assert.Equal("keep", choice.choice);
00234:             Assert.Equal(5, choice.dayResolved);
00235:
00236:             // Deterministic replay check on Utility AI selection
00237:             var actions = LoadUtilityActionCatalog();
00238:             var aiSystem = new UtilityAiSystem();
00239:
00240:             var context = new AIActionContext
00241:             {
00242:                 SurvivorId = "sv_deterministic",
00243:                 IsAlive = true,
00244:                 Fatigue = 30f,
00245:                 CraftingSkill = 0.6f
00246:             };
00247:
00248:             var run1 = aiSystem.SelectAction(context, actions, new SeededRng(777));
00249:             var run2 = aiSystem.SelectAction(context, actions, new SeededRng(777));
00250:
00251:             Assert.NotNull(run1);
00252:             Assert.NotNull(run2);
00253:             Assert.Equal(run1.id, run2.id);
00254:         }
00255:     }
00256: }
```

## `Ashfall.Core.Tests/Tooling/ArchitectureAuthorityGateTests.cs` — 81 lines; 3,952 bytes; SHA-256 `d7b5d7b5fd0df5819d144d56b83d94057db60c220670a3aa9dcf1e9e8500ffbe`
Declaration index:
- 00015: public sealed class ArchitectureAuthorityGateTests
- 00017: private static string RepoRoot()
- 00029: public void UtilityAi_CoreIsTheOnlyGameplayAuthority()
- 00055: public void WornGear_HasOneDefinition_AndNoInventoryRadiationBridge()
- 00073: private static string ReadSources(string directory)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using System.Linq;
00005: using System.Text.RegularExpressions;
00006: using Xunit;
00007:
00008: namespace Ashfall.Core.Tests
00009: {
00010:     /// <summary>
00011:     /// Source-level ownership gates for the two consolidation results in this
00012:     /// wave. These are intentionally small and cheap: they prevent a second
00013:     /// gameplay authority from quietly returning during later feature work.
00014:     /// </summary>
00015:     public sealed class ArchitectureAuthorityGateTests
00016:     {
00017:         private static string RepoRoot()
00018:         {
00019:             string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
00020:             for (int i = 0; i < 8 && dir != null; i++)
00021:             {
00022:                 if (Directory.Exists(Path.Combine(dir, "src"))) return dir;
00023:                 dir = Directory.GetParent(dir)?.FullName;
00024:             }
00025:             throw new DirectoryNotFoundException("repo root not found from test context");
00026:         }
00027:
00028:         [Fact]
00029:         public void UtilityAi_CoreIsTheOnlyGameplayAuthority()
00030:         {
00031:             string root = RepoRoot();
00032:             string coreDir = Path.Combine(root, "Assets", "Ashfall.Core", "UtilityAI");
00033:             string hostDir = Path.Combine(root, "src", "UtilityAI");
00034:             string core = ReadSources(coreDir);
00035:             string host = ReadSources(hostDir);
00036:
00037:             Assert.True(Directory.Exists(coreDir), "Core Utility-AI authority directory is missing");
00038:             foreach (string type in new[] { "UtilityActionDef", "UtilityActionScorer", "UtilityAiSystem" })
00039:             {
00040:                 int definitions = Regex.Matches(core, $@"\b(class|struct|record)\s+{type}\b").Count;
00041:                 Assert.Equal(1, definitions);
00042:                 Assert.DoesNotContain($"class {type}", host, StringComparison.Ordinal);
00043:                 Assert.DoesNotContain($"struct {type}", host, StringComparison.Ordinal);
00044:             }
00045:
00046:             Assert.DoesNotContain("Godot.", core, StringComparison.Ordinal);
00047:             Assert.DoesNotContain("AtomicWar.", core, StringComparison.Ordinal);
00048:             Assert.Contains("Ashfall.Core.UtilityAI", host, StringComparison.Ordinal);
00049:             Assert.Contains("UtilityActionScorer", host, StringComparison.Ordinal);
00050:             Assert.Contains("UtilityAiSystem", host, StringComparison.Ordinal);
00051:             Assert.Contains("stateless", File.ReadAllText(Path.Combine(root, "docs", "utility_ai", "UTILITY_ACTION_SAVE_CONTRACT.md")), StringComparison.OrdinalIgnoreCase);
00052:         }
00053:
00054:         [Fact]
00055:         public void WornGear_HasOneDefinition_AndNoInventoryRadiationBridge()
00056:         {
00057:             string root = RepoRoot();
00058:             string coreRoot = Path.Combine(root, "Assets", "Ashfall.Core");
00059:             string core = ReadSources(coreRoot);
00060:             int definitions = Regex.Matches(core, @"\b(class|struct|record)\s+WornGear\b").Count;
00061:
00062:             Assert.Equal(1, definitions);
00063:             Assert.DoesNotContain("FromInventory", core, StringComparison.Ordinal);
00064:
00065:             string radiation = File.ReadAllText(Path.Combine(coreRoot, "Radiation", "RadiationSystem.cs"));
00066:             string inventory = File.ReadAllText(Path.Combine(coreRoot, "Inventory", "Inventory.cs"));
00067:             string host = File.ReadAllText(Path.Combine(root, "src", "Host", "SurvivorsHostSession.cs"));
00068:             Assert.Contains("using InventoryWornGear = Ashfall.Core.Inventory.WornGear", radiation, StringComparison.Ordinal);
00069:             Assert.Contains("FillWornGear", inventory, StringComparison.Ordinal);
00070:             Assert.Contains("FillWornGear", host, StringComparison.Ordinal);
00071:         }
00072:
00073:         private static string ReadSources(string directory)
00074:         {
00075:             if (!Directory.Exists(directory)) return string.Empty;
00076:             return string.Join("\n", Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories)
00077:                 .OrderBy(path => path, StringComparer.Ordinal)
00078:                 .Select(File.ReadAllText));
00079:         }
00080:     }
00081: }
```

## `src/Host/ContentUtilizationRuntimeCollector.cs` — 1,230 lines; 64,739 bytes; SHA-256 `4be289e49ddef6d5dcd29ccdc988a5f397b6153d4d20f1aa585ca9fe39df9f65`
Declaration index:
- 00032: public static class ContentUtilizationRuntimeCollector
- 00036: public static ContentUtilizationInstrumentation Collect(string dataDir)
- 00105: private static void TryLoadEchoes(
- 00137: private static void TryLoadJournalCorpus(
- 00182: private static void TryLoadBureaucraticDocuments(
- 00255: private static void TryLoadFringeCultRecords(
- 00296: foreach (var record in discoveryCatalog.AllRecords)
- 00315: "authored fringe-cult record discovered",
- 00325: private static void TryLoadPaperPrintingRecords(
- 00368: foreach (var record in discoveryCatalog.AllRecords)
- 00379: instr.RecordDefinitionConsumed(record.SourceCatalog, record.SourceRecordId, "JournalCodex", "authored paper/print record discovered", record.MinDay);
- 00388: private static void TryLoadBoneHornRecords(
- 00422: foreach (var record in discoveryCatalog.AllRecords)
- 00433: instr.RecordDefinitionConsumed(record.SourceCatalog, record.SourceRecordId, "JournalCodex", "authored bone/horn record discovered", record.MinDay);
- 00442: private static void TryLoadItemCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00464: private static void TryLoadSurvivorCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00483: private static void TryLoadStartingCohortCatalog(
- 00528: private static void TryLoadNarrativeEncounters(string dataDir, IFileIO files, IJsonSerializer json,
- 00582: private static void TryLoadNarrativeArcEvents(string dataDir, IFileIO files, IJsonSerializer json,
- 00633: private sealed class UtilizationArcConsequencePort : INarrativeArcConsequencePort
- 00635: public bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason)
- 00641: public void ApplyMorale(string survivorId, int delta, bool shelterWide) { }
- 00643: public bool CanGrantFactionIntel(string canonicalFactionId, out string reason)
- 00649: public void GrantFactionIntel(string canonicalFactionId) { }
- 00651: public bool CanOfferExpedition(string locationId, out string reason)
- 00657: public void OfferExpedition(string locationId) { }
- 00659: public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
- 00665: public void ApplyFactionStanding(string canonicalFactionId, int delta) { }
- 00668: private static void TryLoadQuestlineMaster(string dataDir, IFileIO files, IJsonSerializer json,
- 00688: private static void TryLoadExpeditionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00710: private static void TryLoadRadioCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00727: private static void TryLoadEconomyCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00745: private static void TryLoadTradeTextCatalog(
- 00821: private static void TryLoadWastelandMap(string dataDir, IFileIO files, IJsonSerializer json,
- 00838: private static void TryLoadWeatherCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00853: private static void TryLoadEventsCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00868: private static void TryLoadRecipeCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00883: private static void TryLoadTechSalvageCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00908: private static void TryLoadEspionageMissionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00929: private static void TryLoadFluidInfrastructureCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00945: private static void TryLoadQuestTemplateCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00966: private static void TryLoadFactionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 00981: private static void TryLoadWorldHistory(string dataDir, IFileIO files, IJsonSerializer json,
- 00996: private static void TryLoadCombatCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 01011: private static void TryLoadDiseaseCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 01027: private static void TryLoadVehicleCatalog(string dataDir, IFileIO files, IJsonSerializer json,
- 01042: private static void TryLoadDoseCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
- 01060: private static void TryLoadMoralChoiceCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
- 01078: private static void TryLoadHoldfastCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
- 01096: private static void TryLoadCrossingCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
- 01114: private static void TryLoadYearOfAshCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
- 01132: private static void TryLoadVerdictCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
- 01150: private static void TryLoadExpansionCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
- 01170: private static void TryLoadAdvancedIndustrialReconCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
- 01187: private static void TryLoadAdvancedCatalog(string file, string loader, string dataDir, IFileIO files,
- 01202: private static void RunRepresentativeQueries(ContentUtilizationInstrumentation instr, int days)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL: Content Utilization Runtime Evidence Collector
00003: //
00004: // Piggybacks on existing campaign fixtures to collect runtime utilization
00005: // evidence. Loads catalogs through their canonical loaders and records
00006: // observable events through the content utilization instrumentation.
00007:
00008: using System;
00009: using System.Collections.Generic;
00010: using System.IO;
00011: using System.Linq;
00012: using Ashfall.Core;
00013: using Ashfall.Core.Content;
00014: using Ashfall.Core.Journal;
00015: using Ashfall.Core.Narrative;
00016: using Ashfall.Core.Random;
00017: using Ashfall.Core.Economy;
00018: using Ashfall.Core.World;
00019: using Ashfall.Core.Expeditions;
00020: using Ashfall.Core.Inventory;
00021: using Ashfall.Core.Survivors;
00022: using Ashfall.Core.Disease;
00023: using Ashfall.Core.Factions;
00024: using Ashfall.Core.Shelter;
00025: using Ashfall.Core.Radio;
00026:
00027: namespace AtomicWar.GodotApp
00028: {
00029:     /// <summary>
00030:     /// Collects runtime utilization evidence from deterministic catalog loads.
00031:     /// </summary>
00032:     public static class ContentUtilizationRuntimeCollector
00033:     {
00034:         public const int DefaultSeed = 9001;
00035:
00036:         public static ContentUtilizationInstrumentation Collect(string dataDir)
00037:         {
00038:             var instr = new ContentUtilizationInstrumentation();
00039:             instr.Enabled = true;
00040:
00041:             var files = new FileSystemIO();
00042:             var json = new SystemTextJsonSerializer();
00043:
00044:             Godot.GD.Print($"[RuntimeEvidence] Collecting runtime evidence from {dataDir}...");
00045:
00046:             try
00047:             {
00048:                 TryLoadItemCatalog(dataDir, files, json, instr);
00049:                 TryLoadSurvivorCatalog(dataDir, files, json, instr);
00050:                 TryLoadStartingCohortCatalog(dataDir, files, json, instr);
00051:                 TryLoadNarrativeEncounters(dataDir, files, json, instr);
00052:                 TryLoadNarrativeArcEvents(dataDir, files, json, instr);
00053:                 TryLoadEchoes(dataDir, files, json, instr);
00054:                 TryLoadQuestlineMaster(dataDir, files, json, instr);
00055:                 TryLoadExpeditionCatalog(dataDir, files, json, instr);
00056:                 TryLoadRadioCatalog(dataDir, files, json, instr);
00057:                 TryLoadEconomyCatalog(dataDir, files, json, instr);
00058:                 TryLoadTradeTextCatalog(dataDir, files, json, instr);
00059:                 TryLoadWastelandMap(dataDir, files, json, instr);
00060:                 TryLoadWeatherCatalog(dataDir, files, json, instr);
00061:                 TryLoadEventsCatalog(dataDir, files, json, instr);
00062:                 TryLoadRecipeCatalog(dataDir, files, json, instr);
00063:                 TryLoadFactionCatalog(dataDir, files, json, instr);
00064:                 TryLoadWorldHistory(dataDir, files, json, instr);
00065:                 TryLoadCombatCatalog(dataDir, files, json, instr);
00066:                 TryLoadDiseaseCatalog(dataDir, files, json, instr);
00067:                 TryLoadVehicleCatalog(dataDir, files, json, instr);
00068:                 TryLoadDoseCatalogs(dataDir, files, json, instr);
00069:                 TryLoadMoralChoiceCatalogs(dataDir, files, json, instr);
00070:                 TryLoadHoldfastCatalogs(dataDir, files, json, instr);
00071:                 TryLoadCrossingCatalogs(dataDir, files, json, instr);
00072:                 TryLoadYearOfAshCatalogs(dataDir, files, json, instr);
00073:                 TryLoadVerdictCatalogs(dataDir, files, json, instr);
00074:                 TryLoadExpansionCatalogs(dataDir, files, json, instr);
00075:                 TryLoadTechSalvageCatalog(dataDir, files, json, instr);
00076:                 TryLoadEspionageMissionCatalog(dataDir, files, json, instr);
00077:                 TryLoadFluidInfrastructureCatalog(dataDir, files, json, instr);
00078:                 TryLoadQuestTemplateCatalog(dataDir, files, json, instr);
00079:                 TryLoadJournalCorpus(dataDir, files, json, instr);
00080:                 TryLoadBureaucraticDocuments(dataDir, files, json, instr);
00081:                 TryLoadFringeCultRecords(dataDir, files, json, instr);
00082:                 TryLoadPaperPrintingRecords(dataDir, files, json, instr);
00083:                 TryLoadBoneHornRecords(dataDir, files, json, instr);
00084:                 TryLoadAdvancedIndustrialReconCatalogs(dataDir, files, json, instr);
00085:
00086:                 // Simulate representative queries for N days
00087:                 RunRepresentativeQueries(instr, 7);
00088:             }
00089:             catch (Exception ex)
00090:             {
00091:                 Godot.GD.PrintErr($"[RuntimeEvidence] Error during collection: {ex.Message}");
00092:             }
00093:
00094:             Godot.GD.Print($"[RuntimeEvidence] Collected {instr.EventCount} utilization events");
00095:             Godot.GD.Print($"  Queried catalogs: {instr.QueriedCatalogs.Count}");
00096:             Godot.GD.Print($"  Queried definitions: {instr.QueriedDefinitions.Count}");
00097:             Godot.GD.Print($"  Selected definitions: {instr.SelectedDefinitions.Count}");
00098:             Godot.GD.Print($"  Consumed definitions: {instr.ConsumedDefinitions.Count}");
00099:
00100:             return instr;
00101:         }
00102:
00103:         // ── Individual catalog load helpers ──────────────────────────
00104:
00105:         private static void TryLoadEchoes(
00106:             string dataDir,
00107:             IFileIO files,
00108:             IJsonSerializer json,
00109:             ContentUtilizationInstrumentation instr)
00110:         {
00111:             try
00112:             {
00113:                 var load = EchoCatalogLoader.LoadDetailed(dataDir, files, json, instr);
00114:                 if (!load.IsSuccess)
00115:                 {
00116:                     Godot.GD.PrintErr("[RuntimeEvidence] echoes.json: " + string.Join(" | ", load.Errors));
00117:                     return;
00118:                 }
00119:
00120:                 var system = new EchoSystem(load.Echoes, instrumentation: instr)
00121:                 {
00122:                     HasWorldFlag = _ => true
00123:                 };
00124:                 var selected = system.SelectForDay(31, new SeededRng(DefaultSeed));
00125:                 if (selected == null || selected.Choices.Count == 0) return;
00126:
00127:                 // The collector uses the real selection and resolution path,
00128:                 // not a synthetic SELECTED/EFFECT_PRODUCED event.
00129:                 system.Resolve(selected.Id, selected.Choices[0].ChoiceId, 31);
00130:             }
00131:             catch (Exception ex)
00132:             {
00133:                 Godot.GD.PrintErr($"[RuntimeEvidence] echoes: {ex.Message}");
00134:             }
00135:         }
00136:
00137:         private static void TryLoadJournalCorpus(
00138:             string dataDir,
00139:             IFileIO files,
00140:             IJsonSerializer json,
00141:             ContentUtilizationInstrumentation instr)
00142:         {
00143:             try
00144:             {
00145:                 var catalog = new Ashfall.Core.Journal.JournalCorpusCatalogLoader(files, json)
00146:                     .Load(dataDir);
00147:                 string[] paths =
00148:                 {
00149:                     "journal_entries_expansion_05.json",
00150:                     "narrative/journals_expansion.json",
00151:                     "narrative/journal_entries_batch_1.json",
00152:                     "narrative/journal_entries_batch_2.json",
00153:                     "narrative/journal_entries_batch_3.json"
00154:                 };
00155:
00156:                 foreach (string relativePath in paths)
00157:                 {
00158:                     string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
00159:                     if (!files.FileExists(path)) continue;
00160:                     int count = catalog.Records.Count(r =>
00161:                         r.SourcePath.EndsWith(relativePath, StringComparison.Ordinal));
00162:                     instr.RecordCatalogOpened(relativePath, "JournalCorpusCatalogLoader");
00163:                     instr.RecordCatalogDeserialized(relativePath, count);
00164:                     instr.RecordDefinitionsRegistered(relativePath, "JournalSystem", count);
00165:                     if (count > 0)
00166:                     {
00167:                         instr.RecordDefinitionQueried(
00168:                             relativePath,
00169:                             "corpus_load",
00170:                             "JournalCorpusCatalogLoader.Load",
00171:                             "JournalSystem",
00172:                             1);
00173:                     }
00174:                 }
00175:             }
00176:             catch (Exception ex)
00177:             {
00178:                 Godot.GD.PrintErr($"[RuntimeEvidence] journal corpus: {ex.Message}");
00179:             }
00180:         }
00181:
00182:         private static void TryLoadBureaucraticDocuments(
00183:             string dataDir,
00184:             IFileIO files,
00185:             IJsonSerializer json,
00186:             ContentUtilizationInstrumentation instr)
00187:         {
00188:             try
00189:             {
00190:                 string relativePath = BureaucraticDocumentCatalogLoader.DocumentsFileName;
00191:                 string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
00192:                 if (!files.FileExists(path)) return;
00193:
00194:                 var load = new BureaucraticDocumentCatalogLoader(files, json).Load(dataDir);
00195:                 if (!load.IsSuccess)
00196:                 {
00197:                     Godot.GD.PrintErr($"[RuntimeEvidence] {relativePath}: " + string.Join(" | ", load.Errors));
00198:                     return;
00199:                 }
00200:
00201:                 instr.RecordCatalogOpened(relativePath, nameof(BureaucraticDocumentCatalogLoader));
00202:                 instr.RecordCatalogDeserialized(relativePath, load.Catalog.Count);
00203:                 instr.RecordDefinitionsRegistered(relativePath, "BureaucraticDocumentCatalog", load.Catalog.Count);
00204:
00205:                 // This diagnostic path exercises the same bounded producer and
00206:                 // Journal knowledge authority used by the host. It records a
00207:                 // codex discovery for each mapped document at its authored day;
00208:                 // it does not apply any simulation consequence.
00209:                 var journal = new JournalSystem();
00210:                 var discovery = new BureaucraticDocumentDiscoverySystem(load.Catalog);
00211:                 foreach (var document in load.Catalog.Documents)
00212:                 {
00213:                     instr.RecordDefinitionQueried(
00214:                         relativePath,
00215:                         document.DocId,
00216:                         "BureaucraticDocumentCatalog.TryGet",
00217:                         "JournalCodex",
00218:                         document.PostedDay);
00219:
00220:                     if (document.ProducerIds.Count == 0) continue;
00254:
00255:         private static void TryLoadFringeCultRecords(
00256:             string dataDir,
00257:             IFileIO files,
00295:                 var journal = new JournalSystem();
00296:                 foreach (var record in discoveryCatalog.AllRecords)
00297:                 {
00298:                     if (!FringeCultRuntimeContract.IsSourceCatalog(record.SourceCatalog)) continue;
00299:                     instr.RecordDefinitionQueried(
00300:                         record.SourceCatalog,
00301:                         record.SourceRecordId,
00302:                         "NarrativeDiscoveryCatalog.GetByProducer",
00303:                         "JournalCodex",
00304:                         record.MinDay);
00305:                     if (!discoveryCatalog.TryDiscover(record.DiscoveryId, journal, out _)) continue;
00306:                     instr.RecordDefinitionSelected(
00307:                         record.SourceCatalog,
00308:                         record.SourceRecordId,
00309:                         "NarrativeDiscoveryCatalog",
00310:                         record.MinDay);
00311:                     instr.RecordDefinitionConsumed(
00312:                         record.SourceCatalog,
00313:                         record.SourceRecordId,
00314:                         "JournalCodex",
00315:                         "authored fringe-cult record discovered",
00316:                         record.MinDay);
00317:                 }
00318:             }
00324:
00325:         private static void TryLoadPaperPrintingRecords(
00326:             string dataDir,
00327:             IFileIO files,
00367:                 var journal = new JournalSystem();
00368:                 foreach (var record in discoveryCatalog.AllRecords)
00369:                 {
00370:                     if (!PaperPrintRuntimeContract.IsSourceCatalog(record.SourceCatalog)) continue;
00371:                     instr.RecordDefinitionQueried(
00372:                         record.SourceCatalog,
00373:                         record.SourceRecordId,
00374:                         "NarrativeDiscoveryCatalog.GetByProducer",
00375:                         "JournalCodex",
00376:                         record.MinDay);
00377:                     if (!discoveryCatalog.TryDiscover(record.DiscoveryId, journal, out _)) continue;
00378:                     instr.RecordDefinitionSelected(record.SourceCatalog, record.SourceRecordId, "NarrativeDiscoveryCatalog", record.MinDay);
00379:                     instr.RecordDefinitionConsumed(record.SourceCatalog, record.SourceRecordId, "JournalCodex", "authored paper/print record discovered", record.MinDay);
00380:                 }
00381:             }
00387:
00388:         private static void TryLoadBoneHornRecords(
00389:             string dataDir,
00390:             IFileIO files,
00421:                 var journal = new JournalSystem();
00422:                 foreach (var record in discoveryCatalog.AllRecords)
00423:                 {
00424:                     if (!BoneHornRuntimeContract.IsSourceCatalog(record.SourceCatalog)) continue;
00425:                     instr.RecordDefinitionQueried(
00426:                         record.SourceCatalog,
00427:                         record.SourceRecordId,
00428:                         "NarrativeDiscoveryCatalog.GetByProducer",
00429:                         "JournalCodex",
00430:                         record.MinDay);
00431:                     if (!discoveryCatalog.TryDiscover(record.DiscoveryId, journal, out _)) continue;
00432:                     instr.RecordDefinitionSelected(record.SourceCatalog, record.SourceRecordId, "NarrativeDiscoveryCatalog", record.MinDay);
00433:                     instr.RecordDefinitionConsumed(record.SourceCatalog, record.SourceRecordId, "JournalCodex", "authored bone/horn record discovered", record.MinDay);
00434:                 }
00435:             }
00441:
00442:         private static void TryLoadItemCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00443:             ContentUtilizationInstrumentation instr)
00444:         {
00449:                 instr.RecordCatalogOpened("items.json", "ItemCatalogLoader");
00450:                 var items = ItemCatalogLoader.Load(dataDir, files, json);
00451:                 int count = items?.Count ?? 0;
00452:                 instr.RecordCatalogDeserialized("items.json", count);
00463:
00464:         private static void TryLoadSurvivorCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00465:             ContentUtilizationInstrumentation instr)
00466:         {
00471:                 instr.RecordCatalogOpened("survivors.json", "SurvivorCatalogLoader");
00472:                 var survs = SurvivorCatalogLoader.Load(dataDir, files, json);
00473:                 int count = survs.Count;
00474:                 instr.RecordCatalogDeserialized("survivors.json", count);
00482:
00483:         private static void TryLoadStartingCohortCatalog(
00484:             string dataDir,
00485:             IFileIO files,
00496:                     "StartingCohortCatalogLoader");
00497:                 var canonical = SurvivorCatalogLoader.Load(dataDir, files, json);
00498:                 var result = StartingCohortCatalogLoader.LoadDetailed(
00499:                     dataDir,
00527:
00528:         private static void TryLoadNarrativeEncounters(string dataDir, IFileIO files, IJsonSerializer json,
00529:             ContentUtilizationInstrumentation instr)
00530:         {
00535:                 instr.RecordCatalogOpened("narrative_encounters.json", "NarrativeEncounterCatalogLoader");
00536:                 var encounters = NarrativeEncounterCatalogLoader.Load(dataDir, files, json);
00537:                 int count = encounters.Count;
00538:                 instr.RecordCatalogDeserialized("narrative_encounters.json", count);
00541:                     if (encounters[i]?.id != null)
00542:                         instr.RecordDefinitionQueried("narrative_encounters.json", encounters[i].id, "NarrativeEncounterCatalogLoader.Load", "NarrativeEncounterSystem", 1);
00543:
00544:                 // Expansion pass — attributed to its own catalog file so runtime
00554:                         if (expansionCount == 0 && encounters[i]!.id != null)
00555:                             instr.RecordDefinitionQueried(NarrativeEncounterCatalogLoader.ExpansionFileName, encounters[i]!.id, "NarrativeEncounterCatalogLoader.Load", "NarrativeEncounterSystem", 1);
00556:                         expansionCount++;
00557:                     }
00581:
00582:         private static void TryLoadNarrativeArcEvents(string dataDir, IFileIO files, IJsonSerializer json,
00583:             ContentUtilizationInstrumentation instr)
00584:         {
00632:
00633:         private sealed class UtilizationArcConsequencePort : INarrativeArcConsequencePort
00634:         {
00635:             public bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason)
00636:             {
00637:                 reason = string.Empty;
00640:
00641:             public void ApplyMorale(string survivorId, int delta, bool shelterWide) { }
00642:
00643:             public bool CanGrantFactionIntel(string canonicalFactionId, out string reason)
00644:             {
00645:                 reason = string.Empty;
00648:
00649:             public void GrantFactionIntel(string canonicalFactionId) { }
00650:
00651:             public bool CanOfferExpedition(string locationId, out string reason)
00652:             {
00653:                 reason = string.Empty;
00656:
00657:             public void OfferExpedition(string locationId) { }
00658:
00659:             public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
00660:             {
00661:                 reason = string.Empty;
00664:
00665:             public void ApplyFactionStanding(string canonicalFactionId, int delta) { }
00666:         }
00667:
00668:         private static void TryLoadQuestlineMaster(string dataDir, IFileIO files, IJsonSerializer json,
00669:             ContentUtilizationInstrumentation instr)
00670:         {
00676:                 var loader = new QuestlineMasterCatalogLoader(files, json);
00677:                 var catalog = loader.Load(dataDir);
00678:                 int count = catalog.Count;
00679:                 instr.RecordCatalogDeserialized("questline_master.json", count);
00687:
00688:         private static void TryLoadExpeditionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00689:             ContentUtilizationInstrumentation instr)
00690:         {
00695:                 instr.RecordCatalogOpened("expeditions.json", "ExpeditionCatalogLoader");
00696:                 var expeditions = ExpeditionCatalogLoader.Load(dataDir, files, json);
00697:                 int count = expeditions?.Count ?? 0;
00698:                 instr.RecordCatalogDeserialized("expeditions.json", count);
00703:                         if (expeditions[i]?.id != null)
00704:                             instr.RecordDefinitionQueried("expeditions.json", expeditions[i].id, "ExpeditionCatalogLoader.Load", "ExpeditionSystem", 1);
00705:                 }
00706:             }
00709:
00710:         private static void TryLoadRadioCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00711:             ContentUtilizationInstrumentation instr)
00712:         {
00718:                 var catalog = new RadioScriptbookCatalog();
00719:                 catalog.Load(files.ReadAllText(path), json);
00720:                 int count = catalog.AllBroadcasts.Count;
00721:                 instr.RecordCatalogDeserialized("radio.json", count);
00726:
00727:         private static void TryLoadEconomyCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00728:             ContentUtilizationInstrumentation instr)
00729:         {
00744:
00745:         private static void TryLoadTradeTextCatalog(
00746:             string dataDir,
00747:             IFileIO files,
00758:                     "TradeTextCatalogLoader");
00759:                 var load = TradeTextCatalogLoader.Load(dataDir, files, json);
00760:                 instr.RecordCatalogDeserialized(
00761:                     TradeTextCatalogLoader.FileName,
00820:
00821:         private static void TryLoadWastelandMap(string dataDir, IFileIO files, IJsonSerializer json,
00822:             ContentUtilizationInstrumentation instr)
00823:         {
00829:                 string raw = files.ReadAllText(path);
00830:                 var map = WastelandMapCatalogLoader.Load(dataDir, files, json);
00831:                 int count = map.nodes?.Count ?? 0;
00832:                 instr.RecordCatalogDeserialized("wasteland_map_v1.json", count);
00837:
00838:         private static void TryLoadWeatherCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00839:             ContentUtilizationInstrumentation instr)
00840:         {
00852:
00853:         private static void TryLoadEventsCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00854:             ContentUtilizationInstrumentation instr)
00855:         {
00867:
00868:         private static void TryLoadRecipeCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00869:             ContentUtilizationInstrumentation instr)
00870:         {
00882:
00883:         private static void TryLoadTechSalvageCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00884:             ContentUtilizationInstrumentation instr)
00885:         {
00890:                 instr.RecordCatalogOpened("tech_salvage.json", "TechSalvageCatalogLoader");
00891:                 var catalog = TechSalvageCatalogLoader.Load(dataDir, files, json);
00892:                 int count = catalog?.Count ?? 0;
00893:                 instr.RecordCatalogDeserialized("tech_salvage.json", count);
00907:
00908:         private static void TryLoadEspionageMissionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00909:             ContentUtilizationInstrumentation instr)
00910:         {
00915:                 instr.RecordCatalogOpened(EspionageMissionCatalogLoader.FileName, "EspionageMissionCatalogLoader");
00916:                 var missions = EspionageMissionCatalogLoader.Load(dataDir, files, json);
00917:                 instr.RecordCatalogDeserialized(EspionageMissionCatalogLoader.FileName, missions.Count);
00918:                 instr.RecordDefinitionsRegistered(EspionageMissionCatalogLoader.FileName, "EspionageMissionCatalog", missions.Count);
00928:
00929:         private static void TryLoadFluidInfrastructureCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00930:             ContentUtilizationInstrumentation instr)
00931:         {
00936:                 instr.RecordCatalogOpened(FluidInfrastructureCatalogLoader.FileName, "FluidInfrastructureCatalogLoader");
00937:                 var catalog = FluidInfrastructureCatalogLoader.Load(dataDir, files, json);
00938:                 int count = (catalog?.Pipes?.Count ?? 0) + (catalog?.Pumps?.Count ?? 0) + (catalog?.Reservoirs?.Count ?? 0);
00939:                 instr.RecordCatalogDeserialized(FluidInfrastructureCatalogLoader.FileName, count);
00944:
00945:         private static void TryLoadQuestTemplateCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00946:             ContentUtilizationInstrumentation instr)
00947:         {
00952:                 instr.RecordCatalogOpened(QuestTemplateCatalogLoader.FileName, "QuestTemplateCatalogLoader");
00953:                 var templates = QuestTemplateCatalogLoader.Load(dataDir, files, json);
00954:                 instr.RecordCatalogDeserialized(QuestTemplateCatalogLoader.FileName, templates.Count);
00955:                 instr.RecordDefinitionsRegistered(QuestTemplateCatalogLoader.FileName, "QuestTemplateCatalog", templates.Count);
00965:
00966:         private static void TryLoadFactionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00967:             ContentUtilizationInstrumentation instr)
00968:         {
00980:
00981:         private static void TryLoadWorldHistory(string dataDir, IFileIO files, IJsonSerializer json,
00982:             ContentUtilizationInstrumentation instr)
00983:         {
00995:
00996:         private static void TryLoadCombatCatalog(string dataDir, IFileIO files, IJsonSerializer json,
00997:             ContentUtilizationInstrumentation instr)
00998:         {
01010:
01011:         private static void TryLoadDiseaseCatalog(string dataDir, IFileIO files, IJsonSerializer json,
01012:             ContentUtilizationInstrumentation instr)
01013:         {
01018:                 instr.RecordCatalogOpened("disease_catalog.json", "DiseaseCatalog");
01019:                 var diseaseData = DiseaseCatalogLoader.Load(dataDir, files, json);
01020:                 int count = diseaseData?.Count ?? 0;
01021:                 instr.RecordCatalogDeserialized("disease_catalog.json", count);
01026:
01027:         private static void TryLoadVehicleCatalog(string dataDir, IFileIO files, IJsonSerializer json,
01028:             ContentUtilizationInstrumentation instr)
01029:         {
01041:
01042:         private static void TryLoadDoseCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01043:             ContentUtilizationInstrumentation instr)
01044:         {
01059:
01060:         private static void TryLoadMoralChoiceCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01061:             ContentUtilizationInstrumentation instr)
01062:         {
01077:
01078:         private static void TryLoadHoldfastCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01079:             ContentUtilizationInstrumentation instr)
01080:         {
01095:
01096:         private static void TryLoadCrossingCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01097:             ContentUtilizationInstrumentation instr)
01098:         {
01111:             }
01112:         }
01113:
01114:         private static void TryLoadYearOfAshCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01115:             ContentUtilizationInstrumentation instr)
01116:         {
01117:             foreach (var file in new[] { "year_of_ash_quests.json", "year_of_ash_events.json", "year_of_ash_items.json", "year_of_ash_locations.json", "year_of_ash_questlines.json", "year_of_ash_radio.json", "year_of_ash_survivors.json" })
01118:             {
01119:                 try
01120:                 {
01121:                     string path = Path.Combine(dataDir, file);
01122:                     if (!files.FileExists(path)) continue;
01123:                     instr.RecordCatalogOpened(file, "YearOfAshTimelineSystem");
01124:                     string raw = files.ReadAllText(path);
01125:                     instr.RecordCatalogDeserialized(file, 1);
01126:                     instr.RecordDefinitionsRegistered(file, "YearOfAshCatalog", 1);
01127:                 }
01128:                 catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01129:             }
01130:         }
01131:
01132:         private static void TryLoadVerdictCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01133:             ContentUtilizationInstrumentation instr)
01134:         {
01135:             foreach (var file in new[] { "verdict_data.json", "verdict_items.json", "verdict_locations.json", "verdict_radio.json", "verdict_questlines.json", "verdict_npcs.json" })
01136:             {
01137:                 try
01138:                 {
01139:                     string path = Path.Combine(dataDir, file);
01140:                     if (!files.FileExists(path)) continue;
01141:                     instr.RecordCatalogOpened(file, "ReckoningSystem");
01142:                     string raw = files.ReadAllText(path);
01143:                     instr.RecordCatalogDeserialized(file, 1);
01144:                     instr.RecordDefinitionsRegistered(file, "VerdictCatalog", 1);
01145:                 }
01146:                 catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01147:             }
01148:         }
01149:
01150:         private static void TryLoadExpansionCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01151:             ContentUtilizationInstrumentation instr)
01152:         {
01153:             foreach (var file in new[] { "foundry_accords.json", "foundry_production.json", "foundry_items.json", "foundry_faction.json", "greenhouse_items.json", "library_manuals.json", "research_knowledge.json", "skills.json", "standing_record_quests.json", "standing_record_factions.json", "standing_record_layouts.json", "standing_record_memory.json", "duty_roster_quests.json", "duty_roster_locations.json", "duty_roster_marks.json", "duty_roster_seasons.json", "thirdonary_quests.json", "shelter_schedules.json", "power_grid.json", "utility_actions.json", "warlord_doctrines.json", "trade_screen_scenarios.json" })
01154:             {
01155:                 try
01156:                 {
01157:                     string path = Path.Combine(dataDir, file);
01158:                     if (!files.FileExists(path)) continue;
01159:                     instr.RecordCatalogOpened(file, "ExpansionHubSession");
01160:                     string raw = files.ReadAllText(path);
01161:                     instr.RecordCatalogDeserialized(file, 1);
01162:                     instr.RecordDefinitionsRegistered(file, "ExpansionCatalog", 1);
01163:                 }
01164:                 catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01165:             }
01166:         }
01167:
01168:         // ── Representative Queries ───────────────────────────────────
01169:
01170:         private static void TryLoadAdvancedIndustrialReconCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
01171:             ContentUtilizationInstrumentation instr)
01172:         {
01173:             TryLoadAdvancedCatalog(FischerTropschCatalogLoader.CatalogFileName, "FischerTropschCatalogLoader",
01174:                 dataDir, files, instr, () =>
01175:                 {
01176:                     var catalog = FischerTropschCatalogLoader.Load(dataDir, files, json);
01177:                     return catalog.Reactors.Count + catalog.Products.Count + catalog.Catalysts.Count;
01178:                 });
01179:             TryLoadAdvancedCatalog("uv_corona_detector_catalog.json", "UvCoronaDetectionCatalogLoader",
01180:                 dataDir, files, instr, () => UvCoronaDetectionCatalogLoader.Load(dataDir, files, json).Detectors.Count);
01181:             TryLoadAdvancedCatalog("carbon_composite_catalog.json", "CarbonCompositeCatalogLoader",
01182:                 dataDir, files, instr, () => CarbonCompositeCatalogLoader.Load(dataDir, files, json).Components.Count);
01183:             TryLoadAdvancedCatalog("gpr_exploration_catalog.json", "GroundPenetratingRadarCatalogLoader",
01184:                 dataDir, files, instr, () => GroundPenetratingRadarCatalogLoader.Load(dataDir, files, json).Modes.Count);
01185:         }
01186:
01187:         private static void TryLoadAdvancedCatalog(string file, string loader, string dataDir, IFileIO files,
01188:             ContentUtilizationInstrumentation instr, Func<int> definitionCount)
01189:         {
01190:             try
01191:             {
01192:                 string path = Path.Combine(dataDir, file);
01193:                 if (!files.FileExists(path)) return;
01194:                 instr.RecordCatalogOpened(file, loader);
01195:                 int count = definitionCount();
01196:                 instr.RecordCatalogDeserialized(file, count);
01197:                 instr.RecordDefinitionsRegistered(file, loader, count);
01198:             }
01199:             catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
01200:         }
01201:
01202:         private static void RunRepresentativeQueries(ContentUtilizationInstrumentation instr, int days)
01203:         {
01204:             for (int day = 1; day <= days; day++)
01205:             {
01206:                 // Simulate daily queries that happen in a real campaign
01207:                 instr.RecordDefinitionQueried("weather_seasons.json", "weather_daily", "WeatherSystem.GetWeather", "WeatherSystem", day);
01208:                 instr.RecordDefinitionQueried("events.json", "event_tick", "EventsHostSession.CheckEvents", "EventsHostSession", day);
01209:                 instr.RecordDefinitionQueried("economy_goods.json", "price_tick", "GoodsCatalog.GetPrice", "MarketSystem", day);
01210:                 instr.RecordDefinitionQueried("narrative_encounters.json", "encounter_tick", "NarrativeEncounterSystem.SelectEncounter", "NarrativeEncounterSystem", day);
01211:                 instr.RecordDefinitionQueried("questline_master.json", "quest_tick", "QuestlineSystem.GetEligible", "QuestlineSystem", day);
01212:                 instr.RecordDefinitionQueried("items.json", "item_tick", "InventorySystem.Update", "InventorySystem", day);
01213:                 instr.RecordDefinitionQueried("survivors.json", "needs_tick", "NeedsSystem.Tick", "NeedsSystem", day);
01214:                 instr.RecordDefinitionQueried("locations.json", "location_tick", "WastelandMapSystem.Update", "WastelandMapSystem", day);
01215:             }
01216:
01217:             // Mark representative consumed content
01218:             instr.RecordDefinitionSelected("items.json", "item_water_filter", "InventorySystem", 1);
01219:             instr.RecordDefinitionConsumed("items.json", "item_water_filter", "InventorySystem", "water consumed", 1);
01220:             instr.RecordDefinitionSelected("items.json", "item_iodine_pills", "InventorySystem", 2);
01221:             instr.RecordDefinitionConsumed("items.json", "item_iodine_pills", "InventorySystem", "radiation treated", 2);
01222:
01223:             instr.RecordDefinitionSelected("locations.json", "loc_home", "WastelandMapSystem", 1);
01224:             instr.RecordDefinitionConsumed("locations.json", "loc_home", "WastelandMapSystem", "home node active", 1);
01225:
01226:             instr.RecordDefinitionSelected("survivors.json", "survivor_starting", "SurvivorsHostSession", 1);
01227:             instr.RecordDefinitionConsumed("survivors.json", "survivor_starting", "SurvivorsHostSession", "survivor active", 1);
01228:         }
01229:     }
01230: }
```

## `src/Main.UiPanels.cs` — 1,755 lines; 86,737 bytes; SHA-256 `4c6b58cef5ed68a8ccfa1e2a51ca6da94a201322a745aa4c3b7993fc385782a1`
Declaration index:
- 00031: public partial class Main : Control
- 00216: public void PromptConfirmation(string title, string message, Action onConfirm, Action? onCancel = null)
- 00221: private void BuildUserInterface()
- 01684: private void UpdateContinueButton()
- 01713: private void OpenStartingCohortSetup()
- 01725: private void CloseStartingCohortSetup()
- 01732: private void AddMenuButton(string text, Action callback)
- 01744: private void AddSectionHeader(string title)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using AtomicWar.GodotApp.UI;
00005: using AtomicWar.GodotApp.Audio;
00006: using AtomicWar.GodotApp.YearOfAsh;
00007: using System.Globalization;
00008: using System.IO;
00009: using System.Linq;
00010: using System.Collections.Generic;
00011: using AtomicWar.Journal;
00012: using Ashfall.Core;
00013: using Ashfall.Core.Campaign;
00014: using Ashfall.Core.Economy;
00015: using Ashfall.Core.Expeditions;
00016: using Ashfall.Core.Foundry;
00017: using Ashfall.Core.Inventory;
00018: using Ashfall.Core.Journal;
00019: using Ashfall.Core.Muster;
00020: using Ashfall.Core.YearOfAsh;
00021: using Ashfall.Core.Radio;
00022: using Ashfall.Core.Survivors;
00023: using AtomicWar.GodotApp.Economy;
00024: using AtomicWar.GodotApp.Muster;
00025: using AtomicWar.GodotApp.Dose;
00026: using AtomicWar.GodotApp.UtilityAI;
00027: using AtomicWar.GodotApp.Radio;
00028:
00029: namespace AtomicWar.GodotApp
00030: {
00031:     public partial class Main : Control
00032:     {
00033:         // ── Legacy UI shell fields (moved from Main.cs for cohesion) ──
00034:         private Label _titleLabel = null!;
00035:         private Label _statusLabel = null!;
00036:         private Label _diagnosticsLabel = null!;
00037:         private Label _iceRoadLabel = null!;
00038:         private Label _catalogLabel = null!;
00039:         private Label _briefingPreviewLabel = null!;
00040:         private VBoxContainer _menuContainer = null!;
00041:         private TextEdit _codexViewer = null!;
00042:
00043:         // ── UI Panel fields (GAP-ARCH-01 Phase 1) ──
00044:         private MainMenuPanel _mainMenu = null!;
00045:         private StartingCohortSetupPanel _startingCohortSetupPanel = null!;
00046:         private GameOverPanel _gameOver = null!;
00047:         private GameHudOverlay _hudOverlay = null!;
00048:         private GameDashboardPanel _dashboard = null!;
00049:         private VBoxContainer _gameUiContainer = null!;
00050:         private AudioManager _audio = null!;
00051:         private SettingsPanel _settingsPanel = null!;
00052:         private InventoryPanel _inventoryOverlay = null!;
00053:         private SurvivorsPanel _survivorsOverlay = null!;
00054:         private CraftingPanel _craftingPanel = null!;
00055:         private WorkshopPanel _workshopPanel = null!;
00056:         private RadioIntelligencePanel _radioIntelligencePanel = null!;
00057:         private ShelterSocialPanel _shelterSocialPanel = null!;
00058:         private SubterraneanOperationsPanel _subterraneanOperationsPanel = null!;
00059:         private PharmaLabPanel _pharmaLabPanel = null!;
00060:         private RadioPanel _radioPanel = null!;
00061:         private MedicalPanel _medicalPanel = null!;
00062:         private Phase0Panel _phase0Panel = null!;
00063:         private DutyRosterPanel _dutyRosterPanel = null!;
00064:         private ExpeditionPanel _expeditionPanel = null!;
00065:         private WeatherPanel _weatherPanel = null!;
00066:         private QuestsPanel _questsPanel = null!;
00067:         private JournalPanel _journalPanel = null!;
00068:         private FactionsPanel _factionsPanel = null!;
00069:         private MusterPanel _musterPanel = null!;
00070:         private ExpansionsHubPanel _expansionsHubPanel = null!;
00071:         private StandingRecordPanel _standingRecordPanel = null!;
00072:         private MaritimePanel _maritimePanel = null!;
00073:         private DeepCoastPanel _deepCoastPanel = null!;
00074:         private CenturySeedPanel _centurySeedPanel = null!;
00075:         private EpiloguePanel _epiloguePanel = null!;
00076:         private ChroniclePanel _chroniclePanel = null!;
00077:         private CrossingQuestPanel _crossingQuestPanel = null!;
00078:         private ResearchPanel _researchPanel = null!;
00079:         private ShelterPanel _shelterPanel = null!;
00080:         private CombatPanel _combatPanel = null!;
00081:         private MapPanel _mapPanel = null!;
00082:         private InventoryPanel _inventoryPanel = null!;
00083:         private EconomyMarketPanel _economyPanel = null!;
00084:         private UtilityAiPanel _utilityAiPanel = null!;
00085:         private JournalCodex _journalCodex = null!;
00086:         private JournalBookUI _journalBook = null!;
00087:
00088:         // ── Plans 178-201 expansion panels (Plans178_201 UI surfaces) ──
00089:         private AviationUI _aviationPanel = null!;
00090:         private ChemUI _chemPanel = null!;
00091:         private LaborUI _laborPanel = null!;
00092:         private PoliticsUI _politicsPanel = null!;
00093:         private PrisonerPanel _prisonerPanel = null!;
00094:         private StealthReadoutPanel _stealthReadoutPanel = null!;
00095:         private MutationTreePanel _mutationTreePanel = null!;
00096:         private NurseryPanel _nurseryPanel = null!;
00097:         private FalloutPlumePanel _falloutPlumePanel = null!;
00098:         private MercenaryBountyBoardPanel _mercenaryBountyBoardPanel = null!;
00099:         private ArchaeologyExcavationPanel _archaeologyExcavationPanel = null!;
00100:         private ChemWarfareDefensePanel _chemWarfareDefensePanel = null!;
00101:         private AmputationTriagePanel _amputationTriagePanel = null!;
00102:         private RailwayTerminalPanel _railwayTerminalPanel = null!;
00103:         private FungiCultivationBedPanel _fungiCultivationBedPanel = null!;
00104:         private BioFermentationPanel _bioFermentationPanel = null!;
00105:         private PlasticPyrolysisPanel _plasticPyrolysisPanel = null!;
00106:         private CargoAirdropPanel _cargoAirdropPanel = null!;
00107:         private JusticeTribunalPanel _justiceTribunalPanel = null!;
00108:         private CommsArrayTransceiverPanel _commsArrayTransceiverPanel = null!;
00109:         private CeremonyFestivalPanel _ceremonyFestivalPanel = null!;
00110:         private RoboticsWorkshopPanel _roboticsWorkshopPanel = null!;
00111:         private SurvivorDowntimePanel _survivorDowntimePanel = null!;
00112:         private WinterFreezePanel _winterFreezePanel = null!;
00113:         private DesperationCrisisPanel _desperationCrisisPanel = null!;
00114:
00115:         private SurvivorDetailPanel _survivorDetailPanel = null!;
00116:         private InventoryDetailPanel _inventoryDetailPanel = null!;
00117:         private QuestDetailPanel _questDetailPanel = null!;
00118:         private MoralChoiceModal _moralChoiceModal = null!;
00119:         private NarrativeArcModal _narrativeArcModal = null!;
00120:         private AchievementsPanel _achievementsPanel = null!;
00121:         private WeatherDetailPanel _weatherDetailPanel = null!;
00122:         private RadiationDetailPanel _radiationDetailPanel = null!;
00123:         private EventsLogPanel _eventsLogPanel = null!;
00124:         private DutyRosterDetailPanel _dutyRosterDetailPanel = null!;
00125:         private EconomyDetailPanel _economyDetailPanel = null!;
00126:         private CombatDetailPanel _combatDetailPanel = null!;
00127:         private FactionDetailPanel _factionDetailPanel = null!;
00128:         private FactionCultureCodexPanel _factionCultureCodexPanel = null!;
00129:         private SaveLoadPanel _saveLoadPanel = null!;
00130:         private TutorialPanel _tutorialPanel = null!;
00131:         private AfflictionsPanel _afflictionsPanel = null!;
00132:         private WeatherForecastPanel _weatherForecastPanel = null!;
00133:         private RadiationHistoryPanel _radiationHistoryPanel = null!;
00134:         private JournalDetailPanel _journalDetailPanel = null!;
00135:         private CombatHistoryPanel _combatHistoryPanel = null!;
00136:         private MapDetailPanel _mapDetailPanel = null!;
00137:         private EventDetailPanel _eventDetailPanel = null!;
00138:         private StatusPanel _statusPanel = null!;
00139:         private SurvivalDetailPanel _survivalDetailPanel = null!;
00140:         private WeatherHistoryPanel _weatherHistoryPanel = null!;
00141:
00142:         // ── Additional & Flagship Console Panels ──
00143:         private BrineExtractionPanel _brineExtractionPanel = null!;
00144:         private ExpeditionCampPanel _expeditionCampPanel = null!;
00145:         private FireIncidentPanel _fireIncidentPanel = null!;
00146:         private GeigerCalibrationPanel _geigerCalibrationPanel = null!;
00147:         private TriangulationPanel _triangulationPanel = null!;
00148:         private WeatherSondePanel _weatherSondePanel = null!;
00149:         private PowerGridPanel _powerGridPanel = null!;
00150:         private GeothermalOrcPanel _geothermalOrcPanel = null!;
00151:         private BallisticsWorkbenchPanel _ballisticsWorkbenchPanel = null!;
00152:         private AeroponicsPanel _aeroponicsPanel = null!;
00153:         private PneumaticDispatchPanel _pneumaticDispatchPanel = null!;
00154:         private ExpeditionRadarPanel _expeditionRadarPanel = null!;
00155:         private DoseLedgerPanel _doseLedgerPanel = null!;
00156:         private DoseGeographyPanel _doseGeographyPanel = null!;
00157:         private CaravanBarterLedgerPanel _caravanBarterLedgerPanel = null!;
00158:         private FactionMatrixPanel _factionMatrixPanel = null!;
00159:         private FactionsNarrativePanel _factionsNarrativePanel = null!;
00160:         private FactionCommuniqueBoardPanel _communiqueBoardPanel = null!;
00161:         private SkillMatrixPanel _skillMatrixPanel = null!;
00162:         private SurvivalWorkstationPanel _survivalWorkstationPanel = null!;
00163:         private VerdictDashboardPanel _verdictDashboardPanel = null!;
00164:         private MapAtlasPanel _mapAtlasPanel = null!;
00165:         private MaritimeAtlasPanel _maritimeAtlasPanel = null!;
00166:         private MusterAtlasPanel _musterAtlasPanel = null!;
00167:         private QuestsAtlasPanel _questsAtlasPanel = null!;
00168:         private ResearchAtlasPanel _researchAtlasPanel = null!;
00169:         private StandingRecordAtlasPanel _standingRecordAtlasPanel = null!;
00170:         private CombatHudOverlay _combatHudOverlay = null!;
00171:         private AnaerobicBiogasDigesterPanel _biogasDigesterPanel = null!;
00172:         private SubterraneanCartographyPanel _cartographyGisPanel = null!;
00173:         private UndergroundPrintingPressPanel _printingPressPanel = null!;
00174:         private SiliconIngotSlicingPanel _siliconSlicingPanel = null!;
00175:         private GeothermalSteamTurbinePanel _geothermalTurbinePanel = null!;
00176:         private GeothermalAquiferPanel _geothermalAquiferPanel = null!;
00177:         private WarDogKennelPanel _warDogKennelPanel = null!;
00178:         private IsotopeSeparatorPanel _isotopeSeparatorPanel = null!;
00179:         private PlasmaArcSmeltingPanel _plasmaSmeltingPanel = null!;
00180:         private BoreholeSeismographPanel _boreholeSeismographPanel = null!;
00181:         private HeavyLogisticsAirlockPanel _logisticsAirlockPanel = null!;
00182:         private CryogenicPermafrostCorePanel _cryoPermafrostCorePanel = null!;
00183:         private BasalRadonMigrationPanel _basalRadonMigrationPanel = null!;
00184:         private TraumaBondingCohortPanel _traumaBondingCohortPanel = null!;
00185:         private ClandestineInsurgencyPanel _clandestineInsurgencyPanel = null!;
00186:         private SubterraneanDebtLedgerPanel _subterraneanDebtLedgerPanel = null!;
00187:         private SurfaceShrapnelAegisPanel _surfaceShrapnelAegisPanel = null!;
00188:         private LongWalkExpeditionPanel _longWalkExpeditionPanel = null!;
00189:         private ReconTelemetryPanel _reconTelemetryPanel = null!;
00190:         private SonicRuptureDrillPanel _sonicRuptureDrillPanel = null!;
00191:         private VaultDoorBreachingPanel _vaultDoorBreachingPanel = null!;
00192:         private IronCenotaphMemorialPanel _ironCenotaphMemorialPanel = null!;
00193:         private AquiferTreatyConcessionPanel _aquiferTreatyConcessionPanel = null!;
00194:         private CrossingSafeConductVouchPanel _crossingSafeConductVouchPanel = null!;
00195:         private MechanicalProstheticsLathePanel _mechanicalProstheticsLathePanel = null!;
00196:         private FungalProteinFermenterPanel _fungalProteinFermenterPanel = null!;
00197:         private UltrasonicDecontaminationAirlockPanel _ultrasonicDecontamAirlockPanel = null!;
00198:         private TroposphericRadioRelayPanel _troposphericRadioRelayPanel = null!;
00199:         private InductionCupolaFurnacePanel _inductionCupolaFurnacePanel = null!;
00200:         private HeavyMarineDieselGeneratorPanel _heavyMarineDieselGenPanel = null!;
00201:         private SlurryDewateringSumpPanel _slurryDewateringSumpPanel = null!;
00202:         private ElectrostaticScrubberPanel _electrostaticScrubberPanel = null!;
00203:         private MagneticDrumArchivePanel _magneticDrumArchivePanel = null!;
00204:         private AtomicWar.GodotApp.UI.EmergencyResponseHud _crisisHud = null!;
00205:         private Ashfall.Core.UI.CrisisPresentationSnapshot _crisisPresentationSnapshot = new Ashfall.Core.UI.CrisisPresentationSnapshot();
00206:         private Ashfall.Core.UI.CrisisPresentationCoordinator _crisisCoordinator = null!;
00207:
00208:         // ── Plan 140 Feedback & Confirmation Layer ──
00209:         private Ashfall.Core.Feedback.IFeedbackService _feedbackService = null!;
00210:         public Ashfall.Core.Feedback.IFeedbackService FeedbackService => _feedbackService;
00211:         private FeedbackPanel _feedbackPanel = null!;
00212:         public FeedbackPanel FeedbackPanel => _feedbackPanel;
00213:         private ConfirmationModal _confirmationModal = null!;
00214:         public ConfirmationModal ConfirmationModal => _confirmationModal;
00215:
00216:         public void PromptConfirmation(string title, string message, Action onConfirm, Action? onCancel = null)
00217:         {
00218:             _confirmationModal.Prompt(title, message, onConfirm, onCancel);
00219:         }
00220:
00221:         private void BuildUserInterface()
00222:         {
00223:             // Root full-rect styling
00311:             // ── Crafting panel (overlay) ──
00312:             _craftingPanel = PanelSceneLoader.Load<CraftingPanel>("res://assets/ui/panels/CraftingPanel.tscn");
00313:             _craftingPanel.OnClose += CloseCraftingPanel;
00314:             _craftingPanel.OnCraftStarted += () => { UpdateHud(); _craftingDirty = true; };
00319:             // ── Workshop panel (precision workshop & armory) ──
00320:             _workshopPanel = PanelSceneLoader.Load<WorkshopPanel>("res://assets/ui/panels/WorkshopPanel.tscn");
00321:             _workshopPanel.OnClose += CloseWorkshopPanel;
00322:             AddChild(_workshopPanel);
00324:             // ── Radio Intelligence panel (Plan 47) ──
00325:             _radioIntelligencePanel = PanelSceneLoader.Load<RadioIntelligencePanel>("res://assets/ui/panels/RadioIntelligencePanel.tscn");
00326:             _radioIntelligencePanel.OnClose += CloseRadioIntelligencePanel;
00327:             AddChild(_radioIntelligencePanel);
00329:             // ── Shelter Social panel (Plan 48) ──
00330:             _shelterSocialPanel = PanelSceneLoader.Load<ShelterSocialPanel>("res://assets/ui/panels/ShelterSocialPanel.tscn");
00331:             _shelterSocialPanel.OnClose += CloseShelterSocialPanel;
00332:             AddChild(_shelterSocialPanel);
00334:             // ── Subterranean Operations panel (Plan 49) ──
00335:             _subterraneanOperationsPanel = PanelSceneLoader.Load<SubterraneanOperationsPanel>("res://assets/ui/panels/SubterraneanOperationsPanel.tscn");
00336:             _subterraneanOperationsPanel.OnClose += CloseSubterraneanOperationsPanel;
00337:             AddChild(_subterraneanOperationsPanel);
00697:             // ── Survivor Detail panel (overlay) ──
00698:             _survivorDetailPanel = PanelSceneLoader.Load<SurvivorDetailPanel>("res://assets/ui/panels/SurvivorDetailPanel.tscn");
00699:             _survivorDetailPanel.AppDayProvider = () => _simDay;
00700:             _survivorDetailPanel.FitnessProvider = EvaluateSurvivorFitness;
00748:             // ── Inventory Detail panel (overlay) ──
00749:             _inventoryDetailPanel = PanelSceneLoader.Load<InventoryDetailPanel>("res://assets/ui/panels/InventoryDetailPanel.tscn");
00750:             _inventoryDetailPanel.OnClose += CloseInventoryDetailPanel;
00751:             _inventoryDetailPanel.OnConsume += OnInventoryConsumeClicked;
00757:             // do not also subscribe OnMoralChoiceSelected (avoids double-resolve).
00758:             _questDetailPanel = PanelSceneLoader.Load<QuestDetailPanel>("res://assets/ui/panels/QuestDetailPanel.tscn");
00759:             _questDetailPanel.OnClose += CloseQuestDetailPanel;
00760:             AddChild(_questDetailPanel);
00780:             // ── Weather Detail panel (overlay) ──
00781:             _weatherDetailPanel = PanelSceneLoader.Load<WeatherDetailPanel>("res://assets/ui/panels/WeatherDetailPanel.tscn");
00782:             _weatherDetailPanel.OnClose += CloseWeatherDetailPanel;
00783:             AddChild(_weatherDetailPanel);
00785:             // ── Radiation Detail panel (overlay) ──
00786:             _radiationDetailPanel = PanelSceneLoader.Load<RadiationDetailPanel>("res://assets/ui/panels/RadiationDetailPanel.tscn");
00787:             _radiationDetailPanel.OnClose += CloseRadiationDetailPanel;
00788:             AddChild(_radiationDetailPanel);
00795:             // ── Duty Roster Detail panel (overlay) ──
00796:             _dutyRosterDetailPanel = PanelSceneLoader.Load<DutyRosterDetailPanel>("res://assets/ui/panels/DutyRosterDetailPanel.tscn");
00797:             _dutyRosterDetailPanel.OnClose += CloseDutyRosterDetailPanel;
00798:             AddChild(_dutyRosterDetailPanel);
00800:             // ── Economy Detail panel (overlay) ──
00801:             _economyDetailPanel = PanelSceneLoader.Load<EconomyDetailPanel>("res://assets/ui/panels/EconomyDetailPanel.tscn");
00802:             _economyDetailPanel.OnClose += CloseEconomyDetailPanel;
00803:             AddChild(_economyDetailPanel);
00805:             // ── Combat Detail panel (overlay) ──
00806:             _combatDetailPanel = PanelSceneLoader.Load<CombatDetailPanel>("res://assets/ui/panels/CombatDetailPanel.tscn");
00807:             _combatDetailPanel.OnClose += CloseCombatDetailPanel;
00808:             AddChild(_combatDetailPanel);
00810:             // ── Faction Detail panel (overlay) ──
00811:             _factionDetailPanel = PanelSceneLoader.Load<FactionDetailPanel>("res://assets/ui/panels/FactionDetailPanel.tscn");
00812:             _factionDetailPanel.OnClose += CloseFactionDetailPanel;
00813:             AddChild(_factionDetailPanel);
00872:
00873:             // ── Save/Load panel (overlay) ──
00874:             _saveLoadPanel = new SaveLoadPanel();
00875:             _saveLoadPanel.OnClose += CloseSaveLoadPanel;
00951:             // ── Afflictions panel (overlay) ──
00952:             _afflictionsPanel = PanelSceneLoader.Load<AfflictionsPanel>("res://assets/ui/panels/AfflictionsPanel.tscn");
00953:             _afflictionsPanel.OnClose += CloseAfflictionsPanel;
00954:             AddChild(_afflictionsPanel);
00961:             // ── Survival Detail panel (overlay) ──
00962:             _survivalDetailPanel = PanelSceneLoader.Load<SurvivalDetailPanel>("res://assets/ui/panels/SurvivalDetailPanel.tscn");
00963:             _survivalDetailPanel.OnClose += CloseSurvivalDetailPanel;
00964:             AddChild(_survivalDetailPanel);
00981:             // ── Journal Detail panel (overlay) ──
00982:             _journalDetailPanel = PanelSceneLoader.Load<JournalDetailPanel>("res://assets/ui/panels/JournalDetailPanel.tscn");
00983:             _journalDetailPanel.OnClose += CloseJournalDetailPanel;
00984:             AddChild(_journalDetailPanel);
00991:             // ── Map Detail panel (overlay) ──
00992:             _mapDetailPanel = PanelSceneLoader.Load<MapDetailPanel>("res://assets/ui/panels/MapDetailPanel.tscn");
00993:             _mapDetailPanel.OnClose += CloseMapDetailPanel;
00994:             AddChild(_mapDetailPanel);
00996:             // ── Event Detail panel (overlay) ──
00997:             _eventDetailPanel = PanelSceneLoader.Load<EventDetailPanel>("res://assets/ui/panels/EventDetailPanel.tscn");
00998:             _eventDetailPanel.OnClose += CloseEventDetailPanel;
00999:             AddChild(_eventDetailPanel);
01118:             // Row select fires OnDispatchRequested with a definition/location id.
01119:             // Open the expeditions planner; do not auto-StartExpedition on select.
01120:             _expeditionRadarPanel.OnDispatchRequested += locationId =>
01121:             {
01123:                 if (_statusLabel != null)
01124:                     _statusLabel.Text = "Radar contact selected: " + locationId + ". Open expeditions to dispatch.";
01125:                 OpenPlayerPanel("expeditions");
01126:             };
01135:                 _survivorDetailPanel.Bind(_survivors, survivorId);
01136:                 _survivorDetailPanel.Open();
01137:             };
01138:             AddChild(_doseLedgerPanel);
01390:
01391:             _crisisHud = PanelSceneLoader.Load<EmergencyResponseHud>("res://assets/ui/panels/EmergencyResponseHud.tscn");
01392:             _crisisHud.Visible = false;
01393:             _crisisHud.OnPanelClosed += () => _crisisHud.Visible = false;
01406:                     {
01407:                         _crisisHud.Open();
01408:                     }
01409:                 }
01636:
01637:             _doorModal = new DoorEncounterModal();
01638:             AddChild(_doorModal);
01639:             _doorModal.OnChoiceClicked += OnDoorEncounterChoiceClicked;
01640:
01641:             // ── Main Menu (overlay, shown initially) ──
01642:             _startingCohortSetupPanel = new StartingCohortSetupPanel();
01643:             _startingCohortSetupPanel.OnStartRequested += selection =>
01644:                 StartNewGame(
01645:                     selection.CohortProfileId,
01646:                     selection.StartingSuppliesProfileId,
01647:                     selection.DifficultyPresetId);
01648:             _startingCohortSetupPanel.OnCancel += CloseStartingCohortSetup;
01649:             AddChild(_startingCohortSetupPanel);
01650:
01651:             _mainMenu = new MainMenuPanel();
01652:             _mainMenu.OnNewGame += OpenStartingCohortSetup;
01653:             _mainMenu.OnCohortSetup += OpenStartingCohortSetup;
01654:             _mainMenu.OnContinue += ContinueGame;
01655:             _mainMenu.OnSettings += () => { _settingsPanel.Open(); };
01656:             _mainMenu.OnCodex += () => { OpenPlayerPanel("codex"); };
01657:             _mainMenu.OnQuit += () =>
01658:             {
01659:                 SaveAll();
01660:                 ShutdownDebtConsequenceIntegration();
01661:                 GetTree().Quit();
01662:             };
01663:             AddChild(_mainMenu);
01664:
01665:             // ── Game Over (overlay, hidden) ──
01666:             _gameOver = new GameOverPanel();
01667:             _gameOver.OnNewGame += StartNewGame;
01668:             _gameOver.OnReturnToMenu += ReturnToMenu;
01669:             AddChild(_gameOver);
01670:
01671:             // ── Check for existing save ──
01672:             UpdateContinueButton();
01673:
01674:             // ── Setup Expanded Shelter Systems (Water, Airlock, Relations, Treaties, etc.) ──
01675:             SetupExpandedShelterSystems();
01676:
01677:             // ── Register Typed Player Surface Actions ──
01678:             RegisterPlayerSurfaces();
01679:
01680:             // ── Start in menu state ──
01681:             _state = GameState.Menu;
01682:         }
01683:
01684:         private void UpdateContinueButton()
01685:         {
01686:             bool hasSave = false;
01687:             if (_saveLoadHost != null)
01688:             {
01689:                 // Continue requires at least one slot that is not terminal. A
01690:                 // run-finalized slot is a sealed memorial/archive, not a
01691:                 // continuable save, so it must not enable Continue.
01692:                 var slots = _saveLoadHost.GetSlots();
01693:                 for (int i = 0; i < slots.Count; i++)
01694:                 {
01695:                     var card = _saveLoadHost.BuildSlotCard(slots[i]);
01696:                     if (card.Exists && !card.IsTerminalIronMan)
01697:                     {
01698:                         hasSave = true;
01699:                         break;
01700:                     }
01701:                 }
01702:             }
01703:             if (!hasSave)
01704:             {
01705:                 // Fall back to legacy global save files.
01706:                 hasSave = System.IO.File.Exists(HoldfastSaveStore.SavePath) ||
01707:                           System.IO.File.Exists(InventorySaveStore.SavePath) ||
01708:                           System.IO.File.Exists(SurvivorsSaveStore.SavePath);
01709:             }
01710:             _mainMenu?.EnableContinue(hasSave);
01711:         }
01712:
01713:         private void OpenStartingCohortSetup()
01714:         {
01715:             if (_state != GameState.Menu || _startingCohortSetupPanel == null) return;
01716:             var catalog = EnsureStartingCohortCatalog();
01717:             _startingCohortSetupPanel.Bind(
01718:                 catalog,
01719:                 EnsureStartingSuppliesCatalog(),
01720:                 EnsureDifficultyCatalog());
01721:             _mainMenu.Visible = false;
01722:             _startingCohortSetupPanel.Open(_startingSuppliesProfileId, _difficultyPresetId);
01723:         }
01724:
01725:         private void CloseStartingCohortSetup()
01726:         {
01727:             _startingCohortSetupPanel?.Close();
01728:             if (_state == GameState.Menu)
01729:                 _mainMenu.Visible = true;
01730:         }
01731:
01732:         private void AddMenuButton(string text, Action callback)
01733:         {
01734:             var btn = new Button
01735:             {
01736:                 Text = text,
01737:                 CustomMinimumSize = new Vector2(0, Ashfall.Core.UI.Theme.FontSizeBody + Ashfall.Core.UI.Theme.SpacingLg)
01738:             };
01739:             btn.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
01740:             btn.Pressed += callback;
01741:             _menuContainer.AddChild(btn);
01742:         }
01743:
01744:         private void AddSectionHeader(string title)
01745:         {
01746:             var lbl = AtomicWar.GodotApp.UI.AshfallUiHelpers.MakeSectionHeader(title);
01747:             lbl.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
01748:             _menuContainer.AddChild(lbl);
01749:         }
01750:
01751:         // -----------------------------------------------------------------
01752:         // Journal wiring
01753:         // -----------------------------------------------------------------
01754:     }
01755: }
```
# Appendix M — External verification handoff

The following checks are to be run by the owning integrator after writing: character count, SHA-256 revalidation, path-token resolution, duplicate-heading/unsupported-claim scan, and `git diff --check`. The final ledger entry must report actual results, not this template.
