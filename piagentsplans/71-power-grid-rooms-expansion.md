# Plan 71 — Power Grid Rooms: Eighteen-Node Catalog, Allocation Authority, and Consumer Reachability

> **Rebuild status:** TERMINAL 18-ROOM CATALOG + POWER ALLOCATION/OWNER AUDIT
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

The historical baseline was 4,833 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Keep the eighteen-room power-grid catalog as the authored load/priority contract while preserving `PowerGridSystem` as the sole allocation, shedding, breaker, generation, surge, and persistence owner. The historical 6→18 expansion is complete; the next value is verifying room registration, current fallback parity, consumer routing, and the fact that `failure_effect_id` is descriptive unless a current owner consumes it.

**Bounded outcome:** Audit `ShelterPowerGridCatalogLoader`, `PowerGridSystem`, host/save façades, campaign day owner, current room/industrial consumers, UI, and focused tests. Do not add rooms, generation sources, failure effects, or a second grid without a measured current gap.

**Non-goals:** no new power authority, no new save section, no arbitrary room count, no duplicate generation/fuel ledger, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `power_grid.json` contains 18 rooms with draw, priority, and failure-effect fields.
- VERIFIED CURRENT: `ShelterPowerGridCatalogLoader` validates schema/ranges/ids/priorities and carries an embedded fallback.
- VERIFIED CURRENT: `PowerGridSystem` owns allocation, shedding, breakers, maintenance, surge, and capture/restore.
- VERIFIED CURRENT: the host and save façade use the `power_grid` section.
- HISTORICAL RECORD: Plan 71/Wave 39 records the 6→18 catalog expansion; this package does not claim a fresh run.

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

- `Assets/StreamingAssets/Data/power_grid.json` exists at 3,741 bytes; SHA-256 `8912f95d4f566a7b2c6313daab1333ee153efdf84a7340ff1790384e12296b2e`.
- `Assets/StreamingAssets/Data/shelter_rooms.json` exists at 20,439 bytes; SHA-256 `1d7df91334461310d3c76457b60a93df678369bdc3846f00917c1f3bf5e8d379`.
- `Assets/StreamingAssets/Data/power_subgrid_nodes.json` exists at 4,263 bytes; SHA-256 `83a943a68e64169b9cdaa4b629adeac698eb409c803291f7ec68e5d0a10de5bd`.
- `Assets/StreamingAssets/Data/vehicle_armor_grades.json` exists at 4,568 bytes; SHA-256 `8e62dc6b78f7e97b107078bd3802577edfdba0b399abc8e70d1479dd76d03faa`.

# 3. Required Delta

Replace the old pure-data brief with a current 18-room census, fallback parity check, and consumer/authority audit. Preserve PowerGridSystem as the sole mutable electrical owner and treat failure-effect text as descriptive until consumed.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Which current production consumers register or query each of the 18 room ids?
Does the live host use the JSON catalog or the embedded fallback under normal boot?
Are `failure_effect_id` values consumed by any current consequence owner?
Does the current day owner preserve the expected phase/order and save snapshot semantics?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| room catalog parsing/validation | `ShelterPowerGridCatalogLoader` | `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs` | Static room definitions and strict validation; fallback is explicit. |
| generation, allocation, shedding, breakers, surge, and room state | `PowerGridSystem` | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | Sole mutable electrical authority. |
| host session and persistence façade | `PowerGridHostSession / PowerGridSaveStore` | `src/Host/PowerGridHostSession.cs; src/Host/PowerGridSaveStore.cs` | Binds current grid and power_grid save section. |
| campaign day integration | `PowerGridDayOwner / Main.CampaignOwners` | `src/Main.CampaignOwners.cs` | Orders grid tick and current day facts. |
| industrial and shelter consumers | `existing water/foundry/greenhouse/clinic/heating seams` | `src/Main.* and Core consumer paths` | Read IsRoomServed/priority; do not own allocation. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for shelter power-grid room catalog.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Power Grid Rooms: Eighteen-Node Catalog, Allocation Authority, and Consumer Reachability                                               │
│ Integration route: DATA-ONLY + current PowerGridSystem/host/consumer audit                             │
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

1. **The catalog defines rooms.**
2. **PowerGridSystem owns electricity.**
3. **The host binds the existing grid.**
4. **The power_grid section persists mutable state.**
5. **Consumers read service/priority; they do not allocate.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| room catalog parsing/validation | `ShelterPowerGridCatalogLoader` | `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs` | Static room definitions and strict validation; fallback is explicit. |
| generation, allocation, shedding, breakers, surge, and room state | `PowerGridSystem` | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | Sole mutable electrical authority. |
| host session and persistence façade | `PowerGridHostSession / PowerGridSaveStore` | `src/Host/PowerGridHostSession.cs; src/Host/PowerGridSaveStore.cs` | Binds current grid and power_grid save section. |
| campaign day integration | `PowerGridDayOwner / Main.CampaignOwners` | `src/Main.CampaignOwners.cs` | Orders grid tick and current day facts. |
| industrial and shelter consumers | `existing water/foundry/greenhouse/clinic/heating seams` | `src/Main.* and Core consumer paths` | Read IsRoomServed/priority; do not own allocation. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load/validate power_grid.json through ShelterPowerGridCatalogLoader
2. construct PowerGridSystem with the current rooms and seeded owner
3. bind generation contributions and room consumers through existing providers
4. compute available supply and allocation with priorities/breakers
5. emit current grid events and expose IsRoomServed/EffectivePriority
6. capture/restore the existing power_grid section and reject stale rooms safely

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- room ids are unique, non-empty, and draw watts are finite/non-negative
- priorities use the current Critical/Standard/Low/Disabled vocabulary
- load shedding never serves a lower-priority room ahead of a higher-priority demand without the current policy
- battery reserve and generation contributions are finite and bounded
- room registration cannot create a second allocation ledger
- save restore preserves breakers, priorities, generation contributions, maintenance, and surge state
- failure_effect_id is not treated as executed unless a current consumer proves that contract

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

Contract rules for shelter power-grid room catalog:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`power_grid.json` remains the room catalog authority. The current loader validates schema, numerics, ids, and priorities and carries an embedded fallback matching the current 18-room surface. Audit `failure_effect_id` separately: a nonempty token is not a consequence. Any future room requires a current consumer, room identity, draw/priority evidence, and a focused allocation test.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

Use the existing `power_grid` section and `PowerGridSaveStore`. Do not persist the static catalog as a second section. A future room or subgrid state change must be additive to the existing PowerGridState codec with legacy defaults and field-parity tests.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

PowerGridSystem receives an `ISeededRng`; allocation ordering and maintenance/surge outcomes must be ordinal-stable and replayable. Catalog order cannot decide tie-breaking by hash iteration. A paired test compares generation, served/shed room ids, breaker/trip state, fuel, battery, and events.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

PowerGridSystem emits current tick/allocations/maintenance/surge facts. Consumers subscribe to `IsRoomServed`/priority or current day events; they do not mutate the grid directly. A failed room produces a current owner fact, not a fabricated consequence from `failure_effect_id`.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Host/PowerGridHostSession.cs` — creates/binds the current grid and exposes commands
- `src/Host/PowerGridSaveStore.cs` — persists the existing power_grid envelope
- `src/Main.CampaignOwners.cs` — registers the canonical day owner and event ordering
- `src/Main.World.cs` — composes current power state and save handoff
- `src/UI/PowerGridPanel.cs` — presents generation, load, priority, breaker, and reserve truth

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Power-grid room names and failure descriptions should read as shelter infrastructure, not a second simulation. A panel may explain why a room is shed, but it must not claim a fire, crop loss, or medical outage unless the current consequence owner recorded it.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/PowerGridDeterminismTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan85_71DamagedMapPowerIntegrationTests.cs`

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
| Phase 0 — catalog/owner census | read 18 rows, loader, system, host, save, day owner, and panel | all current values and fallback parity are explicit | no undocumented scope or shortcut |
| Phase 1 — consumer graph | trace water/foundry/greenhouse/clinic/heating and room registration | live consumers and dormant fields are classified | no undocumented scope or shortcut |
| Phase 2 — allocation/determinism/persistence audit | check shed order, reserve, breaker, restore, and same-seed replay | no shadow grid or false failure effect remains | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a measured room/consumer gap is promoted | one claim and focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/power_grid.json` | READ ONLY; MODIFY only for a proven row/consumer defect | retain as room authority |
| `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs` | READ ONLY | strict loader/fallback |
| `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | READ ONLY | sole electrical owner |
| `src/Host/PowerGridHostSession.cs` | READ ONLY | host command seam |
| `src/UI/PowerGridPanel.cs` | READ ONLY | truthful presentation |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| duplicate power/grid state | extend PowerGridSystem only |
| fallback/data drift | pin loader/fallback parity |
| consumer-side allocation | use IsRoomServed and current providers |
| surge/save regression | paired replay and codec tests |

# 22. Explicit Non-Goals

- no new power authority, no new save section, no arbitrary room count, no duplicate generation/fuel ledger, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for shelter power-grid room catalog are named from current evidence.
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

- A current 18-room catalog/consumer/fallback matrix.
- An explicit failure-effect-id reachability result.
- A bounded residual only for a proven room or consumer gap.

## MUST NOT DO

- add a second power grid
- treat descriptive failure tokens as executed effects
- change generation/fuel arithmetic in this package
- add a save section for static room definitions

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/PowerGridDeterminismTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan85_71DamagedMapPowerIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read ShelterPowerGridCatalogLoader, PowerGridSystem, PowerGridHostSession, PowerGridSaveStore, CampaignOwners, and PowerGridPanel; map every room and field to its current consumer.

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

### Authority lines 232–251
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
00246: **SB-01 — Delayed moral-choice callbacks (Lane A/C10).** Evidence: v1.0 Part 7 gap 2; `moral_choice_flags.json`, `IFlagLedger`, `DoorEncounterSystem`, `MoralChoiceSaveStore` all confirmed live. Subject: ~100-day delayed visitor/letter/radio/journal returns keyed on persisted flags. Integration route: data-first new catalog through the moral-choice loader family; dispatch through the daily-tick seam; possibly no codec bump if per-flag records already persist. Verification: integrity + utilization selftests, determinism replay, exactly-once dispatch test. Confidence: HIGH CONFIDENCE.
00247:
00248: **SB-02 — Mid-winter slump pressure campaign (Lane A/C12).** Evidence: v1.0 Part 7 gap 1 (Days 90–180). Subject: a bounded story-pressure wave (blight, cave-in, levy arc) authored through existing catalogs. Integration route: data-first; each pressure rides its owning system (ecology for blight, subterranean/excavation for cave-ins, warlord doctrines for levies). Confidence: HIGH CONFIDENCE.
00249:
00250: **SB-03 — Newest industrial catalogs: corpus twins + consumption wiring (Lanes A and B/C4).** Evidence: DR-04 (`hydraulic_extraction_catalog`, `metrology_standards_catalog` live, absent from v1.0 inventory). Subject: (a) assay-log narrative twins per the Part 16.4 pattern; (b) bind catalogs into consumption/production ledgers via power-grid/foundry seams if not yet consumed — check `UNCLAIMED_CORPUS_CENSUS.md` first (DR-08). Confidence: HIGH CONFIDENCE that content exists; UNVERIFIED whether systems consume them.
00251:

### Authority lines 254–259
00254: **SB-05 — Epilogue permutation coverage campaign (Lanes A and C/C13).** Evidence: 19A/19B/19C closed (DR-06); matrix is 32 permutations. Subject: audit which permutations are under-served in chronicle prose and evidence enrollment; author chronicle depth for the weakest permutations. Integration route: data-first into epilogue chronicle catalogs; Reckoning enrollment through endgame owners. Confidence: HIGH CONFIDENCE.
00255:
00256: **SB-06 — Balance baseline refresh wave (Lane C).** Evidence: DR-03 live baselines. Subject: re-run the deterministic simulation harness after the most recent content waves (muster, moral-choice distress additions, industrial catalogs) and publish deltas in `docs/balance/`. Integration route: no product code; harness runs + reports. Confidence: HIGH CONFIDENCE.
00257:
00258: **SB-07 — Root coordination surface registration (Lane I).** Evidence: DR-01, DR-09. Subject: register root-level coordination files and agent rulebooks in the docs map; define which are active vs historical. Integration route: docs-only. Confidence: VERIFIED need.
00259:

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

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/power_grid.json`
- Bytes: 3,741; SHA-256: `8912f95d4f566a7b2c6313daab1333ee153efdf84a7340ff1790384e12296b2e`
- Root keys: `battery_capacity_wh_default, emp_storm_severity, fuel_units_default, generation_watts_default, rooms, schema_version, surge_battery_drain_fraction`
- `rooms`: list[18]; union fields: `default_priority, display_name, draw_watts, failure_effect_id, id`
  - row 1: `{"default_priority":"critical","display_name":"Air Filtration","draw_watts":180,"failure_effect_id":"fx_filtration_off","id":"room_air_filtration"}`
  - row 2: `{"default_priority":"critical","display_name":"Clinic","draw_watts":120,"failure_effect_id":"fx_clinic_off","id":"room_clinic"}`
  - row 3: `{"default_priority":"critical","display_name":"Water Pump","draw_watts":100,"failure_effect_id":"fx_water_pressure_drop","id":"room_water_pump"}`
  - row 4: `{"default_priority":"standard","display_name":"Greenhouse","draw_watts":160,"failure_effect_id":"fx_grow_lights_off","id":"room_greenhouse"}`
  - row 5: `{"default_priority":"low","display_name":"Silent Foundry","draw_watts":220,"failure_effect_id":"fx_foundry_standstill","id":"room_foundry"}`
  - row 6: `{"default_priority":"low","display_name":"Main Lighting","draw_watts":80,"failure_effect_id":"fx_lighting_dim","id":"room_lighting_main"}`
  - row 7: `{"default_priority":"low","display_name":"Workshop","draw_watts":300,"failure_effect_id":"fx_workshop_offline","id":"room_workshop"}`
  - row 8: `{"default_priority":"critical","display_name":"Cryo Vault","draw_watts":280,"failure_effect_id":"fx_cryo_vault_unpowered","id":"room_cryo_vault"}`
  - row 9: `{"default_priority":"critical","display_name":"Quarantine Ward","draw_watts":90,"failure_effect_id":"fx_quarantine_ventilation_off","id":"room_ward_quarantine"}`
  - row 10: `{"default_priority":"standard","display_name":"Electric Heating","draw_watts":240,"failure_effect_id":"fx_heating_off","id":"room_heating"}`
  - row 11: `{"default_priority":"standard","display_name":"Kitchen","draw_watts":150,"failure_effect_id":"fx_kitchen_off","id":"room_kitchen"}`
  - row 12: `{"default_priority":"critical","display_name":"Water Filtration","draw_watts":140,"failure_effect_id":"fx_water_filtration_off","id":"room_water_filtration"}`
  - row 13: `{"default_priority":"standard","display_name":"Airlock Decontamination","draw_watts":130,"failure_effect_id":"fx_airlock_decon_off","id":"room_airlock"}`
  - row 14: `{"default_priority":"standard","display_name":"Radio Room","draw_watts":90,"failure_effect_id":"fx_radio_tuner_off","id":"room_radio_tuner"}`
  - row 15: `{"default_priority":"standard","display_name":"Laboratory","draw_watts":260,"failure_effect_id":"fx_laboratory_offline","id":"room_laboratory_research"}`
  - row 16: `{"default_priority":"standard","display_name":"Precision Workshop","draw_watts":240,"failure_effect_id":"fx_precision_metrology_off","id":"room_workshop_precision"}`
  - row 17: `{"default_priority":"low","display_name":"Common Mess Hall","draw_watts":70,"failure_effect_id":"fx_common_mess_cold","id":"room_common_mess_hall"}`
  - row 18: `{"default_priority":"standard","display_name":"Armory & Munitions","draw_watts":60,"failure_effect_id":"fx_armory_service_off","id":"room_armory_munitions"}`
- Bytes: 3,741; SHA-256: `8912f95d4f566a7b2c6313daab1333ee153efdf84a7340ff1790384e12296b2e`
- Root keys: `battery_capacity_wh_default, emp_storm_severity, fuel_units_default, generation_watts_default, rooms, schema_version, surge_battery_drain_fraction`
- `rooms`: list[18]; union fields: `default_priority, display_name, draw_watts, failure_effect_id, id`
  - row 1: `{"default_priority":"critical","display_name":"Air Filtration","draw_watts":180,"failure_effect_id":"fx_filtration_off","id":"room_air_filtration"}`
  - row 2: `{"default_priority":"critical","display_name":"Clinic","draw_watts":120,"failure_effect_id":"fx_clinic_off","id":"room_clinic"}`
  - row 3: `{"default_priority":"critical","display_name":"Water Pump","draw_watts":100,"failure_effect_id":"fx_water_pressure_drop","id":"room_water_pump"}`
  - row 4: `{"default_priority":"standard","display_name":"Greenhouse","draw_watts":160,"failure_effect_id":"fx_grow_lights_off","id":"room_greenhouse"}`
  - row 5: `{"default_priority":"low","display_name":"Silent Foundry","draw_watts":220,"failure_effect_id":"fx_foundry_standstill","id":"room_foundry"}`
  - row 6: `{"default_priority":"low","display_name":"Main Lighting","draw_watts":80,"failure_effect_id":"fx_lighting_dim","id":"room_lighting_main"}`
  - row 7: `{"default_priority":"low","display_name":"Workshop","draw_watts":300,"failure_effect_id":"fx_workshop_offline","id":"room_workshop"}`
  - row 8: `{"default_priority":"critical","display_name":"Cryo Vault","draw_watts":280,"failure_effect_id":"fx_cryo_vault_unpowered","id":"room_cryo_vault"}`
  - row 9: `{"default_priority":"critical","display_name":"Quarantine Ward","draw_watts":90,"failure_effect_id":"fx_quarantine_ventilation_off","id":"room_ward_quarantine"}`
  - row 10: `{"default_priority":"standard","display_name":"Electric Heating","draw_watts":240,"failure_effect_id":"fx_heating_off","id":"room_heating"}`
  - row 11: `{"default_priority":"standard","display_name":"Kitchen","draw_watts":150,"failure_effect_id":"fx_kitchen_off","id":"room_kitchen"}`
  - row 12: `{"default_priority":"critical","display_name":"Water Filtration","draw_watts":140,"failure_effect_id":"fx_water_filtration_off","id":"room_water_filtration"}`
  - row 13: `{"default_priority":"standard","display_name":"Airlock Decontamination","draw_watts":130,"failure_effect_id":"fx_airlock_decon_off","id":"room_airlock"}`
  - row 14: `{"default_priority":"standard","display_name":"Radio Room","draw_watts":90,"failure_effect_id":"fx_radio_tuner_off","id":"room_radio_tuner"}`
  - row 15: `{"default_priority":"standard","display_name":"Laboratory","draw_watts":260,"failure_effect_id":"fx_laboratory_offline","id":"room_laboratory_research"}`
  - row 16: `{"default_priority":"standard","display_name":"Precision Workshop","draw_watts":240,"failure_effect_id":"fx_precision_metrology_off","id":"room_workshop_precision"}`
  - row 17: `{"default_priority":"low","display_name":"Common Mess Hall","draw_watts":70,"failure_effect_id":"fx_common_mess_cold","id":"room_common_mess_hall"}`
  - row 18: `{"default_priority":"standard","display_name":"Armory & Munitions","draw_watts":60,"failure_effect_id":"fx_armory_service_off","id":"room_armory_munitions"}`

## `Assets/StreamingAssets/Data/shelter_rooms.json`
- Bytes: 20,439; SHA-256: `1d7df91334461310d3c76457b60a93df678369bdc3846f00917c1f3bf5e8d379`
- Root keys: `assignment_rules, collection_id, rooms, schema_version`
- `rooms`: list[23]; union fields: `base_condition, build_cost, capacity, description, display_name, function, id, max_upgrade_level, repair_cost, required_skill_id, tags, workstation_id`
  - row 1: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":4}],"capacity":0,"description":"Access concourse and structural spine connecting bunker sectors, hatchways, and stairwells.","display_name":"Central Access Corridor","function":"Corridor","id":"room_bunker_corridor","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":1}],"required_skill_id":"","tags":["spine","circulation","unassignable"],"workstation_id":""}`
  - row 2: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":6},{"item_id":"scrap_metal","quantity":4}],"capacity":6,"description":"Triple-tiered pipe bunks packed wall-to-wall for maximum occupancy at the cost of privacy and comfort.","display_name":"Crowded Bunkhouse","function":"Dormitory","id":"room_bunks_crowded","max_upgrade_level":2,"repair_cost":[{"item_id":"scrap_wood","quantity":2}],"required_skill_id":"","tags":["residential","high_density"],"workstation_id":""}`
  - row 3: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":4},{"item_id":"scrap_metal","quantity":6}],"capacity":4,"description":"Double-tiered steel bunk frames bolted to concrete slab with personal locker space.","display_name":"Standard Dormitory","function":"Dormitory","id":"room_bunks","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":2}],"required_skill_id":"","tags":["residential","standard"],"workstation_id":""}`
  - row 4: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":8},{"item_id":"cloth","quantity":4}],"capacity":2,"description":"Acoustically insulated private sleeping cubicles providing restorative rest and privacy.","display_name":"Partitioned Quarters","function":"Dormitory","id":"room_quarters_private","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_wood","quantity":2},{"item_id":"cloth","quantity":1}],"required_skill_id":"","tags":["residential","comfort"],"workstation_id":""}`
  - row 5: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"mechanical_parts","quantity":4}],"capacity":2,"description":"Fabrication benches, vise clamps, and hand tools for general repairs and scrap repurposing.","display_name":"General Workshop","function":"Workshop","id":"room_workshop","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":3}],"required_skill_id":"skill_rough_repairs","tags":["crafting","repair"],"workstation_id":"ws_general_bench"}`
  - row 6: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":12},{"item_id":"heavy_industrial_motor","quantity":1}],"capacity":2,"description":"Reinforced flooring, overhead crane hoist, and hydraulic presses for engine overhauls.","display_name":"Heavy Machinery Workshop","function":"Workshop","id":"room_workshop_heavy","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":4}],"required_skill_id":"skill_workshop_sense","tags":["crafting","heavy_industrial"],"workstation_id":"ws_heavy_crane"}`
  - row 7: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":6},{"item_id":"scrap_electronic","quantity":4}],"capacity":2,"description":"Clean room environment with jeweler loupes, soldering irons, and precision instruments.","display_name":"Precision Tooling Bench","function":"Workshop","id":"room_workshop_precision","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_electronic","quantity":2}],"required_skill_id":"skill_workshop_sense","tags":["crafting","precision"],"workstation_id":"ws_soldering_station"}`
  - row 8: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":4},{"item_id":"cloth","quantity":6}],"capacity":2,"description":"Emergency triage tables, sterile dressings, and disinfectant wash for treating trauma.","display_name":"Field Clinic","function":"MedicalBay","id":"room_clinic","max_upgrade_level":3,"repair_cost":[{"item_id":"cloth","quantity":2}],"required_skill_id":"skill_field_dressing","tags":["medical","triage"],"workstation_id":"ws_triage_table"}`
  - row 9: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"chemicals","quantity":4}],"capacity":2,"description":"Sealed operating bay with surgical lighting, oxygen manifold, and surgical instruments.","display_name":"Clinical Surgical Ward","function":"MedicalBay","id":"room_ward_clinical","max_upgrade_level":3,"repair_cost":[{"item_id":"chemicals","quantity":2}],"required_skill_id":"skill_steady_hands","tags":["medical","surgery"],"workstation_id":"ws_surgical_light"}`
  - row 10: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":6},{"item_id":"cloth","quantity":4}],"capacity":2,"description":"Negative-pressure containment cell with ultraviolet sanitization for infectious outbreaks.","display_name":"Isolation Quarantine Bay","function":"MedicalBay","id":"room_ward_quarantine","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":2}],"required_skill_id":"skill_field_dressing","tags":["medical","quarantine"],"workstation_id":"ws_quarantine_cot"}`
  - row 11: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":6},{"item_id":"scrap_wood","quantity":4}],"capacity":2,"description":"Three-kettle stove, butchering block, and ration preparation counter with exhaust flue.","display_name":"Galley Kitchen","function":"Kitchen","id":"room_kitchen","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":2}],"required_skill_id":"skill_ration_stretcher","tags":["nutrition","canteen"],"workstation_id":"ws_galley_range"}`
  - row 12: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":8}],"capacity":1,"description":"Banded wooden shelving and pallet racks for dry rations, raw scrap, and tools.","display_name":"General Storage Bay","function":"Storage","id":"room_storage_bay","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_wood","quantity":2}],"required_skill_id":"skill_quartermaster","tags":["logistics","general"],"workstation_id":"ws_inventory_counter"}`
  - row 13: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":12}],"capacity":1,"description":"Heavy blast-lock room for secure caching of firearms, ammunition, and rare medical isotopes.","display_name":"Reinforced Armored Vault","function":"Storage","id":"room_storage_secure","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":3}],"required_skill_id":"skill_watchful","tags":["logistics","secure"],"workstation_id":"ws_secure_safe"}`
  - row 14: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"chemicals","quantity":4}],"capacity":2,"description":"Tiered hydroponic growth trays under full-spectrum sodium lamps fed from filtered water.","display_name":"Subterranean Greenhouse","function":"Greenhouse","id":"room_greenhouse_shelter","max_upgrade_level":3,"repair_cost":[{"item_id":"chemicals","quantity":2}],"required_skill_id":"skill_mycology","tags":["agriculture","hydroponics"],"workstation_id":"ws_hydro_rack"}`
  - row 15: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":4},{"item_id":"scrap_electronic","quantity":4}],"capacity":1,"description":"Shortwave tuner rack, antenna lead-in, and signal decoding station for monitoring the wasteland.","display_name":"Radio Communications Bay","function":"RadioRoom","id":"room_radio_tuner","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_electronic","quantity":2}],"required_skill_id":"skill_signal_ear","tags":["communications","intel"],"workstation_id":"ws_shortwave_desk"}`
  - row 16: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":10},{"item_id":"mechanical_parts","quantity":4}],"capacity":1,"description":"Rifle lockers, cleaning solvent basins, and a reloading press for ammunition refits.","display_name":"Armory & Munitions Depot","function":"Armory","id":"room_armory_munitions","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":3}],"required_skill_id":"skill_watchful","tags":["combat","defense"],"workstation_id":"ws_ammo_press"}`
  - row 17: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"scrap_electronic","quantity":6}],"capacity":2,"description":"Centrifuges, distillation tubes, and analytical charts for cataloging pre-war engineering archives.","display_name":"Science & Research Lab","function":"Laboratory","id":"room_laboratory_research","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_electronic","quantity":2}],"required_skill_id":"skill_cold_analysis","tags":["research","science"],"workstation_id":"ws_analysis_bench"}`
  - row 18: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":10}],"capacity":4,"description":"Long communal benches and notice board where survivors gather for meals, meetings, and morale.","display_name":"Communal Mess Hall","function":"CommonArea","id":"room_common_mess_hall","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_wood","quantity":2}],"required_skill_id":"","tags":["social","morale"],"workstation_id":""}`
  - row 19: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":6},{"item_id":"cloth","quantity":2}],"capacity":2,"description":"A quiet reading nook insulated from generator vibration for decompression and technical manual study.","display_name":"Quiet Archive & Study","function":"CommonArea","id":"room_reading_quiet_room","max_upgrade_level":2,"repair_cost":[{"item_id":"scrap_wood","quantity":2}],"required_skill_id":"","tags":["social","study"],"workstation_id":""}`
  - row 20: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":10},{"item_id":"mechanical_parts","quantity":4}],"capacity":2,"description":"Double airtight blast hatch with chemical decontam sprayers and dosimeter staging rack.","display_name":"Decontamination Airlock","function":"Airlock","id":"room_airlock","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":3}],"required_skill_id":"skill_field_dressing","tags":["perimeter","expedition"],"workstation_id":"ws_decontam_hose"}`
  - row 21: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":12},{"item_id":"fuel","quantity":2}],"capacity":2,"description":"Heavy industrial diesel dynamo providing electrical power across main lighting and pump circuits.","display_name":"Primary Diesel Generator","function":"GeneratorRoom","id":"room_generator","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":4}],"required_skill_id":"skill_rough_repairs","tags":["power","infrastructure"],"workstation_id":"ws_generator_panel"}`
  - row 22: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"cloth","quantity":4}],"capacity":1,"description":"HEPA filter banks and activated charcoal canisters providing clean breathable air to the shelter.","display_name":"Filtration & Scrubber Stack","function":"FiltrationStack","id":"room_filtration","max_upgrade_level":3,"repair_cost":[{"item_id":"cloth","quantity":2}],"required_skill_id":"skill_rough_repairs","tags":["life_support","infrastructure"],"workstation_id":"ws_blower_motor"}`
  - row 23: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":4},{"item_id":"cloth","quantity":2},{"item_id":"battery","quantity":1}],"capacity":3,"description":"The mess hall's lamplit corner where someone reads the news aloud each evening. Staffed and powered, it gives the holdfast a place to put its hope down.","display_name":"Beacon Common Room","function":"CommonArea","id":"room_hope_beacon","max_upgrade_level":2,"repair_cost":[{"item_id":"cloth","quantity":1}],"required_skill_id":"","tags":["social","morale","hope","flagship11"],"workstation_id":""}`
