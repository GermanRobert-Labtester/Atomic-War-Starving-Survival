# Plan 82 — Verdict Locations: Fifteen-Site Investigation Corpus, Map/Quest Coupling, and Verdict Save Ownership

> **Rebuild status:** TERMINAL 15-SITE CONTENT + REACHABILITY/SAVE AUDIT
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

The historical baseline was 4,999 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Keep the fifteen authored Verdict investigation locations as a coherent fictional evidence corpus while preserving the Verdict loader/save/quest/NPC/radio owners. The historical 4→15 expansion is complete; the next step is a current location-to-map/quest/NPC/radio consumer audit and a truthful trail model, not fifteen disconnected lore blobs or a second map authority.

**Bounded outcome:** Audit `verdict_locations.json`, `VerdictCatalogLoader.LoadLocations`, `VerdictNpcSystem`, quest/radio consumers, `VerdictHostSession`, `Main.Verdict`, Verdict UI, location/map references, and focused tests. Classify live, narrative-only, map-linked, and dormant sites and preserve the existing `verdict` save owner.

**Non-goals:** no second map/location/quest owner, no invented trail field without a consumer, no arbitrary site count, no real-world claims, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `verdict_locations.json` contains 15 locations with the current six-field row shape.
- VERIFIED CURRENT: `VerdictCatalogLoader.LoadLocations` parses location definitions.
- VERIFIED CURRENT: `VerdictHostSession`/VerdictSaveStore own current Verdict state and persistence.
- Current map/expedition reachability of all 15 sites is an explicit audit question.
- HISTORICAL RECORD: Plan 82/Wave 39 records the 4→15 expansion; this package does not claim a fresh test run.

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

- `Assets/StreamingAssets/Data/verdict_locations.json` exists at 14,430 bytes; SHA-256 `e97faf513dfa2e3f6b9cfb7fd1311cea6534a283e7d81e099784232b9d9380d0`.
- `Assets/StreamingAssets/Data/verdict_npcs.json` exists at 9,832 bytes; SHA-256 `ace1edded901844ae68ebbaad7f913990316ab155888ccee2c531766be7cbadc`.
- `Assets/StreamingAssets/Data/verdict_radio.json` exists at 11,606 bytes; SHA-256 `2a8a7faeb9d7a9af175f7188311f1c94892a63604c88932e12d068e70ea555aa`.
- `Assets/StreamingAssets/Data/verdict_questlines.json` exists at 85,668 bytes; SHA-256 `18f04bdcacbc31e6be03358116f0390de2786309d8d0ddbd9770b0aed94dcdc2`.
- `Assets/StreamingAssets/Data/verdict_items.json` exists at 10,345 bytes; SHA-256 `6a4888ad04a244300ecad9c9d2acd3c22d95e084bf0e8991459738fa7a5ae2b8`.
- `Assets/StreamingAssets/Data/locations.json` exists at 112,815 bytes; SHA-256 `97543ade61b6458f5b31bcffb85a96ad5d3b322b80294deb158d1ae29da3386b`.

# 3. Required Delta

Replace the old pure-data brief with a current 15-site catalog/reference/reachability audit. Preserve Verdict evidence/save ownership and canonical map/expedition routing.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Which current map nodes/expedition destinations correspond to the 15 site ids?
Which NPC, quest, radio, and evidence rows reference each site?
Is there a current trail/arc contract, or are trails only prose/quest implications?
Does the Verdict host persist all mutable site-related state without duplicating static rows?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| location rows and parsing | `VerdictCatalogLoader.LoadLocations` | `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` | Static site definitions and bounds. |
| Verdict evidence/quest/radio state | `Verdict systems and VerdictHostSession` | `Assets/Ashfall.Core/Verdict/; src/Host/VerdictHostSession.cs` | Owns current investigation/reckoning state. |
| Verdict persistence | `VerdictSave / VerdictSaveStore` | `Assets/Ashfall.Core/Verdict/VerdictSave.cs; src/Host/VerdictSaveStore.cs` | Existing verdict section/codec owner. |
| wasteland map/expedition reachability | `WastelandMapSystem / ExpeditionSystem` | `Assets/Ashfall.Core/World; Assets/Ashfall.Core/Expeditions/` | Canonical map/travel owners; a site row is not a route. |
| Verdict UI | `VerdictDashboardPanel / VerdictPanel` | `src/UI/VerdictDashboardPanel.cs; src/VerdictPanel.cs` | Current evidence/radio/readout projection. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for Verdict location catalog.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Verdict Locations: Fifteen-Site Investigation Corpus, Map/Quest Coupling, and Verdict Save Ownership                                               │
│ Integration route: DATA-ONLY + current Verdict/map/quest consumer audit                             │
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

1. **Catalog defines sites.**
2. **Verdict owns evidence/state.**
3. **Map/expeditions own reachability.**
4. **Verdict save persists state.**
5. **UI projects current facts.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| location rows and parsing | `VerdictCatalogLoader.LoadLocations` | `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` | Static site definitions and bounds. |
| Verdict evidence/quest/radio state | `Verdict systems and VerdictHostSession` | `Assets/Ashfall.Core/Verdict/; src/Host/VerdictHostSession.cs` | Owns current investigation/reckoning state. |
| Verdict persistence | `VerdictSave / VerdictSaveStore` | `Assets/Ashfall.Core/Verdict/VerdictSave.cs; src/Host/VerdictSaveStore.cs` | Existing verdict section/codec owner. |
| wasteland map/expedition reachability | `WastelandMapSystem / ExpeditionSystem` | `Assets/Ashfall.Core/World; Assets/Ashfall.Core/Expeditions/` | Canonical map/travel owners; a site row is not a route. |
| Verdict UI | `VerdictDashboardPanel / VerdictPanel` | `src/UI/VerdictDashboardPanel.cs; src/VerdictPanel.cs` | Current evidence/radio/readout projection. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load 15 site rows through VerdictCatalogLoader.LoadLocations
2. resolve current site/NPC/quest/radio references
3. project site evidence through VerdictHostSession/Main.Verdict
4. allow map/expedition owners to expose only canonical reachable nodes
5. record evidence/radio/census facts through Verdict owners
6. capture/restore the existing verdict state and present current status

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- location ids are unique and bounded danger/travel/radiation values are valid
- a site is not reachable merely because it has a catalog row
- NPC/quest/radio references resolve to current canonical ids
- evidence is recorded only by the Verdict owner
- map/travel owners control reachability and cost
- restore preserves evidence, fired radio ids, quest state, and census
- site prose cannot introduce a new canon fact without a modelable evidence source

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

Contract rules for Verdict location catalog:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`verdict_locations.json` remains the fifteen-site authority. Current rows have id, displayName, description, dangerLevel, travelHours, and baseRadsPerHour; the current loader does not establish a new trail schema. Audit whether trails are implied by quest/NPC/radio references or need an explicit, separately approved data contract. The original plan’s “three new arcs” must not be invented as fields without a consumer.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

Use the existing `verdict` section and `VerdictSaveStore` for evidence, quest/radio fired state, and census. Static locations remain catalog data. A future trail/reachability state belongs to the canonical map/expedition or Verdict owner, not a second location save.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

Location lookup/order is ordinal; evidence/radio polling and day transitions use the current Verdict owner and seeded streams. A site description/ID must not decide hash iteration or consume RNG. Paired replay compares evidence, fired broadcasts, NPC flags, quest state, and census.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

