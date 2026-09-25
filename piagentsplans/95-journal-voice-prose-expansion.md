# Plan 95 — Journal Voice Prose: Thirty-Nine Situation Keys, Seven Risk-Bias Voices, and Canonical Knowledge Flow

> **Rebuild status:** TERMINAL 39-KEY CONTENT + VOICE/READER CONSUMER AUDIT
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

The historical baseline was 4,912 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Keep the current thirty-nine journal-voice situation keys and seven risk-bias variants as a deterministic presentation vocabulary while preserving JournalSystem, knowledge keys, survivor risk-bias identity, Muster witness prose, and existing journal persistence as the owners. The historical expansion is complete; the residual is a key-to-consumer and voice-quality audit, not arbitrary prose growth.

**Bounded outcome:** Audit `journal_voice_prose.json`, `JournalVoiceProseCatalog`, `JournalVoice`, `JournalSystem`, `Main.Narrative` binding, Muster witness/journal surfaces, and focused tests. Classify keys as live, dormant, duplicated, or invalid and preserve the current source-language/format contract.

**Non-goals:** no new journal system, no free-form AI prose, no key that bypasses a canonical event, no new save section, no arbitrary key/variant count, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `journal_voice_prose.json` contains 39 situation keys, each with seven risk-bias variants.
- VERIFIED CURRENT: `JournalVoice` binds a catalog and composes text from canonical knowledge keys and `RiskBiasTrait`.
- VERIFIED CURRENT: `Main.Narrative` binds the default/current catalog; JournalSystem owns entries.
- Current producer coverage for every key is an explicit premise question.
- HISTORICAL RECORD: Plan 95/Wave 40 records the catalog expansion; this package does not claim a fresh test run.

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

- `Assets/StreamingAssets/Data/journal_voice_prose.json` exists at 44,842 bytes; SHA-256 `a131fa5dbe50b0702af43987b14f3f2c40544730d4df78de9ac9a78171292021`.

# 3. Required Delta

Replace the old “add 12 keys” brief with a current 39-key census, producer/consumer trace, and voice-quality audit. Preserve JournalSystem as the sole entry owner and voice as a pure projection.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Which current JournalSystem/Muster producers can emit each of the 39 keys?
What is the explicit missing-variant/fallback behavior?
How are risk-bias traits sourced for ordinary survivors versus witness authors?
Which current panels display the composed text and how do they handle fallback?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| voice catalog and key validation | `JournalVoiceProseCatalogLoader / JournalVoiceProseCatalog` | `Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs` | Static key/variant definitions; no journal state. |
| voice selection and formatting | `JournalVoice` | `Assets/Ashfall.Core/Journal/JournalVoice.cs` | Binds catalog, selects bias variant, formats timestamp. |
| journal entry lifecycle and knowledge facts | `JournalSystem` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` | Owns journal entries, knowledge keys, and dedupe/persistence. |
| host catalog binding | `Main.Narrative` | `src/Main.Narrative.cs` | Binds the current catalog at setup. |
| witness/read-model presentation | `Muster witness/journal surfaces` | `src/Muster/JournalWitnessPanel.cs; src/UI/MusterPanel.cs` | Projects current authored witness/journal facts. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for journal-voice prose catalog.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Journal Voice Prose: Thirty-Nine Situation Keys, Seven Risk-Bias Voices, and Canonical Knowledge Flow                                               │
│ Integration route: DATA-ONLY + current journal/consumer audit                             │
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

1. **Catalog defines voices.**
2. **JournalVoice formats.**
3. **JournalSystem owns entries.**
4. **Main.Narrative binds.**
5. **Panels project current entries.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| voice catalog and key validation | `JournalVoiceProseCatalogLoader / JournalVoiceProseCatalog` | `Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs` | Static key/variant definitions; no journal state. |
| voice selection and formatting | `JournalVoice` | `Assets/Ashfall.Core/Journal/JournalVoice.cs` | Binds catalog, selects bias variant, formats timestamp. |
| journal entry lifecycle and knowledge facts | `JournalSystem` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` | Owns journal entries, knowledge keys, and dedupe/persistence. |
| host catalog binding | `Main.Narrative` | `src/Main.Narrative.cs` | Binds the current catalog at setup. |
| witness/read-model presentation | `Muster witness/journal surfaces` | `src/Muster/JournalWitnessPanel.cs; src/UI/MusterPanel.cs` | Projects current authored witness/journal facts. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load and validate the current prose catalog in Main.Narrative
2. JournalVoice binds the catalog for the campaign
3. JournalSystem receives a canonical knowledge key and current day
4. JournalVoice selects the exact risk-bias variant or explicit fallback
5. compose the current journal/witness text with timestamp
6. persist the canonical JournalSystem entry and present it through current UI

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- each situation key has the current required bias variants or an explicit documented fallback
- knowledge keys are canonical and not invented by prose
- risk bias is a current survivor/read-model input
- same key/bias/day yields the same text
- missing variant is visible in diagnostics and never fabricated as a new event
- journal entry identity/dedupe remains JournalSystem-owned
- voice text does not mutate mechanics or save state

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

Contract rules for journal-voice prose catalog:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`journal_voice_prose.json` is the current voice authority with 39 situation keys and seven variants per key. Audit each key against a real producer/consumer. The original 12-key proposal is terminal content growth; future additions require a named producer, information-flow legality, tone review, and a focused key test.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

Voice prose is static and should not be persisted. Journal entries and knowledge keys use the existing JournalSystem save owner. A future preference/voice override must use the existing survivor/read-model owner, not a new journal-text cache.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

Variant selection is a pure key+bias lookup and timestamp formatting is deterministic for the supplied day/hour. No random selection or wall-clock formatting may enter Core. Paired tests compare key, bias, text, timestamp, and journal dedupe identity.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

JournalSystem owns entry creation and canonical facts; voice formatting is downstream presentation. A voice key cannot create a death, faction event, or knowledge unlock. Muster witness surfaces may request framing but do not own journal state.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Main.Narrative.cs` — binds the current journal voice catalog
- `src/Muster/JournalWitnessPanel.cs` — projects witness knowledge text with author bias
- `src/UI/MusterPanel.cs` — current journal/witness presentation
- `src/Main.UiPanels.cs` — panel lifecycle/route integration
- `src/Main.SaveOrchestrator.cs` — existing journal persistence handoff

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Voice variants should be distinct in worldview while grounded in the same event. A paranoid, cautious, realist, reckless, denialist, or fatalist perspective can interpret one fact differently; it cannot invent a different fact, reveal an ending, or copy a real-world text/layout.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/JournalVoiceProseCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/JournalVoiceProseExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/Plan95_103JournalTreatyIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/JournalProducerIntegrationTests.cs`

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
| Phase 0 — key/variant census | read 39 keys, catalog, JournalVoice, JournalSystem, host, and tests | all keys and fallback behavior are explicit | no undocumented scope or shortcut |
| Phase 1 — producer/consumer trace | map keys to canonical events and current panels | live/dormant/duplicate keys are classified | no undocumented scope or shortcut |
| Phase 2 — voice/save/accessibility audit | check bias distinction, dedupe, fallback, and readable presentation | no prose becomes authority | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a proven producer/quality gap is promoted | one key/consumer package with focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/journal_voice_prose.json` | READ ONLY; MODIFY only for a proven key/producer defect | retain as voice authority |
| `Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs` | READ ONLY | catalog validation |
| `Assets/Ashfall.Core/Journal/JournalVoice.cs` | READ ONLY | selection/formatting |
| `Assets/Ashfall.Core/Journal/JournalSystem.cs` | READ ONLY | journal owner |
| `src/Main.Narrative.cs` | READ ONLY | catalog binding |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| parallel journal authority | keep JournalSystem canonical |
| unreachable keys | trace producers |
| voice text changing facts | pure downstream formatting |
| prose quality drift | review variants and focused tests |

# 22. Explicit Non-Goals

- no new journal system, no free-form AI prose, no key that bypasses a canonical event, no new save section, no arbitrary key/variant count, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for journal-voice prose catalog are named from current evidence.
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

- A current key-to-producer/consumer and bias-fallback matrix.
- A voice-quality and continuity review of all 39 keys.
- A bounded residual only for a proven producer or presentation gap.

## MUST NOT DO

- add free-form generated prose
- create a second journal/save store
- let voice text mutate events
- grow keys without producer evidence

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/JournalVoiceProseCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/JournalVoiceProseExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/Plan95_103JournalTreatyIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/JournalProducerIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read journal_voice_prose.json, JournalVoiceProseCatalog, JournalVoice, JournalSystem, Main.Narrative, and focused tests; enumerate every key and current producer.

# Appendix A — Master expansion authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Authority lines: 5,510; bytes: 635,647

The following slices are read-only design constraints. Live source remains higher authority.

### Authority lines 40–45
00040:
00041: **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
00042: Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.
00043:
00044: **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
00045: Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.

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

### Authority lines 710–715
00710: **A-22 · C9 · Final-wishes document corpus.** Subject: unsent-letter and testament prose for `final_wishes.json` entries lacking document twins. Evidence: catalog verified live; `unsent_letters_batch_2` demonstrates the genre. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00711:
00712: **A-23 · C9 · Intake interview continuation.** Subject: new-arrival intake interviews conditioned on the arrival channels that exist (rescue, crossing, holdfast). Evidence: `new_arrival_intake_interviews` exists; arrival channels are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00713:
00714: **A-24 · C10 · Bureaucratic-morality quest prose completion.** Subject: prose-field completion across `quests_bureaucratic_morality.json` records with skeleton `quest_hook`/outcome texts. Evidence: catalog verified live; Part 9 contracts define the fields. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00715:

### Authority lines 720–725
00720: **A-27 · C12 · Storm-window almanac entries.** Subject: almanac prose conditioned on `year_of_ash_storm_windows.json` entries. Evidence: catalog verified live; `weather_almanac_expansion` exists in the corpus. Route: DATA-ONLY, within the Year-of-Ash window (180–360) canon. Confidence: HIGH CONFIDENCE.
00721:
00722: **A-28 · C13 · Under-served epilogue chronicle depth.** Subject: consumed by F-005 after the permutation audit selects the weakest cells. Evidence: matrix is canon (32 permutations). Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00723:
00724: **A-29 · C14 · Bestiary natural-history continuation.** Subject: sighting-log and specimen-record prose for bestiary entries with thin coverage. Evidence: `wasteland_wildlife_bestiary.json` verified live; vulture-sighting and cockroach-hive log genres exist. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00725:

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

### Authority lines 1140–1145
01140: - 2026-09-27 — Volume 16: worked content tranche library — twelve PROPOSAL-model JSON tranche examples per established genre (glitch, load-shed, assay, interlock report, marginalia, rundown, intake, quest prose, sighting log, ordnance manifest, almanac), each with validation notes and the three mandatory focused tests — ~12,400 — cumulative ~313,000
01141: - 2026-09-27 — Volume 17: cluster-by-cluster expansion roadmaps C1–C17, each anchored to its deep map with Phase I–IV structure and five cross-cluster sequencing rules — ~16,900 — cumulative ~330,000
01142: - 2026-09-27 — Volume 18: re-audit against the repository's public surface — four drift-register candidates (DR-16 communiqué board and tick-gate, DR-17 map-atlas repair, DR-18 hardening/quarantine cleanup, DR-19 post-v1.0 subsystem families absent from the deep maps), five premise corrections, five evidence-gated seed replenishments (A-31, A-32, B-26, G-09, E-11), and the factory self-audit — ~9,900 — cumulative ~342,000
01143: - 2026-09-27 — Volume 19: public-surface confirmation pass — DR-16, DR-17, DR-18 upgraded to MERGED-AS-LISTED with merge dates; four new entries (DR-20 wave-5/6 subsystem inventory, DR-21 shelter atmosphere/noise panels with follow-up repair, DR-22 commitment-event semantic parity incident corroborating DP-04, DR-23 port-contract seam ratchet cleanup); six premise corrections — ~9,700 — cumulative ~352,000
01144: - 2026-09-27 — Volume 20: authoring session runbook library — eight runbooks (RB-CENSUS, RB-COLLISION, RB-PAIR, RB-SEALGUARD, RB-KNOWLEDGE, RB-QUANT, RB-WAVE, RB-DRCONFIRM) turning every open-premise class into an executable session procedure — ~7,700 — cumulative ~359,000
01145: - 2026-09-27 — Volume 21: remaining Lane B and all Lane C seed expansions — FP-B04, FP-B06, FP-B14, FP-B15 plus the full economy set FP-C01 through FP-C14 with the lane rule that no Lane C plan changes a number in its first tranche; Lane B and Lane C compressed backlogs now zero (GATE items excepted, correctly) — ~23,100 — cumulative ~382,000

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/journal_voice_prose.json`
- Bytes: 44,842; SHA-256: `a131fa5dbe50b0702af43987b14f3f2c40544730d4df78de9ac9a78171292021`
- Root keys: `prose_variants, schema_version`
- `prose_variants`: object[39]
  - `high_co2`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `has_seen_radiation`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `has_experienced_storm`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `filter_failing`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `freezing_shelter`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `history_continuity_reclamation_decree`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_hydro_baron_rate_card_origin`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_deserter_coalition_founding`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_cold_count_before_the_lab`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_the_provisioned_advance_knowledge`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_checkpoint_conscripts_confession`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_quartermasters_paperwork`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_the_intercepted_cipher`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_the_ledger_nobody_signed`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `faction_military_history`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_state_propaganda`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_military_operations`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_military_deployment`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_civil_defense`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_military_units`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_trade_guilds`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `journal_casualty_records`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `journal_soldier_letters`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `journal_religious_texts`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `journal_exchange_day`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `low_food`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `low_water`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `death_of_survivor`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `successful_expedition`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `failed_expedition`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `faction_raid`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `disease_outbreak`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `power_failure`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `new_survivor_arrived`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `severe_cold`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `high_radiation_zone`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `moral_compromise`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `micro_radio_tower_log`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `micro_dead_livestock_tags`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
- Bytes: 44,842; SHA-256: `a131fa5dbe50b0702af43987b14f3f2c40544730d4df78de9ac9a78171292021`
- Root keys: `prose_variants, schema_version`
- `prose_variants`: object[39]
  - `high_co2`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `has_seen_radiation`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `has_experienced_storm`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `filter_failing`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `freezing_shelter`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `history_continuity_reclamation_decree`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_hydro_baron_rate_card_origin`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_deserter_coalition_founding`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_cold_count_before_the_lab`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_the_provisioned_advance_knowledge`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_checkpoint_conscripts_confession`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_quartermasters_paperwork`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_the_intercepted_cipher`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `history_the_ledger_nobody_signed`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `faction_military_history`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_state_propaganda`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_military_operations`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_military_deployment`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_civil_defense`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_military_units`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `faction_trade_guilds`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `journal_casualty_records`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `journal_soldier_letters`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `journal_religious_texts`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `journal_exchange_day`: object[7]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist`
  - `low_food`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `low_water`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `death_of_survivor`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `successful_expedition`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `failed_expedition`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `faction_raid`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `disease_outbreak`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `power_failure`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `new_survivor_arrived`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `severe_cold`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `high_radiation_zone`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `moral_compromise`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `micro_radio_tower_log`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`
  - `micro_dead_livestock_tags`: object[9]; keys: `default, paranoid, cautious, realist, reckless, denialist, fatalist, empath, sociopath`

# Appendix D — Current caller/reference graph

### `JournalVoiceProseCatalog` (18 sampled current references)
- Assets/Ashfall.Core/Journal/JournalVoice.cs:10: private static JournalVoiceProseCatalog? _catalog;
- Assets/Ashfall.Core/Journal/JournalVoice.cs:12: public static void BindCatalog(JournalVoiceProseCatalog? catalog)
- Assets/Ashfall.Core/Journal/JournalVoice.cs:17: public static JournalVoiceProseCatalog? GetCatalog() => _catalog;
- Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs:55: public sealed class JournalVoiceProseCatalog
- Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs:59: public JournalVoiceProseCatalog(IReadOnlyDictionary<string, JournalVoiceProseEntry> entries)
- Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs:92: public sealed class JournalVoiceProseCatalogLoader
- Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs:99: public JournalVoiceProseCatalogLoader(IFileIO files, IJsonSerializer json)
- Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs:105: public JournalVoiceProseCatalog Load(string dataDirectory)
- Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs:116: return new JournalVoiceProseCatalog(root?.prose_variants ?? new());
- Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs:125: public static JournalVoiceProseCatalog Empty => new JournalVoiceProseCatalog(new Dictionary<string, JournalVoiceProseEntry>());
- Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs:127: public static JournalVoiceProseCatalog LoadDefault(IFileIO? files = null, IJsonSerializer? json = null)
- Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs:131: var loader = new JournalVoiceProseCatalogLoader(files, json);
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:435: ["journal_voice_prose.json"] = new[] { "JournalVoiceProseCatalog" },
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:711: ["journal_voice_prose.json"] = "JournalVoiceProseCatalog",
- src/Main.Narrative.cs:60: JournalVoice.BindCatalog(JournalVoiceProseCatalogLoader.LoadDefault());
- src/Journal/JournalSelfTest.cs:105: var proseCatalog = JournalVoiceProseCatalogLoader.LoadDefault();
- Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs:67: JournalVoice.BindCatalog(new JournalVoiceProseCatalogLoader(FileIO, Serializer).Load(DataDir));
- Ashfall.Core.Tests/CollectibleNarrativeQualityTests.cs:190: private static JournalVoiceProseCatalog LoadProse()
### `JournalVoice.ComposeFullText` (14 sampled current references)
- Assets/Ashfall.Core/Journal/JournalSystem.cs:211: string text = JournalVoice.ComposeFullText(knowledgeKey, bias, day);
- Assets/Ashfall.Core/Journal/JournalSystem.cs:245: string text = JournalVoice.ComposeFullText(knowledgeKey, bias, day);
- src/Journal/JournalSelfTest.cs:97: string voice = JournalVoice.ComposeFullText(KnowledgeKeys.HighCo2, RiskBiasTrait.Paranoid, 3);
- src/Journal/JournalSelfTest.cs:109: Check(JournalVoice.ComposeFullText("micro_radio_tower_log", RiskBiasTrait.Realist, 6) != Placeholder,
- src/Journal/JournalSelfTest.cs:111: Check(JournalVoice.ComposeFullText("micro_dead_livestock_tags", RiskBiasTrait.Fatalist, 6) != Placeholder,
- src/Muster/JournalWitnessPanel.cs:15: /// JournalVoice.ComposeFullText per the authoring survivor's RiskBiasTrait.
- src/Muster/JournalWitnessPanel.cs:70: string framing = JournalVoice.ComposeFullText(w.knowledgeKey, authorBias, day);
- src/UI/MusterPanel.cs:318: string framing = JournalVoice.ComposeFullText(w.knowledgeKey, _muster.AuthorBias, day);
- Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs:109: string expected = JournalVoice.ComposeFullText(def.effect_target, RiskBiasTrait.Realist, FixtureDay);
- Ashfall.Core.Tests/JournalProducerIntegrationTests.cs:276: string fullText = JournalVoice.ComposeFullText("k_radiation_alarm", RiskBiasTrait.Realist, day: 12);
- Ashfall.Core.Tests/MicroLocationRadioIntegrationTests.cs:231: var text = JournalVoice.ComposeFullText(RadioTowerLogKey, RiskBiasTrait.Realist, 6);
- Ashfall.Core.Tests/Narrative/JournalVoiceProseCatalogTests.cs:168: string fullText = JournalVoice.ComposeFullText(KnowledgeKeys.HighCo2, RiskBiasTrait.Realist, 45);
- Ashfall.Core.Tests/Narrative/JournalVoiceProseCatalogTests.cs:323: string fullText = JournalVoice.ComposeFullText(KnowledgeKeys.HighCo2, RiskBiasTrait.Realist, 90);
- Ashfall.Core.Tests/Narrative/JournalVoiceProseExpansionTests.cs:154: string full = JournalVoice.ComposeFullText("low_food", RiskBiasTrait.Realist, 42);
### `JournalSystem` (18 sampled current references)
- Assets/Ashfall.Core/ArchiveDeskSystem.cs:65: private readonly JournalSystem _journal;
- Assets/Ashfall.Core/ArchiveDeskSystem.cs:77: JournalSystem journal,
- Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs:222: // ── Journal once-only (real JournalSystem dedupe) ──────────
- Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs:223: var journal = new JournalSystem();
- Assets/Ashfall.Core/District8DeepCoastSystem.cs:146: // Journal knowledge keys — once-only via the real JournalSystem.
- Assets/Ashfall.Core/LibraryStudySystem.cs:101: private readonly JournalSystem _journal;
- Assets/Ashfall.Core/LibraryStudySystem.cs:129: JournalSystem journal,
- Assets/Ashfall.Core/LibraryStudySystem.cs:318: // Add knowledge evidence (idempotent and deduped in JournalSystem)
- Assets/Ashfall.Core/Journal/JournalSystem.cs:13: public class JournalSystem
- Assets/Ashfall.Core/Journal/JournalSystem.cs:47: public JournalSystem()
- Assets/Ashfall.Core/Narrative/BureaucraticDocumentCatalog.cs:451: /// Bounded producer coordinator. Discovery state is owned by JournalSystem's
- Assets/Ashfall.Core/Narrative/BureaucraticDocumentCatalog.cs:470: public bool IsDiscovered(JournalSystem journal, string docId)
- Assets/Ashfall.Core/Narrative/BureaucraticDocumentCatalog.cs:481: JournalSystem journal)
- Assets/Ashfall.Core/Narrative/BureaucraticDocumentCatalog.cs:505: JournalSystem journal)
- Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs:1263: /// deterministic source adapters, and connects discovery state to JournalSystem.
- Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs:1537: /// Attempts to discover the given narrative record for the player via JournalSystem.
- Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs:1540: public bool TryDiscover(string discoveryId, JournalSystem journal, out NarrativeDiscoveredRecord? record)
- Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs:137: JournalSystem? journal,
### `Main.Narrative` (4 sampled current references)
- src/Host/HostEventSaveStore.cs:5: // Host Caller: Main.Narrative / HostEventAdapter
- src/Host/NarrativeSaveStore.cs:5: // Host Caller: Main.Narrative / NarrativeHostSession
- src/Host/RadioSaveStore.cs:5: // Host Caller: Main.Narrative / RadioHostSession
- src/Journal/JournalSaveStore.cs:5: // Host Caller: Main.Holdfast, Main.Narrative / JournalHostSession
### `JournalWitnessPanel` (3 sampled current references)
- src/Main.Muster.cs:39: private JournalWitnessPanel _witnessPanel = null!;
- src/Main.Muster.cs:85: _witnessPanel = new JournalWitnessPanel();
- src/Muster/JournalWitnessPanel.cs:18: public partial class JournalWitnessPanel : PanelContainer

# Appendix E — Current focused-test inventory

Current test declaration inventory: 76 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/Narrative/JournalVoiceProseCatalogTests.cs` — 44 test declarations; bytes=13,390; SHA-256=`ba911d0f0f23466991b6e75589e4ee51fc5f6b73a45641d61db68ff43eec4773`
- 00039: [Fact]
- 00040: public void JournalVoiceProseJsonHasSchemaVersion()
- 00049: [Fact]
- 00050: public void JournalVoiceProseJsonLoadsWithoutErrors()
- 00057: [Fact]
- 00058: public void AllKnowledgeKeysHaveProseEntries()
- 00069: [Fact]
- 00070: public void AllKnowledgeKeysHaveDefaultVariant()
- 00083: [Fact]
- 00084: public void AllCoreBiasTraitsHaveVariants()
- 00105: [Fact]
- 00106: public void AllExpansionBiasTraitsHaveVariantsWhereApplicable()
- 00137: [Fact]
- 00138: public void GetProseReturnsCorrectVariant()
- 00147: [Fact]
- 00148: public void GetProseFallsBackToDefault()
- 00163: [Fact]
- 00164: public void ComposeFullTextFormatsCorrectly()
- 00172: [Fact]
- 00173: public void FormatTimestampWorksCorrectly()
- 00188: [Fact]
- 00189: public void AllProseTextIsNonEmpty()
- 00211: [Fact]
- 00212: public void CatalogCountMatchesExpected()
- 00219: [Fact]
- 00220: public void UnknownKnowledgeKeyReturnsFallback()
- 00228: [Fact]
- 00229: public void PinHighCo2ParanoidOutput()
- 00238: [Fact]
- 00239: public void PinHighCo2CautiousOutput()
- 00248: [Fact]
- 00249: public void PinHighCo2RealistOutput()
- 00258: [Fact]
- 00259: public void PinSeenRadiationAllBiasTraits()
- 00280: [Fact]
- 00281: public void PinExperiencedStormAllBiasTraits()
- 00302: [Fact]
- 00303: public void PinExpansionProseForEmpathAndSociopath()
- 00317: [Fact]
- 00318: public void ComposeFullTextPreservesDayPrefix()
- 00327: [Fact]
- 00328: public void CatalogBindIsIdempotent()
- 00340: [Fact]
- 00341: public void AllKnowledgeKeysAreLowercaseSnakeCase()
### `Ashfall.Core.Tests/Narrative/JournalVoiceProseExpansionTests.cs` — 14 test declarations; bytes=6,357; SHA-256=`efc8fb9569960c5b92551d121d5a5ea7685ad208093df2b259c28b1c228bfdbd`
- 00058: [Fact]
- 00059: public void All12Plan95SituationKeysExistInCatalog()
- 00070: [Fact]
- 00071: public void AllPlan95KeysHaveDefaultAndCoreBiasVariants()
- 00090: [Fact]
- 00091: public void AllPlan95VariantsWithinEachKeyAreDistinct()
- 00114: [Fact]
- 00115: public void AllPlan95SituationKeysAreStrictSnakeCase()
- 00125: [Fact]
- 00126: public void JournalVoiceComposeBodyProducesVariantsForPlan95Keys()
- 00148: [Fact]
- 00149: public void JournalVoiceComposeFullTextFormatsCorrectly()
- 00159: [Fact]
- 00160: public void JournalVoiceFallsBackGracefullyOnUnknownKey()
### `Ashfall.Core.Tests/Narrative/Plan95_103JournalTreatyIntegrationTests.cs` — 8 test declarations; bytes=8,874; SHA-256=`f15c60bd77cc2e1b4ebdd4704c163b33c0d4db813bfcab6f0eebc99a265b228b`
- 00047: [Fact]
- 00048: public void Plan95_JournalVoiceProseCatalog_LoadsAllSituationKeys_WithDistinctBiasVariants()
- 00077: [Fact]
- 00078: public void Plan103_FoundryTreatyConsequences_LoadsAllFifteenPolicies_WithValidSignatoriesAndModifiers()
- 00117: [Fact]
- 00118: public void CrossSystem_TreatyConsequenceOutcomesAndJournalVoices_ExhibitSystemicResonance()
- 00163: [Fact]
- 00164: public void CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses()
### `Ashfall.Core.Tests/JournalProducerIntegrationTests.cs` — 10 test declarations; bytes=12,942; SHA-256=`66dd88993fedcf329e7fab1058adf095357abdefd73081fdac8634e22c10b878`
- 00029: [Fact]
- 00030: public void AutopsyProducer_WritesJournalEntry_AndEnforcesDedupOnRepeat()
- 00092: [Fact]
- 00093: public void LibraryStudyProducer_GrantsKnowledgeUnlocks_ViaAddKnowledgeEvidence()
- 00132: [Fact]
- 00133: public void MoralChoiceProducer_WritesJournalEntry_OnQuestResolved()
- 00181: [Fact]
- 00182: public void ProceduralEulogyEngine_ComposesFullMemorial_AndSupportsLosslessSaveRestore()
- 00241: [Fact]
- 00242: public void JournalVoice_ToneShiftsWithRiskBias_AndFormatsCleanly()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Journal Voice Prose: Thirty-Nine Situation Keys, Seven Risk-Bias Voices, and Canonical Knowledge Flow` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan describes 12+ new keys; the current file has 39 and the old count is not a target.
- A catalog key is not evidence of a live event producer.

## H.2 Evidence-strength corrections
- Trace all keys to real producers.
- Keep voice distinct but factually bounded.
- Remove unreachable or duplicated prose.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Journal Voice Prose: Thirty-Nine Situation Keys, Seven Risk-Bias Voices, and Canonical Knowledge Flow while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use JournalVoiceProseCatalog for static key/bias definitions.
- Use JournalVoice for pure selection/formatting.
- Use JournalSystem for entry identity, dedupe, and persistence.
- Use Main.Narrative for catalog binding only.
- Use current witness/journal panels for presentation.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement journal voice prose: thirty-nine situation keys, seven risk-bias voices, and canonical knowledge flow arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- No voice key is claimed live without a current producer.
- No new journal state or free-form generation is proposed.

## J.2 Questions deliberately left open
- Should all 39 keys remain visible in authoring tools, or should dormant keys be quarantined?
- Which risk-bias source is canonical for a witness authored by a dead/dead-adjacent subject?

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

The original file at `HEAD:piagentsplans/95-journal-voice-prose-expansion.md` contained 4,912 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 95 — Journal Voice Prose Expansion (expand personality-variant prose)

## Goal (2 lines)
Expand `journal_voice_prose.json` with more situation keys and more personality
variants per key. The journal voice system (`JournalVoice.cs` and
`JournalVoiceProseCatalog.cs` confirmed live) defines personality-variant prose
for journal entries — each situation key (high_co2, has_seen_radiation,
has_experienced_storm) has variants for 7 personality types (default, paranoid,
cautious, realist, reckless, denialist, fatalist). The existing catalog has
too few situation keys.

## Why (P2)
- Verified: `journal_voice_prose.json` has a `prose_variants` object with
  situation keys, each containing 7 personality variants (default, paranoid,
  cautious, realist, reckless, denialist, fatalist).
  `JournalVoice.cs` and `JournalVoiceProseCatalog.cs` are confirmed live.
- Creates the journal-voice pillar: journal entries should sound different
  depending on who's writing — a paranoid survivor sees conspiracy in a CO2
  spike; a fatalist sees inevitability; a denialist sees nothing. More
  situation keys mean more journal entries have personality, not just
  generic text.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/journal_voice_prose.json` (add situation keys)
- Read-only: `Assets/Ashfall.Core/Journal/JournalVoice.cs`,
  `Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs` (confirm how
  situation keys are selected and how personality variants are applied)

## Content grammar (per situation key)
- Key name: snake_case describing the situation (high_co2, has_seen_radiation,
  has_experienced_storm, low_food, death_of_survivor, faction_raid,
  successful_expedition, etc.).
- 7 personality variants per key: default, paranoid, cautious, realist,
  reckless, denialist, fatalist. Each is 1–2 sentences in that personality's
  voice.
- Personality voice rules:
  - default: neutral, factual.
  - paranoid: suspicious, conspiratorial, sees threat everywhere.
  - cautious: careful, measured, focuses on risk mitigation.
  - realist: pragmatic, data-focused, no emotion.
  - reckless: dismissive of danger, action-oriented.
  - denialist: refuses to acknowledge the problem.
  - fatalist: accepts doom as inevitable, darkly resigned.
- Each variant should be distinct — no two personalities should say the same
  thing in different words.

## Steps
1. Read `JournalVoice.cs` and `JournalVoiceProseCatalog.cs` to confirm how
   situation keys are selected (by game state? by event trigger?) and how
   personality variants are chosen (by survivor personality trait?).
2. Read the existing situation keys (high_co2, has_seen_radiation,
   has_experienced_storm, and any others) to confirm the quality bar and
   the 7-variant pattern.
3. Author 12 new situation keys:
   - `low_food`: food shortage journal entry.
   - `low_water`: water shortage journal entry.
   - `death_of_survivor`: a survivor has died.
   - `successful_expedition`: an expedition returned with loot.
   - `failed_expedition`: an expedition returned empty or with casualties.
   - `faction_raid`: the shelter was raided.
   - `disease_outbreak`: a disease is spreading.
   - `power_failure`: the grid went down.
   - `new_survivor_arrived`: a new survivor joined the shelter.
   - `severe_cold`: extreme cold weather event.
   - `high_radiation_zone`: entered a high-radiation area.
   - `moral_compromise`: the player made a difficult moral choice.
4. Each key: 7 personality variants, each 1–2 sentences. Match the existing
   quality — each personality has a distinct voice and worldview.
5. Cross-reference: every situation key unique; every key has all 7
   personality variants; no two variants within a key are identical.
6. Validate: `--data-integrity-selftest` (all keys resolve).
7. xUnit: journal voice prose catalog loads all situation keys, each with 7
   non-empty variants, no duplicate variants within a key.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is situation-key selection (step 1): confirm
how the system chooses which key to use — if it's event-triggered, new keys
must correspond to events the system actually fires.

## Definition of Done
- `journal_voice_prose.json` has 12+ new situation keys, each with 7
  personality variants, all keys unique, all variants distinct, integrity +
  tests green.

## Follow-on
- Plan 88 (confession secrets) — confessions could trigger journal entries.
- Plan 66 (guilt sources) — guilt triggers moral_compromise journal entries.
- Plan 65 (final wishes) — a survivor's death triggers death_of_survivor.
- Plan 57 (incidents) — incidents trigger situation keys.
- Plan 27C (psychological contamination) — journal voice is the psychological
  expression layer.

```

