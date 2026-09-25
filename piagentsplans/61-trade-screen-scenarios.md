# Plan 61 — Trade Screen Scenarios: Fifteen-Catalog Negotiation Surface and Canonical Transaction Boundary

> **Rebuild status:** TERMINAL 15-SCENARIO CONTENT + LIVE-SURFACE PREMISE AUDIT
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

The historical baseline was 4,801 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Preserve the fifteen authored trade-screen scenarios as a deterministic negotiation/presentation corpus while distinguishing the scenario presenter from the canonical economy transaction, inventory, funds, faction-standing, and debt owners. The old plan’s 3→15 data expansion is complete; the next step is a current reachability and fairness audit, not another scenario count.

**Bounded outcome:** Audit the scenario loader/presenter, the Godot trade panel, live `OpenTradeScreen` composition, market/inventory/funds seams, and focused tests. Determine whether scenario rows are live command inputs, headless fixtures, or a parallel presentation model, and document the smallest safe correction.

**Non-goals:** no second market or barter owner, no arbitrary scenario growth, no automatic debt/funds mutation, no new save section, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `trade_screen_scenarios.json` contains 15 rows with scenario, faction, trust, shock, scarcity, offer, demand, and fairness fields.
- VERIFIED CURRENT: `TradeScreenPresenter` is deterministic, ordinal-stable, and uses a caller-supplied seeded stream.
- VERIFIED CURRENT: `Main.OpenTradeScreen` binds the live economy/Foundry Guild trade surface; the current plan must verify whether it also binds the scenario catalog.
- HISTORICAL RECORD: Plan 61/earlier work records the 3→15 expansion; this package does not claim a fresh test run.

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

- `Assets/StreamingAssets/Data/trade_screen_scenarios.json` exists at 23,775 bytes; SHA-256 `fc2d88b9a5cbe663c054a25226096a59dc259089b01d4521952314d6c37e507c`.
- `Assets/StreamingAssets/Data/trade_tell_lines.json` exists at 8,811 bytes; SHA-256 `a86a8e9486d67d14d4ed4df63a47a32ecc25988d2287f822bb57754376b28b08`.
- `Assets/StreamingAssets/Data/economy_goods.json` exists at 21,607 bytes; SHA-256 `18c8a68c9479179b68557cc28bc9fbce71a988e1d5c2a130c9bcf47c20a0739b`.
- `Assets/StreamingAssets/Data/regional_prices.json` exists at 3,789 bytes; SHA-256 `5dee33fbf091fdaae985b322764f32d6c2693c2a4da2a1486d0491eff4d48860`.

# 3. Required Delta

Replace the old pure-data brief with a current fifteen-row census and a live-route/fairness audit. Separate deterministic negotiation projection from canonical market, inventory, funds, standing, and debt mutation.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Does the live `OpenTradeScreen` path load any row from `trade_screen_scenarios.json`, or are rows currently headless/demo fixtures?
Which fields are presentation-only and which are read by current economy owners?
Does the presenter’s `confirm_succeeds` ever reach a canonical transaction command?
How are seeded tie-breaks and empty/refusal states preserved across panel reopen?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| scenario definitions and parsing | `TradeScreenScenarioLoader` | `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs` | Owns authored scenario DTOs, not inventory or funds. |
| deterministic negotiation projection | `TradeScreenPresenter` | `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs` | Builds view models and intent results; canonical transaction remains elsewhere. |
| live trade panel | `TradeScreenGodotPanel` | `src/Economy/TradeScreenGodotPanel.cs` | Presentation and intent sink; no shadow economy. |
| canonical market and trade settlement | `EconomyHostSession / MarketSystem / ShelterBarterSystem` | `src/Host/EconomyHostSession.cs; Assets/Ashfall.Core/Economy/` | Authoritative prices, transactions, and inventory. |
| faction standing/debt consequences | `FactionStanceEngine / LedgerDebtSystem` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs; Assets/Ashfall.Core/LedgerDebtSystem.cs` | A scenario cannot write standing or debt directly. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for trade-screen scenario catalog.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Trade Screen Scenarios: Fifteen-Catalog Negotiation Surface and Canonical Transaction Boundary                                               │
│ Integration route: DATA-ONLY + presenter/host seam audit                             │
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

1. **Scenario rows are authored definitions.**
2. **Presenter output is a projection, not settlement.**
3. **Market/inventory/funds remain canonical.**
4. **Faction/debt consequences use their own owners.**
5. **UI never rerolls or fabricates a transaction.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| scenario definitions and parsing | `TradeScreenScenarioLoader` | `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs` | Owns authored scenario DTOs, not inventory or funds. |
| deterministic negotiation projection | `TradeScreenPresenter` | `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs` | Builds view models and intent results; canonical transaction remains elsewhere. |
| live trade panel | `TradeScreenGodotPanel` | `src/Economy/TradeScreenGodotPanel.cs` | Presentation and intent sink; no shadow economy. |
| canonical market and trade settlement | `EconomyHostSession / MarketSystem / ShelterBarterSystem` | `src/Host/EconomyHostSession.cs; Assets/Ashfall.Core/Economy/` | Authoritative prices, transactions, and inventory. |
| faction standing/debt consequences | `FactionStanceEngine / LedgerDebtSystem` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs; Assets/Ashfall.Core/LedgerDebtSystem.cs` | A scenario cannot write standing or debt directly. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load a scenario definition through TradeScreenScenarioLoader
2. bind it to the deterministic TradeScreenPresenter and seeded voice/tell provider
3. project the current scenario into TradeScreenGodotPanel
4. collect player intent through the existing sink
5. execute any real trade through canonical market/inventory/funds owners
6. journal/refresh the current owner state without saving presenter-only DTOs

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- scenario ids and faction ids are unique and stable
- price shocks and scarcity are bounded and deterministic
- fairness/confirm fields are presentation facts, not settlement authority
- a refused or empty table cannot fabricate a trade
- live settlement consumes and delivers through canonical inventory/funds
- reopening a panel cannot reroll a committed transaction

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

Contract rules for trade-screen scenario catalog:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`trade_screen_scenarios.json` remains the scenario-definition authority. Current rows contain faction, stance, trust, scarcity, shock, offer, demand, and expected-fairness fields. Audit each field against its current loader/presenter/host consumer; do not infer that every `item_id` is valid merely because the catalog parses. A future row needs a real settlement or presentation consumer and a continuity check.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

The scenario DTO is presentation/fixture data and should not acquire a new save section. Any committed trade uses the existing economy/market/inventory/funds save owners. If a future negotiation session must survive reload, persist the canonical transaction/session owner rather than the presenter’s view model.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

TradeScreenPresenter uses the caller-supplied seeded stream for voice/tie behavior and must keep ordinal candidate ordering. Scenario rows must not be selected by dictionary/hash order. The same scenario, owner state, seed, and intent sequence must produce the same quote, fairness, and event trace.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

The presenter emits selection/confirm/parley intents through its sink. A successful real transaction belongs to the canonical economy owner and its existing events. A scenario must not emit a fake debt, standing, inventory, or market mutation merely because `confirm_succeeds` is true in a fixture.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Economy/TradeScreenGodotPanel.cs` — binds the presenter/view model and renders current trade state
- `src/Main.Economy.cs` — opens the live Foundry Guild trade surface and binds canonical economy/stance providers
- `src/UI/CaravanBarterLedgerPanel.cs` — dashboard wrapper for the existing trade surface
- `src/Host/EconomyHostSession.cs` — canonical market, inventory, and transaction seam
- `src/Main.PlayerSurfaces.cs` — route/open/lifecycle integration

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Scenario descriptions and radio ticker lines are diegetic negotiation texture. They may describe scarcity, shock, or a faction’s posture, but cannot claim that a transaction settled unless the canonical owner recorded it. Keep prices and quantities legible and avoid real currencies, copied layouts, or moralizing exposition.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/TradeScreenPresenterSnapshotTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/TradeScreenSeamTests.cs`
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
| Phase 0 — scenario/catalog census | read all 15 rows and current loader/presenter | all fields and current consumers are enumerated | no undocumented scope or shortcut |
| Phase 1 — live-route trace | trace Main.Economy → panel → presenter → canonical owner | scenario reachability is proven or explicitly downgraded | no undocumented scope or shortcut |
| Phase 2 — fairness/save audit | check seeded ordering, empty/refusal, and canonical persistence | no presenter-only state is persisted | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a proven live route or fairness defect is proposed | one owner and focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/trade_screen_scenarios.json` | READ ONLY; MODIFY only for a proven row/consumer defect | retain as scenario authority |
| `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs` | READ ONLY | loader/DTO boundary |
| `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs` | READ ONLY | deterministic presenter |
| `src/Economy/TradeScreenGodotPanel.cs` | READ ONLY | live presentation |
| `src/Main.Economy.cs` | READ ONLY | live route composition |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| parallel economy authority | keep market/inventory/funds canonical |
| fixture data mistaken for live content | trace production callers and label evidence |
| RNG drift from UI refresh | use the existing campaign economy stream and stable ordering |
| unbounded scenario growth | measure coverage before adding rows |

# 22. Explicit Non-Goals

- no second market or barter owner, no arbitrary scenario growth, no automatic debt/funds mutation, no new save section, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for trade-screen scenario catalog are named from current evidence.
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

- A current field-to-consumer and fixture/live classification for all fifteen scenarios.
- A precise boundary between TradeScreenPresenter and canonical settlement.
- A bounded live-route/fairness package only if current evidence proves a gap.

## MUST NOT DO

- create a second market, barter, funds, or debt owner
- make a scenario boolean settle inventory
- save the presenter view model as a new section
- grow scenarios merely to reach a number

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/TradeScreenPresenterSnapshotTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/TradeScreenSeamTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read TradeScreenScenarios, TradeScreenPresenter, TradeScreenGodotPanel, Main.Economy, and focused tests; classify each scenario as live, fixture, dormant, or invalid.

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

### Authority lines 762–767
00762: **B-17 · C10 · Moral-choice gossip propagation depth.** Subject: choice-driven gossip traveling the modeled channels with time lag proportional to distance. Evidence: `moral_choice_gossip.json` verified live; information-flow rules are canon. Route: CORE-EXTENSION. Confidence: PROPOSAL.
00763:
00764: **B-18 · C11 · Trade-screen scenario expansion.** Subject: additional scenarios and tell lines for under-covered merchant identities. Evidence: `trade_screen_scenarios.json`, `trade_tell_lines.json`, `trade_specialties.json` verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00765:
00766: **B-19 · C12 · Winter pressure for power and water systems.** Subject: Year-of-Ash window (180–360) pressure extensions for systems that currently produce no winter-specific cost (filter burn, diesel reserve drawdown curves conditioned on storm windows). Evidence: storm windows canon; hardening upgrades catalog live. Route: CORE-EXTENSION + data. Confidence: PROPOSAL.
00767:

### Authority lines 842–847
00842: **E-07 · C5 · Expedition camp panel depth.** Subject: expose truthful existing expedition state that the camp panel does not yet render (verify via layout selftest before claiming). Evidence: expedition camp panel exists (v1.0 Part 5.7). Route: HOST-WIRING. Confidence: INFERENCE pending selftest.
00843:
00844: **E-08 · C11 · Caravan barter ledger legibility.** Subject: `CaravanBarterLedger` panel state completeness against the sealed merchant-restock display-order priority (DEC-05). Evidence: DEC-05 sealed (DR-06). Route: HOST-WIRING. Confidence: PROPOSAL.
00845:
00846: **E-09 · C12 · Storm-window forecast legibility.** Subject: weather forecast surface rendering storm-window warnings with adequate lead time for the 180–360 window. Evidence: forecast observation is a canon loop step. Route: HOST-WIRING. Confidence: PROPOSAL.
00847:

### Authority lines 871–876
00871: **G-01 · C10 · Moral-choice flag consumer coverage.** Subject: tests proving every authored flag id has at least one consumer path and every consumer reads a persisted flag (supports F-001 and D-07). Evidence: flags catalog live. Route: focused xUnit, aggregate with per-row failures. Confidence: HIGH CONFIDENCE.
00872:
00873: **G-02 · C11 · Debt-consequence dispatcher coverage.** Subject: dispatcher coverage for every consequence kind in the closed vocabulary, with recovery-path assertions. Evidence: dispatchers canon; recovery grammar canon. Route: focused xUnit. Confidence: HIGH CONFIDENCE.
00874:
00875: **G-03 · C2 · Dose-treatment matrix pairing tests.** Subject: pin each matrix row to its implementing treatment logic so the generated matrix cannot drift from code. Evidence: matrix live (DR-03). Route: focused xUnit + generation check. Confidence: HIGH CONFIDENCE.
00876:

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

### Authority lines 951–956
00951: **DM-10 — Quests and moral choice (C10).** Owners: questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip/quests (five split catalogs live), branching faction quests, bureaucratic morality, massive expansion corpus, repeatable quests, templates, domain questlines (dose, year-of-ash, holdfast, crossing, thirdonary, verdict, expansion). Live catalogs: `questline_master`, `dynamic_questlines`, `personal_quests`, `npc_arcs`, `quests_npc_arcs`, `moral_choice_chains/flags/gossip/quests/quests_branching/quests_distress/quests_expansion`, `quests_faction_branching`, `quests_bureaucratic_morality`, `quests_massive_expansion_200`, `quests_moral_branching_expansion`, `repeatable_quests`, `quest_templates`. Hosts: NarrativeQuestline, PersonalQuest, MoralChoice, DynamicQuestline, ExpansionQuest, NpcArc. Openings: A-24, A-25, B-16, B-17, D-07, G-01, plus the F-001 flagship.
00952:
00953: **DM-11 — Economy (C11).** Owners: market, price factors, shocks, baselines, regional prices, hardcore tuning, rumor bands, black market, caravans, debt ledger, foundry economy, bounty board, trade screens. Live catalogs: `commodity_baselines`, `regional_prices`, `hardcore_economy_tuning`, `economy_goods`, `black_market_inventory`, `ledger_debt_templates`, `trade_screen_scenarios`, `trade_tell_lines`, `trade_specialties`, `trade_texts`, `bounty_board`. Hosts: Economy, BlackMarket, TravelingCaravan, SilentFoundry. Docs: `ECONOMY_FAIRNESS_AUDIT.md`, `ECONOMY_PRICE_FACTOR_MATRIX.md` (verified live). Sealed: merchant restock priority (DEC-05). Openings: A-26, B-18, C-07, C-08, C-13, E-08, G-02. GATE: black-market funds legs.
00954:
00955: **DM-12 — Weather and Year of Ash (C12).** Owners: weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash family (events/items/locations/questlines/quests/radio/survivors/storm windows). Live catalogs: all of the above verified live. Hosts: WeatherHardening, YearOfAsh widgets, WeatherStationSystem. Openings: A-27, B-03, B-19, C-09, E-09, F-02, G-07, plus the F-002 campaign. Constraint: tick window 180–360 canon.
00956:

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/trade_screen_scenarios.json`
- Bytes: 23,775; SHA-256: `fc2d88b9a5cbe663c054a25226096a59dc259089b01d4521952314d6c37e507c`
- Root keys: `$schema, description, scenarios, schema_version, version`
- `scenarios`: list[15]; union fields: `aggression, biological_offers, can_demand_parley, confirm_succeeds, consecutive_repels, expected_fairness, faction_demands, faction_id, faction_name, has_surrendered, id, leader_name, player_offers, price_shocks, radio_ticker, scarcity, stance, succession_generation, trust, world_day, world_phase`
  - row 1: `{"aggression":0.35,"biological_offers":{"PintOfBlood":1},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Clean Water","item_id":"clean_water","quantity":2,"unit_price":22}],"faction_id":"scavenger_camp","faction_name":"Scavenger Camp","has_surrendered":false,"id":"fair_deal","leader_name":"Varek","player_offers":[{"display_name":"Canned Food","item_id":"canned_food","quantity":3,"unit_price":18},{"display_name":"Duct Tape","item_id":"duct_tape","quantity":1,"unit_price":15}],"price_shocks":[{"kind":"PlumePassing","multiplier":2.5,"note":"rad plume over th…`
  - row 2: `{"aggression":0.6,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":2,"expected_fairness":"short","faction_demands":[{"display_name":"Fuel","item_id":"fuel","quantity":2,"unit_price":40}],"faction_id":"upland_militia","faction_name":"Upland Militia","has_surrendered":false,"id":"offer_short","leader_name":"Sergeant Oduya","player_offers":[{"display_name":"Canned Food","item_id":"canned_food","quantity":1,"unit_price":18}],"price_shocks":[{"kind":"ConvoyAmbush","multiplier":1.8,"note":"fuel convoy burned on the ridge road"}],"radio_ticker":"RADIO: [GRAYLINE] Ridge road closed after the fire. Haul y…`
  - row 3: `{"aggression":0.2,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":0,"expected_fairness":"empty","faction_demands":[],"faction_id":"rot_farmers","faction_name":"Rot Farmers","has_surrendered":false,"id":"empty_table","leader_name":"Mother Ilde","player_offers":[],"price_shocks":[],"radio_ticker":"RADIO: [DEAD RELAY] Nothing but carrier hum. The table stays bare.","scarcity":[],"stance":"Refuse","succession_generation":2,"trust":-25,"world_day":41,"world_phase":"LongWinter"}`
  - row 4: `{"aggression":0.15,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Antibiotics","item_id":"antibiotics","quantity":1,"unit_price":45},{"display_name":"Splint","item_id":"splint","quantity":1,"unit_price":10}],"faction_id":"sump_dredgers","faction_name":"Sump Dredgers","has_surrendered":false,"id":"last_vials","leader_name":"Wading Maret","player_offers":[{"display_name":"Clean Water","item_id":"clean_water","quantity":2,"unit_price":22},{"display_name":"Salted Meat","item_id":"item_salted_meat","quantity":1,"unit_price":16}],"price_…`
  - row 5: `{"aggression":0.25,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":1,"expected_fairness":"short","faction_demands":[{"display_name":"Heavy Wool Coat","item_id":"item_heavy_wool_coat","quantity":1,"unit_price":34},{"display_name":"Fur Mittens","item_id":"item_fur_mittens","quantity":1,"unit_price":14},{"display_name":"Charcoal","item_id":"charcoal","quantity":2,"unit_price":8}],"faction_id":"cult_of_the_glow","faction_name":"Cult of the Glow","has_surrendered":false,"id":"winter_cart","leader_name":"Tender Casimir","player_offers":[{"display_name":"Grain Flour","item_id":"item_grain_flour","quant…`
  - row 6: `{"aggression":0.3,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Gas Mask","item_id":"gas_mask","quantity":1,"unit_price":55},{"display_name":"Filter Pack","item_id":"filter_pack","quantity":1,"unit_price":28}],"faction_id":"military_remnants","faction_name":"Military Remnants","has_surrendered":false,"id":"depot_window","leader_name":"Quartermaster Brann","player_offers":[{"display_name":"Salted Meat","item_id":"item_salted_meat","quantity":3,"unit_price":16},{"display_name":"Trade Salt Sack","item_id":"item_trade_salt_sack","quan…`
  - row 7: `{"aggression":0.7,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":1,"expected_fairness":"short","faction_demands":[{"display_name":"Military Radio","item_id":"military_radio","quantity":1,"unit_price":70},{"display_name":"Cipher Rotor","item_id":"item_radio_cipher_rotor","quantity":1,"unit_price":45}],"faction_id":"upland_militia","faction_name":"Upland Militia","has_surrendered":false,"id":"emergency_requisition","leader_name":"Sergeant Oduya","player_offers":[{"display_name":"Fuel","item_id":"fuel","quantity":2,"unit_price":40},{"display_name":"Chemical Scrap","item_id":"scrap_chemical","quant…`
  - row 8: `{"aggression":0.4,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Rad-Away","item_id":"rad_away","quantity":1,"unit_price":38},{"display_name":"Anti-Rad","item_id":"anti_rad","quantity":1,"unit_price":35},{"display_name":"Filter Pack","item_id":"filter_pack","quantity":1,"unit_price":28}],"faction_id":"faction_black_flotilla","faction_name":"Black Flotilla","has_surrendered":false,"id":"back_room_exchange","leader_name":"Barge-Mistress Yeva","player_offers":[{"display_name":"Clean Water Jug","item_id":"clean_water_jug","quantity":3,…`
  - row 9: `{"aggression":0.3,"biological_offers":{"BoneMarrow":1},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Comm Codebook Alpha","item_id":"item_comm_codebook_alpha","quantity":1,"unit_price":40},{"display_name":"Radio Headset","item_id":"radio_headset","quantity":1,"unit_price":25}],"faction_id":"wire_heads","faction_name":"Wire Heads","has_surrendered":false,"id":"ledgerless_broker","leader_name":"Splice","player_offers":[{"display_name":"Logistics Cipher Sheet","item_id":"item_logistics_cipher_sheet","quantity":1,"unit_price":30},{"display_name":"Electronic…`
  - row 10: `{"aggression":0.2,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Canned Grain Stew","item_id":"item_canned_grain_stew","quantity":2,"unit_price":15},{"display_name":"Grain Flour","item_id":"item_grain_flour","quantity":1,"unit_price":14}],"faction_id":"doomsday_preppers","faction_name":"Doomsday Preppers","has_surrendered":false,"id":"long_road_caravan","leader_name":"Warden Tull","player_offers":[{"display_name":"Trade Salt Sack","item_id":"item_trade_salt_sack","quantity":4,"unit_price":10},{"display_name":"Cloth","item_id":"clot…`
  - row 11: `{"aggression":0.25,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Steel Rail Segment","item_id":"steel_rail_segment","quantity":1,"unit_price":15},{"display_name":"Steel Billet","item_id":"item_metallurgy_steel_billet","quantity":1,"unit_price":18}],"faction_id":"faction_silent_foundry","faction_name":"Silent Foundry","has_surrendered":false,"id":"salvage_caravan","leader_name":"Factor Imke","player_offers":[{"display_name":"Scrap Metal","item_id":"scrap_metal","quantity":3,"unit_price":4},{"display_name":"Box of Nails","item_id":"…`
  - row 12: `{"aggression":0.5,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":0,"expected_fairness":"short","faction_demands":[{"display_name":"Canned Food","item_id":"canned_food","quantity":2,"unit_price":18},{"display_name":"Fuel","item_id":"fuel","quantity":1,"unit_price":40},{"display_name":"Battery Pack","item_id":"battery_pack","quantity":1,"unit_price":20}],"faction_id":"custodians","faction_name":"The Custodians","has_surrendered":false,"id":"settlement_of_accounts","leader_name":"Record-Keeper Anselm","player_offers":[],"price_shocks":[],"radio_ticker":"RADIO: [CUSTODIAN RECORD] Accounts come due …`
  - row 13: `{"aggression":0.2,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Canned Grain Stew","item_id":"item_canned_grain_stew","quantity":5,"unit_price":15},{"display_name":"Grain Flour","item_id":"item_grain_flour","quantity":4,"unit_price":14}],"faction_id":"hydro_barons","faction_name":"Hydro Barons","has_surrendered":false,"id":"crate_lot","leader_name":"Lockmaster Vess","player_offers":[{"display_name":"Clean Water Jug","item_id":"clean_water_jug","quantity":5,"unit_price":30},{"display_name":"Honey Pot","item_id":"item_honey_pot","qu…`
  - row 14: `{"aggression":0.45,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Military Radio Module","item_id":"item_military_radio_module","quantity":1,"unit_price":60},{"display_name":"Radio Vacuum Tube","item_id":"item_radio_vacuum_tube","quantity":1,"unit_price":18}],"faction_id":"echo_bats","faction_name":"Echo Bats","has_surrendered":false,"id":"border_runner","leader_name":"Whistler Kane","player_offers":[{"display_name":"Fuel","item_id":"fuel","quantity":2,"unit_price":40},{"display_name":"Reconditioned Battery","item_id":"item_battery…`
  - row 15: `{"aggression":0.1,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Road Map","item_id":"item_collectible_road_map","quantity":1,"unit_price":20},{"display_name":"Evacuation Route Map","item_id":"item_document_evacuation_route_map","quantity":1,"unit_price":25},{"display_name":"Bandage","item_id":"bandage","quantity":1,"unit_price":12}],"faction_id":"safe_haven_community","faction_name":"Safe Haven Community","has_surrendered":false,"id":"road_knowledge","leader_name":"Door-Warden Pella","player_offers":[{"display_name":"Canned Food",…`
- Bytes: 23,775; SHA-256: `fc2d88b9a5cbe663c054a25226096a59dc259089b01d4521952314d6c37e507c`
- Root keys: `$schema, description, scenarios, schema_version, version`
- `scenarios`: list[15]; union fields: `aggression, biological_offers, can_demand_parley, confirm_succeeds, consecutive_repels, expected_fairness, faction_demands, faction_id, faction_name, has_surrendered, id, leader_name, player_offers, price_shocks, radio_ticker, scarcity, stance, succession_generation, trust, world_day, world_phase`
  - row 1: `{"aggression":0.35,"biological_offers":{"PintOfBlood":1},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Clean Water","item_id":"clean_water","quantity":2,"unit_price":22}],"faction_id":"scavenger_camp","faction_name":"Scavenger Camp","has_surrendered":false,"id":"fair_deal","leader_name":"Varek","player_offers":[{"display_name":"Canned Food","item_id":"canned_food","quantity":3,"unit_price":18},{"display_name":"Duct Tape","item_id":"duct_tape","quantity":1,"unit_price":15}],"price_shocks":[{"kind":"PlumePassing","multiplier":2.5,"note":"rad plume over th…`
  - row 2: `{"aggression":0.6,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":2,"expected_fairness":"short","faction_demands":[{"display_name":"Fuel","item_id":"fuel","quantity":2,"unit_price":40}],"faction_id":"upland_militia","faction_name":"Upland Militia","has_surrendered":false,"id":"offer_short","leader_name":"Sergeant Oduya","player_offers":[{"display_name":"Canned Food","item_id":"canned_food","quantity":1,"unit_price":18}],"price_shocks":[{"kind":"ConvoyAmbush","multiplier":1.8,"note":"fuel convoy burned on the ridge road"}],"radio_ticker":"RADIO: [GRAYLINE] Ridge road closed after the fire. Haul y…`
  - row 3: `{"aggression":0.2,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":0,"expected_fairness":"empty","faction_demands":[],"faction_id":"rot_farmers","faction_name":"Rot Farmers","has_surrendered":false,"id":"empty_table","leader_name":"Mother Ilde","player_offers":[],"price_shocks":[],"radio_ticker":"RADIO: [DEAD RELAY] Nothing but carrier hum. The table stays bare.","scarcity":[],"stance":"Refuse","succession_generation":2,"trust":-25,"world_day":41,"world_phase":"LongWinter"}`
  - row 4: `{"aggression":0.15,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Antibiotics","item_id":"antibiotics","quantity":1,"unit_price":45},{"display_name":"Splint","item_id":"splint","quantity":1,"unit_price":10}],"faction_id":"sump_dredgers","faction_name":"Sump Dredgers","has_surrendered":false,"id":"last_vials","leader_name":"Wading Maret","player_offers":[{"display_name":"Clean Water","item_id":"clean_water","quantity":2,"unit_price":22},{"display_name":"Salted Meat","item_id":"item_salted_meat","quantity":1,"unit_price":16}],"price_…`
  - row 5: `{"aggression":0.25,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":1,"expected_fairness":"short","faction_demands":[{"display_name":"Heavy Wool Coat","item_id":"item_heavy_wool_coat","quantity":1,"unit_price":34},{"display_name":"Fur Mittens","item_id":"item_fur_mittens","quantity":1,"unit_price":14},{"display_name":"Charcoal","item_id":"charcoal","quantity":2,"unit_price":8}],"faction_id":"cult_of_the_glow","faction_name":"Cult of the Glow","has_surrendered":false,"id":"winter_cart","leader_name":"Tender Casimir","player_offers":[{"display_name":"Grain Flour","item_id":"item_grain_flour","quant…`
  - row 6: `{"aggression":0.3,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Gas Mask","item_id":"gas_mask","quantity":1,"unit_price":55},{"display_name":"Filter Pack","item_id":"filter_pack","quantity":1,"unit_price":28}],"faction_id":"military_remnants","faction_name":"Military Remnants","has_surrendered":false,"id":"depot_window","leader_name":"Quartermaster Brann","player_offers":[{"display_name":"Salted Meat","item_id":"item_salted_meat","quantity":3,"unit_price":16},{"display_name":"Trade Salt Sack","item_id":"item_trade_salt_sack","quan…`
  - row 7: `{"aggression":0.7,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":1,"expected_fairness":"short","faction_demands":[{"display_name":"Military Radio","item_id":"military_radio","quantity":1,"unit_price":70},{"display_name":"Cipher Rotor","item_id":"item_radio_cipher_rotor","quantity":1,"unit_price":45}],"faction_id":"upland_militia","faction_name":"Upland Militia","has_surrendered":false,"id":"emergency_requisition","leader_name":"Sergeant Oduya","player_offers":[{"display_name":"Fuel","item_id":"fuel","quantity":2,"unit_price":40},{"display_name":"Chemical Scrap","item_id":"scrap_chemical","quant…`
  - row 8: `{"aggression":0.4,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Rad-Away","item_id":"rad_away","quantity":1,"unit_price":38},{"display_name":"Anti-Rad","item_id":"anti_rad","quantity":1,"unit_price":35},{"display_name":"Filter Pack","item_id":"filter_pack","quantity":1,"unit_price":28}],"faction_id":"faction_black_flotilla","faction_name":"Black Flotilla","has_surrendered":false,"id":"back_room_exchange","leader_name":"Barge-Mistress Yeva","player_offers":[{"display_name":"Clean Water Jug","item_id":"clean_water_jug","quantity":3,…`
  - row 9: `{"aggression":0.3,"biological_offers":{"BoneMarrow":1},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Comm Codebook Alpha","item_id":"item_comm_codebook_alpha","quantity":1,"unit_price":40},{"display_name":"Radio Headset","item_id":"radio_headset","quantity":1,"unit_price":25}],"faction_id":"wire_heads","faction_name":"Wire Heads","has_surrendered":false,"id":"ledgerless_broker","leader_name":"Splice","player_offers":[{"display_name":"Logistics Cipher Sheet","item_id":"item_logistics_cipher_sheet","quantity":1,"unit_price":30},{"display_name":"Electronic…`
  - row 10: `{"aggression":0.2,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Canned Grain Stew","item_id":"item_canned_grain_stew","quantity":2,"unit_price":15},{"display_name":"Grain Flour","item_id":"item_grain_flour","quantity":1,"unit_price":14}],"faction_id":"doomsday_preppers","faction_name":"Doomsday Preppers","has_surrendered":false,"id":"long_road_caravan","leader_name":"Warden Tull","player_offers":[{"display_name":"Trade Salt Sack","item_id":"item_trade_salt_sack","quantity":4,"unit_price":10},{"display_name":"Cloth","item_id":"clot…`
  - row 11: `{"aggression":0.25,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Steel Rail Segment","item_id":"steel_rail_segment","quantity":1,"unit_price":15},{"display_name":"Steel Billet","item_id":"item_metallurgy_steel_billet","quantity":1,"unit_price":18}],"faction_id":"faction_silent_foundry","faction_name":"Silent Foundry","has_surrendered":false,"id":"salvage_caravan","leader_name":"Factor Imke","player_offers":[{"display_name":"Scrap Metal","item_id":"scrap_metal","quantity":3,"unit_price":4},{"display_name":"Box of Nails","item_id":"…`
  - row 12: `{"aggression":0.5,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":false,"consecutive_repels":0,"expected_fairness":"short","faction_demands":[{"display_name":"Canned Food","item_id":"canned_food","quantity":2,"unit_price":18},{"display_name":"Fuel","item_id":"fuel","quantity":1,"unit_price":40},{"display_name":"Battery Pack","item_id":"battery_pack","quantity":1,"unit_price":20}],"faction_id":"custodians","faction_name":"The Custodians","has_surrendered":false,"id":"settlement_of_accounts","leader_name":"Record-Keeper Anselm","player_offers":[],"price_shocks":[],"radio_ticker":"RADIO: [CUSTODIAN RECORD] Accounts come due …`
  - row 13: `{"aggression":0.2,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Canned Grain Stew","item_id":"item_canned_grain_stew","quantity":5,"unit_price":15},{"display_name":"Grain Flour","item_id":"item_grain_flour","quantity":4,"unit_price":14}],"faction_id":"hydro_barons","faction_name":"Hydro Barons","has_surrendered":false,"id":"crate_lot","leader_name":"Lockmaster Vess","player_offers":[{"display_name":"Clean Water Jug","item_id":"clean_water_jug","quantity":5,"unit_price":30},{"display_name":"Honey Pot","item_id":"item_honey_pot","qu…`
  - row 14: `{"aggression":0.45,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Military Radio Module","item_id":"item_military_radio_module","quantity":1,"unit_price":60},{"display_name":"Radio Vacuum Tube","item_id":"item_radio_vacuum_tube","quantity":1,"unit_price":18}],"faction_id":"echo_bats","faction_name":"Echo Bats","has_surrendered":false,"id":"border_runner","leader_name":"Whistler Kane","player_offers":[{"display_name":"Fuel","item_id":"fuel","quantity":2,"unit_price":40},{"display_name":"Reconditioned Battery","item_id":"item_battery…`
  - row 15: `{"aggression":0.1,"biological_offers":{},"can_demand_parley":false,"confirm_succeeds":true,"consecutive_repels":0,"expected_fairness":"fair","faction_demands":[{"display_name":"Road Map","item_id":"item_collectible_road_map","quantity":1,"unit_price":20},{"display_name":"Evacuation Route Map","item_id":"item_document_evacuation_route_map","quantity":1,"unit_price":25},{"display_name":"Bandage","item_id":"bandage","quantity":1,"unit_price":12}],"faction_id":"safe_haven_community","faction_name":"Safe Haven Community","has_surrendered":false,"id":"road_knowledge","leader_name":"Door-Warden Pella","player_offers":[{"display_name":"Canned Food",…`

## `Assets/StreamingAssets/Data/trade_tell_lines.json`
- Bytes: 8,811; SHA-256: `a86a8e9486d67d14d4ed4df63a47a32ecc25988d2287f822bb57754376b28b08`
- Root keys: `$schema, description, schema_version, tells, trust_bands, version`
- `trust_bands`: list[4]; union fields: `id, max, min`
  - row 1: `{"id":"hostile","max":-40,"min":-100}`
  - row 2: `{"id":"wary","max":0,"min":-39}`
  - row 3: `{"id":"neutral","max":40,"min":1}`
  - row 4: `{"id":"warm","max":100,"min":41}`
- `tells`: object[5]
  - `hostile_raid`: object[4]; keys: `hostile, wary, neutral, warm`
  - `rob`: object[4]; keys: `hostile, wary, neutral, warm`
  - `refuse`: object[4]; keys: `hostile, wary, neutral, warm`
  - `trade`: object[4]; keys: `hostile, wary, neutral, warm`
  - `share_intel`: object[4]; keys: `hostile, wary, neutral, warm`
- Bytes: 8,811; SHA-256: `a86a8e9486d67d14d4ed4df63a47a32ecc25988d2287f822bb57754376b28b08`
- Root keys: `$schema, description, schema_version, tells, trust_bands, version`
- `trust_bands`: list[4]; union fields: `id, max, min`
  - row 1: `{"id":"hostile","max":-40,"min":-100}`
  - row 2: `{"id":"wary","max":0,"min":-39}`
  - row 3: `{"id":"neutral","max":40,"min":1}`
  - row 4: `{"id":"warm","max":100,"min":41}`
- `tells`: object[5]
  - `hostile_raid`: object[4]; keys: `hostile, wary, neutral, warm`
  - `rob`: object[4]; keys: `hostile, wary, neutral, warm`
  - `refuse`: object[4]; keys: `hostile, wary, neutral, warm`
  - `trade`: object[4]; keys: `hostile, wary, neutral, warm`
  - `share_intel`: object[4]; keys: `hostile, wary, neutral, warm`