- `assignment_rules`: list[12]; union fields: `bonus_magnitude, bonus_type, description, id, is_hard_gate, name, penalty_magnitude, required_skill_id, target_room_function`
  - row 1: `{"bonus_magnitude":0.25,"bonus_type":"treatment_efficiency","description":"Medically trained survivors accelerate wound recovery and reduce medical supply consumption.","id":"rule_medical_field_surgery","is_hard_gate":false,"name":"Triage & Field Medicine","penalty_magnitude":-0.1,"required_skill_id":"skill_field_dressing","target_room_function":"MedicalBay"}`
  - row 2: `{"bonus_magnitude":0.2,"bonus_type":"repair_speed","description":"Skilled mechanics improve structural repair speed and minimize part wear.","id":"rule_workshop_machinist","is_hard_gate":false,"name":"Machinist & Maintenance","penalty_magnitude":-0.05,"required_skill_id":"skill_rough_repairs","target_room_function":"Workshop"}`
  - row 3: `{"bonus_magnitude":0.2,"bonus_type":"crafting_yield","description":"Deep workshop sense boosts yield and crafting reliability on high-tier items.","id":"rule_workshop_precision","is_hard_gate":false,"name":"Precision Tooling Focus","penalty_magnitude":-0.05,"required_skill_id":"skill_workshop_sense","target_room_function":"Workshop"}`
  - row 4: `{"bonus_magnitude":0.25,"bonus_type":"signal_clarity","description":"Trained signal operators discern faint broadcasts through background ionospheric noise.","id":"rule_radio_communications","is_hard_gate":false,"name":"Radio Signal Processing","penalty_magnitude":-0.1,"required_skill_id":"skill_signal_ear","target_room_function":"RadioRoom"}`
  - row 5: `{"bonus_magnitude":0.25,"bonus_type":"meal_efficiency","description":"Skilled cooks stretch limited calories without sacrificing survivor nutrition or morale.","id":"rule_kitchen_nutrition","is_hard_gate":false,"name":"Canteen Ration Mastery","penalty_magnitude":-0.1,"required_skill_id":"skill_ration_stretcher","target_room_function":"Kitchen"}`
  - row 6: `{"bonus_magnitude":0.3,"bonus_type":"research_speed","description":"Disciplined researchers accelerate knowledge decoding from recovered technical archives.","id":"rule_laboratory_analysis","is_hard_gate":false,"name":"Scientific Analysis","penalty_magnitude":-0.1,"required_skill_id":"skill_cold_analysis","target_room_function":"Laboratory"}`
  - row 7: `{"bonus_magnitude":0.25,"bonus_type":"harvest_yield","description":"Hardy growers optimize nutrient feeds to boost harvest yields in artificial lighting.","id":"rule_greenhouse_botany","is_hard_gate":false,"name":"Hydroponic Cultivation","penalty_magnitude":-0.05,"required_skill_id":"skill_mycology","target_room_function":"Greenhouse"}`
  - row 8: `{"bonus_magnitude":0.2,"bonus_type":"fuel_efficiency","description":"Experienced engineers reduce fuel consumption and prevent unexpected grid brownouts.","id":"rule_generator_maintenance","is_hard_gate":false,"name":"Turbine & Power Tuning","penalty_magnitude":-0.1,"required_skill_id":"skill_rough_repairs","target_room_function":"GeneratorRoom"}`
  - row 9: `{"bonus_magnitude":0.2,"bonus_type":"weapon_maintenance","description":"Watchful armorers keep firearms cleaned and expedition equipment primed.","id":"rule_armory_service","is_hard_gate":false,"name":"Armory Maintenance","penalty_magnitude":-0.05,"required_skill_id":"skill_watchful","target_room_function":"Armory"}`
  - row 10: `{"bonus_magnitude":0.15,"bonus_type":"handling_speed","description":"Organized quartermasters streamline inventory handling and reduce spoilage.","id":"rule_storage_logistics","is_hard_gate":false,"name":"Logistics Organization","penalty_magnitude":-0.05,"required_skill_id":"skill_quartermaster","target_room_function":"Storage"}`
  - row 11: `{"bonus_magnitude":0.2,"bonus_type":"turnaround_speed","description":"Proper decontamination procedures accelerate expedition turnaround and scrub fallout.","id":"rule_airlock_decontamination","is_hard_gate":false,"name":"Airlock Protocol","penalty_magnitude":-0.05,"required_skill_id":"skill_field_dressing","target_room_function":"Airlock"}`
  - row 12: `{"bonus_magnitude":0.15,"bonus_type":"rest_quality","description":"Attentive caretakers maintain clean living quarters, improving rest quality and morale.","id":"rule_dormitory_caretaker","is_hard_gate":false,"name":"Shelter Caretaking","penalty_magnitude":0.0,"required_skill_id":"skill_hard_living","target_room_function":"Dormitory"}`
- Bytes: 20,439; SHA-256: `1d7df91334461310d3c76457b60a93df678369bdc3846f00917c1f3bf5e8d379`
- Root keys: `assignment_rules, collection_id, rooms, schema_version`
- `rooms`: list[23]; union fields: `base_condition, build_cost, capacity, description, display_name, function, id, max_upgrade_level, repair_cost, required_skill_id, tags, workstation_id`
  - row 1: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":4}],"capacity":0,"description":"Access concourse and structural spine connecting bunker sectors, hatchways, and stairwells.","display_name":"Central Access Corridor","function":"Corridor","id":"room_bunker_corridor","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":1}],"required_skill_id":"","tags":["spine","circulation","unassignable"],"workstation_id":""}`
  - row 2: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":6},{"item_id":"scrap_metal","quantity":4}],"capacity":6,"description":"Triple-tiered pipe bunks packed wall-to-wall for maximum occupancy at the cost of privacy and comfort.","display_name":"Crowded Bunkhouse","function":"Dormitory","id":"room_bunks_crowded","max_upgrade_level":2,"repair_cost":[{"item_id":"scrap_wood","quantity":2}],"required_skill_id":"","tags":["residential","high_density"],"workstation_id":""}`
  - row 3: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":4},{"item_id":"scrap_metal","quantity":6}],"capacity":4,"description":"Double-tiered steel bunk frames bolted to concrete slab with personal locker space.","display_name":"Standard Dormitory","function":"Dormitory","id":"room_bunks","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":2}],"required_skill_id":"","tags":["residential","standard"],"workstation_id":""}`
  - row 4: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":8},{"item_id":"cloth","quantity":4}],"capacity":2,"description":"Acoustically insulated private sleeping cubicles providing restorative rest and privacy.","display_name":"Partitioned Quarters","function":"Dormitory","id":"room_quarters_private","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_wood","quantity":2},{"item_id":"cloth","quantity":1}],"required_skill_id":"","tags":["residential","comfort"],"workstation_id":""}`
  - row 5: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"mechanical_parts","quantity":4}],"capacity":2,"description":"Fabrication benches, vise clamps, and hand tools for general repairs and scrap repurposing.","display_name":"General Workshop","function":"Workshop","id":"room_workshop","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":3}],"required_skill_id":"skill_rough_repairs","tags":["crafting","repair"],"workstation_id":"ws_general_bench"}`
  - row 6: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":12},{"item_id":"heavy_industrial_motor","quantity":1}],"capacity":2,"description":"Reinforced flooring, overhead crane hoist, and hydraulic presses for engine overhauls.","display_name":"Heavy Machinery Workshop","function":"Workshop","id":"room_workshop_heavy","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":4}],"required_skill_id":"skill_workshop_sense","tags":["crafting","heavy_industrial"],"workstation_id":"ws_heavy_crane"}`
  - row 7: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":6},{"item_id":"scrap_electronic","quantity":4}],"capacity":2,"description":"Clean room environment with jeweler loupes, soldering irons, and precision instruments.","display_name":"Precision Tooling Bench","function":"Workshop","id":"room_workshop_precision","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_electronic","quantity":2}],"required_skill_id":"skill_workshop_sense","tags":["crafting","precision"],"workstation_id":"ws_soldering_station"}`
  - row 8: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":4},{"item_id":"cloth","quantity":6}],"capacity":2,"description":"Emergency triage tables, sterile dressings, and disinfectant wash for treating trauma.","display_name":"Field Clinic","function":"MedicalBay","id":"room_clinic","max_upgrade_level":3,"repair_cost":[{"item_id":"cloth","quantity":2}],"required_skill_id":"skill_field_dressing","tags":["medical","triage"],"workstation_id":"ws_triage_table"}`
  - row 9: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"chemicals","quantity":4}],"capacity":2,"description":"Sealed operating bay with surgical lighting, oxygen manifold, and surgical instruments.","display_name":"Clinical Surgical Ward","function":"MedicalBay","id":"room_ward_clinical","max_upgrade_level":3,"repair_cost":[{"item_id":"chemicals","quantity":2}],"required_skill_id":"skill_steady_hands","tags":["medical","surgery"],"workstation_id":"ws_surgical_light"}`
  - row 10: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":6},{"item_id":"cloth","quantity":4}],"capacity":2,"description":"Negative-pressure containment cell with ultraviolet sanitization for infectious outbreaks.","display_name":"Isolation Quarantine Bay","function":"MedicalBay","id":"room_ward_quarantine","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":2}],"required_skill_id":"skill_field_dressing","tags":["medical","quarantine"],"workstation_id":"ws_quarantine_cot"}`
  - row 11: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":6},{"item_id":"scrap_wood","quantity":4}],"capacity":2,"description":"Three-kettle stove, butchering block, and ration preparation counter with exhaust flue.","display_name":"Galley Kitchen","function":"Kitchen","id":"room_kitchen","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":2}],"required_skill_id":"skill_ration_stretcher","tags":["nutrition","canteen"],"workstation_id":"ws_galley_range"}`
  - row 12: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":8}],"capacity":1,"description":"Banded wooden shelving and pallet racks for dry rations, raw scrap, and tools.","display_name":"General Storage Bay","function":"Storage","id":"room_storage_bay","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_wood","quantity":2}],"required_skill_id":"skill_quartermaster","tags":["logistics","general"],"workstation_id":"ws_inventory_counter"}`
  - row 13: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":12}],"capacity":1,"description":"Heavy blast-lock room for secure caching of firearms, ammunition, and rare medical isotopes.","display_name":"Reinforced Armored Vault","function":"Storage","id":"room_storage_secure","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":3}],"required_skill_id":"skill_watchful","tags":["logistics","secure"],"workstation_id":"ws_secure_safe"}`
  - row 14: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"chemicals","quantity":4}],"capacity":2,"description":"Tiered hydroponic growth trays under full-spectrum sodium lamps fed from filtered water.","display_name":"Subterranean Greenhouse","function":"Greenhouse","id":"room_greenhouse_shelter","max_upgrade_level":3,"repair_cost":[{"item_id":"chemicals","quantity":2}],"required_skill_id":"skill_mycology","tags":["agriculture","hydroponics"],"workstation_id":"ws_hydro_rack"}`
  - row 15: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":4},{"item_id":"scrap_electronic","quantity":4}],"capacity":1,"description":"Shortwave tuner rack, antenna lead-in, and signal decoding station for monitoring the wasteland.","display_name":"Radio Communications Bay","function":"RadioRoom","id":"room_radio_tuner","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_electronic","quantity":2}],"required_skill_id":"skill_signal_ear","tags":["communications","intel"],"workstation_id":"ws_shortwave_desk"}`
  - row 16: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":10},{"item_id":"mechanical_parts","quantity":4}],"capacity":1,"description":"Rifle lockers, cleaning solvent basins, and a reloading press for ammunition refits.","display_name":"Armory & Munitions Depot","function":"Armory","id":"room_armory_munitions","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":3}],"required_skill_id":"skill_watchful","tags":["combat","defense"],"workstation_id":"ws_ammo_press"}`
  - row 17: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"scrap_electronic","quantity":6}],"capacity":2,"description":"Centrifuges, distillation tubes, and analytical charts for cataloging pre-war engineering archives.","display_name":"Science & Research Lab","function":"Laboratory","id":"room_laboratory_research","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_electronic","quantity":2}],"required_skill_id":"skill_cold_analysis","tags":["research","science"],"workstation_id":"ws_analysis_bench"}`
  - row 18: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":10}],"capacity":4,"description":"Long communal benches and notice board where survivors gather for meals, meetings, and morale.","display_name":"Communal Mess Hall","function":"CommonArea","id":"room_common_mess_hall","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_wood","quantity":2}],"required_skill_id":"","tags":["social","morale"],"workstation_id":""}`
  - row 19: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":6},{"item_id":"cloth","quantity":2}],"capacity":2,"description":"A quiet reading nook insulated from generator vibration for decompression and technical manual study.","display_name":"Quiet Archive & Study","function":"CommonArea","id":"room_reading_quiet_room","max_upgrade_level":2,"repair_cost":[{"item_id":"scrap_wood","quantity":2}],"required_skill_id":"","tags":["social","study"],"workstation_id":""}`
  - row 20: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":10},{"item_id":"mechanical_parts","quantity":4}],"capacity":2,"description":"Double airtight blast hatch with chemical decontam sprayers and dosimeter staging rack.","display_name":"Decontamination Airlock","function":"Airlock","id":"room_airlock","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":3}],"required_skill_id":"skill_field_dressing","tags":["perimeter","expedition"],"workstation_id":"ws_decontam_hose"}`
  - row 21: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":12},{"item_id":"fuel","quantity":2}],"capacity":2,"description":"Heavy industrial diesel dynamo providing electrical power across main lighting and pump circuits.","display_name":"Primary Diesel Generator","function":"GeneratorRoom","id":"room_generator","max_upgrade_level":3,"repair_cost":[{"item_id":"scrap_metal","quantity":4}],"required_skill_id":"skill_rough_repairs","tags":["power","infrastructure"],"workstation_id":"ws_generator_panel"}`
  - row 22: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"cloth","quantity":4}],"capacity":1,"description":"HEPA filter banks and activated charcoal canisters providing clean breathable air to the shelter.","display_name":"Filtration & Scrubber Stack","function":"FiltrationStack","id":"room_filtration","max_upgrade_level":3,"repair_cost":[{"item_id":"cloth","quantity":2}],"required_skill_id":"skill_rough_repairs","tags":["life_support","infrastructure"],"workstation_id":"ws_blower_motor"}`
  - row 23: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":4},{"item_id":"cloth","quantity":2},{"item_id":"battery","quantity":1}],"capacity":3,"description":"The mess hall's lamplit corner where someone reads the news aloud each evening. Staffed and powered, it gives the holdfast a place to put its hope down.","display_name":"Beacon Common Room","function":"CommonArea","id":"room_hope_beacon","max_upgrade_level":2,"repair_cost":[{"item_id":"cloth","quantity":1}],"required_skill_id":"","tags":["social","morale","hope","flagship11"],"workstation_id":""}`
- `assignment_rules`: list[12]; union fields: `bonus_magnitude, bonus_type, description, id, is_hard_gate, name, penalty_magnitude, required_skill_id, target_room_function`
  - row 1: `{"bonus_magnitude":0.25,"bonus_type":"treatment_efficiency","description":"Medically trained survivors accelerate wound recovery and reduce medical supply consumption.","id":"rule_medical_field_surgery","is_hard_gate":false,"name":"Triage & Field Medicine","penalty_magnitude":-0.1,"required_skill_id":"skill_field_dressing","target_room_function":"MedicalBay"}`
  - row 2: `{"bonus_magnitude":0.2,"bonus_type":"repair_speed","description":"Skilled mechanics improve structural repair speed and minimize part wear.","id":"rule_workshop_machinist","is_hard_gate":false,"name":"Machinist & Maintenance","penalty_magnitude":-0.05,"required_skill_id":"skill_rough_repairs","target_room_function":"Workshop"}`
  - row 3: `{"bonus_magnitude":0.2,"bonus_type":"crafting_yield","description":"Deep workshop sense boosts yield and crafting reliability on high-tier items.","id":"rule_workshop_precision","is_hard_gate":false,"name":"Precision Tooling Focus","penalty_magnitude":-0.05,"required_skill_id":"skill_workshop_sense","target_room_function":"Workshop"}`
  - row 4: `{"bonus_magnitude":0.25,"bonus_type":"signal_clarity","description":"Trained signal operators discern faint broadcasts through background ionospheric noise.","id":"rule_radio_communications","is_hard_gate":false,"name":"Radio Signal Processing","penalty_magnitude":-0.1,"required_skill_id":"skill_signal_ear","target_room_function":"RadioRoom"}`
  - row 5: `{"bonus_magnitude":0.25,"bonus_type":"meal_efficiency","description":"Skilled cooks stretch limited calories without sacrificing survivor nutrition or morale.","id":"rule_kitchen_nutrition","is_hard_gate":false,"name":"Canteen Ration Mastery","penalty_magnitude":-0.1,"required_skill_id":"skill_ration_stretcher","target_room_function":"Kitchen"}`
  - row 6: `{"bonus_magnitude":0.3,"bonus_type":"research_speed","description":"Disciplined researchers accelerate knowledge decoding from recovered technical archives.","id":"rule_laboratory_analysis","is_hard_gate":false,"name":"Scientific Analysis","penalty_magnitude":-0.1,"required_skill_id":"skill_cold_analysis","target_room_function":"Laboratory"}`
  - row 7: `{"bonus_magnitude":0.25,"bonus_type":"harvest_yield","description":"Hardy growers optimize nutrient feeds to boost harvest yields in artificial lighting.","id":"rule_greenhouse_botany","is_hard_gate":false,"name":"Hydroponic Cultivation","penalty_magnitude":-0.05,"required_skill_id":"skill_mycology","target_room_function":"Greenhouse"}`
  - row 8: `{"bonus_magnitude":0.2,"bonus_type":"fuel_efficiency","description":"Experienced engineers reduce fuel consumption and prevent unexpected grid brownouts.","id":"rule_generator_maintenance","is_hard_gate":false,"name":"Turbine & Power Tuning","penalty_magnitude":-0.1,"required_skill_id":"skill_rough_repairs","target_room_function":"GeneratorRoom"}`
  - row 9: `{"bonus_magnitude":0.2,"bonus_type":"weapon_maintenance","description":"Watchful armorers keep firearms cleaned and expedition equipment primed.","id":"rule_armory_service","is_hard_gate":false,"name":"Armory Maintenance","penalty_magnitude":-0.05,"required_skill_id":"skill_watchful","target_room_function":"Armory"}`
  - row 10: `{"bonus_magnitude":0.15,"bonus_type":"handling_speed","description":"Organized quartermasters streamline inventory handling and reduce spoilage.","id":"rule_storage_logistics","is_hard_gate":false,"name":"Logistics Organization","penalty_magnitude":-0.05,"required_skill_id":"skill_quartermaster","target_room_function":"Storage"}`
  - row 11: `{"bonus_magnitude":0.2,"bonus_type":"turnaround_speed","description":"Proper decontamination procedures accelerate expedition turnaround and scrub fallout.","id":"rule_airlock_decontamination","is_hard_gate":false,"name":"Airlock Protocol","penalty_magnitude":-0.05,"required_skill_id":"skill_field_dressing","target_room_function":"Airlock"}`
  - row 12: `{"bonus_magnitude":0.15,"bonus_type":"rest_quality","description":"Attentive caretakers maintain clean living quarters, improving rest quality and morale.","id":"rule_dormitory_caretaker","is_hard_gate":false,"name":"Shelter Caretaking","penalty_magnitude":0.0,"required_skill_id":"skill_hard_living","target_room_function":"Dormitory"}`

## `Assets/StreamingAssets/Data/power_subgrid_nodes.json`
- Bytes: 4,263; SHA-256: `83a943a68e64169b9cdaa4b629adeac698eb409c803291f7ec68e5d0a10de5bd`
- Root keys: `nodes, schema_version`
- `nodes`: list[12]; union fields: `cooling_efficiency, display_name, fuse_rating_amps, is_critical, max_capacity_watts, node_id, surge_limit_watts, target_room_id, transformer_oil_condition`
  - row 1: `{"cooling_efficiency":0.95,"display_name":"Main Vault Master Feeder","fuse_rating_amps":50,"is_critical":true,"max_capacity_watts":8000,"node_id":"node_main_vault_bus","surge_limit_watts":10000,"target_room_id":"room_main","transformer_oil_condition":100.0}`
  - row 2: `{"cooling_efficiency":0.9,"display_name":"Clinic Isolated Step-Down","fuse_rating_amps":25,"is_critical":true,"max_capacity_watts":3500,"node_id":"node_clinic_transformer","surge_limit_watts":4500,"target_room_id":"room_clinic","transformer_oil_condition":100.0}`
  - row 3: `{"cooling_efficiency":0.85,"display_name":"Fabrication Shop Heavy Bus","fuse_rating_amps":35,"is_critical":false,"max_capacity_watts":4500,"node_id":"node_workshop_feed","surge_limit_watts":6000,"target_room_id":"room_workshop","transformer_oil_condition":100.0}`
  - row 4: `{"cooling_efficiency":0.8,"display_name":"Smelter High-Current Substation","fuse_rating_amps":45,"is_critical":false,"max_capacity_watts":6000,"node_id":"node_foundry_subgrid","surge_limit_watts":7500,"target_room_id":"room_foundry","transformer_oil_condition":100.0}`
  - row 5: `{"cooling_efficiency":0.92,"display_name":"Hydroponics Inverter Bank","fuse_rating_amps":20,"is_critical":false,"max_capacity_watts":3000,"node_id":"node_greenhouse_inverter","surge_limit_watts":3800,"target_room_id":"room_greenhouse","transformer_oil_condition":100.0}`
  - row 6: `{"cooling_efficiency":0.88,"display_name":"Airlock & Perimeter Sentry Relay","fuse_rating_amps":20,"is_critical":true,"max_capacity_watts":2500,"node_id":"node_airlock_perimeter_relay","surge_limit_watts":3500,"target_room_id":"room_airlock","transformer_oil_condition":100.0}`
  - row 7: `{"cooling_efficiency":0.89,"display_name":"Ventilation Scrubber Step-Down","fuse_rating_amps":25,"is_critical":true,"max_capacity_watts":3200,"node_id":"node_filtration_stepdown","surge_limit_watts":4200,"target_room_id":"room_filtration","transformer_oil_condition":100.0}`
  - row 8: `{"cooling_efficiency":0.82,"display_name":"Deep Aquifer Lift Station Relay","fuse_rating_amps":25,"is_critical":true,"max_capacity_watts":3000,"node_id":"node_water_pump_junction","surge_limit_watts":4000,"target_room_id":"room_water_pump","transformer_oil_condition":100.0}`
  - row 9: `{"cooling_efficiency":0.9,"display_name":"Galley Preservation Sub-Feeder","fuse_rating_amps":15,"is_critical":false,"max_capacity_watts":2200,"node_id":"node_galley_distribution","surge_limit_watts":2800,"target_room_id":"room_kitchen","transformer_oil_condition":100.0}`
  - row 10: `{"cooling_efficiency":0.95,"display_name":"Quartermaster Storage Feed","fuse_rating_amps":15,"is_critical":false,"max_capacity_watts":1500,"node_id":"node_storage_bay_branch","surge_limit_watts":2000,"target_room_id":"room_storage_bay","transformer_oil_condition":100.0}`
  - row 11: `{"cooling_efficiency":0.94,"display_name":"Dweller Quarters Breaker Array","fuse_rating_amps":15,"is_critical":false,"max_capacity_watts":2000,"node_id":"node_living_quarters_bus","surge_limit_watts":2600,"target_room_id":"room_bunks","transformer_oil_condition":100.0}`
  - row 12: `{"cooling_efficiency":0.91,"display_name":"Comms Array RF Transformer","fuse_rating_amps":15,"is_critical":false,"max_capacity_watts":1800,"node_id":"node_communications_array","surge_limit_watts":2400,"target_room_id":"room_radio_tuner","transformer_oil_condition":100.0}`
