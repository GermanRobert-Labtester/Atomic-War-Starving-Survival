# Plan 83 — Weather Seasons: Ten-Window Calendar, Weather Effects, and Consumer Reachability

> **Rebuild status:** TERMINAL 10-WINDOW CONTENT + SEASON/ WEATHER CONSUMER AUDIT
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

The historical baseline was 4,812 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Reconcile the ten authored weather-season windows with the current weather system, campaign calendar, weather-intelligence coordinator, effects catalog, gates, and consumers. The 3→10 expansion is complete; the valuable work is proving how the active window is selected and where its weights are actually consumed, not inventing another seasonal model.

**Bounded outcome:** Audit `WeatherSystem`’s season loader and selection, `CampaignCalendar`, `WeatherIntelligenceCoordinator`, weather effects/gates, wildlife/greenhouse consumers, host wiring, and focused tests. Preserve the current weather owner and document any dormant or conflicting seasonal signals.

**Non-goals:** no second weather RNG, no duplicate season catalog, no new weather kind, no change to Year-of-Ash 180–360 canon, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `weather_seasons.json` contains 10 windows with start days 0 through 280 and seven weather weights.
- VERIFIED CURRENT: `WeatherSystem` contains the engine-free `weather_seasons.json` loader and current weather/effects state.
- VERIFIED CURRENT: `CampaignCalendar` and `WeatherIntelligenceCoordinator` project season/window facts to current consumers.
- HISTORICAL RECORD: Plan 83/Wave 40 records the 3→10 expansion and tests; this package does not claim a fresh run.

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

- `Assets/StreamingAssets/Data/weather_seasons.json` exists at 3,212 bytes; SHA-256 `a9032e68bc0d46db2ec732d5d79d655b2b96f228b85d87ff50781e0e8449f268`.
- `Assets/StreamingAssets/Data/weather_effects.json` exists at 7,615 bytes; SHA-256 `2879d31c294f9f84f274e14b5b266d02d73b4f7bb1abe0a1d8604023af846600`.
- `Assets/StreamingAssets/Data/weather_route_gates.json` exists at 10,986 bytes; SHA-256 `1957b31fbb87f1ad5a86f83ae9a38c01cd8f80701a4a04073e691b327a454f10`.
- `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json` exists at 23,204 bytes; SHA-256 `ad76e65163efc3860f4ef7d6b209479f27489c0f5cb34a717fe4e64bd5d64d00`.

# 3. Required Delta

Replace the old data-only brief with a current ten-window census, selection/effect trace, and consumer audit. Preserve the single weather/calendar owner and identify only evidence-backed residuals.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Which current production method selects the latest window and how are weights normalized?
Are all ten windows consumed by the live weather path, or are some only specialized/test consumers?
How do weather effects, route gates, dose, and forecast confidence consume the selected window?
Does any UI or host path create a second season or weather state?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| weather kind/effects and seasonal selection | `WeatherSystem` | `Assets/Ashfall.Core/World/WeatherSystem.cs` | Owns current weather state and authored effects/season inputs. |
| campaign season projection | `CampaignCalendar` | `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` | Owns calendar day/season projection; does not own weather rolls. |
| weather intelligence and orbital/season coordination | `WeatherIntelligenceCoordinator` | `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs` | Composes current weather owners and emits facts. |
| weather gates | `WeatherGate / WeatherRouteGate catalog owners` | `Assets/Ashfall.Core/World/WeatherGate.cs; Assets/StreamingAssets/Data/weather_route_gates.json` | Gates own access decisions; they do not select weather. |
| weather presentation | `WeatherPanel / WeatherForecastPanel` | `src/UI/WeatherPanel.cs; src/UI/WeatherForecastPanel.cs` | Present the current owner state and decision effects. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for weather-season calendar.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Weather Seasons: Ten-Window Calendar, Weather Effects, and Consumer Reachability                                               │
│ Integration route: DATA-ONLY + current weather/calendar consumer audit                             │
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

1. **WeatherSystem owns weather and season selection.**
2. **CampaignCalendar owns campaign-day projection.**
3. **WeatherEffectsCatalog owns kind effects.**
4. **Gates and consumers receive typed facts.**
5. **UI never invents a forecast or second state.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| weather kind/effects and seasonal selection | `WeatherSystem` | `Assets/Ashfall.Core/World/WeatherSystem.cs` | Owns current weather state and authored effects/season inputs. |
| campaign season projection | `CampaignCalendar` | `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` | Owns calendar day/season projection; does not own weather rolls. |
| weather intelligence and orbital/season coordination | `WeatherIntelligenceCoordinator` | `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs` | Composes current weather owners and emits facts. |
| weather gates | `WeatherGate / WeatherRouteGate catalog owners` | `Assets/Ashfall.Core/World/WeatherGate.cs; Assets/StreamingAssets/Data/weather_route_gates.json` | Gates own access decisions; they do not select weather. |
| weather presentation | `WeatherPanel / WeatherForecastPanel` | `src/UI/WeatherPanel.cs; src/UI/WeatherForecastPanel.cs` | Present the current owner state and decision effects. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load the ten season windows through the current WeatherSystem loader
2. select the latest valid window by campaign day using current calendar semantics
3. normalize authored weather weights through the current weather owner
4. apply weather effects through WeatherEffectsCatalog/current consumers
5. derive route/health/forecast consequences through existing owners
6. project the current season/effects into panels and briefings

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- season start days are strictly ordered and day zero is covered
- weights are finite, non-negative, and normalized by the current owner
- a missing season never fabricates a clear-sky fallback
- weather kind/effect rows remain the authority for consequences
- the same campaign day and seed produce the same weather trace
- forecast miss and severe-weather events are emitted by the current owner

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

Contract rules for weather-season calendar:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`weather_seasons.json` remains the ten-window authority. The current `WeatherSystem` owns selection and weather output; `weather_effects.json` owns per-kind effects. Audit whether each weight is used by current production code, tests, or only specialized consumers. Do not merge season weights into a new parallel weather table.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

Weather and calendar state use their existing owner/save paths; no new season save section is justified by static window rows. A future mutable forecast/season override must use the existing weather/calendar owner and its codec.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

Weather selection and effect calculation must use the existing campaign weather stream and ordinal window order. No wall-clock or hash iteration. Paired replay compares selected window, weather kind, effect row, route/health consequences, and event order.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

WeatherSystem/current weather owner emits weather changes and severe-weather facts. Gates, dose, expeditions, power, and panels consume those facts; they do not select a second season. A forecast miss is a current owner event, not a season-catalog side effect.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Host/WorldHostSession.cs` — loads weather/effects and coordinates current world intelligence
- `src/Main.World.cs` — campaign weather setup, ticks, and consequence routing
- `src/UI/WeatherPanel.cs` — presents current weather and season state
- `src/UI/WeatherForecastPanel.cs` — presents reliability/effects without inventing forecasts
- `src/Main.CampaignOwners.cs` — canonical day-owner ordering and event delivery

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Season windows are calendar and forecast context, not a second plot timeline. Almanac/forecast prose may describe the modeled window and effects, but it cannot announce an unmodeled storm, guarantee safety, or contradict current weather effects.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/World/WeatherSeasonExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/Plan83_74WeatherNarrativeIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/WeatherGateCatalogIntegrityTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/World/WeatherGateBalanceAuditTests.cs`

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
| Phase 0 — window/effect census | read ten windows, WeatherSystem, effects, calendar, and gates | current selection and effect axes are explicit | no undocumented scope or shortcut |
| Phase 1 — caller graph | trace production, tests, and specialized consumers | live versus dormant rows are classified | no undocumented scope or shortcut |
| Phase 2 — determinism/accessibility audit | check paired replay, severe states, and readable effects | no duplicate weather owner or misleading UI | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a proven consumer/coverage gap is promoted | one owner, one claim, focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/weather_seasons.json` | READ ONLY; MODIFY only for a proven row/consumer defect | retain as season-window authority |
| `Assets/Ashfall.Core/World/WeatherSystem.cs` | READ ONLY | weather/season owner |
| `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` | READ ONLY | campaign day/season projection |
| `Assets/Ashfall.Core/World/WeatherEffectsCatalog.cs` | READ ONLY | kind-effect authority |
| `src/Main.World.cs` | READ ONLY | host consequence routing |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| parallel season selection | keep WeatherSystem/CampaignCalendar authoritative |
| stale forecast claims | require current confidence/effects owner |
| season/calendar axis mismatch | use canonical campaign day |
| weight normalization drift | pin finite/range tests and replay |

# 22. Explicit Non-Goals

- no second weather RNG, no duplicate season catalog, no new weather kind, no change to Year-of-Ash 180–360 canon, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for weather-season calendar are named from current evidence.
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

- A current window-to-consumer/effect map.
- A precise statement of which specialized systems consume season ids.
- A bounded residual only for a proven live consumer or coverage gap.

## MUST NOT DO

- add a second weather/season system
- move weather effects into season rows
- change Year-of-Ash timing
- use UI refresh as a weather event

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/World/WeatherSeasonExpansionTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/Plan83_74WeatherNarrativeIntegrationTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/WeatherGateCatalogIntegrityTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/World/WeatherGateBalanceAuditTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read WeatherSystem’s season loader/selection, CampaignCalendar, WeatherIntelligenceCoordinator, effects/gates, and the focused tests; write the day-to-window-to-effect table.

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

### Authority lines 718–723
00718: **A-26 · C11 · Ledger-debt statement prose.** Subject: debtor statements and collection notices for `ledger_debt_templates.json` rows. Evidence: catalog verified live; `LedgerDebtSystem` with consequence dispatchers is canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00719:
00720: **A-27 · C12 · Storm-window almanac entries.** Subject: almanac prose conditioned on `year_of_ash_storm_windows.json` entries. Evidence: catalog verified live; `weather_almanac_expansion` exists in the corpus. Route: DATA-ONLY, within the Year-of-Ash window (180–360) canon. Confidence: HIGH CONFIDENCE.
00721:
00722: **A-28 · C13 · Under-served epilogue chronicle depth.** Subject: consumed by F-005 after the permutation audit selects the weakest cells. Evidence: matrix is canon (32 permutations). Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00723:

### Authority lines 764–769
00764: **B-18 · C11 · Trade-screen scenario expansion.** Subject: additional scenarios and tell lines for under-covered merchant identities. Evidence: `trade_screen_scenarios.json`, `trade_tell_lines.json`, `trade_specialties.json` verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00765:
00766: **B-19 · C12 · Winter pressure for power and water systems.** Subject: Year-of-Ash window (180–360) pressure extensions for systems that currently produce no winter-specific cost (filter burn, diesel reserve drawdown curves conditioned on storm windows). Evidence: storm windows canon; hardening upgrades catalog live. Route: CORE-EXTENSION + data. Confidence: PROPOSAL.
00767:
00768: **B-20 · C13 · Reckoning evidence enrollment sweep.** Subject: audit systems added since the 19-wave for evidence enrollment gaps; enroll through the existing Reckoning path only. Evidence: 19C closed (DR-06); subsequent waves exist (Waves 8–12 logs). Route: audit then CORE-EXTENSION. Confidence: HIGH CONFIDENCE.
00769:

### Authority lines 796–801
00796: **C-08 · C11 · Black-market price-tier audit.** Subject: black-market inventory pricing versus legal market bands and scarcity premiums. Evidence: `black_market_inventory.json` verified live. Route: harness. Confidence: PROPOSAL.
00797:
00798: **C-09 · C12 · Winter resource compression audit.** Subject: Days 90–180 and 180–360 calorie/fuel/filter/morale sustainability per difficulty preset (sustainability-day math). Evidence: baselines live (DR-03). Route: harness. Confidence: HIGH CONFIDENCE.
00799:
00800: **C-10 · C14 · Trapping yield versus degradation cost.** Subject: `wildlife_trapping_catalog.json` yields against equipment condition degradation; uncooked-yield zoonosis risk premium. Evidence: trapping and zoonosis bridge canon. Route: harness. Confidence: HIGH CONFIDENCE.
00801:

### Authority lines 844–849
00844: **E-08 · C11 · Caravan barter ledger legibility.** Subject: `CaravanBarterLedger` panel state completeness against the sealed merchant-restock display-order priority (DEC-05). Evidence: DEC-05 sealed (DR-06). Route: HOST-WIRING. Confidence: PROPOSAL.
00845:
00846: **E-09 · C12 · Storm-window forecast legibility.** Subject: weather forecast surface rendering storm-window warnings with adequate lead time for the 180–360 window. Evidence: forecast observation is a canon loop step. Route: HOST-WIRING. Confidence: PROPOSAL.
00847:
00848: **E-10 · C13 · Reckoning evidence submission surface.** Subject: submission UX for enrolled evidence classes that lack a clear submission affordance (audit first). Evidence: Reckoning consumes enrolled evidence by canon. Route: HOST-WIRING. Confidence: PROPOSAL.
00849:

### Authority lines 857–862
00857: **F-01 · C17 · Per-frame UI allocation audit.** Subject: measure per-frame allocations in the shell components (dashboard shell, metric cards, data grids) during a 15-FPS headless session; only optimize what the profiler demonstrates. Evidence: 15-FPS runtime test sessions are canon (v1.0 Part 14.1); `Performance/` Core and the CI performance gate exist. Route: measurement harness, then targeted repair with before/after numbers in `docs/perf/`. Confidence: potential hotspot — requires profiling.
00858:
00859: **F-02 · C12 · Storm-window tick concentration.** Subject: measure per-day tick cost spikes inside storm windows (Days 180–360) where weather, fallout, route gates, and morale effects co-fire. Evidence: window canon; co-firing systems canon. Route: profiler comparison across window/non-window days. Confidence: potential hotspot — requires profiling.
00860:
00861: **F-03 · C13 · Epilogue-matrix evaluation cost.** Subject: one-shot Day-360 evaluation cost across 32 permutations plus the Day-3650 pass; likely negligible, measure only if reported slow. Evidence: matrix canon. Route: one measurement, likely a no-change area. Confidence: HYPOTHESIS.
00862:

### Authority lines 881–886
00881: **G-06 · C8 · Exactly-once guard regression suite.** Subject: regression tests covering every sealed exactly-once guard class (ignore consequences, arrival resolution, salvage grants) against restore-mid-effect saves. Evidence: sealed runtime models the guards (DR-06). Route: focused xUnit + fixture saves. Confidence: HIGH CONFIDENCE.
00882:
00883: **G-07 · C12 · Two-pass determinism proof for any new winter simulation.** Subject: any Lane B/C12 plan shipping new simulation logic must ship the byte-identical two-pass proof (the Plan 76.2 pattern). Evidence: pattern canon. Route: mandatory plan component. Confidence: CANON process.
00884:
00885: **G-08 · C16 · XP W1 consumer-binding test wave.** Subject: for every consumer bound under B-23, a focused test that the scalar flows from the authority to the consumer and no parallel scalar exists. Evidence: W1 ACTIVE. Route: focused xUnit per consumer, post-binding. Confidence: HIGH CONFIDENCE, sequence-gated.
00886:

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

### Authority lines 953–958
00953: **DM-11 — Economy (C11).** Owners: market, price factors, shocks, baselines, regional prices, hardcore tuning, rumor bands, black market, caravans, debt ledger, foundry economy, bounty board, trade screens. Live catalogs: `commodity_baselines`, `regional_prices`, `hardcore_economy_tuning`, `economy_goods`, `black_market_inventory`, `ledger_debt_templates`, `trade_screen_scenarios`, `trade_tell_lines`, `trade_specialties`, `trade_texts`, `bounty_board`. Hosts: Economy, BlackMarket, TravelingCaravan, SilentFoundry. Docs: `ECONOMY_FAIRNESS_AUDIT.md`, `ECONOMY_PRICE_FACTOR_MATRIX.md` (verified live). Sealed: merchant restock priority (DEC-05). Openings: A-26, B-18, C-07, C-08, C-13, E-08, G-02. GATE: black-market funds legs.
00954:
00955: **DM-12 — Weather and Year of Ash (C12).** Owners: weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash family (events/items/locations/questlines/quests/radio/survivors/storm windows). Live catalogs: all of the above verified live. Hosts: WeatherHardening, YearOfAsh widgets, WeatherStationSystem. Openings: A-27, B-03, B-19, C-09, E-09, F-02, G-07, plus the F-002 campaign. Constraint: tick window 180–360 canon.
00956:
00957: **DM-13 — Endgame and epilogue (C13).** Owners: Reckoning, verdict ending evaluator, epilogue matrix runtime, epilogue chronicle, standing records, census, muster epilogues, holdfast endings. Live catalogs: `endings`, `campaign_epilogues`, `epilogue_chronicle`, `verdict_data/items/locations/npcs/questlines/radio`, `standing_record_factions/layouts/memory/quests`, `muster_epilogues`. Hosts: Endgame, Verdict, StandingRecord. Openings: A-28, B-20, D-03, E-10, F-03, G-04, plus the F-005 campaign. Constraint: main ending cannot be invalidated by optional content.
00958:

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/weather_seasons.json`
- Bytes: 3,212; SHA-256: `a9032e68bc0d46db2ec732d5d79d655b2b96f228b85d87ff50781e0e8449f268`
- Root keys: `displayName, id, schema_version, seasons, weatherCheckIntervalHours`
- `seasons`: list[10]; union fields: `ashfallWeight, blackRainWeight, blizzardWeight, clearWeight, displayName, falloutStormWeight, id, overcastWeight, rainWeight, startDay`
  - row 1: `{"ashfallWeight":1.4,"blackRainWeight":0.3,"blizzardWeight":0.7,"clearWeight":2.6,"displayName":"First Thaw","falloutStormWeight":0.6,"id":"window_first_thaw","overcastWeight":2.4,"rainWeight":3.0,"startDay":0}`
  - row 2: `{"ashfallWeight":2.0,"blackRainWeight":0.1,"blizzardWeight":0.2,"clearWeight":0.8,"displayName":"Ash Settling","falloutStormWeight":0.3,"id":"window_ash_settling","overcastWeight":1.6,"rainWeight":0.7,"startDay":30}`
  - row 3: `{"ashfallWeight":0.8,"blackRainWeight":0.2,"blizzardWeight":2.5,"clearWeight":0.6,"displayName":"The Deep Freeze","falloutStormWeight":0.4,"id":"window_deep_freeze","overcastWeight":1.2,"rainWeight":0.2,"startDay":60}`
  - row 4: `{"ashfallWeight":1.0,"blackRainWeight":0.1,"blizzardWeight":0.3,"clearWeight":0.7,"displayName":"Spring Storms","falloutStormWeight":0.5,"id":"window_spring_storms","overcastWeight":2.0,"rainWeight":2.2,"startDay":90}`
  - row 5: `{"ashfallWeight":2.6,"blackRainWeight":0.2,"blizzardWeight":0.2,"clearWeight":0.8,"displayName":"Dry Ash","falloutStormWeight":0.8,"id":"window_dry_ash","overcastWeight":1.2,"rainWeight":0.3,"startDay":120}`
  - row 6: `{"ashfallWeight":2.0,"blackRainWeight":0.8,"blizzardWeight":0.4,"clearWeight":0.4,"displayName":"First Fallout","falloutStormWeight":2.7,"id":"window_first_fallout","overcastWeight":1.0,"rainWeight":0.4,"startDay":150}`
  - row 7: `{"ashfallWeight":0.8,"blackRainWeight":0.2,"blizzardWeight":0.2,"clearWeight":2.4,"displayName":"False Spring","falloutStormWeight":0.4,"id":"window_false_spring","overcastWeight":1.8,"rainWeight":1.0,"startDay":180}`
  - row 8: `{"ashfallWeight":2.8,"blackRainWeight":0.8,"blizzardWeight":1.8,"clearWeight":0.3,"displayName":"Deep Ash","falloutStormWeight":1.6,"id":"window_deep_ash","overcastWeight":0.8,"rainWeight":0.2,"startDay":200}`
  - row 9: `{"ashfallWeight":1.6,"blackRainWeight":0.6,"blizzardWeight":2.4,"clearWeight":0.3,"displayName":"The Long Winter","falloutStormWeight":1.0,"id":"window_long_winter","overcastWeight":0.9,"rainWeight":0.2,"startDay":240}`
  - row 10: `{"ashfallWeight":1.5,"blackRainWeight":3.0,"blizzardWeight":1.8,"clearWeight":0.2,"displayName":"Black Rain Season","falloutStormWeight":2.1,"id":"window_black_rain_season","overcastWeight":0.7,"rainWeight":0.3,"startDay":280}`
- Bytes: 3,212; SHA-256: `a9032e68bc0d46db2ec732d5d79d655b2b96f228b85d87ff50781e0e8449f268`
- Root keys: `displayName, id, schema_version, seasons, weatherCheckIntervalHours`
- `seasons`: list[10]; union fields: `ashfallWeight, blackRainWeight, blizzardWeight, clearWeight, displayName, falloutStormWeight, id, overcastWeight, rainWeight, startDay`
  - row 1: `{"ashfallWeight":1.4,"blackRainWeight":0.3,"blizzardWeight":0.7,"clearWeight":2.6,"displayName":"First Thaw","falloutStormWeight":0.6,"id":"window_first_thaw","overcastWeight":2.4,"rainWeight":3.0,"startDay":0}`
  - row 2: `{"ashfallWeight":2.0,"blackRainWeight":0.1,"blizzardWeight":0.2,"clearWeight":0.8,"displayName":"Ash Settling","falloutStormWeight":0.3,"id":"window_ash_settling","overcastWeight":1.6,"rainWeight":0.7,"startDay":30}`
  - row 3: `{"ashfallWeight":0.8,"blackRainWeight":0.2,"blizzardWeight":2.5,"clearWeight":0.6,"displayName":"The Deep Freeze","falloutStormWeight":0.4,"id":"window_deep_freeze","overcastWeight":1.2,"rainWeight":0.2,"startDay":60}`
  - row 4: `{"ashfallWeight":1.0,"blackRainWeight":0.1,"blizzardWeight":0.3,"clearWeight":0.7,"displayName":"Spring Storms","falloutStormWeight":0.5,"id":"window_spring_storms","overcastWeight":2.0,"rainWeight":2.2,"startDay":90}`
  - row 5: `{"ashfallWeight":2.6,"blackRainWeight":0.2,"blizzardWeight":0.2,"clearWeight":0.8,"displayName":"Dry Ash","falloutStormWeight":0.8,"id":"window_dry_ash","overcastWeight":1.2,"rainWeight":0.3,"startDay":120}`
  - row 6: `{"ashfallWeight":2.0,"blackRainWeight":0.8,"blizzardWeight":0.4,"clearWeight":0.4,"displayName":"First Fallout","falloutStormWeight":2.7,"id":"window_first_fallout","overcastWeight":1.0,"rainWeight":0.4,"startDay":150}`
  - row 7: `{"ashfallWeight":0.8,"blackRainWeight":0.2,"blizzardWeight":0.2,"clearWeight":2.4,"displayName":"False Spring","falloutStormWeight":0.4,"id":"window_false_spring","overcastWeight":1.8,"rainWeight":1.0,"startDay":180}`
  - row 8: `{"ashfallWeight":2.8,"blackRainWeight":0.8,"blizzardWeight":1.8,"clearWeight":0.3,"displayName":"Deep Ash","falloutStormWeight":1.6,"id":"window_deep_ash","overcastWeight":0.8,"rainWeight":0.2,"startDay":200}`
  - row 9: `{"ashfallWeight":1.6,"blackRainWeight":0.6,"blizzardWeight":2.4,"clearWeight":0.3,"displayName":"The Long Winter","falloutStormWeight":1.0,"id":"window_long_winter","overcastWeight":0.9,"rainWeight":0.2,"startDay":240}`
  - row 10: `{"ashfallWeight":1.5,"blackRainWeight":3.0,"blizzardWeight":1.8,"clearWeight":0.2,"displayName":"Black Rain Season","falloutStormWeight":2.1,"id":"window_black_rain_season","overcastWeight":0.7,"rainWeight":0.3,"startDay":280}`

## `Assets/StreamingAssets/Data/weather_effects.json`
- Bytes: 7,615; SHA-256: `2879d31c294f9f84f274e14b5b266d02d73b4f7bb1abe0a1d8604023af846600`
- Root keys: `schema_version, weather_effects`
- `weather_effects`: list[22]; union fields: `caravan_availability_multiplier, explicitly_neutral, outdoor_rad_modifier, thermal_load_additive_c, trap_yield_multiplier, travel_encounter_multiplier, travel_speed_multiplier, visibility_modifier, weather`
  - row 1: `{"caravan_availability_multiplier":1.0,"explicitly_neutral":true,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":1.0,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":1.0,"weather":"Clear"}`
  - row 2: `{"caravan_availability_multiplier":0.95,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.7,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":0.95,"visibility_modifier":0.85,"weather":"Rain"}`
  - row 3: `{"caravan_availability_multiplier":1.0,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":1.0,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":0.95,"weather":"Overcast"}`
  - row 4: `{"caravan_availability_multiplier":0.9,"explicitly_neutral":false,"outdoor_rad_modifier":45,"thermal_load_additive_c":0,"trap_yield_multiplier":0.6,"travel_encounter_multiplier":1.05,"travel_speed_multiplier":0.9,"visibility_modifier":0.65,"weather":"Ashfall"}`
  - row 5: `{"caravan_availability_multiplier":0.5,"explicitly_neutral":false,"outdoor_rad_modifier":150,"thermal_load_additive_c":-5,"trap_yield_multiplier":0.5,"travel_encounter_multiplier":1.25,"travel_speed_multiplier":0.7,"visibility_modifier":0.0,"weather":"FalloutStorm"}`
  - row 6: `{"caravan_availability_multiplier":0.25,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":-15,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.15,"travel_speed_multiplier":0.6,"visibility_modifier":0.4,"weather":"Blizzard"}`
  - row 7: `{"caravan_availability_multiplier":0.25,"explicitly_neutral":false,"outdoor_rad_modifier":250,"thermal_load_additive_c":-8,"trap_yield_multiplier":0.5,"travel_encounter_multiplier":1.3,"travel_speed_multiplier":0.7,"visibility_modifier":0.0,"weather":"BlackRain"}`
  - row 8: `{"caravan_availability_multiplier":0.5,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":-4,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.05,"travel_speed_multiplier":0.85,"visibility_modifier":0.7,"weather":"AcidSnow"}`
  - row 9: `{"caravan_availability_multiplier":0.75,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.6,"travel_encounter_multiplier":1.1,"travel_speed_multiplier":0.85,"visibility_modifier":0.5,"weather":"BioFog"}`
  - row 10: `{"caravan_availability_multiplier":0.5,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":-6,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.05,"travel_speed_multiplier":0.8,"visibility_modifier":0.6,"weather":"BlackSnow"}`
  - row 11: `{"caravan_availability_multiplier":0.5,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.5,"travel_encounter_multiplier":1.2,"travel_speed_multiplier":0.85,"visibility_modifier":0.6,"weather":"BloodRain"}`
  - row 12: `{"caravan_availability_multiplier":0.8,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.5,"travel_encounter_multiplier":1.1,"travel_speed_multiplier":0.9,"visibility_modifier":0.8,"weather":"EMPStorm"}`
  - row 13: `{"caravan_availability_multiplier":0.25,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.3,"travel_speed_multiplier":0.5,"visibility_modifier":0.3,"weather":"GlassStorm"}`
  - row 14: `{"caravan_availability_multiplier":0.4,"explicitly_neutral":false,"outdoor_rad_modifier":60,"thermal_load_additive_c":0,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.2,"travel_speed_multiplier":0.7,"visibility_modifier":0.5,"weather":"RadHail"}`
  - row 15: `{"caravan_availability_multiplier":0.9,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.7,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":0.9,"weather":"AlgaeBloom"}`
  - row 16: `{"caravan_availability_multiplier":0.8,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.5,"travel_encounter_multiplier":1.2,"travel_speed_multiplier":0.85,"visibility_modifier":0.6,"weather":"AshLightning"}`
  - row 17: `{"caravan_availability_multiplier":0.85,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.6,"travel_encounter_multiplier":1.05,"travel_speed_multiplier":0.8,"visibility_modifier":0.45,"weather":"ParticulateFog"}`
  - row 18: `{"caravan_availability_multiplier":1.0,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":2,"trap_yield_multiplier":0.6,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":0.9,"weather":"ThermalInversion"}`
  - row 19: `{"caravan_availability_multiplier":0.3,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":-10,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.15,"travel_speed_multiplier":0.55,"visibility_modifier":0.5,"weather":"IceStorm"}`
  - row 20: `{"caravan_availability_multiplier":1.0,"explicitly_neutral":true,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":1.0,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":1.0,"weather":"Silence"}`
  - row 21: `{"caravan_availability_multiplier":1.05,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":5,"trap_yield_multiplier":1.0,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":1.0,"weather":"FalseSpring"}`
  - row 22: `{"caravan_availability_multiplier":1.0,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":1.0,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":0.95,"weather":"SilentSpring"}`
- Bytes: 7,615; SHA-256: `2879d31c294f9f84f274e14b5b266d02d73b4f7bb1abe0a1d8604023af846600`
- Root keys: `schema_version, weather_effects`
- `weather_effects`: list[22]; union fields: `caravan_availability_multiplier, explicitly_neutral, outdoor_rad_modifier, thermal_load_additive_c, trap_yield_multiplier, travel_encounter_multiplier, travel_speed_multiplier, visibility_modifier, weather`
  - row 1: `{"caravan_availability_multiplier":1.0,"explicitly_neutral":true,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":1.0,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":1.0,"weather":"Clear"}`
  - row 2: `{"caravan_availability_multiplier":0.95,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.7,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":0.95,"visibility_modifier":0.85,"weather":"Rain"}`
  - row 3: `{"caravan_availability_multiplier":1.0,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":1.0,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":0.95,"weather":"Overcast"}`
  - row 4: `{"caravan_availability_multiplier":0.9,"explicitly_neutral":false,"outdoor_rad_modifier":45,"thermal_load_additive_c":0,"trap_yield_multiplier":0.6,"travel_encounter_multiplier":1.05,"travel_speed_multiplier":0.9,"visibility_modifier":0.65,"weather":"Ashfall"}`
  - row 5: `{"caravan_availability_multiplier":0.5,"explicitly_neutral":false,"outdoor_rad_modifier":150,"thermal_load_additive_c":-5,"trap_yield_multiplier":0.5,"travel_encounter_multiplier":1.25,"travel_speed_multiplier":0.7,"visibility_modifier":0.0,"weather":"FalloutStorm"}`
  - row 6: `{"caravan_availability_multiplier":0.25,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":-15,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.15,"travel_speed_multiplier":0.6,"visibility_modifier":0.4,"weather":"Blizzard"}`
  - row 7: `{"caravan_availability_multiplier":0.25,"explicitly_neutral":false,"outdoor_rad_modifier":250,"thermal_load_additive_c":-8,"trap_yield_multiplier":0.5,"travel_encounter_multiplier":1.3,"travel_speed_multiplier":0.7,"visibility_modifier":0.0,"weather":"BlackRain"}`
  - row 8: `{"caravan_availability_multiplier":0.5,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":-4,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.05,"travel_speed_multiplier":0.85,"visibility_modifier":0.7,"weather":"AcidSnow"}`
  - row 9: `{"caravan_availability_multiplier":0.75,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.6,"travel_encounter_multiplier":1.1,"travel_speed_multiplier":0.85,"visibility_modifier":0.5,"weather":"BioFog"}`
  - row 10: `{"caravan_availability_multiplier":0.5,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":-6,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.05,"travel_speed_multiplier":0.8,"visibility_modifier":0.6,"weather":"BlackSnow"}`
  - row 11: `{"caravan_availability_multiplier":0.5,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.5,"travel_encounter_multiplier":1.2,"travel_speed_multiplier":0.85,"visibility_modifier":0.6,"weather":"BloodRain"}`
  - row 12: `{"caravan_availability_multiplier":0.8,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.5,"travel_encounter_multiplier":1.1,"travel_speed_multiplier":0.9,"visibility_modifier":0.8,"weather":"EMPStorm"}`
  - row 13: `{"caravan_availability_multiplier":0.25,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.3,"travel_speed_multiplier":0.5,"visibility_modifier":0.3,"weather":"GlassStorm"}`
  - row 14: `{"caravan_availability_multiplier":0.4,"explicitly_neutral":false,"outdoor_rad_modifier":60,"thermal_load_additive_c":0,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.2,"travel_speed_multiplier":0.7,"visibility_modifier":0.5,"weather":"RadHail"}`
  - row 15: `{"caravan_availability_multiplier":0.9,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.7,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":0.9,"weather":"AlgaeBloom"}`
  - row 16: `{"caravan_availability_multiplier":0.8,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.5,"travel_encounter_multiplier":1.2,"travel_speed_multiplier":0.85,"visibility_modifier":0.6,"weather":"AshLightning"}`
  - row 17: `{"caravan_availability_multiplier":0.85,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":0.6,"travel_encounter_multiplier":1.05,"travel_speed_multiplier":0.8,"visibility_modifier":0.45,"weather":"ParticulateFog"}`
  - row 18: `{"caravan_availability_multiplier":1.0,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":2,"trap_yield_multiplier":0.6,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":0.9,"weather":"ThermalInversion"}`
  - row 19: `{"caravan_availability_multiplier":0.3,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":-10,"trap_yield_multiplier":0.2,"travel_encounter_multiplier":1.15,"travel_speed_multiplier":0.55,"visibility_modifier":0.5,"weather":"IceStorm"}`
  - row 20: `{"caravan_availability_multiplier":1.0,"explicitly_neutral":true,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":1.0,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":1.0,"weather":"Silence"}`
  - row 21: `{"caravan_availability_multiplier":1.05,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":5,"trap_yield_multiplier":1.0,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":1.0,"weather":"FalseSpring"}`
  - row 22: `{"caravan_availability_multiplier":1.0,"explicitly_neutral":false,"outdoor_rad_modifier":0,"thermal_load_additive_c":0,"trap_yield_multiplier":1.0,"travel_encounter_multiplier":1.0,"travel_speed_multiplier":1.0,"visibility_modifier":0.95,"weather":"SilentSpring"}`