## `Assets/StreamingAssets/Data/economy_goods.json`
- Bytes: 21,607; SHA-256: `18c8a68c9479179b68557cc28bc9fbce71a988e1d5c2a130c9bcf47c20a0739b`
- Root keys: `goods, schema_version`
- `goods`: list[51]; union fields: `barterNote, basePrice, category, displayName, elasticity, id, regionalSupply, stackSize, volatility, weightKg`
  - row 1: `{"barterNote":"The universal measure. Every faction prices its offers against the litre. A single bottle can buy a night's silence—or a bullet.","basePrice":8.0,"category":"water","displayName":"Clean Water","elasticity":1.2,"id":"clean_water","stackSize":5,"volatility":0.15,"weightKg":1.0}`
  - row 2: `{"barterNote":"Tier-zero scrap; the fallback value everything else is compared against. A handful can patch a hole in the bunker wall—or a hole in a man.","basePrice":3.0,"category":"materials","displayName":"Scrap Metal","elasticity":0.8,"id":"scrap_metal","stackSize":20,"volatility":0.1,"weightKg":2.0}`
  - row 3: `{"barterNote":"Shortage-spiked when supplies run thin. The white cloth turns pink before it turns red. The antiseptic stings worse than the wound.","basePrice":15.0,"category":"medical","displayName":"Bandages","elasticity":1.5,"id":"bandages","stackSize":10,"volatility":0.2,"weightKg":0.2}`
  - row 4: `{"barterNote":"The medicine tier anchor; the sick room's price follows it. The pills are bitter, the cure is worse than the disease. But it works.","basePrice":40.0,"category":"medical","displayName":"Antibiotics","elasticity":1.5,"id":"antibiotics","stackSize":5,"volatility":0.25,"weightKg":0.2}`
  - row 5: `{"barterNote":"Rises when the fallout maps update. The taste is metallic and leaves a film on the tongue. One pill a day keeps the thyroid at bay.","basePrice":25.0,"category":"medical","displayName":"Iodine Pills","elasticity":1.3,"id":"iodine_pills","stackSize":10,"volatility":0.2,"weightKg":0.1}`
  - row 6: `{"barterNote":"The generators drink it; so does the market. A full can means warmth through the night. An empty can means a long, cold watch.","basePrice":20.0,"category":"fuel","displayName":"Fuel","elasticity":1.4,"id":"fuel","stackSize":4,"volatility":0.18,"weightKg":3.0}`
  - row 7: `{"barterNote":"Ammo holds value better than most promises. The brass is tarnished, the powder smells old. But a bullet is a bullet when the lights go out.","basePrice":12.0,"category":"ammo","displayName":"9mm Ammunition","elasticity":1.2,"id":"9mm_ammo","stackSize":50,"volatility":0.2,"weightKg":0.5}`
  - row 8: `{"barterNote":"Tools are inelastic: scarce on supply, steady on price. The prying end is polished bright from use. It opens doors, crates, and arguments.","basePrice":18.0,"category":"tools","displayName":"Crowbar","elasticity":0.6,"id":"crowbar","stackSize":1,"volatility":0.08,"weightKg":2.5}`
  - row 9: `{"barterNote":"Storm weather moves this faster than any argument. The straps are frayed, the filter is questionable. But it's better than breathing in whatever's out there.","basePrice":60.0,"category":"tools","displayName":"Gas Mask","elasticity":1.0,"id":"gas_mask","stackSize":1,"volatility":0.15,"weightKg":1.8}`
  - row 10: `{"barterNote":"The registrar's pen; calibration keeps its price honest. The needle jumps like a spooked animal. That's never a good sign.","basePrice":80.0,"category":"tools","displayName":"Dosimeter","elasticity":0.9,"id":"dosimeter","stackSize":1,"volatility":0.12,"weightKg":0.4}`
  - row 11: `{"barterNote":"Food spikes hardest in the shortage windows. The label claims 'balanced nutrition.' The contents suggest it's been balanced with something else entirely.","basePrice":14.0,"category":"food","displayName":"Canned Food","elasticity":1.5,"id":"canned_food","regionalSupply":"greenhouse","stackSize":6,"volatility":0.18,"weightKg":0.8}`
  - row 12: `{"barterNote":"Luxury softens when supplies run short — nobody trades shine for thirst. The cut diamond in a velvet box is worth more than the bunker's generator.","basePrice":150.0,"category":"luxury","displayName":"Diamond","elasticity":0.3,"id":"diamond","stackSize":1,"volatility":0.05,"weightKg":0.01}`
  - row 13: `{"barterNote":"Cupola fuel and the ice road's payload; scarce when the window closes. The chunks are black and heavy, leaving dust on everything they touch.","basePrice":12.0,"category":"fuel","displayName":"Coal","elasticity":1.1,"id":"coal","stackSize":10,"volatility":0.15,"weightKg":1.0}`
  - row 14: `{"barterNote":"Lead-antimony pipe owed to the membrane hall under the Brine Pipe Exchange. The metal is worth more than the pipe itself. The pipe is worth more than your life.","basePrice":70.0,"category":"materials","displayName":"Brine-Resistant Pipe","elasticity":1.2,"id":"item_foundry_brine_pipe","stackSize":5,"volatility":0.1,"weightKg":12.0}`
  - row 15: `{"barterNote":"Counted by the bundle; the road charter's quota good. A handful can save a life—or end one. The ice doesn't care which.","basePrice":2.0,"category":"materials","displayName":"Ice Anchor Spike","elasticity":0.8,"id":"item_foundry_ice_anchor","stackSize":50,"volatility":0.1,"weightKg":0.5}`
  - row 16: `{"barterNote":"Chilled-flange drums for the barge windlass and Berth 9. The weight is the value. The drum is the difference between a working barge and a sunken one.","basePrice":90.0,"category":"materials","displayName":"Winch Drum","elasticity":1.0,"id":"item_foundry_winch_drum","stackSize":5,"volatility":0.1,"weightKg":75.0}`
  - row 17: `{"barterNote":"The wasteland's only reliable luxury. A slab of cooked meat at the trade table makes the room go quiet. People who haven't eaten it in months will trade things they swore they would never trade.","basePrice":18.0,"category":"food","displayName":"Cooked Meat","elasticity":1.6,"id":"cooked_meat","regionalSupply":"traplines","stackSize":5,"volatility":0.2,"weightKg":0.5}`
  - row 18: `{"barterNote":"The ceramic heart of the purifier. A bunker with a spare filter is a bunker with a future. The element is good for a season, then it trades down to the desperate at half price.","basePrice":35.0,"category":"tools","displayName":"Water Filter","elasticity":1.1,"id":"water_filter","regionalSupply":"foundry","stackSize":5,"volatility":0.12,"weightKg":0.5}`
  - row 19: `{"barterNote":"The bunker's lungs. Demand spikes with every fallout storm. The paper is three years old and brittler than the people who depend on it.","basePrice":30.0,"category":"tools","displayName":"Air Filter","elasticity":1.3,"id":"air_filter","regionalSupply":"foundry","stackSize":5,"volatility":0.22,"weightKg":1.0}`
  - row 20: `{"barterNote":"Rendered fat and wintergreen oil in a tin. It stops the black from spreading. In the deep freeze, a tin of salve is worth more than a pistol.","basePrice":22.0,"category":"medical","displayName":"Frostbite Salve","elasticity":1.4,"id":"item_frostbite_salve","regionalSupply":"traplines","stackSize":5,"volatility":0.25,"weightKg":0.15}`
  - row 21: `{"barterNote":"Sealed foil packets from the last agricultural station. Kale, potato eyes, and the beans that grow in cardboard. A single packet is the difference between a greenhouse and a cold room. Caravans that carry these are guarded like ambulances.","basePrice":45.0,"category":"food","displayName":"Greenhouse Seeds","elasticity":0.7,"id":"seed_packets","regionalSupply":"greenhouse","stackSize":10,"volatility":0.1,"weightKg":0.1}`
  - row 22: `{"barterNote":"Acids, solvents, powders in unlabeled jars. The foundry district runs on them. Handle with care — the jars have no labels, and the smell tells you what the label would have said.","basePrice":8.0,"category":"materials","displayName":"Industrial Chemicals","elasticity":0.9,"id":"chemicals","regionalSupply":"foundry","stackSize":30,"volatility":0.12,"weightKg":0.25}`
  - row 23: `{"barterNote":"Circuit boards, wiring, chips. The gold pins still shine. Some of it is worth nothing; some of it is a radio waiting for a soldering iron. People sort it by hand, in the evenings.","basePrice":10.0,"category":"materials","displayName":"Electronic Scrap","elasticity":1.0,"id":"electronic_scrap","regionalSupply":"foundry","stackSize":50,"volatility":0.15,"weightKg":0.1}`
  - row 24: `{"barterNote":"Gears, shafts, bearings in a greasy bag. They rebuild pumps, generators, latches. The wasteland runs on salvage, and this is what salvage looks like before it becomes a tool.","basePrice":5.0,"category":"materials","displayName":"Mechanical Parts","elasticity":0.8,"id":"mechanical_parts","regionalSupply":"foundry","stackSize":50,"volatility":0.1,"weightKg":0.15}`
  - row 25: `{"barterNote":"The sun still does its part. A charged cell feeds a battery; a broken one is still the best glass around. First pick, first price, no haggling — the only utility left that hasn't failed.","basePrice":55.0,"category":"tools","displayName":"Solar Cell","elasticity":0.5,"id":"solar_cell","regionalSupply":"settlement","stackSize":10,"volatility":0.06,"weightKg":1.2}`
  - row 26: `{"barterNote":"A canvas kit with rolled bandages, tape, scissors and antiseptic. What a bunker calls a clinic. People who carry one walk a little straighter, and everyone notices.","basePrice":50.0,"category":"medical","displayName":"Medical Kit","elasticity":1.5,"id":"medical_kit","regionalSupply":"settlement","stackSize":5,"volatility":0.2,"weightKg":0.5}`
  - row 27: `{"barterNote":"Decorporation medication in foil. It pulls accumulated dose out of the body — not a cure, a deduction. Pharmacies that still stand trade it only for things they cannot scavenge. People argue over the last pack like it is a promise.","basePrice":65.0,"category":"medical","displayName":"Anti-Rad","elasticity":1.6,"id":"anti_rad","regionalSupply":"settlement","stackSize":5,"volatility":0.28,"weightKg":0.1}`
  - row 28: `{"barterNote":"A polymer membrane sheet from the water treatment plant. It turns brine into drinking water under pressure. The flotilla settlements trade these at a premium — they are the difference between a working barge and a stranded one.","basePrice":85.0,"category":"materials","displayName":"Desalination Membrane","elasticity":0.7,"id":"item_desal_membrane","regionalSupply":"flotilla","stackSize":3,"volatility":0.08,"weightKg":0.8}`
  - row 29: `{"barterNote":"Deep-blue decorporation pellets in a sealed medical vial. They bind cesium and thallium in the gut. Pre-war pharmaceutical stock — the last production run was the year the exchange stopped everything. Each vial is a non-renewable resource.","basePrice":95.0,"category":"medical","displayName":"Prussian Blue Pellets","elasticity":1.2,"id":"item_prussian_blue_chelating_pellets","regionalSupply":"settlement","stackSize":5,"volatility":0.15,"weightKg":0.08}`
  - row 30: `{"barterNote":"A small lead-lined cask with a threaded lid, used for transporting hot samples. Science teams trade these like currency. The lead is the value; the shielding is the purpose. Without it, no one can study what the ground has become.","basePrice":120.0,"category":"tools","displayName":"Lead-Shielded Sample Cask","elasticity":0.4,"id":"item_lead_shielded_sample_cask","regionalSupply":"settlement","stackSize":1,"volatility":0.05,"weightKg":4.5}`
  - row 31: `{"barterNote":"A spiral-wound RO element. The heart of the advanced water purifier. Foundry district workshops rebuild these from salvaged housing and new membrane sheets. The flotilla pays double — their stills run on these.","basePrice":75.0,"category":"materials","displayName":"Reverse-Osmosis Membrane","elasticity":0.8,"id":"item_ro_membrane","regionalSupply":"foundry","stackSize":3,"volatility":0.1,"weightKg":0.6}`
  - row 32: `{"barterNote":"A twist of copper wire shaped into a loop. The cheapest passive catch a survivor can make. Breaks often, catches little, costs almost nothing. Every scavenger has set one at least once.","basePrice":10.0,"category":"tools","displayName":"Improvised Wire Snare","elasticity":0.6,"id":"trap_improvised_wire","regionalSupply":"general","stackSize":5,"volatility":0.1,"weightKg":0.2}`
  - row 33: `{"barterNote":"A wooden box with a gravity door and a treadle trigger. Dependable, reusable, and bulky. Workshop hunters build these from salvaged lumber and scrap metal. The animal lives inside it, which is either mercy or a problem.","basePrice":22.0,"category":"tools","displayName":"Box Trap","elasticity":0.9,"id":"trap_box","regionalSupply":"settlement","stackSize":3,"volatility":0.15,"weightKg":4.0}`
  - row 34: `{"barterNote":"A woven funnel of wood and cord set in moving water. Fish enter but cannot find the exit. Strong where geography supports it, worthless where it does not. The freeze kills it. The thaw brings it back.","basePrice":16.0,"category":"tools","displayName":"Fish Trap","elasticity":1.0,"id":"trap_fish","regionalSupply":"coastal","stackSize":3,"volatility":0.2,"weightKg":2.5}`
  - row 35: `{"barterNote":"Rifle rounds ride behind the frontline convoys; the price climbs with every skirmish report.","basePrice":18.0,"category":"ammo","displayName":"5.56mm Rounds","elasticity":1.2,"id":"ammo_556","stackSize":20,"volatility":0.22,"weightKg":0.2}`
  - row 36: `{"barterNote":"Shell bags trade steady at the strongholds and slow everywhere else.","basePrice":10.0,"category":"ammo","displayName":"12-Gauge Shells","elasticity":1.2,"id":"ammo_12g","stackSize":20,"volatility":0.2,"weightKg":0.25}`
  - row 37: `{"barterNote":"Rail sidings and river barges burn it by the drum; the price follows the timetable.","basePrice":16.0,"category":"fuel","displayName":"Diesel Fuel","elasticity":1.3,"id":"diesel_fuel","stackSize":4,"volatility":0.16,"weightKg":2.5}`
  - row 38: `{"barterNote":"Keeps for weeks without ice, so it sells wherever the next meal is uncertain.","basePrice":20.0,"category":"food","displayName":"Hardwood Smoked Rations","elasticity":1.4,"id":"item_smoked_meat","stackSize":8,"volatility":0.16,"weightKg":0.3}`
  - row 39: `{"barterNote":"Sealed jars of brine-cured greenhouse produce; the salt camps never run short of buyers.","basePrice":11.0,"category":"food","displayName":"Pickled Greenhouse Tubers","elasticity":1.3,"id":"item_pickled_tubers","stackSize":10,"volatility":0.12,"weightKg":0.4}`
  - row 40: `{"barterNote":"Trades far above its weight wherever a garrison has been paid recently.","basePrice":30.0,"category":"luxury","displayName":"Tobacco Pouch","elasticity":1.8,"id":"tobacco_pouch","regionalSupply":"settlement","stackSize":10,"volatility":0.35,"weightKg":0.2}`
  - row 41: `{"barterNote":"Encrypted warehousing codes sell to whoever has trucks and no scruples. The sheet outlives the handshake.","basePrice":60.0,"category":"documents","displayName":"Logistics Cipher Sheet","elasticity":1.8,"id":"item_logistics_cipher_sheet","regionalSupply":"flotilla","stackSize":2,"volatility":0.35,"weightKg":0.05}`
  - row 42: `{"barterNote":"Nobody reads them and everybody wants them sealed. Bureaucracy is the one crop that never froze.","basePrice":11.0,"category":"documents","displayName":"Sealed Government Document","elasticity":1.1,"id":"sealed_government_document","regionalSupply":"general","stackSize":5,"volatility":0.1,"weightKg":0.3}`
  - row 43: `{"barterNote":"Garrisons pay the rack price without haggling. Sidearms hold value because nobody admits wanting one.","basePrice":35.0,"category":"weapons","displayName":"Duty Sidearm","elasticity":0.9,"id":"weapon_sidearm","regionalSupply":"settlement","stackSize":1,"volatility":0.15,"weightKg":1.0}`
  - row 44: `{"barterNote":"Two pipes and a belief in tomorrow. Sells at dusk in camps that heard something outside the wire.","basePrice":20.0,"category":"weapons","displayName":"Pipe Shotgun","elasticity":1.1,"id":"weapon_pipe_shotgun","regionalSupply":"settlement","stackSize":1,"volatility":0.18,"weightKg":2.5}`
  - row 45: `{"barterNote":"Traded in whispers and wrapped in the same cloth as the bandages. The price doubles wherever the mines are working.","basePrice":25.0,"category":"contraband","displayName":"Opioid Taper Kit","elasticity":1.4,"id":"item_taper_kit_opioid","regionalSupply":"flotilla","stackSize":2,"volatility":0.3,"weightKg":0.2}`
  - row 46: `{"barterNote":"Still unsticks from itself, which puts it ahead of most promises. Every repair job starts and ends with it.","basePrice":6.0,"category":"misc","displayName":"Duct Tape","elasticity":0.7,"id":"duct_tape","regionalSupply":"general","stackSize":15,"volatility":0.08,"weightKg":0.3}`
  - row 47: `{"barterNote":"Ten metres of three-strand hemp. Quarries and lighthouses measure rope in lives, so they never haggle hard.","basePrice":8.0,"category":"misc","displayName":"Hemp Rope (10 m)","elasticity":0.8,"id":"rope","regionalSupply":"general","stackSize":6,"volatility":0.1,"weightKg":2.0}`
  - row 48: `{"barterNote":"Nobody admits what's on it. Music, a sermon, somebody's last recorded voice — morale is whatever plays.","basePrice":9.0,"category":"misc","displayName":"Cassette Tape","elasticity":1.6,"id":"item_cassette_tape","regionalSupply":"settlement","stackSize":8,"volatility":0.25,"weightKg":0.15}`
  - row 49: `{"barterNote":"Shifted in bulk from the factories before the clouds came. Patch a suit, wrap a wound, filter water in a pinch. Never worth much by the bolt; priceless by the strip.","basePrice":3.0,"category":"materials","displayName":"Cloth","elasticity":1.0,"id":"cloth","regionalSupply":"settlement","stackSize":20,"volatility":0.2,"weightKg":0.2}`
  - row 50: `{"barterNote":"The bunker breathes through these. A sealed cartridge trades at the price of the seller's own remaining air.","basePrice":30.0,"category":"tools","displayName":"HEPA Filter Cartridge","elasticity":0.9,"id":"item_air_filter_hepa","regionalSupply":"foundry","stackSize":4,"volatility":0.25,"weightKg":1.5}`
  - row 51: `{"barterNote":"Dense, oily seeds from the greenhouse sun-flax beds. Presses into clean cooking oil; the press-cake bolsters flour rations.","basePrice":10.0,"category":"materials","displayName":"Harvested Sun-Flax","elasticity":0.9,"id":"crop_oilseed","regionalSupply":"settlement","stackSize":25,"volatility":0.15,"weightKg":0.15}`
- Bytes: 21,607; SHA-256: `18c8a68c9479179b68557cc28bc9fbce71a988e1d5c2a130c9bcf47c20a0739b`
- Root keys: `goods, schema_version`
- `goods`: list[51]; union fields: `barterNote, basePrice, category, displayName, elasticity, id, regionalSupply, stackSize, volatility, weightKg`
  - row 1: `{"barterNote":"The universal measure. Every faction prices its offers against the litre. A single bottle can buy a night's silence—or a bullet.","basePrice":8.0,"category":"water","displayName":"Clean Water","elasticity":1.2,"id":"clean_water","stackSize":5,"volatility":0.15,"weightKg":1.0}`
  - row 2: `{"barterNote":"Tier-zero scrap; the fallback value everything else is compared against. A handful can patch a hole in the bunker wall—or a hole in a man.","basePrice":3.0,"category":"materials","displayName":"Scrap Metal","elasticity":0.8,"id":"scrap_metal","stackSize":20,"volatility":0.1,"weightKg":2.0}`
  - row 3: `{"barterNote":"Shortage-spiked when supplies run thin. The white cloth turns pink before it turns red. The antiseptic stings worse than the wound.","basePrice":15.0,"category":"medical","displayName":"Bandages","elasticity":1.5,"id":"bandages","stackSize":10,"volatility":0.2,"weightKg":0.2}`
  - row 4: `{"barterNote":"The medicine tier anchor; the sick room's price follows it. The pills are bitter, the cure is worse than the disease. But it works.","basePrice":40.0,"category":"medical","displayName":"Antibiotics","elasticity":1.5,"id":"antibiotics","stackSize":5,"volatility":0.25,"weightKg":0.2}`
  - row 5: `{"barterNote":"Rises when the fallout maps update. The taste is metallic and leaves a film on the tongue. One pill a day keeps the thyroid at bay.","basePrice":25.0,"category":"medical","displayName":"Iodine Pills","elasticity":1.3,"id":"iodine_pills","stackSize":10,"volatility":0.2,"weightKg":0.1}`
  - row 6: `{"barterNote":"The generators drink it; so does the market. A full can means warmth through the night. An empty can means a long, cold watch.","basePrice":20.0,"category":"fuel","displayName":"Fuel","elasticity":1.4,"id":"fuel","stackSize":4,"volatility":0.18,"weightKg":3.0}`
  - row 7: `{"barterNote":"Ammo holds value better than most promises. The brass is tarnished, the powder smells old. But a bullet is a bullet when the lights go out.","basePrice":12.0,"category":"ammo","displayName":"9mm Ammunition","elasticity":1.2,"id":"9mm_ammo","stackSize":50,"volatility":0.2,"weightKg":0.5}`
  - row 8: `{"barterNote":"Tools are inelastic: scarce on supply, steady on price. The prying end is polished bright from use. It opens doors, crates, and arguments.","basePrice":18.0,"category":"tools","displayName":"Crowbar","elasticity":0.6,"id":"crowbar","stackSize":1,"volatility":0.08,"weightKg":2.5}`
  - row 9: `{"barterNote":"Storm weather moves this faster than any argument. The straps are frayed, the filter is questionable. But it's better than breathing in whatever's out there.","basePrice":60.0,"category":"tools","displayName":"Gas Mask","elasticity":1.0,"id":"gas_mask","stackSize":1,"volatility":0.15,"weightKg":1.8}`
  - row 10: `{"barterNote":"The registrar's pen; calibration keeps its price honest. The needle jumps like a spooked animal. That's never a good sign.","basePrice":80.0,"category":"tools","displayName":"Dosimeter","elasticity":0.9,"id":"dosimeter","stackSize":1,"volatility":0.12,"weightKg":0.4}`
  - row 11: `{"barterNote":"Food spikes hardest in the shortage windows. The label claims 'balanced nutrition.' The contents suggest it's been balanced with something else entirely.","basePrice":14.0,"category":"food","displayName":"Canned Food","elasticity":1.5,"id":"canned_food","regionalSupply":"greenhouse","stackSize":6,"volatility":0.18,"weightKg":0.8}`
  - row 12: `{"barterNote":"Luxury softens when supplies run short — nobody trades shine for thirst. The cut diamond in a velvet box is worth more than the bunker's generator.","basePrice":150.0,"category":"luxury","displayName":"Diamond","elasticity":0.3,"id":"diamond","stackSize":1,"volatility":0.05,"weightKg":0.01}`
  - row 13: `{"barterNote":"Cupola fuel and the ice road's payload; scarce when the window closes. The chunks are black and heavy, leaving dust on everything they touch.","basePrice":12.0,"category":"fuel","displayName":"Coal","elasticity":1.1,"id":"coal","stackSize":10,"volatility":0.15,"weightKg":1.0}`
  - row 14: `{"barterNote":"Lead-antimony pipe owed to the membrane hall under the Brine Pipe Exchange. The metal is worth more than the pipe itself. The pipe is worth more than your life.","basePrice":70.0,"category":"materials","displayName":"Brine-Resistant Pipe","elasticity":1.2,"id":"item_foundry_brine_pipe","stackSize":5,"volatility":0.1,"weightKg":12.0}`
  - row 15: `{"barterNote":"Counted by the bundle; the road charter's quota good. A handful can save a life—or end one. The ice doesn't care which.","basePrice":2.0,"category":"materials","displayName":"Ice Anchor Spike","elasticity":0.8,"id":"item_foundry_ice_anchor","stackSize":50,"volatility":0.1,"weightKg":0.5}`
  - row 16: `{"barterNote":"Chilled-flange drums for the barge windlass and Berth 9. The weight is the value. The drum is the difference between a working barge and a sunken one.","basePrice":90.0,"category":"materials","displayName":"Winch Drum","elasticity":1.0,"id":"item_foundry_winch_drum","stackSize":5,"volatility":0.1,"weightKg":75.0}`
  - row 17: `{"barterNote":"The wasteland's only reliable luxury. A slab of cooked meat at the trade table makes the room go quiet. People who haven't eaten it in months will trade things they swore they would never trade.","basePrice":18.0,"category":"food","displayName":"Cooked Meat","elasticity":1.6,"id":"cooked_meat","regionalSupply":"traplines","stackSize":5,"volatility":0.2,"weightKg":0.5}`
  - row 18: `{"barterNote":"The ceramic heart of the purifier. A bunker with a spare filter is a bunker with a future. The element is good for a season, then it trades down to the desperate at half price.","basePrice":35.0,"category":"tools","displayName":"Water Filter","elasticity":1.1,"id":"water_filter","regionalSupply":"foundry","stackSize":5,"volatility":0.12,"weightKg":0.5}`
  - row 19: `{"barterNote":"The bunker's lungs. Demand spikes with every fallout storm. The paper is three years old and brittler than the people who depend on it.","basePrice":30.0,"category":"tools","displayName":"Air Filter","elasticity":1.3,"id":"air_filter","regionalSupply":"foundry","stackSize":5,"volatility":0.22,"weightKg":1.0}`
  - row 20: `{"barterNote":"Rendered fat and wintergreen oil in a tin. It stops the black from spreading. In the deep freeze, a tin of salve is worth more than a pistol.","basePrice":22.0,"category":"medical","displayName":"Frostbite Salve","elasticity":1.4,"id":"item_frostbite_salve","regionalSupply":"traplines","stackSize":5,"volatility":0.25,"weightKg":0.15}`
  - row 21: `{"barterNote":"Sealed foil packets from the last agricultural station. Kale, potato eyes, and the beans that grow in cardboard. A single packet is the difference between a greenhouse and a cold room. Caravans that carry these are guarded like ambulances.","basePrice":45.0,"category":"food","displayName":"Greenhouse Seeds","elasticity":0.7,"id":"seed_packets","regionalSupply":"greenhouse","stackSize":10,"volatility":0.1,"weightKg":0.1}`
  - row 22: `{"barterNote":"Acids, solvents, powders in unlabeled jars. The foundry district runs on them. Handle with care — the jars have no labels, and the smell tells you what the label would have said.","basePrice":8.0,"category":"materials","displayName":"Industrial Chemicals","elasticity":0.9,"id":"chemicals","regionalSupply":"foundry","stackSize":30,"volatility":0.12,"weightKg":0.25}`
  - row 23: `{"barterNote":"Circuit boards, wiring, chips. The gold pins still shine. Some of it is worth nothing; some of it is a radio waiting for a soldering iron. People sort it by hand, in the evenings.","basePrice":10.0,"category":"materials","displayName":"Electronic Scrap","elasticity":1.0,"id":"electronic_scrap","regionalSupply":"foundry","stackSize":50,"volatility":0.15,"weightKg":0.1}`
  - row 24: `{"barterNote":"Gears, shafts, bearings in a greasy bag. They rebuild pumps, generators, latches. The wasteland runs on salvage, and this is what salvage looks like before it becomes a tool.","basePrice":5.0,"category":"materials","displayName":"Mechanical Parts","elasticity":0.8,"id":"mechanical_parts","regionalSupply":"foundry","stackSize":50,"volatility":0.1,"weightKg":0.15}`
  - row 25: `{"barterNote":"The sun still does its part. A charged cell feeds a battery; a broken one is still the best glass around. First pick, first price, no haggling — the only utility left that hasn't failed.","basePrice":55.0,"category":"tools","displayName":"Solar Cell","elasticity":0.5,"id":"solar_cell","regionalSupply":"settlement","stackSize":10,"volatility":0.06,"weightKg":1.2}`
  - row 26: `{"barterNote":"A canvas kit with rolled bandages, tape, scissors and antiseptic. What a bunker calls a clinic. People who carry one walk a little straighter, and everyone notices.","basePrice":50.0,"category":"medical","displayName":"Medical Kit","elasticity":1.5,"id":"medical_kit","regionalSupply":"settlement","stackSize":5,"volatility":0.2,"weightKg":0.5}`
  - row 27: `{"barterNote":"Decorporation medication in foil. It pulls accumulated dose out of the body — not a cure, a deduction. Pharmacies that still stand trade it only for things they cannot scavenge. People argue over the last pack like it is a promise.","basePrice":65.0,"category":"medical","displayName":"Anti-Rad","elasticity":1.6,"id":"anti_rad","regionalSupply":"settlement","stackSize":5,"volatility":0.28,"weightKg":0.1}`
  - row 28: `{"barterNote":"A polymer membrane sheet from the water treatment plant. It turns brine into drinking water under pressure. The flotilla settlements trade these at a premium — they are the difference between a working barge and a stranded one.","basePrice":85.0,"category":"materials","displayName":"Desalination Membrane","elasticity":0.7,"id":"item_desal_membrane","regionalSupply":"flotilla","stackSize":3,"volatility":0.08,"weightKg":0.8}`
  - row 29: `{"barterNote":"Deep-blue decorporation pellets in a sealed medical vial. They bind cesium and thallium in the gut. Pre-war pharmaceutical stock — the last production run was the year the exchange stopped everything. Each vial is a non-renewable resource.","basePrice":95.0,"category":"medical","displayName":"Prussian Blue Pellets","elasticity":1.2,"id":"item_prussian_blue_chelating_pellets","regionalSupply":"settlement","stackSize":5,"volatility":0.15,"weightKg":0.08}`
  - row 30: `{"barterNote":"A small lead-lined cask with a threaded lid, used for transporting hot samples. Science teams trade these like currency. The lead is the value; the shielding is the purpose. Without it, no one can study what the ground has become.","basePrice":120.0,"category":"tools","displayName":"Lead-Shielded Sample Cask","elasticity":0.4,"id":"item_lead_shielded_sample_cask","regionalSupply":"settlement","stackSize":1,"volatility":0.05,"weightKg":4.5}`
  - row 31: `{"barterNote":"A spiral-wound RO element. The heart of the advanced water purifier. Foundry district workshops rebuild these from salvaged housing and new membrane sheets. The flotilla pays double — their stills run on these.","basePrice":75.0,"category":"materials","displayName":"Reverse-Osmosis Membrane","elasticity":0.8,"id":"item_ro_membrane","regionalSupply":"foundry","stackSize":3,"volatility":0.1,"weightKg":0.6}`
  - row 32: `{"barterNote":"A twist of copper wire shaped into a loop. The cheapest passive catch a survivor can make. Breaks often, catches little, costs almost nothing. Every scavenger has set one at least once.","basePrice":10.0,"category":"tools","displayName":"Improvised Wire Snare","elasticity":0.6,"id":"trap_improvised_wire","regionalSupply":"general","stackSize":5,"volatility":0.1,"weightKg":0.2}`
  - row 33: `{"barterNote":"A wooden box with a gravity door and a treadle trigger. Dependable, reusable, and bulky. Workshop hunters build these from salvaged lumber and scrap metal. The animal lives inside it, which is either mercy or a problem.","basePrice":22.0,"category":"tools","displayName":"Box Trap","elasticity":0.9,"id":"trap_box","regionalSupply":"settlement","stackSize":3,"volatility":0.15,"weightKg":4.0}`
  - row 34: `{"barterNote":"A woven funnel of wood and cord set in moving water. Fish enter but cannot find the exit. Strong where geography supports it, worthless where it does not. The freeze kills it. The thaw brings it back.","basePrice":16.0,"category":"tools","displayName":"Fish Trap","elasticity":1.0,"id":"trap_fish","regionalSupply":"coastal","stackSize":3,"volatility":0.2,"weightKg":2.5}`
  - row 35: `{"barterNote":"Rifle rounds ride behind the frontline convoys; the price climbs with every skirmish report.","basePrice":18.0,"category":"ammo","displayName":"5.56mm Rounds","elasticity":1.2,"id":"ammo_556","stackSize":20,"volatility":0.22,"weightKg":0.2}`
  - row 36: `{"barterNote":"Shell bags trade steady at the strongholds and slow everywhere else.","basePrice":10.0,"category":"ammo","displayName":"12-Gauge Shells","elasticity":1.2,"id":"ammo_12g","stackSize":20,"volatility":0.2,"weightKg":0.25}`
  - row 37: `{"barterNote":"Rail sidings and river barges burn it by the drum; the price follows the timetable.","basePrice":16.0,"category":"fuel","displayName":"Diesel Fuel","elasticity":1.3,"id":"diesel_fuel","stackSize":4,"volatility":0.16,"weightKg":2.5}`
  - row 38: `{"barterNote":"Keeps for weeks without ice, so it sells wherever the next meal is uncertain.","basePrice":20.0,"category":"food","displayName":"Hardwood Smoked Rations","elasticity":1.4,"id":"item_smoked_meat","stackSize":8,"volatility":0.16,"weightKg":0.3}`
  - row 39: `{"barterNote":"Sealed jars of brine-cured greenhouse produce; the salt camps never run short of buyers.","basePrice":11.0,"category":"food","displayName":"Pickled Greenhouse Tubers","elasticity":1.3,"id":"item_pickled_tubers","stackSize":10,"volatility":0.12,"weightKg":0.4}`
  - row 40: `{"barterNote":"Trades far above its weight wherever a garrison has been paid recently.","basePrice":30.0,"category":"luxury","displayName":"Tobacco Pouch","elasticity":1.8,"id":"tobacco_pouch","regionalSupply":"settlement","stackSize":10,"volatility":0.35,"weightKg":0.2}`
  - row 41: `{"barterNote":"Encrypted warehousing codes sell to whoever has trucks and no scruples. The sheet outlives the handshake.","basePrice":60.0,"category":"documents","displayName":"Logistics Cipher Sheet","elasticity":1.8,"id":"item_logistics_cipher_sheet","regionalSupply":"flotilla","stackSize":2,"volatility":0.35,"weightKg":0.05}`
  - row 42: `{"barterNote":"Nobody reads them and everybody wants them sealed. Bureaucracy is the one crop that never froze.","basePrice":11.0,"category":"documents","displayName":"Sealed Government Document","elasticity":1.1,"id":"sealed_government_document","regionalSupply":"general","stackSize":5,"volatility":0.1,"weightKg":0.3}`
  - row 43: `{"barterNote":"Garrisons pay the rack price without haggling. Sidearms hold value because nobody admits wanting one.","basePrice":35.0,"category":"weapons","displayName":"Duty Sidearm","elasticity":0.9,"id":"weapon_sidearm","regionalSupply":"settlement","stackSize":1,"volatility":0.15,"weightKg":1.0}`
  - row 44: `{"barterNote":"Two pipes and a belief in tomorrow. Sells at dusk in camps that heard something outside the wire.","basePrice":20.0,"category":"weapons","displayName":"Pipe Shotgun","elasticity":1.1,"id":"weapon_pipe_shotgun","regionalSupply":"settlement","stackSize":1,"volatility":0.18,"weightKg":2.5}`
  - row 45: `{"barterNote":"Traded in whispers and wrapped in the same cloth as the bandages. The price doubles wherever the mines are working.","basePrice":25.0,"category":"contraband","displayName":"Opioid Taper Kit","elasticity":1.4,"id":"item_taper_kit_opioid","regionalSupply":"flotilla","stackSize":2,"volatility":0.3,"weightKg":0.2}`
  - row 46: `{"barterNote":"Still unsticks from itself, which puts it ahead of most promises. Every repair job starts and ends with it.","basePrice":6.0,"category":"misc","displayName":"Duct Tape","elasticity":0.7,"id":"duct_tape","regionalSupply":"general","stackSize":15,"volatility":0.08,"weightKg":0.3}`
  - row 47: `{"barterNote":"Ten metres of three-strand hemp. Quarries and lighthouses measure rope in lives, so they never haggle hard.","basePrice":8.0,"category":"misc","displayName":"Hemp Rope (10 m)","elasticity":0.8,"id":"rope","regionalSupply":"general","stackSize":6,"volatility":0.1,"weightKg":2.0}`
  - row 48: `{"barterNote":"Nobody admits what's on it. Music, a sermon, somebody's last recorded voice — morale is whatever plays.","basePrice":9.0,"category":"misc","displayName":"Cassette Tape","elasticity":1.6,"id":"item_cassette_tape","regionalSupply":"settlement","stackSize":8,"volatility":0.25,"weightKg":0.15}`
  - row 49: `{"barterNote":"Shifted in bulk from the factories before the clouds came. Patch a suit, wrap a wound, filter water in a pinch. Never worth much by the bolt; priceless by the strip.","basePrice":3.0,"category":"materials","displayName":"Cloth","elasticity":1.0,"id":"cloth","regionalSupply":"settlement","stackSize":20,"volatility":0.2,"weightKg":0.2}`
  - row 50: `{"barterNote":"The bunker breathes through these. A sealed cartridge trades at the price of the seller's own remaining air.","basePrice":30.0,"category":"tools","displayName":"HEPA Filter Cartridge","elasticity":0.9,"id":"item_air_filter_hepa","regionalSupply":"foundry","stackSize":4,"volatility":0.25,"weightKg":1.5}`
  - row 51: `{"barterNote":"Dense, oily seeds from the greenhouse sun-flax beds. Presses into clean cooking oil; the press-cake bolsters flour rations.","basePrice":10.0,"category":"materials","displayName":"Harvested Sun-Flax","elasticity":0.9,"id":"crop_oilseed","regionalSupply":"settlement","stackSize":25,"volatility":0.15,"weightKg":0.15}`