## End of Plan 95 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/Journal/JournalVoice.cs` — 58 lines; 2,119 bytes; SHA-256 `bb815f0bcc0e193a853cf6d2e29be02afcc66aa6d080799f45991db603be116e`
Declaration index:
- 00008: public static class JournalVoice
- 00012: public static void BindCatalog(JournalVoiceProseCatalog? catalog)
- 00017: public static JournalVoiceProseCatalog? GetCatalog() => _catalog;
- 00023: public static string ComposeBody(string knowledgeKey, RiskBiasTrait bias)
- 00037: public static string ComposeFullText(string knowledgeKey, RiskBiasTrait bias, int day)
- 00048: public static string FormatTimestamp(int day, float hour = -1f)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: namespace Ashfall.Core.Journal
00003: {
00004:     /// <summary>
00005:     /// Trait-driven diegetic copy for journal discoveries. Cold, exhausted,
00006:     /// human — no fourth-wall tutorial language. Tone shifts with RiskBiasTrait.
00007:     /// </summary>
00008:     public static class JournalVoice
00009:     {
00010:         private static JournalVoiceProseCatalog? _catalog;
00011:
00012:         public static void BindCatalog(JournalVoiceProseCatalog? catalog)
00013:         {
00014:             _catalog = catalog;
00015:         }
00016:
00017:         public static JournalVoiceProseCatalog? GetCatalog() => _catalog;
00018:
00019:         /// <summary>
00020:         /// Compose body text (without leading "Day N.") for a knowledge key.
00021:         /// Falls back to a default message if the catalog is missing or the key is unknown.
00022:         /// </summary>
00023:         public static string ComposeBody(string knowledgeKey, RiskBiasTrait bias)
00024:         {
00025:             if (_catalog != null && _catalog.HasKey(knowledgeKey))
00026:             {
00027:                 return _catalog.GetProse(knowledgeKey, bias);
00028:             }
00029:
00030:             // Fallback: catalog not bound or key not found
00031:             return "Something changed. I wrote it down so I would not forget.";
00032:         }
00033:
00034:         /// <summary>
00035:         /// Full entry text: "Day N. …" matching the acceptance example shape.
00036:         /// </summary>
00037:         public static string ComposeFullText(string knowledgeKey, RiskBiasTrait bias, int day)
00038:         {
00039:             string body = ComposeBody(knowledgeKey, bias);
00040:             if (string.IsNullOrEmpty(body)) body = "I marked the day. That is all.";
00041:             int d = day > 0 ? day : 1;
00042:             // Body may already start with a sentence; prefix day stamp once.
00043:             if (body.StartsWith("Day "))
00044:                 return body;
00045:             return $"Day {d}. {body}";
00046:         }
00047:
00048:         public static string FormatTimestamp(int day, float hour = -1f)
00049:         {
00050:             int d = day > 0 ? day : 1;
00051:             if (hour < 0f) return $"Day {d}";
00052:             int h = (int)hour;
00053:             if (h < 0) h = 0;
00054:             if (h > 23) h = h % 24;
00055:             return $"Day {d}, {h:00}h";
00056:         }
00057:     }
00058: }
```

## `Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs` — 149 lines; 5,463 bytes; SHA-256 `29cf6cfa62ac472f90ddd6c60d6b5fba3e2c0bbff05f7f0462142d0c597abf10`
Declaration index:
- 00009: public class JournalVoiceProseEntry
- 00021: public string GetProseForBias(RiskBiasTrait bias)
- 00037: public bool HasVariantForBias(RiskBiasTrait bias)
- 00055: public sealed class JournalVoiceProseCatalog
- 00064: public JournalVoiceProseEntry? GetEntry(string knowledgeKey)
- 00071: public string GetProse(string knowledgeKey, RiskBiasTrait bias)
- 00083: public bool HasKey(string knowledgeKey)
- 00088: public IReadOnlyCollection<string> GetAllKeys() => new List<string>(_entries.Keys);
- 00092: public sealed class JournalVoiceProseCatalogLoader
- 00105: public JournalVoiceProseCatalog Load(string dataDirectory)
- 00127: public static JournalVoiceProseCatalog LoadDefault(IFileIO? files = null, IJsonSerializer? json = null)
- 00142: internal class JournalVoiceProseRoot
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.IO;
00005:
00006: namespace Ashfall.Core.Journal
00007: {
00008:     [Serializable]
00009:     public class JournalVoiceProseEntry
00010:     {
00011:         public string paranoid = string.Empty;
00012:         public string cautious = string.Empty;
00013:         public string realist = string.Empty;
00014:         public string reckless = string.Empty;
00015:         public string denialist = string.Empty;
00016:         public string fatalist = string.Empty;
00017:         public string empath = string.Empty;
00018:         public string sociopath = string.Empty;
00019:         public string @default = string.Empty;
00020:
00021:         public string GetProseForBias(RiskBiasTrait bias)
00022:         {
00023:             return bias switch
00024:             {
00025:                 RiskBiasTrait.Paranoid => paranoid,
00026:                 RiskBiasTrait.Cautious => cautious,
00027:                 RiskBiasTrait.Realist => realist,
00028:                 RiskBiasTrait.Reckless => reckless,
00029:                 RiskBiasTrait.Denialist => denialist,
00030:                 RiskBiasTrait.Fatalist => fatalist,
00031:                 RiskBiasTrait.Empath => empath,
00032:                 RiskBiasTrait.Sociopath => sociopath,
00033:                 _ => @default
00034:             };
00035:         }
00036:
00037:         public bool HasVariantForBias(RiskBiasTrait bias)
00038:         {
00039:             return bias switch
00040:             {
00041:                 RiskBiasTrait.Paranoid => !string.IsNullOrEmpty(paranoid),
00042:                 RiskBiasTrait.Cautious => !string.IsNullOrEmpty(cautious),
00043:                 RiskBiasTrait.Realist => !string.IsNullOrEmpty(realist),
00044:                 RiskBiasTrait.Reckless => !string.IsNullOrEmpty(reckless),
00045:                 RiskBiasTrait.Denialist => !string.IsNullOrEmpty(denialist),
00046:                 RiskBiasTrait.Fatalist => !string.IsNullOrEmpty(fatalist),
00047:                 RiskBiasTrait.Empath => !string.IsNullOrEmpty(empath),
00048:                 RiskBiasTrait.Sociopath => !string.IsNullOrEmpty(sociopath),
00049:                 _ => !string.IsNullOrEmpty(@default)
00050:             };
00051:         }
00052:     }
00053:
00054:     [Serializable]
00055:     public sealed class JournalVoiceProseCatalog
00056:     {
00057:         private readonly IReadOnlyDictionary<string, JournalVoiceProseEntry> _entries;
00058:
00059:         public JournalVoiceProseCatalog(IReadOnlyDictionary<string, JournalVoiceProseEntry> entries)
00060:         {
00061:             _entries = entries ?? throw new ArgumentNullException(nameof(entries));
00062:         }
00063:
00064:         public JournalVoiceProseEntry? GetEntry(string knowledgeKey)
00065:         {
00066:             if (string.IsNullOrEmpty(knowledgeKey)) return null;
00067:             _entries.TryGetValue(knowledgeKey, out var entry);
00068:             return entry;
00069:         }
00070:
00071:         public string GetProse(string knowledgeKey, RiskBiasTrait bias)
00072:         {
00073:             var entry = GetEntry(knowledgeKey);
00074:             if (entry != null)
00075:             {
00076:                 string prose = entry.GetProseForBias(bias);
00077:                 if (!string.IsNullOrEmpty(prose))
00078:                     return prose;
00079:             }
00080:             return "Something changed. I wrote it down so I would not forget.";
00081:         }
00082:
00083:         public bool HasKey(string knowledgeKey)
00084:         {
00085:             return !string.IsNullOrEmpty(knowledgeKey) && _entries.ContainsKey(knowledgeKey);
00086:         }
00087:
00088:         public IReadOnlyCollection<string> GetAllKeys() => new List<string>(_entries.Keys);
00089:         public int Count => _entries.Count;
00090:     }
00091:
00092:     public sealed class JournalVoiceProseCatalogLoader
00093:     {
00094:         public const string ProseFile = "journal_voice_prose.json";
00095:
00096:         private readonly IFileIO _files;
00097:         private readonly IJsonSerializer _json;
00098:
00099:         public JournalVoiceProseCatalogLoader(IFileIO files, IJsonSerializer json)
00100:         {
00101:             _files = files ?? throw new ArgumentNullException(nameof(files));
00102:             _json = json ?? throw new ArgumentNullException(nameof(json));
00103:         }
00104:
00105:         public JournalVoiceProseCatalog Load(string dataDirectory)
00106:         {
00107:             string path = _files.Combine(dataDirectory, ProseFile);
00108:
00109:             if (!_files.FileExists(path))
00110:                 return Empty;
00111:
00112:             try
00113:             {
00114:                 string jsonText = _files.ReadAllText(path);
00115:                 var root = _json.Deserialize<JournalVoiceProseRoot>(jsonText);
00116:                 return new JournalVoiceProseCatalog(root?.prose_variants ?? new());
00117:             }
00118:             catch (Exception ex)
00119:             {
00120:                 CatalogDiagnostics.Warn(ProseFile, "<root>", ex);
00121:                 return Empty;
00122:             }
00123:         }
00124:
00125:         public static JournalVoiceProseCatalog Empty => new JournalVoiceProseCatalog(new Dictionary<string, JournalVoiceProseEntry>());
00126:
00127:         public static JournalVoiceProseCatalog LoadDefault(IFileIO? files = null, IJsonSerializer? json = null)
00128:         {
00129:             files ??= new FileSystemIO();
00130:             json ??= new SystemTextJsonSerializer();
00131:             var loader = new JournalVoiceProseCatalogLoader(files, json);
00132:             if (CatalogLocator.TryFindDataDirectory(Environment.CurrentDirectory, out string dataDir)
00133:                 || CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir))
00134:             {
00135:                 return loader.Load(dataDir);
00136:             }
00137:             return Empty;
00138:         }
00139:     }
00140:
00141:     [Serializable]
00142:     internal class JournalVoiceProseRoot
00143:     {
00144: #pragma warning disable CS0649
00145:         public int schema_version;
00146:         public Dictionary<string, JournalVoiceProseEntry> prose_variants;
00147: #pragma warning restore CS0649
00148:     }
00149: }
```