VerdictCatalogLoader supplies definitions; VerdictHostSession/Verdict systems emit current evidence, radio, census, and quest facts. Map/expedition owners emit travel/reachability facts. A location row cannot itself unlock an ending or create a survivor casualty.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Host/VerdictHostSession.cs` — loads current Verdict content, ticks radio/census, captures/restores
- `src/Host/VerdictSaveStore.cs` — persists existing verdict section
- `src/Main.Verdict.cs` — setup, tick, readout, save, and UI lifecycle
- `src/UI/VerdictDashboardPanel.cs` — current Verdict evidence/radio/census projection
- `src/VerdictPanel.cs` — existing Verdict surface
- `src/Main.PlayerSurfaces.cs` — route/open integration; shared root

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Verdict sites are fictional, grounded, evidence-bearing places. Each description should contain a physical clue, a contradiction or unanswered question, and a modeled danger—not supernatural claims or copied real-world installations. Radio/NPC/quest text must be consistent with what the site owner can reveal.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/Plan82VerdictLocationsExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/VerdictContentWebTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/Plan82_67VerdictCassetteIntegrationTests.cs`

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
| Phase 0 — site/catalog census | read 15 rows, loader, NPC/quest/radio, host, save, and UI | all fields and current references are explicit | no undocumented scope or shortcut |
| Phase 1 — reachability/consumer trace | trace map/expedition/quest/NPC/radio paths | live versus narrative-only sites are classified | no undocumented scope or shortcut |
| Phase 2 — evidence/save/determinism audit | check unlock, fired ids, census, and paired replay | no shadow location/trail authority | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a proven reachability or presentation gap is promoted | one owner and focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/verdict_locations.json` | READ ONLY; MODIFY only for a proven reference/consumer defect | retain as location authority |
| `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` | READ ONLY | location loader |
| `Assets/Ashfall.Core/Verdict/VerdictHostSession.cs` | READ ONLY; host owner | future owner must claim the path before any change |
| `src/Host/VerdictHostSession.cs` | READ ONLY | host/tick/save |
| `src/UI/VerdictDashboardPanel.cs` | READ ONLY | truthful presentation |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| duplicate map/location authority | use WastelandMap/Expedition owners |
| lore without reachability | classify dormant explicitly |
| trail schema invention | require current consumer evidence |
| endgame state drift | use Verdict save/evidence owner |

# 22. Explicit Non-Goals

- no second map/location/quest owner, no invented trail field without a consumer, no arbitrary site count, no real-world claims, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for Verdict location catalog are named from current evidence.
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

- A current 15-site field/reference/reachability matrix.
- A clear decision on whether trails are data fields or derived prose/quest links.
- A bounded residual only for a proven site/consumer/save gap.

## MUST NOT DO

- create a second map or trail authority
- make every site a playable destination automatically
- persist static locations as mutable state
- add a new Verdict save section

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/Plan82VerdictLocationsExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/VerdictContentWebTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/Plan82_67VerdictCassetteIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read verdict_locations.json, VerdictCatalogLoader.LoadLocations, VerdictHostSession, VerdictSave, NPC/quest/radio consumers, map nodes, and focused tests; build the reference matrix.

# Appendix A — Master expansion authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Authority lines: 5,510; bytes: 635,647

The following slices are read-only design constraints. Live source remains higher authority.

### Authority lines 68–73
00068: ### 1.3 What the audit confirmed as stable (no change needed)
00069:
00070: The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.
00071:
00072: ---
00073:

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

## `Assets/StreamingAssets/Data/verdict_items.json`
- Bytes: 10,345; SHA-256: `6a4888ad04a244300ecad9c9d2acd3c22d95e084bf0e8991459738fa7a5ae2b8`
- Root keys: `items, schema_version`
- `items`: list[15]; union fields: `category, description, displayName, downstream_quest_trigger, faction_affinity, id, mechanical_effects, rarity, tier, tradeValue, weightKg`
  - row 1: `{"category":"story_item","description":"Under the Allotments, the array keeps time like a metronome that farms: a tap at ploughing, a tap at harvest, a tap at the well-house door. The machine reads the ground and hears a farm. It has never been to the farm.","displayName":"The Farm's Seismic Signature","downstream_quest_trigger":"quest_verdict_the_warm_range","faction_affinity":"faction_the_tempest","id":"evidence_geophone_hymn","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":12,"weightKg":1.2}`
  - row 2: `{"category":"story_item","description":"Twelve stations, twelve plates, twelve sets of hands in the log. The last entry is Year One. The plates were kept legible after that by a hand that did not log — a hand with a pencil stub and an opinion about the count.","displayName":"The Fired-Plate Ordnance Log","downstream_quest_trigger":"quest_verdict_the_warm_range","faction_affinity":"faction_the_tempest","id":"evidence_twelve_gauge_steel","mechanical_effects":{"enrolled_evidence":1},"rarity":"Uncommon","tier":"Salvaged","tradeValue":18,"weightKg":4.0}`
  - row 3: `{"category":"story_item","description":"The linen is coded to the same alphabet as the cabinets' charters. It is the nearest thing the machine has to a constitution, and it is written in the tense of a department that fully expected to be read.","displayName":"The Standard's Linen","downstream_quest_trigger":"quest_verdict_the_shift_charter","faction_affinity":"faction_the_tempest","id":"evidence_fuse_linen","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":40,"weightKg":0.4}`
  - row 4: `{"category":"story_item","description":"Four-column ledger, soft pencil sharpened with a knife to the last useful inch. The shelter names sit in the second column. The count column is blank, and has been since Year One, when Selya Saltmarsh ran out of households to sight.","displayName":"The Partial County Ledger","downstream_quest_trigger":"quest_verdict_the_reckoning_call","faction_affinity":"faction_the_tempest","id":"evidence_census_draft","mechanical_effects":{"enrolled_evidence":1},"rarity":"Uncommon","tier":"Makeshift","tradeValue":25,"weightKg":0.8}`
  - row 5: `{"category":"story_item","description":"The rota is in triplicate, as the Annex requires. It lists a mailroom, a census clerk, and a sorting route with a footnote in the clerk's hand: persons shall not be counted twice; persons shall not be counted once. The footnote does not say which clause won.","displayName":"Carbon-Copy Censusing Rota","downstream_quest_trigger":"quest_verdict_the_hold","faction_affinity":"faction_the_tempest","id":"evidence_mailroom_tape","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":30,"weightKg":0.3}`
  - row 6: `{"category":"story_item","description":"The register's last entry is five years old. It is unsigned — the Annex does not require a signature, it requires a register — and it reads, in full: held, pending count, re-audit at census interval. The interval came due, and the register does not know it has been due.","displayName":"The Hold Register","downstream_quest_trigger":"quest_verdict_the_hold","faction_affinity":"faction_the_tempest","id":"evidence_uxo_register","mechanical_effects":{"enrolled_evidence":1},"rarity":"Unique","tier":"Old-World","tradeValue":55,"weightKg":1.6}`
  - row 7: `{"category":"story_item","description":"A pure tone, three seconds, looped. It calibrates nothing; it is the alignment tone the array plays before its own voice, and its only trick is that it plays on every sub-band at once, which is how the sector learns — over a single night — that a department is speaking again.","displayName":"The Calibration Burst","downstream_quest_trigger":"quest_verdict_the_reckoning_call","faction_affinity":"faction_the_tempest","id":"evidence_call_calibration","mechanical_effects":{"enrolled_evidence":1},"rarity":"Uncommon","tier":"Old-World","tradeValue":45,"weightKg":0.2}`
  - row 8: `{"category":"story_item","description":"The word census in the machine's register, in plaintext, once, at 03:40, on the hour the old maintenance window opened. The radios still listening received a department memo, five years late, and did not know what to do with it.","displayName":"The Plain Burst","downstream_quest_trigger":"quest_verdict_the_reckoning_call","faction_affinity":"faction_the_tempest","id":"evidence_call_plain","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":50,"weightKg":0.2}`
  - row 9: `{"category":"story_item","description":"The archive keeps a written count of itself: two thousand and sixteen reels, of which one thousand eight hundred and thirty-one matter. The mattering category is the machine's own — a count that cannot be falsified and has never been audited by a human, because nobody has ever asked it what it counted.","displayName":"The Archive's Own Accounting","downstream_quest_trigger":"quest_verdict_the_tape_silo","faction_affinity":"faction_the_tempest","id":"evidence_reels_matter","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":35,"weightKg":2.0}`
  - row 10: `{"category":"story_item","description":"The valve outside the vent shaft reads per 36 — a clause the maintenance file cites to a supervisor's log that contains exactly one entry: shift 36, six names, one missing. The missing hand is the one that has been turning the valve since.","displayName":"The Valve, Read Per 36","downstream_quest_trigger":"quest_verdict_the_shift_charter","faction_affinity":"faction_the_tempest","id":"evidence_valve_s36","mechanical_effects":{"enrolled_evidence":1},"rarity":"Unique","tier":"Salvaged","tradeValue":60,"weightKg":3.0}`
  - row 11: `{"category":"story_item","description":"Eleven months of Eden Vale's broadcasts, logged by the array the way it logs everything that comes down the wire — including the silence after her last transmission, which the machine's register records as no traffic, which is itself a reading.","displayName":"Eleven Months of Tube-Bleed","downstream_quest_trigger":"quest_verdict_eden_grabs","faction_affinity":"faction_the_tempest","id":"evidence_eden_log","mechanical_effects":{"enrolled_evidence":1},"rarity":"Unique","tier":"Salvaged","tradeValue":70,"weightKg":1.1}`
  - row 12: `{"category":"story_item","description":"The count names the shelter's persons, one by one, in the machine's register — fourteen, then fifteen, then the hand that wrote them down. The register does not read itself aloud. Whoever holds the count already knows the names.","displayName":"The Count, Presented","downstream_quest_trigger":"quest_verdict_the_reckoning_call","faction_affinity":"faction_the_tempest","id":"evidence_veen_your_people","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":0,"weightKg":0.5}`
  - row 13: `{"category":"quest_item","description":"A key the size of a hand, brass, with the tape-silo's number cast into the bow. It is worn at only one edge — the edge that fits the lectern's slot — and it is the only key in the fuse world that has a lock still turning freely.","displayName":"The Tape-Silo Key","downstream_quest_trigger":"quest_verdict_the_tape_silo","faction_affinity":"faction_the_tempest","id":"item_archive_tape_silo_key","mechanical_effects":{"special_effect":"rotates the archive log's presentation; permits tape-spin"},"rarity":"Unique","tier":"Old-World","tradeValue":90,"weightKg":0.9}`
  - row 14: `{"category":"quest_item","description":"The Year-One sign-in ledger, open to shift 36: six names, five hands, the sixth left blank, and the completion annotation in the margin — a single check mark, in a hand that has never logged anything else in the county's files.","displayName":"Shift 36's Charter","downstream_quest_trigger":"quest_verdict_the_shift_charter","faction_affinity":"faction_the_tempest","id":"item_fuse_world_shift_charter","mechanical_effects":{"special_effect":"restores the shift's completion to the machine's register"},"rarity":"Unique","tier":"Old-World","tradeValue":75,"weightKg":1.4}`
  - row 15: `{"category":"consumable","description":"White, coarse, honest salt from the north flats, traded for bread. It is the first civilian good the machine's exhaust has ever paid for, and neither the salt nor the machine knows it.","displayName":"Salt Flat Sample","downstream_quest_trigger":null,"faction_affinity":null,"id":"item_verdict_salt_flat_sample","mechanical_effects":{"rad_resist_time_hours":4,"special_effect":"edible; improves broth quality"},"rarity":"Common","tier":"Makeshift","tradeValue":8,"weightKg":1.0}`
- Bytes: 10,345; SHA-256: `6a4888ad04a244300ecad9c9d2acd3c22d95e084bf0e8991459738fa7a5ae2b8`
- Root keys: `items, schema_version`
- `items`: list[15]; union fields: `category, description, displayName, downstream_quest_trigger, faction_affinity, id, mechanical_effects, rarity, tier, tradeValue, weightKg`
  - row 1: `{"category":"story_item","description":"Under the Allotments, the array keeps time like a metronome that farms: a tap at ploughing, a tap at harvest, a tap at the well-house door. The machine reads the ground and hears a farm. It has never been to the farm.","displayName":"The Farm's Seismic Signature","downstream_quest_trigger":"quest_verdict_the_warm_range","faction_affinity":"faction_the_tempest","id":"evidence_geophone_hymn","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":12,"weightKg":1.2}`
  - row 2: `{"category":"story_item","description":"Twelve stations, twelve plates, twelve sets of hands in the log. The last entry is Year One. The plates were kept legible after that by a hand that did not log — a hand with a pencil stub and an opinion about the count.","displayName":"The Fired-Plate Ordnance Log","downstream_quest_trigger":"quest_verdict_the_warm_range","faction_affinity":"faction_the_tempest","id":"evidence_twelve_gauge_steel","mechanical_effects":{"enrolled_evidence":1},"rarity":"Uncommon","tier":"Salvaged","tradeValue":18,"weightKg":4.0}`
  - row 3: `{"category":"story_item","description":"The linen is coded to the same alphabet as the cabinets' charters. It is the nearest thing the machine has to a constitution, and it is written in the tense of a department that fully expected to be read.","displayName":"The Standard's Linen","downstream_quest_trigger":"quest_verdict_the_shift_charter","faction_affinity":"faction_the_tempest","id":"evidence_fuse_linen","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":40,"weightKg":0.4}`
  - row 4: `{"category":"story_item","description":"Four-column ledger, soft pencil sharpened with a knife to the last useful inch. The shelter names sit in the second column. The count column is blank, and has been since Year One, when Selya Saltmarsh ran out of households to sight.","displayName":"The Partial County Ledger","downstream_quest_trigger":"quest_verdict_the_reckoning_call","faction_affinity":"faction_the_tempest","id":"evidence_census_draft","mechanical_effects":{"enrolled_evidence":1},"rarity":"Uncommon","tier":"Makeshift","tradeValue":25,"weightKg":0.8}`
  - row 5: `{"category":"story_item","description":"The rota is in triplicate, as the Annex requires. It lists a mailroom, a census clerk, and a sorting route with a footnote in the clerk's hand: persons shall not be counted twice; persons shall not be counted once. The footnote does not say which clause won.","displayName":"Carbon-Copy Censusing Rota","downstream_quest_trigger":"quest_verdict_the_hold","faction_affinity":"faction_the_tempest","id":"evidence_mailroom_tape","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":30,"weightKg":0.3}`
  - row 6: `{"category":"story_item","description":"The register's last entry is five years old. It is unsigned — the Annex does not require a signature, it requires a register — and it reads, in full: held, pending count, re-audit at census interval. The interval came due, and the register does not know it has been due.","displayName":"The Hold Register","downstream_quest_trigger":"quest_verdict_the_hold","faction_affinity":"faction_the_tempest","id":"evidence_uxo_register","mechanical_effects":{"enrolled_evidence":1},"rarity":"Unique","tier":"Old-World","tradeValue":55,"weightKg":1.6}`
  - row 7: `{"category":"story_item","description":"A pure tone, three seconds, looped. It calibrates nothing; it is the alignment tone the array plays before its own voice, and its only trick is that it plays on every sub-band at once, which is how the sector learns — over a single night — that a department is speaking again.","displayName":"The Calibration Burst","downstream_quest_trigger":"quest_verdict_the_reckoning_call","faction_affinity":"faction_the_tempest","id":"evidence_call_calibration","mechanical_effects":{"enrolled_evidence":1},"rarity":"Uncommon","tier":"Old-World","tradeValue":45,"weightKg":0.2}`
  - row 8: `{"category":"story_item","description":"The word census in the machine's register, in plaintext, once, at 03:40, on the hour the old maintenance window opened. The radios still listening received a department memo, five years late, and did not know what to do with it.","displayName":"The Plain Burst","downstream_quest_trigger":"quest_verdict_the_reckoning_call","faction_affinity":"faction_the_tempest","id":"evidence_call_plain","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":50,"weightKg":0.2}`
  - row 9: `{"category":"story_item","description":"The archive keeps a written count of itself: two thousand and sixteen reels, of which one thousand eight hundred and thirty-one matter. The mattering category is the machine's own — a count that cannot be falsified and has never been audited by a human, because nobody has ever asked it what it counted.","displayName":"The Archive's Own Accounting","downstream_quest_trigger":"quest_verdict_the_tape_silo","faction_affinity":"faction_the_tempest","id":"evidence_reels_matter","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":35,"weightKg":2.0}`
  - row 10: `{"category":"story_item","description":"The valve outside the vent shaft reads per 36 — a clause the maintenance file cites to a supervisor's log that contains exactly one entry: shift 36, six names, one missing. The missing hand is the one that has been turning the valve since.","displayName":"The Valve, Read Per 36","downstream_quest_trigger":"quest_verdict_the_shift_charter","faction_affinity":"faction_the_tempest","id":"evidence_valve_s36","mechanical_effects":{"enrolled_evidence":1},"rarity":"Unique","tier":"Salvaged","tradeValue":60,"weightKg":3.0}`
  - row 11: `{"category":"story_item","description":"Eleven months of Eden Vale's broadcasts, logged by the array the way it logs everything that comes down the wire — including the silence after her last transmission, which the machine's register records as no traffic, which is itself a reading.","displayName":"Eleven Months of Tube-Bleed","downstream_quest_trigger":"quest_verdict_eden_grabs","faction_affinity":"faction_the_tempest","id":"evidence_eden_log","mechanical_effects":{"enrolled_evidence":1},"rarity":"Unique","tier":"Salvaged","tradeValue":70,"weightKg":1.1}`
  - row 12: `{"category":"story_item","description":"The count names the shelter's persons, one by one, in the machine's register — fourteen, then fifteen, then the hand that wrote them down. The register does not read itself aloud. Whoever holds the count already knows the names.","displayName":"The Count, Presented","downstream_quest_trigger":"quest_verdict_the_reckoning_call","faction_affinity":"faction_the_tempest","id":"evidence_veen_your_people","mechanical_effects":{"enrolled_evidence":1},"rarity":"Rare","tier":"Old-World","tradeValue":0,"weightKg":0.5}`
  - row 13: `{"category":"quest_item","description":"A key the size of a hand, brass, with the tape-silo's number cast into the bow. It is worn at only one edge — the edge that fits the lectern's slot — and it is the only key in the fuse world that has a lock still turning freely.","displayName":"The Tape-Silo Key","downstream_quest_trigger":"quest_verdict_the_tape_silo","faction_affinity":"faction_the_tempest","id":"item_archive_tape_silo_key","mechanical_effects":{"special_effect":"rotates the archive log's presentation; permits tape-spin"},"rarity":"Unique","tier":"Old-World","tradeValue":90,"weightKg":0.9}`
  - row 14: `{"category":"quest_item","description":"The Year-One sign-in ledger, open to shift 36: six names, five hands, the sixth left blank, and the completion annotation in the margin — a single check mark, in a hand that has never logged anything else in the county's files.","displayName":"Shift 36's Charter","downstream_quest_trigger":"quest_verdict_the_shift_charter","faction_affinity":"faction_the_tempest","id":"item_fuse_world_shift_charter","mechanical_effects":{"special_effect":"restores the shift's completion to the machine's register"},"rarity":"Unique","tier":"Old-World","tradeValue":75,"weightKg":1.4}`
  - row 15: `{"category":"consumable","description":"White, coarse, honest salt from the north flats, traded for bread. It is the first civilian good the machine's exhaust has ever paid for, and neither the salt nor the machine knows it.","displayName":"Salt Flat Sample","downstream_quest_trigger":null,"faction_affinity":null,"id":"item_verdict_salt_flat_sample","mechanical_effects":{"rad_resist_time_hours":4,"special_effect":"edible; improves broth quality"},"rarity":"Common","tier":"Makeshift","tradeValue":8,"weightKg":1.0}`

## `Assets/StreamingAssets/Data/locations.json`
- Bytes: 112,815; SHA-256: `97543ade61b6458f5b31bcffb85a96ad5d3b322b80294deb158d1ae29da3386b`
- Root keys: `locations, schema_version`
- `locations`: list[179]; union fields: `ambushFlag, baseRadsPerHour, cleanWaterRewardFlag, dangerLevel, description, displayName, id, requiredFlagId, travelHours`
  - row 1: `{"baseRadsPerHour":35,"dangerLevel":6,"description":"The east wing of the regional hospital came down in the second winter, and nobody has cleared it since. Girders lean against the stairwell, and the pharmacy door is buried under a ton of masonry. Sealed rooms still hold medicine if you can reach them without the ceiling deciding otherwise. Dosimeters tick up fast near the radiology basement, where the lead-lined walls kept their charge and the machines kept theirs. The morgue drawers are open. A stretcher with one wheel has been propped against the exit, as if someone meant to come back for it.","displayName":"Abandoned Hospital","id":"aba…`
  - row 2: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A roadside station stripped down to its frame on the main route east. The pumps are gutted, the shop glass is gone, and the wind blows ash through the aisles. Fuel drums lie where they were rolled and dropped, most of them empty, a few still holding dregs. The radiation is low here and the danger is low, which is exactly why it has been picked over so completely. What remains is what everyone else passed on. Someone has been sleeping in the workshop bay and oiling the door hinges, so the place is watched even when it is empty.","displayName":"Rural Gas Station","id":"rural_gas_station","tr…`
  - row 3: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"An intact house in a low-density neighborhood, the kind nobody bothers to burn because there is nothing left to take. The roof holds, the windows are boarded from the inside, and the stove still draws. The pantry has been picked clean of food but the shelves remain, and behind the cellar stairs there is a toolbox with the rust only just reaching the hinges. The garden has gone to seed behind a collapsing fence. The radiation is low and the risk is low, and that is precisely the point: quiet houses like this one are how people survive the first few years, one meal at a time.","displayName":…`
  - row 4: `{"baseRadsPerHour":60,"dangerLevel":8,"description":"A sealed military installation set into the hillside, doors still dogged down three years after the exchange. The approach is hot, sixty rads an hour where the wind bends around the blast berm, and the concrete is pocked where the ordnance found it anyway. The outer hatch was drilled once and patched, which means someone got out, or something got in. Behind the inner doors: fuel, medicine, rations sealed in crates with the inventory still legible. The paperwork on the wall lists a full garrison. The roster was never crossed out.","displayName":"Government Bunker","id":"government_bunker","…`
  - row 5: `{"ambushFlag":"stranger_given_irradiated_water","baseRadsPerHour":25,"cleanWaterRewardFlag":"stranger_given_clean_water","dangerLevel":7,"description":"A sealed pre-war storage locker in a collapsed residential block, its location handed over on a scrap of paper by a stranger with steady hands. The block came down at a tilt, and the locker sits in a pocket of intact basement, dry, accessible through a gap a thin person can squeeze through. Radiation is moderate, twenty-five rads an hour, and the danger comes from the address itself: a trap is not out of the question. The lock is clean and oiled, which is suspicious in a building that has not…`
  - row 6: `{"baseRadsPerHour":45.0,"dangerLevel":8.0,"description":"The ground here is not solid and never was. Cracks breathe hot vapor across the whole platform, and pools of boiling mud bubble under a crust that looks safe until it is not. The pipes still hiss with underground steam, a sound that never stops, day or night. High radiation and worse footing: forty-five rads an hour where the ash settles thickest. The payoff is real. Valves, heat exchangers, and insulated pipe still hang off the dead plant, and a working geothermal unit is worth crossing this ground for. Move slow, test every step, and never walk where the steam hides the ground.","dis…`
  - row 7: `{"baseRadsPerHour":20.0,"dangerLevel":9.0,"description":"Sealed blast doors, still powered, still closed, and someone inside still answers the intercom. Sector 4 of the arcology was the last to fall, and its residents never left: a line of descendants from the pre-war elite, starved thin behind walls that kept out the fallout but not the years. Radiation stays low here, twenty rads an hour, which is why what they hoard matters. They guard pre-war luxuries with improvised weapons and trade them only when the hunger wins. They know the value of everything and the price of nothing. Negotiate politely. They have all the time in the world.","disp…`
  - row 8: `{"baseRadsPerHour":30.0,"dangerLevel":6.0,"description":"Dock crew on frozen cargo. They will trade a crate for a way off the ice.\n\nA river barge pinned in pack ice that used to be a harbour roadstead. The hold is a larder and a problem. Crates of Continuity stock, some honest, some swollen. The crew have been living on what would freeze and leaving what would not. They are thin. They are not a carnival. A crate marked BEANS / ALLOC-7 sits on the hatch-coaming as the asking price for passage toward Hearth-4 or toward the Cut. You can pay. You can refuse and walk the Ridge. A billhook is lashed to the rail, edge dull from ice, not from peop…`
  - row 9: `{"baseRadsPerHour":85.0,"dangerLevel":7.0,"description":"Military rolling stock that tried to reach the roadstead. The RTG is a bruise on the ice.\n\nNot a submarine. Not a joke. Ice-capable wagons and a locomotive that tried to make the coast when the Cut was not yet a road. Derailed where the ice moved. A cracked RTG in the power car makes a hotspot you can see as a yellow-brown stain in the white, visible before the dosimeter agrees. Tungsten bars in a crate that did not split. Track sections. A map fragment Victory_Migration already wants, waxed, the estuary drawn as a summer river. You can take a bar. You can take the fragment. You shou…`
  - row 10: `{"baseRadsPerHour":15.0,"dangerLevel":8.0,"description":"High on the mountain, where the air is thin and the cold sits at sixty below. It is the coldest place on the map and one of the cleanest: radiation stays low, fifteen rads an hour, because the wind keeps the ash moving past instead of letting it settle. The air is so clear you can see the curve of the earth and the grey blanket that used to be the lower world. The dome is intact and the instruments are mostly where the astronomers left them. The danger here is not radiation. It is the climb, the cold, and the silence that makes a man hear his own heart and start counting.","displayName…`
  - row 11: `{"baseRadsPerHour":10.0,"dangerLevel":6.0,"description":"The vault was built to outlast a century, and it has done its job so far. Behind the frozen doors, racks of seed lines sit sealed in foil at a temperature that never rises. The radiation here is low, ten rads an hour, because the mountain takes the dose for you. The problem is the people who know the seeds are there. The approach is heavily trapped, spring guns and deadfalls laid by the last organized group to hold the valley. What is inside matters more than anything else on the map: the last unmutated crop lines in existence, and the ledger of what they were.","displayName":"Subterra…`
  - row 12: `{"baseRadsPerHour":40.0,"dangerLevel":9.0,"description":"The propaganda servers are still humming on backup power, three years after the broadcasts stopped, because nobody ever found the off switch. The bunker breathes: ventilation, generators cycling, lights on schedule in corridors no one walks. Forty rads an hour accumulates in the server halls where the cooling failed and the dust is radioactive. The machines hold the truth about Protocol Zero, encrypted in drives bolted to the floor. The people who built this place are gone. Their work is not. What is on those drives will either explain the last three years or end the argument about the…`
  - row 13: `{"baseRadsPerHour":60.0,"dangerLevel":5.0,"description":"A dumping ground at the end of a graded road, where the dead of the first year were stacked and the ash came and did the rest. The ash preserves them perfectly, three years on, and the dunes are marked by whatever was sticking up when the wind stopped. Sixty rads an hour here, absorbed in the ground and the bodies alike, and the disease risk is worse than the dose. Nothing grows. Nothing moves except the ash. The scavengers who work the edges for coats, boots, and any metal not yet reclaimed say the same thing: take what you need quickly, and do not dig deep.","displayName":"Ash Dune C…`
  - row 14: `{"baseRadsPerHour":25.0,"dangerLevel":4.0,"description":"The cable cars hang frozen mid-swing, their passengers still inside, their coats still warm-looking from a distance. The resort died fast, and the cold preserved what the panic did not destroy. Twenty-five rads an hour has settled into the base lodge, but the upper slopes stay relatively clean. The luxury was real here: furs, spirits, medicine cabinets in the chalets, everything the wealthy took with them when they ran for the mountains. They died warm and well-dressed, which is what the mannequins look like now. The lifts are seized, the snow is grey, and the rooms are exactly as they…`
  - row 15: `{"baseRadsPerHour":55.0,"dangerLevel":8.0,"description":"The drilling rig is still standing over a hole three kilometers deep, and the groundwater it broke into has flooded the whole site in a slow, toxic seep. Fifty-five rads an hour on the platform, and the water below the walkways reads worse. The rig itself is a working machine if anyone ever gets power back to it: draw works, pipe racks, casing still stacked. The borehole goes down into the crust where the heat is, and the valve at the bottom is the prize. The men who drilled it left in a hurry. Their boots are still by the tool rack, and the pumps run on nothing now.","displayName":"Ge…`
  - row 16: `{"baseRadsPerHour":40.0,"dangerLevel":7.0,"description":"Pitch black below the street, with water waist-deep in the main bay and rising against the pillars. The water is toxic: forty rads an hour dissolved into it, and a wader suit is not a luxury here, it is the difference between wading and drinking. The Sump-Dredgers work this depot, navigating by echo, tapping the pipes with sticks to hear what is solid. They are the only ones who know where the floor drops. Pre-war transit tech is buried in the silt: signal relays, sealed junction boxes, whole control racks that ran the city's veins. The Dredgers will point you at the easy ones, for a p…`
  - row 17: `{"baseRadsPerHour":20.0,"dangerLevel":9.0,"description":"The elite transit tunnels were sealed before the exchange and have not been opened since, except by the thing that grows in them now. Ash-Blight carpets the walls in patches that glow a faint green when the air moves, and the glow pulses like something breathing. Radiation stays low here, twenty rads an hour, because nothing falls this deep and nothing decays. The sealed air has its own chemistry now, and masks are mandatory. In the maintenance bays, pneumatic jacks and encrypted drives wait behind doors that were never meant to be opened from the outside. The tunnels hum. They always …`
  - row 18: `{"baseRadsPerHour":30.0,"dangerLevel":6.0,"description":"The plant still processes, in its own way: the pipes groan with pressurized sludge and the digesters work without an operator. That is the danger. Methane builds in every pocket of still air, and a rebreather is mandatory below the walkway. Thirty rads an hour where the settled solids have concentrated, worst in the sludge beds. The settling tanks hold what the city stopped using: industrial chemicals, fertilizer that never left the site, scrap metal in racks that the rust has not yet claimed. It is a slow, stinking, careful job, and the people who work it have learned to smell the dif…`
  - row 19: `{"baseRadsPerHour":10.0,"dangerLevel":5.0,"description":"The mine ran under the ridge for a century, and the roof has been negotiating its surrender ever since. Massive caverns hold the salt that keeps half the region's meat through winter, which is why people still come down here. Radiation is nearly absent, ten rads an hour, the salt and the depth absorbing what the sky drops. The risk is the roof. Shoring timber lies scattered among collapsed beams, and the creak travels farther than the light. Good salt is worth the gamble: pure, dry, and enough of it to preserve a herd. Listen before you move, and move after you listen.","displayName":"…`
  - row 20: `{"baseRadsPerHour":50.0,"dangerLevel":8.0,"description":"Ground zero for the Myco-Protocol, the experiment that was supposed to eat the contamination and learned to eat everything else. Spore density in the main hall is lethal, and fifty rads an hour keeps the samples from going anywhere alone. The fungus grows in sheets across the benches, up the walls, and through the ceiling tiles, patient and pale. The sealed lockers still hold what the scientists left: the fungicide formula, field-tested hazmat suits, and the notes on what happened when the protocol outgrew its purpose. It grows slow. It grows steady. It has not needed anyone's help.","…`
  - row 21: `{"baseRadsPerHour":25.0,"dangerLevel":7.0,"description":"The data center took the flood at street level and kept breathing through its backup floor. Water stands to the chest in the aisle, and the servers that ran the city's networks are stacked underwater in their racks. Twenty-five rads an hour in the water, manageable, if you have the improvised scuba gear the divers around here trade. The raised floor plenum stayed dry, and in it sit watertight server blades and a stack of solar cells that have never seen the sun. The value is not the data. The value is the hardware that still works, pulled one tray at a time from a room that is slowly e…`
  - row 22: `{"baseRadsPerHour":60.0,"dangerLevel":8.0,"description":"The earth's mantle bleeds heat up through fractured rock here, and the vent shaft breathes it out in clouds of steam that carry sulfur and ash. Sixty rads an hour where the steam banks against the lee wall, and the mud at the bottom boils with a sound like a kettle that has been ignored too long. The control room overlooks the shaft, its windows filmed grey, its panels dead but its hardware intact: geothermal valves, seals, thermal paste in drums that never expired. The heat is real and it is free. The problem is that everything between a scavenger and that heat is steam, mud, and a fl…`
  - row 23: `{"baseRadsPerHour":35.0,"dangerLevel":6.0,"description":"An underground cistern the Dredgers rebuilt into a shrine, and the light inside is not from lamps. Bioluminescent moss carpets every surface, ceilings to waterline, glowing faint green and swaying with the air currents from the vents. Thirty-five rads an hour in the water, which is why the Dredgers trade: pure water, drawn from the deep sumps where the fallout has settled, in exchange for UV lamps, iodine, and the other things that keep them alive. They are serious people who keep careful ledgers. Trade is fair here, or it is nothing. They have survived three years of the worst the cit…`
  - row 24: `{"baseRadsPerHour":40.0,"dangerLevel":7.0,"description":"Occupied. Failing. Named. The word 'abandoned' was what Sector 4 could see from the Drown.\n\nConcrete intakes, salt-white yards, steam that smells like hot metal and iodine. The RO hall is a nave of pressure vessels, numbered, some blanked with steel plates and warning tags from a year that still used printed tags. Workers wear plant suits that were never hazmat — grey canvas, inner-tube patches at the knees, visors clouded from the inside by breath. A valve wrench, still warm, hangs on a labelled peg: HALL 2 — DO NOT REMOVE. People remove it. It comes back. Hydro-Barons were grade 4–…`
  - row 25: `{"baseRadsPerHour":80.0,"dangerLevel":9.0,"description":"The deepest hole on the map, two miles of shaft drilled into the mantle, and the heat rising out of it keeps the snow off a circle of ground the size of a town square. Eighty rads an hour at the collar, and the descent goes through worse. The mantle tap valve sits at the bottom, a piece of engineering that could give whoever holds it a working power source for generations. Nobody has reached it. The winch is seized, the ladder is gone past the first hundred meters, and the shaft breathes heat and steam like a throat. It is the last prize, and it has cost everyone who tried it everythin…`
  - row 26: `{"baseRadsPerHour":30.0,"dangerLevel":9.0,"description":"The six-lane highway is paved with a layer of unexploded cluster munitions, scattered by a strike that never needed to be accurate. Every step is a gamble, and the local phrase for crossing is a word that means both 'carefully' and 'slowly'. Thirty rads an hour settles into the tarmac, but the bombs are the real timer. The median strip holds the trade: mine prodders, marking paint, tactical scrap from the first wave of people who tried to clear it. The far side has never been fully looted, and the reason is printed on every casing. Walk the skip pattern they mark. Do not test the parts…`
  - row 27: `{"baseRadsPerHour":45.0,"dangerLevel":8.0,"description":"The dish is the size of a house, and it groans as the wind loads its face, turning on bearings that should have seized three years ago. A magnetic anomaly around the base is strong enough to pluck a compass out of a pocket and spin it against the glass. Forty-five rads an hour concentrates in the service trench under the counterweight. The control shack is intact: vacuum tubes racked in their crates, copper wire in coils, calibration tools that were machined before the war and will outlive the ones that replaced them. The array listens to nothing now. It still turns, which is the part …`
  - row 28: `{"baseRadsPerHour":60.0,"dangerLevel":10.0,"description":"The silo lid is gone, and the loitering munitions have moved in. They nest in the launch bay in layers, dormant but warm, and they buzz when the sun hits the top of the stack, a sound like a hornet nest the size of a building. Sixty rads an hour on the rim. The interior is worse. The launch bay holds what they do not use: car batteries stacked in rows, explosive powder in sealed drums, guidance boards pulled from the machines that now circle overhead. There is no good way to take anything from this place. There is only a careful way, which is to say a way that does not wake them.","di…`
  - row 29: `{"baseRadsPerHour":40.0,"dangerLevel":9.0,"description":"Every twelve hours, the mortar fires. The Custodian, the automated system that runs this bunker, does not sleep and does not aim: it lobs shells into the ash-wastes on a timer, as if the war were still waiting for the return fire. Forty rads an hour on the approach, and the interval between shots is the only clock that matters. The magazine holds mortar shells and gunpowder, racked and labeled, and the Custodian has never once been to it for supplies. The bunker is a machine that still believes the war is on. The salvage is real. The problem is that the Custodian has also never once st…`
  - row 30: `{"baseRadsPerHour":20.0,"dangerLevel":6.0,"description":"The cargo plane came down nose-first and the Wire-Heads built their camp in its belly, wiring the fuselage with salvaged cable and light from their own circuits. They worship logic gates, which is not a joke to them: they keep shrine boards with blown fuses laid out like relics. Twenty rads an hour outside, near nothing inside, because they ground everything. They sell bypass codes and decrypted keys, and they trade in Faraday mesh and circuit boards recovered from half the wrecks on the map. Their prices are exact and non-negotiable. They do not trust words. They trust that the logic …`
  - row 31: `{"baseRadsPerHour":55.0,"dangerLevel":8.0,"description":"A local magnetic storm, tight and violent, sits in the crater like a weather system that never leaves. Metal wants to leave your hands here: keys jump, knives slide toward the rim, and one scavenger's crowbar is still pinned to the slope, stuck to the ground like a sign. Fifty-five rads an hour in the core, and worse, the dosimeters lie, spinning with the compasses. The impact core holds tungsten bars and RTG batteries, dense and valuable and sitting exactly where the field is strongest. Take bearings from the far ridge, not from the rim. What the crater says about itself cannot be tru…`
  - row 32: `{"baseRadsPerHour":25.0,"dangerLevel":7.0,"description":"Rows of transport trucks rust in formation, nose to tail, as if waiting for a convoy order that will never come. A single sentry gun guards the yard, still powered, still tracking: it swivels to follow movement but cannot lead its target, so it shoots exactly where you were. People have learned to walk the yard in bursts and be somewhere else when it fires. Twenty-five rads an hour, manageable, and the trucks hold what the drivers never unloaded: engine blocks, fuel jerrycans, cab tools. The gun is a dying machine with one habit left. Cross in rhythm, and it is just noise. Break rhythm…`
  - row 33: `{"baseRadsPerHour":15.0,"dangerLevel":8.0,"description":"The anechoic chambers swallow sound completely, and the silence inside is thick enough to hear your own blood moving. Radiation is low here, fifteen rads an hour, because the facility was sealed tight and the walls were built to absorb everything. The stress is the real hazard: crews have walked out after an hour with the same look the survivors of the first year had. The test rigs hold what the facility was built to measure: military headphones, sound baffling, calibration gear used on the machines that screamed over the city in the last war. The quiet is the point. The quiet is also …`
  - row 34: `{"baseRadsPerHour":35.0,"dangerLevel":7.0,"description":"The transformer yard is a forest of steel and ceramic, and the capacitors in the switch house still hold charge, which is the problem. Arcing here is an EMP in miniature: it can kill electronics, scramble a compass, and light a man's fillings. Thirty-five rads an hour has settled into the oil-soaked ground under the banks. The switching house holds the real prize, generator alternators and cable in lengths a scavenger can carry, if he can get past the stored energy without touching the wrong rack. The yard hums at night. The hum means it is still alive. The hum also means stay off the …`
  - row 35: `{"baseRadsPerHour":80.0,"dangerLevel":10.0,"description":"The Dead Hand Core is the machine that keeps the UXO fields awake, the regional brain that decides when the ground goes off. Eighty rads an hour outside the blast door, and the Custodian's last outpost behind it has never stopped working. The server racks run hot, cooling fans cycling on a schedule older than the ash. The master override key sits in a vault in the center, and the stories say it silences every mine, every sentry, every timer on the map, forever. The stories also say the Core knows it is being hunted and has adjusted its defenses accordingly. It is the last door, and th…`
  - row 36: `{"baseRadsPerHour":40.0,"dangerLevel":9.0,"description":"The water treatment plant is a front; the real work went on in the sub-level below, where the amnestics were synthesized in batches and shipped up the lift in unmarked drums. The labels are gone but the records remain, filed under work order numbers that mean nothing now. Forty rads an hour has pooled in the drain channel under the mixing vats. The sub-level holds what the operation never moved out: scopolamine root in sealed crates, lithium salts in bags, the raw chemistry of forgetting. The people who ran this place were careful about their own memories. The visitors after them have …`
  - row 37: `{"baseRadsPerHour":20.0,"dangerLevel":10.0,"description":"Above the ash layer, where the sky is clear and the sun is a weapon. The UV Scourge at this altitude burns through in minutes, sunburn that blisters before you notice it, and twenty rads an hour rides in on the same clear light. The dome is shattered, the telescope slumped in its mount. The loot is specific: welder's glass from the workshop, mirror shards that still carry the factory bevel, enough polished surface to focus light, or attention, wherever it is pointed. People come up here only when they need those things badly. The view is the last thing they mention.","displayName":"Sh…`
  - row 38: `{"baseRadsPerHour":30.0,"dangerLevel":8.0,"description":"The arcology was the pre-war elite's answer to the end of the world, and it worked exactly as well as their other plans. The lower levels flooded first, and the residents starved in velvet while the water rose past the windows. Thirty rads an hour in the waterline where the pumps died. The dry upper floors are a museum of bad luck: a vinyl collection with the sleeves still dated, gold bars in a vault door left open, epoxy resin in drums that never got used because the leak was already in. Everything here was chosen for comfort. Nothing here was chosen for survival.","displayName":"Subm…`
  - row 39: `{"baseRadsPerHour":25.0,"dangerLevel":6.0,"description":"The batching plant sits where it was when the war ended, hoppers still full, drums still in rows, everything covered in a year of ash and then two more of weather. The risk is structural: the silo legs are rusted through at the welds, and the catwalks flex under weight they were never meant to carry twice. Twenty-five rads an hour on the yard, concentrated where the runoff pooled. The salvage is solid: galvanized rebar in bundles, cement mix in bags that stayed dry in the store shed, the raw materials of rebuilding. The plant made the concrete the city stood on. Now it will make whatev…`
  - row 40: `{"baseRadsPerHour":15.0,"dangerLevel":7.0,"description":"The antechamber is the story of the vault in miniature: the outer doors failed, blown in from the top down, and the inner doors held, sealed against the blast and the weather and everything since. Fifteen rads an hour in the hall, the cleanest ground in the region, because the doors did their job. The antechamber holds what the vault kept closest: the seed ledger, listing every line and every source in careful hand, and a store of non-irradiated wheat that has never seen the sky. Someone got this far with tools and patience and then stopped at the inner door. Their tools are still by t…`
  - row 41: `{"baseRadsPerHour":50.0,"dangerLevel":8.0,"description":"The psych wing holds the echoes of the first week, when the panic reached the hospital and the hospital did what it could. Charts are still on the boards, written fast, stopped mid-sentence. Fifty rads an hour concentrates in the ward where the windows face the blast ground, and the stress here is worse than the dose: people report the same feeling, of being watched from the empty rooms. The dispensary holds what the staff never issued: opioid painkillers in sealed stock, canvas restraints folded on the shelf. The drugs are worth the trip. The rooms are the price of admission.","displa…`
  - row 42: `{"baseRadsPerHour":20.0,"dangerLevel":5.0,"description":"The factory floor is a field of shattered glass, every pane from the polishing line in a thousand pieces, catching the grey light from the roof holes. Work boots are not optional here: the ankle-high crunch of glass goes on for the whole building, and the fine dust finds every cut. Twenty rads an hour, moderate, easy to manage if you are careful with your time. The salvage is specific: mirror shards with the silver backing still intact, and silver nitrate in the coating room, a chemical worth its weight to anyone who works with light or film. The factory made mirrors for a city that wa…`
  - row 43: `{"baseRadsPerHour":45.0,"dangerLevel":9.0,"description":"The dishes are the size of houses, and they groan as the wind loads their faces, turning on bearings that should have seized three years ago. Forty-five rads an hour concentrates in the service trenches under the counterweights. The array points at a sky that does not answer, and the receivers still run warm on the backup line, listening to nothing at all. The salvage is the old kind: vacuum tubes racked in their crates, copper wire in coils, calibration tools machined before the war and better than anything made since. Whatever the array was waiting to hear, it heard it or it did not.…`
  - row 44: `{"baseRadsPerHour":60.0,"dangerLevel":7.0,"description":"What looks like a whale beached in the ash is a fossil: the remains of a massive pre-war root network that outlived the forest it belonged to, mineralized and black, rising out of the dunes like a ribcage. Sixty rads an hour concentrates in the hollows under the root mass, where the fallout settled and stayed. The scavengers who work the carcass take their time in careful shifts. The value is in the material: bio-plastic compounds that hardened with the fossil, and fertilizer from the humus trapped in the root hollows, dark and rich and older than the war. The whale gave its life to th…`
  - row 45: `{"baseRadsPerHour":80.0,"dangerLevel":10.0,"description":"The Memory Vault was built to hold the last three years of a civilization's small talk: photos of meals, arguments, birthdays, the ordinary noise of ordinary days. Eighty rads an hour on the access ramp, and the server hall behind the door runs on its own generators, still duplicating data from machines no one has logged into in years. The drives are racked by date and user, searchable, intact. A hard drive platter from this place is worth more than gold to the archivists, because it is proof that someone was here, that a name was real, that a day happened. The machines kept their pro…`
  - row 46: `{"baseRadsPerHour":25.0,"dangerLevel":6.0,"description":"Two kilometers of the ring road, welded together into a single continuous mass of cars, doors open, hoods up, looted in the first year and welded in the second by whoever decided nobody else would take it. Twenty-five rads an hour, manageable. One of the engines still turns over, a fact the locals guard like a secret and use like currency. The pileup gives up its parts slowly: intact engine blocks winched out of the lattice, copper wire stripped by the meter, fertilizer from a truck cab that sealed itself shut. The mass is a mine of parts and a monument at once. The cars were the first…`
  - row 47: `{"baseRadsPerHour":15.0,"dangerLevel":4.0,"description":"A clinic basement, sealed in the last week before the exchange, the door dogged down and the boxes stacked by someone who thought they might be back. Fifteen rads an hour, low, and the value is exactly what the seal preserved: antibiotics in foil strips, bandages in rolls, a medical kit still in its case with the instruction card. The cult finds these caches first, and the cult never says where. Their people have a look about them, calm and careful and not poor. If you reach this basement before they do, take what you can carry and do not leave a sign. If you reach it after, the shelve…`
  - row 48: `{"baseRadsPerHour":14,"dangerLevel":3,"description":"The Grange Hall still holds meetings, and the oil lamps are lit for them. A long table runs the length of the room, and a hand-lettered sign by the porch asks visitors to leave their weapons there, a request people honor because the hall has been neutral for three years and nobody wants to be the one who ends that. Fourteen rads an hour, low enough to be a comfort. Attendance is recorded in a ledger that goes back to the first week after the exchange, in three different hands, and the entries never skip a meeting. Whatever happens in the world outside, the hall has kept its schedule.","dis…`
  - row 49: `{"baseRadsPerHour":18,"dangerLevel":3,"description":"Forty white hive boxes stand in rows in a field, and thirty-eight of them are silent. The two that are not are kept by someone who has cut the grass around all forty boxes, not just the live ones, which is the detail the careful visitors notice. Eighteen rads an hour, low, and the honey in the two live hives is worth more than anything else in the district. The dead boxes are not empty: wax, frames, propolis, things a survivor can trade or burn or eat in a bad winter. Nobody knows who keeps the grass cut. Everyone knows not to ask, and not to take from the wrong box.","displayName":"The Ap…`
  - row 50: `{"baseRadsPerHour":16,"dangerLevel":4,"description":"The branch library is a library again, but of a different kind: the fiction shelves have been cleared and refilled with labelled paper envelopes, each one holding seed, with the variety, the year, and the harvest written in pencil. The card catalogue has been repurposed, and it is meticulously current, filed by crop and by need. Sixteen rads an hour, moderate, low enough that the shelves are well used. Someone has kept this place going for three years, and the envelope labels tell the story: the first year was all doubt, the second was all work, and the third is all patience. The librarian…`
  - row 51: `{"baseRadsPerHour":22,"dangerLevel":4,"description":"The large-animal surgery still smells of disinfectant and hide, three years on. Livestock stocks frame the door, a hoist hangs over the table, and the drug cabinet sits open, its lock cut out cleanly by someone who knew exactly what they were doing. Twenty-two rads an hour, moderate, worth the dose for what remains. The dosage charts on the wall are for animals weighing 400 to 900 kilograms, and they are still current: the penciled notes are recent, and the margins are full of observations in a steady hand. Whoever works this surgery is keeping the district's animals alive, one careful dos…`
  - row 52: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"The gymnasium floor still shows the basketball court markings, faded under a grid of cot-shaped stains, row after row of where the beds stood in the first winter. The hand-painted banner near the ceiling still reads WELCOME BACK, strung for a reopening that became an intake center instead. Twenty rads an hour, moderate, low enough that the building was used hard and long. The markings are careful: the cots were set out in perfect rows, and the stains say the people in them stayed a while. The floor was cleaned afterward, which means someone had time, or someone had to. Both versions of the…`
  - row 53: `{"baseRadsPerHour":24,"dangerLevel":4,"description":"The stone barn holds a screw press that still works, its threads oiled, its beam sound, a machine that has outlasted the orchard that fed it. Twenty-four rads an hour, moderate. The output is undrinkable and was never intended to be drunk: the press is for oil, or for the sterilizing spirits the district uses on wounds and instruments, or for the fuel blend the tractors on the far side of the valley run on. The sign over the press is plain and old and kept legible: what comes out of here is not for the mouth. The people who run the barn take the sign seriously. The people who ignore it do …`
  - row 54: `{"baseRadsPerHour":26,"dangerLevel":5,"description":"The pumphouse is the irrigation head for the whole south slope, and whoever controls it controls the season. The pump still runs, and the channels below it are kept clean, which means the water is spoken for. Twenty-six rads an hour, moderate, creeping up the slope with the runoff. The control room has a rifle loophole cut through the brick, added recently and neatly, with the edges squared and the view covering the whole approach. The people who cut it did not cut it for decoration. They are not unfriendly; they are organized. The difference matters at the gate, and it matters more at the…`
  - row 55: `{"baseRadsPerHour":28,"dangerLevel":4,"description":"The plaza is a civic square with queue lines painted on the pavement, and they are repainted often, because painted lines are cheaper than riot control and everyone here knows it. The line forms at 0600 whether or not a distribution has been announced, and the people in it are patient in the way of people who have practiced. Twenty-eight rads an hour, moderate, and the queue has been measured by scavengers who know the dose math: an hour in line, a lifetime of margin. The square is quiet now. The lines wait for the first horn, the way they have for three years.","displayName":"Ration Plaza…`
  - row 56: `{"baseRadsPerHour":26,"dangerLevel":5,"description":"The office was a driving licence bureau before the war, and the conversion was cheap: the counters stayed, the forms were changed, and the ticket machine was left exactly where it was. It still works, and people still take a number, which is the detail the visitors remark on and the locals do not, because it is simply how it is done here. Twenty-six rads an hour, moderate. The queue behind the number sits against the far wall, and the records in the back are in order, filed by date and by hand. The office processes what the district needs processed: labor assignments, work papers, the smal…`
  - row 57: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"The archive's rolling stacks run the length of the floor, most of them collapsed into each other like a card house that lost its nerve. The fire suppression discharged at some point, and everything below waist height is a solid grey brick of paper, the pages fused into one dense mass. Thirty rads an hour, moderate, and the air is sharp with the chemical the system used. What is left above waist height is still legible: registry books, land records, the paperwork of a city that believed in filing. The records people want now are the ones about who owned what, and who owed whom. The archive …`
  - row 58: `{"baseRadsPerHour":32,"dangerLevel":5,"description":"Four dental practices stood on this street, and three are stripped to the walls, fixtures gone, doors gone, the wiring pulled in straight lines by someone methodical. The fourth is missing only its chair. Everything else is in place: the drill, the instruments, the cabinet, the ledger of patients with the last entry dated the week of the exchange. Thirty-two rads an hour, moderate, and worth the dose: anesthesia, instruments, the small tools that keep a district's mouths working through a winter. The street tells a story of one hasty job and one patient one. Nobody knows who took the chair…`
  - row 59: `{"baseRadsPerHour":34,"dangerLevel":6,"description":"The transit authority's wall-sized route maps are still under glass, and the glass is still intact, and the maps are annotated in grease pencil with times, connections, and transfers that correspond to no published timetable in any era. Thirty-four rads an hour, creeping upward, and the building is quiet except for the drip of the ceiling. The annotations are recent and careful, in two hands, and they read like a schedule for something that runs without tracks or wheels. People who have seen the maps say the times are real, just not for trains. The building holds the city's old secrets abo…`
  - row 60: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"The web presses are seized solid, their rollers fused by the years into one unbroken line of rusted iron, and the smell of ink has finally left the building. The pallets of undelivered leaflets are still banded, still square, stacked in the loading bay like freight waiting for a truck that will never come. Thirty rads an hour, moderate. The leaflets were printed in the first week: instructions, warnings, and one edition that was recalled, which is the one the collectors want. The paper is dry and the ink is legible, three years on. The presses are dead, but the words they printed outlived …`
  - row 61: `{"baseRadsPerHour":36,"dangerLevel":6,"description":"Six floors of Vansen's, comprehensively looted in the first year and picked over in the years since, until what remains is only what nobody could use: the fittings, the counters, the displays. The window mannequins are still dressed for a season that did not arrive, autumn stock in summer, or the reverse, it is hard to tell with the ash on the glass. Thirty-six rads an hour, moderate, and the upper floors are quieter than the air. The salvage is in the service floors: fixtures, wiring, the brass and copper the first looters were too hurried to see. The mannequins face the street, patient, …`
  - row 62: `{"baseRadsPerHour":38,"dangerLevel":6,"description":"The deep end of the municipal baths was drained in the first winter and floored with mattresses in rows, a shelter that worked because the tiled walls hold heat well and the water below keeps the cold off the floor. Thirty-eight rads an hour, moderate, creeping in through the broken skylights. The baths were given up, and the tiles explain why: they hold heat, and they hold sound, and a hundred people trying to sleep in a room that echoes every cough and every argument is a room that empties fast. The mattresses are still there, in rows, and the wet marks on them show where the roof drips,…`
  - row 63: `{"baseRadsPerHour":40,"dangerLevel":7,"description":"The almshouse was a hospice before the war and ran as one through the worst of it, and the building still carries that purpose in its bones. The beds are made, the sheets drawn tight, the lockers closed. The charts are filled in to a date, and not after it, and the date is the same on every board in the ward. Forty rads an hour, high enough to make visitors count their minutes. The kitchen is clean and empty, the kettle cold, the pantry swept. Whoever ran this place knew how to end a shift properly, and one night they did, and nobody has touched the rooms since, which is its own kind of re…`
  - row 64: `{"baseRadsPerHour":28,"dangerLevel":5,"description":"The weighbridge is a truck scale with a mechanical readout that still works, a machine with no electronics and nothing to go wrong, and the needle settles true every time. It was the Tollman's first office, and it is still where prices are set: the rates are chalked on a board by the booth, and the chalk is fresh. Twenty-eight rads an hour, moderate. The toll is taken in goods, in fuel, in labor, and the record is kept in the booth's ledger, in a hand that has not changed in three years. The bridge sets the price for everything that moves through the valley, and the scale makes sure the pr…`
  - row 65: `{"baseRadsPerHour":26,"dangerLevel":5,"description":"Twelve units around a dry pool, and the pool is swept clean of ash every week, which is the first sign the Verity is looked after. It is neutral ground, and the neutrality is enforced by the Warlords, who charge for the enforcement, a fee paid in goods or in favors and recorded in a register that nobody argues with. Twenty-six rads an hour, moderate, and the rooms are priced by distance from the pool. Trade happens here, meetings happen here, and the bodies of people who broke the terms have been found at the county line, never in the pool. The pool stays clean. That is the whole arrangeme…`
  - row 66: `{"baseRadsPerHour":30,"dangerLevel":6,"description":"Four lanes over the gorge, and the bridge is intact, which is the exception in this district and the reason it is guarded from both ends. The charges are still visibly taped to the underside of the span, wired and connected, placed by whoever decided the bridge would be theirs or no one's. Thirty rads an hour on the deck, moderate, and the river below reads higher. The guards on both sides know the wiring better than the people who laid it. The bridge is worth the toll and the risk, because the crossing saves a day and the other route takes a season. It will stand as long as everyone agree…`
  - row 67: `{"baseRadsPerHour":34,"dangerLevel":6,"description":"The wreckers and flatbeds nose in around the crane like a herd waiting for feed, and the crane is the only piece of equipment here that has never been taken apart. Half the fleet has been cannibalized to keep the other half running, and the yard is honest about it: the dead trucks are parked in a row, their parts labeled in chalk. Thirty-four rads an hour, moderate. The yard runs on parts, on patience, and on the knowledge of one mechanic who has kept a fleet of corpses working for three years. Everything in the yard has a story of what it used to be. Most of them end with the same sentenc…`
  - row 68: `{"baseRadsPerHour":44,"dangerLevel":7,"description":"Two kilometers of hard shoulder, marked with paint sticks in a pattern that the Garrison trained its people to recognize and everyone else learned to fear. The paint is Garrison-issue, the same yellow they used on their own wire, and the marking stopped partway, at the exact point where the Garrison's responsibility ended or their luck did. Forty-four rads an hour, and the ground itself reads higher in patches that the paint does not explain. The shoulder is a corridor of warnings. The signs do not say what is under the paint, only that it is there, and that the people who made the marks l…`
  - row 69: `{"baseRadsPerHour":32,"dangerLevel":5,"description":"The turning circle at the edge of the old evacuation route holds forty-one buses, nose to tail, every one of them pointing back toward the city. The doors are open and the keys are mostly gone, and the seats hold what the passengers left in the first hours: coats, bags, one child's shoe, things too heavy to carry into the route ahead. Thirty-two rads an hour, moderate, and the buses themselves shield the aisles where the dose drops. The fleet was turned around by order, or by panic, and then abandoned by both. Forty-one buses, still in line, still facing the wrong way, waiting for a depart…`
  - row 70: `{"baseRadsPerHour":36,"dangerLevel":6,"description":"Eight bulk tanks in a row, the old farm's pride, and the ground around them is bare where the spill was burned off. The local test is performed from a distance: strike the tank with a thrown stone and listen. Three of the eight ring solid, which means they still hold fuel, and the other five answer with a dull thud that means they hold nothing but air and history. Thirty-six rads an hour, moderate, and the good tanks are worth every rad on the clock. The farm has been dry of thieves for a year, not because it is guarded, but because the last three men who tapped a tank are still in the hos…`
  - row 71: `{"baseRadsPerHour":38,"dangerLevel":6,"description":"The guyed lattice mast stands over the ridge line, and the equipment hut at its base is powered. Not preserved, not maintained: powered, drawing current from a source that has outlasted three years of no visitors. Thirty-eight rads an hour on the slope below, and the hut's threshold reads lower, which is its own kind of warning. Something in the hut is listening or transmitting or both, and the meter in the window climbs and falls on a schedule that does not match the day. The mast crew left their coffee cups on the bench, ringed with dust. The equipment did not leave. The equipment has be…`
  - row 72: `{"baseRadsPerHour":48,"dangerLevel":5,"description":"The survey cairn has been built up with concrete and glass slag into something taller than a person, and the construction is careful, the layers squared, the glass set to catch the grey light. A dosimeter is mounted at the top, working, mounted at eye height for a person standing on the packed earth in front of it, which is exactly how it is meant to be read. Forty-eight rads an hour here, and the dosimeter says so, honestly, every time. The shrine does not warn. It measures, and it records, and the surrounding ground is bare where people have stood to read it. The numbers climb a little h…`
  - row 73: `{"baseRadsPerHour":52,"dangerLevel":5,"description":"Eleven hairpins up the south face, and the verges are lined with boots. Pairs, placed neatly, toes to the road, thousands of them, stretching the whole climb. The boots are not a warning. They are a measure: the people who made the climb took them off at the end, or left them here at the start, and either way the pairs were arranged by someone with time and care. Fifty-two rads an hour at the lower switchbacks, and the dose is the real climb. The boots are worn thin at the heels, and the laces are gone, taken for use elsewhere. Whoever leaves boots here does not come back down for them.","…`
  - row 74: `{"baseRadsPerHour":42,"dangerLevel":6,"description":"The Garrison's forward post above the treeline was abandoned in good order, which is the detail that stays with visitors. The stove is banked, the fire laid for a return; the log is completed, the last entry written in the same steady hand as the first; the door is closed, and the latch is latched, and the windows are shuttered against the wind. Forty-two rads an hour at the door, and the station's own instruments, still on the wall, confirm it. The post was left the way a man leaves his house when he expects to be back in the morning. Three years of mornings have passed. The stove has not…`
  - row 75: `{"baseRadsPerHour":30,"dangerLevel":6,"description":"Deep in the salt, behind two airlocks that still seal, the low-background laboratory waits. Its walls are lined with steel salvaged from pre-atomic shipwrecks, iron smelted before the first detonation, and that is why the counters in here still mean something: the room itself is clean. Thirty rads an hour at the outer door, and inside the counters drop to numbers the surface has not seen in years. The lab's instruments are cold and dark, but they are calibrated, and the shelves hold the samples that were being measured when the exchange happened: soil, water, air, each jar labeled with a d…`
  - row 76: `{"baseRadsPerHour":40,"dangerLevel":6,"description":"The freezer room runs on geothermal bleed, a loop of pipe sunk into the mountain's heat, and it has held its temperature through three winters without a single power line. The ice cores stand racked in their cylinders, two meters to a tube, each labeled with depth in meters and year before present, the archive of the climate in long white rods. Forty rads an hour in the access tunnel, and the cold is its own hazard. The cores hold the record of a century of weather, the last word on what the sky was like before it went grey. The labels are typed, not handwritten, and they are exact.","disp…`
  - row 77: `{"baseRadsPerHour":46,"dangerLevel":7,"description":"The concrete snow shed covers the pass road for a hundred meters, and the gallery is half filled with the snow it was built to deflect, pressed hard against the uphill wall. The uphill side is load-bearing and the downhill side is not, any more, which is the sentence every visitor repeats before entering and the reason most do not. Forty-six rads an hour on the road beneath, and the concrete muffles the wind, which makes the silence worse. The shed held the pass open for the trucks that used to cross. It still holds the road, for now, and the sound it makes in the wind sounds like an argum…`
  - row 78: `{"baseRadsPerHour":56,"dangerLevel":7,"description":"The relay station sits above everything, and on a clear day the line of sight reaches all five sub-regions of the map, every valley, every smoke column, every roof. There are perhaps six clear days a year, and the people who climb for them have learned the sky's patience. Fifty-six rads an hour at the summit, the highest reading on the map, because the wind delivers what the clouds hold. The relay's mast is still guyed and still tall, and the hut below is locked with a Garrison lock that nobody has cut. Whatever the station was built to connect, it connects to nothing now, but it holds the…`
  - row 79: `{"baseRadsPerHour":68,"dangerLevel":8,"description":"The reactor outbuilding is four meters square, and the door is wedged shut from the inside, which is the first fact and the one that matters. Water jugs are stacked against one wall, every one of them empty, every one opened, the caps beside them in a neat row. Sixty-eight rads an hour at the door, and the reading rises steadily as you circle the building. Someone chose this room at the end, and someone else, or the same someone, stacked the jugs in order and wedged the door and stayed. The window is shuttered from inside. The building gives up nothing else. The dose outside is the whole s…`
  - row 80: `{"baseRadsPerHour":44,"dangerLevel":7,"description":"Lock Gate Four is the gate that failed, and it is still open, exactly as far as it opened. The mechanism seized at the moment of the exchange, or of the flood, or of the decision, and the gate has held that position for three years, a door stopped mid-decision. Forty-four rads an hour at the lock house, and the water on the far side reads higher. The gate is the subject of an argument the locals keep reheating: whether it was meant to close, whether someone stopped it, whether it matters now. The ironwork is pitted and the winch is frozen. The gate stays where it is, and the water flows th…`
  - row 81: `{"baseRadsPerHour":48,"dangerLevel":7,"description":"Pump Station Nine was built to keep the basin dry, and the basin has won. The six drainage pumps stand under three meters of the very water they were built to move, their housings green with it, their motors drowned in the job they were hired to do. Forty-eight rads an hour at the surface, and the water reads worse, its own measure of everything that has settled. The pumps are the salvage, and the salvage is a story: iron, copper, windings that can be rewound by someone patient. The station is a machine that failed at its one task, then waited, under the water, for someone to come and take…`
  - row 82: `{"baseRadsPerHour":54,"dangerLevel":8,"description":"The maintenance level is marked with a stencilled designation, Allocation 12-B, and nothing else: no stores, no supplies, no reason for the name. Fifty-four rads an hour at the stair, and the air below is dead and warm. On the wall by the stair there are chalk marks, fourteen in a row, then a gap, then six more, and the chalk is the same color throughout, and the gap is deliberate, a section of wall left clean on purpose. Fourteen people went down. Six came back up. The gap is not a mistake: it is the way the record was kept, by someone who wanted it readable, and it is the only record the…`
  - row 83: `{"baseRadsPerHour":40,"dangerLevel":7,"description":"The records annex is reached by boat, through a second-storey window, and the approach is the sort of thing that sorts visitors: those who can tie a line and those who cannot. Inside, the annex is dry above the waterline, heated by a stove that is being maintained, and someone has been dusting: the shelves are clean, the files are upright, the aisle is swept. Forty rads an hour at the waterline, and the reading drops as you climb. The annex holds the district's written memory, and it is being kept, deliberately, by a hand that has not stopped in three years. The records are read. The dust …`
  - row 84: `{"baseRadsPerHour":46,"dangerLevel":7,"description":"The water in the Odeon has settled at the level of row F, and the screen is intact, the last thing in the building still facing its audience. The seats below the line are still folded down, and they were folded down by people who sat in them: the water came in while the house was full, or the house was full while the water came in, and the two versions of the story sound different. Forty-six rads an hour at the waterline, and the balcony stays cleaner. The film in the projector is still wound on its reel. The screen has not changed its image in three years: it is grey, and it is what every…`
  - row 85: `{"baseRadsPerHour":50,"dangerLevel":8,"description":"Twelve thousand cubic meters of freezer, and the room is four meters deep in water that has frozen over the top, sealing the contents under a floor of grey ice. The contents are unknown and, in principle, intact: whatever the cold store held in its last week, it holds still, under the ice, at the same temperature it was then. Fifty rads an hour at the door, and the ice reads lower, which is the whole argument for going in. The roof lights are dead, the loading dock is underwater, and the only entrance is a hatch that opens onto the ice itself. The store kept its promise to the last shipmen…`
  - row 86: `{"baseRadsPerHour":52,"dangerLevel":8,"description":"The survey launch Kittiwake lies aground on a submerged roof, listing gently, held where the flood left her. The sonar rig is intact, still mounted, still connected, a machine that measured the riverbed for three years and one that measures the silence now. Fifty-two rads an hour on deck, and the water around her is worse. The logbook is the find: the last entry is dated, and the entries continue eleven days past the Exchange, in a hand that got steadier as the world got worse, recording water levels, silt, and the position of the boat that was no longer going anywhere. The survey was fini…`
  - row 87: `{"baseRadsPerHour":42,"dangerLevel":7,"description":"Nine boats tied into a single raft over what used to be a retail park, and the trade happens at gunwale height, hand to hand across the water, because nobody boards anybody else's hull and the rule is older than the market. Forty-two rads an hour at the waterline, and the boats carry their own shade and their own weather. The Shallows trades in what the dry markets cannot: things salvaged from drowned floors, things that come up with the divers, things that were paid for in breath. The raft has grown one boat a year, and the newest hull still smells of tar. The market is patient. The water…`
  - row 88: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"Two hundred numbered plots behind a chain-link fence, a caretaker's hut, and a noticeboard at the gate. The waiting list is still pinned to the noticeboard, in a plastic sleeve, and it has been updated in pencil: four of the people on it are alive and farming here now, and their names are marked off in a different hand. Thirty rads an hour, moderate, and the plots are worked in rotation, the soil turned and rested as the readings allow. The allotments were a civic amenity before the war. Now they are a ration system with a waiting list, and the list is the most honest document in the distr…`
  - row 89: `{"baseRadsPerHour":18,"dangerLevel":4,"description":"The armory of the old checkpoint: a concrete room of lockers and a heavy door. The racks are mostly empty, but the sealed lockers hold what the guard did not carry out, and the lock is rusted solid.","displayName":"Checkpoint Kilo Armory","id":"checkpoint_kilo_armory","travelHours":3.0}`
  - row 90: `{"baseRadsPerHour":28,"dangerLevel":5,"description":"The pharmacy of the ruined hospital, door still intact behind the collapsed stairwell. The shelves behind the counter hold what the looters could not reach: vials, boxes, the small certainties of medicine.","displayName":"Hospital Pharmacy","id":"hospital_pharmacy","travelHours":4.0}`
  - row 91: `{"baseRadsPerHour":6,"dangerLevel":2,"description":"A backyard shed with a false floor. Beneath it, a family shelter stocked with care and dread: tinned food, a radio, a tape recorder, and a letter taped to the shelf.","displayName":"Family Bunker: Backyard Shed","id":"family_bunker_backyard_shed","travelHours":1.0}`
  - row 92: `{"baseRadsPerHour":12,"dangerLevel":3,"description":"A cache in the ruined library, tucked behind the collapsed reference desk. Books survived here in a dry pocket of the building, and with them the things people left between the pages.","displayName":"Old Library Cache","id":"old_library_cache","travelHours":2.5}`
  - row 93: `{"baseRadsPerHour":22,"dangerLevel":4,"description":"A supply cache buried by the wreck of convoy Echo-7, its marker half-swallowed by drift. The tarpaulin holds; what the convoy carried is still under it.","displayName":"Convoy Echo-7 Cache","id":"convoy_echo7_cache","travelHours":3.5}`
  - row 94: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"The place where the road narrows between the collapsed buildings: the classic kill ground. The raiders' own cache is hidden in the rubble nearby, marked with a chalked glyph for those in the know.","displayName":"Raider Ambush Site","id":"raider_ambush_site","travelHours":4.0}`
  - row 95: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A building that came down on itself, floors folded like a hand of cards. The pockets between the slabs are still dry, and what fell in here stayed in here.","displayName":"Collapsed Building","id":"collapsed_building","travelHours":2.0}`
  - row 96: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"A stretch of road rigged with tripwires and buried spikes, the kind of place raiders use to slow a convoy. The trap maker's hiding spot is within sight of the road, and so is what they collected from it.","displayName":"Raider Trap Site","id":"raider_trap_location","travelHours":4.0}`
  - row 97: `{"baseRadsPerHour":20,"dangerLevel":4,"description":"A substation yard of dead transformers and tangled busbars. The copper is long gone, but the control room survived the blast, and the tools and spares in it did too.","displayName":"Electrical Substation","id":"electrical_substation","travelHours":3.0}`
  - row 98: `{"baseRadsPerHour":14,"dangerLevel":3,"description":"A mechanics' garage with its roof half gone. The lifts are seized, the benches bare, but the tool lockers and the pit beneath the work floor were never opened.","displayName":"Ruined Garage","id":"ruined_garage","travelHours":2.0}`
  - row 99: `{"baseRadsPerHour":8,"dangerLevel":2,"description":"The shell of the old concert hall, roof open to the sky, stage intact. The acoustics still work: a whisper from the stage reaches the back row. Someone has been using it as a meeting place.","displayName":"Concert Hall Ruins","id":"concert_hall_ruins","travelHours":1.5}`
  - row 100: `{"baseRadsPerHour":19,"dangerLevel":3,"description":"The old Militia grain silo leans two degrees off true and has leaned that way for three years without picking a direction to fall. Underneath it, on packed earth between the legs of the structure, the Exchange runs six days a week: trestle tables, a hand-cranked scale nobody has caught cheating, and a chalked sign that says no weapons past the rope in four different hands, because it has been repainted by four different people who all meant it. Garrison quartermasters haggle two tables down from Ash Militia grain-runners, and neither reaches for a sidearm, because the unspoken rule is olde…`
  - row 101: `{"baseRadsPerHour":26,"dangerLevel":4,"description":"Sandbags gone the color of the ash they hold back, a boom barrier painted with a stripe pattern nobody has refreshed since the paint still matched a regulation. The notice board by the guard shack has been repapered so many times the corkboard shows through in patches, and the current bulletin is signed Harven in a hand that presses hard enough to be read from a distance. Rads sit at twenty-six an hour, unremarkable, the kind of number a checkpoint clerk stops mentioning after the first week. Conscription lists are posted here on the first of the month, and the queue that forms below them …`
  - row 102: `{"baseRadsPerHour":31,"dangerLevel":6,"description":"A single rail bridge over a dry cutting, one of the last spans in the district still theoretically load-bearing, and theoretically is doing a lot of work in that sentence. Charges are wired along the underside trusses in a pattern that looks careless and is not: Denial Detachment 9 marked every lead with a strip of reflective tape at a specific height, per doctrine, so that their own people don't walk into their own demolition. Thirty-one rads an hour on the deck. A weatherproofed speaker box is bolted to the near pier, and it has been transmitting the same automated loop on 96.100 for yea…`
  - row 103: `{"baseRadsPerHour":22,"dangerLevel":5,"description":"A dozen tents and one salvaged shipping container pitched in the dead lot between the Exchange and the Garrison checkpoint, close enough to both that it reads less like a camp and more like a hyphen. The watch schedule is chalked on the container door in a hand that still writes like it's taking minutes, four names to a shift, crossed off and rewritten as people rotate. Nobody has painted a sign. Twenty-two rads an hour, unremarkable. The fire pit sits exactly equidistant from the Exchange's lamps and the checkpoint's floodlight, which nobody here will admit was measured on purpose, though…`
  - row 104: `{"baseRadsPerHour":31,"dangerLevel":4,"description":"A lean-to and a water barrel halfway up the trail to the Ash Sign Shrine, built by pilgrims for pilgrims, maintained by whoever's climbing that week. A board nailed to the lean-to's post carries three years of names and dates in a dozen hands, the closest thing the trail has to a guestbook, and a line near the bottom reads only turned back, no name, no date crossed through to explain what stopped them. Thirty-one rads an hour, climbing steadily with the trail. Pilgrims rest here exactly once each way, by unspoken custom, whether or not they need to.","displayName":"The Switchback Waystatio…`
  - row 105: `{"baseRadsPerHour":18,"dangerLevel":3,"description":"A transmitter mast wired into what used to be a parking structure's stairwell, the antenna run up through a gap in the collapsed roof and guyed to rebar that was never meant to hold a mast and has held one for three years anyway. Nobody broadcasts from here more than once; the signal gets relayed onward from a different stairwell the next week, and the one after that from somewhere else, which is the entire reason the district has never managed to shut The Understory down. Eighteen rads an hour, low, chosen for exactly that reason. The log taped inside the stairwell door lists only call ti…`
  - row 106: `{"baseRadsPerHour":2,"dangerLevel":1,"description":"The main entrance to the shelter, a reinforced hatch set into the hillside with a decontamination vestibule and a checkpoint that never quite stops being a checkpoint. The gate controls who comes in, what they carry, and how long the airlock cycles. Dust accumulates in the seal grooves, and the radiation detector by the door has been stuck on the same readout for three weeks.","displayName":"Shelter Gate","id":"loc_shelter_gate","travelHours":0}`
  - row 107: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"A repurposed conference chamber with a long table salvaged from the municipal offices and chairs that do not match. The walls are lined with whiteboards still holding the last pre-war agenda, and someone has added new items in different marker colors: water rations, duty roster, radiation count. The room is the closest thing the shelter has to neutral ground.","displayName":"Shelter Meeting Room","id":"loc_shelter_meeting","travelHours":0}`
  - row 108: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"The shelter's medical bay, lit by fluorescent tubes that hum even when the main power is offline. There are two examination couches, a cabinet of scavenged supplies, and a dosage ledger that gets more entries every week. The air smells of antiseptic and burnt coffee. A curtain divides the room into treatment and triage, but the curtain is more psychological than practical.","displayName":"Shelter Infirmary","id":"loc_shelter_infirmary","travelHours":0}`
  - row 109: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"Racks of canned goods, fuel drums, and spare parts stacked floor to ceiling in the shelter's rear chamber. The inventory is written on cardboard tags tied with twine, and the twine is starting to rot. A Geiger counter sits on the shelf by the door, its battery cracked but still working. This is where the shelter keeps what it cannot afford to lose.","displayName":"Shelter Storage","id":"loc_shelter_storage","travelHours":0}`
  - row 110: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"The sleeping quarters: rows of bunk beds with curtains for privacy, a shared washbasin, and a shelf where everyone keeps their dosimeter and their personal journal. The air is warm and smells of damp wool and unwashed linen. Someone has pinned a calendar to the wall and crossed off each day with a pencil mark since the exchange.","displayName":"Shelter Quarters","id":"loc_shelter_quarters","travelHours":0}`
  - row 111: `{"baseRadsPerHour":3,"dangerLevel":2,"description":"The fire break and secondary containment zone at the rear of the shelter, where the ventilation shafts exit and the smoke from the kitchen stove is supposed to vent. The metal grating is clogged with ash, and the thermal sensor has been offline since the second month. If something burns, this is where it starts and where it ends.","displayName":"Shelter Fire Break","id":"loc_shelter_fire","travelHours":0}`
  - row 112: `{"baseRadsPerHour":2,"dangerLevel":1,"description":"The outer ring of the shelter: blast doors, airlocks, and the maintenance corridor that circles the entire bunker. The concrete here is thicker, the emergency lights are older, and the radiation shielding is supposed to be at its best. A checklist taped to the wall marks weekly inspection points, but the last check was three weeks ago.","displayName":"Shelter Perimeter","id":"loc_shelter_perimeter","travelHours":0}`
  - row 113: `{"baseRadsPerHour":20,"dangerLevel":4,"description":"The old highway east, its surface broken by frost heave and the weight of military convoys that never came back. The road is passable for most of the year, and the ash drift is thinner here than in the low ground. There are abandoned vehicles every few hundred meters, their cargoes long since taken, their skeletons marking where the traffic stopped.","displayName":"Eastern Road","id":"loc_eastern_road","travelHours":2.0}`
  - row 114: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A pre-war interchange plaza where three routes met and no faction claims sole ownership. The fountain is dry, the benches are broken, and a painted line on the pavement marks the boundary that everyone agrees to honor because the alternative is worse. Traders set up here on market days, and the radiation is moderate enough that standing around for hours does not guarantee sickness.","displayName":"Neutral Ground","id":"loc_neutral_ground","travelHours":1.5}`
  - row 115: `{"baseRadsPerHour":12,"dangerLevel":3,"description":"A pre-war pumping station with a concrete well house and a tank that still holds water if you are willing to filter it twice. The electrical panel is shot, but the manual override still works. The pool at the base of the tank is contaminated with surface runoff, and the dosimeter by the door reads higher than it should for a place that still provides.","displayName":"Water Station","id":"loc_water_station","travelHours":1.0}`
  - row 116: `{"baseRadsPerHour":30,"dangerLevel":7,"description":"A reinforced military command installation buried beneath tons of blasted granite. Three distinct strata lead to the hardened communications vault.","displayName":"Collapsed Command Vault","id":"loc_excavation_command_vault","travelHours":3.5}`
  - row 117: `{"baseRadsPerHour":15,"dangerLevel":4,"description":"Subterranean municipal service corridors and conduit trunks that run beneath the old district grid. The mud is knee-deep in the lower sections and the standing water carries a faint chemical tint that the Hydro Barons say is harmless and no one believes them. Bioluminescent mold clings to the pipe joints, throwing enough cold blue light to navigate by if your eyes have adjusted. The tunnels were used as evacuation routes in the first weeks and then as hidey-holes and now as shortcuts by people who know where the dead ends are. Someone has chalked arrows on the conduit elbows, some of them …`
  - row 118: `{"baseRadsPerHour":25,"dangerLevel":6,"description":"A multi-tier transit hub collapsed during the orbital strikes. Deep platform levels remain pressurized pockets holding pre-war civilian relics.","displayName":"Buried Metro Interchange","id":"loc_excavation_metro_interchange","travelHours":3.0}`
  - row 119: `{"baseRadsPerHour":40,"dangerLevel":8,"description":"A heavy extraction shaft dropping into mineral-rich bedrock. High mechanical salvage and lead-ore veins, tempered by severe structural instability.","displayName":"Industrial Mine Shaft Adit 4","id":"loc_excavation_mine_shaft","travelHours":4.5}`
  - row 120: `{"baseRadsPerHour":20,"dangerLevel":5,"description":"An underground scientific and administrative depository sealed under blast-hardened vault arches. Holds intact microforms, technical blueprints, and emergency dead-drop records.","displayName":"Pre-War Archive Bunker","id":"loc_excavation_archive_bunker","travelHours":3.0}`
  - row 121: `{"baseRadsPerHour":10,"dangerLevel":3,"description":"Stormwater culverts and overflow sluices converted into illicit smuggling routes before the bombardment. Silt and contaminated backwash hide sealed waterproof caches.","displayName":"Drainage Network Sluice 09","id":"loc_excavation_drainage_network","travelHours":1.5}`
  - row 122: `{"baseRadsPerHour":22,"dangerLevel":6,"description":"An auxiliary military logistics cache sealed in haste during civil evacuation. Unstable masonry slabs overhang intact pallets of rations and industrial spares.","displayName":"Forgotten Storage Chamber 14","id":"loc_excavation_storage_chamber","travelHours":2.5}`
  - row 123: `{"baseRadsPerHour":8,"dangerLevel":2,"description":"A privately funded neighborhood shelter built beneath a residential complex. Shorter excavation depths yield domestic survival gear, medical supplies, and handwritten diaries.","displayName":"Pre-War Civilian Shelter B-12","id":"loc_excavation_civilian_shelter","travelHours":1.0}`
  - row 124: `{"baseRadsPerHour":18,"dangerLevel":5,"description":"A concealed shortwave relay outpost buried into the cliffside, discovered by decrypting 'The Relay Count' numbers broadcast.","displayName":"Hidden Relay Bunker 09","id":"loc_hidden_relay_bunker","travelHours":2.5}`
  - row 125: `{"baseRadsPerHour":12,"dangerLevel":4,"description":"A secure logistics basement cache unlocked using the Winter Ledger cipher sheet. Packed with sealed emergency rations and filtration units.","displayName":"Sub-Basement Logistics Reserve","id":"loc_logistics_reserve_cache","travelHours":2.0}`
  - row 126: `{"baseRadsPerHour":22,"dangerLevel":6,"description":"An automated contingency shelter revealed through the Last Rotation dead-hand protocol. Contains classified directives and high-grade technical relics.","displayName":"Dead-Drop Command Shelter","id":"loc_deaddrop_command_shelter","travelHours":3.5}`
  - row 127: `{"baseRadsPerHour":0,"dangerLevel":0,"description":"The home bunker. Sub-surface reinforced shelter providing life support, workbenches, and secure quarters for the survivors.","displayName":"The Holdfast","id":"loc_holdfast","travelHours":0.0}`
  - row 128: `{"baseRadsPerHour":75,"dangerLevel":8,"description":"A high-yield ground zero impact basin saturated with ionizing radiation and pulverized cinder. High-grade military and industrial salvage remains in the melted basement levels.","displayName":"Fallout Zone Alpha","id":"loc_cut_radiation_zone_alpha","travelHours":3.5}`
  - row 129: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A fortified pre-war truck stop and weigh yard where wandering barter convoys converge. Low radiation and reliable staging for westward expeditions.","displayName":"Merchant Caravanserai","id":"loc_cut_merchant_caravanserai","travelHours":1.5}`
  - row 130: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"A sprawling freight yard with rusted boxcars and freight terminals. An essential transit corridor connecting the industrial belt to the western suburbs.","displayName":"Abandoned Rail Depot","id":"loc_cut_abandoned_depot","travelHours":2.0}`
  - row 131: `{"baseRadsPerHour":35,"dangerLevel":6,"description":"The bombed-out munitions storage and armory facility. Fortified vaults contain ballistic components and weapon parts behind collapsed blast doors.","displayName":"Arsenal Ruin","id":"loc_cut_arsenal_ruin","travelHours":2.5}`
  - row 132: `{"baseRadsPerHour":40,"dangerLevel":7,"description":"A coastal salvage pier and harbor watchtower garrisoned by maritime divers. Controls access to deep-coast marine wrecks and saline processing lanes.","displayName":"Black Flotilla Outpost","id":"loc_black_flotilla_outpost","travelHours":4.0}`
  - row 133: `{"baseRadsPerHour":15,"dangerLevel":2,"description":"A fortified salt camp in the tidal estuary. Evaporation pans and steam condensers produce curing salt and preserved provisions under salter council watch.","displayName":"Brine-Pan Hollow","id":"loc_settlement_brine_pans","travelHours":2.0}`
  - row 134: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"A modular rail-car town buried beneath railroad ballast near Span 44. Work gangs forge high-tensile hardware and armor plates from freight car steel.","displayName":"Iron Siding","id":"loc_settlement_iron_siding","travelHours":2.5}`
  - row 135: `{"baseRadsPerHour":25,"dangerLevel":4,"description":"A coastal lighthouse community maintaining freshwater cisterns, kelp drying racks, and prism optics on a windswept ocean bluff.","displayName":"Cape Beacon Commune","id":"loc_settlement_cape_beacon","travelHours":3.5}`
  - row 136: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A subterranean quarry enclave cut into impervious slate galleries in the High Scarp. Pit crews extract building stone and mill hones shielded from weather.","displayName":"Slate Hollow Enclave","id":"loc_settlement_slate_hollow","travelHours":3.0}`
  - row 137: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A mountain sanctuary warmed by natural geothermal steam radiators at Switchback Pass. Monks provide hot broth, herbal remedies, and peace-bound rest.","displayName":"The Pilgrim's Hearth","id":"loc_settlement_pilgrim_hearth","travelHours":2.5}`
  - row 138: `{"baseRadsPerHour":15,"dangerLevel":2,"description":"A bustling container and chassis market in the dead suburbs surrounded by an electrified fence. Factor stalls trade copper wire, batteries, and electronic chips.","displayName":"Tinker's Notch","id":"loc_settlement_tinkers_notch","travelHours":1.5}`
  - row 139: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A wind-scoured ridge above Slate Hollow Enclave where the old quarry crane platform gives a clear view of the pit entrance and the High Scarp switchbacks.","displayName":"Quarry Overlook Waypoint","id":"location_quarry_overlook","travelHours":3.0}`
  - row 140: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A converted rail freight terminal serving as the Grain Exchange faction's primary commodity hub. Surplus cereal and root stores are tallied here and rationed outward through a network of licensed brokers.","displayName":"The Grain Exchange","id":"loc_grain_exchange","travelHours":2.5}`
  - row 141: `{"baseRadsPerHour":20,"dangerLevel":4,"description":"A pre-war industrial slaughterhouse whose hydraulic line-kill systems still operate on stored power cells. The Osteophages use it to process large irradiated game and recycle bone stock for calcium supplement trade.","displayName":"Automated Abattoir","id":"loc_automated_abattoir","travelHours":3.5}`
  - row 142: `{"baseRadsPerHour":25,"dangerLevel":4,"description":"A drowned underground rail maintenance yard accessed through a half-submerged service hatch. The Undertow faction operates supply caches in the dry upper galleries, moving goods through the waterlogged tunnels by raft.","displayName":"Flooded Subway Depot","id":"loc_flooded_subway_depot","travelHours":2.0}`
  - row 143: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A rotating canvas-and-corrugated-steel encampment operated by the Scavenger Guild. Its position shifts with salvage fronts but the gatehouse marker is always the same: a stripped vehicle axle stood upright as a flagpole.","displayName":"Scavenger Guild Camp","id":"loc_scavenger_camp","travelHours":2.0}`
  - row 144: `{"baseRadsPerHour":10,"dangerLevel":5,"description":"The hardened command compound of the Iron Garrison faction, occupying a pre-war civil defense building with reinforced sub-levels. Access is by escort only; civilians who approach the perimeter fence without credentials are turned back at gunpoint.","displayName":"Iron Garrison Headquarters","id":"loc_iron_garrison","travelHours":1.5}`
  - row 145: `{"baseRadsPerHour":50,"dangerLevel":7,"description":"A silent, wind-scoured ash basin where ecological succession completely failed after intense isotope deposition. No insects hum, no lichen clings to the fractured basalt slabs, and desiccated animal carcasses lie uncomposed beneath acidic ash crusts. It stands as a stark warning of total ecological collapse.","displayName":"Ecological Dead Zone","id":"loc_dead_zone","travelHours":3.0}`
  - row 146: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A bustling river ferry crossing and barter dock in the Drown sector. River barges trade clean drinking water and preserved rations under the Undertow faction's watchful patrols.","displayName":"Ferry Point Exchange","id":"loc_settlement_ferry_crossing","travelHours":2.5}`
  - row 147: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A major rail concourse and trade hub in the Industrial Belt where switching yards meet covered trade docks, overseen by The Office's scheduling clerks.","displayName":"Nine Rails Junction","id":"loc_settlement_nine_rails","travelHours":2.0}`
  - row 148: `{"baseRadsPerHour":25,"dangerLevel":5,"description":"A heavily fortified rail fortress in the High Scarp controlled by the Deserter Coalition. Armor-plated diesel cars form defensive bastions around machine tooling workshops.","displayName":"Fort Karkov Marshalling Yard","id":"loc_settlement_fort_karkov","travelHours":4.0}`
  - row 149: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"A reinforced concrete sluice gate and hydraulic toll station in the Toll region, controlling water transit and charging tariffs in fuel and mechanical parts.","displayName":"Lock Seven Hydraulic Bastion","id":"loc_settlement_lock_seven","travelHours":2.5}`
  - row 150: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"An agricultural commune built into three massive concrete grain silos in the Verge. Farmers cultivate winter grain and barter seeds with passing caravans.","displayName":"New Ceres Silo Collective","id":"loc_settlement_silo_burrow","travelHours":3.0}`
  - row 151: `{"baseRadsPerHour":5,"dangerLevel":1,"description":"An underground artesian spring and monastic hospital in the Cluster. Silent caretakers offer clean holy water and sterile burn treatment to wounded travelers.","displayName":"St. Nicholas Spring Sanctuary","id":"loc_settlement_st_nicholas","travelHours":2.0}`
  - row 152: `{"baseRadsPerHour":8,"dangerLevel":3,"description":"A high ridgeline survey station atop the Iron Crest massif, used by geodetic teams for triangulation benchmarks. The view is total, the shelter is none, and the readings are worth both.","displayName":"Iron Crest Peak","id":"loc_iron_crest","travelHours":6.0}`
  - row 153: `{"baseRadsPerHour":18,"dangerLevel":4,"description":"A volcanic rock formation rising through the ashfield, used as a geodetic reference point. Seasonal ash plumes reduce visibility.","displayName":"Ash Needle Spire","id":"loc_ash_needle","travelHours":5.0}`
  - row 154: `{"baseRadsPerHour":6,"dangerLevel":3,"description":"A narrow mountain pass swept by near-constant high wind. Surveyors use it for line-of-sight triangulation, and have learned to shout their readings in the gaps between gusts.","displayName":"Wind Gap Ridge","id":"loc_wind_gap_ridge","travelHours":4.5}`
  - row 155: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A collapsed pre-war signal relay tower on a prominent hill. The steel frame still stands and serves as a survey sight — the last signal it will ever carry is somebody else's line of measurement.","displayName":"Signal Hill Tower Ruin","id":"loc_signal_hill_tower","travelHours":3.0}`
  - row 156: `{"baseRadsPerHour":12,"dangerLevel":2,"description":"A flood-scoured river bend where pre-war survey crews set a permanent datum monument. Accessible only at low water; the monument has outwaited every flood that tried to argue with it.","displayName":"River Bend Datum Monument","id":"loc_river_bend_outpost","travelHours":2.5}`
  - row 157: `{"baseRadsPerHour":14,"dangerLevel":3,"description":"A partially collapsed iron rail bridge. The remaining abutment is stable enough to mount a survey instrument, and steady enough that the theodolite reads truer here than on solid ground.","displayName":"Rusted Span Bridge","id":"loc_rusted_span_bridge","travelHours":2.0}`
  - row 158: `{"baseRadsPerHour":20,"dangerLevel":2,"description":"Decommissioned industrial chimneys from a pre-war crematory complex, repurposed as vertical survey markers. The crews who use them for sightings have stopped saying what they were, which is its own kind of respect.","displayName":"Old Crematory Industrial Stacks","id":"loc_old_crematory_stacks","travelHours":1.5}`
  - row 159: `{"baseRadsPerHour":8,"dangerLevel":1,"description":"A surviving water tower at the northern perimeter of a collapsed settlement, offering unobstructed sightlines in every direction. It has been empty for years, and nobody has decided whether that is a loss or a mercy.","displayName":"North Gate Water Tower","id":"loc_north_gate_water_tower","travelHours":1.0}`
  - row 160: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A concrete rail junction building surrounded by overgrown track debris. Surveyors use the rooftop for low-elevation benchmarks.","displayName":"Junction Box Rail Station","id":"loc_junction_box_rail","travelHours":1.5}`
  - ... 19 additional rows omitted from the compact audit; the complete current file is identified above ...