## `Assets/StreamingAssets/Data/weather_route_gates.json`
- Bytes: 10,986; SHA-256: `1957b31fbb87f1ad5a86f83ae9a38c01cd8f80701a4a04073e691b327a454f10`
- Root keys: `gates, schema_version`
- `gates`: list[18]; union fields: `blocked_weather, consequence_on_force, description, force_rad_dose, force_stamina_cost, gate_type, id, override_item, override_skill, required_weather, target`
  - row 1: `{"blocked_weather":["Blizzard"],"consequence_on_force":"Severe cold exposure and disorientation. Whiteout conditions reduce visibility to zero.","description":"The Mount Karkov ascent becomes a death trap during blizzards. Crosswinds exceed 80 km/h on the exposed ridge, and the trail markers vanish under drifting snow within minutes.","gate_type":"route","id":"gate_mountain_pass_blizzard","override_item":"","override_skill":"","required_weather":[],"target":"route_12_the_cloud_eyrie_meteorological_ascent"}`
  - row 2: `{"blocked_weather":["Blizzard"],"consequence_on_force":"Drifting snow buries the road surface. Vehicles stall; sleds overturn.","description":"The anthracite convoy route crosses an exposed slag plateau where blizzard winds pile snow into axle-deep drifts. The heavy trucks cannot maintain traction, and there is no shelter for twelve kilometres.","gate_type":"route","id":"gate_highland_supply_blizzard","override_item":"","override_skill":"","required_weather":[],"target":"route_05_the_spoil_heap_coke_and_steel_route"}`
  - row 3: `{"blocked_weather":["Blizzard"],"consequence_on_force":"Whiteout exposure. Wind chill drops effective temperature below -40C.","description":"The Volga-1 steppe drift route follows an exposed ridgeline with no cover. During blizzards the wind chill becomes lethal within minutes, and the trail disappears into a wall of white.","gate_type":"route","id":"gate_exposed_ridge_blizzard","override_item":"","override_skill":"","required_weather":[],"target":"route_09_the_nomad_ridge_drift"}`
  - row 4: `{"blocked_weather":["Blizzard"],"consequence_on_force":"Canal ice becomes unstable under wind loading. Barge moorings fail.","description":"The canal waterway follows the frozen shoreline where blizzard winds pile ice against the embankment. Barge moorings snap under the loading, and the ice surface becomes a wind-scoured skating rink.","gate_type":"route","id":"gate_lake_edge_blizzard","override_item":"","override_skill":"","required_weather":[],"target":"route_02_the_canal_barge_waterway_run"}`
  - row 5: `{"blocked_weather":["BioFog"],"consequence_on_force":"Respiratory contamination. Bioaerosol exposure through unprotected airways.","description":"The peat bog salvage route crosses a lowland marsh where biofog settles thick and low. The contaminated aerosol causes severe respiratory distress within minutes of unprotected exposure.","gate_type":"route","id":"gate_lowland_marsh_fog","override_item":"gas_mask","override_skill":"","required_weather":[],"target":"route_07_the_aluminium_whale_salvage_run"}`
  - row 6: `{"blocked_weather":["BioFog"],"consequence_on_force":"Chemical and biological contamination. Skin exposure to aerosolized industrial residue.","description":"The bitumen highway passes through an industrial valley where biofog traps vaporized tar and chemical residue at ground level. Without respiratory protection, the air itself becomes corrosive.","gate_type":"route","id":"gate_industrial_valley_fog","override_item":"gas_mask","override_skill":"","required_weather":[],"target":"route_15_the_tar_cauldron_pitch_express"}`
  - row 7: `{"blocked_weather":["BioFog"],"consequence_on_force":"Zero visibility in contaminated air. Navigation failure likely.","description":"The grain haul route follows a river basin where biofog pools so thick that landmarks vanish at arm's length. The contaminated air makes navigation impossible, and the riverbank drops away without warning.","gate_type":"route","id":"gate_river_basin_fog","override_item":"","override_skill":"","required_weather":[],"target":"route_03_the_black_loess_grain_haul"}`
  - row 8: `{"blocked_weather":["BlackRain"],"consequence_on_force":"Flash flooding. Radioactive sediment fills the underpass within minutes.","description":"The metro underpass drains badly even in ordinary rain. During black rain, oily radioactive runoff fills the tunnel to the ceiling within minutes, carrying sediment and debris that blocks all passage.","gate_type":"route","id":"gate_underpass_black_rain","override_item":"","override_skill":"","required_weather":[],"target":"route_13_the_subway_tile_vault_underpass"}`
  - row 9: `{"blocked_weather":["BlackRain"],"consequence_on_force":"Riverbank collapse. Contaminated floodwater submerges the path.","description":"The pilgrimage route follows a riverbank that becomes unstable during black rain. The swollen river carries radioactive sediment over the path, and the undercut bank collapses without warning.","gate_type":"route","id":"gate_riverside_black_rain","override_item":"","override_skill":"","required_weather":[],"target":"route_04_the_holy_spring_mercy_trek"}`
  - row 10: `{"blocked_weather":["BlackRain"],"consequence_on_force":"Flash flood in drainage channel. Radioactive debris flow.","description":"The telegraph line route crosses a drainage culvert that becomes a flash-flood channel during black rain. The water carries radioactive ash and debris at speed, making the crossing impassable.","gate_type":"route","id":"gate_culvert_black_rain","override_item":"","override_skill":"","required_weather":[],"target":"route_17_the_telegraph_wire_runner_post"}`
  - row 11: `{"blocked_weather":["FalloutStorm"],"consequence_on_force":"Extreme radiation exposure. No cover exists along the route.","description":"The crane graveyard route crosses open wasteland with no shelter for two kilometres. During fallout storms, radiation levels spike to lethal doses within minutes, and there is nowhere to take cover.","gate_type":"route","id":"gate_open_wasteland_fallout","override_item":"hazmat_suit","override_skill":"","required_weather":[],"target":"route_16_the_radioactive_graveyard_salvage_crawl"}`
  - row 12: `{"blocked_weather":["FalloutStorm"],"consequence_on_force":"Severe radiation exposure along exposed rail line.","description":"The Iron Line Express follows an elevated rail embankment with no overhead cover. Fallout storms deposit radioactive material directly on the route, and the wind carries it into every gap in protection.","gate_type":"route","id":"gate_exposed_highway_fallout","override_item":"hazmat_suit","override_skill":"","required_weather":[],"target":"route_01_the_iron_line_garrison_express"}`
  - row 13: `{"blocked_weather":[],"consequence_on_force":"","description":"The salt pass crosses a frozen lake that only becomes safe to traverse during sustained deep cold. The blizzard-season ice thickens enough to carry loaded sleds, opening a shortcut that saves a full day of travel around the shoreline.","gate_type":"route","id":"gate_frozen_lake_crossing","override_item":"","override_skill":"","required_weather":["Blizzard"],"target":"route_06_the_thermal_brine_salt_pass"}`
  - row 14: `{"blocked_weather":[],"consequence_on_force":"","description":"The high-voltage relay route follows a seasonal ice road across a frozen marsh. The ice only becomes thick enough for loaded vehicles during the deep winter blizzard season, opening a direct route that is impassable in warmer conditions.","gate_type":"route","id":"gate_seasonal_ice_road","override_item":"","override_skill":"","required_weather":["Blizzard"],"target":"route_08_the_high_voltage_grid_battery_relay"}`
  - row 15: `{"blocked_weather":["EMPStorm"],"consequence_on_force":"Vehicle electronics fail. Navigation and communication systems go dark.","description":"The granite haul route depends on powered winches and electronic navigation through the quarry pit. During EMP storms, the vehicle electronics fail and the winch systems go dead, leaving the convoy stranded in the pit.","gate_type":"route","id":"gate_electronics_route_emp","override_item":"","override_skill":"","required_weather":[],"target":"route_11_the_quarry_pit_granite_cartage"}`
  - row 16: `{"blocked_weather":["Blizzard","IceStorm"],"consequence_on_force":"Whiteout above the treeline. The switchbacks vanish, and the wind does the rest.","description":"The mountain road to the observatory closes in any blizzard or ice storm. At sixty below, the wind strips heat faster than a climber can replace it.","force_stamina_cost":30,"gate_type":"destination","id":"gate_dest_silent_observatory_blizzard","override_item":"","override_skill":"","required_weather":[],"target":"location_silent_observatory"}`
  - row 17: `{"blocked_weather":["BlackRain"],"consequence_on_force":"Black rain percolates straight into the flooded bay. Waders do not stop what dissolves.","description":"The depot floods from below and above. Black rain raises the waterline and feeds the radiation already dissolved in it.","force_rad_dose":15,"force_stamina_cost":25,"gate_type":"destination","id":"gate_dest_flooded_subway_depot_blackrain","override_item":"","override_skill":"","required_weather":[],"target":"location_flooded_subway_depot"}`
  - row 18: `{"blocked_weather":["FalloutStorm"],"consequence_on_force":"An open-air market under a fallout storm is a ledger of the dead. The stalls would stand empty anyway.","description":"The Shallows trades in the open air on the drowned waterfront. When fallout comes off the water, there is nowhere to shelter and no trade worth the dose.","force_rad_dose":20,"force_stamina_cost":25,"gate_type":"destination","id":"gate_dest_shallows_market_falloutstorm","override_item":"","override_skill":"","required_weather":[],"target":"loc_the_shallows_market"}`
- Bytes: 10,986; SHA-256: `1957b31fbb87f1ad5a86f83ae9a38c01cd8f80701a4a04073e691b327a454f10`
- Root keys: `gates, schema_version`
- `gates`: list[18]; union fields: `blocked_weather, consequence_on_force, description, force_rad_dose, force_stamina_cost, gate_type, id, override_item, override_skill, required_weather, target`
  - row 1: `{"blocked_weather":["Blizzard"],"consequence_on_force":"Severe cold exposure and disorientation. Whiteout conditions reduce visibility to zero.","description":"The Mount Karkov ascent becomes a death trap during blizzards. Crosswinds exceed 80 km/h on the exposed ridge, and the trail markers vanish under drifting snow within minutes.","gate_type":"route","id":"gate_mountain_pass_blizzard","override_item":"","override_skill":"","required_weather":[],"target":"route_12_the_cloud_eyrie_meteorological_ascent"}`
  - row 2: `{"blocked_weather":["Blizzard"],"consequence_on_force":"Drifting snow buries the road surface. Vehicles stall; sleds overturn.","description":"The anthracite convoy route crosses an exposed slag plateau where blizzard winds pile snow into axle-deep drifts. The heavy trucks cannot maintain traction, and there is no shelter for twelve kilometres.","gate_type":"route","id":"gate_highland_supply_blizzard","override_item":"","override_skill":"","required_weather":[],"target":"route_05_the_spoil_heap_coke_and_steel_route"}`
  - row 3: `{"blocked_weather":["Blizzard"],"consequence_on_force":"Whiteout exposure. Wind chill drops effective temperature below -40C.","description":"The Volga-1 steppe drift route follows an exposed ridgeline with no cover. During blizzards the wind chill becomes lethal within minutes, and the trail disappears into a wall of white.","gate_type":"route","id":"gate_exposed_ridge_blizzard","override_item":"","override_skill":"","required_weather":[],"target":"route_09_the_nomad_ridge_drift"}`
  - row 4: `{"blocked_weather":["Blizzard"],"consequence_on_force":"Canal ice becomes unstable under wind loading. Barge moorings fail.","description":"The canal waterway follows the frozen shoreline where blizzard winds pile ice against the embankment. Barge moorings snap under the loading, and the ice surface becomes a wind-scoured skating rink.","gate_type":"route","id":"gate_lake_edge_blizzard","override_item":"","override_skill":"","required_weather":[],"target":"route_02_the_canal_barge_waterway_run"}`
  - row 5: `{"blocked_weather":["BioFog"],"consequence_on_force":"Respiratory contamination. Bioaerosol exposure through unprotected airways.","description":"The peat bog salvage route crosses a lowland marsh where biofog settles thick and low. The contaminated aerosol causes severe respiratory distress within minutes of unprotected exposure.","gate_type":"route","id":"gate_lowland_marsh_fog","override_item":"gas_mask","override_skill":"","required_weather":[],"target":"route_07_the_aluminium_whale_salvage_run"}`
  - row 6: `{"blocked_weather":["BioFog"],"consequence_on_force":"Chemical and biological contamination. Skin exposure to aerosolized industrial residue.","description":"The bitumen highway passes through an industrial valley where biofog traps vaporized tar and chemical residue at ground level. Without respiratory protection, the air itself becomes corrosive.","gate_type":"route","id":"gate_industrial_valley_fog","override_item":"gas_mask","override_skill":"","required_weather":[],"target":"route_15_the_tar_cauldron_pitch_express"}`
  - row 7: `{"blocked_weather":["BioFog"],"consequence_on_force":"Zero visibility in contaminated air. Navigation failure likely.","description":"The grain haul route follows a river basin where biofog pools so thick that landmarks vanish at arm's length. The contaminated air makes navigation impossible, and the riverbank drops away without warning.","gate_type":"route","id":"gate_river_basin_fog","override_item":"","override_skill":"","required_weather":[],"target":"route_03_the_black_loess_grain_haul"}`
  - row 8: `{"blocked_weather":["BlackRain"],"consequence_on_force":"Flash flooding. Radioactive sediment fills the underpass within minutes.","description":"The metro underpass drains badly even in ordinary rain. During black rain, oily radioactive runoff fills the tunnel to the ceiling within minutes, carrying sediment and debris that blocks all passage.","gate_type":"route","id":"gate_underpass_black_rain","override_item":"","override_skill":"","required_weather":[],"target":"route_13_the_subway_tile_vault_underpass"}`
  - row 9: `{"blocked_weather":["BlackRain"],"consequence_on_force":"Riverbank collapse. Contaminated floodwater submerges the path.","description":"The pilgrimage route follows a riverbank that becomes unstable during black rain. The swollen river carries radioactive sediment over the path, and the undercut bank collapses without warning.","gate_type":"route","id":"gate_riverside_black_rain","override_item":"","override_skill":"","required_weather":[],"target":"route_04_the_holy_spring_mercy_trek"}`
  - row 10: `{"blocked_weather":["BlackRain"],"consequence_on_force":"Flash flood in drainage channel. Radioactive debris flow.","description":"The telegraph line route crosses a drainage culvert that becomes a flash-flood channel during black rain. The water carries radioactive ash and debris at speed, making the crossing impassable.","gate_type":"route","id":"gate_culvert_black_rain","override_item":"","override_skill":"","required_weather":[],"target":"route_17_the_telegraph_wire_runner_post"}`
  - row 11: `{"blocked_weather":["FalloutStorm"],"consequence_on_force":"Extreme radiation exposure. No cover exists along the route.","description":"The crane graveyard route crosses open wasteland with no shelter for two kilometres. During fallout storms, radiation levels spike to lethal doses within minutes, and there is nowhere to take cover.","gate_type":"route","id":"gate_open_wasteland_fallout","override_item":"hazmat_suit","override_skill":"","required_weather":[],"target":"route_16_the_radioactive_graveyard_salvage_crawl"}`
  - row 12: `{"blocked_weather":["FalloutStorm"],"consequence_on_force":"Severe radiation exposure along exposed rail line.","description":"The Iron Line Express follows an elevated rail embankment with no overhead cover. Fallout storms deposit radioactive material directly on the route, and the wind carries it into every gap in protection.","gate_type":"route","id":"gate_exposed_highway_fallout","override_item":"hazmat_suit","override_skill":"","required_weather":[],"target":"route_01_the_iron_line_garrison_express"}`
  - row 13: `{"blocked_weather":[],"consequence_on_force":"","description":"The salt pass crosses a frozen lake that only becomes safe to traverse during sustained deep cold. The blizzard-season ice thickens enough to carry loaded sleds, opening a shortcut that saves a full day of travel around the shoreline.","gate_type":"route","id":"gate_frozen_lake_crossing","override_item":"","override_skill":"","required_weather":["Blizzard"],"target":"route_06_the_thermal_brine_salt_pass"}`
  - row 14: `{"blocked_weather":[],"consequence_on_force":"","description":"The high-voltage relay route follows a seasonal ice road across a frozen marsh. The ice only becomes thick enough for loaded vehicles during the deep winter blizzard season, opening a direct route that is impassable in warmer conditions.","gate_type":"route","id":"gate_seasonal_ice_road","override_item":"","override_skill":"","required_weather":["Blizzard"],"target":"route_08_the_high_voltage_grid_battery_relay"}`
  - row 15: `{"blocked_weather":["EMPStorm"],"consequence_on_force":"Vehicle electronics fail. Navigation and communication systems go dark.","description":"The granite haul route depends on powered winches and electronic navigation through the quarry pit. During EMP storms, the vehicle electronics fail and the winch systems go dead, leaving the convoy stranded in the pit.","gate_type":"route","id":"gate_electronics_route_emp","override_item":"","override_skill":"","required_weather":[],"target":"route_11_the_quarry_pit_granite_cartage"}`
  - row 16: `{"blocked_weather":["Blizzard","IceStorm"],"consequence_on_force":"Whiteout above the treeline. The switchbacks vanish, and the wind does the rest.","description":"The mountain road to the observatory closes in any blizzard or ice storm. At sixty below, the wind strips heat faster than a climber can replace it.","force_stamina_cost":30,"gate_type":"destination","id":"gate_dest_silent_observatory_blizzard","override_item":"","override_skill":"","required_weather":[],"target":"location_silent_observatory"}`
  - row 17: `{"blocked_weather":["BlackRain"],"consequence_on_force":"Black rain percolates straight into the flooded bay. Waders do not stop what dissolves.","description":"The depot floods from below and above. Black rain raises the waterline and feeds the radiation already dissolved in it.","force_rad_dose":15,"force_stamina_cost":25,"gate_type":"destination","id":"gate_dest_flooded_subway_depot_blackrain","override_item":"","override_skill":"","required_weather":[],"target":"location_flooded_subway_depot"}`
  - row 18: `{"blocked_weather":["FalloutStorm"],"consequence_on_force":"An open-air market under a fallout storm is a ledger of the dead. The stalls would stand empty anyway.","description":"The Shallows trades in the open air on the drowned waterfront. When fallout comes off the water, there is nowhere to shelter and no trade worth the dose.","force_rad_dose":20,"force_stamina_cost":25,"gate_type":"destination","id":"gate_dest_shallows_market_falloutstorm","override_item":"","override_skill":"","required_weather":[],"target":"loc_the_shallows_market"}`

## `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json`
- Bytes: 23,204; SHA-256: `ad76e65163efc3860f4ef7d6b209479f27489c0f5cb34a717fe4e64bd5d64d00`
- Root keys: `baits, prey, schema_version, traps`
- `traps`: list[10]; union fields: `baseCatchModifier, bycatchChance, bycatchSpecies, checkIntervalDays, compatiblePrey, description, displayName, durabilityChecks, narrativeIncidentChance, narrativeIncidentIds, requiresWater, setupCosts, trapEncounterChance, trapType, trap_id, weatherSensitivity`
  - row 1: `{"baseCatchModifier":1.0,"checkIntervalDays":2,"compatiblePrey":["rabbit","cotton_hare","fox","rat"],"description":"A loop of cord or wire set across a game trail, tightened by a springy branch. Cheap enough to deploy more than one, unreliable enough that materials still matter. The wire remembers the shape of the animal it was meant to hold.","displayName":"Wire Snare","durabilityChecks":8,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":1,"itemId":"rope"}],"trapEncounterChance":0.05,"trapType":"snare","trap_id":"trap_sn…`
  - row 2: `{"baseCatchModifier":0.8,"checkIntervalDays":2,"compatiblePrey":["rat","rabbit","hedgehog","irradiated_squirrel","fox"],"description":"A flat stone or salvaged plate balanced on a trigger stick. Low-tech, material-light, and as old as hunger. The trigger is the hard part. The rest is weight and patience.","displayName":"Deadfall Trap","durabilityChecks":5,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":2,"itemId":"scrap_wood"}],"trapEncounterChance":0.04,"trapType":"deadfall","trap_id":"trap_deadfall","weatherSensitivity…`
  - row 3: `{"baseCatchModifier":0.6,"checkIntervalDays":4,"compatiblePrey":["boar","deer"],"description":"A concealed pit dug into a game trail, lined with sharpened stakes. Labor-heavy, location-dependent, and capable of stopping something large. The earth does the work. The survivor does the digging.","displayName":"Pit Trap","durabilityChecks":10,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":3,"itemId":"scrap_wood"}],"trapEncounterChance":0.03,"trapType":"pit","trap_id":"trap_pit","weatherSensitivity":0.0}`
  - row 4: `{"baseCatchModifier":0.9,"bycatchChance":0.25,"bycatchSpecies":[{"speciesId":"rat","weight":3.0},{"speciesId":"hedgehog","weight":2.0},{"speciesId":"irradiated_squirrel","weight":1.0}],"checkIntervalDays":1,"compatiblePrey":["pheasant","ash_crow","contaminated_fowl","rabbit"],"description":"A mesh of cord or salvaged netting strung between posts or branches. Broader coverage than a snare, higher upfront cost, and more things can go wrong. Birds and small mammals both tangle in it.","displayName":"Net Trap","durabilityChecks":8,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_boot…`
  - row 5: `{"baseCatchModifier":0.7,"checkIntervalDays":3,"compatiblePrey":["mirror_carp","ash_pike","muskrat"],"description":"A woven funnel of wood and cord set in moving water. Fish enter but cannot find the exit. Strong where geography supports it, worthless where it does not. The freeze kills it. The thaw brings it back.","displayName":"Fish Trap","durabilityChecks":12,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":true,"setupCosts":[{"amount":2,"itemId":"scrap_wood"},{"amount":1,"itemId":"rope"}],"trapEncounterChance":0.02,"trapType":"fish_trap","trap_id…`
  - row 6: `{"baseCatchModifier":0.8,"checkIntervalDays":2,"compatiblePrey":["rabbit","rat","fox","rad_dog"],"description":"A sprung cage of salvaged metal and wire, triggered by weight on a plate. Expensive to build, durable once built. The animal lives inside it, which is either a mercy or a problem depending on what caught.","displayName":"Cage Trap","durabilityChecks":15,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":3,"itemId":"scrap_metal"},{"amount":1,"itemId":"box_of_nails_10"}],"trapEncounterChance":0.08,"trapType":"cage",…`
  - row 7: `{"baseCatchModifier":0.7,"bycatchChance":0.15,"bycatchSpecies":[{"speciesId":"irradiated_squirrel","weight":2.0},{"speciesId":"rat","weight":1.0}],"checkIntervalDays":1,"compatiblePrey":["pheasant","ash_crow","contaminated_fowl"],"description":"A fine noose of thread or thin wire set on a perch stick. Lightweight, fast to deploy, and fragile. Frequent small calories with a health-risk shadow. The birds that land on it are often the ones you should not eat.","displayName":"Bird Snare","durabilityChecks":4,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater…`
  - row 8: `{"baseCatchModifier":1.0,"checkIntervalDays":2,"compatiblePrey":["muskrat","fox","cotton_hare"],"description":"A heavy spring-loaded mechanism that closes steel jaws on anything that passes through the frame. Efficient, durable, and severe. The misfire is as dangerous as the catch. Manufactured before the exchange, or found in trapper caches.","displayName":"Body-Grip Trap","durabilityChecks":15,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":4,"itemId":"scrap_metal"},{"amount":1,"itemId":"leather_strap"}],"trapEncounter…`
  - row 9: `{"baseCatchModifier":0.8,"checkIntervalDays":2,"compatiblePrey":["rabbit","rat","hedgehog","irradiated_squirrel"],"description":"A wooden box with a gravity door, triggered by a treadle inside. Bulky, dependable, and shelter-craftable. The workhorse once the bunker can afford one. The animal is alive when you check it, which is the point.","displayName":"Box Trap","durabilityChecks":15,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":2,"itemId":"scrap_wood"},{"amount":1,"itemId":"scrap_metal"},{"amount":1,"itemId":"box_of…`
  - row 10: `{"baseCatchModifier":0.5,"checkIntervalDays":1,"compatiblePrey":["rat","rabbit"],"description":"A twist of copper wire shaped into a loose loop and pegged to the ground. The poorest survivor's option, better than doing nothing. It breaks often, catches little, and costs almost nothing to replace.","displayName":"Improvised Wire Snare","durabilityChecks":3,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":1,"itemId":"copper_wire_10m_of_10m"}],"trapEncounterChance":0.07,"trapType":"improvised_wire","trap_id":"trap_improvised…`
- `prey`: list[15]; union fields: `activeSeasons, attractedByBaitIds, baseYieldKg, contaminationDose, contaminationRisk, description, diseaseId, diseaseRisk, displayName, hideItemId, hideYield, isRareSpecies, migrationSpeciesId, minSkillLevel, moralWeight, moraleEffect, preferredTrapType, speciesId, toxicChance`
  - row 1: `{"activeSeasons":[],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.2,"contaminationRisk":0.05,"description":"A small grey-brown rabbit, lean from foraging in contaminated soil. Common enough to teach the trapping loop, modest enough to keep the survivor humble.","diseaseRisk":0.1,"displayName":"Ash Rabbit","hideItemId":"leather_strap","hideYield":0.3,"migrationSpeciesId":"","minSkillLevel":0,"moraleEffect":1.0,"preferredTrapType":"snare","speciesId":"rabbit","toxicChance":0.15}`
  - row 2: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.5,"contaminationRisk":0.05,"description":"A larger hare with pale winter fur that darkens in the thaw. Faster and less common than the rabbit, with a slightly stronger yield. Seasonal — follows the warmth.","diseaseRisk":0.1,"displayName":"Cotton Hare","hideItemId":"leather_strap","hideYield":0.4,"migrationSpeciesId":"species_cotton_hare","minSkillLevel":5,"preferredTrapType":"snare","speciesId":"cotton_hare","toxicChance":0.12}`
  - row 3: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_berry_mash","bait_salt_lick"],"baseYieldKg":15.0,"contaminationRisk":0.05,"description":"A gaunt mule deer, ribs showing through a dusty coat. Rare jackpot, never baseline passive food. Strong migration gating, high yield, long opportunity window. Cannot be caught by ordinary wire.","diseaseRisk":0.15,"displayName":"Wasteland Mule Deer","hideItemId":"leather_strap","hideYield":2.0,"migrationSpeciesId":"","minSkillLevel":40,"preferredTrapType":"pit","speciesId":"deer","toxicChance":0.1}`
  - row 4: `{"activeSeasons":["window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_salt_lick","bait_fat_cake"],"baseYieldKg":12.0,"contaminationRisk":0.1,"description":"A heavy-bodied boar with a ridge of bristled hair along its spine. High-value but risky — disease risk higher than deer, capable of destroying a trap. Heavy trap only.","diseaseRisk":0.2,"displayName":"Razorback Boar","hideItemId":"leather_strap","hideYield":1.8,"migrationSpeciesId":"species_ash_boar","minSkillLevel":60,"preferredTrapType":"pit","speciesId":"boar","toxicChance":0.25}`
  - row 5: `{"activeSeasons":[],"attractedByBaitIds":["bait_scrap_meat","bait_fat_cake"],"baseYieldKg":2.0,"contaminationRisk":0.1,"description":"A lean fox with patchy fur and alert eyes. Uncommon, lower food-efficiency than rabbit, with elevated disease risk. A pelt if you can get it, meat if you cannot.","diseaseRisk":0.2,"displayName":"Barren Fox","hideItemId":"leather_strap","hideYield":0.5,"migrationSpeciesId":"","minSkillLevel":10,"preferredTrapType":"deadfall","speciesId":"fox","toxicChance":0.2}`
  - row 6: `{"activeSeasons":[],"attractedByBaitIds":["bait_scrap_meat"],"baseYieldKg":0.6,"contaminationDose":4.0,"contaminationRisk":0.2,"description":"A bloated rat with thinning fur, common near ruins and shelter edges. Desperate food — low yield, among the highest disease risks. Useful in famine, dangerous in normal play.","diseaseId":"disease_typhoid_waterborne","diseaseRisk":0.35,"displayName":"Irradiated Rat","hideItemId":"","hideYield":0.0,"migrationSpeciesId":"species_blight_rat","minSkillLevel":0,"moraleEffect":-1.0,"preferredTrapType":"snare","speciesId":"rat","toxicChance":0.35}`
  - row 7: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.5,"contaminationRisk":0.1,"description":"A ground-feeding bird with dull plumage, common in the thaw and bloom. Low yield, contamination higher than rabbit. Bird-specialist traps work best.","diseaseRisk":0.15,"displayName":"Ash Pheasant","hideItemId":"","hideYield":0.2,"migrationSpeciesId":"species_ash_gull","minSkillLevel":5,"preferredTrapType":"bird_snare","speciesId":"pheasant","toxicChance":0.1}`
  - row 8: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_pheromone","bait_grain_lure"],"baseYieldKg":0.8,"contaminationDose":6.0,"contaminationRisk":0.15,"description":"A scavenger bird associated with contaminated ground and corpse-rich environments. Low food yield, higher contamination risk. The third eye is a mutation, not a metaphor.","diseaseId":"disease_blood_fever","diseaseRisk":0.25,"displayName":"Three-Eyed Sentry Crow","hideItemId":"","hideYield":0.1,"migrationSpeciesId":"species_iron_crow","minSkillLevel":10,"preferredTrapType":"bird_snare","speciesId":"ash_crow","toxicChance":0.…`
  - row 9: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.8,"contaminationRisk":0.08,"description":"A large-scaled carp, reliable in moving water. Water location required, seasonal — the freeze kills the fish trap, the thaw brings it back. Modest yield, low disease risk in cleaner zones.","diseaseRisk":0.1,"displayName":"Mirror Carp","hideItemId":"","hideYield":0.0,"migrationSpeciesId":"species_mirror_carp","minSkillLevel":0,"preferredTrapType":"fish_trap","speciesId":"mirror_carp","toxicChance":0.12}`
  - row 10: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_scrap_meat","bait_fat_cake"],"baseYieldKg":2.5,"contaminationRisk":0.1,"description":"A predatory freshwater fish, rarer and larger than the carp. Stronger yield, lower prevalence. Water only, seasonal window. The teeth are real.","diseaseRisk":0.12,"displayName":"Ash Pike","hideItemId":"","hideYield":0.0,"migrationSpeciesId":"","minSkillLevel":10,"preferredTrapType":"fish_trap","speciesId":"ash_pike","toxicChance":0.15}`
  - row 11: `{"activeSeasons":[],"attractedByBaitIds":["bait_berry_mash","bait_grain_lure"],"baseYieldKg":0.4,"contaminationDose":20.0,"contaminationRisk":0.35,"description":"A small rodent with patchy fur and swollen glands, common in contaminated zones. Low yield, very high contamination probability. Never a free source of safe meat.","diseaseId":"disease_spore_blight","diseaseRisk":0.3,"displayName":"Irradiated Squirrel","hideItemId":"","hideYield":0.0,"isRareSpecies":true,"migrationSpeciesId":"","minSkillLevel":0,"moralWeight":0.2,"preferredTrapType":"deadfall","speciesId":"irradiated_squirrel","toxicChance":0.45}`
  - row 12: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.0,"contaminationDose":12.0,"contaminationRisk":0.4,"description":"A bird with discolored plumage and labored breathing, visible ecological warning sign. Somewhat usable in desperation but medically risky. Very high contamination and disease risk.","diseaseId":"disease_zoonotic_flu","diseaseRisk":0.4,"displayName":"Contaminated Fowl","hideItemId":"","hideYield":0.1,"migrationSpeciesId":"","minSkillLevel":5,"moralWeight":0.5,"moraleEffect":-2.0,"preferredTrapType":"bird_snare","speciesId":"contaminated_fowl","to…`
  - row 13: `{"activeSeasons":[],"attractedByBaitIds":["bait_scrap_meat","bait_fat_cake"],"baseYieldKg":4.0,"contaminationDose":8.0,"contaminationRisk":0.2,"description":"A feral canine with patchy fur and wary eyes, traveling in small packs. Rare, morally and medically fraught. High disease risk. A prime candidate for a moral hook rather than a normal food source.","diseaseId":"disease_zoonotic_flu","diseaseRisk":0.3,"displayName":"Rad Dog","hideItemId":"leather_strap","hideYield":0.8,"migrationSpeciesId":"species_rad_dog","minSkillLevel":30,"moralWeight":0.9,"moraleEffect":-3.0,"preferredTrapType":"cage","speciesId":"rad_dog","toxicChance":0.3}`
  - row 14: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_berry_mash","bait_grain_lure"],"baseYieldKg":1.5,"contaminationRisk":0.15,"description":"A semi-aquatic rodent found near waterways and wetlands. Medium yield, meaningful disease risk, seasonal activity. The fur is worth more than the meat.","diseaseRisk":0.2,"displayName":"Marsh Muskrat","hideItemId":"leather_strap","hideYield":0.3,"migrationSpeciesId":"","minSkillLevel":10,"preferredTrapType":"body_grip","speciesId":"muskrat","toxicChance":0.2}`
  - row 15: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_berry_mash","bait_grain_lure"],"baseYieldKg":0.5,"contaminationRisk":0.05,"description":"A small spined mammal, seasonal and low-yield. Limited active window, modest disease risk. A deadfall or box trap target — too small for anything else.","diseaseRisk":0.15,"displayName":"Ash Hedgehog","hideItemId":"","hideYield":0.0,"migrationSpeciesId":"","minSkillLevel":0,"preferredTrapType":"deadfall","speciesId":"hedgehog","toxicChance":0.1}`
- `baits`: list[6]; union fields: `baitId, catchBonusMultiplier, craftCostChemicals, craftCostRoots, craftCostScrapMeat, displayName, preferredSpecies, toxicReduction`
  - row 1: `{"baitId":"bait_scrap_meat","catchBonusMultiplier":1.3,"craftCostChemicals":0,"craftCostRoots":0,"craftCostScrapMeat":1,"displayName":"Scrap-Meat Bait","preferredSpecies":["rat","fox","rad_dog"],"toxicReduction":0.0}`
  - row 2: `{"baitId":"bait_grain_lure","catchBonusMultiplier":1.5,"craftCostChemicals":0,"craftCostRoots":2,"craftCostScrapMeat":0,"displayName":"Grain Lure","preferredSpecies":["rabbit","pheasant","cotton_hare"],"toxicReduction":0.1}`
  - row 3: `{"baitId":"bait_pheromone","catchBonusMultiplier":2.0,"craftCostChemicals":1,"craftCostRoots":0,"craftCostScrapMeat":2,"displayName":"Mutated-Beast Pheromone Lure","preferredSpecies":["molerat","slag_beetle","ash_crow"],"toxicReduction":0.0}`
  - row 4: `{"baitId":"bait_fat_cake","catchBonusMultiplier":1.8,"craftCostChemicals":0,"craftCostRoots":0,"craftCostScrapMeat":2,"displayName":"Rendered Fat Cake","preferredSpecies":["fox","dust_lynx","wolf","rad_dog"],"toxicReduction":0.15}`
  - row 5: `{"baitId":"bait_berry_mash","catchBonusMultiplier":1.2,"craftCostChemicals":0,"craftCostRoots":3,"craftCostScrapMeat":0,"displayName":"Fermented Berry Mash","preferredSpecies":["rabbit","pheasant","deer","cotton_hare"],"toxicReduction":0.2}`
  - row 6: `{"baitId":"bait_salt_lick","catchBonusMultiplier":1.6,"craftCostChemicals":1,"craftCostRoots":1,"craftCostScrapMeat":0,"displayName":"Mineral Salt Lick","preferredSpecies":["deer","wolf","boar"],"toxicReduction":0.1}`
- Bytes: 23,204; SHA-256: `ad76e65163efc3860f4ef7d6b209479f27489c0f5cb34a717fe4e64bd5d64d00`
- Root keys: `baits, prey, schema_version, traps`
- `traps`: list[10]; union fields: `baseCatchModifier, bycatchChance, bycatchSpecies, checkIntervalDays, compatiblePrey, description, displayName, durabilityChecks, narrativeIncidentChance, narrativeIncidentIds, requiresWater, setupCosts, trapEncounterChance, trapType, trap_id, weatherSensitivity`
  - row 1: `{"baseCatchModifier":1.0,"checkIntervalDays":2,"compatiblePrey":["rabbit","cotton_hare","fox","rat"],"description":"A loop of cord or wire set across a game trail, tightened by a springy branch. Cheap enough to deploy more than one, unreliable enough that materials still matter. The wire remembers the shape of the animal it was meant to hold.","displayName":"Wire Snare","durabilityChecks":8,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":1,"itemId":"rope"}],"trapEncounterChance":0.05,"trapType":"snare","trap_id":"trap_sn…`
  - row 2: `{"baseCatchModifier":0.8,"checkIntervalDays":2,"compatiblePrey":["rat","rabbit","hedgehog","irradiated_squirrel","fox"],"description":"A flat stone or salvaged plate balanced on a trigger stick. Low-tech, material-light, and as old as hunger. The trigger is the hard part. The rest is weight and patience.","displayName":"Deadfall Trap","durabilityChecks":5,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":2,"itemId":"scrap_wood"}],"trapEncounterChance":0.04,"trapType":"deadfall","trap_id":"trap_deadfall","weatherSensitivity…`
  - row 3: `{"baseCatchModifier":0.6,"checkIntervalDays":4,"compatiblePrey":["boar","deer"],"description":"A concealed pit dug into a game trail, lined with sharpened stakes. Labor-heavy, location-dependent, and capable of stopping something large. The earth does the work. The survivor does the digging.","displayName":"Pit Trap","durabilityChecks":10,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":3,"itemId":"scrap_wood"}],"trapEncounterChance":0.03,"trapType":"pit","trap_id":"trap_pit","weatherSensitivity":0.0}`
  - row 4: `{"baseCatchModifier":0.9,"bycatchChance":0.25,"bycatchSpecies":[{"speciesId":"rat","weight":3.0},{"speciesId":"hedgehog","weight":2.0},{"speciesId":"irradiated_squirrel","weight":1.0}],"checkIntervalDays":1,"compatiblePrey":["pheasant","ash_crow","contaminated_fowl","rabbit"],"description":"A mesh of cord or salvaged netting strung between posts or branches. Broader coverage than a snare, higher upfront cost, and more things can go wrong. Birds and small mammals both tangle in it.","displayName":"Net Trap","durabilityChecks":8,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_boot…`
  - row 5: `{"baseCatchModifier":0.7,"checkIntervalDays":3,"compatiblePrey":["mirror_carp","ash_pike","muskrat"],"description":"A woven funnel of wood and cord set in moving water. Fish enter but cannot find the exit. Strong where geography supports it, worthless where it does not. The freeze kills it. The thaw brings it back.","displayName":"Fish Trap","durabilityChecks":12,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":true,"setupCosts":[{"amount":2,"itemId":"scrap_wood"},{"amount":1,"itemId":"rope"}],"trapEncounterChance":0.02,"trapType":"fish_trap","trap_id…`
  - row 6: `{"baseCatchModifier":0.8,"checkIntervalDays":2,"compatiblePrey":["rabbit","rat","fox","rad_dog"],"description":"A sprung cage of salvaged metal and wire, triggered by weight on a plate. Expensive to build, durable once built. The animal lives inside it, which is either a mercy or a problem depending on what caught.","displayName":"Cage Trap","durabilityChecks":15,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":3,"itemId":"scrap_metal"},{"amount":1,"itemId":"box_of_nails_10"}],"trapEncounterChance":0.08,"trapType":"cage",…`
  - row 7: `{"baseCatchModifier":0.7,"bycatchChance":0.15,"bycatchSpecies":[{"speciesId":"irradiated_squirrel","weight":2.0},{"speciesId":"rat","weight":1.0}],"checkIntervalDays":1,"compatiblePrey":["pheasant","ash_crow","contaminated_fowl"],"description":"A fine noose of thread or thin wire set on a perch stick. Lightweight, fast to deploy, and fragile. Frequent small calories with a health-risk shadow. The birds that land on it are often the ones you should not eat.","displayName":"Bird Snare","durabilityChecks":4,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater…`
  - row 8: `{"baseCatchModifier":1.0,"checkIntervalDays":2,"compatiblePrey":["muskrat","fox","cotton_hare"],"description":"A heavy spring-loaded mechanism that closes steel jaws on anything that passes through the frame. Efficient, durable, and severe. The misfire is as dangerous as the catch. Manufactured before the exchange, or found in trapper caches.","displayName":"Body-Grip Trap","durabilityChecks":15,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":4,"itemId":"scrap_metal"},{"amount":1,"itemId":"leather_strap"}],"trapEncounter…`
  - row 9: `{"baseCatchModifier":0.8,"checkIntervalDays":2,"compatiblePrey":["rabbit","rat","hedgehog","irradiated_squirrel"],"description":"A wooden box with a gravity door, triggered by a treadle inside. Bulky, dependable, and shelter-craftable. The workhorse once the bunker can afford one. The animal is alive when you check it, which is the point.","displayName":"Box Trap","durabilityChecks":15,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":2,"itemId":"scrap_wood"},{"amount":1,"itemId":"scrap_metal"},{"amount":1,"itemId":"box_of…`
  - row 10: `{"baseCatchModifier":0.5,"checkIntervalDays":1,"compatiblePrey":["rat","rabbit"],"description":"A twist of copper wire shaped into a loose loop and pegged to the ground. The poorest survivor's option, better than doing nothing. It breaks often, catches little, and costs almost nothing to replace.","displayName":"Improvised Wire Snare","durabilityChecks":3,"narrativeIncidentChance":0.05,"narrativeIncidentIds":["trap_sprung_blood_trail","trap_bait_stolen","trap_human_bootprints"],"requiresWater":false,"setupCosts":[{"amount":1,"itemId":"copper_wire_10m_of_10m"}],"trapEncounterChance":0.07,"trapType":"improvised_wire","trap_id":"trap_improvised…`