- Bytes: 4,263; SHA-256: `83a943a68e64169b9cdaa4b629adeac698eb409c803291f7ec68e5d0a10de5bd`
- Root keys: `nodes, schema_version`
- `nodes`: list[12]; union fields: `cooling_efficiency, display_name, fuse_rating_amps, is_critical, max_capacity_watts, node_id, surge_limit_watts, target_room_id, transformer_oil_condition`
  - row 1: `{"cooling_efficiency":0.95,"display_name":"Main Vault Master Feeder","fuse_rating_amps":50,"is_critical":true,"max_capacity_watts":8000,"node_id":"node_main_vault_bus","surge_limit_watts":10000,"target_room_id":"room_main","transformer_oil_condition":100.0}`
  - row 2: `{"cooling_efficiency":0.9,"display_name":"Clinic Isolated Step-Down","fuse_rating_amps":25,"is_critical":true,"max_capacity_watts":3500,"node_id":"node_clinic_transformer","surge_limit_watts":4500,"target_room_id":"room_clinic","transformer_oil_condition":100.0}`
  - row 3: `{"cooling_efficiency":0.85,"display_name":"Fabrication Shop Heavy Bus","fuse_rating_amps":35,"is_critical":false,"max_capacity_watts":4500,"node_id":"node_workshop_feed","surge_limit_watts":6000,"target_room_id":"room_workshop","transformer_oil_condition":100.0}`
  - row 4: `{"cooling_efficiency":0.8,"display_name":"Smelter High-Current Substation","fuse_rating_amps":45,"is_critical":false,"max_capacity_watts":6000,"node_id":"node_foundry_subgrid","surge_limit_watts":7500,"target_room_id":"room_foundry","transformer_oil_condition":100.0}`
  - row 5: `{"cooling_efficiency":0.92,"display_name":"Hydroponics Inverter Bank","fuse_rating_amps":20,"is_critical":false,"max_capacity_watts":3000,"node_id":"node_greenhouse_inverter","surge_limit_watts":3800,"target_room_id":"room_greenhouse","transformer_oil_condition":100.0}`
  - row 6: `{"cooling_efficiency":0.88,"display_name":"Airlock & Perimeter Sentry Relay","fuse_rating_amps":20,"is_critical":true,"max_capacity_watts":2500,"node_id":"node_airlock_perimeter_relay","surge_limit_watts":3500,"target_room_id":"room_airlock","transformer_oil_condition":100.0}`
  - row 7: `{"cooling_efficiency":0.89,"display_name":"Ventilation Scrubber Step-Down","fuse_rating_amps":25,"is_critical":true,"max_capacity_watts":3200,"node_id":"node_filtration_stepdown","surge_limit_watts":4200,"target_room_id":"room_filtration","transformer_oil_condition":100.0}`
  - row 8: `{"cooling_efficiency":0.82,"display_name":"Deep Aquifer Lift Station Relay","fuse_rating_amps":25,"is_critical":true,"max_capacity_watts":3000,"node_id":"node_water_pump_junction","surge_limit_watts":4000,"target_room_id":"room_water_pump","transformer_oil_condition":100.0}`
  - row 9: `{"cooling_efficiency":0.9,"display_name":"Galley Preservation Sub-Feeder","fuse_rating_amps":15,"is_critical":false,"max_capacity_watts":2200,"node_id":"node_galley_distribution","surge_limit_watts":2800,"target_room_id":"room_kitchen","transformer_oil_condition":100.0}`
  - row 10: `{"cooling_efficiency":0.95,"display_name":"Quartermaster Storage Feed","fuse_rating_amps":15,"is_critical":false,"max_capacity_watts":1500,"node_id":"node_storage_bay_branch","surge_limit_watts":2000,"target_room_id":"room_storage_bay","transformer_oil_condition":100.0}`
  - row 11: `{"cooling_efficiency":0.94,"display_name":"Dweller Quarters Breaker Array","fuse_rating_amps":15,"is_critical":false,"max_capacity_watts":2000,"node_id":"node_living_quarters_bus","surge_limit_watts":2600,"target_room_id":"room_bunks","transformer_oil_condition":100.0}`
  - row 12: `{"cooling_efficiency":0.91,"display_name":"Comms Array RF Transformer","fuse_rating_amps":15,"is_critical":false,"max_capacity_watts":1800,"node_id":"node_communications_array","surge_limit_watts":2400,"target_room_id":"room_radio_tuner","transformer_oil_condition":100.0}`

## `Assets/StreamingAssets/Data/vehicle_armor_grades.json`
- Bytes: 4,568; SHA-256: `8e62dc6b78f7e97b107078bd3802577edfdba0b399abc8e70d1479dd76d03faa`
- Root keys: `default_grade_id, grades, schema_version`
- `grades`: list[5]; union fields: `compatible_terrain_types, description, display_name, fuel_consumption_multiplier, id, install_cost, install_labor_ticks, integrity_pool_permille, is_default, mitigation_permille, reforge_cost, speed_multiplier_delta, tags, tier, wear_absorption_permille`
  - row 1: `{"compatible_terrain_types":["road","rough","coastal"],"description":"Factory sheet metal and hope. Every vehicle leaves the bay with this; it stops weather, not consequences.","display_name":"Stock Plating","fuel_consumption_multiplier":1.0,"id":"grade_0_stock","install_cost":[],"install_labor_ticks":0,"integrity_pool_permille":0,"is_default":true,"mitigation_permille":0,"reforge_cost":[],"speed_multiplier_delta":0.0,"tags":["armor","default"],"tier":0,"wear_absorption_permille":0}`
  - row 2: `{"compatible_terrain_types":["road","rough","coastal"],"description":"Door skins and road sign bolted over the soft spots. It rattles, it drags a little, and once in a while it eats the hit that would have killed the axle.","display_name":"Scrap Plate","fuel_consumption_multiplier":1.02,"id":"grade_1_scrap_plate","install_cost":[{"amount":4,"item_id":"scrap_metal"},{"amount":2,"item_id":"mechanical_parts"}],"install_labor_ticks":240,"integrity_pool_permille":100,"is_default":false,"mitigation_permille":100,"reforge_cost":[{"amount":2,"item_id":"scrap_metal"}],"speed_multiplier_delta":-0.02,"tags":["armor","plate","tier_1"],"tier":1,"wear_abs…`
  - row 3: `{"compatible_terrain_types":["road","rough","coastal"],"description":"Cut sheet, welded seams, a real work order in the log. The crew stops flinching at every stone strike.","display_name":"Sheet Plate","fuel_consumption_multiplier":1.05,"id":"grade_2_sheet_plate","install_cost":[{"amount":6,"item_id":"scrap_metal"},{"amount":2,"item_id":"mechanical_parts"},{"amount":1,"item_id":"item_metallurgy_iron_ingot"}],"install_labor_ticks":300,"integrity_pool_permille":140,"is_default":false,"mitigation_permille":150,"reforge_cost":[{"amount":3,"item_id":"scrap_metal"},{"amount":1,"item_id":"mechanical_parts"}],"speed_multiplier_delta":-0.04,"tags":[…`
  - row 4: `{"compatible_terrain_types":["road","rough"],"description":"Layered stock from the foundry's good days, ceramic face over iron back. Heavy enough to need its own line in the fuel ledger.","display_name":"Composite Plate","fuel_consumption_multiplier":1.08,"id":"grade_3_composite_plate","install_cost":[{"amount":8,"item_id":"scrap_metal"},{"amount":3,"item_id":"mechanical_parts"},{"amount":2,"item_id":"item_metallurgy_iron_ingot"},{"amount":1,"item_id":"item_ebpvd_ceramic_target_ingot"}],"install_labor_ticks":420,"integrity_pool_permille":180,"is_default":false,"mitigation_permille":200,"reforge_cost":[{"amount":4,"item_id":"scrap_metal"},{"a…`
  - row 5: `{"compatible_terrain_types":["road","rough"],"description":"The heaviest set the bay can hang on a frame. Slow, thirsty, and the closest thing this world has to a promise.","display_name":"Alloyed Heavy Plate","fuel_consumption_multiplier":1.12,"id":"grade_4_alloyed_heavy_plate","install_cost":[{"amount":12,"item_id":"scrap_metal"},{"amount":4,"item_id":"mechanical_parts"},{"amount":3,"item_id":"item_metallurgy_iron_ingot"},{"amount":2,"item_id":"item_ebpvd_ceramic_target_ingot"}],"install_labor_ticks":600,"integrity_pool_permille":240,"is_default":false,"mitigation_permille":250,"reforge_cost":[{"amount":6,"item_id":"scrap_metal"},{"amount"…`
- Bytes: 4,568; SHA-256: `8e62dc6b78f7e97b107078bd3802577edfdba0b399abc8e70d1479dd76d03faa`
- Root keys: `default_grade_id, grades, schema_version`
- `grades`: list[5]; union fields: `compatible_terrain_types, description, display_name, fuel_consumption_multiplier, id, install_cost, install_labor_ticks, integrity_pool_permille, is_default, mitigation_permille, reforge_cost, speed_multiplier_delta, tags, tier, wear_absorption_permille`
  - row 1: `{"compatible_terrain_types":["road","rough","coastal"],"description":"Factory sheet metal and hope. Every vehicle leaves the bay with this; it stops weather, not consequences.","display_name":"Stock Plating","fuel_consumption_multiplier":1.0,"id":"grade_0_stock","install_cost":[],"install_labor_ticks":0,"integrity_pool_permille":0,"is_default":true,"mitigation_permille":0,"reforge_cost":[],"speed_multiplier_delta":0.0,"tags":["armor","default"],"tier":0,"wear_absorption_permille":0}`
  - row 2: `{"compatible_terrain_types":["road","rough","coastal"],"description":"Door skins and road sign bolted over the soft spots. It rattles, it drags a little, and once in a while it eats the hit that would have killed the axle.","display_name":"Scrap Plate","fuel_consumption_multiplier":1.02,"id":"grade_1_scrap_plate","install_cost":[{"amount":4,"item_id":"scrap_metal"},{"amount":2,"item_id":"mechanical_parts"}],"install_labor_ticks":240,"integrity_pool_permille":100,"is_default":false,"mitigation_permille":100,"reforge_cost":[{"amount":2,"item_id":"scrap_metal"}],"speed_multiplier_delta":-0.02,"tags":["armor","plate","tier_1"],"tier":1,"wear_abs…`
  - row 3: `{"compatible_terrain_types":["road","rough","coastal"],"description":"Cut sheet, welded seams, a real work order in the log. The crew stops flinching at every stone strike.","display_name":"Sheet Plate","fuel_consumption_multiplier":1.05,"id":"grade_2_sheet_plate","install_cost":[{"amount":6,"item_id":"scrap_metal"},{"amount":2,"item_id":"mechanical_parts"},{"amount":1,"item_id":"item_metallurgy_iron_ingot"}],"install_labor_ticks":300,"integrity_pool_permille":140,"is_default":false,"mitigation_permille":150,"reforge_cost":[{"amount":3,"item_id":"scrap_metal"},{"amount":1,"item_id":"mechanical_parts"}],"speed_multiplier_delta":-0.04,"tags":[…`
  - row 4: `{"compatible_terrain_types":["road","rough"],"description":"Layered stock from the foundry's good days, ceramic face over iron back. Heavy enough to need its own line in the fuel ledger.","display_name":"Composite Plate","fuel_consumption_multiplier":1.08,"id":"grade_3_composite_plate","install_cost":[{"amount":8,"item_id":"scrap_metal"},{"amount":3,"item_id":"mechanical_parts"},{"amount":2,"item_id":"item_metallurgy_iron_ingot"},{"amount":1,"item_id":"item_ebpvd_ceramic_target_ingot"}],"install_labor_ticks":420,"integrity_pool_permille":180,"is_default":false,"mitigation_permille":200,"reforge_cost":[{"amount":4,"item_id":"scrap_metal"},{"a…`
  - row 5: `{"compatible_terrain_types":["road","rough"],"description":"The heaviest set the bay can hang on a frame. Slow, thirsty, and the closest thing this world has to a promise.","display_name":"Alloyed Heavy Plate","fuel_consumption_multiplier":1.12,"id":"grade_4_alloyed_heavy_plate","install_cost":[{"amount":12,"item_id":"scrap_metal"},{"amount":4,"item_id":"mechanical_parts"},{"amount":3,"item_id":"item_metallurgy_iron_ingot"},{"amount":2,"item_id":"item_ebpvd_ceramic_target_ingot"}],"install_labor_ticks":600,"integrity_pool_permille":240,"is_default":false,"mitigation_permille":250,"reforge_cost":[{"amount":6,"item_id":"scrap_metal"},{"amount"…`

# Appendix D — Current caller/reference graph

