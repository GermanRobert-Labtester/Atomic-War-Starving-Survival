# Plan 39 — Orbital Harrow Telemetry: Twelve-Event Catalog, Detection-to-Impact Authority, and Salvage Consequences

> **Rebuild status:** TERMINAL 12-EVENT CONTENT + LIVE SCHEDULING/REACHABILITY AUDIT
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

The historical baseline was 4,858 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Keep the twelve authored orbital-harrow telemetry events as the detection catalog while preserving `OrbitalHarrowTelemetrySystem` as the sole warning, impact, armor-mitigation, salvage, and persistence authority. The historical “12 events + 8 consequence records” brief is partly stale: the current catalog has twelve event rows and consequence behavior is expressed by system fields, not a second consequence catalog.

**Bounded outcome:** Audit the current event DTO/catalog, telemetry state, armor system, weather-intelligence/world host, flagship institutions, radio hooks, UI/salvage routes, and focused tests. Identify whether authored events are scheduled in a real campaign or currently only through demos/selftests, and plan the smallest safe scheduling seam if justified.

**Non-goals:** no second orbital event/consequence catalog, no unseeded strike randomness, no new armor owner, no arbitrary event count, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `orbital_harrow_events.json` contains 12 event rows, not the historical 12+8 two-table shape.
- VERIFIED CURRENT: `OrbitalHarrowTelemetrySystem` owns activation, scheduling, impact resolution, salvage, and capture/restore.
- VERIFIED CURRENT: `SkyLayerArmorSystem` is the armor mitigation owner.
- Current authored-event scheduling is an explicit production-caller premise question; demo/selftest callers are not automatically live campaign reachability.
- HISTORICAL RECORD: Plan 39/Wave 20 records the event-catalog implementation; this package does not claim a fresh run.

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

- `Assets/StreamingAssets/Data/orbital_harrow_events.json` exists at 9,102 bytes; SHA-256 `3b9d4b30dc64569dde7bbc737c7a33a0b33dc013b9ce1c09b965f936919d2a88`.
- `Assets/StreamingAssets/Data/sky_layer_armor_catalog.json` exists at 5,475 bytes; SHA-256 `b4085538fc1f4fc58718464d5d5f981becccc16acb71bb74da0c5e80e020d33a`.
- `Assets/StreamingAssets/Data/radio_intercepts.json` exists at 13,716 bytes; SHA-256 `9123e10212b21db35ebf451984908ae6ed5780cc766cae56caff2083905749f9`.
- `Assets/StreamingAssets/Data/items.json` exists at 390,056 bytes; SHA-256 `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`.

# 3. Required Delta

Replace the stale 12-event-plus-8-consequence brief with a current twelve-row catalog census, system-owned consequence map, and live scheduling audit. Preserve one telemetry/armor chain.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Which current production path loads and schedules authored event rows, if any?
How does `OrbitalHarrowTelemetrySystem` apply armor and salvage, and what state is persisted?
Are false-positive and radio-hook semantics explicit in current data/owner code?
Which current UI surface exposes warning/lead time and salvage truthfully?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| event definitions and loading | `OrbitalHarrowCatalogLoader` | `Assets/Ashfall.Core/Shelter/OrbitalHarrowCatalog.cs` | Authored event metadata; no second consequence owner. |
| warning/impact/salvage state and armor resolution | `OrbitalHarrowTelemetrySystem` | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | Owns pending impact, warnings, mitigation, salvage, and capture/restore. |
| armor penetration/mitigation | `SkyLayerArmorSystem` | `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` | Owns armor condition and interception effects. |
| world/weather host composition | `WeatherIntelligenceCoordinator / WorldHostSession` | `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs; src/Host/WorldHostSession.cs` | Composes current telemetry and routes facts. |
| campaign/institution lifecycle | `Main.FlagshipInstitutions / Main.World` | `src/Main.FlagshipInstitutions.cs; src/Main.World.cs` | Current owner setup, save, and event routing. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for orbital-harrow telemetry loop.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Orbital Harrow Telemetry: Twelve-Event Catalog, Detection-to-Impact Authority, and Salvage Consequences                                               │
│ Integration route: DATA-ONLY + current scheduling/owner audit                             │
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

1. **Catalog defines events.**
2. **Telemetry owns warning/impact/salvage.**
3. **Armor owns mitigation.**
4. **World/radio consume facts.**
5. **Existing persistence remains authoritative.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| event definitions and loading | `OrbitalHarrowCatalogLoader` | `Assets/Ashfall.Core/Shelter/OrbitalHarrowCatalog.cs` | Authored event metadata; no second consequence owner. |
| warning/impact/salvage state and armor resolution | `OrbitalHarrowTelemetrySystem` | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | Owns pending impact, warnings, mitigation, salvage, and capture/restore. |
| armor penetration/mitigation | `SkyLayerArmorSystem` | `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` | Owns armor condition and interception effects. |
| world/weather host composition | `WeatherIntelligenceCoordinator / WorldHostSession` | `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs; src/Host/WorldHostSession.cs` | Composes current telemetry and routes facts. |
| campaign/institution lifecycle | `Main.FlagshipInstitutions / Main.World` | `src/Main.FlagshipInstitutions.cs; src/Main.World.cs` | Current owner setup, save, and event routing. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load a catalog event through OrbitalHarrowCatalogLoader
2. activate telemetry on a current campaign day
3. schedule a typed event/impact through OrbitalHarrowTelemetrySystem
4. surface warning/lead time through current world/radio/journal seams
5. on impact, ask SkyLayerArmorSystem to resolve penetration/mitigation
6. record salvage/aftermath through the existing owner and persist weather-intelligence/telemetry state

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- event ids are unique and signal/severity/lead-time fields are bounded
- a false-positive row cannot produce a real impact
- lead time is derived from the current owner and never shortened by UI
- armor mitigation is applied by SkyLayerArmorSystem, not by catalog prose
- salvage is a typed owner result and inventory delivery is canonical
- pending impact/warnings/salvage/revealed sites survive save/load
- same seed and schedule produce the same warning/impact trace

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

Contract rules for orbital-harrow telemetry loop:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`orbital_harrow_events.json` remains the twelve-row event authority. The current row shape contains signal type, false-positive flag, energy, lead time, spread, penetration, salvage, revealed site, and radio hook. The historical eight consequence rows are not a current separate schema; consequence behavior is evaluated by the telemetry/armor owners. Any future event needs a real schedule/consumer and continuity-safe consequence fields.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

Telemetry state is captured by the current weather-intelligence/world persistence path through `CaptureState`/`RestoreState`; do not add a second orbital section for static events. Any future campaign schedule must use the existing owner state and preserve pending impact/warnings/salvage exactly once.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

The telemetry owner receives `ISeededRng`; false-positive and salvage decisions must be seeded and ordinal-stable. Catalog order cannot decide a tie by hash iteration. Paired replay compares schedule, warning lead, impact resolution, armor result, salvage, and revealed sites.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

OrbitalHarrowTelemetrySystem owns warning/impact/salvage events. World/radio/journal/flagship surfaces consume the facts. A radio hook may announce a signal but cannot resolve the impact; SkyLayerArmorSystem owns the armor result; inventory receives salvage only through the canonical sink.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Host/WorldHostSession.cs` — current world weather-intelligence and orbital demo/command seam
- `src/Main.FlagshipInstitutions.cs` — composes orbital telemetry with current institutions
- `src/Main.World.cs` — binds orbital facts to power/EMP/current world consumers
- `src/UI/RadioIntelligencePanel.cs` — presents current orbital/radio intelligence
- `src/Host/HostCli.SkyDefense.cs` — diagnostic surface only unless a future claim promotes it

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Telemetry prose should communicate uncertainty and physical consequence without turning orbital machinery into omnipotent prophecy. A radio hook can be ambiguous; the current owner’s signal type and false-positive state determine whether the player is warned or misled.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs`

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
| Phase 0 — event/system census | read 12 rows, catalog, telemetry, armor, host, and tests | current row and consequence shapes are explicit | no undocumented scope or shortcut |
| Phase 1 — live scheduling trace | follow activation/schedule/warning/impact/salvage in production | live versus demo-only behavior is classified | no undocumented scope or shortcut |
| Phase 2 — save/determinism/accessibility audit | check pending state, replay, warnings, salvage, and truthful UI | no shadow owner or false consequence | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a proven scheduling or presentation gap is promoted | one claim and focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/orbital_harrow_events.json` | READ ONLY; MODIFY only for a proven event/consumer defect | retain as event authority |
| `Assets/Ashfall.Core/Shelter/OrbitalHarrowCatalog.cs` | READ ONLY | event loader |
| `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | READ ONLY | sole telemetry owner |
| `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` | READ ONLY | armor owner |
| `src/Main.World.cs` | READ ONLY | current consequence routing |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| parallel consequence model | keep telemetry/armor owners authoritative |
| unseeded orbital randomness | use existing seeded stream |
| demo mistaken for campaign route | trace production callers |
| salvage duplication | use owner exactly-once state |

# 22. Explicit Non-Goals

- no second orbital event/consequence catalog, no unseeded strike randomness, no new armor owner, no arbitrary event count, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for orbital-harrow telemetry loop are named from current evidence.
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

- A current twelve-row event-to-consumer and consequence matrix.
- An explicit classification of demo-only versus production scheduling.
- A bounded scheduling/save/determinism package only if current evidence proves a gap.

## MUST NOT DO

- create a second consequence catalog
- make a radio hook the impact authority
- mutate armor from UI
- add a new save section for static event rows

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read the twelve event rows, OrbitalHarrowCatalogLoader, OrbitalHarrowTelemetrySystem, SkyLayerArmorSystem, WorldHostSession, and focused tests; trace schedule and consequence paths.

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

### Authority lines 158–163
00158: | C13 | Reckoning evidence enrollment for systems added since the last endgame wave (19A/19B/19C closed, DR-06) | HIGH CONFIDENCE |
00159: | C14 | Trapping→disease zoonosis bridge exists; open: migration × expedition route encounters; infestation × crop economy | PROPOSAL |
00160: | C15 | EMP effects exist (shelter EMP/medical power logs observed); open: defense grid × warlord siege math; sky-armor × orbital harrow telemetry | PROPOSAL |
00161: | C16 | XP Expansion W1 is ACTIVE (DR-06): difficulty-authority consumer binding is the sanctioned open seam in this cluster — extend it, do not parallel it | HIGH CONFIDENCE |
00162: | C17 | Panels rendering stale or missing data for newer systems; verify against `--ui-layout-selftest` before claiming | HIGH CONFIDENCE |
00163:

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

### Authority lines 664–669
00664: Each seed is a compressed subject plan: subject, evidence anchor, and route tier. Seeds are consumed through the Factory Protocol (Part II): premise sweep, then Template S expansion, then Template R route. Evidence anchors cite the live repository (verified in the 2026-09-24 audit or carried from the v1.0 bible's verified inventories). Seeds marked GATE require a named signature before drafting; seeds marked SEALED sit on closed surfaces and open only with new evidence plus foreman signature. Every seed carries the standing constraints of Part 0.4 v1.0: no parallel authority, no new ids without collision sweep, no wall-clock randomness, owner-first extension.
00665:
00666: ## 2.1 Lane A — Narrative and prose seeds (A-01 … A-30)
00667:
00668: **A-01 · C1 · Bunker maintenance glitch batch N+1.** Subject: additional `bunker_maintenance_glitches` batches for shelter rooms that gained systems in Waves 8–12 (EMP feed, medical power, grid catalog seal — logs observed in `docs/plans/`). Evidence: glitch batches 2 and 3 exist in the narrative corpus; room coverage is enumerable from `shelter_rooms.json`. Route: DATA-ONLY into the existing glitch corpus family. Confidence: HIGH CONFIDENCE.
00669:

### Authority lines 770–775
00770: **B-21 · C14 · Migration ↔ route-encounter bridge.** Subject: wildlife migration state conditioning travel-encounter selection on routes crossing migration corridors. Evidence: `WildlifeMigrationSystem`, `travel_encounters.json` both canon. Route: CORE-EXTENSION. Confidence: PROPOSAL.
00771:
00772: **B-22 · C15 · Defense-grid ↔ siege math.** Subject: perimeter defense values entering warlord siege/raid resolution; sky-armor values entering orbital-harrow telemetry thresholds. Evidence: warlord siege math and orbital harrow telemetry are canon systems; catalogs live. Route: CORE-EXTENSION. Confidence: PROPOSAL.
00773:
00774: **B-23 · C16 · Difficulty binding consumers (CF-XP01).** Subject: complete difficulty preset scalar consumer binding across systems — the ledger records this line as available and W1-reinforced. Evidence: DR-06. Route: CORE-EXTENSION through the difficulty authority only. Confidence: HIGH CONFIDENCE.
00775:

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

### Authority lines 959–964
00959: **DM-14 — Ecology and wildlife (C14).** Owners: migration, trapping, ecosystem, seasonal calendar, bestiary, underground flora, infestations, contagion, pathogens, crop genomes. Live catalogs: `wildlife_ecosystem`, `wildlife_trapping_catalog`, `wasteland_wildlife_bestiary`, `underground_flora`, `ecological_infestations`, `contagion_events`, `pathogens`, `crop_strains`, `mutations`. Hosts: WildlifeEcosystem, WildlifeTrapping. Openings: A-29, B-21, C-10, plus the F-002 blight arc. Constraint: zoonosis bridge and campfire sanitization are the owned seams.
00960:
00961: **DM-15 — Defense and security (C15).** Owners: perimeter defenses, defense grid, sky defense ordnance and armor, chemical defense, orbital harrow telemetry, interlocks, EMP effects. Live catalogs: `perimeter_defenses`, `defenses`, `sky_defense_ordnance`, `sky_layer_armor_catalog`, `chemical_weapons`, `orbital_harrow_events`, `railway_interlock_catalog`. Hosts: DefenseGrid, SkyDefense, ChemWarfareDefense, OrbitalHarrowTelemetrySystem. Openings: A-30, B-22. Constraint: sky-armor-to-weather bridge already partially built; verify before extending.
00962:
00963: **DM-16 — Progression and meta (C16).** Owners: skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, L10N, mods, settings, input, cohort tuning, apprenticeship, library study. Live catalogs: `skills`, `research_knowledge`, `collectibles`, `trophies`, `difficulty_presets`, `cohort_tuning`, `apprenticeship_catalog`, `library_manuals`, `cultural_archive_tomes`, `codex_entries`, `field_guide`. Hosts: Codex, Research, Collectibles, Difficulty, Apprenticeship, LibraryStudy, Mods, Onboarding, StartingLevel. Openings: B-06, B-23, C-11, D-08, E-03, G-08, J-02. Constraint: XP W1 owns difficulty authority while ACTIVE.
00964:

### Authority lines 5147–5152
05147: | A-28 | C13 | Epilogue chronicle depth | OPEN (consumed by F-005) | D |
05148: | A-29 | C14 | Bestiary continuation | OPEN | D |
05149: | A-30 | C15 | Sky-defense manifests | OPEN (corpus sweep first) | D |
05150: | A-31 | C9 | Institution archive corpus (contract in V32) | OPEN | D |
05151: | A-32 | C7 | Summit protocol documents (contract in V32) | OPEN | D |
05152: | A-41 | C10 | Faction-branching de-templatization | OPEN (segment reads block tranches) | D |

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/orbital_harrow_events.json`
- Bytes: 9,102; SHA-256: `3b9d4b30dc64569dde7bbc737c7a33a0b33dc013b9ce1c09b965f936919d2a88`
- Root keys: `events, schema_version`
- `events`: list[12]; union fields: `affected_cell_spread, description, id, impact_energy_mj, is_false_positive, lead_time_days, name, penetration_power_mj, radio_hook_text, revealed_site_id, salvage_yield_item_id, salvage_yield_quantity, severity, signal_type`
  - row 1: `{"affected_cell_spread":1,"description":"The perimeter radar locks onto something falling — a tungsten rod, left over from a war fought by people who never intended to survive it. It tumbles through decaying orbit with the lazy patience of a thing that has all the time in the world.","id":"event_orbital_kinetic_early_track","impact_energy_mj":22.0,"is_false_positive":false,"lead_time_days":5,"name":"High-Altitude Radar Track Lock","penetration_power_mj":18.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_utility_tunnels","salvage_yield_item_id":"scrap_mechanical","salvage_yield_quantity":4,"severity":"Moderate","signal_type":"radar_…`
  - row 2: `{"affected_cell_spread":1,"description":"A white-hot streak tears across the sky like a crack in the firmament, visible even through the ash haze. The thermal imagers scream as something plunges through the mesosphere, aimed directly at Sector 4 — and there is absolutely nothing anyone can do but watch it fall.","id":"event_orbital_kinetic_thermal_descent","impact_energy_mj":45.0,"is_false_positive":false,"lead_time_days":2,"name":"Thermal Descent Plasma Signature","penetration_power_mj":38.0,"radio_hook_text":"THERMAL ALERT: Ionization track confirmed at 80 km... descent vector locked.","revealed_site_id":"loc_excavation_command_vault","sal…`
  - row 3: `{"affected_cell_spread":1,"description":"The walls hum. A deep, bass vibration that rattles teeth and loosens bolts — the bedrock itself flinching as a military penetrator screams through the upper atmosphere, punching a shockwave ahead of it into the earth.","id":"event_orbital_kinetic_seismic_precursor","impact_energy_mj":38.0,"is_false_positive":false,"lead_time_days":1,"name":"Deep Infrasound Shock Precursor","penetration_power_mj":32.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_mine_shaft","salvage_yield_item_id":"heavy_industrial_motor","salvage_yield_quantity":1,"severity":"Severe","signal_type":"seismic_precursor"}`
  - row 4: `{"affected_cell_spread":2,"description":"The radar track splits into two. A tungsten rod has fractured during reentry, its halves tumbling apart on diverging trajectories — each one still carrying enough kinetic energy to punch through four metres of reinforced concrete.","id":"event_orbital_kinetic_fragmented_track","impact_energy_mj":28.0,"is_false_positive":false,"lead_time_days":3,"name":"Fragmented Ballast Rod Scatter","penetration_power_mj":22.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_storage_chamber","salvage_yield_item_id":"scrap_mechanical","salvage_yield_quantity":5,"severity":"Moderate","signal_type":"radar_anomaly…`
  - row 5: `{"affected_cell_spread":4,"description":"A dead satellite breaks apart in the upper atmosphere, scattering a constellation of burning fragments across the shelter's roof quadrants. It looks like fireworks — the last fireworks anyone will ever see — and each one hits like a mortar round.","id":"event_orbital_cluster_multiple_returns","impact_energy_mj":24.0,"is_false_positive":false,"lead_time_days":4,"name":"Telemetry Station Cluster Shower","penetration_power_mj":18.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_metro_interchange","salvage_yield_item_id":"copper_wire_10m_of_10m","salvage_yield_quantity":3,"severity":"Moderate","s…`
  - row 6: `{"affected_cell_spread":3,"description":"Infrared tracking captures a single incoming signature deploying three distinct submunition reentry vehicles across shelter perimeter coordinates.","id":"event_orbital_cluster_split_track","impact_energy_mj":36.0,"is_false_positive":false,"lead_time_days":3,"name":"Defense Submunition Cluster Strike","penetration_power_mj":28.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_drainage_network","salvage_yield_item_id":"mechanical_parts","salvage_yield_quantity":5,"severity":"Severe","signal_type":"thermal_signature"}`
  - row 7: `{"affected_cell_spread":3,"description":"Static cascades across all shortwave frequencies as a degrading high-altitude nuclear battery discharges ionized burst energy above the troposphere.","id":"event_orbital_emp_radio_blackout","impact_energy_mj":14.0,"is_false_positive":false,"lead_time_days":3,"name":"High-Altitude EMP Airburst Precursor","penetration_power_mj":8.0,"radio_hook_text":"COMM STATIC: Automated orbital battery discharge sequence active... EMP burst warning...","revealed_site_id":"loc_excavation_civilian_shelter","salvage_yield_item_id":"battery","salvage_yield_quantity":4,"severity":"Moderate","signal_type":"rf_interference"}`
  - row 8: `{"affected_cell_spread":2,"description":"Sensors report high electromagnetic flux paired with a weak kinetic profile, indicating an airburst plasma shockwave that threatens unshielded busbars.","id":"event_orbital_emp_signature_mismatch","impact_energy_mj":28.0,"is_false_positive":false,"lead_time_days":2,"name":"Ionized Plasma Arc Shockwave","penetration_power_mj":20.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_archive_bunker","salvage_yield_item_id":"fuel","salvage_yield_quantity":4,"severity":"Severe","signal_type":"thermal_signature"}`
  - row 9: `{"affected_cell_spread":1,"description":"An automated pre-war dead-hand satellite pings continuous binary activation sequences, initiating an emergency orbital repositioning maneuver.","id":"event_orbital_dead_hand_repeating_ping","impact_energy_mj":20.0,"is_false_positive":false,"lead_time_days":4,"name":"Automated Dead-Hand Command Relay","penetration_power_mj":15.0,"radio_hook_text":"CARRIER DETECT: 4.882 MHz repeating digital chirp — PROTOCOL CRIMSON-SEVEN CONFIRMED.","revealed_site_id":"loc_excavation_command_vault","salvage_yield_item_id":"scrap_electronic","salvage_yield_quantity":5,"severity":"Moderate","signal_type":"dead_hand_ping"}`
  - row 10: `{"affected_cell_spread":2,"description":"A decaying military command platform transmits corrupt telemetry packets while autonomously releasing ballast penetrators on an unguided descent trajectory.","id":"event_orbital_dead_hand_broken_checksum","impact_energy_mj":32.0,"is_false_positive":false,"lead_time_days":3,"name":"Corrupted Defense Grid Ping","penetration_power_mj":25.0,"radio_hook_text":"INTERCEPT: Garbled framing packet... CHECKSUM INVALID... autonomous descent vector engaged.","revealed_site_id":"loc_excavation_archive_bunker","salvage_yield_item_id":"scrap_electronic","salvage_yield_quantity":6,"severity":"Severe","signal_type":"d…`
  - row 11: `{"affected_cell_spread":1,"description":"Atmospheric inversion creates a false radar phantom mimicking a descending ballast rod. Tracking disappears harmlessly upon sensor recalibration.","id":"event_orbital_radar_ducting_false_alarm","impact_energy_mj":0.0,"is_false_positive":true,"lead_time_days":3,"name":"Tropospheric Radar Ducting Echo","penetration_power_mj":0.0,"radio_hook_text":"","revealed_site_id":"","salvage_yield_item_id":"scrap_mechanical","salvage_yield_quantity":1,"severity":"Minor","signal_type":"radar_anomaly"}`
  - row 12: `{"affected_cell_spread":1,"description":"A shredded high-altitude foil meteorological balloon enters radar coverage, triggering an anomalous threat warning that dissolves harmlessly in upper winds.","id":"event_orbital_debris_misclassification","impact_energy_mj":0.0,"is_false_positive":true,"lead_time_days":2,"name":"Decaying Weather Balloon Misclassification","penetration_power_mj":0.0,"radio_hook_text":"","revealed_site_id":"","salvage_yield_item_id":"scrap_metal","salvage_yield_quantity":1,"severity":"Minor","signal_type":"radar_anomaly"}`
- Bytes: 9,102; SHA-256: `3b9d4b30dc64569dde7bbc737c7a33a0b33dc013b9ce1c09b965f936919d2a88`
- Root keys: `events, schema_version`
- `events`: list[12]; union fields: `affected_cell_spread, description, id, impact_energy_mj, is_false_positive, lead_time_days, name, penetration_power_mj, radio_hook_text, revealed_site_id, salvage_yield_item_id, salvage_yield_quantity, severity, signal_type`
  - row 1: `{"affected_cell_spread":1,"description":"The perimeter radar locks onto something falling — a tungsten rod, left over from a war fought by people who never intended to survive it. It tumbles through decaying orbit with the lazy patience of a thing that has all the time in the world.","id":"event_orbital_kinetic_early_track","impact_energy_mj":22.0,"is_false_positive":false,"lead_time_days":5,"name":"High-Altitude Radar Track Lock","penetration_power_mj":18.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_utility_tunnels","salvage_yield_item_id":"scrap_mechanical","salvage_yield_quantity":4,"severity":"Moderate","signal_type":"radar_…`
  - row 2: `{"affected_cell_spread":1,"description":"A white-hot streak tears across the sky like a crack in the firmament, visible even through the ash haze. The thermal imagers scream as something plunges through the mesosphere, aimed directly at Sector 4 — and there is absolutely nothing anyone can do but watch it fall.","id":"event_orbital_kinetic_thermal_descent","impact_energy_mj":45.0,"is_false_positive":false,"lead_time_days":2,"name":"Thermal Descent Plasma Signature","penetration_power_mj":38.0,"radio_hook_text":"THERMAL ALERT: Ionization track confirmed at 80 km... descent vector locked.","revealed_site_id":"loc_excavation_command_vault","sal…`
  - row 3: `{"affected_cell_spread":1,"description":"The walls hum. A deep, bass vibration that rattles teeth and loosens bolts — the bedrock itself flinching as a military penetrator screams through the upper atmosphere, punching a shockwave ahead of it into the earth.","id":"event_orbital_kinetic_seismic_precursor","impact_energy_mj":38.0,"is_false_positive":false,"lead_time_days":1,"name":"Deep Infrasound Shock Precursor","penetration_power_mj":32.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_mine_shaft","salvage_yield_item_id":"heavy_industrial_motor","salvage_yield_quantity":1,"severity":"Severe","signal_type":"seismic_precursor"}`
  - row 4: `{"affected_cell_spread":2,"description":"The radar track splits into two. A tungsten rod has fractured during reentry, its halves tumbling apart on diverging trajectories — each one still carrying enough kinetic energy to punch through four metres of reinforced concrete.","id":"event_orbital_kinetic_fragmented_track","impact_energy_mj":28.0,"is_false_positive":false,"lead_time_days":3,"name":"Fragmented Ballast Rod Scatter","penetration_power_mj":22.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_storage_chamber","salvage_yield_item_id":"scrap_mechanical","salvage_yield_quantity":5,"severity":"Moderate","signal_type":"radar_anomaly…`
  - row 5: `{"affected_cell_spread":4,"description":"A dead satellite breaks apart in the upper atmosphere, scattering a constellation of burning fragments across the shelter's roof quadrants. It looks like fireworks — the last fireworks anyone will ever see — and each one hits like a mortar round.","id":"event_orbital_cluster_multiple_returns","impact_energy_mj":24.0,"is_false_positive":false,"lead_time_days":4,"name":"Telemetry Station Cluster Shower","penetration_power_mj":18.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_metro_interchange","salvage_yield_item_id":"copper_wire_10m_of_10m","salvage_yield_quantity":3,"severity":"Moderate","s…`
  - row 6: `{"affected_cell_spread":3,"description":"Infrared tracking captures a single incoming signature deploying three distinct submunition reentry vehicles across shelter perimeter coordinates.","id":"event_orbital_cluster_split_track","impact_energy_mj":36.0,"is_false_positive":false,"lead_time_days":3,"name":"Defense Submunition Cluster Strike","penetration_power_mj":28.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_drainage_network","salvage_yield_item_id":"mechanical_parts","salvage_yield_quantity":5,"severity":"Severe","signal_type":"thermal_signature"}`
  - row 7: `{"affected_cell_spread":3,"description":"Static cascades across all shortwave frequencies as a degrading high-altitude nuclear battery discharges ionized burst energy above the troposphere.","id":"event_orbital_emp_radio_blackout","impact_energy_mj":14.0,"is_false_positive":false,"lead_time_days":3,"name":"High-Altitude EMP Airburst Precursor","penetration_power_mj":8.0,"radio_hook_text":"COMM STATIC: Automated orbital battery discharge sequence active... EMP burst warning...","revealed_site_id":"loc_excavation_civilian_shelter","salvage_yield_item_id":"battery","salvage_yield_quantity":4,"severity":"Moderate","signal_type":"rf_interference"}`
  - row 8: `{"affected_cell_spread":2,"description":"Sensors report high electromagnetic flux paired with a weak kinetic profile, indicating an airburst plasma shockwave that threatens unshielded busbars.","id":"event_orbital_emp_signature_mismatch","impact_energy_mj":28.0,"is_false_positive":false,"lead_time_days":2,"name":"Ionized Plasma Arc Shockwave","penetration_power_mj":20.0,"radio_hook_text":"","revealed_site_id":"loc_excavation_archive_bunker","salvage_yield_item_id":"fuel","salvage_yield_quantity":4,"severity":"Severe","signal_type":"thermal_signature"}`
  - row 9: `{"affected_cell_spread":1,"description":"An automated pre-war dead-hand satellite pings continuous binary activation sequences, initiating an emergency orbital repositioning maneuver.","id":"event_orbital_dead_hand_repeating_ping","impact_energy_mj":20.0,"is_false_positive":false,"lead_time_days":4,"name":"Automated Dead-Hand Command Relay","penetration_power_mj":15.0,"radio_hook_text":"CARRIER DETECT: 4.882 MHz repeating digital chirp — PROTOCOL CRIMSON-SEVEN CONFIRMED.","revealed_site_id":"loc_excavation_command_vault","salvage_yield_item_id":"scrap_electronic","salvage_yield_quantity":5,"severity":"Moderate","signal_type":"dead_hand_ping"}`
  - row 10: `{"affected_cell_spread":2,"description":"A decaying military command platform transmits corrupt telemetry packets while autonomously releasing ballast penetrators on an unguided descent trajectory.","id":"event_orbital_dead_hand_broken_checksum","impact_energy_mj":32.0,"is_false_positive":false,"lead_time_days":3,"name":"Corrupted Defense Grid Ping","penetration_power_mj":25.0,"radio_hook_text":"INTERCEPT: Garbled framing packet... CHECKSUM INVALID... autonomous descent vector engaged.","revealed_site_id":"loc_excavation_archive_bunker","salvage_yield_item_id":"scrap_electronic","salvage_yield_quantity":6,"severity":"Severe","signal_type":"d…`
  - row 11: `{"affected_cell_spread":1,"description":"Atmospheric inversion creates a false radar phantom mimicking a descending ballast rod. Tracking disappears harmlessly upon sensor recalibration.","id":"event_orbital_radar_ducting_false_alarm","impact_energy_mj":0.0,"is_false_positive":true,"lead_time_days":3,"name":"Tropospheric Radar Ducting Echo","penetration_power_mj":0.0,"radio_hook_text":"","revealed_site_id":"","salvage_yield_item_id":"scrap_mechanical","salvage_yield_quantity":1,"severity":"Minor","signal_type":"radar_anomaly"}`
  - row 12: `{"affected_cell_spread":1,"description":"A shredded high-altitude foil meteorological balloon enters radar coverage, triggering an anomalous threat warning that dissolves harmlessly in upper winds.","id":"event_orbital_debris_misclassification","impact_energy_mj":0.0,"is_false_positive":true,"lead_time_days":2,"name":"Decaying Weather Balloon Misclassification","penetration_power_mj":0.0,"radio_hook_text":"","revealed_site_id":"","salvage_yield_item_id":"scrap_metal","salvage_yield_quantity":1,"severity":"Minor","signal_type":"radar_anomaly"}`

## `Assets/StreamingAssets/Data/sky_layer_armor_catalog.json`
- Bytes: 5,475; SHA-256: `b4085538fc1f4fc58718464d5d5f981becccc16acb71bb74da0c5e80e020d33a`
- Root keys: `configurations, schema_version`
- `configurations`: list[6]; union fields: `attenuation_factor, blast_resistance_mj, composition, default_thickness_meters, degradation_rate, description, id, material_tier, name, repair_cost, tier`
  - row 1: `{"attenuation_factor":0.6,"blast_resistance_mj":5.0,"composition":[{"item_id":"scrap_wood","quantity":4},{"item_id":"cloth","quantity":6}],"default_thickness_meters":0.8,"degradation_rate":0.35,"description":"Stacked burlap sacks filled with dry soil and gravel, braced with timber struts. Protects against light shrapnel but degrades rapidly under direct kinetic hits.","id":"sky_armor_sandbag_layer","material_tier":"Dirt","name":"Improvised Sandbag Layer","repair_cost":[{"item_id":"scrap_wood","quantity":1},{"item_id":"cloth","quantity":2}],"tier":"improvised"}`
  - row 2: `{"attenuation_factor":0.45,"blast_resistance_mj":12.0,"composition":[{"item_id":"scrap_metal","quantity":8},{"item_id":"mechanical_parts","quantity":4}],"default_thickness_meters":1.0,"degradation_rate":0.25,"description":"Overlapping salvage steel plates bolted across roof framing. Provides resilient deflection against low-velocity orbital debris.","id":"sky_armor_scrap_overlay","material_tier":"Wood","name":"Scrap-Plate Overlay","repair_cost":[{"item_id":"scrap_metal","quantity":3}],"tier":"improvised"}`
  - row 3: `{"attenuation_factor":0.2,"blast_resistance_mj":37.5,"composition":[{"item_id":"scrap_metal","quantity":10},{"item_id":"chemicals","quantity":4},{"item_id":"scrap_wood","quantity":4}],"default_thickness_meters":1.5,"degradation_rate":0.15,"description":"Poured aggregate cement laced with scrap steel rebar. High mass and blast dampening offer durable long-term orbital mitigation.","id":"sky_armor_reinforced_concrete","material_tier":"ReinforcedConcrete","name":"Reinforced Concrete Slab","repair_cost":[{"item_id":"scrap_metal","quantity":3},{"item_id":"chemicals","quantity":1}],"tier":"reinforced"}`
  - row 4: `{"attenuation_factor":0.05,"blast_resistance_mj":45.0,"composition":[{"item_id":"scrap_metal","quantity":14},{"item_id":"mechanical_parts","quantity":6},{"item_id":"fuel","quantity":2}],"default_thickness_meters":1.2,"degradation_rate":0.12,"description":"Heavy naval and industrial pressure-hull plate welded directly over bunker arches. Excellent resistance against hypervelocity kinetic penetrators.","id":"sky_armor_steel_hull_plating","material_tier":"LeadSheeting","name":"Steel Hull Plating","repair_cost":[{"item_id":"scrap_metal","quantity":4},{"item_id":"mechanical_parts","quantity":2}],"tier":"reinforced"}`
  - row 5: `{"attenuation_factor":0.01,"blast_resistance_mj":160.0,"composition":[{"item_id":"scrap_metal","quantity":20},{"item_id":"electronic_scrap","quantity":8},{"item_id":"mechanical_parts","quantity":10}],"default_thickness_meters":2.0,"degradation_rate":0.08,"description":"Layered tungsten-carbide tiles and shock-absorbing ceramic matrix. Top-tier kinetic and radiation shielding designed for military continuity vaults.","id":"sky_armor_composite_military","material_tier":"TungstenComposite","name":"Composite Military-Grade Armor","repair_cost":[{"item_id":"scrap_metal","quantity":5},{"item_id":"electronic_scrap","quantity":2},{"item_id":"mechani…`
  - row 6: `{"attenuation_factor":0.5,"blast_resistance_mj":8.0,"composition":[{"item_id":"scrap_wood","quantity":6},{"item_id":"cloth","quantity":4},{"item_id":"scrap_metal","quantity":2}],"default_thickness_meters":0.6,"degradation_rate":0.4,"description":"A sacrificial pitched timber and sheet canopy designed for rapid deployment ahead of predicted orbital debris showers.","id":"sky_armor_emergency_blast_canopy","material_tier":"Wood","name":"Emergency Blast Canopy","repair_cost":[{"item_id":"scrap_wood","quantity":2},{"item_id":"cloth","quantity":2}],"tier":"improvised"}`
- Bytes: 5,475; SHA-256: `b4085538fc1f4fc58718464d5d5f981becccc16acb71bb74da0c5e80e020d33a`
- Root keys: `configurations, schema_version`
- `configurations`: list[6]; union fields: `attenuation_factor, blast_resistance_mj, composition, default_thickness_meters, degradation_rate, description, id, material_tier, name, repair_cost, tier`
  - row 1: `{"attenuation_factor":0.6,"blast_resistance_mj":5.0,"composition":[{"item_id":"scrap_wood","quantity":4},{"item_id":"cloth","quantity":6}],"default_thickness_meters":0.8,"degradation_rate":0.35,"description":"Stacked burlap sacks filled with dry soil and gravel, braced with timber struts. Protects against light shrapnel but degrades rapidly under direct kinetic hits.","id":"sky_armor_sandbag_layer","material_tier":"Dirt","name":"Improvised Sandbag Layer","repair_cost":[{"item_id":"scrap_wood","quantity":1},{"item_id":"cloth","quantity":2}],"tier":"improvised"}`
  - row 2: `{"attenuation_factor":0.45,"blast_resistance_mj":12.0,"composition":[{"item_id":"scrap_metal","quantity":8},{"item_id":"mechanical_parts","quantity":4}],"default_thickness_meters":1.0,"degradation_rate":0.25,"description":"Overlapping salvage steel plates bolted across roof framing. Provides resilient deflection against low-velocity orbital debris.","id":"sky_armor_scrap_overlay","material_tier":"Wood","name":"Scrap-Plate Overlay","repair_cost":[{"item_id":"scrap_metal","quantity":3}],"tier":"improvised"}`
  - row 3: `{"attenuation_factor":0.2,"blast_resistance_mj":37.5,"composition":[{"item_id":"scrap_metal","quantity":10},{"item_id":"chemicals","quantity":4},{"item_id":"scrap_wood","quantity":4}],"default_thickness_meters":1.5,"degradation_rate":0.15,"description":"Poured aggregate cement laced with scrap steel rebar. High mass and blast dampening offer durable long-term orbital mitigation.","id":"sky_armor_reinforced_concrete","material_tier":"ReinforcedConcrete","name":"Reinforced Concrete Slab","repair_cost":[{"item_id":"scrap_metal","quantity":3},{"item_id":"chemicals","quantity":1}],"tier":"reinforced"}`
  - row 4: `{"attenuation_factor":0.05,"blast_resistance_mj":45.0,"composition":[{"item_id":"scrap_metal","quantity":14},{"item_id":"mechanical_parts","quantity":6},{"item_id":"fuel","quantity":2}],"default_thickness_meters":1.2,"degradation_rate":0.12,"description":"Heavy naval and industrial pressure-hull plate welded directly over bunker arches. Excellent resistance against hypervelocity kinetic penetrators.","id":"sky_armor_steel_hull_plating","material_tier":"LeadSheeting","name":"Steel Hull Plating","repair_cost":[{"item_id":"scrap_metal","quantity":4},{"item_id":"mechanical_parts","quantity":2}],"tier":"reinforced"}`
  - row 5: `{"attenuation_factor":0.01,"blast_resistance_mj":160.0,"composition":[{"item_id":"scrap_metal","quantity":20},{"item_id":"electronic_scrap","quantity":8},{"item_id":"mechanical_parts","quantity":10}],"default_thickness_meters":2.0,"degradation_rate":0.08,"description":"Layered tungsten-carbide tiles and shock-absorbing ceramic matrix. Top-tier kinetic and radiation shielding designed for military continuity vaults.","id":"sky_armor_composite_military","material_tier":"TungstenComposite","name":"Composite Military-Grade Armor","repair_cost":[{"item_id":"scrap_metal","quantity":5},{"item_id":"electronic_scrap","quantity":2},{"item_id":"mechani…`
  - row 6: `{"attenuation_factor":0.5,"blast_resistance_mj":8.0,"composition":[{"item_id":"scrap_wood","quantity":6},{"item_id":"cloth","quantity":4},{"item_id":"scrap_metal","quantity":2}],"default_thickness_meters":0.6,"degradation_rate":0.4,"description":"A sacrificial pitched timber and sheet canopy designed for rapid deployment ahead of predicted orbital debris showers.","id":"sky_armor_emergency_blast_canopy","material_tier":"Wood","name":"Emergency Blast Canopy","repair_cost":[{"item_id":"scrap_wood","quantity":2},{"item_id":"cloth","quantity":2}],"tier":"improvised"}`

## `Assets/StreamingAssets/Data/radio_intercepts.json`
- Bytes: 13,716; SHA-256: `9123e10212b21db35ebf451984908ae6ed5780cc766cae56caff2083905749f9`
- Root keys: `intercepts, schema_version`
- `intercepts`: list[16]; union fields: `band, base_signal_strength, callsign, encryption, expiry_days, frequency_khz, id, message, signal_class, source_faction_id, tags, triangulation`
  - row 1: `{"band":"hf","base_signal_strength":0.65,"callsign":"MERIDIAN-ACT-7","encryption":{"difficulty":40,"required_skill_ids":["skill_signal_ear","skill_cold_analysis"],"scheme":"field_cipher"},"expiry_days":4,"frequency_khz":7115,"id":"radio_intercept_meridian_supply_column_01","message":"Bravo route is choked with white-hot ash. We just lost two rigs to melt-throughs. Rerouting the surviving tankers to the deep bulk storage. If we don't make it, burn the manifest.","signal_class":"logistics_chatter","source_faction_id":"faction_the_compact","tags":["military","logistics","fuel"],"triangulation":{"required_bearings":3,"revealed_location_id":"loc_…`
  - row 2: `{"band":"hf","base_signal_strength":0.5,"callsign":"SHELTER-44-SOS","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":3,"frequency_khz":3850,"id":"radio_intercept_sos_quarry_shelter_02","message":"MAYDAY. The intake fan just screamed and died—we're buried under thirty feet of frozen sludge. Seven of us coughing in the sub-level. Oxygen's thinning. (coughing) We can't clear the hatch...","signal_class":"sos_distress","source_faction_id":"faction_the_office","tags":["distress","emergency","rescue"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_recovery_yard"}}`
  - row 3: `{"band":"hf","base_signal_strength":0.8,"callsign":"DEAD-HAND-CARRIER-9","encryption":{"difficulty":80,"required_skill_ids":["skill_cold_analysis"],"scheme":"otp_military"},"expiry_days":0,"frequency_khz":14220,"id":"radio_intercept_dead_hand_silo_beacon_03","message":"AUTOMATED TELEMETRY: Silo doors forced. Micro-fractures in primary concrete casing. Sub-level three ordnance bays unsealed. Radiation spikes detected in retrieval corridors.","signal_class":"dead_hand_beacon","source_faction_id":"faction_the_compact","tags":["military","automated","high_value"],"triangulation":{"required_bearings":3,"revealed_location_id":"loc_ordnance_shoulde…`
  - row 4: `{"band":"hf","base_signal_strength":0.7,"callsign":"OBS-IONO-NORTH","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":2,"frequency_khz":5150,"id":"radio_intercept_weather_ionosphere_bulletin_04","message":"ATMOSPHERIC ALERT: Sky is boiling green. Severe geomagnetic cascade expected in two hours. Expect massive static across all bands. The northern passes are entirely blacked out.","signal_class":"weather_telemetry","source_faction_id":"faction_the_office","tags":["weather","science","propagation"],"triangulation":{"required_bearings":1,"revealed_location_id":"loc_low_background_lab"}}`
  - row 5: `{"band":"hf","base_signal_strength":0.9,"callsign":"ORBIT-TELEM-HARROW","encryption":{"difficulty":55,"required_skill_ids":["skill_signal_ear","skill_cold_analysis"],"scheme":"military_telemetry"},"expiry_days":1,"frequency_khz":10125,"id":"radio_intercept_orbital_harrow_early_warning_05","message":"DEORBIT WARNING: Six kinetic rods separated from orbit-tether. Thermal bloom detected. Thirty seconds to upper-crust impact. All remaining personnel, brace for seismic shear.","signal_class":"harrow_warning","source_faction_id":"faction_the_compact","tags":["orbital","harrow","strike_warning"],"triangulation":{"required_bearings":2,"revealed_loca…`
  - row 6: `{"band":"vhf","base_signal_strength":0.45,"callsign":"IRON-FANG-04","encryption":{"difficulty":25,"required_skill_ids":["skill_signal_ear"],"scheme":"slang_code"},"expiry_days":2,"frequency_khz":27150,"id":"radio_intercept_raider_ambush_chatter_06","message":"(Static) ...caltrops are laid in the blind spot. Let the lead hauler hit the bottleneck, wait till they pop the hatches to check the tires, then we take them. Leave the grain, grab the water.","signal_class":"tactical_chatter","source_faction_id":"faction_iron_raiders","tags":["raider","ambush","intel"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_weighbridge"}}`
  - row 7: `{"band":"vhf","base_signal_strength":0.6,"callsign":"FLOTILLA-TIDE-2","encryption":{"difficulty":35,"required_skill_ids":["skill_signal_ear"],"scheme":"nautical_code"},"expiry_days":5,"frequency_khz":156800,"id":"radio_intercept_flotilla_coastal_relay_07","message":"Barge Three is tied off in the shadows. The water's glowing again tonight, but the salt-fish and stripped copper are dry. Signal with two red flares if the exchange is still on.","signal_class":"maritime_chatter","source_faction_id":"faction_black_flotilla","tags":["maritime","trade","supplies"],"triangulation":{"required_bearings":3,"revealed_location_id":"loc_cold_store_atlanti…`
  - row 8: `{"band":"hf","base_signal_strength":0.4,"callsign":"SHELTER-UNKNOWN-SOS","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":3,"frequency_khz":3620,"id":"radio_intercept_spoofed_distress_trap_08","message":"(Sobbing) Please, is anyone on this frequency? We're at the old motel... we have kids here, we have generator parts to trade, we just need iodine... please, they're starting to bleed...","signal_class":"spoofed_signal","source_faction_id":"faction_the_cutters","tags":["trap","danger","ambush"],"triangulation":{"required_bearings":3,"revealed_location_id":"loc_motel_verity"}}`
  - row 9: `{"band":"hf","base_signal_strength":0.55,"callsign":"EXPEDITION-GLACIER-9","encryption":{"difficulty":45,"required_skill_ids":["skill_cold_analysis"],"scheme":"scientific_encoding"},"expiry_days":6,"frequency_khz":14180,"id":"radio_intercept_scientific_ice_core_relay_09","message":"Core extraction successful. The ice strata shows the soot layer from the third exchange. It's... it's thicker than we modeled. The data vault is intact, but the surface... there's nothing left up there.","signal_class":"science_log","source_faction_id":"faction_the_office","tags":["lore","science","research"],"triangulation":{"required_bearings":3,"revealed_locati…`
  - row 10: `{"band":"vhf","base_signal_strength":0.5,"callsign":"CUTTER-PRIME","encryption":{"difficulty":50,"required_skill_ids":["skill_signal_ear","skill_cold_analysis"],"scheme":"field_cipher"},"expiry_days":3,"frequency_khz":146520,"id":"radio_intercept_cutters_bunker_assault_10","message":"Thermite is burning through the hinges now. The bunker rats are screaming through the comms. We breach at dusk. Collar the engineers, put a bullet in the rest.","signal_class":"raider_chatter","source_faction_id":"faction_the_cutters","tags":["raider","assault","intel"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_forward_roster_camp"}}`
  - row 11: `{"band":"hf","base_signal_strength":0.6,"callsign":"AQUIFER-STATION-9","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":2,"frequency_khz":1850,"id":"radio_intercept_pumphouse_distress_11","message":"The water's black and it smells like sulfur. It just swallowed the secondary pumps. We're running on fumes. Sealing the bulkhead and praying the blast door holds the pressure...","signal_class":"sos_distress","source_faction_id":"faction_the_office","tags":["distress","flood","infrastructure"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_pump_station_nine"}}`
  - row 12: `{"band":"hf","base_signal_strength":0.7,"callsign":"VALLEY-AGRIC-4","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":4,"frequency_khz":7080,"id":"radio_intercept_grain_silo_cache_12","message":"We cracked the secondary elevator. Half a ton of viable winter rye, sealed in vacuum glass. We're choking on the dust though. Will trade fifty kilos for pristine carbon filters. No raiders.","signal_class":"civilian_broadcast","source_faction_id":"faction_the_scale","tags":["trade","food","cache"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_grain_silo"}}`
  - row 13: `{"band":"uhf","base_signal_strength":0.85,"callsign":"PEAK-BEACON-ONE","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":0,"frequency_khz":446000,"id":"radio_intercept_high_mountain_relay_13","message":"AUTOMATED PING: Solar degraded by ash accumulation. Gyros stabilizing. If you receive this signal, the Compact endures. Maintain your stations.","signal_class":"navigation_beacon","source_faction_id":"faction_the_office","tags":["navigation","repeater","permanent"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_radio_relay_mast"}}`
  - row 14: `{"band":"hf","base_signal_strength":0.55,"callsign":"CANAL-WATCH-7","encryption":{"difficulty":30,"required_skill_ids":["skill_signal_ear"],"scheme":"field_cipher"},"expiry_days":4,"frequency_khz":7240,"id":"radio_intercept_lock_gate_sabotage_14","message":"The whole mechanism is screaming. Somebody packed the gears with rebar and concrete. The water is backing up fast, and the pressure seals are starting to whine. We need a welding team NOW!","signal_class":"regional_chatter","source_faction_id":"faction_the_scale","tags":["water","sabotage","warning"],"triangulation":{"required_bearings":3,"revealed_location_id":"loc_lock_gate_four"}}`
  - row 15: `{"band":"hf","base_signal_strength":0.4,"callsign":"SNOWLINE-CAMP-2","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":3,"frequency_khz":14290,"id":"radio_intercept_snowline_researcher_sos_15","message":"The line snapped. We're in the dark. The heaters died ten minutes ago and the frost is already creeping up the walls. (shivering) We can't feel our hands. Please...","signal_class":"sos_distress","source_faction_id":"faction_the_office","tags":["distress","cold","rescue"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_snowline_station"}}`
  - row 16: `{"band":"hf","base_signal_strength":0.75,"callsign":"COMMUNITY-GRANGE","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":7,"frequency_khz":1920,"id":"radio_intercept_grange_community_hymn_16","message":"(Static crackles, giving way to a choir of raspy, exhausted voices singing an old hymn)... reading the roll. Station Four, silent. Station Seven, silent. Station Nine... (a long pause) Station Nine, we pray for your souls.","signal_class":"community_broadcast","source_faction_id":"faction_the_scale","tags":["lore","community","morale"],"triangulation":{"required_bearings":1,"revealed_location_id":"loc_grange_…`
- Bytes: 13,716; SHA-256: `9123e10212b21db35ebf451984908ae6ed5780cc766cae56caff2083905749f9`
- Root keys: `intercepts, schema_version`
- `intercepts`: list[16]; union fields: `band, base_signal_strength, callsign, encryption, expiry_days, frequency_khz, id, message, signal_class, source_faction_id, tags, triangulation`
  - row 1: `{"band":"hf","base_signal_strength":0.65,"callsign":"MERIDIAN-ACT-7","encryption":{"difficulty":40,"required_skill_ids":["skill_signal_ear","skill_cold_analysis"],"scheme":"field_cipher"},"expiry_days":4,"frequency_khz":7115,"id":"radio_intercept_meridian_supply_column_01","message":"Bravo route is choked with white-hot ash. We just lost two rigs to melt-throughs. Rerouting the surviving tankers to the deep bulk storage. If we don't make it, burn the manifest.","signal_class":"logistics_chatter","source_faction_id":"faction_the_compact","tags":["military","logistics","fuel"],"triangulation":{"required_bearings":3,"revealed_location_id":"loc_…`
  - row 2: `{"band":"hf","base_signal_strength":0.5,"callsign":"SHELTER-44-SOS","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":3,"frequency_khz":3850,"id":"radio_intercept_sos_quarry_shelter_02","message":"MAYDAY. The intake fan just screamed and died—we're buried under thirty feet of frozen sludge. Seven of us coughing in the sub-level. Oxygen's thinning. (coughing) We can't clear the hatch...","signal_class":"sos_distress","source_faction_id":"faction_the_office","tags":["distress","emergency","rescue"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_recovery_yard"}}`
  - row 3: `{"band":"hf","base_signal_strength":0.8,"callsign":"DEAD-HAND-CARRIER-9","encryption":{"difficulty":80,"required_skill_ids":["skill_cold_analysis"],"scheme":"otp_military"},"expiry_days":0,"frequency_khz":14220,"id":"radio_intercept_dead_hand_silo_beacon_03","message":"AUTOMATED TELEMETRY: Silo doors forced. Micro-fractures in primary concrete casing. Sub-level three ordnance bays unsealed. Radiation spikes detected in retrieval corridors.","signal_class":"dead_hand_beacon","source_faction_id":"faction_the_compact","tags":["military","automated","high_value"],"triangulation":{"required_bearings":3,"revealed_location_id":"loc_ordnance_shoulde…`
  - row 4: `{"band":"hf","base_signal_strength":0.7,"callsign":"OBS-IONO-NORTH","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":2,"frequency_khz":5150,"id":"radio_intercept_weather_ionosphere_bulletin_04","message":"ATMOSPHERIC ALERT: Sky is boiling green. Severe geomagnetic cascade expected in two hours. Expect massive static across all bands. The northern passes are entirely blacked out.","signal_class":"weather_telemetry","source_faction_id":"faction_the_office","tags":["weather","science","propagation"],"triangulation":{"required_bearings":1,"revealed_location_id":"loc_low_background_lab"}}`
  - row 5: `{"band":"hf","base_signal_strength":0.9,"callsign":"ORBIT-TELEM-HARROW","encryption":{"difficulty":55,"required_skill_ids":["skill_signal_ear","skill_cold_analysis"],"scheme":"military_telemetry"},"expiry_days":1,"frequency_khz":10125,"id":"radio_intercept_orbital_harrow_early_warning_05","message":"DEORBIT WARNING: Six kinetic rods separated from orbit-tether. Thermal bloom detected. Thirty seconds to upper-crust impact. All remaining personnel, brace for seismic shear.","signal_class":"harrow_warning","source_faction_id":"faction_the_compact","tags":["orbital","harrow","strike_warning"],"triangulation":{"required_bearings":2,"revealed_loca…`
  - row 6: `{"band":"vhf","base_signal_strength":0.45,"callsign":"IRON-FANG-04","encryption":{"difficulty":25,"required_skill_ids":["skill_signal_ear"],"scheme":"slang_code"},"expiry_days":2,"frequency_khz":27150,"id":"radio_intercept_raider_ambush_chatter_06","message":"(Static) ...caltrops are laid in the blind spot. Let the lead hauler hit the bottleneck, wait till they pop the hatches to check the tires, then we take them. Leave the grain, grab the water.","signal_class":"tactical_chatter","source_faction_id":"faction_iron_raiders","tags":["raider","ambush","intel"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_weighbridge"}}`
  - row 7: `{"band":"vhf","base_signal_strength":0.6,"callsign":"FLOTILLA-TIDE-2","encryption":{"difficulty":35,"required_skill_ids":["skill_signal_ear"],"scheme":"nautical_code"},"expiry_days":5,"frequency_khz":156800,"id":"radio_intercept_flotilla_coastal_relay_07","message":"Barge Three is tied off in the shadows. The water's glowing again tonight, but the salt-fish and stripped copper are dry. Signal with two red flares if the exchange is still on.","signal_class":"maritime_chatter","source_faction_id":"faction_black_flotilla","tags":["maritime","trade","supplies"],"triangulation":{"required_bearings":3,"revealed_location_id":"loc_cold_store_atlanti…`
  - row 8: `{"band":"hf","base_signal_strength":0.4,"callsign":"SHELTER-UNKNOWN-SOS","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":3,"frequency_khz":3620,"id":"radio_intercept_spoofed_distress_trap_08","message":"(Sobbing) Please, is anyone on this frequency? We're at the old motel... we have kids here, we have generator parts to trade, we just need iodine... please, they're starting to bleed...","signal_class":"spoofed_signal","source_faction_id":"faction_the_cutters","tags":["trap","danger","ambush"],"triangulation":{"required_bearings":3,"revealed_location_id":"loc_motel_verity"}}`
  - row 9: `{"band":"hf","base_signal_strength":0.55,"callsign":"EXPEDITION-GLACIER-9","encryption":{"difficulty":45,"required_skill_ids":["skill_cold_analysis"],"scheme":"scientific_encoding"},"expiry_days":6,"frequency_khz":14180,"id":"radio_intercept_scientific_ice_core_relay_09","message":"Core extraction successful. The ice strata shows the soot layer from the third exchange. It's... it's thicker than we modeled. The data vault is intact, but the surface... there's nothing left up there.","signal_class":"science_log","source_faction_id":"faction_the_office","tags":["lore","science","research"],"triangulation":{"required_bearings":3,"revealed_locati…`
  - row 10: `{"band":"vhf","base_signal_strength":0.5,"callsign":"CUTTER-PRIME","encryption":{"difficulty":50,"required_skill_ids":["skill_signal_ear","skill_cold_analysis"],"scheme":"field_cipher"},"expiry_days":3,"frequency_khz":146520,"id":"radio_intercept_cutters_bunker_assault_10","message":"Thermite is burning through the hinges now. The bunker rats are screaming through the comms. We breach at dusk. Collar the engineers, put a bullet in the rest.","signal_class":"raider_chatter","source_faction_id":"faction_the_cutters","tags":["raider","assault","intel"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_forward_roster_camp"}}`
  - row 11: `{"band":"hf","base_signal_strength":0.6,"callsign":"AQUIFER-STATION-9","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":2,"frequency_khz":1850,"id":"radio_intercept_pumphouse_distress_11","message":"The water's black and it smells like sulfur. It just swallowed the secondary pumps. We're running on fumes. Sealing the bulkhead and praying the blast door holds the pressure...","signal_class":"sos_distress","source_faction_id":"faction_the_office","tags":["distress","flood","infrastructure"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_pump_station_nine"}}`
  - row 12: `{"band":"hf","base_signal_strength":0.7,"callsign":"VALLEY-AGRIC-4","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":4,"frequency_khz":7080,"id":"radio_intercept_grain_silo_cache_12","message":"We cracked the secondary elevator. Half a ton of viable winter rye, sealed in vacuum glass. We're choking on the dust though. Will trade fifty kilos for pristine carbon filters. No raiders.","signal_class":"civilian_broadcast","source_faction_id":"faction_the_scale","tags":["trade","food","cache"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_grain_silo"}}`
  - row 13: `{"band":"uhf","base_signal_strength":0.85,"callsign":"PEAK-BEACON-ONE","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":0,"frequency_khz":446000,"id":"radio_intercept_high_mountain_relay_13","message":"AUTOMATED PING: Solar degraded by ash accumulation. Gyros stabilizing. If you receive this signal, the Compact endures. Maintain your stations.","signal_class":"navigation_beacon","source_faction_id":"faction_the_office","tags":["navigation","repeater","permanent"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_radio_relay_mast"}}`
  - row 14: `{"band":"hf","base_signal_strength":0.55,"callsign":"CANAL-WATCH-7","encryption":{"difficulty":30,"required_skill_ids":["skill_signal_ear"],"scheme":"field_cipher"},"expiry_days":4,"frequency_khz":7240,"id":"radio_intercept_lock_gate_sabotage_14","message":"The whole mechanism is screaming. Somebody packed the gears with rebar and concrete. The water is backing up fast, and the pressure seals are starting to whine. We need a welding team NOW!","signal_class":"regional_chatter","source_faction_id":"faction_the_scale","tags":["water","sabotage","warning"],"triangulation":{"required_bearings":3,"revealed_location_id":"loc_lock_gate_four"}}`
  - row 15: `{"band":"hf","base_signal_strength":0.4,"callsign":"SNOWLINE-CAMP-2","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":3,"frequency_khz":14290,"id":"radio_intercept_snowline_researcher_sos_15","message":"The line snapped. We're in the dark. The heaters died ten minutes ago and the frost is already creeping up the walls. (shivering) We can't feel our hands. Please...","signal_class":"sos_distress","source_faction_id":"faction_the_office","tags":["distress","cold","rescue"],"triangulation":{"required_bearings":2,"revealed_location_id":"loc_snowline_station"}}`
  - row 16: `{"band":"hf","base_signal_strength":0.75,"callsign":"COMMUNITY-GRANGE","encryption":{"difficulty":0,"required_skill_ids":[],"scheme":"none"},"expiry_days":7,"frequency_khz":1920,"id":"radio_intercept_grange_community_hymn_16","message":"(Static crackles, giving way to a choir of raspy, exhausted voices singing an old hymn)... reading the roll. Station Four, silent. Station Seven, silent. Station Nine... (a long pause) Station Nine, we pray for your souls.","signal_class":"community_broadcast","source_faction_id":"faction_the_scale","tags":["lore","community","morale"],"triangulation":{"required_bearings":1,"revealed_location_id":"loc_grange_…`

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

### `OrbitalHarrowCatalogLoader` (10 sampled current references)
- Assets/Ashfall.Core/Shelter/OrbitalHarrowCatalog.cs:35: public static class OrbitalHarrowCatalogLoader
- src/Host/HostCli.DynamicWorld.cs:94: var orbitalEvents = OrbitalHarrowCatalogLoader.Load(dataDirectory, files, json);
- Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs:93: var events = OrbitalHarrowCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs:119: var events = OrbitalHarrowCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs:145: var events = OrbitalHarrowCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs:126: var events = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, json);
- Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs:256: var threatEvents = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, json);
- Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs:122: var list = OrbitalHarrowCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs:117: var harrowEvents = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, serializer);
- Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs:393: var harrow = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, serializer);
### `OrbitalHarrowTelemetrySystem` (18 sampled current references)
- Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs:24: public string systemId = OrbitalHarrowTelemetrySystem.SystemId;
- Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs:69: public sealed class OrbitalHarrowTelemetrySystem
- Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs:89: public OrbitalHarrowTelemetrySystem(SkyLayerArmorSystem armor, ISeededRng rng, ILog? log = null)
- Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs:127: private readonly OrbitalHarrowTelemetrySystem? _harrowTelemetry;
- Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs:146: OrbitalHarrowTelemetrySystem? harrowTelemetry = null,
- Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs:106: public OrbitalHarrowTelemetrySystem Orbital { get; }
- Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs:135: Orbital = new OrbitalHarrowTelemetrySystem(_armor, new SeededRng(unchecked(seed ^ 0x5A5A5A5A)), _log);
- Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs:67: /// authoritative <see cref="OrbitalHarrowTelemetrySystem"/> warnings,
- Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs:70: /// <see cref="OrbitalHarrowTelemetrySystem.ApplyInterceptionMitigation"/> —
- Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs:97: private readonly OrbitalHarrowTelemetrySystem? _telemetry;
- Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs:109: OrbitalHarrowTelemetrySystem? telemetry = null,
- src/Main.FlagshipInstitutions.cs:85: private OrbitalHarrowTelemetrySystem EnsureOrbitalHarrowTelemetry()
- src/Main.FlagshipInstitutions.cs:89: telemetry = new OrbitalHarrowTelemetrySystem(new SkyLayerArmorSystem(), new SeededRng(FlagshipMasterSeed));
- src/Host/HostCli.DynamicWorld.cs:107: var orbital = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(54321));
- src/Host/HostCli.SkyDefense.cs:39: var telemetry = new OrbitalHarrowTelemetrySystem(new SkyLayerArmorSystem(), new SeededRng(42));
- src/UI/RadioIntelligencePanel.cs:23: private OrbitalHarrowTelemetrySystem? _harrow;
- src/UI/RadioIntelligencePanel.cs:42: public void Bind(ShelterRadioStationSystem radio, OrbitalHarrowTelemetrySystem? harrow = null, int currentDay = 0)
- Ashfall.Core.Tests/IslandBridgesTests.cs:151: var orbital = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(42));
### `ScheduleEventDef` (6 sampled current references)
- Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs:108: public void ScheduleEventDef(OrbitalEventDef def, int day, int gridX)
- src/Host/HostCli.DynamicWorld.cs:113: orbital.ScheduleEventDef(heavyStrike, day: 10, gridX: 10);
- Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs:124: oh.ScheduleEventDef(falseAlarm, day: 4, gridX: 5);
- Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs:153: oh.ScheduleEventDef(deadHand, day: 5, gridX: 5);
- Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs:267: telemetry.ScheduleEventDef(standardStrike, day: 3, gridX: 4);
- Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs:153: orbital.ScheduleEventDef(heavyEvent, day: 5, gridX: 5);
### `SkyLayerArmorSystem` (18 sampled current references)
- Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs:74: private readonly SkyLayerArmorSystem _armor;
- Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs:89: public OrbitalHarrowTelemetrySystem(SkyLayerArmorSystem armor, ISeededRng rng, ILog? log = null)
- Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs:28: public sealed class SkyLayerArmorSystem
- Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs:96: /// orbital impact clock against <see cref="SkyLayerArmorSystem"/>, and
- Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs:111: private readonly SkyLayerArmorSystem _armor;
- Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs:124: SkyLayerArmorSystem armor,
- Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs:94: private readonly SkyLayerArmorSystem? _skyArmor;
- Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs:119: SkyLayerArmorSystem? skyArmor = null,
- Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs:72: /// SkyLayerArmorSystem pipeline, never around it.
- src/Main.FlagshipInstitutions.cs:89: telemetry = new OrbitalHarrowTelemetrySystem(new SkyLayerArmorSystem(), new SeededRng(FlagshipMasterSeed));
- src/Host/HostCli.DynamicWorld.cs:103: var armor = new SkyLayerArmorSystem();
- src/Host/HostCli.SkyDefense.cs:39: var telemetry = new OrbitalHarrowTelemetrySystem(new SkyLayerArmorSystem(), new SeededRng(42));
- src/Host/WorldHostSession.cs:22: public SkyLayerArmorSystem SkyArmor { get; }
- src/Host/WorldHostSession.cs:84: SkyLayerArmorSystem skyArmor = null!,
- src/Host/WorldHostSession.cs:96: SkyArmor = skyArmor ?? new SkyLayerArmorSystem();
- src/Host/HostCli.PanelTests.cs:2290: // ── 1. SkyLayerArmorSystem ──────────────────────────────
- src/Host/HostCli.PanelTests.cs:2291: var sky = new SkyLayerArmorSystem();
- src/Host/HostCli.PanelTests.cs:2311: var sky2 = new SkyLayerArmorSystem();
### `CaptureWeatherIntelligenceSave` (2 sampled current references)
- src/Main.World.cs:225: _world!.CaptureWeatherIntelligenceSave()!,
- src/Host/WorldHostSession.cs:334: public WeatherIntelligenceSaveState CaptureWeatherIntelligenceSave()

# Appendix E — Current focused-test inventory

Current test declaration inventory: 61 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs` — 18 test declarations; bytes=5,825; SHA-256=`0a1ab6c8356a96a99ebb8df15b0efef3c009b8967d902f19aaf93481f0390ca3`
- 00029: [Fact]
- 00030: public void ActivateTelemetry_EnablesSystem()
- 00037: [Fact]
- 00038: public void ScheduleImpact_CreatesWarning()
- 00046: [Fact]
- 00047: public void Brace_MitigatesImpact()
- 00056: [Fact]
- 00057: public void TickDay_OnImpactDay_Resolves()
- 00068: [Fact]
- 00069: public void Brace_WhenNoImpact_Blocks()
- 00076: [Fact]
- 00077: public void CaptureRestoreState_PreservesImpact()
- 00089: [Fact]
- 00090: public void TelemetryCatalog_ContainsTwelveCanonicalEvents()
- 00115: [Fact]
- 00116: public void FalsePositiveEvents_ResolveWithoutDamageOrBreach()
- 00141: [Fact]
- 00142: public void DeadHandEvents_CarryRadioHooksAndRevealSites()
### `Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs` — 18 test declarations; bytes=12,575; SHA-256=`2522af036681c87b5311c24414087f23485aec6d26ce6c9bfc749a321a70aff4`
- 00030: [Fact]
- 00031: public void ArmorCatalog_LoadsAllSixAuthoredConfigurations()
- 00064: [Fact]
- 00065: public void ArmorCatalog_AllMaterialItemReferencesResolveInItemsCatalog()
- 00105: [Fact]
- 00106: public void ArmorCatalog_DefaultConfigurationsFallbackMatchesSixConfigs()
- 00120: [Fact]
- 00121: public void OrbitalThreatCatalog_LoadsTwelveUniqueEvents()
- 00148: [Fact]
- 00149: public void SkyLayerArmor_InstallationAndAttenuationHierarchy()
- 00176: [Fact]
- 00177: public void SkyLayerArmor_EvaluateImpact_MitigationAndBreach()
- 00203: [Fact]
- 00204: public void SkyLayerArmor_RepairCell_RestoresDurability()
- 00219: [Fact]
- 00220: public void SkyLayerArmor_SaveAndRestore_PreservesAllCells()
- 00250: [Fact]
- 00251: public void FullDefenseLoop_TelemetryWarning_Brace_Strike_Mitigation_Salvage()
### `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs` — 7 test declarations; bytes=21,080; SHA-256=`58130dae9d890fd61d10c85f141e3fa59852d2b910e0cba09663d9b5a5ea7155`
- 00048: public void SetSkillBonus(string disciplineId, float bonus) { }
- 00051: [Fact]
- 00052: public void Test1_ReferentialIntegrity_CrossCatalogContracts_AllResolve()
- 00335: [Fact]
- 00336: public void Test2_FullDeterministicCampaignJourney_TouchesAllTenSystems_ReplaysIdentically()
- 00352: [Fact]
- 00353: public void Test3_Batch1CatalogCountsAndLoaderClassifications_MatchAuthoritativeTruth()
### `Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs` — 18 test declarations; bytes=13,480; SHA-256=`bb49504442f792a1d3a4a55c962a168047d4ebaa12253c41c01911e15c70aab4`
- 00030: [Fact]
- 00031: public void WeatherForecast_PeekDoesNotMutateWeatherState()
- 00046: [Fact]
- 00047: public void WeatherForecast_SameSeedProducesDeterministicForecast()
- 00065: [Fact]
- 00066: public void WeatherStation_TierProgression_AffectsHorizonAndConfidence()
- 00101: [Fact]
- 00102: public void WeatherStation_PreparationPayoffs_ProvideActionableAdvice()
- 00119: [Fact]
- 00120: public void OrbitalCatalog_LoadsAllFiveTemplates()
- 00131: [Fact]
- 00132: public void OrbitalImpact_EvaluatesSkyArmor_GeneratesSalvageAndRevealsSite()
- 00178: [Fact]
- 00179: public void SeasonModel_DefinesTenPhasesAcrossYear()
- 00206: [Fact]
- 00207: public void SeasonalEvents_LoadCatalog_AndTriggerDeterministically()
- 00236: [Fact]
- 00237: public void WeatherIntelligenceCoordinator_SaveRestoreRoundTrip_PreservesAllStates()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Orbital Harrow Telemetry: Twelve-Event Catalog, Detection-to-Impact Authority, and Salvage Consequences` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan’s 8 consequence records are not present as a current separate table.
- The plan must not call a selftest/demo schedule a production campaign loop.

## H.2 Evidence-strength corrections
- Remove the fictional separate consequence table.
- Trace live scheduling instead of demo wiring.
- Keep uncertainty and salvage semantics truthful.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Orbital Harrow Telemetry: Twelve-Event Catalog, Detection-to-Impact Authority, and Salvage Consequences while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use OrbitalHarrowCatalogLoader for event definitions.
- Use OrbitalHarrowTelemetrySystem for warning/impact/salvage state.
- Use SkyLayerArmorSystem for armor resolution.
- Use existing world/institution/radio consumers for facts.
- Persist through the current weather-intelligence/telemetry owner path.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement orbital harrow telemetry: twelve-event catalog, detection-to-impact authority, and salvage consequences arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- No second orbital consequence catalog is proposed.
- Any future schedule must name a current day/host seam and deterministic stream.

## J.2 Questions deliberately left open
- What current campaign event should schedule authored orbital rows?
- Should radio hooks be advisory-only, and where should the player acknowledge an incoming impact?

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

The original file at `HEAD:piagentsplans/39-orbital-harrow-telemetry-events.md` contained 4,858 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 39 — Orbital Harrow Telemetry Events (system exists, no data)

## Goal (2 lines)
Create `orbital_harrow_events.json` for `OrbitalHarrowTelemetrySystem` — the system is fully
implemented and save-supported but has **no event data** (verified: file missing). Add 12
telemetry events and 8 strike-consequence records that give the player warning time and
meaningful responses to orbital kinetic strikes.

## Why (P2)
- Verified: `OrbitalHarrowTelemetrySystem.cs` exists in Core; no event catalog exists.
- The telemetry system is the early-warning layer: without event data, the player gets no
  warning before strikes hit (Plan 38 armor has nothing to defend against in practice).
- Creates a time-pressure survival loop: detect → interpret → decide (reinforce / evacuate /
  accept damage) → consequence.

## Files to touch
- `Assets/StreamingAssets/Data/orbital_harrow_events.json` (CREATE — 12 events + 8 consequences)
- Read-only: `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` (confirm event schema:
  event id, signal type, detection window, impact coordinates, strike type, severity),
  `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` (confirm how events feed armor)
- Check loader: `grep -rn "orbital_harrow\|OrbitalHarrow\|harrow_event" Assets/Ashfall.Core/`

## Content grammar (per telemetry event)
- snake_case `id` with prefix `event_` or `telemetry_` (confirm accepted prefix).
- signal_type: radar_anomaly / seismic_precursor / radio_interference / thermal_signature /
  dead_hand_ping (the dead-hand system from W17 in roadmap 31).
- detection_window: ticks between detection and impact (the player's response window).
- strike_type: kinetic_rod / cluster / emp_burst / debris_fall (must match Plan 38 threats).
- severity: damage value passed to `SkyLayerArmorSystem`.
- false_positive_chance: some signals are noise — the player must interpret, not react blindly.

## Content grammar (per consequence)
- snake_case `id` with prefix `consequence_` or `event_` (confirm accepted prefix).
- trigger: armor_breach / armor_hold / evacuation / no_response.
- effects: shelter damage, radiation ingress, fire, structural collapse, survivor casualties,
  morale impact, electronics disruption (EMP).
- delayed_effect: some consequences manifest days later (structural weakness, radiation
  sickness, survivor trauma — feeds existing 09B/27C systems).

## Steps
1. Read `OrbitalHarrowTelemetrySystem.cs` end-to-end: confirm the event schema, the
   detection→impact timeline, the false-positive logic, and the save DTO shape.
2. Confirm how events feed into `SkyLayerArmorSystem` (Plan 38) — are they the same catalog or
   separate? Reconcile with Plan 38's threat events to avoid duplication.
3. Confirm loader status; if missing, add a mechanical loader.
4. Author 12 telemetry events: 4 confirmed kinetic-rod strikes (varying detection windows), 2
   cluster-strike warnings, 2 EMP-burst precursors, 2 dead-hand pings (ambiguous — could be
   drill or real), 2 false-positive radar anomalies.
5. Author 8 consequence records: armor_holds (minimal damage), armor_breaches (shelter
   damage + radiation), successful_evacuation (no casualties but shelter takes hit),
   no_response (full damage + casualties), delayed_structural_weakness, delayed_radiation,
   emp_electronics_failure, morale_trauma.
6. Wire 3 events into radio broadcasts (existing 24A schedule) — the player hears the
   dead-hand ping on shortwave before the telemetry system confirms it.
7. Validate: `--data-integrity-selftest`; confirm a detect → interpret → impact → consequence
   loop works in a headless boot; save round-trip for in-progress events.
8. xUnit: detection window fires correctly, false-positive logic is deterministic (seeded),
   consequences apply per trigger type, delayed effects fire on schedule, save round-trip green.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the Plan 38/39 overlap (threat events vs telemetry events) must be reconciled in
step 2. If they're the same catalog, merge; if separate, clearly delineate (armor threats
vs detection signals).

## Definition of Done
- `orbital_harrow_events.json` exists with 12 events + 8 consequences, all ids resolving,
  detect→impact→consequence loop works end-to-end, false-positive determinism pinned, delayed
  effects fire on schedule, save round-trip green, integrity + tests green.

## Follow-on
- Plan 38 (sky armor) — telemetry events are the warning layer; armor is the defense.
- Existing 19B (orbital strikes) — this plan provides the event data.
- W17 in roadmap 31 (dead-hand system) — dead-hand pings as ambiguous telemetry events.
- Existing 24A (radio schedule) — telemetry signals heard on shortwave before confirmation.

```

## End of Plan 39 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` — 331 lines; 14,047 bytes; SHA-256 `818eb4a35846d0e2ae3324a973af8cafe3c4aa1a966c9d5c9b079cbe63185d37`
Declaration index:
- 00010: public sealed class OrbitalSalvageOpportunity
- 00022: public sealed class OrbitalTelemetryState
- 00044: public sealed class OrbitalWarningEntry
- 00054: public sealed class OrbitalImpactReport
- 00069: public sealed class OrbitalHarrowTelemetrySystem
- 00096: public void ActivateTelemetry(int day)
- 00103: public void ScheduleImpact(int day, int gridX, float energyMj)
- 00108: public void ScheduleEventDef(OrbitalEventDef def, int day, int gridX)
- 00122: private void ScheduleImpactInternal(
- 00156: public ActionResult Brace(string materialId, int amount)
- 00171: public void TickDay(int day)
- 00184: private void ResolveImpact()
- 00266: public bool ApplyInterceptionMitigation(string eventId, float residualFraction)
- 00280: public ActionResult ClaimSalvage(string eventId)
- 00292: private static string GetSalvageItemForEvent(string eventId)
- 00315: public OrbitalTelemetryState CaptureState() => CloneState(_state);
- 00317: public void RestoreState(OrbitalTelemetryState saved)
- 00323: private static OrbitalTelemetryState CloneState(OrbitalTelemetryState src)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Ashfall.Core.Shelter;
00006:
00007: namespace Ashfall.Core
00008: {
00009:     [Serializable]
00010:     public sealed class OrbitalSalvageOpportunity
00011:     {
00012:         public string eventId = string.Empty;
00013:         public string itemId = string.Empty;
00014:         public int quantity = 1;
00015:         public int targetGridX;
00016:         public int spawnDay;
00017:         public int expiresDay;
00018:         public bool isClaimed;
00019:     }
00020:
00021:     [Serializable]
00022:     public sealed class OrbitalTelemetryState
00023:     {
00024:         public string systemId = OrbitalHarrowTelemetrySystem.SystemId;
00025:         public bool telemetryActive;
00026:         public int lastImpactDay = -1;
00027:         public int nextImpactDay = -1;
00028:         public int warningLeadDays = 3;
00029:         public int targetGridX = -1;
00030:         public int affectedCellSpread = 1;
00031:         public float impactEnergyMj = 10f;
00032:         public string scheduledEventId = string.Empty;
00033:         public string scheduledEventName = string.Empty;
00034:         public string revealedSiteId = string.Empty;
00035:         public bool isBraced;
00036:         public bool braceUsed;
00037:         public List<int> impactHistory = new List<int>();
00038:         public List<OrbitalWarningEntry> warnings = new List<OrbitalWarningEntry>();
00039:         public List<OrbitalSalvageOpportunity> activeSalvage = new List<OrbitalSalvageOpportunity>();
00040:         public List<string> revealedSites = new List<string>();
00041:     }
00042:
00043:     [Serializable]
00044:     public sealed class OrbitalWarningEntry
00045:     {
00046:         public int day;
00047:         public int targetGridX;
00048:         public float energyMj;
00049:         public string eventId = string.Empty;
00050:         public string telemetryText = string.Empty;
00051:         public string severity = "Minor";
00052:     }
00053:
00054:     public sealed class OrbitalImpactReport
00055:     {
00056:         public int Day;
00057:         public string EventId = string.Empty;
00058:         public int TargetGridX;
00059:         public int CellsAffected;
00060:         public float TotalEnergyMj;
00061:         public bool AnyBreached;
00062:         public float TotalPenetrationDamage;
00063:         public float PowerGridDisruption;
00064:         public string SalvageItemId = string.Empty;
00065:         public int SalvageQuantity;
00066:         public string RevealedSiteId = string.Empty;
00067:     }
00068:
00069:     public sealed class OrbitalHarrowTelemetrySystem
00070:     {
00071:         public const string SystemId = "orbital_harrow_telemetry";
00072:
00073:         private OrbitalTelemetryState _state = new OrbitalTelemetryState();
00074:         private readonly SkyLayerArmorSystem _armor;
00075:         private readonly ISeededRng _rng;
00076:         private readonly ILog _log;
00077:         private int _currentDay;
00078:
00079:         public OrbitalTelemetryState State => _state;
00080:         public bool HasPendingImpact => _state.nextImpactDay > _currentDay;
00081:         public IReadOnlyList<OrbitalSalvageOpportunity> ActiveSalvage => _state.activeSalvage.AsReadOnly();
00082:         public IReadOnlyList<string> RevealedSites => _state.revealedSites.AsReadOnly();
00083:
00084:         public event Action<OrbitalWarningEntry> OnImpactWarning;
00085:         public event Action<int, float> OnImpactResolved; // day, energy
00086:         public event Action<OrbitalImpactReport> OnImpactDetailed;
00087:         public event Action OnTelemetryChanged;
00088:
00089:         public OrbitalHarrowTelemetrySystem(SkyLayerArmorSystem armor, ISeededRng rng, ILog? log = null)
00090:         {
00091:             _armor = armor ?? throw new ArgumentNullException(nameof(armor));
00092:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00093:             _log = log ?? NullLog.Instance;
00094:         }
00095:
00096:         public void ActivateTelemetry(int day)
00097:         {
00098:             _state.telemetryActive = true;
00099:             _log.Info($"[OrbitalHarrow] telemetry activated on day {day}");
00100:             OnTelemetryChanged?.Invoke();
00101:         }
00102:
00103:         public void ScheduleImpact(int day, int gridX, float energyMj)
00104:         {
00105:             ScheduleImpactInternal(day, gridX, energyMj, spread: 1, eventId: "custom_impact", eventName: "Kinetic Debris Impact", siteId: string.Empty, severity: "Moderate");
00106:         }
00107:
00108:         public void ScheduleEventDef(OrbitalEventDef def, int day, int gridX)
00109:         {
00110:             if (def == null) return;
00111:             ScheduleImpactInternal(
00112:                 day: day,
00113:                 gridX: gridX,
00114:                 energyMj: def.impact_energy_mj,
00115:                 spread: Math.Max(1, def.affected_cell_spread),
00116:                 eventId: def.id,
00117:                 eventName: def.name,
00118:                 siteId: def.revealed_site_id,
00119:                 severity: def.severity);
00120:         }
00121:
00122:         private void ScheduleImpactInternal(
00123:             int day,
00124:             int gridX,
00125:             float energyMj,
00126:             int spread,
00127:             string eventId,
00128:             string eventName,
00129:             string siteId,
00130:             string severity)
00131:         {
00132:             _state.nextImpactDay = day;
00133:             _state.targetGridX = gridX;
00134:             _state.impactEnergyMj = energyMj;
00135:             _state.affectedCellSpread = Math.Max(1, spread);
00136:             _state.scheduledEventId = eventId;
00137:             _state.scheduledEventName = eventName;
00138:             _state.revealedSiteId = siteId ?? string.Empty;
00139:             _state.isBraced = false;
00140:             _state.braceUsed = false;
00141:
00142:             var warning = new OrbitalWarningEntry
00143:             {
00144:                 day = day,
00145:                 targetGridX = gridX,
00146:                 energyMj = energyMj,
00147:                 eventId = eventId,
00148:                 severity = severity,
00149:                 telemetryText = $"ORBITAL WARNING [Day {day}]: {eventName} over Grid {gridX} (Energy {energyMj:F1} MJ, Spread {spread} cells)"
00150:             };
00151:             _state.warnings.Add(warning);
00152:             OnImpactWarning?.Invoke(warning);
00153:             OnTelemetryChanged?.Invoke();
00154:         }
00155:
00156:         public ActionResult Brace(string materialId, int amount)
00157:         {
00158:             if (!HasPendingImpact)
00159:                 return ActionResult.Blocked("no_impact", "orbital.no_impact");
00160:             if (_state.braceUsed)
00161:                 return ActionResult.Blocked("already_braced", "orbital.already_braced");
00162:
00163:             _state.isBraced = true;
00164:             _state.braceUsed = true;
00165:             _log.Info($"[OrbitalHarrow] braced with {amount}x {materialId}");
00166:             OnTelemetryChanged?.Invoke();
00167:             return ActionResult.Success("orbital.braced",
00168:                 new Dictionary<string, double> { { "mitigation", _state.isBraced ? 0.5 : 0.0 } });
00169:         }
00170:
00171:         public void TickDay(int day)
00172:         {
00173:             _currentDay = day;
00174:
00175:             // Prune expired salvage opportunities
00176:             _state.activeSalvage.RemoveAll(s => s.expiresDay < day && !s.isClaimed);
00177:
00178:             if (_state.nextImpactDay == day)
00179:             {
00180:                 ResolveImpact();
00181:             }
00182:         }
00183:
00184:         private void ResolveImpact()
00185:         {
00186:             float totalEnergy = _state.isBraced ? _state.impactEnergyMj * 0.5f : _state.impactEnergyMj;
00187:             int spread = Math.Max(1, _state.affectedCellSpread);
00188:             float perCellEnergy = totalEnergy / spread;
00189:
00190:             bool anyBreached = false;
00191:             float totalDamage = 0f;
00192:
00193:             for (int offset = 0; offset < spread; offset++)
00194:             {
00195:                 int cellX = _state.targetGridX + offset;
00196:                 bool breached = _armor.EvaluateKineticImpact(cellX, perCellEnergy, out float cellDamage);
00197:                 if (breached)
00198:                 {
00199:                     anyBreached = true;
00200:                     totalDamage += cellDamage;
00201:                 }
00202:             }
00203:
00204:             // Downstream shelter cascading power impact
00205:             float powerDisruption = anyBreached ? Math.Min(100f, totalDamage * 2.5f) : 0f;
00206:
00207:             // Spawn salvage aftermath if item yield exists
00208:             string salvageItem = !string.IsNullOrEmpty(_state.scheduledEventId)
00209:                 ? GetSalvageItemForEvent(_state.scheduledEventId)
00210:                 : "scrap_mechanical";
00211:             int salvageQty = Math.Max(1, (int)Math.Round(totalEnergy / 6f));
00212:
00213:             var salvage = new OrbitalSalvageOpportunity
00214:             {
00215:                 eventId = _state.scheduledEventId,
00216:                 itemId = salvageItem,
00217:                 quantity = salvageQty,
00218:                 targetGridX = _state.targetGridX,
00219:                 spawnDay = _state.nextImpactDay,
00220:                 expiresDay = _state.nextImpactDay + 7,
00221:                 isClaimed = false
00222:             };
00223:             _state.activeSalvage.Add(salvage);
00224:
00225:             // Reveal hidden site if present
00226:             if (!string.IsNullOrEmpty(_state.revealedSiteId) && !_state.revealedSites.Contains(_state.revealedSiteId))
00227:             {
00228:                 _state.revealedSites.Add(_state.revealedSiteId);
00229:             }
00230:
00231:             int resolvedDay = _state.nextImpactDay;
00232:             _state.impactHistory.Add(resolvedDay);
00233:             _state.lastImpactDay = resolvedDay;
00234:             _state.nextImpactDay = -1;
00235:             _state.isBraced = false;
00236:
00237:             var report = new OrbitalImpactReport
00238:             {
00239:                 Day = resolvedDay,
00240:                 EventId = _state.scheduledEventId,
00241:                 TargetGridX = _state.targetGridX,
00242:                 CellsAffected = spread,
00243:                 TotalEnergyMj = totalEnergy,
00244:                 AnyBreached = anyBreached,
00245:                 TotalPenetrationDamage = totalDamage,
00246:                 PowerGridDisruption = powerDisruption,
00247:                 SalvageItemId = salvageItem,
00248:                 SalvageQuantity = salvageQty,
00249:                 RevealedSiteId = _state.revealedSiteId
00250:             };
00251:
00252:             _log.Info($"[OrbitalHarrow] impact resolved: breached={anyBreached}, damage={totalDamage:F1}, powerDisruption={powerDisruption:F1}");
00253:             OnImpactResolved?.Invoke(resolvedDay, totalEnergy);
00254:             OnImpactDetailed?.Invoke(report);
00255:             OnTelemetryChanged?.Invoke();
00256:         }
00257:
00258:         /// <summary>
00259:         /// Flagship sky-defense integration (Task 7): reduce the pending
00260:         /// impact's energy after a successful interception. The residual
00261:         /// fraction keeps flowing through the normal armor pipeline in
00262:         /// <see cref="ResolveImpact"/> — interception modifies the strike,
00263:         /// it never bypasses shelter armor. Returns false when no pending
00264:         /// impact matches the event id.
00265:         /// </summary>
00266:         public bool ApplyInterceptionMitigation(string eventId, float residualFraction)
00267:         {
00268:             if (string.IsNullOrEmpty(eventId)) return false;
00269:             if (_state.nextImpactDay < _currentDay && _state.nextImpactDay != _currentDay) return false;
00270:             if (_state.nextImpactDay < 0) return false;
00271:             if (!string.Equals(_state.scheduledEventId, eventId, StringComparison.Ordinal)) return false;
00272:
00273:             float clamped = Math.Clamp(residualFraction, 0f, 1f);
00274:             _state.impactEnergyMj = Math.Max(0f, _state.impactEnergyMj * clamped);
00275:             _log.Info($"[OrbitalHarrow] interception mitigation for '{eventId}': residual fraction {clamped:F2}, energy now {_state.impactEnergyMj:F1} MJ");
00276:             OnTelemetryChanged?.Invoke();
00277:             return true;
00278:         }
00279:
00280:         public ActionResult ClaimSalvage(string eventId)
00281:         {
00282:             var opp = _state.activeSalvage.Find(s => s.eventId == eventId && !s.isClaimed);
00283:             if (opp == null)
00284:                 return ActionResult.Blocked("not_found", "orbital.salvage_not_found");
00285:
00286:             opp.isClaimed = true;
00287:             OnTelemetryChanged?.Invoke();
00288:             return ActionResult.Success("orbital.salvage_claimed",
00289:                 new Dictionary<string, double> { { "quantity", opp.quantity } });
00290:         }
00291:
00292:         private static string GetSalvageItemForEvent(string eventId)
00293:         {
00294:             return eventId switch
00295:             {
00296:                 "event_orbital_kinetic_early_track" or "event_orbital_fragmented_kinetic_rod" or "event_orbital_small_debris_shower" => "scrap_mechanical",
00297:                 "event_orbital_kinetic_thermal_descent" or "event_orbital_standard_kinetic_strike" or "event_orbital_heavy_kinetic_impact" => "scrap_electronic",
00298:                 "event_orbital_kinetic_seismic_precursor" or "event_orbital_heavy_penetrator_impact" or "event_orbital_low_warning_strike" => "heavy_industrial_motor",
00299:                 "event_orbital_kinetic_fragmented_track" => "scrap_mechanical",
00300:                 "event_orbital_cluster_multiple_returns" or "event_orbital_telemetry_station_cluster" or "event_orbital_clustered_impact" => "copper_wire_10m_of_10m",
00301:                 "event_orbital_cluster_split_track" or "event_orbital_defense_submunitions_spread" => "mechanical_parts",
00302:                 "event_orbital_emp_radio_blackout" or "event_orbital_airburst_emp_detonation" => "battery",
00303:                 "event_orbital_emp_signature_mismatch" or "event_orbital_ionized_plasma_shockwave" or "event_orbital_near_miss_shockwave" => "fuel",
00304:                 "event_orbital_dead_hand_repeating_ping" => "scrap_electronic",
00305:                 "event_orbital_dead_hand_broken_checksum" => "scrap_electronic",
00306:                 "event_orbital_radar_ducting_false_alarm" => "scrap_mechanical",
00307:                 "event_orbital_debris_misclassification" => "scrap_metal",
00308:                 "event_orbital_catastrophic_kinetic_lance" => "scrap_electronic",
00309:                 "event_orbital_solar_array_debris_shower" => "scrap_mechanical",
00310:                 "event_orbital_booster_casing_decay" => "scrap_metal",
00311:                 _ => "scrap_mechanical"
00312:             };
00313:         }
00314:
00315:         public OrbitalTelemetryState CaptureState() => CloneState(_state);
00316:
00317:         public void RestoreState(OrbitalTelemetryState saved)
00318:         {
00319:             if (saved == null) return;
00320:             _state = CloneState(saved);
00321:         }
00322:
00323:         private static OrbitalTelemetryState CloneState(OrbitalTelemetryState src)
00324:         {
00325:             if (src == null) return new OrbitalTelemetryState();
00326:             var s = new SystemTextJsonSerializer();
00327:             var json = s.Serialize(src);
00328:             return s.Deserialize<OrbitalTelemetryState>(json) ?? new OrbitalTelemetryState();
00329:         }
00330:     }
00331: }
```

## `Assets/Ashfall.Core/Shelter/OrbitalHarrowCatalog.cs` — 69 lines; 2,205 bytes; SHA-256 `6d77064ecd5864203314d577462cca2cd002fb00fa2519cef11d407e8a346555`
Declaration index:
- 00010: public sealed class OrbitalEventDef
- 00029: public sealed class OrbitalHarrowCatalogFile
- 00035: public static class OrbitalHarrowCatalogLoader
- 00039: public static List<OrbitalEventDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Ashfall.Core.IO;
00006:
00007: namespace Ashfall.Core.Shelter
00008: {
00009:     [Serializable]
00010:     public sealed class OrbitalEventDef
00011:     {
00012:         public string id = string.Empty;
00013:         public string name = string.Empty;
00014:         public string description = string.Empty;
00015:         public string severity = "Minor";
00016:         public string signal_type = "radar_anomaly";
00017:         public bool is_false_positive = false;
00018:         public float impact_energy_mj = 10f;
00019:         public int lead_time_days = 3;
00020:         public int affected_cell_spread = 1;
00021:         public float penetration_power_mj = 8f;
00022:         public string salvage_yield_item_id = string.Empty;
00023:         public int salvage_yield_quantity = 1;
00024:         public string revealed_site_id = string.Empty;
00025:         public string radio_hook_text = string.Empty;
00026:     }
00027:
00028:     [Serializable]
00029:     public sealed class OrbitalHarrowCatalogFile
00030:     {
00031:         public int schema_version = 1;
00032:         public List<OrbitalEventDef> events = new List<OrbitalEventDef>();
00033:     }
00034:
00035:     public static class OrbitalHarrowCatalogLoader
00036:     {
00037:         public const string FileName = "orbital_harrow_events.json";
00038:
00039:         public static List<OrbitalEventDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
00040:         {
00041:             var list = new List<OrbitalEventDef>();
00042:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00043:                 return list;
00044:
00045:             string path = fileIO.Combine(dataDir, FileName);
00046:             if (!fileIO.FileExists(path))
00047:                 return list;
00048:
00049:             string raw = fileIO.ReadAllText(path);
00050:             if (string.IsNullOrWhiteSpace(raw))
00051:                 return list;
00052:
00053:             try
00054:             {
00055:                 var parsed = json.Deserialize<OrbitalHarrowCatalogFile>(raw);
00056:                 if (parsed?.events != null)
00057:                 {
00058:                     list.AddRange(parsed.events);
00059:                 }
00060:             }
00061:             catch (Exception ex)
00062:             {
00063:                 CatalogDiagnostics.Warn(path, "OrbitalHarrowCatalogFile", ex);
00064:             }
00065:
00066:             return list;
00067:         }
00068:     }
00069: }
```

## `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` — 143 lines; 5,219 bytes; SHA-256 `15e0b1b127dc15d88f38dd96f25d432cddad739e13fed2897b6349a86d530f39`
Declaration index:
- 00007: public enum CeilingMaterialTier { Dirt, Wood, ReinforcedConcrete, LeadSheeting, TungstenComposite }
- 00010: public sealed class CeilingCellArmor
- 00019: public sealed class SkyArmorSaveState
- 00028: public sealed class SkyLayerArmorSystem
- 00032: public void SetCellArmor(int gridX, CeilingMaterialTier material, float thicknessMeters, float durability = 100f)
- 00043: public void InstallConfiguration(int gridX, SkyLayerArmorConfigDef config)
- 00049: public CeilingCellArmor? GetCell(int gridX)
- 00054: public float GetAttenuationFactor(int gridX)
- 00073: public void RepairCell(int gridX, float durabilityAmount)
- 00081: public bool EvaluateKineticImpact(int gridX, float impactEnergyMegaJoules, out float damageDealtToRoof)
- 00111: public SkyArmorSaveState CaptureState()
- 00127: public void RestoreState(SkyArmorSaveState state)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core.Shelter
00006: {
00007:     public enum CeilingMaterialTier { Dirt, Wood, ReinforcedConcrete, LeadSheeting, TungstenComposite }
00008:
00009:     [Serializable]
00010:     public sealed class CeilingCellArmor
00011:     {
00012:         public int gridX;
00013:         public CeilingMaterialTier material;
00014:         public float thicknessMeters;
00015:         public float currentDurability; // 0.0 to 100.0
00016:     }
00017:
00018:     [Serializable]
00019:     public sealed class SkyArmorSaveState
00020:     {
00021:         public List<CeilingCellArmor> cells = new List<CeilingCellArmor>();
00022:     }
00023:
00024:     /// <summary>
00025:     /// ASHFALL: THE ORBITAL HARROW (Expansion 11) — Sky Layer Armor System.
00026:     /// Simulates 2D overhead vertical armor, atmospheric rad attenuation, and kinetic impact resistance.
00027:     /// </summary>
00028:     public sealed class SkyLayerArmorSystem
00029:     {
00030:         private readonly Dictionary<int, CeilingCellArmor> _cells = new Dictionary<int, CeilingCellArmor>();
00031:
00032:         public void SetCellArmor(int gridX, CeilingMaterialTier material, float thicknessMeters, float durability = 100f)
00033:         {
00034:             _cells[gridX] = new CeilingCellArmor
00035:             {
00036:                 gridX = gridX,
00037:                 material = material,
00038:                 thicknessMeters = Math.Max(0.1f, thicknessMeters),
00039:                 currentDurability = MathfCompat.Clamp(durability, 0f, 100f)
00040:             };
00041:         }
00042:
00043:         public void InstallConfiguration(int gridX, SkyLayerArmorConfigDef config)
00044:         {
00045:             if (config == null) return;
00046:             SetCellArmor(gridX, config.material_tier, config.default_thickness_meters, 100f);
00047:         }
00048:
00049:         public CeilingCellArmor? GetCell(int gridX)
00050:         {
00051:             return _cells.TryGetValue(gridX, out var cell) ? cell : null;
00052:         }
00053:
00054:         public float GetAttenuationFactor(int gridX)
00055:         {
00056:             if (!_cells.TryGetValue(gridX, out var cell))
00057:                 return 1.0f; // Unprotected surface bleed
00058:
00059:             float baseMultiplier = cell.material switch
00060:             {
00061:                 CeilingMaterialTier.Dirt => 0.60f,
00062:                 CeilingMaterialTier.Wood => 0.85f,
00063:                 CeilingMaterialTier.ReinforcedConcrete => 0.20f,
00064:                 CeilingMaterialTier.LeadSheeting => 0.05f,
00065:                 CeilingMaterialTier.TungstenComposite => 0.01f,
00066:                 _ => 1.0f
00067:             };
00068:
00069:             float conditionFactor = Math.Max(0.2f, cell.currentDurability / 100f);
00070:             return MathfCompat.Clamp(baseMultiplier / (cell.thicknessMeters * conditionFactor), 0.005f, 1.0f);
00071:         }
00072:
00073:         public void RepairCell(int gridX, float durabilityAmount)
00074:         {
00075:             if (_cells.TryGetValue(gridX, out var cell))
00076:             {
00077:                 cell.currentDurability = Math.Min(100f, cell.currentDurability + Math.Max(0f, durabilityAmount));
00078:             }
00079:         }
00080:
00081:         public bool EvaluateKineticImpact(int gridX, float impactEnergyMegaJoules, out float damageDealtToRoof)
00082:         {
00083:             damageDealtToRoof = 0f;
00084:             if (!_cells.TryGetValue(gridX, out var cell))
00085:             {
00086:                 damageDealtToRoof = impactEnergyMegaJoules * 10f;
00087:                 return true; // Complete penetration into unprotected bunker
00088:             }
00089:
00090:             float absorptionThreshold = cell.material switch
00091:             {
00092:                 CeilingMaterialTier.Dirt => 5f,
00093:                 CeilingMaterialTier.Wood => 2f,
00094:                 CeilingMaterialTier.ReinforcedConcrete => 25f,
00095:                 CeilingMaterialTier.LeadSheeting => 15f,
00096:                 CeilingMaterialTier.TungstenComposite => 80f,
00097:                 _ => 1f
00098:             } * cell.thicknessMeters;
00099:
00100:             if (impactEnergyMegaJoules > absorptionThreshold)
00101:             {
00102:                 cell.currentDurability = Math.Max(0f, cell.currentDurability - 50f);
00103:                 damageDealtToRoof = impactEnergyMegaJoules - absorptionThreshold;
00104:                 return true; // Breached
00105:             }
00106:
00107:             cell.currentDurability = Math.Max(0f, cell.currentDurability - (impactEnergyMegaJoules / absorptionThreshold) * 20f);
00108:             return false; // Absorbed by armor
00109:         }
00110:
00111:         public SkyArmorSaveState CaptureState()
00112:         {
00113:             var save = new SkyArmorSaveState();
00114:             foreach (var kvp in _cells)
00115:             {
00116:                 save.cells.Add(new CeilingCellArmor
00117:                 {
00118:                     gridX = kvp.Value.gridX,
00119:                     material = kvp.Value.material,
00120:                     thicknessMeters = kvp.Value.thicknessMeters,
00121:                     currentDurability = kvp.Value.currentDurability
00122:                 });
00123:             }
00124:             return save;
00125:         }
00126:
00127:         public void RestoreState(SkyArmorSaveState state)
00128:         {
00129:             _cells.Clear();
00130:             if (state?.cells == null) return;
00131:             foreach (var c in state.cells)
00132:             {
00133:                 _cells[c.gridX] = new CeilingCellArmor
00134:                 {
00135:                     gridX = c.gridX,
00136:                     material = c.material,
00137:                     thicknessMeters = c.thicknessMeters,
00138:                     currentDurability = c.currentDurability
00139:                 };
00140:             }
00141:         }
00142:     }
00143: }
```

## `Assets/Ashfall.Core/Shelter/SkyLayerArmorCatalog.cs` — 221 lines; 11,073 bytes; SHA-256 `c10d386d924cbefdb2ab02a84725411f7e032061be87d420079bbde51a9f7a7a`
Declaration index:
- 00009: public sealed class ArmorMaterialCostDef
- 00019: public sealed class SkyLayerArmorConfigDef
- 00038: public sealed class SkyLayerArmorCatalogContainer
- 00047: public static class SkyLayerArmorCatalogLoader
- 00051: public static List<SkyLayerArmorConfigDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
- 00078: public static List<SkyLayerArmorConfigDef> GetDefaultConfigurations()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.IO;
00005:
00006: namespace Ashfall.Core.Shelter
00007: {
00008:     [Serializable]
00009:     public sealed class ArmorMaterialCostDef
00010:     {
00011:         public string item_id { get; set; } = string.Empty;
00012:         public int quantity { get; set; } = 1;
00013:     }
00014:
00015:     /// <summary>
00016:     /// DTO defining an authored sky-layer armor configuration.
00017:     /// </summary>
00018:     [Serializable]
00019:     public sealed class SkyLayerArmorConfigDef
00020:     {
00021:         public string id { get; set; } = string.Empty;
00022:         public string name { get; set; } = string.Empty;
00023:         public string description { get; set; } = string.Empty;
00024:         public string tier { get; set; } = "improvised"; // improvised, reinforced, military_grade
00025:         public CeilingMaterialTier material_tier { get; set; } = CeilingMaterialTier.Dirt;
00026:         public float default_thickness_meters { get; set; } = 1.0f;
00027:         public float blast_resistance_mj { get; set; } = 10f;
00028:         public float attenuation_factor { get; set; } = 0.5f;
00029:         public float degradation_rate { get; set; } = 0.2f;
00030:         public List<ArmorMaterialCostDef> composition { get; set; } = new List<ArmorMaterialCostDef>();
00031:         public List<ArmorMaterialCostDef> repair_cost { get; set; } = new List<ArmorMaterialCostDef>();
00032:     }
00033:
00034:     /// <summary>
00035:     /// Container for sky_layer_armor_catalog.json.
00036:     /// </summary>
00037:     [Serializable]
00038:     public sealed class SkyLayerArmorCatalogContainer
00039:     {
00040:         public int schema_version { get; set; } = 1;
00041:         public List<SkyLayerArmorConfigDef> configurations { get; set; } = new List<SkyLayerArmorConfigDef>();
00042:     }
00043:
00044:     /// <summary>
00045:     /// Loader and query authority for authored sky-layer armor configurations.
00046:     /// </summary>
00047:     public static class SkyLayerArmorCatalogLoader
00048:     {
00049:         public const string CatalogFileName = "sky_layer_armor_catalog.json";
00050:
00051:         public static List<SkyLayerArmorConfigDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
00052:         {
00053:             fileIO ??= new FileSystemIO();
00054:             serializer ??= new SystemTextJsonSerializer();
00055:
00056:             if (string.IsNullOrEmpty(dataDir))
00057:                 return GetDefaultConfigurations();
00058:
00059:             string fullPath = fileIO.Combine(dataDir, CatalogFileName);
00060:             if (!fileIO.FileExists(fullPath))
00061:                 return GetDefaultConfigurations();
00062:
00063:             try
00064:             {
00065:                 string json = fileIO.ReadAllText(fullPath);
00066:                 var container = serializer.Deserialize<SkyLayerArmorCatalogContainer>(json);
00067:                 if (container?.configurations != null && container.configurations.Count > 0)
00068:                     return container.configurations;
00069:             }
00070:             catch (Exception ex)
00071:             {
00072:                 CatalogDiagnostics.Warn(fullPath, "SkyLayerArmorCatalogContainer", ex);
00073:             }
00074:
00075:             return GetDefaultConfigurations();
00076:         }
00077:
00078:         public static List<SkyLayerArmorConfigDef> GetDefaultConfigurations()
00079:         {
00080:             return new List<SkyLayerArmorConfigDef>
00081:             {
00082:                 new SkyLayerArmorConfigDef
00083:                 {
00084:                     id = "sky_armor_sandbag_layer",
00085:                     name = "Improvised Sandbag Layer",
00086:                     description = "Stacked burlap sacks filled with dry soil and gravel, braced with timber struts. Protects against light shrapnel but degrades rapidly under direct kinetic hits.",
00087:                     tier = "improvised",
00088:                     material_tier = CeilingMaterialTier.Dirt,
00089:                     default_thickness_meters = 0.8f,
00090:                     blast_resistance_mj = 5.0f,
00091:                     attenuation_factor = 0.60f,
00092:                     degradation_rate = 0.35f,
00093:                     composition = new List<ArmorMaterialCostDef>
00094:                     {
00095:                         new ArmorMaterialCostDef { item_id = "scrap_wood", quantity = 4 },
00096:                         new ArmorMaterialCostDef { item_id = "cloth", quantity = 6 }
00097:                     },
00098:                     repair_cost = new List<ArmorMaterialCostDef>
00099:                     {
00100:                         new ArmorMaterialCostDef { item_id = "scrap_wood", quantity = 1 },
00101:                         new ArmorMaterialCostDef { item_id = "cloth", quantity = 2 }
00102:                     }
00103:                 },
00104:                 new SkyLayerArmorConfigDef
00105:                 {
00106:                     id = "sky_armor_scrap_overlay",
00107:                     name = "Scrap-Plate Overlay",
00108:                     description = "Overlapping salvage steel plates bolted across roof framing. Provides resilient deflection against low-velocity orbital debris.",
00109:                     tier = "improvised",
00110:                     material_tier = CeilingMaterialTier.Wood,
00111:                     default_thickness_meters = 1.0f,
00112:                     blast_resistance_mj = 12.0f,
00113:                     attenuation_factor = 0.45f,
00114:                     degradation_rate = 0.25f,
00115:                     composition = new List<ArmorMaterialCostDef>
00116:                     {
00117:                         new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 8 },
00118:                         new ArmorMaterialCostDef { item_id = "mechanical_parts", quantity = 4 }
00119:                     },
00120:                     repair_cost = new List<ArmorMaterialCostDef>
00121:                     {
00122:                         new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 3 }
00123:                     }
00124:                 },
00125:                 new SkyLayerArmorConfigDef
00126:                 {
00127:                     id = "sky_armor_reinforced_concrete",
00128:                     name = "Reinforced Concrete Slab",
00129:                     description = "Poured aggregate cement laced with scrap steel rebar. High mass and blast dampening offer durable long-term orbital mitigation.",
00130:                     tier = "reinforced",
00131:                     material_tier = CeilingMaterialTier.ReinforcedConcrete,
00132:                     default_thickness_meters = 1.5f,
00133:                     blast_resistance_mj = 37.5f,
00134:                     attenuation_factor = 0.20f,
00135:                     degradation_rate = 0.15f,
00136:                     composition = new List<ArmorMaterialCostDef>
00137:                     {
00138:                         new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 10 },
00139:                         new ArmorMaterialCostDef { item_id = "chemicals", quantity = 4 },
00140:                         new ArmorMaterialCostDef { item_id = "scrap_wood", quantity = 4 }
00141:                     },
00142:                     repair_cost = new List<ArmorMaterialCostDef>
00143:                     {
00144:                         new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 3 },
00145:                         new ArmorMaterialCostDef { item_id = "chemicals", quantity = 1 }
00146:                     }
00147:                 },
00148:                 new SkyLayerArmorConfigDef
00149:                 {
00150:                     id = "sky_armor_steel_hull_plating",
00151:                     name = "Steel Hull Plating",
00152:                     description = "Heavy naval and industrial pressure-hull plate welded directly over bunker arches. Excellent resistance against hypervelocity kinetic penetrators.",
00153:                     tier = "reinforced",
00154:                     material_tier = CeilingMaterialTier.LeadSheeting,
00155:                     default_thickness_meters = 1.2f,
00156:                     blast_resistance_mj = 45.0f,
00157:                     attenuation_factor = 0.05f,
00158:                     degradation_rate = 0.12f,
00159:                     composition = new List<ArmorMaterialCostDef>
00160:                     {
00161:                         new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 14 },
00162:                         new ArmorMaterialCostDef { item_id = "mechanical_parts", quantity = 6 },
00163:                         new ArmorMaterialCostDef { item_id = "fuel", quantity = 2 }
00164:                     },
00165:                     repair_cost = new List<ArmorMaterialCostDef>
00166:                     {
00167:                         new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 4 },
00168:                         new ArmorMaterialCostDef { item_id = "mechanical_parts", quantity = 2 }
00169:                     }
00170:                 },
00171:                 new SkyLayerArmorConfigDef
00172:                 {
00173:                     id = "sky_armor_composite_military",
00174:                     name = "Composite Military-Grade Armor",
00175:                     description = "Layered tungsten-carbide tiles and shock-absorbing ceramic matrix. Top-tier kinetic and radiation shielding designed for military continuity vaults.",
00176:                     tier = "military_grade",
00177:                     material_tier = CeilingMaterialTier.TungstenComposite,
00178:                     default_thickness_meters = 2.0f,
00179:                     blast_resistance_mj = 160.0f,
00180:                     attenuation_factor = 0.01f,
00181:                     degradation_rate = 0.08f,
00182:                     composition = new List<ArmorMaterialCostDef>
00183:                     {
00184:                         new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 20 },
00185:                         new ArmorMaterialCostDef { item_id = "electronic_scrap", quantity = 8 },
00186:                         new ArmorMaterialCostDef { item_id = "mechanical_parts", quantity = 10 }
00187:                     },
00188:                     repair_cost = new List<ArmorMaterialCostDef>
00189:                     {
00190:                         new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 5 },
00191:                         new ArmorMaterialCostDef { item_id = "electronic_scrap", quantity = 2 },
00192:                         new ArmorMaterialCostDef { item_id = "mechanical_parts", quantity = 2 }
00193:                     }
00194:                 },
00195:                 new SkyLayerArmorConfigDef
00196:                 {
00197:                     id = "sky_armor_emergency_blast_canopy",
00198:                     name = "Emergency Blast Canopy",
00199:                     description = "A sacrificial pitched timber and sheet canopy designed for rapid deployment ahead of predicted orbital debris showers.",
00200:                     tier = "improvised",
00201:                     material_tier = CeilingMaterialTier.Wood,
00202:                     default_thickness_meters = 0.6f,
00203:                     blast_resistance_mj = 8.0f,
00204:                     attenuation_factor = 0.50f,
00205:                     degradation_rate = 0.40f,
00206:                     composition = new List<ArmorMaterialCostDef>
00207:                     {
00208:                         new ArmorMaterialCostDef { item_id = "scrap_wood", quantity = 6 },
00209:                         new ArmorMaterialCostDef { item_id = "cloth", quantity = 4 },
00210:                         new ArmorMaterialCostDef { item_id = "scrap_metal", quantity = 2 }
00211:                     },
00212:                     repair_cost = new List<ArmorMaterialCostDef>
00213:                     {
00214:                         new ArmorMaterialCostDef { item_id = "scrap_wood", quantity = 2 },
00215:                         new ArmorMaterialCostDef { item_id = "cloth", quantity = 2 }
00216:                     }
00217:                 }
00218:             };
00219:         }
00220:     }
00221: }
```

## `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs` — 355 lines; 15,367 bytes; SHA-256 `962ab63c433b2bf189343ca17c0807b637189bcc8b1ec4c08d6b82a0725139b5`
Declaration index:
- 00019: public sealed class WeatherIntelligenceSaveState
- 00037: public sealed class WeatherIntelligenceReadModel
- 00103: public sealed class WeatherIntelligenceCoordinator
- 00156: public void TickDay(int day)
- 00172: public WeatherIntelligenceReadModel BuildReadModel()
- 00262: private static bool IsSevereWeather(WeatherKind kind)
- 00279: private static string GetCrisisAdvice(WeatherKind kind)
- 00291: private static string BuildAdvisory(WeatherIntelligenceReadModel rm)
- 00332: public WeatherIntelligenceSaveState CaptureState()
- 00343: public void RestoreState(WeatherIntelligenceSaveState? saved)
- 00353: private void RaiseChanged() => OnIntelligenceChanged?.Invoke();
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Text;
00005: #pragma warning disable CS8618
00006:
00007: using Ashfall.Core.Shelter;
00008:
00009: namespace Ashfall.Core.World
00010: {
00011:     // ── Aggregate save DTO (one nested state, not two loose files) ──────────
00012:
00013:     /// <summary>
00014:     /// Single persisted state for the weather-intelligence cluster. Nests the
00015:     /// weather-station calibration/forecast state, orbital-harrow telemetry
00016:     /// state, and seasonal events so the world section persists them together.
00017:     /// </summary>
00018:     [Serializable]
00019:     public sealed class WeatherIntelligenceSaveState
00020:     {
00021:         public WeatherStationState station = new WeatherStationState();
00022:         public OrbitalTelemetryState orbital = new OrbitalTelemetryState();
00023:         public SeasonalEventSaveState seasonal = new SeasonalEventSaveState();
00024:         public CloudSeedingSaveState? cloudSeeding;
00025:     }
00026:
00027:     // ── Read model (consumed by Weather and Map panels) ─────────────────────
00028:
00029:     /// <summary>
00030:     /// Read-only projection of weather intelligence for the UI: forecast
00031:     /// confidence, orbital warning lead time, seasonal phase, and expedition route-safety
00032:     /// information. Improving weather infrastructure (installing/calibrating
00033:     /// the station, activating orbital telemetry) demonstrably enriches this
00034:     /// model — that is the player-facing signal that infrastructure matters.
00035:     /// </summary>
00036:     [Serializable]
00037:     public sealed class WeatherIntelligenceReadModel
00038:     {
00039:         // Season
00040:         public string seasonId = string.Empty;
00041:         public string seasonDisplayName = string.Empty;
00042:
00043:         // Station
00044:         public bool stationInstalled;
00045:         public bool stationCalibrated;
00046:         public bool stationOperational;
00047:         public WeatherStationTier stationTier;
00048:         public string stationTierName = string.Empty;
00049:         public float stationAccuracy;
00050:         public float stationDurability;
00051:         public int forecastHorizonDays;
00052:         public int lastForecastDay;
00053:         public List<ForecastEntry> forecast = new List<ForecastEntry>();
00054:
00055:         // Orbital
00056:         public bool telemetryActive;
00057:         public bool hasPendingImpact;
00058:         public int impactDay;
00059:         public int warningLeadDays;
00060:         public int daysUntilImpact;
00061:         public int activeSalvageCount;
00062:         public List<OrbitalSalvageOpportunity> activeSalvage = new List<OrbitalSalvageOpportunity>();
00063:
00064:         // Seasonal events
00065:         public List<ActiveSeasonalEvent> activeSeasonalEvents = new List<ActiveSeasonalEvent>();
00066:
00067:         // Derived expedition information
00068:         public int routeSafeDays;
00069:         public int bestTravelDay;
00070:         public float bestTravelConfidence;
00071:         public string advisory = string.Empty;
00072:
00073:         // C1.4 — Crisis Prediction
00074:         public string? predictedCrisisEventId;
00075:         public int predictedCrisisDay;
00076:         public float predictedCrisisConfidence;
00077:         public string? crisisPreparationAdvice;
00078:         public WeatherKind? predictedWeatherKind;
00079:         public bool hasPredictedCrisis;
00080:         public int daysUntilPredictedCrisis;
00081:         public string predictionSource = string.Empty;
00082:         public bool isStationCalibrated;
00083:
00084:         // C1.5 — Cloud Seeding
00085:         public bool cloudSeedingInstalled;
00086:         public bool cloudSeedingOnCooldown;
00087:         public int cloudSeedingCooldownDays;
00088:         public bool cloudSeedingPartialProtectionActive;
00089:     }
00090:
00091:     /// <summary>
00092:     /// Weather-intelligence coordinator — the single wiring point for the
00093:     /// weather-infrastructure systems (WeatherStation, OrbitalHarrowTelemetry,
00094:     /// and SeasonalEventSystem). Owns all three systems, feeds the station from the
00095:     /// authoritative <see cref="WeatherSystem"/> forecast rolls, ticks the
00096:     /// orbital impact clock against <see cref="SkyLayerArmorSystem"/>, and
00097:     /// persists as ONE nested state inside the world section of the campaign
00098:     /// envelope.
00099:     ///
00100:     /// Engine-agnostic (Core). The host constructs it with real system
00101:     /// references and calls <see cref="TickDay"/> from the daily tick.
00102:     /// </summary>
00103:     public sealed class WeatherIntelligenceCoordinator
00104:     {
00105:         public WeatherStationSystem Station { get; }
00106:         public OrbitalHarrowTelemetrySystem Orbital { get; }
00107:         public SeasonalEventSystem Seasonal { get; }
00108:         public CloudSeedingSystem CloudSeeding { get; }
00109:
00110:         private readonly WeatherSystem _weather;
00111:         private readonly SkyLayerArmorSystem _armor;
00112:         private readonly ISeededRng _rng;
00113:         private readonly ILog _log;
00114:         private int _currentDay;
00115:
00116:         /// <summary>
00117:         /// Raised whenever the station forecast, orbital telemetry, or seasonal event changes,
00118:         /// so the host can mark the world section dirty and refresh panels.
00119:         /// </summary>
00120:         public event Action? OnIntelligenceChanged;
00121:
00122:         public WeatherIntelligenceCoordinator(
00123:             WeatherSystem weather,
00124:             SkyLayerArmorSystem armor,
00125:             ISeededRng rng,
00126:             ILog? log = null)
00127:         {
00128:             _weather = weather ?? throw new ArgumentNullException(nameof(weather));
00129:             _armor = armor ?? throw new ArgumentNullException(nameof(armor));
00130:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00131:             _log = log ?? NullLog.Instance;
00132:
00133:             int seed = _rng.Seed;
00134:             Station = new WeatherStationSystem(_weather, new SeededRng(seed), _log);
00135:             Orbital = new OrbitalHarrowTelemetrySystem(_armor, new SeededRng(unchecked(seed ^ 0x5A5A5A5A)), _log);
00136:             Seasonal = new SeasonalEventSystem(_log);
00137:             CloudSeeding = new CloudSeedingSystem(_weather, Station, null, new SeededRng(unchecked(seed ^ 0x3C3C3C3C)), _log);
00138:
00139:             Station.OnForecastUpdated += RaiseChanged;
00140:             Station.OnStationStateChanged += RaiseChanged;
00141:             Orbital.OnTelemetryChanged += RaiseChanged;
00142:             Orbital.OnImpactWarning += _ => RaiseChanged();
00143:             Orbital.OnImpactResolved += (_, _) => RaiseChanged();
00144:             Seasonal.OnStateChanged += RaiseChanged;
00145:             CloudSeeding.OnStateChanged += RaiseChanged;
00146:             CloudSeeding.OnCloudSeedingDeployed += _ => RaiseChanged();
00147:         }
00148:
00149:         // ── Daily tick ──────────────────────────────────────────────────────
00150:
00151:         /// <summary>
00152:         /// Advance the weather-intelligence cluster by one day. Regenerates
00153:         /// the station forecast (if operational), resolves pending orbital impacts,
00154:         /// and evaluates seasonal events.
00155:         /// </summary>
00156:         public void TickDay(int day)
00157:         {
00158:             _currentDay = day;
00159:             if (Station.IsOperational)
00160:                 Station.GenerateForecast(day);
00161:
00162:             Orbital.TickDay(day);
00163:
00164:             var season = _weather.GetSeasonForDay(day);
00165:             Seasonal.TickDay(day, season?.id ?? "window_first_thaw", new SeededRng(unchecked(_rng.Seed * 31 + day)));
00166:
00167:             CloudSeeding.TickDay(day);
00168:         }
00169:
00170:         // ── Read model ─────────────────────────────────────────────────────
00171:
00172:         public WeatherIntelligenceReadModel BuildReadModel()
00173:         {
00174:             var s = Station.State;
00175:             var o = Orbital.State;
00176:             var season = _weather.GetSeasonForDay(_currentDay);
00177:
00178:             var rm = new WeatherIntelligenceReadModel
00179:             {
00180:                 seasonId = season?.id ?? "window_first_thaw",
00181:                 seasonDisplayName = season?.displayName ?? "Ash Fall",
00182:                 stationInstalled = s.isInstalled,
00183:                 stationCalibrated = s.isCalibrated,
00184:                 stationOperational = Station.IsOperational,
00185:                 stationTier = Station.CurrentTier,
00186:                 stationTierName = Station.CurrentTier.ToString(),
00187:                 stationAccuracy = s.accuracy,
00188:                 stationDurability = s.durability,
00189:                 forecastHorizonDays = Station.EffectiveHorizonDays,
00190:                 lastForecastDay = s.lastForecastDay,
00191:                 telemetryActive = o.telemetryActive,
00192:                 hasPendingImpact = Orbital.HasPendingImpact,
00193:                 impactDay = o.nextImpactDay,
00194:                 warningLeadDays = o.warningLeadDays,
00195:                 daysUntilImpact = o.nextImpactDay > _currentDay ? o.nextImpactDay - _currentDay : 0,
00196:                 activeSalvageCount = o.activeSalvage.FindAll(x => !x.isClaimed).Count
00197:             };
00198:
00199:             foreach (var f in s.cachedForecast)
00200:                 rm.forecast.Add(f);
00201:
00202:             foreach (var sal in o.activeSalvage)
00203:                 rm.activeSalvage.Add(sal);
00204:
00205:             foreach (var evt in Seasonal.ActiveEvents)
00206:                 rm.activeSeasonalEvents.Add(evt);
00207:
00208:             // Derive expedition route-safety information from the forecast.
00209:             int safeDays = 0;
00210:             int bestDay = 0;
00211:             float bestConf = 0f;
00212:             foreach (var f in s.cachedForecast)
00213:             {
00214:                 if (f.isRouteSafe)
00215:                 {
00216:                     safeDays++;
00217:                     if (bestDay == 0 || f.confidence > bestConf)
00218:                     {
00219:                         bestDay = f.day;
00220:                         bestConf = f.confidence;
00221:                     }
00222:                 }
00223:             }
00224:             rm.routeSafeDays = safeDays;
00225:             rm.bestTravelDay = bestDay;
00226:             rm.bestTravelConfidence = bestConf;
00227:
00228:             // C1.4 — Crisis Prediction from Weather Station
00229:             rm.isStationCalibrated = s.isCalibrated;
00230:             if (Station.IsOperational && s.cachedForecast.Count > 0)
00231:             {
00232:                 foreach (var f in s.cachedForecast)
00233:                 {
00234:                     if (IsSevereWeather(f.weather) && f.day >= _currentDay)
00235:                     {
00236:                         rm.hasPredictedCrisis = true;
00237:                         rm.predictedCrisisDay = f.day;
00238:                         rm.daysUntilPredictedCrisis = Math.Max(0, f.day - _currentDay);
00239:                         rm.predictedWeatherKind = f.weather;
00240:                         rm.predictedCrisisEventId = $"crisis.weather.{f.weather.ToString().ToLowerInvariant()}";
00241:                         float calBonus = s.isCalibrated ? 0.15f : 0.0f;
00242:                         int dist = Math.Max(1, f.day - _currentDay);
00243:                         float distFactor = Math.Max(0.5f, 1.0f - (dist - 1) * 0.12f);
00244:                         rm.predictedCrisisConfidence = Math.Clamp((s.accuracy + calBonus) * distFactor * f.confidence, 0.35f, 0.98f);
00245:                         rm.crisisPreparationAdvice = GetCrisisAdvice(f.weather);
00246:                         rm.predictionSource = "weather_station";
00247:                         break;
00248:                     }
00249:                 }
00250:             }
00251:
00252:             // C1.5 — Cloud Seeding
00253:             rm.cloudSeedingInstalled = CloudSeeding.IsInstalled;
00254:             rm.cloudSeedingOnCooldown = CloudSeeding.IsOnCooldown;
00255:             rm.cloudSeedingCooldownDays = CloudSeeding.CooldownRemaining;
00256:             rm.cloudSeedingPartialProtectionActive = CloudSeeding.PartialProtectionActive;
00257:
00258:             rm.advisory = BuildAdvisory(rm);
00259:             return rm;
00260:         }
00261:
00262:         private static bool IsSevereWeather(WeatherKind kind)
00263:         {
00264:             return kind switch
00265:             {
00266:                 WeatherKind.FalloutStorm or
00267:                 WeatherKind.Blizzard or
00268:                 WeatherKind.GlassStorm or
00269:                 WeatherKind.RadHail or
00270:                 WeatherKind.EMPStorm or
00271:                 WeatherKind.AcidSnow or
00272:                 WeatherKind.BlackRain or
00273:                 WeatherKind.BioFog or
00274:                 WeatherKind.IceStorm => true,
00275:                 _ => false
00276:             };
00277:         }
00278:
00279:         private static string GetCrisisAdvice(WeatherKind kind)
00280:         {
00281:             return kind switch
00282:             {
00283:                 WeatherKind.GlassStorm or WeatherKind.RadHail => "Deploy cloud seeding or reinforce ceiling armor against kinetic impacts.",
00284:                 WeatherKind.FalloutStorm or WeatherKind.AcidSnow => "Seal outer blast doors and administer anti-rad medication.",
00285:                 WeatherKind.EMPStorm => "Disconnect delicate electronics and charge auxiliary battery banks.",
00286:                 WeatherKind.Blizzard or WeatherKind.IceStorm => "Stock furnace fuel and cancel outdoor surface expeditions.",
00287:                 _ => "Prepare shelter environmental seals and monitor broadcast frequencies."
00288:             };
00289:         }
00290:
00291:         private static string BuildAdvisory(WeatherIntelligenceReadModel rm)
00292:         {
00293:             var sb = new StringBuilder();
00294:             sb.Append($"Season: {rm.seasonDisplayName}. ");
00295:
00296:             if (!rm.stationOperational)
00297:             {
00298:                 sb.Append("Weather station offline/damaged — forecast unavailable. ");
00299:             }
00300:             else
00301:             {
00302:                 sb.Append($"Station tier: {rm.stationTierName} (accuracy {rm.stationAccuracy:P0}, {rm.forecastHorizonDays}d horizon). ");
00303:                 if (rm.routeSafeDays > 0)
00304:                     sb.Append($"Best travel window: day {rm.bestTravelDay} ({rm.bestTravelConfidence:P0} confidence). ");
00305:                 else
00306:                     sb.Append("No safe travel windows in forecast. ");
00307:             }
00308:
00309:             if (rm.hasPredictedCrisis)
00310:             {
00311:                 sb.Append($"CRISIS ALERT: {rm.predictedWeatherKind} on day {rm.predictedCrisisDay} ({rm.predictedCrisisConfidence:P0} confidence). {rm.crisisPreparationAdvice} ");
00312:             }
00313:
00314:             if (rm.telemetryActive)
00315:             {
00316:                 if (rm.hasPendingImpact)
00317:                     sb.Append($"Orbital impact in {rm.daysUntilImpact}d (grid warning active). ");
00318:                 else
00319:                     sb.Append("Orbital telemetry clear. ");
00320:             }
00321:
00322:             if (rm.activeSeasonalEvents.Count > 0)
00323:             {
00324:                 sb.Append($"Active seasonal hazards: {rm.activeSeasonalEvents.Count}. ");
00325:             }
00326:
00327:             return sb.ToString().Trim();
00328:         }
00329:
00330:         // ── Save / Load ─────────────────────────────────────────────────────
00331:
00332:         public WeatherIntelligenceSaveState CaptureState()
00333:         {
00334:             return new WeatherIntelligenceSaveState
00335:             {
00336:                 station = Station.CaptureState(),
00337:                 orbital = Orbital.CaptureState(),
00338:                 seasonal = Seasonal.CaptureState(),
00339:                 cloudSeeding = CloudSeeding.CaptureState()
00340:             };
00341:         }
00342:
00343:         public void RestoreState(WeatherIntelligenceSaveState? saved)
00344:         {
00345:             if (saved == null) return;
00346:             if (saved.station != null) Station.RestoreState(saved.station);
00347:             if (saved.orbital != null) Orbital.RestoreState(saved.orbital);
00348:             if (saved.seasonal != null) Seasonal.RestoreState(saved.seasonal);
00349:             if (saved.cloudSeeding != null) CloudSeeding.RestoreState(saved.cloudSeeding);
00350:             RaiseChanged();
00351:         }
00352:
00353:         private void RaiseChanged() => OnIntelligenceChanged?.Invoke();
00354:     }
00355: }
```

## `src/Host/WorldHostSession.cs` — 402 lines; 19,415 bytes; SHA-256 `b932e2d0b66a2c471eb95d222294abeb1f921424e2d1b157d41c53c32c68a816`
Declaration index:
- 00017: public sealed class WorldHostSession
- 00054: public string WildlifeSightingFor(string locationId)
- 00069: public string HomeSectorWildlifeStatus()
- 00114: public static WorldHostSession Create(string dataDir, ICampaignRngManager? campaignRng = null)
- 00199: private static void OverlayWildlifeMapGraph(
- 00242: public string FlavorTextForLocation(string locationId, string? weather = null)
- 00265: public void TickHours(float hours)
- 00272: public string ForceDemo(WeatherKind kind)
- 00278: public string StatusLine()
- 00287: public WorldWeatherState CaptureSave() => Weather.CaptureState();
- 00288: public void RestoreSave(WorldWeatherState state) => Weather.RestoreState(state);
- 00292: public string SetSkyArmorDemo(int gridX, string material, float thickness)
- 00307: public string ImpactDemo(int gridX, float energyMJ)
- 00313: public string SkyArmorStatusLine()
- 00320: private float AvgAttenuation()
- 00329: public SkyArmorSaveState CaptureSkyArmorSave() => SkyArmor.CaptureState();
- 00330: public void RestoreSkyArmorSave(SkyArmorSaveState state) => SkyArmor.RestoreState(state);
- 00334: public WeatherIntelligenceSaveState CaptureWeatherIntelligenceSave()
- 00337: public string InstallWeatherStationDemo(int day)
- 00345: public string CalibrateWeatherStationDemo(int day)
- 00353: public string ActivateOrbitalTelemetryDemo(int day)
- 00359: public string ScheduleOrbitalImpactDemo(int day, int gridX, float energyMj)
- 00365: public string WeatherIntelligenceStatusLine()
- 00375: internal static bool IsHazardWeather(WeatherKind kind)
- 00390: internal bool IsSevereWeather(WeatherKind kind)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Ashfall.Core;
00006: using Ashfall.Core.Random;
00007: using Ashfall.Core.Shelter;
00008: using Ashfall.Core.World;
00009:
00010: namespace AtomicWar.GodotApp
00011: {
00012:     /// <summary>
00013:     /// Thin Godot-host session for the World port (weather core). Loads the
00014:     /// season profile JSON, ticks the weather clock, persists state. No rules
00015:     /// here — hosts only wire and present.
00016:     /// </summary>
00017:     public sealed class WorldHostSession
00018:     : HostSessionBase{
00019:         public const int DemoSeed = 1234;
00020:
00021:         public WeatherSystem Weather { get; }
00022:         public SkyLayerArmorSystem SkyArmor { get; }
00023:         public WeatherIntelligenceCoordinator WeatherIntelligence { get; }
00024:         public LocationEvolutionSystem LocationEvolution { get; }
00025:         public WildlifeMigrationSystem Wildlife { get; }
00026:         public LandmarkDegradationSystem Landmarks { get; }
00027:         public WastelandMapSystem WastelandMap { get; }
00028:         public DamagedMapSystem? DamagedMap { get; private set; }
00029:         public SeasonProfileDef Profile { get; private set; }
00030:
00031:         /// <summary>Plan 48 weather gate catalog loaded from weather_route_gates.json.</summary>
00032:         public WeatherGateCatalog GateCatalog { get; private set; } = new WeatherGateCatalog();
00033:
00034:         /// <summary>Location atmosphere flavor texts loaded from environmental_atmosphere_expansion.json.</summary>
00035:         public AtmosphereTextSystem AtmosphereTexts { get; } = new AtmosphereTextSystem();
00036:
00037:         /// <summary>Environmental flavor texts loaded from environmental_texts_expansion_05.json.</summary>
00038:         public EnvironmentalTextSystem EnvironmentalTexts { get; } = new EnvironmentalTextSystem();
00039:
00040:         /// <summary>
00041:         /// Seed catalog for the evolving-world trio (loaded once in Create).
00042:         /// Null when no data dir was provided; hosts read the shelter sector
00043:         /// and scarcity goods from here.
00044:         /// </summary>
00045:         public EvolvingWorldSeedContainer? Seeds { get; private set; }
00046:
00047:         public string ShelterSectorId => EvolvingWorldSeeder.ShelterSectorId(Seeds);
00048:
00049:         /// <summary>
00050:         /// Plan 28 Phase 5 (28L) — coarse wildlife sighting for a location,
00051:         /// via its seed-bound sector. Discovery-gated: unknown ground reads
00052:         /// empty. "holding" | "passing" | "" (never a population count).
00053:         /// </summary>
00054:         public string WildlifeSightingFor(string locationId)
00055:         {
00056:             if (Wildlife == null || Seeds?.location_seeds == null) return string.Empty;
00057:             string? sector = null;
00058:             foreach (var seed in Seeds.location_seeds)
00059:                 if (seed != null && string.Equals(seed.location_id, locationId, StringComparison.Ordinal))
00060:                 { sector = seed.sector_id; break; }
00061:             if (string.IsNullOrEmpty(sector)) return string.Empty;
00062:
00063:             int pop = Wildlife.GetSectorPackPopulation(sector);
00064:             if (pop <= 0) return string.Empty;
00065:             return pop >= 8 ? "wildlife holding" : "wildlife passing";
00066:         }
00067:
00068:         /// <summary>28L overview contract — home-sector wildlife band, no counts.</summary>
00069:         public string HomeSectorWildlifeStatus()
00070:         {
00071:             if (Wildlife == null || string.IsNullOrEmpty(ShelterSectorId)) return string.Empty;
00072:             int pop = Wildlife.GetSectorPackPopulation(ShelterSectorId);
00073:             if (pop <= 0) return string.Empty;
00074:             return pop >= 8 ? "Wildlife: herds reported" : "Wildlife: movement reported";
00075:         }
00076:
00077:         public string LastEvent { get; private set; } = string.Empty;
00078:         /// <summary>C2 / Plan 20C — the bound weather-effects authority
00079:         /// (null when no valid data file; consumers fall back to legacy).</summary>
00080:         public WeatherEffectsCatalog? WeatherEffects { get; private set; }
00081:
00082:         public WorldHostSession(
00083:             WeatherSystem weather = null!,
00084:             SkyLayerArmorSystem skyArmor = null!,
00085:             LocationEvolutionSystem locationEvolution = null!,
00086:             WildlifeMigrationSystem wildlife = null!,
00087:             LandmarkDegradationSystem landmarks = null!,
00088:             WastelandMapSystem wastelandMap = null!,
00089:             DamagedMapSystem? damagedMap = null,
00090:             ICampaignRngManager? campaignRng = null)
00091:         {
00092:             int weatherSeed = campaignRng != null
00093:                 ? campaignRng.GetStream(CampaignStreamIds.Weather).DerivedBaseSeed
00094:                 : DemoSeed;
00095:             Weather = weather ?? new WeatherSystem();
00096:             SkyArmor = skyArmor ?? new SkyLayerArmorSystem();
00097:             WeatherIntelligence = new WeatherIntelligenceCoordinator(Weather, SkyArmor, new SeededRng(weatherSeed));
00098:             LocationEvolution = locationEvolution ?? new LocationEvolutionSystem();
00099:             Wildlife = wildlife ?? new WildlifeMigrationSystem();
00100:             Landmarks = landmarks ?? new LandmarkDegradationSystem();
00101:             WastelandMap = wastelandMap ?? WastelandMapCatalogLoader.CreateSystem(string.Empty);
00102:             DamagedMap = damagedMap;
00103:             Weather.OnWeatherChanged += kind =>
00104:             {
00105:                 LastEvent = $"Weather: {kind}";
00106:                 if (IsSevereWeather(kind))
00107:                     AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayWeatherAlert();
00108:                 RaiseStateChanged();
00109:             };
00110:             Weather.OnStateChanged += _ => RaiseStateChanged();
00111:             WeatherIntelligence.OnIntelligenceChanged += () => RaiseStateChanged();
00112:         }
00113:
00114:         public static WorldHostSession Create(string dataDir, ICampaignRngManager? campaignRng = null)
00115:         {
00116:             var mapSystem = !string.IsNullOrEmpty(dataDir)
00117:                 ? WastelandMapCatalogLoader.CreateSystem(dataDir)
00118:                 : null!;
00119:             var session = new WorldHostSession(wastelandMap: mapSystem, campaignRng: campaignRng);
00120:             if (!string.IsNullOrEmpty(dataDir))
00121:             {
00122:                 session.DamagedMap = DamagedMapCatalogLoader.CreateSystem(dataDir, session.WastelandMap);
00123:             }
00124:             var profile = !string.IsNullOrEmpty(dataDir)
00125:                 ? WeatherProfileLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
00126:                 : null;
00127:             if (profile != null)
00128:             {
00129:                 session.Profile = profile;
00130:                 int weatherSeed = campaignRng != null
00131:                     ? campaignRng.GetStream(CampaignStreamIds.Weather).DerivedBaseSeed
00132:                     : DemoSeed;
00133:                 session.Weather.BindProfile(profile, weatherSeed);
00134:                 // Plan 28: the same Plan 19 authority paces wildlife abundance.
00135:                 session.Wildlife.BindSeasonProfile(profile);
00136:             }
00137:             // C2 / Plan 20C (§36/§40/§41) — expose the bound weather-effects
00138:             // authority so consumer hosts (trapping, expeditions, caravans)
00139:             // sample the ONE table.
00140:             if (!string.IsNullOrEmpty(dataDir))
00141:             {
00142:                 var effects = WeatherEffectsCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00143:                 if (effects.LoadedCount > 0)
00144:                 {
00145:                     session.Weather.BindWeatherEffects(effects);
00146:                     session.WeatherEffects = effects;
00147:                 }
00148:             }
00149:             var env = WorldSaveStore.TryLoadEnvelope();
00150:             if (env != null)
00151:             {
00152:                 if (env.State != null) session.Weather.RestoreState(env.State);
00153:                 if (env.SkyArmor != null) session.RestoreSkyArmorSave(env.SkyArmor);
00154:                 if (env.WeatherIntelligence != null) session.WeatherIntelligence.RestoreState(env.WeatherIntelligence);
00155:                 if (env.LocationEvolution != null) session.LocationEvolution.RestoreState(env.LocationEvolution);
00156:                 if (env.Wildlife != null) session.Wildlife.RestoreState(env.Wildlife);
00157:                 if (env.Landmark != null) session.Landmarks.RestoreState(env.Landmark);
00158:                 session.LastEvent = "World state restored from save.";
00159:             }
00160:
00161:             // Evolving-world activation (task 122): load the seed authority,
00162:             // then seed — AFTER restore, and only into empty ledgers, so a
00163:             // restored save is never overwritten by the starting world.
00164:             session.Seeds = !string.IsNullOrEmpty(dataDir)
00165:                 ? EvolvingWorldCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
00166:                 : null;
00167:             EvolvingWorldSeeder.Seed(session.LocationEvolution, session.Wildlife, session.Landmarks, session.Seeds);
00168:
00169:             // Seasonal events activation (Plan 19)
00170:             var seasonalEvents = !string.IsNullOrEmpty(dataDir)
00171:                 ? SeasonalEventCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
00172:                 : null;
00173:             if (seasonalEvents != null && seasonalEvents.Count > 0)
00174:                 session.WeatherIntelligence.Seasonal.BindDefinitions(seasonalEvents);
00175:
00176:             // Atmosphere / environmental flavor catalogs — consumed by location
00177:             // presentation (expedition/map detail) via FlavorTextForLocation.
00178:             if (!string.IsNullOrEmpty(dataDir))
00179:             {
00180:                 var files = new FileSystemIO();
00181:                 var json = new SystemTextJsonSerializer();
00182:                 session.GateCatalog = WeatherGateCatalogLoader.LoadFromDirectory(dataDir, files, json);
00183:                 session.WeatherIntelligence.Station.GateCatalog = session.GateCatalog;
00184:                 AtmosphereCatalogLoader.LoadAndRegister(session.AtmosphereTexts, dataDir, files, json);
00185:                 EnvironmentalTextCatalogLoader.LoadAndRegister(session.EnvironmentalTexts, dataDir, files, json);
00186:             }
00187:
00188:             var mapSave = WastelandMapSaveStore.TryLoad();
00189:             if (mapSave != null)
00190:                 session.WastelandMap.RestoreState(mapSave);
00191:             OverlayWildlifeMapGraph(session.Wildlife, session.WastelandMap, session.Seeds);
00192:             return session;
00193:         }
00194:
00195:         /// <summary>
00196:         /// Plan 30C — packs also migrate across sectors that the wasteland map
00197:         /// connects through location seeds. Seed neighbors are kept.
00198:         /// </summary>
00199:         private static void OverlayWildlifeMapGraph(
00200:             WildlifeMigrationSystem wildlife,
00201:             WastelandMapSystem map,
00202:             EvolvingWorldSeedContainer? seeds)
00203:         {
00204:             if (wildlife == null || map == null || seeds?.location_seeds == null) return;
00205:             var locToSector = new Dictionary<string, string>(StringComparer.Ordinal);
00206:             for (int i = 0; i < seeds.location_seeds.Count; i++)
00207:             {
00208:                 var seed = seeds.location_seeds[i];
00209:                 if (seed == null || string.IsNullOrEmpty(seed.location_id) || string.IsNullOrEmpty(seed.sector_id))
00210:                     continue;
00211:                 locToSector[seed.location_id] = seed.sector_id;
00212:             }
00213:             if (locToSector.Count == 0) return;
00214:
00215:             var merged = new Dictionary<string, List<string>>(StringComparer.Ordinal);
00216:             foreach (var route in map.Routes)
00217:             {
00218:                 if (route == null) continue;
00219:                 if (!locToSector.TryGetValue(route.From, out string fromSector)) continue;
00220:                 if (!locToSector.TryGetValue(route.To, out string toSector)) continue;
00221:                 if (string.Equals(fromSector, toSector, StringComparison.Ordinal)) continue;
00222:                 if (!merged.TryGetValue(fromSector, out var neighbors))
00223:                 {
00224:                     neighbors = new List<string>();
00225:                     merged[fromSector] = neighbors;
00226:                 }
00227:                 if (!neighbors.Contains(toSector))
00228:                     neighbors.Add(toSector);
00229:             }
00230:             if (merged.Count == 0) return;
00231:
00232:             var links = new List<(string sectorId, List<string> neighbors)>(merged.Count);
00233:             foreach (var kv in merged)
00234:                 links.Add((kv.Key, kv.Value));
00235:             wildlife.MergeSectorAdjacency(links);
00236:         }
00237:
00238:         /// <summary>
00239:         /// Prefer atmosphere catalog text for a location; fall back to environmental texts.
00240:         /// Empty string when neither catalog has an entry (presentation must handle silence).
00241:         /// </summary>
00242:         public string FlavorTextForLocation(string locationId, string? weather = null)
00243:         {
00244:             if (string.IsNullOrEmpty(locationId)) return string.Empty;
00245:
00246:             if (!string.IsNullOrEmpty(weather))
00247:             {
00248:                 var atmWeather = AtmosphereTexts.GetTextForLocationAndWeather(locationId, weather);
00249:                 if (atmWeather != null && !string.IsNullOrEmpty(atmWeather.text))
00250:                     return atmWeather.text;
00251:             }
00252:
00253:             var atm = AtmosphereTexts.GetTextForLocation(locationId);
00254:             if (atm != null && !string.IsNullOrEmpty(atm.text))
00255:                 return atm.text;
00256:
00257:             var env = EnvironmentalTexts.GetTextForLocation(locationId);
00258:             if (env != null && !string.IsNullOrEmpty(env.text))
00259:                 return env.text;
00260:
00261:             return string.Empty;
00262:         }
00263:
00264:         // ── Production Runtime Actions ───────────────────────────────
00265:         public void TickHours(float hours)
00266:         {
00267:             Weather.Tick(hours);
00268:         }
00269:
00270:         // ── Demo actions ─────────────────────────────────────────────
00271:
00272:         public string ForceDemo(WeatherKind kind)
00273:         {
00274:             Weather.ForceWeather(kind);
00275:             return $"Weather forced to {kind}.";
00276:         }
00277:
00278:         public string StatusLine()
00279:         {
00280:             return $"Weather: {Weather.Current} · visibility {Weather.VisibilityFactor:P0} · " +
00281:                    $"outdoor rad {Weather.OutdoorRadModifier:0} · " +
00282:                    $"temp penalty {Weather.TemperaturePenaltyC(Weather.Current):0}°C";
00283:         }
00284:
00285:         // ── Save / Load ──────────────────────────────────────────────
00286:
00287:         public WorldWeatherState CaptureSave() => Weather.CaptureState();
00288:         public void RestoreSave(WorldWeatherState state) => Weather.RestoreState(state);
00289:
00290:         // ── Sky Layer Armor (Exp 11) ────────────────────────────────
00291:
00292:         public string SetSkyArmorDemo(int gridX, string material, float thickness)
00293:         {
00294:             var tier = material switch
00295:             {
00296:                 "dirt" => CeilingMaterialTier.Dirt,
00297:                 "wood" => CeilingMaterialTier.Wood,
00298:                 "concrete" => CeilingMaterialTier.ReinforcedConcrete,
00299:                 "lead" => CeilingMaterialTier.LeadSheeting,
00300:                 "tungsten" => CeilingMaterialTier.TungstenComposite,
00301:                 _ => CeilingMaterialTier.Dirt
00302:             };
00303:             SkyArmor.SetCellArmor(gridX, tier, thickness);
00304:             return $"Sky armor set at grid {gridX}: {tier} ({thickness}m). Attenuation: {SkyArmor.GetAttenuationFactor(gridX):F3}.";
00305:         }
00306:
00307:         public string ImpactDemo(int gridX, float energyMJ)
00308:         {
00309:             bool breached = SkyArmor.EvaluateKineticImpact(gridX, energyMJ, out float damage);
00310:             return breached ? $"BREACH at grid {gridX}! {damage:F1} MJ through." : $"Impact absorbed at grid {gridX}.";
00311:         }
00312:
00313:         public string SkyArmorStatusLine()
00314:         {
00315:             var save = SkyArmor.CaptureState();
00316:             if (save.cells.Count == 0) return "Sky armor: no cells plated";
00317:             return $"Sky armor: {save.cells.Count} cells · avg attenuation {AvgAttenuation():F3}";
00318:         }
00319:
00320:         private float AvgAttenuation()
00321:         {
00322:             var save = SkyArmor.CaptureState();
00323:             if (save.cells.Count == 0) return 1f;
00324:             float sum = 0f;
00325:             foreach (var c in save.cells) sum += SkyArmor.GetAttenuationFactor(c.gridX);
00326:             return sum / save.cells.Count;
00327:         }
00328:
00329:         public SkyArmorSaveState CaptureSkyArmorSave() => SkyArmor.CaptureState();
00330:         public void RestoreSkyArmorSave(SkyArmorSaveState state) => SkyArmor.RestoreState(state);
00331:
00332:         // ── Weather Intelligence (station + orbital telemetry) ────────────
00333:
00334:         public WeatherIntelligenceSaveState CaptureWeatherIntelligenceSave()
00335:             => WeatherIntelligence.CaptureState();
00336:
00337:         public string InstallWeatherStationDemo(int day)
00338:         {
00339:             var r = WeatherIntelligence.Station.Install(day);
00340:             return r.Status == ActionResult.StatusKind.Success
00341:                 ? $"Weather station installed on day {day}."
00342:                 : "Station already installed.";
00343:         }
00344:
00345:         public string CalibrateWeatherStationDemo(int day)
00346:         {
00347:             var r = WeatherIntelligence.Station.Calibrate(day);
00348:             return r.Status == ActionResult.StatusKind.Success
00349:                 ? $"Station calibrated (accuracy {WeatherIntelligence.Station.State.accuracy:P0})."
00350:                 : "Cannot calibrate — station not installed or already calibrated.";
00351:         }
00352:
00353:         public string ActivateOrbitalTelemetryDemo(int day)
00354:         {
00355:             WeatherIntelligence.Orbital.ActivateTelemetry(day);
00356:             return $"Orbital Harrow telemetry activated on day {day}.";
00357:         }
00358:
00359:         public string ScheduleOrbitalImpactDemo(int day, int gridX, float energyMj)
00360:         {
00361:             WeatherIntelligence.Orbital.ScheduleImpact(day, gridX, energyMj);
00362:             return $"Orbital impact scheduled: day {day}, grid {gridX}, {energyMj:F1} MJ. Warning lead: {WeatherIntelligence.Orbital.State.warningLeadDays}d.";
00363:         }
00364:
00365:         public string WeatherIntelligenceStatusLine()
00366:         {
00367:             var rm = WeatherIntelligence.BuildReadModel();
00368:             return rm.advisory;
00369:         }
00370:
00371:         /// <summary>
00372:         /// Hazard weather kinds that warrant an audio alert on transition.
00373:         /// Matches the Core-rollable hazard set: FalloutStorm, BlackRain, Blizzard.
00374:         /// </summary>
00375:         internal static bool IsHazardWeather(WeatherKind kind)
00376:         {
00377:             return kind == WeatherKind.FalloutStorm
00378:                 || kind == WeatherKind.BlackRain
00379:                 || kind == WeatherKind.Blizzard;
00380:         }
00381:
00382:         /// <summary>
00383:         /// C2 / Plan 20C (§43) — data-driven alert parity from the ONE effects
00384:         /// table: a transition cue fires for every SEVERE kind (rad ≥ 60,
00385:         /// visibility ≤ 0.5, or thermal ≤ −10 °C) — previously only three kinds
00386:         /// alerted while equally severe states (GlassStorm, RadHail, IceStorm)
00387:         /// stayed silent. Exactly-once: OnWeatherChanged fires on the
00388:         /// transition edge only. Unbound → the legacy hazard classification.
00389:         /// </summary>
00390:         internal bool IsSevereWeather(WeatherKind kind)
00391:         {
00392:             if (WeatherEffects != null && WeatherEffects.TryGetEffects(kind, out var fx)
00393:                 && fx != null)
00394:             {
00395:                 return fx.outdoor_rad_modifier >= 60f
00396:                     || fx.visibility_modifier <= 0.5f
00397:                     || fx.thermal_load_additive_c <= -10f;
00398:             }
00399:             return IsHazardWeather(kind);
00400:         }
00401:     }
00402: }
```

## `src/Main.FlagshipInstitutions.cs` — 579 lines; 26,298 bytes; SHA-256 `583894a805a235c894e1dfb161df6b1834511001e727b4f0480a25d2162cd433`
Declaration index:
- 00028: public partial class Main
- 00047: private InstitutionAssignmentLedger EnsureInstitutionLedger() =>
- 00057: private void EnsureFlagshipLifecycleRegistration()
- 00085: private OrbitalHarrowTelemetrySystem EnsureOrbitalHarrowTelemetry()
- 00093: private void MarkCulturalArchiveDirty() => _culturalArchiveDirty = true;
- 00094: private void MarkDiplomaticSummitDirty() => _diplomaticSummitDirty = true;
- 00095: private void MarkSkyDefenseDirty() => _skyDefenseDirty = true;
- 00096: private void MarkSanatoriumDirty() => _sanatoriumDirty = true;
- 00102: private CulturalArchiveVaultSystem EnsureCulturalArchive()
- 00139: public string RecordArchiveChronicle(
- 00166: private void OnMemorializedForArchiveChronicle(MemorialEntry entry)
- 00175: private DiplomaticSummitSystem EnsureDiplomaticSummit()
- 00203: private SkyDefenseBatterySystem EnsureSkyDefense()
- 00230: private PsychologicalSanatoriumSystem EnsureSanatorium()
- 00264: private void SetupCulturalArchive()
- 00273: private void SetupDiplomaticSummit()
- 00281: private void SetupSkyDefense()
- 00290: private void SetupSanatorium()
- 00299: private void SetupFlagshipInstitutions()
- 00311: private void SaveCulturalArchive()
- 00320: private void SaveDiplomaticSummit()
- 00329: private void SaveSkyDefense()
- 00338: private void SaveSanatorium()
- 00349: private void PersistFlagshipInstitutionsIfDirty()
- 00365: internal sealed class HostFactionStandingPort : IFactionStandingPort
- 00369: public float GetStanding(string factionId) =>
- 00371: public void AdjustStanding(string factionId, float delta, string reasonCode) =>
- 00375: internal sealed class HostSurvivorSkillsPort : ISurvivorSkillsPort
- 00379: public bool HasSkill(string survivorId, string skillId) =>
- 00392: internal sealed class HostSurvivorConditionPort : ISurvivorConditionPort
- 00406: public bool HasCondition(string survivorId, string conditionId)
- 00439: public int GetAcuteStressPermille(string survivorId)
- 00449: public void ApplyAcuteStressReduction(string survivorId, int permille)
- 00460: public void ApplyRecoveryProgress(string survivorId, int progress)
- 00469: public void SuppressReversibleCondition(string survivorId, string conditionId)
- 00489: public int GetRelationshipTrust(string therapistId, string patientId) => 50;
- 00496: private void BindCultureCrossDomainEvents()
- 00530: private void BindSanatoriumCrossDomainEvents()
- 00547: private sealed class FlagshipInstitutionsDayOwner : IDayAdvanceOwner
- 00552: public void CapturePreDaySnapshot(int day) { }
- 00554: public void TickDay(int day, List<DayStateChangeEvent> events)
- 00573: private void RegisterFlagshipInstitutionsOwner()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Campaign;
00008: using Ashfall.Core.Catalogs;
00009: using Ashfall.Core.Memorial;
00010: using Ashfall.Core.Culture;
00011: using Ashfall.Core.Diplomacy;
00012: using Ashfall.Core.Lifecycle;
00013: using Ashfall.Core.Survivors;
00014: using Ashfall.Core.Institutions;
00015: using Ashfall.Core.Sanatorium;
00016: using Ashfall.Core.Shelter;
00017: using Ashfall.Core.SkyDefense;
00018:
00019: namespace AtomicWar.GodotApp
00020: {
00021:     /// <summary>
00022:     /// Flagship institutions (Tasks 5-8) host wiring: composition, catalog
00023:     /// loading, save-section triads (SetupXxx/SaveXxx), daily tick owner and
00024:     /// availability claim re-registration after restore (plan §10 step 11).
00025:     /// Presentation-only rules respected: gameplay lives in Core; this partial
00026:     /// only constructs, binds, ticks, persists.
00027:     /// </summary>
00028:     public partial class Main
00029:     {
00030:         private const int FlagshipMasterSeed = 42;
00031:
00032:         private InstitutionAssignmentLedger? _institutionLedger;
00033:         private CulturalArchiveVaultSystem? _culturalArchive;
00034:         private DiplomaticSummitSystem? _diplomaticSummit;
00035:         private SkyDefenseBatterySystem? _skyDefense;
00036:         private PsychologicalSanatoriumSystem? _sanatorium;
00037:
00038:         private bool _culturalArchiveDirty;
00039:         private bool _diplomaticSummitDirty;
00040:         private bool _skyDefenseDirty;
00041:         private bool _sanatoriumDirty;
00042:
00043:         private static string FlagshipDataDir =>
00044:             System.IO.Path.Combine(
00045:                 ProjectSettings.GlobalizePath("res://"), "Assets", "StreamingAssets", "Data");
00046:
00047:         private InstitutionAssignmentLedger EnsureInstitutionLedger() =>
00048:             _institutionLedger ??= new InstitutionAssignmentLedger();
00049:
00050:         private bool _flagshipLifecycleRegistered;
00051:
00052:         /// <summary>
00053:         /// Registers the flagship sessions with the lifecycle registry so a
00054:         /// load-game reset nulls them (fields re-Ensure from disk on the next
00055:         /// Setup call). Idempotent; safe to call from every Ensure.
00056:         /// </summary>
00057:         private void EnsureFlagshipLifecycleRegistration()
00058:         {
00059:             if (_flagshipLifecycleRegistered) return;
00060:             if (_lifecycleRegistry == null) return;
00061:             _flagshipLifecycleRegistered = true;
00062:             _lifecycleRegistry.Register(new DelegateSessionParticipant(
00063:                 "flagship_institutions",
00064:                 dependsOn: new[] { "inventory" },
00065:                 saveSectionKey: null,
00066:                 onReset: () =>
00067:                 {
00068:                     _institutionLedger = null;
00069:                     _culturalArchive = null;
00070:                     _diplomaticSummit = null;
00071:                     _skyDefense = null;
00072:                     _sanatorium = null;
00073:                     _culturalArchiveDirty = false;
00074:                     _diplomaticSummitDirty = false;
00075:                     _skyDefenseDirty = false;
00076:                     _sanatoriumDirty = false;
00077:                 }));
00078:         }
00079:
00080:         /// <summary>
00081:         /// The live campaign telemetry (world-owned). Sky defense consumes the
00082:         /// SAME instance the radio/warning UIs consume — never a parallel one
00083:         /// (plan §9.7).
00084:         /// </summary>
00085:         private OrbitalHarrowTelemetrySystem EnsureOrbitalHarrowTelemetry()
00086:         {
00087:             var telemetry = _world?.WeatherIntelligence.Orbital;
00088:             if (telemetry == null)
00089:                 telemetry = new OrbitalHarrowTelemetrySystem(new SkyLayerArmorSystem(), new SeededRng(FlagshipMasterSeed));
00090:             return telemetry;
00091:         }
00092:
00093:         private void MarkCulturalArchiveDirty() => _culturalArchiveDirty = true;
00094:         private void MarkDiplomaticSummitDirty() => _diplomaticSummitDirty = true;
00095:         private void MarkSkyDefenseDirty() => _skyDefenseDirty = true;
00096:         private void MarkSanatoriumDirty() => _sanatoriumDirty = true;
00097:
00098:         // -----------------------------------------------------------------
00099:         // Composition (idempotent; each system constructed exactly once)
00100:         // -----------------------------------------------------------------
00101:
00102:         private CulturalArchiveVaultSystem EnsureCulturalArchive()
00103:         {
00104:             EnsureFlagshipLifecycleRegistration();
00105:             if (_culturalArchive != null) return _culturalArchive;
00106:             var fileIO = new FileSystemIO();
00107:             var json = new SystemTextJsonSerializer();
00108:
00109:             // No authoritative shelter-humidity authority exists (plan §5.9
00110:             // "if available") — the deep vault reads as a dry environment.
00111:             _culturalArchive = new CulturalArchiveVaultSystem(
00112:                 _inventory.Inventory,
00113:                 availability: EnsureInstitutionLedger());
00114:             _culturalArchive.LoadTomeCatalog(
00115:                 CulturalArchiveTomeCatalogLoader.Load(FlagshipDataDir, fileIO, json));
00116:             _culturalArchive.OnDocumentRestored += _ => MarkCulturalArchiveDirty();
00117:             _culturalArchive.OnMicroficheCreated += _ => MarkCulturalArchiveDirty();
00118:             _culturalArchive.OnTomeTranscribed += _ => MarkCulturalArchiveDirty();
00119:             _culturalArchive.OnDocumentLost += _ => MarkCulturalArchiveDirty();
00120:             _culturalArchive.OnArchiveRecordingCreated += (_, _) => MarkCulturalArchiveDirty();
00121:             _culturalArchive.OnSalonStarted += _ => MarkCulturalArchiveDirty();
00122:             _culturalArchive.OnSalonEnded += _ => MarkCulturalArchiveDirty();
00123:             _culturalArchive.OnChronicleEntryAdded += _ => MarkCulturalArchiveDirty();
00124:             _culturalArchive.OnDocumentationChanged += () => MarkCulturalArchiveDirty();
00125:             BindCultureCrossDomainEvents();
00126:
00127:             var saved = CulturalArchiveSaveStore.TryLoad();
00128:             if (saved != null)
00129:                 _culturalArchive.RestoreState(saved);
00130:             return _culturalArchive;
00131:         }
00132:
00133:         /// <summary>
00134:         /// Plans 178/190 — creation command into the culture vault (DEBT-178-CREATION-TO-VAULT).
00135:         /// The vault owns append + dedup; this only routes a real campaign
00136:         /// milestone into it and surfaces a journal line so the entry is
00137:         /// player-reachable. No second archive save, no art/lore ledger.
00138:         /// </summary>
00139:         public string RecordArchiveChronicle(
00140:             string eventType,
00141:             string summaryKey,
00142:             IReadOnlyList<string>? participants = null,
00143:             string authorId = "")
00144:         {
00145:             var vault = EnsureCulturalArchive();
00146:             if (vault == null) return "The culture vault is unavailable.";
00147:
00148:             var result = vault.TryRecordChronicleEntry(eventType, _simDay, summaryKey, participants, authorId);
00149:             if (result.Status == ActionResult.StatusKind.Success)
00150:             {
00151:                 _journal?.TryAddRawEntry(
00152:                     $"archive_chronicle:{summaryKey}",
00153:                     $"Entered in the vault chronicle: {summaryKey}.",
00154:                     null!,
00155:                     _simDay);
00156:             }
00157:             return result.Status == ActionResult.StatusKind.Success
00158:                 ? "Chronicle entry recorded."
00159:                 : $"Chronicle entry not recorded ({result.FailureCode}).";
00160:         }
00161:
00162:         /// <summary>
00163:         /// Death is the campaign's most significant archival event: the name
00164:         /// enters the vault chronicle beside the memorial wall projection.
00165:         /// </summary>
00166:         private void OnMemorializedForArchiveChronicle(MemorialEntry entry)
00167:         {
00168:             if (entry == null || string.IsNullOrEmpty(entry.SurvivorId)) return;
00169:             RecordArchiveChronicle(
00170:                 Ashfall.Core.Culture.ArchiveChronicleMilestones.Memorial,
00171:                 Ashfall.Core.Culture.ArchiveChronicleMilestones.MemorialKey(entry.SurvivorId),
00172:                 new[] { entry.SurvivorId });
00173:         }
00174:
00175:         private DiplomaticSummitSystem EnsureDiplomaticSummit()
00176:         {
00177:             EnsureFlagshipLifecycleRegistration();
00178:             if (_diplomaticSummit != null) return _diplomaticSummit;
00179:             var fileIO = new FileSystemIO();
00180:             var json = new SystemTextJsonSerializer();
00181:
00182:             _standingPort ??= new HostFactionStandingPort(this);
00183:             _diplomaticSummit = new DiplomaticSummitSystem(
00184:                 FlagshipMasterSeed,
00185:                 inventory: _inventory.Inventory,
00186:                 availability: EnsureInstitutionLedger(),
00187:                 standing: _standingPort);
00188:             _diplomaticSummit.LoadTreatyCatalog(
00189:                 DiplomaticTreatyCatalogLoader.Load(FlagshipDataDir, fileIO, json));
00190:             _diplomaticSummit.OnSummitScheduled += _ => MarkDiplomaticSummitDirty();
00191:             _diplomaticSummit.OnTreatyRatified += _ => MarkDiplomaticSummitDirty();
00192:             _diplomaticSummit.OnTreatyViolationRecorded += _ => MarkDiplomaticSummitDirty();
00193:             _diplomaticSummit.OnTreatyEnded += (_, _) => MarkDiplomaticSummitDirty();
00194:             _diplomaticSummit.OnGuaranteeExchanged += _ => MarkDiplomaticSummitDirty();
00195:             _diplomaticSummit.OnGuaranteeReleased += _ => MarkDiplomaticSummitDirty();
00196:
00197:             var saved = DiplomaticSummitSaveStore.TryLoad();
00198:             if (saved != null)
00199:                 _diplomaticSummit.RestoreState(saved);
00200:             return _diplomaticSummit;
00201:         }
00202:
00203:         private SkyDefenseBatterySystem EnsureSkyDefense()
00204:         {
00205:             EnsureFlagshipLifecycleRegistration();
00206:             if (_skyDefense != null) return _skyDefense;
00207:             var fileIO = new FileSystemIO();
00208:             var json = new SystemTextJsonSerializer();
00209:
00210:             _skyDefense = new SkyDefenseBatterySystem(
00211:                 FlagshipMasterSeed,
00212:                 inventory: _inventory.Inventory,
00213:                 telemetry: EnsureOrbitalHarrowTelemetry(),
00214:                 availability: EnsureInstitutionLedger());
00215:             _skyDefense.LoadOrdnanceCatalog(
00216:                 SkyDefenseOrdnanceCatalogLoader.Load(FlagshipDataDir, fileIO, json));
00217:             _skyDefense.OnOrbitalTrackAcquired += _ => MarkSkyDefenseDirty();
00218:             _skyDefense.OnVolleyFired += (_, _, _) => MarkSkyDefenseDirty();
00219:             _skyDefense.OnInterceptResolved += (_, _, _, _) => MarkSkyDefenseDirty();
00220:             _skyDefense.OnServiced += _ => MarkSkyDefenseDirty();
00221:             _skyDefense.OnMaintenanceDue += _ => MarkSkyDefenseDirty();
00222:
00223:             var saved = SkyDefenseBatterySaveStore.TryLoad();
00224:             if (saved != null)
00225:                 _skyDefense.RestoreState(saved);
00226:             _skyDefense.EnsureDefaultTurret();
00227:             return _skyDefense;
00228:         }
00229:
00230:         private PsychologicalSanatoriumSystem EnsureSanatorium()
00231:         {
00232:             EnsureFlagshipLifecycleRegistration();
00233:             if (_sanatorium != null) return _sanatorium;
00234:             var fileIO = new FileSystemIO();
00235:             var json = new SystemTextJsonSerializer();
00236:
00237:             _skillsPort ??= new HostSurvivorSkillsPort(this);
00238:             _conditionPort ??= new HostSurvivorConditionPort(this);
00239:             _sanatorium = new PsychologicalSanatoriumSystem(
00240:                 FlagshipMasterSeed,
00241:                 inventory: _inventory.Inventory,
00242:                 availability: EnsureInstitutionLedger(),
00243:                 skills: _skillsPort,
00244:                 conditions: _conditionPort);
00245:             _sanatorium.LoadTherapyCatalog(
00246:                 PsychologicalTherapyCatalogLoader.Load(FlagshipDataDir, fileIO, json));
00247:             _sanatorium.OnPatientAdmitted += _ => MarkSanatoriumDirty();
00248:             _sanatorium.OnTherapyStarted += (_, _) => MarkSanatoriumDirty();
00249:             _sanatorium.OnTherapyCompleted += (_, _) => MarkSanatoriumDirty();
00250:             _sanatorium.OnPatientRelapsed += (_, _, _) => MarkSanatoriumDirty();
00251:             _sanatorium.OnPatientDischarged += _ => MarkSanatoriumDirty();
00252:             BindSanatoriumCrossDomainEvents();
00253:
00254:             var saved = PsychologicalSanatoriumSaveStore.TryLoad();
00255:             if (saved != null)
00256:                 _sanatorium.RestoreState(saved);
00257:             return _sanatorium;
00258:         }
00259:
00260:         // -----------------------------------------------------------------
00261:         // Setup triad (restore-order chain) + claim re-registration
00262:         // -----------------------------------------------------------------
00263:
00264:         private void SetupCulturalArchive()
00265:         {
00266:             var culturalArchive = EnsureCulturalArchive();
00267:             foreach (var doc in culturalArchive.Documents)
00268:                 if (!string.IsNullOrEmpty(doc.active_scholar_id))
00269:                     EnsureInstitutionLedger().TryClaim(
00270:                         doc.active_scholar_id, CulturalArchiveVaultSystem.InstitutionId, "scholar");
00271:         }
00272:
00273:         private void SetupDiplomaticSummit()
00274:         {
00275:             var diplomaticSummit = EnsureDiplomaticSummit();
00276:             foreach (var g in diplomaticSummit.Guarantees.Where(g => g.status == "exchanged"))
00277:                 EnsureInstitutionLedger().TryClaim(
00278:                     g.survivor_id, DiplomaticSummitSystem.InstitutionId, "guarantee");
00279:         }
00280:
00281:         private void SetupSkyDefense()
00282:         {
00283:             var skyDefense = EnsureSkyDefense();
00284:             foreach (var turret in skyDefense.Turrets)
00285:                 foreach (var crew in turret.assigned_crew_ids)
00286:                     EnsureInstitutionLedger().TryClaim(
00287:                         crew, SkyDefenseBatterySystem.InstitutionId, "gunner");
00288:         }
00289:
00290:         private void SetupSanatorium()
00291:         {
00292:             var sanatorium = EnsureSanatorium();
00293:             foreach (var p in sanatorium.Patients.Where(p => p.status == "admitted"))
00294:                 EnsureInstitutionLedger().TryClaim(
00295:                     p.survivor_id, PsychologicalSanatoriumSystem.InstitutionId, "patient");
00296:         }
00297:
00298:         /// <summary>All four institutions in dependency order (after world/telemetry bases).</summary>
00299:         private void SetupFlagshipInstitutions()
00300:         {
00301:             SetupSkyDefense();
00302:             SetupCulturalArchive();
00303:             SetupDiplomaticSummit();
00304:             SetupSanatorium();
00305:         }
00306:
00307:         // -----------------------------------------------------------------
00308:         // Save triad
00309:         // -----------------------------------------------------------------
00310:
00311:         private void SaveCulturalArchive()
00312:         {
00313:             if (_culturalArchive == null) return;
00314:             if (!CaptureSection("cultural_archives",
00315:                     CulturalArchiveSaveStore.TryCapturePersisted(_culturalArchive.CaptureState())))
00316:                 return;
00317:             _culturalArchiveDirty = false;
00318:         }
00319:
00320:         private void SaveDiplomaticSummit()
00321:         {
00322:             if (_diplomaticSummit == null) return;
00323:             if (!CaptureSection("diplomatic_summits",
00324:                     DiplomaticSummitSaveStore.TryCapturePersisted(_diplomaticSummit.CaptureState())))
00325:                 return;
00326:             _diplomaticSummitDirty = false;
00327:         }
00328:
00329:         private void SaveSkyDefense()
00330:         {
00331:             if (_skyDefense == null) return;
00332:             if (!CaptureSection("sky_defense_battery",
00333:                     SkyDefenseBatterySaveStore.TryCapturePersisted(_skyDefense.CaptureState())))
00334:                 return;
00335:             _skyDefenseDirty = false;
00336:         }
00337:
00338:         private void SaveSanatorium()
00339:         {
00340:             if (_sanatorium == null) return;
00341:             if (!CaptureSection("psychological_sanatorium",
00342:                     PsychologicalSanatoriumSaveStore.TryCapturePersisted(_sanatorium.CaptureState())))
00343:                 return;
00344:             _sanatoriumDirty = false;
00345:         }
00346:
00347:         // Composite orchestration only; each institution owns its registered
00348:         // save section through the SaveXxx methods above.
00349:         private void PersistFlagshipInstitutionsIfDirty()
00350:         {
00351:             if (_culturalArchiveDirty) SaveCulturalArchive();
00352:             if (_diplomaticSummitDirty) SaveDiplomaticSummit();
00353:             if (_skyDefenseDirty) SaveSkyDefense();
00354:             if (_sanatoriumDirty) SaveSanatorium();
00355:         }
00356:
00357:         // -----------------------------------------------------------------
00358:         // Port adapters (canonical authorities; null-safe before their setups)
00359:         // -----------------------------------------------------------------
00360:
00361:         private HostFactionStandingPort? _standingPort;
00362:         private HostSurvivorSkillsPort? _skillsPort;
00363:         private HostSurvivorConditionPort? _conditionPort;
00364:
00365:         internal sealed class HostFactionStandingPort : IFactionStandingPort
00366:         {
00367:             private readonly Main _m;
00368:             public HostFactionStandingPort(Main m) => _m = m;
00369:             public float GetStanding(string factionId) =>
00370:                 _m._yearOfAsh?.FactionWar.GetStanding(factionId) ?? 0f;
00371:             public void AdjustStanding(string factionId, float delta, string reasonCode) =>
00372:                 _m._yearOfAsh?.FactionWar.ModifyStanding(factionId, (int)Math.Round(delta));
00373:         }
00374:
00375:         internal sealed class HostSurvivorSkillsPort : ISurvivorSkillsPort
00376:         {
00377:             private readonly Main _m;
00378:             public HostSurvivorSkillsPort(Main m) => _m = m;
00379:             public bool HasSkill(string survivorId, string skillId) =>
00380:                 _m.EnsureSharedSkillProgression().HasActiveSkill(survivorId, skillId);
00381:         }
00382:
00383:         /// <summary>
00384:         /// Maps authored condition ids onto the canonical Phase-0 trauma
00385:         /// surfaces (plan §9.9): hypervigilance ← CombatTraumaSystem,
00386:         /// flashback ← SomaticFlashbackSystem, guilt-insomnia ←
00387:         /// GuiltInsomniaSystem. Siege paranoia reads as extreme
00388:         /// hypervigilance. Relapse (negative reduction) deliberately does not
00389:         /// re-escalate canonical surfaces — no canonical re-escalation API
00390:         /// exists; the sanatorium's own risk ledger tracks it.
00391:         /// </summary>
00392:         internal sealed class HostSurvivorConditionPort : ISurvivorConditionPort
00393:         {
00394:             private readonly Main _m;
00395:             public HostSurvivorConditionPort(Main m) => _m = m;
00396:
00397:             private Phase0HostSession? P0
00398:             {
00399:                 get
00400:                 {
00401:                     if (_m._phase0 == null) _m.SetupPhase0();
00402:                     return _m._phase0;
00403:                 }
00404:             }
00405:
00406:             public bool HasCondition(string survivorId, string conditionId)
00407:             {
00408:                 // Plan 164: breakdown-arc conditions live in the arc system;
00409:                 // the port composes them with the Phase0 trauma conditions.
00410:                 if (!string.IsNullOrEmpty(conditionId)
00411:                     && conditionId.StartsWith("arc_", System.StringComparison.Ordinal))
00412:                 {
00413:                     return _m._psychologyArcs?.System.HasArc(survivorId, conditionId) == true;
00414:                 }
00415:                 var p0 = P0;
00416:                 if (p0 == null || string.IsNullOrEmpty(survivorId)) return false;
00417:                 switch (conditionId)
00418:                 {
00419:                     case "condition_combat_ptsd":
00420:                         return p0.CombatTrauma.IsTracked(survivorId)
00421:                             && p0.CombatTrauma.GetHypervigilanceLevel(survivorId) >= 0.25f;
00422:                     case "condition_chronic_hypervigilance":
00423:                         return p0.CombatTrauma.GetHypervigilanceLevel(survivorId) >= 0.4f;
00424:                     case "condition_paranoid_psychosis":
00425:                         return p0.CombatTrauma.GetHypervigilanceLevel(survivorId) >= 0.6f;
00426:                     case "condition_flash_blindness_shock":
00427:                         return p0.Flashbacks.HasActiveFlashback(survivorId)
00428:                             || p0.Flashbacks.GetSusceptibility(survivorId) >= 0.2f;
00429:                     case "condition_severe_survivor_guilt":
00430:                         return p0.Guilt.GetGuiltSourceCount(survivorId) > 0
00431:                             && p0.Guilt.GetInsomniaSeverity(survivorId) >= 0.3f;
00432:                     case "condition_guilt_insomnia_loop":
00433:                         return p0.Guilt.GetInsomniaSeverity(survivorId) >= 0.4f;
00434:                     default:
00435:                         return false;
00436:                 }
00437:             }
00438:
00439:             public int GetAcuteStressPermille(string survivorId)
00440:             {
00441:                 var p0 = P0;
00442:                 if (p0 == null) return 0;
00443:                 float hv = p0.CombatTrauma.GetHypervigilanceLevel(survivorId);
00444:                 float fb = p0.Flashbacks.GetSusceptibility(survivorId);
00445:                 float gi = p0.Guilt.GetInsomniaSeverity(survivorId);
00446:                 return (int)Math.Clamp(Math.Max(hv, Math.Max(fb, gi)) * 1000f, 0f, 1000f);
00447:             }
00448:
00449:             public void ApplyAcuteStressReduction(string survivorId, int permille)
00450:             {
00451:                 if (permille <= 0) return;
00452:                 float fraction = Math.Clamp(permille / 1000f, 0f, 1f);
00453:                 var p0 = P0;
00454:                 if (p0 == null) return;
00455:                 p0.CombatTrauma.ApplyTherapyRelief(survivorId, fraction);
00456:                 p0.Flashbacks.ReduceSusceptibility(survivorId, fraction);
00457:                 p0.Guilt.ApplyTherapyRelief(survivorId, fraction);
00458:             }
00459:
00460:             public void ApplyRecoveryProgress(string survivorId, int progress)
00461:             {
00462:                 // Recovery progress is tracked inside the sanatorium's patient
00463:                 // state; the canonical surface is relieved through
00464:                 // ApplyAcuteStressReduction at outcome time. Plan 164: active
00465:                 // breakdown arcs additionally advance their arc recovery here.
00466:                 _m._psychologyArcs?.System.ApplyTreatmentProgress(survivorId, progress);
00467:             }
00468:
00469:             public void SuppressReversibleCondition(string survivorId, string conditionId)
00470:             {
00471:                 var p0 = P0;
00472:                 if (p0 == null) return;
00473:                 switch (conditionId)
00474:                 {
00475:                     case "condition_flash_blindness_shock":
00476:                         p0.Flashbacks.ReduceSusceptibility(survivorId, 1f);
00477:                         break;
00478:                     case "condition_chronic_hypervigilance":
00479:                     case "condition_combat_ptsd":
00480:                         p0.CombatTrauma.ApplyTherapyRelief(survivorId, 1f);
00481:                         break;
00482:                     case "condition_guilt_insomnia_loop":
00483:                     case "condition_severe_survivor_guilt":
00484:                         p0.Guilt.ApplyTherapyRelief(survivorId, 1f);
00485:                         break;
00486:                 }
00487:             }
00488:
00489:             public int GetRelationshipTrust(string therapistId, string patientId) => 50;
00490:         }
00491:
00492:         // -----------------------------------------------------------------
00493:         // Cross-domain event bindings (bound once inside the Ensure methods)
00494:         // -----------------------------------------------------------------
00495:
00496:         private void BindCultureCrossDomainEvents()
00497:         {
00498:             // Salon morale → canonical needs authority (single consumer).
00499:             _culturalArchive!.OnSalonMoraleTick += delta =>
00500:             {
00501:                 if (_survivors == null) return;
00502:                 foreach (var sv in _survivors.RosterState)
00503:                 {
00504:                     if (sv.Health <= 0f) continue;
00505:                     _survivors.Needs.Modify(sv, NeedKind.Morale, (float)delta);
00506:                 }
00507:             };
00508:
00509:             // Archive disc cut → vinyl media catalog (single consumer; media
00510:             // playback morale stays owned by VinylMoraleSystem).
00511:             _culturalArchive!.OnArchiveRecordingCreated += (_, def) =>
00512:             {
00513:                 if (_vinylMorale == null) SetupVinylMorale();
00514:                 var system = _vinylMorale?.System;
00515:                 if (system == null) return;
00516:                 system.MergeRecord(new VinylRecordDefinition
00517:                 {
00518:                     record_id = def.record_id,
00519:                     display_name = def.display_name,
00520:                     genre = def.genre,
00521:                     morale_daily_bonus = def.morale_daily_bonus,
00522:                     flashback_suppression = def.flashback_suppression,
00523:                     audio_cue_id = def.audio_cue_id,
00524:                     description = def.description,
00525:                 });
00526:                 system.AcquireRecord(def.record_id);
00527:             };
00528:         }
00529:
00530:         private void BindSanatoriumCrossDomainEvents()
00531:         {
00532:             // Dream transcription / testimony (§9.2): one completion → one
00533:             // archive oral-history disc; the culture system's duplicate guard
00534:             // makes this idempotent across re-fires.
00535:             _sanatorium!.OnTherapeuticJournalCompleted += (survivorId, _) =>
00536:             {
00537:                 int day = _core?.Clock.Day ?? 0;
00538:                 _culturalArchive?.TryCutArchiveDisc(
00539:                     $"archive_disc_dream_{survivorId}", "oral_history", survivorId, day);
00540:             };
00541:         }
00542:
00543:         // -----------------------------------------------------------------
00544:         // Daily tick owner (phase 5 — after survivors/medical, before final)
00545:         // -----------------------------------------------------------------
00546:
00547:         private sealed class FlagshipInstitutionsDayOwner : IDayAdvanceOwner
00548:         {
00549:             private readonly Main _m;
00550:             public FlagshipInstitutionsDayOwner(Main m) => _m = m;
00551:
00552:             public void CapturePreDaySnapshot(int day) { }
00553:
00554:             public void TickDay(int day, List<DayStateChangeEvent> events)
00555:             {
00556:                 _m.SetupSkyDefense();
00557:                 _m.SetupCulturalArchive();
00558:                 _m.SetupDiplomaticSummit();
00559:                 _m.SetupSanatorium();
00560:
00561:                 _m._skyDefense!.TickDay(day);
00562:                 _m._culturalArchive!.TickDay(day);
00563:                 _m._diplomaticSummit!.TickDay(day);
00564:                 _m._sanatorium!.TickDay(day);
00565:
00566:                 _m.PersistFlagshipInstitutionsIfDirty();
00567:                 events.Add(new DayStateChangeEvent("flagship_institutions_ticked",
00568:                     "flagship_institutions", null, null, day));
00569:             }
00570:         }
00571:
00572:         /// <summary>Called from RegisterProductionCampaignOwners (phase 5).</summary>
00573:         private void RegisterFlagshipInstitutionsOwner()
00574:         {
00575:             if (_campaignDay == null) return;
00576:             _campaignDay.Register("flagship_institutions", new FlagshipInstitutionsDayOwner(this), phase: 5);
00577:         }
00578:     }
00579: }
```

## `src/Main.World.cs` — 950 lines; 42,412 bytes; SHA-256 `4f75bdf8f2f5c5022d744a331f9da271f16d7ab47d1bc3b9605b8a3a8fd95261`
Declaration index:
- 00033: public partial class Main : Control
- 00061: private void OnGreenhousePlantClicked()
- 00074: private void HandleGreenhouseAction(string action, int plotIndex)
- 00149: private void OnGreenhouseTickClicked()
- 00160: private void FlushWorldIfDirty()
- 00165: private void FlushCraftingIfDirty()
- 00170: private void SetupWorld()
- 00191: private void BindJournalWorldProducerIfReady()
- 00202: private void OnJournalWorldNodeDiscovered(string locationId)
- 00207: private void SetupWeatherSonde()
- 00219: private void SaveWorld()
- 00244: private void WireRelicRestorationDeltas(WorkshopReverseEngineeringSystem workshop)
- 00276: private void SetupCrafting()
- 00346: private void SyncCraftingStationsFromShelter()
- 00414: private void SaveCrafting()
- 00424: private void OnCraftingStartClicked()
- 00430: private void OnCraftingFinishClicked()
- 00436: private void SetupStartingLevel()
- 00457: private void SaveStartingLevel()
- 00467: private void SetupPowerGrid()
- 00508: private void WireSurgeAdapters()
- 00538: private void SavePowerGrid()
- 00558: private void TickPowerGrid(int day)
- 00565: private void OpenPowerGrid()
- 00666: private void CloseOpeningProtocolModal()
- 00671: private void SetupGreenhouse()
- 00699: private void SaveGreenhouse()
- 00709: private void CloseGreenhousePanel()
- 00713: private void CloseDeconAirlockPanel() { _deconAirlockPanel.Visible = false; }
- 00714: private void CloseGeodeticSurveyPanel() { _geodeticSurveyPanel.Visible = false; }
- 00715: private void CloseKineticStoragePanel() { _kineticStoragePanel.Visible = false; }
- 00716: private void CloseChemicalReconPanel() { _chemicalReconPanel.Visible = false; }
- 00717: private void CloseChemWarfareDefensePanel() { _chemWarfareDefensePanel?.Visible = false; }
- 00718: private void CloseCommsArrayTransceiverPanel() { _commsArrayTransceiverPanel?.Visible = false; }
- 00719: private void CloseCeremonyFestivalPanel() { _ceremonyFestivalPanel?.Visible = false; }
- 00720: private void CloseRoboticsWorkshopPanel() { _roboticsWorkshopPanel?.Visible = false; }
- 00721: private void CloseSurvivorDowntimePanel() { _survivorDowntimePanel?.Visible = false; }
- 00722: private void CloseWinterFreezePanel() { _winterFreezePanel?.Visible = false; }
- 00723: private void CloseFungiCultivationPanel() { _fungiCultivationBedPanel.Visible = false; }
- 00724: private void CloseBioFermentationPanel() { _bioFermentationPanel.Visible = false; }
- 00725: private void ClosePlasticPyrolysisPanel() { _plasticPyrolysisPanel.Visible = false; }
- 00726: private void CloseCargoAirdropPanel() { _cargoAirdropPanel.Visible = false; }
- 00728: private void CloseCraftingPanel()
- 00733: private void CloseWorkshopPanel()
- 00738: private void CloseRadioIntelligencePanel()
- 00743: private void CloseShelterSocialPanel()
- 00748: private void CloseSubterraneanOperationsPanel()
- 00753: private void ClosePharmaLabPanel()
- 00758: private void CloseWeatherPanel()
- 00763: private void CloseWeatherDetailPanel()
- 00768: private void CloseWeatherForecastPanel()
- 00774: private void HandleDeconAirlockAction(string action, string param = "")
- 00808: private void HandleGeodeticSurveyAction(string action, string param = "")
- 00855: private void HandleKineticStorageAction(string action, string param = "")
- 00890: private void HandleChemicalReconAction(string action, string param = "")
- 00933: private void HandleGeothermalAction(string action, string param = "")
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
00014: using Ashfall.Core.Greenhouse;
00015: using Ashfall.Core.Inventory;
00016: using Ashfall.Core.Shelter;
00017: using Ashfall.Core.Journal;
00018: using Ashfall.Core.Muster;
00019: using Ashfall.Core.YearOfAsh;
00020: using Ashfall.Core.Radio;
00021: using Ashfall.Core.Survivors;
00022: using AtomicWar.GodotApp.Economy;
00023: using AtomicWar.GodotApp.YearOfAsh;
00024: using AtomicWar.GodotApp.Muster;
00025: using AtomicWar.GodotApp.Dose;
00026: using AtomicWar.GodotApp.UtilityAI;
00027: using AtomicWar.GodotApp.Radio;
00028: using AtomicWar.GodotApp.Audio;
00029: using AtomicWar.GodotApp.UI;
00030:
00031: namespace AtomicWar.GodotApp
00032: {
00033:     public partial class Main : Control
00034:     {
00035:         // ── World / Shelter fields (refactored: campaign/day split to Main.Campaign) ──
00036:         private Ashfall.Core.Medical.MedicalWardSystem _medicalWard = null!;
00037:         private bool _medicalWardDirty;
00038:         private StartingLevelHostSession _startingLevel = null!;
00039:         private bool _startingLevelDirty;
00040:         private OpeningProtocolModal _openingProtocolModal = null!;
00041:         private PowerGridHostSession _powerGrid = null!;
00042:         private bool _powerGridDirty;
00043:         // C2[6] 23B: player-initiated load sheds accumulated during the day, drained
00044:         // by the power day owner into a `power_shed_player` attribution event.
00045:         private readonly List<string> _pendingPlayerSheds = new List<string>();
00046:         private GreenhouseHostSession _greenhouse = null!;
00047:         private AtomicWar.GodotApp.UI.DeconAirlockPanel _deconAirlockPanel = null!;
00048:         private AtomicWar.GodotApp.UI.GeodeticSurveyPanel _geodeticSurveyPanel = null!;
00049:         private AtomicWar.GodotApp.UI.KineticStoragePanel _kineticStoragePanel = null!;
00050:         private AtomicWar.GodotApp.UI.ChemicalReconPanel _chemicalReconPanel = null!;
00051:         private bool _deconAirlockBound;
00052:         private bool _geodeticSurveyBound;
00053:         private bool _kineticStorageBound;
00054:         private bool _chemicalReconBound;
00055:         private GreenhousePanel _greenhousePanel = null!;
00056:         private bool _greenhouseDirty;
00057:         private WorldHostSession _world = null!;
00058:         private bool _worldDirty;
00059:         private WeatherHostSession _weatherSondeHost = null!;
00060:
00061:         private void OnGreenhousePlantClicked()
00062:         {
00063:             SetupGreenhouse();
00064:             SetupExpansions();
00065:             int day = _core != null ? _core.Clock.Day : _simDay;
00066:             // Single growth authority: player GreenhouseHostSession (shared into hub).
00067:             _greenhouse.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, day);
00068:             _greenhouse.Water(0, 60f, tainted: false);
00069:             _statusLabel.Text = "Plot 0 planted (seed_tuber) and watered on day " + day + ". The glass holds its heat.";
00070:             RefreshExpansionsStatus();
00071:         }
00072:
00073:         /// <summary>Item 10: routes greenhouse panel action buttons through GreenhouseHostSession.</summary>
00074:         private void HandleGreenhouseAction(string action, int plotIndex)
00075:         {
00076:             if (_greenhouse == null || plotIndex < 0) return;
00077:             SetupInventory();
00078:             int day = _core != null ? _core.Clock.Day : _simDay;
00079:
00080:             // Plan 22 GAP action routing: parameters encoded in action string
00081:             // so the panel event signature (string, int) stays stable.
00082:             string baseAction = action;
00083:             string? param = null;
00084:             int colon = action.IndexOf(':');
00085:             if (colon > 0)
00086:             {
00087:                 baseAction = action.Substring(0, colon);
00088:                 param = action.Substring(colon + 1);
00089:             }
00090:
00091:             switch (baseAction)
00092:             {
00093:                 case "plant":
00094:                     _greenhouse.Plant(plotIndex, param ?? GreenhouseExpansionCatalog.Items.SeedTuber, day);
00095:                     break;
00096:                 case "water":
00097:                 {
00098:                     // Panel emits "water:25:clean", "water:50:clean", "water:50:tainted"
00099:                     // (and legacy "water:tainted" / "water:25").
00100:                     float units = 50f;
00101:                     bool tainted = false;
00102:                     if (param != null)
00103:                     {
00104:                         string[] parts = param.Split(':');
00105:                         if (parts.Length >= 1
00106:                             && float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
00107:                         {
00108:                             units = parsed;
00109:                         }
00110:                         string quality = parts.Length >= 2 ? parts[parts.Length - 1] : parts[0];
00111:                         tainted = string.Equals(quality, "tainted", StringComparison.OrdinalIgnoreCase);
00112:                     }
00113:                     _greenhouse.Water(plotIndex, Math.Max(1f, units), tainted);
00114:                     break;
00115:                 }
00116:                 case "clear":
00117:                     _greenhouse.Clear(plotIndex);
00118:                     break;
00119:                 case "treat":
00120:                     _greenhouse.TreatBlight(plotIndex);
00121:                     break;
00122:                 case "harvest":
00123:                     _greenhouse.Harvest(plotIndex);
00124:                     break;
00125:                 case "dose_nutrients":
00126:                     _greenhouse.ApplyNutrients(plotIndex);
00127:                     break;
00128:                 case "apiary_inspect":
00129:                     _greenhouse.InspectHive("hive_01", day);
00130:                     break;
00131:                 case "apiary_feed":
00132:                     _greenhouse.FeedHive("hive_01", 0.5f);
00133:                     break;
00134:                 case "apiary_harvest":
00135:                     _greenhouse.HarvestHoney("hive_01");
00136:                     break;
00137:                 case "apiary_install":
00138:                     _greenhouse.InstallHive("hive_01", "bay_orchard", day);
00139:                     break;
00140:                 default:
00141:                     GD.PrintErr($"[Ashfall] unhandled greenhouse action: {action}");
00142:                     break;
00143:             }
00144:             if (_greenhouseDirty) SaveGreenhouse();
00145:             _statusLabel.Text = _greenhouse.LastEvent;
00146:             _greenhousePanel?.RefreshView();
00147:         }
00148:
00149:         private void OnGreenhouseTickClicked()
00150:         {
00151:             SetupGreenhouse();
00152:             SetupExpansions();
00153:             int day = _core != null ? _core.Clock.Day : _simDay;
00154:             // Single growth authority — do not also TickGreenhouse on a hub twin.
00155:             _greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0.04f);
00156:             _statusLabel.Text = "Greenhouse day ticked (day " + day + "). " + _expansions.GreenhouseLine();
00157:             RefreshExpansionsStatus();
00158:         }
00159:
00160:         private void FlushWorldIfDirty()
00161:         {
00162:             if (_worldDirty) SaveWorld();
00163:         }
00164:
00165:         private void FlushCraftingIfDirty()
00166:         {
00167:             if (_craftingDirty) SaveCrafting();
00168:         }
00169:
00170:         private void SetupWorld()
00171:         {
00172:             if (_world != null) return;
00173:             SetupCampaignDay();
00174:             _world = WorldHostSession.Create(_dataDir, _campaignDay.Rng);
00175:             BindJournalWorldProducerIfReady();
00176:             WireSurgeAdapters();
00177:             _world.StateChanged += () =>
00178:             {
00179:                 _worldDirty = true;
00180:                 _weatherPanel?.RefreshView();
00181:                 _shelterPanel?.RefreshView();
00182:                 if (_state == GameState.Playing) UpdateHud();
00183:             };
00184:             if (_expeditions != null)
00185:                 _expeditions.WastelandMap = _world.WastelandMap;
00186:             GD.Print("[Ashfall Godot] World host ready.");
00187:         }
00188:
00189:         private bool _journalWorldProducerBound;
00190:
00191:         private void BindJournalWorldProducerIfReady()
00192:         {
00193:             if (_journalWorldProducerBound
00194:                 || _journal == null
00195:                 || _world?.WastelandMap == null)
00196:                 return;
00197:
00198:             _world.WastelandMap.OnNodeDiscovered += OnJournalWorldNodeDiscovered;
00199:             _journalWorldProducerBound = true;
00200:         }
00201:
00202:         private void OnJournalWorldNodeDiscovered(string locationId)
00203:         {
00204:             _journal?.TryAddAuthoredEntry(locationId);
00205:         }
00206:
00207:         private void SetupWeatherSonde()
00208:         {
00209:             if (_weatherSondeHost != null) return;
00210:             SetupWorld();
00211:             SetupInventory();
00212:             var catalog = Ashfall.Core.World.AtmosphericSoundingCatalogLoader.Load(
00213:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00214:             _weatherSondeHost = new WeatherHostSession(_world?.Weather);
00215:             _weatherSondeHost.SetupSoundingCatalog(catalog?.altitude_bands, catalog?.payloads);
00216:             _weatherSondeHost.BindRecoveryInventory(_inventory?.Inventory);
00217:         }
00218:
00219:         private void SaveWorld()
00220:         {
00221:             if (_world == null) return;
00222:             if (CaptureSection("world", WorldSaveStore.TryCapturePersisted(
00223:                 _world!.CaptureSave()!,
00224:                 _world!.CaptureSkyArmorSave()!,
00225:                 _world!.CaptureWeatherIntelligenceSave()!,
00226:                 _world!.LocationEvolution?.CaptureState()!,
00227:                 _world!.Wildlife?.CaptureState()!,
00228:                 _world!.Landmarks?.CaptureState()!)))
00229:             {
00230:                 _worldDirty = false;
00231:                 GD.Print("[Ashfall Godot] World save written.");
00232:             }
00233:         }
00234:
00235:         private bool _relicDeltaRoutingWired;
00236:
00237:         /// <summary>
00238:         /// Plan 87 follow-up: route relic restoration completion deltas to the
00239:         /// real campaign authorities — shelter-wide morale through the survivors'
00240:         /// needs system, and the restoration world flag through the campaign
00241:         /// consequence ledger. The Core system emits each delta exactly once per
00242:         /// relic (guarded by completedRelicIds); this routing is idempotent.
00243:         /// </summary>
00244:         private void WireRelicRestorationDeltas(WorkshopReverseEngineeringSystem workshop)
00245:         {
00246:             if (_relicDeltaRoutingWired || workshop == null) return;
00247:             _relicDeltaRoutingWired = true;
00248:
00249:             workshop.OnActionCompleted += result =>
00250:             {
00251:                 if (!result.IsSuccess) return;
00252:
00253:                 if (result.Deltas.TryGetValue("morale_bonus", out var morale) && morale > 0 && _survivors != null)
00254:                 {
00255:                     var roster = _survivors.RosterState;
00256:                     for (int i = 0; i < roster.Count; i++)
00257:                     {
00258:                         var s = roster[i];
00259:                         if (s != null && s.IsAliveState)
00260:                             _survivors.Needs.Modify(s, NeedKind.Morale, (float)morale);
00261:                     }
00262:                 }
00263:
00264:                 foreach (var kvp in result.Deltas)
00265:                 {
00266:                     if (kvp.Key != null && kvp.Key.StartsWith("flag_", System.StringComparison.Ordinal) && kvp.Value > 0)
00267:                         _consequenceLedger.Set(
00268:                             kvp.Key.Substring("flag_".Length),
00269:                             WorkshopReverseEngineeringSystem.SystemId,
00270:                             result.MessageKey,
00271:                             _core != null ? _core.Clock.Day : _simDay);
00272:                 }
00273:             };
00274:         }
00275:
00276:         private void SetupCrafting()
00277:         {
00278:             if (_crafting != null) return;
00279:             SetupInventory();
00280:             _sharedResearch = EnsureSharedResearch();
00281:             _crafting = CraftingHostSession.Create(_dataDir, _inventory.Inventory, _sharedResearch);
00282:
00283:             _crafting.Workshop.BindSkillEvaluator(survivorId =>
00284:             {
00285:                 if (string.IsNullOrEmpty(survivorId)) return 1.0f;
00286:                 var def = _survivors?.Roster?.FindDefinition(survivorId);
00287:                 if (def == null) return 1.0f;
00288:                 float skill = 1.0f;
00289:                 if (def.traitIds != null && def.traitIds.Contains("skill_crafting_expert")) skill += 0.5f;
00290:                 if (def.traitIds != null && def.traitIds.Contains("skill_scavenge_efficiency")) skill += 0.3f;
00291:                 return skill;
00292:             });
00293:
00294:             // Plan 24B A2 — the workshop's craft-time leg resolves through the
00295:             // ONE shared worker-productivity contract (campaign skill authority
00296:             // + fitness + duty-hour overwork), composed with — never replacing
00297:             // — the Phase0 penalty slot and the legacy trait evaluator. Workers
00298:             // with no workshop-sense level resolve null ⇒ exact legacy speed.
00299:             // Bounded [0.8, 1.3]× so degraded crafters slow and skilled ones
00300:             // speed up without extreme swings.
00301:             _crafting.Engine.SetCrafterProductivityTimeMultiplier(crafterId =>
00302:             {
00303:                 if (string.IsNullOrEmpty(crafterId)) return 1f;
00304:                 var verdict = EnsureWorkerProductivityContract()
00305:                     .Resolve(crafterId, "skill_workshop_sense");
00306:                 if (verdict == null) return 1f;
00307:                 return MathfCompat.Clamp(verdict.YieldModifierPermille / 1000f, 0.8f, 1.3f);
00308:             });
00309:
00310:             WireRelicRestorationDeltas(_crafting.Workshop);
00311:
00312:             _crafting.PharmaLab.BindSkillEvaluator(chemistId =>
00313:             {
00314:                 if (string.IsNullOrEmpty(chemistId)) return 1.0f;
00315:                 var def = _survivors?.Roster?.FindDefinition(chemistId);
00316:                 if (def == null) return 1.0f;
00317:                 float skill = 1.0f;
00318:                 if (def.traitIds != null && def.traitIds.Contains("skill_medical_doctor")) skill += 0.5f;
00319:                 if (def.traitIds != null && def.traitIds.Contains("skill_chemistry_specialist")) skill += 0.4f;
00320:                 return skill;
00321:             });
00322:
00323:             _crafting.PharmaLab.OnDependencyRisk += risk =>
00324:             {
00325:                 if (!string.IsNullOrEmpty(_crafting.PharmaLab.State.assignedChemistId) && _chemicalDependency != null)
00326:                 {
00327:                     _chemicalDependency.System.OnSubstanceConsumed(
00328:                         _crafting.PharmaLab.State.assignedChemistId,
00329:                         _crafting.PharmaLab.State.currentRecipeId,
00330:                         Ashfall.Core.Medical.ChemicalDependencyKind.Opioid);
00331:                 }
00332:             };
00333:
00334:             var save = CraftingSaveStore.TryLoad();
00335:             if (save != null)
00336:             {
00337:                 _crafting.RestoreSave(save);
00338:             }
00339:
00340:             SyncCraftingStationsFromShelter();
00341:
00342:             _crafting.StateChanged += () => _craftingDirty = true;
00343:             GD.Print("[Ashfall Godot] Crafting host ready.");
00344:         }
00345:
00346:         private void SyncCraftingStationsFromShelter()
00347:         {
00348:             if (_crafting == null) return;
00349:
00350:             // WT-INT-01: Bridge shelter workshop infrastructure to CraftingSystem "workbench" station.
00351:             // Authority: shelter room "room_workshop" in _shelterAssignment or machine health in _shelterWorkshop,
00352:             // combined with power availability in _powerGrid.
00353:             bool roomExists = false;
00354:             if (_shelterAssignment?.System?.Rooms != null)
00355:             {
00356:                 for (int i = 0; i < _shelterAssignment.System.Rooms.Count; i++)
00357:                 {
00358:                     var r = _shelterAssignment.System.Rooms[i];
00359:                     if (r != null && (string.Equals(r.RoomId, "room_workshop", StringComparison.Ordinal) ||
00360:                                       string.Equals(r.RoomId, "room_workshop_heavy", StringComparison.Ordinal) ||
00361:                                       string.Equals(r.RoomId, "room_workshop_precision", StringComparison.Ordinal)))
00362:                     {
00363:                         roomExists = true;
00364:                         break;
00365:                     }
00366:                 }
00367:             }
00368:
00369:             bool hasMachine = false;
00370:             float machineCondition = 100f;
00371:             if (_shelterWorkshop?.State?.machines != null && _shelterWorkshop.State.machines.Count > 0)
00372:             {
00373:                 if (_shelterWorkshop.State.machines.TryGetValue("room_workshop", out var machine))
00374:                 {
00375:                     hasMachine = true;
00376:                     machineCondition = Math.Clamp(machine.ToolingHealth * 100f, 0f, 100f);
00377:                 }
00378:                 else
00379:                 {
00380:                     foreach (var m in _shelterWorkshop.State.machines.Values)
00381:                     {
00382:                         if (m != null && m.RoomId != null && m.RoomId.StartsWith("room_workshop", StringComparison.Ordinal))
00383:                         {
00384:                             hasMachine = true;
00385:                             machineCondition = Math.Min(machineCondition, Math.Clamp(m.ToolingHealth * 100f, 0f, 100f));
00386:                         }
00387:                     }
00388:                 }
00389:             }
00390:
00391:             bool isPowered = _powerGrid?.System == null || _powerGrid.System.IsRoomPowered("room_workshop");
00392:
00393:             bool workshopOperational = (roomExists || hasMachine) && isPowered && machineCondition > 0f;
00394:             float workbenchCondition = (roomExists || hasMachine) && isPowered ? machineCondition : 0f;
00395:
00396:             if (workshopOperational)
00397:             {
00398:                 _crafting.SyncStations(new[]
00399:                 {
00400:                     new Ashfall.Core.Crafting.CraftingStation
00401:                     {
00402:                         id = "workbench",
00403:                         displayName = "Civilian Workbench",
00404:                         condition = workbenchCondition
00405:                     }
00406:                 });
00407:             }
00408:             else
00409:             {
00410:                 _crafting.RemoveStation("workbench");
00411:             }
00412:         }
00413:
00414:         private void SaveCrafting()
00415:         {
00416:             if (_crafting == null) return;
00417:             if (CaptureSection("crafting", CraftingSaveStore.TryCapturePersisted(_crafting.CaptureSave())))
00418:             {
00419:                 _craftingDirty = false;
00420:                 GD.Print("[Ashfall Godot] Crafting save written.");
00421:             }
00422:         }
00423:
00424:         private void OnCraftingStartClicked()
00425:         {
00426:             SetupCrafting();
00427:             _statusLabel.Text = _crafting.Start("recipe_bandage") + "\n" + _crafting.CraftingLine();
00428:         }
00429:
00430:         private void OnCraftingFinishClicked()
00431:         {
00432:             SetupCrafting();
00433:             _statusLabel.Text = _crafting.CompleteAll(1f) + "\n" + _crafting.CraftingLine();
00434:         }
00435:
00436:         private void SetupStartingLevel()
00437:         {
00438:             if (_startingLevel != null) return;
00439:             _startingLevel = StartingLevelHostSession.Create();
00440:             _startingLevel.StateChanged += () =>
00441:             {
00442:                 _startingLevelDirty = true;
00443:                 _openingProtocolModal?.RefreshView();
00444:                 if (_state == GameState.Playing) UpdateHud();
00445:             };
00446:             if (_inventory != null)
00447:             {
00448:                 _startingLevel.BindMaintenance(
00449:                     _inventory.Inventory,
00450:                     knowledgeId => EnsureSharedResearch().HasCapability(knowledgeId));
00451:             }
00452:             if (_openingProtocolModal != null)
00453:                 _openingProtocolModal.Bind(_startingLevel);
00454:             GD.Print("[Ashfall Godot] Starting level host ready.");
00455:         }
00456:
00457:         private void SaveStartingLevel()
00458:         {
00459:             if (_startingLevel == null) return;
00460:             if (CaptureSection("starting_level", StartingLevelSaveStore.TryCapturePersisted(_startingLevel.CaptureState())))
00461:             {
00462:                 _startingLevelDirty = false;
00463:                 GD.Print("[Ashfall Godot] Starting level save written.");
00464:             }
00465:         }
00466:
00467:         private void SetupPowerGrid()
00468:         {
00469:             if (_powerGrid != null) return;
00470:             SetupCampaignDay();
00471:             var rng = _campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Shelter).Rng;
00472:             _powerGrid = PowerGridHostSession.CreateDefault(rng, _dataDir);
00473:             _powerGrid.TryLoad();
00474:             _powerGrid.OnStateChanged += () =>
00475:             {
00476:                 _powerGridDirty = true;
00477:                 SyncCraftingStationsFromShelter();
00478:             };
00479:             // B5–B8 expansion (§27): source degradation/failure events ride the
00480:             // typed power events into the journal authority — presentation
00481:             // only; the grid owns the facts.
00482:             _powerGrid.System.OnPowerChanged += evt =>
00483:             {
00484:                 string? entry = evt.Kind switch
00485:                 {
00486:                     PowerGridEventKind.GeneratorWorn =>
00487:                         "MAINTENANCE: The generator is wearing out — output derated. A machine_oil service restores full rating.",
00488:                     PowerGridEventKind.FuelStarved =>
00489:                         "FUEL WARNING: The generator tank ran dry mid-day — running at partial output.",
00490:                     _ => null
00491:                 };
00492:                 if (entry != null)
00493:                     _journal?.TryAddRawEntry(
00494:                         evt.Kind == PowerGridEventKind.GeneratorWorn ? "generator_worn" : "generator_fuel_starved",
00495:                         entry, null!, _simDay);
00496:             };
00497:             WireSurgeAdapters();
00498:         }
00499:
00500:         private bool _surgeAdaptersWired;
00501:
00502:         /// <summary>
00503:         /// SHELTER_EMP_MEDICAL_POWER (G4): EMP/orbital events feed the grid.
00504:         /// Deterministic + bounded: <see cref="PowerGridSystem.ApplySurgeDay"/>
00505:         /// dedups per day. Called from both setup paths (order-independent);
00506:         /// wiring is idempotent.
00507:         /// </summary>
00508:         private void WireSurgeAdapters()
00509:         {
00510:             if (_surgeAdaptersWired || _powerGrid == null || _world == null) return;
00511:             var weather = _world.Weather;
00512:             if (weather == null) return;
00513:             _surgeAdaptersWired = true;
00514:
00515:             weather.OnWeatherChanged += kind =>
00516:             {
00517:                 if (kind == WeatherKind.EMPStorm)
00518:                     _powerGrid?.System.ApplySurgeDay(_simDay, _powerGrid.System.EmpStormSeverity);
00519:
00520:                 // Plan 135 — every weather front is a cascade candidate. The
00521:                 // cascade session re-derives severity from the canonical
00522:                 // weather-effects table and skips mechanically neutral fronts,
00523:                 // so this notification never fabricates pressure.
00524:                 OnWeatherFrontArrived(kind, _simDay);
00525:             };
00526:
00527:             var orbital = _world.WeatherIntelligence?.Orbital;
00528:             if (orbital != null)
00529:             {
00530:                 orbital.OnImpactDetailed += rep =>
00531:                 {
00532:                     if (rep != null)
00533:                         _powerGrid?.System.ApplySurgeDay(rep.Day, rep.PowerGridDisruption / 100f);
00534:                 };
00535:             }
00536:         }
00537:
00538:         private void SavePowerGrid()
00539:         {
00540:             if (_powerGrid == null) return;
00541:
00542:             var save = new PowerGridSave
00543:             {
00544:                 simDay = _powerGrid.System.State.SimDay,
00545:                 Rooms = new List<PowerGridRoomSave>(),
00546:                 State = _powerGrid.System.State.Capture()
00547:             };
00548:             foreach (var room in _powerGrid.System.Rooms)
00549:                 save.Rooms.Add(PowerGridSaveCodec.FromRoom(room));
00550:
00551:             if (CaptureSection("power_grid", PowerGridSaveStore.TryCapturePersisted(save)))
00552:             {
00553:                 _powerGridDirty = false;
00554:                 _powerGrid.ClearDirty();
00555:             }
00556:         }
00557:
00558:         private void TickPowerGrid(int day)
00559:         {
00560:             SetupPowerGrid();
00561:             _powerGrid.TickDay(day);
00562:             if (_powerGridDirty) SavePowerGrid();
00563:         }
00564:
00565:         private void OpenPowerGrid()
00566:         {
00567:             SetupPowerGrid();
00568:             if (_powerGridPanel == null)
00569:             {
00570:                 _powerGridPanel = new PowerGridPanel();
00571:                 _powerGridPanel.OnRoomToggled += id =>
00572:                 {
00573:                     bool wasPowered = _powerGrid.System.IsRoomPowered(id);
00574:                     if (!_powerGrid.ToggleBreaker(id))
00575:                         return;
00576:                     ObserveSigil("power.breaker_toggled");
00577:                     // C2[6] 23B: if the player just took a served room offline, record
00578:                     // it as a player shed so the briefing can say who did it.
00579:                     if (wasPowered && !_powerGrid.System.IsRoomPowered(id))
00580:                         _pendingPlayerSheds.Add(id);
00581:                 };
00582:                 _powerGridPanel.OnPriorityChanged += (id, p) => _powerGrid.SetPriority(id, p);
00583:                 _powerGridPanel.OnFuelAdded += u => _powerGrid.AddFuel(u);
00584:                 // C2[6] 23B: overload/surge trips need an explicit, costly reset — the
00585:                 // canonical machine_oil service is consumed once (preview/consume/commit
00586:                 // discipline, same as generator service).
00587:                 _powerGridPanel.OnBreakerResetRequested += id =>
00588:                 {
00589:                     if (!_powerGrid.System.IsRoomTripped(id))
00590:                         return;
00591:                     var inv = _inventory.Inventory;
00592:                     if (inv.CountById(PowerGridSystem.GeneratorMaintenanceItemId) < 1)
00593:                     {
00594:                         ObserveSigil("power.breaker_reset_missing_item");
00595:                         return;
00596:                     }
00597:                     if (!_powerGrid.ClearTripped(id))
00598:                         return;
00599:                     inv.TryConsumeById(PowerGridSystem.GeneratorMaintenanceItemId, 1);
00600:                     _journal?.TryAddRawEntry("power_breaker_reset",
00601:                         $"MAINTENANCE: Breaker for {id} reset after an overload trip (machine_oil consumed).", null!, _simDay);
00602:                     ObserveSigil("power.breaker_reset");
00603:                 };
00604:                 // B5–B8 Phase 2: battery-bank install — preview check, canonical
00605:                 // item consumed once, then authoritative commit; blocked installs
00606:                 // mutate nothing and say why.
00607:                 _powerGridPanel.OnBatteryBankInstallRequested += () =>
00608:                 {
00609:                     var inv = _inventory.Inventory;
00610:                     if (inv.CountById(PowerGridSystem.BatteryBankItemId) < 1)
00611:                     {
00612:                         ObserveSigil("power.battery_bank_missing_item");
00613:                         return;
00614:                     }
00615:                     if (!_powerGrid.TryInstallBatteryBank(out var reason))
00616:                     {
00617:                         ObserveSigil("power.battery_bank_blocked_" + reason);
00618:                         return;
00619:                     }
00620:                     inv.TryConsumeById(PowerGridSystem.BatteryBankItemId, 1);
00621:                     ObserveSigil("power.battery_bank_installed");
00622:                 };
00623:                 // B5–B8 Phase 5: generator service — canonical machine_oil,
00624:                 // same preview/consume/commit discipline as battery banks.
00625:                 _powerGridPanel.OnGeneratorServiceRequested += () =>
00626:                 {
00627:                     var inv = _inventory.Inventory;
00628:                     if (inv.CountById(PowerGridSystem.GeneratorMaintenanceItemId) < 1)
00629:                     {
00630:                         ObserveSigil("power.generator_missing_maintenance_item");
00631:                         return;
00632:                     }
00633:                     if (!_powerGrid.PerformGeneratorMaintenance(out var reason))
00634:                     {
00635:                         ObserveSigil("power.generator_service_blocked_" + reason);
00636:                         return;
00637:                     }
00638:                     inv.TryConsumeById(PowerGridSystem.GeneratorMaintenanceItemId, 1);
00639:                     ObserveSigil("power.generator_serviced");
00640:                 };
00641:                 // B5–B8 expansion (§27): emergency presets — Core owns the
00642:                 // policy; the host journals the outcome.
00643:                 _powerGridPanel.OnEmergencyPresetRequested += presetId =>
00644:                 {
00645:                     if (presetId == "shed")
00646:                     {
00647:                         var changed = _powerGrid.System.ApplyBrownoutShedPreset();
00648:                         _journal?.TryAddRawEntry("power_preset_shed",
00649:                             $"EMERGENCY: Load-shed preset applied — {changed.Count} non-critical circuit(s) demoted to preserve life support.", null!, _simDay);
00650:                         ObserveSigil("power.preset_shed");
00651:                     }
00652:                     else if (presetId == "defaults")
00653:                     {
00654:                         int restored = _powerGrid.System.ApplyCatalogDefaultPriorities();
00655:                         _journal?.TryAddRawEntry("power_preset_defaults",
00656:                             $"Power priorities restored to catalog defaults ({restored} override(s) cleared).", null!, _simDay);
00657:                         ObserveSigil("power.preset_defaults");
00658:                     }
00659:                 };
00660:                 AddChild(_powerGridPanel);
00661:             }
00662:             _powerGridPanel.Bind(_powerGrid);
00663:             _powerGridPanel.Open();
00664:         }
00665:
00666:         private void CloseOpeningProtocolModal()
00667:         {
00668:             _openingProtocolModal.Visible = false;
00669:         }
00670:
00671:         private void SetupGreenhouse()
00672:         {
00673:             if (_greenhouse != null) return;
00674:             SetupInventory();
00675:             _greenhouse = GreenhouseHostSession.Create(_inventory);
00676:             _greenhouse.SeasonWindowProvider = () =>
00677:             {
00678:                 var season = WildlifeSeasonalCalendar.SeasonWindowForDay(_world?.Profile, _simDay);
00679:                 return !string.IsNullOrEmpty(season?.displayName) ? season.displayName : (!string.IsNullOrEmpty(season?.id) ? season.id : "Standard");
00680:             };
00681:             _greenhouse.StateChanged += () =>
00682:             {
00683:                 _greenhouseDirty = true;
00684:                 _greenhousePanel?.RefreshView();
00685:                 if (_state == GameState.Playing) UpdateHud();
00686:             };
00687:             if (_greenhousePanel != null)
00688:                 _greenhousePanel.Bind(_greenhouse);
00689:             // Share growth authority with the expansion hub when it already exists
00690:             // (SetupExpansions-first path). Hub capture/restore then mirrors player plots.
00691:             if (_expansions != null)
00692:             {
00693:                 _expansions.BindGreenhouse(_greenhouse.System);
00694:                 _expansions.EnsureGreenhousePlots(3);
00695:             }
00696:             GD.Print("[Ashfall Godot] Greenhouse host ready.");
00697:         }
00698:
00699:         private void SaveGreenhouse()
00700:         {
00701:             if (_greenhouse == null) return;
00702:             if (CaptureSection("greenhouse", GreenhouseSaveStore.TryCapturePersisted(_greenhouse.CaptureSave())))
00703:             {
00704:                 _greenhouseDirty = false;
00705:                 GD.Print("[Ashfall Godot] Greenhouse save written.");
00706:             }
00707:         }
00708:
00709:         private void CloseGreenhousePanel()
00710:         {
00711:             _greenhousePanel.Visible = false;
00712:         }
00713:         private void CloseDeconAirlockPanel() { _deconAirlockPanel.Visible = false; }
00714:         private void CloseGeodeticSurveyPanel() { _geodeticSurveyPanel.Visible = false; }
00715:         private void CloseKineticStoragePanel() { _kineticStoragePanel.Visible = false; }
00716:         private void CloseChemicalReconPanel() { _chemicalReconPanel.Visible = false; }
00717:         private void CloseChemWarfareDefensePanel() { _chemWarfareDefensePanel?.Visible = false; }
00718:         private void CloseCommsArrayTransceiverPanel() { _commsArrayTransceiverPanel?.Visible = false; }
00719:         private void CloseCeremonyFestivalPanel() { _ceremonyFestivalPanel?.Visible = false; }
00720:         private void CloseRoboticsWorkshopPanel() { _roboticsWorkshopPanel?.Visible = false; }
00721:         private void CloseSurvivorDowntimePanel() { _survivorDowntimePanel?.Visible = false; }
00722:         private void CloseWinterFreezePanel() { _winterFreezePanel?.Visible = false; }
00723:         private void CloseFungiCultivationPanel() { _fungiCultivationBedPanel.Visible = false; }
00724:         private void CloseBioFermentationPanel() { _bioFermentationPanel.Visible = false; }
00725:         private void ClosePlasticPyrolysisPanel() { _plasticPyrolysisPanel.Visible = false; }
00726:         private void CloseCargoAirdropPanel() { _cargoAirdropPanel.Visible = false; }
00727:
00728:         private void CloseCraftingPanel()
00729:         {
00730:             if (_craftingPanel != null) _craftingPanel.Visible = false;
00731:         }
00732:
00733:         private void CloseWorkshopPanel()
00734:         {
00735:             if (_workshopPanel != null) _workshopPanel.Visible = false;
00736:         }
00737:
00738:         private void CloseRadioIntelligencePanel()
00739:         {
00740:             if (_radioIntelligencePanel != null) _radioIntelligencePanel.Visible = false;
00741:         }
00742:
00743:         private void CloseShelterSocialPanel()
00744:         {
00745:             if (_shelterSocialPanel != null) _shelterSocialPanel.Visible = false;
00746:         }
00747:
00748:         private void CloseSubterraneanOperationsPanel()
00749:         {
00750:             if (_subterraneanOperationsPanel != null) _subterraneanOperationsPanel.Visible = false;
00751:         }
00752:
00753:         private void ClosePharmaLabPanel()
00754:         {
00755:             if (_pharmaLabPanel != null) _pharmaLabPanel.Visible = false;
00756:         }
00757:
00758:         private void CloseWeatherPanel()
00759:         {
00760:             _weatherPanel.Visible = false;
00761:         }
00762:
00763:         private void CloseWeatherDetailPanel()
00764:         {
00765:             _weatherDetailPanel.Visible = false;
00766:         }
00767:
00768:         private void CloseWeatherForecastPanel()
00769:         {
00770:             _weatherForecastPanel.Visible = false;
00771:         }
00772:
00773:
00774:         private void HandleDeconAirlockAction(string action, string param = "")
00775:         {
00776:             if (action == "OPEN")
00777:             {
00778:                 if (_deconAirlockPanel != null)
00779:                 {
00780:                     if (!_deconAirlockBound && _decontamination != null) { _deconAirlockPanel.Bind(_decontamination); _deconAirlockBound = true; }
00781:                     _deconAirlockPanel.Visible = true;
00782:                 }
00783:                 return;
00784:             }
00785:             if (action == "CLOSE") { if (_deconAirlockPanel != null) _deconAirlockPanel.Visible = false; return; }
00786:             if (_deconAirlockPanel == null || _decontamination == null) return;
00787:             switch(action)
00788:             {
00789:                 case "start_decon":
00790:                 {
00791:                     // param is the selected queue caseId — resolve the real
00792:                     // occupant/gear/contamination from the decon queue.
00793:                     var c = _decontamination.System.State.queue.Find(q => q.caseId == param);
00794:                     if (c != null)
00795:                         _decontamination.System.StartProtocolCycle("decon_standard_return", c.survivorId, c.gearId, c.surfaceContamination);
00796:                     break;
00797:                 }
00798:                 case "tick_stage": _decontamination.System.TickActiveStage(); break;
00799:                 case "manual_override": _decontamination.System.EngageManualOverride(); break;
00800:                 case "dispose_gear": _decontamination.System.DisposeContaminatedGear(param); break;
00801:                 case "treat_effluent": _decontamination.System.TreatEffluent(); break;
00802:                 case "install_filter": _decontamination.System.InstallEffluentFilter(); break;
00803:             }
00804:             _deconAirlockPanel.RefreshView();
00805:             _decontaminationDirty = true;
00806:         }
00807:
00808:         private void HandleGeodeticSurveyAction(string action, string param = "")
00809:         {
00810:             if (action == "OPEN")
00811:             {
00812:                 if (_geodeticSurveyPanel != null)
00813:                 {
00814:                     if (!_geodeticSurveyBound && _geodeticSurvey != null) { _geodeticSurveyPanel.Bind(_geodeticSurvey); _geodeticSurveyBound = true; }
00815:                     _geodeticSurveyPanel.Visible = true;
00816:                 }
00817:                 return;
00818:             }
00819:             if (action == "CLOSE") { if (_geodeticSurveyPanel != null) _geodeticSurveyPanel.Visible = false; return; }
00820:             if (_geodeticSurveyPanel == null || _geodeticSurvey == null) return;
00821:             int day = _campaignDay?.Calendar?.CurrentDay ?? 1;
00822:             switch(action)
00823:             {
00824:                 case "establish":
00825:                     _geodeticSurvey.EstablishMonument(param, day, (id, count) => _inventory?.Inventory?.TryConsume(id, count) ?? false);
00826:                     break;
00827:                 case "observe":
00828:                 {
00829:                     // From = first active monument that is not the target point.
00830:                     string? from = _geodeticSurvey.System.Monuments
00831:                         .Where(m => m.isActive && m.surveyPointId != param)
00832:                         .Select(m => m.surveyPointId)
00833:                         .FirstOrDefault();
00834:                     if (from != null)
00835:                         _geodeticSurvey.System.Observe(from, param, "clear", 0.5f);
00836:                     break;
00837:                 }
00838:                 case "resolve":
00839:                 {
00840:                     // Try every triple of active monuments (idempotent — the
00841:                     // engine unlocks routes exactly once and returns the
00842:                     // existing triangle on repeats).
00843:                     var ids = _geodeticSurvey.System.Monuments
00844:                         .Where(m => m.isActive).Select(m => m.surveyPointId).ToList();
00845:                     for (int a = 0; a < ids.Count; a++)
00846:                         for (int b = a + 1; b < ids.Count; b++)
00847:                             for (int c = b + 1; c < ids.Count; c++)
00848:                                 _geodeticSurvey.System.TryResolveTriangle(ids[a], ids[b], ids[c]);
00849:                     break;
00850:                 }
00851:             }
00852:             _geodeticSurveyPanel.RefreshView();
00853:         }
00854:
00855:         private void HandleKineticStorageAction(string action, string param = "")
00856:         {
00857:             if (action == "OPEN")
00858:             {
00859:                 if (_kineticStoragePanel != null)
00860:                 {
00861:                     if (!_kineticStorageBound && _kineticStorage != null) { _kineticStoragePanel.Bind(_kineticStorage); _kineticStorageBound = true; }
00862:                     _kineticStoragePanel.Visible = true;
00863:                 }
00864:                 return;
00865:             }
00866:             if (action == "CLOSE") { if (_kineticStoragePanel != null) _kineticStoragePanel.Visible = false; return; }
00867:             if (_kineticStoragePanel == null || _kineticStorage == null) return;
00868:             int day = _campaignDay?.Calendar?.CurrentDay ?? 1;
00869:             // Class-rate power limits: never exceed the catalog's charge/discharge kW.
00870:             var flywheel = _kineticStorage.System.FindFlywheel(param);
00871:             var fc = flywheel != null ? _kineticStorage.System.FindClass(flywheel.flywheelClassId) : null;
00872:             switch(action)
00873:             {
00874:                 case "CHARGE":
00875:                     if (fc != null) _kineticStorage.System.Charge(param, fc.max_charge_kw, 600f);
00876:                     break;
00877:                 case "DISCHARGE":
00878:                     if (fc != null) _kineticStorage.System.Discharge(param, fc.max_discharge_kw, 60f);
00879:                     break;
00880:                 case "EMERGENCY_BRAKE":
00881:                     _kineticStorage.EngageEmergencyBrake(param);
00882:                     break;
00883:                 case "MAINTENANCE":
00884:                     _kineticStorage.PerformMaintenance(param, day, (id, count) => _inventory?.Inventory?.TryConsume(id, count) ?? false);
00885:                     break;
00886:             }
00887:             _kineticStoragePanel.RefreshView();
00888:         }
00889:
00890:         private void HandleChemicalReconAction(string action, string param = "")
00891:         {
00892:             if (action == "OPEN")
00893:             {
00894:                 if (_chemicalReconPanel != null)
00895:                 {
00896:                     if (!_chemicalReconBound && _chemicalRecon != null) { _chemicalReconPanel.Bind(_chemicalRecon); _chemicalReconBound = true; }
00897:                     _chemicalReconPanel.Visible = true;
00898:                 }
00899:                 return;
00900:             }
00901:             if (action == "CLOSE") { if (_chemicalReconPanel != null) _chemicalReconPanel.Visible = false; return; }
00902:             if (_chemicalReconPanel == null || _chemicalRecon == null) return;
00903:             switch(action)
00904:             {
00905:                 case "deploy_sensor":
00906:                     // Scan with the engine's active sensor band (Core-owned state).
00907:                     _chemicalRecon.System.ScanLocation(param, _chemicalRecon.System.State.activeSensorBand, 0.5f);
00908:                     break;
00909:                 case "sample":
00910:                 {
00911:                     // param is the hazardId — resolve the location from the
00912:                     // latest observation of that hazard.
00913:                     var obs = _chemicalRecon.System.Observations
00914:                         .Where(o => o.hazardId == param)
00915:                         .OrderByDescending(o => o.lastConfirmedDay)
00916:                         .FirstOrDefault();
00917:                     if (obs != null)
00918:                         _chemicalRecon.CollectSample(param, obs.locationNodeId, 0.5f,
00919:                             (itemId, count) => _inventory?.Inventory?.TryConsume(itemId, count) ?? false);
00920:                     break;
00921:                 }
00922:                 case "change_filter":
00923:                 {
00924:                     // Select the recommended filter category for this location.
00925:                     string recommended = _chemicalRecon.System.GetRecommendedFilter(param);
00926:                     _chemicalRecon.System.SelectFilterCategory(recommended);
00927:                     break;
00928:                 }
00929:             }
00930:             _chemicalReconPanel.RefreshView();
00931:         }
00932:
00933:         private void HandleGeothermalAction(string action, string param = "")
00934:         {
00935:             if (action == "OPEN") { if (_geothermalAquiferPanel != null) _geothermalAquiferPanel.Visible = true; return; }
00936:             if (action == "CLOSE") { if (_geothermalAquiferPanel != null) _geothermalAquiferPanel.Visible = false; return; }
00937:             if (_geothermalAquiferPanel == null || _geothermalAquifer == null) return;
00938:             switch(action)
00939:             {
00940:                 case "start_drilling": _geothermalAquifer.System.StartDrilling(); break;
00941:                 case "install_casing": _geothermalAquifer.System.InstallCasing(float.Parse(param ?? "100")); break;
00942:                 case "commission_turbine": _geothermalAquifer.System.CommissionTurbine(); break;
00943:                 case "descale": _geothermalAquifer.System.Descale(); break;
00944:                 case "vent_pressure": _geothermalAquifer.System.VentPressure(); break;
00945:                 case "tap_aquifer": _geothermalAquifer.System.TapAquifer(); break;
00946:             }
00947:             _geothermalAquiferPanel.RefreshView();
00948:         }
00949:     }
00950: }
```

## `Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs` — 167 lines; 5,825 bytes; SHA-256 `0a1ab6c8356a96a99ebb8df15b0efef3c009b8967d902f19aaf93481f0390ca3`
Declaration index:
- 00012: public class OrbitalHarrowTelemetrySystemTests
- 00014: private static string ResolveDataDir()
- 00030: public void ActivateTelemetry_EnablesSystem()
- 00038: public void ScheduleImpact_CreatesWarning()
- 00047: public void Brace_MitigatesImpact()
- 00057: public void TickDay_OnImpactDay_Resolves()
- 00069: public void Brace_WhenNoImpact_Blocks()
- 00077: public void CaptureRestoreState_PreservesImpact()
- 00090: public void TelemetryCatalog_ContainsTwelveCanonicalEvents()
- 00116: public void FalsePositiveEvents_ResolveWithoutDamageOrBreach()
- 00142: public void DeadHandEvents_CarryRadioHooksAndRevealSites()
- 00160: private static OrbitalHarrowTelemetrySystem Create(out SkyLayerArmorSystem armor)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using System.Linq;
00005: using Ashfall.Core;
00006: using Ashfall.Core.IO;
00007: using Ashfall.Core.Shelter;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests
00011: {
00012:     public class OrbitalHarrowTelemetrySystemTests
00013:     {
00014:         private static string ResolveDataDir()
00015:         {
00016:             string baseDir = AppContext.BaseDirectory;
00017:             string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
00018:             if (Directory.Exists(probe)) return probe;
00019:
00020:             probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
00021:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00022:
00023:             probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
00024:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00025:
00026:             return string.Empty;
00027:         }
00028:
00029:         [Fact]
00030:         public void ActivateTelemetry_EnablesSystem()
00031:         {
00032:             var oh = Create(out _);
00033:             oh.ActivateTelemetry(1);
00034:             Assert.True(oh.State.telemetryActive);
00035:         }
00036:
00037:         [Fact]
00038:         public void ScheduleImpact_CreatesWarning()
00039:         {
00040:             var oh = Create(out _);
00041:             oh.ScheduleImpact(10, 5, 25f);
00042:             Assert.Single(oh.State.warnings);
00043:             Assert.True(oh.HasPendingImpact);
00044:         }
00045:
00046:         [Fact]
00047:         public void Brace_MitigatesImpact()
00048:         {
00049:             var oh = Create(out _);
00050:             oh.ScheduleImpact(10, 5, 25f);
00051:             var r = oh.Brace("concrete", 5);
00052:             Assert.Equal(ActionResult.StatusKind.Success, r.Status);
00053:             Assert.True(oh.State.isBraced);
00054:         }
00055:
00056:         [Fact]
00057:         public void TickDay_OnImpactDay_Resolves()
00058:         {
00059:             var oh = Create(out _);
00060:             oh.ScheduleImpact(10, 5, 25f);
00061:             bool resolved = false;
00062:             oh.OnImpactResolved += (_, _) => resolved = true;
00063:             oh.TickDay(10);
00064:             Assert.True(resolved);
00065:             Assert.False(oh.HasPendingImpact);
00066:         }
00067:
00068:         [Fact]
00069:         public void Brace_WhenNoImpact_Blocks()
00070:         {
00071:             var oh = Create(out _);
00072:             var r = oh.Brace("concrete", 5);
00073:             Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
00074:         }
00075:
00076:         [Fact]
00077:         public void CaptureRestoreState_PreservesImpact()
00078:         {
00079:             var oh = Create(out _);
00080:             oh.ScheduleImpact(10, 5, 25f);
00081:             var state = oh.CaptureState();
00082:             Assert.Equal(10, state.nextImpactDay);
00083:
00084:             var oh2 = Create(out _);
00085:             oh2.RestoreState(state);
00086:             Assert.True(oh2.HasPendingImpact);
00087:         }
00088:
00089:         [Fact]
00090:         public void TelemetryCatalog_ContainsTwelveCanonicalEvents()
00091:         {
00092:             string dataDir = ResolveDataDir();
00093:             var events = OrbitalHarrowCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00094:
00095:             Assert.NotNull(events);
00096:             Assert.Equal(12, events.Count);
00097:
00098:             // 4 kinetic, 2 cluster, 2 emp, 2 dead hand, 2 false alarm
00099:             var kinetic = events.Where(e => e.id.Contains("kinetic")).ToList();
00100:             Assert.Equal(4, kinetic.Count);
00101:
00102:             var cluster = events.Where(e => e.id.Contains("cluster")).ToList();
00103:             Assert.Equal(2, cluster.Count);
00104:
00105:             var emp = events.Where(e => e.id.Contains("emp")).ToList();
00106:             Assert.Equal(2, emp.Count);
00107:
00108:             var deadHand = events.Where(e => e.id.Contains("dead_hand")).ToList();
00109:             Assert.Equal(2, deadHand.Count);
00110:
00111:             var falseAlarms = events.Where(e => e.is_false_positive).ToList();
00112:             Assert.Equal(2, falseAlarms.Count);
00113:         }
00114:
00115:         [Fact]
00116:         public void FalsePositiveEvents_ResolveWithoutDamageOrBreach()
00117:         {
00118:             string dataDir = ResolveDataDir();
00119:             var events = OrbitalHarrowCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00120:             var falseAlarm = events.First(e => e.id == "event_orbital_radar_ducting_false_alarm");
00121:
00122:             var oh = Create(out var armor);
00123:             oh.ActivateTelemetry(1);
00124:             oh.ScheduleEventDef(falseAlarm, day: 4, gridX: 5);
00125:
00126:             OrbitalImpactReport? report = null;
00127:             oh.OnImpactDetailed += r => report = r;
00128:
00129:             oh.TickDay(4);
00130:
00131:             Assert.NotNull(report);
00132:             Assert.False(report.AnyBreached);
00133:             Assert.Equal(0f, report.TotalPenetrationDamage);
00134:             Assert.Equal(0f, report.PowerGridDisruption);
00135:
00136:             var cell = armor.GetCell(5);
00137:             Assert.NotNull(cell);
00138:             Assert.Equal(100f, cell.currentDurability); // 0 damage dealt
00139:         }
00140:
00141:         [Fact]
00142:         public void DeadHandEvents_CarryRadioHooksAndRevealSites()
00143:         {
00144:             string dataDir = ResolveDataDir();
00145:             var events = OrbitalHarrowCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00146:             var deadHand = events.First(e => e.id == "event_orbital_dead_hand_repeating_ping");
00147:
00148:             Assert.False(string.IsNullOrWhiteSpace(deadHand.radio_hook_text));
00149:             Assert.Equal("loc_excavation_command_vault", deadHand.revealed_site_id);
00150:
00151:             var oh = Create(out _);
00152:             oh.ActivateTelemetry(1);
00153:             oh.ScheduleEventDef(deadHand, day: 5, gridX: 5);
00154:
00155:             oh.TickDay(5);
00156:
00157:             Assert.Contains("loc_excavation_command_vault", oh.RevealedSites);
00158:         }
00159:
00160:         private static OrbitalHarrowTelemetrySystem Create(out SkyLayerArmorSystem armor)
00161:         {
00162:             armor = new SkyLayerArmorSystem();
00163:             armor.SetCellArmor(5, CeilingMaterialTier.ReinforcedConcrete, 0.5f);
00164:             return new OrbitalHarrowTelemetrySystem(armor, new SeededRng(42));
00165:         }
00166:     }
00167: }
```

## `Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs` — 309 lines; 12,575 bytes; SHA-256 `2522af036681c87b5311c24414087f23485aec6d26ce6c9bfc749a321a70aff4`
Declaration index:
- 00013: public class SkyLayerArmorCatalogTests
- 00015: private static string ResolveDataDir()
- 00031: public void ArmorCatalog_LoadsAllSixAuthoredConfigurations()
- 00065: public void ArmorCatalog_AllMaterialItemReferencesResolveInItemsCatalog()
- 00106: public void ArmorCatalog_DefaultConfigurationsFallbackMatchesSixConfigs()
- 00121: public void OrbitalThreatCatalog_LoadsTwelveUniqueEvents()
- 00149: public void SkyLayerArmor_InstallationAndAttenuationHierarchy()
- 00177: public void SkyLayerArmor_EvaluateImpact_MitigationAndBreach()
- 00204: public void SkyLayerArmor_RepairCell_RestoresDurability()
- 00220: public void SkyLayerArmor_SaveAndRestore_PreservesAllCells()
- 00251: public void FullDefenseLoop_TelemetryWarning_Brace_Strike_Mitigation_Salvage()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using Ashfall.Core;
00008: using Ashfall.Core.Shelter;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.Shelter
00012: {
00013:     public class SkyLayerArmorCatalogTests
00014:     {
00015:         private static string ResolveDataDir()
00016:         {
00017:             string baseDir = AppContext.BaseDirectory;
00018:             string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
00019:             if (Directory.Exists(probe)) return probe;
00020:
00021:             probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
00022:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00023:
00024:             probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
00025:             if (Directory.Exists(probe)) return Path.GetFullPath(probe);
00026:
00027:             return string.Empty;
00028:         }
00029:
00030:         [Fact]
00031:         public void ArmorCatalog_LoadsAllSixAuthoredConfigurations()
00032:         {
00033:             string dataDir = ResolveDataDir();
00034:             var configs = SkyLayerArmorCatalogLoader.Load(dataDir);
00035:
00036:             Assert.NotNull(configs);
00037:             Assert.Equal(6, configs.Count);
00038:
00039:             var expectedIds = new[]
00040:             {
00041:                 "sky_armor_sandbag_layer",
00042:                 "sky_armor_scrap_overlay",
00043:                 "sky_armor_reinforced_concrete",
00044:                 "sky_armor_steel_hull_plating",
00045:                 "sky_armor_composite_military",
00046:                 "sky_armor_emergency_blast_canopy"
00047:             };
00048:
00049:             foreach (var id in expectedIds)
00050:             {
00051:                 var cfg = configs.FirstOrDefault(c => c.id == id);
00052:                 Assert.NotNull(cfg);
00053:                 Assert.False(string.IsNullOrWhiteSpace(cfg.name));
00054:                 Assert.False(string.IsNullOrWhiteSpace(cfg.description));
00055:                 Assert.True(cfg.default_thickness_meters > 0f);
00056:                 Assert.True(cfg.blast_resistance_mj > 0f);
00057:                 Assert.True(cfg.attenuation_factor > 0f && cfg.attenuation_factor <= 1f);
00058:                 Assert.True(cfg.degradation_rate > 0f);
00059:                 Assert.NotEmpty(cfg.composition);
00060:                 Assert.NotEmpty(cfg.repair_cost);
00061:             }
00062:         }
00063:
00064:         [Fact]
00065:         public void ArmorCatalog_AllMaterialItemReferencesResolveInItemsCatalog()
00066:         {
00067:             string dataDir = ResolveDataDir();
00068:             var configs = SkyLayerArmorCatalogLoader.Load(dataDir);
00069:             string itemsPath = Path.Combine(dataDir, "items.json");
00070:             Assert.True(File.Exists(itemsPath), "items.json must exist");
00071:
00072:             string itemsJson = File.ReadAllText(itemsPath);
00073:             using var doc = JsonDocument.Parse(itemsJson);
00074:             var itemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00075:
00076:             if (doc.RootElement.TryGetProperty("items", out var itemArray))
00077:             {
00078:                 foreach (var item in itemArray.EnumerateArray())
00079:                 {
00080:                     if (item.TryGetProperty("id", out var idProp))
00081:                         itemIds.Add(idProp.GetString() ?? string.Empty);
00082:                 }
00083:             }
00084:
00085:             foreach (var cfg in configs)
00086:             {
00087:                 foreach (var comp in cfg.composition)
00088:                 {
00089:                     Assert.False(string.IsNullOrWhiteSpace(comp.item_id));
00090:                     Assert.True(comp.quantity > 0);
00091:                     Assert.True(itemIds.Contains(comp.item_id),
00092:                         $"Armor config '{cfg.id}' references unknown composition item '{comp.item_id}'");
00093:                 }
00094:
00095:                 foreach (var rep in cfg.repair_cost)
00096:                 {
00097:                     Assert.False(string.IsNullOrWhiteSpace(rep.item_id));
00098:                     Assert.True(rep.quantity > 0);
00099:                     Assert.True(itemIds.Contains(rep.item_id),
00100:                         $"Armor config '{cfg.id}' references unknown repair item '{rep.item_id}'");
00101:                 }
00102:             }
00103:         }
00104:
00105:         [Fact]
00106:         public void ArmorCatalog_DefaultConfigurationsFallbackMatchesSixConfigs()
00107:         {
00108:             var defaultConfigs = SkyLayerArmorCatalogLoader.GetDefaultConfigurations();
00109:             Assert.Equal(6, defaultConfigs.Count);
00110:
00111:             var ids = defaultConfigs.Select(c => c.id).ToHashSet();
00112:             Assert.Contains("sky_armor_sandbag_layer", ids);
00113:             Assert.Contains("sky_armor_scrap_overlay", ids);
00114:             Assert.Contains("sky_armor_reinforced_concrete", ids);
00115:             Assert.Contains("sky_armor_steel_hull_plating", ids);
00116:             Assert.Contains("sky_armor_composite_military", ids);
00117:             Assert.Contains("sky_armor_emergency_blast_canopy", ids);
00118:         }
00119:
00120:         [Fact]
00121:         public void OrbitalThreatCatalog_LoadsTwelveUniqueEvents()
00122:         {
00123:             string dataDir = ResolveDataDir();
00124:             var fileIO = new FileSystemIO();
00125:             var json = new SystemTextJsonSerializer();
00126:             var events = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, json);
00127:
00128:             Assert.NotNull(events);
00129:             Assert.Equal(12, events.Count);
00130:
00131:             var eventIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00132:             foreach (var evt in events)
00133:             {
00134:                 Assert.True(eventIds.Add(evt.id), $"Duplicate event ID: {evt.id}");
00135:                 Assert.False(string.IsNullOrWhiteSpace(evt.name));
00136:                 Assert.False(string.IsNullOrWhiteSpace(evt.description));
00137:                 if (!evt.is_false_positive)
00138:                 {
00139:                     Assert.True(evt.impact_energy_mj > 0f);
00140:                 }
00141:                 Assert.True(evt.lead_time_days > 0);
00142:                 Assert.True(evt.affected_cell_spread >= 1);
00143:                 Assert.False(string.IsNullOrWhiteSpace(evt.salvage_yield_item_id));
00144:                 Assert.True(evt.salvage_yield_quantity >= 1);
00145:             }
00146:         }
00147:
00148:         [Fact]
00149:         public void SkyLayerArmor_InstallationAndAttenuationHierarchy()
00150:         {
00151:             string dataDir = ResolveDataDir();
00152:             var configs = SkyLayerArmorCatalogLoader.Load(dataDir);
00153:             var armor = new SkyLayerArmorSystem();
00154:
00155:             int gridX = 0;
00156:             foreach (var cfg in configs)
00157:             {
00158:                 armor.InstallConfiguration(gridX, cfg);
00159:                 var cell = armor.GetCell(gridX);
00160:                 Assert.NotNull(cell);
00161:                 Assert.Equal(cfg.material_tier, cell.material);
00162:                 Assert.Equal(cfg.default_thickness_meters, cell.thicknessMeters);
00163:                 Assert.Equal(100f, cell.currentDurability);
00164:
00165:                 float attenuation = armor.GetAttenuationFactor(gridX);
00166:                 Assert.True(attenuation > 0f && attenuation <= 1f);
00167:                 gridX++;
00168:             }
00169:
00170:             // Tungsten composite should have much lower attenuation (higher shielding) than dirt
00171:             float dirtAttenuation = armor.GetAttenuationFactor(0); // Sandbag (Dirt)
00172:             float tungstenAttenuation = armor.GetAttenuationFactor(4); // Composite Military (Tungsten)
00173:             Assert.True(tungstenAttenuation < dirtAttenuation);
00174:         }
00175:
00176:         [Fact]
00177:         public void SkyLayerArmor_EvaluateImpact_MitigationAndBreach()
00178:         {
00179:             string dataDir = ResolveDataDir();
00180:             var configs = SkyLayerArmorCatalogLoader.Load(dataDir);
00181:             var concreteCfg = configs.First(c => c.id == "sky_armor_reinforced_concrete");
00182:
00183:             var armor = new SkyLayerArmorSystem();
00184:             armor.InstallConfiguration(5, concreteCfg);
00185:
00186:             // Absorption threshold = 25 * 1.5 = 37.5 MJ
00187:             // Strike 1: 20 MJ -> should be absorbed completely
00188:             bool breached = armor.EvaluateKineticImpact(5, 20f, out float roofDamage);
00189:             Assert.False(breached);
00190:             Assert.Equal(0f, roofDamage);
00191:
00192:             var cell = armor.GetCell(5);
00193:             Assert.NotNull(cell);
00194:             Assert.True(cell.currentDurability < 100f);
00195:
00196:             // Strike 2: 50 MJ -> exceeds 37.5 MJ -> breach occurs
00197:             breached = armor.EvaluateKineticImpact(5, 50f, out roofDamage);
00198:             Assert.True(breached);
00199:             Assert.Equal(50f - 37.5f, roofDamage, precision: 1);
00200:             Assert.True(cell.currentDurability <= 50f);
00201:         }
00202:
00203:         [Fact]
00204:         public void SkyLayerArmor_RepairCell_RestoresDurability()
00205:         {
00206:             var armor = new SkyLayerArmorSystem();
00207:             armor.SetCellArmor(3, CeilingMaterialTier.ReinforcedConcrete, 1.0f, durability: 40f);
00208:
00209:             armor.RepairCell(3, 30f);
00210:             var cell = armor.GetCell(3);
00211:             Assert.NotNull(cell);
00212:             Assert.Equal(70f, cell.currentDurability);
00213:
00214:             // Cannot exceed 100
00215:             armor.RepairCell(3, 50f);
00216:             Assert.Equal(100f, cell.currentDurability);
00217:         }
00218:
00219:         [Fact]
00220:         public void SkyLayerArmor_SaveAndRestore_PreservesAllCells()
00221:         {
00222:             var armor1 = new SkyLayerArmorSystem();
00223:             armor1.SetCellArmor(1, CeilingMaterialTier.Dirt, 0.8f, 75f);
00224:             armor1.SetCellArmor(2, CeilingMaterialTier.LeadSheeting, 1.2f, 90f);
00225:             armor1.SetCellArmor(3, CeilingMaterialTier.TungstenComposite, 2.0f, 100f);
00226:
00227:             var state = armor1.CaptureState();
00228:             Assert.Equal(3, state.cells.Count);
00229:
00230:             var armor2 = new SkyLayerArmorSystem();
00231:             armor2.RestoreState(state);
00232:
00233:             var c1 = armor2.GetCell(1);
00234:             var c2 = armor2.GetCell(2);
00235:             var c3 = armor2.GetCell(3);
00236:
00237:             Assert.NotNull(c1);
00238:             Assert.Equal(CeilingMaterialTier.Dirt, c1.material);
00239:             Assert.Equal(75f, c1.currentDurability);
00240:
00241:             Assert.NotNull(c2);
00242:             Assert.Equal(CeilingMaterialTier.LeadSheeting, c2.material);
00243:             Assert.Equal(90f, c2.currentDurability);
00244:
00245:             Assert.NotNull(c3);
00246:             Assert.Equal(CeilingMaterialTier.TungstenComposite, c3.material);
00247:             Assert.Equal(100f, c3.currentDurability);
00248:         }
00249:
00250:         [Fact]
00251:         public void FullDefenseLoop_TelemetryWarning_Brace_Strike_Mitigation_Salvage()
00252:         {
00253:             string dataDir = ResolveDataDir();
00254:             var fileIO = new FileSystemIO();
00255:             var json = new SystemTextJsonSerializer();
00256:             var threatEvents = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, json);
00257:             var configs = SkyLayerArmorCatalogLoader.Load(dataDir);
00258:
00259:             var armor = new SkyLayerArmorSystem();
00260:             var concreteCfg = configs.First(c => c.id == "sky_armor_reinforced_concrete");
00261:             armor.InstallConfiguration(4, concreteCfg);
00262:
00263:             var telemetry = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(1986));
00264:             telemetry.ActivateTelemetry(1);
00265:
00266:             var standardStrike = threatEvents.First(e => e.id == "event_orbital_kinetic_early_track");
00267:             telemetry.ScheduleEventDef(standardStrike, day: 3, gridX: 4);
00268:
00269:             Assert.True(telemetry.HasPendingImpact);
00270:             Assert.Single(telemetry.State.warnings);
00271:
00272:             // Preparation: Brace before impact
00273:             var braceRes = telemetry.Brace("scrap_metal", 2);
00274:             Assert.True(braceRes.IsSuccess);
00275:             Assert.True(telemetry.State.isBraced);
00276:
00277:             OrbitalImpactReport? finalReport = null;
00278:             telemetry.OnImpactDetailed += r => finalReport = r;
00279:
00280:             // Advance day to 3 (impact day)
00281:             telemetry.TickDay(3);
00282:
00283:             Assert.NotNull(finalReport);
00284:             Assert.Equal("event_orbital_kinetic_early_track", finalReport.EventId);
00285:             Assert.False(telemetry.HasPendingImpact);
00286:             Assert.Equal(3, telemetry.State.lastImpactDay);
00287:
00288:             // With bracing, 35 MJ * 0.5 = 17.5 MJ, well below concrete's 37.5 MJ threshold -> No breach
00289:             Assert.False(finalReport.AnyBreached);
00290:             Assert.Equal(0f, finalReport.TotalPenetrationDamage);
00291:
00292:             // Salvage opportunity spawned
00293:             Assert.Single(telemetry.ActiveSalvage);
00294:             var salvage = telemetry.ActiveSalvage[0];
00295:             Assert.False(salvage.isClaimed);
00296:
00297:             var claimRes = telemetry.ClaimSalvage(standardStrike.id);
00298:             Assert.True(claimRes.IsSuccess);
00299:             Assert.True(salvage.isClaimed);
00300:
00301:             // Post-impact repair
00302:             var cell = armor.GetCell(4);
00303:             Assert.NotNull(cell);
00304:             Assert.True(cell.currentDurability < 100f);
00305:             armor.RepairCell(4, 30f);
00306:             Assert.Equal(100f, cell.currentDurability);
00307:         }
00308:     }
00309: }
```

## `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs` — 410 lines; 21,080 bytes; SHA-256 `58130dae9d890fd61d10c85f141e3fa59852d2b910e0cba09663d9b5a5ea7155`
Declaration index:
- 00025: public sealed class Roadmap42Batch1IntegrationTests
- 00027: private static string ResolveDataDir()
- 00034: private sealed class TestSurvivorActor : SkillActor
- 00048: public void SetSkillBonus(string disciplineId, float bonus) { }
- 00052: public void Test1_ReferentialIntegrity_CrossCatalogContracts_AllResolve()
- 00169: private sealed class CampaignExecutionSnapshot
- 00186: public string ComputeDeterministicHash()
- 00195: private static CampaignExecutionSnapshot ExecuteDeterministicCampaignJourney(int masterSeed)
- 00336: public void Test2_FullDeterministicCampaignJourney_TouchesAllTenSystems_ReplaysIdentically()
- 00353: public void Test3_Batch1CatalogCountsAndLoaderClassifications_MatchAuthoritativeTruth()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Excavation;
00008: using Ashfall.Core.Expeditions;
00009: using Ashfall.Core.IO;
00010: using Ashfall.Core.Shelter;
00011: using Ashfall.Core.Survivors;
00012: using Ashfall.Core.World;
00013: using Xunit;
00014:
00015: namespace Ashfall.Core.Tests.Integration
00016: {
00017:     /// <summary>
00018:     /// Roadmap 42 — Batch 1 (Plans 32–41): Scaffolding & Underused-System Catalogs.
00019:     /// Canonical integration test suite proving:
00020:     /// 1. Referential integrity across all 10 authored catalogs (§23).
00021:     /// 2. The 20-step deterministic multi-system campaign journey (§33).
00022:     /// 3. Cross-system save/restore round-trip preservation (§21).
00023:     /// 4. Deterministic replay equality across independent seeded runs (§22).
00024:     /// </summary>
00025:     public sealed class Roadmap42Batch1IntegrationTests
00026:     {
00027:         private static string ResolveDataDir()
00028:         {
00029:             if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
00030:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
00031:             throw new InvalidOperationException("StreamingAssets/Data directory not found");
00032:         }
00033:
00034:         private sealed class TestSurvivorActor : SkillActor
00035:         {
00036:             public string Id { get; }
00037:             public bool IsAlive => true;
00038:             public float Morale => 100f;
00039:             public float Health => 100f;
00040:             public string ExpertDisciplineId { get; set; } = string.Empty;
00041:
00042:             public TestSurvivorActor(string id, string expertDiscipline = "")
00043:             {
00044:                 Id = id;
00045:                 ExpertDisciplineId = expertDiscipline;
00046:             }
00047:
00048:             public void SetSkillBonus(string disciplineId, float bonus) { }
00049:         }
00050:
00051:         [Fact]
00052:         public void Test1_ReferentialIntegrity_CrossCatalogContracts_AllResolve()
00053:         {
00054:             string dataDir = ResolveDataDir();
00055:             var fileIO = new FileSystemIO();
00056:             var serializer = new SystemTextJsonSerializer();
00057:
00058:             // 1. Plan 32: Expeditions (55 canonical destinations)
00059:             var expeditions = ExpeditionCatalogLoader.Load(dataDir, fileIO, serializer);
00060:             Assert.True(expeditions.Count >= 50, $"Expected >= 50 expedition destinations, got {expeditions.Count}");
00061:             var expIds = new HashSet<string>(expeditions.Select(e => e.id), StringComparer.Ordinal);
00062:
00063:             // 2. Plan 33: Skills (160 authored skills)
00064:             var skills = SkillCatalogLoader.Load(dataDir, fileIO, serializer);
00065:             Assert.True(skills.Count >= 50, $"Expected >= 50 skills, got {skills.Count}");
00066:             var skillIds = new HashSet<string>(skills.Select(s => s.id), StringComparer.Ordinal);
00067:             Assert.Contains("skill_field_dressing", skillIds);
00068:             Assert.Contains("skill_rough_repairs", skillIds);
00069:
00070:             // 3. Plan 34: Research Knowledge
00071:             var researchNodes = ResearchKnowledgeCatalogLoader.Load(dataDir, fileIO, serializer);
00072:             Assert.True(researchNodes.Count >= 15, $"Expected >= 15 research nodes, got {researchNodes.Count}");
00073:             var researchIds = new HashSet<string>(researchNodes.Select(r => r.id), StringComparer.Ordinal);
00074:
00075:             // Cross-ref check 33 -> 34: any skill prerequisites in research point to real skills
00076:             foreach (var node in researchNodes)
00077:             {
00078:                 if (node.prerequisites != null)
00079:                 {
00080:                     foreach (var prereq in node.prerequisites)
00081:                     {
00082:                         if (prereq.StartsWith("skill_", StringComparison.Ordinal))
00083:                         {
00084:                             Assert.True(skillIds.Contains(prereq), $"Research '{node.id}' references unknown skill '{prereq}'");
00085:                         }
00086:                     }
00087:                 }
00088:             }
00089:
00090:             // 4. Plan 35: Wildlife Ecosystem / Migration
00091:             string ecoPath = fileIO.Combine(dataDir, "wildlife_ecosystem.json");
00092:             Assert.True(fileIO.FileExists(ecoPath), "wildlife_ecosystem.json must exist");
00093:
00094:             // 5. Plan 36: Trapping Catalog (10 traps + 15 prey)
00095:             var trapCatalog = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, serializer);
00096:             Assert.NotNull(trapCatalog);
00097:             Assert.True(trapCatalog.Traps.Count >= 10, $"Expected >= 10 traps, got {trapCatalog.Traps.Count}");
00098:             Assert.True(trapCatalog.Prey.Count >= 15, $"Expected >= 15 prey species, got {trapCatalog.Prey.Count}");
00099:
00100:             // 6. Plan 37: Excavation Sites (8 sites)
00101:             var excavationSites = ExcavationCatalogLoader.Load(dataDir, fileIO, serializer);
00102:             Assert.NotNull(excavationSites);
00103:             Assert.Equal(8, excavationSites.Count);
00104:             foreach (var site in excavationSites)
00105:             {
00106:                 Assert.False(string.IsNullOrWhiteSpace(site.site_id));
00107:                 Assert.True(site.max_depth_meters > 0f);
00108:                 Assert.True(site.required_progress > 0f);
00109:             }
00110:
00111:             // 7. Plan 38: Sky-Layer Armor (6 configs)
00112:             var armorConfigs = SkyLayerArmorCatalogLoader.Load(dataDir, fileIO, serializer);
00113:             Assert.NotNull(armorConfigs);
00114:             Assert.Equal(6, armorConfigs.Count);
00115:
00116:             // 8. Plan 39: Orbital Harrow Telemetry Events (12 events)
00117:             var harrowEvents = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, serializer);
00118:             Assert.NotNull(harrowEvents);
00119:             Assert.Equal(12, harrowEvents.Count);
00120:
00121:             // Cross-ref check 37 <-> 39: revealed sites reference real excavation locations
00122:             var excavationLocIds = new HashSet<string>(excavationSites.Select(s => s.location_id), StringComparer.Ordinal);
00123:             foreach (var ev in harrowEvents)
00124:             {
00125:                 if (!string.IsNullOrEmpty(ev.revealed_site_id))
00126:                 {
00127:                     Assert.True(excavationLocIds.Contains(ev.revealed_site_id),
00128:                         $"Telemetry event '{ev.id}' references unknown excavation location '{ev.revealed_site_id}'");
00129:                 }
00130:             }
00131:
00132:             // 9. Plan 40: Ledger Debt Templates (15 templates + 10 consequences)
00133:             var debtCatalog = DebtTemplateCatalogLoader.Load(dataDir, fileIO, serializer);
00134:             Assert.NotNull(debtCatalog);
00135:             Assert.Equal(15, debtCatalog.Templates.Count);
00136:             Assert.Equal(10, debtCatalog.Consequences.Count);
00137:             var consequenceIds = new HashSet<string>(debtCatalog.Consequences.Select(c => c.id), StringComparer.Ordinal);
00138:             foreach (var tmpl in debtCatalog.Templates)
00139:             {
00140:                 Assert.True(consequenceIds.Contains(tmpl.consequenceId),
00141:                     $"Debt template '{tmpl.id}' references unknown consequence '{tmpl.consequenceId}'");
00142:             }
00143:
00144:             // 10. Plan 41: Shelter Rooms (23 rooms + 12 assignment rules)
00145:             var roomCatalog = ShelterRoomCatalogLoader.Load(dataDir);
00146:             Assert.NotNull(roomCatalog);
00147:             Assert.True(roomCatalog.rooms.Count >= 20, $"Expected >= 20 rooms, got {roomCatalog.rooms.Count}");
00148:             Assert.Equal(12, roomCatalog.assignment_rules.Count);
00149:
00150:             // Cross-ref check 33 -> 41: room and rule required skills resolve in skills.json
00151:             foreach (var room in roomCatalog.rooms)
00152:             {
00153:                 if (!string.IsNullOrEmpty(room.required_skill_id))
00154:                 {
00155:                     Assert.True(skillIds.Contains(room.required_skill_id),
00156:                         $"Room '{room.id}' references unknown skill '{room.required_skill_id}'");
00157:                 }
00158:             }
00159:             foreach (var rule in roomCatalog.assignment_rules)
00160:             {
00161:                 if (!string.IsNullOrEmpty(rule.required_skill_id))
00162:                 {
00163:                     Assert.True(skillIds.Contains(rule.required_skill_id),
00164:                         $"Assignment rule '{rule.id}' references unknown skill '{rule.required_skill_id}'");
00165:                 }
00166:             }
00167:         }
00168:
00169:         private sealed class CampaignExecutionSnapshot
00170:         {
00171:             public float SkillXp { get; set; }
00172:             public bool HasActiveSkill { get; set; }
00173:             public int ResearchCompletedCount { get; set; }
00174:             public int ExpeditionCount { get; set; }
00175:             public int PackCount { get; set; }
00176:             public string PackSector { get; set; } = string.Empty;
00177:             public int TrapRemainingDurability { get; set; }
00178:             public bool TrapCatch { get; set; }
00179:             public float DigProgress { get; set; }
00180:             public float DigRisk { get; set; }
00181:             public bool TelemetryActive { get; set; }
00182:             public float CellDurability { get; set; }
00183:             public int DebtDaysRemaining { get; set; }
00184:             public int AssignmentCount { get; set; }
00185:
00186:             public string ComputeDeterministicHash()
00187:             {
00188:                 return $"{SkillXp:F1}_{HasActiveSkill}_{ResearchCompletedCount}_{ExpeditionCount}_" +
00189:                        $"{PackCount}_{PackSector}_{TrapRemainingDurability}_{TrapCatch}_" +
00190:                        $"{DigProgress:F1}_{DigRisk:F2}_{TelemetryActive}_{CellDurability:F1}_" +
00191:                        $"{DebtDaysRemaining}_{AssignmentCount}";
00192:             }
00193:         }
00194:
00195:         private static CampaignExecutionSnapshot ExecuteDeterministicCampaignJourney(int masterSeed)
00196:         {
00197:             string dataDir = ResolveDataDir();
00198:             var fileIO = new FileSystemIO();
00199:             var serializer = new SystemTextJsonSerializer();
00200:
00201:             // Step 1: Start fresh seeded campaign
00202:             var rng = new SeededRng(masterSeed);
00203:
00204:             // Step 2: Inspect canonical skill and research definitions
00205:             var skillSys = new SkillProgressionSystem();
00206:             SkillCatalogLoader.LoadAndRegister(skillSys, dataDir, fileIO, serializer);
00207:
00208:             var researchNodes = ResearchKnowledgeCatalogLoader.Load(dataDir, fileIO, serializer);
00209:             var researchSys = new ResearchSystem();
00210:             foreach (var n in researchNodes) researchSys.Register(n);
00211:
00212:             // Step 3: Gain/progress at least one migrated skill
00213:             var actor = new TestSurvivorActor("survivor_audrey");
00214:             for (int i = 0; i < 11; i++)
00215:             {
00216:                 skillSys.RecordAction(actor, "medical", SkillProgressionSystem.DefaultXpPerAction, 1);
00217:             }
00218:             float skillXp = skillSys.GetXp("survivor_audrey", "medical");
00219:             bool hasFieldDressing = skillSys.HasActiveSkill("survivor_audrey", "skill_field_dressing");
00220:
00221:             // Step 4: Unlock or advance one research node
00222:             researchSys.UnlockManual("knowledge_field_medicine");
00223:             int researchCompleted = researchSys.State.unlockedIds.Count;
00224:
00225:             // Step 5: Dispatch to one newly wired expedition location
00226:             var expeditions = ExpeditionCatalogLoader.Load(dataDir, fileIO, serializer);
00227:             var weighbridge = expeditions.FirstOrDefault(e => e.id == "loc_weighbridge") ?? expeditions.First();
00228:
00229:             // Step 6: Advance wildlife season/migration state
00230:             var wildSys = new WildlifeMigrationSystem(rng);
00231:             wildSys.RegisterPack("pack_rad_dogs_alpha", "species_rad_dog", "sector_ruins_north", population: 6);
00232:             wildSys.SetSectorAdjacency(new[]
00233:             {
00234:                 ("sector_ruins_north", new List<string> { "sector_subway_concourse" }),
00235:                 ("sector_subway_concourse", new List<string> { "sector_ruins_north" })
00236:             });
00237:             wildSys.MigratePack("pack_rad_dogs_alpha", "sector_subway_concourse");
00238:
00239:             // Step 7: Set a trap and resolve catch
00240:             var trapSys = new WildlifeTrappingSystem(rng);
00241:             var trapCatalog = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, serializer);
00242:             trapCatalog.RegisterWith(trapSys);
00243:             trapSys.SetTrap("site_edge_copse", "bait_grain_lure", "survivor_audrey", "snare", "trap_snare", 1, 4);
00244:             trapSys.TickDay(2);
00245:             var trapSite = trapSys.State.trapSites.FirstOrDefault(s => s.siteId == "site_edge_copse");
00246:
00247:             // Step 8 & 9: Dispatch to an excavation site, advance dig, apply shoring
00248:             var excavationSys = new ExcavationSystem(rng);
00249:             excavationSys.AddSite("excavation_utility_tunnels", "loc_excavation_utility_tunnels", 95f, 0.45f);
00250:             excavationSys.AssignWorkers("excavation_utility_tunnels", 2);
00251:             excavationSys.TickDay();
00252:             excavationSys.ApplyShoring("excavation_utility_tunnels");
00253:             var digSite = excavationSys.State.sites.First(s => s.siteId == "excavation_utility_tunnels");
00254:
00255:             // Step 10 & 11: Trigger telemetry event, verify armor config responds to threat
00256:             var skyArmor = new SkyLayerArmorSystem();
00257:             skyArmor.SetCellArmor(5, CeilingMaterialTier.ReinforcedConcrete, 0.8f);
00258:             var harrowSys = new OrbitalHarrowTelemetrySystem(skyArmor, rng);
00259:             harrowSys.ActivateTelemetry(day: 2);
00260:             harrowSys.ScheduleImpact(5, 5, 25f);
00261:             harrowSys.Brace("concrete", 4);
00262:             harrowSys.TickDay(5);
00263:             var cell = skyArmor.GetCell(5);
00264:
00265:             // Step 12 & 13: Create debt from template and advance term
00266:             var debtSys = new LedgerDebtSystem();
00267:             debtSys.PresentContract("survivor_audrey", 8f, termDays: 20, rate: 0.15f, "eight tins of sealed rations");
00268:             debtSys.PresentContract("survivor_audrey", 8f, termDays: 20, rate: 0.15f, "eight tins of sealed rations");
00269:             debtSys.SignContract("survivor_audrey", 2);
00270:             debtSys.TickDaily(3);
00271:             var contract = debtSys.GetContract("survivor_audrey");
00272:
00273:             // Step 14: Assign survivor to new room using skill/rule prerequisites
00274:             var rooms = new List<ShelterRoom>
00275:             {
00276:                 new ShelterRoom("room_clinic", "Clinic", 2, "skill_field_dressing"),
00277:                 new ShelterRoom("room_workshop", "Workshop", 2, "skill_rough_repairs")
00278:             };
00279:             var assignmentSys = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, rng);
00280:             assignmentSys.Assign("survivor_audrey", "room_clinic", day: 3);
00281:
00282:             // Step 15 & 16: Save all system states and reload into fresh instances
00283:             var skillState = skillSys.CaptureState();
00284:             var researchState = researchSys.CaptureState();
00285:             var wildState = wildSys.State;
00286:             var trapState = trapSys.CaptureState();
00287:             var digState = excavationSys.CaptureState();
00288:             var harrowState = harrowSys.CaptureState();
00289:             var debtState = debtSys.CaptureState();
00290:             var assignmentState = assignmentSys.CaptureState();
00291:
00292:             // Reload fresh systems
00293:             var reloadedSkill = new SkillProgressionSystem();
00294:             reloadedSkill.RestoreState(skillState);
00295:
00296:             var reloadedResearch = new ResearchSystem();
00297:             foreach (var n in researchNodes) reloadedResearch.Register(n);
00298:             reloadedResearch.RestoreState(researchState);
00299:
00300:             var reloadedDig = new ExcavationSystem(rng);
00301:             reloadedDig.RestoreState(digState);
00302:
00303:             var reloadedDebt = new LedgerDebtSystem();
00304:             reloadedDebt.RestoreState(debtState);
00305:
00306:             var reloadedAssignment = new ShelterAssignmentSystem(assignmentState, rooms, rng);
00307:
00308:             // Step 17: Verify all affected states match restored instances
00309:             Assert.Equal(skillXp, reloadedSkill.GetXp("survivor_audrey", "medical"));
00310:             Assert.True(reloadedResearch.IsManualUnlocked("knowledge_field_medicine"));
00311:             Assert.Equal(digSite.progress, reloadedDig.State.sites[0].progress);
00312:             Assert.Equal(contract.daysRemaining, reloadedDebt.GetContract("survivor_audrey").daysRemaining);
00313:             Assert.Single(reloadedAssignment.GetAssignments());
00314:
00315:             // Step 18: Build snapshot for deterministic hash comparison
00316:             return new CampaignExecutionSnapshot
00317:             {
00318:                 SkillXp = skillXp,
00319:                 HasActiveSkill = hasFieldDressing,
00320:                 ResearchCompletedCount = researchCompleted,
00321:                 ExpeditionCount = expeditions.Count,
00322:                 PackCount = wildSys.State.packs.Count,
00323:                 PackSector = wildSys.State.packs[0].currentSectorId,
00324:                 TrapRemainingDurability = trapSite?.remainingDurability ?? 0,
00325:                 TrapCatch = trapSite?.hasCatch ?? false,
00326:                 DigProgress = digSite.progress,
00327:                 DigRisk = digSite.structuralRisk,
00328:                 TelemetryActive = harrowSys.State.telemetryActive,
00329:                 CellDurability = cell?.currentDurability ?? 0f,
00330:                 DebtDaysRemaining = contract.daysRemaining,
00331:                 AssignmentCount = reloadedAssignment.GetAssignments().Count
00332:             };
00333:         }
00334:
00335:         [Fact]
00336:         public void Test2_FullDeterministicCampaignJourney_TouchesAllTenSystems_ReplaysIdentically()
00337:         {
00338:             // Step 19 & 20: Repeat entire journey from seed 42 in independent run and compare hashes
00339:             var runA = ExecuteDeterministicCampaignJourney(42);
00340:             var runB = ExecuteDeterministicCampaignJourney(42);
00341:
00342:             Assert.Equal(runA.ComputeDeterministicHash(), runB.ComputeDeterministicHash());
00343:             Assert.Equal(runA.SkillXp, runB.SkillXp);
00344:             Assert.Equal(runA.HasActiveSkill, runB.HasActiveSkill);
00345:             Assert.Equal(runA.ResearchCompletedCount, runB.ResearchCompletedCount);
00346:             Assert.Equal(runA.PackSector, runB.PackSector);
00347:             Assert.Equal(runA.DigProgress, runB.DigProgress);
00348:             Assert.Equal(runA.DebtDaysRemaining, runB.DebtDaysRemaining);
00349:             Assert.Equal(runA.AssignmentCount, runB.AssignmentCount);
00350:         }
00351:
00352:         [Fact]
00353:         public void Test3_Batch1CatalogCountsAndLoaderClassifications_MatchAuthoritativeTruth()
00354:         {
00355:             string dataDir = ResolveDataDir();
00356:             var fileIO = new FileSystemIO();
00357:             var serializer = new SystemTextJsonSerializer();
00358:
00359:             // Plan 32: Expeditions (L1 generic loader, 55 destinations)
00360:             var expeditions = ExpeditionCatalogLoader.Load(dataDir, fileIO, serializer);
00361:             Assert.True(expeditions.Count >= 50, $"Plan 32 target >= 50, got {expeditions.Count}");
00362:
00363:             // Plan 33: Skills (L2 mechanical loader, 160 skills)
00364:             var skills = SkillCatalogLoader.Load(dataDir, fileIO, serializer);
00365:             Assert.True(skills.Count >= 50, $"Plan 33 target >= 50, got {skills.Count}");
00366:
00367:             // Plan 34: Research (L2 mechanical loader, 31+ nodes)
00368:             var research = ResearchKnowledgeCatalogLoader.Load(dataDir, fileIO, serializer);
00369:             Assert.True(research.Count >= 30, $"Plan 34 target >= 30, got {research.Count}");
00370:
00371:             // Plan 35: Wildlife Ecosystem / Seeds (L1 generic loader)
00372:             var seeds = EvolvingWorldCatalogLoader.Load(dataDir, fileIO, serializer);
00373:             Assert.NotNull(seeds);
00374:             Assert.True(seeds!.packs.Count >= 12, $"Plan 35 target >= 12 packs, got {seeds.packs.Count}");
00375:
00376:             // Plan 36: Trapping (L2 mechanical loader, 10 traps + 15 prey)
00377:             var trapping = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, serializer);
00378:             Assert.NotNull(trapping);
00379:             Assert.True(trapping.Traps.Count >= 10, $"Plan 36 target >= 10 traps, got {trapping.Traps.Count}");
00380:             Assert.True(trapping.Prey.Count >= 15, $"Plan 36 target >= 15 prey, got {trapping.Prey.Count}");
00381:
00382:             // Plan 37: Excavation (L2 mechanical loader, 8 sites)
00383:             var excavation = ExcavationCatalogLoader.Load(dataDir, fileIO, serializer);
00384:             Assert.NotNull(excavation);
00385:             Assert.Equal(8, excavation.Count);
00386:
00387:             // Plan 38: Sky-Layer Armor (L2 mechanical loader, 6 configs)
00388:             var armor = SkyLayerArmorCatalogLoader.Load(dataDir, fileIO, serializer);
00389:             Assert.NotNull(armor);
00390:             Assert.Equal(6, armor.Count);
00391:
00392:             // Plan 39: Orbital Harrow (L2 mechanical loader, 12 events)
00393:             var harrow = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, serializer);
00394:             Assert.NotNull(harrow);
00395:             Assert.Equal(12, harrow.Count);
00396:
00397:             // Plan 40: Ledger Debt (L2 mechanical loader, 15 templates + 10 consequences)
00398:             var debt = DebtTemplateCatalogLoader.Load(dataDir, fileIO, serializer);
00399:             Assert.NotNull(debt);
00400:             Assert.Equal(15, debt.Templates.Count);
00401:             Assert.Equal(10, debt.Consequences.Count);
00402:
00403:             // Plan 41: Shelter Rooms (L2 mechanical loader, 23 rooms + 12 rules)
00404:             var rooms = ShelterRoomCatalogLoader.Load(dataDir);
00405:             Assert.NotNull(rooms);
00406:             Assert.True(rooms.rooms.Count >= 20, $"Plan 41 target >= 20 rooms, got {rooms.rooms.Count}");
00407:             Assert.Equal(12, rooms.assignment_rules.Count);
00408:         }
00409:     }
00410: }
```

## `src/Host/HostCli.DynamicWorld.cs` — 187 lines; 11,392 bytes; SHA-256 `264e923774ae6e1c35b80f1445e01a04c4419f71fecb044287e8c0f8fba8af05`
Declaration index:
- 00013: public static partial class HostCli
- 00024: public static int RunDynamicWorldSelfTest(string dataDirectory)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Collections.Generic;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.IO;
00008: using Ashfall.Core.Shelter;
00009: using Ashfall.Core.World;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     public static partial class HostCli
00014:     {
00015:         /// <summary>
00016:         /// --dynamic-world-selftest / --plan19-selftest:
00017:         /// Verifies Plan 19 Dynamic World Systems:
00018:         /// Weather forecasting lookahead, weather station tiers (offline, damaged, functional, calibrated),
00019:         /// 6-phase seasonal calendar, seasonal events (18+ events),
00020:         /// Orbital Harrow kinetic strike event templates (5 templates),
00021:         /// Sky armor impact cascades, salvage opportunity generation, site reveals,
00022:         /// and save/load persistence.
00023:         /// </summary>
00024:         public static int RunDynamicWorldSelfTest(string dataDirectory)
00025:         {
00026:             CatalogLocator.UseInvariantCulture();
00027:             int failures = 0;
00028:             int totalAssertions = 0;
00029:
00030:             void Check(bool ok, string label)
00031:             {
00032:                 totalAssertions++;
00033:                 GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
00034:                 if (!ok) failures++;
00035:             }
00036:
00037:             GD.Print("[DynamicWorldHeadlessDemo] begin Plan 19 verification...");
00038:
00039:             var json = new SystemTextJsonSerializer();
00040:             var files = new FileSystemIO();
00041:
00042:             // 1. Weather Seasons & 10 Phases (weather_seasons.json, Plan 83)
00043:             var expectedWindows = new[]
00044:             {
00045:                 "window_first_thaw", "window_ash_settling", "window_deep_freeze",
00046:                 "window_spring_storms", "window_dry_ash", "window_first_fallout",
00047:                 "window_false_spring", "window_deep_ash", "window_long_winter",
00048:                 "window_black_rain_season"
00049:             };
00050:             var seasonProfile = WeatherProfileLoader.Load(dataDirectory, files, json);
00051:             Check(seasonProfile != null, "Weather seasons profile loaded");
00052:             Check(seasonProfile != null && seasonProfile.seasons.Count >= 10, $"Season phase count (expected >= 10, got {seasonProfile?.seasons.Count ?? 0})");
00053:             foreach (var windowId in expectedWindows)
00054:             {
00055:                 Check(seasonProfile != null && seasonProfile.seasons.Any(s => s.id == windowId), $"Season phase {windowId} present");
00056:             }
00057:
00058:             // 2. Weather System & Non-mutating Lookahead
00059:             var weather = new WeatherSystem();
00060:             if (seasonProfile != null)
00061:                 weather.BindProfile(seasonProfile, seed: 12345);
00062:
00063:             int initialRoll = weather.State.rollCount;
00064:             var peek = weather.PeekForecast(7);
00065:             Check(peek.Count == 7, "WeatherSystem.PeekForecast(7) returns 7 days");
00066:             Check(weather.State.rollCount == initialRoll, "PeekForecast does not mutate simulation rollCount");
00067:
00068:             // 3. Weather Station Tiers & Forecasting
00069:             var station = new WeatherStationSystem(weather, new SeededRng(12345));
00070:             Check(station.CurrentTier == WeatherStationTier.Offline, "Uninstalled station is Tier Offline");
00071:             Check(station.EffectiveHorizonDays == 0, "Offline station horizon is 0 days");
00072:
00073:             station.Install(1);
00074:             Check(station.CurrentTier == WeatherStationTier.Functional, "Installed station is Tier Functional");
00075:             Check(station.State.isInstalled, "Station marked installed");
00076:
00077:             station.Calibrate(2);
00078:             Check(station.CurrentTier == WeatherStationTier.Calibrated, "Calibrated station is Tier Calibrated");
00079:             Check(station.EffectiveHorizonDays == 7, "Calibrated station horizon is 7 days");
00080:
00081:             var forecastRes = station.GenerateForecast(3);
00082:             Check(forecastRes.Status == ActionResult.StatusKind.Success, "Calibrated forecast generated successfully");
00083:             Check(station.GetForecast().Count == 7, "Forecast contains 7 entries");
00084:             Check(!string.IsNullOrEmpty(station.GetForecast()[0].preparationPayoff), "Forecast entries contain actionable preparation payoffs");
00085:             Check(!string.IsNullOrEmpty(station.GetForecast()[0].atmosphericFlavor), "Forecast entries contain atmospheric flavor");
00086:
00087:             // Station degradation
00088:             station.Degrade(70f);
00089:             Check(station.CurrentTier == WeatherStationTier.Damaged, "Degraded station drops to Tier Damaged");
00090:             station.GenerateForecast(4);
00091:             Check(station.GetForecast().Count == 1, "Damaged station horizon restricted to 1 day");
00092:
00093:             // 4. Orbital Harrow Event Templates (orbital_harrow_events.json)
00094:             var orbitalEvents = OrbitalHarrowCatalogLoader.Load(dataDirectory, files, json);
00095:             Check(orbitalEvents.Count >= 5, $"Orbital Harrow event templates count (expected >= 5, got {orbitalEvents.Count})");
00096:             Check(orbitalEvents.Any(e => e.id == "event_orbital_small_debris_shower"), "Template event_orbital_small_debris_shower present");
00097:             Check(orbitalEvents.Any(e => e.id == "event_orbital_heavy_kinetic_impact"), "Template event_orbital_heavy_kinetic_impact present");
00098:             Check(orbitalEvents.Any(e => e.id == "event_orbital_clustered_impact"), "Template event_orbital_clustered_impact present");
00099:             Check(orbitalEvents.Any(e => e.id == "event_orbital_near_miss_shockwave"), "Template event_orbital_near_miss_shockwave present");
00100:             Check(orbitalEvents.Any(e => e.id == "event_orbital_low_warning_strike"), "Template event_orbital_low_warning_strike present");
00101:
00102:             // 5. Orbital Harrow Telemetry & Sky Armor Impact Cascades
00103:             var armor = new SkyLayerArmorSystem();
00104:             armor.SetCellArmor(10, CeilingMaterialTier.ReinforcedConcrete, 1.5f, 100f);
00105:             armor.SetCellArmor(11, CeilingMaterialTier.ReinforcedConcrete, 1.5f, 100f);
00106:
00107:             var orbital = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(54321));
00108:             orbital.ActivateTelemetry(1);
00109:             Check(orbital.State.telemetryActive, "Orbital telemetry activated");
00110:
00111:             var heavyStrike = orbitalEvents.Find(e => e.id == "event_orbital_heavy_kinetic_impact");
00112:             if (heavyStrike != null)
00113:                 orbital.ScheduleEventDef(heavyStrike, day: 10, gridX: 10);
00114:
00115:             Check(orbital.HasPendingImpact, "Pending impact registered");
00116:             Check(orbital.State.warnings.Count >= 1, "Impact warning generated");
00117:
00118:             // Brace impact
00119:             var braceRes = orbital.Brace("concrete", 5);
00120:             Check(braceRes.Status == ActionResult.StatusKind.Success, "Bracing successful");
00121:             Check(orbital.State.isBraced, "Braced status active");
00122:
00123:             // Resolve impact on Day 10
00124:             OrbitalImpactReport? impactReport = null;
00125:             orbital.OnImpactDetailed += rep => impactReport = rep;
00126:             orbital.TickDay(10);
00127:
00128:             Check(!orbital.HasPendingImpact, "Impact resolved on Day 10");
00129:             Check(impactReport != null, "Detailed impact report generated");
00130:             Check(orbital.ActiveSalvage.Count > 0, "Post-strike salvage opportunity spawned");
00131:             Check(orbital.RevealedSites.Contains("loc_excavation_command_vault"), "Rare site revealed from strike");
00132:
00133:             // Claim salvage
00134:             var claimRes = orbital.ClaimSalvage("event_orbital_heavy_kinetic_impact");
00135:             Check(claimRes.Status == ActionResult.StatusKind.Success, "Salvage claimed successfully");
00136:
00137:             // 6. Seasonal Events (seasonal_events.json)
00138:             var seasonalEvents = SeasonalEventCatalogLoader.Load(dataDirectory, files, json);
00139:             Check(seasonalEvents.Count >= 18, $"Seasonal events count (expected >= 18, got {seasonalEvents.Count})");
00140:             Check(seasonalEvents.Any(e => e.id == "event_season_ash_filter_clog"), "Event event_season_ash_filter_clog present");
00141:             Check(seasonalEvents.Any(e => e.id == "event_season_freeze_pipe_burst"), "Event event_season_freeze_pipe_burst present");
00142:             Check(seasonalEvents.Any(e => e.id == "event_season_thaw_sump_flood"), "Event event_season_thaw_sump_flood present");
00143:             Check(seasonalEvents.Any(e => e.id == "event_season_bloom_greenhouse_spores"), "Event event_season_bloom_greenhouse_spores present");
00144:             Check(seasonalEvents.Any(e => e.id == "event_season_highcold_generator_stall"), "Event event_season_highcold_generator_stall present");
00145:             Check(seasonalEvents.Any(e => e.id == "event_season_turning_clear_sky_window"), "Event event_season_turning_clear_sky_window present");
00146:
00147:             var seasonalSys = new SeasonalEventSystem();
00148:             seasonalSys.BindDefinitions(seasonalEvents);
00149:             seasonalSys.TickDay(1, "window_first_thaw", new SeededRng(1111));
00150:
00151:             // 7. Weather Intelligence Coordinator & Save Round-Trip
00152:             var coord = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(777));
00153:             coord.Seasonal.BindDefinitions(seasonalEvents);
00154:             coord.Station.Install(1);
00155:             coord.Station.Calibrate(2);
00156:             coord.Orbital.ActivateTelemetry(1);
00157:             coord.Orbital.ScheduleImpact(15, 3, 25f);
00158:             coord.TickDay(3);
00159:
00160:             var saveState = coord.CaptureState();
00161:             Check(saveState.station.isInstalled, "Save captures station state");
00162:             Check(saveState.orbital.telemetryActive, "Save captures orbital telemetry state");
00163:
00164:             var saveJson = json.Serialize(saveState);
00165:             var restoredState = json.Deserialize<WeatherIntelligenceSaveState>(saveJson);
00166:             Check(restoredState != null, "WeatherIntelligenceSaveState deserializes cleanly");
00167:
00168:             var coordRestored = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(888));
00169:             coordRestored.RestoreState(restoredState);
00170:             var readModel = coordRestored.BuildReadModel();
00171:
00172:             Check(readModel.stationInstalled && readModel.stationCalibrated, "Restored coordinator station is operational");
00173:             Check(readModel.telemetryActive && readModel.hasPendingImpact, "Restored coordinator orbital telemetry is active with pending impact");
00174:             Check(!string.IsNullOrEmpty(readModel.advisory), "Restored coordinator read model advisory is populated");
00175:
00176:             GD.Print($"[DynamicWorldHeadlessDemo] {(failures == 0 ? "PASS" : "FAIL")} {totalAssertions - failures}/{totalAssertions}");
00177:             string status = failures == 0 ? "PASS" : "FAIL";
00178:             GD.Print($"[HOST_SELFTEST] dynamic_world_selftest {status}");
00179:             GD.Print($"[HOST_SELFTEST_SUMMARY] test=dynamic_world_selftest status={status} exit_code={(failures == 0 ? 0 : 1)} passed={totalAssertions - failures} failed={failures} total={totalAssertions} details=\"[DynamicWorldHeadlessDemo] {status} {totalAssertions - failures}/{totalAssertions}\"");
00180:             GD.Print($"[HOST_SELFTEST_JSON] {{\"test\":\"dynamic_world_selftest\",\"status\":\"{status}\",\"exit_code\":{(failures == 0 ? 0 : 1)},\"passed\":{totalAssertions - failures},\"failed\":{failures},\"total\":{totalAssertions},\"details\":\"[DynamicWorldHeadlessDemo] {status} {totalAssertions - failures}/{totalAssertions}\"}}");
00181:             GD.Print($"SELFTEST {status}: dynamic_world_selftest");
00182:             GD.Print($"DYNAMIC_WORLD_SELFTEST {status}");
00183:
00184:             return failures == 0 ? 0 : 1;
00185:         }
00186:     }
00187: }
```

## `Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs` — 282 lines; 13,480 bytes; SHA-256 `bb49504442f792a1d3a4a55c962a168047d4ebaa12253c41c01911e15c70aab4`
Declaration index:
- 00013: public class Plan19DynamicWorldTests
- 00017: private static WeatherSystem CreateWeather(int seed = 42)
- 00031: public void WeatherForecast_PeekDoesNotMutateWeatherState()
- 00047: public void WeatherForecast_SameSeedProducesDeterministicForecast()
- 00066: public void WeatherStation_TierProgression_AffectsHorizonAndConfidence()
- 00102: public void WeatherStation_PreparationPayoffs_ProvideActionableAdvice()
- 00120: public void OrbitalCatalog_LoadsAllFiveTemplates()
- 00132: public void OrbitalImpact_EvaluatesSkyArmor_GeneratesSalvageAndRevealsSite()
- 00179: public void SeasonModel_DefinesTenPhasesAcrossYear()
- 00207: public void SeasonalEvents_LoadCatalog_AndTriggerDeterministically()
- 00237: public void WeatherIntelligenceCoordinator_SaveRestoreRoundTrip_PreservesAllStates()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Ashfall.Core;
00006: using Ashfall.Core.IO;
00007: using Ashfall.Core.Shelter;
00008: using Ashfall.Core.World;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.World
00012: {
00013:     public class Plan19DynamicWorldTests
00014:     {
00015:         private static readonly string DataDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
00016:
00017:         private static WeatherSystem CreateWeather(int seed = 42)
00018:         {
00019:             var ws = new WeatherSystem();
00020:             var profile = WeatherProfileLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer())
00021:                           ?? new SeasonProfileDef();
00022:             ws.BindProfile(profile, seed);
00023:             return ws;
00024:         }
00025:
00026:         // ────────────────────────────────────────────────────────────────────────
00027:         // 1. Weather Forecasting Core & Station Tiers (Tasks 19A, 19B, 19D, 19V)
00028:         // ────────────────────────────────────────────────────────────────────────
00029:
00030:         [Fact]
00031:         public void WeatherForecast_PeekDoesNotMutateWeatherState()
00032:         {
00033:             var weather = CreateWeather(12345);
00034:             var initialRoll = weather.State.rollCount;
00035:             var initialHours = weather.State.totalElapsedHours;
00036:             var initialKind = weather.Current;
00037:
00038:             var forecast = weather.PeekForecast(7);
00039:
00040:             Assert.Equal(7, forecast.Count);
00041:             Assert.Equal(initialRoll, weather.State.rollCount);
00042:             Assert.Equal(initialHours, weather.State.totalElapsedHours);
00043:             Assert.Equal(initialKind, weather.Current);
00044:         }
00045:
00046:         [Fact]
00047:         public void WeatherForecast_SameSeedProducesDeterministicForecast()
00048:         {
00049:             var w1 = CreateWeather(9999);
00050:             var w2 = CreateWeather(9999);
00051:
00052:             var f1 = w1.PeekForecast(7);
00053:             var f2 = w2.PeekForecast(7);
00054:
00055:             Assert.Equal(f1.Count, f2.Count);
00056:             for (int i = 0; i < f1.Count; i++)
00057:             {
00058:                 Assert.Equal(f1[i].Day, f2[i].Day);
00059:                 Assert.Equal(f1[i].Kind, f2[i].Kind);
00060:                 Assert.Equal(f1[i].OutdoorRad, f2[i].OutdoorRad);
00061:                 Assert.Equal(f1[i].Visibility, f2[i].Visibility);
00062:             }
00063:         }
00064:
00065:         [Fact]
00066:         public void WeatherStation_TierProgression_AffectsHorizonAndConfidence()
00067:         {
00068:             var weather = CreateWeather(5555);
00069:             var station = new WeatherStationSystem(weather, new SeededRng(5555));
00070:
00071:             // 1. Offline tier
00072:             Assert.Equal(WeatherStationTier.Offline, station.CurrentTier);
00073:             Assert.Equal(0, station.EffectiveHorizonDays);
00074:             var resOffline = station.GenerateForecast(1);
00075:             Assert.Equal(ActionResult.StatusKind.Blocked, resOffline.Status);
00076:
00077:             // 2. Uncalibrated tier (Installed, uncalibrated - offline from forecasting)
00078:             station.Install(1);
00079:             Assert.Equal(WeatherStationTier.Functional, station.CurrentTier);
00080:             var resFunc = station.GenerateForecast(1);
00081:             Assert.Equal(ActionResult.StatusKind.Blocked, resFunc.Status);
00082:
00083:             // 3. Calibrated tier
00084:             station.Calibrate(2);
00085:             Assert.Equal(WeatherStationTier.Calibrated, station.CurrentTier);
00086:             Assert.Equal(7, station.EffectiveHorizonDays);
00087:             var resCal = station.GenerateForecast(2);
00088:             Assert.Equal(ActionResult.StatusKind.Success, resCal.Status);
00089:             Assert.Equal(7, station.GetForecast().Count);
00090:             Assert.True(station.GetForecast()[0].confidence > 0.70f);
00091:
00092:             // 4. Damaged tier (degraded durability)
00093:             station.Degrade(70f); // durability becomes 30f < 40f
00094:             Assert.Equal(WeatherStationTier.Damaged, station.CurrentTier);
00095:             Assert.Equal(1, station.EffectiveHorizonDays);
00096:             station.GenerateForecast(3);
00097:             Assert.Single(station.GetForecast());
00098:             Assert.True(station.GetForecast()[0].confidence <= 0.40f);
00099:         }
00100:
00101:         [Fact]
00102:         public void WeatherStation_PreparationPayoffs_ProvideActionableAdvice()
00103:         {
00104:             var stormAdvice = WeatherStationSystem.GetPreparationPayoff(WeatherKind.FalloutStorm);
00105:             var blackRainAdvice = WeatherStationSystem.GetPreparationPayoff(WeatherKind.BlackRain);
00106:             var blizzardAdvice = WeatherStationSystem.GetPreparationPayoff(WeatherKind.Blizzard);
00107:             var clearAdvice = WeatherStationSystem.GetPreparationPayoff(WeatherKind.Clear);
00108:
00109:             Assert.Contains("filters", stormAdvice, StringComparison.OrdinalIgnoreCase);
00110:             Assert.Contains("cisterns", blackRainAdvice, StringComparison.OrdinalIgnoreCase);
00111:             Assert.Contains("heating", blizzardAdvice, StringComparison.OrdinalIgnoreCase);
00112:             Assert.Contains("overland", clearAdvice, StringComparison.OrdinalIgnoreCase);
00113:         }
00114:
00115:         // ────────────────────────────────────────────────────────────────────────
00116:         // 2. Orbital Harrow Telemetry & Kinetic Impacts (Tasks 19F–19M)
00117:         // ────────────────────────────────────────────────────────────────────────
00118:
00119:         [Fact]
00120:         public void OrbitalCatalog_LoadsAllFiveTemplates()
00121:         {
00122:             var list = OrbitalHarrowCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00123:             Assert.True(list.Count >= 5);
00124:             Assert.Contains(list, e => e.id == "event_orbital_kinetic_early_track");
00125:             Assert.Contains(list, e => e.id == "event_orbital_kinetic_thermal_descent");
00126:             Assert.Contains(list, e => e.id == "event_orbital_kinetic_seismic_precursor");
00127:             Assert.Contains(list, e => e.id == "event_orbital_kinetic_fragmented_track");
00128:             Assert.Contains(list, e => e.id == "event_orbital_cluster_multiple_returns");
00129:         }
00130:
00131:         [Fact]
00132:         public void OrbitalImpact_EvaluatesSkyArmor_GeneratesSalvageAndRevealsSite()
00133:         {
00134:             var armor = new SkyLayerArmorSystem();
00135:             armor.SetCellArmor(5, CeilingMaterialTier.ReinforcedConcrete, 1.5f, 100f);
00136:             armor.SetCellArmor(6, CeilingMaterialTier.ReinforcedConcrete, 1.5f, 100f);
00137:
00138:             var orbital = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(777));
00139:             orbital.ActivateTelemetry(1);
00140:
00141:             var heavyEvent = new OrbitalEventDef
00142:             {
00143:                 id = "event_orbital_heavy_kinetic_impact",
00144:                 name = "Tungsten Penetrator Plunge",
00145:                 severity = "Severe",
00146:                 impact_energy_mj = 50f,
00147:                 affected_cell_spread = 2,
00148:                 salvage_yield_item_id = "scrap_electronic",
00149:                 salvage_yield_quantity = 6,
00150:                 revealed_site_id = "loc_excavation_command_vault"
00151:             };
00152:
00153:             orbital.ScheduleEventDef(heavyEvent, day: 5, gridX: 5);
00154:             Assert.True(orbital.HasPendingImpact);
00155:
00156:             OrbitalImpactReport? report = null;
00157:             orbital.OnImpactDetailed += r => report = r;
00158:
00159:             orbital.TickDay(5);
00160:
00161:             Assert.False(orbital.HasPendingImpact);
00162:             Assert.NotNull(report);
00163:             Assert.Equal(5, report!.Day);
00164:             Assert.Equal("event_orbital_heavy_kinetic_impact", report.EventId);
00165:             Assert.Equal(2, report.CellsAffected);
00166:             Assert.NotEmpty(orbital.ActiveSalvage);
00167:             Assert.Contains("loc_excavation_command_vault", orbital.RevealedSites);
00168:
00169:             // Claim salvage
00170:             var claimRes = orbital.ClaimSalvage("event_orbital_heavy_kinetic_impact");
00171:             Assert.Equal(ActionResult.StatusKind.Success, claimRes.Status);
00172:         }
00173:
00174:         // ────────────────────────────────────────────────────────────────────────
00175:         // 3. Seasonal Phase Model & Event Cadence (Tasks 19N–19T)
00176:         // ────────────────────────────────────────────────────────────────────────
00177:
00178:         [Fact]
00179:         public void SeasonModel_DefinesTenPhasesAcrossYear()
00180:         {
00181:             // Plan 83 expanded the Plan 19 six-phase model to ten windows.
00182:             var weather = CreateWeather(100);
00183:             var expected = new (int day, string id)[]
00184:             {
00185:                 (0, "window_first_thaw"),
00186:                 (30, "window_ash_settling"),
00187:                 (75, "window_deep_freeze"),
00188:                 (100, "window_spring_storms"),
00189:                 (140, "window_dry_ash"),
00190:                 (165, "window_first_fallout"),
00191:                 (190, "window_false_spring"),
00192:                 (220, "window_deep_ash"),
00193:                 (260, "window_long_winter"),
00194:                 (320, "window_black_rain_season"),
00195:             };
00196:
00197:             var profile = WeatherProfileLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00198:             Assert.NotNull(profile);
00199:             Assert.Equal(10, profile!.seasons.Count);
00200:             foreach (var (day, id) in expected)
00201:             {
00202:                 Assert.Equal(id, weather.GetSeasonForDay(day).id);
00203:             }
00204:         }
00205:
00206:         [Fact]
00207:         public void SeasonalEvents_LoadCatalog_AndTriggerDeterministically()
00208:         {
00209:             var events = SeasonalEventCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00210:             Assert.True(events.Count >= 18);
00211:
00212:             var system = new SeasonalEventSystem();
00213:             system.BindDefinitions(events);
00214:
00215:             var rng = new SeededRng(4242);
00216:             // Simulate 60 days in the ash-settling window (Plan 83: legacy "Ash Fall" events)
00217:             for (int day = 1; day <= 60; day++)
00218:             {
00219:                 system.TickDay(day, "window_ash_settling", rng);
00220:             }
00221:
00222:             Assert.NotEmpty(system.ActiveEvents);
00223:             var active = system.ActiveEvents[0];
00224:             Assert.False(active.isMitigated);
00225:
00226:             // Mitigate event
00227:             var mitRes = system.Mitigate(active.eventId);
00228:             Assert.Equal(ActionResult.StatusKind.Success, mitRes.Status);
00229:             Assert.True(active.isMitigated);
00230:         }
00231:
00232:         // ────────────────────────────────────────────────────────────────────────
00233:         // 4. Coordinator & Save Round-Trip (Tasks 19AC, 19AD, 19AJ)
00234:         // ────────────────────────────────────────────────────────────────────────
00235:
00236:         [Fact]
00237:         public void WeatherIntelligenceCoordinator_SaveRestoreRoundTrip_PreservesAllStates()
00238:         {
00239:             var weather = CreateWeather(8888);
00240:             var armor = new SkyLayerArmorSystem();
00241:             armor.SetCellArmor(1, CeilingMaterialTier.LeadSheeting, 2.0f, 100f);
00242:
00243:             var coord = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(8888));
00244:             var events = SeasonalEventCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00245:             coord.Seasonal.BindDefinitions(events);
00246:
00247:             coord.Station.Install(1);
00248:             coord.Station.Calibrate(2);
00249:             coord.Station.GenerateForecast(3);
00250:
00251:             coord.Orbital.ActivateTelemetry(1);
00252:             coord.Orbital.ScheduleImpact(10, 1, 20f);
00253:
00254:             coord.TickDay(3);
00255:
00256:             var save = coord.CaptureState();
00257:             Assert.True(save.station.isInstalled);
00258:             Assert.True(save.station.isCalibrated);
00259:             Assert.True(save.orbital.telemetryActive);
00260:             Assert.Equal(10, save.orbital.nextImpactDay);
00261:
00262:             var json = new SystemTextJsonSerializer().Serialize(save);
00263:             var restoredSave = new SystemTextJsonSerializer().Deserialize<WeatherIntelligenceSaveState>(json);
00264:             Assert.NotNull(restoredSave);
00265:
00266:             var coord2 = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(9999));
00267:             coord2.RestoreState(restoredSave);
00268:
00269:             Assert.True(coord2.Station.State.isInstalled);
00270:             Assert.True(coord2.Station.State.isCalibrated);
00271:             Assert.True(coord2.Orbital.State.telemetryActive);
00272:             Assert.Equal(10, coord2.Orbital.State.nextImpactDay);
00273:
00274:             var readModel = coord2.BuildReadModel();
00275:             Assert.Equal(WeatherStationTier.Calibrated, readModel.stationTier);
00276:             Assert.True(readModel.telemetryActive);
00277:             Assert.True(readModel.hasPendingImpact);
00278:             Assert.NotEmpty(readModel.forecast);
00279:             Assert.NotEmpty(readModel.advisory);
00280:         }
00281:     }
00282: }
```

## `src/Host/HostCli.SkyDefense.cs` — 130 lines; 7,661 bytes; SHA-256 `8d76e2eb209afad5b1a8ba5c45de37f3db51f45eee11b933641c6020793f623f`
Declaration index:
- 00020: public static partial class HostCli
- 00024: public static int RunSkyDefenseSelfTest(string dataDirectory)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // HostCli Partial : Flagship Task 7 — Sky Defense Battery selftest
00004: // --sky-defense-selftest: telemetry track intake, magazine logistics,
00005: // deterministic volley, heat/hydraulics service, crew claim, save round-trip,
00006: // and player-panel construction/binding over the real Core authority.
00007: // ============================================================================
00008: using System;
00009: using System.Collections.Generic;
00010: using Ashfall.Core;
00011: using Ashfall.Core.Inventory;
00012: using Ashfall.Core.IO;
00013: using Ashfall.Core.SkyDefense;
00014: using Ashfall.Core.Shelter;
00015: using AtomicWar.GodotApp.UI;
00016: using Godot;
00017:
00018: namespace AtomicWar.GodotApp
00019: {
00020:     public static partial class HostCli
00021:     {
00022:         private const string SkyDefenseAmmoItem = "ammo_76mm_he_flak";
00023:
00024:         public static int RunSkyDefenseSelfTest(string dataDirectory)
00025:         {
00026:             int pass = 0, fail = 0;
00027:             var details = new List<string>();
00028:
00029:             void Check(string gate, bool ok, string note = "")
00030:             {
00031:                 if (ok) { pass++; details.Add($"  PASS {gate}"); }
00032:                 else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
00033:                 GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} sky_defense/{gate}");
00034:             }
00035:
00036:             try
00037:             {
00038:                 var inventory = new Inventory();
00039:                 var telemetry = new OrbitalHarrowTelemetrySystem(new SkyLayerArmorSystem(), new SeededRng(42));
00040:                 var defense = new SkyDefenseBatterySystem(42, inventory: inventory, telemetry: telemetry);
00041:
00042:                 var ordnance = SkyDefenseOrdnanceCatalogLoader.Load(dataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
00043:                 Check("catalog_loaded", ordnance.Count >= 1, $"rows={ordnance.Count}");
00044:                 defense.LoadOrdnanceCatalog(ordnance);
00045:
00046:                 var turret = defense.EnsureDefaultTurret();
00047:                 Check("default_turret", turret != null && defense.Turrets.Count >= 1);
00048:
00049:                 inventory.TryProduce(SkyDefenseAmmoItem, 20);
00050:                 inventory.TryProduce(SkyDefenseBatterySystem.ServiceOilItemId, 10);
00051:
00052:                 // ── Telemetry intake + dedup ────────────────────────────────
00053:                 telemetry.ScheduleImpact(30, 4, 12f);
00054:                 Check("track_intake", defense.Tracks.Count == 1, $"tracks={defense.Tracks.Count}");
00055:                 telemetry.ScheduleImpact(30, 4, 12f);
00056:                 Check("track_dedup", defense.Tracks.Count == 1, $"tracks={defense.Tracks.Count}");
00057:
00058:                 // ── Magazine logistics ──────────────────────────────────────
00059:                 var def = defense.GetOrdnance(SkyDefenseAmmoItem);
00060:                 int before = inventory.CountById(SkyDefenseAmmoItem);
00061:                 var load = defense.TryLoadMagazine(turret!.turret_id, SkyDefenseAmmoItem);
00062:                 Check("magazine_loaded", load.IsSuccess && turret.magazine_count == (def?.magazine_units ?? 0),
00063:                     $"mag={turret.magazine_count}");
00064:                 Check("magazine_atomic", inventory.CountById(SkyDefenseAmmoItem) == before - (def?.magazine_units ?? 0),
00065:                     $"inv={inventory.CountById(SkyDefenseAmmoItem)}");
00066:
00067:                 // ── Crew claim ──────────────────────────────────────────────
00068:                 var assign = defense.TryAssignCrew(turret.turret_id, "survivor_selftest_gunner");
00069:                 Check("crew_assign", assign.IsSuccess && turret.assigned_crew_ids.Contains("survivor_selftest_gunner"));
00070:                 var remove = defense.TryRemoveCrew(turret.turret_id, "survivor_selftest_gunner");
00071:                 Check("crew_remove", remove.IsSuccess && !turret.assigned_crew_ids.Contains("survivor_selftest_gunner"));
00072:
00073:                 // ── Deterministic volley ────────────────────────────────────
00074:                 int heatBefore = turret.barrel_heat;
00075:                 var volley = defense.TryFireVolley(turret.turret_id, "custom_impact");
00076:                 Check("volley_fired", volley.IsSuccess && defense.TotalVolleys == 1, $"volleys={defense.TotalVolleys}");
00077:                 Check("volley_cost", turret.magazine_count == (def?.magazine_units ?? 0) - 1 && turret.barrel_heat >= heatBefore,
00078:                     $"mag={turret.magazine_count} heat={turret.barrel_heat}");
00079:
00080:                 // ── Hydraulic service ───────────────────────────────────────
00081:                 int oilBefore = inventory.CountById(SkyDefenseBatterySystem.ServiceOilItemId);
00082:                 var service = defense.TryServiceHydraulics(turret.turret_id);
00083:                 Check("service_ok", service.IsSuccess && turret.volleys_since_service == 0);
00084:                 Check("service_cost", inventory.CountById(SkyDefenseBatterySystem.ServiceOilItemId) == oilBefore - 1,
00085:                     $"oil={inventory.CountById(SkyDefenseBatterySystem.ServiceOilItemId)}");
00086:
00087:                 // ── Daily dissipation + track prune ─────────────────────────
00088:                 int preTickHeat = turret.barrel_heat;
00089:                 turret.barrel_heat = 100;
00090:                 defense.TickDay(31);
00091:                 Check("heat_dissipates", turret.barrel_heat < 100, $"heat={turret.barrel_heat}");
00092:                 Check("track_pruned", defense.Tracks.Count == 0, $"tracks={defense.Tracks.Count}");
00093:
00094:                 // ─ Save round-trip ─────────────────────────────────────────
00095:                 var saved = defense.CaptureState();
00096:                 var restored = new SkyDefenseBatterySystem(42);
00097:                 restored.RestoreState(saved);
00098:                 var restoredTurret = restored.GetTurret(turret.turret_id);
00099:                 Check("save_roundtrip",
00100:                     restored.Turrets.Count == defense.Turrets.Count
00101:                     && restored.TotalVolleys == defense.TotalVolleys
00102:                     && restoredTurret != null
00103:                     && restoredTurret.magazine_count == turret.magazine_count);
00104:
00105:                 // ── Player panel construction + binding ─────────────────────
00106:                 var panel = new SkyDefenseBatteryPanel();
00107:                 panel._Ready();
00108:                 panel.Bind(
00109:                     defense,
00110:                     () => (IReadOnlyList<string>)new List<string> { "survivor_selftest_gunner" },
00111:                     id => id,
00112:                     id => inventory.CountById(id));
00113:                 Check("panel_bound", panel.IsBound);
00114:                 panel.RefreshView();
00115:                 panel.Unbind();
00116:                 Check("panel_unbound", !panel.IsBound);
00117:                 panel.Free();
00118:             }
00119:             catch (Exception ex)
00120:             {
00121:                 Check("exception", false, ex.Message);
00122:             }
00123:
00124:             GD.Print("\n[HostCli] sky_defense self-test" + (fail == 0 ? " PASS" : " FAIL"));
00125:             foreach (var line in details) GD.Print(line);
00126:             return EmitSummary("sky_defense_selftest", fail == 0, passedCount: pass, failedCount: fail,
00127:                 details: "sky-layer counter-battery catalog/telemetry/magazine/volley/service/save/panel");
00128:         }
00129:     }
00130: }
```

## `Ashfall.Core.Tests/Radio/ShelterRadioStationTests.cs` — 228 lines; 8,434 bytes; SHA-256 `ec4160fe6ca805f9c0495e72c6ab1f3def36beeb7aaacf90136921b57559dbbe`
Declaration index:
- 00013: public class ShelterRadioStationTests
- 00015: private static string GetRadioInterceptCatalogJson()
- 00069: private static ShelterRadioStationSystem CreateSystem(
- 00083: public void ScanFrequency_LocksWhenTunedCloseToBroadcast()
- 00099: public void ScanFrequency_MissesWhenDetuned()
- 00110: public void Decryption_AdvancesWithOperatorSkill()
- 00125: public void Triangulation_RequiresDistinctAzimuthsAndUnlocksLocation()
- 00151: public void SOSDistress_ExpiresWhenDeadlineReached()
- 00168: public void OrbitalEarlyWarning_RelaysActiveImpactWarning()
- 00191: public void SaveRestore_PreservesRadioStateAndBearings()
- 00212: public void DeterministicReplay_ProducesIdenticalRadioScans()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Ashfall.Core;
00006: using Ashfall.Core.IO;
00007: using Ashfall.Core.Radio;
00008: using Ashfall.Core.Shelter;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.Radio
00012: {
00013:     public class ShelterRadioStationTests
00014:     {
00015:         private static string GetRadioInterceptCatalogJson()
00016:         {
00017:             string path = Path.Combine(AppContext.BaseDirectory, "Assets/StreamingAssets/Data/radio_intercepts.json");
00018:             if (!File.Exists(path))
00019:             {
00020:                 path = Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data/radio_intercepts.json");
00021:             }
00022:             if (File.Exists(path))
00023:             {
00024:                 return File.ReadAllText(path);
00025:             }
00026:
00027:             return @"{
00028:   ""schema_version"": 1,
00029:   ""intercepts"": [
00030:     {
00031:       ""id"": ""radio_intercept_meridian_supply_column_01"",
00032:       ""callsign"": ""MERIDIAN-ACT-7"",
00033:       ""frequency_khz"": 7115,
00034:       ""band"": ""hf"",
00035:       ""signal_class"": ""logistics_chatter"",
00036:       ""source_faction_id"": ""faction_the_compact"",
00037:       ""base_signal_strength"": 0.65,
00038:       ""encryption"": {
00039:         ""scheme"": ""field_cipher"",
00040:         ""difficulty"": 40,
00041:         ""required_skill_ids"": [""skill_signal_ear"", ""skill_cold_analysis""]
00042:       },
00043:       ""triangulation"": {
00044:         ""required_bearings"": 3,
00045:         ""revealed_location_id"": ""loc_diesel_tank_farm""
00046:       },
00047:       ""expiry_days"": 4,
00048:       ""message"": ""Convoy route Bravo compromised by ash drifts."",
00049:       ""tags"": [""military""]
00050:     },
00051:     {
00052:       ""id"": ""radio_intercept_sos_quarry_shelter_02"",
00053:       ""callsign"": ""SHELTER-44-SOS"",
00054:       ""frequency_khz"": 3850,
00055:       ""band"": ""hf"",
00056:       ""signal_class"": ""sos_distress"",
00057:       ""source_faction_id"": ""faction_the_office"",
00058:       ""base_signal_strength"": 0.50,
00059:       ""encryption"": { ""scheme"": ""none"", ""difficulty"": 0, ""required_skill_ids"": [] },
00060:       ""triangulation"": { ""required_bearings"": 2, ""revealed_location_id"": ""loc_recovery_yard"" },
00061:       ""expiry_days"": 3,
00062:       ""message"": ""MAYDAY. Primary air intake collapsed."",
00063:       ""tags"": [""distress""]
00064:     }
00065:   ]
00066: }";
00067:         }
00068:
00069:         private static ShelterRadioStationSystem CreateSystem(
00070:             out OrbitalHarrowTelemetrySystem harrow,
00071:             int seed = 42)
00072:         {
00073:             var rng = new SeededRng(seed);
00074:             var armor = new SkyLayerArmorSystem();
00075:             harrow = new OrbitalHarrowTelemetrySystem(armor, rng);
00076:
00077:             var station = new ShelterRadioStationSystem(rng, harrow);
00078:             station.LoadCatalog(GetRadioInterceptCatalogJson());
00079:             return station;
00080:         }
00081:
00082:         [Fact]
00083:         public void ScanFrequency_LocksWhenTunedCloseToBroadcast()
00084:         {
00085:             var station = CreateSystem(out _);
00086:             station.TuneTo(7115, "hf"); // Exact match for meridian supply
00087:
00088:             var result = station.ScanFrequency(1);
00089:             Assert.True(result.FoundSignal);
00090:             Assert.Equal("radio_intercept_meridian_supply_column_01", result.InterceptId);
00091:             Assert.True(result.SignalStrength > 0.40f);
00092:
00093:             var progress = station.GetOrCreateInterceptProgress("radio_intercept_meridian_supply_column_01");
00094:             Assert.True(progress.Detected);
00095:             Assert.True(progress.SignalLockPermille > 0);
00096:         }
00097:
00098:         [Fact]
00099:         public void ScanFrequency_MissesWhenDetuned()
00100:         {
00101:             var station = CreateSystem(out _);
00102:             station.TuneTo(9000, "hf"); // Far from any broadcast
00103:
00104:             var result = station.ScanFrequency(1);
00105:             Assert.False(result.FoundSignal);
00106:             Assert.Equal("static_noise", result.StatusMessage);
00107:         }
00108:
00109:         [Fact]
00110:         public void Decryption_AdvancesWithOperatorSkill()
00111:         {
00112:             var station = CreateSystem(out _);
00113:             station.BindSkillProvider(skillId => skillId == "skill_signal_ear" ? 1.0f : 0.5f);
00114:             station.TuneTo(7115, "hf");
00115:             station.ScanFrequency(1);
00116:
00117:             int progress1 = station.ProgressDecryption("radio_intercept_meridian_supply_column_01");
00118:             Assert.True(progress1 > 0);
00119:
00120:             var item = station.GetOrCreateInterceptProgress("radio_intercept_meridian_supply_column_01");
00121:             Assert.True(item.DecryptProgressPermille > 0);
00122:         }
00123:
00124:         [Fact]
00125:         public void Triangulation_RequiresDistinctAzimuthsAndUnlocksLocation()
00126:         {
00127:             var station = CreateSystem(out _);
00128:             station.TuneTo(3850, "hf");
00129:             station.ScanFrequency(1);
00130:
00131:             // Bearing 1 at 45 deg
00132:             bool unlocked1 = station.RecordBearing("radio_intercept_sos_quarry_shelter_02", 45);
00133:             Assert.False(unlocked1);
00134:
00135:             // Duplicate bearing near 45 deg (e.g. 50 deg, < 20 deg diff) -> not counted as distinct
00136:             bool unlockedDup = station.RecordBearing("radio_intercept_sos_quarry_shelter_02", 50);
00137:             Assert.False(unlockedDup);
00138:
00139:             var progress = station.GetOrCreateInterceptProgress("radio_intercept_sos_quarry_shelter_02");
00140:             Assert.Equal(1, progress.BearingsCollected);
00141:
00142:             // Bearing 2 at 180 deg (distinct >= 20 deg) -> unlocks!
00143:             bool unlocked2 = station.RecordBearing("radio_intercept_sos_quarry_shelter_02", 180);
00144:             Assert.True(unlocked2);
00145:             Assert.Equal(2, progress.BearingsCollected);
00146:             Assert.True(progress.Resolved);
00147:             Assert.Contains("loc_recovery_yard", station.State.discoveredLocationIds);
00148:         }
00149:
00150:         [Fact]
00151:         public void SOSDistress_ExpiresWhenDeadlineReached()
00152:         {
00153:             var station = CreateSystem(out _);
00154:             station.TuneTo(3850, "hf");
00155:             station.ScanFrequency(1); // Detected day 1, expires day 1 + 3 = 4
00156:
00157:             var progress = station.GetOrCreateInterceptProgress("radio_intercept_sos_quarry_shelter_02");
00158:             Assert.False(progress.IsExpired);
00159:
00160:             station.TickDay(2);
00161:             Assert.False(progress.IsExpired);
00162:
00163:             station.TickDay(4); // Day 4 >= ExpiresOnDay 4
00164:             Assert.True(progress.IsExpired);
00165:         }
00166:
00167:         [Fact]
00168:         public void OrbitalEarlyWarning_RelaysActiveImpactWarning()
00169:         {
00170:             var station = CreateSystem(out var harrow);
00171:             harrow.ActivateTelemetry(1);
00172:
00173:             // Simulate harrow warning
00174:             harrow.State.warnings.Add(new OrbitalWarningEntry
00175:             {
00176:                 day = 2,
00177:                 targetGridX = 3,
00178:                 energyMj = 25f,
00179:                 eventId = "harrow_strike_alpha",
00180:                 telemetryText = "Incoming rod descent",
00181:                 severity = "Critical"
00182:             });
00183:             harrow.State.nextImpactDay = 4;
00184:
00185:             var relayed = station.CheckOrbitalEarlyWarning(2);
00186:             Assert.NotNull(relayed);
00187:             Assert.Equal("harrow_strike_alpha", relayed.eventId);
00188:         }
00189:
00190:         [Fact]
00191:         public void SaveRestore_PreservesRadioStateAndBearings()
00192:         {
00193:             var station = CreateSystem(out _);
00194:             station.TuneTo(7115, "hf");
00195:             station.SetAntennaAzimuth(135);
00196:             station.ScanFrequency(1);
00197:             station.RecordBearing("radio_intercept_meridian_supply_column_01", 135);
00198:
00199:             var save = station.CaptureState();
00200:             var station2 = CreateSystem(out _);
00201:             station2.RestoreState(save);
00202:
00203:             Assert.Equal(7115, station2.State.tunedFrequencyKhz);
00204:             Assert.Equal(135, station2.State.antennaAzimuthDegrees);
00205:             var restored = station2.GetOrCreateInterceptProgress("radio_intercept_meridian_supply_column_01");
00206:             Assert.NotNull(restored);
00207:             Assert.True(restored.Detected);
00208:             Assert.Equal(1, restored.BearingsCollected);
00209:         }
00210:
00211:         [Fact]
00212:         public void DeterministicReplay_ProducesIdenticalRadioScans()
00213:         {
00214:             var sysA = CreateSystem(out _, seed: 9999);
00215:             var sysB = CreateSystem(out _, seed: 9999);
00216:
00217:             sysA.TuneTo(7115, "hf");
00218:             sysB.TuneTo(7115, "hf");
00219:
00220:             var resA = sysA.ScanFrequency(1);
00221:             var resB = sysB.ScanFrequency(1);
00222:
00223:             Assert.Equal(resA.FoundSignal, resB.FoundSignal);
00224:             Assert.Equal(resA.InterceptId, resB.InterceptId);
00225:             Assert.Equal(resA.SignalStrength, resB.SignalStrength);
00226:         }
00227:     }
00228: }
```

## `Ashfall.Core.Tests/IslandBridgesTests.cs` — 206 lines; 8,938 bytes; SHA-256 `f375690aad3d7dad3c5a0f6b5e88d488a84bab80db8c540440c0cf786d1b9007`
Declaration index:
- 00013: public class IslandBridgesTests
- 00017: public void MaritimeDive_ConductDive_ResolvesAndPreservesState()
- 00038: public void WorkshopReverseEngineering_ExamineAndRepair_UnlocksRelic()
- 00080: public void PharmaLab_SynthesizeMedicine_ProducesOutput()
- 00121: public void WeatherStation_InstallAndCalibrate_GeneratesForecast()
- 00147: public void OrbitalHarrow_WarningAndImpact_ResolvesDamage()
- 00171: public void ExpeditionVehicle_RefuelAndTravel_TracksCondition()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: using Ashfall.Core;
00004: using Ashfall.Core.Crafting;
00005: using Ashfall.Core.Inventory;
00006: using Ashfall.Core.Maritime;
00007: using Ashfall.Core.Shelter;
00008: using Ashfall.Core.World;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests
00012: {
00013:     public class IslandBridgesTests
00014:     {
00015:         // ── Task 4: Maritime & Diving System ──
00016:         [Fact]
00017:         public void MaritimeDive_ConductDive_ResolvesAndPreservesState()
00018:         {
00019:             var sys = new MaritimeDiveSystem(new SeededRng(42));
00020:             var reg = sys.RegisterSite("dive_sub_wreck", "Sunken Submarine Wreck", depthMeters: 45f, hazardLevel: 0.3f);
00021:             Assert.Equal(ActionResult.StatusKind.Success, reg.Status);
00022:
00023:             var dive = sys.ConductDive("dive_sub_wreck", "diver_1", equipmentQuality: 1.2f);
00024:             Assert.Equal(ActionResult.StatusKind.Success, dive.Status);
00025:             Assert.Single(sys.State.outcomes);
00026:             Assert.True(sys.State.sites[0].isExplored);
00027:
00028:             var state = sys.CaptureState();
00029:             var sys2 = new MaritimeDiveSystem(new SeededRng(42));
00030:             sys2.RestoreState(state);
00031:             Assert.Single(sys2.State.sites);
00032:             Assert.Single(sys2.State.outcomes);
00033:             Assert.Equal("dive_sub_wreck", sys2.State.outcomes[0].siteId);
00034:         }
00035:
00036:         // ── Task 5: Workshop Reverse Engineering ──
00037:         [Fact]
00038:         public void WorkshopReverseEngineering_ExamineAndRepair_UnlocksRelic()
00039:         {
00040:             var inv = new Inventory.Inventory();
00041:             var research = new ResearchSystem();
00042:             // Plan 34: authoritative JSON catalog
00043:             ResearchLegacyCatalogFixture.LoadAuthoritativeCatalogInto(research);
00044:             var crafting = new CraftingSystem(inv);
00045:             var workshop = new WorkshopReverseEngineeringSystem(inv, research, crafting);
00046:
00047:             workshop.LoadCatalog(new RelicCatalog
00048:             {
00049:                 relics = new List<RelicDefinition>
00050:                 {
00051:                     new RelicDefinition
00052:                     {
00053:                         relic_id = "relic_gyroscope",
00054:                         display_name = "Navigational Gyroscope",
00055:                         repair_time_hours = 4f,
00056:                         required_components = new List<string> { "item_scrap_metal" },
00057:                         research_unlock_id = "knowledge_solar_basics"
00058:                     }
00059:                 }
00060:             });
00061:
00062:             inv.AddById("item_scrap_metal", 2);
00063:             var start = workshop.StartResearch("relic_gyroscope", "engineer_1");
00064:             Assert.Equal(ActionResult.StatusKind.Success, start.Status);
00065:
00066:             var tick = workshop.TickProgress(10f);
00067:             Assert.Equal(ActionResult.StatusKind.Success, tick.Status);
00068:             Assert.True(workshop.State.isComplete);
00069:             Assert.Contains("relic_gyroscope", workshop.State.completedRelicIds);
00070:             Assert.True(research.IsManualUnlocked("knowledge_solar_basics"));
00071:
00072:             var state = workshop.CaptureState();
00073:             var workshop2 = new WorkshopReverseEngineeringSystem(inv, research, crafting);
00074:             workshop2.RestoreState(state);
00075:             Assert.Contains("relic_gyroscope", workshop2.State.completedRelicIds);
00076:         }
00077:
00078:         // ── Task 6: Pharma Lab & Chemical Synthesis ──
00079:         [Fact]
00080:         public void PharmaLab_SynthesizeMedicine_ProducesOutput()
00081:         {
00082:             var inv = new Inventory.Inventory();
00083:             var lab = new PharmaLabSystem(inv, new SeededRng(42));
00084:
00085:             lab.LoadCatalog(new PharmaRecipeCatalog
00086:             {
00087:                 recipes = new List<PharmaRecipe>
00088:                 {
00089:                     new PharmaRecipe
00090:                     {
00091:                         recipe_id = "pharma_rad_blocker",
00092:                         display_name = "Potassium Iodide Solution",
00093:                         input_ids = new List<string> { "item_chemical_reagents" },
00094:                         input_amounts = new List<int> { 2 },
00095:                         output_item_id = "item_rad_blocker_pure",
00096:                         output_amount = 3,
00097:                         base_hours = 2f,
00098:                         purity_target = 0.5f
00099:                     }
00100:                 }
00101:             });
00102:
00103:             inv.AddById("item_chemical_reagents", 5);
00104:             var start = lab.StartBatch("pharma_rad_blocker", "chemist_1");
00105:             Assert.Equal(ActionResult.StatusKind.Success, start.Status);
00106:
00107:             var tick = lab.TickProgress(3f);
00108:             Assert.Equal(ActionResult.StatusKind.Success, tick.Status);
00109:             Assert.False(lab.State.isProcessing);
00110:             Assert.True(inv.CountById("item_rad_blocker_pure") >= 1);
00111:             Assert.Equal(1, lab.State.totalBatchesProduced);
00112:
00113:             var state = lab.CaptureState();
00114:             var lab2 = new PharmaLabSystem(inv, new SeededRng(42));
00115:             lab2.RestoreState(state);
00116:             Assert.Equal(1, lab2.State.totalBatchesProduced);
00117:         }
00118:
00119:         // ── Task 7: Weather Station ──
00120:         [Fact]
00121:         public void WeatherStation_InstallAndCalibrate_GeneratesForecast()
00122:         {
00123:             var weather = new WeatherSystem();
00124:             weather.BindProfile(new SeasonProfileDef { id = "season_nuclear_winter" }, 42);
00125:             var station = new WeatherStationSystem(weather, new SeededRng(42));
00126:
00127:             var inst = station.Install(1);
00128:             Assert.Equal(ActionResult.StatusKind.Success, inst.Status);
00129:
00130:             var cal = station.Calibrate(2);
00131:             Assert.Equal(ActionResult.StatusKind.Success, cal.Status);
00132:             Assert.True(station.IsOperational);
00133:
00134:             var fc = station.GenerateForecast(3);
00135:             Assert.Equal(ActionResult.StatusKind.Success, fc.Status);
00136:             Assert.True(station.State.cachedForecast.Count > 0);
00137:
00138:             var state = station.CaptureState();
00139:             var station2 = new WeatherStationSystem(weather, new SeededRng(42));
00140:             station2.RestoreState(state);
00141:             Assert.True(station2.IsOperational);
00142:             Assert.Equal(state.lastForecastDay, station2.State.lastForecastDay);
00143:         }
00144:
00145:         // ── Task 8: Orbital Harrow Telemetry ──
00146:         [Fact]
00147:         public void OrbitalHarrow_WarningAndImpact_ResolvesDamage()
00148:         {
00149:             var armor = new SkyLayerArmorSystem();
00150:             armor.SetCellArmor(gridX: 2, CeilingMaterialTier.ReinforcedConcrete, thicknessMeters: 1.5f, durability: 100f);
00151:             var orbital = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(42));
00152:
00153:             orbital.ActivateTelemetry(1);
00154:             orbital.ScheduleImpact(day: 5, gridX: 2, energyMj: 50f);
00155:             var brace = orbital.Brace("item_scrap_metal", 2);
00156:             Assert.Equal(ActionResult.StatusKind.Success, brace.Status);
00157:
00158:             orbital.TickDay(5);
00159:             Assert.Contains(5, orbital.State.impactHistory);
00160:             Assert.Equal(5, orbital.State.lastImpactDay);
00161:
00162:             var state = orbital.CaptureState();
00163:             var orbital2 = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(42));
00164:             orbital2.RestoreState(state);
00165:             Assert.True(orbital2.State.telemetryActive);
00166:             Assert.Contains(5, orbital2.State.impactHistory);
00167:         }
00168:
00169:         // ── Task 9: Expedition Vehicle System ──
00170:         [Fact]
00171:         public void ExpeditionVehicle_RefuelAndTravel_TracksCondition()
00172:         {
00173:             var vehicles = new ExpeditionVehicleSystem(new SeededRng(42));
00174:             vehicles.LoadCatalog(new VehicleCatalog
00175:             {
00176:                 vehicles = new List<VehicleDefinition>
00177:                 {
00178:                     new VehicleDefinition
00179:                     {
00180:                         vehicle_id = "vehicle_armored_truck",
00181:                         display_name = "Armored 6x6 Hauler",
00182:                         max_fuel = 80f,
00183:                         cargo_capacity = 250f,
00184:                         fuel_consumption_per_km = 0.2f
00185:                     }
00186:                 }
00187:             });
00188:
00189:             var reg = vehicles.AcquireVehicle("vehicle_armored_truck");
00190:             Assert.Equal(ActionResult.StatusKind.Success, reg.Status);
00191:
00192:             var refuel = vehicles.Refuel("vehicle_armored_truck", 50f);
00193:             Assert.Equal(ActionResult.StatusKind.Success, refuel.Status);
00194:             Assert.True(vehicles.State.ownedVehicles["vehicle_armored_truck"].fuel > 0f);
00195:
00196:             var prep = vehicles.PrepareForExpedition("vehicle_armored_truck", distanceKm: 50f);
00197:             Assert.True(prep.fuelCost > 0f);
00198:
00199:             var state = vehicles.CaptureState();
00200:             var vehicles2 = new ExpeditionVehicleSystem(new SeededRng(42));
00201:             vehicles2.RestoreState(state);
00202:             Assert.True(vehicles2.State.ownedVehicles.ContainsKey("vehicle_armored_truck"));
00203:             Assert.Equal(vehicles.State.ownedVehicles["vehicle_armored_truck"].fuel, vehicles2.State.ownedVehicles["vehicle_armored_truck"].fuel);
00204:         }
00205:     }
00206: }
```
# Appendix M — External verification handoff

The following checks are to be run by the owning integrator after writing: character count, SHA-256 revalidation, path-token resolution, duplicate-heading/unsupported-claim scan, and `git diff --check`. The final ledger entry must report actual results, not this template.