- `prey`: list[15]; union fields: `activeSeasons, attractedByBaitIds, baseYieldKg, contaminationDose, contaminationRisk, description, diseaseId, diseaseRisk, displayName, hideItemId, hideYield, isRareSpecies, migrationSpeciesId, minSkillLevel, moralWeight, moraleEffect, preferredTrapType, speciesId, toxicChance`
  - row 1: `{"activeSeasons":[],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.2,"contaminationRisk":0.05,"description":"A small grey-brown rabbit, lean from foraging in contaminated soil. Common enough to teach the trapping loop, modest enough to keep the survivor humble.","diseaseRisk":0.1,"displayName":"Ash Rabbit","hideItemId":"leather_strap","hideYield":0.3,"migrationSpeciesId":"","minSkillLevel":0,"moraleEffect":1.0,"preferredTrapType":"snare","speciesId":"rabbit","toxicChance":0.15}`
  - row 2: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.5,"contaminationRisk":0.05,"description":"A larger hare with pale winter fur that darkens in the thaw. Faster and less common than the rabbit, with a slightly stronger yield. Seasonal — follows the warmth.","diseaseRisk":0.1,"displayName":"Cotton Hare","hideItemId":"leather_strap","hideYield":0.4,"migrationSpeciesId":"species_cotton_hare","minSkillLevel":5,"preferredTrapType":"snare","speciesId":"cotton_hare","toxicChance":0.12}`
  - row 3: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_berry_mash","bait_salt_lick"],"baseYieldKg":15.0,"contaminationRisk":0.05,"description":"A gaunt mule deer, ribs showing through a dusty coat. Rare jackpot, never baseline passive food. Strong migration gating, high yield, long opportunity window. Cannot be caught by ordinary wire.","diseaseRisk":0.15,"displayName":"Wasteland Mule Deer","hideItemId":"leather_strap","hideYield":2.0,"migrationSpeciesId":"","minSkillLevel":40,"preferredTrapType":"pit","speciesId":"deer","toxicChance":0.1}`
  - row 4: `{"activeSeasons":["window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_salt_lick","bait_fat_cake"],"baseYieldKg":12.0,"contaminationRisk":0.1,"description":"A heavy-bodied boar with a ridge of bristled hair along its spine. High-value but risky — disease risk higher than deer, capable of destroying a trap. Heavy trap only.","diseaseRisk":0.2,"displayName":"Razorback Boar","hideItemId":"leather_strap","hideYield":1.8,"migrationSpeciesId":"species_ash_boar","minSkillLevel":60,"preferredTrapType":"pit","speciesId":"boar","toxicChance":0.25}`
  - row 5: `{"activeSeasons":[],"attractedByBaitIds":["bait_scrap_meat","bait_fat_cake"],"baseYieldKg":2.0,"contaminationRisk":0.1,"description":"A lean fox with patchy fur and alert eyes. Uncommon, lower food-efficiency than rabbit, with elevated disease risk. A pelt if you can get it, meat if you cannot.","diseaseRisk":0.2,"displayName":"Barren Fox","hideItemId":"leather_strap","hideYield":0.5,"migrationSpeciesId":"","minSkillLevel":10,"preferredTrapType":"deadfall","speciesId":"fox","toxicChance":0.2}`
  - row 6: `{"activeSeasons":[],"attractedByBaitIds":["bait_scrap_meat"],"baseYieldKg":0.6,"contaminationDose":4.0,"contaminationRisk":0.2,"description":"A bloated rat with thinning fur, common near ruins and shelter edges. Desperate food — low yield, among the highest disease risks. Useful in famine, dangerous in normal play.","diseaseId":"disease_typhoid_waterborne","diseaseRisk":0.35,"displayName":"Irradiated Rat","hideItemId":"","hideYield":0.0,"migrationSpeciesId":"species_blight_rat","minSkillLevel":0,"moraleEffect":-1.0,"preferredTrapType":"snare","speciesId":"rat","toxicChance":0.35}`
  - row 7: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.5,"contaminationRisk":0.1,"description":"A ground-feeding bird with dull plumage, common in the thaw and bloom. Low yield, contamination higher than rabbit. Bird-specialist traps work best.","diseaseRisk":0.15,"displayName":"Ash Pheasant","hideItemId":"","hideYield":0.2,"migrationSpeciesId":"species_ash_gull","minSkillLevel":5,"preferredTrapType":"bird_snare","speciesId":"pheasant","toxicChance":0.1}`
  - row 8: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_pheromone","bait_grain_lure"],"baseYieldKg":0.8,"contaminationDose":6.0,"contaminationRisk":0.15,"description":"A scavenger bird associated with contaminated ground and corpse-rich environments. Low food yield, higher contamination risk. The third eye is a mutation, not a metaphor.","diseaseId":"disease_blood_fever","diseaseRisk":0.25,"displayName":"Three-Eyed Sentry Crow","hideItemId":"","hideYield":0.1,"migrationSpeciesId":"species_iron_crow","minSkillLevel":10,"preferredTrapType":"bird_snare","speciesId":"ash_crow","toxicChance":0.…`
  - row 9: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.8,"contaminationRisk":0.08,"description":"A large-scaled carp, reliable in moving water. Water location required, seasonal — the freeze kills the fish trap, the thaw brings it back. Modest yield, low disease risk in cleaner zones.","diseaseRisk":0.1,"displayName":"Mirror Carp","hideItemId":"","hideYield":0.0,"migrationSpeciesId":"species_mirror_carp","minSkillLevel":0,"preferredTrapType":"fish_trap","speciesId":"mirror_carp","toxicChance":0.12}`
  - row 10: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_scrap_meat","bait_fat_cake"],"baseYieldKg":2.5,"contaminationRisk":0.1,"description":"A predatory freshwater fish, rarer and larger than the carp. Stronger yield, lower prevalence. Water only, seasonal window. The teeth are real.","diseaseRisk":0.12,"displayName":"Ash Pike","hideItemId":"","hideYield":0.0,"migrationSpeciesId":"","minSkillLevel":10,"preferredTrapType":"fish_trap","speciesId":"ash_pike","toxicChance":0.15}`
  - row 11: `{"activeSeasons":[],"attractedByBaitIds":["bait_berry_mash","bait_grain_lure"],"baseYieldKg":0.4,"contaminationDose":20.0,"contaminationRisk":0.35,"description":"A small rodent with patchy fur and swollen glands, common in contaminated zones. Low yield, very high contamination probability. Never a free source of safe meat.","diseaseId":"disease_spore_blight","diseaseRisk":0.3,"displayName":"Irradiated Squirrel","hideItemId":"","hideYield":0.0,"isRareSpecies":true,"migrationSpeciesId":"","minSkillLevel":0,"moralWeight":0.2,"preferredTrapType":"deadfall","speciesId":"irradiated_squirrel","toxicChance":0.45}`
  - row 12: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.0,"contaminationDose":12.0,"contaminationRisk":0.4,"description":"A bird with discolored plumage and labored breathing, visible ecological warning sign. Somewhat usable in desperation but medically risky. Very high contamination and disease risk.","diseaseId":"disease_zoonotic_flu","diseaseRisk":0.4,"displayName":"Contaminated Fowl","hideItemId":"","hideYield":0.1,"migrationSpeciesId":"","minSkillLevel":5,"moralWeight":0.5,"moraleEffect":-2.0,"preferredTrapType":"bird_snare","speciesId":"contaminated_fowl","to…`
  - row 13: `{"activeSeasons":[],"attractedByBaitIds":["bait_scrap_meat","bait_fat_cake"],"baseYieldKg":4.0,"contaminationDose":8.0,"contaminationRisk":0.2,"description":"A feral canine with patchy fur and wary eyes, traveling in small packs. Rare, morally and medically fraught. High disease risk. A prime candidate for a moral hook rather than a normal food source.","diseaseId":"disease_zoonotic_flu","diseaseRisk":0.3,"displayName":"Rad Dog","hideItemId":"leather_strap","hideYield":0.8,"migrationSpeciesId":"species_rad_dog","minSkillLevel":30,"moralWeight":0.9,"moraleEffect":-3.0,"preferredTrapType":"cage","speciesId":"rad_dog","toxicChance":0.3}`
  - row 14: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_berry_mash","bait_grain_lure"],"baseYieldKg":1.5,"contaminationRisk":0.15,"description":"A semi-aquatic rodent found near waterways and wetlands. Medium yield, meaningful disease risk, seasonal activity. The fur is worth more than the meat.","diseaseRisk":0.2,"displayName":"Marsh Muskrat","hideItemId":"leather_strap","hideYield":0.3,"migrationSpeciesId":"","minSkillLevel":10,"preferredTrapType":"body_grip","speciesId":"muskrat","toxicChance":0.2}`
  - row 15: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_berry_mash","bait_grain_lure"],"baseYieldKg":0.5,"contaminationRisk":0.05,"description":"A small spined mammal, seasonal and low-yield. Limited active window, modest disease risk. A deadfall or box trap target — too small for anything else.","diseaseRisk":0.15,"displayName":"Ash Hedgehog","hideItemId":"","hideYield":0.0,"migrationSpeciesId":"","minSkillLevel":0,"preferredTrapType":"deadfall","speciesId":"hedgehog","toxicChance":0.1}`
- `baits`: list[6]; union fields: `baitId, catchBonusMultiplier, craftCostChemicals, craftCostRoots, craftCostScrapMeat, displayName, preferredSpecies, toxicReduction`
  - row 1: `{"baitId":"bait_scrap_meat","catchBonusMultiplier":1.3,"craftCostChemicals":0,"craftCostRoots":0,"craftCostScrapMeat":1,"displayName":"Scrap-Meat Bait","preferredSpecies":["rat","fox","rad_dog"],"toxicReduction":0.0}`
  - row 2: `{"baitId":"bait_grain_lure","catchBonusMultiplier":1.5,"craftCostChemicals":0,"craftCostRoots":2,"craftCostScrapMeat":0,"displayName":"Grain Lure","preferredSpecies":["rabbit","pheasant","cotton_hare"],"toxicReduction":0.1}`
  - row 3: `{"baitId":"bait_pheromone","catchBonusMultiplier":2.0,"craftCostChemicals":1,"craftCostRoots":0,"craftCostScrapMeat":2,"displayName":"Mutated-Beast Pheromone Lure","preferredSpecies":["molerat","slag_beetle","ash_crow"],"toxicReduction":0.0}`
  - row 4: `{"baitId":"bait_fat_cake","catchBonusMultiplier":1.8,"craftCostChemicals":0,"craftCostRoots":0,"craftCostScrapMeat":2,"displayName":"Rendered Fat Cake","preferredSpecies":["fox","dust_lynx","wolf","rad_dog"],"toxicReduction":0.15}`
  - row 5: `{"baitId":"bait_berry_mash","catchBonusMultiplier":1.2,"craftCostChemicals":0,"craftCostRoots":3,"craftCostScrapMeat":0,"displayName":"Fermented Berry Mash","preferredSpecies":["rabbit","pheasant","deer","cotton_hare"],"toxicReduction":0.2}`
  - row 6: `{"baitId":"bait_salt_lick","catchBonusMultiplier":1.6,"craftCostChemicals":1,"craftCostRoots":1,"craftCostScrapMeat":0,"displayName":"Mineral Salt Lick","preferredSpecies":["deer","wolf","boar"],"toxicReduction":0.1}`

# Appendix D — Current caller/reference graph

### `WeatherSystem` (18 sampled current references)
- Assets/Ashfall.Core/AtmosphericCondenserSystem.cs:103: private readonly WeatherSystem _weather;
- Assets/Ashfall.Core/AtmosphericCondenserSystem.cs:115: WeatherSystem weather, ILog? log = null)
- Assets/Ashfall.Core/District8DeepCoastSystem.cs:96: // WeatherSystem → this system's daily tick is the ONLY authority for
- Assets/Ashfall.Core/District8DeepCoastSystem.cs:622: // WeatherSystem → this daily tick is the ONLY producer of coastal surge
- Assets/Ashfall.Core/LocationEvolutionSystem.Live.cs:14: /// <summary>WeatherSystem.OutdoorRadModifier for the day (1.0 = clear baseline).</summary>
- Assets/Ashfall.Core/WeatherKind.cs:6: /// Integer values match Assets/_Game/Environment/WeatherSystem.cs so Unity
- Assets/Ashfall.Core/WeatherStationSystem.cs:55: private readonly WeatherSystem _weatherSystem;
- Assets/Ashfall.Core/WeatherStationSystem.cs:89: public WeatherStationSystem(WeatherSystem weatherSystem, ISeededRng rng, ILog? log = null, WeatherGateCatalog? gateCatalog = null)
- Assets/Ashfall.Core/WeatherStationSystem.cs:91: _weatherSystem = weatherSystem ?? throw new ArgumentNullException(nameof(weatherSystem));
- Assets/Ashfall.Core/WeatherStationSystem.cs:178: var rawForecast = _weatherSystem.PeekForecast(horizon);
- Assets/Ashfall.Core/WeatherStationSystem.cs:209: temperature = 5f + WeatherSystem.TemperaturePenaltyForWeather(f.Kind),
- Assets/Ashfall.Core/WildlifeSeasonalCalendar.cs:96: /// <see cref="WeatherSystem.GetSeasonForDay"/> (last window whose
- Assets/Ashfall.Core/WildlifeTrappingSystem.cs:106: /// WT-INT-01: Carries live WeatherSystem snapshot and per-hunter skill levels.
- Assets/Ashfall.Core/WildlifeTrappingSystem.cs:115: /// <summary>Current authoritative weather snapshot from WeatherSystem.</summary>
- Assets/Ashfall.Core/SumpFloodingSystem.cs:147: private readonly WeatherSystem _weather;
- Assets/Ashfall.Core/SumpFloodingSystem.cs:246: WeatherSystem weather,
- Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs:129: /// windowMinDays &lt;= day — see WeatherSystem.GetSeasonForDay), so
- Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs:28: private readonly WeatherSystem? _weather;
### `WeatherSeason` (3 sampled current references)
- Ashfall.Core.Tests/World/WeatherSeasonExpansionTests.cs:18: public sealed class WeatherSeasonExpansionTests
- Ashfall.Core.Tests/World/WeatherSeasonExpansionTests.cs:24: public WeatherSeasonExpansionTests()
- Ashfall.Core.Tests/Campaign/Plan83_74WeatherNarrativeIntegrationTests.cs:35: public void WeatherSeasons_HasTenWindowsWithStrictlyIncreasingStartDays()
### `CampaignCalendar` (18 sampled current references)
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:16: public interface ICampaignCalendar
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:31: CampaignCalendarReadModel CurrentReadModel { get; }
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:34: CampaignCalendarReadModel ResolveDay(int day);
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:49: /// <summary>Default concrete implementation of <see cref="ICampaignCalendar"/>.</summary>
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:50: public sealed class CampaignCalendar : ICampaignCalendar
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:59: public CampaignCalendarReadModel CurrentReadModel => ResolveDay(_currentDay);
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:64: public CampaignCalendar(int initialDay = 1, Ashfall.Core.World.SeasonProfileDef? profile = null)
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:92: public CampaignCalendarReadModel ResolveDay(int day)
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:133: return new CampaignCalendarReadModel(
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:232: /// <summary>Projects <see cref="ICampaignCalendar"/> to the historical <see cref="IClock"/> port.</summary>
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:235: private readonly ICampaignCalendar _calendar;
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:237: public CalendarClockAdapter(ICampaignCalendar calendar)
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:256: /// <summary>Projects <see cref="ICampaignCalendar"/> to the <see cref="ISimClock"/> intraday clock.</summary>
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:262: private readonly ICampaignCalendar _calendar;
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:265: public CalendarSimClockAdapter(ICampaignCalendar calendar)
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:311: public static class CampaignCalendarReconciler
- Assets/Ashfall.Core/Campaign/CampaignCalendarReadModel.cs:13: public sealed class CampaignCalendarReadModel
- Assets/Ashfall.Core/Campaign/CampaignCalendarReadModel.cs:30: public CampaignCalendarReadModel(
### `WeatherIntelligenceCoordinator` (18 sampled current references)
- Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs:103: public sealed class WeatherIntelligenceCoordinator
- Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs:122: public WeatherIntelligenceCoordinator(
- src/Host/HostCli.DynamicWorld.cs:152: var coord = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(777));
- src/Host/HostCli.DynamicWorld.cs:168: var coordRestored = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(888));
- src/Host/WorldHostSession.cs:23: public WeatherIntelligenceCoordinator WeatherIntelligence { get; }
- src/Host/WorldHostSession.cs:97: WeatherIntelligence = new WeatherIntelligenceCoordinator(Weather, SkyArmor, new SeededRng(weatherSeed));
- src/UI/WeatherForecastPanel.cs:29: private Ashfall.Core.World.WeatherIntelligenceCoordinator? _intelligence;
- src/UI/WeatherForecastPanel.cs:34: public void Bind(WeatherSystem weather, Ashfall.Core.World.WeatherIntelligenceCoordinator? intelligence = null)
- Ashfall.Core.Tests/WeatherIntelligenceCoordinatorTests.cs:9: public class WeatherIntelligenceCoordinatorTests
- Ashfall.Core.Tests/WeatherIntelligenceCoordinatorTests.cs:225: private static WeatherIntelligenceCoordinator Create(out WeatherSystem weather)
- Ashfall.Core.Tests/WeatherIntelligenceCoordinatorTests.cs:231: return new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(42));
- Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs:237: public void WeatherIntelligenceCoordinator_SaveRestoreRoundTrip_PreservesAllStates()
- Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs:243: var coord = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(8888));
- Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs:266: var coord2 = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(9999));
- Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs:319: public void WeatherIntelligenceCoordinator_SevereForecast_PopulatesCrisisFields()
- Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs:324: var coord = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(12345));
- Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs:352: public void WeatherIntelligenceCoordinator_NoSevereWeather_HasNoPredictedCrisis()
- Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs:357: var coord = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(9999));
### `weather_seasons.json` (18 sampled current references)
- Assets/Ashfall.Core/WildlifeSeasonalCalendar.cs:35: /// (<see cref="SeasonProfileDef"/> from weather_seasons.json) remain the
- Assets/Ashfall.Core/WildlifeSeasonalCalendar.cs:43: // ── Season window ids (weather_seasons.json is the authority) ───
- Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs:38: /// Bind the Plan 19 season profile (weather_seasons.json). Optional:
- Assets/Ashfall.Core/World/WeatherSystem.cs:500: /// <summary>Engine-agnostic loader for weather_seasons.json.</summary>
- Assets/Ashfall.Core/World/WeatherSystem.cs:503: public const string FileName = "weather_seasons.json";
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:37: "weather_seasons.json", "radio.json", "narrative_encounters.json",
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:355: ["weather_seasons.json"] = new[] { "WeatherSystem" },
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:764: ["weather_seasons.json"] = "WeatherSystem",
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:956: ["weather_seasons.json"] = new[] { "WeatherSystem" },
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1364: "economy_goods.json", "events.json", "weather_seasons.json",
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1421: ["weather_seasons.json"] = new[] { "DashboardHUD" },
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1720: "radio.json", "events.json", "weather_seasons.json",
- Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:36: /// <summary>Binds authored season profile definition (e.g. from weather_seasons.json).</summary>
- Assets/Ashfall.Core/IO/CatalogBootValidator.cs:208: RegisterCatalog("weather_seasons.json", "Weather Seasons", CatalogClassification.Required);
- src/Host/ContentUtilizationRuntimeCollector.cs:843: string path = Path.Combine(dataDir, "weather_seasons.json");
- src/Host/ContentUtilizationRuntimeCollector.cs:845: instr.RecordCatalogOpened("weather_seasons.json", "WeatherSystem");
- src/Host/ContentUtilizationRuntimeCollector.cs:847: instr.RecordCatalogDeserialized("weather_seasons.json", 1);
- src/Host/ContentUtilizationRuntimeCollector.cs:848: instr.RecordDefinitionsRegistered("weather_seasons.json", "WeatherSystem", 1);

# Appendix E — Current focused-test inventory

Current test declaration inventory: 102 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/World/WeatherSeasonExpansionTests.cs` — 26 test declarations; bytes=30,309; SHA-256=`a2378b5c7b0d33e3ce3fc71c8213b5a353d7bd3f63c42fca8799095208f2407f`
- 00055: [Fact]
- 00056: public void Points_01_to_12_Catalogue_LoadsExactTenWindowsWithPreservedAndNewPhases()
- 00093: [Fact]
- 00094: public void Points_13_to_16_Catalogue_IdsAreUniqueAndStartDaysStrictlyIncreasing()
- 00125: [Fact]
- 00126: public void Points_17_to_38_ActiveWindow_SelectionSemantics_AndBoundaryDays()
- 00183: [Fact]
- 00184: public void Points_39_to_43_WeightVectors_AreValidFiniteAndNonNegative()
- 00222: [Fact]
- 00223: public void Point_44_AllTenSevenWeightVectors_AreStrictlyUnique()
- 00259: [Fact]
- 00260: public void Points_45_to_51_DominantSignatures_ThematicIntegrityAndAdjacentDifferences()
- 00335: [Fact]
- 00336: public void Points_52_to_53_NormalizedProbabilities_SumToOneAndZeroHandled()
- 00375: [Fact]
- 00376: public void Points_54_to_60_WeatherSystem_ProducesValidWeatherAcrossMilestones()
- 00395: [Fact]
- 00396: public void Points_61_to_64_Plan48WeatherGates_FirstFalloutAndBlackRainGatesResolve()
- 00435: [Fact]
- 00436: public void Points_65_to_67_DeterminismAndLookaheadIntegrity()
- 00476: [Fact]
- 00477: public void Points_68_to_72_SaveLoad_StateRoundTripPreservesStateAndReDerivesWindow()
- 00523: [Fact]
- 00524: public void Point_73_FullYearProgression_VisitsAllTenWindowsInChronologicalSequence()
- 00561: [Fact]
- 00562: public void Point_74_WeatherSystem_CatalogAndRuntimeIntegrity_PreconditionsHold()
### `Ashfall.Core.Tests/Campaign/Plan83_74WeatherNarrativeIntegrationTests.cs` — 8 test declarations; bytes=6,890; SHA-256=`d44391337ba6f9fcd3edd54061f1273c8e6c8c0d877c4ce55019d41c652fc4e1`
- 00034: [Fact]
- 00035: public void WeatherSeasons_HasTenWindowsWithStrictlyIncreasingStartDays()
- 00071: [Fact]
- 00072: public void NarrativeProgression_HasFifteenChaptersWithContiguousOrder()
- 00094: [Fact]
- 00095: public void CrossSystem_BothCatalogsLoadIndependentlyAndProvideFullCampaignCoverage()
- 00123: [Fact]
- 00124: public void CrossSystem_WeatherAndNarrativeReflectCampaignEvolutionTones()
### `Ashfall.Core.Tests/World/WeatherGateCatalogIntegrityTests.cs` — 42 test declarations; bytes=25,599; SHA-256=`c4bfefad2b8841510cf3e1c1f96b32a0467613ee408636779f6a5bebd4f9420a`
- 00042: [Fact]
- 00043: public void F14_CatalogLoads_Exactly18Gates()
- 00050: [Fact]
- 00051: public void F14_DomainCatalogLoads_AllGatesRegistered()
- 00058: [Fact]
- 00059: public void F14_AllGateIdsAreUnique()
- 00069: [Fact]
- 00070: public void F14_AllRouteGateTargetsResolve()
- 00103: [Fact]
- 00104: public void F14_AllWeatherKindsAreCanonical()
- 00130: [Fact]
- 00131: public void F14_AllOverrideItemsResolve()
- 00154: [Fact]
- 00155: public void F16_NoGateHasRequiredBlockedOverlap()
- 00169: [Fact]
- 00170: public void F16_SevenRollableWeatherStates_AllHavePositiveWeights()
- 00189: [Fact]
- 00190: public void F16_WeatherGateTruthTable_WeatherRowsAllGatesCorrect()
- 00241: [Fact]
- 00242: public void F16_PositiveGates_OpenOnlyDuringRequiredWeather()
- 00271: [Fact]
- 00272: public void F16_NegativeGates_BlockedOnlyDuringBlockedWeather()
- 00297: [Fact]
- 00298: public void F16_OverrideMatrix_CorrectBehavior()
- 00329: [Fact]
- 00330: public void F16_RadioTransitions_EmitOnStateChange_NotOnSameState()
- 00358: [Fact]
- 00359: public void F14_360DaySimulation_WeatherDistribution()
- 00387: [Fact]
- 00388: public void F14_PerGateUtilization_Calculated()
- 00411: [Fact]
- 00412: public void F14_DeadGateDetection_BioFogGatesAreDead()
- 00427: [Fact]
- 00428: public void F14_DeadGateDetection_EMPGateIsDead()
- 00438: [Fact]
- 00439: public void F14_BlizzardGates_HighBlockedRate_InColdSeasons()
- 00458: [Fact]
- 00459: public void F14_NoRedundantGates_SameTargetSameWeather()
- 00495: [Fact]
- 00496: public void F14_NoOrphanGates_AllTargetsExistInData()
- 00525: [Fact]
- 00526: public void F16_DeterminismLinkage_SameSeedSameGates()
### `Ashfall.Core.Tests/World/WeatherGateBalanceAuditTests.cs` — 26 test declarations; bytes=14,110; SHA-256=`266c7b4c3a93cfc4bb4348cb74d1f37ac5f6d607222353750027a612b9228bf8`
- 00033: [Fact]
- 00034: public void F15_EveryGatedRouteHasAvailabilityStats()
- 00051: [Fact]
- 00052: public void F15_SeasonalDistribution_Calculated()
- 00066: [Fact]
- 00067: public void F15_PositiveGates_IceRoads_OpenDuringBlizzard()
- 00090: [Fact]
- 00091: public void F15_OverrideCoverage_Measured()
- 00111: [Fact]
- 00112: public void F15_EMPGateAudit_ZeroWeightConfirmed()
- 00125: [Fact]
- 00126: public void F15_BioFogGateAudit_ZeroWeightConfirmed()
- 00145: [Fact]
- 00146: public void F15_FalloutStormAnalysis_MeaningfulFrequency()
- 00167: [Fact]
- 00168: public void F15_BlizzardAnalysis_HighConcentrationInColdSeasons()
- 00187: [Fact]
- 00188: public void F15_BlizzardNetworkClosure_MaxSimultaneousBlocks()
- 00222: [Fact]
- 00223: public void F15_BlackRainAnalysis_MidFrequency()
- 00241: [Fact]
- 00242: public void F15_NetworkClosureMetrics_Calculated()
- 00256: [Fact]
- 00257: public void F15_DestinationGates_HaveForcePassageCosts()
- 00274: [Fact]
- 00275: public void F15_BalanceReport_Generated()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Weather Seasons: Ten-Window Calendar, Weather Effects, and Consumer Reachability` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan says WeatherSystem directly consumes all seven weights; current evidence must distinguish loader support from actual production weighting.
- The historical plan names two route-gate wires without a current caller proof; those claims are downgraded until traced.

## H.2 Evidence-strength corrections
- Separate static season windows from active weather output.
- Trace the current production consumer rather than inferring from tests.
- Preserve Year-of-Ash and forecast contracts.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Weather Seasons: Ten-Window Calendar, Weather Effects, and Consumer Reachability while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use WeatherSystem for season loading/weather selection.
- Use CampaignCalendar for campaign day and season projection.
- Use WeatherEffectsCatalog for per-kind effects.
- Use existing gate/dose/expedition consumers for consequences.
- Keep UI as a truthful projection and preserve the current weather stream.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement weather seasons: ten-window calendar, weather effects, and consumer reachability arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- No new season owner or weather stream is proposed.
- The final handoff must name the current effect authority and uncertainty language.

## J.2 Questions deliberately left open
- Should specialized consumers receive the season id, a derived climate profile, or only canonical weather effects?
- Which current panel is the authoritative season/effect surface?

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

The original file at `HEAD:piagentsplans/83-weather-seasons-expansion.md` contained 4,812 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 83 — Weather Season Windows Expansion (3 → 10 season windows)

## Goal (2 lines)
Expand `weather_seasons.json` from 3 verified season windows to 10. The weather
system (`WeatherSystem.cs` confirmed live) defines seasonal windows that weight
weather types (clear, rain, overcast, ashfall, fallout storm, blizzard, black
rain). 3 windows (First Thaw, Deep Freeze, Long Winter) cover only the early
and late game — the mid-campaign has no seasonal weather variation.

## Why (P2)
- Verified: `weather_seasons.json` has 3 season windows (id, displayName,
  startDay, clearWeight, rainWeight, overcastWeight, ashfallWeight,
  falloutStormWeight, blizzardWeight, blackRainWeight).
  `WeatherSystem.cs` is confirmed in Core.
- Creates the weather-progression pillar: weather should shift across the
  campaign — early confusion (variable weather), mid-campaign stabilization
  (ashfall dominant), late-game nuclear winter (blizzard and black rain
  dominant). 3 windows means the mid-game weather is a flat line.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/weather_seasons.json` (expand 3 → 10 windows)
- Read-only: `Assets/Ashfall.Core/World/WeatherSystem.cs` (confirm how season
  windows are selected by startDay and how weights are applied)

## Content grammar (per season window)
- snake_case `id` with prefix `window_` (confirmed prefix).
- startDay: integer — the day this window begins. Windows are ordered by
  startDay and non-overlapping. The system selects the active window by current
  day.
- displayName: 1–3 words evoking the campaign phase (First Thaw, Deep Freeze,
  Long Winter, Ash Settling, Spring Storms, etc.).
- Weather weights: 7 weight fields (clearWeight, rainWeight, overcastWeight,
  ashfallWeight, falloutStormWeight, blizzardWeight, blackRainWeight) — each
  0.0–3.0. Higher weight = more likely. Each window should have a distinct
  weather signature (no two windows with identical weight profiles).
- Campaign arc: early windows (variable, moderate danger) → mid windows
  (ashfall dominant, rising danger) → late windows (blizzard/black rain
  dominant, high danger).

## Steps
1. Read `WeatherSystem.cs` to confirm how season windows are selected (by
   startDay — does it pick the latest window with startDay <= current day?) and
   how weights are normalized.
2. Confirm the existing 3 windows (First Thaw day 0, Deep Freeze day 60, Long
   Winter day 240) and identify the gap (days 61–239 have no window transitions).
3. Author 7 new windows filling the mid-campaign and extending the late game:
   - `window_ash_settling` (day 30): ashfall rising, storms declining.
   - `window_spring_storms` (day 90): rain and overcast dominant, brief clear.
   - `window_dry_ash` (day 120): ashfall dominant, low rain, rising fallout.
   - `window_first_fallout` (day 150): fallout storms peak, black rain appears.
   - `window_false_spring` (day 180): brief clear window, deceptive calm.
   - `window_deep_ash` (day 200): ashfall heavy, blizzard rising.
   - `window_black_rain_season` (day 280): black rain dominant, worst weather.
4. Each window: distinct weight profile across all 7 weather types. No two
   windows should have the same dominant weather.
5. Cross-reference: all window ids unique; startDays are strictly increasing;
   every day 0–365+ falls within a window (or confirm the system handles gaps by
   using the last active window).
6. Wire 2 windows into Plan 48 weather gates (black rain season and first
   fallout season block specific expedition routes).
7. Validate: `--data-integrity-selftest`; run a headless day-advance test to
   confirm windows activate at the correct day.
8. xUnit: weather season catalog loads 10 windows, all ids unique, startDays
   strictly increasing, all weights within valid ranges, no two windows with
   identical weight profiles.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is startDay gaps (step 5): confirm the system
uses the last active window for days beyond the latest startDay, and that no
two windows share a startDay.

## Definition of Done
- `weather_seasons.json` has 10 windows, all ids unique, startDays strictly
  increasing, 2 wired to weather gates, integrity + tests green.

## Follow-on
- Plan 48 (weather gates) — seasonal windows block expedition routes.
- Plan 81 (dose locations) — seasonal fallout shifts surface dose levels.
- Plan 77 (duty roster seasons) — weather windows should align with roster
  seasons for consistent campaign pacing.
- Plan 74 (campaign chapters) — weather windows mark chapter transitions.
- Existing 19 (dynamic world systems) — this plan provides the seasonal data.

```

