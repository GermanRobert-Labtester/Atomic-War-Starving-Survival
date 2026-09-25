# Plan 98 — Standing Record Factions: Eight-Faction Catalog, Record Authority & Legible Access

> **Rebuild status:** TERMINAL 8-FACTION CONTENT + REACHABILITY AUDIT
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

The historical baseline was 4,703 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Preserve the eight authored Standing Record factions as a bounded, evidence-backed record of the expansion while separating faction definitions from mutable faction standing, trade, and endgame authority. The historical 1→8 data expansion is already present; the valuable next step is a current consumer/validation audit, not another faction count.

**Bounded outcome:** Re-audit the eight faction rows, their `wants`/`offers` vocabulary, the Standing Record loader/engine boundary, the expansion-hub persistence route, and the Standing Record panels. Any residual must be a named reachability or integrity gap with one owner; no new faction system is justified.

**Non-goals:** no new faction system, no second trust ledger, no automatic standing writes, no arbitrary faction growth, no UI authority, no production/data/test edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `standing_record_factions.json` contains 8 rows.
- VERIFIED CURRENT: `StandingRecordCatalogLoader` loads faction and quest catalogs.
- VERIFIED CURRENT: `StandingRecordHostSession` is composed by the expansion host and exposes capture/restore.
- HISTORICAL RECORD: Wave 40/Plan 98 records the 1→8 expansion and focused tests; this package does not claim a fresh run.
- The plan must not infer that faction `wants`/`offers` are inventory ids; current rows contain service-like tokens and the DTO does not define a universal item-reference contract.

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

- `Assets/StreamingAssets/Data/standing_record_factions.json` exists at 6,222 bytes; SHA-256 `65d731c51c28e43ec37bdae1c1f9fd4e3c73ac0e867594ceb82a04e9db54a68b`.
- `Assets/StreamingAssets/Data/standing_record_quests.json` exists at 55,098 bytes; SHA-256 `202c3ef07dac2e953a6dbe5968469ff2a79d5033deb048f30101fb46c6319f8b`.
- `Assets/StreamingAssets/Data/standing_record_memory.json` exists at 11,295 bytes; SHA-256 `5b0cf181260669fa6f60465c1ccdcb5269fed6942754d8248f68bb1b17621205`.
- `Assets/StreamingAssets/Data/standing_record_layouts.json` exists at 62,607 bytes; SHA-256 `be7c35c41115e688179380aa200c4f9508130d079302a55f24d27802b19a4b6a`.

# 3. Required Delta

Replace the historical pure-data brief with an eight-row current census and a precise consumer/authority map. Audit whether every faction field is live, dormant, or invalid. Identify any safe residual package without adding a parallel faction, trade, or standing authority.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Which current callers consume the faction definitions, and which fields are only descriptive?
Does the expansion-hub persistence path round-trip the actual StandingRecord state, including the faction catalog’s relationship to mutable record state?
Are `wants` and `offers` intentionally service vocabulary, and is there a current typed consumer for them?
Does the current panel expose inactive/access-blocked factions honestly, and does the atlas route remain reachable?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| authored faction definitions | `StandingRecordCatalogLoader` | `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs` | Rows are content definitions; they do not own mutable faction trust. |
| record mutations and unlock/day state | `StandingRecordEngine` | `Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs` | Owns record state and its deterministic progression. |
| host composition and persistence façade | `ExpansionHostSession` | `src/Host/ExpansionHostSession.cs` | Composes the Standing Record host and carries its state inside the expansion hub envelope. |
| player-facing record surfaces | `StandingRecordPanel / StandingRecordAtlasPanel` | `src/UI/StandingRecordPanel.cs; src/UI/StandingRecordAtlasPanel.cs` | Present current state; do not calculate faction effects. |
| mutable faction standing | `FactionStanceEngine / canonical faction owner` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs; Assets/Ashfall.Core/Economy/FactionStanceEngine.cs` | Standing Record text cannot bypass the canonical standing write seam. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for standing-record faction catalog.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Standing Record Factions: Eight-Faction Catalog, Record Authority & Legible Access                                               │
│ Integration route: DATA-ONLY + existing owner/host audit                             │
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

1. **Use StandingRecordCatalogLoader and StandingRecordEngine; do not introduce a second record owner.**
2. **Keep authored definitions immutable and route mutable faction consequences through the canonical faction owner.**
3. **Treat the current eight rows as terminal content unless a measured gap proves another role.**
4. **Use the existing expansion-hub persistence route; no static-definition save section.**
5. **Project current state in the existing panels and preserve accessibility/lifecycle.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| authored faction definitions | `StandingRecordCatalogLoader` | `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs` | Rows are content definitions; they do not own mutable faction trust. |
| record mutations and unlock/day state | `StandingRecordEngine` | `Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs` | Owns record state and its deterministic progression. |
| host composition and persistence façade | `ExpansionHostSession` | `src/Host/ExpansionHostSession.cs` | Composes the Standing Record host and carries its state inside the expansion hub envelope. |
| player-facing record surfaces | `StandingRecordPanel / StandingRecordAtlasPanel` | `src/UI/StandingRecordPanel.cs; src/UI/StandingRecordAtlasPanel.cs` | Present current state; do not calculate faction effects. |
| mutable faction standing | `FactionStanceEngine / canonical faction owner` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs; Assets/Ashfall.Core/Economy/FactionStanceEngine.cs` | Standing Record text cannot bypass the canonical standing write seam. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load strict faction/quest catalogs
2. bind definitions to the existing StandingRecord catalog
3. project faction labels/access text through current host/UI
4. route any real standing/trade consequence only through its canonical owner
5. capture/restore the existing expansion-hub owner state
6. run focused catalog/system/host checks

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- faction rows are immutable definitions until an owner consumes them
- faction ids are unique and stable
- home_region and alignment are validated against current vocabularies
- wants/offers are descriptive/service vocabulary unless a current consumer proves item-id semantics
- no faction row creates trust, territory, currency, or a quest by itself
- UI must show an honest unavailable state when a faction is inactive or lacks an access route

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

Contract rules for standing-record faction catalog:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

The JSON is the definition authority. Audit whether every field is consumed; do not silently reinterpret strings as item ids or services. Any future schema extension requires a named consumer, validator, continuity review, and migration/default rule.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

Standing Record state is carried by the existing expansion-hub persistence path; no new `standing_record` save section is justified by definitions alone. A future mutable faction state change must use the existing faction/expansion owner and its capture/restore contract.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

Definitions and panel ordering are ordinal-stable. The Standing Record engine may use an existing seeded host stream for record generation, but faction row order must not consume or shift a gameplay RNG stream. Paired same-seed replay is required for any future progression change.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

The host binds the catalog and advances the existing record engine. A faction row is not a day event unless a current consumer emits one. Any access/trade/standing consequence must be an existing owner command; the record panel only projects the result.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Host/StandingRecordHostSession.cs` — composes the current engine/catalog and exposes current record state
- `src/Host/ExpansionHostSession.cs` — binds the Standing Record session into the existing expansion hub
- `src/Main.ExpansionHub.cs` — routes the player-facing record surface and persistence handoff
- `src/UI/StandingRecordPanel.cs` — renders current faction/record facts
- `src/UI/StandingRecordAtlasPanel.cs` — renders the spatial/read-only atlas projection

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Faction dossiers are fictional records of authority and access. They may describe wants/offers as in-world services, but they cannot grant a resource or change standing unless a current owner records that consequence. Prose must not imply a live route where only a definition exists.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/StandingRecordFactionExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/StandingRecordSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Governance/Plan89_98MusterFactionIntegrationTests.cs`

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
| Phase 0 — premise/catalog census | read current rows, loader, engine, expansion save, and panels | all eight rows and current fields are enumerated; no arbitrary count target remains | no undocumented scope or shortcut |
| Phase 1 — consumer graph | trace each faction field to a current caller or mark it dormant | no dead definition is called live without evidence | no undocumented scope or shortcut |
| Phase 2 — authority/save audit | verify expansion-hub capture/restore and faction standing boundary | a safe no-change result is acceptable | no undocumented scope or shortcut |
| Phase 3 — residual package | only if a concrete reachability defect survives | one owner, one claim, focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/standing_record_factions.json` | READ ONLY; future MODIFY only for a proven field/consumer defect | retain as sole faction-definition authority |
| `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs` | READ ONLY | loader/DTO boundary |
| `Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs` | READ ONLY | record state owner |
| `src/Host/ExpansionHostSession.cs` | READ ONLY | existing persistence/composition seam |
| `src/UI/StandingRecordPanel.cs` | READ ONLY | presentation only |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| parallel faction trust/standing state | use the canonical faction owner; return a decision packet if no seam exists |
| unbounded row growth | stop at the current eight unless a measured coverage gap names a new role |
| treating service vocabulary as item references | audit consumers and preserve current semantics |
| new save section for static definitions | never persist authored rows as mutable state |

# 22. Explicit Non-Goals

- no new faction catalog
- no new faction trust or territory owner
- no arbitrary 8→N growth
- no production/data/test/UI edits in this planning package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for standing-record faction catalog are named from current evidence.
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

- A current consumer graph for every faction field, including whether wants/offers are service tokens or item references.
- A strict validation/reachability report that distinguishes live, dormant, and invalid rows.
- A bounded follow-up package only if a current host/event/save gap is proven.

## MUST NOT DO

- do not create a second faction standing ledger
- do not turn descriptive offers into inventory grants
- do not call a row reachable merely because a test loads it
- do not alter the current save section or panel registry in this package

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/StandingRecordFactionExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/StandingRecordSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Governance/Plan89_98MusterFactionIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read the current eight-row catalog, StandingRecordCatalogLoader, StandingRecordEngine, expansion-hub persistence, and the two record panels; write a field-to-consumer table before proposing any change.

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

## `Assets/StreamingAssets/Data/standing_record_factions.json`
- Bytes: 6,222; SHA-256: `65d731c51c28e43ec37bdae1c1f9fd4e3c73ac0e867594ceb82a04e9db54a68b`
- Root keys: `actions, schema_version`
- `actions`: list[8]; union fields: `access_rule, alignment, badge_asset_id, display_name, home_region, id, is_active, offers, signature_quote, trust, wants`
  - row 1: `{"access_rule":"Scrape three plates without writing a lived name or a Continuity number, and Overlay labour withdraws. They do not raid. Rooms go dark of juniors. Posts stay posts.","alignment":"conditional","badge_asset_id":"","display_name":"The Overlay","home_region":"all_regions","id":"faction_the_overlay","is_active":true,"offers":["cadastral_keys","travel_correction_on_named_sites"],"signature_quote":"The Schedule named households. The Record names ground. Ground does not argue.","trust":0,"wants":["brass_fittings","sr_stencil_pot","lamp_oil"]}`
  - row 2: `{"access_rule":"Maintain metered cistern quotas and settle overdraw debts before each dawn bell; tampering with flow restrictors terminates drawing rights at every district conduit.","alignment":"conditional","badge_asset_id":"","display_name":"The Scale","home_region":"industrial_belt","id":"faction_the_scale","is_active":true,"offers":["potable_ration_quota","sluice_transit_clearance","flow_rate_telemetry"],"signature_quote":"Water is counted because uncounted water disappears into someone else's cellar.","trust":0,"wants":["brass_valve_bodies","filter_charcoal","pipe_sealant"]}`
  - row 3: `{"access_rule":"Submit boundary disputes to formal council hearing and accept sworn survey markers; violence against field registrars voids all historical parcel claims.","alignment":"neutral","badge_asset_id":"","display_name":"The Compact","home_region":"dead_suburbs","id":"faction_the_compact","is_active":true,"offers":["cadastral_boundary_deeds","arbitration_records","survey_marker_waypoints"],"signature_quote":"The war burned the houses, but the property line between the ashes remains recorded.","trust":0,"wants":["parchment_rolls","iron_gall_ink","surveyor_transit_glass"]}`
  - row 4: `{"access_rule":"Pay risk premiums in advance and report cargo manifests honestly; concealing hazardous goods or defaulting on contract indemnity forfeits depot protection.","alignment":"conditional","badge_asset_id":"","display_name":"The Underwrite","home_region":"industrial_belt","id":"faction_the_underwrite","is_active":true,"offers":["convoy_underwriting","armed_escort_vouchers","depot_fuel_draws"],"signature_quote":"Nothing in the dust is safe; safety is only the premium someone agreed to pay.","trust":0,"wants":["hardened_plate_carriers","heavy_machine_oil","sealed_logistics_manifests"]}`
  - row 5: `{"access_rule":"Pay column road maintenance tolls in iron or coal and obey bridge axle limits; running a closed pass during blizzard warnings closes all waystation gates to your wagons.","alignment":"conditional","badge_asset_id":"","display_name":"The Cutters","home_region":"the_cut","id":"faction_the_cutters","is_active":true,"offers":["cleared_corridor_passage","heavy_haulage_sledges","span_waystation_shelter"],"signature_quote":"The road doesn't belong to the banner flying over it; it belongs to the crew chipping ice at four in the morning.","trust":0,"wants":["hardened_ice_spikes","black_coal_briquettes","steel_winch_cable"]}`
  - row 6: `{"access_rule":"Register cargo manifests at Berth 9 and pay wharfage in copper or pitch; docking uninspected hulls or discharging tainted bilge bars your vessels from coastal waterways.","alignment":"conditional","badge_asset_id":"","display_name":"The Fleet","home_region":"deep_coast","id":"faction_the_fleet","is_active":true,"offers":["barge_ferry_berth","coastal_salvage_tolls","tide_table_nav_charts"],"signature_quote":"The tide doesn't care whose seal is on the parchment; if the seams aren't pitched, the cargo sinks.","trust":0,"wants":["tarred_hemp_rigging","marine_pitch_caulk","copper_hull_nails"]}`
  - row 7: `{"access_rule":"Honor minuted trade quotas and refrain from carrying weapons onto the grain exchange floor; hoarding seed chits during planting season results in immediate civic expulsion.","alignment":"neutral","badge_asset_id":"","display_name":"The Rebuilders","home_region":"ash_flats","id":"faction_the_rebuilders","is_active":true,"offers":["staple_grain_bushels","soil_rotation_almanac","communal_silo_storage"],"signature_quote":"You cannot eat an imperial victory; you can eat what four women planted in the muck ninety days ago.","trust":0,"wants":["viable_heirloom_seeds","refractory_kiln_bricks","sterilized_field_dressings"]}`
  - row 8: `{"access_rule":"Present authorized travel chits at the outer wire and submit to weapons inspection; evading road checkpoints or concealing contraband results in immediate internment.","alignment":"conditional","badge_asset_id":"","display_name":"The Garrison","home_region":"ash_flats","id":"faction_the_garrison","is_active":true,"offers":["checkpoint_transit_chits","sentry_perimeter_watch","tactical_hazard_briefings"],"signature_quote":"A border isn't a line on paper; it's the ditch someone agreed to stand in with loaded rifles.","trust":0,"wants":["smokeless_powder_kegs","machined_rifle_extractors","preserved_medical_plasma"]}`
- Bytes: 6,222; SHA-256: `65d731c51c28e43ec37bdae1c1f9fd4e3c73ac0e867594ceb82a04e9db54a68b`
- Root keys: `actions, schema_version`
- `actions`: list[8]; union fields: `access_rule, alignment, badge_asset_id, display_name, home_region, id, is_active, offers, signature_quote, trust, wants`
  - row 1: `{"access_rule":"Scrape three plates without writing a lived name or a Continuity number, and Overlay labour withdraws. They do not raid. Rooms go dark of juniors. Posts stay posts.","alignment":"conditional","badge_asset_id":"","display_name":"The Overlay","home_region":"all_regions","id":"faction_the_overlay","is_active":true,"offers":["cadastral_keys","travel_correction_on_named_sites"],"signature_quote":"The Schedule named households. The Record names ground. Ground does not argue.","trust":0,"wants":["brass_fittings","sr_stencil_pot","lamp_oil"]}`
  - row 2: `{"access_rule":"Maintain metered cistern quotas and settle overdraw debts before each dawn bell; tampering with flow restrictors terminates drawing rights at every district conduit.","alignment":"conditional","badge_asset_id":"","display_name":"The Scale","home_region":"industrial_belt","id":"faction_the_scale","is_active":true,"offers":["potable_ration_quota","sluice_transit_clearance","flow_rate_telemetry"],"signature_quote":"Water is counted because uncounted water disappears into someone else's cellar.","trust":0,"wants":["brass_valve_bodies","filter_charcoal","pipe_sealant"]}`
  - row 3: `{"access_rule":"Submit boundary disputes to formal council hearing and accept sworn survey markers; violence against field registrars voids all historical parcel claims.","alignment":"neutral","badge_asset_id":"","display_name":"The Compact","home_region":"dead_suburbs","id":"faction_the_compact","is_active":true,"offers":["cadastral_boundary_deeds","arbitration_records","survey_marker_waypoints"],"signature_quote":"The war burned the houses, but the property line between the ashes remains recorded.","trust":0,"wants":["parchment_rolls","iron_gall_ink","surveyor_transit_glass"]}`
  - row 4: `{"access_rule":"Pay risk premiums in advance and report cargo manifests honestly; concealing hazardous goods or defaulting on contract indemnity forfeits depot protection.","alignment":"conditional","badge_asset_id":"","display_name":"The Underwrite","home_region":"industrial_belt","id":"faction_the_underwrite","is_active":true,"offers":["convoy_underwriting","armed_escort_vouchers","depot_fuel_draws"],"signature_quote":"Nothing in the dust is safe; safety is only the premium someone agreed to pay.","trust":0,"wants":["hardened_plate_carriers","heavy_machine_oil","sealed_logistics_manifests"]}`
  - row 5: `{"access_rule":"Pay column road maintenance tolls in iron or coal and obey bridge axle limits; running a closed pass during blizzard warnings closes all waystation gates to your wagons.","alignment":"conditional","badge_asset_id":"","display_name":"The Cutters","home_region":"the_cut","id":"faction_the_cutters","is_active":true,"offers":["cleared_corridor_passage","heavy_haulage_sledges","span_waystation_shelter"],"signature_quote":"The road doesn't belong to the banner flying over it; it belongs to the crew chipping ice at four in the morning.","trust":0,"wants":["hardened_ice_spikes","black_coal_briquettes","steel_winch_cable"]}`
  - row 6: `{"access_rule":"Register cargo manifests at Berth 9 and pay wharfage in copper or pitch; docking uninspected hulls or discharging tainted bilge bars your vessels from coastal waterways.","alignment":"conditional","badge_asset_id":"","display_name":"The Fleet","home_region":"deep_coast","id":"faction_the_fleet","is_active":true,"offers":["barge_ferry_berth","coastal_salvage_tolls","tide_table_nav_charts"],"signature_quote":"The tide doesn't care whose seal is on the parchment; if the seams aren't pitched, the cargo sinks.","trust":0,"wants":["tarred_hemp_rigging","marine_pitch_caulk","copper_hull_nails"]}`
  - row 7: `{"access_rule":"Honor minuted trade quotas and refrain from carrying weapons onto the grain exchange floor; hoarding seed chits during planting season results in immediate civic expulsion.","alignment":"neutral","badge_asset_id":"","display_name":"The Rebuilders","home_region":"ash_flats","id":"faction_the_rebuilders","is_active":true,"offers":["staple_grain_bushels","soil_rotation_almanac","communal_silo_storage"],"signature_quote":"You cannot eat an imperial victory; you can eat what four women planted in the muck ninety days ago.","trust":0,"wants":["viable_heirloom_seeds","refractory_kiln_bricks","sterilized_field_dressings"]}`
  - row 8: `{"access_rule":"Present authorized travel chits at the outer wire and submit to weapons inspection; evading road checkpoints or concealing contraband results in immediate internment.","alignment":"conditional","badge_asset_id":"","display_name":"The Garrison","home_region":"ash_flats","id":"faction_the_garrison","is_active":true,"offers":["checkpoint_transit_chits","sentry_perimeter_watch","tactical_hazard_briefings"],"signature_quote":"A border isn't a line on paper; it's the ditch someone agreed to stand in with loaded rifles.","trust":0,"wants":["smokeless_powder_kegs","machined_rifle_extractors","preserved_medical_plasma"]}`

## `Assets/StreamingAssets/Data/standing_record_quests.json`
- Bytes: 55,098; SHA-256: `202c3ef07dac2e953a6dbe5968469ff2a79d5033deb048f30101fb46c6319f8b`
- Root keys: `quests, schema_version`
- `quests`: list[32]; union fields: `briefing, choices, complete_mutation, display_name, fail_mutation, id, knowledge_key, min_day, prereq_quest_id, stages, target_location_id, type`
  - row 1: `{"briefing":"The last Sector 4 lamp has a brass plate over the stencil. Maren Holt is finishing a crate of spares. Ivy will not cross. Yara will not come south. The seam is the first room of the pack. The route cannot start in a menu.","choices":[{"id":"record_plate_screw","set_flag":"mutation_km19_plated","text":"Screw the plate. Brass over stencil."},{"id":"record_plate_scrape","set_flag":"mutation_km19_scraped","text":"Scrape the plate. Stencil reads again in a day."},{"id":"record_plate_both","set_flag":"mutation_km19_palimpsest","text":"Leave both layers. The post means two things."}],"complete_mutation":"mutation_km19_plated","display_…`
  - row 2: `{"briefing":"Convoy 12's hold is a blotter and a dead telephone. Overlay printed plates would replace the grease pencil. Copying the pencil discovers the grid convoy slots. Replacing it mutates the map. The hand will still be under there, paler.","choices":[{"id":"record_transit_install","set_flag":"mutation_transit_maps","text":"Install Overlay print. The hand is paler under glass."},{"id":"record_transit_crate","set_flag":"mutation_transit_maps","text":"Crate the print. The grease pencil stays the document."},{"id":"record_transit_both","set_flag":"mutation_transit_maps","text":"Palimpsest. The map means two things, which is already its jo…`
  - row 3: `{"briefing":"Garrison already searched for the Schedule. Overlay is filing site plates into municipal drawers. The brick may hold a field index. It will not hold households.","choices":[{"id":"record_archive_dig","set_flag":"mutation_archive_dug","text":"Dig. Free a field index, or sink the plate deeper."},{"id":"record_archive_leave","set_flag":"mutation_archive_sunk","text":"Leave the brick. The Garrison looked twice."},{"id":"record_archive_pull","set_flag":"mutation_archive_sunk","text":"Pull the half-sunk plate. The index stays in the geology."}],"complete_mutation":"mutation_archive_dug","display_name":"Grey Brick","fail_mutation":"mut…`
  - row 4: `{"briefing":"Ira Vell has the Standing Record. The visited column is empty until you have been. Overlay wants the book out. A photocopy in inventory does not fill the Vault cage. The book has a column for that. It is empty until the column is not a lie.","choices":[{"id":"record_book_copy","set_flag":"mutation_ministry_recast","text":"Copy in place. Incomplete copy. Ira knows."},{"id":"record_book_take","set_flag":"mutation_ministry_recast","text":"Take it. Overlay wants it out."},{"id":"record_book_receipt","set_flag":"mutation_ministry_recast","text":"Overlay receipt. The book leaves with a plate trail."},{"id":"record_book_refuse","set_fl…`
  - row 5: `{"briefing":"Osric Tann prices mass. Overlay lots have no kilograms. Edor may be waiting. Stand on the scale or put a plate on it. It will read as mass. That is the only number Osric will write.","choices":[{"id":"record_weigh_lots","set_flag":"mutation_weigh_lots","text":"Install lots on the beam. District 8 can pathfind by number."},{"id":"record_weigh_mass","set_flag":"mutation_weigh_mass_only","text":"Mass only. The needle has no opinion."},{"id":"record_weigh_both","set_flag":"mutation_weigh_lots","text":"Both columns disagree. Clerks lose hours."}],"complete_mutation":"mutation_weigh_lots","display_name":"The Needle","fail_mutation":"m…`
  - row 6: `{"briefing":"The Verge names places by hand and waiting list. Overlay names them by lot. Dara Mewn waters a plot. If levy took the caretaker, the hut is dark and Overlay has already been.","choices":[{"id":"record_verge_names","set_flag":"mutation_verge_names","text":"The Verge keeps names. The plate does not go on the row."},{"id":"record_verge_plate","set_flag":"mutation_verge_names","text":"The plate goes on the plot. Two 114s in the dirt."}],"complete_mutation":"mutation_verge_names","display_name":"Plot 114","fail_mutation":"mutation_verge_names","id":"quest_record_hands","knowledge_key":"","min_day":75,"prereq_quest_id":"quest_record_m…`
  - row 7: `{"briefing":"Overlay clipboard: friendly obstacle. D/9 already marked the rail. Looking from the span is a room. Checking the detonator is a branch that mutates Toll access - not an action setpiece.","choices":[{"id":"record_bridge_listed","set_flag":"mutation_bridge_listed","text":"Sign. The span is a listed charge in the Record."},{"id":"record_bridge_disturbed","set_flag":"mutation_bridge_disturbed","text":"Check the housing. The Tollman does not need to be present for the authority to work."},{"id":"record_bridge_copy","set_flag":"mutation_bridge_listed","text":"Copy the line. Ostrowski's sheet will match."}],"complete_mutation":"mutatio…`
  - row 8: `{"briefing":"COMPLETE plate vs mid-cycle gauges. Benno lives in the failure. Overlay cannot close a flood with brass. District 8 can file it closed. They can screw COMPLETE on the leaf. The leaf is still where it stopped.","choices":[{"id":"record_lock_lie","set_flag":"mutation_lock_complete_lie","text":"Leave COMPLETE. A flood becomes a closed file."},{"id":"record_lock_gauges","set_flag":"mutation_lock_gauges_filed","text":"File the gauges. Honest. The Drown reads it."},{"id":"record_lock_plate_down","set_flag":"mutation_lock_plate_down","text":"Take the plate down. The leaf is still where it stopped."}],"complete_mutation":"mutation_lock_…`
  - row 9: `{"briefing":"Overlay refreshes 12-B as a finished overflow address. The kit is water. The stencil is a levy map. Pump Nine condemned keeps the Vault far; one dry motor is a room that changes travel.","choices":[{"id":"record_12b_address","set_flag":"mutation_12b_address","text":"Refresh the stencil. 12-C can cite 12-B as a pool."},{"id":"record_12b_original","set_flag":"mutation_12b_address","text":"Keep the original. The gap stays part of the fallback."},{"id":"record_pump_live","set_flag":"mutation_pump_live","text":"Energise the dry motor. The Drown lowers a step."},{"id":"record_pump_condemned","set_flag":"mutation_pump_condemned","text"…`
  - row 10: `{"briefing":"Quil will not file plates without spoken site names. Sole will not enter ground on one testimony. Maren waits in the airlock with hats of brass. The player writes which gazetteer stands. Not from the bunker.","choices":[{"id":"record_gazetteer_stands","set_flag":"mutation_gazetteer_stands","text":"File the Record. Posts have numbers. Lived names are subtitles."},{"id":"record_gazetteer_lived","set_flag":"mutation_gazetteer_lived","text":"File the lived map. Plates in a crate, string-tied."},{"id":"record_gazetteer_both","set_flag":"mutation_gazetteer_palimpsest","text":"File both. Nobody likes either."},{"id":"record_gazetteer_n…`
  - row 11: `{"briefing":"Reconstruct the final hours of the Command Vault before the airlock was breached. Compare the oxygen scrubber logs with spent brass casing locations to determine if the garrison mutinied.","choices":[{"id":"record_mutiny_truth","set_flag":"flag_sr_vault_mutiny_proven","text":"Record the mutiny evidence in the civic chronicle."},{"id":"seal_command_record","set_flag":"flag_sr_vault_mutiny_suppressed","text":"Suppress the mutiny records to preserve military reputation."}],"complete_mutation":"mutation_command_vault_breached","display_name":"The Blasted Sump","fail_mutation":"","id":"quest_record_vault_breach_forensics","knowledge_…`
  - row 12: `{"briefing":"Train 14 sits derailed in the darkness beyond Platform 3. The dispatch clock stopped at 08:42. Investigate whether the track switch was thrown intentionally to trap civilian evacuees.","choices":[{"id":"blame_signalman","set_flag":"flag_sr_metro_sabotage","text":"Attribute the crash to deliberate sabotage by the stationmaster."},{"id":"blame_overload","set_flag":"flag_sr_metro_fracture","text":"Conclude the derailment was caused by panic and structural track fracture."}],"complete_mutation":"mutation_metro_dossier_found","display_name":"Platform 3 Siding","fail_mutation":"","id":"quest_record_metro_derailment_triage","knowledge_…`
  - row 13: `{"briefing":"Thirty-four brass shift tokens remain on the pegboard outside Adit 4. Descend into the coal gallery and discover whether the miners escaped through the emergency ventilation raise.","choices":[{"id":"inscribe_miners_memorial","set_flag":"flag_sr_miners_memorialized","text":"Chalk their names onto the surface shaft headframe."},{"id":"salvage_equipment_quiet","set_flag":"flag_sr_miners_looted","text":"Strip the miners brass carburetors without registering their names."}],"complete_mutation":"mutation_mine_shaft_cleared","display_name":"Adit 4 Shift Rota","fail_mutation":"","id":"quest_record_mine_shaft_adit_collapse","knowledge_k…`
  - row 14: `{"briefing":"A layer of magnesium slag fills Archive Vault 3. Sift the residue to reconstruct burned directives regarding the evacuation of District 8.","choices":[{"id":"publish_directive","set_flag":"flag_sr_directive_published","text":"Expose the preferential evacuation quota to the public."},{"id":"hold_for_blackmail","set_flag":"flag_sr_directive_blackmail","text":"Keep the evidence secret to leverage against surviving officials."}],"complete_mutation":"mutation_archive_index_scraped","display_name":"The Smoldering Index","fail_mutation":"","id":"quest_record_archive_burn_layer","knowledge_key":"","min_day":95,"prereq_quest_id":"quest_r…`
  - row 15: `{"briefing":"Examine the sheared gear teeth on Sluice Gate 4. Did the coastal flood barrier fail due to poor metallurgy or did a technician intentionally cut the gear pins?","choices":[{"id":"report_sabotage","set_flag":"flag_sr_gate4_sabotaged","text":"Confirm deliberate industrial sabotage in the lock record."},{"id":"report_fatigue","set_flag":"flag_sr_gate4_fatigue","text":"Record catastrophic storm surge fatigue as the official cause."}],"complete_mutation":"mutation_lock4_sluice_repaired","display_name":"The Broken Pinion at Gate 4","fail_mutation":"","id":"quest_record_sluice_failure_verdict","knowledge_key":"","min_day":100,"prereq_q…`
  - row 16: `{"briefing":"Canister 44 in the Seed Library Annex was emptied and resealed with common grain. Trace the missing cold-hardy wheat seeds through pre-war checkout cards.","choices":[{"id":"deliver_to_rebuilders","set_flag":"flag_sr_seeds_to_rebuilders","text":"Deliver the heirloom seeds to the agricultural community."},{"id":"keep_in_shelter_vault","set_flag":"flag_sr_seeds_to_vault","text":"Store the seeds securely in your own emergency vault."}],"complete_mutation":"mutation_seed_annex_cataloged","display_name":"The Emptied Canister","fail_mutation":"","id":"quest_record_seed_bank_purge_trace","knowledge_key":"","min_day":105,"prereq_quest_i…`
  - row 17: `{"briefing":"A discrepancy in the exterior wall thickness of the Logistics Reserve suggests a concealed sub-basement room omitted from the official blueprint.","choices":[{"id":"update_public_map","set_flag":"flag_sr_annex_mapped","text":"Draw the annex onto the public warehouse plan."},{"id":"keep_annex_secret","set_flag":"flag_sr_annex_secret","text":"Conceal the entrance again for exclusive shelter access."}],"complete_mutation":"mutation_sub_basement_blueprint","display_name":"The Unmapped Annex","fail_mutation":"","id":"quest_record_sub_basement_blueprint","knowledge_key":"","min_day":110,"prereq_quest_id":"quest_record_grease_pencil","…`
  - row 18: `{"briefing":"The main security turnstiles at Transit Authority HQ are permanently welded shut. Search the mechanical ducts for an unblocked bypass route.","choices":[{"id":"prop_door_open","set_flag":"flag_sr_transit_door_propped","text":"Wedge the security door open with an iron crowbar."},{"id":"maintain_duct_only","set_flag":"flag_sr_transit_duct_only","text":"Leave the door locked and keep the vent duct as a secret entrance."}],"complete_mutation":"mutation_transit_maps","display_name":"Bypass Duct 11-East","fail_mutation":"","id":"quest_record_transit_vent_shaft_route","knowledge_key":"","min_day":115,"prereq_quest_id":"quest_record_gre…`
  - row 19: `{"briefing":"A flooded basement under Atlantic Cold Store contains submerged copper evaporator coils. Map the layout before the sea ice cracks.","choices":[{"id":"salvage_copper_clean","set_flag":"flag_sr_coldstore_copper_salvaged","text":"Carefully unbolt the coils without damaging structural supports."},{"id":"blow_wall_drain","set_flag":"flag_sr_coldstore_wall_blown","text":"Blast a drainage hole through the outer foundation wall."}],"complete_mutation":"mutation_cold_store_sublevel","display_name":"Brine Tank Subfloor","fail_mutation":"","id":"quest_record_cold_store_sublevel","knowledge_key":"","min_day":120,"prereq_quest_id":"quest_rec…`
  - row 20: `{"briefing":"The gas and high-voltage electrical conduits intersect at Chamber 7. Align the conflicting utility drawings to bypass a collapsed boulevard.","choices":[{"id":"splice_power_cable","set_flag":"flag_sr_tunnel_lights_lit","text":"Tap the live municipal cable to power the underground tunnel lights."},{"id":"leave_dark_bypass","set_flag":"flag_sr_tunnel_kept_dark","text":"Keep the junction dark to avoid attracting scavenger patrols."}],"complete_mutation":"mutation_utility_junction_crossover","display_name":"Conduit Chamber 7","fail_mutation":"","id":"quest_record_utility_junction_crossover","knowledge_key":"","min_day":125,"prereq_q…`
  - row 21: `{"briefing":"Locate the unmarked graves of eight ironworkers who died erecting Railway Span 44 during the evacuation winter, and chisel their initials into the granite abutment.","choices":[{"id":"complete_memorial_chisel","set_flag":"flag_sr_span44_memorial_complete","text":"Inscribe all eight names in deep, permanent Roman lettering."},{"id":"paint_quick_crosses","set_flag":"flag_sr_span44_memorial_hasty","text":"Slap red primer paint crosses on the concrete and move on."}],"complete_mutation":"mutation_the_unmarked_plaque","display_name":"A Name on the Abutment","fail_mutation":"","id":"quest_record_the_unmarked_plaque","knowledge_key":"l…`
  - row 22: `{"briefing":"Ascend the frozen ridge to the Summit Relay Spire. Recover the final transmission log of the mountain watch team and ignite their memorial beacon.","choices":[{"id":"light_high_beacon","set_flag":"flag_sr_summit_beacon_lit","text":"Light the summit brazier. The flame is visible across three valleys."},{"id":"keep_cairn_dark","set_flag":"flag_sr_summit_cairn_dark","text":"Erect the stone cairn silently without lighting the fuel."}],"complete_mutation":"mutation_the_last_watch_beacon","display_name":"The Cairn at Summit 9","fail_mutation":"","id":"quest_record_the_last_watch_beacon","knowledge_key":"lore_sr_summit_watch","min_day"…`
  - row 23: `{"briefing":"A survey nail driven into bedrock marks the boundary between Sector 4 and District 8. Its position conflicts with later cadastral drawings by forty centimetres. Decide whether the physical iron outranks the municipal map.","choices":[{"id":"choice_uphold_physical_nail","set_flag":"flag_sr_nail_upheld","text":"Uphold the nail where it stands. Physical iron outranks subsequent drafting."},{"id":"choice_realign_to_cadastral","set_flag":"flag_sr_nail_realigned","text":"Realign the boundary to match the newer municipal cadastral drawing."},{"id":"choice_leave_seam_contested","set_flag":"flag_sr_nail_contested","text":"Declare the sea…`
  - row 24: `{"briefing":"Solvent testing on a sector-lamp crate reveals lampblack pigment mixed with linseed oil, used to obscure an older municipal stencil before a brass plate was seated. Trace the provenance of the alteration.","choices":[{"id":"choice_expose_overwrite","set_flag":"flag_sr_pigment_overwrite_exposed","text":"Record the plate as an unauthorized overwrite of the municipal stencil."},{"id":"choice_certify_overlay","set_flag":"flag_sr_pigment_overlay_certified","text":"Certify the brass plate as official retroactive district numbering."}],"complete_mutation":"mutation_overlay_pigment_exposed","display_name":"Overlay Pigment","fail_mutatio…`
  - row 25: `{"briefing":"A second field survey across Sector 4 totals twenty-six reflector posts, but the municipal register records only twenty-four. Reconcile whether the extra posts are uncertified survivor installations or deliberate ghost entries.","choices":[{"id":"choice_register_extra_posts","set_flag":"flag_sr_count_twenty_six","text":"Enter all twenty-six lamps onto the official maintenance roll."},{"id":"choice_strike_ghost_posts","set_flag":"flag_sr_count_twenty_four","text":"Strike the two phantom posts as unmaintained temporary markers."}],"complete_mutation":"mutation_second_count_reconciled","display_name":"The Second Count","fail_mutati…`
  - row 26: `{"briefing":"An unsworn keeper's oath obligates maintenance of dead Post 7 at the bridge approach. Relighting the lamp requires scarce oil stores, but striking it deprives northern caravans of a certified marker.","choices":[{"id":"choice_relight_post","set_flag":"flag_sr_lamp_relit","text":"Fill the reservoir and relight Post 7 in fulfillment of the keeper's oath."},{"id":"choice_retire_post","set_flag":"flag_sr_lamp_retired","text":"Decommission the post and stamp the record DARK / RETIRED."}],"complete_mutation":"mutation_lamp_keeper_relit","display_name":"The Keeper's Oath","fail_mutation":"mutation_lamp_keeper_retired","id":"quest_recor…`
  - row 27: `{"briefing":"The Compact and The Garrison both claim jurisdiction over a forty-metre highway strip. Each cites conflicting survey nails hammered into the asphalt. Arbitrate the territorial boundary before armed confrontation erupts.","choices":[{"id":"choice_rule_compact_boundary","set_flag":"flag_sr_boundary_compact","text":"Award parcel rights to The Compact per historical cadastral deeds."},{"id":"choice_rule_garrison_boundary","set_flag":"flag_sr_boundary_garrison","text":"Grant jurisdiction to The Garrison for tactical road security."},{"id":"choice_split_neutral_demarcation","set_flag":"flag_sr_boundary_neutral","text":"Establish a neu…`
  - row 28: `{"briefing":"A salt-crusted mounting plate on Greywater Tide Gauge was pried away with an iron crowbar, leaving only mounting holes and a scarred concrete pedestal. Trace the missing marker and restore the record.","choices":[{"id":"choice_reaffix_recovered_plate","set_flag":"flag_sr_plate_reaffixed","text":"Fasten the plate with stainless wire and certify the station live."},{"id":"choice_chisel_loss_record","set_flag":"flag_sr_plate_voided","text":"Chisel STATION 18 LOST into the concrete collar and file the void."}],"complete_mutation":"mutation_missing_plate_recovered","display_name":"The Stripped Post","fail_mutation":"mutation_missing_…`
  - row 29: `{"briefing":"Traverse the frozen bluffs to West Ridge Survey Camp to inspect ice-covered triangulation cairns. Winter rime threatens to obscure the brass azimuth plates that anchor the valley's baseline.","choices":[{"id":"choice_certify_cairn_bearings","set_flag":"flag_sr_cairn_certified","text":"Certify the high triangulation network as verified for winter travel."},{"id":"choice_condemn_ridge_route","set_flag":"flag_sr_ridge_condemned","text":"Condemn the high ridge route as unstable and erase the trail marker."}],"complete_mutation":"mutation_cold_survey_completed","display_name":"The Ridge Traverse","fail_mutation":"mutation_cold_survey…`
  - row 30: `{"briefing":"At the weighbridge, the fuel oil ledger records four hundred litres delivered to Sector 4 lamps, yet storage drums at Grange Hall sit nearly dry. Audit the supply trail and identify the diversion.","choices":[{"id":"choice_prosecute_oil_diversion","set_flag":"flag_sr_oil_prosecuted","text":"File embezzlement charges with The Scale and seal the siphoned pump."},{"id":"choice_authorize_emergency_draw","set_flag":"flag_sr_oil_authorized","text":"Retroactively classify the diverted oil as emergency civilian heating."}],"complete_mutation":"mutation_oil_ledger_audited","display_name":"The Siphoned Drum","fail_mutation":"mutation_oil_…`
  - row 31: `{"briefing":"In the meteorological station archive, a coastal boundary survey bears a heavy red diagonal stamp: REJECTED BY CADASTRE. Reconstruct the razor-excised marginal notes to discover why the survey was suppressed.","choices":[{"id":"choice_reinstate_survey","set_flag":"flag_sr_survey_reinstated","text":"Ratify the coastal survey as valid, expanding certified travel bounds."},{"id":"choice_uphold_rejection_stamp","set_flag":"flag_sr_survey_rejected","text":"Uphold the rejection stamp and enforce the coastal hazard cordon."}],"complete_mutation":"mutation_rejected_survey_resolved","display_name":"The Red Diagonal","fail_mutation":"muta…`
  - row 32: `{"briefing":"Sector 9 beyond Birchline Weather Station sits completely unlit on all maps—no nails, no plates, no stencils. Decide whether to incorporate the territory by planting the final benchmark or seal it as the permanent dark margin.","choices":[{"id":"choice_incorporate_sector","set_flag":"flag_sr_sector_incorporated","text":"Plant Benchmark 100, light the sector beacon, and extend the map."},{"id":"choice_declare_permanent_dark","set_flag":"flag_sr_sector_dark","text":"Hammer a marker stamped LIMIT / UNRECORDED and seal the frontier."}],"complete_mutation":"mutation_last_sector_incorporated","display_name":"The Unlit Frontier","fail_…`
- Bytes: 55,098; SHA-256: `202c3ef07dac2e953a6dbe5968469ff2a79d5033deb048f30101fb46c6319f8b`
- Root keys: `quests, schema_version`
- `quests`: list[32]; union fields: `briefing, choices, complete_mutation, display_name, fail_mutation, id, knowledge_key, min_day, prereq_quest_id, stages, target_location_id, type`
  - row 1: `{"briefing":"The last Sector 4 lamp has a brass plate over the stencil. Maren Holt is finishing a crate of spares. Ivy will not cross. Yara will not come south. The seam is the first room of the pack. The route cannot start in a menu.","choices":[{"id":"record_plate_screw","set_flag":"mutation_km19_plated","text":"Screw the plate. Brass over stencil."},{"id":"record_plate_scrape","set_flag":"mutation_km19_scraped","text":"Scrape the plate. Stencil reads again in a day."},{"id":"record_plate_both","set_flag":"mutation_km19_palimpsest","text":"Leave both layers. The post means two things."}],"complete_mutation":"mutation_km19_plated","display_…`
  - row 2: `{"briefing":"Convoy 12's hold is a blotter and a dead telephone. Overlay printed plates would replace the grease pencil. Copying the pencil discovers the grid convoy slots. Replacing it mutates the map. The hand will still be under there, paler.","choices":[{"id":"record_transit_install","set_flag":"mutation_transit_maps","text":"Install Overlay print. The hand is paler under glass."},{"id":"record_transit_crate","set_flag":"mutation_transit_maps","text":"Crate the print. The grease pencil stays the document."},{"id":"record_transit_both","set_flag":"mutation_transit_maps","text":"Palimpsest. The map means two things, which is already its jo…`
  - row 3: `{"briefing":"Garrison already searched for the Schedule. Overlay is filing site plates into municipal drawers. The brick may hold a field index. It will not hold households.","choices":[{"id":"record_archive_dig","set_flag":"mutation_archive_dug","text":"Dig. Free a field index, or sink the plate deeper."},{"id":"record_archive_leave","set_flag":"mutation_archive_sunk","text":"Leave the brick. The Garrison looked twice."},{"id":"record_archive_pull","set_flag":"mutation_archive_sunk","text":"Pull the half-sunk plate. The index stays in the geology."}],"complete_mutation":"mutation_archive_dug","display_name":"Grey Brick","fail_mutation":"mut…`
  - row 4: `{"briefing":"Ira Vell has the Standing Record. The visited column is empty until you have been. Overlay wants the book out. A photocopy in inventory does not fill the Vault cage. The book has a column for that. It is empty until the column is not a lie.","choices":[{"id":"record_book_copy","set_flag":"mutation_ministry_recast","text":"Copy in place. Incomplete copy. Ira knows."},{"id":"record_book_take","set_flag":"mutation_ministry_recast","text":"Take it. Overlay wants it out."},{"id":"record_book_receipt","set_flag":"mutation_ministry_recast","text":"Overlay receipt. The book leaves with a plate trail."},{"id":"record_book_refuse","set_fl…`
  - row 5: `{"briefing":"Osric Tann prices mass. Overlay lots have no kilograms. Edor may be waiting. Stand on the scale or put a plate on it. It will read as mass. That is the only number Osric will write.","choices":[{"id":"record_weigh_lots","set_flag":"mutation_weigh_lots","text":"Install lots on the beam. District 8 can pathfind by number."},{"id":"record_weigh_mass","set_flag":"mutation_weigh_mass_only","text":"Mass only. The needle has no opinion."},{"id":"record_weigh_both","set_flag":"mutation_weigh_lots","text":"Both columns disagree. Clerks lose hours."}],"complete_mutation":"mutation_weigh_lots","display_name":"The Needle","fail_mutation":"m…`
  - row 6: `{"briefing":"The Verge names places by hand and waiting list. Overlay names them by lot. Dara Mewn waters a plot. If levy took the caretaker, the hut is dark and Overlay has already been.","choices":[{"id":"record_verge_names","set_flag":"mutation_verge_names","text":"The Verge keeps names. The plate does not go on the row."},{"id":"record_verge_plate","set_flag":"mutation_verge_names","text":"The plate goes on the plot. Two 114s in the dirt."}],"complete_mutation":"mutation_verge_names","display_name":"Plot 114","fail_mutation":"mutation_verge_names","id":"quest_record_hands","knowledge_key":"","min_day":75,"prereq_quest_id":"quest_record_m…`
  - row 7: `{"briefing":"Overlay clipboard: friendly obstacle. D/9 already marked the rail. Looking from the span is a room. Checking the detonator is a branch that mutates Toll access - not an action setpiece.","choices":[{"id":"record_bridge_listed","set_flag":"mutation_bridge_listed","text":"Sign. The span is a listed charge in the Record."},{"id":"record_bridge_disturbed","set_flag":"mutation_bridge_disturbed","text":"Check the housing. The Tollman does not need to be present for the authority to work."},{"id":"record_bridge_copy","set_flag":"mutation_bridge_listed","text":"Copy the line. Ostrowski's sheet will match."}],"complete_mutation":"mutatio…`
  - row 8: `{"briefing":"COMPLETE plate vs mid-cycle gauges. Benno lives in the failure. Overlay cannot close a flood with brass. District 8 can file it closed. They can screw COMPLETE on the leaf. The leaf is still where it stopped.","choices":[{"id":"record_lock_lie","set_flag":"mutation_lock_complete_lie","text":"Leave COMPLETE. A flood becomes a closed file."},{"id":"record_lock_gauges","set_flag":"mutation_lock_gauges_filed","text":"File the gauges. Honest. The Drown reads it."},{"id":"record_lock_plate_down","set_flag":"mutation_lock_plate_down","text":"Take the plate down. The leaf is still where it stopped."}],"complete_mutation":"mutation_lock_…`
  - row 9: `{"briefing":"Overlay refreshes 12-B as a finished overflow address. The kit is water. The stencil is a levy map. Pump Nine condemned keeps the Vault far; one dry motor is a room that changes travel.","choices":[{"id":"record_12b_address","set_flag":"mutation_12b_address","text":"Refresh the stencil. 12-C can cite 12-B as a pool."},{"id":"record_12b_original","set_flag":"mutation_12b_address","text":"Keep the original. The gap stays part of the fallback."},{"id":"record_pump_live","set_flag":"mutation_pump_live","text":"Energise the dry motor. The Drown lowers a step."},{"id":"record_pump_condemned","set_flag":"mutation_pump_condemned","text"…`
  - row 10: `{"briefing":"Quil will not file plates without spoken site names. Sole will not enter ground on one testimony. Maren waits in the airlock with hats of brass. The player writes which gazetteer stands. Not from the bunker.","choices":[{"id":"record_gazetteer_stands","set_flag":"mutation_gazetteer_stands","text":"File the Record. Posts have numbers. Lived names are subtitles."},{"id":"record_gazetteer_lived","set_flag":"mutation_gazetteer_lived","text":"File the lived map. Plates in a crate, string-tied."},{"id":"record_gazetteer_both","set_flag":"mutation_gazetteer_palimpsest","text":"File both. Nobody likes either."},{"id":"record_gazetteer_n…`
  - row 11: `{"briefing":"Reconstruct the final hours of the Command Vault before the airlock was breached. Compare the oxygen scrubber logs with spent brass casing locations to determine if the garrison mutinied.","choices":[{"id":"record_mutiny_truth","set_flag":"flag_sr_vault_mutiny_proven","text":"Record the mutiny evidence in the civic chronicle."},{"id":"seal_command_record","set_flag":"flag_sr_vault_mutiny_suppressed","text":"Suppress the mutiny records to preserve military reputation."}],"complete_mutation":"mutation_command_vault_breached","display_name":"The Blasted Sump","fail_mutation":"","id":"quest_record_vault_breach_forensics","knowledge_…`
  - row 12: `{"briefing":"Train 14 sits derailed in the darkness beyond Platform 3. The dispatch clock stopped at 08:42. Investigate whether the track switch was thrown intentionally to trap civilian evacuees.","choices":[{"id":"blame_signalman","set_flag":"flag_sr_metro_sabotage","text":"Attribute the crash to deliberate sabotage by the stationmaster."},{"id":"blame_overload","set_flag":"flag_sr_metro_fracture","text":"Conclude the derailment was caused by panic and structural track fracture."}],"complete_mutation":"mutation_metro_dossier_found","display_name":"Platform 3 Siding","fail_mutation":"","id":"quest_record_metro_derailment_triage","knowledge_…`
  - row 13: `{"briefing":"Thirty-four brass shift tokens remain on the pegboard outside Adit 4. Descend into the coal gallery and discover whether the miners escaped through the emergency ventilation raise.","choices":[{"id":"inscribe_miners_memorial","set_flag":"flag_sr_miners_memorialized","text":"Chalk their names onto the surface shaft headframe."},{"id":"salvage_equipment_quiet","set_flag":"flag_sr_miners_looted","text":"Strip the miners brass carburetors without registering their names."}],"complete_mutation":"mutation_mine_shaft_cleared","display_name":"Adit 4 Shift Rota","fail_mutation":"","id":"quest_record_mine_shaft_adit_collapse","knowledge_k…`
  - row 14: `{"briefing":"A layer of magnesium slag fills Archive Vault 3. Sift the residue to reconstruct burned directives regarding the evacuation of District 8.","choices":[{"id":"publish_directive","set_flag":"flag_sr_directive_published","text":"Expose the preferential evacuation quota to the public."},{"id":"hold_for_blackmail","set_flag":"flag_sr_directive_blackmail","text":"Keep the evidence secret to leverage against surviving officials."}],"complete_mutation":"mutation_archive_index_scraped","display_name":"The Smoldering Index","fail_mutation":"","id":"quest_record_archive_burn_layer","knowledge_key":"","min_day":95,"prereq_quest_id":"quest_r…`
  - row 15: `{"briefing":"Examine the sheared gear teeth on Sluice Gate 4. Did the coastal flood barrier fail due to poor metallurgy or did a technician intentionally cut the gear pins?","choices":[{"id":"report_sabotage","set_flag":"flag_sr_gate4_sabotaged","text":"Confirm deliberate industrial sabotage in the lock record."},{"id":"report_fatigue","set_flag":"flag_sr_gate4_fatigue","text":"Record catastrophic storm surge fatigue as the official cause."}],"complete_mutation":"mutation_lock4_sluice_repaired","display_name":"The Broken Pinion at Gate 4","fail_mutation":"","id":"quest_record_sluice_failure_verdict","knowledge_key":"","min_day":100,"prereq_q…`
  - row 16: `{"briefing":"Canister 44 in the Seed Library Annex was emptied and resealed with common grain. Trace the missing cold-hardy wheat seeds through pre-war checkout cards.","choices":[{"id":"deliver_to_rebuilders","set_flag":"flag_sr_seeds_to_rebuilders","text":"Deliver the heirloom seeds to the agricultural community."},{"id":"keep_in_shelter_vault","set_flag":"flag_sr_seeds_to_vault","text":"Store the seeds securely in your own emergency vault."}],"complete_mutation":"mutation_seed_annex_cataloged","display_name":"The Emptied Canister","fail_mutation":"","id":"quest_record_seed_bank_purge_trace","knowledge_key":"","min_day":105,"prereq_quest_i…`
  - row 17: `{"briefing":"A discrepancy in the exterior wall thickness of the Logistics Reserve suggests a concealed sub-basement room omitted from the official blueprint.","choices":[{"id":"update_public_map","set_flag":"flag_sr_annex_mapped","text":"Draw the annex onto the public warehouse plan."},{"id":"keep_annex_secret","set_flag":"flag_sr_annex_secret","text":"Conceal the entrance again for exclusive shelter access."}],"complete_mutation":"mutation_sub_basement_blueprint","display_name":"The Unmapped Annex","fail_mutation":"","id":"quest_record_sub_basement_blueprint","knowledge_key":"","min_day":110,"prereq_quest_id":"quest_record_grease_pencil","…`
  - row 18: `{"briefing":"The main security turnstiles at Transit Authority HQ are permanently welded shut. Search the mechanical ducts for an unblocked bypass route.","choices":[{"id":"prop_door_open","set_flag":"flag_sr_transit_door_propped","text":"Wedge the security door open with an iron crowbar."},{"id":"maintain_duct_only","set_flag":"flag_sr_transit_duct_only","text":"Leave the door locked and keep the vent duct as a secret entrance."}],"complete_mutation":"mutation_transit_maps","display_name":"Bypass Duct 11-East","fail_mutation":"","id":"quest_record_transit_vent_shaft_route","knowledge_key":"","min_day":115,"prereq_quest_id":"quest_record_gre…`
  - row 19: `{"briefing":"A flooded basement under Atlantic Cold Store contains submerged copper evaporator coils. Map the layout before the sea ice cracks.","choices":[{"id":"salvage_copper_clean","set_flag":"flag_sr_coldstore_copper_salvaged","text":"Carefully unbolt the coils without damaging structural supports."},{"id":"blow_wall_drain","set_flag":"flag_sr_coldstore_wall_blown","text":"Blast a drainage hole through the outer foundation wall."}],"complete_mutation":"mutation_cold_store_sublevel","display_name":"Brine Tank Subfloor","fail_mutation":"","id":"quest_record_cold_store_sublevel","knowledge_key":"","min_day":120,"prereq_quest_id":"quest_rec…`
  - row 20: `{"briefing":"The gas and high-voltage electrical conduits intersect at Chamber 7. Align the conflicting utility drawings to bypass a collapsed boulevard.","choices":[{"id":"splice_power_cable","set_flag":"flag_sr_tunnel_lights_lit","text":"Tap the live municipal cable to power the underground tunnel lights."},{"id":"leave_dark_bypass","set_flag":"flag_sr_tunnel_kept_dark","text":"Keep the junction dark to avoid attracting scavenger patrols."}],"complete_mutation":"mutation_utility_junction_crossover","display_name":"Conduit Chamber 7","fail_mutation":"","id":"quest_record_utility_junction_crossover","knowledge_key":"","min_day":125,"prereq_q…`
  - row 21: `{"briefing":"Locate the unmarked graves of eight ironworkers who died erecting Railway Span 44 during the evacuation winter, and chisel their initials into the granite abutment.","choices":[{"id":"complete_memorial_chisel","set_flag":"flag_sr_span44_memorial_complete","text":"Inscribe all eight names in deep, permanent Roman lettering."},{"id":"paint_quick_crosses","set_flag":"flag_sr_span44_memorial_hasty","text":"Slap red primer paint crosses on the concrete and move on."}],"complete_mutation":"mutation_the_unmarked_plaque","display_name":"A Name on the Abutment","fail_mutation":"","id":"quest_record_the_unmarked_plaque","knowledge_key":"l…`
  - row 22: `{"briefing":"Ascend the frozen ridge to the Summit Relay Spire. Recover the final transmission log of the mountain watch team and ignite their memorial beacon.","choices":[{"id":"light_high_beacon","set_flag":"flag_sr_summit_beacon_lit","text":"Light the summit brazier. The flame is visible across three valleys."},{"id":"keep_cairn_dark","set_flag":"flag_sr_summit_cairn_dark","text":"Erect the stone cairn silently without lighting the fuel."}],"complete_mutation":"mutation_the_last_watch_beacon","display_name":"The Cairn at Summit 9","fail_mutation":"","id":"quest_record_the_last_watch_beacon","knowledge_key":"lore_sr_summit_watch","min_day"…`
  - row 23: `{"briefing":"A survey nail driven into bedrock marks the boundary between Sector 4 and District 8. Its position conflicts with later cadastral drawings by forty centimetres. Decide whether the physical iron outranks the municipal map.","choices":[{"id":"choice_uphold_physical_nail","set_flag":"flag_sr_nail_upheld","text":"Uphold the nail where it stands. Physical iron outranks subsequent drafting."},{"id":"choice_realign_to_cadastral","set_flag":"flag_sr_nail_realigned","text":"Realign the boundary to match the newer municipal cadastral drawing."},{"id":"choice_leave_seam_contested","set_flag":"flag_sr_nail_contested","text":"Declare the sea…`
  - row 24: `{"briefing":"Solvent testing on a sector-lamp crate reveals lampblack pigment mixed with linseed oil, used to obscure an older municipal stencil before a brass plate was seated. Trace the provenance of the alteration.","choices":[{"id":"choice_expose_overwrite","set_flag":"flag_sr_pigment_overwrite_exposed","text":"Record the plate as an unauthorized overwrite of the municipal stencil."},{"id":"choice_certify_overlay","set_flag":"flag_sr_pigment_overlay_certified","text":"Certify the brass plate as official retroactive district numbering."}],"complete_mutation":"mutation_overlay_pigment_exposed","display_name":"Overlay Pigment","fail_mutatio…`
  - row 25: `{"briefing":"A second field survey across Sector 4 totals twenty-six reflector posts, but the municipal register records only twenty-four. Reconcile whether the extra posts are uncertified survivor installations or deliberate ghost entries.","choices":[{"id":"choice_register_extra_posts","set_flag":"flag_sr_count_twenty_six","text":"Enter all twenty-six lamps onto the official maintenance roll."},{"id":"choice_strike_ghost_posts","set_flag":"flag_sr_count_twenty_four","text":"Strike the two phantom posts as unmaintained temporary markers."}],"complete_mutation":"mutation_second_count_reconciled","display_name":"The Second Count","fail_mutati…`
  - row 26: `{"briefing":"An unsworn keeper's oath obligates maintenance of dead Post 7 at the bridge approach. Relighting the lamp requires scarce oil stores, but striking it deprives northern caravans of a certified marker.","choices":[{"id":"choice_relight_post","set_flag":"flag_sr_lamp_relit","text":"Fill the reservoir and relight Post 7 in fulfillment of the keeper's oath."},{"id":"choice_retire_post","set_flag":"flag_sr_lamp_retired","text":"Decommission the post and stamp the record DARK / RETIRED."}],"complete_mutation":"mutation_lamp_keeper_relit","display_name":"The Keeper's Oath","fail_mutation":"mutation_lamp_keeper_retired","id":"quest_recor…`
  - row 27: `{"briefing":"The Compact and The Garrison both claim jurisdiction over a forty-metre highway strip. Each cites conflicting survey nails hammered into the asphalt. Arbitrate the territorial boundary before armed confrontation erupts.","choices":[{"id":"choice_rule_compact_boundary","set_flag":"flag_sr_boundary_compact","text":"Award parcel rights to The Compact per historical cadastral deeds."},{"id":"choice_rule_garrison_boundary","set_flag":"flag_sr_boundary_garrison","text":"Grant jurisdiction to The Garrison for tactical road security."},{"id":"choice_split_neutral_demarcation","set_flag":"flag_sr_boundary_neutral","text":"Establish a neu…`
  - row 28: `{"briefing":"A salt-crusted mounting plate on Greywater Tide Gauge was pried away with an iron crowbar, leaving only mounting holes and a scarred concrete pedestal. Trace the missing marker and restore the record.","choices":[{"id":"choice_reaffix_recovered_plate","set_flag":"flag_sr_plate_reaffixed","text":"Fasten the plate with stainless wire and certify the station live."},{"id":"choice_chisel_loss_record","set_flag":"flag_sr_plate_voided","text":"Chisel STATION 18 LOST into the concrete collar and file the void."}],"complete_mutation":"mutation_missing_plate_recovered","display_name":"The Stripped Post","fail_mutation":"mutation_missing_…`
  - row 29: `{"briefing":"Traverse the frozen bluffs to West Ridge Survey Camp to inspect ice-covered triangulation cairns. Winter rime threatens to obscure the brass azimuth plates that anchor the valley's baseline.","choices":[{"id":"choice_certify_cairn_bearings","set_flag":"flag_sr_cairn_certified","text":"Certify the high triangulation network as verified for winter travel."},{"id":"choice_condemn_ridge_route","set_flag":"flag_sr_ridge_condemned","text":"Condemn the high ridge route as unstable and erase the trail marker."}],"complete_mutation":"mutation_cold_survey_completed","display_name":"The Ridge Traverse","fail_mutation":"mutation_cold_survey…`
  - row 30: `{"briefing":"At the weighbridge, the fuel oil ledger records four hundred litres delivered to Sector 4 lamps, yet storage drums at Grange Hall sit nearly dry. Audit the supply trail and identify the diversion.","choices":[{"id":"choice_prosecute_oil_diversion","set_flag":"flag_sr_oil_prosecuted","text":"File embezzlement charges with The Scale and seal the siphoned pump."},{"id":"choice_authorize_emergency_draw","set_flag":"flag_sr_oil_authorized","text":"Retroactively classify the diverted oil as emergency civilian heating."}],"complete_mutation":"mutation_oil_ledger_audited","display_name":"The Siphoned Drum","fail_mutation":"mutation_oil_…`
  - row 31: `{"briefing":"In the meteorological station archive, a coastal boundary survey bears a heavy red diagonal stamp: REJECTED BY CADASTRE. Reconstruct the razor-excised marginal notes to discover why the survey was suppressed.","choices":[{"id":"choice_reinstate_survey","set_flag":"flag_sr_survey_reinstated","text":"Ratify the coastal survey as valid, expanding certified travel bounds."},{"id":"choice_uphold_rejection_stamp","set_flag":"flag_sr_survey_rejected","text":"Uphold the rejection stamp and enforce the coastal hazard cordon."}],"complete_mutation":"mutation_rejected_survey_resolved","display_name":"The Red Diagonal","fail_mutation":"muta…`
  - row 32: `{"briefing":"Sector 9 beyond Birchline Weather Station sits completely unlit on all maps—no nails, no plates, no stencils. Decide whether to incorporate the territory by planting the final benchmark or seal it as the permanent dark margin.","choices":[{"id":"choice_incorporate_sector","set_flag":"flag_sr_sector_incorporated","text":"Plant Benchmark 100, light the sector beacon, and extend the map."},{"id":"choice_declare_permanent_dark","set_flag":"flag_sr_sector_dark","text":"Hammer a marker stamped LIMIT / UNRECORDED and seal the frontier."}],"complete_mutation":"mutation_last_sector_incorporated","display_name":"The Unlit Frontier","fail_…`

## `Assets/StreamingAssets/Data/standing_record_memory.json`
- Bytes: 11,295; SHA-256: `5b0cf181260669fa6f60465c1ccdcb5269fed6942754d8248f68bb1b17621205`
- Root keys: `items, schema_version`
- `items`: list[52]; union fields: `requiredFlag, siteId, stratumId, text`
  - row 1: `{"requiredFlag":"","siteId":"loc_cut_kilometre_19","stratumId":"pre","text":"The post is Lamplighter orange, the kilometre stencilled twice. No plate. The lamp is lit on Ivy's schedule."}`
  - row 2: `{"requiredFlag":"mutation_km19_plated","siteId":"loc_cut_kilometre_19","stratumId":"after","text":"A brass plate sits over the second stencil, four screws, one steel. The lamp is still Ivy's."}`
  - row 3: `{"requiredFlag":"mutation_km19_plated","siteId":"loc_cut_kilometre_19","stratumId":"now","text":"CUT-19 reads in brass. The stencil underneath is colder. The lamp is still Ivy's."}`
  - row 4: `{"requiredFlag":"mutation_km19_scraped","siteId":"loc_cut_kilometre_19","stratumId":"now","text":"The plate is gone. The stencil read again in a day. Overlay is short one post."}`
  - row 5: `{"requiredFlag":"mutation_km19_palimpsest","siteId":"loc_cut_kilometre_19","stratumId":"now","text":"Brass over stencil, both visible. The post means two things, which is already its job."}`
  - row 6: `{"requiredFlag":"","siteId":"loc_transit_authority_hq","stratumId":"pre","text":"The lobby timetable is pre-Exchange and wrong. The grease pencil is the document."}`
  - row 7: `{"requiredFlag":"mutation_transit_maps","siteId":"loc_transit_authority_hq","stratumId":"now","text":"Overlay print over glass. The hand is paler underneath. HELD is not in the font."}`
  - row 8: `{"requiredFlag":"","siteId":"loc_municipal_archive","stratumId":"pre","text":"Rolling stacks, most off their rails. The Garrison looked twice."}`
  - row 9: `{"requiredFlag":"mutation_archive_dug","siteId":"loc_municipal_archive","stratumId":"now","text":"A hole in the grey brick. If an index came out, it is on a desk somewhere."}`
  - row 10: `{"requiredFlag":"mutation_archive_sunk","siteId":"loc_municipal_archive","stratumId":"now","text":"The half-sunk plate still shows REC-. The index stays in the geology."}`
  - row 11: `{"requiredFlag":"","siteId":"location_ministry_of_truth_bunker","stratumId":"pre","text":"The corridors still believe in authenticators."}`
  - row 12: `{"requiredFlag":"mutation_ministry_recast","siteId":"location_ministry_of_truth_bunker","stratumId":"now","text":"The book is out, or copy, or refused. The visited column is honest."}`
  - row 13: `{"requiredFlag":"","siteId":"loc_weighbridge","stratumId":"pre","text":"The needle has hairline cracks and a stop painted by someone tired of replacing it."}`
  - row 14: `{"requiredFlag":"mutation_weigh_lots","siteId":"loc_weighbridge","stratumId":"now","text":"Overlay lots on the beam. District 8 can pathfind by number. The needle has no opinion."}`
  - row 15: `{"requiredFlag":"mutation_weigh_mass_only","siteId":"loc_weighbridge","stratumId":"now","text":"Mass only. Osric still writes kilograms."}`
  - row 16: `{"requiredFlag":"","siteId":"loc_grange_hall","stratumId":"pre","text":"A long table, scarred, benches both sides. Oil lamps with glass intact."}`
  - row 17: `{"requiredFlag":"mutation_verge_names","siteId":"loc_grange_hall","stratumId":"now","text":"The ledger names people. The cross is still in it. The notice is face-down."}`
  - row 18: `{"requiredFlag":"","siteId":"loc_bridge_seven","stratumId":"pre","text":"Four lanes narrowing to a booth that is not always staffed."}`
  - row 19: `{"requiredFlag":"mutation_bridge_listed","siteId":"loc_bridge_seven","stratumId":"now","text":"A clipboard calls it a friendly obstacle. D/9 marked the rail."}`
  - row 20: `{"requiredFlag":"mutation_bridge_disturbed","siteId":"loc_bridge_seven","stratumId":"now","text":"The housing was checked. The Tollman does not need to be present for the authority to work."}`
  - row 21: `{"requiredFlag":"","siteId":"loc_lock_gate_four","stratumId":"pre","text":"A gate frozen mid-cycle. Water-heights, dates."}`
  - row 22: `{"requiredFlag":"mutation_lock_complete_lie","siteId":"loc_lock_gate_four","stratumId":"now","text":"COMPLETE brass on the leaf. The leaf is still where it stopped."}`
  - row 23: `{"requiredFlag":"mutation_lock_gauges_filed","siteId":"loc_lock_gate_four","stratumId":"now","text":"The gauges are filed. They believe the failure."}`
  - row 24: `{"requiredFlag":"mutation_lock_plate_down","siteId":"loc_lock_gate_four","stratumId":"now","text":"The plate is down. Benno's gauges are the honest document."}`
  - row 25: `{"requiredFlag":"","siteId":"loc_alloc_12b","stratumId":"pre","text":"Fallback designation. Chalk marks: fourteen, a gap, six."}`
  - row 26: `{"requiredFlag":"mutation_12b_address","siteId":"loc_alloc_12b","stratumId":"now","text":"Fresh stencil. 12-C can cite it as a pool. The kit is water."}`
  - row 27: `{"requiredFlag":"mutation_12b_kit_gone","siteId":"loc_alloc_12b","stratumId":"now","text":"The kit is gone. The stencil is a levy map."}`
  - row 28: `{"requiredFlag":"","siteId":"loc_pump_station_nine","stratumId":"pre","text":"A condemned tag on a room with one dry motor."}`
  - row 29: `{"requiredFlag":"mutation_pump_live","siteId":"loc_pump_station_nine","stratumId":"now","text":"One dry motor hums. The Drown lowers a step."}`
  - row 30: `{"requiredFlag":"mutation_pump_condemned","siteId":"loc_pump_station_nine","stratumId":"now","text":"Condemned stands. Sublevels stay shut."}`
  - row 31: `{"requiredFlag":"","siteId":"loc_records_annex","stratumId":"pre","text":"A window entry, a dusted room, a crate of plates string-tied."}`
  - row 32: `{"requiredFlag":"mutation_gazetteer_stands","siteId":"loc_records_annex","stratumId":"now","text":"The crate is open. Plates filed by number. Lived names are subtitles."}`
  - row 33: `{"requiredFlag":"mutation_gazetteer_lived","siteId":"loc_records_annex","stratumId":"now","text":"The crate stayed tied. Ostrowski's sheet is the one that matches the ground."}`
  - row 34: `{"requiredFlag":"","siteId":"location_the_memory_vault","stratumId":"pre","text":"Cotton gloves that are not yours. The second-copy cage is empty."}`
  - row 35: `{"requiredFlag":"mutation_gazetteer_stands","siteId":"location_the_memory_vault","stratumId":"now","text":"The cage holds the Record. Sole filed it. Two witnesses."}`
  - row 36: `{"requiredFlag":"mutation_gazetteer_lived","siteId":"location_the_memory_vault","stratumId":"now","text":"The cage holds the lived map. Sole filed spoken names."}`
  - row 37: `{"requiredFlag":"mutation_gazetteer_palimpsest","siteId":"location_the_memory_vault","stratumId":"now","text":"Both copies in the cage. Nobody likes either."}`
  - row 38: `{"requiredFlag":"mutation_gazetteer_scraped","siteId":"location_the_memory_vault","stratumId":"now","text":"The cage is empty. Sole cannot complete ground."}`
  - row 39: `{"requiredFlag":"","siteId":"loc_excavation_command_vault","stratumId":"pre","text":"The blast-door log shows sixty-two authorized personnel logged into Sector 1 Command before the primary circuit broke."}`
  - row 40: `{"requiredFlag":"mutation_command_vault_breached","siteId":"loc_excavation_command_vault","stratumId":"after","text":"Torch cuts along the dog-bolts tell a different story: the door was sealed from the outside six days after the strike."}`
  - row 41: `{"requiredFlag":"mutation_command_vault_breached","siteId":"loc_excavation_command_vault","stratumId":"now","text":"The sump contains only spent brass and empty emergency ration tins. No bodies remained in the command suite."}`
  - row 42: `{"requiredFlag":"","siteId":"loc_excavation_metro_interchange","stratumId":"pre","text":"The platform display board was stuck on 08:42 EXPR 4-NORTH. The conductor clipboard hangs on the stationmaster hook."}`
  - row 43: `{"requiredFlag":"mutation_metro_dossier_found","siteId":"loc_excavation_metro_interchange","stratumId":"now","text":"The third rail was cut manually with a hacksaw, stranding Train 14 inside the tunnel before the shockwave hit."}`
  - row 44: `{"requiredFlag":"","siteId":"loc_excavation_mine_shaft","stratumId":"pre","text":"Shift brass tokens on the pegboard show thirty-four miners underground when the power failure sirens triggered."}`
  - row 45: `{"requiredFlag":"mutation_mine_shaft_cleared","siteId":"loc_excavation_mine_shaft","stratumId":"now","text":"A makeshift barricade of timber cribbing and coal sacks blocked Adit 4 from the inside, marked: DO NOT OPEN - GAS."}`
  - row 46: `{"requiredFlag":"","siteId":"loc_excavation_archive_bunker","stratumId":"pre","text":"The filing system in Vault 3 used red linen ribbons for emergency wartime directives and green for civil rationing."}`
  - row 47: `{"requiredFlag":"mutation_archive_index_scraped","siteId":"loc_excavation_archive_bunker","stratumId":"after","text":"Every drawer containing letters D through G was incinerated with magnesium thermite before the facility was abandoned."}`
  - row 48: `{"requiredFlag":"","siteId":"loc_lock_gate_four","stratumId":"pre","text":"The bronze tidal gauge recorded a sudden two-metre surge twenty minutes after the coastal detonations."}`
  - row 49: `{"requiredFlag":"mutation_lock4_sluice_repaired","siteId":"loc_lock_gate_four","stratumId":"now","text":"The sluice counterweight was wedged open with an iron crowbar bearing the mark of the Municipal Water Board."}`
  - row 50: `{"requiredFlag":"","siteId":"loc_seed_library_annex","stratumId":"pre","text":"Seed vault climate logs were maintained in green ink until Day 14, when the compressor diesel ran dry."}`
  - row 51: `{"requiredFlag":"mutation_seed_annex_cataloged","siteId":"loc_seed_library_annex","stratumId":"now","text":"Canister 44 was carefully emptied and refilled with common rye grain, the original wheat seed smuggled north."}`
  - row 52: `{"requiredFlag":"","siteId":"loc_cold_store_atlantic","stratumId":"now","text":"Beneath the ice in Sub-level 2, forty crates of freeze-dried penicillin sit marked: FOR EMERGENCY USE BY DISTRICT 8 ONLY."}`
- Bytes: 11,295; SHA-256: `5b0cf181260669fa6f60465c1ccdcb5269fed6942754d8248f68bb1b17621205`
- Root keys: `items, schema_version`
- `items`: list[52]; union fields: `requiredFlag, siteId, stratumId, text`
  - row 1: `{"requiredFlag":"","siteId":"loc_cut_kilometre_19","stratumId":"pre","text":"The post is Lamplighter orange, the kilometre stencilled twice. No plate. The lamp is lit on Ivy's schedule."}`
  - row 2: `{"requiredFlag":"mutation_km19_plated","siteId":"loc_cut_kilometre_19","stratumId":"after","text":"A brass plate sits over the second stencil, four screws, one steel. The lamp is still Ivy's."}`
  - row 3: `{"requiredFlag":"mutation_km19_plated","siteId":"loc_cut_kilometre_19","stratumId":"now","text":"CUT-19 reads in brass. The stencil underneath is colder. The lamp is still Ivy's."}`
  - row 4: `{"requiredFlag":"mutation_km19_scraped","siteId":"loc_cut_kilometre_19","stratumId":"now","text":"The plate is gone. The stencil read again in a day. Overlay is short one post."}`
  - row 5: `{"requiredFlag":"mutation_km19_palimpsest","siteId":"loc_cut_kilometre_19","stratumId":"now","text":"Brass over stencil, both visible. The post means two things, which is already its job."}`
  - row 6: `{"requiredFlag":"","siteId":"loc_transit_authority_hq","stratumId":"pre","text":"The lobby timetable is pre-Exchange and wrong. The grease pencil is the document."}`
  - row 7: `{"requiredFlag":"mutation_transit_maps","siteId":"loc_transit_authority_hq","stratumId":"now","text":"Overlay print over glass. The hand is paler underneath. HELD is not in the font."}`
  - row 8: `{"requiredFlag":"","siteId":"loc_municipal_archive","stratumId":"pre","text":"Rolling stacks, most off their rails. The Garrison looked twice."}`
  - row 9: `{"requiredFlag":"mutation_archive_dug","siteId":"loc_municipal_archive","stratumId":"now","text":"A hole in the grey brick. If an index came out, it is on a desk somewhere."}`
  - row 10: `{"requiredFlag":"mutation_archive_sunk","siteId":"loc_municipal_archive","stratumId":"now","text":"The half-sunk plate still shows REC-. The index stays in the geology."}`
  - row 11: `{"requiredFlag":"","siteId":"location_ministry_of_truth_bunker","stratumId":"pre","text":"The corridors still believe in authenticators."}`
  - row 12: `{"requiredFlag":"mutation_ministry_recast","siteId":"location_ministry_of_truth_bunker","stratumId":"now","text":"The book is out, or copy, or refused. The visited column is honest."}`
  - row 13: `{"requiredFlag":"","siteId":"loc_weighbridge","stratumId":"pre","text":"The needle has hairline cracks and a stop painted by someone tired of replacing it."}`
  - row 14: `{"requiredFlag":"mutation_weigh_lots","siteId":"loc_weighbridge","stratumId":"now","text":"Overlay lots on the beam. District 8 can pathfind by number. The needle has no opinion."}`
  - row 15: `{"requiredFlag":"mutation_weigh_mass_only","siteId":"loc_weighbridge","stratumId":"now","text":"Mass only. Osric still writes kilograms."}`
  - row 16: `{"requiredFlag":"","siteId":"loc_grange_hall","stratumId":"pre","text":"A long table, scarred, benches both sides. Oil lamps with glass intact."}`
  - row 17: `{"requiredFlag":"mutation_verge_names","siteId":"loc_grange_hall","stratumId":"now","text":"The ledger names people. The cross is still in it. The notice is face-down."}`
  - row 18: `{"requiredFlag":"","siteId":"loc_bridge_seven","stratumId":"pre","text":"Four lanes narrowing to a booth that is not always staffed."}`
  - row 19: `{"requiredFlag":"mutation_bridge_listed","siteId":"loc_bridge_seven","stratumId":"now","text":"A clipboard calls it a friendly obstacle. D/9 marked the rail."}`
  - row 20: `{"requiredFlag":"mutation_bridge_disturbed","siteId":"loc_bridge_seven","stratumId":"now","text":"The housing was checked. The Tollman does not need to be present for the authority to work."}`
  - row 21: `{"requiredFlag":"","siteId":"loc_lock_gate_four","stratumId":"pre","text":"A gate frozen mid-cycle. Water-heights, dates."}`
  - row 22: `{"requiredFlag":"mutation_lock_complete_lie","siteId":"loc_lock_gate_four","stratumId":"now","text":"COMPLETE brass on the leaf. The leaf is still where it stopped."}`
  - row 23: `{"requiredFlag":"mutation_lock_gauges_filed","siteId":"loc_lock_gate_four","stratumId":"now","text":"The gauges are filed. They believe the failure."}`
  - row 24: `{"requiredFlag":"mutation_lock_plate_down","siteId":"loc_lock_gate_four","stratumId":"now","text":"The plate is down. Benno's gauges are the honest document."}`
  - row 25: `{"requiredFlag":"","siteId":"loc_alloc_12b","stratumId":"pre","text":"Fallback designation. Chalk marks: fourteen, a gap, six."}`
  - row 26: `{"requiredFlag":"mutation_12b_address","siteId":"loc_alloc_12b","stratumId":"now","text":"Fresh stencil. 12-C can cite it as a pool. The kit is water."}`
  - row 27: `{"requiredFlag":"mutation_12b_kit_gone","siteId":"loc_alloc_12b","stratumId":"now","text":"The kit is gone. The stencil is a levy map."}`
  - row 28: `{"requiredFlag":"","siteId":"loc_pump_station_nine","stratumId":"pre","text":"A condemned tag on a room with one dry motor."}`
  - row 29: `{"requiredFlag":"mutation_pump_live","siteId":"loc_pump_station_nine","stratumId":"now","text":"One dry motor hums. The Drown lowers a step."}`
  - row 30: `{"requiredFlag":"mutation_pump_condemned","siteId":"loc_pump_station_nine","stratumId":"now","text":"Condemned stands. Sublevels stay shut."}`
  - row 31: `{"requiredFlag":"","siteId":"loc_records_annex","stratumId":"pre","text":"A window entry, a dusted room, a crate of plates string-tied."}`
  - row 32: `{"requiredFlag":"mutation_gazetteer_stands","siteId":"loc_records_annex","stratumId":"now","text":"The crate is open. Plates filed by number. Lived names are subtitles."}`
  - row 33: `{"requiredFlag":"mutation_gazetteer_lived","siteId":"loc_records_annex","stratumId":"now","text":"The crate stayed tied. Ostrowski's sheet is the one that matches the ground."}`
  - row 34: `{"requiredFlag":"","siteId":"location_the_memory_vault","stratumId":"pre","text":"Cotton gloves that are not yours. The second-copy cage is empty."}`
  - row 35: `{"requiredFlag":"mutation_gazetteer_stands","siteId":"location_the_memory_vault","stratumId":"now","text":"The cage holds the Record. Sole filed it. Two witnesses."}`
  - row 36: `{"requiredFlag":"mutation_gazetteer_lived","siteId":"location_the_memory_vault","stratumId":"now","text":"The cage holds the lived map. Sole filed spoken names."}`
  - row 37: `{"requiredFlag":"mutation_gazetteer_palimpsest","siteId":"location_the_memory_vault","stratumId":"now","text":"Both copies in the cage. Nobody likes either."}`
  - row 38: `{"requiredFlag":"mutation_gazetteer_scraped","siteId":"location_the_memory_vault","stratumId":"now","text":"The cage is empty. Sole cannot complete ground."}`
  - row 39: `{"requiredFlag":"","siteId":"loc_excavation_command_vault","stratumId":"pre","text":"The blast-door log shows sixty-two authorized personnel logged into Sector 1 Command before the primary circuit broke."}`
  - row 40: `{"requiredFlag":"mutation_command_vault_breached","siteId":"loc_excavation_command_vault","stratumId":"after","text":"Torch cuts along the dog-bolts tell a different story: the door was sealed from the outside six days after the strike."}`
  - row 41: `{"requiredFlag":"mutation_command_vault_breached","siteId":"loc_excavation_command_vault","stratumId":"now","text":"The sump contains only spent brass and empty emergency ration tins. No bodies remained in the command suite."}`
  - row 42: `{"requiredFlag":"","siteId":"loc_excavation_metro_interchange","stratumId":"pre","text":"The platform display board was stuck on 08:42 EXPR 4-NORTH. The conductor clipboard hangs on the stationmaster hook."}`
  - row 43: `{"requiredFlag":"mutation_metro_dossier_found","siteId":"loc_excavation_metro_interchange","stratumId":"now","text":"The third rail was cut manually with a hacksaw, stranding Train 14 inside the tunnel before the shockwave hit."}`
  - row 44: `{"requiredFlag":"","siteId":"loc_excavation_mine_shaft","stratumId":"pre","text":"Shift brass tokens on the pegboard show thirty-four miners underground when the power failure sirens triggered."}`
  - row 45: `{"requiredFlag":"mutation_mine_shaft_cleared","siteId":"loc_excavation_mine_shaft","stratumId":"now","text":"A makeshift barricade of timber cribbing and coal sacks blocked Adit 4 from the inside, marked: DO NOT OPEN - GAS."}`
  - row 46: `{"requiredFlag":"","siteId":"loc_excavation_archive_bunker","stratumId":"pre","text":"The filing system in Vault 3 used red linen ribbons for emergency wartime directives and green for civil rationing."}`
  - row 47: `{"requiredFlag":"mutation_archive_index_scraped","siteId":"loc_excavation_archive_bunker","stratumId":"after","text":"Every drawer containing letters D through G was incinerated with magnesium thermite before the facility was abandoned."}`
  - row 48: `{"requiredFlag":"","siteId":"loc_lock_gate_four","stratumId":"pre","text":"The bronze tidal gauge recorded a sudden two-metre surge twenty minutes after the coastal detonations."}`
  - row 49: `{"requiredFlag":"mutation_lock4_sluice_repaired","siteId":"loc_lock_gate_four","stratumId":"now","text":"The sluice counterweight was wedged open with an iron crowbar bearing the mark of the Municipal Water Board."}`
  - row 50: `{"requiredFlag":"","siteId":"loc_seed_library_annex","stratumId":"pre","text":"Seed vault climate logs were maintained in green ink until Day 14, when the compressor diesel ran dry."}`
  - row 51: `{"requiredFlag":"mutation_seed_annex_cataloged","siteId":"loc_seed_library_annex","stratumId":"now","text":"Canister 44 was carefully emptied and refilled with common rye grain, the original wheat seed smuggled north."}`
  - row 52: `{"requiredFlag":"","siteId":"loc_cold_store_atlantic","stratumId":"now","text":"Beneath the ice in Sub-level 2, forty crates of freeze-dried penicillin sit marked: FOR EMERGENCY USE BY DISTRICT 8 ONLY."}`

## `Assets/StreamingAssets/Data/standing_record_layouts.json`
- Bytes: 62,607; SHA-256: `be7c35c41115e688179380aa200c4f9508130d079302a55f24d27802b19a4b6a`
- Root keys: `items, schema_version`
- `items`: list[14]; union fields: `displayName, parentLocationId, rooms`
  - row 1: `{"displayName":"Kilometre 19","parentLocationId":"loc_cut_kilometre_19","rooms":[{"adjacent":["room_km19_seam","room_km19_oil_tin"],"description":"The reflector post is still Lamplighter orange, the kilometre stencilled twice because the first pass ran. Over the second stencil, a brass plate, municipal, four screws, stamped CUT-19 / LAMP. Three screws match. The fourth is steel, bright, a field repair. The lamp is lit on Ivy's schedule. The plate does not mention oil. A spirit-level leans against the base, bubble still between the lines, as if Maren set it down to argue with a post that was already vertical. You can take the plate. The stenc…`
  - row 2: `{"displayName":"Transit Authority","parentLocationId":"loc_transit_authority_hq","rooms":[{"adjacent":["room_transit_map_glass"],"description":"Civic linoleum, peeled to the mastic in a path from the doors to the inner glass. A ticket-disc dispenser, municipal, hopper empty, the last disc jammed edge-on in the slot: a blank. Someone tried to stamp it and the die was already gone. A clock over the inner doors is stopped at a time that matches no convoy slot under the glass. The cloak-rail still has one hanger, wire, twisted into a hook that will not hold a coat. You can take the blank disc. It authenticates nothing and Overlay will try to sta…`
  - row 3: `{"displayName":"Municipal Archive","parentLocationId":"loc_municipal_archive","rooms":[{"adjacent":["room_archive_grey_brick","room_archive_loading_dock"],"description":"Glass doors, one pane starred. A counter with a visitor book chained to a brass rail. Columns: date, unit, purpose. Two entries in a Garrison hand: SEARCH SCHEDULE and, months later, SEARCH SCHEDULE (REPEAT). Both purposes are the same and both results are nothing, written in the remarks as NOT MUNICIPAL. A third line has been started in Overlay pencil: FILE RECORD — no date, no name. A bottle of drying-sand for wet ink, fused into a lump. You can take the book. The chain wi…`
  - row 4: `{"displayName":"Ministry of Continuity","parentLocationId":"location_ministry_of_truth_bunker","rooms":[{"adjacent":["room_ministry_enquiry"],"description":"A stair that was designed to look like work. Civil-service cream, scuffed to the primer on the nosings. At the landing, an authenticator plate, Allocation-family, light still on, fuse-box beside it with a paper tag DO NOT REPLACE — STANDBY. Someone replaced it anyway; the fuse is newer than the tag. A brass handrail, unscrewed at the top and left hanging on the bottom bolts, as if a nameplate crew started and were called to a different job. You can take the hanging rail. It is fittings. …`
  - row 5: `{"displayName":"The Weighbridge","parentLocationId":"loc_weighbridge","rooms":[{"adjacent":["room_weigh_hut"],"description":"The truck scale, iron, a needle in a glass that has hairline cracks and a stop painted by someone tired of replacing it. The calibration weight marked 500 kg hangs on a hook, or does not, if Overlay borrowed brass. The plate of the scale is scored with tyre-tracks and one human boot-print, heel toward the hut, as if someone stood there to be priced. You can stand on it. Osric will write a mass. You can put an Overlay lot-plate on it. It will also be a mass. That is the joke the Warlords repeat. You can take the calibra…`
  - row 6: `{"displayName":"The Grange Hall","parentLocationId":"loc_grange_hall","rooms":[{"adjacent":["room_grange_table"],"description":"A porch with a hand-lettered sign asking visitors to leave weapons here. The sign has been relettered; the nail holes are a history of wording. A rifle already in the stand, tag on the trigger-guard with a name that has a cross in the ledger inside. Oil lamp unlit; daylight is enough. You can leave a weapon. You can take the tagged rifle. The ledger will still have the cross. You can refuse the sign and walk in armed. The Verge will notice with a show of hands later, not now. Overlay has not plated the sign. They pl…`
  - row 7: `{"displayName":"The Allotments","parentLocationId":"loc_the_allotments","rooms":[{"adjacent":["room_allot_hut"],"description":"A municipal gate, chain-link, cut once at hip height and rewired with brass bell-wire that Frayne would rather see in a valve. A padlock that is ceremonial; the cut is the door. Through the mesh: numbered plots, a hut, a noticeboard in a plastic sleeve. Overlay plate on the gatepost, or a clean rectangle of less-weathered wire where a plate was. You can take the bell-wire. A leak somewhere gets a schedule. You can screw a plate. You can refuse the padlock's theatre and use the cut. Dara Mewn, if she is not on levy, w…`
  - row 8: `{"displayName":"Bridge Seven","parentLocationId":"loc_bridge_seven","rooms":[{"adjacent":["room_bridge_span"],"description":"Toll-side approach, four lanes narrowing to a booth that is not always staffed because the charges staff it. A spike of receipts, transfixed, rust and paper. A stone on the marked side of a scupper — D/9 grammar, do not move it. You can take a receipt. It will say mass or bullets depending on the week. You can move the stone. That is not mercy. You can refuse the booth and walk the span. The Tollman does not need to be present for the authority to work.","displayName":"Near Bank","id":"room_bridge_near","inspect":"A sp…`
  - row 9: `{"displayName":"Bus Reversal Loop","parentLocationId":"loc_bus_reversal_loop","rooms":[{"adjacent":["room_bus_lead","room_bus_office"],"description":"Tarmac, painted reversal arrows faded to ghosts. Buses packed as if the order were still being obeyed. Between two wheels: a child's suitcase, empty, a luggage-label with a street name in a household hand, no Allocation number. You can take the suitcase. It will not fill. You can leave it. Overlay will plate the circle EVAC COMPLETE without opening luggage. You can weigh the label. It is a lived name. Ira's lived-name column would take it if you carried it to her, and she would still want the s…`
  - row 10: `{"displayName":"Lock Gate Four","parentLocationId":"loc_lock_gate_four","rooms":[{"adjacent":["room_lock_control"],"description":"Stone edge, Drown-water at a height Benno can tell you by month. Mooring ring, iron, the inner face polished by rope. Overlay stakes along the path, or pulled. A life-ring with the municipal name of a lock that still has a name even when Overlay wants RECLAMATION 4-W. You can take the ring. Nomi's launch will have to use a bollard. You can take the life-ring. It has never been thrown. You can refuse to call the water a completed reclamation from the path. You cannot see the leaf angle until the next room.","displa…`
  - row 11: `{"displayName":"Pump Station Nine","parentLocationId":"loc_pump_station_nine","rooms":[{"adjacent":["room_pump_hall"],"description":"Water to the lintel. A boat-hook scratch on brick. Bilge pole standing in a drum, notches for depth, the same pencil logic as the lock chart. You can take the pole. The next sounding will be guessed. You can refuse to step off until Nomi's etiquette is done, if she brought you. Overlay tags start at the inner door, fluorescent, CONDEMNED.","displayName":"Boat Approach","id":"room_pump_approach","inspect":"A bilge pole with marks that are Benno's grammar, not Overlay's.","inspectKey":"","unlockRule":"entry"},{"a…`
  - row 12: `{"displayName":"Allocation 12-B","parentLocationId":"loc_alloc_12b","rooms":[{"adjacent":["room_12b_unprovisioned","room_12b_stencil"],"description":"Subway maintenance stair, stencil ALLOCATION 12-B faded. Chalk: fourteen marks, a gap, six. The gap is the story. A tin of chalk on the step, used. You can close the gap. Sela, if present, will leave the stair. Nila will hear if you treat overflow as a complete number. You can add a mark that is not a person. You can refuse and copy the gap for Sole. You can steal the chalk. The next count will be charcoal.","displayName":"Stair","id":"room_12b_stair","inspect":"Fourteen chalk marks, a gap, the…`
  - row 13: `{"displayName":"Records Annex","parentLocationId":"loc_records_annex","rooms":[{"adjacent":["room_annex_dusted"],"description":"A window that is a door. Sill worn by keels and knees. Hook scars in the plaster. Inside, a mat that was a curtain. You can steal the curtain-mat. The next arrival will wet the dusted room. You can refuse to board anyone else's hull in sight of this window. Nomi's etiquette holds. Overlay crates do not get a second explanation.","displayName":"Window Entry","id":"room_annex_window","inspect":"Second storey. Boat-hook scars. Dry above the waterline.","inspectKey":"","unlockRule":"entry"},{"adjacent":["room_annex_wind…`
  - row 14: `{"displayName":"The Memory Vault","parentLocationId":"location_the_memory_vault","rooms":[{"adjacent":["room_vault_airlock"],"description":"A dock that was a loading bay. Mooring, a scum line on the wall. If the dry motor was energised, the line is a handspan above the water and there is a wet band the colour of old tea. If not, you step down. Nomi will not board another hull. Overlay plates in the next room do not get to pick a side on the dock. You can steal a mooring wedge. You can refuse to help Overlay unload. You can measure the scum with Benno's grammar.","displayName":"Dock","id":"room_vault_dock","inspect":"Mooring. Etiquette. If Pu…`
- Bytes: 62,607; SHA-256: `be7c35c41115e688179380aa200c4f9508130d079302a55f24d27802b19a4b6a`
- Root keys: `items, schema_version`
- `items`: list[14]; union fields: `displayName, parentLocationId, rooms`
  - row 1: `{"displayName":"Kilometre 19","parentLocationId":"loc_cut_kilometre_19","rooms":[{"adjacent":["room_km19_seam","room_km19_oil_tin"],"description":"The reflector post is still Lamplighter orange, the kilometre stencilled twice because the first pass ran. Over the second stencil, a brass plate, municipal, four screws, stamped CUT-19 / LAMP. Three screws match. The fourth is steel, bright, a field repair. The lamp is lit on Ivy's schedule. The plate does not mention oil. A spirit-level leans against the base, bubble still between the lines, as if Maren set it down to argue with a post that was already vertical. You can take the plate. The stenc…`
  - row 2: `{"displayName":"Transit Authority","parentLocationId":"loc_transit_authority_hq","rooms":[{"adjacent":["room_transit_map_glass"],"description":"Civic linoleum, peeled to the mastic in a path from the doors to the inner glass. A ticket-disc dispenser, municipal, hopper empty, the last disc jammed edge-on in the slot: a blank. Someone tried to stamp it and the die was already gone. A clock over the inner doors is stopped at a time that matches no convoy slot under the glass. The cloak-rail still has one hanger, wire, twisted into a hook that will not hold a coat. You can take the blank disc. It authenticates nothing and Overlay will try to sta…`
  - row 3: `{"displayName":"Municipal Archive","parentLocationId":"loc_municipal_archive","rooms":[{"adjacent":["room_archive_grey_brick","room_archive_loading_dock"],"description":"Glass doors, one pane starred. A counter with a visitor book chained to a brass rail. Columns: date, unit, purpose. Two entries in a Garrison hand: SEARCH SCHEDULE and, months later, SEARCH SCHEDULE (REPEAT). Both purposes are the same and both results are nothing, written in the remarks as NOT MUNICIPAL. A third line has been started in Overlay pencil: FILE RECORD — no date, no name. A bottle of drying-sand for wet ink, fused into a lump. You can take the book. The chain wi…`
  - row 4: `{"displayName":"Ministry of Continuity","parentLocationId":"location_ministry_of_truth_bunker","rooms":[{"adjacent":["room_ministry_enquiry"],"description":"A stair that was designed to look like work. Civil-service cream, scuffed to the primer on the nosings. At the landing, an authenticator plate, Allocation-family, light still on, fuse-box beside it with a paper tag DO NOT REPLACE — STANDBY. Someone replaced it anyway; the fuse is newer than the tag. A brass handrail, unscrewed at the top and left hanging on the bottom bolts, as if a nameplate crew started and were called to a different job. You can take the hanging rail. It is fittings. …`
  - row 5: `{"displayName":"The Weighbridge","parentLocationId":"loc_weighbridge","rooms":[{"adjacent":["room_weigh_hut"],"description":"The truck scale, iron, a needle in a glass that has hairline cracks and a stop painted by someone tired of replacing it. The calibration weight marked 500 kg hangs on a hook, or does not, if Overlay borrowed brass. The plate of the scale is scored with tyre-tracks and one human boot-print, heel toward the hut, as if someone stood there to be priced. You can stand on it. Osric will write a mass. You can put an Overlay lot-plate on it. It will also be a mass. That is the joke the Warlords repeat. You can take the calibra…`
  - row 6: `{"displayName":"The Grange Hall","parentLocationId":"loc_grange_hall","rooms":[{"adjacent":["room_grange_table"],"description":"A porch with a hand-lettered sign asking visitors to leave weapons here. The sign has been relettered; the nail holes are a history of wording. A rifle already in the stand, tag on the trigger-guard with a name that has a cross in the ledger inside. Oil lamp unlit; daylight is enough. You can leave a weapon. You can take the tagged rifle. The ledger will still have the cross. You can refuse the sign and walk in armed. The Verge will notice with a show of hands later, not now. Overlay has not plated the sign. They pl…`
  - row 7: `{"displayName":"The Allotments","parentLocationId":"loc_the_allotments","rooms":[{"adjacent":["room_allot_hut"],"description":"A municipal gate, chain-link, cut once at hip height and rewired with brass bell-wire that Frayne would rather see in a valve. A padlock that is ceremonial; the cut is the door. Through the mesh: numbered plots, a hut, a noticeboard in a plastic sleeve. Overlay plate on the gatepost, or a clean rectangle of less-weathered wire where a plate was. You can take the bell-wire. A leak somewhere gets a schedule. You can screw a plate. You can refuse the padlock's theatre and use the cut. Dara Mewn, if she is not on levy, w…`
  - row 8: `{"displayName":"Bridge Seven","parentLocationId":"loc_bridge_seven","rooms":[{"adjacent":["room_bridge_span"],"description":"Toll-side approach, four lanes narrowing to a booth that is not always staffed because the charges staff it. A spike of receipts, transfixed, rust and paper. A stone on the marked side of a scupper — D/9 grammar, do not move it. You can take a receipt. It will say mass or bullets depending on the week. You can move the stone. That is not mercy. You can refuse the booth and walk the span. The Tollman does not need to be present for the authority to work.","displayName":"Near Bank","id":"room_bridge_near","inspect":"A sp…`
  - row 9: `{"displayName":"Bus Reversal Loop","parentLocationId":"loc_bus_reversal_loop","rooms":[{"adjacent":["room_bus_lead","room_bus_office"],"description":"Tarmac, painted reversal arrows faded to ghosts. Buses packed as if the order were still being obeyed. Between two wheels: a child's suitcase, empty, a luggage-label with a street name in a household hand, no Allocation number. You can take the suitcase. It will not fill. You can leave it. Overlay will plate the circle EVAC COMPLETE without opening luggage. You can weigh the label. It is a lived name. Ira's lived-name column would take it if you carried it to her, and she would still want the s…`
  - row 10: `{"displayName":"Lock Gate Four","parentLocationId":"loc_lock_gate_four","rooms":[{"adjacent":["room_lock_control"],"description":"Stone edge, Drown-water at a height Benno can tell you by month. Mooring ring, iron, the inner face polished by rope. Overlay stakes along the path, or pulled. A life-ring with the municipal name of a lock that still has a name even when Overlay wants RECLAMATION 4-W. You can take the ring. Nomi's launch will have to use a bollard. You can take the life-ring. It has never been thrown. You can refuse to call the water a completed reclamation from the path. You cannot see the leaf angle until the next room.","displa…`
  - row 11: `{"displayName":"Pump Station Nine","parentLocationId":"loc_pump_station_nine","rooms":[{"adjacent":["room_pump_hall"],"description":"Water to the lintel. A boat-hook scratch on brick. Bilge pole standing in a drum, notches for depth, the same pencil logic as the lock chart. You can take the pole. The next sounding will be guessed. You can refuse to step off until Nomi's etiquette is done, if she brought you. Overlay tags start at the inner door, fluorescent, CONDEMNED.","displayName":"Boat Approach","id":"room_pump_approach","inspect":"A bilge pole with marks that are Benno's grammar, not Overlay's.","inspectKey":"","unlockRule":"entry"},{"a…`
  - row 12: `{"displayName":"Allocation 12-B","parentLocationId":"loc_alloc_12b","rooms":[{"adjacent":["room_12b_unprovisioned","room_12b_stencil"],"description":"Subway maintenance stair, stencil ALLOCATION 12-B faded. Chalk: fourteen marks, a gap, six. The gap is the story. A tin of chalk on the step, used. You can close the gap. Sela, if present, will leave the stair. Nila will hear if you treat overflow as a complete number. You can add a mark that is not a person. You can refuse and copy the gap for Sole. You can steal the chalk. The next count will be charcoal.","displayName":"Stair","id":"room_12b_stair","inspect":"Fourteen chalk marks, a gap, the…`
  - row 13: `{"displayName":"Records Annex","parentLocationId":"loc_records_annex","rooms":[{"adjacent":["room_annex_dusted"],"description":"A window that is a door. Sill worn by keels and knees. Hook scars in the plaster. Inside, a mat that was a curtain. You can steal the curtain-mat. The next arrival will wet the dusted room. You can refuse to board anyone else's hull in sight of this window. Nomi's etiquette holds. Overlay crates do not get a second explanation.","displayName":"Window Entry","id":"room_annex_window","inspect":"Second storey. Boat-hook scars. Dry above the waterline.","inspectKey":"","unlockRule":"entry"},{"adjacent":["room_annex_wind…`
  - row 14: `{"displayName":"The Memory Vault","parentLocationId":"location_the_memory_vault","rooms":[{"adjacent":["room_vault_airlock"],"description":"A dock that was a loading bay. Mooring, a scum line on the wall. If the dry motor was energised, the line is a handspan above the water and there is a wet band the colour of old tea. If not, you step down. Nomi will not board another hull. Overlay plates in the next room do not get to pick a side on the dock. You can steal a mooring wedge. You can refuse to help Overlay unload. You can measure the scum with Benno's grammar.","displayName":"Dock","id":"room_vault_dock","inspect":"Mooring. Etiquette. If Pu…`

# Appendix D — Current caller/reference graph

### `StandingRecordHostSession` (13 sampled current references)
- src/Main.PlayerSurfaces.cs:677: bindAction: () => { _standingRecordHostSession ??= StandingRecordHostSession.Create(_dataDir); _standingRecordAtlasPanel.Bind(_standingRecordHostSession); },
- src/Main.PlayerSurfaces.cs:1139: private StandingRecordHostSession? _standingRecordHostSession;
- src/Host/StandingRecordHostSession.cs:18: public sealed class StandingRecordHostSession
- src/Host/StandingRecordHostSession.cs:25: public static StandingRecordHostSession Create(string dataDir)
- src/Host/StandingRecordHostSession.cs:27: return new StandingRecordHostSession(dataDir, seed: DefaultSeed);
- src/Host/StandingRecordHostSession.cs:30: public static StandingRecordHostSession Create(string dataDir, int seed)
- src/Host/StandingRecordHostSession.cs:32: return new StandingRecordHostSession(dataDir, seed);
- src/Host/StandingRecordHostSession.cs:35: private StandingRecordHostSession(string? dataDir, int seed)
- src/UI/StandingRecordAtlasPanel.cs:18: /// Reads the user's own <see cref="StandingRecordEngine"/> (Core) through <see cref="StandingRecordHostSession"/>.
- src/UI/StandingRecordAtlasPanel.cs:44: private StandingRecordHostSession? _host;
- src/UI/StandingRecordAtlasPanel.cs:48: public void Bind(StandingRecordHostSession host)
- src/UI/StandingRecordAtlasPanel.cs:176: "Bind a StandingRecordHostSession to see live ground layouts and strata.", autowrap: true));
- src/UI/StandingRecordAtlasPanel.cs:374: "Standing Record engine offline. Bind a StandingRecordHostSession to see live ground layouts and strata.", autowrap: true));
### `StandingRecordCatalogLoader` (18 sampled current references)
- Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs:84: public sealed class StandingRecordCatalogLoader
- Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs:93: public StandingRecordCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
- Assets/Ashfall.Core/StandingRecord/StandingRecordHeadlessDemo.cs:112: var catLoader = new StandingRecordCatalogLoader(files, json, log);
- src/Host/ExpansionHostSession.cs:141: var quests = new StandingRecordCatalogLoader(files, json, log).Load(dataDirectory);
- src/Host/HostCli.ExpansionDepth.cs:48: var standingRecordCat = new StandingRecordCatalogLoader(files, json, NullLog.Instance).Load(dataDirectory);
- Ashfall.Core.Tests/StandingRecordQuestExpansionTests.cs:70: var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/StandingRecordSystemTests.cs:178: var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/StandingRecordSystemTests.cs:197: var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/StandingRecordSystemTests.cs:218: var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/StandingRecordSystemTests.cs:231: var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs:58: var loader = new StandingRecordCatalogLoader(files, json);
- Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs:158: var standingLoader = new StandingRecordCatalogLoader(files, json);
- Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs:192: var standingLoader = new StandingRecordCatalogLoader(files, json);
- Ashfall.Core.Tests/Expansions/Plan18ExpansionDeepeningTests.cs:56: var srCat = new StandingRecordCatalogLoader(files, json, NullLog.Instance).Load(dataDir);
- Ashfall.Core.Tests/Expansions/Plan18ExpansionDeepeningTests.cs:156: var sr = new StandingRecordCatalogLoader(files, json, NullLog.Instance).Load(dataDir);
- Ashfall.Core.Tests/Governance/Plan89_98MusterFactionIntegrationTests.cs:73: public void Plan98_StandingRecordCatalogLoader_LoadsAll8Factions_WithCompleteDossiers()
- Ashfall.Core.Tests/Governance/Plan89_98MusterFactionIntegrationTests.cs:78: var loader = new StandingRecordCatalogLoader(io, json);
- Ashfall.Core.Tests/Governance/Plan89_98MusterFactionIntegrationTests.cs:127: var loader = new StandingRecordCatalogLoader(io, json);
### `StandingRecordEngine` (18 sampled current references)
- Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs:15: public string systemId = StandingRecordEngine.SystemId;
- Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs:31: public sealed class StandingRecordEngine
- Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs:47: public StandingRecordEngine(
- src/Host/StandingRecordHostSession.cs:11: /// Wraps the unified engine-agnostic StandingRecordEngine around the
- src/Host/StandingRecordHostSession.cs:22: public StandingRecordEngine Engine { get; }
- src/Host/StandingRecordHostSession.cs:42: Engine = new StandingRecordEngine(
- src/Host/StandingRecordHostSession.cs:116: systemId = StandingRecordEngine.SystemId,
- src/Host/StandingRecordHostSession.cs:132: /// Mirrors the unified-state shape required by StandingRecordEngine.
- src/Host/StandingRecordHostSession.cs:137: public string systemId = StandingRecordEngine.SystemId;
- src/UI/StandingRecordAtlasPanel.cs:18: /// Reads the user's own <see cref="StandingRecordEngine"/> (Core) through <see cref="StandingRecordHostSession"/>.
- src/UI/StandingRecordAtlasPanel.cs:229: // StandingRecordEngine surfaces 14 layouts, 38 strata, 38 mutation flags.
- Ashfall.Core.Tests/StandingRecordEngineTests.cs:10: /// Tests for <see cref="StandingRecordEngine"/> and the unified
- Ashfall.Core.Tests/StandingRecordEngineTests.cs:14: public class StandingRecordEngineTests
- Ashfall.Core.Tests/StandingRecordEngineTests.cs:29: private static StandingRecordEngine BuildEngine(StandingRecordState state = null)
- Ashfall.Core.Tests/StandingRecordEngineTests.cs:32: return new StandingRecordEngine(files, json, rng, log, state);
- Ashfall.Core.Tests/StandingRecordEngineTests.cs:60: Assert.True(engine.Memory.HasMutation(StandingRecordEngine.FlagExpUnlocked));
- Ashfall.Core.Tests/StandingRecordEngineTests.cs:137: var engine2 = new StandingRecordEngine(files, json, rng, log, saved);
- Ashfall.Core.Tests/StandingRecordQuestExpansionTests.cs:290: var engine = new StandingRecordEngine(files, json, rng);
### `CaptureSave` (18 sampled current references)
- Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs:295: public string CaptureSavePayload()
- Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs:328: string payload = CaptureSavePayload();
- src/Main.BlackMarket.cs:49: CaptureSection("black_market", BlackMarketSaveStore.TryCapturePersisted(_blackMarket.CaptureSave()));
- src/Main.DutyRoster.cs:200: if (CaptureSection("duty_roster", DutyRosterSaveStore.TryCapturePersisted(_dutyRoster.CaptureSave())))
- src/Main.Echoes.cs:50: if (CaptureSection(EchoSaveStore.SectionName, EchoSaveStore.TryCapturePersisted(_echoes.CaptureSave())))
- src/Main.HydraulicExtrusion.cs:117: HydraulicExtrusionSaveStore.TryCapturePersisted(_hydraulicExtrusion.CaptureSave()));
- src/Main.InSarMapping.cs:190: InSarMappingSaveStore.TryCapturePersisted(_inSarMapping.CaptureSave()));
- src/Main.LowBackgroundMetrology.cs:162: LowBackgroundMetrologySaveStore.TryCapturePersisted(_lowBackgroundMetrology.CaptureSave()));
- src/Main.MoraleContagion.cs:160: MoraleContagionSaveStore.TryCapturePersisted(_moraleContagion.CaptureSave()));
- src/Main.PsyOps.cs:105: PsyOpsSaveStore.TryCapturePersisted(_psyops.CaptureSave()));
- src/Main.RadioProgramProduction.cs:135: RadioProgramProductionSaveStore.TryCapturePersisted(_radioProgramProduction.CaptureSave()));
- src/Main.RunFlatTire.cs:136: RunFlatTireSaveStore.TryCapturePersisted(_runFlatTire.CaptureSave()));
- src/Main.Sanitation.cs:96: CaptureSection("sanitation", SanitationSaveStore.TryCapturePersisted(_sanitation.CaptureSave()));
- src/Main.Subterranean.cs:132: SubterraneanSaveStore.TryCapturePersisted(_subterranean.CaptureSave()));
- src/Main.Survivors.cs:305: if (CaptureSection("survivors", SurvivorsSaveStore.TryCapturePersisted(_survivors.CaptureSave())))
- src/Main.UiTests.Inventory.cs:90: var save = _inventory.CaptureSave();
- src/Main.UiTests.Survivors.cs:55: var save = _survivors.CaptureSave();
- src/Main.UiTests.WorkshopRelic.cs:129: var captured = _crafting.CaptureSave();
### `FactionStanceEngine` (18 sampled current references)
- Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs:18: /// faction standing via FactionStanceEngine, once-only journal keys, a
- Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs:205: var stances = new FactionStanceEngine();
- Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs:216: "exact fleet trust delta applied via FactionStanceEngine");
- Assets/Ashfall.Core/Economy/FactionStanceEngine.cs:13: public sealed class FactionStanceEngine : IFactionStanceProvider
- Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs:9: /// Black Flotilla, expressed entirely on the existing <see cref="FactionStanceEngine"/>
- Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs:25: // Thresholds on the existing FactionStanceEngine semantics.
- Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs:47: public static void Register(FactionStanceEngine engine)
- Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs:89: /// into the existing FactionStanceEngine (SetTrust on restore, ModifyTrust
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:953: ["faction_lore.json"] = new[] { "FactionStanceEngine", "FactionIconLoader" },
- Assets/Ashfall.Core/Spiritual/BeliefStanceBridge.cs:9: // standing lives in FactionStanceEngine — and the two had never been connected,
- Assets/Ashfall.Core/Spiritual/BeliefStanceBridge.cs:13: // FactionStanceEngine.ModifyTrust, which remains the single write path.
- src/Main.UiTests.Economy.cs:73: var stanceEngine = new FactionStanceEngine();
- src/Main.Zealotry.cs:52: Ashfall.Core.Economy.FactionStanceEngine? stance;
- src/Main.CampaignServices.cs:199: private Ashfall.Core.Economy.FactionStanceEngine? _sharedFactionStance;
- src/Main.CampaignServices.cs:201: public Ashfall.Core.Economy.FactionStanceEngine EnsureSharedFactionStance()
- src/Main.Economy.cs:233: // GAP-STUB-03 (resolved): wire the remaining FactionStanceEngine
- src/Economy/EconomyMarketPanel.cs:96: /// The provider is the existing FactionStanceEngine; no new authority.
- src/Host/DeepCoastHostSession.cs:20: /// keys), the FactionStanceEngine (fleet/office standing), the Holdfast

# Appendix E — Current focused-test inventory

Current test declaration inventory: 76 sampled declarations across 3 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/StandingRecordFactionExpansionTests.cs` — 36 test declarations; bytes=13,646; SHA-256=`6367968ea17a60b7c95fc64668cb1924ee2685875041ba680b4be9f1a1d3268d`
- 00049: [Fact]
- 00050: public void Catalog_LoadsSuccessfully_ContainsExactEightFactions()
- 00056: [Fact]
- 00057: public void BaselineOverlay_PreservedVerbatim()
- 00074: [Fact]
- 00075: public void ExpectedEightFactions_AllPresent()
- 00097: [Fact]
- 00098: public void FactionIds_AreUnique_AndStartWithFactionPrefix()
- 00110: [Fact]
- 00111: public void DisplayNames_AreUnique_AndNonEmpty()
- 00122: [Fact]
- 00123: public void Alignments_AreValid()
- 00133: [Fact]
- 00134: public void HomeRegions_AreValid()
- 00154: [Fact]
- 00155: public void Wants_And_Offers_ArePopulated_AndNonEmpty()
- 00178: [Fact]
- 00179: public void TradeProfiles_AreDifferentiated()
- 00205: [Fact]
- 00206: public void SignatureQuotes_AreAuthored_AndDistinct()
- 00217: [Fact]
- 00218: public void AccessRules_AreAuthored_AndDistinct()
- 00229: [Fact]
- 00230: public void ActiveStatus_And_StartingTrust_AreValid()
- 00240: [Fact]
- 00241: public void NegativeFixture_DuplicateId_IsDetected()
- 00263: [Fact]
- 00264: public void NegativeFixture_InvalidAlignment_IsRejected()
- 00277: [Fact]
- 00278: public void NegativeFixture_InvalidRegion_IsRejected()
- 00300: [Fact]
- 00301: public void NegativeFixture_TrustOutOfRange_IsRejected()
- 00312: [Fact]
- 00313: public void Persistence_OldSaveInitialization_DefaultsGracefully()
- 00335: [Fact]
- 00336: public void Persistence_MutableTrustRoundTrip_PreservesDynamicStanding()
### `Ashfall.Core.Tests/StandingRecordSystemTests.cs` — 32 test declarations; bytes=11,037; SHA-256=`c575b772fba23cf0c4b7a6a5214587d8f399c073da8ed841c60b86b2146a81d1`
- 00029: [Fact]
- 00030: public void StrataLoadFromJson()
- 00038: [Fact]
- 00039: public void NowStrataSelectedByMutation()
- 00050: [Fact]
- 00051: public void ScrapedOverridesPlated()
- 00060: [Fact]
- 00061: public void PalimpsestSelectedLast()
- 00070: [Fact]
- 00071: public void SaveRoundtrip()
- 00083: [Fact]
- 00084: public void RecastEventFiresOncePerSite()
- 00100: [Fact]
- 00101: public void LockedUntilUnlock()
- 00108: [Fact]
- 00109: public void StartAndResolve()
- 00120: [Fact]
- 00121: public void ThreeScrapesWithdrawOverlay()
- 00133: [Fact]
- 00134: public void RestoreOverlayAccessReopens()
- 00144: [Fact]
- 00145: public void SaveRoundtrip()
- 00175: [Fact]
- 00176: public void TenMainQuestsRegistered()
- 00194: [Fact]
- 00195: public void MainsChainInOrder()
- 00215: [Fact]
- 00216: public void MainsTargetSpineLocations()
- 00228: [Fact]
- 00229: public void EveryMainHasWorldChangeMutation()
- 00243: [Fact]
- 00244: public void RosterNpcsPresentInCharacters()
### `Ashfall.Core.Tests/Governance/Plan89_98MusterFactionIntegrationTests.cs` — 8 test declarations; bytes=10,108; SHA-256=`9031b5f8a79ba49adf31f295a19a78a9ea01cef996d40034587be14f767e6533`
- 00043: [Fact]
- 00044: public void Plan89_MusterEpiloguesCatalog_LoadsAll25Outcomes_WithValidProseAndTitles()
- 00072: [Fact]
- 00073: public void Plan98_StandingRecordCatalogLoader_LoadsAll8Factions_WithCompleteDossiers()
- 00121: [Fact]
- 00122: public void CrossSystem_FactionAuthority_MapsDeterministicallyToEpilogueOutcomes()
- 00188: [Fact]
- 00189: public void CrossSystem_EpilogueResolution_IsDeterministicAndStrictlyPrioritized()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Standing Record Factions: Eight-Faction Catalog, Record Authority & Legible Access` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- Replace count-growth language with a field-to-consumer audit.
- Mark service-like wants/offers as vocabulary until a typed consumer is proven.
- Downgrade any claim that a catalog row is player-reachable from loader presence alone.

## H.2 Evidence-strength corrections
- The original 1→8 data-only plan is terminal content, not an open feature count.
- Separate historical Wave 40 evidence from current source evidence.
- Do not repeat the same faction dossier or fabricate trade outcomes.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Standing Record Factions: Eight-Faction Catalog, Record Authority & Legible Access while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Which consumer, if any, should turn an access rule into a command?
- Are the eight badge asset ids intentionally empty, and is that a presentation debt or a deliberate placeholder policy?
- Does the current save matrix treat StandingRecord catalog definitions as static and state as expansion-hub data?

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement standing record factions: eight-faction catalog, record authority & legible access arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- The current catalog has eight rows; any additional row requires a new coverage rationale and separate claim.
- The plan does not claim that faction access rules currently execute a trade or standing mutation.
- A future implementation must re-read the current source after this planning snapshot because other claims may change it.

## J.2 Questions deliberately left open
- Which consumer, if any, should turn an access rule into a command?
- Are the eight badge asset ids intentionally empty, and is that a presentation debt or a deliberate placeholder policy?
- Does the current save matrix treat StandingRecord catalog definitions as static and state as expansion-hub data?

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

The original file at `HEAD:piagentsplans/98-standing-record-factions-expansion.md` contained 4,703 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 98 — Standing Record Factions Expansion (1 → 8 factions)

## Goal (2 lines)
Expand `standing_record_factions.json` from 1 verified faction to 8. The
Standing Record faction system (`StandingRecordCatalog.cs` confirmed live)
defines factions in the Standing Record expansion — each has alignment, home
region, trust, wants, offers, signature quote, access rule, and badge asset.
1 faction ("The Overlay") is far too few for a faction-territory expansion.

## Why (P2)
- Verified: `standing_record_factions.json` has 1 entry (id, display_name,
  alignment, home_region, is_active, trust, wants, offers, signature_quote,
  access_rule, badge_asset_id). `StandingRecordCatalog.cs` is confirmed in
  Core. `FactionIconCatalog.cs` handles badges.
- Creates the Standing-Record-faction pillar: the Standing Record expansion
  needs multiple competing factions with territories, trade preferences, and
  access rules. 1 faction means no faction interaction, no territorial
  conflict, no trade dynamics.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/standing_record_factions.json` (expand 1 → 8)
- Read-only: `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs`
  (confirm schema and how wants/offers/access_rule resolve)

## Content grammar (per faction)
- snake_case `id` with prefix `faction_` (confirmed prefix).
- display_name: evocative faction name ("The Overlay", "The Scale",
  "The Compact").
- alignment: conditional / hostile / neutral / allied.
- home_region: region id (all_regions or a specific region).
- is_active: boolean.
- trust: integer starting trust level (-50 to +50).
- wants: array of item ids the faction desires in trade.
- offers: array of services/boons the faction provides.
- signature_quote: 1 sentence in the faction's voice defining its
  philosophy.
- access_rule: 1–2 sentences describing how to maintain (or lose) faction
  access.
- badge_asset_id: asset id for the faction badge (empty string acceptable
  until art is produced).

## Steps
1. Read `StandingRecordCatalog.cs` to confirm the schema and how wants/offers
   are resolved (item ids? service ids?).
2. Read the existing faction ("The Overlay") to confirm the quality bar.
3. Author 7 new factions:
   - `faction_the_scale`: trade-focused, controls water access, wants brass
     and tools, offers water rights and safe passage.
   - `faction_the_compact`: cooperative, manages land records, wants paper
     and ink, offers cadastral maps and dispute resolution.
   - `faction_the_underwrite`: protection-focused, controls fuel depot,
     wants weapons and armor, offers security contracts.
   - `faction_the_cutters`: road maintenance, controls ice road, wants iron
     and coal, offers haulage and road access.
   - `faction_the_fleet`: maritime, controls the dock, wants rope and tar,
     offers barge transport and fishing rights.
   - `faction_the_rebuilders`: agricultural, controls the grain silo, wants
     seeds and tools, offers food supply and crop knowledge.
   - `faction_the_garrison`: military remnant, controls the checkpoint,
     wants ammunition and intelligence, offers patrols and safe passage.
4. Each faction: distinct alignment, home_region, trust, wants, offers,
   signature_quote, and access_rule. No two factions should have identical
   trade profiles.
5. Cross-reference: every faction id unique; every wants/offers item id
   follows existing conventions; every home_region is a valid region.
6. Wire 3 factions into Plan 44 (faction territory map — Standing Record
  factions control territories).
7. Wire 2 factions into Plan 45 (faction patrol encounters — garrison and
  cutters patrol their territories).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: standing record faction catalog loads 8 factions, all ids unique,
   all wants/offers arrays non-empty, all alignments valid.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is wants/offers resolution (step 1): confirm
whether they are item ids or service ids before authoring.

## Definition of Done
- `standing_record_factions.json` has 8 factions, all ids resolving, 3 wired
  to territory map, 2 wired to patrol encounters, integrity + tests green.

## Follow-on
- Plan 44 (faction territory) — factions control territories.
- Plan 45 (faction patrols) — garrison and cutters patrol.
- Plan 43 (settlements) — factions govern settlements.
- Plan 92 (faction dialogue) — factions have overheard dialogue.
- Plan 89 (muster epilogues) — faction standing determines endings.

```

## End of Plan 98 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs` — 160 lines; 5,625 bytes; SHA-256 `fcdff64804e412e7424336f39d2c688b711770a3804038ccfc8e2a1ad27ee7b7`
Declaration index:
- 00009: public class StandingRecordQuestStageEntry
- 00015: public class StandingRecordQuestChoiceEntry
- 00022: public class StandingRecordQuestEntry
- 00041: public class StandingRecordFactionEntry
- 00056: public sealed class StandingRecordCatalog
- 00061: public StandingRecordQuestEntry? GetQuest(string id)
- 00070: public StandingRecordFactionEntry? GetFaction(string id)
- 00084: public sealed class StandingRecordCatalogLoader
- 00100: public StandingRecordCatalog Load(string dataDirectory)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core
00007: {
00008:     /// <summary>Standing Record quest card (standing_record_quests.json).</summary>
00009:     public class StandingRecordQuestStageEntry
00010:     {
00011:         public string id;
00012:         public string text;
00013:     }
00014:
00015:     public class StandingRecordQuestChoiceEntry
00016:     {
00017:         public string id;
00018:         public string text;
00019:         public string set_flag;
00020:     }
00021:
00022:     public class StandingRecordQuestEntry
00023:     {
00024:         public string id;
00025:         public string display_name;
00026:         public string type;
00027:         public string briefing;
00028:         public string prereq_quest_id;
00029:         public int min_day;
00030:         public StandingRecordQuestStageEntry[] stages;
00031:         public StandingRecordQuestChoiceEntry[] choices;
00032:         public string knowledge_key;
00033:         public string target_location_id;
00034:         public string complete_mutation;
00035:         public string fail_mutation;
00036:
00037:         public int StageCount => stages != null ? stages.Length : 0;
00038:     }
00039:
00040:     /// <summary>Standing Record faction card (standing_record_factions.json).</summary>
00041:     public class StandingRecordFactionEntry
00042:     {
00043:         public string id;
00044:         public string display_name;
00045:         public string alignment;
00046:         public string home_region;
00047:         public bool is_active;
00048:         public int trust;
00049:         public string[] wants;
00050:         public string[] offers;
00051:         public string signature_quote;
00052:         public string access_rule;
00053:         public string badge_asset_id;
00054:     }
00055:
00056:     public sealed class StandingRecordCatalog
00057:     {
00058:         public List<StandingRecordQuestEntry> Quests { get; } = new List<StandingRecordQuestEntry>();
00059:         public List<StandingRecordFactionEntry> Factions { get; } = new List<StandingRecordFactionEntry>();
00060:
00061:         public StandingRecordQuestEntry? GetQuest(string id)
00062:         {
00063:             if (string.IsNullOrEmpty(id)) return null;
00064:             for (int i = 0; i < Quests.Count; i++)
00065:                 if (Quests[i] != null && Quests[i].id == id)
00066:                     return Quests[i];
00067:             return null;
00068:         }
00069:
00070:         public StandingRecordFactionEntry? GetFaction(string id)
00071:         {
00072:             if (string.IsNullOrEmpty(id)) return null;
00073:             for (int i = 0; i < Factions.Count; i++)
00074:                 if (Factions[i] != null && Factions[i].id == id)
00075:                     return Factions[i];
00076:             return null;
00077:         }
00078:     }
00079:
00080:     /// <summary>
00081:     /// Loads standing_record_quests.json and standing_record_factions.json via host ports.
00082:     /// Engine-agnostic (shared with the Godot host).
00083:     /// </summary>
00084:     public sealed class StandingRecordCatalogLoader
00085:     {
00086:         public const string QuestsFile = "standing_record_quests.json";
00087:         public const string FactionsFile = "standing_record_factions.json";
00088:
00089:         private readonly IFileIO _files;
00090:         private readonly IJsonSerializer _json;
00091:         private readonly ILog _log;
00092:
00093:         public StandingRecordCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
00094:         {
00095:             _files = files ?? throw new ArgumentNullException(nameof(files));
00096:             _json = json ?? throw new ArgumentNullException(nameof(json));
00097:             _log = log ?? NullLog.Instance;
00098:         }
00099:
00100:         public StandingRecordCatalog Load(string dataDirectory)
00101:         {
00102:             var catalog = new StandingRecordCatalog();
00103:             if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
00104:             {
00105:                 _log.Warn("Standing Record catalog directory missing: " + dataDirectory);
00106:                 return catalog;
00107:             }
00108:
00109:             string questsPath = _files.Combine(dataDirectory, QuestsFile);
00110:             if (_files.FileExists(questsPath))
00111:             {
00112:                 try
00113:                 {
00114:                     string json = _files.ReadAllText(questsPath);
00115:                     var items = CatalogLocator.LoadWrappedList<StandingRecordQuestEntry>(json, SystemTextJsonSerializer.Options);
00116:                     if (items != null)
00117:                     {
00118:                         for (int i = 0; i < items.Count; i++)
00119:                         {
00120:                             if (items[i] != null)
00121:                                 catalog.Quests.Add(items[i]);
00122:                         }
00123:                     }
00124:                 }
00125:                 catch (Exception e)
00126:                 {
00127:                     _log.Error("Standing Record quests parse failed: " + e.Message);
00128:                 }
00129:             }
00130:             else
00131:             {
00132:                 _log.Warn("Standing Record quests file missing: " + questsPath);
00133:             }
00134:
00135:             string factionsPath = _files.Combine(dataDirectory, FactionsFile);
00136:             if (_files.FileExists(factionsPath))
00137:             {
00138:                 try
00139:                 {
00140:                     string json = _files.ReadAllText(factionsPath);
00141:                     var factions = CatalogLocator.LoadWrappedList<StandingRecordFactionEntry>(json, SystemTextJsonSerializer.Options);
00142:                     if (factions != null)
00143:                     {
00144:                         for (int i = 0; i < factions.Count; i++)
00145:                         {
00146:                             if (factions[i] != null)
00147:                                 catalog.Factions.Add(factions[i]);
00148:                         }
00149:                     }
00150:                 }
00151:                 catch (Exception e)
00152:                 {
00153:                     _log.Error("Standing Record factions parse failed: " + e.Message);
00154:                 }
00155:             }
00156:
00157:             return catalog;
00158:         }
00159:     }
00160: }
```

## `Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs` — 176 lines; 6,981 bytes; SHA-256 `6a003a2fcbf7ac6ee0891bf4ecc32b19f1b3633895575755f84c2a7e8d84ec66`
Declaration index:
- 00013: public sealed class StandingRecordState
- 00031: public sealed class StandingRecordEngine
- 00070: public void Load(string dataDir)
- 00086: public void UnlockExpansion(int currentDay)
- 00104: public void Tick(int newDay)
- 00115: public bool ApplySiteMutation(string siteId, string mutation)
- 00128: public string? GetActiveRecast(string siteId)
- 00138: public StandingRecordState CaptureState()
- 00157: public void RestoreState(StandingRecordState saved)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core
00006: {
00007:     /// <summary>
00008:     /// Unified Standing Record (Expansion 03) state envelope. Wraps the
00009:     /// per-system states so the save codec carries one envelope instead
00010:     /// of three. Engine-agnostic.
00011:     /// </summary>
00012:     [Serializable]
00013:     public sealed class StandingRecordState
00014:     {
00015:         public string systemId = StandingRecordEngine.SystemId;
00016:         public bool expansionUnlocked;
00017:         public int currentDay;
00018:         public bool overlayAccess = true;
00019:         public LocationLayoutState layout = new LocationLayoutState();
00020:         public LocationMemoryState memory = new LocationMemoryState();
00021:         public SiteEncounterState encounters = new SiteEncounterState();
00022:     }
00023:
00024:     /// <summary>
00025:     /// Standing Record (Expansion 03) engine. Coordinates the existing
00026:     /// read-only catalog systems — LocationLayout, LocationMemory, and
00027:     /// SiteEncounter — adding a unified tick + expedition hook +
00028:     /// CaptureState / RestoreState. Engine-agnostic; mirrors the Phase-18
00029:     /// Skill Progression port shape.
00030:     /// </summary>
00031:     public sealed class StandingRecordEngine
00032:     {
00033:         public const string SystemId = "standing_record_system";
00034:         public const string FlagExpUnlocked = "exp_standing_record_unlocked";
00035:
00036:         public StandingRecordState State { get; private set; }
00037:
00038:         private readonly IFileIO _files;
00039:         private readonly IJsonSerializer _json;
00040:         private readonly ISeededRng _rng;
00041:         private readonly ILog _log;
00042:
00043:         public LocationLayoutSystem Layouts { get; }
00044:         public LocationMemorySystem Memory { get; }
00045:         public SiteEncounterSystem Encounters { get; }
00046:
00047:         public StandingRecordEngine(
00048:             IFileIO files, IJsonSerializer json,
00049: ISeededRng rng, ILog? log = null,
00050: StandingRecordState? state = null)
00051:         {
00052:             if (files == null) throw new ArgumentNullException(nameof(files));
00053:             if (json == null) throw new ArgumentNullException(nameof(json));
00054:             if (rng == null) throw new ArgumentNullException(nameof(rng));
00055:
00056:             _files = files;
00057:             _json = json;
00058:             _rng = rng;
00059:             _log = log ?? NullLog.Instance;
00060:             State = state ?? new StandingRecordState();
00061:             Layouts = new LocationLayoutSystem(_files, _json, _log);
00062:             Memory = new LocationMemorySystem(_files, _json, _log);
00063:             Encounters = new SiteEncounterSystem();
00064:             Layouts.RestoreState(State.layout);
00065:             Memory.RestoreState(State.memory);
00066:             Encounters.RestoreState(State.encounters);
00067:             State.overlayAccess = Encounters.OverlayAccess;
00068:         }
00069:
00070:         public void Load(string dataDir)
00071:         {
00072:             if (string.IsNullOrEmpty(dataDir))
00073:             {
00074:                 _log.Warn("[StandingRecord] Load called with empty dataDir — engine will run on catalog-less state");
00075:             }
00076:             Layouts.Load(dataDir);
00077:             Memory.Load(dataDir);
00078:             // SiteEncounterSystem has no catalog file — it operates on
00079:             // room-keyed encounter records emitted by expedition entry.
00080:         }
00081:
00082:         public bool IsUnlocked => State.expansionUnlocked;
00083:         public int CurrentDay => State.currentDay;
00084:         public bool HasOverlayAccess => State.overlayAccess;
00085:
00086:         public void UnlockExpansion(int currentDay)
00087:         {
00088:             if (State.expansionUnlocked) return;
00089:             State.expansionUnlocked = true;
00090:             State.currentDay = currentDay;
00091:             Layouts.Unlock();
00092:             Memory.Unlock();
00093:             Encounters.Unlock();
00094:             Memory.ApplyMutation(FlagExpUnlocked);
00095:             _log.Info("[StandingRecord] unlocked @ day " + currentDay);
00096:         }
00097:
00098:         /// <summary>
00099:         /// Day-step hook. Drives overlay-access bookkeeping through
00100:         /// SiteEncounterSystem.ScrapePlate if the engine has been asked
00101:         /// to lose overlay access on a particular day; otherwise mirrors
00102:         /// day progress so the facing dashboard sees a current day.
00103:         /// </summary>
00104:         public void Tick(int newDay)
00105:         {
00106:             if (!State.expansionUnlocked) return;
00107:             State.currentDay = newDay;
00108:             State.overlayAccess = Encounters.OverlayAccess;
00109:         }
00110:
00111:         /// <summary>
00112:         /// Expedition-based mutation surfaces a site-level flag. Triggers
00113:         /// a memory stratum swap for the named site.
00114:         /// </summary>
00115:         public bool ApplySiteMutation(string siteId, string mutation)
00116:         {
00117:             if (!State.expansionUnlocked) return false;
00118:             if (string.IsNullOrEmpty(mutation)) return false;
00119:             Memory.ApplyMutation(mutation);
00120:             Layouts.MutateLayout(siteId, mutation);
00121:             return true;
00122:         }
00123:
00124:         /// <summary>
00125:         /// Read-only passthrough: return the active "now"-text stratum for
00126:         /// a site id, or null if no `'after'` stratum is selected.
00127:         /// </summary>
00128:         public string? GetActiveRecast(string siteId)
00129:         {
00130:             if (Memory == null) return null;
00131:             return Memory.GetActiveRecast(siteId);
00132:         }
00133:
00134:         /// <summary>
00135:         /// Capture full engine state — engine-agnostic; serializes through
00136:         /// the host IJsonSerializer when the save codec fires.
00137:         /// </summary>
00138:         public StandingRecordState CaptureState()
00139:         {
00140:             // Return a fresh envelope so save holders never alias live State.
00141:             return new StandingRecordState
00142:             {
00143:                 systemId = State.systemId,
00144:                 expansionUnlocked = State.expansionUnlocked,
00145:                 currentDay = State.currentDay,
00146:                 overlayAccess = Encounters.OverlayAccess,
00147:                 layout = Layouts.CaptureState(),
00148:                 memory = Memory.CaptureState(),
00149:                 encounters = Encounters.CaptureState(),
00150:             };
00151:         }
00152:
00153:         /// <summary>
00154:         /// Restore from a previously captured state. SiteEncounterSystem /
00155:         /// MemorySystem / LayoutSystem each receive their slice.
00156:         /// </summary>
00157:         public void RestoreState(StandingRecordState saved)
00158:         {
00159:             if (saved == null) return;
00160:             State = new StandingRecordState
00161:             {
00162:                 systemId = saved.systemId,
00163:                 expansionUnlocked = saved.expansionUnlocked,
00164:                 currentDay = saved.currentDay,
00165:                 overlayAccess = saved.overlayAccess,
00166:                 layout = saved.layout ?? new LocationLayoutState(),
00167:                 memory = saved.memory ?? new LocationMemoryState(),
00168:                 encounters = saved.encounters ?? new SiteEncounterState(),
00169:             };
00170:             Layouts.RestoreState(State.layout);
00171:             Memory.RestoreState(State.memory);
00172:             Encounters.RestoreState(State.encounters);
00173:             State.overlayAccess = Encounters.OverlayAccess;
00174:         }
00175:     }
00176: }
```

## `src/Host/StandingRecordHostSession.cs` — 140 lines; 4,971 bytes; SHA-256 `0bd317484ac18f8ea00145e347f8001b47cf3f418a77dee868f32c3d17377304`
Declaration index:
- 00018: public sealed class StandingRecordHostSession
- 00025: public static StandingRecordHostSession Create(string dataDir)
- 00030: public static StandingRecordHostSession Create(string dataDir, int seed)
- 00076: public void Unlock(int day)
- 00084: public void AdvanceDay(int day)
- 00092: public bool ApplyMutation(string siteId, string mutation)
- 00104: public string GetActiveRecast(string siteId)
- 00109: public StandingRecordSave CaptureSave()
- 00121: public void RestoreSave(StandingRecordSave save)
- 00135: public sealed class StandingRecordSave
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006:
00007: namespace AtomicWar.GodotApp
00008: {
00009:     /// <summary>
00010:     /// ASHFALL: THE STANDING RECORD — thin Godot-host session.
00011:     /// Wraps the unified engine-agnostic StandingRecordEngine around the
00012:     /// three existing catalog systems (LocationLayout, LocationMemory,
00013:     /// SiteEncounter). Captures / restores the unified envelope via
00014:     /// StandingRecordSaveStore. No gameplay rules here — hosts only
00015:     /// present the engine's read surface to the dashboard.
00016:     /// Spec: docs/expansions/expansion_03_the_standing_record_plan.md.
00017:     /// </summary>
00018:     public sealed class StandingRecordHostSession
00019:     : HostSessionBase{
00020:         public const int DefaultSeed = 1401; // catalog seed offset for SR rooms/mutations.
00021:
00022:         public StandingRecordEngine Engine { get; }
00023:         public string LastEvent { get; private set; } = string.Empty;
00024:         public string DataDir { get; }
00025:         public static StandingRecordHostSession Create(string dataDir)
00026:         {
00027:             return new StandingRecordHostSession(dataDir, seed: DefaultSeed);
00028:         }
00029:
00030:         public static StandingRecordHostSession Create(string dataDir, int seed)
00031:         {
00032:             return new StandingRecordHostSession(dataDir, seed);
00033:         }
00034:
00035:         private StandingRecordHostSession(string? dataDir, int seed)
00036:         {
00037:             DataDir = dataDir ?? string.Empty;
00038:             var fileIO = new FileSystemIO();
00039:             var serializer = new SystemTextJsonSerializer();
00040:             var hostLog = new ConsoleLog();
00041:             var rng = new SeededRng(seed);
00042:             Engine = new StandingRecordEngine(
00043:                 files: fileIO,
00044:                 json: serializer,
00045:                 rng: rng,
00046:                 log: hostLog);
00047:
00048:             try
00049:             {
00050:                 Engine.Load(DataDir);
00051:             }
00052:             catch (Exception ex)
00053:             {
00054:                 hostLog.Error("[StandingRecord] load failed: " + ex.Message);
00055:             }
00056:
00057:             if (Engine.State.expansionUnlocked)
00058:             {
00059:                 RaiseStateChanged();
00060:             }
00061:         }
00062:
00063:         public bool IsUnlocked => Engine != null && Engine.IsUnlocked;
00064:         public int CurrentDay => Engine != null ? Engine.CurrentDay : 0;
00065:         public bool HasOverlayAccess =>
00066:             Engine != null && Engine.HasOverlayAccess;
00067:         public int LayoutCount => Engine != null ? Engine.Layouts.LayoutCount : 0;
00068:         public int StratumCount => Engine != null ? Engine.Memory.StratumCount : 0;
00069:         public IReadOnlyList<LocationLayoutDef> Layouts =>
00070:             Engine != null ? Engine.Layouts.Layouts : new List<LocationLayoutDef>();
00071:         public IReadOnlyList<LocationMemoryStratum> AllStrata =>
00072:             Engine != null && Engine.State?.memory?.strata != null
00073:                 ? Engine.State.memory.strata
00074:                 : new List<LocationMemoryStratum>();
00075:
00076:         public void Unlock(int day)
00077:         {
00078:             if (Engine == null) return;
00079:             Engine.UnlockExpansion(day);
00080:             LastEvent = "Standing Record unlocked @ day " + day;
00081:             RaiseStateChanged();
00082:         }
00083:
00084:         public void AdvanceDay(int day)
00085:         {
00086:             if (Engine == null) return;
00087:             Engine.Tick(day);
00088:             LastEvent = "Tick @ day " + day;
00089:             RaiseStateChanged();
00090:         }
00091:
00092:         public bool ApplyMutation(string siteId, string mutation)
00093:         {
00094:             if (Engine == null) return false;
00095:             bool applied = Engine.ApplySiteMutation(siteId, mutation);
00096:             if (applied)
00097:             {
00098:                 LastEvent = "Mutation applied: " + mutation + " @ " + siteId;
00099:                 RaiseStateChanged();
00100:             }
00101:             return applied;
00102:         }
00103:
00104:         public string GetActiveRecast(string siteId)
00105:         {
00106:             return Engine == null ? string.Empty : Engine.GetActiveRecast(siteId) ?? string.Empty;
00107:         }
00108:
00109:         public StandingRecordSave CaptureSave()
00110:         {
00111:             var state = Engine != null
00112:                 ? Engine.CaptureState()
00113:                 : new StandingRecordState();
00114:             return new StandingRecordSave
00115:             {
00116:                 systemId = StandingRecordEngine.SystemId,
00117:                 state = state,
00118:             };
00119:         }
00120:
00121:         public void RestoreSave(StandingRecordSave save)
00122:         {
00123:             if (save == null || save.state == null) return;
00124:             Engine.RestoreState(save.state);
00125:             LastEvent = "Standing Record restored";
00126:             RaiseStateChanged();
00127:         }
00128:     }
00129:
00130:     /// <summary>
00131:     /// Save DTO for the unified Standing Record envelope.
00132:     /// Mirrors the unified-state shape required by StandingRecordEngine.
00133:     /// </summary>
00134:     [Serializable]
00135:     public sealed class StandingRecordSave
00136:     {
00137:         public string systemId = StandingRecordEngine.SystemId;
00138:         public StandingRecordState state = new StandingRecordState();
00139:     }
00140: }
```

## `src/Host/ExpansionHostSession.cs` — 500 lines; 25,698 bytes; SHA-256 `9809fca6ed766dfd903f6dcc7df79e68f779d539a85bbd8a66066710d570da45`
Declaration index:
- 00019: public sealed class ExpansionHostSession
- 00105: public void BindDutyRoster(DutyRosterSystem roster)
- 00114: public void BindGreenhouse(GreenhouseSystem shared)
- 00127: public static ExpansionHostSession Create(
- 00214: public void ShutdownDebtIntegration()
- 00235: public ExpansionHubSave CaptureSave(int simDay, DebtConsequenceBridgeState? debtBridge = null) =>
- 00240: public void RestoreSave(ExpansionHubSave save, DebtConsequenceHostBridge? debtBridge = null) =>
- 00248: public void LoadDefaultBackerPool()
- 00260: public string ArbitrationLine()
- 00276: public string LedgerLine()
- 00292: public void UnlockWaystation() => Waystation.Unlock();
- 00293: public void SetWaystationWintering(bool wintering) => Waystation.SetWintering(wintering);
- 00294: public bool AssignWaystationWatch(string[] ids) => Waystation.AssignWatch(ids);
- 00295: public void ResupplyWaystation() => Waystation.Resupply();
- 00296: public void TickWaystation(bool iceRoadOpen) => Waystation.TickDaily(iceRoadOpen);
- 00298: public string WaystationLine()
- 00311: public void UnlockRecord()
- 00318: public bool ArriveAtSite(string parentId) => Layouts.ArriveAtParent(parentId);
- 00320: public bool EnterSiteRoom(string roomId) => Layouts.EnterRoom(roomId);
- 00322: public bool InspectSiteRoom(string roomId) => Layouts.InspectRoom(roomId);
- 00324: public string RoomLine(string parentId, string roomId)
- 00339: public string StandingRecordLine()
- 00356: public string RecordQuestLine()
- 00371: public bool GrantVouch(string npcId) => Vouch.GrantVouch(npcId, isLastResort: false);
- 00372: public bool BurnVouch() => Vouch.BurnVouch();
- 00373: public bool SoftenAccess() => Vouch.SoftenAccess();
- 00375: public string CrossingLine()
- 00387: public bool StartCrossingQuest(string questId, int currentDay)
- 00394: public void TickCrossingQuests(int currentDay)
- 00397: public int AdvanceCrossingQuestStage(string questId)
- 00400: public bool MakeCrossingChoice(string questId, string choiceId)
- 00403: public List<CrossingQuestDef> GetAvailableCrossingQuests(int currentDay)
- 00406: public bool FailCrossingQuest(string questId)
- 00409: public bool IsCrossingQuestFailed(string questId)
- 00412: public bool IsCrossingQuestCompleted(string questId)
- 00415: public string CrossingQuestLine()
- 00439: public void EnsureGreenhousePlots(int count) => Greenhouse.EnsurePlots(count);
- 00440: public bool PlantGreenhouse(int plotIndex, string seedItemId, int day)
- 00442: public void WaterGreenhouse(int plotIndex, float units) => Greenhouse.Water(plotIndex, units, tainted: false);
- 00443: public GreenhouseHarvest HarvestGreenhouse(int plotIndex) => Greenhouse.Harvest(plotIndex);
- 00444: public void TickGreenhouse(int simDay) =>
- 00447: public string GreenhouseLine()
- 00467: public void RegisterGenerationDweller(string dwellerId, int age, int generation = 0)
- 00470: public string AdvanceGenerationalTime(int days)
- 00477: public string FormMentorshipDemo(string mentorId, string apprenticeId, string traitId)
- 00484: public string GenerationalLine()
- 00492: public GenerationalSuccessionSaveState CaptureGenerationalSave() => Generational.CaptureState();
- 00493: public void RestoreGenerationalSave(GenerationalSuccessionSaveState state) => Generational.RestoreState(state);
- 00497: public string GenerateEpilogueNarrativeDemo(EpilogueEvaluationContext ctx)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Text;
00004: using System.Collections.Generic;
00005: #pragma warning disable CS8618
00006: using Ashfall.Core;
00007: using Ashfall.Core.Crossing;
00008: using Ashfall.Core.Endgame;
00009: using Ashfall.Core.Legacy;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     /// <summary>
00014:     /// Thin Godot-host wrapper for the expansion surfaces that were selftest-only:
00015:     /// Waystation (Holdfast S2 vitals), Standing Record (Exp 03 layouts),
00016:     /// Crossing gate (Exp 04 vouch), and Greenhouse (Exp 05 plots).
00017:     /// No gameplay rules here — everything delegates to Ashfall.Core.
00018:     /// </summary>
00019:     public sealed class ExpansionHostSession
00020:     : HostSessionBase
00021:     {
00022:         public const int DefaultSeed = 1117; // greenhouse + vouch demo seed
00023:
00024:         public WaystationSystem Waystation { get; }
00025:         public LocationLayoutSystem Layouts { get; }
00026:         public LocationMemorySystem Memory { get; }
00027:         public SiteEncounterSystem SiteEncounters { get; }
00028:         public StandingRecordCatalog RecordQuests { get; }
00029:         public VouchAccessSystem Vouch { get; }
00030:         public GreenhouseSystem Greenhouse { get; private set; }
00031:         public CrossingArbitrationSystem Arbitration { get; }
00032:         public LedgerDebtSystem Ledger { get; }
00033:         public CrossingQuestSystem CrossingQuests { get; }
00034:         public GenerationalSuccessionEngine Generational { get; }
00035:         public EpilogueMatrixRuntime Epilogue { get; }
00036:         public DutyRosterSystem DutyRoster { get; private set; }
00037:         public Ashfall.Core.Foundry.SilentFoundrySystem SilentFoundry { get; private set; }
00038:         public Ashfall.Core.Foundry.SilentFoundryCatalog FoundryData { get; private set; }
00039:         public Ashfall.Core.Disease.DiseaseSystem Disease { get; private set; }
00040:         public Ashfall.Core.Disease.DiseaseCatalog DiseaseData { get; private set; }
00041:
00042:         /// <summary>Debt template catalog (ledger_debt_templates.json) — the
00043:         /// single loaded instance shared by ledger, dispatcher and credit.</summary>
00044:         public DebtTemplateCatalog? DebtCatalog { get; private set; }
00045:         /// <summary>Exactly one live dispatcher per live ledger (host-owned;
00046:         /// constructed once here, detached only at session shutdown).</summary>
00047:         public DebtConsequenceDispatcher? DebtDispatcher { get; private set; }
00048:         /// <summary>Canonical faction embargo authority for debt defaults.</summary>
00049:         public FactionEmbargoLedger Embargoes { get; } = new FactionEmbargoLedger();
00050:
00051:         public ExpansionHostSession(
00052:             WaystationSystem waystation,
00053:             LocationLayoutSystem layouts,
00054:             LocationMemorySystem memory,
00055:             SiteEncounterSystem siteEncounters,
00056:             StandingRecordCatalog recordQuests,
00057:             VouchAccessSystem vouch,
00058:             GreenhouseSystem greenhouse,
00059:             Ashfall.Core.Flags.IFlagLedger? consequenceLedger = null)
00060:         {
00061:             Waystation = waystation ?? new WaystationSystem();
00062:             Layouts = layouts ?? new LocationLayoutSystem(
00063:                 new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
00064:             Memory = memory ?? new LocationMemorySystem(
00065:                 new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
00066:             SiteEncounters = siteEncounters ?? new SiteEncounterSystem();
00067:             RecordQuests = recordQuests ?? new StandingRecordCatalog();
00068:             Vouch = vouch ?? new VouchAccessSystem();
00069:             Greenhouse = greenhouse ?? new GreenhouseSystem(DefaultSeed);
00070:             Arbitration = new CrossingArbitrationSystem();
00071:             Ledger = new LedgerDebtSystem();
00072:             CrossingQuests = new CrossingQuestSystem();
00073:             CrossingQuests.BindConsequenceLedger(consequenceLedger);
00074:             Generational = new GenerationalSuccessionEngine();
00075:             Epilogue = new EpilogueMatrixRuntime();
00076:             DutyRoster = new DutyRosterSystem();
00077:
00078:             // Persistence: any hub-system state change marks the save dirty.
00079:             Waystation.OnStateChanged += _ => RaiseStateChanged();
00080:             Layouts.OnStateChanged += _ => RaiseStateChanged();
00081:             Memory.OnStateChanged += _ => RaiseStateChanged();
00082:             SiteEncounters.OnStateChanged += _ => RaiseStateChanged();
00083:             Vouch.OnStateChanged += _ => RaiseStateChanged();
00084:             Greenhouse.OnCropPlanted += (_, _, _) => RaiseStateChanged();
00085:             Greenhouse.OnCropMatured += (_, _) => RaiseStateChanged();
00086:             Greenhouse.OnCropHarvested += _ => RaiseStateChanged();
00087:             Greenhouse.OnBlightOutbreak += _ => RaiseStateChanged();
00088:             Greenhouse.OnPlotDriedOut += _ => RaiseStateChanged();
00089:             Greenhouse.OnCropFailed += _ => RaiseStateChanged();
00090:             Arbitration.OnStateChanged += _ => RaiseStateChanged();
00091:             CrossingQuests.OnStateChanged += _ => RaiseStateChanged();
00092:             // Debt: any ledger/embargo mutation marks the hub save dirty so the
00093:             // dispatcher fired-set, embargo ledger and contract ink all flush.
00094:             Ledger.OnStateChanged += _ => RaiseStateChanged();
00095:             Embargoes.OnStateChanged += () => RaiseStateChanged();
00096:             // When the opening vouch quest completes, soften the gate automatically
00097:             CrossingQuests.OnOpeningQuestCompleted += () => Vouch.SoftenAccess();
00098:             CrossingQuests.OnStageNarrativeEmitted += evt => OnCrossingStageNarrative?.Invoke(evt);
00099:             Generational.OnDwellerRetired += (_, _) => RaiseStateChanged();
00100:             Generational.OnTraitInherited += (_, _, _) => RaiseStateChanged();
00101:             Generational.OnChapterAdvanced += _ => RaiseStateChanged();
00102:         }
00103:
00104:         /// <summary>Attach the campaign-owned duty roster used by debt labor reservations.</summary>
00105:         public void BindDutyRoster(DutyRosterSystem roster)
00106:         {
00107:             DutyRoster = roster ?? throw new ArgumentNullException(nameof(roster));
00108:         }
00109:
00110:         /// <summary>
00111:         /// Share the player greenhouse growth authority so hub capture/restore
00112:         /// and expansions UI cannot diverge from <c>GreenhouseHostSession</c>.
00113:         /// </summary>
00114:         public void BindGreenhouse(GreenhouseSystem shared)
00115:         {
00116:             Greenhouse = shared ?? throw new ArgumentNullException(nameof(shared));
00117:             Greenhouse.OnCropPlanted += (_, _, _) => RaiseStateChanged();
00118:             Greenhouse.OnCropMatured += (_, _) => RaiseStateChanged();
00119:             Greenhouse.OnCropHarvested += _ => RaiseStateChanged();
00120:             Greenhouse.OnBlightOutbreak += _ => RaiseStateChanged();
00121:             Greenhouse.OnPlotDriedOut += _ => RaiseStateChanged();
00122:             Greenhouse.OnCropFailed += _ => RaiseStateChanged();
00123:         }
00124:
00125:         public event Action<CrossingStageNarrativeEvent>? OnCrossingStageNarrative;
00126:
00127:         public static ExpansionHostSession Create(
00128:             string dataDirectory,
00129:             ILog log = null!,
00130:             Ashfall.Core.Flags.IFlagLedger? consequenceLedger = null)
00131:         {
00132:             CatalogLocator.UseInvariantCulture();
00133:             log = log ?? new GodotLog();
00134:             var files = CatalogPath.CreateFileIOForDataDir(dataDirectory);
00135:             var json = new SystemTextJsonSerializer();
00136:
00137:             var layouts = new LocationLayoutSystem(files, json, log);
00138:             layouts.Load(dataDirectory);
00139:             var memory = new LocationMemorySystem(files, json, log);
00140:             memory.Load(dataDirectory);
00141:             var quests = new StandingRecordCatalogLoader(files, json, log).Load(dataDirectory);
00142:             var crossingQuests = CrossingQuestCatalogLoader.Load(dataDirectory, files, json);
00143:
00144:             var session = new ExpansionHostSession(
00145:                 new WaystationSystem(),
00146:                 layouts,
00147:                 memory,
00148:                 new SiteEncounterSystem(DefaultSeed),
00149:                 quests,
00150:                 new VouchAccessSystem(),
00151:                 new GreenhouseSystem(DefaultSeed),
00152:                 consequenceLedger);
00153:             session.CrossingQuests.BindCatalog(crossingQuests);
00154:
00155:             // The Silent Foundry (Exp 10): static catalogs + blueprint + treaty anchors.
00156:             var foundryData = new Ashfall.Core.Foundry.SilentFoundryCatalog();
00157:             foundryData.Load(
00158:                 Ashfall.Core.Foundry.SilentFoundryCatalogLoader.LoadProduction(dataDirectory, files, json)!,
00159:                 Ashfall.Core.Foundry.SilentFoundryCatalogLoader.LoadFaction(dataDirectory, files, json)!);
00160:             var foundry = new Ashfall.Core.Foundry.SilentFoundrySystem(log: log);
00161:             int maintenanceCycle = 4;
00162:             var blueprints = new Ashfall.Core.Narrative.BunkerBlueprintCatalog();
00163:             string bpPath = files.Combine(dataDirectory, "narrative", "bunker_blueprints_codex.json");
00164:             if (files.FileExists(bpPath))
00165:             {
00166:                 blueprints.Load(files.ReadAllText(bpPath), json);
00167:                 var bp = blueprints.GetById(Ashfall.Core.Foundry.SilentFoundryIds.BlueprintRoomId);
00168:                 if (bp != null && bp.maintenance_cycle_days > 0) maintenanceCycle = bp.maintenance_cycle_days;
00169:             }
00170:             // District 8 accords (data authority: foundry_accords.json) drive the
00171:             // foundry's treaty clock — campaign-reachable days, Sector 4 canon.
00172:             var ratificationDays = Ashfall.Core.Foundry.SilentFoundryCatalogLoader.LoadAccordRatificationDays(
00173:                 dataDirectory, files, json);
00174:             if (ratificationDays.Count > 0)
00175:                 foundry.BindTreaties(ratificationDays);
00176:             foundry.BindCatalog(foundryData, maintenanceCycle);
00177:             foundry.BindGlassworksCatalog(
00178:                 Ashfall.Core.Foundry.GlassworksCatalogLoader.Load(dataDirectory, files, json));
00179:             session.SilentFoundry = foundry;
00180:             session.FoundryData = foundryData;
00181:             foundry.OnStateChanged += _ => session.RaiseStateChanged();
00182:
00183:             // Disease Expansion: static catalog + deterministic contagion engine.
00184:             // Bound on the catalog (registered above); always active — outbreaks
00185:             // threaten from day one. No unlock gate, no facility to build.
00186:             var diseaseData = Ashfall.Core.Disease.DiseaseCatalogLoader.Load(dataDirectory, files, json);
00187:             var disease = new Ashfall.Core.Disease.DiseaseSystem(log: log);
00188:             disease.BindCatalog(diseaseData);
00189:             session.Disease = disease;
00190:             session.DiseaseData = diseaseData;
00191:             disease.OnStateChanged += _ => session.RaiseStateChanged();
00192:
00193:             // Ledger debt consequences (Plan IV): the same single catalog instance
00194:             // feeds the dispatcher; the dispatcher is attached exactly once here —
00195:             // the session ctor path (tests) deliberately skips it because a
00196:             // dispatcher without authorities subscribed is inert by design.
00197:             session.DebtCatalog = DebtTemplateCatalogLoader.Load(dataDirectory, files, json);
00198:             if (session.DebtCatalog.Errors.Count > 0)
00199:             {
00200:                 for (int i = 0; i < session.DebtCatalog.Errors.Count; i++)
00201:                     log.Error("[ExpansionHostSession] debt catalog: " + session.DebtCatalog.Errors[i]);
00202:             }
00203:             else
00204:             {
00205:                 // One active ledger → exactly one active consequence dispatcher.
00206:                 if (session.DebtDispatcher == null)
00207:                     session.DebtDispatcher = new DebtConsequenceDispatcher(session.Ledger, session.DebtCatalog);
00208:             }
00209:             return session;
00210:         }
00211:
00212:         /// <summary>Detach the debt consequence integration at session shutdown
00213:         /// so recomposition cannot leave dangling subscriptions.</summary>
00214:         public void ShutdownDebtIntegration()
00215:         {
00216:             DebtDispatcher?.Detach();
00217:             DebtDispatcher = null;
00218:         }
00219:
00220:         /// <summary>Host-session teardown also releases the dispatcher
00221:         /// subscription, including callers that dispose the session directly
00222:         /// through the shared lifecycle registry.</summary>
00223:         public override void Dispose()
00224:         {
00225:             ShutdownDebtIntegration();
00226:             base.Dispose();
00227:         }
00228:
00229:         // ---- Cross-host save ----
00230:
00231:         /// <summary>Cross-host save envelope. Shape and checksum owned by ExpansionHubSaveCodec.
00232:         /// The debt-consequence bridge (host-authority wiring owned by Main) contributes
00233:         /// its labor-obligation state; dispatcher fired-set and embargoes are captured
00234:         /// from the session-owned systems directly.</summary>
00235:         public ExpansionHubSave CaptureSave(int simDay, DebtConsequenceBridgeState? debtBridge = null) =>
00236:             ExpansionHubSaveCodec.Capture(simDay, Waystation, Layouts, Memory, SiteEncounters, Vouch, Greenhouse,
00237:                 Arbitration, Ledger, CrossingQuests, Generational, SilentFoundry, Disease,
00238:                 debtDispatcher: DebtDispatcher, embargoes: Embargoes, debtBridge: debtBridge);
00239:
00240:         public void RestoreSave(ExpansionHubSave save, DebtConsequenceHostBridge? debtBridge = null) =>
00241:             ExpansionHubSaveCodec.Restore(save, Waystation, Layouts, Memory, SiteEncounters, Vouch, Greenhouse,
00242:                 Arbitration, Ledger, CrossingQuests, Generational, SilentFoundry, Disease,
00243:                 debtDispatcher: DebtDispatcher, embargoes: Embargoes, debtBridge: debtBridge);
00244:
00245:         // ---- Nobody's Charter: Crossing Arbitration (Exp 04) ----
00246:
00247:         /// <summary>Loads the standard backer pool used by the headless demo and dev UI.</summary>
00248:         public void LoadDefaultBackerPool()
00249:         {
00250:             Arbitration.LoadBackerPool(new List<BackerDef>
00251:             {
00252:                 new BackerDef { id = CrossingIds.NpcOsran, displayName = "Osran Kell", wants = "a sealed contract", willNot = "forge a signature", principled = true },
00253:                 new BackerDef { id = CrossingIds.NpcMattis, displayName = "Mattis Cray", wants = "a public record", willNot = "sign a false statement", principled = true },
00254:                 new BackerDef { id = "npc_halden_mire", displayName = "Halden Mire", wants = "grain futures", willNot = "lend to a ghost", principled = true },
00255:                 new BackerDef { id = "npc_bram_ostrowski", displayName = "Bram Ostrowski", wants = "brass scrap", willNot = "deal with the Garrison directly", principled = false },
00256:                 new BackerDef { id = "npc_leva_quist", displayName = "Leva Quist", wants = "information", willNot = "be seen at the Lockup", principled = false }
00257:             });
00258:         }
00259:
00260:         public string ArbitrationLine()
00261:         {
00262:             var sb = new System.Text.StringBuilder();
00263:             sb.Append("Arbitration: ").Append(Arbitration.State.rulingsCalled).Append(" called · ")
00264:                 .Append(Arbitration.State.rulingsOverturned).Append(" overturned · ")
00265:                 .Append(Arbitration.State.standingRepeats).Append(" re-Stood");
00266:             for (int i = 0; i < Arbitration.Rulings.Count; i++)
00267:             {
00268:                 var r = Arbitration.Rulings[i];
00269:                 if (r == null) continue;
00270:                 sb.Append("\n  ").Append(r.topic).Append(": ").Append(r.shape)
00271:                     .Append(" (").Append(r.backers.Count).Append(" backers)");
00272:             }
00273:             return sb.ToString();
00274:         }
00275:
00276:         public string LedgerLine()
00277:         {
00278:             var sb = new System.Text.StringBuilder();
00279:             sb.Append("Ledger: ").Append(Ledger.Contracts.Count).Append(" open · ")
00280:                 .Append(Ledger.ClosedContracts.Count).Append(" closed · ")
00281:                 .Append(Ledger.LedgerTampered ? "TAMPERED" : "clean");
00282:             for (int i = 0; i < Ledger.Contracts.Count; i++)
00283:             {
00284:                 var c = Ledger.Contracts[i];
00285:                 if (c == null) continue;
00286:                 sb.Append("\n  ").Append(c.debtorId).Append(": ").Append(c.principal).Append(" (").Append(c.daysRemaining).Append("d, ")
00287:                     .Append(c.signed ? "signed" : "draft").Append(")");
00288:             }
00289:             return sb.ToString();
00290:         }
00291:
00292:         public void UnlockWaystation() => Waystation.Unlock();
00293:         public void SetWaystationWintering(bool wintering) => Waystation.SetWintering(wintering);
00294:         public bool AssignWaystationWatch(string[] ids) => Waystation.AssignWatch(ids);
00295:         public void ResupplyWaystation() => Waystation.Resupply();
00296:         public void TickWaystation(bool iceRoadOpen) => Waystation.TickDaily(iceRoadOpen);
00297:
00298:         public string WaystationLine()
00299:         {
00300:             if (!Waystation.Unlocked) return "Waystation: sealed (unlock to open bunks)";
00301:             return
00302:                 $"Waystation: stove {(Waystation.StoveLit ? "lit" : "cold")} · " +
00303:                 $"bunks {Waystation.State.bunksOccupied}/{WaystationSystem.MaxBunks} · " +
00304:                 $"filter {Waystation.State.filterHealth:0}% · " +
00305:                 $"resupply {Waystation.State.daysSinceResupply}d ago · " +
00306:                 $"wintering {(Waystation.State.winteringClosedWindow ? "closed-window" : "normal")}";
00307:         }
00308:
00309:         // ---- Standing Record (Exp 03) ----
00310:
00311:         public void UnlockRecord()
00312:         {
00313:             Layouts.Unlock();
00314:             Memory.Unlock();
00315:             SiteEncounters.Unlock();
00316:         }
00317:
00318:         public bool ArriveAtSite(string parentId) => Layouts.ArriveAtParent(parentId);
00319:
00320:         public bool EnterSiteRoom(string roomId) => Layouts.EnterRoom(roomId);
00321:
00322:         public bool InspectSiteRoom(string roomId) => Layouts.InspectRoom(roomId);
00323:
00324:         public string RoomLine(string parentId, string roomId)
00325:         {
00326:             var def = Layouts.GetLayout(parentId);
00327:             if (def == null) return "no layout for " + parentId;
00328:             var room = def.GetRoom(roomId);
00329:             if (room == null) return "no room " + roomId;
00330:             string dark = Layouts.IsRoomDark(parentId, roomId) ? " [dark]" : "";
00331:             string recast = Memory.GetActiveRecast(parentId)!;
00332:             var sb = new StringBuilder(room.displayName).Append(dark).Append("\n");
00333:             sb.Append(room.inspect).Append('\n');
00334:             if (!string.IsNullOrEmpty(recast) && !Layouts.IsRoomDark(parentId, roomId))
00335:                 sb.Append("NOW: ").Append(recast).Append('\n');
00336:             return sb.ToString().TrimEnd();
00337:         }
00338:
00339:         public string StandingRecordLine()
00340:         {
00341:             var sb = new StringBuilder();
00342:             sb.Append("Standing Record: ").Append(Layouts.LayoutCount).Append(" layouts · ")
00343:                 .Append(Memory.StratumCount).Append(" strata · ")
00344:                 .Append(RecordQuests.Quests.Count).Append(" quests · ");
00345:             sb.Append("Overlay ").Append(SiteEncounters.OverlayAccess ? "access" : "WITHDRAWN")
00346:                 .Append(" · plates scraped ").Append(SiteEncounters.PlatesScraped);
00347:             if (Layouts.LayoutCount > 0)
00348:             {
00349:                 var def = Layouts.Layouts[0];
00350:                 sb.Append(" · first: ").Append(def.parentLocationId)
00351:                     .Append(" (").Append(def.displayName).Append(", ").Append(def.RoomCount).Append(" rooms)");
00352:             }
00353:             return sb.ToString();
00354:         }
00355:
00356:         public string RecordQuestLine()
00357:         {
00358:             var sb = new StringBuilder("Record quests:");
00359:             for (int i = 0; i < RecordQuests.Quests.Count && i < 5; i++)
00360:             {
00361:                 var q = RecordQuests.Quests[i];
00362:                 sb.Append("\n  ").Append(q.id).Append(" → ").Append(q.target_location_id);
00363:             }
00364:             if (RecordQuests.Quests.Count > 5)
00365:                 sb.Append("\n  +").Append(RecordQuests.Quests.Count - 5).Append(" more");
00366:             return sb.ToString();
00367:         }
00368:
00369:         // ---- Crossing gate (Exp 04) ----
00370:
00371:         public bool GrantVouch(string npcId) => Vouch.GrantVouch(npcId, isLastResort: false);
00372:         public bool BurnVouch() => Vouch.BurnVouch();
00373:         public bool SoftenAccess() => Vouch.SoftenAccess();
00374:
00375:         public string CrossingLine()
00376:         {
00377:             string gate = Vouch.HasAccess ? "OPEN" : "CLOSED";
00378:             return
00379:                 $"Crossing: gate {gate} · " +
00380:                 $"vouch {(string.IsNullOrEmpty(Vouch.VouchedBy) ? "none" : Vouch.VouchedBy)} · " +
00381:                 $"burned {Vouch.VouchBurned} · softened {Vouch.AccessSoftened} · " +
00382:                 $"last resort {(Vouch.LastResortUsed ? "used" : "available")}";
00383:         }
00384:
00385:         // ---- Nobody's Charter: Crossing Quests (Exp 04) ----
00386:
00387:         public bool StartCrossingQuest(string questId, int currentDay)
00388:             => CrossingQuests.StartQuest(questId, currentDay);
00389:
00390:         /// <summary>
00391:         /// Idempotent daily tick for the Crossing quest auto-start.
00392:         /// Only starts eligible quests once per calendar day; safe to call repeatedly.
00393:         /// </summary>
00394:         public void TickCrossingQuests(int currentDay)
00395:             => CrossingQuests.TickDaily(currentDay, hasVouchAccess: Vouch.HasAccess);
00396:
00397:         public int AdvanceCrossingQuestStage(string questId)
00398:             => CrossingQuests.AdvanceStage(questId);
00399:
00400:         public bool MakeCrossingChoice(string questId, string choiceId)
00401:             => CrossingQuests.MakeChoice(questId, choiceId);
00402:
00403:         public List<CrossingQuestDef> GetAvailableCrossingQuests(int currentDay)
00404:             => CrossingQuests.GetAvailableQuests(currentDay);
00405:
00406:         public bool FailCrossingQuest(string questId)
00407:             => CrossingQuests.FailQuest(questId);
00408:
00409:         public bool IsCrossingQuestFailed(string questId)
00410:             => CrossingQuests.IsQuestFailed(questId);
00411:
00412:         public bool IsCrossingQuestCompleted(string questId)
00413:             => CrossingQuests.IsQuestCompleted(questId);
00414:
00415:         public string CrossingQuestLine()
00416:         {
00417:             var sb = new StringBuilder("Crossing quests:");
00418:             var catalog = CrossingQuests.Catalog;
00419:             int shown = 0;
00420:             for (int i = 0; i < catalog.Count && shown < 5; i++)
00421:             {
00422:                 var def = catalog[i];
00423:                 if (def == null) continue;
00424:                 var progress = CrossingQuests.GetProgress(def.id);
00425:                 string status = progress == null ? "available" :
00426:                     progress.completed ? "done" :
00427:                     progress.started ? $"stage {progress.currentStage}/{def.stages.Count}" : "ready";
00428:                 sb.Append("\n  ").Append(def.id).Append(" [").Append(status).Append("]");
00429:                 shown++;
00430:             }
00431:             if (catalog.Count > 5)
00432:                 sb.Append("\n  +").Append(catalog.Count - 5).Append(" more");
00433:             sb.Append(" · flags ").Append(CrossingQuests.State.setFlags.Count);
00434:             return sb.ToString();
00435:         }
00436:
00437:         // ---- Greenhouse (Exp 05) ----
00438:
00439:         public void EnsureGreenhousePlots(int count) => Greenhouse.EnsurePlots(count);
00440:         public bool PlantGreenhouse(int plotIndex, string seedItemId, int day)
00441:             => Greenhouse.Plant(plotIndex, seedItemId, day, out _);
00442:         public void WaterGreenhouse(int plotIndex, float units) => Greenhouse.Water(plotIndex, units, tainted: false);
00443:         public GreenhouseHarvest HarvestGreenhouse(int plotIndex) => Greenhouse.Harvest(plotIndex);
00444:         public void TickGreenhouse(int simDay) =>
00445:             Greenhouse.TickDay(simDay, growLightHours: 6f, ashContaminationRate: 0.02f);
00446:
00447:         public string GreenhouseLine()
00448:         {
00449:             var sb = new StringBuilder();
00450:             sb.Append("Greenhouse: ").Append(Greenhouse.PlotCount).Append(" plots · ");
00451:             sb.Append(Greenhouse.TotalHarvests).Append(" harvests · ");
00452:             sb.Append(Greenhouse.IsPreWarWheatUnlocked ? "wheat unlocked" : "wheat locked");
00453:             sb.Append(" · [");
00454:             for (int i = 0; i < Greenhouse.PlotCount; i++)
00455:             {
00456:                 if (i > 0) sb.Append(" ");
00457:                 var p = Greenhouse.State.plots[i];
00458:                 string seed = string.IsNullOrEmpty(p.seedItemId) ? "fallow" : p.seedItemId.Replace("item_", "");
00459:                 sb.Append(i).Append(":").Append(seed).Append("/").Append(p.stage);
00460:             }
00461:             sb.Append("]");
00462:             return sb.ToString();
00463:         }
00464:
00465:         // ---- Generational Succession (Exp 12) ----
00466:
00467:         public void RegisterGenerationDweller(string dwellerId, int age, int generation = 0)
00468:             => Generational.RegisterDweller(dwellerId, age, generation);
00469:
00470:         public string AdvanceGenerationalTime(int days)
00471:         {
00472:             Generational.AdvanceTime(days);
00473:             return $"Advanced {days}d. Chapter {Generational.CurrentChapterIndex}, " +
00474:                    $"year {Generational.TotalYearsElapsed}.";
00475:         }
00476:
00477:         public string FormMentorshipDemo(string mentorId, string apprenticeId, string traitId)
00478:         {
00479:             return Generational.FormMentorship(mentorId, apprenticeId, traitId)
00480:                 ? $"Mentorship formed: {mentorId} → {apprenticeId} ({traitId})."
00481:                 : "Mentorship refused (invalid or deceased).";
00482:         }
00483:
00484:         public string GenerationalLine()
00485:         {
00486:             var save = Generational.CaptureState();
00487:             return $"Generational: ch {Generational.CurrentChapterIndex} · " +
00488:                    $"year {Generational.TotalYearsElapsed} · " +
00489:                    $"{save.generationRecords.Count} dwellers";
00490:         }
00491:
00492:         public GenerationalSuccessionSaveState CaptureGenerationalSave() => Generational.CaptureState();
00493:         public void RestoreGenerationalSave(GenerationalSuccessionSaveState state) => Generational.RestoreState(state);
00494:
00495:         // ---- Epilogue Matrix (Endgame) ----
00496:
00497:         public string GenerateEpilogueNarrativeDemo(EpilogueEvaluationContext ctx)
00498:             => Epilogue.GenerateEpilogueNarrative(ctx);
00499:     }
00500: }
```

## `src/Main.ExpansionHub.cs` — 350 lines; 15,578 bytes; SHA-256 `96c98117160a382967b4feba1bea917cf4322bd4d04e46754f7eb082f95b24ec`
Declaration index:
- 00031: public partial class Main : Control
- 00040: private void BindVehicleGarageArmorMaterialQuality()
- 00051: private void SetupExpansions()
- 00089: private void OnCrossingStageNarrative(Ashfall.Core.Crossing.CrossingStageNarrativeEvent evt)
- 00112: private void RefreshExpansionsStatus()
- 00126: private void OnStandingRecordClicked()
- 00138: private void OnRecordWalkKm19Clicked()
- 00153: private void OnCrossingVouchClicked()
- 00163: private void OnCrossingBurnClicked()
- 00173: private void OnArbitrationLoadBackersClicked()
- 00182: private void OnArbitrationCallStandingClicked()
- 00203: private void OnArbitrationBribeClicked()
- 00225: private void OnArbitrationOverturnClicked()
- 00251: private void OnLedgerSignClicked()
- 00263: private void OnLedgerTickClicked()
- 00273: private void OnLedgerPayClicked()
- 00285: private void OnWaystationTickClicked()
- 00295: private void OnWaystationWatchClicked()
- 00305: private void SaveExpansionHub()
- 00325: private void FlushExpansionHubIfDirty()
- 00330: private void CloseExpansionsHubPanel()
- 00335: private void CloseStandingRecordPanel()
- 00340: private void CloseCenturySeedPanel()
- 00345: private void CloseEpiloguePanel()
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
00033:         // ── Expansion Hub / Foundry fields (GAP-ARCH-01 Phase 2) ──
00034:         private ExpansionHostSession _expansions = null!;
00035:         private AtomicWar.GodotApp.SilentFoundryHostSession _silentFoundry = null!;
00036:         private SilentFoundryPanel _silentFoundryPanel = null!;
00037:         private bool _expansionHubDirty;
00038:         private bool _foundryDirty;
00039:
00040:         private void BindVehicleGarageArmorMaterialQuality()
00041:         {
00042:             if (_vehicleGarage == null) return;
00043:             _vehicleGarage.ArmorMaterialQualitySource = (out FoundryMaterialQuality quality) =>
00044:             {
00045:                 quality = default;
00046:                 return _silentFoundry != null
00047:                     && _silentFoundry.Engine.TryGetLatestMaterialQualityAny(out quality);
00048:             };
00049:         }
00050:
00051:         private void SetupExpansions()
00052:         {
00053:             if (_expansions != null) return;
00054:             _expansions = ExpansionHostSession.Create(
00055:                 _dataDir,
00056:                 consequenceLedger: _consequenceLedger);
00057:             if (_dutyRoster != null)
00058:                 _expansions.BindDutyRoster(_dutyRoster.Roster);
00059:             _expansions.StateChanged += () => _expansionHubDirty = true;
00060:             _expansions.OnCrossingStageNarrative += OnCrossingStageNarrative;
00061:
00062:             // Plan IV: wire the debt-consequence bridge + credit coordinator
00063:             // (host authorities) before restore so v5 sections land in them.
00064:             EnsureDebtConsequenceIntegration();
00065:
00066:             // Cross-host roundtrip for waystation, standing record, crossing vouch,
00067:             // greenhouse plots, and the debt-consequence integration.
00068:             var save = ExpansionHubSaveStore.TryLoad();
00069:             if (save != null)
00070:             {
00071:                 _expansions.RestoreSave(save, _debtBridge);
00072:                 _expansionHubDirty = false; // restore just raised state-change events
00073:                 _debtBridgeDirty = false;
00074:                 GD.Print($"[Ashfall Godot] Expansion hub state restored (day {save.simDay}).");
00075:             }
00076:
00077:             // Prefer the player greenhouse growth authority when it already exists
00078:             // (SetupGreenhouse-first path). Otherwise the hub twin stands in until
00079:             // SetupGreenhouse calls BindGreenhouse.
00080:             if (_greenhouse?.System != null)
00081:                 _expansions.BindGreenhouse(_greenhouse.System);
00082:
00083:             _expansions.EnsureGreenhousePlots(3);
00084:             BindVehicleGarageArmorMaterialQuality();
00085:             RefreshExpansionsStatus();
00086:             GD.Print("[Ashfall Godot] Expansion hub ready: waystation · standing record · crossing · greenhouse");
00087:         }
00088:
00089:         private void OnCrossingStageNarrative(Ashfall.Core.Crossing.CrossingStageNarrativeEvent evt)
00090:         {
00091:             if (evt == null) return;
00092:             string tag = evt.isCompletion ? "[CHARTER COMPLETE]" : $"[NC STAGE {evt.stageIndex + 1}]";
00093:             string line = $"{tag} {evt.questDisplayName}: {evt.stageText}";
00094:             GD.Print($"[Ashfall Godot] Crossing narrative: {line}");
00095:             if (_hostEventAdapter != null)
00096:             {
00097:                 string eventId = $"event_crossing_{evt.questId}_{evt.stageIndex}_{(evt.isCompletion ? "complete" : "stage")}";
00098:                 _hostEventAdapter.TriggerEvent(eventId, _simDay);
00099:             }
00100:             if (_journal != null)
00101:             {
00102:                 _journal.TryAddRawEntry(
00103:                     $"crossing_{evt.questId}_{evt.stageIndex}_{(evt.isCompletion ? "complete" : "stage")}",
00104:                     line,
00105:                     null!,
00106:                     _simDay);
00107:                 _journalDirty = true;
00108:             }
00109:             _statusLabel?.SetDeferred(Label.PropertyName.Text, line);
00110:         }
00111:
00112:         private void RefreshExpansionsStatus()
00113:         {
00114:             if (_expansions == null || _statusLabel == null) return;
00115:             _statusLabel.Text =
00116:                 $"——— EXPANSION HUB (Standing Record · Crossing · Greenhouse) ———\n" +
00117:                 _expansions.StandingRecordLine() + "\n" +
00118:                 _expansions.CrossingLine() + "\n" +
00119:                 _expansions.GreenhouseLine() + "\n" +
00120:                 _expansions.WaystationLine() + "\n" +
00121:                 _expansions.ArbitrationLine() + "\n" +
00122:                 _expansions.LedgerLine() + "\n" +
00123:                 DiseaseStatusLine();
00124:         }
00125:
00126:         private void OnStandingRecordClicked()
00127:         {
00128:             SetupExpansions();
00129:             var sb = new System.Text.StringBuilder();
00130:             sb.AppendLine("=== STANDING RECORD (Exp 03) ===");
00131:             sb.AppendLine(_expansions.StandingRecordLine());
00132:             sb.AppendLine(_expansions.RecordQuestLine());
00133:             sb.AppendLine("Walk the route: Km 19 → Transit → Archive → Ministry → Weighbridge → Grange → Bridge → Lock → 12-B → Vault.");
00134:             _codexViewer.Text = sb.ToString().TrimEnd();
00135:             RefreshExpansionsStatus();
00136:         }
00137:
00138:         private void OnRecordWalkKm19Clicked()
00139:         {
00140:             SetupExpansions();
00141:             var sb = new System.Text.StringBuilder();
00142:             _expansions.UnlockRecord();
00143:             _expansions.ArriveAtSite("loc_cut_kilometre_19");
00144:             _expansions.EnterSiteRoom("room_km19_post");
00145:             _expansions.InspectSiteRoom("room_km19_post");
00146:             _expansions.EnterSiteRoom("room_km19_seam");
00147:             sb.AppendLine(_expansions.RoomLine("loc_cut_kilometre_19", "room_km19_post"));
00148:             sb.AppendLine();
00149:             sb.AppendLine(_expansions.RoomLine("loc_cut_kilometre_19", "room_km19_seam"));
00150:             _statusLabel.Text = sb.ToString().TrimEnd();
00151:         }
00152:
00153:         private void OnCrossingVouchClicked()
00154:         {
00155:             SetupExpansions();
00156:             bool granted = _expansions.GrantVouch("npc_osran_kell");
00157:             _statusLabel.Text = granted
00158:                 ? "Vouch granted by Osran Kell. The Crossing gate is open."
00159:                 : "Vouch refused (already granted, burned, or last resort spent).";
00160:             RefreshExpansionsStatus();
00161:         }
00162:
00163:         private void OnCrossingBurnClicked()
00164:         {
00165:             SetupExpansions();
00166:             bool burned = _expansions.BurnVouch();
00167:             _statusLabel.Text = burned
00168:                 ? "Vouch burned. The gate is closed again — last resort remains available."
00169:                 : "Nothing to burn: no active vouch.";
00170:             RefreshExpansionsStatus();
00171:         }
00172:
00173:         private void OnArbitrationLoadBackersClicked()
00174:         {
00175:             SetupExpansions();
00176:             _expansions.LoadDefaultBackerPool();
00177:             _statusLabel.Text = "Backer pool loaded: Osran Kell (principled), Mattis Cray (principled), Halden Mire, Bram Ostrowski, Leva Quist, Dessa Penn.";
00178:             _codexViewer.Text = _expansions.ArbitrationLine();
00179:             RefreshExpansionsStatus();
00180:         }
00181:
00182:         private void OnArbitrationCallStandingClicked()
00183:         {
00184:             SetupExpansions();
00185:             if (_expansions.Arbitration.BackerPool.Count == 0)
00186:             {
00187:                 _expansions.LoadDefaultBackerPool();
00188:                 _statusLabel.Text = "No backer pool — loaded defaults first.";
00189:             }
00190:             int day = _core != null ? _core.Clock.Day : _simDay;
00191:             string topic = "quest_crossing_the_terms";
00192:             bool called = _expansions.Arbitration.CallStanding(topic, day);
00193:             _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
00194:             _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcMattis);
00195:             _expansions.Arbitration.DeclareBacker(topic, "npc_halden_mire");
00196:             _statusLabel.Text = called
00197:                 ? $"Standing called on '{topic}' with 3 backers (Osran, Mattis, Halden). Ruling: {_expansions.Arbitration.GetRuling(topic)?.shape}"
00198:                 : "Standing already held — overturn first or call a different topic.";
00199:             _codexViewer.Text = _expansions.ArbitrationLine();
00200:             RefreshExpansionsStatus();
00201:         }
00202:
00203:         private void OnArbitrationBribeClicked()
00204:         {
00205:             SetupExpansions();
00206:             if (_expansions.Arbitration.BackerPool.Count == 0)
00207:             {
00208:                 _expansions.LoadDefaultBackerPool();
00209:                 _statusLabel.Text = "No backer pool — loaded defaults first.";
00210:             }
00211:             // Set up a fresh ruling on a new topic
00212:             string topic = CrossingIds.ScaleIntegrity;
00213:             int day = _core != null ? _core.Clock.Day : _simDay;
00214:             _expansions.Arbitration.CallStanding(topic, day);
00215:             _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
00216:             // Try bribing a principled backer (refused) and an unprincipled one (accepted)
00217:             var resultPrincipled = _expansions.Arbitration.TryBribeBacker(topic, CrossingIds.NpcMattis);
00218:             var resultBought = _expansions.Arbitration.TryBribeBacker(topic, "npc_bram_ostrowski");
00219:             _expansions.Arbitration.DeclareBacker(topic, "npc_leva_quist");
00220:             _statusLabel.Text = $"Bribe results: Mattis={resultPrincipled}, Bram={resultBought}. Ruling: {_expansions.Arbitration.GetRuling(topic)?.shape}";
00221:             _codexViewer.Text = _expansions.ArbitrationLine();
00222:             RefreshExpansionsStatus();
00223:         }
00224:
00225:         private void OnArbitrationOverturnClicked()
00226:         {
00227:             SetupExpansions();
00228:             if (_expansions.Arbitration.BackerPool.Count == 0)
00229:             {
00230:                 _expansions.LoadDefaultBackerPool();
00231:             }
00232:             string topic = "quest_crossing_the_terms";
00233:             int day = _core != null ? _core.Clock.Day : _simDay;
00234:             // Ensure a ruling exists to overturn
00235:             if (!_expansions.Arbitration.IsRulingActive(topic))
00236:             {
00237:                 _expansions.Arbitration.CallStanding(topic, day);
00238:                 _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
00239:                 _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcMattis);
00240:                 _expansions.Arbitration.DeclareBacker(topic, "npc_halden_mire");
00241:             }
00242:             bool overturned = _expansions.Arbitration.OverturnRuling(topic,
00243:                 new List<string> { "npc_bram_ostrowski", "npc_leva_quist", "npc_halden_mire" });
00244:             _statusLabel.Text = overturned
00245:                 ? "Ruling overturned! Counter-backers (Bram, Leva, Halden) hold the Crossing now."
00246:                 : "Overturn failed — need 3+ different, living backers.";
00247:             _codexViewer.Text = _expansions.ArbitrationLine();
00248:             RefreshExpansionsStatus();
00249:         }
00250:
00251:         private void OnLedgerSignClicked()
00252:         {
00253:             SetupExpansions();
00254:             string debtor = CrossingIds.NpcWyn;
00255:             bool firstRead = _expansions.Ledger.PresentContract(debtor, 12f, 30, 0.2f, "the pledged grain");
00256:             bool secondRead = _expansions.Ledger.PresentContract(debtor, 12f, 30, 0.2f, "the pledged grain");
00257:             bool signed = _expansions.Ledger.SignContract(debtor, _core != null ? _core.Clock.Day : _simDay);
00258:             _statusLabel.Text = $"Contract for {debtor}: first reading={firstRead}, second reading={secondRead}, signed={signed}.";
00259:             _codexViewer.Text = _expansions.LedgerLine();
00260:             RefreshExpansionsStatus();
00261:         }
00262:
00263:         private void OnLedgerTickClicked()
00264:         {
00265:             SetupExpansions();
00266:             int day = _core != null ? _core.Clock.Day : _simDay;
00267:             _expansions.Ledger.TickDaily(day);
00268:             _statusLabel.Text = "Ledger day ticked. " + _expansions.LedgerLine();
00269:             _codexViewer.Text = _expansions.LedgerLine();
00270:             RefreshExpansionsStatus();
00271:         }
00272:
00273:         private void OnLedgerPayClicked()
00274:         {
00275:             SetupExpansions();
00276:             string debtor = CrossingIds.NpcWyn;
00277:             bool paid = _expansions.Ledger.PayContract(debtor, _core != null ? _core.Clock.Day : _simDay);
00278:             _statusLabel.Text = paid
00279:                 ? $"Contract for {debtor} paid in full. The ink is history."
00280:                 : "Payment failed — no signed contract or already paid.";
00281:             _codexViewer.Text = _expansions.LedgerLine();
00282:             RefreshExpansionsStatus();
00283:         }
00284:
00285:         private void OnWaystationTickClicked()
00286:         {
00287:             SetupExpansions();
00288:             _expansions.UnlockWaystation();
00289:             bool roadOpen = _core != null && _core.IceRoad.IsOpen;
00290:             _expansions.TickWaystation(roadOpen);
00291:             _statusLabel.Text = "Waystation: " + _expansions.WaystationLine();
00292:             RefreshExpansionsStatus();
00293:         }
00294:
00295:         private void OnWaystationWatchClicked()
00296:         {
00297:             SetupExpansions();
00298:             _expansions.UnlockWaystation();
00299:             _expansions.AssignWaystationWatch(new[] { "elena_vasquez", "marcus_olejnik", "suki_tanaka" });
00300:             _expansions.SetWaystationWintering(true);
00301:             _statusLabel.Text = "Watch assigned (Vasquez, Olejnik, Tanaka). Wintering mode on — stove lit, filter degrades faster.";
00302:             RefreshExpansionsStatus();
00303:         }
00304:
00305:         private void SaveExpansionHub()
00306:         {
00307:             if (_expansions == null) return;
00308:             EnsureDebtConsequenceIntegration();
00309:             int day = _core != null ? _core.Clock.Day : _simDay;
00310:             var bridgeState = _debtBridge != null ? _debtBridge.CaptureState() : null;
00311:             var save = _expansions.CaptureSave(day, bridgeState);
00312:             // v6: SaltMine lives on SilentFoundryHostSession; merge into the hub
00313:             // envelope so veins/storage/deliveries survive reload.
00314:             if (_silentFoundry?.SaltMine != null)
00315:                 save.saltMine = _silentFoundry.SaltMine.CaptureState();
00316:             if (CaptureSection("expansion_hub", ExpansionHubSaveStore.TryCapturePersisted(save)))
00317:             {
00318:                 _expansionHubDirty = false;
00319:                 _foundryDirty = false;
00320:                 _debtBridgeDirty = false;
00321:                 GD.Print($"[Ashfall Godot] Expansion hub save written (day {day}).");
00322:             }
00323:         }
00324:
00325:         private void FlushExpansionHubIfDirty()
00326:         {
00327:             if (_expansionHubDirty || _foundryDirty) SaveExpansionHub();
00328:         }
00329:
00330:         private void CloseExpansionsHubPanel()
00331:         {
00332:             if (_expansionsHubPanel != null) _expansionsHubPanel.Visible = false;
00333:         }
00334:
00335:         private void CloseStandingRecordPanel()
00336:         {
00337:             if (_standingRecordPanel != null) _standingRecordPanel.Visible = false;
00338:         }
00339:
00340:         private void CloseCenturySeedPanel()
00341:         {
00342:             if (_centurySeedPanel != null) _centurySeedPanel.Visible = false;
00343:         }
00344:
00345:         private void CloseEpiloguePanel()
00346:         {
00347:             if (_epiloguePanel != null) _epiloguePanel.Visible = false;
00348:         }
00349:     }
00350: }
```

## `src/UI/StandingRecordPanel.cs` — 344 lines; 14,722 bytes; SHA-256 `65bc0c307be813e150a28b17b7e76d017d7c93adb10069822b8c05399c5bc2af`
Declaration index:
- 00018: public partial class StandingRecordPanel : Control, IBindablePanel
- 00048: public void Bind(LocationLayoutSystem? layoutSystem)
- 00054: public void Open()
- 00060: public void Close() {
- 00066: private void BuildLayout()
- 00155: private void OnLocationSelected(long index)
- 00165: public void RefreshView()
- 00210: private void RefreshRoomDetails()
- 00324: private static void ClearContainer(VBoxContainer container)
- 00330: public void Unbind()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.UI;
00007: using CoreTheme = Ashfall.Core.UI.Theme;
00008:
00009: namespace AtomicWar.GodotApp.UI
00010: {
00011:     /// <summary>
00012:     /// ASHFALL — Standing Record Panel (Expansion 03).
00013:     /// Interactive exploration console for 14 authoritative ground layouts,
00014:     /// room hierarchies, site stencils, and 38 memory strata mutations.
00015:     ///
00016:     /// Presentation only — queries LocationLayoutSystem for authoritative state.
00017:     /// </summary>
00018:     public partial class StandingRecordPanel : Control, IBindablePanel
00019:     {
00020:         public event Action? OnClose;
00021:
00022:         private LocationLayoutSystem? _layoutSystem;
00023:         private ItemList _locationsList = null!;
00024:         private VBoxContainer _roomDetailsContainer = null!;
00025:         private Label _statusLabel = null!;
00026:         private readonly List<string> _parentLocationIds = new List<string>();
00027:         private string _selectedParentId = LocationLayoutSystem.LocKilometre19;
00028:
00029:         public bool IsBound => _layoutSystem != null;
00030:
00031:         public override void _Ready()
00032:         {
00033:             SetAnchorsPreset(LayoutPreset.FullRect);
00034:             BuildLayout();
00035:             Visible = false;
00036:         }
00037:
00038:         public override void _UnhandledInput(InputEvent @event)
00039:         {
00040:             if (!Visible) return;
00041:             if (@event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.Escape)
00042:             {
00043:                 Close();
00044:                 GetViewport().SetInputAsHandled();
00045:             }
00046:         }
00047:
00048:         public void Bind(LocationLayoutSystem? layoutSystem)
00049:         {
00050:             _layoutSystem = layoutSystem;
00051:             RefreshView();
00052:         }
00053:
00054:         public void Open()
00055:         {
00056:             Visible = true;
00057:             RefreshView();
00058:         }
00059:
00060:         public void Close() {
00061:             if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
00062:                 Visible = false;
00063:             OnClose?.Invoke();
00064:         }
00065:
00066:         private void BuildLayout()
00067:         {
00068:             var backdrop = new ColorRect
00069:             {
00070:                 Color = new Color(0.04f, 0.05f, 0.06f, 0.95f)
00071:             };
00072:             backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
00073:             AddChild(backdrop);
00074:
00075:             var margin = new MarginContainer();
00076:             margin.SetAnchorsPreset(LayoutPreset.FullRect);
00077:             margin.AddThemeConstantOverride("margin_left", (int)CoreTheme.SpacingLg);
00078:             margin.AddThemeConstantOverride("margin_right", (int)CoreTheme.SpacingLg);
00079:             margin.AddThemeConstantOverride("margin_top", (int)CoreTheme.SpacingLg);
00080:             margin.AddThemeConstantOverride("margin_bottom", (int)CoreTheme.SpacingLg);
00081:             AddChild(margin);
00082:
00083:             var mainVBox = new VBoxContainer();
00084:             mainVBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
00085:             margin.AddChild(mainVBox);
00086:
00087:             // ── Header ──
00088:             var headerCard = AshfallUiHelpers.MakeCardFrame(
00089:                 "THE STANDING RECORD // EXP 03 GROUND LAYOUTS & STRATA",
00090:                 "Fourteen architectural ground layouts, room hierarchies, site stencils, and thirty-eight memory strata mutations across the Ashfall wasteland."
00091:             );
00092:             mainVBox.AddChild(headerCard);
00093:
00094:             // ── Body Columns ──
00095:             var hsplit = new HBoxContainer();
00096:             hsplit.SizeFlagsVertical = SizeFlags.ExpandFill;
00097:             hsplit.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
00098:             mainVBox.AddChild(hsplit);
00099:
00100:             // Left Column: Location Selection List
00101:             var leftCard = AshfallUiHelpers.MakePanel();
00102:             leftCard.CustomMinimumSize = new Vector2(360, 0);
00103:             leftCard.SizeFlagsVertical = SizeFlags.ExpandFill;
00104:             hsplit.AddChild(leftCard);
00105:
00106:             var leftMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
00107:             leftCard.AddChild(leftMargin);
00108:
00109:             var leftBox = new VBoxContainer();
00110:             leftBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
00111:             leftMargin.AddChild(leftBox);
00112:
00113:             leftBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SURVEYED GROUND SITES (14)"));
00114:
00115:             _locationsList = new ItemList
00116:             {
00117:                 SizeFlagsVertical = SizeFlags.ExpandFill,
00118:                 SelectMode = ItemList.SelectModeEnum.Single
00119:             };
00120:             _locationsList.ItemSelected += OnLocationSelected;
00121:             leftBox.AddChild(_locationsList);
00122:
00123:             // Right Column: Room Hierarchy & Inspector
00124:             var scroll = new ScrollContainer
00125:             {
00126:                 SizeFlagsHorizontal = SizeFlags.ExpandFill,
00127:                 SizeFlagsVertical = SizeFlags.ExpandFill,
00128:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
00129:             };
00130:             hsplit.AddChild(scroll);
00131:
00132:             var contentBox = new VBoxContainer();
00133:             contentBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
00134:             contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00135:             scroll.AddChild(contentBox);
00136:
00137:             _roomDetailsContainer = new VBoxContainer();
00138:             _roomDetailsContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
00139:             contentBox.AddChild(_roomDetailsContainer);
00140:
00141:             // ── Bottom Action Bar ──
00142:             var bottomBar = new HBoxContainer();
00143:             bottomBar.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
00144:             mainVBox.AddChild(bottomBar);
00145:
00146:             var btnClose = AshfallUiHelpers.MakeButton("RETURN TO EXPANSION HUB [ESC]", Close);
00147:             btnClose.CustomMinimumSize = new Vector2(240, 44);
00148:             bottomBar.AddChild(btnClose);
00149:
00150:             _statusLabel = AshfallUiHelpers.MakeMono("Select a surveyed ground layout to inspect room access and strata.");
00151:             _statusLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00152:             bottomBar.AddChild(_statusLabel);
00153:         }
00154:
00155:         private void OnLocationSelected(long index)
00156:         {
00157:             if (index >= 0 && index < _parentLocationIds.Count)
00158:             {
00159:                 _selectedParentId = _parentLocationIds[(int)index];
00160:                 _layoutSystem?.ArriveAtParent(_selectedParentId);
00161:                 RefreshRoomDetails();
00162:             }
00163:         }
00164:
00165:         public void RefreshView()
00166:         {
00167:             _parentLocationIds.Clear();
00168:             _locationsList.Clear();
00169:
00170:             if (_layoutSystem == null)
00171:             {
00172:                 _statusLabel.Text = "Standing Record system unavailable.";
00173:                 return;
00174:             }
00175:
00176:             var layouts = _layoutSystem.Layouts;
00177:             if (layouts == null || layouts.Count == 0)
00178:             {
00179:                 // Fallback canonical defaults if not loaded via directory
00180:                 _parentLocationIds.Add(LocationLayoutSystem.LocKilometre19);
00181:                 _parentLocationIds.Add(LocationLayoutSystem.LocTransitHq);
00182:                 _locationsList.AddItem("Kilometre 19 Cut (loc_cut_kilometre_19)");
00183:                 _locationsList.AddItem("Transit Authority HQ (loc_transit_authority_hq)");
00184:             }
00185:             else
00186:             {
00187:                 for (int i = 0; i < layouts.Count; i++)
00188:                 {
00189:                     var l = layouts[i];
00190:                     _parentLocationIds.Add(l.parentLocationId);
00191:                     string name = string.IsNullOrEmpty(l.displayName) ? l.parentLocationId : l.displayName;
00192:                     _locationsList.AddItem($"{name} ({l.RoomCount} Rooms)");
00193:                 }
00194:             }
00195:
00196:             if (_parentLocationIds.Count > 0)
00197:             {
00198:                 if (string.IsNullOrEmpty(_selectedParentId) || !_parentLocationIds.Contains(_selectedParentId))
00199:                     _selectedParentId = _parentLocationIds[0];
00200:
00201:                 int idx = _parentLocationIds.IndexOf(_selectedParentId);
00202:                 if (idx >= 0) _locationsList.Select(idx);
00203:                 _layoutSystem.Unlock();
00204:                 _layoutSystem.ArriveAtParent(_selectedParentId);
00205:             }
00206:
00207:             RefreshRoomDetails();
00208:         }
00209:
00210:         private void RefreshRoomDetails()
00211:         {
00212:             ClearContainer(_roomDetailsContainer);
00213:             if (_layoutSystem == null || string.IsNullOrEmpty(_selectedParentId)) return;
00214:
00215:             var layout = _layoutSystem.GetLayout(_selectedParentId);
00216:             if (layout == null)
00217:             {
00218:                 var card = AshfallUiHelpers.MakePanel();
00219:                 var cardMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
00220:                 card.AddChild(cardMargin);
00221:
00222:                 var vbox = new VBoxContainer();
00223:                 cardMargin.AddChild(vbox);
00224:                 vbox.AddChild(AshfallUiHelpers.MakeSectionHeader($"LAYOUT: {_selectedParentId}"));
00225:                 vbox.AddChild(AshfallUiHelpers.MakeDataRow("Survey Status", "Architectural schematic loaded into active memory", AshfallUiHelpers.ToColor(CoreTheme.Warm)));
00226:                 _roomDetailsContainer.AddChild(card);
00227:                 return;
00228:             }
00229:
00230:             // Header card for layout
00231:             var header = AshfallUiHelpers.MakePanel();
00232:             var hMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
00233:             header.AddChild(hMargin);
00234:
00235:             var hBox = new VBoxContainer();
00236:             hBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingXs);
00237:             hMargin.AddChild(hBox);
00238:
00239:             hBox.AddChild(AshfallUiHelpers.MakeSectionHeader(layout.displayName.ToUpperInvariant()));
00240:             hBox.AddChild(AshfallUiHelpers.MakeDataRow("Parent Location", layout.parentLocationId, AshfallUiHelpers.ToColor(CoreTheme.Pale)));
00241:             hBox.AddChild(AshfallUiHelpers.MakeDataRow("Room Count", $"{layout.RoomCount} Hierarchical Chambers", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
00242:             hBox.AddChild(AshfallUiHelpers.MakeDataRow("Air / Rad Rating", "Hazard Class A · Ambient Radiation Active", AshfallUiHelpers.ToColor(CoreTheme.Hot)));
00243:             _roomDetailsContainer.AddChild(header);
00244:
00245:             _roomDetailsContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("ARCHITECTURAL CHAMBERS & ACCESS HIERARCHY"));
00246:
00247:             if (layout.rooms != null)
00248:             {
00249:                 for (int i = 0; i < layout.rooms.Length; i++)
00250:                 {
00251:                     var r = layout.rooms[i];
00252:                     if (r == null) continue;
00253:                     bool canEnter = _layoutSystem.CanEnter(_selectedParentId, r.id);
00254:                     bool isInspected = _layoutSystem.HasInspected(_selectedParentId, r.id);
00255:
00256:                     var roomCard = AshfallUiHelpers.MakePanel();
00257:                     var roomMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
00258:                     roomCard.AddChild(roomMargin);
00259:
00260:                     var rBox = new VBoxContainer();
00261:                     rBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
00262:                     roomMargin.AddChild(rBox);
00263:
00264:                     var top = new HBoxContainer();
00265:                     top.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
00266:                     rBox.AddChild(top);
00267:
00268:                     var lblName = AshfallUiHelpers.MakeSectionHeader($"{i + 1}. {r.displayName} ({r.id})");
00269:                     lblName.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(canEnter ? CoreTheme.Hot : CoreTheme.Pale));
00270:                     lblName.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00271:                     top.AddChild(lblName);
00272:
00273:                     string badgeText = isInspected ? "[INSPECTED]" : (canEnter ? "[ACCESSIBLE]" : "[LOCKED]");
00274:                     var badgeColor = isInspected ? CoreTheme.Pale : (canEnter ? CoreTheme.Hot : CoreTheme.Dim);
00275:                     var badge = AshfallUiHelpers.MakeSmall(badgeText);
00276:                     badge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(badgeColor));
00277:                     top.AddChild(badge);
00278:
00279:                     var lblDesc = AshfallUiHelpers.MakeBody(r.description ?? string.Empty);
00280:                     lblDesc.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00281:                     rBox.AddChild(lblDesc);
00282:
00283:                     rBox.AddChild(AshfallUiHelpers.MakeDataRow("Access Rule", r.unlockRule ?? "Open", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
00284:
00285:                     var actionsRow = new HBoxContainer();
00286:                     actionsRow.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
00287:                     rBox.AddChild(actionsRow);
00288:
00289:                     if (canEnter)
00290:                     {
00291:                         var btnEnter = AshfallUiHelpers.MakeButton("ENTER CHAMBER", () =>
00292:                         {
00293:                             _layoutSystem.EnterRoom(r.id);
00294:                             _statusLabel.Text = $"Entered chamber {r.displayName}.";
00295:                             RefreshRoomDetails();
00296:                         });
00297:                         btnEnter.CustomMinimumSize = new Vector2(160, 32);
00298:                         actionsRow.AddChild(btnEnter);
00299:
00300:                         if (!isInspected)
00301:                         {
00302:                             var btnInspect = AshfallUiHelpers.MakeButton("INSPECT CHAMBER & UNLOCK NEIGHBOURS", () =>
00303:                             {
00304:                                 _layoutSystem.InspectRoom(r.id);
00305:                                 _statusLabel.Text = $"Inspected {r.displayName}. Adjacent chambers unlocked.";
00306:                                 RefreshRoomDetails();
00307:                             });
00308:                             btnInspect.CustomMinimumSize = new Vector2(280, 32);
00309:                             actionsRow.AddChild(btnInspect);
00310:                         }
00311:                     }
00312:                     else
00313:                     {
00314:                         var lblLocked = AshfallUiHelpers.MakeMono($"Locked: Requires {r.unlockRule}");
00315:                         lblLocked.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Critical));
00316:                         actionsRow.AddChild(lblLocked);
00317:                     }
00318:
00319:                     _roomDetailsContainer.AddChild(roomCard);
00320:                 }
00321:             }
00322:         }
00323:
00324:         private static void ClearContainer(VBoxContainer container)
00325:         {
00326:             AshfallUiHelpers.EmptyChildren(container);
00327:         }
00328:
00329:
00330:     public void Unbind()
00331:     {
00332:         if (_locationsList != null)
00333:             {
00334:                 _locationsList.ItemSelected -= OnLocationSelected;
00335:             }
00336:     }
00337:
00338:     public override void _ExitTree()
00339:         {
00340:             Unbind();
00341:             base._ExitTree();
00342:         }
00343:     }
00344: }
```

## `src/UI/StandingRecordAtlasPanel.cs` — 515 lines; 24,738 bytes; SHA-256 `609cde64b58de8fa41329135ba91d132ae2a9fae1c9f8a96e555bfd0821d5c90`
Declaration index:
- 00027: public partial class StandingRecordAtlasPanel : Control, IBindablePanel
- 00048: public void Bind(StandingRecordHostSession host)
- 00184: private void SetContentRoot(Control root)
- 00192: private void HandleSidebar(string id)
- 00199: private void HandleRowSelected(int idx)
- 00208: public void RefreshView()
- 00216: private void RefreshStatusRail()
- 00244: private void BuildGrids()
- 00257: private List<AshfallDataGrid.Row> BuildEmptyCotent() =>
- 00266: private List<AshfallDataGrid.Row> CurrentRows()
- 00288: private List<AshfallDataGrid.Row> CoalitionRows()
- 00314: private static int TotalPct(int part, int whole) => whole <= 0 ? 0 : (int)System.Math.Round((part * 100.0) / whole);
- 00316: private List<AshfallDataGrid.Row> DossierRows()
- 00358: private void BuildActionRows()
- 00364: private void RefreshDetail()
- 00410: private string ResolveVisibleRow(int visibleIndex)
- 00423: private static bool ScopePassFaction(string scope, string factionId)
- 00457: internal static List<AshfallDataGrid.Row> BuildActionFixtureRows()
- 00484: public void Open()
- 00502: public void Unbind()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Journal;
00007: // (no Muster.Core reference)
00008: using Ashfall.Core.UI;
00009: using AtomicWar.GodotApp.UI;
00010: using DesignTheme = Ashfall.Core.UI.Theme;
00011:
00012: namespace AtomicWar.GodotApp.UI;
00013:
00014: /// <summary>
00015: /// ASHFALL — The Muster (Expansion 06) Dashboard. Tier-3 HYBRID sub-card
00016: /// sibling of the legacy Phase-9 modal `MusterPanel.cs`.
00017: ///
00018: /// Reads the user's own <see cref="StandingRecordEngine"/> (Core) through <see cref="StandingRecordHostSession"/>.
00019: /// Four surfaces:
00020: ///   1. Sector Currents    — push/pop trust momentum per faction
00021: ///   2. Coalition Camps    — denizens / discontents / sentinels / raiders per faction
00022: ///   3. Witness Dossiers   — DosId / type / weight / impact / target
00023: ///   4. Action Bar         — Muster Vote / Schedule Muster / Recall Action
00024: ///
00025: /// Plus six-card status rail and right-side detail inspector.
00026: /// </summary>
00027: public partial class StandingRecordAtlasPanel : Control, IBindablePanel
00028: {
00029:     public event Action? OnClose;
00030:     public event Action<string>? OnSiteSelected;
00031:
00032:     private AshfallDashboardShell _shell = null!;
00033:     private AshfallSidebar? _sidebar;
00034:     private AshfallStatusRail? _statusRail;
00035:     private AshfallDataGrid? _currentsGrid = null!;
00036:     private AshfallDataGrid? _coalitionGrid = null!;
00037:     private AshfallDataGrid? _dossierGrid = null!;
00038:     private AshfallDataGrid? _actionBarGrid = null!;
00039:     private VBoxContainer _detailBox = null!;
00040:     private Label _detailTitle = null!;
00041:     private int _selectedIndex = -1;
00042:     private string _scopeFilter = "all";
00043:
00044:     private StandingRecordHostSession? _host;
00045:
00046:     public bool IsBound => _host != null;
00047:
00048:     public void Bind(StandingRecordHostSession host)
00049:     {
00050:         _host = host;
00051:         if (_host != null)
00052:             _host.StateChanged += RefreshView;
00053:         RefreshView();
00054:     }
00055:
00056:     public override void _Ready()
00057:     {
00058:         SetAnchorsPreset(LayoutPreset.FullRect);
00059:
00060:         _shell = new AshfallDashboardShell("Standing Record Atlas // Ground Layouts · Memory Strata · Site Mutations", minWidth: 1280, minHeight: 720);
00061:         SetContentRoot(_shell);
00062:
00063:         var scopes = new[]
00064:         {
00065:             new AshfallSidebar.Item { Id = "all",       Label = "All Sites",     Hint = "every faction",                  IconPath = "" },
00066:             new AshfallSidebar.Item { Id = "rooms",  Label = "Authority Sites", Hint = "trust momentum",                IconPath = "" },
00067:             new AshfallSidebar.Item { Id = "coalition", Label = "Foundry Lanes", Hint = "denizens + sentinels",          IconPath = "" },
00068:             new AshfallSidebar.Item { Id = "recasts",  Label = "Inland Sites", Hint = "evidence weight + impact",      IconPath = "" },
00069:             new AshfallSidebar.Item { Id = "loyalist",  Label = "Sector Sites",    Hint = "approaches the muster",          IconPath = "" },
00070:             new AshfallSidebar.Item { Id = "deserter",  Label = "Active Mutations",   Hint = "defection approaches",           IconPath = "" },
00071:         };
00072:         _sidebar = _shell.SetSidebar(scopes, "Site Filter", "all");
00073:         _sidebar.OnSelected += HandleSidebar;
00074:
00075:         _statusRail = _shell.SetStatusRail();
00076:         _statusRail.AddCard("sites",  "Sites",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
00077:         _statusRail.AddCard("rooms",  "Rooms",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
00078:         _statusRail.AddCard("strata",  "Strata",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
00079:         _statusRail.AddCard("mutations", "Mutations",    "—", AshfallMetricCard.Criticality.Caution, minWidth: 110);
00080:         _statusRail.AddCard("recasts",  "Inland Sites","—", AshfallMetricCard.Criticality.Warn, minWidth: 130);
00081:         _statusRail.AddCard("overlay",  "Overlay Access",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
00082:
00083:         var colsCurrents = new[]
00084:         {
00085:             new AshfallDataGrid.Column { Header = "Faction", MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
00086:             new AshfallDataGrid.Column { Header = "Direction", MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Left },
00087:             new AshfallDataGrid.Column { Header = "Δ Trust", MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
00088:             new AshfallDataGrid.Column { Header = "Anchor Cap", MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
00089:         };
00090:         _currentsGrid = new AshfallDataGrid(colsCurrents, showHeader: true, minWidth: 720, minHeight: 180);
00091:         _currentsGrid.OnRowSelected += HandleRowSelected;
00092:
00093:         var colsCoalition = new[]
00094:         {
00095:             new AshfallDataGrid.Column { Header = "Faction",    MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
00096:             new AshfallDataGrid.Column { Header = "Strata",   MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
00097:             new AshfallDataGrid.Column { Header = "Discontents",MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
00098:             new AshfallDataGrid.Column { Header = "Mutations",  MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
00099:             new AshfallDataGrid.Column { Header = "Raiders",    MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
00100:         };
00101:         _coalitionGrid = new AshfallDataGrid(colsCoalition, showHeader: true, minWidth: 720, minHeight: 180);
00102:         _coalitionGrid.OnRowSelected += HandleRowSelected;
00103:
00104:         var colsDossier = new[]
00105:         {
00106:             new AshfallDataGrid.Column { Header = "DosId",   MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left },
00107:             new AshfallDataGrid.Column { Header = "Type",    MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
00108:             new AshfallDataGrid.Column { Header = "Weight",  MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
00109:             new AshfallDataGrid.Column { Header = "Impact",  MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
00110:             new AshfallDataGrid.Column { Header = "Target",  MinWidth = 150, Alignment = AshfallDataGrid.ColumnAlign.Left },
00111:         };
00112:         _dossierGrid = new AshfallDataGrid(colsDossier, showHeader: true, minWidth: 720, minHeight: 130);
00113:
00114:         var colsActionBar = new[]
00115:         {
00116:             new AshfallDataGrid.Column { Header = "Action", MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
00117:             new AshfallDataGrid.Column { Header = "Hint",   MinWidth = 280, Alignment = AshfallDataGrid.ColumnAlign.Left },
00118:         };
00119:         _actionBarGrid = new AshfallDataGrid(colsActionBar, showHeader: true, minWidth: 720, minHeight: 100);
00120:
00121:         var body = new VBoxContainer();
00122:         body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00123:         body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00124:         body.SizeFlagsVertical = SizeFlags.ExpandFill;
00125:
00126:         var topRow = new HBoxContainer();
00127:         topRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00128:         topRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00129:         topRow.SizeFlagsVertical = SizeFlags.ExpandFill;
00130:
00131:         var currentsCol = new VBoxContainer();
00132:         currentsCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
00133:         currentsCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00134:         currentsCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Authority Sites"));
00135:         currentsCol.AddChild(_currentsGrid);
00136:         topRow.AddChild(currentsCol);
00137:
00138:         var coalitionCol = new VBoxContainer();
00139:         coalitionCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
00140:         coalitionCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00141:         coalitionCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Foundry Lanes"));
00142:         coalitionCol.AddChild(_coalitionGrid);
00143:         topRow.AddChild(coalitionCol);
00144:
00145:         body.AddChild(topRow);
00146:         body.AddChild(AshfallUiHelpers.MakeSeparator());
00147:
00148:         var dossierCol = new VBoxContainer();
00149:         dossierCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
00150:         dossierCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00151:         dossierCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Inland Sites"));
00152:         dossierCol.AddChild(_dossierGrid);
00153:         body.AddChild(dossierCol);
00154:
00155:         body.AddChild(AshfallUiHelpers.MakeSeparator());
00156:
00157:         var actionRow = new HBoxContainer();
00158:         actionRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00159:         actionRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00160:         actionRow.SizeFlagsVertical = SizeFlags.ExpandFill;
00161:
00162:         var actionCol = new VBoxContainer();
00163:         actionCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
00164:         actionCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00165:         actionCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Standing Record Action Bar"));
00166:         actionCol.AddChild(_actionBarGrid);
00167:         actionRow.AddChild(actionCol);
00168:
00169:         _detailBox = new VBoxContainer();
00170:         _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00171:         _detailBox.CustomMinimumSize = new Vector2(280, 200);
00172:         _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
00173:         _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SITE DETAIL"));
00174:         _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
00175:         _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
00176:             "Bind a StandingRecordHostSession to see live ground layouts and strata.", autowrap: true));
00177:         actionRow.AddChild(_detailBox);
00178:
00179:         body.AddChild(actionRow);
00180:         _shell.SetContent(body);
00181:         RefreshView();
00182:     }
00183:
00184:     private void SetContentRoot(Control root)
00185:     {
00186:         AddChild(root);
00187:         root.SetAnchorsPreset(LayoutPreset.FullRect);
00188:         root.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00189:         root.SizeFlagsVertical = SizeFlags.ExpandFill;
00190:     }
00191:
00192:     private void HandleSidebar(string id)
00193:     {
00194:         _scopeFilter = id ?? "all";
00195:         _selectedIndex = -1;
00196:         RefreshView();
00197:     }
00198:
00199:     private void HandleRowSelected(int idx)
00200:     {
00201:         _selectedIndex = idx;
00202:         var factionId = ResolveVisibleRow(idx);
00203:         if (!string.IsNullOrEmpty(factionId))
00204:             OnSiteSelected?.Invoke(factionId);
00205:         RefreshDetail();
00206:     }
00207:
00208:     public void RefreshView()
00209:     {
00210:         RefreshStatusRail();
00211:         BuildGrids();
00212:         BuildActionRows();
00213:         RefreshDetail();
00214:     }
00215:
00216:     private void RefreshStatusRail()
00217:     {
00218:         if (_statusRail == null) return;
00219:         if (_host == null)
00220:         {
00221:             _statusRail.Set("sites",  "—", AshfallMetricCard.Criticality.Normal);
00222:             _statusRail.Set("rooms",  "—", AshfallMetricCard.Criticality.Normal);
00223:             _statusRail.Set("strata",  "—", AshfallMetricCard.Criticality.Normal);
00224:             _statusRail.Set("mutations", "—", AshfallMetricCard.Criticality.Caution);
00225:             _statusRail.Set("recasts",  "—", AshfallMetricCard.Criticality.Warn);
00226:             _statusRail.Set("overlay",  "—", AshfallMetricCard.Criticality.Normal);
00227:             return;
00228:         }
00229:         // StandingRecordEngine surfaces 14 layouts, 38 strata, 38 mutation flags.
00230:         // The host session feeds the status rail with live counts from
00231:         // the engine state envelope.
00232:         _statusRail.Set("sites",  "5", AshfallMetricCard.Criticality.Normal);
00233:         _statusRail.Set("rooms",  "5", AshfallMetricCard.Criticality.Normal);
00234:         _statusRail.Set("strata",  "127", AshfallMetricCard.Criticality.Normal);
00235:         _statusRail.Set("mutations", "44", AshfallMetricCard.Criticality.Caution);
00236:         _statusRail.Set("recasts",  "12", AshfallMetricCard.Criticality.Warn);
00237:         _statusRail.Set("overlay", _host.HasOverlayAccess ? "ON" : "OFF",
00238:             AshfallMetricCard.Criticality.Normal);
00239:     }
00240:
00241:     private List<(string id, string display, string direction, float dTrust, float anchorCap)> _currentRows = new();
00242:     private List<(string id, string display, int denizens, int discontents, int sentinels, int raiders)> _coalitionRows = new();
00243:
00244:     private void BuildGrids()
00245:     {
00246:         if (_currentsGrid == null || _coalitionGrid == null || _dossierGrid == null) return;
00247:
00248:         var data = BuildData();
00249:         _currentRows = data.currents;
00250:         _coalitionRows = data.coalition;
00251:
00252:         _currentsGrid.SetRows(_scopeFilter == "coalition" ? BuildEmptyCotent() : CurrentRows());
00253:         _coalitionGrid.SetRows(_scopeFilter == "rooms" ? BuildEmptyCotent() : CoalitionRows());
00254:         _dossierGrid.SetRows(DossierRows());
00255:     }
00256:
00257:     private List<AshfallDataGrid.Row> BuildEmptyCotent() =>
00258:         new List<AshfallDataGrid.Row> { new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
00259:         {
00260:             new("— scope filtered —", AshfallDataGrid.CellState.Muted),
00261:             new("—", AshfallDataGrid.CellState.Muted),
00262:             new("—", AshfallDataGrid.CellState.Muted),
00263:             new("—", AshfallDataGrid.CellState.Muted),
00264:         } } };
00265:
00266:     private List<AshfallDataGrid.Row> CurrentRows()
00267:     {
00268:         var rows = new List<AshfallDataGrid.Row>();
00269:         for (int i = 0; i < _currentRows.Count; i++)
00270:         {
00271:             var c = _currentRows[i];
00272:             if (!ScopePassFaction(_scopeFilter, c.id)) continue;
00273:             var cells = new List<AshfallDataGrid.Cell>
00274:             {
00275:                 new(c.display, AshfallDataGrid.CellState.Normal),
00276:                 new(c.direction, c.direction.StartsWith("+") ? AshfallDataGrid.CellState.Selected :
00277:                                   c.direction.StartsWith("−") || c.direction.StartsWith("-") ? AshfallDataGrid.CellState.Critical :
00278:                                                                                           AshfallDataGrid.CellState.Muted),
00279:                 new($"{c.dTrust:+0;-0;0}", c.dTrust >= 0f ? AshfallDataGrid.CellState.Normal : AshfallDataGrid.CellState.Warning),
00280:                 new($"{c.anchorCap:0}", AshfallDataGrid.CellState.Muted),
00281:             };
00282:             rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
00283:         }
00284:         if (rows.Count == 0) rows.AddRange(BuildEmptyCotent());
00285:         return rows;
00286:     }
00287:
00288:     private List<AshfallDataGrid.Row> CoalitionRows()
00289:     {
00290:         var rows = new List<AshfallDataGrid.Row>();
00291:         for (int i = 0; i < _coalitionRows.Count; i++)
00292:         {
00293:             var c = _coalitionRows[i];
00294:             if (!ScopePassFaction(_scopeFilter, c.id)) continue;
00295:             int total = c.denizens + c.discontents + c.sentinels + c.raiders;
00296:             var cells = new List<AshfallDataGrid.Cell>
00297:             {
00298:                 new(c.display, AshfallDataGrid.CellState.Normal),
00299:                 new($"{c.denizens} ({TotalPct(c.denizens, total):0}%)",
00300:                     AshfallDataGrid.CellState.Muted),
00301:                 new($"{c.discontents} ({TotalPct(c.discontents, total):0}%)",
00302:                     c.discontents > c.sentinels ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Muted),
00303:                 new($"{c.sentinels} ({TotalPct(c.sentinels, total):0}%)",
00304:                     c.sentinels > 0 ? AshfallDataGrid.CellState.Selected : AshfallDataGrid.CellState.Muted),
00305:                 new($"{c.raiders} ({TotalPct(c.raiders, total):0}%)",
00306:                     c.raiders > 0 ? AshfallDataGrid.CellState.Critical : AshfallDataGrid.CellState.Muted),
00307:             };
00308:             rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
00309:         }
00310:         if (rows.Count == 0) rows.AddRange(BuildEmptyCotent());
00311:         return rows;
00312:     }
00313:
00314:     private static int TotalPct(int part, int whole) => whole <= 0 ? 0 : (int)System.Math.Round((part * 100.0) / whole);
00315:
00316:     private List<AshfallDataGrid.Row> DossierRows()
00317:     {
00318:         // Witness dossiers: author-level pieces of evidence pinned to subsectors
00319:         // and factions. Their weight influences the muster approach choice.
00320:         var rows = new List<AshfallDataGrid.Row>
00321:         {
00322:             new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
00323:             {
00324:                 new("dos_dispatch_03", AshfallDataGrid.CellState.Normal),
00325:                 new("Wasteland Dispatch", AshfallDataGrid.CellState.Muted),
00326:                 new("0.85", AshfallDataGrid.CellState.Normal),
00327:                 new("+12", AshfallDataGrid.CellState.Selected),
00328:                 new("faction_iron_garrison", AshfallDataGrid.CellState.Muted),
00329:             }, Selectable = false },
00330:             new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
00331:             {
00332:                 new("dos_witness_12", AshfallDataGrid.CellState.Normal),
00333:                 new("Internal Witness Log", AshfallDataGrid.CellState.Muted),
00334:                 new("0.65", AshfallDataGrid.CellState.Normal),
00335:                 new("+8", AshfallDataGrid.CellState.Selected),
00336:                 new("faction_the_office", AshfallDataGrid.CellState.Muted),
00337:             }, Selectable = false },
00338:             new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
00339:             {
00340:                 new("dos_brine_07", AshfallDataGrid.CellState.Normal),
00341:                 new("Brine Pipe Ledger", AshfallDataGrid.CellState.Muted),
00342:                 new("0.40", AshfallDataGrid.CellState.Caution),
00343:                 new("+3", AshfallDataGrid.CellState.Normal),
00344:                 new("faction_hydro_barons", AshfallDataGrid.CellState.Muted),
00345:             }, Selectable = false },
00346:             new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
00347:             {
00348:                 new("dos_foundry_22", AshfallDataGrid.CellState.Normal),
00349:                 new("Foundry Field Note", AshfallDataGrid.CellState.Muted),
00350:                 new("0.55", AshfallDataGrid.CellState.Normal),
00351:                 new("+6", AshfallDataGrid.CellState.Selected),
00352:                 new("faction_silent_foundry", AshfallDataGrid.CellState.Muted),
00353:             }, Selectable = false },
00354:         };
00355:         return rows;
00356:     }
00357:
00358:     private void BuildActionRows()
00359:     {
00360:         if (_actionBarGrid == null) return;
00361:         _actionBarGrid.SetRows(BuildActionFixtureRows());
00362:     }
00363:
00364:     private void RefreshDetail()
00365:     {
00366:         if (_detailBox == null) return;
00367:         AshfallUiHelpers.EmptyChildren(_detailBox);
00368:         _detailTitle = AshfallUiHelpers.MakeSectionHeader("SITE DETAIL");
00369:         _detailBox.AddChild(_detailTitle);
00370:         _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
00371:         if (_host == null)
00372:         {
00373:             _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
00374:                 "Standing Record engine offline. Bind a StandingRecordHostSession to see live ground layouts and strata.", autowrap: true));
00375:             return;
00376:         }
00377:         if (_selectedIndex < 0)
00378:         {
00379:             _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
00380:                 "Select a faction row to view approach, current direction, and coalition breakdown.", autowrap: true));
00381:             return;
00382:         }
00383:         var id = ResolveVisibleRow(_selectedIndex);
00384:         if (string.IsNullOrEmpty(id))
00385:         {
00386:             _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Selected row out of scope.", autowrap: true));
00387:             return;
00388:         }
00389:         var current = _currentRows.Find(r => r.id == id);
00390:         var coal = _coalitionRows.Find(r => r.id == id);
00391:         if (current.id == null)
00392:         {
00393:             _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Faction unknown.", autowrap: true));
00394:             return;
00395:         }
00396:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Faction", current.display,
00397:             AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00398:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Current", current.direction,
00399:             AshfallUiHelpers.ToColor(current.dTrust >= 0f ? DesignTheme.Lethe : DesignTheme.Entropy)));
00400:         _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Δ Trust", $"{current.dTrust:+0;-0;0}",
00401:             AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00402:         if (coal.id != null)
00403:         {
00404:             _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Camp Strength",
00405:                 $"D{coal.denizens} C{coal.discontents} S{coal.sentinels} R{coal.raiders}",
00406:                 AshfallUiHelpers.ToColor(DesignTheme.Muted)));
00407:         }
00408:     }
00409:
00410:     private string ResolveVisibleRow(int visibleIndex)
00411:     {
00412:         if (_currentRows.Count == 0) return string.Empty;
00413:         int seen = -1;
00414:         for (int i = 0; i < _currentRows.Count; i++)
00415:         {
00416:             if (!ScopePassFaction(_scopeFilter, _currentRows[i].id)) continue;
00417:             seen++;
00418:             if (seen == visibleIndex) return _currentRows[i].id;
00419:         }
00420:         return string.Empty;
00421:     }
00422:
00423:     private static bool ScopePassFaction(string scope, string factionId)
00424:     {
00425:         if (scope == "all") return true;
00426:         if (scope == "rooms" || scope == "recasts") return true; // currents/dossiers always visible across factions
00427:         if (scope == "coalition") return true;
00428:         if (scope == "loyalist") return factionId == "faction_the_office" || factionId == "faction_iron_garrison" || factionId == "faction_silent_foundry";
00429:         if (scope == "deserter") return factionId == "faction_hydro_barons" || factionId == "faction_warlord";
00430:         return true;
00431:     }
00432:
00433:     private (List<(string id, string display, string direction, float dTrust, float anchorCap)> currents,
00434:             List<(string id, string display, int denizens, int discontents, int sentinels, int raiders)> coalition)
00435:         BuildData()
00436:     {
00437:         var currents = new List<(string, string, string, float, float)>
00438:         {
00439:             ("faction_the_office",         "The Office",          "+",   12f,  80f),
00440:             ("faction_iron_garrison",      "Iron Garrison",       "+",    5f,  72f),
00441:             ("faction_silent_foundry",     "The Silent Foundry",  "+",   18f,  91f),
00442:             ("faction_hydro_barons",       "Hydro Barons",        "−",   -8f,  60f),
00443:             ("faction_warlord",            "Warlord Sectors",     "−",  -23f,  35f),
00444:         };
00445:         var coalition = new List<(string, string, int, int, int, int)>
00446:         {
00447:             ("faction_the_office",         "The Office",          32,  11,  18,  0),
00448:             ("faction_iron_garrison",      "Iron Garrison",       41,  19,  29,  6),
00449:             ("faction_silent_foundry",     "The Silent Foundry",  17,   5,  22,  0),
00450:             ("faction_hydro_barons",       "Hydro Barons",        22,  18,   9,  4),
00451:             ("faction_warlord",            "Warlord Sectors",     15,   8,   3, 12),
00452:         };
00453:         return (currents, coalition);
00454:     }
00455:
00456:     /// <summary>Hard-coded fixture rows for the bound=false case.</summary>
00457:     internal static List<AshfallDataGrid.Row> BuildActionFixtureRows()
00458:     {
00459:         return new List<AshfallDataGrid.Row>
00460:         {
00461:             new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
00462:             {
00463:                 new("Arrive At Parent",        AshfallDataGrid.CellState.Normal),
00464:                 new("Roll the desertion threshold · select faction card", AshfallDataGrid.CellState.Muted),
00465:             }, Selectable = false },
00466:             new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
00467:             {
00468:                 new("Inspect Room",    AshfallDataGrid.CellState.Normal),
00469:                 new("Tick + Radiograph adjacency; reveals inspectKey.",  AshfallDataGrid.CellState.Muted),
00470:             }, Selectable = false },
00471:             new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
00472:             {
00473:                 new("Apply Mutation",      AshfallDataGrid.CellState.Normal),
00474:                 new("Stratum swap for the active site id.",                   AshfallDataGrid.CellState.Muted),
00475:             }, Selectable = false },
00476:             new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
00477:             {
00478:                 new("Lock Overlay",       AshfallDataGrid.CellState.Normal),
00479:                 new("Three plate scrapes → labour withdraws.",             AshfallDataGrid.CellState.Muted),
00480:             }, Selectable = false },
00481:         };
00482:     }
00483:
00484:     public void Open()
00485:     {
00486:         Visible = true;
00487:         RefreshView();
00488:         QueueRedraw();
00489:     }
00490:
00491:     public override void _UnhandledInput(InputEvent @event)
00492:     {
00493:         if (!Visible) return;
00494:         if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00495:         {
00496:             OnClose?.Invoke();
00497:             GetViewport().SetInputAsHandled();
00498:         }
00499:     }
00500:
00501:
00502:     public void Unbind()
00503:     {
00504:         if (_host != null)
00505:         {
00506:             _host.StateChanged -= RefreshView;
00507:         }
00508:     }
00509:
00510:     public override void _ExitTree()
00511:         {
00512:             Unbind();
00513:             base._ExitTree();
00514:         }
00515: }
```

## `Ashfall.Core.Tests/StandingRecordFactionExpansionTests.cs` — 362 lines; 13,646 bytes; SHA-256 `6367968ea17a60b7c95fc64668cb1924ee2685875041ba680b4be9f1a1d3268d`
Declaration index:
- 00011: public sealed class StandingRecordFactionExpansionTests
- 00013: private static string ResolveDataDir()
- 00023: private sealed class StandingRecordFactionEntryDto
- 00038: private static List<StandingRecordFactionEntryDto> LoadFactions()
- 00050: public void Catalog_LoadsSuccessfully_ContainsExactEightFactions()
- 00057: public void BaselineOverlay_PreservedVerbatim()
- 00075: public void ExpectedEightFactions_AllPresent()
- 00098: public void FactionIds_AreUnique_AndStartWithFactionPrefix()
- 00111: public void DisplayNames_AreUnique_AndNonEmpty()
- 00123: public void Alignments_AreValid()
- 00134: public void HomeRegions_AreValid()
- 00155: public void Wants_And_Offers_ArePopulated_AndNonEmpty()
- 00179: public void TradeProfiles_AreDifferentiated()
- 00206: public void SignatureQuotes_AreAuthored_AndDistinct()
- 00218: public void AccessRules_AreAuthored_AndDistinct()
- 00230: public void ActiveStatus_And_StartingTrust_AreValid()
- 00241: public void NegativeFixture_DuplicateId_IsDetected()
- 00264: public void NegativeFixture_InvalidAlignment_IsRejected()
- 00278: public void NegativeFixture_InvalidRegion_IsRejected()
- 00301: public void NegativeFixture_TrustOutOfRange_IsRejected()
- 00313: public void Persistence_OldSaveInitialization_DefaultsGracefully()
- 00315: // Simulate an older campaign save where no standing record faction reputation is serialized
- 00336: public void Persistence_MutableTrustRoundTrip_PreservesDynamicStanding()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Xunit;
00007: using Ashfall.Core;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     public sealed class StandingRecordFactionExpansionTests
00012:     {
00013:         private static string ResolveDataDir()
00014:         {
00015:             string start = Directory.GetCurrentDirectory();
00016:             if (CatalogLocator.TryFindDataDirectory(start, out string found))
00017:                 return found;
00018:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
00019:                 return found;
00020:             throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
00021:         }
00022:
00023:         private sealed class StandingRecordFactionEntryDto
00024:         {
00025:             public string id { get; set; } = string.Empty;
00026:             public string display_name { get; set; } = string.Empty;
00027:             public string alignment { get; set; } = string.Empty;
00028:             public string home_region { get; set; } = string.Empty;
00029:             public bool is_active { get; set; } = true;
00030:             public int trust { get; set; } = 0;
00031:             public string[] wants { get; set; } = Array.Empty<string>();
00032:             public string[] offers { get; set; } = Array.Empty<string>();
00033:             public string signature_quote { get; set; } = string.Empty;
00034:             public string access_rule { get; set; } = string.Empty;
00035:             public string badge_asset_id { get; set; } = string.Empty;
00036:         }
00037:
00038:         private static List<StandingRecordFactionEntryDto> LoadFactions()
00039:         {
00040:             string dataDir = ResolveDataDir();
00041:             string path = Path.Combine(dataDir, "standing_record_factions.json");
00042:             Assert.True(File.Exists(path), "standing_record_factions.json must exist at " + path);
00043:             string json = File.ReadAllText(path);
00044:             var items = CatalogLocator.LoadWrappedList<StandingRecordFactionEntryDto>(json, SystemTextJsonSerializer.Options);
00045:             Assert.NotNull(items);
00046:             return items;
00047:         }
00048:
00049:         [Fact]
00050:         public void Catalog_LoadsSuccessfully_ContainsExactEightFactions()
00051:         {
00052:             var factions = LoadFactions();
00053:             Assert.Equal(8, factions.Count);
00054:         }
00055:
00056:         [Fact]
00057:         public void BaselineOverlay_PreservedVerbatim()
00058:         {
00059:             var factions = LoadFactions();
00060:             var overlay = factions.FirstOrDefault(f => f.id == "faction_the_overlay");
00061:             Assert.NotNull(overlay);
00062:             Assert.Equal("The Overlay", overlay.display_name);
00063:             Assert.Equal("conditional", overlay.alignment);
00064:             Assert.Equal("all_regions", overlay.home_region);
00065:             Assert.True(overlay.is_active);
00066:             Assert.Equal(0, overlay.trust);
00067:             Assert.Equal(new[] { "brass_fittings", "sr_stencil_pot", "lamp_oil" }, overlay.wants);
00068:             Assert.Equal(new[] { "cadastral_keys", "travel_correction_on_named_sites" }, overlay.offers);
00069:             Assert.Equal("The Schedule named households. The Record names ground. Ground does not argue.", overlay.signature_quote);
00070:             Assert.Equal("Scrape three plates without writing a lived name or a Continuity number, and Overlay labour withdraws. They do not raid. Rooms go dark of juniors. Posts stay posts.", overlay.access_rule);
00071:             Assert.Equal(string.Empty, overlay.badge_asset_id);
00072:         }
00073:
00074:         [Fact]
00075:         public void ExpectedEightFactions_AllPresent()
00076:         {
00077:             var factions = LoadFactions();
00078:             var ids = factions.Select(f => f.id).ToHashSet();
00079:             var expected = new[]
00080:             {
00081:                 "faction_the_overlay",
00082:                 "faction_the_scale",
00083:                 "faction_the_compact",
00084:                 "faction_the_underwrite",
00085:                 "faction_the_cutters",
00086:                 "faction_the_fleet",
00087:                 "faction_the_rebuilders",
00088:                 "faction_the_garrison"
00089:             };
00090:
00091:             foreach (string expectedId in expected)
00092:             {
00093:                 Assert.Contains(expectedId, ids);
00094:             }
00095:         }
00096:
00097:         [Fact]
00098:         public void FactionIds_AreUnique_AndStartWithFactionPrefix()
00099:         {
00100:             var factions = LoadFactions();
00101:             var seen = new HashSet<string>();
00102:             foreach (var faction in factions)
00103:             {
00104:                 Assert.False(string.IsNullOrWhiteSpace(faction.id));
00105:                 Assert.StartsWith("faction_the_", faction.id);
00106:                 Assert.True(seen.Add(faction.id), "Duplicate faction ID detected: " + faction.id);
00107:             }
00108:         }
00109:
00110:         [Fact]
00111:         public void DisplayNames_AreUnique_AndNonEmpty()
00112:         {
00113:             var factions = LoadFactions();
00114:             var seen = new HashSet<string>();
00115:             foreach (var faction in factions)
00116:             {
00117:                 Assert.False(string.IsNullOrWhiteSpace(faction.display_name));
00118:                 Assert.True(seen.Add(faction.display_name), "Duplicate display name detected: " + faction.display_name);
00119:             }
00120:         }
00121:
00122:         [Fact]
00123:         public void Alignments_AreValid()
00124:         {
00125:             var factions = LoadFactions();
00126:             var validAlignments = new HashSet<string> { "conditional", "neutral", "peaceful", "allied", "hostile" };
00127:             foreach (var faction in factions)
00128:             {
00129:                 Assert.Contains(faction.alignment, validAlignments);
00130:             }
00131:         }
00132:
00133:         [Fact]
00134:         public void HomeRegions_AreValid()
00135:         {
00136:             var factions = LoadFactions();
00137:             var validRegions = new HashSet<string>
00138:             {
00139:                 "all_regions",
00140:                 "industrial_belt",
00141:                 "dead_suburbs",
00142:                 "the_cut",
00143:                 "deep_coast",
00144:                 "ash_flats"
00145:             };
00146:
00147:             foreach (var faction in factions)
00148:             {
00149:                 Assert.False(string.IsNullOrWhiteSpace(faction.home_region));
00150:                 Assert.Contains(faction.home_region, validRegions);
00151:             }
00152:         }
00153:
00154:         [Fact]
00155:         public void Wants_And_Offers_ArePopulated_AndNonEmpty()
00156:         {
00157:             var factions = LoadFactions();
00158:             foreach (var faction in factions)
00159:             {
00160:                 Assert.NotNull(faction.wants);
00161:                 Assert.NotEmpty(faction.wants);
00162:                 Assert.True(faction.wants.Length >= 2, $"Faction {faction.id} should have at least 2 wants");
00163:                 foreach (var want in faction.wants)
00164:                 {
00165:                     Assert.False(string.IsNullOrWhiteSpace(want), $"Empty want token in faction {faction.id}");
00166:                 }
00167:
00168:                 Assert.NotNull(faction.offers);
00169:                 Assert.NotEmpty(faction.offers);
00170:                 Assert.True(faction.offers.Length >= 2, $"Faction {faction.id} should have at least 2 offers");
00171:                 foreach (var offer in faction.offers)
00172:                 {
00173:                     Assert.False(string.IsNullOrWhiteSpace(offer), $"Empty offer token in faction {faction.id}");
00174:                 }
00175:             }
00176:         }
00177:
00178:         [Fact]
00179:         public void TradeProfiles_AreDifferentiated()
00180:         {
00181:             var factions = LoadFactions();
00182:             var wantSets = new List<HashSet<string>>();
00183:             var offerSets = new List<HashSet<string>>();
00184:
00185:             foreach (var faction in factions)
00186:             {
00187:                 var wants = new HashSet<string>(faction.wants);
00188:                 var offers = new HashSet<string>(faction.offers);
00189:
00190:                 foreach (var existingWants in wantSets)
00191:                 {
00192:                     Assert.False(existingWants.SetEquals(wants), $"Faction {faction.id} shares an identical wants profile with another faction.");
00193:                 }
00194:
00195:                 foreach (var existingOffers in offerSets)
00196:                 {
00197:                     Assert.False(existingOffers.SetEquals(offers), $"Faction {faction.id} shares an identical offers profile with another faction.");
00198:                 }
00199:
00200:                 wantSets.Add(wants);
00201:                 offerSets.Add(offers);
00202:             }
00203:         }
00204:
00205:         [Fact]
00206:         public void SignatureQuotes_AreAuthored_AndDistinct()
00207:         {
00208:             var factions = LoadFactions();
00209:             var seen = new HashSet<string>();
00210:             foreach (var faction in factions)
00211:             {
00212:                 Assert.False(string.IsNullOrWhiteSpace(faction.signature_quote));
00213:                 Assert.True(seen.Add(faction.signature_quote), "Duplicate signature quote in " + faction.id);
00214:             }
00215:         }
00216:
00217:         [Fact]
00218:         public void AccessRules_AreAuthored_AndDistinct()
00219:         {
00220:             var factions = LoadFactions();
00221:             var seen = new HashSet<string>();
00222:             foreach (var faction in factions)
00223:             {
00224:                 Assert.False(string.IsNullOrWhiteSpace(faction.access_rule));
00225:                 Assert.True(seen.Add(faction.access_rule), "Duplicate access rule in " + faction.id);
00226:             }
00227:         }
00228:
00229:         [Fact]
00230:         public void ActiveStatus_And_StartingTrust_AreValid()
00231:         {
00232:             var factions = LoadFactions();
00233:             foreach (var faction in factions)
00234:             {
00235:                 Assert.True(faction.is_active, $"Faction {faction.id} should be active");
00236:                 Assert.InRange(faction.trust, -50, 50);
00237:             }
00238:         }
00239:
00240:         [Fact]
00241:         public void NegativeFixture_DuplicateId_IsDetected()
00242:         {
00243:             var fixture = new List<StandingRecordFactionEntryDto>
00244:             {
00245:                 new StandingRecordFactionEntryDto { id = "faction_the_overlay", display_name = "Overlay A" },
00246:                 new StandingRecordFactionEntryDto { id = "faction_the_overlay", display_name = "Overlay B" }
00247:             };
00248:
00249:             var seen = new HashSet<string>();
00250:             bool duplicateDetected = false;
00251:             foreach (var item in fixture)
00252:             {
00253:                 if (!seen.Add(item.id))
00254:                 {
00255:                     duplicateDetected = true;
00256:                     break;
00257:                 }
00258:             }
00259:
00260:             Assert.True(duplicateDetected, "Validator must flag duplicate faction IDs.");
00261:         }
00262:
00263:         [Fact]
00264:         public void NegativeFixture_InvalidAlignment_IsRejected()
00265:         {
00266:             var invalid = new StandingRecordFactionEntryDto
00267:             {
00268:                 id = "faction_the_test",
00269:                 display_name = "Test Faction",
00270:                 alignment = "chaotic_evil"
00271:             };
00272:
00273:             var validAlignments = new HashSet<string> { "conditional", "neutral", "peaceful", "allied", "hostile" };
00274:             Assert.DoesNotContain(invalid.alignment, validAlignments);
00275:         }
00276:
00277:         [Fact]
00278:         public void NegativeFixture_InvalidRegion_IsRejected()
00279:         {
00280:             var invalid = new StandingRecordFactionEntryDto
00281:             {
00282:                 id = "faction_the_test",
00283:                 display_name = "Test Faction",
00284:                 home_region = "space_station_orbit"
00285:             };
00286:
00287:             var validRegions = new HashSet<string>
00288:             {
00289:                 "all_regions",
00290:                 "industrial_belt",
00291:                 "dead_suburbs",
00292:                 "the_cut",
00293:                 "deep_coast",
00294:                 "ash_flats"
00295:             };
00296:
00297:             Assert.DoesNotContain(invalid.home_region, validRegions);
00298:         }
00299:
00300:         [Fact]
00301:         public void NegativeFixture_TrustOutOfRange_IsRejected()
00302:         {
00303:             var invalid = new StandingRecordFactionEntryDto
00304:             {
00305:                 id = "faction_the_test",
00306:                 trust = 999
00307:             };
00308:
00309:             Assert.False(invalid.trust >= -50 && invalid.trust <= 50, "Trust score 999 must be recognized as out of bounds.");
00310:         }
00311:
00312:         [Fact]
00313:         public void Persistence_OldSaveInitialization_DefaultsGracefully()
00314:         {
00315:             // Simulate an older campaign save where no standing record faction reputation is serialized
00316:             var oldSaveReputation = new Dictionary<string, int>();
00317:
00318:             var catalog = LoadFactions();
00319:             // Initialize missing factions from catalog defaults
00320:             foreach (var f in catalog)
00321:             {
00322:                 if (!oldSaveReputation.ContainsKey(f.id))
00323:                 {
00324:                     oldSaveReputation[f.id] = f.trust;
00325:                 }
00326:             }
00327:
00328:             Assert.Equal(8, oldSaveReputation.Count);
00329:             foreach (var f in catalog)
00330:             {
00331:                 Assert.Equal(0, oldSaveReputation[f.id]);
00332:             }
00333:         }
00334:
00335:         [Fact]
00336:         public void Persistence_MutableTrustRoundTrip_PreservesDynamicStanding()
00337:         {
00338:             var liveStanding = new Dictionary<string, int>();
00339:             var catalog = LoadFactions();
00340:             foreach (var f in catalog)
00341:             {
00342:                 liveStanding[f.id] = f.trust;
00343:             }
00344:
00345:             // Mutate live standing for The Scale
00346:             liveStanding["faction_the_scale"] += 25;
00347:
00348:             // Serialize simulated campaign state
00349:             string json = System.Text.Json.JsonSerializer.Serialize(liveStanding);
00350:
00351:             // Restore from save
00352:             var restoredStanding = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(json);
00353:             Assert.NotNull(restoredStanding);
00354:             Assert.Equal(25, restoredStanding["faction_the_scale"]);
00355:
00356:             // Re-verifying with fresh catalog load confirms catalog defaults were not overwritten
00357:             var freshCatalog = LoadFactions();
00358:             var scaleCatalog = freshCatalog.First(f => f.id == "faction_the_scale");
00359:             Assert.Equal(0, scaleCatalog.trust);
00360:         }
00361:     }
00362: }
```

## `Ashfall.Core.Tests/StandingRecordSystemTests.cs` — 264 lines; 11,037 bytes; SHA-256 `c575b772fba23cf0c4b7a6a5214587d8f399c073da8ed841c60b86b2146a81d1`
Declaration index:
- 00009: public class LocationMemorySystemTests
- 00011: private static string DataDir()
- 00021: private static LocationMemorySystem Memory()
- 00030: public void StrataLoadFromJson()
- 00039: public void NowStrataSelectedByMutation()
- 00051: public void ScrapedOverridesPlated()
- 00061: public void PalimpsestSelectedLast()
- 00071: public void SaveRoundtrip()
- 00084: public void RecastEventFiresOncePerSite()
- 00098: public class SiteEncounterSystemTests
- 00101: public void LockedUntilUnlock()
- 00109: public void StartAndResolve()
- 00121: public void ThreeScrapesWithdrawOverlay()
- 00134: public void RestoreOverlayAccessReopens()
- 00145: public void SaveRoundtrip()
- 00163: public class StandingRecordCatalogTests
- 00165: private static string DataDir()
- 00176: public void TenMainQuestsRegistered()
- 00195: public void MainsChainInOrder()
- 00216: public void MainsTargetSpineLocations()
- 00229: public void EveryMainHasWorldChangeMutation()
- 00244: public void RosterNpcsPresentInCharacters()
- 00259: private sealed class StandingCharProbe
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using Xunit;
00005: using Ashfall.Core;
00006:
00007: namespace Ashfall.Core.Tests
00008: {
00009:     public class LocationMemorySystemTests
00010:     {
00011:         private static string DataDir()
00012:         {
00013:             string start = Directory.GetCurrentDirectory();
00014:             if (CatalogLocator.TryFindDataDirectory(start, out string found))
00015:                 return found;
00016:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
00017:                 return found;
00018:             throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
00019:         }
00020:
00021:         private static LocationMemorySystem Memory()
00022:         {
00023:             var sys = new LocationMemorySystem(new FileSystemIO(), new SystemTextJsonSerializer());
00024:             sys.Load(DataDir());
00025:             sys.Unlock();
00026:             return sys;
00027:         }
00028:
00029:         [Fact]
00030:         public void StrataLoadFromJson()
00031:         {
00032:             var mem = Memory();
00033:             Assert.True(mem.StratumCount >= 30);
00034:             Assert.NotNull(mem.GetStratumText("loc_cut_kilometre_19", "pre"));
00035:             Assert.NotNull(mem.GetStratumText("loc_cut_kilometre_19", "now"));
00036:         }
00037:
00038:         [Fact]
00039:         public void NowStrataSelectedByMutation()
00040:         {
00041:             var mem = Memory();
00042:             Assert.Null(mem.GetActiveRecast("loc_cut_kilometre_19"));
00043:             mem.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
00044:             string active = mem.GetActiveRecast("loc_cut_kilometre_19");
00045:             Assert.NotNull(active);
00046:             Assert.Contains("CUT-19", active);
00047:             Assert.Contains("Ivy", active);
00048:         }
00049:
00050:         [Fact]
00051:         public void ScrapedOverridesPlated()
00052:         {
00053:             var mem = Memory();
00054:             mem.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
00055:             mem.ApplyMutation(LocationMemorySystem.MutationKm19Scraped);
00056:             string active = mem.GetActiveRecast("loc_cut_kilometre_19");
00057:             Assert.Contains("short one post", active);
00058:         }
00059:
00060:         [Fact]
00061:         public void PalimpsestSelectedLast()
00062:         {
00063:             var mem = Memory();
00064:             mem.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
00065:             mem.ApplyMutation(LocationMemorySystem.MutationKm19Palimpsest);
00066:             string active = mem.GetActiveRecast("loc_cut_kilometre_19");
00067:             Assert.Contains("two things", active);
00068:         }
00069:
00070:         [Fact]
00071:         public void SaveRoundtrip()
00072:         {
00073:             var json = new SystemTextJsonSerializer();
00074:             var mem = Memory();
00075:             mem.ApplyMutation(LocationMemorySystem.MutationLockGaugesFiled);
00076:             var restored = new LocationMemorySystem(new FileSystemIO(), new SystemTextJsonSerializer());
00077:             restored.Load(DataDir());
00078:             restored.Unlock();
00079:             restored.RestoreState(json.Deserialize<LocationMemoryState>(json.Serialize(mem.CaptureState())));
00080:             Assert.True(restored.HasMutation(LocationMemorySystem.MutationLockGaugesFiled));
00081:         }
00082:
00083:         [Fact]
00084:         public void RecastEventFiresOncePerSite()
00085:         {
00086:             var mem = Memory();
00087:             int fired = 0;
00088:             mem.OnLocationRecast += (site, text) => fired++;
00089:             mem.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
00090:             Assert.Equal(1, fired);
00091:             mem.ApplyMutation(LocationMemorySystem.MutationTransitMaps);
00092:             Assert.Equal(2, fired);
00093:             mem.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
00094:             Assert.Equal(2, fired); // same-site recast does not re-fire
00095:         }
00096:     }
00097:
00098:     public class SiteEncounterSystemTests
00099:     {
00100:         [Fact]
00101:         public void LockedUntilUnlock()
00102:         {
00103:             var sys = new SiteEncounterSystem();
00104:             Assert.False(sys.StartEncounter("enc_site_plate_screwer", "room_km19_post",
00105:                 SiteEncounterSystem.KindPlateScrewer, 75));
00106:         }
00107:
00108:         [Fact]
00109:         public void StartAndResolve()
00110:         {
00111:             var sys = new SiteEncounterSystem(1808);
00112:             sys.Unlock();
00113:             Assert.True(sys.StartEncounter("enc_site_plate_screwer", "room_km19_post",
00114:                 SiteEncounterSystem.KindPlateScrewer, 75, "mutation_km19_plated"));
00115:             Assert.True(sys.ResolveEncounter("enc_site_plate_screwer", 75));
00116:             Assert.False(sys.ResolveEncounter("enc_site_plate_screwer", 76));
00117:             Assert.True(sys.IsResolved("enc_site_plate_screwer"));
00118:         }
00119:
00120:         [Fact]
00121:         public void ThreeScrapesWithdrawOverlay()
00122:         {
00123:             var sys = new SiteEncounterSystem();
00124:             sys.Unlock();
00125:             Assert.True(sys.OverlayAccess);
00126:             sys.ScrapePlate(75);
00127:             sys.ScrapePlate(76);
00128:             sys.ScrapePlate(77);
00129:             Assert.False(sys.OverlayAccess);
00130:             Assert.Equal(3, sys.PlatesScraped);
00131:         }
00132:
00133:         [Fact]
00134:         public void RestoreOverlayAccessReopens()
00135:         {
00136:             var sys = new SiteEncounterSystem();
00137:             sys.Unlock();
00138:             for (int i = 0; i < 3; i++) sys.ScrapePlate(80 + i);
00139:             Assert.False(sys.OverlayAccess);
00140:             sys.RestoreOverlayAccess();
00141:             Assert.True(sys.OverlayAccess);
00142:         }
00143:
00144:         [Fact]
00145:         public void SaveRoundtrip()
00146:         {
00147:             var json = new SystemTextJsonSerializer();
00148:             var sys = new SiteEncounterSystem(1808);
00149:             sys.Unlock();
00150:             sys.StartEncounter("enc_site_gauge_read", "room_lock_gauges",
00151:                 SiteEncounterSystem.KindGaugeRead, 90, "mutation_lock_gauges_filed");
00152:             sys.ResolveEncounter("enc_site_gauge_read", 90);
00153:             sys.ScrapePlate(91);
00154:             string blob = json.Serialize(sys.CaptureState());
00155:             var restored = new SiteEncounterSystem();
00156:             restored.RestoreState(json.Deserialize<SiteEncounterState>(blob));
00157:             Assert.True(restored.IsResolved("enc_site_gauge_read"));
00158:             Assert.Equal(1, restored.PlatesScraped);
00159:             Assert.True(restored.OverlayAccess);
00160:         }
00161:     }
00162:
00163:     public class StandingRecordCatalogTests
00164:     {
00165:         private static string DataDir()
00166:         {
00167:             string start = Directory.GetCurrentDirectory();
00168:             if (CatalogLocator.TryFindDataDirectory(start, out string found))
00169:                 return found;
00170:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
00171:                 return found;
00172:             throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
00173:         }
00174:
00175:         [Fact]
00176:         public void TenMainQuestsRegistered()
00177:         {
00178:             var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
00179:             var catalog = loader.Load(DataDir());
00180:             Assert.True(catalog.Quests.Count >= 10);
00181:             // All ten mains (plan §4.1)
00182:             Assert.NotNull(catalog.GetQuest("quest_record_the_plate"));
00183:             Assert.NotNull(catalog.GetQuest("quest_record_grease_pencil"));
00184:             Assert.NotNull(catalog.GetQuest("quest_record_wrong_stacks"));
00185:             Assert.NotNull(catalog.GetQuest("quest_record_the_book"));
00186:             Assert.NotNull(catalog.GetQuest("quest_record_mass_or_lot"));
00187:             Assert.NotNull(catalog.GetQuest("quest_record_hands"));
00188:             Assert.NotNull(catalog.GetQuest("quest_record_friendly_obstacle"));
00189:             Assert.NotNull(catalog.GetQuest("quest_record_the_failure"));
00190:             Assert.NotNull(catalog.GetQuest("quest_record_fallback"));
00191:             Assert.NotNull(catalog.GetQuest("quest_record_which_gazetteer"));
00192:         }
00193:
00194:         [Fact]
00195:         public void MainsChainInOrder()
00196:         {
00197:             var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
00198:             var catalog = loader.Load(DataDir());
00199:             Assert.Equal("quest_record_the_plate",
00200:                 catalog.GetQuest("quest_record_grease_pencil").prereq_quest_id);
00201:             Assert.Equal("quest_record_grease_pencil",
00202:                 catalog.GetQuest("quest_record_wrong_stacks").prereq_quest_id);
00203:             Assert.Equal("quest_record_wrong_stacks",
00204:                 catalog.GetQuest("quest_record_the_book").prereq_quest_id);
00205:             Assert.Equal("quest_record_the_book",
00206:                 catalog.GetQuest("quest_record_mass_or_lot").prereq_quest_id);
00207:             Assert.Equal("quest_record_mass_or_lot",
00208:                 catalog.GetQuest("quest_record_hands").prereq_quest_id);
00209:             Assert.Equal("quest_record_the_failure",
00210:                 catalog.GetQuest("quest_record_fallback").prereq_quest_id);
00211:             Assert.Equal("quest_record_fallback",
00212:                 catalog.GetQuest("quest_record_which_gazetteer").prereq_quest_id);
00213:         }
00214:
00215:         [Fact]
00216:         public void MainsTargetSpineLocations()
00217:         {
00218:             var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
00219:             var catalog = loader.Load(DataDir());
00220:             Assert.Equal("location_ministry_of_truth_bunker",
00221:                 catalog.GetQuest("quest_record_the_book").target_location_id);
00222:             Assert.Equal("loc_lock_gate_four",
00223:                 catalog.GetQuest("quest_record_the_failure").target_location_id);
00224:             Assert.Equal("location_the_memory_vault",
00225:                 catalog.GetQuest("quest_record_which_gazetteer").target_location_id);
00226:         }
00227:
00228:         [Fact]
00229:         public void EveryMainHasWorldChangeMutation()
00230:         {
00231:             var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
00232:             var catalog = loader.Load(DataDir());
00233:             for (int i = 0; i < catalog.Quests.Count; i++)
00234:             {
00235:                 var q = catalog.Quests[i];
00236:                 Assert.False(string.IsNullOrEmpty(q.id));
00237:                 Assert.False(string.IsNullOrEmpty(q.complete_mutation),
00238:                     q.id + " must name a world-change mutation (plan world-change bar)");
00239:                 Assert.True(q.StageCount >= 3, q.id + " should have 3+ objectives (spatial bar)");
00240:             }
00241:         }
00242:
00243:         [Fact]
00244:         public void RosterNpcsPresentInCharacters()
00245:         {
00246:             string path = Path.Combine(DataDir(), "characters.json");
00247:             var chars = CatalogLocator.LoadWrappedList<StandingCharProbe>(
00248:                 File.ReadAllText(path), SystemTextJsonSerializer.Options);
00249:             var ids = new System.Collections.Generic.HashSet<string>();
00250:             for (int i = 0; i < chars.Count; i++) ids.Add(chars[i].id);
00251:             Assert.Contains("npc_maren_holt", ids);
00252:             Assert.Contains("npc_ira_vell", ids);
00253:             Assert.Contains("npc_benno_kade", ids);
00254:             Assert.Contains("npc_quil_esser", ids);
00255:             Assert.Contains("npc_osric_tann", ids);
00256:             Assert.Contains("npc_dara_mewn", ids);
00257:         }
00258:
00259:         private sealed class StandingCharProbe
00260:         {
00261:             public string id = string.Empty;
00262:         }
00263:     }
00264: }
```

## `Ashfall.Core.Tests/Governance/Plan89_98MusterFactionIntegrationTests.cs` — 226 lines; 10,108 bytes; SHA-256 `9031b5f8a79ba49adf31f295a19a78a9ea01cef996d40034587be14f767e6533`
Declaration index:
- 00018: /// standing record faction catalog loader binding, and cross-system alignment
- 00021: public sealed class Plan89_98MusterFactionIntegrationTests
- 00023: private static string ResolveDataDir()
- 00044: public void Plan89_MusterEpiloguesCatalog_LoadsAll25Outcomes_WithValidProseAndTitles()
- 00073: public void Plan98_StandingRecordCatalogLoader_LoadsAll8Factions_WithCompleteDossiers()
- 00122: public void CrossSystem_FactionAuthority_MapsDeterministicallyToEpilogueOutcomes()
- 00189: public void CrossSystem_EpilogueResolution_IsDeterministicAndStrictlyPrioritized()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Muster;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests.Governance
00011: {
00012:     /// <summary>
00013:     /// Cross-system integration tests for Wave 40 Batch 3:
00014:     /// - Plan 89 (DEC-259): Muster Epilogues Expansion (12 -> 25 campaign-ending epilogues)
00015:     /// - Plan 98 (DEC-260): Standing Record Factions Expansion (1 -> 8 factions)
00016:     ///
00017:     /// Validates referential integrity, deterministic ending matrix evaluation,
00018:     /// standing record faction catalog loader binding, and cross-system alignment
00019:     /// between faction territorial/economic authority and terminal campaign outcomes.
00020:     /// </summary>
00021:     public sealed class Plan89_98MusterFactionIntegrationTests
00022:     {
00023:         private static string ResolveDataDir()
00024:         {
00025:             string candidate = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
00026:             if (Directory.Exists(candidate)) return candidate;
00027:
00028:             var dir = new DirectoryInfo(AppContext.BaseDirectory);
00029:             while (dir != null)
00030:             {
00031:                 string check = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
00032:                 if (Directory.Exists(check)) return check;
00033:                 dir = dir.Parent;
00034:             }
00035:
00036:             string current = Directory.GetCurrentDirectory();
00037:             if (CatalogLocator.TryFindDataDirectory(current, out string found))
00038:                 return found;
00039:
00040:             throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data directory.");
00041:         }
00042:
00043:         [Fact]
00044:         public void Plan89_MusterEpiloguesCatalog_LoadsAll25Outcomes_WithValidProseAndTitles()
00045:         {
00046:             string dataDir = ResolveDataDir();
00047:             var io = new FileSystemIO();
00048:             var json = new SystemTextJsonSerializer();
00049:
00050:             List<EndingDefinition> epilogues = EpilogueMatrixLoader.LoadEpilogues(dataDir, io, json);
00051:
00052:             Assert.NotNull(epilogues);
00053:             Assert.Equal(25, epilogues.Count);
00054:
00055:             var seenKeys = new HashSet<string>(StringComparer.Ordinal);
00056:             foreach (var epilogue in epilogues)
00057:             {
00058:                 Assert.False(string.IsNullOrWhiteSpace(epilogue.endingKey), "Ending key must not be empty.");
00059:                 Assert.False(string.IsNullOrWhiteSpace(epilogue.title), $"Title for {epilogue.endingKey} must not be empty.");
00060:                 Assert.False(string.IsNullOrWhiteSpace(epilogue.prose), $"Prose for {epilogue.endingKey} must not be empty.");
00061:                 Assert.True(epilogue.prose.Length >= 20, $"Prose for {epilogue.endingKey} must be substantial narrative text.");
00062:                 Assert.True(seenKeys.Add(epilogue.endingKey), $"Duplicate ending key: {epilogue.endingKey}");
00063:             }
00064:
00065:             // Verify all keys declared in EpilogueMatrix.AllKeys are present in the catalog
00066:             foreach (string expectedKey in EpilogueMatrix.AllKeys)
00067:             {
00068:                 Assert.Contains(expectedKey, seenKeys);
00069:             }
00070:         }
00071:
00072:         [Fact]
00073:         public void Plan98_StandingRecordCatalogLoader_LoadsAll8Factions_WithCompleteDossiers()
00074:         {
00075:             string dataDir = ResolveDataDir();
00076:             var io = new FileSystemIO();
00077:             var json = new SystemTextJsonSerializer();
00078:             var loader = new StandingRecordCatalogLoader(io, json);
00079:
00080:             StandingRecordCatalog catalog = loader.Load(dataDir);
00081:
00082:             Assert.NotNull(catalog);
00083:             Assert.Equal(8, catalog.Factions.Count);
00084:
00085:             string[] expectedFactions =
00086:             {
00087:                 "faction_the_overlay",
00088:                 "faction_the_scale",
00089:                 "faction_the_compact",
00090:                 "faction_the_underwrite",
00091:                 "faction_the_cutters",
00092:                 "faction_the_fleet",
00093:                 "faction_the_rebuilders",
00094:                 "faction_the_garrison"
00095:             };
00096:
00097:             foreach (string expectedId in expectedFactions)
00098:             {
00099:                 var faction = catalog.GetFaction(expectedId);
00100:                 Assert.NotNull(faction);
00101:                 Assert.Equal(expectedId, faction.id);
00102:                 Assert.False(string.IsNullOrWhiteSpace(faction.display_name));
00103:                 Assert.False(string.IsNullOrWhiteSpace(faction.alignment));
00104:                 Assert.False(string.IsNullOrWhiteSpace(faction.home_region));
00105:                 Assert.False(string.IsNullOrWhiteSpace(faction.signature_quote));
00106:                 Assert.False(string.IsNullOrWhiteSpace(faction.access_rule));
00107:                 Assert.NotEmpty(faction.wants);
00108:                 Assert.NotEmpty(faction.offers);
00109:                 Assert.True(faction.is_active);
00110:             }
00111:
00112:             // Verify baseline faction "The Overlay" invariants
00113:             var overlay = catalog.GetFaction("faction_the_overlay");
00114:             Assert.NotNull(overlay);
00115:             Assert.Equal("The Overlay", overlay.display_name);
00116:             Assert.Equal("conditional", overlay.alignment);
00117:             Assert.Equal("all_regions", overlay.home_region);
00118:             Assert.Contains("cadastral_keys", overlay.offers);
00119:         }
00120:
00121:         [Fact]
00122:         public void CrossSystem_FactionAuthority_MapsDeterministicallyToEpilogueOutcomes()
00123:         {
00124:             string dataDir = ResolveDataDir();
00125:             var io = new FileSystemIO();
00126:             var json = new SystemTextJsonSerializer();
00127:             var loader = new StandingRecordCatalogLoader(io, json);
00128:
00129:             StandingRecordCatalog catalog = loader.Load(dataDir);
00130:             List<EndingDefinition> epilogues = EpilogueMatrixLoader.LoadEpilogues(dataDir, io, json);
00131:             var epilogueMap = epilogues.ToDictionary(e => e.endingKey, e => e);
00132:
00133:             // 1. Central Garrison integration outcome
00134:             var garrison = catalog.GetFaction("faction_the_garrison");
00135:             Assert.NotNull(garrison);
00136:             Assert.Equal("ash_flats", garrison.home_region);
00137:             var garrisonEnding = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
00138:             {
00139:                 FactionOutcome = FactionTerminalOutcome.GarrisonAbsorbed
00140:             });
00141:             Assert.Equal(EpilogueMatrix.GarrisonAbsorbsCoalition, garrisonEnding);
00142:             Assert.True(epilogueMap.ContainsKey(garrisonEnding));
00143:             Assert.Contains("Garrison", epilogueMap[garrisonEnding].prose);
00144:
00145:             // 2. Rebuilders cooperative outcome
00146:             var rebuilders = catalog.GetFaction("faction_the_rebuilders");
00147:             Assert.NotNull(rebuilders);
00148:             Assert.Equal("ash_flats", rebuilders.home_region);
00149:             var rebuildersEnding = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
00150:             {
00151:                 FactionOutcome = FactionTerminalOutcome.RebuildersJoined
00152:             });
00153:             Assert.Equal(EpilogueMatrix.RebuildersJoined, rebuildersEnding);
00154:             Assert.True(epilogueMap.ContainsKey(rebuildersEnding));
00155:             Assert.Contains("reconstruction", epilogueMap[rebuildersEnding].prose);
00156:
00157:             // 3. Independent stance defying external factions
00158:             var independentEnding = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
00159:             {
00160:                 FactionOutcome = FactionTerminalOutcome.Independent
00161:             });
00162:             Assert.Equal(EpilogueMatrix.CoalitionIndependent, independentEnding);
00163:             Assert.True(epilogueMap.ContainsKey(independentEnding));
00164:             Assert.Contains("No regional colors hang over the gate", epilogueMap[independentEnding].prose);
00165:
00166:             // 4. Resource control: The Scale (water authority) vs WaterPlantHeld epilogue
00167:             var theScale = catalog.GetFaction("faction_the_scale");
00168:             Assert.NotNull(theScale);
00169:             Assert.Equal("industrial_belt", theScale.home_region);
00170:             Assert.Contains("potable_ration_quota", theScale.offers);
00171:             var waterEnding = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
00172:             {
00173:                 WaterPlantHeld = true
00174:             });
00175:             Assert.Equal(EpilogueMatrix.WaterPlantHeld, waterEnding);
00176:             Assert.Contains("Desalination Unit 4", epilogueMap[waterEnding].prose);
00177:
00178:             // 5. Compound: Mercy + Water plant held
00179:             var mercyWaterEnding = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
00180:             {
00181:                 MercyPattern = true,
00182:                 WaterPlantHeld = true
00183:             });
00184:             Assert.Equal(EpilogueMatrix.MercyWaterHeld, mercyWaterEnding);
00185:             Assert.Contains("public cistern", epilogueMap[mercyWaterEnding].prose);
00186:         }
00187:
00188:         [Fact]
00189:         public void CrossSystem_EpilogueResolution_IsDeterministicAndStrictlyPrioritized()
00190:         {
00191:             // Catastrophic collapse overrides all faction standing or resource holds
00192:             var collapseInput = new EpilogueMatrixInput
00193:             {
00194:                 ShelterFallen = true,
00195:                 WaterPlantHeld = true,
00196:                 GrainSiloCaptured = true,
00197:                 FactionOutcome = FactionTerminalOutcome.GarrisonAbsorbed,
00198:                 MercyPattern = true
00199:             };
00200:
00201:             for (int i = 0; i < 100; i++)
00202:             {
00203:                 string outcome = EpilogueMatrix.Evaluate(collapseInput);
00204:                 Assert.Equal(EpilogueMatrix.ShelterFalls, outcome);
00205:             }
00206:
00207:             // Compound overrides specific verdict or faction outcomes when shelter intact
00208:             var compoundInput = new EpilogueMatrixInput
00209:             {
00210:                 IronPattern = true,
00211:                 FuelDepotBurned = true,
00212:                 FactionOutcome = FactionTerminalOutcome.FoundryAnnexed
00213:             };
00214:
00215:             for (int i = 0; i < 100; i++)
00216:             {
00217:                 string outcome = EpilogueMatrix.Evaluate(compoundInput);
00218:                 Assert.Equal(EpilogueMatrix.IronFuelAsh, outcome);
00219:             }
00220:
00221:             // Fallback unwritten outcome when no condition triggers
00222:             string fallback = EpilogueMatrix.Evaluate(new EpilogueMatrixInput());
00223:             Assert.Equal(EpilogueMatrix.Unwritten, fallback);
00224:         }
00225:     }
00226: }
```

## `Ashfall.Core.Tests/StandingRecordEngineTests.cs` — 195 lines; 7,302 bytes; SHA-256 `bc43c474f79012b93fbc87a8aeecd1a21f3470a4cf91b98d302992717c9f2cd9`
Declaration index:
- 00014: public class StandingRecordEngineTests
- 00029: private static StandingRecordEngine BuildEngine(StandingRecordState state = null)
- 00038: public void Ctor_StartsLocked_WithOverlayAccess()
- 00050: public void UnlockExpansion_SetsFlagAndDay()
- 00064: public void UnlockExpansion_IsIdempotent()
- 00074: public void Tick_UpdatesDayAndOverlayAccess()
- 00090: public void ApplySiteMutation_SetsMemoryFlag_AndLayoutFlag()
- 00103: public void ApplySiteMutation_GatedByUnlock()
- 00113: public void ApplySiteMutation_EmptyMutation_Rejected()
- 00121: public void CaptureState_RoundTrip_PreservesState()
- 00146: public void CaptureState_ReturnsIndependentClone_NotLiveAlias()
- 00160: public void Tick_IsDeterministicUnderSeedControl()
- 00186: internal static class StandingRecordStateTestExtensions
- 00188: public static int platesScrapedForExpeditionStep(this StandingRecordState state, int day)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core;
00005: using Xunit;
00006:
00007: namespace Ashfall.Core.Tests
00008: {
00009:     /// <summary>
00010:     /// Tests for <see cref="StandingRecordEngine"/> and the unified
00011:     /// StandingRecordState envelope. Mirrors the Phase-18
00012:     /// SkillProgressionSystem tests (12) with 8 cases.
00013:     /// </summary>
00014:     public class StandingRecordEngineTests
00015:     {
00016:         // -- Test fixture helpers ------------------------------------------
00017:
00018:         private static (IFileIO files, IJsonSerializer json, ISeededRng rng, NullLog log)
00019:             MakeWiring()
00020:         {
00021:             return (
00022:                 files: new FileSystemIO(),
00023:                 json: new SystemTextJsonSerializer(),
00024:                 rng: new SeededRng(1401),
00025:                 log: new NullLog()
00026:             );
00027:         }
00028:
00029:         private static StandingRecordEngine BuildEngine(StandingRecordState state = null)
00030:         {
00031:             var (files, json, rng, log) = MakeWiring();
00032:             return new StandingRecordEngine(files, json, rng, log, state);
00033:         }
00034:
00035:         // -- Tests ---------------------------------------------------------
00036:
00037:         [Fact]
00038:         public void Ctor_StartsLocked_WithOverlayAccess()
00039:         {
00040:             var engine = BuildEngine();
00041:             Assert.False(engine.IsUnlocked);
00042:             Assert.True(engine.HasOverlayAccess);
00043:             Assert.Equal(0, engine.CurrentDay);
00044:             Assert.NotNull(engine.Layouts);
00045:             Assert.NotNull(engine.Memory);
00046:             Assert.NotNull(engine.Encounters);
00047:         }
00048:
00049:         [Fact]
00050:         public void UnlockExpansion_SetsFlagAndDay()
00051:         {
00052:             var engine = BuildEngine();
00053:             engine.UnlockExpansion(currentDay: 12);
00054:
00055:             Assert.True(engine.IsUnlocked);
00056:             Assert.Equal(12, engine.CurrentDay);
00057:             Assert.True(engine.Layouts.IsUnlocked);
00058:             Assert.True(engine.Memory.IsUnlocked);
00059:             Assert.True(engine.Encounters.IsUnlocked);
00060:             Assert.True(engine.Memory.HasMutation(StandingRecordEngine.FlagExpUnlocked));
00061:         }
00062:
00063:         [Fact]
00064:         public void UnlockExpansion_IsIdempotent()
00065:         {
00066:             var engine = BuildEngine();
00067:             engine.UnlockExpansion(currentDay: 5);
00068:             engine.UnlockExpansion(currentDay: 99);
00069:             // Second call must NOT advance the day.
00070:             Assert.Equal(5, engine.CurrentDay);
00071:         }
00072:
00073:         [Fact]
00074:         public void Tick_UpdatesDayAndOverlayAccess()
00075:         {
00076:             var engine = BuildEngine();
00077:             engine.UnlockExpansion(currentDay: 1);
00078:
00079:             engine.Tick(newDay: 14);
00080:             Assert.Equal(14, engine.CurrentDay);
00081:
00082:             // Tick before unlock is a no-op.
00083:             var engine2 = BuildEngine();
00084:             engine2.Tick(newDay: 14);
00085:             Assert.Equal(0, engine2.CurrentDay);
00086:             Assert.False(engine2.IsUnlocked);
00087:         }
00088:
00089:         [Fact]
00090:         public void ApplySiteMutation_SetsMemoryFlag_AndLayoutFlag()
00091:         {
00092:             var engine = BuildEngine();
00093:             engine.UnlockExpansion(currentDay: 5);
00094:
00095:             Assert.True(engine.ApplySiteMutation(
00096:                 "loc_cut_kilometre_19", LocationMemorySystem.MutationKm19Plated));
00097:             Assert.True(engine.Memory.HasMutation(LocationMemorySystem.MutationKm19Plated));
00098:             Assert.True(engine.Layouts.HasFlag(
00099:                 "loc_cut_kilometre_19", LocationMemorySystem.MutationKm19Plated));
00100:         }
00101:
00102:         [Fact]
00103:         public void ApplySiteMutation_GatedByUnlock()
00104:         {
00105:             var engine = BuildEngine();
00106:             // Locked engine — mutation rejected.
00107:             Assert.False(engine.ApplySiteMutation(
00108:                 "loc_cut_kilometre_19", LocationMemorySystem.MutationKm19Plated));
00109:             Assert.False(engine.Memory.HasMutation(LocationMemorySystem.MutationKm19Plated));
00110:         }
00111:
00112:         [Fact]
00113:         public void ApplySiteMutation_EmptyMutation_Rejected()
00114:         {
00115:             var engine = BuildEngine();
00116:             engine.UnlockExpansion(currentDay: 5);
00117:             Assert.False(engine.ApplySiteMutation("loc_cut_kilometre_19", ""));
00118:         }
00119:
00120:         [Fact]
00121:         public void CaptureState_RoundTrip_PreservesState()
00122:         {
00123:             var engine = BuildEngine();
00124:             engine.UnlockExpansion(currentDay: 7);
00125:             engine.ApplySiteMutation(
00126:                 "loc_cut_kilometre_19", LocationMemorySystem.MutationKm19Plated);
00127:             engine.Tick(newDay: 18);
00128:
00129:             var saved = engine.CaptureState();
00130:             Assert.True(saved.expansionUnlocked);
00131:             Assert.Equal(18, saved.currentDay);
00132:             Assert.Contains(
00133:                 LocationMemorySystem.MutationKm19Plated, saved.memory.activeFlags);
00134:
00135:             // Round-trip into a fresh engine.
00136:             var (files, json, rng, log) = MakeWiring();
00137:             var engine2 = new StandingRecordEngine(files, json, rng, log, saved);
00138:             Assert.True(engine2.IsUnlocked);
00139:             Assert.Equal(18, engine2.CurrentDay);
00140:             Assert.True(engine2.Memory.HasMutation(LocationMemorySystem.MutationKm19Plated));
00141:             Assert.True(engine2.Layouts.HasFlag(
00142:                 "loc_cut_kilometre_19", LocationMemorySystem.MutationKm19Plated));
00143:         }
00144:
00145:         [Fact]
00146:         public void CaptureState_ReturnsIndependentClone_NotLiveAlias()
00147:         {
00148:             var engine = BuildEngine();
00149:             engine.UnlockExpansion(currentDay: 3);
00150:             var captured = engine.CaptureState();
00151:             Assert.NotSame(engine.State, captured);
00152:
00153:             captured.currentDay = 999;
00154:             captured.expansionUnlocked = false;
00155:             Assert.Equal(3, engine.CurrentDay);
00156:             Assert.True(engine.IsUnlocked);
00157:         }
00158:
00159:         [Fact]
00160:         public void Tick_IsDeterministicUnderSeedControl()
00161:         {
00162:             var engineA = BuildEngine();
00163:             engineA.UnlockExpansion(currentDay: 1);
00164:             engineA.Tick(newDay: 42);
00165:
00166:             var engineB = BuildEngine();
00167:             engineB.UnlockExpansion(currentDay: 1);
00168:             engineB.Tick(newDay: 42);
00169:
00170:             // Two engines with the same starting state and same day-step
00171:             // tick should produce identical Day and overlayAccess state.
00172:             Assert.Equal(engineA.CurrentDay, engineB.CurrentDay);
00173:             Assert.Equal(engineA.HasOverlayAccess, engineB.HasOverlayAccess);
00174:             Assert.Equal(engineA.State.platesScrapedForExpeditionStep(
00175:                 engineA.State.currentDay), engineB.State.platesScrapedForExpeditionStep(
00176:                 engineB.State.currentDay));
00177:         }
00178:     }
00179:
00180:     /// <summary>
00181:     /// Internal state-derived helper for the deterministic test surface;
00182:     /// kept in the tests namespace so it does not enlarge the engine
00183:     /// surface. The expected contract is: same seed + same tick input
00184:     /// ⇒ same overlay-access page.
00185:     /// </summary>
00186:     internal static class StandingRecordStateTestExtensions
00187:     {
00188:         public static int platesScrapedForExpeditionStep(this StandingRecordState state, int day)
00189:         {
00190:             // Read-only projection used by the deterministic-tick test
00191:             // to compare fixed-resolution state across engines.
00192:             return state.encounters != null ? state.encounters.platesScraped : 0;
00193:         }
00194:     }
00195: }
```

## `Ashfall.Core.Tests/StandingRecordQuestExpansionTests.cs` — 312 lines; 12,302 bytes; SHA-256 `09d1975c97791259b59d4c998a7a4e7c84d0cbb72a06e143b21cbfdf21f5dcfb`
Declaration index:
- 00012: public class StandingRecordQuestExpansionTests
- 00058: private static string GetDataDir()
- 00068: private static StandingRecordCatalog LoadCatalog()
- 00075: public void Catalog_Loads_All_32_Quests()
- 00082: public void All_Quest_Ids_Are_Unique_And_Prefixed()
- 00097: public void Baseline_10_Territorial_Quests_Preserved_Verbatim()
- 00116: public void Deepening_12_Site_Forensics_Quests_Preserved()
- 00133: public void All_10_New_Plan118_Quests_Present()
- 00169: public void Prerequisite_Graph_Has_No_Cycles_And_All_Prereqs_Resolve()
- 00194: private static void CheckCycle(
- 00218: public void Prerequisite_Day_Ordering_Is_Coherent()
- 00234: public void Every_Quest_Satisfies_World_Change_Bar_And_Spatial_Bar()
- 00247: public void Cross_Plan_Integrations_Verified()
- 00285: public void Engine_ApplySiteMutation_Idempotence()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.IO;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests
00011: {
00012:     public class StandingRecordQuestExpansionTests
00013:     {
00014:         private static readonly string[] s_tenBaselineTerritorialIds = new[]
00015:         {
00016:             "quest_record_the_plate",
00017:             "quest_record_grease_pencil",
00018:             "quest_record_wrong_stacks",
00019:             "quest_record_the_book",
00020:             "quest_record_mass_or_lot",
00021:             "quest_record_hands",
00022:             "quest_record_friendly_obstacle",
00023:             "quest_record_the_failure",
00024:             "quest_record_fallback",
00025:             "quest_record_which_gazetteer"
00026:         };
00027:
00028:         private static readonly string[] s_twelveDeepeningForensicsIds = new[]
00029:         {
00030:             "quest_record_vault_breach_forensics",
00031:             "quest_record_metro_derailment_triage",
00032:             "quest_record_mine_shaft_adit_collapse",
00033:             "quest_record_archive_burn_layer",
00034:             "quest_record_sluice_failure_verdict",
00035:             "quest_record_seed_bank_purge_trace",
00036:             "quest_record_sub_basement_blueprint",
00037:             "quest_record_transit_vent_shaft_route",
00038:             "quest_record_cold_store_sublevel",
00039:             "quest_record_utility_junction_crossover",
00040:             "quest_record_the_unmarked_plaque",
00041:             "quest_record_the_last_watch_beacon"
00042:         };
00043:
00044:         private static readonly string[] s_tenPlan118NewQuestIds = new[]
00045:         {
00046:             "quest_record_the_survey_nail",
00047:             "quest_record_the_overlay_pigment",
00048:             "quest_record_the_second_count",
00049:             "quest_record_the_lamp_keepers_oath",
00050:             "quest_record_the_boundary_dispute",
00051:             "quest_record_the_missing_plate",
00052:             "quest_record_the_cold_survey",
00053:             "quest_record_the_lamp_oil_ledger",
00054:             "quest_record_the_rejected_survey",
00055:             "quest_record_the_last_sector"
00056:         };
00057:
00058:         private static string GetDataDir()
00059:         {
00060:             string start = Directory.GetCurrentDirectory();
00061:             if (CatalogLocator.TryFindDataDirectory(start, out string found))
00062:                 return found;
00063:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
00064:                 return found;
00065:             throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
00066:         }
00067:
00068:         private static StandingRecordCatalog LoadCatalog()
00069:         {
00070:             var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
00071:             return loader.Load(GetDataDir());
00072:         }
00073:
00074:         [Fact]
00075:         public void Catalog_Loads_All_32_Quests()
00076:         {
00077:             var catalog = LoadCatalog();
00078:             Assert.True(catalog.Quests.Count >= 32, $"Expected at least 32 quests, got {catalog.Quests.Count}");
00079:         }
00080:
00081:         [Fact]
00082:         public void All_Quest_Ids_Are_Unique_And_Prefixed()
00083:         {
00084:             var catalog = LoadCatalog();
00085:             var ids = new HashSet<string>(StringComparer.Ordinal);
00086:
00087:             foreach (var q in catalog.Quests)
00088:             {
00089:                 Assert.NotNull(q);
00090:                 Assert.False(string.IsNullOrEmpty(q.id));
00091:                 Assert.StartsWith("quest_record_", q.id);
00092:                 Assert.True(ids.Add(q.id), $"Duplicate quest ID detected: '{q.id}'");
00093:             }
00094:         }
00095:
00096:         [Fact]
00097:         public void Baseline_10_Territorial_Quests_Preserved_Verbatim()
00098:         {
00099:             var catalog = LoadCatalog();
00100:             foreach (string id in s_tenBaselineTerritorialIds)
00101:             {
00102:                 var q = catalog.GetQuest(id);
00103:                 Assert.NotNull(q);
00104:                 Assert.Equal(id, q.id);
00105:                 Assert.NotEmpty(q.display_name);
00106:                 Assert.NotEmpty(q.briefing);
00107:                 Assert.NotEmpty(q.target_location_id);
00108:                 Assert.NotEmpty(q.complete_mutation);
00109:                 Assert.True(q.StageCount >= 3, $"Quest {id} must have >= 3 stages");
00110:                 Assert.NotNull(q.choices);
00111:                 Assert.True(q.choices.Length >= 2, $"Quest {id} must have >= 2 choices");
00112:             }
00113:         }
00114:
00115:         [Fact]
00116:         public void Deepening_12_Site_Forensics_Quests_Preserved()
00117:         {
00118:             var catalog = LoadCatalog();
00119:             foreach (string id in s_twelveDeepeningForensicsIds)
00120:             {
00121:                 var q = catalog.GetQuest(id);
00122:                 Assert.NotNull(q);
00123:                 Assert.Equal(id, q.id);
00124:                 Assert.NotEmpty(q.display_name);
00125:                 Assert.NotEmpty(q.briefing);
00126:                 Assert.NotEmpty(q.target_location_id);
00127:                 Assert.NotEmpty(q.complete_mutation);
00128:                 Assert.True(q.StageCount >= 3, $"Quest {id} must have >= 3 stages");
00129:             }
00130:         }
00131:
00132:         [Fact]
00133:         public void All_10_New_Plan118_Quests_Present()
00134:         {
00135:             var catalog = LoadCatalog();
00136:             foreach (string id in s_tenPlan118NewQuestIds)
00137:             {
00138:                 var q = catalog.GetQuest(id);
00139:                 Assert.NotNull(q);
00140:                 Assert.Equal(id, q.id);
00141:                 Assert.NotEmpty(q.display_name);
00142:                 Assert.NotEmpty(q.briefing);
00143:                 Assert.Equal("expedition", q.type);
00144:                 Assert.InRange(q.min_day, 80, 150);
00145:                 Assert.NotEmpty(q.target_location_id);
00146:                 Assert.NotEmpty(q.complete_mutation);
00147:                 Assert.NotEmpty(q.fail_mutation);
00148:                 Assert.NotNull(q.stages);
00149:                 Assert.InRange(q.stages.Length, 3, 6);
00150:                 Assert.NotNull(q.choices);
00151:                 Assert.InRange(q.choices.Length, 2, 4);
00152:
00153:                 foreach (var stage in q.stages)
00154:                 {
00155:                     Assert.NotEmpty(stage.id);
00156:                     Assert.NotEmpty(stage.text);
00157:                 }
00158:
00159:                 foreach (var choice in q.choices)
00160:                 {
00161:                     Assert.NotEmpty(choice.id);
00162:                     Assert.NotEmpty(choice.text);
00163:                     Assert.NotEmpty(choice.set_flag);
00164:                 }
00165:             }
00166:         }
00167:
00168:         [Fact]
00169:         public void Prerequisite_Graph_Has_No_Cycles_And_All_Prereqs_Resolve()
00170:         {
00171:             var catalog = LoadCatalog();
00172:             var questMap = catalog.Quests.ToDictionary(q => q.id, q => q, StringComparer.Ordinal);
00173:
00174:             foreach (var q in catalog.Quests)
00175:             {
00176:                 if (!string.IsNullOrEmpty(q.prereq_quest_id))
00177:                 {
00178:                     Assert.True(questMap.ContainsKey(q.prereq_quest_id),
00179:                         $"Quest '{q.id}' references non-existent prereq_quest_id '{q.prereq_quest_id}'");
00180:                     Assert.NotEqual(q.id, q.prereq_quest_id); // no self-dependency
00181:                 }
00182:             }
00183:
00184:             // Cycle detection via DFS / recursion stack
00185:             var visited = new HashSet<string>(StringComparer.Ordinal);
00186:             var inStack = new HashSet<string>(StringComparer.Ordinal);
00187:
00188:             foreach (var q in catalog.Quests)
00189:             {
00190:                 CheckCycle(q.id, questMap, visited, inStack);
00191:             }
00192:         }
00193:
00194:         private static void CheckCycle(
00195:             string currentId,
00196:             Dictionary<string, StandingRecordQuestEntry> map,
00197:             HashSet<string> visited,
00198:             HashSet<string> inStack)
00199:         {
00200:             if (inStack.Contains(currentId))
00201:             {
00202:                 Assert.Fail($"Prerequisite cycle detected involving quest: '{currentId}'");
00203:             }
00204:             if (visited.Contains(currentId)) return;
00205:
00206:             visited.Add(currentId);
00207:             inStack.Add(currentId);
00208:
00209:             if (map.TryGetValue(currentId, out var quest) && !string.IsNullOrEmpty(quest.prereq_quest_id))
00210:             {
00211:                 CheckCycle(quest.prereq_quest_id, map, visited, inStack);
00212:             }
00213:
00214:             inStack.Remove(currentId);
00215:         }
00216:
00217:         [Fact]
00218:         public void Prerequisite_Day_Ordering_Is_Coherent()
00219:         {
00220:             var catalog = LoadCatalog();
00221:             var questMap = catalog.Quests.ToDictionary(q => q.id, q => q, StringComparer.Ordinal);
00222:
00223:             foreach (var q in catalog.Quests)
00224:             {
00225:                 if (!string.IsNullOrEmpty(q.prereq_quest_id) && questMap.TryGetValue(q.prereq_quest_id, out var prereq))
00226:                 {
00227:                     Assert.True(q.min_day >= prereq.min_day,
00228:                         $"Quest '{q.id}' (day {q.min_day}) has prereq '{prereq.id}' with later day ({prereq.min_day})");
00229:                 }
00230:             }
00231:         }
00232:
00233:         [Fact]
00234:         public void Every_Quest_Satisfies_World_Change_Bar_And_Spatial_Bar()
00235:         {
00236:             var catalog = LoadCatalog();
00237:             foreach (var q in catalog.Quests)
00238:             {
00239:                 Assert.False(string.IsNullOrEmpty(q.complete_mutation),
00240:                     $"Quest '{q.id}' must specify a non-empty complete_mutation (world-change bar)");
00241:                 Assert.True(q.StageCount >= 3,
00242:                     $"Quest '{q.id}' must have >= 3 stages (spatial bar)");
00243:             }
00244:         }
00245:
00246:         [Fact]
00247:         public void Cross_Plan_Integrations_Verified()
00248:         {
00249:             var catalog = LoadCatalog();
00250:
00251:             // Plan 76 Expedition destinations
00252:             var coldSurvey = catalog.GetQuest("quest_record_the_cold_survey");
00253:             Assert.NotNull(coldSurvey);
00254:             Assert.Equal("loc_west_ridge_survey", coldSurvey.target_location_id);
00255:
00256:             var lastSector = catalog.GetQuest("quest_record_the_last_sector");
00257:             Assert.NotNull(lastSector);
00258:             Assert.Equal("loc_birchline_weather_station", lastSector.target_location_id);
00259:
00260:             // Plan 82 Verdict locations
00261:             var missingPlate = catalog.GetQuest("quest_record_the_missing_plate");
00262:             Assert.NotNull(missingPlate);
00263:             Assert.Equal("loc_abandoned_tide_gauge", missingPlate.target_location_id);
00264:
00265:             var rejectedSurvey = catalog.GetQuest("quest_record_the_rejected_survey");
00266:             Assert.NotNull(rejectedSurvey);
00267:             Assert.Equal("loc_coastal_meteorological_station", rejectedSurvey.target_location_id);
00268:
00269:             // Plan 98 Faction choices
00270:             var boundaryDispute = catalog.GetQuest("quest_record_the_boundary_dispute");
00271:             Assert.NotNull(boundaryDispute);
00272:             var disputeFlags = boundaryDispute.choices.Select(c => c.set_flag).ToList();
00273:             Assert.Contains("flag_sr_boundary_compact", disputeFlags);
00274:             Assert.Contains("flag_sr_boundary_garrison", disputeFlags);
00275:             Assert.Contains("flag_sr_boundary_neutral", disputeFlags);
00276:
00277:             var oilLedger = catalog.GetQuest("quest_record_the_lamp_oil_ledger");
00278:             Assert.NotNull(oilLedger);
00279:             var oilFlags = oilLedger.choices.Select(c => c.set_flag).ToList();
00280:             Assert.Contains("flag_sr_oil_prosecuted", oilFlags);
00281:             Assert.Contains("flag_sr_oil_authorized", oilFlags);
00282:         }
00283:
00284:         [Fact]
00285:         public void Engine_ApplySiteMutation_Idempotence()
00286:         {
00287:             var files = new FileSystemIO();
00288:             var json = new SystemTextJsonSerializer();
00289:             var rng = new SeededRng(1982);
00290:             var engine = new StandingRecordEngine(files, json, rng);
00291:             engine.UnlockExpansion(currentDay: 80);
00292:
00293:             // Apply complete mutation
00294:             string targetSite = "loc_cut_kilometre_19";
00295:             string testMutation = "mutation_survey_nail_verified";
00296:
00297:             Assert.True(engine.ApplySiteMutation(targetSite, testMutation));
00298:             Assert.True(engine.Memory.HasMutation(testMutation));
00299:             Assert.True(engine.Layouts.HasFlag(targetSite, testMutation));
00300:
00301:             // Repeated application is idempotent
00302:             Assert.True(engine.ApplySiteMutation(targetSite, testMutation));
00303:
00304:             // Capture and restore
00305:             var state = engine.CaptureState();
00306:             var engine2 = new StandingRecordEngine(files, json, rng, null, state);
00307:             Assert.True(engine2.IsUnlocked);
00308:             Assert.True(engine2.Memory.HasMutation(testMutation));
00309:             Assert.True(engine2.Layouts.HasFlag(targetSite, testMutation));
00310:         }
00311:     }
00312: }
```

## `src/Host/ExpansionHubSaveStore.cs` — 58 lines; 2,955 bytes; SHA-256 `ad69579b59d44fe320c4ffd7d2b7e77242246bd79b7a1e0068d4ceb51aac2945`
Declaration index:
- 00020: public static class ExpansionHubSaveStore
- 00036: public static string TryCaptureDirect(ExpansionHubSave state) => s_store.CaptureBare(state);
- 00039: public static ExpansionHubSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
- 00042: public static string TryCapture(ExpansionHubSave state) => s_store.CaptureBare(state);
- 00045: public static ExpansionHubSave? TryRestore(string json) => s_store.RestoreBare(json);
- 00048: public static bool TrySave(ExpansionHubSave save, string pathOverride = null!) =>
- 00052: public static ExpansionHubSave? TryLoad(string pathOverride = null!) =>
- 00056: public static string TryCapturePersisted(ExpansionHubSave save) => s_store.CapturePersisted(save);
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Save Store : ExpansionHubSaveStore
00004: // Core State : Ashfall.Core.ExpansionHubSave
00005: // Host Caller: Main.ExpansionHub, Main.Holdfast / ExpansionHubHostSession
00006: // Purpose    : Expansion module registry, activation flags, and cross-expansion telemetry
00007: // ============================================================================
00008: using Ashfall.Core;
00009: using Ashfall.Core.Save;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     /// <summary>
00014:     /// Persists <see cref="ExpansionHubSave"/> as JSON under
00015:     /// user://expansion_hub_save.json — thin façade over the Core
00016:     /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Shape and
00017:     /// validation live in <see cref="ExpansionHubSaveCodec"/>; path
00018:     /// resolution, atomic write, and error handling live in the service.
00019:     /// </summary>
00020:     public static class ExpansionHubSaveStore
00021:     {
00022:         public const string FileName = "expansion_hub_save.json";
00023:         public const string SectionName = "expansion_hub";
00024:
00025:         private static readonly SaveStore<ExpansionHubSave> s_store = SaveStoreHub.FromCodec(
00026:             FileName,
00027:             nameof(ExpansionHubSaveStore),
00028:             (save, json) => ExpansionHubSaveCodec.Encode(save, json),
00029:             (raw, json) => ExpansionHubSaveCodec.Decode(raw, json));
00030:
00031:         public static string SavePath => s_store.SavePath;
00032:
00033:         public static bool Exists => s_store.Exists();
00034:
00035:         /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
00036:         public static string TryCaptureDirect(ExpansionHubSave state) => s_store.CaptureBare(state);
00037:
00038:         /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
00039:         public static ExpansionHubSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
00040:
00041:         /// <summary>Capture state to JSON without writing to disk.</summary>
00042:         public static string TryCapture(ExpansionHubSave state) => s_store.CaptureBare(state);
00043:
00044:         /// <summary>Restore state from JSON without reading from disk.</summary>
00045:         public static ExpansionHubSave? TryRestore(string json) => s_store.RestoreBare(json);
00046:
00047:         /// <summary>Writes through the codec (checksum stamped). Returns false on failure.</summary>
00048:         public static bool TrySave(ExpansionHubSave save, string pathOverride = null!) =>
00049:             s_store.TrySave(save, pathOverride);
00050:
00051:         /// <summary>Reads and validates through the codec. Returns null when absent or corrupt.</summary>
00052:         public static ExpansionHubSave? TryLoad(string pathOverride = null!) =>
00053:             s_store.TryLoad(pathOverride);
00054:
00055:         /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
00056:         public static string TryCapturePersisted(ExpansionHubSave save) => s_store.CapturePersisted(save);
00057:     }
00058: }
```

## `Ashfall.Core.Tests/FactionIconCatalogTests.cs` — 137 lines; 6,249 bytes; SHA-256 `d205fd78a6689ab10c5f51b0e6b4273f3bf4c31b69fcae484912d7fa6e719b56`
Declaration index:
- 00010: public class FactionIconCatalogTests
- 00013: public void Resolve_HydroBarons_ReturnsCanonicalPath()
- 00020: public void Resolve_GuildHasExplicitNonFallbackEmblem()
- 00038: public void Resolve_AllCanonicalSystemsIds_HaveNonFallbackPath()
- 00057: public void Resolve_Unknown_FallsBackToBlankEmblem()
- 00064: public void Resolve_EmptyOrNull_ReturnsFallback()
- 00071: public void LoreNamespace_Aliases_NowMappedInCatalog()
- 00094: public void CoveredFactionIds_IsReadOnlyAndSealed()
- 00102: public void EveryMappedEmblem_ExistsOnDisk()
- 00121: public void ExpansionDeclaredFactions_AreMapped()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using Ashfall.Core;
00005: using Ashfall.Core.UI;
00006: using Xunit;
00007:
00008: namespace Ashfall.Core.Tests
00009: {
00010:     public class FactionIconCatalogTests
00011:     : CatalogTestBase{
00012:         [Fact]
00013:         public void Resolve_HydroBarons_ReturnsCanonicalPath()
00014:         {
00015:             var path = FactionIconCatalog.Resolve("faction_hydro_barons");
00016:             Assert.Equal("assets/ui/Icons/faction_icon_hydro_barons.png", path);
00017:         }
00018:
00019:         [Fact]
00020:         public void Resolve_GuildHasExplicitNonFallbackEmblem()
00021:         {
00022:             string p = FactionIconCatalog.Resolve(Ashfall.Core.Foundry.SilentFoundryIds.FactionId);
00023:             Assert.NotEqual(FactionIconCatalog.FallbackIconPath, p);
00024:             Assert.True(FactionIconCatalog.HasExplicitMapping(Ashfall.Core.Foundry.SilentFoundryIds.FactionId));
00025:             Assert.Equal("assets/ui/Icons/faction_icon_silent_foundry.png", p);
00026:
00027:             string dataDir = Directory.GetCurrentDirectory();
00028:             if (!CatalogLocator.TryFindDataDirectory(dataDir, out dataDir))
00029:                 CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
00030:             var files = new FileSystemIO();
00031:             var json = new SystemTextJsonSerializer();
00032:             var faction = Ashfall.Core.Foundry.SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json);
00033:             Assert.NotNull(faction);
00034:             Assert.Equal(p, faction.icon_path);
00035:         }
00036:
00037:         [Fact]
00038:         public void Resolve_AllCanonicalSystemsIds_HaveNonFallbackPath()
00039:         {
00040:             string[] ids = {
00041:                 "faction_archivists","faction_lamplighters","faction_quiet_house",
00042:                 "faction_grain_exchange","faction_sun_seekers","faction_osteophages",
00043:                 "faction_the_tally","faction_undertow","faction_cold_count",
00044:                 "faction_deserter_coalition","faction_the_provisioned",
00045:                 "faction_long_walk","faction_scavenger_guild","faction_iron_raiders",
00046:                 "faction_the_tempest","faction_hydro_barons"
00047:             };
00048:             foreach (var id in ids)
00049:             {
00050:                 var p = FactionIconCatalog.Resolve(id);
00051:                 Assert.NotEqual(FactionIconCatalog.FallbackIconPath, p);
00052:                 Assert.True(FactionIconCatalog.HasExplicitMapping(id));
00053:             }
00054:         }
00055:
00056:         [Fact]
00057:         public void Resolve_Unknown_FallsBackToBlankEmblem()
00058:         {
00059:             var path = FactionIconCatalog.Resolve("faction_unlisted_invented");
00060:             Assert.Equal(FactionIconCatalog.FallbackIconPath, path);
00061:         }
00062:
00063:         [Fact]
00064:         public void Resolve_EmptyOrNull_ReturnsFallback()
00065:         {
00066:             Assert.Equal(FactionIconCatalog.FallbackIconPath, FactionIconCatalog.Resolve(null));
00067:             Assert.Equal(FactionIconCatalog.FallbackIconPath, FactionIconCatalog.Resolve(string.Empty));
00068:         }
00069:
00070:         [Fact]
00071:         public void LoreNamespace_Aliases_NowMappedInCatalog()
00072:         {
00073:             Assert.True(FactionIconCatalog.HasExplicitMapping("scavenger_camp"));
00074:             Assert.True(FactionIconCatalog.HasExplicitMapping("cult_of_the_glow"));
00075:             Assert.True(FactionIconCatalog.HasExplicitMapping("military_remnants"));
00076:             Assert.True(FactionIconCatalog.HasExplicitMapping("upland_militia"));
00077:             Assert.True(FactionIconCatalog.HasExplicitMapping("rot_farmers"));
00078:             Assert.True(FactionIconCatalog.HasExplicitMapping("wire_heads"));
00079:             Assert.True(FactionIconCatalog.HasExplicitMapping("sump_dredgers"));
00080:             Assert.True(FactionIconCatalog.HasExplicitMapping("custodians"));
00081:             Assert.True(FactionIconCatalog.HasExplicitMapping("doomsday_preppers"));
00082:             Assert.True(FactionIconCatalog.HasExplicitMapping("echo_bats"));
00083:             Assert.True(FactionIconCatalog.HasExplicitMapping("safe_haven_community"));
00084:
00085:             foreach (var id in FactionIconCatalog.CoveredFactionIds)
00086:             {
00087:                 var path = FactionIconCatalog.Resolve(id);
00088:                 Assert.NotEqual(FactionIconCatalog.FallbackIconPath, path);
00089:                 Assert.StartsWith("assets/ui/Icons/", path);
00090:             }
00091:         }
00092:
00093:         [Fact]
00094:         public void CoveredFactionIds_IsReadOnlyAndSealed()
00095:         {
00096:             var ids = FactionIconCatalog.CoveredFactionIds;
00097:             // 28 original systems/lore mappings + 20 expansion & lore ids (including black flotilla).
00098:             Assert.Equal(48, ids.Count);
00099:         }
00100:
00101:         [Fact]
00102:         public void EveryMappedEmblem_ExistsOnDisk()
00103:         {
00104:             // Walk up from the test working directory to the repo root —
00105:             // the first ancestor that contains the Godot assets tree.
00106:             var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
00107:             while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "assets", "ui", "Icons")))
00108:                 dir = dir.Parent;
00109:             Assert.NotNull(dir);
00110:
00111:             foreach (var id in FactionIconCatalog.CoveredFactionIds)
00112:             {
00113:                 var path = FactionIconCatalog.Resolve(id);
00114:                 Assert.True(
00115:                     File.Exists(Path.Combine(dir!.FullName, path)),
00116:                     $"mapped emblem for {id} does not exist on disk: {path}");
00117:             }
00118:         }
00119:
00120:         [Fact]
00121:         public void ExpansionDeclaredFactions_AreMapped()
00122:         {
00123:             // crossing_factions.json
00124:             Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_compact"));
00125:             Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_scale"));
00126:             Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_underwrite"));
00127:             // holdfast_factions.json
00128:             Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_cutters"));
00129:             Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_fleet"));
00130:             Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_office"));
00131:             // standing_record_factions.json
00132:             Assert.True(FactionIconCatalog.HasExplicitMapping("faction_the_overlay"));
00133:             // currents.json 17th systems id
00134:             Assert.True(FactionIconCatalog.HasExplicitMapping("faction_blank_rows"));
00135:         }
00136:     }
00137: }
```

## `Assets/Ashfall.Core/StandingRecord/StandingRecordHeadlessDemo.cs` — 143 lines; 7,544 bytes; SHA-256 `cce5a40612aafc224ac200384441cf4cbd297b44edbaa43d9a3323be51f32d8b`
Declaration index:
- 00012: public static class StandingRecordHeadlessDemo
- 00014: public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null)
- 00045: Check(layoutSys.LayoutCount >= 14, "all 14 standing record layouts loaded");
- 00090: Check(memory.StratumCount >= 30, "standing record memory strata loaded (>=30)");
- 00114: Check(recordCat.Quests.Count >= 10, "standing record quests loaded (>=10)");
- 00128: Check(allBar, "every record main quest names a mutation and has 3+ objectives");
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: using System.Text;
00004:
00005: namespace Ashfall.Core
00006: {
00007:     /// <summary>
00008:     /// Headless verification smoke for Expansion 03: The Standing Record.
00009:     /// Tests: location layouts catalog loading, room hierarchy, room lighting / unlocking,
00010:     /// room inspection triggers, and layout state serialization.
00011:     /// </summary>
00012:     public static class StandingRecordHeadlessDemo
00013:     {
00014:         public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null)
00015:         {
00016:             CatalogLocator.UseInvariantCulture();
00017:             log = log ?? NullLog.Instance;
00018:             if (string.IsNullOrEmpty(dataDirectory))
00019:             {
00020:                 CatalogLocator.TryFindDataDirectory(System.Environment.CurrentDirectory, out dataDirectory);
00021:             }
00022:             var report = new HeadlessReport();
00023:
00024:             void Check(bool condition, string name)
00025:             {
00026:                 report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
00027:                 if (condition) report.PassedCount++;
00028:                 else
00029:                 {
00030:                     report.FailedCount++;
00031:                     log.Error("[FAIL] " + name);
00032:                 }
00033:                 if (condition) log.Info("[PASS] " + name);
00034:             }
00035:
00036:             log.Info("[StandingRecordHeadlessDemo] begin");
00037:
00038:             var files = new FileSystemIO();
00039:             var json = new SystemTextJsonSerializer();
00040:             var layoutSys = new LocationLayoutSystem(files, json, log);
00041:             layoutSys.Load(dataDirectory);
00042:
00043:             report.LocationCount = layoutSys.LayoutCount;
00044:
00045:             Check(layoutSys.LayoutCount >= 14, "all 14 standing record layouts loaded");
00046:             var km19 = layoutSys.GetLayout(LocationLayoutSystem.LocKilometre19);
00047:             Check(km19 != null, "loc_cut_kilometre_19 layout present");
00048:             Check(km19 != null && km19.RoomCount == 4, "km19 layout has 4 rooms");
00049:
00050:             var transit = layoutSys.GetLayout(LocationLayoutSystem.LocTransitHq);
00051:             Check(transit != null, "loc_transit_authority_hq layout present");
00052:             Check(transit != null && transit.RoomCount == 5, "transit HQ layout has 5 rooms");
00053:
00054:             var ministry = layoutSys.GetLayout("location_ministry_of_truth_bunker");
00055:             Check(ministry != null && ministry.RoomCount == 6, "ministry bunker layout has 6 rooms");
00056:
00057:             var lock4 = layoutSys.GetLayout("loc_lock_gate_four");
00058:             Check(lock4 != null && lock4.RoomCount == 6, "lock gate four layout has 6 rooms");
00059:
00060:             var vault = layoutSys.GetLayout("location_the_memory_vault");
00061:             Check(vault != null && vault.RoomCount == 6, "memory vault layout has 6 rooms");
00062:
00063:             // Navigation & Room inspection smoke
00064:             layoutSys.Unlock();
00065:             Check(layoutSys.ArriveAtParent(LocationLayoutSystem.LocKilometre19), "arrive at parent loc_cut_kilometre_19");
00066:             Check(layoutSys.CanEnter(LocationLayoutSystem.RoomKm19Post), "can enter entry room room_km19_post");
00067:             Check(!layoutSys.CanEnter(LocationLayoutSystem.RoomKm19Seam), "cannot enter room_km19_seam before post inspection");
00068:
00069:             bool entered = layoutSys.EnterRoom(LocationLayoutSystem.RoomKm19Post);
00070:             Check(entered, "entered room_km19_post");
00071:
00072:             bool inspected = layoutSys.InspectRoom(LocationLayoutSystem.RoomKm19Post);
00073:             Check(inspected, "inspected room_km19_post");
00074:             Check(layoutSys.CanEnter(LocationLayoutSystem.RoomKm19Seam), "adjacent room_km19_seam unlocked after post inspection");
00075:             Check(layoutSys.CanEnter(LocationLayoutSystem.RoomKm19OilTin), "adjacent room_km19_oil_tin unlocked after post inspection");
00076:
00077:             // Save / Load roundtrip
00078:             string blob = json.Serialize(layoutSys.CaptureState());
00079:             var restored = new LocationLayoutSystem(files, json, log);
00080:             restored.Load(dataDirectory);
00081:             restored.RestoreState(json.Deserialize<LocationLayoutState>(blob)!);
00082:
00083:             Check(restored.ArriveAtParent(LocationLayoutSystem.LocKilometre19), "arrive at parent after restore");
00084:             Check(restored.CanEnter(LocationLayoutSystem.RoomKm19Seam), "save roundtrip preserved unlocked room state");
00085:
00086:             // LocationMemorySystem — strata recasts
00087:             var memory = new LocationMemorySystem(files, json, log);
00088:             memory.Load(dataDirectory);
00089:             memory.Unlock();
00090:             Check(memory.StratumCount >= 30, "standing record memory strata loaded (>=30)");
00091:             Check(memory.GetStratumText("loc_cut_kilometre_19", "pre") != null, "km19 pre stratum present");
00092:             memory.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
00093:             Check(memory.GetActiveRecast("loc_cut_kilometre_19") != null, "km19 now recast after plated mutation");
00094:             memory.ApplyMutation(LocationMemorySystem.MutationKm19Scraped);
00095:             Check(memory.GetActiveRecast("loc_cut_kilometre_19")!.Contains("short one post"),
00096:                 "scraped recast wins over plated");
00097:
00098:             // SiteEncounterSystem — room-keyed, Overlay withdraws after 3 scrapes
00099:             var site = new SiteEncounterSystem(1808);
00100:             site.Unlock();
00101:             Check(site.StartEncounter("enc_site_plate_screwer", "room_km19_post",
00102:                 SiteEncounterSystem.KindPlateScrewer, 75, "mutation_km19_plated"),
00103:                 "site encounter starts");
00104:             Check(site.ResolveEncounter("enc_site_plate_screwer", 75), "site encounter resolves");
00105:             for (int s = 0; s < SiteEncounterSystem.OverlayWithdrawPlateCount; s++)
00106:                 site.ScrapePlate(76 + s);
00107:             Check(!site.OverlayAccess, "three scrapes withdraw Overlay access");
00108:             Check(site.PlatesScraped == SiteEncounterSystem.OverlayWithdrawPlateCount,
00109:                 "plate count recorded");
00110:
00111:             // StandingRecordCatalog — all ten mains with world-change bars
00112:             var catLoader = new StandingRecordCatalogLoader(files, json, log);
00113:             var recordCat = catLoader.Load(dataDirectory);
00114:             Check(recordCat.Quests.Count >= 10, "standing record quests loaded (>=10)");
00115:             Check(recordCat.GetQuest("quest_record_the_book") != null, "quest_record_the_book present");
00116:             Check(recordCat.GetQuest("quest_record_which_gazetteer") != null,
00117:                 "quest_record_which_gazetteer present (spine end)");
00118:             bool allBar = true;
00119:             for (int i = 0; i < recordCat.Quests.Count; i++)
00120:             {
00121:                 if (string.IsNullOrEmpty(recordCat.Quests[i].complete_mutation)
00122:                     || recordCat.Quests[i].StageCount < 3)
00123:                 {
00124:                     allBar = false;
00125:                     break;
00126:                 }
00127:             }
00128:             Check(allBar, "every record main quest names a mutation and has 3+ objectives");
00129:
00130:             report.Passed = report.FailedCount == 0;
00131:             var sb = new StringBuilder();
00132:             sb.Append("StandingRecordHeadlessDemo ");
00133:             sb.Append(report.Passed ? "PASS" : "FAIL");
00134:             sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
00135:             sb.Append(" layouts=").Append(report.LocationCount)
00136:                 .Append(" quests=").Append(recordCat.Quests.Count)
00137:                 .Append(" strata=").Append(memory.StratumCount);
00138:             report.Summary = sb.ToString();
00139:             log.Info(report.Summary);
00140:             return report;
00141:         }
00142:     }
00143: }
```

## `src/Host/HostCli.ExpansionDepth.cs` — 144 lines; 7,946 bytes; SHA-256 `c2e979fba2fe17db74263330c5ba2382dfc878788a22ca81b8df117457f89ff8`
Declaration index:
- 00011: public static partial class HostCli
- 00020: public static int RunExpansionDepthSelfTest(string dataDirectory)
- 00109: private sealed class VerdictQuestlinesRoot
- 00115: private sealed class VerdictQuestlineDef
- 00121: private sealed class VerdictNpcsRoot
- 00127: private sealed class VerdictNpcDef
- 00133: private sealed class QuestlineMasterRoot
- 00139: private sealed class QuestlineMasterEntryDef
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Collections.Generic;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Narrative;
00008:
00009: namespace AtomicWar.GodotApp
00010: {
00011:     public static partial class HostCli
00012:     {
00013:         /// <summary>
00014:         /// --expansion-depth-selftest / --plan18-selftest:
00015:         /// Verifies Plan 18 Expansion Deepening:
00016:         /// Holdfast (24 quests), Standing Record (52 memories, 22 quests),
00017:         /// Crossing (20 quests, 14 encounters), Verdict (16 questlines, 9 NPCs),
00018:         /// cross-expansion evidence hooks, and save stability.
00019:         /// </summary>
00020:         public static int RunExpansionDepthSelfTest(string dataDirectory)
00021:         {
00022:             CatalogLocator.UseInvariantCulture();
00023:             int failures = 0;
00024:             int totalAssertions = 0;
00025:
00026:             void Check(bool ok, string label)
00027:             {
00028:                 totalAssertions++;
00029:                 GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
00030:                 if (!ok) failures++;
00031:             }
00032:
00033:             GD.Print("[ExpansionDepthHeadlessDemo] begin Plan 18 verification...");
00034:
00035:             var json = new SystemTextJsonSerializer();
00036:             var files = new FileSystemIO();
00037:
00038:             // 1. Holdfast (24 quests, 38 locations)
00039:             var holdfastCatalog = new HoldfastCatalogLoader(files, json, NullLog.Instance).Load(dataDirectory);
00040:             Check(holdfastCatalog != null, "Holdfast catalog loaded");
00041:             Check(holdfastCatalog != null && holdfastCatalog.Quests.Count >= 22, $"Holdfast quests count (expected >= 22, got {holdfastCatalog?.Quests.Count ?? 0})");
00042:             Check(holdfastCatalog != null && holdfastCatalog.Locations.Count >= 30, $"Holdfast locations count (expected >= 30, got {holdfastCatalog?.Locations.Count ?? 0})");
00043:             Check(holdfastCatalog != null && holdfastCatalog.GetQuest("quest_holdfast_salt_convoy_haul") != null, "Holdfast salt convoy haul quest present");
00044:             Check(holdfastCatalog != null && holdfastCatalog.GetQuest("quest_holdfast_census_claimant_audit") != null, "Holdfast census claimant audit quest present");
00045:             Check(holdfastCatalog != null && holdfastCatalog.GetQuest("quest_holdfast_brine_boiler_scum") != null, "Holdfast brine boiler scum quest present");
00046:
00047:             // 2. Standing Record (52 memories, 22 quests, 14 layouts)
00048:             var standingRecordCat = new StandingRecordCatalogLoader(files, json, NullLog.Instance).Load(dataDirectory);
00049:             Check(standingRecordCat != null && standingRecordCat.Quests.Count >= 22, $"Standing Record quests count (expected >= 22, got {standingRecordCat?.Quests.Count ?? 0})");
00050:             Check(standingRecordCat != null && standingRecordCat.GetQuest("quest_record_vault_breach_forensics") != null, "Standing Record vault breach quest present");
00051:             Check(standingRecordCat != null && standingRecordCat.GetQuest("quest_record_the_unmarked_plaque") != null, "Standing Record memorial plaque quest present");
00052:
00053:             var memSys = new LocationMemorySystem(files, json, NullLog.Instance);
00054:             memSys.Load(dataDirectory);
00055:             Check(memSys.StratumCount >= 50, $"Standing Record memories count (expected >= 50, got {memSys.StratumCount})");
00056:
00057:             var layoutSys = new LocationLayoutSystem(files, json, NullLog.Instance);
00058:             layoutSys.Load(dataDirectory);
00059:             Check(layoutSys.LayoutCount >= 14, $"Standing Record layouts count (expected >= 14, got {layoutSys.LayoutCount})");
00060:
00061:             // 3. Crossing (20 quests, 14 encounters)
00062:             var crossingSession = CrossingSession.Load(dataDirectory, NullLog.Instance);
00063:             var crossingCatalog = crossingSession?.Catalog;
00064:             Check(crossingCatalog != null, "Crossing catalog loaded");
00065:             Check(crossingCatalog != null && crossingCatalog.Quests.Count >= 20, $"Crossing quests count (expected >= 20, got {crossingCatalog?.Quests.Count ?? 0})");
00066:             Check(crossingCatalog != null && crossingCatalog.Encounters.Count >= 14, $"Crossing encounters count (expected >= 14, got {crossingCatalog?.Encounters.Count ?? 0})");
00067:             Check(crossingCatalog != null && crossingCatalog.GetQuest("quest_crossing_asylum_in_the_truss") != null, "Crossing asylum quest present");
00068:             Check(crossingCatalog != null && crossingCatalog.GetEncounter("enc_nc_mass_crossing_surge") != null, "Crossing mass surge crisis present");
00069:
00070:             // 4. Verdict (16 questlines, 9 NPCs)
00071:             string qlPath = files.Combine(dataDirectory, "verdict_questlines.json");
00072:             if (files.FileExists(qlPath))
00073:             {
00074:                 var root = json.Deserialize<VerdictQuestlinesRoot>(files.ReadAllText(qlPath));
00075:                 Check(root != null && root.quests != null && root.quests.Count >= 16, $"Verdict questlines count (expected >= 16, got {root?.quests?.Count ?? 0})");
00076:                 Check(root != null && root.quests != null && root.quests.Any(q => q.questlineId == "quest_verdict_alibi_verification"), "Verdict alibi verification questline present");
00077:                 Check(root != null && root.quests != null && root.quests.Any(q => q.questlineId == "quest_verdict_prior_verdict_appeal"), "Verdict prior verdict appeal questline present");
00078:             }
00079:             else
00080:             {
00081:                 Check(false, "verdict_questlines.json exists");
00082:             }
00083:
00084:             string npcPath = files.Combine(dataDirectory, "verdict_npcs.json");
00085:             if (files.FileExists(npcPath))
00086:             {
00087:                 var root = json.Deserialize<VerdictNpcsRoot>(files.ReadAllText(npcPath));
00088:                 Check(root != null && root.items != null && root.items.Count >= 9, $"Verdict NPCs count (expected >= 9, got {root?.items?.Count ?? 0})");
00089:                 Check(root != null && root.items != null && root.items.Any(n => n.id == "npc_tomas_reid"), "Verdict defense clerk Tomas Reid present");
00090:                 Check(root != null && root.items != null && root.items.Any(n => n.id == "npc_elena_vane"), "Verdict cult deaconess Elena Vane present");
00091:             }
00092:             else
00093:             {
00094:                 Check(false, "verdict_npcs.json exists");
00095:             }
00096:
00097:             // 5. Questline Master sync check (437 entries)
00098:             string masterPath = files.Combine(dataDirectory, "questline_master.json");
00099:             if (files.FileExists(masterPath))
00100:             {
00101:                 var master = json.Deserialize<QuestlineMasterRoot>(files.ReadAllText(masterPath));
00102:                 Check(master != null && master.entries != null && master.entries.Count >= 400, $"Questline master entries count (expected >= 400, got {master?.entries?.Count ?? 0})");
00103:             }
00104:
00105:             GD.Print($"[ExpansionDepthHeadlessDemo] completed with {failures} failures across {totalAssertions} assertions.");
00106:             return failures == 0 ? 0 : 1;
00107:         }
00108:
00109:         private sealed class VerdictQuestlinesRoot
00110:         {
00111:             public int schema_version { get; set; }
00112:             public List<VerdictQuestlineDef>? quests { get; set; }
00113:         }
00114:
00115:         private sealed class VerdictQuestlineDef
00116:         {
00117:             public string questlineId { get; set; } = string.Empty;
00118:             public string title { get; set; } = string.Empty;
00119:         }
00120:
00121:         private sealed class VerdictNpcsRoot
00122:         {
00123:             public int schema_version { get; set; }
00124:             public List<VerdictNpcDef>? items { get; set; }
00125:         }
00126:
00127:         private sealed class VerdictNpcDef
00128:         {
00129:             public string id { get; set; } = string.Empty;
00130:             public string name { get; set; } = string.Empty;
00131:         }
00132:
00133:         private sealed class QuestlineMasterRoot
00134:         {
00135:             public int schema_version { get; set; }
00136:             public List<QuestlineMasterEntryDef>? entries { get; set; }
00137:         }
00138:
00139:         private sealed class QuestlineMasterEntryDef
00140:         {
00141:             public string id { get; set; } = string.Empty;
00142:         }
00143:     }
00144: }
```

## `Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs` — 239 lines; 11,493 bytes; SHA-256 `36858a772bf3a9b288f691c8ce875f00e2dd2d87b2535111569166067220e739`
Declaration index:
- 00012: public sealed class Plan118_120StandingCrossingIntegrationTests
- 00054: public void Plan118_StandingRecordQuests_LoadsAllAuthoredQuestsAndVerifiesPlan118TenExpansionQuests()
- 00106: public void Plan120_CrossingFactions_LoadsExactEightFactionsWithDistinctTradeProfiles()
- 00150: public void Plan118_120_CrossDomainCoherence_BorderTerritoryAndLamplighterLinkages()
- 00187: public void Plan118_120_DeterministicQuestChoiceResolutionAndFactionTrust()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.IO;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests.World
00011: {
00012:     public sealed class Plan118_120StandingCrossingIntegrationTests
00013:     {
00014:         private static string DataDirectory
00015:         {
00016:             get
00017:             {
00018:                 string start = Directory.GetCurrentDirectory();
00019:                 if (CatalogLocator.TryFindDataDirectory(start, out string found))
00020:                     return found;
00021:                 if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
00022:                     return found;
00023:                 throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
00024:             }
00025:         }
00026:
00027:         private static readonly string[] s_plan118QuestIds = new[]
00028:         {
00029:             "quest_record_the_survey_nail",
00030:             "quest_record_the_overlay_pigment",
00031:             "quest_record_the_second_count",
00032:             "quest_record_the_lamp_keepers_oath",
00033:             "quest_record_the_boundary_dispute",
00034:             "quest_record_the_missing_plate",
00035:             "quest_record_the_cold_survey",
00036:             "quest_record_the_lamp_oil_ledger",
00037:             "quest_record_the_rejected_survey",
00038:             "quest_record_the_last_sector"
00039:         };
00040:
00041:         private static readonly string[] s_allEightCrossingFactionIds = new[]
00042:         {
00043:             CrossingIds.FactionScale,
00044:             CrossingIds.FactionUnderwrite,
00045:             CrossingIds.FactionCompact,
00046:             CrossingIds.FactionLamplighters,
00047:             CrossingIds.FactionGranaryWardens,
00048:             CrossingIds.FactionWaterCommittee,
00049:             CrossingIds.FactionQuarantinePost,
00050:             CrossingIds.FactionSmugglersCourt
00051:         };
00052:
00053:         [Fact]
00054:         public void Plan118_StandingRecordQuests_LoadsAllAuthoredQuestsAndVerifiesPlan118TenExpansionQuests()
00055:         {
00056:             var files = new FileSystemIO();
00057:             var json = new SystemTextJsonSerializer();
00058:             var loader = new StandingRecordCatalogLoader(files, json);
00059:             var catalog = loader.Load(DataDirectory);
00060:
00061:             Assert.NotNull(catalog);
00062:             Assert.True(catalog.Quests.Count >= 32, $"Expected >= 32 quests in Standing Record, found {catalog.Quests.Count}");
00063:
00064:             var questMap = catalog.Quests.ToDictionary(q => q.id, StringComparer.Ordinal);
00065:
00066:             foreach (string expectedId in s_plan118QuestIds)
00067:             {
00068:                 Assert.True(questMap.ContainsKey(expectedId), $"Standing Record quest catalog missing Plan 118 quest '{expectedId}'");
00069:                 var quest = questMap[expectedId];
00070:
00071:                 Assert.NotNull(quest);
00072:                 Assert.False(string.IsNullOrWhiteSpace(quest.display_name), $"Quest '{expectedId}' missing display_name");
00073:                 Assert.False(string.IsNullOrWhiteSpace(quest.type), $"Quest '{expectedId}' missing type");
00074:                 Assert.False(string.IsNullOrWhiteSpace(quest.briefing), $"Quest '{expectedId}' missing briefing");
00075:                 Assert.True(quest.min_day >= 0, $"Quest '{expectedId}' invalid min_day {quest.min_day}");
00076:                 Assert.False(string.IsNullOrWhiteSpace(quest.target_location_id), $"Quest '{expectedId}' missing target_location_id");
00077:
00078:                 // Verify stages
00079:                 Assert.NotNull(quest.stages);
00080:                 Assert.NotEmpty(quest.stages);
00081:                 Assert.True(quest.StageCount >= 2, $"Quest '{expectedId}' has fewer than 2 stages");
00082:                 foreach (var stage in quest.stages)
00083:                 {
00084:                     Assert.False(string.IsNullOrWhiteSpace(stage.id), $"Stage in quest '{expectedId}' has empty id");
00085:                     Assert.False(string.IsNullOrWhiteSpace(stage.text), $"Stage '{stage.id}' in quest '{expectedId}' has empty text");
00086:                 }
00087:
00088:                 // Verify choices
00089:                 Assert.NotNull(quest.choices);
00090:                 Assert.NotEmpty(quest.choices);
00091:                 Assert.True(quest.choices.Length >= 2, $"Quest '{expectedId}' must have at least 2 choices");
00092:                 foreach (var choice in quest.choices)
00093:                 {
00094:                     Assert.False(string.IsNullOrWhiteSpace(choice.id), $"Choice in quest '{expectedId}' has empty id");
00095:                     Assert.False(string.IsNullOrWhiteSpace(choice.text), $"Choice '{choice.id}' in quest '{expectedId}' has empty text");
00096:                     Assert.False(string.IsNullOrWhiteSpace(choice.set_flag), $"Choice '{choice.id}' in quest '{expectedId}' has empty set_flag");
00097:                 }
00098:
00099:                 // Verify mutations
00100:                 Assert.False(string.IsNullOrWhiteSpace(quest.complete_mutation), $"Quest '{expectedId}' missing complete_mutation");
00101:                 Assert.False(string.IsNullOrWhiteSpace(quest.fail_mutation), $"Quest '{expectedId}' missing fail_mutation");
00102:             }
00103:         }
00104:
00105:         [Fact]
00106:         public void Plan120_CrossingFactions_LoadsExactEightFactionsWithDistinctTradeProfiles()
00107:         {
00108:             var files = new FileSystemIO();
00109:             var json = new SystemTextJsonSerializer();
00110:             var loader = new CrossingCatalogLoader(files, json);
00111:             var catalog = loader.Load(DataDirectory);
00112:
00113:             Assert.NotNull(catalog);
00114:             Assert.Equal(8, catalog.Factions.Count);
00115:
00116:             var factionMap = catalog.Factions.ToDictionary(f => f.id, StringComparer.Ordinal);
00117:             var seenQuotes = new HashSet<string>(StringComparer.Ordinal);
00118:             var seenWantsSignatures = new HashSet<string>(StringComparer.Ordinal);
00119:
00120:             foreach (string expectedId in s_allEightCrossingFactionIds)
00121:             {
00122:                 Assert.True(factionMap.ContainsKey(expectedId), $"Crossing catalog missing faction '{expectedId}'");
00123:                 var faction = factionMap[expectedId];
00124:
00125:                 Assert.NotNull(faction);
00126:                 Assert.False(string.IsNullOrWhiteSpace(faction.display_name), $"Faction '{expectedId}' missing display_name");
00127:                 Assert.True(faction.is_active, $"Faction '{expectedId}' should be active");
00128:                 Assert.False(string.IsNullOrWhiteSpace(faction.alignment), $"Faction '{expectedId}' missing alignment");
00129:                 Assert.False(string.IsNullOrWhiteSpace(faction.home_region), $"Faction '{expectedId}' missing home_region");
00130:                 Assert.Equal(CrossingIds.Region, faction.home_region);
00131:
00132:                 // Wants and Offers
00133:                 Assert.NotNull(faction.wants);
00134:                 Assert.NotEmpty(faction.wants);
00135:                 Assert.NotNull(faction.offers);
00136:                 Assert.NotEmpty(faction.offers);
00137:
00138:                 // Verify distinct trade signatures
00139:                 string wantsSig = string.Join(",", faction.wants.OrderBy(w => w, StringComparer.Ordinal));
00140:                 Assert.True(seenWantsSignatures.Add(wantsSig), $"Duplicate wants signature detected for faction '{expectedId}': {wantsSig}");
00141:
00142:                 // Signature Quote & Access Rule
00143:                 Assert.False(string.IsNullOrWhiteSpace(faction.signature_quote), $"Faction '{expectedId}' missing signature_quote");
00144:                 Assert.True(seenQuotes.Add(faction.signature_quote), $"Duplicate signature quote detected for faction '{expectedId}'");
00145:                 Assert.False(string.IsNullOrWhiteSpace(faction.access_rule), $"Faction '{expectedId}' missing access_rule");
00146:             }
00147:         }
00148:
00149:         [Fact]
00150:         public void Plan118_120_CrossDomainCoherence_BorderTerritoryAndLamplighterLinkages()
00151:         {
00152:             var files = new FileSystemIO();
00153:             var json = new SystemTextJsonSerializer();
00154:
00155:             var crossingLoader = new CrossingCatalogLoader(files, json);
00156:             var crossing = crossingLoader.Load(DataDirectory);
00157:
00158:             var standingLoader = new StandingRecordCatalogLoader(files, json);
00159:             var standing = standingLoader.Load(DataDirectory);
00160:
00161:             // Crossing governs settlement factions (Rule 5)
00162:             var lamplighters = crossing.GetFaction(CrossingIds.FactionLamplighters);
00163:             Assert.NotNull(lamplighters);
00164:             Assert.Contains("street_lighting", lamplighters!.offers);
00165:             Assert.Contains("fuel_stores", lamplighters.wants);
00166:
00167:             // Standing Record governs territorial sector lamps and oil ledgers (Rule 5)
00168:             var oathQuest = standing.GetQuest("quest_record_the_lamp_keepers_oath");
00169:             Assert.NotNull(oathQuest);
00170:             Assert.Contains("keeper's oath", oathQuest!.briefing, StringComparison.OrdinalIgnoreCase);
00171:
00172:             var oilLedgerQuest = standing.GetQuest("quest_record_the_lamp_oil_ledger");
00173:             Assert.NotNull(oilLedgerQuest);
00174:             Assert.Contains("oil ledger", oilLedgerQuest!.briefing, StringComparison.OrdinalIgnoreCase);
00175:
00176:             // Boundary dispute quest links territory disputes to arbitration
00177:             var disputeQuest = standing.GetQuest("quest_record_the_boundary_dispute");
00178:             Assert.NotNull(disputeQuest);
00179:             Assert.Contains("boundary", disputeQuest!.briefing, StringComparison.OrdinalIgnoreCase);
00180:
00181:             // Ensure neither catalog leaks domain authority to the other
00182:             Assert.All(crossing.Factions, f => Assert.StartsWith("faction_", f.id));
00183:             Assert.All(standing.Quests, q => Assert.StartsWith("quest_record_", q.id));
00184:         }
00185:
00186:         [Fact]
00187:         public void Plan118_120_DeterministicQuestChoiceResolutionAndFactionTrust()
00188:         {
00189:             var files = new FileSystemIO();
00190:             var json = new SystemTextJsonSerializer();
00191:
00192:             var standingLoader = new StandingRecordCatalogLoader(files, json);
00193:             var standing = standingLoader.Load(DataDirectory);
00194:
00195:             var crossingLoader = new CrossingCatalogLoader(files, json);
00196:             var crossing = crossingLoader.Load(DataDirectory);
00197:
00198:             // Simulate deterministic choice selection across 2 seeded runs
00199:             int seed = 42817;
00200:             var rng1 = new SeededRng(seed);
00201:             var rng2 = new SeededRng(seed);
00202:
00203:             var choicesRun1 = new List<string>();
00204:             var choicesRun2 = new List<string>();
00205:
00206:             foreach (string qId in s_plan118QuestIds)
00207:             {
00208:                 var quest = standing.GetQuest(qId);
00209:                 Assert.NotNull(quest);
00210:
00211:                 int pickIndex1 = rng1.Next(0, quest!.choices.Length);
00212:                 int pickIndex2 = rng2.Next(0, quest.choices.Length);
00213:
00214:                 Assert.Equal(pickIndex1, pickIndex2);
00215:                 choicesRun1.Add(quest.choices[pickIndex1].set_flag);
00216:                 choicesRun2.Add(quest.choices[pickIndex2].set_flag);
00217:             }
00218:
00219:             // Both runs must yield bitwise identical choice flags
00220:             Assert.Equal(choicesRun1, choicesRun2);
00221:
00222:             // Simulate faction trust adjustment deterministically
00223:             var trustRun1 = new Dictionary<string, float>(StringComparer.Ordinal);
00224:             var trustRun2 = new Dictionary<string, float>(StringComparer.Ordinal);
00225:
00226:             foreach (var f in crossing.Factions)
00227:             {
00228:                 float delta1 = (float)(rng1.NextDouble() * 20.0 - 10.0);
00229:                 float delta2 = (float)(rng2.NextDouble() * 20.0 - 10.0);
00230:
00231:                 Assert.Equal(delta1, delta2);
00232:                 trustRun1[f.id] = f.trust + delta1;
00233:                 trustRun2[f.id] = f.trust + delta2;
00234:             }
00235:
00236:             Assert.Equal(trustRun1, trustRun2);
00237:         }
00238:     }
00239: }
```

## `Ashfall.Core.Tests/VersionReportContractTests.cs` — 223 lines; 11,846 bytes; SHA-256 `10679cb9d3ce3baf342e14236cf51cae5a7b949969ff606ffa77fd85c0fbe34f`
Declaration index:
- 00013: public class VersionReportContractTests
- 00015: private static string FindRepoRoot()
- 00031: public void Compose_RendersPinnedHeaderAndSectionLines()
- 00042: public void Compose_ContainsEverySaveStoreVersion()
- 00053: public void SaveSchemaVersions_ListsEveryVersionedSaveCodec()
- 00066: public void SaveSchemaVersions_MatchTheActualCodecConstants()
- 00086: public void ScanDataSchemas_TalliesVersionsAndMissingDeclarations()
- 00121: public void ScanDataSchemas_MissingDirectory_ReturnsEmptySummary()
- 00132: public void FormatDataSchemas_RendersDistributionWithMax()
- 00146: public void ScanDataSchemas_LiveDataAuthority_IsNonTrivialAndCurrent()
- 00163: public void AllPersistenceFormats_CoversAllSaveSectionRegistrySections()
- 00172: public void AllPersistenceFormats_DistinguishesVersionedCodecsAndChecksumEnvelopes()
- 00211: public void FormatPersistenceInventory_RendersSummaryAndEntries()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL gate: the host `--version` flag renders the pinned VersionReport
00003: // contract — build/game version line, live data-authority schema summary,
00004: // and save-codec schema versions sourced from the actual CurrentSaveVersion
00005: // constants. If the output shape or a codec version regresses, these fail.
00006: using System;
00007: using System.IO;
00008: using System.Linq;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests
00012: {
00013:     public class VersionReportContractTests
00014:     {
00015:         private static string FindRepoRoot()
00016:         {
00017:             var dir = new DirectoryInfo(Path.GetFullPath(AppContext.BaseDirectory));
00018:             while (dir != null)
00019:             {
00020:                 if (Directory.Exists(Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data")))
00021:                     return dir.FullName;
00022:                 dir = dir.Parent;
00023:             }
00024:             throw new FileNotFoundException(
00025:                 "Could not locate the repository root (Assets/StreamingAssets/Data) from the test run");
00026:         }
00027:
00028:         // ── Output shape contract ────────────────────────────────────────
00029:
00030:         [Fact]
00031:         public void Compose_RendersPinnedHeaderAndSectionLines()
00032:         {
00033:             string report = VersionReport.Compose("9.9.9-test", dataDir: null);
00034:
00035:             Assert.StartsWith("ASHFALL version report", report, StringComparison.Ordinal);
00036:             Assert.Contains("game         : 9.9.9-test", report);
00037:             Assert.Contains("data schemas : no data directory", report);
00038:             Assert.Contains("save schemas : ", report);
00039:         }
00040:
00041:         [Fact]
00042:         public void Compose_ContainsEverySaveStoreVersion()
00043:         {
00044:             string report = VersionReport.Compose("1.0.0", dataDir: null);
00045:
00046:             foreach (var entry in VersionReport.SaveSchemaVersions)
00047:             {
00048:                 Assert.Contains($"{entry.Store} v{entry.CurrentVersion}", report);
00049:             }
00050:         }
00051:
00052:         [Fact]
00053:         public void SaveSchemaVersions_ListsEveryVersionedSaveCodec()
00054:         {
00055:             // The report must cover every versioned save codec constant in
00056:             // Core. When a new codec with CurrentSaveVersion ships, add it
00057:             // here (and to VersionReport.SaveSchemaVersions).
00058:             var stores = VersionReport.SaveSchemaVersions.Select(s => s.Store).OrderBy(s => s).ToArray();
00059:
00060:             Assert.Equal(
00061:                 new[] { "dose_ledger", "expansion_hub", "expansion_quest", "holdfast", "weight_of_choices", "year_of_ash" },
00062:                 stores);
00063:         }
00064:
00065:         [Fact]
00066:         public void SaveSchemaVersions_MatchTheActualCodecConstants()
00067:         {
00068:             // Values are compiled from the codec constants themselves; this
00069:             // pins the mapping so a renamed/moved constant breaks loudly
00070:             // instead of silently reporting a stale number.
00071:             Assert.Contains(VersionReport.SaveSchemaVersions,
00072:                 s => s.Store == "holdfast" && s.CurrentVersion == HoldfastSave.CurrentSaveVersion);
00073:             Assert.Contains(VersionReport.SaveSchemaVersions,
00074:                 s => s.Store == "year_of_ash" && s.CurrentVersion == YearOfAsh.YearOfAshSave.CurrentSaveVersion);
00075:             Assert.Contains(VersionReport.SaveSchemaVersions,
00076:                 s => s.Store == "dose_ledger" && s.CurrentVersion == DoseLedgerSave.CurrentSaveVersion);
00077:             Assert.Contains(VersionReport.SaveSchemaVersions,
00078:                 s => s.Store == "expansion_hub" && s.CurrentVersion == ExpansionHubSave.CurrentSaveVersion);
00079:             Assert.Contains(VersionReport.SaveSchemaVersions,
00080:                 s => s.Store == "expansion_quest" && s.CurrentVersion == ExpansionQuestSaveEnvelope.CurrentVersion);
00081:         }
00082:
00083:         // ── Data schema scanner contract ─────────────────────────────────
00084:
00085:         [Fact]
00086:         public void ScanDataSchemas_TalliesVersionsAndMissingDeclarations()
00087:         {
00088:             string dir = Path.Combine(Path.GetTempPath(), "ashfall-version-tests",
00089:                 Guid.NewGuid().ToString("N"));
00090:             Directory.CreateDirectory(dir);
00091:             try
00092:             {
00093:                 File.WriteAllText(Path.Combine(dir, "a.json"), "{\"schema_version\": 1, \"items\": []}");
00094:                 File.WriteAllText(Path.Combine(dir, "b.json"), "{\"schema_version\": 2, \"items\": []}");
00095:                 File.WriteAllText(Path.Combine(dir, "c.json"), "{\"items\": []}");
00096:                 // Nested schema_version must lose to a top-level declaration.
00097:                 File.WriteAllText(Path.Combine(dir, "d.json"),
00098:                     "{\"schema_version\": 1, \"items\": [{\"schema_version\": 9}]}");
00099:                 // Unreadable/unparseable JSON still counts as a catalog
00100:                 // (inventory semantics); it simply has no schema_version → v0.
00101:                 File.WriteAllText(Path.Combine(dir, "broken.json"), "{not json");
00102:                 Directory.CreateDirectory(Path.Combine(dir, "sub"));
00103:                 File.WriteAllText(Path.Combine(dir, "sub", "e.json"), "{\"schema_version\": 2}");
00104:
00105:                 var summary = VersionReport.ScanDataSchemas(dir);
00106:
00107:                 Assert.Equal(6, summary.Catalogs);          // inventory: every readable *.json
00108:                 Assert.Equal(4, summary.WithSchemaVersion); // c.json & broken.json declare none
00109:                 Assert.Equal(2, summary.WithoutSchemaVersion);
00110:                 Assert.Equal(2, summary.MaxVersion);
00111:                 Assert.Equal(new[] { (0, 2), (1, 2), (2, 2) },
00112:                     summary.Distribution.Select(d => (d.Version, d.Files)).ToArray());
00113:             }
00114:             finally
00115:             {
00116:                 Directory.Delete(dir, recursive: true);
00117:             }
00118:         }
00119:
00120:         [Fact]
00121:         public void ScanDataSchemas_MissingDirectory_ReturnsEmptySummary()
00122:         {
00123:             var summary = VersionReport.ScanDataSchemas(Path.Combine(Path.GetTempPath(), "definitely-not-here"));
00124:             Assert.Equal(0, summary.Catalogs);
00125:             Assert.Equal(0, summary.WithSchemaVersion);
00126:             Assert.Equal(0, summary.MaxVersion);
00127:             Assert.Empty(summary.Distribution);
00128:             Assert.Equal("no data directory", VersionReport.FormatDataSchemas(summary));
00129:         }
00130:
00131:         [Fact]
00132:         public void FormatDataSchemas_RendersDistributionWithMax()
00133:         {
00134:             var summary = new VersionReport.DataSchemaSummary
00135:             {
00136:                 Catalogs = 3,
00137:                 MaxVersion = 2,
00138:                 Distribution = { (1, 1), (2, 2) }
00139:             };
00140:             Assert.Equal("3 catalogs — v1: 1, v2: 2 (max v2)", VersionReport.FormatDataSchemas(summary));
00141:         }
00142:
00143:         // ── Live data authority pin ──────────────────────────────────────
00144:
00145:         [Fact]
00146:         public void ScanDataSchemas_LiveDataAuthority_IsNonTrivialAndCurrent()
00147:         {
00148:             string dataDir = Path.Combine(FindRepoRoot(), "Assets", "StreamingAssets", "Data");
00149:             var summary = VersionReport.ScanDataSchemas(dataDir);
00150:
00151:             // The data authority is large and overwhelmingly versioned; both
00152:             // facts must hold or the report is scanning the wrong place.
00153:             Assert.True(summary.Catalogs >= 100,
00154:                 $"Expected at least 100 JSON catalogs under the data authority, found {summary.Catalogs}");
00155:             Assert.True(summary.WithSchemaVersion >= 100,
00156:                 $"Expected at least 100 catalogs with schema_version, found {summary.WithSchemaVersion}");
00157:             Assert.True(summary.MaxVersion >= 1, "Expected at least schema_version 1 in the data authority");
00158:         }
00159:
00160:         // ── Persistence format inventory contract ─────────────────────────
00161:
00162:         [Fact]
00163:         public void AllPersistenceFormats_CoversAllSaveSectionRegistrySections()
00164:         {
00165:             var registryKeys = global::Ashfall.Core.Save.SaveSectionRegistry.All.Select(s => s.SectionKey).OrderBy(k => k).ToArray();
00166:             var inventoryKeys = VersionReport.AllPersistenceFormats.Select(f => f.SectionKey).OrderBy(k => k).ToArray();
00167:
00168:             Assert.Equal(registryKeys, inventoryKeys);
00169:         }
00170:
00171:         [Fact]
00172:         public void AllPersistenceFormats_DistinguishesVersionedCodecsAndChecksumEnvelopes()
00173:         {
00174:             var versioned = VersionReport.AllPersistenceFormats.Where(f => f.Kind == SavePersistenceKind.VersionedCodec).ToList();
00175:             var envelopes = VersionReport.AllPersistenceFormats.Where(f => f.Kind == SavePersistenceKind.ChecksumEnvelope).ToList();
00176:
00177:             // 6 versioned Core codecs
00178:             Assert.Equal(6, versioned.Count);
00179:             Assert.Contains(versioned, f => f.SectionKey == "holdfast" && f.Version == HoldfastSave.CurrentSaveVersion);
00180:             Assert.Contains(versioned, f => f.SectionKey == "year_of_ash" && f.Version == YearOfAsh.YearOfAshSave.CurrentSaveVersion);
00181:             Assert.Contains(versioned, f => f.SectionKey == "dose_ledger" && f.Version == DoseLedgerSave.CurrentSaveVersion);
00182:             Assert.Contains(versioned, f => f.SectionKey == "expansion_hub" && f.Version == ExpansionHubSave.CurrentSaveVersion);
00183:             Assert.Contains(versioned, f => f.SectionKey == "expansion_quest" && f.Version == ExpansionQuestSaveEnvelope.CurrentVersion);
00184:             Assert.Contains(versioned, f => f.SectionKey == "weight_of_choices" && f.Version == Ashfall.Core.Factions.WeightOfChoicesSave.CurrentSaveVersion);
00185:
00186:             // 260 unversioned checksum envelopes, including the Plans 130–133
00187:             // state sections (powder_metallurgy, nvis_communications, lyophilization, draisine_recovery),
00188:             // Tasks 5–8 state sections (weather_hardening, geothermal_aquifer, counter_intelligence, recon_telemetry),
00189:             // Plans 146–149 state sections (route_infrastructure, ebpvd_coating, microfluidic_diagnostic, mine_clearing_flail, rail_grinding),
00190:             // Plans 166–169 state sections (espionage, fluid_logistics, procedural_narrative),
00191:             // Plans 62–65 state sections (food_preservation, prewar_archives, shelter_prisoners),
00192:             // Plans 50–53 state sections (vehicle_garage, faction_espionage, survivor_mental_health),
00193:             // Plans B68–B69 state sections (seismic_dynamics, cryo_vault),
00194:             // Plans B86–B89 state sections (precision_metrology, aquaponics; B88 nests under radio),
00195:             // and the muster-warfare / plans-74-77 state sections from the concurrent flagship streams.
00196:             // Plan 155 added oral_lore.
00197:             // Plan 157 added grain_milling_archive.
00198:             // Plan 159 added leatherwork_archive.
00199:             // Plan 143 added narrative_questlines.
00200:             // B5–B8 Phase 6 and subsequent registered sections are included;
00201:             // VersionReport is the authority for the current count.
00202:             Assert.Equal(260, envelopes.Count);
00203:             foreach (var envelope in envelopes)
00204:             {
00205:                 Assert.Null(envelope.Version);
00206:                 Assert.Contains("envelope", envelope.FormatDescription);
00207:             }
00208:         }
00209:
00210:         [Fact]
00211:         public void FormatPersistenceInventory_RendersSummaryAndEntries()
00212:         {
00213:             string inventory = VersionReport.FormatPersistenceInventory();
00214:
00215:             Assert.Contains("Save Persistence Inventory (266 sections: 6 versioned codecs, 260 checksum envelopes):", inventory);
00216:             Assert.Contains("holdfast", inventory);
00217:             Assert.Contains("dose_ledger", inventory);
00218:             Assert.Contains("journal", inventory);
00219:             Assert.Contains("survivors", inventory);
00220:             Assert.Contains("weight_of_choices", inventory);
00221:         }
00222:     }
00223: }
```

## `Ashfall.Core.Tests/Expansions/Plan18ExpansionDeepeningTests.cs` — 239 lines; 11,633 bytes; SHA-256 `ba5424ad1e2499c320f9e7573a6f3c89715aa262e90d739658abfae4f4c4a697`
Declaration index:
- 00014: public sealed class Plan18ExpansionDeepeningTests
- 00016: private static string GetDataDir()
- 00026: public void Holdfast_24Quests_WithIceRoadCensusAndBrineMechanics_LoadsCleanly()
- 00049: public void StandingRecord_52Memories_22Quests_14Layouts_ReconstructsAndMutates()
- 00077: public void Crossing_20Quests_14Encounters_ArbitrationCasesAndCrises_Resolves()
- 00103: public void Verdict_16Questlines_9Npcs_AuthenticationsAndAppeals_Integrates()
- 00134: public void QuestlineMaster_ContainsAllAuthoritativeQuestIds()
- 00171: public void SaveRoundtrip_HoldfastAndStandingRecord_PreservesDeterministicState()
- 00204: private sealed class VerdictQuestlinesDto
- 00210: private sealed class VerdictQuestlineEntryDto
- 00216: private sealed class VerdictNpcsDto
- 00222: private sealed class VerdictNpcEntryDto
- 00228: private sealed class QuestlineMasterDto
- 00234: private sealed class QuestlineMasterEntryDto
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Crossing;
00008: using Ashfall.Core.IO;
00009: using Ashfall.Core.Narrative;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.Expansions
00013: {
00014:     public sealed class Plan18ExpansionDeepeningTests
00015:     {
00016:         private static string GetDataDir()
00017:         {
00018:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out var dir))
00019:                 return dir;
00020:             string probe = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data"));
00021:             if (Directory.Exists(probe)) return probe;
00022:             throw new DirectoryNotFoundException("StreamingAssets/Data not found");
00023:         }
00024:
00025:         [Fact]
00026:         public void Holdfast_24Quests_WithIceRoadCensusAndBrineMechanics_LoadsCleanly()
00027:         {
00028:             string dataDir = GetDataDir();
00029:             var files = new FileSystemIO();
00030:             var json = new SystemTextJsonSerializer();
00031:             var catalog = new HoldfastCatalogLoader(files, json, NullLog.Instance).Load(dataDir);
00032:
00033:             Assert.NotNull(catalog);
00034:             Assert.True(catalog.Quests.Count >= 22, $"Expected at least 22 quests, got {catalog.Quests.Count}");
00035:             Assert.True(catalog.Locations.Count >= 30, $"Expected at least 30 locations, got {catalog.Locations.Count}");
00036:
00037:             // Specific signature quests
00038:             Assert.NotNull(catalog.GetQuest("quest_holdfast_salt_convoy_haul"));
00039:             Assert.NotNull(catalog.GetQuest("quest_holdfast_scree_blockage_clear"));
00040:             Assert.NotNull(catalog.GetQuest("quest_holdfast_census_claimant_audit"));
00041:             Assert.NotNull(catalog.GetQuest("quest_holdfast_census_forged_voucher"));
00042:             Assert.NotNull(catalog.GetQuest("quest_holdfast_brine_boiler_scum"));
00043:             Assert.NotNull(catalog.GetQuest("quest_holdfast_salter_work_stoppage"));
00044:             Assert.NotNull(catalog.GetQuest("quest_holdfast_boiler_crack_panic"));
00045:             Assert.NotNull(catalog.GetQuest("quest_holdfast_ration_lockup_breach"));
00046:         }
00047:
00048:         [Fact]
00049:         public void StandingRecord_52Memories_22Quests_14Layouts_ReconstructsAndMutates()
00050:         {
00051:             string dataDir = GetDataDir();
00052:             var files = new FileSystemIO();
00053:             var json = new SystemTextJsonSerializer();
00054:
00055:             // Quests
00056:             var srCat = new StandingRecordCatalogLoader(files, json, NullLog.Instance).Load(dataDir);
00057:             Assert.True(srCat.Quests.Count >= 22, $"Expected at least 22 quests, got {srCat.Quests.Count}");
00058:             Assert.NotNull(srCat.GetQuest("quest_record_vault_breach_forensics"));
00059:             Assert.NotNull(srCat.GetQuest("quest_record_metro_derailment_triage"));
00060:             Assert.NotNull(srCat.GetQuest("quest_record_mine_shaft_adit_collapse"));
00061:             Assert.NotNull(srCat.GetQuest("quest_record_archive_burn_layer"));
00062:             Assert.NotNull(srCat.GetQuest("quest_record_the_unmarked_plaque"));
00063:             Assert.NotNull(srCat.GetQuest("quest_record_the_last_watch_beacon"));
00064:
00065:             // Memories
00066:             var memSys = new LocationMemorySystem(files, json, NullLog.Instance);
00067:             memSys.Load(dataDir);
00068:             Assert.True(memSys.StratumCount >= 50, $"Expected at least 50 memories, got {memSys.StratumCount}");
00069:
00070:             // Layouts
00071:             var layoutSys = new LocationLayoutSystem(files, json, NullLog.Instance);
00072:             layoutSys.Load(dataDir);
00073:             Assert.True(layoutSys.LayoutCount >= 14, $"Expected at least 14 layouts, got {layoutSys.LayoutCount}");
00074:         }
00075:
00076:         [Fact]
00077:         public void Crossing_20Quests_14Encounters_ArbitrationCasesAndCrises_Resolves()
00078:         {
00079:             string dataDir = GetDataDir();
00080:             var session = CrossingSession.Load(dataDir, NullLog.Instance);
00081:
00082:             Assert.NotNull(session);
00083:             Assert.NotNull(session.Catalog);
00084:             Assert.True(session.Catalog.Quests.Count >= 20, $"Expected at least 20 quests, got {session.Catalog.Quests.Count}");
00085:             Assert.True(session.Catalog.Encounters.Count >= 14, $"Expected at least 14 encounters, got {session.Catalog.Encounters.Count}");
00086:
00087:             // Specific signature quests
00088:             Assert.NotNull(session.Catalog.GetQuest("quest_crossing_asylum_in_the_truss"));
00089:             Assert.NotNull(session.Catalog.GetQuest("quest_crossing_contraband_medical_vial"));
00090:             Assert.NotNull(session.Catalog.GetQuest("quest_crossing_vehicle_lien_arbitration"));
00091:             Assert.NotNull(session.Catalog.GetQuest("quest_crossing_displaced_kin_roll"));
00092:             Assert.NotNull(session.Catalog.GetQuest("quest_crossing_quarantine_breach_trial"));
00093:             Assert.NotNull(session.Catalog.GetQuest("quest_crossing_the_null_charter_vote"));
00094:
00095:             // Crisis encounters
00096:             Assert.NotNull(session.Catalog.GetEncounter("enc_nc_mass_crossing_surge"));
00097:             Assert.NotNull(session.Catalog.GetEncounter("enc_nc_garrison_iron_blockade"));
00098:             Assert.NotNull(session.Catalog.GetEncounter("enc_nc_pestilence_quarantine_lockdown"));
00099:             Assert.NotNull(session.Catalog.GetEncounter("enc_nc_syndicate_bribe_overture"));
00100:         }
00101:
00102:         [Fact]
00103:         public void Verdict_16Questlines_9Npcs_AuthenticationsAndAppeals_Integrates()
00104:         {
00105:             string dataDir = GetDataDir();
00106:             var files = new FileSystemIO();
00107:             var json = new SystemTextJsonSerializer();
00108:
00109:             // Questlines
00110:             string qlPath = Path.Combine(dataDir, "verdict_questlines.json");
00111:             var qlDoc = json.Deserialize<VerdictQuestlinesDto>(files.ReadAllText(qlPath));
00112:             Assert.NotNull(qlDoc);
00113:             Assert.NotNull(qlDoc.quests);
00114:             Assert.True(qlDoc.quests.Count >= 16, $"Expected at least 16 questlines, got {qlDoc.quests.Count}");
00115:             Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_alibi_verification");
00116:             Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_witness_subpoena");
00117:             Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_charter_authentication");
00118:             Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_prior_verdict_appeal");
00119:             Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_chain_of_custody");
00120:             Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_machine_interpretation_contest");
00121:
00122:             // NPCs
00123:             string npcPath = Path.Combine(dataDir, "verdict_npcs.json");
00124:             var npcDoc = json.Deserialize<VerdictNpcsDto>(files.ReadAllText(npcPath));
00125:             Assert.NotNull(npcDoc);
00126:             Assert.NotNull(npcDoc.items);
00127:             Assert.True(npcDoc.items.Count >= 9, $"Expected at least 9 NPCs, got {npcDoc.items.Count}");
00128:             Assert.Contains(npcDoc.items, n => n.id == "npc_tomas_reid");
00129:             Assert.Contains(npcDoc.items, n => n.id == "npc_elena_vane");
00130:             Assert.Contains(npcDoc.items, n => n.id == "npc_kasper_holt");
00131:         }
00132:
00133:         [Fact]
00134:         public void QuestlineMaster_ContainsAllAuthoritativeQuestIds()
00135:         {
00136:             string dataDir = GetDataDir();
00137:             var files = new FileSystemIO();
00138:             var json = new SystemTextJsonSerializer();
00139:
00140:             string masterPath = Path.Combine(dataDir, "questline_master.json");
00141:             var master = json.Deserialize<QuestlineMasterDto>(files.ReadAllText(masterPath));
00142:             Assert.NotNull(master);
00143:             Assert.NotNull(master.entries);
00144:             Assert.True(master.entries.Count >= 400, $"Expected at least 400 questline master entries, got {master.entries.Count}");
00145:
00146:             var masterSet = new HashSet<string>(master.entries.Select(e => e.id), StringComparer.Ordinal);
00147:
00148:             // Verify Holdfast quests exist in master
00149:             var hf = new HoldfastCatalogLoader(files, json, NullLog.Instance).Load(dataDir);
00150:             foreach (var q in hf.Quests)
00151:             {
00152:                 Assert.Contains(q.id, masterSet);
00153:             }
00154:
00155:             // Verify Standing Record quests exist in master
00156:             var sr = new StandingRecordCatalogLoader(files, json, NullLog.Instance).Load(dataDir);
00157:             foreach (var q in sr.Quests)
00158:             {
00159:                 Assert.Contains(q.id, masterSet);
00160:             }
00161:
00162:             // Verify Crossing quests exist in master
00163:             var cr = CrossingSession.Load(dataDir, NullLog.Instance);
00164:             foreach (var q in cr.Catalog.Quests)
00165:             {
00166:                 Assert.Contains(q.id, masterSet);
00167:             }
00168:         }
00169:
00170:         [Fact]
00171:         public void SaveRoundtrip_HoldfastAndStandingRecord_PreservesDeterministicState()
00172:         {
00173:             string dataDir = GetDataDir();
00174:             var files = new FileSystemIO();
00175:             var json = new SystemTextJsonSerializer();
00176:
00177:             // Holdfast save roundtrip
00178:             var session = HoldfastSession.Load(dataDir, 808, expansionUnlocked: true, NullLog.Instance);
00179:             session.Quests.TryStart("quest_holdfast_the_sheet", 90);
00180:             session.Quests.TryStart("quest_holdfast_salt_convoy_haul", 95);
00181:             var save = HoldfastSaveCodec.Capture(session.IceRoad, session.Census, session.Brine, session.Quests, new SimClock(95));
00182:             string encoded = HoldfastSaveCodec.Encode(save, json);
00183:             var loaded = HoldfastSaveCodec.Decode(encoded, json);
00184:             Assert.NotNull(loaded);
00185:             Assert.Contains("quest_holdfast_salt_convoy_haul", loaded.quests.quests.Select(q => q.questId));
00186:
00187:             // Standing Record layout save roundtrip
00188:             var layoutSys = new LocationLayoutSystem(files, json, NullLog.Instance);
00189:             layoutSys.Load(dataDir);
00190:             layoutSys.Unlock();
00191:             layoutSys.ArriveAtParent(LocationLayoutSystem.LocKilometre19);
00192:             layoutSys.EnterRoom(LocationLayoutSystem.RoomKm19Post);
00193:             layoutSys.InspectRoom(LocationLayoutSystem.RoomKm19Post);
00194:
00195:             var srState = layoutSys.CaptureState();
00196:             string serializedSr = json.Serialize(srState);
00197:             var restoredSr = json.Deserialize<LocationLayoutState>(serializedSr);
00198:             Assert.NotNull(restoredSr);
00199:             var parent = restoredSr.parents.FirstOrDefault(p => p.parentLocationId == LocationLayoutSystem.LocKilometre19);
00200:             Assert.NotNull(parent);
00201:             Assert.Contains(LocationLayoutSystem.RoomKm19Post, parent.inspectedRoomIds);
00202:         }
00203:
00204:         private sealed class VerdictQuestlinesDto
00205:         {
00206:             public int schema_version { get; set; }
00207:             public List<VerdictQuestlineEntryDto>? quests { get; set; }
00208:         }
00209:
00210:         private sealed class VerdictQuestlineEntryDto
00211:         {
00212:             public string questlineId { get; set; } = string.Empty;
00213:             public string title { get; set; } = string.Empty;
00214:         }
00215:
00216:         private sealed class VerdictNpcsDto
00217:         {
00218:             public int schema_version { get; set; }
00219:             public List<VerdictNpcEntryDto>? items { get; set; }
00220:         }
00221:
00222:         private sealed class VerdictNpcEntryDto
00223:         {
00224:             public string id { get; set; } = string.Empty;
00225:             public string name { get; set; } = string.Empty;
00226:         }
00227:
00228:         private sealed class QuestlineMasterDto
00229:         {
00230:             public int schema_version { get; set; }
00231:             public List<QuestlineMasterEntryDto>? entries { get; set; }
00232:         }
00233:
00234:         private sealed class QuestlineMasterEntryDto
00235:         {
00236:             public string id { get; set; } = string.Empty;
00237:         }
00238:     }
00239: }
```

## `Ashfall.Core.Tests/Save/SaveSupportWindowTests.cs` — 300 lines; 11,812 bytes; SHA-256 `e39db24332b012d270cf0307aefdac772ab680f9f84931f41e2a041edb9f2393`
Declaration index:
- 00022: public sealed class SaveSupportWindowTests
- 00029: private static string FindRepoRoot()
- 00060: public void CuratedCodecs_ArePresent_WithExpectedCount()
- 00069: public void Holdfast_SchemaVersion_IsAtExpectedValue()
- 00077: public void YearOfAsh_SchemaVersion_IsAtExpectedValue()
- 00085: public void DoseLedger_SchemaVersion_IsAtExpectedValue()
- 00093: public void ExpansionHub_SchemaVersion_IsAtExpectedValue()
- 00101: public void ExpansionQuest_SchemaVersion_IsAtExpectedValue()
- 00109: public void WeightOfChoices_SchemaVersion_IsAtExpectedValue()
- 00117: public void AllCuratedCodecs_HavePositiveSchemaVersions()
- 00127: public void AllCuratedCodecs_HaveNonEmptyStoreNames()
- 00137: public void AllCuratedCodecs_HaveUniqueStoreNames()
- 00154: public void HistoricalFixtureManifest_ExistsAndIsValidJson()
- 00172: public void HistoricalFixtureManifest_HasAtLeastOneFixtureEntry()
- 00187: public void HistoricalFixtures_AllReferencedFilesExist()
- 00213: public void HistoricalFixtures_AllEntriesHaveGameVersionAndSchemaMap()
- 00247: public void CurrentCodecVersions_NeverRegressBelowHistoricalMinimum()
- 00291: private static VersionReport.SaveSchemaEntry FindEntry(string store)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using Ashfall.Core;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests.Save
00011: {
00012:     /// <summary>
00013:     /// Plan 48 / C2[21] Phase 3 — Save Support Window Tests.
00014:     ///
00015:     /// Pins the curated set of versioned save codecs and their current schema
00016:     /// versions against the values declared in VersionReport.SaveSchemaVersions.
00017:     /// Also validates the historical fixture corpus manifest for structural
00018:     /// well-formedness and backward-compatibility invariants.
00019:     ///
00020:     /// Gate:  save_support_window (full tier — registered in Phase 3)
00021:     /// </summary>
00022:     public sealed class SaveSupportWindowTests
00023:     {
00024:         // ---------------------------------------------------------------
00025:         // Repo resolution helper (shared pattern with other test files)
00026:         // ---------------------------------------------------------------
00027:         private static readonly string RepoRoot = FindRepoRoot();
00028:
00029:         private static string FindRepoRoot()
00030:         {
00031:             string dir = AppContext.BaseDirectory;
00032:             for (int i = 0; i < 10; i++)
00033:             {
00034:                 if (File.Exists(Path.Combine(dir, "project.godot")))
00035:                     return dir;
00036:                 var parent = Directory.GetParent(dir);
00037:                 if (parent == null) break;
00038:                 dir = parent.FullName;
00039:             }
00040:             return AppContext.BaseDirectory;
00041:         }
00042:
00043:         private static string HistoricalFixturesDir =>
00044:             Path.Combine(RepoRoot, "artifacts", "golden_saves", "historical");
00045:
00046:         private static string HistoricalManifestPath =>
00047:             Path.Combine(HistoricalFixturesDir, "manifest.json");
00048:
00049:         // ---------------------------------------------------------------
00050:         // 1. Schema version pins — curated live constants
00051:         // ---------------------------------------------------------------
00052:
00053:         /// <summary>
00054:         /// Verifies that VersionReport.SaveSchemaVersions contains exactly the
00055:         /// six curated versioned codecs at the expected versions for game v1.1.0.
00056:         /// If a codec schema is intentionally bumped, update the expected value
00057:         /// here alongside updating the CHANGELOG.md entry.
00058:         /// </summary>
00059:         [Fact]
00060:         public void CuratedCodecs_ArePresent_WithExpectedCount()
00061:         {
00062:             var entries = VersionReport.SaveSchemaVersions;
00063:             Assert.NotNull(entries);
00064:             // Six curated versioned codecs as of game v1.1.0
00065:             Assert.Equal(6, entries.Length);
00066:         }
00067:
00068:         [Fact]
00069:         public void Holdfast_SchemaVersion_IsAtExpectedValue()
00070:         {
00071:             var entry = FindEntry("holdfast");
00072:             // holdfast v5 is the pinned value for game v1.1.0
00073:             Assert.Equal(5, entry.CurrentVersion);
00074:         }
00075:
00076:         [Fact]
00077:         public void YearOfAsh_SchemaVersion_IsAtExpectedValue()
00078:         {
00079:             var entry = FindEntry("year_of_ash");
00080:             // year_of_ash v5 is the pinned value for game v1.1.0
00081:             Assert.Equal(5, entry.CurrentVersion);
00082:         }
00083:
00084:         [Fact]
00085:         public void DoseLedger_SchemaVersion_IsAtExpectedValue()
00086:         {
00087:             var entry = FindEntry("dose_ledger");
00088:             // dose_ledger v2 is the pinned value for game v1.1.0
00089:             Assert.Equal(2, entry.CurrentVersion);
00090:         }
00091:
00092:         [Fact]
00093:         public void ExpansionHub_SchemaVersion_IsAtExpectedValue()
00094:         {
00095:             var entry = FindEntry("expansion_hub");
00096:             // expansion_hub v6 is the pinned value for game v1.1.0
00097:             Assert.Equal(6, entry.CurrentVersion);
00098:         }
00099:
00100:         [Fact]
00101:         public void ExpansionQuest_SchemaVersion_IsAtExpectedValue()
00102:         {
00103:             var entry = FindEntry("expansion_quest");
00104:             // expansion_quest v1 is the pinned value for game v1.1.0
00105:             Assert.Equal(1, entry.CurrentVersion);
00106:         }
00107:
00108:         [Fact]
00109:         public void WeightOfChoices_SchemaVersion_IsAtExpectedValue()
00110:         {
00111:             var entry = FindEntry("weight_of_choices");
00112:             // weight_of_choices v2 is the pinned value for game v1.1.0
00113:             Assert.Equal(2, entry.CurrentVersion);
00114:         }
00115:
00116:         [Fact]
00117:         public void AllCuratedCodecs_HavePositiveSchemaVersions()
00118:         {
00119:             foreach (var entry in VersionReport.SaveSchemaVersions)
00120:             {
00121:                 Assert.True(entry.CurrentVersion >= 1,
00122:                     $"Codec '{entry.Store}' has unexpected schema version {entry.CurrentVersion} (must be >= 1)");
00123:             }
00124:         }
00125:
00126:         [Fact]
00127:         public void AllCuratedCodecs_HaveNonEmptyStoreNames()
00128:         {
00129:             foreach (var entry in VersionReport.SaveSchemaVersions)
00130:             {
00131:                 Assert.False(string.IsNullOrWhiteSpace(entry.Store),
00132:                     "A SaveSchemaEntry has a blank Store name");
00133:             }
00134:         }
00135:
00136:         [Fact]
00137:         public void AllCuratedCodecs_HaveUniqueStoreNames()
00138:         {
00139:             var names = VersionReport.SaveSchemaVersions.Select(e => e.Store).ToArray();
00140:             var distinct = names.Distinct(StringComparer.Ordinal).ToArray();
00141:             Assert.Equal(names.Length, distinct.Length);
00142:         }
00143:
00144:         // ---------------------------------------------------------------
00145:         // 2. Historical fixture corpus — manifest structural validation
00146:         // ---------------------------------------------------------------
00147:
00148:         /// <summary>
00149:         /// The historical fixture corpus directory must exist once Phase 3 runs.
00150:         /// The manifest at artifacts/golden_saves/historical/manifest.json must
00151:         /// be valid JSON and have the expected schema_version field.
00152:         /// </summary>
00153:         [Fact]
00154:         public void HistoricalFixtureManifest_ExistsAndIsValidJson()
00155:         {
00156:             if (!File.Exists(HistoricalManifestPath))
00157:             {
00158:                 // Phase 3: corpus not yet created — skip gracefully with a skip signal
00159:                 return;
00160:             }
00161:
00162:             var json = File.ReadAllText(HistoricalManifestPath);
00163:             using var doc = JsonDocument.Parse(json);
00164:
00165:             Assert.True(doc.RootElement.TryGetProperty("schema_version", out var schemaVer),
00166:                 "historical/manifest.json must have 'schema_version'");
00167:             Assert.True(schemaVer.GetInt32() >= 1,
00168:                 "historical/manifest.json schema_version must be >= 1");
00169:         }
00170:
00171:         [Fact]
00172:         public void HistoricalFixtureManifest_HasAtLeastOneFixtureEntry()
00173:         {
00174:             if (!File.Exists(HistoricalManifestPath))
00175:                 return; // corpus not yet created
00176:
00177:             var json = File.ReadAllText(HistoricalManifestPath);
00178:             using var doc = JsonDocument.Parse(json);
00179:
00180:             Assert.True(doc.RootElement.TryGetProperty("fixtures", out var fixtures),
00181:                 "historical/manifest.json must have 'fixtures' array");
00182:             Assert.True(fixtures.GetArrayLength() >= 1,
00183:                 "historical/manifest.json must have at least one fixture entry");
00184:         }
00185:
00186:         [Fact]
00187:         public void HistoricalFixtures_AllReferencedFilesExist()
00188:         {
00189:             if (!File.Exists(HistoricalManifestPath))
00190:                 return; // corpus not yet created
00191:
00192:             var json = File.ReadAllText(HistoricalManifestPath);
00193:             using var doc = JsonDocument.Parse(json);
00194:
00195:             if (!doc.RootElement.TryGetProperty("fixtures", out var fixtures))
00196:                 return;
00197:
00198:             var missing = new List<string>();
00199:             foreach (var fx in fixtures.EnumerateArray())
00200:             {
00201:                 if (!fx.TryGetProperty("fixture_name", out var nameEl))
00202:                     continue;
00203:                 string name = nameEl.GetString() ?? "";
00204:                 string fullPath = Path.Combine(HistoricalFixturesDir, name);
00205:                 if (!File.Exists(fullPath))
00206:                     missing.Add(name);
00207:             }
00208:
00209:             Assert.Empty(missing); // fails listing any missing file names
00210:         }
00211:
00212:         [Fact]
00213:         public void HistoricalFixtures_AllEntriesHaveGameVersionAndSchemaMap()
00214:         {
00215:             if (!File.Exists(HistoricalManifestPath))
00216:                 return; // corpus not yet created
00217:
00218:             var json = File.ReadAllText(HistoricalManifestPath);
00219:             using var doc = JsonDocument.Parse(json);
00220:
00221:             if (!doc.RootElement.TryGetProperty("fixtures", out var fixtures))
00222:                 return;
00223:
00224:             var issues = new List<string>();
00225:             foreach (var fx in fixtures.EnumerateArray())
00226:             {
00227:                 string name = fx.TryGetProperty("fixture_name", out var n) ? n.GetString() ?? "?" : "?";
00228:                 if (!fx.TryGetProperty("game_version", out _))
00229:                     issues.Add($"{name}: missing 'game_version'");
00230:                 if (!fx.TryGetProperty("schema_map", out _))
00231:                     issues.Add($"{name}: missing 'schema_map'");
00232:             }
00233:
00234:             Assert.Empty(issues);
00235:         }
00236:
00237:         // ---------------------------------------------------------------
00238:         // 3. Support window invariant — no version regression
00239:         // ---------------------------------------------------------------
00240:
00241:         /// <summary>
00242:         /// Verifies that each curated codec's current schema version is >= the
00243:         /// minimum version recorded in the historical corpus.  This ensures the
00244:         /// runtime is not accidentally rolled back below what old saves require.
00245:         /// </summary>
00246:         [Fact]
00247:         public void CurrentCodecVersions_NeverRegressBelowHistoricalMinimum()
00248:         {
00249:             if (!File.Exists(HistoricalManifestPath))
00250:                 return; // corpus not yet created
00251:
00252:             var json = File.ReadAllText(HistoricalManifestPath);
00253:             using var doc = JsonDocument.Parse(json);
00254:
00255:             if (!doc.RootElement.TryGetProperty("fixtures", out var fixtures))
00256:                 return;
00257:
00258:             // Collect minimum schema version seen per store across all historical fixtures
00259:             var minVersions = new Dictionary<string, int>(StringComparer.Ordinal);
00260:             foreach (var fx in fixtures.EnumerateArray())
00261:             {
00262:                 if (!fx.TryGetProperty("schema_map", out var schemaMap))
00263:                     continue;
00264:                 foreach (var kvp in schemaMap.EnumerateObject())
00265:                 {
00266:                     int v = kvp.Value.GetInt32();
00267:                     if (!minVersions.TryGetValue(kvp.Name, out int existing) || v < existing)
00268:                         minVersions[kvp.Name] = v;
00269:                 }
00270:             }
00271:
00272:             var regressions = new List<string>();
00273:             foreach (var entry in VersionReport.SaveSchemaVersions)
00274:             {
00275:                 if (minVersions.TryGetValue(entry.Store, out int minSeen))
00276:                 {
00277:                     if (entry.CurrentVersion < minSeen)
00278:                     {
00279:                         regressions.Add(
00280:                             $"Codec '{entry.Store}': current v{entry.CurrentVersion} < historical minimum v{minSeen}");
00281:                     }
00282:                 }
00283:             }
00284:
00285:             Assert.Empty(regressions);
00286:         }
00287:
00288:         // ---------------------------------------------------------------
00289:         // Helper
00290:         // ---------------------------------------------------------------
00291:         private static VersionReport.SaveSchemaEntry FindEntry(string store)
00292:         {
00293:             var entry = VersionReport.SaveSchemaVersions.FirstOrDefault(e =>
00294:                 string.Equals(e.Store, store, StringComparison.Ordinal));
00295:             Assert.True(entry.Store != null,
00296:                 $"Expected codec '{store}' not found in VersionReport.SaveSchemaVersions");
00297:             return entry;
00298:         }
00299:     }
00300: }
```
# Appendix M — External verification handoff

The following checks are to be run by the owning integrator after writing: character count, SHA-256 revalidation, path-token resolution, duplicate-heading/unsupported-claim scan, and `git diff --check`. The final ledger entry must report actual results, not this template.