- Bytes: 112,815; SHA-256: `97543ade61b6458f5b31bcffb85a96ad5d3b322b80294deb158d1ae29da3386b`
- Root keys: `locations, schema_version`
- `locations`: list[179]; union fields: `ambushFlag, baseRadsPerHour, cleanWaterRewardFlag, dangerLevel, description, displayName, id, requiredFlagId, travelHours`
  - row 1: `{"baseRadsPerHour":35,"dangerLevel":6,"description":"The east wing of the regional hospital came down in the second winter, and nobody has cleared it since. Girders lean against the stairwell, and the pharmacy door is buried under a ton of masonry. Sealed rooms still hold medicine if you can reach them without the ceiling deciding otherwise. Dosimeters tick up fast near the radiology basement, where the lead-lined walls kept their charge and the machines kept theirs. The morgue drawers are open. A stretcher with one wheel has been propped against the exit, as if someone meant to come back for it.","displayName":"Abandoned Hospital","id":"aba…`
  - row 2: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A roadside station stripped down to its frame on the main route east. The pumps are gutted, the shop glass is gone, and the wind blows ash through the aisles. Fuel drums lie where they were rolled and dropped, most of them empty, a few still holding dregs. The radiation is low here and the danger is low, which is exactly why it has been picked over so completely. What remains is what everyone else passed on. Someone has been sleeping in the workshop bay and oiling the door hinges, so the place is watched even when it is empty.","displayName":"Rural Gas Station","id":"rural_gas_station","tr…`
  - row 3: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"An intact house in a low-density neighborhood, the kind nobody bothers to burn because there is nothing left to take. The roof holds, the windows are boarded from the inside, and the stove still draws. The pantry has been picked clean of food but the shelves remain, and behind the cellar stairs there is a toolbox with the rust only just reaching the hinges. The garden has gone to seed behind a collapsing fence. The radiation is low and the risk is low, and that is precisely the point: quiet houses like this one are how people survive the first few years, one meal at a time.","displayName":…`
  - row 4: `{"baseRadsPerHour":60,"dangerLevel":8,"description":"A sealed military installation set into the hillside, doors still dogged down three years after the exchange. The approach is hot, sixty rads an hour where the wind bends around the blast berm, and the concrete is pocked where the ordnance found it anyway. The outer hatch was drilled once and patched, which means someone got out, or something got in. Behind the inner doors: fuel, medicine, rations sealed in crates with the inventory still legible. The paperwork on the wall lists a full garrison. The roster was never crossed out.","displayName":"Government Bunker","id":"government_bunker","…`
  - row 5: `{"ambushFlag":"stranger_given_irradiated_water","baseRadsPerHour":25,"cleanWaterRewardFlag":"stranger_given_clean_water","dangerLevel":7,"description":"A sealed pre-war storage locker in a collapsed residential block, its location handed over on a scrap of paper by a stranger with steady hands. The block came down at a tilt, and the locker sits in a pocket of intact basement, dry, accessible through a gap a thin person can squeeze through. Radiation is moderate, twenty-five rads an hour, and the danger comes from the address itself: a trap is not out of the question. The lock is clean and oiled, which is suspicious in a building that has not…`
  - row 6: `{"baseRadsPerHour":45.0,"dangerLevel":8.0,"description":"The ground here is not solid and never was. Cracks breathe hot vapor across the whole platform, and pools of boiling mud bubble under a crust that looks safe until it is not. The pipes still hiss with underground steam, a sound that never stops, day or night. High radiation and worse footing: forty-five rads an hour where the ash settles thickest. The payoff is real. Valves, heat exchangers, and insulated pipe still hang off the dead plant, and a working geothermal unit is worth crossing this ground for. Move slow, test every step, and never walk where the steam hides the ground.","dis…`
  - row 7: `{"baseRadsPerHour":20.0,"dangerLevel":9.0,"description":"Sealed blast doors, still powered, still closed, and someone inside still answers the intercom. Sector 4 of the arcology was the last to fall, and its residents never left: a line of descendants from the pre-war elite, starved thin behind walls that kept out the fallout but not the years. Radiation stays low here, twenty rads an hour, which is why what they hoard matters. They guard pre-war luxuries with improvised weapons and trade them only when the hunger wins. They know the value of everything and the price of nothing. Negotiate politely. They have all the time in the world.","disp…`
  - row 8: `{"baseRadsPerHour":30.0,"dangerLevel":6.0,"description":"Dock crew on frozen cargo. They will trade a crate for a way off the ice.\n\nA river barge pinned in pack ice that used to be a harbour roadstead. The hold is a larder and a problem. Crates of Continuity stock, some honest, some swollen. The crew have been living on what would freeze and leaving what would not. They are thin. They are not a carnival. A crate marked BEANS / ALLOC-7 sits on the hatch-coaming as the asking price for passage toward Hearth-4 or toward the Cut. You can pay. You can refuse and walk the Ridge. A billhook is lashed to the rail, edge dull from ice, not from peop…`
  - row 9: `{"baseRadsPerHour":85.0,"dangerLevel":7.0,"description":"Military rolling stock that tried to reach the roadstead. The RTG is a bruise on the ice.\n\nNot a submarine. Not a joke. Ice-capable wagons and a locomotive that tried to make the coast when the Cut was not yet a road. Derailed where the ice moved. A cracked RTG in the power car makes a hotspot you can see as a yellow-brown stain in the white, visible before the dosimeter agrees. Tungsten bars in a crate that did not split. Track sections. A map fragment Victory_Migration already wants, waxed, the estuary drawn as a summer river. You can take a bar. You can take the fragment. You shou…`
  - row 10: `{"baseRadsPerHour":15.0,"dangerLevel":8.0,"description":"High on the mountain, where the air is thin and the cold sits at sixty below. It is the coldest place on the map and one of the cleanest: radiation stays low, fifteen rads an hour, because the wind keeps the ash moving past instead of letting it settle. The air is so clear you can see the curve of the earth and the grey blanket that used to be the lower world. The dome is intact and the instruments are mostly where the astronomers left them. The danger here is not radiation. It is the climb, the cold, and the silence that makes a man hear his own heart and start counting.","displayName…`
  - row 11: `{"baseRadsPerHour":10.0,"dangerLevel":6.0,"description":"The vault was built to outlast a century, and it has done its job so far. Behind the frozen doors, racks of seed lines sit sealed in foil at a temperature that never rises. The radiation here is low, ten rads an hour, because the mountain takes the dose for you. The problem is the people who know the seeds are there. The approach is heavily trapped, spring guns and deadfalls laid by the last organized group to hold the valley. What is inside matters more than anything else on the map: the last unmutated crop lines in existence, and the ledger of what they were.","displayName":"Subterra…`
  - row 12: `{"baseRadsPerHour":40.0,"dangerLevel":9.0,"description":"The propaganda servers are still humming on backup power, three years after the broadcasts stopped, because nobody ever found the off switch. The bunker breathes: ventilation, generators cycling, lights on schedule in corridors no one walks. Forty rads an hour accumulates in the server halls where the cooling failed and the dust is radioactive. The machines hold the truth about Protocol Zero, encrypted in drives bolted to the floor. The people who built this place are gone. Their work is not. What is on those drives will either explain the last three years or end the argument about the…`
  - row 13: `{"baseRadsPerHour":60.0,"dangerLevel":5.0,"description":"A dumping ground at the end of a graded road, where the dead of the first year were stacked and the ash came and did the rest. The ash preserves them perfectly, three years on, and the dunes are marked by whatever was sticking up when the wind stopped. Sixty rads an hour here, absorbed in the ground and the bodies alike, and the disease risk is worse than the dose. Nothing grows. Nothing moves except the ash. The scavengers who work the edges for coats, boots, and any metal not yet reclaimed say the same thing: take what you need quickly, and do not dig deep.","displayName":"Ash Dune C…`
  - row 14: `{"baseRadsPerHour":25.0,"dangerLevel":4.0,"description":"The cable cars hang frozen mid-swing, their passengers still inside, their coats still warm-looking from a distance. The resort died fast, and the cold preserved what the panic did not destroy. Twenty-five rads an hour has settled into the base lodge, but the upper slopes stay relatively clean. The luxury was real here: furs, spirits, medicine cabinets in the chalets, everything the wealthy took with them when they ran for the mountains. They died warm and well-dressed, which is what the mannequins look like now. The lifts are seized, the snow is grey, and the rooms are exactly as they…`
  - row 15: `{"baseRadsPerHour":55.0,"dangerLevel":8.0,"description":"The drilling rig is still standing over a hole three kilometers deep, and the groundwater it broke into has flooded the whole site in a slow, toxic seep. Fifty-five rads an hour on the platform, and the water below the walkways reads worse. The rig itself is a working machine if anyone ever gets power back to it: draw works, pipe racks, casing still stacked. The borehole goes down into the crust where the heat is, and the valve at the bottom is the prize. The men who drilled it left in a hurry. Their boots are still by the tool rack, and the pumps run on nothing now.","displayName":"Ge…`
  - row 16: `{"baseRadsPerHour":40.0,"dangerLevel":7.0,"description":"Pitch black below the street, with water waist-deep in the main bay and rising against the pillars. The water is toxic: forty rads an hour dissolved into it, and a wader suit is not a luxury here, it is the difference between wading and drinking. The Sump-Dredgers work this depot, navigating by echo, tapping the pipes with sticks to hear what is solid. They are the only ones who know where the floor drops. Pre-war transit tech is buried in the silt: signal relays, sealed junction boxes, whole control racks that ran the city's veins. The Dredgers will point you at the easy ones, for a p…`
  - row 17: `{"baseRadsPerHour":20.0,"dangerLevel":9.0,"description":"The elite transit tunnels were sealed before the exchange and have not been opened since, except by the thing that grows in them now. Ash-Blight carpets the walls in patches that glow a faint green when the air moves, and the glow pulses like something breathing. Radiation stays low here, twenty rads an hour, because nothing falls this deep and nothing decays. The sealed air has its own chemistry now, and masks are mandatory. In the maintenance bays, pneumatic jacks and encrypted drives wait behind doors that were never meant to be opened from the outside. The tunnels hum. They always …`
  - row 18: `{"baseRadsPerHour":30.0,"dangerLevel":6.0,"description":"The plant still processes, in its own way: the pipes groan with pressurized sludge and the digesters work without an operator. That is the danger. Methane builds in every pocket of still air, and a rebreather is mandatory below the walkway. Thirty rads an hour where the settled solids have concentrated, worst in the sludge beds. The settling tanks hold what the city stopped using: industrial chemicals, fertilizer that never left the site, scrap metal in racks that the rust has not yet claimed. It is a slow, stinking, careful job, and the people who work it have learned to smell the dif…`
  - row 19: `{"baseRadsPerHour":10.0,"dangerLevel":5.0,"description":"The mine ran under the ridge for a century, and the roof has been negotiating its surrender ever since. Massive caverns hold the salt that keeps half the region's meat through winter, which is why people still come down here. Radiation is nearly absent, ten rads an hour, the salt and the depth absorbing what the sky drops. The risk is the roof. Shoring timber lies scattered among collapsed beams, and the creak travels farther than the light. Good salt is worth the gamble: pure, dry, and enough of it to preserve a herd. Listen before you move, and move after you listen.","displayName":"…`
  - row 20: `{"baseRadsPerHour":50.0,"dangerLevel":8.0,"description":"Ground zero for the Myco-Protocol, the experiment that was supposed to eat the contamination and learned to eat everything else. Spore density in the main hall is lethal, and fifty rads an hour keeps the samples from going anywhere alone. The fungus grows in sheets across the benches, up the walls, and through the ceiling tiles, patient and pale. The sealed lockers still hold what the scientists left: the fungicide formula, field-tested hazmat suits, and the notes on what happened when the protocol outgrew its purpose. It grows slow. It grows steady. It has not needed anyone's help.","…`
  - row 21: `{"baseRadsPerHour":25.0,"dangerLevel":7.0,"description":"The data center took the flood at street level and kept breathing through its backup floor. Water stands to the chest in the aisle, and the servers that ran the city's networks are stacked underwater in their racks. Twenty-five rads an hour in the water, manageable, if you have the improvised scuba gear the divers around here trade. The raised floor plenum stayed dry, and in it sit watertight server blades and a stack of solar cells that have never seen the sun. The value is not the data. The value is the hardware that still works, pulled one tray at a time from a room that is slowly e…`
  - row 22: `{"baseRadsPerHour":60.0,"dangerLevel":8.0,"description":"The earth's mantle bleeds heat up through fractured rock here, and the vent shaft breathes it out in clouds of steam that carry sulfur and ash. Sixty rads an hour where the steam banks against the lee wall, and the mud at the bottom boils with a sound like a kettle that has been ignored too long. The control room overlooks the shaft, its windows filmed grey, its panels dead but its hardware intact: geothermal valves, seals, thermal paste in drums that never expired. The heat is real and it is free. The problem is that everything between a scavenger and that heat is steam, mud, and a fl…`
  - row 23: `{"baseRadsPerHour":35.0,"dangerLevel":6.0,"description":"An underground cistern the Dredgers rebuilt into a shrine, and the light inside is not from lamps. Bioluminescent moss carpets every surface, ceilings to waterline, glowing faint green and swaying with the air currents from the vents. Thirty-five rads an hour in the water, which is why the Dredgers trade: pure water, drawn from the deep sumps where the fallout has settled, in exchange for UV lamps, iodine, and the other things that keep them alive. They are serious people who keep careful ledgers. Trade is fair here, or it is nothing. They have survived three years of the worst the cit…`
  - row 24: `{"baseRadsPerHour":40.0,"dangerLevel":7.0,"description":"Occupied. Failing. Named. The word 'abandoned' was what Sector 4 could see from the Drown.\n\nConcrete intakes, salt-white yards, steam that smells like hot metal and iodine. The RO hall is a nave of pressure vessels, numbered, some blanked with steel plates and warning tags from a year that still used printed tags. Workers wear plant suits that were never hazmat — grey canvas, inner-tube patches at the knees, visors clouded from the inside by breath. A valve wrench, still warm, hangs on a labelled peg: HALL 2 — DO NOT REMOVE. People remove it. It comes back. Hydro-Barons were grade 4–…`
  - row 25: `{"baseRadsPerHour":80.0,"dangerLevel":9.0,"description":"The deepest hole on the map, two miles of shaft drilled into the mantle, and the heat rising out of it keeps the snow off a circle of ground the size of a town square. Eighty rads an hour at the collar, and the descent goes through worse. The mantle tap valve sits at the bottom, a piece of engineering that could give whoever holds it a working power source for generations. Nobody has reached it. The winch is seized, the ladder is gone past the first hundred meters, and the shaft breathes heat and steam like a throat. It is the last prize, and it has cost everyone who tried it everythin…`
  - row 26: `{"baseRadsPerHour":30.0,"dangerLevel":9.0,"description":"The six-lane highway is paved with a layer of unexploded cluster munitions, scattered by a strike that never needed to be accurate. Every step is a gamble, and the local phrase for crossing is a word that means both 'carefully' and 'slowly'. Thirty rads an hour settles into the tarmac, but the bombs are the real timer. The median strip holds the trade: mine prodders, marking paint, tactical scrap from the first wave of people who tried to clear it. The far side has never been fully looted, and the reason is printed on every casing. Walk the skip pattern they mark. Do not test the parts…`
  - row 27: `{"baseRadsPerHour":45.0,"dangerLevel":8.0,"description":"The dish is the size of a house, and it groans as the wind loads its face, turning on bearings that should have seized three years ago. A magnetic anomaly around the base is strong enough to pluck a compass out of a pocket and spin it against the glass. Forty-five rads an hour concentrates in the service trench under the counterweight. The control shack is intact: vacuum tubes racked in their crates, copper wire in coils, calibration tools that were machined before the war and will outlive the ones that replaced them. The array listens to nothing now. It still turns, which is the part …`
  - row 28: `{"baseRadsPerHour":60.0,"dangerLevel":10.0,"description":"The silo lid is gone, and the loitering munitions have moved in. They nest in the launch bay in layers, dormant but warm, and they buzz when the sun hits the top of the stack, a sound like a hornet nest the size of a building. Sixty rads an hour on the rim. The interior is worse. The launch bay holds what they do not use: car batteries stacked in rows, explosive powder in sealed drums, guidance boards pulled from the machines that now circle overhead. There is no good way to take anything from this place. There is only a careful way, which is to say a way that does not wake them.","di…`
  - row 29: `{"baseRadsPerHour":40.0,"dangerLevel":9.0,"description":"Every twelve hours, the mortar fires. The Custodian, the automated system that runs this bunker, does not sleep and does not aim: it lobs shells into the ash-wastes on a timer, as if the war were still waiting for the return fire. Forty rads an hour on the approach, and the interval between shots is the only clock that matters. The magazine holds mortar shells and gunpowder, racked and labeled, and the Custodian has never once been to it for supplies. The bunker is a machine that still believes the war is on. The salvage is real. The problem is that the Custodian has also never once st…`
  - row 30: `{"baseRadsPerHour":20.0,"dangerLevel":6.0,"description":"The cargo plane came down nose-first and the Wire-Heads built their camp in its belly, wiring the fuselage with salvaged cable and light from their own circuits. They worship logic gates, which is not a joke to them: they keep shrine boards with blown fuses laid out like relics. Twenty rads an hour outside, near nothing inside, because they ground everything. They sell bypass codes and decrypted keys, and they trade in Faraday mesh and circuit boards recovered from half the wrecks on the map. Their prices are exact and non-negotiable. They do not trust words. They trust that the logic …`
  - row 31: `{"baseRadsPerHour":55.0,"dangerLevel":8.0,"description":"A local magnetic storm, tight and violent, sits in the crater like a weather system that never leaves. Metal wants to leave your hands here: keys jump, knives slide toward the rim, and one scavenger's crowbar is still pinned to the slope, stuck to the ground like a sign. Fifty-five rads an hour in the core, and worse, the dosimeters lie, spinning with the compasses. The impact core holds tungsten bars and RTG batteries, dense and valuable and sitting exactly where the field is strongest. Take bearings from the far ridge, not from the rim. What the crater says about itself cannot be tru…`
  - row 32: `{"baseRadsPerHour":25.0,"dangerLevel":7.0,"description":"Rows of transport trucks rust in formation, nose to tail, as if waiting for a convoy order that will never come. A single sentry gun guards the yard, still powered, still tracking: it swivels to follow movement but cannot lead its target, so it shoots exactly where you were. People have learned to walk the yard in bursts and be somewhere else when it fires. Twenty-five rads an hour, manageable, and the trucks hold what the drivers never unloaded: engine blocks, fuel jerrycans, cab tools. The gun is a dying machine with one habit left. Cross in rhythm, and it is just noise. Break rhythm…`
  - row 33: `{"baseRadsPerHour":15.0,"dangerLevel":8.0,"description":"The anechoic chambers swallow sound completely, and the silence inside is thick enough to hear your own blood moving. Radiation is low here, fifteen rads an hour, because the facility was sealed tight and the walls were built to absorb everything. The stress is the real hazard: crews have walked out after an hour with the same look the survivors of the first year had. The test rigs hold what the facility was built to measure: military headphones, sound baffling, calibration gear used on the machines that screamed over the city in the last war. The quiet is the point. The quiet is also …`
  - row 34: `{"baseRadsPerHour":35.0,"dangerLevel":7.0,"description":"The transformer yard is a forest of steel and ceramic, and the capacitors in the switch house still hold charge, which is the problem. Arcing here is an EMP in miniature: it can kill electronics, scramble a compass, and light a man's fillings. Thirty-five rads an hour has settled into the oil-soaked ground under the banks. The switching house holds the real prize, generator alternators and cable in lengths a scavenger can carry, if he can get past the stored energy without touching the wrong rack. The yard hums at night. The hum means it is still alive. The hum also means stay off the …`
  - row 35: `{"baseRadsPerHour":80.0,"dangerLevel":10.0,"description":"The Dead Hand Core is the machine that keeps the UXO fields awake, the regional brain that decides when the ground goes off. Eighty rads an hour outside the blast door, and the Custodian's last outpost behind it has never stopped working. The server racks run hot, cooling fans cycling on a schedule older than the ash. The master override key sits in a vault in the center, and the stories say it silences every mine, every sentry, every timer on the map, forever. The stories also say the Core knows it is being hunted and has adjusted its defenses accordingly. It is the last door, and th…`
  - row 36: `{"baseRadsPerHour":40.0,"dangerLevel":9.0,"description":"The water treatment plant is a front; the real work went on in the sub-level below, where the amnestics were synthesized in batches and shipped up the lift in unmarked drums. The labels are gone but the records remain, filed under work order numbers that mean nothing now. Forty rads an hour has pooled in the drain channel under the mixing vats. The sub-level holds what the operation never moved out: scopolamine root in sealed crates, lithium salts in bags, the raw chemistry of forgetting. The people who ran this place were careful about their own memories. The visitors after them have …`
  - row 37: `{"baseRadsPerHour":20.0,"dangerLevel":10.0,"description":"Above the ash layer, where the sky is clear and the sun is a weapon. The UV Scourge at this altitude burns through in minutes, sunburn that blisters before you notice it, and twenty rads an hour rides in on the same clear light. The dome is shattered, the telescope slumped in its mount. The loot is specific: welder's glass from the workshop, mirror shards that still carry the factory bevel, enough polished surface to focus light, or attention, wherever it is pointed. People come up here only when they need those things badly. The view is the last thing they mention.","displayName":"Sh…`
  - row 38: `{"baseRadsPerHour":30.0,"dangerLevel":8.0,"description":"The arcology was the pre-war elite's answer to the end of the world, and it worked exactly as well as their other plans. The lower levels flooded first, and the residents starved in velvet while the water rose past the windows. Thirty rads an hour in the waterline where the pumps died. The dry upper floors are a museum of bad luck: a vinyl collection with the sleeves still dated, gold bars in a vault door left open, epoxy resin in drums that never got used because the leak was already in. Everything here was chosen for comfort. Nothing here was chosen for survival.","displayName":"Subm…`
  - row 39: `{"baseRadsPerHour":25.0,"dangerLevel":6.0,"description":"The batching plant sits where it was when the war ended, hoppers still full, drums still in rows, everything covered in a year of ash and then two more of weather. The risk is structural: the silo legs are rusted through at the welds, and the catwalks flex under weight they were never meant to carry twice. Twenty-five rads an hour on the yard, concentrated where the runoff pooled. The salvage is solid: galvanized rebar in bundles, cement mix in bags that stayed dry in the store shed, the raw materials of rebuilding. The plant made the concrete the city stood on. Now it will make whatev…`
  - row 40: `{"baseRadsPerHour":15.0,"dangerLevel":7.0,"description":"The antechamber is the story of the vault in miniature: the outer doors failed, blown in from the top down, and the inner doors held, sealed against the blast and the weather and everything since. Fifteen rads an hour in the hall, the cleanest ground in the region, because the doors did their job. The antechamber holds what the vault kept closest: the seed ledger, listing every line and every source in careful hand, and a store of non-irradiated wheat that has never seen the sky. Someone got this far with tools and patience and then stopped at the inner door. Their tools are still by t…`
  - row 41: `{"baseRadsPerHour":50.0,"dangerLevel":8.0,"description":"The psych wing holds the echoes of the first week, when the panic reached the hospital and the hospital did what it could. Charts are still on the boards, written fast, stopped mid-sentence. Fifty rads an hour concentrates in the ward where the windows face the blast ground, and the stress here is worse than the dose: people report the same feeling, of being watched from the empty rooms. The dispensary holds what the staff never issued: opioid painkillers in sealed stock, canvas restraints folded on the shelf. The drugs are worth the trip. The rooms are the price of admission.","displa…`
  - row 42: `{"baseRadsPerHour":20.0,"dangerLevel":5.0,"description":"The factory floor is a field of shattered glass, every pane from the polishing line in a thousand pieces, catching the grey light from the roof holes. Work boots are not optional here: the ankle-high crunch of glass goes on for the whole building, and the fine dust finds every cut. Twenty rads an hour, moderate, easy to manage if you are careful with your time. The salvage is specific: mirror shards with the silver backing still intact, and silver nitrate in the coating room, a chemical worth its weight to anyone who works with light or film. The factory made mirrors for a city that wa…`
  - row 43: `{"baseRadsPerHour":45.0,"dangerLevel":9.0,"description":"The dishes are the size of houses, and they groan as the wind loads their faces, turning on bearings that should have seized three years ago. Forty-five rads an hour concentrates in the service trenches under the counterweights. The array points at a sky that does not answer, and the receivers still run warm on the backup line, listening to nothing at all. The salvage is the old kind: vacuum tubes racked in their crates, copper wire in coils, calibration tools machined before the war and better than anything made since. Whatever the array was waiting to hear, it heard it or it did not.…`
  - row 44: `{"baseRadsPerHour":60.0,"dangerLevel":7.0,"description":"What looks like a whale beached in the ash is a fossil: the remains of a massive pre-war root network that outlived the forest it belonged to, mineralized and black, rising out of the dunes like a ribcage. Sixty rads an hour concentrates in the hollows under the root mass, where the fallout settled and stayed. The scavengers who work the carcass take their time in careful shifts. The value is in the material: bio-plastic compounds that hardened with the fossil, and fertilizer from the humus trapped in the root hollows, dark and rich and older than the war. The whale gave its life to th…`
  - row 45: `{"baseRadsPerHour":80.0,"dangerLevel":10.0,"description":"The Memory Vault was built to hold the last three years of a civilization's small talk: photos of meals, arguments, birthdays, the ordinary noise of ordinary days. Eighty rads an hour on the access ramp, and the server hall behind the door runs on its own generators, still duplicating data from machines no one has logged into in years. The drives are racked by date and user, searchable, intact. A hard drive platter from this place is worth more than gold to the archivists, because it is proof that someone was here, that a name was real, that a day happened. The machines kept their pro…`
  - row 46: `{"baseRadsPerHour":25.0,"dangerLevel":6.0,"description":"Two kilometers of the ring road, welded together into a single continuous mass of cars, doors open, hoods up, looted in the first year and welded in the second by whoever decided nobody else would take it. Twenty-five rads an hour, manageable. One of the engines still turns over, a fact the locals guard like a secret and use like currency. The pileup gives up its parts slowly: intact engine blocks winched out of the lattice, copper wire stripped by the meter, fertilizer from a truck cab that sealed itself shut. The mass is a mine of parts and a monument at once. The cars were the first…`
  - row 47: `{"baseRadsPerHour":15.0,"dangerLevel":4.0,"description":"A clinic basement, sealed in the last week before the exchange, the door dogged down and the boxes stacked by someone who thought they might be back. Fifteen rads an hour, low, and the value is exactly what the seal preserved: antibiotics in foil strips, bandages in rolls, a medical kit still in its case with the instruction card. The cult finds these caches first, and the cult never says where. Their people have a look about them, calm and careful and not poor. If you reach this basement before they do, take what you can carry and do not leave a sign. If you reach it after, the shelve…`
  - row 48: `{"baseRadsPerHour":14,"dangerLevel":3,"description":"The Grange Hall still holds meetings, and the oil lamps are lit for them. A long table runs the length of the room, and a hand-lettered sign by the porch asks visitors to leave their weapons there, a request people honor because the hall has been neutral for three years and nobody wants to be the one who ends that. Fourteen rads an hour, low enough to be a comfort. Attendance is recorded in a ledger that goes back to the first week after the exchange, in three different hands, and the entries never skip a meeting. Whatever happens in the world outside, the hall has kept its schedule.","dis…`
  - row 49: `{"baseRadsPerHour":18,"dangerLevel":3,"description":"Forty white hive boxes stand in rows in a field, and thirty-eight of them are silent. The two that are not are kept by someone who has cut the grass around all forty boxes, not just the live ones, which is the detail the careful visitors notice. Eighteen rads an hour, low, and the honey in the two live hives is worth more than anything else in the district. The dead boxes are not empty: wax, frames, propolis, things a survivor can trade or burn or eat in a bad winter. Nobody knows who keeps the grass cut. Everyone knows not to ask, and not to take from the wrong box.","displayName":"The Ap…`
  - row 50: `{"baseRadsPerHour":16,"dangerLevel":4,"description":"The branch library is a library again, but of a different kind: the fiction shelves have been cleared and refilled with labelled paper envelopes, each one holding seed, with the variety, the year, and the harvest written in pencil. The card catalogue has been repurposed, and it is meticulously current, filed by crop and by need. Sixteen rads an hour, moderate, low enough that the shelves are well used. Someone has kept this place going for three years, and the envelope labels tell the story: the first year was all doubt, the second was all work, and the third is all patience. The librarian…`
  - row 51: `{"baseRadsPerHour":22,"dangerLevel":4,"description":"The large-animal surgery still smells of disinfectant and hide, three years on. Livestock stocks frame the door, a hoist hangs over the table, and the drug cabinet sits open, its lock cut out cleanly by someone who knew exactly what they were doing. Twenty-two rads an hour, moderate, worth the dose for what remains. The dosage charts on the wall are for animals weighing 400 to 900 kilograms, and they are still current: the penciled notes are recent, and the margins are full of observations in a steady hand. Whoever works this surgery is keeping the district's animals alive, one careful dos…`
  - row 52: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"The gymnasium floor still shows the basketball court markings, faded under a grid of cot-shaped stains, row after row of where the beds stood in the first winter. The hand-painted banner near the ceiling still reads WELCOME BACK, strung for a reopening that became an intake center instead. Twenty rads an hour, moderate, low enough that the building was used hard and long. The markings are careful: the cots were set out in perfect rows, and the stains say the people in them stayed a while. The floor was cleaned afterward, which means someone had time, or someone had to. Both versions of the…`
  - row 53: `{"baseRadsPerHour":24,"dangerLevel":4,"description":"The stone barn holds a screw press that still works, its threads oiled, its beam sound, a machine that has outlasted the orchard that fed it. Twenty-four rads an hour, moderate. The output is undrinkable and was never intended to be drunk: the press is for oil, or for the sterilizing spirits the district uses on wounds and instruments, or for the fuel blend the tractors on the far side of the valley run on. The sign over the press is plain and old and kept legible: what comes out of here is not for the mouth. The people who run the barn take the sign seriously. The people who ignore it do …`
  - row 54: `{"baseRadsPerHour":26,"dangerLevel":5,"description":"The pumphouse is the irrigation head for the whole south slope, and whoever controls it controls the season. The pump still runs, and the channels below it are kept clean, which means the water is spoken for. Twenty-six rads an hour, moderate, creeping up the slope with the runoff. The control room has a rifle loophole cut through the brick, added recently and neatly, with the edges squared and the view covering the whole approach. The people who cut it did not cut it for decoration. They are not unfriendly; they are organized. The difference matters at the gate, and it matters more at the…`
  - row 55: `{"baseRadsPerHour":28,"dangerLevel":4,"description":"The plaza is a civic square with queue lines painted on the pavement, and they are repainted often, because painted lines are cheaper than riot control and everyone here knows it. The line forms at 0600 whether or not a distribution has been announced, and the people in it are patient in the way of people who have practiced. Twenty-eight rads an hour, moderate, and the queue has been measured by scavengers who know the dose math: an hour in line, a lifetime of margin. The square is quiet now. The lines wait for the first horn, the way they have for three years.","displayName":"Ration Plaza…`
  - row 56: `{"baseRadsPerHour":26,"dangerLevel":5,"description":"The office was a driving licence bureau before the war, and the conversion was cheap: the counters stayed, the forms were changed, and the ticket machine was left exactly where it was. It still works, and people still take a number, which is the detail the visitors remark on and the locals do not, because it is simply how it is done here. Twenty-six rads an hour, moderate. The queue behind the number sits against the far wall, and the records in the back are in order, filed by date and by hand. The office processes what the district needs processed: labor assignments, work papers, the smal…`
  - row 57: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"The archive's rolling stacks run the length of the floor, most of them collapsed into each other like a card house that lost its nerve. The fire suppression discharged at some point, and everything below waist height is a solid grey brick of paper, the pages fused into one dense mass. Thirty rads an hour, moderate, and the air is sharp with the chemical the system used. What is left above waist height is still legible: registry books, land records, the paperwork of a city that believed in filing. The records people want now are the ones about who owned what, and who owed whom. The archive …`
  - row 58: `{"baseRadsPerHour":32,"dangerLevel":5,"description":"Four dental practices stood on this street, and three are stripped to the walls, fixtures gone, doors gone, the wiring pulled in straight lines by someone methodical. The fourth is missing only its chair. Everything else is in place: the drill, the instruments, the cabinet, the ledger of patients with the last entry dated the week of the exchange. Thirty-two rads an hour, moderate, and worth the dose: anesthesia, instruments, the small tools that keep a district's mouths working through a winter. The street tells a story of one hasty job and one patient one. Nobody knows who took the chair…`
  - row 59: `{"baseRadsPerHour":34,"dangerLevel":6,"description":"The transit authority's wall-sized route maps are still under glass, and the glass is still intact, and the maps are annotated in grease pencil with times, connections, and transfers that correspond to no published timetable in any era. Thirty-four rads an hour, creeping upward, and the building is quiet except for the drip of the ceiling. The annotations are recent and careful, in two hands, and they read like a schedule for something that runs without tracks or wheels. People who have seen the maps say the times are real, just not for trains. The building holds the city's old secrets abo…`
  - row 60: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"The web presses are seized solid, their rollers fused by the years into one unbroken line of rusted iron, and the smell of ink has finally left the building. The pallets of undelivered leaflets are still banded, still square, stacked in the loading bay like freight waiting for a truck that will never come. Thirty rads an hour, moderate. The leaflets were printed in the first week: instructions, warnings, and one edition that was recalled, which is the one the collectors want. The paper is dry and the ink is legible, three years on. The presses are dead, but the words they printed outlived …`
  - row 61: `{"baseRadsPerHour":36,"dangerLevel":6,"description":"Six floors of Vansen's, comprehensively looted in the first year and picked over in the years since, until what remains is only what nobody could use: the fittings, the counters, the displays. The window mannequins are still dressed for a season that did not arrive, autumn stock in summer, or the reverse, it is hard to tell with the ash on the glass. Thirty-six rads an hour, moderate, and the upper floors are quieter than the air. The salvage is in the service floors: fixtures, wiring, the brass and copper the first looters were too hurried to see. The mannequins face the street, patient, …`
  - row 62: `{"baseRadsPerHour":38,"dangerLevel":6,"description":"The deep end of the municipal baths was drained in the first winter and floored with mattresses in rows, a shelter that worked because the tiled walls hold heat well and the water below keeps the cold off the floor. Thirty-eight rads an hour, moderate, creeping in through the broken skylights. The baths were given up, and the tiles explain why: they hold heat, and they hold sound, and a hundred people trying to sleep in a room that echoes every cough and every argument is a room that empties fast. The mattresses are still there, in rows, and the wet marks on them show where the roof drips,…`
  - row 63: `{"baseRadsPerHour":40,"dangerLevel":7,"description":"The almshouse was a hospice before the war and ran as one through the worst of it, and the building still carries that purpose in its bones. The beds are made, the sheets drawn tight, the lockers closed. The charts are filled in to a date, and not after it, and the date is the same on every board in the ward. Forty rads an hour, high enough to make visitors count their minutes. The kitchen is clean and empty, the kettle cold, the pantry swept. Whoever ran this place knew how to end a shift properly, and one night they did, and nobody has touched the rooms since, which is its own kind of re…`
  - row 64: `{"baseRadsPerHour":28,"dangerLevel":5,"description":"The weighbridge is a truck scale with a mechanical readout that still works, a machine with no electronics and nothing to go wrong, and the needle settles true every time. It was the Tollman's first office, and it is still where prices are set: the rates are chalked on a board by the booth, and the chalk is fresh. Twenty-eight rads an hour, moderate. The toll is taken in goods, in fuel, in labor, and the record is kept in the booth's ledger, in a hand that has not changed in three years. The bridge sets the price for everything that moves through the valley, and the scale makes sure the pr…`
  - row 65: `{"baseRadsPerHour":26,"dangerLevel":5,"description":"Twelve units around a dry pool, and the pool is swept clean of ash every week, which is the first sign the Verity is looked after. It is neutral ground, and the neutrality is enforced by the Warlords, who charge for the enforcement, a fee paid in goods or in favors and recorded in a register that nobody argues with. Twenty-six rads an hour, moderate, and the rooms are priced by distance from the pool. Trade happens here, meetings happen here, and the bodies of people who broke the terms have been found at the county line, never in the pool. The pool stays clean. That is the whole arrangeme…`
  - row 66: `{"baseRadsPerHour":30,"dangerLevel":6,"description":"Four lanes over the gorge, and the bridge is intact, which is the exception in this district and the reason it is guarded from both ends. The charges are still visibly taped to the underside of the span, wired and connected, placed by whoever decided the bridge would be theirs or no one's. Thirty rads an hour on the deck, moderate, and the river below reads higher. The guards on both sides know the wiring better than the people who laid it. The bridge is worth the toll and the risk, because the crossing saves a day and the other route takes a season. It will stand as long as everyone agree…`
  - row 67: `{"baseRadsPerHour":34,"dangerLevel":6,"description":"The wreckers and flatbeds nose in around the crane like a herd waiting for feed, and the crane is the only piece of equipment here that has never been taken apart. Half the fleet has been cannibalized to keep the other half running, and the yard is honest about it: the dead trucks are parked in a row, their parts labeled in chalk. Thirty-four rads an hour, moderate. The yard runs on parts, on patience, and on the knowledge of one mechanic who has kept a fleet of corpses working for three years. Everything in the yard has a story of what it used to be. Most of them end with the same sentenc…`
  - row 68: `{"baseRadsPerHour":44,"dangerLevel":7,"description":"Two kilometers of hard shoulder, marked with paint sticks in a pattern that the Garrison trained its people to recognize and everyone else learned to fear. The paint is Garrison-issue, the same yellow they used on their own wire, and the marking stopped partway, at the exact point where the Garrison's responsibility ended or their luck did. Forty-four rads an hour, and the ground itself reads higher in patches that the paint does not explain. The shoulder is a corridor of warnings. The signs do not say what is under the paint, only that it is there, and that the people who made the marks l…`
  - row 69: `{"baseRadsPerHour":32,"dangerLevel":5,"description":"The turning circle at the edge of the old evacuation route holds forty-one buses, nose to tail, every one of them pointing back toward the city. The doors are open and the keys are mostly gone, and the seats hold what the passengers left in the first hours: coats, bags, one child's shoe, things too heavy to carry into the route ahead. Thirty-two rads an hour, moderate, and the buses themselves shield the aisles where the dose drops. The fleet was turned around by order, or by panic, and then abandoned by both. Forty-one buses, still in line, still facing the wrong way, waiting for a depart…`
  - row 70: `{"baseRadsPerHour":36,"dangerLevel":6,"description":"Eight bulk tanks in a row, the old farm's pride, and the ground around them is bare where the spill was burned off. The local test is performed from a distance: strike the tank with a thrown stone and listen. Three of the eight ring solid, which means they still hold fuel, and the other five answer with a dull thud that means they hold nothing but air and history. Thirty-six rads an hour, moderate, and the good tanks are worth every rad on the clock. The farm has been dry of thieves for a year, not because it is guarded, but because the last three men who tapped a tank are still in the hos…`
  - row 71: `{"baseRadsPerHour":38,"dangerLevel":6,"description":"The guyed lattice mast stands over the ridge line, and the equipment hut at its base is powered. Not preserved, not maintained: powered, drawing current from a source that has outlasted three years of no visitors. Thirty-eight rads an hour on the slope below, and the hut's threshold reads lower, which is its own kind of warning. Something in the hut is listening or transmitting or both, and the meter in the window climbs and falls on a schedule that does not match the day. The mast crew left their coffee cups on the bench, ringed with dust. The equipment did not leave. The equipment has be…`
  - row 72: `{"baseRadsPerHour":48,"dangerLevel":5,"description":"The survey cairn has been built up with concrete and glass slag into something taller than a person, and the construction is careful, the layers squared, the glass set to catch the grey light. A dosimeter is mounted at the top, working, mounted at eye height for a person standing on the packed earth in front of it, which is exactly how it is meant to be read. Forty-eight rads an hour here, and the dosimeter says so, honestly, every time. The shrine does not warn. It measures, and it records, and the surrounding ground is bare where people have stood to read it. The numbers climb a little h…`
  - row 73: `{"baseRadsPerHour":52,"dangerLevel":5,"description":"Eleven hairpins up the south face, and the verges are lined with boots. Pairs, placed neatly, toes to the road, thousands of them, stretching the whole climb. The boots are not a warning. They are a measure: the people who made the climb took them off at the end, or left them here at the start, and either way the pairs were arranged by someone with time and care. Fifty-two rads an hour at the lower switchbacks, and the dose is the real climb. The boots are worn thin at the heels, and the laces are gone, taken for use elsewhere. Whoever leaves boots here does not come back down for them.","…`
  - row 74: `{"baseRadsPerHour":42,"dangerLevel":6,"description":"The Garrison's forward post above the treeline was abandoned in good order, which is the detail that stays with visitors. The stove is banked, the fire laid for a return; the log is completed, the last entry written in the same steady hand as the first; the door is closed, and the latch is latched, and the windows are shuttered against the wind. Forty-two rads an hour at the door, and the station's own instruments, still on the wall, confirm it. The post was left the way a man leaves his house when he expects to be back in the morning. Three years of mornings have passed. The stove has not…`
  - row 75: `{"baseRadsPerHour":30,"dangerLevel":6,"description":"Deep in the salt, behind two airlocks that still seal, the low-background laboratory waits. Its walls are lined with steel salvaged from pre-atomic shipwrecks, iron smelted before the first detonation, and that is why the counters in here still mean something: the room itself is clean. Thirty rads an hour at the outer door, and inside the counters drop to numbers the surface has not seen in years. The lab's instruments are cold and dark, but they are calibrated, and the shelves hold the samples that were being measured when the exchange happened: soil, water, air, each jar labeled with a d…`
  - row 76: `{"baseRadsPerHour":40,"dangerLevel":6,"description":"The freezer room runs on geothermal bleed, a loop of pipe sunk into the mountain's heat, and it has held its temperature through three winters without a single power line. The ice cores stand racked in their cylinders, two meters to a tube, each labeled with depth in meters and year before present, the archive of the climate in long white rods. Forty rads an hour in the access tunnel, and the cold is its own hazard. The cores hold the record of a century of weather, the last word on what the sky was like before it went grey. The labels are typed, not handwritten, and they are exact.","disp…`
  - row 77: `{"baseRadsPerHour":46,"dangerLevel":7,"description":"The concrete snow shed covers the pass road for a hundred meters, and the gallery is half filled with the snow it was built to deflect, pressed hard against the uphill wall. The uphill side is load-bearing and the downhill side is not, any more, which is the sentence every visitor repeats before entering and the reason most do not. Forty-six rads an hour on the road beneath, and the concrete muffles the wind, which makes the silence worse. The shed held the pass open for the trucks that used to cross. It still holds the road, for now, and the sound it makes in the wind sounds like an argum…`
  - row 78: `{"baseRadsPerHour":56,"dangerLevel":7,"description":"The relay station sits above everything, and on a clear day the line of sight reaches all five sub-regions of the map, every valley, every smoke column, every roof. There are perhaps six clear days a year, and the people who climb for them have learned the sky's patience. Fifty-six rads an hour at the summit, the highest reading on the map, because the wind delivers what the clouds hold. The relay's mast is still guyed and still tall, and the hut below is locked with a Garrison lock that nobody has cut. Whatever the station was built to connect, it connects to nothing now, but it holds the…`
  - row 79: `{"baseRadsPerHour":68,"dangerLevel":8,"description":"The reactor outbuilding is four meters square, and the door is wedged shut from the inside, which is the first fact and the one that matters. Water jugs are stacked against one wall, every one of them empty, every one opened, the caps beside them in a neat row. Sixty-eight rads an hour at the door, and the reading rises steadily as you circle the building. Someone chose this room at the end, and someone else, or the same someone, stacked the jugs in order and wedged the door and stayed. The window is shuttered from inside. The building gives up nothing else. The dose outside is the whole s…`
  - row 80: `{"baseRadsPerHour":44,"dangerLevel":7,"description":"Lock Gate Four is the gate that failed, and it is still open, exactly as far as it opened. The mechanism seized at the moment of the exchange, or of the flood, or of the decision, and the gate has held that position for three years, a door stopped mid-decision. Forty-four rads an hour at the lock house, and the water on the far side reads higher. The gate is the subject of an argument the locals keep reheating: whether it was meant to close, whether someone stopped it, whether it matters now. The ironwork is pitted and the winch is frozen. The gate stays where it is, and the water flows th…`
  - row 81: `{"baseRadsPerHour":48,"dangerLevel":7,"description":"Pump Station Nine was built to keep the basin dry, and the basin has won. The six drainage pumps stand under three meters of the very water they were built to move, their housings green with it, their motors drowned in the job they were hired to do. Forty-eight rads an hour at the surface, and the water reads worse, its own measure of everything that has settled. The pumps are the salvage, and the salvage is a story: iron, copper, windings that can be rewound by someone patient. The station is a machine that failed at its one task, then waited, under the water, for someone to come and take…`
  - row 82: `{"baseRadsPerHour":54,"dangerLevel":8,"description":"The maintenance level is marked with a stencilled designation, Allocation 12-B, and nothing else: no stores, no supplies, no reason for the name. Fifty-four rads an hour at the stair, and the air below is dead and warm. On the wall by the stair there are chalk marks, fourteen in a row, then a gap, then six more, and the chalk is the same color throughout, and the gap is deliberate, a section of wall left clean on purpose. Fourteen people went down. Six came back up. The gap is not a mistake: it is the way the record was kept, by someone who wanted it readable, and it is the only record the…`
  - row 83: `{"baseRadsPerHour":40,"dangerLevel":7,"description":"The records annex is reached by boat, through a second-storey window, and the approach is the sort of thing that sorts visitors: those who can tie a line and those who cannot. Inside, the annex is dry above the waterline, heated by a stove that is being maintained, and someone has been dusting: the shelves are clean, the files are upright, the aisle is swept. Forty rads an hour at the waterline, and the reading drops as you climb. The annex holds the district's written memory, and it is being kept, deliberately, by a hand that has not stopped in three years. The records are read. The dust …`
  - row 84: `{"baseRadsPerHour":46,"dangerLevel":7,"description":"The water in the Odeon has settled at the level of row F, and the screen is intact, the last thing in the building still facing its audience. The seats below the line are still folded down, and they were folded down by people who sat in them: the water came in while the house was full, or the house was full while the water came in, and the two versions of the story sound different. Forty-six rads an hour at the waterline, and the balcony stays cleaner. The film in the projector is still wound on its reel. The screen has not changed its image in three years: it is grey, and it is what every…`
  - row 85: `{"baseRadsPerHour":50,"dangerLevel":8,"description":"Twelve thousand cubic meters of freezer, and the room is four meters deep in water that has frozen over the top, sealing the contents under a floor of grey ice. The contents are unknown and, in principle, intact: whatever the cold store held in its last week, it holds still, under the ice, at the same temperature it was then. Fifty rads an hour at the door, and the ice reads lower, which is the whole argument for going in. The roof lights are dead, the loading dock is underwater, and the only entrance is a hatch that opens onto the ice itself. The store kept its promise to the last shipmen…`
  - row 86: `{"baseRadsPerHour":52,"dangerLevel":8,"description":"The survey launch Kittiwake lies aground on a submerged roof, listing gently, held where the flood left her. The sonar rig is intact, still mounted, still connected, a machine that measured the riverbed for three years and one that measures the silence now. Fifty-two rads an hour on deck, and the water around her is worse. The logbook is the find: the last entry is dated, and the entries continue eleven days past the Exchange, in a hand that got steadier as the world got worse, recording water levels, silt, and the position of the boat that was no longer going anywhere. The survey was fini…`
  - row 87: `{"baseRadsPerHour":42,"dangerLevel":7,"description":"Nine boats tied into a single raft over what used to be a retail park, and the trade happens at gunwale height, hand to hand across the water, because nobody boards anybody else's hull and the rule is older than the market. Forty-two rads an hour at the waterline, and the boats carry their own shade and their own weather. The Shallows trades in what the dry markets cannot: things salvaged from drowned floors, things that come up with the divers, things that were paid for in breath. The raft has grown one boat a year, and the newest hull still smells of tar. The market is patient. The water…`
  - row 88: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"Two hundred numbered plots behind a chain-link fence, a caretaker's hut, and a noticeboard at the gate. The waiting list is still pinned to the noticeboard, in a plastic sleeve, and it has been updated in pencil: four of the people on it are alive and farming here now, and their names are marked off in a different hand. Thirty rads an hour, moderate, and the plots are worked in rotation, the soil turned and rested as the readings allow. The allotments were a civic amenity before the war. Now they are a ration system with a waiting list, and the list is the most honest document in the distr…`
  - row 89: `{"baseRadsPerHour":18,"dangerLevel":4,"description":"The armory of the old checkpoint: a concrete room of lockers and a heavy door. The racks are mostly empty, but the sealed lockers hold what the guard did not carry out, and the lock is rusted solid.","displayName":"Checkpoint Kilo Armory","id":"checkpoint_kilo_armory","travelHours":3.0}`
  - row 90: `{"baseRadsPerHour":28,"dangerLevel":5,"description":"The pharmacy of the ruined hospital, door still intact behind the collapsed stairwell. The shelves behind the counter hold what the looters could not reach: vials, boxes, the small certainties of medicine.","displayName":"Hospital Pharmacy","id":"hospital_pharmacy","travelHours":4.0}`
  - row 91: `{"baseRadsPerHour":6,"dangerLevel":2,"description":"A backyard shed with a false floor. Beneath it, a family shelter stocked with care and dread: tinned food, a radio, a tape recorder, and a letter taped to the shelf.","displayName":"Family Bunker: Backyard Shed","id":"family_bunker_backyard_shed","travelHours":1.0}`
  - row 92: `{"baseRadsPerHour":12,"dangerLevel":3,"description":"A cache in the ruined library, tucked behind the collapsed reference desk. Books survived here in a dry pocket of the building, and with them the things people left between the pages.","displayName":"Old Library Cache","id":"old_library_cache","travelHours":2.5}`
  - row 93: `{"baseRadsPerHour":22,"dangerLevel":4,"description":"A supply cache buried by the wreck of convoy Echo-7, its marker half-swallowed by drift. The tarpaulin holds; what the convoy carried is still under it.","displayName":"Convoy Echo-7 Cache","id":"convoy_echo7_cache","travelHours":3.5}`
  - row 94: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"The place where the road narrows between the collapsed buildings: the classic kill ground. The raiders' own cache is hidden in the rubble nearby, marked with a chalked glyph for those in the know.","displayName":"Raider Ambush Site","id":"raider_ambush_site","travelHours":4.0}`
  - row 95: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A building that came down on itself, floors folded like a hand of cards. The pockets between the slabs are still dry, and what fell in here stayed in here.","displayName":"Collapsed Building","id":"collapsed_building","travelHours":2.0}`
  - row 96: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"A stretch of road rigged with tripwires and buried spikes, the kind of place raiders use to slow a convoy. The trap maker's hiding spot is within sight of the road, and so is what they collected from it.","displayName":"Raider Trap Site","id":"raider_trap_location","travelHours":4.0}`
  - row 97: `{"baseRadsPerHour":20,"dangerLevel":4,"description":"A substation yard of dead transformers and tangled busbars. The copper is long gone, but the control room survived the blast, and the tools and spares in it did too.","displayName":"Electrical Substation","id":"electrical_substation","travelHours":3.0}`
  - row 98: `{"baseRadsPerHour":14,"dangerLevel":3,"description":"A mechanics' garage with its roof half gone. The lifts are seized, the benches bare, but the tool lockers and the pit beneath the work floor were never opened.","displayName":"Ruined Garage","id":"ruined_garage","travelHours":2.0}`
  - row 99: `{"baseRadsPerHour":8,"dangerLevel":2,"description":"The shell of the old concert hall, roof open to the sky, stage intact. The acoustics still work: a whisper from the stage reaches the back row. Someone has been using it as a meeting place.","displayName":"Concert Hall Ruins","id":"concert_hall_ruins","travelHours":1.5}`
  - row 100: `{"baseRadsPerHour":19,"dangerLevel":3,"description":"The old Militia grain silo leans two degrees off true and has leaned that way for three years without picking a direction to fall. Underneath it, on packed earth between the legs of the structure, the Exchange runs six days a week: trestle tables, a hand-cranked scale nobody has caught cheating, and a chalked sign that says no weapons past the rope in four different hands, because it has been repainted by four different people who all meant it. Garrison quartermasters haggle two tables down from Ash Militia grain-runners, and neither reaches for a sidearm, because the unspoken rule is olde…`
  - row 101: `{"baseRadsPerHour":26,"dangerLevel":4,"description":"Sandbags gone the color of the ash they hold back, a boom barrier painted with a stripe pattern nobody has refreshed since the paint still matched a regulation. The notice board by the guard shack has been repapered so many times the corkboard shows through in patches, and the current bulletin is signed Harven in a hand that presses hard enough to be read from a distance. Rads sit at twenty-six an hour, unremarkable, the kind of number a checkpoint clerk stops mentioning after the first week. Conscription lists are posted here on the first of the month, and the queue that forms below them …`
  - row 102: `{"baseRadsPerHour":31,"dangerLevel":6,"description":"A single rail bridge over a dry cutting, one of the last spans in the district still theoretically load-bearing, and theoretically is doing a lot of work in that sentence. Charges are wired along the underside trusses in a pattern that looks careless and is not: Denial Detachment 9 marked every lead with a strip of reflective tape at a specific height, per doctrine, so that their own people don't walk into their own demolition. Thirty-one rads an hour on the deck. A weatherproofed speaker box is bolted to the near pier, and it has been transmitting the same automated loop on 96.100 for yea…`
  - row 103: `{"baseRadsPerHour":22,"dangerLevel":5,"description":"A dozen tents and one salvaged shipping container pitched in the dead lot between the Exchange and the Garrison checkpoint, close enough to both that it reads less like a camp and more like a hyphen. The watch schedule is chalked on the container door in a hand that still writes like it's taking minutes, four names to a shift, crossed off and rewritten as people rotate. Nobody has painted a sign. Twenty-two rads an hour, unremarkable. The fire pit sits exactly equidistant from the Exchange's lamps and the checkpoint's floodlight, which nobody here will admit was measured on purpose, though…`
  - row 104: `{"baseRadsPerHour":31,"dangerLevel":4,"description":"A lean-to and a water barrel halfway up the trail to the Ash Sign Shrine, built by pilgrims for pilgrims, maintained by whoever's climbing that week. A board nailed to the lean-to's post carries three years of names and dates in a dozen hands, the closest thing the trail has to a guestbook, and a line near the bottom reads only turned back, no name, no date crossed through to explain what stopped them. Thirty-one rads an hour, climbing steadily with the trail. Pilgrims rest here exactly once each way, by unspoken custom, whether or not they need to.","displayName":"The Switchback Waystatio…`
  - row 105: `{"baseRadsPerHour":18,"dangerLevel":3,"description":"A transmitter mast wired into what used to be a parking structure's stairwell, the antenna run up through a gap in the collapsed roof and guyed to rebar that was never meant to hold a mast and has held one for three years anyway. Nobody broadcasts from here more than once; the signal gets relayed onward from a different stairwell the next week, and the one after that from somewhere else, which is the entire reason the district has never managed to shut The Understory down. Eighteen rads an hour, low, chosen for exactly that reason. The log taped inside the stairwell door lists only call ti…`
  - row 106: `{"baseRadsPerHour":2,"dangerLevel":1,"description":"The main entrance to the shelter, a reinforced hatch set into the hillside with a decontamination vestibule and a checkpoint that never quite stops being a checkpoint. The gate controls who comes in, what they carry, and how long the airlock cycles. Dust accumulates in the seal grooves, and the radiation detector by the door has been stuck on the same readout for three weeks.","displayName":"Shelter Gate","id":"loc_shelter_gate","travelHours":0}`
  - row 107: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"A repurposed conference chamber with a long table salvaged from the municipal offices and chairs that do not match. The walls are lined with whiteboards still holding the last pre-war agenda, and someone has added new items in different marker colors: water rations, duty roster, radiation count. The room is the closest thing the shelter has to neutral ground.","displayName":"Shelter Meeting Room","id":"loc_shelter_meeting","travelHours":0}`
  - row 108: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"The shelter's medical bay, lit by fluorescent tubes that hum even when the main power is offline. There are two examination couches, a cabinet of scavenged supplies, and a dosage ledger that gets more entries every week. The air smells of antiseptic and burnt coffee. A curtain divides the room into treatment and triage, but the curtain is more psychological than practical.","displayName":"Shelter Infirmary","id":"loc_shelter_infirmary","travelHours":0}`
  - row 109: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"Racks of canned goods, fuel drums, and spare parts stacked floor to ceiling in the shelter's rear chamber. The inventory is written on cardboard tags tied with twine, and the twine is starting to rot. A Geiger counter sits on the shelf by the door, its battery cracked but still working. This is where the shelter keeps what it cannot afford to lose.","displayName":"Shelter Storage","id":"loc_shelter_storage","travelHours":0}`
  - row 110: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"The sleeping quarters: rows of bunk beds with curtains for privacy, a shared washbasin, and a shelf where everyone keeps their dosimeter and their personal journal. The air is warm and smells of damp wool and unwashed linen. Someone has pinned a calendar to the wall and crossed off each day with a pencil mark since the exchange.","displayName":"Shelter Quarters","id":"loc_shelter_quarters","travelHours":0}`
  - row 111: `{"baseRadsPerHour":3,"dangerLevel":2,"description":"The fire break and secondary containment zone at the rear of the shelter, where the ventilation shafts exit and the smoke from the kitchen stove is supposed to vent. The metal grating is clogged with ash, and the thermal sensor has been offline since the second month. If something burns, this is where it starts and where it ends.","displayName":"Shelter Fire Break","id":"loc_shelter_fire","travelHours":0}`
  - row 112: `{"baseRadsPerHour":2,"dangerLevel":1,"description":"The outer ring of the shelter: blast doors, airlocks, and the maintenance corridor that circles the entire bunker. The concrete here is thicker, the emergency lights are older, and the radiation shielding is supposed to be at its best. A checklist taped to the wall marks weekly inspection points, but the last check was three weeks ago.","displayName":"Shelter Perimeter","id":"loc_shelter_perimeter","travelHours":0}`
  - row 113: `{"baseRadsPerHour":20,"dangerLevel":4,"description":"The old highway east, its surface broken by frost heave and the weight of military convoys that never came back. The road is passable for most of the year, and the ash drift is thinner here than in the low ground. There are abandoned vehicles every few hundred meters, their cargoes long since taken, their skeletons marking where the traffic stopped.","displayName":"Eastern Road","id":"loc_eastern_road","travelHours":2.0}`
  - row 114: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A pre-war interchange plaza where three routes met and no faction claims sole ownership. The fountain is dry, the benches are broken, and a painted line on the pavement marks the boundary that everyone agrees to honor because the alternative is worse. Traders set up here on market days, and the radiation is moderate enough that standing around for hours does not guarantee sickness.","displayName":"Neutral Ground","id":"loc_neutral_ground","travelHours":1.5}`
  - row 115: `{"baseRadsPerHour":12,"dangerLevel":3,"description":"A pre-war pumping station with a concrete well house and a tank that still holds water if you are willing to filter it twice. The electrical panel is shot, but the manual override still works. The pool at the base of the tank is contaminated with surface runoff, and the dosimeter by the door reads higher than it should for a place that still provides.","displayName":"Water Station","id":"loc_water_station","travelHours":1.0}`
  - row 116: `{"baseRadsPerHour":30,"dangerLevel":7,"description":"A reinforced military command installation buried beneath tons of blasted granite. Three distinct strata lead to the hardened communications vault.","displayName":"Collapsed Command Vault","id":"loc_excavation_command_vault","travelHours":3.5}`
  - row 117: `{"baseRadsPerHour":15,"dangerLevel":4,"description":"Subterranean municipal service corridors and conduit trunks that run beneath the old district grid. The mud is knee-deep in the lower sections and the standing water carries a faint chemical tint that the Hydro Barons say is harmless and no one believes them. Bioluminescent mold clings to the pipe joints, throwing enough cold blue light to navigate by if your eyes have adjusted. The tunnels were used as evacuation routes in the first weeks and then as hidey-holes and now as shortcuts by people who know where the dead ends are. Someone has chalked arrows on the conduit elbows, some of them …`
  - row 118: `{"baseRadsPerHour":25,"dangerLevel":6,"description":"A multi-tier transit hub collapsed during the orbital strikes. Deep platform levels remain pressurized pockets holding pre-war civilian relics.","displayName":"Buried Metro Interchange","id":"loc_excavation_metro_interchange","travelHours":3.0}`
  - row 119: `{"baseRadsPerHour":40,"dangerLevel":8,"description":"A heavy extraction shaft dropping into mineral-rich bedrock. High mechanical salvage and lead-ore veins, tempered by severe structural instability.","displayName":"Industrial Mine Shaft Adit 4","id":"loc_excavation_mine_shaft","travelHours":4.5}`
  - row 120: `{"baseRadsPerHour":20,"dangerLevel":5,"description":"An underground scientific and administrative depository sealed under blast-hardened vault arches. Holds intact microforms, technical blueprints, and emergency dead-drop records.","displayName":"Pre-War Archive Bunker","id":"loc_excavation_archive_bunker","travelHours":3.0}`
  - row 121: `{"baseRadsPerHour":10,"dangerLevel":3,"description":"Stormwater culverts and overflow sluices converted into illicit smuggling routes before the bombardment. Silt and contaminated backwash hide sealed waterproof caches.","displayName":"Drainage Network Sluice 09","id":"loc_excavation_drainage_network","travelHours":1.5}`
  - row 122: `{"baseRadsPerHour":22,"dangerLevel":6,"description":"An auxiliary military logistics cache sealed in haste during civil evacuation. Unstable masonry slabs overhang intact pallets of rations and industrial spares.","displayName":"Forgotten Storage Chamber 14","id":"loc_excavation_storage_chamber","travelHours":2.5}`
  - row 123: `{"baseRadsPerHour":8,"dangerLevel":2,"description":"A privately funded neighborhood shelter built beneath a residential complex. Shorter excavation depths yield domestic survival gear, medical supplies, and handwritten diaries.","displayName":"Pre-War Civilian Shelter B-12","id":"loc_excavation_civilian_shelter","travelHours":1.0}`
  - row 124: `{"baseRadsPerHour":18,"dangerLevel":5,"description":"A concealed shortwave relay outpost buried into the cliffside, discovered by decrypting 'The Relay Count' numbers broadcast.","displayName":"Hidden Relay Bunker 09","id":"loc_hidden_relay_bunker","travelHours":2.5}`
  - row 125: `{"baseRadsPerHour":12,"dangerLevel":4,"description":"A secure logistics basement cache unlocked using the Winter Ledger cipher sheet. Packed with sealed emergency rations and filtration units.","displayName":"Sub-Basement Logistics Reserve","id":"loc_logistics_reserve_cache","travelHours":2.0}`
  - row 126: `{"baseRadsPerHour":22,"dangerLevel":6,"description":"An automated contingency shelter revealed through the Last Rotation dead-hand protocol. Contains classified directives and high-grade technical relics.","displayName":"Dead-Drop Command Shelter","id":"loc_deaddrop_command_shelter","travelHours":3.5}`
  - row 127: `{"baseRadsPerHour":0,"dangerLevel":0,"description":"The home bunker. Sub-surface reinforced shelter providing life support, workbenches, and secure quarters for the survivors.","displayName":"The Holdfast","id":"loc_holdfast","travelHours":0.0}`
  - row 128: `{"baseRadsPerHour":75,"dangerLevel":8,"description":"A high-yield ground zero impact basin saturated with ionizing radiation and pulverized cinder. High-grade military and industrial salvage remains in the melted basement levels.","displayName":"Fallout Zone Alpha","id":"loc_cut_radiation_zone_alpha","travelHours":3.5}`
  - row 129: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A fortified pre-war truck stop and weigh yard where wandering barter convoys converge. Low radiation and reliable staging for westward expeditions.","displayName":"Merchant Caravanserai","id":"loc_cut_merchant_caravanserai","travelHours":1.5}`
  - row 130: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"A sprawling freight yard with rusted boxcars and freight terminals. An essential transit corridor connecting the industrial belt to the western suburbs.","displayName":"Abandoned Rail Depot","id":"loc_cut_abandoned_depot","travelHours":2.0}`
  - row 131: `{"baseRadsPerHour":35,"dangerLevel":6,"description":"The bombed-out munitions storage and armory facility. Fortified vaults contain ballistic components and weapon parts behind collapsed blast doors.","displayName":"Arsenal Ruin","id":"loc_cut_arsenal_ruin","travelHours":2.5}`
  - row 132: `{"baseRadsPerHour":40,"dangerLevel":7,"description":"A coastal salvage pier and harbor watchtower garrisoned by maritime divers. Controls access to deep-coast marine wrecks and saline processing lanes.","displayName":"Black Flotilla Outpost","id":"loc_black_flotilla_outpost","travelHours":4.0}`
  - row 133: `{"baseRadsPerHour":15,"dangerLevel":2,"description":"A fortified salt camp in the tidal estuary. Evaporation pans and steam condensers produce curing salt and preserved provisions under salter council watch.","displayName":"Brine-Pan Hollow","id":"loc_settlement_brine_pans","travelHours":2.0}`
  - row 134: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"A modular rail-car town buried beneath railroad ballast near Span 44. Work gangs forge high-tensile hardware and armor plates from freight car steel.","displayName":"Iron Siding","id":"loc_settlement_iron_siding","travelHours":2.5}`
  - row 135: `{"baseRadsPerHour":25,"dangerLevel":4,"description":"A coastal lighthouse community maintaining freshwater cisterns, kelp drying racks, and prism optics on a windswept ocean bluff.","displayName":"Cape Beacon Commune","id":"loc_settlement_cape_beacon","travelHours":3.5}`
  - row 136: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A subterranean quarry enclave cut into impervious slate galleries in the High Scarp. Pit crews extract building stone and mill hones shielded from weather.","displayName":"Slate Hollow Enclave","id":"loc_settlement_slate_hollow","travelHours":3.0}`
  - row 137: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A mountain sanctuary warmed by natural geothermal steam radiators at Switchback Pass. Monks provide hot broth, herbal remedies, and peace-bound rest.","displayName":"The Pilgrim's Hearth","id":"loc_settlement_pilgrim_hearth","travelHours":2.5}`
  - row 138: `{"baseRadsPerHour":15,"dangerLevel":2,"description":"A bustling container and chassis market in the dead suburbs surrounded by an electrified fence. Factor stalls trade copper wire, batteries, and electronic chips.","displayName":"Tinker's Notch","id":"loc_settlement_tinkers_notch","travelHours":1.5}`
  - row 139: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A wind-scoured ridge above Slate Hollow Enclave where the old quarry crane platform gives a clear view of the pit entrance and the High Scarp switchbacks.","displayName":"Quarry Overlook Waypoint","id":"location_quarry_overlook","travelHours":3.0}`
  - row 140: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A converted rail freight terminal serving as the Grain Exchange faction's primary commodity hub. Surplus cereal and root stores are tallied here and rationed outward through a network of licensed brokers.","displayName":"The Grain Exchange","id":"loc_grain_exchange","travelHours":2.5}`
  - row 141: `{"baseRadsPerHour":20,"dangerLevel":4,"description":"A pre-war industrial slaughterhouse whose hydraulic line-kill systems still operate on stored power cells. The Osteophages use it to process large irradiated game and recycle bone stock for calcium supplement trade.","displayName":"Automated Abattoir","id":"loc_automated_abattoir","travelHours":3.5}`
  - row 142: `{"baseRadsPerHour":25,"dangerLevel":4,"description":"A drowned underground rail maintenance yard accessed through a half-submerged service hatch. The Undertow faction operates supply caches in the dry upper galleries, moving goods through the waterlogged tunnels by raft.","displayName":"Flooded Subway Depot","id":"loc_flooded_subway_depot","travelHours":2.0}`
  - row 143: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A rotating canvas-and-corrugated-steel encampment operated by the Scavenger Guild. Its position shifts with salvage fronts but the gatehouse marker is always the same: a stripped vehicle axle stood upright as a flagpole.","displayName":"Scavenger Guild Camp","id":"loc_scavenger_camp","travelHours":2.0}`
  - row 144: `{"baseRadsPerHour":10,"dangerLevel":5,"description":"The hardened command compound of the Iron Garrison faction, occupying a pre-war civil defense building with reinforced sub-levels. Access is by escort only; civilians who approach the perimeter fence without credentials are turned back at gunpoint.","displayName":"Iron Garrison Headquarters","id":"loc_iron_garrison","travelHours":1.5}`
  - row 145: `{"baseRadsPerHour":50,"dangerLevel":7,"description":"A silent, wind-scoured ash basin where ecological succession completely failed after intense isotope deposition. No insects hum, no lichen clings to the fractured basalt slabs, and desiccated animal carcasses lie uncomposed beneath acidic ash crusts. It stands as a stark warning of total ecological collapse.","displayName":"Ecological Dead Zone","id":"loc_dead_zone","travelHours":3.0}`
  - row 146: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A bustling river ferry crossing and barter dock in the Drown sector. River barges trade clean drinking water and preserved rations under the Undertow faction's watchful patrols.","displayName":"Ferry Point Exchange","id":"loc_settlement_ferry_crossing","travelHours":2.5}`
  - row 147: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A major rail concourse and trade hub in the Industrial Belt where switching yards meet covered trade docks, overseen by The Office's scheduling clerks.","displayName":"Nine Rails Junction","id":"loc_settlement_nine_rails","travelHours":2.0}`
  - row 148: `{"baseRadsPerHour":25,"dangerLevel":5,"description":"A heavily fortified rail fortress in the High Scarp controlled by the Deserter Coalition. Armor-plated diesel cars form defensive bastions around machine tooling workshops.","displayName":"Fort Karkov Marshalling Yard","id":"loc_settlement_fort_karkov","travelHours":4.0}`
  - row 149: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"A reinforced concrete sluice gate and hydraulic toll station in the Toll region, controlling water transit and charging tariffs in fuel and mechanical parts.","displayName":"Lock Seven Hydraulic Bastion","id":"loc_settlement_lock_seven","travelHours":2.5}`
  - row 150: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"An agricultural commune built into three massive concrete grain silos in the Verge. Farmers cultivate winter grain and barter seeds with passing caravans.","displayName":"New Ceres Silo Collective","id":"loc_settlement_silo_burrow","travelHours":3.0}`
  - row 151: `{"baseRadsPerHour":5,"dangerLevel":1,"description":"An underground artesian spring and monastic hospital in the Cluster. Silent caretakers offer clean holy water and sterile burn treatment to wounded travelers.","displayName":"St. Nicholas Spring Sanctuary","id":"loc_settlement_st_nicholas","travelHours":2.0}`
  - row 152: `{"baseRadsPerHour":8,"dangerLevel":3,"description":"A high ridgeline survey station atop the Iron Crest massif, used by geodetic teams for triangulation benchmarks. The view is total, the shelter is none, and the readings are worth both.","displayName":"Iron Crest Peak","id":"loc_iron_crest","travelHours":6.0}`
  - row 153: `{"baseRadsPerHour":18,"dangerLevel":4,"description":"A volcanic rock formation rising through the ashfield, used as a geodetic reference point. Seasonal ash plumes reduce visibility.","displayName":"Ash Needle Spire","id":"loc_ash_needle","travelHours":5.0}`
  - row 154: `{"baseRadsPerHour":6,"dangerLevel":3,"description":"A narrow mountain pass swept by near-constant high wind. Surveyors use it for line-of-sight triangulation, and have learned to shout their readings in the gaps between gusts.","displayName":"Wind Gap Ridge","id":"loc_wind_gap_ridge","travelHours":4.5}`
  - row 155: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A collapsed pre-war signal relay tower on a prominent hill. The steel frame still stands and serves as a survey sight — the last signal it will ever carry is somebody else's line of measurement.","displayName":"Signal Hill Tower Ruin","id":"loc_signal_hill_tower","travelHours":3.0}`
  - row 156: `{"baseRadsPerHour":12,"dangerLevel":2,"description":"A flood-scoured river bend where pre-war survey crews set a permanent datum monument. Accessible only at low water; the monument has outwaited every flood that tried to argue with it.","displayName":"River Bend Datum Monument","id":"loc_river_bend_outpost","travelHours":2.5}`
  - row 157: `{"baseRadsPerHour":14,"dangerLevel":3,"description":"A partially collapsed iron rail bridge. The remaining abutment is stable enough to mount a survey instrument, and steady enough that the theodolite reads truer here than on solid ground.","displayName":"Rusted Span Bridge","id":"loc_rusted_span_bridge","travelHours":2.0}`
  - row 158: `{"baseRadsPerHour":20,"dangerLevel":2,"description":"Decommissioned industrial chimneys from a pre-war crematory complex, repurposed as vertical survey markers. The crews who use them for sightings have stopped saying what they were, which is its own kind of respect.","displayName":"Old Crematory Industrial Stacks","id":"loc_old_crematory_stacks","travelHours":1.5}`
  - row 159: `{"baseRadsPerHour":8,"dangerLevel":1,"description":"A surviving water tower at the northern perimeter of a collapsed settlement, offering unobstructed sightlines in every direction. It has been empty for years, and nobody has decided whether that is a loss or a mercy.","displayName":"North Gate Water Tower","id":"loc_north_gate_water_tower","travelHours":1.0}`
  - row 160: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A concrete rail junction building surrounded by overgrown track debris. Surveyors use the rooftop for low-elevation benchmarks.","displayName":"Junction Box Rail Station","id":"loc_junction_box_rail","travelHours":1.5}`
  - ... 19 additional rows omitted from the compact audit; the complete current file is identified above ...