## `Assets/StreamingAssets/Data/regional_prices.json`
- Bytes: 3,789; SHA-256: `5dee33fbf091fdaae985b322764f32d6c2693c2a4da2a1486d0491eff4d48860`
- Root keys: `collection_id, description, entries, schema_version`
- `entries`: list[24]; union fields: `base_price_modifier_permille, category, item_id, region, scarcity_profile`
  - row 1: `{"base_price_modifier_permille":700,"item_id":"item_foundry_brine_pipe","region":"flotilla","scarcity_profile":"local_surplus"}`
  - row 2: `{"base_price_modifier_permille":700,"item_id":"item_desal_membrane","region":"flotilla","scarcity_profile":"local_surplus"}`
  - row 3: `{"base_price_modifier_permille":700,"item_id":"item_ro_membrane","region":"flotilla","scarcity_profile":"local_surplus"}`
  - row 4: `{"base_price_modifier_permille":1500,"item_id":"seed_packets","region":"flotilla","scarcity_profile":"imported_scarce"}`
  - row 5: `{"base_price_modifier_permille":800,"item_id":"trap_fish","region":"flotilla","scarcity_profile":"local_surplus"}`
  - row 6: `{"base_price_modifier_permille":800,"item_id":"mechanical_parts","region":"foundry","scarcity_profile":"local_surplus"}`
  - row 7: `{"base_price_modifier_permille":800,"item_id":"electronic_scrap","region":"foundry","scarcity_profile":"local_surplus"}`
  - row 8: `{"base_price_modifier_permille":800,"item_id":"chemicals","region":"foundry","scarcity_profile":"local_surplus"}`
  - row 9: `{"base_price_modifier_permille":1300,"category":"food","region":"foundry","scarcity_profile":"imported_scarce"}`
  - row 10: `{"base_price_modifier_permille":800,"item_id":"seed_packets","region":"greenhouse","scarcity_profile":"local_surplus"}`
  - row 11: `{"base_price_modifier_permille":800,"item_id":"canned_food","region":"greenhouse","scarcity_profile":"local_surplus"}`
  - row 12: `{"base_price_modifier_permille":1400,"category":"tools","region":"greenhouse","scarcity_profile":"imported_scarce"}`
  - row 13: `{"base_price_modifier_permille":700,"item_id":"cooked_meat","region":"traplines","scarcity_profile":"local_surplus"}`
  - row 14: `{"base_price_modifier_permille":700,"item_id":"item_frostbite_salve","region":"traplines","scarcity_profile":"local_surplus"}`
  - row 15: `{"base_price_modifier_permille":1500,"item_id":"electronic_scrap","region":"traplines","scarcity_profile":"imported_scarce"}`
  - row 16: `{"base_price_modifier_permille":900,"item_id":"medical_kit","region":"settlement","scarcity_profile":"balanced"}`
  - row 17: `{"base_price_modifier_permille":900,"item_id":"anti_rad","region":"settlement","scarcity_profile":"balanced"}`
  - row 18: `{"base_price_modifier_permille":900,"item_id":"solar_cell","region":"settlement","scarcity_profile":"balanced"}`
  - row 19: `{"base_price_modifier_permille":700,"item_id":"trap_fish","region":"coastal","scarcity_profile":"local_surplus"}`
  - row 20: `{"base_price_modifier_permille":850,"item_id":"clean_water","region":"coastal","scarcity_profile":"local_surplus"}`
  - row 21: `{"base_price_modifier_permille":1200,"category":"tools","region":"coastal","scarcity_profile":"imported_scarce"}`
  - row 22: `{"base_price_modifier_permille":1100,"item_id":"item_taper_kit_opioid","region":"flotilla","scarcity_profile":"balanced"}`
  - row 23: `{"base_price_modifier_permille":1100,"item_id":"tobacco_pouch","region":"settlement","scarcity_profile":"balanced"}`
  - row 24: `{"base_price_modifier_permille":850,"item_id":"trap_box","region":"traplines","scarcity_profile":"local_surplus"}`
- Bytes: 3,789; SHA-256: `5dee33fbf091fdaae985b322764f32d6c2693c2a4da2a1486d0491eff4d48860`
- Root keys: `collection_id, description, entries, schema_version`
- `entries`: list[24]; union fields: `base_price_modifier_permille, category, item_id, region, scarcity_profile`
  - row 1: `{"base_price_modifier_permille":700,"item_id":"item_foundry_brine_pipe","region":"flotilla","scarcity_profile":"local_surplus"}`
  - row 2: `{"base_price_modifier_permille":700,"item_id":"item_desal_membrane","region":"flotilla","scarcity_profile":"local_surplus"}`
  - row 3: `{"base_price_modifier_permille":700,"item_id":"item_ro_membrane","region":"flotilla","scarcity_profile":"local_surplus"}`
  - row 4: `{"base_price_modifier_permille":1500,"item_id":"seed_packets","region":"flotilla","scarcity_profile":"imported_scarce"}`
  - row 5: `{"base_price_modifier_permille":800,"item_id":"trap_fish","region":"flotilla","scarcity_profile":"local_surplus"}`
  - row 6: `{"base_price_modifier_permille":800,"item_id":"mechanical_parts","region":"foundry","scarcity_profile":"local_surplus"}`
  - row 7: `{"base_price_modifier_permille":800,"item_id":"electronic_scrap","region":"foundry","scarcity_profile":"local_surplus"}`
  - row 8: `{"base_price_modifier_permille":800,"item_id":"chemicals","region":"foundry","scarcity_profile":"local_surplus"}`
  - row 9: `{"base_price_modifier_permille":1300,"category":"food","region":"foundry","scarcity_profile":"imported_scarce"}`
  - row 10: `{"base_price_modifier_permille":800,"item_id":"seed_packets","region":"greenhouse","scarcity_profile":"local_surplus"}`
  - row 11: `{"base_price_modifier_permille":800,"item_id":"canned_food","region":"greenhouse","scarcity_profile":"local_surplus"}`
  - row 12: `{"base_price_modifier_permille":1400,"category":"tools","region":"greenhouse","scarcity_profile":"imported_scarce"}`
  - row 13: `{"base_price_modifier_permille":700,"item_id":"cooked_meat","region":"traplines","scarcity_profile":"local_surplus"}`
  - row 14: `{"base_price_modifier_permille":700,"item_id":"item_frostbite_salve","region":"traplines","scarcity_profile":"local_surplus"}`
  - row 15: `{"base_price_modifier_permille":1500,"item_id":"electronic_scrap","region":"traplines","scarcity_profile":"imported_scarce"}`
  - row 16: `{"base_price_modifier_permille":900,"item_id":"medical_kit","region":"settlement","scarcity_profile":"balanced"}`
  - row 17: `{"base_price_modifier_permille":900,"item_id":"anti_rad","region":"settlement","scarcity_profile":"balanced"}`
  - row 18: `{"base_price_modifier_permille":900,"item_id":"solar_cell","region":"settlement","scarcity_profile":"balanced"}`
  - row 19: `{"base_price_modifier_permille":700,"item_id":"trap_fish","region":"coastal","scarcity_profile":"local_surplus"}`
  - row 20: `{"base_price_modifier_permille":850,"item_id":"clean_water","region":"coastal","scarcity_profile":"local_surplus"}`
  - row 21: `{"base_price_modifier_permille":1200,"category":"tools","region":"coastal","scarcity_profile":"imported_scarce"}`
  - row 22: `{"base_price_modifier_permille":1100,"item_id":"item_taper_kit_opioid","region":"flotilla","scarcity_profile":"balanced"}`
  - row 23: `{"base_price_modifier_permille":1100,"item_id":"tobacco_pouch","region":"settlement","scarcity_profile":"balanced"}`
  - row 24: `{"base_price_modifier_permille":850,"item_id":"trap_box","region":"traplines","scarcity_profile":"local_surplus"}`

# Appendix D — Current caller/reference graph