## End of Plan 83 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/World/WeatherSystem.cs` — 532 lines; 22,049 bytes; SHA-256 `23309eb8803b1759e8d529078baea0677ee6d24f777601e3023fb1c6c12ea341`
Declaration index:
- 00011: public class SeasonWindowDef
- 00027: public class SeasonProfileDef
- 00037: public class WorldWeatherState
- 00058: public class WeatherSystem
- 00093: public void BindWeatherEffects(WeatherEffectsCatalog? catalog)
- 00108: public void BindProfile(SeasonProfileDef profile, int seed)
- 00114: public SeasonWindowDef GetSeasonForDay(int day)
- 00142: public void Tick(float gameHours)
- 00161: public void ForceWeather(WeatherKind kind)
- 00168: private WeatherKind RollNextState()
- 00205: private static WeatherKind RollKind(double roll, float clear, float rain, float overcast,
- 00222: private void SetCurrent(WeatherKind next)
- 00230: private float NextCheckInterval()
- 00282: public float ForecastRadModifier(WeatherKind kind)
- 00296: public bool IsScavengingBlocked(bool hasFullSuit) =>
- 00299: public float GetTemperaturePenaltyCelsius()
- 00313: public static float TemperaturePenaltyForWeather(WeatherKind kind)
- 00331: public float TemperaturePenaltyC(WeatherKind kind)
- 00344: public float VisibilityModifier(WeatherKind kind)
- 00361: public WorldWeatherState CaptureState()
- 00376: public void RestoreState(WorldWeatherState saved)
- 00390: public void RestrictToNonHazardWeather(bool restrict)
- 00399: public List<WeatherForecastEntry> PeekForecast(int daysAhead = 3)
- 00459: private static WeatherKind ParseKind(string kind)
- 00464: private void RaiseChanged() => OnStateChanged?.Invoke(_state);
- 00468: private WeatherEffectsDef EffectsFor(WeatherKind kind)
- 00481: public class WeatherForecastEntry
- 00501: public static class WeatherProfileLoader
- 00505: public static SeasonProfileDef? Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: using Ashfall.Core.IO;
00007: namespace Ashfall.Core.World
00008: {
00009:     /// <summary>One seasonal weather window (the JSON is the authority).</summary>
00010:     [Serializable]
00011:     public class SeasonWindowDef
00012:     {
00013:         public string id = string.Empty;
00014:         public string displayName = string.Empty;
00015:         public int startDay = 0;
00016:         public float clearWeight = 1f;
00017:         public float rainWeight = 1f;
00018:         public float overcastWeight = 1f;
00019:         public float ashfallWeight = 1f;
00020:         public float falloutStormWeight = 1f;
00021:         public float blizzardWeight = 1f;
00022:         public float blackRainWeight = 1f;
00023:     }
00024:
00025:     /// <summary>The campaign weather profile (mirrors Unity SeasonProfile).</summary>
00026:     [Serializable]
00027:     public class SeasonProfileDef
00028:     {
00029:         public string id = "default_winter";
00030:         public string displayName = "The Long Winter";
00031:         public float weatherCheckIntervalHours = 6f;
00032:         public List<SeasonWindowDef> seasons = new List<SeasonWindowDef>();
00033:     }
00034:
00035:     /// <summary>Serialized weather state (save/load safe; rolls resume identically).</summary>
00036:     [Serializable]
00037:     public class WorldWeatherState
00038:     {
00039:         public string systemId = WeatherSystem.SystemId;
00040:         public string currentKind = "Clear";
00041:         public float totalElapsedHours = 0f;
00042:         public float hoursUntilNextCheck = 0f;
00043:         public int rollCount = 0;
00044:         public bool restrictToNonHazardWeather = false;
00045:
00046:         // ── Plan 205: deterministic surface wind (single weather authority) ──
00047:         public float wind_direction_deg = 0f;
00048:         public float wind_speed_kph = 0f;
00049:     }
00050:
00051:     /// <summary>
00052:     /// Engine-agnostic port of the Unity WeatherSystem (Assets/_Game/Environment/
00053:     /// WeatherSystem.cs): seeded weighted-random state transitions against the
00054:     /// active season window, deterministic for save/load (each roll reseeds fresh
00055:     /// from seed + rollCount instead of persisting RNG state), plus the weather
00056:     /// modifiers (visibility, outdoor rad, hazmat melt, temperature penalty).
00057:     /// </summary>
00058:     public class WeatherSystem
00059:     {
00060:         public const string SystemId = "world_weather_system";
00061:
00062:         public const float FalloutStormOutdoorRadModifier = 150f;
00063:         public const float BlackRainOutdoorRadModifier = 250f;
00064:         public const float BlackRainHazmatMeltMultiplier = 5f;
00065:         public const float BlizzardTemperaturePenaltyC = -15f;
00066:         public const float FalloutStormTemperaturePenaltyC = -5f;
00067:         public const float BlackRainTemperaturePenaltyC = -8f;
00068:         public const float BlizzardVisibilityFactor = 0.4f;
00069:
00070:         private readonly WorldWeatherState _state;
00071:         private SeasonProfileDef _profile;
00072:         private int _seed;
00073:         private WeatherEffectsCatalog? _effectsCatalog;
00074:
00075:         public event Action<WeatherKind> OnWeatherChanged;
00076:         public event Action<WorldWeatherState> OnStateChanged;
00077:
00078:         public WeatherSystem(WorldWeatherState? state = null)
00079:         {
00080:             _state = state ?? new WorldWeatherState();
00081:         }
00082:
00083:         public WorldWeatherState State => _state;
00084:         public WeatherKind Current => ParseKind(_state.currentKind);
00085:
00086:         /// <summary>
00087:         /// C2 / Plan 20A (G1) — binds the data-authored weather-effects
00088:         /// authority. When bound, OutdoorRadModifier and the forecast rad
00089:         /// projection read the one shared table (plan §57.3: forecast and
00090:         /// runtime can never drift). Unbound callers keep the legacy Core
00091:         /// constants byte-for-byte.
00092:         /// </summary>
00093:         public void BindWeatherEffects(WeatherEffectsCatalog? catalog)
00094:         {
00095:             _effectsCatalog = catalog;
00096:         }
00097:
00098:         // ── Plan 205: surface wind projections (degrees; kph) ──
00099:         public float WindDirectionDeg => _state.wind_direction_deg;
00100:         public float WindSpeedKph => _state.wind_speed_kph;
00101:         public int Seed => _seed;
00102:
00103:         /// <summary>Bound seasonal profile (read-only; null until a profile is bound).</summary>
00104:         public SeasonProfileDef? Profile => _profile;
00105:
00106:         // ── Profile ────────────────────────────────────────────────────
00107:
00108:         public void BindProfile(SeasonProfileDef profile, int seed)
00109:         {
00110:             _profile = profile ?? new SeasonProfileDef();
00111:             _seed = seed;
00112:         }
00113:
00114:         public SeasonWindowDef GetSeasonForDay(int day)
00115:         {
00116:             if (_profile == null || _profile.seasons == null || _profile.seasons.Count == 0)
00117:                 return DefaultWindow;
00118:             SeasonWindowDef? current = null;
00119:             for (int i = 0; i < _profile.seasons.Count; i++)
00120:             {
00121:                 if (_profile.seasons[i] != null && _profile.seasons[i].startDay <= day)
00122:                     current = _profile.seasons[i];
00123:             }
00124:             return current ?? DefaultWindow;
00125:         }
00126:
00127:         private static readonly SeasonWindowDef DefaultWindow = new SeasonWindowDef
00128:         {
00129:             id = "default",
00130:             displayName = "Default",
00131:             clearWeight = 1f,
00132:             rainWeight = 1f,
00133:             overcastWeight = 1f,
00134:             ashfallWeight = 1f,
00135:             falloutStormWeight = 1f,
00136:             blizzardWeight = 1f,
00137:             blackRainWeight = 0f // Unity parity: SeasonProfile.Default keeps Black Rain rare (0)
00138:         };
00139:
00140:         // ── Tick ───────────────────────────────────────────────────────
00141:
00142:         public void Tick(float gameHours)
00143:         {
00144:             if (_profile == null || gameHours <= 0f)
00145:                 return;
00146:
00147:             _state.totalElapsedHours += gameHours;
00148:             _state.hoursUntilNextCheck -= gameHours;
00149:
00150:             int safety = 0;
00151:             while (_state.hoursUntilNextCheck <= 0f && safety < 10000)
00152:             {
00153:                 _state.hoursUntilNextCheck += NextCheckInterval();
00154:                 SetCurrent(RollNextState());
00155:                 safety++;
00156:             }
00157:             RaiseChanged();
00158:         }
00159:
00160:         /// <summary>Force a specific weather state (debug / scripted events).</summary>
00161:         public void ForceWeather(WeatherKind kind)
00162:         {
00163:             _state.hoursUntilNextCheck = NextCheckInterval();
00164:             SetCurrent(kind);
00165:             RaiseChanged();
00166:         }
00167:
00168:         private WeatherKind RollNextState()
00169:         {
00170:             var season = GetSeasonForDay((int)Math.Floor(_state.totalElapsedHours / 24f));
00171:             var rng = new SeededRng(unchecked(_seed * 397 + _state.rollCount));
00172:             _state.rollCount++;
00173:
00174:             bool restrict = _state.restrictToNonHazardWeather;
00175:             float clear = Math.Max(0f, season.clearWeight);
00176:             float rain = Math.Max(0f, season.rainWeight);
00177:             float overcast = Math.Max(0f, season.overcastWeight);
00178:             float ashfall = restrict ? 0f : Math.Max(0f, season.ashfallWeight);
00179:             float storm = restrict ? 0f : Math.Max(0f, season.falloutStormWeight);
00180:             float blizzard = restrict ? 0f : Math.Max(0f, season.blizzardWeight);
00181:             float blackRain = restrict ? 0f : Math.Max(0f, season.blackRainWeight);
00182:             float total = clear + rain + overcast + ashfall + storm + blizzard + blackRain;
00183:             if (total <= 0f)
00184:                 return WeatherKind.Clear;
00185:
00186:             double roll = rng.NextDouble() * total;
00187:             var kind = RollKind(roll, clear, rain, overcast, ashfall, storm, blizzard, blackRain);
00188:
00189:             // Plan 205: wind accompanies every weather roll. Storm kinds run a
00190:             // higher base speed band; calm kinds stay light. Deterministic from
00191:             // the same seeded rng instance as the kind draw.
00192:             double windRoll = rng.NextDouble();
00193:             double speedRoll = rng.NextDouble();
00194:             bool severe = kind is WeatherKind.FalloutStorm or WeatherKind.Blizzard
00195:                 or WeatherKind.BlackRain or WeatherKind.AcidSnow
00196:                 or WeatherKind.BlackSnow or WeatherKind.BloodRain;
00197:             float baseSpeed = severe ? 18f : 6f;
00198:             float bandWidth = severe ? 22f : 10f;
00199:             _state.wind_direction_deg = (float)(windRoll * 360.0);
00200:             _state.wind_speed_kph = baseSpeed + (float)(speedRoll * bandWidth);
00201:
00202:             return kind;
00203:         }
00204:
00205:         private static WeatherKind RollKind(double roll, float clear, float rain, float overcast,
00206:             float ashfall, float storm, float blizzard, float blackRain)
00207:         {
00208:             if (roll < clear) return WeatherKind.Clear;
00209:             roll -= clear;
00210:             if (roll < rain) return WeatherKind.Rain;
00211:             roll -= rain;
00212:             if (roll < overcast) return WeatherKind.Overcast;
00213:             roll -= overcast;
00214:             if (roll < ashfall) return WeatherKind.Ashfall;
00215:             roll -= ashfall;
00216:             if (roll < storm) return WeatherKind.FalloutStorm;
00217:             roll -= storm;
00218:             if (roll < blizzard) return WeatherKind.Blizzard;
00219:             return WeatherKind.BlackRain;
00220:         }
00221:
00222:         private void SetCurrent(WeatherKind next)
00223:         {
00224:             if (next == Current)
00225:                 return;
00226:             _state.currentKind = next.ToString();
00227:             OnWeatherChanged?.Invoke(next);
00228:         }
00229:
00230:         private float NextCheckInterval()
00231:         {
00232:             return Math.Max(0.01f,
00233:                 _profile != null ? _profile.weatherCheckIntervalHours : 6f);
00234:         }
00235:
00236:         // ── Modifiers (Unity parity) ───────────────────────────────────
00237:
00238:         public float VisibilityFactor
00239:         {
00240:             get
00241:             {
00242:                 // C2 / Plan 20C — data authority first (table row); legacy
00243:                 // switch fallback when unbound.
00244:                 if (_effectsCatalog != null && _effectsCatalog.TryGetEffects(Current, out var effects)
00245:                     && effects != null)
00246:                     return Math.Clamp(effects.visibility_modifier, 0f, 1f);
00247:
00248:                 switch (Current)
00249:                 {
00250:                     case WeatherKind.FalloutStorm:
00251:                     case WeatherKind.BlackRain:
00252:                         return 0f;
00253:                     case WeatherKind.Blizzard:
00254:                         return BlizzardVisibilityFactor;
00255:                     default:
00256:                         return 1f;
00257:                 }
00258:             }
00259:         }
00260:
00261:         public float OutdoorRadModifier
00262:         {
00263:             get
00264:             {
00265:                 // C2 / Plan 20A (G1) — data authority first; the catalog is
00266:                 // validated to carry an explicit row for every WeatherKind.
00267:                 if (_effectsCatalog != null && _effectsCatalog.TryGetModifier(Current, out float fromData))
00268:                     return fromData;
00269:
00270:                 switch (Current)
00271:                 {
00272:                     case WeatherKind.BlackRain: return BlackRainOutdoorRadModifier;
00273:                     case WeatherKind.FalloutStorm: return FalloutStormOutdoorRadModifier;
00274:                     default: return 0f;
00275:                 }
00276:             }
00277:         }
00278:
00279:         /// <summary>C2 / Plan 20A (G1) — forecast rad projection for a kind,
00280:         /// from the same weather-effects table the runtime dose consumes.
00281:         /// Legacy fallback preserves the pre-catalog projection constants.</summary>
00282:         public float ForecastRadModifier(WeatherKind kind)
00283:         {
00284:             if (_effectsCatalog != null && _effectsCatalog.TryGetModifier(kind, out float fromData))
00285:                 return fromData;
00286:
00287:             return kind switch
00288:             {
00289:                 WeatherKind.BlackRain => BlackRainOutdoorRadModifier,
00290:                 WeatherKind.FalloutStorm => FalloutStormOutdoorRadModifier,
00291:                 WeatherKind.Ashfall => 45.0f,
00292:                 _ => 0f
00293:             };
00294:         }
00295:
00296:         public bool IsScavengingBlocked(bool hasFullSuit) =>
00297:             (Current == WeatherKind.FalloutStorm || Current == WeatherKind.BlackRain) && !hasFullSuit;
00298:
00299:         public float GetTemperaturePenaltyCelsius()
00300:         {
00301:             return Current switch
00302:             {
00303:                 WeatherKind.Blizzard => BlizzardTemperaturePenaltyC,
00304:                 WeatherKind.FalloutStorm => FalloutStormTemperaturePenaltyC,
00305:                 WeatherKind.BlackRain => BlackRainTemperaturePenaltyC,
00306:                 _ => 0f
00307:             };
00308:         }
00309:
00310:         public float HazmatDegradeMultiplier =>
00311:             Current == WeatherKind.BlackRain ? BlackRainHazmatMeltMultiplier : 1f;
00312:
00313:         public static float TemperaturePenaltyForWeather(WeatherKind kind)
00314:         {
00315:             switch (kind)
00316:             {
00317:                 case WeatherKind.Blizzard: return BlizzardTemperaturePenaltyC;
00318:                 case WeatherKind.FalloutStorm: return FalloutStormTemperaturePenaltyC;
00319:                 case WeatherKind.BlackRain: return BlackRainTemperaturePenaltyC;
00320:                 default: return 0f;
00321:             }
00322:         }
00323:
00324:         /// <summary>
00325:         /// C2 / Plan 20C (§42) — shelter thermal load delta from the bound
00326:         /// weather-effects table (data authority); falls back to the legacy
00327:         /// static constants when unbound. This is THE thermal coupling: cold
00328:         /// weather reaches heating/fuel pressure through the existing
00329:         /// temperature path — never a second fuel drain.
00330:         /// </summary>
00331:         public float TemperaturePenaltyC(WeatherKind kind)
00332:         {
00333:             if (_effectsCatalog != null && _effectsCatalog.TryGetEffects(kind, out var effects)
00334:                 && effects != null)
00335:                 return effects.thermal_load_additive_c;
00336:             return TemperaturePenaltyForWeather(kind);
00337:         }
00338:
00339:         /// <summary>
00340:         /// C2 / Plan 20C (§45) — visibility fraction 0..1 from the bound table;
00341:         /// legacy switch fallback when unbound (same values the forecast
00342:         /// projection previously hardcoded).
00343:         /// </summary>
00344:         public float VisibilityModifier(WeatherKind kind)
00345:         {
00346:             if (_effectsCatalog != null && _effectsCatalog.TryGetEffects(kind, out var effects)
00347:                 && effects != null)
00348:                 return Math.Clamp(effects.visibility_modifier, 0f, 1f);
00349:
00350:             return kind switch
00351:             {
00352:                 WeatherKind.FalloutStorm or WeatherKind.BlackRain => 0f,
00353:                 WeatherKind.Blizzard => BlizzardVisibilityFactor,
00354:                 WeatherKind.Ashfall => 0.65f,
00355:                 _ => 1f
00356:             };
00357:         }
00358:
00359:         // ── Save / Load ────────────────────────────────────────────────
00360:
00361:         public WorldWeatherState CaptureState()
00362:         {
00363:             return new WorldWeatherState
00364:             {
00365:                 systemId = _state.systemId,
00366:                 currentKind = _state.currentKind,
00367:                 totalElapsedHours = _state.totalElapsedHours,
00368:                 hoursUntilNextCheck = _state.hoursUntilNextCheck,
00369:                 rollCount = _state.rollCount,
00370:                 restrictToNonHazardWeather = _state.restrictToNonHazardWeather,
00371:                 wind_direction_deg = _state.wind_direction_deg,
00372:                 wind_speed_kph = _state.wind_speed_kph
00373:             };
00374:         }
00375:
00376:         public void RestoreState(WorldWeatherState saved)
00377:         {
00378:             if (saved == null) return;
00379:             _state.systemId = SystemId;
00380:             _state.currentKind = saved.currentKind;
00381:             _state.totalElapsedHours = Math.Max(0f, saved.totalElapsedHours);
00382:             _state.hoursUntilNextCheck = saved.hoursUntilNextCheck;
00383:             _state.rollCount = Math.Max(0, saved.rollCount);
00384:             _state.restrictToNonHazardWeather = saved.restrictToNonHazardWeather;
00385:             _state.wind_direction_deg = saved.wind_direction_deg;
00386:             _state.wind_speed_kph = saved.wind_speed_kph;
00387:             RaiseChanged();
00388:         }
00389:
00390:         public void RestrictToNonHazardWeather(bool restrict)
00391:         {
00392:             _state.restrictToNonHazardWeather = restrict;
00393:             RaiseChanged();
00394:         }
00395:
00396:         /// <summary>
00397:         /// Deterministically peeks upcoming forecast for N days ahead without mutating simulation roll count or RNG state.
00398:         /// </summary>
00399:         public List<WeatherForecastEntry> PeekForecast(int daysAhead = 3)
00400:         {
00401:             var list = new List<WeatherForecastEntry>();
00402:             int currentDay = (int)Math.Floor(_state.totalElapsedHours / 24f);
00403:
00404:             for (int i = 0; i < daysAhead; i++)
00405:             {
00406:                 int targetDay = currentDay + i;
00407:                 var season = GetSeasonForDay(targetDay);
00408:                 var rng = new SeededRng(unchecked(_seed * 397 + (_state.rollCount + i)));
00409:
00410:                 bool restrict = _state.restrictToNonHazardWeather;
00411:                 float clear = Math.Max(0f, season.clearWeight);
00412:                 float rain = Math.Max(0f, season.rainWeight);
00413:                 float overcast = Math.Max(0f, season.overcastWeight);
00414:                 float ashfall = restrict ? 0f : Math.Max(0f, season.ashfallWeight);
00415:                 float storm = restrict ? 0f : Math.Max(0f, season.falloutStormWeight);
00416:                 float blizzard = restrict ? 0f : Math.Max(0f, season.blizzardWeight);
00417:                 float blackRain = restrict ? 0f : Math.Max(0f, season.blackRainWeight);
00418:                 float total = clear + rain + overcast + ashfall + storm + blizzard + blackRain;
00419:
00420:                 WeatherKind predicted = WeatherKind.Clear;
00421:                 if (i == 0)
00422:                 {
00423:                     predicted = Current;
00424:                 }
00425:                 else if (total > 0f)
00426:                 {
00427:                     double roll = rng.NextDouble() * total;
00428:                     if (roll < clear) predicted = WeatherKind.Clear;
00429:                     else if ((roll -= clear) < rain) predicted = WeatherKind.Rain;
00430:                     else if ((roll -= rain) < overcast) predicted = WeatherKind.Overcast;
00431:                     else if ((roll -= overcast) < ashfall) predicted = WeatherKind.Ashfall;
00432:                     else if ((roll -= ashfall) < storm) predicted = WeatherKind.FalloutStorm;
00433:                     else if ((roll -= storm) < blizzard) predicted = WeatherKind.Blizzard;
00434:                     else predicted = WeatherKind.BlackRain;
00435:                 }
00436:
00437:                 float rad = ForecastRadModifier(predicted);
00438:
00439:                 float vis = VisibilityModifier(predicted);
00440:
00441:                 list.Add(new WeatherForecastEntry
00442:                 {
00443:                     Day = targetDay + 1,
00444:                     Kind = predicted,
00445:                     OutdoorRad = rad,
00446:                     Visibility = vis,
00447:                     Summary = predicted.ToString(),
00448:                     ThermalLoadC = TemperaturePenaltyC(predicted),
00449:                     TravelSpeedMultiplier = EffectsFor(predicted).travel_speed_multiplier,
00450:                     TravelEncounterMultiplier = EffectsFor(predicted).travel_encounter_multiplier,
00451:                     TrapYieldMultiplier = EffectsFor(predicted).trap_yield_multiplier,
00452:                     CaravanAvailabilityMultiplier = EffectsFor(predicted).caravan_availability_multiplier
00453:                 });
00454:             }
00455:
00456:             return list;
00457:         }
00458:
00459:         private static WeatherKind ParseKind(string kind)
00460:         {
00461:             return Enum.TryParse(kind, out WeatherKind parsed) ? parsed : WeatherKind.Clear;
00462:         }
00463:
00464:         private void RaiseChanged() => OnStateChanged?.Invoke(_state);
00465:
00466:         /// <summary>C2 / Plan 20C (§45) — the full effects row for a kind
00467:         /// (legacy-identity fallback when the catalog is unbound).</summary>
00468:         private WeatherEffectsDef EffectsFor(WeatherKind kind)
00469:         {
00470:             if (_effectsCatalog != null && _effectsCatalog.TryGetEffects(kind, out var fx)
00471:                 && fx != null)
00472:                 return fx;
00473:             return new WeatherEffectsDef();
00474:         }
00475:     }
00476:
00477:     /// <summary>
00478:     /// Deterministic daily weather forecast entry.
00479:     /// </summary>
00480:     [Serializable]
00481:     public class WeatherForecastEntry
00482:     {
00483:         public int Day;
00484:         public WeatherKind Kind;
00485:         public float OutdoorRad;
00486:         public float Visibility;
00487:         public string Summary = string.Empty;
00488:
00489:         // C2 / Plan 20C (§45) — decision-relevant effects from the ONE
00490:         // weather-effects table (populated in PeekForecast; legacy defaults
00491:         // when unbound: thermal from the static curve, multipliers neutral).
00492:         public float ThermalLoadC;
00493:         public float TravelSpeedMultiplier = 1f;
00494:         public float TravelEncounterMultiplier = 1f;
00495:         public float TrapYieldMultiplier = 1f;
00496:         public float CaravanAvailabilityMultiplier = 1f;
00497:     }
00498:
00499:
00500:     /// <summary>Engine-agnostic loader for weather_seasons.json.</summary>
00501:     public static class WeatherProfileLoader
00502:     {
00503:         public const string FileName = "weather_seasons.json";
00504:
00505:         public static SeasonProfileDef? Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
00506:         {
00507:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00508:                 return null;
00509:
00510:             string path = fileIO.Combine(dataDir, FileName);
00511:             if (!fileIO.FileExists(path))
00512:                 return null;
00513:
00514:             string raw = fileIO.ReadAllText(path);
00515:             if (string.IsNullOrWhiteSpace(raw))
00516:                 return null;
00517:
00518:             try
00519:             {
00520:                 var parsed = json.Deserialize<SeasonProfileDef>(raw);
00521:                 if (parsed != null && parsed.seasons == null)
00522:                     parsed.seasons = new List<SeasonWindowDef>();
00523:                 return parsed;
00524:             }
00525:             catch (Exception ex_CATDIAG)
00526:             {
00527:                 CatalogDiagnostics.Warn(path, "SeasonProfileDef", ex_CATDIAG);
00528:                 return null;
00529:             }
00530:         }
00531:     }
00532: }
```

## `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` — 399 lines; 15,401 bytes; SHA-256 `c1ae619827695fb1fcce2a847d9b08f8e282e03746217511995f71b945ccf5d0`
Declaration index:
- 00016: public interface ICampaignCalendar
- 00050: public sealed class CampaignCalendar : ICampaignCalendar
- 00070: public void BindProfile(Ashfall.Core.World.SeasonProfileDef? profile)
- 00075: public void SetDay(int day)
- 00092: public CampaignCalendarReadModel ResolveDay(int day)
- 00151: private static float CalculateBaselineAmbientTemperature(int dayInYear)
- 00227: public IClock AsClock() => new CalendarClockAdapter(this);
- 00229: public ISimClock AsSimClock() => new CalendarSimClockAdapter(this);
- 00233: public sealed class CalendarClockAdapter : IClock
- 00244: public void AdvanceDays(int days)
- 00250: public void SetDay(int day)
- 00257: public sealed class CalendarSimClockAdapter : ISimClock, IClock
- 00276: public void AdvanceTicks(long ticks)
- 00288: public void AdvanceHours(int hours)
- 00294: public void AdvanceDays(int days)
- 00300: public void SetDay(int day)
- 00311: public static class CampaignCalendarReconciler
- 00313: public sealed class MismatchRecord
- 00326: public string FormatLogMessage() =>
- 00330: public sealed class ReconciliationResult
- 00351: public static ReconciliationResult Reconcile(IReadOnlyDictionary<string, int>? sectionDays, ILog? log = null)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Clock;
00005:
00006: namespace Ashfall.Core.Campaign
00007: {
00008:     /// <summary>
00009:     /// ASHFALL — Authoritative Campaign Calendar.
00010:     ///
00011:     /// Single authority for the in-game campaign day (>= 1).
00012:     /// Distinguishes whole campaign days from sub-day simulation ticks (ISimClock)
00013:     /// and wall-clock time. Every other system clock (Holdfast, Duty Roster,
00014:     /// Verdict, Year of Ash, Economy) is an adapted projection of this authority.
00015:     /// </summary>
00016:     public interface ICampaignCalendar
00017:     {
00018:         /// <summary>The authoritative campaign day (>= 1).</summary>
00019:         int CurrentDay { get; }
00020:
00021:         /// <summary>Raised whenever the authoritative day changes.</summary>
00022:         event Action<int>? OnDayChanged;
00023:
00024:         /// <summary>
00025:         /// Update the authoritative campaign day.
00026:         /// Only the CampaignDayCoordinator and save restoration pipeline should call this.
00027:         /// </summary>
00028:         void SetDay(int day);
00029:
00030:         /// <summary>The current day's resolved calendar read model.</summary>
00031:         CampaignCalendarReadModel CurrentReadModel { get; }
00032:
00033:         /// <summary>Purely resolves time, seasonal context, and ambient baseline for any day without mutating state.</summary>
00034:         CampaignCalendarReadModel ResolveDay(int day);
00035:
00036:         /// <summary>Binds authored season profile definition (e.g. from weather_seasons.json).</summary>
00037:         void BindProfile(Ashfall.Core.World.SeasonProfileDef? profile);
00038:
00039:         /// <summary>Raised when advancing days crosses into a new season window (oldSeasonId, newSeasonId).</summary>
00040:         event Action<string, string>? OnSeasonChanged;
00041:
00042:         /// <summary>Exposes this calendar as an <see cref="IClock"/> projection.</summary>
00043:         IClock AsClock();
00044:
00045:         /// <summary>Exposes this calendar as an <see cref="ISimClock"/> projection.</summary>
00046:         ISimClock AsSimClock();
00047:     }
00048:
00049:     /// <summary>Default concrete implementation of <see cref="ICampaignCalendar"/>.</summary>
00050:     public sealed class CampaignCalendar : ICampaignCalendar
00051:     {
00052:         public const int DaysPerYear = 365;
00053:
00054:         private int _currentDay;
00055:         private Ashfall.Core.World.SeasonProfileDef? _profile;
00056:
00057:         public int CurrentDay => _currentDay;
00058:
00059:         public CampaignCalendarReadModel CurrentReadModel => ResolveDay(_currentDay);
00060:
00061:         public event Action<int>? OnDayChanged;
00062:         public event Action<string, string>? OnSeasonChanged;
00063:
00064:         public CampaignCalendar(int initialDay = 1, Ashfall.Core.World.SeasonProfileDef? profile = null)
00065:         {
00066:             _currentDay = Math.Max(1, initialDay);
00067:             _profile = profile;
00068:         }
00069:
00070:         public void BindProfile(Ashfall.Core.World.SeasonProfileDef? profile)
00071:         {
00072:             _profile = profile;
00073:         }
00074:
00075:         public void SetDay(int day)
00076:         {
00077:             if (day < 1)
00078:                 throw new ArgumentOutOfRangeException(nameof(day), "Campaign day must be >= 1");
00079:             if (day == _currentDay) return;
00080:
00081:             string oldSeasonId = ResolveDay(_currentDay).SeasonId;
00082:             _currentDay = day;
00083:             string newSeasonId = ResolveDay(_currentDay).SeasonId;
00084:
00085:             OnDayChanged?.Invoke(_currentDay);
00086:             if (!string.Equals(oldSeasonId, newSeasonId, StringComparison.Ordinal))
00087:             {
00088:                 OnSeasonChanged?.Invoke(oldSeasonId, newSeasonId);
00089:             }
00090:         }
00091:
00092:         public CampaignCalendarReadModel ResolveDay(int day)
00093:         {
00094:             if (day < 1) day = 1;
00095:
00096:             int dayInYear = ((day - 1) % DaysPerYear) + 1;
00097:             int year = ((day - 1) / DaysPerYear) + 1;
00098:             int chapter = year;
00099:
00100:             var seasons = _profile?.seasons;
00101:             if (seasons == null || seasons.Count == 0)
00102:             {
00103:                 seasons = DefaultProfileSeasons;
00104:             }
00105:
00106:             // Find matching window by largest startDay <= dayInYear (seasons[0] startDay 0 covers day 1)
00107:             int matchIndex = 0;
00108:             for (int i = 0; i < seasons.Count; i++)
00109:             {
00110:                 if (seasons[i] != null && seasons[i].startDay <= dayInYear)
00111:                 {
00112:                     matchIndex = i;
00113:                 }
00114:             }
00115:
00116:             var currentWindow = seasons[matchIndex] ?? DefaultProfileSeasons[0];
00117:             int nextStartDay = (matchIndex + 1 < seasons.Count && seasons[matchIndex + 1] != null)
00118:                 ? seasons[matchIndex + 1].startDay
00119:                 : DaysPerYear + 1;
00120:
00121:             int startDay = Math.Max(1, currentWindow.startDay);
00122:             int daysIntoSeason = dayInYear - startDay + 1;
00123:             int duration = Math.Max(1, nextStartDay - startDay);
00124:             int daysToSeasonEnd = Math.Max(0, nextStartDay - 1 - dayInYear);
00125:             float progress = Math.Clamp((float)daysIntoSeason / duration, 0f, 1f);
00126:
00127:             float ambientTemp = CalculateBaselineAmbientTemperature(dayInYear);
00128:             float severity = Math.Clamp((10.0f - ambientTemp) / 55.0f, 0.10f, 1.0f);
00129:             float dayLength = Math.Clamp(10.0f + (ambientTemp / 10.0f) * 2.0f, 6.0f, 16.0f);
00130:             float preservationBias = Math.Clamp(1.0f - (ambientTemp * 0.015f), 0.70f, 1.60f);
00131:             float migrationBias = Math.Clamp(1.0f + (ambientTemp * 0.02f), 0.30f, 1.40f);
00132:
00133:             return new CampaignCalendarReadModel(
00134:                 day: day,
00135:                 seasonId: currentWindow.id,
00136:                 seasonDisplayName: currentWindow.displayName,
00137:                 seasonIndex: matchIndex,
00138:                 seasonProgress: progress,
00139:                 daysIntoSeason: daysIntoSeason,
00140:                 daysToSeasonEnd: daysToSeasonEnd,
00141:                 year: year,
00142:                 chapter: chapter,
00143:                 ambientTemperatureC: ambientTemp,
00144:                 seasonalSeverity: severity,
00145:                 dayLengthHours: dayLength,
00146:                 migrationBias: migrationBias,
00147:                 preservationBias: preservationBias
00148:             );
00149:         }
00150:
00151:         private static float CalculateBaselineAmbientTemperature(int dayInYear)
00152:         {
00153:             if (dayInYear <= 29)
00154:             {
00155:                 // First Thaw: -2C rising to +3C
00156:                 float t = (dayInYear - 1) / 29.0f;
00157:                 return -2.0f + (5.0f * t);
00158:             }
00159:             if (dayInYear <= 59)
00160:             {
00161:                 // Ash Settling: +3C dropping to -10C
00162:                 float t = (dayInYear - 30) / 30.0f;
00163:                 return 3.0f - (13.0f * t);
00164:             }
00165:             if (dayInYear <= 89)
00166:             {
00167:                 // The Deep Freeze: -10C dropping to -25C
00168:                 float t = (dayInYear - 60) / 30.0f;
00169:                 return -10.0f - (15.0f * t);
00170:             }
00171:             if (dayInYear <= 119)
00172:             {
00173:                 // Spring Storms: -25C rising to +2C
00174:                 float t = (dayInYear - 90) / 30.0f;
00175:                 return -25.0f + (27.0f * t);
00176:             }
00177:             if (dayInYear <= 149)
00178:             {
00179:                 // Dry Ash: +2C to +8C back to +3C
00180:                 float t = (dayInYear - 120) / 30.0f;
00181:                 return 2.0f + (6.0f * (float)Math.Sin(t * Math.PI));
00182:             }
00183:             if (dayInYear <= 179)
00184:             {
00185:                 // First Fallout: +3C dropping to -25C
00186:                 float t = (dayInYear - 150) / 30.0f;
00187:                 return 3.0f - (28.0f * t);
00188:             }
00189:             if (dayInYear <= 240)
00190:             {
00191:                 // Year of Ash Phase 4 (Deep Freeze): -25C to -45C at day 210, then -30C
00192:                 float t = (dayInYear - 180) / 60.0f;
00193:                 return -25.0f - (20.0f * (float)Math.Sin(t * Math.PI));
00194:             }
00195:             if (dayInYear <= 300)
00196:             {
00197:                 // Year of Ash Phase 5 (Faction Siege): -30C to -10C
00198:                 float t = (dayInYear - 240) / 60.0f;
00199:                 return -30.0f + (20.0f * t);
00200:             }
00201:             if (dayInYear <= 360)
00202:             {
00203:                 // Year of Ash Phase 6 (Great Thaw): -10C to +4C
00204:                 float t = (dayInYear - 300) / 60.0f;
00205:                 return -10.0f + (14.0f * t);
00206:             }
00207:
00208:             // Days 361-365: glide from +4C to -2C into next year's First Thaw
00209:             float endT = (dayInYear - 360) / 5.0f;
00210:             return 4.0f - (6.0f * endT);
00211:         }
00212:
00213:         private static readonly List<Ashfall.Core.World.SeasonWindowDef> DefaultProfileSeasons = new List<Ashfall.Core.World.SeasonWindowDef>
00214:         {
00215:             new() { id = "window_first_thaw", displayName = "First Thaw", startDay = 0 },
00216:             new() { id = "window_ash_settling", displayName = "Ash Settling", startDay = 30 },
00217:             new() { id = "window_deep_freeze", displayName = "The Deep Freeze", startDay = 60 },
00218:             new() { id = "window_spring_storms", displayName = "Spring Storms", startDay = 90 },
00219:             new() { id = "window_dry_ash", displayName = "Dry Ash", startDay = 120 },
00220:             new() { id = "window_first_fallout", displayName = "First Fallout", startDay = 150 },
00221:             new() { id = "window_false_spring", displayName = "False Spring", startDay = 180 },
00222:             new() { id = "window_deep_ash", displayName = "Deep Ash", startDay = 200 },
00223:             new() { id = "window_long_winter", displayName = "The Long Winter", startDay = 240 },
00224:             new() { id = "window_black_rain_season", displayName = "Black Rain Season", startDay = 280 }
00225:         };
00226:
00227:         public IClock AsClock() => new CalendarClockAdapter(this);
00228:
00229:         public ISimClock AsSimClock() => new CalendarSimClockAdapter(this);
00230:     }
00231:
00232:     /// <summary>Projects <see cref="ICampaignCalendar"/> to the historical <see cref="IClock"/> port.</summary>
00233:     public sealed class CalendarClockAdapter : IClock
00234:     {
00235:         private readonly ICampaignCalendar _calendar;
00236:
00237:         public CalendarClockAdapter(ICampaignCalendar calendar)
00238:         {
00239:             _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
00240:         }
00241:
00242:         public int Day => _calendar.CurrentDay;
00243:
00244:         public void AdvanceDays(int days)
00245:         {
00246:             if (days <= 0) return;
00247:             _calendar.SetDay(_calendar.CurrentDay + days);
00248:         }
00249:
00250:         public void SetDay(int day)
00251:         {
00252:             _calendar.SetDay(day);
00253:         }
00254:     }
00255:
00256:     /// <summary>Projects <see cref="ICampaignCalendar"/> to the <see cref="ISimClock"/> intraday clock.</summary>
00257:     public sealed class CalendarSimClockAdapter : ISimClock, IClock
00258:     {
00259:         public const long TicksPerHour = 60;
00260:         public const long TicksPerDay = TicksPerHour * 24;
00261:
00262:         private readonly ICampaignCalendar _calendar;
00263:         private long _intradayTicks;
00264:
00265:         public CalendarSimClockAdapter(ICampaignCalendar calendar)
00266:         {
00267:             _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
00268:         }
00269:
00270:         public long CurrentTick => ((long)_calendar.CurrentDay * TicksPerDay) + _intradayTicks;
00271:
00272:         public int DayIndex => _calendar.CurrentDay;
00273:         public int Day => _calendar.CurrentDay;
00274:         public int HourOfDay => (int)((_intradayTicks % TicksPerDay) / TicksPerHour);
00275:
00276:         public void AdvanceTicks(long ticks)
00277:         {
00278:             if (ticks <= 0) return;
00279:             _intradayTicks += ticks;
00280:             if (_intradayTicks >= TicksPerDay)
00281:             {
00282:                 int daysToAdd = (int)(_intradayTicks / TicksPerDay);
00283:                 _intradayTicks %= TicksPerDay;
00284:                 _calendar.SetDay(_calendar.CurrentDay + daysToAdd);
00285:             }
00286:         }
00287:
00288:         public void AdvanceHours(int hours)
00289:         {
00290:             if (hours <= 0) return;
00291:             AdvanceTicks(hours * TicksPerHour);
00292:         }
00293:
00294:         public void AdvanceDays(int days)
00295:         {
00296:             if (days <= 0) return;
00297:             _calendar.SetDay(_calendar.CurrentDay + days);
00298:         }
00299:
00300:         public void SetDay(int day)
00301:         {
00302:             _calendar.SetDay(day);
00303:             _intradayTicks = 0;
00304:         }
00305:     }
00306:
00307:     /// <summary>
00308:     /// Reconciles conflicting day values stored across legacy or independent save sections.
00309:     /// Identifies the authoritative campaign day and surfaces structured diagnostics for mismatches.
00310:     /// </summary>
00311:     public static class CampaignCalendarReconciler
00312:     {
00313:         public sealed class MismatchRecord
00314:         {
00315:             public string SectionName { get; }
00316:             public int SectionDay { get; }
00317:             public int AuthoritativeDay { get; }
00318:
00319:             public MismatchRecord(string sectionName, int sectionDay, int authoritativeDay)
00320:             {
00321:                 SectionName = sectionName;
00322:                 SectionDay = sectionDay;
00323:                 AuthoritativeDay = authoritativeDay;
00324:             }
00325:
00326:             public string FormatLogMessage() =>
00327:                 $"[CALENDAR_MISMATCH] section='{SectionName}' section_day={SectionDay} authoritative_day={AuthoritativeDay}";
00328:         }
00329:
00330:         public sealed class ReconciliationResult
00331:         {
00332:             public int AuthoritativeDay { get; }
00333:             public string PrimarySource { get; }
00334:             public IReadOnlyList<MismatchRecord> Mismatches { get; }
00335:             public bool HasMismatches => Mismatches.Count > 0;
00336:
00337:             public ReconciliationResult(int authoritativeDay, string primarySource, IReadOnlyList<MismatchRecord> mismatches)
00338:             {
00339:                 AuthoritativeDay = Math.Max(1, authoritativeDay);
00340:                 PrimarySource = primarySource;
00341:                 Mismatches = mismatches ?? Array.Empty<MismatchRecord>();
00342:             }
00343:         }
00344:
00345:         /// <summary>
00346:         /// Reconciles day values collected from save sections.
00347:         /// Priority:
00348:         /// 1. campaign_day section if > 0.
00349:         /// 2. Max of (holdfast, year_of_ash, duty_roster, memorial, 1).
00350:         /// </summary>
00351:         public static ReconciliationResult Reconcile(IReadOnlyDictionary<string, int>? sectionDays, ILog? log = null)
00352:         {
00353:             if (sectionDays == null || sectionDays.Count == 0)
00354:             {
00355:                 return new ReconciliationResult(1, "default", Array.Empty<MismatchRecord>());
00356:             }
00357:
00358:             int authDay = 1;
00359:             string source = "fallback";
00360:
00361:             if (sectionDays.TryGetValue("campaign_day", out int campDay) && campDay > 0)
00362:             {
00363:                 authDay = campDay;
00364:                 source = "campaign_day";
00365:             }
00366:             else if (sectionDays.TryGetValue("holdfast", out int holdfastDay) && holdfastDay > 0)
00367:             {
00368:                 authDay = holdfastDay;
00369:                 source = "holdfast";
00370:             }
00371:             else
00372:             {
00373:                 int maxDay = 1;
00374:                 foreach (var kv in sectionDays)
00375:                 {
00376:                     if (kv.Value > maxDay)
00377:                     {
00378:                         maxDay = kv.Value;
00379:                         source = kv.Key;
00380:                     }
00381:                 }
00382:                 authDay = maxDay;
00383:             }
00384:
00385:             var mismatches = new List<MismatchRecord>();
00386:             foreach (var kv in sectionDays)
00387:             {
00388:                 if (kv.Value > 0 && kv.Value != authDay)
00389:                 {
00390:                     var rec = new MismatchRecord(kv.Key, kv.Value, authDay);
00391:                     mismatches.Add(rec);
00392:                     log?.Warn(rec.FormatLogMessage());
00393:                 }
00394:             }
00395:
00396:             return new ReconciliationResult(authDay, source, mismatches);
00397:         }
00398:     }
00399: }
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

## `Assets/Ashfall.Core/World/WeatherEffectsCatalog.cs` — 198 lines; 9,111 bytes; SHA-256 `ee099069ac05d216b26fdacc5f3170d755b6f66233d7abda21b15f1cd317b436`
Declaration index:
- 00016: public sealed class WeatherEffectsDef
- 00054: public sealed class WeatherEffectsCatalog
- 00145: private static bool ValidateRange(float value, float min, float max)
- 00149: public bool TryGetModifier(WeatherKind kind, out float modifier)
- 00156: public bool TryGetEffects(WeatherKind kind, out WeatherEffectsDef? effects)
- 00165: /// missing an explicit data row, in enum order. Empty means the catalog
- 00169: public List<WeatherKind> MissingKinds()
- 00178: public static WeatherEffectsCatalog LoadFromDirectory(string dataDir, IFileIO fileIO)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Ashfall.Core.IO;
00006:
00007: namespace Ashfall.Core.World
00008: {
00009:     /// <summary>
00010:     /// One authored row of the C2 / Plan 20A weather-effects authority
00011:     /// (weather_effects.json). Field names are snake_case per data policy.
00012:     /// 20A consumes only <c>outdoor_rad_modifier</c>; 20C extends this record
00013:     /// with travel/trap/caravan/thermal/visibility fields behind the same
00014:     /// schema, so 20A does not create a shape 20C must discard (plan §11.3).
00015:     /// </summary>
00016:     public sealed class WeatherEffectsDef
00017:     {
00018:         public string weather { get; set; } = string.Empty;
00019:         public float outdoor_rad_modifier { get; set; }
00020:
00021:         /// <summary>Line-of-sight fraction 0..1 (forecast/map display, foraging).</summary>
00022:         public float visibility_modifier { get; set; } = 1.0f;
00023:
00024:         /// <summary>Shelter thermal load delta in °C (Plan 20C §42 — consumed
00025:         /// through WeatherSystem's temperature penalty, never a second drain).</summary>
00026:         public float thermal_load_additive_c { get; set; }
00027:
00028:         /// <summary>Expedition travel speed × (1 = unaffected).</summary>
00029:         public float travel_speed_multiplier { get; set; } = 1.0f;
00030:
00031:         /// <summary>Expedition encounter chance × (1 = unaffected).</summary>
00032:         public float travel_encounter_multiplier { get; set; } = 1.0f;
00033:
00034:         /// <summary>Trapping/foraging yield × (1 = unaffected).</summary>
00035:         public float trap_yield_multiplier { get; set; } = 1.0f;
00036:
00037:         /// <summary>Caravan availability weight × (1 = unaffected).</summary>
00038:         public float caravan_availability_multiplier { get; set; } = 1.0f;
00039:
00040:         /// <summary>Plan 20C §44 — explicit mechanical-neutrality classification:
00041:         /// a row with all-identity effects must declare itself neutral rather
00042:         /// than silently defaulting; the coverage gate enforces the pairing.</summary>
00043:         public bool explicitly_neutral { get; set; }
00044:     }
00045:
00046:     /// <summary>
00047:     /// Data authority for per-weather-kind mechanical effects. Every
00048:     /// <see cref="WeatherKind"/> value must have an explicit entry — a new enum
00049:     /// value without a data row is a load error (no silent identity defaults,
00050:     /// plan §35.1). Consumers (WeatherSystem.OutdoorRadModifier and the
00051:     /// forecast projection) share this one table so a weather multiplier can
00052:     /// never drift between forecast and runtime (plan §57.3).
00053:     /// </summary>
00054:     public sealed class WeatherEffectsCatalog
00055:     {
00056:         public const string DefaultFileName = "weather_effects.json";
00057:
00058:         private readonly List<WeatherEffectsDef> _rows;
00059:         private readonly Dictionary<WeatherKind, float> _modifiers;
00060:         private readonly Dictionary<WeatherKind, WeatherEffectsDef>? _rowsByKind;
00061:         private readonly List<string> _errors;
00062:
00063:         public WeatherEffectsCatalog(IEnumerable<WeatherEffectsDef>? rows)
00064:         {
00065:             _rows = rows?.Where(r => r != null).ToList() ?? new List<WeatherEffectsDef>();
00066:             _modifiers = new Dictionary<WeatherKind, float>();
00067:             _errors = new List<string>();
00068:             _rowsByKind = new Dictionary<WeatherKind, WeatherEffectsDef>();
00069:
00070:             foreach (var row in _rows)
00071:             {
00072:                 if (string.IsNullOrWhiteSpace(row.weather))
00073:                 {
00074:                     _errors.Add("weather_effects row with empty 'weather' id");
00075:                     continue;
00076:                 }
00077:
00078:                 if (!Enum.TryParse<WeatherKind>(row.weather, ignoreCase: true, out var kind)
00079:                     || !Enum.IsDefined(typeof(WeatherKind), kind))
00080:                 {
00081:                     _errors.Add($"weather_effects: unknown weather kind '{row.weather}'");
00082:                     continue;
00083:                 }
00084:
00085:                 if (float.IsNaN(row.outdoor_rad_modifier) || float.IsInfinity(row.outdoor_rad_modifier))
00086:                 {
00087:                     _errors.Add($"weather_effects[{row.weather}]: outdoor_rad_modifier must be finite");
00088:                     continue;
00089:                 }
00090:
00091:                 if (row.outdoor_rad_modifier < 0f)
00092:                 {
00093:                     _errors.Add($"weather_effects[{row.weather}]: outdoor_rad_modifier must be non-negative");
00094:                     continue;
00095:                 }
00096:
00097:                 // C2 / Plan 20C (§35/§44) — validate the full effects record.
00098:                 if (!ValidateRange(row.visibility_modifier, 0f, 1f)
00099:                     || !ValidateRange(row.travel_speed_multiplier, 0f, 5f)
00100:                     || !ValidateRange(row.travel_encounter_multiplier, 0f, 5f)
00101:                     || !ValidateRange(row.trap_yield_multiplier, 0f, 5f)
00102:                     || !ValidateRange(row.caravan_availability_multiplier, 0f, 5f))
00103:                 {
00104:                     _errors.Add($"weather_effects[{row.weather}]: effect multiplier out of range");
00105:                     continue;
00106:                 }
00107:                 if (float.IsNaN(row.thermal_load_additive_c)
00108:                     || MathF.Abs(row.thermal_load_additive_c) > 60f)
00109:                 {
00110:                     _errors.Add($"weather_effects[{row.weather}]: thermal_load_additive_c out of range [-60,60]°C");
00111:                     continue;
00112:                 }
00113:
00114:                 bool anyIdentity = row.outdoor_rad_modifier != 0f
00115:                     || row.visibility_modifier != 1.0f
00116:                     || row.thermal_load_additive_c != 0f
00117:                     || row.travel_speed_multiplier != 1.0f
00118:                     || row.travel_encounter_multiplier != 1.0f
00119:                     || row.trap_yield_multiplier != 1.0f
00120:                     || row.caravan_availability_multiplier != 1.0f;
00121:                 if (anyIdentity == row.explicitly_neutral)
00122:                 {
00123:                     // Neutral must be all-identity; non-neutral must have ≥1 effect.
00124:                     _errors.Add(row.explicitly_neutral
00125:                         ? $"weather_effects[{row.weather}]: declared explicitly_neutral but has mechanical effects"
00126:                         : $"weather_effects[{row.weather}]: all-identity row must declare explicitly_neutral (no silent defaults)");
00127:                     continue;
00128:                 }
00129:
00130:                 if (_modifiers.ContainsKey(kind))
00131:                 {
00132:                     _errors.Add($"weather_effects: duplicate row for weather kind '{row.weather}'");
00133:                     continue;
00134:                 }
00135:
00136:                 _modifiers[kind] = row.outdoor_rad_modifier;
00137:                 if (_rowsByKind != null) _rowsByKind[kind] = row;
00138:             }
00139:         }
00140:
00141:         public IReadOnlyList<string> Errors => _errors;
00142:         public IReadOnlyList<WeatherEffectsDef> Rows => _rows;
00143:         public int LoadedCount => _modifiers.Count;
00144:
00145:         private static bool ValidateRange(float value, float min, float max)
00146:             => !float.IsNaN(value) && !float.IsInfinity(value) && value >= min && value <= max;
00147:
00148:         /// <summary>O(1) modifier lookup. True only for a validated, explicit row.</summary>
00149:         public bool TryGetModifier(WeatherKind kind, out float modifier)
00150:         {
00151:             return _modifiers.TryGetValue(kind, out modifier);
00152:         }
00153:
00154:         /// <summary>O(1) full-row lookup (Plan 20C consumers: travel, trapping,
00155:         /// caravan, thermal, forecast UI). True only for a validated row.</summary>
00156:         public bool TryGetEffects(WeatherKind kind, out WeatherEffectsDef? effects)
00157:         {
00158:             effects = null;
00159:             if (_rowsByKind == null) return false;
00160:             return _rowsByKind.TryGetValue(kind, out effects);
00161:         }
00162:
00163:         /// <summary>
00164:         /// Table-driven no-silent-defaults gate support: every WeatherKind value
00165:         /// missing an explicit data row, in enum order. Empty means the catalog
00166:         /// is complete; the integrity validator and tests fail loudly on any
00167:         /// remainder (plan §35.1 / §44).
00168:         /// </summary>
00169:         public List<WeatherKind> MissingKinds()
00170:         {
00171:             var missing = new List<WeatherKind>();
00172:             foreach (WeatherKind kind in Enum.GetValues(typeof(WeatherKind)))
00173:                 if (!_modifiers.ContainsKey(kind))
00174:                     missing.Add(kind);
00175:             return missing;
00176:         }
00177:
00178:         public static WeatherEffectsCatalog LoadFromDirectory(string dataDir, IFileIO fileIO)
00179:         {
00180:             if (fileIO == null || string.IsNullOrEmpty(dataDir))
00181:                 return new WeatherEffectsCatalog(null);
00182:             string path = fileIO.Combine(dataDir, DefaultFileName);
00183:             if (!fileIO.FileExists(path))
00184:                 return new WeatherEffectsCatalog(null);
00185:             try
00186:             {
00187:                 var rows = CatalogLocator.LoadWrappedList<WeatherEffectsDef>(
00188:                     fileIO.ReadAllText(path), SystemTextJsonSerializer.Options);
00189:                 return new WeatherEffectsCatalog(rows);
00190:             }
00191:             catch (Exception ex)
00192:             {
00193:                 CatalogDiagnostics.Warn("WeatherEffectsCatalog", path, ex);
00194:                 return new WeatherEffectsCatalog(null);
00195:             }
00196:         }
00197:     }
00198: }
```

## `Assets/Ashfall.Core/WildlifeSeasonalCalendar.cs` — 306 lines; 16,604 bytes; SHA-256 `9d760fd3e7c6a64c8fcd5b368284f08d8db9b6b7e4c149aba4c3f42eec92dfd2`
Declaration index:
- 00013: public enum MigrationArchetype
- 00041: public static class WildlifeSeasonalCalendar
- 00077: public static MigrationArchetype ArchetypeOf(string speciesId) => speciesId switch
- 00099: public static SeasonWindowDef SeasonWindowForDay(SeasonProfileDef? profile, int day)
- 00113: public static float HungerFactor(SeasonWindowDef? season, MigrationArchetype archetype)
- 00170: public static float AbundanceFactor(SeasonWindowDef? season, MigrationArchetype archetype)
- 00223: public static float SectorAbundanceFactor(
- 00245: public static List<string> FilterNeighbors(
- 00268: public static string? FieldGuideEntryFor(string speciesId) => speciesId switch
- 00284: public static string? MigrationNotice(
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.World;
00005:
00006: namespace Ashfall.Core
00007: {
00008:     /// <summary>
00009:     /// Ecological role a seeded species plays across the annual cycle.
00010:     /// Archetypes are assigned per species id (never per pack), so a species
00011:     /// always behaves as one biological population pattern.
00012:     /// </summary>
00013:     public enum MigrationArchetype
00014:     {
00015:         /// <summary>Year-round holder; hunger tracks forage and winter cold, not season windows.</summary>
00016:         Resident,
00017:         /// <summary>Large grazing herd; winter range collapse, thaw recovery, rut movement late in the year.</summary>
00018:         HerdGrazer,
00019:         /// <summary>Burrowing prey that booms on grain and forage, then seeks warm ground in the freeze.</summary>
00020:         BurrowSwarm,
00021:         /// <summary>Omnivorous sounder; peaks on the mast fall in The Turning.</summary>
00022:         Sounder,
00023:         /// <summary>Passage birds; movement windows in thaw and the turning, thin in deep cold.</summary>
00024:         PassageFlock,
00025:         /// <summary>Water-bound runner; the fish run is a thaw-to-bloom population boom in the waterway pair.</summary>
00026:         CoastalRunner,
00027:         /// <summary>Warm-damp insect bloom; near-absent in hard cold, crop pressure in the bloom window.</summary>
00028:         SwarmBlight
00029:     }
00030:
00031:     /// <summary>
00032:     /// Plan 28 — deterministic seasonal ecology calendar.
00033:     ///
00034:     /// Pure functions only: the Plan 19 weather-season windows
00035:     /// (<see cref="SeasonProfileDef"/> from weather_seasons.json) remain the
00036:     /// sole season authority, and this calendar never holds campaign state.
00037:     /// Archetypes modulate the existing hunger/birth/movement rules inside
00038:     /// <see cref="WildlifeMigrationSystem.TickDay(int, ISeededRng?)"/> — no new
00039:     /// population simulator, no wall-clock, no RNG of its own.
00040:     /// </summary>
00041:     public static class WildlifeSeasonalCalendar
00042:     {
00043:         // ── Season window ids (weather_seasons.json is the authority) ───
00044:         // Plan 83 expanded the Plan 19 six-phase model to ten windows.
00045:         // Old → new migration: window_ashfall→window_ash_settling,
00046:         // window_thaw→window_spring_storms, window_black_bloom→window_dry_ash,
00047:         // window_high_cold→window_long_winter, window_the_turning→window_false_spring;
00048:         // window_deep_freeze kept its id.
00049:         public const string SeasonFirstThaw = "window_first_thaw";
00050:         public const string SeasonAshSettling = "window_ash_settling";
00051:         public const string SeasonAshfall = SeasonAshSettling;          // legacy Plan 19 name
00052:         public const string SeasonDeepFreeze = "window_deep_freeze";
00053:         public const string SeasonThaw = "window_spring_storms";        // legacy Plan 19 name (wet interval)
00054:         public const string SeasonSpringStorms = SeasonThaw;
00055:         public const string SeasonDryAsh = "window_dry_ash";
00056:         public const string SeasonFirstFallout = "window_first_fallout";
00057:         public const string SeasonFalseSpring = "window_false_spring";
00058:         public const string SeasonDeepAsh = "window_deep_ash";
00059:         public const string SeasonLongWinter = "window_long_winter";
00060:         public const string SeasonBlackRainSeason = "window_black_rain_season";
00061:         public const string SeasonBlackBloom = SeasonDryAsh;            // legacy Plan 19 name
00062:         public const string SeasonHighCold = SeasonLongWinter;          // legacy Plan 19 name
00063:         public const string SeasonTheTurning = SeasonFalseSpring;       // legacy Plan 19 name
00064:
00065:         /// <summary>Hunger factor bounds. 1.0 keeps the authored +0.05/day cadence.</summary>
00066:         public const float HungerFactorMin = 0.6f;
00067:         public const float HungerFactorMax = 1.5f;
00068:
00069:         /// <summary>Abundance (presence) factor bounds feeding trapping density and notices.</summary>
00070:         public const float AbundanceFactorMin = 0.2f;
00071:         public const float AbundanceFactorMax = 1.5f;
00072:
00073:         /// <summary>
00074:         /// Species → archetype table for the seeded fauna. Species absent from
00075:         /// the table read as <see cref="MigrationArchetype.Resident"/>.
00076:         /// </summary>
00077:         public static MigrationArchetype ArchetypeOf(string speciesId) => speciesId switch
00078:         {
00079:             "species_rad_dog" => MigrationArchetype.Resident,
00080:             "species_wolf" => MigrationArchetype.Resident,
00081:             "species_dust_lynx" => MigrationArchetype.Resident,
00082:             "species_feral_goat" => MigrationArchetype.HerdGrazer,
00083:             "species_blight_rat" => MigrationArchetype.BurrowSwarm,
00084:             "species_ash_boar" => MigrationArchetype.Sounder,
00085:             "species_iron_crow" => MigrationArchetype.PassageFlock,
00086:             "species_ash_gull" => MigrationArchetype.PassageFlock,
00087:             "species_cotton_hare" => MigrationArchetype.BurrowSwarm,
00088:             "species_gray_heron" => MigrationArchetype.CoastalRunner,
00089:             "species_mirror_carp" => MigrationArchetype.CoastalRunner,
00090:             "species_ghost_moth" => MigrationArchetype.SwarmBlight,
00091:             _ => MigrationArchetype.Resident
00092:         };
00093:
00094:         /// <summary>
00095:         /// Season window for a campaign day from the Plan 19 profile; mirrors
00096:         /// <see cref="WeatherSystem.GetSeasonForDay"/> (last window whose
00097:         /// startDay ≤ day). Pure and save-neutral.
00098:         /// </summary>
00099:         public static SeasonWindowDef SeasonWindowForDay(SeasonProfileDef? profile, int day)
00100:         {
00101:             if (profile?.seasons == null || profile.seasons.Count == 0) return null!;
00102:             SeasonWindowDef? current = null;
00103:             for (int i = 0; i < profile.seasons.Count; i++)
00104:             {
00105:                 var s = profile.seasons[i];
00106:                 if (s != null && s.startDay <= day && (current == null || s.startDay >= current.startDay))
00107:                     current = s;
00108:             }
00109:             return current!;
00110:         }
00111:
00112:         /// <summary>Multiplier on the daily starvation growth for a pack of this archetype in this window.</summary>
00113:         public static float HungerFactor(SeasonWindowDef? season, MigrationArchetype archetype)
00114:         {
00115:             float f = (season?.id, archetype) switch
00116:             {
00117:                 // Resident predators: winter is lean, the thaw feeds.
00118:                 (SeasonDeepFreeze, MigrationArchetype.Resident) => 1.15f,
00119:                 (SeasonHighCold, MigrationArchetype.Resident) => 1.15f,
00120:                 (SeasonThaw, MigrationArchetype.Resident) => 0.9f,
00121:
00122:                 // Grazing herds: winter range is thin, thaw country is rich.
00123:                 (SeasonDeepFreeze, MigrationArchetype.HerdGrazer) => 1.3f,
00124:                 (SeasonHighCold, MigrationArchetype.HerdGrazer) => 1.1f,
00125:                 (SeasonThaw, MigrationArchetype.HerdGrazer) => 0.75f,
00126:                 (SeasonBlackBloom, MigrationArchetype.HerdGrazer) => 0.8f,
00127:
00128:                 // Burrowers ride grain and warmth; hard cold drives them under.
00129:                 (SeasonDeepFreeze, MigrationArchetype.BurrowSwarm) => 1.2f,
00130:                 (SeasonThaw, MigrationArchetype.BurrowSwarm) => 0.8f,
00131:                 (SeasonBlackBloom, MigrationArchetype.BurrowSwarm) => 0.9f,
00132:                 (SeasonHighCold, MigrationArchetype.BurrowSwarm) => 1.1f,
00133:                 (SeasonAshfall, MigrationArchetype.BurrowSwarm) => 1.05f,
00134:
00135:                 // Sounders peak on the mast fall in The Turning.
00136:                 (SeasonTheTurning, MigrationArchetype.Sounder) => 0.7f,
00137:                 (SeasonDeepFreeze, MigrationArchetype.Sounder) => 1.2f,
00138:                 (SeasonThaw, MigrationArchetype.Sounder) => 0.85f,
00139:                 (SeasonHighCold, MigrationArchetype.Sounder) => 1.05f,
00140:
00141:                 // Passage birds: hard winters empty the sky.
00142:                 (SeasonDeepFreeze, MigrationArchetype.PassageFlock) => 1.25f,
00143:                 (SeasonThaw, MigrationArchetype.PassageFlock) => 0.8f,
00144:                 (SeasonHighCold, MigrationArchetype.PassageFlock) => 1.15f,
00145:
00146:                 // The fish run: thaw and bloom fill the water, ice starves it.
00147:                 (SeasonDeepFreeze, MigrationArchetype.CoastalRunner) => 1.3f,
00148:                 (SeasonThaw, MigrationArchetype.CoastalRunner) => 0.7f,
00149:                 (SeasonBlackBloom, MigrationArchetype.CoastalRunner) => 0.8f,
00150:                 (SeasonHighCold, MigrationArchetype.CoastalRunner) => 1.1f,
00151:                 (SeasonAshfall, MigrationArchetype.CoastalRunner) => 1.2f,
00152:
00153:                 // The moth bloom: warm damp country, gone by first hard cold.
00154:                 (SeasonBlackBloom, MigrationArchetype.SwarmBlight) => 0.5f,
00155:                 (SeasonThaw, MigrationArchetype.SwarmBlight) => 0.9f,
00156:                 (SeasonDeepFreeze, MigrationArchetype.SwarmBlight) => 1.5f,
00157:                 (SeasonHighCold, MigrationArchetype.SwarmBlight) => 1.3f,
00158:                 (SeasonAshfall, MigrationArchetype.SwarmBlight) => 1.3f,
00159:
00160:                 _ => 1f
00161:             };
00162:             return Math.Clamp(f, HungerFactorMin, HungerFactorMax);
00163:         }
00164:
00165:         /// <summary>
00166:         /// How present ( huntable, trappable, visible ) this archetype is in
00167:         /// the given season window. Feeds the trapping density composition and
00168:         /// the coarse map/radio projections — abundance, not population math.
00169:         /// </summary>
00170:         public static float AbundanceFactor(SeasonWindowDef? season, MigrationArchetype archetype)
00171:         {
00172:             float f = (season?.id, archetype) switch
00173:             {
00174:                 (SeasonDeepFreeze, MigrationArchetype.HerdGrazer) => 0.6f,
00175:                 (SeasonThaw, MigrationArchetype.HerdGrazer) => 1.2f,
00176:                 (SeasonBlackBloom, MigrationArchetype.HerdGrazer) => 1.25f,
00177:                 (SeasonHighCold, MigrationArchetype.HerdGrazer) => 0.9f,
00178:                 (SeasonTheTurning, MigrationArchetype.HerdGrazer) => 1.1f,
00179:
00180:                 (SeasonDeepFreeze, MigrationArchetype.BurrowSwarm) => 0.8f,
00181:                 (SeasonThaw, MigrationArchetype.BurrowSwarm) => 1.3f,
00182:                 (SeasonBlackBloom, MigrationArchetype.BurrowSwarm) => 1.3f,
00183:                 (SeasonHighCold, MigrationArchetype.BurrowSwarm) => 0.7f,
00184:
00185:                 (SeasonDeepFreeze, MigrationArchetype.Sounder) => 0.9f,
00186:                 (SeasonThaw, MigrationArchetype.Sounder) => 1.0f,
00187:                 (SeasonBlackBloom, MigrationArchetype.Sounder) => 1.1f,
00188:                 (SeasonHighCold, MigrationArchetype.Sounder) => 0.9f,
00189:                 (SeasonTheTurning, MigrationArchetype.Sounder) => 1.4f,
00190:
00191:                 (SeasonDeepFreeze, MigrationArchetype.PassageFlock) => 0.4f,
00192:                 (SeasonThaw, MigrationArchetype.PassageFlock) => 1.3f,
00193:                 (SeasonBlackBloom, MigrationArchetype.PassageFlock) => 1.0f,
00194:                 (SeasonHighCold, MigrationArchetype.PassageFlock) => 0.6f,
00195:                 (SeasonTheTurning, MigrationArchetype.PassageFlock) => 1.25f,
00196:
00197:                 // The fish run: water wakes in the thaw, runs through the bloom.
00198:                 (SeasonDeepFreeze, MigrationArchetype.CoastalRunner) => 0.2f,
00199:                 (SeasonThaw, MigrationArchetype.CoastalRunner) => 1.5f,
00200:                 (SeasonBlackBloom, MigrationArchetype.CoastalRunner) => 1.4f,
00201:                 (SeasonHighCold, MigrationArchetype.CoastalRunner) => 0.6f,
00202:                 (SeasonAshfall, MigrationArchetype.CoastalRunner) => 0.8f,
00203:                 (SeasonTheTurning, MigrationArchetype.CoastalRunner) => 0.8f,
00204:
00205:                 // The moth bloom: one warm damp window, then nothing.
00206:                 (SeasonBlackBloom, MigrationArchetype.SwarmBlight) => 1.5f,
00207:                 (SeasonThaw, MigrationArchetype.SwarmBlight) => 0.9f,
00208:                 (SeasonDeepFreeze, MigrationArchetype.SwarmBlight) => 0.1f,
00209:                 (SeasonHighCold, MigrationArchetype.SwarmBlight) => 0.4f,
00210:                 (SeasonAshfall, MigrationArchetype.SwarmBlight) => 0.6f,
00211:                 (SeasonTheTurning, MigrationArchetype.SwarmBlight) => 0.5f,
00212:
00213:                 _ => 1f
00214:             };
00215:             return Math.Clamp(f, AbundanceFactorMin, AbundanceFactorMax);
00216:         }
00217:
00218:         /// <summary>
00219:         /// Mean abundance factor of the packs currently holding
00220:         /// <paramref name="sectorId"/>; 1.0 when no pack or no season stands
00221:         /// there. Deterministic: same day, same packs, same factor.
00222:         /// </summary>
00223:         public static float SectorAbundanceFactor(
00224:             SeasonProfileDef? profile, int day, string sectorId,
00225:             IEnumerable<WildlifePackRecord>? packs)
00226:         {
00227:             if (profile == null || packs == null || string.IsNullOrEmpty(sectorId)) return 1f;
00228:             var season = profile.seasons is { Count: > 0 } ? SeasonWindowForDay(profile, day) : null;
00229:             float sum = 0f;
00230:             int n = 0;
00231:             foreach (var p in packs)
00232:             {
00233:                 if (p == null || !string.Equals(p.currentSectorId, sectorId, StringComparison.Ordinal)) continue;
00234:                 sum += AbundanceFactor(season, ArchetypeOf(p.speciesId));
00235:                 n++;
00236:             }
00237:             return n == 0 ? 1f : Math.Clamp(sum / n, AbundanceFactorMin, AbundanceFactorMax);
00238:         }
00239:
00240:         /// <summary>
00241:         /// Water-bound runners only move along water; everywhere else the
00242:         /// hunger drive may pick any neighbor. A runner stranded on dry ground
00243:         /// (legacy save) may cross it to reach water again.
00244:         /// </summary>
00245:         public static List<string> FilterNeighbors(
00246:             MigrationArchetype archetype, string currentSectorId,
00247:             List<string> neighbors, HashSet<string>? waterSectors)
00248:         {
00249:             if (neighbors == null || neighbors.Count == 0) return neighbors ?? new List<string>();
00250:             if (archetype != MigrationArchetype.CoastalRunner || waterSectors == null || waterSectors.Count == 0)
00251:                 return neighbors;
00252:
00253:             bool inWater = waterSectors.Contains(currentSectorId);
00254:             var water = new List<string>();
00255:             foreach (var n in neighbors)
00256:                 if (!string.IsNullOrEmpty(n) && waterSectors.Contains(n)) water.Add(n);
00257:
00258:             if (inWater && water.Count > 0) return water;
00259:             if (!inWater && water.Count > 0) return water; // head for the nearest water
00260:             return neighbors; // stranded; any move is allowed
00261:         }
00262:
00263:         /// <summary>
00264:         /// Plan 28 Phase 5 — observation map: a seeded species sighted in the
00265:         /// wild unlocks its "reading the land" field-guide entry (Plan 20A).
00266:         /// Species absent from the map have no field-guide teach link.
00267:         /// </summary>
00268:         public static string? FieldGuideEntryFor(string speciesId) => speciesId switch
00269:         {
00270:             "species_wolf" => "field_guide_scat_predator_marking",
00271:             "species_feral_goat" => "field_guide_browsing_stripped_bark",
00272:             "species_iron_crow" => "field_guide_birdsong_silence_omen",
00273:             "species_ash_gull" => "field_guide_carrion_circling_watch",
00274:             "species_ghost_moth" => "field_guide_termite_soil_drill",
00275:             "species_ash_boar" => "field_guide_caribou_rut_track",
00276:             _ => null
00277:         };
00278:
00279:         /// <summary>
00280:         /// Coarse, player-plausible notice for a pack seen moving between
00281:         /// sectors. Never exposes exact population; wording stays scout-radio
00282:         /// plain. Returns null for archetypes with no observation value.
00283:         /// </summary>
00284:         public static string? MigrationNotice(
00285:             MigrationArchetype archetype, string speciesId, string fromSector, string toSector, int day)
00286:         {
00287:             if (string.IsNullOrEmpty(speciesId)) return null;
00288:             return archetype switch
00289:             {
00290:                 MigrationArchetype.HerdGrazer =>
00291:                     $"wildlife net: grazing herd sighted leaving {fromSector} for {toSector} (day {day})",
00292:                 MigrationArchetype.Sounder =>
00293:                     $"wildlife net: boar sign heavy on the {fromSector}–{toSector} line (day {day})",
00294:                 MigrationArchetype.PassageFlock =>
00295:                     $"wildlife net: passage birds moving {fromSector} toward {toSector} (day {day})",
00296:                 MigrationArchetype.CoastalRunner =>
00297:                     $"wildlife net: fish running the water at {toSector} (day {day})",
00298:                 MigrationArchetype.SwarmBlight =>
00299:                     $"wildlife net: insect front drifting from {fromSector} toward {toSector} (day {day})",
00300:                 MigrationArchetype.BurrowSwarm =>
00301:                     $"wildlife net: burrower sign spreading out of {fromSector} (day {day})",
00302:                 _ => null
00303:             };
00304:         }
00305:     }
00306: }
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

## `src/UI/WeatherPanel.cs` — 485 lines; 22,650 bytes; SHA-256 `5811f4a177cb9d2fb666a4edb00bb0dfe25de3d07b8ab67d1e6e87cf99a35481`
Declaration index:
- 00020: public partial class WeatherPanel : Control
- 00040: public void Bind(WeatherHostSession weather)
- 00051: public void Bind(WorldHostSession weather)
- 00065: private void HandleWeatherChanged(WeatherKind _)
- 00071: public void RefreshView()
- 00080: private void RefreshStatusRail()
- 00123: private void BuildForecastRows()
- 00173: private static string WeathersRiskLabel(WeatherForecastEntry f)
- 00181: private void BuildSeasonRows()
- 00199: private void BuildIntelligence()
- 00264: private void BuildAdvisory()
- 00313: private static List<AshfallDataGrid.Row> BuildForecastFixture()
- 00366: private void BuildContent()
- 00442: public void Open()
- 00449: public void Close() {
- 00454: public void Unbind()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.UI;
00008: using Ashfall.Core.World;
00009: using AtomicWar.GodotApp.UI;
00010: using DesignTheme = Ashfall.Core.UI.Theme;
00011:
00012: namespace AtomicWar.GodotApp.UI
00013: {
00014:     /// <summary>
00015:     /// ASHFALL — Weather panel.
00016:     /// Shows current weather and a 3-day forecast. Wrapped in the dashboard
00017:     /// shell with a forecast DataGrid and a status rail that carries the
00018:     /// day's live outdoor radiation + crew visibility.
00019:     /// </summary>
00020:     public partial class WeatherPanel : Control
00021:     {
00022:         public event Action? OnClose;
00023:
00024:         public WeatherKind? BoundWeather => ActiveWeather?.Current;
00025:         public bool IsBound => _worldHost != null || _weatherHost != null;
00026:         public int RenderedHazardCount => _advisoryList?.GetChildCount() ?? 0;
00027:
00028:         private WorldHostSession? _worldHost;
00029:         private WeatherHostSession? _weatherHost;
00030:
00031:         private WeatherSystem? ActiveWeather => _worldHost?.Weather ?? _weatherHost?.System;
00032:
00033:         private AshfallDashboardShell _shell = null!;
00034:         private AshfallStatusRail? _statusRail;
00035:         private AshfallDataGrid? _forecastGrid;
00036:         private VBoxContainer _advisoryList = null!;
00037:         private VBoxContainer _seasonList = null!;
00038:         private VBoxContainer _intelligenceList = null!;
00039:
00040:         public void Bind(WeatherHostSession weather)
00041:         {
00042:             _weatherHost = weather;
00043:             if (_weatherHost?.System != null)
00044:             {
00045:                 _weatherHost.System.OnWeatherChanged -= HandleWeatherChanged;
00046:                 _weatherHost.System.OnWeatherChanged += HandleWeatherChanged;
00047:             }
00048:             RefreshView();
00049:         }
00050:
00051:         public void Bind(WorldHostSession weather)
00052:         {
00053:             _worldHost = weather;
00054:             if (_worldHost?.Weather != null)
00055:             {
00056:                 _worldHost.Weather.OnWeatherChanged -= HandleWeatherChanged;
00057:                 _worldHost.Weather.OnWeatherChanged += HandleWeatherChanged;
00058:             }
00059:             RefreshView();
00060:         }
00061:
00062:         /// <summary>Test/selftest observable: event-driven refresh count — exactly one per publisher event while bound.</summary>
00063:         public int RefreshCount { get; private set; }
00064:
00065:         private void HandleWeatherChanged(WeatherKind _)
00066:         {
00067:             RefreshCount++;
00068:             RefreshView();
00069:         }
00070:
00071:         public void RefreshView()
00072:         {
00073:             RefreshStatusRail();
00074:             BuildForecastRows();
00075:             BuildAdvisory();
00076:             BuildSeasonRows();
00077:             BuildIntelligence();
00078:         }
00079:
00080:         private void RefreshStatusRail()
00081:         {
00082:             if (_statusRail == null) return;
00083:             var w = ActiveWeather;
00084:             if (w == null)
00085:             {
00086:                 _statusRail.Set("pattern", "—", AshfallMetricCard.Criticality.Normal);
00087:                 _statusRail.Set("outdoor", "0", AshfallMetricCard.Criticality.Normal);
00088:                 _statusRail.Set("temp_pen", "0°C", AshfallMetricCard.Criticality.Normal);
00089:                 _statusRail.Set("vis", "0%", AshfallMetricCard.Criticality.Normal);
00090:                 _statusRail.Set("hazmat_decay", "×1.0", AshfallMetricCard.Criticality.Normal);
00091:                 return;
00092:             }
00093:             float tempPen = w.TemperaturePenaltyC(w.Current);
00094:             float outdoor = w.OutdoorRadModifier;
00095:             float vis = w.VisibilityFactor;
00096:             _statusRail.Set("pattern", w.Current.ToString().ToUpperInvariant(),
00097:                 outdoor > 0 ? AshfallMetricCard.Criticality.Warn
00098:                 : AshfallMetricCard.Criticality.Normal);
00099:             _statusRail.Set("outdoor", $"+{outdoor:0} mSv/h",
00100:                 outdoor > 100 ? AshfallMetricCard.Criticality.Critical
00101:                 : outdoor > 25 ? AshfallMetricCard.Criticality.Warn
00102:                 : outdoor > 0 ? AshfallMetricCard.Criticality.Caution
00103:                 : AshfallMetricCard.Criticality.Normal);
00104:             _statusRail.Set("temp_pen", $"{tempPen:+#;-#;0}°C",
00105:                 tempPen <= -10 ? AshfallMetricCard.Criticality.Warn
00106:                 : tempPen <= -5 ? AshfallMetricCard.Criticality.Caution
00107:                 : AshfallMetricCard.Criticality.Normal);
00108:             _statusRail.Set("vis", $"{vis:P0}",
00109:                 vis < 0.5 ? AshfallMetricCard.Criticality.Critical
00110:                 : vis < 0.8 ? AshfallMetricCard.Criticality.Warn
00111:                 : AshfallMetricCard.Criticality.Normal);
00112:             _statusRail.Set("hazmat_decay", $"×{w.HazmatDegradeMultiplier:0.0}",
00113:                 w.HazmatDegradeMultiplier > 1.5 ? AshfallMetricCard.Criticality.Warn
00114:                 : AshfallMetricCard.Criticality.Normal);
00115:             if (_statusRail.GetCard("hazmat_decay") != null)
00116:             {
00117:                 // Subtle: hazmat decay rides with outdoor rad sub-critically.
00118:                 if (outdoor > 100 && w.HazmatDegradeMultiplier > 1.0)
00119:                     _statusRail.Set("hazmat_decay", $"×{w.HazmatDegradeMultiplier:0.0}", AshfallMetricCard.Criticality.Critical);
00120:             }
00121:         }
00122:
00123:         private void BuildForecastRows()
00124:         {
00125:             if (_forecastGrid == null) return;
00126:             var w = ActiveWeather;
00127:             if (w == null)
00128:             {
00129:                 _forecastGrid.SetRows(BuildForecastFixture());
00130:                 return;
00131:             }
00132:             int day = Math.Max(1, (int)Math.Floor(w.State.totalElapsedHours / 24f) + 1);
00133:             var forecast = w.PeekForecast(3);
00134:             var rows = new List<AshfallDataGrid.Row>();
00135:             foreach (var f in forecast)
00136:             {
00137:                 var crit = f.OutdoorRad > 100 ? AshfallDataGrid.CellState.Critical
00138:                     : f.OutdoorRad > 25 ? AshfallDataGrid.CellState.Warning
00139:                     : f.OutdoorRad > 0 ? AshfallDataGrid.CellState.Caution
00140:                     : AshfallDataGrid.CellState.Positive;
00141:                 rows.Add(new AshfallDataGrid.Row
00142:                 {
00143:                     Cells = new List<AshfallDataGrid.Cell>
00144:                     {
00145:                         new($"D{f.Day:00}", AshfallDataGrid.CellState.Normal),
00146:                         new(f.Kind.ToString().ToUpperInvariant(), crit),
00147:                         new($"+{f.OutdoorRad:0} mSv/h", crit),
00148:                         new(f.Visibility < 0.5 ? "VIS LOW" : f.Visibility < 0.8 ? "VIS DIM" : "VIS OK",
00149:                             f.Visibility < 0.5 ? AshfallDataGrid.CellState.Warning
00150:                             : f.Visibility < 0.8 ? AshfallDataGrid.CellState.Caution
00151:                             : AshfallDataGrid.CellState.Positive),
00152:                         new(WeathersRiskLabel(f), AshfallDataGrid.CellState.Muted),
00153:                     }
00154:                 });
00155:             }
00156:             if (rows.Count == 0)
00157:             {
00158:                 rows.Add(new AshfallDataGrid.Row
00159:                 {
00160:                     Cells = new List<AshfallDataGrid.Cell>
00161:                     {
00162:                         new("—", AshfallDataGrid.CellState.Muted),
00163:                         new("—", AshfallDataGrid.CellState.Muted),
00164:                         new("—", AshfallDataGrid.CellState.Muted),
00165:                         new("—", AshfallDataGrid.CellState.Muted),
00166:                         new("no forecast available", AshfallDataGrid.CellState.Muted),
00167:                     }
00168:                 });
00169:             }
00170:             _forecastGrid.SetRows(rows);
00171:         }
00172:
00173:         private static string WeathersRiskLabel(WeatherForecastEntry f)
00174:         {
00175:             if (f.OutdoorRad > 100) return "no overhang";
00176:             if (f.OutdoorRad > 25) return "short window";
00177:             if (f.OutdoorRad > 0) return "calm";
00178:             return "ideal";
00179:         }
00180:
00181:         private void BuildSeasonRows()
00182:         {
00183:             if (_seasonList == null) return;
00184:             AshfallUiHelpers.EmptyChildren(_seasonList);
00185:             var w = ActiveWeather;
00186:             if (w == null)
00187:             {
00188:                 _seasonList.AddChild(AshfallUiHelpers.MakeMetadata("No season profile bound."));
00189:                 return;
00190:             }
00191:             int day = Math.Max(1, (int)Math.Floor(w.State.totalElapsedHours / 24f) + 1);
00192:             var season = w.GetSeasonForDay(day);
00193:             string profileName = _worldHost?.Profile?.displayName ?? season.displayName;
00194:             _seasonList.AddChild(AshfallUiHelpers.MakeDataRow("Active Season Profile", profileName, AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
00195:             _seasonList.AddChild(AshfallUiHelpers.MakeDataRow("Next Weather Shift", $"In {w.State.hoursUntilNextCheck:0.0} Hours", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00196:             _seasonList.AddChild(AshfallUiHelpers.MakeDataRow("Recorded Rolls", $"{w.State.rollCount}", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00197:         }
00198:
00199:         private void BuildIntelligence()
00200:         {
00201:             if (_intelligenceList == null) return;
00202:             AshfallUiHelpers.EmptyChildren(_intelligenceList);
00203:             var rm = _worldHost?.WeatherIntelligence?.BuildReadModel();
00204:             if (rm == null)
00205:             {
00206:                 _intelligenceList.AddChild(AshfallUiHelpers.MakeMetadata("Weather intelligence unavailable (world host not bound)."));
00207:                 return;
00208:             }
00209:
00210:             // Station status
00211:             string stationLine = rm.stationOperational
00212:                 ? $"Station ONLINE — accuracy {rm.stationAccuracy:P0}, horizon {rm.forecastHorizonDays}d"
00213:                 : rm.stationInstalled
00214:                     ? "Station installed but NOT calibrated — calibrate to enable forecasts."
00215:                     : "No weather station installed — forecast confidence unavailable.";
00216:             _intelligenceList.AddChild(AshfallUiHelpers.MakeDataRow(
00217:                 "Weather Station", stationLine,
00218:                 rm.stationOperational ? AshfallUiHelpers.ToColor(DesignTheme.Pale)
00219:                     : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00220:
00221:             // Forecast confidence entries (only when operational)
00222:             if (rm.stationOperational && rm.forecast.Count > 0)
00223:             {
00224:                 foreach (var f in rm.forecast)
00225:                 {
00226:                     string conf = $"D{f.day:00}: {f.weather} — {f.confidence:P0} confidence, route {(f.isRouteSafe ? "SAFE" : "UNSAFE")}";
00227:                     var color = f.isRouteSafe ? AshfallUiHelpers.ToColor(DesignTheme.Pale)
00228:                         : AshfallUiHelpers.ToColor(DesignTheme.Lethe);
00229:                     _intelligenceList.AddChild(AshfallUiHelpers.MakeDataRow("Forecast", conf, color));
00230:                 }
00231:             }
00232:
00233:             // Expedition route-safety summary
00234:             string travelLine = rm.stationOperational
00235:                 ? rm.routeSafeDays > 0
00236:                     ? $"Best travel window: day {rm.bestTravelDay} ({rm.bestTravelConfidence:P0} confidence, {rm.routeSafeDays} safe day(s))"
00237:                     : "No safe travel windows in forecast horizon."
00238:                 : "Route safety unknown without a calibrated station.";
00239:             _intelligenceList.AddChild(AshfallUiHelpers.MakeDataRow(
00240:                 "Expedition Routing", travelLine, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
00241:
00242:             // Orbital telemetry
00243:             string orbitalLine;
00244:             if (rm.telemetryActive)
00245:             {
00246:                 orbitalLine = rm.hasPendingImpact
00247:                     ? $"IMPACT WARNING — day {rm.impactDay} ({rm.daysUntilImpact}d lead), grid warning active"
00248:                     : "Telemetry active — no pending impacts.";
00249:             }
00250:             else
00251:             {
00252:                 orbitalLine = "Orbital telemetry inactive — no impact warnings.";
00253:             }
00254:             _intelligenceList.AddChild(AshfallUiHelpers.MakeDataRow(
00255:                 "Orbital Telemetry", orbitalLine,
00256:                 rm.hasPendingImpact ? AshfallUiHelpers.ToColor(DesignTheme.Lethe)
00257:                     : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00258:
00259:             // Advisory summary
00260:             if (!string.IsNullOrEmpty(rm.advisory))
00261:                 _intelligenceList.AddChild(AshfallUiHelpers.MakeMetadata(rm.advisory));
00262:         }
00263:
00264:         private void BuildAdvisory()
00265:         {
00266:             if (_advisoryList == null) return;
00267:             AshfallUiHelpers.EmptyChildren(_advisoryList);
00268:             var w = ActiveWeather;
00269:             if (w == null)
00270:             {
00271:                 _advisoryList.AddChild(AshfallUiHelpers.MakeMetadata("No advisories available."));
00272:                 return;
00273:             }
00274:             int count = 0;
00275:             if (w.IsScavengingBlocked(false))
00276:             {
00277:                 var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
00278:                 var icon = AshfallUiHelpers.MakeBadgeIcon("badge_corneal_burn", 22);
00279:                 row.AddChild(icon);
00280:                 var lbl = AshfallUiHelpers.MakeWarning("Scavenging expedition blocked without full hazard gear.");
00281:                 lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00282:                 row.AddChild(lbl);
00283:                 _advisoryList.AddChild(row);
00284:                 count++;
00285:             }
00286:             if (w.OutdoorRadModifier > 0f)
00287:             {
00288:                 var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
00289:                 var icon = AshfallUiHelpers.MakeBadgeIcon("badge_radon_poisoning", 22);
00290:                 row.AddChild(icon);
00291:                 var lbl = AshfallUiHelpers.MakeCritical($"Fallout radiation elevated: +{w.OutdoorRadModifier:0} mSv/hr.");
00292:                 lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00293:                 row.AddChild(lbl);
00294:                 _advisoryList.AddChild(row);
00295:                 count++;
00296:             }
00297:             float tempPen = w.TemperaturePenaltyC(w.Current);
00298:             if (tempPen < 0f)
00299:             {
00300:                 var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
00301:                 var icon = AshfallUiHelpers.MakeBadgeIcon("badge_hypothermia", 22);
00302:                 row.AddChild(icon);
00303:                 var lbl = AshfallUiHelpers.MakeWarning($"Severe cold exposure risk: {tempPen:0}°C.");
00304:                 lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00305:                 row.AddChild(lbl);
00306:                 _advisoryList.AddChild(row);
00307:                 count++;
00308:             }
00309:             if (count == 0)
00310:                 _advisoryList.AddChild(AshfallUiHelpers.MakeMetadata("No acute environmental hazards detected. Outdoor scavenging permitted."));
00311:         }
00312:
00313:         private static List<AshfallDataGrid.Row> BuildForecastFixture()
00314:         {
00315:             return new List<AshfallDataGrid.Row>
00316:             {
00317:                 new AshfallDataGrid.Row
00318:                 {
00319:                     Cells = new List<AshfallDataGrid.Cell>
00320:                     {
00321:                         new("D/D?", AshfallDataGrid.CellState.Muted),
00322:                         new("—", AshfallDataGrid.CellState.Muted),
00323:                         new("—", AshfallDataGrid.CellState.Muted),
00324:                         new("—", AshfallDataGrid.CellState.Muted),
00325:                         new("unbound", AshfallDataGrid.CellState.Muted),
00326:                     }
00327:                 }
00328:             };
00329:         }
00330:
00331:         public override void _Ready()
00332:         {
00333:             SetAnchorsPreset(LayoutPreset.FullRect);
00334:             Visible = false;
00335:
00336:             var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
00337:             bg.SetAnchorsPreset(LayoutPreset.FullRect);
00338:             AddChild(bg);
00339:
00340:             _shell = new AshfallDashboardShell(
00341:                 "WEATHER & FALLOUT FORECAST — RAD_NOW_LEDGER",
00342:                 1100, 720);
00343:
00344:             var hostContainer = new MarginContainer();
00345:             hostContainer.AddThemeConstantOverride("margin_left", DesignTheme.HudEdge);
00346:             hostContainer.AddThemeConstantOverride("margin_top", DesignTheme.SpacingLg);
00347:             hostContainer.AddThemeConstantOverride("margin_right", DesignTheme.HudEdge);
00348:             hostContainer.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingMd);
00349:             hostContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00350:             hostContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00351:             hostContainer.AddChild(_shell);
00352:             AddChild(hostContainer);
00353:
00354:             _statusRail = _shell.SetStatusRail();
00355:             _statusRail.AddCard("pattern",   "PATTERN",     "—",       AshfallMetricCard.Criticality.Normal, 130);
00356:             _statusRail.AddCard("outdoor",   "EXT RAD",     "0",        AshfallMetricCard.Criticality.Normal, 120);
00357:             _statusRail.AddCard("temp_pen",  "TEMP PEN",    "0°C",      AshfallMetricCard.Criticality.Normal, 110);
00358:             _statusRail.AddCard("vis",       "VISIBILITY",  "0%",       AshfallMetricCard.Criticality.Normal, 130);
00359:             _statusRail.AddCard("hazmat_decay", "HAZMAT",   "×1.0",     AshfallMetricCard.Criticality.Normal, 110);
00360:             _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());
00361:
00362:             BuildContent();
00363:             RefreshView();
00364:         }
00365:
00366:         private void BuildContent()
00367:         {
00368:             var contentStack = new VBoxContainer();
00369:             contentStack.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00370:             contentStack.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00371:             contentStack.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00372:
00373:             contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("3-DAY FORECAST"));
00374:             var cols = new[]
00375:             {
00376:                 new AshfallDataGrid.Column { Header = "Day",  MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Left  },
00377:                 new AshfallDataGrid.Column { Header = "Pattern", MinWidth = 180, Alignment = AshfallDataGrid.ColumnAlign.Left  },
00378:                 new AshfallDataGrid.Column { Header = "Rad+", MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Right },
00379:                 new AshfallDataGrid.Column { Header = "Visibility", MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Center },
00380:                 new AshfallDataGrid.Column { Header = "Risk",  MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left  },
00381:             };
00382:             _forecastGrid = new AshfallDataGrid(cols, showHeader: true, minWidth: 600, minHeight: 240);
00383:             _forecastGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00384:             _forecastGrid.SizeFlagsVertical = SizeFlags.ExpandFill;
00385:             contentStack.AddChild(_forecastGrid);
00386:
00387:             var split = new HBoxContainer();
00388:             split.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00389:             split.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00390:             split.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
00391:
00392:             var seasonPanel = AshfallUiHelpers.MakePanel();
00393:             seasonPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00394:             seasonPanel.SizeFlagsStretchRatio = 1f;
00395:             var seasonMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
00396:             seasonPanel.AddChild(seasonMargin);
00397:             var seasonVbox = new VBoxContainer();
00398:             seasonVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00399:             seasonMargin.AddChild(seasonVbox);
00400:             seasonVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("SEASON CYCLE"));
00401:             _seasonList = new VBoxContainer();
00402:             _seasonList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
00403:             _seasonList.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00404:             seasonVbox.AddChild(_seasonList);
00405:             split.AddChild(seasonPanel);
00406:
00407:             var advisoryPanel = AshfallUiHelpers.MakePanel();
00408:             advisoryPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00409:             advisoryPanel.SizeFlagsStretchRatio = 1f;
00410:             var advMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
00411:             advisoryPanel.AddChild(advMargin);
00412:             var advVBox = new VBoxContainer();
00413:             advVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00414:             advMargin.AddChild(advVBox);
00415:             advVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ENVIRONMENTAL ADVISORIES"));
00416:             _advisoryList = new VBoxContainer();
00417:             _advisoryList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
00418:             _advisoryList.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00419:             advVBox.AddChild(_advisoryList);
00420:             split.AddChild(advisoryPanel);
00421:
00422:             contentStack.AddChild(split);
00423:
00424:             // Weather Intelligence (station forecast confidence + orbital telemetry)
00425:             var intelPanel = AshfallUiHelpers.MakePanel();
00426:             intelPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00427:             var intelMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
00428:             intelPanel.AddChild(intelMargin);
00429:             var intelVBox = new VBoxContainer();
00430:             intelVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00431:             intelMargin.AddChild(intelVBox);
00432:             intelVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("WEATHER INTELLIGENCE"));
00433:             _intelligenceList = new VBoxContainer();
00434:             _intelligenceList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
00435:             _intelligenceList.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00436:             intelVBox.AddChild(_intelligenceList);
00437:             contentStack.AddChild(intelPanel);
00438:
00439:             _shell.SetContent(contentStack);
00440:         }
00441:
00442:         public void Open()
00443:         {
00444:             Visible = true;
00445:             RefreshView();
00446:             QueueRedraw();
00447:         }
00448:
00449:         public void Close() {
00450:             if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
00451:                 Visible = false;
00452:         }
00453:
00454:         public void Unbind()
00455:         {
00456:             if (_worldHost?.Weather != null)
00457:             {
00458:                 _worldHost.Weather.OnWeatherChanged -= HandleWeatherChanged;
00459:             }
00460:             if (_weatherHost?.System != null)
00461:             {
00462:                 _weatherHost.System.OnWeatherChanged -= HandleWeatherChanged;
00463:             }
00464:             _worldHost = null;
00465:             _weatherHost = null;
00466:             RefreshView();
00467:         }
00468:
00469:         public override void _UnhandledInput(InputEvent @event)
00470:         {
00471:             if (!Visible) return;
00472:             if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00473:             {
00474:                 OnClose?.Invoke();
00475:                 GetViewport().SetInputAsHandled();
00476:             }
00477:         }
00478:
00479:         public override void _ExitTree()
00480:         {
00481:             Unbind();
00482:             base._ExitTree();
00483:         }
00484:     }
00485: }
```

## `src/UI/WeatherForecastPanel.cs` — 364 lines; 15,621 bytes; SHA-256 `827668f3d79f92b0198d952b4a1223d55791d50448aa3184c7b4614f652f2dc1`
Declaration index:
- 00017: public partial class WeatherForecastPanel : Control
- 00034: public void Bind(WeatherSystem weather, Ashfall.Core.World.WeatherIntelligenceCoordinator? intelligence = null)
- 00059: public void RefreshView()
- 00183: private void AddForecastRow(VBoxContainer container, WeatherForecastEntry f)
- 00210: private void AddTemperatureRow(VBoxContainer container, WeatherForecastEntry f)
- 00232: private void AddPrecipRow(VBoxContainer container, WeatherForecastEntry f)
- 00255: private void AddWindRow(VBoxContainer container, WeatherForecastEntry f)
- 00278: private static void AddEmptyHint(VBoxContainer container, string hint)
- 00348: public void Open()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Godot;
00004: using Ashfall.Core;
00005: using Ashfall.Core.World;
00006: using Ashfall.Core.UI;
00007: using AtomicWar.GodotApp.UI;
00008: using DesignTheme = Ashfall.Core.UI.Theme;
00009:
00010: namespace AtomicWar.GodotApp.UI;
00011:
00012: /// <summary>
00013: /// ASHFALL — Weather Forecast panel (wired).
00014: /// Shows real 7-day forecast from WeatherSystem.PeekForecast().
00015: /// Replaces hardcoded placeholder strings with live data binding.
00016: /// </summary>
00017: public partial class WeatherForecastPanel : Control
00018: {
00019:     public event Action? OnClose;
00020:
00021:     private WeatherSystem? _weather;
00022:     private Action<WeatherKind>? _onWeatherChanged;
00023:
00024:     private VBoxContainer _forecastData = null!;
00025:     private VBoxContainer _temperatureTrend = null!;
00026:     private VBoxContainer _precipitationData = null!;
00027:     private VBoxContainer _windForecast = null!;
00028:
00029:     private Ashfall.Core.World.WeatherIntelligenceCoordinator? _intelligence;
00030:
00031:     /// <summary>C2 / Plan 20C (§37) — bind the intelligence coordinator so the
00032:     /// forecast can show its own reliability (station accuracy/calibration/
00033:     /// horizon). Optional; unbound hides the reliability line.</summary>
00034:     public void Bind(WeatherSystem weather, Ashfall.Core.World.WeatherIntelligenceCoordinator? intelligence = null)
00035:     {
00036:         if (_weather != null && _onWeatherChanged != null)
00037:             _weather.OnWeatherChanged -= _onWeatherChanged;
00038:
00039:         _weather = weather;
00040:         _intelligence = intelligence;
00041:         _onWeatherChanged ??= _ => RefreshView();
00042:
00043:         if (_weather != null)
00044:             _weather.OnWeatherChanged += _onWeatherChanged;
00045:
00046:         RefreshView();
00047:     }
00048:
00049:     public override void _ExitTree()
00050:     {
00051:         if (_weather != null && _onWeatherChanged != null)
00052:         {
00053:             _weather.OnWeatherChanged -= _onWeatherChanged;
00054:             _weather = null;
00055:         }
00056:         base._ExitTree();
00057:     }
00058:
00059:     public void RefreshView()
00060:     {
00061:         if (_weather == null || _forecastData == null) return;
00062:
00063:         AshfallUiHelpers.EmptyChildren(_forecastData);
00064:         AshfallUiHelpers.EmptyChildren(_temperatureTrend);
00065:         AshfallUiHelpers.EmptyChildren(_precipitationData);
00066:         AshfallUiHelpers.EmptyChildren(_windForecast);
00067:
00068:         // C2 / Plan 20C (§37) — reliability line: the player must be able to
00069:         // judge HOW MUCH to trust the forecast (station accuracy, calibration,
00070:         // horizon). Imperfect but fair — no hidden arbitrary failure.
00071:         if (_intelligence != null)
00072:         {
00073:             var intel = _intelligence.BuildReadModel();
00074:             if (intel != null && intel.stationInstalled)
00075:             {
00076:                 string cal = intel.stationCalibrated ? "calibrated" : "UNCALIBRATED";
00077:                 string stale = intel.forecastHorizonDays <= 0 ? " · NO DATA" : "";
00078:                 var rel = new Label
00079:                 {
00080:                     Text = $"Station {intel.stationTierName} · accuracy {intel.stationAccuracy:P0} · {cal}" +
00081:                            $" · horizon {intel.forecastHorizonDays}d{stale}"
00082:                 };
00083:                 rel.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
00084:                 rel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(
00085:                     intel.stationCalibrated && intel.stationAccuracy >= 0.5f
00086:                         ? DesignTheme.Pale : DesignTheme.Warm));
00087:                 _forecastData.AddChild(rel);
00088:
00089:                 if (intel.hasPredictedCrisis)
00090:                 {
00091:                     var crisisBox = new HBoxContainer();
00092:                     var crisisIcon = new Label { Text = "[!] " };
00093:                     crisisIcon.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeH2);
00094:                     crisisIcon.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warning));
00095:                     crisisBox.AddChild(crisisIcon);
00096:
00097:                     var crisisLabel = new Label
00098:                     {
00099:                         Text = $"CRISIS PREDICTION: {intel.predictedWeatherKind} projected Day {intel.predictedCrisisDay} " +
00100:                                $"({intel.predictedCrisisConfidence:P0} confidence) — {intel.crisisPreparationAdvice}"
00101:                     };
00102:                     crisisLabel.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
00103:                     crisisLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warning));
00104:                     crisisBox.AddChild(crisisLabel);
00105:                     _forecastData.AddChild(crisisBox);
00106:                 }
00107:
00108:                 // C1.5 — Cloud Seeding Action Surface
00109:                 if (_intelligence.CloudSeeding.IsInstalled)
00110:                 {
00111:                     if (_intelligence.CloudSeeding.IsOnCooldown)
00112:                     {
00113:                         var cdLabel = new Label
00114:                         {
00115:                             Text = $"Cloud Seeding Recharging: {_intelligence.CloudSeeding.CooldownRemaining}d remaining"
00116:                         };
00117:                         cdLabel.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
00118:                         cdLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
00119:                         _forecastData.AddChild(cdLabel);
00120:                     }
00121:                     else if (intel.hasPredictedCrisis && intel.predictedWeatherKind.HasValue)
00122:                     {
00123:                         var preflight = _intelligence.CloudSeeding.PreflightDeploy(
00124:                             intel.predictedCrisisDay,
00125:                             intel.predictedWeatherKind.Value,
00126:                             intel.predictedCrisisDay);
00127:
00128:                         var seedBtn = new Button
00129:                         {
00130:                             Text = preflight.CanDeploy
00131:                                 ? $"DEPLOY CLOUD SEEDING ({preflight.SuccessChance:P0} success chance)"
00132:                                 : $"CLOUD SEEDING BLOCKED: {preflight.Reason}",
00133:                             Disabled = !preflight.CanDeploy
00134:                         };
00135:                         seedBtn.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
00136:                         if (preflight.CanDeploy)
00137:                         {
00138:                             seedBtn.Pressed += () =>
00139:                             {
00140:                                 _intelligence.CloudSeeding.Deploy(
00141:                                     intel.predictedCrisisDay,
00142:                                     intel.predictedWeatherKind.Value,
00143:                                     intel.predictedCrisisDay);
00144:                                 RefreshView();
00145:                             };
00146:                         }
00147:                         _forecastData.AddChild(seedBtn);
00148:                     }
00149:                 }
00150:                 else
00151:                 {
00152:                     var installBtn = new Button
00153:                     {
00154:                         Text = "INSTALL CLOUD SEEDING DISPENSER"
00155:                     };
00156:                     installBtn.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
00157:                     installBtn.Pressed += () =>
00158:                     {
00159:                         _intelligence.CloudSeeding.Install(intel.predictedCrisisDay > 0 ? intel.predictedCrisisDay : 1);
00160:                         RefreshView();
00161:                     };
00162:                     _forecastData.AddChild(installBtn);
00163:                 }
00164:             }
00165:         }
00166:
00167:         var forecast = _weather.PeekForecast(7);
00168:         if (forecast.Count == 0)
00169:         {
00170:             AddEmptyHint(_forecastData, "No forecast data available.");
00171:             return;
00172:         }
00173:
00174:         foreach (var f in forecast)
00175:         {
00176:             AddForecastRow(_forecastData, f);
00177:             AddTemperatureRow(_temperatureTrend, f);
00178:             AddPrecipRow(_precipitationData, f);
00179:             AddWindRow(_windForecast, f);
00180:         }
00181:     }
00182:
00183:     private void AddForecastRow(VBoxContainer container, WeatherForecastEntry f)
00184:     {
00185:         var when = f.Day > 0 ? $"Day {f.Day}" : "Today";
00186:         var rad = f.OutdoorRad > 0f ? $", RAD +{f.OutdoorRad:0}" : "";
00187:         var vis = f.Visibility is < 1f and > 0f ? $", VIS {f.Visibility:P0}" : "";
00188:
00189:         // C2 / Plan 20C (§45) — decision-focused effects from the same
00190:         // table the simulation consumes; presented as consequences, not
00191:         // raw coefficients.
00192:         string thermal = f.ThermalLoadC < 0f ? $", COLD {f.ThermalLoadC:0}°C"
00193:             : f.ThermalLoadC > 0f ? $", WARM +{f.ThermalLoadC:0}°C" : "";
00194:         string travel = f.TravelSpeedMultiplier < 1f
00195:             ? $", TRAVEL {f.TravelSpeedMultiplier:P0}" : "";
00196:         string traps = f.TrapYieldMultiplier is < 1f or > 1f
00197:             ? $", TRAPS {f.TrapYieldMultiplier:P0}" : "";
00198:         string caravan = f.CaravanAvailabilityMultiplier < 1f
00199:             ? $", TRADE {f.CaravanAvailabilityMultiplier:P0}" : "";
00200:         var text = $"{when}: {f.Kind}{rad}{vis}{thermal}{travel}{traps}{caravan}";
00201:
00202:         var label = new Label { Text = text };
00203:         label.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
00204:         label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
00205:         label.CustomMinimumSize = new Vector2(350, 30);
00206:         label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00207:         container.AddChild(label);
00208:     }
00209:
00210:     private void AddTemperatureRow(VBoxContainer container, WeatherForecastEntry f)
00211:     {
00212:         // Temperature is implicit in weather kind; show the kind with hazard flag.
00213:         string tempNote = f.Kind switch
00214:         {
00215:             Ashfall.Core.WeatherKind.Blizzard => "−15 °C wind chill",
00216:             Ashfall.Core.WeatherKind.FalloutStorm => "−5 °C wind chill",
00217:             Ashfall.Core.WeatherKind.BlackRain => "−8 °C wind chill",
00218:             _ => "Baseline winter temperature"
00219:         };
00220:
00221:         var when = f.Day > 0 ? $"Day {f.Day}" : "Today";
00222:         var text = $"{when}: {f.Kind} ({tempNote})";
00223:
00224:         var label = new Label { Text = text };
00225:         label.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
00226:         label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00227:         label.CustomMinimumSize = new Vector2(350, 30);
00228:         label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00229:         container.AddChild(label);
00230:     }
00231:
00232:     private void AddPrecipRow(VBoxContainer container, WeatherForecastEntry f)
00233:     {
00234:         string precip = f.Kind switch
00235:         {
00236:             Ashfall.Core.WeatherKind.Rain => "Light rain",
00237:             Ashfall.Core.WeatherKind.Ashfall => "Ashfall (radioactive dust)",
00238:             Ashfall.Core.WeatherKind.FalloutStorm => "Heavy fallout storm",
00239:             Ashfall.Core.WeatherKind.BlackRain => "Black rain (highly radioactive)",
00240:             Ashfall.Core.WeatherKind.Blizzard => "Blizzard (snow + wind)",
00241:             _ => "None"
00242:         };
00243:
00244:         var when = f.Day > 0 ? $"Day {f.Day}" : "Today";
00245:         var text = $"{when}: {precip}";
00246:
00247:         var label = new Label { Text = text };
00248:         label.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
00249:         label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
00250:         label.CustomMinimumSize = new Vector2(350, 30);
00251:         label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00252:         container.AddChild(label);
00253:     }
00254:
00255:     private void AddWindRow(VBoxContainer container, WeatherForecastEntry f)
00256:     {
00257:         string wind = f.Kind switch
00258:         {
00259:             Ashfall.Core.WeatherKind.Blizzard => "High (storm-force)",
00260:             Ashfall.Core.WeatherKind.FalloutStorm => "Gale (20–40 km/h)",
00261:             Ashfall.Core.WeatherKind.BlackRain => "Heavy (30+ km/h)",
00262:             Ashfall.Core.WeatherKind.Ashfall => "Moderate (15–25 km/h)",
00263:             _ => "Light (< 10 km/h)"
00264:         };
00265:
00266:         var vis = f.Visibility is < 1f and > 0f ? $", visibility {f.Visibility:P0}" : "";
00267:         var when = f.Day > 0 ? $"Day {f.Day}" : "Today";
00268:         var text = $"{when}: {wind}{vis}";
00269:
00270:         var label = new Label { Text = text };
00271:         label.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
00272:         label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
00273:         label.CustomMinimumSize = new Vector2(350, 30);
00274:         label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00275:         container.AddChild(label);
00276:     }
00277:
00278:     private static void AddEmptyHint(VBoxContainer container, string hint)
00279:     {
00280:         var lbl = new Label { Text = hint };
00281:         lbl.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
00282:         lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
00283:         lbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00284:         container.AddChild(lbl);
00285:     }
00286:
00287:     public override void _Ready()
00288:     {
00289:         SetAnchorsPreset(LayoutPreset.FullRect);
00290:         Visible = false;
00291:
00292:         var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
00293:         bg.SetAnchorsPreset(LayoutPreset.FullRect);
00294:         AddChild(bg);
00295:
00296:         var container = new CenterContainer();
00297:         container.SetAnchorsPreset(LayoutPreset.FullRect);
00298:         AddChild(container);
00299:
00300:         var vbox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingLg);
00301:         vbox.CustomMinimumSize = new Vector2(550, 0);
00302:         container.AddChild(vbox);
00303:
00304:         var title = AshfallUiHelpers.MakeTitle("WEATHER FORECAST", DesignTheme.FontSizeH1);
00305:         title.HorizontalAlignment = HorizontalAlignment.Center;
00306:         vbox.AddChild(title);
00307:
00308:         vbox.AddChild(AshfallUiHelpers.MakeSeparator());
00309:
00310:         _forecastData = new VBoxContainer();
00311:         _forecastData.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00312:         _forecastData.CustomMinimumSize = new Vector2(400, 0);
00313:         vbox.AddChild(_forecastData);
00314:
00315:         vbox.AddChild(AshfallUiHelpers.MakeSeparator());
00316:
00317:         _temperatureTrend = new VBoxContainer();
00318:         _temperatureTrend.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00319:         _temperatureTrend.CustomMinimumSize = new Vector2(400, 0);
00320:         vbox.AddChild(_temperatureTrend);
00321:
00322:         vbox.AddChild(AshfallUiHelpers.MakeSeparator());
00323:
00324:         _precipitationData = new VBoxContainer();
00325:         _precipitationData.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00326:         _precipitationData.CustomMinimumSize = new Vector2(400, 0);
00327:         vbox.AddChild(_precipitationData);
00328:
00329:         vbox.AddChild(AshfallUiHelpers.MakeSeparator());
00330:
00331:         _windForecast = new VBoxContainer();
00332:         _windForecast.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
00333:         _windForecast.CustomMinimumSize = new Vector2(400, 0);
00334:         vbox.AddChild(_windForecast);
00335:
00336:         vbox.AddChild(AshfallUiHelpers.MakeSeparator());
00337:
00338:         var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
00339:         btnClose.CustomMinimumSize = new Vector2(200, 40);
00340:         vbox.AddChild(btnClose);
00341:
00342:         var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
00343:         hint.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeLabel);
00344:         hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
00345:         vbox.AddChild(hint);
00346:     }
00347:
00348:     public void Open()
00349:     {
00350:         Visible = true;
00351:         RefreshView();
00352:         QueueRedraw();
00353:     }
00354:
00355:     public override void _UnhandledInput(InputEvent @event)
00356:     {
00357:         if (!Visible) return;
00358:         if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00359:         {
00360:             OnClose?.Invoke();
00361:             GetViewport().SetInputAsHandled();
00362:         }
00363:     }
00364: }
```

## `Ashfall.Core.Tests/World/WeatherSeasonExpansionTests.cs` — 570 lines; 30,309 bytes; SHA-256 `a2378b5c7b0d33e3ce3fc71c8213b5a353d7bd3f63c42fca8799095208f2407f`
Declaration index:
- 00018: public sealed class WeatherSeasonExpansionTests
- 00036: private SeasonProfileDef LoadProfile()
- 00043: private WeatherSystem CreateBoundWeather(int seed = 42)
- 00056: public void Points_01_to_12_Catalogue_LoadsExactTenWindowsWithPreservedAndNewPhases()
- 00094: public void Points_13_to_16_Catalogue_IdsAreUniqueAndStartDaysStrictlyIncreasing()
- 00126: public void Points_17_to_38_ActiveWindow_SelectionSemantics_AndBoundaryDays()
- 00184: public void Points_39_to_43_WeightVectors_AreValidFiniteAndNonNegative()
- 00223: public void Point_44_AllTenSevenWeightVectors_AreStrictlyUnique()
- 00260: public void Points_45_to_51_DominantSignatures_ThematicIntegrityAndAdjacentDifferences()
- 00336: public void Points_52_to_53_NormalizedProbabilities_SumToOneAndZeroHandled()
- 00376: public void Points_54_to_60_WeatherSystem_ProducesValidWeatherAcrossMilestones()
- 00380: // Points 55–60: Weather generation produces valid WeatherKind enum values
- 00396: public void Points_61_to_64_Plan48WeatherGates_FirstFalloutAndBlackRainGatesResolve()
- 00436: public void Points_65_to_67_DeterminismAndLookaheadIntegrity()
- 00477: public void Points_68_to_72_SaveLoad_StateRoundTripPreservesStateAndReDerivesWindow()
- 00524: public void Point_73_FullYearProgression_VisitsAllTenWindowsInChronologicalSequence()
- 00562: public void Point_74_WeatherSystem_CatalogAndRuntimeIntegrity_PreconditionsHold()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core;
00007: using Ashfall.Core.IO;
00008: using Ashfall.Core.Random;
00009: using Ashfall.Core.World;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.World
00013: {
00014:     /// <summary>
00015:     /// Plan 83 — Weather Season Windows Expansion: 3 → 10 Campaign Weather Phases.
00016:     /// Comprehensive test suite verifying all 74 scenario items in the Plan 83 specification catalogue.
00017:     /// </summary>
00018:     public sealed class WeatherSeasonExpansionTests
00019:     {
00020:         private readonly string _dataDir;
00021:         private readonly IFileIO _fileIO;
00022:         private readonly IJsonSerializer _jsonSerializer;
00023:
00024:         public WeatherSeasonExpansionTests()
00025:         {
00026:             _dataDir = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
00027:             if (!Directory.Exists(_dataDir))
00028:             {
00029:                 _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
00030:             }
00031:
00032:             _fileIO = new FileSystemIO();
00033:             _jsonSerializer = new SystemTextJsonSerializer();
00034:         }
00035:
00036:         private SeasonProfileDef LoadProfile()
00037:         {
00038:             var profile = WeatherProfileLoader.Load(_dataDir, _fileIO, _jsonSerializer);
00039:             Assert.NotNull(profile);
00040:             return profile!;
00041:         }
00042:
00043:         private WeatherSystem CreateBoundWeather(int seed = 42)
00044:         {
00045:             var profile = LoadProfile();
00046:             var system = new WeatherSystem();
00047:             system.BindProfile(profile, seed);
00048:             return system;
00049:         }
00050:
00051:         // ────────────────────────────────────────────────────────────────────────
00052:         // Points 1–12: Catalog Parsing, Count, and Presence of All 10 Windows
00053:         // ────────────────────────────────────────────────────────────────────────
00054:
00055:         [Fact]
00056:         public void Points_01_to_12_Catalogue_LoadsExactTenWindowsWithPreservedAndNewPhases()
00057:         {
00058:             // Point 1: Catalog parses cleanly
00059:             var profile = LoadProfile();
00060:
00061:             // Point 2: Total season count is exactly 10
00062:             Assert.Equal(10, profile.seasons.Count);
00063:
00064:             // Points 3–12: All 10 specific windows are present by ID and display name
00065:             var expectedWindows = new[]
00066:             {
00067:                 (id: "window_first_thaw", name: "First Thaw", startDay: 0),
00068:                 (id: "window_ash_settling", name: "Ash Settling", startDay: 30),
00069:                 (id: "window_deep_freeze", name: "The Deep Freeze", startDay: 60),
00070:                 (id: "window_spring_storms", name: "Spring Storms", startDay: 90),
00071:                 (id: "window_dry_ash", name: "Dry Ash", startDay: 120),
00072:                 (id: "window_first_fallout", name: "First Fallout", startDay: 150),
00073:                 (id: "window_false_spring", name: "False Spring", startDay: 180),
00074:                 (id: "window_deep_ash", name: "Deep Ash", startDay: 200),
00075:                 (id: "window_long_winter", name: "The Long Winter", startDay: 240),
00076:                 (id: "window_black_rain_season", name: "Black Rain Season", startDay: 280),
00077:             };
00078:
00079:             for (int i = 0; i < expectedWindows.Length; i++)
00080:             {
00081:                 var expected = expectedWindows[i];
00082:                 var actual = profile.seasons[i];
00083:                 Assert.Equal(expected.id, actual.id);
00084:                 Assert.Equal(expected.name, actual.displayName);
00085:                 Assert.Equal(expected.startDay, actual.startDay);
00086:             }
00087:         }
00088:
00089:         // ────────────────────────────────────────────────────────────────────────
00090:         // Points 13–16: ID Uniqueness, Prefix, and Strictly Increasing StartDays
00091:         // ────────────────────────────────────────────────────────────────────────
00092:
00093:         [Fact]
00094:         public void Points_13_to_16_Catalogue_IdsAreUniqueAndStartDaysStrictlyIncreasing()
00095:         {
00096:             var profile = LoadProfile();
00097:
00098:             // Point 13: All 10 IDs unique and have window_ prefix
00099:             var idSet = new HashSet<string>(StringComparer.Ordinal);
00100:             foreach (var s in profile.seasons)
00101:             {
00102:                 Assert.StartsWith("window_", s.id);
00103:                 Assert.True(idSet.Add(s.id), $"Duplicate season ID: {s.id}");
00104:                 Assert.False(string.IsNullOrWhiteSpace(s.displayName));
00105:             }
00106:             Assert.Equal(10, idSet.Count);
00107:
00108:             // Points 14–16: Strictly increasing startDays, no duplicates, exact schedule
00109:             var expectedSchedule = new[] { 0, 30, 60, 90, 120, 150, 180, 200, 240, 280 };
00110:             for (int i = 0; i < profile.seasons.Count; i++)
00111:             {
00112:                 Assert.Equal(expectedSchedule[i], profile.seasons[i].startDay);
00113:                 if (i > 0)
00114:                 {
00115:                     Assert.True(profile.seasons[i].startDay > profile.seasons[i - 1].startDay,
00116:                         $"startDay must be strictly increasing: index {i} ({profile.seasons[i].startDay}) <= index {i-1} ({profile.seasons[i-1].startDay})");
00117:                 }
00118:             }
00119:         }
00120:
00121:         // ────────────────────────────────────────────────────────────────────────
00122:         // Points 17–38: Active Window Selection Semantics & Day Boundaries
00123:         // ────────────────────────────────────────────────────────────────────────
00124:
00125:         [Fact]
00126:         public void Points_17_to_38_ActiveWindow_SelectionSemantics_AndBoundaryDays()
00127:         {
00128:             var cases = new (int Day, string ExpectedId)[]
00129:             {
00130:                 (-1, "default"),
00131:                 (-50, "default"),
00132:                 (0, "window_first_thaw"),
00133:                 (15, "window_first_thaw"),
00134:                 (29, "window_first_thaw"),
00135:                 (30, "window_ash_settling"),
00136:                 (45, "window_ash_settling"),
00137:                 (59, "window_ash_settling"),
00138:                 (60, "window_deep_freeze"),
00139:                 (75, "window_deep_freeze"),
00140:                 (89, "window_deep_freeze"),
00141:                 (90, "window_spring_storms"),
00142:                 (105, "window_spring_storms"),
00143:                 (119, "window_spring_storms"),
00144:                 (120, "window_dry_ash"),
00145:                 (135, "window_dry_ash"),
00146:                 (149, "window_dry_ash"),
00147:                 (150, "window_first_fallout"),
00148:                 (165, "window_first_fallout"),
00149:                 (179, "window_first_fallout"),
00150:                 (180, "window_false_spring"),
00151:                 (190, "window_false_spring"),
00152:                 (199, "window_false_spring"),
00153:                 (200, "window_deep_ash"),
00154:                 (220, "window_deep_ash"),
00155:                 (239, "window_deep_ash"),
00156:                 (240, "window_long_winter"),
00157:                 (260, "window_long_winter"),
00158:                 (279, "window_long_winter"),
00159:                 (280, "window_black_rain_season"),
00160:                 (365, "window_black_rain_season"),
00161:                 (500, "window_black_rain_season"),
00162:                 (1000, "window_black_rain_season")
00163:             };
00164:             var weather = CreateBoundWeather();
00165:             var failures = new List<string>();
00166:
00167:             foreach (var testCase in cases)
00168:             {
00169:                 var actual = weather.GetSeasonForDay(testCase.Day).id;
00170:                 if (actual != testCase.ExpectedId)
00171:                 {
00172:                     failures.Add($"day {testCase.Day}: expected {testCase.ExpectedId}, got {actual}");
00173:                 }
00174:             }
00175:
00176:             Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
00177:         }
00178:
00179:         // ────────────────────────────────────────────────────────────────────────
00180:         // Points 39–43: All Seven Weights Present, Finite, Non-Negative, Positive Total
00181:         // ────────────────────────────────────────────────────────────────────────
00182:
00183:         [Fact]
00184:         public void Points_39_to_43_WeightVectors_AreValidFiniteAndNonNegative()
00185:         {
00186:             var profile = LoadProfile();
00187:
00188:             foreach (var s in profile.seasons)
00189:             {
00190:                 var weights = new[]
00191:                 {
00192:                     s.clearWeight, s.rainWeight, s.overcastWeight,
00193:                     s.ashfallWeight, s.falloutStormWeight, s.blizzardWeight, s.blackRainWeight
00194:                 };
00195:
00196:                 // Point 39: all seven weights exist and are checked
00197:                 Assert.Equal(7, weights.Length);
00198:
00199:                 float total = 0f;
00200:                 foreach (var w in weights)
00201:                 {
00202:                     // Point 40: no negative weights
00203:                     Assert.True(w >= 0.0f, $"Season {s.id} has negative weight {w}");
00204:                     // Point 41: no NaN weights
00205:                     Assert.False(float.IsNaN(w), $"Season {s.id} has NaN weight");
00206:                     // Point 42: no infinity weights
00207:                     Assert.False(float.IsInfinity(w), $"Season {s.id} has infinite weight");
00208:                     // Reasonable game balance bound
00209:                     Assert.True(w <= 5.0f, $"Season {s.id} has excessive weight {w}");
00210:                     total += w;
00211:                 }
00212:
00213:                 // Point 43: total weight > 0
00214:                 Assert.True(total > 0.0f, $"Season {s.id} total weight must be > 0");
00215:             }
00216:         }
00217:
00218:         // ────────────────────────────────────────────────────────────────────────
00219:         // Point 44: Every Seven-Weight Vector is Unique
00220:         // ────────────────────────────────────────────────────────────────────────
00221:
00222:         [Fact]
00223:         public void Point_44_AllTenSevenWeightVectors_AreStrictlyUnique()
00224:         {
00225:             var profile = LoadProfile();
00226:             var vectorSignatures = new HashSet<string>(StringComparer.Ordinal);
00227:
00228:             foreach (var s in profile.seasons)
00229:             {
00230:                 string sig = $"{s.clearWeight:F2}|{s.rainWeight:F2}|{s.overcastWeight:F2}|" +
00231:                              $"{s.ashfallWeight:F2}|{s.falloutStormWeight:F2}|{s.blizzardWeight:F2}|{s.blackRainWeight:F2}";
00232:                 Assert.True(vectorSignatures.Add(sig),
00233:                     $"Duplicate 7-weight vector found in season {s.id}: {sig}");
00234:             }
00235:
00236:             Assert.Equal(10, vectorSignatures.Count);
00237:         }
00238:
00239:         // ────────────────────────────────────────────────────────────────────────
00240:         // Points 45–51: Dominant Weather Signatures & Thematic Differentiation
00241:         // ────────────────────────────────────────────────────────────────────────
00242:
00243:         private static (WeatherKind dominant, float maxWeight, WeatherKind secondary, float secondWeight) GetSignature(SeasonWindowDef s)
00244:         {
00245:             var list = new (WeatherKind kind, float weight)[]
00246:             {
00247:                 (WeatherKind.Clear, s.clearWeight),
00248:                 (WeatherKind.Rain, s.rainWeight),
00249:                 (WeatherKind.Overcast, s.overcastWeight),
00250:                 (WeatherKind.Ashfall, s.ashfallWeight),
00251:                 (WeatherKind.FalloutStorm, s.falloutStormWeight),
00252:                 (WeatherKind.Blizzard, s.blizzardWeight),
00253:                 (WeatherKind.BlackRain, s.blackRainWeight)
00254:             }.OrderByDescending(x => x.weight).ToArray();
00255:
00256:             return (list[0].kind, list[0].weight, list[1].kind, list[1].weight);
00257:         }
00258:
00259:         [Fact]
00260:         public void Points_45_to_51_DominantSignatures_ThematicIntegrityAndAdjacentDifferences()
00261:         {
00262:             var profile = LoadProfile();
00263:
00264:             // Point 45: No two adjacent windows share the same dominant weather
00265:             for (int i = 0; i < profile.seasons.Count - 1; i++)
00266:             {
00267:                 var cur = GetSignature(profile.seasons[i]);
00268:                 var next = GetSignature(profile.seasons[i + 1]);
00269:                 Assert.NotEqual(cur.dominant, next.dominant);
00270:             }
00271:
00272:             // Verify the sequence of dominant weather types across the 10 windows
00273:             var expectedDominants = new[]
00274:             {
00275:                 WeatherKind.Rain,         // First Thaw (27.3%)
00276:                 WeatherKind.Ashfall,      // Ash Settling (35.1%)
00277:                 WeatherKind.Blizzard,     // Deep Freeze (42.4%)
00278:                 WeatherKind.Rain,         // Spring Storms (32.4%)
00279:                 WeatherKind.Ashfall,      // Dry Ash (42.6%)
00280:                 WeatherKind.FalloutStorm, // First Fallout (35.1%)
00281:                 WeatherKind.Clear,        // False Spring (35.3%)
00282:                 WeatherKind.Ashfall,      // Deep Ash (33.7%)
00283:                 WeatherKind.Blizzard,     // Long Winter (34.3%)
00284:                 WeatherKind.BlackRain     // Black Rain Season (31.3%)
00285:             };
00286:
00287:             for (int i = 0; i < profile.seasons.Count; i++)
00288:             {
00289:                 var sig = GetSignature(profile.seasons[i]);
00290:                 Assert.Equal(expectedDominants[i], sig.dominant);
00291:             }
00292:
00293:             // Point 46 & 47: False Spring dominant is Clear; severe weather is materially lower than adjacent windows
00294:             var firstFallout = profile.seasons.First(s => s.id == "window_first_fallout");
00295:             var falseSpring = profile.seasons.First(s => s.id == "window_false_spring");
00296:             var deepAsh = profile.seasons.First(s => s.id == "window_deep_ash");
00297:
00298:             Assert.Equal(WeatherKind.Clear, GetSignature(falseSpring).dominant);
00299:             Assert.True(falseSpring.clearWeight >= 2.0f);
00300:
00301:             float severeFallout = (firstFallout.falloutStormWeight + firstFallout.blizzardWeight + firstFallout.blackRainWeight)
00302:                                  / (firstFallout.clearWeight + firstFallout.rainWeight + firstFallout.overcastWeight + firstFallout.ashfallWeight + firstFallout.falloutStormWeight + firstFallout.blizzardWeight + firstFallout.blackRainWeight);
00303:             float severeSpring = (falseSpring.falloutStormWeight + falseSpring.blizzardWeight + falseSpring.blackRainWeight)
00304:                                  / (falseSpring.clearWeight + falseSpring.rainWeight + falseSpring.overcastWeight + falseSpring.ashfallWeight + falseSpring.falloutStormWeight + falseSpring.blizzardWeight + falseSpring.blackRainWeight);
00305:             float severeDeepAsh = (deepAsh.falloutStormWeight + deepAsh.blizzardWeight + deepAsh.blackRainWeight)
00306:                                  / (deepAsh.clearWeight + deepAsh.rainWeight + deepAsh.overcastWeight + deepAsh.ashfallWeight + deepAsh.falloutStormWeight + deepAsh.blizzardWeight + deepAsh.blackRainWeight);
00307:
00308:             Assert.True(severeSpring < 0.20f, $"False spring severe weather {severeSpring:P1} should be under 20%");
00309:             Assert.True(severeFallout > 0.40f, $"First fallout severe weather {severeFallout:P1} should exceed 40%");
00310:             Assert.True(severeDeepAsh > 0.40f, $"Deep ash severe weather {severeDeepAsh:P1} should exceed 40%");
00311:             Assert.True(severeSpring < severeFallout * 0.4f, "False spring must be materially calmer than First Fallout");
00312:
00313:             // Point 48: First Fallout peaks fallout storm weight
00314:             Assert.Equal(WeatherKind.FalloutStorm, GetSignature(firstFallout).dominant);
00315:             Assert.Equal(2.7f, firstFallout.falloutStormWeight, 2);
00316:
00317:             // Point 49: Black Rain Season peaks black rain weight
00318:             var blackRainSeason = profile.seasons.First(s => s.id == "window_black_rain_season");
00319:             Assert.Equal(WeatherKind.BlackRain, GetSignature(blackRainSeason).dominant);
00320:             Assert.Equal(3.0f, blackRainSeason.blackRainWeight, 2);
00321:
00322:             // Point 50: Spring Storms rain is dominant
00323:             var springStorms = profile.seasons.First(s => s.id == "window_spring_storms");
00324:             Assert.Equal(WeatherKind.Rain, GetSignature(springStorms).dominant);
00325:
00326:             // Point 51: Dry Ash ashfall is dominant
00327:             var dryAsh = profile.seasons.First(s => s.id == "window_dry_ash");
00328:             Assert.Equal(WeatherKind.Ashfall, GetSignature(dryAsh).dominant);
00329:         }
00330:
00331:         // ────────────────────────────────────────────────────────────────────────
00332:         // Points 52–53: Normalized Probabilities Sum to 1.0 & Zero Weights Handled
00333:         // ────────────────────────────────────────────────────────────────────────
00334:
00335:         [Fact]
00336:         public void Points_52_to_53_NormalizedProbabilities_SumToOneAndZeroHandled()
00337:         {
00338:             var profile = LoadProfile();
00339:
00340:             foreach (var s in profile.seasons)
00341:             {
00342:                 float total = s.clearWeight + s.rainWeight + s.overcastWeight +
00343:                               s.ashfallWeight + s.falloutStormWeight + s.blizzardWeight + s.blackRainWeight;
00344:                 Assert.True(total > 0f);
00345:
00346:                 float pClear = s.clearWeight / total;
00347:                 float pRain = s.rainWeight / total;
00348:                 float pOvercast = s.overcastWeight / total;
00349:                 float pAsh = s.ashfallWeight / total;
00350:                 float pFallout = s.falloutStormWeight / total;
00351:                 float pBlizzard = s.blizzardWeight / total;
00352:                 float pBlackRain = s.blackRainWeight / total;
00353:
00354:                 float sum = pClear + pRain + pOvercast + pAsh + pFallout + pBlizzard + pBlackRain;
00355:                 // Point 52: Sum equals 1.0 within float precision
00356:                 Assert.Equal(1.0f, sum, 4);
00357:             }
00358:
00359:             // Point 53: Zero weight yields exactly 0 probability
00360:             var mockWindow = new SeasonWindowDef
00361:             {
00362:                 id = "window_zero_test",
00363:                 displayName = "Zero Test",
00364:                 clearWeight = 1.0f,
00365:                 rainWeight = 0.0f
00366:             };
00367:             float mockTotal = mockWindow.clearWeight + mockWindow.rainWeight;
00368:             Assert.Equal(0.0f, mockWindow.rainWeight / mockTotal);
00369:         }
00370:
00371:         // ────────────────────────────────────────────────────────────────────────
00372:         // Points 54–60: WeatherSystem Binds Profile & Produces Valid Weather at Milestones
00373:         // ────────────────────────────────────────────────────────────────────────
00374:
00375:         [Fact]
00376:         public void Points_54_to_60_WeatherSystem_ProducesValidWeatherAcrossMilestones()
00377:         {
00378:             var weather = CreateBoundWeather(seed: 12345);
00379:
00380:             // Points 55–60: Weather generation produces valid WeatherKind enum values
00381:             int[] milestoneDays = { 0, 30, 60, 90, 120, 150 };
00382:             foreach (var day in milestoneDays)
00383:             {
00384:                 weather.Tick(24f);
00385:                 var kind = weather.Current;
00386:                 Assert.True(Enum.IsDefined(typeof(WeatherKind), kind),
00387:                     $"Day {day} produced invalid WeatherKind {kind}");
00388:             }
00389:         }
00390:
00391:         // ────────────────────────────────────────────────────────────────────────
00392:         // Points 61–64: Plan 48 Weather-Gate Integration
00393:         // ────────────────────────────────────────────────────────────────────────
00394:
00395:         [Fact]
00396:         public void Points_61_to_64_Plan48WeatherGates_FirstFalloutAndBlackRainGatesResolve()
00397:         {
00398:             // Point 61 & 62: First Fallout and Black Rain Season gate targets resolve in catalog
00399:             var gateCatalog = WeatherRouteGateCatalog.LoadFromDirectory(_dataDir, _fileIO);
00400:             Assert.NotNull(gateCatalog);
00401:             Assert.True(gateCatalog.Gates.Count >= 18);
00402:
00403:             // Verify FalloutStorm gates exist and block during FalloutStorm
00404:             var falloutGates = gateCatalog.Gates.Where(g => g.blocked_weather.Contains("FalloutStorm")).ToList();
00405:             Assert.NotEmpty(falloutGates);
00406:             Assert.Contains(falloutGates, g => g.target == "loc_the_shallows_market" || g.target.Contains("fallout"));
00407:
00408:             // Verify BlackRain gates exist and block during BlackRain
00409:             var blackRainGates = gateCatalog.Gates.Where(g => g.blocked_weather.Contains("BlackRain")).ToList();
00410:             Assert.NotEmpty(blackRainGates);
00411:             Assert.Contains(blackRainGates, g => g.target == "location_flooded_subway_depot" || g.target.Contains("black_rain"));
00412:
00413:             // Point 63: Gates use existing authority (WeatherRouteGateCatalog evaluates without mutating WeatherSystem)
00414:             bool found = gateCatalog.TryGetGatesForTarget("location_flooded_subway_depot", out var gates);
00415:             Assert.True(found);
00416:             Assert.NotEmpty(gates);
00417:             var subwayGate = gates[0];
00418:             bool isBlocked = WeatherRouteGateCatalog.IsGateBlocking(subwayGate, "BlackRain", null);
00419:             Assert.True(isBlocked);
00420:
00421:             bool notBlocked = WeatherRouteGateCatalog.IsGateBlocking(subwayGate, "Clear", null);
00422:             Assert.False(notBlocked);
00423:
00424:             // Point 64: Window activation alone does not duplicate route-state ownership
00425:             // WeatherSystem only owns weather rolling; Plan 48 owns the gate checks.
00426:             var weather = CreateBoundWeather();
00427:             var blackRainWindow = weather.GetSeasonForDay(280);
00428:             Assert.Equal("window_black_rain_season", blackRainWindow.id);
00429:         }
00430:
00431:         // ────────────────────────────────────────────────────────────────────────
00432:         // Points 65–67: Determinism & Lookahead Integrity
00433:         // ────────────────────────────────────────────────────────────────────────
00434:
00435:         [Fact]
00436:         public void Points_65_to_67_DeterminismAndLookaheadIntegrity()
00437:         {
00438:             // Point 65: Same seed produces identical 30-day weather trace
00439:             var w1 = CreateBoundWeather(seed: 9999);
00440:             var w2 = CreateBoundWeather(seed: 9999);
00441:
00442:             var trace1 = new List<WeatherKind>();
00443:             var trace2 = new List<WeatherKind>();
00444:
00445:             for (int day = 0; day < 30; day++)
00446:             {
00447:                 w1.Tick(24f);
00448:                 w2.Tick(24f);
00449:                 trace1.Add(w1.Current);
00450:                 trace2.Add(w2.Current);
00451:             }
00452:
00453:             Assert.Equal(trace1, trace2);
00454:
00455:             // Point 66: Different seeds produce differing weather traces
00456:             var wDiff = CreateBoundWeather(seed: 1111);
00457:             var traceDiff = new List<WeatherKind>();
00458:             for (int day = 0; day < 30; day++)
00459:             {
00460:                 wDiff.Tick(24f);
00461:                 traceDiff.Add(wDiff.Current);
00462:             }
00463:             Assert.NotEqual(trace1, traceDiff);
00464:
00465:             // Point 67: PeekForecast / lookahead does not advance rollCount
00466:             int rollCountBefore = w1.State.rollCount;
00467:             var forecast = w1.PeekForecast(7);
00468:             Assert.Equal(7, forecast.Count);
00469:             Assert.Equal(rollCountBefore, w1.State.rollCount);
00470:         }
00471:
00472:         // ────────────────────────────────────────────────────────────────────────
00473:         // Points 68–72: Save / Load State Round-Trip and Multi-Window Restores
00474:         // ────────────────────────────────────────────────────────────────────────
00475:
00476:         [Fact]
00477:         public void Points_68_to_72_SaveLoad_StateRoundTripPreservesStateAndReDerivesWindow()
00478:         {
00479:             int[] testDays = { 45, 135, 210 }; // Days belonging to Ash Settling, Dry Ash, Deep Ash
00480:
00481:             foreach (var targetDay in testDays)
00482:             {
00483:                 var weather = CreateBoundWeather(seed: 7777 + targetDay);
00484:
00485:                 for (int day = 0; day <= targetDay; day++)
00486:                 {
00487:                     weather.Tick(24f);
00488:                 }
00489:
00490:                 var state = weather.State;
00491:                 int rollCount = state.rollCount;
00492:                 WeatherKind currentKind = weather.Current;
00493:
00494:                 // Capture save state (JSON serialize/deserialize)
00495:                 string json = _jsonSerializer.Serialize(state);
00496:                 var restoredState = _jsonSerializer.Deserialize<WorldWeatherState>(json);
00497:                 Assert.NotNull(restoredState);
00498:
00499:                 // Point 68 & 69: Round-trip preserves rollCount and currentKind
00500:                 Assert.Equal(rollCount, restoredState!.rollCount);
00501:                 Assert.Equal(currentKind.ToString(), restoredState.currentKind);
00502:
00503:                 // Re-bind to a fresh WeatherSystem
00504:                 var restoredWeather = CreateBoundWeather(seed: 7777 + targetDay);
00505:                 restoredWeather.RestoreState(restoredState);
00506:
00507:                 Assert.Equal(rollCount, restoredWeather.State.rollCount);
00508:                 Assert.Equal(currentKind, restoredWeather.Current);
00509:
00510:                 // Points 70–72: Re-derives the active window correctly from the day
00511:                 var activeWindow = restoredWeather.GetSeasonForDay(targetDay);
00512:                 Assert.NotNull(activeWindow);
00513:                 if (targetDay == 45) Assert.Equal("window_ash_settling", activeWindow.id);
00514:                 if (targetDay == 135) Assert.Equal("window_dry_ash", activeWindow.id);
00515:                 if (targetDay == 210) Assert.Equal("window_deep_ash", activeWindow.id);
00516:             }
00517:         }
00518:
00519:         // ────────────────────────────────────────────────────────────────────────
00520:         // Point 73: Full Campaign Progression Passes Through All 10 Windows in Sequence
00521:         // ────────────────────────────────────────────────────────────────────────
00522:
00523:         [Fact]
00524:         public void Point_73_FullYearProgression_VisitsAllTenWindowsInChronologicalSequence()
00525:         {
00526:             var weather = CreateBoundWeather(seed: 54321);
00527:             var visitedWindows = new List<string>();
00528:
00529:             string? lastWindowId = null;
00530:             for (int day = 0; day <= 365; day++)
00531:             {
00532:                 var currentWindow = weather.GetSeasonForDay(day);
00533:                 if (currentWindow.id != lastWindowId)
00534:                 {
00535:                     visitedWindows.Add(currentWindow.id);
00536:                     lastWindowId = currentWindow.id;
00537:                 }
00538:             }
00539:
00540:             var expectedSequence = new[]
00541:             {
00542:                 "window_first_thaw",
00543:                 "window_ash_settling",
00544:                 "window_deep_freeze",
00545:                 "window_spring_storms",
00546:                 "window_dry_ash",
00547:                 "window_first_fallout",
00548:                 "window_false_spring",
00549:                 "window_deep_ash",
00550:                 "window_long_winter",
00551:                 "window_black_rain_season"
00552:             };
00553:
00554:             Assert.Equal(expectedSequence, visitedWindows);
00555:         }
00556:
00557:         // ────────────────────────────────────────────────────────────────────────
00558:         // Point 74: Self-Test / Verification Preconditions Pass
00559:         // ────────────────────────────────────────────────────────────────────────
00560:
00561:         [Fact]
00562:         public void Point_74_WeatherSystem_CatalogAndRuntimeIntegrity_PreconditionsHold()
00563:         {
00564:             var profile = LoadProfile();
00565:             Assert.Equal(10, profile.seasons.Count);
00566:             Assert.True(profile.weatherCheckIntervalHours > 0.0f);
00567:             Assert.Equal("default_winter", profile.id);
00568:         }
00569:     }
00570: }
```

## `Ashfall.Core.Tests/Campaign/Plan83_74WeatherNarrativeIntegrationTests.cs` — 154 lines; 6,890 bytes; SHA-256 `d44391337ba6f9fcd3edd54061f1273c8e6c8c0d877c4ce55019d41c652fc4e1`
Declaration index:
- 00016: public sealed class Plan83_74WeatherNarrativeIntegrationTests
- 00018: private static string FindDataDir()
- 00035: public void WeatherSeasons_HasTenWindowsWithStrictlyIncreasingStartDays()
- 00072: public void NarrativeProgression_HasFifteenChaptersWithContiguousOrder()
- 00095: public void CrossSystem_BothCatalogsLoadIndependentlyAndProvideFullCampaignCoverage()
- 00124: public void CrossSystem_WeatherAndNarrativeReflectCampaignEvolutionTones()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // Plan 83 × Plan 74 cross-system integration test
00003: // Plan 83 — Weather Season Windows Expansion (3 → 10 season windows)
00004: // Plan 74 — Narrative Progression Chapters Expansion (5 → 15 campaign chapters)
00005: using System;
00006: using System.Collections.Generic;
00007: using System.IO;
00008: using System.Linq;
00009: using Ashfall.Core.IO;
00010: using Ashfall.Core.Narrative;
00011: using Ashfall.Core.World;
00012: using Xunit;
00013:
00014: namespace Ashfall.Core.Tests.Campaign
00015: {
00016:     public sealed class Plan83_74WeatherNarrativeIntegrationTests
00017:     {
00018:         private static string FindDataDir()
00019:         {
00020:             string search = Directory.GetCurrentDirectory();
00021:             for (int i = 0; i < 6; i++)
00022:             {
00023:                 string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
00024:                 if (Directory.Exists(candidate)) return candidate;
00025:                 string parent = Directory.GetParent(search)?.FullName;
00026:                 if (parent == null) break;
00027:                 search = parent;
00028:             }
00029:             return string.Empty;
00030:         }
00031:
00032:         // ── Weather Seasons (Plan 83) ──────────────────────────────────────────
00033:
00034:         [Fact]
00035:         public void WeatherSeasons_HasTenWindowsWithStrictlyIncreasingStartDays()
00036:         {
00037:             string dataDir = FindDataDir();
00038:             if (string.IsNullOrEmpty(dataDir)) return;
00039:
00040:             var io = new FileSystemIO();
00041:             var json = new SystemTextJsonSerializer();
00042:             var profile = WeatherProfileLoader.Load(dataDir, io, json);
00043:
00044:             Assert.NotNull(profile);
00045:             Assert.Equal(10, profile.seasons.Count);
00046:
00047:             var ids = new HashSet<string>();
00048:             int lastStartDay = -1;
00049:
00050:             foreach (var season in profile.seasons)
00051:             {
00052:                 Assert.False(string.IsNullOrEmpty(season.id), "Season has empty id");
00053:                 Assert.True(ids.Add(season.id), $"Duplicate season id: {season.id}");
00054:                 Assert.True(season.startDay > lastStartDay,
00055:                     $"Season '{season.id}' startDay {season.startDay} not strictly greater than previous {lastStartDay}");
00056:                 lastStartDay = season.startDay;
00057:
00058:                 // Weights within reasonable bounds (0.0 .. 5.0)
00059:                 Assert.True(season.clearWeight >= 0f && season.clearWeight <= 5f);
00060:                 Assert.True(season.rainWeight >= 0f && season.rainWeight <= 5f);
00061:                 Assert.True(season.overcastWeight >= 0f && season.overcastWeight <= 5f);
00062:                 Assert.True(season.ashfallWeight >= 0f && season.ashfallWeight <= 5f);
00063:                 Assert.True(season.falloutStormWeight >= 0f && season.falloutStormWeight <= 5f);
00064:                 Assert.True(season.blizzardWeight >= 0f && season.blizzardWeight <= 5f);
00065:                 Assert.True(season.blackRainWeight >= 0f && season.blackRainWeight <= 5f);
00066:             }
00067:         }
00068:
00069:         // ── Narrative Progression (Plan 74) ─────────────────────────────────────
00070:
00071:         [Fact]
00072:         public void NarrativeProgression_HasFifteenChaptersWithContiguousOrder()
00073:         {
00074:             string dataDir = FindDataDir();
00075:             if (string.IsNullOrEmpty(dataDir)) return;
00076:
00077:             var io = new FileSystemIO();
00078:             var json = new SystemTextJsonSerializer();
00079:             var chapters = NarrativeProgressionCatalogLoader.Load(dataDir, io, json);
00080:
00081:             Assert.Equal(15, chapters.Count);
00082:
00083:             var orders = chapters.Select(c => c.order).OrderBy(o => o).ToList();
00084:             for (int i = 0; i < 15; i++)
00085:             {
00086:                 Assert.Equal(i + 1, orders[i]);
00087:                 Assert.False(string.IsNullOrWhiteSpace(chapters[i].description),
00088:                     $"Chapter at index {i} has empty description");
00089:             }
00090:         }
00091:
00092:         // ── Cross-System Coherence ──────────────────────────────────────────────
00093:
00094:         [Fact]
00095:         public void CrossSystem_BothCatalogsLoadIndependentlyAndProvideFullCampaignCoverage()
00096:         {
00097:             string dataDir = FindDataDir();
00098:             if (string.IsNullOrEmpty(dataDir)) return;
00099:
00100:             var io = new FileSystemIO();
00101:             var json = new SystemTextJsonSerializer();
00102:
00103:             var weatherProfile = WeatherProfileLoader.Load(dataDir, io, json);
00104:             var chapters = NarrativeProgressionCatalogLoader.Load(dataDir, io, json);
00105:
00106:             Assert.NotNull(weatherProfile);
00107:             Assert.True(weatherProfile.seasons.Count >= 10,
00108:                 $"Weather catalog must contain >= 10 season windows; got {weatherProfile.seasons.Count}");
00109:
00110:             Assert.True(chapters.Count >= 15,
00111:                 $"Narrative progression must contain >= 15 chapters; got {chapters.Count}");
00112:
00113:             // Weather seasons start at day 0 and extend to day 280+
00114:             Assert.Equal(0, weatherProfile.seasons.First().startDay);
00115:             Assert.True(weatherProfile.seasons.Last().startDay >= 240,
00116:                 "Final weather season window should start at or after day 240");
00117:
00118:             // Narrative progression starts at Chapter 1 and concludes at Chapter 15
00119:             Assert.Equal(1, chapters.Min(c => c.order));
00120:             Assert.Equal(15, chapters.Max(c => c.order));
00121:         }
00122:
00123:         [Fact]
00124:         public void CrossSystem_WeatherAndNarrativeReflectCampaignEvolutionTones()
00125:         {
00126:             string dataDir = FindDataDir();
00127:             if (string.IsNullOrEmpty(dataDir)) return;
00128:
00129:             var io = new FileSystemIO();
00130:             var json = new SystemTextJsonSerializer();
00131:
00132:             var weatherProfile = WeatherProfileLoader.Load(dataDir, io, json);
00133:             var chapters = NarrativeProgressionCatalogLoader.Load(dataDir, io, json);
00134:
00135:             // Early game: First Thaw weather exists alongside Chapter 1 (The Exchange) / Chapter 2 (Ashfall)
00136:             var firstThaw = weatherProfile.seasons.FirstOrDefault(s => s.id == "window_first_thaw");
00137:             Assert.NotNull(firstThaw);
00138:
00139:             var chapter1 = chapters.FirstOrDefault(c => c.order == 1);
00140:             Assert.NotNull(chapter1);
00141:             Assert.Contains("The Exchange", chapter1.description);
00142:
00143:             // Late game: Black Rain Season weather exists alongside Chapter 14 (The Muster) / Chapter 15 (The Inheritance)
00144:             var blackRainSeason = weatherProfile.seasons.FirstOrDefault(s => s.id == "window_black_rain_season");
00145:             Assert.NotNull(blackRainSeason);
00146:             Assert.True(blackRainSeason.blackRainWeight >= 2.0f);
00147:
00148:             var chapter15 = chapters.FirstOrDefault(c => c.order == 15);
00149:             Assert.NotNull(chapter15);
00150:             Assert.Contains("The Inheritance", chapter15.description);
00151:         }
00152:
00153:     }
00154: }
```

## `Ashfall.Core.Tests/World/WeatherGateCatalogIntegrityTests.cs` — 579 lines; 25,599 bytes; SHA-256 `c4bfefad2b8841510cf3e1c1f96b32a0467613ee408636779f6a5bebd4f9420a`
Declaration index:
- 00021: public sealed class WeatherGateCatalogIntegrityTests
- 00043: public void F14_CatalogLoads_Exactly18Gates()
- 00051: public void F14_DomainCatalogLoads_AllGatesRegistered()
- 00059: public void F14_AllGateIdsAreUnique()
- 00070: public void F14_AllRouteGateTargetsResolve()
- 00104: public void F14_AllWeatherKindsAreCanonical()
- 00131: public void F14_AllOverrideItemsResolve()
- 00155: public void F16_NoGateHasRequiredBlockedOverlap()
- 00170: public void F16_SevenRollableWeatherStates_AllHavePositiveWeights()
- 00190: public void F16_WeatherGateTruthTable_WeatherRowsAllGatesCorrect()
- 00242: public void F16_PositiveGates_OpenOnlyDuringRequiredWeather()
- 00272: public void F16_NegativeGates_BlockedOnlyDuringBlockedWeather()
- 00298: public void F16_OverrideMatrix_CorrectBehavior()
- 00330: public void F16_RadioTransitions_EmitOnStateChange_NotOnSameState()
- 00359: public void F14_360DaySimulation_WeatherDistribution()
- 00388: public void F14_PerGateUtilization_Calculated()
- 00412: public void F14_DeadGateDetection_BioFogGatesAreDead()
- 00428: public void F14_DeadGateDetection_EMPGateIsDead()
- 00439: public void F14_BlizzardGates_HighBlockedRate_InColdSeasons()
- 00459: public void F14_NoRedundantGates_SameTargetSameWeather()
- 00496: public void F14_NoOrphanGates_AllTargetsExistInData()
- 00526: public void F16_DeterminismLinkage_SameSeedSameGates()
- 00545: private sealed class TradeCaravanRouteEnvelope
- 00551: private sealed class TradeCaravanRouteDef
- 00557: private sealed class ExpeditionEnvelope
- 00563: private sealed class ExpeditionDef
- 00568: private sealed class ItemCatalogEnvelope
- 00574: private sealed class ItemDef
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text;
00007: using Ashfall.Core;
00008: using Ashfall.Core.IO;
00009: using Ashfall.Core.World;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.World
00013: {
00014:     /// <summary>
00015:     /// F14 — Weather Gate Catalog Integrity &amp; Content Utilization Audit.
00016:     /// F16 — Full Weather Gate Regression Matrix (catalog portion).
00017:     /// Proves every gate is structurally valid, targets resolve, weather
00018:     /// names are canonical, override items exist, and the full weather ×
00019:     /// gate truth table is correct.
00020:     /// </summary>
00021:     public sealed class WeatherGateCatalogIntegrityTests
00022:     {
00023:         private readonly string _dataDir;
00024:         private readonly IFileIO _fileIO;
00025:         private readonly WeatherRouteGateCatalog _routeCatalog;
00026:         private readonly WeatherGateCatalog _domainCatalog;
00027:         private readonly WeatherGateEvaluator _evaluator;
00028:
00029:         public WeatherGateCatalogIntegrityTests()
00030:         {
00031:             _dataDir = WeatherGateAuditSimulator.FindDataDir();
00032:             _fileIO = new FileSystemIO();
00033:             _routeCatalog = WeatherRouteGateCatalog.LoadFromDirectory(_dataDir, _fileIO);
00034:             _domainCatalog = new WeatherGateCatalog();
00035:             foreach (var def in _routeCatalog.Gates)
00036:                 _domainCatalog.Register(WeatherGateEvaluator.FromDef(def));
00037:             _evaluator = new WeatherGateEvaluator(_domainCatalog);
00038:         }
00039:
00040:         // ── F14.1 / F16.1 Catalog load ────────────────────────────────
00041:
00042:         [Fact]
00043:         public void F14_CatalogLoads_Exactly18Gates()
00044:         {
00045:             Assert.Equal(18, _routeCatalog.Gates.Count);
00046:             Assert.Equal(15, _routeCatalog.Gates.Count(g => g.gate_type == "route"));
00047:             Assert.Equal(3, _routeCatalog.Gates.Count(g => g.gate_type == "destination"));
00048:         }
00049:
00050:         [Fact]
00051:         public void F14_DomainCatalogLoads_AllGatesRegistered()
00052:         {
00053:             Assert.Equal(18, _domainCatalog.Count);
00054:         }
00055:
00056:         // ── F14.2 / F16.2 Unique IDs ──────────────────────────────────
00057:
00058:         [Fact]
00059:         public void F14_AllGateIdsAreUnique()
00060:         {
00061:             var ids = _routeCatalog.Gates.Select(g => g.id).ToList();
00062:             var unique = ids.Distinct(StringComparer.Ordinal).ToList();
00063:             Assert.Equal(ids.Count, unique.Count);
00064:             Assert.All(ids, id => Assert.False(string.IsNullOrWhiteSpace(id)));
00065:         }
00066:
00067:         // ── F14.3 / F16.3 Target closure ──────────────────────────────
00068:
00069:         [Fact]
00070:         public void F14_AllRouteGateTargetsResolve()
00071:         {
00072:             // Load caravan routes
00073:             string caravanPath = Path.Combine(_dataDir, "narrative", "wasteland_trade_caravan_routes.json");
00074:             Assert.True(File.Exists(caravanPath), "wasteland_trade_caravan_routes.json not found");
00075:             var caravanJson = _fileIO.ReadAllText(caravanPath);
00076:             var caravanData = new SystemTextJsonSerializer().Deserialize<TradeCaravanRouteEnvelope>(caravanJson);
00077:             var routeIds = new HashSet<string>(
00078:                 caravanData?.routes?.Select(r => !string.IsNullOrEmpty(r.route_id) ? r.route_id : r.id) ?? Enumerable.Empty<string>(),
00079:                 StringComparer.Ordinal);
00080:
00081:             // Load expedition destinations
00082:             string expedPath = Path.Combine(_dataDir, "expeditions.json");
00083:             Assert.True(File.Exists(expedPath), "expeditions.json not found");
00084:             var expedJson = _fileIO.ReadAllText(expedPath);
00085:             var expedData = new SystemTextJsonSerializer().Deserialize<ExpeditionEnvelope>(expedJson);
00086:             var destIds = new HashSet<string>(
00087:                 expedData?.expeditions?.Select(e => e.id) ?? Enumerable.Empty<string>(),
00088:                 StringComparer.Ordinal);
00089:
00090:             var unresolved = new List<string>();
00091:             foreach (var gate in _routeCatalog.Gates)
00092:             {
00093:                 bool found = routeIds.Contains(gate.target) || destIds.Contains(gate.target);
00094:                 if (!found) unresolved.Add($"{gate.id} -> {gate.target}");
00095:             }
00096:
00097:             Assert.True(unresolved.Count == 0,
00098:                 $"Unresolved gate targets:\n{string.Join("\n", unresolved)}");
00099:         }
00100:
00101:         // ── F14.4 / F16.4 Weather-kind closure ────────────────────────
00102:
00103:         [Fact]
00104:         public void F14_AllWeatherKindsAreCanonical()
00105:         {
00106:             var knownKinds = new HashSet<string>(
00107:                 Enum.GetNames(typeof(WeatherKind)), StringComparer.Ordinal);
00108:
00109:             var violations = new List<string>();
00110:             foreach (var gate in _routeCatalog.Gates)
00111:             {
00112:                 foreach (var w in (gate.blocked_weather ?? new List<string>()))
00113:                 {
00114:                     if (!knownKinds.Contains(w))
00115:                         violations.Add($"{gate.id} blocked_weather: '{w}'");
00116:                 }
00117:                 foreach (var w in (gate.required_weather ?? new List<string>()))
00118:                 {
00119:                     if (!knownKinds.Contains(w))
00120:                         violations.Add($"{gate.id} required_weather: '{w}'");
00121:                 }
00122:             }
00123:
00124:             Assert.True(violations.Count == 0,
00125:                 $"Non-canonical weather kinds:\n{string.Join("\n", violations)}");
00126:         }
00127:
00128:         // ── F14.5 / F16.5 Override-item closure ───────────────────────
00129:
00130:         [Fact]
00131:         public void F14_AllOverrideItemsResolve()
00132:         {
00133:             string itemsPath = Path.Combine(_dataDir, "items.json");
00134:             Assert.True(File.Exists(itemsPath), "items.json not found");
00135:             var itemsJson = _fileIO.ReadAllText(itemsPath);
00136:             var itemsData = new SystemTextJsonSerializer().Deserialize<ItemCatalogEnvelope>(itemsJson);
00137:             var itemIds = new HashSet<string>(
00138:                 itemsData?.items?.Select(i => i.id) ?? Enumerable.Empty<string>(),
00139:                 StringComparer.Ordinal);
00140:
00141:             var missing = new List<string>();
00142:             foreach (var gate in _routeCatalog.Gates)
00143:             {
00144:                 if (!string.IsNullOrEmpty(gate.override_item) && !itemIds.Contains(gate.override_item))
00145:                     missing.Add($"{gate.id} override_item: '{gate.override_item}'");
00146:             }
00147:
00148:             Assert.True(missing.Count == 0,
00149:                 $"Missing override items:\n{string.Join("\n", missing)}");
00150:         }
00151:
00152:         // ── F16.6 No required/blocked overlap ──────────────────────────
00153:
00154:         [Fact]
00155:         public void F16_NoGateHasRequiredBlockedOverlap()
00156:         {
00157:             foreach (var gate in _routeCatalog.Gates)
00158:             {
00159:                 var blocked = new HashSet<string>(gate.blocked_weather ?? new List<string>(), StringComparer.Ordinal);
00160:                 var required = new HashSet<string>(gate.required_weather ?? new List<string>(), StringComparer.Ordinal);
00161:                 blocked.IntersectWith(required);
00162:                 Assert.True(blocked.Count == 0,
00163:                     $"Gate {gate.id} has required/blocked overlap: {string.Join(", ", blocked)}");
00164:             }
00165:         }
00166:
00167:         // ── F16.7 Rollable weather coverage ────────────────────────────
00168:
00169:         [Fact]
00170:         public void F16_SevenRollableWeatherStates_AllHavePositiveWeights()
00171:         {
00172:             string profilePath = Path.Combine(_dataDir, "weather_seasons.json");
00173:             var profile = WeatherProfileLoader.Load(_dataDir, _fileIO, new SystemTextJsonSerializer());
00174:             Assert.NotNull(profile);
00175:
00176:             var rollableKinds = new[] { "Clear", "Rain", "Overcast", "Ashfall", "FalloutStorm", "Blizzard", "BlackRain" };
00177:
00178:             foreach (var season in profile!.seasons)
00179:             {
00180:                 float total = season.clearWeight + season.rainWeight + season.overcastWeight +
00181:                               season.ashfallWeight + season.falloutStormWeight +
00182:                               season.blizzardWeight + season.blackRainWeight;
00183:                 Assert.True(total > 0, $"Season {season.id} has zero total weight");
00184:             }
00185:         }
00186:
00187:         // ── F16.8 Weather × gate truth table ───────────────────────────
00188:
00189:         [Fact]
00190:         public void F16_WeatherGateTruthTable_WeatherRowsAllGatesCorrect()
00191:         {
00192:             var failures = new List<string>();
00193:
00194:             foreach (WeatherKind weather in new[]
00195:             {
00196:                 WeatherKind.Clear,
00197:                 WeatherKind.Rain,
00198:                 WeatherKind.Overcast,
00199:                 WeatherKind.Ashfall,
00200:                 WeatherKind.FalloutStorm,
00201:                 WeatherKind.Blizzard,
00202:                 WeatherKind.BlackRain,
00203:             })
00204:             {
00205:
00206:                 foreach (var gateDef in _routeCatalog.Gates)
00207:                 {
00208:                     var domain = WeatherGateEvaluator.FromDef(gateDef);
00209:                     var state = WeatherGateEvaluator.EvaluateGateStatic(domain, weather);
00210:                     var exception = Record.Exception(() =>
00211:                     {
00212:                         // Verify positive gates
00213:                         if (gateDef.required_weather != null && gateDef.required_weather.Count > 0 &&
00214:                             (gateDef.blocked_weather == null || gateDef.blocked_weather.Count == 0))
00215:                         {
00216:                             bool requiredMatch = gateDef.required_weather.Contains(weather.ToString());
00217:                             Assert.Equal(requiredMatch, state.IsOpen);
00218:                             Assert.True(state.IsPositiveGate);
00219:                         }
00220:                         // Verify negative gates
00221:                         else if (gateDef.blocked_weather != null && gateDef.blocked_weather.Count > 0 &&
00222:                                  (gateDef.required_weather == null || gateDef.required_weather.Count == 0))
00223:                         {
00224:                             bool blockedMatch = gateDef.blocked_weather.Contains(weather.ToString());
00225:                             Assert.Equal(!blockedMatch, state.IsOpen);
00226:                         }
00227:                     });
00228:
00229:                     if (exception != null)
00230:                     {
00231:                         failures.Add($"weather={weather}: {exception.Message}");
00232:                     }
00233:                 }
00234:             }
00235:
00236:             Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
00237:         }
00238:
00239:         // ── F16.9 Positive gate contract ───────────────────────────────
00240:
00241:         [Fact]
00242:         public void F16_PositiveGates_OpenOnlyDuringRequiredWeather()
00243:         {
00244:             var positiveGates = _routeCatalog.Gates
00245:                 .Where(g => g.required_weather != null && g.required_weather.Count > 0 &&
00246:                             (g.blocked_weather == null || g.blocked_weather.Count == 0))
00247:                 .ToList();
00248:
00249:             Assert.True(positiveGates.Count > 0, "No positive gates found");
00250:
00251:             foreach (var gateDef in positiveGates)
00252:             {
00253:                 var allKinds = Enum.GetValues<WeatherKind>();
00254:                 foreach (var kind in allKinds)
00255:                 {
00256:                     var domain = WeatherGateEvaluator.FromDef(gateDef);
00257:                     var state = WeatherGateEvaluator.EvaluateGateStatic(domain, kind);
00258:
00259:                     bool shouldOpen = gateDef.required_weather!.Contains(kind.ToString());
00260:                     Assert.Equal(shouldOpen, state.IsOpen);
00261:                     if (!shouldOpen)
00262:                     {
00263:                         Assert.Equal("required_weather_not_matched", state.Reason);
00264:                     }
00265:                 }
00266:             }
00267:         }
00268:
00269:         // ── F16.10 Negative gate contract ──────────────────────────────
00270:
00271:         [Fact]
00272:         public void F16_NegativeGates_BlockedOnlyDuringBlockedWeather()
00273:         {
00274:             var negativeGates = _routeCatalog.Gates
00275:                 .Where(g => g.blocked_weather != null && g.blocked_weather.Count > 0 &&
00276:                             (g.required_weather == null || g.required_weather.Count == 0))
00277:                 .ToList();
00278:
00279:             Assert.True(negativeGates.Count > 0, "No negative gates found");
00280:
00281:             foreach (var gateDef in negativeGates)
00282:             {
00283:                 var allKinds = Enum.GetValues<WeatherKind>();
00284:                 foreach (var kind in allKinds)
00285:                 {
00286:                     var domain = WeatherGateEvaluator.FromDef(gateDef);
00287:                     var state = WeatherGateEvaluator.EvaluateGateStatic(domain, kind);
00288:
00289:                     bool shouldBlock = gateDef.blocked_weather!.Contains(kind.ToString());
00290:                     Assert.Equal(!shouldBlock, state.IsOpen);
00291:                 }
00292:             }
00293:         }
00294:
00295:         // ── F16.11 Override matrix ─────────────────────────────────────
00296:
00297:         [Fact]
00298:         public void F16_OverrideMatrix_CorrectBehavior()
00299:         {
00300:             var overrideGates = _routeCatalog.Gates
00301:                 .Where(g => !string.IsNullOrEmpty(g.override_item))
00302:                 .ToList();
00303:
00304:             Assert.Equal(4, overrideGates.Count); // gas_mask x2, hazmat_suit x2
00305:
00306:             foreach (var gateDef in overrideGates)
00307:             {
00308:                 var blockedKind = gateDef.blocked_weather![0];
00309:
00310:                 // No inventory → blocked
00311:                 Assert.True(WeatherRouteGateCatalog.IsGateBlocking(gateDef, blockedKind, _ => false));
00312:
00313:                 // Unrelated item → still blocked
00314:                 Assert.True(WeatherRouteGateCatalog.IsGateBlocking(gateDef, blockedKind,
00315:                     item => item == "unrelated_item"));
00316:
00317:                 // Correct override → not blocked
00318:                 Assert.False(WeatherRouteGateCatalog.IsGateBlocking(gateDef, blockedKind,
00319:                     item => item == gateDef.override_item));
00320:
00321:                 // Open weather → not blocked regardless of inventory
00322:                 Assert.False(WeatherRouteGateCatalog.IsGateBlocking(gateDef, "Clear", _ => false));
00323:                 Assert.False(WeatherRouteGateCatalog.IsGateBlocking(gateDef, "Clear", _ => true));
00324:             }
00325:         }
00326:
00327:         // ── F16.12 Radio transition behavior ───────────────────────────
00328:
00329:         [Fact]
00330:         public void F16_RadioTransitions_EmitOnStateChange_NotOnSameState()
00331:         {
00332:             var catalog = new WeatherGateCatalog();
00333:             catalog.Register(new WeatherGate
00334:             {
00335:                 Id = "gate_test_blizzard", TargetId = "route_test",
00336:                 BlockedWeather = new List<string> { "Blizzard" }
00337:             });
00338:             var evaluator = new WeatherGateEvaluator(catalog);
00339:             var hooks = new WeatherGateRadioHooks(evaluator);
00340:
00341:             // Clear → Blizzard: should produce a closure transition
00342:             var transitions = evaluator.CompareWeatherStates(WeatherKind.Clear, WeatherKind.Blizzard);
00343:             Assert.Single(transitions);
00344:             Assert.True(transitions[0].WasOpen);
00345:             Assert.False(transitions[0].IsOpen);
00346:
00347:             // Same state → no transition
00348:             var noTransitions = evaluator.CompareWeatherStates(WeatherKind.Blizzard, WeatherKind.Blizzard);
00349:             Assert.Empty(noTransitions);
00350:
00351:             // Unrelated change → no transition for blizzard gate
00352:             var unrelated = evaluator.CompareWeatherStates(WeatherKind.Rain, WeatherKind.Overcast);
00353:             Assert.Empty(unrelated);
00354:         }
00355:
00356:         // ── F14.6–F14.8 360-day weather simulation & utilization ───────
00357:
00358:         [Fact]
00359:         public void F14_360DaySimulation_WeatherDistribution()
00360:         {
00361:             var timeline = WeatherGateAuditSimulator.BuildTimeline(
00362:                 WeatherGateAuditSimulator.AuditSeed, WeatherGateAuditSimulator.CampaignDays);
00363:
00364:             Assert.Equal(360, timeline.Count);
00365:
00366:             var freq = new Dictionary<WeatherKind, int>();
00367:             foreach (var day in timeline)
00368:             {
00369:                 if (!freq.ContainsKey(day.Weather)) freq[day.Weather] = 0;
00370:                 freq[day.Weather]++;
00371:             }
00372:
00373:             // All 7 rollable kinds should appear in 360 days
00374:             Assert.True(freq.ContainsKey(WeatherKind.Clear), "Clear never appeared");
00375:             Assert.True(freq.ContainsKey(WeatherKind.Rain), "Rain never appeared");
00376:             Assert.True(freq.ContainsKey(WeatherKind.Overcast), "Overcast never appeared");
00377:             Assert.True(freq.ContainsKey(WeatherKind.Ashfall), "Ashfall never appeared");
00378:             Assert.True(freq.ContainsKey(WeatherKind.FalloutStorm), "FalloutStorm never appeared");
00379:             Assert.True(freq.ContainsKey(WeatherKind.Blizzard), "Blizzard never appeared");
00380:             Assert.True(freq.ContainsKey(WeatherKind.BlackRain), "BlackRain never appeared");
00381:
00382:             // Non-rollable kinds should NOT appear
00383:             Assert.False(freq.ContainsKey(WeatherKind.BioFog), "BioFog appeared but has zero weight");
00384:             Assert.False(freq.ContainsKey(WeatherKind.EMPStorm), "EMPStorm appeared but has zero weight");
00385:         }
00386:
00387:         [Fact]
00388:         public void F14_PerGateUtilization_Calculated()
00389:         {
00390:             var sim = new WeatherGateAuditSimulator(_dataDir);
00391:             var stats = sim.CalculateUtilization();
00392:
00393:             Assert.Equal(18, stats.Count);
00394:
00395:             foreach (var s in stats)
00396:             {
00397:                 Assert.True(s.BlockedDays + s.OpenDays == 360,
00398:                     $"Gate {s.GateId}: {s.BlockedDays}+{s.OpenDays} != 360");
00399:
00400:                 // Dead gate heuristic: <5% trigger
00401:                 if (s.BlockedPct < 5.0 && s.BlockedWeather.Count > 0)
00402:                 {
00403:                     // This is a finding, not a failure — log it
00404:                     Assert.True(true, $"Gate {s.GateId} blocked <5% ({s.BlockedPct:F1}%) — potentially dead");
00405:                 }
00406:             }
00407:         }
00408:
00409:         // ── F14.9–F14.10 Dead/restrictive gate flags ───────────────────
00410:
00411:         [Fact]
00412:         public void F14_DeadGateDetection_BioFogGatesAreDead()
00413:         {
00414:             var sim = new WeatherGateAuditSimulator(_dataDir);
00415:             var stats = sim.CalculateUtilization();
00416:
00417:             // BioFog gates should be dead (0% blocked) since BioFog has zero season weight
00418:             var bioFogGates = stats.Where(s => s.BlockedWeather.Contains("BioFog")).ToList();
00419:             Assert.Equal(3, bioFogGates.Count);
00420:             foreach (var g in bioFogGates)
00421:             {
00422:                 Assert.Equal(0, g.BlockedDays);
00423:                 Assert.Equal(360, g.OpenDays);
00424:             }
00425:         }
00426:
00427:         [Fact]
00428:         public void F14_DeadGateDetection_EMPGateIsDead()
00429:         {
00430:             var sim = new WeatherGateAuditSimulator(_dataDir);
00431:             var stats = sim.CalculateUtilization();
00432:
00433:             var empGate = stats.First(s => s.GateId == "gate_electronics_route_emp");
00434:             Assert.Equal(0, empGate.BlockedDays);
00435:             Assert.Equal(360, empGate.OpenDays);
00436:         }
00437:
00438:         [Fact]
00439:         public void F14_BlizzardGates_HighBlockedRate_InColdSeasons()
00440:         {
00441:             var sim = new WeatherGateAuditSimulator(_dataDir);
00442:             var stats = sim.CalculateUtilization();
00443:
00444:             // Blizzard gates should have meaningful blocked rates
00445:             var blizzardGates = stats.Where(s =>
00446:                 s.BlockedWeather.Count == 1 && s.BlockedWeather[0] == "Blizzard").ToList();
00447:             Assert.Equal(4, blizzardGates.Count);
00448:
00449:             foreach (var g in blizzardGates)
00450:             {
00451:                 Assert.True(g.BlockedPct > 5.0,
00452:                     $"Blizzard gate {g.GateId} blocked only {g.BlockedPct:F1}% — expected >5%");
00453:             }
00454:         }
00455:
00456:         // ── F14.11 Redundancy detection ────────────────────────────────
00457:
00458:         [Fact]
00459:         public void F14_NoRedundantGates_SameTargetSameWeather()
00460:         {
00461:             var groups = _routeCatalog.Gates
00462:                 .GroupBy(g => g.target, StringComparer.Ordinal)
00463:                 .Where(grp => grp.Count() > 1)
00464:                 .ToList();
00465:
00466:             foreach (var group in groups)
00467:             {
00468:                 var normalized = group.Select(g =>
00469:                 {
00470:                     var blocked = new HashSet<string>(g.blocked_weather ?? new List<string>(), StringComparer.Ordinal);
00471:                     var required = new HashSet<string>(g.required_weather ?? new List<string>(), StringComparer.Ordinal);
00472:                     return (g.id, blocked, required);
00473:                 }).ToList();
00474:
00475:                 // If all gates in the group have identical weather sets, flag as redundant
00476:                 for (int i = 0; i < normalized.Count; i++)
00477:                 {
00478:                     for (int j = i + 1; j < normalized.Count; j++)
00479:                     {
00480:                         bool sameBlocked = normalized[i].blocked.SetEquals(normalized[j].blocked);
00481:                         bool sameRequired = normalized[i].required.SetEquals(normalized[j].required);
00482:                         // This is informational — currently no duplicates exist
00483:                         if (sameBlocked && sameRequired)
00484:                         {
00485:                             Assert.Fail($"Redundant gates: {normalized[i].id} and {normalized[j].id} " +
00486:                                         $"target same route with identical weather sets");
00487:                         }
00488:                     }
00489:                 }
00490:             }
00491:         }
00492:
00493:         // ── F14.12 Orphan detection ────────────────────────────────────
00494:
00495:         [Fact]
00496:         public void F14_NoOrphanGates_AllTargetsExistInData()
00497:         {
00498:             // This is the same as F14_AllRouteGateTargetsResolve but also checks
00499:             // that every gate target appears in at least one consumer catalog
00500:             string caravanPath = Path.Combine(_dataDir, "narrative", "wasteland_trade_caravan_routes.json");
00501:             var caravanJson = _fileIO.ReadAllText(caravanPath);
00502:             var caravanData = new SystemTextJsonSerializer().Deserialize<TradeCaravanRouteEnvelope>(caravanJson);
00503:             var routeIds = new HashSet<string>(
00504:                 caravanData?.routes?.Select(r => !string.IsNullOrEmpty(r.route_id) ? r.route_id : r.id) ?? Enumerable.Empty<string>(),
00505:                 StringComparer.Ordinal);
00506:
00507:             string expedPath = Path.Combine(_dataDir, "expeditions.json");
00508:             var expedJson = _fileIO.ReadAllText(expedPath);
00509:             var expedData = new SystemTextJsonSerializer().Deserialize<ExpeditionEnvelope>(expedJson);
00510:             var destIds = new HashSet<string>(
00511:                 expedData?.expeditions?.Select(e => e.id) ?? Enumerable.Empty<string>(),
00512:                 StringComparer.Ordinal);
00513:
00514:             foreach (var gate in _routeCatalog.Gates)
00515:             {
00516:                 bool inRoutes = routeIds.Contains(gate.target);
00517:                 bool inDestinations = destIds.Contains(gate.target);
00518:                 Assert.True(inRoutes || inDestinations,
00519:                     $"Orphan gate {gate.id}: target '{gate.target}' not in caravan routes or expedition destinations");
00520:             }
00521:         }
00522:
00523:         // ── F16.13 Determinism linkage (reuse F13 helpers) ─────────────
00524:
00525:         [Fact]
00526:         public void F16_DeterminismLinkage_SameSeedSameGates()
00527:         {
00528:             var sim = new WeatherGateAuditSimulator(_dataDir);
00529:             var timeline = sim.Timeline;
00530:
00531:             // Rebuild with same seed and verify identical
00532:             var timeline2 = WeatherGateAuditSimulator.BuildTimeline(
00533:                 WeatherGateAuditSimulator.AuditSeed, WeatherGateAuditSimulator.CampaignDays);
00534:
00535:             Assert.Equal(timeline.Count, timeline2.Count);
00536:             for (int i = 0; i < timeline.Count; i++)
00537:             {
00538:                 Assert.Equal(timeline[i].Weather, timeline2[i].Weather);
00539:                 Assert.Equal(timeline[i].SeasonId, timeline2[i].SeasonId);
00540:             }
00541:         }
00542:
00543:         // ── DTOs for JSON deserialization ──────────────────────────────
00544:
00545:         private sealed class TradeCaravanRouteEnvelope
00546:         {
00547:             public int schema_version { get; set; }
00548:             public List<TradeCaravanRouteDef>? routes { get; set; }
00549:         }
00550:
00551:         private sealed class TradeCaravanRouteDef
00552:         {
00553:             public string id { get; set; } = "";
00554:             public string route_id { get; set; } = "";
00555:         }
00556:
00557:         private sealed class ExpeditionEnvelope
00558:         {
00559:             public int schema_version { get; set; }
00560:             public List<ExpeditionDef>? expeditions { get; set; }
00561:         }
00562:
00563:         private sealed class ExpeditionDef
00564:         {
00565:             public string id { get; set; } = "";
00566:         }
00567:
00568:         private sealed class ItemCatalogEnvelope
00569:         {
00570:             public int schema_version { get; set; }
00571:             public List<ItemDef>? items { get; set; }
00572:         }
00573:
00574:         private sealed class ItemDef
00575:         {
00576:             public string id { get; set; } = "";
00577:         }
00578:     }
00579: }
```

## `Ashfall.Core.Tests/World/WeatherGateBalanceAuditTests.cs` — 336 lines; 14,110 bytes; SHA-256 `266c7b4c3a93cfc4bb4348cb74d1f37ac5f6d607222353750027a612b9228bf8`
Declaration index:
- 00020: public sealed class WeatherGateBalanceAuditTests
- 00034: public void F15_EveryGatedRouteHasAvailabilityStats()
- 00052: public void F15_SeasonalDistribution_Calculated()
- 00067: public void F15_PositiveGates_IceRoads_OpenDuringBlizzard()
- 00091: public void F15_OverrideCoverage_Measured()
- 00112: public void F15_EMPGateAudit_ZeroWeightConfirmed()
- 00126: public void F15_BioFogGateAudit_ZeroWeightConfirmed()
- 00146: public void F15_FalloutStormAnalysis_MeaningfulFrequency()
- 00168: public void F15_BlizzardAnalysis_HighConcentrationInColdSeasons()
- 00188: public void F15_BlizzardNetworkClosure_MaxSimultaneousBlocks()
- 00223: public void F15_BlackRainAnalysis_MidFrequency()
- 00242: public void F15_NetworkClosureMetrics_Calculated()
- 00257: public void F15_DestinationGates_HaveForcePassageCosts()
- 00275: public void F15_BalanceReport_Generated()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text;
00007: using Ashfall.Core;
00008: using Ashfall.Core.IO;
00009: using Ashfall.Core.World;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests.World
00013: {
00014:     /// <summary>
00015:     /// F15 — Weather Gate Balance Audit.
00016:     /// Measures whether weather gates produce meaningful planning choices
00017:     /// across the campaign rather than random frustration, permanent
00018:     /// denial, or negligible impact.
00019:     /// </summary>
00020:     public sealed class WeatherGateBalanceAuditTests
00021:     {
00022:         private readonly string _dataDir;
00023:         private readonly WeatherGateAuditSimulator _sim;
00024:
00025:         public WeatherGateBalanceAuditTests()
00026:         {
00027:             _dataDir = WeatherGateAuditSimulator.FindDataDir();
00028:             _sim = new WeatherGateAuditSimulator(_dataDir);
00029:         }
00030:
00031:         // ── F15.2 Route availability statistics ────────────────────────
00032:
00033:         [Fact]
00034:         public void F15_EveryGatedRouteHasAvailabilityStats()
00035:         {
00036:             var stats = _sim.CalculateUtilization();
00037:             Assert.Equal(18, stats.Count);
00038:
00039:             foreach (var s in stats)
00040:             {
00041:                 Assert.True(s.BlockedDays + s.OpenDays == 360,
00042:                     $"Gate {s.GateId}: days don't sum to 360");
00043:                 Assert.True(s.LongestBlockedRun >= 0);
00044:                 Assert.True(s.LongestOpenRun >= 0);
00045:                 Assert.True(s.Transitions >= 0);
00046:             }
00047:         }
00048:
00049:         // ── F15.4 Seasonal distribution ────────────────────────────────
00050:
00051:         [Fact]
00052:         public void F15_SeasonalDistribution_Calculated()
00053:         {
00054:             var seasonal = _sim.CalculateSeasonalDistribution();
00055:             Assert.Equal(18, seasonal.Count);
00056:
00057:             foreach (var gateEntry in seasonal)
00058:             {
00059:                 int totalDays = gateEntry.Value.Values.Sum(v => v.Total);
00060:                 Assert.Equal(360, totalDays);
00061:             }
00062:         }
00063:
00064:         // ── F15.5 Positive gate analysis ───────────────────────────────
00065:
00066:         [Fact]
00067:         public void F15_PositiveGates_IceRoads_OpenDuringBlizzard()
00068:         {
00069:             var stats = _sim.CalculateUtilization();
00070:
00071:             // gate_frozen_lake_crossing and gate_seasonal_ice_road are positive gates
00072:             var frozenLake = stats.First(s => s.GateId == "gate_frozen_lake_crossing");
00073:             var iceRoad = stats.First(s => s.GateId == "gate_seasonal_ice_road");
00074:
00075:             // Both should be open during Blizzard days
00076:             Assert.True(frozenLake.OpenDays > 0,
00077:                 $"Frozen lake crossing never opens ({frozenLake.OpenDays} days)");
00078:             Assert.True(iceRoad.OpenDays > 0,
00079:                 $"Ice road never opens ({iceRoad.OpenDays} days)");
00080:
00081:             // Open percentage should be in the meaningful range (5-35%)
00082:             Assert.True(frozenLake.OpenPct >= 5.0,
00083:                 $"Frozen lake open only {frozenLake.OpenPct:F1}% — too rare");
00084:             Assert.True(frozenLake.OpenPct <= 50.0,
00085:                 $"Frozen lake open {frozenLake.OpenPct:F1}% — may not feel exceptional");
00086:         }
00087:
00088:         // ── F15.6 Override coverage ────────────────────────────────────
00089:
00090:         [Fact]
00091:         public void F15_OverrideCoverage_Measured()
00092:         {
00093:             var stats = _sim.CalculateUtilization();
00094:
00095:             var negativeGates = stats.Where(s => s.RequiredWeather.Count == 0).ToList();
00096:             var withOverride = negativeGates.Where(s => !string.IsNullOrEmpty(s.OverrideItem)).ToList();
00097:             var withoutOverride = negativeGates.Where(s => string.IsNullOrEmpty(s.OverrideItem)).ToList();
00098:
00099:             // Expected: 4 gates with overrides (gas_mask x2, hazmat_suit x2)
00100:             Assert.Equal(4, withOverride.Count);
00101:             Assert.True(withoutOverride.Count > 0, "All negative gates have overrides — unusual");
00102:
00103:             // Verify override items are correct
00104:             Assert.All(withOverride, s =>
00105:                 Assert.True(s.OverrideItem == "gas_mask" || s.OverrideItem == "hazmat_suit",
00106:                     $"Gate {s.GateId} has unexpected override: {s.OverrideItem}"));
00107:         }
00108:
00109:         // ── F15.7 EMP gate audit ───────────────────────────────────────
00110:
00111:         [Fact]
00112:         public void F15_EMPGateAudit_ZeroWeightConfirmed()
00113:         {
00114:             var stats = _sim.CalculateUtilization();
00115:             var empGate = stats.First(s => s.GateId == "gate_electronics_route_emp");
00116:
00117:             // EMPStorm has zero weight in all seasons
00118:             Assert.Equal(0, empGate.BlockedDays);
00119:             Assert.Equal(360, empGate.OpenDays);
00120:             Assert.Equal(0.0, empGate.BlockedPct);
00121:         }
00122:
00123:         // ── F15.8 BioFog gate audit ────────────────────────────────────
00124:
00125:         [Fact]
00126:         public void F15_BioFogGateAudit_ZeroWeightConfirmed()
00127:         {
00128:             var stats = _sim.CalculateUtilization();
00129:             var bioFogGates = stats.Where(s => s.BlockedWeather.Contains("BioFog")).ToList();
00130:
00131:             Assert.Equal(3, bioFogGates.Count);
00132:             foreach (var g in bioFogGates)
00133:             {
00134:                 Assert.Equal(0, g.BlockedDays);
00135:                 Assert.Equal(360, g.OpenDays);
00136:             }
00137:
00138:             // 4 dead gates total (3 BioFog + 1 EMP) out of 18
00139:             int deadCount = stats.Count(s => s.BlockedDays == 0 && s.BlockedWeather.Count > 0);
00140:             Assert.Equal(4, deadCount);
00141:         }
00142:
00143:         // ── F15.9 FalloutStorm analysis ────────────────────────────────
00144:
00145:         [Fact]
00146:         public void F15_FalloutStormAnalysis_MeaningfulFrequency()
00147:         {
00148:             var stats = _sim.CalculateUtilization();
00149:             // 2 route gates + 1 destination gate block FalloutStorm
00150:             var falloutGates = stats.Where(s =>
00151:                 s.BlockedWeather.Contains("FalloutStorm")).ToList();
00152:
00153:             Assert.Equal(3, falloutGates.Count);
00154:
00155:             foreach (var g in falloutGates)
00156:             {
00157:                 // FalloutStorm has low but non-zero weights
00158:                 Assert.True(g.BlockedDays > 0,
00159:                     $"Fallout gate {g.GateId} never blocked — FalloutStorm may be too rare");
00160:                 Assert.True(g.BlockedPct < 60.0,
00161:                     $"Fallout gate {g.GateId} blocked {g.BlockedPct:F1}% — too restrictive");
00162:             }
00163:         }
00164:
00165:         // ── F15.10 Blizzard analysis ───────────────────────────────────
00166:
00167:         [Fact]
00168:         public void F15_BlizzardAnalysis_HighConcentrationInColdSeasons()
00169:         {
00170:             var stats = _sim.CalculateUtilization();
00171:             // 4 route + 1 destination gate block Blizzard
00172:             var blizzardGates = stats.Where(s =>
00173:                 s.BlockedWeather.Contains("Blizzard")).ToList();
00174:
00175:             Assert.Equal(5, blizzardGates.Count);
00176:
00177:             foreach (var g in blizzardGates)
00178:             {
00179:                 // Blizzard has high weights in Deep Freeze and Long Winter
00180:                 Assert.True(g.BlockedPct > 5.0,
00181:                     $"Blizzard gate {g.GateId} blocked only {g.BlockedPct:F1}%");
00182:                 Assert.True(g.LongestBlockedRun > 0,
00183:                     $"Blizzard gate {g.GateId} has zero longest blocked run");
00184:             }
00185:         }
00186:
00187:         [Fact]
00188:         public void F15_BlizzardNetworkClosure_MaxSimultaneousBlocks()
00189:         {
00190:             var timeline = _sim.Timeline;
00191:             var routeCatalog = _sim.RouteCatalog;
00192:
00193:             int maxSimultaneous = 0;
00194:             int worstDay = 0;
00195:
00196:             for (int day = 0; day < timeline.Count; day++)
00197:             {
00198:                 int blocked = 0;
00199:                 foreach (var gateDef in routeCatalog.Gates)
00200:                 {
00201:                     if (gateDef.gate_type == "route" && gateDef.blocked_weather != null && gateDef.blocked_weather.Contains("Blizzard"))
00202:                     {
00203:                         var domain = WeatherGateEvaluator.FromDef(gateDef);
00204:                         var state = WeatherGateEvaluator.EvaluateGateStatic(domain, timeline[day].Weather);
00205:                         if (!state.IsOpen) blocked++;
00206:                     }
00207:                 }
00208:                 if (blocked > maxSimultaneous)
00209:                 {
00210:                     maxSimultaneous = blocked;
00211:                     worstDay = day;
00212:                 }
00213:             }
00214:
00215:             // 4 route + 1 destination gate block Blizzard = 5 max
00216:             Assert.True(maxSimultaneous <= 5,
00217:                 $"More than 5 blizzard gates blocked simultaneously: {maxSimultaneous}");
00218:         }
00219:
00220:         // ── F15.11 BlackRain analysis ──────────────────────────────────
00221:
00222:         [Fact]
00223:         public void F15_BlackRainAnalysis_MidFrequency()
00224:         {
00225:             var stats = _sim.CalculateUtilization();
00226:             // 3 route gates + 1 destination gate block BlackRain
00227:             var blackRainGates = stats.Where(s =>
00228:                 s.BlockedWeather.Contains("BlackRain")).ToList();
00229:
00230:             Assert.Equal(4, blackRainGates.Count);
00231:
00232:             foreach (var g in blackRainGates)
00233:             {
00234:                 Assert.True(g.BlockedDays > 0,
00235:                     $"BlackRain gate {g.GateId} never blocked");
00236:             }
00237:         }
00238:
00239:         // ── F15.12 Network-level balance metrics ───────────────────────
00240:
00241:         [Fact]
00242:         public void F15_NetworkClosureMetrics_Calculated()
00243:         {
00244:             var (worstDay, maxBlocked, daysOver50, daysZeroOpen) = _sim.NetworkClosureMetrics();
00245:
00246:             Assert.True(maxBlocked <= 18, $"More than 18 gates blocked: {maxBlocked}");
00247:             Assert.True(worstDay >= 0 && worstDay < 360);
00248:
00249:             // Log findings (not failures — these are audit metrics)
00250:             // daysZeroOpen should be 0 since dead gates are always open
00251:             Assert.Equal(0, daysZeroOpen);
00252:         }
00253:
00254:         // ── F15.13 Destination gate force-passage analysis ─────────────
00255:
00256:         [Fact]
00257:         public void F15_DestinationGates_HaveForcePassageCosts()
00258:         {
00259:             var destGates = _sim.RouteCatalog.Gates
00260:                 .Where(g => g.gate_type == "destination")
00261:                 .ToList();
00262:
00263:             Assert.Equal(3, destGates.Count);
00264:
00265:             foreach (var g in destGates)
00266:             {
00267:                 Assert.True(g.force_stamina_cost > 0,
00268:                     $"Destination gate {g.id} has no force stamina cost");
00269:             }
00270:         }
00271:
00272:         // ── F15.14 Balance report generation ───────────────────────────
00273:
00274:         [Fact]
00275:         public void F15_BalanceReport_Generated()
00276:         {
00277:             var stats = _sim.CalculateUtilization();
00278:             var seasonal = _sim.CalculateSeasonalDistribution();
00279:             var (worstDay, maxBlocked, daysOver50, daysZeroOpen) = _sim.NetworkClosureMetrics();
00280:             var weatherFreq = _sim.WeatherFrequency();
00281:
00282:             var sb = new StringBuilder();
00283:             sb.AppendLine("# Weather Gate Balance Audit");
00284:             sb.AppendLine();
00285:             sb.AppendLine($"- Audit seed: {WeatherGateAuditSimulator.AuditSeed}");
00286:             sb.AppendLine($"- Campaign horizon: {WeatherGateAuditSimulator.CampaignDays} days");
00287:             sb.AppendLine($"- Gate count: {stats.Count}");
00288:             sb.AppendLine($"- Source: weather_route_gates.json, weather_seasons.json");
00289:             sb.AppendLine();
00290:
00291:             sb.AppendLine("## Per-Gate Availability");
00292:             sb.AppendLine();
00293:             sb.AppendLine("| Gate ID | Target | Weather | Mode | Blocked % | Open % | Longest Block | Longest Open | Override |");
00294:             sb.AppendLine("|---|---|---|---|---:|---:|---:|---:|---|");
00295:
00296:             foreach (var s in stats)
00297:             {
00298:                 string weather = s.RequiredWeather.Count > 0
00299:                     ? $"requires: {string.Join(", ", s.RequiredWeather)}"
00300:                     : $"blocks: {string.Join(", ", s.BlockedWeather)}";
00301:                 string mode = s.RequiredWeather.Count > 0 ? "positive" : "negative";
00302:                 sb.AppendLine($"| {s.GateId} | {s.Target} | {weather} | {mode} | {s.BlockedPct:F1}% | {s.OpenPct:F1}% | {s.LongestBlockedRun} | {s.LongestOpenRun} | {s.OverrideItem} |");
00303:             }
00304:
00305:             sb.AppendLine();
00306:             sb.AppendLine("## Weather Frequency (360 days)");
00307:             sb.AppendLine();
00308:             sb.AppendLine("| WeatherKind | Days | % |");
00309:             sb.AppendLine("|---|---:|---:|");
00310:             foreach (var kv in weatherFreq.OrderByDescending(kv => kv.Value))
00311:             {
00312:                 double pct = 100.0 * kv.Value / 360;
00313:                 sb.AppendLine($"| {kv.Key} | {kv.Value} | {pct:F1}% |");
00314:             }
00315:
00316:             sb.AppendLine();
00317:             sb.AppendLine("## Network Metrics");
00318:             sb.AppendLine();
00319:             sb.AppendLine($"- Worst day: {worstDay} ({maxBlocked} gates blocked)");
00320:             sb.AppendLine($"- Days with >50% gates blocked: {daysOver50}");
00321:             sb.AppendLine($"- Days with zero open gates: {daysZeroOpen}");
00322:
00323:             sb.AppendLine();
00324:             sb.AppendLine("## Dead Content");
00325:             sb.AppendLine();
00326:             var deadGates = stats.Where(s => s.BlockedDays == 0 && s.BlockedWeather.Count > 0).ToList();
00327:             foreach (var g in deadGates)
00328:             {
00329:                 sb.AppendLine($"- **{g.GateId}** ({g.Target}): blocked by [{string.Join(", ", g.BlockedWeather)}] — zero season weight, never triggers");
00330:             }
00331:
00332:             // The report should be non-trivial
00333:             Assert.True(sb.Length > 500, "Balance report is too short");
00334:         }
00335:     }
00336: }
```
# Appendix M — External verification handoff

The following checks are to be run by the owning integrator after writing: character count, SHA-256 revalidation, path-token resolution, duplicate-heading/unsupported-claim scan, and `git diff --check`. The final ledger entry must report actual results, not this template.