## `Assets/Ashfall.Core/Journal/JournalSystem.cs` — 490 lines; 21,046 bytes; SHA-256 `c0d8316adaed06ce3b5415dcfd5fe79f63ad31fa6675181ad85859191601d220`
Declaration index:
- 00013: public class JournalSystem
- 00057: public int GetLastSeenIndex(int tab)
- 00063: public int GetLastSeenCodexIndex(int tab)
- 00069: public bool HasUnreadForTab(int tab)
- 00079: public void SwitchTab(int tab)
- 00090: public void MarkTabViewed(int tab)
- 00106: public bool UnlockItemSeen(string itemId) => UnlockCodex(KnowledgeKeys.ItemSeen(itemId));
- 00107: public bool UnlockLocationVisited(string locationId) => UnlockCodex(KnowledgeKeys.LocationVisited(locationId));
- 00108: public bool UnlockSurvivorMet(string survivorId) => UnlockCodex(KnowledgeKeys.SurvivorMet(survivorId));
- 00109: public bool UnlockEventFired(string eventId) => UnlockCodex(KnowledgeKeys.EventFired(eventId));
- 00110: public bool UnlockRoomHistorySeen(string vignetteId) => UnlockCodex(KnowledgeKeys.RoomHistorySeen(vignetteId));
- 00111: public bool UnlockGlitchNoted(string glitchId) => UnlockCodex(KnowledgeKeys.GlitchNoted(glitchId));
- 00112: public bool UnlockWildlifeCaught(string speciesId) => UnlockCodex(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
- 00113: public bool UnlockNarrativeDiscovered(string discoveryId) => UnlockCodex(KnowledgeKeys.NarrativeDiscovered(discoveryId));
- 00114: public bool UnlockBureaucraticDocument(string docId) => UnlockCodex(KnowledgeKeys.BureaucraticDocument(docId));
- 00115: public bool AddKnowledgeEvidence(string survivorId, string knowledgeKey) => UnlockCodex(knowledgeKey);
- 00117: public bool IsItemSeen(string itemId) => _knowledge.Has(KnowledgeKeys.ItemSeen(itemId));
- 00118: public bool IsLocationVisited(string locationId) => _knowledge.Has(KnowledgeKeys.LocationVisited(locationId));
- 00119: public bool IsSurvivorMet(string survivorId) => _knowledge.Has(KnowledgeKeys.SurvivorMet(survivorId));
- 00120: public bool IsEventFired(string eventId) => _knowledge.Has(KnowledgeKeys.EventFired(eventId));
- 00121: public bool IsRoomHistorySeen(string vignetteId) => _knowledge.Has(KnowledgeKeys.RoomHistorySeen(vignetteId));
- 00122: public bool IsGlitchNoted(string glitchId) => _knowledge.Has(KnowledgeKeys.GlitchNoted(glitchId));
- 00123: public bool IsWildlifeCaught(string speciesId) => _knowledge.Has(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
- 00124: public bool IsNarrativeDiscovered(string discoveryId) => _knowledge.Has(KnowledgeKeys.NarrativeDiscovered(discoveryId));
- 00125: public bool IsBureaucraticDocumentDiscovered(string docId) => _knowledge.Has(KnowledgeKeys.BureaucraticDocument(docId));
- 00127: private bool UnlockCodex(string key)
- 00140: public void SetEntryFactory(Func<JournalEntry> factory, Action<JournalEntry> recycler)
- 00152: public void BindAuthoredCorpus(JournalCorpusAdapter? adapter)
- 00165: /// Activate one authored record from a real producer. Unknown keys are
- 00168: public JournalEntry? TryAddAuthoredEntry(
- 00195: public JournalEntry? TryDiscover(
- 00216: /// F3 — record an expedition/knowledge discovery that is BOTH a journal
- 00224: public JournalEntry? TryDiscoverKnowledge(
- 00253: public JournalEntry? TryDiscoverRawKnowledge(
- 00281: public JournalEntry? TryAddRawEntry(
- 00299: private bool TryGetAuthoredRecord(
- 00309: private bool CanInsertAuthored(JournalCorpusRecord record)
- 00321: private JournalEntry InsertAuthored(
- 00343: private JournalEntry InsertEntry(
- 00369: private void PublishEntry(JournalEntry entry)
- 00391: public void AcknowledgePing()
- 00396: public void MarkRead()
- 00402: public void Clear()
- 00425: public JournalSave CaptureState()
- 00443: public void RestoreState(JournalSave save)
- 00476: public class JournalSave
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.Journal
00007: {
00008:     /// <summary>
00009:     /// Auto-generated survivor journal: playthrough log + immersive tutorial.
00010:     /// Discoveries land once via <see cref="KnowledgeBase"/>; text is trait-voiced.
00011:     /// EventRunner calls into this when world state first trips a discovery.
00012:     /// </summary>
00013:     public class JournalSystem
00014:     {
00015:         public const int MaxEntries = 64;
00016:
00017:         /// <summary>Number of journal tabs (Log, Items, People, Places, Events).</summary>
00018:         public const int TabCount = 5;
00019:
00020:         public event Action<JournalEntry> OnEntryAdded;
00021:         /// <summary>Fired when a new entry should ping the player (diegetic, not a modal).</summary>
00022:         public event Action<JournalEntry> OnNotificationPing;
00023:         /// <summary>Fired when the active tab changes (UI mirrors; save captures).</summary>
00024:         public event Action<int> OnTabChanged;
00025:         /// <summary>Fired when a codex unlock key is discovered for the first time.</summary>
00026:         public event Action<string> OnCodexUnlocked;
00027:
00028:         private readonly List<JournalEntry> _entries = new List<JournalEntry>();
00029:         private readonly KnowledgeBase _knowledge = new KnowledgeBase();
00030:         private int _seq;
00031:         private Func<JournalEntry> _entryFactory;
00032:         private Action<JournalEntry> _entryRecycler;
00033:         private JournalCorpusAdapter? _authoredCorpus;
00034:
00035:         /// <summary>Active tab index (0 = Log). Clamped to [0, TabCount).</summary>
00036:         public int ActiveTab { get; private set; }
00037:
00038:         /// <summary>Entry count at the moment each tab was last viewed (-1 = never).</summary>
00039:         private readonly int[] _lastSeenIndexPerTab = new int[TabCount];
00040:
00041:         /// <summary>Codex unlocks at the moment each tab was last viewed (-1 = never).</summary>
00042:         private readonly int[] _lastSeenCodexPerTab = new int[TabCount];
00043:
00044:         /// <summary>Total first-time codex unlocks (item/location/survivor/event keys).</summary>
00045:         public int CodexUnlockCount { get; private set; }
00046:
00047:         public JournalSystem()
00048:         {
00049:             for (int i = 0; i < TabCount; i++)
00050:             {
00051:                 _lastSeenIndexPerTab[i] = -1;
00052:                 _lastSeenCodexPerTab[i] = -1;
00053:             }
00054:         }
00055:
00056:         /// <summary>Entry count the last time <paramref name="tab"/> was viewed, or -1.</summary>
00057:         public int GetLastSeenIndex(int tab)
00058:         {
00059:             return tab >= 0 && tab < TabCount ? _lastSeenIndexPerTab[tab] : -1;
00060:         }
00061:
00062:         /// <summary>Codex unlock count the last time <paramref name="tab"/> was viewed, or -1.</summary>
00063:         public int GetLastSeenCodexIndex(int tab)
00064:         {
00065:             return tab >= 0 && tab < TabCount ? _lastSeenCodexPerTab[tab] : -1;
00066:         }
00067:
00068:         /// <summary>True when new content landed after the tab was last viewed.</summary>
00069:         public bool HasUnreadForTab(int tab)
00070:         {
00071:             if (tab <= 0) return HasUnread; // Log tab mirrors global unread
00072:             int lastSeen = GetLastSeenIndex(tab);
00073:             if (lastSeen >= 0 && _entries.Count > lastSeen) return true;
00074:             int lastSeenCodex = GetLastSeenCodexIndex(tab);
00075:             return lastSeenCodex >= 0 && CodexUnlockCount > lastSeenCodex;
00076:         }
00077:
00078:         /// <summary>Switch the active tab (clamped); raises OnTabChanged on change.</summary>
00079:         public void SwitchTab(int tab)
00080:         {
00081:             int clamped = tab < 0 ? 0 : (tab >= TabCount ? TabCount - 1 : tab);
00082:             if (clamped == ActiveTab) return;
00083:             ActiveTab = clamped;
00084:             _lastSeenIndexPerTab[clamped] = _entries.Count;
00085:             _lastSeenCodexPerTab[clamped] = CodexUnlockCount;
00086:             OnTabChanged?.Invoke(clamped);
00087:         }
00088:
00089:         /// <summary>Record that the UI showed <paramref name="tab"/> (unread reset for it).</summary>
00090:         public void MarkTabViewed(int tab)
00091:         {
00092:             if (tab < 0 || tab >= TabCount) return;
00093:             _lastSeenIndexPerTab[tab] = _entries.Count;
00094:             _lastSeenCodexPerTab[tab] = CodexUnlockCount;
00095:             if (tab == 0)
00096:             {
00097:                 HasUnread = false;
00098:                 NotificationPing = false;
00099:             }
00100:         }
00101:
00102:         // -----------------------------------------------------------------
00103:         // Codex unlocks (docs/ui/JOURNAL_UI_PLAN.md §5, §7)
00104:         // -----------------------------------------------------------------
00105:
00106:         public bool UnlockItemSeen(string itemId) => UnlockCodex(KnowledgeKeys.ItemSeen(itemId));
00107:         public bool UnlockLocationVisited(string locationId) => UnlockCodex(KnowledgeKeys.LocationVisited(locationId));
00108:         public bool UnlockSurvivorMet(string survivorId) => UnlockCodex(KnowledgeKeys.SurvivorMet(survivorId));
00109:         public bool UnlockEventFired(string eventId) => UnlockCodex(KnowledgeKeys.EventFired(eventId));
00110:         public bool UnlockRoomHistorySeen(string vignetteId) => UnlockCodex(KnowledgeKeys.RoomHistorySeen(vignetteId));
00111:         public bool UnlockGlitchNoted(string glitchId) => UnlockCodex(KnowledgeKeys.GlitchNoted(glitchId));
00112:         public bool UnlockWildlifeCaught(string speciesId) => UnlockCodex(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
00113:         public bool UnlockNarrativeDiscovered(string discoveryId) => UnlockCodex(KnowledgeKeys.NarrativeDiscovered(discoveryId));
00114:         public bool UnlockBureaucraticDocument(string docId) => UnlockCodex(KnowledgeKeys.BureaucraticDocument(docId));
00115:         public bool AddKnowledgeEvidence(string survivorId, string knowledgeKey) => UnlockCodex(knowledgeKey);
00116:
00117:         public bool IsItemSeen(string itemId) => _knowledge.Has(KnowledgeKeys.ItemSeen(itemId));
00118:         public bool IsLocationVisited(string locationId) => _knowledge.Has(KnowledgeKeys.LocationVisited(locationId));
00119:         public bool IsSurvivorMet(string survivorId) => _knowledge.Has(KnowledgeKeys.SurvivorMet(survivorId));
00120:         public bool IsEventFired(string eventId) => _knowledge.Has(KnowledgeKeys.EventFired(eventId));
00121:         public bool IsRoomHistorySeen(string vignetteId) => _knowledge.Has(KnowledgeKeys.RoomHistorySeen(vignetteId));
00122:         public bool IsGlitchNoted(string glitchId) => _knowledge.Has(KnowledgeKeys.GlitchNoted(glitchId));
00123:         public bool IsWildlifeCaught(string speciesId) => _knowledge.Has(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
00124:         public bool IsNarrativeDiscovered(string discoveryId) => _knowledge.Has(KnowledgeKeys.NarrativeDiscovered(discoveryId));
00125:         public bool IsBureaucraticDocumentDiscovered(string docId) => _knowledge.Has(KnowledgeKeys.BureaucraticDocument(docId));
00126:
00127:         private bool UnlockCodex(string key)
00128:         {
00129:             if (!_knowledge.Discover(key)) return false;
00130:             CodexUnlockCount++;
00131:             OnCodexUnlocked?.Invoke(key);
00132:             return true;
00133:         }
00134:
00135:         /// <summary>
00136:         /// Wire an object pool (GenericObjectPool) so evicted/cleared entries are
00137:         /// recycled instead of collected, and new entries reuse pooled instances.
00138:         /// Null factory falls back to `new JournalEntry()`; null recycler disables recycling.
00139:         /// </summary>
00140:         public void SetEntryFactory(Func<JournalEntry> factory, Action<JournalEntry> recycler)
00141:         {
00142:             _entryFactory = factory;
00143:             _entryRecycler = recycler;
00144:         }
00145:
00146:         public KnowledgeBase Knowledge => _knowledge;
00147:         /// <summary>
00148:         /// Optional immutable authored-corpus adapter. This changes only the
00149:         /// source body/attribution for a matching producer key; it does not
00150:         /// create another journal state machine or schedule discoveries.
00151:         /// </summary>
00152:         public void BindAuthoredCorpus(JournalCorpusAdapter? adapter)
00153:         {
00154:             _authoredCorpus = adapter;
00155:         }
00156:
00157:         /// <summary>
00158:         /// True when the authored journal corpus is bound. Lets host audit
00159:         /// surfaces (Plan 49 content certification) read live evidence instead
00160:         /// of trusting a loader name.
00161:         /// </summary>
00162:         public bool HasAuthoredCorpus => _authoredCorpus != null;
00163:
00164:         /// <summary>
00165:         /// Activate one authored record from a real producer. Unknown keys are
00166:         /// ignored so generated producer behavior remains unchanged.
00167:         /// </summary>
00168:         public JournalEntry? TryAddAuthoredEntry(
00169:             string knowledgeKey,
00170:             ISurvivorAuthor? fallbackAuthor = null)
00171:         {
00172:             if (!TryGetAuthoredRecord(knowledgeKey, out var authored))
00173:                 return null;
00174:             if (!CanInsertAuthored(authored)) return null;
00175:             if (!_knowledge.Discover(authored.KnowledgeKey)) return null;
00176:             return InsertAuthored(authored, fallbackAuthor);
00177:         }
00178:
00179:         /// <summary>Newest-first log.</summary>
00180:         public IReadOnlyList<JournalEntry> Entries => _entries;
00181:         public int EntryCount => _entries.Count;
00182:         public string LatestText =>
00183:             _entries.Count > 0 ? (_entries[0].Text ?? string.Empty) : string.Empty;
00184:
00185:         /// <summary>Unread badge / notification state for the journal book UI.</summary>
00186:         public bool HasUnread { get; set; }
00187:         /// <summary>True after a new entry until the UI acknowledges the ping.</summary>
00188:         public bool NotificationPing { get; private set; }
00189:         public int NotificationPingCount { get; private set; }
00190:         public bool HudIsOpen { get; set; }
00191:
00192:         /// <summary>
00193:         /// Record a discovery if new. Returns the entry, or null if already known / invalid.
00194:         /// </summary>
00195:         public JournalEntry? TryDiscover(
00196:             string knowledgeKey,
00197:             ISurvivorAuthor author,
00198:             int day,
00199:             float hour = -1f)
00200:         {
00201:             if (string.IsNullOrEmpty(knowledgeKey)) return null;
00202:             if (TryGetAuthoredRecord(knowledgeKey, out var authored))
00203:             {
00204:                 if (!CanInsertAuthored(authored)) return null;
00205:                 if (!_knowledge.Discover(authored.KnowledgeKey)) return null;
00206:                 return InsertAuthored(authored, author);
00207:             }
00208:             if (!_knowledge.Discover(knowledgeKey)) return null;
00209:
00210:             var bias = author != null ? author!.RiskBias : RiskBiasTrait.Realist;
00211:             string text = JournalVoice.ComposeFullText(knowledgeKey, bias, day);
00212:             return InsertEntry(knowledgeKey, text, author!, day, hour);
00213:         }
00214:
00215:         /// <summary>
00216:         /// F3 — record an expedition/knowledge discovery that is BOTH a journal
00217:         /// entry and a codex unlock, through one dedup gate. Exactly once per
00218:         /// key: on the unknown → known transition the entry is written AND
00219:         /// <see cref="OnCodexUnlocked"/> fires; a repeat call returns null and
00220:         /// does nothing. (Calling <see cref="TryDiscover"/> plus
00221:         /// <see cref="AddKnowledgeEvidence"/> separately cannot provide this:
00222:         /// whichever runs second finds the key already known.)
00223:         /// </summary>
00224:         public JournalEntry? TryDiscoverKnowledge(
00225:             string knowledgeKey,
00226:             ISurvivorAuthor? author,
00227:             int day,
00228:             float hour = -1f)
00229:         {
00230:             if (string.IsNullOrEmpty(knowledgeKey)) return null;
00231:             if (TryGetAuthoredRecord(knowledgeKey, out var authored))
00232:             {
00233:                 if (!CanInsertAuthored(authored)) return null;
00234:                 if (!_knowledge.Discover(authored.KnowledgeKey)) return null;
00235:                 CodexUnlockCount++;
00236:                 OnCodexUnlocked?.Invoke(authored.KnowledgeKey);
00237:                 return InsertAuthored(authored, author);
00238:             }
00239:             if (!_knowledge.Discover(knowledgeKey)) return null; // single dedup gate
00240:
00241:             CodexUnlockCount++;
00242:             OnCodexUnlocked?.Invoke(knowledgeKey);
00243:
00244:             var bias = author != null ? author.RiskBias : RiskBiasTrait.Realist;
00245:             string text = JournalVoice.ComposeFullText(knowledgeKey, bias, day);
00246:             return InsertEntry(knowledgeKey, text, author!, day, hour);
00247:         }
00248:
00249:         /// <summary>
00250:         /// Records a freeform discovery entry and unlocks the codex for that knowledge key
00251:         /// through a single deduplication gate. Returns the created entry, or null if already known.
00252:         /// </summary>
00253:         public JournalEntry? TryDiscoverRawKnowledge(
00254:             string knowledgeKey,
00255:             string text,
00256:             ISurvivorAuthor? author,
00257:             int day,
00258:             float hour = -1f)
00259:         {
00260:             if (string.IsNullOrEmpty(knowledgeKey) || string.IsNullOrEmpty(text)) return null;
00261:             if (TryGetAuthoredRecord(knowledgeKey, out var authored))
00262:             {
00263:                 if (!CanInsertAuthored(authored)) return null;
00264:                 if (!_knowledge.Discover(authored.KnowledgeKey)) return null;
00265:                 CodexUnlockCount++;
00266:                 OnCodexUnlocked?.Invoke(authored.KnowledgeKey);
00267:                 return InsertAuthored(authored, author);
00268:             }
00269:             if (!_knowledge.Discover(knowledgeKey)) return null;
00270:
00271:             CodexUnlockCount++;
00272:             OnCodexUnlocked?.Invoke(knowledgeKey);
00273:
00274:             return InsertEntry(knowledgeKey, text, author!, day, hour);
00275:         }
00276:
00277:         /// <summary>
00278:         /// Record a freeform narrative entry once per knowledge key (Prompt #19
00279:         /// ghost-station diary fragments). Deduped via <see cref="KnowledgeBase"/>.
00280:         /// </summary>
00281:         public JournalEntry? TryAddRawEntry(
00282:             string knowledgeKey,
00283:             string text,
00284:             ISurvivorAuthor author,
00285:             int day,
00286:             float hour = -1f)
00287:         {
00288:             if (string.IsNullOrEmpty(knowledgeKey) || string.IsNullOrEmpty(text)) return null;
00289:             if (TryGetAuthoredRecord(knowledgeKey, out var authored))
00290:             {
00291:                 if (!CanInsertAuthored(authored)) return null;
00292:                 if (!_knowledge.Discover(authored.KnowledgeKey)) return null;
00293:                 return InsertAuthored(authored, author);
00294:             }
00295:             if (!_knowledge.Discover(knowledgeKey)) return null;
00296:             return InsertEntry(knowledgeKey, text, author, day, hour);
00297:         }
00298:
00299:         private bool TryGetAuthoredRecord(
00300:             string key,
00301:             out JournalCorpusRecord record)
00302:         {
00303:             if (_authoredCorpus != null && _authoredCorpus.TryGet(key, out record))
00304:                 return true;
00305:             record = null!;
00306:             return false;
00307:         }
00308:
00309:         private bool CanInsertAuthored(JournalCorpusRecord record)
00310:         {
00311:             for (int i = 0; i < _entries.Count; i++)
00312:             {
00313:                 var existing = _entries[i];
00314:                 if (!string.Equals(existing.Id, record.Id, StringComparison.Ordinal))
00315:                     continue;
00316:                 return false;
00317:             }
00318:             return true;
00319:         }
00320:
00321:         private JournalEntry InsertAuthored(
00322:             JournalCorpusRecord record,
00323:             ISurvivorAuthor? fallbackAuthor)
00324:         {
00325:             ISurvivorAuthor author = _authoredCorpus!.ResolveAuthor(record, fallbackAuthor);
00326:             string name = !string.IsNullOrEmpty(author.DisplayName)
00327:                 ? author.DisplayName
00328:                 : (!string.IsNullOrEmpty(author.Id) ? author.Id : "Unknown");
00329:
00330:             var entry = _entryFactory != null ? _entryFactory() : new JournalEntry();
00331:             entry.Id = record.Id;
00332:             entry.Text = record.Text;
00333:             entry.Timestamp = record.Timestamp;
00334:             entry.AuthorName = name;
00335:             entry.AuthorId = author.Id ?? string.Empty;
00336:             entry.KnowledgeKey = record.KnowledgeKey;
00337:             entry.Day = record.Day > 0 ? record.Day : 1;
00338:             entry.Hour = record.Hour;
00339:             PublishEntry(entry);
00340:             return entry;
00341:         }
00342:
00343:         private JournalEntry InsertEntry(
00344:             string knowledgeKey,
00345:             string text,
00346:             ISurvivorAuthor author,
00347:             int day,
00348:             float hour)
00349:         {
00350:             string name = author != null && !string.IsNullOrEmpty(author.DisplayName)
00351:                 ? author.DisplayName
00352:                 : (author != null && !string.IsNullOrEmpty(author.Id) ? author.Id : "Unknown");
00353:             string authorId = author?.Id ?? string.Empty;
00354:
00355:             var entry = _entryFactory != null ? _entryFactory() : new JournalEntry();
00356:             entry.Id = $"journal_{++_seq}_{knowledgeKey}";
00357:             entry.Text = text ?? string.Empty;
00358:             entry.Timestamp = JournalVoice.FormatTimestamp(day, hour);
00359:             entry.AuthorName = name;
00360:             entry.AuthorId = authorId;
00361:             entry.KnowledgeKey = knowledgeKey;
00362:             entry.Day = day > 0 ? day : 1;
00363:             entry.Hour = hour;
00364:
00365:             PublishEntry(entry);
00366:             return entry;
00367:         }
00368:
00369:         private void PublishEntry(JournalEntry entry)
00370:         {
00371:             _entries.Insert(0, entry);
00372:             JournalEntry? evicted = null;
00373:             if (_entries.Count > MaxEntries)
00374:             {
00375:                 evicted = _entries[_entries.Count - 1];
00376:                 _entries.RemoveAt(_entries.Count - 1);
00377:             }
00378:
00379:             HasUnread = true;
00380:             NotificationPing = true;
00381:             NotificationPingCount++;
00382:             OnEntryAdded?.Invoke(entry);
00383:             OnNotificationPing?.Invoke(entry);
00384:
00385:             // Recycle only after subscribers ran: the journal book trims its
00386:             // mirrored list inside OnEntryAdded and must drop the reference first.
00387:             if (evicted != null)
00388:                 _entryRecycler?.Invoke(evicted);
00389:         }
00390:
00391:         public void AcknowledgePing()
00392:         {
00393:             NotificationPing = false;
00394:         }
00395:
00396:         public void MarkRead()
00397:         {
00398:             HasUnread = false;
00399:             NotificationPing = false;
00400:         }
00401:
00402:         public void Clear()
00403:         {
00404:             if (_entryRecycler != null)
00405:             {
00406:                 for (int i = 0; i < _entries.Count; i++)
00407:                     _entryRecycler(_entries[i]);
00408:             }
00409:             _entries.Clear();
00410:             _knowledge.Clear();
00411:             _seq = 0;
00412:             HasUnread = false;
00413:             NotificationPing = false;
00414:             NotificationPingCount = 0;
00415:             HudIsOpen = false;
00416:             ActiveTab = 0;
00417:             CodexUnlockCount = 0;
00418:             for (int i = 0; i < TabCount; i++)
00419:             {
00420:                 _lastSeenIndexPerTab[i] = -1;
00421:                 _lastSeenCodexPerTab[i] = -1;
00422:             }
00423:         }
00424:
00425:         public JournalSave CaptureState()
00426:         {
00427:             return new JournalSave
00428:             {
00429:                 Entries = _entries.ToArray(),
00430:                 Knowledge = _knowledge.CaptureState(),
00431:                 NextSeq = _seq,
00432:                 HasUnread = HasUnread,
00433:                 NotificationPing = NotificationPing,
00434:                 NotificationPingCount = NotificationPingCount,
00435:                 HudIsOpen = HudIsOpen,
00436:                 ActiveTab = ActiveTab,
00437:                 LastSeenIndexPerTab = (int[])_lastSeenIndexPerTab.Clone(),
00438:                 LastSeenCodexPerTab = (int[])_lastSeenCodexPerTab.Clone(),
00439:                 CodexUnlockCount = CodexUnlockCount
00440:             };
00441:         }
00442:
00443:         public void RestoreState(JournalSave save)
00444:         {
00445:             Clear();
00446:             if (save == null) return;
00447:             _seq = Math.Max(0, save.NextSeq);
00448:             HasUnread = save.HasUnread;
00449:             NotificationPing = save.NotificationPing;
00450:             NotificationPingCount = Math.Max(0, save.NotificationPingCount);
00451:             HudIsOpen = save.HudIsOpen;
00452:             ActiveTab = Math.Max(0, Math.Min(save.ActiveTab, TabCount - 1));
00453:             CodexUnlockCount = Math.Max(0, save.CodexUnlockCount);
00454:             if (save.LastSeenIndexPerTab != null)
00455:             {
00456:                 for (int i = 0; i < TabCount && i < save.LastSeenIndexPerTab.Length; i++)
00457:                     _lastSeenIndexPerTab[i] = save.LastSeenIndexPerTab[i];
00458:             }
00459:             if (save.LastSeenCodexPerTab != null)
00460:             {
00461:                 for (int i = 0; i < TabCount && i < save.LastSeenCodexPerTab.Length; i++)
00462:                     _lastSeenCodexPerTab[i] = save.LastSeenCodexPerTab[i];
00463:             }
00464:             _knowledge.RestoreState(save.Knowledge);
00465:             if (save.Entries == null) return;
00466:             for (int i = 0; i < save.Entries.Length && i < MaxEntries; i++)
00467:             {
00468:                 var e = save.Entries[i];
00469:                 if (e == null || string.IsNullOrEmpty(e.Text)) continue;
00470:                 _entries.Add(e);
00471:             }
00472:         }
00473:     }
00474:
00475:     [Serializable]
00476:     public class JournalSave
00477:     {
00478:         public JournalEntry[] Entries;
00479:         public KnowledgeBaseSave Knowledge;
00480:         public int NextSeq;
00481:         public bool HasUnread;
00482:         public bool NotificationPing;
00483:         public int NotificationPingCount;
00484:         public bool HudIsOpen;
00485:         public int ActiveTab;
00486:         public int[] LastSeenIndexPerTab;
00487:         public int[] LastSeenCodexPerTab;
00488:         public int CodexUnlockCount;
00489:     }
00490: }
```

## `src/Main.Narrative.cs` — 728 lines; 29,845 bytes; SHA-256 `b588f3d83df8088676183b79152ffa57c62a3b5d30ed8c7cb73ae0a4b7ece863`
Declaration index:
- 00034: public partial class Main : Control
- 00046: private void SetupJournal()
- 00117: private void DiscoverBureaucraticDocuments(string producerId)
- 00145: private void DiscoverPersonalLetterRecords(string producerId)
- 00175: private void DiscoverAbyssalAnomalyRecords(string producerId)
- 00207: private void DiscoverFringeCultRecords(string producerId)
- 00239: private void DiscoverPaperPrintingRecords(string producerId)
- 00270: private void DiscoverBoneHornRecords(string producerId)
- 00296: private void ToggleJournal()
- 00302: private void SaveJournal()
- 00309: private void SetupEventAdapter(bool reloadFromDisk = false)
- 00344: private void FlushJournalIfDirty()
- 00349: private void FlushNarrativeIfDirty()
- 00354: private void FlushEventAdapterIfDirty()
- 00359: private void SetupNarrative(bool reloadEventAdapter = false)
- 00376: private void BindExpeditionJournalIfReady()
- 00389: private void EnsureNarrativeSession()
- 00398: private void ConfigureNarrativeArcRuntime()
- 00416: private bool IsNarrativeSurvivorPresent(string survivorId)
- 00439: private void ApplyNarrativeMorale(string survivorId, int delta, bool shelterWide)
- 00466: private void GrantNarrativeIntel(string canonicalFactionId)
- 00483: private void OfferNarrativeExpedition(string locationId)
- 00497: private void ApplyNarrativeStanding(string canonicalFactionId, int delta)
- 00504: private void OpenNarrativeArcModal()
- 00523: private void OnNarrativeArcChoiceSelected(string eventId, string choiceId)
- 00543: private void OnNarrativeArcAcknowledged(string eventId)
- 00557: private void SaveNarrative()
- 00567: private void SaveEventAdapter()
- 00576: private void OnNarrativeOpenClicked()
- 00583: private void SetupRadio()
- 00683: private void TryBridgeDistressFromTriangulation(string locationOrSignalId)
- 00703: private void SaveRadio()
- 00712: private void CloseRadioPanel()
- 00717: private void CloseJournalPanel()
- 00722: private void CloseJournalDetailPanel()
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
00017: using Ashfall.Core.Narrative;
00018: using Ashfall.Core.YearOfAsh;
00019: using Ashfall.Core.Radio;
00020: using Ashfall.Core.Radiation;
00021: using Ashfall.Core.Factions;
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
00036:         // ── Narrative fields (GAP-ARCH-01 Phase 1) ──
00037:         private NarrativeHostSession _narrative = null!;
00038:         private bool _narrativeDirty;
00039:         private RadioHostSession _radio = null!;
00040:         private CraftingHostSession _crafting = null!;
00041:         private bool _craftingDirty;
00042:         private JournalSystem _journal = null!;
00043:         private BureaucraticDocumentDiscoverySystem _bureaucraticDocumentDiscovery = null!;
00044:         private bool _hostEventAdapterDirty;
00045:
00046:         private void SetupJournal()
00047:         {
00048:             if (_journal != null) return;
00049:
00050:             var catalogs = CatalogJsonLoader.Load(new FileSystemIO(), _dataDir);
00051:             _journal = new JournalSystem();
00052:             if (catalogs.BureaucraticDocuments != null)
00053:             {
00054:                 _bureaucraticDocumentDiscovery = new BureaucraticDocumentDiscoverySystem(
00055:                     catalogs.BureaucraticDocuments);
00056:             }
00057:             // Diegetic journal prose — without this binding every entry renders
00058:             // the generic "Something changed." placeholder. LoadDefault resolves
00059:             // the data dir and degrades to an empty catalog if absent.
00060:             JournalVoice.BindCatalog(JournalVoiceProseCatalogLoader.LoadDefault());
00061:             var authoredCorpus = new JournalCorpusCatalogLoader(
00062:                 new FileSystemIO(),
00063:                 new SystemTextJsonSerializer()).Load(_dataDir);
00064:             var authoredAuthors = JournalDemoHarness.BuildAuthors(catalogs).Values;
00065:             _journal.BindAuthoredCorpus(new JournalCorpusAdapter(authoredCorpus, authoredAuthors));
00066:             BindJournalWorldProducerIfReady();
00067:             // Mark dirty rather than writing the whole save file per entry; the
00068:             // _Process tick flushes it. Seeding adds many entries in one frame and
00069:             // used to rewrite journal_save.json once for each of them.
00070:             _journal.OnEntryAdded += _ => _journalDirty = true;
00071:             _journal.OnTabChanged += _ => _journalDirty = true;
00072:             _journal.OnCodexUnlocked += _ => _journalDirty = true;
00073:
00074:             _journalCodex = new JournalCodex(_journal, catalogs);
00075:
00076:             if (_journalBook == null || !_journalBook.IsInsideTree())
00077:             {
00078:                 _journalBook = new JournalBookUI();
00079:                 _journalBook.SetAnchorsPreset(LayoutPreset.FullRect);
00080:                 AddChild(_journalBook);
00081:             }
00082:             _journalBook.Bind(
00083:                 _journal,
00084:                 tab => _journalCodex.BuildRows(tab),
00085:                 tab => _journal.HasUnreadForTab(tab),
00086:                 () => _simDay);
00087:             _journalBook.OnClosed += SaveJournal;
00088:
00089:             if (JournalSaveStore.Exists)
00090:             {
00091:                 var save = JournalSaveStore.Load();
00092:                 if (save != null) _journal.RestoreState(save);
00093:                 _journalBook.SetEntries(_journal.Entries);
00094:                 _journalBook.ApplyUiState(
00095:                     _journal.HudIsOpen,
00096:                     _journal.HasUnread,
00097:                     _journal.NotificationPing,
00098:                     _journal.ActiveTab);
00099:                 GD.Print("[Ashfall Godot] Journal restored from save.");
00100:             }
00101:             else
00102:             {
00103:                 JournalDemoHarness.Seed(_journal, catalogs);
00104:                 _journalBook.SetEntries(_journal.Entries);
00105:                 SaveJournal();
00106:                 GD.Print("[Ashfall Godot] Journal seeded with opening-day entries.");
00107:             }
00108:
00109:             UpdateStatus();
00110:         }
00111:
00112:         /// <summary>
00113:         /// Discover authored shelter paperwork through an explicit physical or
00114:         /// administrative producer. The journal knowledge ledger is the only
00115:         /// persisted discovery state; reading a document has no simulation effect.
00116:         /// </summary>
00117:         private void DiscoverBureaucraticDocuments(string producerId)
00118:         {
00119:             if (_journal == null || _bureaucraticDocumentDiscovery == null) return;
00120:
00121:             int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00122:             var results = _bureaucraticDocumentDiscovery.DiscoverByProducer(
00123:                 producerId,
00124:                 day,
00125:                 _journal);
00126:             int discovered = 0;
00127:             for (int i = 0; i < results.Count; i++)
00128:             {
00129:                 if (results[i].Changed) discovered++;
00130:             }
00131:
00132:             if (discovered > 0)
00133:             {
00134:                 _journalDirty = true;
00135:                 if (_statusLabel != null)
00136:                     _statusLabel.Text = $"[DOCUMENTS] {discovered} institutional record(s) added to the journal.";
00137:             }
00138:         }
00139:
00140:         /// <summary>
00141:         /// Plan 150: reveal personal/unsent letters assigned to an explicit
00142:         /// shelter-room producer. Journal knowledge only — never inventory or
00143:         /// quest authority.
00144:         /// </summary>
00145:         private void DiscoverPersonalLetterRecords(string producerId)
00146:         {
00147:             var catalog = _journalCodex?.Catalogs?.NarrativeDiscoveries;
00148:             if (_journal == null || catalog == null || string.IsNullOrEmpty(producerId)) return;
00149:
00150:             int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00151:             int discovered = 0;
00152:             var records = catalog.GetByProducer(producerId);
00153:             for (int i = 0; i < records.Count; i++)
00154:             {
00155:                 var record = records[i];
00156:                 if (!PersonalLetterRuntimeContract.IsSourceCatalog(record.SourceCatalog)
00157:                     || record.MinDay > day)
00158:                     continue;
00159:                 if (catalog.TryDiscover(record.DiscoveryId, _journal, out _))
00160:                     discovered++;
00161:             }
00162:
00163:             if (discovered > 0)
00164:             {
00165:                 _journalDirty = true;
00166:                 if (_statusLabel != null)
00167:                     _statusLabel.Text = $"[CORRESPONDENCE] {discovered} personal letter(s) added to the journal.";
00168:             }
00169:         }
00170:
00171:         /// <summary>
00172:         /// Plan 151: reveal activated abyssal anomaly records for a map or
00173:         /// room producer. Deferred Phase-2 rows stay locked.
00174:         /// </summary>
00175:         private void DiscoverAbyssalAnomalyRecords(string producerId)
00176:         {
00177:             var catalog = _journalCodex?.Catalogs?.NarrativeDiscoveries;
00178:             if (_journal == null || catalog == null || string.IsNullOrEmpty(producerId)) return;
00179:
00180:             int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00181:             int discovered = 0;
00182:             var records = catalog.GetByProducer(producerId);
00183:             for (int i = 0; i < records.Count; i++)
00184:             {
00185:                 var record = records[i];
00186:                 if (!AbyssalAnomaliesRuntimeContract.IsSourceCatalog(record.SourceCatalog)
00187:                     || !AbyssalAnomaliesRuntimeContract.IsActivatedSourceRecord(record.SourceRecordId)
00188:                     || record.MinDay > day)
00189:                     continue;
00190:                 if (catalog.TryDiscover(record.DiscoveryId, _journal, out _))
00191:                     discovered++;
00192:             }
00193:
00194:             if (discovered > 0)
00195:             {
00196:                 _journalDirty = true;
00197:                 if (_statusLabel != null)
00198:                     _statusLabel.Text = $"[ARCHIVE] {discovered} abyssal anomaly record(s) added to the journal.";
00199:             }
00200:         }
00201:
00202:         /// <summary>
00203:         /// Plan 153: reveal only fringe-cult records assigned to the explicit
00204:         /// physical/archive producer. This shares the Plan 135 narrative
00205:         /// discovery ledger; doctrine is never sent to a simulation authority.
00206:         /// </summary>
00207:         private void DiscoverFringeCultRecords(string producerId)
00208:         {
00209:             var catalog = _journalCodex?.Catalogs?.NarrativeDiscoveries;
00210:             if (_journal == null || catalog == null || string.IsNullOrEmpty(producerId)) return;
00211:
00212:             int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00213:             int discovered = 0;
00214:             var records = catalog.GetByProducer(producerId);
00215:             for (int i = 0; i < records.Count; i++)
00216:             {
00217:                 var record = records[i];
00218:                 if (!FringeCultRuntimeContract.IsSourceCatalog(record.SourceCatalog)
00219:                     || record.MinDay > day)
00220:                     continue;
00221:                 if (catalog.TryDiscover(record.DiscoveryId, _journal, out _))
00222:                     discovered++;
00223:             }
00224:
00225:             if (discovered > 0)
00226:             {
00227:                 _journalDirty = true;
00228:                 if (_statusLabel != null)
00229:                     _statusLabel.Text = $"[ARCHIVE] {discovered} fringe-cult record(s) added to the journal.";
00230:             }
00231:         }
00232:
00233:         /// <summary>
00234:         /// Plan 156: reveal authored paper-making and printing records from an
00235:         /// explicit room, archive, or map-location producer. The source
00236:         /// measurements are projected into Journal only and never enter
00237:         /// inventory, crafting, faction, research, or document authority.
00238:         /// </summary>
00239:         private void DiscoverPaperPrintingRecords(string producerId)
00240:         {
00241:             var catalog = _journalCodex?.Catalogs?.NarrativeDiscoveries;
00242:             if (_journal == null || catalog == null || string.IsNullOrEmpty(producerId)) return;
00243:
00244:             int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00245:             int discovered = 0;
00246:             var records = catalog.GetByProducer(producerId);
00247:             for (int i = 0; i < records.Count; i++)
00248:             {
00249:                 var record = records[i];
00250:                 if (!PaperPrintRuntimeContract.IsSourceCatalog(record.SourceCatalog)
00251:                     || record.MinDay > day)
00252:                     continue;
00253:                 if (catalog.TryDiscover(record.DiscoveryId, _journal, out _))
00254:                     discovered++;
00255:             }
00256:
00257:             if (discovered > 0)
00258:             {
00259:                 _journalDirty = true;
00260:                 if (_statusLabel != null)
00261:                     _statusLabel.Text = $"[ARCHIVE] {discovered} paper/print record(s) added to the journal.";
00262:             }
00263:         }
00264:
00265:         /// <summary>
00266:         /// Plan 160: reveal only bone, horn and antler records assigned to an
00267:         /// explicit workshop, archive or world producer. Source animal labels
00268:         /// remain historical provenance and never target living companions.
00269:         /// </summary>
00270:         private void DiscoverBoneHornRecords(string producerId)
00271:         {
00272:             var catalog = _journalCodex?.Catalogs?.NarrativeDiscoveries;
00273:             if (_journal == null || catalog == null || string.IsNullOrEmpty(producerId)) return;
00274:
00275:             int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00276:             int discovered = 0;
00277:             var records = catalog.GetByProducer(producerId);
00278:             for (int i = 0; i < records.Count; i++)
00279:             {
00280:                 var record = records[i];
00281:                 if (!BoneHornRuntimeContract.IsSourceCatalog(record.SourceCatalog)
00282:                     || record.MinDay > day)
00283:                     continue;
00284:                 if (catalog.TryDiscover(record.DiscoveryId, _journal, out _))
00285:                     discovered++;
00286:             }
00287:
00288:             if (discovered > 0)
00289:             {
00290:                 _journalDirty = true;
00291:                 if (_statusLabel != null)
00292:                     _statusLabel.Text = $"[ARCHIVE] {discovered} bone/horn craft record(s) added to the journal.";
00293:             }
00294:         }
00295:
00296:         private void ToggleJournal()
00297:         {
00298:             if (_journalBook != null) _journalBook.Toggle();
00299:             UpdateStatus();
00300:         }
00301:
00302:         private void SaveJournal()
00303:         {
00304:             if (_journal == null) return;
00305:             if (CaptureSection("journal", JournalSaveStore.TryCapturePersisted(_journal.CaptureState())))
00306:                 _journalDirty = false;
00307:         }
00308:
00309:         private void SetupEventAdapter(bool reloadFromDisk = false)
00310:         {
00311:             if (_hostEventAdapter != null && !reloadFromDisk) return;
00312:
00313:             if (_hostEventAdapter != null)
00314:             {
00315:                 _hostEventAdapter.Dispose();
00316:                 _hostEventAdapter = null!;
00317:             }
00318:
00319:             SetupJournal();
00320:             if (_eventBus == null) _eventBus = new Ashfall.Core.Events.SimpleEventBus();
00321:
00322:             // The adapter is the sole owner of mutable event progress. Restore the
00323:             // selected campaign's projected host_event payload before any day tick
00324:             // can evaluate triggers; the catalog session remains read-only.
00325:             _hostEventAdapter = new AtomicWar.GodotApp.Host.HostEventAdapter(_eventBus, _journal);
00326:             var loadedEventState = HostEventSaveStore.TryLoad();
00327:             if (loadedEventState != null)
00328:             {
00329:                 _hostEventAdapter.RestoreState(loadedEventState);
00330:             }
00331:             _hostEventAdapter.OnEventDispatched += (id, desc) =>
00332:             {
00333:                 if (_statusLabel != null)
00334:                     _statusLabel.Text = $"[EVENT DISPATCHED] {id}: {desc}";
00335:                 _journalDirty = true;
00336:             };
00337:             _hostEventAdapter.StateChanged += () => _hostEventAdapterDirty = true;
00338:         }
00339:
00340:         /// <summary>
00341:         /// Writes the journal only when something actually changed. Called from the
00342:         /// throttled _Process tick so a burst of entries costs one file write.
00343:         /// </summary>
00344:         private void FlushJournalIfDirty()
00345:         {
00346:             if (_journalDirty) SaveJournal();
00347:         }
00348:
00349:         private void FlushNarrativeIfDirty()
00350:         {
00351:             if (_narrativeDirty) SaveNarrative();
00352:         }
00353:
00354:         private void FlushEventAdapterIfDirty()
00355:         {
00356:             if (_hostEventAdapterDirty) SaveEventAdapter();
00357:         }
00358:
00359:         private void SetupNarrative(bool reloadEventAdapter = false)
00360:         {
00361:             EnsureNarrativeSession();
00362:             ConfigureNarrativeArcRuntime();
00363:
00364:             // Narrative setup is part of both composition and restore. Initialize
00365:             // the event adapter here so campaign state is loaded before its first
00366:             // day-owner evaluation, without coupling it to the catalog read-model.
00367:             SetupEventAdapter(reloadEventAdapter);
00368:
00369:             // F3 — the expedition host's journal seam binds once the journal
00370:             // authority exists (SetupEventAdapter → SetupJournal created it).
00371:             BindExpeditionJournalIfReady();
00372:         }
00373:
00374:         /// <summary>F3 — share the one journal authority with the expedition
00375:         /// host's consequence applier. No-op until both sessions exist.</summary>
00376:         private void BindExpeditionJournalIfReady()
00377:         {
00378:             if (_expeditions == null || _journal == null) return;
00379:             if (_expeditions.Journal == _journal) return;
00380:             _expeditions.Journal = _journal;
00381:         }
00382:
00383:         /// <summary>
00384:         /// F1–F4 — ensure the ONE narrative-encounter engine exists (catalog
00385:         /// loaded, save restored) without touching the event adapter. The
00386:         /// expedition host shares this engine so depletion, resolution history,
00387:         /// and the pending queue have a single save-backed authority.
00388:         /// </summary>
00389:         private void EnsureNarrativeSession()
00390:         {
00391:             if (_narrative != null) return;
00392:             SetupCampaignDay();
00393:             _narrative = NarrativeHostSession.Create(_dataDir, _campaignDay.Rng);
00394:             _narrative.StateChanged += () => _narrativeDirty = true;
00395:             GD.Print("[Ashfall Godot] Narrative host ready.");
00396:         }
00397:
00398:         private void ConfigureNarrativeArcRuntime()
00399:         {
00400:             if (_narrative == null) return;
00401:
00402:             var adapter = new NarrativeArcConsequenceAdapter
00403:             {
00404:                 MoralePreflight = CanApplyNarrativeMorale,
00405:                 MoraleCommit = ApplyNarrativeMorale,
00406:                 IntelPreflight = CanGrantNarrativeIntel,
00407:                 IntelCommit = GrantNarrativeIntel,
00408:                 ExpeditionPreflight = CanOfferNarrativeExpedition,
00409:                 ExpeditionCommit = OfferNarrativeExpedition,
00410:                 StandingPreflight = CanApplyNarrativeStanding,
00411:                 StandingCommit = ApplyNarrativeStanding
00412:             };
00413:             _narrative.ConfigureArcRuntime(IsNarrativeSurvivorPresent, adapter);
00414:         }
00415:
00416:         private bool IsNarrativeSurvivorPresent(string survivorId)
00417:         {
00418:             SetupSurvivors();
00419:             if (_survivors == null) return false;
00420:             var survivor = _survivors.Find(survivorId);
00421:             if (survivor == null || !survivor.IsAliveState) return false;
00422:             return _survivors.GetSurvivorLocation(survivorId).Kind == SurvivorExposureLocation.ShelterInterior;
00423:         }
00424:
00425:         private (bool ok, string reason) CanApplyNarrativeMorale(string survivorId, int delta, bool shelterWide)
00426:         {
00427:             SetupSurvivors();
00428:             if (shelterWide)
00429:             {
00430:                 bool resident = _survivors.RosterState.Any(s => s != null && s.IsAliveState &&
00431:                     _survivors.GetSurvivorLocation(s.Id).Kind == SurvivorExposureLocation.ShelterInterior);
00432:                 return resident ? (true, string.Empty) : (false, "no living resident can receive morale");
00433:             }
00434:             return IsNarrativeSurvivorPresent(survivorId)
00435:                 ? (true, string.Empty)
00436:                 : (false, "the addressed survivor is unavailable");
00437:         }
00438:
00439:         private void ApplyNarrativeMorale(string survivorId, int delta, bool shelterWide)
00440:         {
00441:             SetupSurvivors();
00442:             if (!shelterWide)
00443:             {
00444:                 var survivor = _survivors.Find(survivorId);
00445:                 if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
00446:                 return;
00447:             }
00448:
00449:             foreach (var survivor in _survivors.RosterState
00450:                 .Where(s => s != null && s.IsAliveState &&
00451:                     _survivors.GetSurvivorLocation(s.Id).Kind == SurvivorExposureLocation.ShelterInterior)
00452:                 .OrderBy(s => s.Id, StringComparer.Ordinal))
00453:             {
00454:                 _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
00455:             }
00456:         }
00457:
00458:         private (bool ok, string reason) CanGrantNarrativeIntel(string canonicalFactionId)
00459:         {
00460:             SetupJournal();
00461:             return FactionStandingIdResolver.IsKnownFaction(canonicalFactionId) && _journal != null
00462:                 ? (true, string.Empty)
00463:                 : (false, "canonical faction intel or journal authority is unavailable");
00464:         }
00465:
00466:         private void GrantNarrativeIntel(string canonicalFactionId)
00467:         {
00468:             SetupJournal();
00469:             if (_journal == null) return;
00470:             _journal.Knowledge.Discover(KnowledgeKeys.FactionIntel(canonicalFactionId));
00471:             _journalDirty = true;
00472:         }
00473:
00474:         private (bool ok, string reason) CanOfferNarrativeExpedition(string locationId)
00475:         {
00476:             SetupExpeditions();
00477:             bool found = _expeditions != null && _expeditions.Definitions.Any(d => d != null && d.id == locationId);
00478:             return found
00479:                 ? (true, string.Empty)
00480:                 : (false, "narrative expedition target is not in the expedition catalog");
00481:         }
00482:
00483:         private void OfferNarrativeExpedition(string locationId)
00484:         {
00485:             if (_statusLabel != null)
00486:                 _statusLabel.Text = "Expedition opportunity recorded. Review it in Expeditions; normal dispatch requirements apply.";
00487:         }
00488:
00489:         private (bool ok, string reason) CanApplyNarrativeStanding(string canonicalFactionId, int delta)
00490:         {
00491:             SetupYearOfAsh();
00492:             return FactionStandingIdResolver.IsKnownFaction(canonicalFactionId) && _yearOfAsh != null
00493:                 ? (true, string.Empty)
00494:                 : (false, "canonical faction standing authority is unavailable");
00495:         }
00496:
00497:         private void ApplyNarrativeStanding(string canonicalFactionId, int delta)
00498:         {
00499:             SetupYearOfAsh();
00500:             _yearOfAsh?.FactionWar.ModifyStanding(canonicalFactionId, delta);
00501:             _yearOfAshDirty = true;
00502:         }
00503:
00504:         private void OpenNarrativeArcModal()
00505:         {
00506:             SetupEchoes();
00507:             if (_echoes?.PendingEcho != null)
00508:             {
00509:                 OpenEchoModal();
00510:                 return;
00511:             }
00512:
00513:             SetupNarrative();
00514:             var pending = _narrative.PendingArcEvent;
00515:             if (pending == null)
00516:             {
00517:                 if (_statusLabel != null) _statusLabel.Text = "No narrative arc event is waiting for a decision.";
00518:                 return;
00519:             }
00520:             _narrativeArcModal.Display(pending, _simDay);
00521:         }
00522:
00523:         private void OnNarrativeArcChoiceSelected(string eventId, string choiceId)
00524:         {
00525:             if (!string.IsNullOrEmpty(eventId) && eventId.StartsWith("echo_", StringComparison.Ordinal))
00526:             {
00527:                 ResolveEchoChoice(eventId, choiceId);
00528:                 return;
00529:             }
00530:
00531:             SetupNarrative();
00532:             var result = _narrative.ResolveArcChoice(eventId, choiceId, _simDay);
00533:             if (!result.Succeeded)
00534:             {
00535:                 if (_statusLabel != null) _statusLabel.Text = "Narrative choice refused: " + result.Reason;
00536:                 return;
00537:             }
00538:             SaveNarrative();
00539:             if (_statusLabel != null) _statusLabel.Text = _narrative.LastEvent;
00540:             _narrativeArcModal.DisplayOutcome(_narrative.LastEvent);
00541:         }
00542:
00543:         private void OnNarrativeArcAcknowledged(string eventId)
00544:         {
00545:             SetupNarrative();
00546:             var result = _narrative.AcknowledgeArcEvent(eventId, _simDay);
00547:             if (!result.Succeeded)
00548:             {
00549:                 if (_statusLabel != null) _statusLabel.Text = "Narrative event refused: " + result.Reason;
00550:                 return;
00551:             }
00552:             SaveNarrative();
00553:             if (_statusLabel != null) _statusLabel.Text = _narrative.LastEvent;
00554:             _narrativeArcModal.DisplayOutcome(_narrative.LastEvent);
00555:         }
00556:
00557:         private void SaveNarrative()
00558:         {
00559:             if (_narrative == null) return;
00560:             if (CaptureSection("narrative", NarrativeSaveStore.TryCapturePersisted(_narrative.CaptureSave())))
00561:             {
00562:                 _narrativeDirty = false;
00563:                 GD.Print("[Ashfall Godot] Narrative save written.");
00564:             }
00565:         }
00566:
00567:         private void SaveEventAdapter()
00568:         {
00569:             if (_hostEventAdapter == null) return;
00570:             if (CaptureSection("host_event", HostEventSaveStore.TryCapturePersisted(_hostEventAdapter.CaptureState())))
00571:             {
00572:                 _hostEventAdapterDirty = false;
00573:             }
00574:         }
00575:
00576:         private void OnNarrativeOpenClicked()
00577:         {
00578:             SetupNarrative();
00579:             _statusLabel.Text = _narrative.SelectDemo("cautious", 0.5f, "loc_denial_cut_substation")
00580:                 + "\n" + _narrative.StatusLine();
00581:         }
00582:
00583:         private void SetupRadio()
00584:         {
00585:             if (_radio != null)
00586:             {
00587:                 _radio.SetDay(_simDay);
00588:                 return;
00589:             }
00590:
00591:             SetupJournal();
00592:             SetupCampaignDay();
00593:             _radio = RadioHostSession.Create(_dataDir, _core != null ? _core.Clock.Day : _simDay, _campaignDay.Rng);
00594:             _radio.StateChanged += () => _radioPanel?.RefreshView();
00595:             _radio.RescueMissions.OnIgnoreConsequence += (mission, tokens, standingFactionId, day) =>
00596:             {
00597:                 // Core raised the fact once; the host applies standing + journal.
00598:                 if (!string.IsNullOrEmpty(standingFactionId))
00599:                 {
00600:                     try
00601:                     {
00602:                         EnsureSharedFactionStance().ModifyTrust(standingFactionId, -10);
00603:                     }
00604:                     catch (InvalidOperationException)
00605:                     {
00606:                         // Faction stance authority may be offline during sparse boot.
00607:                     }
00608:                 }
00609:                 _journal?.TryAddRawEntry(
00610:                     $"distress_ignored_{mission.QuestId}_{day}",
00611:                     $"The {mission.SignalId} call went unanswered past its deadline. It will not be answered again.",
00612:                     null!,
00613:                     day);
00614:             };
00615:             _radio.RescueMissions.OnRewardsGranted += (mission, items, rep) =>
00616:             {
00617:                 SetupInventory();
00618:                 int stowed = 0;
00619:                 int failed = 0;
00620:                 if (_inventory?.Inventory != null && items != null)
00621:                 {
00622:                     for (int i = 0; i < items.Count; i++)
00623:                     {
00624:                         string itemId = items[i];
00625:                         if (string.IsNullOrEmpty(itemId)) continue;
00626:                         if (_inventory.Inventory.TryProduce(itemId, 1))
00627:                             stowed++;
00628:                         else
00629:                             failed++;
00630:                     }
00631:                 }
00632:                 else if (items != null)
00633:                 {
00634:                     failed = items.Count;
00635:                 }
00636:                 if (rep != 0 && !string.IsNullOrEmpty(mission.FactionTag))
00637:                 {
00638:                     try
00639:                     {
00640:                         EnsureSharedFactionStance().ModifyTrust(mission.FactionTag, rep);
00641:                     }
00642:                     catch (InvalidOperationException)
00643:                     {
00644:                         // Faction stance authority may be offline during sparse boot;
00645:                         // item grants above still apply.
00646:                     }
00647:                 }
00648:                 string stowNote = failed > 0
00649:                     ? $"Rewards partially stowed ({stowed} ok, {failed} failed, rep {rep})."
00650:                     : $"Rewards stowed ({stowed} lines, rep {rep}).";
00651:                 _journal?.TryAddRawEntry(
00652:                     $"distress_reward_{mission.QuestId}_{_radio.Day}",
00653:                     $"Distress rescue settled: {mission.OutcomeSummary} {stowNote}",
00654:                     null!,
00655:                     _radio.Day);
00656:             };
00657:             _radio.Triangulation.OnLocationRevealed += locId =>
00658:             {
00659:                 _journal?.TryAddRawEntry(
00660:                     $"sig_disc_{locId}_{_radio.Day}",
00661:                     $"Direction-finding telemetry confirmed active radio emissions at {locId}.",
00662:                     null!,
00663:                     _radio.Day);
00664:                 // Continuous DF yields a rumor fix — not an instant surveyed Discover.
00665:                 // Exact-fix HF intercepts remain on ShelterRadioStationSystem → Discover.
00666:                 bool rumored = _world?.WastelandMap?.DiscoverRumor(
00667:                     locId,
00668:                     sourceId: "signal_triangulation",
00669:                     day: _radio.Day,
00670:                     confidence: InformationConfidence.Medium) == true;
00671:                 TryBridgeDistressFromTriangulation(locId);
00672:                 GD.Print(rumored
00673:                     ? $"[Ashfall Godot] Triangulation rumored wasteland location '{locId}'."
00674:                     : $"[Ashfall Godot] Triangulation revealed '{locId}' (no map node / already known).");
00675:             };
00676:             GD.Print("[Ashfall Godot] Radio host ready.");
00677:         }
00678:
00679:         /// <summary>
00680:         /// When continuous DF resolves a fingerprint-mapped location, mark any
00681:         /// active distress whose signal id or revealed location matches.
00682:         /// </summary>
00683:         private void TryBridgeDistressFromTriangulation(string locationOrSignalId)
00684:         {
00685:             if (_radio?.DistressSystem == null || string.IsNullOrEmpty(locationOrSignalId)) return;
00686:
00687:             if (_radio.DistressSystem.MarkTriangulated(locationOrSignalId))
00688:                 return;
00689:
00690:             var catalog = _radio.Triangulation.Catalog;
00691:             if (catalog == null) return;
00692:             foreach (var fp in catalog.FingerprintsBySignal.Values)
00693:             {
00694:                 if (fp == null) continue;
00695:                 if (string.Equals(fp.mapped_location_id, locationOrSignalId, StringComparison.Ordinal)
00696:                     || string.Equals(fp.signal_id, locationOrSignalId, StringComparison.Ordinal))
00697:                 {
00698:                     _radio.DistressSystem.MarkTriangulated(fp.signal_id);
00699:                 }
00700:             }
00701:         }
00702:
00703:         private void SaveRadio()
00704:         {
00705:             if (_radio == null) return;
00706:             if (CaptureSection("radio", RadioSaveStore.TryCapturePersisted(_radio.CaptureSave())))
00707:             {
00708:                 GD.Print("[Ashfall Godot] Radio save written.");
00709:             }
00710:         }
00711:
00712:         private void CloseRadioPanel()
00713:         {
00714:             _radioPanel.Visible = false;
00715:         }
00716:
00717:         private void CloseJournalPanel()
00718:         {
00719:             _journalPanel.Visible = false;
00720:         }
00721:
00722:         private void CloseJournalDetailPanel()
00723:         {
00724:             _journalDetailPanel.Visible = false;
00725:         }
00726:
00727:     }
00728: }
```

## `src/Muster/JournalWitnessPanel.cs` — 91 lines; 3,559 bytes; SHA-256 `283cb24de312cddf7afd232b74aa946447c8172dfcb6db5d6269412eac439c33`
Declaration index:
- 00018: public partial class JournalWitnessPanel : PanelContainer
- 00051: public void Bind(List<WitnessDefinition> witnesses)
- 00062: public void RefreshView(int day, RiskBiasTrait authorBias)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: #pragma warning disable CS8618
00004: using Godot;
00005: using AtomicWar.GodotApp.UI;
00006: using Ashfall.Core.UI;
00007: using Ashfall.Core.Journal;
00008: using Ashfall.Core.Muster;
00009:
00010: namespace AtomicWar.GodotApp.Muster
00011: {
00012:     /// <summary>
00013:     /// Section III witness panel: renders the three Harven succession accounts
00014:     /// (muster_witnesses.json) with the journal's trait-based framing —
00015:     /// JournalVoice.ComposeFullText per the authoring survivor's RiskBiasTrait.
00016:     /// Thin presentation only; framing logic lives in the core journal.
00017:     /// </summary>
00018:     public partial class JournalWitnessPanel : PanelContainer
00019:     {
00020:         private List<WitnessDefinition> _witnesses;
00021:         private VBoxContainer _witnessList;
00022:
00023:         public override void _Ready()
00024:         {
00025:             SetAnchorsPreset(LayoutPreset.TopRight);
00026:             CustomMinimumSize = new Vector2(400, 240);
00027:
00028:             var rootVbox = new VBoxContainer();
00029:             rootVbox.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
00030:             AddChild(rootVbox);
00031:
00032:             var title = new Label
00033:             {
00034:                 Text = "THE UNSIGNED ORDER — THREE ACCOUNTS",
00035:                 HorizontalAlignment = HorizontalAlignment.Center
00036:             };
00037:             title.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
00038:             rootVbox.AddChild(title);
00039:
00040:             var scroll = new ScrollContainer
00041:             {
00042:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
00043:                 CustomMinimumSize = new Vector2(0, 200)
00044:             };
00045:             rootVbox.AddChild(scroll);
00046:
00047:             _witnessList = new VBoxContainer();
00048:             scroll.AddChild(_witnessList);
00049:         }
00050:
00051:         public void Bind(List<WitnessDefinition> witnesses)
00052:         {
00053:             _witnesses = witnesses ?? new List<WitnessDefinition>();
00054:         }
00055:
00056:         /// <summary>
00057:         /// Renders the witnesses whose day gate has opened. The framing is
00058:         /// keyed to the RECORDING survivor's RiskBiasTrait (Section III) —
00059:         /// whoever wrote it down colours how it reads — never to a fixed
00060:         /// per-witness bias.
00061:         /// </summary>
00062:         public void RefreshView(int day, RiskBiasTrait authorBias)
00063:         {
00064:             if (_witnessList == null) return;
00065:             AshfallUiHelpers.EmptyChildren(_witnessList);for (int i = 0; i < _witnesses.Count; i++)
00066:             {
00067:                 var w = _witnesses[i];
00068:                 if (day < w.dayMin) continue;
00069:
00070:                 string framing = JournalVoice.ComposeFullText(w.knowledgeKey, authorBias, day);
00071:                 var card = new VBoxContainer();
00072:                 card.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingXs);
00073:
00074:                 var header = new Label { Text = $"{w.witnessName} — {w.locationId}" };
00075:                 header.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
00076:                 header.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
00077:                 card.AddChild(header);
00078:
00079:                 var body = new Label
00080:                 {
00081:                     Text = w.body + "\n" + framing,
00082:                     AutowrapMode = TextServer.AutowrapMode.WordSmart
00083:                 };
00084:                 body.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
00085:                 card.AddChild(body);
00086:
00087:                 _witnessList.AddChild(card);
00088:             }
00089:         }
00090:     }
00091: }
```

## `src/UI/MusterPanel.cs` — 388 lines; 19,200 bytes; SHA-256 `99db5e7c8e43b08395d274505f09b61915f5e85bb8049b5240a4ad9d45c8ac57`
Declaration index:
- 00019: public partial class MusterPanel : Control, IBindablePanel
- 00039: public void Bind(MusterHostSession muster, int currentDay)
- 00053: private void OnStateChangedHandler() => RefreshView();
- 00055: public void Open()
- 00172: public void RefreshView()
- 00368: private static void ClearContainer(VBoxContainer container)
- 00374: public void Unbind()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Journal;
00007: using Ashfall.Core.Muster;
00008: using Ashfall.Core.UI;
00009: using CoreTheme = Ashfall.Core.UI.Theme;
00010:
00011: namespace AtomicWar.GodotApp.UI
00012: {
00013:     /// <summary>
00014:     /// ASHFALL — The Muster (Expansion 06) dedicated host panel.
00015:     /// Integrates the 15 Sector Currents, Deserter Coalition Camp, The Unsigned
00016:     /// Order witness dossiers, and sector faction outposts.
00017:     /// Thin presentation only: delegates all mutations to MusterHostSession.
00018:     /// </summary>
00019:     public partial class MusterPanel : Control, IBindablePanel
00020:     {
00021:         public event Action? OnClose;
00022:         public event Action<string, IReadOnlyList<ApproachOption>>? OnApproachModalRequested;
00023:
00024:         private MusterHostSession? _muster;
00025:         private int _currentDay = 1;
00026:
00027:         // ── UI Nodes ──────────────────────────────────────────────────
00028:         private Label _escalationStatus = null!;
00029:         private VBoxContainer _currentsContainer = null!;
00030:         private VBoxContainer _coalitionContainer = null!;
00031:         private VBoxContainer _witnessContainer = null!;
00032:         private VBoxContainer _factionsContainer = null!;
00033:         private Label _lblAuthorBias = null!;
00034:
00035:         public bool IsBound => _muster != null;
00036:
00037:         // ── Bind ──────────────────────────────────────────────────────
00038:
00039:         public void Bind(MusterHostSession muster, int currentDay)
00040:         {
00041:             if (_muster != null)
00042:                 _muster.StateChanged -= OnStateChangedHandler;
00043:
00044:             _muster = muster;
00045:             _currentDay = currentDay;
00046:
00047:             if (_muster != null)
00048:                 _muster.StateChanged += OnStateChangedHandler;
00049:
00050:             RefreshView();
00051:         }
00052:
00053:         private void OnStateChangedHandler() => RefreshView();
00054:
00055:         public void Open()
00056:         {
00057:             Visible = true;
00058:             RefreshView();
00059:         }
00060:
00061:         // ── Godot Lifecycle ───────────────────────────────────────────
00062:
00063:         public override void _Ready()
00064:         {
00065:             SetAnchorsPreset(LayoutPreset.FullRect);
00066:             Visible = false;
00067:
00068:             var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.95f) };
00069:             bg.SetAnchorsPreset(LayoutPreset.FullRect);
00070:             AddChild(bg);
00071:
00072:             var scroll = new ScrollContainer();
00073:             scroll.SetAnchorsPreset(LayoutPreset.FullRect);
00074:             scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
00075:             AddChild(scroll);
00076:
00077:             var center = new CenterContainer();
00078:             center.SetAnchorsPreset(LayoutPreset.FullRect);
00079:             center.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00080:             center.SizeFlagsVertical = SizeFlags.ExpandFill;
00081:             scroll.AddChild(center);
00082:
00083:             var rootBox = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingMd);
00084:             rootBox.CustomMinimumSize = new Vector2(760, 0);
00085:             center.AddChild(rootBox);
00086:
00087:             // ── Header ─────────────────────────────────────────────
00088:             var header = AshfallUiHelpers.MakeTitle("THE MUSTER // SECTOR ESCALATION & CURRENTS", CoreTheme.FontSizeH1);
00089:             header.HorizontalAlignment = HorizontalAlignment.Center;
00090:             rootBox.AddChild(header);
00091:
00092:             var subtitle = AshfallUiHelpers.MakeSmall("Late-stage sector escalation protocols. The currents move. The holding ground waits.");
00093:             subtitle.HorizontalAlignment = HorizontalAlignment.Center;
00094:             subtitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00095:             rootBox.AddChild(subtitle);
00096:
00097:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00098:
00099:             // ── Escalation status bar ──────────────────────────────
00100:             _escalationStatus = AshfallUiHelpers.MakeMono("ESCALATION: —");
00101:             _escalationStatus.HorizontalAlignment = HorizontalAlignment.Center;
00102:             _escalationStatus.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Entropy));
00103:             rootBox.AddChild(_escalationStatus);
00104:
00105:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00106:
00107:             // ── Section 1: Sector Currents ─────────────────────────
00108:             var currentsTitle = AshfallUiHelpers.MakeSectionHeader("SECTOR CURRENTS & FACTION ALIGNMENTS");
00109:             rootBox.AddChild(currentsTitle);
00110:
00111:             _currentsContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00112:             rootBox.AddChild(_currentsContainer);
00113:
00114:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00115:
00116:             // ── Section 2: Deserter Coalition Camp ─────────────────
00117:             var campTitle = AshfallUiHelpers.MakeSectionHeader("DESERTER COALITION // HOLDING GROUND");
00118:             rootBox.AddChild(campTitle);
00119:
00120:             _coalitionContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00121:             rootBox.AddChild(_coalitionContainer);
00122:
00123:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00124:
00125:             // ── Section 3: The Unsigned Order Witness Dossiers ─────
00126:             var witnessTitle = AshfallUiHelpers.MakeSectionHeader("THE UNSIGNED ORDER // WITNESS DOSSIERS");
00127:             rootBox.AddChild(witnessTitle);
00128:
00129:             _witnessContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00130:             rootBox.AddChild(_witnessContainer);
00131:
00132:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00133:
00134:             // ── Section 4: Sector Faction Outposts ─────────────────
00135:             var factionTitle = AshfallUiHelpers.MakeSectionHeader("SECTOR FACTION OUTPOST STATUS");
00136:             rootBox.AddChild(factionTitle);
00137:
00138:             _factionsContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00139:             rootBox.AddChild(_factionsContainer);
00140:
00141:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00142:
00143:             // ── Footer buttons ──────────────────────────────────────
00144:             var btnRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingMd);
00145:             btnRow.Alignment = BoxContainer.AlignmentMode.Center;
00146:
00147:             var btnClose = AshfallUiHelpers.MakeButton("RETURN TO DASHBOARD [Esc]", () => OnClose?.Invoke(), false);
00148:             btnClose.CustomMinimumSize = new Vector2(260, 42);
00149:             btnRow.AddChild(btnClose);
00150:
00151:             rootBox.AddChild(btnRow);
00152:
00153:             var hint = AshfallUiHelpers.MakeSmall("Press [Esc] to return");
00154:             hint.HorizontalAlignment = HorizontalAlignment.Center;
00155:             hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00156:             rootBox.AddChild(hint);
00157:         }
00158:
00159:         public override void _UnhandledInput(InputEvent @event)
00160:         {
00161:             if (!Visible) return;
00162:
00163:             if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00164:             {
00165:                 OnClose?.Invoke();
00166:                 GetViewport().SetInputAsHandled();
00167:             }
00168:         }
00169:
00170:         // ── View Refresh ──────────────────────────────────────────────
00171:
00172:         public void RefreshView()
00173:         {
00174:             if (_currentsContainer == null) return;
00175:
00176:             ClearContainer(_currentsContainer);
00177:             ClearContainer(_coalitionContainer);
00178:             ClearContainer(_witnessContainer);
00179:             ClearContainer(_factionsContainer);
00180:
00181:             if (_muster == null)
00182:             {
00183:                 _escalationStatus.Text = "Panel not bound to active Muster session.";
00184:                 return;
00185:             }
00186:
00187:             int day = _currentDay;
00188:             var engine = _muster.Engine;
00189:
00190:             // ── Escalation Bar ────────────────────────────────────────
00191:             bool open = engine.MusterTriggered;
00192:             string statusText = open
00193:                 ? $"ESCALATION: DAY {day} — THE MUSTER IS OPEN (Holding Ground Active)"
00194:                 : $"ESCALATION: DAY {day} — DORMANT (Muster opens Day {MusterSystem.MusterOpeningDay})";
00195:
00196:             _escalationStatus.Text = statusText;
00197:             _escalationStatus.AddThemeColorOverride("font_color",
00198:                 open
00199:                     ? AshfallUiHelpers.ToColor(CoreTheme.Hot)
00200:                     : AshfallUiHelpers.ToColor(CoreTheme.Warm));
00201:
00202:             // ── Render Currents ───────────────────────────────────────
00203:             var currentsCard = AshfallUiHelpers.MakeCardFrame("SECTOR CURRENTS MATRIX", $"{_muster.Roster.Count} REGISTERED BLOCS");
00204:             var currentsBox = currentsCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00205:
00206:             foreach (var current in _muster.Roster)
00207:             {
00208:                 var row = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
00209:                 string state = current.isActive ? "ACTIVE" : "DORMANT";
00210:                 Color stateColor = current.isActive
00211:                     ? AshfallUiHelpers.ToColor(CoreTheme.Warm)
00212:                     : AshfallUiHelpers.ToColor(CoreTheme.Dim);
00213:
00214:                 var nameLbl = AshfallUiHelpers.MakeMono($"{current.displayName}");
00215:                 nameLbl.CustomMinimumSize = new Vector2(220, 0);
00216:                 nameLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00217:                 row.AddChild(nameLbl);
00218:
00219:                 var statusLbl = AshfallUiHelpers.MakeSmall($"[{state}] · {current.alignment} · Trust {current.trust:0}");
00220:                 statusLbl.AddThemeColorOverride("font_color", stateColor);
00221:                 statusLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00222:                 row.AddChild(statusLbl);
00223:
00224:                 // If this current has a matching questline in the catalog, show approach button
00225:                 var qDef = engine.FindDefinition(current.id);
00226:                 if (qDef != null)
00227:                 {
00228:                     var rec = engine.FindRecord(qDef.questlineId);
00229:                     if (rec != null && rec.resolved)
00230:                     {
00231:                         var resTag = AshfallUiHelpers.MakeSmall($"[RESOLVED: {rec.selectedApproach}]");
00232:                         resTag.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Warm));
00233:                         row.AddChild(resTag);
00234:                     }
00235:                     else if (qDef.approaches != null && qDef.approaches.Count > 0)
00236:                     {
00237:                         string qId = qDef.questlineId;
00238:                         var appList = qDef.approaches;
00239:                         var btnApp = AshfallUiHelpers.MakeButton("APPROACH", () =>
00240:                         {
00241:                             OnApproachModalRequested?.Invoke(qId, appList);
00242:                         });
00243:                         btnApp.CustomMinimumSize = new Vector2(90, 28);
00244:                         row.AddChild(btnApp);
00245:                     }
00246:                 }
00247:
00248:                 currentsBox.AddChild(row);
00249:             }
00250:             _currentsContainer.AddChild(currentsCard);
00251:
00252:             // ── Render Coalition Camp ─────────────────────────────────
00253:             var campCard = AshfallUiHelpers.MakeCardFrame("DESERTER COALITION HOLDING GROUND", "SECTION VI.2 STATUS");
00254:             var campBox = campCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00255:
00256:             var campState = _muster.Camp.State;
00257:             string formedStatus = campState.formed
00258:                 ? $"Formed Day {campState.formedDay} at {campState.holdingGroundId} · Vask {(campState.vaskWithCamp ? "with camp" : "absent")}"
00259:                 : "Holding ground not formed (requires Muster opening at Day 260).";
00260:
00261:             campBox.AddChild(AshfallUiHelpers.MakeDataRow("Holding Ground", formedStatus, AshfallUiHelpers.ToColor(CoreTheme.Warm)));
00262:             campBox.AddChild(AshfallUiHelpers.MakeDataRow("Members Rallied", $"{campState.membersRallied} fighters", AshfallUiHelpers.ToColor(CoreTheme.Hot)));
00263:             campBox.AddChild(AshfallUiHelpers.MakeDataRow("Campaign Strategy", string.IsNullOrEmpty(campState.chosenStrategy) ? "None chosen" : campState.chosenStrategy, AshfallUiHelpers.ToColor(CoreTheme.Pale)));
00264:             campBox.AddChild(AshfallUiHelpers.MakeDataRow("Garrison Lockout Risk", $"{campState.garrisonLockoutRisk}%", AshfallUiHelpers.ToColor(campState.garrisonLockoutRisk > 50 ? CoreTheme.Critical : CoreTheme.Warm)));
00265:
00266:             var campBtnRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
00267:             var btnRally = AshfallUiHelpers.MakeButton("RALLY DESERTER", () =>
00268:             {
00269:                 _muster.RallyDeserter();
00270:                 RefreshView();
00271:             });
00272:             campBtnRow.AddChild(btnRally);
00273:
00274:             if (string.IsNullOrEmpty(campState.chosenStrategy) && campState.formed)
00275:             {
00276:                 var btnStratA = AshfallUiHelpers.MakeButton("STRATEGY: AMNESTY", () =>
00277:                 {
00278:                     _muster.SetStrategy(QuestApproach.A);
00279:                     RefreshView();
00280:                 });
00281:                 var btnStratB = AshfallUiHelpers.MakeButton("STRATEGY: OPEN MUSTER", () =>
00282:                 {
00283:                     _muster.SetStrategy(QuestApproach.B);
00284:                     RefreshView();
00285:                 });
00286:                 campBtnRow.AddChild(btnStratA);
00287:                 campBtnRow.AddChild(btnStratB);
00288:             }
00289:             campBox.AddChild(campBtnRow);
00290:
00291:             _coalitionContainer.AddChild(campCard);
00292:
00293:             // ── Render Witness Dossiers ───────────────────────────────
00294:             var witCard = AshfallUiHelpers.MakeCardFrame("THE UNSIGNED ORDER (3 ACCOUNTS)", $"RECORDED BY {_muster.AuthorBias.ToString().ToUpperInvariant()} AUTHOR");
00295:             var witBox = witCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00296:
00297:             var biasRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
00298:             _lblAuthorBias = AshfallUiHelpers.MakeBody($"Current Recording Bias: {_muster.AuthorBias}");
00299:             _lblAuthorBias.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00300:             _lblAuthorBias.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00301:             biasRow.AddChild(_lblAuthorBias);
00302:
00303:             var btnCycleBias = AshfallUiHelpers.MakeButton("CYCLE AUTHOR BIAS", () =>
00304:             {
00305:                 _muster.CycleAuthorBias();
00306:                 RefreshView();
00307:             });
00308:             btnCycleBias.CustomMinimumSize = new Vector2(180, 32);
00309:             biasRow.AddChild(btnCycleBias);
00310:             witBox.AddChild(biasRow);
00311:             witBox.AddChild(AshfallUiHelpers.MakeSeparator());
00312:
00313:             for (int i = 0; i < _muster.Witnesses.Count; i++)
00314:             {
00315:                 var w = _muster.Witnesses[i];
00316:                 if (day < w.dayMin) continue;
00317:
00318:                 string framing = JournalVoice.ComposeFullText(w.knowledgeKey, _muster.AuthorBias, day);
00319:                 var entryBox = AshfallUiHelpers.MakeVBox(2);
00320:
00321:                 var wHeader = AshfallUiHelpers.MakeMono($"{w.witnessName} — {w.locationId} (Min Day {w.dayMin})");
00322:                 wHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Warm));
00323:                 entryBox.AddChild(wHeader);
00324:
00325:                 var wBody = AshfallUiHelpers.MakeSmall($"{w.body}\n{framing}");
00326:                 wBody.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00327:                 wBody.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
00328:                 entryBox.AddChild(wBody);
00329:
00330:                 witBox.AddChild(entryBox);
00331:                 witBox.AddChild(AshfallUiHelpers.MakeSeparator());
00332:             }
00333:
00334:             _witnessContainer.AddChild(witCard);
00335:
00336:             // ── Render Sector Faction Outposts ────────────────────────
00337:             var facCard = AshfallUiHelpers.MakeCardFrame("SECTOR OUTPOST STATUS", "SUB-SYSTEM INTEGRITY");
00338:             var facBox = facCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00339:
00340:             // Hydro Barons
00341:             var hb = _muster.HydroBarons;
00342:             string hbStatus = hb.PlantSeized ? "PLANT SEIZED" : (hb.AdminReform ? "ADMIN REFORMED" : "EXTRACTION MONOPOLY ACTIVE");
00343:             facBox.AddChild(AshfallUiHelpers.MakeDataRow("Hydro Barons Water Grid", hbStatus, AshfallUiHelpers.ToColor(CoreTheme.Warm)));
00344:
00345:             // Cold Count
00346:             var cc = _muster.ColdCount;
00347:             facBox.AddChild(AshfallUiHelpers.MakeDataRow("Cold Count Heating", $"Power: {cc.PowerSuppliedDays}/{ColdCountState.RequiredPowerDays} days · Shielding: {cc.ShieldingDelivered}/{ColdCountState.RequiredShieldingUnits} units", AshfallUiHelpers.ToColor(CoreTheme.Hot)));
00348:
00349:             // Provisioned
00350:             var ps = _muster.Provisioned;
00351:             facBox.AddChild(AshfallUiHelpers.MakeDataRow("Provisioned Rations", $"Respect: {ps.RespectScore}/{ProvisionedState.ContactThreshold}", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
00352:
00353:             // Iron Raiders
00354:             var ir = _muster.IronRaiders;
00355:             facBox.AddChild(AshfallUiHelpers.MakeDataRow("Iron Raiders Perimeter", $"Aggression: {ir.AggressionLevel:P0} · Raids This Season: {ir.RaidsThisSeason} · Visibility: {ir.State.shelterVisibility:P0}", AshfallUiHelpers.ToColor(CoreTheme.Warm)));
00356:
00357:             // Scavenger Guild
00358:             var sg = _muster.ScavengerGuild;
00359:             facBox.AddChild(AshfallUiHelpers.MakeDataRow("Scavenger Guild", $"Claimed Sites: {sg.State.claimedSiteIds.Count} · Trust: {sg.Trust:F1} · Blacklists: {sg.State.blacklistedShelterIds.Count}", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
00360:
00361:             // Long Walk
00362:             var lw = _muster.LongWalk;
00363:             facBox.AddChild(AshfallUiHelpers.MakeDataRow("Long Walk Waste Traversal", $"Region: {lw.State.currentRegion} · Departure in {lw.State.daysUntilDeparture}d · Crossings: {lw.State.crossingsCompleted}", AshfallUiHelpers.ToColor(CoreTheme.Warm)));
00364:
00365:             _factionsContainer.AddChild(facCard);
00366:         }
00367:
00368:         private static void ClearContainer(VBoxContainer container)
00369:         {
00370:             AshfallUiHelpers.EmptyChildren(container);
00371:         }
00372:
00373:
00374:     public void Unbind()
00375:     {
00376:         if (_muster != null)
00377:             {
00378:                 _muster.StateChanged -= OnStateChangedHandler;
00379:             }
00380:     }
00381:
00382:     public override void _ExitTree()
00383:         {
00384:             Unbind();
00385:             base._ExitTree();
00386:         }
00387:     }
00388: }
```

## `Ashfall.Core.Tests/Narrative/JournalVoiceProseCatalogTests.cs` — 353 lines; 13,390 bytes; SHA-256 `ba911d0f0f23466991b6e75589e4ee51fc5f6b73a45641d61db68ff43eec4773`
Declaration index:
- 00015: public class JournalVoiceProseCatalogTests
- 00017: private static string DataDir()
- 00027: private static JournalVoiceProseCatalog LoadCatalog()
- 00033: private static void EnsureCatalogBound()
- 00040: public void JournalVoiceProseJsonHasSchemaVersion()
- 00050: public void JournalVoiceProseJsonLoadsWithoutErrors()
- 00058: public void AllKnowledgeKeysHaveProseEntries()
- 00070: public void AllKnowledgeKeysHaveDefaultVariant()
- 00084: public void AllCoreBiasTraitsHaveVariants()
- 00106: public void AllExpansionBiasTraitsHaveVariantsWhereApplicable()
- 00138: public void GetProseReturnsCorrectVariant()
- 00148: public void GetProseFallsBackToDefault()
- 00164: public void ComposeFullTextFormatsCorrectly()
- 00173: public void FormatTimestampWorksCorrectly()
- 00189: public void AllProseTextIsNonEmpty()
- 00212: public void CatalogCountMatchesExpected()
- 00220: public void UnknownKnowledgeKeyReturnsFallback()
- 00229: public void PinHighCo2ParanoidOutput()
- 00239: public void PinHighCo2CautiousOutput()
- 00249: public void PinHighCo2RealistOutput()
- 00259: public void PinSeenRadiationAllBiasTraits()
- 00281: public void PinExperiencedStormAllBiasTraits()
- 00303: public void PinExpansionProseForEmpathAndSociopath()
- 00318: public void ComposeFullTextPreservesDayPrefix()
- 00328: public void CatalogBindIsIdempotent()
- 00341: public void AllKnowledgeKeysAreLowercaseSnakeCase()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Xunit;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Journal;
00008:
00009: namespace Ashfall.Core.Tests.Narrative
00010: {
00011:     /// <summary>
00012:     /// Canonical-ID, reachability, and load-equivalence tests for journal_voice_prose.json.
00013:     /// Validates that all prose definitions meet schema requirements and can be loaded correctly.
00014:     /// </summary>
00015:     public class JournalVoiceProseCatalogTests
00016:     {
00017:         private static string DataDir()
00018:         {
00019:             string start = Directory.GetCurrentDirectory();
00020:             if (CatalogLocator.TryFindDataDirectory(start, out string found))
00021:                 return found;
00022:             if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
00023:                 return found;
00024:             throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
00025:         }
00026:
00027:         private static JournalVoiceProseCatalog LoadCatalog()
00028:         {
00029:             var loader = new JournalVoiceProseCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
00030:             return loader.Load(DataDir());
00031:         }
00032:
00033:         private static void EnsureCatalogBound()
00034:         {
00035:             var catalog = LoadCatalog();
00036:             JournalVoice.BindCatalog(catalog);
00037:         }
00038:
00039:         [Fact]
00040:         public void JournalVoiceProseJsonHasSchemaVersion()
00041:         {
00042:             string jsonPath = Path.Combine(DataDir(), JournalVoiceProseCatalogLoader.ProseFile);
00043:             Assert.True(File.Exists(jsonPath), "journal_voice_prose.json must exist");
00044:
00045:             string json = File.ReadAllText(jsonPath);
00046:             Assert.Contains("\"schema_version\"", json);
00047:         }
00048:
00049:         [Fact]
00050:         public void JournalVoiceProseJsonLoadsWithoutErrors()
00051:         {
00052:             var catalog = LoadCatalog();
00053:             Assert.NotNull(catalog);
00054:             Assert.True(catalog.Count > 0, "Catalog should contain prose entries");
00055:         }
00056:
00057:         [Fact]
00058:         public void AllKnowledgeKeysHaveProseEntries()
00059:         {
00060:             var catalog = LoadCatalog();
00061:
00062:             foreach (string key in KnowledgeKeys.All)
00063:             {
00064:                 Assert.True(catalog.HasKey(key),
00065:                     $"Knowledge key '{key}' must have prose entry in catalog");
00066:             }
00067:         }
00068:
00069:         [Fact]
00070:         public void AllKnowledgeKeysHaveDefaultVariant()
00071:         {
00072:             var catalog = LoadCatalog();
00073:
00074:             foreach (string key in KnowledgeKeys.All)
00075:             {
00076:                 var entry = catalog.GetEntry(key);
00077:                 Assert.NotNull(entry);
00078:                 Assert.NotNull(entry.@default);
00079:                 Assert.NotEmpty(entry.@default);
00080:             }
00081:         }
00082:
00083:         [Fact]
00084:         public void AllCoreBiasTraitsHaveVariants()
00085:         {
00086:             var catalog = LoadCatalog();
00087:             var coreTraits = new[] {
00088:                 RiskBiasTrait.Paranoid, RiskBiasTrait.Cautious, RiskBiasTrait.Realist,
00089:                 RiskBiasTrait.Reckless, RiskBiasTrait.Denialist, RiskBiasTrait.Fatalist
00090:             };
00091:
00092:             foreach (string key in KnowledgeKeys.All)
00093:             {
00094:                 var entry = catalog.GetEntry(key);
00095:                 Assert.NotNull(entry);
00096:
00097:                 foreach (var trait in coreTraits)
00098:                 {
00099:                     Assert.True(entry.HasVariantForBias(trait),
00100:                         $"Key '{key}' missing variant for bias '{trait}'");
00101:                 }
00102:             }
00103:         }
00104:
00105:         [Fact]
00106:         public void AllExpansionBiasTraitsHaveVariantsWhereApplicable()
00107:         {
00108:             var catalog = LoadCatalog();
00109:             var expansionTraits = new[] { RiskBiasTrait.Empath, RiskBiasTrait.Sociopath };
00110:
00111:             // Expansion 06 knowledge keys should have empath/sociopath variants
00112:             var expansionKeys = new[] {
00113:                 KnowledgeKeys.ContinuityReclamationDecree,
00114:                 KnowledgeKeys.HydroBaronRateCardOrigin,
00115:                 KnowledgeKeys.DeserterCoalitionFounding,
00116:                 KnowledgeKeys.ColdCountBeforeTheLab,
00117:                 KnowledgeKeys.ProvisionedAdvanceKnowledge,
00118:                 KnowledgeKeys.CheckpointConscriptsConfession,
00119:                 KnowledgeKeys.QuartermastersPaperwork,
00120:                 KnowledgeKeys.InterceptedCipher,
00121:                 KnowledgeKeys.LedgerNobodySigned
00122:             };
00123:
00124:             foreach (string key in expansionKeys)
00125:             {
00126:                 var entry = catalog.GetEntry(key);
00127:                 Assert.NotNull(entry);
00128:
00129:                 foreach (var trait in expansionTraits)
00130:                 {
00131:                     Assert.True(entry.HasVariantForBias(trait),
00132:                         $"Expansion key '{key}' missing variant for bias '{trait}'");
00133:                 }
00134:             }
00135:         }
00136:
00137:         [Fact]
00138:         public void GetProseReturnsCorrectVariant()
00139:         {
00140:             EnsureCatalogBound();
00141:
00142:             string prose = JournalVoice.ComposeBody(KnowledgeKeys.HighCo2, RiskBiasTrait.Paranoid);
00143:             Assert.NotEmpty(prose);
00144:             Assert.Contains("poison", prose);
00145:         }
00146:
00147:         [Fact]
00148:         public void GetProseFallsBackToDefault()
00149:         {
00150:             EnsureCatalogBound();
00151:
00152:             // ComposeBody should never return empty
00153:             foreach (string key in KnowledgeKeys.All)
00154:             {
00155:                 foreach (RiskBiasTrait trait in Enum.GetValues(typeof(RiskBiasTrait)))
00156:                 {
00157:                     string prose = JournalVoice.ComposeBody(key, trait);
00158:                     Assert.NotEmpty(prose);
00159:                 }
00160:             }
00161:         }
00162:
00163:         [Fact]
00164:         public void ComposeFullTextFormatsCorrectly()
00165:         {
00166:             EnsureCatalogBound();
00167:
00168:             string fullText = JournalVoice.ComposeFullText(KnowledgeKeys.HighCo2, RiskBiasTrait.Realist, 45);
00169:             Assert.StartsWith("Day 45.", fullText);
00170:         }
00171:
00172:         [Fact]
00173:         public void FormatTimestampWorksCorrectly()
00174:         {
00175:             string ts1 = JournalVoice.FormatTimestamp(90);
00176:             Assert.Equal("Day 90", ts1);
00177:
00178:             string ts2 = JournalVoice.FormatTimestamp(90, 14.5f);
00179:             Assert.Equal("Day 90, 14h", ts2);
00180:
00181:             string ts3 = JournalVoice.FormatTimestamp(0);
00182:             Assert.Equal("Day 1", ts3);
00183:
00184:             string ts4 = JournalVoice.FormatTimestamp(5, 25);
00185:             Assert.Equal("Day 5, 01h", ts4);
00186:         }
00187:
00188:         [Fact]
00189:         public void AllProseTextIsNonEmpty()
00190:         {
00191:             var catalog = LoadCatalog();
00192:
00193:             foreach (string key in catalog.GetAllKeys())
00194:             {
00195:                 var entry = catalog.GetEntry(key);
00196:                 Assert.NotNull(entry);
00197:
00198:                 // Check all variants
00199:                 if (!string.IsNullOrEmpty(entry.paranoid)) Assert.NotEmpty(entry.paranoid);
00200:                 if (!string.IsNullOrEmpty(entry.cautious)) Assert.NotEmpty(entry.cautious);
00201:                 if (!string.IsNullOrEmpty(entry.realist)) Assert.NotEmpty(entry.realist);
00202:                 if (!string.IsNullOrEmpty(entry.reckless)) Assert.NotEmpty(entry.reckless);
00203:                 if (!string.IsNullOrEmpty(entry.denialist)) Assert.NotEmpty(entry.denialist);
00204:                 if (!string.IsNullOrEmpty(entry.fatalist)) Assert.NotEmpty(entry.fatalist);
00205:                 if (!string.IsNullOrEmpty(entry.empath)) Assert.NotEmpty(entry.empath);
00206:                 if (!string.IsNullOrEmpty(entry.sociopath)) Assert.NotEmpty(entry.sociopath);
00207:                 Assert.NotEmpty(entry.@default);
00208:             }
00209:         }
00210:
00211:         [Fact]
00212:         public void CatalogCountMatchesExpected()
00213:         {
00214:             var catalog = LoadCatalog();
00215:             Assert.True(catalog.Count >= KnowledgeKeys.All.Length,
00216:                 $"Catalog should have at least {KnowledgeKeys.All.Length} entries, has {catalog.Count}");
00217:         }
00218:
00219:         [Fact]
00220:         public void UnknownKnowledgeKeyReturnsFallback()
00221:         {
00222:             EnsureCatalogBound();
00223:
00224:             string prose = JournalVoice.ComposeBody("nonexistent_key", RiskBiasTrait.Realist);
00225:             Assert.Equal("Something changed. I wrote it down so I would not forget.", prose);
00226:         }
00227:
00228:         [Fact]
00229:         public void PinHighCo2ParanoidOutput()
00230:         {
00231:             EnsureCatalogBound();
00232:
00233:             string expected = "The air is poison. Thick. My skull is a vice. We crack the vents or we choke — ash or no ash.";
00234:             string actual = JournalVoice.ComposeBody(KnowledgeKeys.HighCo2, RiskBiasTrait.Paranoid);
00235:             Assert.Equal(expected, actual);
00236:         }
00237:
00238:         [Fact]
00239:         public void PinHighCo2CautiousOutput()
00240:         {
00241:             EnsureCatalogBound();
00242:
00243:             string expected = "My head is pounding. The air feels thick. We need to open the vents, even if the ash gets in.";
00244:             string actual = JournalVoice.ComposeBody(KnowledgeKeys.HighCo2, RiskBiasTrait.Cautious);
00245:             Assert.Equal(expected, actual);
00246:         }
00247:
00248:         [Fact]
00249:         public void PinHighCo2RealistOutput()
00250:         {
00251:             EnsureCatalogBound();
00252:
00253:             string expected = "CO₂ is climbing — headache, heavy air. Crack a vent or the filter is finished. Ash comes with it.";
00254:             string actual = JournalVoice.ComposeBody(KnowledgeKeys.HighCo2, RiskBiasTrait.Realist);
00255:             Assert.Equal(expected, actual);
00256:         }
00257:
00258:         [Fact]
00259:         public void PinSeenRadiationAllBiasTraits()
00260:         {
00261:             EnsureCatalogBound();
00262:
00263:             var expected = new Dictionary<RiskBiasTrait, string>
00264:             {
00265:                 [RiskBiasTrait.Paranoid] = "The dosimeter twitched. Or I imagined it. Either way I will not take my coat off indoors.",
00266:                 [RiskBiasTrait.Cautious] = "I felt the dose climb. Not much — enough. We log it, we scrub, we do not pretend it is nothing.",
00267:                 [RiskBiasTrait.Realist] = "Radiation is on us now. Small number, real number. Keep the suits sealed when we go out.",
00268:                 [RiskBiasTrait.Reckless] = "Got a tick on the counter. Still standing. Wash the boots and keep moving.",
00269:                 [RiskBiasTrait.Denialist] = "The needle moved. Instruments lie. I feel fine.",
00270:                 [RiskBiasTrait.Fatalist] = "The dose goes up. It always does. Write it down so the next one knows."
00271:             };
00272:
00273:             foreach (var kvp in expected)
00274:             {
00275:                 string actual = JournalVoice.ComposeBody(KnowledgeKeys.HasSeenRadiation, kvp.Key);
00276:                 Assert.Equal(kvp.Value, actual);
00277:             }
00278:         }
00279:
00280:         [Fact]
00281:         public void PinExperiencedStormAllBiasTraits()
00282:         {
00283:             EnsureCatalogBound();
00284:
00285:             var expected = new Dictionary<RiskBiasTrait, string>
00286:             {
00287:                 [RiskBiasTrait.Paranoid] = "The sky is eating the world. Fallout on the roof. Do not open anything. Not the hatch. Not a crack.",
00288:                 [RiskBiasTrait.Cautious] = "Storm hit. Ash and worse. Seal the intake if we can. No trips until it breaks.",
00289:                 [RiskBiasTrait.Realist] = "Fallout storm. Outdoor exposure spikes. Stay under concrete until the wind dies.",
00290:                 [RiskBiasTrait.Reckless] = "Ugly sky. Storm. If someone has to go out, make it short and make them count.",
00291:                 [RiskBiasTrait.Denialist] = "Weather's loud. It will pass. Always does.",
00292:                 [RiskBiasTrait.Fatalist] = "Storm again. The ash settles on everything. We wait. That is the work."
00293:             };
00294:
00295:             foreach (var kvp in expected)
00296:             {
00297:                 string actual = JournalVoice.ComposeBody(KnowledgeKeys.HasExperiencedStorm, kvp.Key);
00298:                 Assert.Equal(kvp.Value, actual);
00299:             }
00300:         }
00301:
00302:         [Fact]
00303:         public void PinExpansionProseForEmpathAndSociopath()
00304:         {
00305:             EnsureCatalogBound();
00306:
00307:             // Check expansion 06 prose has empath/sociopath variants
00308:             string empathProse = JournalVoice.ComposeBody(
00309:                 KnowledgeKeys.CheckpointConscriptsConfession, RiskBiasTrait.Empath);
00310:             Assert.Contains("confession", empathProse);
00311:
00312:             string sociopathProse = JournalVoice.ComposeBody(
00313:                 KnowledgeKeys.CheckpointConscriptsConfession, RiskBiasTrait.Sociopath);
00314:             Assert.Contains("Source", sociopathProse);
00315:         }
00316:
00317:         [Fact]
00318:         public void ComposeFullTextPreservesDayPrefix()
00319:         {
00320:             EnsureCatalogBound();
00321:
00322:             // Body already starts with "Day " should not get prefixed
00323:             string fullText = JournalVoice.ComposeFullText(KnowledgeKeys.HighCo2, RiskBiasTrait.Realist, 90);
00324:             Assert.StartsWith("Day 90. CO", fullText);
00325:         }
00326:
00327:         [Fact]
00328:         public void CatalogBindIsIdempotent()
00329:         {
00330:             var catalog1 = LoadCatalog();
00331:             var catalog2 = LoadCatalog();
00332:
00333:             JournalVoice.BindCatalog(catalog1);
00334:             Assert.Same(catalog1, JournalVoice.GetCatalog());
00335:
00336:             JournalVoice.BindCatalog(catalog2);
00337:             Assert.Same(catalog2, JournalVoice.GetCatalog());
00338:         }
00339:
00340:         [Fact]
00341:         public void AllKnowledgeKeysAreLowercaseSnakeCase()
00342:         {
00343:             var catalog = LoadCatalog();
00344:
00345:             foreach (string key in catalog.GetAllKeys())
00346:             {
00347:                 Assert.Equal(key.ToLowerInvariant(), key);
00348:                 Assert.DoesNotContain(" ", key);
00349:                 Assert.DoesNotContain("-", key);
00350:             }
00351:         }
00352:     }
00353: }
```

## `Ashfall.Core.Tests/Narrative/JournalVoiceProseExpansionTests.cs` — 169 lines; 6,357 bytes; SHA-256 `efc8fb9569960c5b92551d121d5a5ea7685ad208093df2b259c28b1c228bfdbd`
Declaration index:
- 00014: public class JournalVoiceProseExpansionTests
- 00042: private static string DataDir()
- 00052: private static JournalVoiceProseCatalog LoadCatalog()
- 00059: public void All12Plan95SituationKeysExistInCatalog()
- 00071: public void AllPlan95KeysHaveDefaultAndCoreBiasVariants()
- 00091: public void AllPlan95VariantsWithinEachKeyAreDistinct()
- 00115: public void AllPlan95SituationKeysAreStrictSnakeCase()
- 00126: public void JournalVoiceComposeBodyProducesVariantsForPlan95Keys()
- 00149: public void JournalVoiceComposeFullTextFormatsCorrectly()
- 00160: public void JournalVoiceFallsBackGracefullyOnUnknownKey()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Xunit;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Journal;
00008:
00009: namespace Ashfall.Core.Tests.Narrative
00010: {
00011:     /// <summary>
00012:     /// Tests for Plan 95: Journal Voice Prose Expansion (12 situation keys × 7 personality variants = 84 variants).
00013:     /// </summary>
00014:     public class JournalVoiceProseExpansionTests
00015:     {
00016:         public static readonly string[] Plan95SituationKeys =
00017:         {
00018:             "low_food",
00019:             "low_water",
00020:             "death_of_survivor",
00021:             "successful_expedition",
00022:             "failed_expedition",
00023:             "faction_raid",
00024:             "disease_outbreak",
00025:             "power_failure",
00026:             "new_survivor_arrived",
00027:             "severe_cold",
00028:             "high_radiation_zone",
00029:             "moral_compromise"
00030:         };
00031:
00032:         private static readonly RiskBiasTrait[] CoreTraits =
00033:         {
00034:             RiskBiasTrait.Paranoid,
00035:             RiskBiasTrait.Cautious,
00036:             RiskBiasTrait.Realist,
00037:             RiskBiasTrait.Reckless,
00038:             RiskBiasTrait.Denialist,
00039:             RiskBiasTrait.Fatalist
00040:         };
00041:
00042:         private static string DataDir()
00043:         {
00044:             string start = Directory.GetCurrentDirectory();
00045:             if (CatalogLocator.TryFindDataDirectory(start, out string found))
00046:                 return found;
00047:             if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
00048:                 return found;
00049:             throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
00050:         }
00051:
00052:         private static JournalVoiceProseCatalog LoadCatalog()
00053:         {
00054:             var loader = new JournalVoiceProseCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
00055:             return loader.Load(DataDir());
00056:         }
00057:
00058:         [Fact]
00059:         public void All12Plan95SituationKeysExistInCatalog()
00060:         {
00061:             var catalog = LoadCatalog();
00062:             Assert.True(catalog.Count >= 33, $"Catalog should have at least 33 entries, has {catalog.Count}");
00063:
00064:             foreach (string key in Plan95SituationKeys)
00065:             {
00066:                 Assert.True(catalog.HasKey(key), $"Catalog must contain expanded situation key '{key}'");
00067:             }
00068:         }
00069:
00070:         [Fact]
00071:         public void AllPlan95KeysHaveDefaultAndCoreBiasVariants()
00072:         {
00073:             var catalog = LoadCatalog();
00074:
00075:             foreach (string key in Plan95SituationKeys)
00076:             {
00077:                 var entry = catalog.GetEntry(key);
00078:                 Assert.NotNull(entry);
00079:                 Assert.False(string.IsNullOrWhiteSpace(entry.@default), $"Key '{key}' missing non-empty 'default' variant");
00080:
00081:                 foreach (var trait in CoreTraits)
00082:                 {
00083:                     Assert.True(entry.HasVariantForBias(trait), $"Key '{key}' missing variant for bias '{trait}'");
00084:                     string variantText = entry.GetProseForBias(trait);
00085:                     Assert.False(string.IsNullOrWhiteSpace(variantText), $"Key '{key}' variant for '{trait}' must not be whitespace");
00086:                 }
00087:             }
00088:         }
00089:
00090:         [Fact]
00091:         public void AllPlan95VariantsWithinEachKeyAreDistinct()
00092:         {
00093:             var catalog = LoadCatalog();
00094:
00095:             foreach (string key in Plan95SituationKeys)
00096:             {
00097:                 var entry = catalog.GetEntry(key);
00098:                 Assert.NotNull(entry);
00099:
00100:                 var seenVariants = new HashSet<string>(StringComparer.Ordinal);
00101:
00102:                 Assert.True(seenVariants.Add(entry.@default), $"Duplicate default variant found in '{key}'");
00103:                 Assert.True(seenVariants.Add(entry.paranoid), $"Duplicate paranoid variant found in '{key}'");
00104:                 Assert.True(seenVariants.Add(entry.cautious), $"Duplicate cautious variant found in '{key}'");
00105:                 Assert.True(seenVariants.Add(entry.realist), $"Duplicate realist variant found in '{key}'");
00106:                 Assert.True(seenVariants.Add(entry.reckless), $"Duplicate reckless variant found in '{key}'");
00107:                 Assert.True(seenVariants.Add(entry.denialist), $"Duplicate denialist variant found in '{key}'");
00108:                 Assert.True(seenVariants.Add(entry.fatalist), $"Duplicate fatalist variant found in '{key}'");
00109:
00110:                 Assert.Equal(7, seenVariants.Count);
00111:             }
00112:         }
00113:
00114:         [Fact]
00115:         public void AllPlan95SituationKeysAreStrictSnakeCase()
00116:         {
00117:             foreach (string key in Plan95SituationKeys)
00118:             {
00119:                 Assert.Equal(key.ToLowerInvariant(), key);
00120:                 Assert.DoesNotContain(" ", key);
00121:                 Assert.DoesNotContain("-", key);
00122:             }
00123:         }
00124:
00125:         [Fact]
00126:         public void JournalVoiceComposeBodyProducesVariantsForPlan95Keys()
00127:         {
00128:             var catalog = LoadCatalog();
00129:             JournalVoice.BindCatalog(catalog);
00130:
00131:             foreach (string key in Plan95SituationKeys)
00132:             {
00133:                 var entry = catalog.GetEntry(key);
00134:                 Assert.NotNull(entry);
00135:
00136:                 foreach (var trait in CoreTraits)
00137:                 {
00138:                     string actual = JournalVoice.ComposeBody(key, trait);
00139:                     string expected = entry.GetProseForBias(trait);
00140:                     Assert.Equal(expected, actual);
00141:                 }
00142:
00143:                 string actualDefault = JournalVoice.ComposeBody(key, (RiskBiasTrait)999);
00144:                 Assert.Equal(entry.@default, actualDefault);
00145:             }
00146:         }
00147:
00148:         [Fact]
00149:         public void JournalVoiceComposeFullTextFormatsCorrectly()
00150:         {
00151:             var catalog = LoadCatalog();
00152:             JournalVoice.BindCatalog(catalog);
00153:
00154:             string full = JournalVoice.ComposeFullText("low_food", RiskBiasTrait.Realist, 42);
00155:             Assert.StartsWith("Day 42. ", full);
00156:             Assert.Contains("There is less food than the shelter needs.", full);
00157:         }
00158:
00159:         [Fact]
00160:         public void JournalVoiceFallsBackGracefullyOnUnknownKey()
00161:         {
00162:             var catalog = LoadCatalog();
00163:             JournalVoice.BindCatalog(catalog);
00164:
00165:             string fallback = JournalVoice.ComposeBody("nonexistent_unknown_situation_key", RiskBiasTrait.Paranoid);
00166:             Assert.Equal("Something changed. I wrote it down so I would not forget.", fallback);
00167:         }
00168:     }
00169: }
```

## `Ashfall.Core.Tests/Narrative/Plan95_103JournalTreatyIntegrationTests.cs` — 182 lines; 8,874 bytes; SHA-256 `f15c60bd77cc2e1b4ebdd4704c163b33c0d4db813bfcab6f0eebc99a265b228b`
Declaration index:
- 00019: public sealed class Plan95_103JournalTreatyIntegrationTests : CatalogTestBase
- 00048: public void Plan95_JournalVoiceProseCatalog_LoadsAllSituationKeys_WithDistinctBiasVariants()
- 00078: public void Plan103_FoundryTreatyConsequences_LoadsAllFifteenPolicies_WithValidSignatoriesAndModifiers()
- 00118: public void CrossSystem_TreatyConsequenceOutcomesAndJournalVoices_ExhibitSystemicResonance()
- 00164: public void CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Foundry;
00008: using Ashfall.Core.Journal;
00009: using Ashfall.Core.Narrative;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.Narrative
00013: {
00014:     /// <summary>
00015:     /// Wave 40 Batch 7 Cross-System Integration Test:
00016:     /// Validates Plan 95 (Journal Voice Prose & Narrative Tone - 33+ situation keys with 7 personality biases)
00017:     /// alongside Plan 103 (Foundry Treaty Consequences Expansion - 15 consequence policies with market modifiers).
00018:     /// </summary>
00019:     public sealed class Plan95_103JournalTreatyIntegrationTests : CatalogTestBase
00020:     {
00021:         private static readonly string[] Plan95SituationKeys =
00022:         {
00023:             "low_food",
00024:             "low_water",
00025:             "death_of_survivor",
00026:             "successful_expedition",
00027:             "failed_expedition",
00028:             "faction_raid",
00029:             "disease_outbreak",
00030:             "power_failure",
00031:             "new_survivor_arrived",
00032:             "severe_cold",
00033:             "high_radiation_zone",
00034:             "moral_compromise"
00035:         };
00036:
00037:         private static readonly RiskBiasTrait[] CoreTraits =
00038:         {
00039:             RiskBiasTrait.Paranoid,
00040:             RiskBiasTrait.Cautious,
00041:             RiskBiasTrait.Realist,
00042:             RiskBiasTrait.Reckless,
00043:             RiskBiasTrait.Denialist,
00044:             RiskBiasTrait.Fatalist
00045:         };
00046:
00047:         [Fact]
00048:         public void Plan95_JournalVoiceProseCatalog_LoadsAllSituationKeys_WithDistinctBiasVariants()
00049:         {
00050:             var files = new FileSystemIO();
00051:             var json = new SystemTextJsonSerializer();
00052:             var loader = new JournalVoiceProseCatalogLoader(files, json);
00053:             var catalog = loader.Load(DataDirectory);
00054:
00055:             Assert.NotNull(catalog);
00056:             Assert.True(catalog.Count >= 33, $"Expected at least 33 situation keys, got {catalog.Count}");
00057:
00058:             foreach (var key in Plan95SituationKeys)
00059:             {
00060:                 Assert.True(catalog.HasKey(key), $"Catalog must contain situation key '{key}'");
00061:                 var entry = catalog.GetEntry(key);
00062:                 Assert.NotNull(entry);
00063:                 Assert.False(string.IsNullOrWhiteSpace(entry.@default), $"Key '{key}' missing default variant");
00064:
00065:                 var distinctVariants = new HashSet<string>(StringComparer.Ordinal) { entry.@default };
00066:
00067:                 foreach (var trait in CoreTraits)
00068:                 {
00069:                     Assert.True(entry.HasVariantForBias(trait), $"Key '{key}' missing variant for {trait}");
00070:                     string text = entry.GetProseForBias(trait);
00071:                     Assert.False(string.IsNullOrWhiteSpace(text), $"Key '{key}' text empty for {trait}");
00072:                     Assert.True(distinctVariants.Add(text), $"Duplicate variant detected for key '{key}' under trait {trait}");
00073:                 }
00074:             }
00075:         }
00076:
00077:         [Fact]
00078:         public void Plan103_FoundryTreatyConsequences_LoadsAllFifteenPolicies_WithValidSignatoriesAndModifiers()
00079:         {
00080:             var files = new FileSystemIO();
00081:             var json = new SystemTextJsonSerializer();
00082:
00083:             var rawConsequences = SilentFoundryConsequenceCatalogLoader.Load(DataDirectory, files, json);
00084:             Assert.NotNull(rawConsequences);
00085:
00086:             var catalog = new SilentFoundryConsequencePolicyCatalog();
00087:             catalog.Load(rawConsequences);
00088:
00089:             Assert.False(catalog.HasErrors, $"Catalog reported errors: {string.Join("; ", catalog.Errors)}");
00090:             Assert.Equal(15, catalog.PolicyCount);
00091:             Assert.Equal(15, catalog.AllPolicies.Count);
00092:
00093:             string accordsRaw = files.ReadAllText(Path.Combine(DataDirectory, SilentFoundryCatalogLoader.AccordsFileName));
00094:             var accords = json.Deserialize<RegionalTreatiesFile>(accordsRaw)!;
00095:             var treatyIds = new HashSet<string>(accords.treaties.Select(t => t.treaty_id), StringComparer.Ordinal);
00096:
00097:             var validOutcomes = new HashSet<string>(StringComparer.Ordinal) { "met", "missed", "violated" };
00098:
00099:             foreach (var policy in catalog.AllPolicies)
00100:             {
00101:                 Assert.True(treatyIds.Contains(policy.treaty_id), $"Treaty id '{policy.treaty_id}' must exist in foundry_accords.json");
00102:                 Assert.Contains(policy.outcome, validOutcomes);
00103:                 Assert.False(string.IsNullOrWhiteSpace(policy.faction_id), "Faction id must not be empty");
00104:                 Assert.False(string.IsNullOrWhiteSpace(policy.reason), "Reason must not be empty");
00105:                 Assert.InRange(policy.standing_delta, -50f, 30f);
00106:
00107:                 Assert.NotNull(policy.market_modifiers);
00108:                 foreach (var mod in policy.market_modifiers)
00109:                 {
00110:                     Assert.False(string.IsNullOrWhiteSpace(mod.good_id), "Good id must not be empty");
00111:                     Assert.NotEqual(0f, mod.demand_delta);
00112:                     Assert.False(string.IsNullOrWhiteSpace(mod.reason), "Modifier reason must not be empty");
00113:                 }
00114:             }
00115:         }
00116:
00117:         [Fact]
00118:         public void CrossSystem_TreatyConsequenceOutcomesAndJournalVoices_ExhibitSystemicResonance()
00119:         {
00120:             var files = new FileSystemIO();
00121:             var json = new SystemTextJsonSerializer();
00122:
00123:             var journalLoader = new JournalVoiceProseCatalogLoader(files, json);
00124:             var journalCatalog = journalLoader.Load(DataDirectory);
00125:
00126:             var rawConsequences = SilentFoundryConsequenceCatalogLoader.Load(DataDirectory, files, json);
00127:             var consequenceCatalog = new SilentFoundryConsequencePolicyCatalog();
00128:             consequenceCatalog.Load(rawConsequences);
00129:
00130:             // 1. Power failure resonance: treaty_coal_window missed/violated policies result in fuel shortage and production drops,
00131:             // directly resonating with the 'power_failure' journal voice prose.
00132:             var coalPolicies = consequenceCatalog.AllPolicies.Where(p => p.treaty_id == "treaty_coal_window").ToList();
00133:             Assert.NotEmpty(coalPolicies);
00134:             var coalViolated = consequenceCatalog.Find("treaty_coal_window", FoundryTreatyOutcome.Violated)
00135:                                ?? consequenceCatalog.Find("treaty_coal_window", FoundryTreatyOutcome.Missed);
00136:             Assert.NotNull(coalViolated);
00137:             Assert.True(coalViolated.standing_delta < 0, "Violated coal window must penalize standing");
00138:
00139:             var powerJournal = journalCatalog.GetEntry("power_failure");
00140:             Assert.NotNull(powerJournal);
00141:             Assert.Contains("refrigeration", powerJournal.GetProseForBias(RiskBiasTrait.Realist).ToLowerInvariant());
00142:             Assert.Contains("breakers", powerJournal.GetProseForBias(RiskBiasTrait.Paranoid).ToLowerInvariant());
00143:
00144:             // 2. Low water resonance: treaty_saltworks_access violated policies restrict water and penalize trade,
00145:             // matching the 'low_water' journal voice prose.
00146:             var saltViolated = consequenceCatalog.Find("treaty_saltworks_access", FoundryTreatyOutcome.Violated);
00147:             Assert.NotNull(saltViolated);
00148:
00149:             var waterJournal = journalCatalog.GetEntry("low_water");
00150:             Assert.NotNull(waterJournal);
00151:             Assert.False(string.IsNullOrWhiteSpace(waterJournal.GetProseForBias(RiskBiasTrait.Fatalist)));
00152:             Assert.False(string.IsNullOrWhiteSpace(waterJournal.GetProseForBias(RiskBiasTrait.Cautious)));
00153:
00154:             // 3. Moral compromise resonance: mutual aid pacts failing during emergencies mirror the 'moral_compromise' voice.
00155:             var aidViolated = consequenceCatalog.Find("treaty_crisis_mutual_aid", FoundryTreatyOutcome.Violated);
00156:             Assert.NotNull(aidViolated);
00157:             var moralJournal = journalCatalog.GetEntry("moral_compromise");
00158:             Assert.NotNull(moralJournal);
00159:             Assert.False(string.IsNullOrWhiteSpace(moralJournal.@default));
00160:             Assert.False(string.IsNullOrWhiteSpace(moralJournal.GetProseForBias(RiskBiasTrait.Denialist)));
00161:         }
00162:
00163:         [Fact]
00164:         public void CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses()
00165:         {
00166:             var files = new FileSystemIO();
00167:             var json = new SystemTextJsonSerializer();
00168:
00169:             for (int i = 0; i < 50; i++)
00170:             {
00171:                 var journalLoader = new JournalVoiceProseCatalogLoader(files, json);
00172:                 var journalCatalog = journalLoader.Load(DataDirectory);
00173:                 Assert.True(journalCatalog.Count >= 33);
00174:
00175:                 var rawConsequences = SilentFoundryConsequenceCatalogLoader.Load(DataDirectory, files, json);
00176:                 var consequenceCatalog = new SilentFoundryConsequencePolicyCatalog();
00177:                 consequenceCatalog.Load(rawConsequences);
00178:                 Assert.Equal(15, consequenceCatalog.PolicyCount);
00179:             }
00180:         }
00181:     }
00182: }
```

## `Ashfall.Core.Tests/JournalProducerIntegrationTests.cs` — 296 lines; 12,942 bytes; SHA-256 `66dd88993fedcf329e7fab1058adf095357abdefd73081fdac8634e22c10b878`
Declaration index:
- 00013: public class JournalProducerIntegrationTests
- 00015: private sealed class TestSurvivorAuthor : ISurvivorAuthor
- 00030: public void AutopsyProducer_WritesJournalEntry_AndEnforcesDedupOnRepeat()
- 00093: public void LibraryStudyProducer_GrantsKnowledgeUnlocks_ViaAddKnowledgeEvidence()
- 00133: public void MoralChoiceProducer_WritesJournalEntry_OnQuestResolved()
- 00182: public void ProceduralEulogyEngine_ComposesFullMemorial_AndSupportsLosslessSaveRestore()
- 00242: public void JournalVoice_ToneShiftsWithRiskBias_AndFormatsCleanly()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core;
00005: using Ashfall.Core.Flags;
00006: using Ashfall.Core.Journal;
00007: using Ashfall.Core.MoralChoice;
00008: using Ashfall.Core.Survivors;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests
00012: {
00013:     public class JournalProducerIntegrationTests
00014:     {
00015:         private sealed class TestSurvivorAuthor : ISurvivorAuthor
00016:         {
00017:             public TestSurvivorAuthor(string id, string name, RiskBiasTrait bias)
00018:             {
00019:                 Id = id;
00020:                 DisplayName = name;
00021:                 RiskBias = bias;
00022:             }
00023:
00024:             public string Id { get; }
00025:             public string DisplayName { get; }
00026:             public RiskBiasTrait RiskBias { get; }
00027:         }
00028:
00029:         [Fact]
00030:         public void AutopsyProducer_WritesJournalEntry_AndEnforcesDedupOnRepeat()
00031:         {
00032:             var journal = new JournalSystem();
00033:             var rng = new SeededRng(42);
00034:             var inv = new Ashfall.Core.Inventory.Inventory();
00035:             inv.AddById("item_scalpel", 5);
00036:             inv.AddById("item_formalin", 5);
00037:             var radiation = new Ashfall.Core.Radiation.RadiationSystem(seed: 42);
00038:             var starting = new Ashfall.Core.StartingLevel.StartingLevelSystem();
00039:             var ventilation = new Ashfall.Core.VentilationSystem(starting);
00040:             var research = new Ashfall.Core.ResearchSystem();
00041:             var medical = new Ashfall.Core.Medical.MedicalWardSystem(
00042:                 new Ashfall.Core.Medical.MedicalWardState(),
00043:                 new[] { new Ashfall.Core.Medical.MedicalBed("bed_1", "Bed 1", Ashfall.Core.Medical.MedicalBedCategory.General) },
00044:                 new[] { new Ashfall.Core.Medical.MedicalProcedureDef("proc_1", "Proc", "Med") });
00045:
00046:             var autopsy = new AutopsySystem(rng, inv, radiation, ventilation, research, medical);
00047:             autopsy.LoadCatalog(new List<AutopsyProcedure>
00048:             {
00049:                 new AutopsyProcedure
00050:                 {
00051:                     procedure_id = "proc_standard",
00052:                     display_name = "Standard Screen",
00053:                     requiredTools = new List<string> { "item_scalpel" },
00054:                     requiredConsumables = new List<string> { "item_formalin" },
00055:                     possibleFindings = new List<string> { "radiation_tissue_necrosis" }
00056:                 }
00057:             });
00058:
00059:             // Wire producer hook as in host
00060:             autopsy.OnCaseCompleted += c =>
00061:             {
00062:                 journal.TryAddRawEntry(
00063:                     "autopsy_finding_" + c.caseId,
00064:                     $"Autopsy complete for {c.specimenId}: {c.finding}",
00065:                     new TestSurvivorAuthor(c.assignedMedicId, "Dr. Medic", RiskBiasTrait.Cautious),
00066:                     day: 5,
00067:                     hour: 14f);
00068:             };
00069:
00070:             autopsy.QueueAutopsy("specimen_alpha", "proc_standard", "medic_bob");
00071:             var c1 = autopsy.State.cases[0];
00072:             autopsy.BeginAutopsy(c1.caseId);
00073:             autopsy.TickDay(5);
00074:
00075:             Assert.Equal(1, journal.EntryCount);
00076:             Assert.Contains("specimen_alpha", journal.Entries[0].Text);
00077:             Assert.Equal("autopsy_finding_" + c1.caseId, journal.Entries[0].KnowledgeKey);
00078:             Assert.Equal("Dr. Medic", journal.Entries[0].AuthorName);
00079:             Assert.True(journal.HasUnread);
00080:
00081:             // Attempting to log the identical key again (e.g. duplicate notification) must be blocked
00082:             var duplicateEntry = journal.TryAddRawEntry(
00083:                 "autopsy_finding_" + c1.caseId,
00084:                 "Duplicate report text",
00085:                 new TestSurvivorAuthor("medic_bob", "Bob", RiskBiasTrait.Realist),
00086:                 day: 6);
00087:
00088:             Assert.Null(duplicateEntry);
00089:             Assert.Equal(1, journal.EntryCount);
00090:         }
00091:
00092:         [Fact]
00093:         public void LibraryStudyProducer_GrantsKnowledgeUnlocks_ViaAddKnowledgeEvidence()
00094:         {
00095:             var skills = new SkillProgressionSystem();
00096:             var research = new ResearchSystem();
00097:             var journal = new JournalSystem();
00098:             var roster = new DutyRosterSystem();
00099:             var library = new LibraryStudySystem(skills, research, journal, roster);
00100:
00101:             var manual = new ManualDefinition
00102:             {
00103:                 manual_id = "man_radiation_physics",
00104:                 display_name = "Principles of Radiation Shielding",
00105:                 category = "technical",
00106:                 studyHoursRequired = 8,
00107:                 knowledgeUnlocks = new List<string> { "k_lead_baffling", "k_dosimeter_calibration" }
00108:             };
00109:             library.LoadCatalog(new List<ManualDefinition> { manual });
00110:
00111:             Assert.Equal(0, journal.CodexUnlockCount);
00112:             Assert.False(journal.Knowledge.Has("k_lead_baffling"));
00113:             Assert.False(journal.Knowledge.Has("k_dosimeter_calibration"));
00114:
00115:             var result = library.StartStudy("man_radiation_physics", "survivor_clara");
00116:             Assert.True(result.IsSuccess);
00117:
00118:             // Tick 1 day (8 hours) -> completes manual
00119:             library.TickDay(1);
00120:
00121:             Assert.True(library.IsManualCompleted("man_radiation_physics"));
00122:             Assert.Equal(2, journal.CodexUnlockCount);
00123:             Assert.True(journal.Knowledge.Has("k_lead_baffling"));
00124:             Assert.True(journal.Knowledge.Has("k_dosimeter_calibration"));
00125:
00126:             // Repeated call to AddKnowledgeEvidence for same key returns false and does not increment unlocks
00127:             bool repeatUnlock = journal.AddKnowledgeEvidence("survivor_clara", "k_lead_baffling");
00128:             Assert.False(repeatUnlock);
00129:             Assert.Equal(2, journal.CodexUnlockCount);
00130:         }
00131:
00132:         [Fact]
00133:         public void MoralChoiceProducer_WritesJournalEntry_OnQuestResolved()
00134:         {
00135:             var journal = new JournalSystem();
00136:             var flags = new InMemoryFlagLedger();
00137:             var rng = new SeededRng(1337);
00138:             var moralSystem = new MoralChoiceSystem(rng, flags: flags);
00139:
00140:             moralSystem.OnQuestResolved += r =>
00141:             {
00142:                 journal.TryAddRawEntry(
00143:                     r.questId,
00144:                     $"Resolution: {r.epitaph}",
00145:                     new TestSurvivorAuthor("lead_survivor", "Commander", RiskBiasTrait.Realist),
00146:                     r.resolvedDay);
00147:             };
00148:
00149:             var quest = new MoralChoiceQuestDefinition
00150:             {
00151:                 Id = "quest_moral_ration_rationing",
00152:                 DisplayName = "Ration Distribution",
00153:                 Category = "resource",
00154:                 Trigger = "Food stores are low.",
00155:                 Discovery = "A choice must be made.",
00156:                 LocationId = "loc_bunker",
00157:                 MinDay = 0,
00158:                 MaxDay = 10,
00159:                 Choices = new List<MoralChoiceOption>
00160:                 {
00161:                     new MoralChoiceOption
00162:                     {
00163:                         Label = "Equal portions for all",
00164:                         MoralDelta = 5,
00165:                         EmpathyDelta = 1,
00166:                         OutcomeText = "Equal portions for all.",
00167:                         Epitaph = "Everyone ate equal crumbs."
00168:                     }
00169:                 }
00170:             };
00171:
00172:             var resolution = moralSystem.Resolve(quest, choiceIndex: 0, quest.LocationId, day: 3);
00173:
00174:             Assert.NotNull(resolution);
00175:             Assert.Equal(1, journal.EntryCount);
00176:             Assert.Equal("quest_moral_ration_rationing", journal.Entries[0].KnowledgeKey);
00177:             Assert.Contains("crumbs", journal.Entries[0].Text);
00178:             Assert.True(journal.HasUnread);
00179:         }
00180:
00181:         [Fact]
00182:         public void ProceduralEulogyEngine_ComposesFullMemorial_AndSupportsLosslessSaveRestore()
00183:         {
00184:             var engine = new ProceduralEulogyEngine();
00185:             string lastDwellerId = null;
00186:             string lastEulogyText = null;
00187:             engine.OnEulogySpoken += (id, text) =>
00188:             {
00189:                 lastDwellerId = id;
00190:                 lastEulogyText = text;
00191:             };
00192:
00193:             var record = new DwellerLifeRecord
00194:             {
00195:                 dwellerId = "dw_marcus",
00196:                 dwellerName = "Marcus Vance",
00197:                 preWarProfession = "Locomotive Mechanic",
00198:                 daysSurvived = 142,
00199:                 shiftsCompleted = 84,
00200:                 mealsPrepared = 12,
00201:                 radDoseAbsorbedMsv = 450,
00202:                 causeOfDeath = "Acute Radiation Sickness",
00203:                 favoriteRelicName = "a rusted brass caliper",
00204:                 memorableBarkSnippets = new List<string>
00205:                 {
00206:                     "Keep the valves greased.",
00207:                     "If the seam leaks, do not look at it."
00208:                 }
00209:             };
00210:
00211:             string composed = engine.ComposeEulogy(record);
00212:
00213:             Assert.NotNull(composed);
00214:             Assert.Equal("dw_marcus", lastDwellerId);
00215:             Assert.Equal(composed, lastEulogyText);
00216:             Assert.Contains("MARCUS VANCE", composed);
00217:             Assert.Contains("Locomotive Mechanic", composed);
00218:             Assert.Contains("142 days", composed);
00219:             Assert.Contains("84 watches", composed);
00220:             Assert.Contains("If the seam leaks, do not look at it.", composed);
00221:             Assert.Contains("a rusted brass caliper", composed);
00222:             Assert.Contains("Acute Radiation Sickness", composed);
00223:             Assert.Single(engine.ArchivedEulogies);
00224:
00225:             // Save and restore
00226:             var save = engine.CaptureState();
00227:             Assert.Single(save.archivedEulogyTexts);
00228:
00229:             var restored = new ProceduralEulogyEngine();
00230:             int restoreEventsFired = 0;
00231:             restored.OnEulogySpoken += (_, _) => restoreEventsFired++;
00232:
00233:             restored.RestoreState(save);
00234:
00235:             // Restore suppression: 0 events emitted on restore
00236:             Assert.Equal(0, restoreEventsFired);
00237:             Assert.Single(restored.ArchivedEulogies);
00238:             Assert.Equal(composed, restored.ArchivedEulogies[0]);
00239:         }
00240:
00241:         [Fact]
00242:         public void JournalVoice_ToneShiftsWithRiskBias_AndFormatsCleanly()
00243:         {
00244:             var variants = new Dictionary<string, JournalVoiceProseEntry>
00245:             {
00246:                 ["k_radiation_alarm"] = new JournalVoiceProseEntry
00247:                 {
00248:                     paranoid = "The needle is lying. It is ten times worse than the clicker says.",
00249:                     cautious = "Dosimeter spiked on the eastern perimeter. Double the lead curtains.",
00250:                     realist = "Perimeter radiation increased by 15 mSv. Rotate the guard roster.",
00251:                     reckless = "Just a little static on the tube. The dust will blow south.",
00252:                     denialist = "Old sensors always flicker in high humidity. Nothing to worry about.",
00253:                     fatalist = "The cloud came anyway. It was always going to come.",
00254:                     empath = "Everyone feels the heaviness in the air today.",
00255:                     sociopath = "If someone gets dosed, their rations can be redistributed.",
00256:                     @default = "Radiation levels changed."
00257:                 }
00258:             };
00259:
00260:             var catalog = new JournalVoiceProseCatalog(variants);
00261:             JournalVoice.BindCatalog(catalog);
00262:
00263:             try
00264:             {
00265:                 // Verify distinct prose per trait
00266:                 string paranoidProse = JournalVoice.ComposeBody("k_radiation_alarm", RiskBiasTrait.Paranoid);
00267:                 string realistProse = JournalVoice.ComposeBody("k_radiation_alarm", RiskBiasTrait.Realist);
00268:                 string recklessProse = JournalVoice.ComposeBody("k_radiation_alarm", RiskBiasTrait.Reckless);
00269:
00270:                 Assert.Contains("needle is lying", paranoidProse);
00271:                 Assert.Contains("Rotate the guard roster", realistProse);
00272:                 Assert.Contains("little static", recklessProse);
00273:                 Assert.NotEqual(paranoidProse, realistProse);
00274:
00275:                 // Verify ComposeFullText prepends Day stamp
00276:                 string fullText = JournalVoice.ComposeFullText("k_radiation_alarm", RiskBiasTrait.Realist, day: 12);
00277:                 Assert.StartsWith("Day 12. ", fullText);
00278:                 Assert.Contains("Rotate the guard roster", fullText);
00279:
00280:                 // Fallback for unknown key
00281:                 string fallback = JournalVoice.ComposeBody("k_completely_unknown_key", RiskBiasTrait.Realist);
00282:                 Assert.Equal("Something changed. I wrote it down so I would not forget.", fallback);
00283:
00284:                 // Timestamp formatting
00285:                 Assert.Equal("Day 1", JournalVoice.FormatTimestamp(1, -1f));
00286:                 Assert.Equal("Day 7, 08h", JournalVoice.FormatTimestamp(7, 8.2f));
00287:                 Assert.Equal("Day 1, 00h", JournalVoice.FormatTimestamp(-3, 0f));
00288:                 Assert.Equal("Day 5, 23h", JournalVoice.FormatTimestamp(5, 23f));
00289:             }
00290:             finally
00291:             {
00292:                 JournalVoice.BindCatalog(null);
00293:             }
00294:         }
00295:     }
00296: }
```

## `Ashfall.Core.Tests/JournalSystemCoreBehaviorTests.cs` — 98 lines; 3,744 bytes; SHA-256 `6f3bbc80f00bc8e026152834d6345566a04e3993d59c55ee196aeb3e16cf2f8c`
Declaration index:
- 00010: public class JournalSystemCoreBehaviorTests
- 00012: private class TestAuthor : ISurvivorAuthor
- 00020: public void JournalSystem_RawEntryAndCodex_DiscoversAndUnlocks()
- 00049: public void JournalSystem_TabsAndReadState_TracksAccurately()
- 00066: public void JournalSystem_SaveAndRestore_RoundTripsExactly()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Xunit;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Journal;
00007:
00008: namespace Ashfall.Core.Tests
00009: {
00010:     public class JournalSystemCoreBehaviorTests
00011:     {
00012:         private class TestAuthor : ISurvivorAuthor
00013:         {
00014:             public string Id { get; set; } = "survivor_dr_sarah_chen";
00015:             public string DisplayName { get; set; } = "Dr. Sarah Chen";
00016:             public RiskBiasTrait RiskBias { get; set; } = RiskBiasTrait.Realist;
00017:         }
00018:
00019:         [Fact]
00020:         public void JournalSystem_RawEntryAndCodex_DiscoversAndUnlocks()
00021:         {
00022:             var journal = new JournalSystem();
00023:             var author = new TestAuthor();
00024:
00025:             var entry = journal.TryAddRawEntry("event_first_snow", "The first radioactive snow began falling.", author, day: 5, hour: 14f);
00026:             Assert.NotNull(entry);
00027:             Assert.Equal("event_first_snow", entry.KnowledgeKey);
00028:             Assert.Equal("Dr. Sarah Chen", entry.AuthorName);
00029:             Assert.Equal(5, entry.Day);
00030:             Assert.True(journal.HasUnread);
00031:             Assert.True(journal.NotificationPing);
00032:
00033:             // Duplicate discovery rejected
00034:             var dupe = journal.TryAddRawEntry("event_first_snow", "Another snow entry", author, day: 6);
00035:             Assert.Null(dupe);
00036:             Assert.Single(journal.Entries);
00037:
00038:             // Codex unlocks
00039:             bool unlockedItem = journal.UnlockItemSeen("item_gas_mask");
00040:             Assert.True(unlockedItem);
00041:             Assert.True(journal.IsItemSeen("item_gas_mask"));
00042:             Assert.Equal(1, journal.CodexUnlockCount);
00043:
00044:             // Duplicate codex unlock returns false
00045:             Assert.False(journal.UnlockItemSeen("item_gas_mask"));
00046:         }
00047:
00048:         [Fact]
00049:         public void JournalSystem_TabsAndReadState_TracksAccurately()
00050:         {
00051:             var journal = new JournalSystem();
00052:             var author = new TestAuthor();
00053:
00054:             journal.TryAddRawEntry("loc_bunker_hatch", "Found the old bunker hatch.", author, day: 1);
00055:             Assert.True(journal.HasUnread);
00056:
00057:             journal.MarkRead();
00058:             Assert.False(journal.HasUnread);
00059:             Assert.False(journal.NotificationPing);
00060:
00061:             journal.SwitchTab(2);
00062:             Assert.Equal(2, journal.ActiveTab);
00063:         }
00064:
00065:         [Fact]
00066:         public void JournalSystem_SaveAndRestore_RoundTripsExactly()
00067:         {
00068:             var journal = new JournalSystem();
00069:             var author = new TestAuthor();
00070:
00071:             journal.TryAddRawEntry("entry_1", "Log entry 1", author, day: 2, hour: 8f);
00072:             journal.TryAddRawEntry("entry_2", "Log entry 2", author, day: 3, hour: 12f);
00073:             journal.UnlockItemSeen("item_radio_vacuum_tube");
00074:             journal.UnlockLocationVisited("loc_substation_echo");
00075:
00076:             var save = journal.CaptureState();
00077:             Assert.NotNull(save);
00078:             Assert.Equal(2, save.Entries.Length);
00079:             Assert.Equal(2, save.CodexUnlockCount);
00080:
00081:             var serializer = new SystemTextJsonSerializer();
00082:             string json = serializer.Serialize(save);
00083:             Assert.False(string.IsNullOrWhiteSpace(json));
00084:
00085:             var restoredSave = serializer.Deserialize<JournalSave>(json);
00086:             Assert.NotNull(restoredSave);
00087:
00088:             var newJournal = new JournalSystem();
00089:             newJournal.RestoreState(restoredSave);
00090:
00091:             Assert.Equal(2, newJournal.EntryCount);
00092:             Assert.Equal(2, newJournal.CodexUnlockCount);
00093:             Assert.True(newJournal.IsItemSeen("item_radio_vacuum_tube"));
00094:             Assert.True(newJournal.IsLocationVisited("loc_substation_echo"));
00095:             Assert.Equal("entry_2", newJournal.Entries[0].KnowledgeKey);
00096:         }
00097:     }
00098: }
```

## `Ashfall.Core.Tests/JournalSystemTests.cs` — 376 lines; 14,439 bytes; SHA-256 `1b17cc5ad807b917c4745d5552d7755ed96a30b61aca4290ad49ff55b83ae8ce`
Declaration index:
- 00012: public class JournalSystemTests
- 00014: private sealed class Author : ISurvivorAuthor
- 00030: public void TryDiscover_DeduplicatesPerKnowledgeKey()
- 00045: public void TryDiscover_RejectsEmptyKey()
- 00054: public void TryAddRawEntry_RecordsFreeformText_OncePerKey()
- 00069: public void MaxEntries_EvictsOldest()
- 00083: public void CodexUnlock_RecordsAndFlags()
- 00094: public void MarkReadAndAcknowledgePing_ClearFlags()
- 00110: public void CaptureRestore_RoundTrips_EntriesKnowledgeAndFlags()
- 00136: public void Clear_ResetsEverything()
- 00152: public void RestoreState_HandlesNull()
- 00160: public void EntryLifecycle_NewestFirstOrdering_AndSequenceId()
- 00179: public void TryDiscoverKnowledge_DualContract_EntryAndCodexUnlock()
- 00206: public void AddKnowledgeEvidence_Vs_TryDiscoverKnowledge_Interaction()
- 00223: public void TryAddRawEntry_EdgeCases_NullAuthor_ClampedDay_FormattedHour()
- 00241: public void MaxEntries_WithRecyclerAndFactory_RecyclesEvictedAndClears()
- 00273: public void Tabs_Switching_Clamping_AndLastSeenTracking()
- 00321: public void RestoreState_SuppressesAllEvents()
- 00357: public void DeterministicOrdering_WithoutWallClock()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using Ashfall.Core.Journal;
00003: using Xunit;
00004:
00005: namespace Ashfall.Core.Tests
00006: {
00007:     /// <summary>
00008:     /// H11 hardening: JournalSystem previously had zero tests. These cover the
00009:     /// dedup contract, max-entry eviction, codex unlocks, unread/ping state,
00010:     /// and a lossless CaptureState/RestoreState round-trip (Invariant 3).
00011:     /// </summary>
00012:     public class JournalSystemTests
00013:     {
00014:         private sealed class Author : ISurvivorAuthor
00015:         {
00016:             public Author(string id, string name = null, RiskBiasTrait bias = RiskBiasTrait.Realist)
00017:             {
00018:                 Id = id;
00019:                 DisplayName = name ?? id;
00020:                 RiskBias = bias;
00021:             }
00022:             public string Id { get; }
00023:             public string DisplayName { get; }
00024:             public RiskBiasTrait RiskBias { get; }
00025:         }
00026:
00027:         private static readonly Author TestAuthor = new Author("sv_jane", "Jane", RiskBiasTrait.Reckless);
00028:
00029:         [Fact]
00030:         public void TryDiscover_DeduplicatesPerKnowledgeKey()
00031:         {
00032:             var j = new JournalSystem();
00033:             var first = j.TryDiscover("k_found_radio", TestAuthor, 3);
00034:             var second = j.TryDiscover("k_found_radio", TestAuthor, 4);
00035:
00036:             Assert.NotNull(first);
00037:             Assert.Null(second);                 // once per key
00038:             Assert.Equal(1, j.EntryCount);
00039:             Assert.True(j.HasUnread);
00040:             Assert.True(j.NotificationPing);
00041:             Assert.Equal(1, j.NotificationPingCount);
00042:         }
00043:
00044:         [Fact]
00045:         public void TryDiscover_RejectsEmptyKey()
00046:         {
00047:             var j = new JournalSystem();
00048:             Assert.Null(j.TryDiscover("", TestAuthor, 3));
00049:             Assert.Null(j.TryDiscover(null, TestAuthor, 3));
00050:             Assert.Equal(0, j.EntryCount);
00051:         }
00052:
00053:         [Fact]
00054:         public void TryAddRawEntry_RecordsFreeformText_OncePerKey()
00055:         {
00056:             var j = new JournalSystem();
00057:             var e1 = j.TryAddRawEntry("k_diary_ghost", "We found the station.", TestAuthor, 5, 7.5f);
00058:             var e2 = j.TryAddRawEntry("k_diary_ghost", "Different text.", TestAuthor, 6);
00059:
00060:             Assert.NotNull(e1);
00061:             Assert.Null(e2);                     // deduped
00062:             Assert.Equal(1, j.EntryCount);
00063:             Assert.Contains("station", j.LatestText);
00064:             Assert.Equal("Jane", j.Entries[0].AuthorName);
00065:             Assert.Equal("sv_jane", j.Entries[0].AuthorId);
00066:         }
00067:
00068:         [Fact]
00069:         public void MaxEntries_EvictsOldest()
00070:         {
00071:             var j = new JournalSystem();
00072:             // 65 distinct keys beyond 64 cap.
00073:             for (int i = 0; i < JournalSystem.MaxEntries + 1; i++)
00074:                 j.TryDiscover("k_log_" + i, TestAuthor, 1);
00075:
00076:             Assert.Equal(JournalSystem.MaxEntries, j.EntryCount);
00077:             // Newest inserted at index 0; oldest (k_log_0) evicted.
00078:             Assert.Equal("k_log_" + JournalSystem.MaxEntries, j.Entries[0].KnowledgeKey);
00079:             Assert.DoesNotContain(j.Entries, e => e.KnowledgeKey == "k_log_0");
00080:         }
00081:
00082:         [Fact]
00083:         public void CodexUnlock_RecordsAndFlags()
00084:         {
00085:             var j = new JournalSystem();
00086:             Assert.False(j.IsItemSeen("item_uplink"));
00087:             Assert.True(j.UnlockItemSeen("item_uplink"));
00088:             Assert.True(j.IsItemSeen("item_uplink"));
00089:             Assert.False(j.UnlockItemSeen("item_uplink"));   // already unlocked
00090:             Assert.Equal(1, j.CodexUnlockCount);
00091:         }
00092:
00093:         [Fact]
00094:         public void MarkReadAndAcknowledgePing_ClearFlags()
00095:         {
00096:             var j = new JournalSystem();
00097:             j.TryDiscover("k_ping", TestAuthor, 1);
00098:             Assert.True(j.HasUnread);
00099:             Assert.True(j.NotificationPing);
00100:
00101:             j.AcknowledgePing();
00102:             Assert.False(j.NotificationPing);
00103:             Assert.True(j.HasUnread);             // acknowledge only clears ping
00104:
00105:             j.MarkRead();
00106:             Assert.False(j.HasUnread);
00107:         }
00108:
00109:         [Fact]
00110:         public void CaptureRestore_RoundTrips_EntriesKnowledgeAndFlags()
00111:         {
00112:             var j = new JournalSystem();
00113:             j.TryDiscover("k_vault", TestAuthor, 3, 4f);
00114:             j.TryAddRawEntry("k_letter", "Signed under lantern light.", TestAuthor, 8);
00115:             j.UnlockLocationVisited("loc_quartz_office");
00116:             j.MarkRead();                          // persist has-unread=false
00117:             Assert.Equal(2, j.EntryCount);
00118:
00119:             var restored = new JournalSystem();
00120:             restored.RestoreState(j.CaptureState());
00121:
00122:             Assert.Equal(2, restored.EntryCount);
00123:             Assert.Equal("k_letter", restored.Entries[0].KnowledgeKey);   // newest first
00124:             Assert.Equal(8, restored.Entries[0].Day);
00125:             Assert.Equal("k_vault", restored.Entries[1].KnowledgeKey);
00126:             Assert.Equal(3, restored.Entries[1].Day);
00127:             Assert.True(restored.IsLocationVisited("loc_quartz_office"));
00128:             Assert.Equal(1, restored.CodexUnlockCount);
00129:             Assert.False(restored.HasUnread);
00130:             // Dedup must hold after restore: same key cannot be re-added.
00131:             Assert.Null(restored.TryDiscover("k_vault", TestAuthor, 9));
00132:             Assert.Equal(2, restored.EntryCount);
00133:         }
00134:
00135:         [Fact]
00136:         public void Clear_ResetsEverything()
00137:         {
00138:             var j = new JournalSystem();
00139:             j.TryDiscover("k_reset", TestAuthor, 2);
00140:             j.UnlockItemSeen("item_cleaver");
00141:             j.Clear();
00142:
00143:             Assert.Equal(0, j.EntryCount);
00144:             Assert.Equal(0, j.CodexUnlockCount);
00145:             Assert.False(j.HasUnread);
00146:             Assert.False(j.NotificationPing);
00147:             Assert.Equal(0, j.NotificationPingCount);
00148:             Assert.False(j.IsItemSeen("item_cleaver"));
00149:         }
00150:
00151:         [Fact]
00152:         public void RestoreState_HandlesNull()
00153:         {
00154:             var j = new JournalSystem();
00155:             j.RestoreState(null);                  // must not throw
00156:             Assert.Equal(0, j.EntryCount);
00157:         }
00158:
00159:         [Fact]
00160:         public void EntryLifecycle_NewestFirstOrdering_AndSequenceId()
00161:         {
00162:             var j = new JournalSystem();
00163:             var e1 = j.TryDiscover("k_first", TestAuthor, 1, 8f);
00164:             var e2 = j.TryDiscover("k_second", TestAuthor, 2, 14f);
00165:
00166:             Assert.NotNull(e1);
00167:             Assert.NotNull(e2);
00168:             Assert.Equal(2, j.EntryCount);
00169:             // Newest-first: index 0 is second entry
00170:             Assert.Equal("k_second", j.Entries[0].KnowledgeKey);
00171:             Assert.Equal("journal_2_k_second", j.Entries[0].Id);
00172:             Assert.Equal("k_first", j.Entries[1].KnowledgeKey);
00173:             Assert.Equal("journal_1_k_first", j.Entries[1].Id);
00174:             Assert.Equal(2, j.Entries[0].Day);
00175:             Assert.Equal(1, j.Entries[1].Day);
00176:         }
00177:
00178:         [Fact]
00179:         public void TryDiscoverKnowledge_DualContract_EntryAndCodexUnlock()
00180:         {
00181:             var j = new JournalSystem();
00182:             int codexEvents = 0;
00183:             string lastCodexKey = null;
00184:             j.OnCodexUnlocked += k =>
00185:             {
00186:                 codexEvents++;
00187:                 lastCodexKey = k;
00188:             };
00189:
00190:             var entry = j.TryDiscoverKnowledge("k_dual_ruin", TestAuthor, 5, 12f);
00191:             Assert.NotNull(entry);
00192:             Assert.Equal(1, j.EntryCount);
00193:             Assert.Equal(1, j.CodexUnlockCount);
00194:             Assert.Equal(1, codexEvents);
00195:             Assert.Equal("k_dual_ruin", lastCodexKey);
00196:
00197:             // Repeat attempt must return null, add no entry, and not fire event
00198:             var repeat = j.TryDiscoverKnowledge("k_dual_ruin", TestAuthor, 6);
00199:             Assert.Null(repeat);
00200:             Assert.Equal(1, j.EntryCount);
00201:             Assert.Equal(1, j.CodexUnlockCount);
00202:             Assert.Equal(1, codexEvents);
00203:         }
00204:
00205:         [Fact]
00206:         public void AddKnowledgeEvidence_Vs_TryDiscoverKnowledge_Interaction()
00207:         {
00208:             var j = new JournalSystem();
00209:             // AddKnowledgeEvidence unlocks codex only; no journal log entry is created
00210:             bool unlocked = j.AddKnowledgeEvidence("sv_jane", "k_evidence_chem");
00211:             Assert.True(unlocked);
00212:             Assert.Equal(1, j.CodexUnlockCount);
00213:             Assert.Equal(0, j.EntryCount);
00214:
00215:             // Calling TryDiscoverKnowledge with the same key must return null because knowledge is already learned
00216:             var entry = j.TryDiscoverKnowledge("k_evidence_chem", TestAuthor, 3);
00217:             Assert.Null(entry);
00218:             Assert.Equal(0, j.EntryCount);
00219:             Assert.Equal(1, j.CodexUnlockCount);
00220:         }
00221:
00222:         [Fact]
00223:         public void TryAddRawEntry_EdgeCases_NullAuthor_ClampedDay_FormattedHour()
00224:         {
00225:             var j = new JournalSystem();
00226:             // Null author, negative day, hour specified
00227:             var entry = j.TryAddRawEntry("k_anonymous", "A voice on the wire.", null!, day: -5, hour: 9.5f);
00228:
00229:             Assert.NotNull(entry);
00230:             Assert.Equal("Unknown", entry.AuthorName);
00231:             Assert.Equal(string.Empty, entry.AuthorId);
00232:             Assert.Equal(1, entry.Day); // Clamped to 1
00233:             Assert.Equal("Day 1, 09h", entry.Timestamp);
00234:
00235:             // Empty/whitespace text rejected
00236:             Assert.Null(j.TryAddRawEntry("k_blank", "", TestAuthor, 1));
00237:             Assert.Null(j.TryAddRawEntry("k_null", null!, TestAuthor, 1));
00238:         }
00239:
00240:         [Fact]
00241:         public void MaxEntries_WithRecyclerAndFactory_RecyclesEvictedAndClears()
00242:         {
00243:             var j = new JournalSystem();
00244:             var created = new System.Collections.Generic.List<JournalEntry>();
00245:             var recycled = new System.Collections.Generic.List<JournalEntry>();
00246:
00247:             j.SetEntryFactory(
00248:                 () =>
00249:                 {
00250:                     var e = new JournalEntry();
00251:                     created.Add(e);
00252:                     return e;
00253:                 },
00254:                 e => recycled.Add(e));
00255:
00256:             // Add 65 distinct entries
00257:             for (int i = 0; i < JournalSystem.MaxEntries + 1; i++)
00258:             {
00259:                 j.TryDiscover($"k_recycle_{i}", TestAuthor, i + 1);
00260:             }
00261:
00262:             Assert.Equal(JournalSystem.MaxEntries + 1, created.Count);
00263:             Assert.Single(recycled); // 1 evicted entry passed to recycler
00264:             Assert.Equal("k_recycle_0", recycled[0].KnowledgeKey); // Oldest was recycled
00265:
00266:             // Clear should recycle all remaining 64 entries
00267:             j.Clear();
00268:             Assert.Equal(JournalSystem.MaxEntries + 1, recycled.Count);
00269:             Assert.Equal(0, j.EntryCount);
00270:         }
00271:
00272:         [Fact]
00273:         public void Tabs_Switching_Clamping_AndLastSeenTracking()
00274:         {
00275:             var j = new JournalSystem();
00276:             int tabChanges = 0;
00277:             int lastChangedTab = -1;
00278:             j.OnTabChanged += t =>
00279:             {
00280:                 tabChanges++;
00281:                 lastChangedTab = t;
00282:             };
00283:
00284:             // Initial state
00285:             Assert.Equal(0, j.ActiveTab);
00286:             Assert.Equal(-1, j.GetLastSeenIndex(0));
00287:             Assert.Equal(-1, j.GetLastSeenIndex(99)); // Invalid returns -1
00288:
00289:             // Switch tab with clamping
00290:             j.SwitchTab(-5); // Already 0, no change
00291:             Assert.Equal(0, tabChanges);
00292:
00293:             j.SwitchTab(3);
00294:             Assert.Equal(3, j.ActiveTab);
00295:             Assert.Equal(1, tabChanges);
00296:             Assert.Equal(3, lastChangedTab);
00297:
00298:             j.SwitchTab(99); // Clamps to TabCount - 1 (4)
00299:             Assert.Equal(JournalSystem.TabCount - 1, j.ActiveTab);
00300:             Assert.Equal(2, tabChanges);
00301:
00302:             // Test unread tracking across tabs
00303:             j.MarkTabViewed(2); // Viewed tab 2 with 0 entries, 0 codex
00304:             Assert.False(j.HasUnreadForTab(2));
00305:
00306:             // Add entry and codex unlock
00307:             j.TryDiscover("k_tab_test", TestAuthor, 1);
00308:             j.UnlockItemSeen("item_battery");
00309:
00310:             Assert.True(j.HasUnreadForTab(2));
00311:             j.MarkTabViewed(2);
00312:             Assert.False(j.HasUnreadForTab(2));
00313:
00314:             // MarkTabViewed(0) clears global unread & ping
00315:             j.MarkTabViewed(0);
00316:             Assert.False(j.HasUnread);
00317:             Assert.False(j.NotificationPing);
00318:         }
00319:
00320:         [Fact]
00321:         public void RestoreState_SuppressesAllEvents()
00322:         {
00323:             var initial = new JournalSystem();
00324:             initial.TryDiscover("k_event_1", TestAuthor, 1);
00325:             initial.TryDiscoverKnowledge("k_event_2", TestAuthor, 2);
00326:             initial.UnlockLocationVisited("loc_depot");
00327:             initial.SwitchTab(3);
00328:
00329:             var save = initial.CaptureState();
00330:
00331:             var restored = new JournalSystem();
00332:             int entriesAdded = 0;
00333:             int pings = 0;
00334:             int tabChanges = 0;
00335:             int codexUnlocks = 0;
00336:
00337:             restored.OnEntryAdded += _ => entriesAdded++;
00338:             restored.OnNotificationPing += _ => pings++;
00339:             restored.OnTabChanged += _ => tabChanges++;
00340:             restored.OnCodexUnlocked += _ => codexUnlocks++;
00341:
00342:             restored.RestoreState(save);
00343:
00344:             // Invariant: RestoreState must NEVER emit mutation events
00345:             Assert.Equal(0, entriesAdded);
00346:             Assert.Equal(0, pings);
00347:             Assert.Equal(0, tabChanges);
00348:             Assert.Equal(0, codexUnlocks);
00349:
00350:             // But state is correctly restored
00351:             Assert.Equal(2, restored.EntryCount);
00352:             Assert.Equal(2, restored.CodexUnlockCount);
00353:             Assert.Equal(3, restored.ActiveTab);
00354:         }
00355:
00356:         [Fact]
00357:         public void DeterministicOrdering_WithoutWallClock()
00358:         {
00359:             var j1 = new JournalSystem();
00360:             var j2 = new JournalSystem();
00361:
00362:             j1.TryAddRawEntry("k_clock_1", "Text 1", TestAuthor, day: 4, hour: 15.2f);
00363:             j1.TryAddRawEntry("k_clock_2", "Text 2", TestAuthor, day: 5, hour: 2.0f);
00364:
00365:             j2.TryAddRawEntry("k_clock_1", "Text 1", TestAuthor, day: 4, hour: 15.2f);
00366:             j2.TryAddRawEntry("k_clock_2", "Text 2", TestAuthor, day: 5, hour: 2.0f);
00367:
00368:             Assert.Equal(j1.Entries[0].Timestamp, j2.Entries[0].Timestamp);
00369:             Assert.Equal("Day 5, 02h", j1.Entries[0].Timestamp);
00370:             Assert.Equal(j1.Entries[1].Timestamp, j2.Entries[1].Timestamp);
00371:             Assert.Equal("Day 4, 15h", j1.Entries[1].Timestamp);
00372:             Assert.Equal(j1.Entries[0].Id, j2.Entries[0].Id);
00373:             Assert.Equal(j1.Entries[1].Id, j2.Entries[1].Id);
00374:         }
00375:     }
00376: }
```

## `Assets/Ashfall.Core/Journal/RiskBiasTrait.cs` — 42 lines; 1,524 bytes; SHA-256 `c8b894a04a78bf7868f6003a44dbc9a1ff44be4866b9757ff8a1eb238448b9d0`
Declaration index:
- 00014: public enum RiskBiasTrait
- 00036: public interface ISurvivorAuthor
```csharp
00001: // SPDX-License-Identifier: MIT
00002: namespace Ashfall.Core.Journal
00003: {
00004:     /// <summary>
00005:     /// A survivor's characteristic bias in how they interpret radiation risk.
00006:     /// Same world state, different felt danger — this is what makes two survivors
00007:     /// in the same bunker act differently. See BeliefSystem.
00008:     ///
00009:     /// This is the complete set. The Godot port previously carried its own copy that
00010:     /// stopped at <see cref="Fatalist"/>, so a survivor persisted as Empath or Sociopath
00011:     /// deserialized into a value that host could not interpret. Members are persisted by
00012:     /// ordinal — only ever append, never reorder or remove.
00013:     /// </summary>
00014:     public enum RiskBiasTrait
00015:     {
00016:         Paranoid,
00017:         Cautious,
00018:         Realist,
00019:         Reckless,
00020:         Denialist,
00021:         Fatalist,
00022:
00023:         /// <summary>Gains/loses morale based on the bunker's average morale.
00024:         /// A fragile barometer for the group.</summary>
00025:         Empath,
00026:
00027:         /// <summary>Suffers zero morale loss when another survivor dies.
00028:         /// Terrifies others but makes a perfect cold-blooded scavenger.</summary>
00029:         Sociopath
00030:     }
00031:
00032:     /// <summary>
00033:     /// Lightweight author view used by the journal. This decouples the journal from the full
00034:     /// survivor class; anything with an id, a display name and a risk bias can write entries.
00035:     /// </summary>
00036:     public interface ISurvivorAuthor
00037:     {
00038:         string Id { get; }
00039:         string DisplayName { get; }
00040:         RiskBiasTrait RiskBias { get; }
00041:     }
00042: }
```

## `Ashfall.Core.Tests/CollectibleNarrativeQualityTests.cs` — 238 lines; 11,250 bytes; SHA-256 `e5c5f527dd779630627a5acb22aa21d082f7b785e41f53762ea9ad58590b0e8a`
Declaration index:
- 00028: public class CollectibleNarrativeQualityTests
- 00034: private static string FindDataDir()
- 00046: private sealed record CollectibleText(
- 00050: private static List<CollectibleText> LoadCorpus()
- 00076: private static int CountSentences(string text)
- 00082: private static int CountTerm(string haystack, string needle) => Regex.Matches(
- 00088: public void All40Descriptions_NonEmpty_AtMostThreeSentences()
- 00098: public void All40Names_NonEmpty_AtMostFiftyChars_UniqueWithinCategory()
- 00123: public void HardClicheTerms_StayWithinCeilings(string term, int ceiling)
- 00142: public void Descriptions_ContainNoRealBrandsPublicationsOrTeams()
- 00156: public void Descriptions_ContainNoModernInternetSlang()
- 00171: public void Descriptions_ContainNoProceduralConstructionOrHazardInstructions()
- 00190: private static JournalVoiceProseCatalog LoadProse()
- 00199: public void JournalAndFactionTargets_ResolveAgainstProseAuthority()
- 00212: public void CodexTargets_DefaultAndRealistProse_AreTwoToFourSentences()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using System.Text.RegularExpressions;
00008: using Ashfall.Core;
00009: using Ashfall.Core.Journal;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests
00013: {
00014:     /// <summary>
00015:     /// Flagship XII — narrative quality and localization-readiness gates for the
00016:     /// 40-item collectible corpus. Machine-enforceable editorial checks only:
00017:     /// counts, non-empty text, name length/uniqueness, sentence ceilings,
00018:     /// cliché frequencies, brand/slang/procedural-instruction blacklists, and
00019:     /// generic codex-target resolution. Emotional-register distribution is an
00020:     /// editorial judgement and lives in
00021:     /// docs/narrative/COLLECTIBLES_NARRATIVE_QUALITY_AUDIT.md, not here.
00022:     ///
00023:     /// Localization model: catalog text is intentionally raw default-language
00024:     /// strings in items.json (the canonical model used by every mature catalog);
00025:     /// LocalizationService keys are UI chrome only. These gates pin the raw
00026:     /// strings' quality so a later key-first migration starts from a clean corpus.
00027:     /// </summary>
00028:     public class CollectibleNarrativeQualityTests
00029:     {
00030:         private static readonly string DataDir = FindDataDir();
00031:         private static readonly IFileIO FileIO = new FileSystemIO();
00032:         private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();
00033:
00034:         private static string FindDataDir()
00035:         {
00036:             string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
00037:             while (dir != null)
00038:             {
00039:                 string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "collectibles.json");
00040:                 if (File.Exists(probe)) return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
00041:                 dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
00042:             }
00043:             throw new DirectoryNotFoundException("data authority not found");
00044:         }
00045:
00046:         private sealed record CollectibleText(
00047:             string ItemId, string Category, string EffectType, string EffectTarget,
00048:             string DisplayName, string Description);
00049:
00050:         private static List<CollectibleText> LoadCorpus()
00051:         {
00052:             var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer)
00053:                 ?? throw new InvalidOperationException("collectibles.json must load");
00054:
00055:             string raw = FileIO.ReadAllText(Path.Combine(DataDir, "items.json"));
00056:             using var json = JsonDocument.Parse(raw);
00057:             var items = json.RootElement.GetProperty("items");
00058:
00059:             var corpus = new List<CollectibleText>();
00060:             foreach (var it in items.EnumerateArray())
00061:             {
00062:                 string id = it.GetProperty("id").GetString() ?? "";
00063:                 var def = catalog.GetByItemId(id);
00064:                 if (def == null) continue;
00065:                 corpus.Add(new CollectibleText(
00066:                     id, def.category, def.effect_type, def.effect_target,
00067:                     it.GetProperty("displayName").GetString() ?? "",
00068:                     it.GetProperty("description").GetString() ?? ""));
00069:             }
00070:             Assert.Equal(40, corpus.Count);
00071:             return corpus;
00072:         }
00073:
00074:         /// <summary>Simple terminator-based sentence counter (project-approved;
00075:         /// no NLP dependency): split on . ! ? followed by whitespace/end.</summary>
00076:         private static int CountSentences(string text)
00077:         {
00078:             if (string.IsNullOrWhiteSpace(text)) return 0;
00079:             return Regex.Split(text, @"[.!?]+(?:\s|$)").Count(s => !string.IsNullOrWhiteSpace(s));
00080:         }
00081:
00082:         private static int CountTerm(string haystack, string needle) => Regex.Matches(
00083:             haystack.ToLowerInvariant(), $@"\b{Regex.Escape(needle.ToLowerInvariant())}\b").Count;
00084:
00085:         // ── Hard text gates ─────────────────────────────────────────
00086:
00087:         [Fact]
00088:         public void All40Descriptions_NonEmpty_AtMostThreeSentences()
00089:         {
00090:             var broken = LoadCorpus()
00091:                 .Where(c => string.IsNullOrWhiteSpace(c.Description) || CountSentences(c.Description) > 3)
00092:                 .Select(c => $"{c.ItemId}: sentences={CountSentences(c.Description)}")
00093:                 .ToList();
00094:             Assert.True(broken.Count == 0, "description empty or >3 sentences:\n" + string.Join("\n", broken));
00095:         }
00096:
00097:         [Fact]
00098:         public void All40Names_NonEmpty_AtMostFiftyChars_UniqueWithinCategory()
00099:         {
00100:             var corpus = LoadCorpus();
00101:             var broken = corpus
00102:                 .Where(c => string.IsNullOrWhiteSpace(c.DisplayName) || c.DisplayName.Length > 50)
00103:                 .Select(c => $"{c.ItemId}: len={c.DisplayName.Length}")
00104:                 .ToList();
00105:
00106:             var dupes = corpus
00107:                 .GroupBy(c => $"{c.Category}|{c.DisplayName}", StringComparer.Ordinal)
00108:                 .Where(g => g.Count() > 1)
00109:                 .Select(g => g.Key)
00110:                 .ToList();
00111:
00112:             Assert.True(broken.Count == 0, "name empty or >50 chars:\n" + string.Join("\n", broken));
00113:             Assert.True(dupes.Count == 0, "duplicate display name within category:\n" + string.Join("\n", dupes));
00114:         }
00115:
00116:         // ── Cliché ceilings (Flagship XII §1.5) ─────────────────────
00117:
00118:         [Theory]
00119:         [InlineData("faded", 2)]
00120:         [InlineData("torn", 2)]
00121:         [InlineData("bloodstained", 2)]
00122:         [InlineData("haunting reminder", 2)]
00123:         public void HardClicheTerms_StayWithinCeilings(string term, int ceiling)
00124:         {
00125:             string all = string.Join("\n", LoadCorpus().Select(c => c.Description));
00126:             int uses = CountTerm(all, term);
00127:             Assert.True(uses <= ceiling, $"cliché '{term}' used {uses}x (ceiling {ceiling})");
00128:         }
00129:
00130:         // ── IP / brand / slang / procedural gates (§1.9, §1.10, §4.10) ──
00131:
00132:         private static readonly string[] ForbiddenRealWorldTerms =
00133:         {
00134:             "coca-cola", "pepsi", "nike", "adidas", "ford", "chevrolet", "toyota",
00135:             "sony", "nintendo", "playstation", "xbox", "harley-davidson",
00136:             "new york times", "wall street journal", "rolling stone", "forbes",
00137:             "yankees", "dodgers", "lakers", "manchester united", "fifa", "nasa",
00138:             "google", "facebook", "instagram", "tiktok"
00139:         };
00140:
00141:         [Fact]
00142:         public void Descriptions_ContainNoRealBrandsPublicationsOrTeams()
00143:         {
00144:             string all = string.Join("\n", LoadCorpus().Select(c => c.Description + "\n" + c.DisplayName));
00145:             var offenders = ForbiddenRealWorldTerms.Where(t => all.Contains(t, StringComparison.OrdinalIgnoreCase)).ToList();
00146:             Assert.True(offenders.Count == 0, "real-world terms found: " + string.Join(", ", offenders));
00147:         }
00148:
00149:         private static readonly string[] ForbiddenModernSlang =
00150:         {
00151:             "selfie", "meme", "livestream", "hashtag", "influencer", "podcast",
00152:             "emoji", "vlog", "ghosting", "vibe check", "no cap", "rizz", "yeet"
00153:         };
00154:
00155:         [Fact]
00156:         public void Descriptions_ContainNoModernInternetSlang()
00157:         {
00158:             string all = string.Join("\n", LoadCorpus().Select(c => c.Description));
00159:             var offenders = ForbiddenModernSlang.Where(t => CountTerm(all, t) > 0).ToList();
00160:             Assert.True(offenders.Count == 0, "modern slang found: " + string.Join(", ", offenders));
00161:         }
00162:
00163:         private static readonly string[] ForbiddenProceduralTerms =
00164:         {
00165:             "step 1", "step one", "detonator", "gunpowder", "enriched uranium",
00166:             "nerve agent", "mustard gas", "pipe bomb", "explosive lens",
00167:             "critical mass", "how to build", "instructions for making"
00168:         };
00169:
00170:         [Fact]
00171:         public void Descriptions_ContainNoProceduralConstructionOrHazardInstructions()
00172:         {
00173:             var corpus = LoadCorpus();
00174:             string all = string.Join("\n", corpus.Select(c => c.Description));
00175:             var offenders = ForbiddenProceduralTerms.Where(t => all.Contains(t, StringComparison.OrdinalIgnoreCase)).ToList();
00176:
00177:             // Numbered instruction sequences ("1. … 2. …") read as procedures,
00178:             // not prose.
00179:             var numbered = corpus
00180:                 .Where(c => Regex.IsMatch(c.Description, @"\b\d\.[\s]") && CountTerm(c.Description, "then") > 0)
00181:                 .Select(c => c.ItemId)
00182:                 .ToList();
00183:
00184:             Assert.True(offenders.Count == 0, "procedural/hazard terms found: " + string.Join(", ", offenders));
00185:             Assert.True(numbered.Count == 0, "numbered instruction sequences in: " + string.Join(", ", numbered));
00186:         }
00187:
00188:         // ── Codex content gates (§2.10/§3.11 generic; entry contract §2.3) ──
00189:
00190:         private static JournalVoiceProseCatalog LoadProse()
00191:         {
00192:             var loader = new JournalVoiceProseCatalogLoader(FileIO, Serializer);
00193:             var catalog = loader.Load(DataDir);
00194:             Assert.True(catalog.Count > 0, "journal_voice_prose.json must load");
00195:             return catalog;
00196:         }
00197:
00198:         [Fact]
00199:         public void JournalAndFactionTargets_ResolveAgainstProseAuthority()
00200:         {
00201:             var prose = LoadProse();
00202:             var broken = LoadCorpus()
00203:                 .Where(c => c.EffectType == "journal_unlock" || c.EffectType == "faction_info")
00204:                 .Where(c => string.IsNullOrWhiteSpace(c.EffectTarget) || !prose.HasKey(c.EffectTarget))
00205:                 .Select(c => $"{c.ItemId} -> {c.EffectTarget}")
00206:                 .ToList();
00207:             Assert.True(broken.Count == 0,
00208:                 "journal_unlock/faction_info targets must resolve to authored prose keys:\n" + string.Join("\n", broken));
00209:         }
00210:
00211:         [Fact]
00212:         public void CodexTargets_DefaultAndRealistProse_AreTwoToFourSentences()
00213:         {
00214:             // The collectible dispatcher is the only live discovery path for
00215:             // these keys and passes author:null, which JournalSystem resolves
00216:             // to the Realist voice. Default covers the fallback. Both must
00217:             // satisfy the 2–4 sentence entry contract.
00218:             var prose = LoadProse();
00219:             var targets = LoadCorpus()
00220:                 .Where(c => c.EffectType == "journal_unlock" || c.EffectType == "faction_info")
00221:                 .Select(c => c.EffectTarget)
00222:                 .Distinct()
00223:                 .ToList();
00224:
00225:             var broken = new List<string>();
00226:             foreach (string target in targets)
00227:             {
00228:                 var entry = prose.GetEntry(target);
00229:                 if (entry == null) { broken.Add($"{target}: missing"); continue; }
00230:                 int realist = CountSentences(entry.GetProseForBias(RiskBiasTrait.Realist));
00231:                 int def = CountSentences(entry.@default);
00232:                 if (realist is < 2 or > 4) broken.Add($"{target}: realist={realist} sentences");
00233:                 if (def is < 2 or > 4) broken.Add($"{target}: default={def} sentences");
00234:             }
00235:             Assert.True(broken.Count == 0, "entry contract violations:\n" + string.Join("\n", broken));
00236:         }
00237:     }
00238: }
```

## `Ashfall.Core.Tests/MicroLocationIntegrationDeterminismTests.cs` — 311 lines; 16,305 bytes; SHA-256 `19704d50d1484d0abe1efda9cbe4b6b4bde8fdab83e2519dae08d619fca2f2ee`
Declaration index:
- 00031: public class MicroLocationIntegrationDeterminismTests
- 00036: private static string DataDir()
- 00044: private static NarrativeEncounterSystem CreateProductionNarrativeSystem()
- 00053: private static ItemCatalog LoadItemCatalog()
- 00061: private sealed class TraceBuilder
- 00065: public void Entry(string locationId, string choiceId, NarrativeEncounterResolutionResult? r,
- 00083: private string RunIntegrationPass(int seed, int dayBase)
- 00142: private sealed class TraceAuthor : ISurvivorAuthor
- 00150: public void Trace_TwoIndependentPasses_ByteIdentical()
- 00158: public void Trace_CoversAllFourFlagshipSites_WithExpectedEffects()
- 00170: public void Trace_SameSeedDifferentDayBase_StillDeterministicPerFixture()
- 00184: public void ContentValidation_FlagshipRewardItems_ResolveAndHaveDownstreamConsumers()
- 00208: public void ContentValidation_FlagshipJournalKeys_ResolveThroughCanonicalUnlock()
- 00221: public void ContentValidation_FlagshipJournalKeys_HaveAuthoredProse_NeverPlaceholder()
- 00241: public void ContentValidation_EveryAuthoredMicroFlag_HasRegisteredConsumerOrInertContract()
- 00276: public void ContentValidation_ProductionCatalogLoad_StampsMicroLocationMarker()
- 00302: public void ContentValidation_MicroLocations_CarrySchemaVersion()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text;
00007: using Ashfall.Core.Disease;
00008: using Ashfall.Core.Flags;
00009: using Ashfall.Core.IO;
00010: using Ashfall.Core.Journal;
00011: using Ashfall.Core.Narrative;
00012: using Xunit;
00013: namespace Ashfall.Core.Tests
00014: {
00015:     using Inventory = Ashfall.Core.Inventory.Inventory;
00016:     using ItemCatalog = Ashfall.Core.Inventory.ItemCatalog;
00017:     /// <summary>
00018:     /// F17–F20 flagship hardening — shared content validation and the
00019:     /// deterministic integration trace (flagship plan §11 + §13).
00020:     ///
00021:     /// §11 — cross-catalog validation: the four flagship reward items, the two
00022:     /// journal keys, and the one hazard flag must all be real, consumed
00023:     /// content ("valid but dead" fails here).
00024:     ///
00025:     /// §13 — determinism harness: one production-wiring fixture resolves the
00026:     /// flagship choice of each location and records a canonical trace line
00027:     /// (location | choice | items | flags | journal | hazard | discovery |
00028:     /// final resolution state). Two independent passes must serialize
00029:     /// byte-for-byte identically.
00030:     /// </summary>
00031:     public class MicroLocationIntegrationDeterminismTests
00032:     {
00033:         private const string ContaminationFlag = MicroLocationHazardRegistry.ContaminationExposureFlag;
00034:         private const string SurvivorId = "surv_trace";
00035:
00036:         private static string DataDir()
00037:         {
00038:             var dir = new DirectoryInfo(AppContext.BaseDirectory);
00039:             while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
00040:                 dir = dir.Parent!;
00041:             return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
00042:         }
00043:
00044:         private static NarrativeEncounterSystem CreateProductionNarrativeSystem()
00045:         {
00046:             var sys = new NarrativeEncounterSystem();
00047:             var defs = NarrativeEncounterCatalogLoader.Load(
00048:                 DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00049:             sys.RegisterRange(defs);
00050:             return sys;
00051:         }
00052:
00053:         private static ItemCatalog LoadItemCatalog()
00054:         {
00055:             return Ashfall.Core.Inventory.ItemCatalogLoader.LoadCatalog(
00056:                 DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00057:         }
00058:
00059:         // ── §13 — deterministic integration trace ──────────────────────
00060:
00061:         private sealed class TraceBuilder
00062:         {
00063:             public readonly StringBuilder Sb = new StringBuilder();
00064:
00065:             public void Entry(string locationId, string choiceId, NarrativeEncounterResolutionResult? r,
00066:                 string hazardEffect, NarrativeEncounterSystem sys)
00067:             {
00068:                 Sb.Append("loc=").Append(locationId)
00069:                   .Append("|choice=").Append(choiceId)
00070:                   .Append("|item=").Append(r?.GrantItemId ?? "none").Append('x').Append(r?.GrantItemQuantity ?? 0)
00071:                   .Append("|flag=").Append(string.IsNullOrEmpty(r?.SetWorldFlagId) ? "none" : r!.SetWorldFlagId)
00072:                   .Append("|journal=").Append(string.IsNullOrEmpty(r?.JournalUnlockId) ? "none" : r!.JournalUnlockId)
00073:                   .Append("|discovery=").Append(string.IsNullOrEmpty(r?.DiscoverLocationId) ? "none" : r!.DiscoverLocationId)
00074:                   .Append("|hazard=").Append(hazardEffect)
00075:                   .Append("|depleted=").Append(sys.IsDepleted(locationId) ? '1' : '0')
00076:                   .Append("|totalResolved=").Append(sys.TotalResolved)
00077:                   .Append('\n');
00078:             }
00079:         }
00080:
00081:         /// <summary>One full production pass over all four flagship sites in a
00082:         /// fixed order, with the same application order the host uses.</summary>
00083:         private string RunIntegrationPass(int seed, int dayBase)
00084:         {
00085:             var sys = CreateProductionNarrativeSystem();
00086:             var ledger = new CampaignConsequenceLedger();
00087:             var diseaseCatalog = DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00088:             var disease = new DiseaseSystem(rng: new SeededRng(seed));
00089:             disease.BindCatalog(diseaseCatalog);
00090:             var inventory = new Inventory();
00091:             var journal = new JournalSystem();
00092:             var trace = new TraceBuilder();
00093:
00094:             void ApplyFlagship(string encounterId, string choiceId, string locationId, int day)
00095:             {
00096:                 bool flagWasAlreadySet = ledger.IsSet(ContaminationFlag);
00097:                 var res = sys.TryResolve(encounterId, choiceId, locationId, day);
00098:                 Assert.NotNull(res);
00099:
00100:                 // item (canonical AddById grant — the host's loot path shape)
00101:                 if (!string.IsNullOrEmpty(res!.GrantItemId) && res.GrantItemQuantity > 0)
00102:                     inventory.AddById(res.GrantItemId, res.GrantItemQuantity);
00103:
00104:                 // journal (canonical dedup gate)
00105:                 string journalEffect = "none";
00106:                 if (!string.IsNullOrEmpty(res.JournalUnlockId))
00107:                     journalEffect = journal.TryDiscoverKnowledge(res.JournalUnlockId, new TraceAuthor(), day) != null
00108:                         ? "unlocked" : "dedup";
00109:
00110:                 // world flag + hazard routing (ledger verdict decides)
00111:                 string hazardEffect = "none";
00112:                 if (!string.IsNullOrEmpty(res.SetWorldFlagId))
00113:                 {
00114:                     EncounterChoiceEffectDispatcher.ApplyWorldFlag(res, ledger);
00115:                     var hazard = MicroLocationHazardRegistry.ApplyFlagHazard(
00116:                         res.SetWorldFlagId,
00117:                         flagWasAlreadySet: flagWasAlreadySet && res.SetWorldFlagId == ContaminationFlag,
00118:                         SurvivorId, day,
00119:                         (sid, did, d) => disease.Infect(sid, did, d));
00120:                     hazardEffect = hazard.Status.ToString();
00121:                 }
00122:
00123:                 trace.Entry(encounterId, choiceId, res, hazardEffect, sys);
00124:             }
00125:
00126:             ApplyFlagship("micro_dead_livestock", "scavenge_livestock", "loc_suburban_ruins", dayBase + 0);
00127:             ApplyFlagship("micro_ruined_greenhouse", "take_greenhouse_seeds", "loc_allotments", dayBase + 1);
00128:             ApplyFlagship("micro_radio_tower", "open_radio_cabinet", "loc_radio_hill", dayBase + 2);
00129:             ApplyFlagship("micro_water_source", "collect_water", "loc_old_farmstead", dayBase + 3);
00130:
00131:             trace.Sb.Append("inv=").Append(inventory.CountById("cloth")).Append(',')
00132:                       .Append(inventory.CountById("seed_packets")).Append(',')
00133:                       .Append(inventory.CountById("antenna_coil")).Append(',')
00134:                       .Append(inventory.CountById("clean_water"))
00135:                       .Append("|disease=").Append(disease.IsInfected(SurvivorId, MicroLocationHazardRegistry.DeadLivestockDiseaseId))
00136:                       .Append("|journalKeys=").Append(journal.Entries.Count)
00137:                       .Append("|depleted=").Append(sys.DepletedCount)
00138:                       .Append('\n');
00139:             return trace.Sb.ToString();
00140:         }
00141:
00142:         private sealed class TraceAuthor : ISurvivorAuthor
00143:         {
00144:             public string Id => SurvivorId;
00145:             public string DisplayName => "Trace";
00146:             public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
00147:         }
00148:
00149:         [Fact]
00150:         public void Trace_TwoIndependentPasses_ByteIdentical()
00151:         {
00152:             string passA = RunIntegrationPass(seed: 7071, dayBase: 10);
00153:             string passB = RunIntegrationPass(seed: 7071, dayBase: 10);
00154:             Assert.Equal(passA, passB, ignoreLineEndingDifferences: false, ignoreCase: false);
00155:         }
00156:
00157:         [Fact]
00158:         public void Trace_CoversAllFourFlagshipSites_WithExpectedEffects()
00159:         {
00160:             string pass = RunIntegrationPass(seed: 7071, dayBase: 10);
00161:             var lines = pass.Split('\n', StringSplitOptions.RemoveEmptyEntries);
00162:
00163:             Assert.Contains(lines, l => l.StartsWith("loc=micro_dead_livestock|choice=scavenge_livestock") && l.Contains("item=clothx2") && l.Contains("flag=micro_contamination_exposure") && l.Contains("hazard=Applied") && l.Contains("depleted=1"));
00164:             Assert.Contains(lines, l => l.StartsWith("loc=micro_ruined_greenhouse|choice=take_greenhouse_seeds") && l.Contains("item=seed_packetsx2") && l.Contains("hazard=none"));
00165:             Assert.Contains(lines, l => l.StartsWith("loc=micro_radio_tower|choice=open_radio_cabinet") && l.Contains("item=antenna_coilx1") && l.Contains("hazard=none"));
00166:             Assert.Contains(lines, l => l.StartsWith("loc=micro_water_source|choice=collect_water") && l.Contains("item=clean_waterx3") && l.Contains("hazard=none"));
00167:         }
00168:
00169:         [Fact]
00170:         public void Trace_SameSeedDifferentDayBase_StillDeterministicPerFixture()
00171:         {
00172:             // Different campaign days change the recorded day-dependent payload
00173:             // shape but never the grant/flag/hazard outcomes.
00174:             string pass1 = RunIntegrationPass(seed: 7071, dayBase: 10);
00175:             string pass2 = RunIntegrationPass(seed: 7071, dayBase: 99);
00176:             Assert.Equal(
00177:                 pass1.Split('\n').Count(l => l.Contains("hazard=Applied")),
00178:                 pass2.Split('\n').Count(l => l.Contains("hazard=Applied")));
00179:         }
00180:
00181:         // ── §11.1/§11.4 — no valid-but-dead flagship content ───────────
00182:
00183:         [Fact]
00184:         public void ContentValidation_FlagshipRewardItems_ResolveAndHaveDownstreamConsumers()
00185:         {
00186:             var catalog = LoadItemCatalog();
00187:
00188:             // §11.1 — all four reward ids are real catalog entries.
00189:             foreach (var id in new[] { "seed_packets", "crop_medicinal_herb", "antenna_coil", "clean_water" })
00190:                 Assert.True(catalog.Contains(id), $"flagship reward item '{id}' missing from the item catalog");
00191:
00192:             // §11.4 — each has a live downstream consumer:
00193:             // seed_packets → the canonical crop catalog (agriculture input).
00194:             Assert.NotNull(GreenhouseExpansionCatalog.CropCatalog.Get("seed_packets"));
00195:             // crop_medicinal_herb → authored clean yield of the herb seed line.
00196:             Assert.Equal("crop_medicinal_herb", GreenhouseExpansionCatalog.CropCatalog
00197:                 .Get(GreenhouseExpansionCatalog.Items.SeedMedicinalHerb)!.YieldCleanId);
00198:             // clean_water → canonical hydration consumption (thirstRestore > 0).
00199:             Assert.True(catalog.Get("clean_water")!.thirstRestore > 0f);
00200:             // antenna_coil → relic repair bill (asserted in depth by F19_03/05;
00201:             // the file-level reference check here guards the data authority).
00202:             string relicRaw = new FileSystemIO().ReadAllText(
00203:                 new FileSystemIO().Combine(DataDir(), "relic_recipes.json"));
00204:             Assert.Contains("antenna_coil", relicRaw, StringComparison.Ordinal);
00205:         }
00206:
00207:         [Fact]
00208:         public void ContentValidation_FlagshipJournalKeys_ResolveThroughCanonicalUnlock()
00209:         {
00210:             foreach (var key in new[] { "micro_radio_tower_log", "micro_dead_livestock_tags" })
00211:             {
00212:                 Assert.StartsWith("micro_", key, StringComparison.Ordinal);
00213:                 var journal = new JournalSystem();
00214:                 var entry = journal.TryDiscoverKnowledge(key, new TraceAuthor(), 5);
00215:                 Assert.NotNull(entry);
00216:                 Assert.Null(journal.TryDiscoverKnowledge(key, new TraceAuthor(), 6)); // exactly once
00217:             }
00218:         }
00219:
00220:         [Fact]
00221:         public void ContentValidation_FlagshipJournalKeys_HaveAuthoredProse_NeverPlaceholder()
00222:         {
00223:             // F19 §9.6 / F17 — the flagship unlocks carry authored per-bias prose
00224:             // (journal_voice_prose.json). The generic "Something changed."
00225:             // fallback is a content bug for these keys.
00226:             var catalog = JournalVoiceProseCatalogLoader.LoadDefault();
00227:             Assert.True(catalog.Count > 0, "journal_voice_prose.json must load");
00228:
00229:             foreach (var key in new[] { "micro_radio_tower_log", "micro_dead_livestock_tags" })
00230:             {
00231:                 foreach (RiskBiasTrait bias in Enum.GetValues<RiskBiasTrait>())
00232:                 {
00233:                     string text = catalog.GetProse(key, bias);
00234:                     Assert.NotEqual("Something changed. I wrote it down so I would not forget.", text);
00235:                     Assert.False(string.IsNullOrWhiteSpace(text));
00236:                 }
00237:             }
00238:         }
00239:
00240:         [Fact]
00241:         public void ContentValidation_EveryAuthoredMicroFlag_HasRegisteredConsumerOrInertContract()
00242:         {
00243:             // §11.3 — world-flag consumer integrity. Each flag authored by a
00244:             // micro-location must either (a) have a registered hazard consumer
00245:             // in MicroLocationHazardRegistry, or (b) be on the explicitly
00246:             // reviewed inert list (world-state markers consumed by quests /
00247:             // future integration — tracked here so nothing silently rots).
00248:             var inertReviewed = new HashSet<string>(StringComparer.Ordinal)
00249:             {
00250:                 "micro_generator_marked", // waystation generator marker — quest/condition marker (F5 suite)
00251:             };
00252:
00253:             var defs = NarrativeEncounterCatalogLoader.Load(
00254:                 DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00255:             var authored = new SortedSet<string>(StringComparer.Ordinal);
00256:             foreach (var def in defs)
00257:             {
00258:                 if (def?.choices == null) continue;
00259:                 foreach (var choice in def.choices)
00260:                 {
00261:                     if (choice == null || string.IsNullOrWhiteSpace(choice.setWorldFlag)) continue;
00262:                     authored.Add(choice.setWorldFlag.Trim());
00263:
00264:                     bool hasHazardConsumer = MicroLocationHazardRegistry.TryGetFlagDiseaseId(choice.setWorldFlag) != null;
00265:                     Assert.True(hasHazardConsumer || inertReviewed.Contains(choice.setWorldFlag),
00266:                         $"micro-location flag '{choice.setWorldFlag}' has no registered consumer and is not on the reviewed inert list");
00267:                 }
00268:             }
00269:
00270:             Assert.Contains(ContaminationFlag, authored);
00271:             Assert.Equal(MicroLocationHazardRegistry.DeadLivestockDiseaseId,
00272:                 MicroLocationHazardRegistry.TryGetFlagDiseaseId(ContaminationFlag));
00273:         }
00274:
00275:         [Fact]
00276:         public void ContentValidation_ProductionCatalogLoad_StampsMicroLocationMarker()
00277:         {
00278:             // F6 §6.3 seal — production loads micro_locations.json through the shared
00279:             // NarrativeEncounterCatalogLoader.LoadFile, which must apply the same
00280:             // isMicroLocation/sourceFile stamp the dedicated
00281:             // MicroLocationEncounterLoader applies. Previously only the dedicated
00282:             // (test-only) loader stamped; production definitions silently carried
00283:             // isMicroLocation = false.
00284:             var defs = CreateProductionNarrativeSystem().Catalog;
00285:
00286:             var microDefs = defs.Where(d => d.id.StartsWith("micro_", StringComparison.Ordinal)).ToList();
00287:             Assert.True(microDefs.Count >= 6, $"expected the authored micro-location set, found {microDefs.Count}");
00288:
00289:             foreach (var def in microDefs)
00290:             {
00291:                 Assert.True(def.isMicroLocation, $"production-loaded '{def.id}' must be stamped isMicroLocation");
00292:                 Assert.Equal("micro_locations.json", def.sourceFile);
00293:             }
00294:
00295:             // Non-micro files keep their defaults (no cross-file stamping).
00296:             var coreDefs = defs.Where(d => !d.id.StartsWith("micro_", StringComparison.Ordinal)).ToList();
00297:             Assert.True(coreDefs.Count > 0);
00298:             Assert.All(coreDefs, d => Assert.False(d.isMicroLocation));
00299:         }
00300:
00301:         [Fact]
00302:         public void ContentValidation_MicroLocations_CarrySchemaVersion()
00303:         {
00304:             // Data-authority hygiene (Invariant 6): the catalog the flagship
00305:             // hooks resolve from must keep its schema_version.
00306:             string raw = new FileSystemIO().ReadAllText(
00307:                 new FileSystemIO().Combine(DataDir(), "micro_locations.json"));
00308:             Assert.Contains("\"schema_version\"", raw, StringComparison.Ordinal);
00309:         }
00310:     }
00311: }
```

## `Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs` — 226 lines; 10,664 bytes; SHA-256 `f93ce88d3f5ad9b0efc9e5a7e0b800dcccb7cf68a8cf290deff4ae0c8423dee1`
Declaration index:
- 00022: public class CollectibleCodexUnlockLiveTests
- 00029: private static string FindDataDir()
- 00041: private static CollectibleCatalog LoadCatalog() =>
- 00045: private static List<CollectibleDefinition> CodexCollectibles(string effectType) =>
- 00053: private sealed class JournalCounters { public int Entries, Codex, Pings; }
- 00055: private static JournalSystem NewJournal(JournalCounters c)
- 00064: private static CollectibleEffectDispatcher NewDispatcher(
- 00079: public static IEnumerable<object[]> CodexEffectTypes()
- 00087: public void EveryLiveCodexCollectible_WritesAuthoredEntry_OnFirstAcquisition(string effectType)
- 00120: public void RepeatAcquisition_IsIdempotent(string effectType)
- 00140: public void CodexAlreadyKnowsKey_SecondDiscovery_RegistersWithoutDuplicateEntry()
- 00164: public void SaveRestore_PreservesUnlocks_WithoutReplayingNotifications()
- 00201: public void FactionInfoAcquisition_DoesNotMutateFactionStanding()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Journal;
00008: using Ashfall.Core.YearOfAsh;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests
00012: {
00013:     /// <summary>
00014:     /// Flagship XII — live-data codex unlock contract for every collectible
00015:     /// whose effect routes through the journal codex authority
00016:     /// (journal_unlock and faction_info). Targets are enumerated from
00017:     /// collectibles.json — no hardcoded ID list — and each is exercised for:
00018:     /// authored-entry acquisition, duplicate-acquisition idempotency,
00019:     /// save/restore preservation without notification replay, and (for
00020:     /// faction_info) strict standing isolation against FactionWarSystem.
00021:     /// </summary>
00022:     public class CollectibleCodexUnlockLiveTests
00023:     {
00024:         private static readonly string DataDir = FindDataDir();
00025:         private static readonly IFileIO FileIO = new FileSystemIO();
00026:         private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();
00027:         private const int FixtureDay = 14;
00028:
00029:         private static string FindDataDir()
00030:         {
00031:             string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
00032:             while (dir != null)
00033:             {
00034:                 string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "collectibles.json");
00035:                 if (File.Exists(probe)) return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
00036:                 dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
00037:             }
00038:             throw new DirectoryNotFoundException("data authority not found");
00039:         }
00040:
00041:         private static CollectibleCatalog LoadCatalog() =>
00042:             CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer)
00043:                 ?? throw new InvalidOperationException("collectibles.json must load");
00044:
00045:         private static List<CollectibleDefinition> CodexCollectibles(string effectType) =>
00046:             LoadCatalog().ByItemId.Values
00047:                 .Where(d => d.effect_type == effectType)
00048:                 .OrderBy(d => d.item_id, StringComparer.Ordinal)
00049:                 .ToList();
00050:
00051:         /// <summary>Event counters shared by reference so handlers can mutate
00052:         /// them after this helper returns.</summary>
00053:         private sealed class JournalCounters { public int Entries, Codex, Pings; }
00054:
00055:         private static JournalSystem NewJournal(JournalCounters c)
00056:         {
00057:             var journal = new JournalSystem();
00058:             journal.OnEntryAdded += _ => c.Entries++;
00059:             journal.OnCodexUnlocked += _ => c.Codex++;
00060:             journal.OnNotificationPing += _ => c.Pings++;
00061:             return journal;
00062:         }
00063:
00064:         private static CollectibleEffectDispatcher NewDispatcher(
00065:             CollectibleCatalog catalog, CollectibleDiscoveryState discovery, JournalSystem journal)
00066:         {
00067:             JournalVoice.BindCatalog(new JournalVoiceProseCatalogLoader(FileIO, Serializer).Load(DataDir));
00068:             return new CollectibleEffectDispatcher(
00069:                 catalog, discovery,
00070:                 needsProvider: () => null,
00071:                 researchProvider: () => null,
00072:                 journalProvider: () => journal,
00073:                 mapProvider: () => null,
00074:                 dayProvider: () => FixtureDay);
00075:         }
00076:
00077:         // ── Acquisition: every live codex collectible writes authored content ──
00078:
00079:         public static IEnumerable<object[]> CodexEffectTypes()
00080:         {
00081:             yield return new object[] { "journal_unlock" };
00082:             yield return new object[] { "faction_info" };
00083:         }
00084:
00085:         [Theory]
00086:         [MemberData(nameof(CodexEffectTypes))]
00087:         public void EveryLiveCodexCollectible_WritesAuthoredEntry_OnFirstAcquisition(string effectType)
00088:         {
00089:             var items = CodexCollectibles(effectType);
00090:             Assert.True(items.Count > 0, $"live data must contain {effectType} collectibles");
00091:
00092:             foreach (var def in items)
00093:             {
00094:                 var counters = new JournalCounters();
00095:                 var journal = NewJournal(counters);
00096:                 var dispatcher = NewDispatcher(LoadCatalog(), new CollectibleDiscoveryState(), journal);
00097:
00098:                 var result = dispatcher.DispatchOnAcquire(def.item_id);
00099:
00100:                 Assert.True(result.EffectApplied, $"{def.item_id}: effect must apply ({result.FailureReason})");
00101:                 Assert.True(result.DiscoveryRegistered, $"{def.item_id}: discovery must register");
00102:                 Assert.Equal(1, journal.Knowledge.Has(def.effect_target) ? 1 : 0);
00103:                 Assert.Equal(1, journal.EntryCount);
00104:                 Assert.Equal(1, counters.Codex);
00105:                 Assert.Equal(1, counters.Entries);
00106:                 Assert.Equal(1, counters.Pings);
00107:
00108:                 var entry = journal.Entries.Single(e => e.KnowledgeKey == def.effect_target);
00109:                 string expected = JournalVoice.ComposeFullText(def.effect_target, RiskBiasTrait.Realist, FixtureDay);
00110:                 Assert.Equal(expected, entry.Text);
00111:                 Assert.StartsWith("Day ", entry.Text);
00112:                 Assert.DoesNotContain("Something changed. I wrote it down", entry.Text); // no placeholder fallback
00113:             }
00114:         }
00115:
00116:         // ── Duplicate acquisition: idempotent, one entry, no replay ──
00117:
00118:         [Theory]
00119:         [MemberData(nameof(CodexEffectTypes))]
00120:         public void RepeatAcquisition_IsIdempotent(string effectType)
00121:         {
00122:             foreach (var def in CodexCollectibles(effectType))
00123:             {
00124:                 var counters = new JournalCounters();
00125:                 var journal = NewJournal(counters);
00126:                 var discovery = new CollectibleDiscoveryState();
00127:                 var dispatcher = NewDispatcher(LoadCatalog(), discovery, journal);
00128:
00129:                 dispatcher.DispatchOnAcquire(def.item_id);
00130:                 var second = dispatcher.DispatchOnAcquire(def.item_id);
00131:
00132:                 Assert.True(second.AlreadyDiscovered);
00133:                 Assert.False(second.EffectApplied);
00134:                 Assert.Equal(1, journal.EntryCount);
00135:                 Assert.Equal(1, counters.Codex);
00136:             }
00137:         }
00138:
00139:         [Fact]
00140:         public void CodexAlreadyKnowsKey_SecondDiscovery_RegistersWithoutDuplicateEntry()
00141:         {
00142:             // A second acquisition path (e.g. another collectible instance or a
00143:             // post-restore re-acquire with a fresh discovery ledger) must not
00144:             // duplicate the entry, and the dispatch must still count as handled.
00145:             foreach (var def in CodexCollectibles("journal_unlock").Concat(CodexCollectibles("faction_info")))
00146:             {
00147:                 var journal = NewJournal(new JournalCounters());
00148:                 var first = NewDispatcher(LoadCatalog(), new CollectibleDiscoveryState(), journal);
00149:                 first.DispatchOnAcquire(def.item_id);
00150:                 int entriesBefore = journal.EntryCount;
00151:
00152:                 var second = NewDispatcher(LoadCatalog(), new CollectibleDiscoveryState(), journal);
00153:                 var result = second.DispatchOnAcquire(def.item_id);
00154:
00155:                 Assert.True(result.EffectApplied, $"{def.item_id}: unlock content exists so discovery counts as handled");
00156:                 Assert.True(result.DiscoveryRegistered);
00157:                 Assert.Equal(entriesBefore, journal.EntryCount);
00158:             }
00159:         }
00160:
00161:         // ── Save/restore: unlocks persist, notifications never replay ──
00162:
00163:         [Fact]
00164:         public void SaveRestore_PreservesUnlocks_WithoutReplayingNotifications()
00165:         {
00166:             var journal = NewJournal(new JournalCounters());
00167:             var discovery = new CollectibleDiscoveryState();
00168:             var dispatcher = NewDispatcher(LoadCatalog(), discovery, journal);
00169:
00170:             var all = CodexCollectibles("journal_unlock").Concat(CodexCollectibles("faction_info")).ToList();
00171:             foreach (var def in all)
00172:                 dispatcher.DispatchOnAcquire(def.item_id);
00173:             Assert.Equal(all.Count, journal.EntryCount);
00174:
00175:             var save = journal.CaptureState();
00176:             var restoredCounters = new JournalCounters();
00177:             var restored = NewJournal(restoredCounters);
00178:             restored.RestoreState(save);
00179:
00180:             Assert.Equal(0, restoredCounters.Entries); // restore reconstructs; it never notifies
00181:             Assert.Equal(0, restoredCounters.Codex);
00182:             Assert.Equal(0, restoredCounters.Pings);
00183:             foreach (var def in all)
00184:                 Assert.True(restored.Knowledge.Has(def.effect_target), $"{def.effect_target} must survive restore");
00185:
00186:             // A dispatcher backed by the restored discovery ledger must treat
00187:             // re-acquisition as already-discovered: no replay, no new entries.
00188:             var redispatch = NewDispatcher(LoadCatalog(), discovery, restored);
00189:             foreach (var def in all)
00190:             {
00191:                 var result = redispatch.DispatchOnAcquire(def.item_id);
00192:                 Assert.True(result.AlreadyDiscovered, $"{def.item_id}: must be already discovered after restore");
00193:                 Assert.False(result.EffectApplied);
00194:             }
00195:             Assert.Equal(all.Count, restored.EntryCount);
00196:         }
00197:
00198:         // ── Standing isolation: faction_info is informational only ──
00199:
00200:         [Fact]
00201:         public void FactionInfoAcquisition_DoesNotMutateFactionStanding()
00202:         {
00203:             var journal = NewJournal(new JournalCounters());
00204:             var factionWar = new FactionWarSystem();
00205:             factionWar.ModifyStanding("faction_rebuilders", 20); // unrelated faction with real standing
00206:
00207:             var before = factionWar.State.factions.ToDictionary(f => f.factionId, f => f.standing);
00208:             int standingEvents = 0;
00209:             factionWar.OnFactionStandingChanged += (_, _) => standingEvents++;
00210:
00211:             var dispatcher = NewDispatcher(LoadCatalog(), new CollectibleDiscoveryState(), journal);
00212:             foreach (var def in CodexCollectibles("faction_info"))
00213:                 dispatcher.DispatchOnAcquire(def.item_id);
00214:
00215:             var after = factionWar.State.factions.ToDictionary(f => f.factionId, f => f.standing);
00216:             Assert.Equal(before, after); // no faction created, modified, or removed
00217:             Assert.Equal(20, factionWar.GetStanding("faction_rebuilders"));
00218:             Assert.Equal(0, standingEvents);
00219:
00220:             // The codex knowledge itself did land — isolation is about
00221:             // diplomacy, not about swallowing the unlock.
00222:             foreach (var def in CodexCollectibles("faction_info"))
00223:                 Assert.True(journal.Knowledge.Has(def.effect_target));
00224:         }
00225:     }
00226: }
```

## `src/Journal/JournalSelfTest.cs` — 228 lines; 12,076 bytes; SHA-256 `ce4c5c32c80278be6bd03f50dcd52d1d87b49478fa2f8d4463c45c1f7dbc3c70`
Declaration index:
- 00017: public static class JournalSelfTest
- 00019: public static int Run(JournalCatalogs catalogs)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Godot;
00007: using Ashfall.Core.Journal;
00008: using Ashfall.Core.Narrative;
00009:
00010: namespace AtomicWar.Journal
00011: {
00012:     /// <summary>
00013:     /// Headless self-test for the journal domain + save roundtrip. Runs when
00014:     /// the game is launched with `--journal-selftest` (after `--`). Uses only
00015:     /// ids that exist in the StreamingAssets catalogs.
00016:     /// </summary>
00017:     public static class JournalSelfTest
00018:     {
00019:         public static int Run(JournalCatalogs catalogs)
00020:         {
00021:             int passed = 0;
00022:             int total = 0;
00023:
00024:             void Check(bool condition, string name)
00025:             {
00026:                 total++;
00027:                 if (condition)
00028:                 {
00029:                     passed++;
00030:                     GD.Print($"  [PASS] {name}");
00031:                 }
00032:                 else
00033:                 {
00034:                     GD.Print($"  [FAIL] {name}");
00035:                 }
00036:             }
00037:
00038:             GD.Print("[JournalSelfTest] begin");
00039:
00040:             // Plan 153: the source catalog and its Plan 135 projections must
00041:             // both be available in the exported host. Discovery uses the
00042:             // Journal knowledge ledger only; no doctrine authority is bound.
00043:             Check(catalogs != null && catalogs.FringeCults != null
00044:                 && catalogs.FringeCults.TotalCount == 30,
00045:                 "fringe cult source catalog loads 30 records");
00046:             int fringeProjectionCount = catalogs?.NarrativeDiscoveries?.AllRecords
00047:                 .Count(r => FringeCultRuntimeContract.IsSourceCatalog(r.SourceCatalog)) ?? 0;
00048:             Check(fringeProjectionCount == 30, "fringe cult discovery projections load");
00049:
00050:             // Plan 156: both source catalogs remain intact while the shared
00051:             // discovery seam exposes one read-only projection per authored
00052:             // record. Measurements are rendered as historical observations.
00053:             Check(catalogs != null && catalogs.PaperMaking != null
00054:                 && catalogs.PaperMaking.TotalCount == 30,
00055:                 "paper-making source catalog loads 30 records");
00056:             Check(catalogs != null && catalogs.PaperPrinting != null
00057:                 && catalogs.PaperPrinting.TotalCount == 30,
00058:                 "paper-printing source catalog loads 30 records");
00059:             int paperProjectionCount = catalogs?.NarrativeDiscoveries?.AllRecords
00060:                 .Count(r => PaperPrintRuntimeContract.IsSourceCatalog(r.SourceCatalog)) ?? 0;
00061:             Check(paperProjectionCount == 60, "paper/printing discovery projections load 60 records");
00062:
00063:             // Plan 160: bone, horn and antler records use the same read-only
00064:             // discovery projection. Their source labels and measurements are
00065:             // historical observations; no wildlife, inventory or crafting
00066:             // authority is bound here.
00067:             Check(catalogs != null && catalogs.BoneHornCarving != null
00068:                 && catalogs.BoneHornCarving.TotalCount == 30,
00069:                 "bone/horn source catalog loads 30 records");
00070:             int boneHornProjectionCount = catalogs?.NarrativeDiscoveries?.AllRecords
00071:                 .Count(r => BoneHornRuntimeContract.IsSourceCatalog(r.SourceCatalog)) ?? 0;
00072:             Check(boneHornProjectionCount == 30, "bone/horn discovery projections load 30 records");
00073:
00074:             // Plan 17: the daily-survival archive (quiet-hour journals, botanical
00075:             // logs, children's folklore + batch 2, ration fraud) loads as read-only
00076:             // codex text through the same IFileIO catalog path.
00077:             Check(catalogs != null && catalogs.DailySurvival != null
00078:                 && catalogs.DailySurvival.TotalCount == 52,
00079:                 "daily-survival archive loads 52 records");
00080:
00081:             // --- KnowledgeBase dedupe ---
00082:             var kb = new KnowledgeBase();
00083:             bool first = kb.Discover(KnowledgeKeys.HighCo2);
00084:             bool second = kb.Discover(KnowledgeKeys.HighCo2);
00085:             Check(first && !second && kb.Count == 1, "knowledge dedupe");
00086:
00087:             // --- Entry dedupe + flags ---
00088:             var sys = new JournalSystem();
00089:             var author = new DemoSurvivor("elena_vasquez", "Elena Vasquez", RiskBiasTrait.Realist);
00090:             var e1 = sys.TryAddRawEntry("item_seen_dosimeter", "Found one. It still ticks.", author, 2);
00091:             var e2 = sys.TryAddRawEntry("item_seen_dosimeter", "Second copy must not land.", author, 2);
00092:             Check(e1 != null && e2 == null && sys.EntryCount == 1, "entry dedupe by knowledge key");
00093:             Check(sys.HasUnread && sys.NotificationPing && sys.NotificationPingCount == 1, "unread/ping flags after entry");
00094:             Check(e1 != null && e1.AuthorName == "Elena Vasquez" && e1.Day == 2, "entry author + day");
00095:
00096:             // --- JournalVoice shape ---
00097:             string voice = JournalVoice.ComposeFullText(KnowledgeKeys.HighCo2, RiskBiasTrait.Paranoid, 3);
00098:             Check(!string.IsNullOrEmpty(voice) && voice.StartsWith("Day 3."), "voice text shape");
00099:
00100:             // --- JournalVoice prose binding (F17/F19 follow-up) ---
00101:             // Production binds the catalog in Main.SetupJournal; this gate proves
00102:             // the binding contract end-to-end: the data file loads, and the
00103:             // flagship micro-location keys render authored prose instead of the
00104:             // generic "Something changed." placeholder.
00105:             var proseCatalog = JournalVoiceProseCatalogLoader.LoadDefault();
00106:             Check(proseCatalog.Count > 0, "prose catalog loads from data authority");
00107:             JournalVoice.BindCatalog(proseCatalog);
00108:             const string Placeholder = "Something changed. I wrote it down so I would not forget.";
00109:             Check(JournalVoice.ComposeFullText("micro_radio_tower_log", RiskBiasTrait.Realist, 6) != Placeholder,
00110:                 "radio tower log prose is authored (not placeholder)");
00111:             Check(JournalVoice.ComposeFullText("micro_dead_livestock_tags", RiskBiasTrait.Fatalist, 6) != Placeholder,
00112:                 "livestock tags prose is authored (not placeholder)");
00113:
00114:             // --- Tab clamping + unread tracking ---
00115:             sys.SwitchTab(99);
00116:             Check(sys.ActiveTab == JournalSystem.TabCount - 1, "tab clamp on switch");
00117:             sys.SwitchTab(0);
00118:             sys.MarkTabViewed(0);
00119:             Check(!sys.HasUnread && !sys.NotificationPing, "MarkTabViewed clears log unread");
00120:             int itemsSeenAfterEntry = sys.GetLastSeenIndex(1);
00121:             Check(itemsSeenAfterEntry == -1, "fresh tab unseen index");
00122:
00123:             // --- Ring cap (uses only real catalog ids) ---
00124:             var realIds = new List<string>();
00125:             if (catalogs != null)
00126:             {
00127:                 foreach (var it in catalogs.Items)
00128:                     if (!string.IsNullOrEmpty(it.id)) realIds.Add(KnowledgeKeys.ItemSeen(it.id));
00129:                 foreach (var loc in catalogs.Locations)
00130:                     if (!string.IsNullOrEmpty(loc.id)) realIds.Add(KnowledgeKeys.LocationVisited(loc.id));
00131:                 foreach (var evt in catalogs.Events)
00132:                     if (!string.IsNullOrEmpty(evt.id)) realIds.Add(KnowledgeKeys.EventFired(evt.id));
00133:             }
00134:             var ringSys = new JournalSystem();
00135:             int pushed = 0;
00136:             for (int i = 0; i < realIds.Count && pushed < 70; i++)
00137:             {
00138:                 if (ringSys.TryAddRawEntry(realIds[i], $"Log {pushed}", author, 1) != null)
00139:                     pushed++;
00140:             }
00141:             int expected = Math.Min(70, Math.Min(64, pushed));
00142:             Check(ringSys.EntryCount == 64 && ringSys.EntryCount == expected, "ring caps at 64");
00143:
00144:             // --- Save/restore roundtrip ---
00145:             var seeded = new JournalSystem();
00146:             string? firstItemId = catalogs != null && catalogs.Items.Count > 0 ? catalogs.Items[0].id : null;
00147:             if (!string.IsNullOrEmpty(firstItemId))
00148:             {
00149:                 seeded.UnlockItemSeen(firstItemId);
00150:                 seeded.UnlockItemSeen(firstItemId); // idempotent
00151:                 seeded.TryDiscover(KnowledgeKeys.HasExperiencedStorm, author, 5);
00152:                 seeded.SwitchTab(2);
00153:                 seeded.SwitchTab(3);
00154:             }
00155:             int beforeEntries = seeded.EntryCount;
00156:             int beforeUnlocks = seeded.CodexUnlockCount;
00157:             int beforeTab = seeded.ActiveTab;
00158:             bool beforeUnread = seeded.HasUnread;
00159:
00160:             string tmpPath = Path.Combine(
00161:                 ProjectSettings.GlobalizePath("user://"), "journal_selftest.json");
00162:             JournalSaveStore.Save(seeded.CaptureState(), tmpPath);
00163:             var loaded = JournalSaveStore.Load(tmpPath);
00164:             Check(loaded != null, "save file loads");
00165:
00166:             var restored = new JournalSystem();
00167:             if (loaded != null)
00168:                 restored.RestoreState(loaded);
00169:             Check(restored.EntryCount == beforeEntries, "restore entry count");
00170:             Check(restored.CodexUnlockCount == beforeUnlocks && restored.CodexUnlockCount == 1, "restore codex unlocks (idempotent)");
00171:             Check(restored.ActiveTab == beforeTab && restored.ActiveTab == 3, "restore active tab");
00172:             Check(restored.HasUnread == beforeUnread, "restore unread flag");
00173:             Check(restored.IsItemSeen(firstItemId ?? string.Empty),
00174:                 "restore knowledge keys");
00175:             Check(restored.Entries.Count > 0 && restored.Entries[0].AuthorName == "Elena Vasquez",
00176:                 "restore entries (newest first)");
00177:             // Best-effort cleanup of the roundtrip temp file. A failure here must not
00178:             // fail the suite, but it must not be invisible either — a leaked temp file
00179:             // makes the next run's "save file loads" check misleading.
00180:             try
00181:             {
00182:                 File.Delete(tmpPath);
00183:             }
00184:             catch (Exception e)
00185:             {
00186:                 GD.PrintErr($"[JournalSelfTest] temp cleanup failed for {tmpPath}: {e.Message}");
00187:             }
00188:
00189:             // --- Codex rows: unlocked + locked ---
00190:             if (catalogs != null && catalogs.Items.Count > 1 && catalogs.Locations.Count > 0
00191:                 && !string.IsNullOrEmpty(firstItemId))
00192:             {
00193:                 var codexSys = new JournalSystem();
00194:                 var codex = new JournalCodex(codexSys, catalogs);
00195:                 codexSys.UnlockItemSeen(firstItemId);
00196:
00197:                 var itemRows = codex.BuildRows(JournalTab.Items);
00198:                 Check(itemRows.Count == catalogs.Items.Count, "items tab row count");
00199:                 bool foundUnlocked = false;
00200:                 bool foundLocked = false;
00201:                 for (int i = 0; i < itemRows.Count; i++)
00202:                 {
00203:                     var row = itemRows[i];
00204:                     if (row.IsLocked && !string.IsNullOrEmpty(row.DisplayName)) foundLocked = true;
00205:                     if (!row.IsLocked && !string.IsNullOrEmpty(row.Body)) foundUnlocked = true;
00206:                 }
00207:                 Check(foundUnlocked && foundLocked, "items tab unlocked + locked rows");
00208:                 Check(codexSys.UnlockItemSeen(firstItemId) == false, "codex unlock idempotent");
00209:
00210:                 var placeRows = codex.BuildRows(JournalTab.Places);
00211:                 bool placeLocked = false;
00212:                 for (int i = 0; i < placeRows.Count; i++)
00213:                     if (placeRows[i].IsLocked) placeLocked = true;
00214:                 // Plan 29 29A: the Places tab also carries shelter room-history
00215:                 // vignette rows (locked until room_history_seen_* unlocks).
00216:                 int expectedPlaceRows = catalogs.Locations.Count + catalogs.RoomHistories.Count;
00217:                 Check(placeRows.Count == expectedPlaceRows && placeLocked, "places tab locked silhouettes");
00218:             }
00219:             else
00220:             {
00221:                 Check(false, "catalogs populated for codex checks");
00222:             }
00223:
00224:             bool ok = passed == total && total > 0;
00225:             return AtomicWar.GodotApp.HostCli.EmitSummary("journal_selftest", ok, ok ? 0 : 1, passed, total - passed);
00226:         }
00227:     }
00228: }
```

## `Ashfall.Core.Tests/MusterContentCatalogTests.cs` — 256 lines; 11,134 bytes; SHA-256 `b5b84e2035989b83326764f6f26770954680c93dfd08246d75120834968de007`
Declaration index:
- 00011: public class MusterContentCatalogTests : CatalogTestBase
- 00018: private static void EnsureJournalVoiceBound()
- 00030: private static string FindDataDir()
- 00046: public void WitnessCatalog_LoadsTheFoundingAccounts()
- 00070: public void EpilogueMatrix_LoadsAllEightOutcomes()
- 00096: public void EpilogueMatrix_EveryResolvableEndingKeyHasProse()
- 00122: public void JournalVoice_MusterWitnessKeysAreBiasWeighted()
- 00159: public void WitnessFraming_IsKeyedToTheAuthorNotTheWitness()
- 00176: public void WitnessFraming_SociopathRecordsTransactions()
- 00185: public void MusterSystem_EndingKeyForAny_DetectsResolvedMatrixKey()
- 00195: public void RadioCatalog_HarvenDecreeBroadcastParses()
- 00214: private class RadioContainer
- 00219: private class RadioEntry
- 00229: public void QuestCatalog_MusterQuestlinesHaveRealStages()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.IO;
00003: using Ashfall.Core;
00004: using Ashfall.Core.Journal;
00005: using Ashfall.Core.Muster;
00006: using Ashfall.Core.YearOfAsh;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     public class MusterContentCatalogTests : CatalogTestBase
00012:     {
00013:         public MusterContentCatalogTests()
00014:         {
00015:             EnsureJournalVoiceBound();
00016:         }
00017:
00018:         private static void EnsureJournalVoiceBound()
00019:         {
00020:             if (JournalVoice.GetCatalog() != null) return;
00021:             string dataDir = FindDataDir();
00022:             if (!string.IsNullOrEmpty(dataDir))
00023:             {
00024:                 var loader = new JournalVoiceProseCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
00025:                 var catalog = loader.Load(dataDir);
00026:                 JournalVoice.BindCatalog(catalog);
00027:             }
00028:         }
00029:
00030:         private static string FindDataDir()
00031:         {
00032:             string dataDir = string.Empty;
00033:             string search = Directory.GetCurrentDirectory();
00034:             for (int i = 0; i < 6; i++)
00035:             {
00036:                 string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
00037:                 if (Directory.Exists(candidate)) { dataDir = candidate; break; }
00038:                 string parent = Directory.GetParent(search)?.FullName;
00039:                 if (parent == null) break;
00040:                 search = parent;
00041:             }
00042:             return dataDir;
00043:         }
00044:
00045:         [Fact]
00046:         public void WitnessCatalog_LoadsTheFoundingAccounts()
00047:         {
00048:             string dataDir = FindDataDir();
00049:             if (string.IsNullOrEmpty(dataDir)) return;
00050:
00051:             var witnesses = WitnessCatalogLoader.LoadWitnesses(
00052:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00053:             // Plan 25 expands the roster (15+); the three founding accounts must
00054:             // survive any expansion, and every entry must carry testimony.
00055:             Assert.True(witnesses.Count >= 3, $"Expected >= 3 witnesses, got {witnesses.Count}");
00056:             Assert.Contains(witnesses, w => w.id == "witness_1_checkpoint_conscript");
00057:             Assert.Contains(witnesses, w => w.id == "witness_2_quartermaster_paperwork");
00058:             Assert.Contains(witnesses, w => w.id == "witness_3_signals_intercept");
00059:             foreach (var w in witnesses)
00060:             {
00061:                 Assert.False(string.IsNullOrEmpty(w.knowledgeKey));
00062:                 Assert.False(string.IsNullOrEmpty(w.locationId));
00063:                 Assert.False(string.IsNullOrEmpty(w.body));
00064:                 Assert.NotEmpty(w.testimonies);
00065:                 Assert.False(string.IsNullOrEmpty(w.testimonies[0].body));
00066:             }
00067:         }
00068:
00069:         [Fact]
00070:         public void EpilogueMatrix_LoadsAllEightOutcomes()
00071:         {
00072:             string dataDir = FindDataDir();
00073:             if (string.IsNullOrEmpty(dataDir)) return;
00074:
00075:             var epilogues = EpilogueMatrixLoader.LoadEpilogues(
00076:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00077:             Assert.True(epilogues.Count >= 8, $"Expected >= 8 endings, got {epilogues.Count}");
00078:             var keys = new System.Collections.Generic.HashSet<string>();
00079:             foreach (var e in epilogues)
00080:             {
00081:                 keys.Add(e.endingKey);
00082:                 Assert.False(string.IsNullOrEmpty(e.title));
00083:                 Assert.False(string.IsNullOrEmpty(e.prose));
00084:             }
00085:             Assert.Contains("the_open_muster", keys);
00086:             Assert.Contains("the_amnesty", keys);
00087:             Assert.Contains("the_corridor", keys);
00088:             Assert.Contains("the_blood_price", keys);
00089:             Assert.Contains("the_rate_card_revised", keys);
00090:             Assert.Contains("the_administrator", keys);
00091:             Assert.Contains("the_measured_truth_contested", keys);
00092:             Assert.Contains("unwritten", keys);
00093:         }
00094:
00095:         [Fact]
00096:         public void EpilogueMatrix_EveryResolvableEndingKeyHasProse()
00097:         {
00098:             string dataDir = FindDataDir();
00099:             if (string.IsNullOrEmpty(dataDir)) return;
00100:
00101:             var epilogues = EpilogueMatrixLoader.LoadEpilogues(
00102:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00103:             var byKey = new System.Collections.Generic.Dictionary<string, EndingDefinition>();
00104:             foreach (var e in epilogues) byKey[e.endingKey] = e;
00105:
00106:             // Every approach in the founding catalog must resolve to a key
00107:             // the matrix can prose (unless the questline is deliberately
00108:             // outside the matrix, e.g. long walk / guild mid-game questlines).
00109:             var sys = new MusterSystem();
00110:             foreach (var def in sys.Catalog)
00111:             {
00112:                 foreach (var a in def.approaches)
00113:                 {
00114:                     if (string.IsNullOrEmpty(a.endingKey)) continue;
00115:                     Assert.True(byKey.ContainsKey(a.endingKey),
00116:                         $"approach {a.approach} of {def.questlineId} resolves to missing matrix key '{a.endingKey}'");
00117:                 }
00118:             }
00119:         }
00120:
00121:         [Fact]
00122:         public void JournalVoice_MusterWitnessKeysAreBiasWeighted()
00123:         {
00124:             var paranoid = JournalVoice.ComposeBody(
00125:                 KnowledgeKeys.CheckpointConscriptsConfession, RiskBiasTrait.Paranoid);
00126:             var denialist = JournalVoice.ComposeBody(
00127:                 KnowledgeKeys.CheckpointConscriptsConfession, RiskBiasTrait.Denialist);
00128:             Assert.NotEqual(paranoid, denialist);
00129:             Assert.Contains("own staff", paranoid);
00130:             Assert.Contains("drunk boy", denialist);
00131:
00132:             // Empath does not write the dark accounts down.
00133:             var empath = JournalVoice.ComposeBody(
00134:                 KnowledgeKeys.InterceptedCipher, RiskBiasTrait.Empath);
00135:             Assert.DoesNotContain("cipher", empath);
00136:
00137:             // All nine keys resolve to non-default text.
00138:             string[] keys =
00139:             {
00140:                 KnowledgeKeys.ContinuityReclamationDecree,
00141:                 KnowledgeKeys.HydroBaronRateCardOrigin,
00142:                 KnowledgeKeys.DeserterCoalitionFounding,
00143:                 KnowledgeKeys.ColdCountBeforeTheLab,
00144:                 KnowledgeKeys.ProvisionedAdvanceKnowledge,
00145:                 KnowledgeKeys.CheckpointConscriptsConfession,
00146:                 KnowledgeKeys.QuartermastersPaperwork,
00147:                 KnowledgeKeys.InterceptedCipher,
00148:                 KnowledgeKeys.LedgerNobodySigned
00149:             };
00150:             foreach (var k in keys)
00151:             {
00152:                 var text = JournalVoice.ComposeBody(k, RiskBiasTrait.Realist);
00153:                 Assert.NotEqual("Something changed. I wrote it down so I would not forget.", text);
00154:                 Assert.False(string.IsNullOrEmpty(text));
00155:             }
00156:         }
00157:
00158:         [Fact]
00159:         public void WitnessFraming_IsKeyedToTheAuthorNotTheWitness()
00160:         {
00161:             // Section III: the same account reads differently in a different
00162:             // hand — a Paranoid leans into the assassination reading even for
00163:             // the quartermaster's paperwork; a Denialist downplays it.
00164:             string key = KnowledgeKeys.QuartermastersPaperwork;
00165:             string paranoid = JournalVoice.ComposeBody(key, RiskBiasTrait.Paranoid);
00166:             string denialist = JournalVoice.ComposeBody(key, RiskBiasTrait.Denialist);
00167:             string realist = JournalVoice.ComposeBody(key, RiskBiasTrait.Realist);
00168:             Assert.Contains("bury something", paranoid);
00169:             Assert.Contains("drunk kid", denialist);
00170:             Assert.Contains("Plausible, ordinary", realist);
00171:             Assert.NotEqual(paranoid, denialist);
00172:             Assert.NotEqual(denialist, realist);
00173:         }
00174:
00175:         [Fact]
00176:         public void WitnessFraming_SociopathRecordsTransactions()
00177:         {
00178:             string key = KnowledgeKeys.CheckpointConscriptsConfession;
00179:             string sociopath = JournalVoice.ComposeBody(key, RiskBiasTrait.Sociopath);
00180:             Assert.Contains("Source at the checkpoint", sociopath);
00181:             Assert.Contains("Unverified", sociopath);
00182:         }
00183:
00184:         [Fact]
00185:         public void MusterSystem_EndingKeyForAny_DetectsResolvedMatrixKey()
00186:         {
00187:             var sys = new MusterSystem();
00188:             Assert.False(sys.EndingKeyForAny("the_corridor"));
00189:             sys.SelectApproach(QuestApproach.C);
00190:             Assert.True(sys.EndingKeyForAny("the_corridor"));
00191:             Assert.False(sys.EndingKeyForAny("the_amnesty"));
00192:         }
00193:
00194:         [Fact]
00195:         public void RadioCatalog_HarvenDecreeBroadcastParses()
00196:         {
00197:             string dataDir = FindDataDir();
00198:             if (string.IsNullOrEmpty(dataDir)) return;
00199:
00200:             var fileIO = new FileSystemIO();
00201:             var json = new SystemTextJsonSerializer();
00202:             string raw = fileIO.ReadAllText(fileIO.Combine(dataDir, "year_of_ash_radio.json"));
00203:             Assert.False(string.IsNullOrWhiteSpace(raw));
00204:             var container = json.Deserialize<RadioContainer>(raw);
00205:             Assert.NotNull(container);
00206:             Assert.True(container.broadcasts.Count >= 37);
00207:             var decree = container.broadcasts.Find(b => b.id == "radio_harven_succession_decree");
00208:             Assert.NotNull(decree);
00209:             Assert.Equal(240, decree.dayTrigger);
00210:             Assert.False(string.IsNullOrEmpty(decree.message));
00211:             Assert.Contains("Colonel Harven", decree.message);
00212:         }
00213:
00214:         private class RadioContainer
00215:         {
00216:             public System.Collections.Generic.List<RadioEntry> broadcasts = new System.Collections.Generic.List<RadioEntry>();
00217:         }
00218:
00219:         private class RadioEntry
00220:         {
00221:             public string id = string.Empty;
00222:             public string frequency = string.Empty;
00223:             public int dayTrigger = default;
00224:             public string source = string.Empty;
00225:             public string message = string.Empty;
00226:         }
00227:
00228:         [Fact]
00229:         public void QuestCatalog_MusterQuestlinesHaveRealStages()
00230:         {
00231:             string dataDir = FindDataDir();
00232:             if (string.IsNullOrEmpty(dataDir)) return;
00233:
00234:             var fileIO = new FileSystemIO();
00235:             var json = new SystemTextJsonSerializer();
00236:             var questSystem = new QuestlineSystem();
00237:             int loaded = YearOfAshCatalogLoader.LoadAndRegisterQuests(questSystem, dataDir, fileIO, json);
00238:             Assert.True(loaded >= 32, $"Expected >= 32 external quests, got {loaded}");
00239:
00240:             string[] musterQuests =
00241:             {
00242:                 "quest_the_muster_uprising", "quest_the_rate_card_war", "quest_the_unsigned_order",
00243:                 "quest_four_names_on_the_roster", "quest_the_second_winter",
00244:                 "quest_the_eleven_month_circuit", "quest_the_second_color_ledger", "quest_nothing_to_offer"
00245:             };
00246:             foreach (var qid in musterQuests)
00247:             {
00248:                 var def = questSystem.FindDefinition(qid);
00249:                 Assert.NotNull(def);
00250:                 Assert.True(def.stages.Count >= 3, $"{qid} has {def.stages.Count} stages");
00251:                 foreach (var s in def.stages)
00252:                     Assert.False(string.IsNullOrEmpty(s.narrativePrompt));
00253:             }
00254:         }
00255:     }
00256: }
```

## `Ashfall.Core.Tests/MicroLocationRadioIntegrationTests.cs` — 290 lines; 13,939 bytes; SHA-256 `4cf6e2f3dd223c9aaa0c20353f508daf55e8a80fe3d14d92aad2f4f4d7195c89`
Declaration index:
- 00032: public class MicroLocationRadioIntegrationTests
- 00042: private static string DataDir()
- 00050: private static NarrativeEncounterSystem CreateProductionNarrativeSystem()
- 00059: private static RelicCatalog LoadRelicCatalog()
- 00079: private sealed class TestAuthor : ISurvivorAuthor
- 00089: public void F19_01_OpenRadioCabinet_GrantsExactlyOneCoil_DepletesSite()
- 00102: public void F19_02_CoilGrant_IsOneShot_AcrossSaveReload()
- 00123: public void F19_03_Coil_IsAuthoredInputOfRadioRelicRepairs()
- 00136: public void F19_04_CoilAlone_CannotCompleteRadioRepair_ProgressionGatesHold()
- 00148: public void F19_05_Coil_IsConsumedByCanonicalRadioRepair_BillAtomic()
- 00168: public void F19_06_CoilGrant_PlusRepair_EqualsVanillaEconomy_NoSourceBias()
- 00190: public void F19_07_ReadRadioLog_UnlocksJournalEntry_ExactlyOnce()
- 00210: public void F19_08_RadioLogChoice_IsNonDepleting_SiteStaysLiveForCoil()
- 00224: public void F19_09_JournalUnlockKey_StayedInMicroNamespace_EntryRecorded()
- 00241: public void F19_10_IgnoreChoice_NeutralPath()
- 00254: public void F19_11_GrantedCoil_SurvivesSaveReload_AsUsableInventory()
- 00285: internal static class NarrativeResolutionTestExtensions
- 00287: public static bool DepletesOnResolveFlag(this NarrativeEncounterResolutionResult r)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Crafting;
00008: using Ashfall.Core.IO;
00009: using Ashfall.Core.Journal;
00010: using Ashfall.Core.Narrative;
00011: using Xunit;
00012:
00013: namespace Ashfall.Core.Tests
00014: {
00015:     using Inventory = Ashfall.Core.Inventory.Inventory;
00016:     /// <summary>
00017:     /// F19 flagship integration — the radio tower's authored rewards are real
00018:     /// radio-progression inputs, not dead loot.
00019:     ///
00020:     ///   micro_radio_tower / open_radio_cabinet → 1 × antenna_coil
00021:     ///     → consumed by an authoritative relic repair (relic_recipes.json,
00022:     ///       "ham_radio" Vintage Ham Radio Set et al.) through the canonical
00023:     ///       WorkshopReverseEngineeringSystem.StartRepair → TryConsumeBill path.
00024:     ///   micro_radio_tower / read_radio_log → journal micro_radio_tower_log
00025:     ///     → canonical JournalSystem.TryDiscoverKnowledge, exactly once.
00026:     ///
00027:     /// No source-specific "use radio tower coil" path exists or is added: the
00028:     /// coil is consumed by the same bill every radio repair uses. Progression
00029:     /// gates stay canonical — the coil alone cannot complete a repair that
00030:     /// requires more components.
00031:     /// </summary>
00032:     public class MicroLocationRadioIntegrationTests
00033:     {
00034:         private const string RadioTowerId = "micro_radio_tower";
00035:         private const string OpenCabinetChoiceId = "open_radio_cabinet";
00036:         private const string ReadLogChoiceId = "read_radio_log";
00037:         private const string IgnoreChoiceId = "ignore_radio";
00038:         private const string CoilItemId = "antenna_coil";
00039:         private const string RadioTowerLogKey = "micro_radio_tower_log";
00040:         private const string HamRadioRelicId = "ham_radio";
00041:
00042:         private static string DataDir()
00043:         {
00044:             var dir = new DirectoryInfo(AppContext.BaseDirectory);
00045:             while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
00046:                 dir = dir.Parent!;
00047:             return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
00048:         }
00049:
00050:         private static NarrativeEncounterSystem CreateProductionNarrativeSystem()
00051:         {
00052:             var sys = new NarrativeEncounterSystem();
00053:             var defs = NarrativeEncounterCatalogLoader.Load(
00054:                 DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00055:             sys.RegisterRange(defs);
00056:             return sys;
00057:         }
00058:
00059:         private static RelicCatalog LoadRelicCatalog()
00060:         {
00061:             var catalog = RelicCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00062:             Assert.False(catalog.relics.Count == 0, "relic_recipes.json must load — it is the coil's downstream consumer");
00063:             return catalog;
00064:         }
00065:
00066:         private static (WorkshopReverseEngineeringSystem workshop, Inventory inventory) CreateWorkshop(
00067:             params (string itemId, int count)[] stock)
00068:         {
00069:             var inventory = new Inventory();
00070:             foreach (var (itemId, count) in stock)
00071:                 inventory.AddById(itemId, count);
00072:             var research = new ResearchSystem();
00073:             var crafting = new CraftingSystem(inventory);
00074:             var workshop = new WorkshopReverseEngineeringSystem(inventory, research, crafting);
00075:             workshop.LoadCatalog(LoadRelicCatalog());
00076:             return (workshop, inventory);
00077:         }
00078:
00079:         private sealed class TestAuthor : ISurvivorAuthor
00080:         {
00081:             public string Id => "surv_expedition";
00082:             public string DisplayName => "Scavenger";
00083:             public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
00084:         }
00085:
00086:         // ── Reward: the coil grant ─────────────────────────────────────
00087:
00088:         [Fact]
00089:         public void F19_01_OpenRadioCabinet_GrantsExactlyOneCoil_DepletesSite()
00090:         {
00091:             var sys = CreateProductionNarrativeSystem();
00092:             var res = sys.TryResolve(RadioTowerId, OpenCabinetChoiceId, "loc_radio_hill", 6);
00093:             Assert.NotNull(res);
00094:             Assert.Equal(CoilItemId, res!.GrantItemId);
00095:             Assert.Equal(1, res.GrantItemQuantity);
00096:             Assert.True(res.DepletesOnResolveFlag());
00097:             Assert.True(string.IsNullOrEmpty(res.SetWorldFlagId), "coil grant must not fake a world flag");
00098:             Assert.True(sys.IsDepleted(RadioTowerId));
00099:         }
00100:
00101:         [Fact]
00102:         public void F19_02_CoilGrant_IsOneShot_AcrossSaveReload()
00103:         {
00104:             var sys = CreateProductionNarrativeSystem();
00105:             Assert.NotNull(sys.TryResolve(RadioTowerId, OpenCabinetChoiceId, "loc_radio_hill", 6));
00106:
00107:             var json = new SystemTextJsonSerializer();
00108:             var restored = CreateProductionNarrativeSystem();
00109:             restored.RestoreState(json.Deserialize<NarrativeEncounterState>(json.Serialize(sys.CaptureState()))!);
00110:             Assert.True(restored.IsDepleted(RadioTowerId));
00111:
00112:             // The production selector can never re-surface the site for any seed.
00113:             for (int seed = 0; seed < 64; seed++)
00114:             {
00115:                 var picked = restored.SelectEncounter("Cautious", 0f, "loc_radio_hill", new SeededRng(seed));
00116:                 Assert.NotEqual(RadioTowerId, picked?.id);
00117:             }
00118:         }
00119:
00120:         // ── Functional use: the coil enters real radio progression ─────
00121:
00122:         [Fact]
00123:         public void F19_03_Coil_IsAuthoredInputOfRadioRelicRepairs()
00124:         {
00125:             // The coil must appear in at least one authoritative relic bill.
00126:             var catalog = LoadRelicCatalog();
00127:             var consumers = catalog.relics
00128:                 .Where(r => r?.required_components != null && r.required_components.Contains(CoilItemId))
00129:                 .Select(r => r.relic_id)
00130:                 .ToList();
00131:             Assert.Contains(HamRadioRelicId, consumers);
00132:             Assert.True(consumers.Count >= 4, $"expected the authored radio relic family to consume the coil, found: {string.Join(", ", consumers)}");
00133:         }
00134:
00135:         [Fact]
00136:         public void F19_04_CoilAlone_CannotCompleteRadioRepair_ProgressionGatesHold()
00137:         {
00138:             // §9.8 — an item grant is not a free upgrade: with only the coil in
00139:             // inventory, the canonical repair must refuse (missing bill lines).
00140:             var (workshop, inventory) = CreateWorkshop((CoilItemId, 1));
00141:             var result = workshop.StartRepair(HamRadioRelicId, "surv_researcher");
00142:             Assert.False(result.IsSuccess);
00143:             Assert.Equal("missing_components", result.FailureCode);
00144:             Assert.Equal(1, inventory.CountById(CoilItemId)); // bill is atomic — nothing consumed
00145:         }
00146:
00147:         [Fact]
00148:         public void F19_05_Coil_IsConsumedByCanonicalRadioRepair_BillAtomic()
00149:         {
00150:             // Full authored bill for the Vintage Ham Radio Set.
00151:             var catalog = LoadRelicCatalog();
00152:             var relic = catalog.relics.First(r => r.relic_id == HamRadioRelicId);
00153:             var stock = relic.required_components.Select(c => (c, 1)).ToArray();
00154:             var (workshop, inventory) = CreateWorkshop(stock);
00155:
00156:             Assert.True(inventory.CountById(CoilItemId) >= 1);
00157:             var result = workshop.StartRepair(HamRadioRelicId, "surv_researcher");
00158:             Assert.True(result.IsSuccess, $"repair start failed: {result.FailureCode}");
00159:             Assert.Equal(0, inventory.CountById(CoilItemId)); // consumed by the canonical transaction
00160:
00161:             // The grant → consumption loop is deterministic: the same bill
00162:             // always costs exactly one coil.
00163:             var coilLines = relic.required_components.Count(c => c == CoilItemId);
00164:             Assert.Equal(1, coilLines);
00165:         }
00166:
00167:         [Fact]
00168:         public void F19_06_CoilGrant_PlusRepair_EqualsVanillaEconomy_NoSourceBias()
00169:         {
00170:             // A coil from the micro-location must behave identically to a coil
00171:             // from any other source (workshop fabrication): same bill, same
00172:             // cost — exactly one coil consumed, nothing source-specific.
00173:             var (workshopA, inventoryA) = CreateWorkshop();
00174:             inventoryA.AddById(CoilItemId, 1); // simulated micro-location grant
00175:
00176:             var catalog = LoadRelicCatalog();
00177:             var relic = catalog.relics.First(r => r.relic_id == HamRadioRelicId);
00178:             foreach (var comp in relic.required_components)
00179:                 inventoryA.AddById(comp, 1); // grant + bill line = 2 coils in stock
00180:
00181:             Assert.Equal(2, inventoryA.CountById(CoilItemId));
00182:             var resultA = workshopA.StartRepair(HamRadioRelicId, "surv_researcher");
00183:             Assert.True(resultA.IsSuccess);
00184:             Assert.Equal(1, inventoryA.CountById(CoilItemId)); // exactly the bill's one coil consumed
00185:         }
00186:
00187:         // ── Journal integration ────────────────────────────────────────
00188:
00189:         [Fact]
00190:         public void F19_07_ReadRadioLog_UnlocksJournalEntry_ExactlyOnce()
00191:         {
00192:             var sys = CreateProductionNarrativeSystem();
00193:             var journal = new JournalSystem();
00194:
00195:             var res = sys.TryResolve(RadioTowerId, ReadLogChoiceId, "loc_radio_hill", 6);
00196:             Assert.NotNull(res);
00197:             Assert.Equal(RadioTowerLogKey, res!.JournalUnlockId);
00198:
00199:             var first = journal.TryDiscoverKnowledge(res.JournalUnlockId, new TestAuthor(), 6);
00200:             Assert.NotNull(first);
00201:             Assert.Equal(RadioTowerLogKey, first!.KnowledgeKey);
00202:
00203:             // Dedup gate: a second unlock attempt returns null and writes nothing.
00204:             var second = journal.TryDiscoverKnowledge(RadioTowerLogKey, new TestAuthor(), 7);
00205:             Assert.Null(second);
00206:             Assert.Equal(1, journal.Entries.Count(e => e.KnowledgeKey == RadioTowerLogKey));
00207:         }
00208:
00209:         [Fact]
00210:         public void F19_08_RadioLogChoice_IsNonDepleting_SiteStaysLiveForCoil()
00211:         {
00212:             // Authored multi-stage design: reading the log does not deplete;
00213:             // the coil choice does. Both facts pinned so the ordering contract
00214:             // (log first, salvage later — or reverse) stays intact.
00215:             var sys = CreateProductionNarrativeSystem();
00216:             Assert.NotNull(sys.TryResolve(RadioTowerId, ReadLogChoiceId, "loc_radio_hill", 6));
00217:             Assert.False(sys.IsDepleted(RadioTowerId));
00218:
00219:             Assert.NotNull(sys.TryResolve(RadioTowerId, OpenCabinetChoiceId, "loc_radio_hill", 7));
00220:             Assert.True(sys.IsDepleted(RadioTowerId));
00221:         }
00222:
00223:         [Fact]
00224:         public void F19_09_JournalUnlockKey_StayedInMicroNamespace_EntryRecorded()
00225:         {
00226:             // §9.6 — no brittle prose assertions: the canonical checks are the
00227:             // key's namespace convention (same rule as the Plan-49 audit) and
00228:             // a real, non-empty composed journal entry under that key.
00229:             Assert.StartsWith("micro_", RadioTowerLogKey, StringComparison.Ordinal);
00230:
00231:             var text = JournalVoice.ComposeFullText(RadioTowerLogKey, RiskBiasTrait.Realist, 6);
00232:             Assert.False(string.IsNullOrWhiteSpace(text));
00233:
00234:             var journal = new JournalSystem();
00235:             var entry = journal.TryDiscoverKnowledge(RadioTowerLogKey, new TestAuthor(), 6);
00236:             Assert.NotNull(entry);
00237:             Assert.False(string.IsNullOrWhiteSpace(entry!.Text));
00238:         }
00239:
00240:         [Fact]
00241:         public void F19_10_IgnoreChoice_NeutralPath()
00242:         {
00243:             var sys = CreateProductionNarrativeSystem();
00244:             var res = sys.TryResolve(RadioTowerId, IgnoreChoiceId, "loc_radio_hill", 6);
00245:             Assert.NotNull(res);
00246:             Assert.True(string.IsNullOrEmpty(res!.GrantItemId));
00247:             Assert.True(string.IsNullOrEmpty(res.JournalUnlockId));
00248:             Assert.False(res.DepletesEncounter);
00249:         }
00250:
00251:         // ── Persistence: grant-then-use across save boundaries (§12.3) ─
00252:
00253:         [Fact]
00254:         public void F19_11_GrantedCoil_SurvivesSaveReload_AsUsableInventory()
00255:         {
00256:             // Save AFTER the reward, BEFORE using it — the coil must remain a
00257:             // normal consumable component post-restore (§12.3 partial progression).
00258:             var inventory = new Inventory();
00259:             inventory.AddById(CoilItemId, 1); // the micro-location grant, canonical transaction
00260:             var json = new SystemTextJsonSerializer();
00261:             string saved = json.Serialize(inventory.CaptureState());
00262:
00263:             var restored = new Inventory();
00264:             restored.RestoreState(json.Deserialize<Ashfall.Core.Inventory.InventorySaveState>(saved)!, id => new Ashfall.Core.Inventory.ItemDefinition { id = id });
00265:             Assert.Equal(1, restored.CountById(CoilItemId));
00266:
00267:             // And it still completes a real radio repair after the round trip.
00268:             var catalog = LoadRelicCatalog();
00269:             var relic = catalog.relics.First(r => r.relic_id == HamRadioRelicId);
00270:             foreach (var comp in relic.required_components)
00271:                 if (comp != CoilItemId) restored.AddById(comp, 1);
00272:
00273:             var research = new ResearchSystem();
00274:             var crafting = new CraftingSystem(restored);
00275:             var workshop = new WorkshopReverseEngineeringSystem(restored, research, crafting);
00276:             workshop.LoadCatalog(catalog);
00277:             var result = workshop.StartRepair(HamRadioRelicId, "surv_researcher");
00278:             Assert.True(result.IsSuccess);
00279:             Assert.Equal(0, restored.CountById(CoilItemId));
00280:         }
00281:     }
00282:
00283:     /// <summary>Small assertion helper kept off the payload type to avoid
00284:     /// leaking test-only members into Core.</summary>
00285:     internal static class NarrativeResolutionTestExtensions
00286:     {
00287:         public static bool DepletesOnResolveFlag(this NarrativeEncounterResolutionResult r)
00288:             => r.DepletesEncounter;
00289:     }
00290: }
```
# Appendix M — External verification handoff

The following checks are to be run by the owning integrator after writing: character count, SHA-256 revalidation, path-token resolution, duplicate-heading/unsupported-claim scan, and `git diff --check`. The final ledger entry must report actual results, not this template.