### `TradeScreenScenarioLoader` (12 sampled current references)
- Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs:85: public static class TradeScreenScenarioLoader
- Ashfall.Core.Tests/TradeScreenSeamTests.cs:31: return TradeScreenScenarioLoader.LoadFromJson(ReadDataFile("trade_screen_scenarios.json"));
- Ashfall.Core.Tests/TradeScreenSeamTests.cs:73: var binding = TradeScreenScenarioLoader.CreateBinding(
- Ashfall.Core.Tests/TradeScreenSeamTests.cs:95: var binding = TradeScreenScenarioLoader.CreateBinding(
- Ashfall.Core.Tests/TradeScreenSeamTests.cs:112: var binding = TradeScreenScenarioLoader.CreateBinding(
- Ashfall.Core.Tests/TradeScreenSeamTests.cs:132: var binding = TradeScreenScenarioLoader.CreateBinding(
- Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs:34: return TradeScreenScenarioLoader.LoadFromJson(File.ReadAllText(DataPath("trade_screen_scenarios.json")));
- Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs:282: var binding = TradeScreenScenarioLoader.CreateBinding(s, LoadTells(), new SeededRng(61));
- Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs:311: var binding = TradeScreenScenarioLoader.CreateBinding(s, tells, new SeededRng(61));
- Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs:330: var a = TradeScreenScenarioLoader.CreateBinding(s, tells, new SeededRng(2026));
- Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs:331: var b = TradeScreenScenarioLoader.CreateBinding(s, tells, new SeededRng(2026));
- Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs:50: return TradeScreenScenarioLoader.LoadFromJson(File.ReadAllText(path));
### `TradeScreenPresenter` (18 sampled current references)
- Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs:28: public sealed class TradeScreenPresenter : ITradeIntentSink
- Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs:107: public TradeScreenPresenter(
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:396: ["trade_texts.json"] = new[] { "TradeScreenPresenter" },
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:743: ["trade_texts.json"] = "TradeScreenPresenter",
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:954: ["economy_goods.json"] = new[] { "MarketSystem", "TradeScreenPresenter" },
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:999: ["trade_screen_scenarios.json"] = new[] { "TradeScreenPresenter" },
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1001: ["trade_tell_lines.json"] = new[] { "TradeScreenPresenter" },
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1002: ["trade_texts.json"] = new[] { "TradeScreenPresenter" },
- src/Host/ContentUtilizationRuntimeCollector.cs:783: "TradeScreenPresenter",
- src/Host/ContentUtilizationRuntimeCollector.cs:788: "TradeScreenPresenter",
- src/Host/ContentUtilizationRuntimeCollector.cs:804: "TradeScreenPresenter",
- Ashfall.Core.Tests/TradeScreenSeamTests.cs:184: var presenter = new TradeScreenPresenter(
- Ashfall.Core.Tests/TradeScreenSeamTests.cs:208: var presenter = new TradeScreenPresenter(counting, null, LoadTells(), new SeededRng(2026));
- Ashfall.Core.Tests/TradeScreenSeamTests.cs:226: var presenter = new TradeScreenPresenter(
- Ashfall.Core.Tests/TradeScreenSeamTests.cs:266: var presenter = new TradeScreenPresenter(
- Ashfall.Core.Tests/TradeScreenSeamTests.cs:283: var presenter = new TradeScreenPresenter(
- Ashfall.Core.Tests/TradeTextCatalogTests.cs:189: var withoutVoice = new TradeScreenPresenter(
- Ashfall.Core.Tests/TradeTextCatalogTests.cs:193: var withVoice = new TradeScreenPresenter(
### `TradeScreenGodotPanel` (16 sampled current references)
- src/Main.UiTests.Economy.cs:39: /// open/close cycles don't corrupt state, and TradeScreenGodotPanel hits
- src/Main.UiTests.Economy.cs:98: var tradePanel = new TradeScreenGodotPanel();
- src/Main.UiTests.Economy.cs:134: var emptyPanel = new TradeScreenGodotPanel();
- src/Main.Economy.cs:38: private AtomicWar.GodotApp.Economy.TradeScreenGodotPanel _tradePanel = null!;
- src/Main.UiPanels.cs:682: _tradePanel = new AtomicWar.GodotApp.Economy.TradeScreenGodotPanel();
- src/Economy/TradeScreenGodotPanel.cs:25: public partial class TradeScreenGodotPanel : PanelContainer
- src/Host/ContentUtilizationRuntimeCollector.cs:793: "TradeScreenGodotPanel",
- src/Host/ContentUtilizationRuntimeCollector.cs:809: "TradeScreenGodotPanel",
- src/UI/CaravanBarterLedgerPanel.cs:18: /// Dashboard HYBRID wrapper around the existing TradeScreenGodotPanel.
- src/UI/CaravanBarterLedgerPanel.cs:21: /// TradeScreenGodotPanel and would be regressed by any wholesale refactor.
- src/UI/CaravanBarterLedgerPanel.cs:24: /// hosts TradeScreenGodotPanel inside the dashboard shell content slot.
- src/UI/CaravanBarterLedgerPanel.cs:39: /// TradeScreenGodotPanel. This wrapper does not re-implement the wiring; it
- src/UI/CaravanBarterLedgerPanel.cs:50: private TradeScreenGodotPanel _tradeInner = null!;
- src/UI/CaravanBarterLedgerPanel.cs:193: // TradeScreenGodotPanel builds its own internal UI when added to the tree.
- src/UI/CaravanBarterLedgerPanel.cs:196: _tradeInner = new TradeScreenGodotPanel();
- src/UI/SnapshotHarness.cs:42: new Target{ StableId="trade_default",             Title="Trade screen (default state)",         PanelCtor="AtomicWar.GodotApp.Economy.TradeScreenGodotPanel",     StateHint="default",  Width=1920, Height=1080 },
### `OpenTradeScreen` (3 sampled current references)
- src/Main.Economy.cs:328: private void OpenTradeScreen()
- src/Main.GameFlow.cs:612: OpenTradeScreen();
- src/Main.PlayerSurfaces.cs:415: openAction: () => OpenTradeScreen(),
### `TradeScreenSeam` (1 sampled current references)
- Ashfall.Core.Tests/TradeScreenSeamTests.cs:16: public class TradeScreenSeamTests

# Appendix E — Current focused-test inventory

Current test declaration inventory: 86 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs` — 38 test declarations; bytes=19,532; SHA-256=`50867414810aa973729e7f653fd5ed5e9b5ff1006a557a7567a7ffefb37d8974`
- 00107: [Fact]
- 00108: public void Catalog_ContainsExactlyFifteenScenarios()
- 00113: [Fact]
- 00114: public void Catalog_ScenarioIdsAreUniqueSnakeCase()
- 00126: [Fact]
- 00127: public void Catalog_AllTwelvePlan61ScenariosPresent()
- 00136: [Fact]
- 00137: public void Catalog_AllEightArchetypesRepresentedInPlan61Distribution()
- 00159: [Fact]
- 00160: public void Catalog_EveryItemReferenceResolvesToItemsJson()
- 00178: [Fact]
- 00179: public void Catalog_EveryFactionReferenceResolvesToCanonicalSet()
- 00189: [Fact]
- 00190: public void Catalog_ValidStancesMetersAndShockBounds()
- 00207: [Fact]
- 00208: public void Catalog_LineDataIsPositiveAndFinite()
- 00223: [Fact]
- 00224: public void Catalog_UnitPricesAreGloballyConsistentPerItem()
- 00248: [Fact]
- 00249: public void Catalog_CrossScenarioPriceSpreadsStayWithinAntiArbitrageBand()
- 00277: [Fact]
- 00278: public void Catalog_ComputedFairnessMatchesDataExpectation()
- 00291: [Fact]
- 00292: public void Catalog_ConfirmSucceedsOnlyForFairTradableTables()
- 00305: [Fact]
- 00306: public void Catalog_EveryScenarioBindsWithTellAndLegibleState()
- 00324: [Fact]
- 00325: public void Catalog_TellSelectionIsSeedDeterministic()
- 00339: [Fact]
- 00340: public void Catalog_OnlyDeliberateEmptyTablesAreEmpty()
- 00353: [Fact]
- 00354: public void Catalog_DebtCollectorTableIsDemandsOnly()
- 00368: [Fact]
- 00369: public void Catalog_EveryScenarioDiffersFromNearestNeighborInTwoDimensions()
- 00404: [Fact]
- 00405: public void Catalog_OriginalThreeScenariosKeepLockedContracts()
- 00428: [Fact]
- 00429: public void Catalog_RootShapeIsPreserved()
### `Ashfall.Core.Tests/Economy/TradeScreenPresenterSnapshotTests.cs` — 18 test declarations; bytes=16,499; SHA-256=`72be191907fc64c96e71ead28b71dd855df312544c17377e3e30d8206d93a7ce`
- 00038: [Fact]
- 00039: public void BarterTotals_CalculatesPlayerAndFactionWorthCorrectly()
- 00095: [Fact]
- 00096: public void BarterTotals_QualitativeThresholds_Snapshot()
- 00124: [Fact]
- 00125: public void DisabledActions_EmptyTable_ConfirmDisabled()
- 00137: [Fact]
- 00138: public void DisabledActions_OfferShort_ConfirmDisabled()
- 00155: [Theory]
- 00160: public void DisabledActions_StanceGating_WillTradeControlsConfirm(float trust, bool expectCanConfirmWhenFair)
- 00177: [Fact]
- 00178: public void SelectionRestoration_CaptureAndRestore_PreservesAllOffersTotalsAndActions()
- 00256: [Fact]
- 00257: public void SelectionRestoration_FactionSwitching_PreservesIndependence()
- 00297: [Fact]
- 00298: public void BiologicalOfferings_PricingCalculations()
- 00324: [Fact]
- 00325: public void RadioTicker_UpdatesOnRecalculateAndExecution()
### `Ashfall.Core.Tests/TradeScreenSeamTests.cs` — 22 test declarations; bytes=14,462; SHA-256=`7940a9f2745852452d4a37666199d3187914ce18d05551e675588fa27169cfc6`
- 00057: [Fact]
- 00058: public void Scenarios_LoadAllThreeFromData()
- 00070: [Fact]
- 00071: public void Scenario_FairDeal_ComputedFairnessMatchesDataExpectation()
- 00092: [Fact]
- 00093: public void Scenario_OfferShort_BlocksConfirmAndKeepsStanceLegible()
- 00109: [Fact]
- 00110: public void Scenario_EmptyTable_IsDeliberateNotBroken()
- 00129: [Fact]
- 00130: public void Scenario_IntentSink_CloseRecordsTradedFlag()
- 00167: public void SetTrust(string factionId, float value) { Mutations++; _inner.SetTrust(factionId, value); }
- 00169: public void SetRaidAggression(string factionId, float value) { Mutations++; _inner.SetRaidAggression(factionId, value); }
- 00173: [Fact]
- 00174: public void Presenter_MapsProvidersOntoViewModel()
- 00204: [Fact]
- 00205: public void Presenter_ZeroMutation_InvariantHolds()
- 00223: [Fact]
- 00224: public void Presenter_ApiParity_TradeScreenUISurface()
- 00263: [Fact]
- 00264: public void Presenter_BioOffersPricedByCoreRule()
- 00279: [Fact]
- 00280: public void Presenter_RoutesExecutionThroughSink()
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

**Pass intent:** improve `Trade Screen Scenarios: Fifteen-Catalog Negotiation Surface and Canonical Transaction Boundary` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan calls the catalog fully wired to settlements/patrols; current production caller evidence must be checked before retaining that statement.
- The historical plan treats every item reference as economy-valid; the plan now requires a current reference audit.

## H.2 Evidence-strength corrections
- Trace the actual live route instead of trusting the catalog test.
- Distinguish fixture `confirm_succeeds` from a real command result.
- Keep fairness, scarcity, and seeded ordering explicit.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Trade Screen Scenarios: Fifteen-Catalog Negotiation Surface and Canonical Transaction Boundary while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use TradeScreenScenarioLoader for definitions and TradeScreenPresenter for deterministic projection.
- Use canonical economy/inventory/funds owners for all real settlement.
- Use faction/debt owners for consequences; scenario text cannot write them.
- Do not persist presenter-only DTOs.
- Keep the live panel truthful, accessible, and refresh-stable.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement trade screen scenarios: fifteen-catalog negotiation surface and canonical transaction boundary arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- Do not claim live scenario reachability without a production caller and host binding.
- Do not propose a new save section for negotiation presentation.

## J.2 Questions deliberately left open
- Should the live Foundry trade surface select authored scenarios, or should the catalog remain a headless/demo corpus?
- Which canonical owner supplies faction succession/trust if scenario selection is promoted?

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

The original file at `HEAD:piagentsplans/61-trade-screen-scenarios.md` contained 4,801 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 61 — Trade Screen Scenarios Expansion (3 → 15 scenarios)

## Goal (2 lines)
Expand `trade_screen_scenarios.json` from 3 verified entries to 15 trade scenarios. The
trade screen system (`TradeScreenPresenter`, `TradeScreenScenarios`, `TradeScreenSeam`)
is fully implemented but has only 3 scenarios — trade encounters are repetitive and lack
narrative variety.

## Why (P2)
- Verified: `trade_screen_scenarios.json` has 3 entries; `TradeScreenScenarios.cs` and
  `TradeScreenPresenter.cs` are fully implemented in `Assets/Ashfall.Core/Economy/`.
- Creates the trade-variety pillar: each trade scenario gives the encounter a context
  (a desperate trader, a faction supply run, a black-market exchange, a debt collection,
  a bulk deal) that affects prices, available goods, and negotiation options.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/trade_screen_scenarios.json` (expand 3 → 15 scenarios)
- Read-only: `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs` (confirm scenario
  schema: id, display name, trader type, available goods, price modifiers, negotiation
  options, faction link, special conditions)

## Content grammar (per scenario)
- snake_case `id` with prefix `trade_` or `scenario_` (confirm accepted prefix from
  existing 3 entries).
- trader_type: desperate_survivor / faction_quartermaster / black_market / caravan_merchant /
  debt_collector / bulk_dealer / smuggler / refugee_barter.
- available_goods: list of `item_*` ids the trader offers (subset of economy_goods).
- price_modifier: multiplier on base prices (desperate = 1.5, bulk = 0.8, black_market =
  variable).
- negotiation_options: available trade tell lines (feeds `trade_tell_lines.json` — Plan 62).
- faction_link: optional `faction_*` id — faction quartermasters only trade with allies.
- special_condition: min reputation, min day, required flag, or required item to unlock
  the scenario.
- description: 1-2 sentences of grounded trade flavor. Skill `ashfall-write`.

## Steps
1. Read `TradeScreenScenarios.cs` to confirm the scenario schema and how scenarios
   affect the trade screen.
2. Read the 3 existing scenarios to understand the structure and avoid duplication.
3. Author 12 new scenarios across 8 trader types:
   - 2 desperate survivors (high prices, rare goods, moral hook — they need medicine).
   - 2 faction quartermasters (faction-locked, military gear, requires reputation).
   - 2 black market (variable prices, contraband, risk of scam — feeds Plan 40 debt).
   - 2 caravan merchants (bulk goods, standard prices, follows Plan 43 settlement routes).
   - 1 debt collector (the player owes a debt — this scenario is the collection encounter;
     feeds Plan 40).
   - 1 bulk dealer (large quantities, discount for volume — feeds Plan 56 economy goods).
   - 1 smuggler (contraband, high risk, high reward — feeds Plan 45 faction patrols).
   - 1 refugee barter (the refugee has nothing of value but knows a location — feeds
     Plan 43/52).
4. Give each scenario: trader type, available goods, price modifier, negotiation options,
   faction link, special condition, description.
5. Cross-reference: every `item_*` good resolves to `items.json` + `economy_goods.json`;
   every `faction_*` link resolves; every `flag_*` condition resolves.
6. Wire 4 scenarios into Plan 43 settlement trade — each settlement has a default
   scenario type (trade post = caravan merchant, stronghold = quartermaster, refugee
   camp = refugee barter, community = bulk dealer).
7. Wire 2 scenarios into Plan 45 patrol encounters — smuggler and black-market
   scenarios appear when encountering faction patrols.
8. Validate: `--data-integrity-selftest`; confirm a trade scenario loads and modifies
   prices/availability in a headless boot.
9. xUnit: scenario catalog loads, all references resolve, price modifiers apply,
   faction locks block unauthorized trade, special conditions gate scenarios, save
   round-trip preserves trade state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data.

## Definition of Done
- `trade_screen_scenarios.json` has 15 scenarios (3 existing + 12 new), all references
  resolving, 4 wired to settlements, 2 wired to patrols, price modifiers apply, faction
  locks work, save round-trip green, integrity + tests green.

## Follow-on
- Plan 43 (settlements) — each settlement has a default trade scenario.
- Plan 45 (patrols) — smuggler and black-market scenarios on patrol encounters.
- Plan 40 (debt) — debt collector scenario.
- Plan 62 (trade tell lines) — negotiation options consume tell lines.
- Plan 56 (economy goods) — scenarios reference the expanded goods catalog.

```

## End of Plan 61 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs` — 286 lines; 12,304 bytes; SHA-256 `d0860320466aeb33812b16aea8a2484937cb8a57b88f4007e3f8f9989356b795`
Declaration index:
- 00013: public sealed class TradeScreenScenario
- 00039: public sealed class MockTradeIntentSink : ITradeIntentSink
- 00047: public bool TryConfirmTrade()
- 00053: public bool TryDemandParley()
- 00059: public void Close(bool traded)
- 00067: public sealed class MockTradeScreenBinding
- 00085: public static class TradeScreenScenarioLoader
- 00092: public static IReadOnlyList<TradeScreenScenario> LoadFromJson(string json)
- 00190: public static MockTradeScreenBinding CreateBinding(TradeScreenScenario scenario, ITradeTellProvider tells, ISeededRng rng)
- 00220: private static string GetString(JsonElement el, string prop)
- 00227: private static int GetInt(JsonElement el, string prop, int fallback)
- 00234: private static float GetFloat(JsonElement el, string prop, float fallback)
- 00241: private static bool GetBool(JsonElement el, string prop, bool fallback)
- 00249: private static TradeStance ParseStance(string key)
- 00265: private static TradeFairness ParseFairness(string key)
- 00275: private static PriceShockKind ParseShockKind(string key)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: namespace Ashfall.Core.Economy
00003: {
00004:     using System;
00005:     using System.Collections.Generic;
00006:     using System.Text.Json;
00007:
00008:     /// <summary>
00009:     /// One data-defined Negotiation Table scenario (fair deal, offer short,
00010:     /// empty table). JSON in StreamingAssets is the authority; nothing about
00011:     /// a scenario is hardcoded in hosts.
00012:     /// </summary>
00013:     public sealed class TradeScreenScenario
00014:     {
00015:         public string Id { get; set; } = string.Empty;
00016:         public string FactionId { get; set; } = string.Empty;
00017:         public string FactionName { get; set; } = string.Empty;
00018:         public string LeaderName { get; set; } = string.Empty;
00019:         public int SuccessionGeneration { get; set; } = 1;
00020:         public TradeStance Stance { get; set; } = TradeStance.Refuse;
00021:         public float Trust { get; set; }
00022:         public float Aggression { get; set; }
00023:         public int ConsecutiveRepels { get; set; }
00024:         public bool HasSurrendered { get; set; }
00025:         public bool CanDemandParley { get; set; }
00026:         public string WorldPhase { get; set; } = string.Empty;
00027:         public int WorldDay { get; set; } = 1;
00028:         public List<ShockBadgeData> PriceShocks { get; set; } = new();
00029:         public List<ScarcityBandData> Scarcity { get; set; } = new();
00030:         public List<TradeLineData> PlayerOffers { get; set; } = new();
00031:         public List<TradeLineData> FactionDemands { get; set; } = new();
00032:         public Dictionary<BiologicalTradeItem, int> BiologicalOffers { get; set; } = new();
00033:         public TradeFairness ExpectedFairness { get; set; } = TradeFairness.EmptyTable;
00034:         public bool ConfirmSucceeds { get; set; }
00035:         public string RadioTicker { get; set; } = string.Empty;
00036:     }
00037:
00038:     /// <summary>Records intent routed through the mock sink (skin-track assertions).</summary>
00039:     public sealed class MockTradeIntentSink : ITradeIntentSink
00040:     {
00041:         public int ConfirmCalls { get; private set; }
00042:         public int ParleyCalls { get; private set; }
00043:         public int CloseCalls { get; private set; }
00044:         public bool? LastCloseWasTraded { get; private set; }
00045:         public bool ConfirmResult { get; set; } = true;
00046:
00047:         public bool TryConfirmTrade()
00048:         {
00049:             ConfirmCalls++;
00050:             return ConfirmResult;
00051:         }
00052:
00053:         public bool TryDemandParley()
00054:         {
00055:             ParleyCalls++;
00056:             return true;
00057:         }
00058:
00059:         public void Close(bool traded)
00060:         {
00061:             CloseCalls++;
00062:             LastCloseWasTraded = traded;
00063:         }
00064:     }
00065:
00066:     /// <summary>A mock binding: view-model + intent sink built from a scenario.</summary>
00067:     public sealed class MockTradeScreenBinding
00068:     {
00069:         public TradeScreenScenario Scenario { get; }
00070:         public TradeScreenViewModel ViewModel { get; }
00071:         public MockTradeIntentSink Intents { get; }
00072:
00073:         public MockTradeScreenBinding(TradeScreenScenario scenario, TradeScreenViewModel viewModel, MockTradeIntentSink intents)
00074:         {
00075:             Scenario = scenario;
00076:             ViewModel = viewModel;
00077:             Intents = intents;
00078:         }
00079:     }
00080:
00081:     /// <summary>
00082:     /// Loads trade screen scenarios from JSON and builds mock bindings for the
00083:     /// skin track. Both tracks build against the same seam.
00084:     /// </summary>
00085:     public static class TradeScreenScenarioLoader
00086:     {
00087:         /// <summary>
00088:         /// Parses { "scenarios": [ { id, faction_id, stance, trust, player_offers: [{item_id,
00089:         /// display_name, quantity, unit_price}], biological_offers: {PintOfBlood: n},
00090:         /// faction_demands: [...], expected_fairness, confirm_succeeds, ... } ] }.
00091:         /// </summary>
00092:         public static IReadOnlyList<TradeScreenScenario> LoadFromJson(string json)
00093:         {
00094:             var result = new List<TradeScreenScenario>();
00095:             if (string.IsNullOrWhiteSpace(json)) return result;
00096:
00097:             using var doc = JsonDocument.Parse(json);
00098:             var root = doc.RootElement;
00099:             if (!root.TryGetProperty("scenarios", out var scenariosEl) || scenariosEl.ValueKind != JsonValueKind.Array)
00100:             {
00101:                 return result;
00102:             }
00103:
00104:             foreach (var s in scenariosEl.EnumerateArray())
00105:             {
00106:                 var scenario = new TradeScreenScenario
00107:                 {
00108:                     Id = GetString(s, "id"),
00109:                     FactionId = GetString(s, "faction_id"),
00110:                     FactionName = GetString(s, "faction_name"),
00111:                     LeaderName = GetString(s, "leader_name"),
00112:                     SuccessionGeneration = GetInt(s, "succession_generation", 1),
00113:                     Stance = ParseStance(GetString(s, "stance")),
00114:                     Trust = GetFloat(s, "trust", 0f),
00115:                     Aggression = GetFloat(s, "aggression", 0f),
00116:                     ConsecutiveRepels = GetInt(s, "consecutive_repels", 0),
00117:                     HasSurrendered = GetBool(s, "has_surrendered", false),
00118:                     CanDemandParley = GetBool(s, "can_demand_parley", false),
00119:                     WorldPhase = GetString(s, "world_phase"),
00120:                     WorldDay = GetInt(s, "world_day", 1),
00121:                     ExpectedFairness = ParseFairness(GetString(s, "expected_fairness")),
00122:                     ConfirmSucceeds = GetBool(s, "confirm_succeeds", false),
00123:                     RadioTicker = GetString(s, "radio_ticker")
00124:                 };
00125:
00126:                 if (s.TryGetProperty("price_shocks", out var shocksEl) && shocksEl.ValueKind == JsonValueKind.Array)
00127:                 {
00128:                     foreach (var sh in shocksEl.EnumerateArray())
00129:                     {
00130:                         scenario.PriceShocks.Add(new ShockBadgeData(
00131:                             ParseShockKind(GetString(sh, "kind")),
00132:                             GetFloat(sh, "multiplier", 1f),
00133:                             GetString(sh, "note")));
00134:                     }
00135:                 }
00136:
00137:                 if (s.TryGetProperty("scarcity", out var scarEl) && scarEl.ValueKind == JsonValueKind.Array)
00138:                 {
00139:                     foreach (var sc in scarEl.EnumerateArray())
00140:                     {
00141:                         scenario.Scarcity.Add(new ScarcityBandData(
00142:                             GetString(sc, "item_id"),
00143:                             GetString(sc, "display_name"),
00144:                             GetFloat(sc, "multiplier", 1f)));
00145:                     }
00146:                 }
00147:
00148:                 if (s.TryGetProperty("player_offers", out var offersEl) && offersEl.ValueKind == JsonValueKind.Array)
00149:                 {
00150:                     foreach (var o in offersEl.EnumerateArray())
00151:                     {
00152:                         scenario.PlayerOffers.Add(new TradeLineData(
00153:                             GetString(o, "item_id"),
00154:                             GetString(o, "display_name"),
00155:                             GetInt(o, "quantity", 0),
00156:                             GetFloat(o, "unit_price", 0f)));
00157:                     }
00158:                 }
00159:
00160:                 if (s.TryGetProperty("faction_demands", out var demandsEl) && demandsEl.ValueKind == JsonValueKind.Array)
00161:                 {
00162:                     foreach (var d in demandsEl.EnumerateArray())
00163:                     {
00164:                         scenario.FactionDemands.Add(new TradeLineData(
00165:                             GetString(d, "item_id"),
00166:                             GetString(d, "display_name"),
00167:                             GetInt(d, "quantity", 0),
00168:                             GetFloat(d, "unit_price", 0f)));
00169:                     }
00170:                 }
00171:
00172:                 if (s.TryGetProperty("biological_offers", out var bioEl) && bioEl.ValueKind == JsonValueKind.Object)
00173:                 {
00174:                     foreach (var b in bioEl.EnumerateObject())
00175:                     {
00176:                         if (Enum.TryParse<BiologicalTradeItem>(b.Name, ignoreCase: true, out var kind) && b.Value.ValueKind == JsonValueKind.Number)
00177:                         {
00178:                             scenario.BiologicalOffers[kind] = b.Value.GetInt32();
00179:                         }
00180:                     }
00181:                 }
00182:
00183:                 result.Add(scenario);
00184:             }
00185:
00186:             return result;
00187:         }
00188:
00189:         /// <summary>Builds the skin-track binding: a populated view-model + recording mock sink.</summary>
00190:         public static MockTradeScreenBinding CreateBinding(TradeScreenScenario scenario, ITradeTellProvider tells, ISeededRng rng)
00191:         {
00192:             if (scenario == null) throw new ArgumentNullException(nameof(scenario));
00193:
00194:             var vm = new TradeScreenViewModel();
00195:             vm.SetOpen(true);
00196:             vm.SetFaction(scenario.FactionId, scenario.FactionName, scenario.LeaderName, scenario.SuccessionGeneration);
00197:             vm.SetStance(scenario.Stance);
00198:             vm.SetMeters(scenario.Trust, scenario.Aggression);
00199:             vm.SetFactionPresence(scenario.ConsecutiveRepels, scenario.HasSurrendered, scenario.CanDemandParley);
00200:             vm.SetWorld(scenario.WorldPhase, scenario.WorldDay);
00201:             vm.SetShockBadges(scenario.PriceShocks);
00202:             vm.SetScarcityBands(scenario.Scarcity);
00203:
00204:             bool willTrade = scenario.Stance == TradeStance.Trade || scenario.Stance == TradeStance.ShareIntel;
00205:             vm.SetTable(scenario.PlayerOffers, scenario.FactionDemands, scenario.BiologicalOffers, willTrade);
00206:
00207:             if (tells != null && tells.TrySelectTell(scenario.Stance, scenario.Trust, rng, out var tell))
00208:             {
00209:                 vm.SetTell(tell.Id, tell.Line);
00210:             }
00211:
00212:             vm.SetRadioTicker(scenario.RadioTicker);
00213:
00214:             var sink = new MockTradeIntentSink { ConfirmResult = scenario.ConfirmSucceeds };
00215:             return new MockTradeScreenBinding(scenario, vm, sink);
00216:         }
00217:
00218:         // ── JSON helpers (invariant culture only) ────────────────────
00219:
00220:         private static string GetString(JsonElement el, string prop)
00221:         {
00222:             return el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.String
00223:                 ? v.GetString() ?? string.Empty
00224:                 : string.Empty;
00225:         }
00226:
00227:         private static int GetInt(JsonElement el, string prop, int fallback)
00228:         {
00229:             return el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.Number
00230:                 ? v.GetInt32()
00231:                 : fallback;
00232:         }
00233:
00234:         private static float GetFloat(JsonElement el, string prop, float fallback)
00235:         {
00236:             return el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.Number
00237:                 ? (float)v.GetDouble()
00238:                 : fallback;
00239:         }
00240:
00241:         private static bool GetBool(JsonElement el, string prop, bool fallback)
00242:         {
00243:             if (!el.TryGetProperty(prop, out var v)) return fallback;
00244:             if (v.ValueKind == JsonValueKind.True) return true;
00245:             if (v.ValueKind == JsonValueKind.False) return false;
00246:             return fallback;
00247:         }
00248:
00249:         private static TradeStance ParseStance(string key)
00250:         {
00251:             switch ((key ?? string.Empty).Trim())
00252:             {
00253:                 case "hostile_raid":
00254:                 case "HostileRaid": return TradeStance.HostileRaid;
00255:                 case "rob":
00256:                 case "Rob": return TradeStance.Rob;
00257:                 case "trade":
00258:                 case "Trade": return TradeStance.Trade;
00259:                 case "share_intel":
00260:                 case "ShareIntel": return TradeStance.ShareIntel;
00261:                 default: return TradeStance.Refuse;
00262:             }
00263:         }
00264:
00265:         private static TradeFairness ParseFairness(string key)
00266:         {
00267:             switch ((key ?? string.Empty).Trim())
00268:             {
00269:                 case "fair": return TradeFairness.Fair;
00270:                 case "short": return TradeFairness.Short;
00271:                 default: return TradeFairness.EmptyTable;
00272:             }
00273:         }
00274:
00275:         private static PriceShockKind ParseShockKind(string key)
00276:         {
00277:             switch ((key ?? string.Empty).Trim())
00278:             {
00279:                 case "ConvoyAmbush": return PriceShockKind.ConvoyAmbush;
00280:                 case "FactionWar": return PriceShockKind.FactionWar;
00281:                 case "WinterDeepens": return PriceShockKind.WinterDeepens;
00282:                 default: return PriceShockKind.PlumePassing;
00283:             }
00284:         }
00285:     }
00286: }
```

## `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs` — 467 lines; 18,043 bytes; SHA-256 `eaba535609c3ce04e7628d9930b6667dbc223dc4c5fa83db4e78bfa2292387eb`
Declaration index:
- 00009: public sealed class TradeSelectionSnapshot
- 00028: public sealed class TradeScreenPresenter : ITradeIntentSink
- 00055: public int GetPlayerOfferCount(string itemId) =>
- 00058: public int GetFactionAskCount(string itemId) =>
- 00061: public int GetBiologicalOfferCount(BiologicalTradeItem item) =>
- 00064: public TradeSelectionSnapshot CaptureSelection()
- 00074: public void RestoreSelection(TradeSelectionSnapshot snapshot)
- 00133: public void SetVoiceContext(TradeVoiceContext context)
- 00152: public void SetWorldContext(string phaseLabel, int day)
- 00159: public void SetWatchedItems(IEnumerable<string> itemIds)
- 00170: public bool Open(string factionId, string factionName, string leaderName, int successionGeneration)
- 00181: public void Close(bool traded = false)
- 00186: public void SetPlayerOffer(string itemId, int count)
- 00194: public void SetFactionAsk(string itemId, int count)
- 00202: public void SetBiologicalOffer(BiologicalTradeItem item, int count)
- 00209: public void ClearOffers()
- 00221: public void Recalculate()
- 00282: public bool TryConfirmTrade()
- 00328: public bool TryDemandParley()
- 00349: public string BuildQuoteSummary()
- 00386: private static string FormatLines(IReadOnlyList<TradeLineData> lines)
- 00396: private void ApplyVoice(TradeVoiceResult voice)
- 00405: private string JoinBioLines()
- 00415: private List<TradeLineData> BuildLines(Dictionary<string, int> counts)
- 00438: private List<ShockBadgeData> CollectShockBadges()
- 00454: private List<ScarcityBandData> CollectScarcityBands()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: namespace Ashfall.Core.Economy
00003: {
00004:     using System;
00005:     using System.Collections.Generic;
00006:     using System.Text;
00007:     using Ashfall.Core.Radio;
00008:
00009:     public sealed class TradeSelectionSnapshot
00010:     {
00011:         public Dictionary<string, int> PlayerOffers { get; set; } = new(StringComparer.Ordinal);
00012:         public Dictionary<string, int> FactionAsks { get; set; } = new(StringComparer.Ordinal);
00013:         public Dictionary<BiologicalTradeItem, int> BiologicalOffers { get; set; } = new();
00014:     }
00015:
00016:     /// <summary>
00017:     /// Track B (Nerves): maps the frozen core interfaces
00018:     /// (IFactionStanceProvider / IPriceShockProvider) onto the Act 0 seam
00019:     /// (ITradeScreenViewModel) and exposes the existing TradeScreenUI surface
00020:     /// (Open, Close, SetPlayerOffer, SetFactionAsk, Recalculate, TryConfirmTrade,
00021:     /// TryDemandParley, BuildQuoteSummary) for API parity.
00022:     ///
00023:     /// Zero-mutation invariant: the presenter only ever calls read methods on
00024:     /// the stance provider (Get*/WillTrade/TryGet*). Trust, aggression and all
00025:     /// other simulation state are never written through this class; execution
00026:     /// is routed exclusively through the injected ITradeExecutionSink.
00027:     /// </summary>
00028:     public sealed class TradeScreenPresenter : ITradeIntentSink
00029:     {
00030:         private readonly IFactionStanceProvider _stance;
00031:         private readonly IPriceShockProvider _shocks;
00032:         private readonly ITradeTellProvider _tells;
00033:         private readonly IFactionRadioProvider? _radio;
00034:         private readonly ITradeExecutionSink _execution;
00035:         private readonly ISeededRng _rng;
00036:         private readonly Func<string, float> _unitPrice;
00037:         private readonly Func<string, string> _displayName;
00038:         private readonly TradeVoiceResolver? _voiceResolver;
00039:         private TradeVoiceContext _voiceContext = new TradeVoiceContext();
00040:
00041:         private readonly Dictionary<string, int> _playerOfferCounts = new();
00042:         private readonly Dictionary<string, int> _factionAskCounts = new();
00043:         private readonly Dictionary<BiologicalTradeItem, int> _bioOfferCounts = new();
00044:         private readonly List<string> _watchedItems = new();
00045:
00046:         private string _worldPhaseLabel = string.Empty;
00047:         private int _worldDay = 1;
00048:
00049:         public TradeScreenViewModel ViewModel { get; } = new TradeScreenViewModel();
00050:
00051:         public int ActiveOfferCount => _playerOfferCounts.Count;
00052:         public int ActiveAskCount => _factionAskCounts.Count;
00053:         public int ActiveBioCount => _bioOfferCounts.Count;
00054:
00055:         public int GetPlayerOfferCount(string itemId) =>
00056:             _playerOfferCounts.TryGetValue(itemId, out int count) ? count : 0;
00057:
00058:         public int GetFactionAskCount(string itemId) =>
00059:             _factionAskCounts.TryGetValue(itemId, out int count) ? count : 0;
00060:
00061:         public int GetBiologicalOfferCount(BiologicalTradeItem item) =>
00062:             _bioOfferCounts.TryGetValue(item, out int count) ? count : 0;
00063:
00064:         public TradeSelectionSnapshot CaptureSelection()
00065:         {
00066:             return new TradeSelectionSnapshot
00067:             {
00068:                 PlayerOffers = new Dictionary<string, int>(_playerOfferCounts, StringComparer.Ordinal),
00069:                 FactionAsks = new Dictionary<string, int>(_factionAskCounts, StringComparer.Ordinal),
00070:                 BiologicalOffers = new Dictionary<BiologicalTradeItem, int>(_bioOfferCounts)
00071:             };
00072:         }
00073:
00074:         public void RestoreSelection(TradeSelectionSnapshot snapshot)
00075:         {
00076:             if (snapshot == null) return;
00077:             _playerOfferCounts.Clear();
00078:             if (snapshot.PlayerOffers != null)
00079:             {
00080:                 foreach (var pair in snapshot.PlayerOffers)
00081:                 {
00082:                     if (pair.Value > 0) _playerOfferCounts[pair.Key] = pair.Value;
00083:                 }
00084:             }
00085:
00086:             _factionAskCounts.Clear();
00087:             if (snapshot.FactionAsks != null)
00088:             {
00089:                 foreach (var pair in snapshot.FactionAsks)
00090:                 {
00091:                     if (pair.Value > 0) _factionAskCounts[pair.Key] = pair.Value;
00092:                 }
00093:             }
00094:
00095:             _bioOfferCounts.Clear();
00096:             if (snapshot.BiologicalOffers != null)
00097:             {
00098:                 foreach (var pair in snapshot.BiologicalOffers)
00099:                 {
00100:                     if (pair.Value > 0) _bioOfferCounts[pair.Key] = pair.Value;
00101:                 }
00102:             }
00103:
00104:             Recalculate();
00105:         }
00106:
00107:         public TradeScreenPresenter(
00108:             IFactionStanceProvider stanceProvider,
00109:             IPriceShockProvider? priceShockProvider = null,
00110:             ITradeTellProvider? tells = null,
00111:             ISeededRng? rng = null,
00112:             Func<string, float>? unitPriceLookup = null,
00113:             Func<string, string>? displayNameLookup = null,
00114:             ITradeExecutionSink? executionSink = null,
00115:             IFactionRadioProvider? radioProvider = null,
00116:             TradeVoiceResolver? voiceResolver = null)
00117:         {
00118:             _stance = stanceProvider;
00119:             _shocks = priceShockProvider;
00120:             _tells = tells;
00121:             _radio = radioProvider;
00122:             _rng = rng;
00123:             _execution = executionSink;
00124:             _unitPrice = unitPriceLookup ?? (_ => 10f);
00125:             _displayName = displayNameLookup ?? (id => (id ?? string.Empty).Replace('_', ' '));
00126:             _voiceResolver = voiceResolver;
00127:         }
00128:
00129:         /// <summary>
00130:         /// Sets presentation context only. The context is copied into the
00131:         /// resolver on refresh; it never becomes trade or faction state.
00132:         /// </summary>
00133:         public void SetVoiceContext(TradeVoiceContext context)
00134:         {
00135:             context ??= new TradeVoiceContext();
00136:             _voiceContext = new TradeVoiceContext
00137:             {
00138:                 TraderProfileId = context.TraderProfileId,
00139:                 ScenarioId = context.ScenarioId,
00140:                 FactionId = context.FactionId,
00141:                 CaravanId = context.CaravanId,
00142:                 CaravanOriginRegion = context.CaravanOriginRegion,
00143:                 SpecialtyId = context.SpecialtyId,
00144:                 Stance = context.Stance,
00145:                 Trust = context.Trust,
00146:                 StableContextKey = context.StableContextKey
00147:             };
00148:             Recalculate();
00149:         }
00150:
00151:         /// <summary>World context for the news strip (phase label + day).</summary>
00152:         public void SetWorldContext(string phaseLabel, int day)
00153:         {
00154:             _worldPhaseLabel = phaseLabel ?? string.Empty;
00155:             _worldDay = Math.Max(1, day);
00156:         }
00157:
00158:         /// <summary>Items whose scarcity multipliers show in the news strip.</summary>
00159:         public void SetWatchedItems(IEnumerable<string> itemIds)
00160:         {
00161:             _watchedItems.Clear();
00162:             if (itemIds != null)
00163:             {
00164:                 foreach (var id in itemIds) _watchedItems.Add(id);
00165:             }
00166:         }
00167:
00168:         // ── TradeScreenUI parity surface ─────────────────────────────
00169:
00170:         public bool Open(string factionId, string factionName, string leaderName, int successionGeneration)
00171:         {
00172:             if (_stance != null && !_stance.IsFactionActive(factionId)) return false;
00173:
00174:             ViewModel.SetOpen(true);
00175:             ViewModel.SetFaction(factionId, factionName, leaderName, successionGeneration);
00176:             ClearOffers();
00177:             Recalculate();
00178:             return true;
00179:         }
00180:
00181:         public void Close(bool traded = false)
00182:         {
00183:             ViewModel.SetOpen(false);
00184:         }
00185:
00186:         public void SetPlayerOffer(string itemId, int count)
00187:         {
00188:             if (string.IsNullOrEmpty(itemId)) return;
00189:             if (count <= 0) _playerOfferCounts.Remove(itemId);
00190:             else _playerOfferCounts[itemId] = count;
00191:             Recalculate();
00192:         }
00193:
00194:         public void SetFactionAsk(string itemId, int count)
00195:         {
00196:             if (string.IsNullOrEmpty(itemId)) return;
00197:             if (count <= 0) _factionAskCounts.Remove(itemId);
00198:             else _factionAskCounts[itemId] = count;
00199:             Recalculate();
00200:         }
00201:
00202:         public void SetBiologicalOffer(BiologicalTradeItem item, int count)
00203:         {
00204:             if (count <= 0) _bioOfferCounts.Remove(item);
00205:             else _bioOfferCounts[item] = count;
00206:             Recalculate();
00207:         }
00208:
00209:         public void ClearOffers()
00210:         {
00211:             _playerOfferCounts.Clear();
00212:             _factionAskCounts.Clear();
00213:             _bioOfferCounts.Clear();
00214:             Recalculate();
00215:         }
00216:
00217:         /// <summary>
00218:         /// Re-maps the providers onto the view model. Read-only against every
00219:         /// provider; raises exactly one Changed event through the VM batching.
00220:         /// </summary>
00221:         public void Recalculate()
00222:         {
00223:             string factionId = ViewModel.FactionId;
00224:
00225:             var stance = TradeStance.Refuse;
00226:             float trust = 0f;
00227:             float aggression = 0f;
00228:             bool willTrade = false;
00229:
00230:             if (_stance != null)
00231:             {
00232:                 stance = _stance.GetStance(factionId);
00233:                 trust = _stance.GetEffectiveTrust(factionId);
00234:                 aggression = _stance.GetRaidAggression(factionId);
00235:                 willTrade = _stance.WillTrade(factionId);
00236:             }
00237:
00238:             ViewModel.SetStance(stance);
00239:             ViewModel.SetMeters(trust, aggression);
00240:             ViewModel.SetWorld(_worldPhaseLabel, _worldDay);
00241:
00242:             if (_tells != null && _tells.TrySelectTell(stance, trust, _rng, out var tell))
00243:             {
00244:                 ViewModel.SetTell(tell.Id, tell.Line);
00245:             }
00246:
00247:             if (_voiceResolver != null)
00248:             {
00249:                 var context = new TradeVoiceContext
00250:                 {
00251:                     TraderProfileId = _voiceContext.TraderProfileId,
00252:                     ScenarioId = _voiceContext.ScenarioId,
00253:                     FactionId = string.IsNullOrEmpty(_voiceContext.FactionId)
00254:                         ? factionId
00255:                         : _voiceContext.FactionId,
00256:                     CaravanId = _voiceContext.CaravanId,
00257:                     CaravanOriginRegion = _voiceContext.CaravanOriginRegion,
00258:                     SpecialtyId = _voiceContext.SpecialtyId,
00259:                     Stance = stance,
00260:                     Trust = trust,
00261:                     StableContextKey = _voiceContext.StableContextKey
00262:                 };
00263:                 var voice = _voiceResolver.ResolveGreeting(context);
00264:                 ApplyVoice(voice);
00265:             }
00266:
00267:             if (_radio != null)
00268:             {
00269:                 var intercept = _radio.GetFactionEvent(factionId, RadioEventKind.InterceptChatter, _worldDay, _rng);
00270:                 ViewModel.SetRadioTicker(intercept.Message);
00271:             }
00272:
00273:             ViewModel.SetShockBadges(CollectShockBadges());
00274:             ViewModel.SetScarcityBands(CollectScarcityBands());
00275:             ViewModel.SetTable(
00276:                 BuildLines(_playerOfferCounts),
00277:                 BuildLines(_factionAskCounts),
00278:                 _bioOfferCounts,
00279:                 willTrade);
00280:         }
00281:
00282:         public bool TryConfirmTrade()
00283:         {
00284:             if (!ViewModel.CanConfirm) return false;
00285:             if (_execution != null)
00286:             {
00287:                 // Snapshot the table: the sink owns its copy, and clearing our
00288:                 // state afterwards must never empty a dictionary we already gave away.
00289:                 if (!_execution.TryExecuteTrade(
00290:                         ViewModel.FactionId,
00291:                         new Dictionary<string, int>(_playerOfferCounts),
00292:                         new Dictionary<string, int>(_factionAskCounts),
00293:                         new Dictionary<BiologicalTradeItem, int>(_bioOfferCounts)))
00294:                 {
00295:                     return false;
00296:                 }
00297:             }
00298:             ClearOffers();
00299:             if (_voiceResolver != null)
00300:             {
00301:                 var context = new TradeVoiceContext
00302:                 {
00303:                     TraderProfileId = _voiceContext.TraderProfileId,
00304:                     ScenarioId = _voiceContext.ScenarioId,
00305:                     FactionId = string.IsNullOrEmpty(_voiceContext.FactionId)
00306:                         ? ViewModel.FactionId
00307:                         : _voiceContext.FactionId,
00308:                     CaravanId = _voiceContext.CaravanId,
00309:                     CaravanOriginRegion = _voiceContext.CaravanOriginRegion,
00310:                     SpecialtyId = _voiceContext.SpecialtyId,
00311:                     Stance = ViewModel.Stance,
00312:                     Trust = ViewModel.Trust,
00313:                     StableContextKey = _voiceContext.StableContextKey
00314:                 };
00315:                 ApplyVoice(_voiceResolver.ResolveLine(
00316:                     context,
00317:                     TradeVoiceLineFamily.Acceptance,
00318:                     "pleased"));
00319:             }
00320:             if (_radio != null)
00321:             {
00322:                 var reaction = _radio.GetFactionEvent(ViewModel.FactionId, RadioEventKind.TradeReaction, _worldDay, _rng);
00323:                 ViewModel.SetRadioTicker(reaction.Message);
00324:             }
00325:             return true;
00326:         }
00327:
00328:         public bool TryDemandParley()
00329:         {
00330:             bool parleyOk = false;
00331:             if (_execution != null)
00332:             {
00333:                 parleyOk = _execution.TryDemandParley(ViewModel.FactionId);
00334:             }
00335:             else
00336:             {
00337:                 parleyOk = ViewModel.CanDemandParley;
00338:             }
00339:
00340:             if (_radio != null)
00341:             {
00342:                 var parleyLine = _radio.GetFactionEvent(ViewModel.FactionId, RadioEventKind.ParleyResolution, _worldDay, _rng);
00343:                 ViewModel.SetRadioTicker(parleyLine.Message);
00344:             }
00345:             return parleyOk;
00346:         }
00347:
00348:         /// <summary>Qualitative, multi-line quote summary (ECON-002: no raw digits).</summary>
00349:         public string BuildQuoteSummary()
00350:         {
00351:             var sb = new StringBuilder();
00352:             sb.AppendLine("THE NEGOTIATION TABLE");
00353:             sb.AppendLine($"{ViewModel.FactionName} · {ViewModel.LeaderName} (gen {ViewModel.SuccessionGeneration})");
00354:
00355:             if (ViewModel.PlayerOffers.Count == 0 && ViewModel.BiologicalOffers.Count == 0)
00356:             {
00357:                 sb.AppendLine("OFFER: your edge of the table is bare.");
00358:             }
00359:             else
00360:             {
00361:                 sb.Append("OFFER: ");
00362:                 sb.AppendLine(FormatLines(ViewModel.PlayerOffers));
00363:                 if (ViewModel.BiologicalOffers.Count > 0)
00364:                 {
00365:                     sb.Append("OFFER (the drawer): ");
00366:                     sb.AppendLine(JoinBioLines());
00367:                 }
00368:             }
00369:
00370:             if (ViewModel.FactionDemands.Count == 0)
00371:             {
00372:                 sb.AppendLine("DEMAND: their edge of the table is bare.");
00373:             }
00374:             else
00375:             {
00376:                 sb.Append("DEMAND: ");
00377:                 sb.AppendLine(FormatLines(ViewModel.FactionDemands));
00378:             }
00379:
00380:             sb.Append("SCALE: ").AppendLine(ViewModel.FairnessLabel);
00381:             return sb.ToString();
00382:         }
00383:
00384:         // ── Internals ────────────────────────────────────────────────
00385:
00386:         private static string FormatLines(IReadOnlyList<TradeLineData> lines)
00387:         {
00388:             var parts = new List<string>();
00389:             foreach (var line in lines)
00390:             {
00391:                 parts.Add($"{line.Quantity}x {line.DisplayName} — {line.WorthLabel}");
00392:             }
00393:             return string.Join(", ", parts);
00394:         }
00395:
00396:         private void ApplyVoice(TradeVoiceResult voice)
00397:         {
00398:             string displayName = _voiceResolver != null &&
00399:                 _voiceResolver.Catalog.TryGetTrader(voice.ProfileId, out var trader)
00400:                 ? trader.display_name
00401:                 : "The Merchant";
00402:             ViewModel.SetTraderVoice(voice.ProfileId, displayName, voice.Text);
00403:         }
00404:
00405:         private string JoinBioLines()
00406:         {
00407:             var parts = new List<string>();
00408:             foreach (var pair in ViewModel.BiologicalOffers)
00409:             {
00410:                 parts.Add($"{pair.Value}x {pair.Key}");
00411:             }
00412:             return string.Join(", ", parts);
00413:         }
00414:
00415:         private List<TradeLineData> BuildLines(Dictionary<string, int> counts)
00416:         {
00417:             var lines = new List<TradeLineData>();
00418:             foreach (var pair in counts)
00419:             {
00420:                 if (pair.Value > 0)
00421:                 {
00422:                     lines.Add(new TradeLineData(pair.Key, _displayName(pair.Key), pair.Value, _unitPrice(pair.Key)));
00423:                 }
00424:             }
00425:             return lines;
00426:         }
00427:
00428:         private static readonly PriceShockKind[] s_allShockKinds =
00429:         {
00430:             PriceShockKind.PlumePassing,
00431:             PriceShockKind.ConvoyAmbush,
00432:             PriceShockKind.FactionConflict,
00433:             PriceShockKind.SeasonalScarcity,
00434:             PriceShockKind.DiseaseOutbreak,
00435:             PriceShockKind.FuelShortage
00436:         };
00437:
00438:         private List<ShockBadgeData> CollectShockBadges()
00439:         {
00440:             var badges = new List<ShockBadgeData>();
00441:             if (_shocks == null) return badges;
00442:
00443:             for (int i = 0; i < s_allShockKinds.Length; i++)
00444:             {
00445:                 PriceShockKind kind = s_allShockKinds[i];
00446:                 if (_shocks.TryGetPriceShock(kind, _worldDay, out var rule))
00447:                 {
00448:                     badges.Add(new ShockBadgeData(rule.Kind, rule.Multiplier, rule.Trigger));
00449:                 }
00450:             }
00451:             return badges;
00452:         }
00453:
00454:         private List<ScarcityBandData> CollectScarcityBands()
00455:         {
00456:             var bands = new List<ScarcityBandData>();
00457:             if (_shocks == null) return bands;
00458:
00459:             foreach (var itemId in _watchedItems)
00460:             {
00461:                 float multiplier = _shocks.GetScarcityMultiplier(_worldDay, itemId);
00462:                 bands.Add(new ScarcityBandData(itemId, _displayName(itemId), multiplier));
00463:             }
00464:             return bands;
00465:         }
00466:     }
00467: }
```

## `Assets/Ashfall.Core/Economy/TradeScreenSeam.cs` — 368 lines; 14,015 bytes; SHA-256 `9f3560848968c257dff3930122d67bd94a548bfd1618e018f107be16b71e3392`
Declaration index:
- 00012: public enum TradeFairness
- 00023: public static class TradeFairnessLabels
- 00029: public static string For(TradeFairness fairness)
- 00044: public static class TradeWorthLabels
- 00046: public static string Format(float value)
- 00060: public static class TradePricing
- 00062: public static float BioUnitValue(BiologicalTradeItem item)
- 00069: public sealed class TradeLineData
- 00088: public sealed class ShockBadgeData
- 00103: public sealed class ScarcityBandData
- 00122: public interface ITradeScreenViewModel
- 00178: public interface ITradeIntentSink
- 00186: /// Frozen core interface for executing a confirmed trade against live
- 00190: public interface ITradeExecutionSink
- 00206: public sealed class TradeScreenViewModel : ITradeScreenViewModel
- 00247: public TradeScreenViewModel SetOpen(bool open) { IsOpen = open; Bump(); return this; }
- 00248: public TradeScreenViewModel SetFaction(string id, string name, string leader, int generation)
- 00257: public TradeScreenViewModel SetStance(TradeStance stance)
- 00264: public TradeScreenViewModel SetTell(string tellId, string tellLine)
- 00271: public TradeScreenViewModel SetTraderVoice(
- 00282: public TradeScreenViewModel SetMeters(float trust, float aggression)
- 00289: public TradeScreenViewModel SetFactionPresence(int repels, bool surrendered, bool canParley)
- 00297: public TradeScreenViewModel SetWorld(string phaseLabel, int day)
- 00304: public TradeScreenViewModel SetShockBadges(IEnumerable<ShockBadgeData> badges)
- 00311: public TradeScreenViewModel SetScarcityBands(IEnumerable<ScarcityBandData> bands)
- 00318: public TradeScreenViewModel SetTable(
- 00356: public TradeScreenViewModel SetRadioTicker(string line)
- 00363: private void Bump()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: namespace Ashfall.Core.Economy
00003: {
00004:     using System;
00005:     using System.Collections.Generic;
00006: #pragma warning disable CS8618
00007:
00008:     /// <summary>
00009:     /// Verdict of the arbitrator's scale at the negotiation table.
00010:     /// Labels are part of the verified gate surface (FAIR / SHORT).
00011:     /// </summary>
00012:     public enum TradeFairness
00013:     {
00014:         /// <summary>Both sides of the table are bare — a deliberate, closed posture.</summary>
00015:         EmptyTable,
00016:         /// <summary>The player's side outweighs or matches the faction's demands.</summary>
00017:         Fair,
00018:         /// <summary>The player's side is lighter than the faction's demands.</summary>
00019:         Short
00020:     }
00021:
00022:     /// <summary>Canonical fairness labels. Gates check the FAIR / SHORT substrings.</summary>
00023:     public static class TradeFairnessLabels
00024:     {
00025:         public const string EmptyTable = "EMPTY TABLE";
00026:         public const string Fair = "DEAL IS FAIR";
00027:         public const string Short = "OFFER SHORT";
00028:
00029:         public static string For(TradeFairness fairness)
00030:         {
00031:             switch (fairness)
00032:             {
00033:                 case TradeFairness.Fair: return Fair;
00034:                 case TradeFairness.Short: return Short;
00035:                 default: return EmptyTable;
00036:             }
00037:         }
00038:     }
00039:
00040:     /// <summary>
00041:     /// Qualitative worth vocabulary for the trade screen (ECON-002: no raw digits
00042:     /// on the offer edges). Shared by both hosts so thresholds never fork.
00043:     /// </summary>
00044:     public static class TradeWorthLabels
00045:     {
00046:         public static string Format(float value)
00047:         {
00048:             if (value <= 0f) return "None";
00049:             if (value < 20f) return "Sparse";
00050:             if (value < 60f) return "Modest";
00051:             if (value < 150f) return "Substantial";
00052:             return "Generous";
00053:         }
00054:     }
00055:
00056:     /// <summary>
00057:     /// Core pricing for biological offerings. One source of truth so the Godot
00058:     /// panel, the presenter, and scenario mocks cannot disagree.
00059:     /// </summary>
00060:     public static class TradePricing
00061:     {
00062:         public static float BioUnitValue(BiologicalTradeItem item)
00063:         {
00064:             return ((int)item + 1) * 25f;
00065:         }
00066:     }
00067:
00068:     /// <summary>One line on the table: an item pushed across, or an item guarded.</summary>
00069:     public sealed class TradeLineData
00070:     {
00071:         public string ItemId { get; }
00072:         public string DisplayName { get; }
00073:         public int Quantity { get; }
00074:         public string WorthLabel { get; }
00075:         public float TotalValue { get; }
00076:
00077:         public TradeLineData(string itemId, string displayName, int quantity, float unitValue)
00078:         {
00079:             ItemId = itemId ?? string.Empty;
00080:             DisplayName = displayName ?? ItemId;
00081:             Quantity = Math.Max(0, quantity);
00082:             TotalValue = unitValue * Quantity;
00083:             WorthLabel = TradeWorthLabels.Format(TotalValue);
00084:         }
00085:     }
00086:
00087:     /// <summary>One active price shock badge for the "news from outside" strip.</summary>
00088:     public sealed class ShockBadgeData
00089:     {
00090:         public PriceShockKind Kind { get; }
00091:         public float Multiplier { get; }
00092:         public string Note { get; }
00093:
00094:         public ShockBadgeData(PriceShockKind kind, float multiplier, string note)
00095:         {
00096:             Kind = kind;
00097:             Multiplier = multiplier;
00098:             Note = note ?? string.Empty;
00099:         }
00100:     }
00101:
00102:     /// <summary>One scarcity multiplier entry for the "news from outside" strip.</summary>
00103:     public sealed class ScarcityBandData
00104:     {
00105:         public string ItemId { get; }
00106:         public string DisplayName { get; }
00107:         public float Multiplier { get; }
00108:
00109:         public ScarcityBandData(string itemId, string displayName, float multiplier)
00110:         {
00111:             ItemId = itemId ?? string.Empty;
00112:             DisplayName = displayName ?? ItemId;
00113:             Multiplier = multiplier;
00114:         }
00115:     }
00116:
00117:     /// <summary>
00118:     /// Act 0 seam: the engine-agnostic read model for the Negotiation Table.
00119:     /// Exposes every field the 14 verified probes check. Views paint from this
00120:     /// and only this; they never touch simulation types directly.
00121:     /// </summary>
00122:     public interface ITradeScreenViewModel
00123:     {
00124:         bool IsOpen { get; }
00125:
00126:         // The trader's presence
00127:         string FactionId { get; }
00128:         string FactionName { get; }
00129:         string LeaderName { get; }
00130:         int SuccessionGeneration { get; }
00131:
00132:         // Posture / tell
00133:         TradeStance Stance { get; }
00134:         string StanceBadgeText { get; }
00135:         string StanceTellId { get; }
00136:         string StanceTellLine { get; }
00137:         string TraderProfileId { get; }
00138:         string TraderDisplayName { get; }
00139:         string TraderVoiceLine { get; }
00140:
00141:         // Ledger-edge meters
00142:         float Trust { get; }
00143:         float Aggression { get; }
00144:         int ConsecutiveRepels { get; }
00145:         bool HasSurrendered { get; }
00146:         bool CanDemandParley { get; }
00147:
00148:         // The world intruding on the deal
00149:         string WorldPhaseLabel { get; }
00150:         int WorldDay { get; }
00151:         IReadOnlyList<ShockBadgeData> ShockBadges { get; }
00152:         IReadOnlyList<ScarcityBandData> ScarcityMultipliers { get; }
00153:
00154:         // The two edges of the table
00155:         IReadOnlyList<TradeLineData> PlayerOffers { get; }
00156:         IReadOnlyList<TradeLineData> FactionDemands { get; }
00157:         IReadOnlyDictionary<BiologicalTradeItem, int> BiologicalOffers { get; }
00158:         float PlayerOfferValue { get; }
00159:         float FactionAskValue { get; }
00160:
00161:         // The arbitrator's scale
00162:         TradeFairness Fairness { get; }
00163:         string FairnessLabel { get; }
00164:         bool CanConfirm { get; }
00165:
00166:         // The room's radio
00167:         string RadioTickerLine { get; }
00168:
00169:         /// <summary>Raised whenever any field above changes.</summary>
00170:         event Action Changed;
00171:     }
00172:
00173:     /// <summary>
00174:     /// Act 0 seam: where player intent leaves the presentation layer.
00175:     /// Implemented by presenters (routing to an ITradeExecutionSink) and by
00176:     /// mock scenario bindings on the skin track.
00177:     /// </summary>
00178:     public interface ITradeIntentSink
00179:     {
00180:         bool TryConfirmTrade();
00181:         bool TryDemandParley();
00182:         void Close(bool traded);
00183:     }
00184:
00185:     /// <summary>
00186:     /// Frozen core interface for executing a confirmed trade against live
00187:     /// simulation state. Hosts adapt their economy system to this; the
00188:     /// presenter never mutates providers directly.
00189:     /// </summary>
00190:     public interface ITradeExecutionSink
00191:     {
00192:         bool WillTrade(string factionId);
00193:         bool TryExecuteTrade(
00194:             string factionId,
00195:             IReadOnlyDictionary<string, int> playerOffers,
00196:             IReadOnlyDictionary<string, int> factionAsks,
00197:             IReadOnlyDictionary<BiologicalTradeItem, int> biologicalOffers);
00198:         bool TryDemandParley(string factionId);
00199:     }
00200:
00201:     /// <summary>
00202:     /// Mutable, engine-agnostic implementation of the read model. The presenter
00203:     /// (live track) and the scenario mocks (skin track) both write this; views
00204:     /// subscribe to <see cref="Changed"/>.
00205:     /// </summary>
00206:     public sealed class TradeScreenViewModel : ITradeScreenViewModel
00207:     {
00208:         private readonly List<TradeLineData> _playerOffers = new();
00209:         private readonly List<TradeLineData> _factionDemands = new();
00210:         private readonly Dictionary<BiologicalTradeItem, int> _bioOffers = new();
00211:         private readonly List<ShockBadgeData> _shocks = new();
00212:         private readonly List<ScarcityBandData> _scarcity = new();
00213:
00214:         public event Action Changed;
00215:
00216:         public bool IsOpen { get; private set; }
00217:         public string FactionId { get; private set; } = string.Empty;
00218:         public string FactionName { get; private set; } = string.Empty;
00219:         public string LeaderName { get; private set; } = string.Empty;
00220:         public int SuccessionGeneration { get; private set; }
00221:         public TradeStance Stance { get; private set; } = TradeStance.Refuse;
00222:         public string StanceBadgeText { get; private set; } = "[ STANCE: REFUSE ]";
00223:         public string StanceTellId { get; private set; } = string.Empty;
00224:         public string StanceTellLine { get; private set; } = string.Empty;
00225:         public string TraderProfileId { get; private set; } = string.Empty;
00226:         public string TraderDisplayName { get; private set; } = string.Empty;
00227:         public string TraderVoiceLine { get; private set; } = string.Empty;
00228:         public float Trust { get; private set; }
00229:         public float Aggression { get; private set; }
00230:         public int ConsecutiveRepels { get; private set; }
00231:         public bool HasSurrendered { get; private set; }
00232:         public bool CanDemandParley { get; private set; }
00233:         public string WorldPhaseLabel { get; private set; } = string.Empty;
00234:         public int WorldDay { get; private set; } = 1;
00235:         public IReadOnlyList<TradeLineData> PlayerOffers => _playerOffers;
00236:         public IReadOnlyList<TradeLineData> FactionDemands => _factionDemands;
00237:         public IReadOnlyDictionary<BiologicalTradeItem, int> BiologicalOffers => _bioOffers;
00238:         public IReadOnlyList<ShockBadgeData> ShockBadges => _shocks;
00239:         public IReadOnlyList<ScarcityBandData> ScarcityMultipliers => _scarcity;
00240:         public float PlayerOfferValue { get; private set; }
00241:         public float FactionAskValue { get; private set; }
00242:         public TradeFairness Fairness { get; private set; } = TradeFairness.EmptyTable;
00243:         public string FairnessLabel => TradeFairnessLabels.For(Fairness);
00244:         public bool CanConfirm { get; private set; }
00245:         public string RadioTickerLine { get; private set; } = string.Empty;
00246:
00247:         public TradeScreenViewModel SetOpen(bool open) { IsOpen = open; Bump(); return this; }
00248:         public TradeScreenViewModel SetFaction(string id, string name, string leader, int generation)
00249:         {
00250:             FactionId = id ?? string.Empty;
00251:             FactionName = name ?? FactionId;
00252:             LeaderName = leader ?? string.Empty;
00253:             SuccessionGeneration = generation;
00254:             Bump();
00255:             return this;
00256:         }
00257:         public TradeScreenViewModel SetStance(TradeStance stance)
00258:         {
00259:             Stance = stance;
00260:             StanceBadgeText = $"[ STANCE: {stance.ToString().ToUpperInvariant()} ]";
00261:             Bump();
00262:             return this;
00263:         }
00264:         public TradeScreenViewModel SetTell(string tellId, string tellLine)
00265:         {
00266:             StanceTellId = tellId ?? string.Empty;
00267:             StanceTellLine = tellLine ?? string.Empty;
00268:             Bump();
00269:             return this;
00270:         }
00271:         public TradeScreenViewModel SetTraderVoice(
00272:             string profileId,
00273:             string displayName,
00274:             string line)
00275:         {
00276:             TraderProfileId = profileId ?? string.Empty;
00277:             TraderDisplayName = displayName ?? string.Empty;
00278:             TraderVoiceLine = line ?? string.Empty;
00279:             Bump();
00280:             return this;
00281:         }
00282:         public TradeScreenViewModel SetMeters(float trust, float aggression)
00283:         {
00284:             Trust = trust;
00285:             Aggression = aggression;
00286:             Bump();
00287:             return this;
00288:         }
00289:         public TradeScreenViewModel SetFactionPresence(int repels, bool surrendered, bool canParley)
00290:         {
00291:             ConsecutiveRepels = repels;
00292:             HasSurrendered = surrendered;
00293:             CanDemandParley = canParley;
00294:             Bump();
00295:             return this;
00296:         }
00297:         public TradeScreenViewModel SetWorld(string phaseLabel, int day)
00298:         {
00299:             WorldPhaseLabel = phaseLabel ?? string.Empty;
00300:             WorldDay = Math.Max(1, day);
00301:             Bump();
00302:             return this;
00303:         }
00304:         public TradeScreenViewModel SetShockBadges(IEnumerable<ShockBadgeData> badges)
00305:         {
00306:             _shocks.Clear();
00307:             if (badges != null) _shocks.AddRange(badges);
00308:             Bump();
00309:             return this;
00310:         }
00311:         public TradeScreenViewModel SetScarcityBands(IEnumerable<ScarcityBandData> bands)
00312:         {
00313:             _scarcity.Clear();
00314:             if (bands != null) _scarcity.AddRange(bands);
00315:             Bump();
00316:             return this;
00317:         }
00318:         public TradeScreenViewModel SetTable(
00319:             IEnumerable<TradeLineData> playerOffers,
00320:             IEnumerable<TradeLineData> factionDemands,
00321:             IReadOnlyDictionary<BiologicalTradeItem, int> biologicalOffers,
00322:             bool willTrade)
00323:         {
00324:             _playerOffers.Clear();
00325:             if (playerOffers != null) _playerOffers.AddRange(playerOffers);
00326:             _factionDemands.Clear();
00327:             if (factionDemands != null) _factionDemands.AddRange(factionDemands);
00328:             _bioOffers.Clear();
00329:             if (biologicalOffers != null)
00330:             {
00331:                 foreach (var pair in biologicalOffers)
00332:                 {
00333:                     if (pair.Value > 0) _bioOffers[pair.Key] = pair.Value;
00334:                 }
00335:             }
00336:
00337:             PlayerOfferValue = 0f;
00338:             foreach (var line in _playerOffers) PlayerOfferValue += line.TotalValue;
00339:             foreach (var pair in _bioOffers) PlayerOfferValue += TradePricing.BioUnitValue(pair.Key) * pair.Value;
00340:             FactionAskValue = 0f;
00341:             foreach (var line in _factionDemands) FactionAskValue += line.TotalValue;
00342:
00343:             if (_playerOffers.Count == 0 && _factionDemands.Count == 0 && _bioOffers.Count == 0)
00344:             {
00345:                 Fairness = TradeFairness.EmptyTable;
00346:                 CanConfirm = false;
00347:             }
00348:             else
00349:             {
00350:                 Fairness = PlayerOfferValue >= FactionAskValue ? TradeFairness.Fair : TradeFairness.Short;
00351:                 CanConfirm = Fairness == TradeFairness.Fair && willTrade;
00352:             }
00353:             Bump();
00354:             return this;
00355:         }
00356:         public TradeScreenViewModel SetRadioTicker(string line)
00357:         {
00358:             RadioTickerLine = line ?? string.Empty;
00359:             Bump();
00360:             return this;
00361:         }
00362:
00363:         private void Bump()
00364:         {
00365:             Changed?.Invoke();
00366:         }
00367:     }
00368: }
```

## `Assets/Ashfall.Core/Economy/TradeTellEngine.cs` — 213 lines; 8,276 bytes; SHA-256 `ca0b50673b96d37a7d4a74f100dc0e7a3403ec2738106837d728eba22da784cb`
Declaration index:
- 00012: public static class TradeTrustBands
- 00021: public sealed class TradeTell
- 00041: public interface ITradeTellProvider
- 00052: public sealed class TradeTellEngine : ITradeTellProvider
- 00072: public void RegisterBand(string id, float minInclusive, float maxInclusive)
- 00078: public void RegisterTellPool(TradeStance stance, string bandId, IEnumerable<string> lines)
- 00096: public string BandForTrust(float trust)
- 00105: public bool TrySelectTell(TradeStance stance, float trust, ISeededRng rng, out TradeTell tell)
- 00125: public bool TryGetPoolLines(TradeStance stance, string bandId, out IReadOnlyList<string> lines)
- 00136: private static string PoolKey(TradeStance stance, string bandId)
- 00141: private static string StanceKey(TradeStance stance)
- 00158: public static TradeTellEngine LoadFromJson(string json)
- 00200: private static bool TryParseStance(string key, out TradeStance stance)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: namespace Ashfall.Core.Economy
00003: {
00004:     using System;
00005:     using System.Collections.Generic;
00006:     using System.Text.Json;
00007:
00008:     /// <summary>
00009:     /// Trust band the trader falls into at the table. Bands are data-defined
00010:     /// (trade_tell_lines.json); these are the canonical ids.
00011:     /// </summary>
00012:     public static class TradeTrustBands
00013:     {
00014:         public const string Hostile = "hostile";
00015:         public const string Wary = "wary";
00016:         public const string Neutral = "neutral";
00017:         public const string Warm = "warm";
00018:     }
00019:
00020:     /// <summary>One selected tell: the trader's readable posture at the table.</summary>
00021:     public sealed class TradeTell
00022:     {
00023:         public string Id { get; }
00024:         public TradeStance Stance { get; }
00025:         public string Band { get; }
00026:         public string Line { get; }
00027:
00028:         public TradeTell(string id, TradeStance stance, string band, string line)
00029:         {
00030:             Id = id ?? string.Empty;
00031:             Stance = stance;
00032:             Band = band ?? string.Empty;
00033:             Line = line ?? string.Empty;
00034:         }
00035:     }
00036:
00037:     /// <summary>
00038:     /// Tell selection surface: stance × trust band → tell id + line.
00039:     /// Data-defined catalog, deterministic rotation via ISeededRng.
00040:     /// </summary>
00041:     public interface ITradeTellProvider
00042:     {
00043:         bool TrySelectTell(TradeStance stance, float trust, ISeededRng rng, out TradeTell tell);
00044:         string BandForTrust(float trust);
00045:     }
00046:
00047:     /// <summary>
00048:     /// The tell-line corpus engine. Same pattern as FactionRadioEngine: JSON
00049:     /// in StreamingAssets is the authority, selection is seed-deterministic,
00050:     /// and the engine is pure C# with zero host references.
00051:     /// </summary>
00052:     public sealed class TradeTellEngine : ITradeTellProvider
00053:     {
00054:         private readonly List<(string Id, float MinInclusive, float MaxInclusive)> _bands = new();
00055:         private readonly Dictionary<string, List<string>> _pools = new(StringComparer.Ordinal);
00056:         private readonly Dictionary<string, TradeStance> _stances = new(StringComparer.Ordinal);
00057:
00058:         public int BandCount => _bands.Count;
00059:         public int PoolCount => _pools.Count;
00060:         public int LineCount { get; private set; }
00061:
00062:         public IReadOnlyList<string> Bands
00063:         {
00064:             get
00065:             {
00066:                 var ids = new List<string>();
00067:                 foreach (var band in _bands) ids.Add(band.Id);
00068:                 return ids;
00069:             }
00070:         }
00071:
00072:         public void RegisterBand(string id, float minInclusive, float maxInclusive)
00073:         {
00074:             if (string.IsNullOrWhiteSpace(id)) return;
00075:             _bands.Add((id.Trim(), minInclusive, maxInclusive));
00076:         }
00077:
00078:         public void RegisterTellPool(TradeStance stance, string bandId, IEnumerable<string> lines)
00079:         {
00080:             if (string.IsNullOrWhiteSpace(bandId) || lines == null) return;
00081:             string key = PoolKey(stance, bandId.Trim());
00082:             var pool = new List<string>();
00083:             foreach (var line in lines)
00084:             {
00085:                 if (!string.IsNullOrWhiteSpace(line))
00086:                 {
00087:                     pool.Add(line.Trim());
00088:                 }
00089:             }
00090:             _pools[key] = pool;
00091:             _stances[key] = stance;
00092:             LineCount += pool.Count;
00093:         }
00094:
00095:         /// <summary>Ordered scan: first band whose [min, max] range contains the trust value.</summary>
00096:         public string BandForTrust(float trust)
00097:         {
00098:             foreach (var band in _bands)
00099:             {
00100:                 if (trust >= band.MinInclusive && trust <= band.MaxInclusive) return band.Id;
00101:             }
00102:             return _bands.Count > 0 ? _bands[_bands.Count - 1].Id : TradeTrustBands.Neutral;
00103:         }
00104:
00105:         public bool TrySelectTell(TradeStance stance, float trust, ISeededRng rng, out TradeTell tell)
00106:         {
00107:             string band = BandForTrust(trust);
00108:             string key = PoolKey(stance, band);
00109:             if (!_pools.TryGetValue(key, out var pool) || pool.Count == 0)
00110:             {
00111:                 tell = null!;
00112:                 return false;
00113:             }
00114:
00115:             int index = pool.Count <= 1 ? 0 : (rng != null ? rng.Next(0, pool.Count) : 0);
00116:             tell = new TradeTell(
00117:                 id: $"{StanceKey(stance)}_{band}_{index}",
00118:                 stance: stance,
00119:                 band: band,
00120:                 line: pool[index]);
00121:             return true;
00122:         }
00123:
00124:         /// <summary>Raw pool access for corpus lint/tests. Pool is keyed by stance + band id.</summary>
00125:         public bool TryGetPoolLines(TradeStance stance, string bandId, out IReadOnlyList<string> lines)
00126:         {
00127:             if (_pools.TryGetValue(PoolKey(stance, bandId ?? string.Empty), out var pool))
00128:             {
00129:                 lines = pool;
00130:                 return true;
00131:             }
00132:             lines = Array.Empty<string>();
00133:             return false;
00134:         }
00135:
00136:         private static string PoolKey(TradeStance stance, string bandId)
00137:         {
00138:             return StanceKey(stance) + "/" + (bandId ?? string.Empty).Trim().ToLowerInvariant();
00139:         }
00140:
00141:         private static string StanceKey(TradeStance stance)
00142:         {
00143:             switch (stance)
00144:             {
00145:                 case TradeStance.HostileRaid: return "hostile_raid";
00146:                 case TradeStance.Rob: return "rob";
00147:                 case TradeStance.Refuse: return "refuse";
00148:                 case TradeStance.ShareIntel: return "share_intel";
00149:                 case TradeStance.Trade:
00150:                 default: return "trade";
00151:             }
00152:         }
00153:
00154:         /// <summary>
00155:         /// Loads the tell corpus from raw JSON text:
00156:         /// { "trust_bands": [{id,min,max}...], "tells": { "trade": { "warm": [lines...] } } }
00157:         /// </summary>
00158:         public static TradeTellEngine LoadFromJson(string json)
00159:         {
00160:             var engine = new TradeTellEngine();
00161:             if (string.IsNullOrWhiteSpace(json)) return engine;
00162:
00163:             using var doc = JsonDocument.Parse(json);
00164:             var root = doc.RootElement;
00165:
00166:             if (root.TryGetProperty("trust_bands", out var bandsProp) && bandsProp.ValueKind == JsonValueKind.Array)
00167:             {
00168:                 foreach (var b in bandsProp.EnumerateArray())
00169:                 {
00170:                     string? id = b.TryGetProperty("id", out var idEl) ? idEl.GetString() : null;
00171:                     float min = b.TryGetProperty("min", out var minEl) ? (float)minEl.GetDouble()! : -100f;
00172:                     float max = b.TryGetProperty("max", out var maxEl) ? (float)maxEl.GetDouble() : 100f;
00173:                     engine.RegisterBand(id!, min, max);
00174:                 }
00175:             }
00176:
00177:             if (root.TryGetProperty("tells", out var tellsProp) && tellsProp.ValueKind == JsonValueKind.Object)
00178:             {
00179:                 foreach (var stanceProp in tellsProp.EnumerateObject())
00180:                 {
00181:                     if (!TryParseStance(stanceProp.Name, out var stance)) continue;
00182:                     if (stanceProp.Value.ValueKind != JsonValueKind.Object) continue;
00183:
00184:                     foreach (var bandProp in stanceProp.Value.EnumerateObject())
00185:                     {
00186:                         if (bandProp.Value.ValueKind != JsonValueKind.Array) continue;
00187:                         var lines = new List<string>();
00188:                         foreach (var line in bandProp.Value.EnumerateArray())
00189:                         {
00190:                             lines.Add(line.GetString() ?? string.Empty);
00191:                         }
00192:                         engine.RegisterTellPool(stance, bandProp.Name, lines);
00193:                     }
00194:                 }
00195:             }
00196:
00197:             return engine;
00198:         }
00199:
00200:         private static bool TryParseStance(string key, out TradeStance stance)
00201:         {
00202:             switch ((key ?? string.Empty).Trim())
00203:             {
00204:                 case "hostile_raid": stance = TradeStance.HostileRaid; return true;
00205:                 case "rob": stance = TradeStance.Rob; return true;
00206:                 case "refuse": stance = TradeStance.Refuse; return true;
00207:                 case "trade": stance = TradeStance.Trade; return true;
00208:                 case "share_intel": stance = TradeStance.ShareIntel; return true;
00209:                 default: stance = TradeStance.Trade; return false;
00210:             }
00211:         }
00212:     }
00213: }
```

## `src/Economy/TradeScreenGodotPanel.cs` — 1,005 lines; 45,453 bytes; SHA-256 `e1b1550b9352690b706cf5f7090f6fe4251b6293acbe40a80772533e4ac2452c`
Declaration index:
- 00025: public partial class TradeScreenGodotPanel : PanelContainer
- 00111: public void Open()
- 00118: public void Close()
- 00136: private void BuildLayout()
- 00382: private void BuildBioTradeRows()
- 00459: public void BindViewModel(ITradeScreenViewModel viewModel, ITradeIntentSink intentSink)
- 00474: private void OnViewModelChanged()
- 00479: public void BindSession(
- 00509: public void SetActiveFaction(string factionId)
- 00520: public void FocusLedgerSection(string sectionId)
- 00532: public void SetTraderVoiceContext(TradeVoiceContext context)
- 00546: public void AddPlayerOffer(string itemId, int count)
- 00553: public void AddFactionAsk(string itemId, int count)
- 00560: public void RefreshView()
- 00644: private void RefreshFromViewModel()
- 00735: private void RebuildShockBadges(IReadOnlyList<ShockBadgeData> badges)
- 00758: private static void ClearList(VBoxContainer list)
- 00763: private Label MakeTableLineLabel(string text, bool dimmed = false)
- 00771: private void UpdateArbitratorScale(float playerVal, float factionVal)
- 00780: private static Color GetFairnessColor(TradeFairness fairness)
- 00793: private void PopulateGoodsLists()
- 00856: private void RefreshCalculations()
- 00907: private void ExecuteTrade()
- 00934: private void DemandParley()
- 00958: private static Color ToGodotColor((float r, float g, float b, float a) token)
- 00963: private static Color GetStanceColor(TradeStance stance)
- 00980: private static string GetShockIconPath(PriceShockKind kind)
- 00994: private static string FormatWorthLabel(float value)
- 01000: private static Texture2D? LoadTexture(string path)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using AtomicWar.GodotApp.UI;
00003: using System;
00004: using System.Collections.Generic;
00005: #pragma warning disable CS8618
00006: using Godot;
00007: using static AtomicWar.GodotApp.UI.AshfallUiHelpers;
00008: using Ashfall.Core;
00009: using Ashfall.Core.Economy;
00010: using Ashfall.Core.Radio;
00011: namespace AtomicWar.GodotApp.Economy
00012: {
00013:
00014:     /// <summary>
00015:     /// Full Godot host implementation of the Ashfall Trade Screen &amp; Economy HUD.
00016:     /// Hits all fields:
00017:     /// - Header: Faction emblem, name, leader name, succession gen, stance badge, trust meter, aggression meter, repel count, parley readiness
00018:     /// - Market &amp; Shocks: World phase, day, active price shock badges with icons, scarcity tier badges
00019:     /// - Two-column Barter: Player offers (items, qualitative worth, biological offerings: blood, marrow, plasma, organ) and Faction asks
00020:     /// - Arbitrator: Deal fairness badge, value comparison, confirm button, parley button
00021:     /// - Radio Ticker: Intercept chatter and parley resolutions
00022:     ///
00023:     /// Follows strict token styling via Ashfall.Core.UI.Theme.
00024:     /// </summary>
00025:     public partial class TradeScreenGodotPanel : PanelContainer
00026:     {
00027:         // ── Data / Bindings ──────────────────────────────────────────
00028:         private EconomyHostSession _session;
00029:         private IFactionStanceProvider _stanceProvider;
00030:         private IPriceShockProvider _priceShockProvider;
00031:         private TradeVoiceResolver _voiceResolver;
00032:         private ITradeScreenViewModel _viewModel;
00033:         private ITradeIntentSink _intentSink;
00034:
00035:         private string _activeFactionId = "scavenger_camp";
00036:         private string _activeTraderProfileId = string.Empty;
00037:         private readonly Dictionary<string, int> _playerOfferCounts = new();
00038:         private readonly Dictionary<string, int> _factionAskCounts = new();
00039:         private readonly Dictionary<BiologicalTradeItem, int> _bioOfferCounts = new();
00040:         private readonly Dictionary<BiologicalTradeItem, Label> _bioCountLabels = new();
00041:         private readonly List<Button> _bioStepperButtons = new();
00042:
00043:         // ── UI Controls ──────────────────────────────────────────────
00044:         // Header
00045:         private TextureRect _textureFactionEmblem;
00046:         private Label _lblFactionName;
00047:         private Label _lblLeader;
00048:         private Label _badgeStance;
00049:         private Label _lblTellPlate;
00050:         private Label _lblTraderVoice;
00051:         private Label _lblTrust;
00052:         private Label _lblAggression;
00053:         private Label _lblRepels;
00054:         private Label _lblParleyStatus;
00055:
00056:         // News from Outside strip (world phase/day + shocks + scarcity)
00057:         private Label _lblPhaseDay;
00058:         private HBoxContainer _newsStrip;
00059:         private HBoxContainer _shocksContainer;
00060:         private Label _lblScarcitySummary;
00061:
00062:         // Columns
00063:         private VBoxContainer _playerOfferList;
00064:         private Label _lblPlayerWorth;
00065:         private VBoxContainer _bioTradeContainer;
00066:         private readonly List<HBoxContainer> _bioTradeRows = new();
00067:
00068:         private VBoxContainer _factionStockList;
00069:         private Label _lblFactionAskWorth;
00070:
00071:         // Grim drawer (biological trading, collapsed by default)
00072:         private Button _grimDrawerToggle;
00073:         private VBoxContainer _grimDrawerBody;
00074:
00075:         // Arbitrator
00076:         private Label _lblFairness;
00077:         private ColorRect _scalePlayerFill;
00078:         private ColorRect _scaleFactionFill;
00079:         private Button _btnConfirmTrade;
00080:         private Button _btnDemandParley;
00081:
00082:         // Radio Ticker
00083:         private Label _lblRadioTicker;
00084:
00085:         // ── Probing / Verification Surface ───────────────────────────
00086:         public bool HasFactionEmblem => _textureFactionEmblem?.Texture != null;
00087:         public bool HasLeaderLabel => !string.IsNullOrEmpty(_lblLeader?.Text);
00088:         public bool HasStanceBadge => !string.IsNullOrEmpty(_badgeStance?.Text);
00089:         public bool HasTrustMeter => !string.IsNullOrEmpty(_lblTrust?.Text);
00090:         public bool HasAggressionMeter => !string.IsNullOrEmpty(_lblAggression?.Text);
00091:         public bool HasRepelCounter => !string.IsNullOrEmpty(_lblRepels?.Text);
00092:         public bool HasPriceShockBanner => _shocksContainer != null;
00093:         public bool HasBioTradeRows => _bioTradeRows.Count >= 4;
00094:         public bool HasFairnessIndicator => !string.IsNullOrEmpty(_lblFairness?.Text);
00095:         public bool HasParleyButton => _btnDemandParley != null;
00096:         public bool HasRadioTicker => _lblRadioTicker != null;
00097:         public bool HasTellPlate => !string.IsNullOrEmpty(_lblTellPlate?.Text);
00098:         public bool HasTraderVoice => !string.IsNullOrEmpty(_lblTraderVoice?.Text);
00099:         public string TraderProfileId => _viewModel?.TraderProfileId ?? _activeTraderProfileId;
00100:         public bool HasNewsStrip => _newsStrip != null && _newsStrip.GetChildCount() > 0;
00101:         public bool HasArbitratorScale => _scalePlayerFill != null && _scaleFactionFill != null;
00102:         public bool IsGrimDrawerCollapsed => _grimDrawerBody == null || !_grimDrawerBody.Visible;
00103:         public bool IsViewModelBound => _viewModel != null;
00104:         public int ActiveOfferCount => _playerOfferCounts.Count;
00105:         public int ActiveAskCount => _factionAskCounts.Count;
00106:         public int ActiveBioCount => _bioOfferCounts.Count;
00107:
00108:         /// <summary>Raised when the overlay requests dismissal (host wires Escape/close).</summary>
00109:         public event Action? OnClose;
00110:
00111:         public void Open()
00112:         {
00113:             Visible = true;
00114:             RefreshView();
00115:             QueueRedraw();
00116:         }
00117:
00118:         public void Close()
00119:         {
00120:             Visible = false;
00121:             OnClose?.Invoke();
00122:         }
00123:
00124:         public override void _Ready()
00125:         {
00126:             Visible = false;
00127:             SetAnchorsPreset(LayoutPreset.FullRect);
00128:             CustomMinimumSize = new Vector2(global::Ashfall.Core.UI.Theme.TradePanelMinWidth, global::Ashfall.Core.UI.Theme.TradePanelMaxHeight);
00129:
00130:             // Apply 9-slice panel background via shared helper (frame_9slice first)
00131:             AddThemeStyleboxOverride("panel", MakePanelFrameStyleBox());
00132:
00133:             BuildLayout();
00134:         }
00135:
00136:         private void BuildLayout()
00137:         {
00138:             var mainVbox = new VBoxContainer();
00139:             mainVbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);
00140:             AddChild(mainVbox);
00141:
00142:             // 1. Header Bar (9-slice via shared helper; falls back gracefully)
00143:             var headerContainer = new PanelContainer();
00144:             headerContainer.AddThemeStyleboxOverride("panel", MakeHeaderFrameStyleBox());
00145:
00146:             var headerHbox = new HBoxContainer();
00147:             headerHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingMd);
00148:             headerContainer.AddChild(headerHbox);
00149:
00150:             // Faction Emblem
00151:             _textureFactionEmblem = new TextureRect
00152:             {
00153:                 CustomMinimumSize = new Vector2(40, 40),
00154:                 StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
00155:             };
00156:             headerHbox.AddChild(_textureFactionEmblem);
00157:
00158:             // Faction & Leader Info
00159:             var titleVbox = new VBoxContainer();
00160:             _lblFactionName = new Label
00161:             {
00162:                 Text = "FACTION: SCAVENGER CAMP",
00163:                 HorizontalAlignment = HorizontalAlignment.Left
00164:             };
00165:             _lblFactionName.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeH3);
00166:             _lblFactionName.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
00167:             titleVbox.AddChild(_lblFactionName);
00168:
00169:             _lblLeader = new Label
00170:             {
00171:                 Text = "Leader: Varek (gen 1)",
00172:                 HorizontalAlignment = HorizontalAlignment.Left
00173:             };
00174:             _lblLeader.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
00175:             _lblLeader.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
00176:             titleVbox.AddChild(_lblLeader);
00177:             headerHbox.AddChild(titleVbox);
00178:
00179:             headerHbox.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });
00180:
00181:             // Stance & Trust Meters
00182:             var statusVbox = new VBoxContainer();
00183:             statusVbox.Alignment = BoxContainer.AlignmentMode.End;
00184:
00185:             var statusHbox = new HBoxContainer();
00186:             statusHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);
00187:
00188:             _badgeStance = new Label { Text = "[ STANCE: TRADE ]" };
00189:             _badgeStance.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeBody);
00190:             _badgeStance.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Hot));
00191:             statusHbox.AddChild(_badgeStance);
00192:
00193:             _lblTrust = new Label { Text = "Trust: +10" };
00194:             _lblTrust.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
00195:             _lblTrust.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
00196:             statusHbox.AddChild(_lblTrust);
00197:             statusVbox.AddChild(statusHbox);
00198:
00199:             var metaHbox = new HBoxContainer();
00200:             metaHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);
00201:
00202:             _lblAggression = new Label { Text = "Aggression: 0.40" };
00203:             _lblAggression.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
00204:             _lblAggression.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
00205:             metaHbox.AddChild(_lblAggression);
00206:
00207:             _lblRepels = new Label { Text = "Holds: x0" };
00208:             _lblRepels.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
00209:             _lblRepels.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
00210:             metaHbox.AddChild(_lblRepels);
00211:
00212:             _lblParleyStatus = new Label { Text = "" };
00213:             _lblParleyStatus.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
00214:             _lblParleyStatus.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
00215:             metaHbox.AddChild(_lblParleyStatus);
00216:
00217:             statusVbox.AddChild(metaHbox);
00218:             headerHbox.AddChild(statusVbox);
00219:             mainVbox.AddChild(headerContainer);
00220:
00221:             // 1b. Tell Plate — the trader's readable posture (data-defined line)
00222:             _lblTellPlate = new Label
00223:             {
00224:                 Text = "",
00225:                 HorizontalAlignment = HorizontalAlignment.Left,
00226:                 AutowrapMode = TextServer.AutowrapMode.WordSmart
00227:             };
00228:             _lblTellPlate.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
00229:             _lblTellPlate.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
00230:             mainVbox.AddChild(_lblTellPlate);
00231:
00232:             _lblTraderVoice = new Label
00233:             {
00234:                 Text = "",
00235:                 HorizontalAlignment = HorizontalAlignment.Left,
00236:                 AutowrapMode = TextServer.AutowrapMode.WordSmart
00237:             };
00238:             _lblTraderVoice.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
00239:             _lblTraderVoice.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
00240:             mainVbox.AddChild(_lblTraderVoice);
00241:
00242:             // 2. News from Outside strip — the world intruding on the deal
00243:             _newsStrip = new HBoxContainer();
00244:             _newsStrip.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingMd);
00245:
00246:             var lblNewsTitle = new Label { Text = "NEWS FROM OUTSIDE ·" };
00247:             lblNewsTitle.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
00248:             lblNewsTitle.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Dim));
00249:             _newsStrip.AddChild(lblNewsTitle);
00250:
00251:             _lblPhaseDay = new Label { Text = "Phase: CivilWar · Day 1" };
00252:             _lblPhaseDay.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
00253:             _lblPhaseDay.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
00254:             _newsStrip.AddChild(_lblPhaseDay);
00255:
00256:             _shocksContainer = new HBoxContainer();
00257:             _shocksContainer.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);
00258:             _newsStrip.AddChild(_shocksContainer);
00259:
00260:             _lblScarcitySummary = new Label { Text = "Scarcity: Normal" };
00261:             _lblScarcitySummary.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
00262:             _lblScarcitySummary.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
00263:             _newsStrip.AddChild(_lblScarcitySummary);
00264:
00265:             mainVbox.AddChild(_newsStrip);
00266:
00267:             // 3. Two-Column Barter Body
00268:             var columnsHbox = new HBoxContainer();
00269:             columnsHbox.SizeFlagsVertical = SizeFlags.ExpandFill;
00270:             columnsHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingMd);
00271:
00272:             // Left Column: Player Offers
00273:             var playerCol = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
00274:             var lblOfferTitle = new Label { Text = "YOUR OFFER" };
00275:             lblOfferTitle.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeH3);
00276:             lblOfferTitle.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
00277:             playerCol.AddChild(lblOfferTitle);
00278:
00279:             var playerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, CustomMinimumSize = new Vector2(0, 140) };
00280:             _playerOfferList = new VBoxContainer();
00281:             playerScroll.AddChild(_playerOfferList);
00282:             playerCol.AddChild(playerScroll);
00283:
00284:             _lblPlayerWorth = new Label { Text = "Offer Worth: None" };
00285:             _lblPlayerWorth.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
00286:             _lblPlayerWorth.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
00287:             playerCol.AddChild(_lblPlayerWorth);
00288:
00289:             // Biological Trade — the grim drawer beneath the table.
00290:             // Collapsed by default; deliberately unsettling, never required.
00291:             _grimDrawerToggle = new Button
00292:             {
00293:                 Text = "▸ BIOLOGICAL — THE DRAWER",
00294:                 ToggleMode = true,
00295:                 ButtonPressed = false
00296:             };
00297:             _grimDrawerToggle.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
00298:             _grimDrawerToggle.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Critical));
00299:             _grimDrawerToggle.Toggled += on =>
00300:             {
00301:                 _grimDrawerBody.Visible = on;
00302:                 _grimDrawerToggle.Text = on ? "▾ BIOLOGICAL — THE DRAWER" : "▸ BIOLOGICAL — THE DRAWER";
00303:             };
00304:             playerCol.AddChild(_grimDrawerToggle);
00305:
00306:             _grimDrawerBody = new VBoxContainer { Visible = false };
00307:             playerCol.AddChild(_grimDrawerBody);
00308:
00309:             _bioTradeContainer = new VBoxContainer();
00310:             BuildBioTradeRows();
00311:             _grimDrawerBody.AddChild(_bioTradeContainer);
00312:
00313:             columnsHbox.AddChild(playerCol);
00314:
00315:             // Right Column: Faction Stock / Demands
00316:             var factionCol = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
00317:             var lblFactionTitle = new Label { Text = "THEIR STOCK & ASKS" };
00318:             lblFactionTitle.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeH3);
00319:             lblFactionTitle.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
00320:             factionCol.AddChild(lblFactionTitle);
00321:
00322:             var factionScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, CustomMinimumSize = new Vector2(0, 140) };
00323:             _factionStockList = new VBoxContainer();
00324:             factionScroll.AddChild(_factionStockList);
00325:             factionCol.AddChild(factionScroll);
00326:
00327:             _lblFactionAskWorth = new Label { Text = "Demand Worth: None" };
00328:             _lblFactionAskWorth.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
00329:             _lblFactionAskWorth.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
00330:             factionCol.AddChild(_lblFactionAskWorth);
00331:
00332:             columnsHbox.AddChild(factionCol);
00333:             mainVbox.AddChild(columnsHbox);
00334:
00335:             // 4. Center Arbitrator & Buttons
00336:             var arbitratorHbox = new HBoxContainer();
00337:             arbitratorHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingMd);
00338:
00339:             // The arbitrator's scale: balance bar etched at table center.
00340:             // The explicit FAIR/SHORT label is retained (gates check the label).
00341:             _scalePlayerFill = new ColorRect
00342:             {
00343:                 Color = ToGodotColor(global::Ashfall.Core.UI.Theme.Hot),
00344:                 CustomMinimumSize = new Vector2(70, 8)
00345:             };
00346:             _scaleFactionFill = new ColorRect
00347:             {
00348:                 Color = ToGodotColor(global::Ashfall.Core.UI.Theme.Muted),
00349:                 CustomMinimumSize = new Vector2(70, 8)
00350:             };
00351:             arbitratorHbox.AddChild(_scalePlayerFill);
00352:             arbitratorHbox.AddChild(_scaleFactionFill);
00353:
00354:             _lblFairness = new Label { Text = "DEAL IS FAIR" };
00355:             _lblFairness.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeH3);
00356:             _lblFairness.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Hot));
00357:             arbitratorHbox.AddChild(_lblFairness);
00358:
00359:             arbitratorHbox.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });
00360:
00361:             _btnDemandParley = new Button { Text = "DEMAND PARLEY [P]", Visible = false };
00362:             _btnDemandParley.Pressed += () => DemandParley();
00363:             arbitratorHbox.AddChild(_btnDemandParley);
00364:
00365:             _btnConfirmTrade = new Button { Text = "CONFIRM BARTER" };
00366:             _btnConfirmTrade.Pressed += () => ExecuteTrade();
00367:             arbitratorHbox.AddChild(_btnConfirmTrade);
00368:
00369:             mainVbox.AddChild(arbitratorHbox);
00370:
00371:             // 5. Bottom Radio Ticker
00372:             _lblRadioTicker = new Label
00373:             {
00374:                 Text = "RADIO: Monitoring frequency 104.7 MHz...",
00375:                 HorizontalAlignment = HorizontalAlignment.Left
00376:             };
00377:             _lblRadioTicker.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
00378:             _lblRadioTicker.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Dim));
00379:             mainVbox.AddChild(_lblRadioTicker);
00380:         }
00381:
00382:         private void BuildBioTradeRows()
00383:         {
00384:             _bioTradeRows.Clear();
00385:             _bioCountLabels.Clear();
00386:             _bioStepperButtons.Clear();
00387:             var bioItems = new[]
00388:             {
00389:                 (BiologicalTradeItem.PintOfBlood, "Pint of Blood", "icon_bio_blood.png"),
00390:                 (BiologicalTradeItem.BoneMarrow, "Bone Marrow", "icon_bio_marrow.png"),
00391:                 (BiologicalTradeItem.Plasma, "Plasma", "icon_bio_plasma.png"),
00392:                 (BiologicalTradeItem.Organ, "Organ", "icon_bio_organ.png")
00393:             };
00394:
00395:             foreach (var (bioKind, name, iconFile) in bioItems)
00396:             {
00397:                 var row = new HBoxContainer();
00398:                 row.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingXs);
00399:
00400:                 var iconRect = new TextureRect
00401:                 {
00402:                     CustomMinimumSize = new Vector2(20, 20),
00403:                     StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
00404:                     Texture = LoadTexture($"res://assets/ui/Icons/{iconFile}")
00405:                 };
00406:                 row.AddChild(iconRect);
00407:
00408:                 var lbl = new Label { Text = name, CustomMinimumSize = new Vector2(100, 0) };
00409:                 lbl.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
00410:                 lbl.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
00411:                 row.AddChild(lbl);
00412:
00413:                 var btnMinus = new Button { Text = "-" };
00414:                 var btnPlus = new Button { Text = "+" };
00415:                 var countLbl = new Label { Text = "0", CustomMinimumSize = new Vector2(20, 0), HorizontalAlignment = HorizontalAlignment.Center };
00416:                 countLbl.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
00417:
00418:                 btnMinus.Pressed += () =>
00419:                 {
00420:                     int cur = _bioOfferCounts.GetValueOrDefault(bioKind, 0);
00421:                     if (cur > 0)
00422:                     {
00423:                         _bioOfferCounts[bioKind] = cur - 1;
00424:                         countLbl.Text = (cur - 1).ToString();
00425:                         RefreshCalculations();
00426:                     }
00427:                 };
00428:
00429:                 btnPlus.Pressed += () =>
00430:                 {
00431:                     int cur = _bioOfferCounts.GetValueOrDefault(bioKind, 0);
00432:                     _bioOfferCounts[bioKind] = cur + 1;
00433:                     countLbl.Text = (cur + 1).ToString();
00434:                     RefreshCalculations();
00435:                 };
00436:
00437:                 row.AddChild(btnMinus);
00438:                 row.AddChild(countLbl);
00439:                 row.AddChild(btnPlus);
00440:
00441:                 _bioTradeContainer.AddChild(row);
00442:                 _bioTradeRows.Add(row);
00443:                 _bioCountLabels[bioKind] = countLbl;
00444:                 _bioStepperButtons.Add(btnMinus);
00445:                 _bioStepperButtons.Add(btnPlus);
00446:             }
00447:         }
00448:
00449:         // ── Binding & Lifecycle ──────────────────────────────────────
00450:
00451:         private IFactionRadioProvider _radioProvider;
00452:         private ISeededRng _rng;
00453:
00454:         /// <summary>
00455:         /// Skin-track / convergence binding: paint the staged table from the
00456:         /// Act 0 seam. When bound, this takes priority over the session binding
00457:         /// and the panel never touches simulation types.
00458:         /// </summary>
00459:         public void BindViewModel(ITradeScreenViewModel viewModel, ITradeIntentSink intentSink)
00460:         {
00461:             if (_viewModel != null)
00462:             {
00463:                 _viewModel.Changed -= OnViewModelChanged;
00464:             }
00465:             _viewModel = viewModel;
00466:             _intentSink = intentSink;
00467:             if (_viewModel != null)
00468:             {
00469:                 _viewModel.Changed += OnViewModelChanged;
00470:             }
00471:             OnViewModelChanged();
00472:         }
00473:
00474:         private void OnViewModelChanged()
00475:         {
00476:             RefreshView();
00477:         }
00478:
00479:         public void BindSession(
00480:             EconomyHostSession session,
00481:             IFactionStanceProvider stanceProvider = null!,
00482:             IPriceShockProvider priceShockProvider = null!,
00483:             IFactionRadioProvider radioProvider = null!,
00484:             ISeededRng rng = null!,
00485:             TradeVoiceResolver voiceResolver = null!)
00486:         {
00487:             _session = session;
00488:             _stanceProvider = stanceProvider;
00489:             _priceShockProvider = priceShockProvider;
00490:             _radioProvider = radioProvider;
00491:             _rng = rng ?? new SeededRng(2026);
00492:             _voiceResolver = voiceResolver;
00493:
00494:             if (_viewModel != null)
00495:             {
00496:                 _viewModel.Changed -= OnViewModelChanged;
00497:                 _viewModel = null!;
00498:                 _intentSink = null!;
00499:             }
00500:
00501:             if (_session != null)
00502:             {
00503:                 _session.StateChanged += RefreshView;
00504:             }
00505:
00506:             RefreshView();
00507:         }
00508:
00509:         public void SetActiveFaction(string factionId)
00510:         {
00511:             _activeFactionId = factionId;
00512:             RefreshView();
00513:         }
00514:
00515:         /// <summary>
00516:         /// Caravan ledger sidebar focus helper. Section ids are local chrome ops
00517:         /// (not faction ids). Biology expands the existing grim drawer; other
00518:         /// sections currently have no dedicated scroll target.
00519:         /// </summary>
00520:         public void FocusLedgerSection(string sectionId)
00521:         {
00522:             if (string.IsNullOrEmpty(sectionId)) return;
00523:             if (string.Equals(sectionId, "biology", StringComparison.Ordinal))
00524:             {
00525:                 if (_grimDrawerToggle != null && !_grimDrawerToggle.ButtonPressed)
00526:                     _grimDrawerToggle.ButtonPressed = true;
00527:                 if (_grimDrawerBody != null)
00528:                     _grimDrawerBody.Visible = true;
00529:             }
00530:         }
00531:
00532:         public void SetTraderVoiceContext(TradeVoiceContext context)
00533:         {
00534:             if (_voiceResolver == null) return;
00535:             var result = _voiceResolver.ResolveGreeting(context);
00536:             _activeTraderProfileId = result.ProfileId;
00537:             string displayName = _voiceResolver.Catalog.TryGetTrader(
00538:                 result.ProfileId,
00539:                 out var trader)
00540:                 ? trader.display_name
00541:                 : "The Merchant";
00542:             if (_lblTraderVoice != null)
00543:                 _lblTraderVoice.Text = $"{displayName}: {result.Text}";
00544:         }
00545:
00546:         public void AddPlayerOffer(string itemId, int count)
00547:         {
00548:             if (count <= 0) _playerOfferCounts.Remove(itemId);
00549:             else _playerOfferCounts[itemId] = count;
00550:             RefreshCalculations();
00551:         }
00552:
00553:         public void AddFactionAsk(string itemId, int count)
00554:         {
00555:             if (count <= 0) _factionAskCounts.Remove(itemId);
00556:             else _factionAskCounts[itemId] = count;
00557:             RefreshCalculations();
00558:         }
00559:
00560:         public void RefreshView()
00561:         {
00562:             if (_viewModel != null)
00563:             {
00564:                 RefreshFromViewModel();
00565:                 return;
00566:             }
00567:             if (_session == null) return;
00568:
00569:             // The tell plate is seam-driven; clear it in session mode.
00570:             if (_lblTellPlate != null) _lblTellPlate.Text = "";
00571:             if (_lblTraderVoice != null) _lblTraderVoice.Text = "";
00572:
00573:             // 1. Update Header Fields
00574:             _lblFactionName.Text = $"FACTION: {_activeFactionId.ToUpper().Replace('_', ' ')}";
00575:             _textureFactionEmblem.Texture = AtomicWar.GodotApp.FactionIconLoader.LoadFor(_activeFactionId);
00576:
00577:             float trust = _stanceProvider?.GetEffectiveTrust(_activeFactionId) ?? 0f;
00578:             var stance = _stanceProvider?.GetStance(_activeFactionId) ?? TradeStance.Trade;
00579:             float aggression = _stanceProvider?.GetRaidAggression(_activeFactionId) ?? 0.5f;
00580:
00581:             _lblLeader.Text = $"Leader: {_activeFactionId} Commander";
00582:             _badgeStance.Text = $"[ STANCE: {stance.ToString().ToUpper()} ]";
00583:             _badgeStance.AddThemeColorOverride("font_color", GetStanceColor(stance));
00584:
00585:             _lblTrust.Text = $"Trust: {trust:+0;-0;0}";
00586:             _lblAggression.Text = $"Aggression: {aggression:0.00}";
00587:             _lblRepels.Text = "Holds: x0";
00588:             if (_voiceResolver != null)
00589:             {
00590:                 SetTraderVoiceContext(new TradeVoiceContext
00591:                 {
00592:                     FactionId = _activeFactionId,
00593:                     Stance = stance,
00594:                     Trust = trust
00595:                 });
00596:             }
00597:
00598:             // 2. Update Market & Shocks Banner
00599:             int day = _session.Market?.Day ?? 1;
00600:             _lblPhaseDay.Text = $"Phase: CivilWar · Day {day}";
00601:
00602:             AshfallUiHelpers.EmptyChildren(_shocksContainer);
00603:
00604:             if (_priceShockProvider != null)
00605:             {
00606:                 var kinds = new[]
00607:                 {
00608:                     PriceShockKind.PlumePassing,
00609:                     PriceShockKind.ConvoyAmbush,
00610:                     PriceShockKind.FactionConflict,
00611:                     PriceShockKind.SeasonalScarcity,
00612:                     PriceShockKind.DiseaseOutbreak,
00613:                     PriceShockKind.FuelShortage
00614:                 };
00615:                 var activeBadges = new List<ShockBadgeData>();
00616:                 foreach (var k in kinds)
00617:                 {
00618:                     if (_priceShockProvider.TryGetPriceShock(k, day, out var rule))
00619:                     {
00620:                         activeBadges.Add(new ShockBadgeData(rule.Kind, rule.Multiplier, rule.Trigger));
00621:                     }
00622:                 }
00623:                 RebuildShockBadges(activeBadges);
00624:             }
00625:
00626:             // 3. Populate Stock Lists
00627:             PopulateGoodsLists();
00628:
00629:             // 4. Update Arbitrator
00630:             RefreshCalculations();
00631:
00632:             // 5. Update Radio Ticker
00633:             if (_radioProvider != null && _lblRadioTicker != null)
00634:             {
00635:                 var intercept = _radioProvider.GetFactionEvent(_activeFactionId, RadioEventKind.InterceptChatter, day, _rng);
00636:                 _lblRadioTicker.Text = $"RADIO: [{intercept.Callsign}] {intercept.Message}";
00637:             }
00638:         }
00639:
00640:         /// <summary>
00641:         /// Paints the staged table purely from the Act 0 seam. Reads only;
00642:         /// zero simulation types touched.
00643:         /// </summary>
00644:         private void RefreshFromViewModel()
00645:         {
00646:             var vm = _viewModel;
00647:
00648:             // 1. Header — the trader's presence
00649:             _lblFactionName.Text = vm.FactionName.Length > 0
00650:                 ? $"FACTION: {vm.FactionName.ToUpperInvariant()}"
00651:                 : "FACTION: —";
00652:             _textureFactionEmblem.Texture = AtomicWar.GodotApp.FactionIconLoader.LoadFor(vm.FactionId);
00653:             _lblLeader.Text = $"Leader: {vm.LeaderName} (gen {vm.SuccessionGeneration})";
00654:             _badgeStance.Text = vm.StanceBadgeText;
00655:             _badgeStance.AddThemeColorOverride("font_color", GetStanceColor(vm.Stance));
00656:
00657:             // 2. Tell plate — posture readable from the plate alone
00658:             _lblTellPlate.Text = vm.StanceTellLine;
00659:             _lblTraderVoice.Text = string.IsNullOrWhiteSpace(vm.TraderVoiceLine)
00660:                 ? string.Empty
00661:                 : $"{vm.TraderDisplayName}: {vm.TraderVoiceLine}";
00662:
00663:             // 3. Ledger-edge meters
00664:             _lblTrust.Text = $"Trust: {vm.Trust:+0;-0;0}";
00665:             _lblAggression.Text = $"Aggression: {vm.Aggression:0.00}";
00666:             _lblRepels.Text = $"Holds: x{vm.ConsecutiveRepels}";
00667:             _lblParleyStatus.Text = vm.HasSurrendered
00668:                 ? "[S] SURRENDERED"
00669:                 : (vm.CanDemandParley ? "[P] PARLEY READY" : "");
00670:
00671:             // 4. News from Outside strip
00672:             _lblPhaseDay.Text = $"Phase: {vm.WorldPhaseLabel} · Day {vm.WorldDay}";
00673:             RebuildShockBadges(vm.ShockBadges);
00674:             if (vm.ScarcityMultipliers.Count > 0)
00675:             {
00676:                 var parts = new List<string>();
00677:                 foreach (var band in vm.ScarcityMultipliers)
00678:                 {
00679:                     parts.Add($"{band.DisplayName} x{band.Multiplier:0.0}");
00680:                 }
00681:                 _lblScarcitySummary.Text = $"Scarcity: {string.Join(", ", parts)}";
00682:             }
00683:             else
00684:             {
00685:                 _lblScarcitySummary.Text = "Scarcity: Normal";
00686:             }
00687:
00688:             // 5. Table edges — what you push across vs what they guard
00689:             ClearList(_playerOfferList);
00690:             ClearList(_factionStockList);
00691:             foreach (var line in vm.PlayerOffers)
00692:             {
00693:                 _playerOfferList.AddChild(MakeTableLineLabel($"{line.Quantity}x {line.DisplayName} — {line.WorthLabel}"));
00694:             }
00695:             foreach (var line in vm.FactionDemands)
00696:             {
00697:                 _factionStockList.AddChild(MakeTableLineLabel($"{line.Quantity}x {line.DisplayName} — {line.WorthLabel}"));
00698:             }
00699:             if (vm.PlayerOffers.Count == 0 && vm.BiologicalOffers.Count == 0)
00700:             {
00701:                 _playerOfferList.AddChild(MakeTableLineLabel("— your edge of the table is bare —", dimmed: true));
00702:             }
00703:             if (vm.FactionDemands.Count == 0)
00704:             {
00705:                 _factionStockList.AddChild(MakeTableLineLabel("— their edge of the table is bare —", dimmed: true));
00706:             }
00707:
00708:             int bioCount = 0;
00709:             foreach (var pair in vm.BiologicalOffers) bioCount += pair.Value;
00710:             _lblPlayerWorth.Text = $"Offer Worth: {Ashfall.Core.Economy.TradeWorthLabels.Format(vm.PlayerOfferValue)} ({vm.PlayerOffers.Count} items, {bioCount} bio)";
00711:             _lblFactionAskWorth.Text = $"Demand Worth: {Ashfall.Core.Economy.TradeWorthLabels.Format(vm.FactionAskValue)} ({vm.FactionDemands.Count} items)";
00712:
00713:             // 6. Grim drawer — counts shown, steppers are session-mode controls
00714:             foreach (var pair in _bioCountLabels)
00715:             {
00716:                 pair.Value.Text = vm.BiologicalOffers.TryGetValue(pair.Key, out int count) ? count.ToString() : "0";
00717:             }
00718:             bool steppersVisible = _session != null;
00719:             foreach (var stepper in _bioStepperButtons)
00720:             {
00721:                 stepper.Visible = steppersVisible;
00722:             }
00723:
00724:             // 7. Arbitrator — diegetic scale + explicit label (gates check the label)
00725:             _lblFairness.Text = vm.FairnessLabel;
00726:             _lblFairness.AddThemeColorOverride("font_color", GetFairnessColor(vm.Fairness));
00727:             _btnConfirmTrade.Disabled = !vm.CanConfirm;
00728:             _btnDemandParley.Visible = vm.CanDemandParley;
00729:             UpdateArbitratorScale(vm.PlayerOfferValue, vm.FactionAskValue);
00730:
00731:             // 8. Radio — the room's radio, unchanged position
00732:             _lblRadioTicker.Text = vm.RadioTickerLine;
00733:         }
00734:
00735:         private void RebuildShockBadges(IReadOnlyList<ShockBadgeData> badges)
00736:         {
00737:             AshfallUiHelpers.EmptyChildren(_shocksContainer);
00738:
00739:             if (badges == null) return;
00740:             foreach (var badge in badges)
00741:             {
00742:                 var shockBadge = new HBoxContainer();
00743:                 var icon = new TextureRect
00744:                 {
00745:                     CustomMinimumSize = new Vector2(16, 16),
00746:                     StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
00747:                     Texture = LoadTexture(GetShockIconPath(badge.Kind))
00748:                 };
00749:                 shockBadge.AddChild(icon);
00750:                 var lbl = new Label { Text = $"{badge.Kind} (x{badge.Multiplier:0.0})" };
00751:                 lbl.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
00752:                 lbl.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
00753:                 shockBadge.AddChild(lbl);
00754:                 _shocksContainer.AddChild(shockBadge);
00755:             }
00756:         }
00757:
00758:         private static void ClearList(VBoxContainer list)
00759:         {
00760:             AshfallUiHelpers.EmptyChildren(list);
00761:         }
00762:
00763:         private Label MakeTableLineLabel(string text, bool dimmed = false)
00764:         {
00765:             var lbl = new Label { Text = text };
00766:             lbl.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
00767:             lbl.AddThemeColorOverride("font_color", ToGodotColor(dimmed ? global::Ashfall.Core.UI.Theme.Dim : global::Ashfall.Core.UI.Theme.Pale));
00768:             return lbl;
00769:         }
00770:
00771:         private void UpdateArbitratorScale(float playerVal, float factionVal)
00772:         {
00773:             float total = playerVal + factionVal;
00774:             float ratio = total > 0f ? playerVal / total : 0.5f;
00775:             const float width = 140f;
00776:             _scalePlayerFill.CustomMinimumSize = new Vector2(Mathf.Max(6f, width * ratio), 8);
00777:             _scaleFactionFill.CustomMinimumSize = new Vector2(Mathf.Max(6f, width * (1f - ratio)), 8);
00778:         }
00779:
00780:         private static Color GetFairnessColor(TradeFairness fairness)
00781:         {
00782:             switch (fairness)
00783:             {
00784:                 case TradeFairness.Fair:
00785:                     return ToGodotColor(global::Ashfall.Core.UI.Theme.Hot);
00786:                 case TradeFairness.Short:
00787:                     return ToGodotColor(global::Ashfall.Core.UI.Theme.Critical);
00788:                 default:
00789:                     return ToGodotColor(global::Ashfall.Core.UI.Theme.Dim);
00790:             }
00791:         }
00792:
00793:         private void PopulateGoodsLists()
00794:         {
00795:             if (_session?.Catalog == null) return;
00796:
00797:             AshfallUiHelpers.EmptyChildren(_playerOfferList);
00798:             AshfallUiHelpers.EmptyChildren(_factionStockList);
00799:
00800:             foreach (var good in _session.Catalog.All())
00801:             {
00802:                 // Offer row
00803:                 var offerRow = new HBoxContainer();
00804:                 offerRow.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);
00805:
00806:                 var icon1 = new TextureRect
00807:                 {
00808:                     CustomMinimumSize = new Vector2(24, 24),
00809:                     StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
00810:                     Texture = AssetRegistry.GetItem(good.id).Texture
00811:                 };
00812:                 offerRow.AddChild(icon1);
00813:
00814:                 var lblGood1 = new Label { Text = good.displayName, CustomMinimumSize = new Vector2(120, 0) };
00815:                 lblGood1.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
00816:                 offerRow.AddChild(lblGood1);
00817:
00818:                 var btnAddOffer = new Button { Text = "+ Offer" };
00819:                 string gId = good.id;
00820:                 btnAddOffer.Pressed += () =>
00821:                 {
00822:                     int cur = _playerOfferCounts.GetValueOrDefault(gId, 0);
00823:                     AddPlayerOffer(gId, cur + 1);
00824:                 };
00825:                 offerRow.AddChild(btnAddOffer);
00826:                 _playerOfferList.AddChild(offerRow);
00827:
00828:                 // Ask row
00829:                 var askRow = new HBoxContainer();
00830:                 askRow.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);
00831:
00832:                 var icon2 = new TextureRect
00833:                 {
00834:                     CustomMinimumSize = new Vector2(24, 24),
00835:                     StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
00836:                     Texture = AssetRegistry.GetItem(good.id).Texture
00837:                 };
00838:                 askRow.AddChild(icon2);
00839:
00840:                 float price = _session.Market.GetPrice(good.id);
00841:                 var lblGood2 = new Label { Text = $"{good.displayName} ({price:0.00})", CustomMinimumSize = new Vector2(140, 0) };
00842:                 lblGood2.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
00843:                 askRow.AddChild(lblGood2);
00844:
00845:                 var btnAddAsk = new Button { Text = "+ Ask" };
00846:                 btnAddAsk.Pressed += () =>
00847:                 {
00848:                     int cur = _factionAskCounts.GetValueOrDefault(gId, 0);
00849:                     AddFactionAsk(gId, cur + 1);
00850:                 };
00851:                 askRow.AddChild(btnAddAsk);
00852:                 _factionStockList.AddChild(askRow);
00853:             }
00854:         }
00855:
00856:         private void RefreshCalculations()
00857:         {
00858:             float playerVal = 0f;
00859:             foreach (var (itemId, count) in _playerOfferCounts)
00860:             {
00861:                 float baseP = _session?.Market?.GetPrice(itemId) ?? 10f;
00862:                 playerVal += baseP * count;
00863:             }
00864:             foreach (var (bio, count) in _bioOfferCounts)
00865:             {
00866:                 playerVal += global::Ashfall.Core.Economy.TradePricing.BioUnitValue(bio) * count;
00867:             }
00868:
00869:             float factionVal = 0f;
00870:             foreach (var (itemId, count) in _factionAskCounts)
00871:             {
00872:                 float baseP = _session?.Market?.GetPrice(itemId) ?? 10f;
00873:                 factionVal += baseP * count;
00874:             }
00875:
00876:             if (_lblPlayerWorth != null)
00877:             {
00878:                 _lblPlayerWorth.Text = $"Offer Worth: {FormatWorthLabel(playerVal)} ({_playerOfferCounts.Count} items, {_bioOfferCounts.Count} bio)";
00879:             }
00880:             if (_lblFactionAskWorth != null)
00881:             {
00882:                 _lblFactionAskWorth.Text = $"Demand Worth: {FormatWorthLabel(factionVal)} ({_factionAskCounts.Count} items)";
00883:             }
00884:
00885:             foreach (var pair in _bioCountLabels)
00886:             {
00887:                 pair.Value.Text = _bioOfferCounts.GetValueOrDefault(pair.Key, 0).ToString();
00888:             }
00889:
00890:             bool isFair = playerVal >= factionVal;
00891:             if (_lblFairness != null)
00892:             {
00893:                 _lblFairness.Text = isFair ? "DEAL IS FAIR" : "OFFER SHORT";
00894:                 _lblFairness.AddThemeColorOverride("font_color", isFair ? ToGodotColor(global::Ashfall.Core.UI.Theme.Hot) : ToGodotColor(global::Ashfall.Core.UI.Theme.Critical));
00895:             }
00896:
00897:             UpdateArbitratorScale(playerVal, factionVal);
00898:
00899:             var stance = _stanceProvider?.GetStance(_activeFactionId) ?? TradeStance.Trade;
00900:             bool willTrade = stance == TradeStance.Trade || stance == TradeStance.ShareIntel;
00901:             if (_btnConfirmTrade != null)
00902:             {
00903:                 _btnConfirmTrade.Disabled = !isFair || !willTrade;
00904:             }
00905:         }
00906:
00907:         private void ExecuteTrade()
00908:         {
00909:             if (_intentSink != null)
00910:             {
00911:                 _intentSink.TryConfirmTrade();
00912:                 RefreshView();
00913:                 return;
00914:             }
00915:
00916:             if (_lblRadioTicker != null)
00917:             {
00918:                 if (_radioProvider != null)
00919:                 {
00920:                     var intercept = _radioProvider.GetFactionEvent(_activeFactionId, RadioEventKind.TradeReaction, _session?.Market?.Day ?? 1, _rng);
00921:                     _lblRadioTicker.Text = $"RADIO: [{intercept.Callsign}] {intercept.Message}";
00922:                 }
00923:                 else
00924:                 {
00925:                     _lblRadioTicker.Text = "RADIO: Barter confirmed. Goods exchanged at checkpoint.";
00926:                 }
00927:             }
00928:             _playerOfferCounts.Clear();
00929:             _factionAskCounts.Clear();
00930:             _bioOfferCounts.Clear();
00931:             RefreshCalculations();
00932:         }
00933:
00934:         private void DemandParley()
00935:         {
00936:             if (_intentSink != null)
00937:             {
00938:                 _intentSink.TryDemandParley();
00939:                 return;
00940:             }
00941:
00942:             if (_lblRadioTicker != null)
00943:             {
00944:                 if (_radioProvider != null)
00945:                 {
00946:                     var intercept = _radioProvider.GetFactionEvent(_activeFactionId, RadioEventKind.ParleyResolution, _session?.Market?.Day ?? 1, _rng);
00947:                     _lblRadioTicker.Text = $"RADIO: [{intercept.Callsign}] {intercept.Message}";
00948:                 }
00949:                 else
00950:                 {
00951:                     _lblRadioTicker.Text = "RADIO: Parley demand transmitted. Awaiting emissary.";
00952:                 }
00953:             }
00954:         }
00955:
00956:         // ── Presentation Helpers ─────────────────────────────────────
00957:
00958:         private static Color ToGodotColor((float r, float g, float b, float a) token)
00959:         {
00960:             return ToColor(token);
00961:         }
00962:
00963:         private static Color GetStanceColor(TradeStance stance)
00964:         {
00965:             switch (stance)
00966:             {
00967:                 case TradeStance.ShareIntel:
00968:                 case TradeStance.Trade:
00969:                     return ToGodotColor(global::Ashfall.Core.UI.Theme.Hot);
00970:                 case TradeStance.Refuse:
00971:                     return ToGodotColor(global::Ashfall.Core.UI.Theme.Muted);
00972:                 case TradeStance.Rob:
00973:                     return ToGodotColor(global::Ashfall.Core.UI.Theme.Entropy);
00974:                 case TradeStance.HostileRaid:
00975:                 default:
00976:                     return ToGodotColor(global::Ashfall.Core.UI.Theme.Critical);
00977:             }
00978:         }
00979:
00980:         private static string GetShockIconPath(PriceShockKind kind)
00981:         {
00982:             switch (kind)
00983:             {
00984:                 case PriceShockKind.PlumePassing: return "res://assets/ui/Icons/icon_shock_plume.png";
00985:                 case PriceShockKind.ConvoyAmbush: return "res://assets/ui/Icons/icon_shock_convoy.png";
00986:                 case PriceShockKind.FactionConflict: return "res://assets/ui/Icons/icon_shock_war.png";
00987:                 case PriceShockKind.SeasonalScarcity: return "res://assets/ui/Icons/icon_shock_winter.png";
00988:                 case PriceShockKind.DiseaseOutbreak: return "res://assets/ui/Icons/icon_shock_plume.png";
00989:                 case PriceShockKind.FuelShortage: return "res://assets/ui/Icons/icon_shock_convoy.png";
00990:                 default: return "res://assets/ui/Icons/icon_shock_plume.png";
00991:             }
00992:         }
00993:
00994:         private static string FormatWorthLabel(float value)
00995:         {
00996:             // One source of truth: the core owns the qualitative thresholds.
00997:             return global::Ashfall.Core.Economy.TradeWorthLabels.Format(value);
00998:         }
00999:
01000:         private static Texture2D? LoadTexture(string path)
01001:         {
01002:             return TryLoadTexture(path);
01003:         }
01004:     }
01005: }
```

## `src/Main.Economy.cs` — 366 lines; 15,389 bytes; SHA-256 `2043c87b9cfaed583952235f93761567e656a645d675090b74cf213fe2e6c1ef`
Declaration index:
- 00031: public partial class Main : Control
- 00042: private void FlushCaravanIfDirty()
- 00047: private void SetupEconomy()
- 00102: private bool IsCanonicalRationingResource(string resourceId)
- 00111: private void BindRationingToInventory()
- 00118: private void OnEconomyOpenClicked()
- 00125: private void OnEconomySaveClicked()
- 00131: private void SaveEconomy()
- 00141: private void FlushEconomyIfDirty()
- 00158: private void TickEconomyWeatherBridge(int day)
- 00170: private void SetupCaravans()
- 00194: private void SaveCaravans()
- 00205: private void SaveSilentFoundry()
- 00218: private void SetupSilentFoundry()
- 00269: private void CloseSilentFoundryPanel()
- 00274: private void CloseTradePanel()
- 00281: private TradeVoiceResolver GetTradeVoiceResolver()
- 00301: private HardcoreEconomyTuning LoadHardcoreEconomyTuning()
- 00328: private void OpenTradeScreen()
- 00355: private void CloseEconomyPanel()
- 00360: private void CloseEconomyDetailPanel()
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
00033:         // ── Economy fields (GAP-ARCH-01 Phase 1) ──
00034:         private EconomyHostSession _economy = null!;
00035:         private bool _economyDirty;
00036:         private TravelingCaravanHostSession _caravans = null!;
00037:         private bool _caravansDirty;
00038:         private AtomicWar.GodotApp.Economy.TradeScreenGodotPanel _tradePanel = null!;
00039:         private Ashfall.Core.Radio.FactionRadioEngine _tradeRadio = null!;
00040:         private TradeVoiceResolver _tradeVoiceResolver = null!;
00041:
00042:         private void FlushCaravanIfDirty()
00043:         {
00044:             if (_caravansDirty) SaveCaravans();
00045:         }
00046:
00047:         private void SetupEconomy()
00048:         {
00049:             if (_economy != null) return;
00050:             _economy = EconomyHostSession.Create(_dataDir);
00051:             _economy.BindRationingResourceValidator(IsCanonicalRationingResource);
00052:             // Plan 42 / Plan 46 — the rationing owner's tier change reaches the
00053:             // journal voice trigger and the session telemetry through this one
00054:             // forwarder; no second ration model is created in the host.
00055:             _economy.RationTierChangedSeam += target =>
00056:             {
00057:                 RecordPlayMetricRationPolicyChanged(target);
00058:                 TriggerSurvivorVoiceRationCut(target);
00059:             };
00060:             BindRationingToInventory();
00061:             _economy.StateChanged += () => _economyDirty = true;
00062:
00063:             // Plan 212 follow-up — trade rumors from real market state: a shock
00064:             // the canonical market applies (or expires) becomes one band item.
00065:             // The text is Core-projected; this adapter only relays it once per
00066:             // event. Restore never re-fires the events, so no replay.
00067:             _economy.Market.OnShockStarted += shock =>
00068:             {
00069:                 if (shock == null) return;
00070:                 SetupRadio();
00071:                 _radio?.RecordMarketRumor(
00072:                     Ashfall.Core.Economy.EconomyMarketRumorRules.ShockStartedLine(shock), shock.startDay);
00073:             };
00074:             _economy.Market.OnShockExpired += shock =>
00075:             {
00076:                 if (shock == null) return;
00077:                 SetupRadio();
00078:                 _radio?.RecordMarketRumor(
00079:                     Ashfall.Core.Economy.EconomyMarketRumorRules.ShockExpiredLine(shock), shock.startDay);
00080:             };
00081:
00082:             var save = EconomySaveStore.TryLoad();
00083:             if (save != null)
00084:             {
00085:                 _economy.Market.RestoreState(save);
00086:                 _economyDirty = false; // restore just raised state-change events
00087:                 GD.Print("[Ashfall Godot] Economy state restored.");
00088:             }
00089:
00090:             if (_economyPanel == null && _rightColumn != null)
00091:             {
00092:                 _economyPanel = new EconomyMarketPanel();
00093:                 _rightColumn.AddChild(_economyPanel);
00094:             }
00095:             if (_economyPanel != null)
00096:             {
00097:                 _economyPanel.BindSession(_economy);
00098:                 _economyPanel.RefreshView();
00099:             }
00100:         }
00101:
00102:         private bool IsCanonicalRationingResource(string resourceId)
00103:         {
00104:             if (string.IsNullOrWhiteSpace(resourceId)) return false;
00105:             string id = resourceId.Trim();
00106:             return (_inventory?.Catalog?.Get(id) != null)
00107:                 || (_economy?.Catalog?.Find(id) != null)
00108:                 || GoodCategories.IsKnown(id);
00109:         }
00110:
00111:         private void BindRationingToInventory()
00112:         {
00113:             if (_inventory == null || _economy == null) return;
00114:             _inventory.RationingAuthorizer = (resourceId, consumerId, demand, day, available) =>
00115:                 _economy.AuthorizeAllocation(resourceId, consumerId, demand, available, day);
00116:         }
00117:
00118:         private void OnEconomyOpenClicked()
00119:         {
00120:             SetupEconomy();
00121:             _statusLabel.Text = _economy.StatusLine();
00122:             _codexViewer.Text = _economy.StatusLine();
00123:         }
00124:
00125:         private void OnEconomySaveClicked()
00126:         {
00127:             SetupEconomy();
00128:             SaveEconomy();
00129:         }
00130:
00131:         private void SaveEconomy()
00132:         {
00133:             if (_economy == null) return;
00134:             if (CaptureSection("economy", EconomySaveStore.TryCapturePersisted(_economy.CaptureSave())))
00135:             {
00136:                 _economyDirty = false;
00137:                 GD.Print("[Ashfall Godot] Economy save written.");
00138:             }
00139:         }
00140:
00141:         private void FlushEconomyIfDirty()
00142:         {
00143:             if (_economyDirty) SaveEconomy();
00144:         }
00145:
00146:         /// <summary>
00147:         /// Plan 212 — weather→market shock bridge. The weather authority owns
00148:         /// weather; the market owns its indices; the band mapping is Core
00149:         /// policy (<see cref="EconomyWeatherShockRules"/>). This adapter only
00150:         /// reads the canonical weather state and applies the bounded,
00151:         /// idempotent shock. Deterministic: pure function of current weather.
00152:         /// Plan 14A — the embargo authority FIRST advances its decay state with
00153:         /// the same authoritative weather (activation + decay are Core state);
00154:         /// the market then applies the decay-aware multiplier as one embargo
00155:         /// factor. No embargo shock ever enters ApplyShock — the two shock
00156:         /// paths stay separate factors in the canonical price equation.
00157:         /// </summary>
00158:         private void TickEconomyWeatherBridge(int day)
00159:         {
00160:             if (_economy == null) return;
00161:             if (_world?.Weather == null) return;
00162:             _economy.EmbargoSystem?.NotifyWeather(day, _world.Weather.Current);
00163:             var band = EconomyWeatherShockRules.TryGetWeatherShock(_world.Weather.Current);
00164:             if (band == null) return;
00165:             _economy.Market.ApplyShock(
00166:                 band.CategoryId, band.IsShortage, band.SeverityBp,
00167:                 startDay: day, durationDays: band.DurationDays, sourceId: band.SourceId);
00168:         }
00169:
00170:         private void SetupCaravans()
00171:         {
00172:             if (_caravans != null) return;
00173:             SetupEconomy();
00174:             _caravans = TravelingCaravanHostSession.Create(_dataDir);
00175:             // Plan 14A — caravans share the campaign's ONE embargo authority
00176:             // (rules from trade_embargoes.json); route blocking evaluates the
00177:             // same rules the market prices from.
00178:             _caravans.Engine.Embargoes = _economy.EmbargoSystem;
00179:             SetupWorld();
00180:             _caravans.Engine.Map = _world?.WastelandMap;
00181:             // C2 / Plan 20C (§41) — weather availability from the ONE effects
00182:             // table, combined with (never mixed into) the embargo multiplier.
00183:             _caravans.Engine.WeatherAvailabilityProvider = weather =>
00184:             {
00185:                 var effects = _world?.WeatherEffects;
00186:                 if (effects != null && effects.TryGetEffects(weather, out var fx) && fx != null)
00187:                     return fx.caravan_availability_multiplier;
00188:                 return 1f;
00189:             };
00190:             _caravans.StateChanged += () => _caravansDirty = true;
00191:             GD.Print("[Ashfall Godot] Caravan host ready.");
00192:         }
00193:
00194:         private void SaveCaravans()
00195:         {
00196:             if (_caravans == null) return;
00197:             if (CaptureSection("caravan", CaravanSaveStore.TryCapturePersisted(_caravans.CaptureSave())))
00198:             {
00199:             _caravansDirty = false;
00200:             _yearOfAshDirty = false;
00201:                 GD.Print("[Ashfall Godot] Caravan save written.");
00202:             }
00203:         }
00204:
00205:         private void SaveSilentFoundry()
00206:         {
00207:             if (_silentFoundry == null) return;
00208:             try
00209:             {
00210:                 CaptureSection("silent_foundry", SilentFoundrySaveStore.TryCapturePersisted(_silentFoundry.Engine.CaptureState()));
00211:             }
00212:             catch (Exception e)
00213:             {
00214:                 GD.PushWarning("[Ashfall Godot] SilentFoundry save failed: " + e.Message);
00215:             }
00216:         }
00217:
00218:         private void SetupSilentFoundry()
00219:         {
00220:             if (_silentFoundry != null) return;
00221:             SetupExpansions();
00222:             SetupInventory();
00223:             SetupJournal();
00224:             SetupEconomy();
00225:             SetupPowerGrid();
00226:             _silentFoundry = AtomicWar.GodotApp.SilentFoundryHostSession.Create(
00227:                 _dataDir, _expansions, _inventory, _journal, market: _economy.Market,
00228:                 seedSupplies: _campaignInitializationMode == CampaignInitializationMode.FreshInitialize);
00229:             _silentFoundry.BindPowerAndThermal(_powerGrid?.System, _shelterThermal?.System);
00230:             // Plan B66: heavy batches emit smoke/CO through the canonical
00231:             // ventilation authority (register/deactivate around each batch).
00232:             _silentFoundry.Engine.BindVentilation(_ventilation);
00233:             // GAP-STUB-03 (resolved): wire the remaining FactionStanceEngine
00234:             // providers as live accessors into Main state, not one-time
00235:             // captured values, so guild trust reflects the campaign's actual
00236:             // current day, radiation, and military-survivor presence on every
00237:             // future read — including after the values change post-bind.
00238:             _silentFoundry.BindStanceProviders(
00239:                 campaignDayProvider: () => _simDay,
00240:                 partyRadiationProvider: () => _holdfastRuntime?.Radiation ?? 0f,
00241:                 survivorsProvider: () => _survivors);
00242:             // v6 SaltMine: hub envelope already restored into expansions/foundry
00243:             // Core systems; SaltMine lives on this host session — restore it
00244:             // from the same hub payload when present.
00245:             var hubSave = ExpansionHubSaveStore.TryLoad();
00246:             if (hubSave?.saltMine != null)
00247:                 _silentFoundry.SaltMine.RestoreState(hubSave.saltMine);
00248:             // Foundry + SaltMine ride the expansion-hub save; state-change
00249:             // events mark the hub save dirty so nothing is lost.
00250:             _silentFoundry.StateChanged += () =>
00251:             {
00252:                 _foundryDirty = true;
00253:                 _silentFoundryPanel?.RefreshView();
00254:                 _factionsPanel?.RefreshView();
00255:                 _economyPanel?.RefreshView();
00256:                 if (_state == GameState.Playing) UpdateHud();
00257:             };
00258:             if (_silentFoundryPanel != null)
00259:             {
00260:                 _silentFoundryPanel.Bind(_silentFoundry, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay);
00261:                 _silentFoundryPanel.SetMachineTellCatalog(GetMachineTellCatalog());
00262:             }
00263:             // Live market strip: show the guild's real trade access at all times.
00264:             if (_economyPanel != null)
00265:                 _economyPanel.BindStance(_silentFoundry.GuildStanceEngine, Ashfall.Core.Foundry.SilentFoundryIds.FactionId);
00266:             GD.Print("[Ashfall Godot] Silent Foundry host ready (exp_10_the_silent_foundry).");
00267:         }
00268:
00269:         private void CloseSilentFoundryPanel()
00270:         {
00271:             _silentFoundryPanel.Visible = false;
00272:         }
00273:
00274:         private void CloseTradePanel()
00275:         {
00276:             if (_silentFoundry != null)
00277:                 _silentFoundry.StateChanged -= _tradePanel.RefreshView;
00278:             _tradePanel.Visible = false;
00279:         }
00280:
00281:         private TradeVoiceResolver GetTradeVoiceResolver()
00282:         {
00283:             if (_tradeVoiceResolver != null) return _tradeVoiceResolver;
00284:
00285:             var load = TradeTextCatalogLoader.Load(
00286:                 _dataDir,
00287:                 new FileSystemIO(),
00288:                 new SystemTextJsonSerializer());
00289:             if (!load.IsValid)
00290:             {
00291:                 GD.PushWarning("[Ashfall Godot] Trade voice catalog using fallback: "
00292:                     + (load.Errors.Count == 0
00293:                         ? "catalog missing"
00294:                         : string.Join("; ", load.Errors)));
00295:             }
00296:
00297:             _tradeVoiceResolver = new TradeVoiceResolver(load.Catalog);
00298:             return _tradeVoiceResolver;
00299:         }
00300:
00301:         private HardcoreEconomyTuning LoadHardcoreEconomyTuning()
00302:         {
00303:             var tuning = new HardcoreEconomyTuning();
00304:             string path = Path.Combine(_dataDir, "hardcore_economy_tuning.json");
00305:             if (!File.Exists(path))
00306:             {
00307:                 GD.PushWarning($"[Ashfall Godot] Hardcore economy tuning missing: {path}");
00308:                 return tuning;
00309:             }
00310:
00311:             var result = HardcoreEconomyTuningLoader.Load(File.ReadAllText(path));
00312:             if (!result.IsValid || result.Bundle == null)
00313:             {
00314:                 GD.PushWarning("[Ashfall Godot] Hardcore economy tuning rejected: "
00315:                     + string.Join("; ", result.Errors));
00316:                 return tuning;
00317:             }
00318:
00319:             tuning.Apply(result.Bundle);
00320:             return tuning;
00321:         }
00322:
00323:         /// <summary>
00324:         /// Open the live trade screen bound to the Foundry Guild's real stance
00325:         /// engine (derived from the durable consequence ledger). The panel's
00326:         /// confirm gate follows TradeStance: below Trade the stall is blocked.
00327:         /// </summary>
00328:         private void OpenTradeScreen()
00329:         {
00330:             if (_tradePanel == null) return;
00331:             if (_tradeRadio == null)
00332:             {
00333:                 string radioPath = Path.Combine(_dataDir, "faction_radio_corpus.json");
00334:                 _tradeRadio = Ashfall.Core.Radio.FactionRadioEngine.LoadFromJson(
00335:                     System.IO.File.Exists(radioPath) ? System.IO.File.ReadAllText(radioPath) : "{}");
00336:             }
00337:             var tuning = LoadHardcoreEconomyTuning();
00338:             SetupCampaignDay();
00339:             _tradePanel.BindSession(
00340:                 _economy,
00341:                 _silentFoundry.GuildStanceEngine,
00342:                 tuning,
00343:                 _tradeRadio,
00344:                 _campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Economy).Rng,
00345:                 GetTradeVoiceResolver());
00346:             _tradePanel.SetActiveFaction(Ashfall.Core.Foundry.SilentFoundryIds.FactionId);
00347:             // Live refresh when a treaty consequence moves the guild's standing
00348:             // (subscribe once per open; CloseTradePanel removes it).
00349:             _silentFoundry.StateChanged -= _tradePanel.RefreshView;
00350:             _silentFoundry.StateChanged += _tradePanel.RefreshView;
00351:             _tradePanel.Open();
00352:             GD.Print($"[Ashfall Godot] Trade screen open — Foundry Guild stance {_silentFoundry.GuildStance} · trust {_silentFoundry.GuildTrust:F0}");
00353:         }
00354:
00355:         private void CloseEconomyPanel()
00356:         {
00357:             _economyPanel.Visible = false;
00358:         }
00359:
00360:         private void CloseEconomyDetailPanel()
00361:         {
00362:             _economyDetailPanel.Visible = false;
00363:         }
00364:
00365:     }
00366: }
```

