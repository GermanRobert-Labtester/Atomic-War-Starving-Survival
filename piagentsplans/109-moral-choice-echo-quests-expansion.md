# Plan 109 — Moral Choice Echoes: Sixty-Quest Chain Corpus, Exactly-Once Delivery, and Existing Quest Ownership

> **Rebuild status:** TERMINAL 60-ECHO CONTENT + REACHABILITY/PERSISTENCE AUDIT
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

The historical baseline was 4,842 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Keep the sixty authored moral-choice echo definitions while separating the pure chain catalog, `MoralChoiceSystem` resolution state, the canonical quest owner, and the current campaign day/host event surfaces. The historical 32→60 data expansion is complete; the high-value residual is proving whether and how a live echo is surfaced exactly once, without creating a second quest lifecycle.

**Bounded outcome:** Audit chain data/loader, `MoralChoiceSystem.FindAvailableEchoQuests`/`MarkEchoQuestFired`, `Main.MoralChoice`, campaign day owners, save state, quest/panel consumers, gossip/reaction consumers, and focused tests. A future package may wire existing echo definitions to an existing quest presentation only after that route is proven.

**Non-goals:** no new quest lifecycle, no second moral score, no duplicate fired ledger, no arbitrary quest count, no sealed distress-signal content changes, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `moral_choice_chains.json` contains 4 branches, 88 quest gates, and 60 echo quests.
- VERIFIED CURRENT: `MoralChoiceSystem` has explicit echo eligibility and fired-id methods.
- VERIFIED CURRENT: moral choice state persists through the existing `moral_choice` section.
- The current production caller trace for `FindAvailableEchoQuests` is an explicit premise question, not an assumed fact.
- HISTORICAL RECORD: Plan 109/Wave 41 records the 32→60 expansion; this package does not claim a fresh test run.

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

- `Assets/StreamingAssets/Data/moral_choice_chains.json` exists at 31,788 bytes; SHA-256 `2a30c42686baef9f1e64cdda74fdb13cbe9204c641790d47212e5aeb48fc2135`.
- `Assets/StreamingAssets/Data/moral_choice_quests.json` exists at 141,249 bytes; SHA-256 `1c84bf9e37036b9247ba0f48e1a14a8d1b497a0bcc63e0e506f23f8bb399b4e1`.
- `Assets/StreamingAssets/Data/moral_choice_flags.json` exists at 2,090 bytes; SHA-256 `e5a95235ce28f9d2a1c9c8bcaeb72789122d9d77e1d38b0658cc86ae4d6a4db4`.
- `Assets/StreamingAssets/Data/moral_choice_gossip.json` exists at 27,959 bytes; SHA-256 `fabce75419bbfe57e5637338851bd0418a1e3056f49cfe39b61858df4361c0f8`.
- `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json` exists at 14,988 bytes; SHA-256 `fd9f4c31b6dd1c9779bc820d7b02923901257b704f930c3572e6d2d57933950d`.

# 3. Required Delta

Replace the old pure-data brief with a current 60-echo census and a production reachability/persistence audit. Preserve the moral-choice owner and use the canonical quest lifecycle for any future presentation.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Does current production code call `FindAvailableEchoQuests` and `MarkEchoQuestFired`?
Which existing quest/encounter owner can present an echo definition without creating a second lifecycle?
Are all 60 triggered_by/choice references valid against current quest ids and indices?
Can fired ids and branch locks survive moral_choice save/load without duplicate callbacks?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| echo definitions, branches, and gates | `MoralChoiceChainCatalogLoader / MoralChoiceChainData` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainData.cs; Assets/StreamingAssets/Data/moral_choice_chains.json` | Authored chain metadata; not a second quest runtime. |
| resolution, echo eligibility, and fired ids | `MoralChoiceSystem` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` | Owns chain state and exactly-once eligibility. |
| host setup, choice commands, journaling, and gossip seed | `Main.MoralChoice / MoralChoiceSaveStore` | `src/Main.MoralChoice.cs; src/Host/MoralChoiceSaveStore.cs` | Composes the current owner and existing `moral_choice` section. |
| quest lifecycle/presentation | `existing quest/narrative owners` | `Assets/Ashfall.Core/Quests/; src/UI/QuestDetailPanel.cs` | Echo presentation must use the existing quest owner, not a new lifecycle. |
| delayed consequences | `Campaign day/echo consumers and canonical consequence owners` | `src/Main.CampaignOwners.cs; current journal/rumor owners` | Each consequence routes through its own owner. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for moral-choice echo corpus.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Moral Choice Echoes: Sixty-Quest Chain Corpus, Exactly-Once Delivery, and Existing Quest Ownership                                               │
│ Integration route: DATA-ONLY + current moral/quest reachability audit                             │
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

1. **Catalog defines echoes.**
2. **MoralChoiceSystem owns eligibility/fired state.**
3. **Quest owner owns presentation/lifecycle.**
4. **Consumers own their consequences.**
5. **The existing moral_choice section persists the decision spine.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| echo definitions, branches, and gates | `MoralChoiceChainCatalogLoader / MoralChoiceChainData` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainData.cs; Assets/StreamingAssets/Data/moral_choice_chains.json` | Authored chain metadata; not a second quest runtime. |
| resolution, echo eligibility, and fired ids | `MoralChoiceSystem` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` | Owns chain state and exactly-once eligibility. |
| host setup, choice commands, journaling, and gossip seed | `Main.MoralChoice / MoralChoiceSaveStore` | `src/Main.MoralChoice.cs; src/Host/MoralChoiceSaveStore.cs` | Composes the current owner and existing `moral_choice` section. |
| quest lifecycle/presentation | `existing quest/narrative owners` | `Assets/Ashfall.Core/Quests/; src/UI/QuestDetailPanel.cs` | Echo presentation must use the existing quest owner, not a new lifecycle. |
| delayed consequences | `Campaign day/echo consumers and canonical consequence owners` | `src/Main.CampaignOwners.cs; current journal/rumor owners` | Each consequence routes through its own owner. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load branches, gates, and sixty echo definitions through the chain loader
2. resolve a source moral choice through MoralChoiceSystem
3. query FindAvailableEchoQuests only after the minimum delay and branch gate
4. present through an existing quest/encounter owner if a current route is proven
5. mark the echo fired once after presentation/acceptance commits
6. journal/gossip/react through existing consumers and persist moral_choice state

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- every echo id is unique and every triggered_by resolves to a real source quest
- choice-index matching is explicit and never guessed
- branch locks suppress incompatible echoes
- fired ids are persisted and monotonic
- a reload cannot present the same echo twice
- a missing presentation route does not mark an echo fired
- choice outcomes and moral scores remain owned by MoralChoiceSystem

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

Contract rules for moral-choice echo corpus:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`moral_choice_chains.json` remains the chain authority with four branches, 88 gates, and 60 echo rows. Audit each echo’s `triggered_by`, `triggered_by_choice`, delay, and branch against current quest definitions. The data catalog is not evidence that a live echo is currently surfaced; production caller tracing is required.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

Use the existing `moral_choice` section and `MoralChoiceSaveStore` for resolutions, scores, fired echo ids, and flags. Do not add an echo-specific save section. A future quest presentation must preserve fired ids across restore and use the existing quest owner’s persistence where applicable.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

MoralChoiceSystem receives `ISeededRng` for current offer/choice behavior; echo eligibility iterates catalog order and uses persisted resolution day/choice. No dictionary/hash order may decide the first echo. Paired replay compares eligible ids, fired ids, branch locks, choices, flags, and emitted facts.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

Choice resolution emits current moral facts. Echo eligibility is a query; presentation/acceptance is an existing quest/encounter event. Gossip and faction reactions subscribe to canonical choice facts. No catalog row emits a callback, changes standing, or changes an ending by itself.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Main.MoralChoice.cs` — loads chain data, resolves choices, journals facts, and seeds gossip
- `src/Main.CampaignOwners.cs` — current day-owner/event ordering; audit any echo dispatch seam
- `src/Host/MoralChoiceSaveStore.cs` — persists moral choice state
- `src/UI/MoralChoiceModal.cs` — current choice presentation
- `src/UI/QuestDetailPanel.cs` — existing quest detail/presentation seam

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Echoes should feel like consequences remembered by the world, not arbitrary delayed popups. Each callback needs a plausible source, timing, and owner; it cannot reveal a branch outcome the player has not earned or use sealed distress-signal content as a new scenario.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Journeys/MoralChoiceJourneyTests.cs`

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
| Phase 0 — chain/quest census | read 60 echoes, 88 gates, source quests, loader, and current fired state | all references and current count are explicit | no undocumented scope or shortcut |
| Phase 1 — live reachability trace | follow resolution → eligibility → presentation → fired marker | each arrow has a production caller or is labeled dormant | no undocumented scope or shortcut |
| Phase 2 — save/determinism/event audit | verify fired ids, delay, branch locks, and consumer ordering | no duplicate lifecycle or callback | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a proven presentation/consumer gap is promoted | existing quest owner and one focused test set | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/moral_choice_chains.json` | READ ONLY; MODIFY only for a proven reference/consumer defect | retain as chain authority |
| `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainData.cs` | READ ONLY | DTO and echo definition |
| `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` | READ ONLY | eligibility/fired owner |
| `src/Main.MoralChoice.cs` | READ ONLY | host choice/journal/gossip seam |
| `src/Main.CampaignOwners.cs` | READ ONLY; integrator-owned shared root | audit before any future edit |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| duplicate quest lifecycle | use the existing quest owner |
| duplicate fired state | use moral_choice fired ids |
| callback timing drift | persist resolution day and test delay |
| sealed content boundary | do not add distress scenarios |

# 22. Explicit Non-Goals

- no new quest lifecycle, no second moral score, no duplicate fired ledger, no arbitrary quest count, no sealed distress-signal content changes, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for moral-choice echo corpus are named from current evidence.
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

- A current 60-echo/88-gate/source-quest reference census.
- A production reachability trace from choice resolution to an existing presentation owner.
- A bounded exactly-once/save/determinism package only if a real gap is proven.

## MUST NOT DO

- create a new quest manager for echoes
- mark a row fired from a data load
- write faction/debt/journal state outside canonical owners
- expand the 60-row corpus for size alone

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Journeys/MoralChoiceJourneyTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read chain data/loader, MoralChoiceSystem echo methods, Main.MoralChoice, the current quest owner, save store, and focused tests; build the triggered_by/source-choice table.

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

### Authority lines 357–362
00357: Premise sweep executed 2026-09-24: live data listing (342 entries), live docs listing, `INTEGRATION_PLANS.md` (32,793 chars, head and tail), `SESSION_HANDOFF.md`, `AGENTS.md`, branch list, `docs/plans/` (126 entries). Every premise below that depends on file-internal state not readable in this pass is labeled with its open verification step. These are subject plans: they commit no file changes.
00358:
00359: ## Subject Plan F-001 — Delayed Moral-Choice Callbacks
00360:
00361: Lane A · Cluster C10 · Status PROPOSAL.
00362:

### Authority lines 712–717
00712: **A-23 · C9 · Intake interview continuation.** Subject: new-arrival intake interviews conditioned on the arrival channels that exist (rescue, crossing, holdfast). Evidence: `new_arrival_intake_interviews` exists; arrival channels are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00713:
00714: **A-24 · C10 · Bureaucratic-morality quest prose completion.** Subject: prose-field completion across `quests_bureaucratic_morality.json` records with skeleton `quest_hook`/outcome texts. Evidence: catalog verified live; Part 9 contracts define the fields. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00715:
00716: **A-25 · C10 · Massive-expansion corpus prose audit.** Subject: a prose-depth audit of `quests_massive_expansion_200.json` (200 records — the largest single prose debt surface in the data authority), converting skeleton records into contracted fields over several tranches. Evidence: catalog verified live; scale is structural evidence of thin per-record prose. Route: DATA-ONLY, multi-tranche. Confidence: HIGH CONFIDENCE.
00717:

### Authority lines 758–765
00758: **B-15 · C9 · Memorial-rite epilogue evidence enrollment.** Subject: performed rites enrolling as Reckoning evidence (rites exist; evidence vocabulary must be checked for a rite class before authoring). Evidence: `memorial_rites.json`, `spiritual_rituals.json` verified live. Route: CORE-EXTENSION through endgame owners. Confidence: PROPOSAL — evidence vocabulary check first.
00759:
00760: **B-16 · C10 · Quest reopening after new discoveries.** Subject: failed/abandoned quests reopening when discovery conditions later satisfy (the failure-recovery grammar of v1.0 Part 6.7). Evidence: abandoned-quest reopen is canon grammar; implementation state unverified. Route: CORE-EXTENSION through quest owners. Confidence: PROPOSAL.
00761:
00762: **B-17 · C10 · Moral-choice gossip propagation depth.** Subject: choice-driven gossip traveling the modeled channels with time lag proportional to distance. Evidence: `moral_choice_gossip.json` verified live; information-flow rules are canon. Route: CORE-EXTENSION. Confidence: PROPOSAL.
00763:
00764: **B-18 · C11 · Trade-screen scenario expansion.** Subject: additional scenarios and tell lines for under-covered merchant identities. Evidence: `trade_screen_scenarios.json`, `trade_tell_lines.json`, `trade_specialties.json` verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00765:

### Authority lines 822–827
00822: **D-06 · C1 · Shelter failure mid-event save semantics.** Subject: define and test mid-failure-event save/restore behavior for the quarantine-exited failure cascades (B-01 dependency). Evidence: quarantine logs verified. Route: design + tests. Confidence: PROPOSAL.
00823:
00824: **D-07 · C10 · Moral-choice flag persistence audit.** Subject: confirm every authored flag id persists and round-trips; default-tolerant missing-flag handling for older saves. Evidence: flags catalog live; F-001 depends on this. Route: tests. Confidence: HIGH CONFIDENCE as an audit; outcomes may be NONE.
00825:
00826: **D-08 · C16 · Completion-history difficulty stamp integrity.** Subject: verify the difficulty-preset stamp (schema v2) migrates cleanly when W1 adds preset fields. Evidence: stamp is canon (v1.0 Part 16.7). Route: migration + tests, W1-coordinated. Confidence: PROPOSAL, sequence-gated.
00827:

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

### Authority lines 949–954
00949: **DM-9 — Survivors and interiority (C9).** Owners: needs, health, skills, traits, mental arcs, trauma, therapies, guilt, crises, morale contagion, relations, caregiving, dependency, companion animals, beliefs, spiritual rituals, memorial rites, final wishes, belongings, memory decay, phantom memory, lineage, cohorts, apprenticeships. Live catalogs: `survivors`, `skills`, `development_traits`, `mental_arcs`, `psychological_trauma`, `psychological_therapies`, `guilt_sources`, `confession_secrets`, `belief_movements`, `spiritual_rituals`, `memorial_rites`, `final_wishes`, `companion_animals`, `phantom_heirlooms`, `phantom_triggers`, `starting_survivors`, `starting_survivor_cohorts`, `expansion_survivor_fields`. Hosts: Survivors, SurvivorRelations, PsychologyArc, MentalHealthCrisis, Caregiving, Spiritual, PhantomMemory. Openings: A-21, A-22, A-23, B-04, B-14, B-15, D-02. Known caution: `ClaimPersonalBelonging` no-caller finding (unverified at runtime — re-verify before extending).
00950:
00951: **DM-10 — Quests and moral choice (C10).** Owners: questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip/quests (five split catalogs live), branching faction quests, bureaucratic morality, massive expansion corpus, repeatable quests, templates, domain questlines (dose, year-of-ash, holdfast, crossing, thirdonary, verdict, expansion). Live catalogs: `questline_master`, `dynamic_questlines`, `personal_quests`, `npc_arcs`, `quests_npc_arcs`, `moral_choice_chains/flags/gossip/quests/quests_branching/quests_distress/quests_expansion`, `quests_faction_branching`, `quests_bureaucratic_morality`, `quests_massive_expansion_200`, `quests_moral_branching_expansion`, `repeatable_quests`, `quest_templates`. Hosts: NarrativeQuestline, PersonalQuest, MoralChoice, DynamicQuestline, ExpansionQuest, NpcArc. Openings: A-24, A-25, B-16, B-17, D-07, G-01, plus the F-001 flagship.
00952:
00953: **DM-11 — Economy (C11).** Owners: market, price factors, shocks, baselines, regional prices, hardcore tuning, rumor bands, black market, caravans, debt ledger, foundry economy, bounty board, trade screens. Live catalogs: `commodity_baselines`, `regional_prices`, `hardcore_economy_tuning`, `economy_goods`, `black_market_inventory`, `ledger_debt_templates`, `trade_screen_scenarios`, `trade_tell_lines`, `trade_specialties`, `trade_texts`, `bounty_board`. Hosts: Economy, BlackMarket, TravelingCaravan, SilentFoundry. Docs: `ECONOMY_FAIRNESS_AUDIT.md`, `ECONOMY_PRICE_FACTOR_MATRIX.md` (verified live). Sealed: merchant restock priority (DEC-05). Openings: A-26, B-18, C-07, C-08, C-13, E-08, G-02. GATE: black-market funds legs.
00954:

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/moral_choice_chains.json`
- Bytes: 31,788; SHA-256: `2a30c42686baef9f1e64cdda74fdb13cbe9204c641790d47212e5aeb48fc2135`
- Root keys: `branches, description, echo_quests, faction_reactions, gossip_propagation, lockout_rules, merge_rules, quest_gates, schema_version`
- `branches`: list[4]; union fields: `description, display_name, entry_quests, id, lock_threshold, locked_flag, locks_out, merge_allowed`
  - row 1: `{"description":"Compassion as a permanent stance. Helping becomes habit, then identity, then burden. Unlocks cooperative storylines; permanently closes the Iron Way and Broken Compact after the third mercy quest.","display_name":"The Mercy Road","entry_quests":["quest_moral_chain_mercy_01","quest_moral_chain_mercy_02","quest_moral_chain_mercy_03"],"id":"branch_mercy_road","lock_threshold":3,"locked_flag":"flag_branch_mercy_road_locked","locks_out":["branch_iron_way","branch_broken_compact"],"merge_allowed":["branch_listener_thread"]}`
  - row 2: `{"description":"Pragmatism hardens into ruthlessness. Survival at the cost of others becomes a doctrine. Permanently closes the Mercy Road and Listener's Thread after the third iron quest.","display_name":"The Iron Way","entry_quests":["quest_moral_chain_iron_01","quest_moral_chain_iron_02","quest_moral_chain_iron_03"],"id":"branch_iron_way","lock_threshold":3,"locked_flag":"flag_branch_iron_way_locked","locks_out":["branch_mercy_road","branch_listener_thread"],"merge_allowed":["branch_broken_compact"]}`
  - row 3: `{"description":"Understanding over action. The player accumulates stories and wisdom instead of taking sides. Permanently closes the Iron Way and Broken Compact after the third listener quest.","display_name":"The Listener's Thread","entry_quests":["quest_moral_chain_listen_01","quest_moral_chain_listen_02","quest_moral_chain_listen_03"],"id":"branch_listener_thread","lock_threshold":3,"locked_flag":"flag_branch_listener_locked","locks_out":["branch_iron_way","branch_broken_compact"],"merge_allowed":["branch_mercy_road"]}`
  - row 4: `{"description":"Betrayal as a survival strategy. Trust becomes a weapon. Permanently closes the Mercy Road and Listener's Thread after the third betrayal quest.","display_name":"The Broken Compact","entry_quests":["quest_moral_chain_betray_01","quest_moral_chain_betray_02","quest_moral_chain_betray_03"],"id":"branch_broken_compact","lock_threshold":3,"locked_flag":"flag_branch_broken_compact_locked","locks_out":["branch_mercy_road","branch_listener_thread"],"merge_allowed":["branch_iron_way"]}`
- `merge_rules`: object[4]
- `lockout_rules`: object[4]
- `quest_gates`: list[88]; union fields: `branch, quest_id, requires, requires_choice_index, requires_flag, requires_max_moral, requires_min_empathy, requires_min_moral`
  - row 1: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_04","requires":["quest_moral_chain_mercy_03"],"requires_choice_index":null,"requires_min_moral":15}`
  - row 2: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_05","requires":["quest_moral_chain_mercy_04"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 3: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_06","requires":["quest_moral_chain_mercy_05"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 4: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_07","requires":["quest_moral_chain_mercy_06"],"requires_choice_index":null,"requires_min_moral":30}`
  - row 5: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_08","requires":["quest_moral_chain_mercy_07"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 6: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_09","requires":["quest_moral_chain_mercy_08"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 7: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_10","requires":["quest_moral_chain_mercy_09"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 8: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_11","requires":["quest_moral_chain_mercy_10"],"requires_choice_index":null,"requires_min_moral":50}`
  - row 9: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_12","requires":["quest_moral_chain_mercy_11"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 10: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_13","requires":["quest_moral_chain_mercy_12"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 11: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_14","requires":["quest_moral_chain_mercy_13"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 12: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_15","requires":["quest_moral_chain_mercy_14"],"requires_choice_index":null,"requires_min_moral":75}`
  - row 13: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_16","requires":["quest_moral_chain_mercy_15"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 14: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_17","requires":["quest_moral_chain_mercy_16"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 15: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_18","requires":["quest_moral_chain_mercy_17"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 16: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_19","requires":["quest_moral_chain_mercy_18"],"requires_choice_index":null,"requires_min_moral":90}`
  - row 17: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_20","requires":["quest_moral_chain_mercy_19"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 18: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_21","requires":["quest_moral_chain_mercy_20"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 19: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_22","requires":["quest_moral_chain_mercy_21"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 20: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_23","requires":["quest_moral_chain_mercy_22"],"requires_choice_index":null,"requires_min_moral":100}`
  - row 21: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_24","requires":["quest_moral_chain_mercy_23"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 22: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_25","requires":["quest_moral_chain_mercy_24"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 23: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_04","requires":["quest_moral_chain_iron_03"],"requires_choice_index":null,"requires_max_moral":-15}`
  - row 24: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_05","requires":["quest_moral_chain_iron_04"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 25: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_06","requires":["quest_moral_chain_iron_05"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 26: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_07","requires":["quest_moral_chain_iron_06"],"requires_choice_index":null,"requires_max_moral":-30}`
  - row 27: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_08","requires":["quest_moral_chain_iron_07"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 28: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_09","requires":["quest_moral_chain_iron_08"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 29: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_10","requires":["quest_moral_chain_iron_09"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 30: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_11","requires":["quest_moral_chain_iron_10"],"requires_choice_index":null,"requires_max_moral":-50}`
  - row 31: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_12","requires":["quest_moral_chain_iron_11"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 32: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_13","requires":["quest_moral_chain_iron_12"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 33: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_14","requires":["quest_moral_chain_iron_13"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 34: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_15","requires":["quest_moral_chain_iron_14"],"requires_choice_index":null,"requires_max_moral":-75}`
  - row 35: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_16","requires":["quest_moral_chain_iron_15"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 36: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_17","requires":["quest_moral_chain_iron_16"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 37: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_18","requires":["quest_moral_chain_iron_17"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 38: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_19","requires":["quest_moral_chain_iron_18"],"requires_choice_index":null,"requires_max_moral":-90}`
  - row 39: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_20","requires":["quest_moral_chain_iron_19"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 40: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_21","requires":["quest_moral_chain_iron_20"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 41: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_22","requires":["quest_moral_chain_iron_21"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 42: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_23","requires":["quest_moral_chain_iron_22"],"requires_choice_index":null,"requires_max_moral":-100}`
  - row 43: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_24","requires":["quest_moral_chain_iron_23"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 44: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_25","requires":["quest_moral_chain_iron_24"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 45: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_04","requires":["quest_moral_chain_listen_03"],"requires_choice_index":null,"requires_min_empathy":8}`
  - row 46: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_05","requires":["quest_moral_chain_listen_04"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 47: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_06","requires":["quest_moral_chain_listen_05"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 48: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_07","requires":["quest_moral_chain_listen_06"],"requires_choice_index":null,"requires_min_empathy":15}`
  - row 49: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_08","requires":["quest_moral_chain_listen_07"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 50: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_09","requires":["quest_moral_chain_listen_08"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 51: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_10","requires":["quest_moral_chain_listen_09"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 52: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_11","requires":["quest_moral_chain_listen_10"],"requires_choice_index":null,"requires_min_empathy":22}`
  - row 53: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_12","requires":["quest_moral_chain_listen_11"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 54: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_13","requires":["quest_moral_chain_listen_12"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 55: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_14","requires":["quest_moral_chain_listen_13"],"requires_choice_index":null,"requires_min_empathy":30}`
  - row 56: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_15","requires":["quest_moral_chain_listen_14"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 57: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_16","requires":["quest_moral_chain_listen_15"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 58: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_17","requires":["quest_moral_chain_listen_16"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 59: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_18","requires":["quest_moral_chain_listen_17"],"requires_choice_index":null,"requires_min_empathy":38}`
  - row 60: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_19","requires":["quest_moral_chain_listen_18"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 61: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_20","requires":["quest_moral_chain_listen_19"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 62: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_21","requires":["quest_moral_chain_listen_20"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 63: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_22","requires":["quest_moral_chain_listen_21"],"requires_choice_index":null,"requires_min_empathy":45}`
  - row 64: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_23","requires":["quest_moral_chain_listen_22"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 65: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_24","requires":["quest_moral_chain_listen_23"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 66: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_25","requires":["quest_moral_chain_listen_24"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 67: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_04","requires":["quest_moral_chain_betray_03"],"requires_choice_index":null,"requires_flag":"flag_betrayed_trust"}`
  - row 68: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_05","requires":["quest_moral_chain_betray_04"],"requires_choice_index":null,"requires_flag":null}`
  - row 69: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_06","requires":["quest_moral_chain_betray_05"],"requires_choice_index":null,"requires_flag":null}`
  - row 70: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_07","requires":["quest_moral_chain_betray_06"],"requires_choice_index":null,"requires_flag":"flag_betrayed_ally"}`
  - row 71: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_08","requires":["quest_moral_chain_betray_07"],"requires_choice_index":null,"requires_flag":null}`
  - row 72: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_09","requires":["quest_moral_chain_betray_08"],"requires_choice_index":null,"requires_flag":null}`
  - row 73: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_10","requires":["quest_moral_chain_betray_09"],"requires_choice_index":null,"requires_flag":null}`
  - row 74: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_11","requires":["quest_moral_chain_betray_10"],"requires_choice_index":null,"requires_flag":"flag_betrayed_faction"}`
  - row 75: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_12","requires":["quest_moral_chain_betray_11"],"requires_choice_index":null,"requires_flag":null}`
  - row 76: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_13","requires":["quest_moral_chain_betray_12"],"requires_choice_index":null,"requires_flag":null}`
  - row 77: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_14","requires":["quest_moral_chain_betray_13"],"requires_choice_index":null,"requires_flag":null}`
  - row 78: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_15","requires":["quest_moral_chain_betray_14"],"requires_choice_index":null,"requires_flag":"flag_broken_pact"}`
  - row 79: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_16","requires":["quest_moral_chain_betray_15"],"requires_choice_index":null,"requires_flag":null}`
  - row 80: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_17","requires":["quest_moral_chain_betray_16"],"requires_choice_index":null,"requires_flag":null}`
  - row 81: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_18","requires":["quest_moral_chain_betray_17"],"requires_choice_index":null,"requires_flag":null}`
  - row 82: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_19","requires":["quest_moral_chain_betray_18"],"requires_choice_index":null,"requires_flag":"flag_become_warlord"}`
  - row 83: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_20","requires":["quest_moral_chain_betray_19"],"requires_choice_index":null,"requires_flag":null}`
  - row 84: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_21","requires":["quest_moral_chain_betray_20"],"requires_choice_index":null,"requires_flag":null}`
  - row 85: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_22","requires":["quest_moral_chain_betray_21"],"requires_choice_index":null,"requires_flag":null}`
  - row 86: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_23","requires":["quest_moral_chain_betray_22"],"requires_choice_index":null,"requires_flag":"flag_throne_of_ash"}`
  - row 87: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_24","requires":["quest_moral_chain_betray_23"],"requires_choice_index":null,"requires_flag":null}`
  - row 88: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_25","requires":["quest_moral_chain_betray_24"],"requires_choice_index":null,"requires_flag":null}`
- `echo_quests`: object[2]
  - `quests`: list[60]; union fields: `branch, min_days_after, quest_id, triggered_by, triggered_by_choice`
    - row 1: `{"branch":null,"min_days_after":30,"quest_id":"quest_moral_echo_child_returns","triggered_by":"quest_moral_share_child","triggered_by_choice":0}`
    - row 2: `{"branch":null,"min_days_after":20,"quest_id":"quest_moral_echo_child_steals","triggered_by":"quest_moral_share_child","triggered_by_choice":3}`
    - row 3: `{"branch":null,"min_days_after":40,"quest_id":"quest_moral_echo_family_defends","triggered_by":"quest_moral_share_family","triggered_by_choice":0}`
    - row 4: `{"branch":null,"min_days_after":25,"quest_id":"quest_moral_echo_family_ambush","triggered_by":"quest_moral_share_family","triggered_by_choice":3}`
    - row 5: `{"branch":null,"min_days_after":60,"quest_id":"quest_moral_echo_farmer_harvest","triggered_by":"quest_moral_share_farmer","triggered_by_choice":0}`
    - row 6: `{"branch":null,"min_days_after":45,"quest_id":"quest_moral_echo_farmer_dead","triggered_by":"quest_moral_share_farmer","triggered_by_choice":3}`
    - row 7: `{"branch":null,"min_days_after":15,"quest_id":"quest_moral_echo_raider_warning","triggered_by":"quest_moral_share_raider","triggered_by_choice":0}`
    - row 8: `{"branch":null,"min_days_after":10,"quest_id":"quest_moral_echo_raider_ambush","triggered_by":"quest_moral_share_raider","triggered_by_choice":3}`
    - row 9: `{"branch":null,"min_days_after":20,"quest_id":"quest_moral_echo_peacekeeper_intel","triggered_by":"quest_moral_share_peacekeeper","triggered_by_choice":0}`
    - row 10: `{"branch":null,"min_days_after":15,"quest_id":"quest_moral_echo_peacekeeper_hunted","triggered_by":"quest_moral_share_peacekeeper","triggered_by_choice":3}`
    - row 11: `{"branch":null,"min_days_after":25,"quest_id":"quest_moral_echo_widow_gift","triggered_by":"quest_moral_comfort_widow","triggered_by_choice":0}`
    - row 12: `{"branch":null,"min_days_after":35,"quest_id":"quest_moral_echo_prophet_map","triggered_by":"quest_moral_listen_prophet","triggered_by_choice":0}`
    - row 13: `{"branch":null,"min_days_after":20,"quest_id":"quest_moral_echo_prophet_curse","triggered_by":"quest_moral_listen_prophet","triggered_by_choice":3}`
    - row 14: `{"branch":null,"min_days_after":30,"quest_id":"quest_moral_echo_soldier_teaches","triggered_by":"quest_moral_listen_soldier","triggered_by_choice":0}`
    - row 15: `{"branch":null,"min_days_after":15,"quest_id":"quest_moral_echo_soldier_hostile","triggered_by":"quest_moral_listen_soldier","triggered_by_choice":3}`
    - row 16: `{"branch":null,"min_days_after":40,"quest_id":"quest_moral_echo_messenger_packet","triggered_by":"quest_moral_trust_messenger","triggered_by_choice":0}`
    - row 17: `{"branch":null,"min_days_after":20,"quest_id":"quest_moral_echo_messenger_stolen","triggered_by":"quest_moral_trust_messenger","triggered_by_choice":3}`
    - row 18: `{"branch":null,"min_days_after":30,"quest_id":"quest_moral_echo_dead_child_haunt","triggered_by":"quest_moral_dead_child","triggered_by_choice":2}`
    - row 19: `{"branch":null,"min_days_after":50,"quest_id":"quest_moral_echo_dead_child_peace","triggered_by":"quest_moral_dead_child","triggered_by_choice":0}`
    - row 20: `{"branch":null,"min_days_after":45,"quest_id":"quest_moral_echo_scientist_formula","triggered_by":"quest_moral_share_scientist","triggered_by_choice":0}`
    - row 21: `{"branch":"branch_mercy_road","min_days_after":20,"quest_id":"quest_moral_echo_mercy_recognized","triggered_by":"quest_moral_chain_mercy_05","triggered_by_choice":0}`
    - row 22: `{"branch":"branch_iron_way","min_days_after":20,"quest_id":"quest_moral_echo_iron_feared","triggered_by":"quest_moral_chain_iron_05","triggered_by_choice":0}`
    - row 23: `{"branch":"branch_listener_thread","min_days_after":20,"quest_id":"quest_moral_echo_listener_confided","triggered_by":"quest_moral_chain_listen_05","triggered_by_choice":0}`
    - row 24: `{"branch":"branch_broken_compact","min_days_after":15,"quest_id":"quest_moral_echo_betrayer_hunted","triggered_by":"quest_moral_chain_betray_05","triggered_by_choice":0}`
    - row 25: `{"branch":"branch_mercy_road","min_days_after":15,"quest_id":"quest_moral_echo_mercy_tested","triggered_by":"quest_moral_chain_mercy_10","triggered_by_choice":0}`
    - row 26: `{"branch":"branch_iron_way","min_days_after":15,"quest_id":"quest_moral_echo_iron_challenged","triggered_by":"quest_moral_chain_iron_10","triggered_by_choice":0}`
    - row 27: `{"branch":"branch_listener_thread","min_days_after":25,"quest_id":"quest_moral_echo_listener_secret","triggered_by":"quest_moral_chain_listen_10","triggered_by_choice":0}`
    - row 28: `{"branch":"branch_broken_compact","min_days_after":10,"quest_id":"quest_moral_echo_betrayer_cornered","triggered_by":"quest_moral_chain_betray_10","triggered_by_choice":0}`
    - row 29: `{"branch":"branch_mercy_road","min_days_after":10,"quest_id":"quest_moral_echo_mercy_final","triggered_by":"quest_moral_chain_mercy_20","triggered_by_choice":0}`
    - row 30: `{"branch":"branch_iron_way","min_days_after":10,"quest_id":"quest_moral_echo_iron_final","triggered_by":"quest_moral_chain_iron_20","triggered_by_choice":0}`
    - row 31: `{"branch":"branch_listener_thread","min_days_after":10,"quest_id":"quest_moral_echo_listener_final","triggered_by":"quest_moral_chain_listen_20","triggered_by_choice":0}`
    - row 32: `{"branch":"branch_broken_compact","min_days_after":10,"quest_id":"quest_moral_echo_betrayer_final","triggered_by":"quest_moral_chain_betray_20","triggered_by_choice":0}`
    - row 33: `{"branch":"branch_mercy_road","min_days_after":25,"quest_id":"quest_moral_echo_raider_repaid_warning","triggered_by":"quest_moral_chain_mercy_16","triggered_by_choice":0}`
    - row 34: `{"branch":"branch_mercy_road","min_days_after":50,"quest_id":"quest_moral_echo_betrayers_child_grown","triggered_by":"quest_moral_chain_mercy_07","triggered_by_choice":0}`
    - row 35: `{"branch":"branch_mercy_road","min_days_after":35,"quest_id":"quest_moral_echo_medicine_shared_recovered","triggered_by":"quest_moral_chain_mercy_14","triggered_by_choice":0}`
    - row 36: `{"branch":"branch_mercy_road","min_days_after":40,"quest_id":"quest_moral_echo_convoy_haven_opened","triggered_by":"quest_moral_chain_mercy_06","triggered_by_choice":0}`
    - row 37: `{"branch":"branch_mercy_road","min_days_after":30,"quest_id":"quest_moral_echo_plague_secret_infection","triggered_by":"quest_moral_chain_mercy_12","triggered_by_choice":0}`
    - row 38: `{"branch":"branch_mercy_road","min_days_after":30,"quest_id":"quest_moral_echo_well_gratitude_refused","triggered_by":"quest_moral_chain_mercy_08","triggered_by_choice":0}`
    - row 39: `{"branch":"branch_mercy_road","min_days_after":35,"quest_id":"quest_moral_echo_patrol_reputation_spread","triggered_by":"quest_moral_chain_mercy_04","triggered_by_choice":0}`
    - row 40: `{"branch":"branch_mercy_road","min_days_after":30,"quest_id":"quest_moral_echo_shelter_vote_strained_rations","triggered_by":"quest_moral_chain_mercy_11","triggered_by_choice":0}`
    - row 41: `{"branch":"branch_iron_way","min_days_after":45,"quest_id":"quest_moral_echo_aldric_blockade_retaliation","triggered_by":"quest_moral_chain_iron_06","triggered_by_choice":0}`
    - row 42: `{"branch":"branch_iron_way","min_days_after":30,"quest_id":"quest_moral_echo_old_friend_farewell_note","triggered_by":"quest_moral_chain_iron_13","triggered_by_choice":2}`
    - row 43: `{"branch":"branch_iron_way","min_days_after":35,"quest_id":"quest_moral_echo_expulsion_deterrence_held","triggered_by":"quest_moral_chain_iron_07","triggered_by_choice":0}`
    - row 44: `{"branch":"branch_iron_way","min_days_after":30,"quest_id":"quest_moral_echo_strike_broken_fear_quota","triggered_by":"quest_moral_chain_iron_11","triggered_by_choice":2}`
    - row 45: `{"branch":"branch_iron_way","min_days_after":40,"quest_id":"quest_moral_echo_informant_applies_leverage","triggered_by":"quest_moral_chain_iron_04","triggered_by_choice":1}`
    - row 46: `{"branch":"branch_iron_way","min_days_after":50,"quest_id":"quest_moral_echo_lowfield_harvest_dividend","triggered_by":"quest_moral_chain_iron_08","triggered_by_choice":0}`
    - row 47: `{"branch":"branch_iron_way","min_days_after":35,"quest_id":"quest_moral_echo_varek_blood_debt_claim","triggered_by":"quest_moral_chain_iron_15","triggered_by_choice":0}`
    - row 48: `{"branch":"branch_iron_way","min_days_after":45,"quest_id":"quest_moral_echo_calla_camp_empty_ruin","triggered_by":"quest_moral_chain_iron_18","triggered_by_choice":1}`
    - row 49: `{"branch":"branch_listener_thread","min_days_after":35,"quest_id":"quest_moral_echo_defector_corroborates_truth","triggered_by":"quest_moral_chain_listen_11","triggered_by_choice":0}`
    - row 50: `{"branch":"branch_listener_thread","min_days_after":40,"quest_id":"quest_moral_echo_cartographer_water_cache_located","triggered_by":"quest_moral_chain_listen_15","triggered_by_choice":0}`
    - row 51: `{"branch":"branch_listener_thread","min_days_after":30,"quest_id":"quest_moral_echo_prophet_calendar_discrepancy","triggered_by":"quest_moral_chain_listen_09","triggered_by_choice":0}`
    - row 52: `{"branch":"branch_listener_thread","min_days_after":30,"quest_id":"quest_moral_echo_trader_ledger_censorship_threat","triggered_by":"quest_moral_chain_listen_07","triggered_by_choice":0}`
    - row 53: `{"branch":"branch_listener_thread","min_days_after":35,"quest_id":"quest_moral_echo_soldier_second_confession","triggered_by":"quest_moral_chain_listen_08","triggered_by_choice":0}`
    - row 54: `{"branch":"branch_listener_thread","min_days_after":45,"quest_id":"quest_moral_echo_doctors_notes_reinterpreted","triggered_by":"quest_moral_chain_listen_04","triggered_by_choice":0}`
    - row 55: `{"branch":"branch_listener_thread","min_days_after":50,"quest_id":"quest_moral_echo_librarian_memorial_preserved","triggered_by":"quest_moral_chain_listen_12","triggered_by_choice":0}`
    - row 56: `{"branch":"branch_broken_compact","min_days_after":35,"quest_id":"quest_moral_echo_kessler_exile_uncovered","triggered_by":"quest_moral_chain_betray_04","triggered_by_choice":1}`
    - row 57: `{"branch":"branch_broken_compact","min_days_after":30,"quest_id":"quest_moral_echo_poisoned_gift_reputation_drop","triggered_by":"quest_moral_chain_betray_06","triggered_by_choice":1}`
    - row 58: `{"branch":"branch_broken_compact","min_days_after":25,"quest_id":"quest_moral_echo_crisis_gambit_warlord_respect","triggered_by":"quest_moral_chain_betray_11","triggered_by_choice":1}`
    - row 59: `{"branch":"branch_broken_compact","min_days_after":40,"quest_id":"quest_moral_echo_voss_blackmail_exposed","triggered_by":"quest_moral_chain_betray_08","triggered_by_choice":2}`
    - row 60: `{"branch":"branch_broken_compact","min_days_after":35,"quest_id":"quest_moral_echo_pell_hostage_border_locked","triggered_by":"quest_moral_chain_betray_16","triggered_by_choice":1}`
- `gossip_propagation`: object[2]
- `faction_reactions`: object[2]
- Bytes: 31,788; SHA-256: `2a30c42686baef9f1e64cdda74fdb13cbe9204c641790d47212e5aeb48fc2135`
- Root keys: `branches, description, echo_quests, faction_reactions, gossip_propagation, lockout_rules, merge_rules, quest_gates, schema_version`
- `branches`: list[4]; union fields: `description, display_name, entry_quests, id, lock_threshold, locked_flag, locks_out, merge_allowed`
  - row 1: `{"description":"Compassion as a permanent stance. Helping becomes habit, then identity, then burden. Unlocks cooperative storylines; permanently closes the Iron Way and Broken Compact after the third mercy quest.","display_name":"The Mercy Road","entry_quests":["quest_moral_chain_mercy_01","quest_moral_chain_mercy_02","quest_moral_chain_mercy_03"],"id":"branch_mercy_road","lock_threshold":3,"locked_flag":"flag_branch_mercy_road_locked","locks_out":["branch_iron_way","branch_broken_compact"],"merge_allowed":["branch_listener_thread"]}`
  - row 2: `{"description":"Pragmatism hardens into ruthlessness. Survival at the cost of others becomes a doctrine. Permanently closes the Mercy Road and Listener's Thread after the third iron quest.","display_name":"The Iron Way","entry_quests":["quest_moral_chain_iron_01","quest_moral_chain_iron_02","quest_moral_chain_iron_03"],"id":"branch_iron_way","lock_threshold":3,"locked_flag":"flag_branch_iron_way_locked","locks_out":["branch_mercy_road","branch_listener_thread"],"merge_allowed":["branch_broken_compact"]}`
  - row 3: `{"description":"Understanding over action. The player accumulates stories and wisdom instead of taking sides. Permanently closes the Iron Way and Broken Compact after the third listener quest.","display_name":"The Listener's Thread","entry_quests":["quest_moral_chain_listen_01","quest_moral_chain_listen_02","quest_moral_chain_listen_03"],"id":"branch_listener_thread","lock_threshold":3,"locked_flag":"flag_branch_listener_locked","locks_out":["branch_iron_way","branch_broken_compact"],"merge_allowed":["branch_mercy_road"]}`
  - row 4: `{"description":"Betrayal as a survival strategy. Trust becomes a weapon. Permanently closes the Mercy Road and Listener's Thread after the third betrayal quest.","display_name":"The Broken Compact","entry_quests":["quest_moral_chain_betray_01","quest_moral_chain_betray_02","quest_moral_chain_betray_03"],"id":"branch_broken_compact","lock_threshold":3,"locked_flag":"flag_branch_broken_compact_locked","locks_out":["branch_mercy_road","branch_listener_thread"],"merge_allowed":["branch_iron_way"]}`
- `merge_rules`: object[4]
- `lockout_rules`: object[4]
- `quest_gates`: list[88]; union fields: `branch, quest_id, requires, requires_choice_index, requires_flag, requires_max_moral, requires_min_empathy, requires_min_moral`
  - row 1: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_04","requires":["quest_moral_chain_mercy_03"],"requires_choice_index":null,"requires_min_moral":15}`
  - row 2: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_05","requires":["quest_moral_chain_mercy_04"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 3: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_06","requires":["quest_moral_chain_mercy_05"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 4: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_07","requires":["quest_moral_chain_mercy_06"],"requires_choice_index":null,"requires_min_moral":30}`
  - row 5: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_08","requires":["quest_moral_chain_mercy_07"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 6: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_09","requires":["quest_moral_chain_mercy_08"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 7: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_10","requires":["quest_moral_chain_mercy_09"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 8: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_11","requires":["quest_moral_chain_mercy_10"],"requires_choice_index":null,"requires_min_moral":50}`
  - row 9: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_12","requires":["quest_moral_chain_mercy_11"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 10: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_13","requires":["quest_moral_chain_mercy_12"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 11: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_14","requires":["quest_moral_chain_mercy_13"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 12: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_15","requires":["quest_moral_chain_mercy_14"],"requires_choice_index":null,"requires_min_moral":75}`
  - row 13: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_16","requires":["quest_moral_chain_mercy_15"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 14: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_17","requires":["quest_moral_chain_mercy_16"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 15: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_18","requires":["quest_moral_chain_mercy_17"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 16: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_19","requires":["quest_moral_chain_mercy_18"],"requires_choice_index":null,"requires_min_moral":90}`
  - row 17: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_20","requires":["quest_moral_chain_mercy_19"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 18: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_21","requires":["quest_moral_chain_mercy_20"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 19: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_22","requires":["quest_moral_chain_mercy_21"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 20: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_23","requires":["quest_moral_chain_mercy_22"],"requires_choice_index":null,"requires_min_moral":100}`
  - row 21: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_24","requires":["quest_moral_chain_mercy_23"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 22: `{"branch":"branch_mercy_road","quest_id":"quest_moral_chain_mercy_25","requires":["quest_moral_chain_mercy_24"],"requires_choice_index":null,"requires_min_moral":null}`
  - row 23: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_04","requires":["quest_moral_chain_iron_03"],"requires_choice_index":null,"requires_max_moral":-15}`
  - row 24: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_05","requires":["quest_moral_chain_iron_04"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 25: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_06","requires":["quest_moral_chain_iron_05"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 26: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_07","requires":["quest_moral_chain_iron_06"],"requires_choice_index":null,"requires_max_moral":-30}`
  - row 27: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_08","requires":["quest_moral_chain_iron_07"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 28: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_09","requires":["quest_moral_chain_iron_08"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 29: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_10","requires":["quest_moral_chain_iron_09"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 30: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_11","requires":["quest_moral_chain_iron_10"],"requires_choice_index":null,"requires_max_moral":-50}`
  - row 31: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_12","requires":["quest_moral_chain_iron_11"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 32: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_13","requires":["quest_moral_chain_iron_12"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 33: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_14","requires":["quest_moral_chain_iron_13"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 34: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_15","requires":["quest_moral_chain_iron_14"],"requires_choice_index":null,"requires_max_moral":-75}`
  - row 35: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_16","requires":["quest_moral_chain_iron_15"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 36: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_17","requires":["quest_moral_chain_iron_16"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 37: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_18","requires":["quest_moral_chain_iron_17"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 38: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_19","requires":["quest_moral_chain_iron_18"],"requires_choice_index":null,"requires_max_moral":-90}`
  - row 39: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_20","requires":["quest_moral_chain_iron_19"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 40: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_21","requires":["quest_moral_chain_iron_20"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 41: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_22","requires":["quest_moral_chain_iron_21"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 42: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_23","requires":["quest_moral_chain_iron_22"],"requires_choice_index":null,"requires_max_moral":-100}`
  - row 43: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_24","requires":["quest_moral_chain_iron_23"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 44: `{"branch":"branch_iron_way","quest_id":"quest_moral_chain_iron_25","requires":["quest_moral_chain_iron_24"],"requires_choice_index":null,"requires_max_moral":null}`
  - row 45: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_04","requires":["quest_moral_chain_listen_03"],"requires_choice_index":null,"requires_min_empathy":8}`
  - row 46: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_05","requires":["quest_moral_chain_listen_04"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 47: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_06","requires":["quest_moral_chain_listen_05"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 48: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_07","requires":["quest_moral_chain_listen_06"],"requires_choice_index":null,"requires_min_empathy":15}`
  - row 49: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_08","requires":["quest_moral_chain_listen_07"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 50: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_09","requires":["quest_moral_chain_listen_08"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 51: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_10","requires":["quest_moral_chain_listen_09"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 52: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_11","requires":["quest_moral_chain_listen_10"],"requires_choice_index":null,"requires_min_empathy":22}`
  - row 53: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_12","requires":["quest_moral_chain_listen_11"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 54: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_13","requires":["quest_moral_chain_listen_12"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 55: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_14","requires":["quest_moral_chain_listen_13"],"requires_choice_index":null,"requires_min_empathy":30}`
  - row 56: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_15","requires":["quest_moral_chain_listen_14"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 57: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_16","requires":["quest_moral_chain_listen_15"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 58: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_17","requires":["quest_moral_chain_listen_16"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 59: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_18","requires":["quest_moral_chain_listen_17"],"requires_choice_index":null,"requires_min_empathy":38}`
  - row 60: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_19","requires":["quest_moral_chain_listen_18"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 61: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_20","requires":["quest_moral_chain_listen_19"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 62: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_21","requires":["quest_moral_chain_listen_20"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 63: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_22","requires":["quest_moral_chain_listen_21"],"requires_choice_index":null,"requires_min_empathy":45}`
  - row 64: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_23","requires":["quest_moral_chain_listen_22"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 65: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_24","requires":["quest_moral_chain_listen_23"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 66: `{"branch":"branch_listener_thread","quest_id":"quest_moral_chain_listen_25","requires":["quest_moral_chain_listen_24"],"requires_choice_index":null,"requires_min_empathy":null}`
  - row 67: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_04","requires":["quest_moral_chain_betray_03"],"requires_choice_index":null,"requires_flag":"flag_betrayed_trust"}`
  - row 68: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_05","requires":["quest_moral_chain_betray_04"],"requires_choice_index":null,"requires_flag":null}`
  - row 69: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_06","requires":["quest_moral_chain_betray_05"],"requires_choice_index":null,"requires_flag":null}`
  - row 70: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_07","requires":["quest_moral_chain_betray_06"],"requires_choice_index":null,"requires_flag":"flag_betrayed_ally"}`
  - row 71: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_08","requires":["quest_moral_chain_betray_07"],"requires_choice_index":null,"requires_flag":null}`
  - row 72: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_09","requires":["quest_moral_chain_betray_08"],"requires_choice_index":null,"requires_flag":null}`
  - row 73: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_10","requires":["quest_moral_chain_betray_09"],"requires_choice_index":null,"requires_flag":null}`
  - row 74: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_11","requires":["quest_moral_chain_betray_10"],"requires_choice_index":null,"requires_flag":"flag_betrayed_faction"}`
  - row 75: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_12","requires":["quest_moral_chain_betray_11"],"requires_choice_index":null,"requires_flag":null}`
  - row 76: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_13","requires":["quest_moral_chain_betray_12"],"requires_choice_index":null,"requires_flag":null}`
  - row 77: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_14","requires":["quest_moral_chain_betray_13"],"requires_choice_index":null,"requires_flag":null}`
  - row 78: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_15","requires":["quest_moral_chain_betray_14"],"requires_choice_index":null,"requires_flag":"flag_broken_pact"}`
  - row 79: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_16","requires":["quest_moral_chain_betray_15"],"requires_choice_index":null,"requires_flag":null}`
  - row 80: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_17","requires":["quest_moral_chain_betray_16"],"requires_choice_index":null,"requires_flag":null}`
  - row 81: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_18","requires":["quest_moral_chain_betray_17"],"requires_choice_index":null,"requires_flag":null}`
  - row 82: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_19","requires":["quest_moral_chain_betray_18"],"requires_choice_index":null,"requires_flag":"flag_become_warlord"}`
  - row 83: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_20","requires":["quest_moral_chain_betray_19"],"requires_choice_index":null,"requires_flag":null}`
  - row 84: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_21","requires":["quest_moral_chain_betray_20"],"requires_choice_index":null,"requires_flag":null}`
  - row 85: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_22","requires":["quest_moral_chain_betray_21"],"requires_choice_index":null,"requires_flag":null}`
  - row 86: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_23","requires":["quest_moral_chain_betray_22"],"requires_choice_index":null,"requires_flag":"flag_throne_of_ash"}`
  - row 87: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_24","requires":["quest_moral_chain_betray_23"],"requires_choice_index":null,"requires_flag":null}`
  - row 88: `{"branch":"branch_broken_compact","quest_id":"quest_moral_chain_betray_25","requires":["quest_moral_chain_betray_24"],"requires_choice_index":null,"requires_flag":null}`
- `echo_quests`: object[2]
  - `quests`: list[60]; union fields: `branch, min_days_after, quest_id, triggered_by, triggered_by_choice`
    - row 1: `{"branch":null,"min_days_after":30,"quest_id":"quest_moral_echo_child_returns","triggered_by":"quest_moral_share_child","triggered_by_choice":0}`
    - row 2: `{"branch":null,"min_days_after":20,"quest_id":"quest_moral_echo_child_steals","triggered_by":"quest_moral_share_child","triggered_by_choice":3}`
    - row 3: `{"branch":null,"min_days_after":40,"quest_id":"quest_moral_echo_family_defends","triggered_by":"quest_moral_share_family","triggered_by_choice":0}`
    - row 4: `{"branch":null,"min_days_after":25,"quest_id":"quest_moral_echo_family_ambush","triggered_by":"quest_moral_share_family","triggered_by_choice":3}`
    - row 5: `{"branch":null,"min_days_after":60,"quest_id":"quest_moral_echo_farmer_harvest","triggered_by":"quest_moral_share_farmer","triggered_by_choice":0}`
    - row 6: `{"branch":null,"min_days_after":45,"quest_id":"quest_moral_echo_farmer_dead","triggered_by":"quest_moral_share_farmer","triggered_by_choice":3}`
    - row 7: `{"branch":null,"min_days_after":15,"quest_id":"quest_moral_echo_raider_warning","triggered_by":"quest_moral_share_raider","triggered_by_choice":0}`
    - row 8: `{"branch":null,"min_days_after":10,"quest_id":"quest_moral_echo_raider_ambush","triggered_by":"quest_moral_share_raider","triggered_by_choice":3}`
    - row 9: `{"branch":null,"min_days_after":20,"quest_id":"quest_moral_echo_peacekeeper_intel","triggered_by":"quest_moral_share_peacekeeper","triggered_by_choice":0}`
    - row 10: `{"branch":null,"min_days_after":15,"quest_id":"quest_moral_echo_peacekeeper_hunted","triggered_by":"quest_moral_share_peacekeeper","triggered_by_choice":3}`
    - row 11: `{"branch":null,"min_days_after":25,"quest_id":"quest_moral_echo_widow_gift","triggered_by":"quest_moral_comfort_widow","triggered_by_choice":0}`
    - row 12: `{"branch":null,"min_days_after":35,"quest_id":"quest_moral_echo_prophet_map","triggered_by":"quest_moral_listen_prophet","triggered_by_choice":0}`
    - row 13: `{"branch":null,"min_days_after":20,"quest_id":"quest_moral_echo_prophet_curse","triggered_by":"quest_moral_listen_prophet","triggered_by_choice":3}`
    - row 14: `{"branch":null,"min_days_after":30,"quest_id":"quest_moral_echo_soldier_teaches","triggered_by":"quest_moral_listen_soldier","triggered_by_choice":0}`
    - row 15: `{"branch":null,"min_days_after":15,"quest_id":"quest_moral_echo_soldier_hostile","triggered_by":"quest_moral_listen_soldier","triggered_by_choice":3}`
    - row 16: `{"branch":null,"min_days_after":40,"quest_id":"quest_moral_echo_messenger_packet","triggered_by":"quest_moral_trust_messenger","triggered_by_choice":0}`
    - row 17: `{"branch":null,"min_days_after":20,"quest_id":"quest_moral_echo_messenger_stolen","triggered_by":"quest_moral_trust_messenger","triggered_by_choice":3}`
    - row 18: `{"branch":null,"min_days_after":30,"quest_id":"quest_moral_echo_dead_child_haunt","triggered_by":"quest_moral_dead_child","triggered_by_choice":2}`
    - row 19: `{"branch":null,"min_days_after":50,"quest_id":"quest_moral_echo_dead_child_peace","triggered_by":"quest_moral_dead_child","triggered_by_choice":0}`
    - row 20: `{"branch":null,"min_days_after":45,"quest_id":"quest_moral_echo_scientist_formula","triggered_by":"quest_moral_share_scientist","triggered_by_choice":0}`
    - row 21: `{"branch":"branch_mercy_road","min_days_after":20,"quest_id":"quest_moral_echo_mercy_recognized","triggered_by":"quest_moral_chain_mercy_05","triggered_by_choice":0}`
    - row 22: `{"branch":"branch_iron_way","min_days_after":20,"quest_id":"quest_moral_echo_iron_feared","triggered_by":"quest_moral_chain_iron_05","triggered_by_choice":0}`
    - row 23: `{"branch":"branch_listener_thread","min_days_after":20,"quest_id":"quest_moral_echo_listener_confided","triggered_by":"quest_moral_chain_listen_05","triggered_by_choice":0}`
    - row 24: `{"branch":"branch_broken_compact","min_days_after":15,"quest_id":"quest_moral_echo_betrayer_hunted","triggered_by":"quest_moral_chain_betray_05","triggered_by_choice":0}`
    - row 25: `{"branch":"branch_mercy_road","min_days_after":15,"quest_id":"quest_moral_echo_mercy_tested","triggered_by":"quest_moral_chain_mercy_10","triggered_by_choice":0}`
    - row 26: `{"branch":"branch_iron_way","min_days_after":15,"quest_id":"quest_moral_echo_iron_challenged","triggered_by":"quest_moral_chain_iron_10","triggered_by_choice":0}`
    - row 27: `{"branch":"branch_listener_thread","min_days_after":25,"quest_id":"quest_moral_echo_listener_secret","triggered_by":"quest_moral_chain_listen_10","triggered_by_choice":0}`
    - row 28: `{"branch":"branch_broken_compact","min_days_after":10,"quest_id":"quest_moral_echo_betrayer_cornered","triggered_by":"quest_moral_chain_betray_10","triggered_by_choice":0}`
    - row 29: `{"branch":"branch_mercy_road","min_days_after":10,"quest_id":"quest_moral_echo_mercy_final","triggered_by":"quest_moral_chain_mercy_20","triggered_by_choice":0}`
    - row 30: `{"branch":"branch_iron_way","min_days_after":10,"quest_id":"quest_moral_echo_iron_final","triggered_by":"quest_moral_chain_iron_20","triggered_by_choice":0}`
    - row 31: `{"branch":"branch_listener_thread","min_days_after":10,"quest_id":"quest_moral_echo_listener_final","triggered_by":"quest_moral_chain_listen_20","triggered_by_choice":0}`
    - row 32: `{"branch":"branch_broken_compact","min_days_after":10,"quest_id":"quest_moral_echo_betrayer_final","triggered_by":"quest_moral_chain_betray_20","triggered_by_choice":0}`
    - row 33: `{"branch":"branch_mercy_road","min_days_after":25,"quest_id":"quest_moral_echo_raider_repaid_warning","triggered_by":"quest_moral_chain_mercy_16","triggered_by_choice":0}`
    - row 34: `{"branch":"branch_mercy_road","min_days_after":50,"quest_id":"quest_moral_echo_betrayers_child_grown","triggered_by":"quest_moral_chain_mercy_07","triggered_by_choice":0}`
    - row 35: `{"branch":"branch_mercy_road","min_days_after":35,"quest_id":"quest_moral_echo_medicine_shared_recovered","triggered_by":"quest_moral_chain_mercy_14","triggered_by_choice":0}`
    - row 36: `{"branch":"branch_mercy_road","min_days_after":40,"quest_id":"quest_moral_echo_convoy_haven_opened","triggered_by":"quest_moral_chain_mercy_06","triggered_by_choice":0}`
    - row 37: `{"branch":"branch_mercy_road","min_days_after":30,"quest_id":"quest_moral_echo_plague_secret_infection","triggered_by":"quest_moral_chain_mercy_12","triggered_by_choice":0}`
    - row 38: `{"branch":"branch_mercy_road","min_days_after":30,"quest_id":"quest_moral_echo_well_gratitude_refused","triggered_by":"quest_moral_chain_mercy_08","triggered_by_choice":0}`
    - row 39: `{"branch":"branch_mercy_road","min_days_after":35,"quest_id":"quest_moral_echo_patrol_reputation_spread","triggered_by":"quest_moral_chain_mercy_04","triggered_by_choice":0}`
    - row 40: `{"branch":"branch_mercy_road","min_days_after":30,"quest_id":"quest_moral_echo_shelter_vote_strained_rations","triggered_by":"quest_moral_chain_mercy_11","triggered_by_choice":0}`
    - row 41: `{"branch":"branch_iron_way","min_days_after":45,"quest_id":"quest_moral_echo_aldric_blockade_retaliation","triggered_by":"quest_moral_chain_iron_06","triggered_by_choice":0}`
    - row 42: `{"branch":"branch_iron_way","min_days_after":30,"quest_id":"quest_moral_echo_old_friend_farewell_note","triggered_by":"quest_moral_chain_iron_13","triggered_by_choice":2}`
    - row 43: `{"branch":"branch_iron_way","min_days_after":35,"quest_id":"quest_moral_echo_expulsion_deterrence_held","triggered_by":"quest_moral_chain_iron_07","triggered_by_choice":0}`
    - row 44: `{"branch":"branch_iron_way","min_days_after":30,"quest_id":"quest_moral_echo_strike_broken_fear_quota","triggered_by":"quest_moral_chain_iron_11","triggered_by_choice":2}`
    - row 45: `{"branch":"branch_iron_way","min_days_after":40,"quest_id":"quest_moral_echo_informant_applies_leverage","triggered_by":"quest_moral_chain_iron_04","triggered_by_choice":1}`
    - row 46: `{"branch":"branch_iron_way","min_days_after":50,"quest_id":"quest_moral_echo_lowfield_harvest_dividend","triggered_by":"quest_moral_chain_iron_08","triggered_by_choice":0}`
    - row 47: `{"branch":"branch_iron_way","min_days_after":35,"quest_id":"quest_moral_echo_varek_blood_debt_claim","triggered_by":"quest_moral_chain_iron_15","triggered_by_choice":0}`
    - row 48: `{"branch":"branch_iron_way","min_days_after":45,"quest_id":"quest_moral_echo_calla_camp_empty_ruin","triggered_by":"quest_moral_chain_iron_18","triggered_by_choice":1}`
    - row 49: `{"branch":"branch_listener_thread","min_days_after":35,"quest_id":"quest_moral_echo_defector_corroborates_truth","triggered_by":"quest_moral_chain_listen_11","triggered_by_choice":0}`
    - row 50: `{"branch":"branch_listener_thread","min_days_after":40,"quest_id":"quest_moral_echo_cartographer_water_cache_located","triggered_by":"quest_moral_chain_listen_15","triggered_by_choice":0}`
    - row 51: `{"branch":"branch_listener_thread","min_days_after":30,"quest_id":"quest_moral_echo_prophet_calendar_discrepancy","triggered_by":"quest_moral_chain_listen_09","triggered_by_choice":0}`
    - row 52: `{"branch":"branch_listener_thread","min_days_after":30,"quest_id":"quest_moral_echo_trader_ledger_censorship_threat","triggered_by":"quest_moral_chain_listen_07","triggered_by_choice":0}`
    - row 53: `{"branch":"branch_listener_thread","min_days_after":35,"quest_id":"quest_moral_echo_soldier_second_confession","triggered_by":"quest_moral_chain_listen_08","triggered_by_choice":0}`
    - row 54: `{"branch":"branch_listener_thread","min_days_after":45,"quest_id":"quest_moral_echo_doctors_notes_reinterpreted","triggered_by":"quest_moral_chain_listen_04","triggered_by_choice":0}`
    - row 55: `{"branch":"branch_listener_thread","min_days_after":50,"quest_id":"quest_moral_echo_librarian_memorial_preserved","triggered_by":"quest_moral_chain_listen_12","triggered_by_choice":0}`
    - row 56: `{"branch":"branch_broken_compact","min_days_after":35,"quest_id":"quest_moral_echo_kessler_exile_uncovered","triggered_by":"quest_moral_chain_betray_04","triggered_by_choice":1}`
    - row 57: `{"branch":"branch_broken_compact","min_days_after":30,"quest_id":"quest_moral_echo_poisoned_gift_reputation_drop","triggered_by":"quest_moral_chain_betray_06","triggered_by_choice":1}`
    - row 58: `{"branch":"branch_broken_compact","min_days_after":25,"quest_id":"quest_moral_echo_crisis_gambit_warlord_respect","triggered_by":"quest_moral_chain_betray_11","triggered_by_choice":1}`
    - row 59: `{"branch":"branch_broken_compact","min_days_after":40,"quest_id":"quest_moral_echo_voss_blackmail_exposed","triggered_by":"quest_moral_chain_betray_08","triggered_by_choice":2}`
    - row 60: `{"branch":"branch_broken_compact","min_days_after":35,"quest_id":"quest_moral_echo_pell_hostage_border_locked","triggered_by":"quest_moral_chain_betray_16","triggered_by_choice":1}`
- `gossip_propagation`: object[2]
- `faction_reactions`: object[2]

## `Assets/StreamingAssets/Data/moral_choice_quests.json`
- Bytes: 141,249; SHA-256: `1c84bf9e37036b9247ba0f48e1a14a8d1b497a0bcc63e0e506f23f8bb399b4e1`
- Root keys: `quests, schema_version`
- `quests`: list[68]; union fields: `category, choices, discovery, display_name, id, location_id, max_day, min_day, trigger`
  - row 1: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave the child everything in the pack. Came away with a drawing of a house.","label":"Give all your food","moral_delta":10,"outcome_text":"You hand over everything you are carrying. They eat half, fast, and put the rest inside their coat. Then they give you a charcoal drawing of a house, because they think a trade is polite."},{"empathy_delta":1,"epitaph":"Split my rations with the child at the crossing. Kept enough to walk home on.","label":"Give half your food","moral_delta":5,"outcome_text":"They eat slowly and keep both eyes on your hands the whole time, in case the rest of it …`
  - row 2: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Emptied the pack in a stairwell for a family of four. They tried to give a share back.","label":"Give all your supplies","moral_delta":15,"outcome_text":"You empty the pack onto the blanket. The father does not thank you; he starts dividing it into four piles and then, after a moment, into five, and pushes one back toward you."},{"empathy_delta":1,"epitaph":"Left half my stores with the family in the stairwell. Children ate first.","label":"Give half your supplies","moral_delta":8,"outcome_text":"Half of what you carry stays on the blanket. They portion it out before you are throug…`
  - row 3: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Splinted a stranger's leg in an alley. He insists he owes me.","label":"Give medical supplies","moral_delta":8,"outcome_text":"You splint it against a length of pipe and use most of the bandages. He talks the whole time about what he will owe you and where to find him, which is how he keeps from screaming."},{"empathy_delta":1,"epitaph":"Left food with the man in the alley. Left the leg as I found it.","label":"Give food instead","moral_delta":5,"outcome_text":"No bandages to spare, so you leave food and water within reach. It steadies him. The leg is still broken when you go."},{"…`
  - row 4: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave a stranger my whole canteen. Traded it for the way to clean water.","label":"Give all your water","moral_delta":7,"outcome_text":"You hand over your canteen and they drink it dry in one go, then sit down hard. When they can talk again they draw you a map to a spring that runs clean, and make you repeat the turnings back to them."},{"empathy_delta":1,"epitaph":"Poured off half my water at the perimeter. Enough to keep them walking.","label":"Give half your water","moral_delta":4,"outcome_text":"You pour off half. They ration it in sips without being asked, which tells you how l…`
  - row 5: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Spent food and medicine on a dying woman. She left me her family's door.","label":"Give food and medicine","moral_delta":12,"outcome_text":"You spend rations and medicine on someone who will not need them long. She talks for an hour, mostly about her sister, and at the end of it tells you where the family shelter is and which key opens it."},{"empathy_delta":1,"epitaph":"Left supplies with the old woman by the wall. Stayed while she ate.","label":"Give some supplies","moral_delta":6,"outcome_text":"You leave what you can afford at her side and stay while she eats some of it. She is…`
  - row 6: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Emptied the whole medical kit on a delivery in the dark. The child has my name.","label":"Give all your medical supplies","moral_delta":18,"outcome_text":"You put everything sterile you own on the table and boil what you cannot. It takes most of the night, and it works: a small, furious, breathing baby. She asks what you are called, and gives the child your name to carry."},{"empathy_delta":1,"epitaph":"Gave up antiseptic and clean cloth for a field delivery. Both came through.","label":"Give some supplies","moral_delta":9,"outcome_text":"Enough antiseptic for hands and cord, and c…`
  - row 7: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Patched a raider who asked for quarter. Bought their crew's patrol times.","label":"Give medical supplies","moral_delta":5,"outcome_text":"You pack the wound and strap it. They watch you work, decide you are worth paying, and give you their crew's patrol times for the next nine days.","set_flag":"flag_spared_raider"},{"empathy_delta":1,"epitaph":"Fed a bleeding raider instead of patching them. They ate watching the road.","label":"Give food","moral_delta":3,"outcome_text":"No bandages, just food. They eat it one-handed and fast, and never once stop watching the road behind you.","s…`
  - row 8: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Kept a Peacekeeper alive at the bend. Came away with their ciphers and a name to use.","label":"Give all your supplies","moral_delta":14,"outcome_text":"You work the leg and leave them everything you can carry out without. They pay in the only currency they have on them: safehouse ciphers, patrol frequencies, and a name to use at any of their posts."},{"empathy_delta":1,"epitaph":"Stabilized a Peacekeeper and moved on. They chose not to wait for pickup.","label":"Give some supplies","moral_delta":7,"outcome_text":"Enough to stop the bleeding and steady them. They thank you formally…`
  - row 9: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Fed the archivist and found them lenses. They gave me a survey of the valley.","label":"Give food and spare glasses","moral_delta":16,"outcome_text":"Food first, then the spare lenses. They put them on, look at the far wall for a while without saying anything, then read you a pre-war survey of the whole valley and press the document into your hands."},{"empathy_delta":1,"epitaph":"Fed the archivist. Wrote down the coordinates they knew by heart.","label":"Give food only","moral_delta":8,"outcome_text":"You leave food and no lenses. They recite coordinates from memory instead, check…`
  - row 10: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Paid the toll to the warlord by the fire pit. The road stayed open.","label":"Give them supplies","moral_delta":6,"outcome_text":"You put the supplies down by the tins. He looks at what you brought, not at you, and says the road stays open, and it does."},{"empathy_delta":0,"epitaph":"Refused the warlord's toll to his face. He only kept sorting brass.","label":"Give them nothing","moral_delta":0,"outcome_text":"You tell him no. He goes back to sorting brass without any change in his face, which is somehow more of an answer than shouting would have been."},{"empathy_delta":0,"epitap…`
  - row 11: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave a dying specialist all my medicine. She wrote out a treatment schedule worth more.","label":"Give all your medicine","moral_delta":15,"outcome_text":"You hand over the whole medicine roll. Once she can hold a pen she writes out a chelation schedule better than anything your shelter has, and then keeps writing until the light goes."},{"empathy_delta":1,"epitaph":"Split my medicine with the specialist. She left the formula on a beam.","label":"Give some medicine","moral_delta":8,"outcome_text":"You give her part of the roll. It buys her steady hands for an afternoon, which she s…`
  - row 12: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave live seed and feed to the sterile rows. Two-fifths of the harvest is mine on paper.","label":"Give seeds and fertilizer","moral_delta":20,"outcome_text":"You hand over live seed stock and the chemical feed to go with it. He works out your share on the spot, aloud, twice, and writes two-fifths of every harvest against your name where the others can see it."},{"empathy_delta":1,"epitaph":"Gave what seed I could spare. My share was written down honestly.","label":"Give some supplies","moral_delta":10,"outcome_text":"Part of the seed, part of the feed. He revises his numbers down …`
  - row 13: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat through an old man's stories. He gave me the way into a bunker.","label":"Hear the story","moral_delta":8,"outcome_text":"You sit through a story about a water main and the winter it burst. In the middle of it, without changing tone, he gives you the way into an intact bunker under the east junction."},{"empathy_delta":3,"epitaph":"Stayed for every story he had. He trusted me with a cache nobody else claimed.","label":"Stay for all of it","moral_delta":12,"outcome_text":"You stay until the fire is down to coals and he is down to the names of people who signed off on things. At…`
  - row 14: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat with her until the talking ran out. Came away with a toy meant for smaller hands.","label":"Sit with her","moral_delta":10,"outcome_text":"You sit on the floor near the stove and stay while she talks, some of it about the girl and some of it about nothing at all. When the rocking finally slows, she wraps a small wind-up toy in a cloth and puts it in your hands, and does not explain it, and you do not ask."},{"empathy_delta":3,"epitaph":"Kept quiet in her room until the room settled. Nothing got fixed.","label":"Stay, but keep quiet","moral_delta":5,"outcome_text":"You take the…`
  - row 15: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat through the full after-action report. My rifle shoots true again.","label":"Accept the debrief","moral_delta":9,"outcome_text":"You take the whole report, contact by contact, the way they need to give it. When the last name is accounted for, they pick up your rifle, check the sights against their thumbnail, and put right what the road knocked loose."},{"empathy_delta":3,"epitaph":"Picked holes in their report. The holes were real, and what was left was worth having.","label":"Cross-examine the report","moral_delta":4,"outcome_text":"You press the gaps: who held the stairwell, …`
  - row 16: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard a kid's whole theory of the stars. There's a paper sky somewhere with my name in it.","label":"Hear the whole theory","moral_delta":12,"outcome_text":"You crouch down and take the whole theory, every planet and comet of it, in order. When it is over they draw you a map of the sky on paper scrap and write your name on a cluster, because now it belongs to you."},{"empathy_delta":3,"epitaph":"Nodded through a kid's star theory. It made their whole day.","label":"Nod along","moral_delta":6,"outcome_text":"You say 'go on' and 'is that so' at the right places, and it is enough. Th…`
  - row 17: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard every black-tag name read out loud. Left with the key to a pharmacy lockbox.","label":"Hear the names","moral_delta":14,"outcome_text":"You stay for all of it, every name and the reason after it, until the solvent rag dries out. When the list is done they sit lighter in the chair, and they give you a small key and tell you which pharmacy's lockbox it fits."},{"empathy_delta":3,"epitaph":"Told the surgeon the math was sound. Neither of us called it right.","label":"Tell them the math held","moral_delta":7,"outcome_text":"You tell them the numbers were the numbers, that the bo…`
  - row 18: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Stood through the whole sermon. They blessed my weapon like I was one of theirs.","label":"Hear the sermon","moral_delta":6,"outcome_text":"You stand through the whole sermon, fire and judgment and the short roll of the saved. They take your stillness for devotion, and at the end they lay a hand on your weapon and ask the ash to pass it by."},{"empathy_delta":3,"epitaph":"Argued doctrine with the chaplain. Best talk they'd had in months.","label":"Argue the theology","moral_delta":3,"outcome_text":"You take the doctrine apart piece by piece, and they light up like it is a festival…`
  - row 19: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Walked the overpass and heard out the math. Now I know the way into the maintenance level.","label":"Follow the math","moral_delta":11,"outcome_text":"You walk the span with them, twist by twist, until the failure reads like a sentence you could recite. Satisfied that someone finally checked their work, they tell you about a maintenance level underneath that nobody else remembers."},{"empathy_delta":3,"epitaph":"Nodded through the engineer's math. They only needed it witnessed.","label":"Take their word for it","moral_delta":5,"outcome_text":"You nod through the equations without …`
  - row 20: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard the whole warning, every mad word. Left with a filter mask that works.","label":"Hear the warning out","moral_delta":9,"outcome_text":"You stand there and let them get it all out: the shapes, the timing, which road. Once it is said to someone who didn't walk off, they steady enough to press a homemade filter mask on you, the good kind of crude."},{"empathy_delta":3,"epitaph":"Pressed them for proof. They had none, and thanked me for asking.","label":"Ask for proof","moral_delta":4,"outcome_text":"You ask what they saw, exactly, from where, and for how long. They have nothing…`
  - row 21: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard her whole story. She gave me the last photo to carry forward.","label":"Listen sympathetically","moral_delta":9,"outcome_text":"You hear it through. At the end she gives you the last photograph, kept whole. 'Someone should carry it forward.'"},{"empathy_delta":3,"epitaph":"Listened to the lover's tale. She showed me the last photo.","label":"Listen politely","moral_delta":4,"outcome_text":"You nod at the right beats. She keeps one photograph and lets you see it."},{"empathy_delta":0,"epitaph":"Left her to her photographs. The fire was doing its slow work.","label":"Interrupt…`
  - row 22: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat through the teacher's whole lesson. I can read the road signs now.","label":"Listen attentively","moral_delta":15,"outcome_text":"Hours pass like minutes. You leave reading the signage on the dead roads."},{"empathy_delta":3,"epitaph":"Half-listened to the lesson. She left me homework. I did it.","label":"Listen, but bored","moral_delta":7,"outcome_text":"You fidget through half. She assigns you homework: a sign to read on your way out."},{"empathy_delta":0,"epitaph":"Walked out of the schoolroom. Survived fine. Know less.","label":"Interrupt or leave","moral_delta":0,"outcome…`
  - row 23: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard the thief out. Let him keep the light. He's been useful since.","label":"Hear the explanation","moral_delta":6,"outcome_text":"His sister is coughing somewhere. You listen, then let him keep the light. He becomes useful after that."},{"empathy_delta":3,"epitaph":"Questioned the thief's story. Half held up. Half came back.","label":"Listen, skeptical","moral_delta":3,"outcome_text":"You question the story. Half of it holds. He returns half of what he took."},{"empathy_delta":0,"epitaph":"Turned the thief in. The camp was thorough. I didn't stay to learn the rest.","label":"Re…`
  - row 24: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard the prophet's vision. The message was madness with a map in it, and the map was real.","label":"Hear the vision","moral_delta":18,"outcome_text":"The vision is nonsense stitched with coordinates. You use the coordinates. They're real."},{"empathy_delta":3,"epitaph":"Argued the prophet's vision into sense. Kept the parts that held.","label":"Listen skeptically","moral_delta":9,"outcome_text":"You sort the signal from the noise with him. Half the message survives your doubts, and half of that is useful."},{"empathy_delta":0,"epitaph":"Left the prophet to his stars. They talk o…`
  - row 25: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat with a woman beside fresh earth. Came away wearing her partner's coat.","label":"Sit with her","moral_delta":14,"outcome_text":"You sit down in the dirt next to her and stay. Nothing gets better while you are there. When the light starts to go, she stands, pulls his coat off the pile, and puts it in your arms. 'It's a good coat. He'd be angry if it went in the ground.'"},{"empathy_delta":2,"epitaph":"Told a woman her partner was dead because she needed to hear it. She packed before dark.","label":"Say it out loud","moral_delta":7,"outcome_text":"You say the word she has been …`
  - row 26: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked a kid up out of a nightmare. He paid me in a salvage map.","label":"Wake him slowly","moral_delta":16,"outcome_text":"You say his name from a step back until he comes up out of it. He stares at the walls a long time, checking them. Before you leave he presses a folded scrap into your hand, a map of where he has seen salvage, to pay for the waking."},{"empathy_delta":2,"epitaph":"Shook a kid awake to stop the screaming. He shook longer than he had screamed.","label":"Shake him awake","moral_delta":8,"outcome_text":"You grab his shoulder and shake hard. He comes up gasping a…`
  - row 27: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked a gut-shot man through the worst of it. He says he owes me his life.","label":"Keep talking to him","moral_delta":10,"outcome_text":"You crouch and keep your voice on him, your name, his name, the weather, anything, and the shock waits a while. Before he goes under he tells you he owes you his life, and to collect if he lives."},{"empathy_delta":2,"epitaph":"Bound a man's belly wound. Left the rest of it where I found it.","label":"Apply standard first aid","moral_delta":5,"outcome_text":"You press the wound closed and bind it tight. The bleeding slows. Whatever is happeni…`
  - row 28: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat on a cellar floor and talked someone through the dark. They sorted my pack to say thank you.","label":"Talk them through it","moral_delta":12,"outcome_text":"You sit on the floor where they can see you and keep talking, small dull things, until their breathing evens out some. Nothing is fixed, they are still down there when you leave. Later you find your pack repacked and tidy, the only thanks they could manage."},{"empathy_delta":2,"epitaph":"Left a light stick with someone afraid of the dark.","label":"Leave a light","moral_delta":6,"outcome_text":"You crack a chem light an…`
  - row 29: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat up all night with someone coming off the drug. Morning came anyway.","label":"Sit with them through it","moral_delta":8,"outcome_text":"You stay through the worst hours, water when they can hold it, a hand on the shoulder when the shaking peaks. Come morning they eat half a ration and keep it down. It is the first day in a long time they end without the drug."},{"empathy_delta":2,"epitaph":"Handed a shaking person a pill full of nothing. It worked for an hour.","label":"Give them a dummy dose","moral_delta":4,"outcome_text":"You give them a capsule of flour and call it the re…`
  - row 30: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Told a survivor their living meant nothing either way. They gave me a radio frequency for it.","label":"Tell them someone had to be the one","moral_delta":15,"outcome_text":"You tell them that in a dying this size, somebody has to be left over, and it means nothing about them. They sit with that a while. Then they teach you a guarded radio frequency, something useful to carry instead."},{"empathy_delta":2,"epitaph":"Agreed with a man that he should be dead. It was what he wanted to hear.","label":"Agree the odds were bad","moral_delta":8,"outcome_text":"You agree with them. The o…`
  - row 31: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat through an old man's whole archive. He paid me in a good knife.","label":"Listen to all of it","moral_delta":14,"outcome_text":"You sit until your legs ache, through flood marks and filter schedules and the names of the dead who taught him. When it is done he gives you his folding knife, good steel, like a receipt for the hours."},{"empathy_delta":2,"epitaph":"Took the useful parts of an old man's memory. Left the rest.","label":"Take only what keeps you alive","moral_delta":7,"outcome_text":"You cut him down to the parts that keep a body breathing, water, rot, weather. He gi…`
  - row 32: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Woke a sleeper out of a bad dream. They split their breakfast with me.","label":"Wake them gently","moral_delta":10,"outcome_text":"You say their name from where their hands cannot reach you, palms open, until they come up out of it. They check the room twice before they believe it. At breakfast they push half their portion your way without a word."},{"empathy_delta":2,"epitaph":"Hauled a dreamer awake the hard way. They watch me now.","label":"Jolt them awake","moral_delta":5,"outcome_text":"You grab a shoulder and shake. They come up swinging. Afterward they sleep with their ba…`
  - row 33: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked two hours with someone starving for it. They paid me in food.","label":"Stay and talk","moral_delta":12,"outcome_text":"You trade two hours of nothing much, weather, rations, a dog one of you used to know. When you get up to leave they press extra rations on you, as if talk were something you can pay off."},{"empathy_delta":2,"epitaph":"Told a lonely person their loneliness was normal. They wrote it down.","label":"Tell them three weeks is normal now","moral_delta":6,"outcome_text":"You tell them three quiet weeks is about standard these days. They write it down, like a fa…`
  - row 34: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Stood by while someone screamed themselves empty. Then I heard the whole list of their dead.","label":"Wait it out nearby","moral_delta":9,"outcome_text":"You stand off at a distance until the shouting wears itself down. Afterward they tell you the whole list of what they lost, every name, like reading a manifest out loud."},{"empathy_delta":2,"epitaph":"Talked a scream down to a shout. The anger stayed where it was.","label":"Talk them down","moral_delta":4,"outcome_text":"You keep your voice low and even until theirs drops to meet it. The fists stay clenched. That part you cann…`
  - row 35: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Told a dreamer the dream would hold. Now I carry stones for it.","label":"Tell them it can stand","moral_delta":18,"outcome_text":"You go over the drawing and say so, it can stand. Before you are finished saying it you are holding a work detail with your name on it."},{"empathy_delta":2,"epitaph":"Read a rebuilding plan. Half of it was true.","label":"Look the plan over","moral_delta":9,"outcome_text":"You read the whole thing. Some of it would bear weight. You say which parts, and leave the rest unsaid."},{"empathy_delta":0,"epitaph":"Refused to look at a plan for putting a roof…`
  - row 36: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked a dying arithmetic down to one day. They agreed to the day.","label":"Talk about tomorrow only","moral_delta":16,"outcome_text":"You don't argue the winter. You argue for tomorrow, one day, that is all anyone has to get through. They agree to that much. It is not hope, but it is a time you both set."},{"empathy_delta":2,"epitaph":"Gave a despairing person the standard words. They were worth what they cost.","label":"Offer the usual words","moral_delta":8,"outcome_text":"You say the things people say. They hear each one land and stay where it lands. Nothing moves."},{"empat…`
  - row 37: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Marked a grave that had no name on it. Left the coords in the log so nobody digs there twice.","label":"Mark the grave","moral_delta":12,"outcome_text":"You write the coordinates in your log and set a length of pipe upright over the spot, wired to a strip of cloth. The next patrol through here will see it and walk around."},{"empathy_delta":0,"epitaph":"Walked around a grave with no marker. It was already covered. That had to be enough.","label":"Walk around it","moral_delta":0,"outcome_text":"You give the dip a wide berth and keep moving. It is already filled. There is nothing in i…`
  - row 38: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried a burned body under its own wall. Painted a stripe so nobody tracks it home.","label":"Cover and mark it","moral_delta":10,"outcome_text":"You kick down the half-standing partition so it falls over the spot, then paint a stripe on the wall by the door. The dust stays where it is, and the next crew knows not to sweep there."},{"empathy_delta":0,"epitaph":"Walked past a burned body. There was nothing left to bury that the fire had not already done.","label":"Keep your distance","moral_delta":0,"outcome_text":"You cross the room wide of it and go on. Burned that thoroughly, it i…`
  - row 39: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Pulled a body out of the intake. Lost my gloves and my appetite. Three camps drink tomorrow and don't know why.","label":"Drag it clear","moral_delta":8,"outcome_text":"You rope the ankles and haul from the bank, and it takes most of an hour before the grate runs clean. You wash your arms to the elbow and lose your gloves to it. The downstream camps never hear about any of this."},{"empathy_delta":0,"epitaph":"Found the intake fouled by a body. Left a warning scratched on the post and kept my own water.","label":"Mark it foul and go","moral_delta":0,"outcome_text":"You scratch a war…`
  - row 40: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried a child and a toy truck in frozen ground. Cost me the daylight and chipped the shovel. A stranger nodded at me from the road.","label":"Bury them with the truck","moral_delta":20,"outcome_text":"The ground outside is frozen two knuckles deep, and digging takes you the rest of the daylight and chips your shovel. You put the truck in with them. A scavenger passing on the road stops, watches, and gives you one nod before moving on."},{"empathy_delta":0,"epitaph":"Shut the door on a child's bones. They'd kept this long. They'll keep.","label":"Close the door","moral_delta":0,"out…`
  - row 41: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Paced out a pit of forty or more and put it on the map as ground you don't dig. Left a ration on a stone. Can't say why.","label":"Record it and leave a marker","moral_delta":18,"outcome_text":"You walk the edges, pace out the corners, and enter it on your network map as a place to be left alone. You set one of your own rations on a flat stone at the head of the pit. It will be gone by morning, but that is not the point."},{"empathy_delta":0,"epitaph":"Walked around a full pit. The work there was done long before I came.","label":"Walk the perimeter","moral_delta":0,"outcome_text":"…`
  - row 42: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Cut a hanged stranger down off the gantry and put them under the roadbed. Left the rope on the grave. Didn't want it.","label":"Cut them down and bury them","moral_delta":14,"outcome_text":"You climb the gantry, saw through the braid, and lower the body hand over hand. Digging below the roadbed takes the rest of the day. You coil the cut end of the rope and leave it on the grave, because it is theirs and you do not want it."},{"empathy_delta":0,"epitaph":"Walked under a hanged body on the gantry. Looked up once. Kept walking.","label":"Keep to your route","moral_delta":0,"outcome_te…`
  - row 43: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried the one from the locked closet in the garden soil, coat and all. Slept in the house after. It was just a house.","label":"Carry them out and bury them","moral_delta":12,"outcome_text":"You wrap the bones in a bedsheet and dig in the side yard, where the soil is soft from an old garden. The coat goes in too. After, the house is only a house, and you sleep in it without thinking about the closet."},{"empathy_delta":0,"epitaph":"Left the one in the closet where they chose to be. Closed two doors on it.","label":"Shut the door again","moral_delta":0,"outcome_text":"They sealed th…`
  - row 44: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Waded in and pulled a floater onto the walk, buried it in the fill. The water's still bad. It's less bad.","label":"Haul it out and bury it","moral_delta":10,"outcome_text":"You wade in, get a line under the arms, and drag it up onto the concrete walk. Digging into the rubble fill takes an hour. The water will not get cleaner on its own, but the worst of it is now in the ground instead of in the flow."},{"empathy_delta":0,"epitaph":"Chalked a floater's stretch as foul water and walked on. Had enough in the bottles to afford that.","label":"Mark it and move on","moral_delta":0,"outco…`
  - row 45: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Finished a stranger's cremation with a shovel and the wind. Kicked the pit in after. Their fire had quit; mine didn't.","label":"Finish it by hand","moral_delta":16,"outcome_text":"You crush what the fire left with the flat of your shovel and scatter it past the pit, a shovelful at a time, into the wind. It takes the better part of an hour. When the pit is empty you kick the brick lining in after it."},{"empathy_delta":0,"epitaph":"Stepped around a half-finished burn pit. Nothing left in it that could hurt anyone.","label":"Leave the pit","moral_delta":0,"outcome_text":"There is not…`
  - row 46: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Cut the wire off a prisoner's wrists and buried them outside the fence. Left the cell open. Nothing there to keep.","label":"Cut the wire and bury them","moral_delta":18,"outcome_text":"You work the wire off the wrists, carry the body out past the wire fence, and dig where the ground is clear of concrete. It takes the day. You leave the cell block standing open behind you, because nothing in it needs keeping anymore."},{"empathy_delta":0,"epitaph":"Closed the cell door on a wired body. The block keeps its own records.","label":"Leave the cell as found","moral_delta":0,"outcome_text"…`
  - row 47: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried the one from the sealed room with their letter on their chest, unread. It wasn't written to me.","label":"Bury them with the letter unread","moral_delta":14,"outcome_text":"You dig out back, lay the body in, and set the folded sheet on the chest without opening it. Whatever it says, it was not addressed to you. You fill the grave and leave the tape on the door for the next one to find."},{"empathy_delta":0,"epitaph":"Left the sealed room sealed. They arranged it themselves. I just knocked.","label":"Leave the room sealed","moral_delta":0,"outcome_text":"They built this closur…`
  - row 48: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Dug one trench behind the block, two days, broke the shovel, finished with a shelf. Didn't count them. Marked it with a door.","label":"Dig a common grave","moral_delta":22,"outcome_text":"Two full days of daylight, one long trench in the lot behind the block, and your shovel gives out on the second morning so you finish with a length of shelving. You carry them down one at a time and do not count them. When it is done you pile the dirt high and mark it with a door stood on end."},{"empathy_delta":0,"epitaph":"Walked around a block where everyone died at once. Drew it on the map so …`
  - row 49: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Authorized thermal proximity. Acquired updated topographical data.","label":"Authorize proximity","moral_delta":10,"outcome_text":"You permit entry. They transfer a portion of their caloric reserves and provide updated topographical data for the northern route."},{"empathy_delta":1,"epitaph":"Authorized maximum-range thermal proximity. Maintained combat readiness.","label":"Authorize at maximum range","moral_delta":5,"outcome_text":"You assign them the furthest functional radius. Both units maintain readiness. No data is exchanged."},{"empathy_delta":0,"epitaph":"Denied thermal pro…`
  - row 50: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Provided comprehensive aid to compromised unknown. Unit integrated into operations.","label":"Provide comprehensive aid","moral_delta":14,"outcome_text":"You expend medical supplies and carry their mass. They integrate into your operation and provide ongoing tactical analysis."},{"empathy_delta":1,"epitaph":"Provided basic triage. Directed unit to medical facility.","label":"Provide basic triage","moral_delta":7,"outcome_text":"You apply standard coagulation protocols and direct them to the nearest known medical facility."},{"empathy_delta":0,"epitaph":"Bypassed compromised unknown…`
  - row 51: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Executed standard exchange. Acquired coordinates for stable logistics hub.","label":"Execute standard exchange","moral_delta":8,"outcome_text":"You meet the stated caloric price without friction. The asset provides the coordinates of a stable trading hub as a bonus."},{"empathy_delta":1,"epitaph":"Negotiated material deficit. Secured minor caloric advantage.","label":"Negotiate deficit","moral_delta":4,"outcome_text":"You aggressively contest the valuation. The asset yields slightly to expedite the transaction."},{"empathy_delta":0,"epitaph":"Bypassed mobile logistics node. Conserv…`
  - row 52: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Reunited juvenile with primary unit. Secured positive faction standing.","label":"Reunite with unit","moral_delta":16,"outcome_text":"You expend significant operational time locating their camp. The family unit registers a permanent positive standing for your faction."},{"empathy_delta":1,"epitaph":"Transferred unattached juvenile to settlement intake. Identification logged.","label":"Transfer to local authority","moral_delta":8,"outcome_text":"You deliver the asset to a functioning settlement's intake officer. The officer logs your ID."},{"empathy_delta":0,"epitaph":"Bypassed unat…`
  - row 53: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Maintained concealment for AWOL combatant. Acquired weapon maintenance data.","label":"Maintain concealment","moral_delta":12,"outcome_text":"You omit their presence from your logs. In exchange, the combatant provides advanced weapon maintenance training."},{"empathy_delta":1,"epitaph":"Processed tactical data from AWOL combatant prior to release.","label":"Process tactical data","moral_delta":6,"outcome_text":"You extract their unit's patrol routes and deployment status before allowing them to pass."},{"empathy_delta":0,"epitaph":"Bypassed AWOL combatant. Ignored factional dispute…`
  - row 54: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Processed actionable intelligence. Integrated broker into logistics network.","label":"Process intelligence","moral_delta":18,"outcome_text":"The data details hostile rotation schedules for the eastern sector. The broker integrates into your logistics network."},{"empathy_delta":1,"epitaph":"Verified data integrity. Extracted value from confirmed intelligence.","label":"Verify data integrity","moral_delta":9,"outcome_text":"You cross-reference the intelligence before acting. The verified portions prove highly valuable."},{"empathy_delta":0,"epitaph":"Declined intelligence transacti…`
  - row 55: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Integrated pre-war clearing tactics. Acquired veteran combatant asset.","label":"Integrate tactical data","moral_delta":15,"outcome_text":"You drill urban clearing tactics until nightfall. Their mechanical memory is flawless. They join your movement the next day."},{"empathy_delta":1,"epitaph":"Extracted summary tactical briefing. Combatant retained operational independence.","label":"Extract summary data","moral_delta":8,"outcome_text":"You accept a rapid briefing on sight alignment and proceed. The combatant correctly assesses you as a short-term asset."},{"empathy_delta":0,"epit…`
  - row 56: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Executed concealment for pursued asset. Acquired high-caloric ration.","label":"Execute concealment","moral_delta":15,"outcome_text":"You divert the pursuers with false telemetry. The asset departs before dawn, leaving a high-caloric ration on your pack."},{"empathy_delta":1,"epitaph":"Authorized passive cover for pursued asset. Sector bypassed by trackers.","label":"Authorize minimal cover","moral_delta":8,"outcome_text":"You allow them to utilize your blind spot but deny active misdirection. The pursuers eventually bypass the sector."},{"empathy_delta":0,"epitaph":"Denied entry t…`
  - row 57: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Mirrored non-verbal asset signals. Acquired forward scouting capability.","label":"Mirror signals and observe","moral_delta":10,"outcome_text":"You return neutral gestures. They establish a forward scouting position and clear three ambush points over the next transit cycle."},{"empathy_delta":1,"epitaph":"Maintained passive observation of non-verbal asset. Contact broken cleanly.","label":"Maintain passive observation","moral_delta":5,"outcome_text":"You monitor their vector without engagement. They eventually break contact at a major intersection."},{"empathy_delta":0,"epitaph":"A…`
  - row 58: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Executed heavy extraction on subterranean asset. Acquired salvage yield.","label":"Execute extraction","moral_delta":15,"outcome_text":"You expend heavy labor to pry the grate. The trapped asset shares half their subterranean salvage yield in compensation."},{"empathy_delta":1,"epitaph":"Confirmed subterranean presence. Marked coordinates for extraction teams.","label":"Confirm presence","moral_delta":8,"outcome_text":"You signal back to confirm their coordinates, then mark the location for better-equipped logistics units."},{"empathy_delta":0,"epitaph":"Bypassed subterranean distr…`
  - row 59: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Authorized temporary tool transfer. Tool upgraded. Secondary cache acquired.","label":"Authorize transfer","moral_delta":11,"outcome_text":"The tool is returned within 48 hours, upgraded with synthetic grips. They also provide coordinates to a secondary, smaller cache."},{"empathy_delta":1,"epitaph":"Transferred secondary degraded tool. Operation executed.","label":"Transfer secondary tool","moral_delta":4,"outcome_text":"You hand over a degraded spare. The unit logs the lack of trust but executes their operation."},{"empathy_delta":0,"epitaph":"Denied tool transfer request. Equipm…`
  - row 60: `{"category":"trust","choices":[{"empathy_delta":4,"epitaph":"Executed delivery protocol. Seal intact. Granted high-priority node access.","label":"Execute delivery protocol","moral_delta":20,"outcome_text":"You carry the envelope through hostile sectors. The Northern Node processes the intact seal and grants you permanent high-priority access."},{"empathy_delta":2,"epitaph":"Breached courier seal. Delivered compromised intel. Zero reward acquired.","label":"Break seal and deliver","moral_delta":8,"outcome_text":"You analyze the data before delivery. The receiving node detects the breach and processes the intel with zero reward."},{"empathy_d…`
  - row 61: `{"category":"share","choices":[{"empathy_delta":2,"epitaph":"Executed joint extraction with juvenile forager. Extracted metals divided.","label":"Execute joint extraction","moral_delta":12,"outcome_text":"You provide caloric support and apply leverage to the slab. The extracted metals are divided. The juvenile transfers a non-functional gear as 'luck.'"},{"empathy_delta":1,"epitaph":"Transferred minimal calories to juvenile forager. Inefficient extraction continued.","label":"Transfer minimal calories","moral_delta":5,"outcome_text":"You transfer a low-value ration. The juvenile consumes it without ceasing their inefficient extraction attemp…`
  - row 62: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Read an old person's letters aloud, all of them. They said the record held.","label":"Read them all aloud","moral_delta":10,"outcome_text":"You read the whole bundle: love notes, rent receipts, a letter never sent. None of it would sell for a bullet to anyone but them. At the end they sit back and say, 'It's true, then. We kept it right,' like that settles an account."},{"empathy_delta":1,"epitaph":"Read two of the letters. That was enough for them.","label":"Read a couple","moral_delta":4,"outcome_text":"You read two, enough to prove the bundle is only other people's ordinary liv…`
  - row 63: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Authorized structural entry for refugee. Acquired combustion fuel payment.","label":"Authorize structural entry","moral_delta":8,"outcome_text":"You permit entry. The unit powers down in a corner. At dawn, they have departed, leaving a bundle of dry combustion fuel as payment.","set_flag":"flag_sheltered_refugee"},{"empathy_delta":1,"epitaph":"Authorized perimeter cover for refugee. Thermal drop survived.","label":"Authorize perimeter cover","moral_delta":3,"outcome_text":"You allow them to utilize the exterior structural overhang. They survive the thermal drop and depart at first …`
  - row 64: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried the surveyor by the path, canteen and all. Took the logbook. If I get near sector nine, their sister gets it.","label":"Bury them, keep the logbook","moral_delta":8,"outcome_text":"You dig a grave a few steps off the path and tuck the canteen in with them. The logbook goes in your pack. Grid 47 comes off your route tonight, and if you are ever near sector nine you will knock and hand over the book."},{"empathy_delta":1,"epitaph":"Copied one line off a dead surveyor and struck Grid 47 off my route. Left them with their book open.","label":"Note the grid, leave them","moral_del…`
  - row 65: `{"category":"comfort","choices":[{"empathy_delta":2,"epitaph":"Spent my medkit on a man who said he was done. He paid in directions.","label":"Patch the wound","moral_delta":10,"outcome_text":"You spend gauze and powder on a stranger. The bleeding slows. In return he tells you which streets on the next block are clear and which are watched."},{"empathy_delta":1,"epitaph":"Threw a bandage to a bleeding man. He did the rest himself.","label":"Toss them a bandage","moral_delta":5,"outcome_text":"You throw a bandage pack into his lap. He starts wrapping his own leg with the patience of someone who has done it before."},{"empathy_delta":0,"epitap…`
  - row 66: `{"category":"share","choices":[{"empathy_delta":0,"epitaph":"Butchered a collared rad-dog for meat and said nothing.","label":"Butcher it. Meat is meat.","moral_delta":-8,"outcome_text":"You do the work quickly and do not look at the collar. The shelter eats for four days. Nobody asks what it was."},{"empathy_delta":1,"epitaph":"Gave a working dog a clean end instead of a meal.","label":"End it clean and bury it with the collar","moral_delta":4,"outcome_text":"One blow, then digging. The collar goes in with the animal. The empty pot goes on the table anyway."},{"empathy_delta":0,"epitaph":"Reported a collared catch on the net before clearing…`
  - row 67: `{"category":"share","choices":[{"empathy_delta":0,"epitaph":"Served questionable fowl to stretch the rations.","label":"Cook it through and stretch the ration","moral_delta":-5,"outcome_text":"Char on the outside, caution on the inside. Most will be fine. Most is a word you lean on."},{"empathy_delta":0,"epitaph":"Burned contaminated fowl and flagged the ground.","label":"Burn it and mark the site","moral_delta":3,"outcome_text":"The fowl burns quick and greasy. You tie a rag on the snare post so the others know the ground is bad."},{"empathy_delta":0,"epitaph":"Hid contaminated meat for a desperate day.","label":"Save it for the worst week"…`
  - row 68: `{"category":"share","choices":[{"empathy_delta":0,"epitaph":"Boiled a twitching squirrel rather than waste it.","label":"In the pot. Waste is the real cruelty.","moral_delta":-2,"outcome_text":"It is done before the water boils. The children get broth and no explanation."},{"empathy_delta":1,"epitaph":"Gave the children a dying animal to love.","label":"Let the children keep it, doses be damned","moral_delta":2,"outcome_text":"The children name it before noon. You will regret the cage, but tonight it is worth hearing them laugh."}],"discovery":"The squirrel is barely a mouthful and it is still moving wrong — too many pauses, like it forgot w…`
- Bytes: 141,249; SHA-256: `1c84bf9e37036b9247ba0f48e1a14a8d1b497a0bcc63e0e506f23f8bb399b4e1`
- Root keys: `quests, schema_version`
- `quests`: list[68]; union fields: `category, choices, discovery, display_name, id, location_id, max_day, min_day, trigger`
  - row 1: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave the child everything in the pack. Came away with a drawing of a house.","label":"Give all your food","moral_delta":10,"outcome_text":"You hand over everything you are carrying. They eat half, fast, and put the rest inside their coat. Then they give you a charcoal drawing of a house, because they think a trade is polite."},{"empathy_delta":1,"epitaph":"Split my rations with the child at the crossing. Kept enough to walk home on.","label":"Give half your food","moral_delta":5,"outcome_text":"They eat slowly and keep both eyes on your hands the whole time, in case the rest of it …`
  - row 2: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Emptied the pack in a stairwell for a family of four. They tried to give a share back.","label":"Give all your supplies","moral_delta":15,"outcome_text":"You empty the pack onto the blanket. The father does not thank you; he starts dividing it into four piles and then, after a moment, into five, and pushes one back toward you."},{"empathy_delta":1,"epitaph":"Left half my stores with the family in the stairwell. Children ate first.","label":"Give half your supplies","moral_delta":8,"outcome_text":"Half of what you carry stays on the blanket. They portion it out before you are throug…`
  - row 3: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Splinted a stranger's leg in an alley. He insists he owes me.","label":"Give medical supplies","moral_delta":8,"outcome_text":"You splint it against a length of pipe and use most of the bandages. He talks the whole time about what he will owe you and where to find him, which is how he keeps from screaming."},{"empathy_delta":1,"epitaph":"Left food with the man in the alley. Left the leg as I found it.","label":"Give food instead","moral_delta":5,"outcome_text":"No bandages to spare, so you leave food and water within reach. It steadies him. The leg is still broken when you go."},{"…`
  - row 4: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave a stranger my whole canteen. Traded it for the way to clean water.","label":"Give all your water","moral_delta":7,"outcome_text":"You hand over your canteen and they drink it dry in one go, then sit down hard. When they can talk again they draw you a map to a spring that runs clean, and make you repeat the turnings back to them."},{"empathy_delta":1,"epitaph":"Poured off half my water at the perimeter. Enough to keep them walking.","label":"Give half your water","moral_delta":4,"outcome_text":"You pour off half. They ration it in sips without being asked, which tells you how l…`
  - row 5: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Spent food and medicine on a dying woman. She left me her family's door.","label":"Give food and medicine","moral_delta":12,"outcome_text":"You spend rations and medicine on someone who will not need them long. She talks for an hour, mostly about her sister, and at the end of it tells you where the family shelter is and which key opens it."},{"empathy_delta":1,"epitaph":"Left supplies with the old woman by the wall. Stayed while she ate.","label":"Give some supplies","moral_delta":6,"outcome_text":"You leave what you can afford at her side and stay while she eats some of it. She is…`
  - row 6: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Emptied the whole medical kit on a delivery in the dark. The child has my name.","label":"Give all your medical supplies","moral_delta":18,"outcome_text":"You put everything sterile you own on the table and boil what you cannot. It takes most of the night, and it works: a small, furious, breathing baby. She asks what you are called, and gives the child your name to carry."},{"empathy_delta":1,"epitaph":"Gave up antiseptic and clean cloth for a field delivery. Both came through.","label":"Give some supplies","moral_delta":9,"outcome_text":"Enough antiseptic for hands and cord, and c…`
  - row 7: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Patched a raider who asked for quarter. Bought their crew's patrol times.","label":"Give medical supplies","moral_delta":5,"outcome_text":"You pack the wound and strap it. They watch you work, decide you are worth paying, and give you their crew's patrol times for the next nine days.","set_flag":"flag_spared_raider"},{"empathy_delta":1,"epitaph":"Fed a bleeding raider instead of patching them. They ate watching the road.","label":"Give food","moral_delta":3,"outcome_text":"No bandages, just food. They eat it one-handed and fast, and never once stop watching the road behind you.","s…`
  - row 8: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Kept a Peacekeeper alive at the bend. Came away with their ciphers and a name to use.","label":"Give all your supplies","moral_delta":14,"outcome_text":"You work the leg and leave them everything you can carry out without. They pay in the only currency they have on them: safehouse ciphers, patrol frequencies, and a name to use at any of their posts."},{"empathy_delta":1,"epitaph":"Stabilized a Peacekeeper and moved on. They chose not to wait for pickup.","label":"Give some supplies","moral_delta":7,"outcome_text":"Enough to stop the bleeding and steady them. They thank you formally…`
  - row 9: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Fed the archivist and found them lenses. They gave me a survey of the valley.","label":"Give food and spare glasses","moral_delta":16,"outcome_text":"Food first, then the spare lenses. They put them on, look at the far wall for a while without saying anything, then read you a pre-war survey of the whole valley and press the document into your hands."},{"empathy_delta":1,"epitaph":"Fed the archivist. Wrote down the coordinates they knew by heart.","label":"Give food only","moral_delta":8,"outcome_text":"You leave food and no lenses. They recite coordinates from memory instead, check…`
  - row 10: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Paid the toll to the warlord by the fire pit. The road stayed open.","label":"Give them supplies","moral_delta":6,"outcome_text":"You put the supplies down by the tins. He looks at what you brought, not at you, and says the road stays open, and it does."},{"empathy_delta":0,"epitaph":"Refused the warlord's toll to his face. He only kept sorting brass.","label":"Give them nothing","moral_delta":0,"outcome_text":"You tell him no. He goes back to sorting brass without any change in his face, which is somehow more of an answer than shouting would have been."},{"empathy_delta":0,"epitap…`
  - row 11: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave a dying specialist all my medicine. She wrote out a treatment schedule worth more.","label":"Give all your medicine","moral_delta":15,"outcome_text":"You hand over the whole medicine roll. Once she can hold a pen she writes out a chelation schedule better than anything your shelter has, and then keeps writing until the light goes."},{"empathy_delta":1,"epitaph":"Split my medicine with the specialist. She left the formula on a beam.","label":"Give some medicine","moral_delta":8,"outcome_text":"You give her part of the roll. It buys her steady hands for an afternoon, which she s…`
  - row 12: `{"category":"share","choices":[{"empathy_delta":1,"epitaph":"Gave live seed and feed to the sterile rows. Two-fifths of the harvest is mine on paper.","label":"Give seeds and fertilizer","moral_delta":20,"outcome_text":"You hand over live seed stock and the chemical feed to go with it. He works out your share on the spot, aloud, twice, and writes two-fifths of every harvest against your name where the others can see it."},{"empathy_delta":1,"epitaph":"Gave what seed I could spare. My share was written down honestly.","label":"Give some supplies","moral_delta":10,"outcome_text":"Part of the seed, part of the feed. He revises his numbers down …`
  - row 13: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat through an old man's stories. He gave me the way into a bunker.","label":"Hear the story","moral_delta":8,"outcome_text":"You sit through a story about a water main and the winter it burst. In the middle of it, without changing tone, he gives you the way into an intact bunker under the east junction."},{"empathy_delta":3,"epitaph":"Stayed for every story he had. He trusted me with a cache nobody else claimed.","label":"Stay for all of it","moral_delta":12,"outcome_text":"You stay until the fire is down to coals and he is down to the names of people who signed off on things. At…`
  - row 14: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat with her until the talking ran out. Came away with a toy meant for smaller hands.","label":"Sit with her","moral_delta":10,"outcome_text":"You sit on the floor near the stove and stay while she talks, some of it about the girl and some of it about nothing at all. When the rocking finally slows, she wraps a small wind-up toy in a cloth and puts it in your hands, and does not explain it, and you do not ask."},{"empathy_delta":3,"epitaph":"Kept quiet in her room until the room settled. Nothing got fixed.","label":"Stay, but keep quiet","moral_delta":5,"outcome_text":"You take the…`
  - row 15: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat through the full after-action report. My rifle shoots true again.","label":"Accept the debrief","moral_delta":9,"outcome_text":"You take the whole report, contact by contact, the way they need to give it. When the last name is accounted for, they pick up your rifle, check the sights against their thumbnail, and put right what the road knocked loose."},{"empathy_delta":3,"epitaph":"Picked holes in their report. The holes were real, and what was left was worth having.","label":"Cross-examine the report","moral_delta":4,"outcome_text":"You press the gaps: who held the stairwell, …`
  - row 16: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard a kid's whole theory of the stars. There's a paper sky somewhere with my name in it.","label":"Hear the whole theory","moral_delta":12,"outcome_text":"You crouch down and take the whole theory, every planet and comet of it, in order. When it is over they draw you a map of the sky on paper scrap and write your name on a cluster, because now it belongs to you."},{"empathy_delta":3,"epitaph":"Nodded through a kid's star theory. It made their whole day.","label":"Nod along","moral_delta":6,"outcome_text":"You say 'go on' and 'is that so' at the right places, and it is enough. Th…`
  - row 17: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard every black-tag name read out loud. Left with the key to a pharmacy lockbox.","label":"Hear the names","moral_delta":14,"outcome_text":"You stay for all of it, every name and the reason after it, until the solvent rag dries out. When the list is done they sit lighter in the chair, and they give you a small key and tell you which pharmacy's lockbox it fits."},{"empathy_delta":3,"epitaph":"Told the surgeon the math was sound. Neither of us called it right.","label":"Tell them the math held","moral_delta":7,"outcome_text":"You tell them the numbers were the numbers, that the bo…`
  - row 18: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Stood through the whole sermon. They blessed my weapon like I was one of theirs.","label":"Hear the sermon","moral_delta":6,"outcome_text":"You stand through the whole sermon, fire and judgment and the short roll of the saved. They take your stillness for devotion, and at the end they lay a hand on your weapon and ask the ash to pass it by."},{"empathy_delta":3,"epitaph":"Argued doctrine with the chaplain. Best talk they'd had in months.","label":"Argue the theology","moral_delta":3,"outcome_text":"You take the doctrine apart piece by piece, and they light up like it is a festival…`
  - row 19: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Walked the overpass and heard out the math. Now I know the way into the maintenance level.","label":"Follow the math","moral_delta":11,"outcome_text":"You walk the span with them, twist by twist, until the failure reads like a sentence you could recite. Satisfied that someone finally checked their work, they tell you about a maintenance level underneath that nobody else remembers."},{"empathy_delta":3,"epitaph":"Nodded through the engineer's math. They only needed it witnessed.","label":"Take their word for it","moral_delta":5,"outcome_text":"You nod through the equations without …`
  - row 20: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard the whole warning, every mad word. Left with a filter mask that works.","label":"Hear the warning out","moral_delta":9,"outcome_text":"You stand there and let them get it all out: the shapes, the timing, which road. Once it is said to someone who didn't walk off, they steady enough to press a homemade filter mask on you, the good kind of crude."},{"empathy_delta":3,"epitaph":"Pressed them for proof. They had none, and thanked me for asking.","label":"Ask for proof","moral_delta":4,"outcome_text":"You ask what they saw, exactly, from where, and for how long. They have nothing…`
  - row 21: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard her whole story. She gave me the last photo to carry forward.","label":"Listen sympathetically","moral_delta":9,"outcome_text":"You hear it through. At the end she gives you the last photograph, kept whole. 'Someone should carry it forward.'"},{"empathy_delta":3,"epitaph":"Listened to the lover's tale. She showed me the last photo.","label":"Listen politely","moral_delta":4,"outcome_text":"You nod at the right beats. She keeps one photograph and lets you see it."},{"empathy_delta":0,"epitaph":"Left her to her photographs. The fire was doing its slow work.","label":"Interrupt…`
  - row 22: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Sat through the teacher's whole lesson. I can read the road signs now.","label":"Listen attentively","moral_delta":15,"outcome_text":"Hours pass like minutes. You leave reading the signage on the dead roads."},{"empathy_delta":3,"epitaph":"Half-listened to the lesson. She left me homework. I did it.","label":"Listen, but bored","moral_delta":7,"outcome_text":"You fidget through half. She assigns you homework: a sign to read on your way out."},{"empathy_delta":0,"epitaph":"Walked out of the schoolroom. Survived fine. Know less.","label":"Interrupt or leave","moral_delta":0,"outcome…`
  - row 23: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard the thief out. Let him keep the light. He's been useful since.","label":"Hear the explanation","moral_delta":6,"outcome_text":"His sister is coughing somewhere. You listen, then let him keep the light. He becomes useful after that."},{"empathy_delta":3,"epitaph":"Questioned the thief's story. Half held up. Half came back.","label":"Listen, skeptical","moral_delta":3,"outcome_text":"You question the story. Half of it holds. He returns half of what he took."},{"empathy_delta":0,"epitaph":"Turned the thief in. The camp was thorough. I didn't stay to learn the rest.","label":"Re…`
  - row 24: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Heard the prophet's vision. The message was madness with a map in it, and the map was real.","label":"Hear the vision","moral_delta":18,"outcome_text":"The vision is nonsense stitched with coordinates. You use the coordinates. They're real."},{"empathy_delta":3,"epitaph":"Argued the prophet's vision into sense. Kept the parts that held.","label":"Listen skeptically","moral_delta":9,"outcome_text":"You sort the signal from the noise with him. Half the message survives your doubts, and half of that is useful."},{"empathy_delta":0,"epitaph":"Left the prophet to his stars. They talk o…`
  - row 25: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat with a woman beside fresh earth. Came away wearing her partner's coat.","label":"Sit with her","moral_delta":14,"outcome_text":"You sit down in the dirt next to her and stay. Nothing gets better while you are there. When the light starts to go, she stands, pulls his coat off the pile, and puts it in your arms. 'It's a good coat. He'd be angry if it went in the ground.'"},{"empathy_delta":2,"epitaph":"Told a woman her partner was dead because she needed to hear it. She packed before dark.","label":"Say it out loud","moral_delta":7,"outcome_text":"You say the word she has been …`
  - row 26: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked a kid up out of a nightmare. He paid me in a salvage map.","label":"Wake him slowly","moral_delta":16,"outcome_text":"You say his name from a step back until he comes up out of it. He stares at the walls a long time, checking them. Before you leave he presses a folded scrap into your hand, a map of where he has seen salvage, to pay for the waking."},{"empathy_delta":2,"epitaph":"Shook a kid awake to stop the screaming. He shook longer than he had screamed.","label":"Shake him awake","moral_delta":8,"outcome_text":"You grab his shoulder and shake hard. He comes up gasping a…`
  - row 27: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked a gut-shot man through the worst of it. He says he owes me his life.","label":"Keep talking to him","moral_delta":10,"outcome_text":"You crouch and keep your voice on him, your name, his name, the weather, anything, and the shock waits a while. Before he goes under he tells you he owes you his life, and to collect if he lives."},{"empathy_delta":2,"epitaph":"Bound a man's belly wound. Left the rest of it where I found it.","label":"Apply standard first aid","moral_delta":5,"outcome_text":"You press the wound closed and bind it tight. The bleeding slows. Whatever is happeni…`
  - row 28: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat on a cellar floor and talked someone through the dark. They sorted my pack to say thank you.","label":"Talk them through it","moral_delta":12,"outcome_text":"You sit on the floor where they can see you and keep talking, small dull things, until their breathing evens out some. Nothing is fixed, they are still down there when you leave. Later you find your pack repacked and tidy, the only thanks they could manage."},{"empathy_delta":2,"epitaph":"Left a light stick with someone afraid of the dark.","label":"Leave a light","moral_delta":6,"outcome_text":"You crack a chem light an…`
  - row 29: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat up all night with someone coming off the drug. Morning came anyway.","label":"Sit with them through it","moral_delta":8,"outcome_text":"You stay through the worst hours, water when they can hold it, a hand on the shoulder when the shaking peaks. Come morning they eat half a ration and keep it down. It is the first day in a long time they end without the drug."},{"empathy_delta":2,"epitaph":"Handed a shaking person a pill full of nothing. It worked for an hour.","label":"Give them a dummy dose","moral_delta":4,"outcome_text":"You give them a capsule of flour and call it the re…`
  - row 30: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Told a survivor their living meant nothing either way. They gave me a radio frequency for it.","label":"Tell them someone had to be the one","moral_delta":15,"outcome_text":"You tell them that in a dying this size, somebody has to be left over, and it means nothing about them. They sit with that a while. Then they teach you a guarded radio frequency, something useful to carry instead."},{"empathy_delta":2,"epitaph":"Agreed with a man that he should be dead. It was what he wanted to hear.","label":"Agree the odds were bad","moral_delta":8,"outcome_text":"You agree with them. The o…`
  - row 31: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Sat through an old man's whole archive. He paid me in a good knife.","label":"Listen to all of it","moral_delta":14,"outcome_text":"You sit until your legs ache, through flood marks and filter schedules and the names of the dead who taught him. When it is done he gives you his folding knife, good steel, like a receipt for the hours."},{"empathy_delta":2,"epitaph":"Took the useful parts of an old man's memory. Left the rest.","label":"Take only what keeps you alive","moral_delta":7,"outcome_text":"You cut him down to the parts that keep a body breathing, water, rot, weather. He gi…`
  - row 32: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Woke a sleeper out of a bad dream. They split their breakfast with me.","label":"Wake them gently","moral_delta":10,"outcome_text":"You say their name from where their hands cannot reach you, palms open, until they come up out of it. They check the room twice before they believe it. At breakfast they push half their portion your way without a word."},{"empathy_delta":2,"epitaph":"Hauled a dreamer awake the hard way. They watch me now.","label":"Jolt them awake","moral_delta":5,"outcome_text":"You grab a shoulder and shake. They come up swinging. Afterward they sleep with their ba…`
  - row 33: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked two hours with someone starving for it. They paid me in food.","label":"Stay and talk","moral_delta":12,"outcome_text":"You trade two hours of nothing much, weather, rations, a dog one of you used to know. When you get up to leave they press extra rations on you, as if talk were something you can pay off."},{"empathy_delta":2,"epitaph":"Told a lonely person their loneliness was normal. They wrote it down.","label":"Tell them three weeks is normal now","moral_delta":6,"outcome_text":"You tell them three quiet weeks is about standard these days. They write it down, like a fa…`
  - row 34: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Stood by while someone screamed themselves empty. Then I heard the whole list of their dead.","label":"Wait it out nearby","moral_delta":9,"outcome_text":"You stand off at a distance until the shouting wears itself down. Afterward they tell you the whole list of what they lost, every name, like reading a manifest out loud."},{"empathy_delta":2,"epitaph":"Talked a scream down to a shout. The anger stayed where it was.","label":"Talk them down","moral_delta":4,"outcome_text":"You keep your voice low and even until theirs drops to meet it. The fists stay clenched. That part you cann…`
  - row 35: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Told a dreamer the dream would hold. Now I carry stones for it.","label":"Tell them it can stand","moral_delta":18,"outcome_text":"You go over the drawing and say so, it can stand. Before you are finished saying it you are holding a work detail with your name on it."},{"empathy_delta":2,"epitaph":"Read a rebuilding plan. Half of it was true.","label":"Look the plan over","moral_delta":9,"outcome_text":"You read the whole thing. Some of it would bear weight. You say which parts, and leave the rest unsaid."},{"empathy_delta":0,"epitaph":"Refused to look at a plan for putting a roof…`
  - row 36: `{"category":"comfort","choices":[{"empathy_delta":4,"epitaph":"Talked a dying arithmetic down to one day. They agreed to the day.","label":"Talk about tomorrow only","moral_delta":16,"outcome_text":"You don't argue the winter. You argue for tomorrow, one day, that is all anyone has to get through. They agree to that much. It is not hope, but it is a time you both set."},{"empathy_delta":2,"epitaph":"Gave a despairing person the standard words. They were worth what they cost.","label":"Offer the usual words","moral_delta":8,"outcome_text":"You say the things people say. They hear each one land and stay where it lands. Nothing moves."},{"empat…`
  - row 37: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Marked a grave that had no name on it. Left the coords in the log so nobody digs there twice.","label":"Mark the grave","moral_delta":12,"outcome_text":"You write the coordinates in your log and set a length of pipe upright over the spot, wired to a strip of cloth. The next patrol through here will see it and walk around."},{"empathy_delta":0,"epitaph":"Walked around a grave with no marker. It was already covered. That had to be enough.","label":"Walk around it","moral_delta":0,"outcome_text":"You give the dip a wide berth and keep moving. It is already filled. There is nothing in i…`
  - row 38: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried a burned body under its own wall. Painted a stripe so nobody tracks it home.","label":"Cover and mark it","moral_delta":10,"outcome_text":"You kick down the half-standing partition so it falls over the spot, then paint a stripe on the wall by the door. The dust stays where it is, and the next crew knows not to sweep there."},{"empathy_delta":0,"epitaph":"Walked past a burned body. There was nothing left to bury that the fire had not already done.","label":"Keep your distance","moral_delta":0,"outcome_text":"You cross the room wide of it and go on. Burned that thoroughly, it i…`
  - row 39: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Pulled a body out of the intake. Lost my gloves and my appetite. Three camps drink tomorrow and don't know why.","label":"Drag it clear","moral_delta":8,"outcome_text":"You rope the ankles and haul from the bank, and it takes most of an hour before the grate runs clean. You wash your arms to the elbow and lose your gloves to it. The downstream camps never hear about any of this."},{"empathy_delta":0,"epitaph":"Found the intake fouled by a body. Left a warning scratched on the post and kept my own water.","label":"Mark it foul and go","moral_delta":0,"outcome_text":"You scratch a war…`
  - row 40: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried a child and a toy truck in frozen ground. Cost me the daylight and chipped the shovel. A stranger nodded at me from the road.","label":"Bury them with the truck","moral_delta":20,"outcome_text":"The ground outside is frozen two knuckles deep, and digging takes you the rest of the daylight and chips your shovel. You put the truck in with them. A scavenger passing on the road stops, watches, and gives you one nod before moving on."},{"empathy_delta":0,"epitaph":"Shut the door on a child's bones. They'd kept this long. They'll keep.","label":"Close the door","moral_delta":0,"out…`
  - row 41: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Paced out a pit of forty or more and put it on the map as ground you don't dig. Left a ration on a stone. Can't say why.","label":"Record it and leave a marker","moral_delta":18,"outcome_text":"You walk the edges, pace out the corners, and enter it on your network map as a place to be left alone. You set one of your own rations on a flat stone at the head of the pit. It will be gone by morning, but that is not the point."},{"empathy_delta":0,"epitaph":"Walked around a full pit. The work there was done long before I came.","label":"Walk the perimeter","moral_delta":0,"outcome_text":"…`
  - row 42: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Cut a hanged stranger down off the gantry and put them under the roadbed. Left the rope on the grave. Didn't want it.","label":"Cut them down and bury them","moral_delta":14,"outcome_text":"You climb the gantry, saw through the braid, and lower the body hand over hand. Digging below the roadbed takes the rest of the day. You coil the cut end of the rope and leave it on the grave, because it is theirs and you do not want it."},{"empathy_delta":0,"epitaph":"Walked under a hanged body on the gantry. Looked up once. Kept walking.","label":"Keep to your route","moral_delta":0,"outcome_te…`
  - row 43: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried the one from the locked closet in the garden soil, coat and all. Slept in the house after. It was just a house.","label":"Carry them out and bury them","moral_delta":12,"outcome_text":"You wrap the bones in a bedsheet and dig in the side yard, where the soil is soft from an old garden. The coat goes in too. After, the house is only a house, and you sleep in it without thinking about the closet."},{"empathy_delta":0,"epitaph":"Left the one in the closet where they chose to be. Closed two doors on it.","label":"Shut the door again","moral_delta":0,"outcome_text":"They sealed th…`
  - row 44: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Waded in and pulled a floater onto the walk, buried it in the fill. The water's still bad. It's less bad.","label":"Haul it out and bury it","moral_delta":10,"outcome_text":"You wade in, get a line under the arms, and drag it up onto the concrete walk. Digging into the rubble fill takes an hour. The water will not get cleaner on its own, but the worst of it is now in the ground instead of in the flow."},{"empathy_delta":0,"epitaph":"Chalked a floater's stretch as foul water and walked on. Had enough in the bottles to afford that.","label":"Mark it and move on","moral_delta":0,"outco…`
  - row 45: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Finished a stranger's cremation with a shovel and the wind. Kicked the pit in after. Their fire had quit; mine didn't.","label":"Finish it by hand","moral_delta":16,"outcome_text":"You crush what the fire left with the flat of your shovel and scatter it past the pit, a shovelful at a time, into the wind. It takes the better part of an hour. When the pit is empty you kick the brick lining in after it."},{"empathy_delta":0,"epitaph":"Stepped around a half-finished burn pit. Nothing left in it that could hurt anyone.","label":"Leave the pit","moral_delta":0,"outcome_text":"There is not…`
  - row 46: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Cut the wire off a prisoner's wrists and buried them outside the fence. Left the cell open. Nothing there to keep.","label":"Cut the wire and bury them","moral_delta":18,"outcome_text":"You work the wire off the wrists, carry the body out past the wire fence, and dig where the ground is clear of concrete. It takes the day. You leave the cell block standing open behind you, because nothing in it needs keeping anymore."},{"empathy_delta":0,"epitaph":"Closed the cell door on a wired body. The block keeps its own records.","label":"Leave the cell as found","moral_delta":0,"outcome_text"…`
  - row 47: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried the one from the sealed room with their letter on their chest, unread. It wasn't written to me.","label":"Bury them with the letter unread","moral_delta":14,"outcome_text":"You dig out back, lay the body in, and set the folded sheet on the chest without opening it. Whatever it says, it was not addressed to you. You fill the grave and leave the tape on the door for the next one to find."},{"empathy_delta":0,"epitaph":"Left the sealed room sealed. They arranged it themselves. I just knocked.","label":"Leave the room sealed","moral_delta":0,"outcome_text":"They built this closur…`
  - row 48: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Dug one trench behind the block, two days, broke the shovel, finished with a shelf. Didn't count them. Marked it with a door.","label":"Dig a common grave","moral_delta":22,"outcome_text":"Two full days of daylight, one long trench in the lot behind the block, and your shovel gives out on the second morning so you finish with a length of shelving. You carry them down one at a time and do not count them. When it is done you pile the dirt high and mark it with a door stood on end."},{"empathy_delta":0,"epitaph":"Walked around a block where everyone died at once. Drew it on the map so …`
  - row 49: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Authorized thermal proximity. Acquired updated topographical data.","label":"Authorize proximity","moral_delta":10,"outcome_text":"You permit entry. They transfer a portion of their caloric reserves and provide updated topographical data for the northern route."},{"empathy_delta":1,"epitaph":"Authorized maximum-range thermal proximity. Maintained combat readiness.","label":"Authorize at maximum range","moral_delta":5,"outcome_text":"You assign them the furthest functional radius. Both units maintain readiness. No data is exchanged."},{"empathy_delta":0,"epitaph":"Denied thermal pro…`
  - row 50: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Provided comprehensive aid to compromised unknown. Unit integrated into operations.","label":"Provide comprehensive aid","moral_delta":14,"outcome_text":"You expend medical supplies and carry their mass. They integrate into your operation and provide ongoing tactical analysis."},{"empathy_delta":1,"epitaph":"Provided basic triage. Directed unit to medical facility.","label":"Provide basic triage","moral_delta":7,"outcome_text":"You apply standard coagulation protocols and direct them to the nearest known medical facility."},{"empathy_delta":0,"epitaph":"Bypassed compromised unknown…`
  - row 51: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Executed standard exchange. Acquired coordinates for stable logistics hub.","label":"Execute standard exchange","moral_delta":8,"outcome_text":"You meet the stated caloric price without friction. The asset provides the coordinates of a stable trading hub as a bonus."},{"empathy_delta":1,"epitaph":"Negotiated material deficit. Secured minor caloric advantage.","label":"Negotiate deficit","moral_delta":4,"outcome_text":"You aggressively contest the valuation. The asset yields slightly to expedite the transaction."},{"empathy_delta":0,"epitaph":"Bypassed mobile logistics node. Conserv…`
  - row 52: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Reunited juvenile with primary unit. Secured positive faction standing.","label":"Reunite with unit","moral_delta":16,"outcome_text":"You expend significant operational time locating their camp. The family unit registers a permanent positive standing for your faction."},{"empathy_delta":1,"epitaph":"Transferred unattached juvenile to settlement intake. Identification logged.","label":"Transfer to local authority","moral_delta":8,"outcome_text":"You deliver the asset to a functioning settlement's intake officer. The officer logs your ID."},{"empathy_delta":0,"epitaph":"Bypassed unat…`
  - row 53: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Maintained concealment for AWOL combatant. Acquired weapon maintenance data.","label":"Maintain concealment","moral_delta":12,"outcome_text":"You omit their presence from your logs. In exchange, the combatant provides advanced weapon maintenance training."},{"empathy_delta":1,"epitaph":"Processed tactical data from AWOL combatant prior to release.","label":"Process tactical data","moral_delta":6,"outcome_text":"You extract their unit's patrol routes and deployment status before allowing them to pass."},{"empathy_delta":0,"epitaph":"Bypassed AWOL combatant. Ignored factional dispute…`
  - row 54: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Processed actionable intelligence. Integrated broker into logistics network.","label":"Process intelligence","moral_delta":18,"outcome_text":"The data details hostile rotation schedules for the eastern sector. The broker integrates into your logistics network."},{"empathy_delta":1,"epitaph":"Verified data integrity. Extracted value from confirmed intelligence.","label":"Verify data integrity","moral_delta":9,"outcome_text":"You cross-reference the intelligence before acting. The verified portions prove highly valuable."},{"empathy_delta":0,"epitaph":"Declined intelligence transacti…`
  - row 55: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Integrated pre-war clearing tactics. Acquired veteran combatant asset.","label":"Integrate tactical data","moral_delta":15,"outcome_text":"You drill urban clearing tactics until nightfall. Their mechanical memory is flawless. They join your movement the next day."},{"empathy_delta":1,"epitaph":"Extracted summary tactical briefing. Combatant retained operational independence.","label":"Extract summary data","moral_delta":8,"outcome_text":"You accept a rapid briefing on sight alignment and proceed. The combatant correctly assesses you as a short-term asset."},{"empathy_delta":0,"epit…`
  - row 56: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Executed concealment for pursued asset. Acquired high-caloric ration.","label":"Execute concealment","moral_delta":15,"outcome_text":"You divert the pursuers with false telemetry. The asset departs before dawn, leaving a high-caloric ration on your pack."},{"empathy_delta":1,"epitaph":"Authorized passive cover for pursued asset. Sector bypassed by trackers.","label":"Authorize minimal cover","moral_delta":8,"outcome_text":"You allow them to utilize your blind spot but deny active misdirection. The pursuers eventually bypass the sector."},{"empathy_delta":0,"epitaph":"Denied entry t…`
  - row 57: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Mirrored non-verbal asset signals. Acquired forward scouting capability.","label":"Mirror signals and observe","moral_delta":10,"outcome_text":"You return neutral gestures. They establish a forward scouting position and clear three ambush points over the next transit cycle."},{"empathy_delta":1,"epitaph":"Maintained passive observation of non-verbal asset. Contact broken cleanly.","label":"Maintain passive observation","moral_delta":5,"outcome_text":"You monitor their vector without engagement. They eventually break contact at a major intersection."},{"empathy_delta":0,"epitaph":"A…`
  - row 58: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Executed heavy extraction on subterranean asset. Acquired salvage yield.","label":"Execute extraction","moral_delta":15,"outcome_text":"You expend heavy labor to pry the grate. The trapped asset shares half their subterranean salvage yield in compensation."},{"empathy_delta":1,"epitaph":"Confirmed subterranean presence. Marked coordinates for extraction teams.","label":"Confirm presence","moral_delta":8,"outcome_text":"You signal back to confirm their coordinates, then mark the location for better-equipped logistics units."},{"empathy_delta":0,"epitaph":"Bypassed subterranean distr…`
  - row 59: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Authorized temporary tool transfer. Tool upgraded. Secondary cache acquired.","label":"Authorize transfer","moral_delta":11,"outcome_text":"The tool is returned within 48 hours, upgraded with synthetic grips. They also provide coordinates to a secondary, smaller cache."},{"empathy_delta":1,"epitaph":"Transferred secondary degraded tool. Operation executed.","label":"Transfer secondary tool","moral_delta":4,"outcome_text":"You hand over a degraded spare. The unit logs the lack of trust but executes their operation."},{"empathy_delta":0,"epitaph":"Denied tool transfer request. Equipm…`
  - row 60: `{"category":"trust","choices":[{"empathy_delta":4,"epitaph":"Executed delivery protocol. Seal intact. Granted high-priority node access.","label":"Execute delivery protocol","moral_delta":20,"outcome_text":"You carry the envelope through hostile sectors. The Northern Node processes the intact seal and grants you permanent high-priority access."},{"empathy_delta":2,"epitaph":"Breached courier seal. Delivered compromised intel. Zero reward acquired.","label":"Break seal and deliver","moral_delta":8,"outcome_text":"You analyze the data before delivery. The receiving node detects the breach and processes the intel with zero reward."},{"empathy_d…`
  - row 61: `{"category":"share","choices":[{"empathy_delta":2,"epitaph":"Executed joint extraction with juvenile forager. Extracted metals divided.","label":"Execute joint extraction","moral_delta":12,"outcome_text":"You provide caloric support and apply leverage to the slab. The extracted metals are divided. The juvenile transfers a non-functional gear as 'luck.'"},{"empathy_delta":1,"epitaph":"Transferred minimal calories to juvenile forager. Inefficient extraction continued.","label":"Transfer minimal calories","moral_delta":5,"outcome_text":"You transfer a low-value ration. The juvenile consumes it without ceasing their inefficient extraction attemp…`
  - row 62: `{"category":"listen","choices":[{"empathy_delta":2,"epitaph":"Read an old person's letters aloud, all of them. They said the record held.","label":"Read them all aloud","moral_delta":10,"outcome_text":"You read the whole bundle: love notes, rent receipts, a letter never sent. None of it would sell for a bullet to anyone but them. At the end they sit back and say, 'It's true, then. We kept it right,' like that settles an account."},{"empathy_delta":1,"epitaph":"Read two of the letters. That was enough for them.","label":"Read a couple","moral_delta":4,"outcome_text":"You read two, enough to prove the bundle is only other people's ordinary liv…`
  - row 63: `{"category":"trust","choices":[{"empathy_delta":2,"epitaph":"Authorized structural entry for refugee. Acquired combustion fuel payment.","label":"Authorize structural entry","moral_delta":8,"outcome_text":"You permit entry. The unit powers down in a corner. At dawn, they have departed, leaving a bundle of dry combustion fuel as payment.","set_flag":"flag_sheltered_refugee"},{"empathy_delta":1,"epitaph":"Authorized perimeter cover for refugee. Thermal drop survived.","label":"Authorize perimeter cover","moral_delta":3,"outcome_text":"You allow them to utilize the exterior structural overhang. They survive the thermal drop and depart at first …`
  - row 64: `{"category":"dead","choices":[{"empathy_delta":2,"epitaph":"Buried the surveyor by the path, canteen and all. Took the logbook. If I get near sector nine, their sister gets it.","label":"Bury them, keep the logbook","moral_delta":8,"outcome_text":"You dig a grave a few steps off the path and tuck the canteen in with them. The logbook goes in your pack. Grid 47 comes off your route tonight, and if you are ever near sector nine you will knock and hand over the book."},{"empathy_delta":1,"epitaph":"Copied one line off a dead surveyor and struck Grid 47 off my route. Left them with their book open.","label":"Note the grid, leave them","moral_del…`
  - row 65: `{"category":"comfort","choices":[{"empathy_delta":2,"epitaph":"Spent my medkit on a man who said he was done. He paid in directions.","label":"Patch the wound","moral_delta":10,"outcome_text":"You spend gauze and powder on a stranger. The bleeding slows. In return he tells you which streets on the next block are clear and which are watched."},{"empathy_delta":1,"epitaph":"Threw a bandage to a bleeding man. He did the rest himself.","label":"Toss them a bandage","moral_delta":5,"outcome_text":"You throw a bandage pack into his lap. He starts wrapping his own leg with the patience of someone who has done it before."},{"empathy_delta":0,"epitap…`
  - row 66: `{"category":"share","choices":[{"empathy_delta":0,"epitaph":"Butchered a collared rad-dog for meat and said nothing.","label":"Butcher it. Meat is meat.","moral_delta":-8,"outcome_text":"You do the work quickly and do not look at the collar. The shelter eats for four days. Nobody asks what it was."},{"empathy_delta":1,"epitaph":"Gave a working dog a clean end instead of a meal.","label":"End it clean and bury it with the collar","moral_delta":4,"outcome_text":"One blow, then digging. The collar goes in with the animal. The empty pot goes on the table anyway."},{"empathy_delta":0,"epitaph":"Reported a collared catch on the net before clearing…`
  - row 67: `{"category":"share","choices":[{"empathy_delta":0,"epitaph":"Served questionable fowl to stretch the rations.","label":"Cook it through and stretch the ration","moral_delta":-5,"outcome_text":"Char on the outside, caution on the inside. Most will be fine. Most is a word you lean on."},{"empathy_delta":0,"epitaph":"Burned contaminated fowl and flagged the ground.","label":"Burn it and mark the site","moral_delta":3,"outcome_text":"The fowl burns quick and greasy. You tie a rag on the snare post so the others know the ground is bad."},{"empathy_delta":0,"epitaph":"Hid contaminated meat for a desperate day.","label":"Save it for the worst week"…`
  - row 68: `{"category":"share","choices":[{"empathy_delta":0,"epitaph":"Boiled a twitching squirrel rather than waste it.","label":"In the pot. Waste is the real cruelty.","moral_delta":-2,"outcome_text":"It is done before the water boils. The children get broth and no explanation."},{"empathy_delta":1,"epitaph":"Gave the children a dying animal to love.","label":"Let the children keep it, doses be damned","moral_delta":2,"outcome_text":"The children name it before noon. You will regret the cage, but tonight it is worth hearing them laugh."}],"discovery":"The squirrel is barely a mouthful and it is still moving wrong — too many pauses, like it forgot w…`

## `Assets/StreamingAssets/Data/moral_choice_flags.json`
- Bytes: 2,090; SHA-256: `e5a95235ce28f9d2a1c9c8bcaeb72789122d9d77e1d38b0658cc86ae4d6a4db4`
- Root keys: `description, flags, schema_version`
- `flags`: list[25]; union fields: `display_name, id`
  - row 1: `{"display_name":"Betrayed an Ally","id":"flag_betrayed_ally"}`
  - row 2: `{"display_name":"Betrayed a Faction","id":"flag_betrayed_faction"}`
  - row 3: `{"display_name":"Broke Trust","id":"flag_betrayed_trust"}`
  - row 4: `{"display_name":"Broken Pact","id":"flag_broken_pact"}`
  - row 5: `{"display_name":"Became Warlord","id":"flag_become_warlord"}`
  - row 6: `{"display_name":"Throne of Ash","id":"flag_throne_of_ash"}`
  - row 7: `{"display_name":"Mercy Road Locked","id":"flag_branch_mercy_road_locked"}`
  - row 8: `{"display_name":"Iron Way Locked","id":"flag_branch_iron_way_locked"}`
  - row 9: `{"display_name":"Listener's Thread Locked","id":"flag_branch_listener_locked"}`
  - row 10: `{"display_name":"Broken Compact Locked","id":"flag_branch_broken_compact_locked"}`
  - row 11: `{"display_name":"Spared a Raider","id":"flag_spared_raider"}`
  - row 12: `{"display_name":"Executed a Prisoner","id":"flag_executed_prisoner"}`
  - row 13: `{"display_name":"Shared Rations","id":"flag_shared_rations"}`
  - row 14: `{"display_name":"Hoarded Medicine","id":"flag_hoarded_medicine"}`
  - row 15: `{"display_name":"Sheltered a Refugee","id":"flag_sheltered_refugee"}`
  - row 16: `{"display_name":"Expelled a Survivor","id":"flag_expelled_survivor"}`
  - row 17: `{"display_name":"Repaired Shared Infrastructure","id":"flag_repaired_infrastructure"}`
  - row 18: `{"display_name":"Sabotaged a Rival","id":"flag_sabotaged_rival"}`
  - row 19: `{"display_name":"Broke a Treaty","id":"flag_broke_treaty"}`
  - row 20: `{"display_name":"Honored a Debt","id":"flag_honored_debt"}`
  - row 21: `{"display_name":"Ignored a Distress Call","id":"flag_ignored_distress"}`
  - row 22: `{"display_name":"Responded to Distress","id":"flag_responded_distress"}`
  - row 23: `{"display_name":"Forged a Record","id":"flag_forged_record"}`
  - row 24: `{"display_name":"Preserved an Archive","id":"flag_preserved_archive"}`
  - row 25: `{"display_name":"Chose a Faction Side","id":"flag_chosen_faction_side"}`
- Bytes: 2,090; SHA-256: `e5a95235ce28f9d2a1c9c8bcaeb72789122d9d77e1d38b0658cc86ae4d6a4db4`
- Root keys: `description, flags, schema_version`
- `flags`: list[25]; union fields: `display_name, id`
  - row 1: `{"display_name":"Betrayed an Ally","id":"flag_betrayed_ally"}`
  - row 2: `{"display_name":"Betrayed a Faction","id":"flag_betrayed_faction"}`
  - row 3: `{"display_name":"Broke Trust","id":"flag_betrayed_trust"}`
  - row 4: `{"display_name":"Broken Pact","id":"flag_broken_pact"}`
  - row 5: `{"display_name":"Became Warlord","id":"flag_become_warlord"}`
  - row 6: `{"display_name":"Throne of Ash","id":"flag_throne_of_ash"}`
  - row 7: `{"display_name":"Mercy Road Locked","id":"flag_branch_mercy_road_locked"}`
  - row 8: `{"display_name":"Iron Way Locked","id":"flag_branch_iron_way_locked"}`
  - row 9: `{"display_name":"Listener's Thread Locked","id":"flag_branch_listener_locked"}`
  - row 10: `{"display_name":"Broken Compact Locked","id":"flag_branch_broken_compact_locked"}`
  - row 11: `{"display_name":"Spared a Raider","id":"flag_spared_raider"}`
  - row 12: `{"display_name":"Executed a Prisoner","id":"flag_executed_prisoner"}`
  - row 13: `{"display_name":"Shared Rations","id":"flag_shared_rations"}`
  - row 14: `{"display_name":"Hoarded Medicine","id":"flag_hoarded_medicine"}`
  - row 15: `{"display_name":"Sheltered a Refugee","id":"flag_sheltered_refugee"}`
  - row 16: `{"display_name":"Expelled a Survivor","id":"flag_expelled_survivor"}`
  - row 17: `{"display_name":"Repaired Shared Infrastructure","id":"flag_repaired_infrastructure"}`
  - row 18: `{"display_name":"Sabotaged a Rival","id":"flag_sabotaged_rival"}`
  - row 19: `{"display_name":"Broke a Treaty","id":"flag_broke_treaty"}`
  - row 20: `{"display_name":"Honored a Debt","id":"flag_honored_debt"}`
  - row 21: `{"display_name":"Ignored a Distress Call","id":"flag_ignored_distress"}`
  - row 22: `{"display_name":"Responded to Distress","id":"flag_responded_distress"}`
  - row 23: `{"display_name":"Forged a Record","id":"flag_forged_record"}`
  - row 24: `{"display_name":"Preserved an Archive","id":"flag_preserved_archive"}`
  - row 25: `{"display_name":"Chose a Faction Side","id":"flag_chosen_faction_side"}`

## `Assets/StreamingAssets/Data/moral_choice_gossip.json`
- Bytes: 27,959; SHA-256: `fabce75419bbfe57e5637338851bd0418a1e3056f49cfe39b61858df4361c0f8`
- Root keys: `camp_chatter, description, gossip_decay, npc_greeting_shifts, schema_version, whisper_lines`
- `camp_chatter`: object[8]
  - `very_positive`: list[20]
    - row 1: `"Did you hear? They gave their last medicine to that stranger's kid. The whole course."`
    - row 2: `"The one from the eastern shelter — people say they buried every body they found on the ridge. Every one."`
    - row 3: `"Someone told me they sat with old Mara all night. Didn't ask for anything. Just sat."`
    - row 4: `"I heard they turned down a clean trade route to carry a wounded stranger back to camp. On their back."`
    - row 5: `"The Peacekeepers are talking about them. Not the bad kind of talking."`
    - row 6: `"Word is they gave seeds to the last farmer on the plateau. The whole stock. Their own stock."`
    - row 7: `"They say even the raiders won't touch that one. Too much... weight, I guess. Too much weight behind the name."`
    - row 8: `"A child in the east quarter drew a picture of them. Calls them 'the kind one.' Hangs it on the wall."`
    - row 9: `"The Knowledge Keepers want to meet them. Not to ask — to give. That's new."`
    - row 10: `"I don't trust saints. But this one keeps showing up, and keeps giving, and hasn't asked for a single thing back."`
    - row 11: `"They gave the clinic its last clean bandage roll and waited for the next shipment."`
    - row 12: `"The outer-gate watch says they carried a frightened stranger in before checking their papers."`
    - row 13: `"They returned the trade scale with the extra measure still on it."`
    - row 14: `"Someone saw them leave the warm bunk for a sick traveler and never mention it."`
    - row 15: `"They kept the promise even after the promised thing became scarce."`
    - row 16: `"The ration pot went short, and they still put the youngest plates first."`
    - row 17: `"They let the wounded rival cross the yard under their protection."`
    - row 18: `"The repair crew got the part they needed because this one surrendered a useful spare."`
    - row 19: `"They told the truth about the missing cans before anyone had to ask."`
    - row 20: `"A lot of people talk brave; that one paid the cost and kept the door open."`
  - `positive`: list[20]
    - row 1: `"They helped the Peacekeeper on the north road. Shared supplies, I heard. Not nothing."`
    - row 2: `"People say they listened to old Gregor's war story. The whole thing. Even the ugly parts."`
    - row 3: `"Word around camp — they comforted the widow by the east fire. Sat with her through the bad hours."`
    - row 4: `"The merchant at the crossroads gives them a nod now. Fair trader, apparently."`
    - row 5: `"I heard they buried a body near the water tower. Properly. Not just kicked dirt over it."`
    - row 6: `"Someone said they gave food to that raider with the broken gun. The raider told them about the next raid."`
    - row 7: `"The doctor at the clinic speaks well of them. Something about patience. About listening."`
    - row 8: `"They stopped for the thirsty one on the desert road. Gave water. Got directions to a spring."`
    - row 9: `"Not a saint. But decent. That's worth noting, these days."`
    - row 10: `"The trade scale came back honest after they used it. The next crew noticed."`
    - row 11: `"They brought a spare lamp to the night watch instead of waiting for the fire to fail."`
    - row 12: `"The clinic got first pick of the clean cloth. Nobody had to argue for it."`
    - row 13: `"They stopped to carry a tired traveler through the gate and still made the meeting on time."`
    - row 14: `"They left room in the bunk row for people who had not arrived yet."`
    - row 15: `"They answered the hard questions without turning the whole thing into a performance."`
    - row 16: `"Someone says they sent word before changing the ration list. That counts as courtesy now."`
    - row 17: `"They had leverage and did not use all of it."`
    - row 18: `"They returned a borrowed tool before the repair shift asked for it."`
    - row 19: `"The route note was shared with the next crew. No price attached."`
    - row 20: `"They remembered who had been waiting when the meeting ran long."`
  - `slightly_positive`: list[20]
    - row 1: `"They helped someone on the road. I forget who. But they helped."`
    - row 2: `"People say they're not bad. Which is... something."`
    - row 3: `"The Peacekeepers don't frown when that name comes up. Low bar, but there it is."`
    - row 4: `"I heard they gave a little food to a stranger. Not much. But they gave."`
    - row 5: `"Someone said they listened to a story or two. Didn't mock anyone for telling one."`
    - row 6: `"Could have kept the whole crate. Didn't."`
    - row 7: `"I heard they waited their turn at the clinic."`
    - row 8: `"They asked what was needed before offering what they had."`
    - row 9: `"The gate stayed open for one more person, apparently because they asked."`
    - row 10: `"They corrected the count when nobody else noticed."`
    - row 11: `"Not generous, exactly. Just less hard than expected."`
    - row 12: `"Someone got a second chance after they listened."`
    - row 13: `"They let a stranger finish speaking before walking away."`
    - row 14: `"The deal was fair enough that nobody checked the pockets twice."`
    - row 15: `"They shared the route note, then acted like it was nothing."`
    - row 16: `"They carried one end of the stretcher. The other end did most of the work."`
    - row 17: `"The request was small, and they did not make it smaller."`
    - row 18: `"They gave back the tool with the blade still sharp."`
    - row 19: `"They could have made a scene. They chose the quieter answer."`
    - row 20: `"'Maybe they are learning,' someone said. Nobody argued."`
  - `neutral`: list[20]
    - row 1: `"Who? Oh, them. I don't know. They come and go. Keep to themselves."`
    - row 2: `"Never helped me. Never hurt me. That's the whole file on them."`
    - row 3: `"The name doesn't ring any bells. Which is probably fine."`
    - row 4: `"Neither the raiders nor the Peacekeepers mention them. That's a kind of safety."`
    - row 5: `"Average. In the old world that was an insult. Now it's a blessing."`
    - row 6: `"Ask three people what they think and get four answers."`
    - row 7: `"They'll help if the numbers work. That's not nothing."`
    - row 8: `"The door opens for some requests and stays shut for others."`
    - row 9: `"They trade clean, but they don't promise clean."`
    - row 10: `"Nobody knows whether the pause means caution or indifference."`
    - row 11: `"They keep a ledger. So does everyone who plans to last."`
    - row 12: `"One crew calls them fair; another calls them difficult."`
    - row 13: `"The answer changes when the weather does."`
    - row 14: `"They listen until the cost becomes clear."`
    - row 15: `"Somebody got shelter. Somebody else got a refusal."`
    - row 16: `"They brought supplies in and questions out."`
    - row 17: `"The same decision looks wise from one bunk and cold from another."`
    - row 18: `"They do not explain every choice, which leaves room for everyone else's version."`
    - row 19: `"If there is a pattern, it has not shown itself to me."`
    - row 20: `"Wait for the next request. That is usually where the truth is."`
  - `slightly_evil`: list[20]
    - row 1: `"Watch your gear around that one. I'm not saying they took anything. I'm saying watch it."`
    - row 2: `"They walked past someone on the road. I saw it. Just walked past."`
    - row 3: `"People say they refused a child. Could be rumor. Could be fact. The child remembers."`
    - row 4: `"The merchant at the crossroads doesn't give them credit anymore. Something happened."`
    - row 5: `"I don't like the look they give people who ask for help. The look says 'no' before the mouth does."`
    - row 6: `"Count your filters before you hand them over."`
    - row 7: `"They keep the letter of a promise and lose the part that mattered."`
    - row 8: `"Someone asked for help and got a price list instead."`
    - row 9: `"They remember who cannot afford to refuse."`
    - row 10: `"The door opened after the argument, not before."`
    - row 11: `"They do not raise their voice; they just make the alternative smaller."`
    - row 12: `"Keep a second route in mind when they offer the first."`
    - row 13: `"The ration list changed after one name disappeared from it."`
    - row 14: `"They call it efficiency when someone else goes without."`
    - row 15: `"The apology came after the supplies were already gone."`
    - row 16: `"Nobody says betrayal yet. They are waiting to see if it becomes a habit."`
    - row 17: `"A promise from them needs a witness now."`
    - row 18: `"The outer watch learned not to ask twice."`
    - row 19: `"They offered a choice with only one affordable answer."`
    - row 20: `"'Don't make them angry' is becoming part of the instructions."`
  - `evil`: list[20]
    - row 1: `"Stay away from that one. I mean it. People who cross their path end up lighter."`
    - row 2: `"They took from the injured man on the east road. Took his pack while he watched."`
    - row 3: `"Word is they robbed the Knowledge Keeper. Ripped the notes right out of their hands."`
    - row 4: `"The Peacekeepers have a description circulating. Matches that one pretty well."`
    - row 5: `"A raider told me even their people talk about this one — and not with admiration. With caution."`
    - row 6: `"Someone died on the south road. Didn't have to. This one walked past, pockets heavier than before."`
    - row 7: `"The farmer's seed locker is empty. You know who did it. I know who did it. The fence knows."`
    - row 8: `"Don't trade with them. The last person who did found their kit missing the next morning."`
    - row 9: `"People lower their voices when that patrol comes through."`
    - row 10: `"The clinic started counting its bandages before answering their knock."`
    - row 11: `"They call a threat a negotiation and wait for gratitude."`
    - row 12: `"The trader's new escort is not for the road. It is for meetings here."`
    - row 13: `"A surrender is only an invitation to lose something, according to them."`
    - row 14: `"The outer bunks empty when their people ask for space."`
    - row 15: `"They keep the useful part of every bargain and forget the promise."`
    - row 16: `"Someone stopped carrying a family name in public after dealing with them."`
    - row 17: `"No one volunteers a shortage around that table."`
    - row 18: `"They took the safe route off the map and charged for directions."`
    - row 19: `"The apology was posted beside the new lock."`
    - row 20: `"Their offers sound generous until you count who cannot refuse."`
  - `very_evil`: list[20]
    - row 1: `"Lock your shelter. That one is in the area."`
    - row 2: `"The Peacekeepers posted a bounty. You know whose face is on it."`
    - row 3: `"I watched them take from a dying woman. Her last bundle. While she tried to curse them."`
    - row 4: `"The raiders respect them. Think about what that means. Think hard."`
    - row 5: `"A child won't go near the east path anymore. Says 'the bad one' lives there."`
    - row 6: `"They say you can trade with them — but only if you have something they can't just take."`
    - row 7: `"The Knowledge Keepers tell their children about this one. As a warning."`
    - row 8: `"Someone put a mark on the wall outside their shelter. A red mark. Everyone knows what it means."`
    - row 9: `"The old man by the fire stopped telling stories. Says there's no point. Not with this one around."`
    - row 10: `"Even the dogs cross the road when they see them coming. Animals know."`
    - row 11: `"No one leaves a child alone near the outer door when their shadow is on the road."`
    - row 12: `"The traders bring an escort and a second set of scales."`
    - row 13: `"People erase names from the request board before this one can read them."`
    - row 14: `"The lower room fills early whenever their delegation is expected."`
    - row 15: `"They do not ask what a person can spare. They ask what can be taken."`
    - row 16: `"A friendly wave from that direction means check the exits."`
    - row 17: `"Witnesses stopped signing the ledger. They know where the pages go."`
    - row 18: `"The camp keeps a quiet route for anyone who needs to disappear."`
    - row 19: `"Somebody moved the clinic stores before the meeting began."`
    - row 20: `"They call it peace when everyone else has run out of choices."`
- `npc_greeting_shifts`: object[8]
  - `very_positive`: list[20]
    - row 1: `"Oh — you're the one they talk about. The kind one. What can I do for you?"`
    - row 2: `"I heard what you did for the family on the ridge. Thank you. Truly."`
    - row 3: `"You don't have to buy anything. Sit. Eat. You've earned that much."`
    - row 4: `"My daughter drew a picture of you. She's never met you. That's how far your name travels."`
    - row 5: `"The Peacekeepers told us about you. We saved you a share. Here."`
    - row 6: `"You made it. Come in before the cold takes the choice out of your hands."`
    - row 7: `"We've got a place by the stove. Take it; nobody is keeping score."`
    - row 8: `"Tell me what the clinic needs. We'll sort our share first."`
    - row 9: `"You're welcome at this table. If you brought trouble, we can talk about that too."`
    - row 10: `"I heard you kept the gate open. What can I set aside for you?"`
    - row 11: `"Good morning. You look tired; use the good cup."`
    - row 12: `"We can start with the difficult part. I won't make you earn the conversation."`
    - row 13: `"There is room inside. Bring whoever is waiting with you."`
    - row 14: `"You have my attention. Take the time you need."`
    - row 15: `"If you need a witness, I will stand there."`
    - row 16: `"No need to bargain over the first bowl. Eat."`
    - row 17: `"You kept your word before. Let's see what you need today."`
    - row 18: `"Come closer; the fire is shared."`
    - row 19: `"We saved the clean bandages for people who come back through the gate. Take some."`
    - row 20: `"You can ask plainly here. Plain answers are still available."`
  - `positive`: list[20]
    - row 1: `"Ah, you. I've heard good things. What do you need?"`
    - row 2: `"The merchant said you trade fair. That's enough for me. What can I offer?"`
    - row 3: `"You helped someone I know. So I'll help you. Fair's fair."`
    - row 4: `"I trust you more than most. Which isn't saying much, but it's saying something."`
    - row 5: `"Morning. Need the fair price or the honest directions?"`
    - row 6: `"Good to see you. We can work something out."`
    - row 7: `"Someone vouched for you at the gate. Start with what you need."`
    - row 8: `"Come in; the door is open while you are here."`
    - row 9: `"I kept your last message. What changed?"`
    - row 10: `"You can look over the stock before we name a price."`
    - row 11: `"If this is about the clinic, I can point you to the right queue."`
    - row 12: `"We have a little time. Use it well."`
    - row 13: `"I can spare a hand, a can, or an answer. Which one?"`
    - row 14: `"You don't have to explain the whole road. Tell me where it hurts."`
    - row 15: `"The watch knows your name. You won't be delayed."`
    - row 16: `"Trade first, questions after, unless you want it the other way."`
    - row 17: `"You showed up when it mattered. What matters now?"`
    - row 18: `"Take the dry seat. We can talk over the noise."`
    - row 19: `"If someone sent you, say who. If you came yourself, that's fine too."`
    - row 20: `"You are still welcome to ask."`
  - `slightly_positive`: list[20]
    - row 1: `"You seem alright. What are you after?"`
    - row 2: `"Haven't heard anything bad about you. That's practically a recommendation."`
    - row 3: `"You can sit if you want. Fire's open."`
    - row 4: `"Come in if you like. I can spare a minute."`
    - row 5: `"You kept that last answer simple. Let's try again."`
    - row 6: `"I remember you. That is not a complaint."`
    - row 7: `"You can stand by the door or take the chair. Your choice."`
    - row 8: `"Need something? Say it before I decide what you mean."`
    - row 9: `"We can talk. Just don't promise more than you can carry."`
    - row 10: `"You returned the tool, so I am listening."`
    - row 11: `"I have not forgotten the fair count."`
    - row 12: `"Start with the part that costs you."`
    - row 13: `"You can ask for help without making a speech."`
    - row 14: `"I was expecting worse. Good morning."`
    - row 15: `"If you are here to trade, set it down where I can see it."`
    - row 16: `"There is room at the edge of the fire."`
    - row 17: `"You did one decent thing. Let's not spend it twice."`
    - row 18: `"I can give you an answer, maybe not the one you want."`
    - row 19: `"We have met before. Try not to make this awkward."`
    - row 20: `"Say what you need; I will decide what I can spare."`
  - `neutral`: list[20]
    - row 1: `"What do you want?"`
    - row 2: `"Trading or passing through?"`
    - row 3: `"I don't know you. State your business."`
    - row 4: `"Sit if you want. Leave if you want. Makes no difference to me."`
    - row 5: `"You're here. What do you need?"`
    - row 6: `"We can talk. Keep it brief."`
    - row 7: `"If you are trading, show the goods."`
    - row 8: `"I have a few minutes. Use them carefully."`
    - row 9: `"What brings you to this door?"`
    - row 10: `"You know the arrangement. State your request."`
    - row 11: `"Sit or stand; either way, say what this is about."`
    - row 12: `"I can listen before I decide."`
    - row 13: `"No promises from either side. That works for me."`
    - row 14: `"You want something. Start there."`
    - row 15: `"The ledger is open. The answer may not be."`
    - row 16: `"If this is a complaint, make it specific."`
    - row 17: `"We have dealt before. Let us see if that helps."`
    - row 18: `"You can ask. I can say no."`
    - row 19: `"The room is public; the conversation is not."`
    - row 20: `"Tell me what changed since last time."`
  - `slightly_evil`: list[20]
    - row 1: `"Keep your distance. We can talk from here."`
    - row 2: `"I know your face. I'll trade, but I'm watching my pack."`
    - row 3: `"What do you want? Make it quick."`
    - row 4: `"The children go inside when you come around. You notice that?"`
    - row 5: `"We can talk from here. Do not cross the line."`
    - row 6: `"Set the pack down before you sit."`
    - row 7: `"I remember the last promise. Bring a witness this time."`
    - row 8: `"Make your request and keep your hands visible."`
    - row 9: `"There is no reason to raise your voice. There is also no reason to trust it."`
    - row 10: `"You get the outer table. The inner door stays shut."`
    - row 11: `"If you want credit, bring someone who can vouch for you."`
    - row 12: `"Do not make me count the supplies twice."`
    - row 13: `"You can trade, but nothing leaves this room unmarked."`
    - row 14: `"I have heard enough versions. Give me the one you can prove."`
    - row 15: `"Keep to the lit side of the yard."`
    - row 16: `"Ask for what you need, not what you think we owe."`
    - row 17: `"You are allowed to speak. That is not the same as being agreed with."`
    - row 18: `"The children are inside. Choose your words."`
    - row 19: `"We will finish this before the watch changes."`
    - row 20: `"If you came for forgiveness, find another door."`
  - `evil`: list[20]
    - row 1: `"You. I know what you did. Say your piece and go."`
    - row 2: `"My hand's on my knife. Don't take it personally. Actually, do."`
    - row 3: `"Last person who looked at me like that lost a tooth. State your business."`
    - row 4: `"I'd spit, but you're not worth the moisture."`
    - row 5: `"State your business. The inner door stays closed."`
    - row 6: `"Leave the pack where I can see it."`
    - row 7: `"You get one answer. Make it useful."`
    - row 8: `"Do not call this a favor."`
    - row 9: `"We remember the last deal. You do not get the soft version."`
    - row 10: `"The watch will hear this conversation."`
    - row 11: `"If you are asking for supplies, bring the count and the reason."`
    - row 12: `"No room at this table. Use the public bench."`
    - row 13: `"You can speak; nobody promised to help."`
    - row 14: `"Do not reach for the ledger."`
    - row 15: `"The gate is open for trade, not trust."`
    - row 16: `"If you came to bargain, start with what you owe."`
    - row 17: `"Keep your voice down. I do not want this mistaken for welcome."`
    - row 18: `"The clinic will see you after the others. You know why."`
    - row 19: `"Leave before someone thinks I invited you."`
    - row 20: `"We are not discussing what happened. We are discussing what you will do now."`
  - `very_evil`: list[20]
    - row 1: `"Get away from my fire. Now."`
    - row 2: `"I know your face. Everyone does. The bounty poster made sure of that."`
    - row 3: `"You come any closer and I scream. The whole camp comes running."`
    - row 4: `"I have nothing you can take. Leave."`
    - row 5: `"The mark on my door is for you. You know what it means. Walk on."`
    - row 6: `"Stop at the line. I will find someone else to hear you."`
    - row 7: `"The fire is not yours. Keep walking."`
    - row 8: `"You can leave your message with the watch."`
    - row 9: `"Do not say my name like we are friends."`
    - row 10: `"The door stays shut. So does the conversation."`
    - row 11: `"Take one more step and the whole yard hears it."`
    - row 12: `"We moved the stores before you arrived."`
    - row 13: `"If you need help, ask someone who still trusts you."`
    - row 14: `"No trade here. Not after the last one."`
    - row 15: `"You may wait outside until daylight."`
    - row 16: `"Do not bring a child to this door."`
    - row 17: `"Whatever you want, the answer is no."`
    - row 18: `"I will not be alone for this conversation."`
    - row 19: `"The room is full. It was full before you came."`
    - row 20: `"Leave the marker where you found it and go."`
- `whisper_lines`: object[8]
  - `very_positive`: list[20]
    - row 1: `"...gave everything they had, I heard..."`
    - row 2: `"...the Peacekeepers want to meet them, not arrest them..."`
    - row 3: `"...buried every body on the ridge. Every single one..."`
    - row 4: `"...sat with the old woman all night. Didn't sleep..."`
    - row 5: `"...even the raiders won't..."`
    - row 6: `"...a saint, or the closest thing we have left..."`
    - row 7: `"...gave the last clean blanket to someone from outside..."`
    - row 8: `"...they waited for the answer, even when it hurt..."`
    - row 9: `"...the spare part went to the clinic first..."`
    - row 10: `"...kept the promise after the price changed..."`
    - row 11: `"...the gate watch says they never asked who deserved it..."`
    - row 12: `"...they listened before they chose..."`
    - row 13: `"...the little one got the warm bunk..."`
    - row 14: `"...returned the extra measure..."`
    - row 15: `"...not loud about it. That's why people believe it..."`
    - row 16: `"...the rival crossed safely because they said so..."`
    - row 17: `"...they could have taken the credit..."`
    - row 18: `"...someone had to give up a route. They did..."`
    - row 19: `"...you can still trust a promise if that one makes it..."`
    - row 20: `"...the good news travels farther when no one is paid to carry it..."`
  - `positive`: list[20]
    - row 1: `"...fair trader, they say..."`
    - row 2: `"...helped the doctor's patient. Didn't ask for pay..."`
    - row 3: `"...listened to old Gregor. The whole damn story..."`
    - row 4: `"...gave water to the one on the road..."`
    - row 5: `"...fair count, fair price..."`
    - row 6: `"...they brought the clinic what it needed..."`
    - row 7: `"...someone got a second chance..."`
    - row 8: `"...they came back with the missing names..."`
    - row 9: `"...the door was open when the family arrived..."`
    - row 10: `"...they did not press the wounded traveler..."`
    - row 11: `"...you can ask them straight..."`
    - row 12: `"...the trade crew stopped checking the seal twice..."`
    - row 13: `"...a promise kept is still news..."`
    - row 14: `"...they left enough for the next shift..."`
    - row 15: `"...the answer was kind without making a show of it..."`
    - row 16: `"...they remembered the small things..."`
    - row 17: `"...the watch lets them pass without a question..."`
    - row 18: `"...they share the route if you ask..."`
    - row 19: `"...not perfect. Reliable is enough..."`
    - row 20: `"...they help before anyone has to beg..."`
  - `slightly_positive`: list[20]
    - row 1: `"...could have kept it..."`
    - row 2: `"...they gave the extra back..."`
    - row 3: `"...maybe I was wrong..."`
    - row 4: `"...don't make a song of it. They helped..."`
    - row 5: `"...they listened all the way through..."`
    - row 6: `"...the fair count held..."`
    - row 7: `"...they let the stranger finish..."`
    - row 8: `"...one more place at the table, that's all..."`
    - row 9: `"...the deal did not turn ugly..."`
    - row 10: `"...they left the tool where it belonged..."`
    - row 11: `"...not generous. Just decent for once..."`
    - row 12: `"...the door stayed open a little longer..."`
    - row 13: `"...they gave an answer before taking anything..."`
    - row 14: `"...I expected a sharper edge..."`
    - row 15: `"...the second chance was real..."`
    - row 16: `"...they shared the map and asked for nothing..."`
    - row 17: `"...the promise cost them. They paid it..."`
    - row 18: `"...someone spoke up for them..."`
    - row 19: `"...it was a small kindness..."`
    - row 20: `"...wait and see if it happens again..."`
  - `neutral`: list[20]
    - row 1: `"...who? Oh, nobody special..."`
    - row 2: `"...comes and goes..."`
    - row 3: `"...I don't know them. Do you?..."`
    - row 4: `"...depends who is asking..."`
    - row 5: `"...fair in one room, stubborn in the next..."`
    - row 6: `"...they keep their reasons to themselves..."`
    - row 7: `"...someone got help. Someone else got a refusal..."`
    - row 8: `"...the door is open until it isn't..."`
    - row 9: `"...watch what happens when the cost changes..."`
    - row 10: `"...no one can agree what they want..."`
    - row 11: `"...the same story has three endings..."`
    - row 12: `"...they count the cans before they count the people..."`
    - row 13: `"...not cruel. Not kind. Careful..."`
    - row 14: `"...ask again tomorrow..."`
    - row 15: `"...they will trade, but not explain..."`
    - row 16: `"...one decision does not make a pattern..."`
    - row 17: `"...the camp has not chosen what to call them..."`
    - row 18: `"...the answer was practical. That is all..."`
    - row 19: `"...if you know what they need, you know what they will do..."`
    - row 20: `"...keep listening. The next choice matters more..."`
  - `slightly_evil`: list[20]
    - row 1: `"...didn't help, I heard..."`
    - row 2: `"...walked past someone. Just walked past..."`
    - row 3: `"...the merchant watches their hands now..."`
    - row 4: `"...count your own supplies..."`
    - row 5: `"...the promise bends when they need it..."`
    - row 6: `"...someone asked twice and still left empty-handed..."`
    - row 7: `"...don't hand over the whole bundle..."`
    - row 8: `"...the door opened after the price went up..."`
    - row 9: `"...they call it a choice..."`
    - row 10: `"...the watch learned to stand closer..."`
    - row 11: `"...keep a witness nearby..."`
    - row 12: `"...you can hear the apology coming after the damage..."`
    - row 13: `"...they know who cannot say no..."`
    - row 14: `"...one favor, three strings..."`
    - row 15: `"...the missing name was not an accident..."`
    - row 16: `"...no one wants to be first to complain..."`
    - row 17: `"...their kindness has a receipt..."`
    - row 18: `"...ask what happens if you refuse..."`
    - row 19: `"...they do not need to threaten you twice..."`
    - row 20: `"...we are not calling it betrayal yet..."`
  - `evil`: list[20]
    - row 1: `"...took from the dying one..."`
    - row 2: `"...the Keeper screamed for an hour after..."`
    - row 3: `"...lock your things tonight..."`
    - row 4: `"...the Peacekeepers are circulating a description..."`
    - row 5: `"...the clinic is counting before they answer..."`
    - row 6: `"...don't show them the full pack..."`
    - row 7: `"...they call it a bargain after taking the choice away..."`
    - row 8: `"...the watch turns people around at their door..."`
    - row 9: `"...someone stopped using their real name..."`
    - row 10: `"...the room empties when they arrive..."`
    - row 11: `"...keep the family out of the meeting..."`
    - row 12: `"...they remember debts and forget the help..."`
    - row 13: `"...no one volunteers a shortage..."`
    - row 14: `"...the trader brought an escort..."`
    - row 15: `"...the apology came with a new lock..."`
    - row 16: `"...they asked what could be spared, then decided..."`
    - row 17: `"...the surrender did not end the asking..."`
    - row 18: `"...people are weighing exits now..."`
    - row 19: `"...the safe route vanished from the map..."`
    - row 20: `"...you can hear the fear in the polite answers..."`
  - `very_evil`: list[20]
    - row 1: `"...that's them. Don't look. Don't look..."`
    - row 2: `"...the bounty says alive, but I've heard 'dead' works too..."`
    - row 3: `"...even the raiders are careful around..."`
    - row 4: `"...took a child's toy from a grave. A grave..."`
    - row 5: `"...I won't sleep until they're gone from this camp..."`
    - row 6: `"...the lower room first..."`
    - row 7: `"...do not leave the ledger out..."`
    - row 8: `"...the escort is not for the road..."`
    - row 9: `"...people stop speaking when that door opens..."`
    - row 10: `"...move the children before the meeting..."`
    - row 11: `"...they will call it an offer..."`
    - row 12: `"...nobody signs their name near that mark..."`
    - row 13: `"...wait until the yard is empty..."`
    - row 14: `"...hide the spare key..."`
    - row 15: `"...the trader brought two guards..."`
    - row 16: `"...let them think the shelves are bare..."`
    - row 17: `"...somebody moved the clinic stores..."`
    - row 18: `"...do not tell them who is missing..."`
    - row 19: `"...if they smile, check the exits..."`
    - row 20: `"...keep the children away from the door..."`
- `gossip_decay`: object[4]
- Full compact row audit contains 447 lines; first 300 retained here to keep the plan navigable. The complete file is identified above.
- Bytes: 27,959; SHA-256: `fabce75419bbfe57e5637338851bd0418a1e3056f49cfe39b61858df4361c0f8`
- Root keys: `camp_chatter, description, gossip_decay, npc_greeting_shifts, schema_version, whisper_lines`
- `camp_chatter`: object[8]
  - `very_positive`: list[20]
    - row 1: `"Did you hear? They gave their last medicine to that stranger's kid. The whole course."`
    - row 2: `"The one from the eastern shelter — people say they buried every body they found on the ridge. Every one."`
    - row 3: `"Someone told me they sat with old Mara all night. Didn't ask for anything. Just sat."`
    - row 4: `"I heard they turned down a clean trade route to carry a wounded stranger back to camp. On their back."`
    - row 5: `"The Peacekeepers are talking about them. Not the bad kind of talking."`
    - row 6: `"Word is they gave seeds to the last farmer on the plateau. The whole stock. Their own stock."`
    - row 7: `"They say even the raiders won't touch that one. Too much... weight, I guess. Too much weight behind the name."`
    - row 8: `"A child in the east quarter drew a picture of them. Calls them 'the kind one.' Hangs it on the wall."`
    - row 9: `"The Knowledge Keepers want to meet them. Not to ask — to give. That's new."`
    - row 10: `"I don't trust saints. But this one keeps showing up, and keeps giving, and hasn't asked for a single thing back."`
    - row 11: `"They gave the clinic its last clean bandage roll and waited for the next shipment."`
    - row 12: `"The outer-gate watch says they carried a frightened stranger in before checking their papers."`
    - row 13: `"They returned the trade scale with the extra measure still on it."`
    - row 14: `"Someone saw them leave the warm bunk for a sick traveler and never mention it."`
    - row 15: `"They kept the promise even after the promised thing became scarce."`
    - row 16: `"The ration pot went short, and they still put the youngest plates first."`
    - row 17: `"They let the wounded rival cross the yard under their protection."`
    - row 18: `"The repair crew got the part they needed because this one surrendered a useful spare."`
    - row 19: `"They told the truth about the missing cans before anyone had to ask."`
    - row 20: `"A lot of people talk brave; that one paid the cost and kept the door open."`
  - `positive`: list[20]
    - row 1: `"They helped the Peacekeeper on the north road. Shared supplies, I heard. Not nothing."`
    - row 2: `"People say they listened to old Gregor's war story. The whole thing. Even the ugly parts."`
    - row 3: `"Word around camp — they comforted the widow by the east fire. Sat with her through the bad hours."`
    - row 4: `"The merchant at the crossroads gives them a nod now. Fair trader, apparently."`
    - row 5: `"I heard they buried a body near the water tower. Properly. Not just kicked dirt over it."`
    - row 6: `"Someone said they gave food to that raider with the broken gun. The raider told them about the next raid."`
    - row 7: `"The doctor at the clinic speaks well of them. Something about patience. About listening."`
    - row 8: `"They stopped for the thirsty one on the desert road. Gave water. Got directions to a spring."`
    - row 9: `"Not a saint. But decent. That's worth noting, these days."`
    - row 10: `"The trade scale came back honest after they used it. The next crew noticed."`
    - row 11: `"They brought a spare lamp to the night watch instead of waiting for the fire to fail."`
    - row 12: `"The clinic got first pick of the clean cloth. Nobody had to argue for it."`
    - row 13: `"They stopped to carry a tired traveler through the gate and still made the meeting on time."`
    - row 14: `"They left room in the bunk row for people who had not arrived yet."`
    - row 15: `"They answered the hard questions without turning the whole thing into a performance."`
    - row 16: `"Someone says they sent word before changing the ration list. That counts as courtesy now."`
    - row 17: `"They had leverage and did not use all of it."`
    - row 18: `"They returned a borrowed tool before the repair shift asked for it."`
    - row 19: `"The route note was shared with the next crew. No price attached."`
    - row 20: `"They remembered who had been waiting when the meeting ran long."`
  - `slightly_positive`: list[20]
    - row 1: `"They helped someone on the road. I forget who. But they helped."`
    - row 2: `"People say they're not bad. Which is... something."`
    - row 3: `"The Peacekeepers don't frown when that name comes up. Low bar, but there it is."`
    - row 4: `"I heard they gave a little food to a stranger. Not much. But they gave."`
    - row 5: `"Someone said they listened to a story or two. Didn't mock anyone for telling one."`
    - row 6: `"Could have kept the whole crate. Didn't."`
    - row 7: `"I heard they waited their turn at the clinic."`
    - row 8: `"They asked what was needed before offering what they had."`
    - row 9: `"The gate stayed open for one more person, apparently because they asked."`
    - row 10: `"They corrected the count when nobody else noticed."`
    - row 11: `"Not generous, exactly. Just less hard than expected."`
    - row 12: `"Someone got a second chance after they listened."`
    - row 13: `"They let a stranger finish speaking before walking away."`
    - row 14: `"The deal was fair enough that nobody checked the pockets twice."`
    - row 15: `"They shared the route note, then acted like it was nothing."`
    - row 16: `"They carried one end of the stretcher. The other end did most of the work."`
    - row 17: `"The request was small, and they did not make it smaller."`
    - row 18: `"They gave back the tool with the blade still sharp."`
    - row 19: `"They could have made a scene. They chose the quieter answer."`
    - row 20: `"'Maybe they are learning,' someone said. Nobody argued."`
  - `neutral`: list[20]
    - row 1: `"Who? Oh, them. I don't know. They come and go. Keep to themselves."`
    - row 2: `"Never helped me. Never hurt me. That's the whole file on them."`
    - row 3: `"The name doesn't ring any bells. Which is probably fine."`
    - row 4: `"Neither the raiders nor the Peacekeepers mention them. That's a kind of safety."`
    - row 5: `"Average. In the old world that was an insult. Now it's a blessing."`
    - row 6: `"Ask three people what they think and get four answers."`
    - row 7: `"They'll help if the numbers work. That's not nothing."`
    - row 8: `"The door opens for some requests and stays shut for others."`
    - row 9: `"They trade clean, but they don't promise clean."`
    - row 10: `"Nobody knows whether the pause means caution or indifference."`
    - row 11: `"They keep a ledger. So does everyone who plans to last."`
    - row 12: `"One crew calls them fair; another calls them difficult."`
    - row 13: `"The answer changes when the weather does."`
    - row 14: `"They listen until the cost becomes clear."`
    - row 15: `"Somebody got shelter. Somebody else got a refusal."`
    - row 16: `"They brought supplies in and questions out."`
    - row 17: `"The same decision looks wise from one bunk and cold from another."`
    - row 18: `"They do not explain every choice, which leaves room for everyone else's version."`
    - row 19: `"If there is a pattern, it has not shown itself to me."`
    - row 20: `"Wait for the next request. That is usually where the truth is."`
  - `slightly_evil`: list[20]
    - row 1: `"Watch your gear around that one. I'm not saying they took anything. I'm saying watch it."`
    - row 2: `"They walked past someone on the road. I saw it. Just walked past."`
    - row 3: `"People say they refused a child. Could be rumor. Could be fact. The child remembers."`
    - row 4: `"The merchant at the crossroads doesn't give them credit anymore. Something happened."`
    - row 5: `"I don't like the look they give people who ask for help. The look says 'no' before the mouth does."`
    - row 6: `"Count your filters before you hand them over."`
    - row 7: `"They keep the letter of a promise and lose the part that mattered."`
    - row 8: `"Someone asked for help and got a price list instead."`
    - row 9: `"They remember who cannot afford to refuse."`
    - row 10: `"The door opened after the argument, not before."`
    - row 11: `"They do not raise their voice; they just make the alternative smaller."`
    - row 12: `"Keep a second route in mind when they offer the first."`
    - row 13: `"The ration list changed after one name disappeared from it."`
    - row 14: `"They call it efficiency when someone else goes without."`
    - row 15: `"The apology came after the supplies were already gone."`
    - row 16: `"Nobody says betrayal yet. They are waiting to see if it becomes a habit."`
    - row 17: `"A promise from them needs a witness now."`
    - row 18: `"The outer watch learned not to ask twice."`
    - row 19: `"They offered a choice with only one affordable answer."`
    - row 20: `"'Don't make them angry' is becoming part of the instructions."`
  - `evil`: list[20]
    - row 1: `"Stay away from that one. I mean it. People who cross their path end up lighter."`
    - row 2: `"They took from the injured man on the east road. Took his pack while he watched."`
    - row 3: `"Word is they robbed the Knowledge Keeper. Ripped the notes right out of their hands."`
    - row 4: `"The Peacekeepers have a description circulating. Matches that one pretty well."`
    - row 5: `"A raider told me even their people talk about this one — and not with admiration. With caution."`
    - row 6: `"Someone died on the south road. Didn't have to. This one walked past, pockets heavier than before."`
    - row 7: `"The farmer's seed locker is empty. You know who did it. I know who did it. The fence knows."`
    - row 8: `"Don't trade with them. The last person who did found their kit missing the next morning."`
    - row 9: `"People lower their voices when that patrol comes through."`
    - row 10: `"The clinic started counting its bandages before answering their knock."`
    - row 11: `"They call a threat a negotiation and wait for gratitude."`
    - row 12: `"The trader's new escort is not for the road. It is for meetings here."`
    - row 13: `"A surrender is only an invitation to lose something, according to them."`
    - row 14: `"The outer bunks empty when their people ask for space."`
    - row 15: `"They keep the useful part of every bargain and forget the promise."`
    - row 16: `"Someone stopped carrying a family name in public after dealing with them."`
    - row 17: `"No one volunteers a shortage around that table."`
    - row 18: `"They took the safe route off the map and charged for directions."`
    - row 19: `"The apology was posted beside the new lock."`
    - row 20: `"Their offers sound generous until you count who cannot refuse."`
  - `very_evil`: list[20]
    - row 1: `"Lock your shelter. That one is in the area."`
    - row 2: `"The Peacekeepers posted a bounty. You know whose face is on it."`
    - row 3: `"I watched them take from a dying woman. Her last bundle. While she tried to curse them."`
    - row 4: `"The raiders respect them. Think about what that means. Think hard."`
    - row 5: `"A child won't go near the east path anymore. Says 'the bad one' lives there."`
    - row 6: `"They say you can trade with them — but only if you have something they can't just take."`
    - row 7: `"The Knowledge Keepers tell their children about this one. As a warning."`
    - row 8: `"Someone put a mark on the wall outside their shelter. A red mark. Everyone knows what it means."`
    - row 9: `"The old man by the fire stopped telling stories. Says there's no point. Not with this one around."`
    - row 10: `"Even the dogs cross the road when they see them coming. Animals know."`
    - row 11: `"No one leaves a child alone near the outer door when their shadow is on the road."`
    - row 12: `"The traders bring an escort and a second set of scales."`
    - row 13: `"People erase names from the request board before this one can read them."`
    - row 14: `"The lower room fills early whenever their delegation is expected."`
    - row 15: `"They do not ask what a person can spare. They ask what can be taken."`
    - row 16: `"A friendly wave from that direction means check the exits."`
    - row 17: `"Witnesses stopped signing the ledger. They know where the pages go."`
    - row 18: `"The camp keeps a quiet route for anyone who needs to disappear."`
    - row 19: `"Somebody moved the clinic stores before the meeting began."`
    - row 20: `"They call it peace when everyone else has run out of choices."`
- `npc_greeting_shifts`: object[8]
  - `very_positive`: list[20]
    - row 1: `"Oh — you're the one they talk about. The kind one. What can I do for you?"`
    - row 2: `"I heard what you did for the family on the ridge. Thank you. Truly."`
    - row 3: `"You don't have to buy anything. Sit. Eat. You've earned that much."`
    - row 4: `"My daughter drew a picture of you. She's never met you. That's how far your name travels."`
    - row 5: `"The Peacekeepers told us about you. We saved you a share. Here."`
    - row 6: `"You made it. Come in before the cold takes the choice out of your hands."`
    - row 7: `"We've got a place by the stove. Take it; nobody is keeping score."`
    - row 8: `"Tell me what the clinic needs. We'll sort our share first."`
    - row 9: `"You're welcome at this table. If you brought trouble, we can talk about that too."`
    - row 10: `"I heard you kept the gate open. What can I set aside for you?"`
    - row 11: `"Good morning. You look tired; use the good cup."`
    - row 12: `"We can start with the difficult part. I won't make you earn the conversation."`
    - row 13: `"There is room inside. Bring whoever is waiting with you."`
    - row 14: `"You have my attention. Take the time you need."`
    - row 15: `"If you need a witness, I will stand there."`
    - row 16: `"No need to bargain over the first bowl. Eat."`
    - row 17: `"You kept your word before. Let's see what you need today."`
    - row 18: `"Come closer; the fire is shared."`
    - row 19: `"We saved the clean bandages for people who come back through the gate. Take some."`
    - row 20: `"You can ask plainly here. Plain answers are still available."`
  - `positive`: list[20]
    - row 1: `"Ah, you. I've heard good things. What do you need?"`
    - row 2: `"The merchant said you trade fair. That's enough for me. What can I offer?"`
    - row 3: `"You helped someone I know. So I'll help you. Fair's fair."`
    - row 4: `"I trust you more than most. Which isn't saying much, but it's saying something."`
    - row 5: `"Morning. Need the fair price or the honest directions?"`
    - row 6: `"Good to see you. We can work something out."`
    - row 7: `"Someone vouched for you at the gate. Start with what you need."`
    - row 8: `"Come in; the door is open while you are here."`
    - row 9: `"I kept your last message. What changed?"`
    - row 10: `"You can look over the stock before we name a price."`
    - row 11: `"If this is about the clinic, I can point you to the right queue."`
    - row 12: `"We have a little time. Use it well."`
    - row 13: `"I can spare a hand, a can, or an answer. Which one?"`
    - row 14: `"You don't have to explain the whole road. Tell me where it hurts."`
    - row 15: `"The watch knows your name. You won't be delayed."`
    - row 16: `"Trade first, questions after, unless you want it the other way."`
    - row 17: `"You showed up when it mattered. What matters now?"`
    - row 18: `"Take the dry seat. We can talk over the noise."`
    - row 19: `"If someone sent you, say who. If you came yourself, that's fine too."`
    - row 20: `"You are still welcome to ask."`
  - `slightly_positive`: list[20]
    - row 1: `"You seem alright. What are you after?"`
    - row 2: `"Haven't heard anything bad about you. That's practically a recommendation."`
    - row 3: `"You can sit if you want. Fire's open."`
    - row 4: `"Come in if you like. I can spare a minute."`
    - row 5: `"You kept that last answer simple. Let's try again."`
    - row 6: `"I remember you. That is not a complaint."`
    - row 7: `"You can stand by the door or take the chair. Your choice."`
    - row 8: `"Need something? Say it before I decide what you mean."`
    - row 9: `"We can talk. Just don't promise more than you can carry."`
    - row 10: `"You returned the tool, so I am listening."`
    - row 11: `"I have not forgotten the fair count."`
    - row 12: `"Start with the part that costs you."`
    - row 13: `"You can ask for help without making a speech."`
    - row 14: `"I was expecting worse. Good morning."`
    - row 15: `"If you are here to trade, set it down where I can see it."`
    - row 16: `"There is room at the edge of the fire."`
    - row 17: `"You did one decent thing. Let's not spend it twice."`
    - row 18: `"I can give you an answer, maybe not the one you want."`
    - row 19: `"We have met before. Try not to make this awkward."`
    - row 20: `"Say what you need; I will decide what I can spare."`
  - `neutral`: list[20]
    - row 1: `"What do you want?"`
    - row 2: `"Trading or passing through?"`
    - row 3: `"I don't know you. State your business."`
    - row 4: `"Sit if you want. Leave if you want. Makes no difference to me."`
    - row 5: `"You're here. What do you need?"`
    - row 6: `"We can talk. Keep it brief."`
    - row 7: `"If you are trading, show the goods."`
    - row 8: `"I have a few minutes. Use them carefully."`
    - row 9: `"What brings you to this door?"`
    - row 10: `"You know the arrangement. State your request."`
    - row 11: `"Sit or stand; either way, say what this is about."`
    - row 12: `"I can listen before I decide."`
    - row 13: `"No promises from either side. That works for me."`
    - row 14: `"You want something. Start there."`
    - row 15: `"The ledger is open. The answer may not be."`
    - row 16: `"If this is a complaint, make it specific."`
    - row 17: `"We have dealt before. Let us see if that helps."`
    - row 18: `"You can ask. I can say no."`
    - row 19: `"The room is public; the conversation is not."`
    - row 20: `"Tell me what changed since last time."`
  - `slightly_evil`: list[20]
    - row 1: `"Keep your distance. We can talk from here."`
    - row 2: `"I know your face. I'll trade, but I'm watching my pack."`
    - row 3: `"What do you want? Make it quick."`
    - row 4: `"The children go inside when you come around. You notice that?"`
    - row 5: `"We can talk from here. Do not cross the line."`
    - row 6: `"Set the pack down before you sit."`
    - row 7: `"I remember the last promise. Bring a witness this time."`
    - row 8: `"Make your request and keep your hands visible."`
    - row 9: `"There is no reason to raise your voice. There is also no reason to trust it."`
    - row 10: `"You get the outer table. The inner door stays shut."`
    - row 11: `"If you want credit, bring someone who can vouch for you."`
    - row 12: `"Do not make me count the supplies twice."`
    - row 13: `"You can trade, but nothing leaves this room unmarked."`
    - row 14: `"I have heard enough versions. Give me the one you can prove."`
    - row 15: `"Keep to the lit side of the yard."`
    - row 16: `"Ask for what you need, not what you think we owe."`
    - row 17: `"You are allowed to speak. That is not the same as being agreed with."`
    - row 18: `"The children are inside. Choose your words."`
    - row 19: `"We will finish this before the watch changes."`
    - row 20: `"If you came for forgiveness, find another door."`
  - `evil`: list[20]
    - row 1: `"You. I know what you did. Say your piece and go."`
    - row 2: `"My hand's on my knife. Don't take it personally. Actually, do."`
    - row 3: `"Last person who looked at me like that lost a tooth. State your business."`
    - row 4: `"I'd spit, but you're not worth the moisture."`
    - row 5: `"State your business. The inner door stays closed."`
    - row 6: `"Leave the pack where I can see it."`
    - row 7: `"You get one answer. Make it useful."`
    - row 8: `"Do not call this a favor."`
    - row 9: `"We remember the last deal. You do not get the soft version."`
    - row 10: `"The watch will hear this conversation."`
    - row 11: `"If you are asking for supplies, bring the count and the reason."`
    - row 12: `"No room at this table. Use the public bench."`
    - row 13: `"You can speak; nobody promised to help."`
    - row 14: `"Do not reach for the ledger."`
    - row 15: `"The gate is open for trade, not trust."`
    - row 16: `"If you came to bargain, start with what you owe."`
    - row 17: `"Keep your voice down. I do not want this mistaken for welcome."`
    - row 18: `"The clinic will see you after the others. You know why."`
    - row 19: `"Leave before someone thinks I invited you."`
    - row 20: `"We are not discussing what happened. We are discussing what you will do now."`
  - `very_evil`: list[20]
    - row 1: `"Get away from my fire. Now."`
    - row 2: `"I know your face. Everyone does. The bounty poster made sure of that."`
    - row 3: `"You come any closer and I scream. The whole camp comes running."`
    - row 4: `"I have nothing you can take. Leave."`
    - row 5: `"The mark on my door is for you. You know what it means. Walk on."`
    - row 6: `"Stop at the line. I will find someone else to hear you."`
    - row 7: `"The fire is not yours. Keep walking."`
    - row 8: `"You can leave your message with the watch."`
    - row 9: `"Do not say my name like we are friends."`
    - row 10: `"The door stays shut. So does the conversation."`
    - row 11: `"Take one more step and the whole yard hears it."`
    - row 12: `"We moved the stores before you arrived."`
    - row 13: `"If you need help, ask someone who still trusts you."`
    - row 14: `"No trade here. Not after the last one."`
    - row 15: `"You may wait outside until daylight."`
    - row 16: `"Do not bring a child to this door."`
    - row 17: `"Whatever you want, the answer is no."`
    - row 18: `"I will not be alone for this conversation."`
    - row 19: `"The room is full. It was full before you came."`
    - row 20: `"Leave the marker where you found it and go."`
- `whisper_lines`: object[8]
  - `very_positive`: list[20]

## `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json`
- Bytes: 14,988; SHA-256: `fd9f4c31b6dd1c9779bc820d7b02923901257b704f930c3572e6d2d57933950d`
- Root keys: `description, schema_version, threshold_reactions`
- `threshold_reactions`: object[6]
  - `moral_event_bounty_issued`: object[5]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, journal_entry`
  - `moral_event_contract_taken`: object[5]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, journal_entry`
  - `moral_event_contract_raised`: object[5]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, journal_entry`
  - `moral_event_patrol_defense`: object[5]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, journal_entry`
  - `moral_event_legend_positive`: object[6]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, civilian_dialogue, journal_entry`
  - `moral_event_legend_negative`: object[6]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, civilian_dialogue, journal_entry`
- Bytes: 14,988; SHA-256: `fd9f4c31b6dd1c9779bc820d7b02923901257b704f930c3572e6d2d57933950d`
- Root keys: `description, schema_version, threshold_reactions`
- `threshold_reactions`: object[6]
  - `moral_event_bounty_issued`: object[5]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, journal_entry`
  - `moral_event_contract_taken`: object[5]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, journal_entry`
  - `moral_event_contract_raised`: object[5]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, journal_entry`
  - `moral_event_patrol_defense`: object[5]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, journal_entry`
  - `moral_event_legend_positive`: object[6]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, civilian_dialogue, journal_entry`
  - `moral_event_legend_negative`: object[6]; keys: `event_description, peacekeeper_dialogue, raider_dialogue, knowledge_keeper_dialogue, civilian_dialogue, journal_entry`

# Appendix D — Current caller/reference graph

### `MoralChoiceChainCatalogLoader` (18 sampled current references)
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:401: ["moral_choice_chains.json"] = new[] { "MoralChoiceChainCatalogLoader" },
- Assets/Ashfall.Core/MoralChoice/MoralChoiceChainCatalogLoader.cs:87: public static class MoralChoiceChainCatalogLoader
- src/Main.MoralChoice.cs:56: _moralChainData = MoralChoiceChainCatalogLoader.Load(_dataDir, fileIO, json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:22: var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:33: var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:45: var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:58: var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:66: var data = MoralChoiceChainCatalogLoader.Load("/no/such/dir", s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:222: var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:273: var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:287: var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:316: var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:345: var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:361: var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:388: var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:413: var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:565: var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:97: MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
### `FindAvailableEchoQuests` (18 sampled current references)
- Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs:378: public List<MoralEchoQuestDefinition> FindAvailableEchoQuests(int currentDay)
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:376: var available = sys.FindAvailableEchoQuests(30);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:380: available = sys.FindAvailableEchoQuests(40);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:402: var available = sys.FindAvailableEchoQuests(100);
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:427: var available = sys.FindAvailableEchoQuests(100);
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:307: var preDelay = sys.FindAvailableEchoQuests(189);
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:311: var exactDay = sys.FindAvailableEchoQuests(190);
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:318: var wrongChoice = sys2.FindAvailableEchoQuests(200);
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:346: Assert.DoesNotContain(sys.FindAvailableEchoQuests(56), e => e.QuestId == "quest_moral_echo_aldric_blockade_retaliation");
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:349: Assert.Contains(sys.FindAvailableEchoQuests(57), e => e.QuestId == "quest_moral_echo_aldric_blockade_retaliation");
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:377: Assert.DoesNotContain(sys.FindAvailableEchoQuests(56), e => e.QuestId == "quest_moral_echo_defector_corroborates_truth");
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:380: Assert.Contains(sys.FindAvailableEchoQuests(57), e => e.QuestId == "quest_moral_echo_defector_corroborates_truth");
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:408: Assert.DoesNotContain(sys.FindAvailableEchoQuests(69), e => e.QuestId == "quest_moral_echo_kessler_exile_uncovered");
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:411: Assert.Contains(sys.FindAvailableEchoQuests(70), e => e.QuestId == "quest_moral_echo_kessler_exile_uncovered");
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:453: var available = sys.FindAvailableEchoQuests(200);
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:476: Assert.Contains(sys.FindAvailableEchoQuests(200), e => e.QuestId == "quest_moral_echo_raider_repaid_warning");
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:480: Assert.DoesNotContain(sys.FindAvailableEchoQuests(200), e => e.QuestId == "quest_moral_echo_raider_repaid_warning");
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:511: var avail1 = sys1.FindAvailableEchoQuests(100).Select(e => e.QuestId).ToList();
### `MarkEchoQuestFired` (5 sampled current references)
- Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs:398: public void MarkEchoQuestFired(string echoQuestId)
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:425: sys.MarkEchoQuestFired("quest_moral_echo_child_returns");
- Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs:570: sys.MarkEchoQuestFired("quest_moral_echo_child_returns");
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:458: public void Runtime_MarkEchoQuestFired_PreventsRefiring()
- Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs:478: sys.MarkEchoQuestFired("quest_moral_echo_raider_repaid_warning");
### `MoralChoiceSaveStore` (11 sampled current references)
- src/Main.MoralChoice.cs:79: var save = MoralChoiceSaveStore.TryLoad();
- src/Main.MoralChoice.cs:335: if (CaptureSection("moral_choice", MoralChoiceSaveStore.TryCapturePersisted(_moralChoice.CaptureState())))
- src/Host/MoralChoiceSaveStore.cs:3: // Save Store : MoralChoiceSaveStore
- src/Host/MoralChoiceSaveStore.cs:20: public static class MoralChoiceSaveStore
- src/Host/MoralChoiceSaveStore.cs:26: SaveStoreHub.Checksummed<MoralChoiceState>(FileName, nameof(MoralChoiceSaveStore));
- src/Host/HostCli.MoralChoice.cs:93: string path = Path.Combine(tmpDir, MoralChoiceSaveStore.FileName);
- src/Host/HostCli.MoralChoice.cs:96: MoralChoiceSaveStore.Save(sys.CaptureState(), path);
- src/Host/HostCli.MoralChoice.cs:97: var loaded = MoralChoiceSaveStore.TryLoad(path);
- src/Host/HostCli.MoralChoice.cs:114: Check(MoralChoiceSaveStore.TryLoad(path) == null, "tampered save rejected by checksum");
- src/Host/HostCli.MoralChoice.cs:119: Check(MoralChoiceSaveStore.TryLoad(path) == null, "empty-checksum envelope rejected as corrupt");
- Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs:13: //   MoralChoiceSaveStore, MusterSaveStore, NarrativeSaveStore, PhantomMemorySaveStore,
### `echo_consequence_due` (2 sampled current references)
- Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs:196: { "echo_consequence_due", SemanticKind.Narrative },
- src/Main.CampaignOwners.cs:2204: "echo_consequence_due",

# Appendix E — Current focused-test inventory

Current test declaration inventory: 156 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs` — 32 test declarations; bytes=23,695; SHA-256=`d97da758b073e2ea3d621eb78422436230d54d0e469939c184ff2fa9caa93464`
- 00113: [Fact]
- 00114: public void Catalog_LoadsExactlySixtyEchoQuests()
- 00120: [Fact]
- 00121: public void Catalog_EchoQuestIds_AreUniqueAndCanonical()
- 00136: [Fact]
- 00137: public void Catalog_PreservesAllThirtyTwoBaselineEchoes()
- 00151: [Fact]
- 00152: public void Catalog_TwentyEightNewEchoes_MatchRequiredBranchDistribution()
- 00191: [Fact]
- 00192: public void ReferenceIntegrity_AllTriggeredByResolveInQuestCatalogs()
- 00206: [Fact]
- 00207: public void ReferenceIntegrity_AllTwentyEightNewTriggeredByResolveInQuestGates()
- 00227: [Fact]
- 00228: public void ChoiceIndexIntegrity_AllChoicesAreValidForSourceQuests()
- 00244: [Fact]
- 00245: public void TemporalReachability_AllEchoesReachableWithinCampaignHorizon()
- 00264: [Fact]
- 00265: public void BranchConsistency_EchoBranchMatchesGatedQuestBranch()
- 00282: [Fact]
- 00283: public void Runtime_MercyEcho_FiresOnlyAfterDelayAndMatchingChoice()
- 00322: [Fact]
- 00323: public void Runtime_IronEcho_FiresOnlyAfterDelayAndMatchingChoice()
- 00352: [Fact]
- 00353: public void Runtime_ListenerEcho_FiresOnlyAfterDelayAndMatchingChoice()
- 00383: [Fact]
- 00384: public void Runtime_BrokenCompactEcho_FiresOnlyAfterDelayAndMatchingChoice()
- 00414: [Fact]
- 00415: public void Runtime_BranchLockout_SuppressesLockedEchoQuests()
- 00457: [Fact]
- 00458: public void Runtime_MarkEchoQuestFired_PreventsRefiring()
- 00483: [Fact]
- 00484: public void Runtime_Arbitration_DeterministicOrdering()
### `Ashfall.Core.Tests/MoralChoiceSystemTests.cs` — 44 test declarations; bytes=17,283; SHA-256=`43c39778ac33913ad3ea4cb255b57fb54511909b6560e169bb599e20dc6ed4ec`
- 00039: [Fact]
- 00040: public void InitialStateNeutralAndEmpty()
- 00052: [Fact]
- 00053: public void BandEdgesPinned()
- 00087: [Fact]
- 00088: public void ResolveAppliesDeltasAndRaisesEvent()
- 00108: [Fact]
- 00109: public void ResolveIsIdempotentPerQuest()
- 00123: [Fact]
- 00124: public void ScoreClampsAndLegendSettlesAtReconcileOncePerDirection()
- 00146: [Fact]
- 00147: public void ResolveRejectsNonCanonicalQuestId()
- 00153: [Fact]
- 00154: public void ResolveRejectsOutOfRangeChoice()
- 00162: [Fact]
- 00163: public void ImpactMarksFollowDeltaSign()
- 00171: [Fact]
- 00172: public void SameSeedSameRolls()
- 00187: [Fact]
- 00188: public void ReconcileFiresExtremeBandEventsOnce()
- 00216: [Fact]
- 00217: public void ReconcileFiresAllCrossedBandsOnBigJump()
- 00235: [Fact]
- 00236: public void ReconcileFiresContractAtPositiveBand()
- 00248: [Fact]
- 00249: public void ReconcileIgnoresOutOfOrderDays()
- 00257: [Fact]
- 00258: public void EndingStorykeeperOverridesBand()
- 00272: [Fact]
- 00273: public void EndingSelectionRules()
- 00303: [Fact]
- 00304: public void StorykeeperNeedsBothThresholds()
- 00311: [Fact]
- 00312: public void ListenerAndConfidantThresholdsPinnedAtBoundary()
- 00329: [Fact]
- 00330: public void RestoreRejectsMismatchedSystemAndBadSchema()
- 00345: [Fact]
- 00346: public void PendingLegendFlagsSurviveRoundTrip()
- 00363: [Fact]
- 00364: public void SaveRoundTripPreservesLedger()
- 00395: [Fact]
- 00396: public void CapturedStateIsDetached()
- 00410: [Fact]
- 00411: public void AvailabilityWindow()
### `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs` — 72 test declarations; bytes=29,774; SHA-256=`52feaf16447c30403ed9a14ec9a3c991dfb6d2c0ec873633e0821e42cf5d1df3`
- 00019: [Fact]
- 00020: public void ChainCatalogLoadsFourBranches()
- 00030: [Fact]
- 00031: public void ChainCatalogHasQuestGates()
- 00042: [Fact]
- 00043: public void ChainCatalogHasEchoQuests()
- 00055: [Fact]
- 00056: public void ChainCatalogLockoutRulesArePermanent()
- 00063: [Fact]
- 00064: public void ChainCatalogMissingFileReturnsEmpty()
- 00073: [Fact]
- 00074: public void BranchingQuestsLoadAllFourChains()
- 00084: [Fact]
- 00085: public void BranchingQuestsAllChainsComplete()
- 00098: [Fact]
- 00099: public void BranchingQuestsHaveValidChoices()
- 00117: [Fact]
- 00118: public void ExpansionQuestsLoadFiftyQuests()
- 00125: [Fact]
- 00126: public void ExpansionQuestIdsMatchStaticList()
- 00137: [Fact]
- 00138: public void GossipCatalogLoadsAllBands()
- 00148: [Fact]
- 00149: public void GossipCatalogHasNpcGreetings()
- 00157: [Fact]
- 00158: public void GossipCatalogHasDecayRules()
- 00168: [Fact]
- 00169: public void FactionReactionsLoadAllThresholdEvents()
- 00179: [Fact]
- 00180: public void FactionReactionsHaveDialogue()
- 00191: [Fact]
- 00192: public void FlagCatalogLoadsTwentyFiveFlags()
- 00203: [Fact]
- 00204: public void FlagCatalogIdsMatchStaticList()
- 00218: [Fact]
- 00219: public void BranchTracking_LocksOutOpposingBranches()
- 00269: [Fact]
- 00270: public void BranchTracking_LockedBranchBlocksAccessibility()
- 00283: [Fact]
- 00284: public void BranchTracking_GateRequiresMoralThreshold()
- 00312: [Fact]
- 00313: public void BranchTracking_GateRequiresPriorQuestResolved()
- 00341: [Fact]
- 00342: public void BranchTracking_BranchLockFlagsAreSet()
- 00357: [Fact]
- 00358: public void EchoQuests_AvailableAfterTriggerAndDelay()
- 00384: [Fact]
- 00385: public void EchoQuests_NotAvailableForWrongChoice()
- 00409: [Fact]
- 00410: public void EchoQuests_MarkFiredPreventsRefire()
- 00433: [Fact]
- 00434: public void GossipRuntime_ReturnsCorrectBandChatter()
- 00449: [Fact]
- 00450: public void GossipRuntime_PickReturnsNonEmpty()
- 00460: [Fact]
- 00461: public void GossipRuntime_DecayToNeutralAfterFullDecay()
- 00486: [Fact]
- 00487: public void GossipRuntime_DecayOneLevelAfterInterval()
- 00518: [Fact]
- 00519: public void GossipRuntime_StaysNeutralBeforePropagation()
- 00561: [Fact]
- 00562: public void SaveRoundTrip_PreservesBranchTracking()
- 00587: [Fact]
- 00588: public void StaticIds_AllChainHasOneHundredEntries()
- 00597: [Fact]
- 00598: public void StaticIds_AllExpansionHasFiftyEntries()
- 00605: [Fact]
- 00606: public void StaticIds_AllFlagsHasTwentySixEntries()
- 00613: [Fact]
- 00614: public void StaticIds_AllBranchesHasFourEntries()
- 00619: [Fact]
- 00620: public void StaticIds_ChainQuestsFollowNamingPattern()
### `Ashfall.Core.Tests/Journeys/MoralChoiceJourneyTests.cs` — 8 test declarations; bytes=13,186; SHA-256=`7266e91fecfef7f8858b547f105f7ab7d972c008ce8a6701b6e9c29b8cf5c262`
- 00018: [Fact]
- 00019: public void MoralChoiceJourney_Encounter_Resolve_SaveReload_PreventsDuplicateResolution()
- 00113: [Fact]
- 00114: public void Journey_J1_MoralChoice_DecisionSpine_JournalConsequence_SaveReload_Idempotent()
- 00222: [Fact]
- 00223: public void MoralChoice_SaveEnvelope_RoundTripsResolvedLedger()
- 00260: [Fact]
- 00261: public void HostWiring_SaveAllAndProcessFlush_EnrollMoralChoice()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Moral Choice Echoes: Sixty-Quest Chain Corpus, Exactly-Once Delivery, and Existing Quest Ownership` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan calls the 60 rows zero-new-code completion; current production caller evidence is separated from historical content completion.
- The historical plan names journal/confession/epilogue wires without current proof; those are residual candidates only.

## H.2 Evidence-strength corrections
- Prove live callback reachability rather than trusting data counts.
- Separate fired eligibility from presentation acceptance.
- Protect the sealed radio/distress boundary.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Moral Choice Echoes: Sixty-Quest Chain Corpus, Exactly-Once Delivery, and Existing Quest Ownership while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use MoralChoiceChainData/Loader for definitions only.
- Use MoralChoiceSystem for resolution, eligibility, branch locks, and fired ids.
- Use the existing quest/encounter owner for presentation and acceptance.
- Use existing journal/gossip/faction/ending consumers for consequences.
- Persist through moral_choice and canonical quest state.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement moral choice echoes: sixty-quest chain corpus, exactly-once delivery, and existing quest ownership arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- No callback is claimed live without a production caller and presentation owner.
- No new quest lifecycle or fired ledger is authorized.

## J.2 Questions deliberately left open
- Which existing quest owner is the canonical echo presentation surface?
- Should an echo be accepted, dismissed, or resolved by the current quest runtime?
- How should branch-agnostic echoes be ordered against branch-specific echoes?

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

The original file at `HEAD:piagentsplans/109-moral-choice-echo-quests-expansion.md` contained 4,842 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 109 — Moral Choice Echo Quests Expansion (32 → 60 echo quests)

## Goal (2 lines)
Expand the `echo_quests.quests` array in `moral_choice_chains.json` from 32
callback quests to 60. The MoralChoiceSystem (`MoralChoiceSystem.cs` confirmed
live) fires echo quests when a prior moral quest was resolved a specific way,
referencing the earlier choice and presenting delayed consequences. 32 echoes
across 4 branches is too few to make early moral decisions feel like they echo
through the whole campaign.

## Why (P2)
- Verified: `moral_choice_chains.json` has 4 branches (by design — the 4
  permanent paths), 88 quest_gates (healthy), but only 32 echo_quests. The
  echo layer is the thinnest part of the moral-choice system and the part that
  makes choices *feel* like they matter weeks later.
- Echo quests are the single best lever for the "choices that echo later"
  design goal. A mercy decision on Day 20 should produce a callback on Day 60.
  32 echoes across ~88 gated quests means most choices have no delayed payoff.
- Pure DATA work — zero new Core code. `MoralChoiceChainCatalogLoader.cs`
  loads the array; `MoralChoiceSystem.cs` fires echoes by matching
  `triggered_by` + `triggered_by_choice` against resolved quest history.

## Files to touch
- `Assets/StreamingAssets/Data/moral_choice_chains.json` (expand
  `echo_quests.quests` 32 → 60)
- Read-only: `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainData.cs`
  (confirm echo quest DTO fields)
- Read-only: `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` (confirm
  how `triggered_by` / `triggered_by_choice` / `min_days_after` resolve)

## Content grammar (per echo quest)
- `quest_id`: snake_case, prefix `quest_moral_echo_` (confirmed convention).
- `triggered_by`: the quest_id of the earlier moral quest whose resolution
  fires this echo.
- `triggered_by_choice`: integer index into the source quest's choices, or
  `null` if any resolution fires it.
- `min_days_after`: minimum days between the triggering resolution and this
  echo becoming available (creates the "delayed payoff" feeling).
- `branch`: the branch this echo belongs to, or `null` if branch-agnostic.

## Steps
1. Read `MoralChoiceChainData.cs` to confirm the echo quest DTO and that no
   additional fields are required by the loader.
2. Read `MoralChoiceSystem.cs` to confirm how `triggered_by` is matched
   against resolved quest history (by id only, or id + choice index).
3. Inventory all 88 `quest_gates` and the 32 existing echoes; map which gated
   quests currently have no echo callback.
4. Author 28 new echo quests distributed across the 4 branches:
   - 8 mercy-road echoes (delayed kindness payoffs — the person you helped
     returns, the child you fed grows, the raider you spared warns you).
   - 8 iron-way echoes (delayed ruthlessness consequences — the camp you
     raided rebuilt and remembers, the survivor you abandoned left a note).
   - 7 listener-thread echoes (delayed wisdom callbacks — the story you
     collected becomes useful, the witness you interviewed resurfaces).
   - 5 broken-compact echoes (delayed betrayal fallout — the trust you
     weaponized collapses, the ally you sold out finds you).
5. Each echo: distinct `triggered_by` (reference a real gated quest),
   meaningful `min_days_after` (20–80 days for delayed payoff), correct
   `branch`.
6. Ensure no two echoes share the same `quest_id`; every `triggered_by`
   resolves to an existing quest_id in `quest_gates`.
7. Wire 6 echoes into Plan 95 (journal voice — echo resolutions trigger
   journal entries).
8. Wire 4 echoes into Plan 88 (confessions — echo survivors may confess).
9. Wire 3 echoes into Plan 89 (epilogues — echo outcomes feed ending
   determination).
10. Validate: `--data-integrity-selftest` (all triggered_by ids resolve).
11. xUnit: moral choice chain catalog loads 60 echo quests, all triggered_by
    resolve to existing quest_gates, all quest_ids unique.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `triggered_by_choice` indexing (step 2):
confirm whether choice indices are 0-based and whether `null` means "any
resolution" before authoring.

## Definition of Done
- `moral_choice_chains.json` echo_quests.quests has 60 entries, all
  triggered_by resolving, all quest_ids unique, 6 wired to journal voice, 4 to
  confessions, 3 to epilogues, integrity + tests green.

## Follow-on
- Plan 95 (journal voice) — echo resolutions trigger journal entries.
- Plan 88 (confessions) — echo survivors may confess.
- Plan 89 (epilogues) — echo outcomes determine endings.
- Plan 110 (gossip) — echo events propagate as camp chatter.
- Plan 100 (faction reactions) — echo choices shift faction standing.

```

## End of Plan 109 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainData.cs` — 77 lines; 3,351 bytes; SHA-256 `ab1e4581fd3d96cdbee02088e4b2872047e2cc10c2d54d232b3bfc35d3722d9e`
Declaration index:
- 00012: public sealed class MoralChoiceChainData
- 00021: public sealed class MoralBranchDefinition
- 00033: public sealed class MoralMergeRules
- 00041: public sealed class MoralLockoutRules
- 00053: public sealed class MoralQuestGate
- 00069: public sealed class MoralEchoQuestDefinition
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003:
00004: namespace Ashfall.Core.MoralChoice
00005: {
00006:     /// <summary>
00007:     /// Wire shape for moral_choice_chains.json — the branching architecture.
00008:     /// Defines 4 permanent branches (Mercy Road, Iron Way, Listener's Thread,
00009:     /// Broken Compact), merge-back rules, lockout rules, quest gates, and
00010:     /// echo quest triggers.
00011:     /// </summary>
00012:     public sealed class MoralChoiceChainData
00013:     {
00014:         public List<MoralBranchDefinition> Branches { get; set; } = new List<MoralBranchDefinition>();
00015:         public MoralMergeRules MergeRules { get; set; } = new MoralMergeRules();
00016:         public MoralLockoutRules LockoutRules { get; set; } = new MoralLockoutRules();
00017:         public List<MoralQuestGate> QuestGates { get; set; } = new List<MoralQuestGate>();
00018:         public List<MoralEchoQuestDefinition> EchoQuests { get; set; } = new List<MoralEchoQuestDefinition>();
00019:     }
00020:
00021:     public sealed class MoralBranchDefinition
00022:     {
00023:         public string Id { get; set; } = string.Empty;
00024:         public string DisplayName { get; set; } = string.Empty;
00025:         public string Description { get; set; } = string.Empty;
00026:         public int LockThreshold { get; set; }
00027:         public List<string> LocksOut { get; set; } = new List<string>();
00028:         public List<string> MergeAllowed { get; set; } = new List<string>();
00029:         public List<string> EntryQuests { get; set; } = new List<string>();
00030:         public string LockedFlag { get; set; } = string.Empty;
00031:     }
00032:
00033:     public sealed class MoralMergeRules
00034:     {
00035:         public string Description { get; set; } = string.Empty;
00036:         public string MergeQuestPrefix { get; set; } = string.Empty;
00037:         public int MergeQuestsRequireMinProgress { get; set; }
00038:         public bool MergeNeverUnlocksExclusive { get; set; }
00039:     }
00040:
00041:     public sealed class MoralLockoutRules
00042:     {
00043:         public string Description { get; set; } = string.Empty;
00044:         public bool LockoutIsPermanent { get; set; }
00045:         public bool LockoutFiresJournalEntry { get; set; }
00046:         public string LockoutJournalTemplate { get; set; } = string.Empty;
00047:     }
00048:
00049:     /// <summary>
00050:     /// Gate for a chain quest: prerequisites (prior quest in chain), optional
00051:     /// moral/empathy/flag requirements, and branch ownership.
00052:     /// </summary>
00053:     public sealed class MoralQuestGate
00054:     {
00055:         public string QuestId { get; set; } = string.Empty;
00056:         public List<string> Requires { get; set; } = new List<string>();
00057:         public int? RequiresChoiceIndex { get; set; }
00058:         public int? RequiresMinMoral { get; set; }
00059:         public int? RequiresMaxMoral { get; set; }
00060:         public int? RequiresMinEmpathy { get; set; }
00061:         public string RequiresFlag { get; set; } = string.Empty;
00062:         public string Branch { get; set; } = string.Empty;
00063:     }
00064:
00065:     /// <summary>
00066:     /// Echo quest: fires when a specific earlier quest was resolved with a
00067:     /// specific choice, after a minimum number of days have passed.
00068:     /// </summary>
00069:     public sealed class MoralEchoQuestDefinition
00070:     {
00071:         public string QuestId { get; set; } = string.Empty;
00072:         public string TriggeredBy { get; set; } = string.Empty;
00073:         public int TriggeredByChoice { get; set; }
00074:         public int MinDaysAfter { get; set; }
00075:         public string Branch { get; set; } = string.Empty;
00076:     }
00077: }
```

## `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainCatalogLoader.cs` — 188 lines; 7,346 bytes; SHA-256 `42aa7616827338bfb139d21c5575f77af886a3911348e319f0da13a9cafe7a2f`
Declaration index:
- 00010: public sealed class MoralChoiceChainCatalogContainer
- 00022: public sealed class MoralBranchRecord
- 00035: public sealed class MoralMergeRulesRecord
- 00044: public sealed class MoralLockoutRulesRecord
- 00053: public sealed class MoralQuestGateRecord
- 00066: public sealed class MoralEchoQuestsContainer
- 00073: public sealed class MoralEchoQuestRecord
- 00087: public static class MoralChoiceChainCatalogLoader
- 00091: public static MoralChoiceChainData Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
- 00111: private static MoralChoiceChainData Map(MoralChoiceChainCatalogContainer c)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core.MoralChoice
00006: {
00007:     // ── JSON wire records (snake_case, deserialized by IJsonSerializer) ──
00008:
00009:     [Serializable]
00010:     public sealed class MoralChoiceChainCatalogContainer
00011:     {
00012:         public int schema_version = 1;
00013:         public string description = string.Empty;
00014:         public List<MoralBranchRecord> branches = new List<MoralBranchRecord>();
00015:         public MoralMergeRulesRecord merge_rules = new MoralMergeRulesRecord();
00016:         public MoralLockoutRulesRecord lockout_rules = new MoralLockoutRulesRecord();
00017:         public List<MoralQuestGateRecord> quest_gates = new List<MoralQuestGateRecord>();
00018:         public MoralEchoQuestsContainer echo_quests = new MoralEchoQuestsContainer();
00019:     }
00020:
00021:     [Serializable]
00022:     public sealed class MoralBranchRecord
00023:     {
00024:         public string id = string.Empty;
00025:         public string display_name = string.Empty;
00026:         public string description = string.Empty;
00027:         public int lock_threshold;
00028:         public List<string> locks_out = new List<string>();
00029:         public List<string> merge_allowed = new List<string>();
00030:         public List<string> entry_quests = new List<string>();
00031:         public string locked_flag = string.Empty;
00032:     }
00033:
00034:     [Serializable]
00035:     public sealed class MoralMergeRulesRecord
00036:     {
00037:         public string description = string.Empty;
00038:         public string merge_quest_prefix = string.Empty;
00039:         public int merge_quests_require_min_progress;
00040:         public bool merge_never_unlocks_exclusive;
00041:     }
00042:
00043:     [Serializable]
00044:     public sealed class MoralLockoutRulesRecord
00045:     {
00046:         public string description = string.Empty;
00047:         public bool lockout_is_permanent;
00048:         public bool lockout_fires_journal_entry;
00049:         public string lockout_journal_template = string.Empty;
00050:     }
00051:
00052:     [Serializable]
00053:     public sealed class MoralQuestGateRecord
00054:     {
00055:         public string quest_id = string.Empty;
00056:         public List<string> requires = new List<string>();
00057:         public int? requires_choice_index;
00058:         public int? requires_min_moral;
00059:         public int? requires_max_moral;
00060:         public int? requires_min_empathy;
00061:         public string requires_flag = string.Empty;
00062:         public string branch = string.Empty;
00063:     }
00064:
00065:     [Serializable]
00066:     public sealed class MoralEchoQuestsContainer
00067:     {
00068:         public string description = string.Empty;
00069:         public List<MoralEchoQuestRecord> quests = new List<MoralEchoQuestRecord>();
00070:     }
00071:
00072:     [Serializable]
00073:     public sealed class MoralEchoQuestRecord
00074:     {
00075:         public string quest_id = string.Empty;
00076:         public string triggered_by = string.Empty;
00077:         public int triggered_by_choice;
00078:         public int min_days_after;
00079:         public string branch = string.Empty;
00080:     }
00081:
00082:     /// <summary>
00083:     /// Loads moral_choice_chains.json — the branching architecture (4 branches,
00084:     /// merge rules, lockout rules, quest gates, echo quest triggers).
00085:     /// Engine-agnostic: IFileIO + IJsonSerializer ports.
00086:     /// </summary>
00087:     public static class MoralChoiceChainCatalogLoader
00088:     {
00089:         public const string DefaultFileName = "moral_choice_chains.json";
00090:
00091:         public static MoralChoiceChainData Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
00092:         {
00093:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00094:                 return new MoralChoiceChainData();
00095:
00096:             string path = fileIO.Combine(dataDir, DefaultFileName);
00097:             if (!fileIO.FileExists(path))
00098:                 return new MoralChoiceChainData();
00099:
00100:             string raw = fileIO.ReadAllText(path);
00101:             if (string.IsNullOrWhiteSpace(raw))
00102:                 return new MoralChoiceChainData();
00103:
00104:             var container = json.Deserialize<MoralChoiceChainCatalogContainer>(raw);
00105:             if (container == null)
00106:                 return new MoralChoiceChainData();
00107:
00108:             return Map(container);
00109:         }
00110:
00111:         private static MoralChoiceChainData Map(MoralChoiceChainCatalogContainer c)
00112:         {
00113:             var data = new MoralChoiceChainData
00114:             {
00115:                 MergeRules = new MoralMergeRules
00116:                 {
00117:                     Description = c.merge_rules?.description ?? string.Empty,
00118:                     MergeQuestPrefix = c.merge_rules?.merge_quest_prefix ?? string.Empty,
00119:                     MergeQuestsRequireMinProgress = c.merge_rules?.merge_quests_require_min_progress ?? 0,
00120:                     MergeNeverUnlocksExclusive = c.merge_rules?.merge_never_unlocks_exclusive ?? false
00121:                 },
00122:                 LockoutRules = new MoralLockoutRules
00123:                 {
00124:                     Description = c.lockout_rules?.description ?? string.Empty,
00125:                     LockoutIsPermanent = c.lockout_rules?.lockout_is_permanent ?? false,
00126:                     LockoutFiresJournalEntry = c.lockout_rules?.lockout_fires_journal_entry ?? false,
00127:                     LockoutJournalTemplate = c.lockout_rules?.lockout_journal_template ?? string.Empty
00128:                 }
00129:             };
00130:
00131:             if (c.branches != null)
00132:             {
00133:                 foreach (var b in c.branches)
00134:                 {
00135:                     if (b == null) continue;
00136:                     data.Branches.Add(new MoralBranchDefinition
00137:                     {
00138:                         Id = b.id,
00139:                         DisplayName = b.display_name,
00140:                         Description = b.description,
00141:                         LockThreshold = b.lock_threshold,
00142:                         LocksOut = b.locks_out ?? new List<string>(),
00143:                         MergeAllowed = b.merge_allowed ?? new List<string>(),
00144:                         EntryQuests = b.entry_quests ?? new List<string>(),
00145:                         LockedFlag = b.locked_flag ?? string.Empty
00146:                     });
00147:                 }
00148:             }
00149:
00150:             if (c.quest_gates != null)
00151:             {
00152:                 foreach (var g in c.quest_gates)
00153:                 {
00154:                     if (g == null) continue;
00155:                     data.QuestGates.Add(new MoralQuestGate
00156:                     {
00157:                         QuestId = g.quest_id,
00158:                         Requires = g.requires ?? new List<string>(),
00159:                         RequiresChoiceIndex = g.requires_choice_index,
00160:                         RequiresMinMoral = g.requires_min_moral,
00161:                         RequiresMaxMoral = g.requires_max_moral,
00162:                         RequiresMinEmpathy = g.requires_min_empathy,
00163:                         RequiresFlag = g.requires_flag ?? string.Empty,
00164:                         Branch = g.branch ?? string.Empty
00165:                     });
00166:                 }
00167:             }
00168:
00169:             if (c.echo_quests?.quests != null)
00170:             {
00171:                 foreach (var e in c.echo_quests.quests)
00172:                 {
00173:                     if (e == null) continue;
00174:                     data.EchoQuests.Add(new MoralEchoQuestDefinition
00175:                     {
00176:                         QuestId = e.quest_id,
00177:                         TriggeredBy = e.triggered_by,
00178:                         TriggeredByChoice = e.triggered_by_choice,
00179:                         MinDaysAfter = e.min_days_after,
00180:                         Branch = e.branch ?? string.Empty
00181:                     });
00182:                 }
00183:             }
00184:
00185:             return data;
00186:         }
00187:     }
00188: }
```

## `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` — 687 lines; 30,717 bytes; SHA-256 `4cb9adafbbbdfc153d1c80ac2595f1e6c870a6669e2c9e85844a6a416133c9c1`
Declaration index:
- 00008: public enum MoralPathBand
- 00019: public enum MoralEndingKind
- 00037: public sealed class MoralChoiceQuestDefinition
- 00061: public sealed class MoralChoiceOption
- 00082: public sealed class MoralChoiceSystem
- 00149: public void InitializeChainData(MoralChoiceChainData chainData)
- 00171: public static bool IsCanonicalQuestId(string questId) =>
- 00175: public static bool IsAvailableOnDay(MoralChoiceQuestDefinition quest, int day) =>
- 00178: public bool IsResolved(string questId) => TryGetResolution(questId, out _);
- 00180: public bool TryGetResolution(string questId, out MoralChoiceResolution? resolution)
- 00188: public void RegisterQuest(MoralChoiceQuestDefinition def)
- 00194: public void RegisterQuests(IEnumerable<MoralChoiceQuestDefinition> defs)
- 00204: public MoralChoiceQuestDefinition? GetQuest(string id) =>
- 00214: public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyOffers(int day, int maxOffers = 1)
- 00258: public bool TryResolve(string questId, int choiceIndex, string locationId, int day, out MoralResolveResult result)
- 00303: public bool IsBranchLocked(string branchId) =>
- 00307: public int GetBranchProgress(string branchId) =>
- 00311: public string GetQuestBranch(string questId) =>
- 00319: public bool IsChainQuestAccessible(string questId, int day)
- 00339: public bool EvaluateGate(MoralQuestGate gate)
- 00358: public void SetFlag(string flagId)
- 00367: public bool HasFlag(string flagId) =>
- 00378: public List<MoralEchoQuestDefinition> FindAvailableEchoQuests(int currentDay)
- 00398: public void MarkEchoQuestFired(string echoQuestId)
- 00413: public MoralChoiceResolution Resolve(MoralChoiceQuestDefinition quest, int choiceIndex, string locationId, int day)
- 00481: private void TrackBranchProgress(string questId)
- 00524: public void Reconcile(int day)
- 00546: private void FireBandEvents(MoralPathBand band)
- 00563: public MoralEndingKind SelectEnding() =>
- 00571: public static MoralEndingKind SelectEnding(int moralScore, int empathyPoints, int questsResolved)
- 00597: public static MoralPathBand BandForScore(int score)
- 00609: public MoralChoiceState CaptureState() => Clone(_state);
- 00611: public void RestoreState(MoralChoiceState state)
- 00636: private void FireThresholdEvent(string eventId)
- 00643: private static string MarkFor(int moralDelta) =>
- 00647: private static MoralChoiceState Clone(MoralChoiceState source)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005:
00006: namespace Ashfall.Core.MoralChoice
00007: {
00008:     public enum MoralPathBand
00009:     {
00010:         VeryEvil,
00011:         Evil,
00012:         SlightlyEvil,
00013:         Neutral,
00014:         SlightlyPositive,
00015:         Positive,
00016:         VeryPositive
00017:     }
00018:
00019:     public enum MoralEndingKind
00020:     {
00021:         Warlord,
00022:         SurvivorKing,
00023:         NeutralSurvivor,
00024:         BalancedSurvivor,
00025:         CommunityBuilder,
00026:         Savior,
00027:         SaintOfWasteland,
00028:         Storykeeper
00029:     }
00030:
00031:     /// <summary>
00032:     /// In-code quest shape. Phase 2 loads these from
00033:     /// Assets/StreamingAssets/Data/moral_choice_quests.json; ids must use the
00034:     /// canonical quest_moral_ prefix (the design doc drafts them as
00035:     /// qst_moral_* — that spelling is rejected on purpose).
00036:     /// </summary>
00037:     public sealed class MoralChoiceQuestDefinition
00038:     {
00039:         public string Id { get; set; } = string.Empty;
00040:         public string DisplayName { get; set; } = string.Empty;
00041:
00042:         /// <summary>share | listen | comfort | dead | trust</summary>
00043:         public string Category { get; set; } = string.Empty;
00044:
00045:         public string Trigger { get; set; } = string.Empty;
00046:
00047:         /// <summary>Encounter prose shown when the quest is discovered.</summary>
00048:         public string Discovery { get; set; } = string.Empty;
00049:
00050:         public string LocationId { get; set; } = string.Empty;
00051:
00052:         /// <summary>First day the quest may be offered; 0 = always.</summary>
00053:         public int MinDay { get; set; }
00054:
00055:         /// <summary>Last day the quest may be offered; 0 or negative = unbounded.</summary>
00056:         public int MaxDay { get; set; }
00057:
00058:         public List<MoralChoiceOption> Choices { get; set; } = new List<MoralChoiceOption>();
00059:     }
00060:
00061:     public sealed class MoralChoiceOption
00062:     {
00063:         /// <summary>UI text for the choice, e.g. "Give all your food".</summary>
00064:         public string Label { get; set; } = string.Empty;
00065:
00066:         public int MoralDelta { get; set; }
00067:         public int EmpathyDelta { get; set; }
00068:         /// <summary>Optional canonical historical flag written after this choice commits.</summary>
00069:         public string SetFlag { get; set; } = string.Empty;
00070:         public string OutcomeText { get; set; } = string.Empty;
00071:         public string Epitaph { get; set; } = string.Empty;
00072:     }
00073:
00074:     /// <summary>
00075:     /// Engine-agnostic moral choice ledger: invisible moral + empathy
00076:     /// accumulators, band computation, overnight reconciliation with
00077:     /// one-time threshold events, ending selection, branch tracking,
00078:     /// quest gating, and echo quest availability. The score is never
00079:     /// surfaced to the player — the world is the UI (host layers read
00080:     /// CurrentBand / events, never the raw number).
00081:     /// </summary>
00082:     public sealed class MoralChoiceSystem
00083:     {
00084:         public const string SystemId = "moral_choice";
00085:         public const string QuestIdPrefix = "quest_moral_";
00086:
00087:         public const int MinScore = -200;
00088:         public const int MaxScore = 200;
00089:
00090:         public const int ListenerEmpathyThreshold = 15;
00091:         public const int ConfidantEmpathyThreshold = 30;
00092:         public const int StorykeeperEmpathyThreshold = 45;
00093:         public const int StorykeeperQuestThreshold = 25;
00094:
00095:         /// <summary>Below this many resolved quests no ending band locks; mild endings fire instead.</summary>
00096:         public const int EndingLockMinQuests = 20;
00097:
00098:         public const string EventLegendPositive = "moral_event_legend_positive";
00099:         public const string EventLegendNegative = "moral_event_legend_negative";
00100:         public const string EventBountyIssued = "moral_event_bounty_issued";
00101:         public const string EventContractTaken = "moral_event_contract_taken";
00102:         public const string EventContractRaised = "moral_event_contract_raised";
00103:         public const string EventPatrolDefense = "moral_event_patrol_defense";
00104:
00105:         /// <summary>Pending-overflow bits settled at the next Reconcile (bit 1 = positive, bit 2 = negative).</summary>
00106:         public const int LegendPositiveFlag = 1;
00107:         public const int LegendNegativeFlag = 2;
00108:
00109:         private readonly ISeededRng _rng;
00110:         private readonly ILog _log;
00111:         private readonly Flags.IFlagLedger? _flags;
00112:         private MoralChoiceState _state = new MoralChoiceState();
00113:
00114:         /// <summary>Branch architecture from moral_choice_chains.json; null until InitializeChainData.</summary>
00115:         private MoralChoiceChainData? _chainData;
00116:         private readonly Dictionary<string, MoralChoiceQuestDefinition> _catalog = new Dictionary<string, MoralChoiceQuestDefinition>(StringComparer.Ordinal);
00117:         private Dictionary<string, string> _questToBranch = new Dictionary<string, string>();
00118:         private HashSet<string> _entryQuestSet = new HashSet<string>();
00119:
00120:         public event Action<MoralChoiceResolution>? OnQuestResolved;
00121:         public event Action<string>? OnThresholdEventFired;
00122:         public event Action<string>? OnBranchLocked;
00123:
00124:         public MoralChoiceSystem(ISeededRng rng, ILog? log = null, Flags.IFlagLedger? flags = null)
00125:         {
00126:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00127:             _log = log ?? NullLog.Instance;
00128:             _flags = flags;
00129:         }
00130:
00131:         public MoralChoiceState State => _state;
00132:         public int MoralScore => _state.moralScore;
00133:         public int EmpathyPoints => _state.empathyPoints;
00134:         public int QuestsResolved => _state.resolutions.Count;
00135:         public MoralPathBand CurrentBand => BandForScore(_state.moralScore);
00136:         public IReadOnlyList<MoralChoiceResolution> Resolutions => _state.resolutions;
00137:
00138:         public bool IsListener => _state.empathyPoints >= ListenerEmpathyThreshold;
00139:         public bool IsConfidant => _state.empathyPoints >= ConfidantEmpathyThreshold;
00140:
00141:         /// <summary>Chain data reference; null if not yet initialized.</summary>
00142:         public MoralChoiceChainData? ChainData => _chainData;
00143:
00144:         /// <summary>
00145:         /// Load the branching architecture (moral_choice_chains.json). Builds
00146:         /// internal lookup maps for branch ownership and entry-quest tracking.
00147:         /// Safe to call once at startup; subsequent calls are no-ops.
00148:         /// </summary>
00149:         public void InitializeChainData(MoralChoiceChainData chainData)
00150:         {
00151:             if (chainData == null || _chainData != null) return;
00152:             _chainData = chainData;
00153:
00154:             foreach (var branch in chainData.Branches)
00155:             {
00156:                 foreach (var entryQuest in branch.EntryQuests)
00157:                 {
00158:                     _questToBranch[entryQuest] = branch.Id;
00159:                     _entryQuestSet.Add(entryQuest);
00160:                 }
00161:             }
00162:             foreach (var gate in chainData.QuestGates)
00163:             {
00164:                 if (!string.IsNullOrEmpty(gate.Branch) && !string.IsNullOrEmpty(gate.QuestId))
00165:                 {
00166:                     _questToBranch[gate.QuestId] = gate.Branch;
00167:                 }
00168:             }
00169:         }
00170:
00171:         public static bool IsCanonicalQuestId(string questId) =>
00172:             questId.StartsWith(QuestIdPrefix, StringComparison.Ordinal);
00173:
00174:         /// <summary>MaxDay &lt;= 0 means unbounded; a malformed window (max &lt; min) is never available.</summary>
00175:         public static bool IsAvailableOnDay(MoralChoiceQuestDefinition quest, int day) =>
00176:             day >= quest.MinDay && (quest.MaxDay <= 0 || (day <= quest.MaxDay && quest.MaxDay >= quest.MinDay));
00177:
00178:         public bool IsResolved(string questId) => TryGetResolution(questId, out _);
00179:
00180:         public bool TryGetResolution(string questId, out MoralChoiceResolution? resolution)
00181:         {
00182:             resolution = _state.resolutions.FirstOrDefault(r => string.Equals(r.questId, questId, StringComparison.Ordinal));
00183:             return resolution != null;
00184:         }
00185:
00186:         // ── Catalog registration ───────────────────────────────────────
00187:
00188:         public void RegisterQuest(MoralChoiceQuestDefinition def)
00189:         {
00190:             if (def == null || string.IsNullOrEmpty(def.Id)) return;
00191:             _catalog[def.Id] = def;
00192:         }
00193:
00194:         public void RegisterQuests(IEnumerable<MoralChoiceQuestDefinition> defs)
00195:         {
00196:             if (defs == null) return;
00197:             foreach (var def in defs)
00198:                 RegisterQuest(def);
00199:         }
00200:
00201:         public IReadOnlyDictionary<string, MoralChoiceQuestDefinition> Catalog => _catalog;
00202:         public int CatalogCount => _catalog.Count;
00203:
00204:         public MoralChoiceQuestDefinition? GetQuest(string id) =>
00205:             !string.IsNullOrEmpty(id) && _catalog.TryGetValue(id, out var def) ? def : null;
00206:
00207:         // ── Seeded daily offers ────────────────────────────────────────
00208:
00209:         /// <summary>
00210:         /// Returns deterministic daily moral choice offers for the given day.
00211:         /// Uses seed formula: unchecked((ulong)_rng.Seed * 31337UL + (ulong)day * 1009UL + 0x5EEDUL).
00212:         /// Only returns unresolved quests whose day window is active and whose chain prerequisites/gates are met.
00213:         /// </summary>
00214:         public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyOffers(int day, int maxOffers = 1)
00215:         {
00216:             if (maxOffers <= 0 || _catalog.Count == 0) return Array.Empty<MoralChoiceQuestDefinition>();
00217:
00218:             var candidates = new List<MoralChoiceQuestDefinition>();
00219:             foreach (var kv in _catalog)
00220:             {
00221:                 var q = kv.Value;
00222:                 if (!IsResolved(q.Id) && IsAvailableOnDay(q, day) && IsChainQuestAccessible(q.Id, day))
00223:                 {
00224:                     candidates.Add(q);
00225:                 }
00226:             }
00227:
00228:             if (candidates.Count == 0) return Array.Empty<MoralChoiceQuestDefinition>();
00229:
00230:             // Sort deterministically by Id
00231:             candidates.Sort((a, b) => string.Compare(a.Id, b.Id, StringComparison.Ordinal));
00232:
00233:             if (candidates.Count <= maxOffers) return candidates;
00234:
00235:             ulong dailySeedRaw = unchecked((ulong)_rng.Seed * 31337UL + (ulong)day * 1009UL + 0x5EEDUL);
00236:             int dailySeed = unchecked((int)(dailySeedRaw ^ (dailySeedRaw >> 32)));
00237:             var dailyRng = new SeededRng(dailySeed);
00238:
00239:             var pool = new List<MoralChoiceQuestDefinition>(candidates);
00240:             var selected = new List<MoralChoiceQuestDefinition>(maxOffers);
00241:             for (int i = 0; i < maxOffers && pool.Count > 0; i++)
00242:             {
00243:                 int idx = dailyRng.Next(0, pool.Count);
00244:                 selected.Add(pool[idx]);
00245:                 pool.RemoveAt(idx);
00246:             }
00247:
00248:             return selected;
00249:         }
00250:
00251:         // ── Structured resolution ──────────────────────────────────────
00252:
00253:         /// <summary>
00254:         /// Attempts to resolve a registered moral choice quest.
00255:         /// Enforces strict single-resolution, catalog validity, day window, and choice index bounds
00256:         /// returning structured status codes without throwing on invalid player/client input.
00257:         /// </summary>
00258:         public bool TryResolve(string questId, int choiceIndex, string locationId, int day, out MoralResolveResult result)
00259:         {
00260:             if (string.IsNullOrEmpty(questId) || !_catalog.TryGetValue(questId, out var def))
00261:             {
00262:                 result = MoralResolveResult.Failed(MoralResolveResultCode.UnknownChoice,
00263:                     $"Quest '{questId}' is not registered in moral choice catalog.");
00264:                 return false;
00265:             }
00266:
00267:             if (TryGetResolution(questId, out var existing))
00268:             {
00269:                 result = MoralResolveResult.Failed(MoralResolveResultCode.AlreadyResolved,
00270:                     $"Quest '{questId}' was already resolved on day {existing!.resolvedDay}.", existing);
00271:                 return false;
00272:             }
00273:
00274:             if (!IsAvailableOnDay(def, day))
00275:             {
00276:                 result = MoralResolveResult.Failed(MoralResolveResultCode.ChoiceNotAvailable,
00277:                     $"Quest '{questId}' is not available on day {day} (window: {def.MinDay}..{def.MaxDay}).");
00278:                 return false;
00279:             }
00280:
00281:             if (!IsChainQuestAccessible(questId, day))
00282:             {
00283:                 result = MoralResolveResult.Failed(MoralResolveResultCode.RequirementMissing,
00284:                     $"Quest '{questId}' chain gate or branch requirements are not met.");
00285:                 return false;
00286:             }
00287:
00288:             if (choiceIndex < 0 || choiceIndex >= def.Choices.Count)
00289:             {
00290:                 result = MoralResolveResult.Failed(MoralResolveResultCode.UnknownOption,
00291:                     $"Choice index {choiceIndex} is out of bounds for quest '{questId}' (0..{def.Choices.Count - 1}).");
00292:                 return false;
00293:             }
00294:
00295:             var resolution = Resolve(def, choiceIndex, locationId, day);
00296:             result = MoralResolveResult.Succeeded(resolution);
00297:             return true;
00298:         }
00299:
00300:         // ── Branch tracking ────────────────────────────────────────────
00301:
00302:         /// <summary>Whether a branch has been permanently locked by the lockout mechanic.</summary>
00303:         public bool IsBranchLocked(string branchId) =>
00304:             _state.lockedBranches.Contains(branchId);
00305:
00306:         /// <summary>How many entry quests the player has resolved for a branch.</summary>
00307:         public int GetBranchProgress(string branchId) =>
00308:             _state.branchProgress.TryGetValue(branchId, out int v) ? v : 0;
00309:
00310:         /// <summary>Which branch owns a quest (by chain data); empty string if not a chain quest.</summary>
00311:         public string GetQuestBranch(string questId) =>
00312:             _questToBranch.TryGetValue(questId, out var b) ? b : string.Empty;
00313:
00314:         /// <summary>
00315:         /// Whether a chain quest is accessible: branch not locked, gate
00316:         /// prerequisites met, day window valid, and not already resolved.
00317:         /// Non-chain quests (base/expansion) only check day + resolved.
00318:         /// </summary>
00319:         public bool IsChainQuestAccessible(string questId, int day)
00320:         {
00321:             if (IsResolved(questId)) return false;
00322:
00323:             if (_questToBranch.TryGetValue(questId, out var branchId))
00324:             {
00325:                 if (IsBranchLocked(branchId)) return false;
00326:             }
00327:
00328:             var gate = _chainData?.QuestGates.FirstOrDefault(
00329:                 g => string.Equals(g.QuestId, questId, StringComparison.Ordinal));
00330:             if (gate != null && !EvaluateGate(gate)) return false;
00331:
00332:             return true;
00333:         }
00334:
00335:         /// <summary>
00336:         /// Evaluate a quest gate's prerequisites: prior quests resolved,
00337:         /// moral/empathy thresholds, and flag requirements.
00338:         /// </summary>
00339:         public bool EvaluateGate(MoralQuestGate gate)
00340:         {
00341:             if (gate == null) return true;
00342:
00343:             foreach (var req in gate.Requires)
00344:             {
00345:                 if (!IsResolved(req)) return false;
00346:             }
00347:
00348:             if (gate.RequiresMinMoral.HasValue && _state.moralScore < gate.RequiresMinMoral.Value) return false;
00349:             if (gate.RequiresMaxMoral.HasValue && _state.moralScore > gate.RequiresMaxMoral.Value) return false;
00350:             if (gate.RequiresMinEmpathy.HasValue && _state.empathyPoints < gate.RequiresMinEmpathy.Value) return false;
00351:
00352:             if (!string.IsNullOrEmpty(gate.RequiresFlag) && !_state.activeFlags.Contains(gate.RequiresFlag)) return false;
00353:
00354:             return true;
00355:         }
00356:
00357:         /// <summary>Set a moral flag (idempotent).</summary>
00358:         public void SetFlag(string flagId)
00359:         {
00360:             if (string.IsNullOrEmpty(flagId)) return;
00361:             if (!_state.activeFlags.Contains(flagId))
00362:                 _state.activeFlags.Add(flagId);
00363:             _flags?.Set(flagId, "moral_choice");
00364:         }
00365:
00366:         /// <summary>Whether a moral flag is currently set.</summary>
00367:         public bool HasFlag(string flagId) =>
00368:             !string.IsNullOrEmpty(flagId) && (_flags != null ? (_flags.IsSet(flagId) || _state.activeFlags.Contains(flagId)) : _state.activeFlags.Contains(flagId));
00369:
00370:         // ── Echo quests ────────────────────────────────────────────────
00371:
00372:         /// <summary>
00373:         /// Find echo quests that should fire given the current state and day.
00374:         /// An echo quest fires when: its trigger quest was resolved with the
00375:         /// matching choice, enough days have passed, it hasn't fired yet, and
00376:         /// its branch (if any) is not locked.
00377:         /// </summary>
00378:         public List<MoralEchoQuestDefinition> FindAvailableEchoQuests(int currentDay)
00379:         {
00380:             if (_chainData == null) return new List<MoralEchoQuestDefinition>();
00381:
00382:             var result = new List<MoralEchoQuestDefinition>();
00383:             foreach (var echo in _chainData.EchoQuests)
00384:             {
00385:                 if (_state.firedEchoQuests.Contains(echo.QuestId)) continue;
00386:                 if (!TryGetResolution(echo.TriggeredBy, out var trigger)) continue;
00387:                 if (trigger!.choiceIndex != echo.TriggeredByChoice) continue;
00388:                 if (currentDay < trigger.resolvedDay + echo.MinDaysAfter) continue;
00389:
00390:                 if (!string.IsNullOrEmpty(echo.Branch) && IsBranchLocked(echo.Branch)) continue;
00391:
00392:                 result.Add(echo);
00393:             }
00394:             return result;
00395:         }
00396:
00397:         /// <summary>Mark an echo quest as fired (called by the host when the echo is presented).</summary>
00398:         public void MarkEchoQuestFired(string echoQuestId)
00399:         {
00400:             if (string.IsNullOrEmpty(echoQuestId)) return;
00401:             if (!_state.firedEchoQuests.Contains(echoQuestId))
00402:                 _state.firedEchoQuests.Add(echoQuestId);
00403:         }
00404:
00405:         // ── Quest resolution ───────────────────────────────────────────
00406:
00407:         /// <summary>
00408:         /// Resolve a quest choice. One resolution per quest per save: repeat
00409:         /// calls return the stored resolution without re-applying deltas or
00410:         /// re-rolling. Band-crossing consequences never land here — they
00411:         /// settle overnight in Reconcile.
00412:         /// </summary>
00413:         public MoralChoiceResolution Resolve(MoralChoiceQuestDefinition quest, int choiceIndex, string locationId, int day)
00414:         {
00415:             if (quest == null) throw new ArgumentNullException(nameof(quest));
00416:             if (!_catalog.ContainsKey(quest.Id)) _catalog[quest.Id] = quest;
00417:             if (!IsCanonicalQuestId(quest.Id))
00418:             {
00419:                 throw new ArgumentException(
00420:                     $"Moral quest id '{quest.Id}' must use the canonical '{QuestIdPrefix}' prefix " +
00421:                     "(the design doc drafts ids as 'qst_moral_*'; register them as 'quest_moral_*').",
00422:                     nameof(quest));
00423:             }
00424:             if (choiceIndex < 0 || choiceIndex >= quest.Choices.Count)
00425:             {
00426:                 throw new ArgumentOutOfRangeException(nameof(choiceIndex),
00427:                     $"Choice index {choiceIndex} is outside 0..{quest.Choices.Count - 1} for '{quest.Id}'.");
00428:             }
00429:             if (day < 0) throw new ArgumentOutOfRangeException(nameof(day));
00430:
00431:             if (TryGetResolution(quest.Id, out var existing))
00432:             {
00433:                 _log.Warn($"Moral quest '{quest.Id}' already resolved on day {existing!.resolvedDay}; replaying stored outcome.");
00434:                 return existing;
00435:             }
00436:
00437:             var choice = quest.Choices[choiceIndex];
00438:             int unclamped = _state.moralScore + choice.MoralDelta;
00439:             int clamped = Math.Clamp(unclamped, MinScore, MaxScore);
00440:             _state.moralScore = clamped;
00441:             _state.empathyPoints += choice.EmpathyDelta;
00442:
00443:             var resolution = new MoralChoiceResolution
00444:             {
00445:                 questId = quest.Id,
00446:                 locationId = locationId ?? string.Empty,
00447:                 resolvedDay = day,
00448:                 choiceIndex = choiceIndex,
00449:                 moralDelta = choice.MoralDelta,
00450:                 empathyDelta = choice.EmpathyDelta,
00451:                 impactMark = MarkFor(choice.MoralDelta),
00452:                 outcomeRoll = _rng.Next(0, 100),
00453:                 propagatesOnDay = day + 1 + _rng.Next(0, 3),
00454:                 epitaph = choice.Epitaph
00455:             };
00456:             _state.resolutions.Add(resolution);
00457:
00458:             // Historical moral memory is part of the committed resolution.
00459:             // SetFlag is idempotent, and the existing save state remains the
00460:             // sole persistence authority for the resulting active flag set.
00461:             if (!string.IsNullOrEmpty(choice.SetFlag))
00462:                 SetFlag(choice.SetFlag);
00463:
00464:             OnQuestResolved?.Invoke(resolution);
00465:
00466:             // Overflow never lands mid-scene: flag it, settle it overnight.
00467:             if (unclamped > MaxScore) _state.pendingLegendFlags |= LegendPositiveFlag;
00468:             else if (unclamped < MinScore) _state.pendingLegendFlags |= LegendNegativeFlag;
00469:
00470:             // Track branch progress for entry quests and check lockout.
00471:             TrackBranchProgress(quest.Id);
00472:
00473:             return resolution;
00474:         }
00475:
00476:         /// <summary>
00477:         /// If the resolved quest is a branch entry quest, increment that
00478:         /// branch's progress. When the lock threshold is reached, lock out
00479:         /// the opposing branches and set the branch-locked flags.
00480:         /// </summary>
00481:         private void TrackBranchProgress(string questId)
00482:         {
00483:             if (!_entryQuestSet.Contains(questId)) return;
00484:             if (!_questToBranch.TryGetValue(questId, out var branchId)) return;
00485:             if (_chainData == null) return;
00486:
00487:             var branch = _chainData.Branches.FirstOrDefault(
00488:                 b => string.Equals(b.Id, branchId, StringComparison.Ordinal));
00489:             if (branch == null) return;
00490:
00491:             if (!_state.branchProgress.ContainsKey(branchId))
00492:                 _state.branchProgress[branchId] = 0;
00493:             _state.branchProgress[branchId]++;
00494:
00495:             if (_state.branchProgress[branchId] >= branch.LockThreshold)
00496:             {
00497:                 foreach (var lockedId in branch.LocksOut)
00498:                 {
00499:                     if (!_state.lockedBranches.Contains(lockedId))
00500:                     {
00501:                         _state.lockedBranches.Add(lockedId);
00502:
00503:                         var lockedBranch = _chainData.Branches.FirstOrDefault(
00504:                             b => string.Equals(b.Id, lockedId, StringComparison.Ordinal));
00505:                         if (lockedBranch != null && !string.IsNullOrEmpty(lockedBranch.LockedFlag))
00506:                         {
00507:                             SetFlag(lockedBranch.LockedFlag);
00508:                         }
00509:
00510:                         OnBranchLocked?.Invoke(lockedId);
00511:                     }
00512:                 }
00513:             }
00514:         }
00515:
00516:         /// <summary>
00517:         /// Overnight settlement: pending legend overflow, then band crossings
00518:         /// and their one-time faction events, so an act's consequences always
00519:         /// land overnight, never mid-scene. Every band crossed between the
00520:         /// last reconcile and now settles its event (dedup keeps each
00521:         /// one-time). Out-of-order days are ignored. A never-reconciled save
00522:         /// starts from the Neutral band.
00523:         /// </summary>
00524:         public void Reconcile(int day)
00525:         {
00526:             if (day < _state.lastReconciledDay) return;
00527:             _state.lastReconciledDay = day;
00528:
00529:             if ((_state.pendingLegendFlags & LegendPositiveFlag) != 0) FireThresholdEvent(EventLegendPositive);
00530:             if ((_state.pendingLegendFlags & LegendNegativeFlag) != 0) FireThresholdEvent(EventLegendNegative);
00531:             _state.pendingLegendFlags = 0;
00532:
00533:             int from = _state.bandAtLastReconcile < 0 ? (int)MoralPathBand.Neutral : _state.bandAtLastReconcile;
00534:             int to = (int)CurrentBand;
00535:             if (to == from) return;
00536:
00537:             int step = to > from ? 1 : -1;
00538:             for (int band = from + step; ; band += step)
00539:             {
00540:                 FireBandEvents((MoralPathBand)band);
00541:                 if (band == to) break;
00542:             }
00543:             _state.bandAtLastReconcile = to;
00544:         }
00545:
00546:         private void FireBandEvents(MoralPathBand band)
00547:         {
00548:             switch (band)
00549:             {
00550:                 case MoralPathBand.VeryEvil:
00551:                     FireThresholdEvent(EventBountyIssued);
00552:                     break;
00553:                 case MoralPathBand.Positive:
00554:                     FireThresholdEvent(EventContractTaken);
00555:                     break;
00556:                 case MoralPathBand.VeryPositive:
00557:                     FireThresholdEvent(EventContractRaised);
00558:                     FireThresholdEvent(EventPatrolDefense);
00559:                     break;
00560:             }
00561:         }
00562:
00563:         public MoralEndingKind SelectEnding() =>
00564:             SelectEnding(_state.moralScore, _state.empathyPoints, _state.resolutions.Count);
00565:
00566:         /// <summary>
00567:         /// Priority: Storykeeper threshold overrides band; below the quest
00568:         /// lock the mild endings fire (the band has not earned the right to
00569:         /// define the run yet); otherwise band decides.
00570:         /// </summary>
00571:         public static MoralEndingKind SelectEnding(int moralScore, int empathyPoints, int questsResolved)
00572:         {
00573:             if (empathyPoints >= StorykeeperEmpathyThreshold && questsResolved >= StorykeeperQuestThreshold)
00574:             {
00575:                 return MoralEndingKind.Storykeeper;
00576:             }
00577:
00578:             if (questsResolved < EndingLockMinQuests)
00579:             {
00580:                 return moralScore < 0 ? MoralEndingKind.NeutralSurvivor
00581:                     : moralScore == 0 ? MoralEndingKind.BalancedSurvivor
00582:                     : MoralEndingKind.CommunityBuilder;
00583:             }
00584:
00585:             return BandForScore(moralScore) switch
00586:             {
00587:                 MoralPathBand.VeryEvil => MoralEndingKind.Warlord,
00588:                 MoralPathBand.Evil => MoralEndingKind.SurvivorKing,
00589:                 MoralPathBand.SlightlyEvil => MoralEndingKind.NeutralSurvivor,
00590:                 MoralPathBand.Neutral => MoralEndingKind.BalancedSurvivor,
00591:                 MoralPathBand.SlightlyPositive => MoralEndingKind.CommunityBuilder,
00592:                 MoralPathBand.Positive => MoralEndingKind.Savior,
00593:                 _ => MoralEndingKind.SaintOfWasteland
00594:             };
00595:         }
00596:
00597:         public static MoralPathBand BandForScore(int score)
00598:         {
00599:             score = Math.Clamp(score, MinScore, MaxScore);
00600:             if (score <= -100) return MoralPathBand.VeryEvil;
00601:             if (score <= -50) return MoralPathBand.Evil;
00602:             if (score < 0) return MoralPathBand.SlightlyEvil;
00603:             if (score == 0) return MoralPathBand.Neutral;
00604:             if (score < 50) return MoralPathBand.SlightlyPositive;
00605:             if (score < 100) return MoralPathBand.Positive;
00606:             return MoralPathBand.VeryPositive;
00607:         }
00608:
00609:         public MoralChoiceState CaptureState() => Clone(_state);
00610:
00611:         public void RestoreState(MoralChoiceState state)
00612:         {
00613:             if (state == null) throw new ArgumentNullException(nameof(state));
00614:             if (!string.Equals(state.systemId, SystemId, StringComparison.Ordinal))
00615:             {
00616:                 throw new ArgumentException(
00617:                     $"State belongs to system '{state.systemId}', expected '{SystemId}'.", nameof(state));
00618:             }
00619:             if (state.schemaVersion > 1)
00620:             {
00621:                 throw new NotSupportedException(
00622:                     $"Future moral choice save schema {state.schemaVersion}; supported schema is 1.");
00623:             }
00624:             if (state.schemaVersion < 1)
00625:             {
00626:                 throw new ArgumentException("Moral choice save is missing a valid schemaVersion.", nameof(state));
00627:             }
00628:             _state = Clone(state);
00629:             if (_flags != null && _state.activeFlags != null)
00630:             {
00631:                 foreach (var f in _state.activeFlags)
00632:                     _flags.Set(f, "moral_choice");
00633:             }
00634:         }
00635:
00636:         private void FireThresholdEvent(string eventId)
00637:         {
00638:             if (_state.firedThresholdEvents.Contains(eventId)) return;
00639:             _state.firedThresholdEvents.Add(eventId);
00640:             OnThresholdEventFired?.Invoke(eventId);
00641:         }
00642:
00643:         private static string MarkFor(int moralDelta) =>
00644:             moralDelta > 0 ? "up" : moralDelta < 0 ? "down" : "flat";
00645:
00646:         /// <summary>Deep copy so captured/restored states never alias the live ledger.</summary>
00647:         private static MoralChoiceState Clone(MoralChoiceState source)
00648:         {
00649:             var copy = new MoralChoiceState
00650:             {
00651:                 systemId = source.systemId,
00652:                 schemaVersion = source.schemaVersion,
00653:                 moralScore = source.moralScore,
00654:                 empathyPoints = source.empathyPoints,
00655:                 lastReconciledDay = source.lastReconciledDay,
00656:                 bandAtLastReconcile = source.bandAtLastReconcile,
00657:                 pendingLegendFlags = source.pendingLegendFlags,
00658:                 firedThresholdEvents = new List<string>(source.firedThresholdEvents ?? new List<string>()),
00659:                 resolutions = new List<MoralChoiceResolution>(),
00660:                 branchProgress = new Dictionary<string, int>(source.branchProgress ?? new Dictionary<string, int>()),
00661:                 lockedBranches = new List<string>(source.lockedBranches ?? new List<string>()),
00662:                 firedEchoQuests = new List<string>(source.firedEchoQuests ?? new List<string>()),
00663:                 activeFlags = new List<string>(source.activeFlags ?? new List<string>())
00664:             };
00665:             if (source.resolutions != null)
00666:             {
00667:                 foreach (var r in source.resolutions)
00668:                 {
00669:                     copy.resolutions.Add(new MoralChoiceResolution
00670:                     {
00671:                         questId = r.questId,
00672:                         locationId = r.locationId,
00673:                         resolvedDay = r.resolvedDay,
00674:                         choiceIndex = r.choiceIndex,
00675:                         moralDelta = r.moralDelta,
00676:                         empathyDelta = r.empathyDelta,
00677:                         impactMark = r.impactMark,
00678:                         outcomeRoll = r.outcomeRoll,
00679:                         propagatesOnDay = r.propagatesOnDay,
00680:                         epitaph = r.epitaph
00681:                     });
00682:                 }
00683:             }
00684:             return copy;
00685:         }
00686:     }
00687: }
```

## `Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs` — 76 lines; 3,245 bytes; SHA-256 `67dec17ef0a93d7516a0ddf681661c46e8cd566d8a0602fabc77426430f027b5`
Declaration index:
- 00015: public sealed class MoralChoiceState
- 00053: public sealed class MoralChoiceResolution
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core.MoralChoice
00006: {
00007:     /// <summary>
00008:     /// Save DTO for the moral choice system ("The Weight of Survival",
00009:     /// docs/MORAL_CHOICE_SYSTEM.md). Journal resolutions, seeded outcome
00010:     /// rolls, gossip propagation schedules, branch progress, locked branches,
00011:     /// echo quest tracking, and moral flags all live here so a save replays
00012:     /// identically — there is no second file format.
00013:     /// </summary>
00014:     [Serializable]
00015:     public sealed class MoralChoiceState
00016:     {
00017:         public string systemId = MoralChoiceSystem.SystemId;
00018:         public int schemaVersion = 1;
00019:         public int moralScore;
00020:         public int empathyPoints;
00021:         public List<MoralChoiceResolution> resolutions = new List<MoralChoiceResolution>();
00022:         public int lastReconciledDay = -1;
00023:
00024:         /// <summary>Ordinal of MoralPathBand at the last reconcile; -1 = never reconciled.</summary>
00025:         public int bandAtLastReconcile = -1;
00026:
00027:         /// <summary>One-time threshold/legend events already fired, by id.</summary>
00028:         public List<string> firedThresholdEvents = new List<string>();
00029:
00030:         /// <summary>Overflow bits (LegendPositiveFlag/LegendNegativeFlag) awaiting overnight settlement.</summary>
00031:         public int pendingLegendFlags;
00032:
00033:         /// <summary>Entry-quest resolutions per branch (branch_id → count). Drives branch locking.</summary>
00034:         public Dictionary<string, int> branchProgress = new Dictionary<string, int>();
00035:
00036:         /// <summary>Branches permanently locked by the lockout mechanic.</summary>
00037:         public List<string> lockedBranches = new List<string>();
00038:
00039:         /// <summary>Echo quests already fired (by quest_id). Each fires at most once per save.</summary>
00040:         public List<string> firedEchoQuests = new List<string>();
00041:
00042:         /// <summary>Moral flags set during this save (flag_id list, treated as a set).</summary>
00043:         public List<string> activeFlags = new List<string>();
00044:     }
00045:
00046:     /// <summary>
00047:     /// One resolved moral quest: the journal line, the ledger entry, and the
00048:     /// seeded rolls drawn at resolution time. Treated as immutable after
00049:     /// creation; fields stay public/mutable to match the save-DTO convention
00050:     /// the JSON pipeline deserializes into.
00051:     /// </summary>
00052:     [Serializable]
00053:     public sealed class MoralChoiceResolution
00054:     {
00055:         public string questId = string.Empty;
00056:         public string locationId = string.Empty;
00057:         public int resolvedDay = -1;
00058:         public int choiceIndex = -1;
00059:
00060:         /// <summary>Raw design delta of the chosen option (pre-clamp); the journal arrow shows its sign.</summary>
00061:         public int moralDelta;
00062:
00063:         public int empathyDelta;
00064:
00065:         /// <summary>up | down | flat — never the number, only the direction.</summary>
00066:         public string impactMark = "flat";
00067:
00068:         /// <summary>0-99, rolled once at resolution and stored for deterministic outcome branches.</summary>
00069:         public int outcomeRoll = -1;
00070:
00071:         /// <summary>Gossip leaves the witnessing circle on this day (resolvedDay + 1..3).</summary>
00072:         public int propagatesOnDay = -1;
00073:
00074:         public string epitaph = string.Empty;
00075:     }
00076: }
```

## `Assets/Ashfall.Core/MoralChoice/MoralChoiceGossipSeed.cs` — 108 lines; 4,797 bytes; SHA-256 `475b38799cfac138d98e9d815eee92158bc39c57f92dc6e2a9feb825be400eb6`
Declaration index:
- 00021: public sealed class MoralGossipSeed
- 00051: private static float Clamp01(float v)
- 00054: private static float Clamp(float v, float lo, float hi)
- 00064: public static class MoralChoiceGossipSeed
- 00077: public static MoralGossipSeed? Build(
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // CORE-MECH W8 — Moral choice → rumor seed (pure, deterministic).
00004: //
00005: // Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
00006: //       (W8-GOSSIP-PROPAGATION, appendices AA.8 / AW.8 / BD.8).
00007: //
00008: // The information-flow owner already models propagation (hubs, propagation
00009: // speed, decay, interception). What never happened: a moral choice seeding one.
00010: // This file is the missing translation and nothing else — pure data in, a seed
00011: // description out. The caller (Main) hands the result to the canonical RumorSystem,
00012: // which stays the only rumor authority; no second rumor store, no parallel
00013: // propagation model, no new save section.
00014: // ============================================================================
00015:
00016: using System;
00017:
00018: namespace Ashfall.Core.MoralChoice
00019: {
00020:     /// <summary>What a resolved choice would say, if it became a rumor.</summary>
00021:     public sealed class MoralGossipSeed
00022:     {
00023:         public string SubjectId { get; }
00024:         public string OriginLocationId { get; }
00025:         public int OriginDay { get; }
00026:         public string Headline { get; }
00027:         public string Description { get; }
00028:
00029:         /// <summary>How likely the traveling version is to be true (0..1).</summary>
00030:         public float Truthfulness { get; }
00031:
00032:         /// <summary>Authored decay/speed profile the rumor owner may adopt.</summary>
00033:         public float DecayRate { get; }
00034:         public int PropagationSpeed { get; }
00035:
00036:         public MoralGossipSeed(
00037:             string subjectId, string originLocationId, int originDay,
00038:             string headline, string description,
00039:             float truthfulness, float decayRate, int propagationSpeed)
00040:         {
00041:             SubjectId = subjectId ?? string.Empty;
00042:             OriginLocationId = originLocationId ?? string.Empty;
00043:             OriginDay = Math.Max(1, originDay);
00044:             Headline = headline ?? string.Empty;
00045:             Description = description ?? string.Empty;
00046:             Truthfulness = Clamp01(truthfulness);
00047:             DecayRate = Clamp(decayRate, 0f, 1f);
00048:             PropagationSpeed = Math.Clamp(propagationSpeed, 1, 6);
00049:         }
00050:
00051:         private static float Clamp01(float v)
00052:             => float.IsNaN(v) ? 0f : Math.Clamp(v, 0f, 1f);
00053:
00054:         private static float Clamp(float v, float lo, float hi)
00055:             => float.IsNaN(v) ? lo : Math.Clamp(v, lo, hi);
00056:     }
00057:
00058:     /// <summary>
00059:     /// Deterministic translation from a resolved moral choice to a rumor seed.
00060:     /// Restrained by design: choices with visible public impact travel, intimate
00061:     /// ones stay quiet, and suppression is an explicit input rather than a hidden
00062:     /// rule. Pure — same inputs, same seed, every time.
00063:     /// </summary>
00064:     public static class MoralChoiceGossipSeed
00065:     {
00066:         /// <summary>Public choices spread; private ones do not become rumor fuel.</summary>
00067:         public const float PublicImpactThreshold = 0.5f;
00068:
00069:         /// <summary>Choices quieter than this never become rumors.</summary>
00070:         public const float MinimumImpactToTravel = 0.25f;
00071:
00072:         /// <summary>
00073:         /// Build a seed, or return null when the choice is too quiet, too private,
00074:         /// or explicitly suppressed. Suppression is a caller decision (a courier
00075:         /// blackout, a jammed channel) so the rule stays inspectable.
00076:         /// </summary>
00077:         public static MoralGossipSeed? Build(
00078:             string questId,
00079:             string originLocationId,
00080:             int resolvedDay,
00081:             string epitaph,
00082:             float publicImpact01,
00083:             bool isPrivate,
00084:             bool suppressed = false,
00085:             float authoredDecayRate = 0.05f,
00086:             int authoredPropagationSpeed = 1)
00087:         {
00088:             if (string.IsNullOrWhiteSpace(questId)) return null;
00089:             if (isPrivate || suppressed) return null;
00090:
00091:             float impact = float.IsNaN(publicImpact01) ? 0f : Math.Clamp(publicImpact01, 0f, 1f);
00092:             if (impact < MinimumImpactToTravel) return null;
00093:
00094:             // A decisive choice travels accurately; a marginal one gets garbled.
00095:             float truthfulness = 0.45f + 0.45f * Math.Clamp(
00096:                 (impact - PublicImpactThreshold) / (1f - PublicImpactThreshold), 0f, 1f);
00097:             float decay = authoredDecayRate * (2f - truthfulness); // untruth travels, fades
00098:             int speed = authoredPropagationSpeed + (impact >= 0.8f ? 1 : 0);
00099:
00100:             string headline = $"Word out of {originLocationId}: {questId}";
00101:             string description = epitaph ?? string.Empty;
00102:
00103:             return new MoralGossipSeed(
00104:                 questId, originLocationId, resolvedDay,
00105:                 headline, description, truthfulness, decay, speed);
00106:         }
00107:     }
00108: }
```

## `src/Host/MoralChoiceSaveStore.cs` — 45 lines; 1,930 bytes; SHA-256 `0c4d6da4e24f76023002cf48e24931f967220096c74954e6f8f33eea949baf3b`
Declaration index:
- 00017: /// in the service; this class keeps the void Save call surface with an
- 00020: public static class MoralChoiceSaveStore
- 00032: public static void Save(MoralChoiceState state, string? pathOverride = null)
- 00037: public static MoralChoiceState? TryLoad(string? pathOverride = null)
- 00043: public static string TryCapturePersisted(MoralChoiceState state) => s_store.CapturePersisted(state);
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Save Store : MoralChoiceSaveStore
00004: // Core State : Ashfall.Core.MoralChoice.MoralChoiceState
00005: // Host Caller: Main.MoralChoice / MoralChoiceHostSession
00006: // Purpose    : Moral choice branches, ethical dilemmas, community trust, and faction reactions
00007: // ============================================================================
00008: using Ashfall.Core.MoralChoice;
00009: using Ashfall.Core.Save;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     /// <summary>
00014:     /// Persists MoralChoiceState as JSON under user://moral_choice_save.json —
00015:     /// thin façade over the Core SaveStore&lt;T&gt; service (via SaveStoreHub).
00016:     /// Checksummed envelope, atomic write, and legacy bare-state loading live
00017:     /// in the service; this class keeps the void Save call surface with an
00018:     /// optional path override used by the host.
00019:     /// </summary>
00020:     public static class MoralChoiceSaveStore
00021:     {
00022:         public const string FileName = "moral_choice_save.json";
00023:         public const string SectionName = "moral_choice";
00024:
00025:         private static readonly SaveStore<MoralChoiceState> s_store =
00026:             SaveStoreHub.Checksummed<MoralChoiceState>(FileName, nameof(MoralChoiceSaveStore));
00027:
00028:         public static string SavePath => s_store.SavePath;
00029:
00030:         public static bool Exists => s_store.Exists();
00031:
00032:         public static void Save(MoralChoiceState state, string? pathOverride = null)
00033:         {
00034:             s_store.TrySave(state, pathOverride);
00035:         }
00036:
00037:         public static MoralChoiceState? TryLoad(string? pathOverride = null)
00038:         {
00039:             return s_store.TryLoad(pathOverride);
00040:         }
00041:
00042:         /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
00043:         public static string TryCapturePersisted(MoralChoiceState state) => s_store.CapturePersisted(state);
00044:     }
00045: }
```

## `src/Main.MoralChoice.cs` — 344 lines; 16,147 bytes; SHA-256 `e12534c6d2a8cd86c90d7283de173c51bf07cd68e46bb52f6129bbeb05375b8d`
Declaration index:
- 00011: public partial class Main : Control
- 00033: private void SetupMoralChoice()
- 00101: public MoralChoiceQuestDefinition? GetMoralChoiceDef(string questId)
- 00108: public List<MoralChoiceQuestDefinition> GetAvailableMoralChoices()
- 00155: private void MarkTrappingMoralEventDelivered(string questId)
- 00168: public List<MoralChoiceQuestDefinition> GetResolvedMoralChoices()
- 00180: public MoralChoiceResolution? GetMoralChoiceResolution(string questId)
- 00188: public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyMoralOffers(int maxOffers = 1)
- 00199: public bool TryResolveMoralChoice(string questId, int choiceIndex)
- 00224: private void SeedMoralChoiceGossip(Ashfall.Core.MoralChoice.MoralChoiceResolution? resolution)
- 00265: private Ashfall.Core.Flags.OneShotTriggerLedger GossipTriggers()
- 00271: private void WriteMoralChoiceJournalEntry(MoralChoiceResolution resolution)
- 00281: private void WriteBranchLockoutJournalEntry(string lockedBranchId)
- 00299: private MoralThresholdReaction? GetFactionReaction(string eventId)
- 00311: private void WriteThresholdEventJournalEntry(string eventId)
- 00326: private MoralPathBand GetCurrentGossipBand()
- 00332: private void SaveMoralChoice()
- 00339: private void FlushMoralChoiceIfDirty()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Collections.Generic;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.MoralChoice;
00008:
00009: namespace AtomicWar.GodotApp
00010: {
00011:     public partial class Main : Control
00012:     {
00013:         // ── Moral choice ("The Weight of Survival") host wiring ──
00014:         // The score is invisible by design: hosts read CurrentBand and the
00015:         // threshold events, never the raw number.
00016:         private MoralChoiceSystem _moralChoice = null!;
00017:         private List<MoralChoiceQuestDefinition> _moralChoiceDefs = new List<MoralChoiceQuestDefinition>();
00018:         private bool _moralChoiceDirty;
00019:
00020:         // ── Branching / gossip / faction reactions (Phase 2 data) ──
00021:         private MoralChoiceChainData _moralChainData = new MoralChoiceChainData();
00022:         private MoralChoiceGossipData _moralGossipData = new MoralChoiceGossipData();
00023:         private MoralChoiceFactionReactionsData _moralFactionReactions = new MoralChoiceFactionReactionsData();
00024:         private MoralChoiceFlagDefinitions _moralFlagDefs = new MoralChoiceFlagDefinitions();
00025:         private MoralChoiceGossipRuntime _moralGossipRuntime = null!;
00026:
00027:         /// <summary>
00028:         /// Fixed world seed so every host agrees on unseeded rolls; per-save
00029:         /// outcome rolls and propagation days are stored in the ledger DTO.
00030:         /// </summary>
00031:         private const int MoralChoiceSeed = 20260825;
00032:
00033:         private void SetupMoralChoice()
00034:         {
00035:             if (_moralChoice != null) return;
00036:             SetupJournal();
00037:             SetupCampaignDay();
00038:             var fileIO = new FileSystemIO();
00039:             var json = new SystemTextJsonSerializer();
00040:
00041:             _moralChoice = new MoralChoiceSystem(_campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.MoralChoice).Rng, flags: _consequenceLedger);
00042:             _moralChoiceDefs = MoralChoiceCatalogLoader.Load(_dataDir, fileIO, json);
00043:
00044:             // Load branching chain quests and merge into the catalog
00045:             var chainQuests = MoralChoiceBranchQuestCatalogLoader.Load(_dataDir, fileIO, json);
00046:             _moralChoiceDefs.AddRange(chainQuests);
00047:
00048:             // Load expansion quests and merge into the catalog
00049:             var expansionQuests = MoralChoiceExpansionQuestCatalogLoader.Load(_dataDir, fileIO, json);
00050:             _moralChoiceDefs.AddRange(expansionQuests);
00051:
00052:             // Register all definitions into the Core system's authoritative catalog
00053:             _moralChoice.RegisterQuests(_moralChoiceDefs);
00054:
00055:             // Load chain architecture (branches, gates, echo quests)
00056:             _moralChainData = MoralChoiceChainCatalogLoader.Load(_dataDir, fileIO, json);
00057:             _moralChoice.InitializeChainData(_moralChainData);
00058:
00059:             // Load gossip, faction reactions, and flag definitions
00060:             _moralGossipData = MoralChoiceGossipCatalogLoader.Load(_dataDir, fileIO, json);
00061:             _moralFactionReactions = MoralChoiceFactionReactionsCatalogLoader.Load(_dataDir, fileIO, json);
00062:             _moralFlagDefs = MoralChoiceFlagCatalogLoader.Load(_dataDir, fileIO, json);
00063:             _moralGossipRuntime = new MoralChoiceGossipRuntime(_moralGossipData, _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.MoralChoice, 0, 1));
00064:
00065:             _moralChoice.OnQuestResolved += WriteMoralChoiceJournalEntry;
00066:             _moralChoice.OnQuestResolved += _ => _moralChoiceDirty = true;
00067:             // Plan IV Task 5: a resolved trapping dilemma acks its pending
00068:             // outbox fact so the outbox never re-surfaces it after restore.
00069:             _moralChoice.OnQuestResolved += resolution =>
00070:             {
00071:                 if (resolution.questId != null && resolution.questId.StartsWith(TrappingMoralQuestIdPrefix, StringComparison.Ordinal))
00072:                     MarkTrappingMoralEventDelivered(resolution.questId);
00073:             };
00074:             _moralChoice.OnThresholdEventFired += WriteThresholdEventJournalEntry;
00075:             _moralChoice.OnThresholdEventFired += _ => _moralChoiceDirty = true;
00076:             _moralChoice.OnBranchLocked += WriteBranchLockoutJournalEntry;
00077:             _moralChoice.OnBranchLocked += _ => _moralChoiceDirty = true;
00078:
00079:             var save = MoralChoiceSaveStore.TryLoad();
00080:             if (save != null)
00081:             {
00082:                 try
00083:                 {
00084:                     _moralChoice.RestoreState(save);
00085:                     GD.Print($"[Ashfall Godot] Moral choice ledger restored " +
00086:                              $"(day {save.lastReconciledDay}, {_moralChoice.QuestsResolved} resolved).");
00087:                 }
00088:                 catch (Exception e)
00089:                 {
00090:                     GD.PrintErr($"[Ashfall Godot] Moral choice restore rejected: {e.Message}");
00091:                 }
00092:             }
00093:             GD.Print($"[Ashfall Godot] Moral choice ready. {_moralChoiceDefs.Count} quests " +
00094:                      $"({_moralChainData.Branches.Count} branches, " +
00095:                      $"{_moralGossipData.CampChatter.Neutral.Count} neutral chatter lines).");
00096:         }
00097:
00098:         public MoralChoiceSystem MoralChoice => _moralChoice;
00099:         public IReadOnlyList<MoralChoiceQuestDefinition> MoralChoiceDefs => _moralChoiceDefs;
00100:
00101:         public MoralChoiceQuestDefinition? GetMoralChoiceDef(string questId)
00102:         {
00103:             SetupMoralChoice();
00104:             return _moralChoiceDefs.FirstOrDefault(
00105:                 d => string.Equals(d.Id, questId, StringComparison.Ordinal));
00106:         }
00107:
00108:         public List<MoralChoiceQuestDefinition> GetAvailableMoralChoices()
00109:         {
00110:             SetupMoralChoice();
00111:             var list = new List<MoralChoiceQuestDefinition>();
00112:             foreach (var d in _moralChoiceDefs)
00113:             {
00114:                 // Plan IV Task 5: trapping-sourced dilemmas are excluded from
00115:                 // the standing offer pass — they surface only while their
00116:                 // moral-consequence fact is pending in the trapping outbox.
00117:                 if (d.Id != null && d.Id.StartsWith(TrappingMoralQuestIdPrefix, StringComparison.Ordinal))
00118:                     continue;
00119:                 if (!_moralChoice.IsResolved(d.Id) &&
00120:                     MoralChoiceSystem.IsAvailableOnDay(d, _simDay) &&
00121:                     _moralChoice.IsChainQuestAccessible(d.Id, _simDay))
00122:                 {
00123:                     list.Add(d);
00124:                 }
00125:             }
00126:
00127:             // Plan IV Task 5: surface pending trapping dilemmas in outbox
00128:             // sequence order. The pending fact is the persistence owner until
00129:             // the player resolves the dilemma in the moral ledger.
00130:             if (_wildlifeTrapping != null)
00131:             {
00132:                 var pending = _wildlifeTrapping.System.GetPendingEvents();
00133:                 foreach (var ev in pending)
00134:                 {
00135:                     if (ev == null || !string.Equals(ev.kind, WildlifeTrappingEventKinds.MoralConsequence, StringComparison.Ordinal))
00136:                         continue;
00137:                     var def = GetMoralChoiceDef(ev.payloadId);
00138:                     if (def == null || _moralChoice.IsResolved(def.Id)) continue;
00139:                     if (!list.Any(l => string.Equals(l.Id, def.Id, StringComparison.Ordinal)))
00140:                         list.Add(def);
00141:                 }
00142:             }
00143:             return list;
00144:         }
00145:
00146:         /// <summary>Catalog id prefix shared by all trapping-sourced moral dilemmas.</summary>
00147:         public const string TrappingMoralQuestIdPrefix = "quest_moral_trap_prey_";
00148:
00149:         /// <summary>
00150:         /// Plan IV Task 5: ack every pending moral-consequence fact that maps
00151:         /// to the given quest id. Called from the resolution event so the
00152:         /// trapping outbox marks the fact delivered only after the moral
00153:         /// ledger committed the resolution.
00154:         /// </summary>
00155:         private void MarkTrappingMoralEventDelivered(string questId)
00156:         {
00157:             if (_wildlifeTrapping == null) return;
00158:             var pending = _wildlifeTrapping.System.GetPendingEvents();
00159:             foreach (var ev in pending)
00160:             {
00161:                 if (ev == null || !string.Equals(ev.kind, WildlifeTrappingEventKinds.MoralConsequence, StringComparison.Ordinal))
00162:                     continue;
00163:                 if (!string.Equals(ev.payloadId, questId, StringComparison.Ordinal)) continue;
00164:                 _wildlifeTrapping.System.MarkEventDelivered(ev.eventId);
00165:             }
00166:         }
00167:
00168:         public List<MoralChoiceQuestDefinition> GetResolvedMoralChoices()
00169:         {
00170:             SetupMoralChoice();
00171:             var list = new List<MoralChoiceQuestDefinition>();
00172:             foreach (var d in _moralChoiceDefs)
00173:             {
00174:                 if (_moralChoice.IsResolved(d.Id))
00175:                     list.Add(d);
00176:             }
00177:             return list;
00178:         }
00179:
00180:         public MoralChoiceResolution? GetMoralChoiceResolution(string questId)
00181:         {
00182:             SetupMoralChoice();
00183:             if (_moralChoice.TryGetResolution(questId, out var res))
00184:                 return res;
00185:             return null;
00186:         }
00187:
00188:         public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyMoralOffers(int maxOffers = 1)
00189:         {
00190:             SetupMoralChoice();
00191:             return _moralChoice.GetDailyOffers(_simDay, maxOffers);
00192:         }
00193:
00194:         /// <summary>
00195:         /// Resolve a catalog quest by id. Returns false when the id is unknown
00196:         /// or the quest is already resolved; the journal line is written by
00197:         /// the event hook, and overnight settlement lands in TickSimDay.
00198:         /// </summary>
00199:         public bool TryResolveMoralChoice(string questId, int choiceIndex)
00200:         {
00201:             SetupMoralChoice();
00202:             var def = _moralChoiceDefs.FirstOrDefault(
00203:                 d => string.Equals(d.Id, questId, StringComparison.Ordinal));
00204:             if (def == null) return false;
00205:             if (!_moralChoice.TryResolve(questId, choiceIndex, def.LocationId, _simDay, out var resolveResult))
00206:                 return false;
00207:
00208:             // CORE-MECH W8: a choice with a public footprint seeds the canonical
00209:             // rumor network. RumorSystem stays the only rumor authority; the seed
00210:             // builder is pure; the one-shot trigger guarantees one seed per choice.
00211:             SeedMoralChoiceGossip(resolveResult?.Resolution);
00212:
00213:             _moralChoiceDirty = true;
00214:             AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.UiConfirm);
00215:             return true;
00216:         }
00217:
00218:         /// <summary>
00219:         /// CORE-MECH W8 — turn a resolved choice into a rumor seed and hand it to
00220:         /// the existing RumorSystem. Uses the authored <c>propagatesOnDay</c> hook
00221:         /// (gossip leaves the witnessing circle on resolvedDay + 1..3). Fails closed
00222:         /// when the info owner is not set up: the choice still resolves.
00223:         /// </summary>
00224:         private void SeedMoralChoiceGossip(Ashfall.Core.MoralChoice.MoralChoiceResolution? resolution)
00225:         {
00226:             if (resolution == null || string.IsNullOrEmpty(resolution.questId)) return;
00227:
00228:             float impact = Math.Clamp(Math.Abs(resolution.moralDelta) / 20f, 0f, 1f);
00229:             if (resolution.empathyDelta > 0) impact = Math.Clamp(impact + 0.15f, 0f, 1f);
00230:
00231:             var seed = Ashfall.Core.MoralChoice.MoralChoiceGossipSeed.Build(
00232:                 resolution.questId,
00233:                 resolution.locationId,
00234:                 resolution.propagatesOnDay > 0 ? resolution.propagatesOnDay : resolution.resolvedDay,
00235:                 resolution.epitaph,
00236:                 impact,
00237:                 isPrivate: false);
00238:             if (seed == null) return;
00239:
00240:             SetupRumorNetwork();
00241:             if (_rumorNetwork == null) return;
00242:
00243:             // One seed per choice, ever — the W11 primitive rebuilt inline to clear
00244:             // the W8→W11 ordering dependency.
00245:             var triggers = GossipTriggers();
00246:             if (!triggers.TryFire("moral." + seed.SubjectId, Math.Max(1, seed.OriginDay)))
00247:                 return;
00248:
00249:             var rumor = _rumorNetwork.System.GenerateRumor(
00250:                 seed.OriginLocationId,
00251:                 Ashfall.Core.InformationFlow.RumorSubjectType.Faction,
00252:                 seed.SubjectId,
00253:                 seed.Headline,
00254:                 seed.Description,
00255:                 seed.Truthfulness,
00256:                 seed.OriginDay);
00257:             rumor.DecayRate = seed.DecayRate;
00258:             rumor.PropagationSpeed = seed.PropagationSpeed;
00259:             // The rumor host raises its own StateChanged on generation (Main binds
00260:             // that to the dirty flag), so the seed needs no extra bookkeeping.
00261:             _rumorNetworkDirty = true;
00262:         }
00263:
00264:         /// <summary>W8/W11 shared trigger ledger over the campaign consequence ledger.</summary>
00265:         private Ashfall.Core.Flags.OneShotTriggerLedger GossipTriggers()
00266:             => _gossipTriggers ??= new Ashfall.Core.Flags.OneShotTriggerLedger(_consequenceLedger);
00267:
00268:         private Ashfall.Core.Flags.OneShotTriggerLedger? _gossipTriggers;
00269:
00270:         /// <summary>Journal integration: one entry per resolution, arrow only — never the number.</summary>
00271:         private void WriteMoralChoiceJournalEntry(MoralChoiceResolution resolution)
00272:         {
00273:             SetupJournal();
00274:             string arrow = resolution.impactMark == "up" ? "🔺"
00275:                 : resolution.impactMark == "down" ? "🔻" : "⚪";
00276:             _journal.TryAddRawEntry(resolution.questId, $"{arrow} {resolution.epitaph}", null!, resolution.resolvedDay);
00277:             _journalDirty = true;
00278:         }
00279:
00280:         /// <summary>Branch lockout journal entry: a door has closed.</summary>
00281:         private void WriteBranchLockoutJournalEntry(string lockedBranchId)
00282:         {
00283:             if (_moralChainData?.LockoutRules == null) return;
00284:             var branch = _moralChainData.Branches.FirstOrDefault(
00285:                 b => string.Equals(b.Id, lockedBranchId, StringComparison.Ordinal));
00286:             string branchName = branch?.DisplayName ?? lockedBranchId;
00287:             string template = _moralChainData.LockoutRules.LockoutJournalTemplate;
00288:             string text = template.Replace("{locked_branch_name}", branchName);
00289:
00290:             SetupJournal();
00291:             _journal.TryAddRawEntry($"branch_lockout_{lockedBranchId}", text, null!, _simDay);
00292:             _journalDirty = true;
00293:         }
00294:
00295:         /// <summary>
00296:         /// Get the faction reaction dialogue for a threshold event.
00297:         /// Returns null if no reaction data exists for the event.
00298:         /// </summary>
00299:         private MoralThresholdReaction? GetFactionReaction(string eventId)
00300:         {
00301:             SetupMoralChoice();
00302:             if (_moralFactionReactions.ThresholdReactions.TryGetValue(eventId, out var reaction))
00303:                 return reaction;
00304:             return null;
00305:         }
00306:
00307:         /// <summary>
00308:         /// Journal the authored faction reaction when a moral threshold fires.
00309:         /// Uses the catalog journal line when present; otherwise a restrained fallback.
00310:         /// </summary>
00311:         private void WriteThresholdEventJournalEntry(string eventId)
00312:         {
00313:             var reaction = GetFactionReaction(eventId);
00314:             string text = reaction != null && !string.IsNullOrWhiteSpace(reaction.JournalEntry)
00315:                 ? reaction.JournalEntry
00316:                 : $"Threshold crossed: {eventId}.";
00317:
00318:             SetupJournal();
00319:             _journal.TryAddRawEntry($"moral_threshold_{eventId}", text, null!, _simDay);
00320:             _journalDirty = true;
00321:         }
00322:
00323:         /// <summary>
00324:         /// Get the current gossip band (with decay) for NPC interactions.
00325:         /// </summary>
00326:         private MoralPathBand GetCurrentGossipBand()
00327:         {
00328:             SetupMoralChoice();
00329:             return _moralGossipRuntime.GetEffectiveGossipBand(_moralChoice, _simDay);
00330:         }
00331:
00332:         private void SaveMoralChoice()
00333:         {
00334:             if (_moralChoice == null) return;
00335:             if (CaptureSection("moral_choice", MoralChoiceSaveStore.TryCapturePersisted(_moralChoice.CaptureState())))
00336:                 _moralChoiceDirty = false;
00337:         }
00338:
00339:         private void FlushMoralChoiceIfDirty()
00340:         {
00341:             if (_moralChoiceDirty) SaveMoralChoice();
00342:         }
00343:     }
00344: }
```

## `src/Main.CampaignOwners.cs` — 2,957 lines; 146,936 bytes; SHA-256 `6c612e267459f02941f6c11c6536eaba89417aff5cf2a99aa28350aba04b7930`
Declaration index:
- 00013: public partial class Main
- 00015: private void RegisterProductionCampaignOwners()
- 00214: private sealed class TerritoryControlDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00220: public void CapturePreDaySnapshot(int day)
- 00226: public void RestorePreDaySnapshot(int day)
- 00231: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00242: private sealed class CookingDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00248: public void CapturePreDaySnapshot(int day)
- 00254: public void RestorePreDaySnapshot(int day)
- 00259: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00270: private sealed class WeatherCascadeDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00276: public void CapturePreDaySnapshot(int day)
- 00282: public void RestorePreDaySnapshot(int day)
- 00287: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00299: private sealed class OutpostSettlementDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00304: public void CapturePreDaySnapshot(int day)
- 00309: public void RestorePreDaySnapshot(int day)
- 00313: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00325: private sealed class RetentionDayOwner : IDayAdvanceOwner
- 00329: public void CapturePreDaySnapshot(int day) { /* retention is idempotent; captured via save section */ }
- 00330: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00342: private sealed class NeedsPerformanceDayOwner : IDayAdvanceOwner
- 00346: public void CapturePreDaySnapshot(int day) { /* derived read projection */ }
- 00347: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00358: private sealed class CampaignLegacyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00364: public void CapturePreDaySnapshot(int day)
- 00370: public void RestorePreDaySnapshot(int day)
- 00375: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00386: private sealed class ResearchUnlockDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00392: public void CapturePreDaySnapshot(int day)
- 00398: public void RestorePreDaySnapshot(int day)
- 00403: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00414: private sealed class UnifiedEndingDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00420: public void CapturePreDaySnapshot(int day)
- 00426: public void RestorePreDaySnapshot(int day)
- 00431: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00442: private sealed class NpcMemoryDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00448: public void CapturePreDaySnapshot(int day)
- 00454: public void RestorePreDaySnapshot(int day)
- 00459: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00470: private sealed class IdeologicalFrictionDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00476: public void CapturePreDaySnapshot(int day)
- 00482: public void RestorePreDaySnapshot(int day)
- 00487: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00498: private sealed class RomanceFamilyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00504: public void CapturePreDaySnapshot(int day)
- 00510: public void RestorePreDaySnapshot(int day)
- 00515: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00526: private sealed class VehicleCustomizationDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00532: public void CapturePreDaySnapshot(int day)
- 00538: public void RestorePreDaySnapshot(int day)
- 00543: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00554: private sealed class BackstoryDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00560: public void CapturePreDaySnapshot(int day)
- 00566: public void RestorePreDaySnapshot(int day)
- 00571: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00582: private sealed class MetaProgressionDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00588: public void CapturePreDaySnapshot(int day)
- 00594: public void RestorePreDaySnapshot(int day)
- 00599: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00612: private sealed class TunnelNetworkDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00618: public void CapturePreDaySnapshot(int day)
- 00624: public void RestorePreDaySnapshot(int day)
- 00629: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00640: private sealed class ShelterIdentityDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00646: public void CapturePreDaySnapshot(int day)
- 00652: public void RestorePreDaySnapshot(int day)
- 00657: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00668: private sealed class TradeRouteDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00674: public void CapturePreDaySnapshot(int day)
- 00680: public void RestorePreDaySnapshot(int day)
- 00685: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00696: private sealed class HumanMigrationDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00702: public void CapturePreDaySnapshot(int day)
- 00708: public void RestorePreDaySnapshot(int day)
- 00713: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00724: private sealed class ShelterGovernanceDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00730: public void CapturePreDaySnapshot(int day)
- 00736: public void RestorePreDaySnapshot(int day)
- 00741: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00752: private sealed class AgingDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00758: public void CapturePreDaySnapshot(int day)
- 00764: public void RestorePreDaySnapshot(int day)
- 00769: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00780: private sealed class ShelterMaintenanceDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00786: public void CapturePreDaySnapshot(int day)
- 00792: public void RestorePreDaySnapshot(int day)
- 00797: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00808: private sealed class SurvivorRoutinesDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00814: public void CapturePreDaySnapshot(int day)
- 00820: public void RestorePreDaySnapshot(int day)
- 00825: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00837: private sealed class WeatherWorldDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00842: public void CapturePreDaySnapshot(int day)
- 00847: public void RestorePreDaySnapshot(int day)
- 00852: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00888: private sealed class DeepCoastMaritimeDayOwner : IDayAdvanceOwner
- 00892: public void CapturePreDaySnapshot(int day) { }
- 00893: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00905: private sealed class PowerGridDayOwner : IDayAdvanceOwner
- 00909: public void CapturePreDaySnapshot(int day) { }
- 00910: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00952: private sealed class NuclearCoreDayOwner : IDayAdvanceOwner
- 00956: public void CapturePreDaySnapshot(int day) { }
- 00957: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00970: private sealed class HoldfastCoreDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 00975: public void CapturePreDaySnapshot(int day)
- 00981: public void RestorePreDaySnapshot(int day)
- 00985: public void TickDay(int day, List<DayStateChangeEvent> events)
- 01004: private sealed class StartingLevelRationsDayOwner : IDayAdvanceOwner
- 01008: public void CapturePreDaySnapshot(int day) { }
- 01009: public void TickDay(int day, List<DayStateChangeEvent> events)
- 01037: private sealed class CraftingProductionDayOwner : IDayAdvanceOwner
- 01041: public void CapturePreDaySnapshot(int day) { }
- 01042: public void TickDay(int day, List<DayStateChangeEvent> events)
- 01050: private sealed class GreenhouseFoundryDayOwner : IDayAdvanceOwner
- 01054: public void CapturePreDaySnapshot(int day) { }
- 01055: public void TickDay(int day, List<DayStateChangeEvent> events)
- 01089: private sealed class SeismicGeologyDayOwner : IDayAdvanceOwner
- 01093: public void CapturePreDaySnapshot(int day) { }
- 01094: public void TickDay(int day, List<DayStateChangeEvent> events)
- 01108: private sealed class CryoVaultDayOwner : IDayAdvanceOwner
- 01112: public void CapturePreDaySnapshot(int day) { }
- 01113: public void TickDay(int day, List<DayStateChangeEvent> events)
- 01127: private sealed class PrecisionMetrologyDayOwner : IDayAdvanceOwner
- 01131: public void CapturePreDaySnapshot(int day) { }
- 01132: public void TickDay(int day, List<DayStateChangeEvent> events)
- 01143: private sealed class AquaponicsDayOwner : IDayAdvanceOwner
- 01147: public void CapturePreDaySnapshot(int day) { }
- 01148: public void TickDay(int day, List<DayStateChangeEvent> events)
- 01155: private sealed class PsychologyArcsDayOwner : IDayAdvanceOwner
- 01159: public void CapturePreDaySnapshot(int day) { }
- 01160: public void TickDay(int day, List<DayStateChangeEvent> events)
- 01167: private sealed class EconomyMarketDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
- 01172: public void CapturePreDaySnapshot(int day)
- 01177: public void RestorePreDaySnapshot(int day)
- 01182: public void TickDay(int day, List<DayStateChangeEvent> events)
- 01196: private sealed class ShelterFacilitiesDayOwner : IDayAdvanceOwner
- 01200: public void CapturePreDaySnapshot(int day) { }
- 01201: public void TickDay(int day, List<DayStateChangeEvent> events)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Campaign;
00007: using Ashfall.Core.Economy;
00008: using Ashfall.Core.Expeditions;
00009: using Godot;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     public partial class Main
00014:     {
00015:         private void RegisterProductionCampaignOwners()
00016:         {
00017:             if (_campaignDay == null) return;
00018:
00019:             // Phase 1: Environment, Weather & Base Core
00020:             _campaignDay.Register("holdfast_core", new HoldfastCoreDayOwner(this), phase: 1);
00021:             _campaignDay.Register("maritime_deep_coast", new DeepCoastMaritimeDayOwner(this), phase: 1);
00022:             _campaignDay.Register("power_grid", new PowerGridDayOwner(this), phase: 1);
00023:             // Plan B98: nuclear output is an external, fuel-free projection.
00024:             // Its ordinal sorts before power_grid so restored RTG/sealed-cell
00025:             // output is visible when the day's load is resolved.
00026:             _campaignDay.Register("nuclear_core", new NuclearCoreDayOwner(this), phase: 1);
00027:             // Plans B74-B77: ORC output is published before the grid owner
00028:             // resolves the day's load, while chamber and tube milestones run
00029:             // in the production phase.
00030:             _campaignDay.Register("geothermal_orc", new GeothermalOrcDayOwner(this), phase: 1);
00031:             _campaignDay.Register("weather_world", new WeatherWorldDayOwner(this), phase: 1);
00032:             // Plan B68 — geological pulse progression precedes production
00033:             // (foundry sees the quake's interruption context the same day).
00034:             _campaignDay.Register("seismic_geology", new SeismicGeologyDayOwner(this), phase: 1);
00035:
00036:             // Phase 2: Production, Infrastructure & Survival Basics
00037:             _campaignDay.Register("crafting_production", new CraftingProductionDayOwner(this), phase: 2);
00038:             _campaignDay.Register("economy_market", new EconomyMarketDayOwner(this), phase: 2);
00039:             _campaignDay.Register("greenhouse_foundry", new GreenhouseFoundryDayOwner(this), phase: 2);
00040:             _campaignDay.Register("aeroponics", new AeroponicsDayOwner(this), phase: 2);
00041:             _campaignDay.Register("pneumatic_dispatch", new PneumaticDispatchDayOwner(this), phase: 2);
00042:             // Plan B69 — cryo thermal/viability update follows the foundry
00043:             // (shared grid: brownout from the furnace reaches the vault same-day).
00044:             _campaignDay.Register("cryo_vault", new CryoVaultDayOwner(this), phase: 2);
00045:             // Plan B89 — precision metrology drift + workshop projection after
00046:             // seismic (phase 1) so quake disturbance lands before daily drift.
00047:             _campaignDay.Register("precision_metrology", new PrecisionMetrologyDayOwner(this), phase: 2);
00048:             // Plan B87 — aquaponics ecology after power/thermal owners so
00049:             // brownout and room heat are queryable for the same day.
00050:             _campaignDay.Register("aquaponics", new AquaponicsDayOwner(this), phase: 2);
00051:             _campaignDay.Register("shelter_facilities", new ShelterFacilitiesDayOwner(this), phase: 2);
00052:             _campaignDay.Register("shelter_fire", new ShelterFireDayOwner(this), phase: 2);
00053:             _campaignDay.Register("starting_level_rations", new StartingLevelRationsDayOwner(this), phase: 2);
00054:             _campaignDay.Register("plan_166_research", new Plan166ResearchDayOwner(this), phase: 4);
00055:             _campaignDay.Register("plan_168_fluid", new Plan168FluidDayOwner(this), phase: 2);
00056:
00057:             // Phase 3: Survivors, Medical, Disease & Social
00058:             _campaignDay.Register("duty_roster", new DutyRosterDayOwner(this), phase: 3);
00059:             // Plan 210 — sanitation runs BEFORE the disease tick within phase 3:
00060:             // `hygiene` (h) sorts alphabetically before `medical_disease` (m),
00061:             // so waste burden → hygiene → pathogen exposure modifiers are
00062:             // current when the disease authority resolves its daily tick.
00063:             _campaignDay.Register("hygiene", new HygieneDayOwner(this), phase: 3);
00064:             _campaignDay.Register("medical_disease", new MedicalDiseaseDayOwner(this), phase: 3);
00065:             _campaignDay.Register("phase0_psychology", new Phase0PsychologyDayOwner(this), phase: 3);
00066:             _campaignDay.Register("survivor_social", new SurvivorSocialDayOwner(this), phase: 3);
00067:             _campaignDay.Register("survivors_needs", new SurvivorsNeedsDayOwner(this), phase: 3);
00068:
00069:             // Phase 4: Expeditions, World, Factions & Quests
00070:             _campaignDay.Register("expeditions_caravans", new ExpeditionsCaravansDayOwner(this), phase: 4);
00071:             _campaignDay.Register("narrative_quests_verdict", new NarrativeQuestsVerdictDayOwner(this), phase: 4);
00072:             // Task 122: ticks after expeditions (ordinal 'w' > 'e') so it reads
00073:             // fresh sortie results, and after narrative for fresh faction dominance.
00074:             _campaignDay.Register("world_evolution", new EvolvingWorldDayOwner(this), phase: 4);
00075:             // Plan IV: ledger debt ages with the campaign; forfeits dispatch
00076:             // consequences into faction war / raids / inventory / labor.
00077:             _campaignDay.Register("debt_ledger", new DebtLedgerDayOwner(this), phase: 4);
00078:             // Plan 211 — underworld stock refresh + debt/heat tick AFTER the
00079:             // debt-ledger tick: `underworld_market` (u) sorts alphabetically
00080:             // after `debt_ledger` (d) within phase 4 (owner id is not the
00081:             // section key — the save section is `black_market`).
00082:             _campaignDay.Register("underworld_market", new UnderworldMarketDayOwner(this), phase: 4);
00083:             // Flagship XI (Plan 156): underground hazards tick after expeditions
00084:             // (ordinal 's' > 'e', < 'w') so the bridge reads fresh sortie phases.
00085:             _campaignDay.Register("subterranean_network", new SubterraneanDayOwner(this), phase: 4);
00086:             // Flagship XI (Plan 157): psyops broadcast day resolves after
00087:             // expeditions (leaflets) and alongside the world-evolution radio feed.
00088:             _campaignDay.Register("psyops", new PsyOpsDayOwner(this), phase: 4);
00089:             // Plan 173 Phase 2: program prep ticks after psyops so StartCampaign
00090:             // on delivery can reach an already-constructed PsyOpsSystem.
00091:             _campaignDay.Register("radio_program_production", new RadioProgramProductionDayOwner(this), phase: 4);
00092:             // Plans 162-165 (Plan 164): breakdown arcs evaluate AFTER the
00093:             // phase-3 needs tick finalized canonical stress (plan §11.3).
00094:             _campaignDay.Register("psychology_arcs_162", new PsychologyArcsDayOwner(this), phase: 4);
00095:             _campaignDay.Register("plan_167_espionage", new Plan167EspionageDayOwner(this), phase: 4);
00096:             _campaignDay.Register("plan_169_procedural_narrative", new Plan169NarrativeDayOwner(this), phase: 4);
00097:             // Plan 38 — commitments/deadlines evaluate late in phase 4 so the day's
00098:             // expedition/faction facts are settled before a missed obligation routes
00099:             // its consequence into faction standing and the consequence ledger.
00100:             // "shelter_commitments" is the owner id (not the section key `commitment`).
00101:             _campaignDay.Register("shelter_commitments", new CommitmentDayOwner(this), phase: 4);
00102:
00103:             // Phase 5: Events, Memorial & Final Evaluation
00104:             _campaignDay.Register("host_events", new HostEventsDayOwner(this), phase: 5);
00105:             _campaignDay.Register("memorial", new MemorialDayOwner(this), phase: 5);
00106:             // Plan 29 29A: room-history day milestones. Reads only the identity
00107:             // catalog and writes journal knowledge keys; no system ticks here.
00108:             _campaignDay.Register("shelter_room_history", new ShelterRoomHistoryDayOwner(this), phase: 5);
00109:             // Plan 58 — the outpost network runs after the roster, expedition and
00110:             // economy owners so its garrison, rations and hostile pressure read
00111:             // the day's already-finalized population and supply state.
00112:             _campaignDay.Register("outpost_settlement", new OutpostSettlementDayOwner(this), phase: 5);
00113:             // Plan 135 — the weather→gameplay cascade expires fronts whose
00114:             // duration ended, so it runs after the owners that consumed the
00115:             // day's weather and before retention bounds the logs they wrote.
00116:             _campaignDay.Register("weather_cascade", new WeatherCascadeDayOwner(this), phase: 5);
00117:             // Plan 134 — dynamic faction territory and supply line control: delivers
00118:             // active corridors and reinforces held nodes.
00119:             _campaignDay.Register("territory_control", new TerritoryControlDayOwner(this), phase: 5);
00120:             // Plan 136 — wildlife trapping food pipeline & cooking system: progresses
00121:             // active cooking operations and decontaminates fallout-tainted meat.
00122:             _campaignDay.Register("cooking", new CookingDayOwner(this), phase: 5);
00123:             // Plan 137 — needs to performance cascade: evaluates hunger/thirst/fatigue/cold on survivor performance.
00124:             _campaignDay.Register("needs_performance", new NeedsPerformanceDayOwner(this), phase: 5);
00125:             // Plan 140 — generational legacy and campaign inheritance: evaluates active traits and heritage continuity.
00126:             _campaignDay.Register("campaign_legacy", new CampaignLegacyDayOwner(this), phase: 5);
00127:             // Plan 141 — research downstream unlocks bridge: grants breakthrough items, crafting recipes, and capabilities.
00128:             _campaignDay.Register("research_unlock", new ResearchUnlockDayOwner(this), phase: 5);
00129:             // Plan 145 — unified ending resolution: evaluates whole-campaign state and epilogue personalization.
00130:             _campaignDay.Register("unified_ending", new UnifiedEndingDayOwner(this), phase: 5);
00131:             // Plan 147 — per-NPC memory and relationship depth: decays old memories and grudges.
00132:             _campaignDay.Register("npc_memory", new NpcMemoryDayOwner(this), phase: 5);
00133:             // Plan 148 — ideological friction: evaluates bunker frictions, conversions, and confrontations.
00134:             _campaignDay.Register("ideological_friction", new IdeologicalFrictionDayOwner(this), phase: 5);
00135:             // Plan 150 — romance & family: advances bonded tenure and forms new attractions from canonical affinity.
00136:             _campaignDay.Register("romance_family", new RomanceFamilyDayOwner(this), phase: 5);
00137:             // Plan 152 — vehicle customization & mobile base: keeps the module catalog bound for the day report.
00138:             _campaignDay.Register("vehicle_customization", new VehicleCustomizationDayOwner(this), phase: 5);
00139:             // Plan 174 — procedural survivor backstories: updates origin mechanics.
00140:             _campaignDay.Register("backstory", new BackstoryDayOwner(this), phase: 5);
00141:             // Plan 175 — meta progression: evaluates prestige and New Game+ boons.
00142:             _campaignDay.Register("meta_progression", new MetaProgressionDayOwner(this), phase: 5);
00143:             // Plan 167 — underground tunnel network: applies daily structural wear and collapse risk.
00144:             _campaignDay.Register("tunnel_network", new TunnelNetworkDayOwner(this), phase: 5);
00145:             // Plan 166 — shelter identity: selects the founding origin on a fresh campaign.
00146:             _campaignDay.Register("shelter_identity", new ShelterIdentityDayOwner(this), phase: 5);
00147:             // Plan 192 — scheduled trade route contracts: advances run schedules and audits tariffs.
00148:             _campaignDay.Register("trade_routes", new TradeRouteDayOwner(this), phase: 5);
00149:             // Plan 199 — seasonal human migration: tracks regional population weight transitions.
00150:             _campaignDay.Register("human_migration", new HumanMigrationDayOwner(this), phase: 5);
00151:             // Plan 159 — shelter governance: evaluates policy consent, disputes, and shelter stability.
00152:             _campaignDay.Register("shelter_governance", new ShelterGovernanceDayOwner(this), phase: 5);
00153:             // Plan 176 — aging & elderly survivor system: advances chronological age and evaluates milestones/retirement.
00154:             _campaignDay.Register("aging", new AgingDayOwner(this), phase: 5);
00155:             // Expansion 25 — rail track maintenance ledger (event-driven wear; no daily baseline).
00156:             _campaignDay.Register("rail_track_maintenance", new RailTrackMaintenanceDayOwner(this), phase: 5);
00157:             // Expansion 29 — glassworks kiln annealing stages.
00158:             _campaignDay.Register("glassworks", new GlassworksDayOwner(this), phase: 5);
00159:             _campaignDay.Register("broadsheet_press", new BroadsheetPressDayOwner(this), phase: 5);
00160:             _campaignDay.Register("kilnworks", new KilnworksDayOwner(this), phase: 5);
00161:             // Expansion 32 — wildlife harvest quota ledger (season-scoped; quota evaluation is event-driven).
00162:             _campaignDay.Register("wildlife_harvest", new WildlifeHarvestDayOwner(this), phase: 5);
00163:             // Expansion 33 — storm forecast observation-post drift and drill recency.
00164:             _campaignDay.Register("storm_forecast", new StormForecastDayOwner(this), phase: 5);
00165:             // Expansion 35 — chemical dependency taper programs, withdrawal management, and care posture.
00166:             _campaignDay.Register("dependency_taper_withdrawal", new DependencyTaperWithdrawalDayOwner(this), phase: 5);
00167:             // Expansion 37 — antenatal maternal care, trimester progressions, and neonatal deliveries.
00168:             _campaignDay.Register("antenatal_maternal_health", new AntenatalMaternalHealthDayOwner(this), phase: 5);
00169:             // Expansion 38 — clinical ward triage priority, surgical suite readiness, and sterile supply inventory.
00170:             _campaignDay.Register("clinical_ward_triage", new ClinicalWardTriageDayOwner(this), phase: 5);
00171:             // Expansion 39 — chemical synthesis reactor safety, catalyst purity, and reagent grading.
00172:             _campaignDay.Register("chemical_reagent_synthesis", new ChemicalReagentSynthesisDayOwner(this), phase: 5);
00173:             // Expansion 40 — mechanical power driveline, line shafts, and machine tools.
00174:             _campaignDay.Register("mechanical_driveline", new MechanicalDrivelineDayOwner(this), phase: 5);
00175:             // Expansion 41 — sleep quality, soundproofing, and shelter crowding.
00176:             _campaignDay.Register("sleep_acoustic_rest", new SleepAcousticRestDayOwner(this), phase: 5);
00177:             // Plan 162 — shelter history & archive: institutional memory and milestones.
00178:             _campaignDay.Register("shelter_archive", new ShelterArchiveDayOwner(this), phase: 5);
00179:             // Plan 177 — survivor dream & sleep event system: nocturnal dream generation.
00180:             _campaignDay.Register("survivor_dreams", new SurvivorDreamsDayOwner(this), phase: 5);
00181:             // Plan 185 — memory & knowledge decay across cognition, skills, and relationships.
00182:             _campaignDay.Register("memory_decay", new MemoryDecayDayOwner(this), phase: 5);
00183:             // Plan 200 — survivor personal quests and character arcs.
00184:             _campaignDay.Register("personal_quests", new PersonalQuestsDayOwner(this), phase: 5);
00185:             // Plan 202 — interpersonal conflict, grievance accumulation, and mediation resolution.
00186:             _campaignDay.Register("interpersonal_conflict", new InterpersonalConflictDayOwner(this), phase: 5);
00187:             // Plan 216 — survivor exercise routines, physical training adaptation, and conditioning decay.
00188:             _campaignDay.Register("exercise", new ExerciseDayOwner(this), phase: 5);
00189:             // Plan 178 — art and culture creation: survivor artworks, masterworks, cultural identity, and display morale bonus.
00190:             _campaignDay.Register("culture_creation", new CultureCreationDayOwner(this), phase: 5);
00191:             // Plan 179 — unified psychology and phobia profiles: phobias, coping mechanisms, resilience, and therapy.
00192:             _campaignDay.Register("psychological_profiles", new PsychologicalProfilesDayOwner(this), phase: 5);
00193:             // Plan 180 — skill certification and tier system: formal qualifications, exams, benefits, and specializations.
00194:             _campaignDay.Register("skill_certifications", new SkillCertificationsDayOwner(this), phase: 5);
00195:             // Plan 187 — bestiary knowledge and creature encounters tracking.
00196:             _campaignDay.Register("bestiary_knowledge", new BestiaryDayOwner(this), phase: 5);
00197:             // Plan 198 — survivor medical records, longitudinal history, and vaccinations.
00198:             _campaignDay.Register("health_history", new HealthHistoryDayOwner(this), phase: 5);
00199:             // Plan 183 — child development stages, milestones, education, and chore capacity.
00200:             _campaignDay.Register("child_development_stages", new ChildDevelopmentDayOwner(this), phase: 5);
00201:             // Plan 186 — shelter maintenance & degradation: applies daily component wear and environmental stress.
00202:
00203:             _campaignDay.Register("shelter_maintenance", new ShelterMaintenanceDayOwner(this), phase: 5);
00204:             // Plan 188 — individual survivor daily routines: ticks satisfaction and detects schedule conflicts.
00205:             _campaignDay.Register("survivor_routines", new SurvivorRoutinesDayOwner(this), phase: 5);
00206:             // Plan 55 — retention runs last of all: it bounds the campaign logs
00207:             // every other owner just appended to for this day.
00208:             _campaignDay.Register("retention", new RetentionDayOwner(this), phase: 5);
00209:             // Flagship institutions (Tasks 5-8): culture, diplomacy, sky defense, sanatorium.
00210:             RegisterFlagshipInstitutionsOwner();
00211:         }
00212:
00213:         /// <summary>Plan 134 territory-control day owner (ownerId <c>territory_control</c>, phase 5).</summary>
00214:         private sealed class TerritoryControlDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00215:         {
00216:             private readonly Main _m;
00217:             private Ashfall.Core.Factions.TerritoryControlSaveState? _snapshot;
00218:             public TerritoryControlDayOwner(Main m) => _m = m;
00219:
00220:             public void CapturePreDaySnapshot(int day)
00221:             {
00222:                 _m.EnsureTerritoryControl();
00225:
00226:             public void RestorePreDaySnapshot(int day)
00227:             {
00228:                 if (_snapshot != null) _m.TerritoryControl?.System.RestoreState(_snapshot);
00230:
00231:             public void TickDay(int day, List<DayStateChangeEvent> events)
00232:             {
00233:                 _m.EnsureTerritoryControl();
00241:         /// <summary>Plan 136 cooking day owner (ownerId <c>cooking</c>, phase 5).</summary>
00242:         private sealed class CookingDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00243:         {
00244:             private readonly Main _m;
00245:             private Ashfall.Core.Cooking.CookingState? _snapshot;
00246:             public CookingDayOwner(Main m) => _m = m;
00247:
00248:             public void CapturePreDaySnapshot(int day)
00249:             {
00250:                 _m.EnsureCooking();
00253:
00254:             public void RestorePreDaySnapshot(int day)
00255:             {
00256:                 if (_snapshot != null) _m.Cooking?.System.RestoreState(_snapshot);
00258:
00259:             public void TickDay(int day, List<DayStateChangeEvent> events)
00260:             {
00261:                 _m.EnsureCooking();
00269:         /// <summary>Plan 135 weather-cascade day owner (ownerId <c>weather_cascade</c>, phase 5).</summary>
00270:         private sealed class WeatherCascadeDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00271:         {
00272:             private readonly Main _m;
00273:             private Ashfall.Core.Weather.WeatherCascadeState? _snapshot;
00274:             public WeatherCascadeDayOwner(Main m) => _m = m;
00275:
00276:             public void CapturePreDaySnapshot(int day)
00277:             {
00278:                 _m.EnsureWeatherCascade();
00281:
00282:             public void RestorePreDaySnapshot(int day)
00283:             {
00284:                 if (_snapshot != null) _m.WeatherCascade?.System.RestoreState(_snapshot);
00286:
00287:             public void TickDay(int day, List<DayStateChangeEvent> events)
00288:             {
00289:                 int activeBefore = _m.WeatherCascade?.System.State.activeEvents.Count ?? 0;
00298:         /// <summary>Plan 58 outpost day owner (ownerId <c>outpost_settlement</c>, phase 5).</summary>
00299:         private sealed class OutpostSettlementDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00300:         {
00301:             private readonly Main _m;
00302:             private Ashfall.Core.Settlements.OutpostSettlementState? _snapshot;
00303:             public OutpostSettlementDayOwner(Main m) => _m = m;
00304:             public void CapturePreDaySnapshot(int day)
00305:             {
00306:                 _m.EnsureOutpostSettlement();
00308:             }
00309:             public void RestorePreDaySnapshot(int day)
00310:             {
00311:                 if (_snapshot != null) _m.OutpostSettlement?.RestoreState(_snapshot);
00312:             }
00313:             public void TickDay(int day, List<DayStateChangeEvent> events)
00314:             {
00315:                 _m.EnsureOutpostSettlement();
00324:         /// <summary>Plan 55 retention day owner (ownerId <c>retention</c>, phase 5).</summary>
00325:         private sealed class RetentionDayOwner : IDayAdvanceOwner
00326:         {
00327:             private readonly Main _m;
00328:             public RetentionDayOwner(Main m) => _m = m;
00329:             public void CapturePreDaySnapshot(int day) { /* retention is idempotent; captured via save section */ }
00330:             public void TickDay(int day, List<DayStateChangeEvent> events)
00331:             {
00332:                 _m.EnsureRetention();
00341:         /// <summary>Plan 137 needs-performance day owner (ownerId <c>needs_performance</c>, phase 5).</summary>
00342:         private sealed class NeedsPerformanceDayOwner : IDayAdvanceOwner
00343:         {
00344:             private readonly Main _m;
00345:             public NeedsPerformanceDayOwner(Main m) => _m = m;
00346:             public void CapturePreDaySnapshot(int day) { /* derived read projection */ }
00347:             public void TickDay(int day, List<DayStateChangeEvent> events)
00348:             {
00349:                 _m.EnsureNeedsPerformance();
00357:         /// <summary>Plan 140 campaign-legacy day owner (ownerId <c>campaign_legacy</c>, phase 5).</summary>
00358:         private sealed class CampaignLegacyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00359:         {
00360:             private readonly Main _m;
00361:             private Ashfall.Core.Legacy.CampaignLegacyState? _snapshot;
00362:             public CampaignLegacyDayOwner(Main m) => _m = m;
00363:
00364:             public void CapturePreDaySnapshot(int day)
00365:             {
00366:                 _m.EnsureCampaignLegacy();
00369:
00370:             public void RestorePreDaySnapshot(int day)
00371:             {
00372:                 if (_snapshot != null) _m._campaignLegacy?.System.RestoreState(_snapshot);
00374:
00375:             public void TickDay(int day, List<DayStateChangeEvent> events)
00376:             {
00377:                 _m.EnsureCampaignLegacy();
00385:         /// <summary>Plan 141 research-unlock day owner (ownerId <c>research_unlock</c>, phase 5).</summary>
00386:         private sealed class ResearchUnlockDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00387:         {
00388:             private readonly Main _m;
00389:             private Ashfall.Core.Research.ResearchUnlockState? _snapshot;
00390:             public ResearchUnlockDayOwner(Main m) => _m = m;
00391:
00392:             public void CapturePreDaySnapshot(int day)
00393:             {
00394:                 _m.SetupResearchUnlockBridge();
00397:
00398:             public void RestorePreDaySnapshot(int day)
00399:             {
00400:                 if (_snapshot != null) _m._researchUnlock?.RestoreState(_snapshot);
00402:
00403:             public void TickDay(int day, List<DayStateChangeEvent> events)
00404:             {
00405:                 _m.SetupResearchUnlockBridge();
00413:         /// <summary>Plan 145 unified-ending day owner (ownerId <c>unified_ending</c>, phase 5).</summary>
00414:         private sealed class UnifiedEndingDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00415:         {
00416:             private readonly Main _m;
00417:             private Ashfall.Core.Endgame.UnifiedEndingSaveState? _snapshot;
00418:             public UnifiedEndingDayOwner(Main m) => _m = m;
00419:
00420:             public void CapturePreDaySnapshot(int day)
00421:             {
00422:                 _m.SetupUnifiedEnding();
00425:
00426:             public void RestorePreDaySnapshot(int day)
00427:             {
00428:                 if (_snapshot != null) _m._unifiedEnding?.RestoreState(_snapshot);
00430:
00431:             public void TickDay(int day, List<DayStateChangeEvent> events)
00432:             {
00433:                 _m.SetupUnifiedEnding();
00441:         /// <summary>Plan 147 per-NPC memory day owner (ownerId <c>npc_memory</c>, phase 5).</summary>
00442:         private sealed class NpcMemoryDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00443:         {
00444:             private readonly Main _m;
00445:             private Ashfall.Core.Narrative.NpcMemorySaveState? _snapshot;
00446:             public NpcMemoryDayOwner(Main m) => _m = m;
00447:
00448:             public void CapturePreDaySnapshot(int day)
00449:             {
00450:                 _m.SetupNpcMemory();
00453:
00454:             public void RestorePreDaySnapshot(int day)
00455:             {
00456:                 if (_snapshot != null) _m._npcMemory?.RestoreState(_snapshot);
00458:
00459:             public void TickDay(int day, List<DayStateChangeEvent> events)
00460:             {
00461:                 _m.SetupNpcMemory();
00469:         /// <summary>Plan 148 ideological friction day owner (ownerId <c>ideological_friction</c>, phase 5).</summary>
00470:         private sealed class IdeologicalFrictionDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00471:         {
00472:             private readonly Main _m;
00473:             private Ashfall.Core.Survivors.IdeologicalFrictionEventSaveState? _snapshot;
00474:             public IdeologicalFrictionDayOwner(Main m) => _m = m;
00475:
00476:             public void CapturePreDaySnapshot(int day)
00477:             {
00478:                 _m.SetupIdeologicalFriction();
00481:
00482:             public void RestorePreDaySnapshot(int day)
00483:             {
00484:                 if (_snapshot != null) _m._ideologicalFriction?.RestoreState(_snapshot);
00486:
00487:             public void TickDay(int day, List<DayStateChangeEvent> events)
00488:             {
00489:                 _m.SetupIdeologicalFriction();
00497:         /// <summary>Plan 150 romance &amp; family day owner (ownerId <c>romance_family</c>, phase 5).</summary>
00498:         private sealed class RomanceFamilyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00499:         {
00500:             private readonly Main _m;
00501:             private string? _snapshot;
00502:             public RomanceFamilyDayOwner(Main m) => _m = m;
00503:
00504:             public void CapturePreDaySnapshot(int day)
00505:             {
00506:                 _m.SetupRomanceFamily();
00509:
00510:             public void RestorePreDaySnapshot(int day)
00511:             {
00512:                 if (_snapshot != null) _m._romanceFamily?.RestoreCoreState(_snapshot);
00514:
00515:             public void TickDay(int day, List<DayStateChangeEvent> events)
00516:             {
00517:                 _m.SetupRomanceFamily();
00525:         /// <summary>Plan 152 vehicle customization day owner (ownerId <c>vehicle_customization</c>, phase 5).</summary>
00526:         private sealed class VehicleCustomizationDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00527:         {
00528:             private readonly Main _m;
00529:             private string? _snapshot;
00530:             public VehicleCustomizationDayOwner(Main m) => _m = m;
00531:
00532:             public void CapturePreDaySnapshot(int day)
00533:             {
00534:                 _m.SetupVehicleCustomization();
00537:
00538:             public void RestorePreDaySnapshot(int day)
00539:             {
00540:                 if (_snapshot != null) _m._vehicleCustomization?.RestoreCoreState(_snapshot);
00542:
00543:             public void TickDay(int day, List<DayStateChangeEvent> events)
00544:             {
00545:                 _m.SetupVehicleCustomization();
00553:         /// <summary>Plan 174 procedural survivor backstory day owner (ownerId <c>backstory</c>, phase 5).</summary>
00554:         private sealed class BackstoryDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00555:         {
00556:             private readonly Main _m;
00557:             private Ashfall.Core.Survivors.BackstoryState? _snapshot;
00558:             public BackstoryDayOwner(Main m) => _m = m;
00559:
00560:             public void CapturePreDaySnapshot(int day)
00561:             {
00562:                 _m.SetupBackstory();
00565:
00566:             public void RestorePreDaySnapshot(int day)
00567:             {
00568:                 if (_snapshot != null) _m._backstory?.RestoreState(_snapshot);
00570:
00571:             public void TickDay(int day, List<DayStateChangeEvent> events)
00572:             {
00573:                 _m.SetupBackstory();
00581:         /// <summary>Plan 175 meta progression day owner (ownerId <c>meta_progression</c>, phase 5).</summary>
00582:         private sealed class MetaProgressionDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00583:         {
00584:             private readonly Main _m;
00585:             private Ashfall.Core.Endgame.MetaProgressionSaveState? _snapshot;
00586:             public MetaProgressionDayOwner(Main m) => _m = m;
00587:
00588:             public void CapturePreDaySnapshot(int day)
00589:             {
00590:                 _m.SetupMetaProgression();
00593:
00594:             public void RestorePreDaySnapshot(int day)
00595:             {
00596:                 if (_snapshot != null) _m._metaProgression?.RestoreState(_snapshot);
00598:
00599:             public void TickDay(int day, List<DayStateChangeEvent> events)
00600:             {
00601:                 _m.SetupMetaProgression();
00611:         /// <summary>Plan 167 underground tunnel network day owner (ownerId <c>tunnel_network</c>, phase 5).</summary>
00612:         private sealed class TunnelNetworkDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00613:         {
00614:             private readonly Main _m;
00615:             private Ashfall.Core.Underground.TunnelNetworkState? _snapshot;
00616:             public TunnelNetworkDayOwner(Main m) => _m = m;
00617:
00618:             public void CapturePreDaySnapshot(int day)
00619:             {
00620:                 _m.SetupTunnelNetwork();
00623:
00624:             public void RestorePreDaySnapshot(int day)
00625:             {
00626:                 if (_snapshot != null) _m.TunnelNetwork?.RestoreState(_snapshot);
00628:
00629:             public void TickDay(int day, List<DayStateChangeEvent> events)
00630:             {
00631:                 _m.SetupTunnelNetwork();
00639:         /// <summary>Plan 166 shelter identity day owner (ownerId <c>shelter_identity</c>, phase 5).</summary>
00640:         private sealed class ShelterIdentityDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00641:         {
00642:             private readonly Main _m;
00643:             private Ashfall.Core.Shelter.ShelterIdentityState? _snapshot;
00644:             public ShelterIdentityDayOwner(Main m) => _m = m;
00645:
00646:             public void CapturePreDaySnapshot(int day)
00647:             {
00648:                 _m.SetupShelterIdentity();
00651:
00652:             public void RestorePreDaySnapshot(int day)
00653:             {
00654:                 if (_snapshot != null) _m.ShelterIdentity?.RestoreState(_snapshot);
00656:
00657:             public void TickDay(int day, List<DayStateChangeEvent> events)
00658:             {
00659:                 _m.SetupShelterIdentity();
00667:         /// <summary>Plan 192 scheduled trade route contract day owner (ownerId <c>trade_routes</c>, phase 5).</summary>
00668:         private sealed class TradeRouteDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00669:         {
00670:             private readonly Main _m;
00671:             private Ashfall.Core.Economy.PlayerTradeRouteSaveState? _snapshot;
00672:             public TradeRouteDayOwner(Main m) => _m = m;
00673:
00674:             public void CapturePreDaySnapshot(int day)
00675:             {
00676:                 _m.SetupTradeRoutes();
00679:
00680:             public void RestorePreDaySnapshot(int day)
00681:             {
00682:                 if (_snapshot != null) _m._tradeRoutes?.RestoreState(_snapshot);
00684:
00685:             public void TickDay(int day, List<DayStateChangeEvent> events)
00686:             {
00687:                 _m.SetupTradeRoutes();
00695:         /// <summary>Plan 199 seasonal human migration day owner (ownerId <c>human_migration</c>, phase 5).</summary>
00696:         private sealed class HumanMigrationDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00697:         {
00698:             private readonly Main _m;
00699:             private Ashfall.Core.Economy.SeasonalMigrationSaveState? _snapshot;
00700:             public HumanMigrationDayOwner(Main m) => _m = m;
00701:
00702:             public void CapturePreDaySnapshot(int day)
00703:             {
00704:                 _m.SetupHumanMigration();
00707:
00708:             public void RestorePreDaySnapshot(int day)
00709:             {
00710:                 if (_snapshot != null) _m._humanMigration?.RestoreState(_snapshot);
00712:
00713:             public void TickDay(int day, List<DayStateChangeEvent> events)
00714:             {
00715:                 _m.SetupHumanMigration();
00723:         /// <summary>Plan 159 shelter governance day owner (ownerId <c>shelter_governance</c>, phase 5).</summary>
00724:         private sealed class ShelterGovernanceDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00725:         {
00726:             private readonly Main _m;
00727:             private Ashfall.Core.Governance.ShelterGovernanceSaveState? _snapshot;
00728:             public ShelterGovernanceDayOwner(Main m) => _m = m;
00729:
00730:             public void CapturePreDaySnapshot(int day)
00731:             {
00732:                 _m.SetupShelterGovernance();
00735:
00736:             public void RestorePreDaySnapshot(int day)
00737:             {
00738:                 if (_snapshot != null) _m._shelterGovernance?.RestoreState(_snapshot);
00740:
00741:             public void TickDay(int day, List<DayStateChangeEvent> events)
00742:             {
00743:                 _m.SetupShelterGovernance();
00751:         /// <summary>Plan 176 aging & elderly survivor day owner (ownerId <c>aging</c>, phase 5).</summary>
00752:         private sealed class AgingDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00753:         {
00754:             private readonly Main _m;
00755:             private Ashfall.Core.Survivors.AgingState? _snapshot;
00756:             public AgingDayOwner(Main m) => _m = m;
00757:
00758:             public void CapturePreDaySnapshot(int day)
00759:             {
00760:                 _m.SetupAging();
00763:
00764:             public void RestorePreDaySnapshot(int day)
00765:             {
00766:                 if (_snapshot != null) _m._aging?.RestoreState(_snapshot);
00768:
00769:             public void TickDay(int day, List<DayStateChangeEvent> events)
00770:             {
00771:                 _m.SetupAging();
00779:         /// <summary>Plan 186 shelter maintenance day owner (ownerId <c>shelter_maintenance</c>, phase 5).</summary>
00780:         private sealed class ShelterMaintenanceDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00781:         {
00782:             private readonly Main _m;
00783:             private Ashfall.Core.Shelter.ShelterMaintenanceState? _snapshot;
00784:             public ShelterMaintenanceDayOwner(Main m) => _m = m;
00785:
00786:             public void CapturePreDaySnapshot(int day)
00787:             {
00788:                 _m.SetupShelterMaintenance();
00791:
00792:             public void RestorePreDaySnapshot(int day)
00793:             {
00794:                 if (_snapshot != null) _m._shelterMaintenance?.RestoreState(_snapshot);
00796:
00797:             public void TickDay(int day, List<DayStateChangeEvent> events)
00798:             {
00799:                 _m.SetupShelterMaintenance();
00807:         /// <summary>Plan 188 survivor routines day owner (ownerId <c>survivor_routines</c>, phase 5).</summary>
00808:         private sealed class SurvivorRoutinesDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00809:         {
00810:             private readonly Main _m;
00811:             private Ashfall.Core.Survivors.SurvivorRoutineState? _snapshot;
00812:             public SurvivorRoutinesDayOwner(Main m) => _m = m;
00813:
00814:             public void CapturePreDaySnapshot(int day)
00815:             {
00816:                 _m.SetupSurvivorRoutines();
00819:
00820:             public void RestorePreDaySnapshot(int day)
00821:             {
00822:                 if (_snapshot != null) _m._survivorRoutines?.RestoreState(_snapshot);
00824:
00825:             public void TickDay(int day, List<DayStateChangeEvent> events)
00826:             {
00827:                 _m.SetupSurvivorRoutines();
00836:
00837:         private sealed class WeatherWorldDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00838:         {
00839:             private readonly Main _m;
00840:             private Ashfall.Core.World.WorldWeatherState? _snapshot;
00841:             public WeatherWorldDayOwner(Main m) => _m = m;
00842:             public void CapturePreDaySnapshot(int day)
00843:             {
00844:                 _m.SetupWorld();
00846:             }
00847:             public void RestorePreDaySnapshot(int day)
00848:             {
00849:                 if (_snapshot != null)
00851:             }
00852:             public void TickDay(int day, List<DayStateChangeEvent> events)
00853:             {
00854:                 _m.SetupWorld();
00887:
00888:         private sealed class DeepCoastMaritimeDayOwner : IDayAdvanceOwner
00889:         {
00890:             private readonly Main _m;
00891:             public DeepCoastMaritimeDayOwner(Main m) => _m = m;
00892:             public void CapturePreDaySnapshot(int day) { }
00893:             public void TickDay(int day, List<DayStateChangeEvent> events)
00894:             {
00895:                 _m.SetupMaritime();
00904:
00905:         private sealed class PowerGridDayOwner : IDayAdvanceOwner
00906:         {
00907:             private readonly Main _m;
00908:             public PowerGridDayOwner(Main m) => _m = m;
00909:             public void CapturePreDaySnapshot(int day) { }
00910:             public void TickDay(int day, List<DayStateChangeEvent> events)
00911:             {
00912:                 _m.TickPowerGrid(day);
00951:
00952:         private sealed class NuclearCoreDayOwner : IDayAdvanceOwner
00953:         {
00954:             private readonly Main _m;
00955:             public NuclearCoreDayOwner(Main m) => _m = m;
00956:             public void CapturePreDaySnapshot(int day) { }
00957:             public void TickDay(int day, List<DayStateChangeEvent> events)
00958:             {
00959:                 var nuclear = _m.EnsureNuclearCore();
00969:
00970:         private sealed class HoldfastCoreDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00971:         {
00972:             private readonly Main _m;
00973:             private int _clockDaySnapshot;
00974:             public HoldfastCoreDayOwner(Main m) => _m = m;
00975:             public void CapturePreDaySnapshot(int day)
00976:             {
00977:                 // The clock is the double-day hazard: if a later phase fails,
00980:             }
00981:             public void RestorePreDaySnapshot(int day)
00982:             {
00983:                 _m._core.Clock.SetDay(_clockDaySnapshot);
00984:             }
00985:             public void TickDay(int day, List<DayStateChangeEvent> events)
00986:             {
00987:                 _m.SetupIceRoad();
00995:                 // land exactly on the campaign day being committed, whatever
00996:                 // its internal tick state said.
00997:                 _m._core.Clock.SetDay(day);
00998:                 events.Add(new DayStateChangeEvent("holdfast_ticked", "holdfast_core", delta, null, _m._core.Clock.Day));
01003:
01004:         private sealed class StartingLevelRationsDayOwner : IDayAdvanceOwner
01005:         {
01006:             private readonly Main _m;
01007:             public StartingLevelRationsDayOwner(Main m) => _m = m;
01008:             public void CapturePreDaySnapshot(int day) { }
01009:             public void TickDay(int day, List<DayStateChangeEvent> events)
01010:             {
01011:                 _m.SetupStartingLevel();
01036:
01037:         private sealed class CraftingProductionDayOwner : IDayAdvanceOwner
01038:         {
01039:             private readonly Main _m;
01040:             public CraftingProductionDayOwner(Main m) => _m = m;
01041:             public void CapturePreDaySnapshot(int day) { }
01042:             public void TickDay(int day, List<DayStateChangeEvent> events)
01043:             {
01044:                 _m.SetupCrafting();
01049:
01050:         private sealed class GreenhouseFoundryDayOwner : IDayAdvanceOwner
01051:         {
01052:             private readonly Main _m;
01053:             public GreenhouseFoundryDayOwner(Main m) => _m = m;
01054:             public void CapturePreDaySnapshot(int day) { }
01055:             public void TickDay(int day, List<DayStateChangeEvent> events)
01056:             {
01057:                 // Single growth authority: player GreenhouseHostSession (shared
01088:         /// </summary>
01089:         private sealed class SeismicGeologyDayOwner : IDayAdvanceOwner
01090:         {
01091:             private readonly Main _m;
01092:             public SeismicGeologyDayOwner(Main m) => _m = m;
01093:             public void CapturePreDaySnapshot(int day) { }
01094:             public void TickDay(int day, List<DayStateChangeEvent> events)
01095:             {
01096:                 _m.SetupSeismicDynamics();
01107:         /// </summary>
01108:         private sealed class CryoVaultDayOwner : IDayAdvanceOwner
01109:         {
01110:             private readonly Main _m;
01111:             public CryoVaultDayOwner(Main m) => _m = m;
01112:             public void CapturePreDaySnapshot(int day) { }
01113:             public void TickDay(int day, List<DayStateChangeEvent> events)
01114:             {
01115:                 _m.SetupCryoVault();
01126:         /// </summary>
01127:         private sealed class PrecisionMetrologyDayOwner : IDayAdvanceOwner
01128:         {
01129:             private readonly Main _m;
01130:             public PrecisionMetrologyDayOwner(Main m) => _m = m;
01131:             public void CapturePreDaySnapshot(int day) { }
01132:             public void TickDay(int day, List<DayStateChangeEvent> events)
01133:             {
01134:                 _m.TickPrecisionMetrology(day);
01142:         /// </summary>
01143:         private sealed class AquaponicsDayOwner : IDayAdvanceOwner
01144:         {
01145:             private readonly Main _m;
01146:             public AquaponicsDayOwner(Main m) => _m = m;
01147:             public void CapturePreDaySnapshot(int day) { }
01148:             public void TickDay(int day, List<DayStateChangeEvent> events)
01149:             {
01150:                 _m.TickAquaponics(day);
01154:
01155:         private sealed class PsychologyArcsDayOwner : IDayAdvanceOwner
01156:         {
01157:             private readonly Main _m;
01158:             public PsychologyArcsDayOwner(Main m) => _m = m;
01159:             public void CapturePreDaySnapshot(int day) { }
01160:             public void TickDay(int day, List<DayStateChangeEvent> events)
01161:             {
01162:                 _m.TickPsychologyArcsDay(day);
01166:
01167:         private sealed class EconomyMarketDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01168:         {
01169:             private readonly Main _m;
01170:             private Ashfall.Core.Economy.MarketState? _snapshot;
01171:             public EconomyMarketDayOwner(Main m) => _m = m;
01172:             public void CapturePreDaySnapshot(int day)
01173:             {
01174:                 _m.SetupEconomy();
01176:             }
01177:             public void RestorePreDaySnapshot(int day)
01178:             {
01179:                 if (_snapshot != null)
01181:             }
01182:             public void TickDay(int day, List<DayStateChangeEvent> events)
01183:             {
01184:                 _m.SetupEconomy();
01195:
01196:         private sealed class ShelterFacilitiesDayOwner : IDayAdvanceOwner
01197:         {
01198:             private readonly Main _m;
01199:             public ShelterFacilitiesDayOwner(Main m) => _m = m;
01200:             public void CapturePreDaySnapshot(int day) { }
01201:             public void TickDay(int day, List<DayStateChangeEvent> events)
01202:             {
01203:                 // C2 / Plan 20B (§29) — capture pre-tick shelter shielding state
01239:             /// <summary>Ordering for filter condition bands (higher = healthier).</summary>
01240:             private static int FilterBandRank(string? band) => band switch
01241:             {
01242:                 "healthy" => 2,
01254:         /// </summary>
01255:         private sealed class CommitmentDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01256:         {
01257:             private readonly Main _m;
01258:             public CommitmentDayOwner(Main m) => _m = m;
01259:
01260:             public void CapturePreDaySnapshot(int day) => _m._commitments?.System.CapturePreDaySnapshot(day);
01261:
01262:             public void RestorePreDaySnapshot(int day) => _m._commitments?.System.RestorePreDaySnapshot(day);
01263:
01264:             public void TickDay(int day, List<DayStateChangeEvent> events)
01265:             {
01266:                 var session = _m.EnsureCommitments();
01272:
01273:         private sealed class ShelterFireDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01274:         {
01275:             private readonly Main _m;
01276:             private Dictionary<string, Ashfall.Core.Shelter.FireIncidentState>? _snapshot;
01277:
01278:             public ShelterFireDayOwner(Main m) => _m = m;
01279:
01280:             public void CapturePreDaySnapshot(int day)
01281:             {
01282:                 _m.SetupShelterFireHazard();
01285:
01286:             public void RestorePreDaySnapshot(int day)
01287:             {
01288:                 if (_snapshot != null)
01294:
01295:             public void TickDay(int day, List<DayStateChangeEvent> events)
01296:             {
01297:                 _m.SetupShelterFireHazard();
01306:
01307:         private sealed class SurvivorsNeedsDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01308:         {
01309:             private readonly Main _m;
01310:             private SurvivorsSaveState? _snapshot;
01311:             public SurvivorsNeedsDayOwner(Main m) => _m = m;
01312:             public void CapturePreDaySnapshot(int day)
01313:             {
01314:                 _m.SetupSurvivors();
01316:             }
01317:             public void RestorePreDaySnapshot(int day)
01318:             {
01319:                 if (_snapshot != null)
01321:             }
01322:             public void TickDay(int day, List<DayStateChangeEvent> events)
01323:             {
01324:                 _m.SetupSurvivors();
01377:         /// </summary>
01378:         private sealed class HygieneDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01379:         {
01380:             private readonly Main _m;
01381:             private Ashfall.Core.Shelter.SanitationState? _snapshot;
01382:             public HygieneDayOwner(Main m) => _m = m;
01383:             public void CapturePreDaySnapshot(int day)
01384:             {
01385:                 _m.SetupSanitation();
01387:             }
01388:             public void RestorePreDaySnapshot(int day)
01389:             {
01390:                 if (_snapshot != null)
01392:             }
01393:             public void TickDay(int day, List<DayStateChangeEvent> events)
01394:             {
01395:                 _m.SetupSanitation();
01420:             /// </summary>
01421:             private void TickSanitationConsequences(int day, List<DayStateChangeEvent> events)
01422:             {
01423:                 var system = _m._sanitation!.System;
01476:
01477:         private sealed class MedicalDiseaseDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01478:         {
01479:             private readonly Main _m;
01480:             private static readonly Ashfall.Core.SystemTextJsonSerializer s_json = new Ashfall.Core.SystemTextJsonSerializer();
01481:             private Ashfall.Core.Medical.ChemicalDependencyLedgerState? _medicalSnapshot;
01482:             private string? _diseaseSnapshotJson;
01483:             private Ashfall.Core.Medical.MedicalPipelineSaveState? _pipelineSnapshot;
01484:             public MedicalDiseaseDayOwner(Main m) => _m = m;
01485:             public void CapturePreDaySnapshot(int day)
01486:             {
01487:                 _m.SetupMedical();
01497:             }
01498:             public void RestorePreDaySnapshot(int day)
01499:             {
01500:                 if (_medicalSnapshot != null)
01510:             }
01511:             public void TickDay(int day, List<DayStateChangeEvent> events)
01512:             {
01513:                 _m.SetupMedical();
01560:
01561:         private sealed class DutyRosterDayOwner : IDayAdvanceOwner
01562:         {
01563:             private readonly Main _m;
01564:             public DutyRosterDayOwner(Main m) => _m = m;
01565:             public void CapturePreDaySnapshot(int day) { }
01566:             public void TickDay(int day, List<DayStateChangeEvent> events)
01567:             {
01568:                 _m.SetupDutyRoster();
01580:
01581:         private sealed class SurvivorSocialDayOwner : IDayAdvanceOwner
01582:         {
01583:             private readonly Main _m;
01584:             public SurvivorSocialDayOwner(Main m) => _m = m;
01585:             public void CapturePreDaySnapshot(int day) { }
01586:             public void TickDay(int day, List<DayStateChangeEvent> events)
01587:             {
01588:                 _m.TickSurvivorSocial(day);
01592:
01593:         private sealed class Phase0PsychologyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01594:         {
01595:             private readonly Main _m;
01596:             private Phase0EffectsSaveState? _snapshot;
01597:             public Phase0PsychologyDayOwner(Main m) => _m = m;
01598:             // Phase0EffectsSaveState is built fresh by CaptureSave (deep-copied
01599:             // sub-states), so holding it directly is a true independent snapshot.
01600:             public void CapturePreDaySnapshot(int day)
01601:             {
01602:                 _m.SetupPhase0();
01604:             }
01605:             public void RestorePreDaySnapshot(int day)
01606:             {
01607:                 if (_snapshot != null)
01609:             }
01610:             public void TickDay(int day, List<DayStateChangeEvent> events)
01611:             {
01612:                 _m.SetupPhase0();
01623:
01624:         private sealed class ExpeditionsCaravansDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01625:         {
01626:             private readonly Main _m;
01627:             private ExpeditionAggregateState? _expeditionSnapshot;
01628:             private TravelingCaravanState? _caravanSnapshot;
01629:             private VehicleGarageState? _garageSnapshot;
01630:             public ExpeditionsCaravansDayOwner(Main m) => _m = m;
01631:             public void CapturePreDaySnapshot(int day)
01632:             {
01633:                 _m.SetupExpeditions();
01640:             }
01641:             public void RestorePreDaySnapshot(int day)
01642:             {
01643:                 if (_expeditionSnapshot != null)
01649:             }
01650:             public void TickDay(int day, List<DayStateChangeEvent> events)
01651:             {
01652:                 _m.SetupExpeditions();
01706:         /// </summary>
01707:         private sealed class PsyOpsDayOwner : IDayAdvanceOwner
01708:         {
01709:             private readonly Main _m;
01710:             public PsyOpsDayOwner(Main m) => _m = m;
01711:             public void CapturePreDaySnapshot(int day) { /* no snapshot: campaign days are day-local */ }
01712:             public void TickDay(int day, List<DayStateChangeEvent> events)
01713:             {
01714:                 _m.SetupPsyOps();
01731:         /// </summary>
01732:         private sealed class RadioProgramProductionDayOwner : IDayAdvanceOwner
01733:         {
01734:             private readonly Main _m;
01735:             public RadioProgramProductionDayOwner(Main m) => _m = m;
01736:             public void CapturePreDaySnapshot(int day) { /* jobs are day-local; capture via save section */ }
01737:             public void TickDay(int day, List<DayStateChangeEvent> events)
01738:             {
01739:                 int before = _m._radioProgramProduction?.System.GetActiveJobs().Count ?? 0;
01761:         /// </summary>
01762:         private sealed class UnderworldMarketDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01763:         {
01764:             private readonly Main _m;
01765:             private Ashfall.Core.Economy.BlackMarketState? _snapshot;
01766:             public UnderworldMarketDayOwner(Main m) => _m = m;
01767:             public void CapturePreDaySnapshot(int day)
01768:             {
01769:                 _m.SetupBlackMarket();
01771:             }
01772:             public void RestorePreDaySnapshot(int day)
01773:             {
01774:                 if (_snapshot != null)
01776:             }
01777:             public void TickDay(int day, List<DayStateChangeEvent> events)
01778:             {
01779:                 _m.SetupBlackMarket();
01787:
01788:         private sealed class SubterraneanDayOwner : IDayAdvanceOwner
01789:         {
01790:             private readonly Main _m;
01791:             public SubterraneanDayOwner(Main m) => _m = m;
01792:             public void CapturePreDaySnapshot(int day) { /* no snapshot: hazards are day-local */ }
01793:             public void TickDay(int day, List<DayStateChangeEvent> events)
01794:             {
01795:                 _m.SetupSubterranean();
01823:         /// </summary>
01824:         private sealed class EvolvingWorldDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01825:         {
01826:             private readonly Main _m;
01827:             private LocationEvolutionSaveState? _locSnapshot;
01828:             private WildlifeSaveState? _wildSnapshot;
01829:             private LandmarkSaveState? _landSnapshot;
01830:             private readonly HashSet<string> _processedExpeditions = new HashSet<string>();
01831:             private string _lastDominantFaction = string.Empty;
01832:
01833:             public EvolvingWorldDayOwner(Main m) => _m = m;
01834:
01835:             public void CapturePreDaySnapshot(int day)
01836:             {
01837:                 _m.SetupWorld();
01842:
01843:             public void RestorePreDaySnapshot(int day)
01844:             {
01845:                 if (_locSnapshot != null) _m._world.LocationEvolution?.RestoreState(_locSnapshot);
01850:
01851:             public void TickDay(int day, List<DayStateChangeEvent> events)
01852:             {
01853:                 _m.SetupWorld();
01997:                         var origins = new List<string>();
01998:                         foreach (var cd in CaravanCatalogLoader.Load(_m._dataDir))
01999:                             if (!string.IsNullOrEmpty(cd.origin_region) && !origins.Contains(cd.origin_region))
02000:                                 origins.Add(cd.origin_region);
02104:             /// <summary>Collapse notices re-arm after this many days (anti-spam).</summary>
02105:             private const int CollapseNoticeCooldownDays = 12;
02106:             private int _lastCollapseNoticeDay = -30;
02107:
02108:             /// <summary>
02113:             /// </summary>
02114:             private static string? SectorOfLocation(WorldHostSession world, string locationId)
... [line 2115 onward omitted from bounded excerpt] ...
```

## `src/UI/MoralChoiceModal.cs` — 317 lines; 14,077 bytes; SHA-256 `ef9e9b6e34bb952eb6ac37133cccd919dfc0ef4135b4831ff0085ff6662da334`
Declaration index:
- 00018: public partial class MoralChoiceModal : Control, IModalPanel
- 00046: private void BuildLayout()
- 00116: public void Bind(
- 00129: public void RefreshContent()
- 00254: private void ExecuteChoice(int choiceIndex)
- 00270: public void Open()
- 00277: public void SelectChoiceForTest(int choiceIndex) => ExecuteChoice(choiceIndex);
- 00279: public void CloseModal()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.MoralChoice;
00007: using Ashfall.Core.UI;
00008: using DesignTheme = Ashfall.Core.UI.Theme;
00009:
00010: namespace AtomicWar.GodotApp.UI
00011: {
00012:     /// <summary>
00013:     /// ASHFALL — Moral Choice Decision Modal ("The Weight of Survival").
00014:     /// Surfaces authored moral dilemmas, narrative encounters, and irrevocable tactical options
00015:     /// to the player without exposing underlying numeric moral/empathy metrics.
00016:     /// Implements <see cref="IModalPanel"/> for focus management and keyboard handling.
00017:     /// </summary>
00018:     public partial class MoralChoiceModal : Control, IModalPanel
00019:     {
00020:         public event Action<string, int>? OnChoiceSelected;
00021:         public event Action? OnClose;
00022:         public event Action? OnModalClosed;
00023:
00024:         public bool IsModalOpen => Visible;
00025:         public Control? InitialFocusControl => _firstInteractiveButton ?? _closeButton;
00026:
00027:         private Label _titleLabel = null!;
00028:         private Label _subtitleLabel = null!;
00029:         private VBoxContainer _encounterContainer = null!;
00030:         private VBoxContainer _choicesContainer = null!;
00031:         private VBoxContainer _feedbackContainer = null!;
00032:         private Button _closeButton = null!;
00033:         private Control? _firstInteractiveButton;
00034:
00035:         private MoralChoiceQuestDefinition? _currentQuest;
00036:         private MoralChoiceSystem? _moralChoiceSystem;
00037:         private Action<string, int>? _onChoiceCallback;
00038:
00039:         public override void _Ready()
00040:         {
00041:             SetAnchorsPreset(LayoutPreset.FullRect);
00042:             BuildLayout();
00043:             Visible = false;
00044:         }
00045:
00046:         private void BuildLayout()
00047:         {
00048:             AshfallUiHelpers.EmptyChildren(this);
00049:
00050:             // Dark semi-transparent scrim backdrop
00051:             var scrim = new ColorRect
00052:             {
00053:                 Color = new Color(0.02f, 0.02f, 0.04f, 0.88f)
00054:             };
00055:             scrim.SetAnchorsPreset(LayoutPreset.FullRect);
00056:             AddChild(scrim);
00057:
00058:             // Center dialog container (max width 1100, centered)
00059:             var center = new CenterContainer();
00060:             center.SetAnchorsPreset(LayoutPreset.FullRect);
00061:             AddChild(center);
00062:
00063:             var panelCard = AshfallUiHelpers.MakeCardFrame("THE WEIGHT OF SURVIVAL", "ETHICAL DIRECTIVE & TACTICAL CHOICE");
00064:             panelCard.CustomMinimumSize = new Vector2(1040, 680);
00065:             center.AddChild(panelCard);
00066:
00067:             var margin = panelCard.GetChild<MarginContainer>(0);
00068:             var mainVBox = margin.GetChild<VBoxContainer>(0);
00069:
00070:             // Title & category header
00071:             _titleLabel = AshfallUiHelpers.MakeTitle("MORAL DILEMMA // UNRESOLVED");
00072:             _titleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
00073:             mainVBox.AddChild(_titleLabel);
00074:
00075:             _subtitleLabel = AshfallUiHelpers.MakeSmall("CATEGORY: UNKNOWN · LOCATION: GENERAL SECTOR");
00076:             _subtitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
00077:             mainVBox.AddChild(_subtitleLabel);
00078:
00079:             mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());
00080:
00081:             // Scrollable central content
00082:             var scroll = new ScrollContainer
00083:             {
00084:                 CustomMinimumSize = new Vector2(980, 440),
00085:                 SizeFlagsVertical = SizeFlags.ExpandFill,
00086:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
00087:             };
00088:             mainVBox.AddChild(scroll);
00089:
00090:             var scrollContent = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
00091:             scrollContent.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00092:             scroll.AddChild(scrollContent);
00093:
00094:             _encounterContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
00095:             scrollContent.AddChild(_encounterContainer);
00096:
00097:             _choicesContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
00098:             scrollContent.AddChild(_choicesContainer);
00099:
00100:             _feedbackContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
00101:             scrollContent.AddChild(_feedbackContainer);
00102:
00103:             mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());
00104:
00105:             // Bottom bar with close/return
00106:             var bottomBar = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
00107:             bottomBar.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00108:
00109:             _closeButton = AshfallUiHelpers.MakeButton("RETURN TO OVERVIEW // [ESC]", () => CloseModal());
00110:             _closeButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00111:             bottomBar.AddChild(_closeButton);
00112:
00113:             mainVBox.AddChild(bottomBar);
00114:         }
00115:
00116:         public void Bind(
00117:             MoralChoiceQuestDefinition quest,
00118:             MoralChoiceSystem? moralChoiceSystem = null,
00119:             Action<string, int>? onChoiceCallback = null)
00120:         {
00121:             _currentQuest = quest ?? throw new ArgumentNullException(nameof(quest));
00122:             _moralChoiceSystem = moralChoiceSystem;
00123:             _onChoiceCallback = onChoiceCallback;
00124:             _firstInteractiveButton = null;
00125:
00126:             RefreshContent();
00127:         }
00128:
00129:         public void RefreshContent()
00130:         {
00131:             if (_currentQuest == null) return;
00132:
00133:             // The choice buttons are rebuilt on every refresh. Clear the
00134:             // cached focus target before freeing the old button tree.
00135:             _firstInteractiveButton = null;
00136:
00137:             bool isResolved = _moralChoiceSystem?.IsResolved(_currentQuest.Id) ?? false;
00138:             MoralChoiceResolution? resolution = null;
00139:             _moralChoiceSystem?.TryGetResolution(_currentQuest.Id, out resolution);
00140:
00141:             // Header titles
00142:             string statusTag = isResolved ? "RESOLVED & RECORDED" : "TACTICAL ACTION REQUIRED";
00143:             _titleLabel.Text = $"ETHICAL PROTOCOL // {_currentQuest.DisplayName.ToUpperInvariant()}";
00144:             _subtitleLabel.Text = $"CATEGORY: {_currentQuest.Category.ToUpperInvariant()} · STATUS: {statusTag} · LOCATION: {(string.IsNullOrEmpty(_currentQuest.LocationId) ? "SECTOR PERIMETER" : _currentQuest.LocationId)}";
00145:
00146:             // 1. Encounter / Narrative briefing
00147:             AshfallUiHelpers.EmptyChildren(_encounterContainer);
00148:             var encounterCard = AshfallUiHelpers.MakeCardFrame("FIELD ENCOUNTER DOSSIER", _currentQuest.Category.ToUpperInvariant());
00149:             var encBox = encounterCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00150:
00151:             if (!string.IsNullOrWhiteSpace(_currentQuest.Trigger))
00152:             {
00153:                 var trigLabel = AshfallUiHelpers.MakeBody($"► SITUATION: {_currentQuest.Trigger}");
00154:                 trigLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00155:                 encBox.AddChild(trigLabel);
00156:                 encBox.AddChild(AshfallUiHelpers.MakeSeparator());
00157:             }
00158:
00159:             string encounterText = !string.IsNullOrWhiteSpace(_currentQuest.Discovery)
00160:                 ? _currentQuest.Discovery
00161:                 : "A critical dilemma confronts the shelter cohort. Survival calculations require immediate leadership action.";
00162:
00163:             var bodyLbl = AshfallUiHelpers.MakeBody(encounterText);
00164:             encBox.AddChild(bodyLbl);
00165:             _encounterContainer.AddChild(encounterCard);
00166:
00167:             // 2. Choices section
00168:             AshfallUiHelpers.EmptyChildren(_choicesContainer);
00169:             var choicesCard = AshfallUiHelpers.MakeCardFrame("AUTHORITATIVE DECISION GATES", isResolved ? "RESOLUTION RECORDED" : "SELECT ACTION");
00170:             var chBox = choicesCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00171:
00172:             if (!isResolved)
00173:             {
00174:                 var warnNotice = AshfallUiHelpers.MakeSmall("ATTENTION: Ethical choices permanently alter survivor morale, camp chatter, and regional branch viability. Once committed, a choice cannot be rescinded.");
00175:                 warnNotice.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warning));
00176:                 chBox.AddChild(warnNotice);
00177:                 chBox.AddChild(AshfallUiHelpers.MakeSeparator());
00178:             }
00179:
00180:             for (int i = 0; i < _currentQuest.Choices.Count; i++)
00181:             {
00182:                 int choiceIndex = i;
00183:                 var opt = _currentQuest.Choices[i];
00184:                 bool wasChosen = isResolved && resolution != null && resolution.choiceIndex == i;
00185:
00186:                 var optBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
00187:
00188:                 if (isResolved)
00189:                 {
00190:                     if (wasChosen)
00191:                     {
00192:                         var row = AshfallUiHelpers.MakeDataRow($"[COMMITTED RESOLUTION] Option {i + 1}", opt.Label, AshfallUiHelpers.ToColor(DesignTheme.Hot));
00193:                         optBox.AddChild(row);
00194:
00195:                         if (!string.IsNullOrEmpty(opt.OutcomeText))
00196:                         {
00197:                             var outLbl = AshfallUiHelpers.MakeSmall($"Consequence: {opt.OutcomeText}");
00198:                             outLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00199:                             optBox.AddChild(outLbl);
00200:                         }
00201:
00202:                         if (!string.IsNullOrEmpty(opt.Epitaph))
00203:                         {
00204:                             var epiLbl = AshfallUiHelpers.MakeSmall($"Camp Chronicle: \"{opt.Epitaph}\"");
00205:                             epiLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
00206:                             optBox.AddChild(epiLbl);
00207:                         }
00208:                     }
00209:                     else
00210:                     {
00211:                         var row = AshfallUiHelpers.MakeDataRow($"[UNSELECTED] Option {i + 1}", opt.Label, AshfallUiHelpers.ToColor(DesignTheme.Dim));
00212:                         optBox.AddChild(row);
00213:                     }
00214:                 }
00215:                 else
00216:                 {
00217:                     // Active unresolved option: interactive button without exposing numeric scores
00218:                     var btn = AshfallUiHelpers.MakeButton($"[{i + 1}] COMMIT PATH // {opt.Label.ToUpperInvariant()}", () =>
00219:                     {
00220:                         ExecuteChoice(choiceIndex);
00221:                     });
00222:                     btn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00223:                     optBox.AddChild(btn);
00224:
00225:                     if (_firstInteractiveButton == null)
00226:                         _firstInteractiveButton = btn;
00227:                 }
00228:
00229:                 chBox.AddChild(optBox);
00230:                 if (i < _currentQuest.Choices.Count - 1)
00231:                     chBox.AddChild(AshfallUiHelpers.MakeSeparator());
00232:             }
00233:
00234:             _choicesContainer.AddChild(choicesCard);
00235:
00236:             // 3. Feedback / consequence strip
00237:             AshfallUiHelpers.EmptyChildren(_feedbackContainer);
00238:             if (isResolved && resolution != null)
00239:             {
00240:                 var fbCard = AshfallUiHelpers.MakeCardFrame("RESOLUTION ARCHIVE & CONSEQUENCE RECORD", $"RESOLVED DAY {resolution.resolvedDay}");
00241:                 var fbBox = fbCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00242:
00243:                 string arrow = resolution.impactMark == "up" ? "🔺 Positive Social Trajectory"
00244:                     : resolution.impactMark == "down" ? "🔻 Hardened Survival Stance" : "⚪ Neutral Pragmatic Shift";
00245:
00246:                 fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Moral Resonance", arrow, AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00247:                 fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Camp Record", resolution.epitaph, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00248:                 fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Journal Status", "Archived to permanent Holdfast survival chronicle.", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00249:
00250:                 _feedbackContainer.AddChild(fbCard);
00251:             }
00252:         }
00253:
00254:         private void ExecuteChoice(int choiceIndex)
00255:         {
00256:             if (_currentQuest == null) return;
00257:             string questId = _currentQuest.Id;
00258:
00259:             // Prefer Bind callback when present so host paths cannot double-resolve
00260:             // via both OnChoiceSelected and the Bind delegate.
00261:             if (_onChoiceCallback != null)
00262:                 _onChoiceCallback.Invoke(questId, choiceIndex);
00263:             else
00264:                 OnChoiceSelected?.Invoke(questId, choiceIndex);
00265:
00266:             // Re-render in place
00267:             RefreshContent();
00268:         }
00269:
00270:         public void Open()
00271:         {
00272:             Visible = true;
00273:             RefreshContent();
00274:             _firstInteractiveButton?.GrabFocus();
00275:         }
00276:
00277:         public void SelectChoiceForTest(int choiceIndex) => ExecuteChoice(choiceIndex);
00278:
00279:         public void CloseModal()
00280:         {
00281:             Visible = false;
00282:             OnModalClosed?.Invoke();
00283:             OnClose?.Invoke();
00284:         }
00285:
00286:         public override void _UnhandledInput(InputEvent @event)
00287:         {
00288:             if (!Visible) return;
00289:
00290:             if (@event is InputEventKey key && key.Pressed)
00291:             {
00292:                 if (key.Keycode == Key.Escape)
00293:                 {
00294:                     CloseModal();
00295:                     GetViewport().SetInputAsHandled();
00296:                     return;
00297:                 }
00298:
00299:                 // Keyboard quick-selection for options 1-9 if unresolved
00300:                 if (_currentQuest != null && (_moralChoiceSystem == null || !_moralChoiceSystem.IsResolved(_currentQuest.Id)))
00301:                 {
00302:                     int number = -1;
00303:                     if (key.Keycode >= Key.Key1 && key.Keycode <= Key.Key9)
00304:                         number = (int)(key.Keycode - Key.Key1);
00305:                     else if (key.Keycode >= Key.Kp1 && key.Keycode <= Key.Kp9)
00306:                         number = (int)(key.Keycode - Key.Kp1);
00307:
00308:                     if (number >= 0 && number < _currentQuest.Choices.Count)
00309:                     {
00310:                         ExecuteChoice(number);
00311:                         GetViewport().SetInputAsHandled();
00312:                     }
00313:                 }
00314:             }
00315:         }
00316:     }
00317: }
```

## `Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs` — 518 lines; 23,695 bytes; SHA-256 `d97da758b073e2ea3d621eb78422436230d54d0e469939c184ff2fa9caa93464`
Declaration index:
- 00019: public sealed class MoralChoiceEchoExpansionTests : CatalogTestBase
- 00096: private static MoralChoiceChainData LoadChains() =>
- 00114: public void Catalog_LoadsExactlySixtyEchoQuests()
- 00121: public void Catalog_EchoQuestIds_AreUniqueAndCanonical()
- 00137: public void Catalog_PreservesAllThirtyTwoBaselineEchoes()
- 00152: public void Catalog_TwentyEightNewEchoes_MatchRequiredBranchDistribution()
- 00192: public void ReferenceIntegrity_AllTriggeredByResolveInQuestCatalogs()
- 00207: public void ReferenceIntegrity_AllTwentyEightNewTriggeredByResolveInQuestGates()
- 00228: public void ChoiceIndexIntegrity_AllChoicesAreValidForSourceQuests()
- 00245: public void TemporalReachability_AllEchoesReachableWithinCampaignHorizon()
- 00265: public void BranchConsistency_EchoBranchMatchesGatedQuestBranch()
- 00283: public void Runtime_MercyEcho_FiresOnlyAfterDelayAndMatchingChoice()
- 00323: public void Runtime_IronEcho_FiresOnlyAfterDelayAndMatchingChoice()
- 00353: public void Runtime_ListenerEcho_FiresOnlyAfterDelayAndMatchingChoice()
- 00384: public void Runtime_BrokenCompactEcho_FiresOnlyAfterDelayAndMatchingChoice()
- 00415: public void Runtime_BranchLockout_SuppressesLockedEchoQuests()
- 00458: public void Runtime_MarkEchoQuestFired_PreventsRefiring()
- 00484: public void Runtime_Arbitration_DeterministicOrdering()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Xunit;
00007: using Ashfall.Core;
00008: using Ashfall.Core.MoralChoice;
00009:
00010: namespace Ashfall.Core.Tests
00011: {
00012:     /// <summary>
00013:     /// Plan 109: Comprehensive test suite for the expanded 60-echo quest catalog
00014:     /// in moral_choice_chains.json (32 baseline + 28 new delayed callback quests).
00015:     /// Covers schema parsing, ID uniqueness, baseline preservation, branch distribution,
00016:     /// trigger reference integrity, choice index bounds, temporal reachability,
00017:     /// branch consistency, and runtime eligibility / arbitration / lockout behavior.
00018:     /// </summary>
00019:     public sealed class MoralChoiceEchoExpansionTests : CatalogTestBase
00020:     {
00021:         private static readonly IFileIO s_files = new FileSystemIO();
00022:         private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
00023:
00024:         private static readonly string[] s_baselineEchoIds = new[]
00025:         {
00026:             "quest_moral_echo_child_returns",
00027:             "quest_moral_echo_child_steals",
00028:             "quest_moral_echo_family_defends",
00029:             "quest_moral_echo_family_ambush",
00030:             "quest_moral_echo_farmer_harvest",
00031:             "quest_moral_echo_farmer_dead",
00032:             "quest_moral_echo_raider_warning",
00033:             "quest_moral_echo_raider_ambush",
00034:             "quest_moral_echo_peacekeeper_intel",
00035:             "quest_moral_echo_peacekeeper_hunted",
00036:             "quest_moral_echo_widow_gift",
00037:             "quest_moral_echo_prophet_map",
00038:             "quest_moral_echo_prophet_curse",
00039:             "quest_moral_echo_soldier_teaches",
00040:             "quest_moral_echo_soldier_hostile",
00041:             "quest_moral_echo_messenger_packet",
00042:             "quest_moral_echo_messenger_stolen",
00043:             "quest_moral_echo_dead_child_haunt",
00044:             "quest_moral_echo_dead_child_peace",
00045:             "quest_moral_echo_scientist_formula",
00046:             "quest_moral_echo_mercy_recognized",
00047:             "quest_moral_echo_iron_feared",
00048:             "quest_moral_echo_listener_confided",
00049:             "quest_moral_echo_betrayer_hunted",
00050:             "quest_moral_echo_mercy_tested",
00051:             "quest_moral_echo_iron_challenged",
00052:             "quest_moral_echo_listener_secret",
00053:             "quest_moral_echo_betrayer_cornered",
00054:             "quest_moral_echo_mercy_final",
00055:             "quest_moral_echo_iron_final",
00056:             "quest_moral_echo_listener_final",
00057:             "quest_moral_echo_betrayer_final"
00058:         };
00059:
00060:         private static readonly string[] s_newEchoIds = new[]
00061:         {
00062:             // Mercy Road (8)
00063:             "quest_moral_echo_raider_repaid_warning",
00064:             "quest_moral_echo_betrayers_child_grown",
00065:             "quest_moral_echo_medicine_shared_recovered",
00066:             "quest_moral_echo_convoy_haven_opened",
00067:             "quest_moral_echo_plague_secret_infection",
00068:             "quest_moral_echo_well_gratitude_refused",
00069:             "quest_moral_echo_patrol_reputation_spread",
00070:             "quest_moral_echo_shelter_vote_strained_rations",
00071:             // Iron Way (8)
00072:             "quest_moral_echo_aldric_blockade_retaliation",
00073:             "quest_moral_echo_old_friend_farewell_note",
00074:             "quest_moral_echo_expulsion_deterrence_held",
00075:             "quest_moral_echo_strike_broken_fear_quota",
00076:             "quest_moral_echo_informant_applies_leverage",
00077:             "quest_moral_echo_lowfield_harvest_dividend",
00078:             "quest_moral_echo_varek_blood_debt_claim",
00079:             "quest_moral_echo_calla_camp_empty_ruin",
00080:             // Listener Thread (7)
00081:             "quest_moral_echo_defector_corroborates_truth",
00082:             "quest_moral_echo_cartographer_water_cache_located",
00083:             "quest_moral_echo_prophet_calendar_discrepancy",
00084:             "quest_moral_echo_trader_ledger_censorship_threat",
00085:             "quest_moral_echo_soldier_second_confession",
00086:             "quest_moral_echo_doctors_notes_reinterpreted",
00087:             "quest_moral_echo_librarian_memorial_preserved",
00088:             // Broken Compact (5)
00089:             "quest_moral_echo_kessler_exile_uncovered",
00090:             "quest_moral_echo_poisoned_gift_reputation_drop",
00091:             "quest_moral_echo_crisis_gambit_warlord_respect",
00092:             "quest_moral_echo_voss_blackmail_exposed",
00093:             "quest_moral_echo_pell_hostage_border_locked"
00094:         };
00095:
00096:         private static MoralChoiceChainData LoadChains() =>
00097:             MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00098:
00099:         private static Dictionary<string, MoralChoiceQuestDefinition> LoadAllQuests()
00100:         {
00101:             var dict = new Dictionary<string, MoralChoiceQuestDefinition>(StringComparer.Ordinal);
00102:             var baseQuests = MoralChoiceCatalogLoader.Load(DataDirectory, s_files, s_json);
00103:             foreach (var q in baseQuests) dict[q.Id] = q;
00104:             var branchQuests = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
00105:             foreach (var q in branchQuests) dict[q.Id] = q;
00106:             var expQuests = MoralChoiceExpansionQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
00107:             foreach (var q in expQuests) dict[q.Id] = q;
00108:             return dict;
00109:         }
00110:
00111:         // ── 1. Catalog Count & Parse Tests ──────────────────────────────
00112:
00113:         [Fact]
00114:         public void Catalog_LoadsExactlySixtyEchoQuests()
00115:         {
00116:             var data = LoadChains();
00117:             Assert.Equal(60, data.EchoQuests.Count);
00118:         }
00119:
00120:         [Fact]
00121:         public void Catalog_EchoQuestIds_AreUniqueAndCanonical()
00122:         {
00123:             var data = LoadChains();
00124:             var seen = new HashSet<string>(StringComparer.Ordinal);
00125:             foreach (var echo in data.EchoQuests)
00126:             {
00127:                 Assert.False(string.IsNullOrWhiteSpace(echo.QuestId));
00128:                 Assert.StartsWith("quest_moral_echo_", echo.QuestId);
00129:                 Assert.True(seen.Add(echo.QuestId), $"Duplicate echo quest ID: {echo.QuestId}");
00130:             }
00131:             Assert.Equal(60, seen.Count);
00132:         }
00133:
00134:         // ── 2. Baseline Preservation Tests ──────────────────────────────
00135:
00136:         [Fact]
00137:         public void Catalog_PreservesAllThirtyTwoBaselineEchoes()
00138:         {
00139:             var data = LoadChains();
00140:             var byId = data.EchoQuests.ToDictionary(e => e.QuestId, StringComparer.Ordinal);
00141:
00142:             foreach (var baseId in s_baselineEchoIds)
00143:             {
00144:                 Assert.True(byId.ContainsKey(baseId), $"Baseline echo missing: {baseId}");
00145:             }
00146:             Assert.Equal(32, s_baselineEchoIds.Length);
00147:         }
00148:
00149:         // ── 3. Branch Distribution Tests ────────────────────────────────
00150:
00151:         [Fact]
00152:         public void Catalog_TwentyEightNewEchoes_MatchRequiredBranchDistribution()
00153:         {
00154:             var data = LoadChains();
00155:             var byId = data.EchoQuests.ToDictionary(e => e.QuestId, StringComparer.Ordinal);
00156:
00157:             Assert.Equal(28, s_newEchoIds.Length);
00158:             foreach (var newId in s_newEchoIds)
00159:             {
00160:                 Assert.True(byId.ContainsKey(newId), $"New echo missing: {newId}");
00161:             }
00162:
00163:             var newEchoes = s_newEchoIds.Select(id => byId[id]).ToList();
00164:
00165:             int mercyCount = newEchoes.Count(e => e.Branch == "branch_mercy_road");
00166:             int ironCount = newEchoes.Count(e => e.Branch == "branch_iron_way");
00167:             int listenerCount = newEchoes.Count(e => e.Branch == "branch_listener_thread");
00168:             int brokenCount = newEchoes.Count(e => e.Branch == "branch_broken_compact");
00169:
00170:             Assert.Equal(8, mercyCount);
00171:             Assert.Equal(8, ironCount);
00172:             Assert.Equal(7, listenerCount);
00173:             Assert.Equal(5, brokenCount);
00174:
00175:             // Total catalog counts (including 3 baseline per branch + 20 branch-agnostic)
00176:             int totalMercy = data.EchoQuests.Count(e => e.Branch == "branch_mercy_road");
00177:             int totalIron = data.EchoQuests.Count(e => e.Branch == "branch_iron_way");
00178:             int totalListener = data.EchoQuests.Count(e => e.Branch == "branch_listener_thread");
00179:             int totalBroken = data.EchoQuests.Count(e => e.Branch == "branch_broken_compact");
00180:             int totalAgnostic = data.EchoQuests.Count(e => string.IsNullOrEmpty(e.Branch));
00181:
00182:             Assert.Equal(11, totalMercy);
00183:             Assert.Equal(11, totalIron);
00184:             Assert.Equal(10, totalListener);
00185:             Assert.Equal(8, totalBroken);
00186:             Assert.Equal(20, totalAgnostic);
00187:         }
00188:
00189:         // ── 4. Trigger Reference Integrity Tests ────────────────────────
00190:
00191:         [Fact]
00192:         public void ReferenceIntegrity_AllTriggeredByResolveInQuestCatalogs()
00193:         {
00194:             var data = LoadChains();
00195:             var allQuests = LoadAllQuests();
00196:
00197:             foreach (var echo in data.EchoQuests)
00198:             {
00199:                 Assert.False(string.IsNullOrWhiteSpace(echo.TriggeredBy),
00200:                     $"{echo.QuestId} has empty TriggeredBy");
00201:                 Assert.True(allQuests.ContainsKey(echo.TriggeredBy),
00202:                     $"{echo.QuestId} TriggeredBy '{echo.TriggeredBy}' not found in any moral quest catalog");
00203:             }
00204:         }
00205:
00206:         [Fact]
00207:         public void ReferenceIntegrity_AllTwentyEightNewTriggeredByResolveInQuestGates()
00208:         {
00209:             var data = LoadChains();
00210:             var gateIds = new HashSet<string>(data.QuestGates.Select(g => g.QuestId), StringComparer.Ordinal);
00211:             var byId = data.EchoQuests.ToDictionary(e => e.QuestId, StringComparer.Ordinal);
00212:
00213:             var triggersSeen = new HashSet<string>(StringComparer.Ordinal);
00214:             foreach (var newId in s_newEchoIds)
00215:             {
00216:                 var echo = byId[newId];
00217:                 Assert.True(gateIds.Contains(echo.TriggeredBy),
00218:                     $"New echo {echo.QuestId} TriggeredBy '{echo.TriggeredBy}' not in quest_gates");
00219:                 Assert.True(triggersSeen.Add(echo.TriggeredBy),
00220:                     $"Duplicate source quest across new echoes: {echo.TriggeredBy}");
00221:             }
00222:             Assert.Equal(28, triggersSeen.Count);
00223:         }
00224:
00225:         // ── 5. Choice Index & Bounds Integrity Tests ────────────────────
00226:
00227:         [Fact]
00228:         public void ChoiceIndexIntegrity_AllChoicesAreValidForSourceQuests()
00229:         {
00230:             var data = LoadChains();
00231:             var allQuests = LoadAllQuests();
00232:
00233:             foreach (var echo in data.EchoQuests)
00234:             {
00235:                 Assert.True(allQuests.TryGetValue(echo.TriggeredBy, out var sourceQuest),
00236:                     $"Source quest '{echo.TriggeredBy}' not found for {echo.QuestId}");
00237:
00238:                 Assert.InRange(echo.TriggeredByChoice, 0, sourceQuest!.Choices.Count - 1);
00239:             }
00240:         }
00241:
00242:         // ── 6. Temporal Reachability & Branch Consistency ───────────────
00243:
00244:         [Fact]
00245:         public void TemporalReachability_AllEchoesReachableWithinCampaignHorizon()
00246:         {
00247:             var data = LoadChains();
00248:             var allQuests = LoadAllQuests();
00249:
00250:             foreach (var echo in data.EchoQuests)
00251:             {
00252:                 Assert.True(echo.MinDaysAfter > 0, $"{echo.QuestId} MinDaysAfter must be positive");
00253:                 Assert.InRange(echo.MinDaysAfter, 5, 80);
00254:
00255:                 if (allQuests.TryGetValue(echo.TriggeredBy, out var source))
00256:                 {
00257:                     int earliestFirableDay = source.MinDay + echo.MinDaysAfter;
00258:                     Assert.True(earliestFirableDay <= 360,
00259:                         $"{echo.QuestId} earliest day {earliestFirableDay} exceeds 360-day campaign horizon");
00260:                 }
00261:             }
00262:         }
00263:
00264:         [Fact]
00265:         public void BranchConsistency_EchoBranchMatchesGatedQuestBranch()
00266:         {
00267:             var data = LoadChains();
00268:             var gateBranches = data.QuestGates.ToDictionary(g => g.QuestId, g => g.Branch, StringComparer.Ordinal);
00269:             var byId = data.EchoQuests.ToDictionary(e => e.QuestId, StringComparer.Ordinal);
00270:
00271:             foreach (var newId in s_newEchoIds)
00272:             {
00273:                 var echo = byId[newId];
00274:                 Assert.True(gateBranches.TryGetValue(echo.TriggeredBy, out var expectedBranch),
00275:                     $"Source {echo.TriggeredBy} has no gate branch");
00276:                 Assert.Equal(expectedBranch, echo.Branch);
00277:             }
00278:         }
00279:
00280:         // ── 7. Runtime Simulation & Gating Tests ────────────────────────
00281:
00282:         [Fact]
00283:         public void Runtime_MercyEcho_FiresOnlyAfterDelayAndMatchingChoice()
00284:         {
00285:             var sys = new MoralChoiceSystem(new SeededRng(42));
00286:             var chainData = LoadChains();
00287:             sys.InitializeChainData(chainData);
00288:
00289:             // Source: quest_moral_chain_mercy_16 ("The Prodigal Raider"), min_day 165, choice 0, delay 25
00290:             var quest = new MoralChoiceQuestDefinition
00291:             {
00292:                 Id = "quest_moral_chain_mercy_16",
00293:                 DisplayName = "The Prodigal Raider",
00294:                 Category = "comfort",
00295:                 Choices = new List<MoralChoiceOption>
00296:                 {
00297:                     new MoralChoiceOption { Label = "Accept him", MoralDelta = 12, EmpathyDelta = 2, Epitaph = "Accepted" },
00298:                     new MoralChoiceOption { Label = "House outside", MoralDelta = 6, EmpathyDelta = 0, Epitaph = "Outside" },
00299:                     new MoralChoiceOption { Label = "Send away", MoralDelta = 0, EmpathyDelta = 0, Epitaph = "Away" },
00300:                     new MoralChoiceOption { Label = "Refuse", MoralDelta = -4, EmpathyDelta = 0, Epitaph = "Refused" }
00301:                 }
00302:             };
00303:
00304:             sys.Resolve(quest, 0, "loc_shelter_gate", 165);
00305:
00306:             // Day 189: 165 + 24 < 165 + 25 -> Not available
00307:             var preDelay = sys.FindAvailableEchoQuests(189);
00308:             Assert.DoesNotContain(preDelay, e => e.QuestId == "quest_moral_echo_raider_repaid_warning");
00309:
00310:             // Day 190: 165 + 25 -> Available
00311:             var exactDay = sys.FindAvailableEchoQuests(190);
00312:             Assert.Contains(exactDay, e => e.QuestId == "quest_moral_echo_raider_repaid_warning");
00313:
00314:             // Wrong choice rejection
00315:             var sys2 = new MoralChoiceSystem(new SeededRng(43));
00316:             sys2.InitializeChainData(chainData);
00317:             sys2.Resolve(quest, 3, "loc_shelter_gate", 165);
00318:             var wrongChoice = sys2.FindAvailableEchoQuests(200);
00319:             Assert.DoesNotContain(wrongChoice, e => e.QuestId == "quest_moral_echo_raider_repaid_warning");
00320:         }
00321:
00322:         [Fact]
00323:         public void Runtime_IronEcho_FiresOnlyAfterDelayAndMatchingChoice()
00324:         {
00325:             var sys = new MoralChoiceSystem(new SeededRng(42));
00326:             var chainData = LoadChains();
00327:             sys.InitializeChainData(chainData);
00328:
00329:             // Source: quest_moral_chain_iron_06 ("The Merchant's Proposition"), min_day 12, choice 0, delay 45
00330:             var quest = new MoralChoiceQuestDefinition
00331:             {
00332:                 Id = "quest_moral_chain_iron_06",
00333:                 DisplayName = "The Merchant's Proposition",
00334:                 Category = "trust",
00335:                 Choices = new List<MoralChoiceOption>
00336:                 {
00337:                     new MoralChoiceOption { Label = "Accept", MoralDelta = -10, EmpathyDelta = 0, Epitaph = "Accepted" },
00338:                     new MoralChoiceOption { Label = "Counter", MoralDelta = 2, EmpathyDelta = 1, Epitaph = "Countered" },
00339:                     new MoralChoiceOption { Label = "Refuse", MoralDelta = 6, EmpathyDelta = 1, Epitaph = "Refused" }
00340:                 }
00341:             };
00342:
00343:             sys.Resolve(quest, 0, "loc_aldric", 12);
00344:
00345:             // Day 56: 12 + 44 < 12 + 45 -> Not available
00346:             Assert.DoesNotContain(sys.FindAvailableEchoQuests(56), e => e.QuestId == "quest_moral_echo_aldric_blockade_retaliation");
00347:
00348:             // Day 57: 12 + 45 -> Available
00349:             Assert.Contains(sys.FindAvailableEchoQuests(57), e => e.QuestId == "quest_moral_echo_aldric_blockade_retaliation");
00350:         }
00351:
00352:         [Fact]
00353:         public void Runtime_ListenerEcho_FiresOnlyAfterDelayAndMatchingChoice()
00354:         {
00355:             var sys = new MoralChoiceSystem(new SeededRng(42));
00356:             var chainData = LoadChains();
00357:             sys.InitializeChainData(chainData);
00358:
00359:             // Source: quest_moral_chain_listen_11 ("The Defector's Confession"), min_day 22, choice 0, delay 35
00360:             var quest = new MoralChoiceQuestDefinition
00361:             {
00362:                 Id = "quest_moral_chain_listen_11",
00363:                 DisplayName = "The Defector's Confession",
00364:                 Category = "listen",
00365:                 Choices = new List<MoralChoiceOption>
00366:                 {
00367:                     new MoralChoiceOption { Label = "Listen full", MoralDelta = 14, EmpathyDelta = 4, Epitaph = "Listened" },
00368:                     new MoralChoiceOption { Label = "Advocate", MoralDelta = 8, EmpathyDelta = 2, Epitaph = "Advocated" },
00369:                     new MoralChoiceOption { Label = "No backstory", MoralDelta = -2, EmpathyDelta = 0, Epitaph = "Info" },
00370:                     new MoralChoiceOption { Label = "Distrust", MoralDelta = -5, EmpathyDelta = 0, Epitaph = "Distrusted" }
00371:                 }
00372:             };
00373:
00374:             sys.Resolve(quest, 0, "loc_relay", 22);
00375:
00376:             // Day 56 < 57
00377:             Assert.DoesNotContain(sys.FindAvailableEchoQuests(56), e => e.QuestId == "quest_moral_echo_defector_corroborates_truth");
00378:
00379:             // Day 57 >= 57
00380:             Assert.Contains(sys.FindAvailableEchoQuests(57), e => e.QuestId == "quest_moral_echo_defector_corroborates_truth");
00381:         }
00382:
00383:         [Fact]
00384:         public void Runtime_BrokenCompactEcho_FiresOnlyAfterDelayAndMatchingChoice()
00385:         {
00386:             var sys = new MoralChoiceSystem(new SeededRng(42));
00387:             var chainData = LoadChains();
00388:             sys.InitializeChainData(chainData);
00389:
00390:             // Source: quest_moral_chain_betray_04 ("The Framed Hand"), min_day 35, choice 1, delay 35
00391:             var quest = new MoralChoiceQuestDefinition
00392:             {
00393:                 Id = "quest_moral_chain_betray_04",
00394:                 DisplayName = "The Framed Hand",
00395:                 Category = "trust",
00396:                 Choices = new List<MoralChoiceOption>
00397:                 {
00398:                     new MoralChoiceOption { Label = "Clear record", MoralDelta = 5, EmpathyDelta = 1, Epitaph = "Cleared" },
00399:                     new MoralChoiceOption { Label = "Plant tools", MoralDelta = -15, EmpathyDelta = 0, Epitaph = "Planted" },
00400:                     new MoralChoiceOption { Label = "Report anon", MoralDelta = -8, EmpathyDelta = 0, Epitaph = "Reported" },
00401:                     new MoralChoiceOption { Label = "Tell privately", MoralDelta = 2, EmpathyDelta = 1, Epitaph = "Told" }
00402:                 }
00403:             };
00404:
00405:             sys.Resolve(quest, 1, "loc_workshop", 35);
00406:
00407:             // Day 69 < 70
00408:             Assert.DoesNotContain(sys.FindAvailableEchoQuests(69), e => e.QuestId == "quest_moral_echo_kessler_exile_uncovered");
00409:
00410:             // Day 70 >= 70
00411:             Assert.Contains(sys.FindAvailableEchoQuests(70), e => e.QuestId == "quest_moral_echo_kessler_exile_uncovered");
00412:         }
00413:
00414:         [Fact]
00415:         public void Runtime_BranchLockout_SuppressesLockedEchoQuests()
00416:         {
00417:             var sys = new MoralChoiceSystem(new SeededRng(42));
00418:             var chainData = LoadChains();
00419:             sys.InitializeChainData(chainData);
00420:
00421:             // Resolve 3 entry quests on Iron Way to lock out Mercy Road & Listener Thread
00422:             for (int i = 1; i <= 3; i++)
00423:             {
00424:                 var ironEntry = new MoralChoiceQuestDefinition
00425:                 {
00426:                     Id = $"quest_moral_chain_iron_{i:D2}",
00427:                     DisplayName = $"Iron Entry {i}",
00428:                     Category = "trust",
00429:                     Choices = new List<MoralChoiceOption>
00430:                     {
00431:                         new MoralChoiceOption { Label = "Iron Choice", MoralDelta = -10, EmpathyDelta = 0, Epitaph = "Iron" }
00432:                     }
00433:                 };
00434:                 sys.Resolve(ironEntry, 0, "loc_iron", 10 + i);
00435:             }
00436:
00437:             Assert.True(sys.IsBranchLocked("branch_mercy_road"));
00438:
00439:             // Also resolve a Mercy quest
00440:             var mercyQuest = new MoralChoiceQuestDefinition
00441:             {
00442:                 Id = "quest_moral_chain_mercy_16",
00443:                 DisplayName = "The Prodigal Raider",
00444:                 Category = "comfort",
00445:                 Choices = new List<MoralChoiceOption>
00446:                 {
00447:                     new MoralChoiceOption { Label = "Accept him", MoralDelta = 12, EmpathyDelta = 2, Epitaph = "Accepted" }
00448:                 }
00449:             };
00450:             sys.Resolve(mercyQuest, 0, "loc_shelter_gate", 50);
00451:
00452:             // Even on day 200 (> 50 + 25), the Mercy echo MUST NOT fire because Mercy Road is locked!
00453:             var available = sys.FindAvailableEchoQuests(200);
00454:             Assert.DoesNotContain(available, e => e.QuestId == "quest_moral_echo_raider_repaid_warning");
00455:         }
00456:
00457:         [Fact]
00458:         public void Runtime_MarkEchoQuestFired_PreventsRefiring()
00459:         {
00460:             var sys = new MoralChoiceSystem(new SeededRng(42));
00461:             var chainData = LoadChains();
00462:             sys.InitializeChainData(chainData);
00463:
00464:             var quest = new MoralChoiceQuestDefinition
00465:             {
00466:                 Id = "quest_moral_chain_mercy_16",
00467:                 DisplayName = "The Prodigal Raider",
00468:                 Category = "comfort",
00469:                 Choices = new List<MoralChoiceOption>
00470:                 {
00471:                     new MoralChoiceOption { Label = "Accept him", MoralDelta = 12, EmpathyDelta = 2, Epitaph = "Accepted" }
00472:                 }
00473:             };
00474:             sys.Resolve(quest, 0, "loc_shelter_gate", 165);
00475:
00476:             Assert.Contains(sys.FindAvailableEchoQuests(200), e => e.QuestId == "quest_moral_echo_raider_repaid_warning");
00477:
00478:             sys.MarkEchoQuestFired("quest_moral_echo_raider_repaid_warning");
00479:
00480:             Assert.DoesNotContain(sys.FindAvailableEchoQuests(200), e => e.QuestId == "quest_moral_echo_raider_repaid_warning");
00481:         }
00482:
00483:         [Fact]
00484:         public void Runtime_Arbitration_DeterministicOrdering()
00485:         {
00486:             var sys1 = new MoralChoiceSystem(new SeededRng(100));
00487:             var sys2 = new MoralChoiceSystem(new SeededRng(200));
00488:             var chainData = LoadChains();
00489:             sys1.InitializeChainData(chainData);
00490:             sys2.InitializeChainData(chainData);
00491:
00492:             var q1 = new MoralChoiceQuestDefinition
00493:             {
00494:                 Id = "quest_moral_chain_mercy_04",
00495:                 DisplayName = "Mercy 04",
00496:                 Choices = new List<MoralChoiceOption> { new MoralChoiceOption { Label = "C0" } }
00497:             };
00498:             var q2 = new MoralChoiceQuestDefinition
00499:             {
00500:                 Id = "quest_moral_chain_mercy_06",
00501:                 DisplayName = "Mercy 06",
00502:                 Choices = new List<MoralChoiceOption> { new MoralChoiceOption { Label = "C0" } }
00503:             };
00504:
00505:             sys1.Resolve(q1, 0, "loc_a", 10);
00506:             sys1.Resolve(q2, 0, "loc_b", 10);
00507:
00508:             sys2.Resolve(q1, 0, "loc_a", 10);
00509:             sys2.Resolve(q2, 0, "loc_b", 10);
00510:
00511:             var avail1 = sys1.FindAvailableEchoQuests(100).Select(e => e.QuestId).ToList();
00512:             var avail2 = sys2.FindAvailableEchoQuests(100).Select(e => e.QuestId).ToList();
00513:
00514:             Assert.NotEmpty(avail1);
00515:             Assert.Equal(avail1, avail2);
00516:         }
00517:     }
00518: }
```

## `Ashfall.Core.Tests/MoralChoiceSystemTests.cs` — 428 lines; 17,283 bytes; SHA-256 `43c39778ac33913ad3ea4cb255b57fb54511909b6560e169bb599e20dc6ed4ec`
Declaration index:
- 00011: public class MoralChoiceSystemTests
- 00013: private static MoralChoiceSystem Sys(int seed = 42) => new MoralChoiceSystem(new SeededRng(seed));
- 00015: private static MoralChoiceQuestDefinition Quest(
- 00040: public void InitialStateNeutralAndEmpty()
- 00053: public void BandEdgesPinned()
- 00088: public void ResolveAppliesDeltasAndRaisesEvent()
- 00109: public void ResolveIsIdempotentPerQuest()
- 00124: public void ScoreClampsAndLegendSettlesAtReconcileOncePerDirection()
- 00147: public void ResolveRejectsNonCanonicalQuestId()
- 00154: public void ResolveRejectsOutOfRangeChoice()
- 00163: public void ImpactMarksFollowDeltaSign()
- 00172: public void SameSeedSameRolls()
- 00188: public void ReconcileFiresExtremeBandEventsOnce()
- 00217: public void ReconcileFiresAllCrossedBandsOnBigJump()
- 00236: public void ReconcileFiresContractAtPositiveBand()
- 00249: public void ReconcileIgnoresOutOfOrderDays()
- 00258: public void EndingStorykeeperOverridesBand()
- 00273: public void EndingSelectionRules()
- 00304: public void StorykeeperNeedsBothThresholds()
- 00312: public void ListenerAndConfidantThresholdsPinnedAtBoundary()
- 00330: public void RestoreRejectsMismatchedSystemAndBadSchema()
- 00346: public void PendingLegendFlagsSurviveRoundTrip()
- 00364: public void SaveRoundTripPreservesLedger()
- 00396: public void CapturedStateIsDetached()
- 00411: public void AvailabilityWindow()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Xunit;
00006: using Ashfall.Core;
00007: using Ashfall.Core.MoralChoice;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     public class MoralChoiceSystemTests
00012:     {
00013:         private static MoralChoiceSystem Sys(int seed = 42) => new MoralChoiceSystem(new SeededRng(seed));
00014:
00015:         private static MoralChoiceQuestDefinition Quest(
00016:             string id = "quest_moral_test",
00017:             params (int moral, int empathy)[] deltas)
00018:         {
00019:             (int moral, int empathy)[] source = deltas.Length == 0
00020:                 ? new (int moral, int empathy)[] { (moral: 10, empathy: 1), (moral: 5, empathy: 0), (moral: 0, empathy: 0), (moral: -5, empathy: 0) }
00021:                 : deltas;
00022:             var choices = source
00023:                 .Select(d => new MoralChoiceOption
00024:                 {
00025:                     MoralDelta = d.moral,
00026:                     EmpathyDelta = d.empathy,
00027:                     Epitaph = $"chose {d.moral}"
00028:                 })
00029:                 .ToList();
00030:             return new MoralChoiceQuestDefinition
00031:             {
00032:                 Id = id,
00033:                 DisplayName = id,
00034:                 Category = "trust",
00035:                 Choices = choices
00036:             };
00037:         }
00038:
00039:         [Fact]
00040:         public void InitialStateNeutralAndEmpty()
00041:         {
00042:             var sys = Sys();
00043:             Assert.Equal(0, sys.MoralScore);
00044:             Assert.Equal(0, sys.EmpathyPoints);
00045:             Assert.Equal(0, sys.QuestsResolved);
00046:             Assert.Equal(MoralPathBand.Neutral, sys.CurrentBand);
00047:             Assert.False(sys.IsListener);
00048:             Assert.False(sys.IsConfidant);
00049:             Assert.Equal(MoralEndingKind.BalancedSurvivor, sys.SelectEnding());
00050:         }
00051:
00052:         [Fact]
00053:         public void BandEdgesPinned()
00054:         {
00055:             var cases = new (int Score, MoralPathBand Expected)[]
00056:             {
00057:                 (-500, MoralPathBand.VeryEvil),
00058:                 (-200, MoralPathBand.VeryEvil),
00059:                 (-100, MoralPathBand.VeryEvil),
00060:                 (-99, MoralPathBand.Evil),
00061:                 (-50, MoralPathBand.Evil),
00062:                 (-49, MoralPathBand.SlightlyEvil),
00063:                 (-1, MoralPathBand.SlightlyEvil),
00064:                 (0, MoralPathBand.Neutral),
00065:                 (1, MoralPathBand.SlightlyPositive),
00066:                 (49, MoralPathBand.SlightlyPositive),
00067:                 (50, MoralPathBand.Positive),
00068:                 (99, MoralPathBand.Positive),
00069:                 (100, MoralPathBand.VeryPositive),
00070:                 (200, MoralPathBand.VeryPositive),
00071:                 (500, MoralPathBand.VeryPositive)
00072:             };
00073:             var failures = new List<string>();
00074:
00075:             foreach (var testCase in cases)
00076:             {
00077:                 var actual = MoralChoiceSystem.BandForScore(testCase.Score);
00078:                 if (actual != testCase.Expected)
00079:                 {
00080:                     failures.Add($"score {testCase.Score}: expected {testCase.Expected}, got {actual}");
00081:                 }
00082:             }
00083:
00084:             Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
00085:         }
00086:
00087:         [Fact]
00088:         public void ResolveAppliesDeltasAndRaisesEvent()
00089:         {
00090:             var sys = Sys();
00091:             MoralChoiceResolution? raised = null;
00092:             sys.OnQuestResolved += r => raised = r;
00093:
00094:             var resolution = sys.Resolve(Quest("quest_moral_share_child"), 0, "loc_urban_ruins_block_9", 12);
00095:
00096:             Assert.Equal(10, sys.MoralScore);
00097:             Assert.Equal(1, sys.EmpathyPoints);
00098:             Assert.Equal(1, sys.QuestsResolved);
00099:             Assert.Same(resolution, raised);
00100:             Assert.Equal("quest_moral_share_child", resolution.questId);
00101:             Assert.Equal(12, resolution.resolvedDay);
00102:             Assert.Equal("up", resolution.impactMark);
00103:             Assert.InRange(resolution.outcomeRoll, 0, 99);
00104:             Assert.InRange(resolution.propagatesOnDay, 13, 15);
00105:             Assert.True(sys.IsResolved("quest_moral_share_child"));
00106:         }
00107:
00108:         [Fact]
00109:         public void ResolveIsIdempotentPerQuest()
00110:         {
00111:             var sys = Sys();
00112:             var first = sys.Resolve(Quest("quest_moral_a"), 0, "loc_x", 5);
00113:
00114:             var second = sys.Resolve(Quest("quest_moral_a"), 3, "loc_x", 40);
00115:
00116:             Assert.Equal(1, sys.QuestsResolved);
00117:             Assert.Equal(10, sys.MoralScore);
00118:             Assert.Equal(first.outcomeRoll, second.outcomeRoll);
00119:             Assert.Equal(first.propagatesOnDay, second.propagatesOnDay);
00120:             Assert.Equal(first.choiceIndex, second.choiceIndex);
00121:         }
00122:
00123:         [Fact]
00124:         public void ScoreClampsAndLegendSettlesAtReconcileOncePerDirection()
00125:         {
00126:             var sys = Sys();
00127:             var fired = new List<string>();
00128:             sys.OnThresholdEventFired += fired.Add;
00129:
00130:             sys.Resolve(Quest("quest_moral_cap_a", (250, 0)), 0, "", 1);
00131:             Assert.Equal(MoralChoiceSystem.MaxScore, sys.MoralScore);
00132:             Assert.Empty(fired); // overflow never lands mid-scene
00133:
00134:             sys.Resolve(Quest("quest_moral_cap_b", (250, 0)), 0, "", 2);
00135:             Assert.Equal(MoralChoiceSystem.MaxScore, sys.MoralScore);
00136:             sys.Reconcile(3);
00137:             Assert.Equal(1, fired.Count(id => id == MoralChoiceSystem.EventLegendPositive));
00138:
00139:             sys.Resolve(Quest("quest_moral_cap_c", (-600, 0)), 0, "", 4);
00140:             sys.Resolve(Quest("quest_moral_cap_d", (-600, 0)), 0, "", 5);
00141:             Assert.Equal(MoralChoiceSystem.MinScore, sys.MoralScore);
00142:             sys.Reconcile(6);
00143:             Assert.Equal(1, fired.Count(id => id == MoralChoiceSystem.EventLegendNegative));
00144:         }
00145:
00146:         [Fact]
00147:         public void ResolveRejectsNonCanonicalQuestId()
00148:         {
00149:             var sys = Sys();
00150:             Assert.Throws<ArgumentException>(() => sys.Resolve(Quest("qst_moral_share_child"), 0, "", 1));
00151:         }
00152:
00153:         [Fact]
00154:         public void ResolveRejectsOutOfRangeChoice()
00155:         {
00156:             var sys = Sys();
00157:             var quest = Quest();
00158:             Assert.Throws<ArgumentOutOfRangeException>(() => sys.Resolve(quest, -1, "", 1));
00159:             Assert.Throws<ArgumentOutOfRangeException>(() => sys.Resolve(quest, 4, "", 1));
00160:         }
00161:
00162:         [Fact]
00163:         public void ImpactMarksFollowDeltaSign()
00164:         {
00165:             var sys = Sys();
00166:             Assert.Equal("up", sys.Resolve(Quest("quest_moral_up", (7, 0)), 0, "", 1).impactMark);
00167:             Assert.Equal("flat", sys.Resolve(Quest("quest_moral_flat", (0, 0)), 0, "", 2).impactMark);
00168:             Assert.Equal("down", sys.Resolve(Quest("quest_moral_down", (-7, 0)), 0, "", 3).impactMark);
00169:         }
00170:
00171:         [Fact]
00172:         public void SameSeedSameRolls()
00173:         {
00174:             var ids = new[] { "quest_moral_r1", "quest_moral_r2", "quest_moral_r3" };
00175:             var a = Sys(977);
00176:             var b = Sys(977);
00177:
00178:             for (int i = 0; i < ids.Length; i++)
00179:             {
00180:                 var ra = a.Resolve(Quest(ids[i]), 0, "", i + 1);
00181:                 var rb = b.Resolve(Quest(ids[i]), 0, "", i + 1);
00182:                 Assert.Equal(ra.outcomeRoll, rb.outcomeRoll);
00183:                 Assert.Equal(ra.propagatesOnDay, rb.propagatesOnDay);
00184:             }
00185:         }
00186:
00187:         [Fact]
00188:         public void ReconcileFiresExtremeBandEventsOnce()
00189:         {
00190:             var sys = Sys();
00191:             var fired = new List<string>();
00192:             sys.OnThresholdEventFired += fired.Add;
00193:
00194:             sys.Resolve(Quest("quest_moral_saint", (120, 0)), 0, "", 1);
00195:             sys.Reconcile(2);
00196:             Assert.Contains(MoralChoiceSystem.EventContractRaised, fired);
00197:             Assert.Contains(MoralChoiceSystem.EventPatrolDefense, fired);
00198:
00199:             sys.Reconcile(3);
00200:             sys.Resolve(Quest("quest_moral_fall", (-250, 0)), 0, "", 4);
00201:             sys.Reconcile(5);
00202:             Assert.Contains(MoralChoiceSystem.EventBountyIssued, fired);
00203:
00204:             sys.Resolve(Quest("quest_moral_return", (250, 0)), 0, "", 6);
00205:             sys.Reconcile(7);
00206:
00207:             // Insertion order: crossing Neutral -> VeryPositive settles the
00208:             // Positive contract plus both VeryPositive events; the fall to
00209:             // VeryEvil settles the bounty; the return crossing re-fires nothing.
00210:             Assert.Equal(
00211:                 new[] { MoralChoiceSystem.EventContractTaken, MoralChoiceSystem.EventContractRaised,
00212:                         MoralChoiceSystem.EventPatrolDefense, MoralChoiceSystem.EventBountyIssued },
00213:                 fired);
00214:         }
00215:
00216:         [Fact]
00217:         public void ReconcileFiresAllCrossedBandsOnBigJump()
00218:         {
00219:             var sys = Sys();
00220:             var fired = new List<string>();
00221:             sys.OnThresholdEventFired += fired.Add;
00222:
00223:             sys.Resolve(Quest("quest_moral_deep", (-150, 0)), 0, "", 1);
00224:             sys.Reconcile(2);
00225:             Assert.Equal(new[] { MoralChoiceSystem.EventBountyIssued }, fired);
00226:
00227:             sys.Resolve(Quest("quest_moral_swing", (250, 0)), 0, "", 3);
00228:             sys.Reconcile(4);
00229:             Assert.Equal(
00230:                 new[] { MoralChoiceSystem.EventBountyIssued, MoralChoiceSystem.EventContractTaken,
00231:                         MoralChoiceSystem.EventContractRaised, MoralChoiceSystem.EventPatrolDefense },
00232:                 fired);
00233:         }
00234:
00235:         [Fact]
00236:         public void ReconcileFiresContractAtPositiveBand()
00237:         {
00238:             var sys = Sys();
00239:             var fired = new List<string>();
00240:             sys.OnThresholdEventFired += fired.Add;
00241:
00242:             sys.Resolve(Quest("quest_moral_savior", (60, 0)), 0, "", 1);
00243:             sys.Reconcile(2);
00244:
00245:             Assert.Equal(new[] { MoralChoiceSystem.EventContractTaken }, fired);
00246:         }
00247:
00248:         [Fact]
00249:         public void ReconcileIgnoresOutOfOrderDays()
00250:         {
00251:             var sys = Sys();
00252:             sys.Reconcile(10);
00253:             sys.Reconcile(5);
00254:             Assert.Equal(10, sys.State.lastReconciledDay);
00255:         }
00256:
00257:         [Fact]
00258:         public void EndingStorykeeperOverridesBand()
00259:         {
00260:             var sys = Sys(31);
00261:             for (int i = 0; i < 25; i++)
00262:             {
00263:                 sys.Resolve(Quest($"quest_moral_arc_{i}", (-8, 2)), 0, "loc_arc", i + 1);
00264:             }
00265:
00266:             Assert.Equal(MoralChoiceSystem.MinScore, sys.MoralScore);
00267:             Assert.Equal(50, sys.EmpathyPoints);
00268:             Assert.Equal(MoralPathBand.VeryEvil, sys.CurrentBand);
00269:             Assert.Equal(MoralEndingKind.Storykeeper, sys.SelectEnding());
00270:         }
00271:
00272:         [Fact]
00273:         public void EndingSelectionRules()
00274:         {
00275:             var cases = new (int Score, int Quests, MoralEndingKind Expected)[]
00276:             {
00277:                 (150, 19, MoralEndingKind.CommunityBuilder),
00278:                 (-150, 19, MoralEndingKind.NeutralSurvivor),
00279:                 (0, 19, MoralEndingKind.BalancedSurvivor),
00280:                 (150, 20, MoralEndingKind.SaintOfWasteland),
00281:                 (-150, 25, MoralEndingKind.Warlord),
00282:                 (-60, 25, MoralEndingKind.SurvivorKing),
00283:                 (-10, 20, MoralEndingKind.NeutralSurvivor),
00284:                 (0, 20, MoralEndingKind.BalancedSurvivor),
00285:                 (30, 20, MoralEndingKind.CommunityBuilder),
00286:                 (55, 22, MoralEndingKind.Savior),
00287:                 (150, 30, MoralEndingKind.SaintOfWasteland)
00288:             };
00289:             var failures = new List<string>();
00290:
00291:             foreach (var testCase in cases)
00292:             {
00293:                 var actual = MoralChoiceSystem.SelectEnding(testCase.Score, 0, testCase.Quests);
00294:                 if (actual != testCase.Expected)
00295:                 {
00296:                     failures.Add($"score {testCase.Score}, quests {testCase.Quests}: expected {testCase.Expected}, got {actual}");
00297:                 }
00298:             }
00299:
00300:             Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
00301:         }
00302:
00303:         [Fact]
00304:         public void StorykeeperNeedsBothThresholds()
00305:         {
00306:             Assert.Equal(MoralEndingKind.Warlord, MoralChoiceSystem.SelectEnding(-150, 45, 24));
00307:             Assert.Equal(MoralEndingKind.Warlord, MoralChoiceSystem.SelectEnding(-150, 44, 25));
00308:             Assert.Equal(MoralEndingKind.Storykeeper, MoralChoiceSystem.SelectEnding(-150, 45, 25));
00309:         }
00310:
00311:         [Fact]
00312:         public void ListenerAndConfidantThresholdsPinnedAtBoundary()
00313:         {
00314:             var sys = Sys();
00315:             sys.Resolve(Quest("quest_moral_em14", (0, 14)), 0, "", 1);
00316:             Assert.False(sys.IsListener);
00317:
00318:             sys.Resolve(Quest("quest_moral_em15", (0, 1)), 0, "", 2);
00319:             Assert.True(sys.IsListener);
00320:             Assert.False(sys.IsConfidant);
00321:
00322:             sys.Resolve(Quest("quest_moral_em29", (0, 14)), 0, "", 3);
00323:             Assert.False(sys.IsConfidant);
00324:
00325:             sys.Resolve(Quest("quest_moral_em30", (0, 1)), 0, "", 4);
00326:             Assert.True(sys.IsConfidant);
00327:         }
00328:
00329:         [Fact]
00330:         public void RestoreRejectsMismatchedSystemAndBadSchema()
00331:         {
00332:             var sys = Sys();
00333:             Assert.Throws<ArgumentException>(() =>
00334:                 sys.RestoreState(new MoralChoiceState { systemId = "other_system", schemaVersion = 1 }));
00335:
00336:             var future = sys.CaptureState();
00337:             future.schemaVersion = 2;
00338:             Assert.Throws<NotSupportedException>(() => sys.RestoreState(future));
00339:
00340:             var malformed = sys.CaptureState();
00341:             malformed.schemaVersion = 0;
00342:             Assert.Throws<ArgumentException>(() => sys.RestoreState(malformed));
00343:         }
00344:
00345:         [Fact]
00346:         public void PendingLegendFlagsSurviveRoundTrip()
00347:         {
00348:             var sys = Sys();
00349:             sys.Resolve(Quest("quest_moral_overflow", (500, 0)), 0, "", 1);
00350:             Assert.Equal(MoralChoiceSystem.LegendPositiveFlag, sys.State.pendingLegendFlags);
00351:
00352:             var restored = Sys(99);
00353:             restored.RestoreState(sys.CaptureState());
00354:             Assert.Equal(MoralChoiceSystem.LegendPositiveFlag, restored.State.pendingLegendFlags);
00355:
00356:             var fired = new List<string>();
00357:             restored.OnThresholdEventFired += fired.Add;
00358:             restored.Reconcile(2);
00359:             Assert.Contains(MoralChoiceSystem.EventLegendPositive, fired);
00360:             Assert.Equal(0, restored.State.pendingLegendFlags);
00361:         }
00362:
00363:         [Fact]
00364:         public void SaveRoundTripPreservesLedger()
00365:         {
00366:             var sys = Sys(7);
00367:             sys.Resolve(Quest("quest_moral_a", (30, 3)), 0, "loc_a", 5);
00368:             sys.Resolve(Quest("quest_moral_b", (30, 3)), 0, "loc_b", 6);
00369:             sys.Resolve(Quest("quest_moral_c", (250, 4)), 0, "loc_c", 7);
00370:             sys.Reconcile(8);
00371:             var snap = sys.CaptureState();
00372:
00373:             var restored = Sys(1234);
00374:             restored.RestoreState(snap);
00375:
00376:             Assert.Equal(sys.MoralScore, restored.MoralScore);
00377:             Assert.Equal(sys.EmpathyPoints, restored.EmpathyPoints);
00378:             Assert.Equal(3, restored.QuestsResolved);
00379:             Assert.Equal(sys.State.lastReconciledDay, restored.State.lastReconciledDay);
00380:             Assert.Equal(sys.State.bandAtLastReconcile, restored.State.bandAtLastReconcile);
00381:             Assert.Equal(sys.State.firedThresholdEvents, restored.State.firedThresholdEvents);
00382:             for (int i = 0; i < snap.resolutions.Count; i++)
00383:             {
00384:                 Assert.Equal(snap.resolutions[i].questId, restored.Resolutions[i].questId);
00385:                 Assert.Equal(snap.resolutions[i].outcomeRoll, restored.Resolutions[i].outcomeRoll);
00386:                 Assert.Equal(snap.resolutions[i].propagatesOnDay, restored.Resolutions[i].propagatesOnDay);
00387:                 Assert.Equal(snap.resolutions[i].impactMark, restored.Resolutions[i].impactMark);
00388:             }
00389:
00390:             restored.Resolve(Quest("quest_moral_a", (30, 3)), 0, "loc_a", 99);
00391:             Assert.Equal(sys.MoralScore, restored.MoralScore);
00392:             Assert.Equal(3, restored.QuestsResolved);
00393:         }
00394:
00395:         [Fact]
00396:         public void CapturedStateIsDetached()
00397:         {
00398:             var sys = Sys();
00399:             sys.Resolve(Quest("quest_moral_a", (30, 3)), 0, "loc_a", 5);
00400:             var snap = sys.CaptureState();
00401:
00402:             snap.resolutions.Clear();
00403:             snap.firedThresholdEvents.Clear();
00404:             snap.moralScore = -77;
00405:
00406:             Assert.Equal(1, sys.QuestsResolved);
00407:             Assert.Equal(30, sys.MoralScore);
00408:         }
00409:
00410:         [Fact]
00411:         public void AvailabilityWindow()
00412:         {
00413:             var windowed = new MoralChoiceQuestDefinition { Id = "quest_moral_window", MinDay = 10, MaxDay = 30 };
00414:             Assert.False(MoralChoiceSystem.IsAvailableOnDay(windowed, 9));
00415:             Assert.True(MoralChoiceSystem.IsAvailableOnDay(windowed, 10));
00416:             Assert.True(MoralChoiceSystem.IsAvailableOnDay(windowed, 30));
00417:             Assert.False(MoralChoiceSystem.IsAvailableOnDay(windowed, 31));
00418:
00419:             var openEnded = new MoralChoiceQuestDefinition { Id = "quest_moral_late", MinDay = 200, MaxDay = 0 };
00420:             Assert.False(MoralChoiceSystem.IsAvailableOnDay(openEnded, 199));
00421:             Assert.True(MoralChoiceSystem.IsAvailableOnDay(openEnded, 200));
00422:             Assert.True(MoralChoiceSystem.IsAvailableOnDay(openEnded, 5000));
00423:
00424:             var malformed = new MoralChoiceQuestDefinition { Id = "quest_moral_bad", MinDay = 30, MaxDay = 10 };
00425:             Assert.False(MoralChoiceSystem.IsAvailableOnDay(malformed, 20));
00426:         }
00427:     }
00428: }
```

## `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs` — 681 lines; 29,774 bytes; SHA-256 `52feaf16447c30403ed9a14ec9a3c991dfb6d2c0ec873633e0821e42cf5d1df3`
Declaration index:
- 00012: public sealed class MoralChoiceBranchGossipTests : CatalogTestBase
- 00020: public void ChainCatalogLoadsFourBranches()
- 00031: public void ChainCatalogHasQuestGates()
- 00043: public void ChainCatalogHasEchoQuests()
- 00056: public void ChainCatalogLockoutRulesArePermanent()
- 00064: public void ChainCatalogMissingFileReturnsEmpty()
- 00074: public void BranchingQuestsLoadAllFourChains()
- 00085: public void BranchingQuestsAllChainsComplete()
- 00099: public void BranchingQuestsHaveValidChoices()
- 00118: public void ExpansionQuestsLoadFiftyQuests()
- 00126: public void ExpansionQuestIdsMatchStaticList()
- 00138: public void GossipCatalogLoadsAllBands()
- 00149: public void GossipCatalogHasNpcGreetings()
- 00158: public void GossipCatalogHasDecayRules()
- 00169: public void FactionReactionsLoadAllThresholdEvents()
- 00180: public void FactionReactionsHaveDialogue()
- 00192: public void FlagCatalogLoadsTwentyFiveFlags()
- 00204: public void FlagCatalogIdsMatchStaticList()
- 00219: public void BranchTracking_LocksOutOpposingBranches()
- 00270: public void BranchTracking_LockedBranchBlocksAccessibility()
- 00284: public void BranchTracking_GateRequiresMoralThreshold()
- 00313: public void BranchTracking_GateRequiresPriorQuestResolved()
- 00342: public void BranchTracking_BranchLockFlagsAreSet()
- 00358: public void EchoQuests_AvailableAfterTriggerAndDelay()
- 00385: public void EchoQuests_NotAvailableForWrongChoice()
- 00410: public void EchoQuests_MarkFiredPreventsRefire()
- 00434: public void GossipRuntime_ReturnsCorrectBandChatter()
- 00450: public void GossipRuntime_PickReturnsNonEmpty()
- 00461: public void GossipRuntime_DecayToNeutralAfterFullDecay()
- 00487: public void GossipRuntime_DecayOneLevelAfterInterval()
- 00519: public void GossipRuntime_StaysNeutralBeforePropagation()
- 00562: public void SaveRoundTrip_PreservesBranchTracking()
- 00588: public void StaticIds_AllChainHasOneHundredEntries()
- 00598: public void StaticIds_AllExpansionHasFiftyEntries()
- 00606: public void StaticIds_AllFlagsHasTwentySixEntries()
- 00614: public void StaticIds_AllBranchesHasFourEntries()
- 00620: public void StaticIds_ChainQuestsFollowNamingPattern()
- 00634: private static List<MoralChoiceOption> MakeChoices(params (int moral, int empathy)[] deltas)
- 00644: private static List<MoralChoiceOption> MakeChoices2(int m1, int e1) =>
- 00647: private static List<MoralChoiceOption> MakeChoices4(int m1, int e1, int m2, int e2, int m3, int e3, int m4, int e4) =>
- 00650: private static void ResolveEntryQuests(MoralChoiceSystem sys, string chain, int count)
- 00676: private static void ResolveChainQuests(MoralChoiceSystem sys, string chain, int count)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Xunit;
00007: using Ashfall.Core;
00008: using Ashfall.Core.MoralChoice;
00009:
00010: namespace Ashfall.Core.Tests
00011: {
00012:     public sealed class MoralChoiceBranchGossipTests : CatalogTestBase
00013:     {
00014:         private static readonly IFileIO s_files = new FileSystemIO();
00015:         private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
00016:
00017:         // ── Chain catalog loader ────────────────────────────────────────
00018:
00019:         [Fact]
00020:         public void ChainCatalogLoadsFourBranches()
00021:         {
00022:             var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00023:             Assert.Equal(4, data.Branches.Count);
00024:             Assert.Contains(data.Branches, b => b.Id == "branch_mercy_road");
00025:             Assert.Contains(data.Branches, b => b.Id == "branch_iron_way");
00026:             Assert.Contains(data.Branches, b => b.Id == "branch_listener_thread");
00027:             Assert.Contains(data.Branches, b => b.Id == "branch_broken_compact");
00028:         }
00029:
00030:         [Fact]
00031:         public void ChainCatalogHasQuestGates()
00032:         {
00033:             var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00034:             Assert.NotEmpty(data.QuestGates);
00035:             Assert.All(data.QuestGates, g =>
00036:             {
00037:                 Assert.False(string.IsNullOrWhiteSpace(g.QuestId));
00038:                 Assert.False(string.IsNullOrWhiteSpace(g.Branch));
00039:             });
00040:         }
00041:
00042:         [Fact]
00043:         public void ChainCatalogHasEchoQuests()
00044:         {
00045:             var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00046:             Assert.NotEmpty(data.EchoQuests);
00047:             Assert.All(data.EchoQuests, e =>
00048:             {
00049:                 Assert.False(string.IsNullOrWhiteSpace(e.QuestId));
00050:                 Assert.False(string.IsNullOrWhiteSpace(e.TriggeredBy));
00051:                 Assert.True(e.MinDaysAfter > 0);
00052:             });
00053:         }
00054:
00055:         [Fact]
00056:         public void ChainCatalogLockoutRulesArePermanent()
00057:         {
00058:             var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00059:             Assert.True(data.LockoutRules.LockoutIsPermanent);
00060:             Assert.True(data.LockoutRules.LockoutFiresJournalEntry);
00061:         }
00062:
00063:         [Fact]
00064:         public void ChainCatalogMissingFileReturnsEmpty()
00065:         {
00066:             var data = MoralChoiceChainCatalogLoader.Load("/no/such/dir", s_files, s_json);
00067:             Assert.Empty(data.Branches);
00068:             Assert.Empty(data.QuestGates);
00069:         }
00070:
00071:         // ── Branching quest catalog loader ──────────────────────────────
00072:
00073:         [Fact]
00074:         public void BranchingQuestsLoadAllFourChains()
00075:         {
00076:             var quests = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
00077:             Assert.Equal(100, quests.Count);
00078:             Assert.Equal(25, quests.Count(q => q.Id.StartsWith("quest_moral_chain_mercy_")));
00079:             Assert.Equal(25, quests.Count(q => q.Id.StartsWith("quest_moral_chain_iron_")));
00080:             Assert.Equal(25, quests.Count(q => q.Id.StartsWith("quest_moral_chain_listen_")));
00081:             Assert.Equal(25, quests.Count(q => q.Id.StartsWith("quest_moral_chain_betray_")));
00082:         }
00083:
00084:         [Fact]
00085:         public void BranchingQuestsAllChainsComplete()
00086:         {
00087:             var quests = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
00088:             Assert.Equal(100, quests.Count);
00089:             foreach (string prefix in new[] { "mercy", "iron", "listen", "betray" })
00090:             {
00091:                 for (int i = 1; i <= 25; i++)
00092:                 {
00093:                     Assert.Contains(quests, q => q.Id == $"quest_moral_chain_{prefix}_{i:D2}");
00094:                 }
00095:             }
00096:         }
00097:
00098:         [Fact]
00099:         public void BranchingQuestsHaveValidChoices()
00100:         {
00101:             var quests = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
00102:             Assert.All(quests, q =>
00103:             {
00104:                 Assert.InRange(q.Choices.Count, 3, 4);
00105:                 Assert.False(string.IsNullOrWhiteSpace(q.DisplayName));
00106:                 Assert.False(string.IsNullOrWhiteSpace(q.Discovery));
00107:                 foreach (var c in q.Choices)
00108:                 {
00109:                     Assert.False(string.IsNullOrWhiteSpace(c.Label));
00110:                     Assert.False(string.IsNullOrWhiteSpace(c.Epitaph));
00111:                 }
00112:             });
00113:         }
00114:
00115:         // ── Expansion quest catalog loader ──────────────────────────────
00116:
00117:         [Fact]
00118:         public void ExpansionQuestsLoadFiftyQuests()
00119:         {
00120:             var quests = MoralChoiceExpansionQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
00121:             Assert.Equal(50, quests.Count);
00122:             Assert.All(quests, q => Assert.StartsWith("quest_moral_", q.Id));
00123:         }
00124:
00125:         [Fact]
00126:         public void ExpansionQuestIdsMatchStaticList()
00127:         {
00128:             var quests = MoralChoiceExpansionQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
00129:             var catalogIds = quests.Select(q => q.Id).ToHashSet();
00130:             var staticIds = MoralChoiceIds.AllExpansion.ToHashSet();
00131:             Assert.True(catalogIds.SetEquals(staticIds),
00132:                 "Expansion catalog and MoralChoiceIds.AllExpansion must match");
00133:         }
00134:
00135:         // ── Gossip catalog loader ───────────────────────────────────────
00136:
00137:         [Fact]
00138:         public void GossipCatalogLoadsAllBands()
00139:         {
00140:             var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
00141:             Assert.NotEmpty(data.CampChatter.VeryPositive);
00142:             Assert.NotEmpty(data.CampChatter.Positive);
00143:             Assert.NotEmpty(data.CampChatter.Neutral);
00144:             Assert.NotEmpty(data.CampChatter.Evil);
00145:             Assert.NotEmpty(data.CampChatter.VeryEvil);
00146:         }
00147:
00148:         [Fact]
00149:         public void GossipCatalogHasNpcGreetings()
00150:         {
00151:             var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
00152:             Assert.NotEmpty(data.NpcGreetingShifts.VeryPositive);
00153:             Assert.NotEmpty(data.NpcGreetingShifts.Neutral);
00154:             Assert.NotEmpty(data.NpcGreetingShifts.VeryEvil);
00155:         }
00156:
00157:         [Fact]
00158:         public void GossipCatalogHasDecayRules()
00159:         {
00160:             var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
00161:             Assert.Equal(30, data.GossipDecay.DecayIntervalDays);
00162:             Assert.Equal(60, data.GossipDecay.FullDecayDays);
00163:             Assert.Equal(10, data.GossipDecay.DramaticResetThreshold);
00164:         }
00165:
00166:         // ── Faction reactions catalog loader ────────────────────────────
00167:
00168:         [Fact]
00169:         public void FactionReactionsLoadAllThresholdEvents()
00170:         {
00171:             var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
00172:             Assert.Contains("moral_event_bounty_issued", data.ThresholdReactions.Keys);
00173:             Assert.Contains("moral_event_contract_taken", data.ThresholdReactions.Keys);
00174:             Assert.Contains("moral_event_contract_raised", data.ThresholdReactions.Keys);
00175:             Assert.Contains("moral_event_legend_positive", data.ThresholdReactions.Keys);
00176:             Assert.Contains("moral_event_legend_negative", data.ThresholdReactions.Keys);
00177:         }
00178:
00179:         [Fact]
00180:         public void FactionReactionsHaveDialogue()
00181:         {
00182:             var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
00183:             var bounty = data.ThresholdReactions["moral_event_bounty_issued"];
00184:             Assert.NotEmpty(bounty.PeacekeeperDialogue);
00185:             Assert.NotEmpty(bounty.RaiderDialogue);
00186:             Assert.False(string.IsNullOrWhiteSpace(bounty.JournalEntry));
00187:         }
00188:
00189:         // ── Flag catalog loader ─────────────────────────────────────────
00190:
00191:         [Fact]
00192:         public void FlagCatalogLoadsTwentyFiveFlags()
00193:         {
00194:             var data = MoralChoiceFlagCatalogLoader.Load(DataDirectory, s_files, s_json);
00195:             Assert.Equal(25, data.Flags.Count);
00196:             Assert.All(data.Flags, f =>
00197:             {
00198:                 Assert.StartsWith("flag_", f.Id);
00199:                 Assert.False(string.IsNullOrWhiteSpace(f.DisplayName));
00200:             });
00201:         }
00202:
00203:         [Fact]
00204:         public void FlagCatalogIdsMatchStaticList()
00205:         {
00206:             var data = MoralChoiceFlagCatalogLoader.Load(DataDirectory, s_files, s_json);
00207:             var catalogFlagIds = data.Flags.Select(f => f.Id).ToHashSet();
00208:             // AllFlags has 11 entries (10 from JSON + FlagMessengerKept from code).
00209:             // The JSON flags should be a subset of AllFlags.
00210:             foreach (var id in catalogFlagIds)
00211:             {
00212:                 Assert.Contains(id, MoralChoiceIds.AllFlags);
00213:             }
00214:         }
00215:
00216:         // ── Branch tracking in MoralChoiceSystem ────────────────────────
00217:
00218:         [Fact]
00219:         public void BranchTracking_LocksOutOpposingBranches()
00220:         {
00221:             var sys = new MoralChoiceSystem(new SeededRng(42));
00222:             var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00223:             sys.InitializeChainData(chainData);
00224:
00225:             var lockedBranches = new List<string>();
00226:             sys.OnBranchLocked += lockedBranches.Add;
00227:
00228:             // Resolve 3 Mercy Road entry quests → should lock Iron Way + Broken Compact
00229:             var mercyEntry = new MoralChoiceQuestDefinition
00230:             {
00231:                 Id = "quest_moral_chain_mercy_01",
00232:                 DisplayName = "test",
00233:                 Category = "share",
00234:                 Choices = MakeChoices2(10, 2)
00235:             };
00236:             sys.Resolve(mercyEntry, 0, "", 10);
00237:
00238:             var mercyEntry2 = new MoralChoiceQuestDefinition
00239:             {
00240:                 Id = "quest_moral_chain_mercy_02",
00241:                 DisplayName = "test",
00242:                 Category = "comfort",
00243:                 Choices = MakeChoices2(10, 2)
00244:             };
00245:             sys.Resolve(mercyEntry2, 0, "", 20);
00246:
00247:             Assert.Equal(2, sys.GetBranchProgress(MoralChoiceIds.BranchMercyRoad));
00248:             Assert.False(sys.IsBranchLocked(MoralChoiceIds.BranchIronWay));
00249:
00250:             var mercyEntry3 = new MoralChoiceQuestDefinition
00251:             {
00252:                 Id = "quest_moral_chain_mercy_03",
00253:                 DisplayName = "test",
00254:                 Category = "share",
00255:                 Choices = MakeChoices2(10, 2)
00256:             };
00257:             sys.Resolve(mercyEntry3, 0, "", 30);
00258:
00259:             Assert.Equal(3, sys.GetBranchProgress(MoralChoiceIds.BranchMercyRoad));
00260:             Assert.True(sys.IsBranchLocked(MoralChoiceIds.BranchIronWay));
00261:             Assert.True(sys.IsBranchLocked(MoralChoiceIds.BranchBrokenCompact));
00262:             Assert.False(sys.IsBranchLocked(MoralChoiceIds.BranchMercyRoad));
00263:             Assert.False(sys.IsBranchLocked(MoralChoiceIds.BranchListenerThread));
00264:
00265:             Assert.Contains(MoralChoiceIds.BranchIronWay, lockedBranches);
00266:             Assert.Contains(MoralChoiceIds.BranchBrokenCompact, lockedBranches);
00267:         }
00268:
00269:         [Fact]
00270:         public void BranchTracking_LockedBranchBlocksAccessibility()
00271:         {
00272:             var sys = new MoralChoiceSystem(new SeededRng(42));
00273:             var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00274:             sys.InitializeChainData(chainData);
00275:
00276:             // Lock Iron Way by resolving 3 mercy entry quests
00277:             ResolveEntryQuests(sys, "mercy", 3);
00278:
00279:             Assert.True(sys.IsBranchLocked(MoralChoiceIds.BranchIronWay));
00280:             Assert.False(sys.IsChainQuestAccessible("quest_moral_chain_iron_01", 100));
00281:         }
00282:
00283:         [Fact]
00284:         public void BranchTracking_GateRequiresMoralThreshold()
00285:         {
00286:             var sys = new MoralChoiceSystem(new SeededRng(42));
00287:             var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00288:             sys.InitializeChainData(chainData);
00289:
00290:             // quest_moral_chain_mercy_04 requires min_moral 15 and prior quest_03
00291:             // Without resolving prerequisites, gate should fail
00292:             Assert.False(sys.IsChainQuestAccessible("quest_moral_chain_mercy_04", 100));
00293:
00294:             // Resolve prerequisites and boost moral score
00295:             ResolveEntryQuests(sys, "mercy", 3);
00296:             ResolveChainQuests(sys, "mercy", 3); // resolve 01, 02, 03
00297:
00298:             // Now boost moral to 15+
00299:             var boost = new MoralChoiceQuestDefinition
00300:             {
00301:                 Id = "quest_moral_boost",
00302:                 DisplayName = "boost",
00303:                 Category = "share",
00304:                 Choices = MakeChoices2(20, 0)
00305:             };
00306:             sys.Resolve(boost, 0, "", 50);
00307:
00308:             // Gate should now pass (moral >= 15, prerequisites resolved)
00309:             Assert.True(sys.IsChainQuestAccessible("quest_moral_chain_mercy_04", 100));
00310:         }
00311:
00312:         [Fact]
00313:         public void BranchTracking_GateRequiresPriorQuestResolved()
00314:         {
00315:             var sys = new MoralChoiceSystem(new SeededRng(42));
00316:             var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00317:             sys.InitializeChainData(chainData);
00318:
00319:             // quest_moral_chain_mercy_04 requires quest_moral_chain_mercy_03 resolved
00320:             // Without resolving it, gate should fail even with high moral
00321:             var boost = new MoralChoiceQuestDefinition
00322:             {
00323:                 Id = "quest_moral_gate_boost",
00324:                 DisplayName = "boost",
00325:                 Category = "share",
00326:                 Choices = MakeChoices2(50, 0)
00327:             };
00328:             sys.Resolve(boost, 0, "", 1);
00329:             Assert.True(sys.MoralScore >= 15);
00330:
00331:             // Gate should fail because prerequisite quest not resolved
00332:             Assert.False(sys.IsChainQuestAccessible("quest_moral_chain_mercy_04", 100));
00333:
00334:             // Now resolve the prerequisite chain (entry quests 01-03)
00335:             ResolveEntryQuests(sys, "mercy", 3);
00336:
00337:             // Gate should now pass (prerequisites resolved + moral >= 15)
00338:             Assert.True(sys.IsChainQuestAccessible("quest_moral_chain_mercy_04", 100));
00339:         }
00340:
00341:         [Fact]
00342:         public void BranchTracking_BranchLockFlagsAreSet()
00343:         {
00344:             var sys = new MoralChoiceSystem(new SeededRng(42));
00345:             var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00346:             sys.InitializeChainData(chainData);
00347:
00348:             ResolveEntryQuests(sys, "mercy", 3);
00349:
00350:             Assert.True(sys.HasFlag(MoralChoiceIds.FlagIronWayLocked));
00351:             Assert.True(sys.HasFlag(MoralChoiceIds.FlagBrokenCompactLocked));
00352:             Assert.False(sys.HasFlag(MoralChoiceIds.FlagMercyRoadLocked));
00353:         }
00354:
00355:         // ── Echo quest availability ─────────────────────────────────────
00356:
00357:         [Fact]
00358:         public void EchoQuests_AvailableAfterTriggerAndDelay()
00359:         {
00360:             var sys = new MoralChoiceSystem(new SeededRng(42));
00361:             var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00362:             sys.InitializeChainData(chainData);
00363:
00364:             // Resolve quest_moral_share_child with choice 0 (best option)
00365:             var childQuest = new MoralChoiceQuestDefinition
00366:             {
00367:                 Id = MoralChoiceIds.ShareChild,
00368:                 DisplayName = "test",
00369:                 Category = "share",
00370:                 Choices = MakeChoices2(10, 2)
00371:             };
00372:             sys.Resolve(childQuest, 0, "loc_test", 10);
00373:
00374:             // Echo quest_moral_echo_child_returns needs choice 0 + 30 days
00375:             // At day 30 (10 + 20), should NOT be available yet
00376:             var available = sys.FindAvailableEchoQuests(30);
00377:             Assert.DoesNotContain(available, e => e.QuestId == "quest_moral_echo_child_returns");
00378:
00379:             // At day 40 (10 + 30), should be available
00380:             available = sys.FindAvailableEchoQuests(40);
00381:             Assert.Contains(available, e => e.QuestId == "quest_moral_echo_child_returns");
00382:         }
00383:
00384:         [Fact]
00385:         public void EchoQuests_NotAvailableForWrongChoice()
00386:         {
00387:             var sys = new MoralChoiceSystem(new SeededRng(42));
00388:             var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00389:             sys.InitializeChainData(chainData);
00390:
00391:             // Resolve quest_moral_share_child with choice 3 (refuse)
00392:             var childQuest = new MoralChoiceQuestDefinition
00393:             {
00394:                 Id = MoralChoiceIds.ShareChild,
00395:                 DisplayName = "test",
00396:                 Category = "share",
00397:                 Choices = MakeChoices4(10, 2, 5, 1, 0, 0, -5, 0)
00398:             };
00399:             sys.Resolve(childQuest, 3, "loc_test", 10);
00400:
00401:             // Echo child_returns needs choice 0 — should NOT fire for choice 3
00402:             var available = sys.FindAvailableEchoQuests(100);
00403:             Assert.DoesNotContain(available, e => e.QuestId == "quest_moral_echo_child_returns");
00404:
00405:             // Echo child_steals needs choice 3 — should fire
00406:             Assert.Contains(available, e => e.QuestId == "quest_moral_echo_child_steals");
00407:         }
00408:
00409:         [Fact]
00410:         public void EchoQuests_MarkFiredPreventsRefire()
00411:         {
00412:             var sys = new MoralChoiceSystem(new SeededRng(42));
00413:             var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00414:             sys.InitializeChainData(chainData);
00415:
00416:             var childQuest = new MoralChoiceQuestDefinition
00417:             {
00418:                 Id = MoralChoiceIds.ShareChild,
00419:                 DisplayName = "test",
00420:                 Category = "share",
00421:                 Choices = MakeChoices2(10, 2)
00422:             };
00423:             sys.Resolve(childQuest, 0, "loc_test", 10);
00424:
00425:             sys.MarkEchoQuestFired("quest_moral_echo_child_returns");
00426:
00427:             var available = sys.FindAvailableEchoQuests(100);
00428:             Assert.DoesNotContain(available, e => e.QuestId == "quest_moral_echo_child_returns");
00429:         }
00430:
00431:         // ── Gossip runtime ──────────────────────────────────────────────
00432:
00433:         [Fact]
00434:         public void GossipRuntime_ReturnsCorrectBandChatter()
00435:         {
00436:             var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
00437:             var runtime = new MoralChoiceGossipRuntime(data, new SeededRng(42));
00438:
00439:             var positive = runtime.GetCampChatter(MoralPathBand.VeryPositive);
00440:             Assert.NotEmpty(positive);
00441:
00442:             var evil = runtime.GetCampChatter(MoralPathBand.VeryEvil);
00443:             Assert.NotEmpty(evil);
00444:
00445:             var neutral = runtime.GetCampChatter(MoralPathBand.Neutral);
00446:             Assert.NotEmpty(neutral);
00447:         }
00448:
00449:         [Fact]
00450:         public void GossipRuntime_PickReturnsNonEmpty()
00451:         {
00452:             var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
00453:             var runtime = new MoralChoiceGossipRuntime(data, new SeededRng(42));
00454:
00455:             Assert.False(string.IsNullOrWhiteSpace(runtime.PickCampChatter(MoralPathBand.Positive)));
00456:             Assert.False(string.IsNullOrWhiteSpace(runtime.PickNpcGreeting(MoralPathBand.Evil)));
00457:             Assert.False(string.IsNullOrWhiteSpace(runtime.PickWhisper(MoralPathBand.VeryEvil)));
00458:         }
00459:
00460:         [Fact]
00461:         public void GossipRuntime_DecayToNeutralAfterFullDecay()
00462:         {
00463:             var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
00464:             var runtime = new MoralChoiceGossipRuntime(data, new SeededRng(42));
00465:
00466:             var sys = new MoralChoiceSystem(new SeededRng(42));
00467:             var quest = new MoralChoiceQuestDefinition
00468:             {
00469:                 Id = "quest_moral_gossip_test",
00470:                 DisplayName = "test",
00471:                 Category = "share",
00472:                 Choices = MakeChoices2(30, 0)
00473:             };
00474:             sys.Resolve(quest, 0, "", 10);
00475:
00476:             Assert.Equal(MoralPathBand.SlightlyPositive, sys.CurrentBand);
00477:
00478:             // After full decay from the day gossip actually propagates (not
00479:             // from resolvedDay — the wasteland hasn't heard yet on day 10),
00480:             // gossip should be neutral.
00481:             int propagatesOnDay = sys.Resolutions[0].propagatesOnDay;
00482:             var effective = runtime.GetEffectiveGossipBand(sys, propagatesOnDay + 60);
00483:             Assert.Equal(MoralPathBand.Neutral, effective);
00484:         }
00485:
00486:         [Fact]
00487:         public void GossipRuntime_DecayOneLevelAfterInterval()
00488:         {
00489:             var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
00490:             var runtime = new MoralChoiceGossipRuntime(data, new SeededRng(42));
00491:
00492:             var sys = new MoralChoiceSystem(new SeededRng(42));
00493:             // Use a small moral delta (< dramatic threshold of 10) so decay kicks in
00494:             var quest = new MoralChoiceQuestDefinition
00495:             {
00496:                 Id = "quest_moral_gossip_decay_test",
00497:                 DisplayName = "test",
00498:                 Category = "share",
00499:                 Choices = new List<MoralChoiceOption>
00500:                 {
00501:                     new() { MoralDelta = 60, EmpathyDelta = 0, Epitaph = "big" },
00502:                     new() { MoralDelta = 8, EmpathyDelta = 0, Epitaph = "small" },
00503:                     new() { MoralDelta = 0, EmpathyDelta = 0, Epitaph = "none" },
00504:                     new() { MoralDelta = -5, EmpathyDelta = 0, Epitaph = "bad" }
00505:                 }
00506:             };
00507:             // Resolve with choice 1 (delta=8, below dramatic threshold of 10)
00508:             sys.Resolve(quest, 1, "", 10);
00509:
00510:             // Score 8 → SlightlyPositive
00511:             Assert.Equal(MoralPathBand.SlightlyPositive, sys.CurrentBand);
00512:
00513:             // After 30 days (decay interval), non-dramatic → decay one level to Neutral
00514:             var effective = runtime.GetEffectiveGossipBand(sys, 10 + 31);
00515:             Assert.Equal(MoralPathBand.Neutral, effective);
00516:         }
00517:
00518:         [Fact]
00519:         public void GossipRuntime_StaysNeutralBeforePropagation()
00520:         {
00521:             // A resolution's consequences must not reach camp chatter before
00522:             // MoralChoiceResolution.propagatesOnDay — resolvedDay + 1..3 with
00523:             // this seed. Checking the very next day (resolvedDay + 1) must
00524:             // still show Neutral if propagatesOnDay lands later than that.
00525:             var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
00526:             var runtime = new MoralChoiceGossipRuntime(data, new SeededRng(42));
00527:
00528:             var sys = new MoralChoiceSystem(new SeededRng(1));
00529:             var quest = new MoralChoiceQuestDefinition
00530:             {
00531:                 Id = "quest_moral_gossip_propagation_test",
00532:                 DisplayName = "test",
00533:                 Category = "share",
00534:                 Choices = MakeChoices2(60, 0)
00535:             };
00536:             sys.Resolve(quest, 0, "", 10);
00537:
00538:             var resolution = sys.Resolutions[0];
00539:             Assert.True(resolution.propagatesOnDay > resolution.resolvedDay,
00540:                 "propagatesOnDay must be strictly after resolvedDay for this test to be meaningful");
00541:
00542:             // The instant the choice resolves, gossip has nothing to work
00543:             // with yet — the wasteland has not heard.
00544:             var immediately = runtime.GetEffectiveGossipBand(sys, resolution.resolvedDay);
00545:             Assert.Equal(MoralPathBand.Neutral, immediately);
00546:
00547:             // One day before propagation (if there's a gap to test), still neutral.
00548:             if (resolution.propagatesOnDay - 1 > resolution.resolvedDay)
00549:             {
00550:                 var stillWaiting = runtime.GetEffectiveGossipBand(sys, resolution.propagatesOnDay - 1);
00551:                 Assert.Equal(MoralPathBand.Neutral, stillWaiting);
00552:             }
00553:
00554:             // On the day it propagates, gossip reflects the actual band.
00555:             var propagated = runtime.GetEffectiveGossipBand(sys, resolution.propagatesOnDay);
00556:             Assert.Equal(sys.CurrentBand, propagated);
00557:         }
00558:
00559:         // ── Save round-trip with new fields ─────────────────────────────
00560:
00561:         [Fact]
00562:         public void SaveRoundTrip_PreservesBranchTracking()
00563:         {
00564:             var sys = new MoralChoiceSystem(new SeededRng(42));
00565:             var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
00566:             sys.InitializeChainData(chainData);
00567:
00568:             ResolveEntryQuests(sys, "mercy", 3);
00569:             sys.SetFlag(MoralChoiceIds.FlagMessengerKept);
00570:             sys.MarkEchoQuestFired("quest_moral_echo_child_returns");
00571:
00572:             var snap = sys.CaptureState();
00573:
00574:             var restored = new MoralChoiceSystem(new SeededRng(99));
00575:             restored.InitializeChainData(chainData);
00576:             restored.RestoreState(snap);
00577:
00578:             Assert.True(restored.IsBranchLocked(MoralChoiceIds.BranchIronWay));
00579:             Assert.True(restored.IsBranchLocked(MoralChoiceIds.BranchBrokenCompact));
00580:             Assert.Equal(3, restored.GetBranchProgress(MoralChoiceIds.BranchMercyRoad));
00581:             Assert.True(restored.HasFlag(MoralChoiceIds.FlagMessengerKept));
00582:             Assert.Contains("quest_moral_echo_child_returns", restored.State.firedEchoQuests);
00583:         }
00584:
00585:         // ── Static IDs ──────────────────────────────────────────────────
00586:
00587:         [Fact]
00588:         public void StaticIds_AllChainHasOneHundredEntries()
00589:         {
00590:             Assert.Equal(100, MoralChoiceIds.AllChain.Length);
00591:             Assert.Equal(25, MoralChoiceIds.ChainMercy.Length);
00592:             Assert.Equal(25, MoralChoiceIds.ChainIron.Length);
00593:             Assert.Equal(25, MoralChoiceIds.ChainListen.Length);
00594:             Assert.Equal(25, MoralChoiceIds.ChainBetray.Length);
00595:         }
00596:
00597:         [Fact]
00598:         public void StaticIds_AllExpansionHasFiftyEntries()
00599:         {
00600:             Assert.Equal(50, MoralChoiceIds.AllExpansion.Length);
00601:             Assert.All(MoralChoiceIds.AllExpansion, id =>
00602:                 Assert.StartsWith("quest_moral_", id));
00603:         }
00604:
00605:         [Fact]
00606:         public void StaticIds_AllFlagsHasTwentySixEntries()
00607:         {
00608:             Assert.Equal(26, MoralChoiceIds.AllFlags.Length);
00609:             Assert.All(MoralChoiceIds.AllFlags, id =>
00610:                 Assert.StartsWith("flag_", id));
00611:         }
00612:
00613:         [Fact]
00614:         public void StaticIds_AllBranchesHasFourEntries()
00615:         {
00616:             Assert.Equal(4, MoralChoiceIds.AllBranches.Length);
00617:         }
00618:
00619:         [Fact]
00620:         public void StaticIds_ChainQuestsFollowNamingPattern()
00621:         {
00622:             Assert.All(MoralChoiceIds.ChainMercy, id =>
00623:                 Assert.Matches(@"^quest_moral_chain_mercy_\d{2}$", id));
00624:             Assert.All(MoralChoiceIds.ChainIron, id =>
00625:                 Assert.Matches(@"^quest_moral_chain_iron_\d{2}$", id));
00626:             Assert.All(MoralChoiceIds.ChainListen, id =>
00627:                 Assert.Matches(@"^quest_moral_chain_listen_\d{2}$", id));
00628:             Assert.All(MoralChoiceIds.ChainBetray, id =>
00629:                 Assert.Matches(@"^quest_moral_chain_betray_\d{2}$", id));
00630:         }
00631:
00632:         // ── Helpers ─────────────────────────────────────────────────────
00633:
00634:         private static List<MoralChoiceOption> MakeChoices(params (int moral, int empathy)[] deltas)
00635:         {
00636:             return deltas.Select(d => new MoralChoiceOption
00637:             {
00638:                 MoralDelta = d.moral,
00639:                 EmpathyDelta = d.empathy,
00640:                 Epitaph = $"chose {d.moral}"
00641:             }).ToList();
00642:         }
00643:
00644:         private static List<MoralChoiceOption> MakeChoices2(int m1, int e1) =>
00645:             MakeChoices((m1, e1), (m1 / 2, e1 > 0 ? e1 - 1 : 0), (0, 0), (-m1 / 2, 0));
00646:
00647:         private static List<MoralChoiceOption> MakeChoices4(int m1, int e1, int m2, int e2, int m3, int e3, int m4, int e4) =>
00648:             MakeChoices((m1, e1), (m2, e2), (m3, e3), (m4, e4));
00649:
00650:         private static void ResolveEntryQuests(MoralChoiceSystem sys, string chain, int count)
00651:         {
00652:             string[] prefixes = { "mercy", "iron", "listen", "betray" };
00653:             string prefix = chain.Length <= 4 ? chain : chain switch
00654:             {
00655:                 "mercy" => "mercy",
00656:                 "iron" => "iron",
00657:                 "listen" => "listen",
00658:                 "betray" => "betray",
00659:                 _ => chain
00660:             };
00661:
00662:             string[] categories = { "share", "comfort", "share" };
00663:             for (int i = 1; i <= count; i++)
00664:             {
00665:                 var quest = new MoralChoiceQuestDefinition
00666:                 {
00667:                     Id = $"quest_moral_chain_{prefix}_{i:D2}",
00668:                     DisplayName = "test",
00669:                     Category = categories[(i - 1) % categories.Length],
00670:                     Choices = MakeChoices4(10, 2, 5, 1, 0, 0, -5, 0)
00671:                 };
00672:                 sys.Resolve(quest, 0, "", i * 10);
00673:             }
00674:         }
00675:
00676:         private static void ResolveChainQuests(MoralChoiceSystem sys, string chain, int count)
00677:         {
00678:             ResolveEntryQuests(sys, chain, count);
00679:         }
00680:     }
00681: }
```

## `Ashfall.Core.Tests/Journeys/MoralChoiceJourneyTests.cs` — 286 lines; 13,186 bytes; SHA-256 `7266e91fecfef7f8858b547f105f7ab7d972c008ce8a6701b6e9c29b8cf5c262`
Declaration index:
- 00016: public sealed class MoralChoiceJourneyTests
- 00019: public void MoralChoiceJourney_Encounter_Resolve_SaveReload_PreventsDuplicateResolution()
- 00114: public void Journey_J1_MoralChoice_DecisionSpine_JournalConsequence_SaveReload_Idempotent()
- 00223: public void MoralChoice_SaveEnvelope_RoundTripsResolvedLedger()
- 00261: public void HostWiring_SaveAllAndProcessFlush_EnrollMoralChoice()
- 00272: private static string? FindSrcRoot()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL Moral Choice Journey & Host-UI Action Path Verification (REM-004 / R07).
00003: using System;
00004: using System.Collections.Generic;
00005: using System.IO;
00006: using System.Linq;
00007: using Ashfall.Core;
00008: using Ashfall.Core.Flags;
00009: using Ashfall.Core.Journal;
00010: using Ashfall.Core.MoralChoice;
00011: using Ashfall.Core.Save;
00012: using Xunit;
00013:
00014: namespace Ashfall.Core.Tests.Journeys
00015: {
00016:     public sealed class MoralChoiceJourneyTests
00017:     {
00018:         [Fact]
00019:         public void MoralChoiceJourney_Encounter_Resolve_SaveReload_PreventsDuplicateResolution()
00020:         {
00021:             var flags = new InMemoryFlagLedger();
00022:             var rng = new SeededRng(20260904);
00023:             var system = new MoralChoiceSystem(rng, flags: flags);
00024:
00025:             // Authored dilemma definition
00026:             var quest = new MoralChoiceQuestDefinition
00027:             {
00028:                 Id = "quest_moral_water_share",
00029:                 DisplayName = "The Thirsty Wanderer",
00030:                 Category = "share",
00031:                 Discovery = "A desperate wanderer collapses against the exterior decontamination hatch, begging for a single canteen of water.",
00032:                 LocationId = "loc_surface_airlock",
00033:                 MinDay = 1,
00034:                 MaxDay = 10,
00035:                 Choices = new List<MoralChoiceOption>
00036:                 {
00037:                     new()
00038:                     {
00039:                         Label = "Share a clean water ration",
00040:                         MoralDelta = 15,
00041:                         EmpathyDelta = 10,
00042:                         OutcomeText = "The wanderer drinks with trembling gratitude and promises to spread word of the shelter's humanity.",
00043:                         Epitaph = "Gave water to the dying."
00044:                     },
00045:                     new()
00046:                     {
00047:                         Label = "Drive them away into the ash storm",
00048:                         MoralDelta = -15,
00049:                         EmpathyDelta = -5,
00050:                         OutcomeText = "The wanderer curses the bunker door before vanishing into the particulate haze.",
00051:                         Epitaph = "Hoarded water behind sealed iron."
00052:                     }
00053:                 }
00054:             };
00055:
00056:             // Track events
00057:             var resolvedEvents = new List<MoralChoiceResolution>();
00058:             system.OnQuestResolved += r => resolvedEvents.Add(r);
00059:
00060:             // Step 1: Initial state verification
00061:             Assert.False(system.IsResolved(quest.Id));
00062:             Assert.Equal(0, system.QuestsResolved);
00063:             Assert.Equal(0, system.MoralScore);
00064:             Assert.Equal(0, system.EmpathyPoints);
00065:             Assert.Equal(MoralPathBand.Neutral, system.CurrentBand);
00066:
00067:             // Step 2: Resolve Option 0 (Compassion / Share)
00068:             var resolution = system.Resolve(quest, choiceIndex: 0, quest.LocationId, day: 1);
00069:
00070:             Assert.NotNull(resolution);
00071:             Assert.Equal(quest.Id, resolution.questId);
00072:             Assert.Equal(0, resolution.choiceIndex);
00073:             Assert.Single(resolvedEvents);
00074:             Assert.True(system.IsResolved(quest.Id));
00075:             Assert.Equal(1, system.QuestsResolved);
00076:             Assert.Equal(15, system.MoralScore);
00077:             Assert.Equal(10, system.EmpathyPoints);
00078:             Assert.Equal(MoralPathBand.SlightlyPositive, system.CurrentBand);
00079:
00080:             // Step 3: Save to state DTO and serialize
00081:             var savedState = system.CaptureState();
00082:             var serializer = new SystemTextJsonSerializer();
00083:             string json = serializer.Serialize(savedState);
00084:             Assert.False(string.IsNullOrWhiteSpace(json));
00085:
00086:             // Step 4: Restore in a fresh system instance
00087:             var reloadedFlags = new InMemoryFlagLedger();
00088:             var reloadedRng = new SeededRng(20260904);
00089:             var reloadedSystem = new MoralChoiceSystem(reloadedRng, flags: reloadedFlags);
00090:             var restoredState = serializer.Deserialize<MoralChoiceState>(json);
00091:             Assert.NotNull(restoredState);
00092:             reloadedSystem.RestoreState(restoredState);
00093:
00094:             // Step 5: Verify restored state integrity
00095:             Assert.True(reloadedSystem.IsResolved(quest.Id));
00096:             Assert.Equal(1, reloadedSystem.QuestsResolved);
00097:             Assert.Equal(15, reloadedSystem.MoralScore);
00098:             Assert.Equal(10, reloadedSystem.EmpathyPoints);
00099:             Assert.Equal(MoralPathBand.SlightlyPositive, reloadedSystem.CurrentBand);
00100:
00101:             // Step 6: Verify lockout against duplicate selection / double application
00102:             // Host pattern: check IsResolved before invoking Resolve
00103:             bool canResolveAgain = !reloadedSystem.IsResolved(quest.Id);
00104:             Assert.False(canResolveAgain, "Host must reject already resolved moral choices");
00105:
00106:             // Even if Resolve is called directly on Core, it returns stored resolution and does not re-apply deltas
00107:             var duplicateRes = reloadedSystem.Resolve(quest, choiceIndex: 1, quest.LocationId, day: 2);
00108:             Assert.Equal(0, duplicateRes.choiceIndex); // Stored original choice preserved
00109:             Assert.Equal(15, reloadedSystem.MoralScore); // Score NOT corrupted by second call
00110:             Assert.Equal(1, reloadedSystem.QuestsResolved); // Count remains exactly 1
00111:         }
00112:
00113:         [Fact]
00114:         public void Journey_J1_MoralChoice_DecisionSpine_JournalConsequence_SaveReload_Idempotent()
00115:         {
00116:             // Journey J1 specification from Flagship Remediation Plan (Section 7):
00117:             // 1. Load one unresolved authored choice.
00118:             // 2. Open decision UI / surface options.
00119:             // 3. Resolve an option.
00120:             // 4. Verify moral state changes.
00121:             // 5. Verify journal consequence.
00122:             // 6. Save.
00123:             // 7. Reload.
00124:             // 8. Verify it cannot resolve twice.
00125:
00126:             var flags = new InMemoryFlagLedger();
00127:             var rng = new SeededRng(20260905);
00128:             var system = new MoralChoiceSystem(rng, flags: flags);
00129:             var journal = new JournalSystem();
00130:
00131:             int journalEntriesCount = 0;
00132:             string? lastJournalEntryText = null;
00133:
00134:             system.OnQuestResolved += r =>
00135:             {
00136:                 string arrow = r.impactMark == "up" ? "🔺" : r.impactMark == "down" ? "🔻" : "⚪";
00137:                 journal.TryAddRawEntry(r.questId, $"{arrow} {r.epitaph}", null!, r.resolvedDay);
00138:                 journalEntriesCount++;
00139:                 lastJournalEntryText = $"{arrow} {r.epitaph}";
00140:             };
00141:
00142:             // 1. Authored choice: The Caloric Deficit
00143:             var quest = new MoralChoiceQuestDefinition
00144:             {
00145:                 Id = "quest_moral_share_child",
00146:                 DisplayName = "The Caloric Deficit",
00147:                 Category = "share",
00148:                 Trigger = "An unaccompanied minor requests rations at a ruin crossing.",
00149:                 Discovery = "A child sits against the concrete. Their caloric math is visibly failing.",
00150:                 LocationId = "loc_sector_ruins",
00151:                 MinDay = 0,
00152:                 MaxDay = 0,
00153:                 Choices = new List<MoralChoiceOption>
00154:                 {
00155:                     new()
00156:                     {
00157:                         Label = "Give all your food",
00158:                         MoralDelta = 10,
00159:                         EmpathyDelta = 1,
00160:                         OutcomeText = "You hand over the rations. The child consumes half and pockets the rest.",
00161:                         Epitaph = "Transferred all rations to the minor. Received charcoal schematic in return."
00162:                     },
00163:                     new()
00164:                     {
00165:                         Label = "Refuse and walk on",
00166:                         MoralDelta = -5,
00167:                         EmpathyDelta = 0,
00168:                         OutcomeText = "You shake your head. The child goes back to conserving energy.",
00169:                         Epitaph = "Refused the minor's requisition. Conserved my own supply."
00170:                     }
00171:                 }
00172:             };
00173:
00174:             // 2. Initial state verification before player decision
00175:             Assert.False(system.IsResolved(quest.Id));
00176:             Assert.Equal(0, system.QuestsResolved);
00177:             Assert.Equal(0, system.MoralScore);
00178:             Assert.Equal(0, system.EmpathyPoints);
00179:             Assert.Equal(0, journalEntriesCount);
00180:
00181:             // 3. Player resolves Option 0 ("Give all your food")
00182:             var resolution = system.Resolve(quest, choiceIndex: 0, quest.LocationId, day: 3);
00183:
00184:             // 4. Verify moral state mutated
00185:             Assert.NotNull(resolution);
00186:             Assert.True(system.IsResolved(quest.Id));
00187:             Assert.Equal(1, system.QuestsResolved);
00188:             Assert.Equal(10, system.MoralScore);
00189:             Assert.Equal(1, system.EmpathyPoints);
00190:             Assert.Equal("up", resolution.impactMark);
00191:
00192:             // 5. Verify journal consequence was recorded
00193:             Assert.Equal(1, journalEntriesCount);
00194:             Assert.NotNull(lastJournalEntryText);
00195:             Assert.StartsWith("🔺", lastJournalEntryText!);
00196:             Assert.Contains("Transferred all rations to the minor", lastJournalEntryText!);
00197:
00198:             // 6. Save state to DTO
00199:             var state = system.CaptureState();
00200:             var serializer = new SystemTextJsonSerializer();
00201:             string json = serializer.Serialize(state);
00202:
00203:             // 7. Reload in clean host session
00204:             var reloadedSystem = new MoralChoiceSystem(new SeededRng(20260905));
00205:             var restoredState = serializer.Deserialize<MoralChoiceState>(json);
00206:             Assert.NotNull(restoredState);
00207:             reloadedSystem.RestoreState(restoredState!);
00208:
00209:             // 8. Verify persistence and impossibility of double resolution
00210:             Assert.True(reloadedSystem.IsResolved(quest.Id));
00211:             Assert.Equal(1, reloadedSystem.QuestsResolved);
00212:             Assert.Equal(10, reloadedSystem.MoralScore);
00213:             Assert.Equal(1, reloadedSystem.EmpathyPoints);
00214:
00215:             // Attempting to resolve again returns stored resolution without mutating scores or firing duplicate events
00216:             var secondRes = reloadedSystem.Resolve(quest, choiceIndex: 1, quest.LocationId, day: 4);
00217:             Assert.Equal(0, secondRes.choiceIndex); // preserved original choice
00218:             Assert.Equal(10, reloadedSystem.MoralScore); // untouched
00219:             Assert.Equal(1, reloadedSystem.QuestsResolved); // untouched
00220:         }
00221:
00222:         [Fact]
00223:         public void MoralChoice_SaveEnvelope_RoundTripsResolvedLedger()
00224:         {
00225:             // Audit #30 — checksum envelope over MoralChoiceState (host SaveStore shape).
00226:             var system = new MoralChoiceSystem(new SeededRng(42));
00227:             var quest = new MoralChoiceQuestDefinition
00228:             {
00229:                 Id = "quest_moral_envelope_pin",
00230:                 DisplayName = "Envelope Pin",
00231:                 Category = "share",
00232:                 Discovery = "Fixture dilemma for save-envelope pin.",
00233:                 LocationId = "loc_surface_airlock",
00234:                 MinDay = 1,
00235:                 MaxDay = 10,
00236:                 Choices = new List<MoralChoiceOption>
00237:                 {
00238:                     new() { Label = "Share", MoralDelta = 5, EmpathyDelta = 1, OutcomeText = "ok", Epitaph = "shared" },
00239:                     new() { Label = "Refuse", MoralDelta = -5, EmpathyDelta = -1, OutcomeText = "no", Epitaph = "refused" }
00240:                 }
00241:             };
00242:             Assert.NotNull(system.Resolve(quest, 0, quest.LocationId, 2));
00243:
00244:             string envelope = SaveEnvelopeHelper.CaptureEnvelope(system.CaptureState());
00245:             var (ok, restored, error) = SaveEnvelopeHelper.RestoreEnvelope<MoralChoiceState>(
00246:                 envelope, allowBareFallback: false);
00247:             Assert.True(ok, error);
00248:             Assert.NotNull(restored);
00249:
00250:             var reloaded = new MoralChoiceSystem(new SeededRng(99));
00251:             reloaded.RestoreState(restored!);
00252:             Assert.True(reloaded.IsResolved(quest.Id));
00253:             Assert.Equal(5, reloaded.MoralScore);
00254:
00255:             var again = reloaded.Resolve(quest, 1, quest.LocationId, 3);
00256:             Assert.Equal(0, again.choiceIndex);
00257:             Assert.Equal(5, reloaded.MoralScore);
00258:         }
00259:
00260:         [Fact]
00261:         public void HostWiring_SaveAllAndProcessFlush_EnrollMoralChoice()
00262:         {
00263:             string? srcRoot = FindSrcRoot();
00264:             Assert.NotNull(srcRoot);
00265:
00266:             string orch = File.ReadAllText(Path.Combine(srcRoot!, "Main.SaveOrchestrator.cs"));
00267:             string app = File.ReadAllText(Path.Combine(srcRoot!, "Main.Application.cs"));
00268:             Assert.Contains("SaveMoralChoice()", orch);
00269:             Assert.Contains("FlushMoralChoiceIfDirty()", app);
00270:         }
00271:
00272:         private static string? FindSrcRoot()
00273:         {
00274:             string current = Directory.GetCurrentDirectory();
00275:             while (!string.IsNullOrEmpty(current))
00276:             {
00277:                 string candidate = Path.Combine(current, "src");
00278:                 if (Directory.Exists(candidate)) return candidate;
00279:                 string parent = Path.GetDirectoryName(current)!;
00280:                 if (parent == current) break;
00281:                 current = parent;
00282:             }
00283:             return null;
00284:         }
00285:     }
00286: }
```
# Appendix M — External verification handoff

The following checks are to be run by the owning integrator after writing: character count, SHA-256 revalidation, path-token resolution, duplicate-heading/unsupported-claim scan, and `git diff --check`. The final ledger entry must report actual results, not this template.