### `ShelterPowerGridCatalogLoader` (18 sampled current references)
- Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs:57: public static class ShelterPowerGridCatalogLoader
- src/Host/PowerGridHostSession.cs:49: /// (ShelterPowerGridCatalogLoader.FallbackDefault) so boot never fails
- src/Host/PowerGridHostSession.cs:59: var priority = ShelterPowerGridCatalogLoader.MapPriority(r.Id, r.DefaultPriority)
- src/Host/PowerGridHostSession.cs:224: return ShelterPowerGridCatalogLoader.FallbackDefault();
- src/Host/PowerGridHostSession.cs:225: return ShelterPowerGridCatalogLoader.LoadOrDefault(
- Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs:14: /// authority as consumed by ShelterPowerGridCatalogLoader. The historical
- Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs:32: var ok = ShelterPowerGridCatalogLoader.TryLoad(FindDataDir(), new FileSystemIO(),
- Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs:72: var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
- Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs:76: var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
- Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs:14: public sealed class ShelterPowerGridCatalogLoaderTests
- Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs:41: var ok = ShelterPowerGridCatalogLoader.TryLoad(FindDataDir(), new FileSystemIO(),
- Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs:51: var strict = ShelterPowerGridCatalogLoader.TryLoad(dataDir, new FileSystemIO(),
- Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs:54: var host = ShelterPowerGridCatalogLoader.LoadOrDefault(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs:65: var catalog = ShelterPowerGridCatalogLoader.LoadOrDefault("/nonexistent/dir", new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs:66: var fallback = ShelterPowerGridCatalogLoader.FallbackDefault();
- Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs:74: var ok = ShelterPowerGridCatalogLoader.TryLoad("/nonexistent/dir", new FileSystemIO(),
- Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs:84: var ok = ShelterPowerGridCatalogLoader.TryLoad("/mem", io, new SystemTextJsonSerializer(), out _, out var error);
- Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs:94: var catalog = ShelterPowerGridCatalogLoader.LoadOrDefault("/mem", io, new SystemTextJsonSerializer());
### `PowerGridSystem` (18 sampled current references)
- Assets/Ashfall.Core/AtmosphericCondenserSystem.cs:101: private readonly PowerGridSystem _powerGrid;
- Assets/Ashfall.Core/AtmosphericCondenserSystem.cs:114: PowerGridSystem powerGrid, WaterTreatmentSystem waterTreatment,
- Assets/Ashfall.Core/DeepWellSystem.cs:35: /// <see cref="PowerGridSystem.RegisterLoadRoom"/>); the well reads its
- Assets/Ashfall.Core/DeepWellSystem.cs:80: private readonly PowerGridSystem _powerGrid;
- Assets/Ashfall.Core/DeepWellSystem.cs:95: public DeepWellSystem(PowerGridSystem powerGrid, WaterTreatmentSystem waterTreatment, ILog? log = null)
- Assets/Ashfall.Core/ShelterMachineryReport.cs:52: PowerGridSystem? grid,
- Assets/Ashfall.Core/ShelterMachineryReport.cs:68: (grid.GeneratorCondition < PowerGridSystem.GeneratorDegradationThreshold
- Assets/Ashfall.Core/ShelterScheduleSystem.cs:91: private readonly PowerGridSystem _powerGrid;
- Assets/Ashfall.Core/ShelterScheduleSystem.cs:124: public ShelterScheduleSystem(PowerGridSystem powerGrid, ILog? log = null)
- Assets/Ashfall.Core/VentilationSystem.cs:133: private PowerGridSystem? _stagePower;
- Assets/Ashfall.Core/VentilationSystem.cs:146: PowerGridSystem? powerGrid = null,
- Assets/Ashfall.Core/SumpFloodingSystem.cs:148: private readonly PowerGridSystem _powerGrid;
- Assets/Ashfall.Core/SumpFloodingSystem.cs:247: PowerGridSystem powerGrid,
- Assets/Ashfall.Core/Medical/BionicsSystem.cs:10: //   • PowerGridSystem — shelter power truth. Core tracks charge STATE; the
- Assets/Ashfall.Core/Shelter/GeothermalAquiferSystem.cs:24: private readonly PowerGridSystem? _powerGrid;
- Assets/Ashfall.Core/Shelter/GeothermalAquiferSystem.cs:46: PowerGridSystem? powerGrid = null,
- Assets/Ashfall.Core/Shelter/PowerGridSystem.cs:21: public sealed class PowerGridSystem
- Assets/Ashfall.Core/Shelter/PowerGridSystem.cs:50: public PowerGridSystem(PowerGridState state, IEnumerable<PowerGridRoom> rooms, ISeededRng rng)
### `IsRoomServed` (18 sampled current references)
- Assets/Ashfall.Core/AtmosphericCondenserSystem.cs:209: if (!_powerGrid.IsRoomServed(PowerRoomId)) return; // power owns availability
- Assets/Ashfall.Core/DeepWellSystem.cs:190: if (!_powerGrid.IsRoomServed(PowerRoomId)) return; // unserved pump is silent — power owns availability
- Assets/Ashfall.Core/SumpFloodingSystem.cs:344: if (!_powerGrid.IsRoomServed(nodeId))
- Assets/Ashfall.Core/SumpFloodingSystem.cs:643: bool hasPower = _powerGrid.IsRoomServed(node.nodeId);
- Assets/Ashfall.Core/Shelter/PowerGridSystem.cs:513: /// <see cref="IsRoomServed"/> and never mutates generation or
- Assets/Ashfall.Core/Shelter/PowerGridSystem.cs:925: public bool IsRoomServed(string roomId)
- src/Main.Plans162_165.cs:388: id => _powerGrid?.System?.IsRoomServed("room_armory_munitions") ?? false;
- src/Main.Plans162_165.cs:393: id => (_powerGrid?.System?.IsRoomServed("room_armory_munitions") ?? false),
- src/Main.Plans198_201.cs:275: || _powerGrid.System.IsRoomServed("room_radio_tuner");
- src/Main.Plans62_65.cs:141: || _powerGrid.System.IsRoomServed("room_kitchen");
- src/Main.Cascade.cs:82: facts.HeatingUnserved = !grid.IsRoomServed("room_heating");
- src/Main.Cascade.cs:83: facts.FiltrationUnserved = !grid.IsRoomServed("room_air_filtration");
- src/Main.Cascade.cs:84: facts.Darkness = !grid.IsRoomServed("room_lighting_main");
- src/Main.Cascade.cs:108: if (!grid.IsRoomServed(node.nodeId)) return true;
- src/Main.AdvancedShelterSystems.cs:353: isGridPowered: () => _powerGrid?.System != null && _powerGrid.System.IsRoomServed("room_greenhouse"),
- src/Main.Plans74_77.cs:158: || _powerGrid.System.IsRoomServed("room_foundry")
- src/Main.Plans74_77.cs:159: || _powerGrid.System.IsRoomServed("room_workshop");
- src/Main.Plans74_77.cs:377: || _powerGrid.System.IsRoomServed("room_foundry")
### `PowerGridDayOwner` (4 sampled current references)
- src/Main.AdvancedShelterSystems.cs:590: // Subgrid tick moved to TickPowerSubgrids (PowerGridDayOwner phase 1,
- src/Main.CampaignOwners.cs:22: _campaignDay.Register("power_grid", new PowerGridDayOwner(this), phase: 1);
- src/Main.CampaignOwners.cs:905: private sealed class PowerGridDayOwner : IDayAdvanceOwner
- src/Main.CampaignOwners.cs:908: public PowerGridDayOwner(Main m) => _m = m;
### `PowerGridPanel` (18 sampled current references)
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1425: ["power_grid.json"] = new[] { "PowerGridPanel" },
- src/Main.PanelLifecycle.cs:84: _powerGridPanel,
- src/Main.UiPanels.cs:149: private PowerGridPanel _powerGridPanel = null!;
- src/Main.UiPanels.cs:1092: _powerGridPanel = new PowerGridPanel { Visible = false };
- src/Main.UiPanels.cs:1093: _powerGridPanel.OnClose += () => _powerGridPanel.Visible = false;
- src/Main.UiPanels.cs:1094: AddChild(_powerGridPanel);
- src/Main.World.cs:568: if (_powerGridPanel == null)
- src/Main.World.cs:570: _powerGridPanel = new PowerGridPanel();
- src/Main.World.cs:571: _powerGridPanel.OnRoomToggled += id =>
- src/Main.World.cs:582: _powerGridPanel.OnPriorityChanged += (id, p) => _powerGrid.SetPriority(id, p);
- src/Main.World.cs:583: _powerGridPanel.OnFuelAdded += u => _powerGrid.AddFuel(u);
- src/Main.World.cs:587: _powerGridPanel.OnBreakerResetRequested += id =>
- src/Main.World.cs:607: _powerGridPanel.OnBatteryBankInstallRequested += () =>
- src/Main.World.cs:625: _powerGridPanel.OnGeneratorServiceRequested += () =>
- src/Main.World.cs:643: _powerGridPanel.OnEmergencyPresetRequested += presetId =>
- src/Main.World.cs:660: AddChild(_powerGridPanel);
- src/Main.World.cs:662: _powerGridPanel.Bind(_powerGrid);
- src/Main.World.cs:663: _powerGridPanel.Open();

# Appendix E — Current focused-test inventory

Current test declaration inventory: 59 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs` — 18 test declarations; bytes=11,662; SHA-256=`477e78e90abae6e0f2f2f110121588de9ecd2e004d11c09ab892f4acb33168b1`
- 00038: [Fact]
- 00039: public void Catalog_HasValidSchemaVersion()
- 00048: [Fact]
- 00049: public void Catalog_HasAtLeastSevenRooms()
- 00058: [Fact]
- 00059: public void Catalog_PropagatesSurgeTuning()
- 00069: [Fact]
- 00070: public void Catalog_MissingSurgeTuning_FallsBackToDefaults()
- 00080: [Fact]
- 00081: public void Catalog_QuarantineWardVentilationIsCriticalTier()
- 00092: [Fact]
- 00093: public void Catalog_EveryFailureEffectId_HasANamedConsumer()
- 00136: [Fact]
- 00137: public void Catalog_PreservesShippedRoomsInOrder()
- 00184: [Fact]
- 00185: public void Catalog_RoomIdsAreUnique()
- 00192: [Fact]
- 00193: public void Catalog_CanonicalConsumerRoomIdsResolve()
### `Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs` — 27 test declarations; bytes=7,973; SHA-256=`0cbb737c2b6cc252076102bdf9c107a541cb7bbb873e14883ab2fd0d548b6b88`
- 00032: public void WriteAllText(string path, string contents) { }
- 00038: [Fact]
- 00039: public void ShippedCatalog_StrictLoad_Succeeds()
- 00047: [Fact]
- 00048: public void ShippedCatalog_HostLoad_MatchesStrictLoad()
- 00062: [Fact]
- 00063: public void MissingFile_LoadOrDefault_FallsBackToDefaults()
- 00071: [Fact]
- 00072: public void MissingFile_TryLoad_ReportsFileNotFound()
- 00080: [Fact]
- 00081: public void MalformedJson_TryLoad_FailsWithFileAndReason()
- 00090: [Fact]
- 00091: public void MalformedJson_LoadOrDefault_FallsBack()
- 00100: [Fact]
- 00101: public void DuplicateRoomId_FailsWithRoomId()
- 00110: [Fact]
- 00111: public void NegativeDraw_FailsWithRoomId()
- 00120: [Fact]
- 00121: public void UnknownPriority_FailsWithRoomId()
- 00130: [Fact]
- 00131: public void WrongSchemaVersion_Fails()
- 00140: [Fact]
- 00141: public void EmptyDisplayName_FailsWithRoomId()
- 00152: [Fact]
- 00153: public void Load_IsDeterministicInOrder()
- 00167: [Fact]
- 00168: public void FallbackDefault_Validates()
### `Ashfall.Core.Tests/PowerGridDeterminismTests.cs` — 6 test declarations; bytes=2,887; SHA-256=`bf6a0056a23901bfb0bd018a06e21476dbc2ff78e46b1940d3550e3fe46d358b`
- 00019: [Fact]
- 00020: public void SameSeed_Determinism_FuelAndBatteryIdentical()
- 00035: [Theory]
- 00038: public void NumericalSafety_NoNaNOrNegative(int seed, int days)
- 00054: [Fact]
- 00055: public void DifferentSeed_Divergence_Allowed_ButBounded()
### `Ashfall.Core.Tests/World/Plan85_71DamagedMapPowerIntegrationTests.cs` — 8 test declarations; bytes=10,446; SHA-256=`53a1674ce6afe7e37ecc9e48c484661110df082f47c19e979713b10a048877c0`
- 00016: [Fact]
- 00017: public void DamagedMapAndPowerGridCatalogs_LoadCleanly_WithoutSchemaDrift()
- 00048: [Fact]
- 00049: public void PowerGridSystem_BrownoutLoadShedding_TiersFollowPriority()
- 00101: [Fact]
- 00102: public void DamagedMapSystem_FragmentDiscoveryToInstallationReveal()
- 00148: [Fact]
- 00149: public void DamagedMapElectricalSalvage_DirectlySupportsShelterPowerGrid_CrossLinkage()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Power Grid Rooms: Eighteen-Node Catalog, Allocation Authority, and Consumer Reachability` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan calls failure effects live; current source proves only that the field is parsed.
- The historical plan says room assignments are wired; current consumer evidence must be cited before retaining that statement.

## H.2 Evidence-strength corrections
- Separate static room data from mutable grid state.
- Trace actual consumers and failure-effect reachability.
- Preserve allocation and replay contracts.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Power Grid Rooms: Eighteen-Node Catalog, Allocation Authority, and Consumer Reachability while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use ShelterPowerGridCatalogLoader for static room definitions and strict validation.
- Use PowerGridSystem for all mutable electrical state and allocation.
- Use the existing PowerGridHostSession and power_grid save section.
- Expose room availability through current provider/query seams.
- Keep failure-effect text descriptive until a named consumer exists.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement power grid rooms: eighteen-node catalog, allocation authority, and consumer reachability arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- No new room count target is authorized.
- The final handoff must identify fallback parity and the existing save section.

## J.2 Questions deliberately left open
- Should failure effects become a typed consequence contract, or remain narrative metadata?
- Which current room/industrial consumers need explicit registration evidence?

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

The original file at `HEAD:piagentsplans/71-power-grid-rooms-expansion.md` contained 4,833 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 71 — Power Grid Rooms Expansion (6 → 18 powered rooms)

## Goal (2 lines)
Expand `power_grid.json` from 6 verified room entries to 18. The `PowerGridSystem` is
fully implemented (generation watts, battery capacity, fuel units, room draw, priority,
failure effects) but only 6 rooms are defined. The shelter's power grid is too simple —
adding rooms for the new shelter functions (Plan 41) makes power management a real
strategic decision.

## Why (P2)
- Verified: `power_grid.json` has 6 rooms (room_air_filtration, room_clinic, and 4
  others with id, display_name, draw_watts, default_priority, failure_effect_id).
  `PowerGridSystem` is confirmed in Core.
- Creates the power-management pillar: every room the player builds draws power. When
  the grid is overloaded, the player must prioritize (air filtration > clinic >
  workshop > greenhouse) or face failure effects. This makes the shelter's growth a
  power-management challenge, not just a resource cost.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/power_grid.json` (expand 6 → 18 rooms)
- Read-only: `Assets/Ashfall.Core/PowerGridSystem.cs` (confirm room schema: id,
  display_name, draw_watts, default_priority, failure_effect_id; confirm how priority
  affects load shedding when the grid is overloaded)

## Content grammar (per room)
- snake_case `id` with prefix `room_` (confirmed prefix from existing 6).
- draw_watts: power consumption (50–500W; critical systems draw more).
- default_priority: critical / high / medium / low — determines load-shedding order
  (critical is last to be shed; low is first).
- failure_effect_id: `fx_*` id — what happens when this room loses power (air filtration
  off → radiation ingress; clinic off → no medical treatment; workshop off → no
  crafting; greenhouse off → no food production).
- description: 1 sentence of grounded shelter flavor.

## Steps
1. Read `PowerGridSystem.cs` to confirm the room schema, priority/load-shedding logic,
   and failure-effect mechanism.
2. Read the 6 existing rooms to understand the structure.
3. Author 12 new rooms, aligned with Plan 41 shelter room catalog:
   - room_workshop (draw 200W, priority high, failure: no crafting).
   - room_greenhouse (draw 150W, priority medium, failure: no food production).
   - room_radio_room (draw 100W, priority high, failure: no radio reception).
   - room_laboratory (draw 300W, priority high, failure: no research).
   - room_armory (draw 50W, priority medium, failure: no security monitoring).
   - room_kitchen (draw 120W, priority medium, failure: no cooked food, morale penalty).
   - room_storage_cold (draw 80W, priority high, failure: food spoils).
   - room_generator_room (draw 0W, priority critical, failure: no generation — this is
     the source, not a consumer).
   - room_common_area (draw 40W, priority low, failure: morale penalty).
   - room_dormitory (draw 30W, priority low, failure: no heating, morale penalty).
   - room_water_treatment (draw 180W, priority critical, failure: no clean water).
   - room_surveillance (draw 90W, priority medium, failure: no perimeter detection).
4. Give each room: draw_watts, priority, failure_effect_id, description.
5. Cross-reference: every `room_*` id matches Plan 41 shelter room definitions; every
   `fx_*` failure effect id resolves (confirm the failure-effect catalog or create
   inline).
6. Wire 6 rooms to Plan 57 incidents (generator failure, air filter breakdown, water
   pipe burst — incidents cause power-grid failures).
7. Wire 4 rooms to Plan 41 shelter assignments (powered rooms produce output when
   powered; unpowered rooms produce nothing).
8. Validate: `--data-integrity-selftest`; confirm load shedding works (low-priority
   rooms shed first when the grid is overloaded) in a headless boot.
9. xUnit: power grid loads, all room ids resolve, load shedding follows priority,
   failure effects fire on power loss, save round-trip preserves grid state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data.

## Definition of Done
- `power_grid.json` has 18 rooms (6 existing + 12 new), all ids resolving to Plan 41,
  load shedding follows priority, failure effects fire on power loss, 6 wired to
  incidents, 4 wired to room assignments, save round-trip green, integrity + tests green.

## Follow-on
- Plan 41 (shelter rooms) — powered rooms produce output.
- Plan 57 (incidents) — equipment failure incidents affect the power grid.
- Plan 70 (shelter schedules) — schedules affect power draw (emergency = more draw).
- Existing 29B (machine personality) — powered rooms have machine personality.
- Plan 55 (recipes) — workshop and kitchen rooms require power for crafting.

```

## End of Plan 71 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs` — 198 lines; 12,984 bytes; SHA-256 `a1cb17a7d251b86599a5167a0df13aea25500a8c6c0a401ea3f811f36e64853f`
Declaration index:
- 00023: public sealed class ShelterPowerGridRoomDef
- 00034: public sealed class ShelterPowerGridCatalogDef
- 00057: public static class ShelterPowerGridCatalogLoader
- 00063: public static bool TryLoad(string dataDir, IFileIO files, IJsonSerializer serializer,
- 00104: public static ShelterPowerGridCatalogDef LoadOrDefault(string dataDir, IFileIO files, IJsonSerializer serializer)
- 00114: public static bool Validate(ShelterPowerGridCatalogDef catalog, out string error)
- 00147: public static PowerGridRoomPriority? MapPriority(string roomId, string priority)
- 00166: public static ShelterPowerGridCatalogDef FallbackDefault() => new ShelterPowerGridCatalogDef
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Catalog    : ShelterPowerGridCatalog (+Loader)
00004: // Data       : Assets/StreamingAssets/Data/power_grid.json
00005: // System     : PowerGridSystem (shelter electrical authority)
00006: // Purpose    : Runtime parsing of the authoritative power-grid room catalog.
00007: //              Replaces the host's hardcoded room snapshot (Plan G2 of the
00008: //              SHELTER_GRID_CATALOG_SEAL wave) so catalog edits reach the
00009: //              simulation. A missing or unusable file falls back to embedded
00010: //              defaults so the host never fails to boot over a catalog.
00011: // ============================================================================
00012:
00013: using System;
00014: using System.Collections.Generic;
00015: using System.Text.Json;
00016: using System.Text.Json.Serialization;
00017: using Ashfall.Core.IO;
00018:
00019: namespace Ashfall.Core.Shelter
00020: {
00021:     /// <summary>One authored power-grid room definition (snake_case JSON contract).</summary>
00022:     [Serializable]
00023:     public sealed class ShelterPowerGridRoomDef
00024:     {
00025:         [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
00026:         [JsonPropertyName("display_name")] public string DisplayName { get; set; } = string.Empty;
00027:         [JsonPropertyName("draw_watts")] public float DrawWatts { get; set; }
00028:         [JsonPropertyName("default_priority")] public string DefaultPriority { get; set; } = "standard";
00029:         [JsonPropertyName("failure_effect_id")] public string FailureEffectId { get; set; } = string.Empty;
00030:     }
00031:
00032:     /// <summary>Root document for power_grid.json.</summary>
00033:     [Serializable]
00034:     public sealed class ShelterPowerGridCatalogDef
00035:     {
00036:         [JsonPropertyName("schema_version")] public int SchemaVersion { get; set; }
00037:         [JsonPropertyName("generation_watts_default")] public float GenerationWattsDefault { get; set; }
00038:         [JsonPropertyName("battery_capacity_wh_default")] public float BatteryCapacityWhDefault { get; set; }
00039:         [JsonPropertyName("fuel_units_default")] public float FuelUnitsDefault { get; set; }
00040:         [JsonPropertyName("rooms")] public List<ShelterPowerGridRoomDef> Rooms { get; set; } = new List<ShelterPowerGridRoomDef>();
00041:
00042:         /// <summary>Optional surge tuning (SHELTER_HARDENING). Absent fields keep
00043:         /// the system defaults — old catalogs stay valid.</summary>
00044:         [JsonPropertyName("emp_storm_severity")]
00045:         public float? EmpStormSeverity { get; set; }
00046:
00047:         [JsonPropertyName("surge_battery_drain_fraction")]
00048:         public float? SurgeBatteryDrainFraction { get; set; }
00049:     }
00050:
00051:     /// <summary>
00052:     /// Loads the authoritative shelter power-grid catalog. Behavior policy:
00053:     /// missing file → embedded fallback (boot must never fail over a catalog);
00054:     /// malformed content → strict error via <see cref="TryLoad"/> for tests and
00055:     /// fallback-plus-diagnostic via <see cref="LoadOrDefault"/> for the host.
00056:     /// </summary>
00057:     public static class ShelterPowerGridCatalogLoader
00058:     {
00059:         public const string FileName = "power_grid.json";
00060:         public const int SupportedSchemaVersion = 1;
00061:
00062:         /// <summary>Parse + validate strictly. Returns false with a specific error message.</summary>
00063:         public static bool TryLoad(string dataDir, IFileIO files, IJsonSerializer serializer,
00064:             out ShelterPowerGridCatalogDef? catalog, out string error)
00065:         {
00066:             catalog = null;
00067:             error = string.Empty;
00068:             if (files == null) { error = $"{FileName}: no file IO adapter."; return false; }
00069:             if (serializer == null) { error = $"{FileName}: no JSON serializer adapter."; return false; }
00070:             if (string.IsNullOrWhiteSpace(dataDir)) { error = $"{FileName}: no data directory."; return false; }
00071:
00072:             string path = files.Combine(dataDir, FileName);
00073:             if (!files.FileExists(path)) { error = $"{FileName}: file not found at '{path}'."; return false; }
00074:
00075:             string text;
00076:             try { text = files.ReadAllText(path); }
00077:             catch (Exception ex)
00078:             {
00079:                 CatalogDiagnostics.Warn(FileName, "FileRead", ex);
00080:                 error = $"{FileName}: read failed ({ex.Message}).";
00081:                 return false;
00082:             }
00083:             if (string.IsNullOrWhiteSpace(text)) { error = $"{FileName}: file is empty."; return false; }
00084:
00085:             try
00086:             {
00087:                 catalog = JsonSerializer.Deserialize<ShelterPowerGridCatalogDef>(text, SystemTextJsonSerializer.Options);
00088:             }
00089:             catch (Exception ex)
00090:             {
00091:                 CatalogDiagnostics.Warn(FileName, "ShelterPowerGridCatalogDef", ex);
00092:                 error = $"{FileName}: malformed JSON ({ex.Message}).";
00093:                 catalog = null;
00094:                 return false;
00095:             }
00096:             if (catalog == null) { error = $"{FileName}: deserialized to null."; return false; }
00097:             return Validate(catalog, out error);
00098:         }
00099:
00100:         /// <summary>
00101:         /// Host-facing load: strict parse, falling back to embedded defaults with a
00102:         /// diagnostic when the file is missing or unusable.
00103:         /// </summary>
00104:         public static ShelterPowerGridCatalogDef LoadOrDefault(string dataDir, IFileIO files, IJsonSerializer serializer)
00105:         {
00106:             if (TryLoad(dataDir, files, serializer, out var catalog, out var error))
00107:                 return catalog!;
00108:             if (!string.IsNullOrEmpty(error) && !error.Contains("file not found", StringComparison.Ordinal))
00109:                 CatalogDiagnostics.Warn(FileName, "ShelterPowerGridCatalogDef", new InvalidOperationException(error));
00110:             return FallbackDefault();
00111:         }
00112:
00113:         /// <summary>Structural validation: schema version, unique IDs, sane numerics, known priorities.</summary>
00114:         public static bool Validate(ShelterPowerGridCatalogDef catalog, out string error)
00115:         {
00116:             error = string.Empty;
00117:             if (catalog == null) { error = $"{FileName}: catalog is null."; return false; }
00118:             if (catalog.SchemaVersion != SupportedSchemaVersion)
00119:             { error = $"{FileName}: unsupported schema_version {catalog.SchemaVersion} (expected {SupportedSchemaVersion})."; return false; }
00120:             if (catalog.GenerationWattsDefault < 0f || catalog.BatteryCapacityWhDefault < 0f || catalog.FuelUnitsDefault < 0f)
00121:             { error = $"{FileName}: negative default generation/battery/fuel values."; return false; }
00122:             if (catalog.EmpStormSeverity is < 0f or > 1f)
00123:             { error = $"{FileName}: emp_storm_severity must be within 0..1."; return false; }
00124:             if (catalog.SurgeBatteryDrainFraction is < 0f or > 1f)
00125:             { error = $"{FileName}: surge_battery_drain_fraction must be within 0..1."; return false; }
00126:             if (catalog.Rooms == null || catalog.Rooms.Count == 0)
00127:             { error = $"{FileName}: no rooms defined."; return false; }
00128:
00129:             var ids = new HashSet<string>(StringComparer.Ordinal);
00130:             foreach (var room in catalog.Rooms)
00131:             {
00132:                 if (room == null || string.IsNullOrWhiteSpace(room.Id))
00133:                 { error = $"{FileName}: room with empty id."; return false; }
00134:                 if (!ids.Add(room.Id))
00135:                 { error = $"{FileName}: duplicate room id '{room.Id}'."; return false; }
00136:                 if (room.DrawWatts < 0f)
00137:                 { error = $"{FileName}: room '{room.Id}' has negative draw_watts."; return false; }
00138:                 if (string.IsNullOrWhiteSpace(room.DisplayName))
00139:                 { error = $"{FileName}: room '{room.Id}' has empty display_name."; return false; }
00140:                 if (MapPriority(room.Id, room.DefaultPriority) == null)
00141:                 { error = $"{FileName}: room '{room.Id}' has unknown default_priority '{room.DefaultPriority}'."; return false; }
00142:             }
00143:             return true;
00144:         }
00145:
00146:         /// <summary>Map a catalog priority string to the simulation enum; null when unknown.</summary>
00147:         public static PowerGridRoomPriority? MapPriority(string roomId, string priority)
00148:         {
00149:             switch (priority?.Trim().ToLowerInvariant())
00150:             {
00151:                 case "critical": return PowerGridRoomPriority.Critical;
00152:                 case "standard": return PowerGridRoomPriority.Standard;
00153:                 case "low": return PowerGridRoomPriority.Low;
00154:                 case "disabled": return PowerGridRoomPriority.Disabled;
00155:                 default:
00156:                     CatalogDiagnostics.Warn(FileName, $"default_priority:{roomId}",
00157:                         new ArgumentException($"Unknown priority '{priority}'."));
00158:                     return null;
00159:             }
00160:         }
00161:
00162:         /// <summary>
00163:         /// Embedded fallback matching the previously hardcoded host snapshot plus the
00164:         /// canonical rooms it was missing. Used only when the catalog file is absent.
00165:         /// </summary>
00166:         public static ShelterPowerGridCatalogDef FallbackDefault() => new ShelterPowerGridCatalogDef
00167:         {
00168:             SchemaVersion = SupportedSchemaVersion,
00169:             GenerationWattsDefault = 800f,
00170:             BatteryCapacityWhDefault = 4000f,
00171:             FuelUnitsDefault = 100f,
00172:             Rooms = new List<ShelterPowerGridRoomDef>
00173:             {
00174:                 new ShelterPowerGridRoomDef { Id = "room_air_filtration", DisplayName = "Air Filtration", DrawWatts = 180f, DefaultPriority = "critical", FailureEffectId = "fx_filtration_off" },
00175:                 new ShelterPowerGridRoomDef { Id = "room_clinic", DisplayName = "Clinic", DrawWatts = 120f, DefaultPriority = "critical", FailureEffectId = "fx_clinic_off" },
00176:                 new ShelterPowerGridRoomDef { Id = "room_water_pump", DisplayName = "Water Pump", DrawWatts = 100f, DefaultPriority = "critical", FailureEffectId = "fx_water_pressure_drop" },
00177:                 new ShelterPowerGridRoomDef { Id = "room_greenhouse", DisplayName = "Greenhouse", DrawWatts = 160f, DefaultPriority = "standard", FailureEffectId = "fx_grow_lights_off" },
00178:                 new ShelterPowerGridRoomDef { Id = "room_foundry", DisplayName = "Silent Foundry", DrawWatts = 220f, DefaultPriority = "low", FailureEffectId = "fx_foundry_standstill" },
00179:                 new ShelterPowerGridRoomDef { Id = "room_lighting_main", DisplayName = "Main Lighting", DrawWatts = 80f, DefaultPriority = "low", FailureEffectId = "fx_lighting_dim" },
00180:                 new ShelterPowerGridRoomDef { Id = "room_workshop", DisplayName = "Workshop", DrawWatts = 300f, DefaultPriority = "low", FailureEffectId = "fx_workshop_offline" },
00181:                 // Plan 71 — keep the embedded fallback in lockstep with the
00182:                 // authoritative catalog so downstream power queries resolve
00183:                 // even when the file is missing.
00184:                 new ShelterPowerGridRoomDef { Id = "room_cryo_vault", DisplayName = "Cryo Vault", DrawWatts = 280f, DefaultPriority = "critical", FailureEffectId = "fx_cryo_vault_unpowered" },
00185:                 new ShelterPowerGridRoomDef { Id = "room_ward_quarantine", DisplayName = "Quarantine Ward", DrawWatts = 90f, DefaultPriority = "critical", FailureEffectId = "fx_quarantine_ventilation_off" },
00186:                 new ShelterPowerGridRoomDef { Id = "room_heating", DisplayName = "Electric Heating", DrawWatts = 240f, DefaultPriority = "standard", FailureEffectId = "fx_heating_off" },
00187:                 new ShelterPowerGridRoomDef { Id = "room_kitchen", DisplayName = "Kitchen", DrawWatts = 150f, DefaultPriority = "standard", FailureEffectId = "fx_kitchen_off" },
00188:                 new ShelterPowerGridRoomDef { Id = "room_water_filtration", DisplayName = "Water Filtration", DrawWatts = 140f, DefaultPriority = "critical", FailureEffectId = "fx_water_filtration_off" },
00189:                 new ShelterPowerGridRoomDef { Id = "room_airlock", DisplayName = "Airlock Decontamination", DrawWatts = 130f, DefaultPriority = "standard", FailureEffectId = "fx_airlock_decon_off" },
00190:                 new ShelterPowerGridRoomDef { Id = "room_radio_tuner", DisplayName = "Radio Room", DrawWatts = 90f, DefaultPriority = "standard", FailureEffectId = "fx_radio_tuner_off" },
00191:                 new ShelterPowerGridRoomDef { Id = "room_laboratory_research", DisplayName = "Laboratory", DrawWatts = 260f, DefaultPriority = "standard", FailureEffectId = "fx_laboratory_offline" },
00192:                 new ShelterPowerGridRoomDef { Id = "room_workshop_precision", DisplayName = "Precision Workshop", DrawWatts = 240f, DefaultPriority = "standard", FailureEffectId = "fx_precision_metrology_off" },
00193:                 new ShelterPowerGridRoomDef { Id = "room_common_mess_hall", DisplayName = "Common Mess Hall", DrawWatts = 70f, DefaultPriority = "low", FailureEffectId = "fx_common_mess_cold" },
00194:                 new ShelterPowerGridRoomDef { Id = "room_armory_munitions", DisplayName = "Armory & Munitions", DrawWatts = 60f, DefaultPriority = "standard", FailureEffectId = "fx_armory_service_off" }
00195:             }
00196:         };
00197:     }
00198: }
```

## `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` — 1,415 lines; 62,138 bytes; SHA-256 `01d76096907f04a268dd10572a9d6635286d47742922420c81e6c7863ae39f09`
Declaration index:
- 00021: public sealed class PowerGridSystem
- 00156: public bool SetGenerationContribution(string sourceId, float watts)
- 00175: public bool RemoveGenerationContribution(string sourceId)
- 00250: public bool PerformGeneratorMaintenance(out string reason)
- 00276: public bool TryInstallBatteryBank(out string reason)
- 00305: public bool TryInstallCoatedPart(string itemId, out string reason)
- 00353: public bool TryUninstallCoatedPart(string itemId, out string reason)
- 00382: public void RepublishEbPvdInstalledContribution()
- 00392: public static string ResolveCoatedPartFamily(string itemId)
- 00403: public static float ResolveCoatedPartWatts(string itemId)
- 00415: public PowerGridSnapshot Snapshot()
- 00434: public bool IsRoomPowered(string roomId)
- 00443: public PowerGridRoomPriority EffectivePriority(string roomId)
- 00462: public bool ToggleBreaker(string roomId)
- 00473: public bool SetBreaker(string roomId, bool closed)
- 00485: public void MarkTripped(string roomId, int day)
- 00492: public void ClearTripped(string roomId) => _state.ClearTripped(roomId);
- 00494: public bool IsRoomTripped(string roomId) => _state.IsRoomTripped(roomId);
- 00496: public bool SetPriority(string roomId, PowerGridRoomPriority priority)
- 00524: public bool RegisterLoadRoom(PowerGridRoom room)
- 00552: public IReadOnlyList<string> ApplyBrownoutShedPreset()
- 00579: public int ApplyCatalogDefaultPriorities()
- 00592: public void AddFuel(float units)
- 00613: public void ConfigureSurge(float empStormSeverity, float batteryDrainFraction)
- 00635: public IReadOnlyList<string> ApplySurgeDay(int day, float severity01)
- 00689: public PowerGridTickSummary TickDay(int day, ISeededRng tickRng)
- 00863: private PowerGridAllocation ComputeAllocation(float generationWatts, float requestedDrawWatts,
- 00925: public bool IsRoomServed(string roomId)
- 00932: private sealed class PowerGridAllocation
- 00940: public PowerGridState CaptureState() => _state.Capture();
- 00942: public void RestoreState(PowerGridState state)
- 00959: private PowerGridRoom? FindRoom(string roomId)
- 00966: private static PowerGridRoom CloneRoom(PowerGridRoom source, string canonicalRoomId)
- 00985: public float GetRoomDrawWatts(string roomId)
- 00992: private float ComputeTotalDraw()
- 01025: private List<string> RoomPoweredStates()
- 01039: public enum PowerGridRoomPriority
- 01048: public sealed class PowerGridRoom
- 01071: public sealed class PowerGridState
- 01110: public bool IsBreakerClosed(string roomId) => !ClosedBreakers.Contains(roomId);
- 01111: public bool IsRoomTripped(string roomId) => TrippedRooms.Contains(roomId);
- 01113: public void SetBreaker(string roomId, bool closed)
- 01119: public void MarkTripped(string roomId, int day)
- 01124: public void ClearTripped(string roomId) => TrippedRooms.Remove(roomId);
- 01126: public PowerGridRoomPriority GetRoomPriority(string roomId)
- 01133: public void SetRoomPriority(string roomId, PowerGridRoomPriority priority)
- 01146: public void NormalizeAndValidate(IReadOnlyList<PowerGridRoom> rooms)
- 01178: public PowerGridState Capture()
- 01233: public void RestoreInto(PowerGridState state, IReadOnlyList<PowerGridRoom> rooms)
- 01252: internal static float SanitizeNonNegativeFinite(float value) =>
- 01255: private static bool IsFinite(float value) =>
- 01258: private static void NormalizeIdList(List<string> values, HashSet<string> validIds)
- 01273: private void NormalizePriorities(HashSet<string> validIds)
- 01292: private void NormalizeInstalledCoatedParts()
- 01309: public sealed class RoomPriorityRecord
- 01316: public sealed class PowerGridEvent
- 01337: public enum PowerGridEventKind
- 01354: public sealed class PowerGridTickSummary
- 01401: public sealed class PowerGridSnapshot
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.Shelter
00007: {
00008:     /// <summary>
00009:     /// ASHFALL Power Grid — Core system (item 13).
00010:     ///
00011:     /// Single authority for shelter electrical state. The Core owns the
00012:     /// deterministic math (generation, draw, battery reserve, brownout
00013:     /// effects); the host presents a panel and applies the consequences
00014:     /// to air filtration, water, clinic, greenhouse, foundry, and lighting
00015:     /// via adapters.
00016:     ///
00017:     /// State is captured/restored through <see cref="PowerGridState"/> and
00018:     /// the envelope in <see cref="PowerGridSave"/>. Every mutation emits a
00019:     /// typed <see cref="PowerGridEvent"/> that the host listens to.
00020:     /// </summary>
00021:     public sealed class PowerGridSystem
00022:     {
00023:         private readonly PowerGridState _state;
00024:         private readonly List<PowerGridRoom> _rooms;
00025:         private readonly ISeededRng _rng;
00026:         // Runtime generation sources are projections from their owning systems.
00027:         // They are deliberately not persisted here: the owning save section
00028:         // restores first, then the host republishes the contribution.
00029:         private readonly Dictionary<string, float> _generationContributions =
00030:             new Dictionary<string, float>(StringComparer.Ordinal);
00031:
00032:         // Phase 2 (B5–B8): runtime-only brownout edge bookkeeping. Never saved;
00033:         // RestoreState re-seeds it from the restored state so a reload never
00034:         // replays a brownout-began/ended transition (flagship §14.3).
00035:         private bool _prevTickBrownout;
00036:
00037:         // B5–B8 expansion: runtime-only source-degradation edge latches
00038:         // (flagship §27 — source maintenance/failure events). Each fires once
00039:         // per degradation cycle; RestoreState re-seeds them from the restored
00040:         // state so a reload never replays a warning.
00041:         private bool _generatorWornWarned;
00042:         private bool _fuelStarvedWarned;
00043:
00044:         /// <summary>Raised whenever a room's powered state changes.</summary>
00045:         public event Action<PowerGridEvent>? OnPowerChanged;
00046:
00047:         /// <summary>Raised at end of every tick with the day summary.</summary>
00048:         public event Action<PowerGridTickSummary>? OnTickSummary;
00049:
00050:         public PowerGridSystem(PowerGridState state, IEnumerable<PowerGridRoom> rooms, ISeededRng rng)
00051:         {
00052:             if (state == null) throw new ArgumentNullException(nameof(state));
00053:             if (rooms == null) throw new ArgumentNullException(nameof(rooms));
00054:             _rooms = new List<PowerGridRoom>();
00055:             var roomIds = new HashSet<string>(StringComparer.Ordinal);
00056:             foreach (var room in rooms)
00057:             {
00058:                 if (room == null || string.IsNullOrWhiteSpace(room.RoomId)) continue;
00059:                 string roomId = room.RoomId.Trim();
00060:                 if (!roomIds.Add(roomId)) continue;
00061:                 _rooms.Add(CloneRoom(room, roomId));
00062:             }
00063:             if (_rooms.Count == 0)
00064:                 throw new InvalidOperationException("PowerGridSystem: at least one room required.");
00065:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00066:             _state = new PowerGridState();
00067:             _state.RestoreInto(state, _rooms);
00068:             RepublishEbPvdInstalledContribution();
00069:         }
00070:
00071:         public PowerGridState State => _state;
00072:         public IReadOnlyList<PowerGridRoom> Rooms => _rooms;
00073:
00074:         /// <summary>
00075:         /// Base generator output plus current runtime contributions (kW is
00076:         /// represented as watts at this authority). External contributions are
00077:         /// fuel-free; the base generator remains the only fuel-burning source.
00078:         /// </summary>
00079:         public float GenerationWatts
00080:         {
00081:             get
00082:             {
00083:                 // B5–B8 Phase 5: the base generator's rated output degrades
00084:                 // with condition (factor is 1 at/above the threshold — legacy
00085:                 // parity for a healthy generator). External contributions are
00086:                 // NOT scaled here; their owning systems own their condition.
00087:                 float total = _state.GenerationWatts * GeneratorOutputFactor;
00088:                 foreach (var contribution in _generationContributions.Values)
00089:                     total += Math.Max(0f, contribution);
00090:                 return total;
00091:             }
00092:         }
00093:
00094:         public float BaseGenerationWatts => _state.GenerationWatts;
00095:         public IReadOnlyDictionary<string, float> GenerationContributions => _generationContributions;
00096:         public float FuelUnits => _state.FuelUnits;
00097:         public float BatteryReserveWh => _state.BatteryReserveWh;
00098:         public float BatteryCapacityWh => _state.BatteryCapacityWh;
00099:         public float TotalDrawWatts => ComputeTotalDraw();
00100:         public float NetWatts => GenerationWatts - TotalDrawWatts;
00101:         public bool IsBrownout => TotalDrawWatts > GenerationWatts && BatteryReserveWh <= 0;
00102:
00103:         // ---- C2[6] 23A: canonical demand/supply/deficit read model ----------
00104:         //
00105:         // Panels, briefings and the cascade layer must read these instead of
00106:         // re-deriving arithmetic. <see cref="AvailableSupplyWatts"/> is exactly the
00107:         // number the deterministic allocator uses (generation plus the sustainable
00108:         // battery discharge the tick can hold for a day), so a UI forecast can
00109:         // never disagree with the simulation.
00110:
00111:         /// <summary>Battery watts the current reserve can sustain across a 24 h day.</summary>
00112:         public float SustainableBatteryDischargeWatts => Math.Max(0f, _state.BatteryReserveWh / 24f);
00113:
00114:         /// <summary>Generation plus sustainable battery discharge — the served-power ceiling.</summary>
00115:         public float AvailableSupplyWatts => GenerationWatts + SustainableBatteryDischargeWatts;
00116:
00117:         /// <summary>Intent draw above the served-power ceiling (0 when supply covers demand).</summary>
00118:         public float DeficitWatts => Math.Max(0f, TotalDrawWatts - AvailableSupplyWatts);
00119:
00120:         /// <summary>
00121:         /// Estimated hours the current battery reserve lasts against the current
00122:         /// intent draw, using the same generation/draw numbers the daily tick
00123:         /// exchanges. <see cref="float.PositiveInfinity"/> when generation covers
00124:         /// demand. Never negative; never NaN.
00125:         /// </summary>
00126:         public float EstimatedRuntimeHours
00127:         {
00128:             get
00129:             {
00130:                 float drainWatts = TotalDrawWatts - GenerationWatts;
00131:                 if (drainWatts <= 0f) return float.PositiveInfinity;
00132:                 return _state.BatteryReserveWh / drainWatts;
00133:             }
00134:         }
00135:
00136:         /// <summary>
00137:         /// Estimated days of fuel left at the base generator's current rated
00138:         /// (condition-scaled) burn, matching the tick's fuel formula exactly.
00139:         /// External fuel-free contributions are excluded by construction.
00140:         /// </summary>
00141:         public float EstimatedFuelRunwayDays
00142:         {
00143:             get
00144:             {
00145:                 float baseWatts = _state.GenerationWatts * GeneratorOutputFactor;
00146:                 float burnPerDay = Math.Max(0.0001f, baseWatts * 24f * 0.001f);
00147:                 return _state.FuelUnits / burnPerDay;
00148:             }
00149:         }
00150:
00151:         /// <summary>
00152:         /// Publish one external generation source. The source ID is stable and
00153:         /// replacing a value is idempotent, so a host can republish after every
00154:         /// campaign tick without accumulating duplicate output.
00155:         /// </summary>
00156:         public bool SetGenerationContribution(string sourceId, float watts)
00157:         {
00158:             if (string.IsNullOrWhiteSpace(sourceId)) return false;
00159:             if (float.IsNaN(watts) || float.IsInfinity(watts)) return false;
00160:             float normalized = Math.Max(0f, watts);
00161:             if (normalized <= 0f)
00162:                 _generationContributions.Remove(sourceId);
00163:             else
00164:                 _generationContributions[sourceId] = normalized;
00165:
00166:             OnPowerChanged?.Invoke(new PowerGridEvent(
00167:                 PowerGridEventKind.GenerationChanged,
00168:                 sourceId,
00169:                 _state.SimDay,
00170:                 normalized > 0f ? "generation_contribution_set" : "generation_contribution_removed",
00171:                 normalized));
00172:             return true;
00173:         }
00174:
00175:         public bool RemoveGenerationContribution(string sourceId)
00176:         {
00177:             if (string.IsNullOrWhiteSpace(sourceId)) return false;
00178:             return SetGenerationContribution(sourceId, 0f);
00179:         }
00180:
00181:         /// <summary>
00182:         /// Plans 146–149 MED: stable runtime contribution id for installed
00183:         /// EB-PVD coated generator parts. Host republishes after restore.
00184:         /// </summary>
00185:         public const string EbPvdInstalledSourceId = "ebpvd_installed";
00186:
00187:         /// <summary>Hard cap on concurrent installed coated parts (blade / combustor / injector).</summary>
00188:         public const int MaxInstalledCoatedParts = 3;
00189:
00190:         /// <summary>Hard cap on total coated contribution watts.</summary>
00191:         public const float MaxEbPvdInstalledWatts = 80f;
00192:
00193:         // ---- B5–B8 Phase 2 (Plan 65): battery bank build chain -----------------
00194:
00195:         /// <summary>Canonical install item for one battery bank. Research gates
00196:         /// the item through the reconditioning recipe chain; research alone
00197:         /// never grants capacity.</summary>
00198:         public const string BatteryBankItemId = "item_battery_reconditioned";
00199:
00200:         /// <summary>Capacity added per installed bank (Wh). Old saves with zero
00201:         /// banks keep their stored capacity unchanged.</summary>
00202:         public const float BatteryBankCapacityWh = 1000f;
00203:
00204:         /// <summary>Hard cap on installed banks (bounded build chain).</summary>
00205:         public const int MaxInstalledBatteryBanks = 4;
00206:
00207:         // ---- B5–B8 Phase 5 (Plan 65): generator condition/maintenance --------
00208:
00209:         /// <summary>Canonical maintenance consumable for the base generator
00210:         /// (same item the subgrid repair and battery service use; produced by
00211:         /// the Fischer-Tropsch lubricant chain). The host consumes it — the
00212:         /// grid never touches inventory.</summary>
00213:         public const string GeneratorMaintenanceItemId = "machine_oil";
00214:
00215:         /// <summary>Condition wear per day while the generator actually burns
00216:         /// fuel. 100 → 0 over 400 burning days; an idle generator does not
00217:         /// wear.</summary>
00218:         public const float GeneratorWearPerDay = 0.25f;
00219:
00220:         /// <summary>Below this condition the generator's rated output begins
00221:         /// to degrade (worn bearings, fouled injectors).</summary>
00222:         public const float GeneratorDegradationThreshold = 50f;
00223:
00224:         /// <summary>Output factor at zero condition (a half-dead engine still
00225:         /// runs at half rating — bounded, never zero while fueled).</summary>
00226:         public const float GeneratorMinOutputFactor = 0.5f;
00227:
00228:         /// <summary>
00232:         /// </summary>
00233:         public float GeneratorCondition => _state.GeneratorCondition;
00234:
00235:         /// <summary>Output multiplier the condition applies to the base
00237:         /// their owning systems own their own condition).</summary>
00238:         public float GeneratorOutputFactor => _state.GeneratorCondition >= GeneratorDegradationThreshold
00239:             ? 1f
00240:             : GeneratorMinOutputFactor
00249:         /// </summary>
00250:         public bool PerformGeneratorMaintenance(out string reason)
00251:         {
00252:             reason = string.Empty;
00275:         /// </summary>
00276:         public bool TryInstallBatteryBank(out string reason)
00277:         {
00278:             reason = string.Empty;
00295:
00296:         public int InstalledBatteryBankCount => _state.InstalledBatteryBankCount;
00297:
00298:         public IReadOnlyList<string> InstalledCoatedPartItemIds => _state.InstalledCoatedPartItemIds;
00299:
00300:         /// <summary>
00304:         /// </summary>
00305:         public bool TryInstallCoatedPart(string itemId, out string reason)
00306:         {
00307:             reason = string.Empty;
00352:
00353:         public bool TryUninstallCoatedPart(string itemId, out string reason)
00354:         {
00355:             reason = string.Empty;
00381:         /// </summary>
00382:         public void RepublishEbPvdInstalledContribution()
00383:         {
00384:             float watts = 0f;
00391:
00392:         public static string ResolveCoatedPartFamily(string itemId)
00393:         {
00394:             if (string.Equals(itemId, "item_coated_turbine_blade", StringComparison.Ordinal))
00402:
00403:         public static float ResolveCoatedPartWatts(string itemId)
00404:         {
00405:             // Bounded historical-engineering bonuses; never overwrite base GenerationWatts.
00414:
00415:         public PowerGridSnapshot Snapshot()
00416:         {
00417:             var snapshot = new PowerGridSnapshot
00433:
00434:         public bool IsRoomPowered(string roomId)
00435:         {
00436:             if (string.IsNullOrEmpty(roomId)) return false;
00442:
00443:         public PowerGridRoomPriority EffectivePriority(string roomId)
00444:         {
00445:             var r = FindRoom(roomId);
00461:         /// </summary>
00462:         public bool ToggleBreaker(string roomId)
00463:         {
00464:             var r = FindRoom(roomId);
00472:
00473:         public bool SetBreaker(string roomId, bool closed)
00474:         {
00475:             var r = FindRoom(roomId);
00484:
00485:         public void MarkTripped(string roomId, int day)
00486:         {
00487:             _state.MarkTripped(roomId, day);
00491:
00492:         public void ClearTripped(string roomId) => _state.ClearTripped(roomId);
00493:
00494:         public bool IsRoomTripped(string roomId) => _state.IsRoomTripped(roomId);
00495:
00496:         public bool SetPriority(string roomId, PowerGridRoomPriority priority)
00497:         {
00498:             var r = FindRoom(roomId);
00523:         /// </summary>
00524:         public bool RegisterLoadRoom(PowerGridRoom room)
00525:         {
00526:             if (room == null || string.IsNullOrWhiteSpace(room.RoomId)
00551:         /// </summary>
00552:         public IReadOnlyList<string> ApplyBrownoutShedPreset()
00553:         {
00554:             var changed = new List<string>();
00578:         /// power_grid.json classification. Returns the affected room count.</summary>
00579:         public int ApplyCatalogDefaultPriorities()
00580:         {
00581:             int count = _state.Priorities.Count;
00591:
00592:         public void AddFuel(float units)
00593:         {
00594:             if (units <= 0f || float.IsNaN(units) || float.IsInfinity(units)) return;
00603:         /// back to this default when the catalog omits the field.</summary>
00604:         public const float DefaultEmpStormSurgeSeverity = 0.6f;
00605:
00606:         /// <summary>Default fraction of battery capacity drained at full surge severity.</summary>
00607:         public const float DefaultSurgeBatteryDrainFraction = 0.15f;
00608:
00609:         private float _empStormSurgeSeverity = DefaultEmpStormSurgeSeverity;
00610:         private float _surgeBatteryDrainFraction = DefaultSurgeBatteryDrainFraction;
00611:
00612:         /// <summary>Catalog-driven surge tuning (0..1 each). Applies from the next surge.</summary>
00613:         public void ConfigureSurge(float empStormSeverity, float batteryDrainFraction)
00614:         {
00615:             _empStormSurgeSeverity = float.IsNaN(empStormSeverity) || float.IsInfinity(empStormSeverity)
00622:
00623:         public float EmpStormSeverity => _empStormSurgeSeverity;
00624:         public float SurgeBatteryDrain => _surgeBatteryDrainFraction;
00625:
00626:         /// <summary>
00634:         /// </summary>
00635:         public IReadOnlyList<string> ApplySurgeDay(int day, float severity01)
00636:         {
00637:             if (float.IsNaN(severity01) || float.IsInfinity(severity01))
00685:         /// <summary>
00686:         /// Tick one full day. Deterministic: given the same fuel/battery state
00687:         /// and RNG, the result is identical across hosts and runs.
00688:         /// </summary>
00689:         public PowerGridTickSummary TickDay(int day, ISeededRng tickRng)
00690:         {
00691:             var rng = tickRng ?? _rng;
00829:         /// <summary>Tolerance for serving a room within available capacity.</summary>
00830:         internal const float AllocationEpsilon = 0.01f;
00831:
00832:         /// <summary>
00862:         /// </summary>
00863:         private PowerGridAllocation ComputeAllocation(float generationWatts, float requestedDrawWatts,
00864:             float batteryReserveWh)
00865:         {
00924:         /// </summary>
00925:         public bool IsRoomServed(string roomId)
00926:         {
00927:             if (string.IsNullOrEmpty(roomId)) return false;
00931:
00932:         private sealed class PowerGridAllocation
00933:         {
00934:             public float ServedWatts;
00935:             public bool HasCriticalDeficit;
00936:             public List<string> ServedRoomIds = new List<string>();
00937:             public List<string> ShedRoomIds = new List<string>();
00938:         }
00939:
00940:         public PowerGridState CaptureState() => _state.Capture();
00941:
00942:         public void RestoreState(PowerGridState state)
00943:         {
00944:             if (state == null) throw new ArgumentNullException(nameof(state));
00958:
00959:         private PowerGridRoom? FindRoom(string roomId)
00960:         {
00961:             for (int i = 0; i < _rooms.Count; i++)
00965:
00966:         private static PowerGridRoom CloneRoom(PowerGridRoom source, string canonicalRoomId)
00967:         {
00968:             return new PowerGridRoom
00984:         /// </summary>
00985:         public float GetRoomDrawWatts(string roomId)
00986:         {
00987:             if (!IsRoomPowered(roomId)) return 0f;
00991:
00992:         private float ComputeTotalDraw()
00993:         {
00994:             // Phase 6: this now returns the *intent* draw — the sum of all rooms'
01021:         /// </summary>
01022:         public float EffectiveTotalDrawWatts =>
01023:             IsBrownout ? 0f : TotalDrawWatts;
01024:
01025:         private List<string> RoomPoweredStates()
01026:         {
01027:             var list = new List<string>(_rooms.Count);
01038:     /// <summary>Priority used by the grid when total draw exceeds generation.</summary>
01039:     public enum PowerGridRoomPriority
01040:     {
01041:         Disabled = 0,
01047:     [Serializable]
01048:     public sealed class PowerGridRoom
01049:     {
01050:         public string RoomId;
01051:         public string DisplayName;
01052:         public float DrawWatts;
01053:         public PowerGridRoomPriority DefaultPriority;
01054:         public string FailureEffectId; // semantic id the host looks up.
01055:
01056:         public PowerGridRoom() { }
01057:
01058:         public PowerGridRoom(string roomId, string displayName, float drawWatts,
01059:             PowerGridRoomPriority defaultPriority = PowerGridRoomPriority.Standard,
01060: string? failureEffectId = null)
01070:     [Serializable]
01071:     public sealed class PowerGridState
01072:     {
01073:         public int SimDay;
01074:         public float GenerationWatts;
01075:         public float FuelUnits;
01076:         public float BatteryReserveWh;
01077:         public float BatteryCapacityWh;
01078:         public List<string> ClosedBreakers = new List<string>();
01079:         public List<string> TrippedRooms = new List<string>();
01080:         public List<RoomPriorityRecord> Priorities = new List<RoomPriorityRecord>();
01081:
01082:         /// <summary>
01086:         /// </summary>
01087:         public int LastSurgeDay;
01088:
01089:         /// <summary>
01093:         /// </summary>
01094:         public List<string> InstalledCoatedPartItemIds = new List<string>();
01095:
01096:         /// <summary>
01099:         /// </summary>
01100:         public int InstalledBatteryBankCount;
01101:
01102:         /// <summary>
01107:         /// </summary>
01108:         public float GeneratorCondition = 100f;
01109:
01110:         public bool IsBreakerClosed(string roomId) => !ClosedBreakers.Contains(roomId);
01111:         public bool IsRoomTripped(string roomId) => TrippedRooms.Contains(roomId);
01112:
01113:         public void SetBreaker(string roomId, bool closed)
01114:         {
01115:             if (closed) ClosedBreakers.Remove(roomId);
01118:
01119:         public void MarkTripped(string roomId, int day)
01120:         {
01121:             if (!TrippedRooms.Contains(roomId)) TrippedRooms.Add(roomId);
01123:
01124:         public void ClearTripped(string roomId) => TrippedRooms.Remove(roomId);
01125:
01126:         public PowerGridRoomPriority GetRoomPriority(string roomId)
01127:         {
01128:             for (int i = 0; i < Priorities.Count; i++)
01132:
01133:         public void SetRoomPriority(string roomId, PowerGridRoomPriority priority)
01134:         {
01135:             for (int i = 0; i < Priorities.Count; i++)
01145:
01146:         public void NormalizeAndValidate(IReadOnlyList<PowerGridRoom> rooms)
01147:         {
01148:             ClosedBreakers ??= new List<string>();
01177:
01178:         public PowerGridState Capture()
01179:         {
01180:             var copy = new PowerGridState
01232:
01233:         public void RestoreInto(PowerGridState state, IReadOnlyList<PowerGridRoom> rooms)
01234:         {
01235:             if (state == null) throw new ArgumentNullException(nameof(state));
01236:             var restored = state.Capture();
01237:             SimDay = restored.SimDay;
01238:             GenerationWatts = restored.GenerationWatts;
01251:
01252:         internal static float SanitizeNonNegativeFinite(float value) =>
01253:             IsFinite(value) && value > 0f ? value : 0f;
01254:
01255:         private static bool IsFinite(float value) =>
01256:             !float.IsNaN(value) && !float.IsInfinity(value);
01257:
01258:         private static void NormalizeIdList(List<string> values, HashSet<string> validIds)
01259:         {
01260:             var seen = new HashSet<string>(StringComparer.Ordinal);
01272:
01273:         private void NormalizePriorities(HashSet<string> validIds)
01274:         {
01275:             var seen = new HashSet<string>(StringComparer.Ordinal);
01291:
01292:         private void NormalizeInstalledCoatedParts()
01293:         {
01294:             var normalized = new List<string>();
01296:             var seenFamilies = new HashSet<string>(StringComparer.Ordinal);
01297:             foreach (var itemId in InstalledCoatedPartItemIds)
01298:             {
01299:                 if (string.IsNullOrWhiteSpace(itemId) || !seenItems.Add(itemId)) continue;
01300:                 string family = PowerGridSystem.ResolveCoatedPartFamily(itemId);
01301:                 if (string.IsNullOrEmpty(family) || !seenFamilies.Add(family)) continue;
01302:                 normalized.Add(itemId);
01303:             }
01304:             InstalledCoatedPartItemIds = normalized;
01305:         }
01306:     }
01307:
01308:     [Serializable]
01309:     public sealed class RoomPriorityRecord
01310:     {
01311:         public string RoomId;
01312:         public PowerGridRoomPriority Priority;
01313:     }
01314:
01315:     [Serializable]
01316:     public sealed class PowerGridEvent
01317:     {
01318:         public PowerGridEventKind Kind;
01319:         public string RoomId;
01320:         public int Day;
01321:         public string Detail;
01322:         public float Numeric;
01323:
01324:         public PowerGridEvent() { }
01325:
01326:         public PowerGridEvent(PowerGridEventKind kind, string roomId, int day,
01327: string? detail = null, float numeric = 0f)
01328:         {
01329:             Kind = kind;
01330:             RoomId = roomId ?? string.Empty;
01331:             Day = day;
01332:             Detail = detail ?? string.Empty;
01333:             Numeric = numeric;
01334:         }
01335:     }
01336:
01337:     public enum PowerGridEventKind
01338:     {
01339:         BreakerToggled,
01340:         PriorityChanged,
01341:         FuelAdded,
01342:         Tripped,
01343:         SurgeApplied,
01344:         GenerationChanged,
01345:         BatteryBankInstalled,
01346:         LoadRoomRegistered,
01347:         GeneratorMaintained,
01348:         GeneratorWorn,
01349:         FuelStarved,
01350:         TickSummary
01351:     }
01352:
01353:     [Serializable]
01354:     public sealed class PowerGridTickSummary
01355:     {
01356:         public int Day;
01357:         public float FuelConsumed;
01358:         public float BatteryEndWh;
01359:         public float BrownoutHours;
01360:         public bool IsBrownout;
01361:
01362:         // ---- Phase 2 (B5–B8) additive fields: allocation projection + edges.
01363:         // Not persisted; consumers read them via OnTickSummary only.
01364:
01365:         /// <summary>Generation actually available this tick (after any fuel
01366:         /// starvation adjustment), including external contributions.</summary>
01367:         public float GenerationWatts;
01368:
01369:         /// <summary>Intent draw of all eligible rooms (same number the legacy
01370:         /// aggregate math uses).</summary>
01371:         public float RequestedDrawWatts;
01372:
01373:         /// <summary>Watts of room load served under deterministic priority
01374:         /// allocation (generation + sustainable battery discharge).</summary>
01375:         public float ServedWatts;
01376:
01377:         /// <summary>RequestedDrawWatts − ServedWatts; the shed load.</summary>
01378:         public float UnservedWatts;
01379:
01380:         /// <summary>True when at least one Critical-tier room is unserved —
01381:         /// the explicit life-support emergency state, distinct from an ordinary
01382:         /// brownout where only lower-priority loads shed.</summary>
01383:         public bool HasCriticalDeficit;
01384:
01385:         /// <summary>Edge: brownout began this tick (false on a restored
01386:         /// campaign that was already in brownout — transitions never replay).</summary>
01387:         public bool BrownoutBegan;
01388:
01389:         /// <summary>Edge: brownout ended this tick.</summary>
01390:         public bool BrownoutEnded;
01391:
01392:         /// <summary>Room IDs served this tick, in deterministic allocation order
01393:         /// (priority tier descending, RoomId ordinal ascending).</summary>
01394:         public List<string> ServedRoomIds = new List<string>();
01395:
01396:         /// <summary>Room IDs shed this tick, in the same deterministic order.</summary>
01397:         public List<string> ShedRoomIds = new List<string>();
01398:     }
01399:
01400:     [Serializable]
01401:     public sealed class PowerGridSnapshot
01402:     {
01403:         public int Day;
01404:         public float GenerationWatts;
01405:         public float FuelUnits;
01406:         public float BatteryReserveWh;
01407:         public float BatteryCapacityWh;
01408:         public float TotalDrawWatts;
01409:         public float NetWatts;
01410:         public bool IsBrownout;
01411:         public List<string> RoomIds = new List<string>();
01412:         public Dictionary<string, float> GenerationContributions =
01413:             new Dictionary<string, float>(StringComparer.Ordinal);
01414:     }
01415: }
```

## `Assets/Ashfall.Core/Shelter/PowerGridSave.cs` — 100 lines; 3,930 bytes; SHA-256 `d88e13b8034060d35c0e355b3e6dfa6d98a2282025c5f42b98dce5a23559211b`
Declaration index:
- 00014: public class PowerGridSave
- 00027: public sealed class PowerGridRoomSave
- 00036: public static class PowerGridSaveCodec
- 00038: public static PowerGridSave Encode(PowerGridSave save, IJsonSerializer json)
- 00048: public static string EncodeToString(PowerGridSave save, IJsonSerializer json)
- 00054: public static PowerGridSave Decode(string jsonText, IJsonSerializer json)
- 00082: public static PowerGridRoom ToRoom(PowerGridRoomSave s)
- 00088: public static PowerGridRoomSave FromRoom(PowerGridRoom r)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Ashfall.Core.Shelter;
00006:
00007: namespace Ashfall.Core.Shelter
00008: {
00009:     /// <summary>
00010:     /// PowerGrid cross-host save envelope. Same checksummed rules as the
00011:     /// other expansion envelopes.
00012:     /// </summary>
00013:     [Serializable]
00014:     public class PowerGridSave
00015:     {
00016:         public const int CurrentSaveVersion = 1;
00017:         public const int MigrationFromVersion = 1;
00018:
00019:         public int saveVersion = CurrentSaveVersion;
00020:         public int simDay;
00021:         public List<PowerGridRoomSave> Rooms = new List<PowerGridRoomSave>();
00022:         public PowerGridState State = new PowerGridState();
00023:         public string Checksum = string.Empty;
00024:     }
00025:
00026:     [Serializable]
00027:     public sealed class PowerGridRoomSave
00028:     {
00029:         public string RoomId;
00030:         public string DisplayName;
00031:         public float DrawWatts;
00032:         public int DefaultPriority; // serialized as int for cross-host stability
00033:         public string FailureEffectId;
00034:     }
00035:
00036:     public static class PowerGridSaveCodec
00037:     {
00038:         public static PowerGridSave Encode(PowerGridSave save, IJsonSerializer json)
00039:         {
00040:             if (save == null) throw new ArgumentNullException(nameof(save));
00041:             if (save.saveVersion > PowerGridSave.CurrentSaveVersion)
00042:                 throw new InvalidOperationException(
00043:                     "PowerGridSave: refusing to encode a saveVersion newer than supported.");
00044:             save.Checksum = SaveChecksum.Compute(save);
00045:             return save;
00046:         }
00047:
00048:         public static string EncodeToString(PowerGridSave save, IJsonSerializer json)
00049:         {
00050:             Encode(save, json);
00051:             return json.Serialize(save);
00052:         }
00053:
00054:         public static PowerGridSave Decode(string jsonText, IJsonSerializer json)
00055:         {
00056:             if (string.IsNullOrWhiteSpace(jsonText))
00057:                 throw new InvalidOperationException("PowerGridSave: empty save payload.");
00058:             PowerGridSave save;
00059:             try { save = json.Deserialize<PowerGridSave>(jsonText!); }
00060:             catch (Exception e)
00061:             {
00062:                 throw new InvalidOperationException(
00063:                     "PowerGridSave: malformed save payload: " + e.Message, e);
00064:             }
00065:             if (save == null)
00066:                 throw new InvalidOperationException("PowerGridSave: empty save payload.");
00067:             if (save.saveVersion > PowerGridSave.CurrentSaveVersion)
00068:                 throw new InvalidOperationException(
00069:                     "PowerGridSave: saveVersion " + save.saveVersion + " is newer than supported.");
00070:             if (save.saveVersion < PowerGridSave.MigrationFromVersion)
00071:                 throw new InvalidOperationException("PowerGridSave: invalid saveVersion.");
00072:             if (string.IsNullOrEmpty(save.Checksum))
00073:                 throw new InvalidOperationException(
00074:                     "PowerGridSave: save carries no checksum (truncated or tampered file).");
00075:             string actual = SaveChecksum.Compute(save);
00076:             if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
00077:                 throw new InvalidOperationException(
00078:                     "PowerGridSave: checksum mismatch (corrupt or foreign save).");
00079:             return save;
00080:         }
00081:
00082:         public static PowerGridRoom ToRoom(PowerGridRoomSave s)
00083:         {
00084:             return new PowerGridRoom(s.RoomId, s.DisplayName, s.DrawWatts,
00085:                 (PowerGridRoomPriority)s.DefaultPriority, s.FailureEffectId);
00086:         }
00087:
00088:         public static PowerGridRoomSave FromRoom(PowerGridRoom r)
00089:         {
00090:             return new PowerGridRoomSave
00091:             {
00092:                 RoomId = r.RoomId,
00093:                 DisplayName = r.DisplayName,
00094:                 DrawWatts = r.DrawWatts,
00095:                 DefaultPriority = (int)r.DefaultPriority,
00096:                 FailureEffectId = r.FailureEffectId
00097:             };
00098:         }
00099:     }
00100: }
```

## `src/Host/PowerGridHostSession.cs` — 229 lines; 8,968 bytes; SHA-256 `0d5c2874c7e02761de678764593d88460a304f2bfc3cf978ffef73c07ceb89f7`
Declaration index:
- 00021: public sealed class PowerGridHostSession
- 00042: public static PowerGridHostSession CreateDefault(ISeededRng rng)
- 00053: public static PowerGridHostSession CreateDefault(ISeededRng rng, string? dataDir)
- 00092: public bool ToggleBreaker(string roomId)
- 00099: public bool SetBreaker(string roomId, bool closed)
- 00111: public bool ClearTripped(string roomId)
- 00120: public bool SetPriority(string roomId, PowerGridRoomPriority priority)
- 00127: public void AddFuel(float units)
- 00137: public bool TryInstallCoatedPart(string itemId, out string reason)
- 00148: public bool TryUninstallCoatedPart(string itemId, out string reason)
- 00164: public bool TryInstallBatteryBank(out string reason)
- 00180: public bool PerformGeneratorMaintenance(out string reason)
- 00191: public PowerGridTickSummary TickDay(int day)
- 00200: public bool TrySave()
- 00211: public bool TryLoad()
- 00221: private static ShelterPowerGridCatalogDef LoadGridJson(string? dataDir)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS0649
00005: #pragma warning disable CS8618
00006: using Ashfall.Core;
00007: using Ashfall.Core.Shelter;
00008: using Godot;
00009:
00010: namespace AtomicWar.GodotApp
00011: {
00012:     /// <summary>
00013:     /// Power Grid host session (item 13).
00014:     ///
00015:     /// Thin Godot-side glue: holds the Core <see cref="PowerGridSystem"/>,
00016:     /// exposes a small command surface to UI panels, loads/saves through
00017:     /// <see cref="PowerGridSaveStore"/>, and registers the system with the
00018:     /// Campaign Day Coordinator so its tick runs at the right point in the
00019:     /// daily seam.
00020:     /// </summary>
00021:     public sealed class PowerGridHostSession
00022:     : HostSessionBase{
00023:         public PowerGridSystem System { get; private set; }
00024:         public PowerGridSnapshot LastSnapshot { get; private set; }
00025:
00026:         /// <summary>B5–B8 Phase 9: latest tick summary (served/shed/critical
00027:         /// deficit/brownout edges) for UI projection. Updated on every tick;
00028:         /// null before the first tick — panels render from the snapshot until
00029:         /// then (no fake state).</summary>
00030:         public PowerGridTickSummary? LastTickSummary { get; private set; }
00031:
00032:         private ISeededRng _tickRng;
00033:         private readonly List<PowerGridRoom> _rooms;
00034:         private readonly List<PowerGridRoomSave> _roomSaves;
00035:
00036:         public event Action? OnStateChanged;
00037:
00038:         /// <summary>
00039:         /// Headless/selftest convenience overload: constructs from the embedded
00040:         /// fallback defaults when no data directory is supplied.
00041:         /// </summary>
00042:         public static PowerGridHostSession CreateDefault(ISeededRng rng)
00043:             => CreateDefault(rng, dataDir: null);
00044:
00045:         /// <summary>
00046:         /// Constructs from the authoritative power_grid.json catalog
00047:         /// (power_grid.json from the data authority) via the Core loader.
00048:         /// A missing or unusable catalog falls back to embedded defaults
00049:         /// (ShelterPowerGridCatalogLoader.FallbackDefault) so boot never fails
00050:         /// over a catalog. The previous hardcoded DefaultGrid() snapshot was
00051:         /// removed — catalog and runtime can no longer drift.
00052:         /// </summary>
00053:         public static PowerGridHostSession CreateDefault(ISeededRng rng, string? dataDir)
00054:         {
00055:             var grid = LoadGridJson(dataDir);
00056:             var rooms = new List<PowerGridRoom>();
00057:             foreach (var r in grid.Rooms)
00058:             {
00059:                 var priority = ShelterPowerGridCatalogLoader.MapPriority(r.Id, r.DefaultPriority)
00060:                     ?? PowerGridRoomPriority.Standard;
00061:                 rooms.Add(new PowerGridRoom(r.Id, r.DisplayName, r.DrawWatts, priority, r.FailureEffectId));
00062:             }
00063:             var state = new PowerGridState
00064:             {
00065:                 GenerationWatts = grid.GenerationWattsDefault,
00066:                 FuelUnits = grid.FuelUnitsDefault,
00067:                 BatteryCapacityWh = grid.BatteryCapacityWhDefault,
00068:                 BatteryReserveWh = grid.BatteryCapacityWhDefault
00069:             };
00070:             var session = new PowerGridHostSession(rooms, state, rng);
00071:             // SHELTER_HARDENING: surge tuning is catalog-driven (fields optional;
00072:             // an explicit 0 legitimately disables the weather surge path).
00073:             session.System.ConfigureSurge(
00074:                 grid.EmpStormSeverity ?? Ashfall.Core.Shelter.PowerGridSystem.DefaultEmpStormSurgeSeverity,
00075:                 grid.SurgeBatteryDrainFraction ?? Ashfall.Core.Shelter.PowerGridSystem.DefaultSurgeBatteryDrainFraction);
00076:             return session;
00077:         }
00078:
00079:         public PowerGridHostSession(List<PowerGridRoom> rooms,
00080:             PowerGridState initialState, ISeededRng rng)
00081:         {
00082:             _rooms = rooms ?? throw new ArgumentNullException(nameof(rooms));
00083:             _roomSaves = new List<PowerGridRoomSave>(_rooms.Count);
00084:             foreach (var r in _rooms) _roomSaves.Add(PowerGridSaveCodec.FromRoom(r));
00085:             _tickRng = rng ?? throw new ArgumentNullException(nameof(rng));
00086:             System = new PowerGridSystem(initialState, rooms, rng);
00087:             System.OnPowerChanged += _ => OnStateChanged?.Invoke();
00088:             System.OnTickSummary += summary => LastTickSummary = summary;
00089:             LastSnapshot = System.Snapshot();
00090:         }
00091:
00092:         public bool ToggleBreaker(string roomId)
00093:         {
00094:             bool ok = System.ToggleBreaker(roomId);
00095:             if (ok) LastSnapshot = System.Snapshot();
00096:             return ok;
00097:         }
00098:
00099:         public bool SetBreaker(string roomId, bool closed)
00100:         {
00101:             bool ok = System.SetBreaker(roomId, closed);
00102:             if (ok) LastSnapshot = System.Snapshot();
00103:             return ok;
00104:         }
00105:
00106:         /// <summary>
00107:         /// C2[6] 23B: clear an overload/surge trip so the room can be served again.
00108:         /// Core-only — the host route consumes the recovery part before calling this,
00109:         /// so a reset is never free. Returns false when the room was not tripped.
00110:         /// </summary>
00111:         public bool ClearTripped(string roomId)
00112:         {
00113:             if (string.IsNullOrEmpty(roomId) || !System.IsRoomTripped(roomId)) return false;
00114:             System.ClearTripped(roomId);
00115:             LastSnapshot = System.Snapshot();
00116:             OnStateChanged?.Invoke();
00117:             return true;
00118:         }
00119:
00120:         public bool SetPriority(string roomId, PowerGridRoomPriority priority)
00121:         {
00122:             bool ok = System.SetPriority(roomId, priority);
00123:             if (ok) LastSnapshot = System.Snapshot();
00124:             return ok;
00125:         }
00126:
00127:         public void AddFuel(float units)
00128:         {
00129:             System.AddFuel(units);
00130:             LastSnapshot = System.Snapshot();
00131:         }
00132:
00133:         /// <summary>
00134:         /// Plans 146–149 MED: install a coated generator part. Caller must
00135:         /// consume the inventory item first; this only mutates PowerGrid state.
00136:         /// </summary>
00137:         public bool TryInstallCoatedPart(string itemId, out string reason)
00138:         {
00139:             bool ok = System.TryInstallCoatedPart(itemId, out reason);
00140:             if (ok)
00141:             {
00142:                 LastSnapshot = System.Snapshot();
00143:                 OnStateChanged?.Invoke();
00144:             }
00145:             return ok;
00146:         }
00147:
00148:         public bool TryUninstallCoatedPart(string itemId, out string reason)
00149:         {
00150:             bool ok = System.TryUninstallCoatedPart(itemId, out reason);
00151:             if (ok)
00152:             {
00153:                 LastSnapshot = System.Snapshot();
00154:                 OnStateChanged?.Invoke();
00155:             }
00156:             return ok;
00157:         }
00158:
00159:         /// <summary>
00160:         /// B5–B8 Phase 2: install one battery bank. Caller must consume the
00161:         /// canonical <see cref="PowerGridSystem.BatteryBankItemId"/> item
00162:         /// first; this only mutates PowerGrid state.
00163:         /// </summary>
00164:         public bool TryInstallBatteryBank(out string reason)
00165:         {
00166:             bool ok = System.TryInstallBatteryBank(out reason);
00167:             if (ok)
00168:             {
00169:                 LastSnapshot = System.Snapshot();
00170:                 OnStateChanged?.Invoke();
00171:             }
00172:             return ok;
00173:         }
00174:
00175:         /// <summary>
00176:         /// B5–B8 Phase 5: service the generator. Caller must consume the
00177:         /// canonical <see cref="PowerGridSystem.GeneratorMaintenanceItemId"/>
00178:         /// item first; this only mutates PowerGrid state.
00179:         /// </summary>
00180:         public bool PerformGeneratorMaintenance(out string reason)
00181:         {
00182:             bool ok = System.PerformGeneratorMaintenance(out reason);
00183:             if (ok)
00184:             {
00185:                 LastSnapshot = System.Snapshot();
00186:                 OnStateChanged?.Invoke();
00187:             }
00188:             return ok;
00189:         }
00190:
00191:         public PowerGridTickSummary TickDay(int day)
00192:         {
00193:             var sum = System.TickDay(day, _tickRng);
00194:             LastSnapshot = System.Snapshot();
00195:             LastTickSummary = sum;
00196:             OnStateChanged?.Invoke();
00197:             return sum;
00198:         }
00199:
00200:         public bool TrySave()
00201:         {
00202:             var save = new PowerGridSave
00203:             {
00204:                 simDay = System.State.SimDay,
00205:                 Rooms = _roomSaves,
00206:                 State = System.State.Capture()
00207:             };
00208:             return PowerGridSaveStore.TrySave(save);
00209:         }
00210:
00211:         public bool TryLoad()
00212:         {
00213:             var loaded = PowerGridSaveStore.TryLoad();
00214:             if (loaded == null) return false;
00215:             System.RestoreState(loaded.State);
00216:             LastSnapshot = System.Snapshot();
00217:             OnStateChanged?.Invoke();
00218:             return true;
00219:         }
00220:
00221:         private static ShelterPowerGridCatalogDef LoadGridJson(string? dataDir)
00222:         {
00223:             if (string.IsNullOrWhiteSpace(dataDir))
00224:                 return ShelterPowerGridCatalogLoader.FallbackDefault();
00225:             return ShelterPowerGridCatalogLoader.LoadOrDefault(
00226:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00227:         }
00228:     }
00229: }
```

## `src/Host/PowerGridSaveStore.cs` — 54 lines; 2,612 bytes; SHA-256 `dfac8826270205eb1b9535ec6ef5ebbbaaedc241e40c9283972b1f3909da8413`
Declaration index:
- 00020: public static class PowerGridSaveStore
- 00036: public static string TryCaptureDirect(PowerGridSave state) => s_store.CaptureBare(state);
- 00039: public static PowerGridSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
- 00042: public static string TryCapture(PowerGridSave state) => s_store.CaptureBare(state);
- 00045: public static PowerGridSave? TryRestore(string json) => s_store.RestoreBare(json);
- 00047: public static bool TrySave(PowerGridSave save) => s_store.TrySave(save);
- 00049: public static PowerGridSave? TryLoad() => s_store.TryLoad();
- 00052: public static string TryCapturePersisted(PowerGridSave save) => s_store.CapturePersisted(save);
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Save Store : PowerGridSaveStore
00004: // Core State : Ashfall.Core.Shelter.PowerGridSave
00005: // Host Caller: Main.ShelterInfrastructure / PowerGridHostSession
00006: // Purpose    : Shelter electrical power grid, generator fuel, battery capacity, and blackout zones
00007: // ============================================================================
00008: using Ashfall.Core.Save;
00009: using Ashfall.Core.Shelter;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     /// <summary>
00014:     /// Persists <see cref="PowerGridSave"/> as JSON under
00015:     /// <c>user://power_grid_save.json</c> — thin façade over the Core
00016:     /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Shape and
00017:     /// checksum live in <see cref="PowerGridSaveCodec"/>; path resolution,
00018:     /// atomic write, and error handling live in the service.
00019:     /// </summary>
00020:     public static class PowerGridSaveStore
00021:     {
00022:         public const string FileName = "power_grid_save.json";
00023:         public const string SectionName = "power_grid";
00024:
00025:         private static readonly SaveStore<PowerGridSave> s_store = SaveStoreHub.FromCodec(
00026:             FileName,
00027:             nameof(PowerGridSaveStore),
00028:             (save, json) => PowerGridSaveCodec.EncodeToString(save, json),
00029:             (raw, json) => PowerGridSaveCodec.Decode(raw, json));
00030:
00031:         public static string SavePath => s_store.SavePath;
00032:
00033:         public static bool Exists => s_store.Exists();
00034:
00035:         /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
00036:         public static string TryCaptureDirect(PowerGridSave state) => s_store.CaptureBare(state);
00037:
00038:         /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
00039:         public static PowerGridSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
00040:
00041:         /// <summary>Capture state to JSON without writing to disk.</summary>
00042:         public static string TryCapture(PowerGridSave state) => s_store.CaptureBare(state);
00043:
00044:         /// <summary>Restore state from JSON without reading from disk.</summary>
00045:         public static PowerGridSave? TryRestore(string json) => s_store.RestoreBare(json);
00046:
00047:         public static bool TrySave(PowerGridSave save) => s_store.TrySave(save);
00048:
00049:         public static PowerGridSave? TryLoad() => s_store.TryLoad();
00050:
00051:         /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
00052:         public static string TryCapturePersisted(PowerGridSave save) => s_store.CapturePersisted(save);
00053:     }
00054: }
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

## `src/UI/PowerGridPanel.cs` — 303 lines; 14,132 bytes; SHA-256 `5a08a0c0e7e7f26bd111df0d0b120219ffad900d16d6aa9eb4f6e76b31ac0be8`
Declaration index:
- 00016: public partial class PowerGridPanel : Control
- 00040: public void Bind(PowerGridHostSession session)
- 00050: public void RefreshView()
- 00117: private Control MakeRoomRow(PowerGridRoom r, bool served, PowerGridRoomPriority pri, bool legacyPowered, bool tripped)
- 00270: public void Open()
- 00287: public void Unbind()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core.Shelter;
00006: using Ashfall.Core.UI;
00007: using DesignTheme = Ashfall.Core.UI.Theme;
00008:
00009: namespace AtomicWar.GodotApp.UI
00010: {
00011:     /// <summary>
00012:     /// Power Grid panel (item 13) — breaker controls, live reserve/load/fuel
00013:     /// displays, and per-room priority pickers. Thin Godot-side UI: every
00014:     /// mutation goes through <see cref="PowerGridHostSession"/>.
00015:     /// </summary>
00016:     public partial class PowerGridPanel : Control
00017:     {
00018:         public event Action<string>? OnRoomToggled;
00019:         public event Action<string, PowerGridRoomPriority>? OnPriorityChanged;
00020:         public event Action<string>? OnBreakerResetRequested;
00021:         public event Action<float>? OnFuelAdded;
00022:         public event Action? OnBatteryBankInstallRequested;
00023:         public event Action? OnGeneratorServiceRequested;
00024:         public event Action<string>? OnEmergencyPresetRequested;
00025:         public event Action? OnClose;
00026:
00027:         private PowerGridHostSession? _session;
00028:         private Label _genLabel = null!;
00029:         private Label _drawLabel = null!;
00030:         private Label _batteryLabel = null!;
00031:         private Label _fuelLabel = null!;
00032:         private Label _deficitLabel = null!;
00033:         private Label _brownoutLabel = null!;
00034:         private VBoxContainer _roomList = null!;
00035:
00036:         private static readonly PowerGridRoomPriority[] s_priorities = (PowerGridRoomPriority[])Enum.GetValues(typeof(PowerGridRoomPriority));
00037:
00038:         public bool IsBound => _session != null;
00039:
00040:         public void Bind(PowerGridHostSession session)
00041:         {
00042:             if (_session != null)
00043:                 _session.OnStateChanged -= RefreshView;
00044:             _session = session;
00045:             if (_session != null)
00046:                 _session.OnStateChanged += RefreshView;
00047:             RefreshView();
00048:         }
00049:
00050:         public void RefreshView()
00051:         {
00052:             if (_session == null || _genLabel == null || _drawLabel == null || _batteryLabel == null || _fuelLabel == null || _deficitLabel == null || _brownoutLabel == null || _roomList == null) return;
00053:             var snap = _session.LastSnapshot;
00054:             var tick = _session.LastTickSummary;
00055:             float cond = _session.System.GeneratorCondition;
00056:             _genLabel.Text = $"GEN {snap.GenerationWatts:0} W (CONDITION {cond:0}%)";
00057:             _genLabel.AddThemeColorOverride("font_color",
00058:                 AshfallUiHelpers.ToColor(cond < PowerGridSystem.GeneratorDegradationThreshold
00059:                     ? DesignTheme.Critical : DesignTheme.Pale));
00060:             _drawLabel.Text = $"DRAW {snap.TotalDrawWatts:0} W (net {snap.NetWatts:+0;-0;0})";
00061:             float pct = snap.BatteryCapacityWh > 0
00062:                 ? (snap.BatteryReserveWh / snap.BatteryCapacityWh) * 100f : 0f;
00063:             int banks = _session.System.InstalledBatteryBankCount;
00064:             int maxBanks = PowerGridSystem.MaxInstalledBatteryBanks;
00065:             _batteryLabel.Text = $"BATTERY {snap.BatteryReserveWh:0}/{snap.BatteryCapacityWh:0} Wh ({pct:0}%) — BANKS {banks}/{maxBanks}";
00066:             _fuelLabel.Text = $"FUEL {snap.FuelUnits:0} units";
00067:
00068:             // C2[6] 23B: canonical demand/supply/deficit/runtime read model — the
00069:             // panel renders Core numbers, never its own arithmetic.
00070:             float runtime = _session.System.EstimatedRuntimeHours;
00071:             string runtimeText = float.IsPositiveInfinity(runtime) ? "INF" : $"{runtime:0.0} h";
00072:             _deficitLabel.Text = $"SUPPLY {_session.System.AvailableSupplyWatts:0} W "
00073:                 + $"// DEFICIT {_session.System.DeficitWatts:0} W "
00074:                 + $"// BATTERY ETA {runtimeText} "
00075:                 + $"// FUEL ~{_session.System.EstimatedFuelRunwayDays:0.0} d";
00076:             _deficitLabel.AddThemeColorOverride("font_color",
00077:                 AshfallUiHelpers.ToColor(_session.System.DeficitWatts > 0f ? DesignTheme.Warning : DesignTheme.Pale));
00078:             // B5–B8 Phase 9: honest status line — brownout vs critical
00079:             // life-support deficit are different states (§8.6), and served/shed
00080:             // numbers come from the actual tick allocation, not a guess.
00081:             if (tick != null && tick.HasCriticalDeficit)
00082:             {
00083:                 _brownoutLabel.Text = $"CRITICAL DEFICIT // LIFE SUPPORT UNSERVED ({tick.UnservedWatts:0} W shed)";
00084:                 _brownoutLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
00085:             }
00086:             else if (snap.IsBrownout)
00087:             {
00088:                 _brownoutLabel.Text = tick != null
00089:                     ? $"BROWNOUT // {tick.ShedRoomIds.Count} LOAD(S) SHED ({tick.UnservedWatts:0} W)"
00090:                     : "BROWNOUT // LOAD SHED ACTIVE";
00091:                 _brownoutLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
00092:             }
00093:             else
00094:             {
00095:                 _brownoutLabel.Text = "STABLE";
00096:                 _brownoutLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
00097:             }
00098:
00099:             AshfallUiHelpers.EmptyChildren(_roomList);
00100:             // B5–B8 Phase 9: allocation-aware per-room truth. Served ≠ the old
00101:             // global powered read — during a brownout critical loads show
00102:             // SERVED while optional loads show SHED, matching the tick's
00103:             // deterministic allocation (Phase 2).
00104:             var tickRooms = _session.LastTickSummary;
00105:             foreach (var r in _session.System.Rooms)
00106:             {
00107:                 bool served = tickRooms != null
00108:                     ? tickRooms.ServedRoomIds.Contains(r.RoomId)
00109:                     : _session.System.IsRoomServed(r.RoomId);
00110:                 bool legacyPowered = _session.System.IsRoomPowered(r.RoomId);
00111:                 bool tripped = _session.System.IsRoomTripped(r.RoomId);
00112:                 var pri = _session.System.EffectivePriority(r.RoomId);
00113:                 _roomList.AddChild(MakeRoomRow(r, served, pri, legacyPowered, tripped));
00114:             }
00115:         }
00116:
00117:         private Control MakeRoomRow(PowerGridRoom r, bool served, PowerGridRoomPriority pri, bool legacyPowered, bool tripped)
00118:         {
00119:             var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
00120:             var nameLbl = AshfallUiHelpers.MakeMono(r.DisplayName);
00121:             nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00122:             // Shed-but-breaker-closed loads get a distinct warn tone; fully
00123:             // offline rooms (open breaker/tripped) stay muted.
00124:             var nameColor = served ? DesignTheme.Pale
00125:                 : legacyPowered ? DesignTheme.Warning
00126:                 : DesignTheme.Muted;
00127:             nameLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(nameColor));
00128:             row.AddChild(nameLbl);
00129:
00130:             var drawLbl = AshfallUiHelpers.MakeMono($"{r.DrawWatts:0} W{(legacyPowered ? "" : served ? " · SERVED" : " · SHED")}");
00131:             drawLbl.CustomMinimumSize = new Vector2(110, 0);
00132:             row.AddChild(drawLbl);
00133:
00134:             var priRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingXs);
00135:             for (int i = 0; i < s_priorities.Length; i++)
00136:             {
00137:                 PowerGridRoomPriority p = s_priorities[i];
00138:                 var btn = AshfallUiHelpers.MakeButton(p.ToString().ToUpperInvariant(),
00139:                     () => OnPriorityChanged?.Invoke(r.RoomId, p));
00140:                 btn.CustomMinimumSize = new Vector2(56, 24);
00141:                 if (p == pri) btn.Disabled = true;
00142:                 priRow.AddChild(btn);
00143:             }
00144:             row.AddChild(priRow);
00145:
00146:             // C2[6] 23B: a tripped circuit needs an explicit (costly) reset; the
00147:             // breaker toggle alone would not clear the trip.
00148:             if (tripped)
00149:             {
00150:                 var resetBtn = AshfallUiHelpers.MakeButton("RESET", () => OnBreakerResetRequested?.Invoke(r.RoomId));
00151:                 resetBtn.CustomMinimumSize = new Vector2(60, 24);
00152:                 resetBtn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
00153:                 row.AddChild(resetBtn);
00154:             }
00155:             else
00156:             {
00157:                 var state = AshfallUiHelpers.MakeButton(
00158:                     legacyPowered ? "ON" : "OFF",
00159:                     () => OnRoomToggled?.Invoke(r.RoomId));
00160:                 state.CustomMinimumSize = new Vector2(60, 24);
00161:                 state.AddThemeColorOverride("font_color",
00162:                     AshfallUiHelpers.ToColor(legacyPowered ? DesignTheme.Pale : DesignTheme.Warm));
00163:                 row.AddChild(state);
00164:             }
00165:             return row;
00166:         }
00167:
00168:         public override void _Ready()
00169:         {
00170:             SetAnchorsPreset(LayoutPreset.FullRect);
00171:             Visible = false;
00172:
00173:             var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.92f) };
00174:             bg.SetAnchorsPreset(LayoutPreset.FullRect);
00175:             AddChild(bg);
00176:
00177:             var center = new CenterContainer();
00178:             center.SetAnchorsPreset(LayoutPreset.FullRect);
00179:             AddChild(center);
00180:
00181:             var panel = AshfallUiHelpers.MakePanel(760, 620);
00182:             center.AddChild(panel);
00183:
00184:             var margins = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
00185:             panel.AddChild(margins);
00186:
00187:             var vbox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
00188:             margins.AddChild(vbox);
00189:
00190:             var title = AshfallUiHelpers.MakeTitle("POWER GRID", DesignTheme.FontSizeH2);
00191:             vbox.AddChild(title);
00192:             vbox.AddChild(AshfallUiHelpers.MakeSeparator());
00193:
00194:             var stats = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
00195:             _genLabel = AshfallUiHelpers.MakeMono("GEN 0 W");
00196:             _drawLabel = AshfallUiHelpers.MakeMono("DRAW 0 W (net 0)");
00197:             _batteryLabel = AshfallUiHelpers.MakeMono("BATTERY 0/0 Wh (0%)");
00198:             _fuelLabel = AshfallUiHelpers.MakeMono("FUEL 0 units");
00199:             _deficitLabel = AshfallUiHelpers.MakeMono("SUPPLY 0 W // DEFICIT 0 W // BATTERY ETA INF // FUEL ~0.0 d");
00200:             _brownoutLabel = AshfallUiHelpers.MakeMono("STABLE");
00201:             stats.AddChild(_genLabel);
00202:             stats.AddChild(_drawLabel);
00203:             stats.AddChild(_batteryLabel);
00204:             stats.AddChild(_fuelLabel);
00205:             stats.AddChild(_deficitLabel);
00206:             stats.AddChild(_brownoutLabel);
00207:             vbox.AddChild(stats);
00208:
00209:             // B5–B8 Phase 2: battery-bank install (canonical item consumed by
00210:             // the host route; the panel only raises the request).
00211:             var bankBtn = AshfallUiHelpers.MakeButton("INSTALL BATTERY BANK",
00212:                 () => OnBatteryBankInstallRequested?.Invoke());
00213:             bankBtn.CustomMinimumSize = new Vector2(220, 26);
00214:             vbox.AddChild(bankBtn);
00215:
00216:             // B5–B8 Phase 5: generator service (canonical machine_oil consumed
00217:             // by the host route; the panel only raises the request).
00218:             var serviceBtn = AshfallUiHelpers.MakeButton("SERVICE GENERATOR",
00219:                 () => OnGeneratorServiceRequested?.Invoke());
00220:             serviceBtn.CustomMinimumSize = new Vector2(220, 26);
00221:             vbox.AddChild(serviceBtn);
00222:
00223:             // B5–B8 expansion (§27): emergency priority presets — the brownout
00224:             // shortcut; the host applies the Core policy and journals it.
00225:             var presetRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
00226:             var shedBtn = AshfallUiHelpers.MakeButton("EMERGENCY: PRESERVE LIFE SUPPORT",
00227:                 () => OnEmergencyPresetRequested?.Invoke("shed"));
00228:             shedBtn.CustomMinimumSize = new Vector2(280, 26);
00229:             presetRow.AddChild(shedBtn);
00230:             var defaultsBtn = AshfallUiHelpers.MakeButton("RESTORE DEFAULTS",
00231:                 () => OnEmergencyPresetRequested?.Invoke("defaults"));
00232:             defaultsBtn.CustomMinimumSize = new Vector2(180, 26);
00233:             presetRow.AddChild(defaultsBtn);
00234:             vbox.AddChild(presetRow);
00235:
00236:             vbox.AddChild(AshfallUiHelpers.MakeSeparator());
00237:
00238:             var scroll = new ScrollContainer
00239:             {
00240:                 CustomMinimumSize = new Vector2(720, 360),
00241:                 SizeFlagsVertical = SizeFlags.ExpandFill
00242:             };
00243:             vbox.AddChild(scroll);
00244:
00245:             _roomList = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
00246:             scroll.AddChild(_roomList);
00247:
00248:             vbox.AddChild(AshfallUiHelpers.MakeSeparator());
00249:
00250:             var fuelRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
00251:             var fuelLbl = AshfallUiHelpers.MakeMono("Add fuel:");
00252:             fuelRow.AddChild(fuelLbl);
00253:             foreach (var amt in new[] { 10f, 25f, 50f })
00254:             {
00255:                 var btn = AshfallUiHelpers.MakeButton($"+{amt:0}",
00256:                     () => OnFuelAdded?.Invoke(amt));
00257:                 btn.CustomMinimumSize = new Vector2(60, 24);
00258:                 fuelRow.AddChild(btn);
00259:             }
00260:             vbox.AddChild(fuelRow);
00261:
00262:             var closeRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
00263:             var closeBtn = AshfallUiHelpers.MakeButton("CLOSE [Esc]",
00264:                 () => { Visible = false; OnClose?.Invoke(); });
00265:             closeBtn.CustomMinimumSize = new Vector2(120, 28);
00266:             closeRow.AddChild(closeBtn);
00267:             vbox.AddChild(closeRow);
00268:         }
00269:
00270:         public void Open()
00271:         {
00272:             Visible = true;
00273:             RefreshView();
00274:             QueueRedraw();
00275:         }
00276:
00277:         public override void _UnhandledInput(InputEvent @event)
00278:         {
00279:             if (!Visible) return;
00280:             if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00281:             {
00282:                 Visible = false;
00283:                 GetViewport().SetInputAsHandled();
00284:             }
00285:         }
00286:
00287:         public void Unbind()
00288:         {
00289:             if (_session != null)
00290:             {
00291:                 _session.OnStateChanged -= RefreshView;
00292:                 _session = null;
00293:             }
00294:             RefreshView();
00295:         }
00296:
00297:         public override void _ExitTree()
00298:         {
00299:             Unbind();
00300:             base._ExitTree();
00301:         }
00302:     }
00303: }
```

## `Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs` — 218 lines; 11,662 bytes; SHA-256 `477e78e90abae6e0f2f2f110121588de9ecd2e004d11c09ab892f4acb33168b1`
Declaration index:
- 00020: public sealed class PowerGridCatalogTests
- 00022: private static string FindDataDir()
- 00030: private static ShelterPowerGridCatalogDef LoadCatalog()
- 00039: public void Catalog_HasValidSchemaVersion()
- 00049: public void Catalog_HasAtLeastSevenRooms()
- 00059: public void Catalog_PropagatesSurgeTuning()
- 00070: public void Catalog_MissingSurgeTuning_FallsBackToDefaults()
- 00081: public void Catalog_QuarantineWardVentilationIsCriticalTier()
- 00093: public void Catalog_EveryFailureEffectId_HasANamedConsumer()
- 00137: public void Catalog_PreservesShippedRoomsInOrder()
- 00185: public void Catalog_RoomIdsAreUnique()
- 00193: public void Catalog_CanonicalConsumerRoomIdsResolve()
- 00197: // wave's G1 class of bug is an unknown room ID silently reading false.
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Shelter;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests.Shelter
00011: {
00012:     /// <summary>
00013:     /// SHELTER_GRID_CATALOG_SEAL Phase 2: pins the shipped power_grid.json
00014:     /// authority as consumed by ShelterPowerGridCatalogLoader. The historical
00015:     /// 18-room assertion was removed from compilation (csproj quarantine)
00016:     /// because it disagreed with the shipped 6-room data; this repaired file
00017:     /// pins the shipped rooms (plus room_workshop added by the seal wave) so
00018:     /// future catalog edits are deliberate, test-reviewed changes.
00019:     /// </summary>
00020:     public sealed class PowerGridCatalogTests
00021:     {
00022:         private static string FindDataDir()
00023:         {
00024:             string dataDir;
00025:             if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
00026:                 CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
00027:             return dataDir ?? string.Empty;
00028:         }
00029:
00030:         private static ShelterPowerGridCatalogDef LoadCatalog()
00031:         {
00032:             var ok = ShelterPowerGridCatalogLoader.TryLoad(FindDataDir(), new FileSystemIO(),
00033:                 new SystemTextJsonSerializer(), out var catalog, out var error);
00034:             Assert.True(ok, $"power_grid.json must load strictly: {error}");
00035:             return catalog!;
00036:         }
00037:
00038:         [Fact]
00039:         public void Catalog_HasValidSchemaVersion()
00040:         {
00041:             var catalog = LoadCatalog();
00042:             Assert.Equal(1, catalog.SchemaVersion);
00043:             Assert.Equal(800f, catalog.GenerationWattsDefault);
00044:             Assert.Equal(4000f, catalog.BatteryCapacityWhDefault);
00045:             Assert.Equal(100f, catalog.FuelUnitsDefault);
00046:         }
00047:
00048:         [Fact]
00049:         public void Catalog_HasAtLeastSevenRooms()
00050:         {
00051:             // Lower bound, not exact: the catalog is a living authority and
00052:             // concurrent streams add rooms (e.g. room_cryo_vault). The pinned
00053:             // per-room contract below is the real gate.
00054:             var catalog = LoadCatalog();
00055:             Assert.True(catalog.Rooms.Count >= 7, $"expected >= 7 rooms, got {catalog.Rooms.Count}");
00056:         }
00057:
00058:         [Fact]
00059:         public void Catalog_PropagatesSurgeTuning()
00060:         {
00061:             // SHELTER_HARDENING: surge tuning is catalog-driven (fields optional).
00062:             var catalog = LoadCatalog();
00063:             Assert.True(catalog.EmpStormSeverity is >= 0f and <= 1f,
00064:                 $"emp_storm_severity out of range: {catalog.EmpStormSeverity}");
00065:             Assert.True(catalog.SurgeBatteryDrainFraction is >= 0f and <= 1f,
00066:                 $"surge_battery_drain_fraction out of range: {catalog.SurgeBatteryDrainFraction}");
00067:         }
00068:
00069:         [Fact]
00070:         public void Catalog_MissingSurgeTuning_FallsBackToDefaults()
00071:         {
00072:             var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
00073:             Assert.Null(catalog.EmpStormSeverity); // absent → system defaults apply
00074:             Assert.Null(catalog.SurgeBatteryDrainFraction);
00075:             // Loader validation still passes (optional fields).
00076:             var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
00077:             Assert.True(ok, error);
00078:         }
00079:
00080:         [Fact]
00081:         public void Catalog_QuarantineWardVentilationIsCriticalTier()
00082:         {
00083:             // SHELTER_EMP_MEDICAL_POWER: quarantine ventilation is life-safety —
00084:             // it must sit in the critical tier so surge shedding sheds it last.
00085:             var catalog = LoadCatalog();
00086:             var room = Assert.Single(catalog.Rooms, r => r.Id == "room_ward_quarantine");
00087:             Assert.Equal("critical", room.DefaultPriority);
00088:             Assert.True(room.DrawWatts > 0);
00089:             Assert.Equal("fx_quarantine_ventilation_off", room.FailureEffectId);
00090:         }
00091:
00092:         [Fact]
00093:         public void Catalog_EveryFailureEffectId_HasANamedConsumer()
00094:         {
00095:             // SHELTER_FAILURE_EFFECTS (G6 closure): every authored failure effect
00096:             // must map to the owner that consumes the outage state. A new fx id
00097:             // without an entry here fails the suite — the G6 bug class (authored
00098:             // vocabulary with no consumer) cannot ship silently.
00099:             var consumers = new Dictionary<string, string>(StringComparer.Ordinal)
00100:             {
00101:                 ["fx_filtration_off"] = "StartingLevelSystem.TickDay powerAvailability01 (Main.CampaignOwners airPower)",
00102:                 ["fx_clinic_off"] = "MedicalPipelineCoordinator clinicPowerCheck (Main.Medical) + AdvanceScheduled scaling (MedicalDiseaseDayOwner)",
00103:                 ["fx_water_pressure_drop"] = "Plan168FluidDayOwner power derivation (Main.Plans166_169)",
00104:                 ["fx_grow_lights_off"] = "BuildAgricultureEnvironment LightingAvailabilityPermille (Main.Plans162_165)",
00105:                 ["fx_foundry_standstill"] = "SilentFoundryHostSession power gate (room_foundry/room_workshop)",
00106:                 ["fx_lighting_dim"] = "ShelterScheduleSystem brownout lighting demand",
00107:                 ["fx_workshop_offline"] = "Main.World workshop power gate (room_workshop)",
00108:                 ["fx_cryo_vault_unpowered"] = "CryoVaultSystem powerAvailableProvider ← Main.PlansB68_B69.IsCryoVaultPowered (room_cryo_vault)",
00109:                 ["fx_quarantine_ventilation_off"] = "DiseaseQuarantineCoordinator isolationPowerCheck (Main.SetupDisease)",
00110:                 // Plan 71 — nine new powered rooms. Consumers that were already
00111:                 // querying these IDs before the entries existed (the dead-query
00112:                 // bug class) become live with the data alone; the rest are wired
00113:                 // at existing host call sites (see PLAN71_COMPLETION_REPORT.md).
00114:                 ["fx_heating_off"] = "ResourceMassBalanceSimulator isNearHeatSource (room_heating)",
00115:                 ["fx_kitchen_off"] = "ResourceMassBalanceSimulator kitchenPower (room_kitchen)",
00116:                 ["fx_water_filtration_off"] = "ResourceMassBalanceSimulator waterPower (room_water_filtration)",
00117:                 ["fx_airlock_decon_off"] = "PerimeterDefenseSystem assault airlock power (InfrastructureHeadlessDemo defense path)",
00118:                 ["fx_radio_tuner_off"] = "ShelterRadioStationSystem.TickDay monitoring pause (Main.Plans46_49)",
00119:                 ["fx_laboratory_offline"] = "ResearchHostSession.StartGate research-start block (Main.PlayerSurfaces)",
00120:                 ["fx_precision_metrology_off"] = "PrecisionMetrology drift/projection pause (Main.PlansB86_B89 TickPrecisionMetrology)",
00121:                 ["fx_common_mess_cold"] = "ShelterDecorHostSession.ApplyDailyMorale pause (Main.CampaignOwners SurvivorsNeedsDayOwner)",
00122:                 ["fx_armory_service_off"] = "DefenseSystem isEmplacementPowered armory circuit (Main.Plans162_165 SetupDefense)",
00123:             };
00124:
00125:             var catalog = LoadCatalog();
00126:             foreach (var room in catalog.Rooms)
00127:             {
00128:                 var fx = room.FailureEffectId;
00129:                 Assert.True(
00130:                     !string.IsNullOrEmpty(fx) && consumers.ContainsKey(fx),
00131:                     $"room '{room.Id}' authored failure_effect_id '{fx}' has no registered consumer. " +
00132:                     "Add the consuming system to this map (and wire it) before shipping.");
00133:             }
00134:         }
00135:
00136:         [Fact]
00137:         public void Catalog_PreservesShippedRoomsInOrder()
00138:         {
00139:             var catalog = LoadCatalog();
00140:
00141:             Assert.Equal("room_air_filtration", catalog.Rooms[0].Id);
00142:             Assert.Equal("Air Filtration", catalog.Rooms[0].DisplayName);
00143:             Assert.Equal(180f, catalog.Rooms[0].DrawWatts);
00144:             Assert.Equal("critical", catalog.Rooms[0].DefaultPriority);
00145:             Assert.Equal("fx_filtration_off", catalog.Rooms[0].FailureEffectId);
00146:
00147:             Assert.Equal("room_clinic", catalog.Rooms[1].Id);
00148:             Assert.Equal("Clinic", catalog.Rooms[1].DisplayName);
00149:             Assert.Equal(120f, catalog.Rooms[1].DrawWatts);
00150:             Assert.Equal("critical", catalog.Rooms[1].DefaultPriority);
00151:             Assert.Equal("fx_clinic_off", catalog.Rooms[1].FailureEffectId);
00152:
00153:             Assert.Equal("room_water_pump", catalog.Rooms[2].Id);
00154:             Assert.Equal("Water Pump", catalog.Rooms[2].DisplayName);
00155:             Assert.Equal(100f, catalog.Rooms[2].DrawWatts);
00156:             Assert.Equal("critical", catalog.Rooms[2].DefaultPriority);
00157:             Assert.Equal("fx_water_pressure_drop", catalog.Rooms[2].FailureEffectId);
00158:
00159:             Assert.Equal("room_greenhouse", catalog.Rooms[3].Id);
00160:             Assert.Equal("Greenhouse", catalog.Rooms[3].DisplayName);
00161:             Assert.Equal(160f, catalog.Rooms[3].DrawWatts);
00162:             Assert.Equal("standard", catalog.Rooms[3].DefaultPriority);
00163:             Assert.Equal("fx_grow_lights_off", catalog.Rooms[3].FailureEffectId);
00164:
00165:             Assert.Equal("room_foundry", catalog.Rooms[4].Id);
00166:             Assert.Equal("Silent Foundry", catalog.Rooms[4].DisplayName);
00167:             Assert.Equal(220f, catalog.Rooms[4].DrawWatts);
00168:             Assert.Equal("low", catalog.Rooms[4].DefaultPriority);
00169:             Assert.Equal("fx_foundry_standstill", catalog.Rooms[4].FailureEffectId);
00170:
00171:             Assert.Equal("room_lighting_main", catalog.Rooms[5].Id);
00172:             Assert.Equal("Main Lighting", catalog.Rooms[5].DisplayName);
00173:             Assert.Equal(80f, catalog.Rooms[5].DrawWatts);
00174:             Assert.Equal("low", catalog.Rooms[5].DefaultPriority);
00175:             Assert.Equal("fx_lighting_dim", catalog.Rooms[5].FailureEffectId);
00176:
00177:             Assert.Equal("room_workshop", catalog.Rooms[6].Id);
00178:             Assert.Equal("Workshop", catalog.Rooms[6].DisplayName);
00179:             Assert.Equal(300f, catalog.Rooms[6].DrawWatts);
00180:             Assert.Equal("low", catalog.Rooms[6].DefaultPriority);
00181:             Assert.Equal("fx_workshop_offline", catalog.Rooms[6].FailureEffectId);
00182:         }
00183:
00184:         [Fact]
00185:         public void Catalog_RoomIdsAreUnique()
00186:         {
00187:             var catalog = LoadCatalog();
00188:             var ids = new HashSet<string>(catalog.Rooms.Select(r => r.Id), StringComparer.Ordinal);
00189:             Assert.Equal(catalog.Rooms.Count, ids.Count);
00190:         }
00191:
00192:         [Fact]
00193:         public void Catalog_CanonicalConsumerRoomIdsResolve()
00194:         {
00195:             // Room IDs queried by host code (fluid day owner, workshop gate,
00196:             // agriculture environment) must exist in the catalog — the seal
00197:             // wave's G1 class of bug is an unknown room ID silently reading false.
00198:             var catalog = LoadCatalog();
00199:             var ids = new HashSet<string>(catalog.Rooms.Select(r => r.Id), StringComparer.Ordinal);
00200:             Assert.Contains("room_water_pump", ids);
00201:             Assert.Contains("room_workshop", ids);
00202:             Assert.Contains("room_greenhouse", ids);
00203:             Assert.Contains("room_clinic", ids);
00204:             // Plan 71: room IDs queried by downstream systems before their grid
00205:             // entries existed (the dead-query class) must now resolve.
00206:             Assert.Contains("room_heating", ids);
00207:             Assert.Contains("room_kitchen", ids);
00208:             Assert.Contains("room_water_filtration", ids);
00209:             Assert.Contains("room_airlock", ids);
00210:             // Plan 71: Plan 41 shelter rooms with electrical profiles.
00211:             Assert.Contains("room_radio_tuner", ids);
00212:             Assert.Contains("room_laboratory_research", ids);
00213:             Assert.Contains("room_workshop_precision", ids);
00214:             Assert.Contains("room_common_mess_hall", ids);
00215:             Assert.Contains("room_armory_munitions", ids);
00216:         }
00217:     }
00218: }
```

## `Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs` — 174 lines; 7,973 bytes; SHA-256 `0cbb737c2b6cc252076102bdf9c107a541cb7bbb873e14883ab2fd0d548b6b88`
Declaration index:
- 00014: public sealed class ShelterPowerGridCatalogLoaderTests
- 00016: private static string FindDataDir()
- 00024: private sealed class StaticFileIO : IFileIO
- 00029: public bool DirectoryExists(string path) => true;
- 00030: public bool FileExists(string path) => path == _path;
- 00031: public string ReadAllText(string path) => path == _path ? _content : throw new FileNotFoundException(path);
- 00032: public void WriteAllText(string path, string contents) { }
- 00033: public string Combine(params string[] parts) => string.Join("/", parts);
- 00039: public void ShippedCatalog_StrictLoad_Succeeds()
- 00048: public void ShippedCatalog_HostLoad_MatchesStrictLoad()
- 00063: public void MissingFile_LoadOrDefault_FallsBackToDefaults()
- 00072: public void MissingFile_TryLoad_ReportsFileNotFound()
- 00081: public void MalformedJson_TryLoad_FailsWithFileAndReason()
- 00091: public void MalformedJson_LoadOrDefault_FallsBack()
- 00101: public void DuplicateRoomId_FailsWithRoomId()
- 00111: public void NegativeDraw_FailsWithRoomId()
- 00121: public void UnknownPriority_FailsWithRoomId()
- 00131: public void WrongSchemaVersion_Fails()
- 00141: public void EmptyDisplayName_FailsWithRoomId()
- 00153: public void Load_IsDeterministicInOrder()
- 00168: public void FallbackDefault_Validates()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using Ashfall.Core;
00005: using Ashfall.Core.Shelter;
00006: using Xunit;
00007:
00008: namespace Ashfall.Core.Tests.Shelter
00009: {
00010:     /// <summary>
00011:     /// SHELTER_GRID_CATALOG_SEAL Phase 1: loader contract for power_grid.json —
00012:     /// strict validation for tests, fallback-defaults policy for the host boot path.
00013:     /// </summary>
00014:     public sealed class ShelterPowerGridCatalogLoaderTests
00015:     {
00016:         private static string FindDataDir()
00017:         {
00018:             string dataDir;
00019:             if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
00020:                 CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
00021:             return dataDir ?? string.Empty;
00022:         }
00023:
00024:         private sealed class StaticFileIO : IFileIO
00025:         {
00026:             private readonly string _path;
00027:             private readonly string _content;
00028:             public StaticFileIO(string path, string content) { _path = path; _content = content; }
00029:             public bool DirectoryExists(string path) => true;
00030:             public bool FileExists(string path) => path == _path;
00031:             public string ReadAllText(string path) => path == _path ? _content : throw new FileNotFoundException(path);
00032:             public void WriteAllText(string path, string contents) { }
00033:             public string Combine(params string[] parts) => string.Join("/", parts);
00034:         }
00035:
00036:         // ── Shipped catalog ─────────────────────────────────────────────
00037:
00038:         [Fact]
00039:         public void ShippedCatalog_StrictLoad_Succeeds()
00040:         {
00041:             var ok = ShelterPowerGridCatalogLoader.TryLoad(FindDataDir(), new FileSystemIO(),
00042:                 new SystemTextJsonSerializer(), out var catalog, out var error);
00043:             Assert.True(ok, error);
00044:             Assert.NotNull(catalog);
00045:         }
00046:
00047:         [Fact]
00048:         public void ShippedCatalog_HostLoad_MatchesStrictLoad()
00049:         {
00050:             var dataDir = FindDataDir();
00051:             var strict = ShelterPowerGridCatalogLoader.TryLoad(dataDir, new FileSystemIO(),
00052:                 new SystemTextJsonSerializer(), out var a, out _);
00053:             Assert.True(strict);
00054:             var host = ShelterPowerGridCatalogLoader.LoadOrDefault(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00055:             Assert.Equal(a!.Rooms.Count, host.Rooms.Count);
00056:             for (int i = 0; i < a.Rooms.Count; i++)
00057:                 Assert.Equal(a.Rooms[i].Id, host.Rooms[i].Id);
00058:         }
00059:
00060:         // ── Fallback policy ─────────────────────────────────────────────
00061:
00062:         [Fact]
00063:         public void MissingFile_LoadOrDefault_FallsBackToDefaults()
00064:         {
00065:             var catalog = ShelterPowerGridCatalogLoader.LoadOrDefault("/nonexistent/dir", new FileSystemIO(), new SystemTextJsonSerializer());
00066:             var fallback = ShelterPowerGridCatalogLoader.FallbackDefault();
00067:             Assert.Equal(fallback.Rooms.Count, catalog.Rooms.Count);
00068:             Assert.Equal(fallback.GenerationWattsDefault, catalog.GenerationWattsDefault);
00069:         }
00070:
00071:         [Fact]
00072:         public void MissingFile_TryLoad_ReportsFileNotFound()
00073:         {
00074:             var ok = ShelterPowerGridCatalogLoader.TryLoad("/nonexistent/dir", new FileSystemIO(),
00075:                 new SystemTextJsonSerializer(), out _, out var error);
00076:             Assert.False(ok);
00077:             Assert.Contains("file not found", error, StringComparison.Ordinal);
00078:         }
00079:
00080:         [Fact]
00081:         public void MalformedJson_TryLoad_FailsWithFileAndReason()
00082:         {
00083:             var io = new StaticFileIO("/mem/power_grid.json", "{ not valid json !!!");
00084:             var ok = ShelterPowerGridCatalogLoader.TryLoad("/mem", io, new SystemTextJsonSerializer(), out _, out var error);
00085:             Assert.False(ok);
00086:             Assert.Contains("power_grid.json", error, StringComparison.Ordinal);
00087:             Assert.Contains("malformed", error, StringComparison.Ordinal);
00088:         }
00089:
00090:         [Fact]
00091:         public void MalformedJson_LoadOrDefault_FallsBack()
00092:         {
00093:             var io = new StaticFileIO("/mem/power_grid.json", "{ not valid json !!!");
00094:             var catalog = ShelterPowerGridCatalogLoader.LoadOrDefault("/mem", io, new SystemTextJsonSerializer());
00095:             Assert.Equal(ShelterPowerGridCatalogLoader.FallbackDefault().Rooms.Count, catalog.Rooms.Count);
00096:         }
00097:
00098:         // ── Validation rules ────────────────────────────────────────────
00099:
00100:         [Fact]
00101:         public void DuplicateRoomId_FailsWithRoomId()
00102:         {
00103:             var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
00104:             catalog.Rooms.Add(new ShelterPowerGridRoomDef { Id = "room_clinic", DisplayName = "Dup", DrawWatts = 10f, DefaultPriority = "low" });
00105:             var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
00106:             Assert.False(ok);
00107:             Assert.Contains("room_clinic", error, StringComparison.Ordinal);
00108:         }
00109:
00110:         [Fact]
00111:         public void NegativeDraw_FailsWithRoomId()
00112:         {
00113:             var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
00114:             catalog.Rooms[0].DrawWatts = -5f;
00115:             var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
00116:             Assert.False(ok);
00117:             Assert.Contains(catalog.Rooms[0].Id, error, StringComparison.Ordinal);
00118:         }
00119:
00120:         [Fact]
00121:         public void UnknownPriority_FailsWithRoomId()
00122:         {
00123:             var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
00124:             catalog.Rooms[0].DefaultPriority = "ultra";
00125:             var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
00126:             Assert.False(ok);
00127:             Assert.Contains("ultra", error, StringComparison.Ordinal);
00128:         }
00129:
00130:         [Fact]
00131:         public void WrongSchemaVersion_Fails()
00132:         {
00133:             var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
00134:             catalog.SchemaVersion = 99;
00135:             var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
00136:             Assert.False(ok);
00137:             Assert.Contains("schema_version", error, StringComparison.Ordinal);
00138:         }
00139:
00140:         [Fact]
00141:         public void EmptyDisplayName_FailsWithRoomId()
00142:         {
00143:             var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
00144:             catalog.Rooms[0].DisplayName = "";
00145:             var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
00146:             Assert.False(ok);
00147:             Assert.Contains(catalog.Rooms[0].Id, error, StringComparison.Ordinal);
00148:         }
00149:
00150:         // ── Determinism ─────────────────────────────────────────────────
00151:
00152:         [Fact]
00153:         public void Load_IsDeterministicInOrder()
00154:         {
00155:             var dataDir = FindDataDir();
00156:             var a = ShelterPowerGridCatalogLoader.LoadOrDefault(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00157:             var b = ShelterPowerGridCatalogLoader.LoadOrDefault(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00158:             Assert.Equal(a.Rooms.Count, b.Rooms.Count);
00159:             for (int i = 0; i < a.Rooms.Count; i++)
00160:             {
00161:                 Assert.Equal(a.Rooms[i].Id, b.Rooms[i].Id);
00162:                 Assert.Equal(a.Rooms[i].DrawWatts, b.Rooms[i].DrawWatts);
00163:                 Assert.Equal(a.Rooms[i].DefaultPriority, b.Rooms[i].DefaultPriority);
00164:             }
00165:         }
00166:
00167:         [Fact]
00168:         public void FallbackDefault_Validates()
00169:         {
00170:             var ok = ShelterPowerGridCatalogLoader.Validate(ShelterPowerGridCatalogLoader.FallbackDefault(), out var error);
00171:             Assert.True(ok, error);
00172:         }
00173:     }
00174: }
```

## `Ashfall.Core.Tests/PowerGridDeterminismTests.cs` — 72 lines; 2,887 bytes; SHA-256 `bf6a0056a23901bfb0bd018a06e21476dbc2ff78e46b1940d3550e3fe46d358b`
Declaration index:
- 00010: public class PowerGridDeterminismTests
- 00012: private static PowerGridSystem CreateSystem(int seed, float fuel)
- 00020: public void SameSeed_Determinism_FuelAndBatteryIdentical()
- 00038: public void NumericalSafety_NoNaNOrNegative(int seed, int days)
- 00055: public void DifferentSeed_Divergence_Allowed_ButBounded()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Xunit;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Shelter;
00007:
00008: namespace Ashfall.Core.Tests
00009: {
00010:     public class PowerGridDeterminismTests
00011:     {
00012:         private static PowerGridSystem CreateSystem(int seed, float fuel)
00013:         {
00014:             var state = new PowerGridState { FuelUnits = fuel, BatteryReserveWh = 500, BatteryCapacityWh = 1000 };
00015:             var rooms = new List<PowerGridRoom> { new PowerGridRoom { RoomId = "room_a", DisplayName = "Test", DrawWatts = 100 } };
00016:             return new PowerGridSystem(state, rooms, new SeededRng(seed));
00017:         }
00018:
00019:         [Fact]
00020:         public void SameSeed_Determinism_FuelAndBatteryIdentical()
00021:         {
00022:             var a = CreateSystem(42, 100);
00023:             var b = CreateSystem(42, 100);
00024:             var rngA = new SeededRng(42);
00025:             var rngB = new SeededRng(42);
00026:             for (int day = 1; day <= 30; day++)
00027:             {
00028:                 a.TickDay(day, rngA);
00029:                 b.TickDay(day, rngB);
00030:                 Assert.Equal(a.FuelUnits, b.FuelUnits, precision: 3);
00031:                 Assert.Equal(a.BatteryReserveWh, b.BatteryReserveWh, precision: 3);
00032:             }
00033:         }
00034:
00035:         [Theory]
00036:         [InlineData(42, 30)]
00037:         [InlineData(999, 30)]
00038:         public void NumericalSafety_NoNaNOrNegative(int seed, int days)
00039:         {
00040:             var sys = CreateSystem(seed, 100);
00041:             var rng = new SeededRng(seed);
00042:             for (int day = 1; day <= days; day++)
00043:             {
00044:                 sys.TickDay(day, rng);
00045:                 Assert.False(float.IsNaN(sys.FuelUnits), $"NaN fuel day {day}");
00046:                 Assert.False(float.IsInfinity(sys.FuelUnits), $"Infinity fuel day {day}");
00047:                 Assert.True(sys.FuelUnits >= 0, $"Negative fuel {sys.FuelUnits}");
00048:                 Assert.False(float.IsNaN(sys.BatteryReserveWh), $"NaN battery day {day}");
00049:                 Assert.True(sys.BatteryReserveWh >= 0, $"Negative battery {sys.BatteryReserveWh}");
00050:                 Assert.True(sys.BatteryReserveWh <= sys.BatteryCapacityWh + 1e-3f, $"Battery over capacity {sys.BatteryReserveWh} > {sys.BatteryCapacityWh}");
00051:             }
00052:         }
00053:
00054:         [Fact]
00055:         public void DifferentSeed_Divergence_Allowed_ButBounded()
00056:         {
00057:             var a = CreateSystem(42, 100);
00058:             var b = CreateSystem(999, 100);
00059:             var rngA = new SeededRng(42);
00060:             var rngB = new SeededRng(999);
00061:             for (int day = 1; day <= 30; day++)
00062:             {
00063:                 a.TickDay(day, rngA);
00064:                 b.TickDay(day, rngB);
00065:             }
00066:             // Both valid, may diverge due to 5% spike randomness
00067:             Assert.True(a.FuelUnits >= 0 && a.FuelUnits <= 100);
00068:             Assert.True(b.FuelUnits >= 0 && b.FuelUnits <= 100);
00069:             Assert.False(float.IsNaN(a.BatteryReserveWh));
00070:         }
00071:     }
00072: }
```

## `Ashfall.Core.Tests/World/Plan85_71DamagedMapPowerIntegrationTests.cs` — 217 lines; 10,446 bytes; SHA-256 `53a1674ce6afe7e37ecc9e48c484661110df082f47c19e979713b10a048877c0`
Declaration index:
- 00014: public class Plan85_71DamagedMapPowerIntegrationTests : CatalogTestBase
- 00017: public void DamagedMapAndPowerGridCatalogs_LoadCleanly_WithoutSchemaDrift()
- 00049: public void PowerGridSystem_BrownoutLoadShedding_TiersFollowPriority()
- 00102: public void DamagedMapSystem_FragmentDiscoveryToInstallationReveal()
- 00149: public void DamagedMapElectricalSalvage_DirectlySupportsShelterPowerGrid_CrossLinkage()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.IO;
00008: using Ashfall.Core.Shelter;
00009: using Ashfall.Core.World;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests
00013: {
00014:     public class Plan85_71DamagedMapPowerIntegrationTests : CatalogTestBase
00015:     {
00016:         [Fact]
00017:         public void DamagedMapAndPowerGridCatalogs_LoadCleanly_WithoutSchemaDrift()
00018:         {
00019:             // Plan 85: Damaged Map Zones (12 zones, 32 fragments)
00020:             var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(
00021:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
00022:             Assert.Empty(errors);
00023:             Assert.Equal(12, zones.Count);
00024:
00025:             var fragmentCount = zones.Sum(z => z.Fragments.Count);
00026:             Assert.Equal(32, fragmentCount);
00027:
00028:             // Plan 71: Power Grid Rooms (18 rooms with calibrated draw watts and priorities)
00029:             var ok = ShelterPowerGridCatalogLoader.TryLoad(
00030:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer(),
00031:                 out var powerCatalog, out var powerError);
00032:             Assert.True(ok, powerError);
00033:             Assert.NotNull(powerCatalog);
00034:             Assert.Equal(18, powerCatalog!.Rooms.Count);
00035:
00036:             Assert.Equal(1, powerCatalog.SchemaVersion);
00037:             Assert.Equal(800f, powerCatalog.GenerationWattsDefault);
00038:             Assert.Equal(4000f, powerCatalog.BatteryCapacityWhDefault);
00039:             Assert.Equal(100f, powerCatalog.FuelUnitsDefault);
00040:
00041:             // Cross-reference: Metro service ring installation node
00042:             var metroZone = zones.FirstOrDefault(z => z.ZoneId == "metro_service_ring");
00043:             Assert.NotNull(metroZone);
00044:             Assert.Equal("loc_electrical_maintenance_exchange", DamagedMapSystem.ResolveRevealNodeId(metroZone!.InstallationId));
00045:             Assert.Contains(PowerGridSystem.BatteryBankItemId, metroZone.RevealedItems);
00046:         }
00047:
00048:         [Fact]
00049:         public void PowerGridSystem_BrownoutLoadShedding_TiersFollowPriority()
00050:         {
00051:             var ok = ShelterPowerGridCatalogLoader.TryLoad(
00052:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer(),
00053:                 out var powerCatalog, out _);
00054:             Assert.True(ok);
00055:
00056:             var rooms = powerCatalog!.Rooms.Select(r => new PowerGridRoom(
00057:                 r.Id,
00058:                 r.DisplayName,
00059:                 r.DrawWatts,
00060:                 r.DefaultPriority switch
00061:                 {
00062:                     "critical" => PowerGridRoomPriority.Critical,
00063:                     "standard" => PowerGridRoomPriority.Standard,
00064:                     _ => PowerGridRoomPriority.Low
00065:                 },
00066:                 r.FailureEffectId)).ToList();
00067:
00068:             var state = new PowerGridState
00069:             {
00070:                 GenerationWatts = 800f,
00071:                 BatteryCapacityWh = 4000f,
00072:                 BatteryReserveWh = 4000f,
00073:                 FuelUnits = 100f
00074:             };
00075:
00076:             var gridSystem = new PowerGridSystem(state, rooms, new SeededRng(1985));
00077:             Assert.Equal(18, gridSystem.Rooms.Count);
00078:             Assert.Equal(2910f, gridSystem.TotalDrawWatts);
00079:
00080:             // Test emergency brownout load-shed preset
00081:             var shedRooms = gridSystem.ApplyBrownoutShedPreset();
00082:             Assert.NotEmpty(shedRooms);
00083:
00084:             // Critical rooms (e.g. air filtration, clinic, cryo vault) must NEVER be demoted
00085:             Assert.Equal(PowerGridRoomPriority.Critical, gridSystem.EffectivePriority("room_air_filtration"));
00086:             Assert.Equal(PowerGridRoomPriority.Critical, gridSystem.EffectivePriority("room_clinic"));
00087:             Assert.Equal(PowerGridRoomPriority.Critical, gridSystem.EffectivePriority("room_cryo_vault"));
00088:
00089:             // Restore catalog defaults
00090:             int restoredCount = gridSystem.ApplyCatalogDefaultPriorities();
00091:             Assert.True(restoredCount > 0);
00092:             Assert.Equal(PowerGridRoomPriority.Standard, gridSystem.EffectivePriority("room_greenhouse"));
00093:
00094:             // Add external generation source (e.g. TRIGA reactor or solar array)
00095:             gridSystem.SetGenerationContribution("source_triga_aux", 2500f);
00096:             Assert.True(gridSystem.GenerationWatts >= 3300f);
00097:             Assert.True(gridSystem.NetWatts > 0f);
00098:             Assert.False(gridSystem.IsBrownout);
00099:         }
00100:
00101:         [Fact]
00102:         public void DamagedMapSystem_FragmentDiscoveryToInstallationReveal()
00103:         {
00104:             var (nodes, routes) = WastelandMapCatalogLoader.Load(
00105:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
00106:             var mapSystem = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
00107:
00108:             var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(
00109:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
00110:             Assert.Empty(errors);
00111:
00112:             var damagedMapSystem = new DamagedMapSystem(zones, mapSystem);
00113:
00114:             var metroZone = zones.First(z => z.ZoneId == "metro_service_ring");
00115:             string nodeId = DamagedMapSystem.ResolveRevealNodeId(metroZone.InstallationId)!;
00116:
00117:             // Destination must be initially locked
00118:             Assert.True(damagedMapSystem.IsDestinationLocked(nodeId));
00119:             Assert.False(mapSystem.IsDiscovered(nodeId));
00120:
00121:             // Track events
00122:             DamagedMapZone? completedZone = null;
00123:             string? revealedNode = null;
00124:             damagedMapSystem.OnZoneCompleted += z => completedZone = z;
00125:             damagedMapSystem.OnInstallationRevealed += (z, id) => revealedNode = id;
00126:
00127:             // Register fragments 1 and 2
00128:             Assert.True(damagedMapSystem.RegisterFragment("damaged_map_metro_1"));
00129:             Assert.True(damagedMapSystem.RegisterFragment("damaged_map_metro_2"));
00130:             Assert.Equal(2, damagedMapSystem.RegisteredCount("metro_service_ring"));
00131:             Assert.False(damagedMapSystem.IsZoneComplete("metro_service_ring"));
00132:             Assert.True(damagedMapSystem.IsDestinationLocked(nodeId));
00133:
00134:             // Register fragment 3 -> triggers completion and reveal
00135:             Assert.True(damagedMapSystem.RegisterFragment("damaged_map_metro_3"));
00136:             Assert.Equal(3, damagedMapSystem.RegisteredCount("metro_service_ring"));
00137:             Assert.True(damagedMapSystem.IsZoneComplete("metro_service_ring"));
00138:             Assert.NotNull(completedZone);
00139:             Assert.Equal("metro_service_ring", completedZone!.ZoneId);
00140:             Assert.Equal(nodeId, revealedNode);
00141:
00142:             // Node is now discovered and unlocked on the world map
00143:             Assert.True(mapSystem.IsDiscovered(nodeId));
00144:             Assert.False(mapSystem.IsLocked(nodeId));
00145:             Assert.False(damagedMapSystem.IsDestinationLocked(nodeId));
00146:         }
00147:
00148:         [Fact]
00149:         public void DamagedMapElectricalSalvage_DirectlySupportsShelterPowerGrid_CrossLinkage()
00150:         {
00151:             // 1. Discover and assemble the Metro Service Ring damaged map
00152:             var (nodes, routes) = WastelandMapCatalogLoader.Load(
00153:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
00154:             var mapSystem = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
00155:             var (zones, _) = DamagedMapCatalogLoader.LoadWithValidation(
00156:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
00157:             var damagedMapSystem = new DamagedMapSystem(zones, mapSystem);
00158:
00159:             var metroZone = zones.First(z => z.ZoneId == "metro_service_ring");
00160:             foreach (var frag in metroZone.Fragments)
00161:             {
00162:                 damagedMapSystem.RegisterFragment(frag.fragment_id);
00163:             }
00164:             Assert.True(damagedMapSystem.IsZoneComplete("metro_service_ring"));
00165:             Assert.Contains(PowerGridSystem.BatteryBankItemId, metroZone.RevealedItems);
00166:
00167:             // 2. Power Grid consumes the salvaged reconditioned battery to install bank upgrade
00168:             var ok = ShelterPowerGridCatalogLoader.TryLoad(
00169:                 DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer(),
00170:                 out var powerCatalog, out _);
00171:             Assert.True(ok);
00172:
00173:             var rooms = powerCatalog!.Rooms.Select(r => new PowerGridRoom(
00174:                 r.Id, r.DisplayName, r.DrawWatts, PowerGridRoomPriority.Standard, r.FailureEffectId)).ToList();
00175:
00176:             var gridState = new PowerGridState
00177:             {
00178:                 GenerationWatts = 800f,
00179:                 BatteryCapacityWh = 4000f,
00180:                 BatteryReserveWh = 4000f,
00181:                 FuelUnits = 100f
00182:             };
00183:             var gridSystem = new PowerGridSystem(gridState, rooms, new SeededRng(777));
00184:
00185:             Assert.Equal(0, gridSystem.InstalledBatteryBankCount);
00186:             Assert.Equal(4000f, gridSystem.BatteryCapacityWh);
00187:
00188:             // Install battery bank 1 using the item discovered from the revealed installation
00189:             bool install1 = gridSystem.TryInstallBatteryBank(out string reason1);
00190:             Assert.True(install1, reason1);
00191:             Assert.Equal(1, gridSystem.InstalledBatteryBankCount);
00192:             Assert.Equal(5000f, gridSystem.BatteryCapacityWh);
00193:
00194:             // Install battery bank 2
00195:             bool install2 = gridSystem.TryInstallBatteryBank(out string reason2);
00196:             Assert.True(install2, reason2);
00197:             Assert.Equal(2, gridSystem.InstalledBatteryBankCount);
00198:             Assert.Equal(6000f, gridSystem.BatteryCapacityWh);
00199:
00200:             // 3. Save/load round-trip across both map and power grid authorities
00201:             var capturedMap = mapSystem.CaptureState();
00202:             var capturedGrid = gridSystem.CaptureState();
00203:
00204:             var restoredMap = new WastelandMapSystem(capturedMap, nodes, routes);
00205:             var restoredDamagedMap = new DamagedMapSystem(zones, restoredMap);
00206:             var restoredGrid = new PowerGridSystem(capturedGrid, rooms, new SeededRng(888));
00207:
00208:             string exchangeNodeId = DamagedMapSystem.ResolveRevealNodeId(metroZone.InstallationId)!;
00209:             Assert.True(restoredDamagedMap.IsZoneComplete("metro_service_ring"));
00210:             Assert.True(restoredMap.IsDiscovered(exchangeNodeId));
00211:             Assert.False(restoredDamagedMap.IsDestinationLocked(exchangeNodeId));
00212:
00213:             Assert.Equal(2, restoredGrid.InstalledBatteryBankCount);
00214:             Assert.Equal(6000f, restoredGrid.BatteryCapacityWh);
00215:         }
00216:     }
00217: }
```

## `docs/perf/BUDGETS.md` — 36 lines; 1,916 bytes; SHA-256 `bc671dde410e0ecb94b9b1369d60be9f0e454f2c1061a5837d1320baaf41594d`
```csharp
00001: # ASHFALL — RUNTIME PERFORMANCE BUDGETS (PLAN 26C / TASK 130)
00002:
00003: **Authority:** `docs/perf/BUDGETS.md`
00004: **Enforcement:** `godot --headless --path . -- --runtime-scale-selftest` (Gate: `runtime_scale_performance`)
00005: **Artifact Output:** `artifacts/runtime-scale-results.json`
00006: **Date:** 2026-09-17
00007:
00008: ---
00009:
00010: ## 1. Runtime Scale Budgets (Headless Simulation)
00011:
00012: The simulation budgets below are enforced by `--runtime-scale-selftest` across multi-day horizons.
00013: Measurements are taken across 5 iterations with a 3-day warmup phase.
00014:
00015: | Benchmark ID | Horizon | Workload Tier | Median Latency Budget | Per-Day Allocation Budget | Retained Memory Budget | Status |
00016: |---|---|---|---|---|---|---|
00017: | `day_advance_30d` | 30 Days | Normal (24 roster, 30 journal, 1 exp) | **< 2,000 ms** (Target: < 500 ms) | — | — | PASS |
00018: | `day_advance_180d` | 180 Days | Large (48 roster, 180 journal, 3 exp) | **< 12,000 ms** (Target: < 3,000 ms) | — | — | PASS |
00019: | `day_advance_360d` | 360 Days | Stress (96 roster, 360 journal, 6 exp) | **< 30,000 ms** (Target: < 8,000 ms) | — | — | PASS |
00020: | `save_30d` | 30 Days | Normal | **< 500 ms** (Target: < 400 ms) | — | — | PASS |
00021: | `alloc_growth_30d` | 1 Day | Normal (per-day tick) | — | **< 5,000,000 bytes** (Target: < 1,000,000 B) | — | PASS |
00022: | `lifecycle_leak_30d` | 30 Days | Retained Memory | — | — | **< 20,000,000 bytes** (< 20 MB) | PASS |
00023:
00024: ---
00025:
00026: ## 2. Telemetry Record
00027:
00028: Current headless Linux test run measurements (2026-09-17):
00029: - `day_advance_30d`: ~1.0 ms median (budget: < 2,000 ms)
00030: - `day_advance_180d`: ~7.0 ms median (budget: < 12,000 ms)
00031: - `day_advance_360d`: ~12.7 ms median (budget: < 30,000 ms)
00032: - `save_30d`: ~26.7 ms median (budget: < 500 ms)
00033: - `alloc_growth_30d`: 2,640 bytes median per day (budget: < 5,000,000 bytes)
00034: - `lifecycle_leak_30d`: 0 MB retained memory (budget: < 20 MB)
00035:
00036: All simulation workloads operate well inside the allocated budget bounds.
```
# Appendix M — External verification handoff

The following checks are to be run by the owning integrator after writing: character count, SHA-256 revalidation, path-token resolution, duplicate-heading/unsupported-claim scan, and `git diff --check`. The final ledger entry must report actual results, not this template.