## `src/UI/CaravanBarterLedgerPanel.cs` — 273 lines; 10,835 bytes; SHA-256 `d26fed01020da21467d00dbcef0594d808b44162cbeb8e55fc54f0d9acb0ce4f`
Declaration index:
- 00042: public partial class CaravanBarterLedgerPanel : Control, IBindablePanel
- 00058: public void Bind(
- 00078: public void BindViewModel(ITradeScreenViewModel viewModel, ITradeIntentSink intentSink)
- 00086: public void SetActiveFaction(string factionId)
- 00096: public void RefreshView()
- 00135: private static int SafeGetConsecutiveRepels(IFactionStanceProvider stance, string factionId)
- 00234: public void Open()
- 00242: public void Close() {
- 00260: public void Unbind()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Godot;
00004: using Ashfall.Core;
00005: using Ashfall.Core.Economy;
00006: using Ashfall.Core.Radio;
00007: using Ashfall.Core.UI;
00008: using AtomicWar.GodotApp.UI;
00009: using AtomicWar.GodotApp.Economy;
00010: using DesignTheme = Ashfall.Core.UI.Theme;
00011:
00012: using Ashfall.Core.IO;
00013: namespace AtomicWar.GodotApp.UI;
00014:
00015: /// <summary>
00016: /// ASHFALL — Caravan Barter Ledger (#35 Stitch).
00017: ///
00018: /// Dashboard HYBRID wrapper around the existing TradeScreenGodotPanel.
00019: /// The "ledger components" — trader card, two-column offer/ask table,
00020: /// arbitrator, radio ticker — are already implemented inside
00021: /// TradeScreenGodotPanel and would be regressed by any wholesale refactor.
00022: /// This wrapper adds the Stitch dashboard chrome (sidebar nav for trade-flow
00023: /// actions + status rail of faction stance / trust / repels counters) and
00024: /// hosts TradeScreenGodotPanel inside the dashboard shell content slot.
00025: ///
00026: /// The trade engine (EconomyHostSession / IFactionStanceProvider /
00027: /// IPriceShockProvider / IFactionRadioProvider) remains the authoritative
00028: /// source. The wrapper reads stance / trust / aggression / repels strictly
00029: /// from existing session APIs to populate the status rail — no fake
00030: /// metrics, no derived values not already exposed by the engine.
00031: ///
00032: /// Sub-section nav (sidebar) lets the user jump between:
00033: ///   • Caravan context (faction profile, headline stance)
00034: ///   • Player offer column (open the existing player offer list)
00035: ///   • Faction stock column (open the existing faction asks)
00036: ///   • Arbitrator scale (open the existing fair-deal strip)
00037: ///
00038: /// The existing "ACCEPT BARTER" / "DEMAND PARLEY" buttons still live inside
00039: /// TradeScreenGodotPanel. This wrapper does not re-implement the wiring; it
00040: /// only routes the user toward the right sub-section via sidebar selection.
00041: /// </summary>
00042: public partial class CaravanBarterLedgerPanel : Control, IBindablePanel
00043: {
00044:     public event Action? OnClose;
00045:     public event Action<string>? OnSetActiveFaction;
00046:
00047:     private AshfallDashboardShell _shell = null!;
00048:     private AshfallSidebar? _sidebar;
00049:     private AshfallStatusRail? _statusRail;
00050:     private TradeScreenGodotPanel _tradeInner = null!;
00051:
00052:     private EconomyHostSession? _session;
00053:     private IFactionStanceProvider? _stance;
00054:     private string _activeFactionId = "scavenger_camp";
00055:
00056:     public bool IsBound => _tradeInner != null && _session != null;
00057:
00058:     public void Bind(
00059:         EconomyHostSession session,
00060:         IFactionStanceProvider? stanceProvider = null,
00061:         IPriceShockProvider? priceShockProvider = null,
00062:         IFactionRadioProvider? radioProvider = null,
00063:         ISeededRng? rng = null)
00064:     {
00065:         if (_session != null)
00066:             _session.StateChanged -= RefreshView;
00067:         _session = session;
00068:         _stance = stanceProvider;
00069:         if (_session != null)
00070:             _session.StateChanged += RefreshView;
00071:         if (_tradeInner != null)
00072:         {
00073:             _tradeInner.BindSession(session, stanceProvider!, priceShockProvider!, radioProvider!, rng!);
00074:         }
00075:         RefreshView();
00076:     }
00077:
00078:     public void BindViewModel(ITradeScreenViewModel viewModel, ITradeIntentSink intentSink)
00079:     {
00080:         if (_tradeInner != null)
00081:         {
00082:             _tradeInner.BindViewModel(viewModel, intentSink);
00083:         }
00084:     }
00085:
00086:     public void SetActiveFaction(string factionId)
00087:     {
00088:         _activeFactionId = factionId ?? "scavenger_camp";
00089:         if (_tradeInner != null)
00090:         {
00091:             _tradeInner.SetActiveFaction(_activeFactionId);
00092:         }
00093:         RefreshView();
00094:     }
00095:
00096:     public void RefreshView()
00097:     {
00098:         if (_statusRail == null || _tradeInner == null) return;
00099:         if (_session == null || _stance == null)
00100:         {
00101:             _statusRail.Set("faction",  "—",            AshfallMetricCard.Criticality.Normal);
00102:             _statusRail.Set("stance",   "—",            AshfallMetricCard.Criticality.Normal);
00103:             _statusRail.Set("trust",    "0",            AshfallMetricCard.Criticality.Normal);
00104:             _statusRail.Set("aggress",  "0.00",         AshfallMetricCard.Criticality.Normal);
00105:             _statusRail.Set("repels",   "0",            AshfallMetricCard.Criticality.Normal);
00106:             return;
00107:         }
00108:
00109:         var stance = _stance.GetStance(_activeFactionId);
00110:         float trust = _stance.GetEffectiveTrust(_activeFactionId);
00111:         float aggression = _stance.GetRaidAggression(_activeFactionId);
00112:         int consecutiveRepels = SafeGetConsecutiveRepels(_stance, _activeFactionId);
00113:
00114:         string stanceLabel = stance.ToString().ToUpperInvariant();
00115:         var stanceCrit = stance.ToString() switch
00116:         {
00117:             "Trade" => AshfallMetricCard.Criticality.Normal,
00118:             "Rob" => AshfallMetricCard.Criticality.Warn,
00119:             "HostileRaid" => AshfallMetricCard.Criticality.Critical,
00120:             _ => AshfallMetricCard.Criticality.Caution,
00121:         };
00122:
00123:         var trustCrit = trust >= 50 ? AshfallMetricCard.Criticality.Normal
00124:             : trust >= 0 ? AshfallMetricCard.Criticality.Caution
00125:             : trust >= -25 ? AshfallMetricCard.Criticality.Warn
00126:             : AshfallMetricCard.Criticality.Critical;
00127:
00128:         _statusRail.Set("faction",  _activeFactionId.Replace('_', ' ').ToUpperInvariant(), AshfallMetricCard.Criticality.Normal);
00129:         _statusRail.Set("stance",   $"[{stanceLabel}]", stanceCrit);
00130:         _statusRail.Set("trust",    trust > 0 ? $"+{trust:0}" : $"{trust:0}",       trustCrit);
00131:         _statusRail.Set("aggress",  $"{aggression:0.00}",                            AshfallMetricCard.Criticality.Normal);
00132:         _statusRail.Set("repels",   $"{consecutiveRepels}",                          AshfallMetricCard.Criticality.Normal);
00133:     }
00134:
00135:     private static int SafeGetConsecutiveRepels(IFactionStanceProvider stance, string factionId)
00136:     {
00137:         try
00138:         {
00139:             // The stance engine exposes a negotiated-count accessor in HoldfastTradeSession,
00140:             // but the IFactionStanceProvider abstraction does not. We probe via a known
00141:             // property pattern, otherwise fall back to zero — never invent data.
00142:             var t = stance.GetType();
00143:             var prop = t.GetProperty("ConsecutiveRepels");
00144:             if (prop != null)
00145:             {
00146:                 var raw = prop.GetValue(stance);
00147:                 if (raw is int i) return i;
00148:             }
00149:             return 0;
00150:         }
00151:         catch (Exception ex_CATDIAG)
00152:         {
00153:             CatalogDiagnostics.Warn("<reflection>", "ConsecutiveRepels property", ex_CATDIAG);
00154:             return 0;
00155:         }
00156:     }
00157:
00158:     public override void _Ready()
00159:     {
00160:         SetAnchorsPreset(LayoutPreset.FullRect);
00161:         Visible = false;
00162:
00163:         var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.90f) };
00164:         bg.SetAnchorsPreset(LayoutPreset.FullRect);
00165:         AddChild(bg);
00166:
00167:         var center = new CenterContainer();
00168:         center.SetAnchorsPreset(LayoutPreset.FullRect);
00169:         AddChild(center);
00170:
00171:         _shell = new AshfallDashboardShell(
00172:             "CARAVAN BARTER LEDGER — OPEN_TRADE_TABLE",
00173:             1100, 720);
00174:         center.AddChild(_shell);
00175:
00176:         _sidebar = _shell.SetSidebar(new[]
00177:         {
00178:             new AshfallSidebar.Item { Id = "context",      Label = "Context",       Hint = "Faction profile" },
00179:             new AshfallSidebar.Item { Id = "your_offers",  Label = "Your Offers",   Hint = "Player edge" },
00180:             new AshfallSidebar.Item { Id = "their_asks",   Label = "Their Asks",    Hint = "Faction edge" },
00181:             new AshfallSidebar.Item { Id = "fairness",     Label = "Fairness",      Hint = "DEAL IS FAIR indicator" },
00182:             new AshfallSidebar.Item { Id = "biology",      Label = "Biology",       Hint = "Biological drawer" },
00183:         }, "LEDGER OPS", "context");
00184:         _statusRail = _shell.SetStatusRail();
00185:         _statusRail.AddCard("faction", "FACTION",   "—",        AshfallMetricCard.Criticality.Normal, 180);
00186:         _statusRail.AddCard("stance",  "STANCE",    "—",        AshfallMetricCard.Criticality.Normal, 140);
00187:         _statusRail.AddCard("trust",   "TRUST",     "0",        AshfallMetricCard.Criticality.Normal, 110);
00188:         _statusRail.AddCard("aggress", "AGGRESSION","0.00",     AshfallMetricCard.Criticality.Normal, 130);
00189:         _statusRail.AddCard("repels",  "REPELS",    "0",        AshfallMetricCard.Criticality.Normal, 110);
00190:
00191:         _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());
00192:
00193:         // TradeScreenGodotPanel builds its own internal UI when added to the tree.
00194:         // We reparent it into the shell's content slot so the existing chrome
00195:         // plots inside the dashboard frame.
00196:         _tradeInner = new TradeScreenGodotPanel();
00197:         _tradeInner.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00198:         _tradeInner.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00199:         _shell.SetContent(_tradeInner);
00200:
00201:         if (_session != null)
00202:         {
00203:             _tradeInner.BindSession(_session, _stance!);
00204:         }
00205:
00206:         // Sidebar ids are ledger section ops (context/your_offers/their_asks/
00207:         // fairness/biology), not faction ids. Never route section ids through
00208:         // SetActiveFaction / OnSetActiveFaction — that would corrupt stance rail.
00209:         if (_sidebar != null)
00210:         {
00211:             _sidebar.OnSelected += id =>
00212:             {
00213:                 switch (id)
00214:                 {
00215:                     case "context":
00216:                     case "your_offers":
00217:                     case "their_asks":
00218:                     case "fairness":
00219:                     case "biology":
00220:                         _tradeInner?.FocusLedgerSection(id);
00221:                         break;
00222:                     default:
00223:                         // Real faction ids only.
00224:                         SetActiveFaction(id);
00225:                         OnSetActiveFaction?.Invoke(id);
00226:                         break;
00227:                 }
00228:                 RefreshView();
00229:             };
00230:         }
00231:         RefreshView();
00232:     }
00233:
00234:     public void Open()
00235:     {
00236:         Visible = true;
00237:         _tradeInner.Visible = true;
00238:         _tradeInner.Open();
00239:         RefreshView();
00240:     }
00241:
00242:     public void Close() {
00243:             _tradeInner.Close();
00244:             if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
00245:                 Visible = false;
00246:             OnClose?.Invoke();
00247:         }
00248:
00249:     public override void _UnhandledInput(InputEvent @event)
00250:     {
00251:         if (!Visible) return;
00252:         if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00253:         {
00254:             OnClose?.Invoke();
00255:             GetViewport().SetInputAsHandled();
00256:         }
00257:     }
00258:
00259:
00260:     public void Unbind()
00261:     {
00262:         if (_session != null)
00263:         {
00264:             _session.StateChanged -= RefreshView;
00265:         }
00266:     }
00267:
00268:     public override void _ExitTree()
00269:         {
00270:             Unbind();
00271:             base._ExitTree();
00272:         }
00273: }
```

## `Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs` — 440 lines; 19,532 bytes; SHA-256 `50867414810aa973729e7f653fd5ed5e9b5ff1006a557a7567a7ffefb37d8974`
Declaration index:
- 00020: public class TradeScreenScenarioCatalogTests
- 00022: private static string DataPath(string fileName)
- 00032: private static IReadOnlyList<TradeScreenScenario> LoadScenarios()
- 00037: private static TradeTellEngine LoadTells()
- 00042: private static HashSet<string> LoadItemIds()
- 00057: private static HashSet<string> LoadFactionIds()
- 00065: private static void Collect(JsonElement el, HashSet<string> ids)
- 00108: public void Catalog_ContainsExactlyFifteenScenarios()
- 00114: public void Catalog_ScenarioIdsAreUniqueSnakeCase()
- 00127: public void Catalog_AllTwelvePlan61ScenariosPresent()
- 00137: public void Catalog_AllEightArchetypesRepresentedInPlan61Distribution()
- 00160: public void Catalog_EveryItemReferenceResolvesToItemsJson()
- 00179: public void Catalog_EveryFactionReferenceResolvesToCanonicalSet()
- 00190: public void Catalog_ValidStancesMetersAndShockBounds()
- 00208: public void Catalog_LineDataIsPositiveAndFinite()
- 00224: public void Catalog_UnitPricesAreGloballyConsistentPerItem()
- 00249: public void Catalog_CrossScenarioPriceSpreadsStayWithinAntiArbitrageBand()
- 00278: public void Catalog_ComputedFairnessMatchesDataExpectation()
- 00292: public void Catalog_ConfirmSucceedsOnlyForFairTradableTables()
- 00306: public void Catalog_EveryScenarioBindsWithTellAndLegibleState()
- 00325: public void Catalog_TellSelectionIsSeedDeterministic()
- 00340: public void Catalog_OnlyDeliberateEmptyTablesAreEmpty()
- 00354: public void Catalog_DebtCollectorTableIsDemandsOnly()
- 00369: public void Catalog_EveryScenarioDiffersFromNearestNeighborInTwoDimensions()
- 00405: public void Catalog_OriginalThreeScenariosKeepLockedContracts()
- 00429: public void Catalog_RootShapeIsPreserved()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using Ashfall.Core;
00008: using Ashfall.Core.Economy;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests
00012: {
00013:     /// <summary>
00014:     /// Plan 61 — trade scenario catalog expansion gates (3 → 15).
00015:     /// Data-contract, reference-integrity, fairness, price-consistency,
00016:     /// differentiation, reachability, and determinism gates for the expanded
00017:     /// catalog. The CatalogIntegrityValidator does not scan this file, so these
00018:     /// tests carry the whole validation load.
00019:     /// </summary>
00020:     public class TradeScreenScenarioCatalogTests
00021:     {
00022:         private static string DataPath(string fileName)
00023:         {
00024:             string path = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", fileName);
00025:             if (!File.Exists(path))
00026:             {
00027:                 path = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", fileName);
00028:             }
00029:             return path;
00030:         }
00031:
00032:         private static IReadOnlyList<TradeScreenScenario> LoadScenarios()
00033:         {
00034:             return TradeScreenScenarioLoader.LoadFromJson(File.ReadAllText(DataPath("trade_screen_scenarios.json")));
00035:         }
00036:
00037:         private static TradeTellEngine LoadTells()
00038:         {
00039:             return TradeTellEngine.LoadFromJson(File.ReadAllText(DataPath("trade_tell_lines.json")));
00040:         }
00041:
00042:         private static HashSet<string> LoadItemIds()
00043:         {
00044:             using var doc = JsonDocument.Parse(File.ReadAllText(DataPath("items.json")));
00045:             var ids = new HashSet<string>(StringComparer.Ordinal);
00046:             foreach (var it in doc.RootElement.GetProperty("items").EnumerateArray())
00047:             {
00048:                 if (it.TryGetProperty("id", out var idEl) && idEl.ValueKind == JsonValueKind.String)
00049:                 {
00050:                     ids.Add(idEl.GetString() ?? string.Empty);
00051:                 }
00052:             }
00053:             return ids;
00054:         }
00055:
00056:         /// <summary>Canonical faction ID set from faction_radio_corpus.json.</summary>
00057:         private static HashSet<string> LoadFactionIds()
00058:         {
00059:             using var doc = JsonDocument.Parse(File.ReadAllText(DataPath("faction_radio_corpus.json")));
00060:             var ids = new HashSet<string>(StringComparer.Ordinal);
00061:             Collect(doc.RootElement, ids);
00062:             return ids;
00063:         }
00064:
00065:         private static void Collect(JsonElement el, HashSet<string> ids)
00066:         {
00067:             switch (el.ValueKind)
00068:             {
00069:                 case JsonValueKind.Object:
00070:                     foreach (var p in el.EnumerateObject())
00071:                     {
00072:                         if (p.Name == "faction_id" && p.Value.ValueKind == JsonValueKind.String)
00073:                         {
00074:                             ids.Add(p.Value.GetString() ?? string.Empty);
00075:                         }
00076:                         else
00077:                         {
00078:                             Collect(p.Value, ids);
00079:                         }
00080:                     }
00081:                     break;
00082:                 case JsonValueKind.Array:
00083:                     foreach (var item in el.EnumerateArray()) Collect(item, ids);
00084:                     break;
00085:             }
00086:         }
00087:
00088:         /// <summary>The twelve Plan 61 scenario IDs mapped to their trader archetype.</summary>
00089:         private static readonly Dictionary<string, string> Plan61Archetypes = new()
00090:         {
00091:             ["last_vials"] = "desperate_survivor",
00092:             ["winter_cart"] = "desperate_survivor",
00093:             ["depot_window"] = "faction_quartermaster",
00094:             ["emergency_requisition"] = "faction_quartermaster",
00095:             ["back_room_exchange"] = "black_market",
00096:             ["ledgerless_broker"] = "black_market",
00097:             ["long_road_caravan"] = "caravan_merchant",
00098:             ["salvage_caravan"] = "caravan_merchant",
00099:             ["settlement_of_accounts"] = "debt_collector",
00100:             ["crate_lot"] = "bulk_dealer",
00101:             ["border_runner"] = "smuggler",
00102:             ["road_knowledge"] = "refugee_barter",
00103:         };
00104:
00105:         // ── Data contract ────────────────────────────────────────────
00106:
00107:         [Fact]
00108:         public void Catalog_ContainsExactlyFifteenScenarios()
00109:         {
00110:             Assert.Equal(15, LoadScenarios().Count);
00111:         }
00112:
00113:         [Fact]
00114:         public void Catalog_ScenarioIdsAreUniqueSnakeCase()
00115:         {
00116:             var scenarios = LoadScenarios();
00117:             var ids = scenarios.Select(s => s.Id).ToList();
00118:             Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
00119:             Assert.All(ids, id =>
00120:             {
00121:                 Assert.False(string.IsNullOrWhiteSpace(id));
00122:                 Assert.Matches("^[a-z0-9_]+$", id);
00123:             });
00124:         }
00125:
00126:         [Fact]
00127:         public void Catalog_AllTwelvePlan61ScenariosPresent()
00128:         {
00129:             var ids = LoadScenarios().Select(s => s.Id).ToHashSet(StringComparer.Ordinal);
00130:             foreach (var expected in Plan61Archetypes.Keys)
00131:             {
00132:                 Assert.True(ids.Contains(expected), $"Missing Plan 61 scenario: {expected}");
00133:             }
00134:         }
00135:
00136:         [Fact]
00137:         public void Catalog_AllEightArchetypesRepresentedInPlan61Distribution()
00138:         {
00139:             var scenarios = LoadScenarios().Where(s => Plan61Archetypes.ContainsKey(s.Id));
00140:             var counts = new Dictionary<string, int>();
00141:             foreach (var s in scenarios)
00142:             {
00143:                 var archetype = Plan61Archetypes[s.Id];
00144:                 counts[archetype] = (counts.TryGetValue(archetype, out var c) ? c : 0) + 1;
00145:             }
00146:
00147:             Assert.Equal(2, counts["desperate_survivor"]);
00148:             Assert.Equal(2, counts["faction_quartermaster"]);
00149:             Assert.Equal(2, counts["black_market"]);
00150:             Assert.Equal(2, counts["caravan_merchant"]);
00151:             Assert.Equal(1, counts["debt_collector"]);
00152:             Assert.Equal(1, counts["bulk_dealer"]);
00153:             Assert.Equal(1, counts["smuggler"]);
00154:             Assert.Equal(1, counts["refugee_barter"]);
00155:         }
00156:
00157:         // ── Reference integrity ──────────────────────────────────────
00158:
00159:         [Fact]
00160:         public void Catalog_EveryItemReferenceResolvesToItemsJson()
00161:         {
00162:             var itemIds = LoadItemIds();
00163:             foreach (var s in LoadScenarios())
00164:             {
00165:                 foreach (var line in s.PlayerOffers.Concat(s.FactionDemands))
00166:                 {
00167:                     Assert.True(itemIds.Contains(line.ItemId),
00168:                         $"Scenario {s.Id} references unknown item '{line.ItemId}'");
00169:                 }
00170:                 foreach (var band in s.Scarcity)
00171:                 {
00172:                     Assert.True(itemIds.Contains(band.ItemId),
00173:                         $"Scenario {s.Id} scarcity references unknown item '{band.ItemId}'");
00174:                 }
00175:             }
00176:         }
00177:
00178:         [Fact]
00179:         public void Catalog_EveryFactionReferenceResolvesToCanonicalSet()
00180:         {
00181:             var factions = LoadFactionIds();
00182:             foreach (var s in LoadScenarios())
00183:             {
00184:                 Assert.True(factions.Contains(s.FactionId),
00185:                     $"Scenario {s.Id} references faction '{s.FactionId}' not present in faction_radio_corpus.json");
00186:             }
00187:         }
00188:
00189:         [Fact]
00190:         public void Catalog_ValidStancesMetersAndShockBounds()
00191:         {
00192:             var validStances = new[] { TradeStance.HostileRaid, TradeStance.Rob, TradeStance.Refuse, TradeStance.Trade, TradeStance.ShareIntel };
00193:             foreach (var s in LoadScenarios())
00194:             {
00195:                 Assert.Contains(s.Stance, validStances);
00196:                 Assert.InRange(s.Trust, -100f, 100f);
00197:                 Assert.InRange(s.Aggression, 0f, 1f);
00198:                 Assert.True(s.WorldDay >= 1);
00199:                 foreach (var shock in s.PriceShocks)
00200:                 {
00201:                     Assert.True(float.IsFinite(shock.Multiplier) && shock.Multiplier > 0f,
00202:                         $"Scenario {s.Id} has invalid shock multiplier");
00203:                 }
00204:             }
00205:         }
00206:
00207:         [Fact]
00208:         public void Catalog_LineDataIsPositiveAndFinite()
00209:         {
00210:             foreach (var s in LoadScenarios())
00211:             {
00212:                 foreach (var line in s.PlayerOffers.Concat(s.FactionDemands))
00213:                 {
00214:                     Assert.True(line.Quantity > 0, $"Scenario {s.Id}: {line.ItemId} quantity must be positive");
00215:                     Assert.True(float.IsFinite(line.TotalValue) && line.TotalValue > 0f,
00216:                         $"Scenario {s.Id}: {line.ItemId} total value must be positive");
00217:                 }
00218:             }
00219:         }
00220:
00221:         // ── Price authority: global unit-price consistency ───────────
00222:
00223:         [Fact]
00224:         public void Catalog_UnitPricesAreGloballyConsistentPerItem()
00225:         {
00226:             // Anti-arbitrage rule: the same item_id must carry one authored
00227:             // worth everywhere (both edges, all scenarios). Baseline convention
00228:             // (canned_food = 18 in fair_deal and offer_short) generalized.
00229:             var prices = new Dictionary<string, float>(StringComparer.Ordinal);
00230:             foreach (var s in LoadScenarios())
00231:             {
00232:                 foreach (var line in s.PlayerOffers.Concat(s.FactionDemands))
00233:                 {
00234:                     float perUnit = line.Quantity > 0 ? line.TotalValue / line.Quantity : 0f;
00235:                     if (prices.TryGetValue(line.ItemId, out var known))
00236:                     {
00237:                         Assert.True(Math.Abs(known - perUnit) < 0.01f,
00238:                             $"Item '{line.ItemId}' has inconsistent unit price: {known} vs {perUnit} (scenario {s.Id})");
00239:                     }
00240:                     else
00241:                     {
00242:                         prices[line.ItemId] = perUnit;
00243:                     }
00244:                 }
00245:             }
00246:         }
00247:
00248:         [Fact]
00249:         public void Catalog_CrossScenarioPriceSpreadsStayWithinAntiArbitrageBand()
00250:         {
00251:             // Even with consistency enforced, guard the band: no item's worth
00252:             // may differ across scenarios (regression guard if data forks).
00253:             var byItem = new Dictionary<string, List<float>>(StringComparer.Ordinal);
00254:             foreach (var s in LoadScenarios())
00255:             {
00256:                 foreach (var line in s.PlayerOffers.Concat(s.FactionDemands))
00257:                 {
00258:                     float perUnit = line.TotalValue / line.Quantity;
00259:                     if (!byItem.TryGetValue(line.ItemId, out var list))
00260:                     {
00261:                         list = new List<float>();
00262:                         byItem[line.ItemId] = list;
00263:                     }
00264:                     list.Add(perUnit);
00265:                 }
00266:             }
00267:             foreach (var pair in byItem)
00268:             {
00269:                 var spread = pair.Value.Max() - pair.Value.Min();
00270:                 Assert.True(spread < 0.01f,
00271:                     $"Arbitrage risk: '{pair.Key}' worth spreads by {spread} across scenario tables");
00272:             }
00273:         }
00274:
00275:         // ── Fairness & confirm contract ──────────────────────────────
00276:
00277:         [Fact]
00278:         public void Catalog_ComputedFairnessMatchesDataExpectation()
00279:         {
00280:             foreach (var s in LoadScenarios())
00281:             {
00282:                 var binding = TradeScreenScenarioLoader.CreateBinding(s, LoadTells(), new SeededRng(61));
00283:                 var vm = binding.ViewModel;
00284:                 var expected = s.ExpectedFairness;
00285:                 Assert.True(vm.Fairness == expected,
00286:                     $"Scenario {s.Id}: computed fairness {vm.Fairness} != expected {expected} " +
00287:                     $"(player {vm.PlayerOfferValue} vs ask {vm.FactionAskValue})");
00288:             }
00289:         }
00290:
00291:         [Fact]
00292:         public void Catalog_ConfirmSucceedsOnlyForFairTradableTables()
00293:         {
00294:             foreach (var s in LoadScenarios())
00295:             {
00296:                 bool willTrade = s.Stance == TradeStance.Trade || s.Stance == TradeStance.ShareIntel;
00297:                 bool expectedConfirm = willTrade && s.ExpectedFairness == TradeFairness.Fair;
00298:                 Assert.True(s.ConfirmSucceeds == expectedConfirm,
00299:                     $"Scenario {s.Id}: confirm_succeeds={s.ConfirmSucceeds} but fairness/stance imply {expectedConfirm}");
00300:             }
00301:         }
00302:
00303:         // ── Reachability & tell integration (Plan 62 — live) ─────────
00304:
00305:         [Fact]
00306:         public void Catalog_EveryScenarioBindsWithTellAndLegibleState()
00307:         {
00308:             var tells = LoadTells();
00309:             foreach (var s in LoadScenarios())
00310:             {
00311:                 var binding = TradeScreenScenarioLoader.CreateBinding(s, tells, new SeededRng(61));
00312:                 var vm = binding.ViewModel;
00313:                 Assert.True(vm.IsOpen, $"{s.Id} must open");
00314:                 Assert.False(string.IsNullOrWhiteSpace(vm.FactionId), $"{s.Id} missing faction");
00315:                 Assert.False(string.IsNullOrWhiteSpace(vm.FactionName), $"{s.Id} missing faction name");
00316:                 Assert.False(string.IsNullOrWhiteSpace(vm.LeaderName), $"{s.Id} missing leader");
00317:                 Assert.False(string.IsNullOrWhiteSpace(vm.StanceTellLine),
00318:                     $"{s.Id}: tell engine must resolve a line for stance {s.Stance} at trust {s.Trust}");
00319:                 Assert.False(string.IsNullOrWhiteSpace(vm.RadioTickerLine), $"{s.Id} missing radio ticker");
00320:                 Assert.False(string.IsNullOrWhiteSpace(vm.WorldPhaseLabel), $"{s.Id} missing world phase");
00321:             }
00322:         }
00323:
00324:         [Fact]
00325:         public void Catalog_TellSelectionIsSeedDeterministic()
00326:         {
00327:             var tells = LoadTells();
00328:             foreach (var s in LoadScenarios())
00329:             {
00330:                 var a = TradeScreenScenarioLoader.CreateBinding(s, tells, new SeededRng(2026));
00331:                 var b = TradeScreenScenarioLoader.CreateBinding(s, tells, new SeededRng(2026));
00332:                 Assert.Equal(a.ViewModel.StanceTellId, b.ViewModel.StanceTellId);
00333:                 Assert.Equal(a.ViewModel.StanceTellLine, b.ViewModel.StanceTellLine);
00334:             }
00335:         }
00336:
00337:         // ── Empty-table discipline ───────────────────────────────────
00338:
00339:         [Fact]
00340:         public void Catalog_OnlyDeliberateEmptyTablesAreEmpty()
00341:         {
00342:             foreach (var s in LoadScenarios())
00343:             {
00344:                 bool bare = s.PlayerOffers.Count == 0 && s.FactionDemands.Count == 0 && s.BiologicalOffers.Count == 0;
00345:                 if (bare)
00346:                 {
00347:                     Assert.Equal(TradeStance.Refuse, s.Stance);
00348:                     Assert.Equal(TradeFairness.EmptyTable, s.ExpectedFairness);
00349:                 }
00350:             }
00351:         }
00352:
00353:         [Fact]
00354:         public void Catalog_DebtCollectorTableIsDemandsOnly()
00355:         {
00356:             // Settlement of Accounts: the player's edge is bare, the ledger edge
00357:             // is heavy. Presentation of the outstanding obligation — never a
00358:             // shadow debt ledger.
00359:             var s = LoadScenarios().First(x => x.Id == "settlement_of_accounts");
00360:             Assert.Empty(s.PlayerOffers);
00361:             Assert.NotEmpty(s.FactionDemands);
00362:             Assert.Equal(TradeFairness.Short, s.ExpectedFairness);
00363:             Assert.False(s.ConfirmSucceeds);
00364:         }
00365:
00366:         // ── Differentiation audit (plan §61B.14) ─────────────────────
00367:
00368:         [Fact]
00369:         public void Catalog_EveryScenarioDiffersFromNearestNeighborInTwoDimensions()
00370:         {
00371:             var scenarios = LoadScenarios();
00372:             string Signature(TradeScreenScenario s) =>
00373:                 $"{s.Stance}|{Band(s.Trust)}|{string.Join(';', s.PlayerOffers.Concat(s.FactionDemands).Select(l => l.ItemId).OrderBy(x => x, StringComparer.Ordinal))}|" +
00374:                 $"{string.Join(';', s.PriceShocks.Select(p => p.Kind).OrderBy(x => x))}|{string.Join(';', s.Scarcity.Select(x => x.ItemId).OrderBy(x => x, StringComparer.Ordinal))}";
00375:
00376:             static string Band(float trust) =>
00377:                 trust <= -40f ? "hostile" : trust <= 0f ? "wary" : trust <= 40f ? "neutral" : "warm";
00378:
00379:             for (int i = 0; i < scenarios.Count; i++)
00380:             {
00381:                 for (int j = i + 1; j < scenarios.Count; j++)
00382:                 {
00383:                     var a = scenarios[i];
00384:                     var b = scenarios[j];
00385:                     int dims = 0;
00386:                     if (a.Stance != b.Stance) dims++;
00387:                     if (Band(a.Trust) != Band(b.Trust)) dims++;
00388:                     if (Signature(a) != Signature(b) &&
00389:                         (!a.PlayerOffers.Concat(a.FactionDemands).Select(l => l.ItemId).OrderBy(x => x, StringComparer.Ordinal)
00390:                             .SequenceEqual(b.PlayerOffers.Concat(b.FactionDemands).Select(l => l.ItemId).OrderBy(x => x, StringComparer.Ordinal)))) dims++;
00391:                     if (!a.PriceShocks.Select(p => p.Kind).OrderBy(x => x).SequenceEqual(b.PriceShocks.Select(p => p.Kind).OrderBy(x => x))) dims++;
00392:                     if (!a.Scarcity.Select(x => x.ItemId).OrderBy(x => x, StringComparer.Ordinal)
00393:                             .SequenceEqual(b.Scarcity.Select(x => x.ItemId).OrderBy(x => x, StringComparer.Ordinal))) dims++;
00394:                     if (a.ExpectedFairness != b.ExpectedFairness) dims++;
00395:
00396:                     Assert.True(dims >= 2,
00397:                         $"Scenarios '{a.Id}' and '{b.Id}' differ in only {dims} dimension(s) — semantic duplication");
00398:                 }
00399:             }
00400:         }
00401:
00402:         // ── Old-save / characterization protection ───────────────────
00403:
00404:         [Fact]
00405:         public void Catalog_OriginalThreeScenariosKeepLockedContracts()
00406:         {
00407:             var byId = LoadScenarios().ToDictionary(s => s.Id, StringComparer.Ordinal);
00408:
00409:             var fair = byId["fair_deal"];
00410:             Assert.Equal("scavenger_camp", fair.FactionId);
00411:             Assert.Equal(TradeStance.Trade, fair.Stance);
00412:             Assert.Equal(22f, fair.Trust, 2);
00413:             Assert.Equal(TradeFairness.Fair, fair.ExpectedFairness);
00414:             Assert.True(fair.ConfirmSucceeds);
00415:
00416:             var shortOffer = byId["offer_short"];
00417:             Assert.Equal("upland_militia", shortOffer.FactionId);
00418:             Assert.Equal(TradeFairness.Short, shortOffer.ExpectedFairness);
00419:             Assert.False(shortOffer.ConfirmSucceeds);
00420:
00421:             var empty = byId["empty_table"];
00422:             Assert.Equal("rot_farmers", empty.FactionId);
00423:             Assert.Equal(TradeStance.Refuse, empty.Stance);
00424:             Assert.Equal(TradeFairness.EmptyTable, empty.ExpectedFairness);
00425:             Assert.False(empty.ConfirmSucceeds);
00426:         }
00427:
00428:         [Fact]
00429:         public void Catalog_RootShapeIsPreserved()
00430:         {
00431:             using var doc = JsonDocument.Parse(File.ReadAllText(DataPath("trade_screen_scenarios.json")));
00432:             var root = doc.RootElement;
00433:             Assert.Equal(1, root.GetProperty("schema_version").GetInt32());
00434:             Assert.Equal(1, root.GetProperty("version").GetInt32());
00435:             Assert.True(root.TryGetProperty("scenarios", out var scenarios));
00436:             Assert.Equal(JsonValueKind.Array, scenarios.ValueKind);
00437:             Assert.Equal(15, scenarios.GetArrayLength());
00438:         }
00439:     }
00440: }
```