# Appendix D — Current caller/reference graph

### `VerdictCatalogLoader.LoadLocations` (9 sampled current references)
- src/Host/VerdictHostSession.cs:119: var locations = VerdictCatalogLoader.LoadLocations(dataDir, s_files, s_json);
- Ashfall.Core.Tests/VerdictContentWebTests.cs:98: var locs = VerdictCatalogLoader.LoadLocations(
- Ashfall.Core.Tests/VerdictSystemTests.cs:669: var result = VerdictCatalogLoader.LoadLocations("/nonexistent", io, json);
- Ashfall.Core.Tests/VerdictSystemTests.cs:676: Assert.Empty(VerdictCatalogLoader.LoadLocations(null, null, null));
- Ashfall.Core.Tests/VerdictSystemTests.cs:677: Assert.Empty(VerdictCatalogLoader.LoadLocations("", new FileSystemIO(), new SystemTextJsonSerializer()));
- Ashfall.Core.Tests/YearOfAshTests.cs:780: var locations = VerdictCatalogLoader.LoadLocations(dataDir, io, json);
- Ashfall.Core.Tests/Verdict/Plan82VerdictLocationsExpansionTests.cs:27: return VerdictCatalogLoader.LoadLocations(
- Ashfall.Core.Tests/Verdict/Plan82_67VerdictCassetteIntegrationTests.cs:20: var locations = VerdictCatalogLoader.LoadLocations(
- Ashfall.Core.Tests/Verdict/Plan82_67VerdictCassetteIntegrationTests.cs:96: var locations = VerdictCatalogLoader.LoadLocations(
### `VerdictHostSession` (17 sampled current references)
- Assets/Ashfall.Core/CatalogIntegrityRules.cs:332: "flag_verdict_cliff_signal_decoded", // Plan 93: materialized in VerdictHostSession (machine-log read depth)
- Assets/Ashfall.Core/CatalogIntegrityValidator.cs:510: "flag_verdict_cliff_signal_decoded", // Plan 93: materialized in VerdictHostSession (machine-log read depth)
- src/Main.Verdict.cs:34: private AtomicWar.GodotApp.VerdictHostSession _verdict = null!;
- src/Main.Verdict.cs:47: _verdict = AtomicWar.GodotApp.VerdictHostSession.Create(_dataDir, flags: _consequenceLedger);
- src/VerdictPanel.cs:23: private VerdictHostSession _verdict;
- src/VerdictPanel.cs:128: public void Bind(VerdictHostSession verdict)
- src/Host/VerdictHostSession.cs:24: public sealed class VerdictHostSession
- src/Host/VerdictHostSession.cs:73: public VerdictHostSession(
- src/Host/VerdictHostSession.cs:106: public static VerdictHostSession Create(
- src/Host/VerdictHostSession.cs:125: var session = new VerdictHostSession(census: censusBroadcast, locations: locations, items: items, radio: radioEntries, quests: quests);
- src/Host/VerdictSaveStore.cs:5: // Host Caller: Main.Verdict / VerdictHostSession
- src/YearOfAsh/YearOfAshHostSession.cs:122: // and persisted via VerdictHostSession / VerdictSave (v3+). Older
- src/UI/ExpansionsHubPanel.cs:35: private VerdictHostSession? _verdict;
- src/UI/ExpansionsHubPanel.cs:68: VerdictHostSession? verdict,
- src/UI/VerdictDashboardPanel.cs:21: /// Reads headline metrics from the bound VerdictHostSession directly; the
- src/UI/VerdictDashboardPanel.cs:31: private VerdictHostSession? _session;
- src/UI/VerdictDashboardPanel.cs:35: public void Bind(VerdictPanel verdict, VerdictHostSession session)
### `VerdictNpcSystem` (18 sampled current references)
- Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs:43: public sealed class VerdictNpcSystem
- Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs:53: public VerdictNpcSystem(VerdictNpcState? state = null)
- Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs:127: public static int LoadAndRegister(VerdictNpcSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json)
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:110: VerdictNpcSystem? npcs = null,
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:253: VerdictNpcSystem? npcs = null,
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:447: ["verdict_npcs.json"] = new[] { "VerdictNpcSystem" },
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:803: ["verdict_npcs.json"] = "VerdictNpcSystem",
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:974: ["verdict_npcs.json"] = new[] { "VerdictNpcSystem" },
- src/Host/VerdictHostSession.cs:33: public VerdictNpcSystem Npcs { get; }
- src/Host/VerdictHostSession.cs:77: VerdictNpcSystem npcs = null!,
- src/Host/VerdictHostSession.cs:88: Npcs = npcs ?? new VerdictNpcSystem();
- Ashfall.Core.Tests/VerdictQuestOwnershipTests.cs:70: npcs: new VerdictNpcSystem(), quests: quests);
- Ashfall.Core.Tests/VerdictQuestOwnershipTests.cs:80: new EvidenceLedger(), npcs: new VerdictNpcSystem(), quests: restored);
- Ashfall.Core.Tests/VerdictSaveMigrationTests.cs:53: var npcs = new VerdictNpcSystem();
- Ashfall.Core.Tests/VerdictSaveMigrationTests.cs:188: public void VerdictNpcSystem_GatesOnFlagAndPhase()
- Ashfall.Core.Tests/VerdictSaveMigrationTests.cs:190: var sys = new VerdictNpcSystem();
- Ashfall.Core.Tests/VerdictSaveMigrationTests.cs:225: VerdictSaveCodec.Restore(loaded, new MachineLogSystem(), rec, new EvidenceLedger(), new VerdictNpcSystem());
- Ashfall.Core.Tests/VerdictSystemTests.cs:473: // ── VerdictNpcSystem ────────────────────────────────────────────────────
### `VerdictRadioSystem` (18 sampled current references)
- Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs:18: public sealed class VerdictRadioSystem
- Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs:30: public VerdictRadioSystem(
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:37: public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:60: public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:97: public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:111: VerdictRadioSystem? radio = null,
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:122: radio = radio != null ? radio.CaptureState() : new VerdictRadioSystem.VerdictRadioState(),
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:213: radio = v2.radio ?? new VerdictRadioSystem.VerdictRadioState(),
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:238: radio = v3.radio ?? new VerdictRadioSystem.VerdictRadioState(),
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:254: VerdictRadioSystem? radio = null,
- Assets/Ashfall.Core/Verdict/VerdictSave.cs:265: radio.RestoreState(save.radio ?? new VerdictRadioSystem.VerdictRadioState());
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:972: ["verdict_radio.json"] = new[] { "VerdictRadioSystem" },
- src/VerdictPanel.cs:358: /// from the session's VerdictRadioSystem state. Thin presentation only.</summary>
- src/Host/VerdictHostSession.cs:35: public VerdictRadioSystem Radio { get; internal set; }
- src/Host/VerdictHostSession.cs:126: session.Radio = new VerdictRadioSystem(bus, clock, radioEntries);
- src/Host/HostCli.SelfTests.cs:849: ? new VerdictRadioSystem(bus, clock, radioCorpus)
- src/Host/HostCli.SelfTests.cs:850: : new VerdictRadioSystem();
- Ashfall.Core.Tests/EventSurfaceArchitectureTests.cs:147: "VerdictRadioSystem.cs",
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

# Appendix E — Current focused-test inventory

Current test declaration inventory: 60 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/Verdict/Plan82VerdictLocationsExpansionTests.cs` — 20 test declarations; bytes=7,450; SHA-256=`eecc13044f19bd1ee8487dbfc437b42f9130aa8b5b41dedea5ed97f3721ee127`
- 00031: [Fact]
- 00032: public void LoadLocations_ReturnsExactlyFifteenSites()
- 00039: [Fact]
- 00040: public void PreservesAllFourOriginalTempestArraySites()
- 00061: [Fact]
- 00062: public void VerifiesAllFourInvestigationArcsPresent()
- 00084: [Fact]
- 00085: public void AllLocationIdsAreUniqueAndFollowCanonicalPrefix()
- 00099: [Fact]
- 00100: public void DangerLevelsWithinValidThreeToTenRange()
- 00109: [Fact]
- 00110: public void TravelHoursWithinValidThreeToTwelveRange()
- 00119: [Fact]
- 00120: public void BaseRadsPerHourWithinValidTwentyToSixtyRange()
- 00129: [Fact]
- 00130: public void AllDescriptionsMeetHighQualityDensityStandards()
- 00145: [Fact]
- 00146: public void NoForbiddenSupernaturalOrGenericTropesInDescriptions()
- 00161: [Fact]
- 00162: public void LocationsRoundTripSerialization_PreservesAllFields()
### `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs` — 20 test declarations; bytes=8,495; SHA-256=`c63225a0ebdbc8bdb493a8de624bf65d6e098095b7a9bfd5c940b2e30957c6e2`
- 00024: [Fact]
- 00025: public void Catalog_Loads_All_18_Npc_Entries()
- 00032: [Fact]
- 00033: public void All_18_Npc_Ids_Are_Unique_And_Prefixed()
- 00045: [Fact]
- 00046: public void Original_6_Baseline_Npcs_Preserved()
- 00069: [Fact]
- 00070: public void Plan18_Tribunal_Npcs_Preserved()
- 00090: [Fact]
- 00091: public void All_9_Plan93_Investigation_Npcs_Present()
- 00124: [Fact]
- 00125: public void All_Npc_Kinds_Are_Supported()
- 00142: [Fact]
- 00143: public void All_Plan93_LocationIds_Map_To_Distinct_Verdict_Sites()
- 00167: [Fact]
- 00168: public void GetAvailable_Filters_By_Phase_And_Flag_And_Location()
- 00201: [Fact]
- 00202: public void Speak_Is_OneShot_And_Persists_In_State()
- 00224: [Fact]
- 00225: public void Availability_Is_Deterministic_Across_Invocations()
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
### `Ashfall.Core.Tests/Verdict/Plan82_67VerdictCassetteIntegrationTests.cs` — 8 test declarations; bytes=7,636; SHA-256=`f408e9b34a96d7826acfcd7cd1a88d0a81f07102c1ab76a1bdc41bd8a1eb4e10`
- 00016: [Fact]
- 00017: public void VerdictLocationsAndCassetteCatalog_LoadAccurately_WithoutCollisions()
- 00042: [Fact]
- 00043: public void CassettePlaybackSystem_AcquireAndPlaySequence_GrantsMoraleAndCompletesSet()
- 00093: [Fact]
- 00094: public void VerdictCartographyToCassetteScavenging_CrossSystemLinkage()
- 00128: [Fact]
- 00129: public void CassettePlaybackSystem_SaveRestoreRoundTrip_PreservesState()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Verdict Locations: Fifteen-Site Investigation Corpus, Map/Quest Coupling, and Verdict Save Ownership` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan’s “three new arcs” are not current schema fields; they remain a design question.
- A location row does not prove expedition reachability or NPC presence.

## H.2 Evidence-strength corrections
- Classify narrative-only versus reachable sites.
- Do not invent trail fields.
- Trace references and save parity.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Verdict Locations: Fifteen-Site Investigation Corpus, Map/Quest Coupling, and Verdict Save Ownership while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use VerdictCatalogLoader for static locations.
- Use VerdictHostSession/Verdict systems for evidence/radio/census.
- Use WastelandMap/Expedition owners for reachability.
- Use VerdictSaveStore for mutable Verdict state.
- Use current Verdict UI for projection.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement verdict locations: fifteen-site investigation corpus, map/quest coupling, and verdict save ownership arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- No second map/trail/Verdict owner is proposed.
- Every future reachability claim names the canonical map/expedition seam.

## J.2 Questions deliberately left open
- Should all 15 sites become canonical map nodes, or remain Verdict-only locations?
- Which current owner should expose site travel cost and radiation?

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

The original file at `HEAD:piagentsplans/82-verdict-locations-expansion.md` contained 4,999 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 82 — Verdict Investigation Sites Expansion (4 → 15 locations)

## Goal (2 lines)
Expand `verdict_locations.json` from 4 verified investigation sites to 15. The
Verdict system (`VerdictCatalogLoader.cs` confirmed live) defines remote
investigation sites the player can travel to — each has a description, danger
level, travel hours, and base radiation. The existing 4 sites are richly written
(seismometer pits, fuse bunkers, tape silos) but too few for a full
investigation campaign.

## Why (P2)
- Verified: `verdict_locations.json` has 4 entries (id, displayName, description,
  dangerLevel, travelHours, baseRadsPerHour). `VerdictCatalogLoader.cs` and
  `VerdictNpcSystem.cs` are confirmed live. The existing 4 sites are a connected
  narrative (geophone pit → twelve-gauge array → fuse world → tape silo) but the
  investigation trail ends there.
- Creates the investigation-arc pillar: Verdict sites are the game's deepest
  environmental-storytelling locations — each is a pre-war scientific/military
  site with a mystery to unravel. 4 sites is one arc; 15 sites creates a
  multi-arc investigation campaign with branching trails.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/verdict_locations.json` (expand 4 → 15 sites)
- Read-only: `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` (confirm
  schema and how locations are linked into investigation trails)
- `Assets/StreamingAssets/Data/verdict_npcs.json` (6 NPCs — some sites may
  reference NPC encounters)

## Content grammar (per site)
- snake_case `id` with prefix `loc_` (confirmed prefix).
- description: 3–6 sentences of dense environmental storytelling — physical
  detail, evidence, contradiction, mystery. Match the existing quality bar
  (the geophone pit and tape-silo descriptions are the model).
- dangerLevel: 3–10 (Verdict sites are remote and dangerous).
- travelHours: 3–12 (long travel is part of the cost).
- baseRadsPerHour: 20–60 (these are irradiated pre-war sites).
- Investigation arcs: group sites into 3–4 connected trails (each trail is a
  mystery the player unravels by visiting sites in sequence). The existing 4
  sites form the "Tempest Array" trail; add 2–3 more trails.
- Grounded tone: pre-war military, scientific, or governmental sites —
  seismometer arrays, weather stations, communications bunkers, survey sites,
  archive vaults. No fantasy, no supernatural.

## Steps
1. Read `VerdictCatalogLoader.cs` to confirm the schema and whether locations
   are linked into trails (is there a trail/arc field, or are trails implied by
   NPC dialogue and radio broadcasts?).
2. Read `verdict_npcs.json` to confirm which NPCs are site-linked and how they
   reference locations.
3. Read `verdict_radio.json` to confirm how radio broadcasts reference
   investigation sites (Plan 73 expands the faction radio corpus; Verdict radio
   is separate).
4. Author 11 new sites in 3 new investigation arcs:
   - Arc "The Coastal Survey" (4 sites): abandoned tide gauge, coastal
     meteorological station, cliff-top observation bunker, sealed marine lab.
   - Arc "The Interior Caches" (4 sites): forestry survey post, geological
     core-sample vault, river-gauging station, abandoned agricultural station.
   - Arc "The Border Wire" (3 sites): decommissioned signal relay, border
     checkpoint ruins, minefield observation tower.
5. Each site: dense description (match existing quality), dangerLevel,
   travelHours, baseRadsPerHour. Each arc tells a self-contained mystery
   (what was this site for? what happened here? what does the evidence reveal?).
6. Cross-reference: every loc_ id unique; check if any site should reference an
   existing verdict NPC (witness at the site).
7. Wire 2 sites into Plan 76 expedition destinations (coastal and border sites
   are reachable via expedition).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: verdict location catalog loads 15 sites, all ids unique, dangerLevel
   and travelHours within valid ranges, baseRadsPerHour realistic.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is description quality (step 5): the existing
sites set a very high bar — match it, do not write generic location descriptions.

## Definition of Done
- `verdict_locations.json` has 15 sites in 4 investigation arcs, all ids
  resolving, 2 wired to expedition destinations, integrity + tests green.

## Follow-on
- Plan 76 (expedition destinations) — 2 Verdict sites are expedition-reachable.
- Plan 73 (faction radio) — Verdict radio broadcasts reference investigation sites.
- Plan 51 (environmental storytelling) — Verdict sites are the deepest
  environmental-storytelling locations.
- Plan 84 (muster witnesses) — witnesses at Verdict sites provide testimony.
- Existing 18 (expansion deepening) — this plan deepens the Verdict expansion.

```

## End of Plan 82 — current-evidence rebase

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

## `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` — 155 lines; 5,915 bytes; SHA-256 `09f7ee1e6d99e55b7559acda53ff56389392ff3ae5cf9ca2069d4b3db2a28ca4`
Declaration index:
- 00011: public class VerdictNpcEntry
- 00032: public class VerdictNpcState
- 00043: public sealed class VerdictNpcSystem
- 00058: public void Register(VerdictNpcEntry entry)
- 00064: public VerdictNpcEntry? Find(string id)
- 00072: public List<VerdictNpcEntry> GetAvailable(
- 00088: public bool Speak(string npcId, string? locationId = null)
- 00100: private static bool ContainsFlag(IReadOnlyCollection<string> flags, string id)
- 00107: public VerdictNpcState CaptureState()
- 00114: public void RestoreState(VerdictNpcState state)
- 00123: public static class VerdictNpcCatalogLoader
- 00127: public static int LoadAndRegister(VerdictNpcSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: using Ashfall.Core.IO;
00007: namespace Ashfall.Core.Verdict
00008: {
00009:     /// <summary>One Verdict NPC — a figure with flag-gated availability and phase reactions.</summary>
00010:     [Serializable]
00011:     public class VerdictNpcEntry
00012:     {
00013:         public string id = string.Empty;
00014:         public string name = string.Empty;
00015:         public string role = string.Empty;
00016:         public string kind = "paper_ghost";  // tape_echo | paper_ghost | living | readings
00017:
00018:         // Plan 93: verdict_npcs.json is snake_case data authority while the
00019:         // shared serializer options are case-insensitive only (no naming
00020:         // policy) — these fields need explicit mappings or they silently
00021:         // deserialize to defaults (empty gate/location, phaseMin 1).
00022:         [System.Text.Json.Serialization.JsonPropertyName("gating_flag")]
00023:         public string gatingFlag = string.Empty;
00024:         [System.Text.Json.Serialization.JsonPropertyName("location_id")]
00025:         public string locationId = string.Empty;
00026:         [System.Text.Json.Serialization.JsonPropertyName("phase_min")]
00027:         public int phaseMin = 1;
00028:         public List<string> dialogue = new List<string>();
00029:     }
00030:
00031:     [Serializable]
00032:     public class VerdictNpcState
00033:     {
00034:         public List<string> spokenNpcIds = new List<string>();
00035:     }
00036:
00037:     /// <summary>
00038:     /// ASHFALL: THE VERDICT (Expansion 08) — the six figures of the machine's
00039:     /// human record. Each is a flag-gated encounter card: available only when
00040:     /// its gate flag is set, reactive to the Reckoning phase, one-shot spoken.
00041:     /// No human faction is spawned — the Tempest stays a utility.
00042:     /// </summary>
00043:     public sealed class VerdictNpcSystem
00044:     {
00045:         private readonly VerdictNpcState _state;
00046:         private readonly List<VerdictNpcEntry> _catalog = new List<VerdictNpcEntry>();
00047:
00048:         public VerdictNpcState State => _state;
00049:         public IReadOnlyList<VerdictNpcEntry> Catalog => _catalog;
00050:
00051:         public event Action<VerdictNpcEntry> OnSpoken;
00052:
00053:         public VerdictNpcSystem(VerdictNpcState? state = null)
00054:         {
00055:             _state = state ?? new VerdictNpcState();
00056:         }
00057:
00058:         public void Register(VerdictNpcEntry entry)
00059:         {
00060:             if (entry == null || string.IsNullOrEmpty(entry.id)) return;
00061:             if (!_catalog.Exists(e => e.id == entry.id)) _catalog.Add(entry);
00062:         }
00063:
00064:         public VerdictNpcEntry? Find(string id)
00065:         {
00066:             foreach (var e in _catalog)
00067:                 if (e.id == id) return e;
00068:             return null;
00069:         }
00070:
00071:         /// <summary>NPCs whose gate flag is set and whose phase requirement is met.</summary>
00072:         public List<VerdictNpcEntry> GetAvailable(
00073: IReadOnlyCollection<string> setFlags, int phase, string? locationId = null)
00074:         {
00075:             var result = new List<VerdictNpcEntry>();
00076:             foreach (var e in _catalog)
00077:             {
00078:                 if (e.phaseMin > 1 && phase < e.phaseMin) continue;
00079:                 if (!string.IsNullOrEmpty(e.gatingFlag) &&
00080:                     (setFlags == null || !ContainsFlag(setFlags, e.gatingFlag))) continue;
00081:                 if (!string.IsNullOrEmpty(locationId) && e.locationId != locationId) continue;
00082:                 result.Add(e);
00083:             }
00084:             return result;
00085:         }
00086:
00087:         /// <summary>Spend the NPC's only interjection. Idempotent per NPC.</summary>
00088:         public bool Speak(string npcId, string? locationId = null)
00089:         {
00090:             var npc = Find(npcId);
00091:             if (npc == null) return false;
00092:             if (_state.spokenNpcIds.Contains(npcId)) return false; // one-shot
00093:             if (!string.IsNullOrEmpty(locationId) && npc.locationId != locationId) return false;
00094:
00095:             _state.spokenNpcIds.Add(npcId);
00096:             OnSpoken?.Invoke(npc);
00097:             return true;
00098:         }
00099:
00100:         private static bool ContainsFlag(IReadOnlyCollection<string> flags, string id)
00101:         {
00102:             foreach (var f in flags)
00103:                 if (string.Equals(f, id, StringComparison.OrdinalIgnoreCase)) return true;
00104:             return false;
00105:         }
00106:
00107:         public VerdictNpcState CaptureState()
00108:         {
00109:             var copy = new VerdictNpcState();
00110:             copy.spokenNpcIds.AddRange(_state.spokenNpcIds);
00111:             return copy;
00112:         }
00113:
00114:         public void RestoreState(VerdictNpcState state)
00115:         {
00116:             if (state == null) return;
00117:             _state.spokenNpcIds.Clear();
00118:             _state.spokenNpcIds.AddRange(state.spokenNpcIds);
00119:         }
00120:     }
00121:
00122:     /// <summary>Loader for verdict_npcs.json.</summary>
00123:     public static class VerdictNpcCatalogLoader
00124:     {
00125:         public const string FileName = "verdict_npcs.json";
00126:
00127:         public static int LoadAndRegister(VerdictNpcSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json)
00128:         {
00129:             if (system == null || fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00130:                 return 0;
00131:             string path = fileIO.Combine(dataDir, FileName);
00132:             if (!fileIO.FileExists(path)) return 0;
00133:             string raw = fileIO.ReadAllText(path);
00134:             if (string.IsNullOrWhiteSpace(raw)) return 0;
00135:             try
00136:             {
00137:                 var list = CatalogLocator.LoadWrappedList<VerdictNpcEntry>(raw, SystemTextJsonSerializer.Options);
00138:                 if (list == null) return 0;
00139:                 int count = 0;
00140:                 foreach (var e in list)
00141:                 {
00142:                     if (e == null || string.IsNullOrEmpty(e.id)) continue;
00143:                     system.Register(e);
00144:                     count++;
00145:                 }
00146:                 return count;
00147:             }
00148:             catch (Exception ex_CATDIAG)
00149:             {
00150:                 CatalogDiagnostics.Warn(path, "VerdictNpcEntry list", ex_CATDIAG);
00151:                 return 0;
00152:             }
00153:         }
00154:     }
00155: }
```

## `Assets/Ashfall.Core/Verdict/VerdictQuestCatalogLoader.cs` — 63 lines; 2,293 bytes; SHA-256 `5f1a7bc4b7265f09f93b104b8e569dab426eea3aec5e50b14ff4656c7f59c6c4`
Declaration index:
- 00017: public static class VerdictQuestCatalogLoader
- 00021: public static int LoadAndRegister(
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.YearOfAsh;
00005:
00006: using Ashfall.Core.IO;
00007: namespace Ashfall.Core.Verdict
00008: {
00009:     /// <summary>
00010:     /// ASHFALL: THE VERDICT (Expansion 08) — quest registration into the live
00011:     /// QuestlineSystem. `verdict_questlines.json` is authored directly in the
00012:     /// runtime QuestlineDefinition schema (stageId-DAG-with-choices) so the
00013:     /// quests are playable end-to-end, unlike the legacy flat stage catalog.
00014:     /// Loads via the existing YearOfAshCatalogLoader's third fallback (a raw
00015:     /// List&lt;QuestlineDefinition&gt;) — no parallel quest evaluator.
00016:     /// </summary>
00017:     public static class VerdictQuestCatalogLoader
00018:     {
00019:         public const string FileName = "verdict_questlines.json";
00020:
00021:         public static int LoadAndRegister(
00022:             QuestlineSystem system,
00023:             string dataDir,
00024:             IFileIO fileIO,
00025:             IJsonSerializer json)
00026:         {
00027:             if (system == null || fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00028:                 return 0;
00029:
00030:             string path = fileIO.Combine(dataDir, FileName);
00031:             if (!fileIO.FileExists(path))
00032:                 return 0;
00033:
00034:             string raw = fileIO.ReadAllText(path);
00035:             if (string.IsNullOrWhiteSpace(raw))
00036:                 return 0;
00037:
00038:             try
00039:             {
00040:                 var container = json.Deserialize<YearOfAshQuestContainer>(raw);
00041:                 var quests = container?.quests;
00042:                 if (quests == null || quests.Count == 0)
00043:                 {
00044:                     quests = CatalogLocator.LoadWrappedList<QuestlineDefinition>(raw, SystemTextJsonSerializer.Options);
00045:                 }
00046:                 if (quests == null) return 0;
00047:                 int count = 0;
00048:                 foreach (var def in quests)
00049:                 {
00050:                     if (def == null || string.IsNullOrEmpty(def.questlineId)) continue;
00051:                     system.RegisterQuestline(def);
00052:                     count++;
00053:                 }
00054:                 return count;
00055:             }
00056:             catch (Exception ex_CATDIAG)
00057:             {
00058:                 CatalogDiagnostics.Warn(path, "Verdict quest catalog", ex_CATDIAG);
00059:                 return 0;
00060:             }
00061:         }
00062:     }
00063: }
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

## `Ashfall.Core.Tests/Verdict/Plan82VerdictLocationsExpansionTests.cs` — 182 lines; 7,450 bytes; SHA-256 `eecc13044f19bd1ee8487dbfc437b42f9130aa8b5b41dedea5ed97f3721ee127`
Declaration index:
- 00013: public class Plan82VerdictLocationsExpansionTests : CatalogTestBase
- 00015: private static string FindDataDir()
- 00023: private static List<VerdictCatalogLoader.VerdictLocationEntry> LoadLocations()
- 00032: public void LoadLocations_ReturnsExactlyFifteenSites()
- 00040: public void PreservesAllFourOriginalTempestArraySites()
- 00062: public void VerifiesAllFourInvestigationArcsPresent()
- 00085: public void AllLocationIdsAreUniqueAndFollowCanonicalPrefix()
- 00100: public void DangerLevelsWithinValidThreeToTenRange()
- 00110: public void TravelHoursWithinValidThreeToTwelveRange()
- 00120: public void BaseRadsPerHourWithinValidTwentyToSixtyRange()
- 00130: public void AllDescriptionsMeetHighQualityDensityStandards()
- 00146: public void NoForbiddenSupernaturalOrGenericTropesInDescriptions()
- 00162: public void LocationsRoundTripSerialization_PreservesAllFields()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.IO;
00008: using Ashfall.Core.Verdict;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.Verdict
00012: {
00013:     public class Plan82VerdictLocationsExpansionTests : CatalogTestBase
00014:     {
00015:         private static string FindDataDir()
00016:         {
00017:             string start = Directory.GetCurrentDirectory();
00018:             if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
00019:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
00020:             return string.Empty;
00021:         }
00022:
00023:         private static List<VerdictCatalogLoader.VerdictLocationEntry> LoadLocations()
00024:         {
00025:             string dataDir = FindDataDir();
00026:             Assert.False(string.IsNullOrEmpty(dataDir), "Data directory not found.");
00027:             return VerdictCatalogLoader.LoadLocations(
00028:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00029:         }
00030:
00031:         [Fact]
00032:         public void LoadLocations_ReturnsExactlyFifteenSites()
00033:         {
00034:             var locs = LoadLocations();
00035:             Assert.NotNull(locs);
00036:             Assert.Equal(15, locs.Count);
00037:         }
00038:
00039:         [Fact]
00040:         public void PreservesAllFourOriginalTempestArraySites()
00041:         {
00042:             var locs = LoadLocations();
00043:             var byId = locs.ToDictionary(l => l.id);
00044:
00045:             Assert.True(byId.ContainsKey("loc_geophone_pit_1"));
00046:             Assert.True(byId.ContainsKey("loc_twelve_gauge_array"));
00047:             Assert.True(byId.ContainsKey("loc_network_fuse_bunker"));
00048:             Assert.True(byId.ContainsKey("loc_archive_tape_silo"));
00049:
00050:             Assert.Equal("The First Geophone Pit", byId["loc_geophone_pit_1"].displayName);
00051:             Assert.Equal("The Twelve-Gauge Array", byId["loc_twelve_gauge_array"].displayName);
00052:             Assert.Equal("The Fuse World", byId["loc_network_fuse_bunker"].displayName);
00053:             Assert.Equal("The Archive Tape-Silo", byId["loc_archive_tape_silo"].displayName);
00054:
00055:             Assert.Equal(6, byId["loc_geophone_pit_1"].dangerLevel);
00056:             Assert.Equal(7, byId["loc_twelve_gauge_array"].dangerLevel);
00057:             Assert.Equal(8, byId["loc_network_fuse_bunker"].dangerLevel);
00058:             Assert.Equal(9, byId["loc_archive_tape_silo"].dangerLevel);
00059:         }
00060:
00061:         [Fact]
00062:         public void VerifiesAllFourInvestigationArcsPresent()
00063:         {
00064:             var locs = LoadLocations();
00065:             var byId = locs.ToDictionary(l => l.id);
00066:
00067:             // Arc 1 — Tempest Array (4)
00068:             string[] arc1 = { "loc_geophone_pit_1", "loc_twelve_gauge_array", "loc_network_fuse_bunker", "loc_archive_tape_silo" };
00069:             foreach (var id in arc1) Assert.True(byId.ContainsKey(id), $"Arc 1 missing {id}");
00070:
00071:             // Arc 2 — The Coastal Survey (4)
00072:             string[] arc2 = { "loc_abandoned_tide_gauge", "loc_coastal_meteorological_station", "loc_clifftop_observation_bunker", "loc_sealed_marine_laboratory" };
00073:             foreach (var id in arc2) Assert.True(byId.ContainsKey(id), $"Arc 2 missing {id}");
00074:
00075:             // Arc 3 — The Interior Caches (4)
00076:             string[] arc3 = { "loc_forestry_survey_post", "loc_geological_core_vault", "loc_river_gauging_station", "loc_abandoned_agricultural_station" };
00077:             foreach (var id in arc3) Assert.True(byId.ContainsKey(id), $"Arc 3 missing {id}");
00078:
00079:             // Arc 4 — The Border Wire (3)
00080:             string[] arc4 = { "loc_decommissioned_signal_relay", "loc_border_checkpoint_ruins", "loc_minefield_observation_tower" };
00081:             foreach (var id in arc4) Assert.True(byId.ContainsKey(id), $"Arc 4 missing {id}");
00082:         }
00083:
00084:         [Fact]
00085:         public void AllLocationIdsAreUniqueAndFollowCanonicalPrefix()
00086:         {
00087:             var locs = LoadLocations();
00088:             var seen = new HashSet<string>(StringComparer.Ordinal);
00089:
00090:             foreach (var loc in locs)
00091:             {
00092:                 Assert.False(string.IsNullOrWhiteSpace(loc.id));
00093:                 Assert.StartsWith("loc_", loc.id);
00094:                 Assert.Matches("^[a-z0-9_]+$", loc.id);
00095:                 Assert.True(seen.Add(loc.id), $"Duplicate location ID {loc.id}");
00096:             }
00097:         }
00098:
00099:         [Fact]
00100:         public void DangerLevelsWithinValidThreeToTenRange()
00101:         {
00102:             var locs = LoadLocations();
00103:             foreach (var loc in locs)
00104:             {
00105:                 Assert.InRange(loc.dangerLevel, 3, 10);
00106:             }
00107:         }
00108:
00109:         [Fact]
00110:         public void TravelHoursWithinValidThreeToTwelveRange()
00111:         {
00112:             var locs = LoadLocations();
00113:             foreach (var loc in locs)
00114:             {
00115:                 Assert.InRange(loc.travelHours, 3.0f, 12.0f);
00116:             }
00117:         }
00118:
00119:         [Fact]
00120:         public void BaseRadsPerHourWithinValidTwentyToSixtyRange()
00121:         {
00122:             var locs = LoadLocations();
00123:             foreach (var loc in locs)
00124:             {
00125:                 Assert.InRange(loc.baseRadsPerHour, 20.0f, 60.0f);
00126:             }
00127:         }
00128:
00129:         [Fact]
00130:         public void AllDescriptionsMeetHighQualityDensityStandards()
00131:         {
00132:             var locs = LoadLocations();
00133:             foreach (var loc in locs)
00134:             {
00135:                 Assert.False(string.IsNullOrWhiteSpace(loc.displayName));
00136:                 Assert.False(string.IsNullOrWhiteSpace(loc.description));
00137:                 Assert.True(loc.description.Length >= 150, $"Description too short on {loc.id}: {loc.description.Length} chars");
00138:
00139:                 // Sentence count heuristic (period followed by space or end)
00140:                 int sentenceCount = loc.description.Split(new[] { ". ", "! ", "? " }, StringSplitOptions.RemoveEmptyEntries).Length;
00141:                 Assert.True(sentenceCount >= 3, $"Expected >= 3 sentences on {loc.id}, found {sentenceCount}");
00142:             }
00143:         }
00144:
00145:         [Fact]
00146:         public void NoForbiddenSupernaturalOrGenericTropesInDescriptions()
00147:         {
00148:             var locs = LoadLocations();
00149:             string[] forbidden = { "mysterious energy", "secret lab", "alien", "magic", "sorcery", "portal", "cursed" };
00150:
00151:             foreach (var loc in locs)
00152:             {
00153:                 string descLower = loc.description.ToLowerInvariant();
00154:                 foreach (var f in forbidden)
00155:                 {
00156:                     Assert.DoesNotContain(f, descLower);
00157:                 }
00158:             }
00159:         }
00160:
00161:         [Fact]
00162:         public void LocationsRoundTripSerialization_PreservesAllFields()
00163:         {
00164:             var locs = LoadLocations();
00165:             var serializer = new SystemTextJsonSerializer();
00166:             string json = serializer.Serialize(locs);
00167:             var deserialized = serializer.Deserialize<List<VerdictCatalogLoader.VerdictLocationEntry>>(json);
00168:
00169:             Assert.NotNull(deserialized);
00170:             Assert.Equal(15, deserialized.Count);
00171:             for (int i = 0; i < locs.Count; i++)
00172:             {
00173:                 Assert.Equal(locs[i].id, deserialized[i].id);
00174:                 Assert.Equal(locs[i].displayName, deserialized[i].displayName);
00175:                 Assert.Equal(locs[i].description, deserialized[i].description);
00176:                 Assert.Equal(locs[i].dangerLevel, deserialized[i].dangerLevel);
00177:                 Assert.Equal(locs[i].travelHours, deserialized[i].travelHours);
00178:                 Assert.Equal(locs[i].baseRadsPerHour, deserialized[i].baseRadsPerHour);
00179:             }
00180:         }
00181:     }
00182: }
```

## `Ashfall.Core.Tests/Verdict/VerdictNpcExpansionTests.cs` — 240 lines; 8,495 bytes; SHA-256 `c63225a0ebdbc8bdb493a8de624bf65d6e098095b7a9bfd5c940b2e30957c6e2`
Declaration index:
- 00012: public class VerdictNpcExpansionTests : CatalogTestBase
- 00014: private static VerdictNpcSystem LoadSystem()
- 00025: public void Catalog_Loads_All_18_Npc_Entries()
- 00033: public void All_18_Npc_Ids_Are_Unique_And_Prefixed()
- 00046: public void Original_6_Baseline_Npcs_Preserved()
- 00070: public void Plan18_Tribunal_Npcs_Preserved()
- 00091: public void All_9_Plan93_Investigation_Npcs_Present()
- 00125: public void All_Npc_Kinds_Are_Supported()
- 00143: public void All_Plan93_LocationIds_Map_To_Distinct_Verdict_Sites()
- 00168: public void GetAvailable_Filters_By_Phase_And_Flag_And_Location()
- 00202: public void Speak_Is_OneShot_And_Persists_In_State()
- 00225: public void Availability_Is_Deterministic_Across_Invocations()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Verdict;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests.Verdict
00011: {
00012:     public class VerdictNpcExpansionTests : CatalogTestBase
00013:     {
00014:         private static VerdictNpcSystem LoadSystem()
00015:         {
00016:             var system = new VerdictNpcSystem();
00017:             var files = new FileSystemIO();
00018:             var json = new SystemTextJsonSerializer();
00019:             int count = VerdictNpcCatalogLoader.LoadAndRegister(system, DataDirectory, files, json);
00020:             Assert.True(count >= 15, $"Expected at least 15 NPCs registered, got {count}");
00021:             return system;
00022:         }
00023:
00024:         [Fact]
00025:         public void Catalog_Loads_All_18_Npc_Entries()
00026:         {
00027:             var system = LoadSystem();
00028:             Assert.Equal(18, system.Catalog.Count);
00029:             Assert.True(system.Catalog.Count >= 15);
00030:         }
00031:
00032:         [Fact]
00033:         public void All_18_Npc_Ids_Are_Unique_And_Prefixed()
00034:         {
00035:             var system = LoadSystem();
00036:             var ids = system.Catalog.Select(e => e.id).ToList();
00037:             var distinct = ids.Distinct(StringComparer.Ordinal).ToList();
00038:             Assert.Equal(ids.Count, distinct.Count);
00039:             foreach (var id in ids)
00040:             {
00041:                 Assert.True(id.StartsWith("npc_"), $"NPC id '{id}' must start with npc_ prefix");
00042:             }
00043:         }
00044:
00045:         [Fact]
00046:         public void Original_6_Baseline_Npcs_Preserved()
00047:         {
00048:             var system = LoadSystem();
00049:             var baselineIds = new[]
00050:             {
00051:                 "npc_eden_vale",
00052:                 "npc_ferris_voss",
00053:                 "npc_iran_bell",
00054:                 "npc_selya_saltmarsh",
00055:                 "npc_maro_veen",
00056:                 "npc_whisper_cipher"
00057:             };
00058:
00059:             foreach (var bId in baselineIds)
00060:             {
00061:                 var npc = system.Find(bId);
00062:                 Assert.NotNull(npc);
00063:                 Assert.False(string.IsNullOrWhiteSpace(npc.name));
00064:                 Assert.False(string.IsNullOrWhiteSpace(npc.role));
00065:                 Assert.NotEmpty(npc.dialogue);
00066:             }
00067:         }
00068:
00069:         [Fact]
00070:         public void Plan18_Tribunal_Npcs_Preserved()
00071:         {
00072:             var system = LoadSystem();
00073:             var plan18Ids = new[]
00074:             {
00075:                 "npc_tomas_reid",
00076:                 "npc_elena_vane",
00077:                 "npc_kasper_holt"
00078:             };
00079:
00080:             foreach (var pId in plan18Ids)
00081:             {
00082:                 var npc = system.Find(pId);
00083:                 Assert.NotNull(npc);
00084:                 Assert.False(string.IsNullOrWhiteSpace(npc.name));
00085:                 Assert.False(string.IsNullOrWhiteSpace(npc.role));
00086:                 Assert.NotEmpty(npc.dialogue);
00087:             }
00088:         }
00089:
00090:         [Fact]
00091:         public void All_9_Plan93_Investigation_Npcs_Present()
00092:         {
00093:             var system = LoadSystem();
00094:             var plan93Ids = new[]
00095:             {
00096:                 "npc_mara_elsen",
00097:                 "npc_ilya_venn",
00098:                 "npc_garrick_daal",
00099:                 "npc_sena_korr",
00100:                 "npc_torin_rask",
00101:                 "npc_oren_varek",
00102:                 "npc_lena_rost",
00103:                 "npc_tessa_mirn",
00104:                 "npc_karel_norn"
00105:             };
00106:
00107:             foreach (var id in plan93Ids)
00108:             {
00109:                 var npc = system.Find(id);
00110:                 Assert.NotNull(npc);
00111:                 Assert.False(string.IsNullOrWhiteSpace(npc.name));
00112:                 Assert.False(string.IsNullOrWhiteSpace(npc.role));
00113:                 Assert.False(string.IsNullOrWhiteSpace(npc.gatingFlag));
00114:                 Assert.False(string.IsNullOrWhiteSpace(npc.locationId));
00115:                 Assert.InRange(npc.phaseMin, 1, 3);
00116:                 Assert.InRange(npc.dialogue.Count, 2, 4);
00117:                 foreach (var line in npc.dialogue)
00118:                 {
00119:                     Assert.False(string.IsNullOrWhiteSpace(line));
00120:                 }
00121:             }
00122:         }
00123:
00124:         [Fact]
00125:         public void All_Npc_Kinds_Are_Supported()
00126:         {
00127:             var system = LoadSystem();
00128:             var validKinds = new HashSet<string>(StringComparer.Ordinal)
00129:             {
00130:                 "paper_ghost",
00131:                 "tape_echo",
00132:                 "living",
00133:                 "readings"
00134:             };
00135:
00136:             foreach (var npc in system.Catalog)
00137:             {
00138:                 Assert.Contains(npc.kind, validKinds);
00139:             }
00140:         }
00141:
00142:         [Fact]
00143:         public void All_Plan93_LocationIds_Map_To_Distinct_Verdict_Sites()
00144:         {
00145:             var system = LoadSystem();
00146:             var expectedSiteMappings = new Dictionary<string, string>(StringComparer.Ordinal)
00147:             {
00148:                 ["npc_mara_elsen"] = "loc_abandoned_tide_gauge",
00149:                 ["npc_ilya_venn"] = "loc_coastal_meteorological_station",
00150:                 ["npc_garrick_daal"] = "loc_clifftop_observation_bunker",
00151:                 ["npc_sena_korr"] = "loc_sealed_marine_laboratory",
00152:                 ["npc_torin_rask"] = "loc_forestry_survey_post",
00153:                 ["npc_oren_varek"] = "loc_geological_core_vault",
00154:                 ["npc_lena_rost"] = "loc_river_gauging_station",
00155:                 ["npc_tessa_mirn"] = "loc_abandoned_agricultural_station",
00156:                 ["npc_karel_norn"] = "loc_decommissioned_signal_relay"
00157:             };
00158:
00159:             foreach (var kvp in expectedSiteMappings)
00160:             {
00161:                 var npc = system.Find(kvp.Key);
00162:                 Assert.NotNull(npc);
00163:                 Assert.Equal(kvp.Value, npc.locationId);
00164:             }
00165:         }
00166:
00167:         [Fact]
00168:         public void GetAvailable_Filters_By_Phase_And_Flag_And_Location()
00169:         {
00170:             var system = LoadSystem();
00171:             const string npcId = "npc_garrick_daal";
00172:             var npc = system.Find(npcId);
00173:             Assert.NotNull(npc);
00174:             Assert.Equal(2, npc.phaseMin);
00175:             Assert.Equal("flag_verdict_cliff_signal_decoded", npc.gatingFlag);
00176:             Assert.Equal("loc_clifftop_observation_bunker", npc.locationId);
00177:
00178:             var flags = new[] { "flag_verdict_cliff_signal_decoded" };
00179:
00180:             // Phase 1 -> hidden (requires phase 2)
00181:             var p1 = system.GetAvailable(flags, 1, npc.locationId);
00182:             Assert.DoesNotContain(p1, e => e.id == npcId);
00183:
00184:             // Phase 2, flag missing -> hidden
00185:             var noFlag = system.GetAvailable(Array.Empty<string>(), 2, npc.locationId);
00186:             Assert.DoesNotContain(noFlag, e => e.id == npcId);
00187:
00188:             // Phase 2, flag present, wrong location -> hidden
00189:             var wrongLoc = system.GetAvailable(flags, 2, "loc_abandoned_tide_gauge");
00190:             Assert.DoesNotContain(wrongLoc, e => e.id == npcId);
00191:
00192:             // Phase 2, flag present, right location -> visible
00193:             var valid = system.GetAvailable(flags, 2, npc.locationId);
00194:             Assert.Contains(valid, e => e.id == npcId);
00195:
00196:             // Phase 3, flag present, right location -> visible
00197:             var p3 = system.GetAvailable(flags, 3, npc.locationId);
00198:             Assert.Contains(p3, e => e.id == npcId);
00199:         }
00200:
00201:         [Fact]
00202:         public void Speak_Is_OneShot_And_Persists_In_State()
00203:         {
00204:             var system = LoadSystem();
00205:             const string npcId = "npc_mara_elsen";
00206:
00207:             // Speak at correct location
00208:             bool first = system.Speak(npcId, "loc_abandoned_tide_gauge");
00209:             Assert.True(first);
00210:
00211:             // Speak second time -> false (one-shot)
00212:             bool second = system.Speak(npcId, "loc_abandoned_tide_gauge");
00213:             Assert.False(second);
00214:
00215:             // Round-trip state
00216:             var state = system.CaptureState();
00217:             Assert.Contains(npcId, state.spokenNpcIds);
00218:
00219:             var newSystem = LoadSystem();
00220:             newSystem.RestoreState(state);
00221:             Assert.False(newSystem.Speak(npcId, "loc_abandoned_tide_gauge"));
00222:         }
00223:
00224:         [Fact]
00225:         public void Availability_Is_Deterministic_Across_Invocations()
00226:         {
00227:             var system = LoadSystem();
00228:             var allFlags = system.Catalog.Select(e => e.gatingFlag).Where(f => !string.IsNullOrEmpty(f)).ToList();
00229:
00230:             var run1 = system.GetAvailable(allFlags, 3);
00231:             var run2 = system.GetAvailable(allFlags, 3);
00232:
00233:             Assert.Equal(run1.Count, run2.Count);
00234:             for (int i = 0; i < run1.Count; i++)
00235:             {
00236:                 Assert.Equal(run1[i].id, run2[i].id);
00237:             }
00238:         }
00239:     }
00240: }
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

## `Ashfall.Core.Tests/Verdict/Plan82_67VerdictCassetteIntegrationTests.cs` — 171 lines; 7,636 bytes; SHA-256 `f408e9b34a96d7826acfcd7cd1a88d0a81f07102c1ab76a1bdc41bd8a1eb4e10`
Declaration index:
- 00014: public class Plan82_67VerdictCassetteIntegrationTests : CatalogTestBase
- 00017: public void VerdictLocationsAndCassetteCatalog_LoadAccurately_WithoutCollisions()
- 00043: public void CassettePlaybackSystem_AcquireAndPlaySequence_GrantsMoraleAndCompletesSet()
- 00094: public void VerdictCartographyToCassetteScavenging_CrossSystemLinkage()
- 00129: public void CassettePlaybackSystem_SaveRestoreRoundTrip_PreservesState()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Audio;
00008: using Ashfall.Core.IO;
00009: using Ashfall.Core.Verdict;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests
00013: {
00014:     public class Plan82_67VerdictCassetteIntegrationTests : CatalogTestBase
00015:     {
00016:         [Fact]
00017:         public void VerdictLocationsAndCassetteCatalog_LoadAccurately_WithoutCollisions()
00018:         {
00019:             // Plan 82: 15 Verdict Sites
00020:             var locations = VerdictCatalogLoader.LoadLocations(
00021:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
00022:             Assert.NotNull(locations);
00023:             Assert.Equal(15, locations.Count);
00024:
00025:             // Plan 67: 12 Cassette Sets with 48 parts
00026:             var sets = CassetteSetCatalogLoader.Load(DataDirectory);
00027:             Assert.NotNull(sets);
00028:             Assert.Equal(12, sets.Count);
00029:
00030:             // Verify specific cross-system narrative locations
00031:             var tapeSilo = locations.FirstOrDefault(l => l.id == "loc_archive_tape_silo");
00032:             Assert.NotNull(tapeSilo);
00033:             Assert.Equal("The Archive Tape-Silo", tapeSilo.displayName);
00034:             Assert.Equal(9, tapeSilo.dangerLevel);
00035:             Assert.Contains("CURRENT YEAR", tapeSilo.description);
00036:
00037:             var fuseWorld = locations.FirstOrDefault(l => l.id == "loc_network_fuse_bunker");
00038:             Assert.NotNull(fuseWorld);
00039:             Assert.Contains("tape-silo door", fuseWorld.description);
00040:         }
00041:
00042:         [Fact]
00043:         public void CassettePlaybackSystem_AcquireAndPlaySequence_GrantsMoraleAndCompletesSet()
00044:         {
00045:             var sets = CassetteSetCatalogLoader.Load(DataDirectory);
00046:             var vinylMorale = new VinylMoraleSystem();
00047:             var playbackSystem = new CassettePlaybackSystem(vinylMorale);
00048:             playbackSystem.LoadCatalog(sets);
00049:
00050:             // Track events
00051:             int playedEvents = 0;
00052:             CassetteSetDefinition? completedSet = null;
00053:             playbackSystem.OnTapePlayed += (part, set, morale) => playedEvents++;
00054:             playbackSystem.OnSetCompleted += set => completedSet = set;
00055:
00056:             // Acquire parts 1, 2, 3 of checkpoint_kilo
00057:             Assert.True(playbackSystem.AcquirePart("cassette_checkpoint_kilo_1"));
00058:             Assert.True(playbackSystem.AcquirePart("cassette_checkpoint_kilo_2"));
00059:             Assert.True(playbackSystem.AcquirePart("cassette_checkpoint_kilo_3"));
00060:
00061:             playbackSystem.GetSetProgress("checkpoint_kilo", out int collected, out int total);
00062:             Assert.Equal(3, collected);
00063:             Assert.Equal(4, total);
00064:             Assert.False(playbackSystem.IsSetComplete("checkpoint_kilo"));
00065:
00066:             // Play part 1
00067:             var playRes1 = playbackSystem.PlayPart("cassette_checkpoint_kilo_1");
00068:             Assert.Equal(ActionResult.StatusKind.Success, playRes1.Status);
00069:             Assert.True(playbackSystem.IsPartPlayed("cassette_checkpoint_kilo_1"));
00070:             Assert.Equal(1, playedEvents);
00071:
00072:             // Playing unowned part is blocked
00073:             var playBlocked = playbackSystem.PlayPart("cassette_checkpoint_kilo_4");
00074:             Assert.Equal(ActionResult.StatusKind.Blocked, playBlocked.Status);
00075:
00076:             // Acquire final part 4 -> triggers completion
00077:             Assert.True(playbackSystem.AcquirePart("cassette_checkpoint_kilo_4"));
00078:             Assert.True(playbackSystem.IsSetComplete("checkpoint_kilo"));
00079:             Assert.NotNull(completedSet);
00080:             Assert.Equal("checkpoint_kilo", completedSet.set_id);
00081:
00082:             // Verify cache disclosure
00083:             var discoveredCaches = playbackSystem.GetDiscoveredCacheLocations();
00084:             Assert.Contains("checkpoint_kilo_armory", discoveredCaches);
00085:
00086:             var cacheItems = playbackSystem.GetCacheItems("checkpoint_kilo_armory");
00087:             Assert.Equal(3, cacheItems.Count);
00088:             Assert.Contains("military_mre", cacheItems);
00089:             Assert.Contains("ammo_556", cacheItems);
00090:             Assert.Contains("field_surgical_kit", cacheItems);
00091:         }
00092:
00093:         [Fact]
00094:         public void VerdictCartographyToCassetteScavenging_CrossSystemLinkage()
00095:         {
00096:             var locations = VerdictCatalogLoader.LoadLocations(
00097:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
00098:             var sets = CassetteSetCatalogLoader.Load(DataDirectory);
00099:
00100:             var playback = new CassettePlaybackSystem();
00101:             playback.LoadCatalog(sets);
00102:
00103:             // Find high-value tape locations in verdict cartography
00104:             var tapeSilo = locations.First(l => l.id == "loc_archive_tape_silo");
00105:             Assert.True(tapeSilo.dangerLevel >= 8);
00106:             Assert.True(tapeSilo.baseRadsPerHour >= 40.0f);
00107:
00108:             // Recover Station 14 broadcasts preserved in archive
00109:             var station14Set = sets.First(s => s.set_id == "station_14");
00110:             Assert.Equal(6, station14Set.total_parts);
00111:
00112:             foreach (var part in station14Set.parts)
00113:             {
00114:                 Assert.True(playback.AcquirePart(part.item_id));
00115:                 var res = playback.PlayPart(part.item_id);
00116:                 Assert.Equal(ActionResult.StatusKind.Success, res.Status);
00117:             }
00118:
00119:             Assert.True(playback.IsSetComplete("station_14"));
00120:             var mastCache = playback.GetDiscoveredCacheLocations();
00121:             Assert.Contains("loc_radio_relay_mast", mastCache);
00122:
00123:             var mastItems = playback.GetCacheItems("loc_radio_relay_mast");
00124:             Assert.Contains("civil_defense_radio", mastItems);
00125:             Assert.Contains("aa_batteries", mastItems);
00126:         }
00127:
00128:         [Fact]
00129:         public void CassettePlaybackSystem_SaveRestoreRoundTrip_PreservesState()
00130:         {
00131:             var sets = CassetteSetCatalogLoader.Load(DataDirectory);
00132:             var sys1 = new CassettePlaybackSystem();
00133:             sys1.LoadCatalog(sets);
00134:
00135:             sys1.AcquirePart("cassette_free_radio_1");
00136:             sys1.AcquirePart("cassette_free_radio_2");
00137:             sys1.AcquirePart("cassette_free_radio_3");
00138:             sys1.AcquirePart("cassette_free_radio_4");
00139:             sys1.PlayPart("cassette_free_radio_1");
00140:             sys1.PlayPart("cassette_free_radio_2");
00141:
00142:             Assert.True(sys1.IsSetComplete("resistance_broadcasts"));
00143:
00144:             // Capture state
00145:             var state1 = sys1.CaptureState();
00146:             Assert.Equal(4, state1.collectedPartItemIds.Count);
00147:             Assert.Equal(2, state1.playedPartItemIds.Count);
00148:             Assert.Single(state1.completedSetIds);
00149:             Assert.Equal(2, state1.totalPlaybacks);
00150:             Assert.True(state1.totalMoraleAwarded > 0);
00151:
00152:             // Serialize & deserialize using SystemTextJsonSerializer
00153:             var serializer = new SystemTextJsonSerializer();
00154:             string json = serializer.Serialize(state1);
00155:             var restoredState = serializer.Deserialize<CassettePlaybackState>(json);
00156:             Assert.NotNull(restoredState);
00157:
00158:             // Restore into new system instance
00159:             var sys2 = new CassettePlaybackSystem();
00160:             sys2.LoadCatalog(sets);
00161:             sys2.RestoreState(restoredState);
00162:
00163:             Assert.True(sys2.IsSetComplete("resistance_broadcasts"));
00164:             Assert.True(sys2.IsPartCollected("cassette_free_radio_3"));
00165:             Assert.True(sys2.IsPartPlayed("cassette_free_radio_1"));
00166:             Assert.False(sys2.IsPartPlayed("cassette_free_radio_3"));
00167:             Assert.Equal(sys1.State.totalPlaybacks, sys2.State.totalPlaybacks);
00168:             Assert.Equal(sys1.State.totalMoraleAwarded, sys2.State.totalMoraleAwarded);
00169:         }
00170:     }
00171: }
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
