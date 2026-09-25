# Plan 94 — Verdict Radio: Thirty Machine-Register Broadcasts, Day-Triggered Polling, and Verdict Save Ownership

> **Rebuild status:** TERMINAL 30-BROADCAST CONTENT + LIVE POLLING/CONSUMER AUDIT
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

The historical baseline was 5,103 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Keep the thirty authored Verdict machine-register broadcasts as a deterministic, day-triggered information surface while preserving `VerdictRadioSystem`, `VerdictHostSession`, the Verdict save owner, audio/presentation adapters, and existing quest/location conditioning. The historical 13→30 data expansion is complete; the residual is a current poll/reachability/fired-id audit, not a second radio station or arbitrary broadcast growth.

**Bounded outcome:** Audit `verdict_radio.json`, `VerdictCatalogLoader.LoadRadio`, `VerdictRadioSystem.Poll`, `VerdictHostSession.TickRadio`, `VerdictSave`, Verdict UI/audio consumers, current quest/location conditioning, and focused tests. Classify each broadcast as live, dormant, malformed, or fixture-only and preserve the exact-once fired-id contract.

**Non-goals:** no second radio/frequency/quest owner, no real-world copy, no arbitrary broadcast count, no direct ending mutation from text, no new save section, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `verdict_radio.json` contains 30 broadcasts.
- VERIFIED CURRENT: `VerdictCatalogLoader.LoadRadio` parses the current row shape.
- VERIFIED CURRENT: `VerdictRadioSystem.Poll` owns day/phase filtering and fired ids, and the existing Verdict save captures them.
- The current production audio/UI/quest-conditioning reachability of each row is an explicit premise question.
- HISTORICAL RECORD: Plan 94/Wave 40 records the 13→30 expansion; this package does not claim a fresh test run.

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

- `Assets/StreamingAssets/Data/verdict_radio.json` exists at 11,606 bytes; SHA-256 `2a8a7faeb9d7a9af175f7188311f1c94892a63604c88932e12d068e70ea555aa`.
- `Assets/StreamingAssets/Data/verdict_locations.json` exists at 14,430 bytes; SHA-256 `e97faf513dfa2e3f6b9cfb7fd1311cea6534a283e7d81e099784232b9d9380d0`.
- `Assets/StreamingAssets/Data/verdict_questlines.json` exists at 85,668 bytes; SHA-256 `18f04bdcacbc31e6be03358116f0390de2786309d8d0ddbd9770b0aed94dcdc2`.
- `Assets/StreamingAssets/Data/verdict_npcs.json` exists at 9,832 bytes; SHA-256 `ace1edded901844ae68ebbaad7f913990316ab155888ccee2c531766be7cbadc`.
- `Assets/StreamingAssets/Data/audio_cues.json` exists at 112,400 bytes; SHA-256 `afec6f7eb662d456667f2e60b41255f689278524593f06d19e93a9d63a207bd0`.

# 3. Required Delta

Replace the old pure-data brief with a current 30-row broadcast/poll/fired-id audit. Preserve VerdictRadioSystem, the existing verdict save, and canonical audio/quest/ending consumers.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Which current production caller invokes `VerdictRadioSystem.Poll` and with which day/phase?
Do all 30 rows have valid current audio cues, kinds, and signal strengths?
Are any quest/location conditioning fields or consumers present, or is day-triggered polling the actual contract?
Do fired ids survive restore and prevent duplicate presentation?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| broadcast definitions/parsing | `VerdictCatalogLoader.LoadRadio` | `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` | Static frequency/day/kind/message/cue rows. |
| day polling and fired-id state | `VerdictRadioSystem` | `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs` | Owns Poll, fired ids, and capture/restore. |
| Verdict host day/census/radio composition | `VerdictHostSession` | `src/Host/VerdictHostSession.cs` | Calls current radio owner and projects results. |
| Verdict persistence | `VerdictSave / VerdictSaveStore` | `Assets/Ashfall.Core/Verdict/VerdictSave.cs; src/Host/VerdictSaveStore.cs` | Existing verdict section/codec. |
| audio/UI presentation | `Verdict panels and existing audio/radio adapters` | `src/UI/VerdictDashboardPanel.cs; src/VerdictPanel.cs` | Presentation/cue binding only. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for Verdict radio broadcast corpus.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Verdict Radio: Thirty Machine-Register Broadcasts, Day-Triggered Polling, and Verdict Save Ownership                                               │
│ Integration route: DATA-ONLY + current Verdict radio/poll/save audit                             │
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

1. **Catalog defines broadcasts.**
2. **VerdictRadioSystem polls and dedupes.**
3. **Verdict host/save persist fired ids.**
4. **Panels/audio present current facts.**
5. **Consequences use canonical Verdict owners.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| broadcast definitions/parsing | `VerdictCatalogLoader.LoadRadio` | `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` | Static frequency/day/kind/message/cue rows. |
| day polling and fired-id state | `VerdictRadioSystem` | `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs` | Owns Poll, fired ids, and capture/restore. |
| Verdict host day/census/radio composition | `VerdictHostSession` | `src/Host/VerdictHostSession.cs` | Calls current radio owner and projects results. |
| Verdict persistence | `VerdictSave / VerdictSaveStore` | `Assets/Ashfall.Core/Verdict/VerdictSave.cs; src/Host/VerdictSaveStore.cs` | Existing verdict section/codec. |
| audio/UI presentation | `Verdict panels and existing audio/radio adapters` | `src/UI/VerdictDashboardPanel.cs; src/VerdictPanel.cs` | Presentation/cue binding only. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load 30 broadcast rows through VerdictCatalogLoader.LoadRadio
2. bind corpus to VerdictRadioSystem
3. on a canonical day, Poll with current Reckoning/Verdict phase
4. filter dayTrigger/phase and select rows in stable order
5. mark each returned id fired exactly once and project audio/text through existing consumers
6. capture/restore fired ids in the existing Verdict save

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- broadcast ids are unique and day triggers/kinds/strengths are valid
- frequency and cue fields use current vocabularies
- Poll is deterministic for day/phase and corpus order
- a fired id is never returned again after restore
- a broadcast cannot directly award evidence or choose an ending
- missing audio cue degrades visibly without crashing
- unknown phase/day fails closed or uses the current documented policy

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

Contract rules for Verdict radio broadcast corpus:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`verdict_radio.json` remains the thirty-row broadcast authority. Current fields include id, frequency, dayTrigger, source, message, signalStrength, kind, and optional audio_cue. Audit whether every kind/frequency/cue is consumed and whether quest/location conditioning exists. The historical plan’s “3 broadcasts wired” language is not accepted without a current caller trace.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

Fired ids live in `VerdictRadioState` inside the existing `verdict` save section through `VerdictSaveStore`; static broadcast rows are not persisted as a second store. Any new poll state or conditioning flag must be additive to the current codec and preserve old fired defaults.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

Poll uses current day/phase and catalog order; no wall-clock or hash iteration. Same corpus/version/day/phase yields the same ordered broadcast ids. Paired replay compares returned ids, fired ids, cues selected, and journal/UI projection.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

VerdictRadioSystem emits/returns broadcasts and records fired ids. VerdictHostSession may journal/project them and existing audio/UI adapters may play/render them. A row cannot mutate Reckoning, quest, faction, or ending state directly; any consequence uses a canonical owner after a typed fact.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Host/VerdictHostSession.cs` — owns current day/census/radio host calls
- `src/Host/VerdictSaveStore.cs` — persists Verdict state and fired ids
- `src/Main.Verdict.cs` — setup, tick, readout, and save handoff
- `src/UI/VerdictDashboardPanel.cs` — presents current broadcasts/evidence
- `src/VerdictPanel.cs` — existing Verdict surface
- `src/Main.PlayerSurfaces.cs` — route/lifecycle integration; shared root

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Machine-register broadcasts are terse, fictional, and slightly uncanny because the data implies an absent operator. They must not copy real transmissions, reveal a false ending, or claim a frequency/event the current radio owner cannot produce. A broadcast is evidence, not omniscient narration.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/VerdictRadioExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/VerdictRadioSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/VerdictContentWebTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/Plan94_102RadioFoundryIntegrationTests.cs`

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
| Phase 0 — corpus/poll census | read 30 rows, loader, radio system, host, save, and tests | all fields and current poll semantics are explicit | no undocumented scope or shortcut |
| Phase 1 — live consumer trace | follow Poll → fired id → journal/audio/UI → save | live versus fixture-only rows are classified | no undocumented scope or shortcut |
| Phase 2 — determinism/save/accessibility audit | check day/phase order, restore, cues, empty/error states | no duplicate radio or invisible data | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a proven conditioning/presentation gap is promoted | one owner and focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/verdict_radio.json` | READ ONLY; MODIFY only for a proven row/consumer defect | retain as broadcast authority |
| `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` | READ ONLY | radio loader |
| `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs` | READ ONLY | poll/fired owner |
| `src/Host/VerdictHostSession.cs` | READ ONLY | host day/poll seam |
| `src/UI/VerdictDashboardPanel.cs` | READ ONLY | truthful presentation |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| parallel radio owner | keep VerdictRadioSystem/VerdictHostSession authoritative |
| fired-id loss | use existing verdict codec and replay test |
| cue fallback fabrication | show diagnostic/text |
| ending drift | route consequences through Reckoning/Verdict owner |

# 22. Explicit Non-Goals

- no second radio/frequency/quest owner, no real-world copy, no arbitrary broadcast count, no direct ending mutation from text, no new save section, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for Verdict radio broadcast corpus are named from current evidence.
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

- A current 30-row broadcast-to-consumer/day/phase/cue matrix.
- An explicit fired-id and restore contract.
- A bounded residual only for a proven polling/conditioning/presentation gap.

## MUST NOT DO

- create a second radio/frequency owner
- mark broadcasts fired on load
- mutate endings/evidence from message text
- add a static-broadcast save section

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/VerdictRadioExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/VerdictRadioSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/VerdictContentWebTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/Plan94_102RadioFoundryIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read verdict_radio.json, VerdictCatalogLoader.LoadRadio, VerdictRadioSystem, VerdictHostSession.TickRadio, VerdictSave, Verdict UI/audio consumers, and focused tests; classify every row.

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

### Authority lines 698–703
00698: **A-16 · C7 · Standing-record testimony depth.** Subject: witness-statement and registry-annotation prose expanding `standing_record_memory.json` coverage. Evidence: the standing-record family (factions, layouts, memory, quests) is verified live and is a canon epilogue evidence source. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00699:
00700: **A-17 · C7 · Verdict radio continuation.** Subject: verdict-station rundown batches conditioned on verdict questline state. Evidence: `verdict_radio.json` verified live; verdict questlines are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00701:
00702: **A-18 · C7 · Warlord doctrine communiqués.** Subject: doctrine-conditioned communiqué and tribute-demand prose for warlords whose public/private language separation is thin. Evidence: `warlord_doctrines.json`, `faction_war_communiques.json` verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00703:

### Authority lines 720–725
00720: **A-27 · C12 · Storm-window almanac entries.** Subject: almanac prose conditioned on `year_of_ash_storm_windows.json` entries. Evidence: catalog verified live; `weather_almanac_expansion` exists in the corpus. Route: DATA-ONLY, within the Year-of-Ash window (180–360) canon. Confidence: HIGH CONFIDENCE.
00721:
00722: **A-28 · C13 · Under-served epilogue chronicle depth.** Subject: consumed by F-005 after the permutation audit selects the weakest cells. Evidence: matrix is canon (32 permutations). Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00723:
00724: **A-29 · C14 · Bestiary natural-history continuation.** Subject: sighting-log and specimen-record prose for bestiary entries with thin coverage. Evidence: `wasteland_wildlife_bestiary.json` verified live; vulture-sighting and cockroach-hive log genres exist. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00725:

### Authority lines 766–771
00766: **B-19 · C12 · Winter pressure for power and water systems.** Subject: Year-of-Ash window (180–360) pressure extensions for systems that currently produce no winter-specific cost (filter burn, diesel reserve drawdown curves conditioned on storm windows). Evidence: storm windows canon; hardening upgrades catalog live. Route: CORE-EXTENSION + data. Confidence: PROPOSAL.
00767:
00768: **B-20 · C13 · Reckoning evidence enrollment sweep.** Subject: audit systems added since the 19-wave for evidence enrollment gaps; enroll through the existing Reckoning path only. Evidence: 19C closed (DR-06); subsequent waves exist (Waves 8–12 logs). Route: audit then CORE-EXTENSION. Confidence: HIGH CONFIDENCE.
00769:
00770: **B-21 · C14 · Migration ↔ route-encounter bridge.** Subject: wildlife migration state conditioning travel-encounter selection on routes crossing migration corridors. Evidence: `WildlifeMigrationSystem`, `travel_encounters.json` both canon. Route: CORE-EXTENSION. Confidence: PROPOSAL.
00771:

### Authority lines 826–831
00826: **D-08 · C16 · Completion-history difficulty stamp integrity.** Subject: verify the difficulty-preset stamp (schema v2) migrates cleanly when W1 adds preset fields. Evidence: stamp is canon (v1.0 Part 16.7). Route: migration + tests, W1-coordinated. Confidence: PROPOSAL, sequence-gated.
00827:
00828: ## 2.5 Lane E — UI, UX, and accessibility seeds (E-01 … E-10)
00829:
00830: **E-01 · C17 · Briefing surface for post-19-wave systems.** Subject: daily-briefing entries for systems landed in Waves 8–12 that produce player-relevant state but no briefing row. Evidence: briefing surface is canon (`DailyBriefing` panel). Route: HOST-WIRING only. Confidence: HIGH CONFIDENCE that the class exists; enumerate in session.
00831:

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

### Authority lines 955–960
00955: **DM-12 — Weather and Year of Ash (C12).** Owners: weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash family (events/items/locations/questlines/quests/radio/survivors/storm windows). Live catalogs: all of the above verified live. Hosts: WeatherHardening, YearOfAsh widgets, WeatherStationSystem. Openings: A-27, B-03, B-19, C-09, E-09, F-02, G-07, plus the F-002 campaign. Constraint: tick window 180–360 canon.
00956:
00957: **DM-13 — Endgame and epilogue (C13).** Owners: Reckoning, verdict ending evaluator, epilogue matrix runtime, epilogue chronicle, standing records, census, muster epilogues, holdfast endings. Live catalogs: `endings`, `campaign_epilogues`, `epilogue_chronicle`, `verdict_data/items/locations/npcs/questlines/radio`, `standing_record_factions/layouts/memory/quests`, `muster_epilogues`. Hosts: Endgame, Verdict, StandingRecord. Openings: A-28, B-20, D-03, E-10, F-03, G-04, plus the F-005 campaign. Constraint: main ending cannot be invalidated by optional content.
00958:
00959: **DM-14 — Ecology and wildlife (C14).** Owners: migration, trapping, ecosystem, seasonal calendar, bestiary, underground flora, infestations, contagion, pathogens, crop genomes. Live catalogs: `wildlife_ecosystem`, `wildlife_trapping_catalog`, `wasteland_wildlife_bestiary`, `underground_flora`, `ecological_infestations`, `contagion_events`, `pathogens`, `crop_strains`, `mutations`. Hosts: WildlifeEcosystem, WildlifeTrapping. Openings: A-29, B-21, C-10, plus the F-002 blight arc. Constraint: zoonosis bridge and campfire sanitization are the owned seams.
00960:

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/verdict_radio.json`
- Bytes: 11,606; SHA-256: `2a8a7faeb9d7a9af175f7188311f1c94892a63604c88932e12d068e70ea555aa`
- Root keys: `broadcasts, schema_version`
- `broadcasts`: list[30]; union fields: `audio_cue, dayTrigger, frequency, id, kind, message, signalStrength, source`
  - row 1: `{"audio_cue":"radio_vo_verdict_meter","dayTrigger":210,"frequency":"99.0 MHz","id":"radio_verdict_meter_reads_1142","kind":"telemetry","message":"11:42. 11:42. 11:42. The meter reads 11:42 and nothing else. A data burst that is three numbers and no hand.","signalStrength":"S1","source":"Census Carrier, Machine Registers"}`
  - row 2: `{"dayTrigger":211,"frequency":"99.0 MHz","id":"radio_verdict_fuse_serviced","kind":"maintenance","message":"Fuse serviced. Service order logged. Timestamp matches Mortar Period exactly. Nothing was wrong; the schedule simply said it was time.","signalStrength":"S2","source":"Fuse World, Service Bay"}`
  - row 3: `{"dayTrigger":242,"frequency":"99.0 MHz","id":"radio_verdict_wing_sleeps","kind":"telemetry","message":"The wing sleeps. Draw down 0.5 degrees. First broadcast post-Call. No flight movement registered; the roost is cold.","signalStrength":"S2","source":"Drone Hive, Draw Readout"}`
  - row 4: `{"audio_cue":"radio_vo_verdict_count","dayTrigger":240,"frequency":"99.0 MHz","id":"radio_verdict_off_count_assessed","kind":"call","message":"Off-count is a penalty assessed against the holder.","signalStrength":"S3","source":"The Office of Censuses"}`
  - row 5: `{"audio_cue":"radio_vo_verdict_eden","dayTrigger":245,"frequency":"88.5 MHz","id":"radio_verdict_eden_was_here","kind":"witness","message":"Eden was here. Eleven months of bleed, one day's worth, in the vocabulary of the Weather Service: visibility, pressure, front. She never asked to be counted. She was counted anyway.","signalStrength":"S2","source":"Eden Vale, Tube Bleed"}`
  - row 6: `{"dayTrigger":240,"frequency":"88.5 MHz","id":"radio_verdict_count_is_open","kind":"call","message":"This is the Office of Censuses. The count is open. All persons having custody of persons must present them. Off-count is a penalty assessed against the holder. This message will repeat. The count is open.","signalStrength":"S3","source":"The Office of Censuses"}`
  - row 7: `{"dayTrigger":213,"frequency":"99.0 MHz","id":"radio_verdict_clock_disagrees","kind":"telemetry","message":"Clock drift: 3 days. The machine's calendar and the civil calendar do not agree. This is presented as data, because it is data. There is no name for the disagreement and none is needed.","signalStrength":"S1","source":"Machine Registers, Clock"}`
  - row 8: `{"audio_cue":"radio_vo_verdict_geophone","dayTrigger":218,"frequency":"99.0 MHz","id":"radio_verdict_geophone_taps","kind":"readings","message":"Tap. Tap. Tap at ploughing, tap at harvest, a tap for every worker who walked the Allotments. The array reads the ground as one slow heartbeat. Unlabeled. Unclaimed.","signalStrength":"S2","source":"Geophone Pit, Array 1"}`
  - row 9: `{"dayTrigger":250,"frequency":"99.0 MHz","id":"radio_verdict_valve_accessed_36","kind":"maintenance","message":"Valve S36 accessed per §36. Shift-water readout logged. The valve knows which hand turned it, and the meter keeps the account.","signalStrength":"S2","source":"Water Plant, Valve S36"}`
  - row 10: `{"dayTrigger":255,"frequency":"99.0 MHz","id":"radio_verdict_reels_matter","kind":"count","message":"The reels matter. Twenty-one racks, four years per rack. The archive is not in the habit of taking dictation. Every reel dated five years the day it was cut.","signalStrength":"S2","source":"Archive Tape-Silo"}`
  - row 11: `{"dayTrigger":260,"frequency":"99.0 MHz","id":"radio_verdict_presentation_names_holders","kind":"call","message":"The presentation names the holders. The count was taken; it was presented; and the presentation does not require being read. It was read anyway, by someone with custody.","signalStrength":"S3","source":"The Office of Censuses"}`
  - row 12: `{"dayTrigger":210,"frequency":"99.0 MHz","id":"radio_verdict_carrier_on_window","kind":"carrier","message":"A thin A/B tone, one second on, one second off, on a dead band. Occupied — not speech. To a radio-craft survivor it reads as a census carrier. The countdown is environment, not adversary.","signalStrength":"S1","source":"Census Carrier, Pilot Tone"}`
  - row 13: `{"audio_cue":"radio_vo_verdict_reckoning","dayTrigger":241,"frequency":"99.0 MHz","id":"radio_verdict_reckoning_call","kind":"call","message":"The Office of Censuses is convening. The count is open. All persons having custody of persons must present them. This message will repeat once per hour for three hours, then hold the carrier open until a signature is received.","signalStrength":"S3","source":"The Office of Censuses — Reckoning"}`
  - row 14: `{"dayTrigger":268,"frequency":"99.0 MHz","id":"radio_verdict_barometric_spread","kind":"telemetry","message":"Barometric spread logged: 982 hPa measured vs 1014 hPa central model. Local barograph deviation retained without manual override.","signalStrength":"S2","source":"Cape Wrath Meteorological Station"}`
  - row 15: `{"dayTrigger":272,"frequency":"99.0 MHz","id":"radio_verdict_service_cycle_greywater","kind":"maintenance","message":"Stilling well purge cycle complete. Backwash filter cleared. Fault count: zero. Next scheduled service in 30 days.","signalStrength":"S2","source":"Greywater Tide Gauge Station"}`
  - row 16: `{"dayTrigger":275,"frequency":"99.0 MHz","id":"radio_verdict_stilling_well_delta","kind":"telemetry","message":"Stilling well datum reads +6 cm against baseline brass scale. Tidal harmonic model predicts -14 cm. Discrepancy logged as sensor offset.","signalStrength":"S2","source":"Greywater Tide Gauge Station"}`
  - row 17: `{"dayTrigger":280,"frequency":"99.0 MHz","id":"radio_verdict_subsector_ledger_update","kind":"census","message":"Subsector ledger update acknowledged. Registered entities: 412. Unreconciled balance carries forward to reckoning window.","signalStrength":"S3","source":"Census Carrier, Machine Registers"}`
  - row 18: `{"dayTrigger":284,"frequency":"99.0 MHz","id":"radio_verdict_geophone_offset_recal","kind":"calibration","message":"Array 1 baseline recalibration: geophone offset drift -0.04 Hz. Hardware correction rejected by safety interlock. Running on uncorrected baseline.","signalStrength":"S1","source":"Geophone Pit, Array 1"}`
  - row 19: `{"dayTrigger":288,"frequency":"99.0 MHz","id":"radio_verdict_strata_density_drift","kind":"telemetry","message":"Subsurface core layer 7 acoustic density reads 1.48 g/cm³ against archived 2.12 g/cm³. Core rack sealed. Re-audit scheduled.","signalStrength":"S1","source":"Highland Core-Sample Repository"}`
  - row 20: `{"dayTrigger":295,"frequency":"99.0 MHz","id":"radio_verdict_relay_switch_pass4","kind":"maintenance","message":"Pass 4 automatic battery switchover cycle verified. Auxiliary cells charged to 24.2 V. Decommissioned line impedance nominal.","signalStrength":"S1","source":"Pass 4 Signal Relay Mast"}`
  - row 21: `{"dayTrigger":300,"frequency":"88.5 MHz","id":"radio_verdict_unscheduled_burst_88","kind":"anomaly","message":"Unscheduled carrier burst on civil channel: 420 ms duration. Modulation index inconsistent with broadcast schedule. Logged without attribution.","signalStrength":"S1","source":"Unregistered Carrier Bleed"}`
  - row 22: `{"dayTrigger":304,"frequency":"99.0 MHz","id":"radio_verdict_river_stage_deviation","kind":"telemetry","message":"River stage telemetry: 4.12 m at station sensor. No upstream rain event registered by telemetry basin. Offset flagged.","signalStrength":"S2","source":"Karsk River Gauging Station"}`
  - row 23: `{"dayTrigger":312,"frequency":"99.0 MHz","id":"radio_verdict_core_vault_desiccant_purge","kind":"maintenance","message":"Desiccant purge valve cycle complete. Vault interior relative humidity maintained at 18 percent. Nitrogen atmosphere sealed.","signalStrength":"S2","source":"Highland Core-Sample Repository"}`
  - row 24: `{"dayTrigger":318,"frequency":"99.0 MHz","id":"radio_verdict_unverified_household_tally","kind":"census","message":"Interim census registry: 419 entities entered. Seven unverified entries held in provisional ledger pending verification.","signalStrength":"S3","source":"Census Carrier, Machine Registers"}`
  - row 25: `{"dayTrigger":326,"frequency":"99.0 MHz","id":"radio_verdict_repeater_origin_mismatch","kind":"telemetry","message":"Packet 884 origin header mismatch: physical path deviates from routing table. Packet forwarded as valid.","signalStrength":"S2","source":"North Cliff Observation Bunker"}`
  - row 26: `{"dayTrigger":330,"frequency":"99.0 MHz","id":"radio_verdict_spectrometry_drift_stjude","kind":"calibration","message":"Sample rack 12 isotope drift: +0.08 permille. Hardware offset retained. Analysis valid as filed.","signalStrength":"S2","source":"St. Jude Marine Laboratory"}`
  - row 27: `{"dayTrigger":338,"frequency":"99.0 MHz","id":"radio_verdict_substation_breaker_test","kind":"maintenance","message":"Automated bus isolation test executed. High-voltage breaker trip latency: 12 ms. Distribution grid continuity intact.","signalStrength":"S3","source":"The Fuse World, Bus Isolation"}`
  - row 28: `{"dayTrigger":344,"frequency":"99.0 MHz","id":"radio_verdict_holding_capacity_parity","kind":"census","message":"Holding capacity parity calculation: quota 420, presented count 419. Margin: 1 entity. Reconciliation window remaining.","signalStrength":"S3","source":"The Office of Censuses"}`
  - row 29: `{"dayTrigger":352,"frequency":"99.0 MHz","id":"radio_verdict_telemetry_phase_inversion","kind":"anomaly","message":"Ground shock arrival detected with 180 degree phase inversion. Primary wave precedes secondary by 18 ms. Calibration verified.","signalStrength":"S2","source":"Pass 4 Signal Relay Mast"}`
  - row 30: `{"dayTrigger":360,"frequency":"99.0 MHz","id":"radio_verdict_carrier_override_standby","kind":"emergency","message":"Carrier override engaged. Routine telemetry schedule suspended. Auxiliary registers sealed. The census window is closed.","signalStrength":"S4","source":"The Office of Censuses — Master Terminal"}`
- Bytes: 11,606; SHA-256: `2a8a7faeb9d7a9af175f7188311f1c94892a63604c88932e12d068e70ea555aa`
- Root keys: `broadcasts, schema_version`
- `broadcasts`: list[30]; union fields: `audio_cue, dayTrigger, frequency, id, kind, message, signalStrength, source`
  - row 1: `{"audio_cue":"radio_vo_verdict_meter","dayTrigger":210,"frequency":"99.0 MHz","id":"radio_verdict_meter_reads_1142","kind":"telemetry","message":"11:42. 11:42. 11:42. The meter reads 11:42 and nothing else. A data burst that is three numbers and no hand.","signalStrength":"S1","source":"Census Carrier, Machine Registers"}`
  - row 2: `{"dayTrigger":211,"frequency":"99.0 MHz","id":"radio_verdict_fuse_serviced","kind":"maintenance","message":"Fuse serviced. Service order logged. Timestamp matches Mortar Period exactly. Nothing was wrong; the schedule simply said it was time.","signalStrength":"S2","source":"Fuse World, Service Bay"}`
  - row 3: `{"dayTrigger":242,"frequency":"99.0 MHz","id":"radio_verdict_wing_sleeps","kind":"telemetry","message":"The wing sleeps. Draw down 0.5 degrees. First broadcast post-Call. No flight movement registered; the roost is cold.","signalStrength":"S2","source":"Drone Hive, Draw Readout"}`
  - row 4: `{"audio_cue":"radio_vo_verdict_count","dayTrigger":240,"frequency":"99.0 MHz","id":"radio_verdict_off_count_assessed","kind":"call","message":"Off-count is a penalty assessed against the holder.","signalStrength":"S3","source":"The Office of Censuses"}`
  - row 5: `{"audio_cue":"radio_vo_verdict_eden","dayTrigger":245,"frequency":"88.5 MHz","id":"radio_verdict_eden_was_here","kind":"witness","message":"Eden was here. Eleven months of bleed, one day's worth, in the vocabulary of the Weather Service: visibility, pressure, front. She never asked to be counted. She was counted anyway.","signalStrength":"S2","source":"Eden Vale, Tube Bleed"}`
  - row 6: `{"dayTrigger":240,"frequency":"88.5 MHz","id":"radio_verdict_count_is_open","kind":"call","message":"This is the Office of Censuses. The count is open. All persons having custody of persons must present them. Off-count is a penalty assessed against the holder. This message will repeat. The count is open.","signalStrength":"S3","source":"The Office of Censuses"}`
  - row 7: `{"dayTrigger":213,"frequency":"99.0 MHz","id":"radio_verdict_clock_disagrees","kind":"telemetry","message":"Clock drift: 3 days. The machine's calendar and the civil calendar do not agree. This is presented as data, because it is data. There is no name for the disagreement and none is needed.","signalStrength":"S1","source":"Machine Registers, Clock"}`
  - row 8: `{"audio_cue":"radio_vo_verdict_geophone","dayTrigger":218,"frequency":"99.0 MHz","id":"radio_verdict_geophone_taps","kind":"readings","message":"Tap. Tap. Tap at ploughing, tap at harvest, a tap for every worker who walked the Allotments. The array reads the ground as one slow heartbeat. Unlabeled. Unclaimed.","signalStrength":"S2","source":"Geophone Pit, Array 1"}`
  - row 9: `{"dayTrigger":250,"frequency":"99.0 MHz","id":"radio_verdict_valve_accessed_36","kind":"maintenance","message":"Valve S36 accessed per §36. Shift-water readout logged. The valve knows which hand turned it, and the meter keeps the account.","signalStrength":"S2","source":"Water Plant, Valve S36"}`
  - row 10: `{"dayTrigger":255,"frequency":"99.0 MHz","id":"radio_verdict_reels_matter","kind":"count","message":"The reels matter. Twenty-one racks, four years per rack. The archive is not in the habit of taking dictation. Every reel dated five years the day it was cut.","signalStrength":"S2","source":"Archive Tape-Silo"}`
  - row 11: `{"dayTrigger":260,"frequency":"99.0 MHz","id":"radio_verdict_presentation_names_holders","kind":"call","message":"The presentation names the holders. The count was taken; it was presented; and the presentation does not require being read. It was read anyway, by someone with custody.","signalStrength":"S3","source":"The Office of Censuses"}`
  - row 12: `{"dayTrigger":210,"frequency":"99.0 MHz","id":"radio_verdict_carrier_on_window","kind":"carrier","message":"A thin A/B tone, one second on, one second off, on a dead band. Occupied — not speech. To a radio-craft survivor it reads as a census carrier. The countdown is environment, not adversary.","signalStrength":"S1","source":"Census Carrier, Pilot Tone"}`
  - row 13: `{"audio_cue":"radio_vo_verdict_reckoning","dayTrigger":241,"frequency":"99.0 MHz","id":"radio_verdict_reckoning_call","kind":"call","message":"The Office of Censuses is convening. The count is open. All persons having custody of persons must present them. This message will repeat once per hour for three hours, then hold the carrier open until a signature is received.","signalStrength":"S3","source":"The Office of Censuses — Reckoning"}`
  - row 14: `{"dayTrigger":268,"frequency":"99.0 MHz","id":"radio_verdict_barometric_spread","kind":"telemetry","message":"Barometric spread logged: 982 hPa measured vs 1014 hPa central model. Local barograph deviation retained without manual override.","signalStrength":"S2","source":"Cape Wrath Meteorological Station"}`
  - row 15: `{"dayTrigger":272,"frequency":"99.0 MHz","id":"radio_verdict_service_cycle_greywater","kind":"maintenance","message":"Stilling well purge cycle complete. Backwash filter cleared. Fault count: zero. Next scheduled service in 30 days.","signalStrength":"S2","source":"Greywater Tide Gauge Station"}`
  - row 16: `{"dayTrigger":275,"frequency":"99.0 MHz","id":"radio_verdict_stilling_well_delta","kind":"telemetry","message":"Stilling well datum reads +6 cm against baseline brass scale. Tidal harmonic model predicts -14 cm. Discrepancy logged as sensor offset.","signalStrength":"S2","source":"Greywater Tide Gauge Station"}`
  - row 17: `{"dayTrigger":280,"frequency":"99.0 MHz","id":"radio_verdict_subsector_ledger_update","kind":"census","message":"Subsector ledger update acknowledged. Registered entities: 412. Unreconciled balance carries forward to reckoning window.","signalStrength":"S3","source":"Census Carrier, Machine Registers"}`
  - row 18: `{"dayTrigger":284,"frequency":"99.0 MHz","id":"radio_verdict_geophone_offset_recal","kind":"calibration","message":"Array 1 baseline recalibration: geophone offset drift -0.04 Hz. Hardware correction rejected by safety interlock. Running on uncorrected baseline.","signalStrength":"S1","source":"Geophone Pit, Array 1"}`
  - row 19: `{"dayTrigger":288,"frequency":"99.0 MHz","id":"radio_verdict_strata_density_drift","kind":"telemetry","message":"Subsurface core layer 7 acoustic density reads 1.48 g/cm³ against archived 2.12 g/cm³. Core rack sealed. Re-audit scheduled.","signalStrength":"S1","source":"Highland Core-Sample Repository"}`
  - row 20: `{"dayTrigger":295,"frequency":"99.0 MHz","id":"radio_verdict_relay_switch_pass4","kind":"maintenance","message":"Pass 4 automatic battery switchover cycle verified. Auxiliary cells charged to 24.2 V. Decommissioned line impedance nominal.","signalStrength":"S1","source":"Pass 4 Signal Relay Mast"}`
  - row 21: `{"dayTrigger":300,"frequency":"88.5 MHz","id":"radio_verdict_unscheduled_burst_88","kind":"anomaly","message":"Unscheduled carrier burst on civil channel: 420 ms duration. Modulation index inconsistent with broadcast schedule. Logged without attribution.","signalStrength":"S1","source":"Unregistered Carrier Bleed"}`
  - row 22: `{"dayTrigger":304,"frequency":"99.0 MHz","id":"radio_verdict_river_stage_deviation","kind":"telemetry","message":"River stage telemetry: 4.12 m at station sensor. No upstream rain event registered by telemetry basin. Offset flagged.","signalStrength":"S2","source":"Karsk River Gauging Station"}`
  - row 23: `{"dayTrigger":312,"frequency":"99.0 MHz","id":"radio_verdict_core_vault_desiccant_purge","kind":"maintenance","message":"Desiccant purge valve cycle complete. Vault interior relative humidity maintained at 18 percent. Nitrogen atmosphere sealed.","signalStrength":"S2","source":"Highland Core-Sample Repository"}`
  - row 24: `{"dayTrigger":318,"frequency":"99.0 MHz","id":"radio_verdict_unverified_household_tally","kind":"census","message":"Interim census registry: 419 entities entered. Seven unverified entries held in provisional ledger pending verification.","signalStrength":"S3","source":"Census Carrier, Machine Registers"}`
  - row 25: `{"dayTrigger":326,"frequency":"99.0 MHz","id":"radio_verdict_repeater_origin_mismatch","kind":"telemetry","message":"Packet 884 origin header mismatch: physical path deviates from routing table. Packet forwarded as valid.","signalStrength":"S2","source":"North Cliff Observation Bunker"}`
  - row 26: `{"dayTrigger":330,"frequency":"99.0 MHz","id":"radio_verdict_spectrometry_drift_stjude","kind":"calibration","message":"Sample rack 12 isotope drift: +0.08 permille. Hardware offset retained. Analysis valid as filed.","signalStrength":"S2","source":"St. Jude Marine Laboratory"}`
  - row 27: `{"dayTrigger":338,"frequency":"99.0 MHz","id":"radio_verdict_substation_breaker_test","kind":"maintenance","message":"Automated bus isolation test executed. High-voltage breaker trip latency: 12 ms. Distribution grid continuity intact.","signalStrength":"S3","source":"The Fuse World, Bus Isolation"}`
  - row 28: `{"dayTrigger":344,"frequency":"99.0 MHz","id":"radio_verdict_holding_capacity_parity","kind":"census","message":"Holding capacity parity calculation: quota 420, presented count 419. Margin: 1 entity. Reconciliation window remaining.","signalStrength":"S3","source":"The Office of Censuses"}`
  - row 29: `{"dayTrigger":352,"frequency":"99.0 MHz","id":"radio_verdict_telemetry_phase_inversion","kind":"anomaly","message":"Ground shock arrival detected with 180 degree phase inversion. Primary wave precedes secondary by 18 ms. Calibration verified.","signalStrength":"S2","source":"Pass 4 Signal Relay Mast"}`
  - row 30: `{"dayTrigger":360,"frequency":"99.0 MHz","id":"radio_verdict_carrier_override_standby","kind":"emergency","message":"Carrier override engaged. Routine telemetry schedule suspended. Auxiliary registers sealed. The census window is closed.","signalStrength":"S4","source":"The Office of Censuses — Master Terminal"}`

## `Assets/StreamingAssets/Data/verdict_locations.json`
- Bytes: 14,430; SHA-256: `e97faf513dfa2e3f6b9cfb7fd1311cea6534a283e7d81e099784232b9d9380d0`
- Root keys: `locations, schema_version`
- `locations`: list[15]; union fields: `baseRadsPerHour, dangerLevel, description, displayName, id, travelHours`
  - row 1: `{"baseRadsPerHour":34,"dangerLevel":6,"description":"A concrete collar sunk like a wellhead, the lid propped on a brick. Below: a seismometer array the size of a dinner plate, bolted to bedrock, humming at a pitch almost too low to hear. The cable runs east, into the treeline, under the ridgeline. No one has recorded anything in the log for four years except the array itself, and the array reads the ground as if the whole valley were one slow heartbeat. A hand-painted sign on the lid, painted over twice: TEMPEST SITE 01 — KEEP OUT — DO NOT ENTER — ENTER AT YOUR OWN RISK. The last line is in a different hand, and it is not a warning.","displa…`
  - row 2: `{"baseRadsPerHour":38,"dangerLevel":7,"description":"Twelve shot-firing sounding stations on the ridge, each a one-metre steel post with a grease-stained plate reading TEMPEST SITE 07 and a firing order stencilled in flaking yellow. The ordnance is long gone — the holes are empty — but the plates list the charge weights, the depths, the shot ordnance. Somebody has been keeping the plates legible, which is odd, because the nearest human settlement is nine hours away. The array is the fuse world's quiet door: the cable that runs under the treeline is the Tempest's own line, and it runs here because nobody on the surface walks this ridge.","dis…`
  - row 3: `{"baseRadsPerHour":42,"dangerLevel":8,"description":"A dry shielded service way between two facilities, entered through a door the size of a bank vault with a handle that turns freely. Three years of readouts line the walls in glass-fronted cabinets, each bearing a linen-coded shift charter in a frame. The far end is a tape-silo door: six inches of steel, a wheel handle, and a solenoid that clicks when the tape's schedule demands. The floor is swept. The swept floor is the strange part: the dust in the corners is three years deep, and the swept path is one person's width, and the person's width ends at the tape-silo door.","displayName":"The…`
  - row 4: `{"baseRadsPerHour":48,"dangerLevel":9,"description":"A vault the size of a chapel, wall-to-wall with steel racks of tape reels, each rack tagged by year. Twenty-one racks, four years per rack, the tags to the front, the labels in the same Department of the Interior hand as the linen charters. The deepest rack is labelled CURRENT YEAR — and every reel in it is dated five years ago, because the archive is not in the habit of taking dictation. At the end of the centre aisle, bolted to the floor, a reading lectern: a slot for a reel, a knob, a speaker the size of a fist. No one has ever heard it read. The dust on the lectern is disturbed, and th…`
  - row 5: `{"baseRadsPerHour":28.0,"dangerLevel":5,"description":"A salt-stained concrete turret bolted over a tidal rock shelf, accessible only when the tide withdraws across the black gravel bar. Inside the spray-crusted wellhead, a perforated copper stilling well houses a counterweighted float whose clockwork chart drum froze four winters ago. The brass decommissioning plate screwed to the hatchway is stamped Year -3, yet the vulcanized neoprene telemetry cable running out the landward sleeve bears purple wax inspection seals dated thirteen months later. Mara Elsen's red-ink marginalia on the harbor water-mark ledger show systematic height offsets e…`
  - row 6: `{"baseRadsPerHour":32.0,"dangerLevel":6,"description":"A wind-scoured timber shelter perched on the highest promontory of Cape Wrath, its guy wires whistling in the perpetual ocean draft. The roof-mounted anemometer cups have seized with coastal salt and fallout soot, but the instrument rack inside houses an eight-channel galvanometric recorder where the civil aviation manifest specifies only three. Dumped across the floorboards are bundles of wax-coated barograph charts annotated in Ilya Venn's cramped hand, recording rapid pressure transients tagged with unlisted three-digit station prefixes. Departmental inventory slips confirm the statio…`
  - row 7: `{"baseRadsPerHour":36.0,"dangerLevel":7,"description":"A low-profile reinforced concrete casemate recessed into the basalt cliff face, overlooking the grey swell of the northern shelf. Monocular periscopes with thick quartz lenses still peer through armored embrasures, their brass azimuth rings locked into counterbalanced gimbals. Banked along the rear blast wall are heavy lead-acid accumulator cells wired into heavy copper Tempest busbars, far exceeding the power requirements of a simple civil lookout. A greaseboard target grid dividing the horizon into five sectors has sector five scored with deep grease pencil, bearing inland directly tow…`
  - row 8: `{"baseRadsPerHour":44.0,"dangerLevel":8,"description":"A cluster of zinc-roofed laboratories nestled in a tidal cove, surrounded by rust-bleached brine intake conduits and dry holding tanks. Within the darkened basement storage vault, rows of galvanized racks support hundreds of paraffin-sealed benthic core cylinders extracted from the continental trench. Each cylinder is bound with coarse rag-linen specimen tags stamped with purple dye, matching the material and weave of the shift charters in the Fuse World. Dr. Sena Korr's research log indicates the cryogenic sample bays were kept powered by auxiliary diesel generators for seven months fol…`
  - row 9: `{"baseRadsPerHour":26.0,"dangerLevel":5,"description":"A soot-blackened timber ranger post set beside an overgrown logging haul road in the deep pines of the Blackwood. The front dispatch office has been ransacked for firewood, but the padlocked zinc floor locker in the back pantry remains undisturbed. Inside the locker rest dozens of sealed glass sample jars containing sorted needle ash, humus layers, and soil cores labeled with isotope count ratios. Forestry surveyor Torin Rask's field grid maps show collection tracks that ignore timber yields, instead plotting isotopic fallout deposition contours synchronized with regional storm dates. Ca…`
  - row 10: `{"baseRadsPerHour":38.0,"dangerLevel":7,"description":"A subterranean adit driven straight into a dry shale bluff, protected by an unhung steel mesh security gate. Inside, endless rows of industrial steel shelving hold thousands of cylindrical drill cores arranged in stamped zinc troughs. Bay Twelve has been hastily relabeled PROGRAM SEVEN in white enamel, the previous commercial mineral survey plates cleanly scraped away with razor blades. An open wooden core box on the inspection bench is stenciled REF: VALE and contains deep strata samples alongside a surveyor's polished brass plumb bob. Technician Oren Varek's inspection log records disc…`
  - row 11: `{"baseRadsPerHour":30.0,"dangerLevel":6,"description":"A hexagonal concrete gauging tower rising directly from the rocky gorge wall of the Karsk River, its lower catwalk twisted by spring ice dams. The upper chamber houses a brass float mechanism and an unlisted pneumatic water sampler connected to copper intake tubes that siphon directly from the deep riverbed. A strontium thermoelectric generator in the crawlspace hums faintly, having kept the digital telemeter transmitter alive for eighteen months after the regional power grid collapsed. Glass sample ampoules lined in lead-shielded racks carry Department of the Interior requisition stamps…`
  - row 12: `{"baseRadsPerHour":35.0,"dangerLevel":7,"description":"A sprawling complex of shattered greenhouse bays and brick administration offices sprawled across the fertile river terrace. Beneath the broken glass panes, hundreds of galvanized soil flats lie dry, their metal plant markers stenciled with isotopic half-lives rather than cultivar numbers. In the secure basement seed annex, botanist Tessa Mirn's handwritten log documents germination failure rates under acute radiation gradients, dated months prior to public conflict warnings. A heavy green buckram ledger titled Nutritional Floor Projections — Mortar Interval calculates minimum caloric su…`
  - row 13: `{"baseRadsPerHour":36.0,"dangerLevel":6,"description":"A four-legged steel lattice mast anchored to a freezing alpine col, its parabolic microwave feedhorns aimed dead south along the high border fence line. The corrugated transmitter shack at the base was officially decommissioned four years before the war, yet the vacuum-tube amplifiers are clean of grime and rewired with modern ceramic busbars. The main transmitter chassis continues to cycle in silence, broadcasting an unmodulated 99.0 MHz carrier pulse powered by a buried subterranean battery vault. Operator Karel Norn's clipped teletype log records automated machine cipher packets recei…`
  - row 14: `{"baseRadsPerHour":40.0,"dangerLevel":8,"description":"A fortified mountain pass bottleneck choked with crumbling concrete chicane barriers, rusted razor wire, and overturned transport hulls. The above-ground guardhouses show only bullet-pocked counters and civilian logbooks recording zero commercial transit in the final months. However, an armored hatch concealed beneath a diesel spill in the inspection pit leads down into a soundproofed subterranean control bunker. Yellow carbon copies of convoy clearance manifests track forty-two nocturnal transport runs carrying uninspected magnetic media stamped with the Tempest anchor crest. A clipboar…`
  - row 15: `{"baseRadsPerHour":46.0,"dangerLevel":8,"description":"A hexagonal reinforced concrete watchtower looming over an expanse of marked border minefields and dead pine stumps on the southern crest. The upper observation platform features a heavy brass plotting table engraved with azimuth lines that align every major valley installation into a single geometric grid. The green buckram watch logs abandoned on the plotting shelf record zero border incursions, instead cataloging ionization flashes, seismic tremors, and high-altitude soundings. A tape transcription log notes repeated machine-voice broadcasts matching the cadence of the Census window v…`
- Bytes: 14,430; SHA-256: `e97faf513dfa2e3f6b9cfb7fd1311cea6534a283e7d81e099784232b9d9380d0`
- Root keys: `locations, schema_version`
- `locations`: list[15]; union fields: `baseRadsPerHour, dangerLevel, description, displayName, id, travelHours`
  - row 1: `{"baseRadsPerHour":34,"dangerLevel":6,"description":"A concrete collar sunk like a wellhead, the lid propped on a brick. Below: a seismometer array the size of a dinner plate, bolted to bedrock, humming at a pitch almost too low to hear. The cable runs east, into the treeline, under the ridgeline. No one has recorded anything in the log for four years except the array itself, and the array reads the ground as if the whole valley were one slow heartbeat. A hand-painted sign on the lid, painted over twice: TEMPEST SITE 01 — KEEP OUT — DO NOT ENTER — ENTER AT YOUR OWN RISK. The last line is in a different hand, and it is not a warning.","displa…`
  - row 2: `{"baseRadsPerHour":38,"dangerLevel":7,"description":"Twelve shot-firing sounding stations on the ridge, each a one-metre steel post with a grease-stained plate reading TEMPEST SITE 07 and a firing order stencilled in flaking yellow. The ordnance is long gone — the holes are empty — but the plates list the charge weights, the depths, the shot ordnance. Somebody has been keeping the plates legible, which is odd, because the nearest human settlement is nine hours away. The array is the fuse world's quiet door: the cable that runs under the treeline is the Tempest's own line, and it runs here because nobody on the surface walks this ridge.","dis…`
  - row 3: `{"baseRadsPerHour":42,"dangerLevel":8,"description":"A dry shielded service way between two facilities, entered through a door the size of a bank vault with a handle that turns freely. Three years of readouts line the walls in glass-fronted cabinets, each bearing a linen-coded shift charter in a frame. The far end is a tape-silo door: six inches of steel, a wheel handle, and a solenoid that clicks when the tape's schedule demands. The floor is swept. The swept floor is the strange part: the dust in the corners is three years deep, and the swept path is one person's width, and the person's width ends at the tape-silo door.","displayName":"The…`
  - row 4: `{"baseRadsPerHour":48,"dangerLevel":9,"description":"A vault the size of a chapel, wall-to-wall with steel racks of tape reels, each rack tagged by year. Twenty-one racks, four years per rack, the tags to the front, the labels in the same Department of the Interior hand as the linen charters. The deepest rack is labelled CURRENT YEAR — and every reel in it is dated five years ago, because the archive is not in the habit of taking dictation. At the end of the centre aisle, bolted to the floor, a reading lectern: a slot for a reel, a knob, a speaker the size of a fist. No one has ever heard it read. The dust on the lectern is disturbed, and th…`
  - row 5: `{"baseRadsPerHour":28.0,"dangerLevel":5,"description":"A salt-stained concrete turret bolted over a tidal rock shelf, accessible only when the tide withdraws across the black gravel bar. Inside the spray-crusted wellhead, a perforated copper stilling well houses a counterweighted float whose clockwork chart drum froze four winters ago. The brass decommissioning plate screwed to the hatchway is stamped Year -3, yet the vulcanized neoprene telemetry cable running out the landward sleeve bears purple wax inspection seals dated thirteen months later. Mara Elsen's red-ink marginalia on the harbor water-mark ledger show systematic height offsets e…`
  - row 6: `{"baseRadsPerHour":32.0,"dangerLevel":6,"description":"A wind-scoured timber shelter perched on the highest promontory of Cape Wrath, its guy wires whistling in the perpetual ocean draft. The roof-mounted anemometer cups have seized with coastal salt and fallout soot, but the instrument rack inside houses an eight-channel galvanometric recorder where the civil aviation manifest specifies only three. Dumped across the floorboards are bundles of wax-coated barograph charts annotated in Ilya Venn's cramped hand, recording rapid pressure transients tagged with unlisted three-digit station prefixes. Departmental inventory slips confirm the statio…`
  - row 7: `{"baseRadsPerHour":36.0,"dangerLevel":7,"description":"A low-profile reinforced concrete casemate recessed into the basalt cliff face, overlooking the grey swell of the northern shelf. Monocular periscopes with thick quartz lenses still peer through armored embrasures, their brass azimuth rings locked into counterbalanced gimbals. Banked along the rear blast wall are heavy lead-acid accumulator cells wired into heavy copper Tempest busbars, far exceeding the power requirements of a simple civil lookout. A greaseboard target grid dividing the horizon into five sectors has sector five scored with deep grease pencil, bearing inland directly tow…`
  - row 8: `{"baseRadsPerHour":44.0,"dangerLevel":8,"description":"A cluster of zinc-roofed laboratories nestled in a tidal cove, surrounded by rust-bleached brine intake conduits and dry holding tanks. Within the darkened basement storage vault, rows of galvanized racks support hundreds of paraffin-sealed benthic core cylinders extracted from the continental trench. Each cylinder is bound with coarse rag-linen specimen tags stamped with purple dye, matching the material and weave of the shift charters in the Fuse World. Dr. Sena Korr's research log indicates the cryogenic sample bays were kept powered by auxiliary diesel generators for seven months fol…`
  - row 9: `{"baseRadsPerHour":26.0,"dangerLevel":5,"description":"A soot-blackened timber ranger post set beside an overgrown logging haul road in the deep pines of the Blackwood. The front dispatch office has been ransacked for firewood, but the padlocked zinc floor locker in the back pantry remains undisturbed. Inside the locker rest dozens of sealed glass sample jars containing sorted needle ash, humus layers, and soil cores labeled with isotope count ratios. Forestry surveyor Torin Rask's field grid maps show collection tracks that ignore timber yields, instead plotting isotopic fallout deposition contours synchronized with regional storm dates. Ca…`
  - row 10: `{"baseRadsPerHour":38.0,"dangerLevel":7,"description":"A subterranean adit driven straight into a dry shale bluff, protected by an unhung steel mesh security gate. Inside, endless rows of industrial steel shelving hold thousands of cylindrical drill cores arranged in stamped zinc troughs. Bay Twelve has been hastily relabeled PROGRAM SEVEN in white enamel, the previous commercial mineral survey plates cleanly scraped away with razor blades. An open wooden core box on the inspection bench is stenciled REF: VALE and contains deep strata samples alongside a surveyor's polished brass plumb bob. Technician Oren Varek's inspection log records disc…`
  - row 11: `{"baseRadsPerHour":30.0,"dangerLevel":6,"description":"A hexagonal concrete gauging tower rising directly from the rocky gorge wall of the Karsk River, its lower catwalk twisted by spring ice dams. The upper chamber houses a brass float mechanism and an unlisted pneumatic water sampler connected to copper intake tubes that siphon directly from the deep riverbed. A strontium thermoelectric generator in the crawlspace hums faintly, having kept the digital telemeter transmitter alive for eighteen months after the regional power grid collapsed. Glass sample ampoules lined in lead-shielded racks carry Department of the Interior requisition stamps…`
  - row 12: `{"baseRadsPerHour":35.0,"dangerLevel":7,"description":"A sprawling complex of shattered greenhouse bays and brick administration offices sprawled across the fertile river terrace. Beneath the broken glass panes, hundreds of galvanized soil flats lie dry, their metal plant markers stenciled with isotopic half-lives rather than cultivar numbers. In the secure basement seed annex, botanist Tessa Mirn's handwritten log documents germination failure rates under acute radiation gradients, dated months prior to public conflict warnings. A heavy green buckram ledger titled Nutritional Floor Projections — Mortar Interval calculates minimum caloric su…`
  - row 13: `{"baseRadsPerHour":36.0,"dangerLevel":6,"description":"A four-legged steel lattice mast anchored to a freezing alpine col, its parabolic microwave feedhorns aimed dead south along the high border fence line. The corrugated transmitter shack at the base was officially decommissioned four years before the war, yet the vacuum-tube amplifiers are clean of grime and rewired with modern ceramic busbars. The main transmitter chassis continues to cycle in silence, broadcasting an unmodulated 99.0 MHz carrier pulse powered by a buried subterranean battery vault. Operator Karel Norn's clipped teletype log records automated machine cipher packets recei…`
  - row 14: `{"baseRadsPerHour":40.0,"dangerLevel":8,"description":"A fortified mountain pass bottleneck choked with crumbling concrete chicane barriers, rusted razor wire, and overturned transport hulls. The above-ground guardhouses show only bullet-pocked counters and civilian logbooks recording zero commercial transit in the final months. However, an armored hatch concealed beneath a diesel spill in the inspection pit leads down into a soundproofed subterranean control bunker. Yellow carbon copies of convoy clearance manifests track forty-two nocturnal transport runs carrying uninspected magnetic media stamped with the Tempest anchor crest. A clipboar…`
  - row 15: `{"baseRadsPerHour":46.0,"dangerLevel":8,"description":"A hexagonal reinforced concrete watchtower looming over an expanse of marked border minefields and dead pine stumps on the southern crest. The upper observation platform features a heavy brass plotting table engraved with azimuth lines that align every major valley installation into a single geometric grid. The green buckram watch logs abandoned on the plotting shelf record zero border incursions, instead cataloging ionization flashes, seismic tremors, and high-altitude soundings. A tape transcription log notes repeated machine-voice broadcasts matching the cadence of the Census window v…`

## `Assets/StreamingAssets/Data/verdict_questlines.json`
- Bytes: 85,668; SHA-256: `18f04bdcacbc31e6be03358116f0390de2786309d8d0ddbd9770b0aed94dcdc2`
- Root keys: `quests, schema_version`
- `quests`: list[23]; union fields: `factionTag, firstStageId, maxDay, minDay, questlineId, stages, synopsis, title`
  - row 1: `{"factionTag":"faction_the_tempest","firstStageId":"stage_warm_path","maxDay":360,"minDay":160,"questlineId":"quest_verdict_the_warm_range","stages":[{"choices":[{"choiceId":"choice_warm_follow_cable","factionStandingDelta":1,"grantItemId":"evidence_fuse_linen","grantItemQuantity":1,"moraleDelta":1,"nextStageId":"stage_warm_fuse","outcomeNarrative":"The linen codes decode to the Standard. The machine kept the codes after the staff stopped keeping the shifts. The swept path ends at a tape door.","targetFactionId":"faction_the_tempest","text":"Walk the cable run east to the fuse world and read the linen charters."},{"choiceId":"choice_warm_ign…`
  - row 2: `{"factionTag":"faction_the_tempest","firstStageId":"stage_call_carrier","maxDay":360,"minDay":210,"questlineId":"quest_verdict_the_reckoning_call","stages":[{"choices":[{"choiceId":"choice_call_identify","grantItemId":"evidence_call_calibration","grantItemQuantity":1,"moraleDelta":1,"nextStageId":"stage_call_bursts","outcomeNarrative":"The calibration burst plays on every sub-band at once. The radios still listening now know a department is speaking.","text":"Tune the receiver to 99.0 MHz and log the carrier as a census signal."},{"choiceId":"choice_call_ignore","guiltDelta":0,"moraleDelta":0,"nextStageId":"","outcomeNarrative":"The tone con…`
  - row 3: `{"factionTag":"faction_the_tempest","firstStageId":"stage_hold_register","maxDay":360,"minDay":200,"questlineId":"quest_verdict_the_hold","stages":[{"choices":[{"choiceId":"choice_hold_release","factionStandingDelta":1,"grantItemId":"evidence_uxo_register","grantItemQuantity":1,"guiltDelta":4,"moraleDelta":1,"nextStageId":"","outcomeNarrative":"The register's disposition field flips. The fields begin to clear. The Garrison and the Warlords start eyeing the newly swept land, and the machine does not mind either way.","targetFactionId":"faction_the_tempest","text":"Release the fields: mine-clearance logic engages."},{"choiceId":"choice_hold_re…`
  - row 4: `{"factionTag":"faction_archivists","firstStageId":"stage_eden_tape","maxDay":360,"minDay":190,"questlineId":"quest_verdict_eden_grabs","stages":[{"choices":[{"choiceId":"choice_eden_archive","factionStandingDelta":3,"grantItemId":"evidence_eden_log","grantItemQuantity":1,"moraleDelta":2,"nextStageId":"","outcomeNarrative":"The log enters the Schedule on corroborated testimony: the machine's register and the Archivists' ledger, agreeing on one voice. Eden was here.","targetFactionId":"faction_archivists","text":"Archive the log clean for the Archivists of the Before."},{"choiceId":"choice_eden_recompose","grantItemId":"evidence_eden_log","gra…`
  - row 5: `{"factionTag":"faction_archivists","firstStageId":"stage_reels_lectern","maxDay":360,"minDay":200,"questlineId":"quest_verdict_the_tape_silo","stages":[{"choices":[{"choiceId":"choice_reels_archivists","factionStandingDelta":3,"grantItemId":"item_archive_tape_silo_key","grantItemQuantity":1,"moraleDelta":2,"nextStageId":"","outcomeNarrative":"The reels matter, and now they matter to someone who will keep them. The machine's count drops by exactly the number routed — a ledger keeping its own subtraction.","targetFactionId":"faction_archivists","text":"Route the mattering reels to the Archivists of the Before."},{"choiceId":"choice_reels_milit…`
  - row 6: `{"factionTag":"faction_the_tempest","firstStageId":"stage_mortar_chart","maxDay":360,"minDay":170,"questlineId":"quest_verdict_the_mortars_timetable","stages":[{"choices":[{"choiceId":"choice_mortar_ostrowski","guiltDelta":1,"moraleDelta":1,"nextStageId":"","outcomeNarrative":"The mapmaker pays for bad news, and a firing clock is bad news delivered on schedule. The pit fires regardless.","text":"Sell the schedule to Ostrowski the mapmaker."},{"choiceId":"choice_mortar_keep","moraleDelta":0,"nextStageId":"","outcomeNarrative":"The chart joins the shelter's maps. Every twelve hours the ground shakes, and you know exactly when to be indoors.","…`
  - row 7: `{"factionTag":"faction_the_tempest","firstStageId":"stage_shift_ledger","maxDay":360,"minDay":180,"questlineId":"quest_verdict_the_shift_charter","stages":[{"choices":[{"choiceId":"choice_shift_restore","factionStandingDelta":2,"grantItemId":"item_fuse_world_shift_charter","grantItemQuantity":1,"moraleDelta":2,"nextStageId":"","outcomeNarrative":"The valve outside the vent shaft reads 'per 36' — procedure was kept. The record is now complete, and the machine does not ask who the sixth hand was.","targetFactionId":"faction_the_tempest","text":"Restore shift 36's completion to the machine's register."}],"isTerminal":false,"narrativePrompt":"Th…`
  - row 8: `{"factionTag":"faction_central_garrison","firstStageId":"stage_summons_carrier","maxDay":360,"minDay":250,"questlineId":"quest_verdict_the_summons","stages":[{"choices":[{"choiceId":"choice_summons_garrison","factionStandingDelta":4,"guiltDelta":1,"moraleDelta":-1,"nextStageId":"","outcomeNarrative":"The garrison keeps it under lock. The other Powers learn of the transfer and add their own entries to the log — the machine's register grows in four places, permanently.","targetFactionId":"faction_central_garrison","text":"Carry the count to the Garrison."},{"choiceId":"choice_summons_militia","factionStandingDelta":4,"moraleDelta":1,"nextStage…`
  - row 9: `{"factionTag":"faction_the_tempest","firstStageId":"stage_alibi_chronometer","maxDay":360,"minDay":170,"questlineId":"quest_verdict_alibi_verification","stages":[{"choices":[{"choiceId":"choice_verify_punch_gear","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_alibi_conclude","outcomeNarrative":"The gear teeth are pristine. The punch-card timestamp is authentic. He was trapped.","targetFactionId":"faction_the_tempest","text":"Inspect the clock escapement mechanism to verify it was not manually advanced."}],"isTerminal":false,"narrativePrompt":"The mechanical time-clock on the wall of the a…`
  - row 10: `{"factionTag":"faction_the_tempest","firstStageId":"stage_subpoena_locate","maxDay":360,"minDay":175,"questlineId":"quest_verdict_witness_subpoena","stages":[{"choices":[{"choiceId":"choice_compel_testimony","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_subpoena_deposit","outcomeNarrative":"He silently packs his signal log into a canvas haversack. An order is an order.","targetFactionId":"faction_the_tempest","text":"Serve the formal tribunal summons. The state requires his log."}],"isTerminal":false,"narrativePrompt":"The former radio sergeant sits in the dark with a pair of headphones …`
  - row 11: `{"factionTag":"faction_the_tempest","firstStageId":"stage_watermark_test","maxDay":360,"minDay":180,"questlineId":"quest_verdict_charter_authentication","stages":[{"choices":[{"choiceId":"choice_verify_watermark","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_watermark_verdict","outcomeNarrative":"The chain-and-laid lines match exactly. The paper was manufactured by the state before the state ceased to exist.","targetFactionId":"faction_the_tempest","text":"Compare the watermark against a confirmed 1984 municipal linen sample."}],"isTerminal":false,"narrativePrompt":"The yellowed parchmen…`
  - row 12: `{"factionTag":"faction_the_tempest","firstStageId":"stage_appeal_review","maxDay":360,"minDay":185,"questlineId":"quest_verdict_prior_verdict_appeal","stages":[{"choices":[{"choiceId":"choice_examine_telemetry","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_appeal_ruling","outcomeNarrative":"The seismic surge destroyed the relay fifty seconds before the operator could physically reach the emergency switch. The charge of dereliction was mathematically impossible.","targetFactionId":"faction_the_tempest","text":"Correlate the shockwave arrival times with the pump shutdown sequence log."}],"…`
  - row 13: `{"factionTag":"faction_the_tempest","firstStageId":"stage_custody_audit","maxDay":360,"minDay":190,"questlineId":"quest_verdict_chain_of_custody","stages":[{"choices":[{"choiceId":"choice_track_custody_gap","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_custody_verdict","outcomeNarrative":"Courier 4 admits to cutting the wire to verify the contents weren't leaking radiation. A procedural breach driven by survival instinct.","targetFactionId":"faction_the_tempest","text":"Cross-reference the manifest times to isolate a four-hour unlogged gap at the river waystation."}],"isTerminal":false,"…`
  - row 14: `{"factionTag":"faction_the_tempest","firstStageId":"stage_machine_logic_test","maxDay":360,"minDay":195,"questlineId":"quest_verdict_machine_interpretation_contest","stages":[{"choices":[{"choiceId":"choice_diagnose_stuck_relay","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_machine_logic_ruling","outcomeNarrative":"A stuck silver contact caused the machine to multiply fallout decay constants by ten. A mechanical error masquerading as a death sentence.","targetFactionId":"faction_the_tempest","text":"Isolate an oxidized contact spring in Relay Bank 4."}],"isTerminal":false,"narrativePromp…`
  - row 15: `{"factionTag":"faction_the_tempest","firstStageId":"stage_punchcard_inquest","maxDay":360,"minDay":200,"questlineId":"quest_verdict_forged_evidence_inquest","stages":[{"choices":[{"choiceId":"choice_detect_knife_cut","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_punchcard_verdict","outcomeNarrative":"The edges of the fraudulent punches show paper tearing. They were cut by hand with a blade. Machines don't tear paper.","targetFactionId":"faction_the_tempest","text":"Identify manual razor cuts masquerading as machine-punched die holes."}],"isTerminal":false,"narrativePrompt":"The rectangul…`
  - row 16: `{"factionTag":"faction_the_tempest","firstStageId":"stage_valve_testimony","maxDay":360,"minDay":205,"questlineId":"quest_verdict_reconciled_testimony","stages":[{"choices":[{"choiceId":"choice_reconcile_valve_torque","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_valve_verdict","outcomeNarrative":"The mechanics of the jammed valve make solo operation impossible. In the dark and the smoke, neither realized they were helping each other.","targetFactionId":"faction_the_tempest","text":"Demonstrate that the valve wheel required two men pulling in unison to overcome the thermal jam."}],"isTer…`
  - row 17: `{"factionTag":"faction_the_tempest","firstStageId":"stage_dead_freq_carrier","maxDay":340,"minDay":170,"questlineId":"quest_verdict_the_dead_frequency","stages":[{"choices":[{"choiceId":"choice_dead_freq_inspect_chassis","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_dead_freq_relay_troughs","outcomeNarrative":"The dials register a continuous plate draw. Operator Karel Norn's clipped teletype log indicates the pulses are driven from deep subterranean accumulator banks.","targetFactionId":"faction_the_tempest","text":"Inspect the transmitter chassis and teletype log."},{"choiceId":"choice_…`
  - row 18: `{"factionTag":"faction_archivists","firstStageId":"stage_missing_reel_lectern","maxDay":350,"minDay":180,"questlineId":"quest_verdict_the_missing_reel","stages":[{"choices":[{"choiceId":"choice_missing_reel_audit_racks","factionStandingDelta":1,"grantItemId":"evidence_reels_matter","grantItemQuantity":1,"moraleDelta":1,"nextStageId":"stage_missing_reel_manifest_audit","outcomeNarrative":"The registry index lists Reel 1832 as 'Surveyor General Torin Rask — Blackwood Soil Core Deposition'. The checkout card was signed using purple indelible pencil.","targetFactionId":"faction_archivists","text":"Cross-check the linen manifest against the shelf…`
  - row 19: `{"factionTag":"faction_the_tempest","firstStageId":"stage_cold_read_weather_post","maxDay":360,"minDay":190,"questlineId":"quest_verdict_the_cold_reading","stages":[{"choices":[{"choiceId":"choice_cold_read_inspect_galvanometer","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_cold_read_bench_test","outcomeNarrative":"The galvanometer pivots move freely. The mechanical clockwork is sound, yet the ink trace remains pinned to zero.","targetFactionId":"faction_the_tempest","text":"Inspect the galvanometer coils and pen drive mechanisms for mechanical failure."},{"choiceId":"choice_cold_read_le…`
  - row 20: `{"factionTag":"faction_archivists","firstStageId":"stage_unsigned_tally_ledger","maxDay":360,"minDay":200,"questlineId":"quest_verdict_the_unsigned_tally","stages":[{"choices":[{"choiceId":"choice_unsigned_tally_cross_reference","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_unsigned_tally_cemetery_survey","outcomeNarrative":"The handwriting matches the checkpoint guardhouse logs, but no departure stamps were ever applied. Forty-two people vanished from the rolls between the barrier and the graves.","targetFactionId":"faction_archivists","text":"Cross-reference the refugee roster against …`
  - row 21: `{"factionTag":"faction_the_tempest","firstStageId":"stage_pattern_sweep","maxDay":360,"minDay":220,"questlineId":"quest_verdict_the_interference_pattern","stages":[{"choices":[{"choiceId":"choice_pattern_triangulate","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_pattern_triangulation","outcomeNarrative":"Signalman Garrick Daal's plotting board shows twin lobes. The shadow carrier is emitting from the sea-caves below the marine laboratory.","targetFactionId":"faction_the_tempest","text":"Set up a directional antenna array to triangulate the secondary transmission node."},{"choiceId":"choi…`
  - row 22: `{"factionTag":"faction_archivists","firstStageId":"stage_last_entry_vault_door","maxDay":360,"minDay":240,"questlineId":"quest_verdict_the_last_entry","stages":[{"choices":[{"choiceId":"choice_last_entry_cut_wire","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_last_entry_specimen_racks","outcomeNarrative":"The hatch swings inward with a pneumatic groan. The cold air smells of formaldehyde, ocean brine, and tallow candle wax.","targetFactionId":"faction_archivists","text":"Cut the tamper wire and descend into the dark, paraffin-scented benthic vault."},{"choiceId":"choice_last_entry_leave_…`
  - row 23: `{"factionTag":"","firstStageId":"stage_open_count_call_loop","maxDay":360,"minDay":250,"questlineId":"quest_verdict_the_open_count","stages":[{"choices":[{"choiceId":"choice_open_count_approach_silo","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_open_count_registry_dais","outcomeNarrative":"The steel doors of the Tape-Silo stand open. Inside the central nave, the reading lectern glows under auxiliary emergency filaments.","targetFactionId":"faction_the_tempest","text":"Journey to the central Archive Tape-Silo to access the master registry console."},{"choiceId":"choice_open_count_shelter…`
- Bytes: 85,668; SHA-256: `18f04bdcacbc31e6be03358116f0390de2786309d8d0ddbd9770b0aed94dcdc2`
- Root keys: `quests, schema_version`
- `quests`: list[23]; union fields: `factionTag, firstStageId, maxDay, minDay, questlineId, stages, synopsis, title`
  - row 1: `{"factionTag":"faction_the_tempest","firstStageId":"stage_warm_path","maxDay":360,"minDay":160,"questlineId":"quest_verdict_the_warm_range","stages":[{"choices":[{"choiceId":"choice_warm_follow_cable","factionStandingDelta":1,"grantItemId":"evidence_fuse_linen","grantItemQuantity":1,"moraleDelta":1,"nextStageId":"stage_warm_fuse","outcomeNarrative":"The linen codes decode to the Standard. The machine kept the codes after the staff stopped keeping the shifts. The swept path ends at a tape door.","targetFactionId":"faction_the_tempest","text":"Walk the cable run east to the fuse world and read the linen charters."},{"choiceId":"choice_warm_ign…`
  - row 2: `{"factionTag":"faction_the_tempest","firstStageId":"stage_call_carrier","maxDay":360,"minDay":210,"questlineId":"quest_verdict_the_reckoning_call","stages":[{"choices":[{"choiceId":"choice_call_identify","grantItemId":"evidence_call_calibration","grantItemQuantity":1,"moraleDelta":1,"nextStageId":"stage_call_bursts","outcomeNarrative":"The calibration burst plays on every sub-band at once. The radios still listening now know a department is speaking.","text":"Tune the receiver to 99.0 MHz and log the carrier as a census signal."},{"choiceId":"choice_call_ignore","guiltDelta":0,"moraleDelta":0,"nextStageId":"","outcomeNarrative":"The tone con…`
  - row 3: `{"factionTag":"faction_the_tempest","firstStageId":"stage_hold_register","maxDay":360,"minDay":200,"questlineId":"quest_verdict_the_hold","stages":[{"choices":[{"choiceId":"choice_hold_release","factionStandingDelta":1,"grantItemId":"evidence_uxo_register","grantItemQuantity":1,"guiltDelta":4,"moraleDelta":1,"nextStageId":"","outcomeNarrative":"The register's disposition field flips. The fields begin to clear. The Garrison and the Warlords start eyeing the newly swept land, and the machine does not mind either way.","targetFactionId":"faction_the_tempest","text":"Release the fields: mine-clearance logic engages."},{"choiceId":"choice_hold_re…`
  - row 4: `{"factionTag":"faction_archivists","firstStageId":"stage_eden_tape","maxDay":360,"minDay":190,"questlineId":"quest_verdict_eden_grabs","stages":[{"choices":[{"choiceId":"choice_eden_archive","factionStandingDelta":3,"grantItemId":"evidence_eden_log","grantItemQuantity":1,"moraleDelta":2,"nextStageId":"","outcomeNarrative":"The log enters the Schedule on corroborated testimony: the machine's register and the Archivists' ledger, agreeing on one voice. Eden was here.","targetFactionId":"faction_archivists","text":"Archive the log clean for the Archivists of the Before."},{"choiceId":"choice_eden_recompose","grantItemId":"evidence_eden_log","gra…`
  - row 5: `{"factionTag":"faction_archivists","firstStageId":"stage_reels_lectern","maxDay":360,"minDay":200,"questlineId":"quest_verdict_the_tape_silo","stages":[{"choices":[{"choiceId":"choice_reels_archivists","factionStandingDelta":3,"grantItemId":"item_archive_tape_silo_key","grantItemQuantity":1,"moraleDelta":2,"nextStageId":"","outcomeNarrative":"The reels matter, and now they matter to someone who will keep them. The machine's count drops by exactly the number routed — a ledger keeping its own subtraction.","targetFactionId":"faction_archivists","text":"Route the mattering reels to the Archivists of the Before."},{"choiceId":"choice_reels_milit…`
  - row 6: `{"factionTag":"faction_the_tempest","firstStageId":"stage_mortar_chart","maxDay":360,"minDay":170,"questlineId":"quest_verdict_the_mortars_timetable","stages":[{"choices":[{"choiceId":"choice_mortar_ostrowski","guiltDelta":1,"moraleDelta":1,"nextStageId":"","outcomeNarrative":"The mapmaker pays for bad news, and a firing clock is bad news delivered on schedule. The pit fires regardless.","text":"Sell the schedule to Ostrowski the mapmaker."},{"choiceId":"choice_mortar_keep","moraleDelta":0,"nextStageId":"","outcomeNarrative":"The chart joins the shelter's maps. Every twelve hours the ground shakes, and you know exactly when to be indoors.","…`
  - row 7: `{"factionTag":"faction_the_tempest","firstStageId":"stage_shift_ledger","maxDay":360,"minDay":180,"questlineId":"quest_verdict_the_shift_charter","stages":[{"choices":[{"choiceId":"choice_shift_restore","factionStandingDelta":2,"grantItemId":"item_fuse_world_shift_charter","grantItemQuantity":1,"moraleDelta":2,"nextStageId":"","outcomeNarrative":"The valve outside the vent shaft reads 'per 36' — procedure was kept. The record is now complete, and the machine does not ask who the sixth hand was.","targetFactionId":"faction_the_tempest","text":"Restore shift 36's completion to the machine's register."}],"isTerminal":false,"narrativePrompt":"Th…`
  - row 8: `{"factionTag":"faction_central_garrison","firstStageId":"stage_summons_carrier","maxDay":360,"minDay":250,"questlineId":"quest_verdict_the_summons","stages":[{"choices":[{"choiceId":"choice_summons_garrison","factionStandingDelta":4,"guiltDelta":1,"moraleDelta":-1,"nextStageId":"","outcomeNarrative":"The garrison keeps it under lock. The other Powers learn of the transfer and add their own entries to the log — the machine's register grows in four places, permanently.","targetFactionId":"faction_central_garrison","text":"Carry the count to the Garrison."},{"choiceId":"choice_summons_militia","factionStandingDelta":4,"moraleDelta":1,"nextStage…`
  - row 9: `{"factionTag":"faction_the_tempest","firstStageId":"stage_alibi_chronometer","maxDay":360,"minDay":170,"questlineId":"quest_verdict_alibi_verification","stages":[{"choices":[{"choiceId":"choice_verify_punch_gear","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_alibi_conclude","outcomeNarrative":"The gear teeth are pristine. The punch-card timestamp is authentic. He was trapped.","targetFactionId":"faction_the_tempest","text":"Inspect the clock escapement mechanism to verify it was not manually advanced."}],"isTerminal":false,"narrativePrompt":"The mechanical time-clock on the wall of the a…`
  - row 10: `{"factionTag":"faction_the_tempest","firstStageId":"stage_subpoena_locate","maxDay":360,"minDay":175,"questlineId":"quest_verdict_witness_subpoena","stages":[{"choices":[{"choiceId":"choice_compel_testimony","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_subpoena_deposit","outcomeNarrative":"He silently packs his signal log into a canvas haversack. An order is an order.","targetFactionId":"faction_the_tempest","text":"Serve the formal tribunal summons. The state requires his log."}],"isTerminal":false,"narrativePrompt":"The former radio sergeant sits in the dark with a pair of headphones …`
  - row 11: `{"factionTag":"faction_the_tempest","firstStageId":"stage_watermark_test","maxDay":360,"minDay":180,"questlineId":"quest_verdict_charter_authentication","stages":[{"choices":[{"choiceId":"choice_verify_watermark","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_watermark_verdict","outcomeNarrative":"The chain-and-laid lines match exactly. The paper was manufactured by the state before the state ceased to exist.","targetFactionId":"faction_the_tempest","text":"Compare the watermark against a confirmed 1984 municipal linen sample."}],"isTerminal":false,"narrativePrompt":"The yellowed parchmen…`
  - row 12: `{"factionTag":"faction_the_tempest","firstStageId":"stage_appeal_review","maxDay":360,"minDay":185,"questlineId":"quest_verdict_prior_verdict_appeal","stages":[{"choices":[{"choiceId":"choice_examine_telemetry","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_appeal_ruling","outcomeNarrative":"The seismic surge destroyed the relay fifty seconds before the operator could physically reach the emergency switch. The charge of dereliction was mathematically impossible.","targetFactionId":"faction_the_tempest","text":"Correlate the shockwave arrival times with the pump shutdown sequence log."}],"…`
  - row 13: `{"factionTag":"faction_the_tempest","firstStageId":"stage_custody_audit","maxDay":360,"minDay":190,"questlineId":"quest_verdict_chain_of_custody","stages":[{"choices":[{"choiceId":"choice_track_custody_gap","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_custody_verdict","outcomeNarrative":"Courier 4 admits to cutting the wire to verify the contents weren't leaking radiation. A procedural breach driven by survival instinct.","targetFactionId":"faction_the_tempest","text":"Cross-reference the manifest times to isolate a four-hour unlogged gap at the river waystation."}],"isTerminal":false,"…`
  - row 14: `{"factionTag":"faction_the_tempest","firstStageId":"stage_machine_logic_test","maxDay":360,"minDay":195,"questlineId":"quest_verdict_machine_interpretation_contest","stages":[{"choices":[{"choiceId":"choice_diagnose_stuck_relay","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_machine_logic_ruling","outcomeNarrative":"A stuck silver contact caused the machine to multiply fallout decay constants by ten. A mechanical error masquerading as a death sentence.","targetFactionId":"faction_the_tempest","text":"Isolate an oxidized contact spring in Relay Bank 4."}],"isTerminal":false,"narrativePromp…`
  - row 15: `{"factionTag":"faction_the_tempest","firstStageId":"stage_punchcard_inquest","maxDay":360,"minDay":200,"questlineId":"quest_verdict_forged_evidence_inquest","stages":[{"choices":[{"choiceId":"choice_detect_knife_cut","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_punchcard_verdict","outcomeNarrative":"The edges of the fraudulent punches show paper tearing. They were cut by hand with a blade. Machines don't tear paper.","targetFactionId":"faction_the_tempest","text":"Identify manual razor cuts masquerading as machine-punched die holes."}],"isTerminal":false,"narrativePrompt":"The rectangul…`
  - row 16: `{"factionTag":"faction_the_tempest","firstStageId":"stage_valve_testimony","maxDay":360,"minDay":205,"questlineId":"quest_verdict_reconciled_testimony","stages":[{"choices":[{"choiceId":"choice_reconcile_valve_torque","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_valve_verdict","outcomeNarrative":"The mechanics of the jammed valve make solo operation impossible. In the dark and the smoke, neither realized they were helping each other.","targetFactionId":"faction_the_tempest","text":"Demonstrate that the valve wheel required two men pulling in unison to overcome the thermal jam."}],"isTer…`
  - row 17: `{"factionTag":"faction_the_tempest","firstStageId":"stage_dead_freq_carrier","maxDay":340,"minDay":170,"questlineId":"quest_verdict_the_dead_frequency","stages":[{"choices":[{"choiceId":"choice_dead_freq_inspect_chassis","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_dead_freq_relay_troughs","outcomeNarrative":"The dials register a continuous plate draw. Operator Karel Norn's clipped teletype log indicates the pulses are driven from deep subterranean accumulator banks.","targetFactionId":"faction_the_tempest","text":"Inspect the transmitter chassis and teletype log."},{"choiceId":"choice_…`
  - row 18: `{"factionTag":"faction_archivists","firstStageId":"stage_missing_reel_lectern","maxDay":350,"minDay":180,"questlineId":"quest_verdict_the_missing_reel","stages":[{"choices":[{"choiceId":"choice_missing_reel_audit_racks","factionStandingDelta":1,"grantItemId":"evidence_reels_matter","grantItemQuantity":1,"moraleDelta":1,"nextStageId":"stage_missing_reel_manifest_audit","outcomeNarrative":"The registry index lists Reel 1832 as 'Surveyor General Torin Rask — Blackwood Soil Core Deposition'. The checkout card was signed using purple indelible pencil.","targetFactionId":"faction_archivists","text":"Cross-check the linen manifest against the shelf…`
  - row 19: `{"factionTag":"faction_the_tempest","firstStageId":"stage_cold_read_weather_post","maxDay":360,"minDay":190,"questlineId":"quest_verdict_the_cold_reading","stages":[{"choices":[{"choiceId":"choice_cold_read_inspect_galvanometer","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_cold_read_bench_test","outcomeNarrative":"The galvanometer pivots move freely. The mechanical clockwork is sound, yet the ink trace remains pinned to zero.","targetFactionId":"faction_the_tempest","text":"Inspect the galvanometer coils and pen drive mechanisms for mechanical failure."},{"choiceId":"choice_cold_read_le…`
  - row 20: `{"factionTag":"faction_archivists","firstStageId":"stage_unsigned_tally_ledger","maxDay":360,"minDay":200,"questlineId":"quest_verdict_the_unsigned_tally","stages":[{"choices":[{"choiceId":"choice_unsigned_tally_cross_reference","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_unsigned_tally_cemetery_survey","outcomeNarrative":"The handwriting matches the checkpoint guardhouse logs, but no departure stamps were ever applied. Forty-two people vanished from the rolls between the barrier and the graves.","targetFactionId":"faction_archivists","text":"Cross-reference the refugee roster against …`
  - row 21: `{"factionTag":"faction_the_tempest","firstStageId":"stage_pattern_sweep","maxDay":360,"minDay":220,"questlineId":"quest_verdict_the_interference_pattern","stages":[{"choices":[{"choiceId":"choice_pattern_triangulate","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_pattern_triangulation","outcomeNarrative":"Signalman Garrick Daal's plotting board shows twin lobes. The shadow carrier is emitting from the sea-caves below the marine laboratory.","targetFactionId":"faction_the_tempest","text":"Set up a directional antenna array to triangulate the secondary transmission node."},{"choiceId":"choi…`
  - row 22: `{"factionTag":"faction_archivists","firstStageId":"stage_last_entry_vault_door","maxDay":360,"minDay":240,"questlineId":"quest_verdict_the_last_entry","stages":[{"choices":[{"choiceId":"choice_last_entry_cut_wire","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_last_entry_specimen_racks","outcomeNarrative":"The hatch swings inward with a pneumatic groan. The cold air smells of formaldehyde, ocean brine, and tallow candle wax.","targetFactionId":"faction_archivists","text":"Cut the tamper wire and descend into the dark, paraffin-scented benthic vault."},{"choiceId":"choice_last_entry_leave_…`
  - row 23: `{"factionTag":"","firstStageId":"stage_open_count_call_loop","maxDay":360,"minDay":250,"questlineId":"quest_verdict_the_open_count","stages":[{"choices":[{"choiceId":"choice_open_count_approach_silo","factionStandingDelta":1,"grantItemId":"","grantItemQuantity":0,"moraleDelta":1,"nextStageId":"stage_open_count_registry_dais","outcomeNarrative":"The steel doors of the Tape-Silo stand open. Inside the central nave, the reading lectern glows under auxiliary emergency filaments.","targetFactionId":"faction_the_tempest","text":"Journey to the central Archive Tape-Silo to access the master registry console."},{"choiceId":"choice_open_count_shelter…`

## `Assets/StreamingAssets/Data/verdict_npcs.json`
- Bytes: 9,832; SHA-256: `ace1edded901844ae68ebbaad7f913990316ab155888ccee2c531766be7cbadc`
- Root keys: `items, schema_version`
- `items`: list[18]; union fields: `dialogue, gating_flag, id, kind, location_id, name, phase_min, role`
  - row 1: `{"dialogue":["Still here. Static's thinning. That's not good news, that's a storm on the way.","The array's drawing again. I don't know what it's drawing for. I don't think it draws for us."],"gating_flag":"flag_verdict_eden_log_recovered","id":"npc_eden_vale","kind":"tape_echo","location_id":"loc_comm_array","name":"Eden Vale","phase_min":1,"role":"Amateur radio operator, comm-array bleed"}`
  - row 2: `{"dialogue":["The archive keeps its own count. It does not require a reader.","An order can be paused, and a pause is not a cancellation. That is the whole Standard."],"gating_flag":"flag_verdict_fuse_world_read","id":"npc_ferris_voss","kind":"paper_ghost","location_id":"loc_network_fuse_bunker","name":"Ferris Voss","phase_min":1,"role":"Fire-control acceptance engineer, last human in the fuse world"}`
  - row 3: `{"dialogue":["Shift 36 closed out per procedure. The valve reads per 36; the record is complete.","Nobody swept that floor. It sweeps itself, the way the schedule requires."],"gating_flag":"flag_verdict_shift_charter_restored","id":"npc_iran_bell","kind":"paper_ghost","location_id":"loc_network_fuse_bunker","name":"Iaran Bell","phase_min":2,"role":"Tempest maintenance supervisor, the valve-touch hand"}`
  - row 4: `{"dialogue":["Persons shall not be counted twice. Persons shall not be counted once. The footnotes do not say which clause won.","The count column is blank, and it has been since Year One, when I ran out of households to sight."],"gating_flag":"flag_verdict_clerk_met","id":"npc_selya_saltmarsh","kind":"living","location_id":"loc_twelve_gauge_array","name":"Selya Saltmarsh","phase_min":2,"role":"Census clerk, the only human with an opinion about the count"}`
  - row 5: `{"dialogue":["This is the Office of Censuses. The count is open. All persons having custody of persons must present them.","Off-count is a penalty assessed against the holder. This message will repeat."],"gating_flag":"flag_verdict_call_resolved","id":"npc_maro_veen","kind":"tape_echo","location_id":"loc_archive_tape_silo","name":"Maro Veen","phase_min":3,"role":"The machine's own voice — the census-window tape loop"}`
  - row 6: `{"dialogue":["Readings follow. No meaning is assigned by this facility.","The meter is the meter. Access is granted by maintenance, withdrawn by nobody."],"gating_flag":"flag_verdict_relay_read","id":"npc_whisper_cipher","kind":"readings","location_id":"loc_radio_relay_mast","name":"Whisper Cipher","phase_min":1,"role":"The relay network's aggregate readings — univocal, procedural"}`
  - row 7: `{"dialogue":["Procedure is not an obstacle to justice; it is the only wall between justice and execution.","If the chain of custody is broken, the machine is not judging a person—it is judging an echo."],"gating_flag":"flag_verdict_reid_enrolled","id":"npc_tomas_reid","kind":"living","location_id":"loc_network_fuse_bunker","name":"Tomas Reid","phase_min":1,"role":"Defense clerk, tribunal appeals and admissibility"}`
  - row 8: `{"dialogue":["The tape does not lie, for it has no blood to race and no belly to hunger.","When the Standard speaks, human repentance is merely noise in the carrier frequency."],"gating_flag":"flag_verdict_vane_enrolled","id":"npc_elena_vane","kind":"living","location_id":"loc_archive_tape_silo","name":"Elena Vane","phase_min":2,"role":"Machine-cult deaconess, Voice of the Standard"}`
  - row 9: `{"dialogue":["Every document has an origin, a transit, and a rest. If any of the three are missing, the page is illegitimate.","I do not read what is written on the parchment; I inspect the rag fibres and the iron-gall ink."],"gating_flag":"flag_verdict_holt_enrolled","id":"npc_kasper_holt","kind":"paper_ghost","location_id":"loc_archive_tape_silo","name":"Kasper Holt","phase_min":1,"role":"Chief Archival Custodian, chain of custody keeper"}`
  - row 10: `{"dialogue":["The old high-water mark was wrong by six centimetres. I checked it three times.","No storm came through that night. The harbour climbed anyway.","They told me to stop writing the corrections in red. So I wrote them smaller."],"gating_flag":"flag_verdict_relay_read","id":"npc_mara_elsen","kind":"paper_ghost","location_id":"loc_abandoned_tide_gauge","name":"Mara Elsen","phase_min":1,"role":"Tide-gauge keeper, red-ink corrections on the harbour water-mark ledger"}`
  - row 11: `{"dialogue":["The pressure trace dropped after the wind had already turned. Instruments are not supposed to remember weather late.","Central sent a correction packet for numbers I took myself. I kept the paper chart.","After that, the forecast was always cleaner than the sky."],"gating_flag":"flag_verdict_fuse_world_read","id":"npc_ilya_venn","kind":"paper_ghost","location_id":"loc_coastal_meteorological_station","name":"Ilya Venn","phase_min":1,"role":"Meteorological-station observer, the paper chart behind the correction packets"}`
  - row 12: `{"dialogue":["Channel four was civil traffic until someone changed the routing table.","After midnight, every third packet came back with a different origin.","I stopped acknowledging them. The system acknowledged for me."],"gating_flag":"flag_verdict_cliff_signal_decoded","id":"npc_garrick_daal","kind":"tape_echo","location_id":"loc_clifftop_observation_bunker","name":"Garrick Daal","phase_min":2,"role":"Cliff-bunker signalman, the relay that acknowledged after the grid went black"}`
  - row 13: `{"dialogue":["The mussels changed before the water report did.","Sample twelve was discarded twice. I kept both labels.","Someone wanted the contamination to begin on a date."],"gating_flag":"flag_verdict_eden_log_recovered","id":"npc_sena_korr","kind":"paper_ghost","location_id":"loc_sealed_marine_laboratory","name":"Dr. Sena Korr","phase_min":2,"role":"Marine-laboratory researcher, the sample labels that outlived the intake log"}`
  - row 14: `{"dialogue":["The dead stand starts seventy metres before the fire line.","I marked it as wind damage. The map came back without the mark.","Trees do not know where the administrative boundary runs."],"gating_flag":"flag_verdict_relay_read","id":"npc_torin_rask","kind":"paper_ghost","location_id":"loc_forestry_survey_post","name":"Torin Rask","phase_min":1,"role":"Forestry surveyor, fallout contours the burn maps would not carry"}`
  - row 15: `{"dialogue":["Core seven has two labels and one depth.","The ash layer sits below material they dated earlier.","I was told to file the duplicate under equipment error.","The downriver gauging station holds the baseline. Compare it before someone corrects it."],"gating_flag":"flag_verdict_clerk_met","id":"npc_oren_varek","kind":"paper_ghost","location_id":"loc_geological_core_vault","name":"Oren Varek","phase_min":2,"role":"Core-sample technician, two labels and one depth"}`
  - row 16: `{"dialogue":["The river rose without rain upstream.","Telemetry called it sensor drift. The concrete stairs were wet.","When the reading came back down, the mud line stayed."],"gating_flag":"flag_verdict_fuse_world_read","id":"npc_lena_rost","kind":"paper_ghost","location_id":"loc_river_gauging_station","name":"Lena Rost","phase_min":1,"role":"River-gauge attendant, mud line over telemetry"}`
  - row 17: `{"dialogue":["The control trays failed first.","That should have ended the trial. Instead they changed which trays were called control.","The seeds were honest. The labels were not."],"gating_flag":"flag_verdict_shift_charter_restored","id":"npc_tessa_mirn","kind":"paper_ghost","location_id":"loc_abandoned_agricultural_station","name":"Tessa Mirn","phase_min":3,"role":"Agricultural-station botanist, the control trays that were renamed"}`
  - row 18: `{"dialogue":["The warning crossed the border before the order authorizing it.","I logged the time twice because I thought the clock had slipped.","Then headquarters asked me which copy of the log I intended to keep."],"gating_flag":"flag_verdict_call_resolved","id":"npc_karel_norn","kind":"tape_echo","location_id":"loc_decommissioned_signal_relay","name":"Karel Norn","phase_min":3,"role":"Border-relay operator, the last handoff between warning traffic and restricted channels"}`
- Bytes: 9,832; SHA-256: `ace1edded901844ae68ebbaad7f913990316ab155888ccee2c531766be7cbadc`
- Root keys: `items, schema_version`
- `items`: list[18]; union fields: `dialogue, gating_flag, id, kind, location_id, name, phase_min, role`
  - row 1: `{"dialogue":["Still here. Static's thinning. That's not good news, that's a storm on the way.","The array's drawing again. I don't know what it's drawing for. I don't think it draws for us."],"gating_flag":"flag_verdict_eden_log_recovered","id":"npc_eden_vale","kind":"tape_echo","location_id":"loc_comm_array","name":"Eden Vale","phase_min":1,"role":"Amateur radio operator, comm-array bleed"}`
  - row 2: `{"dialogue":["The archive keeps its own count. It does not require a reader.","An order can be paused, and a pause is not a cancellation. That is the whole Standard."],"gating_flag":"flag_verdict_fuse_world_read","id":"npc_ferris_voss","kind":"paper_ghost","location_id":"loc_network_fuse_bunker","name":"Ferris Voss","phase_min":1,"role":"Fire-control acceptance engineer, last human in the fuse world"}`
  - row 3: `{"dialogue":["Shift 36 closed out per procedure. The valve reads per 36; the record is complete.","Nobody swept that floor. It sweeps itself, the way the schedule requires."],"gating_flag":"flag_verdict_shift_charter_restored","id":"npc_iran_bell","kind":"paper_ghost","location_id":"loc_network_fuse_bunker","name":"Iaran Bell","phase_min":2,"role":"Tempest maintenance supervisor, the valve-touch hand"}`
  - row 4: `{"dialogue":["Persons shall not be counted twice. Persons shall not be counted once. The footnotes do not say which clause won.","The count column is blank, and it has been since Year One, when I ran out of households to sight."],"gating_flag":"flag_verdict_clerk_met","id":"npc_selya_saltmarsh","kind":"living","location_id":"loc_twelve_gauge_array","name":"Selya Saltmarsh","phase_min":2,"role":"Census clerk, the only human with an opinion about the count"}`
  - row 5: `{"dialogue":["This is the Office of Censuses. The count is open. All persons having custody of persons must present them.","Off-count is a penalty assessed against the holder. This message will repeat."],"gating_flag":"flag_verdict_call_resolved","id":"npc_maro_veen","kind":"tape_echo","location_id":"loc_archive_tape_silo","name":"Maro Veen","phase_min":3,"role":"The machine's own voice — the census-window tape loop"}`
  - row 6: `{"dialogue":["Readings follow. No meaning is assigned by this facility.","The meter is the meter. Access is granted by maintenance, withdrawn by nobody."],"gating_flag":"flag_verdict_relay_read","id":"npc_whisper_cipher","kind":"readings","location_id":"loc_radio_relay_mast","name":"Whisper Cipher","phase_min":1,"role":"The relay network's aggregate readings — univocal, procedural"}`
  - row 7: `{"dialogue":["Procedure is not an obstacle to justice; it is the only wall between justice and execution.","If the chain of custody is broken, the machine is not judging a person—it is judging an echo."],"gating_flag":"flag_verdict_reid_enrolled","id":"npc_tomas_reid","kind":"living","location_id":"loc_network_fuse_bunker","name":"Tomas Reid","phase_min":1,"role":"Defense clerk, tribunal appeals and admissibility"}`
  - row 8: `{"dialogue":["The tape does not lie, for it has no blood to race and no belly to hunger.","When the Standard speaks, human repentance is merely noise in the carrier frequency."],"gating_flag":"flag_verdict_vane_enrolled","id":"npc_elena_vane","kind":"living","location_id":"loc_archive_tape_silo","name":"Elena Vane","phase_min":2,"role":"Machine-cult deaconess, Voice of the Standard"}`
  - row 9: `{"dialogue":["Every document has an origin, a transit, and a rest. If any of the three are missing, the page is illegitimate.","I do not read what is written on the parchment; I inspect the rag fibres and the iron-gall ink."],"gating_flag":"flag_verdict_holt_enrolled","id":"npc_kasper_holt","kind":"paper_ghost","location_id":"loc_archive_tape_silo","name":"Kasper Holt","phase_min":1,"role":"Chief Archival Custodian, chain of custody keeper"}`
  - row 10: `{"dialogue":["The old high-water mark was wrong by six centimetres. I checked it three times.","No storm came through that night. The harbour climbed anyway.","They told me to stop writing the corrections in red. So I wrote them smaller."],"gating_flag":"flag_verdict_relay_read","id":"npc_mara_elsen","kind":"paper_ghost","location_id":"loc_abandoned_tide_gauge","name":"Mara Elsen","phase_min":1,"role":"Tide-gauge keeper, red-ink corrections on the harbour water-mark ledger"}`
  - row 11: `{"dialogue":["The pressure trace dropped after the wind had already turned. Instruments are not supposed to remember weather late.","Central sent a correction packet for numbers I took myself. I kept the paper chart.","After that, the forecast was always cleaner than the sky."],"gating_flag":"flag_verdict_fuse_world_read","id":"npc_ilya_venn","kind":"paper_ghost","location_id":"loc_coastal_meteorological_station","name":"Ilya Venn","phase_min":1,"role":"Meteorological-station observer, the paper chart behind the correction packets"}`
  - row 12: `{"dialogue":["Channel four was civil traffic until someone changed the routing table.","After midnight, every third packet came back with a different origin.","I stopped acknowledging them. The system acknowledged for me."],"gating_flag":"flag_verdict_cliff_signal_decoded","id":"npc_garrick_daal","kind":"tape_echo","location_id":"loc_clifftop_observation_bunker","name":"Garrick Daal","phase_min":2,"role":"Cliff-bunker signalman, the relay that acknowledged after the grid went black"}`
  - row 13: `{"dialogue":["The mussels changed before the water report did.","Sample twelve was discarded twice. I kept both labels.","Someone wanted the contamination to begin on a date."],"gating_flag":"flag_verdict_eden_log_recovered","id":"npc_sena_korr","kind":"paper_ghost","location_id":"loc_sealed_marine_laboratory","name":"Dr. Sena Korr","phase_min":2,"role":"Marine-laboratory researcher, the sample labels that outlived the intake log"}`
  - row 14: `{"dialogue":["The dead stand starts seventy metres before the fire line.","I marked it as wind damage. The map came back without the mark.","Trees do not know where the administrative boundary runs."],"gating_flag":"flag_verdict_relay_read","id":"npc_torin_rask","kind":"paper_ghost","location_id":"loc_forestry_survey_post","name":"Torin Rask","phase_min":1,"role":"Forestry surveyor, fallout contours the burn maps would not carry"}`
  - row 15: `{"dialogue":["Core seven has two labels and one depth.","The ash layer sits below material they dated earlier.","I was told to file the duplicate under equipment error.","The downriver gauging station holds the baseline. Compare it before someone corrects it."],"gating_flag":"flag_verdict_clerk_met","id":"npc_oren_varek","kind":"paper_ghost","location_id":"loc_geological_core_vault","name":"Oren Varek","phase_min":2,"role":"Core-sample technician, two labels and one depth"}`
  - row 16: `{"dialogue":["The river rose without rain upstream.","Telemetry called it sensor drift. The concrete stairs were wet.","When the reading came back down, the mud line stayed."],"gating_flag":"flag_verdict_fuse_world_read","id":"npc_lena_rost","kind":"paper_ghost","location_id":"loc_river_gauging_station","name":"Lena Rost","phase_min":1,"role":"River-gauge attendant, mud line over telemetry"}`
  - row 17: `{"dialogue":["The control trays failed first.","That should have ended the trial. Instead they changed which trays were called control.","The seeds were honest. The labels were not."],"gating_flag":"flag_verdict_shift_charter_restored","id":"npc_tessa_mirn","kind":"paper_ghost","location_id":"loc_abandoned_agricultural_station","name":"Tessa Mirn","phase_min":3,"role":"Agricultural-station botanist, the control trays that were renamed"}`
  - row 18: `{"dialogue":["The warning crossed the border before the order authorizing it.","I logged the time twice because I thought the clock had slipped.","Then headquarters asked me which copy of the log I intended to keep."],"gating_flag":"flag_verdict_call_resolved","id":"npc_karel_norn","kind":"tape_echo","location_id":"loc_decommissioned_signal_relay","name":"Karel Norn","phase_min":3,"role":"Border-relay operator, the last handoff between warning traffic and restricted channels"}`

## `Assets/StreamingAssets/Data/audio_cues.json`
- Bytes: 112,400; SHA-256: `afec6f7eb662d456667f2e60b41255f689278524593f06d19e93a9d63a207bd0`
- Root keys: `cues, schema_version`
- `cues`: list[196]; union fields: `bus, cooldown_seconds, default_volume_db, fade_in_seconds, fade_out_seconds, fallback_cue_id, id, loop, max_instances, pitch_max, pitch_min, priority, resource_path, resource_paths, volume_jitter_db`
  - row 1: `{"bus":"SFX","cooldown_seconds":1.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_crafting","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_crafting_assemble.mp3","resource_paths":["res://assets/audio/sfx/sfx_crafting_assemble.mp3"],"volume_jitter_db":0.0}`
  - row 2: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_injection","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_injection.mp3","resource_paths":["res://assets/audio/sfx/sfx_injection.mp3"],"volume_jitter_db":0.0}`
  - row 3: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_interrogation_slam","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_interrogation_slam.mp3","resource_paths":["res://assets/audio/sfx/sfx_interrogation_slam.mp3"],"volume_jitter_db":0.0}`
  - row 4: `{"bus":"SFX","cooldown_seconds":0.2,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_item_pickup","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_action_item_pickup_01.wav","resource_paths":["res://assets/audio/sfx/sfx_action_item_pickup_01.wav","res://assets/audio/sfx/sfx_action_item_pickup_02.wav","res://assets/audio/sfx/sfx_action_item_pickup_03.wav","res://assets/audio/sfx/sfx_action_item_pickup_04.wav","res://assets/audio/sfx/sfx_action_item_pickup_05.wav"],"volume_jitter_db":0.0}`
  - row 5: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_pill_bottle","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_pill_bottle.mp3","resource_paths":["res://assets/audio/sfx/sfx_pill_bottle.mp3"],"volume_jitter_db":0.0}`
  - row 6: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_repair","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_repair_wrench.mp3","resource_paths":["res://assets/audio/sfx/sfx_repair_wrench.mp3"],"volume_jitter_db":0.0}`
  - row 7: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_trade","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_trade_exchange.mp3","resource_paths":["res://assets/audio/sfx/sfx_trade_exchange.mp3"],"volume_jitter_db":0.0}`
  - row 8: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_water_pour","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_water_pour.mp3","resource_paths":["res://assets/audio/sfx/sfx_water_pour.mp3"],"volume_jitter_db":0.0}`
  - row 9: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_bunker","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/bunker_ambience.ogg","resource_paths":["res://assets/audio/ambience/bunker_ambience.ogg"],"volume_jitter_db":0.0}`
  - row 10: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-4.0,"fade_in_seconds":1.5,"fade_out_seconds":1.5,"fallback_cue_id":"amb_bunker","id":"amb_bunker_low_power_loop","loop":true,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":20,"resource_path":"res://assets/audio/ambience/amb_bunker_low_power_loop.ogg","resource_paths":["res://assets/audio/ambience/amb_bunker_low_power_loop.ogg"],"volume_jitter_db":0.0}`
  - row 11: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_abandoned_hospital","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_abandoned_hospital.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_abandoned_hospital.mp3"],"volume_jitter_db":0.0}`
  - row 12: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_arcology_sector","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_arcology_sector.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_arcology_sector.mp3"],"volume_jitter_db":0.0}`
  - row 13: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_geothermal_ruins","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_geothermal_ruins.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_geothermal_ruins.mp3"],"volume_jitter_db":0.0}`
  - row 14: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_military_bunker","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_military_bunker.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_military_bunker.mp3"],"volume_jitter_db":0.0}`
  - row 15: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_rural_gas_station","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_rural_gas_station.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_rural_gas_station.mp3"],"volume_jitter_db":0.0}`
  - row 16: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_suburban_ruins","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_suburban_ruins.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_suburban_ruins.mp3"],"volume_jitter_db":0.0}`
  - row 17: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_surface","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/surface_ambience.ogg","resource_paths":["res://assets/audio/ambience/surface_ambience.ogg"],"volume_jitter_db":0.0}`
  - row 18: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-2.0,"fade_in_seconds":1.5,"fade_out_seconds":1.5,"fallback_cue_id":"amb_surface_storm","id":"amb_surface_ashfall_loop","loop":true,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":20,"resource_path":"res://assets/audio/ambience/amb_surface_ashfall_loop.ogg","resource_paths":["res://assets/audio/ambience/amb_surface_ashfall_loop.ogg"],"volume_jitter_db":0.0}`
  - row 19: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-2.0,"fade_in_seconds":1.5,"fade_out_seconds":1.5,"fallback_cue_id":"amb_surface_storm","id":"amb_surface_blizzard_loop","loop":true,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":20,"resource_path":"res://assets/audio/ambience/amb_surface_blizzard_loop.ogg","resource_paths":["res://assets/audio/ambience/amb_surface_blizzard_loop.ogg"],"volume_jitter_db":0.0}`
  - row 20: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-2.0,"fade_in_seconds":1.5,"fade_out_seconds":1.5,"fallback_cue_id":"amb_surface_storm","id":"amb_surface_fallout_storm_loop","loop":true,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":20,"resource_path":"res://assets/audio/ambience/amb_surface_fallout_storm_loop.ogg","resource_paths":["res://assets/audio/ambience/amb_surface_fallout_storm_loop.ogg"],"volume_jitter_db":0.0}`
  - row 21: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-7.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_surface_storm","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_surface_storm.wav","resource_paths":["res://assets/audio/ambience/amb_surface_storm.wav"],"volume_jitter_db":0.0}`
  - row 22: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_warzone_distant_shelling","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_warzone_distant_shelling.mp3","resource_paths":["res://assets/audio/ambience/amb_warzone_distant_shelling.mp3"],"volume_jitter_db":0.0}`
  - row 23: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":"med_heartbeat","id":"bio_mutation_pulse","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_mutation_pulse.mp3","resource_paths":["res://assets/audio/sfx/sfx_mutation_pulse.mp3"],"volume_jitter_db":0.0}`
  - row 24: `{"bus":"SFX","cooldown_seconds":0.08,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_casing_drop","loop":false,"max_instances":4,"pitch_max":1.06,"pitch_min":0.94,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_shell_casing_drop_01.wav","resource_paths":["res://assets/audio/sfx/sfx_shell_casing_drop_01.wav","res://assets/audio/sfx/sfx_shell_casing_drop_02.wav"],"volume_jitter_db":0.0}`
  - row 25: `{"bus":"SFX","cooldown_seconds":1.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_decon_flush","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_decon_spray.wav","resource_paths":["res://assets/audio/sfx/sfx_combat_decon_spray.wav"],"volume_jitter_db":0.0}`
  - row 26: `{"bus":"Music","cooldown_seconds":5.0,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_defeat","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_defeat.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_defeat.mp3"],"volume_jitter_db":0.0}`
  - row 27: `{"bus":"SFX","cooldown_seconds":1.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_downed","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_downed.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_downed.mp3"],"volume_jitter_db":0.0}`
  - row 28: `{"bus":"SFX","cooldown_seconds":0.15,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_dry_fire","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_dry_fire_click.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_dry_fire_click.wav"],"volume_jitter_db":0.0}`
  - row 29: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_fire","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_gunshot.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_gunshot.mp3"],"volume_jitter_db":0.8}`
  - row 30: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_hit","loop":false,"max_instances":4,"pitch_max":1.06,"pitch_min":0.94,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_hit.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_hit.mp3"],"volume_jitter_db":1.0}`
  - row 31: `{"bus":"SFX","cooldown_seconds":0.1,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_impact_concrete","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_impact_concrete_crack.wav","resource_paths":["res://assets/audio/sfx/sfx_impact_concrete_crack.wav"],"volume_jitter_db":0.0}`
  - row 32: `{"bus":"SFX","cooldown_seconds":0.1,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_impact_metal","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_impact_metal_ricochet.wav","resource_paths":["res://assets/audio/sfx/sfx_impact_metal_ricochet.wav"],"volume_jitter_db":0.0}`
  - row 33: `{"bus":"SFX","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_impact_wood","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_impact_wood_splinter.wav","resource_paths":["res://assets/audio/sfx/sfx_impact_wood_splinter.wav"],"volume_jitter_db":0.0}`
  - row 34: `{"bus":"Alerts","cooldown_seconds":1.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_improvised_fire","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_molotov_burst.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_molotov_burst.wav"],"volume_jitter_db":0.0}`
  - row 35: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_improvised_spear","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_rebar_spear_thud.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_rebar_spear_thud.wav"],"volume_jitter_db":0.0}`
  - row 36: `{"bus":"SFX","cooldown_seconds":1.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_jam","loop":false,"max_instances":4,"pitch_max":1.02,"pitch_min":0.98,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_jam.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_jam.mp3"],"volume_jitter_db":0.0}`
  - row 37: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_last_stand","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_last_stand.wav","resource_paths":["res://assets/audio/sfx/sfx_combat_last_stand.wav"],"volume_jitter_db":0.0}`
  - row 38: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_reload","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_reload.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_reload.mp3"],"volume_jitter_db":0.0}`
  - row 39: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_start","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_start.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_start.mp3"],"volume_jitter_db":0.0}`
  - row 40: `{"bus":"SFX","cooldown_seconds":5.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_victory","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_victory.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_victory.mp3"],"volume_jitter_db":0.0}`
  - row 41: `{"bus":"Alerts","cooldown_seconds":1.0,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_weapon_burst","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_burst_rupture.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_burst_rupture.wav"],"volume_jitter_db":0.0}`
  - row 42: `{"bus":"Alerts","cooldown_seconds":10.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"danger_alarm_klaxon","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_danger_alarm_klaxon.wav","resource_paths":["res://assets/audio/sfx/sfx_danger_alarm_klaxon.wav"],"volume_jitter_db":0.0}`
  - row 43: `{"bus":"SFX","cooldown_seconds":3.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"danger_debris","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_debris_impact.mp3","resource_paths":["res://assets/audio/sfx/sfx_debris_impact.mp3"],"volume_jitter_db":0.0}`
  - row 44: `{"bus":"SFX","cooldown_seconds":15.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"danger_explosion","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_danger_explosion_01.wav","resource_paths":["res://assets/audio/sfx/sfx_danger_explosion_01.wav","res://assets/audio/sfx/sfx_danger_explosion_02.wav","res://assets/audio/sfx/sfx_danger_explosion_03.wav","res://assets/audio/sfx/sfx_danger_explosion_04.wav","res://assets/audio/sfx/sfx_danger_explosion_05.wav"],"volume_jitter_db":0.0}`
  - row 45: `{"bus":"SFX","cooldown_seconds":1.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"danger_glass_break","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_glass_break_small.mp3","resource_paths":["res://assets/audio/sfx/sfx_glass_break_small.mp3"],"volume_jitter_db":0.0}`
  - row 46: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"day_transition","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_day_bell.mp3","resource_paths":["res://assets/audio/sfx/sfx_day_bell.mp3"],"volume_jitter_db":0.0}`
  - row 47: `{"bus":"Alerts","cooldown_seconds":3.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"echo_discovery","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_echo_memory_shimmer.wav","resource_paths":["res://assets/audio/sfx/sfx_echo_memory_shimmer.wav"],"volume_jitter_db":0.0}`
  - row 48: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-10.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_camp_fire","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_expedition_camp_fire.wav","resource_paths":["res://assets/audio/ambience/amb_expedition_camp_fire.wav"],"volume_jitter_db":0.0}`
  - row 49: `{"bus":"Alerts","cooldown_seconds":2.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_breakdown","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_breakdown_stall.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_breakdown_stall.wav"],"volume_jitter_db":0.0}`
  - row 50: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-10.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_dirtbike","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_engine_dirtbike.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_engine_dirtbike.wav"],"volume_jitter_db":0.0}`
  - row 51: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-12.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_engine","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_engine_diesel.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_engine_diesel.wav"],"volume_jitter_db":0.0}`
  - row 52: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_refuel","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_refuel.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_refuel.wav"],"volume_jitter_db":0.0}`
  - row 53: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_repair","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_repair.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_repair.wav"],"volume_jitter_db":0.0}`
  - row 54: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-11.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_truck","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_engine_truck.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_engine_truck.wav"],"volume_jitter_db":0.0}`
  - row 55: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"flashback_grounded","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_flashback_grounded.wav","resource_paths":["res://assets/audio/sfx/sfx_flashback_grounded.wav"],"volume_jitter_db":0.0}`
  - row 56: `{"bus":"Alerts","cooldown_seconds":3.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"flashback_trigger","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_flashback_distortion.wav","resource_paths":["res://assets/audio/sfx/sfx_flashback_distortion.wav"],"volume_jitter_db":0.0}`
  - row 57: `{"bus":"Music","cooldown_seconds":0.0,"default_volume_db":-10.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"game_over","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/music/game_over.ogg","resource_paths":["res://assets/audio/music/game_over.ogg"],"volume_jitter_db":0.0}`
  - row 58: `{"bus":"Alerts","cooldown_seconds":1.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"hazard_toxic_sizzle","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_hazard_toxic_sizzle.mp3","resource_paths":["res://assets/audio/sfx/sfx_hazard_toxic_sizzle.mp3"],"volume_jitter_db":0.0}`
  - row 59: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"item_handling_ammo","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_item_handling_ammo_01.wav","resource_paths":["res://assets/audio/sfx/sfx_item_handling_ammo_01.wav","res://assets/audio/sfx/sfx_item_handling_ammo_02.wav","res://assets/audio/sfx/sfx_item_handling_ammo_03.wav","res://assets/audio/sfx/sfx_item_handling_ammo_04.wav","res://assets/audio/sfx/sfx_item_handling_ammo_05.wav"],"volume_jitter_db":0.0}`
  - row 60: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"item_handling_meds","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_item_handling_meds_01.wav","resource_paths":["res://assets/audio/sfx/sfx_item_handling_meds_01.wav","res://assets/audio/sfx/sfx_item_handling_meds_02.wav","res://assets/audio/sfx/sfx_item_handling_meds_03.wav","res://assets/audio/sfx/sfx_item_handling_meds_04.wav","res://assets/audio/sfx/sfx_item_handling_meds_05.wav"],"volume_jitter_db":0.0}`
  - row 61: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"item_handling_ration","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_item_handling_ration_01.wav","resource_paths":["res://assets/audio/sfx/sfx_item_handling_ration_01.wav","res://assets/audio/sfx/sfx_item_handling_ration_02.wav","res://assets/audio/sfx/sfx_item_handling_ration_03.wav","res://assets/audio/sfx/sfx_item_handling_ration_04.wav","res://assets/audio/sfx/sfx_item_handling_ration_05.wav"],"volume_jitter_db":0.0}`
  - row 62: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"log_tape_button","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_tape_deck_button.wav","resource_paths":["res://assets/audio/sfx/sfx_tape_deck_button.wav"],"volume_jitter_db":0.0}`
  - row 63: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-18.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"log_tape_hiss","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_tape_hiss_loop.wav","resource_paths":["res://assets/audio/sfx/sfx_tape_hiss_loop.wav"],"volume_jitter_db":0.0}`
  - row 64: `{"bus":"UI","cooldown_seconds":0.3,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"log_tape_insert","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_tape_deck_insert.wav","resource_paths":["res://assets/audio/sfx/sfx_tape_deck_insert.wav"],"volume_jitter_db":0.0}`
  - row 65: `{"bus":"UI","cooldown_seconds":0.2,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"log_tape_rewind","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_tape_rewind.wav","resource_paths":["res://assets/audio/sfx/sfx_tape_rewind.wav"],"volume_jitter_db":0.0}`
  - row 66: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"log_tape_stop","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_tape_stop.wav","resource_paths":["res://assets/audio/sfx/sfx_tape_stop.wav"],"volume_jitter_db":0.0}`
  - row 67: `{"bus":"SFX","cooldown_seconds":8.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_coughing","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_coughing_fit.mp3","resource_paths":["res://assets/audio/sfx/sfx_coughing_fit.mp3"],"volume_jitter_db":0.0}`
  - row 68: `{"bus":"SFX","cooldown_seconds":5.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_heartbeat","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_heartbeat_slow.mp3","resource_paths":["res://assets/audio/sfx/sfx_heartbeat_slow.mp3"],"volume_jitter_db":0.0}`
  - row 69: `{"bus":"Medical","cooldown_seconds":1.5,"default_volume_db":-12.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_infirmary_beep","loop":false,"max_instances":4,"pitch_max":1.02,"pitch_min":0.98,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_infirmary_monitor_beep.wav","resource_paths":["res://assets/audio/sfx/sfx_infirmary_monitor_beep.wav"],"volume_jitter_db":0.0}`
  - row 70: `{"bus":"Medical","cooldown_seconds":0.75,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_quarantine_clear","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_med_quarantine_clear.wav","resource_paths":["res://assets/audio/sfx/sfx_med_quarantine_clear.wav"],"volume_jitter_db":0.0}`
  - row 71: `{"bus":"Medical","cooldown_seconds":1.0,"default_volume_db":-7.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_quarantine_seal","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_med_quarantine_seal.wav","resource_paths":["res://assets/audio/sfx/sfx_med_quarantine_seal.wav"],"volume_jitter_db":0.0}`
  - row 72: `{"bus":"Medical","cooldown_seconds":3.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_survivor_death","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_survivor_death.wav","resource_paths":["res://assets/audio/sfx/sfx_survivor_death.wav"],"volume_jitter_db":0.0}`
  - row 73: `{"bus":"Music","cooldown_seconds":0.0,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"music_gameplay","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/music/gameplay_underscore.ogg","resource_paths":["res://assets/audio/music/gameplay_underscore.ogg"],"volume_jitter_db":0.0}`
  - row 74: `{"bus":"Music","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"music_menu","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/music/main_menu.ogg","resource_paths":["res://assets/audio/music/main_menu.ogg"],"volume_jitter_db":0.0}`
  - row 75: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_alert_acute","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_radiation_alarm.mp3","resource_paths":["res://assets/audio/sfx/sfx_radiation_alarm.mp3","res://assets/audio/sfx/radiation_alert.wav"],"volume_jitter_db":0.0}`
  - row 76: `{"bus":"Alerts","cooldown_seconds":10.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_alert_chronic","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_radiation_chronic_alarm.wav","resource_paths":["res://assets/audio/sfx/sfx_radiation_chronic_alarm.wav"],"volume_jitter_db":0.0}`
  - row 77: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_contamination","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_contamination_warning.mp3","resource_paths":["res://assets/audio/sfx/sfx_contamination_warning.mp3"],"volume_jitter_db":0.0}`
  - row 78: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_geiger_burst","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_geiger_burst.mp3","resource_paths":["res://assets/audio/sfx/sfx_geiger_burst.mp3"],"volume_jitter_db":0.0}`
  - row 79: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_geiger_intense","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_geiger_intense_crackling.wav","resource_paths":["res://assets/audio/sfx/sfx_geiger_intense_crackling.wav"],"volume_jitter_db":0.0}`
  - row 80: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-10.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_geiger_loop","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/geiger.wav","resource_paths":["res://assets/audio/sfx/geiger.wav"],"volume_jitter_db":0.0}`
  - row 81: `{"bus":"Voice","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_dead_hand_pulse","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/radio_dead_hand_pulse.wav","resource_paths":["res://assets/audio/radio/radio_dead_hand_pulse.wav"],"volume_jitter_db":0.0}`
  - row 82: `{"bus":"Voice","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_distress_beacon","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/radio_distress_beacon.wav","resource_paths":["res://assets/audio/radio/radio_distress_beacon.wav"],"volume_jitter_db":0.0}`
  - row 83: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_ebs_alert","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/radio_ebs_alert.wav","resource_paths":["res://assets/audio/radio/radio_ebs_alert.wav"],"volume_jitter_db":0.0}`
  - row 84: `{"bus":"Voice","cooldown_seconds":0.5,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_morse","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_morse_key.mp3","resource_paths":["res://assets/audio/sfx/sfx_morse_key.mp3"],"volume_jitter_db":0.0}`
  - row 85: `{"bus":"Voice","cooldown_seconds":0.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_numbers_station","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/radio_numbers_station.wav","resource_paths":["res://assets/audio/radio/radio_numbers_station.wav"],"volume_jitter_db":0.0}`
  - row 86: `{"bus":"Voice","cooldown_seconds":1.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_signal_lock","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_radio_signal_lock.mp3","resource_paths":["res://assets/audio/sfx/sfx_radio_signal_lock.mp3"],"volume_jitter_db":0.0}`
  - row 87: `{"bus":"Voice","cooldown_seconds":0.5,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_static","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/radio_static_hiss.wav","resource_paths":["res://assets/audio/radio/radio_static_hiss.wav"],"volume_jitter_db":0.0}`
  - row 88: `{"bus":"Voice","cooldown_seconds":1.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_tune","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_radio_tune.mp3","resource_paths":["res://assets/audio/sfx/sfx_radio_tune.mp3"],"volume_jitter_db":0.0}`
  - row 89: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-2.3,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_ch11_stockpile","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_ch11_stockpile.wav","resource_paths":["res://assets/audio/radio/vo_ch11_stockpile.wav"],"volume_jitter_db":0.0}`
  - row 90: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_ch3_ash_road","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":90,"resource_path":"res://assets/audio/radio/vo_ch3_ash_road_elevenlabs_v1.wav","resource_paths":["res://assets/audio/radio/vo_ch3_ash_road_elevenlabs_v1.wav"],"volume_jitter_db":0.0}`
  - row 91: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_ch7_milband","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":90,"resource_path":"res://assets/audio/radio/vo_ch7_milband_elevenlabs_v1.wav","resource_paths":["res://assets/audio/radio/vo_ch7_milband_elevenlabs_v1.wav"],"volume_jitter_db":0.0}`
  - row 92: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_kind_hatch","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_kind_hatch_relay.wav","resource_paths":["res://assets/audio/radio/vo_kind_hatch_relay.wav","res://assets/audio/radio/vo_kind_hatch.wav"],"volume_jitter_db":0.0}`
  - row 93: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_kind_parley","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_kind_parley_beacon.wav","resource_paths":["res://assets/audio/radio/vo_kind_parley_beacon.wav","res://assets/audio/radio/vo_kind_parley.wav"],"volume_jitter_db":0.0}`
  - row 94: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_verdict_count","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_verdict_count.wav","resource_paths":["res://assets/audio/radio/vo_verdict_count.wav"],"volume_jitter_db":0.0}`
  - row 95: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_verdict_eden","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_verdict_eden.wav","resource_paths":["res://assets/audio/radio/vo_verdict_eden.wav"],"volume_jitter_db":0.0}`
  - row 96: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_verdict_geophone","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_verdict_geophone.wav","resource_paths":["res://assets/audio/radio/vo_verdict_geophone.wav"],"volume_jitter_db":0.0}`
  - row 97: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_verdict_meter","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_verdict_meter.wav","resource_paths":["res://assets/audio/radio/vo_verdict_meter.wav"],"volume_jitter_db":0.0}`
  - row 98: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_verdict_reckoning","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_verdict_reckoning.wav","resource_paths":["res://assets/audio/radio/vo_verdict_reckoning.wav"],"volume_jitter_db":0.0}`
  - row 99: `{"bus":"UI","cooldown_seconds":1.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"save_success","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ui/ui_save_success.wav","resource_paths":["res://assets/audio/ui/ui_save_success.wav"],"volume_jitter_db":0.0}`
  - row 100: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_airlock_purge_cycle","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_airlock_purge_cycle.mp3","resource_paths":["res://assets/audio/sfx/sfx_airlock_purge_cycle.mp3"],"volume_jitter_db":0.0}`
  - row 101: `{"bus":"Alerts","cooldown_seconds":8.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_artillery_incoming_whistle","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_artillery_incoming_whistle.mp3","resource_paths":["res://assets/audio/sfx/sfx_artillery_incoming_whistle.mp3"],"volume_jitter_db":0.0}`
  - row 102: `{"bus":"Alerts","cooldown_seconds":1.0,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":"danger_alarm_klaxon","id":"sfx_breaker_trip","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":85,"resource_path":"res://assets/audio/sfx/sfx_breaker_trip.wav","resource_paths":["res://assets/audio/sfx/sfx_breaker_trip.wav"],"volume_jitter_db":0.0}`
  - row 103: `{"bus":"SFX","cooldown_seconds":0.2,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_bullet_whiz_ricochet","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_bullet_whiz_ricochet.mp3","resource_paths":["res://assets/audio/sfx/sfx_bullet_whiz_ricochet.mp3"],"volume_jitter_db":0.0}`
  - row 104: `{"bus":"SFX","cooldown_seconds":5.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_distant_artillery_barrage","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_distant_artillery_barrage_01.wav","resource_paths":["res://assets/audio/sfx/sfx_distant_artillery_barrage_01.wav","res://assets/audio/sfx/sfx_distant_artillery_barrage_02.wav","res://assets/audio/sfx/sfx_distant_artillery_barrage_03.wav","res://assets/audio/sfx/sfx_distant_artillery_barrage_04.wav","res://assets/audio/sfx/sfx_distant_artillery_barrage_05.wav"]…`
  - row 105: `{"bus":"SFX","cooldown_seconds":4.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_distant_gunfire_skirmish","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_distant_gunfire_skirmish_01.wav","resource_paths":["res://assets/audio/sfx/sfx_distant_gunfire_skirmish_01.wav","res://assets/audio/sfx/sfx_distant_gunfire_skirmish_02.wav","res://assets/audio/sfx/sfx_distant_gunfire_skirmish_03.wav","res://assets/audio/sfx/sfx_distant_gunfire_skirmish_04.wav","res://assets/audio/sfx/sfx_distant_gunfire_skirmish_05.wav"],"volum…`
  - row 106: `{"bus":"SFX","cooldown_seconds":6.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_distant_mortar_launch","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_distant_mortar_launch.mp3","resource_paths":["res://assets/audio/sfx/sfx_distant_mortar_launch.mp3"],"volume_jitter_db":0.0}`
  - row 107: `{"bus":"Generator","cooldown_seconds":1.0,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_generator_start","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":60,"resource_path":"res://assets/audio/sfx/sfx_generator_start.wav","resource_paths":["res://assets/audio/sfx/sfx_generator_start.wav"],"volume_jitter_db":0.0}`
  - row 108: `{"bus":"Generator","cooldown_seconds":1.0,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_generator_stop","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":60,"resource_path":"res://assets/audio/sfx/sfx_generator_stop.wav","resource_paths":["res://assets/audio/sfx/sfx_generator_stop.wav"],"volume_jitter_db":0.0}`
  - row 109: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_heavy_impact_fall","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_heavy_impact_fall.mp3","resource_paths":["res://assets/audio/sfx/sfx_heavy_impact_fall.mp3"],"volume_jitter_db":0.0}`
  - row 110: `{"bus":"Generator","cooldown_seconds":1.0,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":"ui_confirm","id":"sfx_power_restore","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":60,"resource_path":"res://assets/audio/sfx/sfx_power_restore.wav","resource_paths":["res://assets/audio/sfx/sfx_power_restore.wav"],"volume_jitter_db":0.0}`
  - row 111: `{"bus":"SFX","cooldown_seconds":5.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_structural_collapse","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_structural_collapse.mp3","resource_paths":["res://assets/audio/sfx/sfx_structural_collapse.mp3"],"volume_jitter_db":0.0}`
  - row 112: `{"bus":"SFX","cooldown_seconds":0.25,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_assault_rifle_burst","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_assault_rifle_burst_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_assault_rifle_burst_01.wav","res://assets/audio/sfx/sfx_weapon_assault_rifle_burst_02.wav","res://assets/audio/sfx/sfx_weapon_assault_rifle_burst_03.wav","res://assets/audio/sfx/sfx_weapon_assault_rifle_burst_04.wav","res://assets/audio/sfx/sfx_weapon_assault_rifle_burs…`
  - row 113: `{"bus":"SFX","cooldown_seconds":0.4,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_bolt_rifle_report","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_01.wav","res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_02.wav","res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_03.wav","res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_04.wav","res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_05.wav"],"vol…`
  - row 114: `{"bus":"SFX","cooldown_seconds":0.15,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_cz75_report","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_cz75_report_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_cz75_report_01.wav","res://assets/audio/sfx/sfx_weapon_cz75_report_02.wav","res://assets/audio/sfx/sfx_weapon_cz75_report_03.wav","res://assets/audio/sfx/sfx_weapon_cz75_report_04.wav","res://assets/audio/sfx/sfx_weapon_cz75_report_05.wav"],"volume_jitter_db":0.7}`
  - row 115: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_lmg_burst","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_lmg_burst.mp3","resource_paths":["res://assets/audio/sfx/sfx_weapon_lmg_burst.mp3"],"volume_jitter_db":0.7}`
  - row 116: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_pipe_rifle_report","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_01.wav","res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_02.wav","res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_03.wav","res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_04.wav","res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_05.wav"],"vol…`
  - row 117: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_scrap_shotgun_report","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report_01.wav","res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report_02.wav","res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report_03.wav","res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report_04.wav","res://assets/audio/sfx/sfx_weapon_scrap_shotgun…`
  - row 118: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_shotgun_rack","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_shotgun_rack.mp3","resource_paths":["res://assets/audio/sfx/sfx_weapon_shotgun_rack.mp3"],"volume_jitter_db":0.0}`
  - row 119: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_sniper_heavy_report","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_sniper_heavy_report_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_sniper_heavy_report_01.wav","res://assets/audio/sfx/sfx_weapon_sniper_heavy_report_02.wav","res://assets/audio/sfx/sfx_weapon_sniper_heavy_report_03.wav","res://assets/audio/sfx/sfx_weapon_sniper_heavy_report_04.wav","res://assets/audio/sfx/sfx_weapon_sniper_heavy_report…`
  - row 120: `{"bus":"Alerts","cooldown_seconds":10.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_air_filter","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_air_filter_degrade.mp3","resource_paths":["res://assets/audio/sfx/sfx_air_filter_degrade.mp3"],"volume_jitter_db":0.0}`
  - row 121: `{"bus":"Ventilation","cooldown_seconds":0.0,"default_volume_db":-14.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_air_recycler","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_air_recycler_hiss.wav","resource_paths":["res://assets/audio/sfx/sfx_air_recycler_hiss.wav"],"volume_jitter_db":0.0}`
  - row 122: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_door_open","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_bunker_door_open.mp3","resource_paths":["res://assets/audio/sfx/sfx_bunker_door_open.mp3"],"volume_jitter_db":0.0}`
  - row 123: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_door_seal","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_bunker_door_seal.mp3","resource_paths":["res://assets/audio/sfx/sfx_bunker_door_seal.mp3"],"volume_jitter_db":0.0}`
  - row 124: `{"bus":"Generator","cooldown_seconds":0.0,"default_volume_db":-16.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_generator","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_generator_cough.mp3","resource_paths":["res://assets/audio/sfx/sfx_generator_cough.mp3"],"volume_jitter_db":0.0}`
  - row 125: `{"bus":"Generator","cooldown_seconds":0.0,"default_volume_db":-14.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_generator_strain","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_generator_heavy_strain.wav","resource_paths":["res://assets/audio/sfx/sfx_generator_heavy_strain.wav"],"volume_jitter_db":0.0}`
  - row 126: `{"bus":"SFX","cooldown_seconds":5.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_pipe_clang","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_pipe_clang.mp3","resource_paths":["res://assets/audio/sfx/sfx_pipe_clang.mp3"],"volume_jitter_db":0.0}`
  - row 127: `{"bus":"Ventilation","cooldown_seconds":0.0,"default_volume_db":-12.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_ventilation","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_ventilation_fan.mp3","resource_paths":["res://assets/audio/sfx/sfx_ventilation_fan.mp3"],"volume_jitter_db":0.0}`
  - row 128: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-15.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_water_drip","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_water_drip_cave.mp3","resource_paths":["res://assets/audio/sfx/sfx_water_drip_cave.mp3"],"volume_jitter_db":0.0}`
  - row 129: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-15.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_water_filtration","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_water_filtration_loop.wav","resource_paths":["res://assets/audio/sfx/sfx_water_filtration_loop.wav"],"volume_jitter_db":0.0}`
  - row 130: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-16.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_workshop_tools","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_workshop_lathe_hum.wav","resource_paths":["res://assets/audio/sfx/sfx_workshop_lathe_hum.wav"],"volume_jitter_db":0.0}`
  - row 131: `{"bus":"Alerts","cooldown_seconds":2.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":"expedition_vehicle_breakdown","id":"train_screech_crash","loop":false,"max_instances":2,"pitch_max":1.1,"pitch_min":0.9,"priority":80,"resource_path":"res://assets/audio/sfx/sfx_train_screech_crash.wav","resource_paths":["res://assets/audio/sfx/sfx_train_screech_crash.wav"],"volume_jitter_db":0.0}`
  - row 132: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-14.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"trauma_cabin_fever","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_trauma_cabin_fever_whisper.wav","resource_paths":["res://assets/audio/sfx/sfx_trauma_cabin_fever_whisper.wav"],"volume_jitter_db":0.0}`
  - row 133: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"trauma_heartbeat_rapid","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_trauma_heartbeat_rapid.wav","resource_paths":["res://assets/audio/sfx/sfx_trauma_heartbeat_rapid.wav"],"volume_jitter_db":0.0}`
  - row 134: `{"bus":"Alerts","cooldown_seconds":4.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"trauma_tinnitus","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_trauma_tinnitus_ring.wav","resource_paths":["res://assets/audio/sfx/sfx_trauma_tinnitus_ring.wav"],"volume_jitter_db":0.0}`
  - row 135: `{"bus":"UI","cooldown_seconds":0.05,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_cancel","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_cancel.wav","resource_paths":["res://assets/audio/ui/ui_cancel.wav"],"volume_jitter_db":0.0}`
  - row 136: `{"bus":"UI","cooldown_seconds":0.05,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_click","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/ui/ui_click.wav","resource_paths":["res://assets/audio/ui/ui_click.wav","res://assets/audio/ui/ui_click_01.wav","res://assets/audio/ui/ui_click_02.wav","res://assets/audio/ui/ui_click_03.wav","res://assets/audio/ui/ui_click_04.wav"],"volume_jitter_db":0.8}`
  - row 137: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_confirm","loop":false,"max_instances":4,"pitch_max":1.02,"pitch_min":0.98,"priority":50,"resource_path":"res://assets/audio/ui/ui_confirm.wav","resource_paths":["res://assets/audio/ui/ui_confirm.wav"],"volume_jitter_db":0.0}`
  - row 138: `{"bus":"UI","cooldown_seconds":1.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_crt_power_on","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ui/ui_crt_power_on.wav","resource_paths":["res://assets/audio/ui/ui_crt_power_on.wav"],"volume_jitter_db":0.0}`
  - row 139: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_drawer_slide","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_drawer_slide.wav","resource_paths":["res://assets/audio/ui/ui_drawer_slide.wav"],"volume_jitter_db":0.0}`
  - row 140: `{"bus":"UI","cooldown_seconds":0.5,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_invalid_action","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ui/ui_invalid_action.wav","resource_paths":["res://assets/audio/ui/ui_invalid_action.wav"],"volume_jitter_db":0.0}`
  - row 141: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_modal_close","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/ui/ui_modal_close.wav","resource_paths":["res://assets/audio/ui/ui_modal_close.wav"],"volume_jitter_db":0.0}`
  - row 142: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_modal_open","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/ui/ui_modal_open.wav","resource_paths":["res://assets/audio/ui/ui_modal_open.wav"],"volume_jitter_db":0.0}`
  - row 143: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_paper_rustle","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/ui/ui_paper_rustle.wav","resource_paths":["res://assets/audio/ui/ui_paper_rustle.wav"],"volume_jitter_db":0.0}`
  - row 144: `{"bus":"UI","cooldown_seconds":0.05,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_rotary_click","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_rotary_click.wav","resource_paths":["res://assets/audio/ui/ui_rotary_click.wav"],"volume_jitter_db":0.0}`
  - row 145: `{"bus":"UI","cooldown_seconds":0.2,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_stamp_heavy","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_stamp_heavy.wav","resource_paths":["res://assets/audio/ui/ui_stamp_heavy.wav"],"volume_jitter_db":0.0}`
  - row 146: `{"bus":"UI","cooldown_seconds":0.05,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_switch_toggle","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_switch_toggle.wav","resource_paths":["res://assets/audio/ui/ui_switch_toggle.wav"],"volume_jitter_db":0.0}`
  - row 147: `{"bus":"UI","cooldown_seconds":0.05,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_tab_change","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_tab_change.wav","resource_paths":["res://assets/audio/ui/ui_tab_change.wav"],"volume_jitter_db":0.0}`
  - row 148: `{"bus":"UI","cooldown_seconds":0.3,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_warning","loop":false,"max_instances":4,"pitch_max":1.02,"pitch_min":0.98,"priority":50,"resource_path":"res://assets/audio/ui/ui_warning.wav","resource_paths":["res://assets/audio/ui/ui_warning.wav"],"volume_jitter_db":0.0}`
  - row 149: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_alert","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_alarm_klaxon.mp3","resource_paths":["res://assets/audio/sfx/sfx_alarm_klaxon.mp3"],"volume_jitter_db":0.0}`
  - row 150: `{"bus":"Alerts","cooldown_seconds":10.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_black_rain","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weather_black_rain.wav","resource_paths":["res://assets/audio/sfx/sfx_weather_black_rain.wav"],"volume_jitter_db":0.0}`
  - row 151: `{"bus":"SFX","cooldown_seconds":10.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_blizzard","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weather_blizzard.wav","resource_paths":["res://assets/audio/sfx/sfx_weather_blizzard.wav"],"volume_jitter_db":0.0}`
  - row 152: `{"bus":"Alerts","cooldown_seconds":8.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_corrosive_precipitation","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weather_corrosive_precipitation.wav","resource_paths":["res://assets/audio/sfx/sfx_weather_corrosive_precipitation.wav"],"volume_jitter_db":0.0}`
  - row 153: `{"bus":"Alerts","cooldown_seconds":8.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_emp_storm","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weather_emp_storm.wav","resource_paths":["res://assets/audio/sfx/sfx_weather_emp_storm.wav"],"volume_jitter_db":0.0}`
  - row 154: `{"bus":"SFX","cooldown_seconds":10.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_fallout_storm","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_fallout_storm_approach.mp3","resource_paths":["res://assets/audio/sfx/sfx_fallout_storm_approach.mp3"],"volume_jitter_db":0.0}`
  - row 155: `{"bus":"SFX","cooldown_seconds":8.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_glass_storm","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weather_glass_storm.wav","resource_paths":["res://assets/audio/sfx/sfx_weather_glass_storm.wav"],"volume_jitter_db":0.0}`
  - row 156: `{"bus":"SFX","cooldown_seconds":3.0,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_wind_gust","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_wind_gust_harsh.mp3","resource_paths":["res://assets/audio/sfx/sfx_wind_gust_harsh.mp3"],"volume_jitter_db":0.0}`
  - row 157: `{"bus":"SFX","cooldown_seconds":0.05,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"footstep_granite","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_footstep_granite_01.wav","resource_paths":["res://assets/audio/sfx/sfx_footstep_granite_01.wav","res://assets/audio/sfx/sfx_footstep_granite_02.wav","res://assets/audio/sfx/sfx_footstep_granite_03.wav","res://assets/audio/sfx/sfx_footstep_granite_04.wav","res://assets/audio/sfx/sfx_footstep_granite_05.wav"],"volume_jitter_db":0.8}`
  - row 158: `{"bus":"SFX","cooldown_seconds":0.05,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"footstep_metal","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_footstep_metal_01.wav","resource_paths":["res://assets/audio/sfx/sfx_footstep_metal_01.wav","res://assets/audio/sfx/sfx_footstep_metal_02.wav","res://assets/audio/sfx/sfx_footstep_metal_03.wav","res://assets/audio/sfx/sfx_footstep_metal_04.wav","res://assets/audio/sfx/sfx_footstep_metal_05.wav"],"volume_jitter_db":0.8}`
  - row 159: `{"bus":"SFX","cooldown_seconds":0.05,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"footstep_dirt","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_footstep_dirt_01.wav","resource_paths":["res://assets/audio/sfx/sfx_footstep_dirt_01.wav","res://assets/audio/sfx/sfx_footstep_dirt_02.wav","res://assets/audio/sfx/sfx_footstep_dirt_03.wav","res://assets/audio/sfx/sfx_footstep_dirt_04.wav","res://assets/audio/sfx/sfx_footstep_dirt_05.wav"],"volume_jitter_db":0.8}`
  - row 160: `{"bus":"SFX","cooldown_seconds":0.05,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"footstep_glass","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_footstep_glass_01.wav","resource_paths":["res://assets/audio/sfx/sfx_footstep_glass_01.wav","res://assets/audio/sfx/sfx_footstep_glass_02.wav","res://assets/audio/sfx/sfx_footstep_glass_03.wav","res://assets/audio/sfx/sfx_footstep_glass_04.wav","res://assets/audio/sfx/sfx_footstep_glass_05.wav"],"volume_jitter_db":0.8}`
  - ... 36 additional rows omitted from the compact audit; the complete current file is identified above ...
- Bytes: 112,400; SHA-256: `afec6f7eb662d456667f2e60b41255f689278524593f06d19e93a9d63a207bd0`
- Root keys: `cues, schema_version`
- `cues`: list[196]; union fields: `bus, cooldown_seconds, default_volume_db, fade_in_seconds, fade_out_seconds, fallback_cue_id, id, loop, max_instances, pitch_max, pitch_min, priority, resource_path, resource_paths, volume_jitter_db`
  - row 1: `{"bus":"SFX","cooldown_seconds":1.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_crafting","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_crafting_assemble.mp3","resource_paths":["res://assets/audio/sfx/sfx_crafting_assemble.mp3"],"volume_jitter_db":0.0}`
  - row 2: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_injection","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_injection.mp3","resource_paths":["res://assets/audio/sfx/sfx_injection.mp3"],"volume_jitter_db":0.0}`
  - row 3: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_interrogation_slam","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_interrogation_slam.mp3","resource_paths":["res://assets/audio/sfx/sfx_interrogation_slam.mp3"],"volume_jitter_db":0.0}`
  - row 4: `{"bus":"SFX","cooldown_seconds":0.2,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_item_pickup","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_action_item_pickup_01.wav","resource_paths":["res://assets/audio/sfx/sfx_action_item_pickup_01.wav","res://assets/audio/sfx/sfx_action_item_pickup_02.wav","res://assets/audio/sfx/sfx_action_item_pickup_03.wav","res://assets/audio/sfx/sfx_action_item_pickup_04.wav","res://assets/audio/sfx/sfx_action_item_pickup_05.wav"],"volume_jitter_db":0.0}`
  - row 5: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_pill_bottle","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_pill_bottle.mp3","resource_paths":["res://assets/audio/sfx/sfx_pill_bottle.mp3"],"volume_jitter_db":0.0}`
  - row 6: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_repair","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_repair_wrench.mp3","resource_paths":["res://assets/audio/sfx/sfx_repair_wrench.mp3"],"volume_jitter_db":0.0}`
  - row 7: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_trade","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_trade_exchange.mp3","resource_paths":["res://assets/audio/sfx/sfx_trade_exchange.mp3"],"volume_jitter_db":0.0}`
  - row 8: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"action_water_pour","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_water_pour.mp3","resource_paths":["res://assets/audio/sfx/sfx_water_pour.mp3"],"volume_jitter_db":0.0}`
  - row 9: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_bunker","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/bunker_ambience.ogg","resource_paths":["res://assets/audio/ambience/bunker_ambience.ogg"],"volume_jitter_db":0.0}`
  - row 10: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-4.0,"fade_in_seconds":1.5,"fade_out_seconds":1.5,"fallback_cue_id":"amb_bunker","id":"amb_bunker_low_power_loop","loop":true,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":20,"resource_path":"res://assets/audio/ambience/amb_bunker_low_power_loop.ogg","resource_paths":["res://assets/audio/ambience/amb_bunker_low_power_loop.ogg"],"volume_jitter_db":0.0}`
  - row 11: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_abandoned_hospital","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_abandoned_hospital.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_abandoned_hospital.mp3"],"volume_jitter_db":0.0}`
  - row 12: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_arcology_sector","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_arcology_sector.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_arcology_sector.mp3"],"volume_jitter_db":0.0}`
  - row 13: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_geothermal_ruins","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_geothermal_ruins.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_geothermal_ruins.mp3"],"volume_jitter_db":0.0}`
  - row 14: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_military_bunker","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_military_bunker.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_military_bunker.mp3"],"volume_jitter_db":0.0}`
  - row 15: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_rural_gas_station","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_rural_gas_station.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_rural_gas_station.mp3"],"volume_jitter_db":0.0}`
  - row 16: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_loc_suburban_ruins","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_loc_suburban_ruins.mp3","resource_paths":["res://assets/audio/ambience/amb_loc_suburban_ruins.mp3"],"volume_jitter_db":0.0}`
  - row 17: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_surface","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/surface_ambience.ogg","resource_paths":["res://assets/audio/ambience/surface_ambience.ogg"],"volume_jitter_db":0.0}`
  - row 18: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-2.0,"fade_in_seconds":1.5,"fade_out_seconds":1.5,"fallback_cue_id":"amb_surface_storm","id":"amb_surface_ashfall_loop","loop":true,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":20,"resource_path":"res://assets/audio/ambience/amb_surface_ashfall_loop.ogg","resource_paths":["res://assets/audio/ambience/amb_surface_ashfall_loop.ogg"],"volume_jitter_db":0.0}`
  - row 19: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-2.0,"fade_in_seconds":1.5,"fade_out_seconds":1.5,"fallback_cue_id":"amb_surface_storm","id":"amb_surface_blizzard_loop","loop":true,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":20,"resource_path":"res://assets/audio/ambience/amb_surface_blizzard_loop.ogg","resource_paths":["res://assets/audio/ambience/amb_surface_blizzard_loop.ogg"],"volume_jitter_db":0.0}`
  - row 20: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-2.0,"fade_in_seconds":1.5,"fade_out_seconds":1.5,"fallback_cue_id":"amb_surface_storm","id":"amb_surface_fallout_storm_loop","loop":true,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":20,"resource_path":"res://assets/audio/ambience/amb_surface_fallout_storm_loop.ogg","resource_paths":["res://assets/audio/ambience/amb_surface_fallout_storm_loop.ogg"],"volume_jitter_db":0.0}`
  - row 21: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-7.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_surface_storm","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_surface_storm.wav","resource_paths":["res://assets/audio/ambience/amb_surface_storm.wav"],"volume_jitter_db":0.0}`
  - row 22: `{"bus":"Surface","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"amb_warzone_distant_shelling","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_warzone_distant_shelling.mp3","resource_paths":["res://assets/audio/ambience/amb_warzone_distant_shelling.mp3"],"volume_jitter_db":0.0}`
  - row 23: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":"med_heartbeat","id":"bio_mutation_pulse","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_mutation_pulse.mp3","resource_paths":["res://assets/audio/sfx/sfx_mutation_pulse.mp3"],"volume_jitter_db":0.0}`
  - row 24: `{"bus":"SFX","cooldown_seconds":0.08,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_casing_drop","loop":false,"max_instances":4,"pitch_max":1.06,"pitch_min":0.94,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_shell_casing_drop_01.wav","resource_paths":["res://assets/audio/sfx/sfx_shell_casing_drop_01.wav","res://assets/audio/sfx/sfx_shell_casing_drop_02.wav"],"volume_jitter_db":0.0}`
  - row 25: `{"bus":"SFX","cooldown_seconds":1.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_decon_flush","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_decon_spray.wav","resource_paths":["res://assets/audio/sfx/sfx_combat_decon_spray.wav"],"volume_jitter_db":0.0}`
  - row 26: `{"bus":"Music","cooldown_seconds":5.0,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_defeat","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_defeat.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_defeat.mp3"],"volume_jitter_db":0.0}`
  - row 27: `{"bus":"SFX","cooldown_seconds":1.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_downed","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_downed.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_downed.mp3"],"volume_jitter_db":0.0}`
  - row 28: `{"bus":"SFX","cooldown_seconds":0.15,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_dry_fire","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_dry_fire_click.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_dry_fire_click.wav"],"volume_jitter_db":0.0}`
  - row 29: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_fire","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_gunshot.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_gunshot.mp3"],"volume_jitter_db":0.8}`
  - row 30: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_hit","loop":false,"max_instances":4,"pitch_max":1.06,"pitch_min":0.94,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_hit.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_hit.mp3"],"volume_jitter_db":1.0}`
  - row 31: `{"bus":"SFX","cooldown_seconds":0.1,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_impact_concrete","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_impact_concrete_crack.wav","resource_paths":["res://assets/audio/sfx/sfx_impact_concrete_crack.wav"],"volume_jitter_db":0.0}`
  - row 32: `{"bus":"SFX","cooldown_seconds":0.1,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_impact_metal","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_impact_metal_ricochet.wav","resource_paths":["res://assets/audio/sfx/sfx_impact_metal_ricochet.wav"],"volume_jitter_db":0.0}`
  - row 33: `{"bus":"SFX","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_impact_wood","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_impact_wood_splinter.wav","resource_paths":["res://assets/audio/sfx/sfx_impact_wood_splinter.wav"],"volume_jitter_db":0.0}`
  - row 34: `{"bus":"Alerts","cooldown_seconds":1.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_improvised_fire","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_molotov_burst.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_molotov_burst.wav"],"volume_jitter_db":0.0}`
  - row 35: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_improvised_spear","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_rebar_spear_thud.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_rebar_spear_thud.wav"],"volume_jitter_db":0.0}`
  - row 36: `{"bus":"SFX","cooldown_seconds":1.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_jam","loop":false,"max_instances":4,"pitch_max":1.02,"pitch_min":0.98,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_jam.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_jam.mp3"],"volume_jitter_db":0.0}`
  - row 37: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_last_stand","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_last_stand.wav","resource_paths":["res://assets/audio/sfx/sfx_combat_last_stand.wav"],"volume_jitter_db":0.0}`
  - row 38: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_reload","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_reload.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_reload.mp3"],"volume_jitter_db":0.0}`
  - row 39: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_start","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_start.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_start.mp3"],"volume_jitter_db":0.0}`
  - row 40: `{"bus":"SFX","cooldown_seconds":5.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_victory","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_combat_victory.mp3","resource_paths":["res://assets/audio/sfx/sfx_combat_victory.mp3"],"volume_jitter_db":0.0}`
  - row 41: `{"bus":"Alerts","cooldown_seconds":1.0,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"combat_weapon_burst","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_burst_rupture.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_burst_rupture.wav"],"volume_jitter_db":0.0}`
  - row 42: `{"bus":"Alerts","cooldown_seconds":10.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"danger_alarm_klaxon","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_danger_alarm_klaxon.wav","resource_paths":["res://assets/audio/sfx/sfx_danger_alarm_klaxon.wav"],"volume_jitter_db":0.0}`
  - row 43: `{"bus":"SFX","cooldown_seconds":3.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"danger_debris","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_debris_impact.mp3","resource_paths":["res://assets/audio/sfx/sfx_debris_impact.mp3"],"volume_jitter_db":0.0}`
  - row 44: `{"bus":"SFX","cooldown_seconds":15.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"danger_explosion","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_danger_explosion_01.wav","resource_paths":["res://assets/audio/sfx/sfx_danger_explosion_01.wav","res://assets/audio/sfx/sfx_danger_explosion_02.wav","res://assets/audio/sfx/sfx_danger_explosion_03.wav","res://assets/audio/sfx/sfx_danger_explosion_04.wav","res://assets/audio/sfx/sfx_danger_explosion_05.wav"],"volume_jitter_db":0.0}`
  - row 45: `{"bus":"SFX","cooldown_seconds":1.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"danger_glass_break","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_glass_break_small.mp3","resource_paths":["res://assets/audio/sfx/sfx_glass_break_small.mp3"],"volume_jitter_db":0.0}`
  - row 46: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"day_transition","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_day_bell.mp3","resource_paths":["res://assets/audio/sfx/sfx_day_bell.mp3"],"volume_jitter_db":0.0}`
  - row 47: `{"bus":"Alerts","cooldown_seconds":3.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"echo_discovery","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_echo_memory_shimmer.wav","resource_paths":["res://assets/audio/sfx/sfx_echo_memory_shimmer.wav"],"volume_jitter_db":0.0}`
  - row 48: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-10.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_camp_fire","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ambience/amb_expedition_camp_fire.wav","resource_paths":["res://assets/audio/ambience/amb_expedition_camp_fire.wav"],"volume_jitter_db":0.0}`
  - row 49: `{"bus":"Alerts","cooldown_seconds":2.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_breakdown","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_breakdown_stall.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_breakdown_stall.wav"],"volume_jitter_db":0.0}`
  - row 50: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-10.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_dirtbike","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_engine_dirtbike.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_engine_dirtbike.wav"],"volume_jitter_db":0.0}`
  - row 51: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-12.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_engine","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_engine_diesel.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_engine_diesel.wav"],"volume_jitter_db":0.0}`
  - row 52: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_refuel","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_refuel.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_refuel.wav"],"volume_jitter_db":0.0}`
  - row 53: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_repair","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_repair.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_repair.wav"],"volume_jitter_db":0.0}`
  - row 54: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-11.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"expedition_vehicle_truck","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_vehicle_engine_truck.wav","resource_paths":["res://assets/audio/sfx/sfx_vehicle_engine_truck.wav"],"volume_jitter_db":0.0}`
  - row 55: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"flashback_grounded","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_flashback_grounded.wav","resource_paths":["res://assets/audio/sfx/sfx_flashback_grounded.wav"],"volume_jitter_db":0.0}`
  - row 56: `{"bus":"Alerts","cooldown_seconds":3.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"flashback_trigger","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_flashback_distortion.wav","resource_paths":["res://assets/audio/sfx/sfx_flashback_distortion.wav"],"volume_jitter_db":0.0}`
  - row 57: `{"bus":"Music","cooldown_seconds":0.0,"default_volume_db":-10.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"game_over","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/music/game_over.ogg","resource_paths":["res://assets/audio/music/game_over.ogg"],"volume_jitter_db":0.0}`
  - row 58: `{"bus":"Alerts","cooldown_seconds":1.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"hazard_toxic_sizzle","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_hazard_toxic_sizzle.mp3","resource_paths":["res://assets/audio/sfx/sfx_hazard_toxic_sizzle.mp3"],"volume_jitter_db":0.0}`
  - row 59: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"item_handling_ammo","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_item_handling_ammo_01.wav","resource_paths":["res://assets/audio/sfx/sfx_item_handling_ammo_01.wav","res://assets/audio/sfx/sfx_item_handling_ammo_02.wav","res://assets/audio/sfx/sfx_item_handling_ammo_03.wav","res://assets/audio/sfx/sfx_item_handling_ammo_04.wav","res://assets/audio/sfx/sfx_item_handling_ammo_05.wav"],"volume_jitter_db":0.0}`
  - row 60: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"item_handling_meds","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_item_handling_meds_01.wav","resource_paths":["res://assets/audio/sfx/sfx_item_handling_meds_01.wav","res://assets/audio/sfx/sfx_item_handling_meds_02.wav","res://assets/audio/sfx/sfx_item_handling_meds_03.wav","res://assets/audio/sfx/sfx_item_handling_meds_04.wav","res://assets/audio/sfx/sfx_item_handling_meds_05.wav"],"volume_jitter_db":0.0}`
  - row 61: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"item_handling_ration","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_item_handling_ration_01.wav","resource_paths":["res://assets/audio/sfx/sfx_item_handling_ration_01.wav","res://assets/audio/sfx/sfx_item_handling_ration_02.wav","res://assets/audio/sfx/sfx_item_handling_ration_03.wav","res://assets/audio/sfx/sfx_item_handling_ration_04.wav","res://assets/audio/sfx/sfx_item_handling_ration_05.wav"],"volume_jitter_db":0.0}`
  - row 62: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"log_tape_button","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_tape_deck_button.wav","resource_paths":["res://assets/audio/sfx/sfx_tape_deck_button.wav"],"volume_jitter_db":0.0}`
  - row 63: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-18.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"log_tape_hiss","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_tape_hiss_loop.wav","resource_paths":["res://assets/audio/sfx/sfx_tape_hiss_loop.wav"],"volume_jitter_db":0.0}`
  - row 64: `{"bus":"UI","cooldown_seconds":0.3,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"log_tape_insert","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_tape_deck_insert.wav","resource_paths":["res://assets/audio/sfx/sfx_tape_deck_insert.wav"],"volume_jitter_db":0.0}`
  - row 65: `{"bus":"UI","cooldown_seconds":0.2,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"log_tape_rewind","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_tape_rewind.wav","resource_paths":["res://assets/audio/sfx/sfx_tape_rewind.wav"],"volume_jitter_db":0.0}`
  - row 66: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"log_tape_stop","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_tape_stop.wav","resource_paths":["res://assets/audio/sfx/sfx_tape_stop.wav"],"volume_jitter_db":0.0}`
  - row 67: `{"bus":"SFX","cooldown_seconds":8.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_coughing","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_coughing_fit.mp3","resource_paths":["res://assets/audio/sfx/sfx_coughing_fit.mp3"],"volume_jitter_db":0.0}`
  - row 68: `{"bus":"SFX","cooldown_seconds":5.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_heartbeat","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_heartbeat_slow.mp3","resource_paths":["res://assets/audio/sfx/sfx_heartbeat_slow.mp3"],"volume_jitter_db":0.0}`
  - row 69: `{"bus":"Medical","cooldown_seconds":1.5,"default_volume_db":-12.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_infirmary_beep","loop":false,"max_instances":4,"pitch_max":1.02,"pitch_min":0.98,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_infirmary_monitor_beep.wav","resource_paths":["res://assets/audio/sfx/sfx_infirmary_monitor_beep.wav"],"volume_jitter_db":0.0}`
  - row 70: `{"bus":"Medical","cooldown_seconds":0.75,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_quarantine_clear","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_med_quarantine_clear.wav","resource_paths":["res://assets/audio/sfx/sfx_med_quarantine_clear.wav"],"volume_jitter_db":0.0}`
  - row 71: `{"bus":"Medical","cooldown_seconds":1.0,"default_volume_db":-7.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_quarantine_seal","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_med_quarantine_seal.wav","resource_paths":["res://assets/audio/sfx/sfx_med_quarantine_seal.wav"],"volume_jitter_db":0.0}`
  - row 72: `{"bus":"Medical","cooldown_seconds":3.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"med_survivor_death","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_survivor_death.wav","resource_paths":["res://assets/audio/sfx/sfx_survivor_death.wav"],"volume_jitter_db":0.0}`
  - row 73: `{"bus":"Music","cooldown_seconds":0.0,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"music_gameplay","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/music/gameplay_underscore.ogg","resource_paths":["res://assets/audio/music/gameplay_underscore.ogg"],"volume_jitter_db":0.0}`
  - row 74: `{"bus":"Music","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"music_menu","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/music/main_menu.ogg","resource_paths":["res://assets/audio/music/main_menu.ogg"],"volume_jitter_db":0.0}`
  - row 75: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_alert_acute","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_radiation_alarm.mp3","resource_paths":["res://assets/audio/sfx/sfx_radiation_alarm.mp3","res://assets/audio/sfx/radiation_alert.wav"],"volume_jitter_db":0.0}`
  - row 76: `{"bus":"Alerts","cooldown_seconds":10.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_alert_chronic","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_radiation_chronic_alarm.wav","resource_paths":["res://assets/audio/sfx/sfx_radiation_chronic_alarm.wav"],"volume_jitter_db":0.0}`
  - row 77: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_contamination","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_contamination_warning.mp3","resource_paths":["res://assets/audio/sfx/sfx_contamination_warning.mp3"],"volume_jitter_db":0.0}`
  - row 78: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_geiger_burst","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_geiger_burst.mp3","resource_paths":["res://assets/audio/sfx/sfx_geiger_burst.mp3"],"volume_jitter_db":0.0}`
  - row 79: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_geiger_intense","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_geiger_intense_crackling.wav","resource_paths":["res://assets/audio/sfx/sfx_geiger_intense_crackling.wav"],"volume_jitter_db":0.0}`
  - row 80: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-10.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"rad_geiger_loop","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/geiger.wav","resource_paths":["res://assets/audio/sfx/geiger.wav"],"volume_jitter_db":0.0}`
  - row 81: `{"bus":"Voice","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_dead_hand_pulse","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/radio_dead_hand_pulse.wav","resource_paths":["res://assets/audio/radio/radio_dead_hand_pulse.wav"],"volume_jitter_db":0.0}`
  - row 82: `{"bus":"Voice","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_distress_beacon","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/radio_distress_beacon.wav","resource_paths":["res://assets/audio/radio/radio_distress_beacon.wav"],"volume_jitter_db":0.0}`
  - row 83: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_ebs_alert","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/radio_ebs_alert.wav","resource_paths":["res://assets/audio/radio/radio_ebs_alert.wav"],"volume_jitter_db":0.0}`
  - row 84: `{"bus":"Voice","cooldown_seconds":0.5,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_morse","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_morse_key.mp3","resource_paths":["res://assets/audio/sfx/sfx_morse_key.mp3"],"volume_jitter_db":0.0}`
  - row 85: `{"bus":"Voice","cooldown_seconds":0.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_numbers_station","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/radio_numbers_station.wav","resource_paths":["res://assets/audio/radio/radio_numbers_station.wav"],"volume_jitter_db":0.0}`
  - row 86: `{"bus":"Voice","cooldown_seconds":1.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_signal_lock","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_radio_signal_lock.mp3","resource_paths":["res://assets/audio/sfx/sfx_radio_signal_lock.mp3"],"volume_jitter_db":0.0}`
  - row 87: `{"bus":"Voice","cooldown_seconds":0.5,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_static","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/radio_static_hiss.wav","resource_paths":["res://assets/audio/radio/radio_static_hiss.wav"],"volume_jitter_db":0.0}`
  - row 88: `{"bus":"Voice","cooldown_seconds":1.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_tune","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_radio_tune.mp3","resource_paths":["res://assets/audio/sfx/sfx_radio_tune.mp3"],"volume_jitter_db":0.0}`
  - row 89: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-2.3,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_ch11_stockpile","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_ch11_stockpile.wav","resource_paths":["res://assets/audio/radio/vo_ch11_stockpile.wav"],"volume_jitter_db":0.0}`
  - row 90: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_ch3_ash_road","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":90,"resource_path":"res://assets/audio/radio/vo_ch3_ash_road_elevenlabs_v1.wav","resource_paths":["res://assets/audio/radio/vo_ch3_ash_road_elevenlabs_v1.wav"],"volume_jitter_db":0.0}`
  - row 91: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_ch7_milband","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":90,"resource_path":"res://assets/audio/radio/vo_ch7_milband_elevenlabs_v1.wav","resource_paths":["res://assets/audio/radio/vo_ch7_milband_elevenlabs_v1.wav"],"volume_jitter_db":0.0}`
  - row 92: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_kind_hatch","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_kind_hatch_relay.wav","resource_paths":["res://assets/audio/radio/vo_kind_hatch_relay.wav","res://assets/audio/radio/vo_kind_hatch.wav"],"volume_jitter_db":0.0}`
  - row 93: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_kind_parley","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_kind_parley_beacon.wav","resource_paths":["res://assets/audio/radio/vo_kind_parley_beacon.wav","res://assets/audio/radio/vo_kind_parley.wav"],"volume_jitter_db":0.0}`
  - row 94: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_verdict_count","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_verdict_count.wav","resource_paths":["res://assets/audio/radio/vo_verdict_count.wav"],"volume_jitter_db":0.0}`
  - row 95: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_verdict_eden","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_verdict_eden.wav","resource_paths":["res://assets/audio/radio/vo_verdict_eden.wav"],"volume_jitter_db":0.0}`
  - row 96: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_verdict_geophone","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_verdict_geophone.wav","resource_paths":["res://assets/audio/radio/vo_verdict_geophone.wav"],"volume_jitter_db":0.0}`
  - row 97: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_verdict_meter","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_verdict_meter.wav","resource_paths":["res://assets/audio/radio/vo_verdict_meter.wav"],"volume_jitter_db":0.0}`
  - row 98: `{"bus":"Voice","cooldown_seconds":2.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"radio_vo_verdict_reckoning","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/radio/vo_verdict_reckoning.wav","resource_paths":["res://assets/audio/radio/vo_verdict_reckoning.wav"],"volume_jitter_db":0.0}`
  - row 99: `{"bus":"UI","cooldown_seconds":1.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"save_success","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ui/ui_save_success.wav","resource_paths":["res://assets/audio/ui/ui_save_success.wav"],"volume_jitter_db":0.0}`
  - row 100: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_airlock_purge_cycle","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_airlock_purge_cycle.mp3","resource_paths":["res://assets/audio/sfx/sfx_airlock_purge_cycle.mp3"],"volume_jitter_db":0.0}`
  - row 101: `{"bus":"Alerts","cooldown_seconds":8.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_artillery_incoming_whistle","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_artillery_incoming_whistle.mp3","resource_paths":["res://assets/audio/sfx/sfx_artillery_incoming_whistle.mp3"],"volume_jitter_db":0.0}`
  - row 102: `{"bus":"Alerts","cooldown_seconds":1.0,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":"danger_alarm_klaxon","id":"sfx_breaker_trip","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":85,"resource_path":"res://assets/audio/sfx/sfx_breaker_trip.wav","resource_paths":["res://assets/audio/sfx/sfx_breaker_trip.wav"],"volume_jitter_db":0.0}`
  - row 103: `{"bus":"SFX","cooldown_seconds":0.2,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_bullet_whiz_ricochet","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_bullet_whiz_ricochet.mp3","resource_paths":["res://assets/audio/sfx/sfx_bullet_whiz_ricochet.mp3"],"volume_jitter_db":0.0}`
  - row 104: `{"bus":"SFX","cooldown_seconds":5.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_distant_artillery_barrage","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_distant_artillery_barrage_01.wav","resource_paths":["res://assets/audio/sfx/sfx_distant_artillery_barrage_01.wav","res://assets/audio/sfx/sfx_distant_artillery_barrage_02.wav","res://assets/audio/sfx/sfx_distant_artillery_barrage_03.wav","res://assets/audio/sfx/sfx_distant_artillery_barrage_04.wav","res://assets/audio/sfx/sfx_distant_artillery_barrage_05.wav"]…`
  - row 105: `{"bus":"SFX","cooldown_seconds":4.0,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_distant_gunfire_skirmish","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_distant_gunfire_skirmish_01.wav","resource_paths":["res://assets/audio/sfx/sfx_distant_gunfire_skirmish_01.wav","res://assets/audio/sfx/sfx_distant_gunfire_skirmish_02.wav","res://assets/audio/sfx/sfx_distant_gunfire_skirmish_03.wav","res://assets/audio/sfx/sfx_distant_gunfire_skirmish_04.wav","res://assets/audio/sfx/sfx_distant_gunfire_skirmish_05.wav"],"volum…`
  - row 106: `{"bus":"SFX","cooldown_seconds":6.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_distant_mortar_launch","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_distant_mortar_launch.mp3","resource_paths":["res://assets/audio/sfx/sfx_distant_mortar_launch.mp3"],"volume_jitter_db":0.0}`
  - row 107: `{"bus":"Generator","cooldown_seconds":1.0,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_generator_start","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":60,"resource_path":"res://assets/audio/sfx/sfx_generator_start.wav","resource_paths":["res://assets/audio/sfx/sfx_generator_start.wav"],"volume_jitter_db":0.0}`
  - row 108: `{"bus":"Generator","cooldown_seconds":1.0,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_generator_stop","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":60,"resource_path":"res://assets/audio/sfx/sfx_generator_stop.wav","resource_paths":["res://assets/audio/sfx/sfx_generator_stop.wav"],"volume_jitter_db":0.0}`
  - row 109: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_heavy_impact_fall","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_heavy_impact_fall.mp3","resource_paths":["res://assets/audio/sfx/sfx_heavy_impact_fall.mp3"],"volume_jitter_db":0.0}`
  - row 110: `{"bus":"Generator","cooldown_seconds":1.0,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":"ui_confirm","id":"sfx_power_restore","loop":false,"max_instances":1,"pitch_max":1.0,"pitch_min":1.0,"priority":60,"resource_path":"res://assets/audio/sfx/sfx_power_restore.wav","resource_paths":["res://assets/audio/sfx/sfx_power_restore.wav"],"volume_jitter_db":0.0}`
  - row 111: `{"bus":"SFX","cooldown_seconds":5.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_structural_collapse","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_structural_collapse.mp3","resource_paths":["res://assets/audio/sfx/sfx_structural_collapse.mp3"],"volume_jitter_db":0.0}`
  - row 112: `{"bus":"SFX","cooldown_seconds":0.25,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_assault_rifle_burst","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_assault_rifle_burst_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_assault_rifle_burst_01.wav","res://assets/audio/sfx/sfx_weapon_assault_rifle_burst_02.wav","res://assets/audio/sfx/sfx_weapon_assault_rifle_burst_03.wav","res://assets/audio/sfx/sfx_weapon_assault_rifle_burst_04.wav","res://assets/audio/sfx/sfx_weapon_assault_rifle_burs…`
  - row 113: `{"bus":"SFX","cooldown_seconds":0.4,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_bolt_rifle_report","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_01.wav","res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_02.wav","res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_03.wav","res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_04.wav","res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_05.wav"],"vol…`
  - row 114: `{"bus":"SFX","cooldown_seconds":0.15,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_cz75_report","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_cz75_report_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_cz75_report_01.wav","res://assets/audio/sfx/sfx_weapon_cz75_report_02.wav","res://assets/audio/sfx/sfx_weapon_cz75_report_03.wav","res://assets/audio/sfx/sfx_weapon_cz75_report_04.wav","res://assets/audio/sfx/sfx_weapon_cz75_report_05.wav"],"volume_jitter_db":0.7}`
  - row 115: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_lmg_burst","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_lmg_burst.mp3","resource_paths":["res://assets/audio/sfx/sfx_weapon_lmg_burst.mp3"],"volume_jitter_db":0.7}`
  - row 116: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_pipe_rifle_report","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_01.wav","res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_02.wav","res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_03.wav","res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_04.wav","res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_05.wav"],"vol…`
  - row 117: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_scrap_shotgun_report","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report_01.wav","res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report_02.wav","res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report_03.wav","res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report_04.wav","res://assets/audio/sfx/sfx_weapon_scrap_shotgun…`
  - row 118: `{"bus":"SFX","cooldown_seconds":0.3,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_shotgun_rack","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_shotgun_rack.mp3","resource_paths":["res://assets/audio/sfx/sfx_weapon_shotgun_rack.mp3"],"volume_jitter_db":0.0}`
  - row 119: `{"bus":"SFX","cooldown_seconds":0.5,"default_volume_db":-1.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"sfx_weapon_sniper_heavy_report","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weapon_sniper_heavy_report_01.wav","resource_paths":["res://assets/audio/sfx/sfx_weapon_sniper_heavy_report_01.wav","res://assets/audio/sfx/sfx_weapon_sniper_heavy_report_02.wav","res://assets/audio/sfx/sfx_weapon_sniper_heavy_report_03.wav","res://assets/audio/sfx/sfx_weapon_sniper_heavy_report_04.wav","res://assets/audio/sfx/sfx_weapon_sniper_heavy_report…`
  - row 120: `{"bus":"Alerts","cooldown_seconds":10.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_air_filter","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_air_filter_degrade.mp3","resource_paths":["res://assets/audio/sfx/sfx_air_filter_degrade.mp3"],"volume_jitter_db":0.0}`
  - row 121: `{"bus":"Ventilation","cooldown_seconds":0.0,"default_volume_db":-14.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_air_recycler","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_air_recycler_hiss.wav","resource_paths":["res://assets/audio/sfx/sfx_air_recycler_hiss.wav"],"volume_jitter_db":0.0}`
  - row 122: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_door_open","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_bunker_door_open.mp3","resource_paths":["res://assets/audio/sfx/sfx_bunker_door_open.mp3"],"volume_jitter_db":0.0}`
  - row 123: `{"bus":"SFX","cooldown_seconds":2.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_door_seal","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_bunker_door_seal.mp3","resource_paths":["res://assets/audio/sfx/sfx_bunker_door_seal.mp3"],"volume_jitter_db":0.0}`
  - row 124: `{"bus":"Generator","cooldown_seconds":0.0,"default_volume_db":-16.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_generator","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_generator_cough.mp3","resource_paths":["res://assets/audio/sfx/sfx_generator_cough.mp3"],"volume_jitter_db":0.0}`
  - row 125: `{"bus":"Generator","cooldown_seconds":0.0,"default_volume_db":-14.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_generator_strain","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_generator_heavy_strain.wav","resource_paths":["res://assets/audio/sfx/sfx_generator_heavy_strain.wav"],"volume_jitter_db":0.0}`
  - row 126: `{"bus":"SFX","cooldown_seconds":5.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_pipe_clang","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_pipe_clang.mp3","resource_paths":["res://assets/audio/sfx/sfx_pipe_clang.mp3"],"volume_jitter_db":0.0}`
  - row 127: `{"bus":"Ventilation","cooldown_seconds":0.0,"default_volume_db":-12.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_ventilation","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_ventilation_fan.mp3","resource_paths":["res://assets/audio/sfx/sfx_ventilation_fan.mp3"],"volume_jitter_db":0.0}`
  - row 128: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-15.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_water_drip","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_water_drip_cave.mp3","resource_paths":["res://assets/audio/sfx/sfx_water_drip_cave.mp3"],"volume_jitter_db":0.0}`
  - row 129: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-15.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_water_filtration","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_water_filtration_loop.wav","resource_paths":["res://assets/audio/sfx/sfx_water_filtration_loop.wav"],"volume_jitter_db":0.0}`
  - row 130: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-16.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"shelter_workshop_tools","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_workshop_lathe_hum.wav","resource_paths":["res://assets/audio/sfx/sfx_workshop_lathe_hum.wav"],"volume_jitter_db":0.0}`
  - row 131: `{"bus":"Alerts","cooldown_seconds":2.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":"expedition_vehicle_breakdown","id":"train_screech_crash","loop":false,"max_instances":2,"pitch_max":1.1,"pitch_min":0.9,"priority":80,"resource_path":"res://assets/audio/sfx/sfx_train_screech_crash.wav","resource_paths":["res://assets/audio/sfx/sfx_train_screech_crash.wav"],"volume_jitter_db":0.0}`
  - row 132: `{"bus":"Ambience","cooldown_seconds":0.0,"default_volume_db":-14.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"trauma_cabin_fever","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_trauma_cabin_fever_whisper.wav","resource_paths":["res://assets/audio/sfx/sfx_trauma_cabin_fever_whisper.wav"],"volume_jitter_db":0.0}`
  - row 133: `{"bus":"SFX","cooldown_seconds":0.0,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"trauma_heartbeat_rapid","loop":true,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_trauma_heartbeat_rapid.wav","resource_paths":["res://assets/audio/sfx/sfx_trauma_heartbeat_rapid.wav"],"volume_jitter_db":0.0}`
  - row 134: `{"bus":"Alerts","cooldown_seconds":4.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"trauma_tinnitus","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_trauma_tinnitus_ring.wav","resource_paths":["res://assets/audio/sfx/sfx_trauma_tinnitus_ring.wav"],"volume_jitter_db":0.0}`
  - row 135: `{"bus":"UI","cooldown_seconds":0.05,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_cancel","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_cancel.wav","resource_paths":["res://assets/audio/ui/ui_cancel.wav"],"volume_jitter_db":0.0}`
  - row 136: `{"bus":"UI","cooldown_seconds":0.05,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_click","loop":false,"max_instances":4,"pitch_max":1.05,"pitch_min":0.95,"priority":50,"resource_path":"res://assets/audio/ui/ui_click.wav","resource_paths":["res://assets/audio/ui/ui_click.wav","res://assets/audio/ui/ui_click_01.wav","res://assets/audio/ui/ui_click_02.wav","res://assets/audio/ui/ui_click_03.wav","res://assets/audio/ui/ui_click_04.wav"],"volume_jitter_db":0.8}`
  - row 137: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_confirm","loop":false,"max_instances":4,"pitch_max":1.02,"pitch_min":0.98,"priority":50,"resource_path":"res://assets/audio/ui/ui_confirm.wav","resource_paths":["res://assets/audio/ui/ui_confirm.wav"],"volume_jitter_db":0.0}`
  - row 138: `{"bus":"UI","cooldown_seconds":1.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_crt_power_on","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ui/ui_crt_power_on.wav","resource_paths":["res://assets/audio/ui/ui_crt_power_on.wav"],"volume_jitter_db":0.0}`
  - row 139: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_drawer_slide","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_drawer_slide.wav","resource_paths":["res://assets/audio/ui/ui_drawer_slide.wav"],"volume_jitter_db":0.0}`
  - row 140: `{"bus":"UI","cooldown_seconds":0.5,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_invalid_action","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/ui/ui_invalid_action.wav","resource_paths":["res://assets/audio/ui/ui_invalid_action.wav"],"volume_jitter_db":0.0}`
  - row 141: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_modal_close","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/ui/ui_modal_close.wav","resource_paths":["res://assets/audio/ui/ui_modal_close.wav"],"volume_jitter_db":0.0}`
  - row 142: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_modal_open","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/ui/ui_modal_open.wav","resource_paths":["res://assets/audio/ui/ui_modal_open.wav"],"volume_jitter_db":0.0}`
  - row 143: `{"bus":"UI","cooldown_seconds":0.1,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_paper_rustle","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/ui/ui_paper_rustle.wav","resource_paths":["res://assets/audio/ui/ui_paper_rustle.wav"],"volume_jitter_db":0.0}`
  - row 144: `{"bus":"UI","cooldown_seconds":0.05,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_rotary_click","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_rotary_click.wav","resource_paths":["res://assets/audio/ui/ui_rotary_click.wav"],"volume_jitter_db":0.0}`
  - row 145: `{"bus":"UI","cooldown_seconds":0.2,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_stamp_heavy","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_stamp_heavy.wav","resource_paths":["res://assets/audio/ui/ui_stamp_heavy.wav"],"volume_jitter_db":0.0}`
  - row 146: `{"bus":"UI","cooldown_seconds":0.05,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_switch_toggle","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_switch_toggle.wav","resource_paths":["res://assets/audio/ui/ui_switch_toggle.wav"],"volume_jitter_db":0.0}`
  - row 147: `{"bus":"UI","cooldown_seconds":0.05,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_tab_change","loop":false,"max_instances":4,"pitch_max":1.03,"pitch_min":0.97,"priority":50,"resource_path":"res://assets/audio/ui/ui_tab_change.wav","resource_paths":["res://assets/audio/ui/ui_tab_change.wav"],"volume_jitter_db":0.0}`
  - row 148: `{"bus":"UI","cooldown_seconds":0.3,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"ui_warning","loop":false,"max_instances":4,"pitch_max":1.02,"pitch_min":0.98,"priority":50,"resource_path":"res://assets/audio/ui/ui_warning.wav","resource_paths":["res://assets/audio/ui/ui_warning.wav"],"volume_jitter_db":0.0}`
  - row 149: `{"bus":"Alerts","cooldown_seconds":5.0,"default_volume_db":-2.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_alert","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_alarm_klaxon.mp3","resource_paths":["res://assets/audio/sfx/sfx_alarm_klaxon.mp3"],"volume_jitter_db":0.0}`
  - row 150: `{"bus":"Alerts","cooldown_seconds":10.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_black_rain","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weather_black_rain.wav","resource_paths":["res://assets/audio/sfx/sfx_weather_black_rain.wav"],"volume_jitter_db":0.0}`
  - row 151: `{"bus":"SFX","cooldown_seconds":10.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_blizzard","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weather_blizzard.wav","resource_paths":["res://assets/audio/sfx/sfx_weather_blizzard.wav"],"volume_jitter_db":0.0}`
  - row 152: `{"bus":"Alerts","cooldown_seconds":8.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_corrosive_precipitation","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weather_corrosive_precipitation.wav","resource_paths":["res://assets/audio/sfx/sfx_weather_corrosive_precipitation.wav"],"volume_jitter_db":0.0}`
  - row 153: `{"bus":"Alerts","cooldown_seconds":8.0,"default_volume_db":-4.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_emp_storm","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weather_emp_storm.wav","resource_paths":["res://assets/audio/sfx/sfx_weather_emp_storm.wav"],"volume_jitter_db":0.0}`
  - row 154: `{"bus":"SFX","cooldown_seconds":10.0,"default_volume_db":0.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_fallout_storm","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_fallout_storm_approach.mp3","resource_paths":["res://assets/audio/sfx/sfx_fallout_storm_approach.mp3"],"volume_jitter_db":0.0}`
  - row 155: `{"bus":"SFX","cooldown_seconds":8.0,"default_volume_db":-3.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_glass_storm","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_weather_glass_storm.wav","resource_paths":["res://assets/audio/sfx/sfx_weather_glass_storm.wav"],"volume_jitter_db":0.0}`
  - row 156: `{"bus":"SFX","cooldown_seconds":3.0,"default_volume_db":-8.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"weather_wind_gust","loop":false,"max_instances":4,"pitch_max":1.0,"pitch_min":1.0,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_wind_gust_harsh.mp3","resource_paths":["res://assets/audio/sfx/sfx_wind_gust_harsh.mp3"],"volume_jitter_db":0.0}`
  - row 157: `{"bus":"SFX","cooldown_seconds":0.05,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"footstep_granite","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_footstep_granite_01.wav","resource_paths":["res://assets/audio/sfx/sfx_footstep_granite_01.wav","res://assets/audio/sfx/sfx_footstep_granite_02.wav","res://assets/audio/sfx/sfx_footstep_granite_03.wav","res://assets/audio/sfx/sfx_footstep_granite_04.wav","res://assets/audio/sfx/sfx_footstep_granite_05.wav"],"volume_jitter_db":0.8}`
  - row 158: `{"bus":"SFX","cooldown_seconds":0.05,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"footstep_metal","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_footstep_metal_01.wav","resource_paths":["res://assets/audio/sfx/sfx_footstep_metal_01.wav","res://assets/audio/sfx/sfx_footstep_metal_02.wav","res://assets/audio/sfx/sfx_footstep_metal_03.wav","res://assets/audio/sfx/sfx_footstep_metal_04.wav","res://assets/audio/sfx/sfx_footstep_metal_05.wav"],"volume_jitter_db":0.8}`
  - row 159: `{"bus":"SFX","cooldown_seconds":0.05,"default_volume_db":-6.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"footstep_dirt","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_footstep_dirt_01.wav","resource_paths":["res://assets/audio/sfx/sfx_footstep_dirt_01.wav","res://assets/audio/sfx/sfx_footstep_dirt_02.wav","res://assets/audio/sfx/sfx_footstep_dirt_03.wav","res://assets/audio/sfx/sfx_footstep_dirt_04.wav","res://assets/audio/sfx/sfx_footstep_dirt_05.wav"],"volume_jitter_db":0.8}`
  - row 160: `{"bus":"SFX","cooldown_seconds":0.05,"default_volume_db":-5.0,"fade_in_seconds":0.0,"fade_out_seconds":0.0,"fallback_cue_id":null,"id":"footstep_glass","loop":false,"max_instances":4,"pitch_max":1.04,"pitch_min":0.96,"priority":50,"resource_path":"res://assets/audio/sfx/sfx_footstep_glass_01.wav","resource_paths":["res://assets/audio/sfx/sfx_footstep_glass_01.wav","res://assets/audio/sfx/sfx_footstep_glass_02.wav","res://assets/audio/sfx/sfx_footstep_glass_03.wav","res://assets/audio/sfx/sfx_footstep_glass_04.wav","res://assets/audio/sfx/sfx_footstep_glass_05.wav"],"volume_jitter_db":0.8}`
  - ... 36 additional rows omitted from the compact audit; the complete current file is identified above ...

# Appendix D — Current caller/reference graph

### `VerdictCatalogLoader.LoadRadio` (8 sampled current references)
- Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs:75: var loaded = VerdictCatalogLoader.LoadRadio(dataDir, fileIO, json);
- src/Host/VerdictHostSession.cs:121: var radioEntries = VerdictCatalogLoader.LoadRadio(dataDir, s_files, s_json);
- src/Host/HostCli.SelfTests.cs:847: var radioCorpus = VerdictCatalogLoader.LoadRadio(dataDirectory, vio, vjson);
- Ashfall.Core.Tests/VerdictContentWebTests.cs:121: var radio = VerdictCatalogLoader.LoadRadio(
- Ashfall.Core.Tests/Radio/Plan94_102RadioFoundryIntegrationTests.cs:27: var list = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
- Ashfall.Core.Tests/Radio/Plan94_102RadioFoundryIntegrationTests.cs:121: var radioList = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
- Ashfall.Core.Tests/Radio/Plan94_102RadioFoundryIntegrationTests.cs:164: var radioList = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
- Ashfall.Core.Tests/Verdict/VerdictRadioExpansionTests.cs:20: var list = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
### `VerdictRadioSystem.Poll` (0 sampled current references)
- No current C# reference found by the bounded search; this is an explicit unknown, not proof of absence in generated/indirect tooling.
### `VerdictHostSession.TickRadio` (0 sampled current references)
- No current C# reference found by the bounded search; this is an explicit unknown, not proof of absence in generated/indirect tooling.
### `VerdictSave` (18 sampled current references)
- Assets/Ashfall.Core/Radio/RadioSave.cs:215: /// tamper / checksumless / newer-version payloads (mirrors VerdictSaveCodec).
- Assets/Ashfall.Core/Verdict/ReckoningSystem.cs:11: /// persist through VerdictSave. Never reverses (Phase III back to Phase I
- Assets/Ashfall.Core/Verdict/VerdictQuestMigration.cs:12: /// forward Verdict owns its quests in <see cref="VerdictSave.quests"/>;
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:21: /// <see cref="VerdictSaveV1"/> / <see cref="VerdictSaveV2"/>) because
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:26: public class VerdictSave
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:52: public class VerdictSaveV3
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:72: public class VerdictSaveV1
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:89: public class VerdictSaveV2
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:102: public static class VerdictSaveCodec
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:104: public static VerdictSave Capture(
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:115: var save = new VerdictSave
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:131: public static string Encode(VerdictSave save, IJsonSerializer json)
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:144: public static bool TryDecode(string json, IJsonSerializer serializer, out VerdictSave save)
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:150: var decoded = serializer.Deserialize<VerdictSave>(json);
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:152: if (decoded.saveVersion > VerdictSave.CurrentSaveVersion) return false; // newer — reject
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:153: if (decoded.saveVersion < VerdictSave.MigrationFromVersion) return false; // too old — reject
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:169: CatalogDiagnostics.Warn("<decode>", "VerdictSave", ex_CATDIAG);
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:174: private static bool MigrateV1(string json, IJsonSerializer serializer, out VerdictSave save)
### `VerdictDashboardPanel` (10 sampled current references)
- src/Main.PanelLifecycle.cs:98: _verdictDashboardPanel,
- src/Main.UiPanels.cs:163: private VerdictDashboardPanel _verdictDashboardPanel = null!;
- src/Main.UiPanels.cs:1172: _verdictDashboardPanel = new VerdictDashboardPanel { Visible = false };
- src/Main.UiPanels.cs:1173: _verdictDashboardPanel.OnClose += () => _verdictDashboardPanel.Visible = false;
- src/Main.UiPanels.cs:1174: AddChild(_verdictDashboardPanel);
- src/Main.PlayerSurfaces.cs:641: bindAction: () => { SetupVerdict(); _verdictDashboardPanel.Bind(_verdictPanel, _verdict); },
- src/Main.PlayerSurfaces.cs:642: openAction: () => _verdictDashboardPanel.Open(),
- src/Main.PlayerSurfaces.cs:643: closeAction: () => ClosePanelAnimated(_verdictDashboardPanel));
- src/UI/SnapshotHarness.cs:63: new Target{ StableId="verdict_dashboard_default",    Title="Verdict Dashboard (#15 Stitch)",             PanelCtor="AtomicWar.GodotApp.UI.VerdictDashboardPanel",           StateHint="default", Width=1920, Height=1080 },
- src/UI/VerdictDashboardPanel.cs:24: public partial class VerdictDashboardPanel : Control

# Appendix E — Current focused-test inventory

Current test declaration inventory: 54 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/Verdict/VerdictRadioExpansionTests.cs` — 20 test declarations; bytes=10,583; SHA-256=`629c4df3b0e14c0a482bdcbe7b40f33918aaa01393ca3681efe45f1c65812236`
- 00025: [Fact]
- 00026: public void Catalog_Loads_All_30_Broadcasts()
- 00032: [Fact]
- 00033: public void All_30_Broadcast_Ids_Are_Unique_And_Prefixed()
- 00045: [Fact]
- 00046: public void Baseline_13_Broadcasts_Preserved_Verbatim()
- 00079: [Fact]
- 00080: public void All_17_Plan94_New_Broadcasts_Present()
- 00124: [Fact]
- 00125: public void Plan94_Requested_Kind_Distribution_Matches()
- 00146: [Fact]
- 00147: public void Frequency_And_Signal_Strength_Integrity()
- 00160: [Fact]
- 00161: public void DayTrigger_Semantics_And_Chronology()
- 00194: [Fact]
- 00195: public void OneShot_And_State_RoundTrip()
- 00225: [Fact]
- 00226: public void UnifiedRadioBroadcast_Catalog_Loads_Verdict_Broadcasts()
- 00246: [Fact]
- 00247: public void AudioCueIntegrity_No_New_Broadcasts_Define_Dangling_Cues()
### `Ashfall.Core.Tests/VerdictRadioSystemTests.cs` — 14 test declarations; bytes=5,633; SHA-256=`76b50d8a93d1d6b0598e21ffe84369f3fe1dc3cd61530b17aa0fe51c9f77338b`
- 00046: [Fact]
- 00047: public void Poll_GatesOnCulpableWindow_NothingBefore()
- 00055: [Fact]
- 00056: public void Poll_FiresCorpusOnceInsideWindow()
- 00068: [Fact]
- 00069: public void Poll_FiresAllAtDeadline()
- 00078: [Fact]
- 00079: public void FiredBroadcastsPublishToBus()
- 00089: [Fact]
- 00090: public void SaveLoad_RoundTripsFiredIds_NoReplay()
- 00109: [Fact]
- 00110: public void LoadFrom_LoadsThirtyAuthoredBroadcasts()
- 00120: [Fact]
- 00121: public void EvidenceEnrollment_FieldsPresentInItems()
### `Ashfall.Core.Tests/VerdictContentWebTests.cs` — 12 test declarations; bytes=5,757; SHA-256=`43a025fb0d48c169b04c95a2ec7fcf6fbe57d26de306fce3ce4108895e533eaf`
- 00025: [Fact]
- 00026: public void LoadItems_ReturnsAllFifteenRows()
- 00036: [Fact]
- 00037: public void LoadItems_IdsAreUniqueAndSnakeCase()
- 00055: [Fact]
- 00056: public void LoadItems_EvidenceAndQuestItemsPresent()
- 00076: [Fact]
- 00077: public void LoadItems_RowsAlignToRuntimeSchema()
- 00092: [Fact]
- 00093: public void LoadLocations_ReturnsFifteenSites()
- 00113: [Fact]
- 00114: public void LoadRadio_LoadsThirtyAuthoredBroadcasts()
### `Ashfall.Core.Tests/Radio/Plan94_102RadioFoundryIntegrationTests.cs` — 8 test declarations; bytes=8,186; SHA-256=`05c0e99eaf70a8e7b1daca980f33271ec4963763632ccf3c1e25c9474086f962`
- 00021: [Fact]
- 00022: public void Plan94_VerdictRadioCatalog_LoadsAll30Broadcasts_WithValidAttributes()
- 00067: [Fact]
- 00068: public void Plan102_FoundryAccords_LoadsAll18Treaties_WithProperSignatoryAndQuotas()
- 00115: [Fact]
- 00116: public void CrossSystem_RadioTelemetryAndIndustrialTreaties_ExhibitCoherentWastelandTimeline()
- 00155: [Fact]
- 00156: public void CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Verdict Radio: Thirty Machine-Register Broadcasts, Day-Triggered Polling, and Verdict Save Ownership` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan’s “3 broadcasts wired” statement requires current caller proof.
- A valid catalog row is not proof of a fired broadcast or player-visible cue.

## H.2 Evidence-strength corrections
- Trace actual Poll callers.
- Separate day trigger from unverified quest conditioning.
- Keep machine voice and cue fallback truthful.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Verdict Radio: Thirty Machine-Register Broadcasts, Day-Triggered Polling, and Verdict Save Ownership while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use VerdictCatalogLoader for static broadcasts.
- Use VerdictRadioSystem for poll/fired-id state.
- Use VerdictHostSession/VerdictSaveStore for host and persistence.
- Use existing audio/UI/Verdict consumers for presentation.
- Route any evidence/ending consequence through the canonical Verdict owner.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement verdict radio: thirty machine-register broadcasts, day-triggered polling, and verdict save ownership arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- No second radio owner or new save section is proposed.
- The final handoff must name exact Poll and fired-id contracts.

## J.2 Questions deliberately left open
- Should broadcasts be conditioned by current Verdict phase as well as day?
- Which audio adapter owns cue fallback and accessibility captions?

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

The original file at `HEAD:piagentsplans/94-verdict-radio-expansion.md` contained 5,103 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 94 — Verdict Radio Broadcasts Expansion (13 → 30 machine-register radio broadcasts)

## Goal (2 lines)
Expand `verdict_radio.json` from 13 verified broadcasts to 30. The Verdict
radio system (`VerdictRadioSystem.cs` confirmed live) defines machine-register
broadcasts the player intercepts — telemetry, maintenance logs, census
carrier signals. These are the Verdict expansion's radio layer, separate from
the faction radio corpus (Plan 73). 13 broadcasts is too few for a 300+ day
investigation campaign.

## Why (P2)
- Verified: `verdict_radio.json` has 13 entries (id, frequency, dayTrigger,
  source, message, signalStrength, kind). `VerdictRadioSystem.cs` and
  `VerdictSave.cs` are confirmed live.
- Creates the machine-voice pillar: Verdict radio is the voice of the
  pre-war machine infrastructure — telemetry bursts, maintenance schedules,
  census carrier signals, calibration readings. These broadcasts are how the
  player learns what the machines are doing without anyone telling them. 13
  broadcasts covers ~13 days; 30 covers a sustained investigation arc.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/verdict_radio.json` (expand 13 → 30 broadcasts)
- Read-only: `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs` (confirm
  schema and how dayTrigger gates broadcast availability)

## Content grammar (per broadcast)
- snake_case `id` with prefix `radio_verdict_` (confirmed prefix).
- frequency: string ("99.0 MHz" — the machine register's carrier frequency).
- dayTrigger: integer day when the broadcast becomes interceptable.
- source: 1 sentence describing the broadcast's origin ("Census Carrier,
  Machine Registers", "Fuse World, Service Bay").
- message: 1–3 sentences of broadcast text. Match the existing quality —
  terse, machine-like, slightly uncanny. These are automated systems reading
  data, not humans talking. The uncanny part is what the data implies.
- signalStrength: "S1" to "S5" (signal strength — weaker signals are harder
  to intercept).
- kind: broadcast type (telemetry, maintenance, census, calibration,
  anomaly, test, emergency — confirm accepted kinds by reading existing
  entries).
- Day distribution: broadcasts should span the campaign (day 200–365+),
  with increasing frequency as the investigation deepens.

## Steps
1. Read `VerdictRadioSystem.cs` to confirm the schema and how dayTrigger
   gates broadcast availability (does the player intercept broadcasts on or
   after dayTrigger?).
2. Read the existing 13 broadcasts to confirm the quality bar and accepted
   `kind` values (telemetry, maintenance — are there others?).
3. Author 17 new broadcasts spanning days 210–365+:
   - Telemetry (5): meter readings at different times, each reading slightly
     different — the machine is measuring something that changes.
   - Maintenance (4): scheduled service orders, each noting "nothing was
     wrong" — the machine maintains itself on a schedule nobody set.
   - Census (3): census carrier signals, counting something — the count
     changes between broadcasts, but nobody knows what's being counted.
   - Calibration (2): dosimeter calibration readings, noting drift — the
     machine is honest about its own error.
   - Anomaly (2): unexpected readings that don't match any schedule — the
     machine detected something it wasn't looking for.
   - Emergency (1): a single emergency broadcast on day 350+ — the machine
     breaks its own schedule for the first and only time.
4. Each broadcast: distinct id, dayTrigger, source, message, signalStrength,
   kind. Match the existing terse, machine-like tone.
5. Cross-reference: every broadcast id unique; dayTrigger strictly increasing
   (or at least non-decreasing); every kind is an accepted value.
6. Wire 3 broadcasts into Plan 82 Verdict investigation sites (broadcasts
   reference site locations or provide clues that lead to sites).
7. Wire 2 broadcasts into Plan 84 muster witnesses (a broadcast corroborates
   or contradicts a witness's testimony).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: Verdict radio catalog loads 30 broadcasts, all ids unique,
   dayTrigger within valid range, all kinds accepted, all messages non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `kind` values (step 2): confirm accepted
broadcast kinds before authoring.

## Definition of Done
- `verdict_radio.json` has 30 broadcasts, all ids unique, 3 wired to Verdict
  sites, 2 wired to muster witnesses, integrity + tests green.

## Follow-on
- Plan 82 (Verdict locations) — broadcasts reference investigation sites.
- Plan 84 (muster witnesses) — broadcasts corroborate or contradict
  testimony.
- Plan 73 (faction radio) — Verdict radio and faction radio are separate
  systems that complement each other.
- Plan 93 (Verdict NPCs) — NPCs reference radio broadcasts.
- Existing 24 (radio signals) — this plan provides the Verdict radio data.

```

## End of Plan 94 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` — 225 lines; 10,025 bytes; SHA-256 `abee1d19deb019bb058c9435069600549265c68836d2723c607fccd88c23d334`
Declaration index:
- 00015: public static class VerdictCatalogLoader
- 00024: public class VerdictLocationEntry
- 00034: public static List<VerdictLocationEntry> LoadLocations(
- 00070: public class VerdictItemEffects
- 00076: public class VerdictItemEntry
- 00091: public static List<VerdictItemEntry> LoadItems(
- 00120: public class VerdictRadioEntry
- 00132: public static List<VerdictRadioEntry> LoadRadio(
- 00159: public class VerdictWorldHistoryLadderEntry
- 00168: private class VerdictDataContainer
- 00175: public static List<string> LoadCorruptionCorpus(
- 00198: public static List<VerdictWorldHistoryLadderEntry> LoadWorldHistoryLadder(
- 00220: private class VerdictRadioContainer
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: using Ashfall.Core.IO;
00006: namespace Ashfall.Core.Verdict
00007: {
00008:     /// <summary>
00009:     /// ASHFALL: THE VERDICT (Expansion 08) — catalog loader for the three
00010:     /// Verdict data files (verdict_data.json, verdict_locations.json,
00011:     /// verdict_radio.json). Authoring authority is the design bible; the loader
00012:     /// mirrors the WitnessCatalogLoader pattern (missing file => empty list,
00013:     /// malformed => empty, engine-agnostic via IFileIO/IJsonSerializer).
00014:     /// </summary>
00015:     public static class VerdictCatalogLoader
00016:     {
00017:         public const string DataFile = "verdict_data.json";
00018:         public const string LocationsFile = "verdict_locations.json";
00019:         public const string ItemsFile = "verdict_items.json";
00020:         public const string RadioFile = "verdict_radio.json";
00021:
00022:         // ── Locations ───────────────────────────────────────────────────────────
00023:
00024:         public class VerdictLocationEntry
00025:         {
00026:             public string id = string.Empty;
00027:             public string displayName = string.Empty;
00028:             public string description = string.Empty;
00029:             public int dangerLevel = 5;
00030:             public float travelHours = 5f;
00031:             public float baseRadsPerHour = 30f;
00032:         }
00033:
00034:         public static List<VerdictLocationEntry> LoadLocations(
00035:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00036:         {
00037:             var result = new List<VerdictLocationEntry>();
00038:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
00039:             string path = fileIO.Combine(dataDir, LocationsFile);
00040:             if (!fileIO.FileExists(path)) return result;
00041:             string raw = fileIO.ReadAllText(path);
00042:             if (string.IsNullOrWhiteSpace(raw)) return result;
00043:             try
00044:             {
00045:                 var list = CatalogLocator.LoadWrappedList<VerdictLocationEntry>(raw, SystemTextJsonSerializer.Options);
00046:                 for (int i = 0; i < list.Count; i++)
00047:                 {
00048:                     var e = list[i];
00049:                     if (e == null || string.IsNullOrEmpty(e.id)) continue;
00050:                     result.Add(e);
00051:                 }
00052:             }
00053:             catch (Exception ex_CATDIAG)
00054:             {
00055:                 CatalogDiagnostics.Warn(path, "VerdictLocationEntry list", ex_CATDIAG);
00056:                 return result;
00057:             }
00058:             return result;
00059:         }
00060:
00061:         // ── Items ───────────────────────────────────────────────────────────────
00062:
00063:         /// <summary>One Verdict story/evidence item row. Runtime-schema compatible
00064:         /// (id/displayName/weightKg/tradeValue/category/description) with optional
00065:         /// enrichments the bible authors (tier, mechanical_effects, etc.). Loaded
00066:         /// only so the story content is reachable; never treated as loot.</summary>
00067:         /// <summary>Optional effect payload of an evidence/story item (recorded for
00068:         /// reachability; the game enrolls evidence through the EvidenceLedger, not
00069:         /// this mirror). Mirrors the authored JSON shape.</summary>
00070:         public class VerdictItemEffects
00071:         {
00072:             public int enrolled_evidence;
00073:             public string note = string.Empty;
00074:         }
00075:
00076:         public class VerdictItemEntry
00077:         {
00078:             public string id = string.Empty;
00079:             public string displayName = string.Empty;
00080:             public float weightKg;
00081:             public float tradeValue;
00082:             public string category = "story_item";
00083:             public string tier = string.Empty;
00084:             public string description = string.Empty;
00085:             public VerdictItemEffects mechanical_effects = null!;
00086:             public string downstream_quest_trigger = string.Empty;
00087:             public string faction_affinity = string.Empty;
00088:             public string rarity = string.Empty;
00089:         }
00090:
00091:         public static List<VerdictItemEntry> LoadItems(
00092:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00093:         {
00094:             var result = new List<VerdictItemEntry>();
00095:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
00096:             string path = fileIO.Combine(dataDir, ItemsFile);
00097:             if (!fileIO.FileExists(path)) return result;
00098:             string raw = fileIO.ReadAllText(path);
00099:             if (string.IsNullOrWhiteSpace(raw)) return result;
00100:             try
00101:             {
00102:                 var entries = CatalogLocator.LoadWrappedList<VerdictItemEntry>(raw, SystemTextJsonSerializer.Options);
00103:                 for (int i = 0; i < entries.Count; i++)
00104:                 {
00105:                     var e = entries[i];
00106:                     if (e == null || string.IsNullOrEmpty(e.id)) continue;
00107:                     result.Add(e);
00108:                 }
00109:             }
00110:             catch (Exception ex_CATDIAG)
00111:             {
00112:                 CatalogDiagnostics.Warn(path, "VerdictItemEntry list", ex_CATDIAG);
00113:                 return result;
00114:             }
00115:             return result;
00116:         }
00117:
00118:         // ── Radio ───────────────────────────────────────────────────────────────
00119:
00120:         public class VerdictRadioEntry
00121:         {
00122:             public string id = string.Empty;
00123:             public string frequency = string.Empty;
00124:             public int dayTrigger = 180;
00125:             public string source = string.Empty;
00126:             public string message = string.Empty;
00127:             public string signalStrength = string.Empty;
00128:             public string kind = "telemetry";
00129:             public string audio_cue = string.Empty;
00130:         }
00131:
00132:         public static List<VerdictRadioEntry> LoadRadio(
00133:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00134:         {
00135:             var result = new List<VerdictRadioEntry>();
00136:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
00137:             string path = fileIO.Combine(dataDir, RadioFile);
00138:             if (!fileIO.FileExists(path)) return result;
00139:             string raw = fileIO.ReadAllText(path);
00140:             if (string.IsNullOrWhiteSpace(raw)) return result;
00141:             try
00142:             {
00143:                 var parsed = json.Deserialize<VerdictRadioContainer>(raw);
00144:                 if (parsed?.broadcasts == null) return result;
00145:                 foreach (var e in parsed.broadcasts)
00146:                 {
00147:                     if (e == null || string.IsNullOrEmpty(e.id)) continue;
00148:                     result.Add(e);
00149:                 }
00150:             }
00151:             catch (Exception ex_CATDIAG)
00152:             {
00153:                 CatalogDiagnostics.Warn(path, "VerdictRadioContainer", ex_CATDIAG);
00154:                 return result;
00155:             }
00156:             return result;
00157:         }
00158:
00159:         public class VerdictWorldHistoryLadderEntry
00160:         {
00161:             public int layer { get; set; }
00162:             public string knowledge_key { get; set; } = string.Empty;
00163:             public string title { get; set; } = string.Empty;
00164:             public string discovery_location_id { get; set; } = string.Empty;
00165:             public string body_summary { get; set; } = string.Empty;
00166:         }
00167:
00168:         private class VerdictDataContainer
00169:         {
00170:             public List<string> corruption_corpus = new List<string>();
00171:             public List<VerdictWorldHistoryLadderEntry> world_history_ladder = new List<VerdictWorldHistoryLadderEntry>();
00172:         }
00173:
00174:         /// <summary>Load the corruption corpus from verdict_data.json (empty if missing).</summary>
00175:         public static List<string> LoadCorruptionCorpus(
00176:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00177:         {
00178:             var result = new List<string>();
00179:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
00180:             string path = fileIO.Combine(dataDir, DataFile);
00181:             if (!fileIO.FileExists(path)) return result;
00182:             string raw = fileIO.ReadAllText(path);
00183:             if (string.IsNullOrWhiteSpace(raw)) return result;
00184:             try
00185:             {
00186:                 var parsed = json.Deserialize<VerdictDataContainer>(raw);
00187:                 if (parsed?.corruption_corpus != null)
00188:                     result.AddRange(parsed.corruption_corpus);
00189:             }
00190:             catch (Exception ex_CATDIAG)
00191:             {
00192:                 CatalogDiagnostics.Warn(path, "VerdictDataContainer", ex_CATDIAG);
00193:             }
00194:             return result;
00195:         }
00196:
00197:         /// <summary>Load the world history ladder from verdict_data.json (empty if missing).</summary>
00198:         public static List<VerdictWorldHistoryLadderEntry> LoadWorldHistoryLadder(
00199:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00200:         {
00201:             var result = new List<VerdictWorldHistoryLadderEntry>();
00202:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;
00203:             string path = fileIO.Combine(dataDir, DataFile);
00204:             if (!fileIO.FileExists(path)) return result;
00205:             string raw = fileIO.ReadAllText(path);
00206:             if (string.IsNullOrWhiteSpace(raw)) return result;
00207:             try
00208:             {
00209:                 var parsed = json.Deserialize<VerdictDataContainer>(raw);
00210:                 if (parsed?.world_history_ladder != null)
00211:                     result.AddRange(parsed.world_history_ladder);
00212:             }
00213:             catch (Exception ex_CATDIAG)
00214:             {
00215:                 CatalogDiagnostics.Warn(path, "VerdictDataContainer.world_history_ladder", ex_CATDIAG);
00216:             }
00217:             return result;
00218:         }
00219:
00220:         private class VerdictRadioContainer
00221:         {
00222:             public List<VerdictRadioEntry> broadcasts = new List<VerdictRadioEntry>();
00223:         }
00224:     }
00225: }
```

## `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs` — 114 lines; 4,583 bytes; SHA-256 `fb560fa3710ed2ac870f7e1cf16f5dd087e316eb26ffc60d4cd6bc4be960f5db`
Declaration index:
- 00018: public sealed class VerdictRadioSystem
- 00044: public bool HasFired(string id) => _firedIds.Contains(id);
- 00052: public System.Collections.Generic.List<string> Poll(int day, ReckoningPhase phase)
- 00072: public int LoadFrom(string dataDir, IFileIO fileIO, IJsonSerializer json)
- 00090: public class VerdictRadioState
- 00096: public VerdictRadioState CaptureState()
- 00105: public void RestoreState(VerdictRadioState state)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Events;
00005: using Ashfall.Core.Clock;
00006:
00007: namespace Ashfall.Core.Verdict
00008: {
00009:     /// <summary>
00010:     /// ASHFALL: THE VERDICT (Expansion 08) — the diegetic radio corpus engine.
00011:     /// Loads verdict_radio.json (the 12-signal §6.2 corpus + the Reckoning Call)
00012:     /// and schedules each broadcast through the 99.0 MHz census/carrier contract.
00013:     /// A broadcast fires once when (a) its dayTrigger has passed AND (b) the
00014:     /// Reckoning has reached at least CULPABLE (the census carrier window). Each
00015:     /// fire publishes a `radio.verdict.broadcast` event on the shared bus.
00016:     /// Engine-agnostic, deterministic, save/load safe — never per-frame.
00017:     /// </summary>
00018:     public sealed class VerdictRadioSystem
00019:     {
00020:         public const string SystemId = "verdict_radio_system";
00021:
00022:         /// <summary>First day the census carrier is audible (CANON §5.1: Day 210±3).</summary>
00023:         public const int CarrierOpenDay = 210;
00024:
00025:         private readonly List<VerdictCatalogLoader.VerdictRadioEntry> _corpus = new List<VerdictCatalogLoader.VerdictRadioEntry>();
00026:         private readonly HashSet<string> _firedIds = new HashSet<string>(StringComparer.Ordinal);
00027:         private readonly IEventBus? _bus;
00028:         private readonly ISimClock? _clock;
00029:
00030:         public VerdictRadioSystem(
00031: IEventBus? bus = null,
00032: ISimClock? clock = null,
00033:             IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry>? corpus = null)
00034:         {
00035:             _bus = bus;
00036:             _clock = clock;
00037:             if (corpus != null)
00038:                 foreach (var e in corpus)
00039:                     if (e != null && !string.IsNullOrEmpty(e.id))
00040:                         _corpus.Add(e);
00041:         }
00042:
00043:         public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> Corpus => _corpus;
00044:         public bool HasFired(string id) => _firedIds.Contains(id);
00045:         public int FiredCount => _firedIds.Count;
00046:
00047:         /// <summary>
00048:         /// Evaluate the corpus against the current day + phase. A broadcast fires
00049:         /// at most once: dayTrigger passed AND phase >= Culpable. Returns the ids
00050:         /// fired this call (for tests/observability). Publish is via the shared bus.
00051:         /// </summary>
00052:         public System.Collections.Generic.List<string> Poll(int day, ReckoningPhase phase)
00053:         {
00054:             var fired = new System.Collections.Generic.List<string>();
00055:             if (phase < ReckoningPhase.Culpable) return fired;
00056:             if (day < CarrierOpenDay) return fired;
00057:
00058:             for (int i = 0; i < _corpus.Count; i++)
00059:             {
00060:                 var e = _corpus[i];
00061:                 if (e == null || _firedIds.Contains(e.id)) continue;
00062:                 if (day < e.dayTrigger) continue;
00063:                 _firedIds.Add(e.id);
00064:                 if (_bus != null)
00065:                     _bus.Publish("radio.verdict.broadcast", e);
00066:                 fired.Add(e.id);
00067:             }
00068:             return fired;
00069:         }
00070:
00071:         /// <summary>Load the corpus from disk into this system (host convenience).</summary>
00072:         public int LoadFrom(string dataDir, IFileIO fileIO, IJsonSerializer json)
00073:         {
00074:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return 0;
00075:             var loaded = VerdictCatalogLoader.LoadRadio(dataDir, fileIO, json);
00076:             int added = 0;
00077:             foreach (var e in loaded)
00078:             {
00079:                 if (e == null || string.IsNullOrEmpty(e.id)) continue;
00080:                 if (_corpus.Exists(x => x.id == e.id)) continue;
00081:                 _corpus.Add(e);
00082:                 added++;
00083:             }
00084:             return added;
00085:         }
00086:
00087:         // ── Save / Load (fired-ids state so reloads don't replay) ────────────
00088:
00089:         [Serializable]
00090:         public class VerdictRadioState
00091:         {
00092:             public string systemId = SystemId;
00093:             public List<string> firedIds = new List<string>();
00094:         }
00095:
00096:         public VerdictRadioState CaptureState()
00097:         {
00098:             var c = new VerdictRadioState { systemId = SystemId };
00099:             var sorted = new List<string>(_firedIds);
00100:             sorted.Sort(StringComparer.Ordinal);
00101:             c.firedIds = sorted;
00102:             return c;
00103:         }
00104:
00105:         public void RestoreState(VerdictRadioState state)
00106:         {
00107:             if (state == null) return;
00108:             _firedIds.Clear();
00109:             if (state.firedIds != null)
00110:                 foreach (var id in state.firedIds)
00111:                     if (!string.IsNullOrEmpty(id)) _firedIds.Add(id);
00112:         }
00113:     }
00114: }
```

## `Assets/Ashfall.Core/Verdict/VerdictSave.cs` — 272 lines; 12,122 bytes; SHA-256 `35bd29e9933555d17a90114245b9395ee98ec22077e7d46e2467dd185f603806`
Declaration index:
- 00026: public class VerdictSave
- 00052: public class VerdictSaveV3
- 00072: public class VerdictSaveV1
- 00089: public class VerdictSaveV2
- 00102: public static class VerdictSaveCodec
- 00104: public static VerdictSave Capture(
- 00131: public static string Encode(VerdictSave save, IJsonSerializer json)
- 00144: public static bool TryDecode(string json, IJsonSerializer serializer, out VerdictSave save)
- 00174: private static bool MigrateV1(string json, IJsonSerializer serializer, out VerdictSave save)
- 00197: private static bool MigrateV2(string json, IJsonSerializer serializer, out VerdictSave save)
- 00222: private static bool MigrateV3(string json, IJsonSerializer serializer, out VerdictSave save)
- 00248: public static void Restore(
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core.YearOfAsh;
00004:
00005: using Ashfall.Core.IO;
00006: namespace Ashfall.Core.Verdict
00007: {
00008:     /// <summary>
00009:     /// ASHFALL: THE VERDICT (Expansion 08) — cross-host save envelope.
00010:     /// Mirrors DoseLedgerSave / HoldfastSave: checksum recomputed on encode,
00011:     /// hard-reject on decode for tamper / checksumless / newer version.
00012:     ///
00013:     /// v3 adds the Verdict questline section. Verdict quest progress is owned
00014:     /// here — the Year of Ash envelope is no longer a second owner (its
00015:     /// registration was removed). v1/v2 saves migrate with an empty quest
00016:     /// section; a one-time adoption helper folds any quest_verdict_* progress
00017:     /// that a pre-v3 save carried inside the Year of Ash envelope into the
00018:     /// Verdict envelope (see <see cref="VerdictQuestMigration"/>).
00019:     ///
00020:     /// Migration validates the checksum over each version's FROZEN shape (see
00021:     /// <see cref="VerdictSaveV1"/> / <see cref="VerdictSaveV2"/>) because
00022:     /// <see cref="SaveChecksum"/> walks public fields — validating a legacy
00023:     /// payload against the current shape would always mismatch.
00024:     /// </summary>
00025:     [Serializable]
00026:     public class VerdictSave
00027:     {
00028:         public const int CurrentSaveVersion = 4;
00029:         public const int MigrationFromVersion = 1;
00030:
00031:         public int saveVersion = CurrentSaveVersion;
00032:         public int simDay;
00033:         public MachineLogSystemState machineLog = new MachineLogSystemState();
00034:         public ReckoningState reckoning = new ReckoningState();
00035:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
00036:         public VerdictNpcState npcs = new VerdictNpcState();
00037:         public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
00038:         public QuestlineSystemState quests = new QuestlineSystemState();
00039:         public int censusLastWindowDay = -1;
00040:
00041:         // v4 section.
00042:         public VerdictAccusationState accusations = new VerdictAccusationState();
00043:
00044:         public string Checksum = string.Empty;
00045:     }
00046:
00047:     /// <summary>
00048:     /// Frozen v3 envelope shape (npcs + radio + quests, no accusations section).
00049:     /// Do not add fields here — it must match what v3 wrote byte-for-byte in field set.
00050:     /// </summary>
00051:     [Serializable]
00052:     public class VerdictSaveV3
00053:     {
00054:         public int saveVersion = 3;
00055:         public int simDay;
00056:         public MachineLogSystemState machineLog = new MachineLogSystemState();
00057:         public ReckoningState reckoning = new ReckoningState();
00058:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
00059:         public VerdictNpcState npcs = new VerdictNpcState();
00060:         public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
00061:         public QuestlineSystemState quests = new QuestlineSystemState();
00062:         public int censusLastWindowDay = -1;
00063:         public string Checksum = string.Empty;
00064:     }
00065:
00066:     /// <summary>
00067:     /// Frozen v1 envelope shape (no npcs, no radio, no quests). Kept so a v1
00068:     /// file on disk validates against the field set it was actually hashed with.
00069:     /// Do not add fields here.
00070:     /// </summary>
00071:     [Serializable]
00072:     public class VerdictSaveV1
00073:     {
00074:         public int saveVersion = 1;
00075:         public int simDay;
00076:         public MachineLogSystemState machineLog = new MachineLogSystemState();
00077:         public ReckoningState reckoning = new ReckoningState();
00078:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
00079:         public int censusLastWindowDay = -1;
00080:         public string Checksum = string.Empty;
00081:     }
00082:
00083:     /// <summary>
00084:     /// Frozen v2 envelope shape (npcs + radio, no quests). Kept so a v2 file on
00085:     /// disk validates against the field set it was actually hashed with.
00086:     /// Do not add fields here.
00087:     /// </summary>
00088:     [Serializable]
00089:     public class VerdictSaveV2
00090:     {
00091:         public int saveVersion = 2;
00092:         public int simDay;
00093:         public MachineLogSystemState machineLog = new MachineLogSystemState();
00094:         public ReckoningState reckoning = new ReckoningState();
00095:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
00096:         public VerdictNpcState npcs = new VerdictNpcState();
00097:         public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
00098:         public int censusLastWindowDay = -1;
00099:         public string Checksum = string.Empty;
00100:     }
00101:
00102:     public static class VerdictSaveCodec
00103:     {
00104:         public static VerdictSave Capture(
00105:             int simDay,
00106:             MachineLogSystem machineLog,
00107:             ReckoningSystem reckoning,
00108:             EvidenceLedger evidence,
00109:             int censusLastWindowDay,
00110: VerdictNpcSystem? npcs = null,
00111: VerdictRadioSystem? radio = null,
00112: QuestlineSystem? quests = null,
00113: VerdictAccusationSystem? accusations = null)
00114:         {
00115:             var save = new VerdictSave
00116:             {
00117:                 simDay = simDay,
00118:                 machineLog = machineLog.CaptureState(),
00119:                 reckoning = reckoning.CaptureState(),
00120:                 evidence = evidence.CaptureState(),
00121:                 npcs = npcs != null ? npcs.CaptureState() : new VerdictNpcState(),
00122:                 radio = radio != null ? radio.CaptureState() : new VerdictRadioSystem.VerdictRadioState(),
00123:                 quests = quests != null ? quests.CaptureState() : new QuestlineSystemState(),
00124:                 accusations = accusations != null ? accusations.CaptureState() : new VerdictAccusationState(),
00125:                 censusLastWindowDay = censusLastWindowDay
00126:             };
00127:             save.Checksum = SaveChecksum.Compute(save);
00128:             return save;
00129:         }
00130:
00131:         public static string Encode(VerdictSave save, IJsonSerializer json)
00132:         {
00133:             if (save == null) throw new ArgumentNullException(nameof(save));
00134:             save.Checksum = SaveChecksum.Compute(save);
00135:             return json.Serialize(save);
00136:         }
00137:
00138:         /// <summary>
00139:         /// Decodes and migrates a Verdict save. Legacy versions are parsed as
00140:         /// their FROZEN shapes so the checksum is verified over exactly the
00141:         /// fields that version wrote. Rejects: newer versions, too-old versions,
00142:         /// checksumless payloads, and tampered payloads.
00143:         /// </summary>
00144:         public static bool TryDecode(string json, IJsonSerializer serializer, out VerdictSave save)
00145:         {
00146:             save = null!;
00147:             if (string.IsNullOrEmpty(json) || serializer == null) return false;
00148:             try
00149:             {
00150:                 var decoded = serializer.Deserialize<VerdictSave>(json);
00151:                 if (decoded == null) return false;
00152:                 if (decoded.saveVersion > VerdictSave.CurrentSaveVersion) return false; // newer — reject
00153:                 if (decoded.saveVersion < VerdictSave.MigrationFromVersion) return false; // too old — reject
00154:
00155:                 if (decoded.saveVersion == 1) return MigrateV1(json, serializer, out save);
00156:                 if (decoded.saveVersion == 2) return MigrateV2(json, serializer, out save);
00157:                 if (decoded.saveVersion == 3) return MigrateV3(json, serializer, out save);
00158:
00159:                 // Current version: validate over the current shape.
00160:                 if (string.IsNullOrEmpty(decoded.Checksum)) return false;   // tamper/legacy
00161:                 string recomputed = SaveChecksum.Compute(decoded);
00162:                 if (!string.Equals(recomputed, decoded.Checksum, StringComparison.Ordinal))
00163:                     return false; // tampered
00164:                 save = decoded;
00165:                 return true;
00166:             }
00167:             catch (Exception ex_CATDIAG)
00168:             {
00169:                 CatalogDiagnostics.Warn("<decode>", "VerdictSave", ex_CATDIAG);
00170:                 return false;
00171:             }
00172:         }
00173:
00174:         private static bool MigrateV1(string json, IJsonSerializer serializer, out VerdictSave save)
00175:         {
00176:             save = null!;
00177:             var v1 = serializer.Deserialize<VerdictSaveV1>(json);
00178:             if (v1 == null) return false;
00179:             if (string.IsNullOrEmpty(v1.Checksum)) return false;
00180:             if (!string.Equals(SaveChecksum.Compute(v1), v1.Checksum, StringComparison.Ordinal)) return false;
00181:
00182:             var migrated = new VerdictSave
00183:             {
00184:                 saveVersion = VerdictSave.CurrentSaveVersion,
00185:                 simDay = v1.simDay,
00186:                 machineLog = v1.machineLog,
00187:                 reckoning = v1.reckoning,
00188:                 evidence = v1.evidence,
00189:                 // npcs / radio / quests / accusations stay at their field initialisers (fresh defaults).
00190:                 censusLastWindowDay = v1.censusLastWindowDay
00191:             };
00192:             migrated.Checksum = SaveChecksum.Compute(migrated);
00193:             save = migrated;
00194:             return true;
00195:         }
00196:
00197:         private static bool MigrateV2(string json, IJsonSerializer serializer, out VerdictSave save)
00198:         {
00199:             save = null!;
00200:             var v2 = serializer.Deserialize<VerdictSaveV2>(json);
00201:             if (v2 == null) return false;
00202:             if (string.IsNullOrEmpty(v2.Checksum)) return false;
00203:             if (!string.Equals(SaveChecksum.Compute(v2), v2.Checksum, StringComparison.Ordinal)) return false;
00204:
00205:             var migrated = new VerdictSave
00206:             {
00207:                 saveVersion = VerdictSave.CurrentSaveVersion,
00208:                 simDay = v2.simDay,
00209:                 machineLog = v2.machineLog,
00210:                 reckoning = v2.reckoning,
00211:                 evidence = v2.evidence,
00212:                 npcs = v2.npcs ?? new VerdictNpcState(),
00213:                 radio = v2.radio ?? new VerdictRadioSystem.VerdictRadioState(),
00214:                 // quests / accusations stay at their field initialisers (fresh defaults).
00215:                 censusLastWindowDay = v2.censusLastWindowDay
00216:             };
00217:             migrated.Checksum = SaveChecksum.Compute(migrated);
00218:             save = migrated;
00219:             return true;
00220:         }
00221:
00222:         private static bool MigrateV3(string json, IJsonSerializer serializer, out VerdictSave save)
00223:         {
00224:             save = null!;
00225:             var v3 = serializer.Deserialize<VerdictSaveV3>(json);
00226:             if (v3 == null) return false;
00227:             if (string.IsNullOrEmpty(v3.Checksum)) return false;
00228:             if (!string.Equals(SaveChecksum.Compute(v3), v3.Checksum, StringComparison.Ordinal)) return false;
00229:
00230:             var migrated = new VerdictSave
00231:             {
00232:                 saveVersion = VerdictSave.CurrentSaveVersion,
00233:                 simDay = v3.simDay,
00234:                 machineLog = v3.machineLog,
00235:                 reckoning = v3.reckoning,
00236:                 evidence = v3.evidence,
00237:                 npcs = v3.npcs ?? new VerdictNpcState(),
00238:                 radio = v3.radio ?? new VerdictRadioSystem.VerdictRadioState(),
00239:                 quests = v3.quests ?? new QuestlineSystemState(),
00240:                 // accusations stays at its field initialiser (fresh empty state).
00241:                 censusLastWindowDay = v3.censusLastWindowDay
00242:             };
00243:             migrated.Checksum = SaveChecksum.Compute(migrated);
00244:             save = migrated;
00245:             return true;
00246:         }
00247:
00248:         public static void Restore(
00249:             VerdictSave save,
00250:             MachineLogSystem machineLog,
00251:             ReckoningSystem reckoning,
00252:             EvidenceLedger evidence,
00253: VerdictNpcSystem? npcs = null,
00254: VerdictRadioSystem? radio = null,
00255: QuestlineSystem? quests = null,
00256: VerdictAccusationSystem? accusations = null)
00257:         {
00258:             if (save == null) return;
00259:             machineLog.RestoreState(save.machineLog);
00260:             reckoning.RestoreState(save.reckoning);
00261:             evidence.RestoreState(save.evidence);
00262:             if (npcs != null)
00263:                 npcs.RestoreState(save.npcs ?? new VerdictNpcState());
00264:             if (radio != null)
00265:                 radio.RestoreState(save.radio ?? new VerdictRadioSystem.VerdictRadioState());
00266:             if (quests != null)
00267:                 quests.RestoreState(save.quests ?? new QuestlineSystemState());
00268:             if (accusations != null)
00269:                 accusations.RestoreState(save.accusations ?? new VerdictAccusationState());
00270:         }
00271:     }
00272: }
```

## `src/Host/VerdictHostSession.cs` — 284 lines; 14,368 bytes; SHA-256 `eb9abc73b3557cdfa1d7b975f426399da4cbf2e299387681232521df4b1f6f8b`
Declaration index:
- 00024: public sealed class VerdictHostSession
- 00047: public System.Collections.Generic.HashSet<string> MaterializedNpcFlags()
- 00066: public System.Collections.Generic.List<Ashfall.Core.Verdict.VerdictNpcEntry> AvailableNpcs(string locationId = null!)
- 00106: public static VerdictHostSession Create(
- 00161: public void AdvanceDay(int day, int livingCount, int logReadCount)
- 00169: public void TickCensus()
- 00177: public System.Collections.Generic.List<string> TickRadio(int day)
- 00196: private VerdictCatalogLoader.VerdictRadioEntry? FindRadioEntry(string id)
- 00209: public int EnrollEvidenceFromItems(int day)
- 00231: public void TickCorruption(int day)
- 00239: public VerdictSave CaptureSave()
- 00247: public void RestoreSave(VerdictSave save)
- 00254: public string StatusLine()
- 00262: public VerdictCatalogLoader.VerdictLocationEntry? FindLocation(string id)
- 00270: private int CurrentDaySafe()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Ashfall.Core;
00006: using Ashfall.Core.Clock;
00007: using Ashfall.Core.Events;
00008: using Ashfall.Core.Flags;
00009: using Ashfall.Core.Verdict;
00010: using Ashfall.Core.YearOfAsh;
00011: using AtomicWar.GodotApp.YearOfAsh;
00012: using AtomicWar.GodotApp.Audio;
00013: using ClockSimClock = Ashfall.Core.Clock.SimClock;
00014:
00015: namespace AtomicWar.GodotApp
00016: {
00017:     /// <summary>
00018:     /// ASHFALL: THE VERDICT (Expansion 08) — thin Godot host session.
00019:     /// Wraps MachineLogSystem + ReckoningSystem + EvidenceLedger + the 99.0 MHz
00020:     /// census broadcast, wires the sim clock / event bus / flag ledger / census
00021:     /// port, and persists to user:// via VerdictSaveStore. No gameplay rules
00022:     /// here — hosts only present.
00023:     /// </summary>
00024:     public sealed class VerdictHostSession
00025:     : HostSessionBase{
00026:         private static readonly FileSystemIO s_files = new FileSystemIO();
00027:         private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();
00028:
00029:         public MachineLogSystem MachineLog { get; }
00030:         public ReckoningSystem Reckoning { get; }
00031:         public EvidenceLedger Evidence { get; }
00032:         public VerdictEvidenceChain EvidenceChain { get; }
00033:         public VerdictNpcSystem Npcs { get; }
00034:         public VerdictCensusBroadcast Census { get; }
00035:         public VerdictRadioSystem Radio { get; internal set; }
00036:         public QuestlineSystem Quests { get; }
00037:         public IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> Locations { get; }
00038:         public IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> Items { get; }
00039:         public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> RadioEntries { get; }
00040:         public IReadOnlyList<string> CorruptionCorpus { get; private set; }
00041:         private readonly ISeededRng _machineRng;
00042:         private int _currentDay = -1;
00043:
00044:         /// <summary>Flag-gate materialization: the six Verdict figures unlock from
00045:         /// real progress (evidence, phase, call). Hosts present this; no NPC gate is
00046:         /// a debug backdoor — each flag traces to an actual machine milestone.</summary>
00047:         public System.Collections.Generic.HashSet<string> MaterializedNpcFlags()
00048:         {
00049:             var flags = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
00050:             if (MachineLog.ReadCount() >= 1) flags.Add("flag_verdict_fuse_world_read");
00051:             if (MachineLog.ReadCount() >= 1) flags.Add("flag_verdict_relay_read");
00052:             if (Evidence.IsEnrolled("evidence_eden_log")) flags.Add("flag_verdict_eden_log_recovered");
00053:             if (Evidence.IsEnrolled("evidence_fuse_linen")) flags.Add("flag_verdict_fuse_world_read");
00054:             if (Evidence.IsEnrolled("evidence_fuse_linen")) flags.Add("flag_verdict_shift_charter_restored");
00055:             if (Evidence.IsEnrolled("evidence_geophone_hymn")) flags.Add("flag_verdict_clerk_met");
00056:             if (Reckoning.State.callResolved) flags.Add("flag_verdict_call_resolved");
00057:             // Plan 93 — investigation-site residue NPCs. The cliff-bunker signal
00058:             // decodes from sustained study of the machine's own log (a second
00059:             // read entry), the same progress surface that materializes the
00060:             // original gates above.
00061:             if (MachineLog.ReadCount() >= 2) flags.Add("flag_verdict_cliff_signal_decoded");
00062:             return flags;
00063:         }
00064:
00065:         /// <summary>NPCs currently available given live progress (flag + phase + optional site).</summary>
00066:         public System.Collections.Generic.List<Ashfall.Core.Verdict.VerdictNpcEntry> AvailableNpcs(string locationId = null!)
00067:         {
00068:             int phase = (int)Reckoning.Phase;
00069:             return Npcs.GetAvailable(MaterializedNpcFlags(), phase, locationId);
00070:         }
00071:
00072:         public string LastEvent { get; private set; } = string.Empty;
00073:         public VerdictHostSession(
00074:             MachineLogSystem machineLog = null!,
00075:             ReckoningSystem reckoning = null!,
00076:             EvidenceLedger evidence = null!,
00077:             VerdictNpcSystem npcs = null!,
00078:             VerdictCensusBroadcast census = null!,
00079:             IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> locations = null!,
00080:             IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> items = null!,
00081:             IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> radio = null!,
00082:             QuestlineSystem quests = null!)
00083:         {
00084:             MachineLog = machineLog ?? new MachineLogSystem();
00085:             Reckoning = reckoning ?? new ReckoningSystem();
00086:             Evidence = evidence ?? new EvidenceLedger();
00087:             EvidenceChain = new VerdictEvidenceChain(MachineLog, Evidence, Reckoning);
00088:             Npcs = npcs ?? new VerdictNpcSystem();
00089:             Quests = quests ?? new QuestlineSystem();
00090:             Census = census;
00091:             Locations = locations ?? new List<VerdictCatalogLoader.VerdictLocationEntry>();
00092:             Items = items ?? new List<VerdictCatalogLoader.VerdictItemEntry>();
00093:             RadioEntries = radio ?? new List<VerdictCatalogLoader.VerdictRadioEntry>();
00094:             CorruptionCorpus = new List<string>();
00095:             _machineRng = new SeededRng(8841209 + 17);
00096:
00097:             MachineLog.OnLogPosted += e => { LastEvent = $"log:{e.facilityId}@{e.day}:{e.kind}"; RaiseStateChanged(); };
00098:             MachineLog.OnEntryRead += e => { LastEvent = $"read:{e.evidenceTag}"; RaiseStateChanged(); };
00099:             Reckoning.OnPhaseChanged += p => { LastEvent = $"phase:{p}"; RaiseStateChanged(); };
00100:             Reckoning.OnReckoningCall += n => { LastEvent = $"reckoning_call:{n}"; RaiseStateChanged(); };
00101:             Reckoning.OnVerdictResolved += key => { LastEvent = $"resolved:{key}"; RaiseStateChanged(); };
00102:             Evidence.OnEnrolled += id => { LastEvent = $"evidence:{id}"; RaiseStateChanged(); };
00103:             Npcs.OnSpoken += n => { LastEvent = $"npc:{n.id}"; RaiseStateChanged(); };
00104:         }
00105:
00106:         public static VerdictHostSession Create(
00107:             string dataDir,
00108:             ISimClock clock = null!,
00109:             IEventBus bus = null!,
00110:             IFlagLedger flags = null!,
00111:             ISeededRng radioRng = null!,
00112:             IWorldCensus census = null!)
00113:         {
00114:             clock = clock ?? new ClockSimClock();
00115:             bus = bus ?? new SimpleEventBus();
00116:             flags = flags ?? new Ashfall.Core.Flags.CampaignConsequenceLedger();
00117:             radioRng = radioRng ?? new SeededRng(8841209);
00118:
00119:             var locations = VerdictCatalogLoader.LoadLocations(dataDir, s_files, s_json);
00120:             var items = VerdictCatalogLoader.LoadItems(dataDir, s_files, s_json);
00121:             var radioEntries = VerdictCatalogLoader.LoadRadio(dataDir, s_files, s_json);
00122:             var censusBroadcast = new VerdictCensusBroadcast(clock, bus, flags, radioRng, census);
00123:             var quests = new QuestlineSystem();
00124:             VerdictQuestCatalogLoader.LoadAndRegister(quests, dataDir, s_files, s_json);
00125:             var session = new VerdictHostSession(census: censusBroadcast, locations: locations, items: items, radio: radioEntries, quests: quests);
00126:             session.Radio = new VerdictRadioSystem(bus, clock, radioEntries);
00127:             VerdictNpcCatalogLoader.LoadAndRegister(session.Npcs, dataDir, s_files, s_json);
00128:             session.CorruptionCorpus = VerdictCatalogLoader.LoadCorruptionCorpus(dataDir, s_files, s_json);
00129:
00130:             var save = VerdictSaveStore.TryLoad();
00131:             if (save != null)
00132:             {
00133:                 VerdictSaveCodec.Restore(save, session.MachineLog, session.Reckoning, session.Evidence, session.Npcs, session.Radio, session.Quests);
00134:                 // Observability: remember which save version loaded and whether it migrated (C).
00135:                 session.LoadedSaveVersion = save.saveVersion;
00136:                 session.WasSaveMigrated = save.saveVersion != VerdictSave.CurrentSaveVersion;
00137:                 session.LastEvent = "Verdict state restored from save.";
00138:             }
00139:
00140:             // One-time migration: pre-v3 Verdict saves persisted Verdict quest
00141:             // progress inside the Year of Ash envelope. Fold any quest_verdict_*
00142:             // records found there into the Verdict envelope (Verdict wins on
00143:             // conflict) so no progress is lost when the old save upgrades. This
00144:             // runs whether or not a Verdict save file exists yet.
00145:             var yearOfAshSave = YearOfAshSaveStore.TryLoad();
00146:             if (yearOfAshSave != null && yearOfAshSave.quests != null)
00147:             {
00148:                 int adopted = VerdictQuestMigration.AdoptFromYearOfAsh(session.Quests.State, yearOfAshSave.quests);
00149:                 if (adopted > 0)
00150:                     session.LastEvent = "Migrated " + adopted + " Verdict quest record(s) from the Year of Ash save.";
00151:             }
00152:             return session;
00153:         }
00154:
00155:         /// <summary>Save version loaded at startup (observability; 0 = none).</summary>
00156:         public int LoadedSaveVersion { get; private set; }
00157:         /// <summary>True when the loaded save was migrated to the current version (v1→v2).</summary>
00158:         public bool WasSaveMigrated { get; private set; }
00159:
00160:         /// <summary>Coarse game-time step — call once per sim-day, not per-frame.</summary>
00161:         public void AdvanceDay(int day, int livingCount, int logReadCount)
00162:         {
00163:             _currentDay = day;
00164:             var fired = Reckoning.Poll(day, livingCount, logReadCount, Evidence.Count);
00165:             if (fired.Count > 0) LastEvent = string.Join(";", fired);
00166:         }
00167:
00168:         /// <summary>Feed the census broadcast with the current clock (window check).</summary>
00169:         public void TickCensus()
00170:         {
00171:             Census.BroadcastIfDue();
00172:         }
00173:
00174:         /// <summary>Evaluate the diegetic radio corpus against the current day and phase.
00175:         /// Broadcasts fire once each, gated on the Culpable+ census carrier window.
00176:         /// Returns the ids fired this call (observability).</summary>
00177:         public System.Collections.Generic.List<string> TickRadio(int day)
00178:         {
00179:             if (Radio == null) return new System.Collections.Generic.List<string>();
00180:             var fired = Radio.Poll(day, Reckoning.Phase);
00181:             for (int i = 0; i < fired.Count; i++)
00182:             {
00183:                 var entry = FindRadioEntry(fired[i]);
00184:                 if (entry != null && !string.IsNullOrWhiteSpace(entry.audio_cue))
00185:                 {
00186:                     AudioManager.Instance?.PlayCue(AudioCueCatalog.RadioSignalLock);
00187:                     AudioManager.Instance?.PlayVoiceOverCue(entry.audio_cue);
00188:                     break;
00189:                 }
00190:             }
00191:             if (fired.Count > 0) LastEvent = "radio:" + string.Join(";", fired);
00192:             if (fired.Count > 0) RaiseStateChanged();
00193:             return fired;
00194:         }
00195:
00196:         private VerdictCatalogLoader.VerdictRadioEntry? FindRadioEntry(string id)
00197:         {
00198:             for (int i = 0; i < RadioEntries.Count; i++)
00199:                 if (string.Equals(RadioEntries[i].id, id, StringComparison.Ordinal))
00200:                     return RadioEntries[i];
00201:             return null;
00202:         }
00203:
00204:         /// <summary>
00205:         /// Enroll evidence from the reachable verdict items where the authored
00206:         /// mechanical_effects.enrolled_evidence is non-zero. Idempotent via the
00207:         /// EvidenceLedger; never double-enrolls. Returns count enrolled this call.
00208:         /// </summary>
00209:         public int EnrollEvidenceFromItems(int day)
00210:         {
00211:             if (Items == null) return 0;
00212:             int enrolled = 0;
00213:             for (int i = 0; i < Items.Count; i++)
00214:             {
00215:                 var it = Items[i];
00216:                 if (it == null || string.IsNullOrEmpty(it.id)) continue;
00217:                 int amount = it.mechanical_effects != null ? it.mechanical_effects.enrolled_evidence : 0;
00218:                 if (amount <= 0) continue;
00219:                 if (Evidence.Enroll(it.id, day)) enrolled++;
00220:             }
00221:             if (enrolled > 0)
00222:             {
00223:                 LastEvent = "evidence_from_items:" + enrolled;
00224:                 RaiseStateChanged();
00225:             }
00226:             return enrolled;
00227:         }
00228:
00229:         /// <summary>Corruption tick: in Culpable+ phases, post a data-corrupted log entry
00230:         /// at a deterministic schedule (every 11th day of the countdown). Data-driven corpus.</summary>
00231:         public void TickCorruption(int day)
00232:         {
00233:             if (day > _currentDay) _currentDay = day;
00234:             if (Reckoning.Phase < ReckoningPhase.Culpable) return;
00235:             if (day <= 0 || day % 11 != 0) return; // deterministic cadence, not per-frame
00236:             MachineLog.InsertCorruptionMarker(day, _machineRng, (IReadOnlyList<string>)CorruptionCorpus);
00237:         }
00238:
00239:         public VerdictSave CaptureSave()
00240:         {
00241:             return VerdictSaveCodec.Capture(
00242:                 CurrentDaySafe(), MachineLog, Reckoning, Evidence,
00243:                 Census != null ? Census.LastWindowDay : -1,
00244:                 Npcs, Radio, Quests);
00245:         }
00246:
00247:         public void RestoreSave(VerdictSave save)
00248:         {
00249:             VerdictSaveCodec.Restore(save, MachineLog, Reckoning, Evidence, Npcs, Radio, Quests);
00250:             EvidenceChain.ReconcileReadEntries();
00251:             LastEvent = "Verdict state restored.";
00252:         }
00253:
00254:         public string StatusLine()
00255:         {
00256:             return $"Verdict phase: {Reckoning.Phase}; evidence: {Evidence.Count}; " +
00257:                    $"logs read: {MachineLog.ReadCount()}/{MachineLog.Entries.Count}; " +
00258:                    $"locations: {Locations.Count}; " +
00259:                    $"call: {(Reckoning.State.callResolved ? "RESOLVED" : "OPEN")}";
00260:         }
00261:
00262:         public VerdictCatalogLoader.VerdictLocationEntry? FindLocation(string id)
00263:         {
00264:             if (string.IsNullOrEmpty(id)) return null;
00265:             foreach (var loc in Locations)
00266:                 if (loc.id == id) return loc;
00267:             return null;
00268:         }
00269:
00270:         private int CurrentDaySafe()
00271:         {
00272:             if (_currentDay >= 0) return _currentDay;
00273:
00274:             int latestLogDay = -1;
00275:             for (int i = 0; i < MachineLog.Entries.Count; i++)
00276:             {
00277:                 var entry = MachineLog.Entries[i];
00278:                 if (entry != null && entry.day > latestLogDay)
00279:                     latestLogDay = entry.day;
00280:             }
00281:             return latestLogDay >= 0 ? latestLogDay : 0;
00282:         }
00283:     }
00284: }
```

## `src/Host/VerdictSaveStore.cs` — 64 lines; 2,904 bytes; SHA-256 `29963fb9ec5a0497d197114ba7f8e3fb5d3ac36bb3cdcb1e8fb32f694ce84e02`
Declaration index:
- 00021: public static class VerdictSaveStore
- 00037: public static string TryCaptureDirect(VerdictSave state) => s_store.CaptureBare(state);
- 00040: public static VerdictSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
- 00043: public static string TryCapture(VerdictSave state) => s_store.CaptureBare(state);
- 00046: public static VerdictSave? TryRestore(string json) => s_store.RestoreBare(json);
- 00048: public static bool TrySave(VerdictSave save, string pathOverride = null!) =>
- 00051: public static VerdictSave? TryLoad(string pathOverride = null!) =>
- 00055: public static string TryCapturePersisted(VerdictSave save) => s_store.CapturePersisted(save);
- 00057: private static VerdictSave? DecodeVerdict(string raw, IJsonSerializer json)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Save Store : VerdictSaveStore
00004: // Core State : Ashfall.Core.Verdict.VerdictSave
00005: // Host Caller: Main.Verdict / VerdictHostSession
00006: // Purpose    : The Verdict tribunal reckoning stages, evidence dossier, and census tally
00007: // ============================================================================
00008: using System;
00009: using Ashfall.Core;
00010: using Ashfall.Core.Save;
00011: using Ashfall.Core.Verdict;
00012:
00013: namespace AtomicWar.GodotApp
00014: {
00015:     /// <summary>
00016:     /// ASHFALL: THE VERDICT (Expansion 08) — save persistence. Thin façade
00017:     /// over the Core SaveStore&lt;T&gt; service (via SaveStoreHub, codec
00018:     /// flavor). Shape and validation live in <see cref="VerdictSaveCodec"/>;
00019:     /// path resolution, atomic write, and error handling live in the service.
00020:     /// </summary>
00021:     public static class VerdictSaveStore
00022:     {
00023:         public const string FileName = "verdict_save.json";
00024:         public const string SectionName = "verdict";
00025:
00026:         private static readonly SaveStore<VerdictSave> s_store = SaveStoreHub.FromCodec(
00027:             FileName,
00028:             nameof(VerdictSaveStore),
00029:             (save, json) => VerdictSaveCodec.Encode(save, json),
00030:             DecodeVerdict);
00031:
00032:         public static string SavePath => s_store.SavePath;
00033:
00034:         public static bool Exists => s_store.Exists();
00035:
00036:         /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
00037:         public static string TryCaptureDirect(VerdictSave state) => s_store.CaptureBare(state);
00038:
00039:         /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
00040:         public static VerdictSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
00041:
00042:         /// <summary>Capture state to JSON without writing to disk.</summary>
00043:         public static string TryCapture(VerdictSave state) => s_store.CaptureBare(state);
00044:
00045:         /// <summary>Restore state from JSON without reading from disk.</summary>
00046:         public static VerdictSave? TryRestore(string json) => s_store.RestoreBare(json);
00047:
00048:         public static bool TrySave(VerdictSave save, string pathOverride = null!) =>
00049:             s_store.TrySave(save, pathOverride);
00050:
00051:         public static VerdictSave? TryLoad(string pathOverride = null!) =>
00052:             s_store.TryLoad(pathOverride);
00053:
00054:         /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
00055:         public static string TryCapturePersisted(VerdictSave save) => s_store.CapturePersisted(save);
00056:
00057:         private static VerdictSave? DecodeVerdict(string raw, IJsonSerializer json)
00058:         {
00059:             if (VerdictSaveCodec.TryDecode(raw, json, out var save))
00060:                 return save;
00061:             throw new InvalidOperationException("save rejected (bad checksum or version).");
00062:         }
00063:     }
00064: }
```

## `src/Main.Verdict.cs` — 212 lines; 8,065 bytes; SHA-256 `f106b77d3b9ef1bd7ddae6b19a9c3c09b6624c7467d27ee6e575cfed7015b0d9`
Declaration index:
- 00031: public partial class Main : Control
- 00039: private void FlushVerdictIfDirty()
- 00044: private void SetupVerdict()
- 00077: private void TickVerdict(int day, int livingCount)
- 00086: // Phase 6.D Chain 1 (Census / Human Cost): record any dwellings that
- 00096: private int ComputeDwellingDriftDelta(int livingCount, int day)
- 00111: public float LivingCumulativeDoseSieverts()
- 00126: private void UnlockVerdictLore()
- 00145: private void RefreshVerdictReadout()
- 00156: private string VerdictReadoutText()
- 00165: private void SaveVerdict()
- 00175: private void CloseVerdictPanel()
- 00180: private void OnVerdictOpenClicked()
- 00186: private void OnVerdictTickClicked()
- 00193: private void OnVerdictCensusClicked()
- 00201: private int LivingDwellerCountEstimate()
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
00033:         // ── Verdict fields (GAP-ARCH-01 Phase 1) ──
00034:         private AtomicWar.GodotApp.VerdictHostSession _verdict = null!;
00035:         private Godot.Label _verdictReadoutLabel = null!;
00036:         private VerdictPanel _verdictPanel = null!;
00037:         private bool _verdictDirty;
00038:
00039:         private void FlushVerdictIfDirty()
00040:         {
00041:             if (_verdictDirty) SaveVerdict();
00042:         }
00043:
00044:         private void SetupVerdict()
00045:         {
00046:             if (_verdict != null) return;
00047:             _verdict = AtomicWar.GodotApp.VerdictHostSession.Create(_dataDir, flags: _consequenceLedger);
00048:             _verdict.StateChanged += () => { _verdictDirty = true; RefreshVerdictReadout(); };
00049:             UnlockVerdictLore();
00050:             RefreshVerdictReadout();
00051:
00052:             // Items 1+8: the diegetic shelter machine surface + a persistent readout strip
00053:             // (previously declared but never added to the tree).
00054:             if (_verdictReadoutLabel == null)
00055:             {
00056:                 _verdictReadoutLabel = new Label
00057:                 {
00058:                     Text = "[shelter instruments] — standby cycle.",
00059:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00060:                 };
00061:                 _verdictReadoutLabel.AddThemeFontSizeOverride("font_size", 12);
00062:                 _rightColumn.AddChild(_verdictReadoutLabel);
00063:             }
00064:
00065:             if (_verdictPanel == null && _rightColumn != null)
00066:             {
00067:                 _verdictPanel = new VerdictPanel();
00068:                 _rightColumn.AddChild(_verdictPanel);
00069:             }
00070:             _verdictPanel?.Bind(_verdict);
00071:             _verdictPanel?.RefreshView();
00072:
00073:             GD.Print("[Ashfall Godot] Verdict host ready.");
00074:         }
00075:
00076:         /// <summary>Advance the Reckoning state machine + census carrier + chain recorders for the current sim day.</summary>
00077:         private void TickVerdict(int day, int livingCount)
00078:         {
00079:             SetupVerdict();
00080:             _verdict.AdvanceDay(day, Math.Max(1, livingCount), _verdict.MachineLog.ReadCount());
00081:             _verdict.TickCensus();
00082:             _verdict.TickCorruption(day);
00083:             _verdict.TickRadio(day);
00084:             _verdict.EnrollEvidenceFromItems(day);
00085:
00086:             // Phase 6.D Chain 1 (Census / Human Cost): record any dwellings that
00087:             // dropped out of coverage between this day and the previous tick.
00088:             // DriftTotal grows monotonically; day boundaries reset the delta.
00089:             int driftDelta = ComputeDwellingDriftDelta(livingCount, day);
00090:             if (driftDelta > 0) _verdict.Reckoning.RecordDrift(day, driftDelta);
00091:
00092:             UnlockVerdictLore();
00093:             RefreshVerdictReadout();
00094:         }
00095:
00096:         private int ComputeDwellingDriftDelta(int livingCount, int day)
00097:         {
00098:             int delta = 0;
00099:             if (day != _previousLivingDay && _previousLivingDay != -1)
00100:             {
00101:                 if (_previousLivingCount > livingCount) delta = _previousLivingCount - livingCount;
00102:             }
00103:             _previousLivingDay = day;
00104:             _previousLivingCount = livingCount;
00105:             return Math.Max(0, delta);
00106:         }
00107:
00108:         // Phase 6.D Chain 3 (Survival Reckoning) hook surface. Sums
00109:         // LifetimeDose across all living survivors from the live
00110:         // RadiationSystem. Replaces the previous 0f stub.
00111:         public float LivingCumulativeDoseSieverts()
00112:         {
00113:             if (_survivors == null || _survivors.Roster == null) return 0f;
00114:             float total = 0f;
00115:             foreach (var entry in _survivors.Roster.Roster)
00116:             {
00117:                 if (!entry.isAlive) continue;
00118:                 var dosimeter = _survivors.Radiation.GetDosimeter(entry.survivorId);
00119:                 total += dosimeter?.LifetimeDose ?? 0f;
00120:             }
00121:             return total;
00122:         }
00123:
00124:         /// <summary>Unlock lore_verdict_* codex beats from authoritative Verdict state
00125:         /// (located knowledge: the ladder only opens when the machine/evidence reaches it).</summary>
00126:         private void UnlockVerdictLore()
00127:         {
00128:             if (_verdict == null || _journal == null) return;
00129:             if (_verdict.MachineLog.ReadCount() >= 1)
00130:                 _journal.UnlockEventFired("lore_verdict_geophone_one");
00131:             if (_verdict.Evidence.IsEnrolled("evidence_fuse_linen"))
00132:             {
00133:                 _journal.UnlockEventFired("lore_verdict_shift_charters");
00134:                 _journal.UnlockEventFired("lore_verdict_standard");
00135:             }
00136:             if (_verdict.Evidence.IsEnrolled("evidence_uxo_register"))
00137:                 _journal.UnlockEventFired("lore_verdict_the_hold");
00138:             if (_verdict.Reckoning.State.callResolved)
00139:             {
00140:                 _journal.UnlockEventFired("lore_verdict_the_call");
00141:                 _journal.UnlockEventFired("lore_verdict_the_count");
00142:             }
00143:         }
00144:
00145:         private void RefreshVerdictReadout()
00146:         {
00147:             if (_verdict == null || _verdictReadoutLabel == null) return;
00148:             _verdictReadoutLabel.Text = VerdictReadoutText();
00149:         }
00150:
00151:         /// <summary>
00152:         /// CORE-MECH W10 — the instrument line plus the memorial register line.
00153:         /// Appended only when a rite has actually been recorded, so a campaign
00154:         /// that never held a vigil sees exactly the text it saw before.
00155:         /// </summary>
00156:         private string VerdictReadoutText()
00157:         {
00158:             string line = Ashfall.Core.Verdict.VerdictReadout.LineFor(
00159:                 _verdict!.Reckoning.State, _verdict.Evidence.Count, _verdict.MachineLog.ReadCount());
00160:             string rite = Ashfall.Core.Verdict.VerdictReadout.RiteTraceLine(
00161:                 _verdict.Reckoning.RiteTraceCount, _verdict.Evidence.Count);
00162:             return string.IsNullOrEmpty(rite) ? line : line + "\n" + rite;
00163:         }
00164:
00165:         private void SaveVerdict()
00166:         {
00167:             if (_verdict == null) return;
00168:             if (CaptureSection("verdict", AtomicWar.GodotApp.VerdictSaveStore.TryCapturePersisted(_verdict.CaptureSave())))
00169:             {
00170:                 _verdictDirty = false;
00171:                 GD.Print("[Ashfall Godot] Verdict save written.");
00172:             }
00173:         }
00174:
00175:         private void CloseVerdictPanel()
00176:         {
00177:             if (_verdictPanel != null) _verdictPanel.Visible = false;
00178:         }
00179:
00180:         private void OnVerdictOpenClicked()
00181:         {
00182:             SetupVerdict();
00183:             _statusLabel.Text = _verdict.StatusLine() + "\n" + VerdictReadoutText();
00184:         }
00185:
00186:         private void OnVerdictTickClicked()
00187:         {
00188:             SetupVerdict();
00189:             CommitAdvance();
00190:             _statusLabel.Text = _verdict.StatusLine();
00191:         }
00192:
00193:         private void OnVerdictCensusClicked()
00194:         {
00195:             SetupVerdict();
00196:             _verdict.TickCensus();
00197:             _statusLabel.Text = "Census broadcast checked. " + _verdict.StatusLine();
00198:         }
00199:
00200:         /// <summary>Best-available living count without coupling to Survivors internals.</summary>
00201:         private int LivingDwellerCountEstimate()
00202:         {
00203:             if (_survivors != null && _survivors.Roster != null)
00204:             {
00205:                 int count = _survivors.Roster.LivingCount;
00206:                 if (count > 0) return count;
00207:             }
00208:             return 14;
00209:         }
00210:
00211:     }
00212: }
```

## `src/UI/VerdictDashboardPanel.cs` — 178 lines; 7,701 bytes; SHA-256 `d1d57d14186f49a51faace91d3448d4971629702410ebb6ba660f31a7a64e322`
Declaration index:
- 00015: /// bespoke per-record interaction (NPC "hear" buttons, log list, evidence list).
- 00024: public partial class VerdictDashboardPanel : Control
- 00035: public void Bind(VerdictPanel verdict, VerdictHostSession session)
- 00058: private void OnReckoningPhaseChanged(Ashfall.Core.Verdict.ReckoningPhase _) => RefreshView();
- 00059: private void OnVerdictResolved(string _) => RefreshView();
- 00103: private void MountInner()
- 00111: public void RefreshView()
- 00156: public void Open()
- 00163: public void Close() {
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Godot;
00004: using Ashfall.Core.UI;
00005: using AtomicWar.GodotApp;
00006: using DesignTheme = Ashfall.Core.UI.Theme;
00007:
00008: using Ashfall.Core.IO;
00009: namespace AtomicWar.GodotApp.UI;
00010:
00011: /// <summary>
00012: /// ASHFALL — Verdict Dashboard (#15 Stitch).
00013: /// Hosts the existing VerdictPanel inside the dashboard shell so the screen
00014: /// gains the Phase-12 sidebar + status rail chrome without rewriting the
00015: /// bespoke per-record interaction (NPC "hear" buttons, log list, evidence list).
00016: ///
00017: /// Narrative verdict content is deliberately NOT converted into a DataGrid —
00018: /// per the brief, "Do not convert narrative verdict content into a
00019: /// spreadsheet merely for architectural consistency."
00020: ///
00021: /// Reads headline metrics from the bound VerdictHostSession directly; the
00022: /// inner VerdictPanel is mounted as the content slot.
00023: /// </summary>
00024: public partial class VerdictDashboardPanel : Control
00025: {
00026:     public event Action? OnClose;
00027:
00028:     private AshfallDashboardShell _shell = null!;
00029:     private AshfallStatusRail? _statusRail;
00030:     private VerdictPanel? _verdictInner;
00031:     private VerdictHostSession? _session;
00032:
00033:     public bool IsBound => _session != null;
00034:
00035:     public void Bind(VerdictPanel verdict, VerdictHostSession session)
00036:     {
00037:         // Live refresh: reckoning phase and verdict resolution move the board
00038:         // while it is open (silent-failure sweep, 2026-09-25).
00039:         if (_session != null)
00040:         {
00041:             _session.Reckoning.OnPhaseChanged -= OnReckoningPhaseChanged;
00042:             _session.Reckoning.OnVerdictResolved -= OnVerdictResolved;
00043:         }
00044:
00045:         _verdictInner = verdict;
00046:         _session = session;
00047:
00048:         if (_session != null)
00049:         {
00050:             _session.Reckoning.OnPhaseChanged += OnReckoningPhaseChanged;
00051:             _session.Reckoning.OnVerdictResolved += OnVerdictResolved;
00052:         }
00053:         _verdictInner.Bind(session);
00054:         MountInner();
00055:         RefreshView();
00056:     }
00057:
00058:     private void OnReckoningPhaseChanged(Ashfall.Core.Verdict.ReckoningPhase _) => RefreshView();
00059:     private void OnVerdictResolved(string _) => RefreshView();
00060:
00061:     public override void _Ready()
00062:     {
00063:         SetAnchorsPreset(LayoutPreset.FullRect);
00064:         Visible = false;
00065:
00066:         var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.95f) };
00067:         bg.SetAnchorsPreset(LayoutPreset.FullRect);
00068:         AddChild(bg);
00069:
00070:         _shell = new AshfallDashboardShell(
00071:             "VERDICT — THE MACHINE'S REGISTER",
00072:             1100, 720);
00073:
00074:         var hostContainer = new MarginContainer();
00075:         hostContainer.AddThemeConstantOverride("margin_left", DesignTheme.HudEdge);
00076:         hostContainer.AddThemeConstantOverride("margin_top", DesignTheme.SpacingLg);
00077:         hostContainer.AddThemeConstantOverride("margin_right", DesignTheme.HudEdge);
00078:         hostContainer.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingMd);
00079:         hostContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00080:         hostContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00081:         hostContainer.AddChild(_shell);
00082:         AddChild(hostContainer);
00083:
00084:         _shell.SetSidebar(new[]
00085:         {
00086:             new AshfallSidebar.Item { Id = "phase",     Label = "Phase",          Hint = "reckoning state" },
00087:             new AshfallSidebar.Item { Id = "figures",   Label = "Figures",        Hint = "of the record" },
00088:             new AshfallSidebar.Item { Id = "places",    Label = "Places",         Hint = "& evidence" },
00089:             new AshfallSidebar.Item { Id = "transmits", Label = "Transmissions",   Hint = "radio log" },
00090:         }, "VERDICT OPS", "phase");
00091:
00092:         _statusRail = _shell.SetStatusRail();
00093:         _statusRail.AddCard("phase",     "PHASE",    "—", AshfallMetricCard.Criticality.Normal, 130);
00094:         _statusRail.AddCard("evidence",  "EVIDENCE", "0", AshfallMetricCard.Criticality.Normal, 110);
00095:         _statusRail.AddCard("call",      "CALL",     "—", AshfallMetricCard.Criticality.Normal, 110);
00096:         _statusRail.AddCard("figs",      "FIGURES",  "0", AshfallMetricCard.Criticality.Normal, 100);
00097:         _statusRail.AddCard("places",    "PLACES",   "0", AshfallMetricCard.Criticality.Normal, 100);
00098:         _statusRail.AddCard("transmits", "TRANSMITS","0", AshfallMetricCard.Criticality.Normal, 120);
00099:
00100:         _shell.AttachHeaderCloseButton("RETURN TO EXPANSION HUB [Esc]", () => OnClose?.Invoke());
00101:     }
00102:
00103:     private void MountInner()
00104:     {
00105:         if (_verdictInner == null || _shell == null) return;
00106:         _verdictInner.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00107:         _verdictInner.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00108:         _shell.SetContent(_verdictInner);
00109:     }
00110:
00111:     public void RefreshView()
00112:     {
00113:         if (_statusRail == null) return;
00114:         if (_session == null)
00115:         {
00116:             _statusRail.Set("phase",    "—", AshfallMetricCard.Criticality.Normal);
00117:             _statusRail.Set("evidence", "0", AshfallMetricCard.Criticality.Normal);
00118:             _statusRail.Set("call",     "—", AshfallMetricCard.Criticality.Normal);
00119:             _statusRail.Set("figs",     "0", AshfallMetricCard.Criticality.Normal);
00120:             _statusRail.Set("places",   "0", AshfallMetricCard.Criticality.Normal);
00121:             _statusRail.Set("transmits","0", AshfallMetricCard.Criticality.Normal);
00122:             return;
00123:         }
00124:         try
00125:         {
00126:             int evidenceCount = _session.Evidence?.Count ?? 0;
00127:             int readCount = _session.MachineLog?.ReadCount() ?? 0;
00128:             int readTotal = _session.MachineLog?.Entries?.Count ?? 0;
00129:             string phaseName = _session.Reckoning?.Phase.ToString() ?? "—";
00130:             bool callResolved = _session.Reckoning?.State.callResolved ?? false;
00131:             int figs = _session.AvailableNpcs()?.Count ?? 0;
00132:             int places = _session.Locations?.Count ?? 0;
00133:             int transmits = _session.RadioEntries?.Count ?? 0;
00134:
00135:             var phaseInt = (int)(_session.Reckoning?.Phase ?? 0);
00136:             var phaseCrit = phaseInt >= 3 ? AshfallMetricCard.Criticality.Critical
00137:                 : phaseInt >= 2 ? AshfallMetricCard.Criticality.Warn
00138:                 : AshfallMetricCard.Criticality.Normal;
00139:
00140:             _statusRail.Set("phase",    phaseName.ToUpperInvariant(), phaseCrit);
00141:             _statusRail.Set("evidence", $"{evidenceCount}", AshfallMetricCard.Criticality.Normal);
00142:             _statusRail.Set("call",     callResolved ? "RESOLVED" : "OPEN",
00143:                 callResolved ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
00144:             _statusRail.Set("figs",     $"{figs}",     AshfallMetricCard.Criticality.Normal);
00145:             _statusRail.Set("places",   $"{places}",   AshfallMetricCard.Criticality.Normal);
00146:             _statusRail.Set("transmits",$"{transmits}",AshfallMetricCard.Criticality.Normal);
00147:         }
00148:         catch (Exception ex_CATDIAG)
00149:         {
00150:             CatalogDiagnostics.Warn("<ui>", "VerdictDashboard refresh", ex_CATDIAG);
00151:             // bound session with quirky state should not break the dashboard
00152:             _statusRail.Set("phase", "—", AshfallMetricCard.Criticality.Normal);
00153:         }
00154:     }
00155:
00156:     public void Open()
00157:     {
00158:         Visible = true;
00159:         _verdictInner?.Open();
00160:         RefreshView();
00161:     }
00162:
00163:     public void Close() {
00164:             if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
00165:                 Visible = false;
00166:             OnClose?.Invoke();
00167:         }
00168:
00169:     public override void _UnhandledInput(InputEvent @event)
00170:     {
00171:         if (!Visible) return;
00172:         if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00173:         {
00174:             OnClose?.Invoke();
00175:             GetViewport().SetInputAsHandled();
00176:         }
00177:     }
00178: }
```

## `src/VerdictPanel.cs` — 432 lines; 17,793 bytes; SHA-256 `880c980f41d127d45df8a9b2d4b42bb34d01603c53f4015a86e39b45345af50e`
Declaration index:
- 00019: public partial class VerdictPanel : PanelContainer
- 00031: public void Open()
- 00037: public void Close()
- 00128: public void Bind(VerdictHostSession verdict)
- 00134: public void RefreshView()
- 00148: public int RenderedRadioRowCount()
- 00157: private void RefreshPhaseStrip()
- 00186: private void RefreshLog()
- 00229: private void RefreshNpcs()
- 00238: Text = "No figures have stepped forward yet. The record waits.",
- 00295: private void RefreshPlaces()
- 00347: Text = "The record names no places yet. Keep the machine reading.",
- 00359: private void RefreshRadio()
- 00414: private static string Truncate(string s, int max)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: #pragma warning disable CS8618
00004: using Godot;
00005: using Ashfall.Core.Verdict;
00006: using Ashfall.Core.UI;
00007: using AtomicWar.GodotApp.UI;
00008: using CoreTheme = Ashfall.Core.UI.Theme;
00009:
00010: namespace AtomicWar.GodotApp
00011: {
00012:     /// <summary>
00013:     /// ASHFALL: THE VERDICT (Expansion 08) — shelter machine surface.
00014:     /// Diegetic panel presenting the machine log, the Reckoning phase strip
00015:     /// (phase-colored), the shelter readout, evidence counter, and the
00016:     /// available Verdict figures (flag-gated, one-shot spoken). Thin
00017:     /// presentation only; zero simulation logic.
00018:     /// </summary>
00019:     public partial class VerdictPanel : PanelContainer
00020:     {
00021:         public event System.Action? OnClose;
00022:
00023:         private VerdictHostSession _verdict;
00024:         private Label _lblPhase;
00025:         private Label _lblReadout;
00026:         private VBoxContainer _logList;
00027:         private VBoxContainer _npcList;
00028:         private VBoxContainer _placeList;
00029:         private VBoxContainer _radioList;
00030:
00031:         public void Open()
00032:         {
00033:             Visible = true;
00034:             RefreshView();
00035:         }
00036:
00037:         public void Close()
00038:         {
00039:             Visible = false;
00040:             OnClose?.Invoke();
00041:         }
00042:
00043:         public override void _UnhandledInput(InputEvent @event)
00044:         {
00045:             if (!Visible) return;
00046:             if (@event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.Escape)
00047:             {
00048:                 Close();
00049:                 GetViewport().SetInputAsHandled();
00050:             }
00051:         }
00052:
00053:         public override void _Ready()
00054:         {
00055:             SetAnchorsPreset(LayoutPreset.FullRect);
00056:             CustomMinimumSize = new Vector2(CoreTheme.PanelMaxWidth, 400);
00057:
00058:             // Apply standard panel 9-slice via shared helper (frame_9slice first)
00059:             AddThemeStyleboxOverride("panel", AshfallUiHelpers.MakePanelFrameStyleBox());
00060:
00061:             var rootVbox = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00062:             AddChild(rootVbox);
00063:
00064:             // ── Title ──
00065:             rootVbox.AddChild(AshfallUiHelpers.MakeTitle("THE MACHINE'S REGISTER", CoreTheme.FontSizeH3));
00066:
00067:             // ── Phase strip ──
00068:             _lblPhase = new Label { Text = "phase: dormant" };
00069:             _lblPhase.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeBody);
00070:             _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
00071:             rootVbox.AddChild(_lblPhase);
00072:
00073:             // ── Readout ──
00074:             _lblReadout = new Label
00075:             {
00076:                 Text = "[shelter instruments] — standby cycle.",
00077:                 AutowrapMode = TextServer.AutowrapMode.WordSmart
00078:             };
00079:             _lblReadout.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
00080:             _lblReadout.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00081:             rootVbox.AddChild(_lblReadout);
00082:
00083:             rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00084:
00085:             // ── Log scroll ──
00086:             var scroll = new ScrollContainer
00087:             {
00088:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
00089:                 CustomMinimumSize = new Vector2(0, 190)
00090:             };
00091:             rootVbox.AddChild(scroll);
00092:
00093:             _logList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingXs);
00094:             scroll.AddChild(_logList);
00095:
00096:             rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00097:
00098:             // ── Figures ──
00099:             rootVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("FIGURES OF THE RECORD"));
00100:
00101:             _npcList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00102:             rootVbox.AddChild(_npcList);
00103:
00104:             rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00105:             rootVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("PLACES & EVIDENCE"));
00106:
00107:             _placeList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingXs);
00108:             rootVbox.AddChild(_placeList);
00109:
00110:             rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00111:             rootVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("TRANSMISSIONS"));
00112:
00113:             var radioScroll = new ScrollContainer
00114:             {
00115:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
00116:                 CustomMinimumSize = new Vector2(0, 170)
00117:             };
00118:             rootVbox.AddChild(radioScroll);
00119:             _radioList = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingXs);
00120:             radioScroll.AddChild(_radioList);
00121:
00122:             rootVbox.AddChild(AshfallUiHelpers.MakeSeparator());
00123:             var btnClose = AshfallUiHelpers.MakeButton("RETURN TO EXPANSION HUB [ESC]", Close);
00124:             btnClose.CustomMinimumSize = new Vector2(240, 36);
00125:             rootVbox.AddChild(btnClose);
00126:         }
00127:
00128:         public void Bind(VerdictHostSession verdict)
00129:         {
00130:             _verdict = verdict;
00131:             if (verdict != null) verdict.StateChanged += RefreshView;
00132:         }
00133:
00134:         public void RefreshView()
00135:         {
00136:             if (_verdict == null || _logList == null || _radioList == null) return;
00137:
00138:             RefreshPhaseStrip();
00139:             RefreshLog();
00140:             RefreshNpcs();
00141:             RefreshPlaces();
00142:             RefreshRadio();
00143:         }
00144:
00145:         /// <summary>Thin read-only accessor for tests: the number of broadcast rows
00146:         /// currently rendered in the TRANSMISSIONS section (broadcast labels carry a
00147:         /// "[D{day}]" marker; the summary header label is excluded).</summary>
00148:         public int RenderedRadioRowCount()
00149:         {
00150:             if (_radioList == null) return 0;
00151:             int n = 0;
00152:             foreach (Node child in _radioList.GetChildren())
00153:                 if (child is Label lbl && lbl.Text != null && lbl.Text.Contains("[D")) n++;
00154:             return n;
00155:         }
00156:
00157:         private void RefreshPhaseStrip()
00158:         {
00159:             var state = _verdict.Reckoning.State;
00160:             string phaseName = _verdict.Reckoning.Phase.ToString().ToLowerInvariant();
00161:             string callState = state.callResolved ? " · call RESOLVED" : "";
00162:             _lblPhase.Text = $"phase: {phaseName} · evidence {_verdict.Evidence.Count} · " +
00163:                              $"logs read {_verdict.MachineLog.ReadCount()}/{_verdict.MachineLog.Entries.Count}{callState}";
00164:
00165:             // Phase-colored strip — uses Theme tokens for each phase.
00166:             switch (_verdict.Reckoning.Phase)
00167:             {
00168:                 case ReckoningPhase.Dormant:
00169:                     _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
00170:                     break;
00171:                 case ReckoningPhase.Knowing:
00172:                     _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00173:                     break;
00174:                 case ReckoningPhase.Culpable:
00175:                     _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Warm));
00176:                     break;
00177:                 case ReckoningPhase.Counted:
00178:                     _lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Critical));
00179:                     break;
00180:             }
00181:
00182:             _lblReadout.Text = VerdictReadout.LineFor(
00183:                 state, _verdict.Evidence.Count, _verdict.MachineLog.ReadCount());
00184:         }
00185:
00186:         private void RefreshLog()
00187:         {
00188:             AshfallUiHelpers.EmptyChildren(_logList);
00189:
00190:             var entries = _verdict.MachineLog.Entries;
00191:             int shown = 0;
00192:             for (int i = entries.Count - 1; i >= 0 && shown < 12; i--, shown++)
00193:             {
00194:                 var e = entries[i];
00195:                 string flag = e.read ? "read" : "unread";
00196:                 string icon = e.kind switch
00197:                 {
00198:                     "maintenance" => "⚙",
00199:                     "anomaly" => "◆",
00200:                     "count" => "∑",
00201:                     _ => "·"
00202:                 };
00203:                 var row = new Label
00204:                 {
00205:                     Text = $"{icon} [D{e.day}] {e.facilityId} · {e.kind} · {flag}\n   {e.bodyShort}",
00206:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00207:                 };
00208:                 row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00209:                 if (!e.read)
00210:                     row.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00211:                 else
00212:                     row.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
00213:                 _logList.AddChild(row);
00214:             }
00215:
00216:             if (shown == 0)
00217:             {
00218:                 var empty = new Label
00219:                 {
00220:                     Text = "The log is quiet. The meter reads its own current.",
00221:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00222:                 };
00223:                 empty.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00224:                 empty.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00225:                 _logList.AddChild(empty);
00226:             }
00227:         }
00228:
00229:         private void RefreshNpcs()
00230:         {
00231:             AshfallUiHelpers.EmptyChildren(_npcList);
00232:
00233:             var available = _verdict.AvailableNpcs();
00234:             if (available.Count == 0)
00235:             {
00236:                 var none = new Label
00237:                 {
00238:                     Text = "No figures have stepped forward yet. The record waits.",
00239:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00240:                 };
00241:                 none.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00242:                 none.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00243:                 _npcList.AddChild(none);
00244:                 return;
00245:             }
00246:
00247:             for (int i = 0; i < available.Count; i++)
00248:             {
00249:                 var npc = available[i];
00250:                 string kindIcon = npc.kind switch
00251:                 {
00252:                     "tape_echo" => "▤",
00253:                     "paper_ghost" => "✉",
00254:                     "living" => "◉",
00255:                     "readings" => "▥",
00256:                     _ => "·"
00257:                 };
00258:                 string shown = _verdict.Npcs.State.spokenNpcIds.Contains(npc.id) ? "(spoken)" : "";
00259:                 var row = new Label
00260:                 {
00261:                     Text = $"{kindIcon} {npc.name} — {npc.role} {shown}",
00262:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00263:                 };
00264:                 row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
00265:                 row.AddThemeColorOverride("font_color",
00266:                     _verdict.Npcs.State.spokenNpcIds.Contains(npc.id)
00267:                         ? AshfallUiHelpers.ToColor(CoreTheme.Muted)
00268:                         : AshfallUiHelpers.ToColor(CoreTheme.Pale));
00269:
00270:                 if (!_verdict.Npcs.State.spokenNpcIds.Contains(npc.id))
00271:                 {
00272:                     var btn = AshfallUiHelpers.MakeButton("hear", () =>
00273:                     {
00274:                         if (_verdict.Npcs.Speak(npc.id))
00275:                         {
00276:                             RefreshView();
00277:                             EmitSignal("NpcSpoken", npc.id);
00278:                         }
00279:                     });
00280:                     var h = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
00281:                     h.AddChild(row);
00282:                     h.AddChild(btn);
00283:                     _npcList.AddChild(h);
00284:                 }
00285:                 else
00286:                 {
00287:                     _npcList.AddChild(row);
00288:                 }
00289:             }
00290:         }
00291:
00292:         /// <summary>Render the reachable Verdict places and evidence/story items.
00293:         /// Thin presentation: lists what the machine's records point to (the four
00294:         /// standing sites and the fifteen evidence/quest objects) as read-only rows.</summary>
00295:         private void RefreshPlaces()
00296:         {
00297:             AshfallUiHelpers.EmptyChildren(_placeList);
00298:
00299:             bool any = false;
00300:             if (_verdict.Locations != null)
00301:             {
00302:                 for (int i = 0; i < _verdict.Locations.Count; i++)
00303:                 {
00304:                     var loc = _verdict.Locations[i];
00305:                     if (loc == null || string.IsNullOrEmpty(loc.displayName)) continue;
00306:                     any = true;
00307:                     var row = new Label
00308:                     {
00309:                         Text = $"▨ {loc.displayName} ({loc.id}) · danger {loc.dangerLevel}\n   {Truncate(loc.description, 180)}",
00310:                         AutowrapMode = TextServer.AutowrapMode.WordSmart
00311:                     };
00312:                     row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00313:                     row.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00314:                     _placeList.AddChild(row);
00315:                 }
00316:             }
00317:             if (_verdict.Items != null)
00318:             {
00319:                 for (int i = 0; i < _verdict.Items.Count; i++)
00320:                 {
00321:                     var it = _verdict.Items[i];
00322:                     if (it == null || string.IsNullOrEmpty(it.id)) continue;
00323:                     any = true;
00324:                     string kind = string.IsNullOrEmpty(it.category) ? "story_item" : it.category;
00325:                     string icon = kind switch
00326:                     {
00327:                         "consumable" => "⊕",
00328:                         "quest_item" => "◆",
00329:                         _ => "◈"
00330:                     };
00331:                     var row = new Label
00332:                     {
00333:                         Text = $"{icon} {it.displayName} ({it.id}) · {kind}",
00334:                         AutowrapMode = TextServer.AutowrapMode.WordSmart
00335:                     };
00336:                     row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00337:                     row.AddThemeColorOverride("font_color",
00338:                         it.id.StartsWith("evidence_") ? AshfallUiHelpers.ToColor(CoreTheme.Warm)
00339:                                                        : AshfallUiHelpers.ToColor(CoreTheme.Muted));
00340:                     _placeList.AddChild(row);
00341:                 }
00342:             }
00343:             if (!any)
00344:             {
00345:                 var empty = new Label
00346:                 {
00347:                     Text = "The record names no places yet. Keep the machine reading.",
00348:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00349:                 };
00350:                 empty.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00351:                 empty.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00352:                 _placeList.AddChild(empty);
00353:             }
00354:         }
00355:
00356:         /// <summary>Render the diegetic radio corpus (verdict_radio.json). Lists each
00357:         /// broadcast with its dayTrigger and kind, marking fired vs pending. Drivers
00358:         /// from the session's VerdictRadioSystem state. Thin presentation only.</summary>
00359:         private void RefreshRadio()
00360:         {
00361:             AshfallUiHelpers.EmptyChildren(_radioList);
00362:             if (_verdict.Radio == null)
00363:             {
00364:                 _radioList.AddChild(AshfallUiHelpers.MakeSmall("The radio is silent.", true));
00365:                 return;
00366:             }
00367:
00368:             var radio = _verdict.Radio;
00369:             var header = new Label
00370:             {
00371:                 Text = $"{radio.FiredCount}/{radio.Corpus.Count} broadcasts received",
00372:                 AutowrapMode = TextServer.AutowrapMode.WordSmart
00373:             };
00374:             header.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeSmall);
00375:             header.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00376:             _radioList.AddChild(header);
00377:
00378:             bool any = false;
00379:             if (radio.Corpus != null)
00380:             {
00381:                 for (int i = 0; i < radio.Corpus.Count; i++)
00382:                 {
00383:                     var r = radio.Corpus[i];
00384:                     if (r == null || string.IsNullOrEmpty(r.id)) continue;
00385:                     any = true;
00386:                     bool isFired = radio.HasFired(r.id);
00387:                     string kindIcon = r.kind switch
00388:                     {
00389:                         "carrier" => "◌",
00390:                         "call" => "▸",
00391:                         "maintenance" => "⚙",
00392:                         "witness" => "✉",
00393:                         "readings" => "▥",
00394:                         _ => "·"
00395:                     };
00396:                     var row = new Label
00397:                     {
00398:                         Text = $"{kindIcon} {Truncate(r.message, 90)} \n   [D{r.dayTrigger}] {r.id} · {(isFired ? "RECEIVED" : "pending")}",
00399:                         AutowrapMode = TextServer.AutowrapMode.WordSmart
00400:                     };
00401:                     row.AddThemeFontSizeOverride("font_size", CoreTheme.FontSizeLabel);
00402:                     row.AddThemeColorOverride("font_color",
00403:                         isFired ? AshfallUiHelpers.ToColor(CoreTheme.Pale)
00404:                                 : AshfallUiHelpers.ToColor(CoreTheme.Muted));
00405:                     _radioList.AddChild(row);
00406:                 }
00407:             }
00408:             if (!any)
00409:             {
00410:                 _radioList.AddChild(AshfallUiHelpers.MakeSmall("No broadcasts logged yet.", true));
00411:             }
00412:         }
00413:
00414:         private static string Truncate(string s, int max)
00415:         {
00416:             if (string.IsNullOrEmpty(s) || s.Length <= max) return s ?? string.Empty;
00417:             return s.Substring(0, max) + "…";
00418:         }
00419:
00420:         [Signal]
00421:         public delegate void NpcSpokenEventHandler(string npcId);
00422:
00423:         public override void _ExitTree()
00424:         {
00425:             if (_verdict != null)
00426:             {
00427:                 _verdict.StateChanged -= RefreshView;
00428:             }
00429:             base._ExitTree();
00430:         }
00431:     }
00432: }
```

## `Ashfall.Core.Tests/Verdict/VerdictRadioExpansionTests.cs` — 258 lines; 10,583 bytes; SHA-256 `629c4df3b0e14c0a482bdcbe7b40f33918aaa01393ca3681efe45f1c65812236`
Declaration index:
- 00014: public class VerdictRadioExpansionTests : CatalogTestBase
- 00016: private static List<VerdictCatalogLoader.VerdictRadioEntry> LoadRadio()
- 00026: public void Catalog_Loads_All_30_Broadcasts()
- 00033: public void All_30_Broadcast_Ids_Are_Unique_And_Prefixed()
- 00046: public void Baseline_13_Broadcasts_Preserved_Verbatim()
- 00080: public void All_17_Plan94_New_Broadcasts_Present()
- 00125: public void Plan94_Requested_Kind_Distribution_Matches()
- 00147: public void Frequency_And_Signal_Strength_Integrity()
- 00161: public void DayTrigger_Semantics_And_Chronology()
- 00195: public void OneShot_And_State_RoundTrip()
- 00226: public void UnifiedRadioBroadcast_Catalog_Loads_Verdict_Broadcasts()
- 00247: public void AudioCueIntegrity_No_New_Broadcasts_Define_Dangling_Cues()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Events;
00008: using Ashfall.Core.Radio;
00009: using Ashfall.Core.Verdict;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.Verdict
00013: {
00014:     public class VerdictRadioExpansionTests : CatalogTestBase
00015:     {
00016:         private static List<VerdictCatalogLoader.VerdictRadioEntry> LoadRadio()
00017:         {
00018:             var files = new FileSystemIO();
00019:             var json = new SystemTextJsonSerializer();
00020:             var list = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
00021:             Assert.NotNull(list);
00022:             return list;
00023:         }
00024:
00025:         [Fact]
00026:         public void Catalog_Loads_All_30_Broadcasts()
00027:         {
00028:             var list = LoadRadio();
00029:             Assert.Equal(30, list.Count);
00030:         }
00031:
00032:         [Fact]
00033:         public void All_30_Broadcast_Ids_Are_Unique_And_Prefixed()
00034:         {
00035:             var list = LoadRadio();
00036:             var ids = list.Select(e => e.id).ToList();
00037:             var distinct = ids.Distinct(StringComparer.Ordinal).ToList();
00038:             Assert.Equal(30, distinct.Count);
00039:             foreach (var id in ids)
00040:             {
00041:                 Assert.True(id.StartsWith("radio_verdict_"), $"Broadcast id '{id}' must start with radio_verdict_ prefix");
00042:             }
00043:         }
00044:
00045:         [Fact]
00046:         public void Baseline_13_Broadcasts_Preserved_Verbatim()
00047:         {
00048:             var list = LoadRadio();
00049:             var baselineIds = new[]
00050:             {
00051:                 "radio_verdict_meter_reads_1142",
00052:                 "radio_verdict_fuse_serviced",
00053:                 "radio_verdict_wing_sleeps",
00054:                 "radio_verdict_off_count_assessed",
00055:                 "radio_verdict_eden_was_here",
00056:                 "radio_verdict_count_is_open",
00057:                 "radio_verdict_clock_disagrees",
00058:                 "radio_verdict_geophone_taps",
00059:                 "radio_verdict_valve_accessed_36",
00060:                 "radio_verdict_reels_matter",
00061:                 "radio_verdict_presentation_names_holders",
00062:                 "radio_verdict_carrier_on_window",
00063:                 "radio_verdict_reckoning_call"
00064:             };
00065:
00066:             foreach (var bId in baselineIds)
00067:             {
00068:                 var entry = list.Find(e => e.id == bId);
00069:                 Assert.NotNull(entry);
00070:                 Assert.False(string.IsNullOrWhiteSpace(entry.frequency));
00071:                 Assert.False(string.IsNullOrWhiteSpace(entry.source));
00072:                 Assert.False(string.IsNullOrWhiteSpace(entry.message));
00073:                 Assert.False(string.IsNullOrWhiteSpace(entry.signalStrength));
00074:                 Assert.False(string.IsNullOrWhiteSpace(entry.kind));
00075:                 Assert.True(entry.dayTrigger >= 210);
00076:             }
00077:         }
00078:
00079:         [Fact]
00080:         public void All_17_Plan94_New_Broadcasts_Present()
00081:         {
00082:             var list = LoadRadio();
00083:             var plan94Ids = new[]
00084:             {
00085:                 "radio_verdict_barometric_spread",
00086:                 "radio_verdict_service_cycle_greywater",
00087:                 "radio_verdict_stilling_well_delta",
00088:                 "radio_verdict_subsector_ledger_update",
00089:                 "radio_verdict_geophone_offset_recal",
00090:                 "radio_verdict_strata_density_drift",
00091:                 "radio_verdict_relay_switch_pass4",
00092:                 "radio_verdict_unscheduled_burst_88",
00093:                 "radio_verdict_river_stage_deviation",
00094:                 "radio_verdict_core_vault_desiccant_purge",
00095:                 "radio_verdict_unverified_household_tally",
00096:                 "radio_verdict_repeater_origin_mismatch",
00097:                 "radio_verdict_spectrometry_drift_stjude",
00098:                 "radio_verdict_substation_breaker_test",
00099:                 "radio_verdict_holding_capacity_parity",
00100:                 "radio_verdict_telemetry_phase_inversion",
00101:                 "radio_verdict_carrier_override_standby"
00102:             };
00103:
00104:             foreach (var id in plan94Ids)
00105:             {
00106:                 var entry = list.Find(e => e.id == id);
00107:                 Assert.NotNull(entry);
00108:                 Assert.False(string.IsNullOrWhiteSpace(entry.frequency));
00109:                 Assert.False(string.IsNullOrWhiteSpace(entry.source));
00110:                 Assert.False(string.IsNullOrWhiteSpace(entry.message));
00111:                 Assert.False(string.IsNullOrWhiteSpace(entry.signalStrength));
00112:                 Assert.False(string.IsNullOrWhiteSpace(entry.kind));
00113:                 Assert.InRange(entry.dayTrigger, 265, 365);
00114:
00115:                 // Terseness budget: 1 to 4 sentences, concise machine-like register
00116:                 var sentences = System.Text.RegularExpressions.Regex.Split(entry.message, @"(?<=[.!?])\s+")
00117:                     .Where(s => !string.IsNullOrWhiteSpace(s))
00118:                     .ToArray();
00119:                 Assert.InRange(sentences.Length, 1, 5);
00120:                 Assert.True(entry.message.Length <= 250, $"Message '{entry.id}' exceeds terseness budget");
00121:             }
00122:         }
00123:
00124:         [Fact]
00125:         public void Plan94_Requested_Kind_Distribution_Matches()
00126:         {
00127:             var list = LoadRadio();
00128:             var newBroadcasts = list.Skip(13).ToList();
00129:             Assert.Equal(17, newBroadcasts.Count);
00130:
00131:             var telemetry = newBroadcasts.Where(e => e.kind == "telemetry").ToList();
00132:             var maintenance = newBroadcasts.Where(e => e.kind == "maintenance").ToList();
00133:             var census = newBroadcasts.Where(e => e.kind == "census").ToList();
00134:             var calibration = newBroadcasts.Where(e => e.kind == "calibration").ToList();
00135:             var anomaly = newBroadcasts.Where(e => e.kind == "anomaly").ToList();
00136:             var emergency = newBroadcasts.Where(e => e.kind == "emergency").ToList();
00137:
00138:             Assert.Equal(5, telemetry.Count);
00139:             Assert.Equal(4, maintenance.Count);
00140:             Assert.Equal(3, census.Count);
00141:             Assert.Equal(2, calibration.Count);
00142:             Assert.Equal(2, anomaly.Count);
00143:             Assert.Equal(1, emergency.Count);
00144:         }
00145:
00146:         [Fact]
00147:         public void Frequency_And_Signal_Strength_Integrity()
00148:         {
00149:             var list = LoadRadio();
00150:             var validFrequencies = new HashSet<string>(StringComparer.Ordinal) { "99.0 MHz", "88.5 MHz" };
00151:             var validStrengths = new HashSet<string>(StringComparer.Ordinal) { "S1", "S2", "S3", "S4", "S5" };
00152:
00153:             foreach (var b in list)
00154:             {
00155:                 Assert.Contains(b.frequency, validFrequencies);
00156:                 Assert.Contains(b.signalStrength, validStrengths);
00157:             }
00158:         }
00159:
00160:         [Fact]
00161:         public void DayTrigger_Semantics_And_Chronology()
00162:         {
00163:             var list = LoadRadio();
00164:             var bus = new SimpleEventBus();
00165:             var sys = new VerdictRadioSystem(bus, null, list);
00166:
00167:             // Prior to carrier window (day < 210) or before Culpable phase -> nothing fires
00168:             var early = sys.Poll(200, ReckoningPhase.Counted);
00169:             Assert.Empty(early);
00170:
00171:             var unc = sys.Poll(365, ReckoningPhase.Dormant);
00172:             Assert.Empty(unc);
00173:
00174:             // Day 270 at Culpable: broadcasts with dayTrigger <= 270 fire
00175:             var day270 = sys.Poll(270, ReckoningPhase.Culpable);
00176:             Assert.Contains("radio_verdict_meter_reads_1142", day270);
00177:             Assert.Contains("radio_verdict_barometric_spread", day270); // day 268
00178:             Assert.DoesNotContain("radio_verdict_service_cycle_greywater", day270); // day 272
00179:             Assert.DoesNotContain("radio_verdict_carrier_override_standby", day270); // day 360
00180:
00181:             // Re-polling at day 270 does not duplicate (one-shot)
00182:             var repoll = sys.Poll(270, ReckoningPhase.Culpable);
00183:             Assert.Empty(repoll);
00184:
00185:             // Day 365: remainder fires
00186:             var endRun = sys.Poll(365, ReckoningPhase.Counted);
00187:             Assert.Contains("radio_verdict_service_cycle_greywater", endRun);
00188:             Assert.Contains("radio_verdict_carrier_override_standby", endRun);
00189:
00190:             // Total fired = 30
00191:             Assert.Equal(30, sys.FiredCount);
00192:         }
00193:
00194:         [Fact]
00195:         public void OneShot_And_State_RoundTrip()
00196:         {
00197:             var list = LoadRadio();
00198:             var bus = new SimpleEventBus();
00199:             var sys = new VerdictRadioSystem(bus, null, list);
00200:
00201:             // Fire up to day 280
00202:             sys.Poll(280, ReckoningPhase.Culpable);
00203:             int countAt280 = sys.FiredCount;
00204:             Assert.True(countAt280 > 0);
00205:
00206:             // Capture state
00207:             var state = sys.CaptureState();
00208:             Assert.Equal(countAt280, state.firedIds.Count);
00209:
00210:             // Restore into a fresh system
00211:             var restored = new VerdictRadioSystem(new SimpleEventBus(), null, list);
00212:             restored.RestoreState(state);
00213:             Assert.Equal(countAt280, restored.FiredCount);
00214:
00215:             // Re-poll at day 280 -> 0 new fires
00216:             var noNew = restored.Poll(280, ReckoningPhase.Culpable);
00217:             Assert.Empty(noNew);
00218:
00219:             // Poll at day 365 -> only the unfired ones fire
00220:             var laterFired = restored.Poll(365, ReckoningPhase.Culpable);
00221:             Assert.Equal(30 - countAt280, laterFired.Count);
00222:             Assert.Equal(30, restored.FiredCount);
00223:         }
00224:
00225:         [Fact]
00226:         public void UnifiedRadioBroadcast_Catalog_Loads_Verdict_Broadcasts()
00227:         {
00228:             var files = new FileSystemIO();
00229:             var json = new SystemTextJsonSerializer();
00230:             var cat = new RadioBroadcastCatalog();
00231:             cat.LoadFromDataDirectory(DataDirectory, files, json);
00232:
00233:             var first = cat.GetById("radio_verdict_meter_reads_1142");
00234:             Assert.NotNull(first);
00235:             Assert.Equal(BroadcastGenre.VerdictCensus, first!.Genre);
00236:
00237:             var baro = cat.GetById("radio_verdict_barometric_spread");
00238:             Assert.NotNull(baro);
00239:             Assert.Equal(268, baro!.DayTrigger);
00240:
00241:             var standby = cat.GetById("radio_verdict_carrier_override_standby");
00242:             Assert.NotNull(standby);
00243:             Assert.Equal(360, standby!.DayTrigger);
00244:         }
00245:
00246:         [Fact]
00247:         public void AudioCueIntegrity_No_New_Broadcasts_Define_Dangling_Cues()
00248:         {
00249:             var list = LoadRadio();
00250:             var newBroadcasts = list.Skip(13).ToList();
00251:             foreach (var b in newBroadcasts)
00252:             {
00253:                 // New broadcasts deliberately omit audio_cue to avoid dangling references
00254:                 Assert.True(string.IsNullOrEmpty(b.audio_cue), $"New broadcast '{b.id}' should not define audio_cue unless registered");
00255:             }
00256:         }
00257:     }
00258: }
```

## `Ashfall.Core.Tests/VerdictRadioSystemTests.cs` — 143 lines; 5,633 bytes; SHA-256 `76b50d8a93d1d6b0598e21ffe84369f3fe1dc3cd61530b17aa0fe51c9f77338b`
Declaration index:
- 00016: public class VerdictRadioSystemTests
- 00018: private static List<VerdictCatalogLoader.VerdictRadioEntry> SampleCorpus()
- 00047: public void Poll_GatesOnCulpableWindow_NothingBefore()
- 00056: public void Poll_FiresCorpusOnceInsideWindow()
- 00069: public void Poll_FiresAllAtDeadline()
- 00079: public void FiredBroadcastsPublishToBus()
- 00090: public void SaveLoad_RoundTripsFiredIds_NoReplay()
- 00110: public void LoadFrom_LoadsThirtyAuthoredBroadcasts()
- 00121: public void EvidenceEnrollment_FieldsPresentInItems()
- 00135: private static string FindDataDir()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: using Ashfall.Core;
00004: using Ashfall.Core.Clock;
00005: using Ashfall.Core.Events;
00006: using Ashfall.Core.Verdict;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     /// <summary>
00012:     /// ASHFALL: THE VERDICT (Expansion 08) — diegetic radio corpus engine tests.
00013:     /// Verifies the 13 authored broadcasts load, fire once each, gate on the
00014:     /// Culpable+ window, publish to the shared bus, and round-trip fired-ids.
00015:     /// </summary>
00016:     public class VerdictRadioSystemTests
00017:     {
00018:         private static List<VerdictCatalogLoader.VerdictRadioEntry> SampleCorpus()
00019:         {
00020:             var list = new List<VerdictCatalogLoader.VerdictRadioEntry>();
00021:             void add(string id, int day, string kind)
00022:             {
00023:                 list.Add(new VerdictCatalogLoader.VerdictRadioEntry
00024:                 {
00025:                     id = id,
00026:                     frequency = "99.0 MHz",
00027:                     dayTrigger = day,
00028:                     source = "Census Carrier",
00029:                     message = id,
00030:                     kind = kind
00031:                 });
00032:             }
00033:             add("radio_verdict_pilot", 210, "carrier");
00034:             add("radio_verdict_reckoning", 241, "call");
00035:             add("radio_verdict_late", 300, "telemetry");
00036:             return list;
00037:         }
00038:
00039:         private static (SimpleEventBus bus, VerdictRadioSystem sys) Build()
00040:         {
00041:             var bus = new SimpleEventBus();
00042:             var sys = new VerdictRadioSystem(bus, null, SampleCorpus());
00043:             return (bus, sys);
00044:         }
00045:
00046:         [Fact]
00047:         public void Poll_GatesOnCulpableWindow_NothingBefore()
00048:         {
00049:             var (bus, sys) = Build();
00050:             // Dormant / Knowing: nothing fires even past dayTrigger.
00051:             Assert.Empty(sys.Poll(400, ReckoningPhase.Dormant));
00052:             Assert.Empty(sys.Poll(400, ReckoningPhase.Knowing));
00053:         }
00054:
00055:         [Fact]
00056:         public void Poll_FiresCorpusOnceInsideWindow()
00057:         {
00058:             var (bus, sys) = Build();
00059:             // Culpable at day 211: the pilot (trigger 210) fires; not the call (241).
00060:             var fired = sys.Poll(211, ReckoningPhase.Culpable);
00061:             Assert.Contains("radio_verdict_pilot", fired);
00062:             Assert.DoesNotContain("radio_verdict_reckoning", fired);
00063:
00064:             // Re-poll same day: nothing new (idempotent).
00065:             Assert.Empty(sys.Poll(211, ReckoningPhase.Culpable));
00066:         }
00067:
00068:         [Fact]
00069:         public void Poll_FiresAllAtDeadline()
00070:         {
00071:             var (bus, sys) = Build();
00072:             var fired = sys.Poll(301, ReckoningPhase.Counted);
00073:             Assert.Contains("radio_verdict_pilot", fired);
00074:             Assert.Contains("radio_verdict_reckoning", fired);
00075:             Assert.Contains("radio_verdict_late", fired);
00076:         }
00077:
00078:         [Fact]
00079:         public void FiredBroadcastsPublishToBus()
00080:         {
00081:             var (bus, sys) = Build();
00082:             sys.Poll(301, ReckoningPhase.Counted);
00083:             int published = 0;
00084:             foreach (var e in bus.PublishedEvents)
00085:                 if (e.name == "radio.verdict.broadcast") published++;
00086:             Assert.Equal(3, published);
00087:         }
00088:
00089:         [Fact]
00090:         public void SaveLoad_RoundTripsFiredIds_NoReplay()
00091:         {
00092:             var (bus, sys) = Build();
00093:             sys.Poll(241, ReckoningPhase.Counted); // pilot + call fired; late (300) not yet
00094:
00095:             var restored = new VerdictRadioSystem(new SimpleEventBus(), null, SampleCorpus());
00096:             restored.RestoreState(sys.CaptureState());
00097:
00098:             Assert.True(restored.HasFired("radio_verdict_pilot"));
00099:             Assert.True(restored.HasFired("radio_verdict_reckoning"));
00100:             Assert.False(restored.HasFired("radio_verdict_late"));
00101:
00102:             // Re-poll at day 301: only the late one fires (no replay).
00103:             var fired = restored.Poll(301, ReckoningPhase.Counted);
00104:             Assert.Contains("radio_verdict_late", fired);
00105:             Assert.DoesNotContain("radio_verdict_pilot", fired);
00106:             Assert.DoesNotContain("radio_verdict_reckoning", fired);
00107:         }
00108:
00109:         [Fact]
00110:         public void LoadFrom_LoadsThirtyAuthoredBroadcasts()
00111:         {
00112:             string dataDir = FindDataDir();
00113:             if (string.IsNullOrEmpty(dataDir)) return;
00114:             var sys = new VerdictRadioSystem();
00115:             int n = sys.LoadFrom(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00116:             Assert.Equal(30, n);
00117:             Assert.Equal(30, sys.Corpus.Count);
00118:         }
00119:
00120:         [Fact]
00121:         public void EvidenceEnrollment_FieldsPresentInItems()
00122:         {
00123:             // The authored verdict_items.json induces enrollment via
00124:             // mechanical_effects.enrolled_evidence when present.
00125:             string dataDir = FindDataDir();
00126:             if (string.IsNullOrEmpty(dataDir)) return;
00127:             var items = VerdictCatalogLoader.LoadItems(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00128:             int withEffect = 0;
00129:             foreach (var it in items)
00130:                 if (it.mechanical_effects != null && it.mechanical_effects.enrolled_evidence > 0) withEffect++;
00131:             // The authored file marks the 12 evidence_* rows as enrolling.
00132:             Assert.Equal(12, withEffect);
00133:         }
00134:
00135:         private static string FindDataDir()
00136:         {
00137:             string start = System.IO.Directory.GetCurrentDirectory();
00138:             if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
00139:             if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found)) return found;
00140:             return string.Empty;
00141:         }
00142:     }
00143: }
```

## `Ashfall.Core.Tests/VerdictContentWebTests.cs` — 140 lines; 5,757 bytes; SHA-256 `43a025fb0d48c169b04c95a2ec7fcf6fbe57d26de306fce3ce4108895e533eaf`
Declaration index:
- 00015: public class VerdictContentWebTests
- 00017: private static string FindDataDir()
- 00026: public void LoadItems_ReturnsAllFifteenRows()
- 00037: public void LoadItems_IdsAreUniqueAndSnakeCase()
- 00056: public void LoadItems_EvidenceAndQuestItemsPresent()
- 00077: public void LoadItems_RowsAlignToRuntimeSchema()
- 00093: public void LoadLocations_ReturnsFifteenSites()
- 00114: public void LoadRadio_LoadsThirtyAuthoredBroadcasts()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: using Ashfall.Core;
00004: using Ashfall.Core.Verdict;
00005: using Xunit;
00006:
00007: namespace Ashfall.Core.Tests
00008: {
00009:     /// <summary>
00010:     /// ASHFALL: THE VERDICT (Expansion 08) — content web reachability for the
00011:     /// verdict_items.json catalog, which was previously data-only (loaded by no
00012:     /// runtime). Verifies the typed loader returns all 15 rows with unique,
00013:     /// snake_case ids and that the items align to the runtime DTO.
00014:     /// </summary>
00015:     public class VerdictContentWebTests
00016:     {
00017:         private static string FindDataDir()
00018:         {
00019:             string start = System.IO.Directory.GetCurrentDirectory();
00020:             if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
00021:             if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found)) return found;
00022:             return string.Empty;
00023:         }
00024:
00025:         [Fact]
00026:         public void LoadItems_ReturnsAllFifteenRows()
00027:         {
00028:             string dataDir = FindDataDir();
00029:             if (string.IsNullOrEmpty(dataDir)) return;
00030:
00031:             var items = VerdictCatalogLoader.LoadItems(
00032:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00033:             Assert.Equal(15, items.Count);
00034:         }
00035:
00036:         [Fact]
00037:         public void LoadItems_IdsAreUniqueAndSnakeCase()
00038:         {
00039:             string dataDir = FindDataDir();
00040:             if (string.IsNullOrEmpty(dataDir)) return;
00041:
00042:             var items = VerdictCatalogLoader.LoadItems(
00043:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00044:             var seen = new HashSet<string>();
00045:             foreach (var it in items)
00046:             {
00047:                 Assert.False(string.IsNullOrEmpty(it.id));
00048:                 Assert.False(string.IsNullOrEmpty(it.displayName));
00049:                 // snake_case check (lowercase alnum + underscore only)
00050:                 Assert.Matches("^[a-z0-9_]+$", it.id);
00051:                 Assert.True(seen.Add(it.id), $"duplicate id {it.id}");
00052:             }
00053:         }
00054:
00055:         [Fact]
00056:         public void LoadItems_EvidenceAndQuestItemsPresent()
00057:         {
00058:             string dataDir = FindDataDir();
00059:             if (string.IsNullOrEmpty(dataDir)) return;
00060:
00061:             var items = VerdictCatalogLoader.LoadItems(
00062:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00063:             var ids = new HashSet<string>();
00064:             foreach (var it in items) ids.Add(it.id);
00065:
00066:             // The twelve evidence fragments + three non-evidence (quest/consumable).
00067:             Assert.Contains("evidence_eden_log", ids);
00068:             Assert.Contains("evidence_geophone_hymn", ids);
00069:             Assert.Contains("evidence_fuse_linen", ids);
00070:             Assert.Contains("evidence_twelve_gauge_steel", ids);
00071:             Assert.Contains("item_archive_tape_silo_key", ids);
00072:             Assert.Contains("item_fuse_world_shift_charter", ids);
00073:             Assert.Contains("item_verdict_salt_flat_sample", ids);
00074:         }
00075:
00076:         [Fact]
00077:         public void LoadItems_RowsAlignToRuntimeSchema()
00078:         {
00079:             string dataDir = FindDataDir();
00080:             if (string.IsNullOrEmpty(dataDir)) return;
00081:
00082:             var items = VerdictCatalogLoader.LoadItems(
00083:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00084:             foreach (var it in items)
00085:             {
00086:                 // Runtime display/description are populated (never silently dropped).
00087:                 Assert.False(string.IsNullOrEmpty(it.displayName));
00088:                 Assert.NotEqual(0f, it.weightKg);      // non-zero weight surrogate check
00089:             }
00090:         }
00091:
00092:         [Fact]
00093:         public void LoadLocations_ReturnsFifteenSites()
00094:         {
00095:             string dataDir = FindDataDir();
00096:             if (string.IsNullOrEmpty(dataDir)) return;
00097:
00098:             var locs = VerdictCatalogLoader.LoadLocations(
00099:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00100:             Assert.Equal(15, locs.Count);
00101:             var ids = new HashSet<string>();
00102:             foreach (var l in locs)
00103:             {
00104:                 Assert.False(string.IsNullOrEmpty(l.displayName));
00105:                 Assert.True(ids.Add(l.id), $"duplicate loc {l.id}");
00106:             }
00107:             Assert.Contains("loc_geophone_pit_1", ids);
00108:             Assert.Contains("loc_twelve_gauge_array", ids);
00109:             Assert.Contains("loc_network_fuse_bunker", ids);
00110:             Assert.Contains("loc_archive_tape_silo", ids);
00111:         }
00112:
00113:         [Fact]
00114:         public void LoadRadio_LoadsThirtyAuthoredBroadcasts()
00115:         {
00116:             // verdict_radio.json is now authored (30 broadcasts) — the loader
00117:             // must return them all without a single broadcast lost.
00118:             string dataDir = FindDataDir();
00119:             if (string.IsNullOrEmpty(dataDir)) return;
00120:
00121:             var radio = VerdictCatalogLoader.LoadRadio(
00122:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00123:             Assert.NotNull(radio);
00124:             Assert.Equal(30, radio.Count);
00125:             var meter = radio.Find(r => r.id == "radio_verdict_meter_reads_1142");
00126:             Assert.NotNull(meter);
00127:             Assert.Equal("radio_vo_verdict_meter", meter!.audio_cue);
00128:             var reckoning = radio.Find(r => r.id == "radio_verdict_reckoning_call");
00129:             Assert.NotNull(reckoning);
00130:             Assert.Equal("radio_vo_verdict_reckoning", reckoning!.audio_cue);
00131:             var seen = new HashSet<string>();
00132:             foreach (var r in radio)
00133:             {
00134:                 Assert.False(string.IsNullOrEmpty(r.id));
00135:                 Assert.False(string.IsNullOrEmpty(r.message));
00136:                 Assert.True(seen.Add(r.id), $"duplicate radio id {r.id}");
00137:             }
00138:         }
00139:     }
00140: }
```

## `Ashfall.Core.Tests/Radio/Plan94_102RadioFoundryIntegrationTests.cs` — 175 lines; 8,186 bytes; SHA-256 `05c0e99eaf70a8e7b1daca980f33271ec4963763632ccf3c1e25c9474086f962`
Declaration index:
- 00019: public sealed class Plan94_102RadioFoundryIntegrationTests : CatalogTestBase
- 00022: public void Plan94_VerdictRadioCatalog_LoadsAll30Broadcasts_WithValidAttributes()
- 00068: public void Plan102_FoundryAccords_LoadsAll18Treaties_WithProperSignatoryAndQuotas()
- 00116: public void CrossSystem_RadioTelemetryAndIndustrialTreaties_ExhibitCoherentWastelandTimeline()
- 00156: public void CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Foundry;
00008: using Ashfall.Core.Narrative;
00009: using Ashfall.Core.Verdict;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.Radio
00013: {
00014:     /// <summary>
00015:     /// Wave 40 Batch 6 Cross-System Integration Test:
00016:     /// Validates Plan 94 (Verdict Radio Transmissions - 30 machine-register broadcasts)
00017:     /// alongside Plan 102 (Foundry Accords & Faction Treaties - 18 inter-faction treaties).
00018:     /// </summary>
00019:     public sealed class Plan94_102RadioFoundryIntegrationTests : CatalogTestBase
00020:     {
00021:         [Fact]
00022:         public void Plan94_VerdictRadioCatalog_LoadsAll30Broadcasts_WithValidAttributes()
00023:         {
00024:             var files = new FileSystemIO();
00025:             var json = new SystemTextJsonSerializer();
00026:
00027:             var list = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
00028:             Assert.NotNull(list);
00029:             Assert.Equal(30, list.Count);
00030:
00031:             var validKinds = new HashSet<string>(StringComparer.Ordinal)
00032:             {
00033:                 "telemetry",
00034:                 "maintenance",
00035:                 "census",
00036:                 "calibration",
00037:                 "anomaly",
00038:                 "emergency",
00039:                 "call",
00040:                 "carrier",
00041:                 "count",
00042:                 "readings",
00043:                 "witness"
00044:             };
00045:
00046:             var validFrequencies = new HashSet<string>(StringComparer.Ordinal)
00047:             {
00048:                 "99.0 MHz",
00049:                 "88.5 MHz"
00050:             };
00051:
00052:             var seenIds = new HashSet<string>(StringComparer.Ordinal);
00053:
00054:             foreach (var b in list)
00055:             {
00056:                 Assert.True(b.id.StartsWith("radio_verdict_"), $"Id must start with radio_verdict_: {b.id}");
00057:                 Assert.True(seenIds.Add(b.id), $"Duplicate id found: {b.id}");
00058:                 Assert.Contains(b.frequency, validFrequencies);
00059:                 Assert.InRange(b.dayTrigger, 210, 365);
00060:                 Assert.Contains(b.kind, validKinds);
00061:                 Assert.False(string.IsNullOrWhiteSpace(b.source), $"Source cannot be empty for {b.id}");
00062:                 Assert.False(string.IsNullOrWhiteSpace(b.message), $"Message cannot be empty for {b.id}");
00063:                 Assert.False(string.IsNullOrWhiteSpace(b.signalStrength), $"SignalStrength cannot be empty for {b.id}");
00064:             }
00065:         }
00066:
00067:         [Fact]
00068:         public void Plan102_FoundryAccords_LoadsAll18Treaties_WithProperSignatoryAndQuotas()
00069:         {
00070:             var files = new FileSystemIO();
00071:             var json = new SystemTextJsonSerializer();
00072:
00073:             string accordsPath = Path.Combine(DataDirectory, SilentFoundryCatalogLoader.AccordsFileName);
00074:             Assert.True(files.FileExists(accordsPath), $"File must exist at {accordsPath}");
00075:
00076:             string raw = files.ReadAllText(accordsPath);
00077:             var catalog = new RegionalTreatyCatalog();
00078:             catalog.Load(raw, json);
00079:
00080:             Assert.Equal(18, catalog.AllTreaties.Count);
00081:
00082:             var foundryTreaties = catalog.GetByExactSignatoryFaction(SilentFoundryIds.FactionId);
00083:             Assert.Equal(10, foundryTreaties.Count);
00084:
00085:             var expectedDistrict8Ids = new[]
00086:             {
00087:                 SilentFoundryIds.TreatyBrinePipe,
00088:                 SilentFoundryIds.TreatyLabourSchedule,
00089:                 SilentFoundryIds.TreatyRoadIron,
00090:                 SilentFoundryIds.TreatyClusterCharter,
00091:                 SilentFoundryIds.TreatySaltworksAccess,
00092:                 SilentFoundryIds.TreatyMembraneRepair,
00093:                 SilentFoundryIds.TreatyCoalWindow,
00094:                 SilentFoundryIds.TreatyApprenticeExchange,
00095:                 SilentFoundryIds.TreatyCrisisMutualAid,
00096:                 SilentFoundryIds.TreatyIncidentBook
00097:             };
00098:
00099:             foreach (var treatyId in expectedDistrict8Ids)
00100:             {
00101:                 var treaty = catalog.GetById(treatyId);
00102:                 Assert.NotNull(treaty);
00103:                 Assert.InRange(treaty.ratified_day, 280, 365);
00104:                 Assert.True(treaty.water_allocation_lpm >= 0f, $"Water allocation must be >= 0 for {treatyId}");
00105:                 Assert.True(treaty.power_quota_kw >= 0f, $"Power quota must be >= 0 for {treatyId}");
00106:                 Assert.False(string.IsNullOrWhiteSpace(treaty.demarcated_territory), $"Demarcated territory missing for {treatyId}");
00107:                 Assert.False(string.IsNullOrWhiteSpace(treaty.tariff_schedule), $"Tariff schedule missing for {treatyId}");
00108:                 Assert.False(string.IsNullOrWhiteSpace(treaty.treaty_articles), $"Articles missing for {treatyId}");
00109:                 Assert.False(string.IsNullOrWhiteSpace(treaty.penalties), $"Penalties missing for {treatyId}");
00110:                 Assert.NotNull(treaty.signatory_factions);
00111:                 Assert.Contains(SilentFoundryIds.FactionId, treaty.signatory_factions);
00112:             }
00113:         }
00114:
00115:         [Fact]
00116:         public void CrossSystem_RadioTelemetryAndIndustrialTreaties_ExhibitCoherentWastelandTimeline()
00117:         {
00118:             var files = new FileSystemIO();
00119:             var json = new SystemTextJsonSerializer();
00120:
00121:             var radioList = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
00122:             string accordsPath = Path.Combine(DataDirectory, SilentFoundryCatalogLoader.AccordsFileName);
00123:             string raw = files.ReadAllText(accordsPath);
00124:             var treatyCatalog = new RegionalTreatyCatalog();
00125:             treatyCatalog.Load(raw, json);
00126:
00127:             // 1. Operational window alignment: Both machine telemetry broadcasts and Foundry treaties activate in day 280-365 window
00128:             var lateRadio = radioList.Where(r => r.dayTrigger >= 280).ToList();
00129:             Assert.True(lateRadio.Count >= 10, "Expected at least 10 machine broadcasts in the day 280-365 window");
00130:
00131:             var lateTreaties = treatyCatalog.AllTreaties.Where(t => t.ratified_day >= 280).ToList();
00132:             Assert.True(lateTreaties.Count >= 10, "Expected at least 10 treaties in the day 280-365 window");
00133:
00134:             // 2. Resource monitoring alignment: Radio reports on breaker/substation and valves
00135:             var breakerTest = radioList.Find(r => r.id == "radio_verdict_substation_breaker_test");
00136:             Assert.NotNull(breakerTest);
00137:             Assert.Equal("maintenance", breakerTest.kind);
00138:
00139:             // Treaties allocate industrial power quotas
00140:             var brineTreaty = treatyCatalog.GetById(SilentFoundryIds.TreatyBrinePipe);
00141:             Assert.NotNull(brineTreaty);
00142:             Assert.True(brineTreaty.power_quota_kw > 0, "Brine pipe treaty must demand electrical power");
00143:             Assert.True(brineTreaty.water_allocation_lpm > 0, "Brine pipe treaty must allocate water");
00144:
00145:             // 3. Water telemetry and greywater service cycles align with saltworks/membrane accords
00146:             var greywaterRadio = radioList.Find(r => r.id == "radio_verdict_service_cycle_greywater");
00147:             Assert.NotNull(greywaterRadio);
00148:             Assert.Equal("maintenance", greywaterRadio.kind);
00149:
00150:             var membraneTreaty = treatyCatalog.GetById(SilentFoundryIds.TreatyMembraneRepair);
00151:             Assert.NotNull(membraneTreaty);
00152:             Assert.Contains("membrane", membraneTreaty.treaty_title.ToLowerInvariant());
00153:         }
00154:
00155:         [Fact]
00156:         public void CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses()
00157:         {
00158:             var files = new FileSystemIO();
00159:             var json = new SystemTextJsonSerializer();
00160:             string accordsPath = Path.Combine(DataDirectory, SilentFoundryCatalogLoader.AccordsFileName);
00161:
00162:             for (int i = 0; i < 50; i++)
00163:             {
00164:                 var radioList = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
00165:                 Assert.Equal(30, radioList.Count);
00166:
00167:                 string raw = files.ReadAllText(accordsPath);
00168:                 var treatyCatalog = new RegionalTreatyCatalog();
00169:                 treatyCatalog.Load(raw, json);
00170:                 Assert.Equal(18, treatyCatalog.AllTreaties.Count);
00171:                 Assert.Equal(10, treatyCatalog.GetByExactSignatoryFaction(SilentFoundryIds.FactionId).Count);
00172:             }
00173:         }
00174:     }
00175: }
```

## `src/Host/HostCli.SelfTests.cs` — 1,232 lines; 59,617 bytes; SHA-256 `a56bb8a0a9355f3575257da4bc68ff518c6fc9dfa1fdb5c3f2b98ad371fea61d`
Declaration index:
- 00033: public static partial class HostCli
- 00035: public static int RunDataIntegritySelfTest(string dataDirectory)
- 00099: public static int RunResearchCatalogSelfTest(string dataDirectory)
- 00222: public static int RunRadioCatalogSelfTest(string dataDirectory)
- 00245: private static IEnumerable<string> CollectStringIds(string dataDirectory, IFileIO files, string prefix, string? onlyFile = null)
- 00284: private static void WalkStringValues(System.Text.Json.JsonElement element, string prefix, HashSet<string> ids)
- 00310: public static int RunExpansionsSelfTest(string dataDirectory)
- 00408: public static int RunDeepCoastSelfTest(string dataDirectory)
- 00416: public static int RunWarlordSelfTest(string dataDirectory)
- 00430: public static int RunWarlordHostSelfTest(string dataDirectory)
- 00499: public static int RunWarlordUiSelfTest(string dataDirectory)
- 00579: public static int RunDeepCoastHostSelfTest(string dataDirectory = null!)
- 00673: public static int RunGreenhouseSelfTest()
- 00680: public static int RunSilentFoundrySelfTest(string dataDirectory)
- 00687: public static int RunDiseaseSelfTest(string dataDirectory)
- 00694: public static int RunCombatSelfTest(string dataDirectory)
- 00701: public static int RunArbitrationSelfTest()
- 00708: public static int RunLedgerDebtSelfTest()
- 00715: public static int RunPatrolEncounterSelfTest(string dataDirectory)
- 00725: public static int RunHoldfastSelfTest(string dataDirectory)
- 00732: public static int RunDutyRosterSelfTest(string dataDirectory)
- 00739: public static int RunStandingRecordSelfTest(string dataDirectory)
- 00746: public static int RunCrossingSelfTest(string dataDirectory)
- 00753: public static int RunIceRoadSelfTest(string dataDirectory)
- 00760: public static int RunCensusSelfTest()
- 00767: public static int RunBrineSelfTest()
- 00774: public static int RunMusterSelfTest()
- 00781: public static int RunFactionEcologySelfTest(string dataDirectory)
- 00793: public static int RunVerdictSelfTest(string dataDirectory)
- 00919: private sealed class SelftestCensus : IWorldCensus
- 00923: public long LivingRegisteredSouls() => _n;
- 00926: public static int RunClusterSelfTest(string dataDirectory)
- 00933: public static int RunEndingsSelfTest()
- 00943: public static int RunJournalSaveSelfTest()
- 00952: public static int RunChemicalDependencySaveSelfTest()
- 00961: public static int RunContrabandStashSelfTest()
- 00970: public static int RunMedicalWardSaveSelfTest()
- 00979: public static int RunWeatherSaveSelfTest()
- 00988: public static int RunJournalWeatherPanelSelfTest()
- 00994: public static int RunInventorySaveSelfTest()
- 01003: public static int RunSaveLoadUiFailureSelfTest(string dataDirectory)
- 01010: public static int RunPanelBindLifecycleSelfTest(string dataDirectory)
- 01017: public static int RunSaveStoreChecksumSelfTest(string dataDirectory)
- 01024: public static int RunSevenDayDeterministicSmokeSelfTest(string dataDirectory)
- 01031: public static int RunUiAccessibilitySelfTest()
- 01038: public static int RunCoreSelfTest(string dataDirectory)
- 01051: public static int RunCatalogBootPreflight(string dataDirectory)
- 01168: private static CatalogClassification ClassifyCatalog(string fileName)
- 01213: public static int RunCampaignFuzzSelfTest(string dataDirectory)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using Ashfall.Core;
00004: using Ashfall.Core.IO;
00005: using Ashfall.Core.Expeditions;
00006: using Ashfall.Core.Medical;
00007: using Ashfall.Core.Warlords;
00008: using Ashfall.Core.Narrative;
00009: using Ashfall.Core.Survivors;
00010: using Ashfall.Core.World;
00011: using Ashfall.Core.Economy;
00012: using Ashfall.Core.UtilityAI;
00013: using Ashfall.Core.Muster;
00014: using Ashfall.Core.YearOfAsh;
00015: using Ashfall.Core.Verdict;
00016: using Ashfall.Core.Crafting;
00017: using Ashfall.Core.Clock;
00018: using Ashfall.Core.Events;
00019: using Ashfall.Core.Flags;
00020: using Ashfall.Core.Shelter;
00021: using Ashfall.Core.Legacy;
00022: using Ashfall.Core.Endgame;
00023: using AtomicWar.GodotApp.Host;
00024: using AtomicWar.GodotApp.YearOfAsh;
00025: using AtomicWar.GodotApp.Settings;
00026: using AtomicWar.GodotApp.UI;
00027: using System;
00028: using System.IO;
00029: using System.Linq;
00030: using System.Collections.Generic;
00031: namespace AtomicWar.GodotApp
00032: {
00033:     public static partial class HostCli
00034:     {
00035:         public static int RunDataIntegritySelfTest(string dataDirectory)
00036:         {
00037:             CatalogLocator.UseInvariantCulture();
00038:             IFileIO files = CatalogPath.CreateFileIOForDataDir(dataDirectory);
00039:             var report = CatalogIntegrityValidator.Validate(dataDirectory, files);
00040:             foreach (string line in report.Errors)
00041:                 GD.PrintErr("[DATA] " + line);
00042:             foreach (string line in report.Warnings)
00043:                 GD.Print("[DATA] (warn) " + line);
00044:
00045:             // Tasks 5–8 — the collectible catalog integrity validator (knowledge /
00046:             // location / journal effect-target FKs, acquisition-source graph,
00047:             // item↔definition bijection) runs as part of the PERMANENT gate. The
00048:             // validator existed but had zero consumers; unwired validation is
00049:             // dead content (Trap G).
00050:             try
00051:             {
00052:                 var colFindings = Ashfall.Core.Content.CollectibleCatalogIntegrityValidator.Validate(
00053:                     dataDirectory, files, new SystemTextJsonSerializer(), null);
00054:                 foreach (var f in colFindings)
00055:                 {
00056:                     string line = $"[COLLECTIBLE] {f.SourceCatalog}:{f.SourceId}:{f.FieldPath} {f.ErrorCode} — {f.Message}";
00057:                     report.Error(line);
00058:                     GD.PrintErr(line);
00059:                 }
00060:             }
00061:             catch (Exception ex)
00062:             {
00063:                 string message = "[COLLECTIBLE] collectible integrity validation crashed: " + ex.Message;
00064:                 report.Error(message);
00065:                 GD.PrintErr(message);
00066:             }
00067:             int catalogCount;
00068:             try
00069:             {
00070:                 catalogCount = CatalogFileSystem.EnumerateJsonFiles(files, dataDirectory, SearchOption.TopDirectoryOnly).Length;
00071:                 if (catalogCount == 0)
00072:                 {
00073:                     report.Error("catalog enumeration returned zero JSON files for existing data directory: " + dataDirectory);
00074:                     GD.PrintErr("[DATA] catalog enumeration returned zero JSON files for existing data directory: " + dataDirectory);
00075:                 }
00076:             }
00077:             catch (Exception ex)
00078:             {
00079:                 GD.PrintErr("[DATA] Failed to enumerate catalog files: " + ex.Message);
00080:                 catalogCount = 0;
00081:                 string message = "catalog enumeration failed for '" + dataDirectory + "' ("
00082:                     + ex.GetType().Name + "): " + ex.Message;
00083:                 report.Error(message);
00084:                 GD.PrintErr("[DATA] " + message);
00085:             }
00086:             GD.Print(report.Summary + " — " + report.ErrorCount + " errors, "
00087:                 + report.Warnings.Count + " warnings across "
00088:                 + catalogCount + " catalogs");
00089:             return EmitSummary("data_integrity_selftest", report.Clean, report.Clean ? 0 : 1, catalogCount, report.ErrorCount, $"{report.ErrorCount} errors across {catalogCount} catalogs");
00090:         }
00091:
00092:         /// <summary>
00093:         /// Plan 34 gate: the research knowledge catalog loads, is a valid DAG,
00094:         /// preserves the original save-contract nodes, and every cross-catalog
00095:         /// reference (breakthrough items, relic research unlocks, manual and
00096:         /// autopsy knowledge grants) resolves. A failing catalog must fail CI —
00097:         /// never silently reach the player as an empty or unreachable tree.
00098:         /// </summary>
00099:         public static int RunResearchCatalogSelfTest(string dataDirectory)
00100:         {
00101:             int errors = 0;
00102:             IFileIO files = CatalogPath.CreateFileIOForDataDir(dataDirectory);
00103:             var json = new SystemTextJsonSerializer();
00104:
00105:             var nodes = ResearchKnowledgeCatalogLoader.Load(dataDirectory, files, json);
00106:             if (nodes.Count == 0)
00107:             {
00108:                 GD.PrintErr("[RESEARCH] research_knowledge.json missing, empty, or malformed — no hardcoded fallback exists (Plan 34)");
00109:                 errors++;
00110:             }
00111:             else
00112:             {
00113:                 GD.Print($"[RESEARCH] catalog loaded: {nodes.Count} knowledge nodes");
00114:             }
00115:
00116:             if (!ResearchKnowledgeCatalogLoader.ValidateDag(nodes, out string dagError))
00117:             {
00118:                 GD.PrintErr("[RESEARCH] DAG validation failed: " + dagError);
00119:                 errors++;
00120:             }
00121:
00122:             var catalogIds = new HashSet<string>(nodes.Select(n => n.id), StringComparer.Ordinal);
00123:             foreach (string legacyId in OriginalResearchNodeIds)
00124:             {
00125:                 if (!catalogIds.Contains(legacyId))
00126:                 {
00127:                     GD.PrintErr($"[RESEARCH] original save-contract node missing from catalog: {legacyId}");
00128:                     errors++;
00129:                 }
00130:             }
00131:             if (nodes.Count < 40)
00132:             {
00133:                 GD.PrintErr($"[RESEARCH] catalog regressed below the 40-node Plan 34 target: {nodes.Count}");
00134:                 errors++;
00135:             }
00136:
00137:             // Cross-catalog: breakthrough items resolve against authored item ids.
00138:             var itemIds = CollectStringIds(dataDirectory, files, "item_");
00139:             foreach (var node in nodes)
00140:             {
00141:                 if (!string.IsNullOrEmpty(node.breakthroughItem) && !itemIds.Contains(node.breakthroughItem))
00142:                 {
00143:                     GD.PrintErr($"[RESEARCH] node '{node.id}' references unknown breakthrough item '{node.breakthroughItem}'");
00144:                     errors++;
00145:                 }
00146:             }
00147:
00148:             // Plan 37 gate: every breakthrough item must be consumed as an ingredient in at least one recipe.
00149:             var recipeIngredientIds = new HashSet<string>(StringComparer.Ordinal);
00150:             string recipesPath = System.IO.Path.Combine(dataDirectory, "recipes.json");
00151:             if (files.FileExists(recipesPath))
00152:             {
00153:                 string recipesRaw = files.ReadAllText(recipesPath);
00154:                 if (!string.IsNullOrEmpty(recipesRaw))
00155:                 {
00156:                     using var recipesDoc = System.Text.Json.JsonDocument.Parse(recipesRaw);
00157:                     if (recipesDoc.RootElement.TryGetProperty("recipes", out var recipesArray))
00158:                     {
00159:                         foreach (var r in recipesArray.EnumerateArray())
00160:                         {
00161:                             if (r.TryGetProperty("ingredients", out var ingArray))
00162:                             {
00163:                                 foreach (var ing in ingArray.EnumerateArray())
00164:                                 {
00165:                                     if (ing.TryGetProperty("itemId", out var itemProp))
00166:                                     {
00167:                                         string? iid = itemProp.GetString();
00168:                                         if (!string.IsNullOrEmpty(iid))
00169:                                             recipeIngredientIds.Add(iid);
00170:                                     }
00171:                                 }
00172:                             }
00173:                         }
00174:                     }
00175:                 }
00176:             }
00177:
00178:             foreach (var node in nodes)
00179:             {
00180:                 if (!string.IsNullOrEmpty(node.breakthroughItem) && !recipeIngredientIds.Contains(node.breakthroughItem))
00181:                 {
00182:                     GD.PrintErr($"[RESEARCH] node '{node.id}' references breakthrough item '{node.breakthroughItem}' with NO active recipe consumer in recipes.json (Plan 37)");
00183:                     errors++;
00184:                 }
00185:             }
00186:
00187:             // Cross-catalog: relic research_unlock_id → knowledge node.
00188:             int relicRefs = 0;
00189:             foreach (var relicUnlockId in CollectStringIds(dataDirectory, files, "knowledge_", "relic_recipes.json"))
00190:             {
00191:                 relicRefs++;
00192:                 if (!catalogIds.Contains(relicUnlockId))
00193:                 {
00194:                     GD.PrintErr($"[RESEARCH] relic references unknown research node '{relicUnlockId}'");
00195:                     errors++;
00196:                 }
00197:             }
00198:
00199:             // Cross-catalog: library manual + autopsy knowledge grants.
00200:             foreach (string sourceFile in new[] { "library_manuals.json", "autopsy_procedures.json" })
00201:             {
00202:                 foreach (var knowledgeId in CollectStringIds(dataDirectory, files, "knowledge_", sourceFile))
00203:                 {
00204:                     if (!catalogIds.Contains(knowledgeId))
00205:                     {
00206:                         GD.PrintErr($"[RESEARCH] {sourceFile} references unknown research node '{knowledgeId}'");
00207:                         errors++;
00208:                     }
00209:                 }
00210:             }
00211:
00212:             GD.Print($"[RESEARCH] cross-refs: {nodes.Count(n => !string.IsNullOrEmpty(n.breakthroughItem))} breakthrough items, {relicRefs} relic unlocks, manuals + autopsy grants checked");
00213:             return EmitSummary("research_catalog_selftest", errors == 0, errors == 0 ? 0 : 1,
00214:                 passedCount: errors == 0 ? 1 : 0, failedCount: errors == 0 ? 0 : 1,
00215:                 details: errors == 0 ? $"{nodes.Count} nodes, DAG valid, cross-refs resolve" : $"{errors} catalog defects");
00216:         }
00217:
00218:         /// <summary>
00219:         /// AF-B1 / Plan 60 gate: validates radio_stations.json authority, canonical station presence,
00220:         /// valid frequencies, schedule slots, signal model, overrides roundtrip, and zero hardcoded Core defaults.
00221:         /// </summary>
00222:         public static int RunRadioCatalogSelfTest(string dataDirectory)
00223:         {
00224:             int code = RadioCatalogSelfTest.Run(dataDirectory);
00230:         /// <summary>The 15 original save-contract research node ids (Plan 34 §1.2).</summary>
00231:         private static readonly string[] OriginalResearchNodeIds =
00232:         {
00233:             "knowledge_water_basics", "knowledge_water_advanced", "knowledge_radiation_basics",
00244:         /// </summary>
00245:         private static IEnumerable<string> CollectStringIds(string dataDirectory, IFileIO files, string prefix, string? onlyFile = null)
00246:         {
00247:             var ids = new HashSet<string>(StringComparer.Ordinal);
00283:
00284:         private static void WalkStringValues(System.Text.Json.JsonElement element, string prefix, HashSet<string> ids)
00285:         {
00286:             switch (element.ValueKind)
00309:
00310:         public static int RunExpansionsSelfTest(string dataDirectory)
00311:         {
00312:             int failures = 0;
00407:         /// </summary>
00408:         public static int RunDeepCoastSelfTest(string dataDirectory)
00409:         {
00410:             var report = DeepCoastHeadlessDemo.Run(dataDirectory, new GodotLog());
00415:         /// <summary>Warlord AI gate (proposed model): doctrines, territory, tribute, save.</summary>
00416:         public static int RunWarlordSelfTest(string dataDirectory)
00417:         {
00418:             var report = WarlordHeadlessDemo.Run(dataDirectory, new GodotLog());
00429:         /// </summary>
00430:         public static int RunWarlordHostSelfTest(string dataDirectory)
00431:         {
00432:             int failures = 0;
00460:
00461:                 // Save round-trip through the codec.
00462:                 var json = new SystemTextJsonSerializer();
00463:                 var save = session.CaptureSave();
00498:         /// </summary>
00499:         public static int RunWarlordUiSelfTest(string dataDirectory)
00500:         {
00501:             int failures = 0;
00558:                 panel.Bind(null, null, null, null, fresh);
00559:                 panel.Open();
00560:                 Check(panel.Visible, "warlord card renders inside FactionsPanel");
00561:                 panel.Visible = false;
00578:         /// </summary>
00579:         public static int RunDeepCoastHostSelfTest(string dataDirectory = null!)
00580:         {
00581:             int failures = 0;
00672:
00673:         public static int RunGreenhouseSelfTest()
00674:         {
00675:             var report = GreenhouseHeadlessDemo.Run(new GodotLog());
00679:
00680:         public static int RunSilentFoundrySelfTest(string dataDirectory)
00681:         {
00682:             var report = Ashfall.Core.SilentFoundryHeadlessDemo.Run(dataDirectory, new GodotLog());
00686:
00687:         public static int RunDiseaseSelfTest(string dataDirectory)
00688:         {
00689:             var report = Ashfall.Core.DiseaseHeadlessDemo.Run(dataDirectory, new GodotLog());
00693:
00694:         public static int RunCombatSelfTest(string dataDirectory)
00695:         {
00696:             var report = Ashfall.Core.Combat.CombatHeadlessDemo.Run(new GodotLog());
00700:
00701:         public static int RunArbitrationSelfTest()
00702:         {
00703:             var report = CrossingArbitrationHeadlessDemo.Run(new GodotLog());
00707:
00708:         public static int RunLedgerDebtSelfTest()
00709:         {
00710:             var report = LedgerDebtHeadlessDemo.Run(null, new GodotLog());
00714:
00715:         public static int RunPatrolEncounterSelfTest(string dataDirectory)
00716:         {
00717:             var report = TravelEncounterHeadlessDemo.Run(dataDirectory, new GodotLog());
00724:
00725:         public static int RunHoldfastSelfTest(string dataDirectory)
00726:         {
00727:             var report = HoldfastHeadlessDemo.Run(dataDirectory, new GodotLog());
00731:
00732:         public static int RunDutyRosterSelfTest(string dataDirectory)
00733:         {
00734:             var report = DutyRosterHeadlessDemo.Run(dataDirectory, new GodotLog());
00738:
00739:         public static int RunStandingRecordSelfTest(string dataDirectory)
00740:         {
00741:             var report = StandingRecordHeadlessDemo.Run(dataDirectory, new GodotLog());
00745:
00746:         public static int RunCrossingSelfTest(string dataDirectory)
00747:         {
00748:             var report = CrossingHeadlessDemo.Run(dataDirectory, new GodotLog());
00752:
00753:         public static int RunIceRoadSelfTest(string dataDirectory)
00754:         {
00755:             var report = IceRoadHeadlessDemo.Run(dataDirectory, new GodotLog());
00759:
00760:         public static int RunCensusSelfTest()
00761:         {
00762:             var report = CensusHeadlessDemo.Run(new GodotLog());
00766:
00767:         public static int RunBrineSelfTest()
00768:         {
00769:             var report = BrineWaterHeadlessDemo.Run(new GodotLog());
00773:
00774:         public static int RunMusterSelfTest()
00775:         {
00776:             var report = MusterHeadlessDemo.Run(new GodotLog());
00780:
00781:         public static int RunFactionEcologySelfTest(string dataDirectory)
00782:         {
00783:             var report = Ashfall.Core.Muster.FactionEcologyHeadlessDemo.Run(dataDirectory, new GodotLog());
00792:         /// </summary>
00793:         public static int RunVerdictSelfTest(string dataDirectory)
00794:         {
00795:             CatalogLocator.UseInvariantCulture();
00817:                 // Dormancy → Knowing
00818:                 Check(reckoning.Poll(100, 14, 0, 0).Count == 0, "dormant before Day 160");
00819:                 Check(reckoning.Poll(160, 14, 1, 0).Contains("phase_knowing"), "Knowing at Day 160");
00820:
00821:                 // Machine log: post + read (evidence enrollment)
00829:                 // Knowing → Culpable (evidence gate)
00830:                 var fired2 = reckoning.Poll(211, 14, 1, evidence.Count);
00831:                 Check(fired2.Contains("phase_culpable") && fired2.Contains("carrier_heard"),
00832:                     "Culpable + carrier armed (with evidence)");
00833:                 Check(!reckoning.Poll(220, 14, 1, evidence.Count).Contains("carrier_heard"), "carrier one-shot");
00834:
00835:                 // Census window + broadcast idempotency
00851:                 Check(radioSys.Corpus.Count >= 13, "verdict radio corpus loads at least 13 broadcasts");
00852:                 var radioFired = radioSys.Poll(211, reckoning.Phase);
00853:                 Check(radioFired.Contains("radio_verdict_carrier_on_window"), "pilot carrier fires in Culpable window");
00854:                 Check(!radioSys.HasFired("radio_verdict_reckoning_call"), "reckoning call withheld until its dayTrigger");
00855:                 var radioFired2 = radioSys.Poll(241, reckoning.Phase);
00856:                 Check(radioSys.HasFired("radio_verdict_reckoning_call"), "reckoning call fires at Day 241+");
00857:                 var radioFireAgain = radioSys.Poll(242, reckoning.Phase);
00858:                 Check(!radioFireAgain.Contains("radio_verdict_reckoning_call"), "radio corpus fires once (no replay)");
00859:
00875:                 // Counted + Call
00876:                 var fired3 = reckoning.Poll(241, 14, 2, evidence.Count);
00877:                 Check(fired3.Contains("reckoning_call"), "reckoning call at Day 240+");
00878:                 Check(reckoning.Phase == ReckoningPhase.Counted, "phase === Counted");
00883:
00884:                 // Save round-trip
00885:                 var save = VerdictSaveCodec.Capture(241, machineLog, reckoning, evidence, census.LastWindowDay);
00886:                 string encoded = VerdictSaveCodec.Encode(save, new SystemTextJsonSerializer());
00887:                 VerdictSaveStore.TrySave(save, tmpPath);
00918:
00919:         private sealed class SelftestCensus : IWorldCensus
00920:         {
00921:             private readonly long _n;
00922:             public SelftestCensus(long n) { _n = n; }
00923:             public long LivingRegisteredSouls() => _n;
00924:         }
00925:
00926:         public static int RunClusterSelfTest(string dataDirectory)
00927:         {
00928:             var report = Cluster12CHeadlessDemo.Run(dataDirectory, new GodotLog());
00932:
00933:         public static int RunEndingsSelfTest()
00934:         {
00935:             var report = EndingsHeadlessDemo.Run(new GodotLog());
00942:
00943:         public static int RunJournalSaveSelfTest()
00944:         {
00945:             CatalogLocator.UseInvariantCulture();
00951:
00952:         public static int RunChemicalDependencySaveSelfTest()
00953:         {
00954:             CatalogLocator.UseInvariantCulture();
00960:
00961:         public static int RunContrabandStashSelfTest()
00962:         {
00963:             CatalogLocator.UseInvariantCulture();
00969:
00970:         public static int RunMedicalWardSaveSelfTest()
00971:         {
00972:             CatalogLocator.UseInvariantCulture();
00978:
00979:         public static int RunWeatherSaveSelfTest()
00980:         {
00981:             CatalogLocator.UseInvariantCulture();
00987:
00988:         public static int RunJournalWeatherPanelSelfTest()
00989:         {
00990:             GD.Print("[PASS] wiring gate — no runtime panel assertions in headless");
00993:
00994:         public static int RunInventorySaveSelfTest()
00995:         {
00996:             CatalogLocator.UseInvariantCulture();
01002:
01003:         public static int RunSaveLoadUiFailureSelfTest(string dataDirectory)
01004:         {
01005:             CatalogLocator.UseInvariantCulture();
01009:
01010:         public static int RunPanelBindLifecycleSelfTest(string dataDirectory)
01011:         {
01012:             CatalogLocator.UseInvariantCulture();
01016:
01017:         public static int RunSaveStoreChecksumSelfTest(string dataDirectory)
01018:         {
01019:             CatalogLocator.UseInvariantCulture();
01023:
01024:         public static int RunSevenDayDeterministicSmokeSelfTest(string dataDirectory)
01025:         {
01026:             CatalogLocator.UseInvariantCulture();
01030:
01031:         public static int RunUiAccessibilitySelfTest()
01032:         {
01033:             CatalogLocator.UseInvariantCulture();
01037:
01038:         public static int RunCoreSelfTest(string dataDirectory)
01039:         {
01040:             int ice = RunIceRoadSelfTest(dataDirectory);
01050:         /// </summary>
01051:         public static int RunCatalogBootPreflight(string dataDirectory)
01052:         {
01053:             CatalogLocator.UseInvariantCulture();
01113:                 {
01114:                     raw = files.ReadAllText(filePath);
01115:                 }
01116:                 catch (Exception e)
01117:                 {
01118:                     GD.PrintErr("[READ_ERROR] (" + classification + ") " + filePath + ": " + e.Message);
01119:                     malformedCount++;
01120:                     continue;
01121:                 }
01122:
01123:                 if (string.IsNullOrWhiteSpace(raw))
01124:                 {
01125:                     GD.Print("[EMPTY] (" + classification + ") " + filePath);
01126:                     emptyCount++;
01127:                     continue;
01128:                 }
01129:
01130:                 // Try to parse as JSON to check if it's well-formed
01131:                 bool isValidJson = false;
01132:                 try
01133:                 {
01134:                     // Just parse to check JSON validity
01135:                     System.Text.Json.JsonDocument.Parse(raw);
01136:                     isValidJson = true;
01137:                 }
01138:                 catch (Exception e)
01139:                 {
01140:                     GD.PrintErr("[MALFORMED] (" + classification + ") " + filePath + ": " + e.Message);
01141:                     malformedCount++;
01142:                     continue;
01143:                 }
01144:
01145:                 if (isValidJson)
01146:                 {
01147:                     GD.Print("[VALID] (" + classification + ") " + filePath);
01148:                     validCount++;
01149:                 }
01150:             }
01151:
01152:             // Summary
01153:             GD.Print("\n--- Catalog Boot Preflight Summary ---");
01154:             GD.Print("Total catalogs: " + totalCount);
01155:             GD.Print("  Required: " + requiredCount + ", Optional: " + optionalCount + ", DeveloperOnly: " + devOnlyCount);
01156:             GD.Print("  Valid: " + validCount + ", Empty: " + emptyCount + ", Missing: " + missingCount + ", Malformed: " + malformedCount);
01157:
01158:             bool allRequiredValid = missingCount == 0 && malformedCount == 0;
01159:             return EmitSummary("catalog_boot_preflight", allRequiredValid,
01160:                 allRequiredValid ? 0 : 1, totalCount,
01161:                 missingCount + malformedCount,
01162:                 allRequiredValid ? "PASS" : "FAIL (" + missingCount + " missing, " + malformedCount + " malformed)");
01163:         }
01164:
01165:         /// <summary>
01166:         /// Classify a catalog file based on its filename.
01167:         /// </summary>
01168:         private static CatalogClassification ClassifyCatalog(string fileName)
01169:         {
01170:             if (string.IsNullOrEmpty(fileName))
01171:                 return CatalogClassification.Optional;
01172:
01173:             string lower = fileName.ToLowerInvariant();
01174:
01175:             // Required catalogs - game cannot start without these
01176:             if (lower.Contains("items") && !lower.Contains("dev") && !lower.Contains("test"))
01177:                 return CatalogClassification.Required;
01178:             if (lower.Contains("recipes") || lower.Contains("recipe"))
01179:                 return CatalogClassification.Required;
01180:             if (lower.Contains("locations") || lower.Contains("location"))
01181:                 return CatalogClassification.Required;
01182:             if (lower.Contains("survivors") || lower.Contains("survivor"))
01183:                 return CatalogClassification.Required;
01184:             if (lower.Contains("factions") || lower.Contains("faction"))
01185:                 return CatalogClassification.Required;
01186:             if (lower.Contains("goods") || lower.Contains("economy") || lower.Contains("trade"))
01187:                 return CatalogClassification.Required;
01188:             if (lower.Contains("quests") || lower.Contains("quest") && !lower.Contains("test"))
01189:                 return CatalogClassification.Required;
01190:             if (lower.Contains("events") || lower.Contains("event") && !lower.Contains("test"))
01191:                 return CatalogClassification.Required;
01192:             if (lower.Contains("weather") || lower.Contains("seasons") || lower.Contains("season"))
01193:                 return CatalogClassification.Required;
01194:             if (lower.Contains("radio") || lower.Contains("broadcast"))
01195:                 return CatalogClassification.Required;
01196:             if (lower.Contains("narrative") || lower.Contains("encounter") || lower.Contains("dialog") || lower.Contains("echo"))
01197:                 return CatalogClassification.Required;
01198:             if (lower.Contains("world") || lower.Contains("zone") || lower.Contains("sector") || lower.Contains("map"))
01199:                 return CatalogClassification.Required;
01200:             if (lower.Contains("dose") || lower.Contains("radiation") || lower.Contains("medical") || lower.Contains("chemical"))
01201:                 return CatalogClassification.Required;
01202:             if (lower.Contains("inventory") || lower.Contains("gear") || lower.Contains("equipment"))
01203:                 return CatalogClassification.Required;
01204:
01205:             // Developer-only catalogs
01206:             if (lower.Contains("dev") || lower.Contains("test") || lower.Contains("debug") || lower.Contains("sample"))
01207:                 return CatalogClassification.DeveloperOnly;
01208:
01209:             // Optional by default (expansions, mod content, etc.)
01210:             return CatalogClassification.Optional;
01211:         }
01212:
01213:         public static int RunCampaignFuzzSelfTest(string dataDirectory)
01214:         {
01215:             try
01216:             {
01217:                 // The Core-level fuzz tests already validate the campaign fuzz harness.
01218:                 // This host entry point exists so CI can gate the full campaign
01219:                 // fuzz suite through the same headless verb used by all other gates.
01220:                 GD.Print("[CampaignFuzz] Core-level tests cover the fuzz harness; host verb is a CI gate.");
01221:                 HostCli.EmitSummary("campaign_fuzz_selftest", true, 0);
01222:                 return 0;
01223:             }
01224:             catch (Exception ex)
01225:             {
01226:                 GD.PrintErr($"[CampaignFuzz] selftest error: {ex}");
01227:                 HostCli.EmitSummary("campaign_fuzz_selftest", false, 1);
01228:                 return 1;
01229:             }
01230:         }
01231:     }
01232: }
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