## `Ashfall.Core.Tests/Economy/TradeScreenPresenterSnapshotTests.cs` — 358 lines; 16,499 bytes; SHA-256 `72be191907fc64c96e71ead28b71dd855df312544c17377e3e30d8206d93a7ce`
Declaration index:
- 00019: public class TradeScreenPresenterSnapshotTests
- 00021: private static FactionStanceEngine CreateTestStanceEngine(float trust = 0f, float raidAggression = 0.35f)
- 00039: public void BarterTotals_CalculatesPlayerAndFactionWorthCorrectly()
- 00096: public void BarterTotals_QualitativeThresholds_Snapshot()
- 00125: public void DisabledActions_EmptyTable_ConfirmDisabled()
- 00138: public void DisabledActions_OfferShort_ConfirmDisabled()
- 00160: public void DisabledActions_StanceGating_WillTradeControlsConfirm(float trust, bool expectCanConfirmWhenFair)
- 00178: public void SelectionRestoration_CaptureAndRestore_PreservesAllOffersTotalsAndActions()
- 00257: public void SelectionRestoration_FactionSwitching_PreservesIndependence()
- 00298: public void BiologicalOfferings_PricingCalculations()
- 00325: public void RadioTicker_UpdatesOnRecalculateAndExecution()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core;
00005: using Ashfall.Core.Economy;
00006: using Ashfall.Core.Radio;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     /// <summary>
00012:     /// Snapshot and verification tests for TradeScreenPresenter (Item 77):
00013:     /// 1. Barter totals & qualitative worth calculations
00014:     /// 2. Disabled actions & stance/fairness gating
00015:     /// 3. Selection capture, round-trip, and deterministic restoration
00016:     /// 4. Biological offerings pricing
00017:     /// 5. Radio ticker resolution
00018:     /// </summary>
00019:     public class TradeScreenPresenterSnapshotTests
00020:     {
00021:         private static FactionStanceEngine CreateTestStanceEngine(float trust = 0f, float raidAggression = 0.35f)
00022:         {
00023:             var engine = new FactionStanceEngine();
00024:             engine.RegisterFaction(new FactionThresholds(
00025:                 "scavenger_camp",
00026:                 raidThreshold: -50f,
00027:                 robThreshold: -20f,
00028:                 minTrustToTrade: -40f,
00029:                 intelShareThreshold: 40f,
00030:                 raidAggression: raidAggression,
00031:                 trustInversion: false,
00032:                 healthyRadiationCeiling: 20f,
00033:                 highRadiationFloor: 60f));
00034:             engine.SetTrust("scavenger_camp", trust);
00035:             return engine;
00036:         }
00037:
00038:         [Fact]
00039:         public void BarterTotals_CalculatesPlayerAndFactionWorthCorrectly()
00040:         {
00041:             var stance = CreateTestStanceEngine(trust: 10f);
00042:             var presenter = new TradeScreenPresenter(
00043:                 stance,
00044:                 unitPriceLookup: id => id switch
00045:                 {
00046:                     "clean_water" => 20f,
00047:                     "canned_food" => 15f,
00048:                     "medical_kit" => 40f,
00049:                     _ => 10f
00050:                 },
00051:                 displayNameLookup: id => id switch
00052:                 {
00053:                     "clean_water" => "Clean Water",
00054:                     "canned_food" => "Canned Food",
00055:                     "medical_kit" => "Medical Kit",
00056:                     _ => id
00057:                 });
00058:
00059:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00060:
00061:             // 1. Initial state (empty)
00062:             Assert.Equal(0f, presenter.ViewModel.PlayerOfferValue);
00063:             Assert.Equal(0f, presenter.ViewModel.FactionAskValue);
00064:             Assert.Equal("None", TradeWorthLabels.Format(presenter.ViewModel.PlayerOfferValue));
00065:             Assert.Equal("None", TradeWorthLabels.Format(presenter.ViewModel.FactionAskValue));
00066:             Assert.Equal(TradeFairness.EmptyTable, presenter.ViewModel.Fairness);
00067:
00068:             // 2. Add player item offers: 2x canned_food (30) + 1x clean_water (20) = 50
00069:             presenter.SetPlayerOffer("canned_food", 2);
00070:             presenter.SetPlayerOffer("clean_water", 1);
00071:             Assert.Equal(50f, presenter.ViewModel.PlayerOfferValue);
00072:             Assert.Equal("Modest", TradeWorthLabels.Format(presenter.ViewModel.PlayerOfferValue));
00073:             Assert.Equal(2, presenter.ActiveOfferCount);
00074:
00075:             // 3. Add biological offering: 1x PintOfBlood (25) + 1x BoneMarrow (50) -> +75 = 125 total
00076:             presenter.SetBiologicalOffer(BiologicalTradeItem.PintOfBlood, 1);
00077:             presenter.SetBiologicalOffer(BiologicalTradeItem.BoneMarrow, 1);
00078:             Assert.Equal(125f, presenter.ViewModel.PlayerOfferValue);
00079:             Assert.Equal("Substantial", TradeWorthLabels.Format(presenter.ViewModel.PlayerOfferValue));
00080:             Assert.Equal(2, presenter.ActiveBioCount);
00081:
00082:             // 4. Add faction ask: 2x medical_kit (80) + 1x clean_water (20) = 100
00083:             presenter.SetFactionAsk("medical_kit", 2);
00084:             presenter.SetFactionAsk("clean_water", 1);
00085:             Assert.Equal(100f, presenter.ViewModel.FactionAskValue);
00086:             Assert.Equal("Substantial", TradeWorthLabels.Format(presenter.ViewModel.FactionAskValue));
00087:             Assert.Equal(2, presenter.ActiveAskCount);
00088:
00089:             // 5. Check scale comparison: 125 >= 100 -> Fair
00090:             Assert.Equal(TradeFairness.Fair, presenter.ViewModel.Fairness);
00091:             Assert.Equal("DEAL IS FAIR", presenter.ViewModel.FairnessLabel);
00092:             Assert.True(presenter.ViewModel.CanConfirm);
00093:         }
00094:
00095:         [Fact]
00096:         public void BarterTotals_QualitativeThresholds_Snapshot()
00097:         {
00098:             var cases = new (float Value, string ExpectedLabel)[]
00099:             {
00100:                 (0f, "None"),
00101:                 (10f, "Sparse"),
00102:                 (19.9f, "Sparse"),
00103:                 (20f, "Modest"),
00104:                 (59.9f, "Modest"),
00105:                 (60f, "Substantial"),
00106:                 (149.9f, "Substantial"),
00107:                 (150f, "Generous"),
00108:                 (500f, "Generous")
00109:             };
00110:             var failures = new List<string>();
00111:
00112:             foreach (var testCase in cases)
00113:             {
00114:                 var actual = TradeWorthLabels.Format(testCase.Value);
00115:                 if (actual != testCase.ExpectedLabel)
00116:                 {
00117:                     failures.Add($"value {testCase.Value}: expected {testCase.ExpectedLabel}, got {actual}");
00118:                 }
00119:             }
00120:
00121:             Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
00122:         }
00123:
00124:         [Fact]
00125:         public void DisabledActions_EmptyTable_ConfirmDisabled()
00126:         {
00127:             var stance = CreateTestStanceEngine(trust: 20f);
00128:             var presenter = new TradeScreenPresenter(stance);
00129:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00130:
00131:             Assert.False(presenter.ViewModel.CanConfirm);
00132:             Assert.Equal(TradeFairness.EmptyTable, presenter.ViewModel.Fairness);
00133:             Assert.Equal("EMPTY TABLE", presenter.ViewModel.FairnessLabel);
00134:             Assert.False(presenter.TryConfirmTrade());
00135:         }
00136:
00137:         [Fact]
00138:         public void DisabledActions_OfferShort_ConfirmDisabled()
00139:         {
00140:             var stance = CreateTestStanceEngine(trust: 20f);
00141:             var presenter = new TradeScreenPresenter(
00142:                 stance,
00143:                 unitPriceLookup: id => id == "clean_water" ? 30f : 10f);
00144:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00145:
00146:             presenter.SetPlayerOffer("canned_food", 1); // 10
00147:             presenter.SetFactionAsk("clean_water", 1);  // 30
00148:
00149:             Assert.False(presenter.ViewModel.CanConfirm);
00150:             Assert.Equal(TradeFairness.Short, presenter.ViewModel.Fairness);
00151:             Assert.Equal("OFFER SHORT", presenter.ViewModel.FairnessLabel);
00152:             Assert.False(presenter.TryConfirmTrade());
00153:         }
00154:
00155:         [Theory]
00156:         [InlineData(-60f, false)] // Below raid threshold (-50): HostileRaid -> willTrade = false
00157:         [InlineData(-30f, false)] // Below rob threshold (-20): Rob -> willTrade = false
00158:         [InlineData(10f, true)]   // Between -20 and 40: Trade -> willTrade = true
00159:         [InlineData(50f, true)]   // Above intelShare (40): ShareIntel -> willTrade = true
00160:         public void DisabledActions_StanceGating_WillTradeControlsConfirm(float trust, bool expectCanConfirmWhenFair)
00161:         {
00162:             var stance = CreateTestStanceEngine(trust: trust);
00163:             var presenter = new TradeScreenPresenter(
00164:                 stance,
00165:                 unitPriceLookup: _ => 20f);
00166:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00167:
00168:             // Offer 2x (40) vs Ask 1x (20) -> mathematically fair
00169:             presenter.SetPlayerOffer("canned_food", 2);
00170:             presenter.SetFactionAsk("clean_water", 1);
00171:
00172:             Assert.Equal(TradeFairness.Fair, presenter.ViewModel.Fairness);
00173:             Assert.Equal(expectCanConfirmWhenFair, presenter.ViewModel.CanConfirm);
00174:             Assert.Equal(expectCanConfirmWhenFair, presenter.TryConfirmTrade());
00175:         }
00176:
00177:         [Fact]
00178:         public void SelectionRestoration_CaptureAndRestore_PreservesAllOffersTotalsAndActions()
00179:         {
00180:             var stance = CreateTestStanceEngine(trust: 15f);
00181:             var presenter = new TradeScreenPresenter(
00182:                 stance,
00183:                 unitPriceLookup: id => id switch
00184:                 {
00185:                     "canned_food" => 12f,
00186:                     "clean_water" => 25f,
00187:                     "ammo" => 5f,
00188:                     _ => 10f
00189:                 },
00190:                 displayNameLookup: id => id.Replace('_', ' '));
00191:
00192:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00193:
00194:             // 1. Configure complex selections
00195:             presenter.SetPlayerOffer("canned_food", 3); // 36
00196:             presenter.SetPlayerOffer("ammo", 10);        // 50
00197:             presenter.SetBiologicalOffer(BiologicalTradeItem.PintOfBlood, 2); // 50
00198:             presenter.SetBiologicalOffer(BiologicalTradeItem.Plasma, 1);      // 75
00199:             // Total player value: 36 + 50 + 50 + 75 = 211
00200:
00201:             presenter.SetFactionAsk("clean_water", 4);   // 100
00202:             // Total faction value: 100
00203:
00204:             float initialPlayerVal = presenter.ViewModel.PlayerOfferValue;
00205:             float initialFactionVal = presenter.ViewModel.FactionAskValue;
00206:             var initialFairness = presenter.ViewModel.Fairness;
00207:             bool initialCanConfirm = presenter.ViewModel.CanConfirm;
00208:             string initialSummary = presenter.BuildQuoteSummary();
00209:
00210:             Assert.Equal(211f, initialPlayerVal);
00211:             Assert.Equal(100f, initialFactionVal);
00212:             Assert.Equal(TradeFairness.Fair, initialFairness);
00213:             Assert.True(initialCanConfirm);
00214:
00215:             // 2. Capture selection snapshot
00216:             var selectionSnapshot = presenter.CaptureSelection();
00217:             Assert.Equal(2, selectionSnapshot.PlayerOffers.Count);
00218:             Assert.Equal(3, selectionSnapshot.PlayerOffers["canned_food"]);
00219:             Assert.Equal(10, selectionSnapshot.PlayerOffers["ammo"]);
00220:             Assert.Equal(1, selectionSnapshot.FactionAsks.Count);
00221:             Assert.Equal(4, selectionSnapshot.FactionAsks["clean_water"]);
00222:             Assert.Equal(2, selectionSnapshot.BiologicalOffers.Count);
00223:             Assert.Equal(2, selectionSnapshot.BiologicalOffers[BiologicalTradeItem.PintOfBlood]);
00224:             Assert.Equal(1, selectionSnapshot.BiologicalOffers[BiologicalTradeItem.Plasma]);
00225:
00226:             // 3. Clear selections and assert empty state
00227:             presenter.ClearOffers();
00228:             Assert.Equal(0, presenter.ActiveOfferCount);
00229:             Assert.Equal(0, presenter.ActiveAskCount);
00230:             Assert.Equal(0, presenter.ActiveBioCount);
00231:             Assert.Equal(0f, presenter.ViewModel.PlayerOfferValue);
00232:             Assert.Equal(0f, presenter.ViewModel.FactionAskValue);
00233:             Assert.Equal(TradeFairness.EmptyTable, presenter.ViewModel.Fairness);
00234:             Assert.False(presenter.ViewModel.CanConfirm);
00235:
00236:             // 4. Restore selection snapshot
00237:             presenter.RestoreSelection(selectionSnapshot);
00238:
00239:             // 5. Assert all counts and state restored with exact fidelity
00240:             Assert.Equal(2, presenter.ActiveOfferCount);
00241:             Assert.Equal(1, presenter.ActiveAskCount);
00242:             Assert.Equal(2, presenter.ActiveBioCount);
00243:             Assert.Equal(3, presenter.GetPlayerOfferCount("canned_food"));
00244:             Assert.Equal(10, presenter.GetPlayerOfferCount("ammo"));
00245:             Assert.Equal(4, presenter.GetFactionAskCount("clean_water"));
00246:             Assert.Equal(2, presenter.GetBiologicalOfferCount(BiologicalTradeItem.PintOfBlood));
00247:             Assert.Equal(1, presenter.GetBiologicalOfferCount(BiologicalTradeItem.Plasma));
00248:
00249:             Assert.Equal(initialPlayerVal, presenter.ViewModel.PlayerOfferValue);
00250:             Assert.Equal(initialFactionVal, presenter.ViewModel.FactionAskValue);
00251:             Assert.Equal(initialFairness, presenter.ViewModel.Fairness);
00252:             Assert.Equal(initialCanConfirm, presenter.ViewModel.CanConfirm);
00253:             Assert.Equal(initialSummary, presenter.BuildQuoteSummary());
00254:         }
00255:
00256:         [Fact]
00257:         public void SelectionRestoration_FactionSwitching_PreservesIndependence()
00258:         {
00259:             var stance = CreateTestStanceEngine(trust: 10f);
00260:             stance.RegisterFaction(new FactionThresholds(
00261:                 "nomads",
00262:                 raidThreshold: -60f,
00263:                 robThreshold: -30f,
00264:                 minTrustToTrade: -20f,
00265:                 intelShareThreshold: 50f));
00266:             stance.SetTrust("nomads", 25f);
00267:
00268:             var presenter = new TradeScreenPresenter(
00269:                 stance,
00270:                 unitPriceLookup: id => id == "bread" ? 8f : 15f);
00271:
00272:             // Setup Scavenger Camp trade
00273:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00274:             presenter.SetPlayerOffer("bread", 5); // 40
00275:             presenter.SetFactionAsk("clean_water", 2); // 30
00276:             var scavSnapshot = presenter.CaptureSelection();
00277:
00278:             // Switch to Nomads (which resets offers on Open)
00279:             presenter.Open("nomads", "Nomads", "Zara", 2);
00280:             Assert.Equal(0, presenter.ActiveOfferCount);
00281:             Assert.Equal(0, presenter.ActiveAskCount);
00282:
00283:             presenter.SetPlayerOffer("bread", 2); // 16
00284:             var nomadSnapshot = presenter.CaptureSelection();
00285:             Assert.Equal(2, nomadSnapshot.PlayerOffers["bread"]);
00286:
00287:             // Switch back to Scavenger Camp and restore selection
00288:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00289:             presenter.RestoreSelection(scavSnapshot);
00290:
00291:             Assert.Equal(5, presenter.GetPlayerOfferCount("bread"));
00292:             Assert.Equal(2, presenter.GetFactionAskCount("clean_water"));
00293:             Assert.Equal(40f, presenter.ViewModel.PlayerOfferValue);
00294:             Assert.Equal(30f, presenter.ViewModel.FactionAskValue);
00295:         }
00296:
00297:         [Fact]
00298:         public void BiologicalOfferings_PricingCalculations()
00299:         {
00300:             Assert.Equal(25f, TradePricing.BioUnitValue(BiologicalTradeItem.PintOfBlood));
00301:             Assert.Equal(50f, TradePricing.BioUnitValue(BiologicalTradeItem.BoneMarrow));
00302:             Assert.Equal(75f, TradePricing.BioUnitValue(BiologicalTradeItem.Plasma));
00303:             Assert.Equal(100f, TradePricing.BioUnitValue(BiologicalTradeItem.Organ));
00304:
00305:             var stance = CreateTestStanceEngine(trust: 10f);
00306:             var presenter = new TradeScreenPresenter(stance);
00307:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00308:
00309:             presenter.SetBiologicalOffer(BiologicalTradeItem.PintOfBlood, 2); // 50
00310:             presenter.SetBiologicalOffer(BiologicalTradeItem.BoneMarrow, 1);  // 50
00311:             presenter.SetBiologicalOffer(BiologicalTradeItem.Plasma, 2);      // 150
00312:             presenter.SetBiologicalOffer(BiologicalTradeItem.Organ, 1);       // 100
00313:
00314:             // 50 + 50 + 150 + 100 = 350
00315:             Assert.Equal(350f, presenter.ViewModel.PlayerOfferValue);
00316:             Assert.Equal(4, presenter.ActiveBioCount);
00317:
00318:             // Removing count clears it
00319:             presenter.SetBiologicalOffer(BiologicalTradeItem.Organ, 0);
00320:             Assert.Equal(3, presenter.ActiveBioCount);
00321:             Assert.Equal(250f, presenter.ViewModel.PlayerOfferValue);
00322:         }
00323:
00324:         [Fact]
00325:         public void RadioTicker_UpdatesOnRecalculateAndExecution()
00326:         {
00327:             var stance = CreateTestStanceEngine(trust: 10f);
00328:             var radio = FactionRadioEngine.LoadFromJson(@"{
00329:                 ""factions"": {
00330:                     ""scavenger_camp"": {
00331:                         ""frequency_mhz"": 104.7,
00332:                         ""callsign"": ""SCAV-1"",
00333:                         ""intercept_chatter"": [ ""Listening on wire."" ],
00334:                         ""trade_reaction"": [ ""Deal accepted."" ],
00335:                         ""parley_resolution"": [ ""Parley acknowledged."" ]
00336:                     }
00337:                 }
00338:             }");
00339:
00340:             var presenter = new TradeScreenPresenter(
00341:                 stance,
00342:                 unitPriceLookup: _ => 20f,
00343:                 radioProvider: radio,
00344:                 rng: new SeededRng(2026));
00345:
00346:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00347:             Assert.Contains("Listening on wire.", presenter.ViewModel.RadioTickerLine);
00348:
00349:             presenter.SetPlayerOffer("food", 2);
00350:             presenter.SetFactionAsk("water", 1);
00351:             Assert.True(presenter.TryConfirmTrade());
00352:             Assert.Contains("Deal accepted.", presenter.ViewModel.RadioTickerLine);
00353:
00354:             presenter.TryDemandParley();
00355:             Assert.Contains("Parley acknowledged.", presenter.ViewModel.RadioTickerLine);
00356:         }
00357:     }
00358: }
```

## `Ashfall.Core.Tests/TradeScreenSeamTests.cs` — 329 lines; 14,462 bytes; SHA-256 `7940a9f2745852452d4a37666199d3187914ce18d05551e675588fa27169cfc6`
Declaration index:
- 00016: public class TradeScreenSeamTests
- 00018: private static string ReadDataFile(string fileName)
- 00029: private static IReadOnlyList<TradeScreenScenario> LoadScenarios()
- 00034: private static TradeTellEngine LoadTells()
- 00039: private static FactionStanceEngine CreateStanceEngine()
- 00058: public void Scenarios_LoadAllThreeFromData()
- 00071: public void Scenario_FairDeal_ComputedFairnessMatchesDataExpectation()
- 00093: public void Scenario_OfferShort_BlocksConfirmAndKeepsStanceLegible()
- 00110: public void Scenario_EmptyTable_IsDeliberateNotBroken()
- 00130: public void Scenario_IntentSink_CloseRecordsTradedFlag()
- 00140: private static TradeScreenScenario GetScenario(string id)
- 00154: private sealed class MutationCountingStanceProvider : IFactionStanceProvider
- 00161: public TradeStance GetStance(string factionId) => _inner.GetStance(factionId);
- 00162: public bool WillTrade(string factionId) => _inner.WillTrade(factionId);
- 00163: public bool WillShareIntel(string factionId) => _inner.WillShareIntel(factionId);
- 00164: public float GetTrust(string factionId) => _inner.GetTrust(factionId);
- 00165: public float GetEffectiveTrust(string factionId) => _inner.GetEffectiveTrust(factionId);
- 00166: public float ModifyTrust(string factionId, float delta) { Mutations++; return _inner.ModifyTrust(factionId, delta); }
- 00167: public void SetTrust(string factionId, float value) { Mutations++; _inner.SetTrust(factionId, value); }
- 00168: public float GetRaidAggression(string factionId) => _inner.GetRaidAggression(factionId);
- 00169: public void SetRaidAggression(string factionId, float value) { Mutations++; _inner.SetRaidAggression(factionId, value); }
- 00170: public bool IsFactionActive(string factionId) => _inner.IsFactionActive(factionId);
- 00174: public void Presenter_MapsProvidersOntoViewModel()
- 00205: public void Presenter_ZeroMutation_InvariantHolds()
- 00224: public void Presenter_ApiParity_TradeScreenUISurface()
- 00264: public void Presenter_BioOffersPricedByCoreRule()
- 00280: public void Presenter_RoutesExecutionThroughSink()
- 00301: private sealed class RecordingExecutionSink : ITradeExecutionSink
- 00308: public bool WillTrade(string factionId) => true;
- 00310: public bool TryExecuteTrade(
- 00322: public bool TryDemandParley(string factionId)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Economy;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     /// <summary>
00012:     /// Negotiation Table seam: Act 0 scenarios (fair / short / empty),
00013:     /// Track B presenter mapping + zero-mutation invariant + TradeScreenUI
00014:     /// API parity.
00015:     /// </summary>
00016:     public class TradeScreenSeamTests
00017:     {
00018:         private static string ReadDataFile(string fileName)
00019:         {
00020:             string path = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", fileName);
00021:             if (!File.Exists(path))
00022:             {
00023:                 path = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", fileName);
00024:             }
00025:             Assert.True(File.Exists(path), $"Data file not found at {path}");
00026:             return File.ReadAllText(path);
00027:         }
00028:
00029:         private static IReadOnlyList<TradeScreenScenario> LoadScenarios()
00030:         {
00031:             return TradeScreenScenarioLoader.LoadFromJson(ReadDataFile("trade_screen_scenarios.json"));
00032:         }
00033:
00034:         private static TradeTellEngine LoadTells()
00035:         {
00036:             return TradeTellEngine.LoadFromJson(ReadDataFile("trade_tell_lines.json"));
00037:         }
00038:
00039:         private static FactionStanceEngine CreateStanceEngine()
00040:         {
00041:             var engine = new FactionStanceEngine();
00042:             engine.RegisterFaction(new FactionThresholds(
00043:                 "scavenger_camp",
00044:                 raidThreshold: -50f,
00045:                 robThreshold: -20f,
00046:                 minTrustToTrade: -40f,
00047:                 intelShareThreshold: 40f,
00048:                 raidAggression: 0.35f,
00049:                 trustInversion: false,
00050:                 healthyRadiationCeiling: 20f,
00051:                 highRadiationFloor: 60f));
00052:             return engine;
00053:         }
00054:
00055:         // ── Act 0: mock scenarios ────────────────────────────────────
00056:
00057:         [Fact]
00058:         public void Scenarios_LoadAllThreeFromData()
00059:         {
00060:             var scenarios = LoadScenarios();
00061:
00062:             // Plan 61: catalog expanded from 3 to 15 (12 new scenarios across
00063:             // eight trader archetypes). The three originals remain pinned.
00064:             Assert.True(scenarios.Count >= 15, $"Expected at least 15 scenarios, got {scenarios.Count}");
00065:             Assert.Contains(scenarios, s => s.Id == "fair_deal");
00066:             Assert.Contains(scenarios, s => s.Id == "offer_short");
00067:             Assert.Contains(scenarios, s => s.Id == "empty_table");
00068:         }
00069:
00070:         [Fact]
00071:         public void Scenario_FairDeal_ComputedFairnessMatchesDataExpectation()
00072:         {
00073:             var binding = TradeScreenScenarioLoader.CreateBinding(
00074:                 GetScenario("fair_deal"), LoadTells(), new SeededRng(2026));
00075:
00076:             var vm = binding.ViewModel;
00077:             Assert.True(vm.IsOpen);
00078:             Assert.Equal(TradeFairness.Fair, vm.Fairness);
00079:             Assert.Equal("DEAL IS FAIR", vm.FairnessLabel);
00080:             Assert.True(vm.CanConfirm);
00081:             Assert.False(string.IsNullOrWhiteSpace(vm.StanceTellLine));
00082:
00083:             // 3x18 + 1x15 + 1 pint of blood (25) = 94 vs 2x22 = 44.
00084:             Assert.Equal(94f, vm.PlayerOfferValue, 2);
00085:             Assert.Equal(44f, vm.FactionAskValue, 2);
00086:
00087:             // Intent routing through the seam.
00088:             Assert.True(binding.Intents.TryConfirmTrade());
00089:             Assert.Equal(1, binding.Intents.ConfirmCalls);
00090:         }
00091:
00092:         [Fact]
00093:         public void Scenario_OfferShort_BlocksConfirmAndKeepsStanceLegible()
00094:         {
00095:             var binding = TradeScreenScenarioLoader.CreateBinding(
00096:                 GetScenario("offer_short"), LoadTells(), new SeededRng(2026));
00097:
00098:             var vm = binding.ViewModel;
00099:             Assert.Equal(TradeFairness.Short, vm.Fairness);
00100:             Assert.Equal("OFFER SHORT", vm.FairnessLabel);
00101:             Assert.False(vm.CanConfirm);
00102:             Assert.True(vm.ConsecutiveRepels > 0);
00103:
00104:             // The mock sink still routes and records intent; the verdict is data-defined.
00105:             Assert.False(binding.Intents.TryConfirmTrade());
00106:             Assert.Equal(1, binding.Intents.ConfirmCalls);
00107:         }
00108:
00109:         [Fact]
00110:         public void Scenario_EmptyTable_IsDeliberateNotBroken()
00111:         {
00112:             var binding = TradeScreenScenarioLoader.CreateBinding(
00113:                 GetScenario("empty_table"), LoadTells(), new SeededRng(2026));
00114:
00115:             var vm = binding.ViewModel;
00116:             Assert.Equal(TradeFairness.EmptyTable, vm.Fairness);
00117:             Assert.Equal("EMPTY TABLE", vm.FairnessLabel);
00118:             Assert.False(vm.CanConfirm);
00119:             Assert.Empty(vm.PlayerOffers);
00120:             Assert.Empty(vm.FactionDemands);
00121:             Assert.Empty(vm.BiologicalOffers);
00122:
00123:             // A deliberate posture: stance, tell, and radio all speak.
00124:             Assert.Equal(TradeStance.Refuse, vm.Stance);
00125:             Assert.False(string.IsNullOrWhiteSpace(vm.StanceTellLine));
00126:             Assert.False(string.IsNullOrWhiteSpace(vm.RadioTickerLine));
00127:         }
00128:
00129:         [Fact]
00130:         public void Scenario_IntentSink_CloseRecordsTradedFlag()
00131:         {
00132:             var binding = TradeScreenScenarioLoader.CreateBinding(
00133:                 GetScenario("fair_deal"), LoadTells(), new SeededRng(2026));
00134:
00135:             binding.Intents.Close(traded: true);
00136:             Assert.Equal(1, binding.Intents.CloseCalls);
00137:             Assert.True(binding.Intents.LastCloseWasTraded);
00138:         }
00139:
00140:         private static TradeScreenScenario GetScenario(string id)
00141:         {
00142:             var scenarios = LoadScenarios();
00143:             foreach (var s in scenarios)
00144:             {
00145:                 if (s.Id == id) return s;
00146:             }
00147:             Assert.Fail($"Scenario {id} not found");
00148:             return null;
00149:         }
00150:
00151:         // ── Track B: presenter ───────────────────────────────────────
00152:
00153:         /// <summary>Counting decorator proving the presenter never mutates providers.</summary>
00154:         private sealed class MutationCountingStanceProvider : IFactionStanceProvider
00155:         {
00156:             private readonly IFactionStanceProvider _inner;
00157:             public int Mutations { get; private set; }
00158:
00159:             public MutationCountingStanceProvider(IFactionStanceProvider inner) { _inner = inner; }
00160:
00161:             public TradeStance GetStance(string factionId) => _inner.GetStance(factionId);
00162:             public bool WillTrade(string factionId) => _inner.WillTrade(factionId);
00163:             public bool WillShareIntel(string factionId) => _inner.WillShareIntel(factionId);
00164:             public float GetTrust(string factionId) => _inner.GetTrust(factionId);
00165:             public float GetEffectiveTrust(string factionId) => _inner.GetEffectiveTrust(factionId);
00166:             public float ModifyTrust(string factionId, float delta) { Mutations++; return _inner.ModifyTrust(factionId, delta); }
00167:             public void SetTrust(string factionId, float value) { Mutations++; _inner.SetTrust(factionId, value); }
00168:             public float GetRaidAggression(string factionId) => _inner.GetRaidAggression(factionId);
00169:             public void SetRaidAggression(string factionId, float value) { Mutations++; _inner.SetRaidAggression(factionId, value); }
00170:             public bool IsFactionActive(string factionId) => _inner.IsFactionActive(factionId);
00171:         }
00172:
00173:         [Fact]
00174:         public void Presenter_MapsProvidersOntoViewModel()
00175:         {
00176:             var stance = CreateStanceEngine();
00177:             var tuning = new HardcoreEconomyTuning();
00178:             tuning.Apply(new HardcoreEconomyTuningBundle(
00179:                 new[] { new ScarcityEntry(ScarcityTier.Critical, 2.0f, "1-50", new[] { "clean_water" }, "drought") },
00180:                 Array.Empty<FactionTradePreference>(),
00181:                 new[] { new PriceShockRule(PriceShockKind.PlumePassing, 2.5f, 10, new[] { "rad_pills" }, "rad plume") }
00182:             ));
00183:
00184:             var presenter = new TradeScreenPresenter(
00185:                 stance, tuning, LoadTells(), new SeededRng(2026),
00186:                 unitPriceLookup: id => id == "clean_water" ? 22f : 18f);
00187:             presenter.SetWorldContext("CivilWar", 5);
00188:             presenter.SetWatchedItems(new[] { "clean_water" });
00189:
00190:             Assert.True(presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1));
00191:
00192:             var vm = presenter.ViewModel;
00193:             Assert.True(vm.IsOpen);
00194:             Assert.Equal(TradeStance.Trade, vm.Stance);
00195:             Assert.Equal(0f, vm.Trust, 2);
00196:             Assert.Equal(0.35f, vm.Aggression, 2);
00197:             Assert.False(string.IsNullOrWhiteSpace(vm.StanceTellLine));
00198:             Assert.Single(vm.ShockBadges);
00199:             Assert.Equal(PriceShockKind.PlumePassing, vm.ShockBadges[0].Kind);
00200:             Assert.Single(vm.ScarcityMultipliers);
00201:             Assert.Equal(2.0f, vm.ScarcityMultipliers[0].Multiplier, 2);
00202:         }
00203:
00204:         [Fact]
00205:         public void Presenter_ZeroMutation_InvariantHolds()
00206:         {
00207:             var counting = new MutationCountingStanceProvider(CreateStanceEngine());
00208:             var presenter = new TradeScreenPresenter(counting, null, LoadTells(), new SeededRng(2026));
00209:
00210:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00211:             presenter.SetPlayerOffer("canned_food", 3);
00212:             presenter.SetFactionAsk("clean_water", 2);
00213:             presenter.SetBiologicalOffer(BiologicalTradeItem.PintOfBlood, 1);
00214:             presenter.Recalculate();
00215:             presenter.TryConfirmTrade();
00216:             presenter.TryDemandParley();
00217:             presenter.BuildQuoteSummary();
00218:             presenter.Close(traded: false);
00219:
00220:             Assert.Equal(0, counting.Mutations);
00221:         }
00222:
00223:         [Fact]
00224:         public void Presenter_ApiParity_TradeScreenUISurface()
00225:         {
00226:             var presenter = new TradeScreenPresenter(
00227:                 CreateStanceEngine(), null, LoadTells(), new SeededRng(2026),
00228:                 unitPriceLookup: id => id == "clean_water" ? 30f : 18f,
00229:                 displayNameLookup: id => id == "canned_food" ? "Canned Food" : "Clean Water");
00230:
00231:             // Open rejects inactive factions like TradeScreenUI.Open.
00232:             Assert.False(presenter.Open("unknown_nomads", "Nomads", "None", 1));
00233:             Assert.True(presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1));
00234:
00235:             // SetPlayerOffer / SetFactionAsk drive the fairness verdict.
00236:             presenter.SetPlayerOffer("canned_food", 1);   // 18
00237:             presenter.SetFactionAsk("clean_water", 1);    // 30
00238:             Assert.Equal(TradeFairness.Short, presenter.ViewModel.Fairness);
00239:             Assert.False(presenter.TryConfirmTrade());
00240:
00241:             presenter.SetPlayerOffer("canned_food", 3);   // 54 >= 30
00242:             Assert.Equal(TradeFairness.Fair, presenter.ViewModel.Fairness);
00243:             Assert.True(presenter.TryConfirmTrade());
00244:
00245:             // Successful confirm clears the table, like TradeScreenUI.
00246:             Assert.Equal(TradeFairness.EmptyTable, presenter.ViewModel.Fairness);
00247:             Assert.Empty(presenter.ViewModel.PlayerOffers);
00248:
00249:             // BuildQuoteSummary is qualitative — no raw digit totals.
00250:             presenter.SetPlayerOffer("canned_food", 2);
00251:             presenter.SetFactionAsk("clean_water", 1);
00252:             string summary = presenter.BuildQuoteSummary();
00253:             Assert.Contains("DEAL IS FAIR", summary);
00254:             Assert.Contains("Canned Food", summary);
00255:             Assert.Contains("Clean Water", summary);
00256:             Assert.DoesNotContain("36.0", summary);
00257:
00258:             // Close collapses the open state.
00259:             presenter.Close(traded: true);
00260:             Assert.False(presenter.ViewModel.IsOpen);
00261:         }
00262:
00263:         [Fact]
00264:         public void Presenter_BioOffersPricedByCoreRule()
00265:         {
00266:             var presenter = new TradeScreenPresenter(
00267:                 CreateStanceEngine(), null, LoadTells(), new SeededRng(2026),
00268:                 unitPriceLookup: _ => 0f);
00269:
00270:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00271:             presenter.SetBiologicalOffer(BiologicalTradeItem.Organ, 1);   // 4*25 = 100
00272:             presenter.SetFactionAsk("clean_water", 3);                    // 0
00273:
00274:             // Bio-only offer still counts toward the scale; demands worth nothing => fair.
00275:             Assert.Equal(TradeFairness.Fair, presenter.ViewModel.Fairness);
00276:             Assert.Equal(100f, presenter.ViewModel.PlayerOfferValue, 2);
00277:         }
00278:
00279:         [Fact]
00280:         public void Presenter_RoutesExecutionThroughSink()
00281:         {
00282:             var recorder = new RecordingExecutionSink();
00283:             var presenter = new TradeScreenPresenter(
00284:                 CreateStanceEngine(), null, LoadTells(), new SeededRng(2026),
00285:                 unitPriceLookup: _ => 10f,
00286:                 executionSink: recorder);
00287:
00288:             presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
00289:             presenter.SetPlayerOffer("canned_food", 2);
00290:             presenter.SetFactionAsk("clean_water", 1);
00291:             Assert.True(presenter.TryConfirmTrade());
00292:
00293:             Assert.Equal(1, recorder.ExecuteCalls);
00294:             Assert.Equal("scavenger_camp", recorder.LastFactionId);
00295:             Assert.Equal(2, recorder.LastPlayerOffers["canned_food"]);
00296:
00297:             presenter.TryDemandParley();
00298:             Assert.Equal(1, recorder.ParleyCalls);
00299:         }
00300:
00301:         private sealed class RecordingExecutionSink : ITradeExecutionSink
00302:         {
00303:             public int ExecuteCalls { get; private set; }
00304:             public int ParleyCalls { get; private set; }
00305:             public string LastFactionId { get; private set; }
00306:             public IReadOnlyDictionary<string, int> LastPlayerOffers { get; private set; }
00307:
00308:             public bool WillTrade(string factionId) => true;
00309:
00310:             public bool TryExecuteTrade(
00311:                 string factionId,
00312:                 IReadOnlyDictionary<string, int> playerOffers,
00313:                 IReadOnlyDictionary<string, int> factionAsks,
00314:                 IReadOnlyDictionary<BiologicalTradeItem, int> biologicalOffers)
00315:             {
00316:                 ExecuteCalls++;
00317:                 LastFactionId = factionId;
00318:                 LastPlayerOffers = playerOffers;
00319:                 return true;
00320:             }
00321:
00322:             public bool TryDemandParley(string factionId)
00323:             {
00324:                 ParleyCalls++;
00325:                 return true;
00326:             }
00327:         }
00328:     }
00329: }
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
# Appendix M — External verification handoff

The following checks are to be run by the owning integrator after writing: character count, SHA-256 revalidation, path-token resolution, duplicate-heading/unsupported-claim scan, and `git diff --check`. The final ledger entry must report actual results, not this template.
